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
	xmlns:ormdt="http://schemas.orm.net/ORMDataTypes"
	xmlns:plx="http://schemas.neumont.edu/CodeGeneration/PLiX"
	xmlns:prop="urn:schemas-orm-net:PLiX:CLI:Properties"
	exclude-result-prefixes="oil ormdt prop"
	extension-element-prefixes="exsl">

	<!-- Input file:  [ORM Model Name].Implementation.PLiX.xml -->
	<!-- Output file:  InputControl.PLiX.xml -->

	<xsl:import href="../OIALtoPLiX_GlobalSupportFunctions.xslt"/>
	
	<xsl:param name="OIAL"/>

	<xsl:output method="xml" encoding="utf-8" media-type="text/xml" indent="yes"/>

	<xsl:variable name="SprocSuffix" select="'SP'"/>
	<xsl:variable name="CollectionSuffix" select="'Collections'"/>
	<xsl:variable name="debugMode" select="'false'"/>

	<xsl:template match="/" mode="AddNamespaceImports">
		<plx:namespaceImport name="System"/>
		<plx:namespaceImport name="System.Collections.Generic"/>
		<plx:namespaceImport name="System.ComponentModel"/>
		<plx:namespaceImport name="System.Drawing"/>
		<plx:namespaceImport name="System.Data"/>
		<plx:namespaceImport name="System.Text"/>
		<plx:namespaceImport name="System.Windows.Forms"/>
		<plx:namespaceImport name="System.Data.SqlClient"/>
		<plx:namespaceImport name="System.Configuration"/>
	</xsl:template>

	<xsl:template match="/">
		<xsl:variable name="Model" select="./child::*"/>
		<xsl:variable name="ModelName" select="$Model/plx:namespace/@name"/>
		<xsl:variable name="ClassName" select="'InputControl'"/>
		<xsl:variable name="OIL" select="$OIAL"/>

		<xsl:variable name="OilInfoFragment">
			<xsl:element name="AllTypes">
				<!-- Value Types -->
				<xsl:for-each select="$OIL/oil:informationTypeFormats/ormdt:identity">
					<!-- Identity(Object) -->
					<xsl:call-template name="CreateValueTypeElement">
						<xsl:with-param name="Element" select="current()"/>
						<xsl:with-param name="Type" select="'.object'"/>
						<xsl:with-param name="AutoIncrement" select="'true'"/>
					</xsl:call-template>
				</xsl:for-each>
				<xsl:for-each select="$OIL/oil:informationTypeFormats/ormdt:string">
					<!-- string -->
					<xsl:call-template name="CreateValueTypeElement">
						<xsl:with-param name="Element" select="current()"/>
						<xsl:with-param name="Type" select="'.string'"/>
					</xsl:call-template>
				</xsl:for-each>
				<xsl:for-each select="$OIL/oil:informationTypeFormats/ormdt:boolean">
					<!-- boolean -->
					<xsl:call-template name="CreateValueTypeElement">
						<xsl:with-param name="Element" select="current()"/>
						<xsl:with-param name="Type" select="'.boolean'"/>
					</xsl:call-template>
				</xsl:for-each>
				<xsl:for-each select="$OIL/oil:informationTypeFormats/ormdt:decimalNumber">
					<xsl:choose>
						<xsl:when test="@fractionDigits = 0 and not(substring-after(@name, 'Date_') = '')">
							<!-- Date -->
							<xsl:call-template name="CreateValueTypeElement">
								<xsl:with-param name="Element" select="current()"/>
								<xsl:with-param name="Type" select="'.date'"/>
							</xsl:call-template>
						</xsl:when>
						<xsl:when test="@fractionDigits = 0">
							<!-- int -->
							<xsl:call-template name="CreateValueTypeElement">
								<xsl:with-param name="Element" select="current()"/>
								<xsl:with-param name="Type" select="'.i4'"/>
							</xsl:call-template>
						</xsl:when>
						<xsl:when test="@fractionDigits = 1 or @fractionDigits = 2">
							<!-- decimal -->
							<xsl:call-template name="CreateValueTypeElement">
								<xsl:with-param name="Element" select="current()"/>
								<xsl:with-param name="Type" select="'.decimal'"/>
							</xsl:call-template>
						</xsl:when>
						<xsl:otherwise>
							<!-- double -->
							<xsl:call-template name="CreateValueTypeElement">
								<xsl:with-param name="Element" select="current()"/>
								<xsl:with-param name="Type" select="'.r8'"/>
							</xsl:call-template>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:for-each>
				<xsl:for-each select="$OIL/oil:informationTypeFormats/ormdt:floatingPointNumber">
					<xsl:choose>
						<xsl:when test="@precision = 'single'">
							<!-- float -->
							<xsl:call-template name="CreateValueTypeElement">
								<xsl:with-param name="Element" select="current()"/>
								<xsl:with-param name="Type" select="'.r4'"/>
							</xsl:call-template>
						</xsl:when>
						<xsl:otherwise>
							<!-- double -->
							<xsl:call-template name="CreateValueTypeElement">
								<xsl:with-param name="Element" select="current()"/>
								<xsl:with-param name="Type" select="'.r8'"/>
							</xsl:call-template>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:for-each>
				<!-- Entity Types -->
				<xsl:for-each select="$OIL//oil:conceptType">
					<xsl:element name="Type">
						<xsl:copy-of select="@*"/>
						<xsl:attribute name="typeUseName">
							<xsl:value-of select="concat(@name, 'Core')"/>
						</xsl:attribute>
						<xsl:for-each select="oil:conceptTypeRef">
							<xsl:element name="Field">
								<xsl:copy-of select="@*"/>
							</xsl:element>
						</xsl:for-each>
						<xsl:for-each select="oil:informationType">
							<xsl:element name="Field">
								<xsl:copy-of select="@*"/>
								<xsl:attribute name="type_kind">
									<xsl:text>value</xsl:text>
								</xsl:attribute>
								<xsl:for-each select="//oil:informationTypeFormats/ormdt:identity[@name = current()/@formatRef]">
									<!-- old ../../oil:informationTypeFormats/ormdt:identity[@name = current()/@formatRef] -->
									<xsl:attribute name="autoIncrement">
										<xsl:text>true</xsl:text>
									</xsl:attribute>
								</xsl:for-each>
							</xsl:element>
						</xsl:for-each>
						<xsl:element name="PreferredIdentifier">
							<xsl:for-each select="oil:roleSequenceUniquenessConstraint[@isPreferred='true']/oil:roleSequence/oil:typeRef">
								<xsl:element name="Field">
									<xsl:copy-of select="@*"/>
									<xsl:attribute name="id_kind">
										<xsl:choose>
											<xsl:when test="../../../oil:conceptTypeRef[@name = current()/@targetChild]/@target">
												<xsl:value-of select="../../../oil:conceptTypeRef[@name = current()/@targetChild]/@target"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:text>value</xsl:text>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:attribute>
									<xsl:attribute name="name">
										<xsl:value-of select="@targetChild"/>
									</xsl:attribute>
									<xsl:for-each select="//oil:informationTypeFormats/ormdt:identity[@name = current()/@formatRef]">
										<!-- old ../../../../oil:informationTypeFormats/ormdt:identity[@name = current()/@formatRef] -->
										<xsl:attribute name="autoIncrement">
											<xsl:text>true</xsl:text>
										</xsl:attribute>
									</xsl:for-each>
								</xsl:element>
							</xsl:for-each>
							<xsl:for-each select="oil:roleSequenceUniquenessConstraint[@isPreferred='true']/oil:roleSequence/oil:informationType">
								<xsl:element name="Field">
									<xsl:copy-of select="@*"/>
									<xsl:attribute name="id_kind">
										<xsl:text>value</xsl:text>
									</xsl:attribute>
									<xsl:for-each select="//oil:informationTypeFormats/ormdt:identity[@name = current()/@formatRef]">
										<!-- old ../../../../oil:informationTypeFormats/ormdt:identity[@name = current()/@formatRef] -->
										<xsl:attribute name="autoIncrement">
											<xsl:text>true</xsl:text>
										</xsl:attribute>
									</xsl:for-each>
								</xsl:element>
							</xsl:for-each>
							<xsl:for-each select="oil:conceptTypeRef[oil:singleRoleUniquenessConstraint/@isPreferred='true']">
								<xsl:element name="Field">
									<xsl:copy-of select="@*"/>
									<xsl:attribute name="id_kind">
										<xsl:value-of select="@target"/>
									</xsl:attribute>
									<xsl:for-each select="//oil:informationTypeFormats/ormdt:identity[@name = current()/@formatRef]">
										<!-- old ../../oil:informationTypeFormats/ormdt:identity[@name = current()/@formatRef] -->
										<xsl:attribute name="autoIncrement">
											<xsl:text>true</xsl:text>
										</xsl:attribute>
									</xsl:for-each>
								</xsl:element>
							</xsl:for-each>
							<xsl:for-each select="oil:informationType[oil:singleRoleUniquenessConstraint/@isPreferred='true']">
								<xsl:element name="Field">
									<xsl:copy-of select="@*"/>
									<xsl:attribute name="id_kind">
										<xsl:text>value</xsl:text>
									</xsl:attribute>
									<xsl:for-each select="//oil:informationTypeFormats/ormdt:identity[@name = current()/@formatRef]">
										<!-- old ../../oil:informationTypeFormats/ormdt:identity[@name = current()/@formatRef] -->
										<xsl:attribute name="autoIncrement">
											<xsl:text>true</xsl:text>
										</xsl:attribute>
									</xsl:for-each>
								</xsl:element>
							</xsl:for-each>
						</xsl:element>
					</xsl:element>
				</xsl:for-each>
			</xsl:element>
		</xsl:variable>
		<xsl:variable name="OilInfo" select="exsl:node-set($OilInfoFragment)/child::*"/>

		<xsl:variable name="AllClassesFragment">
			<xsl:for-each select="plx:root/plx:namespace//plx:class">
				<xsl:choose>
					<xsl:when test="not(substring-before(@name, 'Core') = '')">
						<xsl:element name="Class">
							<xsl:copy-of select="@name"/>
							<xsl:attribute name="displayName">
								<xsl:value-of select="substring(@name, 0, string-length(@name) - 3 )"/>
							</xsl:attribute>
							<xsl:element name="PreferredIdentifier">
								<xsl:for-each select="$OilInfo/Type[@typeUseName = current()/@name]/PreferredIdentifier/Field">
									<xsl:element name="ID_Field">
										<xsl:copy-of select="@*"/>
										<xsl:attribute name="displayName">
											<xsl:call-template name="CreatePreferredIdentifierDisplayName">
												<xsl:with-param name="ID_Kind" select="@id_kind"/>
												<xsl:with-param name="Name" select="@name"/>
												<xsl:with-param name="OilInfo" select="$OilInfo"/>
											</xsl:call-template>
										</xsl:attribute>
									</xsl:element>
								</xsl:for-each>
							</xsl:element>
							<xsl:for-each select="plx:function">
								<xsl:element name="Function">
									<xsl:copy-of select="@*"/>
									<xsl:call-template name="ParameterElement">
										<xsl:with-param name="ParameterSet" select="plx:param"/>
										<xsl:with-param name="OilInfo" select="$OilInfo"/>
									</xsl:call-template>
								</xsl:element>
							</xsl:for-each>
							<xsl:for-each select="plx:property">
								<xsl:element name="Property">
									<xsl:copy-of select="@*"/>
									<xsl:attribute name="dataTypeName">
										<xsl:choose>
											<xsl:when test="plx:returns/@dataTypeName = 'Nullable'">
												<xsl:value-of select="plx:returns/plx:passTypeParam/@dataTypeName"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="plx:returns/@dataTypeName"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:attribute>
									<xsl:copy-of select="$OilInfo/Type[@typeUseName = current()/../@name]/Field[@name = current()/@name]/@autoIncrement"/>
									<xsl:for-each select="$OilInfo/Type[@typeUseName = current()/../@name]/PreferredIdentifier/Field[@name = current()/@name]">
										<xsl:attribute name="displayName">
											<xsl:call-template name="CreatePreferredIdentifierDisplayName">
												<xsl:with-param name="ID_Kind" select="@id_kind"/>
												<xsl:with-param name="Name" select="@name"/>
												<xsl:with-param name="OilInfo" select="$OilInfo"/>
											</xsl:call-template>
										</xsl:attribute>
									</xsl:for-each>
								</xsl:element>
							</xsl:for-each>
						</xsl:element>
					</xsl:when>
					<xsl:otherwise>
						<xsl:element name="TestClass">
							<xsl:copy-of select="@name"/>
							<xsl:element name="Actions">
								<xsl:for-each select="plx:function[starts-with(@name, 'Get') or starts-with(@name, 'Create')][@visibility = 'public']">
									<xsl:choose>
										<xsl:when test="starts-with(@name, 'Get')">
											<xsl:element name="Select">
												<xsl:element name="Function">
													<xsl:copy-of select="@*"/>
													<xsl:attribute name="objectToSelect">
														<xsl:value-of select="substring-after(substring-before(@name, 'By'), 'Get')"/>
													</xsl:attribute>
													<xsl:attribute name="selectionMode">
														<xsl:value-of select="substring-after(@name, 'By')"/>
													</xsl:attribute>
													<xsl:call-template name="ParameterElement">
														<xsl:with-param name="ParameterSet" select="plx:param"/>
														<xsl:with-param name="OilInfo" select="$OilInfo"/>
													</xsl:call-template>
												</xsl:element>
											</xsl:element>
										</xsl:when>
										<xsl:when test="starts-with(@name, 'Create')">
											<xsl:element name="Create">
												<xsl:element name="Function">
													<xsl:copy-of select="@*"/>
													<xsl:attribute name="objectToCreate">
														<xsl:value-of select="substring-after(@name, 'Create')"/>
													</xsl:attribute>
													<xsl:call-template name="ParameterElement">
														<xsl:with-param name="ParameterSet" select="plx:param"/>
														<xsl:with-param name="OilInfo" select="$OilInfo"/>
													</xsl:call-template>
												</xsl:element>
											</xsl:element>
										</xsl:when>
										<xsl:otherwise>
											<xsl:element name="Function">
												<xsl:copy-of select="@*"/>
												<xsl:call-template name="ParameterElement">
													<xsl:with-param name="ParameterSet" select="plx:param"/>
													<xsl:with-param name="OilInfo" select="$OilInfo"/>
												</xsl:call-template>
											</xsl:element>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:for-each>
							</xsl:element>
							<xsl:for-each select="plx:property">
								<xsl:element name="Property">
									<xsl:copy-of select="@*"/>
									<xsl:attribute name="dataTypeName">
										<xsl:choose>
											<xsl:when test="plx:returns/@dataTypeName = 'Nullable'">
												<xsl:value-of select="plx:returns/plx:passTypeParam/@dataTypeName"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="plx:returns/@dataTypeName"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:attribute>
									<xsl:copy-of select="$OilInfo/Type[@typeUseName = current()/../@name]/Field[@name = current()/@name]/@autoIncrement"/>
									<xsl:for-each select="$OilInfo/Type[@typeUseName = current()/../@name]/PreferredIdentifier/Field[@name = current()/@name]">
										<xsl:attribute name="displayName">
											<xsl:call-template name="CreatePreferredIdentifierDisplayName">
												<xsl:with-param name="ID_Kind" select="@id_kind"/>
												<xsl:with-param name="Name" select="@name"/>
												<xsl:with-param name="OilInfo" select="$OilInfo"/>
											</xsl:call-template>
										</xsl:attribute>
									</xsl:for-each>
								</xsl:element>
							</xsl:for-each>
						</xsl:element>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="AllClasses" select="exsl:node-set($AllClassesFragment)/child::*"/>

		<plx:root>
			<xsl:apply-templates select="." mode="AddNamespaceImports"/>
			
			<xsl:variable name="bodyFragment">
				<plx:namespace name="{$ModelName}">
					<xsl:for-each select="$AllClasses[not(substring-before(@name, 'Core') = '')]">
						<xsl:sort select="@name"/>
						<!--
					//
					// InputControl
					//
					-->
						<plx:class name="{@name}_{$ClassName}" partial="true" visibility="public">
							<plx:leadingInfo>
								<plx:pragma type="region" data="{@name}_{$ClassName}"/>
							</plx:leadingInfo>
							<plx:trailingInfo>
								<plx:pragma type="closeRegion" data="{@name}_{$ClassName}"/>
							</plx:trailingInfo>
							<plx:derivesFromClass dataTypeName="UserControl"/>

							<plx:function name=".construct" visibility="public" >
								<plx:callThis name="InitializeComponent" type="methodCall"/>
							</plx:function>

							<xsl:call-template name="CommonMethods">
								<xsl:with-param name="AbstractTypeName" select="@name"/>
							</xsl:call-template>

						</plx:class>

						<!--
					//
					// Create
					//
					-->
						<plx:class name="Create_{@name}_{$ClassName}" partial="true" visibility="public">
							<plx:leadingInfo>
								<plx:pragma type="region" data="Create_{@name}_{$ClassName}"/>
							</plx:leadingInfo>
							<plx:trailingInfo>
								<plx:pragma type="closeRegion" data="Create_{@name}_{$ClassName}"/>
							</plx:trailingInfo>
							<plx:derivesFromClass dataTypeName="UserControl"/>

							<plx:function name=".construct" visibility="public" >
								<plx:callThis name="InitializeComponent" type="methodCall"/>
							</plx:function>

							<xsl:call-template name="CommonMethods">
								<xsl:with-param name="ActionType" select="'Create'"/>
								<xsl:with-param name="AbstractTypeName" select="@name"/>
							</xsl:call-template>

							<plx:function name="btnCreate_Click" visibility="private">
								<plx:param name="sender" dataTypeName="object"/>
								<plx:param name="e" dataTypeName="EventArgs" dataTypeQualifier="System"/>

								<plx:try>
									<plx:assign>
										<plx:left>
											<plx:callThis name="abstractTypeVar" type="field" accessor="this"/>
										</plx:left>
										<plx:right>
											<plx:callInstance name="Create{@displayName}" type="methodCall">
												<plx:callObject>
													<plx:callThis name="testVar" type="field" accessor="this"/>
												</plx:callObject>
												<xsl:call-template name="SelectParameters">
													<xsl:with-param name="ParameterSet" select="../TestClass/Actions/Create/Function[current()/@displayName = @objectToCreate]/Parameter"/>
													<xsl:with-param name="AllClasses" select="$AllClasses"/>
													<xsl:with-param name="DGV" select="'dgvCreate'"/>
												</xsl:call-template>
											</plx:callInstance>
										</plx:right>
									</plx:assign>

									<plx:branch>
										<plx:condition>
											<plx:binaryOperator type="identityInequality">
												<plx:left>
													<plx:callThis name="abstractTypeVar" type="field" accessor="this"/>
												</plx:left>
												<plx:right>
													<plx:nullKeyword/>
												</plx:right>
											</plx:binaryOperator>
										</plx:condition>

										<xsl:for-each select="Property[not(@autoIncrement = 'true')][not(@name = 'Context')][substring-before(@name, 'Collection') = '']">
											<plx:branch>
												<plx:condition>
													<plx:binaryOperator type="booleanAnd">
														<plx:left>
															<plx:binaryOperator type="identityInequality">
																<plx:left>
																	<plx:callInstance name="Value" type="property">
																		<plx:callObject>
																			<plx:callInstance name="Cells" type="property">
																				<plx:callObject>
																					<plx:callInstance name="Rows" type="property">
																						<plx:callObject>
																							<plx:callInstance name="dgvCreate" type="field">
																								<plx:callObject>
																									<plx:thisKeyword/>
																								</plx:callObject>
																							</plx:callInstance>
																						</plx:callObject>
																						<plx:passParam>
																							<plx:value data="0" type="i4"/>
																						</plx:passParam>
																					</plx:callInstance>
																				</plx:callObject>
																				<xsl:choose>
																					<xsl:when test="@displayName">
																						<plx:passParam>
																							<plx:string data="{@displayName}"/>
																						</plx:passParam>
																					</xsl:when>
																					<xsl:otherwise>
																						<plx:passParam>
																							<plx:string data="{@name}"/>
																						</plx:passParam>
																					</xsl:otherwise>
																				</xsl:choose>
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
																	<xsl:call-template name="CastDGVValueAndGetID">
																		<xsl:with-param name="Property" select="current()"/>
																		<xsl:with-param name="DGV" select="'dgvCreate'"/>
																		<xsl:with-param name="FunctionList" select="../../TestClass/Actions/Select/Function"/>
																		<xsl:with-param name="AllClasses" select="$AllClasses"/>
																		<xsl:with-param name="OilInfo" select="$OilInfo"/>
																	</xsl:call-template>
																</plx:left>
																<plx:right>
																	<xsl:variable name="instanceCallerFragment">
																		<plx:callThis name="abstractTypeVar" type="field" accessor="this"/>
																	</xsl:variable>
																	<xsl:call-template name="PropertyAccessor">
																		<xsl:with-param name="ID_Kind" select="@dataTypeName"/>
																		<xsl:with-param name="Name" select="@name"/>
																		<xsl:with-param name="InstanceCaller" select="exsl:node-set($instanceCallerFragment)/child::*"/>
																		<xsl:with-param name="OilInfo" select="$OilInfo"/>
																	</xsl:call-template>
																</plx:right>
															</plx:binaryOperator>
														</plx:right>
													</plx:binaryOperator>
												</plx:condition>

												<xsl:call-template name="UpdatePropertyValue">
													<xsl:with-param name="Property" select="current()"/>
													<xsl:with-param name="DGV" select="'dgvCreate'"/>
													<xsl:with-param name="FunctionList" select="../../TestClass/Actions/Select/Function"/>
													<xsl:with-param name="AllClasses" select="$AllClasses"/>
												</xsl:call-template>
											</plx:branch>
										</xsl:for-each>

										<plx:callInstance name="Clear" type="methodCall">
											<plx:callObject>
												<plx:callInstance name="Rows" type="property">
													<plx:callObject>
														<plx:callThis name="dgvCreate" type="field" accessor="this"/>
													</plx:callObject>
												</plx:callInstance>
											</plx:callObject>
										</plx:callInstance>
									</plx:branch>

									<plx:catch localName="exception" dataTypeName="Exception">
										<plx:callStatic name="Show" dataTypeName="MessageBox" dataTypeQualifier="System.Windows.Forms" type="methodCall">
											<plx:passParam>
												<plx:callInstance name="Message" type="property">
													<plx:callObject>
														<plx:nameRef name="exception" type="local"/>
													</plx:callObject>
												</plx:callInstance>
											</plx:passParam>
											<plx:passParam>
												<plx:string data="Create {@displayName}"/>
											</plx:passParam>
										</plx:callStatic>
									</plx:catch>
								</plx:try>

							</plx:function>
						</plx:class>

						<!--
					//
					// Collection
					//
					-->
						<plx:class name="Collection_{@name}_{$ClassName}" partial="true" visibility="public">
							<plx:leadingInfo>
								<plx:pragma type="region" data="Collection_{@name}_{$ClassName}"/>
							</plx:leadingInfo>
							<plx:trailingInfo>
								<plx:pragma type="closeRegion" data="Collection_{@name}_{$ClassName}"/>
							</plx:trailingInfo>
							<plx:derivesFromClass dataTypeName="UserControl"/>

							<plx:function name=".construct" visibility="public" >
								<plx:callThis name="InitializeComponent" type="methodCall"/>
							</plx:function>

							<xsl:call-template name="CommonMethods">
								<xsl:with-param name="ActionType" select="'Collection'"/>
								<xsl:with-param name="AbstractTypeName" select="@name"/>
							</xsl:call-template>

							<plx:function name="btnCollection_Click" visibility="private">
								<plx:param name="sender" dataTypeName="object"/>
								<plx:param name="e" dataTypeName="EventArgs" dataTypeQualifier="System"/>

								<plx:try>
									<plx:local name="index" dataTypeName=".i4">
										<plx:initialize>
											<plx:value data="0" type="i4"/>
										</plx:initialize>
									</plx:local>

									<plx:callInstance name="Clear" type="methodCall">
										<plx:callObject>
											<plx:callInstance name="Rows" type="property">
												<plx:callObject>
													<plx:callThis name="dgvCollection" type="field" accessor="this"/>
												</plx:callObject>
											</plx:callInstance>
										</plx:callObject>
									</plx:callInstance>

									<plx:autoDispose localName="iterator" dataTypeName="IEnumerator">
										<plx:passTypeParam dataTypeName="{@displayName}"/>
										<plx:initialize>
											<plx:callInstance name="GetEnumerator" type="methodCall">
												<plx:callObject>
													<plx:callInstance name="{substring-before(@name, 'Core')}Collection" type="property">
														<plx:callObject>
															<plx:callThis name="testVar" type="field" accessor="this"/>
														</plx:callObject>
													</plx:callInstance>
												</plx:callObject>
											</plx:callInstance>
										</plx:initialize>

										<plx:loop checkCondition="before">
											<plx:condition>
												<plx:callInstance name="MoveNext" type="methodCall">
													<plx:callObject>
														<plx:nameRef name="iterator" type="local"/>
													</plx:callObject>
												</plx:callInstance>
											</plx:condition>

											<plx:callInstance name="Add" type="methodCall">
												<plx:callObject>
													<plx:callInstance name="Rows" type="property">
														<plx:callObject>
															<plx:callThis name="dgvCollection" type="field" accessor="this"/>
														</plx:callObject>
													</plx:callInstance>
												</plx:callObject>
											</plx:callInstance>

											<xsl:for-each select="Property[not(@name = 'Context')][substring-before(@name, 'Collection') = '']">
												<plx:assign>
													<plx:left>
														<plx:callInstance name="Value" type="property">
															<plx:callObject>
																<plx:callInstance name="Cells" type="property">
																	<plx:callObject>
																		<plx:callInstance name="Rows" type="property">
																			<plx:callObject>
																				<plx:callInstance name="dgvCollection" type="field">
																					<plx:callObject>
																						<plx:thisKeyword/>
																					</plx:callObject>
																				</plx:callInstance>
																			</plx:callObject>
																			<plx:passParam>
																				<plx:nameRef name="index" type="local"/>
																			</plx:passParam>
																		</plx:callInstance>
																	</plx:callObject>
																	<xsl:choose>
																		<xsl:when test="@displayName">
																			<plx:passParam>
																				<plx:string data="{@displayName}"/>
																			</plx:passParam>
																		</xsl:when>
																		<xsl:otherwise>
																			<plx:passParam>
																				<plx:string data="{@name}"/>
																			</plx:passParam>
																		</xsl:otherwise>
																	</xsl:choose>
																</plx:callInstance>
															</plx:callObject>
														</plx:callInstance>
													</plx:left>
													<plx:right>
														<xsl:variable name="instanceCallerFragment">
															<plx:callInstance name="Current" type="property">
																<plx:callObject>
																	<plx:nameRef name="iterator"/>
																</plx:callObject>
															</plx:callInstance>
														</xsl:variable>
														<xsl:call-template name="PropertyAccessor">
															<xsl:with-param name="ID_Kind" select="@dataTypeName"/>
															<xsl:with-param name="Name" select="@name"/>
															<xsl:with-param name="InstanceCaller" select="exsl:node-set($instanceCallerFragment)/child::*"/>
															<xsl:with-param name="OilInfo" select="$OilInfo"/>
														</xsl:call-template>
													</plx:right>
												</plx:assign>
											</xsl:for-each>

											<plx:increment>
												<plx:nameRef name="index" type="local"/>
											</plx:increment>
										</plx:loop>
									</plx:autoDispose>

									<plx:catch localName="exception" dataTypeName="Exception">
										<plx:callStatic name="Show" dataTypeName="MessageBox" dataTypeQualifier="System.Windows.Forms" type="methodCall">
											<plx:passParam>
												<plx:callInstance name="Message" type="property">
													<plx:callObject>
														<plx:nameRef name="exception" type="local"/>
													</plx:callObject>
												</plx:callInstance>
											</plx:passParam>
											<plx:passParam>
												<plx:string data="Get Collection of {@displayName}"/>
											</plx:passParam>
										</plx:callStatic>
									</plx:catch>
								</plx:try>

							</plx:function>

						</plx:class>

						<!--
					//
					// Select
					//
					-->
						<plx:class name="Select_{@name}_{$ClassName}" partial="true" visibility="public">
							<plx:leadingInfo>
								<plx:pragma type="region" data="Select_{@name}_{$ClassName}"/>
							</plx:leadingInfo>
							<plx:trailingInfo>
								<plx:pragma type="closeRegion" data="Select_{@name}_{$ClassName}"/>
							</plx:trailingInfo>
							<plx:derivesFromClass dataTypeName="UserControl"/>

							<plx:function name=".construct" visibility="public" >
								<plx:callThis name="InitializeComponent" type="methodCall"/>
							</plx:function>

							<xsl:call-template name="CommonMethods">
								<xsl:with-param name="ActionType" select="'Select'"/>
								<xsl:with-param name="AbstractTypeName" select="@name"/>
							</xsl:call-template>

							<plx:function name="btnSelect_Click" visibility="private">
								<plx:param name="sender" dataTypeName="object"/>
								<plx:param name="e" dataTypeName="EventArgs" dataTypeQualifier="System"/>

								<plx:try>
									<plx:switch>
										<plx:condition>
											<plx:callInstance name="ToString" type="methodCall">
												<plx:callObject>
													<plx:callInstance name="SelectedItem" type="property">
														<plx:callObject>
															<plx:callThis name="cbxSelectionMode" type="field" accessor="this"/>
														</plx:callObject>
													</plx:callInstance>
												</plx:callObject>
											</plx:callInstance>
										</plx:condition>

										<xsl:for-each select="../TestClass/Actions/Select/Function[current()/@displayName = @objectToSelect]">
											<plx:case>
												<plx:condition>
													<plx:string data="{@selectionMode}"/>
												</plx:condition>

												<plx:assign>
													<plx:left>
														<plx:callThis name="abstractTypeVar" type="field" accessor="this"/>
													</plx:left>
													<plx:right>
														<plx:callInstance name="{@name}" type="methodCall">
															<plx:callObject>
																<plx:callThis name="testVar" type="field" accessor="this"/>
															</plx:callObject>
															<xsl:choose>
																<!-- The selection mode is one of the properties -->
																<xsl:when test="../../../../Class[@name = current()/@objectToSelect]/Property[@name = @selectionMode]">
																	<xsl:choose>
																		<!-- The property has a displayName -->
																		<xsl:when test="../../../../Class[@name = current()/@objectToSelect]/Property[@name = @selectionMode]/@displayName">
																			<xsl:call-template name="SelectParameters">
																				<xsl:with-param name="ParameterSet" select="Parameter"/>
																				<xsl:with-param name="AllClasses" select="$AllClasses"/>
																				<xsl:with-param name="DGV" select="'dgvSelect'"/>
																				<xsl:with-param name="DisplayName" select="../../../../Class[@name = current()/@objectToSelect]/Property[@name = @selectionMode]/@displayName"/>
																			</xsl:call-template>
																		</xsl:when>
																		<!-- The property does not have a displayName -->
																		<xsl:otherwise>
																			<xsl:call-template name="SelectParameters">
																				<xsl:with-param name="ParameterSet" select="Parameter"/>
																				<xsl:with-param name="AllClasses" select="$AllClasses"/>
																				<xsl:with-param name="DGV" select="'dgvSelect'"/>
																				<xsl:with-param name="DisplayName" select="../../../../Class[@name = current()/@objectToSelect]/Property[@name = @selectionMode]/@name"/>
																			</xsl:call-template>
																		</xsl:otherwise>
																	</xsl:choose>
																</xsl:when>
																<!-- The selection mode is not one of the properties -->
																<xsl:otherwise>
																	<xsl:call-template name="SelectParameters">
																		<xsl:with-param name="ParameterSet" select="Parameter"/>
																		<xsl:with-param name="AllClasses" select="$AllClasses"/>
																		<xsl:with-param name="DGV" select="'dgvSelect'"/>
																	</xsl:call-template>
																</xsl:otherwise>
															</xsl:choose>
														</plx:callInstance>
													</plx:right>
												</plx:assign>

											</plx:case>
										</xsl:for-each>
										<plx:fallbackCase>
										</plx:fallbackCase>
									</plx:switch>

									<plx:assign>
										<plx:left>
											<plx:callInstance name="Enabled" type="property">
												<plx:callObject>
													<plx:callThis name="btnSave" type="field" accessor="this"/>
												</plx:callObject>
											</plx:callInstance>
										</plx:left>
										<plx:right>
											<plx:falseKeyword/>
										</plx:right>
									</plx:assign>

									<plx:assign>
										<plx:left>
											<plx:callInstance name="Enabled" type="property">
												<plx:callObject>
													<plx:callThis name="btnCancel" type="field" accessor="this"/>
												</plx:callObject>
											</plx:callInstance>
										</plx:left>
										<plx:right>
											<plx:falseKeyword/>
										</plx:right>
									</plx:assign>

									<plx:callThis name="DisplaySelection" type="methodCall" accessor="this"/>

									<plx:catch localName="exception" dataTypeName="Exception">
										<plx:callStatic name="Show" dataTypeName="MessageBox" dataTypeQualifier="System.Windows.Forms" type="methodCall">
											<plx:passParam>
												<plx:callInstance name="Message" type="property">
													<plx:callObject>
														<plx:nameRef name="exception" type="local"/>
													</plx:callObject>
												</plx:callInstance>
											</plx:passParam>
											<plx:passParam>
												<plx:string data="Select {@displayName}"/>
											</plx:passParam>
										</plx:callStatic>
									</plx:catch>
								</plx:try>

							</plx:function>

							<plx:function name='DisplaySelection' visibility='private'>

								<plx:try>
									<plx:branch>
										<plx:condition>
											<plx:binaryOperator type="identityEquality">
												<plx:left>
													<plx:callThis name="abstractTypeVar" type="field" accessor="this"/>
												</plx:left>
												<plx:right>
													<plx:nullKeyword/>
												</plx:right>
											</plx:binaryOperator>
										</plx:condition>

										<plx:assign>
											<plx:left>
												<plx:callThis name="editMode" type="field"/>
											</plx:left>
											<plx:right>
												<plx:falseKeyword/>
											</plx:right>
										</plx:assign>

										<plx:assign>
											<plx:left>
												<plx:callInstance name="Visible" type="property">
													<plx:callObject>
														<plx:callThis name="pnlSave" type="field" accessor="this"/>
													</plx:callObject>
												</plx:callInstance>
											</plx:left>
											<plx:right>
												<plx:falseKeyword/>
											</plx:right>
										</plx:assign>

										<plx:assign>
											<plx:left>
												<plx:callInstance name="Visible" type="property">
													<plx:callObject>
														<plx:callThis name="dgvCurrentObject" type="field" accessor="this"/>
													</plx:callObject>
												</plx:callInstance>
											</plx:left>
											<plx:right>
												<plx:falseKeyword/>
											</plx:right>
										</plx:assign>

										<plx:assign>
											<plx:left>
												<plx:callInstance name="Text" type="property">
													<plx:callObject>
														<plx:callThis name="lblCurrentObject" type="field" accessor="this"/>
													</plx:callObject>
												</plx:callInstance>
											</plx:left>
											<plx:right>
												<plx:string data="There is no selected {@displayName}."/>
											</plx:right>
										</plx:assign>

									</plx:branch>
									<plx:fallbackBranch>
										<plx:assign>
											<plx:left>
												<plx:callThis name="editMode" type="field"/>
											</plx:left>
											<plx:right>
												<plx:trueKeyword/>
											</plx:right>
										</plx:assign>

										<plx:assign>
											<plx:left>
												<plx:callInstance name="Visible" type="property">
													<plx:callObject>
														<plx:callThis name="pnlSave" type="field" accessor="this"/>
													</plx:callObject>
												</plx:callInstance>
											</plx:left>
											<plx:right>
												<plx:trueKeyword/>
											</plx:right>
										</plx:assign>

										<plx:assign>
											<plx:left>
												<plx:callInstance name="Visible" type="property">
													<plx:callObject>
														<plx:callThis name="dgvCurrentObject" type="field" accessor="this"/>
													</plx:callObject>
												</plx:callInstance>
											</plx:left>
											<plx:right>
												<plx:trueKeyword/>
											</plx:right>
										</plx:assign>

										<plx:assign>
											<plx:left>
												<plx:callInstance name="Text" type="property">
													<plx:callObject>
														<plx:callThis name="lblCurrentObject" type="field" accessor="this"/>
													</plx:callObject>
												</plx:callInstance>
											</plx:left>
											<plx:right>
												<plx:string data="The current selected {@displayName}:"/>
											</plx:right>
										</plx:assign>

										<xsl:for-each select="Property[not(@name = 'Context')][substring-before(@name, 'Collection') = '']">
											<plx:assign>
												<plx:left>
													<plx:callInstance name="Value" type="property">
														<plx:callObject>
															<plx:callInstance name="Cells" type="property">
																<plx:callObject>
																	<plx:callInstance name="Rows" type="property">
																		<plx:callObject>
																			<plx:callInstance name="dgvCurrentObject" type="field">
																				<plx:callObject>
																					<plx:thisKeyword/>
																				</plx:callObject>
																			</plx:callInstance>
																		</plx:callObject>
																		<plx:passParam>
																			<plx:value data="0" type="i4"/>
																		</plx:passParam>
																	</plx:callInstance>
																</plx:callObject>
																<xsl:choose>
																	<xsl:when test="@displayName">
																		<plx:passParam>
																			<plx:string data="{@displayName}"/>
																		</plx:passParam>
																	</xsl:when>
																	<xsl:otherwise>
																		<plx:passParam>
																			<plx:string data="{@name}"/>
																		</plx:passParam>
																	</xsl:otherwise>
																</xsl:choose>
															</plx:callInstance>
														</plx:callObject>
													</plx:callInstance>
												</plx:left>
												<plx:right>
													<xsl:variable name="instanceCallerFragment">
														<plx:callThis name="abstractTypeVar" type="field" accessor="this"/>
													</xsl:variable>
													<xsl:call-template name="PropertyAccessor">
														<xsl:with-param name="ID_Kind" select="@dataTypeName"/>
														<xsl:with-param name="Name" select="@name"/>
														<xsl:with-param name="InstanceCaller" select="exsl:node-set($instanceCallerFragment)/child::*"/>
														<xsl:with-param name="OilInfo" select="$OilInfo"/>
													</xsl:call-template>
												</plx:right>
											</plx:assign>

											<plx:assign>
												<plx:left>
													<plx:callInstance name="Visible" type="property">
														<plx:callObject>
															<plx:callInstance name="Columns" type="property">
																<plx:callObject>
																	<plx:callInstance name="dgvCurrentObject" type="field">
																		<plx:callObject>
																			<plx:thisKeyword/>
																		</plx:callObject>
																	</plx:callInstance>
																</plx:callObject>
																<xsl:choose>
																	<xsl:when test="@displayName">
																		<plx:passParam>
																			<plx:string data="{@displayName}"/>
																		</plx:passParam>
																	</xsl:when>
																	<xsl:otherwise>
																		<plx:passParam>
																			<plx:string data="{@name}"/>
																		</plx:passParam>
																	</xsl:otherwise>
																</xsl:choose>
															</plx:callInstance>
														</plx:callObject>
													</plx:callInstance>
												</plx:left>
												<plx:right>
													<plx:trueKeyword/>
												</plx:right>
											</plx:assign>
										</xsl:for-each>
									</plx:fallbackBranch>

									<plx:assign>
										<plx:left>
											<plx:callInstance name="Text" type="field">
												<plx:callObject>
													<plx:callThis name="lblNeedToSave" type="field" accessor="this"/>
												</plx:callObject>
											</plx:callInstance>
										</plx:left>
										<plx:right>
											<plx:string></plx:string>
										</plx:right>
									</plx:assign>

									<plx:catch localName="exception" dataTypeName="Exception">
										<plx:callStatic name="Show" dataTypeName="MessageBox" dataTypeQualifier="System.Windows.Forms" type="methodCall">
											<plx:passParam>
												<plx:callInstance name="Message" type="property">
													<plx:callObject>
														<plx:nameRef name="exception" type="local"/>
													</plx:callObject>
												</plx:callInstance>
											</plx:passParam>
											<plx:passParam>
												<plx:string data="Display {@displayName}"/>
											</plx:passParam>
										</plx:callStatic>
									</plx:catch>
								</plx:try>

							</plx:function>

							<plx:function name="cbxSelectionMode_SelectedIndexChanged" visibility="private">
								<plx:param name="sender" dataTypeName="object"/>
								<plx:param name="e" dataTypeName="EventArgs" dataTypeQualifier="System"/>

								<plx:try>
									<xsl:for-each select="Property[not(@name = 'Context')][substring-before(@name, 'Collection') = '']">
										<plx:assign>
											<plx:left>
												<plx:callInstance name="Visible" type="property">
													<plx:callObject>
														<plx:callInstance name="Columns" type="property">
															<plx:callObject>
																<plx:callInstance name="dgvSelect" type="field">
																	<plx:callObject>
																		<plx:thisKeyword/>
																	</plx:callObject>
																</plx:callInstance>
															</plx:callObject>
															<xsl:choose>
																<xsl:when test="@displayName">
																	<plx:passParam>
																		<plx:string data="{@displayName}"/>
																	</plx:passParam>
																</xsl:when>
																<xsl:otherwise>
																	<plx:passParam>
																		<plx:string data="{@name}"/>
																	</plx:passParam>
																</xsl:otherwise>
															</xsl:choose>
														</plx:callInstance>
													</plx:callObject>
												</plx:callInstance>
											</plx:left>
											<plx:right>
												<plx:falseKeyword/>
											</plx:right>
										</plx:assign>
									</xsl:for-each>

									<plx:switch>
										<plx:condition>
											<plx:callInstance name="ToString" type="methodCall">
												<plx:callObject>
													<plx:callInstance name="SelectedItem" type="property">
														<plx:callObject>
															<plx:callThis name="cbxSelectionMode" type="field" accessor="this"/>
														</plx:callObject>
													</plx:callInstance>
												</plx:callObject>
											</plx:callInstance>
										</plx:condition>
										<xsl:for-each select="../TestClass/Actions/Select/Function[current()/@displayName = @objectToSelect]">
											<plx:case>
												<plx:condition>
													<plx:string data="{@selectionMode}"/>
												</plx:condition>
												<xsl:for-each select="Parameter">
													<plx:assign>
														<plx:left>
															<plx:callInstance name="Visible" type="property">
																<plx:callObject>
																	<plx:callInstance name="Columns" type="property">
																		<plx:callObject>
																			<plx:callInstance name="dgvSelect" type="field">
																				<plx:callObject>
																					<plx:thisKeyword/>
																				</plx:callObject>
																			</plx:callInstance>
																		</plx:callObject>
																		<xsl:choose>
																			<xsl:when test="@displayName">
																				<plx:passParam>
																					<plx:string data="{@displayName}"/>
																				</plx:passParam>
																			</xsl:when>
																			<xsl:otherwise>
																				<plx:passParam>
																					<plx:string data="{@name}"/>
																				</plx:passParam>
																			</xsl:otherwise>
																		</xsl:choose>
																	</plx:callInstance>
																</plx:callObject>
															</plx:callInstance>
														</plx:left>
														<plx:right>
															<plx:trueKeyword/>
														</plx:right>
													</plx:assign>
												</xsl:for-each>
											</plx:case>
										</xsl:for-each>
										<plx:fallbackCase>
										</plx:fallbackCase>
									</plx:switch>

									<plx:catch localName="exception" dataTypeName="Exception">
										<plx:callStatic name="Show" dataTypeName="MessageBox" dataTypeQualifier="System.Windows.Forms" type="methodCall">
											<plx:passParam>
												<plx:callInstance name="Message" type="property">
													<plx:callObject>
														<plx:nameRef name="exception" type="local"/>
													</plx:callObject>
												</plx:callInstance>
											</plx:passParam>
											<plx:passParam>
												<plx:string data="Change Selection Mode for {@displayName}"/>
											</plx:passParam>
										</plx:callStatic>
									</plx:catch>
								</plx:try>

							</plx:function>

							<plx:function name="btnSave_Click" visibility="private">
								<plx:param name="sender" dataTypeName="object"/>
								<plx:param name="e" dataTypeName="EventArgs" dataTypeQualifier="System"/>

								<plx:try>
									<xsl:for-each select="Property[not(@autoIncrement = 'true')][not(@name = 'Context')][substring-before(@name, 'Collection') = '']">

										<xsl:call-template name="UpdatePropertyValue">
											<xsl:with-param name="Property" select="current()"/>
											<xsl:with-param name="DGV" select="'dgvCurrentObject'"/>
											<xsl:with-param name="FunctionList" select="../../TestClass/Actions/Select/Function"/>
											<xsl:with-param name="AllClasses" select="$AllClasses"/>
										</xsl:call-template>
									</xsl:for-each>

									<plx:assign>
										<plx:left>
											<plx:callInstance name="Text" type="field">
												<plx:callObject>
													<plx:callThis name="lblNeedToSave" type="field" accessor="this"/>
												</plx:callObject>
											</plx:callInstance>
										</plx:left>
										<plx:right>
											<plx:string></plx:string>
										</plx:right>
									</plx:assign>

									<plx:assign>
										<plx:left>
											<plx:callInstance name="Enabled" type="property">
												<plx:callObject>
													<plx:callThis name="btnSave" type="field" accessor="this"/>
												</plx:callObject>
											</plx:callInstance>
										</plx:left>
										<plx:right>
											<plx:falseKeyword/>
										</plx:right>
									</plx:assign>

									<plx:assign>
										<plx:left>
											<plx:callInstance name="Enabled" type="property">
												<plx:callObject>
													<plx:callThis name="btnCancel" type="field" accessor="this"/>
												</plx:callObject>
											</plx:callInstance>
										</plx:left>
										<plx:right>
											<plx:falseKeyword/>
										</plx:right>
									</plx:assign>

									<plx:catch localName="exception" dataTypeName="Exception">
										<plx:callStatic name="Show" dataTypeName="MessageBox" dataTypeQualifier="System.Windows.Forms" type="methodCall">
											<plx:passParam>
												<plx:callInstance name="Message" type="property">
													<plx:callObject>
														<plx:nameRef name="exception" type="local"/>
													</plx:callObject>
												</plx:callInstance>
											</plx:passParam>
											<plx:passParam>
												<plx:string data="Save {@displayName}"/>
											</plx:passParam>
										</plx:callStatic>
									</plx:catch>
								</plx:try>

							</plx:function>

							<plx:function name="btnCancel_Click" visibility="private">
								<plx:param name="sender" dataTypeName="object"/>
								<plx:param name="e" dataTypeName="EventArgs" dataTypeQualifier="System"/>

								<plx:try>
									<plx:callThis name="DisplaySelection" type="methodCall" accessor="this"/>

									<plx:assign>
										<plx:left>
											<plx:callInstance name="Text" type="field">
												<plx:callObject>
													<plx:callThis name="lblNeedToSave" type="field" accessor="this"/>
												</plx:callObject>
											</plx:callInstance>
										</plx:left>
										<plx:right>
											<plx:callStatic type="field" name="Empty" dataTypeName=".string"/>
										</plx:right>
									</plx:assign>

									<plx:assign>
										<plx:left>
											<plx:callInstance name="Enabled" type="property">
												<plx:callObject>
													<plx:callThis name="btnCancel" type="field" accessor="this"/>
												</plx:callObject>
											</plx:callInstance>
										</plx:left>
										<plx:right>
											<plx:falseKeyword/>
										</plx:right>
									</plx:assign>

									<plx:assign>
										<plx:left>
											<plx:callInstance name="Enabled" type="property">
												<plx:callObject>
													<plx:callThis name="btnSave" type="field" accessor="this"/>
												</plx:callObject>
											</plx:callInstance>
										</plx:left>
										<plx:right>
											<plx:falseKeyword/>
										</plx:right>
									</plx:assign>

									<plx:catch localName="exception" dataTypeName="Exception">
										<plx:callStatic name="Show" dataTypeName="MessageBox" dataTypeQualifier="System.Windows.Forms" type="methodCall">
											<plx:passParam>
												<plx:callInstance name="Message" type="property">
													<plx:callObject>
														<plx:nameRef name="exception" type="local"/>
													</plx:callObject>
												</plx:callInstance>
											</plx:passParam>
											<plx:passParam>
												<plx:string data="Cancel Edit {@displayName}"/>
											</plx:passParam>
										</plx:callStatic>
									</plx:catch>
								</plx:try>

							</plx:function>

							<plx:function name="dgvCurrentObject_CellBeginEdit" visibility="private">
								<plx:param name="sender" dataTypeName="object"/>
								<plx:param name="e" dataTypeName="DataGridViewCellCancelEventArgs" dataTypeQualifier="System.Windows.Forms"/>

								<plx:try>
									<plx:branch>
										<plx:condition>
											<plx:callThis name="editMode" type="field" accessor="this"/>
										</plx:condition>

										<plx:assign>
											<plx:left>
												<plx:callInstance name="Text" type="field">
													<plx:callObject>
														<plx:callThis name="lblNeedToSave" type="field" accessor="this"/>
													</plx:callObject>
												</plx:callInstance>
											</plx:left>
											<plx:right>
												<plx:string data="The changed data has not been saved."/>
											</plx:right>
										</plx:assign>

										<plx:assign>
											<plx:left>
												<plx:callInstance name="Enabled" type="property">
													<plx:callObject>
														<plx:callThis name="btnCancel" type="field" accessor="this"/>
													</plx:callObject>
												</plx:callInstance>
											</plx:left>
											<plx:right>
												<plx:trueKeyword/>
											</plx:right>
										</plx:assign>

										<plx:assign>
											<plx:left>
												<plx:callInstance name="Enabled" type="property">
													<plx:callObject>
														<plx:callThis name="btnSave" type="field" accessor="this"/>
													</plx:callObject>
												</plx:callInstance>
											</plx:left>
											<plx:right>
												<plx:trueKeyword/>
											</plx:right>
										</plx:assign>
									</plx:branch>

									<plx:catch localName="exception" dataTypeName="Exception">
										<plx:callStatic name="Show" dataTypeName="MessageBox" dataTypeQualifier="System.Windows.Forms" type="methodCall">
											<plx:passParam>
												<plx:callInstance name="Message" type="property">
													<plx:callObject>
														<plx:nameRef name="exception" type="local"/>
													</plx:callObject>
												</plx:callInstance>
											</plx:passParam>
											<plx:passParam>
												<plx:string data="Begin Edit {@displayName}"/>
											</plx:passParam>
										</plx:callStatic>
									</plx:catch>
								</plx:try>

							</plx:function>
						</plx:class>

					</xsl:for-each>
				</plx:namespace>
			</xsl:variable>
			
			<xsl:choose>
				<xsl:when test="$DefaultNamespace">
					<plx:namespace name="{$DefaultNamespace}">
						<xsl:copy-of select="exsl:node-set($bodyFragment)/child::*"/>
					</plx:namespace>
				</xsl:when>
				<xsl:otherwise>
					<xsl:copy-of select="exsl:node-set($bodyFragment)/child::*"/>
				</xsl:otherwise>
			</xsl:choose>

		</plx:root>
	</xsl:template>

	<!-- Define methods needed in each custom control.  Can filter by ActionType. -->
	<xsl:template name="CommonMethods">
		<xsl:param name="ActionType"/>
		<!-- The type of manipulation you are doing.  Do not use the ActionType if the control is not doing any manipulation. - Create, Select, Collection -->
		<xsl:param name="AbstractTypeName"/>
		<!-- The name of the abstract entity type to manipulate. -->

		<xsl:choose>
			<xsl:when test="$ActionType">
				<plx:function name="GetConnection" visibility="public">
					<plx:returns dataTypeName="IDbConnection"/>
					<plx:try>
						<plx:local name="connectionString" dataTypeName=".string">
							<plx:initialize>
								<plx:callInstance name="ConnectionString" type="property">
									<plx:callObject>
										<plx:callStatic name="ConnectionStrings" dataTypeName="ConfigurationManager" dataTypeQualifier="System.Configuration" type="property">
											<plx:passParam>
												<plx:string data="connString"/>
											</plx:passParam>
										</plx:callStatic>
									</plx:callObject>
								</plx:callInstance>
							</plx:initialize>
						</plx:local>
						<plx:return>
							<plx:callNew dataTypeName="SqlConnection">
								<plx:passParam>
									<plx:nameRef name="connectionString" type="local"/>
								</plx:passParam>
							</plx:callNew>
						</plx:return>
						<plx:catch localName="exception" dataTypeName="Exception">
							<plx:callStatic name="Show" dataTypeName="MessageBox" dataTypeQualifier="System.Windows.Forms" type="methodCall">
								<plx:passParam>
									<plx:callInstance name="Message" type="property">
										<plx:callObject>
											<plx:nameRef name="exception" type="local"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:passParam>
								<plx:passParam>
									<plx:string data="Get Database Connection for {$ActionType} on {$AbstractTypeName}"/>
								</plx:passParam>
							</plx:callStatic>

							<plx:return>
								<plx:nullKeyword/>
							</plx:return>
						</plx:catch>
					</plx:try>

				</plx:function>
			</xsl:when>
		</xsl:choose>
	</xsl:template>

	<!-- When the data type of a property is an abstract entity type then a special display name is useful to a user.  
	The display name will follow this pattern:  [Property Name]([Perferred Identifier for abstract type returned by property]).
	Since a perferred identifier can be an entity type, this template is recursive. -->
	<xsl:template name="CreatePreferredIdentifierDisplayName">
		<xsl:param name="ID_Kind" select="value"/>
		<!-- ID_Kind will be 'value' if it is a value type or the name of the type if it is an entity type -->
		<xsl:param name="Name"/>
		<!-- The name of the property -->
		<xsl:param name="OilInfo"/>
		<!-- OilInfo var -->

		<xsl:choose>
			<xsl:when test="not($ID_Kind) or $ID_Kind = 'value' or not(substring-after($ID_Kind, '.') = '')">
				<xsl:value-of select="$Name"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$Name"/>
				<xsl:text>(</xsl:text>
				<xsl:call-template name="CreatePreferredIdentifierDisplayName">
					<xsl:with-param name="ID_Kind" select="$OilInfo/Type[@name = $ID_Kind]/PreferredIdentifier/Field/@id_kind"/>
					<xsl:with-param name="Name" select="$OilInfo/Type[@name = $ID_Kind]/PreferredIdentifier/Field/@name"/>
					<xsl:with-param name="OilInfo" select="$OilInfo"/>
				</xsl:call-template>
				<xsl:text>)</xsl:text>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- Used when creating the AllClasses var from the Oil var -->
	<xsl:template name="ParameterElement">
		<xsl:param name="ParameterSet"/>
		<!-- The list of parameters for a function. -->
		<xsl:param name="OilInfo"/>
		<!-- OilInfo var -->

		<xsl:for-each select="$ParameterSet">
			<xsl:element name="Parameter">
				<xsl:copy-of select="@*"/>
				<xsl:attribute name="dataTypeName">
					<xsl:choose>
						<xsl:when test="@dataTypeName = 'Nullable'">
							<xsl:value-of select="plx:passTypeParam/@dataTypeName"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="@dataTypeName"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<xsl:choose>
					<xsl:when test="not(substring-after(@dataTypeName, '.') = '')">
						<!-- value type -->
						<xsl:attribute name="displayName">
							<xsl:call-template name="CreatePreferredIdentifierDisplayName">
								<xsl:with-param name="ID_Kind" select="'value'"/>
								<xsl:with-param name="Name" select="@name"/>
								<xsl:with-param name="OilInfo" select="$OilInfo"/>
							</xsl:call-template>
						</xsl:attribute>
					</xsl:when>
					<xsl:otherwise>
						<!-- entity type -->
						<xsl:attribute name="displayName">
							<xsl:call-template name="CreatePreferredIdentifierDisplayName">
								<xsl:with-param name="ID_Kind" select="@dataTypeName"/>
								<xsl:with-param name="Name" select="@name"/>
								<xsl:with-param name="OilInfo" select="$OilInfo"/>
							</xsl:call-template>
						</xsl:attribute>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:element>
		</xsl:for-each>
	</xsl:template>

	<!-- Use when calling a method to ensure that you are passing it all the required parameters. -->
	<xsl:template name="SelectParameters">
		<xsl:param name="ParameterSet"/>
		<!-- The set of parameters for the method -->
		<xsl:param name="AllClasses"/>
		<!-- AllClasses var -->
		<xsl:param name="DGV" select="'dgvSelect'"/>
		<!-- The name of the DataGridView -->
		<xsl:param name="DisplayName"/>
		<!-- The display name of the parameter in the DataGridView-->

		<xsl:for-each select="$ParameterSet">
			<xsl:call-template name="SelectParametersWorker">
				<xsl:with-param name="Parameter" select="current()"/>
				<xsl:with-param name="AllClasses" select="$AllClasses"/>
				<xsl:with-param name="DGV" select="$DGV"/>
				<xsl:with-param name="DisplayName" select="$DisplayName"/>
			</xsl:call-template>
		</xsl:for-each>
	</xsl:template>

	<!-- Does all the recursive work for the SelectParameters template -->
	<xsl:template name="SelectParametersWorker">
		<xsl:param name="Parameter"/>
		<!-- The set of parameters for the method -->
		<xsl:param name="AllClasses"/>
		<!-- AllClasses var -->
		<xsl:param name="DGV" select="'dgvSelect'"/>
		<!-- The name of the DataGridView -->
		<xsl:param name="DisplayName"/>
		<!-- The display name of the parameter in the DataGridView-->

		<xsl:choose>
			<xsl:when test="substring-after($Parameter/@dataTypeName, '.')">
				<plx:passParam>
					<xsl:call-template name="GetCellValue">
						<xsl:with-param name="Property" select="$Parameter"/>
						<xsl:with-param name="DGV" select="$DGV"/>
						<xsl:with-param name="DisplayName" select="$DisplayName"/>
					</xsl:call-template>
				</plx:passParam>
			</xsl:when>
			<xsl:otherwise>
				<xsl:for-each select="$AllClasses[@displayName = current()/@dataTypeName]/PreferredIdentifier/ID_Field">
					<plx:passParam>
						<plx:callInstance name="Get{../../@displayName}By{@name}" type="methodCall">
							<plx:callObject>
								<plx:callThis name="testVar" type="field" accessor="this"/>
							</plx:callObject>
							<plx:passParam>
								<plx:callInstance name="ToString" type="methodCall">
									<plx:callObject>
										<plx:callInstance name="Value" type="property">
											<plx:callObject>
												<plx:callInstance name="Cells" type="property">
													<plx:callObject>
														<plx:callInstance name="Rows" type="property">
															<plx:callObject>
																<plx:callInstance name="{$DGV}" type="field">
																	<plx:callObject>
																		<plx:thisKeyword/>
																	</plx:callObject>
																</plx:callInstance>
															</plx:callObject>
															<plx:passParam>
																<plx:value data="0" type="i4"/>
															</plx:passParam>
														</plx:callInstance>
													</plx:callObject>
													<xsl:choose>
														<xsl:when test="not(@id_kind = 'value')">
															<xsl:call-template name="SelectParametersWorker">
																<xsl:with-param name="Parameter" select="$Parameter"/>
																<xsl:with-param name="AllClasses" select="$AllClasses"/>
																<xsl:with-param name="DGV" select="$DGV"/>
																<xsl:with-param name="DisplayName" select="$DisplayName"/>
															</xsl:call-template>
														</xsl:when>
														<xsl:when test="$DisplayName">
															<plx:passParam>
																<plx:string data="{$DisplayName}"/>
															</plx:passParam>
														</xsl:when>
														<xsl:when test="@displayName">
															<plx:passParam>
																<plx:string data="{$Parameter/@displayName}"/>
															</plx:passParam>
														</xsl:when>
														<xsl:otherwise>
															<plx:passParam>
																<plx:string data="{$Parameter/@name}"/>
															</plx:passParam>
														</xsl:otherwise>
													</xsl:choose>
												</plx:callInstance>
											</plx:callObject>
										</plx:callInstance>
									</plx:callObject>
								</plx:callInstance>
							</plx:passParam>
						</plx:callInstance>
					</plx:passParam>
				</xsl:for-each>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- Use this to display the value of a property.  It will return the value or the value of the perferred identifier. -->
	<xsl:template name="PropertyAccessor">
		<xsl:param name="ID_Kind" select="value"/>
		<!-- ID_Kind will be 'value' if it is a value type or the name of the type if it is an entity type -->
		<xsl:param name="Name"/>
		<!-- The name of the property -->
		<xsl:param name="InstanceCaller"/>
		<!-- Who is calling the property. -->
		<xsl:param name="OilInfo"/>
		<!-- OilInfo var -->

		<xsl:variable name="newInstanceCallerFragment">
			<plx:callInstance name="{$Name}" type="property">
				<plx:callObject>
					<xsl:copy-of select="$InstanceCaller"/>
				</plx:callObject>
			</plx:callInstance>
		</xsl:variable>
		<xsl:variable name="newInstanceCaller" select="exsl:node-set($newInstanceCallerFragment)/child::*"/>
		
		<xsl:choose>
			<xsl:when test="not($ID_Kind) or $ID_Kind = 'value' or not(substring-after($ID_Kind, '.') = '')">
				<xsl:copy-of select="$newInstanceCaller"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="PropertyAccessor">
					<xsl:with-param name="ID_Kind" select="$OilInfo/Type[@name = $ID_Kind]/PreferredIdentifier/Field/@id_kind"/>
					<xsl:with-param name="Name" select="$OilInfo/Type[@name = $ID_Kind]/PreferredIdentifier/Field/@name"/>
					<xsl:with-param name="InstanceCaller" select="$newInstanceCaller"/>
					<xsl:with-param name="OilInfo" select="$OilInfo"/>
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- Get a value from a DataGridView cell and cast it accordingly. -->
	<xsl:template name="GetCellValue">
		<xsl:param name="Property"/>
		<!-- The property that is displayed in the DataGridView. -->
		<xsl:param name="DGV"/>
		<!-- The name of the DataGridView -->
		<xsl:param name="DisplayName"/>
		<!-- The name of the column in the DataGridView -->

		<!-- Determine the type so you can cast it appropriatly. -->
		<xsl:choose>
			<xsl:when test="$Property/@dataTypeName = '.object'">
				<xsl:call-template name="GetCellValueObject">
					<xsl:with-param name="Property" select="$Property"/>
					<xsl:with-param name="DGV" select="$DGV"/>
					<xsl:with-param name="DisplayName" select="$DisplayName"/>
				</xsl:call-template>
			</xsl:when>
			<!-- Check to see if the type is one of the remaining supported types, except string.  string is the default type. -->
			<xsl:when test="$Property/@dataTypeName = '.boolean' or $Property/@dataTypeName = '.char' or $Property/@dataTypeName = '.date' or $Property/@dataTypeName = '.decimal' or $Property/@dataTypeName = '.i1' or $Property/@dataTypeName = '.i2' or $Property/@dataTypeName = '.i4' or $Property/@dataTypeName = '.i8' or $Property/@dataTypeName = '.r4' or $Property/@dataTypeName = '.r8' or $Property/@dataTypeName = '.u1' or $Property/@dataTypeName = '.u2' or $Property/@dataTypeName = '.u4' or $Property/@dataTypeName = '.u8'">
				<xsl:call-template name="GetCellValueSupportedType">
					<xsl:with-param name="Property" select="$Property"/>
					<xsl:with-param name="DGV" select="$DGV"/>
					<xsl:with-param name="DisplayName" select="$DisplayName"/>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="GetCellValueString">
					<xsl:with-param name="Property" select="$Property"/>
					<xsl:with-param name="DGV" select="$DGV"/>
					<xsl:with-param name="DisplayName" select="$DisplayName"/>
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- Cast a specified value from a DataGridView as a PLiX supported value type -->
	<xsl:template name="GetCellValueSupportedType">
		<xsl:param name="Property"/>
		<!-- The property that is displayed in the DataGridView. -->
		<xsl:param name="DGV"/>
		<!-- The name of the DataGridView -->
		<xsl:param name="DisplayName"/>
		<!-- The name of the column in the DataGridView -->
		<plx:callStatic name="Parse" dataTypeName="{$Property/@dataTypeName}" type="methodCall">
			<plx:passParam>
				<xsl:call-template name="GetCellValueString">
					<xsl:with-param name="Property" select="$Property"/>
					<xsl:with-param name="DGV" select="$DGV"/>
					<xsl:with-param name="DisplayName" select="$DisplayName"/>
				</xsl:call-template>
			</plx:passParam>
		</plx:callStatic>
	</xsl:template>

	<!-- Cast a specified value from a DataGridView as a string -->
	<xsl:template name="GetCellValueString">
		<xsl:param name="Property"/>
		<!-- The property that is displayed in the DataGridView. -->
		<xsl:param name="DGV"/>
		<!-- The name of the DataGridView -->
		<xsl:param name="DisplayName"/>
		<!-- The name of the column in the DataGridView -->
		<plx:callInstance name="ToString" type="methodCall">
			<plx:callObject>
				<xsl:call-template name="GetCellValueObject">
					<xsl:with-param name="Property" select="$Property"/>
					<xsl:with-param name="DGV" select="$DGV"/>
					<xsl:with-param name="DisplayName" select="$DisplayName"/>
				</xsl:call-template>
			</plx:callObject>
		</plx:callInstance>
	</xsl:template>

	<!-- Get a specified value from a DataGridView as an object -->
	<xsl:template name="GetCellValueObject">
		<xsl:param name="Property"/>
		<!-- The property that is displayed in the DataGridView. -->
		<xsl:param name="DGV"/>
		<!-- The name of the DataGridView -->
		<xsl:param name="DisplayName"/>
		<!-- The name of the column in the DataGridView -->
		<plx:callInstance name="Value" type="property">
			<plx:callObject>
				<plx:callInstance name="Cells" type="property">
					<plx:callObject>
						<plx:callInstance name="Rows" type="property">
							<plx:callObject>
								<plx:callInstance name="{$DGV}" type="field">
									<plx:callObject>
										<plx:thisKeyword/>
									</plx:callObject>
								</plx:callInstance>
							</plx:callObject>
							<plx:passParam>
								<plx:value data="0" type="i4"/>
							</plx:passParam>
						</plx:callInstance>
					</plx:callObject>
					<xsl:choose>
						<xsl:when test="$DisplayName">
							<plx:passParam>
								<plx:string data="{$DisplayName}"/>
							</plx:passParam>
						</xsl:when>
						<xsl:when test="$Property/@displayName">
							<plx:passParam>
								<plx:string data="{$Property/@displayName}"/>
							</plx:passParam>
						</xsl:when>
						<xsl:otherwise>
							<plx:passParam>
								<plx:string data="{$Property/@name}"/>
							</plx:passParam>
						</xsl:otherwise>
					</xsl:choose>
				</plx:callInstance>
			</plx:callObject>
		</plx:callInstance>
	</xsl:template>

	<xsl:template name="CreateValueTypeElement">
		<xsl:param name="Element"/>
		<!-- The Element from the OIAL file containing value type. -->
		<xsl:param name="Type"/>
		<!-- The type of the value type. -->
		<xsl:param name="AutoIncrement"/>
		<!-- Pass in true if this value type is incremented by the database. -->
		<xsl:element name="ValueType">
			<xsl:copy-of select="@*"/>
			<xsl:attribute name="dataTypeName">
				<xsl:value-of select="$Type"/>
			</xsl:attribute>
			<xsl:if test="$AutoIncrement">
				<xsl:attribute name="autoIncrement">
					<xsl:value-of select="$AutoIncrement"/>
				</xsl:attribute>
			</xsl:if>
		</xsl:element>
	</xsl:template>

	<!-- Set a specified property value in an abstract entity type from a DataGridView value -->
	<xsl:template name="UpdatePropertyValue">
		<xsl:param name="Property"/>
		<!-- The property that is displayed in the DataGridView. -->
		<xsl:param name="DGV"/>
		<!-- The name of the DataGridView -->
		<xsl:param name="FunctionList"/>
		<!-- All fuctions for the entity type -->
		<xsl:param name="AllClasses"/>
		<!-- AllClasses var -->

		<plx:assign>
			<plx:left>
				<plx:callInstance name="{$Property/@name}" type="property">
					<plx:callObject>
						<plx:callThis name="abstractTypeVar" type="field" accessor="this"/>
					</plx:callObject>
				</plx:callInstance>
			</plx:left>
			<plx:right>
				<xsl:call-template name="CastDGVValue">
					<xsl:with-param name="Property" select="$Property"/>
					<xsl:with-param name="DGV" select="$DGV"/>
					<xsl:with-param name="FunctionList" select="$FunctionList"/>
					<xsl:with-param name="AllClasses" select="$AllClasses"/>
				</xsl:call-template>
			</plx:right>
		</plx:assign>
	</xsl:template>

	<!-- Cast a specified value from a DataGridView -->
	<xsl:template name="CastDGVValue">
		<xsl:param name="Property"/>
		<!-- The property that is displayed in the DataGridView. -->
		<xsl:param name="DGV"/>
		<!-- The name of the DataGridView -->
		<xsl:param name="FunctionList"/>
		<!-- All fuctions for the entity type -->
		<xsl:param name="AllClasses"/>
		<!-- AllClasses var -->

		<xsl:choose>
			<!-- Value Type -->
			<xsl:when test="not(substring-after($Property/@dataTypeName, '.') = '')">
				<xsl:call-template name="GetCellValue">
					<xsl:with-param name="Property" select="current()"/>
					<xsl:with-param name="DGV" select="$DGV"/>
				</xsl:call-template>
			</xsl:when>
			<!-- Entity Type -->
			<xsl:otherwise>
				<xsl:choose>
					<!-- Single Ref Mode -->
					<xsl:when test="$FunctionList[@objectToSelect = $Property/@dataTypeName][@selectionMode = $AllClasses/../Class[@displayName = $Property/@dataTypeName]/PreferredIdentifier/ID_Field/@name]/@name">
						<plx:callInstance name="{$FunctionList[@objectToSelect = $Property/@dataTypeName][@selectionMode = $AllClasses/../Class[@displayName = $Property/@dataTypeName]/PreferredIdentifier/ID_Field/@name]/@name}" type="methodCall">
							<plx:callObject>
								<plx:callThis name="testVar" type="field" accessor="this"/>
							</plx:callObject>
							<xsl:choose>
								<xsl:when test="$Property/@displayName">
									<xsl:call-template name="SelectParameters">
										<xsl:with-param name="ParameterSet" select="$FunctionList[@objectToSelect = $Property/@dataTypeName][@selectionMode = $AllClasses/../Class[@displayName = $Property/@dataTypeName]/PreferredIdentifier/ID_Field/@name]/Parameter"/>
										<xsl:with-param name="AllClasses" select="$AllClasses"/>
										<xsl:with-param name="DGV" select="$DGV"/>
										<xsl:with-param name="DisplayName" select="$Property/@displayName"/>
									</xsl:call-template>
								</xsl:when>
								<xsl:otherwise>
									<xsl:call-template name="SelectParameters">
										<xsl:with-param name="ParameterSet" select="$FunctionList[@objectToSelect = $Property/@dataTypeName][@selectionMode = $AllClasses/../Class[@displayName = $Property/@dataTypeName]/PreferredIdentifier/ID_Field/@name]/Parameter"/>
										<xsl:with-param name="AllClasses" select="$AllClasses"/>
										<xsl:with-param name="DGV" select="$DGV"/>
										<xsl:with-param name="DisplayName" select="$Property/@name"/>
									</xsl:call-template>
								</xsl:otherwise>
							</xsl:choose>

						</plx:callInstance>
					</xsl:when>
					<!-- Complex Ref Mode -->
					<!-- This part might not work yet, but it seems like it is. -->
					<xsl:otherwise>
						<plx:callInstance name="{$FunctionList[@objectToSelect = $Property/@dataTypeName][not(@selectionMode = $AllClasses/../Class[@displayName = $Property/@dataTypeName]/PreferredIdentifier/ID_Field/@name)]/@name}" type="methodCall">
							<plx:callObject>
								<plx:callThis name="testVar" type="field" accessor="this"/>
							</plx:callObject>
							<xsl:choose>
								<xsl:when test="$Property/@displayName">
									<xsl:call-template name="SelectParameters">
										<xsl:with-param name="ParameterSet" select="$FunctionList[@objectToSelect = $Property/@dataTypeName][not(@selectionMode = $AllClasses/../Class[@displayName = $Property/@dataTypeName]/PreferredIdentifier/ID_Field/@name)]/Parameter"/>
										<xsl:with-param name="AllClasses" select="$AllClasses"/>
										<xsl:with-param name="DGV" select="$DGV"/>
										<xsl:with-param name="DisplayName" select="$Property/@displayName"/>
									</xsl:call-template>
								</xsl:when>
								<xsl:otherwise>
									<xsl:call-template name="SelectParameters">
										<xsl:with-param name="ParameterSet" select="$FunctionList[@objectToSelect = $Property/@dataTypeName][not(@selectionMode = $AllClasses/../Class[@displayName = $Property/@dataTypeName]/PreferredIdentifier/ID_Field/@name)]/Parameter"/>
										<xsl:with-param name="AllClasses" select="$AllClasses"/>
										<xsl:with-param name="DGV" select="$DGV"/>
										<xsl:with-param name="DisplayName" select="$Property/@name"/>
									</xsl:call-template>
								</xsl:otherwise>
							</xsl:choose>
						</plx:callInstance>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- Cast a specified value from a DataGridView, then get the preferred identifier if it is an entity type -->
	<xsl:template name="CastDGVValueAndGetID">
		<xsl:param name="Property"/>
		<!-- The property that is displayed in the DataGridView. -->
		<xsl:param name="DGV"/>
		<!-- The name of the DataGridView -->
		<xsl:param name="FunctionList"/>
		<!-- All fuctions for the entity type -->
		<xsl:param name="AllClasses"/>
		<!-- AllClasses var -->
		<xsl:param name="OilInfo"/>
		<!-- OilInfo var -->

		<xsl:choose>
			<!-- Value Type -->
			<xsl:when test="not(substring-after($Property/@dataTypeName, '.') = '')">
				<xsl:call-template name="CastDGVValue">
					<xsl:with-param name="Property" select="$Property"/>
					<xsl:with-param name="DGV" select="$DGV"/>
					<xsl:with-param name="FunctionList" select="$FunctionList"/>
					<xsl:with-param name="AllClasses" select="$AllClasses"/>
				</xsl:call-template>
			</xsl:when>
			<!-- Entity Type -->
			<xsl:otherwise>
				<xsl:choose>
					<!-- Single Ref Mode -->
					<xsl:when test="$FunctionList[@objectToSelect = $Property/@dataTypeName][@selectionMode = $AllClasses/../Class[@displayName = $Property/@dataTypeName]/PreferredIdentifier/ID_Field/@name]/@name">
						<plx:callInstance name="{$OilInfo/Type[@name = $Property/@dataTypeName]/PreferredIdentifier/Field/@name}" type="property">
							<plx:callObject>
								<xsl:call-template name="CastDGVValue">
									<xsl:with-param name="Property" select="$Property"/>
									<xsl:with-param name="DGV" select="$DGV"/>
									<xsl:with-param name="FunctionList" select="$FunctionList"/>
									<xsl:with-param name="AllClasses" select="$AllClasses"/>
								</xsl:call-template>
							</plx:callObject>
						</plx:callInstance>
					</xsl:when>
					<!-- Complex Ref Mode -->
					<!-- Enter at your own risk. -->
					<xsl:otherwise>
						<plx:callInstance name="{$OilInfo/Type[@name = $Property/@dataTypeName]/PreferredIdentifier/Field/@name}" type="property">
							<plx:callObject>
								<xsl:call-template name="CastDGVValue">
									<xsl:with-param name="Property" select="$Property"/>
									<xsl:with-param name="DGV" select="$DGV"/>
									<xsl:with-param name="FunctionList" select="$FunctionList"/>
									<xsl:with-param name="AllClasses" select="$AllClasses"/>
								</xsl:call-template>
							</plx:callObject>
						</plx:callInstance>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

</xsl:stylesheet>
