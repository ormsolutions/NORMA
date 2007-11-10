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
	xmlns:cvg="http://schemas.neumont.edu/ORM/SDK/CoreVerbalizationGenerator"
	xmlns:exsl="http://exslt.org/common"
	extension-element-prefixes="exsl"
	exclude-result-prefixes="cvg">
	
	<!-- Indenting is useful for debugging the transform, but a waste of memory at generation time -->
	<xsl:output method="xml" encoding="utf-8" indent="no"/>
	<xsl:preserve-space elements="cvg:Snippet"/>
	<!-- Pick up param value supplied automatically by plix loader -->
<xsl:param name="CustomToolNamespace" select="'Neumont.Tools.ORM.ObjectModel'"/>

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

	<!-- Include templates to generate the shared verbalization classes -->
	<xsl:include href="VerbalizationGenerator.Sets.xslt"/>
	<xsl:template match="cvg:VerbalizationRoot">
		<plx:root>
			<plx:namespaceImport name="System"/>
			<plx:namespaceImport name="System.IO"/>
			<plx:namespaceImport name="System.Text"/>
			<plx:namespaceImport name="System.Collections.Generic"/>
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
	<xsl:template match="cvg:Note|cvg:Definition" mode="GenerateClasses">
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
						<summary>IVerbalize.GetVerbalization implementation</summary>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:interfaceMember memberName="GetVerbalization" dataTypeName="IVerbalize"/>
				<plx:param name="writer" dataTypeName="TextWriter"/>
				<plx:param name="snippetsDictionary" dataTypeName="IDictionary">
					<plx:passTypeParam dataTypeName="Type"/>
					<plx:passTypeParam dataTypeName="IVerbalizationSets"/>
				</plx:param>
				<plx:param name="verbalizationContext" dataTypeName="IVerbalizationContext"/>
				<plx:param name="isNegative" dataTypeName=".boolean"/>
				<plx:returns dataTypeName=".boolean"/>

				<!-- Verbalizing a fact type is a simple case of verbalizing a constraint.
					 Leverage the code snippets we use for constraints by setting the right
					 variable names and calling the constraint verbalization templates -->
				<xsl:call-template name="DeclareSnippetsLocal"/>
				<!-- Don't proceed with verbalization if blocking errors are present -->
				<xsl:call-template name="CheckErrorConditions"/>
				<plx:local name="isDeontic" dataTypeName=".boolean" const="true">
					<plx:initialize>
						<plx:falseKeyword/>
					</plx:initialize>
				</plx:local>
				<xsl:apply-templates select="child::*" mode="ConstraintVerbalization">
					<xsl:with-param name="TopLevel" select="true()"/>
				</xsl:apply-templates>
				<xsl:call-template name="CheckErrorConditions">
					<xsl:with-param name="Primary" select="false()"/>
					<xsl:with-param name="DeclareErrorOwner" select="false()"/>
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
	<xsl:template match="cvg:SingleSnippet" mode="GenerateClasses">
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
		<plx:class name="{@type}" visibility="private" partial="true">
			<plx:leadingInfo>
				<plx:pragma type="region" data="FactType verbalization block start"/>
			</plx:leadingInfo>
			<plx:trailingInfo>
				<plx:pragma type="closeRegion" data="FactType verbalization block start"/>
			</plx:trailingInfo>
			<plx:implementsInterface dataTypeName="IVerbalize"/>
			<xsl:call-template name="GetVerbalizer">
				<xsl:with-param name="type" select="@type"/>
				<xsl:with-param name="useDisposeHelper" select="false()"/>
			</xsl:call-template>
			<plx:function name="GetVerbalization" visibility="protected">
				<plx:leadingInfo>
					<plx:docComment>
						<summary>IVerbalize.GetVerbalization implementation</summary>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:interfaceMember memberName="GetVerbalization" dataTypeName="IVerbalize"/>
				<plx:param name="writer" dataTypeName="TextWriter"/>
				<plx:param name="snippetsDictionary" dataTypeName="IDictionary">
					<plx:passTypeParam dataTypeName="Type"/>
					<plx:passTypeParam dataTypeName="IVerbalizationSets"/>
				</plx:param>
				<plx:param name="verbalizationContext" dataTypeName="IVerbalizationContext"/>
				<plx:param name="isNegative" dataTypeName=".boolean"/>
				<plx:returns dataTypeName=".boolean"/>
				<!-- Verbalizing a fact type is a simple case of verbalizing a constraint.
					 Leverage the code snippets we use for constraints by setting the right
					 variable names and calling the constraint verbalization templates -->
				<xsl:call-template name="DeclareSnippetsLocal"/>
				<plx:callInstance name="Write" type="methodCall">
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
						<summary>IVerbalize.GetVerbalization implementation</summary>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:interfaceMember memberName="GetVerbalization" dataTypeName="IVerbalize"/>
				<plx:param name="writer" dataTypeName="TextWriter"/>
				<plx:param name="snippetsDictionary" dataTypeName="IDictionary">
					<plx:passTypeParam dataTypeName="Type"/>
					<plx:passTypeParam dataTypeName="IVerbalizationSets"/>
				</plx:param>
				<plx:param name="verbalizationContext" dataTypeName="IVerbalizationContext"/>
				<plx:param name="isNegative" dataTypeName=".boolean"/>
				<plx:returns dataTypeName=".boolean"/>

				<!-- Verbalizing a fact type is a simple case of verbalizing a constraint.
					 Leverage the code snippets we use for constraints by setting the right
					 variable names and calling the constraint verbalization templates -->
				<xsl:call-template name="DeclareSnippetsLocal"/>
				<!-- Don't proceed with verbalization if blocking errors are present -->
				<xsl:call-template name="CheckErrorConditions"/>
				<xsl:if test="$parentClass='FactType'">
					<plx:local name="parentFact" dataTypeName="FactType">
						<plx:initialize>
							<plx:callThis name="FactType" type="property" />
						</plx:initialize>
					</plx:local>
					<plx:local name="factRoles" dataTypeName="IList">
						<plx:passTypeParam dataTypeName="RoleBase"/>
						<plx:initialize>
							<plx:callInstance name="RoleCollection" type="property">
								<plx:callObject>
									<plx:nameRef name="parentFact" type="local" />
								</plx:callObject>
							</plx:callInstance>
						</plx:initialize>
					</plx:local>
					<plx:local name="unaryRoleIndex" dataTypeName="Nullable">
						<plx:passTypeParam dataTypeName=".i4"/>
						<plx:initialize>
							<xsl:call-template name="InitializeUnaryRoleIndex"/>
						</plx:initialize>
					</plx:local>
					<plx:local name="factArity" dataTypeName=".i4">
						<plx:initialize>
							<xsl:call-template name="InitializeFactArity"/>
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
					<plx:local name="isDeontic" dataTypeName=".boolean" const="true">
						<plx:initialize>
							<plx:falseKeyword/>
						</plx:initialize>
					</plx:local>
					<!--<plx:local name="readingOrder" dataTypeName="ReadingOrder"/>-->
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
					<xsl:variable name="factMockup">
						<cvg:Fact />
					</xsl:variable>
					<xsl:apply-templates select="exsl:node-set($factMockup)/child::*" mode="ConstraintVerbalization">
						<xsl:with-param name="TopLevel" select="true()"/>
					</xsl:apply-templates>
				</xsl:if>
				<!-- End FactType Parent -->
				<!-- Check if we are iterating for ObjectType -->
				<xsl:if test="$parentClass='ObjectType'">
					<xsl:apply-templates select="child::*" mode="ConstraintVerbalization"/>
				</xsl:if>
				<!-- End check if we are iterating for ObjectType -->
				<xsl:call-template name="CheckErrorConditions">
					<xsl:with-param name="Primary" select="false()"/>
					<xsl:with-param name="DeclareErrorOwner" select="false()"/>
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
						<summary>IVerbalize.GetVerbalization implementation</summary>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:interfaceMember memberName="GetVerbalization" dataTypeName="IVerbalize"/>
				<plx:param name="writer" dataTypeName="TextWriter"/>
				<plx:param name="snippetsDictionary" dataTypeName="IDictionary">
					<plx:passTypeParam dataTypeName="Type"/>
					<plx:passTypeParam dataTypeName="IVerbalizationSets"/>
				</plx:param>
				<plx:param name="verbalizationContext" dataTypeName="IVerbalizationContext"/>
				<plx:param name="isNegative" dataTypeName=".boolean"/>
				<plx:returns dataTypeName=".boolean"/>

				<!-- Verbalizing a fact type is a simple case of verbalizing a constraint.
					 Leverage the code snippets we use for constraints by setting the right
					 variable names and calling the constraint verbalization templates -->
				<xsl:call-template name="DeclareSnippetsLocal"/>
				<!-- Don't proceed with verbalization if blocking errors are present -->
				<xsl:call-template name="CheckErrorConditions"/>
				<plx:local name="factRoles" dataTypeName="IList">
					<plx:passTypeParam dataTypeName="RoleBase"/>
					<plx:initialize>
						<plx:callThis name="RoleCollection" type="property"/>
					</plx:initialize>
				</plx:local>
				<plx:local name="unaryRoleIndex" dataTypeName="Nullable">
					<plx:passTypeParam dataTypeName=".i4"/>
					<plx:initialize>
						<xsl:call-template name="InitializeUnaryRoleIndex"/>
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
				<plx:local name="allReadingOrders" dataTypeName="LinkedElementCollection">
					<plx:passTypeParam dataTypeName="ReadingOrder"/>
					<plx:initialize>
						<plx:callThis name="ReadingOrderCollection" type="property"/>
					</plx:initialize>
				</plx:local>
				<plx:local name="isDeontic" dataTypeName=".boolean" const="true">
					<plx:initialize>
						<plx:falseKeyword/>
					</plx:initialize>
				</plx:local>
				<!--<plx:local name="readingOrder" dataTypeName="ReadingOrder"/>-->
				<plx:local name="reading" dataTypeName="IReading"/>
				<plx:local name="hyphenBinder" dataTypeName="VerbalizationHyphenBinder"/>
				<xsl:call-template name="PopulateBasicRoleReplacements"/>
				<xsl:variable name="factMockup">
					<cvg:Fact/>
				</xsl:variable>
				<xsl:apply-templates select="exsl:node-set($factMockup)/child::*" mode="ConstraintVerbalization">
					<xsl:with-param name="TopLevel" select="true()"/>
				</xsl:apply-templates>
				<xsl:call-template name="CheckErrorConditions">
					<xsl:with-param name="Primary" select="false()"/>
					<xsl:with-param name="DeclareErrorOwner" select="false()"/>
				</xsl:call-template>
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
						<summary>IVerbalize.GetVerbalization implementation</summary>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:interfaceMember memberName="GetVerbalization" dataTypeName="IVerbalize"/>
				<plx:param name="writer" dataTypeName="TextWriter"/>
				<plx:param name="snippetsDictionary" dataTypeName="IDictionary">
					<plx:passTypeParam dataTypeName="Type"/>
					<plx:passTypeParam dataTypeName="IVerbalizationSets"/>
				</plx:param>
				<plx:param name="verbalizationContext" dataTypeName="IVerbalizationContext"/>
				<plx:param name="isNegative" dataTypeName=".boolean"/>
				<plx:returns dataTypeName=".boolean"/>

				<xsl:call-template name="DeclareSnippetsLocal"/>
				<!-- Don't proceed with verbalization if blocking errors are present -->
				<xsl:call-template name="CheckErrorConditions"/>
				<plx:local name="isDeontic" dataTypeName=".boolean" const="true">
					<plx:initialize>
						<plx:falseKeyword/>
					</plx:initialize>
				</plx:local>
				<xsl:apply-templates select="child::cvg:*" mode="ConstraintVerbalization">
					<xsl:with-param name="TopLevel" select="true()"/>
				</xsl:apply-templates>
				<xsl:call-template name="CheckErrorConditions">
					<xsl:with-param name="Primary" select="false()"/>
					<xsl:with-param name="DeclareErrorOwner" select="false()"/>
				</xsl:call-template>
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
						<summary>IVerbalize.GetVerbalization implementation</summary>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:interfaceMember memberName="GetVerbalization" dataTypeName="IVerbalize"/>
				<plx:param name="writer" dataTypeName="TextWriter"/>
				<plx:param name="snippetsDictionary" dataTypeName="IDictionary">
					<plx:passTypeParam dataTypeName="Type"/>
					<plx:passTypeParam dataTypeName="IVerbalizationSets"/>
				</plx:param>
				<plx:param name="verbalizationContext" dataTypeName="IVerbalizationContext"/>
				<plx:param name="isNegative" dataTypeName=".boolean"/>
				<plx:returns dataTypeName=".boolean"/>

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
				<xsl:apply-templates select="child::*" mode="ConstraintVerbalization">
					<xsl:with-param name="TopLevel" select="true()"/>
				</xsl:apply-templates>
				<xsl:call-template name="CheckErrorConditions">
					<xsl:with-param name="Primary" select="false()"/>
					<xsl:with-param name="DeclareErrorOwner" select="false()"/>
				</xsl:call-template>
				<plx:return>
					<plx:trueKeyword/>
				</plx:return>
			</plx:function>
		</plx:class>
	</xsl:template>
	<xsl:template match="cvg:Constraint" mode="ConstraintVerbalization">
		<xsl:variable name="patternGroup" select="string(@patternGroup)"/>
		<xsl:variable name="isValueTypeValueConstraint" select="$patternGroup='ValueTypeValueConstraint'"/>
		<xsl:variable name="isRoleValue" select="$patternGroup='RoleValueConstraint'"/>
		<xsl:variable name="isInternal" select="$patternGroup='InternalConstraint' or $isRoleValue"/>
		<xsl:variable name="isSingleColumn" select="$patternGroup='SetConstraint'"/>
		<xsl:variable name="parentClass" select="string(@childHelperFor)"/>
		<xsl:variable name="isChildHelper" select="boolean($parentClass)"/>
		<xsl:variable name="isSetComparisonConstraint" select="$patternGroup='SetComparisonConstraint'"/>
		<xsl:variable name="compatibleColumns" select="@compatibleColumns='true' or @compatibleColumns='1'"/>
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
						<summary>IVerbalize.GetVerbalization implementation</summary>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:interfaceMember memberName="GetVerbalization" dataTypeName="IVerbalize"/>
				<plx:param name="writer" dataTypeName="TextWriter"/>
				<plx:param name="snippetsDictionary" dataTypeName="IDictionary">
					<plx:passTypeParam dataTypeName="Type"/>
					<plx:passTypeParam dataTypeName="IVerbalizationSets"/>
				</plx:param>
				<plx:param name="verbalizationContext" dataTypeName="IVerbalizationContext"/>
				<plx:param name="isNegative" dataTypeName=".boolean"/>
				<plx:returns dataTypeName=".boolean"/>

				<xsl:call-template name="DeclareSnippetsLocal"/>
				<!-- Don't proceed with verbalization if blocking errors are present -->
				<xsl:if test="not($isChildHelper)">
					<xsl:call-template name="CheckErrorConditions"/>
				</xsl:if>
				<xsl:variable name="subscriptConditionsFragment">
					<!-- UNDONE: Better subscript handling. The conditional processing needs
						 to be moved inside each pattern, but we need to prepare for the situation
						 up front. Consider getting the generator out of the subscripting business
						 altogether. We're basically just spitting an inline function. For now,
						 keep the conditional checks in place so we don't lose the work. The trueKeyword
						 spit here will be compiled out and not appear in code. -->
					<xsl:apply-templates select="cvg:EnableSubscripts" mode="SubscriptConditions"/>
					<!--<plx:trueKeyword/>-->
				</xsl:variable>
				<xsl:variable name="subscriptConditions" select="exsl:node-set($subscriptConditionsFragment)/child::*"/>

				<!-- Pick up standard code we'll need for any constraint -->
				<xsl:if test="$isRoleValue">
					<plx:local name="valueRole" dataTypeName="Role">
						<plx:initialize>
							<plx:callThis name="Role" type="property"/>
						</plx:initialize>
					</plx:local>
				</xsl:if>
				<xsl:choose>
					<xsl:when test="($isInternal and not($isRoleValue)) or $isSingleColumn or $isSetComparisonConstraint">
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
					<xsl:when test="$isValueTypeValueConstraint">
						<plx:local name="isDeontic" dataTypeName=".boolean" const="true">
							<plx:initialize>
								<plx:falseKeyword/>
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
				<xsl:if test="not($isValueTypeValueConstraint)">
					<plx:local name="parentFact" dataTypeName="FactType">
						<xsl:choose>
							<xsl:when test="$isInternal and not($isRoleValue)">
								<plx:initialize>
									<plx:callThis name="FactType" type="property"/>
								</plx:initialize>
							</xsl:when>
							<xsl:when test="$isRoleValue">
								<plx:initialize>
									<plx:callInstance name="FactType" type="property">
										<plx:callObject>
											<plx:nameRef name="valueRole" type="local"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:initialize>
							</xsl:when>
						</xsl:choose>
					</plx:local>
				</xsl:if>
				<xsl:if test="$isInternal and not($isRoleValue)">
					<plx:local name="includedRoles" dataTypeName="LinkedElementCollection">
						<plx:passTypeParam dataTypeName="Role"/>
						<plx:initialize>
							<plx:callThis name="RoleCollection" type="property"/>
						</plx:initialize>
					</plx:local>
				</xsl:if>
				<xsl:if test="$isRoleValue">
					<plx:local name="includedRoles" dataTypeName="IList">
						<plx:passTypeParam dataTypeName="Role"/>
						<plx:initialize>
							<plx:callNew dataTypeName="Role">
								<plx:arrayDescriptor rank="1"/>
								<plx:arrayInitializer>
									<plx:nameRef name="valueRole" type="local"/>
								</plx:arrayInitializer>
							</plx:callNew>
						</plx:initialize>
					</plx:local>
				</xsl:if>
				<xsl:if test="not($isValueTypeValueConstraint)">
					<plx:local name="factRoles" dataTypeName="IList">
						<plx:passTypeParam dataTypeName="RoleBase"/>
						<plx:initialize>
							<xsl:choose>
								<xsl:when test="$isInternal">
									<plx:callInstance name="RoleCollection" type="property">
										<plx:callObject>
											<plx:nameRef name="parentFact"/>
										</plx:callObject>
									</plx:callInstance>
								</xsl:when>
								<xsl:otherwise>
									<plx:nullKeyword/>
								</xsl:otherwise>
							</xsl:choose>
						</plx:initialize>
					</plx:local>
					<plx:local name="unaryRoleIndex" dataTypeName="Nullable">
						<plx:passTypeParam dataTypeName=".i4"/>
						<plx:initialize>
							<xsl:choose>
								<xsl:when test="$isInternal">
									<xsl:call-template name="InitializeUnaryRoleIndex"/>
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
				</xsl:if>
				<xsl:if test="$isSetComparisonConstraint">
					<plx:local name="constraintSequences" dataTypeName="LinkedElementCollection">
						<plx:passTypeParam dataTypeName="SetComparisonConstraintRoleSequence"/>
						<plx:initialize>
							<plx:callThis type="property" name="RoleSequenceCollection"/>
						</plx:initialize>
					</plx:local>
				</xsl:if>
				<xsl:if test="not($isInternal) and not($isValueTypeValueConstraint)">
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
							<plx:local name="allConstraintSequences" dataTypeIsSimpleArray="true" dataTypeName="IList">
								<plx:passTypeParam dataTypeName="Role"/>
								<plx:initialize>
									<plx:callNew dataTypeName="IList" dataTypeIsSimpleArray="true">
										<plx:passTypeParam dataTypeName="Role"/>
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
												<plx:nameRef name="allConstraintSequences" type="local"/>
											</plx:callObject>
											<plx:passParam>
												<plx:nameRef name="i" type="local" />
											</plx:passParam>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:callInstance name="RoleCollection" type="property">
											<plx:callObject>
												<plx:callInstance name=".implied" type="indexerCall">
													<plx:callObject>
														<plx:nameRef name="constraintSequences" type="local"/>
													</plx:callObject>
													<plx:passParam>
														<plx:nameRef name="i" type="local" />
													</plx:passParam>
												</plx:callInstance>
											</plx:callObject>
										</plx:callInstance>
									</plx:right>
								</plx:assign>
							</plx:loop>
							<plx:local name="columnArity" dataTypeName=".i4">
								<plx:initialize>
									<plx:callInstance name="Count" type="property">
										<plx:callObject>
											<plx:callInstance name=".implied" type="indexerCall">
												<plx:callObject>
													<plx:nameRef name="allConstraintSequences" type="local"/>
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
						<xsl:if test="not($isChildHelper)">
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
							<xsl:if test="not($isChildHelper)">
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
						<plx:local name="allBasicRoleReplacements" dataTypeName=".string">
							<plx:arrayDescriptor rank="1">
								<plx:arrayDescriptor rank="1"/>
							</plx:arrayDescriptor>
							<plx:initialize>
								<plx:callNew dataTypeName=".string">
									<plx:arrayDescriptor rank="1">
										<plx:arrayDescriptor rank="1"/>
									</plx:arrayDescriptor>
									<plx:passParam>
										<plx:nameRef name="allFactsCount"/>
									</plx:passParam>
								</plx:callNew>
							</plx:initialize>
						</plx:local>
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
											<plx:callInstance name="ReadingRequiredError" type="property">
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
								<xsl:if test="not($isChildHelper)">
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
									<plx:nameRef name="factRoles"/>
								</plx:left>
								<plx:right>
									<plx:callInstance name="RoleCollection" type="property">
										<plx:callObject>
											<plx:nameRef name="currentFact"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:right>
							</plx:assign>
							<plx:assign>
								<plx:left>
									<plx:nameRef name="unaryRoleIndex"/>
								</plx:left>
								<plx:right>
									<xsl:call-template name="InitializeUnaryRoleIndex"/>
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
							<!-- Populate the basic replacements for this fact -->
							<xsl:call-template name="PopulateBasicRoleReplacements">
								<xsl:with-param name="SubscriptConditions" select="$subscriptConditions"/>
								<xsl:with-param name="CompatibleColumns" select="$compatibleColumns"/>
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
					</xsl:call-template>
				</xsl:if>
				<xsl:if test="not($isValueTypeValueConstraint)">
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
				<xsl:if test="$isRoleValue or $isValueTypeValueConstraint">
					<plx:local name="ranges" dataTypeName="LinkedElementCollection">
						<plx:passTypeParam dataTypeName="ValueRange"/>
						<plx:initialize>
							<plx:callThis name="ValueRangeCollection" type="property"/>
						</plx:initialize>
					</plx:local>
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
				</xsl:if>
				<xsl:apply-templates select="child::*" mode="ConstraintVerbalization">
					<xsl:with-param name="PatternGroup" select="$patternGroup"/>
					<xsl:with-param name="TopLevel" select="true()"/>
				</xsl:apply-templates>
				<xsl:if test="not($isChildHelper)">
					<xsl:call-template name="CheckErrorConditions">
						<xsl:with-param name="Primary" select="false()"/>
						<xsl:with-param name="DeclareErrorOwner" select="false()"/>
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
		<xsl:call-template name="ConstraintConditions">
			<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
		</xsl:call-template>
	</xsl:template>
	<xsl:template name="InitializeUnaryRoleIndex">
		<plx:callStatic dataTypeName="FactType" name="GetUnaryRoleIndex">
			<plx:passParam>
				<plx:nameRef name="factRoles"/>
			</plx:passParam>
		</plx:callStatic>
	</xsl:template>
	<xsl:template name="InitializeFactArity">
		<plx:inlineStatement dataTypeName=".i4">
			<plx:conditionalOperator>
				<plx:condition>
					<plx:callInstance name="HasValue" type="property">
						<plx:callObject>
							<plx:nameRef name="unaryRoleIndex"/>
						</plx:callObject>
					</plx:callInstance>
				</plx:condition>
				<plx:left>
					<plx:value data="1" type="i4"/>
				</plx:left>
				<plx:right>
					<plx:callInstance name="Count" type="property">
						<plx:callObject>
							<plx:nameRef name="factRoles"/>
						</plx:callObject>
					</plx:callInstance>
				</plx:right>
			</plx:conditionalOperator>
		</plx:inlineStatement>
	</xsl:template>
	<xsl:template name="CheckErrorConditions">
		<xsl:param name="Primary" select="true()"/>
		<xsl:param name="BeginVerbalization" select="$Primary"/>
		<xsl:param name="DeclareErrorOwner" select="true()"/>
		<xsl:if test="$DeclareErrorOwner">
			<plx:local name="errorOwner" dataTypeName="IModelErrorOwner">
				<plx:initialize>
					<plx:cast type="testCast" dataTypeName="IModelErrorOwner">
						<plx:thisKeyword/>
					</plx:cast>
				</plx:initialize>
			</plx:local>
			<plx:local name="firstErrorPending" dataTypeName=".boolean"/>
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
				<xsl:with-param name="BeginVerbalization" select="$BeginVerbalization"/>
			</xsl:call-template>
		</plx:branch>
	</xsl:template>
	<xsl:template name="CheckErrorConditionsBody">
		<xsl:param name="Primary" select="true()"/>
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
				<plx:callInstance name="GetErrorCollection" type="methodCall">
					<plx:callObject>
						<plx:nameRef name="errorOwner"/>
					</plx:callObject>
					<plx:passParam>
						<plx:callStatic dataTypeName="ModelErrorUses" name="{$filter}" type="field"/>
					</plx:passParam>
				</plx:callInstance>
			</plx:initialize>
			<xsl:if test="not($Primary)">
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
						<plx:binaryOperator type="booleanAnd">
							<plx:left>
								<plx:binaryOperator type="identityInequality">
									<plx:left>
										<plx:nameRef name="errorDisplayFilter"/>
									</plx:left>
									<plx:right>
										<plx:nullKeyword/>
									</plx:right>
								</plx:binaryOperator>
							</plx:left>
							<plx:right>
								<plx:unaryOperator type="booleanNot">
									<plx:callInstance name="ShouldDisplay">
										<plx:callObject>
											<plx:nameRef name="errorDisplayFilter"/>
										</plx:callObject>
										<plx:passParam>
											<plx:nameRef name="error"/>
										</plx:passParam>
									</plx:callInstance>
								</plx:unaryOperator>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:continue/>
				</plx:branch>
			</xsl:if>
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
						<plx:callInstance name="BeginVerbalization">
							<plx:callObject>
								<plx:nameRef name="verbalizationContext" type="parameter"/>
							</plx:callObject>
							<plx:passParam>
								<plx:callStatic name="ErrorReport" dataTypeName="VerbalizationContent" type="field"/>
							</plx:passParam>
						</plx:callInstance>
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
			<xsl:if test="$Primary">
				<xsl:call-template name="CheckErrorConditionsBody">
					<xsl:with-param name="Primary" select="false()"/>
				</xsl:call-template>
				<plx:return>
					<plx:trueKeyword/>
				</plx:return>
			</xsl:if>
		</plx:branch>
	</xsl:template>
	<!-- Handle the span constraint condition attribute -->
	<xsl:template match="@span" mode="ConstraintConditionOperator">
		<xsl:param name="PatternGroup"/>
		<xsl:choose>
			<xsl:when test=".='all'">
				<plx:binaryOperator type="equality">
					<plx:left>
						<plx:nameRef name="factArity"/>
					</plx:left>
					<plx:right>
						<xsl:choose>
							<xsl:when test="$PatternGroup='SetConstraint' and parent::*[@constraintArity=1 or @factCount=1]">
								<plx:nameRef name="constraintRoleArity"/>
							</xsl:when>
							<xsl:otherwise>
								<plx:nameRef name="includedArity"/>
							</xsl:otherwise>
						</xsl:choose>
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
	<!-- Helper template for spitting conditions based on specified conditions. All conditions
		 are combined with an and operator, and are given priority based on the order they
		 appear in the data file. The assumption is made that the unconstrained condition
		 is sorted last. -->
	<xsl:template name="ConstraintConditions">
		<xsl:param name="PatternGroup"/>
		<xsl:variable name="fallback" select="boolean(preceding-sibling::cvg:ConstrainedRoles)"/>
		<xsl:variable name="conditionTestFragment">
			<xsl:variable name="conditionOperatorsFragment">
				<xsl:apply-templates select="@*" mode="ConstraintConditionOperator">
					<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
				</xsl:apply-templates>
			</xsl:variable>
			<xsl:for-each select="exsl:node-set($conditionOperatorsFragment)/child::*">
				<xsl:if test="position()=1">
					<xsl:call-template name="CombineElements">
						<xsl:with-param name="OperatorType" select="'booleanAnd'"/>
					</xsl:call-template>
				</xsl:if>
			</xsl:for-each>
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
					</xsl:call-template>
				</xsl:element>
			</xsl:when>
			<xsl:when test="$fallback">
				<plx:fallbackBranch>
					<xsl:call-template name="ConstraintBodyContent">
						<xsl:with-param name="PatternGroup" select="string($forwardPatternGroup)"/>
					</xsl:call-template>
				</plx:fallbackBranch>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="ConstraintBodyContent">
					<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- Helper template to combine expressions using the specified OperatorType. An external
		 call should fire this from inside a for each for the first element, it will then
		 recurse to pick up remaining elements -->
	<xsl:template name="CombineElements">
		<xsl:param name="OperatorType"/>
		<xsl:choose>
			<xsl:when test="position()=last()">
				<xsl:copy-of select="."/>
			</xsl:when>
			<xsl:otherwise>
				<plx:binaryOperator type="{$OperatorType}">
					<plx:left>
						<xsl:copy-of select="."/>
					</plx:left>
					<plx:right>
						<xsl:for-each select="following-sibling::*">
							<xsl:if test="position()=1">
								<xsl:call-template name="CombineElements">
									<xsl:with-param name="OperatorType" select="$OperatorType"/>
								</xsl:call-template>
							</xsl:if>
						</xsl:for-each>
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
				<plx:callInstance name="Name" type="property">
					<plx:callObject>
						<xsl:copy-of select="$ObjectTypeExpression"/>
					</plx:callObject>
				</plx:callInstance>
			</plx:passParam>
		</plx:callStatic>
	</xsl:template>
	<xsl:template name="PopulateBasicRoleReplacements_FormatSubscript">
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
					<xsl:with-param name="SnippetType" select="'ObjectTypeWithSubscript'"/>
				</xsl:call-template>
			</plx:passParam>
			<plx:passParam>
				<plx:callInstance name="Name" type="property">
					<plx:callObject>
						<xsl:copy-of select="$ObjectTypeExpression"/>
					</plx:callObject>
				</plx:callInstance>
			</plx:passParam>
			<plx:passParam>
				<plx:binaryOperator type="add">
					<plx:left>
						<plx:nameRef name="subscript"/>
					</plx:left>
					<plx:right>
						<plx:value type="i4" data="1"/>
					</plx:right>
				</plx:binaryOperator>
			</plx:passParam>
		</plx:callStatic>
	</xsl:template>
	<xsl:template name="PopulateBasicRoleReplacements_Assign_Body">
		<xsl:param name="ObjectTypeExpression"/>
		<xsl:param name="Append" select="false()"/>
		<xsl:param name="SubscriptConditions" select="true()"/>
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
			<xsl:when test="$SubscriptConditions">
				<xsl:variable name="subscriptFormat">
					<xsl:call-template name="PopulateBasicRoleReplacements_FormatSubscript">
						<xsl:with-param name="ObjectTypeExpression" select="$ObjectTypeExpression"/>
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
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- Declare the basicRoleReplacements variable for a single fact and populate the basic
		 replacement fields. The fact's roles will be in the factRoles variable
		 and the fact arity in the factArity variable -->
	<xsl:template name="PopulateBasicRoleReplacements">
		<xsl:param name="SubscriptConditions"/>
		<xsl:param name="DeclareBasicRoleReplacements" select="true()"/>
		<xsl:param name="CompatibleColumns" select="false()"/>
		<xsl:param name="PatternGroup"/>
		<xsl:param name="IncludeInstanceData" select="false()"/>
		<xsl:if test="$DeclareBasicRoleReplacements">
			<plx:local name="basicRoleReplacements" dataTypeName=".string" dataTypeIsSimpleArray="true">
				<plx:initialize>
					<plx:callNew dataTypeName=".string" dataTypeIsSimpleArray="true">
						<plx:passParam>
							<plx:nameRef name="factArity"/>
						</plx:passParam>
					</plx:callNew>
				</plx:initialize>
			</plx:local>
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
						</xsl:variable>
						<!-- See if we need a subscript by comparing to other role players before and after this one -->
						<plx:local name="subscript" dataTypeName=".i4">
							<plx:initialize>
								<plx:value type="i4" data="0"/>
							</plx:initialize>
						</plx:local>
						<plx:local name="useSubscript" dataTypeName=".boolean">
							<plx:initialize>
								<plx:falseKeyword/>
							</plx:initialize>
						</plx:local>
						<xsl:choose>
							<xsl:when test="count($SubscriptConditions)=1 and local-name($SubscriptConditions)='trueKeyword'">
								<xsl:copy-of select="$subscriptBody"/>
							</xsl:when>
							<xsl:otherwise>
								<plx:branch>
									<plx:condition>
										<xsl:copy-of select="$SubscriptConditions"/>
									</plx:condition>
									<xsl:copy-of select="$subscriptBody"/>
								</plx:branch>
							</xsl:otherwise>
						</xsl:choose>
						<xsl:call-template name="PopulateBasicRoleReplacements_Assign">
							<xsl:with-param name="CompatibleColumns" select="$CompatibleColumns"/>
							<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
						</xsl:call-template>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="PopulateBasicRoleReplacements_Assign">
							<xsl:with-param name="CompatibleColumns" select="$CompatibleColumns"/>
							<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
							<xsl:with-param name="SubscriptConditions" select="false()"/>
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
					</plx:callInstance>
				</plx:left>
				<plx:right>
					<plx:nameRef name="basicReplacement"/>
				</plx:right>
			</plx:assign>
		</plx:loop>
	</xsl:template>
	<xsl:template name="ConstraintBodyContent">
		<xsl:param name="PatternGroup"/>
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
		<xsl:apply-templates select="child::*" mode="ConstraintVerbalization">
			<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
			<xsl:with-param name="TopLevel" select="true()"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="cvg:ReadingContext" mode="ConstraintVerbalization">
		<xsl:param name="TopLevel" select="false()"/>
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="'snippet'"/>
		<xsl:param name="IteratorContext" select="''"/>
		<xsl:param name="IteratorVariableName" select="''"/>
		<xsl:param name="ReadingFactTypeVariableName" select="''"/>
		<xsl:param name="PatternGroup"/>
		<xsl:call-template name="PopulateReading">
			<xsl:with-param name="ReadingChoice" select="@match"/>
			<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
		</xsl:call-template>
		<xsl:apply-templates select="child::*" mode="ConstraintVerbalization">
			<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
			<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
			<xsl:with-param name="IteratorVariableName" select="$IteratorVariableName"/>
			<xsl:with-param name="ReadingFactTypeVariableName" select="$ReadingFactTypeVariableName"/>
			<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
			<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
			<xsl:with-param name="TopLevel" select="$TopLevel"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="cvg:ConditionalReading" mode="ConstraintVerbalization">
		<xsl:param name="TopLevel" select="false()"/>
		<xsl:param name="IteratorContext" select="''"/>
		<xsl:param name="IteratorVariableName" select="''"/>
		<xsl:param name="ReadingFactTypeVariableName" select="''"/>
		<xsl:param name="PatternGroup"/>
		<xsl:for-each select="cvg:ReadingChoice">
			<xsl:if test="position()=1">
				<xsl:call-template name="ProcessConditionalReadingChoice">
					<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
					<xsl:with-param name="IteratorVariableName" select="$IteratorVariableName"/>
					<xsl:with-param name="TopLevel" select="$TopLevel"/>
					<xsl:with-param name="ReadingFactTypeVariableName" select="$ReadingFactTypeVariableName"/>
					<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
				</xsl:call-template>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>
	<xsl:template name="ProcessConditionalReadingChoice">
		<xsl:param name="IteratorContext" select="''"/>
		<xsl:param name="IteratorVariableName" select="''"/>
		<xsl:param name="ReadingFactTypeVariableName" select="''"/>
		<xsl:param name="VariableDecorator" select="'1'"/>
		<xsl:param name="TopLevel" select="false()"/>
		<xsl:param name="Match" select="string(@match)"/>
		<xsl:param name="PatternGroup"/>
		<xsl:param name="CurrentColumnExpression">
			<plx:value type="i4" data="0"/>
		</xsl:param>
		<xsl:choose>
			<xsl:when test="contains($Match,'All')">
				<xsl:variable name="singleMatch" select="concat(substring-before($Match,'All'), substring-after($Match,'All'))"/>
				<xsl:variable name="missingReadingLocalName" select="concat('missingReading',$VariableDecorator)"/>
				<xsl:variable name="readingMatchIndexLocalName" select="concat('readingMatchIndex',$VariableDecorator)"/>
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
							<plx:callInstance name=".implied" type="indexerCall">
								<xsl:choose>
									<xsl:when test="$PatternGroup='SetComparisonConstraint'">
										<plx:callObject>
											<plx:callInstance name=".implied" type="arrayIndexer">
												<plx:callObject>
													<plx:nameRef name="allConstraintSequences"/>
												</plx:callObject>
												<plx:passParam>
													<plx:nameRef name="{$readingMatchIndexLocalName}"/>
												</plx:passParam>
											</plx:callInstance>
										</plx:callObject>
										<plx:passParam>
											<xsl:copy-of select="$CurrentColumnExpression"/>
										</plx:passParam>
									</xsl:when>
									<xsl:otherwise>
										<plx:callObject>
											<plx:nameRef name="allConstraintRoles"/>
										</plx:callObject>
										<plx:passParam>
											<plx:nameRef name="{$readingMatchIndexLocalName}"/>
										</plx:passParam>
									</xsl:otherwise>
								</xsl:choose>
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
									<plx:nameRef name="primaryRole"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:right>
					</plx:assign>
					<plx:assign>
						<plx:left>
							<plx:nameRef name="factRoles"/>
						</plx:left>
						<plx:right>
							<plx:callInstance name="RoleCollection" type="property">
								<plx:callObject>
									<plx:nameRef name="parentFact"/>
								</plx:callObject>
							</plx:callInstance>
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
					<xsl:call-template name="PopulateReading">
						<xsl:with-param name="ReadingChoice" select="$singleMatch"/>
						<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
						<xsl:with-param name="ReadingFactTypeVariableName" select="$ReadingFactTypeVariableName"/>
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
					<xsl:apply-templates select="child::*" mode="ConstraintVerbalization">
						<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
						<xsl:with-param name="IteratorVariableName" select="$IteratorVariableName"/>
						<xsl:with-param name="ReadingFactTypeVariableName" select="$ReadingFactTypeVariableName"/>
						<xsl:with-param name="TopLevel" select="$TopLevel"/>
						<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
					</xsl:apply-templates>
				</plx:branch>
				<xsl:if test="position()!=last()">
					<plx:fallbackBranch>
						<xsl:for-each select="following-sibling::*">
							<xsl:if test="position()=1">
								<xsl:call-template name="ProcessConditionalReadingChoice">
									<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
									<xsl:with-param name="IteratorVariableName" select="$IteratorVariableName"/>
									<xsl:with-param name="ReadingFactTypeVariableName" select="$ReadingFactTypeVariableName"/>
									<xsl:with-param name="VariableDecorator" select="$VariableDecorator + 1"/>
									<xsl:with-param name="TopLevel" select="$TopLevel"/>
									<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
								</xsl:call-template>
							</xsl:if>
						</xsl:for-each>
					</plx:fallbackBranch>
				</xsl:if>
			</xsl:when>
			<xsl:when test="$PatternGroup='SetConstraint'">
				<!-- The rest of this block is duplicated in other when and otherwise conditions, keep in sync -->
				<xsl:apply-templates select="child::*" mode="ConstraintVerbalization">
					<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
					<xsl:with-param name="IteratorVariableName" select="$IteratorVariableName"/>
					<xsl:with-param name="ReadingFactTypeVariableName" select="$ReadingFactTypeVariableName"/>
					<xsl:with-param name="TopLevel" select="$TopLevel"/>
					<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<!-- If we are a SetComparisonConstraint, we do not need to spit any extra code -->
				<xsl:choose>
					<xsl:when test="not($PatternGroup = 'SetComparisonConstraint')">
						<xsl:call-template name="PopulateReading">
							<xsl:with-param name="ReadingChoice" select="$Match"/>
							<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
							<xsl:with-param name="ReadingFactTypeVariableName" select="$ReadingFactTypeVariableName"/>
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
							<xsl:apply-templates select="child::*" mode="ConstraintVerbalization">
								<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
								<xsl:with-param name="IteratorVariableName" select="$IteratorVariableName"/>
								<xsl:with-param name="ReadingFactTypeVariableName" select="$ReadingFactTypeVariableName"/>
								<xsl:with-param name="TopLevel" select="$TopLevel"/>
								<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
							</xsl:apply-templates>
						</plx:branch>
						<xsl:if test="position()!=last()">
							<plx:fallbackBranch>
								<xsl:for-each select="following-sibling::*">
									<xsl:if test="position()=1">
										<xsl:call-template name="ProcessConditionalReadingChoice">
											<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
											<xsl:with-param name="IteratorVariableName" select="$IteratorVariableName"/>
											<xsl:with-param name="ReadingFactTypeVariableName" select="$ReadingFactTypeVariableName"/>
											<xsl:with-param name="VariableDecorator" select="$VariableDecorator + 1"/>
											<xsl:with-param name="TopLevel" select="$TopLevel"/>
											<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
										</xsl:call-template>
									</xsl:if>
								</xsl:for-each>
							</plx:fallbackBranch>
						</xsl:if>
					</xsl:when>
					<xsl:otherwise>
						<xsl:apply-templates select="child::*" mode="ConstraintVerbalization">
							<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
							<xsl:with-param name="IteratorVariableName" select="$IteratorVariableName"/>
							<xsl:with-param name="ReadingFactTypeVariableName" select="$ReadingFactTypeVariableName"/>
							<xsl:with-param name="TopLevel" select="$TopLevel"/>
							<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
						</xsl:apply-templates>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:otherwise>
		</xsl:choose>
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

	<xsl:template match="cvg:ConditionalSnippet" mode="ConstraintVerbalization">
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="'variableSnippet'"/>
		<xsl:param name="TopLevel" select="false()"/>
		<xsl:param name="IteratorContext" select="''"/>
		<xsl:param name="IteratorVariableName" select="''"/>
		<xsl:param name="ReadingFactTypeVariableName" select="''"/>
		<xsl:param name="PatternGroup"/>
		<xsl:variable name="SnippetTypeVariable" select="concat($VariablePrefix, 'SnippetType', $VariableDecorator)"/>
		<xsl:variable name="conditionFragment">
			<xsl:call-template name="ConditionalMatchCondition"/>
		</xsl:variable>
		<xsl:variable name="condition" select="exsl:node-set($conditionFragment)/child::*"/>
		<xsl:if test="child::cvg:Snippet/@conditionalMatch='IsBinaryLeadReading'">
			<plx:local name="isBinaryLeadReading" dataTypeName=".boolean">
				<plx:initialize>
					<plx:falseKeyword/>
				</plx:initialize>
			</plx:local>
			<plx:local name="columnIndex" dataTypeName=".i4">
				<plx:initialize>
					<plx:value data="-1" type="i4"/>
				</plx:initialize>
			</plx:local>
			<plx:loop>
				<plx:initializeLoop>
					<plx:local name="ConstraintIter1" dataTypeName=".i4">
						<plx:initialize>
							<plx:value data="0" type="i4"/>
						</plx:initialize>
					</plx:local>
				</plx:initializeLoop>
				<plx:condition>
					<plx:binaryOperator type="lessThan">
						<plx:left>
							<plx:nameRef name="ConstraintIter1"/>
						</plx:left>
						<plx:right>
							<plx:nameRef name="constraintRoleArity"/>
						</plx:right>
					</plx:binaryOperator>
				</plx:condition>
				<plx:beforeLoop>
					<plx:increment>
						<plx:nameRef name="ConstraintIter1"/>
					</plx:increment>
				</plx:beforeLoop>
				<plx:local name="roleArity" dataTypeName=".i4">
					<plx:initialize>
						<plx:callInstance name="Count" type="property">
							<plx:callObject>
								<plx:callInstance name=".implied" type="arrayIndexer">
									<plx:callObject>
										<plx:nameRef name="allConstraintSequences"/>
									</plx:callObject>
									<plx:passParam>
										<plx:nameRef name="ConstraintIter1"/>
									</plx:passParam>
								</plx:callInstance>
							</plx:callObject>
						</plx:callInstance>
					</plx:initialize>
				</plx:local>
				<plx:loop>
					<plx:initializeLoop>
						<plx:local name="RoleIter1" dataTypeName=".i4">
							<plx:initialize>
								<plx:value data="0" type="i4"/>
							</plx:initialize>
						</plx:local>
					</plx:initializeLoop>
					<plx:condition>
						<plx:binaryOperator type="lessThan">
							<plx:left>
								<plx:nameRef name="RoleIter1"/>
							</plx:left>
							<plx:right>
								<plx:nameRef name="roleArity"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:beforeLoop>
						<plx:increment>
							<plx:nameRef name="RoleIter1"/>
						</plx:increment>
					</plx:beforeLoop>
					<plx:local name="primaryRole1" dataTypeName="RoleBase">
						<plx:initialize>
							<plx:callInstance name=".implied" type="indexerCall">
								<plx:callObject>
									<plx:callInstance name=".implied" type="arrayIndexer">
										<plx:callObject>
											<plx:nameRef name="allConstraintSequences"/>
										</plx:callObject>
										<plx:passParam>
											<plx:nameRef name="ConstraintIter1"/>
										</plx:passParam>
									</plx:callInstance>
								</plx:callObject>
								<plx:passParam>
									<plx:nameRef name="RoleIter1"/>
								</plx:passParam>
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
									<plx:nameRef name="primaryRole1"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:right>
					</plx:assign>
					<plx:local name="factRoles1" dataTypeName="LinkedElementCollection">
						<plx:passTypeParam dataTypeName="RoleBase"/>
						<plx:initialize>
							<plx:callInstance name="RoleCollection" type="property">
								<plx:callObject>
									<plx:nameRef name="parentFact"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:initialize>
					</plx:local>
					<plx:branch>
						<plx:condition>
							<plx:binaryOperator type="booleanOr">
								<plx:left>
									<plx:binaryOperator type="inequality">
										<plx:left>
											<plx:callInstance name="Count" type="property">
												<plx:callObject>
													<plx:nameRef name="factRoles1"/>
												</plx:callObject>
											</plx:callInstance>
										</plx:left>
										<plx:right>
											<plx:value data="0" type="i4"/>
										</plx:right>
									</plx:binaryOperator>
								</plx:left>
								<plx:right>
									<plx:callInstance name="HasValue" type="property">
										<plx:callObject>
											<plx:callStatic name="GetUnaryRoleIndex"  dataTypeName="FactType">
												<plx:passParam>
													<plx:nameRef name="factRoles1"/>
												</plx:passParam>
											</plx:callStatic>
										</plx:callObject>
									</plx:callInstance>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:break/>
					</plx:branch>
					<plx:local name="readingOrders1" dataTypeName="LinkedElementCollection">
						<plx:passTypeParam dataTypeName="ReadingOrder"/>
						<plx:initialize>
							<plx:callInstance name="ReadingOrderCollection" type="property">
								<plx:callObject>
									<plx:nameRef name="parentFact"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:initialize>
					</plx:local>
					<plx:local name="reading1" dataTypeName="IReading">
						<plx:initialize>
							<plx:callInstance name="GetMatchingReading" type="methodCall">
								<plx:callObject>
									<plx:nameRef name="parentFact"/>
								</plx:callObject>
								<plx:passParam>
									<plx:nameRef name="readingOrders1"/>
								</plx:passParam>
								<plx:passParam>
									<plx:nullKeyword/>
								</plx:passParam>
								<plx:passParam>
									<plx:nameRef name="primaryRole1"/>
								</plx:passParam>
								<plx:passParam>
									<plx:nullKeyword/>
								</plx:passParam>
								<plx:passParam>
									<plx:falseKeyword/>
								</plx:passParam>
								<plx:passParam>
									<plx:falseKeyword/>
								</plx:passParam>
								<plx:passParam>
									<plx:nameRef name="factRoles1"/>
								</plx:passParam>
								<plx:passParam>
									<plx:falseKeyword/>
								</plx:passParam>
							</plx:callInstance>
						</plx:initialize>
					</plx:local>
					<plx:branch>
						<plx:condition>
							<plx:binaryOperator type="booleanAnd">
								<plx:left>
									<plx:binaryOperator type="identityInequality">
										<plx:left>
											<plx:nameRef name="reading1"/>
										</plx:left>
										<plx:right>
											<plx:nullKeyword/>
										</plx:right>
									</plx:binaryOperator>
								</plx:left>
								<plx:right>
									<plx:binaryOperator type="identityEquality">
										<plx:left>
											<plx:nameRef name="ConstraintIter1"/>
										</plx:left>
										<plx:right>
											<plx:value data="0" type="i4"/>
										</plx:right>
									</plx:binaryOperator>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:assign>
							<plx:left>
								<plx:nameRef name="columnIndex"/>
							</plx:left>
							<plx:right>
								<plx:nameRef name="RoleIter1"/>
							</plx:right>
						</plx:assign>
					</plx:branch>
					<plx:alternateBranch>
						<plx:condition>
							<plx:binaryOperator type="booleanAnd">
								<plx:left>
									<plx:binaryOperator type="identityInequality">
										<plx:left>
											<plx:nameRef name="reading1"/>
										</plx:left>
										<plx:right>
											<plx:nullKeyword/>
										</plx:right>
									</plx:binaryOperator>
								</plx:left>
								<plx:right>
									<plx:binaryOperator type="identityEquality">
										<plx:left>
											<plx:nameRef name="ConstraintIter1"/>
										</plx:left>
										<plx:right>
											<plx:value data="1" type="i4"/>
										</plx:right>
									</plx:binaryOperator>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:assign>
							<plx:left>
								<plx:nameRef name="isBinaryLeadReading"/>
							</plx:left>
							<plx:right>
								<plx:binaryOperator type="identityEquality">
									<plx:left>
										<plx:nameRef name="columnIndex"/>
									</plx:left>
									<plx:right>
										<plx:nameRef name="RoleIter1"/>
									</plx:right>
								</plx:binaryOperator>
							</plx:right>
						</plx:assign>
					</plx:alternateBranch>
				</plx:loop>
			</plx:loop>
			<plx:loop>
				<plx:initializeLoop>
					<plx:local name="ConstraintIter1" dataTypeName=".i4">
						<plx:initialize>
							<plx:value data="0" type="i4"/>
						</plx:initialize>
					</plx:local>
				</plx:initializeLoop>
				<plx:condition>
					<plx:binaryOperator type="lessThan">
						<plx:left>
							<plx:nameRef name="ConstraintIter1"/>
						</plx:left>
						<plx:right>
							<plx:nameRef name="constraintRoleArity"/>
						</plx:right>
					</plx:binaryOperator>
				</plx:condition>
				<plx:beforeLoop>
					<plx:increment>
						<plx:nameRef name="ConstraintIter1"/>
					</plx:increment>
				</plx:beforeLoop>
				<plx:local name="roleArity" dataTypeName=".i4">
					<plx:initialize>
						<plx:callInstance name="Count" type="property">
							<plx:callObject>
								<plx:callInstance name=".implied" type="arrayIndexer">
									<plx:callObject>
										<plx:nameRef name="allConstraintSequences"/>
									</plx:callObject>
									<plx:passParam>
										<plx:nameRef name="ConstraintIter1"/>
									</plx:passParam>
								</plx:callInstance>
							</plx:callObject>
						</plx:callInstance>
					</plx:initialize>
				</plx:local>
				<plx:loop>
					<plx:initializeLoop>
						<plx:local name="RoleIter1" dataTypeName=".i4">
							<plx:initialize>
								<plx:value data="0" type="i4"/>
							</plx:initialize>
						</plx:local>
					</plx:initializeLoop>
					<plx:condition>
						<plx:binaryOperator type="lessThan">
							<plx:left>
								<plx:nameRef name="RoleIter1"/>
							</plx:left>
							<plx:right>
								<plx:nameRef name="roleArity"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:beforeLoop>
						<plx:increment>
							<plx:nameRef name="RoleIter1"/>
						</plx:increment>
					</plx:beforeLoop>
					<plx:assign>
						<plx:left>
							<plx:nameRef name="factRoles"/>
						</plx:left>
						<plx:right>
							<plx:callInstance name="RoleCollection" type="property">
								<plx:callObject>
									<plx:callInstance name="FactType" type="property">
										<plx:callObject>
											<plx:callInstance name=".implied" type="indexerCall">
												<plx:callObject>
													<plx:callInstance name=".implied" type="indexerCall">
														<plx:callObject>
															<plx:nameRef name="allConstraintSequences"/>
														</plx:callObject>
														<plx:passParam>
															<plx:nameRef name="ConstraintIter1"/>
														</plx:passParam>
													</plx:callInstance>
												</plx:callObject>
												<plx:passParam>
													<plx:nameRef name="RoleIter1"/>
												</plx:passParam>
											</plx:callInstance>
										</plx:callObject>
									</plx:callInstance>
								</plx:callObject>
							</plx:callInstance>
						</plx:right>
					</plx:assign>
					<plx:local name="factCount" dataTypeName=".i4">
						<plx:initialize>
							<plx:callInstance name="Count" type="property">
								<plx:callObject>
									<plx:nameRef name="factRoles"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:initialize>
					</plx:local>
					<plx:loop>
						<plx:initializeLoop>
							<plx:local name="FactIter1" dataTypeName=".i4">
								<plx:initialize>
									<plx:value data="0" type="i4"/>
								</plx:initialize>
							</plx:local>
						</plx:initializeLoop>
						<plx:condition>
							<plx:binaryOperator type="lessThan">
								<plx:left>
									<plx:nameRef name="FactIter1"/>
								</plx:left>
								<plx:right>
									<plx:nameRef name="factCount"/>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:beforeLoop>
							<plx:increment>
								<plx:nameRef name="FactIter1"/>
							</plx:increment>
						</plx:beforeLoop>
						<plx:assign>
							<plx:left>
								<plx:nameRef name="parentFact"/>
							</plx:left>
							<plx:right>
								<plx:callInstance name="FactType" type="property">
									<plx:callObject>
										<plx:callInstance name=".implied" type="indexerCall">
											<plx:callObject>
												<plx:nameRef name="factRoles" />
											</plx:callObject>
											<plx:passParam>
												<plx:nameRef name="FactIter1"/>
											</plx:passParam>
										</plx:callInstance>
									</plx:callObject>
								</plx:callInstance>
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
										<plx:falseKeyword/>
									</plx:passParam>
									<plx:passParam>
										<plx:falseKeyword/>
									</plx:passParam>
									<plx:passParam>
										<plx:nameRef name="factRoles"/>
									</plx:passParam>
									<plx:passParam>
										<plx:trueKeyword/>
									</plx:passParam>
								</plx:callInstance>
							</plx:right>
						</plx:assign>
						<plx:branch>
							<plx:condition>
								<plx:callStatic name="IsHyphenBound" dataTypeName="VerbalizationHyphenBinder">
									<plx:passParam>
										<plx:nameRef name="reading"/>
									</plx:passParam>
								</plx:callStatic>
							</plx:condition>
							<plx:assign>
								<plx:left>
									<plx:nameRef name="isBinaryLeadReading"/>
								</plx:left>
								<plx:right>
									<plx:falseKeyword/>
								</plx:right>
							</plx:assign>
							<plx:break/>
						</plx:branch>
					</plx:loop>
				</plx:loop>
			</plx:loop>
		</xsl:if>
		<xsl:for-each select="child::cvg:Snippet">
			<xsl:if test="position()=1">
				<xsl:call-template name="ProcessSnippetConditions">
					<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
					<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
					<xsl:with-param name="SnippetTypeVariable" select="$SnippetTypeVariable"/>
				</xsl:call-template>
			</xsl:if>
		</xsl:for-each>
		<xsl:call-template name="ProcessSnippet">
			<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
			<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
			<xsl:with-param name="TopLevel" select="$TopLevel"/>
			<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
			<xsl:with-param name="IteratorVariableName" select="$IteratorVariableName"/>
			<xsl:with-param name="ReadingFactTypeVariableName" select="$ReadingFactTypeVariableName"/>
			<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
			<xsl:with-param name="ReplacementContents" select="cvg:SnippetReplacements/child::*"/>
			<xsl:with-param name="SnippetTypeVariable" select="$SnippetTypeVariable"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template name="ProcessSnippetConditions">
		<xsl:param name="VariableDecorator"/>
		<xsl:param name="VariablePrefix"/>
		<xsl:param name="SnippetTypeVariable"/>
		<xsl:param name="fallback" select="false()"/>
		<xsl:variable name="conditionFragment">
			<xsl:call-template name="ConditionalMatchCondition"/>
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
							<xsl:call-template name="SetSnippetVariable">
								<xsl:with-param name="SnippetType" select="@ref"/>
								<xsl:with-param name="VariableName" select="$SnippetTypeVariable"/>
							</xsl:call-template>
							<!--<xsl:apply-templates mode="ConstraintVerbalization" select="child::*">
								<xsl:with-param name="VariableDecorator" select="position()"/>
								<xsl:with-param name="VariablePrefix" select="'variableSnippet'"/>
								<xsl:with-param name="TopLevel" select="false()"/>
								<xsl:with-param name="PatternGroup"/>
							</xsl:apply-templates>-->
						</plx:alternateBranch>
					</xsl:when>
					<xsl:otherwise>
						<plx:fallbackBranch>
							<xsl:call-template name="SetSnippetVariable">
								<xsl:with-param name="SnippetType" select="@ref"/>
								<xsl:with-param name="VariableName" select="$SnippetTypeVariable"/>
							</xsl:call-template>
							<!--<xsl:apply-templates mode="ConstraintVerbalization" select="child::*">
								<xsl:with-param name="VariableDecorator" select="position()"/>
								<xsl:with-param name="VariablePrefix" select="'variableSnippet'"/>
								<xsl:with-param name="TopLevel" select="false()"/>
								<xsl:with-param name="PatternGroup"/>
							</xsl:apply-templates>-->
						</plx:fallbackBranch>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<xsl:otherwise>
				<xsl:choose>
					<xsl:when test="$condition">
						<plx:local name="{$SnippetTypeVariable}" dataTypeName="{$VerbalizationTextSnippetType}">
							<plx:initialize>
								<plx:value data="0" type="i4"/>
							</plx:initialize>
						</plx:local>
						<plx:branch>
							<plx:condition>
								<xsl:copy-of select="$condition"/>
							</plx:condition>
							<xsl:call-template name="SetSnippetVariable">
								<xsl:with-param name="SnippetType" select="@ref"/>
								<xsl:with-param name="VariableName" select="$SnippetTypeVariable"/>
							</xsl:call-template>
							<!--<xsl:apply-templates mode="ConstraintVerbalization" select="child::*">
								<xsl:with-param name="VariableDecorator" select="position()"/>
								<xsl:with-param name="VariablePrefix" select="'variableSnippet'"/>
								<xsl:with-param name="TopLevel" select="false()"/>
								<xsl:with-param name="PatternGroup"/>
							</xsl:apply-templates>-->
						</plx:branch>
						<xsl:for-each select="following-sibling::cvg:Snippet">
							<xsl:call-template name="ProcessSnippetConditions">
								<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
								<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
								<xsl:with-param name="SnippetTypeVariable" select="$SnippetTypeVariable"/>
								<xsl:with-param name="fallback" select="true()"/>
							</xsl:call-template>
						</xsl:for-each>
					</xsl:when>
				</xsl:choose>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="cvg:ConditionalReplacement" mode="ConstraintVerbalization">
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="'snippet'"/>
		<xsl:param name="TopLevel" select="false()"/>
		<xsl:param name="IteratorContext" select="''"/>
		<xsl:param name="IteratorVariableName" select="''"/>
		<xsl:param name="ReadingFactTypeVariableName" select="''"/>
		<xsl:param name="PatternGroup"/>
		<xsl:for-each select="*">
			<xsl:if test="position()=1">
				<xsl:call-template name="ProcessConditionalReplacements">
					<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
					<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
					<xsl:with-param name="TopLevel" select="$TopLevel"/>
					<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
					<xsl:with-param name="IteratorVariableName" select="$IteratorVariableName"/>
					<xsl:with-param name="ReadingFactTypeVariableName" select="$ReadingFactTypeVariableName"/>
					<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
				</xsl:call-template>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>

	<xsl:template name="ProcessConditionalReplacements">
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="'snippet'"/>
		<xsl:param name="TopLevel" select="false()"/>
		<xsl:param name="IteratorContext" select="''"/>
		<xsl:param name="IteratorVariableName" select="''"/>
		<xsl:param name="ReadingFactTypeVariableName" select="''"/>
		<xsl:param name="PatternGroup"/>
		<xsl:param name="fallback" select="false()"/>
		<xsl:variable name="conditionFragment">
			<xsl:call-template name="ConditionalMatchCondition"/>
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
								<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
								<xsl:with-param name="TopLevel" select="$TopLevel"/>
								<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
								<xsl:with-param name="IteratorVariableName" select="$IteratorVariableName"/>
								<xsl:with-param name="ReadingFactTypeVariableName" select="$ReadingFactTypeVariableName"/>
								<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
								<xsl:with-param name="ConditionalMatch" select="''"/>
							</xsl:apply-templates>
						</plx:alternateBranch>
					</xsl:when>
					<xsl:otherwise>
						<plx:fallbackBranch>
							<xsl:apply-templates select="." mode="ConstraintVerbalization">
								<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
								<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
								<xsl:with-param name="TopLevel" select="$TopLevel"/>
								<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
								<xsl:with-param name="IteratorVariableName" select="$IteratorVariableName"/>
								<xsl:with-param name="ReadingFactTypeVariableName" select="$ReadingFactTypeVariableName"/>
								<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
								<xsl:with-param name="ConditionalMatch" select="''"/>
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
								<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
								<xsl:with-param name="TopLevel" select="$TopLevel"/>
								<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
								<xsl:with-param name="IteratorVariableName" select="$IteratorVariableName"/>
								<xsl:with-param name="ReadingFactTypeVariableName" select="$ReadingFactTypeVariableName"/>
								<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
								<xsl:with-param name="ConditionalMatch" select="''"/>
							</xsl:apply-templates>
						</plx:branch>
						<xsl:for-each select="following-sibling::cvg:*">
							<xsl:call-template name="ProcessConditionalReplacements">
								<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
								<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
								<xsl:with-param name="TopLevel" select="$TopLevel"/>
								<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
								<xsl:with-param name="IteratorVariableName" select="$IteratorVariableName"/>
								<xsl:with-param name="ReadingFactTypeVariableName" select="$ReadingFactTypeVariableName"/>
								<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
								<xsl:with-param name="ConditionalMatch" select="''"/>
								<xsl:with-param name="fallback" select="true()"/>
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
		<plx:local name="unaryRoleIndex" dataTypeName="Nullable">
			<plx:passTypeParam dataTypeName=".i4"/>
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
				<plx:assign>
					<plx:left>
						<plx:nameRef name="factRoles"/>
					</plx:left>
					<plx:right>
						<plx:callInstance name="RoleCollection" type="property">
							<plx:callObject>
								<plx:nameRef name="parentFact"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:right>
				</plx:assign>
				<plx:assign>
					<plx:left>
						<plx:nameRef name="unaryRoleIndex"/>
					</plx:left>
					<plx:right>
						<xsl:call-template name="InitializeUnaryRoleIndex"/>
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
		<xsl:param name="VariablePrefix" select="'snippet'"/>
		<xsl:param name="TopLevel" select="false()"/>
		<xsl:param name="IteratorContext" select="''"/>
		<xsl:param name="IteratorVariableName" select="''"/>
		<xsl:param name="ReadingFactTypeVariableName" select="''"/>
		<xsl:param name="PatternGroup"/>
		<xsl:param name="ReplacementContents" select="child::*"/>
		<xsl:param name="SnippetTypeVariable" select="''"/>
		<xsl:param name="ConditionalMatch" select="@conditionalMatch"/>
		<xsl:variable name="byPassTopLevel" select="boolean(@byPassTopLevel)"/>
		<xsl:variable name="conditionFragment">
			<xsl:if test="$TopLevel and string-length($ConditionalMatch)">
				<xsl:call-template name="ConditionalMatchCondition">
					<xsl:with-param name="ConditionalMatch" select="$ConditionalMatch"/>
				</xsl:call-template>
			</xsl:if>
		</xsl:variable>
		<xsl:variable name="condition" select="exsl:node-set($conditionFragment)/child::*"/>
		<xsl:if test="$condition">
			<xsl:text disable-output-escaping="yes"><![CDATA[<plx:branch><plx:condition>]]></xsl:text>
			<xsl:copy-of select="$condition"/>
			<xsl:text disable-output-escaping="yes"><![CDATA[</plx:condition>]]></xsl:text>
			<xsl:call-template name="ConditionalBlockContext"/>
		</xsl:if>
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

		<plx:local name="{$VariablePrefix}{$FormatVariablePart}{$VariableDecorator}" dataTypeName=".string">
			<plx:initialize>
				<xsl:call-template name="SnippetFor">
					<xsl:with-param name="SnippetType" select="@ref"/>
					<xsl:with-param name="VariableName" select="$SnippetTypeVariable"/>
				</xsl:call-template>
			</plx:initialize>
		</plx:local>
		<xsl:for-each select="$ReplacementContents">
			<plx:local name="{$VariablePrefix}{$VariableDecorator}{$ReplaceVariablePart}{position()}" dataTypeName=".string">
				<plx:initialize>
					<plx:nullKeyword/>
				</plx:initialize>
			</plx:local>
			<xsl:apply-templates select="."  mode="ConstraintVerbalization">
				<xsl:with-param name="VariablePrefix" select="concat($VariablePrefix,$VariableDecorator,$ReplaceVariablePart)"/>
				<!-- The position will jump back to 1 with this call, so pick up the real position before jumping -->
				<xsl:with-param name="VariableDecorator" select="position()"/>
				<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
				<xsl:with-param name="IteratorVariableName" select="$IteratorVariableName"/>
				<xsl:with-param name="ReadingFactTypeVariableName" select="$ReadingFactTypeVariableName"/>
				<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
			</xsl:apply-templates>
		</xsl:for-each>
		<xsl:if test="not($byPassTopLevel)">
			<xsl:variable name="formatCall">
				<plx:callStatic name="Format" dataTypeName=".string">
					<plx:passParam>
						<plx:callInstance name="FormatProvider" type="property">
							<plx:callObject>
								<plx:nameRef type="parameter" name="writer"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:passParam>
					<plx:passParam>
						<plx:nameRef name="{$VariablePrefix}{$FormatVariablePart}{$VariableDecorator}"/>
					</plx:passParam>
					<xsl:choose>
						<xsl:when test="$ReplacementContents">
							<xsl:for-each select="$ReplacementContents">
								<plx:passParam>
									<plx:nameRef name="{$VariablePrefix}{$VariableDecorator}{$ReplaceVariablePart}{position()}"/>
								</plx:passParam>
							</xsl:for-each>
						</xsl:when>
						<xsl:otherwise>
							<xsl:for-each select="child::*">
								<plx:passParam>
									<plx:nameRef name="{$VariablePrefix}{$VariableDecorator}{$ReplaceVariablePart}{position()}"/>
								</plx:passParam>
							</xsl:for-each>
						</xsl:otherwise>
					</xsl:choose>
				</plx:callStatic>
			</xsl:variable>
			<xsl:choose>
				<xsl:when test="$TopLevel">
					<!-- Write the snippet directly to the text writer after sentence modification -->
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
				</xsl:when>
				<xsl:otherwise>
					<!-- Snippet is used as a replacement field in another snippet -->
					<plx:assign>
						<plx:left>
							<plx:nameRef name="{$VariablePrefix}{$VariableDecorator}"/>
						</plx:left>
						<plx:right>
							<xsl:copy-of select="$formatCall"/>
						</plx:right>
					</plx:assign>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="$condition">
			<xsl:text disable-output-escaping="yes"><![CDATA[</plx:branch>]]></xsl:text>
		</xsl:if>
	</xsl:template>
	<xsl:template match="cvg:PortableDataType" mode="ConstraintVerbalization">
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="'factText'"/>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="{$VariablePrefix}{$VariableDecorator}"/>
			</plx:left>
			<plx:right>
				<plx:callInstance name="ToString" type="methodCall">
					<plx:callObject>
						<plx:callThis name="DataType" type="property"/>
					</plx:callObject>
				</plx:callInstance>
			</plx:right>
		</plx:assign>
	</xsl:template>
	<xsl:template match="cvg:ValueRangeValueTypeName" mode="ConstraintVerbalization">
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="'factText'"/>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="{$VariablePrefix}{$VariableDecorator}"/>
			</plx:left>
			<plx:right>
				<plx:callInstance name="Name" type="property">
					<plx:callObject>
						<plx:callThis name="ValueType" type="property"/>
					</plx:callObject>
				</plx:callInstance>
			</plx:right>
		</plx:assign>
	</xsl:template>
	<xsl:template match="cvg:ObjectTypeName" mode="ConstraintVerbalization">
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="'factText'"/>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="{$VariablePrefix}{$VariableDecorator}"/>
			</plx:left>
			<plx:right>
					<plx:callThis name="Name" type="property" />
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
				<plx:callInstance name="Name" type="property">
					<plx:callObject>
						<plx:callThis name="Subtype" type="property"/>
					</plx:callObject>
				</plx:callInstance>
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
				<plx:callInstance name="Name" type="property">
					<plx:callObject>
						<plx:callThis name="Supertype" type="property"/>
					</plx:callObject>
				</plx:callInstance>
			</plx:right>
		</plx:assign>
	</xsl:template>
	<xsl:template match="cvg:ReferenceMode" mode="ConstraintVerbalization">
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="'factText'"/>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="{$VariablePrefix}{$VariableDecorator}"/>
			</plx:left>
			<plx:right>
				<plx:callThis name="ReferenceModeString" type="property"/>
			</plx:right>
		</plx:assign>
	</xsl:template>
	<xsl:template match="cvg:RoleName" mode="ConstraintVerbalization">
		<xsl:param name="VariableDecorator"/>
		<xsl:param name="VariablePrefix"/>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="{$VariablePrefix}{$VariableDecorator}"/>
			</plx:left>
			<plx:right>
				<plx:callInstance name="Name" type="property">
					<plx:callObject>
						<plx:callInstance name="Role" type="property">
							<plx:callObject>
								<plx:nameRef name="primaryRole"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:callObject>
				</plx:callInstance>
			</plx:right>
		</plx:assign>
	</xsl:template>
	<xsl:template match="cvg:RolePlayer" mode="ConstraintVerbalization">
		<xsl:param name="VariableDecorator"/>
		<xsl:param name="VariablePrefix"/>
		<xsl:param name="IteratorVariableName"/>
		<xsl:param name="PatternGroup"/>
		<xsl:param name="ListStyle" select="'null'"/>
		<xsl:param name="ConditionalMatch" select="'null'"/>
		<xsl:param name="ContextMatch"/>
		<xsl:choose>
			<xsl:when test="not($ListStyle='null') and not(@pass)">
				<xsl:variable name="iterVarName" select="concat($VariablePrefix,$FactRoleIterVariablePart,$VariableDecorator)"/>
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
								<xsl:choose>
									<xsl:when test="boolean(parent::cvg:IterateSequenceFacts)">
										<plx:nameRef name="currentRoleCount"/>
									</xsl:when>
									<xsl:otherwise>
										<plx:nameRef name="factArity"/>
									</xsl:otherwise>
								</xsl:choose>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:beforeLoop>
						<plx:increment>
							<plx:nameRef name="{$iterVarName}"/>
						</plx:increment>
					</plx:beforeLoop>
					<xsl:call-template name="PopulateListSnippet">
						<xsl:with-param name="IteratorVariableName" select="$iterVarName"/>
						<xsl:with-param name="ListStyle" select="$ListStyle"/>
						<xsl:with-param name="IteratorBound">
							<plx:nameRef name="currentRoleCount"/>
						</xsl:with-param>
					</xsl:call-template>
					<xsl:choose>
						<xsl:when test="$ContextMatch='included' or $ContextMatch='constraintRoles'">
							<plx:branch>
								<plx:condition>
									<plx:callInstance name="Contains" type="methodCall">
										<plx:callObject>
											<plx:nameRef name="includedSequenceRoles"/>
										</plx:callObject>
										<plx:passParam>
											<plx:callInstance name="Role" type="property">
												<plx:callObject>
													<plx:callInstance name=".implied" type="indexerCall">
														<plx:callObject>
															<plx:nameRef name="factRoles"/>
														</plx:callObject>
														<plx:passParam>
															<plx:nameRef name="{$iterVarName}"/>
														</plx:passParam>
													</plx:callInstance>
												</plx:callObject>
											</plx:callInstance>
										</plx:passParam>
									</plx:callInstance>
								</plx:condition>
								<plx:branch>
									<plx:condition>
										<plx:binaryOperator type="booleanOr">
											<plx:left>
												<plx:binaryOperator type="booleanAnd">
													<plx:left>
														<plx:nameRef name="isFirstAppend"/>
													</plx:left>
													<plx:right>
														<plx:binaryOperator type="identityEquality">
															<plx:left>
																<plx:nameRef name="listSnippet"/>
															</plx:left>
															<plx:right>
																<plx:callInstance name="{concat($ListStyle,'Open')}" type="property">
																	<plx:callObject>
																		<plx:nameRef name="CoreVerbalizationSnippetType" type="local"/>
																	</plx:callObject>
																</plx:callInstance>
															</plx:right>
														</plx:binaryOperator>
													</plx:right>
												</plx:binaryOperator>
											</plx:left>
											<plx:right>
												<plx:unaryOperator type="booleanNot">
													<plx:nameRef name="isFirstAppend"/>
												</plx:unaryOperator>
											</plx:right>
										</plx:binaryOperator>
									</plx:condition>
									<plx:callInstance name="Append" type="methodCall">
										<plx:callObject>
											<plx:nameRef name="sbTemp"/>
										</plx:callObject>
										<plx:passParam>
											<plx:callInstance name="GetSnippet" type="methodCall">
												<plx:callObject>
													<plx:nameRef name="snippets"/>
												</plx:callObject>
												<plx:passParam>
													<plx:nameRef name="listSnippet"/>
												</plx:passParam>
											</plx:callInstance>
										</plx:passParam>
									</plx:callInstance>
									<plx:assign>
										<plx:left>
											<plx:nameRef name="isFirstAppend"/>
										</plx:left>
										<plx:right>
											<plx:falseKeyword/>
										</plx:right>
									</plx:assign>
								</plx:branch>
								<plx:callInstance name="Append" type="methodCall">
									<plx:callObject>
										<plx:nameRef name="sbTemp"/>
									</plx:callObject>
									<plx:passParam>
										<plx:callInstance name=".implied" type="indexerCall">
											<plx:callObject>
												<plx:callInstance name=".implied" type="indexerCall">
													<plx:callObject>
														<plx:nameRef name="allBasicRoleReplacements"/>
													</plx:callObject>
													<plx:passParam>
														<plx:callInstance name="IndexOf" type="methodCall">
															<plx:callObject>
																<plx:nameRef name="allFacts" type="local"/>
															</plx:callObject>
															<plx:passParam>
																<plx:callInstance name="FactType" type="property">
																	<plx:callObject>
																		<plx:callInstance name=".implied" type="arrayIndexer">
																			<plx:callObject>
																				<plx:nameRef name="includedSequenceRoles"/>
																			</plx:callObject>
																			<plx:passParam>
																				<plx:nameRef name="{$IteratorVariableName}"/>
																			</plx:passParam>
																		</plx:callInstance>
																	</plx:callObject>
																</plx:callInstance>
															</plx:passParam>
														</plx:callInstance>
													</plx:passParam>
												</plx:callInstance>
											</plx:callObject>
											<plx:passParam>
												<plx:callInstance name="IndexOf" type="methodCall">
													<plx:callObject>
														<plx:callInstance name="RoleCollection" type="property">
															<plx:callObject>
																<plx:callInstance name="FactType" type="property">
																	<plx:callObject>
																		<plx:callInstance name=".implied" type="indexerCall">
																			<plx:callObject>
																				<plx:nameRef name="includedSequenceRoles"/>
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
													<plx:passParam>
														<plx:callInstance name="Role" type="property">
															<plx:callObject>
																<plx:callInstance name=".implied" type="indexerCall">
																	<plx:callObject>
																		<plx:nameRef name="factRoles"/>
																	</plx:callObject>
																	<plx:passParam>
																		<plx:nameRef name="{$iterVarName}"/>
																	</plx:passParam>
																</plx:callInstance>
															</plx:callObject>
														</plx:callInstance>
													</plx:passParam>
												</plx:callInstance>
											</plx:passParam>
										</plx:callInstance>
									</plx:passParam>
								</plx:callInstance>
							</plx:branch>
						</xsl:when>
					</xsl:choose>
				</plx:loop>
			</xsl:when>
			<xsl:when test="$PatternGroup='SetComparisonConstraint' and (@pass='first' or @pass='notFirst')">
				<plx:local name="rolePlayer" dataTypeName="RoleBase">
					<plx:initialize>
						<plx:nullKeyword/>
					</plx:initialize>
				</plx:local>
				<plx:loop>
					<plx:initializeLoop>
						<plx:local name="RoleIter" dataTypeName=".i4">
							<plx:initialize>
								<plx:value type="i4" data="0"/>
							</plx:initialize>
						</plx:local>
					</plx:initializeLoop>
					<plx:condition>
						<plx:binaryOperator type="lessThan">
							<plx:left>
								<plx:nameRef name="RoleIter"/>
							</plx:left>
							<plx:right>
								<plx:nameRef name="currentRoleCount"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:beforeLoop>
						<plx:increment>
							<plx:nameRef name="RoleIter"/>
						</plx:increment>
					</plx:beforeLoop>
					<plx:local name="currentRole" dataTypeName="RoleBase">
						<plx:initialize>
							<plx:callInstance name=".implied" type="indexerCall">
								<plx:callObject>
									<plx:nameRef name="factRoles" type="local"/>
								</plx:callObject>
								<plx:passParam>
									<plx:nameRef name="RoleIter"/>
								</plx:passParam>
							</plx:callInstance>
						</plx:initialize>
					</plx:local>
					<plx:assign>
						<plx:left>
							<plx:nameRef name="reading" type="local"/>
						</plx:left>
						<plx:right>
							<plx:callInstance name="GetMatchingReading">
								<plx:callObject>
									<plx:nameRef name="currentFact"/>
								</plx:callObject>
								<plx:passParam>
									<plx:nameRef name="allReadingOrders"/>
								</plx:passParam>
								<plx:passParam>
									<plx:nullKeyword/>
								</plx:passParam>
								<plx:passParam>
									<plx:nameRef name="currentRole"/>
								</plx:passParam>
								<plx:passParam>
									<plx:nullKeyword/>
								</plx:passParam>
								<plx:passParam>
									<plx:falseKeyword/>
								</plx:passParam>
								<plx:passParam>
									<plx:falseKeyword/>
								</plx:passParam>
								<plx:passParam>
									<plx:nameRef name="factRoles"/>
								</plx:passParam>
								<plx:passParam>
									<plx:falseKeyword/>
								</plx:passParam>
							</plx:callInstance>
						</plx:right>
					</plx:assign>
					<plx:branch>
						<plx:condition>
							<plx:binaryOperator>
								<xsl:attribute name="type">
									<xsl:choose>
										<xsl:when test="@pass='first'">
											<xsl:text>identityInequality</xsl:text>
										</xsl:when>
										<xsl:otherwise>
											<xsl:text>identityEquality</xsl:text>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:attribute>
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
								<plx:nameRef name="rolePlayer"/>
							</plx:left>
							<plx:right>
								<plx:nameRef name="currentRole"/>
							</plx:right>
						</plx:assign>
						<plx:break/>
					</plx:branch>
				</plx:loop>
				<plx:assign>
					<plx:left>
						<plx:nameRef name="{$VariablePrefix}{$VariableDecorator}"/>
					</plx:left>
					<plx:right>
						<plx:callInstance name=".implied" type="indexerCall">
							<plx:callObject>
								<plx:callInstance name=".implied" type="indexerCall">
									<plx:callObject>
										<plx:nameRef name="allBasicRoleReplacements"/>
									</plx:callObject>
									<plx:passParam>
										<plx:callInstance name="IndexOf" type="methodCall">
											<plx:callObject>
												<plx:nameRef name="allFacts" type="local"/>
											</plx:callObject>
											<plx:passParam>
												<plx:callInstance name="FactType" type="property">
													<plx:callObject>
														<plx:callInstance name=".implied" type="arrayIndexer">
															<plx:callObject>
																<plx:nameRef name="includedSequenceRoles"/>
															</plx:callObject>
															<plx:passParam>
																<plx:nameRef name="{$IteratorVariableName}"/>
															</plx:passParam>
														</plx:callInstance>
													</plx:callObject>
												</plx:callInstance>
											</plx:passParam>
										</plx:callInstance>
									</plx:passParam>
								</plx:callInstance>
							</plx:callObject>
							<plx:passParam>
								<plx:callInstance name="IndexOf" type="methodCall">
									<plx:callObject>
										<plx:callInstance name="RoleCollection" type="property">
											<plx:callObject>
												<plx:callInstance name="FactType" type="property">
													<plx:callObject>
														<plx:callInstance name=".implied" type="indexerCall">
															<plx:callObject>
																<plx:nameRef name="includedSequenceRoles"/>
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
									<plx:passParam>
										<plx:nameRef name="rolePlayer"/>
									</plx:passParam>
								</plx:callInstance>
							</plx:passParam>
						</plx:callInstance>
					</plx:right>
				</plx:assign>
			</xsl:when>
			<xsl:otherwise>
				<plx:assign>
					<plx:left>
						<plx:nameRef name="{$VariablePrefix}{$VariableDecorator}"/>
					</plx:left>
					<plx:right>
						<xsl:choose>
							<xsl:when test="$PatternGroup='SetComparisonConstraint'">
								<plx:callInstance name=".implied" type="indexerCall">
									<plx:callObject>
										<plx:callInstance name=".implied" type="indexerCall">
											<plx:callObject>
												<plx:nameRef name="allBasicRoleReplacements"/>
											</plx:callObject>
											<plx:passParam>
												<plx:callInstance name="IndexOf" type="methodCall">
													<plx:callObject>
														<plx:nameRef name="allFacts" type="local"/>
													</plx:callObject>
													<plx:passParam>
														<plx:callInstance name="FactType" type="property">
															<plx:callObject>
																<plx:callInstance name=".implied" type="arrayIndexer">
																	<plx:callObject>
																		<plx:nameRef name="includedSequenceRoles"/>
																	</plx:callObject>
																	<plx:passParam>
																		<plx:nameRef name="{$IteratorVariableName}"/>
																	</plx:passParam>
																</plx:callInstance>
															</plx:callObject>
														</plx:callInstance>
													</plx:passParam>
												</plx:callInstance>
											</plx:passParam>
										</plx:callInstance>
									</plx:callObject>
									<plx:passParam>
										<plx:callInstance name="IndexOf" type="methodCall">
											<plx:callObject>
												<plx:callInstance name="RoleCollection" type="property">
													<plx:callObject>
														<plx:callInstance name="FactType" type="property">
															<plx:callObject>
																<plx:callInstance name=".implied" type="indexerCall">
																	<plx:callObject>
																		<plx:nameRef name="includedSequenceRoles"/>
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
											<plx:passParam>
												<plx:callInstance name=".implied" type="indexerCall">
													<plx:callObject>
														<plx:nameRef name="includedSequenceRoles"/>
													</plx:callObject>
													<plx:passParam>
														<xsl:choose>
															<xsl:when test="not($ConditionalMatch='null') or not(@pass)">
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
											</plx:passParam>
										</plx:callInstance>
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
					</plx:right>
				</plx:assign>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="cvg:ValueRangeValueTypeName" mode="ConstraintVerbalization">
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="'factText'"/>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="{$VariablePrefix}{$VariableDecorator}"/>
			</plx:left>
			<plx:right>
				<plx:callInstance name="Name" type="property">
					<plx:callObject>
						<plx:callThis name="ValueType" type="property"/>
					</plx:callObject>
				</plx:callInstance>
			</plx:right>
		</plx:assign>
	</xsl:template>
	<xsl:template match="cvg:RolePlayerReferenceMode" mode="ConstraintVerbalization">
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="'factText'"/>
		<plx:assign>
			<plx:left>
				<plx:nameRef name="{$VariablePrefix}{$VariableDecorator}"/>
			</plx:left>
			<plx:right>
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
			</plx:right>
		</plx:assign>
	</xsl:template>
	<xsl:template match="cvg:Fact" mode="ConstraintVerbalization" name="ProcessFact">
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="'factText'"/>
		<xsl:param name="PatternGroup"/>
		<xsl:param name="FirstPassVariable"/>
		<xsl:param name="IteratorContext" select="''"/>
		<xsl:param name="IteratorVariableName" select="''"/>
		<xsl:param name="ReadingFactTypeVariableName" select="''"/>
		<xsl:param name="TopLevel" select="false()"/>
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
		<xsl:variable name="complexReplacement" select="boolean(cvg:PredicateReplacement)"/>
		<xsl:call-template name="PopulateReading">
			<xsl:with-param name="ReadingChoice" select="@readingChoice"/>
			<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
			<xsl:with-param name="ReadingFactTypeVariableName" select="$ReadingFactTypeVariableName"/>
			<xsl:with-param name="ConditionalReadingOrderIndex">
				<xsl:if test="$IteratorContext='constraintRoles'">
					<xsl:value-of select="concat($RoleIterVariablePart,$VariableDecorator)"/>
				</xsl:if>
			</xsl:with-param>
		</xsl:call-template>
		<xsl:choose>
			<xsl:when test="$complexReplacement">
				<xsl:variable name="iterVarName" select="concat($VariablePrefix,$FactRoleIterVariablePart,$VariableDecorator)"/>
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
								<xsl:choose>
									<xsl:when test="boolean(parent::cvg:IterateSequenceFacts)">
										<plx:nameRef name="currentRoleCount"/>
									</xsl:when>
									<xsl:otherwise>
										<plx:nameRef name="factArity"/>
									</xsl:otherwise>
								</xsl:choose>
								
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:beforeLoop>
						<plx:increment>
							<plx:nameRef name="{$iterVarName}"/>
						</plx:increment>
					</plx:beforeLoop>
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
					<plx:local name="basicReplacement" dataTypeName=".string">
						<plx:initialize>
							<plx:callInstance name="HyphenBindRoleReplacement">
								<plx:callObject>
									<plx:nameRef name="hyphenBinder"/>
								</plx:callObject>
								<plx:passParam>
									<plx:callInstance name=".implied" type="arrayIndexer">
										<plx:callObject>
											<xsl:choose>
												<xsl:when test="$PatternGroup='InternalSetConstraint' or $PatternGroup='SetComparisonConstraint'">
													<plx:callInstance name=".implied" type="arrayIndexer">
														<plx:callObject>
															<plx:nameRef name="allBasicRoleReplacements"/>
														</plx:callObject>
														<plx:passParam>
															<xsl:choose>
																<xsl:when test="$PatternGroup='SetComparisonConstraint'">
																	<plx:callInstance name="IndexOf" type="methodCall">
																		<plx:callObject>
																			<plx:nameRef name="allFacts" type="local"/>
																		</plx:callObject>
																		<plx:passParam>
																			<plx:callInstance name="FactType" type="property">
																				<plx:callObject>
																					<plx:callInstance name=".implied" type="arrayIndexer">
																						<plx:callObject>
																							<plx:nameRef name="includedSequenceRoles"/>
																						</plx:callObject>
																						<plx:passParam>
																							<plx:nameRef name="{$IteratorVariableName}"/>
																						</plx:passParam>
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
										</plx:callObject>
										<plx:passParam>
											<plx:nameRef name="{$iterVarName}"/>
										</plx:passParam>
									</plx:callInstance>
								</plx:passParam>
								<plx:passParam>
									<plx:nameRef name="{$iterVarName}"/>
								</plx:passParam>
							</plx:callInstance>
						</plx:initialize>
					</plx:local>

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
												<xsl:with-param name="ReadingFactTypeVariableName" select="$ReadingFactTypeVariableName"/>
												<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
												<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
												<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
												<xsl:with-param name="FirstPassVariable" select="$FirstPassVariable"/>
											</xsl:call-template>
										</plx:condition>
										<xsl:call-template name="PredicateReplacementBody"/>
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
															<xsl:with-param name="ReadingFactTypeVariableName" select="$ReadingFactTypeVariableName"/>
															<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
															<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
															<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
															<xsl:with-param name="FirstPassVariable" select="$FirstPassVariable"/>
														</xsl:call-template>
													</plx:condition>
													<xsl:call-template name="PredicateReplacementBody"/>
												</plx:alternateBranch>
											</xsl:when>
											<xsl:otherwise>
												<plx:fallbackBranch>
													<xsl:call-template name="PredicateReplacementBody"/>
												</plx:fallbackBranch>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:for-each>
								</xsl:when>
								<xsl:otherwise>
									<xsl:call-template name="PredicateReplacementBody"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:if>
					</xsl:for-each>

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
								<plx:nameRef name="basicReplacement"/>
							</plx:right>
						</plx:assign>
					</plx:branch>
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
							<plx:nameRef name="roleReplacement"/>
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
					<plx:nameRef name="factRoles"/>
				</plx:passParam>
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
						<plx:nameRef name="{$VariablePrefix}{$VariableDecorator}"/>
					</plx:left>
					<plx:right>
						<xsl:copy-of select="$predicateText"/>
					</plx:right>
				</plx:assign>
			</xsl:otherwise>
		</xsl:choose>
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
			<xsl:when test="count($conditions)">
				<xsl:for-each select="$conditions">
					<xsl:if test="position()=1">
						<xsl:call-template name="CombineElements">
							<xsl:with-param name="OperatorType" select="'booleanAnd'"/>
						</xsl:call-template>
					</xsl:if>
				</xsl:for-each>
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
		<xsl:param name="PrimaryRole" select="'primaryRole'"/>
		<xsl:param name="CurrentRole" select="'currentRole'"/>
		<xsl:variable name="includedRolesFragment">
			<xsl:choose>
				<xsl:when test="$PatternGroup='InternalSetConstraint'">
					<xsl:text>allConstraintRoles</xsl:text>
				</xsl:when>
				<xsl:when test="$PatternGroup='SetComparisonConstraint' and $Match='included'">
					<xsl:text>includedSequenceRoles</xsl:text>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text>includedRoles</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="includedRoles" select="string($includedRolesFragment)"/>
		<xsl:variable name="operatorsFragment">
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
				<xsl:when test="$Match='included'">
					<xsl:choose>
						<xsl:when test="$IteratorContext='constraintRoles'">
							<!-- For the single column case, the included role is always a set consisting of the primary role only -->
							<plx:binaryOperator type="identityEquality">
								<plx:left>
									<plx:nameRef name="{$CurrentRole}"/>
								</plx:left>
								<plx:right>
									<plx:nameRef name="{$PrimaryRole}"/>
								</plx:right>
							</plx:binaryOperator>
						</xsl:when>
						<xsl:otherwise>
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
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:when test="$Match='excluded'">
					<xsl:choose>
						<xsl:when test="$IteratorContext='constraintRoles'">
							<!-- For the single column case, the included role is always a set consisting of the primary role only -->
							<plx:binaryOperator type="identityInequality">
								<plx:left>
									<plx:nameRef name="{$CurrentRole}"/>
								</plx:left>
								<plx:right>
									<plx:nameRef name="{$PrimaryRole}"/>
								</plx:right>
							</plx:binaryOperator>
						</xsl:when>
						<xsl:otherwise>
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
						</xsl:otherwise>
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
			<xsl:if test="@pass='first'">
				
			</xsl:if>
		</xsl:variable>
		<xsl:for-each select="exsl:node-set($operatorsFragment)/child::*">
			<xsl:if test="position()=1">
				<xsl:call-template name="CombineElements">
					<xsl:with-param name="OperatorType" select="'booleanAnd'"/>
				</xsl:call-template>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>
	<xsl:template name="PredicateReplacementBody">
		<xsl:for-each select="cvg:Snippet">
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
									<plx:nameRef name="basicReplacement"/>
								</plx:passParam>
							</plx:callStatic>
						</xsl:otherwise>
					</xsl:choose>
				</plx:right>
			</plx:assign>
		</xsl:for-each>
	</xsl:template>
	<xsl:template name="ConditionalMatchCondition">
		<xsl:variable name="ConditionalMatch" select="string(@conditionalMatch)"/>
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
				<xsl:when test="$ConditionalMatch='HasPreferredIdentifier'">
					<plx:binaryOperator type="identityInequality">
						<plx:left>
							<plx:callThis name="PreferredIdentifier" type="property"/>
						</plx:left>
						<plx:right>
							<plx:nullKeyword/>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='HasReferenceMode'">
					<plx:callThis name="HasReferenceMode" type="property"/>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='HasNotes'">
					<plx:unaryOperator type="booleanNot">
					<plx:callStatic name="IsNullOrEmpty" type="methodCall" dataTypeName=".string">
						<plx:passParam>
							<plx:callThis type="property" name="NoteText"/>
						</plx:passParam>
					</plx:callStatic>
					</plx:unaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='HasPortableDataType'">
					<plx:binaryOperator type="identityInequality">
						<plx:left>
							<plx:callThis name="DataType" type="property"/>
						</plx:left>
						<plx:right>
							<plx:nullKeyword/>
						</plx:right>
					</plx:binaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='IsBinaryLeadReading'">
					<plx:nameRef name="isBinaryLeadReading"/>
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
				<xsl:when test="$ConditionalMatch='IsEntityType'">
					<plx:unaryOperator type="booleanNot">
						<plx:callThis name="IsValueType" type="property"/>
					</plx:unaryOperator>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='IsPreferredIdentifier'">
					<plx:callThis type="property" name="IsPreferred"/>
				</xsl:when>
				<xsl:when test="$ConditionalMatch='IsSingleValue'">
					<plx:nameRef name="isSingleValue" type="local"/>
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
													<plx:nameRef name="valueRole" type="local"/>
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
	<!-- Provides the chance to write inline code inside conditional
		 snippet conditions -->
	<xsl:template name="ConditionalBlockContext">
		<xsl:variable name="blockContext" select="string(@conditionalBlockContext)"/>
		<xsl:if test="string-length($blockContext)">
			<xsl:choose>
				<xsl:when test="$blockContext='ObjectifiedFactType'">
					<xsl:call-template name="DeclareVariablesForFact">
						<xsl:with-param name="NestedFact" select="true()"/>
					</xsl:call-template>
					<xsl:call-template name="PopulateBasicRoleReplacements"/>
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
				<plx:callInstance name="ToString" type="methodCall">
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
		<xsl:param name="VariablePrefix" select="'factText'"/>
		<xsl:param name="ReadingFactTypeVariableName" select="''"/>
		<xsl:param name="CompositeCount"/>
		<xsl:param name="CompositeIterator"/>
		<xsl:param name="ListStyle" select="@listStyle"/>
		<xsl:param name="PatternGroup"/>
		<xsl:param name="SequenceIterator" select="false()"/>
		<xsl:call-template name="RoleIterator">
			<xsl:with-param name="TopLevel" select="$TopLevel"/>
			<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
			<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
			<xsl:with-param name="CompositeCount" select="$CompositeCount"/>
			<xsl:with-param name="CompositeIterator" select="$CompositeIterator"/>
			<xsl:with-param name="ReadingFactTypeVariableName" select="$ReadingFactTypeVariableName"/>
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
		<xsl:param name="VariablePrefix" select="'factText'"/>
		<xsl:param name="ReadingFactTypeVariableName" select="''"/>
		<xsl:param name="CompositeCount"/>
		<xsl:param name="CompositeIterator"/>
		<xsl:param name="ListStyle" select="@listStyle"/>
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
		<xsl:variable name="IteratorVarName" select="concat('SequenceIter', $VariableDecorator)"/>
		<!--Iterate through sequences as the outer loop of iterate roles-->
		<plx:loop>
			<plx:initializeLoop>
				<plx:local name="{$IteratorVarName}" dataTypeName=".i4">
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
				<plx:binaryOperator type="lessThan">
					<plx:left>
						<plx:nameRef name="{$IteratorVarName}"/>
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
					<plx:nameRef name="{$IteratorVarName}"/>
				</plx:increment>
			</plx:beforeLoop>
			<plx:local dataTypeName="IList" name="includedSequenceRoles">
				<plx:passTypeParam dataTypeName="Role"/>
				<plx:initialize>
					<plx:callInstance name=".implied" type="indexerCall">
						<plx:callObject>
							<plx:nameRef name="allConstraintSequences"/>
						</plx:callObject>
						<plx:passParam>
							<plx:nameRef name="{$IteratorVarName}"/>
						</plx:passParam>
					</plx:callInstance>
				</plx:initialize>
			</plx:local>
			<xsl:if test="descendant::cvg:*[not(@pass='first')]">
				<plx:local name="roleArity" dataTypeName=".i4">
					<plx:initialize>
						<plx:callInstance name="Count" type="property">
							<plx:callObject>
								<plx:nameRef name="includedSequenceRoles"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:initialize>
				</plx:local>
			</xsl:if>
			<xsl:if test="not($ListStyle='null')">
				<plx:local name="listSnippet" dataTypeName="{$VerbalizationTextSnippetType}"/>
				<xsl:call-template name="PopulateListSnippet">
					<xsl:with-param name="IteratorVariableName" select="$IteratorVarName"/>
					<xsl:with-param name="ListStyle" select="$ListStyle"/>
					<xsl:with-param name="IteratorBound">
						<plx:nameRef name="constraintRoleArity"/>
					</xsl:with-param>
				</xsl:call-template>
			</xsl:if>
			<xsl:choose>
				<xsl:when test="boolean(@conditionalMatch)">
					<xsl:apply-templates mode="ConstraintVerbalization" select="child::*">
						<xsl:with-param name="TopLevel" select="$TopLevel"/>
						<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
						<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
						<xsl:with-param name="CompositeCount" select="$CompositeCount"/>
						<xsl:with-param name="CompositeIterator" select="$CompositeIterator"/>
						<xsl:with-param name="ReadingFactTypeVariableName" select="$ReadingFactTypeVariableName"/>
						<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
						<xsl:with-param name="SequenceIterator" select="$IteratorVarName"/>
						<xsl:with-param name="ParentListStyle" select="$ListStyle"/>
						<xsl:with-param name="ConditionalMatch" select="@conditionalMatch"/>
					</xsl:apply-templates>
				</xsl:when>
				<xsl:otherwise>
					<xsl:apply-templates mode="ConstraintVerbalization" select="child::*">
						<xsl:with-param name="TopLevel" select="$TopLevel"/>
						<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
						<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
						<xsl:with-param name="CompositeCount" select="$CompositeCount"/>
						<xsl:with-param name="CompositeIterator" select="$CompositeIterator"/>
						<xsl:with-param name="ReadingFactTypeVariableName" select="$ReadingFactTypeVariableName"/>
						<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
						<xsl:with-param name="SequenceIterator" select="$IteratorVarName"/>
						<xsl:with-param name="ParentListStyle" select="$ListStyle"/>
					</xsl:apply-templates>
				</xsl:otherwise>
			</xsl:choose>
		</plx:loop>
		<xsl:if test="not($ListStyle='null') or boolean(child::*/cvg:RolePlayer) and (not(@conditionalMatch) and not(../@conditionalMatch))">
			<plx:assign>
				<plx:left>
					<plx:nameRef name="{$VariablePrefix}{$VariableDecorator}"/>
				</plx:left>
				<plx:right>
					<plx:callInstance name="ToString" type="methodCall">
						<plx:callObject>
							<plx:nameRef name="sbTemp"/>
						</plx:callObject>
					</plx:callInstance>
				</plx:right>
			</plx:assign>
		</xsl:if>
	</xsl:template>
	<!-- An IterateInstances tag is used to walk a set of instances and combine the verbalizations for
			those instances into a single list. The type of verbalization is not necessarily flexible, aside from
			allowing the user to rework the TextInstanceValue and NonTextInstanceValue snippets. -->
	<xsl:template match="cvg:IterateInstances" mode="ConstraintVerbalization">
		<xsl:param name="TopLevel" select="false()"/>
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="'factText'"/>
		<xsl:param name="ReadingFactTypeVariableName" select="''"/>
		<xsl:param name="CompositeCount"/>
		<xsl:param name="CompositeIterator"/>
		<xsl:param name="ListStyle" select="@listStyle"/>
		<xsl:param name="PatternGroup"/>
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
				<plx:callInstance name="Length" type="property">
					<plx:callObject>
						<plx:callThis name="Instances" type="property"/>
					</plx:callObject>
				</plx:callInstance>
			</plx:initialize>
		</plx:local>
		<xsl:call-template name="InstanceIterator">
			<xsl:with-param name="TopLevel" select="$TopLevel"/>
			<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
			<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
			<xsl:with-param name="CompositeCount" select="$CompositeCount"/>
			<xsl:with-param name="CompositeIterator" select="$CompositeIterator"/>
			<xsl:with-param name="ListStyle" select="$ListStyle"/>
			<xsl:with-param name="ReadingFactTypeVariableName" select="$ReadingFactTypeVariableName"/>
			<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
		</xsl:call-template>
	</xsl:template>
	<xsl:template match="cvg:IterateFacts" mode="ConstraintVerbalization">
		<xsl:param name="TopLevel" select="false()"/>
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="'factText'"/>
		<xsl:param name="ReadingFactTypeVariableName" select="''"/>
		<xsl:param name="CompositeCount"/>
		<xsl:param name="CompositeIterator"/>
		<xsl:param name="ListStyle" select="@listStyle"/>
		<xsl:param name="PatternGroup"/>
		<xsl:call-template name="RoleIterator">
			<xsl:with-param name="TopLevel" select="$TopLevel"/>
			<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
			<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
			<xsl:with-param name="CompositeCount" select="$CompositeCount"/>
			<xsl:with-param name="CompositeIterator" select="$CompositeIterator"/>
			<xsl:with-param name="ListStyle" select="$ListStyle"/>
			<xsl:with-param name="ReadingFactTypeVariableName" select="$ReadingFactTypeVariableName"/>
			<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
		</xsl:call-template>
	</xsl:template>
	<!-- An IterateSequenceFacts tag is used to build and iterate through a give Sequence's unique facts -->
	<xsl:template match="cvg:IterateSequenceFacts" mode="ConstraintVerbalization">
		<xsl:param name="TopLevel" select="false()"/>
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="'FactIter'"/>
		<xsl:param name="ReadingFactTypeVariableName" select="''"/>
		<xsl:param name="CompositeCount"/>
		<xsl:param name="CompositeIterator"/>
		<xsl:param name="ConditionalMatch" select="'null'"/>
		<xsl:param name="ListStyle" select="@listStyle"/>
		<xsl:param name="ParentListStyle" select="'null'"/>
		<xsl:param name="PatternGroup"/>
		<xsl:param name="SequenceIterator"/>
		<xsl:if test="position() = 1">
			<!-- Iterate through the current sequence's facts, and retrieve the unique facts of that collection -->
			<plx:comment>Iterate through the current sequence's fact, and retrieve the unique facts of that collection</plx:comment>
			<plx:local name="currentSequenceFactCount" dataTypeName=".i4">
				<plx:initialize>
					<plx:value data="0" type="i4"/>
				</plx:initialize>
			</plx:local>
			<plx:assign>
				<plx:left>
					<plx:nameRef name="currentSequenceFactCount"/>
				</plx:left>
				<plx:right>
					<plx:value data="0" type="i4"/>
				</plx:right>
			</plx:assign>
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
							<plx:nameRef name="roleArity"/>
						</plx:right>
					</plx:binaryOperator>
				</plx:condition>
				<plx:beforeLoop>
					<plx:increment>
						<plx:nameRef name="i"/>
					</plx:increment>
				</plx:beforeLoop>
				<plx:local name="currentFact" dataTypeName="FactType">
					<plx:initialize>
						<plx:callInstance name="FactType" type="property">
							<plx:callObject>
								<plx:callInstance name=".implied" type="indexerCall">
									<plx:callObject>
										<plx:nameRef name="includedSequenceRoles"/>
									</plx:callObject>
									<plx:passParam>
										<plx:nameRef name="i"/>
									</plx:passParam>
								</plx:callInstance>
							</plx:callObject>
						</plx:callInstance>
					</plx:initialize>
				</plx:local>
				<plx:local name="j" dataTypeName=".i4">
					<plx:initialize>
						<plx:value data="0" type="i4"/>
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
									<plx:nameRef name="currentFact"/>
								</plx:left>
								<plx:right>
									<plx:callInstance name="FactType" type="property">
										<plx:callObject>
											<plx:callInstance name=".implied" type="indexerCall">
												<plx:callObject>
													<plx:nameRef name="includedSequenceRoles"/>
												</plx:callObject>
												<plx:passParam>
													<plx:nameRef name="j"/>
												</plx:passParam>
											</plx:callInstance>
										</plx:callObject>
									</plx:callInstance>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:break/>
					</plx:branch>
				</plx:loop>
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="equality">
							<plx:left>
								<plx:nameRef name="j"/>
							</plx:left>
							<plx:right>
								<plx:nameRef name="i"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:increment>
						<plx:nameRef name="currentSequenceFactCount"/>
					</plx:increment>
				</plx:branch>
			</plx:loop>
			<plx:local name="currentSequenceFacts" dataTypeIsSimpleArray="true" dataTypeName="FactType">
				<plx:initialize>
					<plx:callNew dataTypeName="FactType" type="new">
						<plx:arrayDescriptor rank="1"/>
						<plx:passParam>
							<plx:nameRef name="currentSequenceFactCount"/>
						</plx:passParam>
					</plx:callNew>
				</plx:initialize>
			</plx:local>
			<plx:assign>
				<plx:left>
					<plx:nameRef name="currentSequenceFacts"/>
				</plx:left>
				<plx:right>
					<plx:callNew dataTypeName="FactType" type="new">
						<plx:arrayDescriptor rank="1"/>
						<plx:passParam>
							<plx:nameRef name="currentSequenceFactCount"/>
						</plx:passParam>
					</plx:callNew>
				</plx:right>
			</plx:assign>
			<plx:assign>
				<plx:left>
					<plx:nameRef name="currentSequenceFactCount"/>
				</plx:left>
				<plx:right>
					<plx:value data="0" type="i4"/>
				</plx:right>
			</plx:assign>

			<!-- Start unique fact build -->
			<plx:comment>Building the unique fact list.</plx:comment>
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
							<plx:nameRef name="roleArity"/>
						</plx:right>
					</plx:binaryOperator>
				</plx:condition>
				<plx:beforeLoop>
					<plx:increment>
						<plx:nameRef name="i"/>
					</plx:increment>
				</plx:beforeLoop>
				<plx:local name="currentFact" dataTypeName="FactType">
					<plx:initialize>
						<plx:callInstance name="FactType" type="property">
							<plx:callObject>
								<plx:callInstance name=".implied" type="indexerCall">
									<plx:callObject>
										<plx:nameRef name="includedSequenceRoles"/>
									</plx:callObject>
									<plx:passParam>
										<plx:nameRef name="i"/>
									</plx:passParam>
								</plx:callInstance>
							</plx:callObject>
						</plx:callInstance>
					</plx:initialize>
				</plx:local>
				<plx:local name="j" dataTypeName=".i4">
					<plx:initialize>
						<plx:value data="0" type="i4"/>
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
									<plx:nameRef name="currentFact"/>
								</plx:left>
								<plx:right>
									<plx:callInstance name="FactType" type="property">
										<plx:callObject>
											<plx:callInstance name=".implied" type="indexerCall">
												<plx:callObject>
													<plx:nameRef name="includedSequenceRoles"/>
												</plx:callObject>
												<plx:passParam>
													<plx:nameRef name="j"/>
												</plx:passParam>
											</plx:callInstance>
										</plx:callObject>
									</plx:callInstance>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:break/>
					</plx:branch>
				</plx:loop>
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="equality">
							<plx:left>
								<plx:nameRef name="j"/>
							</plx:left>
							<plx:right>
								<plx:nameRef name="i"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:assign>
						<plx:left>
							<plx:callInstance name=".implied" type="arrayIndexer">
								<plx:callObject>
									<plx:nameRef name="currentSequenceFacts"/>
								</plx:callObject>
								<plx:passParam>
									<plx:nameRef name="currentSequenceFactCount"/>
								</plx:passParam>
							</plx:callInstance>
						</plx:left>
						<plx:right>
							<plx:nameRef name="currentFact"/>
						</plx:right>
					</plx:assign>
					<plx:increment>
						<plx:nameRef name="currentSequenceFactCount"/>
					</plx:increment>
				</plx:branch>
			</plx:loop>
			<!-- End Unique Fact Build -->
			<plx:assign>
				<plx:left>
					<plx:nameRef name="factArity"/>
				</plx:left>
				<plx:right>
					<plx:callInstance name="Length" type="property">
						<plx:callObject>
							<plx:nameRef name="currentSequenceFacts"/>
						</plx:callObject>
					</plx:callInstance>
				</plx:right>
			</plx:assign>
		</xsl:if>
		<xsl:choose>
			<xsl:when test="not($ConditionalMatch='null')">
				<xsl:call-template name="RoleIterator">
					<xsl:with-param name="TopLevel" select="$TopLevel"/>
					<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
					<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
					<xsl:with-param name="CompositeCount" select="$CompositeCount"/>
					<xsl:with-param name="CompositeIterator" select="$CompositeIterator"/>
					<xsl:with-param name="ListStyle" select="$ListStyle"/>
					<xsl:with-param name="ParentListStyle" select="$ParentListStyle"/>
					<xsl:with-param name="ReadingFactTypeVariableName" select="$ReadingFactTypeVariableName"/>
					<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
					<xsl:with-param name="SequenceIterator" select="$SequenceIterator"/>
					<xsl:with-param name="ConditionalMatch" select="$ConditionalMatch"/>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="RoleIterator">
					<xsl:with-param name="TopLevel" select="$TopLevel"/>
					<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
					<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
					<xsl:with-param name="CompositeCount" select="$CompositeCount"/>
					<xsl:with-param name="CompositeIterator" select="$CompositeIterator"/>
					<xsl:with-param name="ListStyle" select="$ListStyle"/>
					<xsl:with-param name="ParentListStyle" select="$ParentListStyle"/>
					<xsl:with-param name="ReadingFactTypeVariableName" select="$ReadingFactTypeVariableName"/>
					<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
					<xsl:with-param name="SequenceIterator" select="$SequenceIterator"/>
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>

	</xsl:template>
	<xsl:template name="RoleIterator">
		<xsl:param name="TopLevel"/>
		<xsl:param name="VariableDecorator"/>
		<xsl:param name="VariablePrefix"/>
		<xsl:param name="CompositeCount"/>
		<xsl:param name="CompositeIterator"/>
		<xsl:param name="ListStyle"/>
		<xsl:param name="PatternGroup"/>
		<xsl:param name="IteratorContext" select="''"/>
		<xsl:param name="SequenceIterator" select="false()"/>
		<xsl:param name="ParentListStyle" select="'null'"/>
		<xsl:param name="ReadingFactTypeVariableName" select="''"/>
		<!-- Other parameters should be forwarded to IterateRolesConstraintVerbalizationBody -->

		<!-- Normalize the match data -->
		<xsl:variable name="iterateFacts" select="boolean(self::cvg:IterateFacts)"/>
		<xsl:variable name="iterateSequenceFacts" select="boolean(self::cvg:IterateSequenceFacts)"/>
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
		<xsl:variable name="forwardReadingFactTypeVariableNameFragment">
			<xsl:choose>
				<xsl:when test="$iterateSequenceFacts">
					<xsl:text>currentFact</xsl:text>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$ReadingFactTypeVariableName"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="forwardReadingFactTypeVariableName" select="string($forwardReadingFactTypeVariableNameFragment)"/>
		<xsl:variable name="iterVarNameFragment">
			<xsl:choose>
				<xsl:when test="$iterateFacts or $iterateSequenceFacts">
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
			<xsl:choose>
				<xsl:when test="$iterateSequenceFacts and (not($ListStyle='null') or not(child::*/@listStyle='null'))">
					<xsl:call-template name="EnsureTempStringBuilder">
						<xsl:with-param name="BypassLengthReset" select="true()"/>
						<xsl:with-param name="IteratorVariableName" select="$SequenceIterator"/>
					</xsl:call-template>
				</xsl:when>
				<xsl:otherwise>
					<xsl:call-template name="EnsureTempStringBuilder"/>
				</xsl:otherwise>
			</xsl:choose>
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
			<xsl:for-each select="exsl:node-set($filterOperatorsFragment)/child::*">
				<xsl:if test="position()=1">
					<xsl:call-template name="CombineElements">
						<xsl:with-param name="OperatorType" select="'booleanAnd'"/>
					</xsl:call-template>
				</xsl:if>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="filterTest" select="exsl:node-set($filterTestFragment)/child::*"/>
		<xsl:variable name="filteredCountVarName" select="concat($VariablePrefix,'FilteredCount',$VariableDecorator)"/>
		<xsl:variable name="filteredIterVarName" select="concat($VariablePrefix,'FilteredIter',$VariableDecorator)"/>
		<xsl:variable name="trackFirstPass" select="0!=count(descendant::cvg:PredicateReplacement[@pass='first']) or 0!=count(descendant::cvg:PredicateReplacement[@pass='notFirst'])"/>
		<xsl:variable name="isNotFirst" select="0!=count(descendant::cvg:PredicateReplacement[@pass='notFirst'])"/>
		<xsl:variable name="trackFirstPassVarName" select="concat($VariablePrefix,'IsFirstPass',$VariableDecorator)"/>
		<xsl:variable name="createList" select="not($ListStyle='null')"/>
		<xsl:variable name="createListOrTrackFirstPass" select="$trackFirstPass or $createList"/>
		<xsl:variable name="hyphenBind" select="@hyphenBind='true' or @hyphenBind='1'"/>

		<xsl:if test="$trackFirstPass and not($PatternGroup='SetComparisonConstraint')">
			<plx:local name="{$trackFirstPassVarName}" dataTypeName=".boolean">
				<plx:initialize>
					<plx:trueKeyword/>
				</plx:initialize>
			</plx:local>
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
						<xsl:with-param name="ReadingFactTypeVariableName" select="$forwardReadingFactTypeVariableName"/>
						<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
						<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
						<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
						<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
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
		<xsl:if test="$contextMatch='preferredIdentifier'">
			<plx:local dataTypeName="LinkedElementCollection" name="includedRoles">
				<plx:passTypeParam dataTypeName="Role"/>
				<plx:initialize>
					<plx:callInstance name="RoleCollection" type="property">
						<plx:callObject>
							<plx:cast dataTypeName="ConstraintRoleSequence">
								<plx:callThis name="PreferredIdentifier" type="property"/>
							</plx:cast>
						</plx:callObject>
					</plx:callInstance>
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
				<plx:binaryOperator type="lessThan">
					<plx:left>
						<plx:nameRef name="{$iterVarName}"/>
					</plx:left>
					<plx:right>
						<xsl:choose>
							<xsl:when test="@pass='first'">
								<plx:value type="i4" data="1"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:call-template name="ReferenceIteratorBound">
									<xsl:with-param name="ContextMatch" select="$contextMatch"/>
									<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
								</xsl:call-template>
							</xsl:otherwise>
						</xsl:choose>
					</plx:right>
				</plx:binaryOperator>
			</plx:condition>
			<plx:beforeLoop>
				<plx:increment>
					<plx:nameRef name="{$iterVarName}"/>
				</plx:increment>
			</plx:beforeLoop>
			<xsl:if test="$iterateSequenceFacts">
				<plx:local dataTypeName="FactType" name="currentFact">
					<plx:initialize>
						<plx:callInstance name=".implied" type="indexerCall">
							<plx:callObject>
								<plx:nameRef name="currentSequenceFacts"/>
							</plx:callObject>
							<plx:passParam>
								<plx:nameRef name="{$iterVarName}"/>
							</plx:passParam>
						</plx:callInstance>
					</plx:initialize>
				</plx:local>
				<plx:assign>
					<plx:left>
						<plx:nameRef name="factRoles"/>
					</plx:left>
					<plx:right>
						<plx:callInstance name="RoleCollection" type="property">
							<plx:callObject>
								<plx:nameRef name="currentFact"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:right>
				</plx:assign>
				<plx:assign>
					<plx:left>
						<plx:nameRef name="unaryRoleIndex"/>
					</plx:left>
					<plx:right>
						<xsl:call-template name="InitializeUnaryRoleIndex"/>
					</plx:right>
				</plx:assign>
				<plx:local name="currentRoleCount" dataTypeName=".i4">
					<plx:initialize>
						<xsl:call-template name="InitializeFactArity"/>
					</plx:initialize>
				</plx:local>
				<plx:branch>
					<plx:condition>
						<plx:callInstance name="HasValue" type="property">
							<plx:callObject>
								<plx:nameRef name="unaryRoleIndex"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:condition>
					<plx:assign>
						<plx:left>
							<plx:nameRef name="factRoles"/>
						</plx:left>
						<plx:right>
							<plx:callNew dataTypeName="RoleBase" dataTypeIsSimpleArray="true">
								<plx:arrayInitializer>
									<plx:callInstance name=".implied" type="indexerCall">
										<plx:callObject>
											<plx:nameRef name="factRoles"/>
										</plx:callObject>
										<plx:passParam>
											<plx:callInstance name="Value" type="property">
												<plx:callObject>
													<plx:nameRef name="unaryRoleIndex"/>
												</plx:callObject>
											</plx:callInstance>
										</plx:passParam>
									</plx:callInstance>
								</plx:arrayInitializer>
							</plx:callNew>
						</plx:right>
					</plx:assign>
				</plx:branch>
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
			</xsl:if>
			<xsl:if test="$contextMatch='constraintRoles' or $contextMatch='preferredIdentifier' or descendant::cvg:*[@match='primary' or @match='secondary' or @conditionMatch='RolePlayerHasReferenceScheme'] or descendant::cvg:RoleName or descendant::cvg:IterateContextRoles or $IteratorContext">
				<xsl:variable name="primaryRoleInitializerFragment">
					<plx:callInstance name=".implied" type="arrayIndexer">
						<plx:callObject>
							<xsl:call-template name="ReferenceIteratorSet">
								<xsl:with-param name="ContextMatch" select="$contextMatch"/>
								<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
							</xsl:call-template>
						</plx:callObject>
						<plx:passParam>
							<plx:nameRef name="{$iterVarName}"/>
						</plx:passParam>
					</plx:callInstance>
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
				<xsl:when test="$contextMatch='preferredIdentifier'">
					<plx:local name="parentFact" dataTypeName="FactType">
						<plx:initialize>
							<plx:callInstance name="FactType" type="property">
								<plx:callObject>
									<plx:nameRef name="primaryRole"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:initialize>
					</plx:local>
					<plx:local name="factRoles" dataTypeName="IList">
						<plx:passTypeParam dataTypeName="RoleBase"/>
						<plx:initialize>
							<plx:callInstance name="RoleCollection" type="property">
								<plx:callObject>
									<plx:nameRef name="parentFact"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:initialize>
					</plx:local>
					<plx:local name="unaryRoleIndex" dataTypeName="Nullable">
						<plx:passTypeParam dataTypeName=".i4"/>
						<plx:initialize>
							<xsl:call-template name="InitializeUnaryRoleIndex"/>
						</plx:initialize>
					</plx:local>
					<plx:local name="factArity" dataTypeName=".i4">
						<plx:initialize>
							<xsl:call-template name="InitializeFactArity"/>
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
					<plx:local name="reading" dataTypeName="IReading">
						<plx:initialize>
							<plx:nullKeyword/>
						</plx:initialize>
					</plx:local>
					<plx:local name="hyphenBinder" dataTypeName="VerbalizationHyphenBinder"/>
				</xsl:when>
				<xsl:when test="$contextMatch='constraintRoles' or $iterateFacts">
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
							<plx:nameRef name="factRoles"/>
						</plx:left>
						<plx:right>
							<plx:callInstance name="RoleCollection" type="property">
								<plx:callObject>
									<plx:nameRef name="parentFact"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:right>
					</plx:assign>
					<xsl:if test="not($PatternGroup='SetComparisonConstraint')">
						<plx:assign>
							<plx:left>
								<plx:nameRef name="unaryRoleIndex"/>
							</plx:left>
							<plx:right>
								<xsl:call-template name="InitializeUnaryRoleIndex"/>
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
					<plx:local name="basicRoleReplacements" dataTypeName=".string" dataTypeIsSimpleArray="true">
						<plx:initialize>
							<plx:callInstance name=".implied" type="arrayIndexer">
								<plx:callObject>
									<plx:nameRef name="allBasicRoleReplacements"/>
								</plx:callObject>
								<plx:passParam>
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
								</plx:passParam>
							</plx:callInstance>
						</plx:initialize>
					</plx:local>
				</xsl:when>
			</xsl:choose>
			<xsl:choose>
				<xsl:when test="$filterTest">
					<plx:branch>
						<plx:condition>
							<xsl:copy-of select="$filterTest"/>
						</plx:condition>
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
						<xsl:choose>
							<xsl:when test="not($iterateSequenceFacts)">
								<xsl:call-template name="IterateRolesConstraintVerbalizationBody">
									<xsl:with-param name="TopLevel" select="$TopLevel"/>
									<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
									<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
									<xsl:with-param name="CompositeCount" select="string($passCompositeCount)"/>
									<xsl:with-param name="CompositeIterator" select="string($passCompositeIterator)"/>
									<xsl:with-param name="ListStyle" select="$ListStyle"/>
									<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
									<xsl:with-param name="FirstPassVariable" select="$trackFirstPassVarName"/>
									<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
									<xsl:with-param name="ReadingFactTypeVariableName" select="$forwardReadingFactTypeVariableName"/>
									<!-- Forwarded local parameters -->
									<xsl:with-param name="contextMatch" select="$contextMatch"/>
									<xsl:with-param name="iterVarName" select="$iterVarName"/>
									<xsl:with-param name="hyphenBind" select="$hyphenBind"/>
									<xsl:with-param name="byPassList" select="not($ParentListStyle='null')"/>
								</xsl:call-template>
							</xsl:when>
							<xsl:when test="$iterateSequenceFacts and not($ListStyle='null')">
								<xsl:call-template name="IterateRolesConstraintVerbalizationBody">
									<xsl:with-param name="TopLevel" select="$TopLevel"/>
									<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
									<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
									<xsl:with-param name="CompositeCount" select="string($passCompositeCount)"/>
									<xsl:with-param name="CompositeIterator" select="string($passCompositeIterator)"/>
									<xsl:with-param name="ListStyle" select="$ListStyle"/>
									<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
									<xsl:with-param name="FirstPassVariable" select="$trackFirstPassVarName"/>
									<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
									<xsl:with-param name="ReadingFactTypeVariableName" select="$forwardReadingFactTypeVariableName"/>
									<!-- Forwarded local parameters -->
									<xsl:with-param name="contextMatch" select="$contextMatch"/>
									<xsl:with-param name="iterVarName" select="$iterVarName"/>
									<xsl:with-param name="hyphenBind" select="$hyphenBind"/>
									<xsl:with-param name="byPassList" select="not($ParentListStyle='null')"/>
								</xsl:call-template>
							</xsl:when>
							<xsl:otherwise>
								<xsl:call-template name="IterateRolesConstraintVerbalizationBody">
									<xsl:with-param name="TopLevel" select="$TopLevel"/>
									<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
									<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
									<xsl:with-param name="CompositeCount" select="string($passCompositeCount)"/>
									<xsl:with-param name="CompositeIterator" select="string($passCompositeIterator)"/>
									<xsl:with-param name="ListStyle" select="'null'"/>
									<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
									<xsl:with-param name="FirstPassVariable" select="$trackFirstPassVarName"/>
									<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
									<xsl:with-param name="ReadingFactTypeVariableName" select="$forwardReadingFactTypeVariableName"/>
									<!-- Forwarded local parameters -->
									<xsl:with-param name="contextMatch" select="$contextMatch"/>
									<xsl:with-param name="iterVarName" select="$iterVarName"/>
									<xsl:with-param name="hyphenBind" select="$hyphenBind"/>
								</xsl:call-template>
							</xsl:otherwise>
						</xsl:choose>
						<xsl:if test="$createListOrTrackFirstPass">
							<plx:increment>
								<plx:nameRef name="{$passCompositeIterator}"/>
							</plx:increment>
						</xsl:if>
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
					</plx:branch>
				</xsl:when>
				<xsl:otherwise>
					<xsl:if test="$contextMatch='preferredIdentifier'">
						<xsl:call-template name="PopulateBasicRoleReplacements"/>
					</xsl:if>
					<xsl:choose>
						<xsl:when test="not($iterateSequenceFacts)">
							<xsl:call-template name="IterateRolesConstraintVerbalizationBody">
								<xsl:with-param name="TopLevel" select="$TopLevel"/>
								<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
								<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
								<xsl:with-param name="CompositeCount" select="$CompositeCount"/>
								<xsl:with-param name="CompositeIterator" select="$CompositeIterator"/>
								<xsl:with-param name="ListStyle" select="$ListStyle"/>
								<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
								<xsl:with-param name="FirstPassVariable" select="$trackFirstPassVarName"/>
								<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
								<xsl:with-param name="ReadingFactTypeVariableName" select="$forwardReadingFactTypeVariableName"/>
								<!-- Forwarded local parameters -->
								<xsl:with-param name="contextMatch" select="$contextMatch"/>
								<xsl:with-param name="iterVarName" select="$iterVarName"/>
								<xsl:with-param name="hyphenBind" select="$hyphenBind"/>
								<xsl:with-param name="byPassList" select="not($ParentListStyle='null')"/>
							</xsl:call-template>
						</xsl:when>
						<xsl:when test="$iterateSequenceFacts and not($ListStyle='null')">
							<xsl:call-template name="IterateRolesConstraintVerbalizationBody">
								<xsl:with-param name="TopLevel" select="$TopLevel"/>
								<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
								<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
								<xsl:with-param name="CompositeCount" select="$CompositeCount"/>
								<xsl:with-param name="CompositeIterator" select="$CompositeIterator"/>
								<xsl:with-param name="ListStyle" select="$ListStyle"/>
								<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
								<xsl:with-param name="FirstPassVariable" select="$trackFirstPassVarName"/>
								<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
								<xsl:with-param name="ReadingFactTypeVariableName" select="$forwardReadingFactTypeVariableName"/>
								<!-- Forwarded local parameters -->
								<xsl:with-param name="contextMatch" select="$contextMatch"/>
								<xsl:with-param name="iterVarName" select="$iterVarName"/>
								<xsl:with-param name="hyphenBind" select="$hyphenBind"/>
								<xsl:with-param name="byPassList" select="not($ParentListStyle='null')"/>
							</xsl:call-template>
						</xsl:when>
						<xsl:otherwise>
							<xsl:call-template name="IterateRolesConstraintVerbalizationBody">
								<xsl:with-param name="TopLevel" select="$TopLevel"/>
								<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
								<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
								<xsl:with-param name="CompositeCount" select="$CompositeCount"/>
								<xsl:with-param name="CompositeIterator" select="$CompositeIterator"/>
								<xsl:with-param name="ListStyle" select="$ListStyle"/>
								<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
								<xsl:with-param name="FirstPassVariable" select="$trackFirstPassVarName"/>
								<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
								<xsl:with-param name="ReadingFactTypeVariableName" select="$forwardReadingFactTypeVariableName"/>
								<!-- Forwarded local parameters -->
								<xsl:with-param name="contextMatch" select="$contextMatch"/>
								<xsl:with-param name="iterVarName" select="$iterVarName"/>
								<xsl:with-param name="hyphenBind" select="$hyphenBind"/>
								<xsl:with-param name="byPassList" select="not($ParentListStyle='null')"/>
							</xsl:call-template>
						</xsl:otherwise>
					</xsl:choose>
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
				</xsl:otherwise>
			</xsl:choose>
		</plx:loop>
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
			<xsl:when test="0=string-length($CompositeIterator) and not($iterateSequenceFacts)">
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
		<xsl:variable name="trackFirstPass" select="0!=count(descendant::cvg:PredicateReplacement[@pass='first'])"/>
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
			<xsl:variable name="IterateRanges" select="$contextMatch='rangeCount'"/>
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
		<xsl:param name="PatternGroup"/>
		<plx:nameRef>
			<xsl:attribute name="name">
				<xsl:choose>
					<xsl:when test="$CompositeCount">
						<xsl:value-of select="$CompositeCount"/>
					</xsl:when>
					<xsl:when test="$ContextMatch='all'">
						<xsl:text>factArity</xsl:text>
					</xsl:when>
					<xsl:when test="$ContextMatch='facts'">
						<xsl:text>allFactsCount</xsl:text>
					</xsl:when>
					<xsl:when test="$ContextMatch='included'">
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
					<xsl:when test="$ContextMatch='constraintRoles' and not($PatternGroup='SetComparisonConstraint')">
						<xsl:text>constraintRoleArity</xsl:text>
					</xsl:when>
					<xsl:when test="$ContextMatch='constraintRoles' and $PatternGroup='SetComparisonConstraint'">
						<xsl:text>factArity</xsl:text>
					</xsl:when>
					<xsl:when test="$ContextMatch='excluded'">
						<xsl:text>factArity</xsl:text>
					</xsl:when>
					<xsl:when test="$ContextMatch='rangeCount'">
						<xsl:text>rangeCount</xsl:text>
					</xsl:when>
					<xsl:when test="$ContextMatch='preferredIdentifier'">
						<xsl:text>constraintRoleArity</xsl:text>
					</xsl:when>
					<xsl:otherwise>
						<xsl:message terminate="yes">NO ITERATOR NAME</xsl:message>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
		</plx:nameRef>
	</xsl:template>
	<xsl:template name="ReferenceIteratorSet">
		<xsl:param name="ContextMatch"/>
		<xsl:param name="PatternGroup"/>
		<plx:nameRef>
			<xsl:attribute name="name">
				<xsl:choose>
					<xsl:when test="$ContextMatch='all'">
						<xsl:text>factRoles</xsl:text>
					</xsl:when>
					<xsl:when test="$ContextMatch='included'">
						<xsl:choose>
							<xsl:when test="$PatternGroup='InternalSetConstraint'">
								<xsl:text>allConstraintRoles</xsl:text>
							</xsl:when>
							<xsl:when test="$PatternGroup='SetComparisonConstraint'">
								<xsl:text>includedSequenceRoles</xsl:text>
							</xsl:when>
							<xsl:otherwise>
								<xsl:text>includedRoles</xsl:text>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:when test="$ContextMatch='constraintRoles'">
						<xsl:choose>
							<xsl:when test="$PatternGroup='SetComparisonConstraint'">
								<xsl:text>includedSequenceRoles</xsl:text>
							</xsl:when>
							<xsl:otherwise>
								<xsl:text>allConstraintRoles</xsl:text>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:when test="$ContextMatch='excluded'">
						<xsl:text>factRoles</xsl:text>
					</xsl:when>
					<xsl:when test="$ContextMatch='preferredIdentifier'">
						<xsl:text>includedRoles</xsl:text>
					</xsl:when>
					<xsl:otherwise>
						<xsl:message terminate="yes">UNSUPPORTED ITERATOR SET</xsl:message>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
		</plx:nameRef>
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
		<xsl:param name="ListStyle"/>
		<xsl:param name="PatternGroup"/>
		<xsl:param name="FirstPassVariable"/>
		<xsl:param name="IteratorContext"/>
		<xsl:param name="ReadingFactTypeVariableName" select="''"/>
		<!-- Forwarded local parameters -->
		<xsl:param name="contextMatch"/>
		<xsl:param name="iterVarName"/>
		<xsl:param name="hyphenBind" select="false()"/>
		<xsl:param name="byPassList" select="false()"/>
		<!-- Use the current snippets data to open the list -->
		<xsl:variable name="createList" select="not($ListStyle='null')"/>
		<xsl:variable name="IterateRanges" select="$contextMatch='rangeCount'"/>
		<xsl:if test="$createList">
			<plx:local name="listSnippet" dataTypeName="{$VerbalizationTextSnippetType}"/>
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
				<xsl:if test="not(child::cvg:RolePlayer)">
					<xsl:call-template name="PopulateListSnippet">
						<xsl:with-param name="IteratorVariableName" select="$iterVarName"/>
						<xsl:with-param name="ListStyle" select="$ListStyle"/>
						<xsl:with-param name="IteratorBound">
							<xsl:call-template name="ReferenceIteratorBound">
								<xsl:with-param name="CompositeCount" select="$CompositeCount"/>
								<xsl:with-param name="ContextMatch" select="$contextMatch"/>
								<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
							</xsl:call-template>
						</xsl:with-param>
						<xsl:with-param name="CompositeIterator" select="$CompositeIterator"/>
					</xsl:call-template>
				</xsl:if>
			</xsl:otherwise>
		</xsl:choose>
		<xsl:if test="$createList and not(child::cvg:RolePlayer)">
			<xsl:call-template name="AppendListSnippet"/>
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
					<xsl:with-param name="ReadingFactTypeVariableName" select="$ReadingFactTypeVariableName"/>
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
					<xsl:choose>
						<xsl:when test="$TopLevel">
							<plx:local name="{$VariablePrefix}{$VariableDecorator}" dataTypeName=".string">
								<plx:initialize>
									<plx:nullKeyword/>
								</plx:initialize>
							</plx:local>
						</xsl:when>
						<xsl:otherwise>
							<plx:assign>
								<plx:left>
									<plx:nameRef name="{$VariablePrefix}{$VariableDecorator}"/>
								</plx:left>
								<plx:right>
									<plx:nullKeyword/>
								</plx:right>
							</plx:assign>
							<xsl:if test="boolean(self::cvg:RolePlayer) and not($ListStyle='null')">
								<plx:local name="isFirstAppend" dataTypeName=".boolean">
									<plx:initialize>
										<plx:trueKeyword/>
									</plx:initialize>
								</plx:local>
							</xsl:if>
						</xsl:otherwise>
					</xsl:choose>
					<xsl:apply-templates select="." mode="ConstraintVerbalization">
						<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
						<!-- Pass the position in here or it will always be 1 -->
						<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
						<xsl:with-param name="IteratorContext" select="$contextMatch"/>
						<xsl:with-param name="IteratorVariableName" select="$iterVarName"/>
						<xsl:with-param name="ReadingFactTypeVariableName" select="$ReadingFactTypeVariableName"/>
						<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
						<xsl:with-param name="FirstPassVariable" select="$FirstPassVariable"/>
						<xsl:with-param name="ContextMatch" select="$contextMatch"/>
						<xsl:with-param name="ListStyle" select="$ListStyle"/>
					</xsl:apply-templates>
					<xsl:if test="$byPassList and boolean(self::cvg:Fact)">
						<xsl:call-template name="AppendListSnippet"/>
					</xsl:if>
					<xsl:if test="not(self::cvg:RolePlayer)">
						<plx:callInstance name="Append">
							<plx:callObject>
								<plx:nameRef name="sbTemp"/>
							</plx:callObject>
							<plx:passParam>
								<plx:nameRef name="{$VariablePrefix}{$VariableDecorator}"/>
							</plx:passParam>
						</plx:callInstance>
					</xsl:if>
				</xsl:for-each>
			</xsl:when>
			<xsl:otherwise>
				<xsl:variable name="roleIndexExpression">
					<xsl:choose>
						<xsl:when test="@match='included' or @match='constraintRoles'">
							<!-- The role index needs to be retrieved from the all roles list -->
							<plx:callStatic name="IndexOfRole" dataTypeName="FactType">
								<plx:passParam>
									<plx:nameRef name="factRoles"/>
								</plx:passParam>
								<plx:passParam>
									<plx:callInstance name=".implied" type="arrayIndexer">
										<plx:callObject>
											<plx:nameRef name="includedRoles">
												<xsl:if test="@match='constraintRoles' or $PatternGroup='InternalSetConstraint'">
													<xsl:attribute name="name">
														<xsl:choose>
															<xsl:when test="$PatternGroup='SetComparisonConstraint'">
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
								</plx:passParam>
							</plx:callStatic>
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
							<plx:nameRef name="{concat($ResolvedRoleVariablePart,$VariableDecorator)}"/>
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
					<plx:local name="{concat($ResolvedRoleVariablePart,$VariableDecorator)}" dataTypeName=".i4">
						<plx:initialize>
							<xsl:copy-of select="$roleIndexExpression"/>
						</plx:initialize>
					</plx:local>
				</xsl:if>
				<xsl:variable name="roleReplacementContents">
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
							<xsl:copy-of select="$roleIndexReference"/>
						</plx:passParam>
					</plx:callInstance>
				</xsl:variable>
				<plx:callInstance name="Append">
					<plx:callObject>
						<plx:nameRef name="sbTemp"/>
					</plx:callObject>
					<plx:passParam>
						<xsl:choose>
							<xsl:when test="$hyphenBind">
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
				<xsl:if test="not(child::cvg:RolePlayer)">
					<xsl:call-template name="CloseList">
						<xsl:with-param name="IteratorVariableName" select="$iterVarName"/>
						<xsl:with-param name="ListStyle" select="$ListStyle"/>
						<xsl:with-param name="IteratorBound">
							<xsl:call-template name="ReferenceIteratorBound">
								<xsl:with-param name="CompositeCount" select="$CompositeCount"/>
								<xsl:with-param name="ContextMatch" select="$contextMatch"/>
								<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
							</xsl:call-template>
						</xsl:with-param>
						<xsl:with-param name="CompositeIterator" select="$CompositeIterator"/>
					</xsl:call-template>
				</xsl:if>
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
		<xsl:param name="ReadingFactTypeVariableName" select="''"/>
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
		<xsl:apply-templates select="child::*" mode="CompositeOrFilteredListCount">
			<xsl:with-param name="TotalCountCollectorVariable" select="$compositeCountVarName"/>
			<xsl:with-param name="IteratorVariableName" select="$iteratorVarName"/>
			<xsl:with-param name="ReadingFactTypeVariableName" select="$ReadingFactTypeVariableName"/>
			<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
			<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
			<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
			<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
		</xsl:apply-templates>
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
				<xsl:with-param name="ReadingFactTypeVariableName" select="$ReadingFactTypeVariableName"/>
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
	<xsl:template match="@currentSequence2" mode="IterateRolesFilterOperator">
		<plx:nameRef name="FILTERFACTSBYSEQUENCE2"/>
	</xsl:template>
	<xsl:template match="@currentSequence" mode="IterateRolesFilterOperator">
		<plx:nameRef name="FILTERFACTSBYSEQUENCE"/>
	</xsl:template>
	
	<!-- Ignore attributes that are not used as a filter -->
	<xsl:template match="@listStyle|@pass|@conditionalMatch|@hyphenBind" mode="IterateRolesFilterOperator"/>
	<!-- Terminate processing if we see an unrecognized operator -->
	<xsl:template match="@*" mode="IterateRolesFilterOperator">
		<xsl:call-template name="TerminateForInvalidAttribute">
			<xsl:with-param name="MessageText">Unrecognized role iterator filter attribute</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
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
			<xsl:for-each select="exsl:node-set($filterOperatorsFragment)/child::*">
				<xsl:if test="position()=1">
					<xsl:call-template name="CombineElements">
						<xsl:with-param name="OperatorType" select="'booleanAnd'"/>
					</xsl:call-template>
				</xsl:if>
			</xsl:for-each>
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
					<!-- UNDONE: We may not need this for all cases, it depends on the
						 filters in place. -->
					<xsl:variable name="primaryRoleInitializerFragment">
						<plx:callInstance name=".implied" type="arrayIndexer">
							<plx:callObject>
								<xsl:call-template name="ReferenceIteratorSet">
									<xsl:with-param name="ContextMatch" select="$contextMatch"/>
									<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
								</xsl:call-template>
							</plx:callObject>
							<plx:passParam>
								<plx:nameRef name="{$IteratorVariableName}"/>
							</plx:passParam>
						</plx:callInstance>
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
								<plx:nameRef name="factRoles"/>
							</plx:left>
							<plx:right>
								<plx:callInstance name="RoleCollection" type="property">
									<plx:callObject>
										<plx:nameRef name="parentFact"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:right>
						</plx:assign>
						<plx:assign>
							<plx:left>
								<plx:nameRef name="unaryRoleIndex"/>
							</plx:left>
							<plx:right>
								<xsl:call-template name="InitializeUnaryRoleIndex"/>
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
					<plx:branch>
						<plx:condition>
							<xsl:copy-of select="$filterTest"/>
						</plx:condition>
						<plx:increment>
							<plx:nameRef name="{$TotalCountCollectorVariable}"/>
						</plx:increment>
					</plx:branch>
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
		<xsl:param name="ReverseSign" select="@reverseSign='true' or @reverseSign=1"/>
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
					<xsl:when test="$ReverseSign">
						<plx:unaryOperator type="booleanNot">
							<xsl:copy-of select="$IsNegativeSnippet"/>
						</plx:unaryOperator>
					</xsl:when>
					<xsl:otherwise>
						<xsl:copy-of select="$IsNegativeSnippet"/>
					</xsl:otherwise>
				</xsl:choose>
			</plx:passParam>
		</plx:callInstance>
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
	<!-- Helper function to create an initialized string builder in the sbTemp local variable -->
	<xsl:template name="EnsureTempStringBuilder">
		<xsl:param name="BypassLengthReset" select="false()"/>
		<xsl:param name="IteratorVariableName" select="''"/>
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
		<xsl:choose>
			<xsl:when test="not($BypassLengthReset)">
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
			</xsl:when>
			<xsl:otherwise>
				<plx:alternateBranch>
					<plx:condition>
						<plx:binaryOperator type="equality">
							<plx:left>
								<plx:nameRef name="{$IteratorVariableName}"/>
							</plx:left>
							<plx:right>
								<plx:value data="0" type="i4"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
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
				</plx:alternateBranch>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="PopulateReading">
		<!-- Support readings for {Context, {Prefer|Require}[Non][Primary]LeadReading[NoFrontText], null} ReadingChoice values -->
		<xsl:param name="ReadingChoice"/>
		<xsl:param name="PatternGroup"/>
		<xsl:param name="ReadingFactTypeVariableName" select="''"/>
		<xsl:param name="ConditionalLoop" select="boolean(self::cvg:ReadingChoice)"/>
		<xsl:param name="ConditionalReadingOrderIndex"/>
		<xsl:choose>
			<xsl:when test="$ReadingChoice='Context' and $PatternGroup='SetConstraint'">
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
								<plx:nameRef name="factRoles"/>
							</plx:passParam>
							<plx:passParam>
								<plx:nameRef name="unaryRoleIndex"/>
							</plx:passParam>
							<plx:passParam>
								<xsl:call-template name="SnippetFor">
									<xsl:with-param name="SnippetType" select="'HyphenBoundPredicatePart'"/>
								</xsl:call-template>
							</plx:passParam>
						</plx:callNew>
					</plx:right>
				</plx:assign>
			</xsl:when>
			<xsl:when test="not($ReadingChoice='Context')">
				<plx:assign>
					<plx:left>
						<plx:nameRef name="reading"/>
					</plx:left>
					<plx:right>
						<plx:callInstance name="GetMatchingReading">
							<plx:callObject>
								<plx:nameRef name="parentFact">
									<xsl:if test="string($ReadingFactTypeVariableName)">
										<xsl:attribute name="name">
											<xsl:value-of select="$ReadingFactTypeVariableName"/>
										</xsl:attribute>
									</xsl:if>
								</plx:nameRef>
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
									<xsl:when test="not($PatternGroup='SetConstraint') and contains($ReadingChoice,'LeadReading') and not(contains($ReadingChoice,'PrimaryLeadReading'))">
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
								<!-- The invertLeadRoles param -->
								<xsl:choose>
									<xsl:when test="contains($ReadingChoice,'Non')">
										<plx:trueKeyword/>
									</xsl:when>
									<xsl:otherwise>
										<plx:falseKeyword/>
									</xsl:otherwise>
								</xsl:choose>
							</plx:passParam>
							<plx:passParam>
								<!-- The noFrontText param -->
								<xsl:choose>
									<xsl:when test="contains($ReadingChoice,'NoFrontText')">
										<plx:trueKeyword/>
									</xsl:when>
									<xsl:otherwise>
										<plx:falseKeyword/>
									</xsl:otherwise>
								</xsl:choose>
							</plx:passParam>
							<plx:passParam>
								<!-- The defaultRoleOrder param -->
								<plx:nameRef name="factRoles"/>
							</plx:passParam>
							<plx:passParam>
								<!-- The allowAnyOrder param -->
								<xsl:choose>
									<xsl:when test="not(starts-with($ReadingChoice,'Require'))">
										<plx:trueKeyword/>
									</xsl:when>
									<xsl:otherwise>
										<plx:falseKeyword/>
									</xsl:otherwise>
								</xsl:choose>
							</plx:passParam>
						</plx:callInstance>
					</plx:right>
				</plx:assign>
				<xsl:if test="not($ConditionalLoop) or not($PatternGroup='SetConstraint' or $PatternGroup='SetComparisonConstraint')">
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
									<plx:nameRef name="factRoles"/>
								</plx:passParam>
								<plx:passParam>
									<plx:nameRef name="unaryRoleIndex"/>
								</plx:passParam>
								<plx:passParam>
									<xsl:call-template name="SnippetFor">
										<xsl:with-param name="SnippetType" select="'HyphenBoundPredicatePart'"/>
									</xsl:call-template>
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
		<plx:field name="myCache"  visibility="private" static="true" dataTypeName="{@type}">
			<plx:leadingInfo>
				<plx:comment>Cache an instance so we only create one helper in single-threaded scenarios</plx:comment>
			</plx:leadingInfo>
		</plx:field>
		<plx:function name="GetVerbalizer" visibility="public" modifier="static">
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
	</xsl:template>
</xsl:stylesheet>
