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
			<plx:namespaceImport name="Neumont.Tools.ORM.Framework.DynamicSurveyTreeGrid"/>
			<plx:namespace name="{$CustomToolNamespace}">
				<plx:class name="{@class}" partial="true" visibility="public">
					<plx:implementsInterface dataTypeName="ISurveyQuestionProvider"/>
					<plx:field name="SurveyQuestionTypeInfo" visibility="private" static="true" readOnly="true" dataTypeName="ISurveyQuestionTypeInfo" dataTypeIsSimpleArray="true">
						<plx:initialize>
							<plx:callNew dataTypeName="ISurveyQuestionTypeInfo" dataTypeIsSimpleArray="true">
								<plx:arrayInitializer>
									<xsl:for-each select="qp:provideSurveyQuestion">
										<plx:passParam>
											<plx:callStatic type="field" name="Instance" dataTypeName="ProvideSurveyQuestionFor{@questionType}"/>
										</plx:passParam>
									</xsl:for-each>
								</plx:arrayInitializer>
							</plx:callNew>
						</plx:initialize>
					</plx:field>
					<plx:function visibility="protected" modifier="static" name="GetSurveyQuestionTypeInfo">
						<plx:leadingInfo>
							<plx:docComment>
								<summary>Returns an array of ISurveyQuestionTypeInfo representing the questions that can be asked of objects in this DomainModel</summary>
							</plx:docComment>
						</plx:leadingInfo>
						<plx:interfaceMember dataTypeName="ISurveyQuestionProvider" memberName="GetSurveyQuestionTypeInfo"/>
						<plx:returns dataTypeName="ISurveyQuestionTypeInfo" dataTypeIsSimpleArray="true"/>
						<plx:return>
							<plx:cast type="exceptionCast" dataTypeName="ISurveyQuestionTypeInfo" dataTypeIsSimpleArray="true">
								<plx:callInstance name="Clone">
									<plx:callObject>
										<plx:callStatic type="field" dataTypeName="{@class}" name="SurveyQuestionTypeInfo"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:cast>
						</plx:return>
					</plx:function>
					<xsl:apply-templates select="qp:provideSurveyQuestion"/>
				</plx:class>
			</plx:namespace>
		</plx:root>
	</xsl:template>
	<xsl:template match="qp:provideSurveyQuestion">
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
		</plx:class>
	</xsl:template>
</xsl:stylesheet>