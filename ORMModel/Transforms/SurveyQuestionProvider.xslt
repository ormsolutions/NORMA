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
	xmlns:qp="http://schemas.neumont.edu/ORM/SDK/SurveyQuestionProvider"
	xmlns:exsl="http://exslt.org/common">
	<!-- Indenting is useful for debugging the transform, but a waste of memory at generation time -->
	<xsl:output method="xml" encoding="utf-8" indent="no"/>

	<!-- Pick up param value supplied automatically by plix loader -->
	<xsl:param name="CustomToolNamespace" select="'TestNamespace'"/>
	<xsl:template match="qp:surveyQuestionProvider">
		<plx:root>
			<plx:namespaceImport name="System"/>
			<plx:namespaceImport name="System.Windows.Forms"/>
			<plx:namespaceImport name="System.Collections.Generic"/>
			<plx:namespaceImport name="Neumont.Tools.Modeling.Shell.DynamicSurveyTreeGrid"/>
			<plx:namespace name="{$CustomToolNamespace}">
				<plx:class name="{@class}" partial="true" visibility="deferToPartial">
					<plx:implementsInterface dataTypeName="ISurveyQuestionProvider"/>
					<plx:field name="mySurveyQuestionTypeInfo" visibility="private" static="true" readOnly="true" dataTypeName="ISurveyQuestionTypeInfo" dataTypeIsSimpleArray="true">
						<plx:initialize>
							<plx:callNew dataTypeName="ISurveyQuestionTypeInfo" dataTypeIsSimpleArray="true">
								<plx:arrayInitializer>
									<xsl:for-each select="qp:surveyQuestions/qp:surveyQuestion">
										<plx:callStatic type="field" name="Instance" dataTypeName="ProvideSurveyQuestionFor{@questionType}"/>
									</xsl:for-each>
								</plx:arrayInitializer>
							</plx:callNew>
						</plx:initialize>
					</plx:field>
					<plx:function visibility="protected" modifier="static" name="GetSurveyQuestions">
						<plx:leadingInfo>
							<plx:docComment>
								<summary>Implements <see cref="ISurveyQuestionProvider.GetSurveyQuestions"/></summary>
							</plx:docComment>
						</plx:leadingInfo>
						<plx:interfaceMember dataTypeName="ISurveyQuestionProvider" memberName="GetSurveyQuestions"/>
						<plx:returns dataTypeName="IEnumerable">
							<plx:passTypeParam dataTypeName="ISurveyQuestionTypeInfo"/>
						</plx:returns>
						<plx:return>
							<plx:callThis accessor="static" type="field" name="mySurveyQuestionTypeInfo"/>
						</plx:return>
					</plx:function>
					<plx:property name="SurveyQuestionImageList" visibility="protected">
						<plx:leadingInfo>
							<plx:docComment>
								<summary>Implements <see cref="ISurveyQuestionProvider.SurveyQuestionImageList"/></summary>
							</plx:docComment>
						</plx:leadingInfo>
						<plx:interfaceMember dataTypeName="ISurveyQuestionProvider" memberName="SurveyQuestionImageList"/>
						<plx:returns dataTypeName="ImageList"/>
						<plx:get>
							<xsl:variable name="imageSnippet" select="qp:imageInformation/child::plx:*"/>
							<xsl:choose>
								<xsl:when test="$imageSnippet">
									<xsl:copy-of select="$imageSnippet"/>
								</xsl:when>
								<xsl:otherwise>
									<plx:return>
										<plx:nullKeyword/>
									</plx:return>
								</xsl:otherwise>
							</xsl:choose>
						</plx:get>
					</plx:property>
					<xsl:apply-templates select="qp:surveyQuestions/qp:surveyQuestion"/>
				</plx:class>
			</plx:namespace>
		</plx:root>
	</xsl:template>
	<xsl:template match="qp:surveyQuestion">
		<plx:class name="ProvideSurveyQuestionFor{@questionType}" visibility="private" modifier="sealed">
			<plx:implementsInterface dataTypeName="ISurveyQuestionTypeInfo"/>
			<plx:function name=".construct" visibility="private"/>
			<plx:field name="Instance" visibility="public" static="true" readOnly="true" dataTypeName="ISurveyQuestionTypeInfo">
				<plx:initialize>
					<plx:callNew dataTypeName="ProvideSurveyQuestionFor{@questionType}"/>
				</plx:initialize>
			</plx:field>
			<plx:property name="QuestionType" visibility="public">
				<plx:interfaceMember dataTypeName="ISurveyQuestionTypeInfo" memberName="QuestionType"/>
				<plx:returns dataTypeName="Type"/>
				<plx:get>
					<plx:return>
						<plx:typeOf dataTypeName="{@questionType}"/>
					</plx:return>
				</plx:get>
			</plx:property>
			<plx:function name="AskQuestion" visibility="public">
				<plx:interfaceMember dataTypeName="ISurveyQuestionTypeInfo" memberName="AskQuestion"/>
				<plx:param name="data" dataTypeName=".object"/>
				<plx:returns dataTypeName=".i4"/>
				<plx:local name="typedData" dataTypeName="IAnswerSurveyQuestion">
					<plx:passTypeParam dataTypeName="{@questionType}"/>
					<plx:initialize>
						<plx:cast dataTypeName="IAnswerSurveyQuestion" type="testCast">
							<plx:passTypeParam dataTypeName="{@questionType}"/>
							<plx:nameRef name="data" type="parameter"/>
						</plx:cast>
					</plx:initialize>
				</plx:local>
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="identityInequality">
							<plx:left>
								<plx:nameRef name="typedData"/>
							</plx:left>
							<plx:right>
								<plx:nullKeyword/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:return>
						<plx:callInstance name="AskQuestion" type="methodCall">
							<plx:callObject>
								<plx:nameRef name="typedData"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:return>
				</plx:branch>
				<plx:fallbackBranch>
					<plx:return>
						<plx:value type="i4" data="-1"/>
					</plx:return>
				</plx:fallbackBranch>
			</plx:function>
				<plx:function name="MapAnswerToImageIndex" visibility="public" xmlns:plx="http://schemas.neumont.edu/CodeGeneration/PLiX">
					<plx:param name="answer" dataTypeName=".i4"/>
					<plx:returns dataTypeName=".i4"/>
					<plx:branch>
						<plx:condition>
							<plx:binaryOperator type="inequality">
								<plx:left>
									<plx:binaryOperator type="bitwiseAnd">
										<plx:left>
											<plx:callThis name="UISupport" type="property"/>
										</plx:left>
										<plx:right>
											<plx:callStatic name="Glyph" type="field" dataTypeName="SurveyQuestionUISupport"/>
										</plx:right>
									</plx:binaryOperator>
								</plx:left>
								<plx:right>
									<plx:callStatic name="None" type="field" dataTypeName="SurveyQuestionUISupport"/>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:local name="t" dataTypeName="SurveyQuestionGlyph">
							<plx:initialize>
								<plx:cast dataTypeName="SurveyQuestionGlyph">
									<plx:nameRef name="answer" type="parameter"/>
								</plx:cast>
							</plx:initialize>
						</plx:local>
						<plx:return>
							<plx:cast dataTypeName=".i4">
								<plx:nameRef name="t"/>
							</plx:cast>
						</plx:return>
					</plx:branch>
					<plx:fallbackBranch>
						<plx:return>
							<plx:value type="i4" data="-1"/>
						</plx:return>
					</plx:fallbackBranch>
				</plx:function>
				<plx:property name="UISupport" visibility="public">
				<plx:returns dataTypeName="SurveyQuestionUISupport"/>
				<plx:get>
					<plx:return>
						<xsl:choose>
							<xsl:when test="count(qp:displaySupport) = 1">
								<plx:callStatic dataTypeName="SurveyQuestionUISupport" name="{qp:displaySupport/@displayCategory}" type="field"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:for-each select="qp:displaySupport">
									<xsl:if test="position()=1">
										<xsl:call-template name="OrTogetherEnumElements">
											<xsl:with-param name="EnumType" select="'SurveyQuestionUISupport'"/>
											<xsl:with-param name="name" select="@displayCategory"/>
										</xsl:call-template>
									</xsl:if>								
								</xsl:for-each>
							</xsl:otherwise>
						</xsl:choose>
					</plx:return>
				</plx:get>
			</plx:property>

		</plx:class>
	</xsl:template>
	<!-- Or together enum values from the given type. The current state on the initial
	     call should be the position()=1 element inside a for-each context where the elements
		 contain the (unqualified) names of the enum values to or together -->
	<xsl:template name="OrTogetherEnumElements">
		<xsl:param name="EnumType"/>
		<xsl:param name="name"/>
		<xsl:choose>
			<xsl:when test="position()=last()">
				<plx:callStatic dataTypeName="{$EnumType}" name="{$name}" type="field"/>
			</xsl:when>
			<xsl:otherwise>
				<plx:binaryOperator type="bitwiseOr">
					<plx:left>
						<plx:callStatic dataTypeName="{$EnumType}" name="{$name}" type="field"/>
					</plx:left>
					<plx:right>
						<xsl:for-each select="following-sibling::*">
							<xsl:if test="position()=1">
								<xsl:call-template name="OrTogetherEnumElements">
									<xsl:with-param name="EnumType" select="$EnumType"/>
									<xsl:with-param name="name" select="@displayCategory"/>
								</xsl:call-template>
							</xsl:if>
						</xsl:for-each>
					</plx:right>
				</plx:binaryOperator>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>