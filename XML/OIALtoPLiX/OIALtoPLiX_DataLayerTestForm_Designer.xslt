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
	xmlns:exsl="http://exslt.org/common"
	xmlns:oil="http://schemas.orm.net/OIAL"
	xmlns:odt="http://schemas.orm.net/ORMDataTypes"
	xmlns:plx="http://schemas.neumont.edu/CodeGeneration/PLiX"
	xmlns:prop="urn:schemas-orm-net:PLiX:CLI:Properties"
	exclude-result-prefixes="oil odt"
	extension-element-prefixes="exsl">

	<xsl:output method="xml" encoding="utf-8" media-type="text/xml" indent="yes"/>

	<xsl:variable name="SprocSuffix" select="'SP'"/>
	<xsl:variable name="CollectionSuffix" select="'Collections'"/>
	<xsl:variable name="debugMode" select="'false'"/>

	<xsl:template match="/">

			<xsl:variable name="Model" select="./child::*"/>
			<xsl:variable name="ModelName" select="$Model/@name"/>
			<xsl:variable name="ModelContextName" select="concat($ModelName,'Context')"/>
			<xsl:variable name="ConceptTypes" select="$Model//oil:conceptType"/>
			<xsl:variable name="InformationTypeFormatMappingsFragment">
				<xsl:apply-templates select="$Model/oil:informationTypeFormats/child::*" mode="GenerateInformationTypeFormatMapping"/>
			</xsl:variable>
			<xsl:variable name="InformationTypeFormatMappings" select="exsl:node-set($InformationTypeFormatMappingsFragment)/child::*"/>
			<xsl:variable name="AllPropertiesFragment">
				<xsl:variable name="ConceptTypeRefs" select="$Model//oil:conceptTypeRef"/>
				<xsl:for-each select="$ConceptTypes">
					<prop:Properties conceptTypeName="{@name}">
						<xsl:apply-templates select="." mode="GenerateProperties">
							<xsl:with-param name="ConceptTypeRefs" select="$ConceptTypeRefs"/>
							<xsl:with-param name="InformationTypeFormatMappings" select="$InformationTypeFormatMappings"/>
						</xsl:apply-templates>
					</prop:Properties>
				</xsl:for-each>
			</xsl:variable>
			<xsl:variable name="AllProperties" select="exsl:node-set($AllPropertiesFragment)/child::*"/>
			<xsl:variable name="AllRoleSequenceUniquenessConstraints" select="$Model//oil:roleSequenceUniquenessConstraint"/>

			<xsl:variable name="AllVarsFragment">


				<xsl:for-each select="$ConceptTypes">

					<xsl:variable name="Properties" select="$AllProperties[@conceptTypeName=current()/@name]/prop:Property"/>
					<xsl:variable name="ClassName" select="@name"/>
					<xsl:variable name="mandatoryProperties" select="$Properties[@mandatory='alethic']"/>
					<xsl:variable name="mandatoryPropertiesWithPreferredIdentifiersFragment">
						<xsl:for-each select="$mandatoryProperties">
							<xsl:choose>
								<xsl:when test="@isCustomType='true'">
									<xsl:call-template name="GetPreferredIdentifierProperties">
										<xsl:with-param name="Model" select="$Model"/>
										<xsl:with-param name="AllProperties" select="$AllProperties"/>
										<xsl:with-param name="TargetConceptTypeName" select="prop:DataType/@dataTypeName"/>
										<xsl:with-param name="ParentName" select="@name"/>
									</xsl:call-template>
								</xsl:when>
								<xsl:otherwise>
									<xsl:copy-of select="."/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:for-each>
					</xsl:variable>
					<xsl:variable name="ParametersWithPreferredIdentifiers" select="exsl:node-set($mandatoryPropertiesWithPreferredIdentifiersFragment)/child::*"/>

					<plx:field name="{$ClassName}Page" visibility="private" dataTypeName="TabPage" dataTypeQualifier="System.Windows.Forms"/>
					<plx:field name="gbCreate{$ClassName}" visibility="private" dataTypeName="GroupBox" dataTypeQualifier="System.Windows.Forms"/>

					<plx:assign>
						<plx:left>
							<plx:callInstance name="Name" type="property">
								<plx:callObject>
									<plx:nameRef name="{$ClassName}Page"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:left>
						<plx:right>
							<plx:string><xsl:value-of select="$ClassName"/></plx:string>
						</plx:right>
					</plx:assign>

					<plx:assign>
						<plx:left>
							<plx:callInstance name="Name" type="property">
								<plx:callObject>
									<plx:nameRef name="gbCreate{$ClassName}"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:left>
						<plx:right>
							<plx:string>Create <xsl:value-of select="$ClassName"/>
							</plx:string>
						</plx:right>
					</plx:assign>

					<plx:assign>
						<plx:left>
							<plx:callInstance name="Text" type="property">
								<plx:callObject>
									<plx:nameRef name="gbCreate{$ClassName}"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:left>
						<plx:right>
							<plx:string>Create <xsl:value-of select="$ClassName"/>
							</plx:string>
						</plx:right>
					</plx:assign>

					<plx:assign>
						<plx:left>
							<plx:callInstance name="Text" type="property">
								<plx:callObject>
									<plx:nameRef name="{$ClassName}Page"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:left>
						<plx:right>
							<plx:string><xsl:value-of select="$ClassName"/></plx:string>
						</plx:right>
					</plx:assign>

					<xsl:for-each select="$ParametersWithPreferredIdentifiers">
						<plx:field name="tb{$ClassName}_{@parentName}{@name}" visibility="private" dataTypeName="TextBox" dataTypeQualifier="System.Windows.Forms"/>
						<plx:field name="lb{$ClassName}_{@parentName}{@name}" visibility="private" dataTypeName="Label" dataTypeQualifier="System.Windows.Forms"/>

						<plx:assign>
							<plx:left>
								<plx:callInstance name="Size" type="property">
									<plx:callObject>
										<plx:nameRef name="tb{$ClassName}_{@parentName}{@name}"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:left>
							<plx:right>
								<plx:callNew dataTypeName="Size" type="new" dataTypeQualifier="System.Drawing">
									<plx:passParam>
										<plx:value data="192" type="i4"/>
									</plx:passParam>
									<plx:passParam>
										<plx:value data="72" type="i4"/>
									</plx:passParam>
								</plx:callNew>
							</plx:right>
						</plx:assign>

						<plx:assign>
							<plx:left>
								<plx:callInstance name="Location" type="property">
									<plx:callObject>
										<plx:nameRef name="lb{$ClassName}_{@parentName}{@name}"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:left>
							<plx:right>
								<plx:callNew dataTypeName="Point" type="new" dataTypeQualifier="System.Drawing">
									<plx:passParam>
										<plx:value data="10" type="i4"/>
									</plx:passParam>
									<plx:passParam>
										<plx:value data="{(25*position())}" type="i4"/>
									</plx:passParam>
								</plx:callNew>
							</plx:right>
						</plx:assign>

						<plx:assign>
							<plx:left>
								<plx:callInstance name="Location" type="property">
									<plx:callObject>
										<plx:nameRef name="tb{$ClassName}_{@parentName}{@name}"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:left>
							<plx:right>
								<plx:callNew dataTypeName="Point" type="new" dataTypeQualifier="System.Drawing">
									<plx:passParam>
										<plx:value data="100" type="i4"/>
									</plx:passParam>
									<plx:passParam>
										<plx:value data="{(25*position())}" type="i4"/>
									</plx:passParam>
								</plx:callNew>
							</plx:right>
						</plx:assign>

						<plx:callInstance name="Add" type="methodCall">
							<plx:callObject>
								<plx:callInstance name="Controls" type="property">
									<plx:callObject>
										<plx:nameRef name="gbCreate{$ClassName}"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:callObject>
							<plx:passParam>
								<plx:nameRef name="tb{$ClassName}_{@parentName}{@name}"/>
							</plx:passParam>
						</plx:callInstance>

						<plx:callInstance name="Add" type="methodCall">
							<plx:callObject>
								<plx:callInstance name="Controls" type="property">
									<plx:callObject>
										<plx:nameRef name="gbCreate{$ClassName}"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:callObject>
							<plx:passParam>
								<plx:nameRef name="lb{$ClassName}_{@parentName}{@name}"/>
							</plx:passParam>
						</plx:callInstance>

						<plx:assign>
							<plx:left>
								<plx:callInstance name="Name" type="property">
									<plx:callObject>
										<plx:nameRef name="tb{$ClassName}_{@parentName}{@name}"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:left>
							<plx:right>
								<plx:string>tb<xsl:value-of select="@parentName"/><xsl:value-of select="@name"/></plx:string>
							</plx:right>
						</plx:assign>
						<!--this.label1.Text = "label1";-->
						<plx:assign>
							<plx:left>
								<plx:callInstance name="Text" type="property">
									<plx:callObject>
										<plx:nameRef name="lb{$ClassName}_{@parentName}{@name}"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:left>
							<plx:right>
								<plx:string><xsl:value-of select="@parentName"/><xsl:value-of select="@name"/></plx:string>
							</plx:right>
						</plx:assign>
						<!--this.label1.Name = "label1";-->
						<plx:assign>
							<plx:left>
								<plx:callInstance name="Name" type="property">
									<plx:callObject>
										<plx:nameRef name="lb{$ClassName}_{@parentName}{@name}"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:left>
							<plx:right>
								<plx:string>lb<xsl:value-of select="@parentName"/><xsl:value-of select="@name"/></plx:string>
							</plx:right>
						</plx:assign>
						<!--this.label1.AutoSize = true; -->
						<plx:assign>
							<plx:left>
								<plx:callInstance name="AutoSize" type="property">
									<plx:callObject>
										<plx:nameRef name="lb{$ClassName}_{@parentName}{@name}"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:left>
							<plx:right>
								<plx:trueKeyword/>
							</plx:right>
						</plx:assign>

						<xsl:if test="position()=last()">
							<plx:field name="btnCreate{$ClassName}" visibility="private" dataTypeName="Button" dataTypeQualifier="System.Windows.Forms"/>
							<plx:field name="tb{$ClassName}" visibility="private" dataTypeName="RichTextBox" dataTypeQualifier="System.Windows.Forms"/>
							<plx:assign>
								<plx:left>
									<plx:callInstance name="Location" type="property">
										<plx:callObject>
											<plx:nameRef name="btnCreate{$ClassName}"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:left>
								<plx:right>
									<plx:callNew dataTypeName="Point" type="new" dataTypeQualifier="System.Drawing">
										<plx:passParam>
											<plx:value data="100" type="i4"/>
										</plx:passParam>
										<plx:passParam>
											<plx:value data="{(25*position())+25}" type="i4"/>
										</plx:passParam>
									</plx:callNew>
								</plx:right>
							</plx:assign>
							<plx:assign>
								<plx:left>
									<plx:callInstance name="Text" type="property">
										<plx:callObject>
											<plx:nameRef name="btnCreate{$ClassName}"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:left>
								<plx:right>
									<plx:string>Create</plx:string>
								</plx:right>
							</plx:assign>
							<plx:callInstance name="Add" type="methodCall">
								<plx:callObject>
									<plx:callInstance name="Controls" type="property">
										<plx:callObject>
											<plx:nameRef name="{$ClassName}Page"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:callObject>
								<plx:passParam>
									<plx:nameRef name="btnCreate{$ClassName}"/>
								</plx:passParam>
							</plx:callInstance>
							<!-- this.btnCreateVehicleSale.Click += new System.EventHandler(this.btnCreateVehicleSale_Click); -->
							<plx:attachEvent>
								<plx:left>
									<plx:callInstance name="Click" type="property">
										<plx:callObject>
											<plx:callThis name="btnCreate{$ClassName}" type="property"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:left>
								<plx:right>
									<plx:callNew dataTypeName="EventHandler" dataTypeQualifier="System" type="new">
										<plx:passParam>
											<plx:callThis name="btnCreate{$ClassName}_Click" type="property"/>
										</plx:passParam>
									</plx:callNew>
								</plx:right>
							</plx:attachEvent>

							<plx:assign>
								<plx:left>
									<plx:callInstance name="Location" type="property">
										<plx:callObject>
											<plx:nameRef name="tb{$ClassName}"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:left>
								<plx:right>
									<plx:callNew dataTypeName="Point" type="new" dataTypeQualifier="System.Drawing">
										<plx:passParam>
											<plx:value data="10" type="i4"/>
										</plx:passParam>
										<plx:passParam>
											<plx:value data="{(25*position())+75}" type="i4"/>
										</plx:passParam>
									</plx:callNew>
								</plx:right>
							</plx:assign>

							<plx:assign>
								<plx:left>
									<plx:callInstance name="Size" type="property">
										<plx:callObject>
											<plx:nameRef name="tb{$ClassName}"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:left>
								<plx:right>
									<plx:callNew dataTypeName="Size" type="new" dataTypeQualifier="System.Drawing">
										<plx:passParam>
											<plx:value data="475" type="i4"/>
										</plx:passParam>
										<plx:passParam>
											<plx:value data="300" type="i4"/>
										</plx:passParam>
									</plx:callNew>
								</plx:right>
							</plx:assign>

							<plx:assign>
								<plx:left>
									<plx:callInstance name="Size" type="property">
										<plx:callObject>
											<plx:callThis name="gbCreate{$ClassName}" type="property"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:left>
								<plx:right>
									<plx:callNew dataTypeName="Size" dataTypeQualifier="System.Drawing">
										<plx:passParam>
											<plx:value data="300" type="i4"/>
										</plx:passParam>
										<plx:passParam>
											<plx:value data="{(25*position())+55}" type="i4"/>
										</plx:passParam>
									</plx:callNew>
								</plx:right>
							</plx:assign>

							<plx:callInstance name="Add" type="methodCall">
								<plx:callObject>
									<plx:callInstance name="Controls" type="property">
										<plx:callObject>
											<plx:nameRef name="{$ClassName}Page"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:callObject>
								<plx:passParam>
									<plx:nameRef name="tb{$ClassName}"/>
								</plx:passParam>
							</plx:callInstance>


							<plx:function name="btnCreate{$ClassName}_Click" visibility="private">
								<plx:param name="sender" dataTypeName=".object"/>
								<plx:param name="e" dataTypeName="EventArgs" dataTypeQualifier="System"/>
								<plx:callInstance name="Create{$ClassName}" type="methodCall">
									<plx:callObject>
										<plx:nameRef name="testContext"/>
									</plx:callObject>
									<xsl:for-each select="$ParametersWithPreferredIdentifiers">
										<plx:passParam>
											<xsl:choose>
												<xsl:when test="$mandatoryProperties[@name=current()/@parentName and @isCustomType='true']">
													<!--testContext.GetPersonBySSN(tbVehicleSale_BuyerSSN.Text)-->
													<plx:callInstance name="Get{$mandatoryProperties[@name=current()/@parentName]/*/@dataTypeName}By{@name}">
														<plx:callObject>
															<plx:nameRef name="testContext"/>
														</plx:callObject>
														<plx:passParam>
															<plx:callInstance name="Text" type="property">
																<plx:callObject>
																	<plx:nameRef name="tb{$ClassName}_{@parentName}{@name}"/>
																</plx:callObject>
															</plx:callInstance>
														</plx:passParam>
													</plx:callInstance>
												</xsl:when>
												<xsl:otherwise>
													<plx:callInstance name="Text" type="property">
														<plx:callObject>
															<plx:callThis name="tb{$ClassName}_{@parentName}{@name}" type="property"/>
														</plx:callObject>
													</plx:callInstance>
												</xsl:otherwise>
											</xsl:choose>
										</plx:passParam>
									</xsl:for-each>
								</plx:callInstance>
								<plx:callInstance name="Clear" type="methodCall">
									<plx:callObject>
										<plx:callThis name="tb{$ClassName}" type="field"/>
									</plx:callObject>
								</plx:callInstance>
								<plx:local name="sb" dataTypeName="StringBuilder" dataTypeQualifier="System.Text"/>
								<plx:iterator localName="item" dataTypeName="{$ClassName}" dataTypeQualifier="{$ModelName}">
									<plx:initialize>
										<plx:callInstance name="{$ClassName}Collection" type="property">
											<plx:callObject>
												<plx:nameRef name="testContext"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:initialize>
									<plx:assign>
										<plx:left>
											<plx:nameRef name="sb"/>
										</plx:left>
										<plx:right>
											<plx:callNew dataTypeName="StringBuilder" dataTypeQualifier="System.Text"/>
										</plx:right>
									</plx:assign>

									<xsl:for-each select="$ParametersWithPreferredIdentifiers">
										<plx:callInstance name="Append">
											<plx:callObject>
												<plx:nameRef name="sb"/>
											</plx:callObject>
											<plx:passParam>
												<plx:string> <xsl:value-of select="@name"/>: </plx:string>
											</plx:passParam>
										</plx:callInstance>
										<plx:callInstance name="Append">
											<plx:callObject>
												<plx:nameRef name="sb"/>
											</plx:callObject>
											<plx:passParam>
												<xsl:choose>
													<xsl:when test="@parentName">
														<plx:callInstance name="{@name}" type="property">
															<plx:callObject>
																<plx:callInstance name="{@parentName}" type="property">
																	<plx:callObject>
																		<plx:nameRef name="item"/>
																	</plx:callObject>
																</plx:callInstance>
															</plx:callObject>
														</plx:callInstance>
													</xsl:when>
													<xsl:otherwise>
														<plx:callInstance name="{@name}" type="property">
															<plx:callObject>
																<plx:nameRef name="item"/>
															</plx:callObject>
														</plx:callInstance>
													</xsl:otherwise>
												</xsl:choose>
											</plx:passParam>
										</plx:callInstance>
										<xsl:if test="position()=last()">
											<plx:callInstance name="Append">
												<plx:callObject>
													<plx:nameRef name="sb"/>
												</plx:callObject>
												<plx:passParam>
													<plx:string>\n</plx:string>
												</plx:passParam>
											</plx:callInstance>
										</xsl:if>
									</xsl:for-each>
									
									<plx:callInstance name="AppendText" type="methodCall">
										<plx:callObject>
											<plx:callThis name="tb{$ClassName}" type="field"/>
										</plx:callObject>
										<plx:passParam>
											<plx:callInstance name="ToString">
												<plx:callObject>
													<plx:nameRef name="sb"/>
												</plx:callObject>
											</plx:callInstance>
										</plx:passParam>
									</plx:callInstance>
								</plx:iterator>
							</plx:function>
						</xsl:if>
					</xsl:for-each>

					<plx:callInstance name="Add" type="methodCall">
						<plx:callObject>
							<plx:callInstance name="Controls" type="property">
								<plx:callObject>
									<plx:nameRef name="{$ClassName}Page"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:callObject>
						<plx:passParam>
							<plx:nameRef name="gbCreate{$ClassName}"/>
						</plx:passParam>
					</plx:callInstance>

				</xsl:for-each>
			</xsl:variable>
			<xsl:variable name="AllVars" select="exsl:node-set($AllVarsFragment)/child::*"/>
		<plx:root>
			<!-- We're duplicating the WinForms designer here, which does not use imports. -->
			<!--<xsl:apply-templates select="." mode="AddNamespaceImports"/>-->

			<plx:namespace name="Sample1">
				<plx:class visibility="public" partial="true" name="{$ModelName}Tester">
					<plx:field name="components" visibility="private" dataTypeName="IContainer" dataTypeQualifier="System.ComponentModel">
						<plx:initialize>
							<plx:nullKeyword/>
						</plx:initialize>
					</plx:field>
					<plx:field name="MasterTabControl" visibility="private" dataTypeName="TabControl" dataTypeQualifier="System.Windows.Forms"/>
					<plx:field name="testContext" dataTypeName="{$ModelName}Context" dataTypeQualifier="{$ModelName}" visibility="private"/>
					<plx:field name="connect" dataTypeName="ConnectionDelegate" dataTypeQualifier="{$ModelName}.{$ModelName}Context" visibility="private"/>
					<plx:field name="CONNECTION_STRING" const="true" dataTypeName=".string" visibility="public">
						<plx:initialize>
							<plx:string>Data Source=.\\SQL2005;Initial Catalog=VehicleSale;Integrated Security=True</plx:string>
						</plx:initialize>
					</plx:field>

					<xsl:copy-of select="$AllVars[self::plx:field]"/>

					<plx:function name="Dispose" overload="true" modifier="override" visibility="protected">
						<plx:param name="disposing" dataTypeName=".boolean"/>
						<plx:branch>
							<plx:condition>
								<plx:binaryOperator type="booleanAnd">
									<plx:left>
										<plx:nameRef name="disposing"/>
									</plx:left>
									<plx:right>
										<plx:binaryOperator type="inequality">
											<plx:left>
												<plx:nameRef name="components"/>
											</plx:left>
											<plx:right>
												<plx:nullKeyword/>
											</plx:right>
										</plx:binaryOperator>
									</plx:right>
								</plx:binaryOperator>
							</plx:condition>
							<plx:callInstance name="Dispose" type="methodCall">
								<plx:callObject>
									<plx:nameRef name="components"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:branch>
						<plx:callInstance name="Dispose" type="methodCall">
							<plx:callObject>
								<plx:nameRef name="base"/>
							</plx:callObject>
							<plx:passParam>
								<plx:nameRef name="disposing"/>
							</plx:passParam>
						</plx:callInstance>
					</plx:function>

					<plx:function visibility="private" name="InitializeComponent">

						<plx:assign>
							<plx:left>
								<plx:callThis name="MasterTabControl" type="field"/>
							</plx:left>
							<plx:right>
								<plx:callNew dataTypeName="TabControl" type="new" dataTypeQualifier="System.Windows.Forms"/>
							</plx:right>
						</plx:assign>
						<xsl:for-each select="$AllVars[self::plx:field]">
							<plx:assign>
								<plx:left>
									<plx:callThis name="{@name}" type="field"/>
								</plx:left>
								<plx:right>
									<plx:callNew dataTypeName="{@dataTypeName}" type="new" dataTypeQualifier="{@dataTypeQualifier}"/>
								</plx:right>
							</plx:assign>
						</xsl:for-each>
						<plx:assign>
							<plx:left>
								<plx:callThis name="components" type="field"/>
							</plx:left>
							<plx:right>
								<plx:callNew dataTypeName="Container" dataTypeQualifier="System.ComponentModel" type="new"/>
							</plx:right>
						</plx:assign>

						<plx:assign>
							<plx:left>
								<plx:callThis name="AutoScaleMode" type="field"/>
							</plx:left>
							<plx:right>
								<plx:callStatic name="Font" type="property" dataTypeName="AutoScaleMode" dataTypeQualifier="System.Windows.Forms"/>
							</plx:right>
						</plx:assign>

						<plx:assign>
							<plx:left>
								<plx:callThis name="Text" type="field"/>
							</plx:left>
							<plx:right>
								<plx:string><xsl:value-of select="$ModelName"/>Tester</plx:string>
							</plx:right>
						</plx:assign>

						<xsl:copy-of select="$AllVars[self::plx:callInstance]"/>
						<xsl:copy-of select="$AllVars[self::plx:assign]"/>
						<xsl:copy-of select="$AllVars[self::plx:attachEvent]"/>

						<xsl:for-each select="$AllVars[self::plx:field/@dataTypeName='TabPage']">
							<plx:callInstance name="Add" type="methodCall">
								<plx:callObject>
									<plx:callInstance name="Controls" type="property">
										<plx:callObject>
											<plx:nameRef name="MasterTabControl"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:callObject>
								<plx:passParam>
									<plx:callThis name="{@name}" type="field"/>
								</plx:passParam>
							</plx:callInstance>
						</xsl:for-each>
						<plx:assign>
							<plx:left>
								<plx:callInstance name="Size" type="property">
									<plx:callObject>
										<plx:callThis name="MasterTabControl" type="property"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:left>
							<plx:right>
								<plx:callNew dataTypeName="Size" dataTypeQualifier="System.Drawing">
									<plx:passParam>
										<plx:value data="500" type="i4"/>
									</plx:passParam>
									<plx:passParam>
										<plx:value data="500" type="i4"/>
									</plx:passParam>
								</plx:callNew>
							</plx:right>
						</plx:assign>

						<plx:callInstance name="Add" type="methodCall">
							<plx:callObject>
								<plx:callInstance name="Controls" type="property">
									<plx:callObject>
										<plx:thisKeyword/>
									</plx:callObject>
								</plx:callInstance>
							</plx:callObject>
							<plx:passParam>
								<plx:nameRef name="MasterTabControl"/>
							</plx:passParam>
						</plx:callInstance>

						<plx:assign>
							<plx:left>
								<plx:callThis name="Size" type="property"/>
							</plx:left>
							<plx:right>
								<plx:callNew dataTypeName="Size" dataTypeQualifier="System.Drawing">
									<plx:passParam>
										<plx:value data="500" type="i4"/>
									</plx:passParam>
									<plx:passParam>
										<plx:value data="500" type="i4"/>
									</plx:passParam>
								</plx:callNew>
							</plx:right>
						</plx:assign>

					</plx:function>

					<xsl:copy-of select="$AllVars[self::plx:function]"/>
				</plx:class>
			</plx:namespace>
		</plx:root>

	</xsl:template>
	<!--	partial class Form2
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Text = "Form2";
		}

		#endregion
	} -->

	<xsl:template name="GetPreferredIdentifierProperties">
		<xsl:param name="Model"/>
		<xsl:param name="AllProperties"/>
		<xsl:param name="TargetConceptTypeName"/>
		<xsl:param name="ParentName"/>
		<xsl:variable name="targetConceptType" select="$Model//oil:conceptType[@name=$TargetConceptTypeName]"/>
		<xsl:variable name="targetConceptTypeProperties" select="$AllProperties[@conceptTypeName=$TargetConceptTypeName]/child::*"/>
		<xsl:variable name="singleRolePreferred" select="$targetConceptType/oil:*[oil:singleRoleUniquenessConstraint/@isPreferred='true']"/>
		<xsl:variable name="roleSequencePreferredUniquenessConstraint" select="$targetConceptType/oil:roleSequenceUniquenessConstraint[@isPreferred='true']"/>
		<xsl:variable name="roleSequencePreferredRolesFragment">
			<!-- This for-each is to preserve the role order from the uniqueness constraint. -->
			<xsl:for-each select="$roleSequencePreferredUniquenessConstraint/oil:roleSequence/oil:typeRef">
				<xsl:copy-of select="$targetConceptType/oil:*[@name=current()/@targetChild]"/>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="preferredRoles" select="$singleRolePreferred | exsl:node-set($roleSequencePreferredRolesFragment)/child::*"/>
		<xsl:choose>
			<xsl:when test="$preferredRoles">
				<xsl:for-each select="$preferredRoles">
					<xsl:choose>
						<xsl:when test="self::oil:informationType">
							<xsl:for-each select="$targetConceptTypeProperties[@name=current()/@name]">
								<xsl:copy>
									<xsl:attribute name="parentName">
										<xsl:value-of select="$ParentName"/>
									</xsl:attribute>
									<xsl:copy-of select="node()|@*"/>
								</xsl:copy>
							</xsl:for-each>
						</xsl:when>
						<xsl:when test="self::oil:conceptTypeRef">
							<xsl:call-template name="GetPreferredIdentifierProperties">
								<xsl:with-param name="Model" select="$Model"/>
								<xsl:with-param name="AllProperties" select="$AllProperties"/>
								<xsl:with-param name="TargetConceptTypeName" select="@targetConceptType"/>
								<xsl:with-param name="ParentName" select="$ParentName"/>
							</xsl:call-template>
						</xsl:when>
					</xsl:choose>
				</xsl:for-each>
			</xsl:when>
			<xsl:when test="$targetConceptType/parent::oil:conceptType">
				<xsl:call-template name="GetPreferredIdentifierProperties">
					<xsl:with-param name="Model" select="$Model"/>
					<xsl:with-param name="AllProperties" select="$AllProperties"/>
					<xsl:with-param name="TargetConceptTypeName" select="$targetConceptType/parent::oil:conceptType/@name"/>
					<xsl:with-param name="ParentName" select="$ParentName"/>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:message terminate="yes">An oil:conceptType must have a preferred identifier.</xsl:message>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="odt:identity" mode="GenerateInformationTypeFormatMapping">
		<prop:FormatMapping name="{@name}" canBeNull="false" isIdentity="true"/>
	</xsl:template>
	<xsl:template match="odt:boolean" mode="GenerateInformationTypeFormatMapping">
		<prop:FormatMapping name="{@name}" canBeNull="false">
			<prop:DataType dataTypeName=".boolean"/>
			<xsl:if test="string-length(@fixed)">
				<plx:binaryOperator type="identityEquality">
					<plx:left>
						<plx:valueKeyword/>
					</plx:left>
					<plx:right>
						<xsl:element name="plx:{@fixed}Keyword"/>
					</plx:right>
				</plx:binaryOperator>
			</xsl:if>
		</prop:FormatMapping>
	</xsl:template>

	<xsl:template match="oil:conceptType" mode="GenerateProperties">
		<xsl:param name="ConceptTypeRefs"/>
		<xsl:param name="InformationTypeFormatMappings"/>
		<xsl:variable name="thisClassName" select="@name"/>
		<xsl:variable name="identityFormatRefNames" select="$InformationTypeFormatMappings[@isIdentity='true']/@name"/>

		<!--Process directly contained oil:conceptTypeRef and oil:informationType elements,
			as well as nested oil:conceptType elements and oil:conceptType elements that we are nested within.
			Also process all oil:conceptTypeRef elements that are targetted at us.-->

		<xsl:for-each select="oil:informationType[not(@formatRef=$identityFormatRefNames)]">
			<xsl:variable name="informationTypeFormatMapping" select="$InformationTypeFormatMappings[@name=current()/@formatRef]"/>
			<prop:Property name="{@name}" mandatory="{@mandatory}" isUnique="{boolean(oil:singleRoleUniquenessConstraint)}" canBeNull="{not(@mandatory='alethic') or $informationTypeFormatMapping/@canBeNull='true'}" isCollection="false" isCustomType="false">
				<xsl:choose>
					<xsl:when test="not(@mandatory='alethic') and $informationTypeFormatMapping/@canBeNull='false'">
						<prop:DataType dataTypeName="Nullable">
							<plx:passTypeParam>
								<xsl:copy-of select="$informationTypeFormatMapping/prop:DataType/@*"/>
								<xsl:copy-of select="$informationTypeFormatMapping/prop:DataType/child::*"/>
							</plx:passTypeParam>
						</prop:DataType>
					</xsl:when>
					<xsl:otherwise>
						<xsl:copy-of select="$informationTypeFormatMapping/prop:DataType"/>
					</xsl:otherwise>
				</xsl:choose>
			</prop:Property>
		</xsl:for-each>
		<xsl:for-each select="oil:conceptTypeRef">
			<prop:Property name="{@name}" mandatory="{@mandatory}" isUnique="{boolean(oil:singleRoleUniquenessConstraint)}" isCollection="false" isCustomType="true" canBeNull="true" oppositeName="{@oppositeName}">
				<prop:DataType dataTypeName="{@target}"/>
			</prop:Property>
		</xsl:for-each>
		<xsl:for-each select="oil:conceptType">
			<prop:Property name="{@name}" mandatory="{@mandatory}" isUnique="true" isCollection="false" isCustomType="true" canBeNull="true" oppositeName="{$thisClassName}">
				<prop:DataType dataTypeName="{@name}"/>
			</prop:Property>
		</xsl:for-each>
		<xsl:for-each select="parent::oil:conceptType">
			<prop:Property name="{@name}" mandatory="alethic" isUnique="true" isCollection="false" isCustomType="true" canBeNull="true" oppositeName="{$thisClassName}">
				<prop:DataType dataTypeName="{@name}"/>
			</prop:Property>
		</xsl:for-each>
		<xsl:for-each select="$ConceptTypeRefs[@target=current()/@name]">
			<xsl:variable name="isCollection" select="not(boolean(oil:singleRoleUniquenessConstraint))"/>
			<prop:Property name="{@oppositeName}" mandatory="false" isUnique="true" isCollection="{$isCollection}" isCustomType="true" canBeNull="true" oppositeName="{@name}">
				<xsl:variable name="parentConceptTypeName" select="parent::oil:conceptType/@name"/>
				<xsl:choose>
					<xsl:when test="$isCollection">
						<prop:DataType dataTypeName="IEnumerable">
							<plx:passTypeParam dataTypeName="{$parentConceptTypeName}"/>
						</prop:DataType>
					</xsl:when>
					<xsl:otherwise>
						<prop:DataType dataTypeName="{$parentConceptTypeName}"/>
					</xsl:otherwise>
				</xsl:choose>
			</prop:Property>
		</xsl:for-each>
	</xsl:template>


</xsl:stylesheet>
