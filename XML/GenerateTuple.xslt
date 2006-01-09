<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:gnsoc="urn:schemas-neumont-edu:CodeGeneration:GetNodeSetOfCount"
	xmlns:plx="http://schemas.neumont.edu/CodeGeneration/PLiX"
	extension-element-prefixes="gnsoc">

	<xsl:variable name="SpecifyParamNameForArgumentNullException" select="false()"/>
	
	<msxsl:script language="C#" implements-prefix="gnsoc"><![CDATA[
		public XPathNodeIterator GetNodeSetOfCount(double count)
		{
			int iCount = (int)count;
			XmlDocument xmldoc = new XmlDocument();
			xmldoc.AppendChild(xmldoc.CreateElement("Root"));
			for (int i = 0; i < iCount; i++)
			{
				xmldoc.DocumentElement.AppendChild(xmldoc.CreateElement("PlaceHolder"));
			}
			return xmldoc.DocumentElement.CreateNavigator().SelectChildren(XPathNodeType.Element);
		}
	]]></msxsl:script>

	<xsl:template name="GenerateCommonTuples">
		<xsl:call-template name="GenerateTupleBase"/>
		<xsl:variable name="items2">
			<PlaceHolder/>
			<PlaceHolder/>
		</xsl:variable>
		<xsl:call-template name="GenerateTuple">
			<xsl:with-param name="itemsCount" select="msxsl:node-set($items2)/child::*"/>
		</xsl:call-template>
		<xsl:call-template name="GenerateTuple">
			<xsl:with-param name="itemsCount" select="3"/>
		</xsl:call-template>
		<xsl:call-template name="GenerateTuple">
			<xsl:with-param name="itemsCount" select="4"/>
		</xsl:call-template>
		<xsl:call-template name="GenerateTuple">
			<xsl:with-param name="itemsCount" select="5"/>
		</xsl:call-template>
		<xsl:call-template name="GenerateTuple">
			<xsl:with-param name="itemsCount" select="6"/>
		</xsl:call-template>
		<xsl:call-template name="GenerateTuple">
			<xsl:with-param name="itemsCount" select="7"/>
		</xsl:call-template>
		<xsl:call-template name="GenerateTuple">
			<xsl:with-param name="itemsCount" select="8"/>
		</xsl:call-template>
		<xsl:call-template name="GenerateTuple">
			<xsl:with-param name="itemsCount" select="9"/>
		</xsl:call-template>
		<xsl:call-template name="GenerateTuple">
			<xsl:with-param name="itemsCount" select="10"/>
		</xsl:call-template>
	</xsl:template>
	
	<xsl:template name="GenerateTupleBase">
		<plx:class visibility="public" modifier="static" partial="true" name="Tuple">
			<plx:leadingInfo>
				<plx:pragma type="region" data="Tuple Support"/>
			</plx:leadingInfo>
			<plx:trailingInfo>
				<plx:pragma type="closeRegion" data="Tuple Support"/>
			</plx:trailingInfo>
			<plx:function visibility="internal" modifier="static" name="RotateRight">
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
		</plx:class>
	</xsl:template>

	<!--
	NOTE: For a variety of reasons, Tuples are NOT generated as structs (that is, sealed classes that inherit
	from System.ValueType). These reasons include:
		Structs cannot be null without being wrapped as a Nullable<>, and Tuples need to be able to be null-propogating.
		The contents of a struct should NOT be greater than or equal to 16 bytes in size. Since Tuples will usually
			contain references to other objects, this means that Tuples with an arity greater than 3 should be classes
			anyway. (On 64-bit platforms, all Tuples should be classes.)
		Structs can be initialized by the runtime without any constructor being called. (Incidently, this is why some
			CLI languages, including C#, do not allow the user to create a default (i.e. parameter-less) constructor on
			a struct, since in most cases it will never be called.) When this occurs, all bits are set to zero. This means
			that any reference type variables in that struct are null. Part of our Tuple specification is that no part of
			a Tuple can ever be null without the entire Tuple being null. Having Tuples as structs would make this extremely
			difficult to enforce.
	-->

	<xsl:template name="GenerateTuple">
		<!-- itemsCount determines the arity of the Tuple to be created. -->
		<!-- If itemsCount is a node-set, the number of nodes it contains is used as the arity of the Tuple to be created. -->
		<!-- If itemsCount is a number, it is used as the arity of the Tuple to be created.-->
		<!-- BUGBUG: If you pass 2 for itemsCount, bad things happen. Make sure that for a two-Tuple you pass a node-set. -->
		<xsl:param name="itemsCount"/>

		<xsl:variable name="itemsFragment">
			<xsl:choose>
				<xsl:when test="number($itemsCount)">
					<xsl:if test="not(function-available('gnsoc:GetNodeSetOfCount'))">
						<xsl:message terminate="yes">
							<xsl:text>A number was passed for itemsCount, but the GetNodeSetOfCount function is not available.</xsl:text>
						</xsl:message>
					</xsl:if>
					<xsl:for-each select="gnsoc:GetNodeSetOfCount(number($itemsCount))">
						<Item name="Item{position()}" paramName="item{position()}"  dataTypeName="T{position()}"/>
					</xsl:for-each>
				</xsl:when>
				<xsl:otherwise>
					<xsl:for-each select="$itemsCount">
						<Item name="Item{position()}" paramName="item{position()}"  dataTypeName="T{position()}"/>
					</xsl:for-each>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="items" select="msxsl:node-set($itemsFragment)/child::*"/>
		<xsl:variable name="arity" select="count($items)"/>

		<xsl:variable name="paramsFragment">
			<xsl:for-each select="$items">
				<plx:param type="in" name="{@paramName}" dataTypeName="{@dataTypeName}"/>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="params" select="msxsl:node-set($paramsFragment)/child::*"/>

		<xsl:variable name="typeParamsFragment">
			<xsl:for-each select="$items">
				<plx:typeParam name="{@dataTypeName}"/>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="typeParams" select="msxsl:node-set($typeParamsFragment)/child::*"/>

		<xsl:variable name="passTypeParamsFragment">
			<xsl:for-each select="$items">
				<plx:passTypeParam dataTypeName="{@dataTypeName}"/>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="passTypeParams" select="msxsl:node-set($passTypeParamsFragment)/child::*"/>

		<plx:class visibility="public" modifier="static" partial="true" name="Tuple">
			<plx:leadingInfo>
				<plx:pragma type="region" data="{$arity}-ary Tuple"/>
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
				<plx:pragma type="closeRegion" data="{$arity}-ary Tuple"/>
			</plx:trailingInfo>
			<xsl:call-template name="GenerateSuppressMessageAttribute">
				<xsl:with-param name="category" select="'Microsoft.Design'"/>
				<xsl:with-param name="checkId" select="'CA1005'"/>
			</xsl:call-template>
			<plx:attribute dataTypeName="ImmutableObjectAttribute" dataTypeQualifier="System.ComponentModel">
				<plx:passParam>
					<plx:trueKeyword/>
				</plx:passParam>
			</plx:attribute>
			<xsl:copy-of select="$typeParams"/>
			<plx:implementsInterface dataTypeName="IEquatable" dataTypeQualifier="System">
				<plx:passTypeParam dataTypeName="Tuple">
					<xsl:copy-of select="$passTypeParams"/>
				</plx:passTypeParam>
			</plx:implementsInterface>

			<xsl:for-each select="$items">
				<plx:field visibility="public" readOnly="true" name="{@name}" dataTypeName="{@dataTypeName}">
					<xsl:call-template name="GenerateSuppressMessageAttribute">
						<xsl:with-param name="category" select="'Microsoft.Design'"/>
						<xsl:with-param name="checkId" select="'CA1051'"/>
					</xsl:call-template>
					<xsl:call-template name="GenerateSuppressMessageAttribute">
						<xsl:with-param name="category" select="'Microsoft.Security'"/>
						<xsl:with-param name="checkId" select="'CA2104'"/>
					</xsl:call-template>
				</plx:field>
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
							<plx:callThis accessor="this" type="field" name="{@name}"/>
						</plx:left>
						<plx:right>
							<plx:nameRef type="parameter" name="{@paramName}"/>
						</plx:right>
					</plx:assign>
				</xsl:for-each>
			</plx:function>

			<plx:function visibility="public" modifier="override" overload="true" name="Equals">
				<plx:param type="in" name="obj" dataTypeName=".object"/>
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
				<plx:param type="in" name="other" dataTypeName="Tuple">
					<xsl:copy-of select="$passTypeParams"/>
				</plx:param>
				<plx:returns dataTypeName=".boolean"/>
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="booleanOr">
							<plx:left>
								<plx:binaryOperator type="identityEquality">
									<plx:left>
										<plx:nameRef type="parameter" name="other"/>
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
									<plx:callThis accessor="this" type="field" name="{$items[1]/@name}"/>
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
			<plx:function visibility="public" overload="true" name="ToString">
				<plx:param type="in" name="provider" dataTypeName="IFormatProvider" dataTypeQualifier="System"/>
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
								<plx:callThis accessor="this" type="field" name="{@name}"/>
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
						<plx:unaryOperator type="booleanNot">
							<plx:callStatic type="methodCall" dataTypeName=".object" name="ReferenceEquals">
								<plx:passParam>
									<plx:nameRef type="parameter" name="tuple1"/>
								</plx:passParam>
								<plx:passParam>
									<plx:nullKeyword/>
								</plx:passParam>
							</plx:callStatic>
						</plx:unaryOperator>
					</plx:condition>
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
				</plx:branch>
				<plx:return>
					<plx:callStatic type="methodCall" dataTypeName=".object" name="ReferenceEquals">
						<plx:passParam>
							<plx:nameRef type="parameter" name="tuple2"/>
						</plx:passParam>
						<plx:passParam>
							<plx:nullKeyword/>
						</plx:passParam>
					</plx:callStatic>
				</plx:return>
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

		</plx:class>
	</xsl:template>

	<xsl:template name="GetCompoundCode">
		<xsl:param name="items"/>
		<xsl:param name="countItems"/>
		<xsl:param name="currentPosition"/>
		<xsl:param name="codeChoice"/>
		<xsl:param name="operator"/>

		<xsl:variable name="chosenCode">
			<xsl:variable name="currentItem" select="$items[$currentPosition]"/>
			<xsl:choose>
				<xsl:when test="$codeChoice='rotateCode'">
					<plx:callStatic type="methodCall" name="RotateRight" dataTypeName="Tuple">
						<plx:passParam>
							<plx:callInstance type="methodCall" name="GetHashCode">
								<plx:callObject>
									<plx:callThis accessor="this" type="field" name="{$currentItem/@name}"/>
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
								<plx:callThis accessor="this" type="field" name="{$currentItem/@name}"/>
							</plx:callObject>
							<plx:passParam>
								<plx:callInstance type="field" name="{$currentItem/@name}">
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
