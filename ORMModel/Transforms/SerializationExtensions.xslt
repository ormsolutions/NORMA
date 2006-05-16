<?xml version="1.0" encoding="utf-8"?>
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
			<plx:namespaceImport name="Neumont.Tools.ORM.Shell"/>
			<plx:namespaceImport name="Neumont.Tools.ORM.ObjectModel"/>
			<plx:namespace name="{$CustomToolNamespace}">
				<plx:leadingInfo>
					<plx:comment>Common Public License Copyright Notice</plx:comment>
					<plx:comment>/**************************************************************************\</plx:comment>
					<plx:comment>* Neumont Object-Role Modeling Architect for Visual Studio                 *</plx:comment>
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
				<xsl:apply-templates select="child::*"/>
			</plx:namespace>
		</plx:root>
	</xsl:template>
	<xsl:template match="se:Copyright"/>
	<xsl:template match="se:Element">
		<xsl:variable name="ClassName" select="@Class"/>
		<xsl:variable name="ClassOverride" select="@Override='true'"/>
		<plx:class name="{$ClassName}" visibility="public" partial="true">
			<plx:leadingInfo>
				<plx:pragma type="region" data="{$ClassName} serialization"/>
			</plx:leadingInfo>
			<plx:trailingInfo>
				<plx:pragma type="closeRegion" data="{$ClassName} serialization"/>
			</plx:trailingInfo>
			<plx:implementsInterface dataTypeName="IORMCustomSerializedElement"/>
			<plx:property visibility="protected" name="SupportedCustomSerializedOperations" replacesName="{$ClassOverride}">
				<plx:leadingInfo>
					<plx:docComment>
						<summary>Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations</summary>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:interfaceMember dataTypeName="IORMCustomSerializedElement" memberName="SupportedCustomSerializedOperations"/>
				<plx:returns dataTypeName="ORMCustomSerializedElementSupportedOperations"/>
				<plx:get>
					<xsl:variable name="currentSupport">
						<xsl:call-template name="ReturnORMCustomSerializedElementSupportedOperations">
							<xsl:with-param name="childElements" select="count(se:ChildElement)"/>
							<xsl:with-param name="element" select="count(@Prefix)+count(@Name)+count(@Namespace)+count(@WriteStyle)+count(@DoubleTagName)+count(se:ConditionalName)"/>
							<xsl:with-param name="attributes" select="count(se:Attribute)"/>
							<xsl:with-param name="links" select="count(se:Link)"/>
							<xsl:with-param name="customSort" select="@SortChildElements='true'"/>
							<xsl:with-param name="mixedTypedAttributes" select="@HasMixedTypedAttributes='true'"/>
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
			<xsl:variable name="childElementCount" select="count(se:ChildElement)"/>
			<xsl:variable name="haveCustomChildInfo" select="0!=$childElementCount"/>
			<xsl:if test="$haveCustomChildInfo">
				<plx:field visibility="private" static="true" dataTypeName="ORMCustomSerializedChildElementInfo" dataTypeIsSimpleArray="true" name="myCustomSerializedChildElementInfo"/>
			</xsl:if>
			<xsl:if test="$haveCustomChildInfo or not($ClassOverride)">
				<plx:function visibility="protected" name="GetCustomSerializedChildElementInfo" replacesName="{$ClassOverride}">
					<plx:leadingInfo>
						<plx:docComment>
							<summary>Implements IORMCustomSerializedElement.GetCustomSerializedChildElementInfo</summary>
						</plx:docComment>
					</plx:leadingInfo>
					<plx:interfaceMember dataTypeName="IORMCustomSerializedElement" memberName="GetCustomSerializedChildElementInfo"/>
					<plx:returns dataTypeName="ORMCustomSerializedChildElementInfo" dataTypeIsSimpleArray="true"/>
					<xsl:choose>
						<xsl:when test="$haveCustomChildInfo">
							<plx:local dataTypeName="ORMCustomSerializedChildElementInfo" dataTypeIsSimpleArray="true" name="ret">
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
									<plx:local name="baseInfo" dataTypeName="ORMCustomSerializedChildElementInfo" dataTypeIsSimpleArray="true">
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
															<plx:callStatic name="ChildElementInfo" dataTypeName="ORMCustomSerializedElementSupportedOperations" type="field"/>
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
										<plx:callNew dataTypeName="ORMCustomSerializedChildElementInfo" dataTypeIsSimpleArray="true">
											<plx:passParam>
												<xsl:choose>
													<xsl:when test="$ClassOverride">
														<plx:binaryOperator type="add">
															<plx:left>
																<plx:nameRef name="baseInfoCount"/>
															</plx:left>
															<plx:right>
																<plx:value type="i4" data="{$childElementCount}"/>
															</plx:right>
														</plx:binaryOperator>
													</xsl:when>
													<xsl:otherwise>
														<plx:value type="i4" data="{$childElementCount}"/>
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
												<plx:value type="i4" data="{$childElementCount}"/>
											</plx:passParam>
										</plx:callInstance>
									</plx:branch>
								</xsl:if>
								<xsl:for-each select="se:ChildElement">
									<xsl:variable name="index" select="position()-1"/>
									<xsl:call-template name="CreateORMCustomSerializedElementInfoNameVariable">
										<xsl:with-param name="modifier" select="$index"/>
									</xsl:call-template>
									<plx:assign>
										<plx:left>
											<plx:callInstance name=".implied" type="arrayIndexer">
												<plx:callObject>
													<plx:nameRef name="ret"/>
												</plx:callObject>
												<plx:passParam>
													<plx:value type="i4" data="{$index}"/>
												</plx:passParam>
											</plx:callInstance>
										</plx:left>
										<plx:right>
											<plx:callNew dataTypeName="ORMCustomSerializedChildElementInfo">
												<xsl:call-template name="PassORMCustomSerializedElementInfoParams">
													<xsl:with-param name="modifier" select="$index"/>
												</xsl:call-template>
												<xsl:for-each select="se:Link">
													<plx:passParam>
														<plx:callStatic name="{@RoleName}MetaRoleGuid" dataTypeName="{@RelationshipName}" type="field"/>
													</plx:passParam>
												</xsl:for-each>
											</plx:callNew>
										</plx:right>
									</plx:assign>
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
				<plx:property visibility="protected" name="CustomSerializedElementInfo" replacesName="{$ClassOverride}">
					<plx:leadingInfo>
						<plx:docComment>
							<summary>Implements IORMCustomSerializedElement.CustomSerializedElementInfo</summary>
						</plx:docComment>
					</plx:leadingInfo>
					<plx:interfaceMember dataTypeName="IORMCustomSerializedElement" memberName="CustomSerializedElementInfo"/>
					<plx:returns dataTypeName="ORMCustomSerializedElementInfo"/>
					<plx:get>
						<xsl:choose>
							<xsl:when test="$haveCustomElementInfo">
								<xsl:call-template name="ReturnORMCustomSerializedElementInfo"/>
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
			<xsl:variable name="haveCustomAttributeInfo" select="0!=count(se:Attribute)"/>
			<xsl:if test="$haveCustomAttributeInfo or not($ClassOverride)">
				<plx:function visibility="protected" name="GetCustomSerializedAttributeInfo" replacesName="{$ClassOverride}">
					<plx:leadingInfo>
						<plx:docComment>
							<summary>Implements IORMCustomSerializedElement.GetCustomSerializedAttributeInfo</summary>
						</plx:docComment>
					</plx:leadingInfo>
					<plx:interfaceMember dataTypeName="IORMCustomSerializedElement" memberName="GetCustomSerializedAttributeInfo"/>
					<plx:param name="attributeInfo" dataTypeName="MetaAttributeInfo"></plx:param>
					<plx:param name="rolePlayedInfo" dataTypeName="MetaRoleInfo"></plx:param>
					<plx:returns dataTypeName="ORMCustomSerializedAttributeInfo"/>
					<xsl:choose>
						<xsl:when test="count(se:Attribute)">
							<xsl:for-each select="se:Attribute">
								<plx:branch>
									<plx:condition>
										<plx:binaryOperator type="equality">
											<plx:left>
												<plx:callInstance type="property" name="Id">
													<plx:callObject>
														<plx:nameRef type="parameter" name="attributeInfo"/>
													</plx:callObject>
												</plx:callInstance>
											</plx:left>
											<plx:right>
												<plx:callStatic type="field" name="{@ID}MetaAttributeGuid" dataTypeName="{$ClassName}" />
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
																<plx:nameRef type="parameter" name="rolePlayedInfo"/>
															</plx:callObject>
														</plx:callInstance>
													</plx:left>
													<plx:right>
														<plx:callStatic type="field" name="{@ID}MetaRoleGuid" dataTypeName="{$ClassName}" />
													</plx:right>
												</plx:binaryOperator>
											</plx:condition>
											<xsl:call-template name="ReturnORMCustomSerializedAttributeInfo"/>
										</plx:branch>
									</xsl:for-each>
									<xsl:call-template name="ReturnORMCustomSerializedAttributeInfo"/>
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
														<plx:callStatic name="AttributeInfo" dataTypeName="ORMCustomSerializedElementSupportedOperations" type="field"/>
													</plx:left>
													<plx:right>
														<plx:callThis accessor="base" name="SupportedCustomSerializedOperations" type="property"/>
													</plx:right>
												</plx:binaryOperator>
											</plx:right>
										</plx:binaryOperator>
									</plx:condition>
									<plx:return>
										<plx:callThis accessor="base" name="GetCustomSerializedAttributeInfo">
											<plx:passParam>
												<plx:nameRef type="parameter" name="attributeInfo"/>
											</plx:passParam>
											<plx:passParam>
												<plx:nameRef type="parameter" name="rolePlayedInfo"/>
											</plx:passParam>
										</plx:callThis>
									</plx:return>
								</plx:branch>
							</xsl:if>
							<plx:return>
								<plx:callStatic dataTypeName="ORMCustomSerializedAttributeInfo" name="Default" type="field"/>
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
			<xsl:variable name="haveCustomLinkInfo" select="0!=count(se:Link)"/>
			<xsl:if test="$haveCustomLinkInfo or not($ClassOverride)">
				<plx:function visibility="protected" name="GetCustomSerializedLinkInfo" replacesName="{$ClassOverride}">
					<plx:leadingInfo>
						<plx:docComment>
							<summary>Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo</summary>
						</plx:docComment>
					</plx:leadingInfo>
					<plx:interfaceMember dataTypeName="IORMCustomSerializedElement" memberName="GetCustomSerializedLinkInfo"/>
					<plx:param name="rolePlayedInfo" dataTypeName="MetaRoleInfo"/>
					<plx:param name="elementLink" dataTypeName="ElementLink"/>
					<plx:returns dataTypeName="ORMCustomSerializedElementInfo"/>
					<xsl:choose>
						<xsl:when test="$haveCustomLinkInfo">
							<xsl:for-each select="se:Link">
								<plx:branch>
									<plx:condition>
										<plx:binaryOperator type="equality">
											<plx:left>
												<plx:callInstance type="property" name="Id">
													<plx:callObject>
														<plx:nameRef type="parameter" name="rolePlayedInfo"/>
													</plx:callObject>
												</plx:callInstance>
											</plx:left>
											<plx:right>
												<plx:callStatic type="field" name="{@RoleName}MetaRoleGuid" dataTypeName="{@RelationshipName}" />
											</plx:right>
										</plx:binaryOperator>
									</plx:condition>
									<xsl:call-template name="ReturnORMCustomSerializedElementInfo"/>
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
														<plx:callStatic name="LinkInfo" dataTypeName="ORMCustomSerializedElementSupportedOperations" type="field"/>
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
												<plx:nameRef type="parameter" name="rolePlayedInfo"/>
											</plx:passParam>
											<plx:passParam>
												<plx:nameRef type="parameter" name="elementLink"/>
											</plx:passParam>
										</plx:callThis>
									</plx:return>
								</plx:branch>
							</xsl:if>
							<plx:return>
								<plx:callStatic dataTypeName="ORMCustomSerializedElementInfo" name="Default" type="field"/>
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
			<xsl:choose>
				<xsl:when test="@SortChildElements='true'">
					<plx:field name="myCustomSortChildComparer" static="true" visibility="private" dataTypeName="IComparer">
						<plx:passTypeParam dataTypeName="MetaRoleInfo"/>
					</plx:field>
					<plx:class name="CustomSortChildComparer" visibility="private">
						<plx:implementsInterface dataTypeName="IComparer">
							<plx:passTypeParam dataTypeName="MetaRoleInfo"/>
						</plx:implementsInterface>
						<plx:field name="myRoleOrderDictionary" visibility="private" dataTypeName="Dictionary">
							<plx:passTypeParam dataTypeName=".string"/>
							<plx:passTypeParam dataTypeName=".i4"/>
						</plx:field>
						<xsl:if test="$ClassOverride">
							<plx:field name="myBaseComparer" visibility="private" dataTypeName="IComparer">
								<plx:passTypeParam dataTypeName="MetaRoleInfo"/>
							</plx:field>
						</xsl:if>
						<plx:function name=".construct" visibility="public">
							<plx:param name="store" dataTypeName="Store"/>
							<xsl:if test="$ClassOverride">
								<plx:param name="baseComparer" dataTypeName="IComparer">
									<plx:passTypeParam dataTypeName="MetaRoleInfo"/>
								</plx:param>
								<plx:assign>
									<plx:left>
										<plx:callThis name="myBaseComparer" type="field"/>
									</plx:left>
									<plx:right>
										<plx:nameRef type="parameter" name="baseComparer"/>
									</plx:right>
								</plx:assign>
							</xsl:if>
							<xsl:variable name="SortedLevelsFragment">
								<!-- ChildElement/Link links may have more information in Link. Just use
								     the ChildElement one. -->
								<xsl:variable name="childLinks" select="se:ChildElement[not(@NotSorted='true')]/se:Link"/>
								<!-- Define a variable with structure <SortLevel><Role/><SortLevel/> -->
								<xsl:for-each select="se:Link | se:ChildElement">
									<xsl:if test="not(@NotSorted='true')">
										<xsl:choose>
											<xsl:when test="local-name()='Link'">
												<xsl:variable name="relName" select="@RelationshipName"/>
												<xsl:variable name="roleName" select="@RoleName"/>
												<xsl:if test="0=count($childLinks[@RelationshipName=$relName and @RoleName=$roleName])">
													<SortLevel>
														<Role RelationshipName="{@RelationshipName}" RoleName="{@RoleName}"/>
													</SortLevel>
												</xsl:if>
											</xsl:when>
											<xsl:when test="local-name()='ChildElement'">
												<xsl:choose>
													<xsl:when test="@SortChildElements='true'">
														<!-- Add one sort level for each child -->
														<xsl:for-each select="se:Link">
															<SortLevel>
																<Role RelationshipName="{@RelationshipName}" RoleName="{@RoleName}"/>
															</SortLevel>
														</xsl:for-each>
													</xsl:when>
													<xsl:otherwise>
														<!-- Add one sort level for all children -->
														<SortLevel>
															<xsl:for-each select="se:Link">
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
							<plx:local name="metaDataDir" dataTypeName="MetaDataDirectory">
								<plx:initialize>
									<plx:callInstance name="MetaDataDirectory" type="property">
										<plx:callObject>
											<plx:nameRef type="parameter" name="store"/>
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
							<plx:local name="metaRole" dataTypeName="MetaRoleInfo"/>
							<xsl:for-each select="$SortedLevels/SortLevel">
								<xsl:variable name="level" select="position()-1"/>
								<xsl:for-each select="Role">
									<plx:assign>
										<plx:left>
											<plx:nameRef name="metaRole"/>
										</plx:left>
										<plx:right>
											<plx:callInstance name="FindMetaRole">
												<plx:callObject>
													<plx:nameRef name="metaDataDir"/>
												</plx:callObject>
												<plx:passParam>
													<plx:callStatic dataTypeName="{@RelationshipName}" name="{@RoleName}MetaRoleGuid" type="field"/>
												</plx:passParam>
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
													<plx:callInstance name="FullName" type="property">
														<plx:callObject>
															<plx:callInstance name="OppositeMetaRole" type="property">
																<plx:callObject>
																	<plx:nameRef name="metaRole"/>
																</plx:callObject>
															</plx:callInstance>
														</plx:callObject>
													</plx:callInstance>
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
								<plx:passTypeParam dataTypeName="MetaRoleInfo"/>
							</plx:interfaceMember>
							<plx:param name="x" dataTypeName="MetaRoleInfo"/>
							<plx:param name="y" dataTypeName="MetaRoleInfo"/>
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
													<plx:nameRef type="parameter" name="x"/>
												</plx:passParam>
												<plx:passParam>
													<plx:nameRef type="parameter" name="y"/>
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
													<plx:callInstance name="FullName" type="property">
														<plx:callObject>
															<plx:nameRef type="parameter" name="{.}"/>
														</plx:callObject>
													</plx:callInstance>
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
											<plx:callStatic dataTypeName=".i4" name="MaxValue" type="field"/>
										</plx:right>
									</plx:assign>
								</plx:branch>
							</xsl:for-each>
							<plx:branch>
								<plx:condition>
									<plx:binaryOperator type="equality">
										<plx:left>
											<plx:nameRef name="xPos"/>
										</plx:left>
										<plx:right>
											<plx:nameRef name="yPos"/>
										</plx:right>
									</plx:binaryOperator>
								</plx:condition>
								<plx:return>
									<plx:value type="i4" data="0"/>
								</plx:return>
							</plx:branch>
							<plx:alternateBranch>
								<plx:condition>
									<plx:binaryOperator type="lessThan">
										<plx:left>
											<plx:nameRef name="xPos"/>
										</plx:left>
										<plx:right>
											<plx:nameRef name="yPos"/>
										</plx:right>
									</plx:binaryOperator>
								</plx:condition>
								<plx:return>
									<plx:value type="i4" data="-1"/>
								</plx:return>
							</plx:alternateBranch>
							<plx:return>
								<plx:value type="i4" data="1"/>
							</plx:return>
						</plx:function>
					</plx:class>
					<plx:property visibility="protected" name="CustomSerializedChildRoleComparer" replacesName="{$ClassOverride}">
						<plx:leadingInfo>
							<plx:docComment>
								<summary>Implements IORMCustomSerializedElement.CustomSerializedChildRoleComparer</summary>
							</plx:docComment>
						</plx:leadingInfo>
						<plx:interfaceMember dataTypeName="IORMCustomSerializedElement" memberName="CustomSerializedChildRoleComparer"/>
						<plx:returns dataTypeName="IComparer">
							<plx:passTypeParam dataTypeName="MetaRoleInfo"/>
						</plx:returns>
						<plx:get>
							<plx:local name="retVal" dataTypeName="IComparer">
								<plx:passTypeParam dataTypeName="MetaRoleInfo"/>
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
										<plx:passTypeParam dataTypeName="MetaRoleInfo"/>
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
															<plx:callStatic name="CustomSortChildRoles" dataTypeName="ORMCustomSerializedElementSupportedOperations" type="field"/>
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
					<plx:property visibility="protected" name="CustomSerializedChildRoleComparer">
						<plx:leadingInfo>
							<plx:docComment>
								<summary>Implements IORMCustomSerializedElement.CustomSerializedChildRoleComparer</summary>
							</plx:docComment>
						</plx:leadingInfo>
						<plx:interfaceMember dataTypeName="IORMCustomSerializedElement" memberName="CustomSerializedChildRoleComparer"/>
						<plx:returns dataTypeName="IComparer">
							<plx:passTypeParam dataTypeName="MetaRoleInfo"/>
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
				<xsl:variable name="namespaces" select="../se:MetaModel/se:Namespaces/se:Namespace"/>
				<xsl:variable name="namespace">
					<xsl:call-template name="ResolveNamespace">
						<xsl:with-param name="namespaces" select="$namespaces"/>
						<!-- Use default for prefix parameter -->
					</xsl:call-template>
				</xsl:variable>
				<xsl:variable name="linksInChildElementFragment">
					<xsl:for-each select="se:ChildElement/se:Link">
						<xsl:copy>
							<xsl:copy-of select="@*"/>
							<xsl:for-each select="parent::se:ChildElement">
								<xsl:attribute name="ContainerName">
									<xsl:value-of select="@Name"/>
								</xsl:attribute>
								<xsl:attribute name="ContainerPrefix">
									<xsl:value-of select="@Prefix"/>
								</xsl:attribute>
							</xsl:for-each>
						</xsl:copy>
					</xsl:for-each>
				</xsl:variable>
				<xsl:variable name="linksInChildElement" select="exsl:node-set($linksInChildElementFragment)/child::*"/>
				<xsl:variable name="childElements" select="se:ChildElement"/>
				<xsl:variable name="allLinksTemp">
					<xsl:for-each select="se:Link[not(@WriteStyle='NotWritten')]">
						<xsl:copy>
							<xsl:copy-of select="@*"/>
							<xsl:if test="$linksInChildElement[@RelationshipName=current()/@RelationshipName and @RoleName=current()/@RoleName and string(@CreateAsRelationshipName)=string(current()/@CreateAsRelationshipName)]">
								<xsl:attribute name="contained">
									<xsl:value-of select="true()"/>
								</xsl:attribute>
							</xsl:if>
						</xsl:copy>
					</xsl:for-each>
				</xsl:variable>
				<xsl:variable name="allLinks" select="exsl:node-set($allLinksTemp)/child::*"/>

				<xsl:for-each select="$allLinks">
					<xsl:choose>
						<!-- Walk the $allLinks, then add the ones that are
						NOT contained in $linksInChildElement -->
						<xsl:when test="not(@contained)">
							<xsl:variable name="createAsRelationshipName" select="string(@CreateAsRelationshipName)"/>
							<plx:callInstance name="InitializeRoles">
								<xsl:if test="$createAsRelationshipName">
									<xsl:attribute name="name">
										<xsl:text>InitializeRolesWithExplicitRelationship</xsl:text>
									</xsl:attribute>
								</xsl:if>
								<plx:callObject>
									<plx:nameRef name="match"/>
								</plx:callObject>
								<xsl:if test="$createAsRelationshipName">
									<plx:passParam>
										<plx:callStatic name="MetaRelationshipGuid" dataTypeName="{$createAsRelationshipName}" type="property"/>
									</plx:passParam>
								</xsl:if>
								<plx:passParam>
									<plx:callStatic name="{@RoleName}MetaRoleGuid" dataTypeName="{@RelationshipName}" type="property"/>
								</plx:passParam>
							</plx:callInstance>
							<plx:callInstance name="Add">
								<plx:callObject>
									<plx:nameRef name="childElementMappings"/>
								</plx:callObject>
								<plx:passParam>
									<plx:string>
										<plx:string>||</plx:string>
										<plx:string>
											<xsl:call-template name="ResolveNamespace">
												<xsl:with-param name="namespaces" select="$namespaces"/>
												<!-- Use default for prefix parameter -->
											</xsl:call-template>
										</plx:string>
										<plx:string>|</plx:string>
										<plx:string>
											<xsl:value-of select="@Name"/>
										</plx:string>
									</plx:string>
								</plx:passParam>
								<plx:passParam>
									<plx:nameRef name="match"/>
								</plx:passParam>
							</plx:callInstance>
						</xsl:when>
					</xsl:choose>
				</xsl:for-each>
				<!-- Walk $linksInChildElements, then add the ones that
				INTERSECT with the $allLinks list to the dictionary. -->
				<xsl:for-each select="$childElements">
					<xsl:variable name="localLinksFragment">
						<xsl:for-each select="se:Link">
							<xsl:variable name="relationshipName" select="@RelationshipName"/>
							<xsl:variable name="roleName" select="@RoleName"/>
							<xsl:variable name="namedLinks" select="$allLinks[@RelationshipName=current()/@RelationshipName and @RoleName=current()/@RoleName and string(@CreateAsRelationshipName)=string(current()/@CreateAsRelationshipName)]"/>
							<xsl:if test="$namedLinks">
								<xsl:copy>
									<xsl:copy-of select="@*"/>
									<xsl:for-each select="$namedLinks[1]">
										<xsl:choose>
											<xsl:when test="string-length(@Name)">
												<xsl:copy-of select="@Name"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:attribute name="Name">
													<xsl:value-of select="@RelationshipName"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="@RoleName"/>
												</xsl:attribute>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:for-each>
								</xsl:copy>
							</xsl:if>
						</xsl:for-each>
					</xsl:variable>
					<xsl:variable name="localLinks" select="exsl:node-set($localLinksFragment)/child::*"/>
					<xsl:if test="count($localLinks)">
						<xsl:variable name="containerName" select="@Name"/>
						<xsl:for-each select="$localLinks">
							<xsl:variable name="createAsRelationshipName" select="string(@CreateAsRelationshipName)"/>
							<plx:callInstance name="InitializeRoles">
								<xsl:if test="$createAsRelationshipName">
									<xsl:attribute name="name">
										<xsl:text>InitializeRolesWithExplicitRelationship</xsl:text>
									</xsl:attribute>
								</xsl:if>
								<plx:callObject>
									<plx:nameRef name="match"/>
								</plx:callObject>
								<xsl:if test="$createAsRelationshipName">
									<plx:passParam>
										<plx:callStatic name="MetaRelationshipGuid" dataTypeName="{$createAsRelationshipName}" type="property"/>
									</plx:passParam>
								</xsl:if>
								<plx:passParam>
									<plx:callStatic name="{@RoleName}MetaRoleGuid" dataTypeName="{@RelationshipName}" type="field"/>
								</plx:passParam>
							</plx:callInstance>
							<plx:callInstance name="Add">
								<plx:callObject>
									<plx:nameRef name="childElementMappings"/>
								</plx:callObject>
								<plx:passParam>
									<plx:string>
										<plx:string>
											<xsl:call-template name="ResolveNamespace">
												<xsl:with-param name="namespaces" select="$namespaces"/>
												<!-- Use default for prefix parameter -->
											</xsl:call-template>
										</plx:string>
										<plx:string>|</plx:string>
										<plx:string>
											<plx:string>
												<xsl:value-of select="$containerName"/>
											</plx:string>
										</plx:string>
										<plx:string>|</plx:string>
										<plx:string>
											<xsl:call-template name="ResolveNamespace">
												<xsl:with-param name="namespaces" select="$namespaces"/>
												<!-- Use default for prefix parameter -->
											</xsl:call-template>
										</plx:string>
										<plx:string>|</plx:string>
										<plx:string>
											<xsl:value-of select="@Name"/>
										</plx:string>
									</plx:string>
								</plx:passParam>
								<plx:passParam>
									<plx:nameRef name="match"/>
								</plx:passParam>
							</plx:callInstance>
						</xsl:for-each>
					</xsl:if>
				</xsl:for-each>

				<!-- Walk $linksInChildElements, then add the ones that are
				NOT in the $allLinks list to the dictionary. -->
				<xsl:for-each select="$childElements">
					<xsl:variable name="linksFragment">
						<xsl:for-each select="se:Link">
							<xsl:variable name="relationshipName" select="string(@RelationshipName)"/>
							<xsl:variable name="roleName" select="string(@RoleName)"/>
							<xsl:variable name="createAsRelationshipName" select="string(@CreateAsRelationshipName)"/>
							<xsl:if test="not($allLinks[@RelationshipName=$relationshipName and @RoleName=$roleName and string(@CreateAsRelationshipName)=$createAsRelationshipName])">
								<xsl:if test="$createAsRelationshipName">
									<explicitCreateMarker>
										<plx:passParam>
											<plx:callStatic name="MetaRelationshipGuid" dataTypeName="{$createAsRelationshipName}" type="property"/>
										</plx:passParam>
									</explicitCreateMarker>
								</xsl:if>
								<plx:passParam>
									<plx:callStatic name="{$roleName}MetaRoleGuid" dataTypeName="{$relationshipName}" type="property"/>
								</plx:passParam>
							</xsl:if>
						</xsl:for-each>
					</xsl:variable>
					<xsl:variable name="links" select="exsl:node-set($linksFragment)/child::*"/>
					<xsl:if test="$links">
						<xsl:variable name="explicitCreateParam" select="$links/self::explicitCreateMarker/plx:passParam"/>
						<xsl:choose>
							<xsl:when test="$explicitCreateParam">
								<plx:callInstance name="InitializeRolesWithExplicitRelationship">
									<plx:callObject>
										<plx:nameRef name="match"/>
									</plx:callObject>
									<xsl:copy-of select="$explicitCreateParam"/>
									<xsl:copy-of select="$links/self::plx:passParam"/>
								</plx:callInstance>
							</xsl:when>
							<xsl:otherwise>
								<plx:callInstance name="InitializeRoles">
									<plx:callObject>
										<plx:nameRef name="match"/>
									</plx:callObject>
									<xsl:copy-of select="$links/self::plx:passParam"/>
								</plx:callInstance>
							</xsl:otherwise>
						</xsl:choose>
						<plx:callInstance name="Add">
							<plx:callObject>
								<plx:nameRef name="childElementMappings"/>
							</plx:callObject>
							<plx:passParam>
								<plx:string>
									<plx:string>
										<xsl:call-template name="ResolveNamespace">
											<xsl:with-param name="namespaces" select="$namespaces"/>
											<!-- Use default for prefix parameter -->
										</xsl:call-template>
									</plx:string>
									<plx:string>|</plx:string>
									<plx:string>
										<plx:string>
											<xsl:value-of select="@Name"/>
										</plx:string>
									</plx:string>
									<plx:string>||</plx:string>
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
							<plx:callStatic name="{@ID}MetaAttributeGuid" dataTypeName="{$ClassName}" type="field"/>
						</plx:passParam>
						<plx:passParam>
							<xsl:choose>
								<xsl:when test="string-length(@DoubleTagName)">
									<plx:string>
										<xsl:value-of select="@DoubleTagName"/>
									</plx:string>
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
								<plx:string>||</plx:string>
								<plx:string>
									<xsl:call-template name="ResolveNamespace">
										<xsl:with-param name="namespaces" select="$namespaces"/>
										<!-- Use default for prefix parameter -->
									</xsl:call-template>
								</plx:string>
								<plx:string>|</plx:string>
								<plx:string>
									<xsl:choose>
										<xsl:when test="string-length(@Name)">
											<xsl:value-of select="@Name"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="@ID"/>
										</xsl:otherwise>
									</xsl:choose>
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
					<plx:passTypeParam dataTypeName="ORMCustomSerializedElementMatch"/>
				</plx:field>
			</xsl:if>
			<xsl:if test="$hasMappedChildElements or not($ClassOverride)">
				<plx:function visibility="protected" name="MapChildElement" replacesName="{$ClassOverride}">
					<plx:leadingInfo>
						<plx:docComment>
							<summary>Implements IORMCustomSerializedElement.MapChildElement</summary>
						</plx:docComment>
					</plx:leadingInfo>
					<plx:interfaceMember dataTypeName="IORMCustomSerializedElement" memberName="MapChildElement"/>
					<plx:param dataTypeName=".string" name="elementNamespace"/>
					<plx:param dataTypeName=".string" name="elementName"/>
					<plx:param dataTypeName=".string" name="containerNamespace"/>
					<plx:param dataTypeName=".string" name="containerName"/>
					<plx:returns dataTypeName="ORMCustomSerializedElementMatch"/>
					<xsl:variable name="forwardToBase">
						<xsl:if test="$ClassOverride">
							<plx:callThis accessor="base" name="MapChildElement">
								<plx:passParam>
									<plx:nameRef type="parameter" name="elementNamespace"/>
								</plx:passParam>
								<plx:passParam>
									<plx:nameRef type="parameter" name="elementName"/>
								</plx:passParam>
								<plx:passParam>
									<plx:nameRef type="parameter" name="containerNamespace"/>
								</plx:passParam>
								<plx:passParam>
									<plx:nameRef type="parameter" name="containerName"/>
								</plx:passParam>
							</plx:callThis>
						</xsl:if>
					</xsl:variable>
					<xsl:choose>
						<xsl:when test="$hasMappedChildElements">
							<plx:local name="childElementMappings" dataTypeName="Dictionary">
								<plx:passTypeParam dataTypeName=".string"/>
								<plx:passTypeParam dataTypeName="ORMCustomSerializedElementMatch"/>
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
											<plx:passTypeParam dataTypeName="ORMCustomSerializedElementMatch"/>
										</plx:callNew>
									</plx:right>
								</plx:assign>
								<plx:local name="match" dataTypeName="ORMCustomSerializedElementMatch">
									<plx:initialize>
										<plx:callNew dataTypeName="ORMCustomSerializedElementMatch"/>
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
							<xsl:if test="se:Link | se:Attribute[@WriteStyle='Element']">
							</xsl:if>
							<plx:local name="rVal" dataTypeName="ORMCustomSerializedElementMatch"/>
							<xsl:variable name="lookupCall">
								<plx:callInstance name="TryGetValue">
									<plx:callObject>
										<plx:nameRef name="childElementMappings"/>
									</plx:callObject>
									<plx:passParam>
										<plx:callStatic name="Concat" dataTypeName=".string" type="methodCall">
											<plx:passParam>
												<plx:nameRef name="containerNamespace"/>
											</plx:passParam>
											<plx:passParam>
												<plx:string>|</plx:string>
											</plx:passParam>
											<plx:passParam>
												<plx:nameRef name="containerName"/>
											</plx:passParam>
											<plx:passParam>
												<plx:string>|</plx:string>
											</plx:passParam>
											<plx:passParam>
												<plx:nameRef name="elementNamespace"/>
											</plx:passParam>
											<plx:passParam>
												<plx:string>|</plx:string>
											</plx:passParam>
											<plx:passParam>
												<plx:nameRef name="elementName"/>
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
										<plx:defaultValueOf dataTypeName="ORMCustomSerializedElementMatch"/>
									</xsl:otherwise>
								</xsl:choose>
							</plx:return>
						</xsl:otherwise>
					</xsl:choose>
				</plx:function>
			</xsl:if>
			<xsl:variable name="attributes" select="se:Attribute[@WriteStyle='Attribute' or not(@WriteStyle)]"/>
			<xsl:if test="$attributes">
				<plx:field name="myCustomSerializedAttributes" dataTypeName="Dictionary" visibility="private" static="true">
					<plx:passTypeParam dataTypeName=".string"/>
					<plx:passTypeParam dataTypeName="Guid"/>
				</plx:field>
			</xsl:if>
			<xsl:if test="$attributes or not($ClassOverride)">
				<plx:function visibility="protected" name="MapAttribute" replacesName="{$ClassOverride}">
					<plx:leadingInfo>
						<plx:docComment>
							<summary>Implements IORMCustomSerializedElement.MapAttribute</summary>
						</plx:docComment>
					</plx:leadingInfo>
					<plx:interfaceMember dataTypeName="IORMCustomSerializedElement" memberName="MapAttribute"/>
					<plx:param dataTypeName=".string" name="xmlNamespace"/>
					<plx:param dataTypeName=".string" name="attributeName"/>
					<plx:returns dataTypeName="Guid"/>
					<xsl:variable name="forwardToBase">
						<xsl:if test="$ClassOverride">
							<plx:callThis accessor="base" name="MapAttribute">
								<plx:passParam>
									<plx:nameRef type="parameter" name="xmlNamespace"/>
								</plx:passParam>
								<plx:passParam>
									<plx:nameRef type="parameter" name="attributeName"/>
								</plx:passParam>
							</plx:callThis>
						</xsl:if>
					</xsl:variable>
					<xsl:choose>
						<xsl:when test="$attributes">
							<xsl:variable name="namespaces" select="../se:MetaModel/se:Namespaces/se:Namespace"/>
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
									<plx:callInstance name="Add">
										<plx:callObject>
											<plx:nameRef name="customSerializedAttributes"/>
										</plx:callObject>
										<plx:passParam>
											<plx:string>
												<xsl:if test="string-length(@Prefix)">
													<!-- For attributes, the lack of a prefix means unqualified. Only concatenate if a namespace is explicitly specified -->
													<plx:string>
														<xsl:call-template name="ResolveNamespace">
															<xsl:with-param name="namespaces" select="$namespaces"/>
															<!-- Use default for prefix parameter -->
														</xsl:call-template>
													</plx:string>
													<plx:string>|</plx:string>
												</xsl:if>
												<plx:string>
													<xsl:choose>
														<xsl:when test="string-length(@Name)">
															<xsl:value-of select="@Name"/>
														</xsl:when>
														<xsl:otherwise>
															<xsl:value-of select="@ID"/>
														</xsl:otherwise>
													</xsl:choose>
												</plx:string>
											</plx:string>
										</plx:passParam>
										<plx:passParam>
											<plx:callStatic name="{@ID}MetaAttributeGuid" dataTypeName="{$ClassName}" type="field"/>
										</plx:passParam>
									</plx:callInstance>
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
									<plx:nameRef type="parameter" name="attributeName"/>
								</plx:initialize>
							</plx:local>
							<plx:branch>
								<plx:condition>
									<plx:binaryOperator type="inequality">
										<plx:left>
											<plx:callInstance name="Length" type="property">
												<plx:callObject>
													<plx:nameRef type="parameter" name="xmlNamespace"/>
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
												<plx:nameRef type="parameter" name="xmlNamespace"/>
											</plx:passParam>
											<plx:passParam>
												<plx:string>|</plx:string>
											</plx:passParam>
											<plx:passParam>
												<plx:nameRef type="parameter" name="attributeName"/>
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
				<plx:function visibility="protected" name="ShouldSerialize" replacesName="{$ClassOverride}">
					<xsl:if test="not($serializeBody and count($serializeBody/descendant::plx:thisKeyword | $serializeBody/descendant::plx:callThis[not(@accessor!='static')]))">
						<xsl:attribute name="modifier">
							<xsl:text>static</xsl:text>
						</xsl:attribute>
					</xsl:if>
					<plx:leadingInfo>
						<plx:docComment>
							<summary>Implements IORMCustomSerializedElement.ShouldSerialize</summary>
						</plx:docComment>
					</plx:leadingInfo>
					<plx:interfaceMember dataTypeName="IORMCustomSerializedElement" memberName="ShouldSerialize"/>
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
	<xsl:template match="se:MetaModel">
		<xsl:variable name="ModelName" select="@Class"/>
		<xsl:variable name="DisplayResourceId" select="@DisplayNameResourceId"/>
		<xsl:variable name="DescriptionResourceId" select="@DescriptionResourceId"/>
		<plx:class name="{$ModelName}" visibility="public" partial="true">
			<plx:leadingInfo>
				<plx:pragma type="region" data="{$ModelName} model serialization"/>
			</plx:leadingInfo>
			<plx:trailingInfo>
				<plx:pragma type="closeRegion" data="{$ModelName} model serialization"/>
			</plx:trailingInfo>
			<xsl:if test="$DisplayResourceId and $DescriptionResourceId">
				<plx:attribute dataTypeName="MetaModelDisplayName">
					<plx:passParam>
						<plx:typeOf dataTypeName="{$ModelName}"/>
					</plx:passParam>
					<plx:passParam>
						<plx:string>
							<xsl:value-of select="$DisplayResourceId"/>
						</plx:string>
					</plx:passParam>
				</plx:attribute>
				<plx:attribute dataTypeName="MetaModelDescription">
					<plx:passParam>
						<plx:typeOf dataTypeName="{$ModelName}"/>
					</plx:passParam>
					<plx:passParam>
						<plx:string>
							<xsl:value-of select="$DescriptionResourceId"/>
						</plx:string>
					</plx:passParam>
				</plx:attribute>
			</xsl:if>
			<plx:implementsInterface dataTypeName="IORMCustomSerializedMetaModel"/>
			<plx:field name="XmlNamespace" visibility="public" const="true" dataTypeName=".string" static="false">
				<plx:leadingInfo>
					<plx:docComment>
						<summary>The default XmlNamespace associated with the '<xsl:value-of select="$ModelName"/>' extension model</summary>
					</plx:docComment>
				</plx:leadingInfo>
				<xsl:variable name="DefaultXmlNamespace">
					<xsl:variable name="namespaces" select="se:Namespaces/se:Namespace"/>
					<xsl:variable name="defaultNamespace" select="$namespaces[@DefaultPrefix='true' or @DefaultPrefix='1']"/>
					<xsl:choose>
						<xsl:when test="$defaultNamespace">
							<xsl:value-of select="$defaultNamespace[1]/@URI"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="$namespaces[1]/@URI"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<plx:initialize>
					<plx:string><xsl:value-of select="$DefaultXmlNamespace"/></plx:string>
				</plx:initialize>
			</plx:field>
			<xsl:for-each select="se:Namespaces">
				<plx:property visibility="protected" name="DefaultElementPrefix" modifier="static">
					<plx:leadingInfo>
						<plx:docComment>
							<summary>Implements IORMCustomSerializedMetaModel.DefaultElementPrefix</summary>
						</plx:docComment>
					</plx:leadingInfo>
					<plx:interfaceMember dataTypeName="IORMCustomSerializedMetaModel" memberName="DefaultElementPrefix"/>
					<plx:returns dataTypeName=".string"/>
					<plx:get>
						<plx:return>
							<xsl:variable name="defaultElement" select="se:Namespace[@DefaultPrefix='true']"/>
							<xsl:choose>
								<xsl:when test="count($defaultElement)">
									<plx:string>
										<xsl:value-of select="$defaultElement/@Prefix"/>
									</plx:string>
								</xsl:when>
								<xsl:otherwise>
									<plx:nullKeyword/>
								</xsl:otherwise>
							</xsl:choose>
						</plx:return>
					</plx:get>
				</plx:property>
				<plx:function visibility="protected" name="GetCustomElementNamespaces" modifier="static">
					<plx:leadingInfo>
						<plx:docComment>
							<summary>Implements IORMCustomSerializedMetaModel.GetCustomElementNamespaces</summary>
						</plx:docComment>
					</plx:leadingInfo>
					<plx:interfaceMember dataTypeName="IORMCustomSerializedMetaModel" memberName="GetCustomElementNamespaces"/>
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
								<plx:string>
									<xsl:value-of select="@Prefix"/>
								</plx:string>
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
								<plx:string>
									<xsl:value-of select="@URI"/>
								</plx:string>
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
								<plx:string>
									<xsl:value-of select="@SchemaFile"/>
								</plx:string>
							</plx:right>
						</plx:assign>
					</xsl:for-each>
					<plx:return>
						<plx:nameRef name="ret"/>
					</plx:return>
				</plx:function>
			</xsl:for-each>
			<xsl:variable name="hasOmittedElements" select="0!=count(se:OmittedMetaElements/child::se:*)"/>
			<xsl:if test="$hasOmittedElements">
				<plx:field name="myCustomSerializationOmissions" dataTypeName="Dictionary" visibility="private">
					<plx:passTypeParam dataTypeName="MetaClassInfo"/>
					<plx:passTypeParam dataTypeName=".object"/>
				</plx:field>
				<plx:function name="BuildCustomSerializationOmissions" visibility="private" modifier="static">
					<plx:param name="store" dataTypeName="Store"/>
					<plx:returns dataTypeName="Dictionary">
						<plx:passTypeParam dataTypeName="MetaClassInfo"/>
						<plx:passTypeParam dataTypeName=".object"/>
					</plx:returns>
					<plx:local name="retVal" dataTypeName="Dictionary">
						<plx:passTypeParam dataTypeName="MetaClassInfo"/>
						<plx:passTypeParam dataTypeName=".object"/>
						<plx:initialize>
							<plx:callNew dataTypeName="Dictionary">
								<plx:passTypeParam dataTypeName="MetaClassInfo"/>
								<plx:passTypeParam dataTypeName=".object"/>
							</plx:callNew>
						</plx:initialize>
					</plx:local>
					<plx:local name="dataDir" dataTypeName="MetaDataDirectory">
						<plx:initialize>
							<plx:callInstance name="MetaDataDirectory" type="property">
								<plx:callObject>
									<plx:nameRef type="parameter" name="store"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:initialize>
					</plx:local>
					<xsl:for-each select="se:OmittedMetaElements/child::se:*">
						<xsl:variable name="classOrRelationship">
							<xsl:choose>
								<xsl:when test="local-name()='OmitClass'">
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
										<plx:callInstance name="FindMeta{$classOrRelationship}">
											<plx:callObject>
												<plx:nameRef name="dataDir"/>
											</plx:callObject>
											<plx:passParam>
												<plx:callStatic name="Meta{$classOrRelationship}Guid" type="field" dataTypeName="{@Class}" dataTypeQualifier="{@Namespace}"/>
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
			<plx:function visibility="protected" name="ShouldSerializeMetaClass">
				<plx:leadingInfo>
					<plx:docComment>
						<summary>Implements IORMCustomSerializedMetaModel.ShouldSerializeMetaClass</summary>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:interfaceMember dataTypeName="IORMCustomSerializedMetaModel" memberName="ShouldSerializeMetaClass"/>
				<plx:param name="store" dataTypeName="Store"/>
				<plx:param name="classInfo" dataTypeName="MetaClassInfo"/>
				<plx:returns dataTypeName=".boolean"/>
				<xsl:choose>
					<xsl:when test="$hasOmittedElements">
						<plx:local name="omissions" dataTypeName="Dictionary">
							<plx:passTypeParam dataTypeName="MetaClassInfo"/>
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
											<plx:nameRef type="parameter" name="store"/>
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
										<plx:nameRef type="parameter" name="classInfo"/>
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
			<plx:function visibility="protected" name="GetRootElementClasses" modifier="static">
				<plx:leadingInfo>
					<plx:docComment>
						<summary>Implements IORMCustomSerializedMetaModel.GetRootElementClasses</summary>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:interfaceMember dataTypeName="IORMCustomSerializedMetaModel" memberName="GetRootElementClasses"/>
				<plx:returns dataTypeName="Guid" dataTypeIsSimpleArray="true"/>
				<plx:return>
					<plx:callNew dataTypeName="Guid" dataTypeIsSimpleArray="true">
						<xsl:variable name="rootElements" select="se:RootElements/se:RootElement"/>
						<xsl:choose>
							<xsl:when test="$rootElements">
								<plx:arrayInitializer>
									<xsl:for-each select="$rootElements">
										<plx:passParam>
											<plx:callStatic dataTypeName="{@Class}" name="MetaClassGuid" type="field"/>
										</plx:passParam>
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
			<plx:function visibility="protected" name="MapRootElement" modifier="static">
				<plx:leadingInfo>
					<plx:docComment>
						<summary>Implements IORMCustomSerializedMetaModel.MapRootElement</summary>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:interfaceMember dataTypeName="IORMCustomSerializedMetaModel" memberName="MapRootElement"/>
				<plx:param name="xmlNamespace" dataTypeName=".string"/>
				<plx:param name="elementName" dataTypeName=".string"/>
				<plx:returns dataTypeName="Guid"/>
				<xsl:variable name="namespaces" select="se:Namespaces/se:Namespace"/>
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
											<plx:nameRef type="parameter" name="elementName"/>
										</plx:left>
										<plx:right>
											<plx:string>
												<xsl:value-of select="$tagName"/>
											</plx:string>
										</plx:right>
									</plx:binaryOperator>
								</plx:left>
								<plx:right>
									<plx:binaryOperator type="equality">
										<plx:left>
											<plx:nameRef type="parameter" name="xmlNamespace"/>
										</plx:left>
										<plx:right>
											<plx:string>
												<xsl:value-of select="$namespace"/>
											</plx:string>
										</plx:right>
									</plx:binaryOperator>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:return>
							<plx:callStatic dataTypeName="{$className}" name="MetaClassGuid" type="field"/>
						</plx:return>
					</plx:branch>
				</xsl:for-each>
				<plx:return>
					<plx:defaultValueOf dataTypeName="Guid"/>
				</plx:return>
			</plx:function>
			<plx:function visibility="protected" name="MapClassName" modifier="static">
				<plx:leadingInfo>
					<plx:docComment>
						<summary>Implements IORMCustomSerializedMetaModel.MapClassName</summary>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:interfaceMember dataTypeName="IORMCustomSerializedMetaModel" memberName="MapClassName"/>
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
								<plx:string>
									<xsl:value-of select="@URI"/>
								</plx:string>
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
					<xsl:for-each select="../se:Element">
						<plx:callInstance name="Add">
							<plx:callObject>
								<plx:nameRef name="classNameMap"/>
							</plx:callObject>
							<plx:passParam>
								<plx:string>
									<xsl:choose>
										<xsl:when test="string-length(@Name) > 0">
											<xsl:value-of select="@Name"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="@Class"/>
										</xsl:otherwise>
									</xsl:choose>
								</plx:string>
							</plx:passParam>
							<plx:passParam>
								<plx:callStatic name="MetaClassGuid" dataTypeName="{@Class}" type="property"/>
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
									<plx:string>
										<xsl:value-of select="@Name"/>
									</plx:string>
								</plx:passParam>
								<plx:passParam>
									<plx:callStatic name="MetaClassGuid" dataTypeName="{$className}" type="property"/>
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
										<plx:nameRef name="elementName"/>
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
								<plx:nameRef name="elementName"/>
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
	<xsl:template name="ReturnORMCustomSerializedElementSupportedOperations">
		<xsl:param name="childElements"/>
		<xsl:param name="element"/>
		<xsl:param name="attributes"/>
		<xsl:param name="links"/>
		<xsl:param name="customSort"/>
		<xsl:param name="mixedTypedAttributes"/>
		<xsl:variable name="supportedOperationsFragment">
			<xsl:if test="$childElements">
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
					<xsl:text>AttributeInfo</xsl:text>
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
		</xsl:variable>
		<xsl:variable name="supportedOperations" select="exsl:node-set($supportedOperationsFragment)"/>
		<xsl:variable name="operationCount" select="count($supportedOperations/child::*)"/>
		<xsl:choose>
			<xsl:when test="$operationCount=0">
				<plx:callStatic dataTypeName="ORMCustomSerializedElementSupportedOperations" name="None" type="field"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:for-each select="$supportedOperations/SupportedOperation">
					<xsl:if test="position()=1">
						<xsl:call-template name="OrTogetherEnumElements">
							<xsl:with-param name="EnumType" select="'ORMCustomSerializedElementSupportedOperations'"></xsl:with-param>
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
	<xsl:template name="CreateORMCustomSerializedElementInfoNameVariable">
		<xsl:param name="modifier"/>
		<xsl:variable name="conditionalNames" select="se:ConditionalName"/>
		<xsl:if test="$conditionalNames">
			<plx:local dataTypeName=".string" name="name{$modifier}">
				<plx:initialize>
					<xsl:choose>
						<xsl:when test="string-length(@Name)">
							<plx:string>
								<xsl:value-of select="@Name"/>
							</plx:string>
						</xsl:when>
						<xsl:otherwise>
							<plx:nullKeyword/>
						</xsl:otherwise>
					</xsl:choose>
				</plx:initialize>
			</plx:local>
			<xsl:variable name="primaryWriteStyle" select="string(@WriteStyle)"/>
			<xsl:if test="$conditionalNames/@WriteStyle[not(.=$primaryWriteStyle)]">
				<plx:local dataTypeName="ORMCustomSerializedElementWriteStyle" name="writeStyle{$modifier}">
					<plx:initialize>
						<plx:callStatic name="Element" dataTypeName="ORMCustomSerializedElementWriteStyle" type="field">
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
								<plx:string>
									<xsl:value-of select="$primaryDoubleTagName"/>
								</plx:string>
							</xsl:when>
							<xsl:otherwise>
								<plx:nullKeyword/>
							</xsl:otherwise>
						</xsl:choose>
					</plx:initialize>
				</plx:local>
			</xsl:if>
			<xsl:for-each select="$conditionalNames">
				<xsl:variable name="branchType">
					<xsl:choose>
						<xsl:when test="position()=1">
							<xsl:text>plx:branch</xsl:text>
						</xsl:when>
						<xsl:otherwise>
							<xsl:text>plx:alternateBranch</xsl:text>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:element name="{$branchType}">
					<plx:condition>
						<xsl:copy-of select="child::*"/>
					</plx:condition>
					<plx:assign>
						<plx:left>
							<plx:nameRef name="name{$modifier}"/>
						</plx:left>
						<plx:right>
							<plx:string>
								<xsl:value-of select="@Name"/>
							</plx:string>
						</plx:right>
					</plx:assign>
					<xsl:variable name="currentWriteStyle" select="string(@WriteStyle)"/>
					<xsl:if test="$currentWriteStyle and not($currentWriteStyle=$primaryWriteStyle)">
						<plx:assign>
							<plx:left>
								<plx:nameRef name="writeStyle{$modifier}"/>
							</plx:left>
							<plx:right>
								<plx:callStatic name="{$currentWriteStyle}" dataTypeName="ORMCustomSerializedElementWriteStyle" type="field"/>
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
								<plx:string>
									<xsl:value-of select="$currentDoubleTagName"/>
								</plx:string>
							</plx:right>
						</plx:assign>
					</xsl:if>
				</xsl:element>
			</xsl:for-each>
		</xsl:if>
	</xsl:template>
	<xsl:template name="PassORMCustomSerializedElementInfoParams">
		<xsl:param name="modifier"/>
		<xsl:variable name="conditionalNames" select="se:ConditionalName"/>
		<plx:passParam>
			<xsl:choose>
				<xsl:when test="string-length(@Prefix)">
					<plx:string>
						<xsl:value-of select="@Prefix"/>
					</plx:string>
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
							<plx:string>
								<xsl:value-of select="@Name"/>
							</plx:string>
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
					<plx:string>
						<xsl:value-of select="@Namespace"/>
					</plx:string>
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
					<plx:callStatic name="pending" type="field" dataTypeName="ORMCustomSerializedElementWriteStyle">
						<xsl:attribute name="name">
							<xsl:choose>
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
							<plx:string>
								<xsl:value-of select="$primaryDoubleTagName"/>
							</plx:string>
						</xsl:when>
						<xsl:otherwise>
							<plx:nullKeyword/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:otherwise>
			</xsl:choose>
		</plx:passParam>
	</xsl:template>
	<xsl:template name="ReturnORMCustomSerializedElementInfo">
		<xsl:call-template name="CreateORMCustomSerializedElementInfoNameVariable"/>
		<plx:return>
			<plx:callNew dataTypeName="ORMCustomSerializedElementInfo">
				<xsl:call-template name="PassORMCustomSerializedElementInfoParams"/>
			</plx:callNew>
		</plx:return>
	</xsl:template>
	<xsl:template name="ReturnORMCustomSerializedAttributeInfo">
		<xsl:for-each select="se:Condition">
			<plx:branch>
				<plx:condition>
					<xsl:copy-of select="child::*"/>
				</plx:condition>
				<xsl:call-template name="ReturnORMCustomSerializedAttributeInfo"/>
			</plx:branch>
		</xsl:for-each>
		<plx:return>
			<plx:callNew dataTypeName="ORMCustomSerializedAttributeInfo">
				<plx:passParam>
					<xsl:choose>
						<xsl:when test="string-length(@Prefix)">
							<plx:string>
								<xsl:value-of select="@Prefix"/>
							</plx:string>
						</xsl:when>
						<xsl:otherwise>
							<plx:nullKeyword/>
						</xsl:otherwise>
					</xsl:choose>
				</plx:passParam>
				<plx:passParam>
					<xsl:choose>
						<xsl:when test="string-length(@Name)">
							<plx:string>
								<xsl:value-of select="@Name"/>
							</plx:string>
						</xsl:when>
						<xsl:otherwise>
							<plx:nullKeyword/>
						</xsl:otherwise>
					</xsl:choose>
				</plx:passParam>
				<plx:passParam>
					<xsl:choose>
						<xsl:when test="string-length(@Namespace)">
							<plx:string>
								<xsl:value-of select="@Namespace"/>
							</plx:string>
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
					<plx:callStatic name="pending" type="field" dataTypeName="ORMCustomSerializedAttributeWriteStyle">
						<xsl:attribute name="name">
							<xsl:choose>
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
							<plx:string>
								<xsl:value-of select="@DoubleTagName"/>
							</plx:string>
						</xsl:when>
						<xsl:otherwise>
							<plx:nullKeyword/>
						</xsl:otherwise>
					</xsl:choose>
				</plx:passParam>
			</plx:callNew>
		</plx:return>
	</xsl:template>
</xsl:stylesheet>
