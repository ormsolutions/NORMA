<?xml version="1.0" encoding="utf-8"?>
<!--
	Natural Object-Role Modeling Architect for Visual Studio

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
	<xsl:output method="xml" encoding="utf-8" indent="yes"/>

	<!-- Pick up param value supplied automatically by plix loader -->
	<xsl:param name="CustomToolNamespace" select="'TestNamespace'"/>
	<xsl:variable name="contextType" select="*/qp:surveyContextType"/>
	<xsl:variable name="contextTypeParam">
		<plx:passTypeParam>
			<xsl:copy-of select="$contextType/@* | $contextType/*"/>
		</plx:passTypeParam>
	</xsl:variable>
	<xsl:variable name="contextParam">
		<plx:param name="surveyContext">
			<xsl:copy-of select="$contextType/@* | $contextType/*"/>
		</plx:param>
	</xsl:variable>
	<xsl:variable name="contextPassParam">
		<plx:passParam>
			<plx:nameRef name="surveyContext" type="parameter"/>
		</plx:passParam>
	</xsl:variable>
	<xsl:template match="qp:surveyQuestionProvider">
		<xsl:variable name="questions" select="qp:surveyQuestions/qp:surveyQuestion"/>
		<xsl:variable name="dynamicQuestions" select="$questions[@dynamicValues='true' or @dynamicValues='1']"/>
		<plx:root>
			<plx:namespaceImport name="System"/>
			<plx:namespaceImport name="System.Windows.Forms"/>
			<plx:namespaceImport name="System.Collections.Generic"/>
			<plx:namespaceImport name="ORMSolutions.ORMArchitect.Framework.Shell"/>
			<plx:namespaceImport name="ORMSolutions.ORMArchitect.Framework.Shell.DynamicSurveyTreeGrid"/>
			<plx:namespace name="{$CustomToolNamespace}">
				<plx:class name="{@class}" partial="true" visibility="deferToPartial">
					<plx:implementsInterface dataTypeName="ISurveyQuestionProvider">
						<xsl:copy-of select="$contextTypeParam"/>
					</plx:implementsInterface>
					<xsl:variable name="groupings" select="qp:groupings/qp:grouping"/>
					<xsl:for-each select="$dynamicQuestions">
						<plx:field name="myDynamic{@questionType}QuestionInstance" dataTypeName="ProvideSurveyQuestionFor{@questionType}" visibility="private"/>
					</xsl:for-each>
					<xsl:choose>
						<xsl:when test="$groupings">
							<xsl:for-each select="$groupings">
								<xsl:choose>
									<xsl:when test="qp:surveyQuestion/@ref=$dynamicQuestions/@questionType">
										<plx:field name="mySurveyQuestionTypeInfo{position()}" visibility="private" dataTypeName="ISurveyQuestionTypeInfo" dataTypeIsSimpleArray="true">
											<xsl:copy-of select="$contextTypeParam"/>
										</plx:field>
										<plx:function name="EnsureSurveyQuestionTypeInfo{position()}" visibility="private">
											<xsl:copy-of select="$contextParam"/>
											<plx:returns dataTypeName="ISurveyQuestionTypeInfo" dataTypeIsSimpleArray="true">
												<xsl:copy-of select="$contextTypeParam"/>
											</plx:returns>
											<plx:return>
												<plx:inlineStatement dataTypeName="ISurveyQuestionTypeInfo" dataTypeIsSimpleArray="true">
													<xsl:copy-of select="$contextTypeParam"/>
													<plx:nullFallbackOperator>
														<plx:left>
															<plx:callThis name="mySurveyQuestionTypeInfo{position()}" type="field"/>
														</plx:left>
														<plx:right>
															<plx:inlineStatement dataTypeName="ISurveyQuestionTypeInfo" dataTypeIsSimpleArray="true">
																<xsl:copy-of select="$contextTypeParam"/>
																<plx:assign>
																	<plx:left>
																		<plx:callThis name="mySurveyQuestionTypeInfo{position()}" type="field"/>
																	</plx:left>
																	<plx:right>
																		<plx:callNew dataTypeName="ISurveyQuestionTypeInfo" dataTypeIsSimpleArray="true">
																			<xsl:copy-of select="$contextTypeParam"/>
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
																														<xsl:copy-of select="$contextPassParam"/>
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
											<xsl:copy-of select="$contextTypeParam"/>
											<plx:initialize>
												<plx:callNew dataTypeName="ISurveyQuestionTypeInfo" dataTypeIsSimpleArray="true">
													<xsl:copy-of select="$contextTypeParam"/>
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
									<plx:field name="mySurveyQuestionTypeInfo" visibility="private" dataTypeName="ISurveyQuestionTypeInfo" dataTypeIsSimpleArray="true">
										<xsl:copy-of select="$contextTypeParam"/>
									</plx:field>
									<plx:function name="EnsureSurveyQuestionTypeInfo" visibility="private">
										<xsl:copy-of select="$contextParam"/>
										<plx:returns dataTypeName="ISurveyQuestionTypeInfo" dataTypeIsSimpleArray="true">
											<xsl:copy-of select="$contextTypeParam"/>
										</plx:returns>
										<plx:return>
											<plx:inlineStatement dataTypeName="ISurveyQuestionTypeInfo" dataTypeIsSimpleArray="true">
												<xsl:copy-of select="$contextTypeParam"/>
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
																													<xsl:copy-of select="$contextPassParam"/>
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
										<xsl:copy-of select="$contextTypeParam"/>
										<plx:initialize>
											<plx:callNew dataTypeName="ISurveyQuestionTypeInfo" dataTypeIsSimpleArray="true">
												<xsl:copy-of select="$contextTypeParam"/>
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
								<summary>Implements <see cref="ISurveyQuestionProvider{{Object}}.GetSurveyQuestions"/>
								</summary>
							</plx:docComment>
						</plx:leadingInfo>
						<plx:interfaceMember dataTypeName="ISurveyQuestionProvider" memberName="GetSurveyQuestions">
							<xsl:copy-of select="$contextTypeParam"/>
						</plx:interfaceMember>
						<xsl:copy-of select="$contextParam"/>
						<plx:param name="expansionKey" dataTypeName=".object"/>
						<plx:returns dataTypeName="IEnumerable">
							<plx:passTypeParam dataTypeName="ISurveyQuestionTypeInfo">
								<xsl:copy-of select="$contextTypeParam"/>
							</plx:passTypeParam>
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
													<plx:callThis name="EnsureSurveyQuestionTypeInfo{position()}">
														<xsl:copy-of select="$contextPassParam"/>
													</plx:callThis>
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
											<plx:callThis name="EnsureSurveyQuestionTypeInfo">
												<xsl:copy-of select="$contextPassParam"/>
											</plx:callThis>
										</xsl:when>
										<xsl:otherwise>
											<plx:callThis accessor="static" type="field" name="mySurveyQuestionTypeInfo"/>
										</xsl:otherwise>
									</xsl:choose>
								</plx:return>
							</xsl:otherwise>
						</xsl:choose>
					</plx:function>
					<plx:function name="GetSurveyQuestionImageLists" visibility="protected">
						<plx:leadingInfo>
							<plx:docComment>
								<summary>Implements <see cref="ISurveyQuestionProvider{{Object}}.GetSurveyQuestionImageLists"/>
								</summary>
							</plx:docComment>
						</plx:leadingInfo>
						<plx:interfaceMember dataTypeName="ISurveyQuestionProvider" memberName="GetSurveyQuestionImageLists">
							<xsl:copy-of select="$contextTypeParam"/>
						</plx:interfaceMember>
						<xsl:copy-of select="$contextParam"/>
						<plx:returns dataTypeName="ImageList" dataTypeIsSimpleArray="true"/>
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
					</plx:function>
					<plx:function visibility="protected" name="GetErrorDisplayTypes" modifier="static">
						<plx:leadingInfo>
							<plx:docComment>
								<summary>Implements <see cref="ISurveyQuestionProvider{{Object}}.GetErrorDisplayTypes"/>
								</summary>
							</plx:docComment>
						</plx:leadingInfo>
						<plx:interfaceMember dataTypeName="ISurveyQuestionProvider" memberName="GetErrorDisplayTypes">
							<xsl:copy-of select="$contextTypeParam"/>
						</plx:interfaceMember>
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
			<plx:implementsInterface dataTypeName="ISurveyQuestionTypeInfo">
				<xsl:copy-of select="$contextTypeParam"/>
			</plx:implementsInterface>
			<xsl:choose>
				<xsl:when test="$dynamic">
					<plx:field name="myDynamicValues" dataTypeName="{@questionType}" visibility="private"/>
					<plx:function name=".construct" visibility="public">
						<xsl:copy-of select="$contextParam"/>
						<plx:assign>
							<plx:left>
								<plx:callThis name="myDynamicValues" type="field"/>
							</plx:left>
							<plx:right>
								<plx:callNew dataTypeName="{@questionType}">
									<xsl:copy-of select="$contextPassParam"/>
								</plx:callNew>
							</plx:right>
						</plx:assign>
					</plx:function>
				</xsl:when>
				<xsl:otherwise>
					<plx:function name=".construct" visibility="private"/>
					<plx:field name="Instance" visibility="public" static="true" readOnly="true" dataTypeName="ISurveyQuestionTypeInfo">
						<xsl:copy-of select="$contextTypeParam"/>
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
			<plx:function name="MapAnswerToImageIndex" visibility="public">
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
			<plx:function name="GetFreeFormCommands" visibility="public">
				<plx:interfaceMember dataTypeName="ISurveyQuestionTypeInfo" memberName="GetFreeFormCommands">
					<xsl:copy-of select="$contextTypeParam"/>
				</plx:interfaceMember>
				<xsl:copy-of select="$contextParam"/>
				<plx:param name="answer" dataTypeName=".i4"/>
				<plx:returns dataTypeName="IFreeFormCommandProvider">
					<xsl:copy-of select="$contextTypeParam"/>
				</plx:returns>
				<xsl:if test="qp:displaySupport[@displayCategory='Grouping']">
					<xsl:variable name="commandMaps" select="qp:commandProviderMap/qp:commandProvider"/>
					<xsl:if test="$commandMaps">
						<xsl:variable name="questionType" select="string(@questionType)"/>
						<plx:switch>
							<plx:condition>
								<plx:cast dataTypeName="{$questionType}">
									<plx:nameRef name="answer" type="parameter"/>
								</plx:cast>
							</plx:condition>
							<xsl:for-each select="$commandMaps">
								<plx:case>
									<plx:condition>
										<plx:callStatic dataTypeName="{$questionType}" name="{@enumValue}" type="field"/>
									</plx:condition>
									<xsl:for-each select="../qp:commandProviderSameAs[@targetEnumValue=current()/@enumValue]">
										<plx:condition>
											<plx:callStatic dataTypeName="{$questionType}" name="{@enumValue}" type="field"/>
										</plx:condition>
									</xsl:for-each>
									<xsl:copy-of select="child::plx:*"/>
								</plx:case>
							</xsl:for-each>
						</plx:switch>
					</xsl:if>
				</xsl:if>
				<plx:return>
					<plx:nullKeyword/>
				</plx:return>
			</plx:function>
			<plx:function name="ShowEmptyGroup" visibility="public">
				<plx:interfaceMember dataTypeName="ISurveyQuestionTypeInfo" memberName="ShowEmptyGroup">
					<xsl:copy-of select="$contextTypeParam"/>
				</plx:interfaceMember>
				<xsl:copy-of select="$contextParam"/>
				<plx:param name="answer" dataTypeName=".i4"/>
				<plx:returns dataTypeName=".boolean"/>
				<xsl:choose>
					<xsl:when test="qp:displaySupport[@displayCategory='Grouping']">
						<xsl:variable name="emptyHeaderSupport" select="qp:emptyHeaderMap"/>
						<xsl:choose>
							<xsl:when test="$emptyHeaderSupport">
								<xsl:variable name="defaultTrue" select="$emptyHeaderSupport/@defaultVisible='true' or $emptyHeaderSupport/@defaultVisible=1"/>
								<xsl:variable name="customValues" select="$emptyHeaderSupport/qp:map"/>
								<xsl:choose>
									<xsl:when test="$customValues">
										<xsl:variable name="questionType" select="string(@questionType)"/>
										<xsl:variable name="mapSameAs" select="$emptyHeaderSupport/qp:mapSameAs"/>
										<plx:switch>
											<plx:condition>
												<plx:cast dataTypeName="{$questionType}">
													<plx:nameRef name="answer" type="parameter"/>
												</plx:cast>
											</plx:condition>
											<xsl:variable name="reverseCustomValues" select="$customValues[not(child::plx:*)]"/>
											<xsl:if test="$reverseCustomValues">
												<plx:case>
													<xsl:for-each select="$reverseCustomValues">
														<plx:condition>
															<plx:callStatic dataTypeName="{$questionType}" name="{@enumValue}" type="field"/>
														</plx:condition>
														<xsl:for-each select="$mapSameAs[@targetEnumValue=current()/@enumValue]">
															<plx:condition>
																<plx:callStatic dataTypeName="{$questionType}" name="{@enumValue}" type="field"/>
															</plx:condition>
														</xsl:for-each>
													</xsl:for-each>
													<plx:return>
														<xsl:choose>
															<xsl:when test="$defaultTrue">
																<plx:falseKeyword/>
															</xsl:when>
															<xsl:otherwise>
																<plx:trueKeyword/>
															</xsl:otherwise>
														</xsl:choose>
													</plx:return>
												</plx:case>
											</xsl:if>
											<xsl:for-each select="$customValues[child::plx:*]">
												<plx:case>
													<plx:condition>
														<plx:callStatic dataTypeName="{$questionType}" name="{@enumValue}" type="field"/>
													</plx:condition>
													<xsl:for-each select="$mapSameAs[@targetEnumValue=current()/@enumValue]">
														<plx:condition>
															<plx:callStatic dataTypeName="{$questionType}" name="{@enumValue}" type="field"/>
														</plx:condition>
													</xsl:for-each>
													<xsl:copy-of select="child::plx:*"/>
												</plx:case>
											</xsl:for-each>
											<plx:fallbackCase>
												<plx:return>
													<xsl:choose>
														<xsl:when test="$defaultTrue">
															<plx:trueKeyword/>
														</xsl:when>
														<xsl:otherwise>
															<plx:falseKeyword/>
														</xsl:otherwise>
													</xsl:choose>
												</plx:return>
											</plx:fallbackCase>
										</plx:switch>
									</xsl:when>
									<xsl:when test="$defaultTrue">
										<plx:return>
											<plx:trueKeyword/>
										</plx:return>
									</xsl:when>
									<xsl:otherwise>
										<plx:return>
											<plx:falseKeyword/>
										</plx:return>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<plx:return>
									<plx:falseKeyword/>
								</plx:return>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<plx:return>
							<plx:falseKeyword/>
						</plx:return>
					</xsl:otherwise>
				</xsl:choose>
			</plx:function>
			<plx:function name="GetDisplayData" visibility="public">
				<plx:interfaceMember dataTypeName="ISurveyQuestionTypeInfo" memberName="GetDisplayData"/>
				<plx:param name="answer" dataTypeName=".i4"/>
				<plx:returns dataTypeName="SurveyQuestionDisplayData"/>
				<xsl:choose>
					<xsl:when test="qp:displaySupport[@displayCategory='DisplayData']">
						<xsl:variable name="colorMap" select="qp:displayDataMap/qp:colorMap/plx:*"/>
						<xsl:variable name="displayDataMaps" select="qp:displayDataMap/qp:displayData[(@bold='true' or @bold='1') or (@gray='true' or @gray='1')]"/>
						<xsl:variable name="questionType" select="string(@questionType)"/>
						<xsl:if test="$colorMap | $displayDataMaps">
							<plx:local name="typedAnswer" dataTypeName="{$questionType}">
								<plx:initialize>
									<plx:cast dataTypeName="{$questionType}">
										<plx:nameRef name="answer" type="parameter"/>
									</plx:cast>
								</plx:initialize>
							</plx:local>
						</xsl:if>
						<xsl:if test="$colorMap">
							<plx:local name="foreColor" dataTypeName="Color" dataTypeQualifier="System.Drawing">
								<plx:initialize>
									<plx:callStatic name="Empty" type="field" dataTypeName="Color" dataTypeQualifier="System.Drawing"/>
								</plx:initialize>
							</plx:local>
							<plx:local name="backColor" dataTypeName="Color" dataTypeQualifier="System.Drawing">
								<plx:initialize>
									<plx:callStatic name="Empty" type="field" dataTypeName="Color" dataTypeQualifier="System.Drawing"/>
								</plx:initialize>
							</plx:local>
							<xsl:for-each select="$colorMap">
								<xsl:copy>
									<xsl:copy-of select="@*"/>
									<plx:passParam>
										<plx:nameRef name="typedAnswer"/>
									</plx:passParam>
									<plx:passParam type="inOut">
										<plx:nameRef name="foreColor"/>
									</plx:passParam>
									<plx:passParam type="inOut">
										<plx:nameRef name="backColor"/>
									</plx:passParam>
								</xsl:copy>
							</xsl:for-each>
						</xsl:if>
						<xsl:if test="$displayDataMaps">
							<plx:switch>
								<plx:condition>
									<plx:nameRef name="typedAnswer"/>
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
												<xsl:if test="$colorMap">
													<plx:passParam>
														<plx:nameRef name="foreColor"/>
													</plx:passParam>
													<plx:passParam>
														<plx:nameRef name="backColor"/>
													</plx:passParam>
												</xsl:if>
											</plx:callNew>
										</plx:return>
									</plx:case>
								</xsl:for-each>
							</plx:switch>
						</xsl:if>
						<plx:return>
							<xsl:choose>
								<xsl:when test="$colorMap">
									<plx:callNew dataTypeName="SurveyQuestionDisplayData">
										<plx:passParam>
											<plx:nameRef name="foreColor"/>
										</plx:passParam>
										<plx:passParam>
											<plx:nameRef name="backColor"/>
										</plx:passParam>
									</plx:callNew>
								</xsl:when>
								<xsl:otherwise>
									<plx:callStatic name="Default" dataTypeName="SurveyQuestionDisplayData" type="property"/>
								</xsl:otherwise>
							</xsl:choose>
						</plx:return>
					</xsl:when>
					<xsl:otherwise>
						<plx:return>
							<plx:callStatic name="Default" dataTypeName="SurveyQuestionDisplayData" type="property"/>
						</plx:return>
					</xsl:otherwise>
				</xsl:choose>
			</plx:function>
			<plx:property name="UISupport" visibility="public">
				<plx:interfaceMember dataTypeName="ISurveyQuestionTypeInfo" memberName="UISupport"/>
				<plx:returns dataTypeName="SurveyQuestionUISupport"/>
				<plx:get>
					<plx:return>
						<xsl:variable name="supportedCategoriesFragment">
							<xsl:for-each select="qp:displaySupport/@displayCategory">
								<qp:dummy>
									<xsl:value-of select="."/>
								</qp:dummy>
							</xsl:for-each>
							<xsl:if test="qp:emptyHeaderMap[@defaultVisible='true' or @defaultVisible=1 or qp:map]">
								<qp:dummy>
									<xsl:text>EmptyGroups</xsl:text>
								</qp:dummy>
							</xsl:if>
						</xsl:variable>
						<xsl:call-template name="OrTogetherEnumItems">
							<xsl:with-param name="EnumType" select="'SurveyQuestionUISupport'"/>
							<xsl:with-param name="ItemNames" select="exsl:node-set($supportedCategoriesFragment)/child::*"/>
						</xsl:call-template>
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
					<xsl:when test="$afterQuestion or ($lastAnswer and $ApplyLastAnswer)">
						<xsl:variable name="referencedOffsetFragment">
							<xsl:if test="$afterQuestion">
								<xsl:call-template name="ResolveOffset">
									<xsl:with-param name="ImageMap" select="$AllQuestions[@questionType=$afterQuestion]/qp:*[self::qp:sequentialImageMap | self::qp:explicitImageMap]"/>
									<xsl:with-param name="AllQuestions" select="$AllQuestions"/>
									<xsl:with-param name="ApplyLastAnswer" select="true()"/>
								</xsl:call-template>
							</xsl:if>
						</xsl:variable>
						<xsl:variable name="referencedOffset" select="exsl:node-set($referencedOffsetFragment)/child::*"/>
						<xsl:variable name="localOffsetFragment">
							<xsl:choose>
								<xsl:when test="$ApplyLastAnswer and $lastAnswer">
									<xsl:variable name="customOffset" select="$offset/child::plx:*"/>
									<xsl:variable name="lastAnswerOffsetFragment">
										<xsl:choose>
											<xsl:when test="'NaN'=string(number($lastAnswer))">
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
											<xsl:otherwise>
												<plx:value data="{floor(number($lastAnswer)) + 1}" type="i4"/>
											</xsl:otherwise>
										</xsl:choose>
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
				<xsl:choose>
					<xsl:when test="'NaN'=string(number($lastAnswer))">
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
					<xsl:otherwise>
						<plx:value data="{floor(number($lastAnswer)) + 1}" type="i4"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
	<!-- Or together enum values from the given type. ItemNames is any node-set where
	the text of each node is an enum item name.  -->
	<xsl:template name="OrTogetherEnumItems">
		<xsl:param name="EnumType"/>
		<xsl:param name="ItemNames"/>
		<xsl:param name="CurrentIndex" select="1"/>
		<xsl:param name="ItemCount"	select="count($ItemNames)"/>
		<xsl:choose>
			<xsl:when test="$CurrentIndex=$ItemCount">
				<plx:callStatic dataTypeName="{$EnumType}" name="{$ItemNames[$CurrentIndex]}" type="field"/>
			</xsl:when>
			<xsl:otherwise>
				<plx:binaryOperator type="bitwiseOr">
					<plx:left>
						<plx:callStatic dataTypeName="{$EnumType}" name="{$ItemNames[$CurrentIndex]}" type="field"/>
					</plx:left>
					<plx:right>
						<xsl:call-template name="OrTogetherEnumItems">
							<xsl:with-param name="EnumType" select="$EnumType"/>
							<xsl:with-param name="ItemNames" select="$ItemNames"/>
							<xsl:with-param name="CurrentIndex" select="$CurrentIndex+1"/>
							<xsl:with-param name="ItemCount" select="$ItemCount"/>
						</xsl:call-template>
					</plx:right>
				</plx:binaryOperator>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>