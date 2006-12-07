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
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:plx="http://schemas.neumont.edu/CodeGeneration/PLiX"
	xmlns:exsl="http://exslt.org/common"
	version="1.0"
	extension-element-prefixes="exsl">

	<xsl:output method="xml" encoding="utf-8" indent="no" media-type="text/xml"/>

	<xsl:template match="ModelingEventManager">
		<plx:root xmlns:plx="http://schemas.neumont.edu/CodeGeneration/PLiX">
			<plx:namespaceImport name="System"/>
			<plx:namespaceImport name="System.Collections.Generic"/>
			<plx:namespaceImport name="System.ComponentModel"/>
			<plx:namespaceImport name="Microsoft.VisualStudio.Modeling"/>
			<plx:namespace name="Neumont.Tools.Modeling">
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

				<plx:class name="ModelingEventManager" partial="true" visibility="deferToPartial">
					<xsl:apply-templates select="ModelEvent | ElementEvent"/>
				</plx:class>

			</plx:namespace>
		</plx:root>
	</xsl:template>

	<xsl:template match="ModelEvent | ElementEvent">
		<xsl:variable name="pascalName" select="string(@name)"/>
		<xsl:variable name="camelName" select="concat(translate(substring($pascalName,1,1),'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'),substring($pascalName,2))"/>
		<xsl:variable name="eventArgsName">
			<xsl:choose>
				<xsl:when test="string-length(@eventArgsName)">
					<xsl:value-of select="@eventArgsName"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="concat($pascalName,'EventArgs')"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="addRemoveNameDecorator">
			<xsl:if test="$eventArgsName='EventArgs'">
				<xsl:value-of select="$pascalName"/>
			</xsl:if>
		</xsl:variable>
		<xsl:variable name="needsGuidPairDictionary" select="boolean(self::ElementEvent and child::GuidPair)"/>

		<xsl:variable name="eventHandlerPassTypeParamFragment">
			<plx:passTypeParam dataTypeName="EventHandler">
				<plx:passTypeParam dataTypeName="{$eventArgsName}"/>
			</plx:passTypeParam>
		</xsl:variable>
		<xsl:variable name="eventHandlerPassTypeParam" select="exsl:node-set($eventHandlerPassTypeParamFragment)/child::*"/>

		<plx:field name="_{$camelName}" dataTypeName="List" visibility="private">
			<xsl:copy-of select="$eventHandlerPassTypeParam"/>
			<plx:leadingInfo>
				<plx:pragma type="region" data="{$pascalName} support"/>
			</plx:leadingInfo>
		</plx:field>

		<xsl:if test="self::ElementEvent">
			<plx:field name="_{$camelName}Dictionary" dataTypeName="Dictionary" visibility="private">
				<plx:passTypeParam dataTypeName="Guid"/>
				<plx:passTypeParam dataTypeName="List">
					<xsl:copy-of select="$eventHandlerPassTypeParam"/>
				</plx:passTypeParam>
			</plx:field>

			<xsl:if test="$needsGuidPairDictionary">
				<plx:field name="_{$camelName}GuidPairDictionary" dataTypeName="Dictionary" visibility="private">
					<plx:passTypeParam dataTypeName="GuidPair"/>
					<plx:passTypeParam dataTypeName="List">
						<xsl:copy-of select="$eventHandlerPassTypeParam"/>
					</plx:passTypeParam>
				</plx:field>
			</xsl:if>
		</xsl:if>

		<plx:function name="Get{$pascalName}Handlers" visibility="private">
			<plx:param name="action" dataTypeName="EventHandlerAction"/>
			<plx:returns dataTypeName="List">
				<xsl:copy-of select="$eventHandlerPassTypeParam"/>
			</plx:returns>
			<plx:local name="{$camelName}" dataTypeName="List">
				<xsl:copy-of select="$eventHandlerPassTypeParam"/>
				<plx:initialize>
					<plx:callThis name="_{$camelName}" type="field"/>
				</plx:initialize>
			</plx:local>
			<plx:branch>
				<plx:condition>
					<plx:binaryOperator type="booleanAnd">
						<plx:left>
							<plx:binaryOperator type="identityEquality">
								<plx:left>
									<plx:nameRef name="{$camelName}" type="local"/>
								</plx:left>
								<plx:right>
									<plx:nullKeyword/>
								</plx:right>
							</plx:binaryOperator>
						</plx:left>
						<plx:right>
							<plx:binaryOperator type="equality">
								<plx:left>
									<plx:nameRef name="action" type="parameter"/>
								</plx:left>
								<plx:right>
									<plx:callStatic name="Add" type="field" dataTypeName="EventHandlerAction"/>
								</plx:right>
							</plx:binaryOperator>
						</plx:right>
					</plx:binaryOperator>
				</plx:condition>
				<xsl:choose>
					<xsl:when test="self::ElementEvent">
						<plx:callThis name="Initialize{$pascalName}HandlersStorage" type="methodCall"/>
						<plx:assign>
							<plx:left>
								<plx:nameRef name="{$camelName}" type="local"/>
							</plx:left>
							<plx:right>
								<plx:callThis name="_{$camelName}" type="field"/>
							</plx:right>
						</plx:assign>
					</xsl:when>
					<xsl:otherwise>
						<plx:lock>
							<plx:initialize>
								<plx:callThis name="_lockObject" type="field"/>
							</plx:initialize>
							<plx:branch>
								<plx:condition>
									<plx:binaryOperator type="identityEquality">
										<plx:left>
											<plx:expression parens="true">
												<plx:inlineStatement dataTypeName="List">
													<xsl:copy-of select="$eventHandlerPassTypeParam"/>
													<plx:assign>
														<plx:left>
															<plx:nameRef name="{$camelName}" type="local"/>
														</plx:left>
														<plx:right>
															<plx:callThis name="_{$camelName}" type="field"/>
														</plx:right>
													</plx:assign>
												</plx:inlineStatement>
											</plx:expression>
										</plx:left>
										<plx:right>
											<plx:nullKeyword/>
										</plx:right>
									</plx:binaryOperator>
								</plx:condition>
								<plx:assign>
									<plx:left>
										<plx:callThis name="_{$camelName}" type="field"/>
									</plx:left>
									<plx:right>
										<plx:inlineStatement dataTypeName="List">
											<xsl:copy-of select="$eventHandlerPassTypeParam"/>
											<plx:assign>
												<plx:left>
													<plx:nameRef name="{$camelName}" type="local"/>
												</plx:left>
												<plx:right>
													<plx:callNew dataTypeName="List">
														<xsl:copy-of select="$eventHandlerPassTypeParam"/>
													</plx:callNew>
												</plx:right>
											</plx:assign>
										</plx:inlineStatement>
									</plx:right>
								</plx:assign>
								<xsl:call-template name="GetAttachHandlerCode">
									<xsl:with-param name="IsEvent" select="@isEvent='true'"/>
									<xsl:with-param name="PascalName" select="$pascalName"/>
									<xsl:with-param name="EventArgsName" select="$eventArgsName"/>
								</xsl:call-template>
							</plx:branch>
						</plx:lock>
					</xsl:otherwise>
				</xsl:choose>
			</plx:branch>
			<plx:return>
				<plx:nameRef name="{$camelName}" type="local"/>
			</plx:return>
		</plx:function>

		<xsl:if test="self::ElementEvent">
			<plx:function name="Get{$pascalName}HandlersDictionary" visibility="private">
				<plx:param name="action" dataTypeName="EventHandlerAction"/>
				<plx:returns dataTypeName="Dictionary">
					<plx:passTypeParam dataTypeName="Guid"/>
					<plx:passTypeParam dataTypeName="List">
						<xsl:copy-of select="$eventHandlerPassTypeParam"/>
					</plx:passTypeParam>
				</plx:returns>
				<plx:local name="{$camelName}Dictionary" dataTypeName="Dictionary">
					<plx:passTypeParam dataTypeName="Guid"/>
					<plx:passTypeParam dataTypeName="List">
						<xsl:copy-of select="$eventHandlerPassTypeParam"/>
					</plx:passTypeParam>
					<plx:initialize>
						<plx:callThis name="_{$camelName}Dictionary" type="field"/>
					</plx:initialize>
				</plx:local>
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="booleanAnd">
							<plx:left>
								<plx:binaryOperator type="identityEquality">
									<plx:left>
										<plx:nameRef name="{$camelName}Dictionary" type="local"/>
									</plx:left>
									<plx:right>
										<plx:nullKeyword/>
									</plx:right>
								</plx:binaryOperator>
							</plx:left>
							<plx:right>
								<plx:binaryOperator type="equality">
									<plx:left>
										<plx:nameRef name="action" type="parameter"/>
									</plx:left>
									<plx:right>
										<plx:callStatic name="Add" type="field" dataTypeName="EventHandlerAction"/>
									</plx:right>
								</plx:binaryOperator>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:callThis name="Initialize{$pascalName}HandlersStorage" type="methodCall"/>
					<plx:assign>
						<plx:left>
							<plx:nameRef name="{$camelName}Dictionary" type="local"/>
						</plx:left>
						<plx:right>
							<plx:callThis name="_{$camelName}Dictionary" type="field"/>
						</plx:right>
					</plx:assign>
				</plx:branch>
				<plx:return>
					<plx:nameRef name="{$camelName}Dictionary" type="local"/>
				</plx:return>
			</plx:function>

			<xsl:if test="child::GuidPair">
				<plx:function name="Get{$pascalName}HandlersGuidPairDictionary" visibility="private">
					<plx:param name="action" dataTypeName="EventHandlerAction"/>
					<plx:returns dataTypeName="Dictionary">
						<plx:passTypeParam dataTypeName="GuidPair"/>
						<plx:passTypeParam dataTypeName="List">
							<xsl:copy-of select="$eventHandlerPassTypeParam"/>
						</plx:passTypeParam>
					</plx:returns>
					<plx:local name="{$camelName}GuidPairDictionary" dataTypeName="Dictionary">
						<plx:passTypeParam dataTypeName="GuidPair"/>
						<plx:passTypeParam dataTypeName="List">
							<xsl:copy-of select="$eventHandlerPassTypeParam"/>
						</plx:passTypeParam>
						<plx:initialize>
							<plx:callThis name="_{$camelName}GuidPairDictionary" type="field"/>
						</plx:initialize>
					</plx:local>
					<plx:branch>
						<plx:condition>
							<plx:binaryOperator type="booleanAnd">
								<plx:left>
									<plx:binaryOperator type="identityEquality">
										<plx:left>
											<plx:nameRef name="{$camelName}GuidPairDictionary" type="local"/>
										</plx:left>
										<plx:right>
											<plx:nullKeyword/>
										</plx:right>
									</plx:binaryOperator>
								</plx:left>
								<plx:right>
									<plx:binaryOperator type="equality">
										<plx:left>
											<plx:nameRef name="action" type="parameter"/>
										</plx:left>
										<plx:right>
											<plx:callStatic name="Add" type="field" dataTypeName="EventHandlerAction"/>
										</plx:right>
									</plx:binaryOperator>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:callThis name="Initialize{$pascalName}HandlersStorage" type="methodCall"/>
						<plx:assign>
							<plx:left>
								<plx:nameRef name="{$camelName}GuidPairDictionary" type="local"/>
							</plx:left>
							<plx:right>
								<plx:callThis name="_{$camelName}GuidPairDictionary" type="field"/>
							</plx:right>
						</plx:assign>
					</plx:branch>
					<plx:return>
						<plx:nameRef name="{$camelName}GuidPairDictionary" type="local"/>
					</plx:return>
				</plx:function>
			</xsl:if>

			<plx:function name="Initialize{$pascalName}HandlersStorage" visibility="private">
				<plx:lock>
					<plx:initialize>
						<plx:callThis name="_lockObject" type="field"/>
					</plx:initialize>
					<plx:branch>
						<plx:condition>
							<plx:binaryOperator type="identityEquality">
								<plx:left>
									<plx:callThis name="_{$camelName}" type="field"/>
								</plx:left>
								<plx:right>
									<plx:nullKeyword/>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:assign>
							<plx:left>
								<plx:callThis name="_{$camelName}" type="field"/>
							</plx:left>
							<plx:right>
								<plx:callNew dataTypeName="List">
									<xsl:copy-of select="$eventHandlerPassTypeParam"/>
								</plx:callNew>
							</plx:right>
						</plx:assign>
						<plx:assign>
							<plx:left>
								<plx:callThis name="_{$camelName}Dictionary" type="field"/>
							</plx:left>
							<plx:right>
								<plx:callNew dataTypeName="Dictionary">
									<plx:passTypeParam dataTypeName="Guid"/>
									<plx:passTypeParam dataTypeName="List">
										<xsl:copy-of select="$eventHandlerPassTypeParam"/>
									</plx:passTypeParam>
								</plx:callNew>
							</plx:right>
						</plx:assign>
						<xsl:if test="child::GuidPair">
							<plx:assign>
								<plx:left>
									<plx:callThis name="_{$camelName}GuidPairDictionary" type="field"/>
								</plx:left>
								<plx:right>
									<plx:callNew dataTypeName="Dictionary">
										<plx:passTypeParam dataTypeName="GuidPair"/>
										<plx:passTypeParam dataTypeName="List">
											<xsl:copy-of select="$eventHandlerPassTypeParam"/>
										</plx:passTypeParam>
									</plx:callNew>
								</plx:right>
							</plx:assign>
						</xsl:if>
						<xsl:call-template name="GetAttachHandlerCode">
							<xsl:with-param name="IsEvent" select="@isEvent='true'"/>
							<xsl:with-param name="PascalName" select="$pascalName"/>
							<xsl:with-param name="EventArgsName" select="$eventArgsName"/>
						</xsl:call-template>
					</plx:branch>
				</plx:lock>
			</plx:function>
		</xsl:if>

		<xsl:variable name="isOverload" select="not(string-length($addRemoveNameDecorator)) or (self::ElementEvent and (@acceptsElementId='true' or child::Guid or child::GuidPair))"/>

		<!-- Do the static versions -->

		<plx:function name="Add{$addRemoveNameDecorator}Handler" visibility="public" modifier="static" overload="{$isOverload}">
			<plx:leadingInfo>
				<plx:docComment>
					<summary><![CDATA[Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.]]></summary>
					<param name="store"><![CDATA[The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.]]></param>
					<param name="handler"><![CDATA[The <see cref="EventHandler{TEventArgs}"/> that should be added.]]></param>
					<exception cref="ArgumentNullException"><![CDATA[<paramref name="store"/> is <see langword="null"/>.]]></exception>
					<exception cref="ObjectDisposedException"><![CDATA[<paramref name="store"/> has been disposed.]]></exception>
				</plx:docComment>
			</plx:leadingInfo>
			<plx:param name="store" dataTypeName="Store"/>
			<plx:param name="handler" dataTypeName="EventHandler">
				<plx:passTypeParam dataTypeName="{$eventArgsName}"/>
			</plx:param>
			<plx:callStatic name="AddOrRemove{$addRemoveNameDecorator}Handler" dataTypeName="ModelingEventManager">
				<plx:passParam>
					<plx:nameRef name="store" type="parameter"/>
				</plx:passParam>
				<plx:passParam>
					<plx:nameRef name="handler" type="parameter"/>
				</plx:passParam>
				<plx:passParam>
					<plx:callStatic name="Add" type="field" dataTypeName="EventHandlerAction"/>
				</plx:passParam>
			</plx:callStatic>
		</plx:function>

		<plx:function name="Remove{$addRemoveNameDecorator}Handler" visibility="public" modifier="static" overload="{$isOverload}">
			<plx:leadingInfo>
				<plx:docComment>
					<summary><![CDATA[Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.]]></summary>
					<param name="store"><![CDATA[The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.]]></param>
					<param name="handler"><![CDATA[The <see cref="EventHandler{TEventArgs}"/> that should be removed.]]></param>
					<exception cref="ArgumentNullException"><![CDATA[<paramref name="store"/> is <see langword="null"/>.]]></exception>
					<exception cref="ObjectDisposedException"><![CDATA[<paramref name="store"/> has been disposed.]]></exception>
				</plx:docComment>
			</plx:leadingInfo>
			<plx:param name="store" dataTypeName="Store"/>
			<plx:param name="handler" dataTypeName="EventHandler">
				<plx:passTypeParam dataTypeName="{$eventArgsName}"/>
			</plx:param>
			<plx:callStatic name="AddOrRemove{$addRemoveNameDecorator}Handler" dataTypeName="ModelingEventManager">
				<plx:passParam>
					<plx:nameRef name="store" type="parameter"/>
				</plx:passParam>
				<plx:passParam>
					<plx:nameRef name="handler" type="parameter"/>
				</plx:passParam>
				<plx:passParam>
					<plx:callStatic name="Remove" type="field" dataTypeName="EventHandlerAction"/>
				</plx:passParam>
			</plx:callStatic>
		</plx:function>

		<plx:function name="AddOrRemove{$addRemoveNameDecorator}Handler" visibility="public" modifier="static" overload="{$isOverload}">
			<plx:leadingInfo>
				<plx:docComment>
					<summary><![CDATA[Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.]]></summary>
					<param name="store"><![CDATA[The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.]]></param>
					<param name="handler"><![CDATA[The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.]]></param>
					<param name="action"><![CDATA[The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
specified by <paramref name="handler"/>.]]></param>
					<exception cref="ArgumentNullException"><![CDATA[<paramref name="store"/> is <see langword="null"/>.]]></exception>
					<exception cref="ObjectDisposedException"><![CDATA[<paramref name="store"/> has been disposed.]]></exception>
				</plx:docComment>
			</plx:leadingInfo>
			<plx:attribute dataTypeName="EditorBrowsableAttribute">
				<plx:passParam>
					<plx:callStatic name="Advanced" type="field" dataTypeName="EditorBrowsableState"/>
				</plx:passParam>
			</plx:attribute>
			<plx:param name="store" dataTypeName="Store"/>
			<plx:param name="handler" dataTypeName="EventHandler">
				<plx:passTypeParam dataTypeName="{$eventArgsName}"/>
			</plx:param>
			<plx:param name="action" dataTypeName="EventHandlerAction"/>
			<plx:branch>
				<plx:condition>
					<plx:binaryOperator type="identityInequality">
						<plx:left>
							<plx:cast type="exceptionCast" dataTypeName=".object">
								<plx:nameRef name="handler" type="parameter"/>
							</plx:cast>
						</plx:left>
						<plx:right>
							<plx:nullKeyword/>
						</plx:right>
					</plx:binaryOperator>
				</plx:condition>
				<plx:local name="manager" dataTypeName="ModelingEventManager">
					<plx:initialize>
						<plx:callStatic name="GetModelingEventManager" dataTypeName="ModelingEventManager">
							<plx:passParam>
								<plx:nameRef name="store" type="parameter"/>
							</plx:passParam>
							<plx:passParam>
								<plx:nameRef name="action" type="parameter"/>
							</plx:passParam>
						</plx:callStatic>
					</plx:initialize>
				</plx:local>
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="identityInequality">
							<plx:left>
								<plx:nameRef name="manager" type="local"/>
							</plx:left>
							<plx:right>
								<plx:nullKeyword/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:callStatic name="AddOrRemoveHandler" dataTypeName="ModelingEventManager">
						<plx:passMemberTypeParam dataTypeName="{$eventArgsName}"/>
						<plx:passParam>
							<plx:callInstance name="Get{$pascalName}Handlers">
								<plx:callObject>
									<plx:nameRef name="manager" type="local"/>
								</plx:callObject>
								<plx:passParam>
									<plx:nameRef name="action" type="parameter"/>
								</plx:passParam>
							</plx:callInstance>
						</plx:passParam>
						<plx:passParam>
							<plx:nameRef name="handler" type="parameter"/>
						</plx:passParam>
						<plx:passParam>
							<plx:nameRef name="action" type="parameter"/>
						</plx:passParam>
					</plx:callStatic>
				</plx:branch>
			</plx:branch>
		</plx:function>

		<!-- Do the instance versions -->

		<plx:function name="Add{$addRemoveNameDecorator}Handler" visibility="public" overload="{$isOverload}">
			<plx:leadingInfo>
				<plx:docComment>
					<summary><![CDATA[Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.]]></summary>
					<param name="handler"><![CDATA[The <see cref="EventHandler{TEventArgs}"/> that should be added.]]></param>
				</plx:docComment>
			</plx:leadingInfo>
			<plx:param name="handler" dataTypeName="EventHandler">
				<plx:passTypeParam dataTypeName="{$eventArgsName}"/>
			</plx:param>
			<plx:callThis name="AddOrRemove{$addRemoveNameDecorator}Handler">
				<plx:passParam>
					<plx:nameRef name="handler" type="parameter"/>
				</plx:passParam>
				<plx:passParam>
					<plx:callStatic name="Add" type="field" dataTypeName="EventHandlerAction"/>
				</plx:passParam>
			</plx:callThis>
		</plx:function>

		<plx:function name="Remove{$addRemoveNameDecorator}Handler" visibility="public" overload="{$isOverload}">
			<plx:leadingInfo>
				<plx:docComment>
					<summary><![CDATA[Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.]]></summary>
					<param name="handler"><![CDATA[The <see cref="EventHandler{TEventArgs}"/> that should be removed.]]></param>
				</plx:docComment>
			</plx:leadingInfo>
			<plx:param name="handler" dataTypeName="EventHandler">
				<plx:passTypeParam dataTypeName="{$eventArgsName}"/>
			</plx:param>
			<plx:callThis name="AddOrRemove{$addRemoveNameDecorator}Handler">
				<plx:passParam>
					<plx:nameRef name="handler" type="parameter"/>
				</plx:passParam>
				<plx:passParam>
					<plx:callStatic name="Remove" type="field" dataTypeName="EventHandlerAction"/>
				</plx:passParam>
			</plx:callThis>
		</plx:function>

		<plx:function name="AddOrRemove{$addRemoveNameDecorator}Handler" visibility="public" overload="{$isOverload}">
			<plx:leadingInfo>
				<plx:docComment>
					<summary><![CDATA[Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
as a handler for the <see cref="Store"/> specified specified when this <see cref="ModelingEventManager"/> was created.]]></summary>
					<param name="handler"><![CDATA[The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.]]></param>
					<param name="action"><![CDATA[The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
specified by <paramref name="handler"/>.]]></param>
				</plx:docComment>
			</plx:leadingInfo>
			<plx:attribute dataTypeName="EditorBrowsableAttribute">
				<plx:passParam>
					<plx:callStatic name="Advanced" type="field" dataTypeName="EditorBrowsableState"/>
				</plx:passParam>
			</plx:attribute>
			<plx:param name="handler" dataTypeName="EventHandler">
				<plx:passTypeParam dataTypeName="{$eventArgsName}"/>
			</plx:param>
			<plx:param name="action" dataTypeName="EventHandlerAction"/>
			<plx:branch>
				<plx:condition>
					<plx:binaryOperator type="identityInequality">
						<plx:left>
							<plx:cast type="exceptionCast" dataTypeName=".object">
								<plx:nameRef name="handler" type="parameter"/>
							</plx:cast>
						</plx:left>
						<plx:right>
							<plx:nullKeyword/>
						</plx:right>
					</plx:binaryOperator>
				</plx:condition>
				<plx:callStatic name="AddOrRemoveHandler" dataTypeName="ModelingEventManager">
					<plx:passMemberTypeParam dataTypeName="{$eventArgsName}"/>
					<plx:passParam>
						<plx:callThis name="Get{$pascalName}Handlers">
							<plx:passParam>
								<plx:nameRef name="action" type="parameter"/>
							</plx:passParam>
						</plx:callThis>
					</plx:passParam>
					<plx:passParam>
						<plx:nameRef name="handler" type="parameter"/>
					</plx:passParam>
					<plx:passParam>
						<plx:nameRef name="action" type="parameter"/>
					</plx:passParam>
				</plx:callStatic>
			</plx:branch>
		</plx:function>

		<xsl:if test="self::ElementEvent">

			<xsl:call-template name="GetAddOrRemoveGuidHandlerCode">
				<xsl:with-param name="PascalName" select="$pascalName"/>
				<xsl:with-param name="EventArgsName" select="$eventArgsName"/>
				<xsl:with-param name="AddRemoveNameDecorator" select="$addRemoveNameDecorator"/>
				<xsl:with-param name="ExtraParamName" select="'domainClass'"/>
				<xsl:with-param name="ExtraParamType" select="'DomainClassInfo'"/>
			</xsl:call-template>

			<xsl:call-template name="GetAddOrRemoveGuidHandlerCode">
				<xsl:with-param name="PascalName" select="$pascalName"/>
				<xsl:with-param name="EventArgsName" select="$eventArgsName"/>
				<xsl:with-param name="AddRemoveNameDecorator" select="$addRemoveNameDecorator"/>
				<xsl:with-param name="ExtraParamName" select="'domainModel'"/>
				<xsl:with-param name="ExtraParamType" select="'DomainModelInfo'"/>
			</xsl:call-template>

			<xsl:if test="@acceptsElementId='true'">
				<xsl:call-template name="GetAddOrRemoveGuidHandlerCode">
					<xsl:with-param name="PascalName" select="$pascalName"/>
					<xsl:with-param name="EventArgsName" select="$eventArgsName"/>
					<xsl:with-param name="AddRemoveNameDecorator" select="$addRemoveNameDecorator"/>
					<xsl:with-param name="ExtraParamName" select="'elementId'"/>
					<xsl:with-param name="ExtraParamType" select="'Guid'"/>
				</xsl:call-template>
			</xsl:if>

			<xsl:for-each select="child::Guid/child::Param">
				<xsl:call-template name="GetAddOrRemoveGuidHandlerCode">
					<xsl:with-param name="PascalName" select="$pascalName"/>
					<xsl:with-param name="EventArgsName" select="$eventArgsName"/>
					<xsl:with-param name="AddRemoveNameDecorator" select="$addRemoveNameDecorator"/>
					<xsl:with-param name="ExtraParamName" select="@name"/>
					<xsl:with-param name="ExtraParamType" select="@type"/>
				</xsl:call-template>
			</xsl:for-each>

			<xsl:for-each select="child::GuidPair">
				<xsl:call-template name="GetAddOrRemoveGuidPairHandlerCode">
					<xsl:with-param name="PascalName" select="$pascalName"/>
					<xsl:with-param name="EventArgsName" select="$eventArgsName"/>
					<xsl:with-param name="AddRemoveNameDecorator" select="$addRemoveNameDecorator"/>
				</xsl:call-template>
			</xsl:for-each>

		</xsl:if>

		<plx:function name="Handle{$pascalName}" visibility="private">
			<plx:trailingInfo>
				<plx:pragma type="closeRegion" data="{$pascalName} support"/>
			</plx:trailingInfo>
			<plx:param name="sender" dataTypeName=".object"/>
			<plx:param name="e" dataTypeName="{$eventArgsName}"/>
			<xsl:if test="self::ElementEvent">
				<plx:local name="serviceProvider" dataTypeName="IServiceProvider">
					<plx:initialize>
						<plx:callThis name="_store" type="field"/>
					</plx:initialize>
				</plx:local>
				<xsl:if test="child::Guid or child::GuidPair">
					<plx:local name="handlers" dataTypeName="List">
						<xsl:copy-of select="$eventHandlerPassTypeParam"/>
					</plx:local>
					<xsl:variable name="params" select="(child::Guid | child::GuidPair)/child::Param"/>
					<xsl:variable name="repeatedParamsFragment">
						<xsl:for-each select="$params">
							<xsl:if test="(count($params[@name=current()/@name]) > 1) or (@type='DomainClassInfo') or (parent::GuidPair/child::Param[@type='DomainClassInfo'])">
								<xsl:copy-of select="."/>
							</xsl:if>
						</xsl:for-each>
					</xsl:variable>
					<xsl:variable name="localsFragment">
						<xsl:call-template name="GetLocalsCode">
							<xsl:with-param name="Params" select="exsl:node-set($repeatedParamsFragment)/child::*"/>
						</xsl:call-template>
					</xsl:variable>
					<xsl:variable name="locals" select="exsl:node-set($localsFragment)/child::*"/>
					<xsl:copy-of select="$locals"/>
					<xsl:variable name="useLocalForGuidPairDictionary" select="count(child::GuidPair) > 1 or (child::GuidPair/child::Param[@type='DomainClassInfo'])"/>
					<xsl:if test="$useLocalForGuidPairDictionary">
						<plx:local name="{$camelName}GuidPairDictionary" dataTypeName="Dictionary">
							<plx:passTypeParam dataTypeName="GuidPair"/>
							<plx:passTypeParam dataTypeName="List">
								<xsl:copy-of select="$eventHandlerPassTypeParam"/>
							</plx:passTypeParam>
							<plx:initialize>
								<plx:callThis name="_{$camelName}GuidPairDictionary" type="field"/>
							</plx:initialize>
						</plx:local>
					</xsl:if>
					<xsl:for-each select="child::GuidPair">
						<xsl:variable name="param1" select="child::Param[1]"/>
						<xsl:variable name="param2" select="child::Param[2]"/>
						<xsl:variable name="param1Name" select="$param1/@name"/>
						<xsl:variable name="param2Name" select="$param2/@name"/>
						<xsl:variable name="param1IsDomainClassInfo" select="$param1/@type='DomainClassInfo'"/>
						<xsl:variable name="param2IsDomainClassInfo" select="$param2/@type='DomainClassInfo'"/>
						<xsl:variable name="bodyCodeFragment">
							<plx:branch>
								<plx:condition>
									<plx:callInstance name="TryGetValue" type="methodCall">
										<plx:callObject>
											<xsl:choose>
												<xsl:when test="$useLocalForGuidPairDictionary">
													<plx:nameRef name="{$camelName}GuidPairDictionary" type="local"/>
												</xsl:when>
												<xsl:otherwise>
													<plx:callThis name="_{$camelName}GuidPairDictionary" type="field"/>
												</xsl:otherwise>
											</xsl:choose>
										</plx:callObject>
										<plx:passParam>
											<plx:callNew dataTypeName="GuidPair">
												<plx:passParam>
													<xsl:choose>
														<xsl:when test="$param1IsDomainClassInfo">
															<plx:callInstance name="Id" type="property">
																<plx:callObject>
																	<plx:nameRef name="{$param1Name}" type="local"/>
																</plx:callObject>
															</plx:callInstance>
														</xsl:when>
														<xsl:when test="$locals[@name=$param1Name]">
															<plx:nameRef name="{$param1Name}" type="local"/>
														</xsl:when>
														<xsl:otherwise>
															<xsl:apply-templates select="$param1" mode="GetGuidFromEventArgs"/>
														</xsl:otherwise>
													</xsl:choose>
												</plx:passParam>
												<plx:passParam>
													<xsl:choose>
														<xsl:when test="$param2IsDomainClassInfo">
															<plx:callInstance name="Id" type="property">
																<plx:callObject>
																	<plx:nameRef name="{$param2Name}" type="local"/>
																</plx:callObject>
															</plx:callInstance>
														</xsl:when>
														<xsl:when test="$locals[@name=$param2Name]">
															<plx:nameRef name="{$param2Name}" type="local"/>
														</xsl:when>
														<xsl:otherwise>
															<xsl:apply-templates select="$param2" mode="GetGuidFromEventArgs"/>
														</xsl:otherwise>
													</xsl:choose>
												</plx:passParam>
											</plx:callNew>
										</plx:passParam>
										<plx:passParam type="out">
											<plx:nameRef name="handlers" type="local"/>
										</plx:passParam>
									</plx:callInstance>
								</plx:condition>
								<plx:callStatic name="InvokeHandlers" dataTypeName="ModelingEventManager">
									<plx:passMemberTypeParam dataTypeName="{$eventArgsName}"/>
									<plx:passParam>
										<plx:nameRef name="serviceProvider" type="local"/>
									</plx:passParam>
									<plx:passParam>
										<plx:nameRef name="handlers" type="local"/>
									</plx:passParam>
									<plx:passParam>
										<plx:nameRef name="sender" type="parameter"/>
									</plx:passParam>
									<plx:passParam>
										<plx:nameRef name="e" type="parameter"/>
									</plx:passParam>
								</plx:callStatic>
							</plx:branch>
						</xsl:variable>
						<xsl:variable name="bodyCode" select="exsl:node-set($bodyCodeFragment)/child::*"/>
						<xsl:choose>
							<xsl:when test="$param1IsDomainClassInfo and $param2IsDomainClassInfo">
								<!-- Note: A further optimization for this case would be to have the inner loop use a cached local for the Id for each iteration of the outer loop. -->
								<!-- However, since there is no known situation where we will have two DomainClassInfos anyway, this optimization has not yet been implemented. -->
								<xsl:variable name="outerBodyCodeFragment">
									<xsl:call-template name="GetDoWhileLoopCodeForDomainClassInfo">
										<xsl:with-param name="LocalName" select="$param1Name"/>
										<xsl:with-param name="BodyCode" select="$bodyCode"/>
									</xsl:call-template>
								</xsl:variable>
								<xsl:call-template name="GetDoWhileLoopCodeForDomainClassInfo">
									<xsl:with-param name="LocalName" select="$param2Name"/>
									<xsl:with-param name="BodyCode" select="exsl:node-set($outerBodyCodeFragment)/child::*"/>
								</xsl:call-template>
							</xsl:when>
							<xsl:when test="$param1IsDomainClassInfo">
								<xsl:call-template name="GetDoWhileLoopCodeForDomainClassInfo">
									<xsl:with-param name="LocalName" select="$param1Name"/>
									<xsl:with-param name="BodyCode" select="$bodyCode"/>
								</xsl:call-template>
							</xsl:when>
							<xsl:when test="$param2IsDomainClassInfo">
								<xsl:call-template name="GetDoWhileLoopCodeForDomainClassInfo">
									<xsl:with-param name="LocalName" select="$param2Name"/>
									<xsl:with-param name="BodyCode" select="$bodyCode"/>
								</xsl:call-template>
							</xsl:when>
							<xsl:otherwise>
								<xsl:copy-of select="$bodyCode"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:for-each>
					<xsl:if test="child::Guid">
						<plx:local name="{$camelName}Dictionary" dataTypeName="Dictionary">
							<plx:passTypeParam dataTypeName="Guid"/>
							<plx:passTypeParam dataTypeName="List">
								<xsl:copy-of select="$eventHandlerPassTypeParam"/>
							</plx:passTypeParam>
							<plx:initialize>
								<plx:callThis name="_{$camelName}Dictionary" type="field"/>
							</plx:initialize>
						</plx:local>
					</xsl:if>
					<xsl:for-each select="child::Guid/child::Param">
						<xsl:variable name="bodyCodeFragment">
							<plx:branch>
								<plx:condition>
									<plx:callInstance name="TryGetValue" type="methodCall">
										<plx:callObject>
											<plx:nameRef name="{$camelName}Dictionary" type="local"/>
										</plx:callObject>
										<plx:passParam>
											<xsl:choose>
												<xsl:when test="@type='DomainClassInfo'">
													<plx:callInstance name="Id" type="property">
														<plx:callObject>
															<plx:nameRef name="{@name}" type="local"/>
														</plx:callObject>
													</plx:callInstance>
												</xsl:when>
												<xsl:when test="$locals[@name=current()/@name]">
													<plx:nameRef name="{@name}" type="local"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:apply-templates select="." mode="GetGuidFromEventArgs"/>
												</xsl:otherwise>
											</xsl:choose>
										</plx:passParam>
										<plx:passParam type="out">
											<plx:nameRef name="handlers" type="local"/>
										</plx:passParam>
									</plx:callInstance>
								</plx:condition>
								<plx:callStatic name="InvokeHandlers" dataTypeName="ModelingEventManager">
									<plx:passMemberTypeParam dataTypeName="{$eventArgsName}"/>
									<plx:passParam>
										<plx:nameRef name="serviceProvider" type="local"/>
									</plx:passParam>
									<plx:passParam>
										<plx:nameRef name="handlers" type="local"/>
									</plx:passParam>
									<plx:passParam>
										<plx:nameRef name="sender" type="parameter"/>
									</plx:passParam>
									<plx:passParam>
										<plx:nameRef name="e" type="parameter"/>
									</plx:passParam>
								</plx:callStatic>
							</plx:branch>
						</xsl:variable>
						<xsl:variable name="bodyCode" select="exsl:node-set($bodyCodeFragment)/child::*"/>
						<xsl:choose>
							<xsl:when test="@type='DomainClassInfo'">
								<xsl:call-template name="GetDoWhileLoopCodeForDomainClassInfo">
									<xsl:with-param name="LocalName" select="@name"/>
									<xsl:with-param name="BodyCode" select="$bodyCode"/>
								</xsl:call-template>
							</xsl:when>
							<xsl:otherwise>
								<xsl:copy-of select="$bodyCode"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:for-each>
				</xsl:if>
				<plx:callStatic name="InvokeHandlers" dataTypeName="ModelingEventManager">
					<plx:passMemberTypeParam dataTypeName="{$eventArgsName}"/>
					<plx:passParam>
						<plx:nameRef name="serviceProvider" type="local"/>
					</plx:passParam>
					<plx:passParam>
						<xsl:choose>
							<xsl:when test="child::Guid">
								<plx:nameRef name="{$camelName}Dictionary" type="local"/>
							</xsl:when>
							<xsl:otherwise>
								<plx:callThis name="_{$camelName}Dictionary" type="field"/>
							</xsl:otherwise>
						</xsl:choose>
					</plx:passParam>
					<plx:passParam>
						<plx:nameRef name="sender" type="parameter"/>
					</plx:passParam>
					<plx:passParam>
						<plx:nameRef name="e" type="parameter"/>
					</plx:passParam>
				</plx:callStatic>
			</xsl:if>
			<plx:callStatic name="InvokeHandlers" dataTypeName="ModelingEventManager">
				<plx:passMemberTypeParam dataTypeName="{$eventArgsName}"/>
				<plx:passParam>
					<xsl:choose>
						<xsl:when test="self::ElementEvent">
							<plx:nameRef name="serviceProvider" type="local"/>
						</xsl:when>
						<xsl:otherwise>
							<plx:callThis name="_store" type="field"/>
						</xsl:otherwise>
					</xsl:choose>
				</plx:passParam>
				<plx:passParam>
					<plx:callThis name="_{$camelName}" type="field"/>
				</plx:passParam>
				<plx:passParam>
					<plx:nameRef name="sender" type="parameter"/>
				</plx:passParam>
				<plx:passParam>
					<plx:nameRef name="e" type="parameter"/>
				</plx:passParam>
			</plx:callStatic>
		</plx:function>

	</xsl:template>

	<xsl:template name="GetAttachHandlerCode">
		<xsl:param name="IsEvent"/>
		<xsl:param name="PascalName"/>
		<xsl:param name="EventArgsName"/>
		<xsl:choose>
			<xsl:when test="$IsEvent">
				<plx:attachEvent>
					<plx:left>
						<plx:callInstance name="{$PascalName}" type="event">
							<plx:callObject>
								<plx:callInstance name="EventManagerDirectory" type="property">
									<plx:callObject>
										<plx:callThis name="_store" type="field"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:callObject>
						</plx:callInstance>
					</plx:left>
					<plx:right>
						<plx:callNew dataTypeName="EventHandler">
							<plx:passTypeParam dataTypeName="{$EventArgsName}"/>
							<plx:passParam>
								<plx:callThis name="Handle{$PascalName}" type="methodReference"/>
							</plx:passParam>
						</plx:callNew>
					</plx:right>
				</plx:attachEvent>
			</xsl:when>
			<xsl:otherwise>
				<plx:callInstance name="Add">
					<plx:callObject>
						<plx:callInstance name="{$PascalName}" type="property">
							<plx:callObject>
								<plx:callInstance name="EventManagerDirectory" type="property">
									<plx:callObject>
										<plx:callThis name="_store" type="field"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:callObject>
						</plx:callInstance>
					</plx:callObject>
					<plx:passParam>
						<plx:callNew dataTypeName="EventHandler">
							<plx:passTypeParam dataTypeName="{$EventArgsName}"/>
							<plx:passParam>
								<plx:callThis name="Handle{$PascalName}" type="methodReference"/>
							</plx:passParam>
						</plx:callNew>
					</plx:passParam>
				</plx:callInstance>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="GetAddOrRemoveGuidHandlerCode">
		<xsl:param name="PascalName"/>
		<xsl:param name="EventArgsName"/>
		<xsl:param name="AddRemoveNameDecorator"/>
		<xsl:param name="ExtraParamName"/>
		<xsl:param name="ExtraParamType"/>

		<!-- Do the static versions -->

		<plx:function name="Add{$AddRemoveNameDecorator}Handler" visibility="public" modifier="static" overload="true">
			<plx:leadingInfo>
				<plx:docComment>
					<summary><![CDATA[Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.]]></summary>
					<param name="store"><![CDATA[The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.]]></param>
					<param name="{$ExtraParamName}"><![CDATA[The <paramref name="]]><xsl:value-of select="string($ExtraParamName)"/><![CDATA["/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.]]></param>
					<param name="handler"><![CDATA[The <see cref="EventHandler{TEventArgs}"/> that should be added.]]></param>
					<exception cref="ArgumentNullException"><![CDATA[<paramref name="store"/> is <see langword="null"/>.]]></exception>
					<exception cref="ObjectDisposedException"><![CDATA[<paramref name="store"/> has been disposed.]]></exception>
					<xsl:if test="not($ExtraParamType='Guid')" xml:space="preserve"><exception cref="ArgumentNullException"><![CDATA[<paramref name="]]><xsl:value-of select="string($ExtraParamName)"/><![CDATA["/> is <see langword="null"/>.]]></exception></xsl:if>
				</plx:docComment>
			</plx:leadingInfo>
			<plx:param name="store" dataTypeName="Store"/>
			<plx:param name="{$ExtraParamName}" dataTypeName="{$ExtraParamType}"/>
			<plx:param name="handler" dataTypeName="EventHandler">
				<plx:passTypeParam dataTypeName="{$EventArgsName}"/>
			</plx:param>
			<plx:callStatic name="AddOrRemove{$AddRemoveNameDecorator}Handler" dataTypeName="ModelingEventManager">
				<plx:passParam>
					<plx:nameRef name="store" type="parameter"/>
				</plx:passParam>
				<plx:passParam>
					<plx:nameRef name="{$ExtraParamName}" type="parameter"/>
				</plx:passParam>
				<plx:passParam>
					<plx:nameRef name="handler" type="parameter"/>
				</plx:passParam>
				<plx:passParam>
					<plx:callStatic name="Add" type="field" dataTypeName="EventHandlerAction"/>
				</plx:passParam>
			</plx:callStatic>
		</plx:function>

		<plx:function name="Remove{$AddRemoveNameDecorator}Handler" visibility="public" modifier="static" overload="true">
			<plx:leadingInfo>
				<plx:docComment>
					<summary><![CDATA[Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.]]></summary>
					<param name="store"><![CDATA[The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.]]></param>
					<param name="{$ExtraParamName}"><![CDATA[The <paramref name="]]><xsl:value-of select="string($ExtraParamName)"/><![CDATA["/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.]]></param>
					<param name="handler"><![CDATA[The <see cref="EventHandler{TEventArgs}"/> that should be removed.]]></param>
					<exception cref="ArgumentNullException"><![CDATA[<paramref name="store"/> is <see langword="null"/>.]]></exception>
					<exception cref="ObjectDisposedException"><![CDATA[<paramref name="store"/> has been disposed.]]></exception>
					<xsl:if test="not($ExtraParamType='Guid')" xml:space="preserve"><exception cref="ArgumentNullException"><![CDATA[<paramref name="]]><xsl:value-of select="string($ExtraParamName)"/><![CDATA["/> is <see langword="null"/>.]]></exception></xsl:if>
				</plx:docComment>
			</plx:leadingInfo>
			<plx:param name="store" dataTypeName="Store"/>
			<plx:param name="{$ExtraParamName}" dataTypeName="{$ExtraParamType}"/>
			<plx:param name="handler" dataTypeName="EventHandler">
				<plx:passTypeParam dataTypeName="{$EventArgsName}"/>
			</plx:param>
			<plx:callStatic name="AddOrRemove{$AddRemoveNameDecorator}Handler" dataTypeName="ModelingEventManager">
				<plx:passParam>
					<plx:nameRef name="store" type="parameter"/>
				</plx:passParam>
				<plx:passParam>
					<plx:nameRef name="{$ExtraParamName}" type="parameter"/>
				</plx:passParam>
				<plx:passParam>
					<plx:nameRef name="handler" type="parameter"/>
				</plx:passParam>
				<plx:passParam>
					<plx:callStatic name="Remove" type="field" dataTypeName="EventHandlerAction"/>
				</plx:passParam>
			</plx:callStatic>
		</plx:function>

		<plx:function name="AddOrRemove{$AddRemoveNameDecorator}Handler" visibility="public" modifier="static" overload="true">
			<plx:leadingInfo>
				<plx:docComment>
					<summary><![CDATA[Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.]]></summary>
					<param name="store"><![CDATA[The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.]]></param>
					<param name="{$ExtraParamName}"><![CDATA[The <paramref name="]]><xsl:value-of select="string($ExtraParamName)"/><![CDATA["/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.]]></param>
					<param name="handler"><![CDATA[The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.]]></param>
					<param name="action"><![CDATA[The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
specified by <paramref name="handler"/>.]]></param>
					<exception cref="ArgumentNullException"><![CDATA[<paramref name="store"/> is <see langword="null"/>.]]></exception>
					<exception cref="ObjectDisposedException"><![CDATA[<paramref name="store"/> has been disposed.]]></exception>
					<xsl:if test="not($ExtraParamType='Guid')" xml:space="preserve"><exception cref="ArgumentNullException"><![CDATA[<paramref name="]]><xsl:value-of select="string($ExtraParamName)"/><![CDATA["/> is <see langword="null"/>.]]></exception></xsl:if>
				</plx:docComment>
			</plx:leadingInfo>
			<plx:attribute dataTypeName="EditorBrowsableAttribute">
				<plx:passParam>
					<plx:callStatic name="Advanced" type="field" dataTypeName="EditorBrowsableState"/>
				</plx:passParam>
			</plx:attribute>
			<plx:param name="store" dataTypeName="Store"/>
			<plx:param name="{$ExtraParamName}" dataTypeName="{$ExtraParamType}"/>
			<plx:param name="handler" dataTypeName="EventHandler">
				<plx:passTypeParam dataTypeName="{$EventArgsName}"/>
			</plx:param>
			<plx:param name="action" dataTypeName="EventHandlerAction"/>
			<xsl:if test="not($ExtraParamType='Guid')">
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="identityEquality">
							<plx:left>
								<plx:nameRef name="{$ExtraParamName}" type="parameter"/>
							</plx:left>
							<plx:right>
								<plx:nullKeyword/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:throw>
						<plx:callNew dataTypeName="ArgumentNullException">
							<plx:passParam>
								<plx:string data="{$ExtraParamName}"/>
							</plx:passParam>
						</plx:callNew>
					</plx:throw>
				</plx:branch>
			</xsl:if>
			<plx:branch>
				<plx:condition>
					<plx:binaryOperator type="identityInequality">
						<plx:left>
							<plx:cast type="exceptionCast" dataTypeName=".object">
								<plx:nameRef name="handler" type="parameter"/>
							</plx:cast>
						</plx:left>
						<plx:right>
							<plx:nullKeyword/>
						</plx:right>
					</plx:binaryOperator>
				</plx:condition>
				<plx:local name="manager" dataTypeName="ModelingEventManager">
					<plx:initialize>
						<plx:callStatic name="GetModelingEventManager" dataTypeName="ModelingEventManager">
							<plx:passParam>
								<plx:nameRef name="store" type="parameter"/>
							</plx:passParam>
							<plx:passParam>
								<plx:nameRef name="action" type="parameter"/>
							</plx:passParam>
						</plx:callStatic>
					</plx:initialize>
				</plx:local>
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="identityInequality">
							<plx:left>
								<plx:nameRef name="manager" type="local"/>
							</plx:left>
							<plx:right>
								<plx:nullKeyword/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:callStatic name="AddOrRemoveHandler" dataTypeName="ModelingEventManager">
						<plx:passMemberTypeParam dataTypeName="Guid"/>
						<plx:passMemberTypeParam dataTypeName="{$EventArgsName}"/>
						<plx:passParam>
							<plx:callInstance name="Get{$PascalName}HandlersDictionary">
								<plx:callObject>
									<plx:nameRef name="manager" type="local"/>
								</plx:callObject>
								<plx:passParam>
									<plx:nameRef name="action" type="parameter"/>
								</plx:passParam>
							</plx:callInstance>
						</plx:passParam>
						<plx:passParam>
							<xsl:choose>
								<xsl:when test="$ExtraParamType='Guid'">
									<plx:nameRef name="{$ExtraParamName}" type="parameter"/>
								</xsl:when>
								<xsl:otherwise>
									<plx:callInstance name="Id" type="property">
										<plx:callObject>
											<plx:nameRef name="{$ExtraParamName}" type="parameter"/>
										</plx:callObject>
									</plx:callInstance>
								</xsl:otherwise>
							</xsl:choose>
						</plx:passParam>
						<plx:passParam>
							<plx:nameRef name="handler" type="parameter"/>
						</plx:passParam>
						<plx:passParam>
							<plx:nameRef name="action" type="parameter"/>
						</plx:passParam>
					</plx:callStatic>
				</plx:branch>
			</plx:branch>
		</plx:function>

		<!-- Do the instance versions -->

		<plx:function name="Add{$AddRemoveNameDecorator}Handler" visibility="public" overload="true">
			<plx:leadingInfo>
				<plx:docComment>
					<summary><![CDATA[Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.]]></summary>
					<param name="{$ExtraParamName}"><![CDATA[The <paramref name="]]><xsl:value-of select="string($ExtraParamName)"/><![CDATA["/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.]]></param>
					<param name="handler"><![CDATA[The <see cref="EventHandler{TEventArgs}"/> that should be added.]]></param>
					<xsl:if test="not($ExtraParamType='Guid')" xml:space="preserve"><exception cref="ArgumentNullException"><![CDATA[<paramref name="]]><xsl:value-of select="string($ExtraParamName)"/><![CDATA["/> is <see langword="null"/>.]]></exception></xsl:if>
				</plx:docComment>
			</plx:leadingInfo>
			<plx:param name="{$ExtraParamName}" dataTypeName="{$ExtraParamType}"/>
			<plx:param name="handler" dataTypeName="EventHandler">
				<plx:passTypeParam dataTypeName="{$EventArgsName}"/>
			</plx:param>
			<plx:callThis name="AddOrRemove{$AddRemoveNameDecorator}Handler">
				<plx:passParam>
					<plx:nameRef name="{$ExtraParamName}" type="parameter"/>
				</plx:passParam>
				<plx:passParam>
					<plx:nameRef name="handler" type="parameter"/>
				</plx:passParam>
				<plx:passParam>
					<plx:callStatic name="Add" type="field" dataTypeName="EventHandlerAction"/>
				</plx:passParam>
			</plx:callThis>
		</plx:function>

		<plx:function name="Remove{$AddRemoveNameDecorator}Handler" visibility="public" overload="true">
			<plx:leadingInfo>
				<plx:docComment>
					<summary><![CDATA[Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.]]></summary>
					<param name="{$ExtraParamName}"><![CDATA[The <paramref name="]]><xsl:value-of select="string($ExtraParamName)"/><![CDATA["/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.]]></param>
					<param name="handler"><![CDATA[The <see cref="EventHandler{TEventArgs}"/> that should be removed.]]></param>
					<xsl:if test="not($ExtraParamType='Guid')" xml:space="preserve"><exception cref="ArgumentNullException"><![CDATA[<paramref name="]]><xsl:value-of select="string($ExtraParamName)"/><![CDATA["/> is <see langword="null"/>.]]></exception></xsl:if>
				</plx:docComment>
			</plx:leadingInfo>
			<plx:param name="{$ExtraParamName}" dataTypeName="{$ExtraParamType}"/>
			<plx:param name="handler" dataTypeName="EventHandler">
				<plx:passTypeParam dataTypeName="{$EventArgsName}"/>
			</plx:param>
			<plx:callThis name="AddOrRemove{$AddRemoveNameDecorator}Handler">
				<plx:passParam>
					<plx:nameRef name="{$ExtraParamName}" type="parameter"/>
				</plx:passParam>
				<plx:passParam>
					<plx:nameRef name="handler" type="parameter"/>
				</plx:passParam>
				<plx:passParam>
					<plx:callStatic name="Remove" type="field" dataTypeName="EventHandlerAction"/>
				</plx:passParam>
			</plx:callThis>
		</plx:function>

		<plx:function name="AddOrRemove{$AddRemoveNameDecorator}Handler" visibility="public" overload="true">
			<plx:leadingInfo>
				<plx:docComment>
					<summary><![CDATA[Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.]]></summary>
					<param name="{$ExtraParamName}"><![CDATA[The <paramref name="]]><xsl:value-of select="string($ExtraParamName)"/><![CDATA["/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.]]></param>
					<param name="handler"><![CDATA[The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.]]></param>
					<param name="action"><![CDATA[The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
specified by <paramref name="handler"/>.]]></param>
					<xsl:if test="not($ExtraParamType='Guid')" xml:space="preserve"><exception cref="ArgumentNullException"><![CDATA[<paramref name="]]><xsl:value-of select="string($ExtraParamName)"/><![CDATA["/> is <see langword="null"/>.]]></exception></xsl:if>
				</plx:docComment>
			</plx:leadingInfo>
			<plx:attribute dataTypeName="EditorBrowsableAttribute">
				<plx:passParam>
					<plx:callStatic name="Advanced" type="field" dataTypeName="EditorBrowsableState"/>
				</plx:passParam>
			</plx:attribute>
			<plx:param name="{$ExtraParamName}" dataTypeName="{$ExtraParamType}"/>
			<plx:param name="handler" dataTypeName="EventHandler">
				<plx:passTypeParam dataTypeName="{$EventArgsName}"/>
			</plx:param>
			<plx:param name="action" dataTypeName="EventHandlerAction"/>
			<xsl:if test="not($ExtraParamType='Guid')">
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="identityEquality">
							<plx:left>
								<plx:nameRef name="{$ExtraParamName}" type="parameter"/>
							</plx:left>
							<plx:right>
								<plx:nullKeyword/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:throw>
						<plx:callNew dataTypeName="ArgumentNullException">
							<plx:passParam>
								<plx:string data="{$ExtraParamName}"/>
							</plx:passParam>
						</plx:callNew>
					</plx:throw>
				</plx:branch>
			</xsl:if>
			<plx:branch>
				<plx:condition>
					<plx:binaryOperator type="identityInequality">
						<plx:left>
							<plx:cast type="exceptionCast" dataTypeName=".object">
								<plx:nameRef name="handler" type="parameter"/>
							</plx:cast>
						</plx:left>
						<plx:right>
							<plx:nullKeyword/>
						</plx:right>
					</plx:binaryOperator>
				</plx:condition>
				<plx:callStatic name="AddOrRemoveHandler" dataTypeName="ModelingEventManager">
					<plx:passMemberTypeParam dataTypeName="Guid"/>
					<plx:passMemberTypeParam dataTypeName="{$EventArgsName}"/>
					<plx:passParam>
						<plx:callThis name="Get{$PascalName}HandlersDictionary">
							<plx:passParam>
								<plx:nameRef name="action" type="parameter"/>
							</plx:passParam>
						</plx:callThis>
					</plx:passParam>
					<plx:passParam>
						<xsl:choose>
							<xsl:when test="$ExtraParamType='Guid'">
								<plx:nameRef name="{$ExtraParamName}" type="parameter"/>
							</xsl:when>
							<xsl:otherwise>
								<plx:callInstance name="Id" type="property">
									<plx:callObject>
										<plx:nameRef name="{$ExtraParamName}" type="parameter"/>
									</plx:callObject>
								</plx:callInstance>
							</xsl:otherwise>
						</xsl:choose>
					</plx:passParam>
					<plx:passParam>
						<plx:nameRef name="handler" type="parameter"/>
					</plx:passParam>
					<plx:passParam>
						<plx:nameRef name="action" type="parameter"/>
					</plx:passParam>
				</plx:callStatic>
			</plx:branch>
		</plx:function>
	</xsl:template>

	<xsl:template name="GetAddOrRemoveGuidPairHandlerCode">
		<xsl:param name="PascalName"/>
		<xsl:param name="EventArgsName"/>
		<xsl:param name="AddRemoveNameDecorator"/>
		<xsl:variable name="param1" select="child::Param[1]"/>
		<xsl:variable name="param2" select="child::Param[2]"/>
		<xsl:variable name="param1Name" select="$param1/@name"/>
		<xsl:variable name="param2Name" select="$param2/@name"/>
		<xsl:variable name="param1Type" select="$param1/@type"/>
		<xsl:variable name="param2Type" select="$param2/@type"/>

		<!-- Do the static versions -->

		<plx:function name="Add{$AddRemoveNameDecorator}Handler" visibility="public" modifier="static" overload="true">
			<plx:leadingInfo>
				<plx:docComment>
					<summary><![CDATA[Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.]]></summary>
					<param name="store"><![CDATA[The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.]]></param>
					<param name="{$param1Name}"><![CDATA[The <paramref name="]]><xsl:value-of select="string($param1Name)"/><![CDATA["/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.]]></param>
					<param name="{$param2Name}"><![CDATA[The <paramref name="]]><xsl:value-of select="string($param2Name)"/><![CDATA["/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.]]></param>
					<param name="handler"><![CDATA[The <see cref="EventHandler{TEventArgs}"/> that should be added.]]></param>
					<exception cref="ArgumentNullException"><![CDATA[<paramref name="store"/> is <see langword="null"/>.]]></exception>
					<exception cref="ObjectDisposedException"><![CDATA[<paramref name="store"/> has been disposed.]]></exception>
					<xsl:if test="not($param1Type='Guid')" xml:space="preserve"><exception cref="ArgumentNullException"><![CDATA[<paramref name="]]><xsl:value-of select="string($param1Name)"/><![CDATA["/> is <see langword="null"/>.]]></exception></xsl:if>
					<xsl:if test="not($param2Type='Guid')" xml:space="preserve"><exception cref="ArgumentNullException"><![CDATA[<paramref name="]]><xsl:value-of select="string($param2Name)"/><![CDATA["/> is <see langword="null"/>.]]></exception></xsl:if>
				</plx:docComment>
			</plx:leadingInfo>
			<plx:param name="store" dataTypeName="Store"/>
			<plx:param name="{$param1Name}" dataTypeName="{$param1Type}"/>
			<plx:param name="{$param2Name}" dataTypeName="{$param2Type}"/>
			<plx:param name="handler" dataTypeName="EventHandler">
				<plx:passTypeParam dataTypeName="{$EventArgsName}"/>
			</plx:param>
			<plx:callStatic name="AddOrRemove{$AddRemoveNameDecorator}Handler" dataTypeName="ModelingEventManager">
				<plx:passParam>
					<plx:nameRef name="store" type="parameter"/>
				</plx:passParam>
				<plx:passParam>
					<plx:nameRef name="{$param1Name}" type="parameter"/>
				</plx:passParam>
				<plx:passParam>
					<plx:nameRef name="{$param2Name}" type="parameter"/>
				</plx:passParam>
				<plx:passParam>
					<plx:nameRef name="handler" type="parameter"/>
				</plx:passParam>
				<plx:passParam>
					<plx:callStatic name="Add" type="field" dataTypeName="EventHandlerAction"/>
				</plx:passParam>
			</plx:callStatic>
		</plx:function>

		<plx:function name="Remove{$AddRemoveNameDecorator}Handler" visibility="public" modifier="static" overload="true">
			<plx:leadingInfo>
				<plx:docComment>
					<summary><![CDATA[Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.]]></summary>
					<param name="store"><![CDATA[The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.]]></param>
					<param name="{$param1Name}"><![CDATA[The <paramref name="]]><xsl:value-of select="string($param1Name)"/><![CDATA["/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.]]></param>
					<param name="{$param2Name}"><![CDATA[The <paramref name="]]><xsl:value-of select="string($param2Name)"/><![CDATA["/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.]]></param>
					<param name="handler"><![CDATA[The <see cref="EventHandler{TEventArgs}"/> that should be removed.]]></param>
					<exception cref="ArgumentNullException"><![CDATA[<paramref name="store"/> is <see langword="null"/>.]]></exception>
					<exception cref="ObjectDisposedException"><![CDATA[<paramref name="store"/> has been disposed.]]></exception>
					<xsl:if test="not($param1Type='Guid')" xml:space="preserve"><exception cref="ArgumentNullException"><![CDATA[<paramref name="]]><xsl:value-of select="string($param1Name)"/><![CDATA["/> is <see langword="null"/>.]]></exception></xsl:if>
					<xsl:if test="not($param2Type='Guid')" xml:space="preserve"><exception cref="ArgumentNullException"><![CDATA[<paramref name="]]><xsl:value-of select="string($param2Name)"/><![CDATA["/> is <see langword="null"/>.]]></exception></xsl:if>
				</plx:docComment>
			</plx:leadingInfo>
			<plx:param name="store" dataTypeName="Store"/>
			<plx:param name="{$param1Name}" dataTypeName="{$param1Type}"/>
			<plx:param name="{$param2Name}" dataTypeName="{$param2Type}"/>
			<plx:param name="handler" dataTypeName="EventHandler">
				<plx:passTypeParam dataTypeName="{$EventArgsName}"/>
			</plx:param>
			<plx:callStatic name="AddOrRemove{$AddRemoveNameDecorator}Handler" dataTypeName="ModelingEventManager">
				<plx:passParam>
					<plx:nameRef name="store" type="parameter"/>
				</plx:passParam>
				<plx:passParam>
					<plx:nameRef name="{$param1Name}" type="parameter"/>
				</plx:passParam>
				<plx:passParam>
					<plx:nameRef name="{$param2Name}" type="parameter"/>
				</plx:passParam>
				<plx:passParam>
					<plx:nameRef name="handler" type="parameter"/>
				</plx:passParam>
				<plx:passParam>
					<plx:callStatic name="Remove" type="field" dataTypeName="EventHandlerAction"/>
				</plx:passParam>
			</plx:callStatic>
		</plx:function>

		<plx:function name="AddOrRemove{$AddRemoveNameDecorator}Handler" visibility="public" modifier="static" overload="true">
			<plx:leadingInfo>
				<plx:docComment>
					<summary><![CDATA[Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
as a handler for the <see cref="Store"/> specified by <paramref name="store"/>.]]></summary>
					<param name="store"><![CDATA[The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.]]></param>
					<param name="{$param1Name}"><![CDATA[The <paramref name="]]><xsl:value-of select="string($param1Name)"/><![CDATA["/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.]]></param>
					<param name="{$param2Name}"><![CDATA[The <paramref name="]]><xsl:value-of select="string($param2Name)"/><![CDATA["/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.]]></param>
					<param name="handler"><![CDATA[The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.]]></param>
					<param name="action"><![CDATA[The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
specified by <paramref name="handler"/>.]]></param>
					<exception cref="ArgumentNullException"><![CDATA[<paramref name="store"/> is <see langword="null"/>.]]></exception>
					<exception cref="ObjectDisposedException"><![CDATA[<paramref name="store"/> has been disposed.]]></exception>
					<xsl:if test="not($param1Type='Guid')" xml:space="preserve"><exception cref="ArgumentNullException"><![CDATA[<paramref name="]]><xsl:value-of select="string($param1Name)"/><![CDATA["/> is <see langword="null"/>.]]></exception></xsl:if>
					<xsl:if test="not($param2Type='Guid')" xml:space="preserve"><exception cref="ArgumentNullException"><![CDATA[<paramref name="]]><xsl:value-of select="string($param2Name)"/><![CDATA["/> is <see langword="null"/>.]]></exception></xsl:if>
				</plx:docComment>
			</plx:leadingInfo>
			<plx:attribute dataTypeName="EditorBrowsableAttribute">
				<plx:passParam>
					<plx:callStatic name="Advanced" type="field" dataTypeName="EditorBrowsableState"/>
				</plx:passParam>
			</plx:attribute>
			<plx:param name="store" dataTypeName="Store"/>
			<plx:param name="{$param1Name}" dataTypeName="{$param1Type}"/>
			<plx:param name="{$param2Name}" dataTypeName="{$param2Type}"/>
			<plx:param name="handler" dataTypeName="EventHandler">
				<plx:passTypeParam dataTypeName="{$EventArgsName}"/>
			</plx:param>
			<plx:param name="action" dataTypeName="EventHandlerAction"/>
			<xsl:if test="not($param1Type='Guid')">
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="identityEquality">
							<plx:left>
								<plx:nameRef name="{$param1Name}" type="parameter"/>
							</plx:left>
							<plx:right>
								<plx:nullKeyword/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:throw>
						<plx:callNew dataTypeName="ArgumentNullException">
							<plx:passParam>
								<plx:string data="{$param1Name}"/>
							</plx:passParam>
						</plx:callNew>
					</plx:throw>
				</plx:branch>
			</xsl:if>
			<xsl:if test="not($param2Type='Guid')">
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="identityEquality">
							<plx:left>
								<plx:nameRef name="{$param2Name}" type="parameter"/>
							</plx:left>
							<plx:right>
								<plx:nullKeyword/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:throw>
						<plx:callNew dataTypeName="ArgumentNullException">
							<plx:passParam>
								<plx:string data="{$param2Name}"/>
							</plx:passParam>
						</plx:callNew>
					</plx:throw>
				</plx:branch>
			</xsl:if>
			<plx:branch>
				<plx:condition>
					<plx:binaryOperator type="identityInequality">
						<plx:left>
							<plx:cast type="exceptionCast" dataTypeName=".object">
								<plx:nameRef name="handler" type="parameter"/>
							</plx:cast>
						</plx:left>
						<plx:right>
							<plx:nullKeyword/>
						</plx:right>
					</plx:binaryOperator>
				</plx:condition>
				<plx:local name="manager" dataTypeName="ModelingEventManager">
					<plx:initialize>
						<plx:callStatic name="GetModelingEventManager" dataTypeName="ModelingEventManager">
							<plx:passParam>
								<plx:nameRef name="store" type="parameter"/>
							</plx:passParam>
							<plx:passParam>
								<plx:nameRef name="action" type="parameter"/>
							</plx:passParam>
						</plx:callStatic>
					</plx:initialize>
				</plx:local>
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="identityInequality">
							<plx:left>
								<plx:nameRef name="manager" type="local"/>
							</plx:left>
							<plx:right>
								<plx:nullKeyword/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:callStatic name="AddOrRemoveHandler" dataTypeName="ModelingEventManager">
						<plx:passMemberTypeParam dataTypeName="GuidPair"/>
						<plx:passMemberTypeParam dataTypeName="{$EventArgsName}"/>
						<plx:passParam>
							<plx:callInstance name="Get{$PascalName}HandlersGuidPairDictionary">
								<plx:callObject>
									<plx:nameRef name="manager" type="local"/>
								</plx:callObject>
								<plx:passParam>
									<plx:nameRef name="action" type="parameter"/>
								</plx:passParam>
							</plx:callInstance>
						</plx:passParam>
						<plx:passParam>
							<plx:callNew dataTypeName="GuidPair">
								<plx:passParam>
									<xsl:choose>
										<xsl:when test="$param1Type='Guid'">
											<plx:nameRef name="{$param1Name}" type="parameter"/>
										</xsl:when>
										<xsl:otherwise>
											<plx:callInstance name="Id" type="property">
												<plx:callObject>
													<plx:nameRef name="{$param1Name}" type="parameter"/>
												</plx:callObject>
											</plx:callInstance>
										</xsl:otherwise>
									</xsl:choose>
								</plx:passParam>
								<plx:passParam>
									<xsl:choose>
										<xsl:when test="$param2Type='Guid'">
											<plx:nameRef name="{$param2Name}" type="parameter"/>
										</xsl:when>
										<xsl:otherwise>
											<plx:callInstance name="Id" type="property">
												<plx:callObject>
													<plx:nameRef name="{$param2Name}" type="parameter"/>
												</plx:callObject>
											</plx:callInstance>
										</xsl:otherwise>
									</xsl:choose>
								</plx:passParam>
							</plx:callNew>
						</plx:passParam>
						<plx:passParam>
							<plx:nameRef name="handler" type="parameter"/>
						</plx:passParam>
						<plx:passParam>
							<plx:nameRef name="action" type="parameter"/>
						</plx:passParam>
					</plx:callStatic>
				</plx:branch>
			</plx:branch>
		</plx:function>

		<!-- Do the instance versions -->

		<plx:function name="Add{$AddRemoveNameDecorator}Handler" visibility="public" overload="true">
			<plx:leadingInfo>
				<plx:docComment>
					<summary><![CDATA[Adds the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.]]></summary>
					<param name="{$param1Name}"><![CDATA[The <paramref name="]]><xsl:value-of select="string($param1Name)"/><![CDATA["/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.]]></param>
					<param name="{$param2Name}"><![CDATA[The <paramref name="]]><xsl:value-of select="string($param2Name)"/><![CDATA["/> for which the <see cref="EventHandler{TEventArgs}"/> should be added.]]></param>
					<param name="handler"><![CDATA[The <see cref="EventHandler{TEventArgs}"/> that should be added.]]></param>
					<xsl:if test="not($param1Type='Guid')" xml:space="preserve"><exception cref="ArgumentNullException"><![CDATA[<paramref name="]]><xsl:value-of select="string($param1Name)"/><![CDATA["/> is <see langword="null"/>.]]></exception></xsl:if>
					<xsl:if test="not($param2Type='Guid')" xml:space="preserve"><exception cref="ArgumentNullException"><![CDATA[<paramref name="]]><xsl:value-of select="string($param2Name)"/><![CDATA["/> is <see langword="null"/>.]]></exception></xsl:if>
				</plx:docComment>
			</plx:leadingInfo>
			<plx:param name="{$param1Name}" dataTypeName="{$param1Type}"/>
			<plx:param name="{$param2Name}" dataTypeName="{$param2Type}"/>
			<plx:param name="handler" dataTypeName="EventHandler">
				<plx:passTypeParam dataTypeName="{$EventArgsName}"/>
			</plx:param>
			<plx:callThis name="AddOrRemove{$AddRemoveNameDecorator}Handler">
				<plx:passParam>
					<plx:nameRef name="{$param1Name}" type="parameter"/>
				</plx:passParam>
				<plx:passParam>
					<plx:nameRef name="{$param2Name}" type="parameter"/>
				</plx:passParam>
				<plx:passParam>
					<plx:nameRef name="handler" type="parameter"/>
				</plx:passParam>
				<plx:passParam>
					<plx:callStatic name="Add" type="field" dataTypeName="EventHandlerAction"/>
				</plx:passParam>
			</plx:callThis>
		</plx:function>

		<plx:function name="Remove{$AddRemoveNameDecorator}Handler" visibility="public" overload="true">
			<plx:leadingInfo>
				<plx:docComment>
					<summary><![CDATA[Removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.]]></summary>
					<param name="{$param1Name}"><![CDATA[The <paramref name="]]><xsl:value-of select="string($param1Name)"/><![CDATA["/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.]]></param>
					<param name="{$param2Name}"><![CDATA[The <paramref name="]]><xsl:value-of select="string($param2Name)"/><![CDATA["/> for which the <see cref="EventHandler{TEventArgs}"/> should be removed.]]></param>
					<param name="handler"><![CDATA[The <see cref="EventHandler{TEventArgs}"/> that should be removed.]]></param>
					<xsl:if test="not($param1Type='Guid')" xml:space="preserve"><exception cref="ArgumentNullException"><![CDATA[<paramref name="]]><xsl:value-of select="string($param1Name)"/><![CDATA["/> is <see langword="null"/>.]]></exception></xsl:if>
					<xsl:if test="not($param2Type='Guid')" xml:space="preserve"><exception cref="ArgumentNullException"><![CDATA[<paramref name="]]><xsl:value-of select="string($param2Name)"/><![CDATA["/> is <see langword="null"/>.]]></exception></xsl:if>
				</plx:docComment>
			</plx:leadingInfo>
			<plx:param name="{$param1Name}" dataTypeName="{$param1Type}"/>
			<plx:param name="{$param2Name}" dataTypeName="{$param2Type}"/>
			<plx:param name="handler" dataTypeName="EventHandler">
				<plx:passTypeParam dataTypeName="{$EventArgsName}"/>
			</plx:param>
			<plx:callThis name="AddOrRemove{$AddRemoveNameDecorator}Handler">
				<plx:passParam>
					<plx:nameRef name="{$param1Name}" type="parameter"/>
				</plx:passParam>
				<plx:passParam>
					<plx:nameRef name="{$param2Name}" type="parameter"/>
				</plx:passParam>
				<plx:passParam>
					<plx:nameRef name="handler" type="parameter"/>
				</plx:passParam>
				<plx:passParam>
					<plx:callStatic name="Remove" type="field" dataTypeName="EventHandlerAction"/>
				</plx:passParam>
			</plx:callThis>
		</plx:function>

		<plx:function name="AddOrRemove{$AddRemoveNameDecorator}Handler" visibility="public" overload="true">
			<plx:leadingInfo>
				<plx:docComment>
					<summary><![CDATA[Adds or removes the <see cref="EventHandler{TEventArgs}"/> specified by <paramref name="handler"/>
as a handler for the <see cref="Store"/> specified when this <see cref="ModelingEventManager"/> was created.]]></summary>
					<param name="{$param1Name}"><![CDATA[The <paramref name="]]><xsl:value-of select="string($param1Name)"/><![CDATA["/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.]]></param>
					<param name="{$param2Name}"><![CDATA[The <paramref name="]]><xsl:value-of select="string($param2Name)"/><![CDATA["/> for which the <see cref="EventHandler{TEventArgs}"/> should be added or removed.]]></param>
					<param name="handler"><![CDATA[The <see cref="EventHandler{TEventArgs}"/> that should be added or removed.]]></param>
					<param name="action"><![CDATA[The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>
specified by <paramref name="handler"/>.]]></param>
					<xsl:if test="not($param1Type='Guid')" xml:space="preserve"><exception cref="ArgumentNullException"><![CDATA[<paramref name="]]><xsl:value-of select="string($param1Name)"/><![CDATA["/> is <see langword="null"/>.]]></exception></xsl:if>
					<xsl:if test="not($param2Type='Guid')" xml:space="preserve"><exception cref="ArgumentNullException"><![CDATA[<paramref name="]]><xsl:value-of select="string($param2Name)"/><![CDATA["/> is <see langword="null"/>.]]></exception></xsl:if>
				</plx:docComment>
			</plx:leadingInfo>
			<plx:attribute dataTypeName="EditorBrowsableAttribute">
				<plx:passParam>
					<plx:callStatic name="Advanced" type="field" dataTypeName="EditorBrowsableState"/>
				</plx:passParam>
			</plx:attribute>
			<plx:param name="{$param1Name}" dataTypeName="{$param1Type}"/>
			<plx:param name="{$param2Name}" dataTypeName="{$param2Type}"/>
			<plx:param name="handler" dataTypeName="EventHandler">
				<plx:passTypeParam dataTypeName="{$EventArgsName}"/>
			</plx:param>
			<plx:param name="action" dataTypeName="EventHandlerAction"/>
			<xsl:if test="not($param1Type='Guid')">
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="identityEquality">
							<plx:left>
								<plx:nameRef name="{$param1Name}" type="parameter"/>
							</plx:left>
							<plx:right>
								<plx:nullKeyword/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:throw>
						<plx:callNew dataTypeName="ArgumentNullException">
							<plx:passParam>
								<plx:string data="{$param1Name}"/>
							</plx:passParam>
						</plx:callNew>
					</plx:throw>
				</plx:branch>
			</xsl:if>
			<xsl:if test="not($param2Type='Guid')">
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="identityEquality">
							<plx:left>
								<plx:nameRef name="{$param2Name}" type="parameter"/>
							</plx:left>
							<plx:right>
								<plx:nullKeyword/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:throw>
						<plx:callNew dataTypeName="ArgumentNullException">
							<plx:passParam>
								<plx:string data="{$param2Name}"/>
							</plx:passParam>
						</plx:callNew>
					</plx:throw>
				</plx:branch>
			</xsl:if>
			<plx:branch>
				<plx:condition>
					<plx:binaryOperator type="identityInequality">
						<plx:left>
							<plx:cast type="exceptionCast" dataTypeName=".object">
								<plx:nameRef name="handler" type="parameter"/>
							</plx:cast>
						</plx:left>
						<plx:right>
							<plx:nullKeyword/>
						</plx:right>
					</plx:binaryOperator>
				</plx:condition>
				<plx:callStatic name="AddOrRemoveHandler" dataTypeName="ModelingEventManager">
					<plx:passMemberTypeParam dataTypeName="GuidPair"/>
					<plx:passMemberTypeParam dataTypeName="{$EventArgsName}"/>
					<plx:passParam>
						<plx:callThis name="Get{$PascalName}HandlersGuidPairDictionary">
							<plx:passParam>
								<plx:nameRef name="action" type="parameter"/>
							</plx:passParam>
						</plx:callThis>
					</plx:passParam>
					<plx:passParam>
						<plx:callNew dataTypeName="GuidPair">
							<plx:passParam>
								<xsl:choose>
									<xsl:when test="$param1Type='Guid'">
										<plx:nameRef name="{$param1Name}" type="parameter"/>
									</xsl:when>
									<xsl:otherwise>
										<plx:callInstance name="Id" type="property">
											<plx:callObject>
												<plx:nameRef name="{$param1Name}" type="parameter"/>
											</plx:callObject>
										</plx:callInstance>
									</xsl:otherwise>
								</xsl:choose>
							</plx:passParam>
							<plx:passParam>
								<xsl:choose>
									<xsl:when test="$param2Type='Guid'">
										<plx:nameRef name="{$param2Name}" type="parameter"/>
									</xsl:when>
									<xsl:otherwise>
										<plx:callInstance name="Id" type="property">
											<plx:callObject>
												<plx:nameRef name="{$param2Name}" type="parameter"/>
											</plx:callObject>
										</plx:callInstance>
									</xsl:otherwise>
								</xsl:choose>
							</plx:passParam>
						</plx:callNew>
					</plx:passParam>
					<plx:passParam>
						<plx:nameRef name="handler" type="parameter"/>
					</plx:passParam>
					<plx:passParam>
						<plx:nameRef name="action" type="parameter"/>
					</plx:passParam>
				</plx:callStatic>
			</plx:branch>
		</plx:function>
	</xsl:template>

	<xsl:template name="GetLocalsCode">
		<xsl:param name="Params"/>
		<xsl:variable name="distinctParamsFragment">
			<xsl:call-template name="GetDistinctParams">
				<xsl:with-param name="Params" select="$Params"/>
				<xsl:with-param name="DistinctParams" select="exsl:node-set('')/child::*"/>
				<xsl:with-param name="Index" select="1"/>
				<xsl:with-param name="Count" select="count($Params)"/>
			</xsl:call-template>
		</xsl:variable>
		<xsl:for-each select="exsl:node-set($distinctParamsFragment)/child::Param">
			<xsl:choose>
				<xsl:when test="@type='DomainClassInfo'">
					<plx:local name="{@name}" dataTypeName="DomainClassInfo">
						<plx:initialize>
							<plx:callInstance name="{@propertyName}" type="property">
								<plx:callObject>
									<plx:nameRef name="e" type="parameter"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:initialize>
					</plx:local>
				</xsl:when>
				<xsl:otherwise>
					<plx:local name="{@name}" dataTypeName="Guid">
						<plx:initialize>
							<xsl:apply-templates select="." mode="GetGuidFromEventArgs"/>
						</plx:initialize>
					</plx:local>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:for-each>
	</xsl:template>

	<xsl:template name="GetDistinctParams">
		<xsl:param name="Params"/>
		<xsl:param name="DistinctParams"/>
		<xsl:param name="Index"/>
		<xsl:param name="Count"/>
		<xsl:choose>
			<xsl:when test="$Index > $Count">
				<xsl:copy-of select="$DistinctParams"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:variable name="currentParam" select="$Params[$Index]"/>
				<xsl:variable name="distinctParamsFragment">
					<xsl:copy-of select="$DistinctParams"/>
					<xsl:if test="not($DistinctParams[@name=$currentParam/@name])">
						<xsl:copy-of select="$currentParam"/>
					</xsl:if>
				</xsl:variable>
				<xsl:call-template name="GetDistinctParams">
					<xsl:with-param name="Params" select="$Params"/>
					<xsl:with-param name="DistinctParams" select="exsl:node-set($distinctParamsFragment)/child::*"/>
					<xsl:with-param name="Index" select="$Index + 1"/>
					<xsl:with-param name="Count" select="$Count"/>
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="Param" mode="GetGuidFromEventArgs">
		<xsl:choose>
			<xsl:when test="@type='Guid'">
				<plx:callInstance name="{@propertyName}" type="property">
					<plx:callObject>
						<plx:nameRef name="e" type="parameter"/>
					</plx:callObject>
				</plx:callInstance>
			</xsl:when>
			<xsl:otherwise>
				<plx:callInstance name="Id" type="property">
					<plx:callObject>
						<plx:callInstance name="{@propertyName}" type="property">
							<plx:callObject>
								<plx:nameRef name="e" type="parameter"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:callObject>
				</plx:callInstance>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="GetDoWhileLoopCodeForDomainClassInfo">
		<xsl:param name="LocalName"/>
		<xsl:param name="BodyCode"/>
		<plx:loop checkCondition="after">
			<plx:condition>
				<plx:binaryOperator type="identityInequality">
					<plx:left>
						<plx:expression parens="true">
							<plx:inlineStatement dataTypeName="DomainClassInfo">
								<plx:assign>
									<plx:left>
										<plx:nameRef name="{$LocalName}" type="local"/>
									</plx:left>
									<plx:right>
										<plx:callInstance name="BaseDomainClass" type="property">
											<plx:callObject>
												<plx:nameRef name="{$LocalName}" type="local"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:right>
								</plx:assign>
							</plx:inlineStatement>
						</plx:expression>
					</plx:left>
					<plx:right>
						<plx:nullKeyword/>
					</plx:right>
				</plx:binaryOperator>
			</plx:condition>
			<xsl:copy-of select="$BodyCode"/>
		</plx:loop>
	</xsl:template>

</xsl:stylesheet>
