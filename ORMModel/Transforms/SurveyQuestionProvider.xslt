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
		<xsl:variable name="questions" select="qp:surveyQuestions/qp:surveyQuestion"/>
		<xsl:variable name="dynamicQuestions" select="$questions[@dynamicValues='true' or @dynamicValues='1']"/>
		<plx:root>
			<plx:namespaceImport name="System"/>
			<plx:namespaceImport name="System.Windows.Forms"/>
			<plx:namespaceImport name="System.Collections.Generic"/>
			<xsl:if test="$dynamicQuestions">
				<plx:namespaceImport name="Microsoft.VisualStudio.Modeling"/>
			</xsl:if>
			<plx:namespaceImport name="Neumont.Tools.Modeling.Shell.DynamicSurveyTreeGrid"/>
			<plx:namespace name="{$CustomToolNamespace}">
				<plx:class name="{@class}" partial="true" visibility="deferToPartial">
					<plx:implementsInterface dataTypeName="ISurveyQuestionProvider"/>
					<xsl:variable name="groupings" select="qp:groupings/qp:grouping"/>
					<xsl:for-each select="$dynamicQuestions">
						<plx:field name="myDynamic{@questionType}QuestionInstance" dataTypeName="ProvideSurveyQuestionFor{@questionType}" visibility="private"/>
					</xsl:for-each>
					<xsl:choose>
						<xsl:when test="$groupings">
							<xsl:for-each select="$groupings">
								<xsl:choose>
									<xsl:when test="qp:surveyQuestion/@ref=$dynamicQuestions/@questionType">
										<plx:field name="mySurveyQuestionTypeInfo{position()}" visibility="private" dataTypeName="ISurveyQuestionTypeInfo" dataTypeIsSimpleArray="true"/>
										<plx:function name="EnsureSurveyQuestionTypeInfo{position()}" visibility="private">
											<plx:returns dataTypeName="ISurveyQuestionTypeInfo" dataTypeIsSimpleArray="true"/>
											<plx:return>
												<plx:inlineStatement dataTypeName="ISurveyQuestionTypeInfo" dataTypeIsSimpleArray="true">
													<plx:nullFallbackOperator>
														<plx:left>
															<plx:callThis name="mySurveyQuestionTypeInfo{position()}" type="field"/>
														</plx:left>
														<plx:right>
															<plx:inlineStatement dataTypeName="ISurveyQuestionTypeInfo" dataTypeIsSimpleArray="true">
																<plx:assign>
																	<plx:left>
																		<plx:callThis name="mySurveyQuestionTypeInfo{position()}" type="field"/>
																	</plx:left>
																	<plx:right>
																		<plx:callNew dataTypeName="ISurveyQuestionTypeInfo" dataTypeIsSimpleArray="true">
																			<plx:arrayInitializer>
																				<xsl:for-each select="qp:surveyQuestion">
																					<xsl:choose>
																						<xsl:when test="$dynamicQuestions/@questionType=@ref">
																							<plx:inlineStatement dataTypeName="ProvideSurveyQuestionFor{@ref}">
																								<plx:nullFallbackOperator>
																									<plx:left>
																										<plx:callThis name="myDynamic{@ref}QuestionInstance" type="field"/>
																									</plx:left>
																									<plx:right>
																										<plx:inlineStatement dataTypeName="{@ref}">
																											<plx:assign>
																												<plx:left>
																													<plx:callThis name="myDynamic{@ref}QuestionInstance" type="field"/>
																												</plx:left>
																												<plx:right>
																													<plx:callNew dataTypeName="ProvideSurveyQuestionFor{@ref}">
																														<plx:passParam>
																															<plx:callThis name="Store" type="property"/>
																														</plx:passParam>
																													</plx:callNew>
																												</plx:right>
																											</plx:assign>
																										</plx:inlineStatement>
																									</plx:right>
																								</plx:nullFallbackOperator>
																							</plx:inlineStatement>
																						</xsl:when>
																						<xsl:otherwise>
																							<plx:callStatic type="field" name="Instance" dataTypeName="ProvideSurveyQuestionFor{@ref}"/>
																						</xsl:otherwise>
																					</xsl:choose>
																				</xsl:for-each>
																			</plx:arrayInitializer>
																		</plx:callNew>
																	</plx:right>
																</plx:assign>
															</plx:inlineStatement>
														</plx:right>
													</plx:nullFallbackOperator>
												</plx:inlineStatement>
											</plx:return>
										</plx:function>
									</xsl:when>
									<xsl:otherwise>
										<plx:field name="mySurveyQuestionTypeInfo{position()}" visibility="private" static="true" readOnly="true" dataTypeName="ISurveyQuestionTypeInfo" dataTypeIsSimpleArray="true">
											<plx:initialize>
												<plx:callNew dataTypeName="ISurveyQuestionTypeInfo" dataTypeIsSimpleArray="true">
													<plx:arrayInitializer>
														<xsl:for-each select="qp:surveyQuestion">
															<plx:callStatic type="field" name="Instance" dataTypeName="ProvideSurveyQuestionFor{@ref}"/>
														</xsl:for-each>
													</plx:arrayInitializer>
												</plx:callNew>
											</plx:initialize>
										</plx:field>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:for-each>
						</xsl:when>
						<xsl:otherwise>
							<xsl:choose>
								<xsl:when test="$dynamicQuestions">
									<plx:field name="mySurveyQuestionTypeInfo" visibility="private" dataTypeName="ISurveyQuestionTypeInfo" dataTypeIsSimpleArray="true"/>
									<plx:function name="EnsureSurveyQuestionTypeInfo" visibility="private">
										<plx:returns dataTypeName="ISurveyQuestionTypeInfo" dataTypeIsSimpleArray="true"/>
										<plx:return>
											<plx:inlineStatement dataTypeName="ISurveyQuestionTypeInfo" dataTypeIsSimpleArray="true">
												<plx:nullFallbackOperator>
													<plx:left>
														<plx:callThis name="mySurveyQuestionTypeInfo" type="field"/>
													</plx:left>
													<plx:right>
														<plx:inlineStatement dataTypeName="ISurveyQuestionTypeInfo" dataTypeIsSimpleArray="true">
															<plx:assign>
																<plx:left>
																	<plx:callThis name="mySurveyQuestionTypeInfo" type="field"/>
																</plx:left>
																<plx:right>
																	<plx:callNew dataTypeName="ISurveyQuestionTypeInfo" dataTypeIsSimpleArray="true">
																		<plx:arrayInitializer>
																			<xsl:for-each select="$questions">
																				<xsl:choose>
																					<xsl:when test="@dynamicValues='true' or @dynamicValues='1'">
																						<plx:inlineStatement dataTypeName="ProvideSurveyQuestionFor{@questionType}">
																							<plx:nullFallbackOperator>
																								<plx:left>
																									<plx:callThis name="myDynamic{@questionType}QuestionInstance" type="field"/>
																								</plx:left>
																								<plx:right>
																									<plx:inlineStatement dataTypeName="{@questionType}">
																										<plx:assign>
																											<plx:left>
																												<plx:callThis name="myDynamic{@questionType}QuestionInstance" type="field"/>
																											</plx:left>
																											<plx:right>
																												<plx:callNew dataTypeName="ProvideSurveyQuestionFor{@questionType}">
																													<plx:passParam>
																														<plx:callThis name="Store" type="property"/>
																													</plx:passParam>
																												</plx:callNew>
																											</plx:right>
																										</plx:assign>
																									</plx:inlineStatement>
																								</plx:right>
																							</plx:nullFallbackOperator>
																						</plx:inlineStatement>
																					</xsl:when>
																					<xsl:otherwise>
																						<plx:callStatic type="field" name="Instance" dataTypeName="ProvideSurveyQuestionFor{@questionType}"/>
																					</xsl:otherwise>
																				</xsl:choose>
																			</xsl:for-each>
																		</plx:arrayInitializer>
																	</plx:callNew>
																</plx:right>
															</plx:assign>
														</plx:inlineStatement>
													</plx:right>
												</plx:nullFallbackOperator>
											</plx:inlineStatement>
										</plx:return>
									</plx:function>
								</xsl:when>
								<xsl:otherwise>
									<plx:field name="mySurveyQuestionTypeInfo" visibility="private" static="true" readOnly="true" dataTypeName="ISurveyQuestionTypeInfo" dataTypeIsSimpleArray="true">
										<plx:initialize>
											<plx:callNew dataTypeName="ISurveyQuestionTypeInfo" dataTypeIsSimpleArray="true">
												<plx:arrayInitializer>
													<xsl:for-each select="$questions">
														<plx:callStatic type="field" name="Instance" dataTypeName="ProvideSurveyQuestionFor{@questionType}"/>
													</xsl:for-each>
												</plx:arrayInitializer>
											</plx:callNew>
										</plx:initialize>
									</plx:field>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:otherwise>
					</xsl:choose>
					<plx:function visibility="protected" modifier="static" name="GetSurveyQuestions">
						<xsl:if test="$dynamicQuestions">
							<xsl:attribute name="modifier">
								<xsl:text>none</xsl:text>
							</xsl:attribute>
						</xsl:if>
						<plx:leadingInfo>
							<plx:docComment>
								<summary>Implements <see cref="ISurveyQuestionProvider.GetSurveyQuestions"/>
								</summary>
							</plx:docComment>
						</plx:leadingInfo>
						<plx:interfaceMember dataTypeName="ISurveyQuestionProvider" memberName="GetSurveyQuestions"/>
						<plx:param name="expansionKey" dataTypeName=".object"/>
						<plx:returns dataTypeName="IEnumerable">
							<plx:passTypeParam dataTypeName="ISurveyQuestionTypeInfo"/>
						</plx:returns>
						<xsl:choose>
							<xsl:when test="$groupings">
								<xsl:for-each select="$groupings">
									<xsl:variable name="elementName">
										<xsl:choose>
											<xsl:when test="position()=1">
												<xsl:text>plx:branch</xsl:text>
											</xsl:when>
											<xsl:otherwise>
												<xsl:text>plx:alternateBranch</xsl:text>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:variable>
									<xsl:element name="{$elementName}">
										<plx:condition>
											<plx:binaryOperator type="identityEquality">
												<plx:left>
													<plx:nameRef name="expansionKey" type="parameter"/>
												</plx:left>
												<plx:right>
													<xsl:variable name="conditionExpression" select="qp:expansionKey/plx:*"/>
													<xsl:choose>
														<xsl:when test="$conditionExpression">
															<xsl:copy-of select="$conditionExpression"/>
														</xsl:when>
														<xsl:otherwise>
															<plx:nullKeyword/>
														</xsl:otherwise>
													</xsl:choose>
												</plx:right>
											</plx:binaryOperator>
										</plx:condition>
										<plx:return>
											<xsl:choose>
												<xsl:when test="qp:surveyQuestion/@ref=$dynamicQuestions/@questionType">
													<plx:callThis name="EnsureSurveyQuestionTypeInfo{position()}"/>
												</xsl:when>
												<xsl:otherwise>
													<plx:callThis accessor="static" type="field" name="mySurveyQuestionTypeInfo{position()}"/>
												</xsl:otherwise>
											</xsl:choose>
										</plx:return>
									</xsl:element>
								</xsl:for-each>
								<plx:return>
									<plx:nullKeyword/>
								</plx:return>
							</xsl:when>
							<xsl:otherwise>
								<plx:return>
									<xsl:choose>
										<xsl:when test="$dynamicQuestions">
											<plx:callThis name="EnsureSurveyQuestionTypeInfo"/>
										</xsl:when>
										<xsl:otherwise>
											<plx:callThis accessor="static" type="field" name="mySurveyQuestionTypeInfo"/>
										</xsl:otherwise>
									</xsl:choose>
								</plx:return>
							</xsl:otherwise>
						</xsl:choose>
					</plx:function>
					<plx:property name="SurveyQuestionImageList" visibility="protected">
						<plx:leadingInfo>
							<plx:docComment>
								<summary>Implements <see cref="ISurveyQuestionProvider.SurveyQuestionImageList"/>
								</summary>
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
					<plx:function visibility="protected" name="GetErrorDisplayTypes" modifier="static">
						<plx:leadingInfo>
							<plx:docComment>
								<summary>Implements <see cref="ISurveyQuestionProvider.GetErrorDisplayTypes"/>
								</summary>
							</plx:docComment>
						</plx:leadingInfo>
						<plx:interfaceMember dataTypeName="ISurveyQuestionProvider" memberName="GetErrorDisplayTypes"/>
						<plx:returns dataTypeName="IEnumerable">
							<plx:passTypeParam dataTypeName="Type"/>
						</plx:returns>
						<plx:return>
							<xsl:variable name="errorTypeNames" select="qp:surveyQuestions/qp:surveyQuestion[@isErrorDisplay='true' or @isErrorDisplay='1']/@questionType"/>
							<xsl:choose>
								<xsl:when test="$errorTypeNames">
									<plx:callNew dataTypeName="Type" dataTypeIsSimpleArray="true">
										<plx:arrayInitializer>
											<xsl:for-each select="$errorTypeNames">
												<plx:typeOf dataTypeName="{.}"/>
											</xsl:for-each>
										</plx:arrayInitializer>
									</plx:callNew>
								</xsl:when>
								<xsl:otherwise>
									<plx:nullKeyword/>
								</xsl:otherwise>
							</xsl:choose>
						</plx:return>
					</plx:function>
					<xsl:apply-templates select="$questions">
						<xsl:with-param name="AllQuestions" select="$questions"/>
					</xsl:apply-templates>
				</plx:class>
			</plx:namespace>
		</plx:root>
	</xsl:template>
	<xsl:template match="qp:surveyQuestion">
		<xsl:param name="AllQuestions"/>
		<xsl:variable name="dynamic" select="@dynamicValues='true' or @dynamicValues='1'"/>
		<plx:class name="ProvideSurveyQuestionFor{@questionType}" visibility="private" modifier="sealed">
			<plx:implementsInterface dataTypeName="ISurveyQuestionTypeInfo"/>
			<xsl:choose>
				<xsl:when test="$dynamic">
					<plx:field name="myDynamicValues" dataTypeName="{@questionType}" visibility="private"/>
					<plx:function name=".construct" visibility="public">
						<plx:param name="store" dataTypeName="Store"/>
						<plx:assign>
							<plx:left>
								<plx:callThis name="myDynamicValues" type="field"/>
							</plx:left>
							<plx:right>
								<plx:callNew dataTypeName="{@questionType}">
									<plx:passParam>
										<plx:nameRef name="store" type="parameter"/>
									</plx:passParam>
								</plx:callNew>
							</plx:right>
						</plx:assign>
					</plx:function>
				</xsl:when>
				<xsl:otherwise>
					<plx:function name=".construct" visibility="private"/>
					<plx:field name="Instance" visibility="public" static="true" readOnly="true" dataTypeName="ISurveyQuestionTypeInfo">
						<plx:initialize>
							<plx:callNew dataTypeName="ProvideSurveyQuestionFor{@questionType}"/>
						</plx:initialize>
					</plx:field>
				</xsl:otherwise>
			</xsl:choose>
			<plx:property name="QuestionType" visibility="public">
				<plx:interfaceMember dataTypeName="ISurveyQuestionTypeInfo" memberName="QuestionType"/>
				<plx:returns dataTypeName="Type"/>
				<plx:get>
					<plx:return>
						<plx:typeOf dataTypeName="{@questionType}"/>
					</plx:return>
				</plx:get>
			</plx:property>
			<plx:property name="DynamicQuestionValues" visibility="public">
				<plx:interfaceMember dataTypeName="ISurveyQuestionTypeInfo" memberName="DynamicQuestionValues"/>
				<plx:returns dataTypeName="ISurveyDynamicValues"/>
				<plx:get>
					<plx:return>
						<xsl:choose>
							<xsl:when test="$dynamic">
								<plx:callThis name="myDynamicValues" type="field"/>
							</xsl:when>
							<xsl:otherwise>
								<plx:nullKeyword/>
							</xsl:otherwise>
						</xsl:choose>
					</plx:return>
				</plx:get>
			</plx:property>
			<plx:function name="AskQuestion" visibility="public">
				<plx:interfaceMember dataTypeName="ISurveyQuestionTypeInfo" memberName="AskQuestion"/>
				<plx:param name="data" dataTypeName=".object"/>
				<plx:param name="contextElement" dataTypeName=".object"/>
				<plx:returns dataTypeName=".i4"/>
				<xsl:variable name="interfaceTypeSnippet">
					<xsl:choose>
						<xsl:when test="$dynamic">
							<xsl:text>IAnswerSurveyDynamicQuestion</xsl:text>
						</xsl:when>
						<xsl:otherwise>
							<xsl:text>IAnswerSurveyQuestion</xsl:text>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:variable name="interfaceType" select="string($interfaceTypeSnippet)"/>
				<plx:local name="typedData" dataTypeName="{$interfaceType}">
					<plx:passTypeParam dataTypeName="{@questionType}"/>
					<plx:initialize>
						<plx:cast dataTypeName="{$interfaceType}" type="testCast">
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
							<xsl:if test="$dynamic">
								<plx:passParam>
									<plx:callThis name="myDynamicValues" type="field"/>
								</plx:passParam>
							</xsl:if>
							<plx:passParam>
								<plx:nameRef name="contextElement" type="parameter"/>
							</plx:passParam>
						</plx:callInstance>
					</plx:return>
				</plx:branch>
				<plx:return>
					<plx:value type="i4" data="-1"/>
				</plx:return>
			</plx:function>
			<plx:function name="MapAnswerToImageIndex" visibility="public" xmlns:plx="http://schemas.neumont.edu/CodeGeneration/PLiX">
				<plx:interfaceMember dataTypeName="ISurveyQuestionTypeInfo" memberName="MapAnswerToImageIndex"/>
				<plx:param name="answer" dataTypeName=".i4"/>
				<plx:returns dataTypeName=".i4"/>
				<xsl:variable name="imageMap" select="qp:sequentialImageMap | qp:explicitImageMap"/>
				<xsl:choose>
					<xsl:when test="$imageMap and qp:displaySupport[@displayCategory='Glyph' or @displayCategory='Overlay']">
						<xsl:variable name="offsetFragment">
							<xsl:call-template name="ResolveOffset">
								<xsl:with-param name="ImageMap" select="$imageMap"/>
								<xsl:with-param name="AllQuestions" select="$AllQuestions"/>
							</xsl:call-template>
						</xsl:variable>
						<xsl:variable name="offset" select="exsl:node-set($offsetFragment)/child::*"/>
						<xsl:choose>
							<xsl:when test="$imageMap[self::qp:sequentialImageMap]">
								<plx:return>
									<xsl:choose>
										<xsl:when test="$offset">
											<plx:binaryOperator type="add">
												<plx:left>
													<xsl:copy-of select="$offset"/>
												</plx:left>
												<plx:right>
													<plx:nameRef name="answer" type="parameter"/>
												</plx:right>
											</plx:binaryOperator>
										</xsl:when>
										<xsl:otherwise>
											<plx:nameRef name="answer" type="parameter"/>
										</xsl:otherwise>
									</xsl:choose>
								</plx:return>
							</xsl:when>
							<xsl:otherwise>
								<xsl:variable name="questionType" select="string(@questionType)"/>
								<plx:local name="retVal" dataTypeName=".i4"/>
								<plx:switch>
									<plx:condition>
										<plx:cast dataTypeName="{$questionType}">
											<plx:nameRef name="answer" type="parameter"/>
										</plx:cast>
									</plx:condition>
									<xsl:for-each select="$imageMap/qp:map">
										<plx:case>
											<plx:condition>
												<plx:callStatic dataTypeName="{$questionType}" name="{@enumValue}" type="field"/>
											</plx:condition>
											<xsl:for-each select="$imageMap/qp:mapSameAs[@targetEnumValue=current()/@enumValue]">
												<plx:condition>
													<plx:callStatic dataTypeName="{$questionType}" name="{@enumValue}" type="field"/>
												</plx:condition>
											</xsl:for-each>
											<plx:assign>
												<plx:left>
													<plx:nameRef name="retVal"/>
												</plx:left>
												<plx:right>
													<xsl:choose>
														<xsl:when test="@imageIndex='.custom'">
															<xsl:choose>
																<xsl:when test="$offset">
																	<plx:binaryOperator type="add">
																		<plx:left>
																			<xsl:copy-of select="$offset"/>
																		</plx:left>
																		<plx:right>
																			<xsl:copy-of select="child::plx:*"/>
																		</plx:right>
																	</plx:binaryOperator>
																</xsl:when>
																<xsl:otherwise>
																	<xsl:copy-of select="child::plx:*"/>
																</xsl:otherwise>
															</xsl:choose>
														</xsl:when>
														<xsl:otherwise>
															<xsl:variable name="mapOffset" select="child::plx:*"/>
															<xsl:choose>
																<xsl:when test="$mapOffset">
																	<plx:binaryOperator type="add">
																		<plx:left>
																			<xsl:choose>
																				<xsl:when test="$offset">
																					<plx:binaryOperator type="add">
																						<plx:left>
																							<xsl:copy-of select="$offset"/>
																						</plx:left>
																						<plx:right>
																							<xsl:copy-of select="$mapOffset"/>
																						</plx:right>
																					</plx:binaryOperator>
																				</xsl:when>
																				<xsl:otherwise>
																					<xsl:copy-of select="$mapOffset"/>
																				</xsl:otherwise>
																			</xsl:choose>
																		</plx:left>
																		<plx:right>
																			<plx:value data="{@imageIndex}" type="i4"/>
																		</plx:right>
																	</plx:binaryOperator>
																</xsl:when>
																<xsl:otherwise>
																	<xsl:choose>
																		<xsl:when test="$offset">
																			<plx:binaryOperator type="add">
																				<plx:left>
																					<xsl:copy-of select="$offset"/>
																				</plx:left>
																				<plx:right>
																					<plx:value data="{@imageIndex}" type="i4"/>
																				</plx:right>
																			</plx:binaryOperator>
																		</xsl:when>
																		<xsl:otherwise>
																			<plx:value data="{@imageIndex}" type="i4"/>
																		</xsl:otherwise>
																	</xsl:choose>
																</xsl:otherwise>
															</xsl:choose>
														</xsl:otherwise>
													</xsl:choose>
												</plx:right>
											</plx:assign>
										</plx:case>
									</xsl:for-each>
									<plx:fallbackCase>
										<plx:assign>
											<plx:left>
												<plx:nameRef name="retVal"/>
											</plx:left>
											<plx:right>
												<plx:value data="-1" type="i4"/>
											</plx:right>
										</plx:assign>
									</plx:fallbackCase>
								</plx:switch>
								<plx:return>
									<plx:nameRef name="retVal"/>
								</plx:return>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<plx:return>
							<plx:value type="i4" data="-1"/>
						</plx:return>
					</xsl:otherwise>
				</xsl:choose>
			</plx:function>
			<plx:function name="GetDisplayData" visibility="public">
				<plx:interfaceMember dataTypeName="ISurveyQuestionTypeInfo" memberName="GetDisplayData"/>
				<plx:param name="answer" dataTypeName=".i4"/>
				<plx:returns dataTypeName="SurveyQuestionDisplayData"/>
				<xsl:if test="qp:displaySupport[@displayCategory='DisplayData']">
					<xsl:variable name="displayDataMaps" select="qp:displayDataMap/qp:displayData[(@bold='true' or @bold='1') or (@gray='true' or @gray='1')]"/>
					<xsl:if test="$displayDataMaps">
						<xsl:variable name="questionType" select="string(@questionType)"/>
						<plx:switch>
							<plx:condition>
								<plx:cast dataTypeName="{$questionType}">
									<plx:nameRef name="answer" type="parameter"/>
								</plx:cast>
							</plx:condition>
							<xsl:for-each select="$displayDataMaps">
								<plx:case>
									<plx:condition>
										<plx:callStatic dataTypeName="{$questionType}" name="{@enumValue}" type="field"/>
									</plx:condition>
									<xsl:for-each select="../qp:displayDataSameAs[@targetEnumValue=current()/@enumValue]">
										<plx:condition>
											<plx:callStatic dataTypeName="{$questionType}" name="{@enumValue}" type="field"/>
										</plx:condition>
									</xsl:for-each>
									<plx:return>
										<plx:callNew dataTypeName="SurveyQuestionDisplayData">
											<plx:passParam>
												<!-- The isBold parameter -->
												<xsl:choose>
													<xsl:when test="@bold='true' or @bold='1'">
														<plx:trueKeyword/>
													</xsl:when>
													<xsl:otherwise>
														<plx:falseKeyword/>
													</xsl:otherwise>
												</xsl:choose>
											</plx:passParam>
											<plx:passParam>
												<!-- The isGrayText parameter -->
												<xsl:choose>
													<xsl:when test="@gray='true' or @gray='1'">
														<plx:trueKeyword/>
													</xsl:when>
													<xsl:otherwise>
														<plx:falseKeyword/>
													</xsl:otherwise>
												</xsl:choose>
											</plx:passParam>
										</plx:callNew>
									</plx:return>
								</plx:case>
							</xsl:for-each>
						</plx:switch>
					</xsl:if>	
				</xsl:if>
				<plx:return>
					<plx:callStatic name="Default" dataTypeName="SurveyQuestionDisplayData" type="property"/>
				</plx:return>
			</plx:function>
			<plx:property name="UISupport" visibility="public">
				<plx:interfaceMember dataTypeName="ISurveyQuestionTypeInfo" memberName="UISupport"/>
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
										<xsl:call-template name="OrTogetherDisplaySupportElements">
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
			<plx:property name="QuestionPriority" visibility="public" modifier="static">
				<plx:interfaceMember dataTypeName="ISurveyQuestionTypeInfo" memberName="QuestionPriority"/>
				<plx:returns dataTypeName=".i4"/>
				<plx:get>
					<plx:return>
						<plx:value data="0" type="i4">
							<xsl:if test="@questionPriority">
								<xsl:attribute name="data">
									<xsl:value-of select="@questionPriority"/>
								</xsl:attribute>
							</xsl:if>
						</plx:value>
					</plx:return>
				</plx:get>
			</plx:property>
		</plx:class>
	</xsl:template>
	<xsl:template name="ResolveOffset">
		<xsl:param name="ImageMap"/>
		<xsl:param name="AllQuestions"/>
		<xsl:param name="ApplyLastAnswer" select="false()"/>
		<xsl:variable name="offset" select="$ImageMap/qp:offset"/>
		<xsl:variable name="lastAnswer" select="string($ImageMap/@lastAnswer)"/>
		<xsl:choose>
			<xsl:when test="$offset">
				<xsl:variable name="afterQuestion" select="string($offset/@afterSurveyQuestion)"/>
				<xsl:choose>
					<xsl:when test="$afterQuestion">
						<xsl:variable name="referencedOffsetFragment">
							<xsl:call-template name="ResolveOffset">
								<xsl:with-param name="ImageMap" select="$AllQuestions[@questionType=$afterQuestion]/qp:*[self::qp:sequentialImageMap | self::qp:explicitImageMap]"/>
								<xsl:with-param name="AllQuestions" select="$AllQuestions"/>
								<xsl:with-param name="ApplyLastAnswer" select="true()"/>
							</xsl:call-template>
						</xsl:variable>
						<xsl:variable name="referencedOffset" select="exsl:node-set($referencedOffsetFragment)/child::*"/>
						<xsl:variable name="localOffsetFragment">
							<xsl:choose>
								<xsl:when test="$ApplyLastAnswer and $lastAnswer">
									<xsl:variable name="customOffset" select="$offset/child::plx:*"/>
									<xsl:variable name="lastAnswerOffsetFragment">
										<plx:binaryOperator type="add">
											<plx:left>
												<plx:cast dataTypeName=".i4">
													<plx:callStatic dataTypeName="{$ImageMap/../@questionType}" name="{$lastAnswer}" type="field"/>
												</plx:cast>
											</plx:left>
											<plx:right>
												<plx:value data="1" type="i4"/>
											</plx:right>
										</plx:binaryOperator>
									</xsl:variable>
									<xsl:choose>
										<xsl:when test="$customOffset">
											<plx:binaryOperator type="add">
												<plx:left>
													<xsl:copy-of select="$lastAnswerOffsetFragment"/>
												</plx:left>
												<plx:right>
													<xsl:copy-of select="$customOffset"/>
												</plx:right>
											</plx:binaryOperator>
										</xsl:when>
										<xsl:otherwise>
											<xsl:copy-of select="$lastAnswerOffsetFragment"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:copy-of select="$offset/child::plx:*"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:variable name="localOffset" select="exsl:node-set($localOffsetFragment)/child::*"/>
						<xsl:choose>
							<xsl:when test="$referencedOffset">
								<xsl:choose>
									<xsl:when test="$localOffset">
										<plx:binaryOperator type="add">
											<plx:left>
												<xsl:copy-of select="$referencedOffset"/>
											</plx:left>
											<plx:right>
												<xsl:copy-of select="$localOffset"/>
											</plx:right>
										</plx:binaryOperator>
									</xsl:when>
									<xsl:otherwise>
										<xsl:copy-of select="$referencedOffset"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:when test="$localOffset">
								<xsl:copy-of select="$localOffset"/>
							</xsl:when>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:copy-of select="$offset/child::plx:*"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<xsl:when test="$ApplyLastAnswer and $lastAnswer">
				<plx:binaryOperator type="add">
					<plx:left>
						<plx:cast dataTypeName=".i4">
							<plx:callStatic dataTypeName="{$ImageMap/../@questionType}" name="{$lastAnswer}" type="field"/>
						</plx:cast>
					</plx:left>
					<plx:right>
						<plx:value data="1" type="i4"/>
					</plx:right>
				</plx:binaryOperator>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
	<!-- Or together enum values from the given type. The current state on the initial
	     call should be the position()=1 element inside a for-each context where the elements
		 contain the (unqualified) names of the enum values to or together -->
	<xsl:template name="OrTogetherDisplaySupportElements">
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
						<xsl:for-each select="following-sibling::qp:displaySupport">
							<xsl:if test="position()=1">
								<xsl:call-template name="OrTogetherDisplaySupportElements">
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