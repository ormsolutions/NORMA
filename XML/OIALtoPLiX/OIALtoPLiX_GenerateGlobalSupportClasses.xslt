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
			<xsl:if test="$GenerateCodeAnalysisAttributes">
				<plx:namespaceImport alias="SuppressMessageAttribute" name="System.Diagnostics.CodeAnalysis.SuppressMessageAttribute"/>
			</xsl:if>
			<plx:namespaceImport alias="GeneratedCodeAttribute" name="System.CodeDom.Compiler.GeneratedCodeAttribute"/>
			<plx:namespaceImport alias="StructLayoutAttribute" name="System.Runtime.InteropServices.StructLayoutAttribute"/>
			<plx:namespaceImport alias="LayoutKind" name="System.Runtime.InteropServices.LayoutKind"/>
			<plx:namespaceImport alias="CharSet" name="System.Runtime.InteropServices.CharSet"/>
			<xsl:call-template name="GenerateGlobalSupportClasses">
				<xsl:with-param name="StructLayoutAttribute" select="$StructLayoutAttribute"/>
			</xsl:call-template>
		</plx:root>
	</xsl:template>
	<!-- TODO: Determine if these classes are already available in the solution (or anything referenced by it) prior to generating them. -->
	<xsl:template name="GenerateGlobalSupportClasses">
		<xsl:param name="StructLayoutAttribute"/>
		<xsl:variable name="propertyChangeEventArgsClassBodyFragment">
			<plx:implementsInterface dataTypeName="IPropertyChangeEventArgs">
				<plx:passTypeParam dataTypeName="TClass"/>
				<plx:passTypeParam dataTypeName="TProperty"/>
			</plx:implementsInterface>
			<plx:field visibility="private" readOnly="true" name="_instance" dataTypeName="TClass"/>
			<plx:field visibility="private" readOnly="true" name="_oldValue" dataTypeName="TProperty"/>
			<plx:field visibility="private" readOnly="true" name="_newValue" dataTypeName="TProperty"/>
			<plx:function visibility="public" name=".construct">
				<plx:param name="instance" dataTypeName="TClass"/>
				<plx:param name="oldValue" dataTypeName="TProperty"/>
				<plx:param name="newValue" dataTypeName="TProperty"/>
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
				<plx:assign>
					<plx:left>
						<plx:callThis accessor="this" type="field" name="_instance"/>
					</plx:left>
					<plx:right>
						<plx:nameRef type="parameter" name="instance"/>
					</plx:right>
				</plx:assign>
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
		</xsl:variable>
		<xsl:variable name="propertyChangeEventArgsClassBody" select="exsl:node-set($propertyChangeEventArgsClassBodyFragment)/child::*"/>
		<plx:namespace name="System">
			<plx:leadingInfo>
				<plx:pragma type="region" data="Global Support Classes"/>
			</plx:leadingInfo>
			<plx:trailingInfo>
				<plx:pragma type="closeRegion" data="Global Support Classes"/>
			</plx:trailingInfo>
			<xsl:call-template name="GenerateCommonTuples"/>
			<plx:class visibility="public" modifier="static" name="EventHandlerUtility">
				<plx:leadingInfo>
					<plx:pragma type="region" data="Property Change Event Support"/>
				</plx:leadingInfo>

				<plx:function visibility="public" name="InvokeCancelableEventHandler" modifier="static">
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
										<plx:nameRef name="cancelableEventHandler" type="parameter"/>
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
										<plx:nameRef name="e" type="parameter"/>
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
									<plx:nameRef name="cancelableEventHandler" type="parameter"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:initialize>
					</plx:local>
					<plx:loop checkCondition="before">
						<plx:initializeLoop>
							<plx:local name="i" dataTypeName=".i4">
								<plx:initialize>
									<plx:value data="0" type="i4"/>
								</plx:initialize>
							</plx:local>
						</plx:initializeLoop>
						<plx:condition>
							<plx:binaryOperator type="booleanAnd">
								<plx:left>
									<plx:binaryOperator type="lessThan">
										<plx:left>
											<plx:nameRef name="i"/>
										</plx:left>
										<plx:right>
											<plx:callInstance name="Length" type="property">
												<plx:callObject>
													<plx:nameRef name="invocationList" type="local"/>
												</plx:callObject>
											</plx:callInstance>
										</plx:right>
									</plx:binaryOperator>
								</plx:left>
								<plx:right>
									<plx:unaryOperator type="booleanNot">
										<plx:callInstance name="Cancel" type="property">
											<plx:callObject>
												<plx:nameRef name="e" type="parameter"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:unaryOperator>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:beforeLoop>
							<plx:increment type="post">
								<plx:nameRef name="i" type="local"/>
							</plx:increment>
						</plx:beforeLoop>
						<plx:callInstance name="Invoke" type="methodCall">
							<plx:callObject>
								<plx:cast dataTypeName="EventHandler">
									<plx:passTypeParam dataTypeName="TEventArgs"/>
									<plx:callInstance name=".implied" type="arrayIndexer">
										<plx:callObject>
											<plx:nameRef name="invocationList" type="local"/>
										</plx:callObject>
										<plx:passParam>
											<plx:nameRef name="i" type="local"/>
										</plx:passParam>
									</plx:callInstance>
								</plx:cast>
							</plx:callObject>
							<plx:passParam>
								<plx:nameRef name="sender" type="parameter"/>
							</plx:passParam>
							<plx:passParam>
								<plx:nameRef name="e" type="parameter"/>
							</plx:passParam>
						</plx:callInstance>
					</plx:loop>
					<plx:return>
						<plx:unaryOperator type="booleanNot">
							<plx:callInstance name="Cancel" type="property">
								<plx:callObject>
									<plx:nameRef name="e" type="parameter"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:unaryOperator>
					</plx:return>
				</plx:function>

				<plx:function visibility="public" name="InvokeEventHandlerAsync" modifier="static">
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
										<plx:nameRef name="eventHandler" type="parameter"/>
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
							<plx:callInstance name="GetInvocationList" type="methodCall">
								<plx:callObject>
									<plx:nameRef name="eventHandler" type="parameter"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:initialize>
					</plx:local>
					<plx:loop checkCondition="before">
						<plx:initializeLoop>
							<plx:local name="i" dataTypeName=".i4">
								<plx:initialize>
									<plx:value data="0" type="i4"/>
								</plx:initialize>
							</plx:local>
						</plx:initializeLoop>
						<plx:condition>
							<plx:binaryOperator type="lessThan">
								<plx:left>
									<plx:nameRef name="i" type="local"/>
								</plx:left>
								<plx:right>
									<plx:callInstance name="Length" type="property">
										<plx:callObject>
											<plx:nameRef name="invocationList" type="local"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:beforeLoop>
							<plx:increment type="post">
								<plx:nameRef name="i" type="local"/>
							</plx:increment>
						</plx:beforeLoop>
						<plx:local name="currentEventHandler" dataTypeName="EventHandler">
							<plx:passTypeParam dataTypeName="TEventArgs"/>
							<plx:initialize>
								<plx:cast type="exceptionCast" dataTypeName="EventHandler">
									<plx:passTypeParam dataTypeName="TEventArgs"/>
									<plx:callInstance name=".implied" type="arrayIndexer">
										<plx:callObject>
											<plx:nameRef name="invocationList" type="local"/>
										</plx:callObject>
										<plx:passParam>
											<plx:nameRef name="i" type="local"/>
										</plx:passParam>
									</plx:callInstance>
								</plx:cast>
							</plx:initialize>
						</plx:local>
						<plx:callInstance name="BeginInvoke">
							<plx:callObject>
								<plx:nameRef name="currentEventHandler" type="local"/>
							</plx:callObject>
							<plx:passParam>
								<plx:nameRef name="sender" type="parameter"/>
							</plx:passParam>
							<plx:passParam>
								<plx:nameRef name="e" type="parameter"/>
							</plx:passParam>
							<plx:passParam>
								<plx:callNew dataTypeName="AsyncCallback">
									<plx:passParam>
										<plx:callInstance name="EndInvoke" type="methodReference">
											<plx:callObject>
												<plx:nameRef name="currentEventHandler" type="local"/>
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

				<plx:function visibility="public" name="InvokeEventHandlerAsync" modifier="static">
					<plx:param name="eventHandler" dataTypeName="PropertyChangedEventHandler"/>
					<plx:param name="sender" dataTypeName=".object"/>
					<plx:param name="e" dataTypeName="PropertyChangedEventArgs"/>
					<plx:branch>
						<plx:condition>
							<plx:binaryOperator type="identityEquality">
								<plx:left>
									<plx:cast type="exceptionCast" dataTypeName=".object">
										<plx:nameRef name="eventHandler" type="parameter"/>
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
							<plx:callInstance name="GetInvocationList" type="methodCall">
								<plx:callObject>
									<plx:nameRef name="eventHandler" type="parameter"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:initialize>
					</plx:local>
					<plx:loop checkCondition="before">
						<plx:initializeLoop>
							<plx:local name="i" dataTypeName=".i4">
								<plx:initialize>
									<plx:value data="0" type="i4"/>
								</plx:initialize>
							</plx:local>
						</plx:initializeLoop>
						<plx:condition>
							<plx:binaryOperator type="lessThan">
								<plx:left>
									<plx:nameRef name="i" type="local"/>
								</plx:left>
								<plx:right>
									<plx:callInstance name="Length" type="property">
										<plx:callObject>
											<plx:nameRef name="invocationList" type="local"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:beforeLoop>
							<plx:increment type="post">
								<plx:nameRef name="i" type="local"/>
							</plx:increment>
						</plx:beforeLoop>
						<plx:local name="currentEventHandler" dataTypeName="PropertyChangedEventHandler">
							<plx:initialize>
								<plx:cast type="exceptionCast" dataTypeName="PropertyChangedEventHandler">
									<plx:callInstance name=".implied" type="arrayIndexer">
										<plx:callObject>
											<plx:nameRef name="invocationList" type="local"/>
										</plx:callObject>
										<plx:passParam>
											<plx:nameRef name="i" type="local"/>
										</plx:passParam>
									</plx:callInstance>
								</plx:cast>
							</plx:initialize>
						</plx:local>
						<plx:callInstance name="BeginInvoke">
							<plx:callObject>
								<plx:nameRef name="currentEventHandler" type="local"/>
							</plx:callObject>
							<plx:passParam>
								<plx:nameRef name="sender" type="parameter"/>
							</plx:passParam>
							<plx:passParam>
								<plx:nameRef name="e" type="parameter"/>
							</plx:passParam>
							<plx:passParam>
								<plx:callNew dataTypeName="AsyncCallback">
									<plx:passParam>
										<plx:callInstance name="EndInvoke" type="methodReference">
											<plx:callObject>
												<plx:nameRef name="currentEventHandler" type="local"/>
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
				<plx:attribute dataTypeName="SuppressMessageAttribute">
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
				<plx:property visibility="public" modifier="abstract" name="OldValue">
					<plx:returns dataTypeName="TProperty"/>
					<plx:get/>
				</plx:property>
				<plx:property visibility="public" modifier="abstract" name="NewValue">
					<plx:returns dataTypeName="TProperty"/>
					<plx:get/>
				</plx:property>
			</plx:interface>
			<plx:class visibility="public" modifier="sealed" name="PropertyChangingEventArgs">
				<plx:attribute dataTypeName="Serializable"/>
				<xsl:copy-of select="$StructLayoutAttribute"/>
				<plx:typeParam name="TClass"/>
				<plx:typeParam name="TProperty"/>
				<plx:derivesFromClass dataTypeName="CancelEventArgs"/>
				<xsl:copy-of select="$propertyChangeEventArgsClassBody"/>
			</plx:class>
			<plx:class visibility="public" modifier="sealed" name="PropertyChangedEventArgs">
				<plx:trailingInfo>
					<plx:pragma type="closeRegion" data="Property Change Event Support"/>
				</plx:trailingInfo>
				<plx:attribute dataTypeName="Serializable"/>
				<xsl:copy-of select="$StructLayoutAttribute"/>
				<plx:typeParam name="TClass"/>
				<plx:typeParam name="TProperty"/>
				<plx:derivesFromClass dataTypeName="EventArgs"/>
				<xsl:copy-of select="$propertyChangeEventArgsClassBody"/>
			</plx:class>
		</plx:namespace>
	</xsl:template>

</xsl:stylesheet>
