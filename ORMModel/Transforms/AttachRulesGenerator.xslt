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
	xmlns:arg="http://schemas.neumont.edu/ORM/SDK/AttachRulesGenerator">
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
			<plx:class name="{@class}" visibility="public" partial="true">
				<plx:leadingInfo>
					<plx:pragma type="region" data="Attach rules to {@class} model"/>
				</plx:leadingInfo>
				<plx:trailingInfo>
					<plx:pragma type="closeRegion" data="Attach rules to {@class} model"/>
				</plx:trailingInfo>
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
							<plx:comment>Given the low likelihood of this even happening, the extra overhead of synchronization would outweigh any possible gain from it.</plx:comment>
							<plx:assign>
								<plx:left>
									<plx:nameRef type="local" name="retVal"/>
								</plx:left>
								<plx:right>
									<plx:callNew dataTypeName="Type" dataTypeIsSimpleArray="true">
										<plx:arrayInitializer>
											<xsl:variable name="contextClass" select="@class"/>
											<xsl:for-each select="arg:*">
												<plx:passParam>
													<xsl:call-template name="GenerateTypeOf">
														<xsl:with-param name="className" select="@class"/>
														<xsl:with-param name="namespace" select="@namespace"/>
														<xsl:with-param name="contextClass" select="$contextClass"/>
														<xsl:with-param name="contextNamespace" select="$namespaceName"/>
													</xsl:call-template>
												</plx:passParam>
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
				<plx:function name="GetCustomDomainModelTypes" visibility="protected" modifier="override">
					<plx:leadingInfo>
						<plx:docComment>
							<summary>Generated code to attach <see cref="Microsoft.VisualStudio.Modeling.Rule"/>s to the <see cref="Microsoft.VisualStudio.Modeling.Store"/>.</summary>
							<seealso cref="Microsoft.VisualStudio.Modeling.DomainModel.GetCustomDomainModelTypes"/>
						</plx:docComment>
					</plx:leadingInfo>
					<plx:returns dataTypeName="Type" dataTypeIsSimpleArray="true"/>
					<plx:branch>
						<plx:condition>
							<plx:unaryOperator type="booleanNot">
								<plx:callStatic dataTypeName="ORMMetaModel" dataTypeQualifier="Neumont.Tools.ORM.ObjectModel" name="InitializingToolboxItems" type="property"/>
							</plx:unaryOperator>
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
			</plx:class>
		</plx:namespace>
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
</xsl:stylesheet>
