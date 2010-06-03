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
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:plx="http://schemas.neumont.edu/CodeGeneration/PLiX"
	xmlns:se="http://schemas.neumont.edu/ORM/SDK/SerializationExtensions"
	xmlns:exsl="http://exslt.org/common">
	<!-- Indenting is useful for debugging the transform, but a waste of memory at generation time -->
	<xsl:output method="xml" encoding="utf-8" indent="no"/>

	<!-- Pick up param value supplied automatically by plix loader -->
	<xsl:param name="CustomToolNamespace" select="'TestNamespace'"/>
	<xsl:template match="se:CustomSerializedElements">
		<plx:root>
			<plx:namespaceImport name="System"/>
			<plx:namespaceImport name="System.Collections"/>
			<plx:namespaceImport name="System.Collections.ObjectModel"/>
			<plx:namespaceImport name="System.Collections.Generic"/>
			<plx:namespaceImport name="Microsoft.VisualStudio.Modeling"/>
			<plx:namespaceImport name="Microsoft.VisualStudio.Modeling.Diagrams"/>
			<plx:namespaceImport name="ORMSolutions.ORMArchitect.Framework.Shell"/>
			<plx:namespace name="{$CustomToolNamespace}">
				<xsl:if test="se:Copyright">
					<plx:leadingInfo>
						<plx:comment blankLine="true"/>
						<plx:comment>
							<xsl:value-of select="se:Copyright/@name"/>
						</plx:comment>
						<xsl:for-each select="se:Copyright/se:CopyrightLine">
							<plx:comment>
								<xsl:value-of select="."/>
							</plx:comment>
						</xsl:for-each>
						<plx:comment blankLine="true"/>
					</plx:leadingInfo>
				</xsl:if>
				<xsl:variable name="namespaces" select="se:DomainModel/se:Namespaces/se:Namespace"/>
				<xsl:variable name="defaultNamespaceFragment">
					<xsl:variable name="explicitDefaultNamespace" select="$namespaces[@DefaultPrefix='true' or @DefaultPrefix='1']"/>
					<xsl:choose>
						<xsl:when test="$explicitDefaultNamespace">
							<xsl:copy-of select="$explicitDefaultNamespace[1]"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:copy-of select="$namespaces[1]"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:apply-templates select="child::*">
					<xsl:with-param name="namespaces" select="$namespaces"/>
					<xsl:with-param name="defaultNamespace" select="exsl:node-set($defaultNamespaceFragment)/child::*"/>
				</xsl:apply-templates>
			</plx:namespace>
		</plx:root>
	</xsl:template>
	<xsl:template match="se:Copyright"/>
	<xsl:template match="se:Element">
		<xsl:param name="namespaces"/>
		<xsl:param name="defaultNamespace"/>
		<xsl:variable name="ClassName" select="@Class"/>
		<xsl:variable name="ClassOverride" select="@Override='true'"/>
		<xsl:variable name="ClassSealed" select="@Sealed='true'"/>
		<xsl:variable name="InterfaceImplementationVisibility">
			<xsl:choose>
				<xsl:when test="$ClassSealed">
					<xsl:value-of select="'privateInterfaceMember'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="'protected'"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<plx:class name="{$ClassName}" visibility="deferToPartial" partial="true">
			<xsl:if test="$ClassSealed">
				<!-- Make sure it is really sealed by also rendering the modifier here. -->
				<xsl:attribute name="modifier">
					<xsl:value-of select="'sealed'"/>
				</xsl:attribute>
			</xsl:if>
			<plx:leadingInfo>
				<plx:pragma type="region" data="{$ClassName} serialization"/>
			</plx:leadingInfo>
			<plx:trailingInfo>
				<plx:pragma type="closeRegion" data="{$ClassName} serialization"/>
			</plx:trailingInfo>
			<plx:implementsInterface dataTypeName="ICustomSerializedElement"/>
			<plx:property visibility="{$InterfaceImplementationVisibility}" name="SupportedCustomSerializedOperations" replacesName="{$ClassOverride}">
				<plx:leadingInfo>
					<plx:docComment>
						<summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:interfaceMember dataTypeName="ICustomSerializedElement" memberName="SupportedCustomSerializedOperations"/>
				<plx:returns dataTypeName="CustomSerializedElementSupportedOperations"/>
				<plx:get>
					<xsl:variable name="currentSupport">
						<xsl:call-template name="ReturnCustomSerializedElementSupportedOperations">
							<xsl:with-param name="containerElements" select="boolean(se:Container)"/>
							<xsl:with-param name="element" select="count(@Prefix)+count(@Name)+count(@Namespace)+count(@WriteStyle)+count(@DoubleTagName)+count(se:ConditionalName)"/>
							<xsl:with-param name="attributes" select="boolean(se:Attribute)"/>
							<xsl:with-param name="links" select="boolean(se:Link | se:StandaloneLink)"/>
							<xsl:with-param name="aggregatingLinks" select="boolean(se:Link[@WriteStyle='EmbeddingLinkElement'])"/>
							<xsl:with-param name="customSort" select="@SortChildElements='true'"/>
							<xsl:with-param name="mixedTypedAttributes" select="boolean(se:Attribute[@WriteStyle='Element' or @WriteStyle='DoubleTaggedElement'][not(@ReadOnly='true' or @ReadOnly='1')])"/>
						</xsl:call-template>
					</xsl:variable>
					<plx:return>
						<xsl:choose>
							<xsl:when test="$ClassOverride">
								<plx:binaryOperator type="bitwiseOr">
									<plx:left>
										<plx:callThis accessor="base" name="SupportedCustomSerializedOperations" type="property"/>
									</plx:left>
									<plx:right>
										<xsl:copy-of select="$currentSupport"/>
									</plx:right>
								</plx:binaryOperator>
							</xsl:when>
							<xsl:otherwise>
								<xsl:copy-of select="$currentSupport"/>
							</xsl:otherwise>
						</xsl:choose>
					</plx:return>
				</plx:get>
			</plx:property>
			<xsl:variable name="allContainers" select=".//se:Container"/>
			<xsl:variable name="containerElementCount" select="count($allContainers)"/>
			<xsl:variable name="haveCustomChildInfo" select="boolean($containerElementCount)"/>
			<xsl:if test="$haveCustomChildInfo">
				<plx:field visibility="private" static="true" dataTypeName="CustomSerializedContainerElementInfo" dataTypeIsSimpleArray="true" name="myCustomSerializedChildElementInfo"/>
			</xsl:if>
			<xsl:if test="$haveCustomChildInfo or not($ClassOverride)">
				<plx:function visibility="{$InterfaceImplementationVisibility}" name="GetCustomSerializedChildElementInfo" replacesName="{$ClassOverride}">
					<plx:leadingInfo>
						<plx:docComment>
							<summary>Implements ICustomSerializedElement.GetCustomSerializedChildElementInfo</summary>
						</plx:docComment>
					</plx:leadingInfo>
					<plx:interfaceMember dataTypeName="ICustomSerializedElement" memberName="GetCustomSerializedChildElementInfo"/>
					<plx:returns dataTypeName="CustomSerializedContainerElementInfo" dataTypeIsSimpleArray="true"/>
					<xsl:choose>
						<xsl:when test="$haveCustomChildInfo">
							<plx:local dataTypeName="CustomSerializedContainerElementInfo" dataTypeIsSimpleArray="true" name="ret">
								<plx:initialize>
									<plx:callStatic dataTypeName="{$ClassName}" name="myCustomSerializedChildElementInfo" type="field"/>
								</plx:initialize>
							</plx:local>
							<plx:branch>
								<plx:condition>
									<plx:binaryOperator type="identityEquality">
										<plx:left>
											<plx:nameRef name="ret"/>
										</plx:left>
										<plx:right>
											<plx:nullKeyword/>
										</plx:right>
									</plx:binaryOperator>
								</plx:condition>
								<xsl:if test="$ClassOverride">
									<plx:local name="baseInfo" dataTypeName="CustomSerializedContainerElementInfo" dataTypeIsSimpleArray="true">
										<plx:initialize>
											<plx:nullKeyword/>
										</plx:initialize>
									</plx:local>
									<plx:local name="baseInfoCount" dataTypeName=".i4">
										<plx:initialize>
											<plx:value type="i4" data="0"/>
										</plx:initialize>
									</plx:local>
									<plx:branch>
										<plx:condition>
											<plx:binaryOperator type="inequality">
												<plx:left>
													<plx:value type="i4" data="0"/>
												</plx:left>
												<plx:right>
													<plx:binaryOperator type="bitwiseAnd">
														<plx:left>
															<plx:callStatic name="ChildElementInfo" dataTypeName="CustomSerializedElementSupportedOperations" type="field"/>
														</plx:left>
														<plx:right>
															<plx:callThis accessor="base" name="SupportedCustomSerializedOperations" type="property"/>
														</plx:right>
													</plx:binaryOperator>
												</plx:right>
											</plx:binaryOperator>
										</plx:condition>
										<plx:assign>
											<plx:left>
												<plx:nameRef name="baseInfo"/>
											</plx:left>
											<plx:right>
												<plx:callThis accessor="base" name="GetCustomSerializedChildElementInfo"/>
											</plx:right>
										</plx:assign>
										<plx:branch>
											<plx:condition>
												<plx:binaryOperator type="identityInequality">
													<plx:left>
														<plx:nameRef name="baseInfo"/>
													</plx:left>
													<plx:right>
														<plx:nullKeyword/>
													</plx:right>
												</plx:binaryOperator>
											</plx:condition>
											<plx:assign>
												<plx:left>
													<plx:nameRef name="baseInfoCount"/>
												</plx:left>
												<plx:right>
													<plx:callInstance name="Length" type="property">
														<plx:callObject>
															<plx:nameRef name="baseInfo"/>
														</plx:callObject>
													</plx:callInstance>
												</plx:right>
											</plx:assign>
										</plx:branch>
									</plx:branch>
								</xsl:if>
								<plx:assign>
									<plx:left>
										<plx:nameRef name="ret"/>
									</plx:left>
									<plx:right>
										<plx:callNew dataTypeName="CustomSerializedContainerElementInfo" dataTypeIsSimpleArray="true">
											<plx:passParam>
												<xsl:choose>
													<xsl:when test="$ClassOverride">
														<plx:binaryOperator type="add">
															<plx:left>
																<plx:nameRef name="baseInfoCount"/>
															</plx:left>
															<plx:right>
																<plx:value type="i4" data="{$containerElementCount}"/>
															</plx:right>
														</plx:binaryOperator>
													</xsl:when>
													<xsl:otherwise>
														<plx:value type="i4" data="{$containerElementCount}"/>
													</xsl:otherwise>
												</xsl:choose>
											</plx:passParam>
										</plx:callNew>
									</plx:right>
								</plx:assign>
								<xsl:if test="$ClassOverride">
									<plx:branch>
										<plx:condition>
											<plx:binaryOperator type="inequality">
												<plx:left>
													<plx:nameRef name="baseInfoCount"/>
												</plx:left>
												<plx:right>
													<plx:value type="i4" data="0"/>
												</plx:right>
											</plx:binaryOperator>
										</plx:condition>
										<plx:callInstance name="CopyTo">
											<plx:callObject>
												<plx:nameRef name="baseInfo"/>
											</plx:callObject>
											<plx:passParam>
												<plx:nameRef name="ret"/>
											</plx:passParam>
											<plx:passParam>
												<plx:value type="i4" data="{$containerElementCount}"/>
											</plx:passParam>
										</plx:callInstance>
									</plx:branch>
								</xsl:if>
								<xsl:for-each select="$allContainers">
									<!-- Handle nested containers in line, but put them in the outer set to get an accurate index -->
									<xsl:if test="not(parent::se:Container)">
										<xsl:if test="@NotSorted='true' and se:Container">
											<xsl:message terminate="yes">
												<xsl:text>Nested Container elements are not compatible with the @NotSorted="true" value for the '</xsl:text>
												<xsl:value-of select="@Name"/>
												<xsl:text>' container in the Element with @Class '</xsl:text>
												<xsl:value-of select="../@Class"/>
												<xsl:text>'.</xsl:text>
											</xsl:message>
										</xsl:if>
										<xsl:variable name="outerIndex" select="position()-1"/>
										<xsl:call-template name="CreateCustomSerializedElementInfoNameVariable">
											<xsl:with-param name="modifier" select="$outerIndex"/>
										</xsl:call-template>
										<plx:assign>
											<plx:left>
												<plx:callInstance name=".implied" type="arrayIndexer">
													<plx:callObject>
														<plx:nameRef name="ret"/>
													</plx:callObject>
													<plx:passParam>
														<plx:value type="i4" data="{$outerIndex}"/>
													</plx:passParam>
												</plx:callInstance>
											</plx:left>
											<plx:right>
												<plx:callNew dataTypeName="CustomSerializedContainerElementInfo">
													<xsl:call-template name="PassCustomSerializedElementInfoParams">
														<xsl:with-param name="namespaces" select="$namespaces"/>
														<xsl:with-param name="defaultNamespace" select="$defaultNamespace"/>
														<xsl:with-param name="modifier" select="$outerIndex"/>
													</xsl:call-template>
													<xsl:for-each select="se:Link | se:Embed">
														<plx:passParam>
															<plx:callStatic name="{@RoleName}DomainRoleId" dataTypeName="{@RelationshipName}" dataTypeQualifier="{@RelationshipTypeQualifier}" type="field"/>
														</plx:passParam>
													</xsl:for-each>
												</plx:callNew>
											</plx:right>
										</plx:assign>
										<xsl:for-each select="se:Container">
											<xsl:variable name="innerIndex" select="$outerIndex + position()"/>
											<xsl:call-template name="CreateCustomSerializedElementInfoNameVariable">
												<xsl:with-param name="modifier" select="$innerIndex"/>
											</xsl:call-template>
											<plx:assign>
												<plx:left>
													<plx:callInstance name=".implied" type="arrayIndexer">
														<plx:callObject>
															<plx:nameRef name="ret"/>
														</plx:callObject>
														<plx:passParam>
															<plx:value type="i4" data="{$innerIndex}"/>
														</plx:passParam>
													</plx:callInstance>
												</plx:left>
												<plx:right>
													<plx:callNew dataTypeName="CustomSerializedInnerContainerElementInfo">
														<xsl:call-template name="PassCustomSerializedElementInfoParams">
															<xsl:with-param name="namespaces" select="$namespaces"/>
															<xsl:with-param name="defaultNamespace" select="$defaultNamespace"/>
															<xsl:with-param name="modifier" select="$innerIndex"/>
														</xsl:call-template>
														<plx:passParam>
															<plx:callInstance name=".implied" type="arrayIndexer">
																<plx:callObject>
																	<plx:nameRef name="ret"/>
																</plx:callObject>
																<plx:passParam>
																	<plx:value type="i4" data="{$outerIndex}"/>
																</plx:passParam>
															</plx:callInstance>
														</plx:passParam>
														<xsl:for-each select="se:Link | se:Embed">
															<plx:passParam>
																<plx:callStatic name="{@RoleName}DomainRoleId" dataTypeName="{@RelationshipName}" dataTypeQualifier="{@RelationshipTypeQualifier}" type="field"/>
															</plx:passParam>
														</xsl:for-each>
													</plx:callNew>
												</plx:right>
											</plx:assign>
										</xsl:for-each>
									</xsl:if>
								</xsl:for-each>
								<plx:assign>
									<plx:left>
										<plx:callStatic dataTypeName="{$ClassName}" name="myCustomSerializedChildElementInfo" type="field"/>
									</plx:left>
									<plx:right>
										<plx:nameRef name="ret"/>
									</plx:right>
								</plx:assign>
							</plx:branch>
							<plx:return>
								<plx:nameRef name="ret"/>
							</plx:return>
						</xsl:when>
						<xsl:otherwise>
							<plx:throw>
								<plx:callNew dataTypeName="NotSupportedException"/>
							</plx:throw>
						</xsl:otherwise>
					</xsl:choose>
				</plx:function>
			</xsl:if>
			<xsl:variable name="haveCustomElementInfo" select="0!=(string-length(@Prefix)+string-length(@Name)+string-length(@Namespace)+string-length(@WriteStyle)+string-length(@DoubleTagName)+count(se:ConditionalName))"/>
			<xsl:if test="$haveCustomElementInfo or not($ClassOverride)">
				<plx:property visibility="{$InterfaceImplementationVisibility}" name="CustomSerializedElementInfo" replacesName="{$ClassOverride}">
					<plx:leadingInfo>
						<plx:docComment>
							<summary>Implements ICustomSerializedElement.CustomSerializedElementInfo</summary>
						</plx:docComment>
					</plx:leadingInfo>
					<plx:interfaceMember dataTypeName="ICustomSerializedElement" memberName="CustomSerializedElementInfo"/>
					<plx:returns dataTypeName="CustomSerializedElementInfo"/>
					<plx:get>
						<xsl:choose>
							<xsl:when test="$haveCustomElementInfo">
								<xsl:call-template name="ReturnCustomSerializedElementInfo">
									<xsl:with-param name="namespaces" select="$namespaces"/>
									<xsl:with-param name="defaultNamespace" select="$defaultNamespace"/>
								</xsl:call-template>
							</xsl:when>
							<xsl:otherwise>
								<plx:throw>
									<plx:callNew dataTypeName="NotSupportedException"/>
								</plx:throw>
							</xsl:otherwise>
						</xsl:choose>
					</plx:get>
				</plx:property>
			</xsl:if>
			<xsl:variable name="haveCustomPropertyInfo" select="0!=count(se:Attribute)"/>
			<xsl:if test="$haveCustomPropertyInfo or not($ClassOverride)">
				<plx:function visibility="{$InterfaceImplementationVisibility}" name="GetCustomSerializedPropertyInfo" replacesName="{$ClassOverride}">
					<plx:leadingInfo>
						<plx:docComment>
							<summary>Implements ICustomSerializedElement.GetCustomSerializedPropertyInfo</summary>
						</plx:docComment>
					</plx:leadingInfo>
					<plx:interfaceMember dataTypeName="ICustomSerializedElement" memberName="GetCustomSerializedPropertyInfo"/>
					<plx:param name="domainPropertyInfo" dataTypeName="DomainPropertyInfo"></plx:param>
					<plx:param name="rolePlayedInfo" dataTypeName="DomainRoleInfo"></plx:param>
					<plx:returns dataTypeName="CustomSerializedPropertyInfo"/>
					<xsl:choose>
						<xsl:when test="$haveCustomPropertyInfo">
							<xsl:for-each select="se:Attribute">
								<plx:branch>
									<plx:condition>
										<plx:binaryOperator type="equality">
											<plx:left>
												<plx:callInstance type="property" name="Id">
													<plx:callObject>
														<plx:nameRef name="domainPropertyInfo" type="parameter"/>
													</plx:callObject>
												</plx:callInstance>
											</plx:left>
											<plx:right>
												<plx:callStatic type="field" name="{@ID}DomainPropertyId" dataTypeName="{$ClassName}" />
											</plx:right>
										</plx:binaryOperator>
									</plx:condition>
									<xsl:for-each select="se:RolePlayed">
										<plx:branch>
											<plx:condition>
												<plx:binaryOperator type="equality">
													<plx:left>
														<plx:callInstance type="property" name="Id">
															<plx:callObject>
																<plx:nameRef name="rolePlayedInfo" type="parameter"/>
															</plx:callObject>
														</plx:callInstance>
													</plx:left>
													<plx:right>
														<plx:callStatic type="field" name="{@ID}DomainRoleId" dataTypeName="{$ClassName}" />
													</plx:right>
												</plx:binaryOperator>
											</plx:condition>
											<xsl:call-template name="ReturnCustomSerializedPropertyInfo"/>
										</plx:branch>
									</xsl:for-each>
									<xsl:call-template name="ReturnCustomSerializedPropertyInfo"/>
								</plx:branch>
							</xsl:for-each>
							<xsl:if test="$ClassOverride">
								<plx:branch>
									<plx:condition>
										<plx:binaryOperator type="inequality">
											<plx:left>
												<plx:value type="i4" data="0"/>
											</plx:left>
											<plx:right>
												<plx:binaryOperator type="bitwiseAnd">
													<plx:left>
														<plx:callStatic name="PropertyInfo" dataTypeName="CustomSerializedElementSupportedOperations" type="field"/>
													</plx:left>
													<plx:right>
														<plx:callThis accessor="base" name="SupportedCustomSerializedOperations" type="property"/>
													</plx:right>
												</plx:binaryOperator>
											</plx:right>
										</plx:binaryOperator>
									</plx:condition>
									<plx:return>
										<plx:callThis accessor="base" name="GetCustomSerializedPropertyInfo">
											<plx:passParam>
												<plx:nameRef name="domainPropertyInfo" type="parameter"/>
											</plx:passParam>
											<plx:passParam>
												<plx:nameRef name="rolePlayedInfo" type="parameter"/>
											</plx:passParam>
										</plx:callThis>
									</plx:return>
								</plx:branch>
							</xsl:if>
							<plx:return>
								<plx:callStatic dataTypeName="CustomSerializedPropertyInfo" name="Default" type="field"/>
							</plx:return>
						</xsl:when>
						<xsl:otherwise>
							<plx:throw>
								<plx:callNew dataTypeName="NotSupportedException"/>
							</plx:throw>
						</xsl:otherwise>
					</xsl:choose>
				</plx:function>
			</xsl:if>
			<xsl:variable name="customLinkInfo" select="se:Link | se:StandaloneLink"/>
			<xsl:if test="$customLinkInfo or not($ClassOverride)">
				<plx:function visibility="{$InterfaceImplementationVisibility}" name="GetCustomSerializedLinkInfo" replacesName="{$ClassOverride}">
					<plx:leadingInfo>
						<plx:docComment>
							<summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
						</plx:docComment>
					</plx:leadingInfo>
					<plx:interfaceMember dataTypeName="ICustomSerializedElement" memberName="GetCustomSerializedLinkInfo"/>
					<plx:param name="rolePlayedInfo" dataTypeName="DomainRoleInfo"/>
					<plx:param name="elementLink" dataTypeName="ElementLink"/>
					<plx:returns dataTypeName="CustomSerializedElementInfo"/>
					<xsl:choose>
						<xsl:when test="$customLinkInfo">
							<plx:local name="roleId" dataTypeName="Guid">
								<plx:initialize>
									<plx:callInstance type="property" name="Id">
										<plx:callObject>
											<plx:nameRef name="rolePlayedInfo" type="parameter"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:initialize>
							</plx:local>
							<xsl:choose>
								<xsl:when test="not((se:Link|se:StandaloneLink)[normalize-space(@CreateAsRelationshipName)])">
									<xsl:for-each select="$customLinkInfo">
										<plx:branch>
											<plx:condition>
												<plx:binaryOperator type="equality">
													<plx:left>
														<plx:nameRef name="roleId"/>
													</plx:left>
													<plx:right>
														<plx:callStatic type="field" name="{@RoleName}DomainRoleId" dataTypeName="{@RelationshipName}" dataTypeQualifier="{@RelationshipTypeQualifier}"/>
													</plx:right>
												</plx:binaryOperator>
											</plx:condition>
											<xsl:call-template name="ReturnCustomSerializedElementInfo">
												<xsl:with-param name="namespaces" select="$namespaces"/>
												<xsl:with-param name="defaultNamespace" select="$defaultNamespace"/>
											</xsl:call-template>
										</plx:branch>
									</xsl:for-each>
								</xsl:when>
								<xsl:otherwise>
									<xsl:variable name="sortedCustomLinkInfo">
										<xsl:for-each select="$customLinkInfo">
											<xsl:sort select="@RelationshipName"/>
											<xsl:sort select="@RoleName"/>
											<!-- Put the ones without a CreatedAsRelationshipName last -->
											<xsl:sort select="normalize-space(@CreateAsRelationshipName)" order="descending"/>
											<xsl:copy-of select="."/>
										</xsl:for-each>
									</xsl:variable>
									<xsl:for-each select="exsl:node-set($sortedCustomLinkInfo)/child::*">
										<xsl:variable name="roleName" select="@RoleName"/>
										<xsl:variable name="relationshipName" select="@RelationshipName"/>
										<xsl:if test="position()=1 or not(preceding-sibling::*[self::se:Link | self::se:StandaloneLink][1][@RelationshipName=$relationshipName and @RoleName=$roleName])">
											<plx:branch>
												<plx:condition>
													<plx:binaryOperator type="equality">
														<plx:left>
															<plx:nameRef name="roleId"/>
														</plx:left>
														<plx:right>
															<plx:callStatic type="field" name="{$roleName}DomainRoleId" dataTypeName="{$relationshipName}" />
														</plx:right>
													</plx:binaryOperator>
												</plx:condition>
												<xsl:choose>
													<!-- Note that the CreateAs conditions are sorted first -->
													<xsl:when test="normalize-space(@CreateAsRelationshipName)">
														<plx:local name="elementLinkDomainId" dataTypeName="Guid">
															<plx:initialize>
																<plx:callInstance name="Id" type="property">
																	<plx:callObject>
																		<plx:callInstance name="GetDomainRelationship" type="methodCall">
																			<plx:callObject>
																				<plx:nameRef name="elementLink"/>
																			</plx:callObject>
																		</plx:callInstance>
																	</plx:callObject>
																</plx:callInstance>
															</plx:initialize>
														</plx:local>
														<plx:branch>
															<plx:condition>
																<plx:binaryOperator type="equality">
																	<plx:left>
																		<plx:nameRef name="elementLinkDomainId"/>
																	</plx:left>
																	<plx:right>
																		<plx:callStatic name="DomainClassId" dataTypeName="{@CreateAsRelationshipName}" type="field"/>
																	</plx:right>
																</plx:binaryOperator>
															</plx:condition>
															<xsl:call-template name="ReturnCustomSerializedElementInfo">
																<xsl:with-param name="namespaces" select="$namespaces"/>
																<xsl:with-param name="defaultNamespace" select="$defaultNamespace"/>
															</xsl:call-template>
														</plx:branch>
														<xsl:if test="position()!=last() and following-sibling::*[self::se:Link | self::se:StandaloneLink][1][@RelationshipName=$relationshipName and @RoleName=$roleName]">
															<xsl:for-each select="following-sibling::*[self::se:Link | self::se:StandaloneLink][@RelationshipName=$relationshipName and @RoleName=$roleName]">
																<xsl:choose>
																	<xsl:when test="normalize-space(@CreateAsRelationshipName)">
																		<plx:alternateBranch>
																			<plx:condition>
																				<plx:binaryOperator type="equality">
																					<plx:left>
																						<plx:nameRef name="elementLinkDomainId"/>
																					</plx:left>
																					<plx:right>
																						<plx:callStatic name="DomainClassId" dataTypeName="{@CreateAsRelationshipName}" type="field"/>
																					</plx:right>
																				</plx:binaryOperator>
																			</plx:condition>
																			<xsl:call-template name="ReturnCustomSerializedElementInfo">
																				<xsl:with-param name="namespaces" select="$namespaces"/>
																				<xsl:with-param name="defaultNamespace" select="$defaultNamespace"/>
																			</xsl:call-template>
																		</plx:alternateBranch>
																	</xsl:when>
																	<xsl:otherwise>
																		<plx:fallbackBranch>
																			<xsl:call-template name="ReturnCustomSerializedElementInfo">
																				<xsl:with-param name="namespaces" select="$namespaces"/>
																				<xsl:with-param name="defaultNamespace" select="$defaultNamespace"/>
																			</xsl:call-template>
																		</plx:fallbackBranch>
																	</xsl:otherwise>
																</xsl:choose>
															</xsl:for-each>
														</xsl:if>
													</xsl:when>
													<xsl:otherwise>
														<xsl:call-template name="ReturnCustomSerializedElementInfo">
															<xsl:with-param name="namespaces" select="$namespaces"/>
															<xsl:with-param name="defaultNamespace" select="$defaultNamespace"/>
														</xsl:call-template>
													</xsl:otherwise>
												</xsl:choose>
											</plx:branch>
										</xsl:if>
									</xsl:for-each>
								</xsl:otherwise>
							</xsl:choose>
							<xsl:if test="$ClassOverride">
								<plx:branch>
									<plx:condition>
										<plx:binaryOperator type="inequality">
											<plx:left>
												<plx:value type="i4" data="0"/>
											</plx:left>
											<plx:right>
												<plx:binaryOperator type="bitwiseAnd">
													<plx:left>
														<plx:callStatic name="LinkInfo" dataTypeName="CustomSerializedElementSupportedOperations" type="field"/>
													</plx:left>
													<plx:right>
														<plx:callThis accessor="base" name="SupportedCustomSerializedOperations" type="property"/>
													</plx:right>
												</plx:binaryOperator>
											</plx:right>
										</plx:binaryOperator>
									</plx:condition>
									<plx:return>
										<plx:callThis accessor="base" name="GetCustomSerializedLinkInfo">
											<plx:passParam>
												<plx:nameRef name="rolePlayedInfo" type="parameter"/>
											</plx:passParam>
											<plx:passParam>
												<plx:nameRef name="elementLink" type="parameter"/>
											</plx:passParam>
										</plx:callThis>
									</plx:return>
								</plx:branch>
							</xsl:if>
							<plx:return>
								<plx:callStatic dataTypeName="CustomSerializedElementInfo" name="Default" type="field"/>
							</plx:return>
						</xsl:when>
						<xsl:otherwise>
							<plx:throw>
								<plx:callNew dataTypeName="NotSupportedException"/>
							</plx:throw>
						</xsl:otherwise>
					</xsl:choose>
				</plx:function>
			</xsl:if>
			<xsl:if test="not(@SortChildElements='true') and se:Container/se:Container">
				<xsl:message terminate="yes">
					<xsl:text>Nested Container elements require Element/@SortChildElements to be true for the Element with @Class '</xsl:text>
					<xsl:value-of select="@Class"/>
					<xsl:text>'.</xsl:text>
				</xsl:message>
			</xsl:if>
			<xsl:choose>
				<xsl:when test="@SortChildElements='true'">
					<xsl:variable name="UnsortedFirst" select="@UnsortedElementsFirst='true'"/>
					<plx:field name="myCustomSortChildComparer" static="true" visibility="private" dataTypeName="IComparer">
						<plx:passTypeParam dataTypeName="DomainRoleInfo"/>
					</plx:field>
					<plx:class name="CustomSortChildComparer" visibility="private" modifier="sealed">
						<plx:implementsInterface dataTypeName="IComparer">
							<plx:passTypeParam dataTypeName="DomainRoleInfo"/>
						</plx:implementsInterface>
						<plx:field name="myRoleOrderDictionary" visibility="private" readOnly="true" dataTypeName="Dictionary">
							<plx:passTypeParam dataTypeName=".string"/>
							<plx:passTypeParam dataTypeName=".i4"/>
						</plx:field>
						<xsl:if test="$ClassOverride">
							<plx:field name="myBaseComparer" visibility="private" dataTypeName="IComparer">
								<plx:passTypeParam dataTypeName="DomainRoleInfo"/>
							</plx:field>
						</xsl:if>
						<plx:function name=".construct" visibility="public">
							<plx:param name="store" dataTypeName="Store"/>
							<xsl:if test="$ClassOverride">
								<plx:param name="baseComparer" dataTypeName="IComparer">
									<plx:passTypeParam dataTypeName="DomainRoleInfo"/>
								</plx:param>
								<plx:assign>
									<plx:left>
										<plx:callThis name="myBaseComparer" type="field"/>
									</plx:left>
									<plx:right>
										<plx:nameRef name="baseComparer" type="parameter"/>
									</plx:right>
								</plx:assign>
							</xsl:if>
							<xsl:variable name="SortedLevelsFragment">
								<!-- ChildElement/Link links may have more information in Link. Just use
								     the ChildElement one. -->
								<xsl:variable name="childLinks" select="se:Container[not(@NotSorted='true')]/descendant::se:*[self::se:Link | self::se:Embed]"/>
								<!-- Define a variable with structure <SortLevel><Role/><SortLevel/> -->
								<xsl:for-each select="se:Link | se:Container">
									<xsl:if test="not(@NotSorted='true')">
										<xsl:choose>
											<xsl:when test="self::se:Link">
												<xsl:variable name="relName" select="@RelationshipName"/>
												<xsl:variable name="roleName" select="@RoleName"/>
												<xsl:if test="0=count($childLinks[@RelationshipName=$relName and @RoleName=$roleName])">
													<SortLevel>
														<Role RelationshipName="{@RelationshipName}" RoleName="{@RoleName}"/>
													</SortLevel>
												</xsl:if>
											</xsl:when>
											<xsl:when test="self::se:Container">
												<xsl:choose>
													<xsl:when test="@SortChildElements='true' or se:Container">
														<!-- Add one sort level for each child -->
														<xsl:for-each select="se:Link | se:Embed | se:Container">
															<xsl:choose>
																<xsl:when test="self::se:Container">
																	<xsl:choose>
																		<xsl:when test="@SortChildElements='true'">
																			<!-- Add one sort level for each nested child -->
																			<xsl:for-each select="se:Link | se:Embed">
																				<SortLevel>
																					<Role RelationshipName="{@RelationshipName}" RoleName="{@RoleName}"/>
																				</SortLevel>
																			</xsl:for-each>
																		</xsl:when>
																		<xsl:otherwise>
																			<!-- Add one sort level for all nested children -->
																			<SortLevel>
																				<xsl:for-each select="se:Link | se:Embed">
																					<Role RelationshipName="{@RelationshipName}" RoleName="{@RoleName}"/>
																				</xsl:for-each>
																			</SortLevel>
																		</xsl:otherwise>
																	</xsl:choose>
																</xsl:when>
																<xsl:otherwise>
																	<SortLevel>
																		<Role RelationshipName="{@RelationshipName}" RoleName="{@RoleName}"/>
																	</SortLevel>
																</xsl:otherwise>
															</xsl:choose>
														</xsl:for-each>
													</xsl:when>
													<xsl:otherwise>
														<!-- Add one sort level for all children -->
														<SortLevel>
															<xsl:for-each select="se:Link | se:Embed">
																<Role RelationshipName="{@RelationshipName}" RoleName="{@RoleName}"/>
															</xsl:for-each>
														</SortLevel>
													</xsl:otherwise>
												</xsl:choose>
											</xsl:when>
										</xsl:choose>
									</xsl:if>
								</xsl:for-each>
							</xsl:variable>
							<xsl:variable name="SortedLevels" select="exsl:node-set($SortedLevelsFragment)"/>
							<plx:local name="domainDataDirectory" dataTypeName="DomainDataDirectory">
								<plx:initialize>
									<plx:callInstance name="DomainDataDirectory" type="property">
										<plx:callObject>
											<plx:nameRef name="store" type="parameter"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:initialize>
							</plx:local>
							<plx:local name="roleOrderDictionary" dataTypeName="Dictionary">
								<plx:passTypeParam dataTypeName=".string"/>
								<plx:passTypeParam dataTypeName=".i4"/>
								<plx:initialize>
									<plx:callNew dataTypeName="Dictionary">
										<plx:passTypeParam dataTypeName=".string"/>
										<plx:passTypeParam dataTypeName=".i4"/>
									</plx:callNew>
								</plx:initialize>
							</plx:local>
							<plx:local name="domainRole" dataTypeName="DomainRoleInfo"/>
							<xsl:for-each select="$SortedLevels/SortLevel">
								<xsl:variable name="level" select="position()-1"/>
								<xsl:for-each select="Role">
									<plx:assign>
										<plx:left>
											<plx:nameRef name="domainRole"/>
										</plx:left>
										<plx:right>
											<plx:callInstance name="OppositeDomainRole" type="property">
												<plx:callObject>
													<plx:callInstance name="FindDomainRole">
														<plx:callObject>
															<plx:nameRef name="domainDataDirectory"/>
														</plx:callObject>
														<plx:passParam>
															<plx:callStatic dataTypeName="{@RelationshipName}" dataTypeQualifier="{@RelationshipTypeQualifier}" name="{@RoleName}DomainRoleId" type="field"/>
														</plx:passParam>
													</plx:callInstance>
												</plx:callObject>
											</plx:callInstance>
										</plx:right>
									</plx:assign>
									<plx:assign>
										<plx:left>
											<plx:callInstance name=".implied" type="indexerCall">
												<plx:callObject>
													<plx:nameRef name="roleOrderDictionary"/>
												</plx:callObject>
												<plx:passParam>
													<xsl:call-template name="DomainRoleInfoFullName">
														<xsl:with-param name="DomainRoleInfoExpression">
															<plx:nameRef name="domainRole"/>
														</xsl:with-param>
													</xsl:call-template>
												</plx:passParam>
											</plx:callInstance>
										</plx:left>
										<plx:right>
											<plx:value type="i4" data="{$level}"/>
										</plx:right>
									</plx:assign>
								</xsl:for-each>
							</xsl:for-each>
							<plx:assign>
								<plx:left>
									<plx:callThis name="myRoleOrderDictionary" type="field"/>
								</plx:left>
								<plx:right>
									<plx:nameRef name="roleOrderDictionary"/>
								</plx:right>
							</plx:assign>
						</plx:function>
						<plx:function visibility="privateInterfaceMember" name="Compare">
							<plx:interfaceMember dataTypeName="IComparer" memberName="Compare">
								<plx:passTypeParam dataTypeName="DomainRoleInfo"/>
							</plx:interfaceMember>
							<plx:param name="x" dataTypeName="DomainRoleInfo"/>
							<plx:param name="y" dataTypeName="DomainRoleInfo"/>
							<plx:returns dataTypeName=".i4"/>
							<xsl:if test="$ClassOverride">
								<!-- Give the base the first shot, we want base elements displayed before derived elements -->
								<plx:branch>
									<plx:condition>
										<plx:binaryOperator type="identityInequality">
											<plx:left>
												<plx:callThis name="myBaseComparer" type="field"/>
											</plx:left>
											<plx:right>
												<plx:nullKeyword/>
											</plx:right>
										</plx:binaryOperator>
									</plx:condition>
									<plx:local name="baseOpinion" dataTypeName=".i4">
										<plx:initialize>
											<plx:callInstance name="Compare">
												<plx:callObject>
													<plx:callThis name="myBaseComparer" type="field"/>
												</plx:callObject>
												<plx:passParam>
													<plx:nameRef name="x" type="parameter"/>
												</plx:passParam>
												<plx:passParam>
													<plx:nameRef name="y" type="parameter"/>
												</plx:passParam>
											</plx:callInstance>
										</plx:initialize>
									</plx:local>
									<plx:branch>
										<plx:condition>
											<plx:binaryOperator type="inequality">
												<plx:left>
													<plx:value type="i4" data="0"/>
												</plx:left>
												<plx:right>
													<plx:nameRef name="baseOpinion"/>
												</plx:right>
											</plx:binaryOperator>
										</plx:condition>
										<plx:return>
											<plx:nameRef name="baseOpinion"/>
										</plx:return>
									</plx:branch>
								</plx:branch>
							</xsl:if>
							<xsl:variable name="paramVals">
								<Value>x</Value>
								<Value>y</Value>
							</xsl:variable>
							<xsl:for-each select="exsl:node-set($paramVals)/child::*">
								<plx:local name="{.}Pos" dataTypeName=".i4"/>
								<plx:branch>
									<plx:condition>
										<plx:unaryOperator type="booleanNot">
											<plx:callInstance name="TryGetValue">
												<plx:callObject>
													<plx:callThis name="myRoleOrderDictionary" type="field"/>
												</plx:callObject>
												<plx:passParam type="in">
													<xsl:call-template name="DomainRoleInfoFullName">
														<xsl:with-param name="DomainRoleInfoExpression">
															<plx:nameRef name="{.}" type="parameter"/>
														</xsl:with-param>
													</xsl:call-template>
												</plx:passParam>
												<plx:passParam type="out">
													<plx:nameRef name="{.}Pos"/>
												</plx:passParam>
											</plx:callInstance>
										</plx:unaryOperator>
									</plx:condition>
									<plx:assign>
										<plx:left>
											<plx:nameRef name="{.}Pos"/>
										</plx:left>
										<plx:right>
											<plx:callStatic dataTypeName=".i4" name="MaxValue" type="field">
												<xsl:if test="$UnsortedFirst">
													<xsl:attribute name="name">
														<xsl:text>MinValue</xsl:text>
													</xsl:attribute>
												</xsl:if>
											</plx:callStatic>
										</plx:right>
									</plx:assign>
								</plx:branch>
							</xsl:for-each>
							<plx:return>
								<plx:callInstance name="CompareTo">
									<plx:callObject>
										<plx:nameRef name="xPos"/>
									</plx:callObject>
									<plx:passParam>
										<plx:nameRef name="yPos"/>
									</plx:passParam>
								</plx:callInstance>
							</plx:return>
						</plx:function>
					</plx:class>
					<plx:property visibility="{$InterfaceImplementationVisibility}" name="CustomSerializedChildRoleComparer" replacesName="{$ClassOverride}">
						<plx:leadingInfo>
							<plx:docComment>
								<summary>Implements ICustomSerializedElement.CustomSerializedChildRoleComparer</summary>
							</plx:docComment>
						</plx:leadingInfo>
						<plx:interfaceMember dataTypeName="ICustomSerializedElement" memberName="CustomSerializedChildRoleComparer"/>
						<plx:returns dataTypeName="IComparer">
							<plx:passTypeParam dataTypeName="DomainRoleInfo"/>
						</plx:returns>
						<plx:get>
							<plx:local name="retVal" dataTypeName="IComparer">
								<plx:passTypeParam dataTypeName="DomainRoleInfo"/>
								<plx:initialize>
									<plx:callStatic dataTypeName="{$ClassName}" name="myCustomSortChildComparer" type="field"/>
								</plx:initialize>
							</plx:local>
							<plx:branch>
								<plx:condition>
									<plx:binaryOperator type="identityEquality">
										<plx:left>
											<plx:nullKeyword/>
										</plx:left>
										<plx:right>
											<plx:nameRef name="retVal"/>
										</plx:right>
									</plx:binaryOperator>
								</plx:condition>
								<xsl:if test="$ClassOverride">
									<plx:local name="baseComparer" dataTypeName="IComparer">
										<plx:passTypeParam dataTypeName="DomainRoleInfo"/>
										<plx:initialize>
											<plx:nullKeyword/>
										</plx:initialize>
									</plx:local>
									<plx:branch>
										<plx:condition>
											<plx:binaryOperator type="inequality">
												<plx:left>
													<plx:value type="i4" data="0"/>
												</plx:left>
												<plx:right>
													<plx:binaryOperator type="bitwiseAnd">
														<plx:left>
															<plx:callStatic name="CustomSortChildRoles" dataTypeName="CustomSerializedElementSupportedOperations" type="field"/>
														</plx:left>
														<plx:right>
															<plx:callThis accessor="base" name="SupportedCustomSerializedOperations" type="property"/>
														</plx:right>
													</plx:binaryOperator>
												</plx:right>
											</plx:binaryOperator>
										</plx:condition>
										<plx:assign>
											<plx:left>
												<plx:nameRef name="baseComparer"/>
											</plx:left>
											<plx:right>
												<plx:callThis accessor="base" name="CustomSerializedChildRoleComparer" type="property"/>
											</plx:right>
										</plx:assign>
									</plx:branch>
								</xsl:if>
								<plx:assign>
									<plx:left>
										<plx:nameRef name="retVal"/>
									</plx:left>
									<plx:right>
										<plx:callNew dataTypeName="CustomSortChildComparer">
											<plx:passParam>
												<plx:callThis name="Store" type="property"/>
											</plx:passParam>
											<xsl:if test="$ClassOverride">
												<plx:passParam>
													<plx:nameRef name="baseComparer"/>
												</plx:passParam>
											</xsl:if>
										</plx:callNew>
									</plx:right>
								</plx:assign>
								<plx:assign>
									<plx:left>
										<plx:callStatic dataTypeName="{$ClassName}" name="myCustomSortChildComparer" type="field"/>
									</plx:left>
									<plx:right>
										<plx:nameRef name="retVal"/>
									</plx:right>
								</plx:assign>
							</plx:branch>
							<plx:return>
								<plx:nameRef name="retVal"/>
							</plx:return>
						</plx:get>
					</plx:property>
				</xsl:when>
				<xsl:when test="not($ClassOverride)">
					<plx:property visibility="{$InterfaceImplementationVisibility}" name="CustomSerializedChildRoleComparer">
						<plx:leadingInfo>
							<plx:docComment>
								<summary>Implements ICustomSerializedElement.CustomSerializedChildRoleComparer</summary>
							</plx:docComment>
						</plx:leadingInfo>
						<plx:interfaceMember dataTypeName="ICustomSerializedElement" memberName="CustomSerializedChildRoleComparer"/>
						<plx:returns dataTypeName="IComparer">
							<plx:passTypeParam dataTypeName="DomainRoleInfo"/>
						</plx:returns>
						<plx:get>
							<plx:return>
								<plx:nullKeyword/>
							</plx:return>
						</plx:get>
					</plx:property>
				</xsl:when>
			</xsl:choose>
			<xsl:variable name="mapChildElementBodyFragment">
				<xsl:variable name="namespace">
					<xsl:call-template name="ResolveNamespace">
						<xsl:with-param name="namespaces" select="$namespaces"/>
						<!-- Use default for prefix parameter -->
					</xsl:call-template>
				</xsl:variable>
				<xsl:variable name="linksInContainerElement" select="se:Container//se:Link"/>
				<xsl:variable name="containerElements" select="$allContainers"/>
				<xsl:variable name="allLinksTemp">
					<xsl:for-each select="se:StandaloneLink | se:Link[not(@WriteStyle='NotWritten') or se:ConditionalName[not(@WriteStyle='NotWritten')]]">
						<xsl:copy>
							<xsl:copy-of select="@*"/>
							<xsl:if test="$linksInContainerElement[@RelationshipName=current()/@RelationshipName and @RoleName=current()/@RoleName and normalize-space(@CreateAsRelationshipName)=normalize-space(current()/@CreateAsRelationshipName)]">
								<xsl:attribute name="contained">
									<xsl:value-of select="true()"/>
								</xsl:attribute>
							</xsl:if>
							<xsl:for-each select="se:ConditionalName">
								<xsl:copy>
									<xsl:copy-of select="@*"/>
								</xsl:copy>
							</xsl:for-each>
							<xsl:copy-of select="se:Role"/>
						</xsl:copy>
					</xsl:for-each>
				</xsl:variable>
				<xsl:variable name="allLinks" select="exsl:node-set($allLinksTemp)/child::*"/>

				<xsl:for-each select="$allLinks">
					<xsl:choose>
						<!-- Walk the $allLinks, then add the ones that are
						NOT contained in $linksInContainerElement -->
						<xsl:when test="not(@contained)">
							<xsl:variable name="createAsRelationshipName" select="normalize-space(@CreateAsRelationshipName)"/>
							<xsl:variable name="explicitForwardReferenceType" select="string(@ForwardReferenceRolePlayerType)"/>
							<plx:callInstance name="InitializeRoles">
								<xsl:choose>
									<xsl:when test="$createAsRelationshipName">
										<xsl:if test="$explicitForwardReferenceType">
											<xsl:message terminate="yes">CreateAsRelationshipName and ForwardReferenceRolePlayerType not supported simultaneously</xsl:message>
										</xsl:if>
										<xsl:attribute name="name">
											<xsl:text>InitializeRolesWithExplicitRelationship</xsl:text>
										</xsl:attribute>
									</xsl:when>
									<xsl:when test="$explicitForwardReferenceType">
										<xsl:attribute name="name">
											<xsl:text>InitializeRolesWithExplicitForwardReference</xsl:text>
										</xsl:attribute>
									</xsl:when>
								</xsl:choose>
								<plx:callObject>
									<plx:nameRef name="match"/>
								</plx:callObject>
								<xsl:if test="@AllowDuplicates='true'">
									<plx:passParam>
										<plx:trueKeyword/>
									</plx:passParam>
								</xsl:if>
								<xsl:if test="$createAsRelationshipName">
									<plx:passParam>
										<plx:callStatic name="DomainClassId" dataTypeName="{$createAsRelationshipName}" type="field"/>
									</plx:passParam>
								</xsl:if>
								<xsl:if test="$explicitForwardReferenceType">
									<plx:passParam>
										<plx:callStatic name="DomainClassId" dataTypeName="{$explicitForwardReferenceType}" type="field">
											<xsl:if test="string(@ForwardReferenceRolePlayerTypeQualifier)">
												<xsl:attribute name="dataTypeQualifier">
													<xsl:value-of select="@ForwardReferenceRolePlayerTypeQualifier"/>
												</xsl:attribute>
											</xsl:if>
										</plx:callStatic>
									</plx:passParam>
								</xsl:if>
								<plx:passParam>
									<plx:callStatic name="{@RoleName}DomainRoleId" dataTypeName="{@RelationshipName}" dataTypeQualifier="{@RelationshipTypeQualifier}" type="property"/>
								</plx:passParam>
							</plx:callInstance>
							<plx:callInstance name="Add">
								<plx:callObject>
									<plx:nameRef name="childElementMappings"/>
								</plx:callObject>
								<plx:passParam>
									<plx:string>
										<plx:string data="||||"/>
										<plx:string>
											<xsl:call-template name="ResolveNamespace">
												<xsl:with-param name="namespaces" select="$namespaces"/>
												<!-- Use default for prefix parameter -->
											</xsl:call-template>
										</plx:string>
										<plx:string data="|"/>
										<plx:string data="{@Name}"/>
									</plx:string>
								</plx:passParam>
								<plx:passParam>
									<plx:nameRef name="match"/>
								</plx:passParam>
							</plx:callInstance>
							<xsl:for-each select="se:ConditionalName">
								<plx:callInstance name="Add">
									<plx:callObject>
										<plx:nameRef name="childElementMappings"/>
									</plx:callObject>
									<plx:passParam>
										<plx:string>
											<plx:string data="||||"/>
											<plx:string>
												<xsl:call-template name="ResolveNamespace">
													<xsl:with-param name="namespaces" select="$namespaces"/>
													<!-- Use default for prefix parameter -->
												</xsl:call-template>
											</plx:string>
											<plx:string data="|"/>
											<plx:string data="{@Name}"/>
										</plx:string>
									</plx:passParam>
									<plx:passParam>
										<plx:nameRef name="match"/>
									</plx:passParam>
								</plx:callInstance>
							</xsl:for-each>
						</xsl:when>
					</xsl:choose>
				</xsl:for-each>
				<!-- Walk $linksInContainerElements, then add the ones that
				INTERSECT with the $allLinks list to the dictionary. -->
				<xsl:for-each select="$containerElements">
					<xsl:variable name="outerContainer" select="parent::se:Container"/>
					<xsl:variable name="localLinksFragment">
						<xsl:for-each select="se:Link">
							<xsl:variable name="relationshipName" select="@RelationshipName"/>
							<xsl:variable name="roleName" select="@RoleName"/>
							<xsl:variable name="namedLinks" select="$allLinks[@RelationshipName=current()/@RelationshipName and @RoleName=current()/@RoleName and normalize-space(@CreateAsRelationshipName)=normalize-space(current()/@CreateAsRelationshipName)]"/>
							<xsl:for-each select="$namedLinks">
								<xsl:copy>
									<xsl:copy-of select="@*[local-name()!='Name']"/>
									<xsl:variable name="conditionalNames" select="se:ConditionalName[string(@Name) and not(@WriteStyle='NotWritten')]"/>
									<xsl:choose>
										<xsl:when test="self::se:StandaloneLink">
											<xsl:if test="not(@PrimaryLinkElement='true' or @PrimaryLinkElement='1')">
												<xsl:choose>
													<xsl:when test="string-length(@Name)">
														<xsl:copy-of select="@Name"/>
													</xsl:when>
													<xsl:otherwise>
														<xsl:attribute name="Name">
															<xsl:value-of select="@Class"/>
														</xsl:attribute>
													</xsl:otherwise>
												</xsl:choose>
											</xsl:if>
										</xsl:when>
										<xsl:when test="string-length(@Name)">
											<xsl:copy-of select="@Name"/>
										</xsl:when>
										<xsl:when test="not($conditionalNames)">
											<xsl:attribute name="Name">
												<xsl:value-of select="@RelationshipName"/>
												<xsl:text>.</xsl:text>
												<xsl:value-of select="@RoleName"/>
											</xsl:attribute>
										</xsl:when>
									</xsl:choose>
									<xsl:copy-of select="@AllowDuplicates"/>
									<xsl:for-each select="$conditionalNames">
										<xsl:copy>
											<xsl:copy-of select="@*"/>
										</xsl:copy>
									</xsl:for-each>
									<xsl:copy-of select="se:Role"/>
								</xsl:copy>
							</xsl:for-each>
						</xsl:for-each>
					</xsl:variable>
					<xsl:variable name="localLinks" select="exsl:node-set($localLinksFragment)/child::*"/>
					<xsl:if test="$localLinks">
						<xsl:variable name="outerContainerName" select="string($outerContainer/@Name)"/>
						<xsl:variable name="outerContainerNamespaceFragment">
							<xsl:for-each select="$outerContainer">
								<xsl:call-template name="ResolveNamespace">
									<xsl:with-param name="namespaces" select="$namespaces"/>
									<!-- Use default for prefix parameter -->
								</xsl:call-template>
							</xsl:for-each>
						</xsl:variable>
						<xsl:variable name="outerContainerNamespace" select="string($outerContainerNamespaceFragment)"/>
						<xsl:variable name="containerName" select="string(@Name)"/>
						<xsl:variable name="containerNamespaceFragment">
							<xsl:call-template name="ResolveNamespace">
								<xsl:with-param name="namespaces" select="$namespaces"/>
								<!-- Use default for prefix parameter -->
							</xsl:call-template>
						</xsl:variable>
						<xsl:variable name="containerNamespace" select="string($containerNamespaceFragment)"/>
						<xsl:for-each select="$localLinks">
							<xsl:variable name="createAsRelationshipName" select="normalize-space(@CreateAsRelationshipName)"/>
							<xsl:variable name="explicitForwardReferenceType" select="string(@ForwardReferenceRolePlayerType)"/>
							<xsl:variable name="elementNamespaceFragment">
								<xsl:call-template name="ResolveNamespace">
									<xsl:with-param name="namespaces" select="$namespaces"/>
									<!-- Use default for prefix parameter -->
								</xsl:call-template>
							</xsl:variable>
							<xsl:variable name="elementNamespace" select="string($elementNamespaceFragment)"/>
							<plx:callInstance name="InitializeRoles">
								<xsl:choose>
									<xsl:when test="$createAsRelationshipName">
										<xsl:if test="$explicitForwardReferenceType">
											<xsl:message terminate="yes">CreateAsRelationshipName and ForwardReferenceRolePlayerType not supported simultaneously</xsl:message>
										</xsl:if>
										<xsl:attribute name="name">
											<xsl:text>InitializeRolesWithExplicitRelationship</xsl:text>
										</xsl:attribute>
									</xsl:when>
									<xsl:when test="$explicitForwardReferenceType">
										<xsl:attribute name="name">
											<xsl:text>InitializeRolesWithExplicitForwardReference</xsl:text>
										</xsl:attribute>
									</xsl:when>
								</xsl:choose>
								<plx:callObject>
									<plx:nameRef name="match"/>
								</plx:callObject>
								<xsl:if test="@AllowDuplicates='true'">
									<plx:passParam>
										<plx:trueKeyword/>
									</plx:passParam>
								</xsl:if>
								<xsl:if test="$createAsRelationshipName">
									<plx:passParam>
										<plx:callStatic name="DomainClassId" dataTypeName="{$createAsRelationshipName}" type="field"/>
									</plx:passParam>
								</xsl:if>
								<xsl:if test="self::se:StandaloneLink">
									<plx:passParam>
										<xsl:call-template name="CreateStandaloneRelationship">
											<xsl:with-param name="namespaces" select="$namespaces"/>
											<xsl:with-param name="defaultNamespace" select="$defaultNamespace"/>
										</xsl:call-template>
									</plx:passParam>
								</xsl:if>
								<xsl:if test="$explicitForwardReferenceType">
									<plx:passParam>
										<plx:callStatic name="DomainClassId" dataTypeName="{$explicitForwardReferenceType}" type="field">
											<xsl:if test="string(@ForwardReferenceRolePlayerTypeQualifier)">
												<xsl:attribute name="dataTypeQualifier">
													<xsl:value-of select="@ForwardReferenceRolePlayerTypeQualifier"/>
												</xsl:attribute>
											</xsl:if>
										</plx:callStatic>
									</plx:passParam>
								</xsl:if>
								<plx:passParam>
									<plx:callStatic name="{@RoleName}DomainRoleId" dataTypeName="{@RelationshipName}" dataTypeQualifier="{@RelationshipTypeQualifier}" type="field"/>
								</plx:passParam>
							</plx:callInstance>
							<xsl:if test="self::se:StandaloneLink or (string(@Name) and not(@WriteStyle='NotWritten'))">
								<plx:callInstance name="Add">
									<plx:callObject>
										<plx:nameRef name="childElementMappings"/>
									</plx:callObject>
									<plx:passParam>
										<plx:string>
											<plx:string data="{$outerContainerNamespace}|{$outerContainerName}|"/>
											<xsl:if test="$outerContainerNamespace!=$containerNamespace">
												<plx:string data="{$containerNamespace}"/>
											</xsl:if>
											<plx:string data="|{$containerName}|"/>
											<xsl:if test="$elementNamespace!=$containerNamespace">
												<plx:string data="{$elementNamespace}"/>
											</xsl:if>
											<plx:string data="|{@Name}"/>
										</plx:string>
									</plx:passParam>
									<plx:passParam>
										<plx:nameRef name="match"/>
									</plx:passParam>
								</plx:callInstance>
							</xsl:if>
							<xsl:for-each select="se:ConditionalName">
								<plx:callInstance name="Add">
									<plx:callObject>
										<plx:nameRef name="childElementMappings"/>
									</plx:callObject>
									<plx:passParam>
										<plx:string>
											<plx:string>
												<plx:string data="{$outerContainerNamespace}|{$outerContainerName}|"/>
												<xsl:if test="$outerContainerNamespace!=$containerNamespace">
													<plx:string data="{$containerNamespace}"/>
												</xsl:if>
												<plx:string data="|{$containerName}|"/>
												<xsl:if test="$elementNamespace!=$containerNamespace">
													<plx:string data="{$elementNamespace}"/>
												</xsl:if>
												<plx:string data="|{@Name}"/>
											</plx:string>
										</plx:string>
									</plx:passParam>
									<plx:passParam>
										<plx:nameRef name="match"/>
									</plx:passParam>
								</plx:callInstance>
							</xsl:for-each>
						</xsl:for-each>
					</xsl:if>
				</xsl:for-each>

				<!-- Walk $linksInContainerElements, then add the ones that are
				NOT in the $allLinks list to the dictionary. -->
				<xsl:for-each select="$containerElements">
					<xsl:variable name="embedsFragment">
						<xsl:for-each select="se:Embed">
							<xsl:variable name="relationshipName" select="string(@RelationshipName)"/>
							<xsl:variable name="roleName" select="string(@RoleName)"/>
							<xsl:variable name="createAsRelationshipName" select="normalize-space(@CreateAsRelationshipName)"/>
							<xsl:if test="not($allLinks[@RelationshipName=$relationshipName and @RoleName=$roleName and normalize-space(@CreateAsRelationshipName)=$createAsRelationshipName])">
								<xsl:if test="$createAsRelationshipName">
									<explicitCreateMarker>
										<plx:passParam>
											<plx:callStatic name="DomainClassId" dataTypeName="{$createAsRelationshipName}" type="field"/>
										</plx:passParam>
									</explicitCreateMarker>
								</xsl:if>
								<plx:passParam>
									<plx:callStatic name="{$roleName}DomainRoleId" dataTypeName="{$relationshipName}" type="property"/>
								</plx:passParam>
							</xsl:if>
						</xsl:for-each>
					</xsl:variable>
					<xsl:variable name="embeds" select="exsl:node-set($embedsFragment)/child::*"/>
					<xsl:if test="$embeds">
						<xsl:variable name="explicitCreateParam" select="$embeds/self::explicitCreateMarker/plx:passParam"/>
						<xsl:choose>
							<!-- Note that InitializeRolesWithExplicitForwardReference is used only link elements, not with embeddings
							because there is no ambiguity with respect to the type of the opposite role player with an embedding. -->
							<xsl:when test="$explicitCreateParam">
								<plx:callInstance name="InitializeRolesWithExplicitRelationship">
									<plx:callObject>
										<plx:nameRef name="match"/>
									</plx:callObject>
									<xsl:copy-of select="$explicitCreateParam"/>
									<xsl:copy-of select="$embeds/self::plx:passParam"/>
								</plx:callInstance>
							</xsl:when>
							<xsl:otherwise>
								<plx:callInstance name="InitializeRoles">
									<plx:callObject>
										<plx:nameRef name="match"/>
									</plx:callObject>
									<xsl:copy-of select="$embeds/self::plx:passParam"/>
								</plx:callInstance>
							</xsl:otherwise>
						</xsl:choose>
						<xsl:variable name="outerContainer" select="parent::se:Container"/>
						<xsl:variable name="outerContainerNamespaceFragment">
							<xsl:for-each select="$outerContainer">
								<xsl:call-template name="ResolveNamespace">
									<xsl:with-param name="namespaces" select="$namespaces"/>
									<!-- Use default for prefix parameter -->
								</xsl:call-template>
							</xsl:for-each>
						</xsl:variable>
						<xsl:variable name="outerContainerNamespace" select="string($outerContainerNamespaceFragment)"/>
						<xsl:variable name="containerNamespaceFragment">
							<xsl:call-template name="ResolveNamespace">
								<xsl:with-param name="namespaces" select="$namespaces"/>
								<!-- Use default for prefix parameter -->
							</xsl:call-template>
						</xsl:variable>
						<xsl:variable name="containerNamespace" select="string($containerNamespaceFragment)"/>
						<plx:callInstance name="Add">
							<plx:callObject>
								<plx:nameRef name="childElementMappings"/>
							</plx:callObject>
							<plx:passParam>
								<plx:string>
									<plx:string data="{$outerContainerNamespace}|{$outerContainer/@Name}|"/>
									<xsl:if test="$outerContainerNamespace!=$containerNamespace">
										<plx:string data="{$containerNamespace}"/>
									</xsl:if>
									<plx:string data="|{@Name}||"/>
								</plx:string>
							</plx:passParam>
							<plx:passParam>
								<plx:nameRef name="match"/>
							</plx:passParam>
						</plx:callInstance>
					</xsl:if>
				</xsl:for-each>

				<!-- Add reverse mapping for attributes serialized as elements -->
				<xsl:for-each select="se:Attribute[@WriteStyle='Element' or @WriteStyle='DoubleTaggedElement']">
					<plx:callInstance name="InitializeAttribute">
						<plx:callObject>
							<plx:nameRef name="match"/>
						</plx:callObject>
						<plx:passParam>
							<plx:callStatic name="{@ID}DomainPropertyId" dataTypeName="{$ClassName}" type="field"/>
						</plx:passParam>
						<plx:passParam>
							<xsl:choose>
								<xsl:when test="string-length(@DoubleTagName)">
									<plx:string data="{@DoubleTagName}"/>
								</xsl:when>
								<xsl:otherwise>
									<plx:nullKeyword/>
								</xsl:otherwise>
							</xsl:choose>
						</plx:passParam>
					</plx:callInstance>
					<plx:callInstance name="Add">
						<plx:callObject>
							<plx:nameRef name="childElementMappings"/>
						</plx:callObject>
						<plx:passParam>
							<plx:string>
								<plx:string data="||||"/>
								<plx:string>
									<xsl:call-template name="ResolveNamespace">
										<xsl:with-param name="namespaces" select="$namespaces"/>
										<!-- Use default for prefix parameter -->
									</xsl:call-template>
								</plx:string>
								<plx:string data="|"/>
								<plx:string data="{@ID}">
									<xsl:if test="string-length(@Name)">
										<xsl:attribute name="data">
											<xsl:value-of select="@Name"/>
										</xsl:attribute>
									</xsl:if>
								</plx:string>
							</plx:string>
						</plx:passParam>
						<plx:passParam>
							<plx:nameRef name="match"/>
						</plx:passParam>
					</plx:callInstance>
				</xsl:for-each>
			</xsl:variable>
			<xsl:variable name="mapChildElementBody" select="exsl:node-set($mapChildElementBodyFragment)/child::*"/>
			<xsl:variable name="hasMappedChildElements" select="0!=count($mapChildElementBody)"/>
			<xsl:if test="$hasMappedChildElements">
				<plx:field name="myChildElementMappings" dataTypeName="Dictionary" visibility="private" static="true">
					<plx:passTypeParam dataTypeName=".string"/>
					<plx:passTypeParam dataTypeName="CustomSerializedElementMatch"/>
				</plx:field>
			</xsl:if>
			<xsl:if test="$hasMappedChildElements or not($ClassOverride)">
				<plx:function visibility="{$InterfaceImplementationVisibility}" name="MapChildElement" replacesName="{$ClassOverride}">
					<plx:leadingInfo>
						<plx:docComment>
							<summary>Implements ICustomSerializedElement.MapChildElement</summary>
						</plx:docComment>
					</plx:leadingInfo>
					<plx:interfaceMember dataTypeName="ICustomSerializedElement" memberName="MapChildElement"/>
					<plx:param dataTypeName=".string" name="elementNamespace"/>
					<plx:param dataTypeName=".string" name="elementName"/>
					<plx:param dataTypeName=".string" name="containerNamespace"/>
					<plx:param dataTypeName=".string" name="containerName"/>
					<plx:param dataTypeName=".string" name="outerContainerNamespace"/>
					<plx:param dataTypeName=".string" name="outerContainerName"/>
					<plx:returns dataTypeName="CustomSerializedElementMatch"/>
					<xsl:variable name="forwardToBase">
						<xsl:if test="$ClassOverride">
							<plx:callThis accessor="base" name="MapChildElement">
								<plx:passParam>
									<plx:nameRef name="elementNamespace" type="parameter"/>
								</plx:passParam>
								<plx:passParam>
									<plx:nameRef name="elementName" type="parameter"/>
								</plx:passParam>
								<plx:passParam>
									<plx:nameRef name="containerNamespace" type="parameter"/>
								</plx:passParam>
								<plx:passParam>
									<plx:nameRef name="containerName" type="parameter"/>
								</plx:passParam>
								<plx:passParam>
									<plx:nameRef name="outerContainerNamespace" type="parameter"/>
								</plx:passParam>
								<plx:passParam>
									<plx:nameRef name="outerContainerName" type="parameter"/>
								</plx:passParam>
							</plx:callThis>
						</xsl:if>
					</xsl:variable>
					<xsl:choose>
						<xsl:when test="$hasMappedChildElements">
							<plx:local name="childElementMappings" dataTypeName="Dictionary">
								<plx:passTypeParam dataTypeName=".string"/>
								<plx:passTypeParam dataTypeName="CustomSerializedElementMatch"/>
								<plx:initialize>
									<plx:callStatic dataTypeName="{$ClassName}" name="myChildElementMappings" type="field"/>
								</plx:initialize>
							</plx:local>
							<plx:branch>
								<plx:condition>
									<plx:binaryOperator type="identityEquality">
										<plx:left>
											<plx:nameRef name="childElementMappings"/>
										</plx:left>
										<plx:right>
											<plx:nullKeyword/>
										</plx:right>
									</plx:binaryOperator>
								</plx:condition>
								<plx:assign>
									<plx:left>
										<plx:nameRef name="childElementMappings"/>
									</plx:left>
									<plx:right>
										<plx:callNew dataTypeName="Dictionary">
											<plx:passTypeParam dataTypeName=".string"/>
											<plx:passTypeParam dataTypeName="CustomSerializedElementMatch"/>
										</plx:callNew>
									</plx:right>
								</plx:assign>
								<plx:local name="match" dataTypeName="CustomSerializedElementMatch">
									<plx:initialize>
										<plx:callNew dataTypeName="CustomSerializedElementMatch"/>
									</plx:initialize>
								</plx:local>
								<xsl:copy-of select="$mapChildElementBody"/>
								<plx:assign>
									<plx:left>
										<plx:callStatic dataTypeName="{$ClassName}" name="myChildElementMappings" type="field"/>
									</plx:left>
									<plx:right>
										<plx:nameRef name="childElementMappings"/>
									</plx:right>
								</plx:assign>
							</plx:branch>
							<plx:local name="rVal" dataTypeName="CustomSerializedElementMatch"/>
							<xsl:variable name="lookupCall">
								<plx:callInstance name="TryGetValue">
									<plx:callObject>
										<plx:nameRef name="childElementMappings"/>
									</plx:callObject>
									<plx:passParam>
										<plx:callStatic name="Concat" dataTypeName=".string" type="methodCall">
											<plx:passParam>
												<plx:nameRef name="outerContainerNamespace" type="parameter"/>
											</plx:passParam>
											<plx:passParam>
												<plx:string data="|"/>
											</plx:passParam>
											<plx:passParam>
												<plx:nameRef name="outerContainerName" type="parameter"/>
											</plx:passParam>
											<plx:passParam>
												<plx:string data="|"/>
											</plx:passParam>
											<plx:passParam>
												<plx:inlineStatement dataTypeName=".string">
													<plx:conditionalOperator>
														<plx:condition>
															<plx:binaryOperator type="identityInequality">
																<plx:left>
																	<plx:cast dataTypeName=".object">
																		<plx:nameRef name="containerNamespace" type="parameter"/>
																	</plx:cast>
																</plx:left>
																<plx:right>
																	<plx:cast dataTypeName=".object">
																		<plx:nameRef name="outerContainerNamespace" type="parameter"/>
																	</plx:cast>
																</plx:right>
															</plx:binaryOperator>
														</plx:condition>
														<plx:left>
															<plx:nameRef name="containerNamespace" type="parameter"/>
														</plx:left>
														<plx:right>
															<plx:nullKeyword/>
														</plx:right>
													</plx:conditionalOperator>
												</plx:inlineStatement>
											</plx:passParam>
											<plx:passParam>
												<plx:string data="|"/>
											</plx:passParam>
											<plx:passParam>
												<plx:nameRef name="containerName" type="parameter"/>
											</plx:passParam>
											<plx:passParam>
												<plx:string data="|"/>
											</plx:passParam>
											<plx:passParam>
												<plx:inlineStatement dataTypeName=".string">
													<plx:conditionalOperator>
														<plx:condition>
															<plx:binaryOperator type="identityInequality">
																<plx:left>
																	<plx:cast dataTypeName=".object">
																		<plx:nameRef name="elementNamespace" type="parameter"/>
																	</plx:cast>
																</plx:left>
																<plx:right>
																	<plx:cast dataTypeName=".object">
																		<plx:nameRef name="containerNamespace" type="parameter"/>
																	</plx:cast>
																</plx:right>
															</plx:binaryOperator>
														</plx:condition>
														<plx:left>
															<plx:nameRef name="elementNamespace" type="parameter"/>
														</plx:left>
														<plx:right>
															<plx:nullKeyword/>
														</plx:right>
													</plx:conditionalOperator>
												</plx:inlineStatement>
											</plx:passParam>
											<plx:passParam>
												<plx:string data="|"/>
											</plx:passParam>
											<plx:passParam>
												<plx:nameRef name="elementName" type="parameter"/>
											</plx:passParam>
										</plx:callStatic>
									</plx:passParam>
									<plx:passParam type="out">
										<plx:nameRef name="rVal"/>
									</plx:passParam>
								</plx:callInstance>
							</xsl:variable>
							<xsl:choose>
								<xsl:when test="$ClassOverride">
									<plx:branch>
										<plx:condition>
											<plx:unaryOperator type="booleanNot">
												<xsl:copy-of select="$lookupCall"/>
											</plx:unaryOperator>
										</plx:condition>
										<plx:assign>
											<plx:left>
												<plx:nameRef name="rVal"/>
											</plx:left>
											<plx:right>
												<xsl:copy-of select="$forwardToBase"/>
											</plx:right>
										</plx:assign>
									</plx:branch>
								</xsl:when>
								<xsl:otherwise>
									<xsl:copy-of select="$lookupCall"/>
								</xsl:otherwise>
							</xsl:choose>
							<plx:return>
								<plx:nameRef name="rVal"/>
							</plx:return>
						</xsl:when>
						<xsl:otherwise>
							<plx:return>
								<xsl:choose>
									<xsl:when test="$ClassOverride">
										<xsl:copy-of select="$forwardToBase"/>
									</xsl:when>
									<xsl:otherwise>
										<plx:defaultValueOf dataTypeName="CustomSerializedElementMatch"/>
									</xsl:otherwise>
								</xsl:choose>
							</plx:return>
						</xsl:otherwise>
					</xsl:choose>
				</plx:function>
			</xsl:if>
			<xsl:variable name="attributes" select="se:Attribute[@WriteStyle='Attribute' or not(@WriteStyle) or se:Condition[@WriteStyle='Attribute' or not(@WriteStyle)]]"/>
			<xsl:if test="$attributes">
				<plx:field name="myCustomSerializedAttributes" dataTypeName="Dictionary" visibility="private" static="true">
					<plx:passTypeParam dataTypeName=".string"/>
					<plx:passTypeParam dataTypeName="Guid"/>
				</plx:field>
			</xsl:if>
			<xsl:if test="$attributes or not($ClassOverride)">
				<plx:function visibility="{$InterfaceImplementationVisibility}" name="MapAttribute" replacesName="{$ClassOverride}">
					<plx:leadingInfo>
						<plx:docComment>
							<summary>Implements ICustomSerializedElement.MapAttribute</summary>
						</plx:docComment>
					</plx:leadingInfo>
					<plx:interfaceMember dataTypeName="ICustomSerializedElement" memberName="MapAttribute"/>
					<plx:param dataTypeName=".string" name="xmlNamespace"/>
					<plx:param dataTypeName=".string" name="attributeName"/>
					<plx:returns dataTypeName="Guid"/>
					<xsl:variable name="forwardToBase">
						<xsl:if test="$ClassOverride">
							<plx:callThis accessor="base" name="MapAttribute">
								<plx:passParam>
									<plx:nameRef name="xmlNamespace" type="parameter"/>
								</plx:passParam>
								<plx:passParam>
									<plx:nameRef name="attributeName" type="parameter"/>
								</plx:passParam>
							</plx:callThis>
						</xsl:if>
					</xsl:variable>
					<xsl:choose>
						<xsl:when test="$attributes">
							<plx:local name="customSerializedAttributes" dataTypeName="Dictionary">
								<plx:passTypeParam dataTypeName=".string"/>
								<plx:passTypeParam dataTypeName="Guid"/>
								<plx:initialize>
									<plx:callStatic name="myCustomSerializedAttributes" dataTypeName="{$ClassName}" type="field"/>
								</plx:initialize>
							</plx:local>
							<plx:branch>
								<plx:condition>
									<plx:binaryOperator type="identityEquality">
										<plx:left>
											<plx:nameRef name="customSerializedAttributes"/>
										</plx:left>
										<plx:right>
											<plx:nullKeyword/>
										</plx:right>
									</plx:binaryOperator>
								</plx:condition>
								<plx:assign>
									<plx:left>
										<plx:nameRef name="customSerializedAttributes"/>
									</plx:left>
									<plx:right>
										<plx:callNew dataTypeName="Dictionary">
											<plx:passTypeParam dataTypeName=".string"/>
											<plx:passTypeParam dataTypeName="Guid"/>
										</plx:callNew>
									</plx:right>
								</plx:assign>
								<xsl:for-each select="$attributes">
									<!-- We need to pick up names for both the main element and any child elements with different names -->
									<xsl:variable name="writeSelf" select="@WriteStyle='Attribute' or not(@WriteStyle) or se:Condition[not(string(@Name)) and not(string(@Prefix))][@WriteStyle='Attribute' or not(@WriteStyle)]"/>
									<xsl:variable name="selfName" select="string(@Name)"/>
									<xsl:variable name="selfPrefix" select="string(@Prefix)"/>
									<xsl:variable name="selfID" select="string(@ID)"/>
									<xsl:if test="$writeSelf">
										<plx:callInstance name="Add">
											<plx:callObject>
												<plx:nameRef name="customSerializedAttributes"/>
											</plx:callObject>
											<plx:passParam>
												<plx:string>
													<xsl:if test="$selfPrefix">
														<!-- For attributes, the lack of a prefix means unqualified. Only concatenate if a namespace is explicitly specified -->
														<plx:string>
															<xsl:call-template name="ResolveNamespace">
																<xsl:with-param name="namespaces" select="$namespaces"/>
																<!-- Use default for prefix parameter -->
															</xsl:call-template>
														</plx:string>
														<plx:string data="|"/>
													</xsl:if>
													<plx:string data="{$selfID}">
														<xsl:if test="$selfName">
															<xsl:attribute name="data">
																<xsl:value-of select="$selfName"/>
															</xsl:attribute>
														</xsl:if>
													</plx:string>
												</plx:string>
											</plx:passParam>
											<plx:passParam>
												<plx:callStatic name="{$selfID}DomainPropertyId" dataTypeName="{$ClassName}" type="field"/>
											</plx:passParam>
										</plx:callInstance>
									</xsl:if>
									<xsl:for-each select="se:Condition[string(@Name) or string(@Prefix)][@WriteStyle='Attribute' or not(@WriteStyle)][not($writeSelf) or not(($selfName and @Name=$selfName and string(@Prefix)=$selfPrefix) or (not($selfName) and @Name=current()/@ID and string(@Prefix)=$selfPrefix))]">
										<xsl:variable name="conditionName" select="string(@Name)"/>
										<xsl:variable name="conditionPrefix" select="string(@Prefix)"/>
										<!-- Make sure the names are unique -->
										<xsl:if test="not(preceding-sibling::*[@WriteStyle='Attribute' or not(@WriteStyle)][@Name=$conditionName and string(@Prefix)=$conditionPrefix])">
											<plx:callInstance name="Add">
												<plx:callObject>
													<plx:nameRef name="customSerializedAttributes"/>
												</plx:callObject>
												<plx:passParam>
													<plx:string>
														<xsl:if test="$conditionPrefix">
															<!-- For attributes, the lack of a prefix means unqualified. Only concatenate if a namespace is explicitly specified -->
															<plx:string>
																<xsl:call-template name="ResolveNamespace">
																	<xsl:with-param name="namespaces" select="$namespaces"/>
																	<!-- Use default for prefix parameter -->
																</xsl:call-template>
															</plx:string>
															<plx:string data="|"/>
														</xsl:if>
														<plx:string data="{$selfID}">
															<xsl:if test="$conditionName">
																<xsl:attribute name="data">
																	<xsl:value-of select="$conditionName"/>
																</xsl:attribute>
															</xsl:if>
														</plx:string>
													</plx:string>
												</plx:passParam>
												<plx:passParam>
													<plx:callStatic name="{$selfID}DomainPropertyId" dataTypeName="{$ClassName}" type="field"/>
												</plx:passParam>
											</plx:callInstance>
										</xsl:if>
									</xsl:for-each>
								</xsl:for-each>
								<plx:assign>
									<plx:left>
										<plx:callStatic name="myCustomSerializedAttributes" dataTypeName="{$ClassName}" type="field"/>
									</plx:left>
									<plx:right>
										<plx:nameRef name="customSerializedAttributes"/>
									</plx:right>
								</plx:assign>
							</plx:branch>
							<plx:local name="rVal" dataTypeName="Guid"/>
							<plx:local name="key" dataTypeName=".string">
								<plx:initialize>
									<plx:nameRef name="attributeName" type="parameter"/>
								</plx:initialize>
							</plx:local>
							<plx:branch>
								<plx:condition>
									<plx:binaryOperator type="inequality">
										<plx:left>
											<plx:callInstance name="Length" type="property">
												<plx:callObject>
													<plx:nameRef name="xmlNamespace" type="parameter"/>
												</plx:callObject>
											</plx:callInstance>
										</plx:left>
										<plx:right>
											<plx:value type="i4" data="0"/>
										</plx:right>
									</plx:binaryOperator>
								</plx:condition>
								<plx:assign>
									<plx:left>
										<plx:nameRef name="key"/>
									</plx:left>
									<plx:right>
										<plx:callStatic name="Concat" dataTypeName=".string">
											<plx:passParam>
												<plx:nameRef name="xmlNamespace" type="parameter"/>
											</plx:passParam>
											<plx:passParam>
												<plx:string data="|"/>
											</plx:passParam>
											<plx:passParam>
												<plx:nameRef name="attributeName" type="parameter"/>
											</plx:passParam>
										</plx:callStatic>
									</plx:right>
								</plx:assign>
							</plx:branch>
							<xsl:variable name="lookupCall">
								<plx:callInstance name="TryGetValue">
									<plx:callObject>
										<plx:nameRef name="customSerializedAttributes"/>
									</plx:callObject>
									<plx:passParam>
										<plx:nameRef name="key"/>
									</plx:passParam>
									<plx:passParam type="out">
										<plx:nameRef name="rVal"/>
									</plx:passParam>
								</plx:callInstance>
							</xsl:variable>
							<xsl:choose>
								<xsl:when test="$ClassOverride">
									<plx:branch>
										<plx:condition>
											<plx:unaryOperator type="booleanNot">
												<xsl:copy-of select="$lookupCall"/>
											</plx:unaryOperator>
										</plx:condition>
										<plx:assign>
											<plx:left>
												<plx:nameRef name="rVal"/>
											</plx:left>
											<plx:right>
												<xsl:copy-of select="$forwardToBase"/>
											</plx:right>
										</plx:assign>
									</plx:branch>
								</xsl:when>
								<xsl:otherwise>
									<xsl:copy-of select="$lookupCall"/>
								</xsl:otherwise>
							</xsl:choose>
							<plx:return>
								<plx:nameRef name="rVal"/>
							</plx:return>
						</xsl:when>
						<xsl:otherwise>
							<plx:return>
								<xsl:choose>
									<xsl:when test="$ClassOverride">
										<xsl:copy-of select="$forwardToBase"/>
									</xsl:when>
									<xsl:otherwise>
										<plx:defaultValueOf dataTypeName="Guid"/>
									</xsl:otherwise>
								</xsl:choose>
							</plx:return>
						</xsl:otherwise>
					</xsl:choose>
				</plx:function>
			</xsl:if>
			<xsl:variable name="serializeBody" select="se:ConditionalSerialization/child::plx:*"/>
			<xsl:if test="$serializeBody or not($ClassOverride)">
				<plx:function visibility="{$InterfaceImplementationVisibility}" name="ShouldSerialize" replacesName="{$ClassOverride}">
					<xsl:if test="not($serializeBody and count($serializeBody/descendant::plx:thisKeyword | $serializeBody/descendant::plx:callThis[not(@accessor!='static')]))">
						<xsl:attribute name="modifier">
							<xsl:text>static</xsl:text>
						</xsl:attribute>
					</xsl:if>
					<plx:leadingInfo>
						<plx:docComment>
							<summary>Implements ICustomSerializedElement.ShouldSerialize</summary>
						</plx:docComment>
					</plx:leadingInfo>
					<plx:interfaceMember dataTypeName="ICustomSerializedElement" memberName="ShouldSerialize"/>
					<plx:returns dataTypeName=".boolean"/>
					<xsl:choose>
						<xsl:when test="$serializeBody">
							<xsl:copy-of select="$serializeBody"/>
						</xsl:when>
						<xsl:otherwise>
							<plx:return>
								<plx:trueKeyword/>
							</plx:return>
						</xsl:otherwise>
					</xsl:choose>
				</plx:function>
			</xsl:if>
		</plx:class>
	</xsl:template>
	<xsl:template name="DomainRoleInfoFullName">
		<xsl:param name="DomainRoleInfoExpression"/>
		<plx:callStatic name="Concat" dataTypeName=".string">
			<plx:passParam>
				<plx:callInstance name="FullName" type="property">
					<plx:callObject>
						<plx:callInstance name="ImplementationClass" type="property">
							<plx:callObject>
								<plx:callInstance name="DomainRelationship" type="property">
									<plx:callObject>
										<xsl:copy-of select="$DomainRoleInfoExpression"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:callObject>
						</plx:callInstance>
					</plx:callObject>
				</plx:callInstance>
			</plx:passParam>
			<plx:passParam>
				<plx:string data="."/>
			</plx:passParam>
			<plx:passParam>
				<plx:callInstance name="Name" type="property">
					<plx:callObject>
						<xsl:copy-of select="$DomainRoleInfoExpression"/>
					</plx:callObject>
				</plx:callInstance>
			</plx:passParam>
		</plx:callStatic>
	</xsl:template>
	<xsl:template name="ResolveNamespace">
		<xsl:param name="namespaces"/>
		<xsl:param name="prefix" select="@Prefix"/>
		<xsl:choose>
			<xsl:when test="string-length($prefix)">
				<xsl:value-of select="$namespaces[@Prefix=$prefix]/@URI"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$namespaces[@DefaultPrefix='true']/@URI"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="se:DomainModel">
		<xsl:param name="namespaces"/>
		<xsl:param name="defaultNamespace"/>
		<xsl:variable name="ModelName" select="@Class"/>
		<xsl:variable name="ClassSealed" select="@Sealed='true'"/>
		<xsl:variable name="InterfaceImplementationVisibility">
			<xsl:choose>
				<xsl:when test="$ClassSealed">
					<xsl:value-of select="'privateInterfaceMember'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="'protected'"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<plx:class name="{$ModelName}" visibility="deferToPartial" partial="true">
			<xsl:if test="$ClassSealed">
				<!-- Make sure it is really sealed by also rendering the modifier here. -->
				<xsl:attribute name="modifier">
					<xsl:value-of select="'sealed'"/>
				</xsl:attribute>
			</xsl:if>
			<plx:leadingInfo>
				<plx:pragma type="region" data="{$ModelName} model serialization"/>
			</plx:leadingInfo>
			<plx:trailingInfo>
				<plx:pragma type="closeRegion" data="{$ModelName} model serialization"/>
			</plx:trailingInfo>
			<xsl:if test="$namespaces">
				<plx:attribute dataTypeName="CustomSerializedXmlNamespaces">
					<xsl:for-each select="$namespaces">
						<plx:passParam>
							<plx:string data="{@URI}"/>
						</plx:passParam>
					</xsl:for-each>
				</plx:attribute>
			</xsl:if>
			<plx:implementsInterface dataTypeName="ICustomSerializedDomainModel"/>
			<plx:field name="XmlNamespace" visibility="public" static="true" readOnly="true" dataTypeName=".string">
				<plx:leadingInfo>
					<plx:docComment>
						<summary>The default XmlNamespace associated with the '<xsl:value-of select="$ModelName"/>' extension model</summary>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:initialize>
					<plx:string data="{$defaultNamespace/@URI}"/>
				</plx:initialize>
			</plx:field>
			<xsl:for-each select="se:Namespaces">
				<plx:property visibility="{$InterfaceImplementationVisibility}" name="DefaultElementPrefix" modifier="static">
					<plx:leadingInfo>
						<plx:docComment>
							<summary>Implements ICustomSerializedDomainModel.DefaultElementPrefix</summary>
						</plx:docComment>
					</plx:leadingInfo>
					<plx:interfaceMember dataTypeName="ICustomSerializedDomainModel" memberName="DefaultElementPrefix"/>
					<plx:returns dataTypeName=".string"/>
					<plx:get>
						<plx:return>
							<xsl:variable name="defaultElement" select="se:Namespace[@DefaultPrefix='true']"/>
							<xsl:choose>
								<xsl:when test="count($defaultElement)">
									<plx:string data="{$defaultElement/@Prefix}"/>
								</xsl:when>
								<xsl:otherwise>
									<plx:nullKeyword/>
								</xsl:otherwise>
							</xsl:choose>
						</plx:return>
					</plx:get>
				</plx:property>
				<plx:function visibility="{$InterfaceImplementationVisibility}" name="GetCustomElementNamespaces" modifier="static">
					<plx:leadingInfo>
						<plx:docComment>
							<summary>Implements ICustomSerializedDomainModel.GetCustomElementNamespaces</summary>
						</plx:docComment>
					</plx:leadingInfo>
					<plx:interfaceMember dataTypeName="ICustomSerializedDomainModel" memberName="GetCustomElementNamespaces"/>
					<plx:returns dataTypeName=".string">
						<plx:arrayDescriptor rank="2"/>
					</plx:returns>
					<plx:local name="ret" dataTypeName=".string">
						<plx:arrayDescriptor rank="2"/>
						<plx:initialize>
							<plx:callNew dataTypeName=".string">
								<plx:arrayDescriptor rank="2"/>
								<plx:passParam>
									<plx:value type="i4" data="{count(se:Namespace)}"/>
								</plx:passParam>
								<plx:passParam>
									<plx:value type="i4" data="3"/>
								</plx:passParam>
							</plx:callNew>
						</plx:initialize>
					</plx:local>
					<xsl:for-each select="se:Namespace">
						<plx:assign>
							<plx:left>
								<plx:callInstance name=".implied" type="arrayIndexer">
									<plx:callObject>
										<plx:nameRef name="ret"/>
									</plx:callObject>
									<plx:passParam>
										<plx:value type="i4" data="{position()-1}"/>
									</plx:passParam>
									<plx:passParam>
										<plx:value type="i4" data="0"/>
									</plx:passParam>
								</plx:callInstance>
							</plx:left>
							<plx:right>
								<plx:string data="{@Prefix}"/>
							</plx:right>
						</plx:assign>
						<plx:assign>
							<plx:left>
								<plx:callInstance name=".implied" type="arrayIndexer">
									<plx:callObject>
										<plx:nameRef name="ret"/>
									</plx:callObject>
									<plx:passParam>
										<plx:value type="i4" data="{position()-1}"/>
									</plx:passParam>
									<plx:passParam>
										<plx:value type="i4" data="1"/>
									</plx:passParam>
								</plx:callInstance>
							</plx:left>
							<plx:right>
								<plx:string data="{@URI}"/>
							</plx:right>
						</plx:assign>
						<plx:assign>
							<plx:left>
								<plx:callInstance name=".implied" type="arrayIndexer">
									<plx:callObject>
										<plx:nameRef name="ret"/>
									</plx:callObject>
									<plx:passParam>
										<plx:value type="i4" data="{position()-1}"/>
									</plx:passParam>
									<plx:passParam>
										<plx:value type="i4" data="2"/>
									</plx:passParam>
								</plx:callInstance>
							</plx:left>
							<plx:right>
								<plx:string data="{@SchemaFile}"/>
							</plx:right>
						</plx:assign>
					</xsl:for-each>
					<plx:return>
						<plx:nameRef name="ret"/>
					</plx:return>
				</plx:function>
			</xsl:for-each>
			<xsl:variable name="hasOmittedElements" select="0!=count(se:OmittedDomainElements/child::se:*)"/>
			<xsl:if test="$hasOmittedElements">
				<plx:field name="myCustomSerializationOmissions" dataTypeName="Dictionary" visibility="private">
					<plx:passTypeParam dataTypeName="DomainClassInfo"/>
					<plx:passTypeParam dataTypeName=".object"/>
				</plx:field>
				<plx:function name="BuildCustomSerializationOmissions" visibility="private" modifier="static">
					<plx:param name="store" dataTypeName="Store"/>
					<plx:returns dataTypeName="Dictionary">
						<plx:passTypeParam dataTypeName="DomainClassInfo"/>
						<plx:passTypeParam dataTypeName=".object"/>
					</plx:returns>
					<plx:local name="retVal" dataTypeName="Dictionary">
						<plx:passTypeParam dataTypeName="DomainClassInfo"/>
						<plx:passTypeParam dataTypeName=".object"/>
						<plx:initialize>
							<plx:callNew dataTypeName="Dictionary">
								<plx:passTypeParam dataTypeName="DomainClassInfo"/>
								<plx:passTypeParam dataTypeName=".object"/>
							</plx:callNew>
						</plx:initialize>
					</plx:local>
					<plx:local name="dataDir" dataTypeName="DomainDataDirectory">
						<plx:initialize>
							<plx:callInstance name="DomainDataDirectory" type="property">
								<plx:callObject>
									<plx:nameRef name="store" type="parameter"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:initialize>
					</plx:local>
					<xsl:for-each select="se:OmittedDomainElements/child::se:*">
						<xsl:variable name="classOrRelationship">
							<xsl:choose>
								<xsl:when test="self::se:OmitClass">
									<xsl:text>Class</xsl:text>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>Relationship</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<plx:assign>
							<plx:left>
								<plx:callInstance name=".implied" type="indexerCall">
									<plx:callObject>
										<plx:nameRef name="retVal"/>
									</plx:callObject>
									<plx:passParam>
										<plx:callInstance name="FindDomain{$classOrRelationship}">
											<plx:callObject>
												<plx:nameRef name="dataDir"/>
											</plx:callObject>
											<plx:passParam>
												<plx:callStatic name="DomainClassId" type="field" dataTypeName="{@Class}" dataTypeQualifier="{@Namespace}"/>
											</plx:passParam>
										</plx:callInstance>
									</plx:passParam>
								</plx:callInstance>
							</plx:left>
							<plx:right>
								<plx:nullKeyword/>
							</plx:right>
						</plx:assign>
					</xsl:for-each>
					<plx:return>
						<plx:nameRef name="retVal"/>
					</plx:return>
				</plx:function>
			</xsl:if>
			<plx:field name="myClassNameMap" dataTypeName="Dictionary" visibility="private" static="true">
				<plx:passTypeParam dataTypeName=".string"/>
				<plx:passTypeParam dataTypeName="Guid"/>
			</plx:field>
			<plx:field name="myValidNamespaces" dataTypeName="Collection" visibility="private" static="true">
				<plx:passTypeParam dataTypeName=".string"/>
			</plx:field>
			<xsl:if test="se:RootLinks/se:Container">
				<plx:field name="myRootRelationshipContainers" dataTypeName="CustomSerializedRootRelationshipContainer" dataTypeIsSimpleArray="true" visibility="private" static="true"/>
			</xsl:if>
			<plx:function visibility="{$InterfaceImplementationVisibility}" name="ShouldSerializeDomainClass">
				<plx:leadingInfo>
					<plx:docComment>
						<summary>Implements ICustomSerializedDomainModel.ShouldSerializeDomainClass</summary>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:interfaceMember dataTypeName="ICustomSerializedDomainModel" memberName="ShouldSerializeDomainClass"/>
				<plx:param name="store" dataTypeName="Store"/>
				<plx:param name="classInfo" dataTypeName="DomainClassInfo"/>
				<plx:returns dataTypeName=".boolean"/>
				<xsl:choose>
					<xsl:when test="$hasOmittedElements">
						<plx:local name="omissions" dataTypeName="Dictionary">
							<plx:passTypeParam dataTypeName="DomainClassInfo"/>
							<plx:passTypeParam dataTypeName=".object"/>
							<plx:initialize>
								<plx:callThis name="myCustomSerializationOmissions" type="field"/>
							</plx:initialize>
						</plx:local>
						<plx:branch>
							<plx:condition>
								<plx:binaryOperator type="identityEquality">
									<plx:left>
										<plx:nameRef name="omissions"/>
									</plx:left>
									<plx:right>
										<plx:nullKeyword/>
									</plx:right>
								</plx:binaryOperator>
							</plx:condition>
							<plx:assign>
								<plx:left>
									<plx:nameRef name="omissions"/>
								</plx:left>
								<plx:right>
									<plx:callStatic name="BuildCustomSerializationOmissions" dataTypeName="{$ModelName}">
										<plx:passParam>
											<plx:nameRef name="store" type="parameter"/>
										</plx:passParam>
									</plx:callStatic>
								</plx:right>
							</plx:assign>
							<plx:assign>
								<plx:left>
									<plx:callThis name="myCustomSerializationOmissions" type="field"/>
								</plx:left>
								<plx:right>
									<plx:nameRef name="omissions"/>
								</plx:right>
							</plx:assign>
						</plx:branch>
						<plx:return>
							<plx:unaryOperator type="booleanNot">
								<plx:callInstance name="ContainsKey">
									<plx:callObject>
										<plx:nameRef name="omissions"/>
									</plx:callObject>
									<plx:passParam>
										<plx:nameRef name="classInfo" type="parameter"/>
									</plx:passParam>
								</plx:callInstance>
							</plx:unaryOperator>
						</plx:return>
					</xsl:when>
					<xsl:otherwise>
						<plx:return>
							<plx:trueKeyword/>
						</plx:return>
					</xsl:otherwise>
				</xsl:choose>
			</plx:function>
			<plx:function visibility="{$InterfaceImplementationVisibility}" name="GetRootElementClasses" modifier="static">
				<plx:leadingInfo>
					<plx:docComment>
						<summary>Implements ICustomSerializedDomainModel.GetRootElementClasses</summary>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:interfaceMember dataTypeName="ICustomSerializedDomainModel" memberName="GetRootElementClasses"/>
				<plx:returns dataTypeName="Guid" dataTypeIsSimpleArray="true"/>
				<plx:return>
					<plx:callNew dataTypeName="Guid" dataTypeIsSimpleArray="true">
						<xsl:variable name="rootElements" select="se:RootElements/se:RootElement"/>
						<xsl:choose>
							<xsl:when test="$rootElements">
								<plx:arrayInitializer>
									<xsl:for-each select="$rootElements">
										<plx:callStatic dataTypeName="{@Class}" name="DomainClassId" type="field"/>
									</xsl:for-each>
								</plx:arrayInitializer>
							</xsl:when>
							<xsl:otherwise>
								<plx:passParam>
									<plx:value type="i4" data="0"/>
								</plx:passParam>
							</xsl:otherwise>
						</xsl:choose>
					</plx:callNew>
				</plx:return>
			</plx:function>
			<plx:function visibility="{$InterfaceImplementationVisibility}" name="ShouldSerializeRootElement" modifier="static">
				<plx:leadingInfo>
					<plx:docComment>
						<summary>Implements ICustomSerializedDomainModel.ShouldSerializeRootElement</summary>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:interfaceMember dataTypeName="ICustomSerializedDomainModel" memberName="ShouldSerializeRootElement"/>
				<plx:param name="element" dataTypeName="ModelElement"/>
				<plx:returns dataTypeName=".boolean"/>
				<xsl:variable name="rootElementConditions" select=".//se:ConditionalSerialization"/>
				<xsl:if test="$rootElementConditions">
					<plx:local name="elementDomainClass" dataTypeName="DomainClassInfo">
						<plx:initialize>
							<plx:callInstance name="GetDomainClass">
								<plx:callObject>
									<plx:nameRef name="element" type="parameter"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:initialize>
					</plx:local>
					<xsl:for-each select="$rootElementConditions">
						<xsl:variable name="elementName">
							<xsl:choose>
								<xsl:when test="position()=1">
									<xsl:text>branch</xsl:text>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>alternateBranch</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:element name="plx:{$elementName}">
							<plx:condition>
								<plx:callInstance name="IsDerivedFrom">
									<plx:callObject>
										<plx:nameRef name="elementDomainClass"/>
									</plx:callObject>
									<plx:passParam>
										<plx:callStatic name="DomainClassId" dataTypeName="{../@Class}" type="field"/>
									</plx:passParam>
								</plx:callInstance>
							</plx:condition>
							<xsl:copy-of select="child::plx:*"/>
						</xsl:element>
					</xsl:for-each>
				</xsl:if>
				<plx:return>
					<plx:trueKeyword/>
				</plx:return>
			</plx:function>
			<plx:function visibility="{$InterfaceImplementationVisibility}" name="GetRootRelationshipContainers" modifier="static">
				<plx:leadingInfo>
					<plx:docComment>
						<summary>Implements ICustomSerializedDomainModel.GetRootRelationshipContainers</summary>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:interfaceMember dataTypeName="ICustomSerializedDomainModel" memberName="GetRootRelationshipContainers"/>
				<plx:returns dataTypeName="CustomSerializedRootRelationshipContainer" dataTypeIsSimpleArray="true"/>
				<xsl:variable name="rootLinkContainers" select="se:RootLinks/se:Container"/>
				<xsl:choose>
					<xsl:when test="$rootLinkContainers">
						<plx:local name="retVal" dataTypeName="CustomSerializedRootRelationshipContainer" dataTypeIsSimpleArray="true">
							<plx:initialize>
								<plx:callStatic dataTypeName="{$ModelName}" name="myRootRelationshipContainers" type="field"/>
							</plx:initialize>
						</plx:local>
						<plx:branch>
							<plx:condition>
								<plx:binaryOperator type="identityEquality">
									<plx:left>
										<plx:nameRef name="retVal"/>
									</plx:left>
									<plx:right>
										<plx:nullKeyword/>
									</plx:right>
								</plx:binaryOperator>
							</plx:condition>
							<plx:assign>
								<plx:left>
									<plx:nameRef name="retVal"/>
								</plx:left>
								<plx:right>
									<plx:callNew dataTypeName="CustomSerializedRootRelationshipContainer" dataTypeIsSimpleArray="true">
										<plx:arrayInitializer>
											<xsl:for-each select="$rootLinkContainers">
												<plx:callNew dataTypeName="CustomSerializedRootRelationshipContainer">
													<plx:passParam>
														<plx:string data="{$defaultNamespace/@Prefix}">
															<xsl:if test="string(@Prefix)">
																<xsl:attribute name="data">
																	<xsl:value-of select="@Prefix"/>
																</xsl:attribute>
															</xsl:if>
														</plx:string>
													</plx:passParam>
													<plx:passParam>
														<plx:string data="{@Name}"/>
													</plx:passParam>
													<plx:passParam>
														<plx:string data="{$defaultNamespace/@URI}">
															<xsl:if test="string(@Prefix)">
																<xsl:attribute name="data">
																	<xsl:value-of select="$namespaces[@Prefix=current()/@Prefix]/@URI"/>
																</xsl:attribute>
															</xsl:if>
														</plx:string>
													</plx:passParam>
													<plx:passParam>
														<plx:callNew dataTypeName="CustomSerializedStandaloneRelationship" dataTypeIsSimpleArray="true">
															<plx:arrayInitializer>
																<xsl:for-each select="se:RootLink">
																	<xsl:call-template name="CreateStandaloneRelationship">
																		<xsl:with-param name="namespaces" select="$namespaces"/>
																		<xsl:with-param name="defaultNamespace" select="$defaultNamespace"/>
																	</xsl:call-template>
																</xsl:for-each>
															</plx:arrayInitializer>
														</plx:callNew>
													</plx:passParam>
												</plx:callNew>
											</xsl:for-each>
										</plx:arrayInitializer>
									</plx:callNew>
								</plx:right>
							</plx:assign>
							<plx:assign>
								<plx:left>
									<plx:callStatic dataTypeName="{$ModelName}" name="myRootRelationshipContainers" type="field"/>
								</plx:left>
								<plx:right>
									<plx:nameRef name="retVal"/>
								</plx:right>
							</plx:assign>
						</plx:branch>
						<plx:return>
							<plx:nameRef name="retVal"/>
						</plx:return>
					</xsl:when>
					<xsl:otherwise>
						<plx:return>
							<plx:nullKeyword/>
						</plx:return>
					</xsl:otherwise>
				</xsl:choose>
			</plx:function>
			<plx:function visibility="{$InterfaceImplementationVisibility}" name="MapRootElement" modifier="static">
				<plx:leadingInfo>
					<plx:docComment>
						<summary>Implements ICustomSerializedDomainModel.MapRootElement</summary>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:interfaceMember dataTypeName="ICustomSerializedDomainModel" memberName="MapRootElement"/>
				<plx:param name="xmlNamespace" dataTypeName=".string"/>
				<plx:param name="elementName" dataTypeName=".string"/>
				<plx:returns dataTypeName="Guid"/>
				<xsl:for-each select="se:RootElements/se:RootElement">
					<xsl:variable name="className" select="@Class"/>
					<xsl:variable name="tagName">
						<xsl:choose>
							<xsl:when test="string-length(@Name)">
								<xsl:value-of select="@Name"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$className"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<xsl:variable name="namespace">
						<xsl:call-template name="ResolveNamespace">
							<xsl:with-param name="namespaces" select="$namespaces"/>
							<!-- Use default for prefix parameter -->
						</xsl:call-template>
					</xsl:variable>
					<plx:branch>
						<plx:condition>
							<plx:binaryOperator type="booleanAnd">
								<plx:left>
									<plx:binaryOperator type="equality">
										<plx:left>
											<plx:nameRef name="elementName" type="parameter"/>
										</plx:left>
										<plx:right>
											<plx:string data="{$tagName}"/>
										</plx:right>
									</plx:binaryOperator>
								</plx:left>
								<plx:right>
									<plx:binaryOperator type="equality">
										<plx:left>
											<plx:nameRef name="xmlNamespace" type="parameter"/>
										</plx:left>
										<plx:right>
											<plx:string data="{$namespace}"/>
										</plx:right>
									</plx:binaryOperator>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:return>
							<plx:callStatic dataTypeName="{$className}" name="DomainClassId" type="field"/>
						</plx:return>
					</plx:branch>
				</xsl:for-each>
				<plx:return>
					<plx:defaultValueOf dataTypeName="Guid"/>
				</plx:return>
			</plx:function>
			<plx:function visibility="{$InterfaceImplementationVisibility}" name="MapClassName" modifier="static">
				<plx:leadingInfo>
					<plx:docComment>
						<summary>Implements ICustomSerializedDomainModel.MapClassName</summary>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:interfaceMember dataTypeName="ICustomSerializedDomainModel" memberName="MapClassName"/>
				<plx:param name="xmlNamespace" dataTypeName=".string"/>
				<plx:param name="elementName" dataTypeName=".string"/>
				<plx:returns dataTypeName="Guid"/>
				<plx:local name="validNamespaces" dataTypeName="Collection">
					<plx:passTypeParam dataTypeName=".string"/>
					<plx:initialize>
						<plx:callStatic dataTypeName="{$ModelName}" name="myValidNamespaces" type="field"/>
					</plx:initialize>
				</plx:local>
				<plx:local name="classNameMap" dataTypeName="Dictionary">
					<plx:passTypeParam dataTypeName=".string"/>
					<plx:passTypeParam dataTypeName="Guid"/>
					<plx:initialize>
						<plx:callStatic dataTypeName="{$ModelName}" name="myClassNameMap" type="field"/>
					</plx:initialize>
				</plx:local>
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="identityEquality">
							<plx:left>
								<plx:nameRef name="validNamespaces"/>
							</plx:left>
							<plx:right>
								<plx:nullKeyword/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:assign>
						<plx:left>
							<plx:nameRef name="validNamespaces"/>
						</plx:left>
						<plx:right>
							<plx:callNew dataTypeName="Collection">
								<plx:passTypeParam dataTypeName=".string"/>
							</plx:callNew>
						</plx:right>
					</plx:assign>
					<xsl:for-each select="se:Namespaces/se:Namespace">
						<plx:callInstance name="Add" type="methodCall">
							<plx:callObject>
								<plx:nameRef name="validNamespaces"/>
							</plx:callObject>
							<plx:passParam>
								<plx:string data="{@URI}"/>
							</plx:passParam>
						</plx:callInstance>
					</xsl:for-each>
					<plx:assign>
						<plx:left>
							<plx:callStatic dataTypeName="{$ModelName}" name="myValidNamespaces" type="field"/>
						</plx:left>
						<plx:right>
							<plx:nameRef name="validNamespaces"/>
						</plx:right>
					</plx:assign>
				</plx:branch>
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="equality">
							<plx:left>
								<plx:nameRef name="classNameMap"/>
							</plx:left>
							<plx:right>
								<plx:nullKeyword/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:assign>
						<plx:left>
							<plx:nameRef name="classNameMap"/>
						</plx:left>
						<plx:right>
							<plx:callNew dataTypeName="Dictionary">
								<plx:passTypeParam dataTypeName=".string"/>
								<plx:passTypeParam dataTypeName="Guid"/>
							</plx:callNew>
						</plx:right>
					</plx:assign>
					<xsl:variable name="LocalNamespace" select="$CustomToolNamespace"/>
					<xsl:for-each select="../se:Element[not(@LinkOnly='true' or @LinkOnly=1)]">
						<plx:callInstance name="Add">
							<plx:callObject>
								<plx:nameRef name="classNameMap"/>
							</plx:callObject>
							<plx:passParam>
								<plx:string data="{@Class}">
									<xsl:if test="string-length(@Name)">
										<xsl:attribute name="data">
											<xsl:value-of select="@Name"/>
										</xsl:attribute>
									</xsl:if>
								</plx:string>
							</plx:passParam>
							<plx:passParam>
								<plx:callStatic name="DomainClassId" dataTypeName="{@Class}" type="field"/>
							</plx:passParam>
						</plx:callInstance>
						<!-- Handle the less obvious Conditional Names -->
						<xsl:variable name="className" select="@Class"/>
						<xsl:for-each select="se:ConditionalName">
							<plx:callInstance name="Add">
								<plx:callObject>
									<plx:nameRef name="classNameMap"/>
								</plx:callObject>
								<plx:passParam>
									<plx:string data="{@Name}"/>
								</plx:passParam>
								<plx:passParam>
									<plx:callStatic name="DomainClassId" dataTypeName="{$className}" type="field"/>
								</plx:passParam>
							</plx:callInstance>
						</xsl:for-each>
					</xsl:for-each>
					<plx:assign>
						<plx:left>
							<plx:callStatic dataTypeName="{$ModelName}" name="myClassNameMap" type="field"/>
						</plx:left>
						<plx:right>
							<plx:nameRef name="classNameMap"/>
						</plx:right>
					</plx:assign>
				</plx:branch>
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="booleanAnd">
							<plx:left>
								<plx:callInstance name="Contains" type="methodCall">
									<plx:callObject>
										<plx:nameRef name="validNamespaces"/>
									</plx:callObject>
									<plx:passParam>
										<plx:nameRef name="xmlNamespace"/>
									</plx:passParam>
								</plx:callInstance>
							</plx:left>
							<plx:right>
								<plx:callInstance name="ContainsKey" type="methodCall">
									<plx:callObject>
										<plx:nameRef name="classNameMap"/>
									</plx:callObject>
									<plx:passParam>
										<plx:nameRef name="elementName" type="parameter"/>
									</plx:passParam>
								</plx:callInstance>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:return>
						<plx:callInstance name=".implied" type="indexerCall">
							<plx:callObject>
								<plx:nameRef name="classNameMap"/>
							</plx:callObject>
							<plx:passParam>
								<plx:nameRef name="elementName" type="parameter"/>
							</plx:passParam>
						</plx:callInstance>
					</plx:return>
				</plx:branch>
				<plx:return>
					<plx:defaultValueOf dataTypeName="Guid"/>
				</plx:return>
			</plx:function>
		</plx:class>
	</xsl:template>
	<xsl:template name="ReturnCustomSerializedElementSupportedOperations">
		<xsl:param name="containerElements"/>
		<xsl:param name="element"/>
		<xsl:param name="attributes"/>
		<xsl:param name="links"/>
		<xsl:param name="aggregatingLinks"/>
		<xsl:param name="customSort"/>
		<xsl:param name="mixedTypedAttributes"/>
		<xsl:variable name="supportedOperationsFragment">
			<xsl:if test="$containerElements">
				<xsl:element name="SupportedOperation">
					<xsl:text>ChildElementInfo</xsl:text>
				</xsl:element>
			</xsl:if>
			<xsl:if test="$element">
				<xsl:element name="SupportedOperation">
					<xsl:text>ElementInfo</xsl:text>
				</xsl:element>
			</xsl:if>
			<xsl:if test="$attributes">
				<xsl:element name="SupportedOperation">
					<xsl:text>PropertyInfo</xsl:text>
				</xsl:element>
			</xsl:if>
			<xsl:if test="$links">
				<xsl:element name="SupportedOperation">
					<xsl:text>LinkInfo</xsl:text>
				</xsl:element>
			</xsl:if>
			<xsl:if test="$customSort">
				<xsl:element name="SupportedOperation">
					<xsl:text>CustomSortChildRoles</xsl:text>
				</xsl:element>
			</xsl:if>
			<xsl:if test="$mixedTypedAttributes">
				<xsl:element name="SupportedOperation">
					<xsl:text>MixedTypedAttributes</xsl:text>
				</xsl:element>
			</xsl:if>
			<xsl:if test="$aggregatingLinks">
				<xsl:element name="SupportedOperation">
					<xsl:text>EmbeddingLinkInfo</xsl:text>
				</xsl:element>
			</xsl:if>
		</xsl:variable>
		<xsl:variable name="supportedOperations" select="exsl:node-set($supportedOperationsFragment)"/>
		<xsl:variable name="operationCount" select="count($supportedOperations/child::*)"/>
		<xsl:choose>
			<xsl:when test="$operationCount=0">
				<plx:callStatic dataTypeName="CustomSerializedElementSupportedOperations" name="None" type="field"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:for-each select="$supportedOperations/SupportedOperation">
					<xsl:if test="position()=1">
						<xsl:call-template name="OrTogetherEnumElements">
							<xsl:with-param name="EnumType" select="'CustomSerializedElementSupportedOperations'"></xsl:with-param>
						</xsl:call-template>
					</xsl:if>
				</xsl:for-each>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- Or together enum values from the given type. The current state on the initial
	     call should be the position()=1 element inside a for-each context where the elements
		 contain the (unqualified) names of the enum values to or together -->
	<xsl:template name="OrTogetherEnumElements">
		<xsl:param name="EnumType"/>
		<xsl:choose>
			<xsl:when test="position()=last()">
				<plx:callStatic dataTypeName="{$EnumType}" name="{.}" type="field"/>
			</xsl:when>
			<xsl:otherwise>
				<plx:binaryOperator type="bitwiseOr">
					<plx:left>
						<plx:callStatic dataTypeName="{$EnumType}" name="{.}" type="field"/>
					</plx:left>
					<plx:right>
						<xsl:for-each select="following-sibling::*">
							<xsl:if test="position()=1">
								<xsl:call-template name="OrTogetherEnumElements">
									<xsl:with-param name="EnumType" select="$EnumType"/>
								</xsl:call-template>
							</xsl:if>
						</xsl:for-each>
					</plx:right>
				</plx:binaryOperator>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="CreateCustomSerializedElementInfoNameVariable">
		<xsl:param name="modifier"/>
		<xsl:variable name="conditionalNames" select="se:ConditionalName"/>
		<xsl:if test="$conditionalNames">
			<plx:local dataTypeName=".string" name="name{$modifier}">
				<plx:initialize>
					<xsl:choose>
						<xsl:when test="string-length(@Name)">
							<plx:string data="{@Name}"/>
						</xsl:when>
						<xsl:otherwise>
							<plx:nullKeyword/>
						</xsl:otherwise>
					</xsl:choose>
				</plx:initialize>
			</plx:local>
			<xsl:variable name="primaryWriteStyle" select="string(@WriteStyle)"/>
			<xsl:if test="$conditionalNames/@WriteStyle[not(.=$primaryWriteStyle)]">
				<plx:local dataTypeName="CustomSerializedElementWriteStyle" name="writeStyle{$modifier}">
					<plx:initialize>
						<plx:callStatic name="Element" dataTypeName="CustomSerializedElementWriteStyle" type="field">
							<xsl:if test="$primaryWriteStyle">
								<xsl:attribute name="name">
									<xsl:value-of select="$primaryWriteStyle"/>
								</xsl:attribute>
							</xsl:if>
						</plx:callStatic>
					</plx:initialize>
				</plx:local>
			</xsl:if>
			<xsl:variable name="primaryDoubleTagName" select="string(@DoubleTagName)"/>
			<xsl:if test="$conditionalNames/@DoubleTagName[not(.=$primaryDoubleTagName)]">
				<plx:local dataTypeName=".string" name="doubleTagName{$modifier}">
					<plx:initialize>
						<xsl:choose>
							<xsl:when test="$primaryDoubleTagName">
								<plx:string data="{$primaryDoubleTagName}"/>
							</xsl:when>
							<xsl:otherwise>
								<plx:nullKeyword/>
							</xsl:otherwise>
						</xsl:choose>
					</plx:initialize>
				</plx:local>
			</xsl:if>
			<xsl:for-each select="$conditionalNames">
				<xsl:variable name="allStatements" select="child::*"/>
				<xsl:variable name="preConditionStatements" select="$allStatements[position()!=last()]"/>
				<xsl:variable name="conditionFragment">
					<xsl:copy-of select="$preConditionStatements"/>
					<xsl:variable name="branchType">
						<xsl:choose>
							<xsl:when test="position()=1 or $preConditionStatements">
								<xsl:text>plx:branch</xsl:text>
							</xsl:when>
							<xsl:otherwise>
								<xsl:text>plx:alternateBranch</xsl:text>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<xsl:element name="{$branchType}">
						<plx:condition>
							<xsl:copy-of select="$allStatements[last()]"/>
						</plx:condition>
						<plx:assign>
							<plx:left>
								<plx:nameRef name="name{$modifier}"/>
							</plx:left>
							<plx:right>
								<plx:string data="{@Name}"/>
							</plx:right>
						</plx:assign>
						<xsl:variable name="currentWriteStyle" select="string(@WriteStyle)"/>
						<xsl:if test="$currentWriteStyle and not($currentWriteStyle=$primaryWriteStyle)">
							<plx:assign>
								<plx:left>
									<plx:nameRef name="writeStyle{$modifier}"/>
								</plx:left>
								<plx:right>
									<plx:callStatic name="{$currentWriteStyle}" dataTypeName="CustomSerializedElementWriteStyle" type="field"/>
								</plx:right>
							</plx:assign>
						</xsl:if>
						<xsl:variable name="currentDoubleTagName" select="string(@DoubleTagName)"/>
						<xsl:if test="$currentDoubleTagName and not($currentDoubleTagName=$primaryDoubleTagName)">
							<plx:assign>
								<plx:left>
									<plx:nameRef name="doubleTagName{$modifier}"/>
								</plx:left>
								<plx:right>
									<plx:string data="{$currentDoubleTagName}"/>
								</plx:right>
							</plx:assign>
						</xsl:if>
					</xsl:element>
				</xsl:variable>
				<xsl:choose>
					<xsl:when test="$preConditionStatements and position()!=1">
						<plx:fallbackBranch>
							<xsl:copy-of select="$conditionFragment"/>
						</plx:fallbackBranch>
					</xsl:when>
					<xsl:otherwise>
						<xsl:copy-of select="$conditionFragment"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:for-each>
		</xsl:if>
	</xsl:template>
	<xsl:template name="PassCustomSerializedElementInfoParams">
		<xsl:param name="namespaces"/>
		<xsl:param name="defaultNamespace"/>
		<xsl:param name="modifier"/>
		<xsl:variable name="conditionalNames" select="se:ConditionalName"/>
		<plx:passParam>
			<xsl:choose>
				<xsl:when test="string-length(@Prefix)">
					<plx:string data="{@Prefix}"/>
				</xsl:when>
				<xsl:otherwise>
					<plx:nullKeyword/>
				</xsl:otherwise>
			</xsl:choose>
		</plx:passParam>
		<plx:passParam>
			<xsl:choose>
				<xsl:when test="$conditionalNames">
					<plx:nameRef name="name{$modifier}"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:choose>
						<xsl:when test="string-length(@Name)">
							<plx:string data="{@Name}"/>
						</xsl:when>
						<xsl:when test="self::se:StandaloneLink[not(@PrimaryLinkElement='true' or @PrimaryLinkElement='1')]">
							<plx:string data="{@Class}"/>
						</xsl:when>
						<xsl:otherwise>
							<plx:nullKeyword/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:otherwise>
			</xsl:choose>
		</plx:passParam>
		<plx:passParam>
			<xsl:choose>
				<xsl:when test="string-length(@Namespace)">
					<plx:string data="{@Namespace}"/>
				</xsl:when>
				<xsl:otherwise>
					<plx:nullKeyword/>
				</xsl:otherwise>
			</xsl:choose>
		</plx:passParam>
		<plx:passParam>
			<xsl:variable name="primaryWriteStyle" select="string(@WriteStyle)"/>
			<xsl:choose>
				<xsl:when test="$conditionalNames/@WriteStyle[not(.=$primaryWriteStyle)]">
					<plx:nameRef name="writeStyle{$modifier}"/>
				</xsl:when>
				<xsl:otherwise>
					<plx:callStatic name="pending" type="field" dataTypeName="CustomSerializedElementWriteStyle">
						<xsl:attribute name="name">
							<xsl:choose>
								<xsl:when test="self::se:StandaloneLink">
									<xsl:choose>
										<xsl:when test="@PrimaryLinkElement='true' or @PrimaryLinkElement='1'">
											<xsl:text>PrimaryStandaloneLinkElement</xsl:text>
										</xsl:when>
										<xsl:otherwise>
											<xsl:text>StandaloneLinkElement</xsl:text>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:when test="string-length(@DoubleTagName)">
									<xsl:text>DoubleTaggedElement</xsl:text>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="$primaryWriteStyle">
											<xsl:value-of select="$primaryWriteStyle"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:text>Element</xsl:text>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
					</plx:callStatic>
				</xsl:otherwise>
			</xsl:choose>
		</plx:passParam>
		<plx:passParam>
			<xsl:variable name="primaryDoubleTagName" select="string(@DoubleTagName)"/>
			<xsl:choose>
				<xsl:when test="$conditionalNames/@DoubleTagName[not(.=$primaryDoubleTagName)]">
					<plx:nameRef name="doubleTagName{$modifier}"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:choose>
						<xsl:when test="$primaryDoubleTagName">
							<plx:string data="{$primaryDoubleTagName}"/>
						</xsl:when>
						<xsl:otherwise>
							<plx:nullKeyword/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:otherwise>
			</xsl:choose>
		</plx:passParam>
		<xsl:if test="self::se:StandaloneLink">
			<plx:passParam>
				<xsl:call-template name="CreateStandaloneRelationship">
					<xsl:with-param name="namespaces" select="$namespaces"/>
					<xsl:with-param name="defaultNamespace" select="$defaultNamespace"/>
				</xsl:call-template>
			</plx:passParam>
		</xsl:if>
	</xsl:template>
	<xsl:template name="ReturnCustomSerializedElementInfo">
		<xsl:param name="namespaces"/>
		<xsl:param name="defaultNamespace"/>
		<xsl:call-template name="CreateCustomSerializedElementInfoNameVariable"/>
		<plx:return>
			<plx:callNew dataTypeName="CustomSerializedElementInfo">
				<xsl:if test="self::se:StandaloneLink">
					<xsl:attribute name="dataTypeName">
						<xsl:text>CustomSerializedStandaloneLinkElementInfo</xsl:text>
					</xsl:attribute>
				</xsl:if>
				<xsl:call-template name="PassCustomSerializedElementInfoParams">
					<xsl:with-param name="namespaces" select="$namespaces"/>
					<xsl:with-param name="defaultNamespace" select="$defaultNamespace"/>
				</xsl:call-template>
			</plx:callNew>
		</plx:return>
	</xsl:template>
	<xsl:template name="ReturnCustomSerializedPropertyInfo">
		<xsl:for-each select="se:Condition">
			<xsl:copy-of select="child::*[position()!=last()]"/>
			<plx:branch>
				<plx:condition>
					<xsl:copy-of select="child::*[last()]"/>
				</plx:condition>
				<xsl:call-template name="ReturnCustomSerializedPropertyInfo"/>
			</plx:branch>
		</xsl:for-each>
		<plx:return>
			<plx:callNew dataTypeName="CustomSerializedPropertyInfo">
				<plx:passParam>
					<xsl:choose>
						<xsl:when test="string-length(@Prefix)">
							<plx:string data="{@Prefix}"/>
						</xsl:when>
						<xsl:otherwise>
							<plx:nullKeyword/>
						</xsl:otherwise>
					</xsl:choose>
				</plx:passParam>
				<plx:passParam>
					<xsl:choose>
						<xsl:when test="string-length(@Name) and not(@WriteStyle='NotWritten' or @ReadOnly='true' or @ReadOnly='1')">
							<plx:string data="{@Name}"/>
						</xsl:when>
						<xsl:otherwise>
							<plx:nullKeyword/>
						</xsl:otherwise>
					</xsl:choose>
				</plx:passParam>
				<plx:passParam>
					<xsl:choose>
						<xsl:when test="string-length(@Namespace)">
							<plx:string data="{@Namespace}"/>
						</xsl:when>
						<xsl:otherwise>
							<plx:nullKeyword/>
						</xsl:otherwise>
					</xsl:choose>
				</plx:passParam>
				<plx:passParam>
					<xsl:choose>
						<xsl:when test="@WriteCustomStorage='true'">
							<plx:trueKeyword/>
						</xsl:when>
						<xsl:otherwise>
							<plx:falseKeyword/>
						</xsl:otherwise>
					</xsl:choose>
				</plx:passParam>
				<plx:passParam>
					<plx:callStatic name="pending" type="field" dataTypeName="CustomSerializedAttributeWriteStyle">
						<xsl:attribute name="name">
							<xsl:choose>
								<xsl:when test="@ReadOnly='true' or @ReadOnly='1'">
									<xsl:text>NotWritten</xsl:text>
								</xsl:when>
								<xsl:when test="string-length(@DoubleTagName)">
									<xsl:text>DoubleTaggedElement</xsl:text>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="string-length(@WriteStyle)">
											<xsl:value-of select="@WriteStyle"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:text>Attribute</xsl:text>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
					</plx:callStatic>
				</plx:passParam>
				<plx:passParam>
					<xsl:choose>
						<xsl:when test="string-length(@DoubleTagName)">
							<plx:string data="{@DoubleTagName}"/>
						</xsl:when>
						<xsl:otherwise>
							<plx:nullKeyword/>
						</xsl:otherwise>
					</xsl:choose>
				</plx:passParam>
			</plx:callNew>
		</plx:return>
	</xsl:template>
	<xsl:template name="CreateStandaloneRelationship">
		<xsl:param name="namespaces"/>
		<xsl:param name="defaultNamespace"/>
		<plx:callNew dataTypeName="CustomSerializedStandaloneRelationship">
			<plx:passParam>
				<plx:callStatic dataTypeName="{@Class}" name="DomainClassId" type="field"/>
			</plx:passParam>
			<plx:passParam>
				<plx:callNew dataTypeName="CustomSerializedStandaloneRelationshipRole" dataTypeIsSimpleArray="true">
					<plx:arrayInitializer>
						<xsl:for-each select="se:Role">
							<plx:callNew dataTypeName="CustomSerializedStandaloneRelationshipRole">
								<plx:passParam>
									<plx:string data="{@RoleName}">
										<xsl:if test="string(@Name)">
											<xsl:attribute name="data">
												<xsl:value-of select="@Name"/>
											</xsl:attribute>
										</xsl:if>
									</plx:string>
								</plx:passParam>
								<plx:passParam>
									<plx:callStatic dataTypeName="{../@Class}" name="{@RoleName}DomainRoleId" type="field"/>
								</plx:passParam>
							</plx:callNew>
						</xsl:for-each>
					</plx:arrayInitializer>
				</plx:callNew>
			</plx:passParam>
			<xsl:choose>
				<xsl:when test="@PrimaryLinkElement='true' or @PrimaryLinkElement='1'">
					<plx:passParam>
						<plx:nullKeyword/>
					</plx:passParam>
					<plx:passParam>
						<plx:nullKeyword/>
					</plx:passParam>
					<plx:passParam>
						<plx:nullKeyword/>
					</plx:passParam>
				</xsl:when>
				<xsl:otherwise>
					<plx:passParam>
						<plx:string data="{$defaultNamespace/@Prefix}">
							<xsl:if test="string(@Prefix)">
								<xsl:attribute name="data">
									<xsl:value-of select="@Prefix"/>
								</xsl:attribute>
							</xsl:if>
						</plx:string>
					</plx:passParam>
					<plx:passParam>
						<xsl:choose>
							<xsl:when test="string(@Name)">
								<plx:string data="{@Name}"/>
							</xsl:when>
							<xsl:otherwise>
								<plx:string data="{@Class}"/>
							</xsl:otherwise>
						</xsl:choose>
					</plx:passParam>
					<plx:passParam>
						<plx:string data="{$defaultNamespace/@URI}">
							<xsl:if test="string(@Prefix)">
								<xsl:attribute name="data">
									<xsl:value-of select="$namespaces[@Prefix=current()/@Prefix]/@URI"/>
								</xsl:attribute>
							</xsl:if>
						</plx:string>
					</plx:passParam>
				</xsl:otherwise>
			</xsl:choose>
		</plx:callNew>
	</xsl:template>
</xsl:stylesheet>
