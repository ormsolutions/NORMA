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
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:exsl="http://exslt.org/common"
	xmlns:plx="http://schemas.neumont.edu/CodeGeneration/PLiX"
	extension-element-prefixes="exsl">

	<xsl:import href="OIALtoPLiX_GenerateTuple.xslt"/>
	<xsl:import href="OIALtoPLiX_GlobalSupportParameters.xslt"/>
	<xsl:output method="xml" encoding="utf-8" media-type="text/xml" indent="yes"/>

	<xsl:template match="/">
		<plx:root>
			<plx:namespaceImport name="System"/>
			<plx:namespaceImport name="System.Collections.Generic"/>
			<plx:namespaceImport name="System.Collections.ObjectModel"/>
			<plx:namespaceImport name="System.ComponentModel"/>
			<plx:namespaceImport name="System.Xml"/>
			<xsl:call-template name="GenerateGlobalSupportClasses">
				<xsl:with-param name="StructLayoutAttribute" select="$StructLayoutAttribute"/>
			</xsl:call-template>
		</plx:root>
	</xsl:template>
	<!-- TODO: Determine if these classes are already available in the solution (or anything referenced by it) prior to generating them. -->
	<xsl:template name="GenerateGlobalSupportClasses">
		<xsl:param name="StructLayoutAttribute"/>
		<xsl:variable name="hostProtectionAttributeFragment">
			<plx:attribute dataTypeName="HostProtectionAttribute" dataTypeQualifier="System.Security.Permissions">
				<plx:passParam>
					<plx:callStatic type="field" name="LinkDemand" dataTypeName="SecurityAction" dataTypeQualifier="System.Security.Permissions"/>
				</plx:passParam>
				<plx:passParam>
					<plx:binaryOperator type="assignNamed">
						<plx:left>
							<plx:nameRef type="namedParameter" name="SharedState"/>
						</plx:left>
						<plx:right>
							<plx:trueKeyword/>
						</plx:right>
					</plx:binaryOperator>
				</plx:passParam>
			</plx:attribute>
		</xsl:variable>
		<xsl:variable name="hostProtectionAttribute" select="exsl:node-set($hostProtectionAttributeFragment)/child::*"/>
		<plx:namespace name="System">
			<plx:leadingInfo>
				<plx:pragma type="region" data="Global Support Classes"/>
			</plx:leadingInfo>
			<plx:trailingInfo>
				<plx:pragma type="closeRegion" data="Global Support Classes"/>
			</plx:trailingInfo>
			<xsl:call-template name="GenerateCommonTuples"/>
			<plx:namespace name="ComponentModel">
				<plx:leadingInfo>
					<plx:pragma type="region" data="Property Change Event Support"/>
				</plx:leadingInfo>
				<plx:trailingInfo>
					<plx:pragma type="closeRegion" data="Property Change Event Support"/>
				</plx:trailingInfo>

				<plx:class visibility="public" modifier="static" name="EventHandlerUtility">
					<xsl:copy-of select="$hostProtectionAttribute"/>

					<plx:function visibility="public" modifier="static" name="InvokeCancelableEventHandler">
						<plx:typeParam name="TEventArgs">
							<plx:typeConstraint dataTypeName="CancelEventArgs"/>
						</plx:typeParam>
						<plx:param name="cancelableEventHandler" dataTypeName="EventHandler">
							<plx:passTypeParam dataTypeName="TEventArgs"/>
						</plx:param>
						<plx:param name="sender" dataTypeName=".object"/>
						<plx:param name="e" dataTypeName="TEventArgs"/>
						<plx:returns dataTypeName=".boolean"/>
						<plx:branch>
							<plx:condition>
								<plx:binaryOperator type="identityEquality">
									<plx:left>
										<plx:cast type="exceptionCast" dataTypeName=".object">
											<plx:nameRef type="parameter" name="cancelableEventHandler"/>
										</plx:cast>
									</plx:left>
									<plx:right>
										<plx:nullKeyword/>
									</plx:right>
								</plx:binaryOperator>
							</plx:condition>
							<plx:throw>
								<plx:callNew dataTypeName="ArgumentNullException">
									<plx:passParam>
										<plx:string data="cancelableEventHandler"/>
									</plx:passParam>
								</plx:callNew>
							</plx:throw>
						</plx:branch>
						<plx:branch>
							<plx:condition>
								<plx:binaryOperator type="identityEquality">
									<plx:left>
										<plx:cast type="exceptionCast" dataTypeName=".object">
											<plx:nameRef type="parameter" name="e"/>
										</plx:cast>
									</plx:left>
									<plx:right>
										<plx:nullKeyword/>
									</plx:right>
								</plx:binaryOperator>
							</plx:condition>
							<plx:throw>
								<plx:callNew dataTypeName="ArgumentNullException">
									<plx:passParam>
										<plx:string data="e"/>
									</plx:passParam>
								</plx:callNew>
							</plx:throw>
						</plx:branch>
						<!-- PLIX_TODO: Once the PLiX formatters support keyword filtering, remove the dataTypeQualifier attribute from the next line. -->
						<plx:local name="invocationList" dataTypeName="Delegate" dataTypeQualifier="System" dataTypeIsSimpleArray="true">
							<plx:initialize>
								<plx:callInstance name="GetInvocationList">
									<plx:callObject>
										<plx:nameRef type="parameter" name="cancelableEventHandler"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:initialize>
						</plx:local>
						<plx:loop checkCondition="before">
							<plx:initializeLoop>
								<plx:local name="i" dataTypeName=".i4">
									<plx:initialize>
										<plx:value type="i4" data="0"/>
									</plx:initialize>
								</plx:local>
							</plx:initializeLoop>
							<plx:condition>
								<plx:binaryOperator type="booleanAnd">
									<plx:left>
										<plx:binaryOperator type="lessThan">
											<plx:left>
												<plx:nameRef type="local" name="i"/>
											</plx:left>
											<plx:right>
												<plx:callInstance type="property" name="Length">
													<plx:callObject>
														<plx:nameRef type="local" name="invocationList"/>
													</plx:callObject>
												</plx:callInstance>
											</plx:right>
										</plx:binaryOperator>
									</plx:left>
									<plx:right>
										<plx:unaryOperator type="booleanNot">
											<plx:callInstance type="property" name="Cancel">
												<plx:callObject>
													<plx:nameRef type="parameter" name="e"/>
												</plx:callObject>
											</plx:callInstance>
										</plx:unaryOperator>
									</plx:right>
								</plx:binaryOperator>
							</plx:condition>
							<plx:beforeLoop>
								<plx:increment type="post">
									<plx:nameRef type="local" name="i"/>
								</plx:increment>
							</plx:beforeLoop>
							<plx:callInstance type="methodCall" name="Invoke">
								<plx:callObject>
									<plx:cast type="exceptionCast" dataTypeName="EventHandler">
										<plx:passTypeParam dataTypeName="TEventArgs"/>
										<plx:callInstance type="arrayIndexer" name=".implied">
											<plx:callObject>
												<plx:nameRef type="local" name="invocationList"/>
											</plx:callObject>
											<plx:passParam>
												<plx:nameRef type="local" name="i"/>
											</plx:passParam>
										</plx:callInstance>
									</plx:cast>
								</plx:callObject>
								<plx:passParam>
									<plx:nameRef type="parameter" name="sender"/>
								</plx:passParam>
								<plx:passParam>
									<plx:nameRef type="parameter" name="e"/>
								</plx:passParam>
							</plx:callInstance>
						</plx:loop>
						<plx:return>
							<plx:unaryOperator type="booleanNot">
								<plx:callInstance type="property" name="Cancel">
									<plx:callObject>
										<plx:nameRef type="parameter" name="e"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:unaryOperator>
						</plx:return>
					</plx:function>

					<plx:function visibility="public" modifier="static" name="InvokeEventHandlerAsync">
						<plx:typeParam name="TEventArgs">
							<plx:typeConstraint dataTypeName="EventArgs"/>
						</plx:typeParam>
						<plx:param name="eventHandler" dataTypeName="EventHandler">
							<plx:passTypeParam dataTypeName="TEventArgs"/>
						</plx:param>
						<plx:param name="sender" dataTypeName=".object"/>
						<plx:param name="e" dataTypeName="TEventArgs"/>
						<plx:branch>
							<plx:condition>
								<plx:binaryOperator type="identityEquality">
									<plx:left>
										<plx:cast type="exceptionCast" dataTypeName=".object">
											<plx:nameRef type="parameter" name="eventHandler"/>
										</plx:cast>
									</plx:left>
									<plx:right>
										<plx:nullKeyword/>
									</plx:right>
								</plx:binaryOperator>
							</plx:condition>
							<plx:throw>
								<plx:callNew dataTypeName="ArgumentNullException">
									<plx:passParam>
										<plx:string data="eventHandler"/>
									</plx:passParam>
								</plx:callNew>
							</plx:throw>
						</plx:branch>
						<!-- PLIX_TODO: Once the PLiX formatters support keyword filtering, remove the dataTypeQualifier attribute from the next line. -->
						<plx:local name="invocationList" dataTypeName="Delegate" dataTypeQualifier="System" dataTypeIsSimpleArray="true">
							<plx:initialize>
								<plx:callInstance type="methodCall" name="GetInvocationList">
									<plx:callObject>
										<plx:nameRef type="parameter" name="eventHandler"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:initialize>
						</plx:local>
						<plx:loop checkCondition="before">
							<plx:initializeLoop>
								<plx:local name="i" dataTypeName=".i4">
									<plx:initialize>
										<plx:value type="i4" data="0"/>
									</plx:initialize>
								</plx:local>
							</plx:initializeLoop>
							<plx:condition>
								<plx:binaryOperator type="lessThan">
									<plx:left>
										<plx:nameRef type="local" name="i"/>
									</plx:left>
									<plx:right>
										<plx:callInstance type="property" name="Length">
											<plx:callObject>
												<plx:nameRef type="local" name="invocationList"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:right>
								</plx:binaryOperator>
							</plx:condition>
							<plx:beforeLoop>
								<plx:increment type="post">
									<plx:nameRef type="local" name="i"/>
								</plx:increment>
							</plx:beforeLoop>
							<plx:local name="currentEventHandler" dataTypeName="EventHandler">
								<plx:passTypeParam dataTypeName="TEventArgs"/>
								<plx:initialize>
									<plx:cast type="exceptionCast" dataTypeName="EventHandler">
										<plx:passTypeParam dataTypeName="TEventArgs"/>
										<plx:callInstance type="arrayIndexer" name=".implied">
											<plx:callObject>
												<plx:nameRef type="local" name="invocationList"/>
											</plx:callObject>
											<plx:passParam>
												<plx:nameRef type="local" name="i"/>
											</plx:passParam>
										</plx:callInstance>
									</plx:cast>
								</plx:initialize>
							</plx:local>
							<plx:callInstance type="methodCall" name="BeginInvoke">
								<plx:callObject>
									<plx:nameRef type="local" name="currentEventHandler"/>
								</plx:callObject>
								<plx:passParam>
									<plx:nameRef type="parameter" name="sender"/>
								</plx:passParam>
								<plx:passParam>
									<plx:nameRef type="parameter" name="e"/>
								</plx:passParam>
								<plx:passParam>
									<plx:callNew dataTypeName="AsyncCallback">
										<plx:passParam>
											<plx:callInstance type="methodReference" name="EndInvoke">
												<plx:callObject>
													<plx:nameRef type="local" name="currentEventHandler"/>
												</plx:callObject>
											</plx:callInstance>
										</plx:passParam>
									</plx:callNew>
								</plx:passParam>
								<plx:passParam>
									<plx:nullKeyword/>
								</plx:passParam>
							</plx:callInstance>
						</plx:loop>
					</plx:function>

					<plx:function visibility="public" modifier="static" name="InvokeEventHandlerAsync">
						<plx:typeParam name="TEventArgs">
							<plx:typeConstraint dataTypeName="PropertyChangedEventArgs"/>
						</plx:typeParam>
						<plx:param name="eventHandler" dataTypeName="EventHandler">
							<plx:passTypeParam dataTypeName="TEventArgs"/>
						</plx:param>
						<plx:param name="sender" dataTypeName=".object"/>
						<plx:param name="e" dataTypeName="TEventArgs"/>
						<plx:param name="secondaryHandler" dataTypeName="PropertyChangedEventHandler"/>
						<plx:callStatic type="methodCall" name="InvokeEventHandlerAsync" dataTypeName="EventHandlerUtility">
							<plx:passMemberTypeParam dataTypeName="TEventArgs"/>
							<plx:passParam>
								<plx:nameRef type="parameter" name="eventHandler"/>
							</plx:passParam>
							<plx:passParam>
								<plx:nameRef type="parameter" name="sender"/>
							</plx:passParam>
							<plx:passParam>
								<plx:nameRef type="parameter" name="e"/>
							</plx:passParam>
						</plx:callStatic>
						<plx:branch>
							<plx:condition>
								<plx:binaryOperator type="identityInequality">
									<plx:left>
										<plx:cast type="exceptionCast" dataTypeName=".object">
											<plx:nameRef type="parameter" name="secondaryHandler"/>
										</plx:cast>
									</plx:left>
									<plx:right>
										<plx:nullKeyword/>
									</plx:right>
								</plx:binaryOperator>
							</plx:condition>
							<plx:callStatic type="methodCall" name="InvokeEventHandlerAsync" dataTypeName="EventHandlerUtility">
								<plx:passParam>
									<plx:nameRef type="parameter" name="secondaryHandler"/>
								</plx:passParam>
								<plx:passParam>
									<plx:nameRef type="parameter" name="sender"/>
								</plx:passParam>
								<plx:passParam>
									<plx:nameRef type="parameter" name="e"/>
								</plx:passParam>
							</plx:callStatic>
						</plx:branch>
						
					</plx:function>

					<plx:function visibility="public" modifier="static" name="InvokeEventHandlerAsync">
						<plx:param name="eventHandler" dataTypeName="PropertyChangedEventHandler"/>
						<plx:param name="sender" dataTypeName=".object"/>
						<plx:param name="e" dataTypeName="PropertyChangedEventArgs"/>
						<plx:branch>
							<plx:condition>
								<plx:binaryOperator type="identityEquality">
									<plx:left>
										<plx:cast type="exceptionCast" dataTypeName=".object">
											<plx:nameRef type="parameter" name="eventHandler"/>
										</plx:cast>
									</plx:left>
									<plx:right>
										<plx:nullKeyword/>
									</plx:right>
								</plx:binaryOperator>
							</plx:condition>
							<plx:throw>
								<plx:callNew dataTypeName="ArgumentNullException">
									<plx:passParam>
										<plx:string data="eventHandler"/>
									</plx:passParam>
								</plx:callNew>
							</plx:throw>
						</plx:branch>
						<!-- PLIX_TODO: Once the PLiX formatters support keyword filtering, remove the dataTypeQualifier attribute from the next line. -->
						<plx:local name="invocationList" dataTypeName="Delegate" dataTypeQualifier="System" dataTypeIsSimpleArray="true">
							<plx:initialize>
								<plx:callInstance type="methodCall" name="GetInvocationList">
									<plx:callObject>
										<plx:nameRef type="parameter" name="eventHandler"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:initialize>
						</plx:local>
						<plx:loop checkCondition="before">
							<plx:initializeLoop>
								<plx:local name="i" dataTypeName=".i4">
									<plx:initialize>
										<plx:value type="i4" data="0"/>
									</plx:initialize>
								</plx:local>
							</plx:initializeLoop>
							<plx:condition>
								<plx:binaryOperator type="lessThan">
									<plx:left>
										<plx:nameRef type="local" name="i"/>
									</plx:left>
									<plx:right>
										<plx:callInstance type="property" name="Length">
											<plx:callObject>
												<plx:nameRef type="local" name="invocationList"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:right>
								</plx:binaryOperator>
							</plx:condition>
							<plx:beforeLoop>
								<plx:increment type="post">
									<plx:nameRef type="local" name="i"/>
								</plx:increment>
							</plx:beforeLoop>
							<plx:local name="currentEventHandler" dataTypeName="PropertyChangedEventHandler">
								<plx:initialize>
									<plx:cast type="exceptionCast" dataTypeName="PropertyChangedEventHandler">
										<plx:callInstance type="arrayIndexer" name=".implied">
											<plx:callObject>
												<plx:nameRef type="local" name="invocationList"/>
											</plx:callObject>
											<plx:passParam>
												<plx:nameRef type="local" name="i"/>
											</plx:passParam>
										</plx:callInstance>
									</plx:cast>
								</plx:initialize>
							</plx:local>
							<plx:callInstance type="methodCall" name="BeginInvoke">
								<plx:callObject>
									<plx:nameRef type="local" name="currentEventHandler"/>
								</plx:callObject>
								<plx:passParam>
									<plx:nameRef type="parameter" name="sender"/>
								</plx:passParam>
								<plx:passParam>
									<plx:nameRef type="parameter" name="e"/>
								</plx:passParam>
								<plx:passParam>
									<plx:callNew dataTypeName="AsyncCallback">
										<plx:passParam>
											<plx:callInstance type="methodReference" name="EndInvoke">
												<plx:callObject>
													<plx:nameRef type="local" name="currentEventHandler"/>
												</plx:callObject>
											</plx:callInstance>
										</plx:passParam>
									</plx:callNew>
								</plx:passParam>
								<plx:passParam>
									<plx:nullKeyword/>
								</plx:passParam>
							</plx:callInstance>
						</plx:loop>
					</plx:function>

				</plx:class>
				<plx:interface visibility="public" name="IPropertyChangeEventArgs">
					<plx:attribute dataTypeName="SuppressMessageAttribute" dataTypeQualifier="System.Diagnostics.CodeAnalysis">
						<plx:passParam>
							<plx:string data="Microsoft.Naming"/>
						</plx:passParam>
						<plx:passParam>
							<plx:string data="CA1711:IdentifiersShouldNotHaveIncorrectSuffix"/>
						</plx:passParam>
					</plx:attribute>
					<plx:typeParam name="TClass"/>
					<plx:typeParam name="TProperty"/>
					<plx:property visibility="public" modifier="abstract" name="Instance">
						<plx:returns dataTypeName="TClass"/>
						<plx:get/>
					</plx:property>
					<plx:property visibility="public" modifier="abstract" name="PropertyName">
						<plx:returns dataTypeName=".string"/>
						<plx:get/>
					</plx:property>
					<plx:property visibility="public" modifier="abstract" name="OldValue">
						<plx:returns dataTypeName="TProperty"/>
						<plx:get/>
					</plx:property>
					<plx:property visibility="public" modifier="abstract" name="NewValue">
						<plx:returns dataTypeName="TProperty"/>
						<plx:get/>
					</plx:property>
				</plx:interface>
				<xsl:call-template name="GeneratePropertyChangeEventArgsClass">
					<xsl:with-param name="HostProtectionAttribute" select="$hostProtectionAttribute"/>
					<xsl:with-param name="StructLayoutAttribute" select="$StructLayoutAttribute"/>
					<xsl:with-param name="ClassName" select="'PropertyChangingEventArgs'"/>
					<xsl:with-param name="BaseClassName" select="'CancelEventArgs'"/>
					<xsl:with-param name="BaseClassHasPropertyName" select="false()"/>
				</xsl:call-template>
				<xsl:call-template name="GeneratePropertyChangeEventArgsClass">
					<xsl:with-param name="HostProtectionAttribute" select="$hostProtectionAttribute"/>
					<xsl:with-param name="StructLayoutAttribute" select="$StructLayoutAttribute"/>
					<xsl:with-param name="ClassName" select="'PropertyChangedEventArgs'"/>
					<xsl:with-param name="BaseClassName" select="'PropertyChangedEventArgs'"/>
					<xsl:with-param name="BaseClassHasPropertyName" select="true()"/>
				</xsl:call-template>
			</plx:namespace>
		</plx:namespace>
	</xsl:template>

	<xsl:template name="GeneratePropertyChangeEventArgsClass">
		<xsl:param name="HostProtectionAttribute"/>
		<xsl:param name="StructLayoutAttribute"/>
		<xsl:param name="ClassName"/>
		<xsl:param name="BaseClassName"/>
		<xsl:param name="BaseClassHasPropertyName"/>

		<plx:class visibility="public" name="{$ClassName}">
			<plx:attribute dataTypeName="SerializableAttribute"/>
			<xsl:copy-of select="$HostProtectionAttribute"/>
			<xsl:copy-of select="$StructLayoutAttribute"/>
			<plx:typeParam name="TClass"/>
			<plx:typeParam name="TProperty"/>
			<plx:derivesFromClass dataTypeName="{$BaseClassName}"/>
			<plx:implementsInterface dataTypeName="IPropertyChangeEventArgs">
				<plx:passTypeParam dataTypeName="TClass"/>
				<plx:passTypeParam dataTypeName="TProperty"/>
			</plx:implementsInterface>
			<plx:field visibility="private" readOnly="true" name="_instance" dataTypeName="TClass"/>
			<xsl:if test="not($BaseClassHasPropertyName)">
				<plx:field visibility="private" readOnly="true" name="_propertyName" dataTypeName=".string"/>
			</xsl:if>
			<plx:field visibility="private" readOnly="true" name="_oldValue" dataTypeName="TProperty"/>
			<plx:field visibility="private" readOnly="true" name="_newValue" dataTypeName="TProperty"/>
			<plx:function visibility="public" name=".construct">
				<plx:param name="instance" dataTypeName="TClass"/>
				<plx:param name="propertyName" dataTypeName=".string"/>
				<plx:param name="oldValue" dataTypeName="TProperty"/>
				<plx:param name="newValue" dataTypeName="TProperty"/>
				<xsl:if test="$BaseClassHasPropertyName">
					<plx:initialize>
						<plx:callThis accessor="base" type="methodCall" name=".implied">
							<plx:passParam>
								<plx:nameRef type="parameter" name="propertyName"/>
							</plx:passParam>
						</plx:callThis>
					</plx:initialize>
				</xsl:if>
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="identityEquality">
							<plx:left>
								<plx:nameRef type="parameter" name="instance"/>
							</plx:left>
							<plx:right>
								<plx:nullKeyword/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:throw>
						<plx:callNew dataTypeName="ArgumentNullException">
							<plx:passParam>
								<plx:string data="instance"/>
							</plx:passParam>
						</plx:callNew>
					</plx:throw>
				</plx:branch>
				<plx:branch>
					<plx:condition>
						<plx:callStatic type="methodCall" name="IsNullOrEmpty" dataTypeName=".string">
							<plx:passParam>
								<plx:nameRef type="parameter" name="propertyName"/>
							</plx:passParam>
						</plx:callStatic>
					</plx:condition>
					<plx:throw>
						<plx:callNew dataTypeName="ArgumentNullException">
							<plx:passParam>
								<plx:string data="propertyName"/>
							</plx:passParam>
						</plx:callNew>
					</plx:throw>
				</plx:branch>
				<plx:assign>
					<plx:left>
						<plx:callThis accessor="this" type="field" name="_instance"/>
					</plx:left>
					<plx:right>
						<plx:nameRef type="parameter" name="instance"/>
					</plx:right>
				</plx:assign>
				<xsl:if test="not($BaseClassHasPropertyName)">
					<plx:assign>
						<plx:left>
							<plx:callThis accessor="this" type="field" name="_propertyName"/>
						</plx:left>
						<plx:right>
							<plx:nameRef type="parameter" name="propertyName"/>
						</plx:right>
					</plx:assign>	
				</xsl:if>
				<plx:assign>
					<plx:left>
						<plx:callThis accessor="this" type="field" name="_oldValue"/>
					</plx:left>
					<plx:right>
						<plx:nameRef type="parameter" name="oldValue"/>
					</plx:right>
				</plx:assign>
				<plx:assign>
					<plx:left>
						<plx:callThis accessor="this" type="field" name="_newValue"/>
					</plx:left>
					<plx:right>
						<plx:nameRef type="parameter" name="newValue"/>
					</plx:right>
				</plx:assign>
			</plx:function>
			<plx:property visibility="public" name="Instance">
				<plx:interfaceMember memberName="Instance" dataTypeName="IPropertyChangeEventArgs">
					<plx:passTypeParam dataTypeName="TClass"/>
					<plx:passTypeParam dataTypeName="TProperty"/>
				</plx:interfaceMember>
				<plx:returns dataTypeName="TClass"/>
				<plx:get>
					<plx:return>
						<plx:callThis accessor="this" type="field" name="_instance"/>
					</plx:return>
				</plx:get>
			</plx:property>
			<xsl:choose>
				<xsl:when test="$BaseClassHasPropertyName">
					<!-- Why is this here? Two reasons: -->
					<!-- First, sealing it prevents other subclasses from changing the meaning of PropertyName, and potentially allows for better performance. -->
					<!-- Second, and more importantly, the Visual Basic compiler is not smart enough to realize that the base class already implements this interface member. -->
					<plx:property visibility="public" modifier="sealedOverride" name="PropertyName">
						<plx:interfaceMember memberName="PropertyName" dataTypeName="IPropertyChangeEventArgs">
							<plx:passTypeParam dataTypeName="TClass"/>
							<plx:passTypeParam dataTypeName="TProperty"/>
						</plx:interfaceMember>
						<plx:returns dataTypeName=".string"/>
						<plx:get>
							<plx:return>
								<plx:callThis accessor="base" type="property" name="PropertyName"/>
							</plx:return>
						</plx:get>
					</plx:property>
				</xsl:when>
				<xsl:otherwise>
					<plx:property visibility="public" name="PropertyName">
						<plx:interfaceMember memberName="PropertyName" dataTypeName="IPropertyChangeEventArgs">
							<plx:passTypeParam dataTypeName="TClass"/>
							<plx:passTypeParam dataTypeName="TProperty"/>
						</plx:interfaceMember>
						<plx:returns dataTypeName=".string"/>
						<plx:get>
							<plx:return>
								<plx:callThis accessor="this" type="field" name="_propertyName"/>
							</plx:return>
						</plx:get>
					</plx:property>
				</xsl:otherwise>
			</xsl:choose>
			<plx:property visibility="public" name="OldValue">
				<plx:interfaceMember memberName="OldValue" dataTypeName="IPropertyChangeEventArgs">
					<plx:passTypeParam dataTypeName="TClass"/>
					<plx:passTypeParam dataTypeName="TProperty"/>
				</plx:interfaceMember>
				<plx:returns dataTypeName="TProperty"/>
				<plx:get>
					<plx:return>
						<plx:callThis accessor="this" type="field" name="_oldValue"/>
					</plx:return>
				</plx:get>
			</plx:property>
			<plx:property visibility="public" name="NewValue">
				<plx:interfaceMember memberName="NewValue" dataTypeName="IPropertyChangeEventArgs">
					<plx:passTypeParam dataTypeName="TClass"/>
					<plx:passTypeParam dataTypeName="TProperty"/>
				</plx:interfaceMember>
				<plx:returns dataTypeName="TProperty"/>
				<plx:get>
					<plx:return>
						<plx:callThis accessor="this" type="field" name="_newValue"/>
					</plx:return>
				</plx:get>
			</plx:property>
		</plx:class>

	</xsl:template>

</xsl:stylesheet>
