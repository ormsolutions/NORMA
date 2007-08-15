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
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:plx="http://schemas.neumont.edu/CodeGeneration/PLiX"
	xmlns:arg="http://schemas.neumont.edu/ORM/SDK/AttachRulesGenerator"
	xmlns:exsl="http://exslt.org/common"
	extension-element-prefixes="exsl">
	<!-- Indenting is useful for debugging the transform, but a waste of memory at generation time -->
	<xsl:output method="xml" encoding="utf-8" indent="no"/>
	<!-- Pick up param value supplied automatically by plix loader -->
	<xsl:param name="CustomToolNamespace" select="'TestNamespace'"/>
	<xsl:template match="arg:Rules">
		<plx:root>
			<plx:namespaceImport name="System"/>
			<plx:namespaceImport name="System.Reflection"/>
			<xsl:apply-templates select="child::*"/>
		</plx:root>
	</xsl:template>
	<xsl:template match="arg:Copyright"/>
	<xsl:template match="arg:Model">
		<xsl:variable name="namespaceNameTemp">
			<xsl:choose>
				<xsl:when test="string-length(@namespace)">
					<xsl:value-of select="@namespace"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$CustomToolNamespace"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="namespaceName" select="string($namespaceNameTemp)"/>
		<xsl:variable name="allRuleContainers" select="arg:RuleContainer"/>
		<xsl:variable name="allReflectedRuleTypesFragment">
			<xsl:copy-of select="arg:Rule"/>
			<!-- Merge all of the auto rules in with the normal rules for type resolution and enabled/disabled processing -->
			<xsl:for-each select="$allRuleContainers">
				<xsl:variable name="containerClass" select="string(@class)"/>
				<xsl:variable name="containerNamespace" select="@namespace"/>
				<xsl:for-each select="child::arg:*">
					<arg:AutoRule>
						<xsl:attribute name="class">
							<xsl:value-of select="$containerClass"/>
							<xsl:text>.</xsl:text>
							<xsl:choose>
								<xsl:when test="string(@className)">
									<xsl:value-of select="@className"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="@methodName"/>
									<xsl:text>Class</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:copy-of select="$containerNamespace"/>
						<xsl:copy-of select="@alwaysOn"/>
					</arg:AutoRule>
				</xsl:for-each>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="allReflectedRuleTypes" select="exsl:node-set($allReflectedRuleTypesFragment)/child::*"/>
		<xsl:variable name="disabledRules" select="$allReflectedRuleTypes[not(@alwaysOn='true' or @alwaysOn='1')]"/>
		<xsl:variable name="allReflectedOtherTypes" select="arg:*[not(self::arg:Rule | self::arg:RuleContainer)]"/>
		<xsl:variable name="enableDiagramRules" select="@enableDiagramRules='true' or @enabledDiagramRules='1'"/>
		<plx:namespace name="{$namespaceName}">
			<xsl:variable name="copyright" select="parent::arg:Rules/arg:Copyright"/>
			<xsl:if test="$copyright">
				<plx:leadingInfo>
					<plx:comment blankLine="true"/>
					<plx:comment>
						<xsl:value-of select="$copyright/@name"/>
					</plx:comment>
					<xsl:for-each select="$copyright/arg:CopyrightLine">
						<plx:comment>
							<xsl:value-of select="."/>
						</plx:comment>
					</xsl:for-each>
					<plx:comment blankLine="true"/>
				</plx:leadingInfo>
			</xsl:if>
			<plx:class name="{@class}" visibility="deferToPartial" partial="true">
				<plx:leadingInfo>
					<plx:pragma type="region" data="Attach rules to {@class} model"/>
				</plx:leadingInfo>
				<plx:trailingInfo>
					<plx:pragma type="closeRegion" data="Attach rules to {@class} model"/>
				</plx:trailingInfo>
				<xsl:if test="$disabledRules or $enableDiagramRules">
					<plx:implementsInterface dataTypeName="IDomainModelEnablesRulesAfterDeserialization" dataTypeQualifier="Neumont.Tools.Modeling.Shell"/>
				</xsl:if>
				<plx:field visibility="private" static="true" name="myCustomDomainModelTypes" dataTypeName="Type" dataTypeIsSimpleArray="true"/>
				<plx:property visibility="private" modifier="static" name="CustomDomainModelTypes">
					<plx:returns dataTypeName="Type" dataTypeIsSimpleArray="true"/>
					<plx:get>
						<plx:local name="retVal" dataTypeName="Type" dataTypeIsSimpleArray="true">
							<plx:initialize>
								<plx:callStatic type="field" name="myCustomDomainModelTypes" dataTypeName="{@class}"/>
							</plx:initialize>
						</plx:local>
						<plx:branch>
							<plx:condition>
								<plx:binaryOperator type="identityEquality">
									<plx:left>
										<plx:nameRef type="local" name="retVal"/>
									</plx:left>
									<plx:right>
										<plx:nullKeyword/>
									</plx:right>
								</plx:binaryOperator>
							</plx:condition>
							<plx:comment>No synchronization is needed here.</plx:comment>
							<plx:comment>If accessed concurrently, the worst that will happen is the array of Types being created multiple times.</plx:comment>
							<plx:comment>This would have a slightly negative impact on performance, but the result would still be correct.</plx:comment>
							<plx:comment>Given the low likelihood of this ever happening, the extra overhead of synchronization would outweigh any possible gain from it.</plx:comment>
							<plx:assign>
								<plx:left>
									<plx:nameRef type="local" name="retVal"/>
								</plx:left>
								<plx:right>
									<plx:callNew dataTypeName="Type" dataTypeIsSimpleArray="true">
										<plx:arrayInitializer>
											<xsl:variable name="contextClass" select="@class"/>
											<!-- Reflect rules first -->
											<xsl:for-each select="$allReflectedRuleTypes">
												<xsl:call-template name="GenerateTypeOf">
													<xsl:with-param name="className" select="@class"/>
													<xsl:with-param name="namespace" select="@namespace"/>
													<xsl:with-param name="contextClass" select="$contextClass"/>
													<xsl:with-param name="contextNamespace" select="$namespaceName"/>
												</xsl:call-template>
											</xsl:for-each>
											<xsl:for-each select="$allReflectedOtherTypes">
												<xsl:call-template name="GenerateTypeOf">
													<xsl:with-param name="className" select="@class"/>
													<xsl:with-param name="namespace" select="@namespace"/>
													<xsl:with-param name="contextClass" select="$contextClass"/>
													<xsl:with-param name="contextNamespace" select="$namespaceName"/>
												</xsl:call-template>
											</xsl:for-each>
										</plx:arrayInitializer>
									</plx:callNew>
								</plx:right>
							</plx:assign>
							<plx:assign>
								<plx:left>
									<plx:callStatic type="field" name="myCustomDomainModelTypes" dataTypeName="{@class}"/>
								</plx:left>
								<plx:right>
									<plx:nameRef type="local" name="retVal"/>
								</plx:right>
							</plx:assign>
							<plx:callStatic name="Assert" dataTypeName="Debug" dataTypeQualifier="System.Diagnostics">
								<plx:passParam>
									<plx:binaryOperator type="lessThan">
										<plx:left>
											<plx:callStatic type="methodCall" name="IndexOf" dataTypeName="Array">
												<plx:passMemberTypeParam dataTypeName="Type"/>
												<plx:passParam>
													<plx:nameRef type="local" name="retVal"/>
												</plx:passParam>
												<plx:passParam>
													<plx:nullKeyword/>
												</plx:passParam>
											</plx:callStatic>
										</plx:left>
										<plx:right>
											<plx:value type="i4" data="0"/>
										</plx:right>
									</plx:binaryOperator>
								</plx:passParam>
								<plx:passParam>
									<plx:string>One or more rule types failed to resolve. The file and/or package will fail to load.</plx:string>
								</plx:passParam>
							</plx:callStatic>
						</plx:branch>
						<plx:return>
							<plx:nameRef name="retVal"/>
						</plx:return>
					</plx:get>
				</plx:property>
				<xsl:if test="$disabledRules and (count($allReflectedRuleTypes)!=count($disabledRules))">
					<!-- If the counts are the same, then all rules are disabled and we don't need
					a second array. Otherwise, build a second array from the first. -->
					<plx:field visibility="private" static="true" name="myInitiallyDisabledRuleTypes" dataTypeName="Type" dataTypeIsSimpleArray="true"/>
					<plx:property visibility="private" modifier="static" name="InitiallyDisabledRuleTypes">
						<plx:returns dataTypeName="Type" dataTypeIsSimpleArray="true"/>
						<plx:get>
							<plx:local name="retVal" dataTypeName="Type" dataTypeIsSimpleArray="true">
								<plx:initialize>
									<plx:callStatic type="field" name="myInitiallyDisabledRuleTypes" dataTypeName="{@class}"/>
								</plx:initialize>
							</plx:local>
							<plx:branch>
								<plx:condition>
									<plx:binaryOperator type="identityEquality">
										<plx:left>
											<plx:nameRef type="local" name="retVal"/>
										</plx:left>
										<plx:right>
											<plx:nullKeyword/>
										</plx:right>
									</plx:binaryOperator>
								</plx:condition>
								<plx:local name="customDomainModelTypes" dataTypeName="Type" dataTypeIsSimpleArray="true">
									<plx:initialize>
										<plx:callStatic type="property" name="CustomDomainModelTypes" dataTypeName="{@class}"/>
									</plx:initialize>
								</plx:local>
								<plx:assign>
									<plx:left>
										<plx:nameRef name="retVal"/>
									</plx:left>
									<plx:right>
										<plx:callNew dataTypeName="Type" dataTypeIsSimpleArray="true">
											<plx:arrayInitializer>
												<xsl:for-each select="$allReflectedRuleTypes">
													<!-- We already have this set in disabledRules, but we need the position values from the allReflectedRuleTypes set, so refilter -->
													<xsl:if test="not(@alwaysOn='true' or @alwaysOn='1')">
														<plx:callInstance name=".implied" type="arrayIndexer">
															<plx:callObject>
																<plx:nameRef name="customDomainModelTypes"/>
															</plx:callObject>
															<plx:passParam>
																<plx:value data="{position()-1}" type="i4"/>
															</plx:passParam>
														</plx:callInstance>
													</xsl:if>
												</xsl:for-each>
											</plx:arrayInitializer>
										</plx:callNew>
									</plx:right>
								</plx:assign>
								<plx:assign>
									<plx:left>
										<plx:callStatic type="field" name="myInitiallyDisabledRuleTypes" dataTypeName="{@class}"/>
									</plx:left>
									<plx:right>
										<plx:nameRef type="local" name="retVal"/>
									</plx:right>
								</plx:assign>
							</plx:branch>
							<plx:return>
								<plx:nameRef name="retVal"/>
							</plx:return>
						</plx:get>
					</plx:property>
				</xsl:if>
				<plx:function name="GetCustomDomainModelTypes" visibility="protected" modifier="override">
					<plx:leadingInfo>
						<plx:docComment>
							<summary>Generated code to attach &lt;see cref="Microsoft.VisualStudio.Modeling.Rule"/&gt;s to the &lt;see cref="Microsoft.VisualStudio.Modeling.Store"/&gt;.</summary>
							<seealso cref="Microsoft.VisualStudio.Modeling.DomainModel.GetCustomDomainModelTypes"/>
						</plx:docComment>
					</plx:leadingInfo>
					<plx:returns dataTypeName="Type" dataTypeIsSimpleArray="true"/>
					<plx:branch>
						<plx:condition>
							<plx:callStatic dataTypeName="FrameworkDomainModel" dataTypeQualifier="Neumont.Tools.Modeling" name="InitializingToolboxItems" type="property"/>
						</plx:condition>
						<plx:return>
							<plx:callStatic dataTypeName="Type" name="EmptyTypes" type="property"/>
						</plx:return>
					</plx:branch>
					<plx:local name="retVal" dataTypeName="Type" dataTypeIsSimpleArray="true">
						<plx:initialize>
							<plx:callThis accessor="base" type="methodCall" name="GetCustomDomainModelTypes"/>
						</plx:initialize>
					</plx:local>
					<plx:local name="baseLength" dataTypeName=".i4">
						<plx:initialize>
							<plx:callInstance type="property" name="Length">
								<plx:callObject>
									<plx:nameRef type="local" name="retVal"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:initialize>
					</plx:local>
					<plx:local name="customDomainModelTypes" dataTypeName="Type" dataTypeIsSimpleArray="true">
						<plx:initialize>
							<plx:callStatic type="property" name="CustomDomainModelTypes" dataTypeName="{@class}"/>
						</plx:initialize>
					</plx:local>
					<plx:branch>
						<plx:condition>
							<plx:binaryOperator type="lessThanOrEqual">
								<plx:left>
									<plx:nameRef type="local" name="baseLength"/>
								</plx:left>
								<plx:right>
									<plx:value type="i4" data="0"/>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:return>
							<plx:nameRef type="local" name="customDomainModelTypes"/>
						</plx:return>
					</plx:branch>
					<plx:fallbackBranch>
						<plx:callStatic type="methodCall" name="Resize" dataTypeName="Array">
							<plx:passMemberTypeParam dataTypeName="Type"/>
							<plx:passParam type="inOut">
								<plx:nameRef type="local" name="retVal"/>
							</plx:passParam>
							<plx:passParam>
								<plx:binaryOperator type="add">
									<plx:left>
										<plx:nameRef type="local" name="baseLength"/>
									</plx:left>
									<plx:right>
										<plx:callInstance type="property" name="Length">
											<plx:callObject>
												<plx:nameRef type="local" name="customDomainModelTypes"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:right>
								</plx:binaryOperator>
							</plx:passParam>
						</plx:callStatic>
						<plx:callInstance type="methodCall" name="CopyTo">
							<plx:callObject>
								<plx:nameRef type="local" name="customDomainModelTypes"/>
							</plx:callObject>
							<plx:passParam>
								<plx:nameRef type="local" name="retVal"/>
							</plx:passParam>
							<plx:passParam>
								<plx:nameRef type="local" name="baseLength"/>
							</plx:passParam>
						</plx:callInstance>
						<plx:return>
							<plx:nameRef type="local" name="retVal"/>
						</plx:return>
					</plx:fallbackBranch>
				</plx:function>
				<xsl:if test="$disabledRules or $enableDiagramRules">
					<plx:function name="EnableRulesAfterDeserialization" visibility="protected">
						<plx:leadingInfo>
							<plx:docComment>
								<summary>Implements IDomainModelEnablesRulesAfterDeserialization.EnableRulesAfterDeserialization</summary>
							</plx:docComment>
						</plx:leadingInfo>
						<plx:interfaceMember dataTypeName="IDomainModelEnablesRulesAfterDeserialization" dataTypeQualifier="Neumont.Tools.Modeling.Shell" memberName="EnableRulesAfterDeserialization"/>
						<plx:param name="store" dataTypeName="Store" dataTypeQualifier="Microsoft.VisualStudio.Modeling"/>
						<xsl:if test="$enableDiagramRules">
							<plx:callStatic name="EnableDiagramRules" dataTypeName="{@class}">
								<plx:passParam>
									<plx:nameRef name="store" type="parameter"/>
								</plx:passParam>
							</plx:callStatic>
						</xsl:if>
						<xsl:if test="$disabledRules">
							<plx:local name="ruleManager" dataTypeName="RuleManager" dataTypeQualifier="Microsoft.VisualStudio.Modeling">
								<plx:initialize>
									<plx:callInstance name="RuleManager" type="property">
										<plx:callObject>
											<plx:nameRef name="store" type="parameter"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:initialize>
							</plx:local>
							<plx:local name="disabledRuleTypes" dataTypeName="Type" dataTypeIsSimpleArray="true">
								<plx:initialize>
									<plx:callStatic type="property" name="CustomDomainModelTypes" dataTypeName="{@class}">
										<xsl:if test="count($allReflectedRuleTypes)!=count($disabledRules)">
											<!-- Only use the cached types if all of the types are rules and all are disabled -->
											<xsl:attribute name="name">
												<xsl:text>InitiallyDisabledRuleTypes</xsl:text>
											</xsl:attribute>
										</xsl:if>
									</plx:callStatic>
								</plx:initialize>
							</plx:local>
							<plx:loop>
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
											<plx:nameRef name="i"/>
										</plx:left>
										<plx:right>
											<plx:value type="i4" data="{count($disabledRules)}"/>
										</plx:right>
									</plx:binaryOperator>
								</plx:condition>
								<plx:beforeLoop>
									<plx:increment>
										<plx:nameRef name="i"/>
									</plx:increment>
								</plx:beforeLoop>
								<plx:callInstance name="EnableRule">
									<plx:callObject>
										<plx:nameRef name="ruleManager"/>
									</plx:callObject>
									<plx:passParam>
										<plx:callInstance name=".implied" type="arrayIndexer">
											<plx:callObject>
												<plx:nameRef name="disabledRuleTypes"/>
											</plx:callObject>
											<plx:passParam>
												<plx:nameRef name="i"/>
											</plx:passParam>
										</plx:callInstance>
									</plx:passParam>
								</plx:callInstance>
							</plx:loop>
						</xsl:if>
					</plx:function>
				</xsl:if>
			</plx:class>
			<xsl:for-each select="$disabledRules[self::arg:Rule][not(@namespace) or @namespace=$namespaceName]">
				<xsl:call-template name="GenerateInitiallyDisabledRuleClass"/>
			</xsl:for-each>
			<xsl:for-each select="$allRuleContainers[not(@namespace) or @namespace=$namespaceName]">
				<xsl:call-template name="GenerateRuleContainerClass"/>
			</xsl:for-each>
		</plx:namespace>
		<!-- Disable rules for namespaces in other classes -->
		<!-- Note that the != predicate is intentional here -->
		<xsl:variable name="foreignDisabledRules" select="$disabledRules[self::arg:Rule][@namespace!=$namespaceName]"/>
		<xsl:if test="$foreignDisabledRules">
			<xsl:variable name="uniqueNamespacesFragment">
				<xsl:variable name="sortedNamespacesFragment">
					<xsl:for-each select="$foreignDisabledRules">
						<xsl:sort select="@namespace"/>
						<dummy>
							<xsl:copy-of select="@namespace"/>
						</dummy>
					</xsl:for-each>
				</xsl:variable>
				<xsl:for-each select="exsl:node-set($sortedNamespacesFragment)/child::*">
					<xsl:if test="not(preceding-sibling::*[@namespace=current()/@namespace])">
						<xsl:copy-of select="."/>
					</xsl:if>
				</xsl:for-each>
			</xsl:variable>
			<xsl:for-each select="exsl:node-set($uniqueNamespacesFragment)/child::*">
				<xsl:variable name="foreignNamespace" select="string(@namespace)"/>
				<plx:namespace name="{$foreignNamespace}">
					<xsl:for-each select="$foreignDisabledRules[@namespace=$foreignNamespace]">
						<xsl:call-template name="GenerateInitiallyDisabledRuleClass"/>
					</xsl:for-each>
				</plx:namespace>
			</xsl:for-each>
		</xsl:if>
		<!-- Generate auto-rule classes for other namespaces -->
		<!-- Note that the != predicate is intentional here -->
		<xsl:variable name="foreignAutoRules" select="$allRuleContainers[@namespace!=$namespaceName]"/>
		<xsl:if test="$foreignAutoRules">
			<xsl:variable name="uniqueNamespacesFragment">
				<xsl:variable name="sortedNamespacesFragment">
					<xsl:for-each select="$foreignAutoRules">
						<xsl:sort select="@namespace"/>
						<dummy>
							<xsl:copy-of select="@namespace"/>
						</dummy>
					</xsl:for-each>
				</xsl:variable>
				<xsl:for-each select="exsl:node-set($sortedNamespacesFragment)/child::*">
					<xsl:if test="not(preceding-sibling::*[@namespace=current()/@namespace])">
						<xsl:copy-of select="."/>
					</xsl:if>
				</xsl:for-each>
			</xsl:variable>
			<xsl:for-each select="exsl:node-set($uniqueNamespacesFragment)/child::*">
				<xsl:variable name="foreignNamespace" select="string(@namespace)"/>
				<plx:namespace name="{$foreignNamespace}">
					<xsl:for-each select="$foreignAutoRules[@namespace=$foreignNamespace]">
						<xsl:call-template name="GenerateRuleContainerClass"/>
					</xsl:for-each>
				</plx:namespace>
			</xsl:for-each>
		</xsl:if>
	</xsl:template>
	<xsl:template name="GenerateTypeOf">
		<xsl:param name="className"/>
		<xsl:param name="namespace" select="''"/>
		<xsl:param name="contextClass" select="''"/>
		<xsl:param name="contextNamespace"/>
		<xsl:variable name="namespaceNameTemp">
			<xsl:choose>
				<xsl:when test="string-length($namespace)">
					<xsl:value-of select="$namespace"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$CustomToolNamespace"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="namespaceName" select="string($namespaceNameTemp)"/>
		<xsl:variable name="normalizedName" select="normalize-space(translate($className, '+.','  '))"/>
		<xsl:variable name="publicPart" select="substring-before($normalizedName,' ')"/>
		<xsl:choose>
			<xsl:when test="string-length($publicPart) and $namespaceName=$contextNamespace and $publicPart=$contextClass">
				<xsl:call-template name="GenerateTypeOf">
					<xsl:with-param name="className" select="substring($normalizedName, string-length($publicPart)+2)"/>
					<xsl:with-param name="contextNamespace" select="$contextNamespace"/>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:variable name="primaryTypeOf">
					<plx:typeOf dataTypeName="{$className}">
						<xsl:if test="string-length($publicPart)">
							<xsl:attribute name="dataTypeName">
								<xsl:value-of select="$publicPart"/>
							</xsl:attribute>
						</xsl:if>
						<xsl:if test="$contextNamespace!=$namespaceName">
							<xsl:attribute name="dataTypeQualifier">
								<xsl:value-of select="$namespaceName"/>
							</xsl:attribute>
						</xsl:if>
					</plx:typeOf>
				</xsl:variable>
				<xsl:choose>
					<xsl:when test="string-length($publicPart)">
						<xsl:call-template name="GeneratedNestedTypeCall">
							<xsl:with-param name="nestedTypes" select="substring($normalizedName, string-length($publicPart)+2)"/>
							<xsl:with-param name="callObject" select="$primaryTypeOf"/>
						</xsl:call-template>
					</xsl:when>
					<xsl:otherwise>
						<xsl:copy-of select="$primaryTypeOf"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="GeneratedNestedTypeCall">
		<xsl:param name="nestedTypes"/>
		<xsl:param name="callObject"/>
		<xsl:variable name="firstType" select="substring-before($nestedTypes, ' ')"/>
		<xsl:variable name="nestedTypeCall">
			<plx:callInstance name="GetNestedType">
				<plx:callObject>
					<xsl:copy-of select="$callObject"/>
				</plx:callObject>
				<plx:passParam>
					<plx:string>
						<xsl:choose>
							<xsl:when test="string-length($firstType)">
								<xsl:value-of select="$firstType"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$nestedTypes"/>
							</xsl:otherwise>
						</xsl:choose>
					</plx:string>
				</plx:passParam>
				<plx:passParam>
					<plx:binaryOperator type="bitwiseOr">
						<plx:left>
							<plx:callStatic name="Public" dataTypeName="BindingFlags" type="field"/>
						</plx:left>
						<plx:right>
							<plx:callStatic name="NonPublic" dataTypeName="BindingFlags" type="field"/>
						</plx:right>
					</plx:binaryOperator>
				</plx:passParam>
			</plx:callInstance>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="string-length($firstType)">
				<xsl:call-template name="GeneratedNestedTypeCall">
					<xsl:with-param name="nestedTypes" select="substring($nestedTypes, string-length($firstType)+2)"/>
					<xsl:with-param name="callObject" select="$nestedTypeCall"/>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:copy-of select="$nestedTypeCall"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="GenerateInitiallyDisabledRuleClass">
		<xsl:param name="remainingName" select="normalize-space(translate(@class, '+.', '  '))"/>
		<xsl:param name="firstPart" select="true()"/>
		<xsl:variable name="publicPart" select="substring-before($remainingName, ' ')"/>
		<plx:class name="{$remainingName}" visibility="deferToPartial" partial="true">
			<xsl:if test="$publicPart">
				<xsl:attribute name="name">
					<xsl:value-of select="$publicPart"/>
				</xsl:attribute>
			</xsl:if>
			<xsl:if test="$firstPart">
				<xsl:if test="position()=1">
					<plx:leadingInfo>
						<plx:pragma type="region" data="Initially disable rules"/>
					</plx:leadingInfo>
				</xsl:if>
				<xsl:if test="position()=last()">
					<plx:trailingInfo>
						<plx:pragma type="closeRegion" data="Initially disable rules"/>
					</plx:trailingInfo>
				</xsl:if>
			</xsl:if>
			<xsl:choose>
				<xsl:when test="$publicPart">
					<xsl:call-template name="GenerateInitiallyDisabledRuleClass">
						<xsl:with-param name="remainingName" select="substring($remainingName, string-length($publicPart)+2)"/>
						<xsl:with-param name="firstPart" select="false()"/>
					</xsl:call-template>
				</xsl:when>
				<xsl:otherwise>
					<plx:function name=".construct" visibility="public">
						<plx:attribute dataTypeName="DebuggerStepThrough" dataTypeQualifier="System.Diagnostics"/>
						<plx:assign>
							<plx:left>
								<plx:callThis accessor="base" name="IsEnabled" type="property"/>
							</plx:left>
							<plx:right>
								<plx:falseKeyword/>
							</plx:right>
						</plx:assign>
					</plx:function>
				</xsl:otherwise>
			</xsl:choose>
		</plx:class>
	</xsl:template>
	<xsl:template name="GenerateRuleContainerClass">
		<xsl:param name="remainingName" select="normalize-space(translate(@class, '+.', '  '))"/>
		<xsl:param name="firstPart" select="true()"/>
		<xsl:param name="originalClass" select="translate($remainingName,' ','.')"/>
		<xsl:variable name="publicPart" select="substring-before($remainingName, ' ')"/>
		<plx:class name="{$remainingName}" visibility="deferToPartial" partial="true">
			<xsl:if test="$publicPart">
				<xsl:attribute name="name">
					<xsl:value-of select="$publicPart"/>
				</xsl:attribute>
			</xsl:if>
			<xsl:if test="$firstPart">
				<plx:leadingInfo>
					<xsl:if test="position()=1">
						<plx:pragma type="region" data="Auto-rule classes"/>
					</xsl:if>
					<plx:pragma type="region">
						<xsl:attribute name="data">
							<xsl:text>Rule classes for </xsl:text>
							<xsl:value-of select="$originalClass"/>
						</xsl:attribute>
					</plx:pragma>
				</plx:leadingInfo>
				<plx:trailingInfo>
					<plx:pragma type="closeRegion">
						<xsl:attribute name="data">
							<xsl:text>Rule classes for </xsl:text>
							<xsl:value-of select="$originalClass"/>
						</xsl:attribute>
					</plx:pragma>
					<xsl:if test="position()=last()">
						<plx:pragma type="closeRegion" data="Auto-rule classes"/>
					</xsl:if>
				</plx:trailingInfo>
			</xsl:if>
			<xsl:choose>
				<xsl:when test="$publicPart">
					<xsl:call-template name="GenerateRuleContainerClass">
						<xsl:with-param name="remainingName" select="substring($remainingName, string-length($publicPart)+2)"/>
						<xsl:with-param name="firstPart" select="false()"/>
						<xsl:with-param name="originalClass" select="$originalClass"/>
					</xsl:call-template>
				</xsl:when>
				<xsl:otherwise>
					<xsl:for-each select="child::*">
						<xsl:variable name="methodInRuleClass" select="@methodInRuleClass='true' or @methodInRuleClass='1'"/>
						<xsl:variable name="methodInfoFragment">
							<xsl:apply-templates select="." mode="RuleMethodInfo"/>
						</xsl:variable>
						<xsl:variable name="methodInfo" select="exsl:node-set($methodInfoFragment)/child::*"/>
						<plx:class modifier="sealed" name="{@methodName}Class" visibility="private" partial="{string($methodInRuleClass)}">
							<xsl:if test="string(@className)">
								<xsl:attribute name="name">
									<xsl:value-of select="@className"/>
								</xsl:attribute>
							</xsl:if>
							<xsl:for-each select="arg:RuleOn">
								<xsl:variable name="fireTime" select="string(@fireTime)"/>
								<xsl:variable name="standardPriority" select="normalize-space(@priority)"/>
								<xsl:variable name="priorityAdjustment" select="arg:PriorityAdjustment/child::plx:*"/>
								<plx:attribute dataTypeName="RuleOn" dataTypeQualifier="Microsoft.VisualStudio.Modeling">
									<plx:passParam>
										<plx:typeOf dataTypeName="{@targetType}" dataTypeQualifier="{@targetTypeQualifier}"/>
									</plx:passParam>
									<xsl:if test="string-length($fireTime) and $fireTime!='Inline'">
										<plx:passParam>
											<plx:binaryOperator type="assignNamed">
												<plx:left>
													<plx:nameRef name="FireTime" type="namedParameter"/>
												</plx:left>
												<plx:right>
													<plx:callStatic name="{$fireTime}" dataTypeName="TimeToFire" dataTypeQualifier="Microsoft.VisualStudio.Modeling" type="field"/>
												</plx:right>
											</plx:binaryOperator>
										</plx:passParam>
									</xsl:if>
									<xsl:if test="$standardPriority or $priorityAdjustment">
										<plx:passParam>
											<plx:binaryOperator type="assignNamed">
												<plx:left>
													<plx:nameRef name="Priority" type="namedParameter"/>
												</plx:left>
												<plx:right>
													<xsl:choose>
														<xsl:when test="$standardPriority">
															<xsl:variable name="standardPriorityFragment">
																<xsl:choose>
																	<xsl:when test="$standardPriority='BeforeDelayValidateRulePriority'">
																		<plx:callStatic name="BeforeDelayValidateRulePriority" dataTypeName="FrameworkDomainModel" dataTypeQualifier="Neumont.Tools.Modeling" type="field"/>
																	</xsl:when>
																	<xsl:when test="$standardPriority='DelayValidateRulePriority'">
																		<plx:callStatic name="DelayValidateRulePriority" dataTypeName="FrameworkDomainModel" dataTypeQualifier="Neumont.Tools.Modeling" type="field"/>
																	</xsl:when>
																	<xsl:when test="starts-with($standardPriority,'D')">
																		<plx:callStatic name="{substring-after($standardPriority,'.')}" dataTypeName="DiagramFixupConstants" dataTypeQualifier="Microsoft.VisualStudio.Modeling.Diagrams" type="field"/>
																	</xsl:when>
																	<xsl:when test="starts-with($standardPriority, '-')">
																		<plx:unaryOperator type="negative">
																			<plx:value data="{substring-after($standardPriority,'-')}" type="i4"/>
																		</plx:unaryOperator>
																	</xsl:when>
																	<xsl:when test="starts-with($standardPriority, '+')">
																		<plx:value data="{substring-after($standardPriority,'+')}" type="i4"/>
																	</xsl:when>
																	<xsl:otherwise>
																		<plx:value data="{$standardPriority}" type="i4"/>
																	</xsl:otherwise>
																</xsl:choose>
															</xsl:variable>
															<xsl:choose>
																<xsl:when test="$priorityAdjustment">
																	<plx:binaryOperator type="add">
																		<plx:left>
																			<xsl:copy-of select="$standardPriorityFragment"/>
																		</plx:left>
																		<plx:right>
																			<xsl:copy-of select="$priorityAdjustment"/>
																		</plx:right>
																	</plx:binaryOperator>
																</xsl:when>
																<xsl:otherwise>
																	<xsl:copy-of select="$standardPriorityFragment"/>
																</xsl:otherwise>
															</xsl:choose>
														</xsl:when>
														<xsl:otherwise>
															<xsl:copy-of select="$priorityAdjustment"/>
														</xsl:otherwise>
													</xsl:choose>
												</plx:right>
											</plx:binaryOperator>
										</plx:passParam>
									</xsl:if>
								</plx:attribute>
							</xsl:for-each>
							<plx:derivesFromClass dataTypeName="{local-name()}" dataTypeQualifier="Microsoft.VisualStudio.Modeling"/>
							<xsl:if test="not(@alwaysOn='true' or @alwaysOn='1')">
								<plx:function name=".construct" visibility="public">
									<plx:attribute dataTypeName="DebuggerStepThrough" dataTypeQualifier="System.Diagnostics"/>
									<plx:assign>
										<plx:left>
											<plx:callThis accessor="base" name="IsEnabled" type="property"/>
										</plx:left>
										<plx:right>
											<plx:falseKeyword/>
										</plx:right>
									</plx:assign>
								</plx:function>
							</xsl:if>
							<xsl:if test="@fireBefore='true' or @fireBefore='1'">
								<plx:property modifier="override" visibility="public" name="FireBefore">
									<plx:returns dataTypeName=".boolean"/>
									<plx:get>
										<plx:attribute dataTypeName="DebuggerStepThrough" dataTypeQualifier="System.Diagnostics"/>
										<plx:return>
											<plx:trueKeyword/>
										</plx:return>
									</plx:get>
								</plx:property>
							</xsl:if>
							<plx:function modifier="override" visibility="public" name="{$methodInfo/@ruleMethodName}">
								<plx:leadingInfo>
									<plx:docComment>
										<summary>
											<xsl:text>&#xd;&#xa;Provide the following method in class: &#xd;&#xa;</xsl:text>
											<xsl:choose>
												<xsl:when test="string(../@namespace)">
													<xsl:value-of select="../@namespace"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="$CustomToolNamespace"/>
												</xsl:otherwise>
											</xsl:choose>
											<xsl:text>.</xsl:text>
											<xsl:value-of select="$originalClass"/>
											<xsl:text>&#xd;&#xa;</xsl:text>
											<xsl:variable name="leadingInfo">
												<plx:leadingInfo>
													<plx:docComment>
														<summary>
															<xsl:text>&#xd;&#xa;</xsl:text>
															<xsl:for-each select="arg:RuleOn">
																<xsl:variable name="fireTime" select="string(@fireTime)"/>
																<xsl:value-of select="local-name(..)"/>
																<xsl:text>: typeof(</xsl:text>
																<xsl:if test="string(@targetTypeQualifier)">
																	<xsl:value-of select="@targetTypeQualifier"/>
																	<xsl:text>.</xsl:text>
																</xsl:if>
																<xsl:value-of select="@targetType"/>
																<xsl:text>)</xsl:text>
																<xsl:if test="$fireTime and $fireTime!='Inline'">
																	<xsl:text>, FireTime=</xsl:text>
																	<xsl:value-of select="$fireTime"/>
																</xsl:if>
																<xsl:variable name="standardPriority" select="normalize-space(@priority)"/>
																<xsl:variable name="priorityAdjustment" select="arg:PriorityAdjustment/child::plx:*"/>
																<xsl:choose>
																	<xsl:when test="$standardPriority or $priorityAdjustment">
																		<xsl:text>, Priority=</xsl:text>
																		<xsl:choose>
																			<xsl:when test="$standardPriority">
																				<xsl:variable name="standardPriorityFragment">
																					<xsl:choose>
																						<xsl:when test="$standardPriority='BeforeDelayValidateRulePriority'">
																							<plx:callStatic name="BeforeDelayValidateRulePriority" dataTypeName="FrameworkDomainModel" type="field"/>
																						</xsl:when>
																						<xsl:when test="$standardPriority='DelayValidateRulePriority'">
																							<plx:callStatic name="DelayValidateRulePriority" dataTypeName="FrameworkDomainModel" type="field"/>
																						</xsl:when>
																						<xsl:when test="starts-with($standardPriority,'D')">
																							<plx:callStatic name="{substring-after($standardPriority,'.')}" dataTypeName="DiagramFixupConstants" type="field"/>
																						</xsl:when>
																						<xsl:when test="starts-with($standardPriority, '-')">
																							<plx:unaryOperator type="negative">
																								<plx:value data="{substring-after($standardPriority,'-')}" type="i4"/>
																							</plx:unaryOperator>
																						</xsl:when>
																						<xsl:when test="starts-with($standardPriority, '+')">
																							<plx:value data="{substring-after($standardPriority,'+')}" type="i4"/>
																						</xsl:when>
																						<xsl:otherwise>
																							<plx:value data="{$standardPriority}" type="i4"/>
																						</xsl:otherwise>
																					</xsl:choose>
																				</xsl:variable>
																				<xsl:choose>
																					<xsl:when test="$priorityAdjustment">
																						<plx:binaryOperator type="add">
																							<plx:left>
																								<xsl:copy-of select="$standardPriorityFragment"/>
																							</plx:left>
																							<plx:right>
																								<xsl:copy-of select="$priorityAdjustment"/>
																							</plx:right>
																						</plx:binaryOperator>
																					</xsl:when>
																					<xsl:otherwise>
																						<xsl:copy-of select="$standardPriorityFragment"/>
																					</xsl:otherwise>
																				</xsl:choose>
																			</xsl:when>
																			<xsl:otherwise>
																				<xsl:copy-of select="$priorityAdjustment"/>
																			</xsl:otherwise>
																		</xsl:choose>
																	</xsl:when>
																	<xsl:otherwise>
																		<xsl:text>&#xd;&#xa;</xsl:text>
																	</xsl:otherwise>
																</xsl:choose>
															</xsl:for-each>
														</summary>
													</plx:docComment>
												</plx:leadingInfo>
											</xsl:variable>
											<xsl:choose>
												<xsl:when test="$methodInRuleClass">
													<plx:class visibility="deferToPartial" partial="true" name="{@methodName}Class">
														<xsl:if test="string(@className)">
															<xsl:attribute name="name">
																<xsl:value-of select="@className"/>
															</xsl:attribute>
														</xsl:if>
														<plx:function name="{@methodName}" visibility="private">
															<xsl:copy-of select="$leadingInfo"/>
															<plx:param name="e" dataTypeName="{$methodInfo/@ruleEventArgsType}"/>
														</plx:function>
													</plx:class>
												</xsl:when>
												<xsl:otherwise>
													<plx:function modifier="static" name="{@methodName}" visibility="private">
														<xsl:copy-of select="$leadingInfo"/>
														<plx:param name="e" dataTypeName="{$methodInfo/@ruleEventArgsType}"/>
													</plx:function>
												</xsl:otherwise>
											</xsl:choose>
										</summary>
									</plx:docComment>
								</plx:leadingInfo>
								<plx:attribute dataTypeName="DebuggerStepThrough" dataTypeQualifier="System.Diagnostics"/>
								<plx:param name="e" dataTypeName="{$methodInfo/@ruleEventArgsType}" dataTypeQualifier="Microsoft.VisualStudio.Modeling"/>
								<xsl:variable name="classFullNameString">
									<plx:string>
										<xsl:attribute name="data">
											<xsl:choose>
												<xsl:when test="string(@namespace)">
													<xsl:value-of select="@namespace"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="$CustomToolNamespace"/>
												</xsl:otherwise>
											</xsl:choose>
											<xsl:text>.</xsl:text>
											<xsl:value-of select="$originalClass"/>
											<xsl:text>.</xsl:text>
											<xsl:choose>
												<xsl:when test="string(@className)">
													<xsl:value-of select="@className"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="@methodName"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:attribute>
									</plx:string>
								</xsl:variable>
								<plx:callStatic name="TraceRuleStart" dataTypeName="TraceUtility" dataTypeQualifier="Neumont.Tools.Modeling.Diagnostics">
									<plx:passParam>
										<xsl:copy-of select="$methodInfo/child::*"/>
									</plx:passParam>
									<plx:passParam>
										<xsl:copy-of select="$classFullNameString"/>
									</plx:passParam>
								</plx:callStatic>
								<xsl:choose>
									<xsl:when test="$methodInRuleClass">
										<plx:callThis name="{@methodName}">
											<plx:passParam>
												<plx:nameRef name="e" type="parameter"/>
											</plx:passParam>
										</plx:callThis>
									</xsl:when>
									<xsl:otherwise>
										<plx:callStatic name="{@methodName}" dataTypeName="{$remainingName}">
											<plx:passParam>
												<plx:nameRef name="e" type="parameter"/>
											</plx:passParam>
										</plx:callStatic>
									</xsl:otherwise>
								</xsl:choose>
								<plx:callStatic name="TraceRuleEnd" dataTypeName="TraceUtility" dataTypeQualifier="Neumont.Tools.Modeling.Diagnostics">
									<plx:passParam>
										<xsl:copy-of select="$methodInfo/child::*"/>
									</plx:passParam>
									<plx:passParam>
										<xsl:copy-of select="$classFullNameString"/>
									</plx:passParam>
								</plx:callStatic>
							</plx:function>
						</plx:class>
					</xsl:for-each>
				</xsl:otherwise>
			</xsl:choose>
		</plx:class>
	</xsl:template>
	<xsl:template match="arg:AddRule" mode="RuleMethodInfo">
		<names ruleType="{local-name()}" ruleMethodName="ElementAdded" ruleEventArgsType="ElementAddedEventArgs">
			<plx:callInstance name="Store" type="property">
				<plx:callObject>
					<plx:callInstance name="ModelElement" type="property">
						<plx:callObject>
							<plx:nameRef name="e" type="parameter"/>
						</plx:callObject>
					</plx:callInstance>
				</plx:callObject>
			</plx:callInstance>
		</names>
	</xsl:template>
	<xsl:template match="arg:ChangeRule" mode="RuleMethodInfo">
		<names ruleType="{local-name()}" ruleMethodName="ElementPropertyChanged" ruleEventArgsType="ElementPropertyChangedEventArgs">
			<plx:callInstance name="Store" type="property">
				<plx:callObject>
					<plx:callInstance name="ModelElement" type="property">
						<plx:callObject>
							<plx:nameRef name="e" type="parameter"/>
						</plx:callObject>
					</plx:callInstance>
				</plx:callObject>
			</plx:callInstance>
		</names>
	</xsl:template>
	<xsl:template match="arg:DeleteRule" mode="RuleMethodInfo">
		<names ruleType="{local-name()}" ruleMethodName="ElementDeleted" ruleEventArgsType="ElementDeletedEventArgs">
			<plx:callInstance name="Store" type="property">
				<plx:callObject>
					<plx:callInstance name="ModelElement" type="property">
						<plx:callObject>
							<plx:nameRef name="e" type="parameter"/>
						</plx:callObject>
					</plx:callInstance>
				</plx:callObject>
			</plx:callInstance>
		</names>
	</xsl:template>
	<xsl:template match="arg:DeletingRule" mode="RuleMethodInfo">
		<names ruleType="{local-name()}" ruleMethodName="ElementDeleting" ruleEventArgsType="ElementDeletingEventArgs">
			<plx:callInstance name="Store" type="property">
				<plx:callObject>
					<plx:callInstance name="ModelElement" type="property">
						<plx:callObject>
							<plx:nameRef name="e" type="parameter"/>
						</plx:callObject>
					</plx:callInstance>
				</plx:callObject>
			</plx:callInstance>
		</names>
	</xsl:template>
	<xsl:template match="arg:MoveRule" mode="RuleMethodInfo">
		<names ruleType="{local-name()}" ruleMethodName="ElementMoved" ruleEventArgsType="ElementMovedEventArgs">
			<plx:callInstance name="Store" type="property">
				<plx:callObject>
					<plx:callInstance name="ModelElement" type="property">
						<plx:callObject>
							<plx:nameRef name="e" type="parameter"/>
						</plx:callObject>
					</plx:callInstance>
				</plx:callObject>
			</plx:callInstance>
		</names>
	</xsl:template>
	<xsl:template match="arg:RolePlayerChangeRule" mode="RuleMethodInfo">
		<names ruleType="{local-name()}" ruleMethodName="RolePlayerChanged" ruleEventArgsType="RolePlayerChangedEventArgs">
			<plx:callInstance name="Store" type="property">
				<plx:callObject>
					<plx:callInstance name="ElementLink" type="property">
						<plx:callObject>
							<plx:nameRef name="e" type="parameter"/>
						</plx:callObject>
					</plx:callInstance>
				</plx:callObject>
			</plx:callInstance>
		</names>
	</xsl:template>
	<xsl:template match="arg:RolePlayerPositionChangeRule" mode="RuleMethodInfo">
		<names ruleType="{local-name()}" ruleMethodName="RolePlayerPositionChanged" ruleEventArgsType="RolePlayerOrderChangedEventArgs">
			<plx:callInstance name="Store" type="property">
				<plx:callObject>
					<plx:callInstance name="SourceElement" type="property">
						<plx:callObject>
							<plx:nameRef name="e" type="parameter"/>
						</plx:callObject>
					</plx:callInstance>
				</plx:callObject>
			</plx:callInstance>
		</names>
	</xsl:template>
	<xsl:template match="arg:TransactionBeginningRule" mode="RuleMethodInfo">
		<names ruleType="{local-name()}" ruleMethodName="TransactionBeginning" ruleEventArgsType="TransactionBeginningEventArgs">
			<plx:callInstance name="Store" type="property">
				<plx:callObject>
					<plx:callInstance name="Transaction" type="property">
						<plx:callObject>
							<plx:nameRef name="e" type="parameter"/>
						</plx:callObject>
					</plx:callInstance>
				</plx:callObject>
			</plx:callInstance>
		</names>
	</xsl:template>
	<xsl:template match="arg:TransactionCommittingRule" mode="RuleMethodInfo">
		<names ruleType="{local-name()}" ruleMethodName="TransactionCommitting" ruleEventArgsType="TransactionCommitEventArgs">
			<plx:callInstance name="Store" type="property">
				<plx:callObject>
					<plx:callInstance name="Transaction" type="property">
						<plx:callObject>
							<plx:nameRef name="e" type="parameter"/>
						</plx:callObject>
					</plx:callInstance>
				</plx:callObject>
			</plx:callInstance>
		</names>
	</xsl:template>
	<xsl:template match="arg:TransactionRollingBackRule" mode="RuleMethodInfo">
		<names ruleType="{local-name()}" ruleMethodName="TransactionRollingBack" ruleEventArgsType="TransactionRollbackEventArgs">
			<plx:callInstance name="Store" type="property">
				<plx:callObject>
					<plx:callInstance name="Transaction" type="property">
						<plx:callObject>
							<plx:nameRef name="e" type="parameter"/>
						</plx:callObject>
					</plx:callInstance>
				</plx:callObject>
			</plx:callInstance>
		</names>
	</xsl:template>
</xsl:stylesheet>
