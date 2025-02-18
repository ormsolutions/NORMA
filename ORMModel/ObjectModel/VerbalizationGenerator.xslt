﻿<?xml version="1.0" encoding="utf-8"?>
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
	xmlns:cvg="http://schemas.neumont.edu/ORM/SDK/CoreVerbalizationGenerator"
	xmlns:exsl="http://exslt.org/common"
	extension-element-prefixes="exsl"
	exclude-result-prefixes="cvg">

	<!-- Indenting is useful for debugging the transform, but a waste of memory at generation time -->
	<xsl:output method="xml" encoding="utf-8" indent="no"/>
	<xsl:preserve-space elements="cvg:Snippet"/>
	<!-- Pick up param value supplied automatically by plix loader -->
	<xsl:param name="CustomToolNamespace" select="'ORMSolutions.ORMArchitect.Core.ObjectModel'"/>

	<!-- Names of the different classes we generate -->
	<xsl:param name="VerbalizationTextSnippetType" select="'CoreVerbalizationSnippetType'"/>
	<xsl:param name="CoreVerbalizationSets" select="'CoreVerbalizationSets'"/>

	<!-- Parts of variable names used in generated code. These
		 names are decorated with position numbers and allow multiple
		 spits of the same template in the same function without name
		 collision -->
	<xsl:param name="FormatVariablePart" select="'Format'"/>
	<xsl:param name="ReplaceVariablePart" select="'Replace'"/>
	<xsl:param name="RoleIterVariablePart" select="'RoleIter'"/>
	<xsl:param name="FactRoleIterVariablePart" select="'FactRoleIter'"/>
	<xsl:param name="ResolvedRoleVariablePart" select="'ResolvedRoleIndex'"/>
	<xsl:param name="ContextRoleIterVariablePart" select="'ContextRoleIter'"/>
	<xsl:param name="FactIterVariablePart" select="'FactIter'"/>
	<xsl:param name="SequenceIterVariablePart" select="'SequenceIter'"/>
	<xsl:param name="PredicateReplacementVariablePart" select="'Predicate'"/>

	<!-- Include templates to generate the shared verbalization classes -->
	<xsl:include href="VerbalizationGenerator.Sets.xslt"/>
	<xsl:template match="cvg:VerbalizationRoot">
		<plx:root>
			<plx:namespaceImport name="System"/>
			<plx:namespaceImport name="System.IO"/>
			<plx:namespaceImport name="System.Text"/>
			<plx:namespaceImport name="System.Collections.Generic"/>
			<plx:namespaceImport name="System.Globalization"/>
			<plx:namespaceImport name="Microsoft.VisualStudio.Modeling"/>
			<plx:namespace name="{$CustomToolNamespace}">
				<xsl:if test="cvg:Copyright">
					<plx:leadingInfo>
						<plx:comment blankLine="true"/>
						<plx:comment>
							<xsl:value-of select="cvg:Copyright/@name"/>
						</plx:comment>
						<xsl:for-each select="cvg:Copyright/cvg:CopyrightLine">
							<plx:comment>
								<xsl:value-of select="."/>
							</plx:comment>
						</xsl:for-each>
						<plx:comment blankLine="true"/>
					</plx:leadingInfo>
				</xsl:if>
				<!-- Generate verbalization set classes and default populations -->
				<xsl:call-template name="GenerateVerbalizationSets">
					<xsl:with-param name="SnippetEnumTypeName" select="$VerbalizationTextSnippetType"/>
					<xsl:with-param name="VerbalizationSetName" select="$CoreVerbalizationSets"/>
					<xsl:with-param name="SnippetsLocation" select="@snippetsLocation"/>
					<xsl:with-param name="SnippetsSchemaLocation" select="@snippetsSchemaLocation"/>
				</xsl:call-template>
				<!-- Generate verbalization implementations for all constructs -->
				<xsl:call-template name="GenerateVerbalizationClasses"/>
				<!-- Generate verbalization set classes and default populations -->
				<!--<xsl:call-template name="GenerateVerbalizationReportSets"/>-->
			</plx:namespace>
		</plx:root>
	</xsl:template>
	<xsl:template name="GenerateVerbalizationClasses">
		<xsl:apply-templates select="cvg:Constructs/child::*" mode="GenerateClasses"/>
	</xsl:template>
	<xsl:template match="cvg:Constraints" mode="GenerateClasses">
		<xsl:apply-templates select="cvg:Constraint" mode="ConstraintVerbalization"/>
	</xsl:template>
	<xsl:template match="cvg:SampleInstances" mode="GenerateClasses">
		<xsl:apply-templates select="cvg:SingleSnippet" mode="GenerateClasses"/>
		<xsl:apply-templates select="cvg:SampleInstance" mode="GenerateClasses"/>
	</xsl:template>
	<xsl:template match="cvg:ChildHelpers" mode="GenerateClasses">
		<xsl:apply-templates select="cvg:ChildVerbalizer" mode="GenerateClasses"/>
	</xsl:template>
	<xsl:template match="cvg:ErrorReports" mode="GenerateClasses">
		<xsl:apply-templates select="cvg:ErrorReport" mode="GenerateClasses"/>
	</xsl:template>
	<xsl:template match="cvg:NoteText" mode="ConstraintVerbalization">
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="'variableSnippet'"/>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="{$VariablePrefix}{$VariableDecorator}" type="local"/>
			</plx:left>
			<plx:right>
				<plx:callThis name="Text" type="property"/>
			</plx:right>
		</plx:assign>
	</xsl:template>
	<xsl:template match="cvg:ExpressionBody" mode="ConstraintVerbalization">
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="'variableSnippet'"/>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="{$VariablePrefix}{$VariableDecorator}" type="local"/>
			</plx:left>
			<plx:right>
				<plx:callThis name="Body" type="property"/>
			</plx:right>
		</plx:assign>
	</xsl:template>
	<xsl:template match="cvg:Note|cvg:Definition|cvg:DerivationNote" mode="GenerateClasses">
		<xsl:variable name="className" select="name()"/>
		<plx:class name="{$className}" visibility="public" partial="true">
			<plx:leadingInfo>
				<plx:pragma type="region" data="{$className} verbalization"/>
			</plx:leadingInfo>
			<plx:trailingInfo>
				<plx:pragma type="closeRegion" data="{$className} verbalization"/>
			</plx:trailingInfo>
			<plx:implementsInterface dataTypeName="IVerbalize"/>
			<plx:function name="GetVerbalization" visibility="protected">
				<plx:leadingInfo>
					<plx:docComment>
						<summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:interfaceMember memberName="GetVerbalization" dataTypeName="IVerbalize"/>
				<plx:param name="writer" dataTypeName="TextWriter"/>
				<plx:param name="snippetsDictionary" dataTypeName="IDictionary">
					<plx:passTypeParam dataTypeName="Type"/>
					<plx:passTypeParam dataTypeName="IVerbalizationSets"/>
				</plx:param>
				<plx:param name="verbalizationContext" dataTypeName="IVerbalizationContext"/>
				<plx:param name="sign" dataTypeName="VerbalizationSign"/>
				<plx:returns dataTypeName=".boolean"/>

				<plx:pragma type="region" data="Preliminary"/>
				<xsl:call-template name="DeclareIsNegative"/>
				<xsl:call-template name="DeclareSnippetsLocal"/>
				<!-- Don't proceed with verbalization if blocking errors are present -->
				<xsl:call-template name="CheckErrorConditions"/>
				<plx:local name="isDeontic" dataTypeName=".boolean" const="true">
					<plx:initialize>
						<plx:falseKeyword/>
					</plx:initialize>
				</plx:local>
				<plx:pragma type="closeRegion" data="Preliminary"/>
				<plx:pragma type="region" data="Pattern Matches"/>
				<xsl:apply-templates select="child::*" mode="ConstraintVerbalization">
					<xsl:with-param name="TopLevel" select="true()"/>
				</xsl:apply-templates>
				<plx:pragma type="closeRegion" data="Pattern Matches"/>
				<xsl:call-template name="CheckErrorConditions">
					<xsl:with-param name="Primary" select="false()"/>
					<xsl:with-param name="DeclareErrorOwner" select="false()"/>
					<xsl:with-param name="BeginVerbalization" select="true()"/>
				</xsl:call-template>
				<plx:return>
					<plx:trueKeyword/>
				</plx:return>
			</plx:function>
		</plx:class>
	</xsl:template>
	<xsl:template name="DeclareSnippetsLocal">
		<plx:local name="snippets" dataTypeName="IVerbalizationSets">
			<plx:passTypeParam dataTypeName="{$VerbalizationTextSnippetType}"/>
			<plx:initialize>
				<plx:cast dataTypeName="IVerbalizationSets">
					<plx:passTypeParam dataTypeName="{$VerbalizationTextSnippetType}"/>
					<plx:callInstance type="indexerCall" name=".implied">
						<plx:callObject>
							<plx:nameRef name="snippetsDictionary" type="parameter"/>
						</plx:callObject>
						<plx:passParam>
							<plx:typeOf dataTypeName="{$VerbalizationTextSnippetType}"/>
						</plx:passParam>
					</plx:callInstance>
				</plx:cast>
			</plx:initialize>
		</plx:local>
	</xsl:template>
	<xsl:template name="DeclareIsNegative">
		<plx:local name="isNegative" dataTypeName=".boolean">
			<plx:initialize>
				<plx:binaryOperator type="inequality">
					<plx:left>
						<plx:value data="0" type="i4"/>
					</plx:left>
					<plx:right>
						<plx:binaryOperator type="bitwiseAnd">
							<plx:left>
								<plx:nameRef name="sign" type="parameter"/>
							</plx:left>
							<plx:right>
								<plx:callStatic dataTypeName="VerbalizationSign" name="Negative" type="field"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:right>
				</plx:binaryOperator>
			</plx:initialize>
		</plx:local>
	</xsl:template>
	<xsl:template match="cvg:SingleSnippet|cvg:ChildVerbalizer" mode="GenerateClasses">
		<xsl:variable name="parentClass" select="string(@childHelperFor)"/>
		<xsl:variable name="isChildHelper" select="boolean($parentClass)"/>
		<xsl:variable name="snippetRef" select="string(@snippetRef)"/>
		<xsl:if test="$isChildHelper">
			<xsl:text disable-output-escaping="yes"><![CDATA[<plx:class name="]]></xsl:text>
			<xsl:value-of select="$parentClass"/>
			<xsl:text disable-output-escaping="yes"><![CDATA[" visibility="public" partial="true"><plx:leadingInfo><plx:pragma type="region" data="]]></xsl:text>
			<xsl:value-of select="concat($parentClass,'.',@type)"/>
			<xsl:text disable-output-escaping="yes"><![CDATA[ verbalization"/></plx:leadingInfo><plx:trailingInfo><plx:pragma type="closeRegion" data="]]></xsl:text>
			<xsl:value-of select="concat($parentClass,'.',@type)"/>
			<xsl:text disable-output-escaping="yes"><![CDATA[ verbalization"/></plx:trailingInfo>]]></xsl:text>
		</xsl:if>
		<plx:class name="{@type}" visibility="deferToPartial" partial="true">
			<plx:leadingInfo>
				<plx:pragma type="region" data="Child Verbalizer {@type}"/>
			</plx:leadingInfo>
			<plx:trailingInfo>
				<plx:pragma type="closeRegion" data="Child Verbalizerion {@type}"/>
			</plx:trailingInfo>
			<plx:implementsInterface dataTypeName="IVerbalize"/>
			<xsl:call-template name="GetVerbalizer">
				<xsl:with-param name="type" select="@type"/>
				<xsl:with-param name="useDisposeHelper" select="not(self::cvg:SingleSnippet)"/>
			</xsl:call-template>
			<plx:function name="GetVerbalization" visibility="protected">
				<plx:leadingInfo>
					<plx:docComment>
						<summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:interfaceMember memberName="GetVerbalization" dataTypeName="IVerbalize"/>
				<plx:param name="writer" dataTypeName="TextWriter"/>
				<plx:param name="snippetsDictionary" dataTypeName="IDictionary">
					<plx:passTypeParam dataTypeName="Type"/>
					<plx:passTypeParam dataTypeName="IVerbalizationSets"/>
				</plx:param>
				<plx:param name="verbalizationContext" dataTypeName="IVerbalizationContext"/>
				<plx:param name="sign" dataTypeName="VerbalizationSign"/>
				<plx:returns dataTypeName=".boolean"/>
				<xsl:choose>
					<xsl:when test="self::cvg:SingleSnippet">
						<xsl:call-template name="DeclareIsNegative"/>
						<xsl:call-template name="DeclareSnippetsLocal"/>
						<plx:callInstance name="BeginVerbalization">
							<plx:callObject>
								<plx:nameRef name="verbalizationContext" type="parameter"/>
							</plx:callObject>
							<plx:passParam>
								<plx:callStatic name="Normal" dataTypeName="VerbalizationContent" type="field"/>
							</plx:passParam>
						</plx:callInstance>
						<plx:callInstance name="Write">
							<plx:callObject>
								<plx:nameRef name="writer" type="local"/>
							</plx:callObject>
							<plx:passParam>
								<plx:callInstance name="GetSnippet">
									<plx:callObject>
										<plx:nameRef name="snippets"/>
									</plx:callObject>
									<plx:passParam>
										<plx:callStatic name="{$snippetRef}" dataTypeName="{$VerbalizationTextSnippetType}" type="field"/>
									</plx:passParam>
									<plx:passParam>
										<plx:falseKeyword/>
									</plx:passParam>
									<plx:passParam>
										<plx:falseKeyword/>
									</plx:passParam>
								</plx:callInstance>
							</plx:passParam>
						</plx:callInstance>
						<plx:return>
							<plx:trueKeyword/>
						</plx:return>
					</xsl:when>
					<xsl:otherwise>
						<xsl:variable name="localsFragment">
							<xsl:call-template name="DeclareIsNegative"/>
							<xsl:call-template name="DeclareSnippetsLocal"/>
							<plx:local name="isDeontic" dataTypeName=".boolean" const="true">
								<plx:initialize>
									<plx:falseKeyword/>
								</plx:initialize>
							</plx:local>
							<plx:local name="sbTemp" dataTypeName="StringBuilder">
								<plx:initialize>
									<plx:nullKeyword/>
								</plx:initialize>
							</plx:local>
						</xsl:variable>
						<xsl:variable name="codeFragment">
							<plx:pragma type="region" data="Pattern Matches"/>
							<xsl:apply-templates select="child::*" mode="ConstraintVerbalization">
								<xsl:with-param name="TopLevel" select="true()"/>
							</xsl:apply-templates>
							<plx:pragma type="closeRegion" data="Pattern Matches"/>
						</xsl:variable>
						<xsl:call-template name="StripUnusedLocals">
							<xsl:with-param name="LocalCode" select="exsl:node-set($localsFragment)/child::*"/>
							<xsl:with-param name="ReferencingCode" select="exsl:node-set($codeFragment)/child::*"/>
						</xsl:call-template>
						<plx:return>
							<plx:trueKeyword/>
						</plx:return>
					</xsl:otherwise>
				</xsl:choose>
			</plx:function>
		</plx:class>
		<xsl:if test="$isChildHelper">
			<xsl:text disable-output-escaping="yes"><![CDATA[</plx:class>]]></xsl:text>
		</xsl:if>
	</xsl:template>
	<xsl:template match="cvg:SampleInstance" mode="GenerateClasses">
		<xsl:variable name="parentClass" select="string(@childHelperFor)"/>
		<xsl:variable name="isChildHelper" select="boolean($parentClass)"/>
		<xsl:if test="$isChildHelper">
			<xsl:text disable-output-escaping="yes"><![CDATA[<plx:class name="]]></xsl:text>
			<xsl:value-of select="$parentClass"/>
			<xsl:text disable-output-escaping="yes"><![CDATA[" visibility="public" partial="true"><plx:leadingInfo><plx:pragma type="region" data="]]></xsl:text>
			<xsl:value-of select="concat($parentClass,'.',@type)"/>
			<xsl:text disable-output-escaping="yes"><![CDATA[ verbalization"/></plx:leadingInfo><plx:trailingInfo><plx:pragma type="closeRegion" data="]]></xsl:text>
			<xsl:value-of select="concat($parentClass,'.',@type)"/>
			<xsl:text disable-output-escaping="yes"><![CDATA[ verbalization"/></plx:trailingInfo>]]></xsl:text>
		</xsl:if>
		<plx:class name="{@type}" visibility="private" partial="true">
			<plx:leadingInfo>
				<plx:pragma type="region" data="{$parentClass} Instance Verbalization"/>
			</plx:leadingInfo>
			<plx:trailingInfo>
				<plx:pragma type="closeRegion" data="{$parentClass} Instance Verbalization"/>
			</plx:trailingInfo>
			<plx:implementsInterface dataTypeName="IVerbalize"/>
			<xsl:call-template name="GetVerbalizer">
				<xsl:with-param name="type" select="@type"/>
			</xsl:call-template>
			<plx:function name="GetVerbalization" visibility="protected">
				<plx:leadingInfo>
					<plx:docComment>
						<summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:interfaceMember memberName="GetVerbalization" dataTypeName="IVerbalize"/>
				<plx:param name="writer" dataTypeName="TextWriter"/>
				<plx:param name="snippetsDictionary" dataTypeName="IDictionary">
					<plx:passTypeParam dataTypeName="Type"/>
					<plx:passTypeParam dataTypeName="IVerbalizationSets"/>
				</plx:param>
				<plx:param name="verbalizationContext" dataTypeName="IVerbalizationContext"/>
				<plx:param name="sign" dataTypeName="VerbalizationSign"/>
				<plx:returns dataTypeName=".boolean"/>

				<plx:pragma type="region" data="Preliminary"/>
				<!-- Verbalizing a fact type is a simple case of verbalizing a constraint.
					 Leverage the code snippets we use for constraints by setting the right
					 variable names and calling the constraint verbalization templates -->
				<xsl:call-template name="DeclareIsNegative"/>
				<xsl:call-template name="DeclareSnippetsLocal"/>
				<!-- Don't proceed with verbalization if blocking errors are present -->
				<xsl:call-template name="CheckErrorConditions"/>
				<xsl:if test="$parentClass='FactType'">
					<plx:local name="parentFact" dataTypeName="FactType">
						<plx:initialize>
							<plx:callThis name="FactType" type="property" />
						</plx:initialize>
					</plx:local>
					<plx:local name="allReadingOrders" dataTypeName="LinkedElementCollection">
						<plx:passTypeParam dataTypeName="ReadingOrder"/>
						<plx:initialize>
							<plx:callInstance name="ReadingOrderCollection" type="property">
								<plx:callObject>
									<plx:nameRef name="parentFact" type="local" />
								</plx:callObject>
							</plx:callInstance>
						</plx:initialize>
					</plx:local>
					<plx:local name="factRoles" dataTypeName="IList">
						<plx:passTypeParam dataTypeName="RoleBase"/>
						<plx:initialize>
							<xsl:call-template name="InitializeDefaultFactRoles"/>
						</plx:initialize>
					</plx:local>
					<plx:local name="factArity" dataTypeName=".i4">
						<plx:initialize>
							<xsl:call-template name="InitializeFactArity"/>
						</plx:initialize>
					</plx:local>
					<plx:local name="isDeontic" dataTypeName=".boolean" const="true">
						<plx:initialize>
							<plx:falseKeyword/>
						</plx:initialize>
					</plx:local>
					<plx:local name="predicatePartFormatString" dataTypeName=".string">
						<plx:initialize>
							<xsl:call-template name="PopulatePredicatePartFormatString"/>
						</plx:initialize>
					</plx:local>
					<plx:local name="reading" dataTypeName="IReading"/>
					<plx:local name="hyphenBinder" dataTypeName="VerbalizationHyphenBinder"/>
					<plx:local name="instanceRoles" dataTypeName="LinkedElementCollection">
						<plx:passTypeParam dataTypeName="FactTypeRoleInstance"/>
						<plx:initialize>
							<plx:callInstance name="RoleInstanceCollection" type="property">
								<plx:callObject>
									<plx:nameRef name="Instance" type="local"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:initialize>
					</plx:local>
					<plx:local name="instanceRoleCount" dataTypeName=".i4">
						<plx:initialize>
							<plx:callInstance name="Count" type="property">
								<plx:callObject>
									<plx:nameRef name="instanceRoles" type="local"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:initialize>
					</plx:local>
					<xsl:call-template name="PopulateBasicRoleReplacements">
						<xsl:with-param name="IncludeInstanceData" select="true()"/>
					</xsl:call-template>
					<plx:local name="objectifyingInstance" dataTypeName="ObjectTypeInstance">
						<plx:initialize>
							<plx:inlineStatement dataTypeName="ObjectTypeInstance">
								<plx:conditionalOperator>
									<plx:condition>
										<plx:callThis name="DisplayIdentifier" type="property"/>
									</plx:condition>
									<plx:left>
										<plx:callInstance name="ObjectifyingInstance" type="property">
											<plx:callObject>
												<plx:callThis name="Instance" type="property"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:nullKeyword/>
									</plx:right>
								</plx:conditionalOperator>
							</plx:inlineStatement>
						</plx:initialize>
					</plx:local>
					<plx:pragma type="closeRegion" data="Preliminary"/>
					<plx:pragma type="region" data="Pattern Matches"/>
					<xsl:apply-templates select="*" mode="ConstraintVerbalization">
						<xsl:with-param name="TopLevel" select="true()"/>
					</xsl:apply-templates>
					<plx:pragma type="closeRegion" data="Pattern Matches"/>
				</xsl:if>
				<!-- End FactType Parent -->
				<!-- Check if we are iterating for ObjectType -->
				<xsl:if test="$parentClass='ObjectType'">
					<plx:pragma type="closeRegion" data="Preliminary"/>
					<plx:pragma type="region" data="Pattern Matches"/>
					<xsl:apply-templates select="child::*" mode="ConstraintVerbalization">
						<xsl:with-param name="TopLevel" select="true()"/>
					</xsl:apply-templates>
					<plx:pragma type="closeRegion" data="Pattern Matches"/>
				</xsl:if>
				<!-- End check if we are iterating for ObjectType -->
				<xsl:call-template name="CheckErrorConditions">
					<xsl:with-param name="Primary" select="false()"/>
					<xsl:with-param name="DeclareErrorOwner" select="false()"/>
					<xsl:with-param name="BeginVerbalization" select="true()"/>
				</xsl:call-template>
				<plx:return>
					<plx:trueKeyword/>
				</plx:return>
			</plx:function>
		</plx:class>
		<xsl:if test="$isChildHelper">
			<xsl:text disable-output-escaping="yes"><![CDATA[</plx:class>]]></xsl:text>
		</xsl:if>
	</xsl:template>
	<xsl:template match="cvg:FactType" mode="GenerateClasses">
		<plx:class name="FactType" visibility="public" partial="true">
			<plx:leadingInfo>
				<plx:pragma type="region" data="FactType verbalization"/>
			</plx:leadingInfo>
			<plx:trailingInfo>
				<plx:pragma type="closeRegion" data="FactType verbalization"/>
			</plx:trailingInfo>
			<plx:implementsInterface dataTypeName="IVerbalize"/>
			<plx:function name="GetVerbalization" visibility="protected">
				<plx:leadingInfo>
					<plx:docComment>
						<summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:interfaceMember memberName="GetVerbalization" dataTypeName="IVerbalize"/>
				<plx:param name="writer" dataTypeName="TextWriter"/>
				<plx:param name="snippetsDictionary" dataTypeName="IDictionary">
					<plx:passTypeParam dataTypeName="Type"/>
					<plx:passTypeParam dataTypeName="IVerbalizationSets"/>
				</plx:param>
				<plx:param name="verbalizationContext" dataTypeName="IVerbalizationContext"/>
				<plx:param name="sign" dataTypeName="VerbalizationSign"/>
				<plx:returns dataTypeName=".boolean"/>

				<!-- Verbalizing a fact type is a simple case of verbalizing a constraint.
					 Leverage the code snippets we use for constraints by setting the right
					 variable names and calling the constraint verbalization templates -->
				<plx:pragma type="region" data="Preliminary"/>
				<xsl:call-template name="DeclareIsNegative"/>
				<xsl:call-template name="DeclareSnippetsLocal"/>
				<!-- Don't proceed with verbalization if blocking errors are present -->
				<xsl:call-template name="CheckErrorConditions"/>
				<plx:local name="allReadingOrders" dataTypeName="LinkedElementCollection">
					<plx:passTypeParam dataTypeName="ReadingOrder"/>
					<plx:initialize>
						<plx:callThis name="ReadingOrderCollection" type="property"/>
					</plx:initialize>
				</plx:local>
				<plx:local name="factRoles" dataTypeName="IList">
					<plx:passTypeParam dataTypeName="RoleBase"/>
					<plx:initialize>
						<xsl:call-template name="InitializeDefaultFactRoles">
							<xsl:with-param name="FallbackRoleCollection">
								<plx:callThis name="RoleCollection" type="property"/>
							</xsl:with-param>
						</xsl:call-template>
					</plx:initialize>
				</plx:local>
				<plx:local name="factArity" dataTypeName=".i4">
					<plx:initialize>
						<xsl:call-template name="InitializeFactArity"/>
					</plx:initialize>
				</plx:local>
				<plx:local name="parentFact" dataTypeName="FactType">
					<plx:initialize>
						<plx:thisKeyword/>
					</plx:initialize>
				</plx:local>
				<plx:local name="isDeontic" dataTypeName=".boolean" const="true">
					<plx:initialize>
						<plx:falseKeyword/>
					</plx:initialize>
				</plx:local>
				<plx:local name="predicatePartFormatString" dataTypeName=".string">
					<plx:initialize>
						<xsl:call-template name="PopulatePredicatePartFormatString"/>
					</plx:initialize>
				</plx:local>
				<plx:local name="reading" dataTypeName="IReading"/>
				<plx:local name="hyphenBinder" dataTypeName="VerbalizationHyphenBinder"/>
				<plx:pragma type="closeRegion" data="Preliminary"/>
				<plx:pragma type="region" data="Pattern Matches"/>
				<plx:comment>UNDONE: RolePathVerbalizerPending Introduce snippet-integrated role path helper patterns</plx:comment>
				<plx:comment>to the verbalization generator instead of hand-coding derivation rules</plx:comment>
				<plx:local name="derivationRule" dataTypeName="FactTypeDerivationRule"/>
				<plx:local name="pathVerbalizer" dataTypeName="RolePathVerbalizer"/>
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="booleanAnd">
							<plx:left>
								<plx:binaryOperator type="identityInequality">
									<plx:left>
										<plx:inlineStatement dataTypeName="FactTypeDerivationRule">
											<plx:assign>
												<plx:left>
													<plx:nameRef name="derivationRule"/>
												</plx:left>
												<plx:right>
													<plx:cast type="testCast" dataTypeName="FactTypeDerivationRule">
														<plx:callThis name="DerivationRule" type="property"/>
													</plx:cast>
												</plx:right>
											</plx:assign>
										</plx:inlineStatement>
									</plx:left>
									<plx:right>
										<plx:nullKeyword/>
									</plx:right>
								</plx:binaryOperator>
							</plx:left>
							<plx:right>
								<plx:callInstance name="HasPathVerbalization">
									<plx:callObject>
										<plx:inlineStatement dataTypeName="RolePathVerbalizer">
											<plx:assign>
												<plx:left>
													<plx:nameRef name="pathVerbalizer"/>
												</plx:left>
												<plx:right>
													<plx:callStatic name="Create" dataTypeName="RolePathVerbalizer">
														<plx:passParam>
															<plx:nameRef name="derivationRule"/>
														</plx:passParam>
														<plx:passParam>
															<plx:callNew dataTypeName="StandardRolePathRenderer">
																<plx:passParam>
																	<plx:nameRef name="snippets"/>
																</plx:passParam>
																<plx:passParam>
																	<plx:nameRef name="verbalizationContext" type="parameter"/>
																</plx:passParam>
																<plx:passParam>
																	<plx:callInstance name="FormatProvider" type="property">
																		<plx:callObject>
																			<plx:nameRef name="writer" type="parameter"/>
																		</plx:callObject>
																	</plx:callInstance>
																</plx:passParam>
															</plx:callNew>
														</plx:passParam>
													</plx:callStatic>
												</plx:right>
											</plx:assign>
										</plx:inlineStatement>
									</plx:callObject>
									<plx:passParam>
										<plx:nameRef name="derivationRule"/>
									</plx:passParam>
								</plx:callInstance>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:assign>
						<plx:left>
							<plx:nameRef name="reading"/>
						</plx:left>
						<plx:right>
							<plx:callInstance name="GetMatchingReading">
								<plx:callObject>
									<plx:nameRef name="parentFact"/>
								</plx:callObject>
								<plx:passParam>
									<plx:nameRef name="allReadingOrders"/>
								</plx:passParam>
								<plx:passParam>
									<plx:nullKeyword/>
								</plx:passParam>
								<plx:passParam>
									<plx:callInstance name=".implied" type="indexerCall">
										<plx:callObject>
											<plx:nameRef name="factRoles"/>
										</plx:callObject>
										<plx:passParam>
											<plx:value data="0" type="i4"/>
										</plx:passParam>
									</plx:callInstance>
								</plx:passParam>
								<plx:passParam>
									<plx:nullKeyword/>
								</plx:passParam>
								<plx:passParam>
									<plx:nameRef name="factRoles"/>
								</plx:passParam>
								<plx:passParam>
									<plx:callStatic name="AllowAnyOrder" type="field" dataTypeName="MatchingReadingOptions"/>
								</plx:passParam>
							</plx:callInstance>
						</plx:right>
					</plx:assign>
					<plx:assign>
						<plx:left>
							<plx:nameRef name="hyphenBinder"/>
						</plx:left>
						<plx:right>
							<plx:callNew dataTypeName="VerbalizationHyphenBinder">
								<plx:passParam>
									<plx:nameRef name="reading"/>
								</plx:passParam>
								<plx:passParam>
									<plx:callInstance name="FormatProvider" type="property">
										<plx:callObject>
											<plx:nameRef name="writer" type="parameter"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:passParam>
								<plx:passParam>
									<plx:nameRef name="factRoles"/>
								</plx:passParam>
								<plx:passParam>
									<plx:callInstance name="GetSnippet">
										<plx:callObject>
											<plx:nameRef name="snippets"/>
										</plx:callObject>
										<plx:passParam>
											<plx:callStatic name="HyphenBoundPredicatePart" type="field" dataTypeName="CoreVerbalizationSnippetType"/>
										</plx:passParam>
										<plx:passParam>
											<plx:falseKeyword/>
										</plx:passParam>
										<plx:passParam>
											<plx:nameRef name="isNegative"/>
										</plx:passParam>
									</plx:callInstance>
								</plx:passParam>
								<plx:passParam>
									<plx:nameRef name="predicatePartFormatString"/>
								</plx:passParam>
							</plx:callNew>
						</plx:right>
					</plx:assign>
					<plx:callInstance name="BeginVerbalization">
						<plx:callObject>
							<plx:nameRef name="verbalizationContext" type="parameter"/>
						</plx:callObject>
						<plx:passParam>
							<plx:callStatic name="Normal" type="field" dataTypeName="VerbalizationContent"/>
						</plx:passParam>
					</plx:callInstance>
					<plx:callStatic name="WriteVerbalizerSentence" dataTypeName="FactType">
						<plx:passParam>
							<plx:nameRef name="writer" type="parameter"/>
						</plx:passParam>
						<plx:passParam>
							<plx:callStatic name="Format" dataTypeName=".string">
								<plx:passParam>
									<plx:callInstance name="FormatProvider" type="property">
										<plx:callObject>
											<plx:nameRef name="writer" type="parameter"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:passParam>
								<plx:passParam>
									<plx:callInstance name="GetSnippet">
										<plx:callObject>
											<plx:nameRef name="snippets"/>
										</plx:callObject>
										<plx:passParam>
											<plx:inlineStatement dataTypeName="CoreVerbalizationSnippetType">
												<plx:conditionalOperator>
													<plx:condition>
														<plx:binaryOperator type="equality">
															<plx:left>
																<plx:callInstance name="DerivationCompleteness" type="property">
																	<plx:callObject>
																		<plx:nameRef name="derivationRule"/>
																	</plx:callObject>
																</plx:callInstance>
															</plx:left>
															<plx:right>
																<plx:callStatic name="FullyDerived" type="field" dataTypeName="DerivationCompleteness"/>
															</plx:right>
														</plx:binaryOperator>
													</plx:condition>
													<plx:left>
														<plx:inlineStatement dataTypeName="CoreVerbalizationSnippetType">
															<plx:conditionalOperator>
																<plx:condition>
																	<plx:binaryOperator type="equality">
																		<plx:left>
																			<plx:callInstance name="DerivationStorage" type="property">
																				<plx:callObject>
																					<plx:nameRef name="derivationRule"/>
																				</plx:callObject>
																			</plx:callInstance>
																		</plx:left>
																		<plx:right>
																			<plx:callStatic name="Stored" type="field" dataTypeName="DerivationStorage"/>
																		</plx:right>
																	</plx:binaryOperator>
																</plx:condition>
																<plx:left>
																	<plx:callStatic name="FullFactTypeStoredDerivation" type="field" dataTypeName="CoreVerbalizationSnippetType"/>
																</plx:left>
																<plx:right>
																	<plx:callStatic name="FullFactTypeDerivation" type="field" dataTypeName="CoreVerbalizationSnippetType"/>
																</plx:right>
															</plx:conditionalOperator>
														</plx:inlineStatement>
													</plx:left>
													<plx:right>
														<plx:inlineStatement dataTypeName="CoreVerbalizationSnippetType">
															<plx:conditionalOperator>
																<plx:condition>
																	<plx:binaryOperator type="equality">
																		<plx:left>
																			<plx:callInstance name="DerivationStorage" type="property">
																				<plx:callObject>
																					<plx:nameRef name="derivationRule"/>
																				</plx:callObject>
																			</plx:callInstance>
																		</plx:left>
																		<plx:right>
																			<plx:callStatic name="Stored" type="field" dataTypeName="DerivationStorage"/>
																		</plx:right>
																	</plx:binaryOperator>
																</plx:condition>
																<plx:left>
																	<plx:callStatic name="PartialFactTypeStoredDerivation" type="field" dataTypeName="CoreVerbalizationSnippetType"/>
																</plx:left>
																<plx:right>
																	<plx:callStatic name="PartialFactTypeDerivation" type="field" dataTypeName="CoreVerbalizationSnippetType"/>
																</plx:right>
															</plx:conditionalOperator>
														</plx:inlineStatement>
													</plx:right>
												</plx:conditionalOperator>
											</plx:inlineStatement>
										</plx:passParam>
										<plx:passParam>
											<plx:falseKeyword/>
										</plx:passParam>
										<plx:passParam>
											<plx:falseKeyword/>
										</plx:passParam>
									</plx:callInstance>
								</plx:passParam>
								<plx:passParamArray dataTypeName=".object" dataTypeIsSimpleArray="true">
									<plx:passParam>
										<plx:callInstance name="PopulatePredicateText">
											<plx:callObject>
												<plx:nameRef name="hyphenBinder"/>
											</plx:callObject>
											<plx:passParam>
												<plx:nameRef name="reading"/>
											</plx:passParam>
											<plx:passParam>
												<plx:callInstance name="FormatProvider" type="property">
													<plx:callObject>
														<plx:nameRef name="writer" type="parameter"/>
													</plx:callObject>
												</plx:callInstance>
											</plx:passParam>
											<plx:passParam>
												<plx:nameRef name="predicatePartFormatString"/>
											</plx:passParam>
											<plx:passParam>
												<plx:nameRef name="factRoles"/>
											</plx:passParam>
											<plx:passParam>
												<plx:anonymousFunction>
													<plx:param name="replaceRoleIndex" dataTypeName=".i4"/>
													<plx:param name="replaceRole" dataTypeName="RoleBase"/>
													<plx:param name="hyphenBindingFormatString" dataTypeName=".string"/>
													<plx:returns dataTypeName=".string"/>
													<plx:return>
														<plx:callInstance name="RenderAssociatedRolePlayer">
															<plx:callObject>
																<plx:nameRef name="pathVerbalizer"/>
															</plx:callObject>
															<plx:passParam>
																<plx:nameRef name="replaceRole" type="parameter"/>
															</plx:passParam>
															<plx:passParam>
																<plx:nameRef name="hyphenBindingFormatString" type="parameter"/>
															</plx:passParam>
															<plx:passParam>
																<plx:callStatic dataTypeName="RolePathRolePlayerRenderingOptions" name="UsedInVerbalizationHead" type="field"/>
															</plx:passParam>
														</plx:callInstance>
													</plx:return>
												</plx:anonymousFunction>
											</plx:passParam>
										</plx:callInstance>
									</plx:passParam>
									<plx:passParam>
										<plx:callInstance name="RenderPathVerbalization">
											<plx:callObject>
												<plx:nameRef name="pathVerbalizer"/>
											</plx:callObject>
											<plx:passParam>
												<plx:nameRef name="derivationRule"/>
											</plx:passParam>
											<plx:passParam>
												<plx:nullKeyword/>
											</plx:passParam>
										</plx:callInstance>
									</plx:passParam>
								</plx:passParamArray>
							</plx:callStatic>
						</plx:passParam>
						<plx:passParam>
							<plx:callInstance name="GetSnippet">
								<plx:callObject>
									<plx:nameRef name="snippets"/>
								</plx:callObject>
								<plx:passParam>
									<plx:callStatic name="CloseVerbalizationSentence" type="field" dataTypeName="CoreVerbalizationSnippetType"/>
								</plx:passParam>
								<plx:passParam>
									<plx:falseKeyword/>
								</plx:passParam>
								<plx:passParam>
									<plx:nameRef name="isNegative"/>
								</plx:passParam>
							</plx:callInstance>
						</plx:passParam>
					</plx:callStatic>
				</plx:branch>
				<plx:fallbackBranch>
					<!-- Use this variation to subscript all ring fact types.
					This was the initial design to match role verbalizations that refer back to the role names
					in the fact type reading, but we don't want to subscript for all cases to handle the occasional
					back reference by a role verbalization -->
					<!--
					<xsl:variable name="subscriptConditionsFragment">
						<plx:trueKeyword/>
					</xsl:variable>
					<xsl:call-template name="PopulateBasicRoleReplacements">
						<xsl:with-param name="CustomSubscripts" select="true()"/>
						<xsl:with-param name="SubscriptConditions" select="exsl:node-set($subscriptConditionsFragment)/child::*"/>
					</xsl:call-template>
					<xsl:variable name="factMockup">
						<cvg:Fact useSubscripter="1"/>
					</xsl:variable>
					-->
					<xsl:call-template name="PopulateBasicRoleReplacements"/>
					<xsl:variable name="factMockup">
						<cvg:Fact/>
					</xsl:variable>
					<xsl:apply-templates select="exsl:node-set($factMockup)/child::*" mode="ConstraintVerbalization">
						<xsl:with-param name="TopLevel" select="true()"/>
					</xsl:apply-templates>
				</plx:fallbackBranch>
				<xsl:call-template name="CheckErrorConditions">
					<xsl:with-param name="Primary" select="false()"/>
					<xsl:with-param name="DeclareErrorOwner" select="false()"/>
					<xsl:with-param name="BeginVerbalization" select="true()"/>
				</xsl:call-template>
				<!-- Pick up other associated phrases associated with the fact type. -->
				<xsl:apply-templates select="child::cvg:*" mode="ConstraintVerbalization">
					<xsl:with-param name="TopLevel" select="true()"/>
					<!-- Verbalize as secondary snippets. Verbalization has already begun. -->
					<xsl:with-param name="IsSecondary" select="true()"/>
				</xsl:apply-templates>
				<plx:pragma type="closeRegion" data="Pattern Matches"/>
				<plx:return>
					<plx:trueKeyword/>
				</plx:return>
			</plx:function>
		</plx:class>
	</xsl:template>
	<xsl:template match="cvg:SubtypeFact" mode="GenerateClasses">
		<plx:class name="SubtypeFact" visibility="public" partial="true">
			<plx:leadingInfo>
				<plx:pragma type="region" data="SubtypeFact verbalization"/>
			</plx:leadingInfo>
			<plx:trailingInfo>
				<plx:pragma type="closeRegion" data="SubtypeFact verbalization"/>
			</plx:trailingInfo>
			<plx:implementsInterface dataTypeName="IVerbalize"/>
			<plx:function name="GetVerbalization" visibility="protected" replacesName="true">
				<plx:leadingInfo>
					<plx:docComment>
						<summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:interfaceMember memberName="GetVerbalization" dataTypeName="IVerbalize"/>
				<plx:param name="writer" dataTypeName="TextWriter"/>
				<plx:param name="snippetsDictionary" dataTypeName="IDictionary">
					<plx:passTypeParam dataTypeName="Type"/>
					<plx:passTypeParam dataTypeName="IVerbalizationSets"/>
				</plx:param>
				<plx:param name="verbalizationContext" dataTypeName="IVerbalizationContext"/>
				<plx:param name="sign" dataTypeName="VerbalizationSign"/>
				<plx:returns dataTypeName=".boolean"/>

				<plx:pragma type="region" data="Preliminary"/>
				<xsl:call-template name="DeclareIsNegative"/>
				<xsl:call-template name="DeclareSnippetsLocal"/>
				<!-- Don't proceed with verbalization if blocking errors are present -->
				<xsl:call-template name="CheckErrorConditions"/>
				<plx:local name="isDeontic" dataTypeName=".boolean" const="true">
					<plx:initialize>
						<plx:falseKeyword/>
					</plx:initialize>
				</plx:local>
				<plx:pragma type="closeRegion" data="Preliminary"/>
				<plx:pragma type="region" data="Pattern Matches"/>
				<xsl:apply-templates select="child::cvg:*" mode="ConstraintVerbalization">
					<xsl:with-param name="TopLevel" select="true()"/>
				</xsl:apply-templates>
				<plx:pragma type="closeRegion" data="Pattern Matches"/>
				<xsl:call-template name="CheckErrorConditions">
					<xsl:with-param name="Primary" select="false()"/>
					<xsl:with-param name="DeclareErrorOwner" select="false()"/>
					<xsl:with-param name="BeginVerbalization" select="true()"/>
				</xsl:call-template>
				<plx:return>
					<plx:trueKeyword/>
				</plx:return>
			</plx:function>
		</plx:class>
	</xsl:template>
	<xsl:template match="cvg:Role" mode="GenerateClasses">
		<plx:class name="Role" visibility="public" partial="true">
			<plx:leadingInfo>
				<plx:pragma type="region" data="Role verbalization"/>
			</plx:leadingInfo>
			<plx:trailingInfo>
				<plx:pragma type="closeRegion" data="Role verbalization"/>
			</plx:trailingInfo>
			<plx:implementsInterface dataTypeName="IVerbalize"/>
			<plx:function name="GetVerbalization" visibility="protected">
				<plx:leadingInfo>
					<plx:docComment>
						<summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:interfaceMember memberName="GetVerbalization" dataTypeName="IVerbalize"/>
				<plx:param name="writer" dataTypeName="TextWriter"/>
				<plx:param name="snippetsDictionary" dataTypeName="IDictionary">
					<plx:passTypeParam dataTypeName="Type"/>
					<plx:passTypeParam dataTypeName="IVerbalizationSets"/>
				</plx:param>
				<plx:param name="verbalizationContext" dataTypeName="IVerbalizationContext"/>
				<plx:param name="sign" dataTypeName="VerbalizationSign"/>
				<plx:returns dataTypeName=".boolean"/>

				<plx:pragma type="region" data="Preliminary"/>
				<xsl:call-template name="DeclareIsNegative"/>
				<xsl:call-template name="DeclareSnippetsLocal"/>
				<plx:local name="isDeontic" dataTypeName=".boolean" const="true">
					<plx:initialize>
						<plx:falseKeyword/>
					</plx:initialize>
				</plx:local>
				<plx:local name="roleName" dataTypeName=".string">
					<plx:initialize>
						<plx:callThis name="Name" type="property"/>
					</plx:initialize>
				</plx:local>
				<plx:local name="rolePlayer" dataTypeName="ObjectType">
					<plx:initialize>
						<plx:callThis name="RolePlayer" type="property"/>
					</plx:initialize>
				</plx:local>
				<plx:local name="subscript" dataTypeName=".i4">
					<plx:initialize>
						<plx:value data="0" type="i4"/>
					</plx:initialize>
				</plx:local>
				<plx:local name="defaultValue" dataTypeName=".string">
					<plx:initialize>
						<plx:callThis name="ResolvedDefaultValue" type="property"/>
					</plx:initialize>
				</plx:local>
				<plx:local name="isText" dataTypeName=".boolean">
					<plx:initialize>
						<plx:binaryOperator type="booleanAnd">
							<plx:left>
								<plx:binaryOperator type="booleanAnd">
									<plx:left>
										<plx:binaryOperator type="identityInequality">
											<plx:left>
												<plx:nameRef name="defaultValue"/>
											</plx:left>
											<plx:right>
												<plx:nullKeyword/>
											</plx:right>
										</plx:binaryOperator>
									</plx:left>
									<plx:right>
										<plx:binaryOperator type="identityEquality">
											<plx:left>
												<plx:callThis name="DefaultValueValueTypeDetachedError" type="property"/>
											</plx:left>
											<plx:right>
												<plx:nullKeyword/>
											</plx:right>
										</plx:binaryOperator>
									</plx:right>
								</plx:binaryOperator>
							</plx:left>
							<plx:right>
								<plx:binaryOperator type="typeEquality">
									<plx:left>
										<plx:callInstance name="SingleValueDataType" type="property">
											<plx:callObject>
												<plx:nameRef name="rolePlayer"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:directTypeReference dataTypeName="TextDataType"/>
									</plx:right>
								</plx:binaryOperator>
							</plx:right>
						</plx:binaryOperator>
					</plx:initialize>
				</plx:local>
				<plx:local name="orderedRoles" dataTypeName="IList">
					<plx:passTypeParam dataTypeName="RoleBase"/>
					<plx:initialize>
						<plx:callInstance name="RoleCollection" type="property">
							<plx:callObject>
								<plx:callInstance name="GetDefaultReading">
									<plx:callObject>
										<plx:callThis name="FactType" type="property"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:callObject>
						</plx:callInstance>
					</plx:initialize>
				</plx:local>
				<plx:local name="roleCount" dataTypeName=".i4">
					<plx:initialize>
						<plx:callInstance name="Count" type="property">
							<plx:callObject>
								<plx:nameRef name="orderedRoles"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:initialize>
				</plx:local>
				<plx:local name="pastMatch" dataTypeName=".boolean">
					<plx:initialize>
						<plx:falseKeyword/>
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
								<plx:nameRef name="roleCount"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:beforeLoop>
						<plx:increment>
							<plx:nameRef name="i"/>
						</plx:increment>
					</plx:beforeLoop>
					<plx:local name="testRole" dataTypeName="Role"/>
					<plx:branch>
						<plx:condition>
							<plx:binaryOperator type="identityInequality">
								<plx:left>
									<plx:nullKeyword/>
								</plx:left>
								<plx:right>
									<plx:inlineStatement dataTypeName="Role">
										<plx:assign>
											<plx:left>
												<plx:nameRef name="testRole"/>
											</plx:left>
											<plx:right>
												<plx:cast type="testCast" dataTypeName="Role">
													<plx:callInstance name=".implied" type="indexerCall">
														<plx:callObject>
															<plx:nameRef name="orderedRoles"/>
														</plx:callObject>
														<plx:passParam>
															<plx:nameRef name="i"/>
														</plx:passParam>
													</plx:callInstance>
												</plx:cast>
											</plx:right>
										</plx:assign>
									</plx:inlineStatement>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:branch>
							<plx:condition>
								<plx:binaryOperator type="identityEquality">
									<plx:left>
										<plx:cast type="exceptionCast" dataTypeName=".object">
											<plx:nameRef name="testRole"/>
										</plx:cast>
									</plx:left>
									<plx:right>
										<plx:thisKeyword/>
									</plx:right>
								</plx:binaryOperator>
							</plx:condition>
							<plx:branch>
								<plx:condition>
									<plx:binaryOperator type="booleanOr">
										<plx:left>
											<plx:binaryOperator type="identityEquality">
												<plx:left>
													<plx:nameRef name="rolePlayer"/>
												</plx:left>
												<plx:right>
													<plx:nullKeyword/>
												</plx:right>
											</plx:binaryOperator>
										</plx:left>
										<plx:right>
											<plx:binaryOperator type="inequality">
												<plx:left>
													<plx:nameRef name="subscript"/>
												</plx:left>
												<plx:right>
													<plx:value data="0" type="i4"/>
												</plx:right>
											</plx:binaryOperator>
										</plx:right>
									</plx:binaryOperator>
								</plx:condition>
								<plx:increment>
									<plx:nameRef name="subscript"/>
								</plx:increment>
								<plx:break/>
							</plx:branch>
							<plx:fallbackBranch>
								<plx:assign>
									<plx:left>
										<plx:nameRef name="pastMatch"/>
									</plx:left>
									<plx:right>
										<plx:trueKeyword/>
									</plx:right>
								</plx:assign>
							</plx:fallbackBranch>
						</plx:branch>
						<plx:fallbackBranch>
							<plx:local name="testRolePlayer" dataTypeName="ObjectType">
								<plx:initialize>
									<plx:callInstance name="RolePlayer" type="property">
										<plx:callObject>
											<plx:nameRef name="testRole"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:initialize>
							</plx:local>
							<plx:branch>
								<plx:condition>
									<plx:binaryOperator type="identityEquality">
										<plx:left>
											<plx:cast type="exceptionCast" dataTypeName=".object">
												<plx:nameRef name="testRolePlayer"/>
											</plx:cast>
										</plx:left>
										<plx:right>
											<plx:nameRef name="rolePlayer"/>
										</plx:right>
									</plx:binaryOperator>
								</plx:condition>
								<plx:increment>
									<plx:nameRef name="subscript"/>
								</plx:increment>
								<plx:branch>
									<plx:condition>
										<plx:binaryOperator type="booleanAnd">
											<plx:left>
												<plx:nameRef name="pastMatch"/>
											</plx:left>
											<plx:right>
												<plx:binaryOperator type="identityInequality">
													<plx:left>
														<plx:nameRef name="rolePlayer"/>
													</plx:left>
													<plx:right>
														<plx:nullKeyword/>
													</plx:right>
												</plx:binaryOperator>
											</plx:right>
										</plx:binaryOperator>
									</plx:condition>
									<plx:comment>The subscript will be 1</plx:comment>
									<plx:break/>
								</plx:branch>
							</plx:branch>
						</plx:fallbackBranch>
					</plx:branch>
				</plx:loop>
				<plx:pragma type="closeRegion" data="Preliminary"/>
				<plx:pragma type="region" data="Pattern Matches"/>
				<xsl:apply-templates select="*" mode="ConstraintVerbalization">
					<xsl:with-param name="TopLevel" select="true()"/>
				</xsl:apply-templates>
				<plx:pragma type="closeRegion" data="Pattern Matches"/>
				<plx:return>
					<plx:trueKeyword/>
				</plx:return>
			</plx:function>
		</plx:class>
	</xsl:template>
	<xsl:template match="cvg:ORMModel" mode="GenerateClasses">
		<plx:class name="ORMModel" visibility="public" partial="true">
			<plx:leadingInfo>
				<plx:pragma type="region" data="ORMModel verbalization"/>
			</plx:leadingInfo>
			<plx:trailingInfo>
				<plx:pragma type="closeRegion" data="ORMModel verbalization"/>
			</plx:trailingInfo>
			<plx:implementsInterface dataTypeName="IVerbalize"/>
			<plx:function name="GetVerbalization" visibility="protected">
				<plx:leadingInfo>
					<plx:docComment>
						<summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:interfaceMember memberName="GetVerbalization" dataTypeName="IVerbalize"/>
				<plx:param name="writer" dataTypeName="TextWriter"/>
				<plx:param name="snippetsDictionary" dataTypeName="IDictionary">
					<plx:passTypeParam dataTypeName="Type"/>
					<plx:passTypeParam dataTypeName="IVerbalizationSets"/>
				</plx:param>
				<plx:param name="verbalizationContext" dataTypeName="IVerbalizationContext"/>
				<plx:param name="sign" dataTypeName="VerbalizationSign"/>
				<plx:returns dataTypeName=".boolean"/>

				<plx:pragma type="region" data="Preliminary"/>
				<xsl:call-template name="DeclareIsNegative"/>
				<xsl:call-template name="DeclareSnippetsLocal"/>
				<!-- Don't proceed with verbalization if blocking errors are present -->
				<xsl:call-template name="CheckErrorConditions"/>
				<plx:local name="isDeontic" dataTypeName=".boolean" const="true">
					<plx:initialize>
						<plx:falseKeyword/>
					</plx:initialize>
				</plx:local>
				<plx:pragma type="closeRegion" data="Preliminary"/>
				<plx:pragma type="region" data="Pattern Matches"/>
				<xsl:apply-templates select="*" mode="ConstraintVerbalization">
					<xsl:with-param name="TopLevel" select="true()"/>
				</xsl:apply-templates>
				<plx:pragma type="closeRegion" data="Pattern Matches"/>
				<xsl:call-template name="CheckErrorConditions">
					<xsl:with-param name="Primary" select="false()"/>
					<xsl:with-param name="DeclareErrorOwner" select="false()"/>
					<xsl:with-param name="BeginVerbalization" select="true()"/>
				</xsl:call-template>
				<plx:return>
					<plx:trueKeyword/>
				</plx:return>
			</plx:function>
		</plx:class>
	</xsl:template>
	<xsl:template match="cvg:ORMModel" mode="GenerateClasses">
		<plx:class name="ORMModel" visibility="public" partial="true">
			<plx:leadingInfo>
				<plx:pragma type="region" data="ORMModel verbalization"/>
			</plx:leadingInfo>
			<plx:trailingInfo>
				<plx:pragma type="closeRegion" data="ORMModel verbalization"/>
			</plx:trailingInfo>
			<plx:implementsInterface dataTypeName="IVerbalize"/>
			<plx:function name="GetVerbalization" visibility="protected">
				<plx:leadingInfo>
					<plx:docComment>
						<summary>
							<see cref="IVerbalize.GetVerbalization"/> implementation
						</summary>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:interfaceMember memberName="GetVerbalization" dataTypeName="IVerbalize"/>
				<plx:param name="writer" dataTypeName="TextWriter"/>
				<plx:param name="snippetsDictionary" dataTypeName="IDictionary">
					<plx:passTypeParam dataTypeName="Type"/>
					<plx:passTypeParam dataTypeName="IVerbalizationSets"/>
				</plx:param>
				<plx:param name="verbalizationContext" dataTypeName="IVerbalizationContext"/>
				<plx:param name="sign" dataTypeName="VerbalizationSign"/>
				<plx:returns dataTypeName=".boolean"/>

				<plx:pragma type="region" data="Preliminary"/>
				<xsl:call-template name="DeclareIsNegative"/>
				<xsl:call-template name="DeclareSnippetsLocal"/>
				<!-- Don't proceed with verbalization if blocking errors are present -->
				<xsl:call-template name="CheckErrorConditions"/>
				<plx:local name="isDeontic" dataTypeName=".boolean" const="true">
					<plx:initialize>
						<plx:falseKeyword/>
					</plx:initialize>
				</plx:local>
				<plx:pragma type="closeRegion" data="Preliminary"/>
				<plx:pragma type="region" data="Pattern Matches"/>
				<xsl:apply-templates select="*" mode="ConstraintVerbalization">
					<xsl:with-param name="TopLevel" select="true()"/>
				</xsl:apply-templates>
				<plx:pragma type="closeRegion" data="Pattern Matches"/>
				<xsl:call-template name="CheckErrorConditions">
					<xsl:with-param name="Primary" select="false()"/>
					<xsl:with-param name="DeclareErrorOwner" select="false()"/>
					<xsl:with-param name="BeginVerbalization" select="true()"/>
				</xsl:call-template>
				<plx:return>
					<plx:trueKeyword/>
				</plx:return>
			</plx:function>
		</plx:class>
	</xsl:template>
	<xsl:template match="cvg:DynamicRule" mode="GenerateClasses">
		<plx:class name="{name()}" partial="true" visibility="public">
			<plx:leadingInfo>
				<plx:pragma type="region" data="{name()} verbalization"/>
			</plx:leadingInfo>
			<plx:trailingInfo>
				<plx:pragma type="closeRegion" data="{name()} verbalization"/>
			</plx:trailingInfo>
			<plx:implementsInterface dataTypeName="IVerbalize"/>
			<plx:function name="GetVerbalization" visibility="private">
				<plx:leadingInfo>
					<plx:docComment>
						<summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:interfaceMember memberName="GetVerbalization" dataTypeName="IVerbalize"/>
				<plx:param name="writer" dataTypeName="TextWriter"/>
				<plx:param name="snippetsDictionary" dataTypeName="IDictionary">
					<plx:passTypeParam dataTypeName="Type"/>
					<plx:passTypeParam dataTypeName="IVerbalizationSets"/>
				</plx:param>
				<plx:param name="verbalizationContext" dataTypeName="IVerbalizationContext"/>
				<plx:param name="sign" dataTypeName="VerbalizationSign"/>
				<plx:returns dataTypeName=".boolean"/>

				<plx:pragma type="region" data="Preliminary"/>
				<xsl:call-template name="DeclareIsNegative"/>
				<xsl:call-template name="DeclareSnippetsLocal"/>
				<!-- Don't proceed with verbalization if blocking errors are present -->
				<xsl:call-template name="CheckErrorConditions"/>
				<plx:local dataTypeName="StringBuilder" name="sbTemp">
					<plx:initialize>
						<plx:nullKeyword/>
					</plx:initialize>
				</plx:local>
				<plx:local name="isDeontic" dataTypeName=".boolean" const="true">
					<plx:initialize>
						<plx:falseKeyword/>
					</plx:initialize>
				</plx:local>
				<plx:local name="pathVerbalizer" dataTypeName="RolePathVerbalizer"/>
				<plx:local name="pathNode" dataTypeName="RolePathNode"/>
				<plx:local name="pathedRole" dataTypeName="PathedRole"/>
				<plx:pragma type="closeRegion" data="Preliminary"/>
				<plx:pragma type="region" data="Pattern Matches"/>
				<xsl:apply-templates select="child::*" mode="ConstraintVerbalization">
					<xsl:with-param name="TopLevel" select="true()"/>
				</xsl:apply-templates>
				<plx:pragma type="closeRegion" data="Pattern Matches"/>
				<xsl:if test="not(cvg:ErrorReportHere)">
					<xsl:call-template name="CheckErrorConditions">
						<xsl:with-param name="Primary" select="false()"/>
						<xsl:with-param name="DeclareErrorOwner" select="false()"/>
						<xsl:with-param name="BeginVerbalization" select="true()"/>
					</xsl:call-template>
				</xsl:if>
				<plx:return>
					<plx:trueKeyword/>
				</plx:return>
			</plx:function>
		</plx:class>
	</xsl:template>
	<xsl:template match="cvg:ObjectType" mode="GenerateClasses">
		<plx:class name="{name()}" partial="true" visibility="public">
			<plx:leadingInfo>
				<plx:pragma type="region" data="{name()} verbalization"/>
			</plx:leadingInfo>
			<plx:trailingInfo>
				<plx:pragma type="closeRegion" data="{name()} verbalization"/>
			</plx:trailingInfo>
			<plx:implementsInterface dataTypeName="IVerbalize"/>
			<plx:function name="GetVerbalization" visibility="protected">
				<plx:leadingInfo>
					<plx:docComment>
						<summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:interfaceMember memberName="GetVerbalization" dataTypeName="IVerbalize"/>
				<plx:param name="writer" dataTypeName="TextWriter"/>
				<plx:param name="snippetsDictionary" dataTypeName="IDictionary">
					<plx:passTypeParam dataTypeName="Type"/>
					<plx:passTypeParam dataTypeName="IVerbalizationSets"/>
				</plx:param>
				<plx:param name="verbalizationContext" dataTypeName="IVerbalizationContext"/>
				<plx:param name="sign" dataTypeName="VerbalizationSign"/>
				<plx:returns dataTypeName=".boolean"/>

				<plx:pragma type="region" data="Preliminary"/>
				<xsl:call-template name="DeclareIsNegative"/>
				<xsl:call-template name="DeclareSnippetsLocal"/>
				<!-- Don't proceed with verbalization if blocking errors are present -->
				<xsl:call-template name="CheckErrorConditions"/>
				<plx:local name="isDeontic" dataTypeName=".boolean" const="true">
					<plx:initialize>
						<plx:falseKeyword/>
					</plx:initialize>
				</plx:local>
				<plx:local name="sbTemp" dataTypeName="StringBuilder">
					<plx:initialize>
						<plx:nullKeyword/>
					</plx:initialize>
				</plx:local>

				<!-- Add some standard helper variables for related conditional patterns -->
				<plx:local name="preferredIdentifier" dataTypeName="UniquenessConstraint">
					<plx:initialize>
						<plx:callThis name="ResolvedPreferredIdentifier" type="property"/>
					</plx:initialize>
				</plx:local>
				<plx:local name="identifyingEntityType" dataTypeName="ObjectType">
					<plx:initialize>
						<plx:inlineStatement dataTypeName="LinkedElementCollection">
							<plx:passTypeParam dataTypeName="Role"/>
							<plx:conditionalOperator>
								<plx:condition>
									<plx:binaryOperator type="identityInequality">
										<plx:left>
											<plx:nameRef name="preferredIdentifier"/>
										</plx:left>
										<plx:right>
											<plx:nullKeyword/>
										</plx:right>
									</plx:binaryOperator>
								</plx:condition>
								<plx:left>
									<plx:callInstance name="PreferredIdentifierFor" type="property">
										<plx:callObject>
											<plx:nameRef name="preferredIdentifier"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:left>
								<plx:right>
									<plx:nullKeyword/>
								</plx:right>
							</plx:conditionalOperator>
						</plx:inlineStatement>
					</plx:initialize>
				</plx:local>
				<plx:local name="dataType" dataTypeName="DataType">
					<plx:initialize>
						<plx:callThis name="DataType" type="property"/>
					</plx:initialize>
				</plx:local>
				<plx:local name="isText" dataTypeName=".boolean">
					<plx:initialize>
						<plx:binaryOperator type="typeEquality">
							<plx:left>
								<plx:nameRef name="dataType"/>
							</plx:left>
							<plx:right>
								<plx:directTypeReference dataTypeName="TextDataType"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:initialize>
				</plx:local>
				<plx:local name="identifyingValueType" dataTypeName="ObjectType">
					<plx:initialize>
						<plx:inlineStatement dataTypeName="ObjectType">
							<plx:conditionalOperator>
								<plx:condition>
									<plx:binaryOperator type="identityInequality">
										<plx:left>
											<plx:nameRef name="dataType"/>
										</plx:left>
										<plx:right>
											<plx:nullKeyword/>
										</plx:right>
									</plx:binaryOperator>
								</plx:condition>
								<plx:left>
									<plx:thisKeyword/>
								</plx:left>
								<plx:right>
									<plx:inlineStatement dataTypeName="ObjectType">
										<plx:conditionalOperator>
											<plx:condition>
												<plx:binaryOperator type="identityInequality">
													<plx:left>
														<plx:nameRef name="identifyingEntityType"/>
													</plx:left>
													<plx:right>
														<plx:nullKeyword/>
													</plx:right>
												</plx:binaryOperator>
											</plx:condition>
											<plx:left>
												<plx:callInstance name="GetValueTypeForPreferredConstraint">
													<plx:callObject>
														<plx:nameRef name="identifyingEntityType"/>
													</plx:callObject>
												</plx:callInstance>
											</plx:left>
											<plx:right>
												<plx:nullKeyword/>
											</plx:right>
										</plx:conditionalOperator>
									</plx:inlineStatement>
								</plx:right>
							</plx:conditionalOperator>
						</plx:inlineStatement>
					</plx:initialize>
				</plx:local>
				<plx:local name="preferredIdentifierRoles" dataTypeName="LinkedElementCollection">
					<plx:passTypeParam dataTypeName="Role"/>
					<plx:initialize>
						<plx:inlineStatement dataTypeName="LinkedElementCollection">
							<plx:passTypeParam dataTypeName="Role"/>
							<plx:conditionalOperator>
								<plx:condition>
									<plx:binaryOperator type="identityInequality">
										<plx:left>
											<plx:nameRef name="preferredIdentifier"/>
										</plx:left>
										<plx:right>
											<plx:nullKeyword/>
										</plx:right>
									</plx:binaryOperator>
								</plx:condition>
								<plx:left>
									<plx:callInstance name="RoleCollection" type="property">
										<plx:callObject>
											<plx:nameRef name="preferredIdentifier"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:left>
								<plx:right>
									<plx:nullKeyword/>
								</plx:right>
							</plx:conditionalOperator>
						</plx:inlineStatement>
					</plx:initialize>
				</plx:local>
				<plx:local name="derivationRule" dataTypeName="SubtypeDerivationRule"/>
				<plx:local name="pathVerbalizer" dataTypeName="RolePathVerbalizer"/>
				<plx:local name="defaultValue" dataTypeName=".string">
					<plx:initialize>
						<plx:callThis name="ResolvedDefaultValue" type="property"/>
					</plx:initialize>
				</plx:local>
				<plx:pragma type="closeRegion" data="Preliminary"/>
				<plx:pragma type="region" data="Pattern Matches"/>
				<xsl:apply-templates select="child::*" mode="ConstraintVerbalization">
					<xsl:with-param name="TopLevel" select="true()"/>
				</xsl:apply-templates>
				<plx:pragma type="closeRegion" data="Pattern Matches"/>
				<xsl:if test="not(cvg:ErrorReportHere)">
					<xsl:call-template name="CheckErrorConditions">
						<xsl:with-param name="Primary" select="false()"/>
						<xsl:with-param name="DeclareErrorOwner" select="false()"/>
						<xsl:with-param name="BeginVerbalization" select="true()"/>
					</xsl:call-template>
				</xsl:if>
				<plx:return>
					<plx:trueKeyword/>
				</plx:return>
			</plx:function>
		</plx:class>
	</xsl:template>
	<xsl:template match="cvg:QueryBase" mode="GenerateClasses">
		<plx:class name="{name()}" partial="true" visibility="public">
			<plx:leadingInfo>
				<plx:pragma type="region" data="{name()} verbalization"/>
			</plx:leadingInfo>
			<plx:trailingInfo>
				<plx:pragma type="closeRegion" data="{name()} verbalization"/>
			</plx:trailingInfo>
			<plx:implementsInterface dataTypeName="IVerbalize"/>
			<plx:function name="GetVerbalization" visibility="protected" replacesName="true">
				<plx:leadingInfo>
					<plx:docComment>
						<summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:interfaceMember memberName="GetVerbalization" dataTypeName="IVerbalize"/>
				<plx:param name="writer" dataTypeName="TextWriter"/>
				<plx:param name="snippetsDictionary" dataTypeName="IDictionary">
					<plx:passTypeParam dataTypeName="Type"/>
					<plx:passTypeParam dataTypeName="IVerbalizationSets"/>
				</plx:param>
				<plx:param name="verbalizationContext" dataTypeName="IVerbalizationContext"/>
				<plx:param name="sign" dataTypeName="VerbalizationSign"/>
				<plx:returns dataTypeName=".boolean"/>

				<plx:pragma type="region" data="Preliminary"/>
				<xsl:call-template name="DeclareIsNegative"/>
				<xsl:call-template name="DeclareSnippetsLocal"/>
				<!-- Don't proceed with verbalization if blocking errors are present -->
				<xsl:call-template name="CheckErrorConditions"/>
				<plx:local name="isDeontic" dataTypeName=".boolean" const="true">
					<plx:initialize>
						<plx:falseKeyword/>
					</plx:initialize>
				</plx:local>
				<plx:local name="sbTemp" dataTypeName="StringBuilder">
					<plx:initialize>
						<plx:nullKeyword/>
					</plx:initialize>
				</plx:local>
				<plx:local name="factRoles" dataTypeName="IList">
					<plx:passTypeParam dataTypeName="RoleBase"/>
					<plx:initialize>
						<plx:callThis name="RoleCollection" type="property"/>
					</plx:initialize>
				</plx:local>
				<plx:local name="factArity" dataTypeName=".i4">
					<plx:initialize>
						<plx:callInstance name="Count" type="property">
							<plx:callObject>
								<plx:nameRef name="factRoles"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:initialize>
				</plx:local>
				<plx:local name="queryParameters" dataTypeName="IList">
					<plx:passTypeParam dataTypeName="QueryParameter"/>
					<plx:initialize>
						<plx:callThis name="ParameterCollection" type="property"/>
					</plx:initialize>
				</plx:local>
				<plx:local name="queryParameterCount" dataTypeName=".i4">
					<plx:initialize>
						<plx:callInstance name="Count" type="property">
							<plx:callObject>
								<plx:nameRef name="queryParameters"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:initialize>
				</plx:local>
				<plx:local name="derivationRule" dataTypeName="QueryDerivationRule"/>
				<plx:local name="pathVerbalizer" dataTypeName="RolePathVerbalizer"/>
				<plx:pragma type="closeRegion" data="Preliminary"/>
				<plx:pragma type="region" data="Pattern Matches"/>
				<xsl:apply-templates select="child::*" mode="ConstraintVerbalization">
					<xsl:with-param name="TopLevel" select="true()"/>
				</xsl:apply-templates>
				<plx:pragma type="closeRegion" data="Pattern Matches"/>
				<xsl:if test="not(cvg:ErrorReportHere)">
					<xsl:call-template name="CheckErrorConditions">
						<xsl:with-param name="Primary" select="false()"/>
						<xsl:with-param name="DeclareErrorOwner" select="false()"/>
						<xsl:with-param name="BeginVerbalization" select="true()"/>
					</xsl:call-template>
				</xsl:if>
				<plx:return>
					<plx:trueKeyword/>
				</plx:return>
			</plx:function>
		</plx:class>
	</xsl:template>
	<xsl:template match="cvg:DerivationPath" mode="ConstraintVerbalization">
		<xsl:param name="VariablePrefix"/>
		<xsl:param name="VariableDecorator"/>
		<xsl:variable name="useThisContext" select="@thisContext[.=1 or .='true']"/>
		<xsl:if test="$useThisContext">
			<xsl:call-template name="EnsureTempStringBuilder"/>
		</xsl:if>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="{$VariablePrefix}{$VariableDecorator}"/>
			</plx:left>
			<plx:right>
				<plx:callInstance name="RenderPathVerbalization">
					<plx:callObject>
						<plx:nameRef name="pathVerbalizer"/>
					</plx:callObject>
					<plx:passParam>
						<xsl:choose>
							<xsl:when test="$useThisContext">
								<plx:thisKeyword/>
							</xsl:when>
							<xsl:otherwise>
								<plx:nameRef name="derivationRule"/>
							</xsl:otherwise>
						</xsl:choose>
					</plx:passParam>
					<plx:passParam>
						<xsl:choose>
							<xsl:when test="$useThisContext">
								<plx:nameRef name="sbTemp"/>
							</xsl:when>
							<xsl:otherwise>
								<plx:nullKeyword/>
							</xsl:otherwise>
						</xsl:choose>
					</plx:passParam>
				</plx:callInstance>
			</plx:right>
		</plx:assign>
	</xsl:template>
	<xsl:template match="cvg:ProvidedRolePlayer" mode="ConstraintVerbalization">
		<xsl:param name="VariablePrefix"/>
		<xsl:param name="VariableDecorator"/>
		<xsl:param name="IteratorVariableName"/>
		<xsl:param name="PatternGroup"/>
		<xsl:param name="ContextMatch"/>
		<xsl:variable name="rolePlayerKey" select="string(@rolePlayerKey)"/>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="{$VariablePrefix}{$VariableDecorator}"/>
			</plx:left>
			<plx:right>
				<plx:callInstance name="RenderAssociatedRolePlayer">
					<plx:callObject>
						<plx:nameRef name="pathVerbalizer"/>
					</plx:callObject>
					<plx:passParam>
						<xsl:choose>
							<xsl:when test="$rolePlayerKey='DerivationRule'">
								<plx:nameRef name="derivationRule"/>
							</xsl:when>
							<xsl:when test="$rolePlayerKey='SingleLeadRolePath'">
								<plx:callInstance name="PathRoot" type="property">
									<plx:callObject>
										<plx:nameRef name="singleLeadRolePath"/>
									</plx:callObject>
								</plx:callInstance>
							</xsl:when>
							<xsl:when test="$rolePlayerKey='ConstraintRole' or $rolePlayerKey='FactRole'">
								<xsl:call-template name="ReferenceIteratorRole">
									<xsl:with-param name="ContextMatch" select="$ContextMatch"/>
									<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
									<xsl:with-param name="IteratorIndex">
										<plx:nameRef name="{$IteratorVariableName}"/>
									</xsl:with-param>
								</xsl:call-template>
							</xsl:when>
							<xsl:when test="$rolePlayerKey='QueryParameter'">
								<plx:nameRef name="queryParameter"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:message terminate="yes">
									<xsl:text>ProvidedRolePlayer has unrecognized rolePlayerKey: </xsl:text>
									<xsl:value-of select="$rolePlayerKey"/>
								</xsl:message>
							</xsl:otherwise>
						</xsl:choose>
					</plx:passParam>
					<plx:passParam>
						<plx:nullKeyword/>
					</plx:passParam>
					<plx:passParam>
						<xsl:call-template name="GetRolePlayerRenderingOptions"/>
					</plx:passParam>
				</plx:callInstance>
			</plx:right>
		</plx:assign>
	</xsl:template>
	<xsl:template match="cvg:ErrorReport" mode="GenerateClasses">
		<xsl:variable name="parentClass" select="string(@childHelperFor)"/>
		<xsl:variable name="isChildHelper" select="boolean($parentClass)"/>
		<xsl:if test="$isChildHelper">
			<xsl:text disable-output-escaping="yes"><![CDATA[<plx:class name="]]></xsl:text>
			<xsl:value-of select="$parentClass"/>
			<xsl:text disable-output-escaping="yes"><![CDATA[" visibility="public" partial="true"><plx:leadingInfo><plx:pragma type="region" data="]]></xsl:text>
			<xsl:value-of select="concat($parentClass,'.',@type)"/>
			<xsl:text disable-output-escaping="yes"><![CDATA[ verbalization"/></plx:leadingInfo><plx:trailingInfo><plx:pragma type="closeRegion" data="]]></xsl:text>
			<xsl:value-of select="concat($parentClass,'.',@type)"/>
			<xsl:text disable-output-escaping="yes"><![CDATA[ verbalization"/></plx:trailingInfo>]]></xsl:text>
		</xsl:if>
		<plx:class name="{@type}" visibility="public" partial="true">
			<xsl:choose>
				<xsl:when test="$isChildHelper">
					<xsl:attribute name="visibility">
						<xsl:text>private</xsl:text>
					</xsl:attribute>
				</xsl:when>
				<xsl:otherwise>
					<plx:leadingInfo>
						<plx:pragma type="region" data="{@type} verbalization"/>
					</plx:leadingInfo>
					<plx:trailingInfo>
						<plx:pragma type="closeRegion" data="{@type} verbalization"/>
					</plx:trailingInfo>
				</xsl:otherwise>
			</xsl:choose>
			<plx:implementsInterface dataTypeName="IVerbalize"/>
			<xsl:if test="$isChildHelper">
				<xsl:call-template name="GetVerbalizer">
					<xsl:with-param name="type" select="@type"/>
				</xsl:call-template>
			</xsl:if>
			<plx:function name="GetVerbalization" visibility="protected">
				<plx:leadingInfo>
					<plx:docComment>
						<summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:interfaceMember memberName="GetVerbalization" dataTypeName="IVerbalize"/>
				<plx:param name="writer" dataTypeName="TextWriter"/>
				<plx:param name="snippetsDictionary" dataTypeName="IDictionary">
					<plx:passTypeParam dataTypeName="Type"/>
					<plx:passTypeParam dataTypeName="IVerbalizationSets"/>
				</plx:param>
				<plx:param name="verbalizationContext" dataTypeName="IVerbalizationContext"/>
				<plx:param name="sign" dataTypeName="VerbalizationSign"/>
				<plx:returns dataTypeName=".boolean"/>

				<xsl:call-template name="DeclareSnippetsLocal"/>
				<xsl:call-template name="CheckErrorConditions">
					<xsl:with-param name="Standalone" select="true()"/>
				</xsl:call-template>
			</plx:function>
		</plx:class>
		<xsl:if test="$isChildHelper">
			<xsl:text disable-output-escaping="yes"><![CDATA[</plx:class>]]></xsl:text>
		</xsl:if>
	</xsl:template>
	<xsl:template match="cvg:ErrorReportHere" mode="ConstraintVerbalization">
		<xsl:call-template name="CheckErrorConditions">
			<xsl:with-param name="Primary" select="false()"/>
			<xsl:with-param name="DeclareErrorOwner" select="false()"/>
		</xsl:call-template>
	</xsl:template>
	<xsl:template match="cvg:Constraint" mode="ConstraintVerbalization">
		<xsl:variable name="patternGroup" select="string(@patternGroup)"/>
		<xsl:variable name="isValueTypeValueConstraint" select="$patternGroup='ValueTypeValueConstraint'"/>
		<xsl:variable name="isObjectTypeCardinality" select="$patternGroup='ObjectTypeCardinalityConstraint'"/>
		<xsl:variable name="isNearestValueConstraint" select="$patternGroup='NearestValueConstraint'"/>
		<xsl:variable name="isObjectTypeConstraint" select="$isValueTypeValueConstraint or $isObjectTypeCardinality or $isNearestValueConstraint"/>
		<xsl:variable name="noReadingVerbalization" select="@type='UniquenessPreferredVerbalizer'"/>
		<xsl:variable name="isRoleValue" select="$patternGroup='RoleValueConstraint'"/>
		<xsl:variable name="isUnaryRoleCardinality" select="$patternGroup='UnaryRoleCardinalityConstraint'"/>
		<xsl:variable name="isSingleRoleConstraint" select="$isRoleValue or $isUnaryRoleCardinality"/>
		<xsl:variable name="isInternal" select="$patternGroup='InternalConstraint' or $isSingleRoleConstraint"/>
		<xsl:variable name="isSingleColumn" select="$patternGroup='SetConstraint'"/>
		<xsl:variable name="resolveProxy" select="@impliedConstraint"/>
		<xsl:variable name="parentClass" select="string(@childHelperFor)"/>
		<xsl:variable name="isChildHelper" select="boolean($parentClass)"/>
		<xsl:variable name="errorReport" select="not($isChildHelper) or (@childHelperErrorReport='true' or @childHelperErrorReport='1')"/>
		<xsl:variable name="isSetComparisonConstraint" select="$patternGroup='SetComparisonConstraint'"/>
		<xsl:variable name="compatibleColumns" select="@compatibleColumns='true' or @compatibleColumns='1'"/>
		<xsl:variable name="deferMatchesTo" select="string(@deferMatchesTo)"/>
		<xsl:if test="$isChildHelper">
			<xsl:text disable-output-escaping="yes"><![CDATA[<plx:class name="]]></xsl:text>
			<xsl:value-of select="$parentClass"/>
			<xsl:text disable-output-escaping="yes"><![CDATA[" visibility="public" partial="true"><plx:leadingInfo><plx:pragma type="region" data="]]></xsl:text>
			<xsl:value-of select="concat($parentClass,'.',@type)"/>
			<xsl:text disable-output-escaping="yes"><![CDATA[ verbalization"/></plx:leadingInfo><plx:trailingInfo><plx:pragma type="closeRegion" data="]]></xsl:text>
			<xsl:value-of select="concat($parentClass,'.',@type)"/>
			<xsl:text disable-output-escaping="yes"><![CDATA[ verbalization"/></plx:trailingInfo>]]></xsl:text>
		</xsl:if>
		<plx:class name="{@type}" visibility="public" partial="true">
			<xsl:choose>
				<xsl:when test="$isChildHelper">
					<xsl:attribute name="visibility">
						<xsl:text>private</xsl:text>
					</xsl:attribute>
				</xsl:when>
				<xsl:otherwise>
					<plx:leadingInfo>
						<plx:pragma type="region" data="{@type} verbalization"/>
					</plx:leadingInfo>
					<plx:trailingInfo>
						<plx:pragma type="closeRegion" data="{@type} verbalization"/>
					</plx:trailingInfo>
				</xsl:otherwise>
			</xsl:choose>
			<plx:implementsInterface dataTypeName="IVerbalize"/>
			<xsl:if test="$isChildHelper">
				<xsl:call-template name="GetVerbalizer">
					<xsl:with-param name="type" select="@type"/>
				</xsl:call-template>
			</xsl:if>
			<plx:function name="GetVerbalization" visibility="protected">
				<plx:leadingInfo>
					<plx:docComment>
						<summary><see cref="IVerbalize.GetVerbalization"/> implementation</summary>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:interfaceMember memberName="GetVerbalization" dataTypeName="IVerbalize"/>
				<plx:param name="writer" dataTypeName="TextWriter"/>
				<plx:param name="snippetsDictionary" dataTypeName="IDictionary">
					<plx:passTypeParam dataTypeName="Type"/>
					<plx:passTypeParam dataTypeName="IVerbalizationSets"/>
				</plx:param>
				<plx:param name="verbalizationContext" dataTypeName="IVerbalizationContext"/>
				<plx:param name="sign" dataTypeName="VerbalizationSign"/>
				<plx:returns dataTypeName=".boolean"/>

				<plx:pragma type="region" data="Preliminary"/>
				<xsl:call-template name="DeclareSnippetsLocal"/>
				<!-- Don't proceed with verbalization if blocking errors are present -->
				<xsl:if test="$errorReport">
					<xsl:call-template name="CheckErrorConditions"/>
				</xsl:if>
				<xsl:if test="not($deferMatchesTo)">
					<xsl:call-template name="DeclareIsNegative"/>
					<xsl:variable name="subscriptsDirective" select="cvg:EnableSubscripts"/>
					<xsl:variable name="subscriptConditionsFragment">
						<!-- UNDONE: Better subscript handling. The conditional processing needs
						 to be moved inside each pattern, but we need to prepare for the situation
						 up front. Consider getting the generator out of the subscripting business
						 altogether. We're basically just spitting an inline function. For now,
						 keep the conditional checks in place so we don't lose the work. The trueKeyword
						 spit here will be compiled out and not appear in code. -->
						<xsl:apply-templates select="$subscriptsDirective" mode="SubscriptConditions"/>
					</xsl:variable>
					<xsl:variable name="subscriptConditions" select="exsl:node-set($subscriptConditionsFragment)/child::*"/>
					<xsl:variable name="customSubscripts" select="boolean($subscriptsDirective[@custom='defaultPlain' or @custom='defaultSubscript'])"/>
					<xsl:variable name="dynamicSubscripts" select="$customSubscripts and descendant::*[@overflowSubscript]"/>

					<!-- Pick up standard code we'll need for any constraint -->
					<xsl:if test="$isSingleRoleConstraint">
						<plx:local name="constrainedRole" dataTypeName="Role">
							<plx:initialize>
								<plx:callThis name="Role" type="property">
									<xsl:if test="$isUnaryRoleCardinality">
										<xsl:attribute name="name">
											<xsl:text>UnaryRole</xsl:text>
										</xsl:attribute>
									</xsl:if>
								</plx:callThis>
							</plx:initialize>
						</plx:local>
					</xsl:if>
					<xsl:choose>
						<xsl:when test="($isInternal and not($isSingleRoleConstraint)) or $isSingleColumn or $isSetComparisonConstraint">
							<plx:local name="isDeontic" dataTypeName=".boolean">
								<plx:initialize>
									<plx:binaryOperator type="equality">
										<plx:left>
											<xsl:choose>
												<xsl:when test="$isChildHelper">
													<plx:callThis name="Modality" type="property"/>
												</xsl:when>
												<xsl:otherwise>
													<plx:callInstance name="Modality" type="property">
														<plx:callObject>
															<plx:cast dataTypeName="IConstraint" type="testCast">
																<plx:thisKeyword/>
															</plx:cast>
														</plx:callObject>
													</plx:callInstance>
												</xsl:otherwise>
											</xsl:choose>
										</plx:left>
										<plx:right>
											<plx:callStatic dataTypeName="ConstraintModality" name="Deontic" type="field"/>
										</plx:right>
									</plx:binaryOperator>
								</plx:initialize>
							</plx:local>
						</xsl:when>
						<xsl:when test="$isObjectTypeCardinality or $isUnaryRoleCardinality or $isValueTypeValueConstraint or $isRoleValue or $isNearestValueConstraint">
							<plx:local name="isDeontic" dataTypeName=".boolean">
								<plx:initialize>
									<plx:binaryOperator type="equality">
										<plx:left>
											<plx:callThis name="Modality" type="property"/>
										</plx:left>
										<plx:right>
											<plx:callStatic dataTypeName="ConstraintModality" name="Deontic" type="field"/>
										</plx:right>
									</plx:binaryOperator>
								</plx:initialize>
							</plx:local>
						</xsl:when>
						<xsl:otherwise>
							<plx:local name="isDeontic" dataTypeName=".boolean">
								<plx:initialize>
									<plx:falseKeyword/>
								</plx:initialize>
							</plx:local>
						</xsl:otherwise>
					</xsl:choose>
					<xsl:if test="descendant::*/@listStyle">
						<plx:local name="sbTemp" dataTypeName="StringBuilder">
							<plx:initialize>
								<plx:nullKeyword/>
							</plx:initialize>
						</plx:local>
					</xsl:if>
					<xsl:if test="not($isObjectTypeConstraint)">
						<plx:local name="parentFact" dataTypeName="FactType">
							<xsl:choose>
								<xsl:when test="$isInternal and not($isSingleRoleConstraint)">
									<plx:initialize>
										<plx:callThis name="FactType" type="property"/>
									</plx:initialize>
								</xsl:when>
								<xsl:when test="$isSingleRoleConstraint">
									<plx:initialize>
										<plx:callInstance name="FactType" type="property">
											<plx:callObject>
												<plx:nameRef name="constrainedRole" type="local"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:initialize>
								</xsl:when>
							</xsl:choose>
						</plx:local>
						<xsl:if test="not($noReadingVerbalization)">
							<plx:local name="predicatePartFormatString" dataTypeName=".string">
								<xsl:if test="$isInternal">
									<plx:initialize>
										<xsl:call-template name="PopulatePredicatePartFormatString"/>
									</plx:initialize>
								</xsl:if>
							</plx:local>
						</xsl:if>
					</xsl:if>
					<xsl:if test="$isInternal and not($isSingleRoleConstraint)">
						<plx:local name="includedRoles" dataTypeName="LinkedElementCollection">
							<plx:passTypeParam dataTypeName="Role"/>
							<plx:initialize>
								<plx:callThis name="RoleCollection" type="property"/>
							</plx:initialize>
						</plx:local>
					</xsl:if>
					<xsl:if test="$isSingleRoleConstraint">
						<plx:local name="includedRoles" dataTypeName="IList">
							<plx:passTypeParam dataTypeName="Role"/>
							<plx:initialize>
								<plx:callNew dataTypeName="Role">
									<plx:arrayDescriptor rank="1"/>
									<plx:arrayInitializer>
										<plx:nameRef name="constrainedRole" type="local"/>
									</plx:arrayInitializer>
								</plx:callNew>
							</plx:initialize>
						</plx:local>
					</xsl:if>
					<xsl:if test="not($isObjectTypeConstraint)">
						<plx:local name="allReadingOrders" dataTypeName="LinkedElementCollection">
							<plx:passTypeParam dataTypeName="ReadingOrder"/>
							<xsl:if test="$isInternal">
								<plx:initialize>
									<plx:callInstance name="ReadingOrderCollection" type="property">
										<plx:callObject>
											<plx:nameRef name="parentFact"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:initialize>
							</xsl:if>
						</plx:local>
						<plx:local name="factRoles" dataTypeName="IList">
							<plx:passTypeParam dataTypeName="RoleBase"/>
							<plx:initialize>
								<xsl:choose>
									<xsl:when test="$isInternal">
										<xsl:call-template name="InitializeDefaultFactRoles"/>
									</xsl:when>
									<xsl:otherwise>
										<plx:nullKeyword/>
									</xsl:otherwise>
								</xsl:choose>
							</plx:initialize>
						</plx:local>
						<plx:local name="factArity" dataTypeName=".i4">
							<plx:initialize>
								<xsl:choose>
									<xsl:when test="$isInternal">
										<xsl:call-template name="InitializeFactArity"/>
									</xsl:when>
									<xsl:otherwise>
										<plx:value data="0" type="i4"/>
									</xsl:otherwise>
								</xsl:choose>
							</plx:initialize>
						</plx:local>
					</xsl:if>
					<xsl:if test="$isSetComparisonConstraint">
						<plx:local name="constraintSequences" dataTypeName="LinkedElementCollection">
							<plx:passTypeParam dataTypeName="SetComparisonConstraintRoleSequence"/>
							<plx:initialize>
								<plx:callThis type="property" name="RoleSequenceCollection"/>
							</plx:initialize>
						</plx:local>
					</xsl:if>
					<xsl:if test="not($isInternal) and not($isObjectTypeConstraint)">
						<xsl:choose>
							<xsl:when test="not($isSetComparisonConstraint)">
								<plx:local name="allConstraintRoles" dataTypeName="LinkedElementCollection">
									<plx:passTypeParam dataTypeName="Role"/>
									<plx:initialize>
										<plx:callThis name="RoleCollection" type="property"/>
									</plx:initialize>
								</plx:local>
							</xsl:when>
							<xsl:otherwise>
								<plx:local name="constraintRoleArity" dataTypeName=".i4">
									<plx:initialize>
										<plx:callInstance name="Count" type="property">
											<plx:callObject>
												<plx:nameRef name="constraintSequences" type="local"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:initialize>
								</plx:local>
								<plx:local name="allConstraintRoleSequences" dataTypeIsSimpleArray="true" dataTypeName="IList">
									<plx:passTypeParam dataTypeName="ConstraintRoleSequenceHasRole"/>
									<plx:initialize>
										<plx:callNew dataTypeName="IList" dataTypeIsSimpleArray="true">
											<plx:passTypeParam dataTypeName="ConstraintRoleSequenceHasRole"/>
											<plx:passParam>
												<plx:nameRef name="constraintRoleArity" type="local"/>
											</plx:passParam>
										</plx:callNew>
									</plx:initialize>
								</plx:local>
								<plx:loop>
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
												<plx:nameRef name="i"/>
											</plx:left>
											<plx:right>
												<plx:nameRef name="constraintRoleArity"/>
											</plx:right>
										</plx:binaryOperator>
									</plx:condition>
									<plx:beforeLoop>
										<plx:increment>
											<plx:nameRef name="i"/>
										</plx:increment>
									</plx:beforeLoop>
									<plx:assign>
										<plx:left>
											<plx:callInstance name=".implied" type="arrayIndexer">
												<plx:callObject>
													<plx:nameRef name="allConstraintRoleSequences" type="local"/>
												</plx:callObject>
												<plx:passParam>
													<plx:nameRef name="i" type="local" />
												</plx:passParam>
											</plx:callInstance>
										</plx:left>
										<plx:right>
											<plx:callStatic dataTypeName="ConstraintRoleSequenceHasRole" name="GetLinksToRoleCollection">
												<plx:passParam>
													<plx:callInstance name=".implied" type="indexerCall">
														<plx:callObject>
															<plx:nameRef name="constraintSequences" type="local"/>
														</plx:callObject>
														<plx:passParam>
															<plx:nameRef name="i" type="local" />
														</plx:passParam>
													</plx:callInstance>
												</plx:passParam>
											</plx:callStatic>
										</plx:right>
									</plx:assign>
								</plx:loop>
								<plx:local name="columnArity" dataTypeName=".i4">
									<plx:initialize>
										<plx:callInstance name="Count" type="property">
											<plx:callObject>
												<plx:callInstance name=".implied" type="indexerCall">
													<plx:callObject>
														<plx:nameRef name="allConstraintRoleSequences" type="local"/>
													</plx:callObject>
													<plx:passParam>
														<plx:value data="0" type="i4"/>
													</plx:passParam>
												</plx:callInstance>
											</plx:callObject>
										</plx:callInstance>
									</plx:initialize>
								</plx:local>
							</xsl:otherwise>
						</xsl:choose>
						<plx:local name="allFacts" dataTypeName="LinkedElementCollection">
							<plx:passTypeParam dataTypeName="FactType"/>
							<plx:initialize>
								<plx:callThis name="FactTypeCollection" type="property"/>
							</plx:initialize>
						</plx:local>
						<plx:local name="allFactsCount" dataTypeName=".i4">
							<plx:initialize>
								<plx:callInstance name="Count" type="property">
									<plx:callObject>
										<plx:nameRef name="allFacts"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:initialize>
						</plx:local>
						<plx:branch>
							<plx:condition>
								<plx:binaryOperator type="equality">
									<plx:left>
										<plx:nameRef name="allFactsCount"/>
									</plx:left>
									<plx:right>
										<plx:value type="i4" data="0"/>
									</plx:right>
								</plx:binaryOperator>
							</plx:condition>
							<xsl:if test="$errorReport">
								<xsl:call-template name="CheckErrorConditions">
									<xsl:with-param name="Primary" select="false()"/>
									<xsl:with-param name="DeclareErrorOwner" select="false()"/>
									<xsl:with-param name="BeginVerbalization" select="true()"/>
								</xsl:call-template>
							</xsl:if>
							<plx:return>
								<!-- This should be an error on the constraint, but be defensive and bail
								if we have no facts -->
								<plx:falseKeyword/>
							</plx:return>
						</plx:branch>
					</xsl:if>
					<xsl:choose>
						<xsl:when test="$isInternal">
							<plx:local name="includedArity" dataTypeName=".i4">
								<plx:initialize>
									<plx:callInstance name="Count" type="property">
										<plx:callObject>
											<plx:nameRef name="includedRoles"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:initialize>
							</plx:local>
							<!-- No included roles is not an error, but we can't verbalize it -->
							<plx:branch>
								<plx:condition>
									<plx:binaryOperator type="booleanOr">
										<plx:left>
											<!-- No readings is an error on the parent, so we can get past the error check without them -->
											<plx:binaryOperator type="equality">
												<plx:left>
													<plx:callInstance name="Count" type="property">
														<plx:callObject>
															<plx:nameRef name="allReadingOrders"/>
														</plx:callObject>
													</plx:callInstance>
												</plx:left>
												<plx:right>
													<plx:value type="i4" data="0"/>
												</plx:right>
											</plx:binaryOperator>
										</plx:left>
										<plx:right>
											<plx:binaryOperator type="equality">
												<plx:left>
													<plx:nameRef name="includedArity"/>
												</plx:left>
												<plx:right>
													<plx:value type="i4" data="0"/>
												</plx:right>
											</plx:binaryOperator>
										</plx:right>
									</plx:binaryOperator>
								</plx:condition>
								<xsl:if test="$errorReport">
									<xsl:call-template name="CheckErrorConditions">
										<xsl:with-param name="Primary" select="false()"/>
										<xsl:with-param name="DeclareErrorOwner" select="false()"/>
										<xsl:with-param name="BeginVerbalization" select="true()"/>
									</xsl:call-template>
								</xsl:if>
								<plx:return>
									<plx:falseKeyword/>
								</plx:return>
							</plx:branch>
						</xsl:when>
						<xsl:when test="$isSingleColumn or $isSetComparisonConstraint">
							<xsl:choose>
								<xsl:when test="$isSetComparisonConstraint">
									<plx:local name="pathVerbalizer" dataTypeName="RolePathVerbalizer">
										<plx:initialize>
											<plx:callStatic dataTypeName="RolePathVerbalizer" name="Create">
												<plx:passParam>
													<plx:thisKeyword/>
												</plx:passParam>
												<plx:passParam>
													<plx:callNew dataTypeName="StandardRolePathRenderer">
														<plx:passParam>
															<plx:nameRef name="snippets"/>
														</plx:passParam>
														<plx:passParam>
															<plx:nameRef name="verbalizationContext" type="parameter"/>
														</plx:passParam>
														<plx:passParam>
															<plx:callInstance name="FormatProvider" type="property">
																<plx:callObject>
																	<plx:nameRef name="writer"/>
																</plx:callObject>
															</plx:callInstance>
														</plx:passParam>
													</plx:callNew>
												</plx:passParam>
											</plx:callStatic>
										</plx:initialize>
									</plx:local>
								</xsl:when>
								<xsl:otherwise>
									<plx:local name="allBasicRoleReplacements" dataTypeName=".string">
										<plx:arrayDescriptor rank="1">
											<plx:arrayDescriptor rank="1">
												<xsl:if test="$customSubscripts">
													<xsl:attribute name="rank">
														<xsl:text>2</xsl:text>
													</xsl:attribute>
												</xsl:if>
											</plx:arrayDescriptor>
										</plx:arrayDescriptor>
										<plx:initialize>
											<plx:callNew dataTypeName=".string">
												<plx:arrayDescriptor rank="1">
													<plx:arrayDescriptor rank="1">
														<xsl:if test="$customSubscripts">
															<xsl:attribute name="rank">
																<xsl:text>2</xsl:text>
															</xsl:attribute>
														</xsl:if>
													</plx:arrayDescriptor>
												</plx:arrayDescriptor>
												<plx:passParam>
													<plx:nameRef name="allFactsCount"/>
												</plx:passParam>
											</plx:callNew>
										</plx:initialize>
									</plx:local>
									<xsl:if test="$customSubscripts">
										<plx:local name="subscripter" dataTypeName="VerbalizationSubscripter">
											<plx:initialize>
												<plx:callNew dataTypeName="VerbalizationSubscripter">
													<plx:passParam>
														<plx:callInstance name="FormatProvider" type="property">
															<plx:callObject>
																<plx:nameRef name="writer" type="parameter"/>
															</plx:callObject>
														</plx:callInstance>
													</plx:passParam>
												</plx:callNew>
											</plx:initialize>
										</plx:local>
									</xsl:if>
								</xsl:otherwise>
							</xsl:choose>
							<xsl:if test="not(@type='RingConstraint' or @childHelperFor='RingConstraint' or @type='FrequencyConstraint' or @type='SimpleMandatoryVerbalizer' or @type='UniquenessPossibilityVerbalizer' or @type='ValueComparisonConstraint' or $isSetComparisonConstraint)">
								<!-- UNDONE: Temporary workaround for RingConstraint and FrequencyConstraint compile errors. Use StripUnusedLocals to test demand. -->
								<plx:local name="contextBasicReplacementIndex" dataTypeName=".i4"/>
							</xsl:if>
							<plx:local name="minFactArity" dataTypeName=".i4">
								<plx:initialize>
									<plx:callStatic name="MaxValue" dataTypeName=".i4" type="field"/>
								</plx:initialize>
							</plx:local>
							<plx:local name="maxFactArity" dataTypeName=".i4">
								<plx:initialize>
									<plx:callStatic name="MinValue" dataTypeName=".i4" type="field"/>
								</plx:initialize>
							</plx:local>
							<plx:loop>
								<plx:initializeLoop>
									<plx:local name="iFact" dataTypeName=".i4">
										<plx:initialize>
											<plx:value type="i4" data="0"/>
										</plx:initialize>
									</plx:local>
								</plx:initializeLoop>
								<plx:condition>
									<plx:binaryOperator type="lessThan">
										<plx:left>
											<plx:nameRef name="iFact"/>
										</plx:left>
										<plx:right>
											<plx:nameRef name="allFactsCount"/>
										</plx:right>
									</plx:binaryOperator>
								</plx:condition>
								<plx:beforeLoop>
									<plx:increment>
										<plx:nameRef name="iFact"/>
									</plx:increment>
								</plx:beforeLoop>
								<!-- Return if there are no readings. We need readings for all facts
								 to verbalize the constraint -->
								<plx:local name="currentFact" dataTypeName="FactType">
									<plx:initialize>
										<plx:callInstance name=".implied" type="arrayIndexer">
											<plx:callObject>
												<plx:nameRef name="allFacts"/>
											</plx:callObject>
											<plx:passParam>
												<plx:nameRef name="iFact"/>
											</plx:passParam>
										</plx:callInstance>
									</plx:initialize>
								</plx:local>
								<plx:branch>
									<plx:condition>
										<plx:binaryOperator type="identityInequality">
											<plx:left>
												<plx:callInstance name="VerbalizationBlockingReadingRequiredError" type="property">
													<plx:callObject>
														<plx:nameRef name="currentFact"/>
													</plx:callObject>
												</plx:callInstance>
											</plx:left>
											<plx:right>
												<plx:nullKeyword/>
											</plx:right>
										</plx:binaryOperator>
									</plx:condition>
									<xsl:if test="$errorReport">
										<xsl:call-template name="CheckErrorConditions">
											<xsl:with-param name="Primary" select="false()"/>
											<xsl:with-param name="DeclareErrorOwner" select="false()"/>
											<xsl:with-param name="BeginVerbalization" select="true()"/>
										</xsl:call-template>
									</xsl:if>
									<plx:return>
										<plx:falseKeyword/>
									</plx:return>
								</plx:branch>
								<!-- Get the roles and role count for the current fact -->
								<plx:assign>
									<plx:left>
										<plx:nameRef name="allReadingOrders"/>
									</plx:left>
									<plx:right>
										<plx:callInstance name="ReadingOrderCollection" type="property">
											<plx:callObject>
												<plx:nameRef name="currentFact"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:right>
								</plx:assign>
								<plx:assign>
									<plx:left>
										<plx:nameRef name="factRoles"/>
									</plx:left>
									<plx:right>
										<xsl:call-template name="InitializeDefaultFactRoles">
											<xsl:with-param name="FallbackRoleCollection">
												<plx:callInstance name="RoleCollection" type="property">
													<plx:callObject>
														<plx:nameRef name="currentFact"/>
													</plx:callObject>
												</plx:callInstance>
											</xsl:with-param>
										</xsl:call-template>
									</plx:right>
								</plx:assign>
								<plx:assign>
									<plx:left>
										<plx:nameRef name="factArity"/>
									</plx:left>
									<plx:right>
										<xsl:call-template name="InitializeFactArity"/>
									</plx:right>
								</plx:assign>
								<!-- Track the min and max values for our current fact arity -->
								<plx:branch>
									<plx:condition>
										<plx:binaryOperator type="lessThan">
											<plx:left>
												<plx:nameRef name="factArity"/>
											</plx:left>
											<plx:right>
												<plx:nameRef name="minFactArity"/>
											</plx:right>
										</plx:binaryOperator>
									</plx:condition>
									<plx:assign>
										<plx:left>
											<plx:nameRef name="minFactArity"/>
										</plx:left>
										<plx:right>
											<plx:nameRef name="factArity"/>
										</plx:right>
									</plx:assign>
								</plx:branch>
								<plx:branch>
									<plx:condition>
										<plx:binaryOperator type="greaterThan">
											<plx:left>
												<plx:nameRef name="factArity"/>
											</plx:left>
											<plx:right>
												<plx:nameRef name="maxFactArity"/>
											</plx:right>
										</plx:binaryOperator>
									</plx:condition>
									<plx:assign>
										<plx:left>
											<plx:nameRef name="maxFactArity"/>
										</plx:left>
										<plx:right>
											<plx:nameRef name="factArity"/>
										</plx:right>
									</plx:assign>
								</plx:branch>
								<xsl:if test="not($isSetComparisonConstraint)">
									<!-- Populate the basic replacements for this fact -->
									<xsl:call-template name="PopulateBasicRoleReplacements">
										<xsl:with-param name="SubscriptConditions" select="$subscriptConditions"/>
										<xsl:with-param name="CustomSubscripts" select="$customSubscripts"/>
										<xsl:with-param name="DynamicSubscripts" select="$dynamicSubscripts"/>
										<xsl:with-param name="CompatibleColumns" select="$compatibleColumns"/>
										<xsl:with-param name="DeclareSubscripter" select="false()"/>
										<xsl:with-param name="PatternGroup" select="$patternGroup"/>
									</xsl:call-template>
									<plx:assign>
										<plx:left>
											<plx:callInstance name=".implied" type="arrayIndexer">
												<plx:callObject>
													<plx:nameRef name="allBasicRoleReplacements"/>
												</plx:callObject>
												<plx:passParam>
													<plx:nameRef name="iFact"/>
												</plx:passParam>
											</plx:callInstance>
										</plx:left>
										<plx:right>
											<plx:nameRef name="basicRoleReplacements"/>
										</plx:right>
									</plx:assign>
								</xsl:if>
							</plx:loop>
							<xsl:if test="not($isSetComparisonConstraint)">
								<plx:local name="constraintRoleArity" dataTypeName=".i4">
									<plx:initialize>
										<plx:callInstance name="Count" type="property">
											<plx:callObject>
												<plx:nameRef name="allConstraintRoles"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:initialize>
								</plx:local>
								<xsl:if test="@automaticJoinPathPattern or cvg:ConstrainedRoles[@blockHelpers and contains(@blockHelpers,'preProjectionKeys')]">
									<plx:local name="joinPath" dataTypeName="ConstraintRoleSequenceJoinPath">
										<plx:initialize>
											<plx:callThis name="JoinPath" type="property"/>
										</plx:initialize>
									</plx:local>
								</xsl:if>
								<xsl:if test="@automaticJoinPathPattern">
									<plx:local name="singleLeadRolePath" dataTypeName="LeadRolePath">
										<plx:initialize>
											<plx:nullKeyword/>
										</plx:initialize>
									</plx:local>
									<xsl:choose>
										<xsl:when test="@automaticJoinPathPattern='OppositeRole'">
											<plx:local name="isTrivialOppositeRolePath" dataTypeName=".boolean">
												<plx:initialize>
													<plx:falseKeyword/>
												</plx:initialize>
											</plx:local>
											<plx:branch>
												<plx:condition>
													<plx:binaryOperator type="booleanAnd">
														<plx:left>
															<plx:binaryOperator type="identityInequality">
																<plx:left>
																	<plx:nameRef name="joinPath"/>
																</plx:left>
																<plx:right>
																	<plx:nullKeyword/>
																</plx:right>
															</plx:binaryOperator>
														</plx:left>
														<plx:right>
															<plx:callInstance name="IsAutomatic" type="property">
																<plx:callObject>
																	<plx:nameRef name="joinPath"/>
																</plx:callObject>
															</plx:callInstance>
														</plx:right>
													</plx:binaryOperator>
												</plx:condition>
												<plx:local name="rootObjectType" dataTypeName="ObjectType"/>
												<plx:branch>
													<plx:condition>
														<plx:binaryOperator type="booleanAnd">
															<plx:left>
																<plx:binaryOperator type="identityInequality">
																	<plx:left>
																		<plx:nullKeyword/>
																	</plx:left>
																	<plx:right>
																		<plx:inlineStatement dataTypeName="LeadRolePath">
																			<plx:assign>
																				<plx:left>
																					<plx:nameRef name="singleLeadRolePath"/>
																				</plx:left>
																				<plx:right>
																					<plx:callInstance name="SingleLeadRolePath" type="property">
																						<plx:callObject>
																							<plx:nameRef name="joinPath"/>
																						</plx:callObject>
																					</plx:callInstance>
																				</plx:right>
																			</plx:assign>
																		</plx:inlineStatement>
																	</plx:right>
																</plx:binaryOperator>
															</plx:left>
															<plx:right>
																<plx:binaryOperator type="identityInequality">
																	<plx:left>
																		<plx:nullKeyword/>
																	</plx:left>
																	<plx:right>
																		<plx:inlineStatement dataTypeName="ObjectType">
																			<plx:assign>
																				<plx:left>
																					<plx:nameRef name="rootObjectType"/>
																				</plx:left>
																				<plx:right>
																					<plx:callInstance name="RootObjectType" type="property">
																						<plx:callObject>
																							<plx:nameRef name="singleLeadRolePath"/>
																						</plx:callObject>
																					</plx:callInstance>
																				</plx:right>
																			</plx:assign>
																		</plx:inlineStatement>
																	</plx:right>
																</plx:binaryOperator>
															</plx:right>
														</plx:binaryOperator>
													</plx:condition>
													<plx:local name="i" dataTypeName=".i4">
														<plx:initialize>
															<plx:value data="0" type="i4"/>
														</plx:initialize>
													</plx:local>
													<plx:loop>
														<plx:condition>
															<plx:binaryOperator type="lessThan">
																<plx:left>
																	<plx:nameRef name="i"/>
																</plx:left>
																<plx:right>
																	<plx:nameRef name="constraintRoleArity"/>
																</plx:right>
															</plx:binaryOperator>
														</plx:condition>
														<plx:beforeLoop>
															<plx:increment>
																<plx:nameRef name="i"/>
															</plx:increment>
														</plx:beforeLoop>
														<plx:local name="oppositeRole" dataTypeName="RoleBase">
															<plx:initialize>
																<plx:callInstance name="OppositeOrUnaryRole" type="property">
																	<plx:callObject>
																		<plx:callInstance name=".implied" type="indexerCall">
																			<plx:callObject>
																				<plx:nameRef name="allConstraintRoles"/>
																			</plx:callObject>
																			<plx:passParam>
																				<plx:nameRef name="i"/>
																			</plx:passParam>
																		</plx:callInstance>
																	</plx:callObject>
																</plx:callInstance>
															</plx:initialize>
														</plx:local>
														<plx:branch>
															<plx:condition>
																<plx:binaryOperator type="booleanOr">
																	<plx:left>
																		<plx:binaryOperator type="identityEquality">
																			<plx:left>
																				<plx:nameRef name="oppositeRole"/>
																			</plx:left>
																			<plx:right>
																				<plx:nullKeyword/>
																			</plx:right>
																		</plx:binaryOperator>
																	</plx:left>
																	<plx:right>
																		<plx:binaryOperator type="identityInequality">
																			<plx:left>
																				<plx:callInstance name="RolePlayer" type="property">
																					<plx:callObject>
																						<plx:callInstance name="Role" type="property">
																							<plx:callObject>
																								<plx:nameRef name="oppositeRole"/>
																							</plx:callObject>
																						</plx:callInstance>
																					</plx:callObject>
																				</plx:callInstance>
																			</plx:left>
																			<plx:right>
																				<plx:nameRef name="rootObjectType"/>
																			</plx:right>
																		</plx:binaryOperator>
																	</plx:right>
																</plx:binaryOperator>
															</plx:condition>
															<plx:break/>
														</plx:branch>
													</plx:loop>
													<plx:assign>
														<plx:left>
															<plx:nameRef name="isTrivialOppositeRolePath"/>
														</plx:left>
														<plx:right>
															<plx:binaryOperator type="equality">
																<plx:left>
																	<plx:nameRef name="i"/>
																</plx:left>
																<plx:right>
																	<plx:nameRef name="constraintRoleArity"/>
																</plx:right>
															</plx:binaryOperator>
														</plx:right>
													</plx:assign>
												</plx:branch>
											</plx:branch>
										</xsl:when>
									</xsl:choose>
								</xsl:if>
							</xsl:if>
							<plx:local name="allConstraintRoleReadings" dataTypeName="IReading" dataTypeIsSimpleArray="true">
								<plx:initialize>
									<plx:callNew dataTypeName="IReading" dataTypeIsSimpleArray="true">
										<plx:passParam>
											<plx:nameRef name="constraintRoleArity"/>
										</plx:passParam>
									</plx:callNew>
								</plx:initialize>
							</plx:local>
						</xsl:when>
					</xsl:choose>
					<xsl:if test="$isInternal">
						<xsl:call-template name="PopulateBasicRoleReplacements">
							<xsl:with-param name="SubscriptConditions" select="$subscriptConditions"/>
							<xsl:with-param name="CustomSubscripts" select="$customSubscripts"/>
							<xsl:with-param name="DynamicSubscripts" select="$dynamicSubscripts"/>
						</xsl:call-template>
					</xsl:if>
					<xsl:if test="not($isObjectTypeConstraint)">
						<plx:local name="roleReplacements" dataTypeName=".string" dataTypeIsSimpleArray="true">
							<plx:initialize>
								<plx:callNew dataTypeName=".string" dataTypeIsSimpleArray="true">
									<plx:passParam>
										<plx:nameRef name="factArity">
											<xsl:if test="not($isInternal)">
												<xsl:attribute name="name">
													<xsl:text>maxFactArity</xsl:text>
												</xsl:attribute>
											</xsl:if>
										</plx:nameRef>
									</plx:passParam>
								</plx:callNew>
							</plx:initialize>
						</plx:local>
					</xsl:if>
					<xsl:if test="descendant::cvg:Fact">
						<plx:local name="reading" dataTypeName="IReading"/>
						<plx:local name="hyphenBinder" dataTypeName="VerbalizationHyphenBinder"/>
					</xsl:if>
					<xsl:if test="descendant::cvg:*[@sameConstraintRolePlayers[.='true' or .='1'] and not(@constraintArity[.=2])]">
						<plx:local name="singleConstrainedRolePlayerType" dataTypeName="ObjectType">
							<plx:initialize>
								<plx:callInstance name="RolePlayer" type="property">
									<plx:callObject>
										<plx:callInstance name=".implied" type="indexerCall">
											<plx:callObject>
												<plx:nameRef name="allConstraintRoles"/>
											</plx:callObject>
											<plx:passParam>
												<plx:value type="i4" data="0"/>
											</plx:passParam>
										</plx:callInstance>
									</plx:callObject>
								</plx:callInstance>
							</plx:initialize>
						</plx:local>
						<plx:loop>
							<plx:initializeLoop>
								<plx:local name="testSameRolePlayerIndex" dataTypeName=".i4">
									<plx:initialize>
										<plx:value type="i4" data="1"/>
									</plx:initialize>
								</plx:local>
							</plx:initializeLoop>
							<plx:condition>
								<plx:binaryOperator type="lessThan">
									<plx:left>
										<plx:nameRef name="testSameRolePlayerIndex"/>
									</plx:left>
									<plx:right>
										<plx:nameRef name="constraintRoleArity"/>
									</plx:right>
								</plx:binaryOperator>
							</plx:condition>
							<plx:beforeLoop>
								<plx:increment>
									<plx:nameRef name="testSameRolePlayerIndex"/>
								</plx:increment>
							</plx:beforeLoop>
							<plx:branch>
								<plx:condition>
									<plx:binaryOperator type="identityInequality">
										<plx:left>
											<plx:callInstance name="RolePlayer" type="property">
												<plx:callObject>
													<plx:callInstance name=".implied" type="indexerCall">
														<plx:callObject>
															<plx:nameRef name="allConstraintRoles"/>
														</plx:callObject>
														<plx:passParam>
															<plx:nameRef name="testSameRolePlayerIndex"/>
														</plx:passParam>
													</plx:callInstance>
												</plx:callObject>
											</plx:callInstance>
										</plx:left>
										<plx:right>
											<plx:nameRef name="singleConstrainedRolePlayerType"/>
										</plx:right>
									</plx:binaryOperator>
								</plx:condition>
								<plx:assign>
									<plx:left>
										<plx:nameRef name="singleConstrainedRolePlayerType"/>
									</plx:left>
									<plx:right>
										<plx:nullKeyword/>
									</plx:right>
								</plx:assign>
								<plx:break/>
							</plx:branch>
						</plx:loop>
					</xsl:if>
					<xsl:if test="$isRoleValue or $isValueTypeValueConstraint or $isNearestValueConstraint">
						<plx:local name="ranges" dataTypeName="LinkedElementCollection">
							<plx:passTypeParam dataTypeName="ValueRange"/>
							<plx:initialize>
								<plx:callThis name="ValueRangeCollection" type="property"/>
							</plx:initialize>
						</plx:local>
						<!-- UNDONE: Equality should be verified with the DataType's compare method, if available -->
						<plx:local name="isSingleValue" dataTypeName=".boolean">
							<plx:initialize>
								<plx:binaryOperator type="booleanAnd">
									<plx:left>
										<plx:binaryOperator type="equality">
											<plx:left>
												<plx:callInstance name="Count" type="property">
													<plx:callObject>
														<plx:nameRef name="ranges" type="local"/>
													</plx:callObject>
												</plx:callInstance>
											</plx:left>
											<plx:right>
												<plx:value data="1" type="i4"/>
											</plx:right>
										</plx:binaryOperator>
									</plx:left>
									<plx:right>
										<plx:binaryOperator type="equality">
											<plx:left>
												<plx:callInstance name="MinValue" type="property">
													<plx:callObject>
														<plx:callInstance name=".implied" type="indexerCall">
															<plx:callObject>
																<plx:nameRef name="ranges"/>
															</plx:callObject>
															<plx:passParam>
																<plx:value data="0" type="i4"/>
															</plx:passParam>
														</plx:callInstance>
													</plx:callObject>
												</plx:callInstance>
											</plx:left>
											<plx:right>
												<plx:callInstance name="MaxValue" type="property">
													<plx:callObject>
														<plx:callInstance name=".implied" type="indexerCall">
															<plx:callObject>
																<plx:nameRef name="ranges"/>
															</plx:callObject>
															<plx:passParam>
																<plx:value data="0" type="i4"/>
															</plx:passParam>
														</plx:callInstance>
													</plx:callObject>
												</plx:callInstance>
											</plx:right>
										</plx:binaryOperator>
									</plx:right>
								</plx:binaryOperator>
							</plx:initialize>
						</plx:local>
						<plx:local name="isText" dataTypeName=".boolean">
							<plx:initialize>
								<plx:callThis name="IsText" type="property"/>
							</plx:initialize>
						</plx:local>
					</xsl:if>
					<xsl:if test="$isObjectTypeCardinality or $isUnaryRoleCardinality">
						<plx:local name="ranges" dataTypeName="LinkedElementCollection">
							<plx:passTypeParam dataTypeName="CardinalityRange"/>
							<plx:initialize>
								<plx:callThis name="RangeCollection" type="property"/>
							</plx:initialize>
						</plx:local>
						<plx:local name="rangeCount" dataTypeName=".i4">
							<plx:initialize>
								<plx:callInstance name="Count" type="property">
									<plx:callObject>
										<plx:nameRef name="ranges"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:initialize>
						</plx:local>
						<plx:local name="range" dataTypeName="CardinalityRange"/>
						<plx:local name="rangeLowerBound" dataTypeName=".i4"/>
						<plx:local name="rangeUpperBound" dataTypeName=".i4"/>
					</xsl:if>
				</xsl:if>
				<plx:pragma type="closeRegion" data="Preliminary"/>
				<plx:pragma type="region" data="Pattern Matches"/>
				<xsl:choose>
					<xsl:when test="$deferMatchesTo">
						<plx:callThis name="{$deferMatchesTo}">
							<plx:passParam>
								<plx:nameRef name="writer"/>
							</plx:passParam>
							<plx:passParam>
								<plx:nameRef name="snippetsDictionary"/>
							</plx:passParam>
							<plx:passParam>
								<plx:nameRef name="verbalizationContext" type="parameter"/>
							</plx:passParam>
							<plx:passParam>
								<plx:nameRef name="sign"/>
							</plx:passParam>
						</plx:callThis>
					</xsl:when>
					<xsl:otherwise>
						<xsl:apply-templates select="child::*" mode="ConstraintVerbalization">
							<xsl:with-param name="PatternGroup" select="$patternGroup"/>
							<xsl:with-param name="ResolveProxy" select="$resolveProxy"/>
							<xsl:with-param name="TopLevel" select="true()"/>
						</xsl:apply-templates>
					</xsl:otherwise>
				</xsl:choose>
				<plx:pragma type="closeRegion" data="Pattern Matches"/>
				<xsl:if test="$errorReport">
					<xsl:call-template name="CheckErrorConditions">
						<xsl:with-param name="Primary" select="false()"/>
						<xsl:with-param name="DeclareErrorOwner" select="false()"/>
						<xsl:with-param name="BeginVerbalization" select="true()"/>
					</xsl:call-template>
				</xsl:if>
				<plx:return>
					<plx:trueKeyword/>
				</plx:return>
			</plx:function>
		</plx:class>
		<xsl:if test="$isChildHelper">
			<xsl:text disable-output-escaping="yes"><![CDATA[</plx:class>]]></xsl:text>
		</xsl:if>
	</xsl:template>
	<xsl:template match="cvg:ConstrainedRoles" mode="ConstraintVerbalization">
		<xsl:param name="PatternGroup"/>
		<xsl:param name="ResolveProxy"/>
		<xsl:call-template name="ConstraintConditions">
			<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
			<xsl:with-param name="ResolveProxy" select="$ResolveProxy"/>
		</xsl:call-template>
	</xsl:template>
	<xsl:template name="InitializeFactArity">
		<plx:callInstance name="Count" type="property">
			<plx:callObject>
				<plx:nameRef name="factRoles"/>
			</plx:callObject>
		</plx:callInstance>
	</xsl:template>
	<xsl:template name="InitializeDefaultFactRoles">
		<xsl:param name="FallbackRoleCollection">
			<plx:callInstance name="RoleCollection" type="property">
				<plx:callObject>
					<plx:nameRef name="parentFact" type="local" />
				</plx:callObject>
			</plx:callInstance>
		</xsl:param>
		<plx:inlineStatement dataTypeName="IList">
			<plx:passTypeParam dataTypeName="RoleBase"/>
			<plx:conditionalOperator>
				<plx:condition>
					<plx:binaryOperator type="inequality">
						<plx:left>
							<plx:callInstance name="Count" type="property">
								<plx:callObject>
									<plx:nameRef name="allReadingOrders"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:left>
						<plx:right>
							<plx:value data="0" type="i4"/>
						</plx:right>
					</plx:binaryOperator>
				</plx:condition>
				<plx:left>
					<plx:callInstance name="RoleCollection" type="property">
						<plx:callObject>
							<plx:callInstance name=".implied"  type="indexerCall">
								<plx:callObject>
									<plx:nameRef name="allReadingOrders"/>
								</plx:callObject>
								<plx:passParam>
									<plx:value data="0" type="i4"/>
								</plx:passParam>
							</plx:callInstance>
						</plx:callObject>
					</plx:callInstance>
				</plx:left>
				<plx:right>
					<xsl:copy-of select="$FallbackRoleCollection"/>
				</plx:right>
			</plx:conditionalOperator>
		</plx:inlineStatement>
	</xsl:template>
	<xsl:template name="CheckErrorConditions">
		<xsl:param name="Primary" select="true()"/>
		<xsl:param name="Standalone" select="false()"/>
		<xsl:param name="BeginVerbalization" select="$Primary"/>
		<xsl:param name="DeclareErrorOwner" select="true()"/>
		<plx:pragma type="region" data="Prerequisite error check">
			<xsl:if test="not($Primary) or $Standalone">
				<xsl:attribute name="data">
					<xsl:text>Error report</xsl:text>
				</xsl:attribute>
			</xsl:if>
		</plx:pragma>
		<xsl:if test="$DeclareErrorOwner">
			<plx:local name="errorOwner" dataTypeName="IModelErrorOwner">
				<plx:initialize>
					<plx:cast type="testCast" dataTypeName="IModelErrorOwner">
						<plx:thisKeyword/>
					</plx:cast>
				</plx:initialize>
			</plx:local>
			<plx:local name="firstErrorPending" dataTypeName=".boolean"/>
			<xsl:if test="not($Standalone)">
				<plx:local name="blockingErrors" dataTypeName=".boolean">
					<plx:initialize>
						<plx:falseKeyword/>
					</plx:initialize>
				</plx:local>
			</xsl:if>
		</xsl:if>
		<plx:branch>
			<plx:condition>
				<plx:binaryOperator type="identityInequality">
					<plx:left>
						<plx:nameRef name="errorOwner"/>
					</plx:left>
					<plx:right>
						<plx:nullKeyword/>
					</plx:right>
				</plx:binaryOperator>
			</plx:condition>
			<xsl:call-template name="CheckErrorConditionsBody">
				<xsl:with-param name="Primary" select="$Primary"/>
				<xsl:with-param name="Standalone" select="$Standalone"/>
				<xsl:with-param name="BeginVerbalization" select="$BeginVerbalization"/>
			</xsl:call-template>
		</plx:branch>
		<plx:pragma type="closeRegion" data="Prerequisite error check">
			<xsl:if test="not($Primary) or $Standalone">
				<xsl:attribute name="data">
					<xsl:text>Error report</xsl:text>
				</xsl:attribute>
			</xsl:if>
		</plx:pragma>
		<xsl:if test="$Standalone">
			<plx:return>
				<plx:falseKeyword/>
			</plx:return>
		</xsl:if>
	</xsl:template>
	<xsl:template name="CheckErrorConditionsBody">
		<xsl:param name="Primary" select="true()"/>
		<xsl:param name="Standalone" select="false()"/>
		<xsl:param name="BeginVerbalization" select="$Primary"/>
		<xsl:variable name="stageFragment">
			<xsl:choose>
				<xsl:when test="$Primary">
					<xsl:text>Primary</xsl:text>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text>Secondary</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="stage" select="string($stageFragment)"/>
		<xsl:variable name="filterFragment">
			<xsl:choose>
				<xsl:when test="$Primary">
					<xsl:text>BlockVerbalization</xsl:text>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text>Verbalize</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="filter" select="string($filterFragment)"/>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="firstErrorPending"/>
			</plx:left>
			<plx:right>
				<plx:trueKeyword/>
			</plx:right>
		</plx:assign>
		<plx:iterator localName="error" dataTypeName="ModelError">
			<plx:initialize>
				<plx:callInstance name="GetErrorCollection">
					<plx:callObject>
						<plx:nameRef name="errorOwner"/>
					</plx:callObject>
					<plx:passParam>
						<plx:callStatic dataTypeName="ModelErrorUses" name="{$filter}" type="field"/>
					</plx:passParam>
				</plx:callInstance>
			</plx:initialize>
			<xsl:variable name="alreadyVerbalized">
				<plx:callInstance name="TestVerbalizedLocally">
					<plx:callObject>
						<plx:nameRef name="verbalizationContext" type="parameter"/>
					</plx:callObject>
					<plx:passParam>
						<plx:nameRef name="error"/>
					</plx:passParam>
				</plx:callInstance>
			</xsl:variable>
			<xsl:choose>
				<xsl:when test="$Primary">
					<xsl:if test="not($Standalone)">
						<plx:assign>
							<plx:left>
								<plx:nameRef name="blockingErrors"/>
							</plx:left>
							<plx:right>
								<plx:trueKeyword/>
							</plx:right>
						</plx:assign>
					</xsl:if>
					<plx:branch>
						<plx:condition>
							<xsl:copy-of select="$alreadyVerbalized"/>
						</plx:condition>
						<plx:continue/>
					</plx:branch>
				</xsl:when>
				<xsl:otherwise>
					<plx:local name="errorDisplayFilter" dataTypeName="ModelErrorDisplayFilter">
						<plx:initialize>
							<plx:callInstance name="ModelErrorDisplayFilter" type="property">
								<plx:callObject>
									<plx:callInstance name="Model" type="property">
										<plx:callObject>
											<plx:nameRef name="error"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:callObject>
							</plx:callInstance>
						</plx:initialize>
					</plx:local>
					<plx:branch>
						<plx:condition>
							<plx:binaryOperator type="booleanOr">
								<plx:left>
									<plx:unaryOperator type="booleanNot">
										<plx:callStatic dataTypeName="ModelError" name="IsDisplayed">
											<plx:passParam>
												<plx:nameRef name="error"/>
											</plx:passParam>
											<plx:passParam>
												<plx:nameRef name="errorDisplayFilter"/>
											</plx:passParam>
										</plx:callStatic>
									</plx:unaryOperator>
								</plx:left>
								<plx:right>
									<xsl:copy-of select="$alreadyVerbalized"/>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:continue/>
					</plx:branch>
				</xsl:otherwise>
			</xsl:choose>
			<plx:branch>
				<plx:condition>
					<plx:nameRef name="firstErrorPending"/>
				</plx:condition>
				<plx:assign>
					<plx:left>
						<plx:nameRef name="firstErrorPending"/>
					</plx:left>
					<plx:right>
						<plx:falseKeyword/>
					</plx:right>
				</plx:assign>
				<xsl:choose>
					<xsl:when test="$BeginVerbalization">
						<xsl:choose>
							<xsl:when test="$Standalone and not($Primary)">
								<plx:branch>
									<plx:condition>
										<plx:nameRef name="blockingErrorsReported"/>
									</plx:condition>
									<plx:callInstance name="WriteLine">
										<plx:callObject>
											<plx:nameRef type="parameter" name="writer"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:branch>
								<plx:fallbackBranch>
									<plx:callInstance name="BeginVerbalization">
										<plx:callObject>
											<plx:nameRef name="verbalizationContext" type="parameter"/>
										</plx:callObject>
										<plx:passParam>
											<plx:callStatic name="ErrorReport" dataTypeName="VerbalizationContent" type="field"/>
										</plx:passParam>
									</plx:callInstance>
								</plx:fallbackBranch>
							</xsl:when>
							<xsl:otherwise>
								<plx:callInstance name="BeginVerbalization">
									<plx:callObject>
										<plx:nameRef name="verbalizationContext" type="parameter"/>
									</plx:callObject>
									<plx:passParam>
										<plx:callStatic name="ErrorReport" dataTypeName="VerbalizationContent" type="field"/>
									</plx:passParam>
								</plx:callInstance>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<plx:callInstance name="WriteLine">
							<plx:callObject>
								<plx:nameRef type="parameter" name="writer"/>
							</plx:callObject>
						</plx:callInstance>
					</xsl:otherwise>
				</xsl:choose>
				<plx:callInstance name="Write">
					<plx:callObject>
						<plx:nameRef type="parameter" name="writer"/>
					</plx:callObject>
					<plx:passParam>
						<xsl:call-template name="SnippetFor">
							<xsl:with-param name="SnippetType" select="concat('ErrorOpen',$stage,'Report')"/>
							<xsl:with-param name="IsDeonticSnippet">
								<plx:falseKeyword/>
							</xsl:with-param>
							<xsl:with-param name="IsNegativeSnippet">
								<plx:falseKeyword/>
							</xsl:with-param>
						</xsl:call-template>
					</plx:passParam>
				</plx:callInstance>
			</plx:branch>
			<plx:fallbackBranch>
				<plx:callInstance name="WriteLine">
					<plx:callObject>
						<plx:nameRef type="parameter" name="writer"/>
					</plx:callObject>
				</plx:callInstance>
			</plx:fallbackBranch>
			<plx:callInstance name="Write">
				<plx:callObject>
					<plx:nameRef type="parameter" name="writer"/>
				</plx:callObject>
				<plx:passParam>
					<plx:callStatic name="Format" dataTypeName=".string">
						<plx:passParam>
							<plx:callInstance name="FormatProvider" type="property">
								<plx:callObject>
									<plx:nameRef type="parameter" name="writer"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:passParam>
						<plx:passParam>
							<xsl:call-template name="SnippetFor">
								<xsl:with-param name="SnippetType" select="concat('Error',$stage)"/>
								<xsl:with-param name="IsDeonticSnippet">
									<plx:falseKeyword/>
								</xsl:with-param>
								<xsl:with-param name="IsNegativeSnippet">
									<plx:falseKeyword/>
								</xsl:with-param>
							</xsl:call-template>
						</plx:passParam>
						<plx:passParam>
							<plx:callInstance name="ErrorText" type="property">
								<plx:callObject>
									<plx:nameRef name="error"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:passParam>
						<plx:passParam>
							<plx:callInstance name="ToString">
								<plx:callObject>
									<plx:callInstance name="Id" type="property">
										<plx:callObject>
											<plx:nameRef name="error"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:callObject>
								<plx:passParam>
									<plx:string data="D"/>
								</plx:passParam>
							</plx:callInstance>
						</plx:passParam>
					</plx:callStatic>
				</plx:passParam>
			</plx:callInstance>
		</plx:iterator>
		<plx:branch>
			<plx:condition>
				<plx:unaryOperator type="booleanNot">
					<plx:nameRef name="firstErrorPending"/>
				</plx:unaryOperator>
			</plx:condition>
			<plx:callInstance name="Write">
				<plx:callObject>
					<plx:nameRef type="parameter" name="writer"/>
				</plx:callObject>
				<plx:passParam>
					<xsl:call-template name="SnippetFor">
						<xsl:with-param name="SnippetType" select="concat('ErrorClose',$stage,'Report')"/>
						<xsl:with-param name="IsDeonticSnippet">
							<plx:falseKeyword/>
						</xsl:with-param>
						<xsl:with-param name="IsNegativeSnippet">
							<plx:falseKeyword/>
						</xsl:with-param>
					</xsl:call-template>
				</plx:passParam>
			</plx:callInstance>
		</plx:branch>
		<xsl:choose>
			<xsl:when test="$Primary and not($Standalone)">
				<plx:branch>
					<plx:condition>
						<plx:nameRef name="blockingErrors"/>
					</plx:condition>
					<xsl:call-template name="CheckErrorConditionsBody">
						<xsl:with-param name="Primary" select="false()"/>
					</xsl:call-template>
					<plx:return>
						<plx:trueKeyword/>
					</plx:return>
				</plx:branch>
			</xsl:when>
		</xsl:choose>
		<xsl:if test="$Standalone and $Primary">
			<plx:local name="blockingErrorsReported" dataTypeName=".boolean">
				<plx:initialize>
					<plx:unaryOperator type="booleanNot">
						<plx:nameRef name="firstErrorPending"/>
					</plx:unaryOperator>
				</plx:initialize>
			</plx:local>
			<xsl:call-template name="CheckErrorConditionsBody">
				<xsl:with-param name="Primary" select="false()"/>
				<xsl:with-param name="Standalone" select="true()"/>
				<xsl:with-param name="BeginVerbalization" select="$BeginVerbalization"/>
			</xsl:call-template>
			<plx:return>
				<plx:binaryOperator type="booleanOr">
					<plx:left>
						<plx:nameRef name="blockingErrorsReported"/>
					</plx:left>
					<plx:right>
						<plx:nameRef name="firstErrorPending"/>
					</plx:right>
				</plx:binaryOperator>
			</plx:return>
		</xsl:if>
	</xsl:template>
	<!-- Handle the span constraint condition attribute -->
	<xsl:template match="@span" mode="ConstraintConditionOperator">
		<xsl:param name="PatternGroup"/>
		<xsl:variable name="compareToNameFragment">
			<xsl:choose>
				<xsl:when test="$PatternGroup='SetConstraint' and parent::*[@constraintArity=1 or @factCount=1]">
					<xsl:text>constraintRoleArity</xsl:text>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text>includedArity</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="compareToName" select="string($compareToNameFragment)"/>
		<xsl:choose>
			<xsl:when test=".='all'">
				<plx:binaryOperator type="equality">
					<plx:left>
						<plx:nameRef name="factArity"/>
					</plx:left>
					<plx:right>
						<plx:nameRef name="{$compareToName}"/>
					</plx:right>
				</plx:binaryOperator>
			</xsl:when>
			<xsl:when test=".='allMinusOne'">
				<plx:binaryOperator type="equality">
					<plx:left>
						<plx:binaryOperator type="subtract">
							<plx:left>
								<plx:nameRef name="factArity"/>
							</plx:left>
							<plx:right>
								<plx:value type="i4" data="1"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:left>
					<plx:right>
						<plx:nameRef name="{$compareToName}"/>
					</plx:right>
				</plx:binaryOperator>
			</xsl:when>
			<xsl:when test=".='allMinusMoreThanOne'">
				<plx:binaryOperator type="greaterThan">
					<plx:left>
						<plx:binaryOperator type="subtract">
							<plx:left>
								<plx:nameRef name="factArity"/>
							</plx:left>
							<plx:right>
								<plx:nameRef name="{$compareToName}"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:left>
					<plx:right>
						<plx:value type="i4" data="1"/>
					</plx:right>
				</plx:binaryOperator>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="TerminateForInvalidAttribute">
					<xsl:with-param name="MessageText">Unrecognized value for span condition attribute</xsl:with-param>
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- Handle the factCount constraint condition attribute -->
	<xsl:template match="@factCount" mode="ConstraintConditionOperator">
		<plx:binaryOperator type="equality">
			<plx:left>
				<plx:nameRef name="allFactsCount"/>
			</plx:left>
			<plx:right>
				<plx:value type="i4" data="{.}"/>
			</plx:right>
		</plx:binaryOperator>
	</xsl:template>
	<!-- Handle the constraintArity constraint condition attribute -->
	<xsl:template match="@constraintArity" mode="ConstraintConditionOperator">
		<xsl:param name="PatternGroup"/>
		<plx:binaryOperator type="equality">
			<plx:left>
				<xsl:choose>
					<xsl:when test="$PatternGroup='InternalConstraint'">
						<plx:nameRef name="includedArity"/>
					</xsl:when>
					<xsl:otherwise>
						<plx:nameRef name="constraintRoleArity"/>
					</xsl:otherwise>
				</xsl:choose>
			</plx:left>
			<plx:right>
				<plx:value type="i4" data="{.}"/>
			</plx:right>
		</plx:binaryOperator>
	</xsl:template>
	<!-- Handle the constraintArity constraint condition attribute -->
	<xsl:template match="@minConstraintArity" mode="ConstraintConditionOperator">
		<xsl:param name="PatternGroup"/>
		<plx:binaryOperator type="greaterThanOrEqual">
			<plx:left>
				<xsl:choose>
					<xsl:when test="$PatternGroup='InternalConstraint'">
						<plx:nameRef name="includedArity"/>
					</xsl:when>
					<xsl:otherwise>
						<plx:nameRef name="constraintRoleArity"/>
					</xsl:otherwise>
				</xsl:choose>
			</plx:left>
			<plx:right>
				<plx:value type="i4" data="{.}"/>
			</plx:right>
		</plx:binaryOperator>
	</xsl:template>
	<!-- Handle the factArity constraint condition attribute -->
	<xsl:template match="@factArity" mode="ConstraintConditionOperator">
		<plx:binaryOperator type="equality">
			<plx:left>
				<plx:nameRef name="factArity"/>
			</plx:left>
			<plx:right>
				<plx:value type="i4" data="{.}"/>
			</plx:right>
		</plx:binaryOperator>
	</xsl:template>
	<!-- Handle the minFactArity constraint condition attribute -->
	<xsl:template match="@minFactArity" mode="ConstraintConditionOperator">
		<xsl:param name="PatternGroup"/>
		<plx:binaryOperator type="greaterThanOrEqual">
			<plx:left>
				<xsl:choose>
					<xsl:when test="$PatternGroup='InternalConstraint'">
						<plx:nameRef name="factArity"/>
					</xsl:when>
					<xsl:otherwise>
						<plx:nameRef name="minFactArity"/>
					</xsl:otherwise>
				</xsl:choose>
			</plx:left>
			<plx:right>
				<plx:value type="i4" data="{.}"/>
			</plx:right>
		</plx:binaryOperator>
	</xsl:template>
	<!-- Handle the maxFactArity constraint condition attribute -->
	<xsl:template match="@maxFactArity" mode="ConstraintConditionOperator">
		<xsl:param name="PatternGroup"/>
		<plx:binaryOperator type="lessThanOrEqual">
			<plx:left>
				<xsl:choose>
					<xsl:when test="$PatternGroup='InternalConstraint'">
						<plx:nameRef name="factArity"/>
					</xsl:when>
					<xsl:otherwise>
						<plx:nameRef name="maxFactArity"/>
					</xsl:otherwise>
				</xsl:choose>
			</plx:left>
			<plx:right>
				<plx:value type="i4" data="{.}"/>
			</plx:right>
		</plx:binaryOperator>
	</xsl:template>
	<!-- Handle the sign constraint condition attributes -->
	<xsl:template match="@sign" mode="ConstraintConditionOperator">
		<xsl:choose>
			<xsl:when test=".='negative'">
				<plx:nameRef name="isNegative"/>
			</xsl:when>
			<xsl:when test=".='positive'">
				<plx:unaryOperator type="booleanNot">
					<plx:nameRef name="isNegative"/>
				</plx:unaryOperator>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="TerminateForInvalidAttribute">
					<xsl:with-param name="MessageText">Unrecognized value for sign condition attribute</xsl:with-param>
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="@ringType" mode="ConstraintConditionOperator">
		<plx:binaryOperator type="equality">
			<plx:left>
				<plx:callThis name="RingType" type="property"/>
			</plx:left>
			<plx:right>
				<plx:callStatic dataTypeName="RingConstraintType" name="{.}" type="field"/>
			</plx:right>
		</plx:binaryOperator>
	</xsl:template>
	<xsl:template match="@columnArity" mode="ConstraintConditionOperator">
		<plx:binaryOperator type="equality">
			<plx:left>
				<plx:nameRef name="columnArity" type="local"/>
			</plx:left>
			<plx:right>
				<plx:value data="{.}" type="i4"/>
			</plx:right>
		</plx:binaryOperator>
	</xsl:template>
	<xsl:template match="@minColumnArity" mode="ConstraintConditionOperator">
		<plx:binaryOperator type="greaterThanOrEqual">
			<plx:left>
				<plx:nameRef name="columnArity" type="local"/>
			</plx:left>
			<plx:right>
				<plx:value data="{.}" type="i4"/>
			</plx:right>
		</plx:binaryOperator>
	</xsl:template>
	<xsl:template match="@frequencyRangePattern" mode="ConstraintConditionOperator">
		<xsl:choose>
			<xsl:when test=".='MinUnbounded'">
				<plx:binaryOperator type="booleanAnd">
					<plx:left>
						<plx:binaryOperator type="equality">
							<plx:left>
								<plx:callThis name="MinFrequency" type="property"/>
							</plx:left>
							<plx:right>
								<plx:value data="1" type="i4"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:left>
					<plx:right>
						<plx:binaryOperator type="inequality">
							<plx:left>
								<plx:callThis name="MaxFrequency" type="property"/>
							</plx:left>
							<plx:right>
								<plx:value data="0" type="i4"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:right>
				</plx:binaryOperator>
			</xsl:when>
			<xsl:when test=".='MaxUnbounded'">
				<plx:binaryOperator type="equality">
					<plx:left>
						<plx:callThis name="MaxFrequency" type="property"/>
					</plx:left>
					<plx:right>
						<plx:value data="0" type="i4"/>
					</plx:right>
				</plx:binaryOperator>
			</xsl:when>
			<xsl:when test=".='Exact'">
				<plx:binaryOperator type="equality">
					<plx:left>
						<plx:callThis name="MinFrequency" type="property"/>
					</plx:left>
					<plx:right>
						<plx:callThis name="MaxFrequency" type="property"/>
					</plx:right>
				</plx:binaryOperator>
			</xsl:when>
			<xsl:when test=".='Both'">
				<plx:binaryOperator type="booleanAnd">
					<plx:left>
						<plx:binaryOperator type="inequality">
							<plx:left>
								<plx:callThis name="MinFrequency" type="property"/>
							</plx:left>
							<plx:right>
								<plx:callThis name="MaxFrequency" type="property"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:left>
					<plx:right>
						<plx:binaryOperator type="inequality">
							<plx:left>
								<plx:callThis name="MaxFrequency" type="property"/>
							</plx:left>
							<plx:right>
								<plx:value data="0" type="i4"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:right>
				</plx:binaryOperator>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="@sameConstraintRolePlayers" mode="ConstraintConditionOperator">
		<xsl:param name="PatternGroup"/>
		<xsl:if test="not(($PatternGroup='InternalSetConstraint' or $PatternGroup='SetConstraint'))">
			<xsl:message terminate="yes">ConstraintRoles/@sameConstraintRolePlayers can be set only for binary set constraints.</xsl:message>
		</xsl:if>
		<xsl:choose>
			<xsl:when test="parent::*[@constraintArity[.=2]]">
				<plx:binaryOperator type="identityEquality">
					<plx:left>
						<plx:callInstance name="RolePlayer" type="property">
							<plx:callObject>
								<plx:callInstance name=".implied" type="indexerCall">
									<plx:callObject>
										<plx:nameRef name="allConstraintRoles"/>
									</plx:callObject>
									<plx:passParam>
										<plx:value data="0" type="i4"/>
									</plx:passParam>
								</plx:callInstance>
							</plx:callObject>
						</plx:callInstance>
					</plx:left>
					<plx:right>
						<plx:callInstance name="RolePlayer" type="property">
							<plx:callObject>
								<plx:callInstance name=".implied" type="indexerCall">
									<plx:callObject>
										<plx:nameRef name="allConstraintRoles"/>
									</plx:callObject>
									<plx:passParam>
										<plx:value data="1" type="i4"/>
									</plx:passParam>
								</plx:callInstance>
							</plx:callObject>
						</plx:callInstance>
					</plx:right>
				</plx:binaryOperator>
			</xsl:when>
			<xsl:otherwise>
				<plx:binaryOperator type="identityInequality">
					<plx:left>
						<plx:nameRef name="singleConstrainedRolePlayerType"/>
					</plx:left>
					<plx:right>
						<plx:nullKeyword/>
					</plx:right>
				</plx:binaryOperator>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="@rolePlayerLimitedToConstraintRoles" mode="ConstraintConditionOperator">
		<xsl:param name="PatternGroup"/>
		<xsl:if test="not(($PatternGroup='InternalSetConstraint' or $PatternGroup='SetConstraint') and parent::*[@sameConstraintRolePlayers[.='true' or .='1']])">
			<xsl:message terminate="yes">ConstraintRoles/@rolePlayerLimitedToConstraintRoles can be set only for set constraints with the same role players.</xsl:message>
		</xsl:if>
		<plx:callStatic name="EnumerableTrueForAll" dataTypeName="Utility" dataTypeQualifier="ORMSolutions.ORMArchitect.Framework">
			<plx:passMemberTypeParam dataTypeName="RoleBase"/>
			<plx:passParam>
				<plx:nameRef name="factRoles"/>
			</plx:passParam>
			<plx:passParam>
				<plx:anonymousFunction>
					<plx:param name="matchRoleBase" dataTypeName="RoleBase"/>
					<plx:returns dataTypeName=".boolean"/>
					<plx:local name="matchRole" dataTypeName="Role">
						<plx:initialize>
							<plx:callInstance name="Role" type="property">
								<plx:callObject>
									<plx:nameRef name="matchRoleBase" type="parameter"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:initialize>
					</plx:local>
					<plx:return>
						<plx:binaryOperator type="booleanOr">
							<plx:left>
								<plx:callInstance name="Contains">
									<plx:callObject>
										<plx:nameRef name="allConstraintRoles"/>
									</plx:callObject>
									<plx:passParam>
										<plx:nameRef name="matchRole"/>
									</plx:passParam>
								</plx:callInstance>
							</plx:left>
							<plx:right>
								<plx:binaryOperator type="identityInequality">
									<plx:left>
										<plx:callInstance name="RolePlayer" type="property">
											<plx:callObject>
												<plx:callInstance name=".implied" type="indexerCall">
													<plx:callObject>
														<plx:nameRef name="allConstraintRoles"/>
													</plx:callObject>
													<plx:passParam>
														<plx:value data="0" type="i4"/>
													</plx:passParam>
												</plx:callInstance>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:callInstance name="RolePlayer" type="property">
											<plx:callObject>
												<plx:nameRef name="matchRole"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:right>
								</plx:binaryOperator>
							</plx:right>
						</plx:binaryOperator>
					</plx:return>
				</plx:anonymousFunction>
			</plx:passParam>
		</plx:callStatic>
	</xsl:template>
	<xsl:template match="@trivialOppositeRolePath" mode="ConstraintConditionOperator">
		<xsl:if test=".='true' or .='1'">
			<plx:nameRef name="isTrivialOppositeRolePath"/>
		</xsl:if>
	</xsl:template>
	<xsl:template match="@blockHelpers" mode="ConstraintConditionOperator">
		<!-- Empty template, this adds additional helpers inside the conditional block, not a condition. -->
	</xsl:template>
	<!-- Terminate processing if we see an unrecognized operator -->
	<xsl:template match="@*" mode="ConstraintConditionOperator">
		<xsl:call-template name="TerminateForInvalidAttribute">
			<xsl:with-param name="MessageText">Unrecognized constraint condition attribute</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<!-- Terminate processing for unrecognized attribute or attribute value-->
	<xsl:template name="TerminateForInvalidAttribute">
		<xsl:param name="MessageText"/>
		<xsl:message terminate="yes">
			<xsl:value-of select="$MessageText"/>
			<xsl:text>: </xsl:text>
			<xsl:value-of select="local-name()"/>
			<xsl:text>="</xsl:text>
			<xsl:value-of select="."/>
			<xsl:text>"</xsl:text>
		</xsl:message>
	</xsl:template>
	<!-- Helper template for adding addition variable context for regions in the code -->
	<xsl:template name="ApplyBlockHelpers">
		<xsl:param name="PatternGroup"/>
		<xsl:variable name="blockHelpers" select="normalize-space(@blockHelpers)"/>
		<xsl:if test="$blockHelpers">
			<xsl:variable name="helpersFragment">
				<xsl:call-template name="SplitList">
					<xsl:with-param name="ItemList" select="$blockHelpers"/>
				</xsl:call-template>
			</xsl:variable>
			<xsl:apply-templates select="exsl:node-set($helpersFragment)/child::*" mode="BlockHelper">
				<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
			</xsl:apply-templates>
		</xsl:if>
	</xsl:template>
	<xsl:template match="*[.='pathVerbalizer']" mode="BlockHelper">
		<xsl:param name="PatternGroup"/>
		<xsl:if test="$PatternGroup!='SetConstraint'">
			<xsl:call-template name="TerminateForInvalidAttribute">
				<xsl:with-param name="MessageText">PathVerbalizer block helper applies to the SetConstraint pattern</xsl:with-param>
			</xsl:call-template>
		</xsl:if>
		<plx:local name="pathVerbalizer" dataTypeName="RolePathVerbalizer">
			<plx:initialize>
				<plx:callStatic name="Create" dataTypeName="RolePathVerbalizer">
					<plx:passParam>
						<plx:thisKeyword/>
					</plx:passParam>
					<plx:passParam>
						<plx:callNew dataTypeName="StandardRolePathRenderer">
							<plx:passParam>
								<plx:nameRef name="snippets"/>
							</plx:passParam>
							<plx:passParam>
								<plx:nameRef name="verbalizationContext" type="parameter"/>
							</plx:passParam>
							<plx:passParam>
								<plx:callInstance name="FormatProvider" type="property">
									<plx:callObject>
										<plx:nameRef name="writer" type="parameter"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:passParam>
						</plx:callNew>
					</plx:passParam>
				</plx:callStatic>
			</plx:initialize>
		</plx:local>
		<plx:local name="includedConstraintRoles" dataTypeName="IList">
			<plx:passTypeParam dataTypeName="ConstraintRoleSequenceHasRole"/>
			<plx:initialize>
				<plx:callStatic dataTypeName="ConstraintRoleSequenceHasRole" name="GetLinksToRoleCollection">
					<plx:passParam>
						<plx:thisKeyword/>
					</plx:passParam>
				</plx:callStatic>
			</plx:initialize>
		</plx:local>
	</xsl:template>
	<xsl:template match="*[.='preProjectionKeys']" mode="BlockHelper">
		<xsl:param name="PatternGroup"/>
		<xsl:if test="$PatternGroup!='SetConstraint'">
			<xsl:call-template name="TerminateForInvalidAttribute">
				<xsl:with-param name="MessageText">PreProjectionKeys block helper applies to the SetConstraint pattern</xsl:with-param>
			</xsl:call-template>
		</xsl:if>
		<plx:local name="preProjectionKeys" dataTypeName="IList">
			<plx:passTypeParam dataTypeName=".object"/>
			<plx:initialize>
				<plx:callInstance name="GetPreProjectionPrimaryNodeKeys">
					<plx:callObject>
						<plx:nameRef name="pathVerbalizer"/>
					</plx:callObject>
					<plx:passParam>
						<plx:nameRef name="includedConstraintRoles"/>
					</plx:passParam>
				</plx:callInstance>
			</plx:initialize>
		</plx:local>
		<plx:local name="preProjectionKeyCount" dataTypeName=".i4">
			<plx:initialize>
				<plx:inlineStatement dataTypeName=".i4">
					<plx:conditionalOperator>
						<plx:condition>
							<plx:binaryOperator type="identityInequality">
								<plx:left>
									<plx:nameRef name="preProjectionKeys"/>
								</plx:left>
								<plx:right>
									<plx:nullKeyword/>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:left>
							<plx:callInstance name="Count" type="property">
								<plx:callObject>
									<plx:nameRef name="preProjectionKeys"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:left>
						<plx:right>
							<plx:value type="i4" data="0"/>
						</plx:right>
					</plx:conditionalOperator>
				</plx:inlineStatement>
			</plx:initialize>
		</plx:local>
	</xsl:template>
	<xsl:template match="*" mode="BlockHelper">
		<xsl:call-template name="TerminateForInvalidAttribute">
			<xsl:with-param name="MessageText">Unrecognized Block Helper</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<!-- Helper template for spitting conditions based on specified conditions. All conditions
		 are combined with an and operator, and are given priority based on the order they
		 appear in the data file. The assumption is made that the unconstrained condition
		 is sorted last. -->
	<xsl:template name="ConstraintConditions">
		<xsl:param name="PatternGroup"/>
		<xsl:param name="ResolveProxy"/>
		<xsl:variable name="fallback" select="boolean(preceding-sibling::cvg:ConstrainedRoles)"/>
		<xsl:variable name="lastBlock" select="position()=last()"/>
		<xsl:variable name="conditionTestFragment">
			<xsl:variable name="conditionOperatorsFragment">
				<xsl:apply-templates select="@*" mode="ConstraintConditionOperator">
					<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
				</xsl:apply-templates>
			</xsl:variable>
			<xsl:variable name="conditionOperators" select="exsl:node-set($conditionOperatorsFragment)/child::*"/>
			<xsl:if test="$conditionOperators">
				<xsl:call-template name="CombineElements">
					<xsl:with-param name="OperatorType" select="'booleanAnd'"/>
					<xsl:with-param name="Elements" select="$conditionOperators"/>
				</xsl:call-template>
			</xsl:if>
		</xsl:variable>
		<xsl:variable name="conditionTest" select="exsl:node-set($conditionTestFragment)/child::*"/>
		<xsl:variable name="forwardPatternGroup">
			<xsl:choose>
				<xsl:when test="$conditionTest and $PatternGroup='SetConstraint' and (@constraintArity=1 or @factCount=1)">
					<xsl:text>InternalSetConstraint</xsl:text>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$PatternGroup"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="$conditionTest">
				<xsl:variable name="branchType">
					<xsl:choose>
						<xsl:when test="$fallback">
							<xsl:text>plx:alternateBranch</xsl:text>
						</xsl:when>
						<xsl:otherwise>
							<xsl:text>plx:branch</xsl:text>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:element name="{$branchType}">
					<plx:condition>
						<xsl:copy-of select="$conditionTest"/>
					</plx:condition>
					<xsl:call-template name="ConstraintBodyContent">
						<xsl:with-param name="PatternGroup" select="string($forwardPatternGroup)"/>
						<xsl:with-param name="ResolveProxy" select="$ResolveProxy"/>
					</xsl:call-template>
				</xsl:element>
				<xsl:if test="position()=last()">
					<xsl:if test="self::*[@sign] or preceding-sibling::cvg:ConstrainedRoles[@sign]">
						<xsl:call-template name="DeferToOppositeSign"/>
					</xsl:if>
				</xsl:if>
			</xsl:when>
			<xsl:when test="$fallback">
				<plx:fallbackBranch>
					<xsl:call-template name="ConstraintBodyContent">
						<xsl:with-param name="PatternGroup" select="string($forwardPatternGroup)"/>
						<xsl:with-param name="ResolveProxy" select="$ResolveProxy"/>
					</xsl:call-template>
				</plx:fallbackBranch>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="ConstraintBodyContent">
					<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
					<xsl:with-param name="ResolveProxy" select="$ResolveProxy"/>
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="DeferToOppositeSign">
		<plx:alternateBranch>
			<plx:condition>
				<plx:binaryOperator type="inequality">
					<plx:left>
						<plx:value data="0" type="i4"/>
					</plx:left>
					<plx:right>
						<plx:binaryOperator type="bitwiseAnd">
							<plx:left>
								<plx:nameRef name="sign" type="parameter"/>
							</plx:left>
							<plx:right>
								<plx:callStatic dataTypeName="VerbalizationSign" name="AttemptOppositeSign" type="field"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:right>
				</plx:binaryOperator>
			</plx:condition>
			<plx:return>
				<plx:callThis name="GetVerbalization">
					<plx:passParam>
						<plx:nameRef name="writer" type="parameter"/>
					</plx:passParam>
					<plx:passParam>
						<plx:nameRef name="snippetsDictionary" type="parameter"/>
					</plx:passParam>
					<plx:passParam>
						<plx:nameRef name="verbalizationContext" type="parameter"/>
					</plx:passParam>
					<plx:passParam>
						<plx:inlineStatement dataTypeName="VerbalizationSign">
							<plx:conditionalOperator>
								<plx:condition>
									<plx:nameRef name="isNegative"/>
								</plx:condition>
								<plx:left>
									<plx:callStatic dataTypeName="VerbalizationSign" name="Positive" type="field"/>
								</plx:left>
								<plx:right>
									<plx:callStatic dataTypeName="VerbalizationSign" name="Negative" type="field"/>
								</plx:right>
							</plx:conditionalOperator>
						</plx:inlineStatement>
					</plx:passParam>
				</plx:callThis>
			</plx:return>
		</plx:alternateBranch>
	</xsl:template>
	<!-- Helper template to combine expressions using the specified OperatorType. An external
		 call should fire this from inside a for each for the first element, it will then
		 recurse to pick up remaining elements -->
	<xsl:template name="CombineElements">
		<xsl:param name="OperatorType"/>
		<xsl:param name="Elements"/>
		<xsl:param name="Position" select="1"/>
		<xsl:param name="Count" select="count($Elements)"/>
		<xsl:choose>
			<xsl:when test="$Position=$Count">
				<xsl:copy-of select="$Elements[$Position]"/>
			</xsl:when>
			<xsl:otherwise>
				<plx:binaryOperator type="{$OperatorType}">
					<plx:left>
						<xsl:copy-of select="$Elements[$Position]"/>
					</plx:left>
					<plx:right>
						<xsl:call-template name="CombineElements">
							<xsl:with-param name="OperatorType" select="$OperatorType"/>
							<xsl:with-param name="Elements" select="$Elements"/>
							<xsl:with-param name="Position" select="$Position + 1"/>
							<xsl:with-param name="Count" select="$Count"/>
						</xsl:call-template>
					</plx:right>
				</plx:binaryOperator>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="PopulateBasicRoleReplacements_FormatNormal">
		<xsl:param name="ObjectTypeExpression"/>
		<plx:callStatic name="Format" dataTypeName=".string">
			<plx:passParam>
				<plx:callInstance name="FormatProvider" type="property">
					<plx:callObject>
						<plx:nameRef type="parameter" name="writer"/>
					</plx:callObject>
				</plx:callInstance>
			</plx:passParam>
			<plx:passParam>
				<xsl:call-template name="SnippetFor">
					<xsl:with-param name="SnippetType" select="'ObjectType'"/>
				</xsl:call-template>
			</plx:passParam>
			<plx:passParam>
				<plx:callStatic name="NormalizeObjectTypeName" dataTypeName="VerbalizationHelper">
					<plx:passParam>
						<xsl:copy-of select="$ObjectTypeExpression"/>
					</plx:passParam>
					<plx:passParam>
						<plx:callInstance name="VerbalizationOptions" type="property">
							<plx:callObject>
								<plx:nameRef name="verbalizationContext" type="parameter"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:passParam>
				</plx:callStatic>
			</plx:passParam>
			<plx:passParam>
				<plx:callInstance name="ToString">
					<plx:callObject>
						<plx:callInstance name="Id" type="property">
							<plx:callObject>
								<xsl:copy-of select="$ObjectTypeExpression"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:callObject>
					<plx:passParam>
						<plx:string data="D"/>
					</plx:passParam>
				</plx:callInstance>
			</plx:passParam>
		</plx:callStatic>
	</xsl:template>
	<xsl:template name="PopulateBasicRoleReplacements_FormatSubscript">
		<xsl:param name="ObjectTypeExpression"/>
		<xsl:param name="DynamicSubscripts" select="false()"/>
		<xsl:param name="CustomSubscripts" select="true()"/>
		<xsl:variable name="sharedParamsFragment">
			<plx:passParam>
				<xsl:call-template name="SnippetFor">
					<xsl:with-param name="SnippetType" select="'ObjectTypeWithSubscript'"/>
				</xsl:call-template>
			</plx:passParam>
			<plx:passParam>
				<plx:callStatic name="NormalizeObjectTypeName" dataTypeName="VerbalizationHelper">
					<plx:passParam>
						<xsl:copy-of select="$ObjectTypeExpression"/>
					</plx:passParam>
					<plx:passParam>
						<plx:callInstance name="VerbalizationOptions" type="property">
							<plx:callObject>
								<plx:nameRef name="verbalizationContext" type="parameter"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:passParam>
				</plx:callStatic>
			</plx:passParam>
			<plx:passParam>
				<plx:callInstance name="ToString">
					<plx:callObject>
						<plx:callInstance name="Id" type="property">
							<plx:callObject>
								<xsl:copy-of select="$ObjectTypeExpression"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:callObject>
					<plx:passParam>
						<plx:string data="D"/>
					</plx:passParam>
				</plx:callInstance>
			</plx:passParam>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="$DynamicSubscripts or not($CustomSubscripts)">
				<plx:callStatic name="Format" dataTypeName=".string">
					<plx:passParam>
						<plx:callInstance name="FormatProvider" type="property">
							<plx:callObject>
								<plx:nameRef type="parameter" name="writer"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:passParam>
					<xsl:copy-of select="$sharedParamsFragment"/>
					<plx:passParam>
						<xsl:choose>
							<xsl:when test="$DynamicSubscripts">
								<plx:string data="{{0}}"/>
							</xsl:when>
							<xsl:otherwise>
								<plx:binaryOperator type="add">
									<plx:left>
										<plx:nameRef name="subscript"/>
									</plx:left>
									<plx:right>
										<plx:value type="i4" data="1"/>
									</plx:right>
								</plx:binaryOperator>
							</xsl:otherwise>
						</xsl:choose>
					</plx:passParam>
				</plx:callStatic>
			</xsl:when>
			<xsl:otherwise>
				<plx:callInstance name="PrepareSubscriptFormatString">
					<plx:callObject>
						<plx:nameRef name="subscripter"/>
					</plx:callObject>
					<xsl:copy-of select="$sharedParamsFragment"/>
				</plx:callInstance>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="PopulateBasicRoleReplacements_Assign_Body">
		<xsl:param name="ObjectTypeExpression"/>
		<xsl:param name="Append" select="false()"/>
		<xsl:param name="SubscriptConditions" select="true()"/>
		<xsl:param name="CustomSubscripts" select="false()"/>
		<xsl:param name="DynamicSubscripts" select="false()"/>
		<xsl:variable name="normalFormat">
			<xsl:call-template name="PopulateBasicRoleReplacements_FormatNormal">
				<xsl:with-param name="ObjectTypeExpression" select="$ObjectTypeExpression"/>
			</xsl:call-template>
		</xsl:variable>
		<xsl:variable name="nonSubscriptedObjectTypeBody">
			<xsl:choose>
				<xsl:when test="$Append">
					<plx:callInstance name="Append">
						<plx:callObject>
							<plx:nameRef name="sbTemp"/>
						</plx:callObject>
						<plx:passParam>
							<xsl:copy-of select="$normalFormat"/>
						</plx:passParam>
					</plx:callInstance>
				</xsl:when>
				<xsl:otherwise>
					<plx:assign>
						<plx:left>
							<plx:nameRef name="basicReplacement"/>
						</plx:left>
						<plx:right>
							<xsl:copy-of select="$normalFormat"/>
						</plx:right>
					</plx:assign>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="$CustomSubscripts">
				<xsl:if test="$Append">
					<xsl:message terminate="yes">Append mode not supported with customizable subscripts.</xsl:message>
				</xsl:if>
				<xsl:copy-of select="$nonSubscriptedObjectTypeBody"/>
				<plx:branch>
					<plx:condition>
						<plx:nameRef name="useSubscript"/>
					</plx:condition>
					<plx:assign>
						<plx:left>
							<plx:nameRef name="basicSubscriptedReplacement"/>
						</plx:left>
						<plx:right>
							<xsl:call-template name="PopulateBasicRoleReplacements_FormatSubscript">
								<xsl:with-param name="ObjectTypeExpression" select="$ObjectTypeExpression"/>
							</xsl:call-template>
						</plx:right>
					</plx:assign>
				</plx:branch>
				<xsl:if test="$DynamicSubscripts">
					<plx:assign>
						<plx:left>
							<plx:nameRef name="basicDynamicSubscriptedReplacement"/>
						</plx:left>
						<plx:right>
							<xsl:call-template name="PopulateBasicRoleReplacements_FormatSubscript">
								<xsl:with-param name="ObjectTypeExpression" select="$ObjectTypeExpression"/>
								<xsl:with-param name="DynamicSubscripts" select="true()"/>
							</xsl:call-template>
						</plx:right>
					</plx:assign>
				</xsl:if>
			</xsl:when>
			<xsl:when test="$SubscriptConditions">
				<xsl:variable name="subscriptFormat">
					<xsl:call-template name="PopulateBasicRoleReplacements_FormatSubscript">
						<xsl:with-param name="ObjectTypeExpression" select="$ObjectTypeExpression"/>
						<xsl:with-param name="CustomSubscripts" select="$CustomSubscripts"/>
					</xsl:call-template>
				</xsl:variable>
				<plx:branch>
					<plx:condition>
						<plx:nameRef name="useSubscript"/>
					</plx:condition>
					<xsl:choose>
						<xsl:when test="$Append">
							<plx:callInstance name="Append">
								<plx:callObject>
									<plx:nameRef name="sbTemp"/>
								</plx:callObject>
								<plx:passParam>
									<xsl:copy-of select="$subscriptFormat"/>
								</plx:passParam>
							</plx:callInstance>
						</xsl:when>
						<xsl:otherwise>
							<plx:assign>
								<plx:left>
									<plx:nameRef name="basicReplacement"/>
								</plx:left>
								<plx:right>
									<xsl:copy-of select="$subscriptFormat"/>
								</plx:right>
							</plx:assign>
						</xsl:otherwise>
					</xsl:choose>
				</plx:branch>
				<plx:fallbackBranch>
					<xsl:copy-of select="$nonSubscriptedObjectTypeBody"/>
				</plx:fallbackBranch>
			</xsl:when>
			<xsl:otherwise>
				<xsl:copy-of select="$nonSubscriptedObjectTypeBody"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="PopulateBasicRoleReplacements_Assign">
		<xsl:param name="CompatibleColumns"/>
		<xsl:param name="PatternGroup"/>
		<xsl:param name="SubscriptConditions" select="true()"/>
		<xsl:param name="CustomSubscripts" select="false()"/>
		<xsl:param name="DynamicSubscripts" select="false()"/>
		<xsl:choose>
			<xsl:when test="$CompatibleColumns">
				<xsl:choose>
					<xsl:when test="$PatternGroup='SetConstraint'">
						<plx:branch>
							<plx:condition>
								<plx:callInstance name="Contains">
									<plx:callObject>
										<plx:nameRef name="allConstraintRoles"/>
									</plx:callObject>
									<plx:passParam>
										<plx:nameRef name="factRole"/>
									</plx:passParam>
								</plx:callInstance>
							</plx:condition>
							<plx:local name="compatibleTypesCount" dataTypeName=".i4">
								<plx:initialize>
									<plx:callInstance name="Length" type="property">
										<plx:callObject>
											<plx:nameRef name="compatibleTypes"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:initialize>
							</plx:local>
							<plx:branch>
								<plx:condition>
									<plx:binaryOperator type="equality">
										<plx:left>
											<plx:nameRef name="compatibleTypesCount"/>
										</plx:left>
										<plx:right>
											<plx:value data="1" type="i4"/>
										</plx:right>
									</plx:binaryOperator>
								</plx:condition>
								<xsl:call-template name="PopulateBasicRoleReplacements_Assign_Body">
									<xsl:with-param name="ObjectTypeExpression">
										<plx:callInstance name=".implied" type="arrayIndexer">
											<plx:callObject>
												<plx:nameRef name="compatibleTypes"/>
											</plx:callObject>
											<plx:passParam>
												<plx:value data="0" type="i4"/>
											</plx:passParam>
										</plx:callInstance>
									</xsl:with-param>
									<xsl:with-param name="SubscriptConditions" select="$SubscriptConditions"/>
									<xsl:with-param name="CustomSubscripts" select="$CustomSubscripts"/>
									<xsl:with-param name="DynamicSubscripts" select="$DynamicSubscripts"/>
								</xsl:call-template>
							</plx:branch>
							<plx:fallbackBranch>
								<xsl:call-template name="EnsureTempStringBuilder"/>
								<plx:loop>
									<plx:initializeLoop>
										<plx:local name="k" dataTypeName=".i4">
											<plx:initialize>
												<plx:value data="0" type="i4"/>
											</plx:initialize>
										</plx:local>
									</plx:initializeLoop>
									<plx:condition>
										<plx:binaryOperator type="lessThan">
											<plx:left>
												<plx:nameRef name="k"/>
											</plx:left>
											<plx:right>
												<plx:nameRef name="compatibleTypesCount"/>
											</plx:right>
										</plx:binaryOperator>
									</plx:condition>
									<plx:beforeLoop>
										<plx:increment>
											<plx:nameRef name="k"/>
										</plx:increment>
									</plx:beforeLoop>
									<plx:local name="listSnippet"  dataTypeName="{$VerbalizationTextSnippetType}"/>
									<xsl:call-template name="PopulateListSnippet">
										<xsl:with-param name="IteratorVariableName" select="'k'"/>
										<xsl:with-param name="ListStyle" select="'IdentityEqualityList'"/>
										<xsl:with-param name="IteratorBound">
											<plx:nameRef name="compatibleTypesCount"/>
										</xsl:with-param>
									</xsl:call-template>
									<xsl:call-template name="AppendListSnippet"/>
									<xsl:call-template name="PopulateBasicRoleReplacements_Assign_Body">
										<xsl:with-param name="ObjectTypeExpression">
											<plx:callInstance name=".implied" type="arrayIndexer">
												<plx:callObject>
													<plx:nameRef name="compatibleTypes"/>
												</plx:callObject>
												<plx:passParam>
													<plx:nameRef name="k"/>
												</plx:passParam>
											</plx:callInstance>
										</xsl:with-param>
										<xsl:with-param name="Append" select="true()"/>
										<xsl:with-param name="SubscriptConditions" select="$SubscriptConditions"/>
										<xsl:with-param name="CustomSubscripts" select="$CustomSubscripts"/>
										<xsl:with-param name="DynamicSubscripts" select="$DynamicSubscripts"/>
									</xsl:call-template>
									<xsl:call-template name="CloseList">
										<xsl:with-param name="IteratorVariableName" select="'k'"/>
										<xsl:with-param name="ListStyle" select="'IdentityEqualityList'"/>
										<xsl:with-param name="IteratorBound">
											<plx:nameRef name="compatibleTypesCount"/>
										</xsl:with-param>
									</xsl:call-template>
								</plx:loop>
								<plx:assign>
									<plx:left>
										<plx:nameRef name="basicReplacement"/>
									</plx:left>
									<plx:right>
										<plx:callInstance name="ToString">
											<plx:callObject>
												<plx:nameRef name="sbTemp"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:right>
								</plx:assign>
							</plx:fallbackBranch>
						</plx:branch>
						<plx:fallbackBranch>
							<xsl:call-template name="PopulateBasicRoleReplacements_Assign_Body">
								<xsl:with-param name="ObjectTypeExpression">
									<plx:nameRef name="rolePlayer"/>
								</xsl:with-param>
								<xsl:with-param name="SubscriptConditions" select="$SubscriptConditions"/>
								<xsl:with-param name="CustomSubscripts" select="$CustomSubscripts"/>
								<xsl:with-param name="DynamicSubscripts" select="$DynamicSubscripts"/>
							</xsl:call-template>
						</plx:fallbackBranch>
					</xsl:when>
				</xsl:choose>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="PopulateBasicRoleReplacements_Assign_Body">
					<xsl:with-param name="ObjectTypeExpression">
						<plx:nameRef name="rolePlayer"/>
					</xsl:with-param>
					<xsl:with-param name="SubscriptConditions" select="$SubscriptConditions"/>
					<xsl:with-param name="CustomSubscripts" select="$CustomSubscripts"/>
					<xsl:with-param name="DynamicSubscripts" select="$DynamicSubscripts"/>
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- Declare the basicRoleReplacements variable for a single fact and populate the basic
		 replacement fields. The fact's roles will be in the factRoles variable
		 and the fact arity in the factArity variable -->
	<xsl:template name="PopulateBasicRoleReplacements">
		<xsl:param name="SubscriptConditions"/>
		<xsl:param name="CustomSubscripts"/>
		<xsl:param name="DynamicSubscripts"/>
		<xsl:param name="DeclareBasicRoleReplacements" select="true()"/>
		<xsl:param name="DeclareSubscripter" select="true()"/>
		<xsl:param name="CompatibleColumns" select="false()"/>
		<xsl:param name="PatternGroup"/>
		<xsl:param name="IncludeInstanceData" select="false()"/>
		<xsl:if test="$DeclareBasicRoleReplacements">
			<plx:local name="basicRoleReplacements" dataTypeName=".string">
				<plx:arrayDescriptor rank="1">
					<xsl:if test="$CustomSubscripts">
						<xsl:attribute name="rank">
							<xsl:text>2</xsl:text>
						</xsl:attribute>
					</xsl:if>
				</plx:arrayDescriptor>
				<plx:initialize>
					<plx:callNew dataTypeName=".string">
						<plx:arrayDescriptor rank="1">
							<xsl:if test="$CustomSubscripts">
								<xsl:attribute name="rank">
									<xsl:text>2</xsl:text>
								</xsl:attribute>
							</xsl:if>
						</plx:arrayDescriptor>
						<plx:passParam>
							<plx:nameRef name="factArity"/>
						</plx:passParam>
						<xsl:if test="$CustomSubscripts">
							<plx:passParam>
								<plx:value data="3" type="i4">
									<xsl:if test="$DynamicSubscripts">
										<xsl:attribute name="data">
											<xsl:text>4</xsl:text>
										</xsl:attribute>
									</xsl:if>
								</plx:value>
							</plx:passParam>
						</xsl:if>
					</plx:callNew>
				</plx:initialize>
			</plx:local>
			<xsl:if test="$CustomSubscripts">
				<xsl:if test="$DeclareSubscripter">
					<plx:local name="subscripter" dataTypeName="VerbalizationSubscripter">
						<plx:initialize>
							<plx:callNew dataTypeName="VerbalizationSubscripter">
								<plx:passParam>
									<plx:callInstance name="FormatProvider" type="property">
										<plx:callObject>
											<plx:nameRef name="writer" type="parameter"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:passParam>
							</plx:callNew>
						</plx:initialize>
					</plx:local>
				</xsl:if>
			</xsl:if>
			<xsl:if test="$IncludeInstanceData">
				<plx:local name="textFormat" dataTypeName="string">
					<plx:initialize>
						<plx:callInstance name="GetSnippet">
							<plx:callObject>
								<plx:nameRef name="snippets"/>
							</plx:callObject>
							<plx:passParam>
								<plx:callStatic name="TextInstanceValue" dataTypeName="{$VerbalizationTextSnippetType}" type="field"/>
							</plx:passParam>
							<plx:passParam>
								<plx:nameRef name="isDeontic" type="local" />
							</plx:passParam>
							<plx:passParam>
								<plx:nameRef name="isNegative" type="local" />
							</plx:passParam>
						</plx:callInstance>
					</plx:initialize>
				</plx:local>
				<plx:local name="nonTextFormat" dataTypeName="string">
					<plx:initialize>
						<plx:callInstance name="GetSnippet">
							<plx:callObject>
								<plx:nameRef name="snippets"/>
							</plx:callObject>
							<plx:passParam>
								<plx:callStatic name="NonTextInstanceValue" dataTypeName="{$VerbalizationTextSnippetType}" type="field"/>
							</plx:passParam>
							<plx:passParam>
								<plx:nameRef name="isDeontic" type="local" />
							</plx:passParam>
							<plx:passParam>
								<plx:nameRef name="isNegative" type="local" />
							</plx:passParam>
						</plx:callInstance>
					</plx:initialize>
				</plx:local>
				<plx:local name="formatProvider" dataTypeName="IFormatProvider">
					<plx:initialize>
						<plx:callInstance name="FormatProvider" type="property">
							<plx:callObject>
								<plx:nameRef name="writer" type="local"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:initialize>
				</plx:local>
			</xsl:if>
			<xsl:if test="$SubscriptConditions and not($SubscriptConditions[1]/self::plx:trueKeyword)">
				<plx:local name="generateSubscripts" dataTypeName=".boolean">
					<plx:initialize>
						<xsl:copy-of select="$SubscriptConditions"/>
					</plx:initialize>
				</plx:local>
			</xsl:if>
		</xsl:if>
		<xsl:if test="$CompatibleColumns">
			<xsl:choose>
				<xsl:when test="$PatternGroup='SetConstraint'">
					<plx:local name="compatibleTypes" dataTypeName="ObjectType" dataTypeIsSimpleArray="true">
						<plx:initialize>
							<plx:callStatic name="GetNearestCompatibleTypes" dataTypeName="ObjectType">
								<plx:passParam>
									<plx:nameRef name="allConstraintRoles"/>
								</plx:passParam>
							</plx:callStatic>
						</plx:initialize>
					</plx:local>
				</xsl:when>
				<xsl:otherwise>
					<xsl:message terminate="yes">
						<xsl:text>PatternGroup '</xsl:text>
						<xsl:value-of select="$PatternGroup"/>
						<xsl:text>' not supported with compatible columns.</xsl:text>
					</xsl:message>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<plx:local name="lastMissingRolePlayerIndex" dataTypeName=".i4">
			<plx:initialize>
				<plx:value data="0" type="i4"/>
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
						<plx:nameRef name="factArity"/>
					</plx:right>
				</plx:binaryOperator>
			</plx:condition>
			<plx:beforeLoop>
				<plx:increment>
					<plx:nameRef name="i"/>
				</plx:increment>
			</plx:beforeLoop>
			<plx:local name="factRole" dataTypeName="Role">
				<plx:initialize>
					<plx:callInstance name="Role" type="property">
						<plx:callObject>
							<plx:callInstance name=".implied" type="indexerCall">
								<plx:callObject>
									<plx:nameRef name="factRoles"/>
								</plx:callObject>
								<plx:passParam>
									<plx:nameRef name="i"/>
								</plx:passParam>
							</plx:callInstance>
						</plx:callObject>
					</plx:callInstance>
				</plx:initialize>
			</plx:local>
			<plx:local name="rolePlayer" dataTypeName="ObjectType">
				<plx:initialize>
					<plx:callInstance name="RolePlayer" type="property">
						<plx:callObject>
							<plx:nameRef name="factRole"/>
						</plx:callObject>
					</plx:callInstance>
				</plx:initialize>
			</plx:local>
			<plx:local name="basicReplacement" dataTypeName=".string"/>
			<xsl:if test="$CustomSubscripts">
				<plx:local name="basicSubscriptedReplacement" dataTypeName=".string">
					<plx:initialize>
						<plx:nullKeyword/>
					</plx:initialize>
				</plx:local>
				<xsl:if test="$DynamicSubscripts">
					<plx:local name="basicDynamicSubscriptedReplacement" dataTypeName=".string">
						<plx:initialize>
							<plx:nullKeyword/>
						</plx:initialize>
					</plx:local>
				</xsl:if>
			</xsl:if>
			<plx:branch>
				<plx:condition>
					<plx:binaryOperator type="identityInequality">
						<plx:left>
							<plx:nameRef name="rolePlayer"/>
						</plx:left>
						<plx:right>
							<plx:nullKeyword/>
						</plx:right>
					</plx:binaryOperator>
				</plx:condition>
				<xsl:choose>
					<xsl:when test="$SubscriptConditions">
						<!-- Portions of the subscripting code that are conditional placed in
							 different spots -->
						<xsl:variable name="subscriptBody">
							<xsl:choose>
								<xsl:when test="$CustomSubscripts">
									<plx:local name="j" dataTypeName=".i4">
										<plx:initialize>
											<plx:value type="i4" data="0"/>
										</plx:initialize>
									</plx:local>
									<plx:loop>
										<plx:condition>
											<plx:binaryOperator type="lessThan">
												<plx:left>
													<plx:nameRef name="j"/>
												</plx:left>
												<plx:right>
													<plx:nameRef name="factArity"/>
												</plx:right>
											</plx:binaryOperator>
										</plx:condition>
										<plx:beforeLoop>
											<plx:increment>
												<plx:nameRef name="j"/>
											</plx:increment>
										</plx:beforeLoop>
										<plx:branch>
											<plx:condition>
												<plx:binaryOperator type="booleanAnd">
													<plx:left>
														<plx:binaryOperator type="inequality">
															<plx:left>
																<plx:nameRef name="i"/>
															</plx:left>
															<plx:right>
																<plx:nameRef name="j"/>
															</plx:right>
														</plx:binaryOperator>
													</plx:left>
													<plx:right>
														<plx:binaryOperator type="identityEquality">
															<plx:left>
																<plx:nameRef name="rolePlayer"/>
															</plx:left>
															<plx:right>
																<plx:callInstance name="RolePlayer" type="property">
																	<plx:callObject>
																		<plx:callInstance name="Role" type="property">
																			<plx:callObject>
																				<plx:callInstance name=".implied" type="indexerCall">
																					<plx:callObject>
																						<plx:nameRef name="factRoles"/>
																					</plx:callObject>
																					<plx:passParam>
																						<plx:nameRef name="j"/>
																					</plx:passParam>
																				</plx:callInstance>
																			</plx:callObject>
																		</plx:callInstance>
																	</plx:callObject>
																</plx:callInstance>
															</plx:right>
														</plx:binaryOperator>
													</plx:right>
												</plx:binaryOperator>
											</plx:condition>
											<plx:assign>
												<plx:left>
													<plx:nameRef name="useSubscript"/>
												</plx:left>
												<plx:right>
													<plx:trueKeyword/>
												</plx:right>
											</plx:assign>
											<plx:break/>
										</plx:branch>
									</plx:loop>
								</xsl:when>
								<xsl:otherwise>
									<plx:local name="j" dataTypeName=".i4">
										<plx:initialize>
											<plx:value type="i4" data="0"/>
										</plx:initialize>
									</plx:local>
									<plx:loop>
										<plx:condition>
											<plx:binaryOperator type="lessThan">
												<plx:left>
													<plx:nameRef name="j"/>
												</plx:left>
												<plx:right>
													<plx:nameRef name="i"/>
												</plx:right>
											</plx:binaryOperator>
										</plx:condition>
										<plx:beforeLoop>
											<plx:increment>
												<plx:nameRef name="j"/>
											</plx:increment>
										</plx:beforeLoop>
										<plx:branch>
											<plx:condition>
												<plx:binaryOperator type="identityEquality">
													<plx:left>
														<plx:nameRef name="rolePlayer"/>
													</plx:left>
													<plx:right>
														<plx:callInstance name="RolePlayer" type="property">
															<plx:callObject>
																<plx:callInstance name="Role" type="property">
																	<plx:callObject>
																		<plx:callInstance name=".implied" type="indexerCall">
																			<plx:callObject>
																				<plx:nameRef name="factRoles"/>
																			</plx:callObject>
																			<plx:passParam>
																				<plx:nameRef name="j"/>
																			</plx:passParam>
																		</plx:callInstance>
																	</plx:callObject>
																</plx:callInstance>
															</plx:callObject>
														</plx:callInstance>
													</plx:right>
												</plx:binaryOperator>
											</plx:condition>
											<plx:assign>
												<plx:left>
													<plx:nameRef name="useSubscript"/>
												</plx:left>
												<plx:right>
													<plx:trueKeyword/>
												</plx:right>
											</plx:assign>
											<plx:increment>
												<plx:nameRef name="subscript"/>
											</plx:increment>
										</plx:branch>
									</plx:loop>
									<plx:loop>
										<plx:initializeLoop>
											<plx:assign>
												<plx:left>
													<plx:nameRef name="j"/>
												</plx:left>
												<plx:right>
													<plx:binaryOperator type="add">
														<plx:left>
															<plx:nameRef name="i"/>
														</plx:left>
														<plx:right>
															<plx:value type="i4" data="1"/>
														</plx:right>
													</plx:binaryOperator>
												</plx:right>
											</plx:assign>
										</plx:initializeLoop>
										<plx:condition>
											<plx:binaryOperator type="booleanAnd">
												<plx:left>
													<plx:unaryOperator type="booleanNot">
														<plx:nameRef name="useSubscript"/>
													</plx:unaryOperator>
												</plx:left>
												<plx:right>
													<plx:binaryOperator type="lessThan">
														<plx:left>
															<plx:nameRef name="j"/>
														</plx:left>
														<plx:right>
															<plx:nameRef name="factArity"/>
														</plx:right>
													</plx:binaryOperator>
												</plx:right>
											</plx:binaryOperator>
										</plx:condition>
										<plx:beforeLoop>
											<plx:increment>
												<plx:nameRef name="j"/>
											</plx:increment>
										</plx:beforeLoop>
										<plx:branch>
											<plx:condition>
												<plx:binaryOperator type="identityEquality">
													<plx:left>
														<plx:nameRef name="rolePlayer"/>
													</plx:left>
													<plx:right>
														<plx:callInstance name="RolePlayer" type="property">
															<plx:callObject>
																<plx:callInstance name="Role" type="property">
																	<plx:callObject>
																		<plx:callInstance name=".implied" type="indexerCall">
																			<plx:callObject>
																				<plx:nameRef name="factRoles"/>
																			</plx:callObject>
																			<plx:passParam>
																				<plx:nameRef name="j"/>
																			</plx:passParam>
																		</plx:callInstance>
																	</plx:callObject>
																</plx:callInstance>
															</plx:callObject>
														</plx:callInstance>
													</plx:right>
												</plx:binaryOperator>
											</plx:condition>
											<plx:assign>
												<plx:left>
													<plx:nameRef name="useSubscript"/>
												</plx:left>
												<plx:right>
													<plx:trueKeyword/>
												</plx:right>
											</plx:assign>
										</plx:branch>
									</plx:loop>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<!-- See if we need a subscript by comparing to other role players before and after this one -->
						<xsl:if test="not($CustomSubscripts)">
							<plx:local name="subscript" dataTypeName=".i4">
								<plx:initialize>
									<plx:value type="i4" data="0"/>
								</plx:initialize>
							</plx:local>
						</xsl:if>
						<plx:local name="useSubscript" dataTypeName=".boolean">
							<plx:initialize>
								<plx:falseKeyword/>
							</plx:initialize>
						</plx:local>
						<xsl:choose>
							<xsl:when test="$SubscriptConditions and $SubscriptConditions/self::plx:trueKeyword">
								<xsl:copy-of select="$subscriptBody"/>
							</xsl:when>
							<xsl:otherwise>
								<plx:branch>
									<plx:condition>
										<plx:nameRef name="generateSubscripts"/>
									</plx:condition>
									<xsl:copy-of select="$subscriptBody"/>
								</plx:branch>
							</xsl:otherwise>
						</xsl:choose>
						<xsl:call-template name="PopulateBasicRoleReplacements_Assign">
							<xsl:with-param name="CompatibleColumns" select="$CompatibleColumns"/>
							<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
							<xsl:with-param name="CustomSubscripts" select="$CustomSubscripts"/>
							<xsl:with-param name="DynamicSubscripts" select="$DynamicSubscripts"/>
						</xsl:call-template>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="PopulateBasicRoleReplacements_Assign">
							<xsl:with-param name="CompatibleColumns" select="$CompatibleColumns"/>
							<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
							<xsl:with-param name="SubscriptConditions" select="false()"/>
							<xsl:with-param name="CustomSubscripts" select="false()"/>
							<xsl:with-param name="DynamicSubscripts" select="false()"/>
						</xsl:call-template>
					</xsl:otherwise>
				</xsl:choose>
			</plx:branch>
			<plx:fallbackBranch>
				<plx:assign>
					<plx:left>
						<plx:nameRef name="basicReplacement"/>
					</plx:left>
					<plx:right>
						<plx:callStatic name="Format" dataTypeName=".string">
							<plx:passParam>
								<plx:callInstance name="FormatProvider" type="property">
									<plx:callObject>
										<plx:nameRef type="parameter" name="writer"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:passParam>
							<plx:passParam>
								<xsl:call-template name="SnippetFor">
									<xsl:with-param name="SnippetType" select="'ObjectTypeMissing'"/>
								</xsl:call-template>
							</plx:passParam>
							<plx:passParam>
								<plx:inlineStatement dataTypeName=".i4">
									<plx:increment type="pre">
										<plx:nameRef name="lastMissingRolePlayerIndex"/>
									</plx:increment>
								</plx:inlineStatement>
							</plx:passParam>
						</plx:callStatic>
					</plx:right>
				</plx:assign>
			</plx:fallbackBranch>
			<xsl:if test="$IncludeInstanceData">
				<plx:local dataTypeName="FactTypeRoleInstance" name="roleInstance">
					<plx:initialize>
						<plx:nullKeyword/>
					</plx:initialize>
				</plx:local>
				<plx:loop checkCondition="before">
					<plx:initializeLoop>
						<plx:local dataTypeName=".i4" name="j">
							<plx:initialize>
								<plx:value data="0" type="i4"/>
							</plx:initialize>
						</plx:local>
					</plx:initializeLoop>
					<plx:condition>
						<plx:binaryOperator type="lessThan">
							<plx:left>
								<plx:nameRef name="j"/>
							</plx:left>
							<plx:right>
								<plx:nameRef name="instanceRoleCount"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:beforeLoop>
						<plx:increment>
							<plx:nameRef name="j"/>
						</plx:increment>
					</plx:beforeLoop>
					<plx:local dataTypeName="FactTypeRoleInstance" name="testInstance">
						<plx:initialize>
							<plx:callInstance name=".implied" type="arrayIndexer">
								<plx:callObject>
									<plx:nameRef name="instanceRoles"/>
								</plx:callObject>
								<plx:passParam>
									<plx:nameRef name="j"/>
								</plx:passParam>
							</plx:callInstance>
						</plx:initialize>
					</plx:local>
					<plx:branch>
						<plx:condition>
							<plx:binaryOperator type="equality">
								<plx:left>
									<plx:callInstance name="Role" type="property">
										<plx:callObject>
											<plx:nameRef name="testInstance"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:left>
								<plx:right>
									<plx:nameRef name="factRole" type="local"/>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:assign>
							<plx:left>
								<plx:nameRef name="roleInstance" type="local"/>
							</plx:left>
							<plx:right>
								<plx:nameRef name="testInstance" type="local"/>
							</plx:right>
						</plx:assign>
						<plx:break/>
					</plx:branch>
				</plx:loop>
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="identityInequality">
							<plx:left>
								<plx:nameRef name="roleInstance" type="local"/>
							</plx:left>
							<plx:right>
								<plx:nullKeyword/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:local name="instanceValue" dataTypeName="string"/>
					<plx:assign>
						<plx:left>
							<plx:nameRef name="instanceValue" type="local"/>
						</plx:left>
						<plx:right>
							<plx:callStatic name="GetDisplayString" dataTypeName="ObjectTypeInstance">
								<plx:passParam>
									<plx:callInstance name="ObjectTypeInstance" type="property">
										<plx:callObject>
											<plx:nameRef name="roleInstance" type="local"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:passParam>
								<plx:passParam>
									<plx:nameRef name="rolePlayer" type="local"/>
								</plx:passParam>
								<plx:passParam>
									<plx:falseKeyword/>
								</plx:passParam>
								<plx:passParam>
									<plx:nameRef name="formatProvider" type="local"/>
								</plx:passParam>
								<plx:passParam>
									<plx:nameRef name="textFormat" type="local"/>
								</plx:passParam>
								<plx:passParam>
									<plx:nameRef name="nonTextFormat" type="local"/>
								</plx:passParam>
							</plx:callStatic>
						</plx:right>
					</plx:assign>
					<plx:assign>
						<plx:left>
							<plx:nameRef name="basicReplacement" type="local"/>
						</plx:left>
						<plx:right>
							<plx:callStatic name="Format" dataTypeName=".string">
								<plx:passParam>
									<plx:callInstance name="FormatProvider" type="property">
										<plx:callObject>
											<plx:nameRef type="parameter" name="writer"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:passParam>
								<plx:passParam>
									<plx:callInstance name="GetSnippet">
										<plx:callObject>
											<plx:nameRef name="snippets"/>
										</plx:callObject>
										<plx:passParam>
											<plx:callStatic name="CombinedObjectAndInstance" dataTypeName="{$VerbalizationTextSnippetType}" type="field"/>
										</plx:passParam>
										<plx:passParam>
											<plx:nameRef name="isDeontic" type="local" />
										</plx:passParam>
										<plx:passParam>
											<plx:nameRef name="isNegative" type="local" />
										</plx:passParam>
									</plx:callInstance>
								</plx:passParam>
								<plx:passParam>
									<plx:nameRef name="basicReplacement" type="local"/>
								</plx:passParam>
								<plx:passParam>
									<plx:nameRef name="instanceValue" type="local"/>
								</plx:passParam>
							</plx:callStatic>
						</plx:right>
					</plx:assign>
				</plx:branch>
				<plx:fallbackBranch>
					<plx:assign>
						<plx:left>
							<plx:nameRef name="basicReplacement"/>
						</plx:left>
						<plx:right>
							<plx:callStatic name="Format" dataTypeName=".string">
								<plx:passParam>
									<plx:callInstance name="FormatProvider" type="property">
										<plx:callObject>
											<plx:nameRef type="parameter" name="writer"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:passParam>
								<plx:passParam>
									<xsl:call-template name="SnippetFor">
										<xsl:with-param name="SnippetType" select="'CombinedObjectAndInstanceTypeMissing'"/>
									</xsl:call-template>
								</plx:passParam>
								<plx:passParam>
									<plx:binaryOperator type="add">
										<plx:left>
											<plx:nameRef name="i"/>
										</plx:left>
										<plx:right>
											<plx:value type="i4" data="1"/>
										</plx:right>
									</plx:binaryOperator>
								</plx:passParam>
							</plx:callStatic>
						</plx:right>
					</plx:assign>
				</plx:fallbackBranch>
			</xsl:if>
			<plx:assign>
				<plx:left>
					<plx:callInstance name=".implied" type="arrayIndexer">
						<plx:callObject>
							<plx:nameRef name="basicRoleReplacements"/>
						</plx:callObject>
						<plx:passParam>
							<plx:nameRef name="i"/>
						</plx:passParam>
						<xsl:if test="$CustomSubscripts">
							<plx:passParam>
								<plx:value data="0" type="i4"/>
							</plx:passParam>
						</xsl:if>
					</plx:callInstance>
				</plx:left>
				<plx:right>
					<plx:nameRef name="basicReplacement"/>
				</plx:right>
			</plx:assign>
			<xsl:if test="$CustomSubscripts">
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="identityEquality">
							<plx:left>
								<plx:nameRef name="basicSubscriptedReplacement"/>
							</plx:left>
							<plx:right>
								<plx:nullKeyword/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:assign>
						<plx:left>
							<plx:callInstance name=".implied" type="arrayIndexer">
								<plx:callObject>
									<plx:nameRef name="basicRoleReplacements"/>
								</plx:callObject>
								<plx:passParam>
									<plx:nameRef name="i"/>
								</plx:passParam>
								<plx:passParam>
									<plx:value data="1" type="i4"/>
								</plx:passParam>
							</plx:callInstance>
						</plx:left>
						<plx:right>
							<plx:nameRef name="basicReplacement"/>
						</plx:right>
					</plx:assign>
					<plx:assign>
						<plx:left>
							<plx:callInstance name=".implied" type="arrayIndexer">
								<plx:callObject>
									<plx:nameRef name="basicRoleReplacements"/>
								</plx:callObject>
								<plx:passParam>
									<plx:nameRef name="i"/>
								</plx:passParam>
								<plx:passParam>
									<plx:value data="2" type="i4"/>
								</plx:passParam>
							</plx:callInstance>
						</plx:left>
						<plx:right>
							<plx:nullKeyword/>
						</plx:right>
					</plx:assign>
				</plx:branch>
				<plx:fallbackBranch>
					<plx:assign>
						<plx:left>
							<plx:callInstance name=".implied" type="arrayIndexer">
								<plx:callObject>
									<plx:nameRef name="basicRoleReplacements"/>
								</plx:callObject>
								<plx:passParam>
									<plx:nameRef name="i"/>
								</plx:passParam>
								<plx:passParam>
									<plx:value data="1" type="i4"/>
								</plx:passParam>
							</plx:callInstance>
						</plx:left>
						<plx:right>
							<plx:nameRef name="basicSubscriptedReplacement"/>
						</plx:right>
					</plx:assign>
					<plx:assign>
						<plx:left>
							<plx:callInstance name=".implied" type="arrayIndexer">
								<plx:callObject>
									<plx:nameRef name="basicRoleReplacements"/>
								</plx:callObject>
								<plx:passParam>
									<plx:nameRef name="i"/>
								</plx:passParam>
								<plx:passParam>
									<plx:value data="2" type="i4"/>
								</plx:passParam>
							</plx:callInstance>
						</plx:left>
						<plx:right>
							<plx:callStatic dataTypeName=".string" name="Empty" type="field"/>
						</plx:right>
					</plx:assign>
				</plx:fallbackBranch>
				<xsl:if test="$DynamicSubscripts">
					<plx:assign>
						<plx:left>
							<plx:callInstance name=".implied" type="arrayIndexer">
								<plx:callObject>
									<plx:nameRef name="basicRoleReplacements"/>
								</plx:callObject>
								<plx:passParam>
									<plx:nameRef name="i"/>
								</plx:passParam>
								<plx:passParam>
									<plx:value data="3" type="i4"/>
								</plx:passParam>
							</plx:callInstance>
						</plx:left>
						<plx:right>
							<plx:inlineStatement dataTypeName=".string">
								<plx:nullFallbackOperator>
									<plx:left>
										<plx:nameRef name="basicDynamicSubscriptedReplacement"/>
									</plx:left>
									<plx:right>
										<plx:nameRef name="basicReplacement"/>
									</plx:right>
								</plx:nullFallbackOperator>
							</plx:inlineStatement>
						</plx:right>
					</plx:assign>
				</xsl:if>
			</xsl:if>
		</plx:loop>
	</xsl:template>
	<xsl:template name="ConstraintBodyContent">
		<xsl:param name="PatternGroup"/>
		<xsl:param name="ResolveProxy"/>
		<!-- If we're a SetConstraint acting like an internal constraint then
			 initialize necessary variables -->
		<xsl:if test="$PatternGroup='InternalSetConstraint'">
			<plx:assign>
				<plx:left>
					<plx:nameRef name="parentFact"/>
				</plx:left>
				<plx:right>
					<plx:callInstance name=".implied" type="indexerCall">
						<plx:callObject>
							<plx:nameRef name="allFacts"/>
						</plx:callObject>
						<plx:passParam>
							<plx:value data="0" type="i4"/>
						</plx:passParam>
					</plx:callInstance>
				</plx:right>
			</plx:assign>
			<plx:assign>
				<plx:left>
					<plx:nameRef name="predicatePartFormatString"/>
				</plx:left>
				<plx:right>
					<xsl:call-template name="PopulatePredicatePartFormatString"/>
				</plx:right>
			</plx:assign>
			<plx:assign>
				<plx:left>
					<plx:nameRef name="allReadingOrders"/>
				</plx:left>
				<plx:right>
					<plx:callInstance name="ReadingOrderCollection" type="property">
						<plx:callObject>
							<plx:nameRef name="parentFact"/>
						</plx:callObject>
					</plx:callInstance>
				</plx:right>
			</plx:assign>
		</xsl:if>
		<!-- At this point we'll either have ConditionalReading or Snippet children -->
		<xsl:call-template name="ApplyBlockHelpers">
			<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
		</xsl:call-template>
		<xsl:apply-templates select="child::*" mode="ConstraintVerbalization">
			<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
			<xsl:with-param name="ResolveProxy" select="$ResolveProxy"/>
			<xsl:with-param name="TopLevel" select="true()"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="cvg:ReadingContext" mode="ConstraintVerbalization">
		<xsl:param name="TopLevel" select="false()"/>
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="''"/>
		<xsl:param name="IteratorContext" select="''"/>
		<xsl:param name="IteratorVariableName" select="''"/>
		<xsl:param name="PatternGroup"/>
		<xsl:param name="ResolveProxy"/>
		<xsl:variable name="useVariablePrefixFragment">
			<xsl:choose>
				<xsl:when test="$VariablePrefix">
					<xsl:value-of select="$VariablePrefix"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text>snippet</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:call-template name="PopulateReading">
			<xsl:with-param name="ReadingChoice" select="@match"/>
			<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
			<xsl:with-param name="ResolveProxy" select="$ResolveProxy"/>
		</xsl:call-template>
		<xsl:apply-templates select="child::*" mode="ConstraintVerbalization">
			<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
			<xsl:with-param name="ResolveProxy" select="$ResolveProxy"/>
			<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
			<xsl:with-param name="IteratorVariableName" select="$IteratorVariableName"/>
			<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
			<xsl:with-param name="VariablePrefix" select="string($useVariablePrefixFragment)"/>
			<xsl:with-param name="TopLevel" select="$TopLevel"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="cvg:ConditionalReading" mode="ConstraintVerbalization">
		<xsl:param name="TopLevel" select="false()"/>
		<xsl:param name="IteratorContext" select="''"/>
		<xsl:param name="IteratorVariableName" select="''"/>
		<xsl:param name="VariablePrefix" select="''"/>
		<xsl:param name="VariableDecorator" select="'1'"/>
		<xsl:param name="PatternGroup"/>
		<xsl:param name="ResolveProxy"/>
		<xsl:for-each select="cvg:ReadingChoice">
			<xsl:if test="position()=1">
				<xsl:call-template name="ProcessConditionalReadingChoice">
					<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
					<xsl:with-param name="IteratorVariableName" select="$IteratorVariableName"/>
					<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
					<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
					<xsl:with-param name="TopLevel" select="$TopLevel"/>
					<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
					<xsl:with-param name="ResolveProxy" select="$ResolveProxy"/>
				</xsl:call-template>
			</xsl:if>
			<xsl:if test="position()=last() and @match and ancestor::cvg:ConstrainedRoles[1]/parent::*/cvg:ConstrainedRoles[@sign]">
				<xsl:call-template name="DeferToOppositeSign"/>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>
	<xsl:template name="ProcessConditionalReadingChoice">
		<xsl:param name="IteratorContext" select="''"/>
		<xsl:param name="IteratorVariableName" select="''"/>
		<xsl:param name="VariablePrefix" select="''"/>
		<xsl:param name="VariableDecorator" select="'1'"/>
		<xsl:param name="TopLevel" select="false()"/>
		<xsl:param name="Match" select="string(@match)"/>
		<xsl:param name="PatternGroup"/>
		<xsl:param name="ResolveProxy"/>
		<xsl:param name="CurrentColumnExpression">
			<plx:value type="i4" data="0"/>
		</xsl:param>
		<xsl:choose>
			<xsl:when test="contains($Match,'All')">
				<xsl:variable name="singleMatch" select="concat(substring-before($Match,'All'), substring-after($Match,'All'))"/>
				<xsl:variable name="missingReadingLocalName" select="concat($VariablePrefix,'missingReading',$VariableDecorator)"/>
				<xsl:variable name="readingMatchIndexLocalName" select="concat($VariablePrefix,'readingMatchIndex',$VariableDecorator)"/>
				<plx:local name="{$missingReadingLocalName}" dataTypeName=".boolean">
					<plx:initialize>
						<plx:falseKeyword/>
					</plx:initialize>
				</plx:local>
				<plx:loop>
					<plx:initializeLoop>
						<plx:local name="{$readingMatchIndexLocalName}" dataTypeName=".i4">
							<plx:initialize>
								<plx:value type="i4" data="0"/>
							</plx:initialize>
						</plx:local>
					</plx:initializeLoop>
					<plx:condition>
						<plx:binaryOperator type="booleanAnd">
							<plx:left>
								<plx:unaryOperator type="booleanNot">
									<plx:nameRef name="{$missingReadingLocalName}"/>
								</plx:unaryOperator>
							</plx:left>
							<plx:right>
								<plx:binaryOperator type="lessThan">
									<plx:left>
										<plx:nameRef name="{$readingMatchIndexLocalName}"/>
									</plx:left>
									<plx:right>
										<plx:nameRef name="constraintRoleArity"/>
									</plx:right>
								</plx:binaryOperator>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:beforeLoop>
						<plx:increment>
							<plx:nameRef name="{$readingMatchIndexLocalName}"/>
						</plx:increment>
					</plx:beforeLoop>
					<plx:local name="primaryRole" dataTypeName="RoleBase">
						<plx:initialize>
							<xsl:choose>
								<xsl:when test="$PatternGroup='SetComparisonConstraint'">
									<plx:callInstance name="Role" type="property">
										<plx:callObject>
											<plx:callInstance name=".implied" type="indexerCall">
												<plx:callObject>
													<plx:callInstance name=".implied" type="arrayIndexer">
														<plx:callObject>
															<plx:nameRef name="allConstraintRoleSequences"/>
														</plx:callObject>
														<plx:passParam>
															<plx:nameRef name="{$readingMatchIndexLocalName}"/>
														</plx:passParam>
													</plx:callInstance>
												</plx:callObject>
												<plx:passParam>
													<xsl:copy-of select="$CurrentColumnExpression"/>
												</plx:passParam>
											</plx:callInstance>
										</plx:callObject>
									</plx:callInstance>
								</xsl:when>
								<xsl:otherwise>
									<plx:callInstance name=".implied" type="indexerCall">
										<plx:callObject>
											<plx:nameRef name="allConstraintRoles"/>
										</plx:callObject>
										<plx:passParam>
											<plx:nameRef name="{$readingMatchIndexLocalName}"/>
										</plx:passParam>
									</plx:callInstance>
								</xsl:otherwise>
							</xsl:choose>
						</plx:initialize>
					</plx:local>
					<plx:assign>
						<plx:left>
							<plx:nameRef name="parentFact"/>
						</plx:left>
						<plx:right>
							<plx:callInstance name="FactType" type="property">
								<plx:callObject>
									<plx:nameRef name="primaryRole"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:right>
					</plx:assign>
					<plx:assign>
						<plx:left>
							<plx:nameRef name="predicatePartFormatString"/>
						</plx:left>
						<plx:right>
							<xsl:call-template name="PopulatePredicatePartFormatString"/>
						</plx:right>
					</plx:assign>
					<plx:assign>
						<plx:left>
							<plx:nameRef name="allReadingOrders"/>
						</plx:left>
						<plx:right>
							<plx:callInstance name="ReadingOrderCollection" type="property">
								<plx:callObject>
									<plx:nameRef name="parentFact"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:right>
					</plx:assign>
					<plx:assign>
						<plx:left>
							<plx:nameRef name="factRoles"/>
						</plx:left>
						<plx:right>
							<xsl:call-template name="InitializeDefaultFactRoles"/>
						</plx:right>
					</plx:assign>
					<xsl:call-template name="PopulateReading">
						<xsl:with-param name="ReadingChoice" select="$singleMatch"/>
						<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
						<xsl:with-param name="ResolveProxy" select="$ResolveProxy"/>
					</xsl:call-template>
					<plx:branch>
						<plx:condition>
							<plx:binaryOperator type="identityEquality">
								<plx:left>
									<plx:nameRef name="reading"/>
								</plx:left>
								<plx:right>
									<plx:nullKeyword/>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:assign>
							<plx:left>
								<plx:nameRef name="{$missingReadingLocalName}"/>
							</plx:left>
							<plx:right>
								<plx:trueKeyword/>
							</plx:right>
						</plx:assign>
						<plx:break/>
					</plx:branch>
					<plx:fallbackBranch>
						<plx:assign>
							<plx:left>
								<plx:callInstance name=".implied" type="arrayIndexer">
									<plx:callObject>
										<plx:nameRef name="allConstraintRoleReadings"/>
									</plx:callObject>
									<plx:passParam>
										<plx:nameRef name="{$readingMatchIndexLocalName}"/>
									</plx:passParam>
								</plx:callInstance>
							</plx:left>
							<plx:right>
								<plx:nameRef name="reading"/>
							</plx:right>
						</plx:assign>
					</plx:fallbackBranch>
				</plx:loop>
				<plx:branch>
					<plx:condition>
						<plx:unaryOperator type="booleanNot">
							<plx:nameRef name="{$missingReadingLocalName}"/>
						</plx:unaryOperator>
					</plx:condition>
					<!-- The rest of this block is duplicated in other when and otherwise conditions, keep in sync -->
					<xsl:call-template name="ApplyBlockHelpers">
						<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
					</xsl:call-template>
					<xsl:apply-templates select="child::*" mode="ConstraintVerbalization">
						<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
						<xsl:with-param name="IteratorVariableName" select="$IteratorVariableName"/>
						<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
						<xsl:with-param name="TopLevel" select="$TopLevel"/>
						<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
						<xsl:with-param name="ResolveProxy" select="$ResolveProxy"/>
					</xsl:apply-templates>
				</plx:branch>
				<xsl:if test="position()!=last()">
					<plx:fallbackBranch>
						<xsl:for-each select="following-sibling::*">
							<xsl:if test="position()=1">
								<xsl:call-template name="ProcessConditionalReadingChoice">
									<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
									<xsl:with-param name="IteratorVariableName" select="$IteratorVariableName"/>
									<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
									<xsl:with-param name="VariableDecorator" select="$VariableDecorator + 1"/>
									<xsl:with-param name="TopLevel" select="$TopLevel"/>
									<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
									<xsl:with-param name="ResolveProxy" select="$ResolveProxy"/>
								</xsl:call-template>
							</xsl:if>
						</xsl:for-each>
					</plx:fallbackBranch>
				</xsl:if>
			</xsl:when>
			<xsl:when test="$PatternGroup='SetConstraint'">
				<!-- The rest of this block is duplicated in other when and otherwise conditions, keep in sync -->
				<xsl:call-template name="ApplyBlockHelpers">
					<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
				</xsl:call-template>
				<xsl:apply-templates select="child::*" mode="ConstraintVerbalization">
					<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
					<xsl:with-param name="IteratorVariableName" select="$IteratorVariableName"/>
					<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
					<xsl:with-param name="TopLevel" select="$TopLevel"/>
					<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
					<xsl:with-param name="ResolveProxy" select="$ResolveProxy"/>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<!-- If we are a SetComparisonConstraint, we do not need to spit any extra code -->
				<xsl:choose>
					<xsl:when test="not($PatternGroup = 'SetComparisonConstraint')">
						<xsl:call-template name="PopulateReading">
							<xsl:with-param name="ReadingChoice" select="$Match"/>
							<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
							<xsl:with-param name="ResolveProxy" select="$ResolveProxy"/>
						</xsl:call-template>
						<plx:branch>
							<plx:condition>
								<plx:binaryOperator type="identityInequality">
									<plx:left>
										<plx:nameRef name="reading"/>
									</plx:left>
									<plx:right>
										<plx:nullKeyword/>
									</plx:right>
								</plx:binaryOperator>
							</plx:condition>
							<!-- The rest of this block is duplicated in when conditions, keep in sync -->
							<xsl:call-template name="ApplyBlockHelpers">
								<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
							</xsl:call-template>
							<xsl:apply-templates select="child::*" mode="ConstraintVerbalization">
								<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
								<xsl:with-param name="IteratorVariableName" select="$IteratorVariableName"/>
								<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
								<xsl:with-param name="TopLevel" select="$TopLevel"/>
								<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
								<xsl:with-param name="ResolveProxy" select="$ResolveProxy"/>
							</xsl:apply-templates>
						</plx:branch>
						<xsl:if test="position()!=last()">
							<plx:fallbackBranch>
								<xsl:for-each select="following-sibling::*">
									<xsl:if test="position()=1">
										<xsl:call-template name="ProcessConditionalReadingChoice">
											<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
											<xsl:with-param name="IteratorVariableName" select="$IteratorVariableName"/>
											<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
											<xsl:with-param name="VariableDecorator" select="$VariableDecorator + 1"/>
											<xsl:with-param name="TopLevel" select="$TopLevel"/>
											<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
											<xsl:with-param name="ResolveProxy" select="$ResolveProxy"/>
										</xsl:call-template>
									</xsl:if>
								</xsl:for-each>
							</plx:fallbackBranch>
						</xsl:if>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="ApplyBlockHelpers">
							<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
						</xsl:call-template>
						<xsl:apply-templates select="child::*" mode="ConstraintVerbalization">
							<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
							<xsl:with-param name="IteratorVariableName" select="$IteratorVariableName"/>
							<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
							<xsl:with-param name="TopLevel" select="$TopLevel"/>
							<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
							<xsl:with-param name="ResolveProxy" select="$ResolveProxy"/>
						</xsl:apply-templates>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="cvg:SequenceJoinPath" mode="ConstraintVerbalization">
		<xsl:param name="TopLevel"/>
		<xsl:param name="VariablePrefix"/>
		<xsl:param name="VariableDecorator"/>
		<xsl:param name="CompositeCount"/>
		<xsl:param name="CompositeIterator"/>
		<xsl:param name="CompositeReplacementArray"/>
		<xsl:param name="PatternGroup"/>
		<xsl:param name="SequenceIterator"/>
		<xsl:param name="ListStyle" select="'null'"/>
		<xsl:param name="ParentListStyle"/>
		<xsl:param name="ConditionalMatch"/>
		<xsl:variable name="pathVerbalizationFragment">
			<plx:assign>
				<plx:left>
					<plx:callInstance name="Options" type="property">
						<plx:callObject>
							<plx:nameRef name="pathVerbalizer"/>
						</plx:callObject>
					</plx:callInstance>
				</plx:left>
				<plx:right>
					<xsl:variable name="optionsFragment">
						<xsl:if test="@markTrailingOutdentStart='true' or @markTrailingOutdentStart='1'">
							<plx:callStatic dataTypeName="RolePathVerbalizerOptions" name="MarkTrailingOutdentStart" type="field"/>
						</xsl:if>
					</xsl:variable>
					<xsl:variable name="options" select="exsl:node-set($optionsFragment)/child::*"/>
					<xsl:choose>
						<xsl:when test="$options">
							<xsl:call-template name="CombineElements">
								<xsl:with-param name="OperatorType" select="'bitwiseOr'"/>
								<xsl:with-param name="Elements" select="$options"/>
							</xsl:call-template>
						</xsl:when>
						<xsl:otherwise>
							<plx:callStatic dataTypeName="RolePathVerbalizerOptions" name="None" type="field"/>
						</xsl:otherwise>
					</xsl:choose>
				</plx:right>
			</plx:assign>
			<xsl:if test="@leadVariableQuantifier">
				<plx:assign>
					<plx:left>
						<plx:callInstance name="LeadVariableQuantifier" type="property">
							<plx:callObject>
								<plx:nameRef name="pathVerbalizer"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:left>
					<plx:right>
						<!-- new CoreSnippetIdentifier(CoreVerbalizationSnippetType.OneQuantifier, isDeontic, isNegative)-->
						<plx:callNew dataTypeName="CoreSnippetIdentifier">
							<plx:passParam>
								<plx:callStatic name="{@leadVariableQuantifier}" dataTypeName="{$VerbalizationTextSnippetType}" type="field"/>
							</plx:passParam>
							<plx:passParam>
								<plx:nameRef name="isDeontic"/>
							</plx:passParam>
							<plx:passParam>
								<plx:nameRef name="isNegative"/>
							</plx:passParam>
						</plx:callNew>
					</plx:right>
				</plx:assign>
			</xsl:if>
			<xsl:variable name="verbalizePathFragment">
				<plx:callInstance name="RenderPathVerbalization">
					<plx:callObject>
						<plx:nameRef name="pathVerbalizer"/>
					</plx:callObject>
					<plx:passParam>
						<plx:nameRef name="joinPath"/>
					</plx:passParam>
					<plx:passParam>
						<plx:nameRef name="sbTemp"/>
					</plx:passParam>
				</plx:callInstance>
			</xsl:variable>
			<xsl:variable name="createList" select="not($ListStyle='null')"/>
			<xsl:if test="not($CompositeCount)">
				<xsl:call-template name="EnsureTempStringBuilder"/>
			</xsl:if>
			<xsl:choose>
				<xsl:when test="$CompositeIterator">
					<xsl:if test="$createList">
						<plx:local name="listSnippet" dataTypeName="{$VerbalizationTextSnippetType}"/>
						<xsl:call-template name="PopulateListSnippet">
							<xsl:with-param name="IteratorVariableName" select="$CompositeIterator"/>
							<xsl:with-param name="ListStyle" select="$ListStyle"/>
							<xsl:with-param name="IteratorBound">
								<xsl:call-template name="ReferenceIteratorBound">
									<xsl:with-param name="CompositeCount" select="$CompositeCount"/>
									<xsl:with-param name="ContextMatch" select="'all'"/>
									<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
								</xsl:call-template>
							</xsl:with-param>
							<xsl:with-param name="CompositeIterator" select="$CompositeIterator"/>
						</xsl:call-template>
						<xsl:call-template name="AppendListSnippet"/>
						<xsl:if test="$CompositeReplacementArray">
							<xsl:call-template name="AppendReplacementField">
								<xsl:with-param name="ReplacementIndexVariable" select="$CompositeIterator"/>
							</xsl:call-template>
						</xsl:if>
					</xsl:if>
					<xsl:choose>
						<xsl:when test="$CompositeReplacementArray">
							<plx:assign>
								<plx:left>
									<plx:callInstance name=".implied" type="arrayIndexer">
										<plx:callObject>
											<plx:nameRef name="{$CompositeReplacementArray}"/>
										</plx:callObject>
										<plx:passParam>
											<plx:nameRef name="{$CompositeIterator}"/>
										</plx:passParam>
									</plx:callInstance>
								</plx:left>
								<plx:right>
									<xsl:copy-of select="$verbalizePathFragment"/>
								</plx:right>
							</plx:assign>
						</xsl:when>
						<xsl:otherwise>
							<plx:callInstance name="Append">
								<plx:callObject>
									<plx:nameRef name="sbTemp"/>
								</plx:callObject>
								<plx:passParam>
									<xsl:copy-of select="$verbalizePathFragment"/>
								</plx:passParam>
							</plx:callInstance>
						</xsl:otherwise>
					</xsl:choose>
					<xsl:if test="$createList">
						<xsl:call-template name="CloseList">
							<xsl:with-param name="IteratorVariableName" select="$CompositeIterator"/>
							<xsl:with-param name="ListStyle" select="$ListStyle"/>
							<xsl:with-param name="IteratorBound">
								<xsl:call-template name="ReferenceIteratorBound">
									<xsl:with-param name="CompositeCount" select="$CompositeCount"/>
									<xsl:with-param name="ContextMatch" select="'all'"/>
									<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
								</xsl:call-template>
							</xsl:with-param>
							<xsl:with-param name="CompositeIterator" select="$CompositeIterator"/>
						</xsl:call-template>
					</xsl:if>
					<plx:increment>
						<plx:nameRef name="{$CompositeIterator}"/>
					</plx:increment>
				</xsl:when>
				<xsl:otherwise>
					<plx:assign>
						<plx:left>
							<plx:nameRef name="{$VariablePrefix}{$VariableDecorator}"/>
						</plx:left>
						<plx:right>
							<xsl:copy-of select="$verbalizePathFragment"/>
						</plx:right>
					</plx:assign>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="@pathPreValidated='true' or @pathPreValidated=1">
				<xsl:copy-of select="$pathVerbalizationFragment"/>
			</xsl:when>
			<xsl:otherwise>
				<plx:branch>
					<plx:condition>
						<plx:callInstance name="HasPathVerbalization">
							<plx:callObject>
								<plx:nameRef name="pathVerbalizer"/>
							</plx:callObject>
							<plx:passParam>
								<plx:nameRef name="joinPath"/>
							</plx:passParam>
						</plx:callInstance>
					</plx:condition>
					<xsl:copy-of select="$pathVerbalizationFragment"/>
				</plx:branch>
			</xsl:otherwise>
		</xsl:choose>
		<xsl:variable name="fallbackFragment">
			<xsl:apply-templates select="*" mode="ConstraintVerbalization">
				<xsl:with-param name="TopLevel" select="$TopLevel"/>
				<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
				<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
				<xsl:with-param name="CompositeCount" select="$CompositeCount"/>
				<xsl:with-param name="CompositeIterator" select="$CompositeIterator"/>
				<xsl:with-param name="CompositeReplacementArray" select="$CompositeReplacementArray"/>
				<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
				<xsl:with-param name="SequenceIterator" select="$SequenceIterator"/>
				<xsl:with-param name="ListStyle" select="$ListStyle"/>
				<xsl:with-param name="ParentListStyle" select="$ParentListStyle"/>
				<xsl:with-param name="ConditionalMatch" select="$ConditionalMatch"/>
			</xsl:apply-templates>
		</xsl:variable>
		<xsl:variable name="fallback" select="exsl:node-set($fallbackFragment)/child::*"/>
		<xsl:if test="$fallback">
			<plx:fallbackBranch>
				<xsl:copy-of select="$fallback"/>
			</plx:fallbackBranch>
		</xsl:if>
	</xsl:template>

	<xsl:template match="cvg:MinValue" mode="ConstraintVerbalization">
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="'variableSnippet'"/>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="{$VariablePrefix}{$VariableDecorator}" type="local"/>
			</plx:left>
			<plx:right>
				<plx:nameRef name="minValue" type="local"/>
			</plx:right>
		</plx:assign>
	</xsl:template>

	<xsl:template match="cvg:MaxValue" mode="ConstraintVerbalization">
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="'variableSnippet'"/>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="{$VariablePrefix}{$VariableDecorator}" type="local"/>
			</plx:left>
			<plx:right>
				<plx:nameRef name="maxValue" type="local"/>
			</plx:right>
		</plx:assign>
	</xsl:template>

	<xsl:template match="cvg:DefaultValue" mode="ConstraintVerbalization">
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="'variableSnippet'"/>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="{$VariablePrefix}{$VariableDecorator}" type="local"/>
			</plx:left>
			<plx:right>
				<plx:nameRef name="defaultValue" type="local"/>
			</plx:right>
		</plx:assign>
	</xsl:template>

	<xsl:template match="cvg:MinFrequencyValue" mode="ConstraintVerbalization">
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="'variableSnippet'"/>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="{$VariablePrefix}{$VariableDecorator}" type="local"/>
			</plx:left>
			<plx:right>
				<plx:callInstance name="ToString">
					<plx:callObject>
						<plx:callThis name="MinFrequency" type="property"/>
					</plx:callObject>
					<plx:passParam>
						<plx:callStatic name="CurrentCulture" dataTypeName="CultureInfo" type="property"/>
					</plx:passParam>
				</plx:callInstance>
			</plx:right>
		</plx:assign>
	</xsl:template>

	<xsl:template match="cvg:MaxFrequencyValue" mode="ConstraintVerbalization">
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="'variableSnippet'"/>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="{$VariablePrefix}{$VariableDecorator}" type="local"/>
			</plx:left>
			<plx:right>
				<plx:callInstance name="ToString">
					<plx:callObject>
						<plx:callThis name="MaxFrequency" type="property"/>
					</plx:callObject>
					<plx:passParam>
						<plx:callStatic name="CurrentCulture" dataTypeName="CultureInfo" type="property"/>
					</plx:passParam>
				</plx:callInstance>
			</plx:right>
		</plx:assign>
	</xsl:template>

	<xsl:template match="cvg:RangeLowerBound" mode="ConstraintVerbalization">
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="'variableSnippet'"/>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="{$VariablePrefix}{$VariableDecorator}" type="local"/>
			</plx:left>
			<plx:right>
				<plx:callInstance name="ToString">
					<plx:callObject>
						<plx:nameRef name="rangeLowerBound"/>
					</plx:callObject>
					<plx:passParam>
						<plx:callStatic name="CurrentCulture" dataTypeName="CultureInfo" type="property"/>
					</plx:passParam>
				</plx:callInstance>
			</plx:right>
		</plx:assign>
	</xsl:template>

	<xsl:template match="cvg:RangeUpperBound" mode="ConstraintVerbalization">
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="'variableSnippet'"/>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="{$VariablePrefix}{$VariableDecorator}" type="local"/>
			</plx:left>
			<plx:right>
				<plx:callInstance name="ToString">
					<plx:callObject>
						<plx:nameRef name="rangeUpperBound"/>
					</plx:callObject>
					<plx:passParam>
						<plx:callStatic name="CurrentCulture" dataTypeName="CultureInfo" type="property"/>
					</plx:passParam>
				</plx:callInstance>
			</plx:right>
		</plx:assign>
	</xsl:template>

	<xsl:template match="cvg:ConditionalSnippet" mode="ConstraintVerbalization">
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="'variableSnippet'"/>
		<xsl:param name="TopLevel" select="false()"/>
		<xsl:param name="IsSecondary" select="false()"/>
		<xsl:param name="IteratorContext" select="''"/>
		<xsl:param name="IteratorVariableName" select="''"/>
		<xsl:param name="PatternGroup"/>
		<xsl:variable name="SnippetTypeVariable" select="concat($VariablePrefix, 'SnippetType', $VariableDecorator)"/>
		<xsl:variable name="snippetConditionsFragment">
			<xsl:call-template name="ProcessSnippetConditions">
				<xsl:with-param name="Snippets" select="cvg:Snippet"/>
				<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
				<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
				<xsl:with-param name="SnippetTypeVariable" select="$SnippetTypeVariable"/>
				<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
			</xsl:call-template>
		</xsl:variable>
		<xsl:call-template name="ProcessSnippet">
			<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
			<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
			<xsl:with-param name="TopLevel" select="$TopLevel"/>
			<xsl:with-param name="IsSecondary" select="$IsSecondary"/>
			<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
			<xsl:with-param name="IteratorVariableName" select="$IteratorVariableName"/>
			<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
			<xsl:with-param name="ReplacementContents" select="cvg:SnippetReplacements/child::*"/>
			<xsl:with-param name="SnippetTypeVariable" select="$SnippetTypeVariable"/>
			<xsl:with-param name="InjectPreliminaryBodyContents" select="exsl:node-set($snippetConditionsFragment)/child::*"/>
			<xsl:with-param name="CheckForInvalidSnippet" select="cvg:Snippet[@ref='null']"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template name="ProcessSnippetConditions">
		<xsl:param name="Snippets"/>
		<xsl:param name="SnippetCount" select="count($Snippets)"/>
		<xsl:param name="CurrentIndex" select="1"/>
		<xsl:param name="NewBranch" select="true()"/>
		<xsl:param name="VariableDecorator"/>
		<xsl:param name="VariablePrefix"/>
		<xsl:param name="SnippetTypeVariable"/>
		<xsl:param name="PatternGroup"/>
		<!-- This template is arranged this way to eventually allow complex conditionals to
		be efficiently inserted into an alternateBlock. This can be done by changing the
		alternateBranch into a fallbackBranch and placing a new branch (after the header
		code) into the fallbackBranch -->
		<xsl:for-each select="$Snippets[$CurrentIndex]">
			<xsl:variable name="conditionFragment">
				<xsl:call-template name="ConditionalMatchCondition">
					<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
				</xsl:call-template>
			</xsl:variable>
			<xsl:variable name="condition" select="exsl:node-set($conditionFragment)/child::*"/>
			<xsl:variable name="conditionalBranchContents">
				<xsl:choose>
					<xsl:when test="@ref='null'">
						<xsl:call-template name="SetSnippetVariableInvalid">
							<xsl:with-param name="VariableName" select="$SnippetTypeVariable"/>
						</xsl:call-template>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="SetSnippetVariable">
							<xsl:with-param name="SnippetType" select="@ref"/>
							<xsl:with-param name="VariableName" select="$SnippetTypeVariable"/>
						</xsl:call-template>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:choose>
				<xsl:when test="$condition">
					<xsl:if test="$CurrentIndex=1">
						<plx:local name="{$SnippetTypeVariable}" dataTypeName="{$VerbalizationTextSnippetType}">
							<plx:initialize>
								<plx:value data="0" type="i4"/>
							</plx:initialize>
						</plx:local>
					</xsl:if>
					<xsl:variable name="branchType">
						<xsl:choose>
							<xsl:when test="$NewBranch">
								<xsl:text>plx:branch</xsl:text>
							</xsl:when>
							<xsl:otherwise>
								<xsl:text>plx:alternateBranch</xsl:text>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<xsl:element name="{$branchType}">
						<plx:condition>
							<xsl:copy-of select="$condition"/>
						</plx:condition>
						<xsl:copy-of select="$conditionalBranchContents"/>
					</xsl:element>
					<xsl:if test="$CurrentIndex&lt;$SnippetCount">
						<xsl:call-template name="ProcessSnippetConditions">
							<xsl:with-param name="Snippets" select="$Snippets"/>
							<xsl:with-param name="SnippetCount" select="$SnippetCount"/>
							<xsl:with-param name="CurrentIndex" select="$CurrentIndex + 1"/>
							<xsl:with-param name="NewBranch" select="false()"/>
							<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
							<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
							<xsl:with-param name="SnippetTypeVariable" select="$SnippetTypeVariable"/>
							<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
							<xsl:with-param name="fallback" select="true()"/>
						</xsl:call-template>
					</xsl:if>
				</xsl:when>
				<xsl:when test="$NewBranch">
					<xsl:copy-of select="$conditionalBranchContents"/>
				</xsl:when>
				<xsl:otherwise>
					<plx:fallbackBranch>
						<xsl:copy-of select="$conditionalBranchContents"/>
					</plx:fallbackBranch>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:for-each>
	</xsl:template>

	<xsl:template match="cvg:ConditionalReplacement" mode="ConstraintVerbalization">
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="''"/>
		<xsl:param name="CompositeCount" select="''"/>
		<xsl:param name="CompositeIterator" select="''"/>
		<xsl:param name="TopLevel" select="false()"/>
		<xsl:param name="IteratorContext" select="''"/>
		<xsl:param name="IteratorVariableName" select="''"/>
		<xsl:param name="PatternGroup"/>
		<xsl:param name="InjectSnippetFormatArgument"/>
		<xsl:variable name="useVariablePrefixFragment">
			<xsl:choose>
				<xsl:when test="$VariablePrefix">
					<xsl:value-of select="$VariablePrefix"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text>snippet</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:for-each select="*">
			<xsl:if test="position()=1">
				<xsl:call-template name="ProcessConditionalReplacements">
					<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
					<xsl:with-param name="VariablePrefix" select="string($useVariablePrefixFragment)"/>
					<xsl:with-param name="CompositeCount" select="$CompositeCount"/>
					<xsl:with-param name="CompositeIterator" select="$CompositeIterator"/>
					<xsl:with-param name="TopLevel" select="$TopLevel"/>
					<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
					<xsl:with-param name="IteratorVariableName" select="$IteratorVariableName"/>
					<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
					<xsl:with-param name="InjectSnippetFormatArgument" select="$InjectSnippetFormatArgument"/>
				</xsl:call-template>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>

	<xsl:template name="ProcessConditionalReplacements">
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="''"/>
		<xsl:param name="CompositeCount" select="''"/>
		<xsl:param name="CompositeIterator" select="''"/>
		<xsl:param name="TopLevel" select="false()"/>
		<xsl:param name="IteratorContext" select="''"/>
		<xsl:param name="IteratorVariableName" select="''"/>
		<xsl:param name="PatternGroup"/>
		<xsl:param name="InjectSnippetFormatArgument"/>
		<xsl:param name="fallback" select="false()"/>
		<xsl:variable name="useVariablePrefixFragment">
			<xsl:choose>
				<xsl:when test="$VariablePrefix">
					<xsl:value-of select="$VariablePrefix"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text>snippet</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="useVariablePrefix" select="string($useVariablePrefixFragment)"/>
		<xsl:variable name="conditionFragment">
			<xsl:call-template name="ConditionalMatchCondition">
				<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
			</xsl:call-template>
		</xsl:variable>
		<xsl:variable name="condition" select="exsl:node-set($conditionFragment)/child::*"/>
		<xsl:choose>
			<xsl:when test="$fallback">
				<xsl:choose>
					<xsl:when test="$condition">
						<plx:alternateBranch>
							<plx:condition>
								<xsl:copy-of select="$condition"/>
							</plx:condition>
							<xsl:apply-templates select="." mode="ConstraintVerbalization">
								<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
								<xsl:with-param name="VariablePrefix" select="$useVariablePrefix"/>
								<xsl:with-param name="TopLevel" select="$TopLevel"/>
								<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
								<xsl:with-param name="IteratorVariableName" select="$IteratorVariableName"/>
								<xsl:with-param name="CompositeCount" select="$CompositeCount"/>
								<xsl:with-param name="CompositeIterator" select="$CompositeIterator"/>
								<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
								<xsl:with-param name="ConditionalMatch" select="''"/>
								<xsl:with-param name="InjectSnippetFormatArgument" select="$InjectSnippetFormatArgument"/>
							</xsl:apply-templates>
						</plx:alternateBranch>
					</xsl:when>
					<xsl:otherwise>
						<plx:fallbackBranch>
							<xsl:apply-templates select="." mode="ConstraintVerbalization">
								<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
								<xsl:with-param name="VariablePrefix" select="$useVariablePrefix"/>
								<xsl:with-param name="TopLevel" select="$TopLevel"/>
								<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
								<xsl:with-param name="IteratorVariableName" select="$IteratorVariableName"/>
								<xsl:with-param name="CompositeCount" select="$CompositeCount"/>
								<xsl:with-param name="CompositeIterator" select="$CompositeIterator"/>
								<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
								<xsl:with-param name="ConditionalMatch" select="''"/>
								<xsl:with-param name="InjectSnippetFormatArgument" select="$InjectSnippetFormatArgument"/>
							</xsl:apply-templates>
						</plx:fallbackBranch>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<xsl:otherwise>
				<xsl:choose>
					<xsl:when test="$condition">
						<plx:branch>
							<plx:condition>
								<xsl:copy-of select="$condition"/>
							</plx:condition>
							<xsl:apply-templates select="." mode="ConstraintVerbalization">
								<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
								<xsl:with-param name="VariablePrefix" select="$useVariablePrefix"/>
								<xsl:with-param name="TopLevel" select="$TopLevel"/>
								<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
								<xsl:with-param name="IteratorVariableName" select="$IteratorVariableName"/>
								<xsl:with-param name="CompositeCount" select="$CompositeCount"/>
								<xsl:with-param name="CompositeIterator" select="$CompositeIterator"/>
								<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
								<xsl:with-param name="ConditionalMatch" select="''"/>
								<xsl:with-param name="InjectSnippetFormatArgument" select="$InjectSnippetFormatArgument"/>
							</xsl:apply-templates>
						</plx:branch>
						<xsl:for-each select="following-sibling::cvg:*">
							<xsl:call-template name="ProcessConditionalReplacements">
								<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
								<xsl:with-param name="VariablePrefix" select="$useVariablePrefix"/>
								<xsl:with-param name="TopLevel" select="$TopLevel"/>
								<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
								<xsl:with-param name="IteratorVariableName" select="$IteratorVariableName"/>
								<xsl:with-param name="CompositeCount" select="$CompositeCount"/>
								<xsl:with-param name="CompositeIterator" select="$CompositeIterator"/>
								<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
								<xsl:with-param name="ConditionalMatch" select="''"/>
								<xsl:with-param name="fallback" select="true()"/>
								<xsl:with-param name="InjectSnippetFormatArgument" select="$InjectSnippetFormatArgument"/>
							</xsl:call-template>
						</xsl:for-each>
					</xsl:when>
				</xsl:choose>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="DeclareVariablesForFact">
		<xsl:param name="NestedFact" select="false()"/>
		<plx:local name="factRoles" dataTypeName="IList">
			<plx:passTypeParam dataTypeName="RoleBase"/>
			<plx:initialize>
				<plx:nullKeyword/>
			</plx:initialize>
		</plx:local>
		<plx:local name="factArity" dataTypeName=".i4">
			<plx:initialize>
				<plx:value data="0" type="i4"/>
			</plx:initialize>
		</plx:local>
		<plx:local name="allReadingOrders" dataTypeName="LinkedElementCollection">
			<plx:passTypeParam dataTypeName="ReadingOrder"/>
			<plx:initialize>
				<plx:nullKeyword/>
			</plx:initialize>
		</plx:local>
		<plx:local name="reading" dataTypeName="IReading">
			<plx:initialize>
				<plx:nullKeyword/>
			</plx:initialize>
		</plx:local>
		<plx:local name="hyphenBinder" dataTypeName="VerbalizationHyphenBinder"/>
		<xsl:choose>
			<xsl:when test="$NestedFact">
				<plx:local name="parentFact" dataTypeName="FactType">
					<plx:initialize>
						<plx:callThis name="NestedFactType" type="property"/>
					</plx:initialize>
				</plx:local>
				<plx:local name="predicatePartFormatString" dataTypeName=".string">
					<plx:initialize>
						<xsl:call-template name="PopulatePredicatePartFormatString"/>
					</plx:initialize>
				</plx:local>
				<plx:assign>
					<plx:left>
						<plx:nameRef name="allReadingOrders"/>
					</plx:left>
					<plx:right>
						<plx:callInstance name="ReadingOrderCollection" type="property">
							<plx:callObject>
								<plx:nameRef name="parentFact"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:right>
				</plx:assign>
				<plx:assign>
					<plx:left>
						<plx:nameRef name="factRoles"/>
					</plx:left>
					<plx:right>
						<xsl:call-template name="InitializeDefaultFactRoles"/>
					</plx:right>
				</plx:assign>
				<plx:assign>
					<plx:left>
						<plx:nameRef name="factArity"/>
					</plx:left>
					<plx:right>
						<xsl:call-template name="InitializeFactArity"/>
					</plx:right>
				</plx:assign>
			</xsl:when>
			<xsl:otherwise>
				<plx:local name="tempFactType" dataTypeName="FactType">
					<plx:initialize>
						<plx:nullKeyword/>
					</plx:initialize>
				</plx:local>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="cvg:Snippet" mode="ConstraintVerbalization" name="ProcessSnippet">
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="''"/>
		<xsl:param name="TopLevel" select="false()"/>
		<xsl:param name="IsSecondary" select="false()"/>
		<xsl:param name="IteratorContext" select="''"/>
		<xsl:param name="IteratorVariableName" select="''"/>
		<xsl:param name="PatternGroup"/>
		<xsl:param name="ReplacementContents" select="child::*"/>
		<xsl:param name="SnippetTypeVariable" select="''"/>
		<xsl:param name="ConditionalMatch" select="@conditionalMatch"/>
		<xsl:param name="InjectPreliminaryBodyContents"/>
		<xsl:param name="InjectSnippetFormatArgument"/>
		<xsl:param name="CheckForInvalidSnippet" select="false()"/>
		<xsl:variable name="useVariablePrefixFragment">
			<xsl:choose>
				<xsl:when test="$VariablePrefix">
					<xsl:value-of select="$VariablePrefix"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text>snippet</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="useVariablePrefix" select="string($useVariablePrefixFragment)"/>
		<xsl:variable name="byPassTopLevel" select="boolean(@byPassTopLevel)"/>
		<xsl:variable name="conditionFragment">
			<xsl:if test="$TopLevel and string-length($ConditionalMatch)">
				<xsl:call-template name="ConditionalMatchCondition">
					<xsl:with-param name="ConditionalMatch" select="$ConditionalMatch"/>
					<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
				</xsl:call-template>
			</xsl:if>
		</xsl:variable>
		<xsl:variable name="condition" select="exsl:node-set($conditionFragment)/child::*"/>
		<xsl:if test="$condition">
			<xsl:text disable-output-escaping="yes"><![CDATA[<plx:branch><plx:condition>]]></xsl:text>
			<xsl:copy-of select="$condition"/>
			<xsl:text disable-output-escaping="yes"><![CDATA[</plx:condition>]]></xsl:text>
		</xsl:if>
		<xsl:call-template name="ConditionalBlockContext"/>
		<xsl:if test="$InjectPreliminaryBodyContents">
			<xsl:copy-of select="$InjectPreliminaryBodyContents"/>
		</xsl:if>
		<xsl:if test="$TopLevel">
			<xsl:choose>
				<xsl:when test="position()&gt;1 or $IsSecondary">
					<xsl:variable name="precedingConditions" select="preceding-sibling::*[@conditionalMatch]"/>
					<xsl:choose>
						<xsl:when test="(position()!=1) and (position()-1=count($precedingConditions))">
							<xsl:variable name="expandedConditionsFragment">
								<xsl:for-each select="$precedingConditions">
									<xsl:call-template name="ConditionalMatchCondition">
										<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
									</xsl:call-template>
								</xsl:for-each>
							</xsl:variable>
							<xsl:variable name="expandedConditions" select="exsl:node-set($expandedConditionsFragment)/child::*"/>
							<plx:branch>
								<plx:condition>
									<xsl:call-template name="CombineElements">
										<xsl:with-param name="OperatorType" select="'booleanOr'"/>
										<xsl:with-param name="Elements" select="$expandedConditions"/>
									</xsl:call-template>
								</plx:condition>
								<plx:callInstance name="WriteLine">
									<plx:callObject>
										<plx:nameRef type="parameter" name="writer"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:branch>
							<plx:fallbackBranch>
								<plx:callInstance name="BeginVerbalization">
									<plx:callObject>
										<plx:nameRef name="verbalizationContext" type="parameter"/>
									</plx:callObject>
									<plx:passParam>
										<plx:callStatic name="Normal" dataTypeName="VerbalizationContent" type="field"/>
									</plx:passParam>
								</plx:callInstance>
							</plx:fallbackBranch>
						</xsl:when>
						<xsl:otherwise>
							<plx:callInstance name="WriteLine">
								<plx:callObject>
									<plx:nameRef type="parameter" name="writer"/>
								</plx:callObject>
							</plx:callInstance>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:otherwise>
					<plx:callInstance name="BeginVerbalization">
						<plx:callObject>
							<plx:nameRef name="verbalizationContext" type="parameter"/>
						</plx:callObject>
						<plx:passParam>
							<plx:callStatic name="Normal" dataTypeName="VerbalizationContent" type="field"/>
						</plx:passParam>
					</plx:callInstance>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>

		<xsl:if test="$CheckForInvalidSnippet">
			<xsl:if test="$TopLevel">
				<xsl:message terminate="yes">Empty snippets not supported for top-level verbalization.</xsl:message>
			</xsl:if>
			<plx:branch>
				<plx:condition>
					<plx:binaryOperator type="equality">
						<plx:left>
							<plx:nameRef name="{$SnippetTypeVariable}"/>
						</plx:left>
						<plx:right>
							<plx:cast dataTypeName="{$VerbalizationTextSnippetType}">
								<!-- UNDONE: Hack, PLiX needs to put parentheses around the negative value for a cast -->
								<plx:binaryOperator type="subtract">
									<plx:left>
										<plx:value data="0" type="i4"/>
									</plx:left>
									<plx:right>
										<plx:value data="1" type="i4"/>
									</plx:right>
								</plx:binaryOperator>
							</plx:cast>
						</plx:right>
					</plx:binaryOperator>
				</plx:condition>
				<plx:assign>
					<plx:left>
						<plx:nameRef name="{$useVariablePrefix}{$VariableDecorator}"/>
					</plx:left>
					<plx:right>
						<plx:string/>
					</plx:right>
				</plx:assign>
			</plx:branch>
			<xsl:text disable-output-escaping="yes"><![CDATA[<plx:fallbackBranch>]]></xsl:text>
		</xsl:if>
		<xsl:variable name="useOutdentFormatter" select="boolean(cvg:IterateSequences[@listStyle='null'][cvg:SequenceJoinPath[@markTrailingOutdentStart='true' or @markTrailingOutdentStart='1']] or self::*[@applyTrailingOutdentHere='true' or @applyTrailingOutdentHere='1'])"/>
		<xsl:if test="not(self::cvg:ConditionalSnippet and not($ReplacementContents)) and (not(@ref='SelfReference') or $useOutdentFormatter)">
			<plx:local name="{$useVariablePrefix}{$FormatVariablePart}{$VariableDecorator}" dataTypeName=".string">
				<plx:initialize>
					<xsl:call-template name="SnippetFor">
						<xsl:with-param name="SnippetType" select="@ref"/>
						<xsl:with-param name="VariableName" select="$SnippetTypeVariable"/>
					</xsl:call-template>
				</plx:initialize>
			</plx:local>
		</xsl:if>
		<xsl:for-each select="$ReplacementContents">
			<plx:local name="{$useVariablePrefix}{$VariableDecorator}{$ReplaceVariablePart}{position()}" dataTypeName=".string">
				<plx:initialize>
					<plx:nullKeyword/>
				</plx:initialize>
			</plx:local>
			<xsl:apply-templates select="."  mode="ConstraintVerbalization">
				<xsl:with-param name="VariablePrefix" select="concat($useVariablePrefix,$VariableDecorator,$ReplaceVariablePart)"/>
				<!-- The position will jump back to 1 with this call, so pick up the real position before jumping -->
				<xsl:with-param name="VariableDecorator" select="position()"/>
				<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
				<xsl:with-param name="IteratorVariableName" select="$IteratorVariableName"/>
				<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
			</xsl:apply-templates>
		</xsl:for-each>
		<xsl:if test="not($byPassTopLevel)">
			<xsl:variable name="formatCall">
				<xsl:choose>
					<xsl:when test="self::cvg:ConditionalSnippet and not($ReplacementContents)">
						<xsl:call-template name="SnippetFor">
							<xsl:with-param name="SnippetType" select="@ref"/>
							<xsl:with-param name="VariableName" select="$SnippetTypeVariable"/>
						</xsl:call-template>
					</xsl:when>
					<xsl:otherwise>
						<xsl:variable name="replacementParamsFragment">
							<xsl:if test="$InjectSnippetFormatArgument">
								<plx:passParam>
									<xsl:copy-of select="$InjectSnippetFormatArgument"/>
								</plx:passParam>
							</xsl:if>
							<xsl:choose>
								<xsl:when test="$ReplacementContents">
									<xsl:for-each select="$ReplacementContents">
										<plx:passParam>
											<plx:nameRef name="{$useVariablePrefix}{$VariableDecorator}{$ReplaceVariablePart}{position()}"/>
										</plx:passParam>
									</xsl:for-each>
								</xsl:when>
								<xsl:otherwise>
									<xsl:for-each select="child::*">
										<plx:passParam>
											<plx:nameRef name="{$useVariablePrefix}{$VariableDecorator}{$ReplaceVariablePart}{position()}"/>
										</plx:passParam>
									</xsl:for-each>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="@ref='SelfReference' and not($useOutdentFormatter)">
								<xsl:copy-of select="exsl:node-set($replacementParamsFragment)/*/*"/>
							</xsl:when>
							<xsl:otherwise>
								<plx:callStatic name="Format" dataTypeName=".string">
									<xsl:if test="$useOutdentFormatter">
										<xsl:attribute name="name">
											<xsl:text>FormatResolveOutdent</xsl:text>
										</xsl:attribute>
										<xsl:attribute name="dataTypeName">
											<xsl:text>RolePathVerbalizer</xsl:text>
										</xsl:attribute>
									</xsl:if>
									<plx:passParam>
										<plx:callInstance name="FormatProvider" type="property">
											<plx:callObject>
												<plx:nameRef type="parameter" name="writer"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:passParam>
									<xsl:if test="$useOutdentFormatter">
										<plx:passParam>
											<plx:nullKeyword/>
										</plx:passParam>
										<plx:passParam>
											<plx:callInstance name="NewLine" type="property">
												<plx:callObject>
													<plx:nameRef type="parameter" name="writer"/>
												</plx:callObject>
											</plx:callInstance>
										</plx:passParam>
									</xsl:if>
									<plx:passParam>
										<plx:nameRef name="{$useVariablePrefix}{$FormatVariablePart}{$VariableDecorator}"/>
									</plx:passParam>
									<xsl:copy-of select="$replacementParamsFragment"/>
								</plx:callStatic>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:choose>
				<xsl:when test="$TopLevel">
					<!-- Write the snippet directly to the text writer after sentence modification -->
					<xsl:choose>
						<xsl:when test="@closeSentence[.='0' or .='false']">
							<plx:callInstance name="Write">
								<plx:callObject>
									<plx:nameRef name="writer" type="parameter"/>
								</plx:callObject>
								<plx:passParam>
									<xsl:copy-of select="$formatCall"/>
								</plx:passParam>
							</plx:callInstance>
						</xsl:when>
						<xsl:otherwise>
							<plx:callStatic name="WriteVerbalizerSentence" dataTypeName="FactType">
								<plx:passParam>
									<plx:nameRef type="parameter" name="writer"/>
								</plx:passParam>
								<plx:passParam>
									<xsl:copy-of select="$formatCall"/>
								</plx:passParam>
								<plx:passParam>
									<xsl:call-template name="SnippetFor">
										<xsl:with-param name="SnippetType" select="'CloseVerbalizationSentence'"/>
									</xsl:call-template>
								</plx:passParam>
							</plx:callStatic>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:otherwise>
					<!-- Snippet is used as a replacement field in another snippet -->
					<plx:assign>
						<plx:left>
							<plx:nameRef name="{$useVariablePrefix}{$VariableDecorator}"/>
						</plx:left>
						<plx:right>
							<xsl:copy-of select="$formatCall"/>
						</plx:right>
					</plx:assign>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="$CheckForInvalidSnippet">
			<xsl:text disable-output-escaping="yes"><![CDATA[</plx:fallbackBranch>]]></xsl:text>
		</xsl:if>
		<xsl:if test="$condition">
			<xsl:text disable-output-escaping="yes"><![CDATA[</plx:branch>]]></xsl:text>
		</xsl:if>
	</xsl:template>
	<xsl:template match="cvg:PortableDataType" mode="ConstraintVerbalization">
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="''"/>
		<xsl:variable name="useVariablePrefixFragment">
			<xsl:choose>
				<xsl:when test="$VariablePrefix">
					<xsl:value-of select="$VariablePrefix"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text>factText</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="{string($useVariablePrefixFragment)}{$VariableDecorator}"/>
			</plx:left>
			<plx:right>
				<plx:callInstance name="ToString">
					<plx:callObject>
						<plx:nameRef name="identifyingDataType"/>
					</plx:callObject>
				</plx:callInstance>
			</plx:right>
		</plx:assign>
	</xsl:template>
	<xsl:template match="cvg:PortableDataTypeLength" mode="ConstraintVerbalization">
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="''"/>
		<xsl:variable name="useVariablePrefixFragment">
			<xsl:choose>
				<xsl:when test="$VariablePrefix">
					<xsl:value-of select="$VariablePrefix"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text>factText</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="{string($useVariablePrefixFragment)}{$VariableDecorator}"/>
			</plx:left>
			<plx:right>
				<plx:callInstance name="ToString">
					<plx:callObject>
						<plx:callInstance name="Length" type="property">
							<plx:callObject>
								<plx:nameRef name="dataTypeUse"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:callObject>
				</plx:callInstance>
			</plx:right>
		</plx:assign>
	</xsl:template>
	<xsl:template match="cvg:PortableDataTypeScale" mode="ConstraintVerbalization">
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="''"/>
		<xsl:variable name="useVariablePrefixFragment">
			<xsl:choose>
				<xsl:when test="$VariablePrefix">
					<xsl:value-of select="$VariablePrefix"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text>factText</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="{string($useVariablePrefixFragment)}{$VariableDecorator}"/>
			</plx:left>
			<plx:right>
				<plx:callInstance name="ToString">
					<plx:callObject>
						<plx:callInstance name="Scale" type="property">
							<plx:callObject>
								<plx:nameRef name="dataTypeUse"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:callObject>
				</plx:callInstance>
			</plx:right>
		</plx:assign>
	</xsl:template>
	<xsl:template match="cvg:ValueRangeValueTypeId" mode="ConstraintVerbalization">
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="''"/>
		<xsl:variable name="useVariablePrefixFragment">
			<xsl:choose>
				<xsl:when test="$VariablePrefix">
					<xsl:value-of select="$VariablePrefix"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text>factText</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="{string($useVariablePrefixFragment)}{$VariableDecorator}"/>
			</plx:left>
			<plx:right>
				<plx:callInstance name="ToString">
					<plx:callObject>
						<plx:callInstance name="Id" type="property">
							<plx:callObject>
								<plx:callThis name="ValueType" type="property"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:callObject>
					<plx:passParam>
						<plx:string data="D"/>
					</plx:passParam>
				</plx:callInstance>
			</plx:right>
		</plx:assign>
	</xsl:template>
	<xsl:template match="cvg:ObjectifyingInstanceIdentifierName" mode="ConstraintVerbalization">
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="'instance'"/>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="{$VariablePrefix}{$VariableDecorator}"/>
			</plx:left>
			<plx:right>
				<plx:callStatic name="GetDisplayString" dataTypeName="ObjectTypeInstance">
					<plx:passParam>
						<plx:nameRef name="objectifyingInstance"/>
					</plx:passParam>
					<plx:passParam>
						<plx:callInstance name="ObjectType" type="property">
							<plx:callObject>
								<plx:nameRef name="objectifyingInstance"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:passParam>
					<plx:passParam>
						<plx:trueKeyword/>
					</plx:passParam>
					<plx:passParam>
						<plx:nameRef name="formatProvider" type="local"/>
					</plx:passParam>
					<plx:passParam>
						<plx:nameRef name="textFormat" type="local"/>
					</plx:passParam>
					<plx:passParam>
						<plx:nameRef name="nonTextFormat" type="local"/>
					</plx:passParam>
				</plx:callStatic>
			</plx:right>
		</plx:assign>
	</xsl:template>
	<xsl:template match="cvg:ContextName|cvg:JoinedSetConstraintName|cvg:JoinedSetComparisonSequenceConstraintName|cvg:DynamicRuleName" name="ContextName" mode="ConstraintVerbalization">
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="''"/>
		<xsl:param name="PatternGroup" select="''"/>
		<!-- Add some helpers so we can call this from other constructs -->
		<xsl:param name="NameExpression"/>
		<xsl:param name="Normalize" select="@normalize='true'"/>
		<xsl:variable name="ObjectTypeAccessor" select="string(@objectTypeAccessor)"/>
		<xsl:variable name="useVariablePrefixFragment">
			<xsl:choose>
				<xsl:when test="$VariablePrefix">
					<xsl:value-of select="$VariablePrefix"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text>factText</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="{string($useVariablePrefixFragment)}{$VariableDecorator}"/>
			</plx:left>
			<plx:right>
				<xsl:choose>
					<xsl:when test="$Normalize">
						<plx:callStatic name="NormalizeObjectTypeName" dataTypeName="VerbalizationHelper">
							<plx:passParam>
								<xsl:choose>
									<xsl:when test="$PatternGroup='RoleValueConstraint' or $PatternGroup='UnaryRoleCardinalityConstraint'">
										<plx:callInstance name="RolePlayer" type="property">
											<plx:callObject>
												<plx:nameRef name="constrainedRole"/>
											</plx:callObject>
										</plx:callInstance>
									</xsl:when>
									<xsl:when test="$PatternGroup='ValueTypeValueConstraint'">
										<plx:callThis name="ValueType" type="property"/>
									</xsl:when>
									<xsl:when test="$PatternGroup='ObjectTypeCardinalityConstraint'">
										<plx:callThis name="ObjectType" type="property"/>
									</xsl:when>
									<xsl:when test="$PatternGroup='NearestValueConstraint'">
										<plx:callThis name="ParentObjectType" type="property"/>
									</xsl:when>
									<xsl:when test="$PatternGroup='DerivedFrom'">
										<plx:callInstance name="Name" type="property">
											<plx:callObject>
												<xsl:choose>
													<xsl:when test="self::cvg:JoinedSetConstraintName">
														<plx:nameRef name="joinedSetConstraint"/>
													</xsl:when>
													<xsl:when test="self::cvg:JoinedSetComparisonSequenceConstraintName">
														<plx:nameRef name="joinedSetComparisonConstraint"/>
													</xsl:when>
													<xsl:when test="self::cvg:DynamicRuleName">
														<plx:nameRef name="dynamicRule"/>
													</xsl:when>
													<xsl:otherwise>
														<plx:nameRef name="derivedSubtype"/>
													</xsl:otherwise>
												</xsl:choose>
											</plx:callObject>
										</plx:callInstance>
									</xsl:when>
									<xsl:when test="$NameExpression">
										<xsl:copy-of select="$NameExpression"/>
									</xsl:when>
									<xsl:otherwise>
										<plx:thisKeyword/>
									</xsl:otherwise>
								</xsl:choose>
							</plx:passParam>
							<plx:passParam>
								<plx:callInstance name="VerbalizationOptions" type="property">
									<plx:callObject>
										<plx:nameRef name="verbalizationContext" type="parameter"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:passParam>
						</plx:callStatic>
					</xsl:when>
					<xsl:otherwise>
						<xsl:choose>
							<xsl:when test="$NameExpression">
								<xsl:copy-of select="$NameExpression"/>
							</xsl:when>
							<xsl:when test="$PatternGroup='DerivedFrom'">
								<plx:callInstance name="Name" type="property">
									<plx:callObject>
										<xsl:choose>
											<xsl:when test="self::cvg:JoinedSetConstraintName">
												<plx:nameRef name="joinedSetConstraint"/>
											</xsl:when>
											<xsl:when test="self::cvg:JoinedSetComparisonSequenceConstraintName">
												<plx:nameRef name="joinedSetComparisonConstraint"/>
											</xsl:when>
											<xsl:when test="self::cvg:DynamicRuleName">
												<plx:nameRef name="dynamicRule"/>
											</xsl:when>
											<xsl:otherwise>
												<plx:nameRef name="derivedSubtype"/>
											</xsl:otherwise>
										</xsl:choose>
									</plx:callObject>
								</plx:callInstance>
							</xsl:when>
							<xsl:otherwise>
								<plx:callThis name="Name" type="property"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:otherwise>
				</xsl:choose>
			</plx:right>
		</plx:assign>
	</xsl:template>
	<xsl:template match="cvg:ParameterName" mode="ConstraintVerbalization">
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="''"/>
		<xsl:call-template name="ContextName">
			<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
			<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
			<xsl:with-param name="Normalize" select="false()"/>
			<xsl:with-param name="NameExpression">
				<plx:nameRef name="queryParameterName"/>
			</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<xsl:template match="cvg:ContextId" mode="ConstraintVerbalization">
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="''"/>
		<xsl:param name="PatternGroup" select="''"/>
		<xsl:variable name="useVariablePrefixFragment">
			<xsl:choose>
				<xsl:when test="$VariablePrefix">
					<xsl:value-of select="$VariablePrefix"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text>factText</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="{string($useVariablePrefixFragment)}{$VariableDecorator}"/>
			</plx:left>
			<plx:right>
				<plx:callInstance name="ToString">
					<plx:callObject>
						<xsl:choose>
							<xsl:when test="$PatternGroup='ValueTypeValueConstraint'">
								<plx:callInstance name="Id" type="property">
									<plx:callObject>
										<plx:callThis name="ValueType" type="property"/>
									</plx:callObject>
								</plx:callInstance>
							</xsl:when>
							<xsl:when test="$PatternGroup='ObjectTypeCardinalityConstraint'">
								<plx:callInstance name="Id" type="property">
									<plx:callObject>
										<plx:callThis name="ObjectType" type="property"/>
									</plx:callObject>
								</plx:callInstance>
							</xsl:when>
							<xsl:when test="$PatternGroup='RoleValueConstraint' or $PatternGroup='UnaryRoleCardinalityConstraint'">
								<plx:callInstance name="Id" type="property">
									<plx:callObject>
										<plx:callInstance name="RolePlayer" type="property">
											<plx:callObject>
												<plx:nameRef name="constrainedRole"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:callObject>
								</plx:callInstance>
							</xsl:when>
							<xsl:when test="$PatternGroup='DerivedFrom'">
								<plx:callInstance name="Id" type="property">
									<plx:callObject>
										<plx:nameRef name="derivedElement"/>
									</plx:callObject>
								</plx:callInstance>
							</xsl:when>
							<xsl:otherwise>
								<plx:callThis name="Id" type="property" />
							</xsl:otherwise>
						</xsl:choose>
					</plx:callObject>
					<plx:passParam>
						<plx:string data="D"/>
					</plx:passParam>
				</plx:callInstance>
			</plx:right>
		</plx:assign>
	</xsl:template>
	<xsl:template match="cvg:ContextSequenceNumber" mode="ConstraintVerbalization">
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="''"/>
		<xsl:param name="PatternGroup" select="''"/>
		<xsl:variable name="useVariablePrefixFragment">
			<xsl:choose>
				<xsl:when test="$VariablePrefix">
					<xsl:value-of select="$VariablePrefix"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text>sequenceNumber</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="{string($useVariablePrefixFragment)}{$VariableDecorator}"/>
			</plx:left>
			<plx:right>
				<plx:callInstance name="ToString">
					<plx:callObject>
						<plx:binaryOperator type="add">
							<plx:left>
								<plx:callInstance name="IndexOf">
									<plx:callObject>
										<plx:callInstance name="RoleSequenceCollection" type="property">
											<plx:callObject>
												<xsl:choose>
													<xsl:when test="$PatternGroup='DerivedFrom'">
														<plx:nameRef name="joinedSetComparisonConstraint"/>
													</xsl:when>
													<xsl:otherwise>
														<plx:callThis name="ExternalConstraint" type="property"/>
													</xsl:otherwise>
												</xsl:choose>
											</plx:callObject>
										</plx:callInstance>
									</plx:callObject>
									<plx:passParam>
										<xsl:choose>
											<xsl:when test="$PatternGroup='DerivedFrom'">
												<plx:nameRef name="joinedSetComparisonSequence"/>
											</xsl:when>
											<xsl:otherwise>
												<plx:thisKeyword/>
											</xsl:otherwise>
										</xsl:choose>
									</plx:passParam>
								</plx:callInstance>
							</plx:left>
							<plx:right>
								<plx:value data="1" type="i4"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:callObject>
				</plx:callInstance>
			</plx:right>
		</plx:assign>
	</xsl:template>
	<xsl:template match="cvg:PreferredIdentifierFor" mode="ConstraintVerbalization">
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="''"/>
		<xsl:variable name="useVariablePrefixFragment">
			<xsl:choose>
				<xsl:when test="$VariablePrefix">
					<xsl:value-of select="$VariablePrefix"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text>factText</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="{string($useVariablePrefixFragment)}{$VariableDecorator}"/>
			</plx:left>
			<plx:right>
				<plx:callStatic name="NormalizeObjectTypeName" dataTypeName="VerbalizationHelper">
					<plx:passParam>
						<!-- preferredFor is guaranteed to be an entity type, pass the name directly to avoid
						testing the value type for being a reference mode. -->
						<plx:callInstance name="Name" type="property">
							<plx:callObject>
								<plx:nameRef name="preferredFor"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:passParam>
					<plx:passParam>
						<plx:callInstance name="VerbalizationOptions" type="property">
							<plx:callObject>
								<plx:nameRef name="verbalizationContext" type="parameter"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:passParam>
				</plx:callStatic>
			</plx:right>
		</plx:assign>
	</xsl:template>
	<xsl:template match="cvg:PreferredIdentifierForId" mode="ConstraintVerbalization">
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="''"/>
		<xsl:variable name="useVariablePrefixFragment">
			<xsl:choose>
				<xsl:when test="$VariablePrefix">
					<xsl:value-of select="$VariablePrefix"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text>factText</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="{string($useVariablePrefixFragment)}{$VariableDecorator}"/>
			</plx:left>
			<plx:right>
				<plx:callInstance name="ToString">
					<plx:callObject>
						<plx:callInstance name="Id" type="property">
							<plx:callObject>
								<plx:nameRef name="preferredFor"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:callObject>
					<plx:passParam>
						<plx:string data="D"/>
					</plx:passParam>
				</plx:callInstance>
			</plx:right>
		</plx:assign>
	</xsl:template>
	<xsl:template match="cvg:Subscript" mode="ConstraintVerbalization">
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="''"/>
		<xsl:param name="PatternGroup" select="''"/>
		<xsl:variable name="useVariablePrefixFragment">
			<xsl:choose>
				<xsl:when test="$VariablePrefix">
					<xsl:value-of select="$VariablePrefix"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text>factText</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="{string($useVariablePrefixFragment)}{$VariableDecorator}"/>
			</plx:left>
			<plx:right>
				<plx:callInstance name="ToString">
					<plx:callObject>
						<plx:nameRef name="subscript"/>
					</plx:callObject>
				</plx:callInstance>
			</plx:right>
		</plx:assign>
	</xsl:template>
	<xsl:template match="cvg:SubtypeName" mode="ConstraintVerbalization">
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="'subtypeNameText'"/>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="{$VariablePrefix}{$VariableDecorator}"/>
			</plx:left>
			<plx:right>
				<plx:callStatic name="NormalizeObjectTypeName" dataTypeName="VerbalizationHelper">
					<plx:passParam>
						<!-- subtype is guaranteed to be an entity type, pass the name directly to avoid
						testing the value type for being a reference mode. -->
						<plx:callInstance name="Name" type="property">
							<plx:callObject>
								<plx:nameRef name="subtype"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:passParam>
					<plx:passParam>
						<plx:callInstance name="VerbalizationOptions" type="property">
							<plx:callObject>
								<plx:nameRef name="verbalizationContext" type="parameter"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:passParam>
				</plx:callStatic>
			</plx:right>
		</plx:assign>
	</xsl:template>
	<xsl:template match="cvg:SupertypeName" mode="ConstraintVerbalization">
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="'supertypeNameText'"/>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="{$VariablePrefix}{$VariableDecorator}"/>
			</plx:left>
			<plx:right>
				<plx:callStatic name="NormalizeObjectTypeName" dataTypeName="VerbalizationHelper">
					<plx:passParam>
						<!-- supertype is guaranteed to be an entity type, pass the name directly to avoid
						testing the value type for being a reference mode. -->
						<plx:nameRef name="supertype"/>
					</plx:passParam>
					<plx:passParam>
						<plx:callInstance name="VerbalizationOptions" type="property">
							<plx:callObject>
								<plx:nameRef name="verbalizationContext" type="parameter"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:passParam>
				</plx:callStatic>
			</plx:right>
		</plx:assign>
	</xsl:template>
	<xsl:template match="cvg:SubtypeId" mode="ConstraintVerbalization">
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="'subtypeIdText'"/>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="{$VariablePrefix}{$VariableDecorator}"/>
			</plx:left>
			<plx:right>
				<plx:callInstance name="ToString">
					<plx:callObject>
						<plx:callInstance name="Id" type="property">
							<plx:callObject>
								<plx:nameRef name="subtype"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:callObject>
					<plx:passParam>
						<plx:string data="D"/>
					</plx:passParam>
				</plx:callInstance>
			</plx:right>
		</plx:assign>
	</xsl:template>
	<xsl:template match="cvg:SupertypeId" mode="ConstraintVerbalization">
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="'supertypeIdText'"/>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="{$VariablePrefix}{$VariableDecorator}"/>
			</plx:left>
			<plx:right>
				<plx:callInstance name="ToString">
					<plx:callObject>
						<plx:callInstance name="Id" type="property">
							<plx:callObject>
								<plx:nameRef name="supertype"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:callObject>
					<plx:passParam>
						<plx:string data="D"/>
					</plx:passParam>
				</plx:callInstance>
			</plx:right>
		</plx:assign>
	</xsl:template>
	<xsl:template match="cvg:ReferenceMode" mode="ConstraintVerbalization">
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="''"/>
		<xsl:variable name="useVariablePrefixFragment">
			<xsl:choose>
				<xsl:when test="$VariablePrefix">
					<xsl:value-of select="$VariablePrefix"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text>factText</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="{string($useVariablePrefixFragment)}{$VariableDecorator}"/>
			</plx:left>
			<plx:right>
				<plx:callInstance name="ReferenceModeDecoratedString" type="property">
					<plx:callObject>
						<plx:nameRef name="identifyingEntityType"/>
					</plx:callObject>
				</plx:callInstance>
			</plx:right>
		</plx:assign>
	</xsl:template>
	<xsl:template match="cvg:RoleName" mode="ConstraintVerbalization">
		<xsl:param name="VariableDecorator"/>
		<xsl:param name="VariablePrefix"/>
		<xsl:param name="PatternGroup" select="''"/>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="{$VariablePrefix}{$VariableDecorator}"/>
			</plx:left>
			<plx:right>
				<xsl:choose>
					<xsl:when test="$PatternGroup='RoleValueConstraint'">
						<plx:callInstance name="Name" type="property">
							<plx:callObject>
								<plx:callInstance name="Role" type="property">
									<plx:callObject>
										<plx:nameRef name="primaryRole"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:callObject>
						</plx:callInstance>
					</xsl:when>
					<xsl:otherwise>
						<plx:nameRef name="roleName"/>
					</xsl:otherwise>
				</xsl:choose>
			</plx:right>
		</plx:assign>
	</xsl:template>
	<xsl:template match="cvg:RoleIndex" mode="ConstraintVerbalization">
		<xsl:param name="VariableDecorator"/>
		<xsl:param name="VariablePrefix"/>
		<xsl:param name="IteratorVariableName"/>
		<xsl:param name="PatternGroup"/>
		<xsl:param name="ListStyle" select="'null'"/>
		<xsl:param name="ConditionalMatch" select="''"/>
		<xsl:param name="ContextMatch"/>
		<xsl:if test="not($PatternGroup='InternalSetConstraint')">
			<xsl:message terminate="yes">Role index has only been tested for the InternalSetConstraint pattern group.</xsl:message>
		</xsl:if>
		<plx:callInstance name="Append">
			<plx:callObject>
				<plx:nameRef name="sbTemp"/>
			</plx:callObject>
			<plx:passParam>
				<plx:callInstance name="ToString">
					<plx:callObject>
						<plx:binaryOperator type="add">
							<plx:left>
								<plx:callInstance name="IndexOf">
									<plx:callObject>
										<plx:callInstance name="RoleCollection" type="property">
											<plx:callObject>
												<plx:nameRef name="reading"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:callObject>
									<plx:passParam>
										<plx:callInstance name=".implied" type="indexerCall">
											<plx:callObject>
												<plx:nameRef name="allConstraintRoles"/>
											</plx:callObject>
											<plx:passParam>
												<plx:nameRef name="{$IteratorVariableName}"/>
											</plx:passParam>
										</plx:callInstance>
									</plx:passParam>
								</plx:callInstance>
							</plx:left>
							<plx:right>
								<plx:value data="1" type="i4"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:callObject>
					<plx:passParam>
						<plx:callInstance name="FormatProvider" type="property">
							<plx:callObject>
								<plx:nameRef name="writer" type="parameter"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:passParam>
				</plx:callInstance>
			</plx:passParam>
		</plx:callInstance>
	</xsl:template>
	<xsl:template match="cvg:RolePlayer" mode="ConstraintVerbalization">
		<xsl:param name="VariableDecorator"/>
		<xsl:param name="VariablePrefix"/>
		<xsl:param name="IteratorVariableName"/>
		<xsl:param name="PatternGroup"/>
		<xsl:param name="ListStyle" select="'null'"/>
		<xsl:param name="ConditionalMatch" select="''"/>
		<xsl:param name="ContextMatch"/>
		<plx:callInstance name="Append">
			<plx:callObject>
				<plx:nameRef name="sbTemp"/>
			</plx:callObject>
			<plx:passParam>
				<xsl:choose>
					<xsl:when test="$PatternGroup='SetComparisonConstraint'">
						<plx:callInstance name=".implied" type="indexerCall">
							<plx:callObject>
								<xsl:choose>
									<xsl:when test="$ContextMatch='constraintRoles'">
										<plx:nameRef name="basicRoleReplacements"/>
									</xsl:when>
									<xsl:otherwise>
										<plx:callInstance name=".implied" type="indexerCall">
											<plx:callObject>
												<plx:nameRef name="allBasicRoleReplacements"/>
											</plx:callObject>
											<plx:passParam>
												<plx:inlineStatement dataTypeName=".i4">
													<plx:assign>
														<plx:left>
															<plx:nameRef name="contextBasicReplacementIndex"/>
														</plx:left>
														<plx:right>
															<plx:callInstance name="IndexOf">
																<plx:callObject>
																	<plx:nameRef name="allFacts" type="local"/>
																</plx:callObject>
																<plx:passParam>
																	<plx:callInstance name="FactType" type="property">
																		<plx:callObject>
																			<plx:callObject name="Role" type="property">
																				<plx:callObject>
																					<plx:callInstance name=".implied" type="arrayIndexer">
																						<plx:callObject>
																							<plx:nameRef name="includedConstraintRoles"/>
																						</plx:callObject>
																						<plx:passParam>
																							<plx:nameRef name="{$IteratorVariableName}"/>
																						</plx:passParam>
																					</plx:callInstance>
																				</plx:callObject>
																			</plx:callObject>
																		</plx:callObject>
																	</plx:callInstance>
																</plx:passParam>
															</plx:callInstance>
														</plx:right>
													</plx:assign>
												</plx:inlineStatement>
											</plx:passParam>
										</plx:callInstance>
									</xsl:otherwise>
								</xsl:choose>
							</plx:callObject>
							<plx:passParam>
								<plx:callStatic name="IndexOfRole" dataTypeName="FactType">
									<plx:passParam>
										<xsl:choose>
											<xsl:when test="$ContextMatch='constraintRoles'">
												<plx:nameRef name="factRoles"/>
											</xsl:when>
											<xsl:otherwise>
												<plx:callInstance name="OrderedRoleCollection" type="property">
													<plx:callObject>
														<plx:callInstance name="FactType" type="property">
															<plx:callObject>
																<plx:callInstance name="Role" type="property">
																	<plx:callObject>
																		<plx:callInstance name=".implied" type="indexerCall">
																			<plx:callObject>
																				<plx:nameRef name="includedConstraintRoles"/>
																			</plx:callObject>
																			<plx:passParam>
																				<plx:nameRef name="{$IteratorVariableName}"/>
																			</plx:passParam>
																		</plx:callInstance>
																	</plx:callObject>
																</plx:callInstance>
															</plx:callObject>
														</plx:callInstance>
													</plx:callObject>
												</plx:callInstance>
											</xsl:otherwise>
										</xsl:choose>
									</plx:passParam>
									<plx:passParam>
										<xsl:choose>
											<xsl:when test="$ContextMatch='constraintRoles'">
												<plx:nameRef name="primaryRole"/>
											</xsl:when>
											<xsl:otherwise>
												<plx:callInstance name="Role" type="property">
													<plx:callObject>
														<plx:callInstance name=".implied" type="indexerCall">
															<plx:callObject>
																<plx:nameRef name="includedConstraintRoles"/>
															</plx:callObject>
															<plx:passParam>
																<xsl:choose>
																	<xsl:when test="$ConditionalMatch or not(@pass)">
																		<plx:nameRef name="{$IteratorVariableName}"/>
																	</xsl:when>
																	<xsl:otherwise>
																		<plx:value type="i4">
																			<xsl:attribute name="data">
																				<xsl:choose>
																					<xsl:when test="@pass='first'">
																						<xsl:text>0</xsl:text>
																					</xsl:when>
																					<xsl:otherwise>
																						<xsl:text>1</xsl:text>
																					</xsl:otherwise>
																				</xsl:choose>
																			</xsl:attribute>
																		</plx:value>
																	</xsl:otherwise>
																</xsl:choose>
															</plx:passParam>
														</plx:callInstance>
													</plx:callObject>
												</plx:callInstance>
											</xsl:otherwise>
										</xsl:choose>
									</plx:passParam>
								</plx:callStatic>
							</plx:passParam>
						</plx:callInstance>
					</xsl:when>
					<xsl:otherwise>
						<plx:callInstance name=".implied" type="arrayIndexer">
							<plx:callObject>
								<xsl:choose>
									<xsl:when test="$PatternGroup='InternalSetConstraint'">
										<plx:callInstance name=".implied" type="arrayIndexer">
											<plx:callObject>
												<plx:nameRef name="allBasicRoleReplacements"/>
											</plx:callObject>
											<plx:passParam>
												<plx:value data="0" type="i4"/>
											</plx:passParam>
										</plx:callInstance>
									</xsl:when>
									<xsl:otherwise>
										<plx:nameRef name="basicRoleReplacements"/>
									</xsl:otherwise>
								</xsl:choose>
							</plx:callObject>
							<plx:passParam>
								<plx:nameRef name="{$IteratorVariableName}"/>
							</plx:passParam>
						</plx:callInstance>
					</xsl:otherwise>
				</xsl:choose>
			</plx:passParam>
		</plx:callInstance>
	</xsl:template>
	<xsl:template match="cvg:RolePlayerName" mode="ConstraintVerbalization">
		<xsl:param name="VariableDecorator"/>
		<xsl:param name="VariablePrefix"/>
		<xsl:param name="PatternGroup"/>
		<xsl:variable name="useVariablePrefixFragment">
			<xsl:choose>
				<xsl:when test="$VariablePrefix">
					<xsl:value-of select="$VariablePrefix"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text>factText</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="{string($useVariablePrefixFragment)}{$VariableDecorator}"/>
			</plx:left>
			<plx:right>
				<plx:callStatic dataTypeName="VerbalizationHelper" name="NormalizeObjectTypeName">
					<plx:passParam>
						<plx:nameRef name="rolePlayer"/>
					</plx:passParam>
					<plx:passParam>
						<plx:callInstance name="VerbalizationOptions" type="property">
							<plx:callObject>
								<plx:nameRef name="verbalizationContext" type="parameter"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:passParam>
				</plx:callStatic>
			</plx:right>
		</plx:assign>
	</xsl:template>
	<xsl:template match="cvg:RolePlayerReferenceMode" mode="ConstraintVerbalization">
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="''"/>
		<xsl:variable name="useVariablePrefixFragment">
			<xsl:choose>
				<xsl:when test="$VariablePrefix">
					<xsl:value-of select="$VariablePrefix"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text>factText</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="{string($useVariablePrefixFragment)}{$VariableDecorator}"/>
			</plx:left>
			<plx:right>
				<plx:callInstance name="ReferenceModeDecoratedString" type="property">
					<plx:callObject>
						<plx:callInstance name="RolePlayer" type="property">
							<plx:callObject>
								<plx:callInstance name="Role" type="property">
									<plx:callObject>
										<plx:nameRef name="primaryRole"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:callObject>
						</plx:callInstance>
					</plx:callObject>
				</plx:callInstance>
			</plx:right>
		</plx:assign>
	</xsl:template>
	<xsl:template match="cvg:FactInstance" mode="ConstraintVerbalization">
		<xsl:param name="TopLevel" select="false()"/>
		<xsl:call-template name="ProcessFact">
			<xsl:with-param name="TopLevel" select="$TopLevel"/>
		</xsl:call-template>
	</xsl:template>
	<xsl:template match="cvg:Fact" mode="ConstraintVerbalization" name="ProcessFact">
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="''"/>
		<xsl:param name="PatternGroup"/>
		<xsl:param name="FirstPassVariable"/>
		<xsl:param name="IteratorContext" select="''"/>
		<xsl:param name="IteratorVariableName" select="''"/>
		<xsl:param name="TopLevel" select="false()"/>
		<xsl:variable name="useVariablePrefixFragment">
			<xsl:choose>
				<xsl:when test="$VariablePrefix">
					<xsl:value-of select="$VariablePrefix"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text>factText</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="useVariablePrefix" select="string($useVariablePrefixFragment)"/>
		<xsl:if test="$TopLevel">
			<xsl:choose>
				<xsl:when test="position()&gt;1">
					<plx:callInstance name="WriteLine">
						<plx:callObject>
							<plx:nameRef type="parameter" name="writer"/>
						</plx:callObject>
					</plx:callInstance>
				</xsl:when>
				<xsl:otherwise>
					<plx:callInstance name="BeginVerbalization">
						<plx:callObject>
							<plx:nameRef name="verbalizationContext" type="parameter"/>
						</plx:callObject>
						<plx:passParam>
							<plx:callStatic name="Normal" dataTypeName="VerbalizationContent" type="field"/>
						</plx:passParam>
					</plx:callInstance>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>

		<xsl:variable name="callbackReplacements" select="cvg:ProvidedPredicateReplacement"/>
		<xsl:variable name="complexReplacement" select="boolean(cvg:PredicateReplacement) or boolean(ancestor::cvg:Constraint[1]/cvg:EnableSubscripts[@custom])"/>
		<xsl:variable name="standardPopulationSnippet">
			<xsl:call-template name="PopulateReading">
				<xsl:with-param name="ReadingChoice" select="@readingChoice"/>
				<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
				<xsl:with-param name="ConditionalReadingOrderIndex">
					<xsl:if test="$IteratorContext='constraintRoles' or $IteratorContext='providedConstraintRoles' or $IteratorContext='preProjectionKeys'">
						<xsl:value-of select="concat($RoleIterVariablePart,$VariableDecorator)"/>
					</xsl:if>
				</xsl:with-param>
			</xsl:call-template>
			<xsl:choose>
				<xsl:when test="$complexReplacement">
					<xsl:variable name="iterVarName" select="concat($useVariablePrefix,$FactRoleIterVariablePart,$VariableDecorator)"/>
					<xsl:for-each select="descendant::cvg:PredicateReplacement/@once">
						<plx:local name="{.}" dataTypeName=".boolean">
							<plx:initialize>
								<plx:trueKeyword/>
							</plx:initialize>
						</plx:local>
					</xsl:for-each>
					<plx:loop>
						<plx:initializeLoop>
							<plx:local name="{$iterVarName}" dataTypeName=".i4">
								<plx:initialize>
									<plx:value type="i4" data="0"/>
								</plx:initialize>
							</plx:local>
						</plx:initializeLoop>
						<plx:condition>
							<plx:binaryOperator type="lessThan">
								<plx:left>
									<plx:nameRef name="{$iterVarName}"/>
								</plx:left>
								<plx:right>
									<plx:nameRef name="factArity"/>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:beforeLoop>
							<plx:increment>
								<plx:nameRef name="{$iterVarName}"/>
							</plx:increment>
						</plx:beforeLoop>
						<!-- If the basic role replacement is conditional, then don't use the 'basicReplacement' variable -->
						<xsl:variable name="conditionalBasicReplacement" select="boolean(cvg:PredicateReplacement[@alternateRolePlayer or @subscript or @overflowSubscript])"/>
						<!-- Test up front if we have a trivial role replacement (assignment to a fixed string).
						If so, then we have a simple assignment -->
						<xsl:variable name="roleReplacementFragment">
							<!-- Do specialized replacement for different role matches -->
							<xsl:for-each select="cvg:PredicateReplacement">
								<!-- The assumption is made here that predicate replacement quantifiers
								are single-valued. -->
								<xsl:if test="position()=1">
									<xsl:choose>
										<xsl:when test="(position()!=last()) or (string-length(@match) and not(@match='all'))">
											<plx:branch>
												<plx:condition>
													<xsl:call-template name="PredicateReplacementConditionTest">
														<xsl:with-param name="Match" select="@match"/>
														<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
														<xsl:with-param name="IteratorVariableName" select="$IteratorVariableName"/>
														<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
														<xsl:with-param name="VariablePrefix" select="$useVariablePrefix"/>
														<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
														<xsl:with-param name="FirstPassVariable" select="$FirstPassVariable"/>
														<xsl:with-param name="UseOnceVariable" select="@once"/>
													</xsl:call-template>
												</plx:condition>
												<xsl:call-template name="PredicateReplacementBody">
													<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
													<xsl:with-param name="IteratorVariableName" select="$IteratorVariableName"/>
													<xsl:with-param name="FactRoleIteratorVariableName" select="$iterVarName"/>
													<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
													<xsl:with-param name="VariablePrefix" select="$useVariablePrefix"/>
													<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
													<xsl:with-param name="FirstPassVariable" select="$FirstPassVariable"/>
													<xsl:with-param name="UseOnceVariable" select="@once"/>
													<xsl:with-param name="ConditionalBasicReplacement" select="$conditionalBasicReplacement"/>
												</xsl:call-template>
											</plx:branch>
											<xsl:for-each select="following-sibling::*">
												<xsl:choose>
													<xsl:when test="(position()!=last()) or (string-length(@match) and not(@match='all'))">
														<plx:alternateBranch>
															<plx:condition>
																<xsl:call-template name="PredicateReplacementConditionTest">
																	<xsl:with-param name="Match" select="@match"/>
																	<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
																	<xsl:with-param name="IteratorVariableName" select="$IteratorVariableName"/>
																	<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
																	<xsl:with-param name="VariablePrefix" select="$useVariablePrefix"/>
																	<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
																	<xsl:with-param name="FirstPassVariable" select="$FirstPassVariable"/>
																	<xsl:with-param name="UseOnceVariable" select="@once"/>
																</xsl:call-template>
															</plx:condition>
															<xsl:call-template name="PredicateReplacementBody">
																<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
																<xsl:with-param name="IteratorVariableName" select="$IteratorVariableName"/>
																<xsl:with-param name="FactRoleIteratorVariableName" select="$iterVarName"/>
																<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
																<xsl:with-param name="VariablePrefix" select="$useVariablePrefix"/>
																<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
																<xsl:with-param name="FirstPassVariable" select="$FirstPassVariable"/>
																<xsl:with-param name="UseOnceVariable" select="@once"/>
																<xsl:with-param name="ConditionalBasicReplacement" select="$conditionalBasicReplacement"/>
															</xsl:call-template>
														</plx:alternateBranch>
													</xsl:when>
													<xsl:otherwise>
														<plx:fallbackBranch>
															<xsl:call-template name="PredicateReplacementBody">
																<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
																<xsl:with-param name="IteratorVariableName" select="$IteratorVariableName"/>
																<xsl:with-param name="FactRoleIteratorVariableName" select="$iterVarName"/>
																<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
																<xsl:with-param name="VariablePrefix" select="$useVariablePrefix"/>
																<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
																<xsl:with-param name="FirstPassVariable" select="$FirstPassVariable"/>
																<xsl:with-param name="UseOnceVariable" select="@once"/>
																<xsl:with-param name="ConditionalBasicReplacement" select="$conditionalBasicReplacement"/>
															</xsl:call-template>
														</plx:fallbackBranch>
													</xsl:otherwise>
												</xsl:choose>
											</xsl:for-each>
										</xsl:when>
										<xsl:otherwise>
											<xsl:call-template name="PredicateReplacementBody">
												<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
												<xsl:with-param name="IteratorVariableName" select="$IteratorVariableName"/>
												<xsl:with-param name="FactRoleIteratorVariableName" select="$iterVarName"/>
												<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
												<xsl:with-param name="VariablePrefix" select="$useVariablePrefix"/>
												<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
												<xsl:with-param name="FirstPassVariable" select="$FirstPassVariable"/>
												<xsl:with-param name="UseOnceVariable" select="@once"/>
												<xsl:with-param name="ConditionalBasicReplacement" select="$conditionalBasicReplacement"/>
											</xsl:call-template>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:if>
							</xsl:for-each>
						</xsl:variable>
						<xsl:variable name="roleReplacement" select="exsl:node-set($roleReplacementFragment)/child::*"/>
						<xsl:variable name="simpleReplacement" select="count($roleReplacement)=1 and $roleReplacement[self::plx:assign][plx:left[plx:nameRef[@name='roleReplacement']]][plx:right[plx:string]]"/>
						<xsl:if test="not($simpleReplacement)">
							<!-- Initialize variables used for all styles of predicate replacement -->
							<plx:local name="currentRole" dataTypeName="RoleBase">
								<plx:initialize>
									<plx:callInstance name=".implied" type="arrayIndexer">
										<plx:callObject>
											<plx:nameRef name="factRoles"/>
										</plx:callObject>
										<plx:passParam>
											<plx:nameRef name="{$iterVarName}"/>
										</plx:passParam>
									</plx:callInstance>
								</plx:initialize>
							</plx:local>
							<plx:local name="roleReplacement" dataTypeName=".string">
								<plx:initialize>
									<plx:nullKeyword/>
								</plx:initialize>
							</plx:local>
							<xsl:if test="not($conditionalBasicReplacement)">
								<plx:local name="basicReplacement" dataTypeName=".string">
									<plx:initialize>
										<xsl:call-template name="GetBasicReplacement">
											<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
											<xsl:with-param name="SequenceIteratorVariableName" select="$IteratorVariableName"/>
											<xsl:with-param name="FactRoleIteratorVariableName" select="$iterVarName"/>
										</xsl:call-template>
									</plx:initialize>
								</plx:local>
							</xsl:if>
							<xsl:copy-of select="$roleReplacement"/>
							<!-- Use the default replacement for the predicate text if nothing was specified -->
							<plx:branch>
								<plx:condition>
									<plx:binaryOperator type="identityEquality">
										<plx:left>
											<plx:nameRef name="roleReplacement"/>
										</plx:left>
										<plx:right>
											<plx:nullKeyword/>
										</plx:right>
									</plx:binaryOperator>
								</plx:condition>
								<plx:assign>
									<plx:left>
										<plx:nameRef name="roleReplacement"/>
									</plx:left>
									<plx:right>
										<xsl:choose>
											<xsl:when test="$conditionalBasicReplacement">
												<xsl:call-template name="GetBasicReplacement">
													<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
													<xsl:with-param name="SequenceIteratorVariableName" select="$IteratorVariableName"/>
													<xsl:with-param name="FactRoleIteratorVariableName" select="$iterVarName"/>
												</xsl:call-template>
											</xsl:when>
											<xsl:otherwise>
												<plx:nameRef name="basicReplacement"/>
											</xsl:otherwise>
										</xsl:choose>
									</plx:right>
								</plx:assign>
							</plx:branch>
						</xsl:if>
						<plx:assign>
							<plx:left>
								<plx:callInstance name=".implied" type="arrayIndexer">
									<plx:callObject>
										<plx:nameRef name="roleReplacements"/>
									</plx:callObject>
									<plx:passParam>
										<plx:nameRef name="{$iterVarName}"/>
									</plx:passParam>
								</plx:callInstance>
							</plx:left>
							<plx:right>
								<xsl:choose>
									<xsl:when test="$simpleReplacement">
										<xsl:copy-of select="$roleReplacement/plx:right/*"/>
									</xsl:when>
									<xsl:otherwise>
										<plx:nameRef name="roleReplacement"/>
									</xsl:otherwise>
								</xsl:choose>
							</plx:right>
						</plx:assign>
					</plx:loop>
				</xsl:when>
			</xsl:choose>
			<xsl:variable name="predicateText">
				<plx:callInstance name="PopulatePredicateText">
					<plx:callObject>
						<plx:nameRef name="hyphenBinder"/>
					</plx:callObject>
					<plx:passParam>
						<plx:nameRef name="reading"/>
					</plx:passParam>
					<plx:passParam>
						<plx:callInstance name="FormatProvider" type="property">
							<plx:callObject>
								<plx:nameRef type="parameter" name="writer"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:passParam>
					<plx:passParam>
						<plx:nameRef name="predicatePartFormatString"/>
					</plx:passParam>
					<plx:passParam>
						<plx:nameRef name="factRoles"/>
					</plx:passParam>
					<xsl:choose>
						<xsl:when test="$callbackReplacements">
							<plx:passParam>
								<plx:anonymousFunction>
									<plx:param name="replaceRoleIndex" dataTypeName=".i4"/>
									<plx:param name="replaceRole" dataTypeName="RoleBase"/>
									<plx:param name="hyphenBindingFormatString" dataTypeName=".string"/>
									<plx:returns dataTypeName="string"/>
									<xsl:variable name="includedCallbacks" select="$callbackReplacements[@match='included' or @match='primary' or @match='secondary']"/>
									<xsl:if test="$includedCallbacks">
										<plx:iterator dataTypeName="ConstraintRoleSequenceHasRole" localName="constraintRole">
											<plx:initialize>
												<plx:nameRef name="includedConstraintRoles"/>
											</plx:initialize>
											<plx:branch>
												<plx:condition>
													<plx:binaryOperator type="identityEquality">
														<plx:left>
															<plx:nameRef name="replaceRole" type="parameter"/>
														</plx:left>
														<plx:right>
															<plx:callInstance name="Role" type="property">
																<plx:callObject>
																	<plx:nameRef name="constraintRole"/>
																</plx:callObject>
															</plx:callInstance>
														</plx:right>
													</plx:binaryOperator>
												</plx:condition>
												<xsl:variable name="conditionsFragment">
													<xsl:for-each select="$includedCallbacks">
														<!-- Add one condition element per callback, including potentially empty items (marked by a trueKeyword fragment) -->
														<condition>
															<xsl:call-template name="PredicateReplacementConditionTest">
																<xsl:with-param name="Match" select="@match"/>
																<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
																<xsl:with-param name="IteratorVariableName" select="$IteratorVariableName"/>
																<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
																<xsl:with-param name="VariablePrefix" select="$useVariablePrefix"/>
																<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
																<xsl:with-param name="FirstPassVariable" select="$FirstPassVariable"/>
																<xsl:with-param name="CurrentRole" select="'replaceRole'"/>
															</xsl:call-template>
														</condition>
													</xsl:for-each>
												</xsl:variable>
												<xsl:variable name="conditions" select="exsl:node-set($conditionsFragment)/child::*"/>
												<xsl:for-each select="$includedCallbacks">
													<xsl:variable name="currentPosition" select="position()"/>
													<xsl:variable name="condition" select="$conditions[$currentPosition][not(plx:trueKeyword)]/child::*"/>
													<xsl:choose>
														<xsl:when test="$condition">
															<xsl:variable name="branchType">
																<xsl:choose>
																	<xsl:when test="$currentPosition=1">
																		<xsl:text>plx:branch</xsl:text>
																	</xsl:when>
																	<xsl:otherwise>
																		<xsl:text>plx:alternateBranch</xsl:text>
																	</xsl:otherwise>
																</xsl:choose>
															</xsl:variable>
															<xsl:element name="{$branchType}">
																<plx:condition>
																	<xsl:copy-of select="$condition"/>
																</plx:condition>
																<xsl:call-template name="ReturnProvidedRoleReplacement"/>
															</xsl:element>
														</xsl:when>
														<xsl:otherwise>
															<xsl:call-template name="ReturnProvidedRoleReplacement"/>
														</xsl:otherwise>
													</xsl:choose>
												</xsl:for-each>
												<xsl:if test="$conditions[count($conditions)][not(plx:trueKeyword)]">
													<!-- We won't find another matching role. If there is no fallback then exit the loop. -->
													<plx:break/>
												</xsl:if>
											</plx:branch>
										</plx:iterator>
									</xsl:if>
									<xsl:variable name="defaultCallbacks" select="$callbackReplacements[not(@match) or @match='excluded']"/>
									<xsl:choose>
										<xsl:when test="$defaultCallbacks">
											<xsl:variable name="conditionsFragment">
												<xsl:for-each select="$defaultCallbacks">
													<!-- Add one condition element per callback, including potentially empty items (marked by a trueKeyword fragment) -->
													<condition>
														<xsl:call-template name="PredicateReplacementConditionTest">
															<xsl:with-param name="Match" select="@match"/>
															<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
															<xsl:with-param name="IteratorVariableName" select="$IteratorVariableName"/>
															<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
															<xsl:with-param name="VariablePrefix" select="$useVariablePrefix"/>
															<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
															<xsl:with-param name="FirstPassVariable" select="$FirstPassVariable"/>
															<xsl:with-param name="CurrentRole" select="'replaceRole'"/>
														</xsl:call-template>
													</condition>
												</xsl:for-each>
											</xsl:variable>
											<xsl:variable name="conditions" select="exsl:node-set($conditionsFragment)/child::*"/>
											<xsl:for-each select="$defaultCallbacks">
												<xsl:variable name="currentPosition" select="position()"/>
												<xsl:variable name="condition" select="$conditions[$currentPosition][not(plx:trueKeyword)]/child::*"/>
												<xsl:choose>
													<xsl:when test="$condition">
														<xsl:variable name="branchType">
															<xsl:choose>
																<xsl:when test="$currentPosition=1">
																	<xsl:text>plx:branch</xsl:text>
																</xsl:when>
																<xsl:otherwise>
																	<xsl:text>plx:alternateBranch</xsl:text>
																</xsl:otherwise>
															</xsl:choose>
														</xsl:variable>
														<xsl:element name="{$branchType}">
															<plx:condition>
																<xsl:copy-of select="$condition"/>
															</plx:condition>
															<xsl:call-template name="ReturnProvidedRoleReplacement">
																<xsl:with-param name="ProviderKey" select="'replaceRole'"/>
															</xsl:call-template>
														</xsl:element>
													</xsl:when>
													<xsl:otherwise>
														<xsl:call-template name="ReturnProvidedRoleReplacement">
															<xsl:with-param name="ProviderKey" select="'replaceRole'"/>
														</xsl:call-template>
													</xsl:otherwise>
												</xsl:choose>
											</xsl:for-each>
											<xsl:if test="$conditions[count($conditions)][not(plx:trueKeyword)]">
												<!-- We need a final included fallback with default settings -->
												<xsl:call-template name="ReturnProvidedRoleReplacement">
													<xsl:with-param name="Default" select="true()"/>
													<xsl:with-param name="ProviderKey" select="'replaceRole'"/>
												</xsl:call-template>
											</xsl:if>
										</xsl:when>
										<xsl:otherwise>
											<xsl:call-template name="ReturnProvidedRoleReplacement">
												<xsl:with-param name="Default" select="true()"/>
												<xsl:with-param name="ProviderKey" select="'replaceRole'"/>
											</xsl:call-template>
										</xsl:otherwise>
									</xsl:choose>
								</plx:anonymousFunction>
							</plx:passParam>
						</xsl:when>
						<xsl:when test="@useSubscripter">
							<plx:passParam>
								<plx:anonymousFunction>
									<plx:param name="replaceRoleIndex" dataTypeName=".i4"/>
									<plx:param name="replaceRole" dataTypeName="RoleBase"/>
									<plx:param name="hyphenBindingFormatString" dataTypeName=".string"/>
									<plx:returns dataTypeName="string"/>
									<plx:return>
										<plx:inlineStatement dataTypeName=".string">
											<plx:conditionalOperator>
												<plx:condition>
													<plx:binaryOperator type="identityInequality">
														<plx:left>
															<plx:nameRef name="hyphenBindingFormatString"/>
														</plx:left>
														<plx:right>
															<plx:nullKeyword/>
														</plx:right>
													</plx:binaryOperator>
												</plx:condition>
												<plx:left>
													<plx:callStatic dataTypeName=".string" name="Format">
														<plx:passParam>
															<plx:callInstance name="FormatProvider" type="property">
																<plx:callObject>
																	<plx:nameRef name="writer"/>
																</plx:callObject>
															</plx:callInstance>
														</plx:passParam>
														<plx:passParam>
															<plx:nameRef name="hyphenBindingFormatString"/>
														</plx:passParam>
														<plx:passParam>
															<plx:callInstance name="GetSubscriptedName">
																<plx:callObject>
																	<plx:nameRef name="subscripter"/>
																</plx:callObject>
																<plx:passParam>
																	<plx:nameRef name="replaceRoleIndex"/>
																</plx:passParam>
																<plx:passParam>
																	<plx:nameRef name="basicRoleReplacements"/>
																</plx:passParam>
															</plx:callInstance>
														</plx:passParam>
													</plx:callStatic>
												</plx:left>
												<plx:right>
													<plx:callInstance name="GetSubscriptedName">
														<plx:callObject>
															<plx:nameRef name="subscripter"/>
														</plx:callObject>
														<plx:passParam>
															<plx:nameRef name="replaceRoleIndex"/>
														</plx:passParam>
														<plx:passParam>
															<plx:nameRef name="basicRoleReplacements"/>
														</plx:passParam>
													</plx:callInstance>
												</plx:right>
											</plx:conditionalOperator>
										</plx:inlineStatement>
									</plx:return>
								</plx:anonymousFunction>
							</plx:passParam>
						</xsl:when>
						<xsl:otherwise>
							<plx:passParam>
								<xsl:choose>
									<xsl:when test="$complexReplacement">
										<plx:nameRef name="roleReplacements"/>
									</xsl:when>
									<xsl:when test="$PatternGroup='InternalSetConstraint'">
										<plx:callInstance name=".implied" type="arrayIndexer">
											<plx:callObject>
												<plx:nameRef name="allBasicRoleReplacements"/>
											</plx:callObject>
											<plx:passParam>
												<plx:value data="0" type="i4"/>
											</plx:passParam>
										</plx:callInstance>
									</xsl:when>
									<xsl:otherwise>
										<plx:nameRef name="basicRoleReplacements"/>
									</xsl:otherwise>
								</xsl:choose>
							</plx:passParam>
							<plx:passParam>
								<xsl:choose>
									<xsl:when test="$complexReplacement">
										<plx:falseKeyword/>
									</xsl:when>
									<xsl:otherwise>
										<plx:trueKeyword/>
									</xsl:otherwise>
								</xsl:choose>
							</plx:passParam>
						</xsl:otherwise>
					</xsl:choose>
				</plx:callInstance>
			</xsl:variable>
			<xsl:choose>
				<xsl:when test="$TopLevel">
					<plx:callStatic name="WriteVerbalizerSentence" dataTypeName="FactType">
						<plx:passParam>
							<plx:nameRef type="parameter" name="writer"/>
						</plx:passParam>
						<plx:passParam>
							<xsl:copy-of select="$predicateText"/>
						</plx:passParam>
						<plx:passParam>
							<xsl:call-template name="SnippetFor">
								<xsl:with-param name="SnippetType" select="'CloseVerbalizationSentence'"/>
							</xsl:call-template>
						</plx:passParam>
					</plx:callStatic>
				</xsl:when>
				<xsl:otherwise>
					<plx:assign>
						<plx:left>
							<plx:nameRef name="{$useVariablePrefix}{$VariableDecorator}"/>
						</plx:left>
						<plx:right>
							<xsl:choose>
								<xsl:when test="@closeSentence='true' or @closeSentence='1'">
									<plx:callStatic name="CreateVerbalizerSentence" dataTypeName="FactType">
										<plx:passParam>
											<xsl:copy-of select="$predicateText"/>
										</plx:passParam>
										<plx:passParam>
											<xsl:call-template name="SnippetFor">
												<xsl:with-param name="SnippetType" select="'CloseVerbalizationSentence'"/>
											</xsl:call-template>
										</plx:passParam>
									</plx:callStatic>
								</xsl:when>
								<xsl:otherwise>
									<xsl:copy-of select="$predicateText"/>
								</xsl:otherwise>
							</xsl:choose>
						</plx:right>
					</plx:assign>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="@subtypeMetaReading='true' or @subtypeMetaReading='1'">
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="identityInequality">
							<plx:left>
								<plx:nameRef name="parentSubtypeFact"/>
							</plx:left>
							<plx:right>
								<plx:nullKeyword/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<xsl:variable name="predicateText">
						<plx:callStatic name="Format" dataTypeName=".string">
							<plx:passParam>
								<plx:callInstance name="FormatProvider" type="property">
									<plx:callObject>
										<plx:nameRef type="parameter" name="writer"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:passParam>
							<plx:passParam>
								<xsl:call-template name="SnippetFor">
									<xsl:with-param name="SnippetType" select="'SubtypeMetaReading'"/>
									<xsl:with-param name="IsNegativeSnippet">
										<plx:falseKeyword/>
									</xsl:with-param>
								</xsl:call-template>
							</plx:passParam>
							<plx:passParam>
								<plx:callInstance name=".implied" type="arrayIndexer">
									<plx:callObject>
										<plx:nameRef name="basicRoleReplacements"/>
									</plx:callObject>
									<plx:passParam>
										<plx:value data="0" type="i4"/>
									</plx:passParam>
								</plx:callInstance>
							</plx:passParam>
							<plx:passParam>
								<plx:callInstance name=".implied" type="arrayIndexer">
									<plx:callObject>
										<plx:nameRef name="basicRoleReplacements"/>
									</plx:callObject>
									<plx:passParam>
										<plx:value data="1" type="i4"/>
									</plx:passParam>
								</plx:callInstance>
							</plx:passParam>
							<plx:passParam>
								<plx:callInstance name="ToString">
									<plx:callObject>
										<plx:callInstance name="Id" type="property">
											<plx:callObject>
												<plx:nameRef name="parentFact"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:callObject>
									<plx:passParam>
										<plx:string data="D"/>
									</plx:passParam>
								</plx:callInstance>
							</plx:passParam>
						</plx:callStatic>
					</xsl:variable>
					<xsl:choose>
						<xsl:when test="$TopLevel">
							<plx:callStatic name="WriteVerbalizerSentence" dataTypeName="FactType">
								<plx:passParam>
									<plx:nameRef type="parameter" name="writer"/>
								</plx:passParam>
								<plx:passParam>
									<xsl:copy-of select="$predicateText"/>
								</plx:passParam>
								<plx:passParam>
									<xsl:call-template name="SnippetFor">
										<xsl:with-param name="SnippetType" select="'CloseVerbalizationSentence'"/>
									</xsl:call-template>
								</plx:passParam>
							</plx:callStatic>
						</xsl:when>
						<xsl:otherwise>
							<plx:assign>
								<plx:left>
									<plx:nameRef name="{$useVariablePrefix}{$VariableDecorator}"/>
								</plx:left>
								<plx:right>
									<xsl:choose>
										<xsl:when test="@closeSentence='true' or @closeSentence='1'">
											<plx:callStatic name="CreateVerbalizerSentence" dataTypeName="FactType">
												<plx:passParam>
													<xsl:copy-of select="$predicateText"/>
												</plx:passParam>
												<plx:passParam>
													<xsl:call-template name="SnippetFor">
														<xsl:with-param name="SnippetType" select="'CloseVerbalizationSentence'"/>
													</xsl:call-template>
												</plx:passParam>
											</plx:callStatic>
										</xsl:when>
										<xsl:otherwise>
											<xsl:copy-of select="$predicateText"/>
										</xsl:otherwise>
									</xsl:choose>
								</plx:right>
							</plx:assign>
						</xsl:otherwise>
					</xsl:choose>
				</plx:branch>
				<plx:fallbackBranch>
					<xsl:copy-of select="$standardPopulationSnippet"/>
				</plx:fallbackBranch>
			</xsl:when>
			<xsl:otherwise>
				<xsl:copy-of select="$standardPopulationSnippet"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="ReturnProvidedRoleReplacement">
		<xsl:param name="Context" select="."/>
		<xsl:param name="Default" select="false()"/>
		<xsl:param name="ProviderKey" select="'constraintRole'"/>
		<plx:return>
			<xsl:for-each select="$Context">
				<xsl:variable name="formattingSnippet" select="string(cvg:Snippet/@ref)"/>
				<xsl:choose>
					<xsl:when test="$formattingSnippet='null'">
						<plx:string/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:variable name="providedReplacementFragment">
							<xsl:choose>
								<xsl:when test="$Default">
									<plx:callInstance name="RenderAssociatedRolePlayer">
										<plx:callObject>
											<plx:nameRef name="pathVerbalizer"/>
										</plx:callObject>
										<plx:passParam>
											<plx:nameRef name="{$ProviderKey}"/>
										</plx:passParam>
										<plx:passParam>
											<plx:nameRef name="hyphenBindingFormatString"/>
										</plx:passParam>
										<plx:passParam>
											<plx:callStatic dataTypeName="RolePathRolePlayerRenderingOptions" name="None" type="field"/>
										</plx:passParam>
									</plx:callInstance>
								</xsl:when>
								<xsl:otherwise>
									<plx:callInstance name="RenderAssociatedRolePlayer">
										<plx:callObject>
											<plx:nameRef name="pathVerbalizer"/>
										</plx:callObject>
										<plx:passParam>
											<plx:nameRef name="{$ProviderKey}"/>
										</plx:passParam>
										<plx:passParam>
											<plx:nameRef name="hyphenBindingFormatString"/>
										</plx:passParam>
										<plx:passParam>
											<xsl:call-template name="GetRolePlayerRenderingOptions"/>
										</plx:passParam>
									</plx:callInstance>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="$formattingSnippet">
								<plx:callStatic name="Format" dataTypeName=".string">
									<plx:passParam>
										<plx:callInstance name="FormatProvider" type="property">
											<plx:callObject>
												<plx:nameRef type="parameter" name="writer"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:passParam>
									<plx:passParam>
										<xsl:call-template name="SnippetFor">
											<xsl:with-param name="SnippetType" select="$formattingSnippet"/>
										</xsl:call-template>
									</plx:passParam>
									<plx:passParam>
										<xsl:copy-of select="$providedReplacementFragment"/>
									</plx:passParam>
								</plx:callStatic>
							</xsl:when>
							<xsl:otherwise>
								<xsl:copy-of select="$providedReplacementFragment"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:for-each>
		</plx:return>
	</xsl:template>
	<xsl:template name="GetRolePlayerRenderingOptions">
		<xsl:variable name="optionsFragment">
			<xsl:if test="@quantify='true' or @quantify='1'">
				<plx:callStatic dataTypeName="RolePathRolePlayerRenderingOptions" name="Quantify" type="field"/>
			</xsl:if>
			<xsl:if test="@markAsHead='true' or @markAsHead='1'">
				<plx:callStatic dataTypeName="RolePathRolePlayerRenderingOptions" name="UsedInVerbalizationHead" type="field"/>
				<xsl:if test="@minimizeHeadSubscripting='true' or @minimizeHeadSubscripting='1'">
					<plx:callStatic dataTypeName="RolePathRolePlayerRenderingOptions" name="MinimizeHeadSubscripting" type="field"/>
				</xsl:if>
			</xsl:if>
		</xsl:variable>
		<xsl:variable name="options" select="exsl:node-set($optionsFragment)/child::*"/>
		<xsl:choose>
			<xsl:when test="$options">
				<xsl:call-template name="CombineElements">
					<xsl:with-param name="OperatorType" select="'bitwiseOr'"/>
					<xsl:with-param name="Elements" select="$options"/>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<plx:callStatic dataTypeName="RolePathRolePlayerRenderingOptions" name="None" type="field"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="GetBasicReplacement">
		<xsl:param name="PatternGroup"/>
		<xsl:param name="SequenceIteratorVariableName"/>
		<xsl:param name="FactRoleIteratorVariableName"/>
		<xsl:param name="AlternateRolePlayer" select="''"/>
		<xsl:param name="ExplicitSubscript" select="''"/>
		<xsl:param name="OverflowSubscript" select="''"/>
		<plx:callInstance name="HyphenBindRoleReplacement">
			<plx:callObject>
				<plx:nameRef name="hyphenBinder"/>
			</plx:callObject>
			<plx:passParam>
				<xsl:variable name="replacementFragment">
					<xsl:variable name="replacementsFragment">
						<xsl:choose>
							<xsl:when test="$PatternGroup='InternalSetConstraint' or $PatternGroup='SetComparisonConstraint'">
								<plx:callInstance name=".implied" type="arrayIndexer">
									<plx:callObject>
										<plx:nameRef name="allBasicRoleReplacements"/>
									</plx:callObject>
									<plx:passParam>
										<xsl:choose>
											<xsl:when test="$PatternGroup='SetComparisonConstraint'">
												<plx:callInstance name="IndexOf">
													<plx:callObject>
														<plx:nameRef name="allFacts" type="local"/>
													</plx:callObject>
													<plx:passParam>
														<plx:callInstance name="FactType" type="property">
															<plx:callObject>
																<plx:callInstance name="Role" type="property">
																	<plx:callObject>
																		<plx:callInstance name=".implied" type="arrayIndexer">
																			<plx:callObject>
																				<plx:nameRef name="includedConstraintRoles"/>
																			</plx:callObject>
																			<plx:passParam>
																				<plx:nameRef name="{$SequenceIteratorVariableName}"/>
																			</plx:passParam>
																		</plx:callInstance>
																	</plx:callObject>
																</plx:callInstance>
															</plx:callObject>
														</plx:callInstance>
													</plx:passParam>
												</plx:callInstance>
											</xsl:when>
											<xsl:otherwise>
												<plx:value data="0" type="i4"/>
											</xsl:otherwise>
										</xsl:choose>
									</plx:passParam>
								</plx:callInstance>
							</xsl:when>
							<xsl:otherwise>
								<plx:nameRef name="basicRoleReplacements"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<xsl:variable name="replacementIndexFragment">
						<xsl:choose>
							<xsl:when test="$AlternateRolePlayer">
								<xsl:if test="not($PatternGroup='InternalSetConstraint')">
									<xsl:message terminate="yes">
										<xsl:text>AlternateRolePlayer semantics currently implemented with InternalSetConstraint pattern only, not</xsl:text>
										<xsl:value-of select="$PatternGroup"/>
										<xsl:text>pattern.</xsl:text>
									</xsl:message>
								</xsl:if>
								<plx:callInstance name="IndexOf">
									<plx:callObject>
										<plx:nameRef name="factRoles"/>
									</plx:callObject>
									<plx:passParam>
										<xsl:choose>
											<xsl:when test="$AlternateRolePlayer='primary'">
												<plx:nameRef name="primaryRole"/>
											</xsl:when>
											<xsl:otherwise>
												<plx:callInstance name=".implied" type="arrayIndexer">
													<plx:callObject>
														<plx:nameRef name="allConstraintRoles"/>
													</plx:callObject>
													<plx:passParam>
														<plx:inlineStatement dataTypeName=".i4">
															<plx:conditionalOperator>
																<plx:condition>
																	<plx:binaryOperator type="equality">
																		<plx:left>
																			<plx:callInstance name="IndexOf">
																				<plx:callObject>
																					<plx:nameRef name="allConstraintRoles"/>
																				</plx:callObject>
																				<plx:passParam>
																					<plx:callInstance name="Role" type="property">
																						<plx:callObject>
																							<plx:nameRef name="currentRole"/>
																						</plx:callObject>
																					</plx:callInstance>
																				</plx:passParam>
																			</plx:callInstance>
																		</plx:left>
																		<plx:right>
																			<plx:value data="0" type="i4"/>
																		</plx:right>
																	</plx:binaryOperator>
																</plx:condition>
																<plx:left>
																	<plx:value data="1" type="i4"/>
																</plx:left>
																<plx:right>
																	<plx:value data="0" type="i4"/>
																</plx:right>
															</plx:conditionalOperator>
														</plx:inlineStatement>
													</plx:passParam>
												</plx:callInstance>
											</xsl:otherwise>
										</xsl:choose>
									</plx:passParam>
								</plx:callInstance>
							</xsl:when>
							<xsl:otherwise>
								<plx:nameRef name="{$FactRoleIteratorVariableName}"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<xsl:variable name="customSubscript" select="string(ancestor::cvg:Constraint[1]/cvg:EnableSubscripts/@custom[.='defaultPlain' or .='defaultSubscript'])"/>
					<xsl:variable name="iteratorSubscript" select="string(ancestor::*[self::cvg:IterateRoles or self::cvg:IterateContextRoles][@subscript][1]/@subscript)"/>
					<xsl:choose>
						<xsl:when test="not($OverflowSubscript) and $customSubscript and (($customSubscript='defaultSubscript' and not((not($ExplicitSubscript) and ($iteratorSubscript='false' or $iteratorSubscript='0')) or $ExplicitSubscript='false' or $ExplicitSubscript='0')) or (not($ExplicitSubscript) and ($iteratorSubscript='true' or $iteratorSubscript='1')) or $ExplicitSubscript='true' or $ExplicitSubscript='1')">
							<plx:callInstance name="GetSubscriptedName">
								<plx:callObject>
									<plx:nameRef name="subscripter"/>
								</plx:callObject>
								<plx:passParam>
									<xsl:copy-of select="$replacementIndexFragment"/>
								</plx:passParam>
								<plx:passParam>
									<xsl:copy-of select="$replacementsFragment"/>
								</plx:passParam>
							</plx:callInstance>
						</xsl:when>
						<xsl:otherwise>
							<plx:callInstance name=".implied" type="arrayIndexer">
								<plx:callObject>
									<xsl:copy-of select="$replacementsFragment"/>
								</plx:callObject>
								<plx:passParam>
									<xsl:copy-of select="$replacementIndexFragment"/>
								</plx:passParam>
								<xsl:if test="$customSubscript or $OverflowSubscript">
									<plx:passParam>
										<plx:value type="i4" data="0">
											<xsl:if test="$OverflowSubscript">
												<xsl:attribute name="data">
													<xsl:text>3</xsl:text>
												</xsl:attribute>
											</xsl:if>
										</plx:value>
									</plx:passParam>
								</xsl:if>
							</plx:callInstance>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:choose>
					<xsl:when test="$OverflowSubscript">
						<plx:callStatic dataTypeName=".string" name="Format">
							<plx:passParam>
								<plx:callInstance name="FormatProvider" type="property">
									<plx:callObject>
										<plx:nameRef type="parameter" name="writer"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:passParam>
							<plx:passParam>
								<xsl:copy-of select="$replacementFragment"/>
							</plx:passParam>
							<plx:passParam>
								<plx:binaryOperator type="add">
									<plx:left>
										<plx:callStatic name="EnumerableTrueCount" dataTypeName="Utility" dataTypeQualifier="ORMSolutions.ORMArchitect.Framework">
											<plx:passParam>
												<plx:nameRef name="factRoles"/>
											</plx:passParam>
											<plx:passParam>
												<plx:anonymousFunction>
													<plx:param name="matchRoleBase" dataTypeName="RoleBase"/>
													<plx:returns dataTypeName=".boolean"/>
													<plx:local name="compareToRolePlayer" dataTypeName="ObjectType">
														<plx:initialize>
															<plx:callInstance name="RolePlayer" type="property">
																<plx:callObject>
																	<plx:callInstance name="Role" type="property">
																		<plx:callObject>
																			<xsl:choose>
																				<xsl:when test="$AlternateRolePlayer='primary'">
																					<plx:nameRef name="primaryRole"/>
																				</xsl:when>
																				<xsl:when test="$AlternateRolePlayer='other'">
																					<plx:callInstance name=".implied" type="arrayIndexer">
																						<plx:callObject>
																							<plx:nameRef name="allConstraintRoles"/>
																						</plx:callObject>
																						<plx:passParam>
																							<plx:inlineStatement dataTypeName=".i4">
																								<plx:conditionalOperator>
																									<plx:condition>
																										<plx:binaryOperator type="equality">
																											<plx:left>
																												<plx:callInstance name="IndexOf">
																													<plx:callObject>
																														<plx:nameRef name="allConstraintRoles"/>
																													</plx:callObject>
																													<plx:passParam>
																														<plx:callInstance name="Role" type="property">
																															<plx:callObject>
																																<plx:nameRef name="currentRole"/>
																															</plx:callObject>
																														</plx:callInstance>
																													</plx:passParam>
																												</plx:callInstance>
																											</plx:left>
																											<plx:right>
																												<plx:value data="0" type="i4"/>
																											</plx:right>
																										</plx:binaryOperator>
																									</plx:condition>
																									<plx:left>
																										<plx:value data="1" type="i4"/>
																									</plx:left>
																									<plx:right>
																										<plx:value data="0" type="i4"/>
																									</plx:right>
																								</plx:conditionalOperator>
																							</plx:inlineStatement>
																						</plx:passParam>
																					</plx:callInstance>
																				</xsl:when>
																				<xsl:otherwise>
																					<plx:nameRef name="currentRole"/>
																				</xsl:otherwise>
																			</xsl:choose>
																		</plx:callObject>
																	</plx:callInstance>
																</plx:callObject>
															</plx:callInstance>
														</plx:initialize>
													</plx:local>
													<plx:return>
														<plx:binaryOperator type="booleanAnd">
															<plx:left>
																<plx:binaryOperator type="identityInequality">
																	<plx:left>
																		<plx:nameRef name="compareToRolePlayer"/>
																	</plx:left>
																	<plx:right>
																		<plx:nullKeyword/>
																	</plx:right>
																</plx:binaryOperator>
															</plx:left>
															<plx:right>
																<plx:binaryOperator type="identityEquality">
																	<plx:left>
																		<plx:nameRef name="compareToRolePlayer"/>
																	</plx:left>
																	<plx:right>
																		<plx:callInstance name="RolePlayer" type="property">
																			<plx:callObject>
																				<plx:callInstance name="Role" type="property">
																					<plx:callObject>
																						<plx:nameRef name="matchRoleBase"/>
																					</plx:callObject>
																				</plx:callInstance>
																			</plx:callObject>
																		</plx:callInstance>
																	</plx:right>
																</plx:binaryOperator>
															</plx:right>
														</plx:binaryOperator>
													</plx:return>
												</plx:anonymousFunction>
											</plx:passParam>
										</plx:callStatic>
									</plx:left>
									<plx:right>
										<plx:value data="{$OverflowSubscript}" type="i4"/>
									</plx:right>
								</plx:binaryOperator>
							</plx:passParam>
						</plx:callStatic>
					</xsl:when>
					<xsl:otherwise>
						<xsl:copy-of select="$replacementFragment"/>
					</xsl:otherwise>
				</xsl:choose>
			</plx:passParam>
			<plx:passParam>
				<plx:nameRef name="{$FactRoleIteratorVariableName}"/>
			</plx:passParam>
		</plx:callInstance>
	</xsl:template>
	<!-- Handle the minFactArity EnableSubscripts condition attribute -->
	<xsl:template match="@minFactArity" mode="SubscriptFilterOperators">
		<plx:binaryOperator type="greaterThanOrEqual">
			<plx:left>
				<plx:nameRef name="factArity"/>
			</plx:left>
			<plx:right>
				<plx:value type="i4" data="{.}"/>
			</plx:right>
		</plx:binaryOperator>
	</xsl:template>
	<!-- Handle the maxFactArity EnableSubscripts condition attribute -->
	<xsl:template match="@maxFactArity" mode="SubscriptFilterOperators">
		<plx:binaryOperator type="lessThanOrEqual">
			<plx:left>
				<plx:nameRef name="factArity"/>
			</plx:left>
			<plx:right>
				<plx:value type="i4" data="{.}"/>
			</plx:right>
		</plx:binaryOperator>
	</xsl:template>
	<!-- Handle the factArity EnableSubscripts condition attribute -->
	<xsl:template match="@factArity" mode="SubscriptFilterOperators">
		<plx:binaryOperator type="equality">
			<plx:left>
				<plx:nameRef name="factArity"/>
			</plx:left>
			<plx:right>
				<plx:value type="i4" data="{.}"/>
			</plx:right>
		</plx:binaryOperator>
	</xsl:template>
	<!-- Handle the factCount EnableSubscripts condition attribute -->
	<xsl:template match="@factCount" mode="SubscriptFilterOperators">
		<plx:binaryOperator type="equality">
			<plx:left>
				<plx:nameRef name="allFactsCount"/>
			</plx:left>
			<plx:right>
				<plx:value type="i4" data="{.}"/>
			</plx:right>
		</plx:binaryOperator>
	</xsl:template>
	<!-- Handle the minFactCount EnableSubscripts condition attribute -->
	<xsl:template match="@minFactCount" mode="SubscriptFilterOperators">
		<plx:binaryOperator type="greaterThanOrEqual">
			<plx:left>
				<plx:nameRef name="allFactsCount"/>
			</plx:left>
			<plx:right>
				<plx:value type="i4" data="{.}"/>
			</plx:right>
		</plx:binaryOperator>
	</xsl:template>
	<!-- Handle the maxFactCount EnableSubscripts condition attribute -->
	<xsl:template match="@maxFactCount" mode="SubscriptFilterOperators">
		<plx:binaryOperator type="lessThanOrEqual">
			<plx:left>
				<plx:nameRef name="allFactsCount"/>
			</plx:left>
			<plx:right>
				<plx:value type="i4" data="{.}"/>
			</plx:right>
		</plx:binaryOperator>
	</xsl:template>
	<!-- Handle the nonOptimizedFrequencyPattern EnableSubscripts condition attribute -->
	<xsl:template match="@nonOptimizedFrequencyPattern" mode="SubscriptFilterOperators">
		<xsl:if test=".='true' or .='1'">
			<plx:binaryOperator type="booleanOr">
				<plx:left>
					<plx:binaryOperator type="booleanOr">
						<plx:left>
							<plx:binaryOperator type="booleanOr">
								<plx:left>
									<plx:binaryOperator type="inequality">
										<plx:left>
											<plx:nameRef name="factArity"/>
										</plx:left>
										<plx:right>
											<plx:value data="2" type="i4"/>
										</plx:right>
									</plx:binaryOperator>
								</plx:left>
								<plx:right>
									<plx:binaryOperator type="inequality">
										<plx:left>
											<plx:callThis name="MinFrequency" type="property"/>
										</plx:left>
										<plx:right>
											<plx:value data="1" type="i4"/>
										</plx:right>
									</plx:binaryOperator>
								</plx:right>
							</plx:binaryOperator>
						</plx:left>
						<plx:right>
							<plx:binaryOperator type="booleanOr">
								<plx:left>
									<plx:binaryOperator type="lessThanOrEqual">
										<plx:left>
											<plx:callThis name="MaxFrequency" type="property"/>
										</plx:left>
										<plx:right>
											<plx:value data="1" type="i4"/>
										</plx:right>
									</plx:binaryOperator>
								</plx:left>
								<plx:right>
									<plx:binaryOperator type="inequality">
										<plx:left>
											<plx:callInstance name="Count" type="property">
												<plx:callObject>
													<plx:nameRef name="allConstraintRoles"/>
												</plx:callObject>
											</plx:callInstance>
										</plx:left>
										<plx:right>
											<plx:value data="1" type="i4"/>
										</plx:right>
									</plx:binaryOperator>
								</plx:right>
							</plx:binaryOperator>
						</plx:right>
					</plx:binaryOperator>
				</plx:left>
				<plx:right>
					<plx:binaryOperator type="identityEquality">
						<plx:left>
							<plx:nullKeyword/>
						</plx:left>
						<plx:right>
							<plx:callInstance name="FindMatchingReadingOrder">
								<plx:callObject>
									<plx:callInstance name="FactType" type="property">
										<plx:callObject>
											<plx:callInstance name=".implied" type="indexerCall">
												<plx:callObject>
													<plx:nameRef name="allConstraintRoles"/>
												</plx:callObject>
												<plx:passParam>
													<plx:value data="0" type="i4"/>
												</plx:passParam>
											</plx:callInstance>
										</plx:callObject>
									</plx:callInstance>
								</plx:callObject>
								<plx:passParam>
									<plx:callNew dataTypeIsSimpleArray="true" dataTypeName="RoleBase">
										<plx:arrayInitializer>
											<plx:callInstance name=".implied" type="indexerCall">
												<plx:callObject>
													<plx:nameRef name="allConstraintRoles"/>
												</plx:callObject>
												<plx:passParam>
													<plx:value data="0" type="i4"/>
												</plx:passParam>
											</plx:callInstance>
										</plx:arrayInitializer>
									</plx:callNew>
								</plx:passParam>
							</plx:callInstance>
						</plx:right>
					</plx:binaryOperator>
				</plx:right>
			</plx:binaryOperator>
		</xsl:if>
	</xsl:template>
	<!-- Add attributes with no filter implications here -->
	<xsl:template match="@custom" mode="SubscriptFilterOperators"/>
	<!-- Terminate processing if we see an unrecognized operator -->
	<xsl:template match="@*" mode="SubscriptFilterOperators">
		<xsl:call-template name="TerminateForInvalidAttribute">
			<xsl:with-param name="MessageText">Unrecognized subscript condition iterator filter attribute</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<!-- Terminate processing if we see an unrecognized operator -->
	<xsl:template match="@*" mode="IterateRolesFilterOperator">
		<xsl:call-template name="TerminateForInvalidAttribute">
			<xsl:with-param name="MessageText">Unrecognized subscript condition iterator filter attribute</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<xsl:template match="cvg:EnableSubscripts" mode="SubscriptConditions">
		<xsl:variable name="conditionalsFragment">
			<xsl:apply-templates select="@*" mode="SubscriptFilterOperators"/>
		</xsl:variable>
		<xsl:variable name="conditions" select="exsl:node-set($conditionalsFragment)/child::*"/>
		<xsl:choose>
			<xsl:when test="$conditions">
				<xsl:call-template name="CombineElements">
					<xsl:with-param name="OperatorType" select="'booleanAnd'"/>
					<xsl:with-param name="Elements" select="$conditions"/>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<plx:trueKeyword/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="cvg:EnableSubscripts" mode="ConstraintVerbalization">
		<!-- Don't do anything in this mode. We preprocess these directive elements -->
	</xsl:template>
	<xsl:template name="PredicateReplacementConditionTest">
		<xsl:param name="Match"/>
		<xsl:param name="IteratorContext"/>
		<xsl:param name="PatternGroup"/>
		<xsl:param name="VariablePrefix"/>
		<xsl:param name="VariableDecorator"/>
		<xsl:param name="FirstPassVariable"/>
		<xsl:param name="UseOnceVariable"/>
		<xsl:param name="PrimaryRole" select="'primaryRole'"/>
		<xsl:param name="CurrentRole" select="'currentRole'"/>
		<xsl:variable name="includedRolesFragment">
			<xsl:choose>
				<xsl:when test="$PatternGroup='InternalSetConstraint' or ($PatternGroup='SetConstraint' and $Match='included')">
					<xsl:text>allConstraintRoles</xsl:text>
				</xsl:when>
				<xsl:when test="self::cvg:ProvidedPredicateReplacement">
					<xsl:text>constraintRole</xsl:text>
				</xsl:when>
				<xsl:when test="$PatternGroup='SetComparisonConstraint' and $Match='included'">
					<xsl:message terminate="yes">PredicateReplacement no longer supported for SetComparisonConstraint, use ProvidedPredicateReplacement instead.</xsl:message>
					<xsl:text>includedSequenceRoles</xsl:text>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text>includedRoles</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="includedRoles" select="string($includedRolesFragment)"/>
		<xsl:variable name="operatorsFragment">
			<xsl:if test="$UseOnceVariable">
				<plx:nameRef name="{$UseOnceVariable}"/>
			</xsl:if>
			<xsl:choose>
				<xsl:when test="$Match='primary'">
					<plx:binaryOperator type="identityEquality">
						<plx:left>
							<plx:nameRef name="{$PrimaryRole}"/>
						</plx:left>
						<plx:right>
							<plx:nameRef name="{$CurrentRole}"/>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$Match='secondary'">
					<xsl:choose>
						<xsl:when test="$IteratorContext='included'">
							<plx:binaryOperator type="identityInequality">
								<plx:left>
									<plx:nameRef name="{$PrimaryRole}"/>
								</plx:left>
								<plx:right>
									<plx:nameRef name="{$CurrentRole}"/>
								</plx:right>
							</plx:binaryOperator>
							<plx:callInstance name="Contains">
								<plx:callObject>
									<plx:nameRef name="{$includedRoles}"/>
								</plx:callObject>
								<plx:passParam>
									<plx:callInstance name="Role" type="property">
										<plx:callObject>
											<plx:nameRef name="{$CurrentRole}"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:passParam>
							</plx:callInstance>
						</xsl:when>
						<xsl:when test="$IteratorContext='excluded'">
							<plx:binaryOperator type="identityInequality">
								<plx:left>
									<plx:nameRef name="{$PrimaryRole}"/>
								</plx:left>
								<plx:right>
									<plx:nameRef name="{$CurrentRole}"/>
								</plx:right>
							</plx:binaryOperator>
							<plx:unaryOperator type="booleanNot">
								<plx:callInstance name="Contains">
									<plx:callObject>
										<plx:nameRef name="{$includedRoles}"/>
									</plx:callObject>
									<plx:passParam>
										<plx:callInstance name="Role" type="property">
											<plx:callObject>
												<plx:nameRef name="{$CurrentRole}"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:passParam>
								</plx:callInstance>
							</plx:unaryOperator>
						</xsl:when>
						<xsl:otherwise>
							<plx:binaryOperator type="identityInequality">
								<plx:left>
									<plx:nameRef name="{$PrimaryRole}"/>
								</plx:left>
								<plx:right>
									<plx:nameRef name="{$CurrentRole}"/>
								</plx:right>
							</plx:binaryOperator>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:when test="not(self::cvg:ProvidedPredicateReplacement)">
					<xsl:choose>
						<xsl:when test="$Match='included'">
							<plx:callInstance name="Contains">
								<plx:callObject>
									<plx:nameRef name="{$includedRoles}"/>
								</plx:callObject>
								<plx:passParam>
									<plx:callInstance name="Role" type="property">
										<plx:callObject>
											<plx:nameRef name="{$CurrentRole}"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:passParam>
							</plx:callInstance>
						</xsl:when>
						<xsl:when test="$Match='excluded'">
							<plx:unaryOperator type="booleanNot">
								<plx:callInstance name="Contains">
									<plx:callObject>
										<plx:nameRef name="{$includedRoles}"/>
									</plx:callObject>
									<plx:passParam>
										<plx:callInstance name="Role" type="property">
											<plx:callObject>
												<plx:nameRef name="{$CurrentRole}"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:passParam>
								</plx:callInstance>
							</plx:unaryOperator>
						</xsl:when>
					</xsl:choose>
				</xsl:when>
			</xsl:choose>
			<xsl:choose>
				<xsl:when test="@pass='notFirst'">
					<plx:unaryOperator type="booleanNot">
						<plx:nameRef name="{$FirstPassVariable}"/>
					</plx:unaryOperator>
				</xsl:when>
				<xsl:when test="@pass='first'">
					<plx:nameRef name="{$FirstPassVariable}"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="operators" select="exsl:node-set($operatorsFragment)/child::*"/>
		<xsl:choose>
			<xsl:when test="$operators">
				<xsl:call-template name="CombineElements">
					<xsl:with-param name="OperatorType" select="'booleanAnd'"/>
					<xsl:with-param name="Elements" select="$operators"/>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<plx:trueKeyword/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="PredicateReplacementBody">
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="''"/>
		<xsl:param name="PatternGroup"/>
		<xsl:param name="FirstPassVariable"/>
		<xsl:param name="UseOnceVariable"/>
		<xsl:param name="IteratorContext" select="''"/>
		<xsl:param name="IteratorVariableName" select="''"/>
		<xsl:param name="FactRoleIteratorVariableName" select="''"/>
		<xsl:param name="ConditionalBasicReplacement" select="false()"/>
		<xsl:param name="TopLevel" select="false()"/>
		<xsl:variable name="useVariablePrefixFragment">
			<xsl:choose>
				<xsl:when test="$VariablePrefix">
					<xsl:value-of select="$VariablePrefix"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text>factText</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="alternateRolePlayer" select="@alternateRolePlayer"/>
		<xsl:variable name="explicitSubscript" select="string(@subscript)"/>
		<xsl:variable name="overflowSubscript" select="string(@overflowSubscript)"/>
		<xsl:variable name="snippetChild" select="cvg:Snippet"/>
		<xsl:variable name="conditionalChild" select="cvg:ConditionalReplacement"/>
		<xsl:variable name="replacementFragment">
			<xsl:if test="$conditionalChild or $snippetChild[@ref!='null']">
				<xsl:choose>
					<xsl:when test="$ConditionalBasicReplacement or $alternateRolePlayer or $explicitSubscript or $overflowSubscript">
						<xsl:call-template name="GetBasicReplacement">
							<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
							<xsl:with-param name="SequenceIteratorVariableName" select="$IteratorVariableName"/>
							<xsl:with-param name="FactRoleIteratorVariableName" select="$FactRoleIteratorVariableName"/>
							<xsl:with-param name="AlternateRolePlayer" select="$alternateRolePlayer"/>
							<xsl:with-param name="ExplicitSubscript" select="$explicitSubscript"/>
							<xsl:with-param name="OverflowSubscript" select="$overflowSubscript"/>
						</xsl:call-template>
					</xsl:when>
					<xsl:otherwise>
						<plx:nameRef name="basicReplacement"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:if>
		</xsl:variable>
		<xsl:variable name="predicateVariablePrefix" select="concat(string($useVariablePrefixFragment),$VariableDecorator,$PredicateReplacementVariablePart)"/>
		<xsl:if test="$conditionalChild">
			<plx:local name="{$predicateVariablePrefix}1" dataTypeName=".string"/>
			<xsl:apply-templates select="$conditionalChild" mode="ConstraintVerbalization">
				<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
				<xsl:with-param name="IteratorVariableName" select="$IteratorVariableName"/>
				<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
				<xsl:with-param name="VariablePrefix" select="$predicateVariablePrefix"/>
				<xsl:with-param name="VariableDecorator" select="'1'"/>
				<xsl:with-param name="FirstPassVariable" select="$FirstPassVariable"/>
				<xsl:with-param name="InjectSnippetFormatArgument" select="exsl:node-set($replacementFragment)/child::*"/>
			</xsl:apply-templates>
			<plx:assign>
				<plx:left>
					<plx:nameRef name="roleReplacement"/>
				</plx:left>
				<plx:right>
					<plx:nameRef name="{$predicateVariablePrefix}1"/>
				</plx:right>
			</plx:assign>
		</xsl:if>
		<xsl:for-each select="$snippetChild">
			<xsl:variable name="extraChildren" select="child::*"/>
			<xsl:if test="$extraChildren">
				<xsl:for-each select="$extraChildren">
					<plx:local name="{$predicateVariablePrefix}{position()}" dataTypeName=".string"/>
				</xsl:for-each>
				<xsl:apply-templates select="$extraChildren" mode="ConstraintVerbalization">
					<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
					<xsl:with-param name="IteratorVariableName" select="$IteratorVariableName"/>
					<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
					<xsl:with-param name="VariablePrefix" select="$predicateVariablePrefix"/>
					<xsl:with-param name="VariableDecorator" select="position()"/>
					<xsl:with-param name="FirstPassVariable" select="$FirstPassVariable"/>
				</xsl:apply-templates>
			</xsl:if>
			<plx:assign>
				<plx:left>
					<plx:nameRef name="roleReplacement"/>
				</plx:left>
				<plx:right>
					<xsl:choose>
						<xsl:when test="@ref='null'">
							<!-- Special case so that we can eliminate replacement text fields -->
							<plx:string/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:choose>
								<xsl:when test="@ref='SelfReference'">
									<xsl:copy-of select="$replacementFragment"/>
								</xsl:when>
								<xsl:otherwise>
									<plx:callStatic name="Format" dataTypeName=".string">
										<plx:passParam>
											<plx:callInstance name="FormatProvider" type="property">
												<plx:callObject>
													<plx:nameRef type="parameter" name="writer"/>
												</plx:callObject>
											</plx:callInstance>
										</plx:passParam>
										<plx:passParam>
											<xsl:call-template name="SnippetFor">
												<xsl:with-param name="SnippetType" select="@ref"/>
											</xsl:call-template>
										</plx:passParam>
										<plx:passParam>
											<xsl:copy-of select="$replacementFragment"/>
										</plx:passParam>
										<xsl:for-each select="$extraChildren">
											<plx:passParam>
												<plx:nameRef name="{$predicateVariablePrefix}{position()}"/>
											</plx:passParam>
										</xsl:for-each>
									</plx:callStatic>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:otherwise>
					</xsl:choose>
				</plx:right>
			</plx:assign>
		</xsl:for-each>
		<xsl:if test="$UseOnceVariable">
			<plx:assign>
				<plx:left>
					<plx:nameRef name="{$UseOnceVariable}"/>
				</plx:left>
				<plx:right>
					<plx:falseKeyword/>
				</plx:right>
			</plx:assign>
		</xsl:if>
	</xsl:template>
	<xsl:template name="ConditionalMatchCondition">
		<xsl:param name="PatternGroup"/>
		<xsl:param name="ConditionalMatch" select="string(@conditionalMatch)"/>
		<xsl:variable name="normalizedMatches" select="normalize-space(@conditionalMatch)"/>
		<xsl:if test="$normalizedMatches">
			<xsl:variable name="conditionsFragment">
				<xsl:call-template name="SplitList">
					<xsl:with-param name="ItemList" select="$normalizedMatches"/>
				</xsl:call-template>
			</xsl:variable>
			<xsl:variable name="allConditionsFragment">
				<xsl:for-each select="exsl:node-set($conditionsFragment)/child::*">
					<xsl:call-template name="SingleConditionalMatchCondition">
						<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
						<xsl:with-param name="ConditionalMatch" select="string(.)"/>
					</xsl:call-template>
				</xsl:for-each>
			</xsl:variable>
			<xsl:call-template name="CombineElements">
				<xsl:with-param name="OperatorType" select="'booleanOr'"/>
				<xsl:with-param name="Elements" select="exsl:node-set($allConditionsFragment)/child::*"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
	<xsl:template name="SingleConditionalMatchCondition">
		<xsl:param name="PatternGroup"/>
		<xsl:param name="ConditionalMatch"/>
		<xsl:if test="string-length($ConditionalMatch)">
			<xsl:choose>
				<xsl:when test="$ConditionalMatch='IsPersonal'">
					<plx:callInstance name="IsPersonal" type="property">
						<plx:callObject>
							<plx:callInstance name="RolePlayer" type="property">
								<plx:callObject>
									<plx:nameRef name="currentRole"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:callObject>
					</plx:callInstance>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='IsObjectifiedFactType'">
					<plx:binaryOperator type="identityInequality">
						<plx:left>
							<plx:callThis name="NestedFactType" type="property"/>
						</plx:left>
						<plx:right>
							<plx:nullKeyword/>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='HasVerbalizableSubtypeDerivationPath' or $ConditionalMatch='HasVerbalizableQueryDerivationPath'">
					<plx:binaryOperator type="booleanAnd">
						<plx:left>
							<plx:binaryOperator type="identityInequality">
								<plx:left>
									<plx:inlineStatement dataTypeName="SubtypeDerivationRule">
										<plx:assign>
											<plx:left>
												<plx:nameRef name="derivationRule"/>
											</plx:left>
											<plx:right>
												<xsl:choose>
													<xsl:when test="$ConditionalMatch='HasVerbalizableSubtypeDerivationPath'">
														<plx:callThis name="DerivationRule" type="property"/>
													</xsl:when>
													<xsl:otherwise>
														<plx:cast dataTypeName="QueryDerivationRule">
															<plx:callThis name="DerivationRule" type="property"/>
														</plx:cast>
													</xsl:otherwise>
												</xsl:choose>
											</plx:right>
										</plx:assign>
									</plx:inlineStatement>
								</plx:left>
								<plx:right>
									<plx:nullKeyword/>
								</plx:right>
							</plx:binaryOperator>
						</plx:left>
						<plx:right>
							<plx:callInstance name="HasPathVerbalization">
								<plx:callObject>
									<plx:inlineStatement dataTypeName="RolePathVerbalizer">
										<plx:assign>
											<plx:left>
												<plx:nameRef name="pathVerbalizer"/>
											</plx:left>
											<plx:right>
												<plx:callStatic dataTypeName="RolePathVerbalizer" name="Create">
													<plx:passParam>
														<plx:nameRef name="derivationRule"/>
													</plx:passParam>
													<plx:passParam>
														<plx:callNew dataTypeName="StandardRolePathRenderer">
															<plx:passParam>
																<plx:nameRef name="snippets"/>
															</plx:passParam>
															<plx:passParam>
																<plx:nameRef name="verbalizationContext" type="parameter"/>
															</plx:passParam>
															<plx:passParam>
																<plx:callInstance name="FormatProvider" type="property">
																	<plx:callObject>
																		<plx:nameRef name="writer"/>
																	</plx:callObject>
																</plx:callInstance>
															</plx:passParam>
														</plx:callNew>
													</plx:passParam>
												</plx:callStatic>
											</plx:right>
										</plx:assign>
									</plx:inlineStatement>
								</plx:callObject>
								<plx:passParam>
									<plx:nameRef name="derivationRule"/>
								</plx:passParam>
							</plx:callInstance>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='HasVerbalizableDynamicRulePath'">
					<plx:callInstance name="CanVerbalizeDynamicRule" type="property">
						<plx:callObject>
							<plx:inlineStatement dataTypeName="RolePathVerbalizer">
								<plx:assign>
									<plx:left>
										<plx:nameRef name="pathVerbalizer"/>
									</plx:left>
									<plx:right>
										<plx:callStatic dataTypeName="RolePathVerbalizer" name="Create">
											<plx:passParam>
												<plx:thisKeyword/>
											</plx:passParam>
											<plx:passParam>
												<plx:callNew dataTypeName="StandardRolePathRenderer">
													<plx:passParam>
														<plx:nameRef name="snippets"/>
													</plx:passParam>
													<plx:passParam>
														<plx:nameRef name="verbalizationContext" type="parameter"/>
													</plx:passParam>
													<plx:passParam>
														<plx:callInstance name="FormatProvider" type="property">
															<plx:callObject>
																<plx:nameRef name="writer"/>
															</plx:callObject>
														</plx:callInstance>
													</plx:passParam>
												</plx:callNew>
											</plx:passParam>
										</plx:callStatic>
									</plx:right>
								</plx:assign>
							</plx:inlineStatement>
						</plx:callObject>
					</plx:callInstance>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='HasStoredDerivationRule'">
					<plx:binaryOperator type="booleanAnd">
						<plx:left>
							<plx:binaryOperator type="identityInequality">
								<plx:left>
									<plx:nameRef name="derivationRule"/>
								</plx:left>
								<plx:right>
									<plx:nullKeyword/>
								</plx:right>
							</plx:binaryOperator>
						</plx:left>
						<plx:right>
							<plx:binaryOperator type="equality">
								<plx:left>
									<plx:callInstance name="DerivationStorage" type="property">
										<plx:callObject>
											<plx:nameRef name="derivationRule"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:left>
								<plx:right>
									<plx:callStatic dataTypeName="DerivationStorage" name="Stored" type="field"/>
								</plx:right>
							</plx:binaryOperator>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='DerivationRuleIsPartial'">
					<plx:binaryOperator type="equality">
						<plx:left>
							<plx:callInstance name="DerivationCompleteness" type="property">
								<plx:callObject>
									<plx:nameRef name="derivationRule"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:left>
						<plx:right>
							<plx:callStatic dataTypeName="DerivationCompleteness" name="PartiallyDerived" type="field"/>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='DerivationRuleIsStored'">
					<plx:binaryOperator type="equality">
						<plx:left>
							<plx:callInstance name="DerivationStorage" type="property">
								<plx:callObject>
									<plx:nameRef name="derivationRule"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:left>
						<plx:right>
							<plx:callStatic dataTypeName="DerivationStorage" name="Stored" type="field"/>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='DerivationRuleIsPartialAndStored'">
					<plx:binaryOperator type="booleanAnd">
						<plx:left>
							<plx:binaryOperator type="equality">
								<plx:left>
									<plx:callInstance name="DerivationCompleteness" type="property">
										<plx:callObject>
											<plx:nameRef name="derivationRule"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:left>
								<plx:right>
									<plx:callStatic dataTypeName="DerivationCompleteness" name="PartiallyDerived" type="field"/>
								</plx:right>
							</plx:binaryOperator>
						</plx:left>
						<plx:right>
							<plx:binaryOperator type="equality">
								<plx:left>
									<plx:callInstance name="DerivationStorage" type="property">
										<plx:callObject>
											<plx:nameRef name="derivationRule"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:left>
								<plx:right>
									<plx:callStatic dataTypeName="DerivationStorage" name="Stored" type="field"/>
								</plx:right>
							</plx:binaryOperator>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='DerivedFactType'">
					<plx:binaryOperator type="identityInequality">
						<plx:left>
							<plx:nullKeyword/>
						</plx:left>
						<plx:right>
							<plx:inlineStatement dataTypeName="FactType">
								<plx:assign>
									<plx:left>
										<plx:nameRef name="derivedFactType"/>
									</plx:left>
									<plx:right>
										<plx:cast type="testCast" dataTypeName="FactType">
											<plx:nameRef name="derivedElement"/>
										</plx:cast>
									</plx:right>
								</plx:assign>
							</plx:inlineStatement>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='DerivedSubtype'">
					<plx:binaryOperator type="identityInequality">
						<plx:left>
							<plx:nullKeyword/>
						</plx:left>
						<plx:right>
							<plx:inlineStatement dataTypeName="ObjectType">
								<plx:assign>
									<plx:left>
										<plx:nameRef name="derivedSubtype"/>
									</plx:left>
									<plx:right>
										<plx:cast type="testCast" dataTypeName="ObjectType">
											<plx:nameRef name="derivedElement"/>
										</plx:cast>
									</plx:right>
								</plx:assign>
							</plx:inlineStatement>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='DynamicRule'">
					<plx:binaryOperator type="identityInequality">
						<plx:left>
							<plx:nullKeyword/>
						</plx:left>
						<plx:right>
							<plx:inlineStatement dataTypeName="DynamicRule">
								<plx:assign>
									<plx:left>
										<plx:nameRef name="dynamicRule"/>
									</plx:left>
									<plx:right>
										<plx:cast type="testCast" dataTypeName="DynamicRule">
											<plx:nameRef name="derivedElement"/>
										</plx:cast>
									</plx:right>
								</plx:assign>
							</plx:inlineStatement>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='DynamicRuleHasHeadSection' or $ConditionalMatch='DynamicRuleHasAddSection' or $ConditionalMatch='DynamicRuleHasDeleteSection' or $ConditionalMatch='DynamicRuleHasConditionSection'">
					<plx:callInstance name="{$ConditionalMatch}" type="property">
						<plx:callObject>
							<plx:nameRef name="pathVerbalizer"/>
						</plx:callObject>
					</plx:callInstance>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='DynamicRuleHasStandaloneConditionSection'">
					<plx:binaryOperator type="booleanAnd">
						<plx:left>
							<plx:callInstance name="DynamicRuleHasConditionSection" type="property">
								<plx:callObject>
									<plx:nameRef name="pathVerbalizer"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:left>
						<plx:right>
							<plx:unaryOperator type="booleanNot">
								<plx:callInstance name="DynamicRuleHasHeadSection" type="property">
									<plx:callObject>
										<plx:nameRef name="pathVerbalizer"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:unaryOperator>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='DynamicStateAdded' or $ConditionalMatch='DynamicStateDeleted' or $ConditionalMatch='DynamicStateInitial'">
					<plx:binaryOperator type="equality">
						<plx:left>
							<plx:nameRef name="dynamicState"/>
						</plx:left>
						<plx:right>
							<plx:callStatic dataTypeName="DynamicRuleNodeState" type="field" name="Added">
								<xsl:choose>
									<xsl:when test="$ConditionalMatch='DynamicStateDeleted'">
										<xsl:attribute name="name">
											<xsl:text>Deleted</xsl:text>
										</xsl:attribute>
									</xsl:when>
									<xsl:when test="$ConditionalMatch='DynamicStateInitial'">
										<xsl:attribute name="name">
											<xsl:text>Initial</xsl:text>
										</xsl:attribute>
									</xsl:when>
								</xsl:choose>
							</plx:callStatic>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='PathNodeFactTypeEntry'">
					<plx:binaryOperator type="booleanAnd">
						<plx:left>
							<plx:binaryOperator type="identityInequality">
								<plx:left>
									<plx:inlineStatement dataTypeName="PathedRole">
										<plx:assign>
											<plx:left>
												<plx:nameRef name="pathedRole"/>
											</plx:left>
											<plx:right>
												<plx:nameRef name="pathNode"/>
											</plx:right>
										</plx:assign>
									</plx:inlineStatement>
								</plx:left>
								<plx:right>
									<plx:nullKeyword/>
								</plx:right>
							</plx:binaryOperator>
						</plx:left>
						<plx:right>
							<plx:binaryOperator type="inequality">
								<plx:left>
									<plx:callInstance name="PathedRolePurpose" type="property">
										<plx:callObject>
											<plx:nameRef name="pathedRole"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:left>
								<plx:right>
									<plx:callStatic dataTypeName="PathedRolePurpose" name="SameFactType" type="field"/>
								</plx:right>
							</plx:binaryOperator>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='HasPreferredIdentifier'">
					<plx:binaryOperator type="identityInequality">
						<plx:left>
							<plx:nameRef name="preferredIdentifier"/>
						</plx:left>
						<plx:right>
							<plx:nullKeyword/>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='HasUnobjectifiedPreferredIdentifier'">
					<plx:binaryOperator type="booleanAnd">
						<plx:left>
							<plx:binaryOperator type="identityInequality">
								<plx:left>
									<plx:nameRef name="preferredIdentifier"/>
								</plx:left>
								<plx:right>
									<plx:nullKeyword/>
								</plx:right>
							</plx:binaryOperator>
						</plx:left>
						<plx:right>
							<plx:unaryOperator type="booleanNot">
								<plx:binaryOperator type="booleanAnd">
									<plx:left>
										<plx:callInstance name="IsInternal" type="property">
											<plx:callObject>
												<plx:nameRef name="preferredIdentifier"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:binaryOperator type="identityInequality">
											<plx:left>
												<plx:callInstance name="NestingType" type="property">
													<plx:callObject>
														<plx:callInstance name=".implied" type="indexerCall">
															<plx:callObject>
																<plx:callInstance name="FactTypeCollection" type="property">
																	<plx:callObject>
																		<plx:nameRef name="preferredIdentifier"/>
																	</plx:callObject>
																</plx:callInstance>
															</plx:callObject>
															<plx:passParam>
																<plx:value data="0" type="i4"/>
															</plx:passParam>
														</plx:callInstance>
													</plx:callObject>
												</plx:callInstance>
											</plx:left>
											<plx:right>
												<plx:nullKeyword/>
											</plx:right>
										</plx:binaryOperator>
									</plx:right>
								</plx:binaryOperator>
							</plx:unaryOperator>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='HasReferenceMode'">
					<plx:binaryOperator type="booleanAnd">
						<plx:left>
							<plx:binaryOperator type="identityInequality">
								<plx:left>
									<plx:nameRef name="identifyingEntityType"/>
								</plx:left>
								<plx:right>
									<plx:nullKeyword/>
								</plx:right>
							</plx:binaryOperator>
						</plx:left>
						<plx:right>
							<plx:binaryOperator type="identityInequality">
								<plx:left>
									<plx:nameRef name="identifyingValueType"/>
								</plx:left>
								<plx:right>
									<plx:nullKeyword/>
								</plx:right>
							</plx:binaryOperator>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='HasRoleName'">
					<plx:unaryOperator type="booleanNot">
						<plx:callStatic dataTypeName=".string" name="IsNullOrEmpty">
							<plx:passParam>
								<plx:nameRef name="roleName"/>
							</plx:passParam>
						</plx:callStatic>
					</plx:unaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='HasRolePlayer'">
					<plx:binaryOperator type="identityInequality">
						<plx:left>
							<plx:nameRef name="rolePlayer"/>
						</plx:left>
						<plx:right>
							<plx:nullKeyword/>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='HasSubscript'">
					<plx:binaryOperator type="inequality">
						<plx:left>
							<plx:nameRef name="subscript"/>
						</plx:left>
						<plx:right>
							<plx:value data="0" type="i4"/>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='IsParameterized'">
					<plx:binaryOperator type="inequality">
						<plx:left>
							<plx:nameRef name="queryParameterCount"/>
						</plx:left>
						<plx:right>
							<plx:value type="i4" data="0"/>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='IsNamedParameter'">
					<plx:unaryOperator type="booleanNot">
						<plx:callStatic name="IsNullOrEmpty" dataTypeName=".string">
							<plx:passParam>
								<plx:nameRef name="queryParameterName"/>
							</plx:passParam>
						</plx:callStatic>
					</plx:unaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='HasNotes'">
					<plx:unaryOperator type="booleanNot">
						<plx:callStatic name="IsNullOrEmpty" dataTypeName=".string">
							<plx:passParam>
								<plx:callThis type="property" name="NoteText"/>
							</plx:passParam>
						</plx:callStatic>
					</plx:unaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='HasPortableDataType'">
					<plx:binaryOperator type="booleanAnd">
						<plx:left>
							<plx:binaryOperator type="identityInequality">
								<plx:left>
									<plx:nameRef name="identifyingValueType"/>
								</plx:left>
								<plx:right>
									<plx:nullKeyword/>
								</plx:right>
							</plx:binaryOperator>
						</plx:left>
						<plx:right>
							<plx:binaryOperator type="identityEquality">
								<plx:left>
									<plx:callThis name="DataTypeNotSpecifiedError" type="property"/>
								</plx:left>
								<plx:right>
									<plx:nullKeyword/>
								</plx:right>
							</plx:binaryOperator>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='HasPortableDataTypeLengthAndScale'">
					<plx:binaryOperator type="booleanAnd">
						<plx:left>
							<plx:binaryOperator type="identityInequality">
								<plx:left>
									<plx:callInstance type="property" name="ScaleName">
										<plx:callObject>
											<plx:nameRef name="identifyingDataType"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:left>
								<plx:right>
									<plx:nullKeyword/>
								</plx:right>
							</plx:binaryOperator>
						</plx:left>
						<plx:right>
							<plx:binaryOperator type="inequality">
								<plx:left>
									<plx:callInstance type="property" name="Scale">
										<plx:callObject>
											<plx:nameRef name="dataTypeUse"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:left>
								<plx:right>
									<plx:value data="0" type="i4"/>
								</plx:right>
							</plx:binaryOperator>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='HasPortableDataTypeLength'">
					<plx:binaryOperator type="booleanAnd">
						<plx:left>
							<plx:binaryOperator type="identityInequality">
								<plx:left>
									<plx:callInstance type="property" name="LengthName">
										<plx:callObject>
											<plx:nameRef name="identifyingDataType"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:left>
								<plx:right>
									<plx:nullKeyword/>
								</plx:right>
							</plx:binaryOperator>
						</plx:left>
						<plx:right>
							<plx:binaryOperator type="booleanOr">
								<plx:left>
									<plx:binaryOperator type="identityEquality">
										<plx:left>
											<plx:callInstance type="property" name="ScaleName">
												<plx:callObject>
													<plx:nameRef name="identifyingDataType"/>
												</plx:callObject>
											</plx:callInstance>
										</plx:left>
										<plx:right>
											<plx:nullKeyword/>
										</plx:right>
									</plx:binaryOperator>
								</plx:left>
								<plx:right>
									<plx:binaryOperator type="inequality">
										<plx:left>
											<plx:callInstance type="property" name="Length">
												<plx:callObject>
													<plx:nameRef name="dataTypeUse"/>
												</plx:callObject>
											</plx:callInstance>
										</plx:left>
										<plx:right>
											<plx:value data="0" type="i4"/>
										</plx:right>
									</plx:binaryOperator>
								</plx:right>
							</plx:binaryOperator>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='IsBinaryLeadReading'">
					<plx:binaryOperator type="equality">
						<plx:left>
							<plx:nameRef name="binaryLeadReadingColumnIndex"/>
						</plx:left>
						<plx:right>
							<plx:value data="0" type="i4"/>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='IsBinaryLeadReadingReverse'">
					<plx:binaryOperator type="equality">
						<plx:left>
							<plx:nameRef name="binaryLeadReadingColumnIndex"/>
						</plx:left>
						<plx:right>
							<plx:value data="1" type="i4"/>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='ExclusionIsExclusiveOrConstraint'">
					<plx:binaryOperator type="identityInequality">
						<plx:left>
							<plx:callThis name="ExclusiveOrMandatoryConstraint" type="property"/>
						</plx:left>
						<plx:right>
							<plx:nullKeyword/>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='IsNegative'">
					<plx:nameRef name="isNegative"/>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='IsPositive'">
					<plx:unaryOperator type="booleanNot">
						<plx:nameRef name="isNegative"/>
					</plx:unaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='IsIndependent'">
					<plx:callThis name="IsIndependent" type="property"/>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='HasDefaultValue'">
					<plx:binaryOperator type="identityInequality">
						<plx:left>
							<plx:nameRef name="defaultValue" type="local"/>
						</plx:left>
						<plx:right>
							<plx:nullKeyword/>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='IsEntityType'">
					<plx:binaryOperator type="identityEquality">
						<plx:left>
							<plx:nameRef name="dataType"/>
						</plx:left>
						<plx:right>
							<plx:nullKeyword/>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='IsPreferredIdentifier'">
					<plx:callThis type="property" name="IsPreferred"/>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='IsText'">
					<plx:nameRef name="isText" type="local"/>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='IsSingleValue'">
					<plx:nameRef name="isSingleValue" type="local"/>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='JoinedSetComparisonSequence'">
					<!-- This can be further refined by Joined[Equality|Exclusion|Subset]Constraint tests -->
					<plx:binaryOperator type="identityInequality">
						<plx:left>
							<plx:nullKeyword/>
						</plx:left>
						<plx:right>
							<plx:inlineStatement dataTypeName="SetComparisonConstraintRoleSequence">
								<plx:assign>
									<plx:left>
										<plx:nameRef name="joinedSetComparisonSequence"/>
									</plx:left>
									<plx:right>
										<plx:cast type="testCast" dataTypeName="SetComparisonConstraintRoleSequence">
											<plx:nameRef name="derivedElement"/>
										</plx:cast>
									</plx:right>
								</plx:assign>
							</plx:inlineStatement>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='JoinedEqualityConstraint'">
					<plx:binaryOperator type="typeEquality">
						<plx:left>
							<plx:nameRef name="joinedSetComparisonConstraint"/>
						</plx:left>
						<plx:right>
							<plx:directTypeReference dataTypeName="EqualityConstraint"/>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='JoinedExclusionConstraint'">
					<plx:binaryOperator type="typeEquality">
						<plx:left>
							<plx:nameRef name="joinedSetComparisonConstraint"/>
						</plx:left>
						<plx:right>
							<plx:directTypeReference dataTypeName="ExclusionConstraint"/>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='JoinedSubsetConstraint'">
					<plx:binaryOperator type="typeEquality">
						<plx:left>
							<plx:nameRef name="joinedSetComparisonConstraint"/>
						</plx:left>
						<plx:right>
							<plx:directTypeReference dataTypeName="SubsetConstraint"/>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='JoinedSetConstraint'">
					<!-- This can be further refined by Joined[Frequency|Ring|Uniqueness|ValueComparison]Constraint tests -->
					<plx:binaryOperator type="identityInequality">
						<plx:left>
							<plx:nullKeyword/>
						</plx:left>
						<plx:right>
							<plx:inlineStatement dataTypeName="SetConstraint">
								<plx:assign>
									<plx:left>
										<plx:nameRef name="joinedSetConstraint"/>
									</plx:left>
									<plx:right>
										<plx:cast type="testCast" dataTypeName="SetConstraint">
											<plx:nameRef name="derivedElement"/>
										</plx:cast>
									</plx:right>
								</plx:assign>
							</plx:inlineStatement>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='JoinedFrequencyConstraint'">
					<plx:binaryOperator type="typeEquality">
						<plx:left>
							<plx:nameRef name="joinedSetConstraint"/>
						</plx:left>
						<plx:right>
							<plx:directTypeReference dataTypeName="FrequencyConstraint"/>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='JoinedRingConstraint'">
					<plx:binaryOperator type="typeEquality">
						<plx:left>
							<plx:nameRef name="joinedSetConstraint"/>
						</plx:left>
						<plx:right>
							<plx:directTypeReference dataTypeName="RingConstraint"/>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='JoinedUniquenessConstraint'">
					<plx:binaryOperator type="typeEquality">
						<plx:left>
							<plx:nameRef name="joinedSetConstraint"/>
						</plx:left>
						<plx:right>
							<plx:directTypeReference dataTypeName="UniquenessConstraint"/>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='JoinedValueComparisonConstraint'">
					<plx:binaryOperator type="typeEquality">
						<plx:left>
							<plx:nameRef name="joinedSetConstraint"/>
						</plx:left>
						<plx:right>
							<plx:directTypeReference dataTypeName="ValueComparisonConstraint"/>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='BinaryWithRoleName'">
					<plx:binaryOperator type="booleanAnd">
						<plx:left>
							<plx:binaryOperator type="equality">
								<plx:left>
									<plx:nameRef name="factArity" type="local"/>
								</plx:left>
								<plx:right>
									<plx:value data="2" type="i4"/>
								</plx:right>
							</plx:binaryOperator>
						</plx:left>
						<plx:right>
							<plx:binaryOperator type="inequality">
								<plx:left>
									<plx:callInstance name="Length" type="property">
										<plx:callObject>
											<plx:callInstance name="Name" type="property">
												<plx:callObject>
													<plx:nameRef name="constrainedRole" type="local"/>
												</plx:callObject>
											</plx:callInstance>
										</plx:callObject>
									</plx:callInstance>
								</plx:left>
								<plx:right>
									<plx:value data="0" type="i4"/>
								</plx:right>
							</plx:binaryOperator>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<!-- UNDONE: Equality should be verified with the DataType's compare method, if available -->
				<xsl:when test="$ConditionalMatch='MinEqualsMax'">
					<plx:binaryOperator type="equality">
						<plx:left>
							<plx:nameRef name="minValue" type="local"/>
						</plx:left>
						<plx:right>
							<plx:nameRef name="maxValue" type="local"/>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='MinClosedMaxClosed'">
					<plx:binaryOperator type="booleanAnd">
						<plx:left>
							<plx:binaryOperator type="inequality">
								<plx:left>
									<plx:nameRef name="minInclusion" type="local"/>
								</plx:left>
								<plx:right>
									<plx:callStatic name="Open" dataTypeName="RangeInclusion" type="field"/>
								</plx:right>
							</plx:binaryOperator>
						</plx:left>
						<plx:right>
							<plx:binaryOperator type="inequality">
								<plx:left>
									<plx:nameRef name="maxInclusion" type="local"/>
								</plx:left>
								<plx:right>
									<plx:callStatic name="Open" dataTypeName="RangeInclusion" type="field"/>
								</plx:right>
							</plx:binaryOperator>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='MinClosedMaxOpen'">
					<plx:binaryOperator type="booleanAnd">
						<plx:left>
							<plx:binaryOperator type="inequality">
								<plx:left>
									<plx:nameRef name="minInclusion" type="local"/>
								</plx:left>
								<plx:right>
									<plx:callStatic name="Open" dataTypeName="RangeInclusion" type="field"/>
								</plx:right>
							</plx:binaryOperator>
						</plx:left>
						<plx:right>
							<plx:binaryOperator type="equality">
								<plx:left>
									<plx:nameRef name="maxInclusion" type="local"/>
								</plx:left>
								<plx:right>
									<plx:callStatic name="Open" dataTypeName="RangeInclusion" type="field"/>
								</plx:right>
							</plx:binaryOperator>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='MinClosedMaxUnbounded'">
					<plx:binaryOperator type="booleanAnd">
						<plx:left>
							<plx:binaryOperator type="inequality">
								<plx:left>
									<plx:nameRef name="minInclusion" type="local"/>
								</plx:left>
								<plx:right>
									<plx:callStatic name="Open" dataTypeName="RangeInclusion" type="field"/>
								</plx:right>
							</plx:binaryOperator>
						</plx:left>
						<plx:right>
							<plx:binaryOperator type="equality">
								<plx:left>
									<plx:callInstance name="Length" type="property">
										<plx:callObject>
											<plx:nameRef name="maxValue" type="local"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:left>
								<plx:right>
									<plx:value data="0" type="i4"/>
								</plx:right>
							</plx:binaryOperator>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='MinOpenMaxClosed'">
					<plx:binaryOperator type="booleanAnd">
						<plx:left>
							<plx:binaryOperator type="equality">
								<plx:left>
									<plx:nameRef name="minInclusion" type="local"/>
								</plx:left>
								<plx:right>
									<plx:callStatic name="Open" dataTypeName="RangeInclusion" type="field"/>
								</plx:right>
							</plx:binaryOperator>
						</plx:left>
						<plx:right>
							<plx:binaryOperator type="inequality">
								<plx:left>
									<plx:nameRef name="maxInclusion" type="local"/>
								</plx:left>
								<plx:right>
									<plx:callStatic name="Open" dataTypeName="RangeInclusion" type="field"/>
								</plx:right>
							</plx:binaryOperator>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='MinOpenMaxOpen'">
					<plx:binaryOperator type="booleanAnd">
						<plx:left>
							<plx:binaryOperator type="equality">
								<plx:left>
									<plx:nameRef name="minInclusion" type="local"/>
								</plx:left>
								<plx:right>
									<plx:callStatic name="Open" dataTypeName="RangeInclusion" type="field"/>
								</plx:right>
							</plx:binaryOperator>
						</plx:left>
						<plx:right>
							<plx:binaryOperator type="equality">
								<plx:left>
									<plx:nameRef name="maxInclusion" type="local"/>
								</plx:left>
								<plx:right>
									<plx:callStatic name="Open" dataTypeName="RangeInclusion" type="field"/>
								</plx:right>
							</plx:binaryOperator>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='MinOpenMaxUnbounded'">
					<plx:binaryOperator type="booleanAnd">
						<plx:left>
							<plx:binaryOperator type="equality">
								<plx:left>
									<plx:nameRef name="minInclusion" type="local"/>
								</plx:left>
								<plx:right>
									<plx:callStatic name="Open" dataTypeName="RangeInclusion" type="field"/>
								</plx:right>
							</plx:binaryOperator>
						</plx:left>
						<plx:right>
							<plx:binaryOperator type="equality">
								<plx:left>
									<plx:callInstance name="Length" type="property">
										<plx:callObject>
											<plx:nameRef name="maxValue" type="local"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:left>
								<plx:right>
									<plx:value data="0" type="i4"/>
								</plx:right>
							</plx:binaryOperator>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='MinUnboundedMaxClosed'">
					<plx:binaryOperator type="booleanAnd">
						<plx:left>
							<plx:binaryOperator type="equality">
								<plx:left>
									<plx:callInstance name="Length" type="property">
										<plx:callObject>
											<plx:nameRef name="minValue" type="local"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:left>
								<plx:right>
									<plx:value data="0" type="i4"/>
								</plx:right>
							</plx:binaryOperator>
						</plx:left>
						<plx:right>
							<plx:binaryOperator type="inequality">
								<plx:left>
									<plx:nameRef name="maxInclusion" type="local"/>
								</plx:left>
								<plx:right>
									<plx:callStatic name="Open" dataTypeName="RangeInclusion" type="field"/>
								</plx:right>
							</plx:binaryOperator>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='MinUnboundedMaxOpen'">
					<plx:binaryOperator type="booleanAnd">
						<plx:left>
							<plx:binaryOperator type="equality">
								<plx:left>
									<plx:callInstance name="Length" type="property">
										<plx:callObject>
											<plx:nameRef name="minValue" type="local"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:left>
								<plx:right>
									<plx:value data="0" type="i4"/>
								</plx:right>
							</plx:binaryOperator>
						</plx:left>
						<plx:right>
							<plx:binaryOperator type="equality">
								<plx:left>
									<plx:nameRef name="maxInclusion" type="local"/>
								</plx:left>
								<plx:right>
									<plx:callStatic name="Open" dataTypeName="RangeInclusion" type="field"/>
								</plx:right>
							</plx:binaryOperator>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='FrequencyRangeExactlyOne'">
					<plx:binaryOperator type="booleanAnd">
						<plx:left>
							<plx:binaryOperator type="equality">
								<plx:left>
									<plx:callThis name="MaxFrequency" type="property"/>
								</plx:left>
								<plx:right>
									<plx:value type="i4" data="1"/>
								</plx:right>
							</plx:binaryOperator>
						</plx:left>
						<plx:right>
							<plx:binaryOperator type="equality">
								<plx:left>
									<plx:callThis name="MinFrequency" type="property"/>
								</plx:left>
								<plx:right>
									<plx:value type="i4" data="1"/>
								</plx:right>
							</plx:binaryOperator>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='FrequencyRangeExact'">
					<plx:binaryOperator type="equality">
						<plx:left>
							<plx:callThis name="MinFrequency" type="property"/>
						</plx:left>
						<plx:right>
							<plx:callThis name="MaxFrequency" type="property"/>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='FrequencyRangeMaxUnbounded'">
					<plx:binaryOperator type="equality">
						<plx:left>
							<plx:callThis name="MaxFrequency" type="property"/>
						</plx:left>
						<plx:right>
							<plx:value data="0" type="i4"/>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='FrequencyRangeMinUnbounded'">
					<plx:binaryOperator type="equality">
						<plx:left>
							<plx:callThis name="MinFrequency" type="property"/>
						</plx:left>
						<plx:right>
							<plx:value data="1" type="i4"/>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='SingleRange'">
					<plx:binaryOperator type="equality">
						<plx:left>
							<plx:nameRef name="rangeCount"/>
						</plx:left>
						<plx:right>
							<plx:value data="1" type="i4"/>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='RangeExactlyOne'">
					<plx:binaryOperator type="booleanAnd">
						<plx:left>
							<plx:binaryOperator type="equality">
								<plx:left>
									<plx:nameRef name="rangeLowerBound"/>
								</plx:left>
								<plx:right>
									<plx:value data="1" type="i4"/>
								</plx:right>
							</plx:binaryOperator>
						</plx:left>
						<plx:right>
							<plx:binaryOperator type="equality">
								<plx:left>
									<plx:nameRef name="rangeUpperBound"/>
								</plx:left>
								<plx:right>
									<plx:value data="1" type="i4"/>
								</plx:right>
							</plx:binaryOperator>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='RangeExact'">
					<plx:binaryOperator type="equality">
						<plx:left>
							<plx:nameRef name="rangeLowerBound"/>
						</plx:left>
						<plx:right>
							<plx:nameRef name="rangeUpperBound"/>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='RangeUpperUnbounded'">
					<plx:binaryOperator type="equality">
						<plx:left>
							<plx:nameRef name="rangeUpperBound"/>
						</plx:left>
						<plx:right>
							<plx:value data="-1" type="i4"/>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='RangeLowerUnbounded'">
					<plx:binaryOperator type="equality">
						<plx:left>
							<plx:nameRef name="rangeLowerBound"/>
						</plx:left>
						<plx:right>
							<plx:value data="0" type="i4"/>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='SingleRoleRoleSequence'">
					<plx:binaryOperator type="equality">
						<plx:left>
							<xsl:choose>
								<xsl:when test="$PatternGroup='InternalConstraint'">
									<plx:nameRef name="includedArity"/>
								</xsl:when>
								<xsl:otherwise>
									<plx:nameRef name="constraintRoleArity"/>
								</xsl:otherwise>
							</xsl:choose>
						</plx:left>
						<plx:right>
							<plx:value type="i4" data="1"/>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='RolePlayerHasReferenceScheme'">
					<!-- UNDONE: Not checking for null on currentRole.RolePlayer, is there a variable already set? -->
					<plx:binaryOperator type="booleanAnd">
						<plx:left>
							<plx:binaryOperator type="inequality">
								<plx:left>
									<plx:callInstance name="RolePlayer" type="property">
										<plx:callObject>
											<plx:callInstance name="Role" type="property">
												<plx:callObject>
													<plx:nameRef name="primaryRole"/>
												</plx:callObject>
											</plx:callInstance>
										</plx:callObject>
									</plx:callInstance>
								</plx:left>
								<plx:right>
									<plx:nullKeyword/>
								</plx:right>
							</plx:binaryOperator>
						</plx:left>
						<plx:right>
							<plx:binaryOperator type="inequality">
								<plx:left>
									<plx:value data="0" type="i4"/>
								</plx:left>
								<plx:right>
									<plx:callInstance name="Length" type="property">
										<plx:callObject>
											<plx:callInstance name="ReferenceModeString" type="property">
												<plx:callObject>
													<plx:callInstance name="RolePlayer" type="property">
														<plx:callObject>
															<plx:callInstance name="Role" type="property">
																<plx:callObject>
																	<plx:nameRef name="primaryRole"/>
																</plx:callObject>
															</plx:callInstance>
														</plx:callObject>
													</plx:callInstance>
												</plx:callObject>
											</plx:callInstance>
										</plx:callObject>
									</plx:callInstance>
								</plx:right>
							</plx:binaryOperator>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='ObjectifyingInstance'">
					<plx:binaryOperator type="identityInequality">
						<plx:left>
							<plx:nameRef name="objectifyingInstance"/>
						</plx:left>
						<plx:right>
							<plx:nullKeyword/>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='VerbalizeFactTypesWithBrowserObjectType'">
					<plx:binaryOperator type="booleanAnd">
						<plx:left>
							<plx:cast type="exceptionCast" dataTypeName=".boolean">
								<plx:callInstance name=".implied" type="indexerCall">
									<plx:callObject>
										<plx:callInstance name="VerbalizationOptions" type="property">
											<plx:callObject>
												<plx:nameRef name="verbalizationContext" type="parameter"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:callObject>
									<plx:passParam>
										<plx:callStatic dataTypeName="CoreVerbalizationOption" name="FactTypesWithObjectType" type="property"/>
									</plx:passParam>
								</plx:callInstance>
							</plx:cast>
						</plx:left>
						<plx:right>
							<plx:binaryOperator type="equality">
								<plx:left>
									<plx:callInstance name="VerbalizationTarget" type="property">
										<plx:callObject>
											<plx:nameRef name="verbalizationContext" type="parameter"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:left>
								<plx:right>
									<plx:callStatic name="VerbalizationTargetName" dataTypeName="ORMCoreDomainModel" type="property"/>
								</plx:right>
							</plx:binaryOperator>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='IsSubtypeActive'">
					<plx:callInstance name="IsSubtype" type="property">
						<plx:callObject>
							<plx:callThis name="Subtype" type="property"/>
						</plx:callObject>
					</plx:callInstance>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='HasPreProjectionKeys'">
					<plx:binaryOperator type="inequality">
						<plx:left>
							<plx:value type="i4" data="0"/>
						</plx:left>
						<plx:right>
							<plx:nameRef name="preProjectionKeyCount"/>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='NoPreProjectionKeys'">
					<plx:binaryOperator type="equality">
						<plx:left>
							<plx:nameRef name="preProjectionKeyCount"/>
						</plx:left>
						<plx:right>
							<plx:value type="i4" data="0"/>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='HasSinglePreProjectionKey'">
					<plx:binaryOperator type="equality">
						<plx:left>
							<plx:value type="i4" data="1"/>
						</plx:left>
						<plx:right>
							<plx:nameRef name="preProjectionKeyCount"/>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='HasSingleLeadPreProjectionKey'">
					<plx:binaryOperator type="booleanAnd">
						<plx:left>
							<plx:binaryOperator type="equality">
								<plx:left>
									<plx:value type="i4" data="1"/>
								</plx:left>
								<plx:right>
									<plx:nameRef name="preProjectionKeyCount"/>
								</plx:right>
							</plx:binaryOperator>
						</plx:left>
						<plx:right>
							<plx:callInstance name="KeyedVariableLeadsVerbalization">
								<plx:callObject>
									<plx:nameRef name="pathVerbalizer"/>
								</plx:callObject>
								<plx:passParam>
									<plx:nameRef name="joinPath"/>
								</plx:passParam>
								<plx:passParam>
									<plx:callInstance type="indexerCall" name=".implied">
										<plx:callObject>
											<plx:nameRef name="preProjectionKeys"/>
										</plx:callObject>
										<plx:passParam>
											<plx:value data="0" type="i4"/>
										</plx:passParam>
									</plx:callInstance>
								</plx:passParam>
							</plx:callInstance>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='BinaryRingFactType'">
					<plx:binaryOperator type="identityEquality">
						<plx:left>
							<plx:callInstance name="RolePlayer" type="property">
								<plx:callObject>
									<plx:callInstance name="Role" type="property">
										<plx:callObject>
											<plx:callInstance name=".implied" type="arrayIndexer">
												<plx:callObject>
													<plx:nameRef name="factRoles"/>
												</plx:callObject>
												<plx:passParam>
													<plx:value type="i4" data="0"/>
												</plx:passParam>
											</plx:callInstance>
										</plx:callObject>
									</plx:callInstance>
								</plx:callObject>
							</plx:callInstance>
						</plx:left>
						<plx:right>
							<plx:callInstance name="RolePlayer" type="property">
								<plx:callObject>
									<plx:callInstance name="Role" type="property">
										<plx:callObject>
											<plx:callInstance name=".implied" type="arrayIndexer">
												<plx:callObject>
													<plx:nameRef name="factRoles"/>
												</plx:callObject>
												<plx:passParam>
													<plx:value type="i4" data="1"/>
												</plx:passParam>
											</plx:callInstance>
										</plx:callObject>
									</plx:callInstance>
								</plx:callObject>
							</plx:callInstance>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="starts-with($ConditionalMatch,'ValueComparisonOperator')">
					<plx:binaryOperator type="equality">
						<plx:left>
							<plx:callThis type="property" name="Operator"/>
						</plx:left>
						<plx:right>
							<plx:callStatic dataTypeName="ValueComparisonOperator" name="{substring-after($ConditionalMatch,'ValueComparisonOperator')}" type="field"/>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:otherwise>
					<xsl:message terminate="yes">
						<xsl:text>Unrecognized conditional snippet pattern '</xsl:text>
						<xsl:value-of select="$ConditionalMatch"/>
						<xsl:text>'.</xsl:text>
					</xsl:message>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>
	<!-- Provides the chance to write inline code for conditional
		 snippet conditions. This can be applied either on a snippet with
		 a conditionalMatch attribute, or a snippet containing a conditionalMatch -->
	<xsl:template name="ConditionalBlockContext">
		<xsl:variable name="blockContext" select="string(@conditionalBlockContext)"/>
		<xsl:if test="$blockContext">
			<xsl:choose>
				<xsl:when test="$blockContext='ObjectifiedFactType'">
					<xsl:call-template name="DeclareVariablesForFact">
						<xsl:with-param name="NestedFact" select="true()"/>
					</xsl:call-template>
					<xsl:call-template name="PopulateBasicRoleReplacements"/>
				</xsl:when>
				<xsl:when test="$blockContext='BinaryLeadReading'">
					<plx:comment>Check for an optimized verbalization form if we have simple infix forms</plx:comment>
					<plx:comment>with no hyphen binding on both fact types and the role players have the same type.</plx:comment>
					<plx:local name="binaryLeadReadingColumnIndex" dataTypeName=".i4">
						<plx:initialize>
							<plx:value data="-1" type="i4"/>
						</plx:initialize>
					</plx:local>
					<plx:loop>
						<plx:initializeLoop>
							<plx:local name="testColumnIndex" dataTypeName=".i4">
								<plx:initialize>
									<plx:value data="0" type="i4"/>
								</plx:initialize>
							</plx:local>
						</plx:initializeLoop>
						<plx:condition>
							<plx:binaryOperator type="lessThan">
								<plx:left>
									<plx:nameRef name="testColumnIndex"/>
								</plx:left>
								<plx:right>
									<plx:value data="2" type="i4"/>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:beforeLoop>
							<plx:increment>
								<plx:nameRef name="testColumnIndex"/>
							</plx:increment>
						</plx:beforeLoop>
						<plx:local name="firstLeadRole" dataTypeName="Role">
							<plx:initialize>
								<plx:callInstance name="Role" type="property">
									<plx:callObject>
										<plx:callInstance name=".implied" type="indexerCall">
											<plx:callObject>
												<plx:callInstance name=".implied" type="arrayIndexer">
													<plx:callObject>
														<plx:nameRef name="allConstraintRoleSequences"/>
													</plx:callObject>
													<plx:passParam>
														<plx:value data="0" type="i4"/>
													</plx:passParam>
												</plx:callInstance>
											</plx:callObject>
											<plx:passParam>
												<plx:nameRef name="testColumnIndex"/>
											</plx:passParam>
										</plx:callInstance>
									</plx:callObject>
								</plx:callInstance>
							</plx:initialize>
						</plx:local>
						<plx:assign>
							<plx:left>
								<plx:nameRef name="parentFact"/>
							</plx:left>
							<plx:right>
								<plx:callInstance name="FactType" type="property">
									<plx:callObject>
										<plx:nameRef name="firstLeadRole"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:right>
						</plx:assign>
						<plx:branch>
							<plx:condition>
								<plx:binaryOperator type="identityInequality">
									<plx:left>
										<plx:callInstance name="GetMatchingReading">
											<plx:callObject>
												<plx:nameRef name="parentFact"/>
											</plx:callObject>
											<plx:passParam>
												<plx:callInstance name="ReadingOrderCollection" type="property">
													<plx:callObject>
														<plx:nameRef name="parentFact"/>
													</plx:callObject>
												</plx:callInstance>
											</plx:passParam>
											<plx:passParam>
												<plx:nullKeyword/>
											</plx:passParam>
											<plx:passParam>
												<plx:nameRef name="firstLeadRole"/>
											</plx:passParam>
											<plx:passParam>
												<plx:nullKeyword/>
											</plx:passParam>
											<plx:passParam>
												<plx:nullKeyword/>
											</plx:passParam>
											<plx:passParam>
												<plx:binaryOperator type="bitwiseOr">
													<plx:left>
														<plx:binaryOperator type="bitwiseOr">
															<plx:left>
																<plx:callStatic dataTypeName="MatchingReadingOptions" name="NoFrontText" type="field"/>
															</plx:left>
															<plx:right>
																<plx:callStatic dataTypeName="MatchingReadingOptions" name="NoTrailingText" type="field"/>
															</plx:right>
														</plx:binaryOperator>
													</plx:left>
													<plx:right>
														<plx:callStatic dataTypeName="MatchingReadingOptions" name="NotHyphenBound" type="field"/>
													</plx:right>
												</plx:binaryOperator>
											</plx:passParam>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:nullKeyword/>
									</plx:right>
								</plx:binaryOperator>
							</plx:condition>
							<plx:local name="secondLeadRole" dataTypeName="Role">
								<plx:initialize>
									<plx:callInstance name="Role" type="property">
										<plx:callObject>
											<plx:callInstance name=".implied" type="indexerCall">
												<plx:callObject>
													<plx:callInstance name=".implied" type="arrayIndexer">
														<plx:callObject>
															<plx:nameRef name="allConstraintRoleSequences"/>
														</plx:callObject>
														<plx:passParam>
															<plx:value data="1" type="i4"/>
														</plx:passParam>
													</plx:callInstance>
												</plx:callObject>
												<plx:passParam>
													<plx:nameRef name="testColumnIndex"/>
												</plx:passParam>
											</plx:callInstance>
										</plx:callObject>
									</plx:callInstance>
								</plx:initialize>
							</plx:local>
							<plx:branch>
								<plx:condition>
									<plx:binaryOperator type="identityInequality">
										<plx:left>
											<plx:callInstance name="RolePlayer" type="property">
												<plx:callObject>
													<plx:nameRef name="firstLeadRole"/>
												</plx:callObject>
											</plx:callInstance>
										</plx:left>
										<plx:right>
											<plx:callInstance name="RolePlayer" type="property">
												<plx:callObject>
													<plx:nameRef name="secondLeadRole"/>
												</plx:callObject>
											</plx:callInstance>
										</plx:right>
									</plx:binaryOperator>
								</plx:condition>
								<plx:break/>
							</plx:branch>
							<plx:assign>
								<plx:left>
									<plx:nameRef name="parentFact"/>
								</plx:left>
								<plx:right>
									<plx:callInstance name="FactType" type="property">
										<plx:callObject>
											<plx:nameRef name="secondLeadRole"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:right>
							</plx:assign>
							<plx:branch>
								<plx:condition>
									<plx:binaryOperator type="identityInequality">
										<plx:left>
											<plx:callInstance name="GetMatchingReading">
												<plx:callObject>
													<plx:nameRef name="parentFact"/>
												</plx:callObject>
												<plx:passParam>
													<plx:callInstance name="ReadingOrderCollection" type="property">
														<plx:callObject>
															<plx:nameRef name="parentFact"/>
														</plx:callObject>
													</plx:callInstance>
												</plx:passParam>
												<plx:passParam>
													<plx:nullKeyword/>
												</plx:passParam>
												<plx:passParam>
													<plx:nameRef name="secondLeadRole"/>
												</plx:passParam>
												<plx:passParam>
													<plx:nullKeyword/>
												</plx:passParam>
												<plx:passParam>
													<plx:nullKeyword/>
												</plx:passParam>
												<plx:passParam>
													<plx:binaryOperator type="bitwiseOr">
														<plx:left>
															<plx:binaryOperator type="bitwiseOr">
																<plx:left>
																	<plx:callStatic dataTypeName="MatchingReadingOptions" name="NoFrontText" type="field"/>
																</plx:left>
																<plx:right>
																	<plx:callStatic dataTypeName="MatchingReadingOptions" name="NoTrailingText" type="field"/>
																</plx:right>
															</plx:binaryOperator>
														</plx:left>
														<plx:right>
															<plx:callStatic dataTypeName="MatchingReadingOptions" name="NotHyphenBound" type="field"/>
														</plx:right>
													</plx:binaryOperator>
												</plx:passParam>
											</plx:callInstance>
										</plx:left>
										<plx:right>
											<plx:nullKeyword/>
										</plx:right>
									</plx:binaryOperator>
								</plx:condition>
								<plx:local name="otherColumnIndex" dataTypeName=".i4">
									<plx:initialize>
										<plx:binaryOperator type="modulus">
											<plx:left>
												<plx:binaryOperator type="add">
													<plx:left>
														<plx:nameRef name="testColumnIndex"/>
													</plx:left>
													<plx:right>
														<plx:value data="1" type="i4"/>
													</plx:right>
												</plx:binaryOperator>
											</plx:left>
											<plx:right>
												<plx:value data="2" type="i4"/>
											</plx:right>
										</plx:binaryOperator>
									</plx:initialize>
								</plx:local>
								<plx:branch>
									<plx:condition>
										<plx:binaryOperator type="identityInequality">
											<plx:left>
												<plx:callInstance name="RolePlayer" type="property">
													<plx:callObject>
														<plx:callInstance name="Role" type="property">
															<plx:callObject>
																<plx:callInstance name=".implied" type="indexerCall">
																	<plx:callObject>
																		<plx:callInstance name=".implied" type="arrayIndexer">
																			<plx:callObject>
																				<plx:nameRef name="allConstraintRoleSequences"/>
																			</plx:callObject>
																			<plx:passParam>
																				<plx:value data="0" type="i4"/>
																			</plx:passParam>
																		</plx:callInstance>
																	</plx:callObject>
																	<plx:passParam>
																		<plx:nameRef name="otherColumnIndex"/>
																	</plx:passParam>
																</plx:callInstance>
															</plx:callObject>
														</plx:callInstance>
													</plx:callObject>
												</plx:callInstance>
											</plx:left>
											<plx:right>
												<plx:callInstance name="RolePlayer" type="property">
													<plx:callObject>
														<plx:callInstance name="Role" type="property">
															<plx:callObject>
																<plx:callInstance name=".implied" type="indexerCall">
																	<plx:callObject>
																		<plx:callInstance name=".implied" type="arrayIndexer">
																			<plx:callObject>
																				<plx:nameRef name="allConstraintRoleSequences"/>
																			</plx:callObject>
																			<plx:passParam>
																				<plx:value data="1" type="i4"/>
																			</plx:passParam>
																		</plx:callInstance>
																	</plx:callObject>
																	<plx:passParam>
																		<plx:nameRef name="otherColumnIndex"/>
																	</plx:passParam>
																</plx:callInstance>
															</plx:callObject>
														</plx:callInstance>
													</plx:callObject>
												</plx:callInstance>
											</plx:right>
										</plx:binaryOperator>
									</plx:condition>
									<plx:break/>
								</plx:branch>
								<plx:assign>
									<plx:left>
										<plx:nameRef name="binaryLeadReadingColumnIndex"/>
									</plx:left>
									<plx:right>
										<plx:nameRef name="testColumnIndex"/>
									</plx:right>
								</plx:assign>
								<plx:break/>
							</plx:branch>
						</plx:branch>
					</plx:loop>
				</xsl:when>
				<xsl:when test="$blockContext='SubtypeFactRolePlayers'">
					<plx:local name="supertype" dataTypeName="ObjectType">
						<plx:initialize>
							<plx:callThis name="Supertype" type="property"/>
						</plx:initialize>
					</plx:local>
					<plx:local name="subtype" dataTypeName="ObjectType">
						<plx:initialize>
							<plx:callThis name="Subtype" type="property"/>
						</plx:initialize>
					</plx:local>
				</xsl:when>
				<xsl:when test="$blockContext='PreferredFor'">
					<plx:local name="preferredFor" dataTypeName="ObjectType">
						<plx:initialize>
							<plx:callThis name="PreferredIdentifierFor" type="property"/>
						</plx:initialize>
					</plx:local>
				</xsl:when>
				<xsl:when test="$blockContext='SingleRangeBounds'">
					<plx:assign>
						<plx:left>
							<plx:nameRef name="range"/>
						</plx:left>
						<plx:right>
							<plx:callInstance name=".implied" type="indexerCall">
								<plx:callObject>
									<plx:nameRef name="ranges"/>
								</plx:callObject>
								<plx:passParam>
									<plx:value data="0" type="i4"/>
								</plx:passParam>
							</plx:callInstance>
						</plx:right>
					</plx:assign>
					<plx:assign>
						<plx:left>
							<plx:nameRef name="rangeLowerBound"/>
						</plx:left>
						<plx:right>
							<plx:callInstance name="LowerBound" type="property">
								<plx:callObject>
									<plx:nameRef name="range"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:right>
					</plx:assign>
					<plx:assign>
						<plx:left>
							<plx:nameRef name="rangeUpperBound"/>
						</plx:left>
						<plx:right>
							<plx:callInstance name="UpperBound" type="property">
								<plx:callObject>
									<plx:nameRef name="range"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:right>
					</plx:assign>
				</xsl:when>
				<xsl:when test="$blockContext='DataType'">
					<plx:local name="dataTypeUse" dataTypeName="ValueTypeHasDataType">
						<plx:initialize>
							<plx:callInstance name="GetDataTypeLink">
								<plx:callObject>
									<plx:nameRef name="identifyingValueType"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:initialize>
					</plx:local>
					<plx:local name="identifyingDataType" dataTypeName="DataType">
						<plx:initialize>
							<plx:callInstance name="DataType" type="property">
								<plx:callObject>
									<plx:nameRef name="dataTypeUse"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:initialize>
					</plx:local>
				</xsl:when>
				<xsl:when test="$blockContext='DerivedFromTypes'">
					<plx:local name="derivedFactType" dataTypeName="FactType"/>
					<plx:local name="derivedSubtype" dataTypeName="ObjectType"/>
					<plx:local name="dynamicRule" dataTypeName="DynamicRule"/>
					<plx:local name="joinedSetConstraint" dataTypeName="SetConstraint"/>
					<plx:local name="joinedSetComparisonSequence" dataTypeName="SetComparisonConstraintRoleSequence"/>
				</xsl:when>
				<xsl:when test="$blockContext='JoinedSetComparisonConstraint'">
					<plx:local name="joinedSetComparisonConstraint" dataTypeName="SetComparisonConstraint">
						<plx:initialize>
							<plx:callInstance name="ExternalConstraint" type="property">
								<plx:callObject>
									<plx:nameRef name="joinedSetComparisonSequence"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:initialize>
					</plx:local>
				</xsl:when>
				<xsl:otherwise>
					<xsl:message terminate="yes">
						<xsl:text>Unrecognized conditional block context pattern '</xsl:text>
						<xsl:value-of select="$blockContext"/>
						<xsl:text>'.</xsl:text>
					</xsl:message>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>

	<xsl:template match="cvg:IterateValueRanges" mode="ConstraintVerbalization">
		<xsl:param name="TopLevel" select="false()"/>
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="''"/>
		<xsl:param name="CompositeCount"/>
		<xsl:param name="CompositeIterator"/>
		<xsl:param name="ListStyle" select="@listStyle"/>
		<xsl:param name="PatternGroup"/>
		<xsl:param name="IteratorContext"/>
		<xsl:call-template name="EnsureTempStringBuilder"/>
		<plx:local name="rangeCount" dataTypeName=".i4">
			<plx:initialize>
				<plx:callInstance name="Count" type="property">
					<plx:callObject>
						<plx:nameRef name="ranges" type="local"/>
					</plx:callObject>
				</plx:callInstance>
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
						<plx:nameRef name="i" type="local"/>
					</plx:left>
					<plx:right>
						<plx:nameRef name="rangeCount" type="local"/>
					</plx:right>
				</plx:binaryOperator>
			</plx:condition>
			<plx:beforeLoop>
				<plx:increment>
					<plx:nameRef name="i" type="local"/>
				</plx:increment>
			</plx:beforeLoop>
			<plx:local name="minValue" dataTypeName=".string">
				<plx:initialize>
					<plx:callInstance name="MinValue" type="property">
						<plx:callObject>
							<plx:callInstance name=".implied" type="indexerCall">
								<plx:callObject>
									<plx:nameRef name="ranges"/>
								</plx:callObject>
								<plx:passParam>
									<plx:nameRef name="i" type="local"/>
								</plx:passParam>
							</plx:callInstance>
						</plx:callObject>
					</plx:callInstance>
				</plx:initialize>
			</plx:local>
			<plx:local name="maxValue" dataTypeName=".string">
				<plx:initialize>
					<plx:callInstance name="MaxValue" type="property">
						<plx:callObject>
							<plx:callInstance name=".implied" type="indexerCall">
								<plx:callObject>
									<plx:nameRef name="ranges"/>
								</plx:callObject>
								<plx:passParam>
									<plx:nameRef name="i" type="local"/>
								</plx:passParam>
							</plx:callInstance>
						</plx:callObject>
					</plx:callInstance>
				</plx:initialize>
			</plx:local>
			<plx:local name="minInclusion" dataTypeName="RangeInclusion">
				<plx:initialize>
					<plx:callInstance name="MinInclusion" type="property">
						<plx:callObject>
							<plx:callInstance name=".implied" type="indexerCall">
								<plx:callObject>
									<plx:nameRef name="ranges"/>
								</plx:callObject>
								<plx:passParam>
									<plx:nameRef name="i" type="local"/>
								</plx:passParam>
							</plx:callInstance>
						</plx:callObject>
					</plx:callInstance>
				</plx:initialize>
			</plx:local>
			<plx:local name="maxInclusion" dataTypeName="RangeInclusion">
				<plx:initialize>
					<plx:callInstance name="MaxInclusion" type="property">
						<plx:callObject>
							<plx:callInstance name=".implied" type="indexerCall">
								<plx:callObject>
									<plx:nameRef name="ranges"/>
								</plx:callObject>
								<plx:passParam>
									<plx:nameRef name="i" type="local"/>
								</plx:passParam>
							</plx:callInstance>
						</plx:callObject>
					</plx:callInstance>
				</plx:initialize>
			</plx:local>

			<xsl:call-template name="IterateRolesConstraintVerbalizationBody">
				<xsl:with-param name="iterVarName" select="'i'"/>
				<xsl:with-param name="contextMatch" select="'rangeCount'"/>
				<xsl:with-param name="ListStyle" select="@listStyle"/>
				<xsl:with-param name="CompositeIterator" select="'i'"/>
				<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
				<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
			</xsl:call-template>
		</plx:loop>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="{$VariablePrefix}{$VariableDecorator}" type="local"/>
			</plx:left>
			<plx:right>
				<plx:callInstance name="ToString">
					<plx:callObject>
						<plx:nameRef name="sbTemp" type="local"/>
					</plx:callObject>
				</plx:callInstance>
			</plx:right>
		</plx:assign>
	</xsl:template>
	<xsl:template match="cvg:IterateRanges" mode="ConstraintVerbalization">
		<xsl:param name="TopLevel" select="false()"/>
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="''"/>
		<xsl:param name="CompositeCount"/>
		<xsl:param name="CompositeIterator"/>
		<xsl:param name="ListStyle" select="@listStyle"/>
		<xsl:param name="PatternGroup"/>
		<xsl:param name="IteratorContext"/>
		<xsl:call-template name="EnsureTempStringBuilder"/>
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
						<plx:nameRef name="i" type="local"/>
					</plx:left>
					<plx:right>
						<plx:nameRef name="rangeCount" type="local"/>
					</plx:right>
				</plx:binaryOperator>
			</plx:condition>
			<plx:beforeLoop>
				<plx:increment>
					<plx:nameRef name="i" type="local"/>
				</plx:increment>
			</plx:beforeLoop>
			<plx:assign>
				<plx:left>
					<plx:nameRef name="range"/>
				</plx:left>
				<plx:right>
					<plx:callInstance name=".implied" type="indexerCall">
						<plx:callObject>
							<plx:nameRef name="ranges"/>
						</plx:callObject>
						<plx:passParam>
							<plx:nameRef name="i"/>
						</plx:passParam>
					</plx:callInstance>
				</plx:right>
			</plx:assign>
			<plx:assign>
				<plx:left>
					<plx:nameRef name="rangeLowerBound"/>
				</plx:left>
				<plx:right>
					<plx:callInstance name="LowerBound" type="property">
						<plx:callObject>
							<plx:nameRef name="range"/>
						</plx:callObject>
					</plx:callInstance>
				</plx:right>
			</plx:assign>
			<plx:assign>
				<plx:left>
					<plx:nameRef name="rangeUpperBound"/>
				</plx:left>
				<plx:right>
					<plx:callInstance name="UpperBound" type="property">
						<plx:callObject>
							<plx:nameRef name="range"/>
						</plx:callObject>
					</plx:callInstance>
				</plx:right>
			</plx:assign>
			<xsl:call-template name="IterateRolesConstraintVerbalizationBody">
				<xsl:with-param name="iterVarName" select="'i'"/>
				<xsl:with-param name="contextMatch" select="'rangeCount'"/>
				<xsl:with-param name="ListStyle" select="@listStyle"/>
				<xsl:with-param name="CompositeIterator" select="'i'"/>
				<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
				<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
			</xsl:call-template>
		</plx:loop>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="{$VariablePrefix}{$VariableDecorator}" type="local"/>
			</plx:left>
			<plx:right>
				<plx:callInstance name="ToString">
					<plx:callObject>
						<plx:nameRef name="sbTemp" type="local"/>
					</plx:callObject>
				</plx:callInstance>
			</plx:right>
		</plx:assign>
	</xsl:template>
	<!-- An IterateContextRoles tag is used to walk elements within another iteration
		 context. Pattern matching is very similar to predicate replacement except that
		 the roles are listed instead of matched to replacement fields in the predicate text -->
	<xsl:template match="cvg:IterateContextRoles" mode="ConstraintVerbalization">
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="'contextIterator'"/>
		<xsl:param name="TopLevel" select="false()"/>
		<xsl:param name="CompositeCount"/>
		<xsl:param name="CompositeIterator"/>
		<xsl:param name="ListStyle" select="@listStyle"/>
		<xsl:param name="IteratorContext" select="''"/>
		<xsl:param name="PatternGroup"/>
		<xsl:if test="$TopLevel or not($IteratorContext)">
			<xsl:message terminate="yes">IterateContextRoles must occur in the context of another iterator.</xsl:message>
		</xsl:if>
		<xsl:if test="not($CompositeCount)">
			<xsl:call-template name="PushIteratorContext">
				<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
				<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:call-template name="RoleIterator">
			<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
			<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
			<xsl:with-param name="CompositeCount" select="$CompositeCount"/>
			<xsl:with-param name="CompositeIterator" select="$CompositeIterator"/>
			<xsl:with-param name="ListStyle" select="$ListStyle"/>
			<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
			<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
		</xsl:call-template>
		<xsl:if test="not($CompositeCount)">
			<xsl:call-template name="PopIteratorContext">
				<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
				<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
	<xsl:template name="PushIteratorContext">
		<xsl:param name="VariableDecorator"/>
		<xsl:param name="VariablePrefix"/>
		<plx:local name="contextPrimaryRole" dataTypeName="RoleBase">
			<plx:initialize>
				<plx:nameRef name="primaryRole"/>
			</plx:initialize>
		</plx:local>
		<plx:local name="contextTempStringBuildLength" dataTypeName=".i4">
			<plx:initialize>
				<plx:callInstance name="Length" type="property">
					<plx:callObject>
						<plx:nameRef name="sbTemp"/>
					</plx:callObject>
				</plx:callInstance>
			</plx:initialize>
		</plx:local>
	</xsl:template>
	<xsl:template name="PopIteratorContext">
		<xsl:param name="VariableDecorator"/>
		<xsl:param name="VariablePrefix"/>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="primaryRole"/>
			</plx:left>
			<plx:right>
				<plx:nameRef name="contextPrimaryRole"/>
			</plx:right>
		</plx:assign>
		<plx:assign>
			<plx:left>
				<plx:callInstance name="Length" type="property">
					<plx:callObject>
						<plx:nameRef name="sbTemp"/>
					</plx:callObject>
				</plx:callInstance>
			</plx:left>
			<plx:right>
				<plx:nameRef name="contextTempStringBuildLength"/>
			</plx:right>
		</plx:assign>
	</xsl:template>
	<!-- An IterateRoles tag is used to walk a set of roles and combine verbalizations for
		 the roles into a list. The type of verbalization depends on the match and filter attributes
		 specified on the list. The list separators are determined by the contents of the listStyle attribute. -->
	<xsl:template match="cvg:IterateRoles" mode="ConstraintVerbalization">
		<xsl:param name="TopLevel" select="false()"/>
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="''"/>
		<xsl:param name="CompositeCount"/>
		<xsl:param name="CompositeIterator"/>
		<xsl:param name="CompositeReplacementArray"/>
		<xsl:param name="ListStyle" select="@listStyle"/>
		<xsl:param name="PatternGroup"/>
		<xsl:param name="SequenceIterator" select="false()"/>
		<xsl:variable name="useVariablePrefixFragment">
			<xsl:choose>
				<xsl:when test="$VariablePrefix">
					<xsl:value-of select="$VariablePrefix"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text>factText</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:call-template name="RoleIterator">
			<xsl:with-param name="TopLevel" select="$TopLevel"/>
			<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
			<xsl:with-param name="VariablePrefix" select="string($useVariablePrefixFragment)"/>
			<xsl:with-param name="CompositeCount" select="$CompositeCount"/>
			<xsl:with-param name="CompositeIterator" select="$CompositeIterator"/>
			<xsl:with-param name="CompositeReplacementArray" select="$CompositeReplacementArray"/>
			<xsl:with-param name="ListStyle" select="$ListStyle"/>
			<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
			<xsl:with-param name="SequenceIterator" select="$SequenceIterator"/>
		</xsl:call-template>
	</xsl:template>
	<!-- An IterateSequences tag is used to walk a set of RoleSequences and transform them to be compatible with the
			patterns already in place to verbalize constraints. -->
	<xsl:template match="cvg:IterateSequences" mode="ConstraintVerbalization">
		<xsl:param name="TopLevel" select="false()"/>
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="''"/>
		<xsl:param name="CompositeCount"/>
		<xsl:param name="CompositeIterator"/>
		<xsl:param name="ListStyle" select="@listStyle"/>
		<xsl:param name="PatternGroup"/>
		<xsl:param name="IteratorContext"/>
		<xsl:variable name="useVariablePrefixFragment">
			<xsl:choose>
				<xsl:when test="$VariablePrefix">
					<xsl:value-of select="$VariablePrefix"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text>factText</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="useVariablePrefix" select="string($useVariablePrefixFragment)"/>
		<xsl:variable name="contextMatchFragment">
			<xsl:choose>
				<xsl:when test="string-length(@match)">
					<xsl:value-of select="@match"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text>all</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="contextMatch" select="string($contextMatchFragment)"/>
		<xsl:variable name="iteratorVarName" select="concat($useVariablePrefix, $SequenceIterVariablePart, $VariableDecorator)"/>
		<xsl:variable name="isCompositeList" select="not(@listStyle='null') and not(@compositeList) or @compositeList='1' or @compositeList='true'"/>
		<plx:local name="{$iteratorVarName}" dataTypeName=".i4"/>
		<xsl:variable name="loopHeaderFragment">
			<plx:initializeLoop>
				<plx:assign>
					<plx:left>
						<plx:nameRef name="{$iteratorVarName}"/>
					</plx:left>
					<plx:right>
						<xsl:choose>
							<xsl:when test="@pass='notFirst'">
								<plx:value type="i4" data="1"/>
							</xsl:when>
							<xsl:otherwise>
								<plx:value type="i4" data="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</plx:right>
				</plx:assign>
			</plx:initializeLoop>
			<plx:condition>
				<plx:binaryOperator type="lessThan">
					<plx:left>
						<plx:nameRef name="{$iteratorVarName}"/>
					</plx:left>
					<plx:right>
						<xsl:choose>
							<xsl:when test="@pass='first'">
								<plx:value type="i4" data="1"/>
							</xsl:when>
							<xsl:otherwise>
								<plx:nameRef name="constraintRoleArity"/>
							</xsl:otherwise>
						</xsl:choose>
					</plx:right>
				</plx:binaryOperator>
			</plx:condition>
			<plx:beforeLoop>
				<plx:increment>
					<plx:nameRef name="{$iteratorVarName}"/>
				</plx:increment>
			</plx:beforeLoop>
		</xsl:variable>
		<xsl:variable name="loopLocalsFragment">
			<plx:local dataTypeName="IList" name="includedConstraintRoles">
				<plx:passTypeParam dataTypeName="ConstraintRoleSequenceHasRole"/>
				<plx:initialize>
					<plx:callInstance name=".implied" type="indexerCall">
						<plx:callObject>
							<plx:nameRef name="allConstraintRoleSequences"/>
						</plx:callObject>
						<plx:passParam>
							<plx:nameRef name="{$iteratorVarName}"/>
						</plx:passParam>
					</plx:callInstance>
				</plx:initialize>
			</plx:local>
			<plx:local dataTypeName="ConstraintRoleSequenceJoinPath" name="joinPath">
				<plx:initialize>
					<plx:callInstance name="JoinPath" type="property">
						<plx:callObject>
							<plx:callInstance name=".implied" type="indexerCall">
								<plx:callObject>
									<plx:nameRef name="constraintSequences"/>
								</plx:callObject>
								<plx:passParam>
									<plx:nameRef name="{$iteratorVarName}"/>
								</plx:passParam>
							</plx:callInstance>
						</plx:callObject>
					</plx:callInstance>
				</plx:initialize>
			</plx:local>
		</xsl:variable>
		<xsl:variable name="loopLocals" select="exsl:node-set($loopLocalsFragment)/child::*"/>
		<xsl:choose>
			<xsl:when test="$isCompositeList">
				<xsl:variable name="childVariablePrefix" select="concat($useVariablePrefix,$VariableDecorator,'Item')"/>
				<xsl:variable name="compositeCountVarName" select="concat($useVariablePrefix,'CompositeCount',$VariableDecorator)"/>
				<xsl:variable name="useOutdentFormatter" select="boolean(self::*[not(parent::*[@applyTrailingOutdentHere='true' or @applyTrailingOutdentHere='1'])]/cvg:SequenceJoinPath[@markTrailingOutdentStart='true' or @markTrailingOutdentStart='1'])"/>
				<plx:local name="{$compositeCountVarName}" dataTypeName=".i4">
					<plx:initialize>
						<plx:value type="i4" data="0"/>
					</plx:initialize>
				</plx:local>
				<xsl:variable name="compositeIteratorVarName" select="concat($useVariablePrefix,'CompositeIterator',$VariableDecorator)"/>
				<xsl:variable name="compositeReplacementsVarNameFragment">
					<xsl:if test="$useOutdentFormatter">
						<xsl:value-of select="concat($useVariablePrefix,'CompositeFields',$VariableDecorator)"/>
					</xsl:if>
				</xsl:variable>
				<xsl:variable name="compositeReplacementsVarName" select="string($compositeReplacementsVarNameFragment)"/>
				<plx:local name="{$compositeIteratorVarName}" dataTypeName=".i4"/>
				<xsl:apply-templates select="*" mode="IterateRolesFilterInitializeState">
					<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
					<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
					<xsl:with-param name="VariablePrefix" select="$childVariablePrefix"/>
					<xsl:with-param name="VariableDecorator" select="position()"/>
				</xsl:apply-templates>
				<plx:loop>
					<xsl:copy-of select="$loopHeaderFragment"/>
					<xsl:variable name="bodyCodeFragment">
						<xsl:for-each select="child::*">
							<xsl:apply-templates select="." mode="CompositeOrFilteredListCount">
								<xsl:with-param name="TotalCountCollectorVariable" select="$compositeCountVarName"/>
								<xsl:with-param name="IteratorVariableName" select="$compositeIteratorVarName"/>
								<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
								<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
								<xsl:with-param name="VariablePrefix" select="$childVariablePrefix"/>
								<xsl:with-param name="VariableDecorator" select="position()"/>
							</xsl:apply-templates>
							<xsl:apply-templates select="." mode="IterateRolesFilterReinitializeState">
								<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
								<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
								<xsl:with-param name="VariablePrefix" select="$childVariablePrefix"/>
								<xsl:with-param name="VariableDecorator" select="position()"/>
							</xsl:apply-templates>
						</xsl:for-each>
					</xsl:variable>
					<xsl:call-template name="StripUnusedLocals">
						<xsl:with-param name="LocalCode" select="$loopLocals"/>
						<xsl:with-param name="ReferencingCode" select="exsl:node-set($bodyCodeFragment)/child::*"/>
					</xsl:call-template>
				</plx:loop>
				<plx:assign>
					<plx:left>
						<plx:nameRef name="{$compositeIteratorVarName}"/>
					</plx:left>
					<plx:right>
						<plx:value data="0" type="i4"/>
					</plx:right>
				</plx:assign>
				<xsl:if test="$useOutdentFormatter">
					<plx:local name="{$compositeReplacementsVarName}" dataTypeName=".string" dataTypeIsSimpleArray="true">
						<plx:initialize>
							<plx:callNew dataTypeName=".string" dataTypeIsSimpleArray="true">
								<plx:passParam>
									<plx:nameRef name="{$compositeCountVarName}"/>
								</plx:passParam>
							</plx:callNew>
						</plx:initialize>
					</plx:local>
				</xsl:if>
				<xsl:call-template name="EnsureTempStringBuilder"/>
				<plx:loop>
					<xsl:copy-of select="$loopHeaderFragment"/>
					<plx:local name="{$childVariablePrefix}{position()}" dataTypeName=".string"/>
					<xsl:variable name="bodyCodeFragment">
						<xsl:for-each select="child::*">
							<xsl:apply-templates mode="ConstraintVerbalization" select=".">
								<xsl:with-param name="TopLevel" select="$TopLevel"/>
								<xsl:with-param name="VariablePrefix" select="$childVariablePrefix"/>
								<xsl:with-param name="VariableDecorator" select="position()"/>
								<xsl:with-param name="CompositeCount" select="$compositeCountVarName"/>
								<xsl:with-param name="CompositeIterator" select="$compositeIteratorVarName"/>
								<xsl:with-param name="CompositeReplacementArray" select="$compositeReplacementsVarName"/>
								<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
								<xsl:with-param name="SequenceIterator" select="$iteratorVarName"/>
								<xsl:with-param name="ListStyle" select="$ListStyle"/>
								<xsl:with-param name="ConditionalMatch" select="@conditionalMatch"/>
							</xsl:apply-templates>
							<xsl:apply-templates select="." mode="IterateRolesFilterReinitializeState">
								<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
								<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
								<xsl:with-param name="VariablePrefix" select="$childVariablePrefix"/>
								<xsl:with-param name="VariableDecorator" select="position()"/>
							</xsl:apply-templates>
						</xsl:for-each>
					</xsl:variable>
					<xsl:call-template name="StripUnusedLocals">
						<xsl:with-param name="LocalCode" select="$loopLocals"/>
						<xsl:with-param name="ReferencingCode" select="exsl:node-set($bodyCodeFragment)/child::*"/>
					</xsl:call-template>
				</plx:loop>
				<xsl:if test="$useOutdentFormatter">
					<xsl:variable name="compositeFormatVarName" select="concat($useVariablePrefix,$FormatVariablePart,$VariableDecorator)"/>
					<plx:local name="{$compositeFormatVarName}" dataTypeName=".string">
						<plx:initialize>
							<plx:callInstance name="ToString">
								<plx:callObject>
									<plx:nameRef name="sbTemp"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:initialize>
					</plx:local>
					<plx:assign>
						<plx:left>
							<plx:callInstance name="Length" type="property">
								<plx:callObject>
									<plx:nameRef name="sbTemp"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:left>
						<plx:right>
							<plx:value data="0" type="i4"/>
						</plx:right>
					</plx:assign>
					<plx:callStatic name="FormatResolveOutdent" dataTypeName="RolePathVerbalizer">
						<plx:passParam>
							<plx:callInstance name="FormatProvider" type="property">
								<plx:callObject>
									<plx:nameRef type="parameter" name="writer"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:passParam>
						<plx:passParam>
							<plx:nameRef name="sbTemp"/>
						</plx:passParam>
						<plx:passParam>
							<plx:callInstance name="NewLine" type="property">
								<plx:callObject>
									<plx:nameRef type="parameter" name="writer"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:passParam>
						<plx:passParam>
							<plx:nameRef name="{$compositeFormatVarName}"/>
						</plx:passParam>
						<plx:passParam>
							<plx:nameRef name="{$compositeReplacementsVarName}"/>
						</plx:passParam>
					</plx:callStatic>
				</xsl:if>
			</xsl:when>
			<xsl:otherwise>
				<!--Iterate through sequences as the outer loop of iterate roles-->
				<plx:loop>
					<xsl:copy-of select="$loopHeaderFragment"/>
					<xsl:variable name="bodyCodeFragment">
						<xsl:if test="not($ListStyle='null')">
							<xsl:message terminate="yes">IterateSequences must be composite if a listStyle is set.</xsl:message>
							<!-- UNDONE: The list snippet population is in the wrong place, it should be outside the loop, and we should
							conditionally push/pop an iterator context -->
							<plx:local name="listSnippet" dataTypeName="{$VerbalizationTextSnippetType}"/>
							<xsl:call-template name="PopulateListSnippet">
								<xsl:with-param name="IteratorVariableName" select="$iteratorVarName"/>
								<xsl:with-param name="ListStyle" select="$ListStyle"/>
								<xsl:with-param name="IteratorBound">
									<plx:nameRef name="constraintRoleArity"/>
								</xsl:with-param>
							</xsl:call-template>
						</xsl:if>
						<xsl:apply-templates mode="ConstraintVerbalization" select="child::*">
							<xsl:with-param name="TopLevel" select="$TopLevel"/>
							<xsl:with-param name="VariablePrefix" select="$useVariablePrefix"/>
							<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
							<xsl:with-param name="CompositeCount" select="$CompositeCount"/>
							<xsl:with-param name="CompositeIterator" select="$CompositeIterator"/>
							<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
							<xsl:with-param name="SequenceIterator" select="$iteratorVarName"/>
							<xsl:with-param name="ParentListStyle" select="$ListStyle"/>
							<xsl:with-param name="ConditionalMatch" select="@conditionalMatch"/>
						</xsl:apply-templates>
					</xsl:variable>
					<xsl:call-template name="StripUnusedLocals">
						<xsl:with-param name="LocalCode" select="$loopLocals"/>
						<xsl:with-param name="ReferencingCode" select="exsl:node-set($bodyCodeFragment)/child::*"/>
					</xsl:call-template>
				</plx:loop>
			</xsl:otherwise>
		</xsl:choose>
		<xsl:if test="not($ListStyle='null')">
			<plx:assign>
				<plx:left>
					<plx:nameRef name="{$useVariablePrefix}{$VariableDecorator}"/>
				</plx:left>
				<plx:right>
					<plx:callInstance name="ToString">
						<plx:callObject>
							<plx:nameRef name="sbTemp"/>
						</plx:callObject>
					</plx:callInstance>
				</plx:right>
			</plx:assign>
		</xsl:if>
	</xsl:template>
	<xsl:template match="cvg:IteratePathNodeLists" mode="ConstraintVerbalization">
		<xsl:param name="TopLevel" select="false()"/>
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="''"/>
		<xsl:param name="CompositeCount"/>
		<xsl:param name="CompositeIterator"/>
		<xsl:param name="ListStyle" select="@listStyle"/>
		<xsl:param name="PatternGroup"/>
		<xsl:param name="IteratorContext"/>
		<xsl:call-template name="EnsureTempStringBuilder"/>
		<xsl:variable name="match" select="string(@match)"/>
		<plx:local name="pathNodeLists" dataTypeName="IList">
			<plx:passTypeParam dataTypeName="Tuple">
				<plx:passTypeParam dataTypeName="DynamicRuleNodeState"/>
				<plx:passTypeParam dataTypeName="List">
					<plx:passTypeParam dataTypeName="RolePathNode"/>
				</plx:passTypeParam>
			</plx:passTypeParam>
			<plx:initialize>
				<plx:callInstance type="property">
					<xsl:attribute name="name">
						<xsl:choose>
							<xsl:when test="$match='quantifiers'">
								<xsl:text>DynamicRuleHeadNodes</xsl:text>
							</xsl:when>
						</xsl:choose>
					</xsl:attribute>
					<plx:callObject>
						<plx:nameRef name="pathVerbalizer"/>
					</plx:callObject>
				</plx:callInstance>
			</plx:initialize>
		</plx:local>
		<plx:local name="pathNodeListCount" dataTypeName=".i4">
			<plx:initialize>
				<plx:callInstance name="Count" type="property">
					<plx:callObject>
						<plx:nameRef name="pathNodeLists" type="local"/>
					</plx:callObject>
				</plx:callInstance>
			</plx:initialize>
		</plx:local>
		<plx:loop>
			<plx:initializeLoop>
				<plx:local name="iNodeList" dataTypeName=".i4">
					<plx:initialize>
						<plx:value data="0" type="i4"/>
					</plx:initialize>
				</plx:local>
			</plx:initializeLoop>
			<plx:condition>
				<plx:binaryOperator type="lessThan">
					<plx:left>
						<plx:nameRef name="iNodeList" type="local"/>
					</plx:left>
					<plx:right>
						<plx:nameRef name="pathNodeListCount" type="local"/>
					</plx:right>
				</plx:binaryOperator>
			</plx:condition>
			<plx:beforeLoop>
				<plx:increment>
					<plx:nameRef name="iNodeList" type="local"/>
				</plx:increment>
			</plx:beforeLoop>
			<plx:local name="pathNodeListTuple" dataTypeName="Tuple">
				<plx:passTypeParam dataTypeName="DynamicRuleNodeState"/>
				<plx:passTypeParam dataTypeName="List">
					<plx:passTypeParam dataTypeName="RolePathNode"/>
				</plx:passTypeParam>
				<plx:initialize>
					<plx:callInstance name=".implied" type="indexerCall">
						<plx:callObject>
							<plx:nameRef name="pathNodeLists"/>
						</plx:callObject>
						<plx:passParam>
							<plx:nameRef name="iNodeList"/>
						</plx:passParam>
					</plx:callInstance>
				</plx:initialize>
			</plx:local>
			<plx:local name="dynamicState" dataTypeName="DynamicRuleNodeState">
				<plx:initialize>
					<plx:callInstance name="Item1" type="property">
						<plx:callObject>
							<plx:nameRef name="pathNodeListTuple"/>
						</plx:callObject>
					</plx:callInstance>
				</plx:initialize>
			</plx:local>
			<plx:local name="pathNodeList" dataTypeName="IList">
				<plx:passTypeParam dataTypeName="RolePathNode"/>
				<plx:initialize>
					<plx:callInstance name="Item2" type="property">
						<plx:callObject>
							<plx:nameRef name="pathNodeListTuple"/>
						</plx:callObject>
					</plx:callInstance>
				</plx:initialize>
			</plx:local>
			<xsl:call-template name="IterateRolesConstraintVerbalizationBody">
				<xsl:with-param name="iterVarName" select="'iNodeList'"/>
				<xsl:with-param name="contextMatch" select="'pathNodeLists'"/>
				<xsl:with-param name="ListStyle" select="@listStyle"/>
				<xsl:with-param name="CompositeIterator" select="'iNodeList'"/>
				<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
				<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
			</xsl:call-template>
		</plx:loop>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="{$VariablePrefix}{$VariableDecorator}" type="local"/>
			</plx:left>
			<plx:right>
				<plx:callInstance name="ToString">
					<plx:callObject>
						<plx:nameRef name="sbTemp" type="local"/>
					</plx:callObject>
				</plx:callInstance>
			</plx:right>
		</plx:assign>
	</xsl:template>
	<xsl:template match="cvg:IteratePathNodes" mode="ConstraintVerbalization">
		<xsl:param name="TopLevel" select="false()"/>
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="''"/>
		<xsl:param name="CompositeCount"/>
		<xsl:param name="CompositeIterator"/>
		<xsl:param name="ListStyle" select="@listStyle"/>
		<xsl:param name="PatternGroup"/>
		<xsl:param name="IteratorContext"/>
		<xsl:variable name="innerIterator" select="$IteratorContext='pathNodeLists'"/>
		<xsl:choose>
			<xsl:when test="$innerIterator">
				<plx:local name="outerStringBuilderLength" dataTypeName=".i4">
					<plx:initialize>
						<plx:callInstance name="Length" type="property">
							<plx:callObject>
								<plx:nameRef name="sbTemp"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:initialize>
				</plx:local>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="EnsureTempStringBuilder"/>
			</xsl:otherwise>
		</xsl:choose>
		<xsl:variable name="match" select="string(@match)"/>
		<plx:local name="pathNodes" dataTypeName="IList">
			<plx:passTypeParam dataTypeName="RolePathNode"/>
			<plx:initialize>
				<xsl:choose>
					<xsl:when test="$match='pathNodeList'">
						<plx:callInstance name="Item2" type="property">
							<plx:callObject>
								<plx:callInstance name=".implied" type="indexerCall">
									<plx:callObject>
										<plx:nameRef name="pathNodeLists"/>
									</plx:callObject>
									<plx:passParam>
										<plx:nameRef name="iNodeList"/>
									</plx:passParam>
								</plx:callInstance>
							</plx:callObject>
						</plx:callInstance>
					</xsl:when>
					<xsl:otherwise>
						<plx:callInstance type="property">
							<xsl:attribute name="name">
								<xsl:choose>
									<xsl:when test="$match='pathNodeList'">
										<xsl:text>DynamicRuleHeadNodes</xsl:text>
									</xsl:when>
									<xsl:when test="$match='add'">
										<xsl:text>DynamicRuleAddNodes</xsl:text>
									</xsl:when>
									<xsl:when test="$match='delete'">
										<xsl:text>DynamicRuleDeleteNodes</xsl:text>
									</xsl:when>
								</xsl:choose>
							</xsl:attribute>
							<plx:callObject>
								<plx:nameRef name="pathVerbalizer"/>
							</plx:callObject>
						</plx:callInstance>
					</xsl:otherwise>
				</xsl:choose>
			</plx:initialize>
		</plx:local>
		<plx:local name="pathNodeCount" dataTypeName=".i4">
			<plx:initialize>
				<plx:callInstance name="Count" type="property">
					<plx:callObject>
						<plx:nameRef name="pathNodes" type="local"/>
					</plx:callObject>
				</plx:callInstance>
			</plx:initialize>
		</plx:local>
		<plx:loop>
			<plx:initializeLoop>
				<plx:local name="iNode" dataTypeName=".i4">
					<plx:initialize>
						<plx:value data="0" type="i4"/>
					</plx:initialize>
				</plx:local>
			</plx:initializeLoop>
			<plx:condition>
				<plx:binaryOperator type="lessThan">
					<plx:left>
						<plx:nameRef name="iNode" type="local"/>
					</plx:left>
					<plx:right>
						<plx:nameRef name="pathNodeCount" type="local"/>
					</plx:right>
				</plx:binaryOperator>
			</plx:condition>
			<plx:beforeLoop>
				<plx:increment>
					<plx:nameRef name="iNode" type="local"/>
				</plx:increment>
			</plx:beforeLoop>
			<plx:assign>
				<plx:left>
					<plx:nameRef name="pathNode"/>
				</plx:left>
				<plx:right>
					<plx:callInstance name=".implied" type="indexerCall">
						<plx:callObject>
							<plx:nameRef name="pathNodes"/>
						</plx:callObject>
						<plx:passParam>
							<plx:nameRef name="iNode"/>
						</plx:passParam>
					</plx:callInstance>
				</plx:right>
			</plx:assign>
			<xsl:call-template name="IterateRolesConstraintVerbalizationBody">
				<xsl:with-param name="iterVarName" select="'iNode'"/>
				<xsl:with-param name="contextMatch" select="'pathNodeCount'"/>
				<xsl:with-param name="ListStyle" select="@listStyle"/>
				<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
				<xsl:with-param name="CompositeIterator" select="'iNode'"/>
				<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
				<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
			</xsl:call-template>
		</plx:loop>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="{$VariablePrefix}{$VariableDecorator}" type="local"/>
			</plx:left>
			<plx:right>
				<plx:callInstance name="ToString">
					<plx:callObject>
						<plx:nameRef name="sbTemp" type="local"/>
					</plx:callObject>
					<xsl:if test="$innerIterator">
						<plx:passParam>
							<plx:nameRef name="outerStringBuilderLength"/>
						</plx:passParam>
						<plx:passParam>
							<plx:binaryOperator type="subtract">
								<plx:left>
									<plx:callInstance name="Length" type="property">
										<plx:callObject>
											<plx:nameRef name="sbTemp"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:left>
								<plx:right>
									<plx:nameRef name="outerStringBuilderLength"/>
								</plx:right>
							</plx:binaryOperator>
						</plx:passParam>
					</xsl:if>
				</plx:callInstance>
			</plx:right>
		</plx:assign>
		<xsl:if test="$innerIterator">
			<plx:assign>
				<plx:left>
					<plx:callInstance name="Length" type="property">
						<plx:callObject>
							<plx:nameRef name="sbTemp"/>
						</plx:callObject>
					</plx:callInstance>
				</plx:left>
				<plx:right>
					<plx:nameRef name="outerStringBuilderLength"/>
				</plx:right>
			</plx:assign>
		</xsl:if>
	</xsl:template>
	<xsl:template match="cvg:PathNodeVariable|cvg:PathNodeFactType" mode="ConstraintVerbalization">
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="'variableSnippet'"/>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="{$VariablePrefix}{$VariableDecorator}" type="local"/>
			</plx:left>
			<plx:right>
				<plx:callInstance name="RenderAssociatedRolePlayer">
					<plx:callObject>
						<plx:nameRef name="pathVerbalizer"/>
					</plx:callObject>
					<plx:passParam>
						<plx:nameRef name="pathNode"/>
					</plx:passParam>
					<plx:passParam>
						<plx:nullKeyword/>
					</plx:passParam>
					<plx:passParam>
						<plx:callStatic dataTypeName="RolePathRolePlayerRenderingOptions" name="UsedInVerbalizationHead" type="field"/>
					</plx:passParam>
				</plx:callInstance>
			</plx:right>
		</plx:assign>
	</xsl:template>
	<xsl:template match="cvg:PathNodeFactType" mode="ConstraintVerbalization">
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="'variableSnippet'"/>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="{$VariablePrefix}{$VariableDecorator}" type="local"/>
			</plx:left>
			<plx:right>
				<plx:callInstance name="RenderPathFactType">
					<plx:callObject>
						<plx:nameRef name="pathVerbalizer"/>
					</plx:callObject>
					<plx:passParam>
						<plx:nameRef name="pathNode"/>
					</plx:passParam>
					<plx:passParam>
						<plx:nameRef name="sbTemp"/>
					</plx:passParam>
				</plx:callInstance>
			</plx:right>
		</plx:assign>
	</xsl:template>
	<!-- An IterateQueryParameters tag is used to walk a set of QueryParameters and transform them to be compatible with the
			patterns already in place to verbalize constraints. -->
	<xsl:template match="cvg:IterateQueryParameters" mode="ConstraintVerbalization">
		<xsl:param name="TopLevel" select="false()"/>
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="''"/>
		<xsl:param name="CompositeCount"/>
		<xsl:param name="CompositeIterator"/>
		<xsl:param name="ListStyle" select="@listStyle"/>
		<xsl:param name="PatternGroup"/>
		<xsl:param name="IteratorContext"/>
		<xsl:call-template name="EnsureTempStringBuilder"/>
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
						<plx:nameRef name="i" type="local"/>
					</plx:left>
					<plx:right>
						<plx:nameRef name="queryParameterCount" type="local"/>
					</plx:right>
				</plx:binaryOperator>
			</plx:condition>
			<plx:beforeLoop>
				<plx:increment>
					<plx:nameRef name="i" type="local"/>
				</plx:increment>
			</plx:beforeLoop>
			<plx:local name="queryParameter" dataTypeName="QueryParameter">
				<plx:initialize>
					<plx:callInstance type="indexerCall" name=".implied">
						<plx:callObject>
							<plx:nameRef name="queryParameters"/>
						</plx:callObject>
						<plx:passParam>
							<plx:nameRef name="i"/>
						</plx:passParam>
					</plx:callInstance>
				</plx:initialize>
			</plx:local>
			<plx:local name="queryParameterName" dataTypeName=".string">
				<plx:initialize>
					<plx:callInstance name="Name" type="property">
						<plx:callObject>
							<plx:nameRef name="queryParameter"/>
						</plx:callObject>
					</plx:callInstance>
				</plx:initialize>
			</plx:local>

			<xsl:call-template name="IterateRolesConstraintVerbalizationBody">
				<xsl:with-param name="iterVarName" select="'i'"/>
				<xsl:with-param name="contextMatch" select="'queryParameterCount'"/>
				<xsl:with-param name="ListStyle" select="@listStyle"/>
				<xsl:with-param name="CompositeIterator" select="'i'"/>
				<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
				<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
			</xsl:call-template>
		</plx:loop>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="{$VariablePrefix}{$VariableDecorator}" type="local"/>
			</plx:left>
			<plx:right>
				<plx:callInstance name="ToString">
					<plx:callObject>
						<plx:nameRef name="sbTemp" type="local"/>
					</plx:callObject>
				</plx:callInstance>
			</plx:right>
		</plx:assign>
	</xsl:template>
	<!-- An IterateInstances tag is used to walk a set of instances and combine the verbalizations for
			those instances into a single list. The type of verbalization is not necessarily flexible, aside from
			allowing the user to rework the TextInstanceValue and NonTextInstanceValue snippets. -->
	<xsl:template match="cvg:IterateInstances" mode="ConstraintVerbalization">
		<xsl:param name="TopLevel" select="false()"/>
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="''"/>
		<xsl:param name="CompositeCount"/>
		<xsl:param name="CompositeIterator"/>
		<xsl:param name="ListStyle" select="@listStyle"/>
		<xsl:param name="PatternGroup"/>
		<xsl:variable name="useVariablePrefixFragment">
			<xsl:choose>
				<xsl:when test="$VariablePrefix">
					<xsl:value-of select="$VariablePrefix"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text>factText</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<plx:local dataTypeName=".boolean" name="isDeontic" const="true">
			<plx:initialize>
				<plx:falseKeyword/>
			</plx:initialize>
		</plx:local>
		<plx:local dataTypeName="StringBuilder" name="sbTemp">
			<plx:initialize>
				<plx:nullKeyword/>
			</plx:initialize>
		</plx:local>
		<plx:local dataTypeName=".i4" name="instanceCount">
			<plx:initialize>
				<plx:callInstance name="Count" type="property">
					<plx:callObject>
						<plx:callThis name="Instances" type="property"/>
					</plx:callObject>
				</plx:callInstance>
			</plx:initialize>
		</plx:local>
		<xsl:call-template name="InstanceIterator">
			<xsl:with-param name="TopLevel" select="$TopLevel"/>
			<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
			<xsl:with-param name="VariablePrefix" select="string($useVariablePrefixFragment)"/>
			<xsl:with-param name="CompositeCount" select="$CompositeCount"/>
			<xsl:with-param name="CompositeIterator" select="$CompositeIterator"/>
			<xsl:with-param name="ListStyle" select="$ListStyle"/>
			<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
		</xsl:call-template>
	</xsl:template>
	<xsl:template match="cvg:IterateFacts" mode="ConstraintVerbalization">
		<xsl:param name="TopLevel" select="false()"/>
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="''"/>
		<xsl:param name="CompositeCount"/>
		<xsl:param name="CompositeIterator"/>
		<xsl:param name="ListStyle" select="@listStyle"/>
		<xsl:param name="PatternGroup"/>
		<xsl:variable name="useVariablePrefixFragment">
			<xsl:choose>
				<xsl:when test="$VariablePrefix">
					<xsl:value-of select="$VariablePrefix"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text>factText</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:call-template name="RoleIterator">
			<xsl:with-param name="TopLevel" select="$TopLevel"/>
			<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
			<xsl:with-param name="VariablePrefix" select="string($useVariablePrefixFragment)"/>
			<xsl:with-param name="CompositeCount" select="$CompositeCount"/>
			<xsl:with-param name="CompositeIterator" select="$CompositeIterator"/>
			<xsl:with-param name="ListStyle" select="$ListStyle"/>
			<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
		</xsl:call-template>
	</xsl:template>
	<xsl:template match="cvg:IterateDerivedFrom" mode="ConstraintVerbalization">
		<xsl:param name="TopLevel" select="false()"/>
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="''"/>
		<xsl:param name="CompositeCount"/>
		<xsl:param name="CompositeIterator"/>
		<xsl:param name="ListStyle" select="@listStyle"/>
		<xsl:param name="PatternGroup"/>
		<xsl:param name="IteratorContext"/>
		<xsl:call-template name="EnsureTempStringBuilder"/>
		<plx:local name="derivedElements" dataTypeName="IList">
			<plx:passTypeParam dataTypeName="ORMModelElement"/>
			<plx:initialize>
				<plx:callThis name="DerivedElements" type="property"/>
			</plx:initialize>
		</plx:local>
		<plx:local name="derivedElementCount" dataTypeName=".i4">
			<plx:initialize>
				<plx:callInstance name="Count" type="property">
					<plx:callObject>
						<plx:nameRef name="derivedElements" type="local"/>
					</plx:callObject>
				</plx:callInstance>
			</plx:initialize>
		</plx:local>
		<plx:loop>
			<plx:initializeLoop>
				<plx:local name="derivedElementIndex" dataTypeName=".i4">
					<plx:initialize>
						<plx:value data="0" type="i4"/>
					</plx:initialize>
				</plx:local>
			</plx:initializeLoop>
			<plx:condition>
				<plx:binaryOperator type="lessThan">
					<plx:left>
						<plx:nameRef name="derivedElementIndex" type="local"/>
					</plx:left>
					<plx:right>
						<plx:nameRef name="derivedElementCount" type="local"/>
					</plx:right>
				</plx:binaryOperator>
			</plx:condition>
			<plx:beforeLoop>
				<plx:increment>
					<plx:nameRef name="derivedElementIndex" type="local"/>
				</plx:increment>
			</plx:beforeLoop>
			<plx:local name="derivedElement" dataTypeName="ORMModelElement">
				<plx:initialize>
					<plx:callInstance name=".implied" type="indexerCall">
						<plx:callObject>
							<plx:nameRef name="derivedElements"/>
						</plx:callObject>
						<plx:passParam>
							<plx:nameRef name="derivedElementIndex" type="local"/>
						</plx:passParam>
					</plx:callInstance>
				</plx:initialize>
			</plx:local>
			<xsl:call-template name="IterateRolesConstraintVerbalizationBody">
				<xsl:with-param name="iterVarName" select="'derivedElementIndex'"/>
				<xsl:with-param name="contextMatch" select="'derivedElementCount'"/>
				<xsl:with-param name="ListStyle" select="@listStyle"/>
				<xsl:with-param name="CompositeIterator" select="'derivedElementIndex'"/>
				<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
				<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
				<xsl:with-param name="PatternGroup" select="'DerivedFrom'"/>
			</xsl:call-template>
		</plx:loop>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="{$VariablePrefix}{$VariableDecorator}" type="local"/>
			</plx:left>
			<plx:right>
				<plx:callInstance name="ToString">
					<plx:callObject>
						<plx:nameRef name="sbTemp" type="local"/>
					</plx:callObject>
				</plx:callInstance>
			</plx:right>
		</plx:assign>
	</xsl:template>
	<xsl:template name="RoleIterator">
		<xsl:param name="TopLevel"/>
		<xsl:param name="VariableDecorator"/>
		<xsl:param name="VariablePrefix"/>
		<xsl:param name="CompositeCount"/>
		<xsl:param name="CompositeIterator"/>
		<xsl:param name="CompositeReplacementArray"/>
		<xsl:param name="ListStyle"/>
		<xsl:param name="PatternGroup"/>
		<xsl:param name="IteratorContext" select="''"/>
		<xsl:param name="SequenceIterator" select="false()"/>
		<xsl:param name="ParentListStyle" select="'null'"/>
		<!-- Other parameters should be forwarded to IterateRolesConstraintVerbalizationBody -->

		<!-- Normalize the match data -->
		<xsl:variable name="iterateFacts" select="boolean(self::cvg:IterateFacts)"/>
		<xsl:variable name="contextMatchFragment">
			<xsl:choose>
				<xsl:when test="$iterateFacts">
					<xsl:text>facts</xsl:text>
				</xsl:when>
				<xsl:when test="string-length(@match)">
					<xsl:value-of select="@match"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text>all</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="contextMatch" select="string($contextMatchFragment)"/>
		<xsl:variable name="providedFactTypeSkipIteration" select="$contextMatch='providedFactType'"/>
		<xsl:variable name="iterVarNameFragment">
			<xsl:choose>
				<xsl:when test="$iterateFacts">
					<xsl:value-of select="concat($FactIterVariablePart,$VariableDecorator)"/>
				</xsl:when>
				<xsl:when test="$IteratorContext">
					<xsl:value-of select="concat($ContextRoleIterVariablePart,$VariableDecorator)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="concat($RoleIterVariablePart,$VariableDecorator)"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="iterVarName" select="string($iterVarNameFragment)"/>
		<xsl:variable name="reverse" select="@reverse='true' or @reverse='1'"/>
		<xsl:variable name="explicitSubscript" select="string(@subscript)"/>
		<xsl:if test="$TopLevel">
			<xsl:choose>
				<xsl:when test="position()&gt;1">
					<plx:callInstance name="WriteLine">
						<plx:callObject>
							<plx:nameRef type="parameter" name="writer"/>
						</plx:callObject>
					</plx:callInstance>
				</xsl:when>
				<xsl:otherwise>
					<plx:callInstance name="BeginVerbalization">
						<plx:callObject>
							<plx:nameRef name="verbalizationContext" type="parameter"/>
						</plx:callObject>
						<plx:passParam>
							<plx:callStatic name="Normal" dataTypeName="VerbalizationContent" type="field"/>
						</plx:passParam>
					</plx:callInstance>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="0=string-length($CompositeCount) and not($IteratorContext) and not($providedFactTypeSkipIteration)">
			<xsl:call-template name="EnsureTempStringBuilder"/>
		</xsl:if>

		<!-- See if any filters are in place. If there are, then pre-walk the elements to
			 get a total count so we can build an accurate list during the main iterator -->
		<xsl:variable name="filterTestFragment">
			<xsl:variable name="filterOperatorsFragment">
				<xsl:apply-templates select="@*" mode="IterateRolesFilterOperator">
					<xsl:with-param name="IteratorVariableName" select="$iterVarName"/>
					<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
					<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
					<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
					<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
				</xsl:apply-templates>
			</xsl:variable>
			<xsl:variable name="filterOperators" select="exsl:node-set($filterOperatorsFragment)/child::*"/>
			<xsl:if test="$filterOperators">
				<xsl:call-template name="CombineElements">
					<xsl:with-param name="OperatorType" select="'booleanAnd'"/>
					<xsl:with-param name="Elements" select="$filterOperators"/>
				</xsl:call-template>
			</xsl:if>
		</xsl:variable>
		<xsl:variable name="filterTest" select="exsl:node-set($filterTestFragment)/child::*"/>
		<xsl:variable name="filteredCountVarName" select="concat($VariablePrefix,'FilteredCount',$VariableDecorator)"/>
		<xsl:variable name="filteredIterVarName" select="concat($VariablePrefix,'FilteredIter',$VariableDecorator)"/>
		<xsl:variable name="localFilteredFirstPass" select="@pass and $filterTest"/>
		<xsl:variable name="passDescendants" select="descendant::cvg:*[self::cvg:PredicateReplacement|self::cvg:ProvidedPredicateReplacement][@pass]"/>
		<xsl:variable name="trackFirstPass" select="boolean($passDescendants[@pass='first' or @pass='notFirst'])"/>
		<xsl:variable name="isNotFirst" select="boolean($passDescendants[@pass='notFirst'])"/>
		<xsl:variable name="trackFirstPassVarName" select="concat($VariablePrefix,'IsFirstPass',$VariableDecorator)"/>
		<xsl:variable name="resolvedListStyleFragment">
			<xsl:variable name="localListStyle" select="string(@listStyle)"/>
			<xsl:choose>
				<xsl:when test="$ListStyle='null' and $localListStyle">
					<xsl:value-of select="$localListStyle"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$ListStyle"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="resolvedListStyle" select="string($resolvedListStyleFragment)"/>
		<xsl:variable name="createList" select="not($resolvedListStyle='null')"/>
		<xsl:variable name="createListOrTrackFirstPass" select="$trackFirstPass or $createList"/>
		<xsl:variable name="hyphenBind" select="@hyphenBind='true' or @hyphenBind='1'"/>
		<xsl:variable name="loopReset" select="@loopReset"/>

		<xsl:if test="$contextMatch='preferredIdentifier'">
			<plx:local dataTypeName="LinkedElementCollection" name="includedRoles">
				<plx:passTypeParam dataTypeName="Role"/>
				<plx:initialize>
					<plx:nameRef name="preferredIdentifierRoles"/>
				</plx:initialize>
			</plx:local>
			<plx:local dataTypeName=".i4" name="constraintRoleArity">
				<plx:initialize>
					<plx:callInstance name="Count" type="property">
						<plx:callObject>
							<plx:nameRef name="includedRoles"/>
						</plx:callObject>
					</plx:callInstance>
				</plx:initialize>
			</plx:local>
		</xsl:if>
		<xsl:if test="$contextMatch='playedRoles'">
			<plx:local dataTypeName="LinkedElementCollection" name="playedRoles">
				<plx:passTypeParam dataTypeName="Role"/>
				<plx:initialize>
					<plx:callThis name="PlayedRoleCollection" type="property"/>
				</plx:initialize>
			</plx:local>
			<plx:local dataTypeName=".i4" name="playedRoleCount">
				<plx:initialize>
					<plx:callInstance name="Count" type="property">
						<plx:callObject>
							<plx:nameRef name="playedRoles"/>
						</plx:callObject>
					</plx:callInstance>
				</plx:initialize>
			</plx:local>
		</xsl:if>
		<xsl:if test="($trackFirstPass or $localFilteredFirstPass) and not($PatternGroup='SetComparisonConstraint')">
			<plx:local name="{$trackFirstPassVarName}" dataTypeName=".boolean">
				<plx:initialize>
					<plx:trueKeyword/>
				</plx:initialize>
			</plx:local>
		</xsl:if>
		<xsl:if test="$filterTest and not(string($CompositeCount))">
			<xsl:apply-templates select="." mode="IterateRolesFilterInitializeState">
				<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
				<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
				<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
				<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
			</xsl:apply-templates>
		</xsl:if>
		<xsl:if test="$filterTest and $createListOrTrackFirstPass">
			<xsl:if test="0=string-length($CompositeCount)">
				<plx:local name="{$filteredIterVarName}" dataTypeName=".i4"/>
				<xsl:if test="$createList">
					<plx:local name="{$filteredCountVarName}" dataTypeName=".i4">
						<plx:initialize>
							<plx:value type="i4" data="0"/>
						</plx:initialize>
					</plx:local>
					<xsl:apply-templates select="." mode="CompositeOrFilteredListCount">
						<xsl:with-param name="TotalCountCollectorVariable" select="$filteredCountVarName"/>
						<xsl:with-param name="IteratorVariableName" select="$filteredIterVarName"/>
						<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
						<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
						<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
						<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
					</xsl:apply-templates>
					<xsl:apply-templates select="." mode="IterateRolesFilterReinitializeState">
						<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
						<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
						<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
						<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
					</xsl:apply-templates>
				</xsl:if>
				<plx:assign>
					<plx:left>
						<plx:nameRef name="{$filteredIterVarName}"/>
					</plx:left>
					<plx:right>
						<plx:value type="i4" data="0"/>
					</plx:right>
				</plx:assign>
			</xsl:if>
		</xsl:if>
		<xsl:variable name="localCodeFragment">
			<xsl:if test="$contextMatch='constraintRoles' or $contextMatch='providedConstraintRoles' or $contextMatch='preferredIdentifier' or $contextMatch='playedRoles' or @conditionalReading[contains(.,'Primary')] or descendant::cvg:*[@match='primary' or @match='secondary' or @conditionMatch='RolePlayerHasReferenceScheme' or self::cvg:ReadingChoice[contains(@match,'Primary')]] or descendant::cvg:RoleName or descendant::cvg:IterateContextRoles or $IteratorContext">
				<xsl:variable name="primaryRoleInitializerFragment">
					<xsl:variable name="iteratorRoleFragment">
						<xsl:call-template name="ReferenceIteratorRole">
							<xsl:with-param name="ContextMatch" select="$contextMatch"/>
							<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
							<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
							<xsl:with-param name="IteratorIndex">
								<xsl:choose>
									<xsl:when test="$reverse">
										<plx:binaryOperator type="subtract">
											<plx:left>
												<plx:binaryOperator type="subtract">
													<plx:left>
														<xsl:call-template name="ReferenceIteratorBound">
															<xsl:with-param name="ContextMatch" select="$contextMatch"/>
															<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
															<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
														</xsl:call-template>
													</plx:left>
													<plx:right>
														<plx:nameRef name="{$iterVarName}"/>
													</plx:right>
												</plx:binaryOperator>
											</plx:left>
											<plx:right>
												<plx:value type="i4" data="1"/>
											</plx:right>
										</plx:binaryOperator>
									</xsl:when>
									<xsl:otherwise>
										<plx:nameRef name="{$iterVarName}"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:with-param>
						</xsl:call-template>
					</xsl:variable>
					<xsl:choose>
						<xsl:when test="$contextMatch='providedConstraintRoles'">
							<plx:callInstance name="Role" type="property">
								<plx:callObject>
									<xsl:copy-of select="$iteratorRoleFragment"/>
								</plx:callObject>
							</plx:callInstance>
						</xsl:when>
						<xsl:otherwise>
							<xsl:copy-of select="$iteratorRoleFragment"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:choose>
					<xsl:when test="$IteratorContext">
						<plx:assign>
							<plx:left>
								<plx:nameRef name="primaryRole"/>
							</plx:left>
							<plx:right>
								<xsl:copy-of select="$primaryRoleInitializerFragment"/>
							</plx:right>
						</plx:assign>
					</xsl:when>
					<xsl:otherwise>
						<plx:local name="primaryRole" dataTypeName="RoleBase">
							<plx:initialize>
								<xsl:copy-of select="$primaryRoleInitializerFragment"/>
							</plx:initialize>
						</plx:local>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:if>
			<xsl:choose>
				<xsl:when test="$contextMatch='preferredIdentifier' or $contextMatch='playedRoles' or $providedFactTypeSkipIteration">
					<xsl:variable name="subtypeMetaSemantics" select="boolean(descendant::cvg:Fact[@subtypeMetaReading='true' or @subtypeMetaReading='1'])"/>
					<plx:local name="parentFact" dataTypeName="FactType">
						<plx:initialize>
							<xsl:choose>
								<xsl:when test="$providedFactTypeSkipIteration">
									<xsl:choose>
										<xsl:when test="$PatternGroup='DerivedFrom'">
											<!-- The term 'parentFact' is hard coded all over. It is not worth the trouble
											of making this dynamic in the generator to save one variable assignment. -->
											<plx:nameRef name="derivedFactType"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:message terminate="yes">
												<xsl:text>UNKNOWN PROVIDED FACT TYPE:</xsl:text>
												<xsl:text> pattern group=</xsl:text>
												<xsl:value-of select="$PatternGroup"/>
												<xsl:text>, context match=</xsl:text>
												<xsl:value-of select="$contextMatch"/>
												<xsl:text>, iterator context=</xsl:text>
												<xsl:value-of select="$IteratorContext"/>
											</xsl:message>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<plx:callInstance name="FactType" type="property">
										<plx:callObject>
											<plx:nameRef name="primaryRole"/>
										</plx:callObject>
									</plx:callInstance>
								</xsl:otherwise>
							</xsl:choose>
						</plx:initialize>
					</plx:local>
					<xsl:if test="$subtypeMetaSemantics">
						<plx:local name="parentSubtypeFact" dataTypeName="SubtypeFact">
							<plx:initialize>
								<plx:cast dataTypeName="SubtypeFact" type="testCast">
									<plx:nameRef name="parentFact"/>
								</plx:cast>
							</plx:initialize>
						</plx:local>
					</xsl:if>
					<plx:local name="predicatePartFormatString" dataTypeName=".string">
						<plx:initialize>
							<xsl:call-template name="PopulatePredicatePartFormatString"/>
						</plx:initialize>
					</plx:local>
					<plx:local name="allReadingOrders" dataTypeName="LinkedElementCollection">
						<plx:passTypeParam dataTypeName="ReadingOrder"/>
						<plx:initialize>
							<plx:callInstance name="ReadingOrderCollection" type="property">
								<plx:callObject>
									<plx:nameRef name="parentFact"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:initialize>
					</plx:local>
					<plx:local name="factRoles" dataTypeName="IList">
						<plx:passTypeParam dataTypeName="RoleBase"/>
						<plx:initialize>
							<xsl:choose>
								<xsl:when test="$subtypeMetaSemantics">
									<xsl:call-template name="InitializeDefaultFactRoles">
										<xsl:with-param name="FallbackRoleCollection">
											<plx:inlineStatement dataTypeName="IList">
												<plx:passTypeParam dataTypeName="RoleBase"/>
												<plx:conditionalOperator>
													<plx:condition>
														<plx:binaryOperator type="identityEquality">
															<plx:left>
																<plx:nameRef name="parentSubtypeFact"/>
															</plx:left>
															<plx:right>
																<plx:nullKeyword/>
															</plx:right>
														</plx:binaryOperator>
													</plx:condition>
													<plx:left>
														<plx:callInstance name="RoleCollection" type="property">
															<plx:callObject>
																<plx:nameRef name="parentFact" type="local" />
															</plx:callObject>
														</plx:callInstance>
													</plx:left>
													<plx:right>
														<plx:cast dataTypeName="IList">
															<plx:passTypeParam dataTypeName="RoleBase"/>
															<plx:callNew dataTypeName="RoleBase" dataTypeIsSimpleArray="true">
																<plx:arrayInitializer>
																	<plx:callInstance name="SubtypeRole" type="property">
																		<plx:callObject>
																			<plx:nameRef name="parentSubtypeFact"/>
																		</plx:callObject>
																	</plx:callInstance>
																	<plx:callInstance name="SupertypeRole" type="property">
																		<plx:callObject>
																			<plx:nameRef name="parentSubtypeFact"/>
																		</plx:callObject>
																	</plx:callInstance>
																</plx:arrayInitializer>
															</plx:callNew>
														</plx:cast>
													</plx:right>
												</plx:conditionalOperator>
											</plx:inlineStatement>
										</xsl:with-param>
									</xsl:call-template>
								</xsl:when>
								<xsl:otherwise>
									<xsl:call-template name="InitializeDefaultFactRoles"/>
								</xsl:otherwise>
							</xsl:choose>
						</plx:initialize>
					</plx:local>
					<plx:local name="factArity" dataTypeName=".i4">
						<plx:initialize>
							<xsl:call-template name="InitializeFactArity"/>
						</plx:initialize>
					</plx:local>
					<plx:local name="reading" dataTypeName="IReading">
						<plx:initialize>
							<plx:nullKeyword/>
						</plx:initialize>
					</plx:local>
					<plx:local name="hyphenBinder" dataTypeName="VerbalizationHyphenBinder"/>
				</xsl:when>
				<xsl:when test="$contextMatch='constraintRoles' or $contextMatch='providedConstraintRoles' or $iterateFacts">
					<plx:assign>
						<plx:left>
							<plx:nameRef name="parentFact"/>
						</plx:left>
						<plx:right>
							<xsl:choose>
								<xsl:when test="$iterateFacts">
									<plx:callInstance name=".implied" type="indexerCall">
										<plx:callObject>
											<plx:nameRef name="allFacts"/>
										</plx:callObject>
										<plx:passParam>
											<plx:nameRef name="{$iterVarName}"/>
										</plx:passParam>
									</plx:callInstance>
								</xsl:when>
								<xsl:otherwise>
									<plx:callInstance name="FactType" type="property">
										<plx:callObject>
											<plx:nameRef name="primaryRole"/>
										</plx:callObject>
									</plx:callInstance>
								</xsl:otherwise>
							</xsl:choose>
						</plx:right>
					</plx:assign>
					<plx:assign>
						<plx:left>
							<plx:nameRef name="predicatePartFormatString"/>
						</plx:left>
						<plx:right>
							<xsl:call-template name="PopulatePredicatePartFormatString"/>
						</plx:right>
					</plx:assign>
					<plx:assign>
						<plx:left>
							<plx:nameRef name="allReadingOrders"/>
						</plx:left>
						<plx:right>
							<plx:callInstance name="ReadingOrderCollection" type="property">
								<plx:callObject>
									<plx:nameRef name="parentFact"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:right>
					</plx:assign>
					<plx:assign>
						<plx:left>
							<plx:nameRef name="factRoles"/>
						</plx:left>
						<plx:right>
							<xsl:call-template name="InitializeDefaultFactRoles"/>
						</plx:right>
					</plx:assign>
					<xsl:if test="$PatternGroup='SetComparisonConstraint' and self::cvg:IterateRoles and child::cvg:Fact">
						<plx:assign>
							<plx:left>
								<plx:nameRef name="factArity"/>
							</plx:left>
							<plx:right>
								<plx:callInstance name="Count" type="property">
									<plx:callObject>
										<plx:nameRef name="factRoles"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:right>
						</plx:assign>
					</xsl:if>
					<xsl:if test="not($PatternGroup='SetComparisonConstraint')">
						<plx:assign>
							<plx:left>
								<plx:nameRef name="factArity"/>
							</plx:left>
							<plx:right>
								<xsl:call-template name="InitializeFactArity"/>
							</plx:right>
						</plx:assign>
					</xsl:if>
					<plx:assign>
						<plx:left>
							<plx:nameRef name="contextBasicReplacementIndex"/>
						</plx:left>
						<plx:right>
							<xsl:choose>
								<xsl:when test="$iterateFacts">
									<plx:nameRef name="{$iterVarName}"/>
								</xsl:when>
								<xsl:otherwise>
									<plx:callInstance name="IndexOf">
										<plx:callObject>
											<plx:nameRef name="allFacts"/>
										</plx:callObject>
										<plx:passParam>
											<plx:nameRef name="parentFact"/>
										</plx:passParam>
									</plx:callInstance>
								</xsl:otherwise>
							</xsl:choose>
						</plx:right>
					</plx:assign>
					<plx:local name="basicRoleReplacements" dataTypeName=".string">
						<xsl:choose>
							<xsl:when test="ancestor::cvg:Constraint[1]/cvg:EnableSubscripts[@custom]">
								<plx:arrayDescriptor rank="2"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:attribute name="dataTypeIsSimpleArray">
									<xsl:text>true</xsl:text>
								</xsl:attribute>
							</xsl:otherwise>
						</xsl:choose>
						<plx:initialize>
							<plx:callInstance name=".implied" type="arrayIndexer">
								<plx:callObject>
									<plx:nameRef name="allBasicRoleReplacements"/>
								</plx:callObject>
								<plx:passParam>
									<plx:nameRef name="contextBasicReplacementIndex"/>
								</plx:passParam>
							</plx:callInstance>
						</plx:initialize>
					</plx:local>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="generatedCodeFragment">
			<xsl:if test="$contextMatch='preferredIdentifier' or $contextMatch='playedRoles' or $providedFactTypeSkipIteration">
				<xsl:call-template name="PopulateBasicRoleReplacements"/>
			</xsl:if>
			<xsl:choose>
				<xsl:when test="$filterTest">
					<xsl:apply-templates select="." mode="IterateRolesFilterPreOperator">
						<xsl:with-param name="IteratorVariableName" select="$iterVarName"/>
						<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
						<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
						<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
						<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
					</xsl:apply-templates>
					<plx:branch>
						<plx:condition>
							<xsl:copy-of select="$filterTest"/>
						</plx:condition>
						<xsl:apply-templates select="." mode="IterateRolesFilterAfterPass">
							<xsl:with-param name="IteratorVariableName" select="$iterVarName"/>
							<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
							<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
							<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
							<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
						</xsl:apply-templates>
						<xsl:variable name="passCompositeCount">
							<xsl:choose>
								<xsl:when test="string-length($CompositeCount)">
									<xsl:value-of select="$CompositeCount"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$filteredCountVarName"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:variable name="passCompositeIterator">
							<xsl:choose>
								<xsl:when test="string-length($CompositeIterator)">
									<xsl:value-of select="$CompositeIterator"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$filteredIterVarName"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:call-template name="IterateRolesConstraintVerbalizationBody">
							<xsl:with-param name="TopLevel" select="$TopLevel"/>
							<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
							<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
							<xsl:with-param name="CompositeCount" select="string($passCompositeCount)"/>
							<xsl:with-param name="CompositeIterator" select="string($passCompositeIterator)"/>
							<xsl:with-param name="CompositeReplacementArray" select="$CompositeReplacementArray"/>
							<xsl:with-param name="ListStyle" select="$resolvedListStyle"/>
							<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
							<xsl:with-param name="FirstPassVariable" select="$trackFirstPassVarName"/>
							<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
							<!-- Forwarded local parameters -->
							<xsl:with-param name="contextMatch" select="$contextMatch"/>
							<xsl:with-param name="iterVarName" select="$iterVarName"/>
							<xsl:with-param name="hyphenBind" select="$hyphenBind"/>
							<xsl:with-param name="byPassList" select="not($ParentListStyle='null')"/>
							<xsl:with-param name="explicitSubscript" select="$explicitSubscript"/>
						</xsl:call-template>
						<xsl:if test="$createListOrTrackFirstPass">
							<plx:increment>
								<plx:nameRef name="{$passCompositeIterator}"/>
							</plx:increment>
						</xsl:if>
						<xsl:if test="($trackFirstPass or $localFilteredFirstPass) and not($PatternGroup='SetComparisonConstraint')">
							<plx:assign>
								<plx:left>
									<plx:nameRef name="{$trackFirstPassVarName}"/>
								</plx:left>
								<plx:right>
									<plx:falseKeyword/>
								</plx:right>
							</plx:assign>
						</xsl:if>
						<xsl:call-template name="ApplyLoopReset">
							<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
						</xsl:call-template>
					</plx:branch>
				</xsl:when>
				<xsl:otherwise>
					<xsl:call-template name="IterateRolesConstraintVerbalizationBody">
						<xsl:with-param name="TopLevel" select="$TopLevel"/>
						<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
						<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
						<xsl:with-param name="CompositeCount" select="$CompositeCount"/>
						<xsl:with-param name="CompositeIterator" select="$CompositeIterator"/>
						<xsl:with-param name="CompositeReplacementArray" select="$CompositeReplacementArray"/>
						<xsl:with-param name="ListStyle" select="$resolvedListStyle"/>
						<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
						<xsl:with-param name="FirstPassVariable" select="$trackFirstPassVarName"/>
						<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
						<!-- Forwarded local parameters -->
						<xsl:with-param name="contextMatch" select="$contextMatch"/>
						<xsl:with-param name="iterVarName" select="$iterVarName"/>
						<xsl:with-param name="hyphenBind" select="$hyphenBind"/>
						<xsl:with-param name="byPassList" select="not($ParentListStyle='null')"/>
						<xsl:with-param name="explicitSubscript" select="$explicitSubscript"/>
					</xsl:call-template>
					<xsl:if test="($trackFirstPass or $localFilteredFirstPass) and not($PatternGroup='SetComparisonConstraint')">
						<plx:assign>
							<plx:left>
								<plx:nameRef name="{$trackFirstPassVarName}"/>
							</plx:left>
							<plx:right>
								<plx:falseKeyword/>
							</plx:right>
						</plx:assign>
					</xsl:if>
					<xsl:if test="$CompositeIterator">
						<plx:increment>
							<plx:nameRef name="{$CompositeIterator}"/>
						</plx:increment>
					</xsl:if>
					<xsl:call-template name="ApplyLoopReset">
						<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
					</xsl:call-template>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="$providedFactTypeSkipIteration">
				<xsl:call-template name="StripUnusedLocals">
					<xsl:with-param name="LocalCode" select="exsl:node-set($localCodeFragment)/child::*"/>
					<xsl:with-param name="ReferencingCode" select="exsl:node-set($generatedCodeFragment)/child::*"/>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<plx:loop>
					<plx:initializeLoop>
						<plx:local name="{$iterVarName}" dataTypeName=".i4">
							<plx:initialize>
								<xsl:choose>
									<xsl:when test="@pass='notFirst'">
										<plx:value type="i4" data="1"/>
									</xsl:when>
									<xsl:otherwise>
										<plx:value type="i4" data="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</plx:initialize>
						</plx:local>
					</plx:initializeLoop>
					<plx:condition>
						<xsl:variable name="boundConditionSnippet">
							<plx:binaryOperator type="lessThan">
								<plx:left>
									<plx:nameRef name="{$iterVarName}"/>
								</plx:left>
								<plx:right>
									<xsl:choose>
										<xsl:when test="@pass='first' and not($localFilteredFirstPass)">
											<plx:value type="i4" data="1"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:call-template name="ReferenceIteratorBound">
												<xsl:with-param name="ContextMatch" select="$contextMatch"/>
												<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
												<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
											</xsl:call-template>
										</xsl:otherwise>
									</xsl:choose>
								</plx:right>
							</plx:binaryOperator>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="$localFilteredFirstPass and @pass='first'">
								<plx:binaryOperator type="booleanAnd">
									<plx:left>
										<xsl:copy-of select="$boundConditionSnippet"/>
									</plx:left>
									<plx:right>
										<plx:nameRef name="{$trackFirstPassVarName}"/>
									</plx:right>
								</plx:binaryOperator>
							</xsl:when>
							<xsl:otherwise>
								<xsl:copy-of select="$boundConditionSnippet"/>
							</xsl:otherwise>
						</xsl:choose>
					</plx:condition>
					<plx:beforeLoop>
						<plx:increment>
							<plx:nameRef name="{$iterVarName}"/>
						</plx:increment>
					</plx:beforeLoop>
					<xsl:call-template name="StripUnusedLocals">
						<xsl:with-param name="LocalCode" select="exsl:node-set($localCodeFragment)/child::*"/>
						<xsl:with-param name="ReferencingCode" select="exsl:node-set($generatedCodeFragment)/child::*"/>
					</xsl:call-template>
				</plx:loop>
			</xsl:otherwise>
		</xsl:choose>
		<xsl:variable name="getListText">
			<plx:callInstance name="ToString">
				<plx:callObject>
					<plx:nameRef name="sbTemp"/>
				</plx:callObject>
				<xsl:if test="$IteratorContext">
					<plx:passParam>
						<plx:nameRef name="contextTempStringBuildLength"/>
					</plx:passParam>
					<plx:passParam>
						<plx:binaryOperator type="subtract">
							<plx:left>
								<plx:callInstance name="Length" type="property">
									<plx:callObject>
										<plx:nameRef name="sbTemp"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:left>
							<plx:right>
								<plx:nameRef name="contextTempStringBuildLength"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:passParam>
				</xsl:if>
			</plx:callInstance>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="$TopLevel">
				<plx:callStatic name="WriteVerbalizerSentence" dataTypeName="FactType">
					<plx:passParam>
						<plx:nameRef type="parameter" name="writer"/>
					</plx:passParam>
					<plx:passParam>
						<xsl:copy-of select="$getListText"/>
					</plx:passParam>
					<plx:passParam>
						<xsl:call-template name="SnippetFor">
							<xsl:with-param name="SnippetType" select="'CloseVerbalizationSentence'"/>
						</xsl:call-template>
					</plx:passParam>
				</plx:callStatic>
			</xsl:when>
			<xsl:when test="0=string-length($CompositeIterator) and not($providedFactTypeSkipIteration)">
				<plx:assign>
					<plx:left>
						<plx:nameRef name="{$VariablePrefix}{$VariableDecorator}"/>
					</plx:left>
					<plx:right>
						<xsl:copy-of select="$getListText"/>
					</plx:right>
				</plx:assign>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="ApplyLoopReset">
		<xsl:param name="PatternGroup"/>
		<xsl:variable name="loopResets" select="normalize-space(@loopReset)"/>
		<xsl:if test="$loopResets">
			<xsl:variable name="resetFragment">
				<xsl:call-template name="SplitList">
					<xsl:with-param name="ItemList" select="$loopResets"/>
				</xsl:call-template>
			</xsl:variable>
			<xsl:apply-templates select="exsl:node-set($resetFragment)/child::*" mode="LoopReset">
				<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
			</xsl:apply-templates>
		</xsl:if>
	</xsl:template>
	<xsl:template match="*[.='resetSubscripts']" mode="LoopReset">
		<plx:callInstance name="ResetSubscripts">
			<plx:callObject>
				<plx:nameRef name="subscripter"/>
			</plx:callObject>
			<plx:passParam>
				<plx:nameRef name="allBasicRoleReplacements"/>
			</plx:passParam>
		</plx:callInstance>
	</xsl:template>
	<xsl:template match="*" mode="LoopReset">
		<xsl:call-template name="TerminateForInvalidAttribute">
			<xsl:with-param name="MessageText">Unrecognized Loop Reset</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<xsl:template name="InstanceIterator">
		<xsl:param name="TopLevel"/>
		<xsl:param name="VariableDecorator"/>
		<xsl:param name="VariablePrefix"/>
		<xsl:param name="CompositeCount"/>
		<xsl:param name="CompositeIterator"/>
		<xsl:param name="ListStyle"/>
		<xsl:param name="PatternGroup"/>
		<xsl:param name="IteratorContext" select="''"/>
		<xsl:variable name="contextMatch" select="string('all')"/>
		<xsl:variable name="iterVarName" select="string('InstanceIter1')"/>
		<xsl:if test="$TopLevel">
			<xsl:choose>
				<xsl:when test="position()&gt;1">
					<plx:callInstance name="WriteLine">
						<plx:callObject>
							<plx:nameRef type="parameter" name="writer"/>
						</plx:callObject>
					</plx:callInstance>
				</xsl:when>
				<xsl:otherwise>
					<plx:callInstance name="BeginVerbalization">
						<plx:callObject>
							<plx:nameRef name="verbalizationContext" type="parameter"/>
						</plx:callObject>
						<plx:passParam>
							<plx:callStatic name="Normal" dataTypeName="VerbalizationContent" type="field"/>
						</plx:passParam>
					</plx:callInstance>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="0=string-length($CompositeCount) and not($IteratorContext)">
			<xsl:call-template name="EnsureTempStringBuilder"/>
		</xsl:if>
		<xsl:variable name="filteredCountVarName" select="concat($VariablePrefix,'FilteredCount',$VariableDecorator)"/>
		<xsl:variable name="filteredIterVarName" select="concat($VariablePrefix,'FilteredIter',$VariableDecorator)"/>
		<xsl:variable name="trackFirstPass" select="0!=count(descendant::cvg:*[self::cvg:PredicateReplacement|self::cvg:ProvidedPredicateReplacement][@pass='first'])"/>
		<xsl:variable name="trackFirstPassVarName" select="concat($VariablePrefix,'IsFirstPass',$VariableDecorator)"/>
		<xsl:variable name="createList" select="not($ListStyle='null')"/>
		<xsl:variable name="createListOrTrackFirstPass" select="$trackFirstPass or $createList"/>
		<xsl:variable name="hyphenBind" select="@hyphenBind='true' or @hyphenBind='1'"/>

		<xsl:if test="$trackFirstPass">
			<plx:local name="{$trackFirstPassVarName}" dataTypeName=".boolean">
				<plx:initialize>
					<plx:trueKeyword/>
				</plx:initialize>
			</plx:local>
		</xsl:if>
		<plx:loop>
			<plx:initializeLoop>
				<plx:local name="{$iterVarName}" dataTypeName=".i4">
					<plx:initialize>
						<plx:value type="i4" data="0"/>
					</plx:initialize>
				</plx:local>
			</plx:initializeLoop>
			<plx:condition>
				<plx:binaryOperator type="lessThan">
					<plx:left>
						<plx:nameRef name="{$iterVarName}"/>
					</plx:left>
					<plx:right>
						<plx:nameRef name="instanceCount" type="local"/>
					</plx:right>
				</plx:binaryOperator>
			</plx:condition>
			<plx:beforeLoop>
				<plx:increment>
					<plx:nameRef name="{$iterVarName}"/>
				</plx:increment>
			</plx:beforeLoop>
			<!-- Cannot use predefined pattern for instances - names conflict as well do the conditions -->
			<xsl:if test="$createList">
				<plx:local name="listSnippet" dataTypeName="{$VerbalizationTextSnippetType}"/>
			</xsl:if>
			<xsl:choose>
				<xsl:when test="not($createList)"/>
				<xsl:when test="@pass='first' and 0=string-length($CompositeCount)">
					<xsl:call-template name="SetSnippetVariable">
						<xsl:with-param name="SnippetType" select="concat($ListStyle,'Open')"/>
						<xsl:with-param name="VariableName" select="'listSnippet'"/>
					</xsl:call-template>
				</xsl:when>
				<xsl:otherwise>
					<xsl:call-template name="PopulateListSnippet">
						<xsl:with-param name="IteratorVariableName" select="$iterVarName"/>
						<xsl:with-param name="ListStyle" select="$ListStyle"/>
						<xsl:with-param name="IteratorBound">
							<plx:nameRef name="instanceCount" type="local"/>
						</xsl:with-param>
						<xsl:with-param name="CompositeIterator" select="$CompositeIterator"/>
					</xsl:call-template>
				</xsl:otherwise>
			</xsl:choose>
			<xsl:if test="$createList">
				<xsl:call-template name="AppendListSnippet"/>
			</xsl:if>

			<!-- Process the child contents for this role -->
			<xsl:variable name="children" select="child::*"/>
			<xsl:if test="$children[not(self::cvg:ReadingContext)]">
				<xsl:if test="$hyphenBind">
					<xsl:message terminate="yes">IterateInstances/@hyphenBind only supported if IterateInstances has no children</xsl:message>
				</xsl:if>
				<xsl:for-each select="$children">
					<!-- Let children assign directly to the normal replacement variable so
						 that we don't have to communicate down the stack that they should assign
						 directly to the temp string builder. -->
					<plx:callInstance name="Append">
						<plx:callObject>
							<plx:nameRef name="sbTemp"/>
						</plx:callObject>
						<plx:passParam>
							<plx:callStatic name="GetDisplayString" dataTypeName="ObjectTypeInstance">
								<plx:passParam>
									<plx:callInstance name=".implied" type="arrayIndexer">
										<plx:callObject>
											<plx:callThis name="Instances" type="property"/>
										</plx:callObject>
										<plx:passParam>
											<plx:nameRef name="{$iterVarName}" type="local" />
										</plx:passParam>
									</plx:callInstance>
								</plx:passParam>
								<plx:passParam>
									<plx:callThis name="ParentObject" type="property"/>
								</plx:passParam>
								<plx:passParam>
									<plx:falseKeyword/>
								</plx:passParam>
								<plx:passParam>
									<plx:callInstance name="FormatProvider" type="property">
										<plx:callObject>
											<plx:nameRef name="writer" type="local"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:passParam>
								<plx:passParam>
									<plx:callInstance name="GetSnippet">
										<plx:callObject>
											<plx:nameRef name="snippets"/>
										</plx:callObject>
										<plx:passParam>
											<plx:callStatic name="TextInstanceValue" dataTypeName="{$VerbalizationTextSnippetType}" type="field"/>
										</plx:passParam>
										<plx:passParam>
											<plx:nameRef name="isDeontic" type="local" />
										</plx:passParam>
										<plx:passParam>
											<plx:nameRef name="isNegative" type="local" />
										</plx:passParam>
									</plx:callInstance>
								</plx:passParam>
								<plx:passParam>
									<plx:callInstance name="GetSnippet">
										<plx:callObject>
											<plx:nameRef name="snippets"/>
										</plx:callObject>
										<plx:passParam>
											<plx:callStatic name="NonTextInstanceValue" dataTypeName="{$VerbalizationTextSnippetType}" type="field"/>
										</plx:passParam>
										<plx:passParam>
											<plx:nameRef name="isDeontic" type="local" />
										</plx:passParam>
										<plx:passParam>
											<plx:nameRef name="isNegative" type="local" />
										</plx:passParam>
									</plx:callInstance>
								</plx:passParam>
							</plx:callStatic>
						</plx:passParam>
					</plx:callInstance>
				</xsl:for-each>
			</xsl:if>
			<!-- Use the current snippets data to close the list -->
			<xsl:choose>
				<xsl:when test="not($createList)"/>
				<xsl:when test="@pass='first' and 0=string-length($CompositeCount)">
					<plx:callInstance name="Append">
						<plx:callObject>
							<plx:nameRef name="sbTemp"/>
						</plx:callObject>
						<plx:passParam>
							<xsl:call-template name="SnippetFor">
								<xsl:with-param name="SnippetType" select="concat($ListStyle,'Close')"/>
							</xsl:call-template>
						</plx:passParam>
					</plx:callInstance>
				</xsl:when>
				<xsl:otherwise>
					<xsl:call-template name="CloseList">
						<xsl:with-param name="IteratorVariableName" select="$iterVarName"/>
						<xsl:with-param name="ListStyle" select="$ListStyle"/>
						<xsl:with-param name="IteratorBound">
							<plx:nameRef name="instanceCount" type="local"/>
						</xsl:with-param>
						<xsl:with-param name="CompositeIterator" select="$CompositeIterator"/>
					</xsl:call-template>
				</xsl:otherwise>
			</xsl:choose>
			<!-- End inline template -->
			<xsl:if test="$trackFirstPass and not($PatternGroup='SetComparisonConstraint')">
				<plx:assign>
					<plx:left>
						<plx:nameRef name="{$trackFirstPassVarName}"/>
					</plx:left>
					<plx:right>
						<plx:falseKeyword/>
					</plx:right>
				</plx:assign>
			</xsl:if>
		</plx:loop>
		<xsl:variable name="getListText">
			<plx:callInstance name="ToString">
				<plx:callObject>
					<plx:nameRef name="sbTemp"/>
				</plx:callObject>
			</plx:callInstance>
		</xsl:variable>
		<plx:callInstance name="Write">
			<plx:callObject>
				<plx:nameRef name="writer"/>
			</plx:callObject>
			<plx:passParam>
				<xsl:copy-of select="$getListText"/>
			</plx:passParam>
		</plx:callInstance>
	</xsl:template>
	<xsl:template name="ReferenceIteratorBound">
		<xsl:param name="CompositeCount" select="''"/>
		<xsl:param name="ContextMatch"/>
		<xsl:param name="IteratorContext" select="''"/>
		<xsl:param name="PatternGroup"/>
		<xsl:variable name="useContextMatchFragment">
			<xsl:choose>
				<xsl:when test="$IteratorContext and ($ContextMatch='primary' or $ContextMatch='secondary')">
					<xsl:value-of select="$IteratorContext"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$ContextMatch"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="useContextMatch" select="string($useContextMatchFragment)"/>
		<plx:nameRef>
			<xsl:attribute name="name">
				<xsl:choose>
					<xsl:when test="$CompositeCount">
						<xsl:value-of select="$CompositeCount"/>
					</xsl:when>
					<xsl:when test="$useContextMatch='all' or $useContextMatch='providedFactRoles'">
						<xsl:text>factArity</xsl:text>
					</xsl:when>
					<xsl:when test="$useContextMatch='facts'">
						<xsl:text>allFactsCount</xsl:text>
					</xsl:when>
					<xsl:when test="$useContextMatch='included'">
						<xsl:choose>
							<xsl:when test="$PatternGroup='InternalSetConstraint'">
								<xsl:text>constraintRoleArity</xsl:text>
							</xsl:when>
							<xsl:when test="$PatternGroup='SetComparisonConstraint'">
								<xsl:text>factArity</xsl:text>
							</xsl:when>
							<xsl:otherwise>
								<xsl:text>includedArity</xsl:text>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:when test="($useContextMatch='constraintRoles' or $useContextMatch='providedConstraintRoles') and $PatternGroup='SetComparisonConstraint'">
						<xsl:choose>
							<xsl:when test="parent::cvg:IterateSequences | parent::cvg:SequenceJoinPath[parent::cvg:IterateSequences]">
								<xsl:text>columnArity</xsl:text>
							</xsl:when>
							<xsl:otherwise>
								<xsl:text>roleArity</xsl:text>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:when test="$useContextMatch='constraintRoles' or $useContextMatch='providedConstraintRoles'">
						<xsl:text>constraintRoleArity</xsl:text>
					</xsl:when>
					<xsl:when test="$useContextMatch='preProjectionKeys'">
						<xsl:text>preProjectionKeyCount</xsl:text>
					</xsl:when>
					<xsl:when test="$useContextMatch='excluded'">
						<xsl:text>factArity</xsl:text>
					</xsl:when>
					<xsl:when test="$useContextMatch='rangeCount'">
						<xsl:text>rangeCount</xsl:text>
					</xsl:when>
					<xsl:when test="$useContextMatch='queryParameterCount'">
						<xsl:text>queryParameterCount</xsl:text>
					</xsl:when>
					<xsl:when test="$useContextMatch='preferredIdentifier'">
						<xsl:text>constraintRoleArity</xsl:text>
					</xsl:when>
					<xsl:when test="$useContextMatch='playedRoles'">
						<xsl:text>playedRoleCount</xsl:text>
					</xsl:when>
					<xsl:when test="$useContextMatch='derivedElementCount'">
						<xsl:text>derivedElementCount</xsl:text>
					</xsl:when>
					<xsl:when test="$useContextMatch='pathNodeCount'">
						<xsl:text>pathNodeCount</xsl:text>
					</xsl:when>
					<xsl:when test="$useContextMatch='pathNodeLists'">
						<xsl:text>pathNodeListCount</xsl:text>
					</xsl:when>
					<xsl:otherwise>
						<xsl:message terminate="yes">
							<xsl:text>NO ITERATOR BOUND:</xsl:text>
							<xsl:text> pattern group=</xsl:text>
							<xsl:value-of select="$PatternGroup"/>
							<xsl:text>, context match=</xsl:text>
							<xsl:value-of select="$ContextMatch"/>
							<xsl:text>, iterator context=</xsl:text>
							<xsl:value-of select="$IteratorContext"/>
						</xsl:message>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
		</plx:nameRef>
	</xsl:template>
	<xsl:template name="ReferenceIteratorRole">
		<xsl:param name="ContextMatch"/>
		<xsl:param name="IteratorContext" select="''"/>
		<xsl:param name="PatternGroup"/>
		<xsl:param name="IteratorIndex"/>
		<xsl:variable name="useContextMatchFragment">
			<xsl:choose>
				<xsl:when test="$IteratorContext and ($ContextMatch='primary' or $ContextMatch='secondary')">
					<xsl:value-of select="$IteratorContext"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$ContextMatch"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="useContextMatch" select="string($useContextMatchFragment)"/>
		<xsl:variable name="iteratorSetReference">
			<plx:callInstance name=".implied" type="arrayIndexer">
				<plx:callObject>
					<plx:nameRef>
						<xsl:attribute name="name">
							<xsl:choose>
								<xsl:when test="$useContextMatch='all' or $useContextMatch='providedFactRoles'">
									<xsl:text>factRoles</xsl:text>
								</xsl:when>
								<xsl:when test="$useContextMatch='included'">
									<xsl:choose>
										<xsl:when test="$PatternGroup='InternalSetConstraint'">
											<xsl:text>allConstraintRoles</xsl:text>
										</xsl:when>
										<xsl:when test="$PatternGroup='SetComparisonConstraint'">
											<xsl:text>includedConstraintRoles</xsl:text>
										</xsl:when>
										<xsl:otherwise>
											<xsl:text>includedRoles</xsl:text>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:when test="$useContextMatch='constraintRoles'">
									<xsl:choose>
										<xsl:when test="$PatternGroup='SetComparisonConstraint'">
											<xsl:text>includedConstraintRoles</xsl:text>
										</xsl:when>
										<xsl:otherwise>
											<xsl:text>allConstraintRoles</xsl:text>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:when test="$useContextMatch='preProjectionKeys' and $PatternGroup='SetConstraint'">
									<xsl:text>preProjectionKeys</xsl:text>
								</xsl:when>
								<xsl:when test="$useContextMatch='providedConstraintRoles' and ($PatternGroup='SetComparisonConstraint' or $PatternGroup='SetConstraint')">
									<xsl:text>includedConstraintRoles</xsl:text>
								</xsl:when>
								<xsl:when test="$useContextMatch='excluded'">
									<xsl:text>factRoles</xsl:text>
								</xsl:when>
								<xsl:when test="$useContextMatch='preferredIdentifier'">
									<xsl:text>includedRoles</xsl:text>
								</xsl:when>
								<xsl:when test="$useContextMatch='playedRoles'">
									<xsl:text>playedRoles</xsl:text>
								</xsl:when>
								<xsl:otherwise>
									<xsl:message terminate="yes">
										<xsl:text>UNSUPPORTED ITERATOR SET:</xsl:text>
										<xsl:text> pattern group=</xsl:text>
										<xsl:value-of select="$PatternGroup"/>
										<xsl:text>, context match=</xsl:text>
										<xsl:value-of select="$ContextMatch"/>
										<xsl:text>, iterator context=</xsl:text>
										<xsl:value-of select="$IteratorContext"/>
									</xsl:message>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
					</plx:nameRef>
				</plx:callObject>
				<plx:passParam>
					<xsl:copy-of select="$IteratorIndex"/>
				</plx:passParam>
			</plx:callInstance>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="$PatternGroup='SetComparisonConstraint' and not($ContextMatch='providedConstraintRoles' or $ContextMatch='preProjectionKeys')">
				<plx:callInstance name="Role" type="property">
					<plx:callObject>
						<xsl:copy-of select="$iteratorSetReference"/>
					</plx:callObject>
				</plx:callInstance>
			</xsl:when>
			<xsl:otherwise>
				<xsl:copy-of select="$iteratorSetReference"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- A helper template to spit the body of an IterateRoles iteration
		 either inside a filter conditional or directly -->
	<xsl:template name="IterateRolesConstraintVerbalizationBody">
		<!-- Primary forward parameters -->
		<xsl:param name="TopLevel"/>
		<xsl:param name="VariableDecorator"/>
		<xsl:param name="VariablePrefix"/>
		<xsl:param name="CompositeCount"/>
		<xsl:param name="CompositeIterator"/>
		<xsl:param name="CompositeReplacementArray"/>
		<xsl:param name="ListStyle"/>
		<xsl:param name="PatternGroup"/>
		<xsl:param name="FirstPassVariable"/>
		<xsl:param name="IteratorContext"/>
		<!-- Forwarded local parameters -->
		<xsl:param name="contextMatch"/>
		<xsl:param name="iterVarName"/>
		<xsl:param name="hyphenBind" select="false()"/>
		<xsl:param name="byPassList" select="false()"/>
		<xsl:param name="explicitSubscript" select="''"/>
		<!-- Use the current snippets data to open the list -->
		<xsl:variable name="createList" select="not($ListStyle='null')"/>
		<xsl:if test="$createList">
			<xsl:if test="not($IteratorContext) or ancestor::cvg:IterateRoles[@listStyle='null']">
				<plx:local name="listSnippet" dataTypeName="{$VerbalizationTextSnippetType}"/>
			</xsl:if>
		</xsl:if>
		<xsl:choose>
			<xsl:when test="not($createList)"/>
			<xsl:when test="(@pass='first' or @pass='notFirst') and 0=string-length($CompositeCount)">
				<xsl:call-template name="SetSnippetVariable">
					<xsl:with-param name="SnippetType" select="concat($ListStyle,'Open')"/>
					<xsl:with-param name="VariableName" select="'listSnippet'"/>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="PopulateListSnippet">
					<xsl:with-param name="IteratorVariableName" select="$iterVarName"/>
					<xsl:with-param name="ListStyle" select="$ListStyle"/>
					<xsl:with-param name="IteratorBound">
						<xsl:call-template name="ReferenceIteratorBound">
							<xsl:with-param name="CompositeCount" select="$CompositeCount"/>
							<xsl:with-param name="ContextMatch" select="$contextMatch"/>
							<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
							<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
						</xsl:call-template>
					</xsl:with-param>
					<xsl:with-param name="CompositeIterator" select="$CompositeIterator"/>
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
		<xsl:if test="$createList">
			<xsl:call-template name="AppendListSnippet"/>
			<xsl:if test="$CompositeReplacementArray">
				<xsl:call-template name="AppendReplacementField">
					<xsl:with-param name="ReplacementIndexVariable" select="$CompositeIterator"/>
				</xsl:call-template>
			</xsl:if>
		</xsl:if>
		<!-- Process the child contents for this role -->
		<xsl:variable name="children" select="child::*"/>
		<xsl:choose>
			<xsl:when test="self::cvg:IterateFacts">
				<plx:assign>
					<plx:left>
						<plx:nameRef name="{$VariablePrefix}{$VariableDecorator}"/>
					</plx:left>
					<plx:right>
						<plx:nullKeyword/>
					</plx:right>
				</plx:assign>
				<xsl:call-template name="ProcessFact">
					<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
					<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
					<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
					<xsl:with-param name="FirstPassVariable" select="$FirstPassVariable"/>
					<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
					<xsl:with-param name="IteratorVariableName" select="$iterVarName"/>
				</xsl:call-template>
				<plx:callInstance name="Append">
					<plx:callObject>
						<plx:nameRef name="sbTemp"/>
					</plx:callObject>
					<plx:passParam>
						<plx:nameRef name="{$VariablePrefix}{$VariableDecorator}"/>
					</plx:passParam>
				</plx:callInstance>
			</xsl:when>
			<xsl:when test="$children[not(self::cvg:ReadingContext)]">
				<xsl:if test="$hyphenBind">
					<xsl:message terminate="yes">IterateRoles/@hyphenBind only supported if IterateRoles has no children</xsl:message>
				</xsl:if>
				<xsl:for-each select="$children">
					<!-- Let children assign directly to the normal replacement variable so
						 that we don't have to communicate down the stack that they should assign
						 directly to the temp string builder. -->
					<xsl:if test="$TopLevel">
						<plx:local name="{$VariablePrefix}{$VariableDecorator}" dataTypeName=".string">
							<plx:initialize>
								<plx:nullKeyword/>
							</plx:initialize>
						</plx:local>
					</xsl:if>
					<xsl:apply-templates select="." mode="ConstraintVerbalization">
						<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
						<!-- Pass the position in here or it will always be 1 -->
						<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
						<xsl:with-param name="IteratorContext" select="$contextMatch"/>
						<xsl:with-param name="IteratorVariableName" select="$iterVarName"/>
						<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
						<xsl:with-param name="FirstPassVariable" select="$FirstPassVariable"/>
						<xsl:with-param name="ContextMatch" select="$contextMatch"/>
						<xsl:with-param name="ListStyle" select="$ListStyle"/>
					</xsl:apply-templates>
					<xsl:if test="$byPassList and boolean(self::cvg:Fact)">
						<xsl:call-template name="AppendListSnippet"/>
					</xsl:if>
					<xsl:choose>
						<xsl:when test="$CompositeReplacementArray">
							<plx:assign>
								<plx:left>
									<plx:callInstance name=".implied" type="arrayIndexer">
										<plx:callObject>
											<plx:nameRef name="{$CompositeReplacementArray}"/>
										</plx:callObject>
										<plx:passParam>
											<plx:nameRef name="{$CompositeIterator}"/>
										</plx:passParam>
									</plx:callInstance>
								</plx:left>
								<plx:right>
									<plx:nameRef name="{$VariablePrefix}{$VariableDecorator}"/>
								</plx:right>
							</plx:assign>
						</xsl:when>
						<xsl:when test="not($contextMatch='providedFactType')">
							<plx:callInstance name="Append">
								<plx:callObject>
									<plx:nameRef name="sbTemp"/>
								</plx:callObject>
								<plx:passParam>
									<xsl:choose>
										<xsl:when test="$PatternGroup='DerivedFrom'">
											<plx:callStatic name="CreateVerbalizerSentence" dataTypeName="FactType">
												<plx:passParam>
													<plx:nameRef name="{$VariablePrefix}{$VariableDecorator}"/>
												</plx:passParam>
												<plx:passParam>
													<xsl:call-template name="SnippetFor">
														<xsl:with-param name="SnippetType" select="'CloseVerbalizationSentence'"/>
													</xsl:call-template>
												</plx:passParam>
											</plx:callStatic>
										</xsl:when>
										<xsl:otherwise>
											<plx:nameRef name="{$VariablePrefix}{$VariableDecorator}"/>
										</xsl:otherwise>
									</xsl:choose>
								</plx:passParam>
							</plx:callInstance>
						</xsl:when>
					</xsl:choose>
				</xsl:for-each>
			</xsl:when>
			<xsl:otherwise>
				<xsl:variable name="roleIndexExpression">
					<xsl:choose>
						<xsl:when test="@match='included' or @match='constraintRoles' or $IteratorContext='included' or $IteratorContext='constraintRoles'">
							<!-- The role index needs to be retrieved from the all roles list -->
							<xsl:variable name="innerRoleIndexExpression">
								<plx:callStatic name="IndexOfRole" dataTypeName="FactType">
									<plx:passParam>
										<plx:nameRef name="factRoles"/>
									</plx:passParam>
									<plx:passParam>
										<xsl:choose>
											<xsl:when test="@match='constraintRoles'">
												<plx:nameRef name="primaryRole"/>
											</xsl:when>
											<xsl:otherwise>
												<plx:callInstance name=".implied" type="arrayIndexer">
													<plx:callObject>
														<plx:nameRef name="includedRoles">
															<xsl:if test="@match='constraintRoles' or $PatternGroup='InternalSetConstraint'">
																<xsl:attribute name="name">
																	<xsl:choose>
																		<xsl:when test="$PatternGroup='SetComparisonConstraint'">
																			<xsl:message terminate="yes">Pattern no longer supported for SetComparisonConstraint, use RolePathProvider patterns.</xsl:message>
																			<xsl:text>includedSequenceRoles</xsl:text>
																		</xsl:when>
																		<xsl:otherwise>
																			<xsl:text>allConstraintRoles</xsl:text>
																		</xsl:otherwise>
																	</xsl:choose>
																</xsl:attribute>
															</xsl:if>
														</plx:nameRef>
													</plx:callObject>
													<plx:passParam>
														<plx:nameRef name="{$iterVarName}"/>
													</plx:passParam>
												</plx:callInstance>
											</xsl:otherwise>
										</xsl:choose>
									</plx:passParam>
								</plx:callStatic>
							</xsl:variable>
							<xsl:choose>
								<xsl:when test="$PatternGroup='InternalConstraint' or $PatternGroup='RoleValueConstraint' or $PatternGroup='UnaryRoleCardinalityConstraint'">
									<xsl:copy-of select="$innerRoleIndexExpression"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:copy-of select="$innerRoleIndexExpression"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:when>
						<!-- UNDONE: Support excluded match -->
						<xsl:otherwise>
							<plx:nameRef name="{$iterVarName}"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:variable name="roleIndexReference">
					<xsl:choose>
						<xsl:when test="not($hyphenBind) or not(@match='included' or @match='constraintRoles')">
							<xsl:copy-of select="$roleIndexExpression"/>
						</xsl:when>
						<xsl:otherwise>
							<plx:nameRef name="{$ResolvedRoleVariablePart}{$VariableDecorator}"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:variable name="readingContext" select="$children[self::cvg:ReadingContext]"/>
				<xsl:if test="$readingContext">
					<xsl:call-template name="PopulateReading">
						<xsl:with-param name="ReadingChoice" select="readingContext/@match"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if test="$hyphenBind and (@match='included' or @match='constraintRoles')">
					<plx:local name="{$ResolvedRoleVariablePart}{$VariableDecorator}" dataTypeName=".i4">
						<plx:initialize>
							<xsl:copy-of select="$roleIndexExpression"/>
						</plx:initialize>
					</plx:local>
				</xsl:if>
				<xsl:variable name="roleReplacementContents">
					<xsl:choose>
						<xsl:when test="$contextMatch='providedConstraintRoles' or $contextMatch='preProjectionKeys'">
							<plx:callInstance name="RenderAssociatedRolePlayer">
								<plx:callObject>
									<plx:nameRef name="pathVerbalizer"/>
								</plx:callObject>
								<plx:passParam>
									<xsl:call-template name="ReferenceIteratorRole">
										<xsl:with-param name="ContextMatch" select="$contextMatch"/>
										<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
										<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
										<xsl:with-param name="IteratorIndex" select="$roleIndexReference"/>
									</xsl:call-template>
								</plx:passParam>
								<plx:passParam>
									<xsl:choose>
										<xsl:when test="$hyphenBind">
											<plx:callInstance name="GetRoleFormatString">
												<plx:callObject>
													<plx:nameRef name="hyphenBinder"/>
												</plx:callObject>
												<plx:passParam>
													<plx:callStatic name="IndexOfRole" dataTypeName="FactType">
														<plx:passParam>
															<plx:nameRef name="factRoles"/>
														</plx:passParam>
														<plx:passParam>
															<plx:nameRef name="primaryRole"/>
														</plx:passParam>
													</plx:callStatic>
												</plx:passParam>
											</plx:callInstance>
										</xsl:when>
										<xsl:otherwise>
											<plx:nullKeyword/>
										</xsl:otherwise>
									</xsl:choose>
								</plx:passParam>
								<plx:passParam>
									<xsl:variable name="optionsFragment">
										<xsl:if test="@quantifyProvidedConstraintRoles='true' or @quantifyProvidedConstraintRoles='1'">
											<plx:callStatic dataTypeName="RolePathRolePlayerRenderingOptions" name="Quantify" type="field"/>
										</xsl:if>
										<xsl:if test="@markProvidedConstraintRolesAsHead='true' or @markProvidedConstraintRolesAsHead='1'">
											<plx:callStatic dataTypeName="RolePathRolePlayerRenderingOptions" name="UsedInVerbalizationHead" type="field"/>
											<xsl:if test="@minimizeProvidedConstraintRoleHeadSubscripting='true' or @minimizeProvidedConstraintRoleHeadSubscripting='1'">
												<plx:callStatic dataTypeName="RolePathRolePlayerRenderingOptions" name="MinimizeHeadSubscripting" type="field"/>
											</xsl:if>
										</xsl:if>
										<xsl:if test="@resolveProvidedConstraintRoleSupertype='true' or @resolveProvidedConstraintRoleSupertype='1'">
											<plx:callStatic dataTypeName="RolePathRolePlayerRenderingOptions" name="ResolveSupertype" type="field"/>
										</xsl:if>
									</xsl:variable>
									<xsl:variable name="options" select="exsl:node-set($optionsFragment)/child::*"/>
									<xsl:choose>
										<xsl:when test="$options">
											<xsl:call-template name="CombineElements">
												<xsl:with-param name="OperatorType" select="'bitwiseOr'"/>
												<xsl:with-param name="Elements" select="$options"/>
											</xsl:call-template>
										</xsl:when>
										<xsl:otherwise>
											<plx:callStatic dataTypeName="RolePathRolePlayerRenderingOptions" name="None" type="field"/>
										</xsl:otherwise>
									</xsl:choose>
								</plx:passParam>
							</plx:callInstance>
						</xsl:when>
						<xsl:otherwise>
							<xsl:variable name="replacementsFragment">
								<xsl:choose>
									<xsl:when test="$PatternGroup='InternalSetConstraint'">
										<plx:callInstance name=".implied" type="arrayIndexer">
											<plx:callObject>
												<plx:nameRef name="allBasicRoleReplacements"/>
											</plx:callObject>
											<plx:passParam>
												<plx:value data="0" type="i4"/>
											</plx:passParam>
										</plx:callInstance>
									</xsl:when>
									<xsl:otherwise>
										<plx:nameRef name="basicRoleReplacements"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<xsl:variable name="customSubscript" select="string(ancestor::cvg:Constraint[1]/cvg:EnableSubscripts/@custom[.='defaultPlain' or .='defaultSubscript'])"/>
							<xsl:choose>
								<xsl:when test="($customSubscript='defaultSubscript' and not($explicitSubscript='false' or $explicitSubscript='0')) or $explicitSubscript='true' or $explicitSubscript='1'">
									<plx:callInstance name="GetSubscriptedName">
										<plx:callObject>
											<plx:nameRef name="subscripter"/>
										</plx:callObject>
										<plx:passParam>
											<xsl:copy-of select="$roleIndexReference"/>
										</plx:passParam>
										<plx:passParam>
											<xsl:copy-of select="$replacementsFragment"/>
										</plx:passParam>
									</plx:callInstance>
								</xsl:when>
								<xsl:otherwise>
									<plx:callInstance name=".implied" type="arrayIndexer">
										<plx:callObject>
											<xsl:copy-of select="$replacementsFragment"/>
										</plx:callObject>
										<plx:passParam>
											<xsl:copy-of select="$roleIndexReference"/>
										</plx:passParam>
										<xsl:if test="$customSubscript">
											<plx:passParam>
												<plx:value data="0" type="i4"/>
											</plx:passParam>
										</xsl:if>
									</plx:callInstance>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<plx:callInstance name="Append">
					<plx:callObject>
						<plx:nameRef name="sbTemp"/>
					</plx:callObject>
					<plx:passParam>
						<xsl:choose>
							<xsl:when test="$hyphenBind and not($contextMatch='providedConstraintRoles' or $contextMatch='preProjectionKeys')">
								<plx:callInstance name="HyphenBindRoleReplacement">
									<plx:callObject>
										<plx:nameRef name="hyphenBinder"/>
									</plx:callObject>
									<plx:passParam>
										<xsl:copy-of select="$roleReplacementContents"/>
									</plx:passParam>
									<plx:passParam>
										<xsl:copy-of select="$roleIndexReference"/>
									</plx:passParam>
								</plx:callInstance>
							</xsl:when>
							<xsl:otherwise>
								<xsl:copy-of select="$roleReplacementContents"/>
							</xsl:otherwise>
						</xsl:choose>
					</plx:passParam>
				</plx:callInstance>
			</xsl:otherwise>
		</xsl:choose>

		<!-- Use the current snippets data to close the list -->
		<xsl:choose>
			<xsl:when test="not($createList)"/>
			<xsl:when test="(@pass='first' or @pass='notFirst') and 0=string-length($CompositeCount)">
				<plx:callInstance name="Append">
					<plx:callObject>
						<plx:nameRef name="sbTemp"/>
					</plx:callObject>
					<plx:passParam>
						<xsl:call-template name="SnippetFor">
							<xsl:with-param name="SnippetType" select="concat($ListStyle,'Close')"/>
						</xsl:call-template>
					</plx:passParam>
				</plx:callInstance>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="CloseList">
					<xsl:with-param name="IteratorVariableName" select="$iterVarName"/>
					<xsl:with-param name="ListStyle" select="$ListStyle"/>
					<xsl:with-param name="IteratorBound">
						<xsl:call-template name="ReferenceIteratorBound">
							<xsl:with-param name="CompositeCount" select="$CompositeCount"/>
							<xsl:with-param name="ContextMatch" select="$contextMatch"/>
							<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
							<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
						</xsl:call-template>
					</xsl:with-param>
					<xsl:with-param name="CompositeIterator" select="$CompositeIterator"/>
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- A helper template to populate the listSnippet variable. Called at the top of a
		 loop generating a list. -->
	<xsl:template name="PopulateListSnippet">
		<xsl:param name="IteratorVariableName"/>
		<xsl:param name="ListStyle"/>
		<xsl:param name="IteratorBound"/>
		<xsl:param name="ListSnippetVariable" select="'listSnippet'"/>
		<xsl:param name="CompositeIterator" select="''"/>
		<plx:branch>
			<plx:condition>
				<plx:binaryOperator type="equality">
					<plx:left>
						<plx:nameRef name="{$IteratorVariableName}">
							<xsl:if test="string-length($CompositeIterator)">
								<xsl:attribute name="name">
									<xsl:value-of select="$CompositeIterator"/>
								</xsl:attribute>
							</xsl:if>
						</plx:nameRef>
					</plx:left>
					<plx:right>
						<plx:value type="i4" data="0"/>
					</plx:right>
				</plx:binaryOperator>
			</plx:condition>
			<xsl:call-template name="SetSnippetVariable">
				<xsl:with-param name="SnippetType" select="concat($ListStyle,'Open')"/>
				<xsl:with-param name="VariableName" select="$ListSnippetVariable"/>
			</xsl:call-template>
		</plx:branch>
		<!-- UNDONE: We could spit less code here if we pass the arity
						 in from the ConstrainedRoles tag. -->
		<plx:alternateBranch>
			<plx:condition>
				<plx:binaryOperator type="equality">
					<plx:left>
						<plx:nameRef name="{$IteratorVariableName}">
							<xsl:if test="string-length($CompositeIterator)">
								<xsl:attribute name="name">
									<xsl:value-of select="$CompositeIterator"/>
								</xsl:attribute>
							</xsl:if>
						</plx:nameRef>
					</plx:left>
					<plx:right>
						<plx:binaryOperator type="subtract">
							<plx:left>
								<xsl:copy-of select="$IteratorBound"/>
							</plx:left>
							<plx:right>
								<plx:value type="i4" data="1"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:right>
				</plx:binaryOperator>
			</plx:condition>
			<plx:branch>
				<plx:condition>
					<plx:binaryOperator type="equality">
						<plx:left>
							<plx:nameRef name="{$IteratorVariableName}">
								<xsl:if test="string-length($CompositeIterator)">
									<xsl:attribute name="name">
										<xsl:value-of select="$CompositeIterator"/>
									</xsl:attribute>
								</xsl:if>
							</plx:nameRef>
						</plx:left>
						<plx:right>
							<plx:value type="i4" data="1"/>
						</plx:right>
					</plx:binaryOperator>
				</plx:condition>
				<xsl:call-template name="SetSnippetVariable">
					<xsl:with-param name="SnippetType" select="concat($ListStyle,'PairSeparator')"/>
					<xsl:with-param name="VariableName" select="$ListSnippetVariable"/>
				</xsl:call-template>
			</plx:branch>
			<plx:fallbackBranch>
				<xsl:call-template name="SetSnippetVariable">
					<xsl:with-param name="SnippetType" select="concat($ListStyle,'FinalSeparator')"/>
					<xsl:with-param name="VariableName" select="$ListSnippetVariable"/>
				</xsl:call-template>
			</plx:fallbackBranch>
		</plx:alternateBranch>
		<plx:fallbackBranch>
			<xsl:call-template name="SetSnippetVariable">
				<xsl:with-param name="SnippetType" select="concat($ListStyle,'Separator')"/>
				<xsl:with-param name="VariableName" select="$ListSnippetVariable"/>
			</xsl:call-template>
		</plx:fallbackBranch>
	</xsl:template>
	<!-- A helper template to append the list snippet variable to the temporary string builder.
		 Called after the PopulateListSnippet template. -->
	<xsl:template name="AppendListSnippet">
		<xsl:param name="ListSnippetVariable" select="'listSnippet'"/>
		<plx:callInstance name="Append">
			<plx:callObject>
				<plx:nameRef name="sbTemp"/>
			</plx:callObject>
			<plx:passParam>
				<xsl:call-template name="SnippetFor">
					<xsl:with-param name="VariableName" select="$ListSnippetVariable"/>
				</xsl:call-template>
			</plx:passParam>
		</plx:callInstance>
	</xsl:template>
	<xsl:template name="AppendReplacementField">
		<xsl:param name="ReplacementIndexVariable"/>
		<plx:callInstance name="AppendFormat">
			<plx:callObject>
				<plx:nameRef name="sbTemp"/>
			</plx:callObject>
			<plx:passParam>
				<plx:string>{{{0}}}</plx:string>
			</plx:passParam>
			<plx:passParam>
				<plx:nameRef name="{$ReplacementIndexVariable}"/>
			</plx:passParam>
		</plx:callInstance>
	</xsl:template>
	<!-- A helper template to close a list. Called at the end of a list-generating loop -->
	<xsl:template name="CloseList">
		<xsl:param name="IteratorVariableName"/>
		<xsl:param name="ListStyle"/>
		<xsl:param name="IteratorBound"/>
		<xsl:param name="CompositeIterator" select="''"/>
		<plx:branch>
			<plx:condition>
				<plx:binaryOperator type="equality">
					<plx:left>
						<plx:nameRef name="{$IteratorVariableName}">
							<xsl:if test="string-length($CompositeIterator)">
								<xsl:attribute name="name">
									<xsl:value-of select="$CompositeIterator"/>
								</xsl:attribute>
							</xsl:if>
						</plx:nameRef>
					</plx:left>
					<plx:right>
						<plx:binaryOperator type="subtract">
							<plx:left>
								<xsl:copy-of select="$IteratorBound"/>
							</plx:left>
							<plx:right>
								<plx:value type="i4" data="1"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:right>
				</plx:binaryOperator>
			</plx:condition>
			<plx:callInstance name="Append">
				<plx:callObject>
					<plx:nameRef name="sbTemp"/>
				</plx:callObject>
				<plx:passParam>
					<xsl:call-template name="SnippetFor">
						<xsl:with-param name="SnippetType" select="concat($ListStyle,'Close')"/>
					</xsl:call-template>
				</plx:passParam>
			</plx:callInstance>
		</plx:branch>
	</xsl:template>

	<!-- A CompositeList tag is used to combine one or more IterateRoles lists into
		 a single list. The listStyle parameter is ignored on IterateRoles if this is set. -->
	<xsl:template match="cvg:CompositeList" mode="ConstraintVerbalization">
		<xsl:param name="TopLevel" select="false()"/>
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="'list'"/>
		<xsl:param name="IteratorContext" select="''"/>
		<xsl:param name="PatternGroup"/>
		<xsl:if test="$TopLevel">
			<xsl:choose>
				<xsl:when test="position()&gt;1">
					<plx:callInstance name="WriteLine">
						<plx:callObject>
							<plx:nameRef type="parameter" name="writer"/>
						</plx:callObject>
					</plx:callInstance>
				</xsl:when>
				<xsl:otherwise>
					<plx:callInstance name="BeginVerbalization">
						<plx:callObject>
							<plx:nameRef name="verbalizationContext" type="parameter"/>
						</plx:callObject>
						<plx:passParam>
							<plx:callStatic name="Normal" dataTypeName="VerbalizationContent" type="field"/>
						</plx:passParam>
					</plx:callInstance>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="$IteratorContext">
			<xsl:call-template name="PushIteratorContext">
				<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
				<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:variable name="compositeCountVarName" select="concat($VariablePrefix,'CompositeCount',$VariableDecorator)"/>
		<plx:local name="{$compositeCountVarName}" dataTypeName=".i4">
			<plx:initialize>
				<plx:value type="i4" data="0"/>
			</plx:initialize>
		</plx:local>
		<xsl:variable name="iteratorVarName" select="concat($VariablePrefix,'CompositeIterator',$VariableDecorator)"/>
		<plx:local name="{$iteratorVarName}" dataTypeName=".i4"/>
		<xsl:for-each select="child::*">
			<xsl:variable name="decorator" select="string(position())"/>
			<xsl:apply-templates select="." mode="IterateRolesFilterInitializeState">
				<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
				<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
				<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
				<xsl:with-param name="VariableDecorator" select="$decorator"/>
			</xsl:apply-templates>
			<xsl:apply-templates select="." mode="CompositeOrFilteredListCount">
				<xsl:with-param name="TotalCountCollectorVariable" select="$compositeCountVarName"/>
				<xsl:with-param name="IteratorVariableName" select="$iteratorVarName"/>
				<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
				<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
				<xsl:with-param name="VariableDecorator" select="$decorator"/>
				<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
			</xsl:apply-templates>
			<xsl:apply-templates select="." mode="IterateRolesFilterReinitializeState">
				<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
				<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
				<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
				<xsl:with-param name="VariableDecorator" select="$decorator"/>
			</xsl:apply-templates>
		</xsl:for-each>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="{$iteratorVarName}"/>
			</plx:left>
			<plx:right>
				<plx:value type="i4" data="0"/>
			</plx:right>
		</plx:assign>
		<xsl:variable name="ListStyle" select="@listStyle"/>
		<xsl:if test="not($IteratorContext)">
			<xsl:call-template name="EnsureTempStringBuilder"/>
		</xsl:if>
		<xsl:for-each select="child::*">
			<xsl:if test="*">
				<plx:local name="{$VariablePrefix}{$VariableDecorator}Item{position()}" dataTypeName=".string">
					<plx:initialize>
						<plx:nullKeyword/>
					</plx:initialize>
				</plx:local>
			</xsl:if>
			<xsl:apply-templates select="." mode="ConstraintVerbalization">
				<xsl:with-param name="VariablePrefix" select="concat($VariablePrefix,$VariableDecorator,'Item')"/>
				<xsl:with-param name="VariableDecorator" select="position()"/>
				<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
				<xsl:with-param name="IteratorVariableName" select="$iteratorVarName"/>
				<xsl:with-param name="CompositeCount" select="$compositeCountVarName"/>
				<xsl:with-param name="CompositeIterator" select="$iteratorVarName"/>
				<xsl:with-param name="ListStyle" select="$ListStyle"/>
				<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
			</xsl:apply-templates>
		</xsl:for-each>
		<xsl:variable name="getListText">
			<plx:callInstance name="ToString">
				<plx:callObject>
					<plx:nameRef name="sbTemp"/>
				</plx:callObject>
				<xsl:if test="$IteratorContext">
					<plx:passParam>
						<plx:nameRef name="contextTempStringBuildLength"/>
					</plx:passParam>
					<plx:passParam>
						<plx:binaryOperator type="subtract">
							<plx:left>
								<plx:callInstance name="Length" type="property">
									<plx:callObject>
										<plx:nameRef name="sbTemp"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:left>
							<plx:right>
								<plx:nameRef name="contextTempStringBuildLength"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:passParam>
				</xsl:if>
			</plx:callInstance>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="$TopLevel">
				<!-- Write the snippet directly to the text writer after sentence modification -->
				<plx:callStatic name="WriteVerbalizerSentence" dataTypeName="FactType">
					<plx:passParam>
						<plx:nameRef type="parameter" name="writer"/>
					</plx:passParam>
					<plx:passParam>
						<xsl:copy-of select="$getListText"/>
					</plx:passParam>
					<plx:passParam>
						<xsl:call-template name="SnippetFor">
							<xsl:with-param name="SnippetType" select="'CloseVerbalizationSentence'"/>
						</xsl:call-template>
					</plx:passParam>
				</plx:callStatic>
			</xsl:when>
			<xsl:otherwise>
				<!-- Snippet is used as a replacement field in another snippet -->
				<plx:assign>
					<plx:left>
						<plx:nameRef name="{$VariablePrefix}{$VariableDecorator}"/>
					</plx:left>
					<plx:right>
						<xsl:copy-of select="$getListText"/>
					</plx:right>
				</plx:assign>
				<xsl:if test="$IteratorContext">
					<xsl:call-template name="PopIteratorContext">
						<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
						<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
					</xsl:call-template>
				</xsl:if>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- Handle the minFactArity IterateRoles filter attribute -->
	<xsl:template match="@minFactArity" mode="IterateRolesFilterOperator">
		<plx:binaryOperator type="greaterThanOrEqual">
			<plx:left>
				<plx:nameRef name="factArity"/>
			</plx:left>
			<plx:right>
				<plx:value type="i4" data="{.}"/>
			</plx:right>
		</plx:binaryOperator>
	</xsl:template>
	<!-- Handle the maxFactArity IterateRoles filter attribute -->
	<xsl:template match="@maxFactArity" mode="IterateRolesFilterOperator">
		<plx:binaryOperator type="lessThanOrEqual">
			<plx:left>
				<plx:nameRef name="factArity"/>
			</plx:left>
			<plx:right>
				<plx:value type="i4" data="{.}"/>
			</plx:right>
		</plx:binaryOperator>
	</xsl:template>
	<!-- Handle the factArity IterateRoles filter attribute -->
	<xsl:template match="@factArity" mode="IterateRolesFilterOperator">
		<plx:binaryOperator type="equality">
			<plx:left>
				<plx:nameRef name="factArity"/>
			</plx:left>
			<plx:right>
				<plx:value type="i4" data="{.}"/>
			</plx:right>
		</plx:binaryOperator>
	</xsl:template>
	<!-- Handle the uniqueFactType IterateRoles filter attribute -->
	<xsl:template match="@uniqueFactType" mode="IterateRolesFilterOperator">
		<xsl:param name="VariablePrefix"/>
		<xsl:param name="VariableDecorator"/>
		<xsl:if test=".='true' or .='1'">
			<plx:binaryOperator type="equality">
				<plx:left>
					<plx:callStatic name="IndexOf" dataTypeName="Array">
						<plx:passParam>
							<plx:nameRef name="{$VariablePrefix}UniqueFactTypes{$VariableDecorator}"/>
						</plx:passParam>
						<plx:passParam>
							<plx:inlineStatement dataTypeName="FactType">
								<plx:assign>
									<plx:left>
										<plx:nameRef name="{$VariablePrefix}TestUniqueFactType{$VariableDecorator}"/>
									</plx:left>
									<plx:right>
										<plx:callInstance name="FactType" type="property">
											<plx:callObject>
												<!-- UNDONE: primaryRole is not always available -->
												<plx:nameRef name="primaryRole"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:right>
								</plx:assign>
							</plx:inlineStatement>
						</plx:passParam>
					</plx:callStatic>
				</plx:left>
				<plx:right>
					<plx:value type="i4" data="-1"/>
				</plx:right>
			</plx:binaryOperator>
		</xsl:if>
	</xsl:template>
	<xsl:template match="@uniqueFactType" mode="IterateRolesFilterAfterPass">
		<xsl:param name="IteratorVariableName"/>
		<xsl:param name="VariablePrefix"/>
		<xsl:param name="VariableDecorator"/>
		<xsl:if test=".='true' or .='1'">
			<plx:assign>
				<plx:left>
					<plx:callInstance name=".implied" type="indexerCall">
						<plx:callObject>
							<plx:nameRef name="{$VariablePrefix}UniqueFactTypes{$VariableDecorator}"/>
						</plx:callObject>
						<plx:passParam>
							<plx:nameRef name="{$IteratorVariableName}"/>
						</plx:passParam>
					</plx:callInstance>
				</plx:left>
				<plx:right>
					<plx:nameRef name="{$VariablePrefix}TestUniqueFactType{$VariableDecorator}"/>
				</plx:right>
			</plx:assign>
		</xsl:if>
	</xsl:template>
	<xsl:template match="@uniqueFactType" mode="IterateRolesFilterInitializeState">
		<xsl:param name="PatternGroup"/>
		<xsl:param name="VariablePrefix"/>
		<xsl:param name="VariableDecorator"/>
		<xsl:if test=".='true' or .='1'">
			<xsl:variable name="contextMatchFragment">
				<xsl:choose>
					<xsl:when test="string(../@match)">
						<xsl:value-of select="../@match"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:text>all</xsl:text>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<plx:local name="{$VariablePrefix}UniqueFactTypes{$VariableDecorator}" dataTypeName="FactType" dataTypeIsSimpleArray="true">
				<plx:initialize>
					<plx:callNew dataTypeName="FactType" dataTypeIsSimpleArray="true">
						<plx:passParam>
							<xsl:choose>
								<xsl:when test="@pass='first'">
									<plx:value type="i4" data="1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:for-each select="..">
										<xsl:call-template name="ReferenceIteratorBound">
											<xsl:with-param name="ContextMatch" select="string($contextMatchFragment)"/>
											<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
										</xsl:call-template>
									</xsl:for-each>
								</xsl:otherwise>
							</xsl:choose>
						</plx:passParam>
					</plx:callNew>
				</plx:initialize>
			</plx:local>
			<plx:local name="{$VariablePrefix}TestUniqueFactType{$VariableDecorator}" dataTypeName="FactType"/>
		</xsl:if>
	</xsl:template>
	<xsl:template match="@uniqueFactType" mode="IterateRolesFilterReinitializeState">
		<xsl:param name="VariablePrefix"/>
		<xsl:param name="VariableDecorator"/>
		<xsl:if test=".='true' or .='1'">
			<xsl:variable name="varName" select="concat($VariablePrefix,'UniqueFactTypes',$VariableDecorator)"/>
			<plx:callStatic name="Clear" dataTypeName="Array">
				<plx:passParam>
					<plx:nameRef name="{$varName}"/>
				</plx:passParam>
				<plx:passParam>
					<plx:value type="i4" data="0"/>
				</plx:passParam>
				<plx:passParam>
					<plx:callInstance name="Length" type="property">
						<plx:callObject>
							<plx:nameRef name="{$varName}"/>
						</plx:callObject>
					</plx:callInstance>
				</plx:passParam>
			</plx:callStatic>
		</xsl:if>
	</xsl:template>
	<xsl:template match="@skipUnaryValueRole" mode="IterateRolesFilterOperator">
		<xsl:param name="VariablePrefix"/>
		<xsl:param name="VariableDecorator"/>
		<xsl:param name="PatternGroup"/>
		<xsl:param name="IteratorVariableName"/>
		<xsl:if test="not($PatternGroup='SetConstraint')">
			<xsl:call-template name="TerminateForInvalidAttribute">
				<xsl:with-param name="MessageText">supported only for SetConstraint pattern group</xsl:with-param>
			</xsl:call-template>
		</xsl:if>
		<plx:binaryOperator type="equality">
			<plx:left>
				<plx:callInstance name="UnaryPattern" type="property">
					<plx:callObject>
						<plx:callInstance name="FactType" type="property">
							<plx:callObject>
								<plx:callInstance name="Role" type="property">
									<plx:callObject>
										<plx:callInstance name=".implied" type="indexerCall">
											<plx:callObject>
												<plx:nameRef name="includedConstraintRoles"/>
											</plx:callObject>
											<plx:passParam>
												<plx:nameRef name="{$IteratorVariableName}"/>
											</plx:passParam>
										</plx:callInstance>
									</plx:callObject>
								</plx:callInstance>
							</plx:callObject>
						</plx:callInstance>
					</plx:callObject>
				</plx:callInstance>
			</plx:left>
			<plx:right>
				<plx:callStatic name="NotUnary" dataTypeName="UnaryValuePattern" type="field"/>
			</plx:right>
		</plx:binaryOperator>
	</xsl:template>
	<xsl:template match="@match" mode="IterateRolesFilterOperator">
		<xsl:param name="IteratorVariableName"/>
		<xsl:param name="PatternGroup"/>
		<xsl:param name="IteratorContext" select="''"/>
		<xsl:param name="VariablePrefix"/>
		<xsl:param name="VariableDecorator"/>
		<xsl:param name="FirstPassVariable"/>
		<xsl:variable name="matchValue" select="."/>
		<xsl:choose>
			<xsl:when test="$IteratorContext">
				<xsl:call-template name="PredicateReplacementConditionTest">
					<xsl:with-param name="Match" select="."/>
					<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
					<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
					<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
					<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
					<xsl:with-param name="FirstPassVariable" select="$FirstPassVariable"/>
					<xsl:with-param name="PrimaryRole" select="'contextPrimaryRole'"/>
					<xsl:with-param name="CurrentRole" select="'primaryRole'"/>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:choose>
					<xsl:when test="$matchValue='excluded'">
						<plx:binaryOperator type="equality">
							<plx:left>
								<plx:callInstance name="IndexOf">
									<plx:callObject>
										<xsl:choose>
											<xsl:when test="$PatternGroup='InternalSetConstraint'">
												<plx:nameRef name="allConstraintRoles"/>
											</xsl:when>
											<xsl:otherwise>
												<plx:nameRef name="includedRoles"/>
											</xsl:otherwise>
										</xsl:choose>
									</plx:callObject>
									<plx:passParam>
										<plx:callInstance name="Role" type="property">
											<plx:callObject>
												<plx:callInstance name=".implied" type="indexerCall">
													<plx:callObject>
														<plx:nameRef name="factRoles"/>
													</plx:callObject>
													<plx:passParam>
														<plx:nameRef name="{$IteratorVariableName}"/>
													</plx:passParam>
												</plx:callInstance>
											</plx:callObject>
										</plx:callInstance>
									</plx:passParam>
								</plx:callInstance>
							</plx:left>
							<plx:right>
								<plx:unaryOperator type="negative">
									<plx:value data="1" type="i4"/>
								</plx:unaryOperator>
							</plx:right>
						</plx:binaryOperator>
					</xsl:when>
				</xsl:choose>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="@verifyCanVerbalizeFactType" mode="IterateRolesFilterOperator">
		<xsl:if test=".='true' or .='1'">
			<plx:binaryOperator type="booleanAnd">
				<plx:left>
					<plx:binaryOperator type="identityEquality">
						<plx:left>
							<plx:callInstance name="VerbalizationBlockingReadingRequiredError" type="property">
								<plx:callObject>
									<plx:callInstance name="FactType" type="property">
										<plx:callObject>
											<plx:nameRef name="primaryRole"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:callObject>
							</plx:callInstance>
						</plx:left>
						<plx:right>
							<plx:nullKeyword/>
						</plx:right>
					</plx:binaryOperator>
				</plx:left>
				<plx:right>
					<plx:binaryOperator type="typeInequality">
						<plx:left>
							<plx:callInstance name="FactType" type="property">
								<plx:callObject>
									<plx:nameRef name="primaryRole"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:left>
						<plx:right>
							<plx:directTypeReference dataTypeName="Subquery"/>
						</plx:right>
					</plx:binaryOperator>
				</plx:right>
			</plx:binaryOperator>
		</xsl:if>
	</xsl:template>
	<xsl:template match="@conditionalReading" mode="IterateRolesFilterOperator">
		<xsl:param name="PatternGroup"/>
		<plx:binaryOperator type="identityInequality">
			<plx:left>
				<plx:inlineStatement dataTypeName="Reading">
					<xsl:call-template name="PopulateReading">
						<xsl:with-param name="ReadingChoice" select="."/>
						<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
						<xsl:with-param name="ReadingOnly" select="true()"/>
					</xsl:call-template>
				</plx:inlineStatement>
			</plx:left>
			<plx:right>
				<plx:nullKeyword/>
			</plx:right>
		</plx:binaryOperator>
	</xsl:template>
	<!-- Ignore attributes that are not used as a filter -->
	<xsl:template match="@listStyle|@pass|@conditionalMatch|@hyphenBind|@subscript|@loopReset|@reverse|@markProvidedConstraintRolesAsHead|@quantifyProvidedConstraintRoles|@minimizeProvidedConstraintRoleHeadSubscripting|@resolveProvidedConstraintRoleSupertype" mode="IterateRolesFilterOperator"/>
	<!-- Terminate processing if we see an unrecognized operator -->
	<xsl:template match="@*" mode="IterateRolesFilterOperator">
		<xsl:call-template name="TerminateForInvalidAttribute">
			<xsl:with-param name="MessageText">Unrecognized role iterator filter attribute</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<!-- Default filters have no extra processing -->
	<xsl:template match="*" mode="IterateRolesFilterInitializeState">
		<xsl:param name="PatternGroup"/>
		<xsl:param name="IteratorContext"/>
		<xsl:param name="VariablePrefix"/>
		<xsl:param name="VariableDecorator"/>
		<xsl:apply-templates select="@*" mode="IterateRolesFilterInitializeState">
			<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
			<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
			<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
			<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="*" mode="IterateRolesFilterReinitializeState">
		<xsl:param name="PatternGroup"/>
		<xsl:param name="IteratorContext"/>
		<xsl:param name="VariablePrefix"/>
		<xsl:param name="VariableDecorator"/>
		<xsl:apply-templates select="@*" mode="IterateRolesFilterReinitializeState">
			<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
			<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
			<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
			<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="*" mode="IterateRolesFilterPreOperator">
		<xsl:param name="IteratorVariableName"/>
		<xsl:param name="PatternGroup"/>
		<xsl:param name="IteratorContext"/>
		<xsl:param name="VariablePrefix"/>
		<xsl:param name="VariableDecorator"/>
		<xsl:apply-templates select="@*" mode="IterateRolesFilterPreOperator">
			<xsl:with-param name="IteratorVariableName" select="$IteratorVariableName"/>
			<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
			<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
			<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
			<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="*" mode="IterateRolesFilterAfterPass">
		<xsl:param name="IteratorVariableName"/>
		<xsl:param name="PatternGroup"/>
		<xsl:param name="IteratorContext"/>
		<xsl:param name="VariablePrefix"/>
		<xsl:param name="VariableDecorator"/>
		<xsl:apply-templates select="@*" mode="IterateRolesFilterAfterPass">
			<xsl:with-param name="IteratorVariableName" select="$IteratorVariableName"/>
			<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
			<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
			<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
			<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="@*" mode="IterateRolesFilterInitializeState"/>
	<xsl:template match="@*" mode="IterateRolesFilterReinitializeState"/>
	<xsl:template match="@*" mode="IterateRolesFilterPreOperator"/>
	<xsl:template match="@*" mode="IterateRolesFilterAfterPass"/>
	<xsl:template match="cvg:IterateRoles | cvg:IterateContextRoles | cvg:IterateFacts" mode="CompositeOrFilteredListCount">
		<xsl:param name="TotalCountCollectorVariable"/>
		<xsl:param name="IteratorVariableName"/>
		<xsl:param name="IteratorContext"/>
		<xsl:param name="VariablePrefix"/>
		<xsl:param name="VariableDecorator"/>
		<xsl:param name="PatternGroup"/>
		<xsl:variable name="contextMatchFragment">
			<xsl:choose>
				<xsl:when test="string-length(@match)">
					<xsl:value-of select="@match"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text>all</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="contextMatch" select="string($contextMatchFragment)"/>
		<xsl:variable name="filterTestFragment">
			<xsl:variable name="filterOperatorsFragment">
				<xsl:apply-templates select="@*" mode="IterateRolesFilterOperator">
					<xsl:with-param name="IteratorVariableName" select="$IteratorVariableName"/>
					<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
					<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
					<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
					<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
				</xsl:apply-templates>
			</xsl:variable>
			<xsl:variable name="filterOperators" select="exsl:node-set($filterOperatorsFragment)/child::*"/>
			<xsl:if test="$filterOperators">
				<xsl:call-template name="CombineElements">
					<xsl:with-param name="OperatorType" select="'booleanAnd'"/>
					<xsl:with-param name="Elements" select="$filterOperators"/>
				</xsl:call-template>
			</xsl:if>
		</xsl:variable>
		<xsl:variable name="filterTest" select="exsl:node-set($filterTestFragment)/child::*"/>

		<!-- Get the count for the set in a value, which will be used either to
			 increment the full count or as a filter upper bound -->
		<xsl:variable name="setCountValueFragment">
			<xsl:choose>
				<xsl:when test="@pass='first'">
					<plx:value type="i4" data="1"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:call-template name="ReferenceIteratorBound">
						<xsl:with-param name="ContextMatch" select="$contextMatch"/>
						<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
						<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
					</xsl:call-template>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="setCountValue" select="exsl:node-set($setCountValueFragment)/child::*"/>
		<xsl:choose>
			<xsl:when test="$filterTest">
				<plx:loop>
					<plx:initializeLoop>
						<plx:assign>
							<plx:left>
								<plx:nameRef name="{$IteratorVariableName}"/>
							</plx:left>
							<plx:right>
								<plx:value type="i4" data="0"/>
							</plx:right>
						</plx:assign>
					</plx:initializeLoop>
					<plx:condition>
						<plx:binaryOperator type="lessThan">
							<plx:left>
								<plx:nameRef name="{$IteratorVariableName}"/>
							</plx:left>
							<plx:right>
								<xsl:copy-of select="$setCountValue"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:beforeLoop>
						<plx:increment>
							<plx:nameRef name="{$IteratorVariableName}"/>
						</plx:increment>
					</plx:beforeLoop>
					<xsl:variable name="localCodeFragment">
						<xsl:variable name="primaryRoleInitializerFragment">
							<xsl:call-template name="ReferenceIteratorRole">
								<xsl:with-param name="ContextMatch" select="$contextMatch"/>
								<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
								<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
								<xsl:with-param name="IteratorIndex">
									<plx:nameRef name="{$IteratorVariableName}"/>
								</xsl:with-param>
							</xsl:call-template>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="$IteratorContext">
								<plx:assign>
									<plx:left>
										<plx:nameRef name="primaryRole"/>
									</plx:left>
									<plx:right>
										<xsl:copy-of select="$primaryRoleInitializerFragment"/>
									</plx:right>
								</plx:assign>
							</xsl:when>
							<xsl:otherwise>
								<plx:local name="primaryRole" dataTypeName="RoleBase">
									<plx:initialize>
										<xsl:copy-of select="$primaryRoleInitializerFragment"/>
									</plx:initialize>
								</plx:local>
							</xsl:otherwise>
						</xsl:choose>
						<xsl:if test="$contextMatch='constraintRoles' or $contextMatch='preferredIdentifier'">
							<plx:assign>
								<plx:left>
									<plx:nameRef name="parentFact"/>
								</plx:left>
								<plx:right>
									<plx:callInstance name="FactType" type="property">
										<plx:callObject>
											<plx:nameRef name="primaryRole"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:right>
							</plx:assign>
							<plx:assign>
								<plx:left>
									<plx:nameRef name="predicatePartFormatString"/>
								</plx:left>
								<plx:right>
									<xsl:call-template name="PopulatePredicatePartFormatString"/>
								</plx:right>
							</plx:assign>
							<plx:assign>
								<plx:left>
									<plx:nameRef name="allReadingOrders"/>
								</plx:left>
								<plx:right>
									<plx:callInstance name="ReadingOrderCollection" type="property">
										<plx:callObject>
											<plx:nameRef name="parentFact"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:right>
							</plx:assign>
							<plx:assign>
								<plx:left>
									<plx:nameRef name="factRoles"/>
								</plx:left>
								<plx:right>
									<xsl:call-template name="InitializeDefaultFactRoles"/>
								</plx:right>
							</plx:assign>
							<plx:assign>
								<plx:left>
									<plx:nameRef name="factArity"/>
								</plx:left>
								<plx:right>
									<xsl:call-template name="InitializeFactArity"/>
								</plx:right>
							</plx:assign>
						</xsl:if>
					</xsl:variable>
					<xsl:variable name="generatedCodeFragment">
						<xsl:apply-templates select="." mode="IterateRolesFilterPreOperator">
							<xsl:with-param name="IteratorVariableName" select="$IteratorVariableName"/>
							<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
							<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
							<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
							<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
						</xsl:apply-templates>
						<plx:branch>
							<plx:condition>
								<xsl:copy-of select="$filterTest"/>
							</plx:condition>
							<xsl:apply-templates select="." mode="IterateRolesFilterAfterPass">
								<xsl:with-param name="IteratorVariableName" select="$IteratorVariableName"/>
								<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
								<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
								<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
								<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
							</xsl:apply-templates>
							<plx:increment>
								<plx:nameRef name="{$TotalCountCollectorVariable}"/>
							</plx:increment>
						</plx:branch>
					</xsl:variable>
					<xsl:call-template name="StripUnusedLocals">
						<xsl:with-param name="LocalCode" select="exsl:node-set($localCodeFragment)/child::*"/>
						<xsl:with-param name="ReferencingCode" select="exsl:node-set($generatedCodeFragment)/child::*"/>
					</xsl:call-template>
				</plx:loop>
			</xsl:when>
			<xsl:otherwise>
				<!-- No filter is in place, just use the total count for the matching set -->
				<plx:assign>
					<plx:left>
						<plx:nameRef name="{$TotalCountCollectorVariable}"/>
					</plx:left>
					<plx:right>
						<plx:binaryOperator type="add">
							<plx:left>
								<plx:nameRef name="{$TotalCountCollectorVariable}"/>
							</plx:left>
							<plx:right>
								<xsl:copy-of select="$setCountValue"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:right>
				</plx:assign>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="cvg:SequenceJoinPath" mode="CompositeOrFilteredListCount">
		<xsl:param name="TotalCountCollectorVariable"/>
		<xsl:param name="IteratorVariableName"/>
		<xsl:param name="IteratorContext"/>
		<xsl:param name="VariablePrefix"/>
		<xsl:param name="VariableDecorator"/>
		<xsl:param name="PatternGroup"/>
		<plx:branch>
			<plx:condition>
				<plx:callInstance name="HasPathVerbalization">
					<plx:callObject>
						<plx:nameRef name="pathVerbalizer"/>
					</plx:callObject>
					<plx:passParam>
						<plx:nameRef name="joinPath"/>
					</plx:passParam>
				</plx:callInstance>
			</plx:condition>
			<plx:increment>
				<plx:nameRef name="{$TotalCountCollectorVariable}"/>
			</plx:increment>
		</plx:branch>
		<xsl:variable name="fallbackFragment">
			<xsl:apply-templates select="child::*" mode="CompositeOrFilteredListCount">
				<xsl:with-param name="TotalCountCollectorVariable" select="$TotalCountCollectorVariable"/>
				<xsl:with-param name="IteratorVariableName" select="$IteratorVariableName"/>
				<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
				<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
				<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
				<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
			</xsl:apply-templates>
		</xsl:variable>
		<xsl:variable name="fallback" select="exsl:node-set($fallbackFragment)/child::*"/>
		<xsl:if test="$fallback">
			<plx:fallbackBranch>
				<xsl:copy-of select="$fallback"/>
			</plx:fallbackBranch>
		</xsl:if>
	</xsl:template>
	<xsl:template match="cvg:SequenceJoinPath" mode="IterateRolesFilterInitializeState">
		<xsl:param name="PatternGroup"/>
		<xsl:param name="IteratorContext"/>
		<xsl:param name="VariablePrefix"/>
		<xsl:param name="VariableDecorator"/>
		<xsl:for-each select="*">
			<xsl:apply-templates select="@*" mode="IterateRolesFilterInitializeState">
				<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
				<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
				<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
				<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
			</xsl:apply-templates>
		</xsl:for-each>
	</xsl:template>
	<xsl:template match="cvg:SequenceJoinPath" mode="IterateRolesFilterReinitializeState">
		<xsl:param name="PatternGroup"/>
		<xsl:param name="IteratorContext"/>
		<xsl:param name="VariablePrefix"/>
		<xsl:param name="VariableDecorator"/>
		<xsl:for-each select="*">
			<xsl:apply-templates select="@*" mode="IterateRolesFilterReinitializeState">
				<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
				<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
				<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
				<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
			</xsl:apply-templates>
		</xsl:for-each>
	</xsl:template>
	<xsl:template match="cvg:SequenceJoinPath" mode="IterateRolesFilterAfterPass">
		<xsl:param name="IteratorVariableName"/>
		<xsl:param name="PatternGroup"/>
		<xsl:param name="IteratorContext"/>
		<xsl:param name="VariablePrefix"/>
		<xsl:param name="VariableDecorator"/>
		<xsl:for-each select="*">
			<xsl:apply-templates select="@*" mode="IterateRolesFilterAfterPass">
				<xsl:with-param name="IteratorVariableName" select="$IteratorVariableName"/>
				<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
				<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
				<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
				<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
			</xsl:apply-templates>
		</xsl:for-each>
	</xsl:template>
	<!-- Get the snippet value from the current snippets set.
		 This assumes snippets, isDeontic and isNegative local
		 variables are defined. Alternately, a VariableName
		 containing the name of a local variable containing the
		 text can be passed in instead of SnippetType. -->
	<xsl:template name="SnippetFor">
		<xsl:param name="SnippetType"/>
		<xsl:param name="VariableName"/>
		<xsl:param name="IsDeonticSnippet">
			<plx:nameRef name="isDeontic"/>
		</xsl:param>
		<xsl:param name="IsNegativeSnippet">
			<plx:nameRef name="isNegative"/>
		</xsl:param>
		<xsl:param name="AlternateSign" select="string(@alternateSign)"/>
		<xsl:choose>
			<xsl:when test="$SnippetType='null'">
				<plx:string/>
			</xsl:when>
			<xsl:otherwise>
				<plx:callInstance name="GetSnippet">
					<plx:callObject>
						<plx:nameRef name="snippets"/>
					</plx:callObject>
					<plx:passParam>
						<xsl:choose>
							<xsl:when test="string-length($VariableName)">
								<plx:nameRef name="{$VariableName}"/>
							</xsl:when>
							<xsl:otherwise>
								<plx:callStatic name="{$SnippetType}" dataTypeName="{$VerbalizationTextSnippetType}" type="field"/>
							</xsl:otherwise>
						</xsl:choose>
					</plx:passParam>
					<plx:passParam>
						<xsl:copy-of select="$IsDeonticSnippet"/>
					</plx:passParam>
					<plx:passParam>
						<xsl:choose>
							<xsl:when test="$AlternateSign">
								<xsl:choose>
									<xsl:when test="$AlternateSign='opposite'">
										<plx:unaryOperator type="booleanNot">
											<xsl:copy-of select="$IsNegativeSnippet"/>
										</plx:unaryOperator>
									</xsl:when>
									<xsl:when test="$AlternateSign='positive'">
										<plx:falseKeyword/>
									</xsl:when>
									<xsl:when test="$AlternateSign='negative'">
										<plx:trueKeyword/>
									</xsl:when>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:copy-of select="$IsNegativeSnippet"/>
							</xsl:otherwise>
						</xsl:choose>
					</plx:passParam>
				</plx:callInstance>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- Assign the specified snippet type to a local variable. -->
	<xsl:template name="SetSnippetVariable">
		<xsl:param name="SnippetType"/>
		<xsl:param name="VariableName"/>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="{$VariableName}"/>
			</plx:left>
			<plx:right>
				<plx:callStatic name="{$SnippetType}" dataTypeName="{$VerbalizationTextSnippetType}" type="field"/>
			</plx:right>
		</plx:assign>
	</xsl:template>
	<!-- Assign an invalid snippet type (value of -1) to a variable. -->
	<xsl:template name="SetSnippetVariableInvalid">
		<xsl:param name="VariableName"/>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="{$VariableName}"/>
			</plx:left>
			<plx:right>
				<plx:cast dataTypeName="{$VerbalizationTextSnippetType}">
					<!-- UNDONE: Hack, PLiX needs to put parentheses around the negative value for a cast -->
					<plx:binaryOperator type="subtract">
						<plx:left>
							<plx:value data="0" type="i4"/>
						</plx:left>
						<plx:right>
							<plx:value data="1" type="i4"/>
						</plx:right>
					</plx:binaryOperator>
				</plx:cast>
			</plx:right>
		</plx:assign>
	</xsl:template>
	<!-- Helper function to create an initialized string builder in the sbTemp local variable -->
	<xsl:template name="EnsureTempStringBuilder">
		<plx:branch>
			<plx:condition>
				<plx:binaryOperator type="identityEquality">
					<plx:left>
						<plx:nameRef name="sbTemp"/>
					</plx:left>
					<plx:right>
						<plx:nullKeyword/>
					</plx:right>
				</plx:binaryOperator>
			</plx:condition>
			<plx:assign>
				<plx:left>
					<plx:nameRef name="sbTemp"/>
				</plx:left>
				<plx:right>
					<plx:callNew dataTypeName="StringBuilder"/>
				</plx:right>
			</plx:assign>
		</plx:branch>
		<plx:fallbackBranch>
			<plx:assign>
				<plx:left>
					<plx:callInstance name="Length" type="property">
						<plx:callObject>
							<plx:nameRef name="sbTemp"/>
						</plx:callObject>
					</plx:callInstance>
				</plx:left>
				<plx:right>
					<plx:value type="i4" data="0"/>
				</plx:right>
			</plx:assign>
		</plx:fallbackBranch>
	</xsl:template>
	<xsl:template name="PopulatePredicatePartFormatString">
		<plx:callStatic name="Format" dataTypeName=".string">
			<plx:passParam>
				<plx:callInstance name="FormatProvider" type="property">
					<plx:callObject>
						<plx:nameRef type="parameter" name="writer"/>
					</plx:callObject>
				</plx:callInstance>
			</plx:passParam>
			<plx:passParam>
				<xsl:call-template name="SnippetFor">
					<xsl:with-param name="SnippetType" select="'PredicatePart'"/>
				</xsl:call-template>
			</plx:passParam>
			<plx:passParam>
				<plx:callInstance name="Name" type="property">
					<plx:callObject>
						<plx:nameRef name="parentFact"/>
					</plx:callObject>
				</plx:callInstance>
			</plx:passParam>
			<plx:passParam>
				<plx:callInstance name="ToString">
					<plx:callObject>
						<plx:callInstance name="Id" type="property">
							<plx:callObject>
								<plx:nameRef name="parentFact"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:callObject>
					<plx:passParam>
						<plx:string data="D"/>
					</plx:passParam>
				</plx:callInstance>
			</plx:passParam>
		</plx:callStatic>
	</xsl:template>
	<xsl:template name="PopulateReading">
		<!-- Support readings for {Context, {Prefer|Require}[Non][Primary]LeadReading[InfixTextOnly|[NoFrontText|NoTrailingText]], null} ReadingChoice values -->
		<xsl:param name="ReadingChoice"/>
		<xsl:param name="PatternGroup"/>
		<xsl:param name="ConditionalLoop" select="boolean(self::cvg:ReadingChoice)"/>
		<xsl:param name="ConditionalReadingOrderIndex"/>
		<xsl:param name="ReadingOnly" select="false()"/>
		<xsl:param name="HyphenBinderOnly" select="$ReadingChoice='ConditionalContext'"/>
		<xsl:param name="ResolveProxy" select="''"/>
		<xsl:choose>
			<xsl:when test="($ReadingChoice='Context' or $ReadingChoice='ConditionalContext') and $PatternGroup='SetConstraint'">
				<xsl:if test="not($HyphenBinderOnly)">
					<plx:assign>
						<plx:left>
							<plx:nameRef name="reading"/>
						</plx:left>
						<plx:right>
							<plx:callInstance name=".implied" type="arrayIndexer">
								<plx:callObject>
									<plx:nameRef name="allConstraintRoleReadings"/>
								</plx:callObject>
								<plx:passParam>
									<plx:nameRef name="{$ConditionalReadingOrderIndex}"/>
								</plx:passParam>
							</plx:callInstance>
						</plx:right>
					</plx:assign>
				</xsl:if>
				<xsl:if test="not($ReadingOnly)">
					<plx:assign>
						<plx:left>
							<plx:nameRef name="hyphenBinder"/>
						</plx:left>
						<plx:right>
							<plx:callNew dataTypeName="VerbalizationHyphenBinder">
								<plx:passParam>
									<plx:nameRef name="reading"/>
								</plx:passParam>
								<plx:passParam>
									<plx:callInstance name="FormatProvider" type="property">
										<plx:callObject>
											<plx:nameRef name="writer" type="parameter"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:passParam>
								<plx:passParam>
									<plx:nameRef name="factRoles"/>
								</plx:passParam>
								<plx:passParam>
									<xsl:call-template name="SnippetFor">
										<xsl:with-param name="SnippetType" select="'HyphenBoundPredicatePart'"/>
									</xsl:call-template>
								</plx:passParam>
								<plx:passParam>
									<plx:nameRef name="predicatePartFormatString"/>
								</plx:passParam>
							</plx:callNew>
						</plx:right>
					</plx:assign>
				</xsl:if>
			</xsl:when>
			<xsl:when test="not($ReadingChoice='Context')">
				<xsl:if test="not($HyphenBinderOnly)">
					<xsl:if test="$ResolveProxy='Check' and contains($ReadingChoice, 'LeadReading')">
						<plx:local name="constrainedRole" dataTypeName="Role">
							<plx:initialize>
								<plx:callInstance type="indexerCall" name=".implied">
									<plx:callObject>
										<plx:nameRef name="includedRoles"/>
									</plx:callObject>
									<plx:passParam>
										<plx:value data="0" type="i4"/>
									</plx:passParam>
								</plx:callInstance>
							</plx:initialize>
						</plx:local>
					</xsl:if>
					<plx:assign>
						<plx:left>
							<plx:nameRef name="reading"/>
						</plx:left>
						<plx:right>
							<plx:callInstance name="GetMatchingReading">
								<plx:callObject>
									<plx:nameRef name="parentFact"/>
								</plx:callObject>
								<plx:passParam>
									<!-- The readingOrders param-->
									<plx:nameRef name="allReadingOrders"/>
								</plx:passParam>
								<plx:passParam>
									<!-- The ignoreReadingOrder param -->
									<plx:nullKeyword/>
								</plx:passParam>
								<plx:passParam>
									<!-- The matchLeadRole param -->
									<xsl:choose>
										<xsl:when test="contains($ReadingChoice,'PrimaryLeadReading')">
											<plx:nameRef name="primaryRole"/>
										</xsl:when>
										<xsl:when test="contains($ReadingChoice, 'LeadReading')">
											<!-- LeadReading and PrimaryLeadReading should be treated the same for single column external constraints -->
											<xsl:choose>
												<xsl:when test="$PatternGroup='SetConstraint'">
													<plx:nameRef name="primaryRole"/>
												</xsl:when>
												<xsl:when test="$ResolveProxy">
													<xsl:choose>
														<xsl:when test="$ResolveProxy='Always'">
															<plx:callInstance type="property" name="Proxy">
																<plx:callObject>
																	<plx:callInstance type="indexerCall" name=".implied">
																		<plx:callObject>
																			<plx:nameRef name="includedRoles"/>
																		</plx:callObject>
																		<plx:passParam>
																			<plx:value data="0" type="i4"/>
																		</plx:passParam>
																	</plx:callInstance>
																</plx:callObject>
															</plx:callInstance>
														</xsl:when>
														<xsl:otherwise>
															<!-- $ResolveProxy='Check' case. The role may not have a proxy. -->
															<plx:inlineStatement dataTypeName="RoleBase">
																<plx:conditionalOperator>
																	<plx:condition>
																		<plx:binaryOperator type="identityEquality">
																			<plx:left>
																				<plx:callInstance name="FactType" type="property">
																					<plx:callObject>
																						<plx:nameRef name="constrainedRole"/>
																					</plx:callObject>
																				</plx:callInstance>
																			</plx:left>
																			<plx:right>
																				<plx:nameRef name="parentFact"/>
																			</plx:right>
																		</plx:binaryOperator>
																	</plx:condition>
																	<plx:left>
																		<plx:nameRef name="constrainedRole"/>
																	</plx:left>
																	<plx:right>
																		<plx:inlineStatement dataTypeName="RoleBase">
																			<plx:nullFallbackOperator>
																				<plx:left>
																					<plx:cast dataTypeName="RoleBase" type="testCast">
																						<plx:callInstance type="property" name="Proxy">
																							<plx:callObject>
																								<plx:nameRef name="constrainedRole"/>
																							</plx:callObject>
																						</plx:callInstance>
																					</plx:cast>
																				</plx:left>
																				<plx:right>
																					<plx:nameRef name="constrainedRole"/>
																				</plx:right>
																			</plx:nullFallbackOperator>
																		</plx:inlineStatement>
																	</plx:right>
																</plx:conditionalOperator>
															</plx:inlineStatement>
														</xsl:otherwise>
													</xsl:choose>
												</xsl:when>
												<xsl:otherwise>
													<plx:nullKeyword/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:when>
										<xsl:otherwise>
											<plx:callInstance name=".implied" type="arrayIndexer">
												<plx:callObject>
													<plx:nameRef name="factRoles"/>
												</plx:callObject>
												<plx:passParam>
													<plx:value type="i4" data="0"/>
												</plx:passParam>
											</plx:callInstance>
										</xsl:otherwise>
									</xsl:choose>
								</plx:passParam>
								<plx:passParam>
									<!-- The matchAnyLeadRole param -->
									<xsl:choose>
										<xsl:when test="not($PatternGroup='SetConstraint') and contains($ReadingChoice,'LeadReading') and not(contains($ReadingChoice,'PrimaryLeadReading')) and not($ResolveProxy)">
											<xsl:choose>
												<xsl:when test="$PatternGroup='InternalSetConstraint'">
													<plx:nameRef name="allConstraintRoles"/>
												</xsl:when>
												<xsl:when test="$PatternGroup='SetComparisonConstraint'">
													<plx:cast dataTypeName="IList" dataTypeQualifier="System.Collections">
														<plx:nameRef name="factRoles"/>
													</plx:cast>
												</xsl:when>
												<xsl:otherwise>
													<plx:nameRef name="includedRoles"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:when>
										<xsl:otherwise>
											<plx:nullKeyword/>
										</xsl:otherwise>
									</xsl:choose>
								</plx:passParam>
								<plx:passParam>
									<!-- The defaultRoleOrder param -->
									<plx:nameRef name="factRoles"/>
								</plx:passParam>
								<plx:passParam>
									<!-- The options parameter -->
									<xsl:variable name="matchedOptionsFragment">
										<xsl:if test="contains($ReadingChoice,'Non')">
											<!-- The InvertLeadRoles option -->
											<plx:callStatic dataTypeName="MatchingReadingOptions" name="InvertLeadRoles" type="field"/>
										</xsl:if>
										<xsl:choose>
											<xsl:when test="contains($ReadingChoice,'InfixTextOnly')">
												<!-- The InfixTextOnly option, combination of NoFrontText and NoTrailingText -->
												<plx:callStatic dataTypeName="MatchingReadingOptions" name="NoFrontText" type="field"/>
												<plx:callStatic dataTypeName="MatchingReadingOptions" name="NoTrailingText" type="field"/>
											</xsl:when>
											<xsl:when test="contains($ReadingChoice,'NoFrontText')">
												<!-- The NoFrontText option -->
												<plx:callStatic dataTypeName="MatchingReadingOptions" name="NoFrontText" type="field"/>
											</xsl:when>
											<xsl:when test="contains($ReadingChoice,'NoTrailingText')">
												<!-- The NoTrailingText option -->
												<plx:callStatic dataTypeName="MatchingReadingOptions" name="NoTrailingText" type="field"/>
											</xsl:when>
										</xsl:choose>
										<xsl:choose>
											<!-- The NotHyphenBound option. Process before the less general LeadRolesNotHyphenBound option -->
											<xsl:when test="contains($ReadingChoice,'NotHyphenBound')">
												<plx:callStatic dataTypeName="MatchingReadingOptions" name="NotHyphenBound" type="field"/>
											</xsl:when>
											<xsl:when test="contains($ReadingChoice,'LeadNotHyphenBound')">
												<!-- The LeadRolesNotHyphenBound option -->
												<plx:callStatic dataTypeName="MatchingReadingOptions" name="LeadRolesNotHyphenBound" type="field"/>
											</xsl:when>
										</xsl:choose>
										<!-- The allowAnyOrder param -->
										<xsl:if test="not(starts-with($ReadingChoice,'Require'))">
											<!-- The AllowAnyOrder option -->
											<plx:callStatic dataTypeName="MatchingReadingOptions" name="AllowAnyOrder" type="field"/>
										</xsl:if>
									</xsl:variable>
									<xsl:variable name="matchedOptions" select="exsl:node-set($matchedOptionsFragment)/child::*"/>
									<xsl:choose>
										<xsl:when test="$matchedOptions">
											<xsl:call-template name="CombineElements">
												<xsl:with-param name="OperatorType" select="'bitwiseOr'"/>
												<xsl:with-param name="Elements" select="$matchedOptions"/>
											</xsl:call-template>
										</xsl:when>
										<xsl:otherwise>
											<plx:callStatic dataTypeName="MatchingReadingOptions" name="None" type="field"/>
										</xsl:otherwise>
									</xsl:choose>
								</plx:passParam>
							</plx:callInstance>
						</plx:right>
					</plx:assign>
				</xsl:if>
				<xsl:if test="not($ReadingOnly) and (not($ConditionalLoop) or not($PatternGroup='SetConstraint' or $PatternGroup='SetComparisonConstraint'))">
					<plx:assign>
						<plx:left>
							<plx:nameRef name="hyphenBinder"/>
						</plx:left>
						<plx:right>
							<plx:callNew dataTypeName="VerbalizationHyphenBinder">
								<plx:passParam>
									<plx:nameRef name="reading"/>
								</plx:passParam>
								<plx:passParam>
									<plx:callInstance name="FormatProvider" type="property">
										<plx:callObject>
											<plx:nameRef name="writer" type="parameter"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:passParam>
								<plx:passParam>
									<plx:nameRef name="factRoles"/>
								</plx:passParam>
								<plx:passParam>
									<xsl:call-template name="SnippetFor">
										<xsl:with-param name="SnippetType" select="'HyphenBoundPredicatePart'"/>
									</xsl:call-template>
								</plx:passParam>
								<plx:passParam>
									<plx:nameRef name="predicatePartFormatString"/>
								</plx:passParam>
							</plx:callNew>
						</plx:right>
					</plx:assign>
				</xsl:if>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="GetVerbalizer" match="*">
		<xsl:param name="type"/>
		<xsl:param name="useDisposeHelper" select="true()"/>
		<plx:implementsInterface dataTypeName="IDisposable"/>
		<plx:pragma type="region" data="Cache management"/>
		<plx:field name="myCache" visibility="private" static="true" dataTypeName="{@type}">
			<plx:leadingInfo>
				<plx:comment>Cache an instance so we only create one helper in single-threaded scenarios</plx:comment>
			</plx:leadingInfo>
		</plx:field>
		<plx:function name="GetVerbalizer" visibility="public" modifier="static">
			<plx:leadingInfo>
				<plx:docComment>
					<summary>Get the cached verbalizer or a new instance.</summary>
				</plx:docComment>
			</plx:leadingInfo>
			<plx:returns dataTypeName="{$type}"/>
			<plx:local name="retVal" dataTypeName="{$type}">
				<plx:initialize>
					<plx:callThis accessor="static" name="myCache" type="field"/>
				</plx:initialize>
			</plx:local>
			<plx:branch>
				<plx:condition>
					<plx:binaryOperator type="identityInequality">
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
						<plx:callStatic name="CompareExchange" dataTypeName="Interlocked" dataTypeQualifier="System.Threading">
							<plx:passMemberTypeParam dataTypeName="{$type}"/>
							<plx:passParam type="inOut">
								<plx:callThis name="myCache" accessor="static" type="field"/>
							</plx:passParam>
							<plx:passParam>
								<plx:cast dataTypeName="{$type}" type="testCast">
									<plx:nullKeyword/>
								</plx:cast>
							</plx:passParam>
							<plx:passParam>
								<plx:nameRef name="retVal"/>
							</plx:passParam>
						</plx:callStatic>
					</plx:right>
				</plx:assign>
			</plx:branch>
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
						<plx:callNew dataTypeName="{$type}" />
					</plx:right>
				</plx:assign>
			</plx:branch>
			<plx:return>
				<plx:nameRef name="retVal"/>
			</plx:return>
		</plx:function>
		<plx:function name="Dispose" visibility="privateInterfaceMember">
			<plx:interfaceMember memberName="Dispose" dataTypeName="IDisposable"/>
			<xsl:if test="boolean($useDisposeHelper)">
				<plx:callThis name="DisposeHelper"/>
			</xsl:if>
			<plx:branch>
				<plx:condition>
					<plx:binaryOperator type="identityEquality">
						<plx:left>
							<plx:callThis name="myCache" accessor="static" type="field"/>
						</plx:left>
						<plx:right>
							<plx:nullKeyword/>
						</plx:right>
					</plx:binaryOperator>
				</plx:condition>
				<plx:callStatic name="CompareExchange" dataTypeName="Interlocked" dataTypeQualifier="System.Threading">
					<plx:passMemberTypeParam dataTypeName="{@type}"/>
					<plx:passParam type="inOut">
						<plx:callThis name="myCache" accessor="static" type="field"/>
					</plx:passParam>
					<plx:passParam>
						<plx:thisKeyword/>
					</plx:passParam>
					<plx:passParam>
						<plx:cast dataTypeName="{@type}" type="testCast">
							<plx:nullKeyword/>
						</plx:cast>
					</plx:passParam>
				</plx:callStatic>
			</plx:branch>
		</plx:function>
		<plx:pragma type="closeRegion" data="Cache management"/>
	</xsl:template>
	<!-- Create a fragment with one element for each item in a list. The ItemList
	should be normalized with normalize-space before this call. -->
	<xsl:template name="SplitList">
		<xsl:param name="ItemList"/>
		<xsl:variable name="itemString" select="substring-before($ItemList,' ')"/>
		<xsl:choose>
			<xsl:when test="$itemString">
				<element>
					<xsl:value-of select="$itemString"/>
				</element>
				<xsl:variable name="remainder" select="substring-after($ItemList,' ')"/>
				<xsl:if test="$remainder">
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
	<!-- Given code that defines local variables and referencing code that may or may not use
	those variables, strip any unused locals. -->
	<xsl:template name="StripUnusedLocals">
		<xsl:param name="LocalCode"/>
		<xsl:param name="ReferencingCode"/>
		<xsl:if test="$LocalCode">
			<xsl:variable name="referencedNames" select="$ReferencingCode/descendant-or-self::plx:nameRef[not(@type) or @type='local']/@name"/>
			<xsl:variable name="referencedLocalCode" select="$LocalCode[descendant-or-self::plx:local[@name=$referencedNames] or descendant-or-self::plx:assign[plx:left/plx:nameRef[@name=$referencedNames]]]"/>
			<xsl:variable name="directReferenceCount" select="count($referencedLocalCode)"/>
			<xsl:choose>
				<!-- Debug code to see what locals are available -->
				<!--<xsl:when test="true()">
					<xsl:copy-of select="$LocalCode"/>
				</xsl:when>-->
				<xsl:when test="$directReferenceCount=0">
					<!-- Nothing referenced, no further processing -->
				</xsl:when>
				<xsl:when test="$directReferenceCount=count($LocalCode)">
					<!-- All elements are referenced, copy all of them -->
					<xsl:copy-of select="$LocalCode"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:variable name="otherNamesFragment">
						<xsl:call-template name="StripUnusedLocals_ResolveLocalDependentNames">
							<xsl:with-param name="UnusedLocalCode" select="$LocalCode[not(descendant-or-self::plx:local[@name=$referencedNames] or descendant-or-self::plx:assign[plx:left/plx:nameRef[@name=$referencedNames]])]"/>
							<xsl:with-param name="NamesReferencedByLocals">
								<xsl:for-each select="$referencedLocalCode/descendant-or-self::plx:nameRef[not(parent::plx:left[parent::plx:assign])]/@name[not(.=$referencedNames)]">
									<name>
										<xsl:value-of select="."/>
									</name>
								</xsl:for-each>
							</xsl:with-param>
						</xsl:call-template>
					</xsl:variable>
					<xsl:variable name="otherNames" select="exsl:node-set($otherNamesFragment)/child::*"/>
					<xsl:choose>
						<xsl:when test="$otherNames">
							<!-- Recalculate the final set in original code order using all names -->
							<xsl:copy-of select="$LocalCode[descendant-or-self::plx:local[@name[.=$referencedNames or .=$otherNames]] or descendant-or-self::plx:assign[plx:left/plx:nameRef[@name[.=$referencedNames or .=$otherNames]]]]"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:copy-of select="$referencedLocalCode"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:copy-of select="$ReferencingCode"/>
	</xsl:template>
	<xsl:template name="StripUnusedLocals_ResolveLocalDependentNames">
		<xsl:param name="UnusedLocalCode"/>
		<xsl:param name="NamesReferencedByLocals"/>
		<xsl:variable name="testNames" select="exsl:node-set($NamesReferencedByLocals)/child::*"/>
		<xsl:variable name="usedLocalCode" select="$UnusedLocalCode[descendant-or-self::plx:local[@name=$testNames] or descendant-or-self::plx:assign[plx:left/plx:nameRef[@name=$testNames]]]"/>
		<xsl:copy-of select="$NamesReferencedByLocals"/>
		<xsl:if test="$usedLocalCode">
			<xsl:variable name="newReferencedLocalNames" select="$usedLocalCode/descendant-or-self::plx:nameRef[not(parent::plx:left[parent::plx:assign])]/@name[not(.=$testNames)]"/>
			<xsl:if test="$newReferencedLocalNames">
				<xsl:variable name="stillUnusedLocalCode" select="$UnusedLocalCode[not(descendant-or-self::plx:local[@name=$testNames] or descendant-or-self::plx:assign[plx:left/plx:nameRef[@name=$testNames]])]"/>
				<xsl:if test="$stillUnusedLocalCode">
					<xsl:call-template name="StripUnusedLocals_ResolveLocalDependentNames">
						<xsl:with-param name="UnusedLocalCode" select="$stillUnusedLocalCode"/>
						<xsl:with-param name="NamesReferencedByLocals">
							<xsl:for-each select="$newReferencedLocalNames">
								<name>
									<xsl:value-of select="."/>
								</name>
							</xsl:for-each>
						</xsl:with-param>
					</xsl:call-template>
				</xsl:if>
			</xsl:if>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>
