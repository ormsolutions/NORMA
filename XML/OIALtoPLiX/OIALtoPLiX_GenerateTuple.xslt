<?xml version="1.0" encoding="utf-8"?>
<!--
	Copyright © Neumont University. All rights reserved.

	This software is provided 'as-is', without any express or implied warranty. In no event will the authors be held liable for any damages arising from the use of this software.
	Permission is granted to anyone to use this software for any purpose, including commercial applications, and to alter it and redistribute it freely, subject to the following restrictions:
	1. The origin of this software must not be misrepresented; you must not claim that you wrote the original software. If you use this software in a product, an acknowledgment in the product documentation would be appreciated but is not required.
	2. Altered source versions must be plainly marked as such, and must not be misrepresented as being the original software.
	3. This notice may not be removed or altered from any source distribution.
-->
<!-- Contributors: Kevin M. Owen -->
<!--
	NOTE:
	The code for the Tuple classes that this transform generates was based heavily on the Tuple class from
	ECMA Technical Report TR/89 "Common Language Infrastructure (CLI) - Common Generics" (available from
	http://www.ecma-international.org/publications/techreports/E-TR-089.htm) and the reference implementation
	of said technical report (available from http://www.mondrian-script.org/ecma).
-->
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:exsl="http://exslt.org/common"
	xmlns:plx="http://schemas.neumont.edu/CodeGeneration/PLiX"
	extension-element-prefixes="exsl">

	<xsl:output method="xml" encoding="utf-8" media-type="text/xml" indent="yes"/>
	
	<xsl:param name="SpecifyParamNameForArgumentNullException" select="false()"/>

	<xsl:param name="StructLayoutAttributeFragment">
		<plx:attribute dataTypeName="StructLayoutAttribute" dataTypeQualifier="System.Runtime.InteropServices">
			<plx:passParam>
				<plx:callStatic type="field" name="Auto" dataTypeName="LayoutKind" dataTypeQualifier="System.Runtime.InteropServices"/>
			</plx:passParam>
			<plx:passParam>
				<plx:binaryOperator type="assignNamed">
					<plx:left>
						<plx:nameRef type="namedParameter" name="CharSet"/>
					</plx:left>
					<plx:right>
						<plx:callStatic type="field" name="Auto" dataTypeName="CharSet" dataTypeQualifier="System.Runtime.InteropServices"/>
					</plx:right>
				</plx:binaryOperator>
			</plx:passParam>
		</plx:attribute>
	</xsl:param>
	<xsl:param name="StructLayoutAttribute" select="exsl:node-set($StructLayoutAttributeFragment)/child::*"/>

	<xsl:template name="GetNodeSetOfCount">
		<xsl:param name="count"/>
		<PlaceHolder/>
		<xsl:if test="$count - 1 > 0">
			<xsl:call-template name="GetNodeSetOfCount">
				<xsl:with-param name="count" select="$count - 1"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>

	<xsl:template name="GenerateCommonTuples">
		<xsl:call-template name="GenerateTupleBase"/>
		<xsl:call-template name="GenerateTuple">
			<xsl:with-param name="arityNumber" select="2"/>
		</xsl:call-template>
		<xsl:call-template name="GenerateTuple">
			<xsl:with-param name="arityNumber" select="3"/>
		</xsl:call-template>
		<xsl:call-template name="GenerateTuple">
			<xsl:with-param name="arityNumber" select="4"/>
		</xsl:call-template>
		<xsl:call-template name="GenerateTuple">
			<xsl:with-param name="arityNumber" select="5"/>
		</xsl:call-template>
		<xsl:call-template name="GenerateTuple">
			<xsl:with-param name="arityNumber" select="6"/>
		</xsl:call-template>
		<xsl:call-template name="GenerateTuple">
			<xsl:with-param name="arityNumber" select="7"/>
		</xsl:call-template>
		<xsl:call-template name="GenerateTuple">
			<xsl:with-param name="arityNumber" select="8"/>
		</xsl:call-template>
		<xsl:call-template name="GenerateTuple">
			<xsl:with-param name="arityNumber" select="9"/>
		</xsl:call-template>
		<xsl:call-template name="GenerateTuple">
			<xsl:with-param name="arityNumber" select="10"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template name="GenerateTupleBase">
		<plx:class visibility="public" modifier="abstract" partial="true" name="Tuple">
			<plx:leadingInfo>
				<plx:pragma type="region" data="Tuple Support"/>
			</plx:leadingInfo>
			<plx:trailingInfo>
				<plx:pragma type="closeRegion" data="Tuple Support"/>
			</plx:trailingInfo>
			<plx:attribute dataTypeName="Serializable" dataTypeQualifier="System"/>
			<plx:attribute dataTypeName="ImmutableObjectAttribute" dataTypeQualifier="System.ComponentModel">
				<plx:passParam>
					<plx:trueKeyword/>
				</plx:passParam>
			</plx:attribute>
			<xsl:copy-of select="$StructLayoutAttribute"/>
			<plx:function visibility="protected" name=".construct"/>
			<plx:function visibility="protected" modifier="static" name="RotateRight">
				<!-- Suppress the 'OperationsShouldNotOverflow' FxCop warning -->
				<plx:attribute dataTypeName="SuppressMessageAttribute" dataTypeQualifier="System.Diagnostics.CodeAnalysis">
					<plx:passParam>
						<plx:string>
							<xsl:value-of select="'Microsoft.Usage'"/>
						</plx:string>
					</plx:passParam>
					<plx:passParam>
						<plx:string>
							<xsl:value-of select="'CA2233:OperationsShouldNotOverflow'"/>
						</plx:string>
					</plx:passParam>
				</plx:attribute>
				<plx:param type="in" name="value" dataTypeName=".i4"/>
				<plx:param type="in" name="places" dataTypeName=".i4"/>
				<plx:returns dataTypeName=".i4"/>
				<plx:assign>
					<plx:left>
						<plx:nameRef type="parameter" name="places"/>
					</plx:left>
					<plx:right>
						<plx:binaryOperator type="bitwiseAnd">
							<plx:left>
								<plx:nameRef type="parameter" name="places"/>
							</plx:left>
							<plx:right>
								<plx:value type="hex4" data="1F"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:right>
				</plx:assign>
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="equality">
							<plx:left>
								<plx:nameRef type="parameter" name="places"/>
							</plx:left>
							<plx:right>
								<plx:value type="i4" data="0"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:return>
						<plx:nameRef type="parameter" name="value"/>
					</plx:return>
				</plx:branch>
				<plx:local name="mask" dataTypeName=".i4">
					<plx:initialize>
						<plx:binaryOperator type="shiftRight">
							<plx:left>
								<plx:unaryOperator type="bitwiseNot">
									<plx:value type="hex4" data="7FFFFFF"/>
								</plx:unaryOperator>
							</plx:left>
							<plx:right>
								<plx:binaryOperator type="subtract">
									<plx:left>
										<plx:nameRef type="parameter" name="places"/>
									</plx:left>
									<plx:right>
										<plx:value type="i4" data="1"/>
									</plx:right>
								</plx:binaryOperator>
							</plx:right>
						</plx:binaryOperator>
					</plx:initialize>
				</plx:local>
				<plx:return>
					<plx:binaryOperator type="bitwiseOr">
						<plx:left>
							<plx:binaryOperator type="bitwiseAnd">
								<plx:left>
									<plx:binaryOperator type="shiftRight">
										<plx:left>
											<plx:nameRef type="parameter" name="value"/>
										</plx:left>
										<plx:right>
											<plx:nameRef type="parameter" name="places"/>
										</plx:right>
									</plx:binaryOperator>
								</plx:left>
								<plx:right>
									<plx:unaryOperator type="bitwiseNot">
										<plx:nameRef type="local" name="mask"/>
									</plx:unaryOperator>
								</plx:right>
							</plx:binaryOperator>
						</plx:left>
						<plx:right>
							<plx:binaryOperator type="bitwiseAnd">
								<plx:left>
									<plx:binaryOperator type="shiftLeft">
										<plx:left>
											<plx:nameRef type="parameter" name="value"/>
										</plx:left>
										<plx:right>
											<plx:binaryOperator type="subtract">
												<plx:left>
													<plx:value type="i4" data="32"/>
												</plx:left>
												<plx:right>
													<plx:nameRef type="parameter" name="places"/>
												</plx:right>
											</plx:binaryOperator>
										</plx:right>
									</plx:binaryOperator>
								</plx:left>
								<plx:right>
									<plx:nameRef type="local" name="mask"/>
								</plx:right>
							</plx:binaryOperator>
						</plx:right>
					</plx:binaryOperator>
				</plx:return>
			</plx:function>
			<plx:function visibility="public" modifier="abstractOverride" overload="true" name="ToString">
				<plx:returns dataTypeName=".string"/>
			</plx:function>
			<plx:function visibility="public" modifier="abstract" overload="true" name="ToString">
				<plx:param name="provider" dataTypeName="IFormatProvider" dataTypeQualifier="System"/>
				<plx:returns dataTypeName=".string"/>
			</plx:function>
		</plx:class>
	</xsl:template>

	<!--
	NOTE: For a variety of reasons, Tuples are NOT generated as structs (that is, sealed classes that inherit
	from System.ValueType). These reasons include:
		Structs cannot be null without being wrapped as a Nullable<>, and Tuples need to be able to be null-propagating.
		The contents of a struct should generally not be greater than or equal to 16 bytes in size. Since Tuples will
			usually contain references to other objects, this means that Tuples with an arity greater than 3 should be
			classes anyway. (On 64-bit platforms, all Tuples should be classes.)
		Structs can be initialized by the runtime without any constructor being called. (Incidently, this is why some
			CLI languages, including C#, do not allow the user to create a default (i.e. parameter-less) constructor on
			a struct, since in most cases it will never be called.) When this occurs, all bits are set to zero. This means
			that any reference type variables in that struct are null. Part of our Tuple specification is that no part of
			a Tuple can ever be null without the entire Tuple being null. Having Tuples as structs would make this extremely
			difficult to enforce.
	-->

	<xsl:template name="GenerateTuple">
		<!-- arityNodeSet or arityNumber determines the arity of the Tuple to be created. -->
		<!-- If arityNodeSet is specified, the number of nodes it contains is used as the arity of the Tuple to be created. -->
		<!-- If arityNumber is specified, it is used as the arity of the Tuple to be created.-->
		<xsl:param name="arityNodeSet"/>
		<xsl:param name="arityNumber"/>

		<xsl:variable name="itemsFragment">
			<xsl:choose>
				<xsl:when test="($arityNodeSet and $arityNumber) or (not($arityNodeSet) and not($arityNumber))">
					<xsl:message terminate="yes">
						<xsl:text>Exactly one of the arityNodeSet and arityNumber parameters must be specified.</xsl:text>
					</xsl:message>
				</xsl:when>
				<xsl:when test="$arityNumber">
					<xsl:variable name="tempNodeSetsFragment">
						<xsl:call-template name="GetNodeSetOfCount">
							<xsl:with-param name="count" select="$arityNumber"/>
						</xsl:call-template>
					</xsl:variable>
					<xsl:for-each select="exsl:node-set($tempNodeSetsFragment)/child::*">
						<Item name="Item{position()}" fieldName="_item{position()}" paramName="item{position()}" dataTypeName="T{position()}"/>
					</xsl:for-each>
				</xsl:when>
				<xsl:otherwise>
					<xsl:for-each select="exsl:node-set($arityNodeSet)/child::*">
						<Item name="Item{position()}" fieldName="_item{position()}" paramName="item{position()}" dataTypeName="T{position()}"/>
					</xsl:for-each>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="items" select="exsl:node-set($itemsFragment)/child::*"/>
		<xsl:variable name="arity" select="count($items)"/>
		<xsl:if test="$arity &lt; 2">
			<xsl:message terminate="yes">
				<xsl:text>A Tuple cannot have an arity less than two.</xsl:text>
			</xsl:message>
		</xsl:if>

		<xsl:variable name="arityText">
			<xsl:variable name="arityName">
				<xsl:choose>
					<xsl:when test="$arity=2">
						<xsl:value-of select="'Binary'"/>
					</xsl:when>
					<xsl:when test="$arity=3">
						<xsl:value-of select="'Ternary'"/>
					</xsl:when>
					<xsl:when test="$arity=4">
						<xsl:value-of select="'Quaternary'"/>
					</xsl:when>
					<xsl:when test="$arity=5">
						<xsl:value-of select="'Quinary'"/>
					</xsl:when>
					<xsl:when test="$arity=6">
						<xsl:value-of select="'Senary'"/>
					</xsl:when>
					<xsl:when test="$arity=7">
						<xsl:value-of select="'Septenary'"/>
					</xsl:when>
					<xsl:when test="$arity=8">
						<xsl:value-of select="'Octonary'"/>
					</xsl:when>
					<xsl:when test="$arity=9">
						<xsl:value-of select="'Nonary'"/>
					</xsl:when>
					<xsl:when test="$arity=10">
						<xsl:value-of select="'Denary'"/>
					</xsl:when>
					<xsl:when test="$arity=11">
						<xsl:value-of select="'Undenary'"/>
					</xsl:when>
					<xsl:when test="$arity=12">
						<xsl:value-of select="'Duodenary'"/>
					</xsl:when>
					<xsl:when test="$arity=20">
						<xsl:value-of select="'Vigesary'"/>
					</xsl:when>
					<xsl:when test="$arity=100">
						<xsl:value-of select="'Centenary'"/>
					</xsl:when>
					<xsl:when test="$arity=1000">
						<xsl:value-of select="'Millenary'"/>
					</xsl:when>
				</xsl:choose>
			</xsl:variable>
			<xsl:choose>
				<xsl:when test="$arityName">
					<xsl:value-of select="concat($arityName,' (',$arity,'-ary)')"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="concat($arity,'-ary')"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		
		<xsl:variable name="paramsFragment">
			<xsl:for-each select="$items">
				<plx:param name="{@paramName}" dataTypeName="{@dataTypeName}"/>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="params" select="exsl:node-set($paramsFragment)/child::*"/>
		
		<xsl:variable name="typeParamsFragment">
			<xsl:for-each select="$items">
				<plx:typeParam name="{@dataTypeName}"/>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="typeParams" select="exsl:node-set($typeParamsFragment)/child::*"/>

		<xsl:variable name="passTypeParamsFragment">
			<xsl:for-each select="$items">
				<plx:passTypeParam dataTypeName="{@dataTypeName}"/>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="passTypeParams" select="exsl:node-set($passTypeParamsFragment)/child::*"/>

		<plx:class visibility="public" modifier="abstract" partial="true" name="Tuple">
			<plx:leadingInfo>
				<plx:pragma type="region" data="{$arityText} Tuple"/>
			</plx:leadingInfo>
			<plx:function visibility="public" modifier="static" overload="true" name="CreateTuple">
				<xsl:copy-of select="$typeParams"/>
				<xsl:copy-of select="$params"/>
				<plx:returns dataTypeName="Tuple">
					<xsl:copy-of select="$passTypeParams"/>
				</plx:returns>
				<plx:branch>
					<plx:condition>
						<xsl:call-template name="GetCompoundCode">
							<xsl:with-param name="items" select="$items"/>
							<xsl:with-param name="countItems" select="$arity"/>
							<xsl:with-param name="currentPosition" select="1"/>
							<xsl:with-param name="codeChoice" select="'checkNullCode'"/>
							<xsl:with-param name="operator" select="'booleanOr'"/>
						</xsl:call-template>
					</plx:condition>
					<plx:return>
						<plx:nullKeyword/>
					</plx:return>
				</plx:branch>
				<plx:return>
					<plx:callNew dataTypeName="Tuple">
						<xsl:copy-of select="$passTypeParams"/>
						<xsl:for-each select="$items">
							<plx:passParam>
								<plx:nameRef type="parameter" name="{@paramName}"/>
							</plx:passParam>
						</xsl:for-each>
					</plx:callNew>
				</plx:return>
			</plx:function>
		</plx:class>

		<plx:class visibility="public" modifier="sealed" name="Tuple">
			<plx:trailingInfo>
				<plx:pragma type="closeRegion" data="{$arityText} Tuple"/>
			</plx:trailingInfo>
			<xsl:if test="$arity > 2">
				<!-- Suppress the 'AvoidExcessiveParametersOnGenericTypes' FxCop warning -->
				<plx:attribute dataTypeName="SuppressMessageAttribute" dataTypeQualifier="System.Diagnostics.CodeAnalysis">
					<plx:passParam>
						<plx:string>
							<xsl:value-of select="'Microsoft.Design'"/>
						</plx:string>
					</plx:passParam>
					<plx:passParam>
						<plx:string>
							<xsl:value-of select="'CA1005:AvoidExcessiveParametersOnGenericTypes'"/>
						</plx:string>
					</plx:passParam>
				</plx:attribute>
			</xsl:if>
			<plx:attribute dataTypeName="Serializable" dataTypeQualifier="System"/>
			<xsl:copy-of select="$StructLayoutAttribute"/>
			<xsl:copy-of select="$typeParams"/>
			<plx:derivesFromClass dataTypeName="Tuple"/>
			<plx:implementsInterface dataTypeName="IEquatable" dataTypeQualifier="System">
				<plx:passTypeParam dataTypeName="Tuple">
					<xsl:copy-of select="$passTypeParams"/>
				</plx:passTypeParam>
			</plx:implementsInterface>

			<xsl:for-each select="$items">
				<plx:field visibility="private" readOnly="true" name="{@fieldName}" dataTypeName="{@dataTypeName}"/>
				<plx:property visibility="public" name="{@name}">
					<plx:returns dataTypeName="{@dataTypeName}"/>
					<plx:get>
						<plx:return>
							<plx:callThis accessor="this" type="field" name="{@fieldName}"/>
						</plx:return>
					</plx:get>
				</plx:property>
			</xsl:for-each>

			<plx:function visibility="public" name=".construct">
				<xsl:copy-of select="$params"/>
				<xsl:choose>
					<xsl:when test="boolean($SpecifyParamNameForArgumentNullException)">
						<xsl:for-each select="$items">
							<plx:branch>
								<plx:condition>
									<plx:binaryOperator type="identityEquality">
										<plx:left>
											<plx:nameRef type="parameter" name="{@paramName}"/>
										</plx:left>
										<plx:right>
											<plx:nullKeyword/>
										</plx:right>
									</plx:binaryOperator>
								</plx:condition>
								<plx:throw>
									<plx:callNew dataTypeName="ArgumentNullException" dataTypeQualifier="System">
										<plx:passParam>
											<plx:string>
												<xsl:value-of select="@paramName"/>
											</plx:string>
										</plx:passParam>
									</plx:callNew>
								</plx:throw>
							</plx:branch>
						</xsl:for-each>
					</xsl:when>
					<xsl:otherwise>
						<plx:branch>
							<plx:condition>
								<xsl:call-template name="GetCompoundCode">
									<xsl:with-param name="items" select="$items"/>
									<xsl:with-param name="countItems" select="$arity"/>
									<xsl:with-param name="currentPosition" select="1"/>
									<xsl:with-param name="codeChoice" select="'checkNullCode'"/>
									<xsl:with-param name="operator" select="'booleanOr'"/>
								</xsl:call-template>
							</plx:condition>
							<plx:throw>
								<plx:callNew dataTypeName="ArgumentNullException" dataTypeQualifier="System"/>
							</plx:throw>
						</plx:branch>
					</xsl:otherwise>
				</xsl:choose>
				<xsl:for-each select="$items">
					<plx:assign>
						<plx:left>
							<plx:callThis accessor="this" type="field" name="{@fieldName}"/>
						</plx:left>
						<plx:right>
							<plx:nameRef type="parameter" name="{@paramName}"/>
						</plx:right>
					</plx:assign>
				</xsl:for-each>
			</plx:function>

			<plx:function visibility="public" modifier="override" overload="true" name="Equals">
				<plx:param name="obj" dataTypeName=".object"/>
				<plx:returns dataTypeName=".boolean"/>
				<plx:return>
					<plx:callThis accessor="this" type="methodCall" name="Equals">
						<plx:passParam>
							<plx:cast type="testCast" dataTypeName="Tuple">
								<xsl:copy-of select="$passTypeParams"/>
								<plx:nameRef type="parameter" name="obj"/>
							</plx:cast>
						</plx:passParam>
					</plx:callThis>
				</plx:return>
			</plx:function>

			<plx:function visibility="public" overload="true" name="Equals">
				<plx:interfaceMember memberName="Equals" dataTypeName="IEquatable" dataTypeQualifier="System">
					<plx:passTypeParam dataTypeName="Tuple">
						<xsl:copy-of select="$passTypeParams"/>
					</plx:passTypeParam>
				</plx:interfaceMember>
				<plx:param name="other" dataTypeName="Tuple">
					<xsl:copy-of select="$passTypeParams"/>
				</plx:param>
				<plx:returns dataTypeName=".boolean"/>
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="booleanOr">
							<plx:left>
								<plx:binaryOperator type="identityEquality">
									<plx:left>
										<plx:cast type="exceptionCast" dataTypeName=".object">
											<plx:nameRef type="parameter" name="other"/>
										</plx:cast>
									</plx:left>
									<plx:right>
										<plx:nullKeyword/>
									</plx:right>
								</plx:binaryOperator>
							</plx:left>
							<plx:right>
								<xsl:call-template name="GetCompoundCode">
									<xsl:with-param name="items" select="$items"/>
									<xsl:with-param name="countItems" select="$arity"/>
									<xsl:with-param name="currentPosition" select="1"/>
									<xsl:with-param name="codeChoice" select="'checkEqualityCode'"/>
									<xsl:with-param name="operator" select="'booleanOr'"/>
								</xsl:call-template>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:return>
						<plx:falseKeyword/>
					</plx:return>
				</plx:branch>
				<plx:return>
					<plx:trueKeyword/>
				</plx:return>
			</plx:function>

			<plx:function visibility="public" modifier="override" name="GetHashCode">
				<plx:returns dataTypeName=".i4"/>
				<plx:return>
					<plx:binaryOperator type="bitwiseExclusiveOr">
						<plx:left>
							<plx:callInstance type="methodCall" name="GetHashCode">
								<plx:callObject>
									<plx:callThis accessor="this" type="field" name="{$items[1]/@fieldName}"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:left>
						<plx:right>
							<xsl:call-template name="GetCompoundCode">
								<xsl:with-param name="items" select="$items"/>
								<xsl:with-param name="countItems" select="$arity"/>
								<xsl:with-param name="currentPosition" select="2"/>
								<xsl:with-param name="codeChoice" select="'rotateCode'"/>
								<xsl:with-param name="operator" select="'bitwiseExclusiveOr'"/>
							</xsl:call-template>
						</plx:right>
					</plx:binaryOperator>
				</plx:return>
			</plx:function>

			<plx:function visibility="public" modifier="override" overload="true" name="ToString">
				<plx:returns dataTypeName=".string"/>
				<plx:return>
					<plx:callThis accessor="this" type="methodCall" name="ToString">
						<plx:passParam>
							<plx:nullKeyword/>
						</plx:passParam>
					</plx:callThis>
				</plx:return>
			</plx:function>
			<plx:function visibility="public" modifier="override" overload="true" name="ToString">
				<plx:param name="provider" dataTypeName="IFormatProvider" dataTypeQualifier="System"/>
				<plx:returns dataTypeName=".string"/>
				<plx:return>
					<plx:callStatic type="methodCall" name="Format" dataTypeName=".string">
						<plx:passParam>
							<plx:nameRef type="parameter" name="provider"/>
						</plx:passParam>
						<plx:passParam>
							<plx:string>
								<xsl:text>(</xsl:text>
								<xsl:for-each select="$items">
									<xsl:value-of select="concat('{',position(),'}')"/>
									<xsl:if test="not(position()=last())">
										<xsl:text>, </xsl:text>
									</xsl:if>
								</xsl:for-each>
								<xsl:text>)</xsl:text>
							</plx:string>
						</plx:passParam>
						<xsl:for-each select="$items">
							<plx:passParam>
								<plx:callThis accessor="this" type="field" name="{@fieldName}"/>
							</plx:passParam>
						</xsl:for-each>
					</plx:callStatic>
				</plx:return>
			</plx:function>

			<plx:operatorFunction type="equality">
				<plx:param type="in" name="tuple1" dataTypeName="Tuple">
					<xsl:copy-of select="$passTypeParams"/>
				</plx:param>
				<plx:param type="in" name="tuple2" dataTypeName="Tuple">
					<xsl:copy-of select="$passTypeParams"/>
				</plx:param>
				<plx:returns dataTypeName=".boolean"/>
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="identityEquality">
							<plx:left>
								<plx:cast type="exceptionCast" dataTypeName=".object">
									<plx:nameRef type="parameter" name="tuple1"/>
								</plx:cast>
							</plx:left>
							<plx:right>
								<plx:nullKeyword/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:return>
						<plx:binaryOperator type="identityEquality">
							<plx:left>
								<plx:cast type="exceptionCast" dataTypeName=".object">
									<plx:nameRef type="parameter" name="tuple2"/>
								</plx:cast>
							</plx:left>
							<plx:right>
								<plx:nullKeyword/>
							</plx:right>
						</plx:binaryOperator>
					</plx:return>
				</plx:branch>
				<plx:fallbackBranch>
					<plx:return>
						<plx:callInstance type="methodCall" name="Equals">
							<plx:callObject>
								<plx:nameRef type="parameter" name="tuple1"/>
							</plx:callObject>
							<plx:passParam>
								<plx:nameRef type="parameter" name="tuple2"/>
							</plx:passParam>
						</plx:callInstance>
					</plx:return>
				</plx:fallbackBranch>
			</plx:operatorFunction>

			<plx:operatorFunction type="inequality">
				<plx:param type="in" name="tuple1" dataTypeName="Tuple">
					<xsl:copy-of select="$passTypeParams"/>
				</plx:param>
				<plx:param type="in" name="tuple2" dataTypeName="Tuple">
					<xsl:copy-of select="$passTypeParams"/>
				</plx:param>
				<plx:returns dataTypeName=".boolean"/>
				<plx:return>
					<plx:unaryOperator type="booleanNot">
						<plx:binaryOperator type="equality">
							<plx:left>
								<plx:nameRef type="parameter" name="tuple1"/>
							</plx:left>
							<plx:right>
								<plx:nameRef type="parameter" name="tuple2"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:unaryOperator>
				</plx:return>
			</plx:operatorFunction>

			<xsl:if test="$arity=2">
				<plx:operatorFunction overload="true" type="castWiden">
					<plx:param name="tuple" dataTypeName="Tuple">
						<xsl:copy-of select="$passTypeParams"/>
					</plx:param>
					<plx:returns dataTypeName="KeyValuePair" dataTypeQualifier="System.Collections.Generic">
						<xsl:copy-of select="$passTypeParams"/>
					</plx:returns>
					<plx:branch>
						<plx:condition>
							<plx:binaryOperator type="identityEquality">
								<plx:left>
									<plx:cast type="exceptionCast" dataTypeName=".object">
										<plx:nameRef type="parameter" name="tuple"/>
									</plx:cast>
								</plx:left>
								<plx:right>
									<plx:nullKeyword/>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:return>
							<plx:defaultValueOf dataTypeName="KeyValuePair" dataTypeQualifier="System.Collections.Generic">
								<xsl:copy-of select="$passTypeParams"/>
							</plx:defaultValueOf>
						</plx:return>
					</plx:branch>
					<plx:fallbackBranch>
						<plx:return>
							<plx:callNew dataTypeName="KeyValuePair" dataTypeQualifier="System.Collections.Generic">
								<xsl:copy-of select="$passTypeParams"/>
								<xsl:for-each select="$items">
									<plx:passParam>
										<plx:callInstance type="field" name="{@fieldName}">
											<plx:callObject>
												<plx:nameRef type="parameter" name="tuple"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:passParam>
								</xsl:for-each>
							</plx:callNew>
						</plx:return>
					</plx:fallbackBranch>
				</plx:operatorFunction>
				<plx:operatorFunction overload="true" type="castNarrow">
					<plx:param name="keyValuePair" dataTypeName="KeyValuePair" dataTypeQualifier="System.Collections.Generic">
						<xsl:copy-of select="$passTypeParams"/>
					</plx:param>
					<plx:returns dataTypeName="Tuple">
						<xsl:copy-of select="$passTypeParams"/>
					</plx:returns>
					<plx:branch>
						<plx:condition>
							<plx:binaryOperator type="booleanOr">
								<plx:left>
									<plx:binaryOperator type="identityEquality">
										<plx:left>
											<plx:callInstance type="property" name="Key">
												<plx:callObject>
													<plx:nameRef type="parameter" name="keyValuePair"/>
												</plx:callObject>
											</plx:callInstance>
										</plx:left>
										<plx:right>
											<plx:nullKeyword/>
										</plx:right>
									</plx:binaryOperator>
								</plx:left>
								<plx:right>
									<plx:binaryOperator type="identityEquality">
										<plx:left>
											<plx:callInstance type="property" name="Value">
												<plx:callObject>
													<plx:nameRef type="parameter" name="keyValuePair"/>
												</plx:callObject>
											</plx:callInstance>
										</plx:left>
										<plx:right>
											<plx:nullKeyword/>
										</plx:right>
									</plx:binaryOperator>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:throw>
							<plx:callNew dataTypeName="InvalidCastException" dataTypeQualifier="System"/>
						</plx:throw>
					</plx:branch>
					<plx:fallbackBranch>
						<plx:return>
							<plx:callNew dataTypeName="Tuple">
								<xsl:copy-of select="$passTypeParams"/>
								<plx:passParam>
									<plx:callInstance type="property" name="Key">
										<plx:callObject>
											<plx:nameRef type="parameter" name="keyValuePair"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:passParam>
								<plx:passParam>
									<plx:callInstance type="property" name="Value">
										<plx:callObject>
											<plx:nameRef type="parameter" name="keyValuePair"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:passParam>
							</plx:callNew>
						</plx:return>
					</plx:fallbackBranch>
				</plx:operatorFunction>
				<plx:operatorFunction overload="true" type="castWiden">
					<plx:param name="tuple" dataTypeName="Tuple">
						<xsl:copy-of select="$passTypeParams"/>
					</plx:param>
					<plx:returns dataTypeName="DictionaryEntry" dataTypeQualifier="System.Collections"/>
					<plx:branch>
						<plx:condition>
							<plx:binaryOperator type="identityEquality">
								<plx:left>
									<plx:cast type="exceptionCast" dataTypeName=".object">
										<plx:nameRef type="parameter" name="tuple"/>
									</plx:cast>
								</plx:left>
								<plx:right>
									<plx:nullKeyword/>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:return>
							<plx:defaultValueOf dataTypeName="DictionaryEntry" dataTypeQualifier="System.Collections"/>
						</plx:return>
					</plx:branch>
					<plx:fallbackBranch>
						<plx:return>
							<plx:callNew dataTypeName="DictionaryEntry" dataTypeQualifier="System.Collections">
								<xsl:for-each select="$items">
									<plx:passParam>
										<plx:callInstance type="field" name="{@fieldName}">
											<plx:callObject>
												<plx:nameRef type="parameter" name="tuple"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:passParam>
								</xsl:for-each>
							</plx:callNew>
						</plx:return>
					</plx:fallbackBranch>
				</plx:operatorFunction>
				<plx:operatorFunction overload="true" type="castNarrow">
					<plx:param name="dictionaryEntry" dataTypeName="DictionaryEntry" dataTypeQualifier="System.Collections"/>
					<plx:returns dataTypeName="Tuple">
						<xsl:copy-of select="$passTypeParams"/>
					</plx:returns>
					<plx:local name="key" dataTypeName=".object">
						<plx:initialize>
							<plx:callInstance type="property" name="Key">
								<plx:callObject>
									<plx:nameRef type="parameter" name="dictionaryEntry"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:initialize>
					</plx:local>
					<plx:local name="value" dataTypeName=".object">
						<plx:initialize>
							<plx:callInstance type="property" name="Value">
								<plx:callObject>
									<plx:nameRef type="parameter" name="dictionaryEntry"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:initialize>
					</plx:local>
					<plx:branch>
						<plx:condition>
							<plx:binaryOperator type="booleanOr">
								<plx:left>
									<plx:binaryOperator type="booleanOr">
										<plx:left>
											<plx:binaryOperator type="identityEquality">
												<plx:left>
													<plx:nameRef type="local" name="key"/>
												</plx:left>
												<plx:right>
													<plx:nullKeyword/>
												</plx:right>
											</plx:binaryOperator>
										</plx:left>
										<plx:right>
											<plx:binaryOperator type="identityEquality">
												<plx:left>
													<plx:nameRef type="local" name="value"/>
												</plx:left>
												<plx:right>
													<plx:nullKeyword/>
												</plx:right>
											</plx:binaryOperator>
										</plx:right>
									</plx:binaryOperator>
								</plx:left>
								<plx:right>
									<plx:unaryOperator type="booleanNot">
										<plx:binaryOperator type="booleanAnd">
											<plx:left>
												<plx:binaryOperator type="typeEquality">
													<plx:left>
														<plx:nameRef type="local" name="key"/>
													</plx:left>
													<plx:right>
														<plx:directTypeReference dataTypeName="T1"/>
													</plx:right>
												</plx:binaryOperator>
											</plx:left>
											<plx:right>
												<plx:binaryOperator type="typeEquality">
													<plx:left>
														<plx:nameRef type="local" name="value"/>
													</plx:left>
													<plx:right>
														<plx:directTypeReference dataTypeName="T2"/>
													</plx:right>
												</plx:binaryOperator>
											</plx:right>
										</plx:binaryOperator>
									</plx:unaryOperator>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:throw>
							<plx:callNew dataTypeName="InvalidCastException" dataTypeQualifier="System"/>
						</plx:throw>
					</plx:branch>
					<plx:fallbackBranch>
						<plx:return>
							<plx:callNew dataTypeName="Tuple">
								<xsl:copy-of select="$passTypeParams"/>
								<plx:passParam>
									<plx:cast type="exceptionCast" dataTypeName="T1">
										<plx:nameRef type="local" name="key"/>
									</plx:cast>
								</plx:passParam>
								<plx:passParam>
									<plx:cast type="exceptionCast" dataTypeName="T2">
										<plx:nameRef type="local" name="value"/>
									</plx:cast>
								</plx:passParam>
							</plx:callNew>
						</plx:return>
					</plx:fallbackBranch>
				</plx:operatorFunction>
			</xsl:if>
			
		</plx:class>
	</xsl:template>

	<xsl:template name="GetCompoundCode">
		<xsl:param name="items"/>
		<xsl:param name="countItems"/>
		<xsl:param name="currentPosition"/>
		<xsl:param name="codeChoice"/>
		<xsl:param name="operator"/>

		<xsl:variable name="chosenCodeFragment">
			<xsl:variable name="currentItem" select="$items[$currentPosition]"/>
			<xsl:choose>
				<xsl:when test="$codeChoice='rotateCode'">
					<plx:callStatic type="methodCall" name="RotateRight" dataTypeName="Tuple">
						<plx:passParam>
							<plx:callInstance type="methodCall" name="GetHashCode">
								<plx:callObject>
									<plx:callThis accessor="this" type="field" name="{$currentItem/@fieldName}"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:passParam>
						<plx:passParam>
							<plx:value type="i4" data="{$currentPosition - 1}"/>
						</plx:passParam>
					</plx:callStatic>
				</xsl:when>
				<xsl:when test="$codeChoice='checkNullCode'">
					<plx:binaryOperator type="identityEquality">
						<plx:left>
							<plx:nameRef type="parameter" name="{$currentItem/@paramName}"/>
						</plx:left>
						<plx:right>
							<plx:nullKeyword/>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$codeChoice='checkEqualityCode'">
					<plx:unaryOperator type="booleanNot">
						<plx:callInstance type="methodCall" name="Equals">
							<plx:callObject>
								<plx:callThis accessor="this" type="field" name="{$currentItem/@fieldName}"/>
							</plx:callObject>
							<plx:passParam>
								<plx:callInstance type="field" name="{$currentItem/@fieldName}">
									<plx:callObject>
										<plx:nameRef type="parameter" name="other"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:passParam>
						</plx:callInstance>
					</plx:unaryOperator>
				</xsl:when>
				<xsl:otherwise>
					<xsl:message terminate="yes">
						<xsl:text>An unrecognized value was specified for codeChoice.</xsl:text>
					</xsl:message>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="chosenCode" select="exsl:node-set($chosenCodeFragment)/child::*"/>

		<xsl:choose>
			<xsl:when test="not($currentPosition=$countItems)">
				<plx:binaryOperator type="{$operator}">
					<plx:left>
						<xsl:copy-of select="$chosenCode"/>
					</plx:left>
					<plx:right>
						<xsl:call-template name="GetCompoundCode">
							<xsl:with-param name="items" select="$items"/>
							<xsl:with-param name="countItems" select="$countItems"/>
							<xsl:with-param name="currentPosition" select="$currentPosition + 1"/>
							<xsl:with-param name="codeChoice" select="$codeChoice"/>
							<xsl:with-param name="operator" select="$operator"/>
						</xsl:call-template>
					</plx:right>
				</plx:binaryOperator>
			</xsl:when>
			<xsl:otherwise>
				<xsl:copy-of select="$chosenCode"/>
			</xsl:otherwise>
		</xsl:choose>

	</xsl:template>

</xsl:stylesheet>
