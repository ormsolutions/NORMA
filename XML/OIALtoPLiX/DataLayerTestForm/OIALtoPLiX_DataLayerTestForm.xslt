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
	<!-- Output file:  [ORM Model Name]TestForm.PLiX.xml -->

	<!-- The generated code for this file should resemble the typical designer file. -->

	<xsl:import href="../OIALtoPLiX_GlobalSupportFunctions.xslt"/>

	<xsl:output method="xml" encoding="utf-8" media-type="text/xml" indent="yes"/>

	<xsl:variable name="SprocSuffix" select="'SP'"/>
	<xsl:variable name="CollectionSuffix" select="'Collections'"/>
	<xsl:variable name="debugMode" select="'false'"/>

	<xsl:template match="/">

		<xsl:variable name="Model" select="./child::*"/>
		<xsl:variable name="ModelName" select="$Model/plx:namespace/@name"/>
		<xsl:variable name="ModelContextName" select="concat($ModelName,'Context')"/>
		<xsl:variable name="AllClassesFragment">
			<xsl:for-each select="plx:root/plx:namespace//plx:class">
				<xsl:choose>
					<xsl:when test="not(substring-before(@name, 'Core') = '')">
						<xsl:element name="Class">
							<xsl:copy-of select="@name"/>
							<xsl:attribute name="displayName">
								<xsl:value-of select="substring(@name, 0, string-length(@name) - 3 )"/>
							</xsl:attribute>
						</xsl:element>
					</xsl:when>
					<xsl:otherwise>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="AllClasses" select="exsl:node-set($AllClassesFragment)/child::*"/>

		<plx:root>
			<xsl:variable name="bodyFragment">
				<plx:namespace name="{$ModelName}">
					<plx:class visibility="public" partial="true" name="{$ModelName}Tester">
						<plx:derivesFromClass dataTypeName="Form" dataTypeQualifier="System.Windows.Forms"/>

						<plx:function name="Main" modifier="static" visibility="public">
							<plx:leadingInfo>
								<plx:docComment>
									<summary>The main entry point for the application.</summary>
								</plx:docComment>
							</plx:leadingInfo>
							<plx:callStatic name="EnableVisualStyles" dataTypeName="Application" dataTypeQualifier="System.Windows.Forms" type="methodCall"/>
							<plx:callStatic name="SetCompatibleTextRenderingDefault" dataTypeName="Application" dataTypeQualifier="System.Windows.Forms" type="methodCall">
								<plx:passParam>
									<plx:falseKeyword/>
								</plx:passParam>
							</plx:callStatic>
							<plx:callStatic name="Run" dataTypeName="Application" dataTypeQualifier="System.Windows.Forms" type="methodCall">
								<plx:passParam>
									<plx:callNew type="new" dataTypeName="{$ModelName}Tester"/>
								</plx:passParam>
							</plx:callStatic>
						</plx:function>

						<plx:function name=".construct" visibility="public">
							<plx:callThis name="InitializeComponent" type="methodCall"/>
						</plx:function>

						<!-- class fields -->
						<plx:field name="components" visibility="private" dataTypeName="IContainer" dataTypeQualifier="System.ComponentModel">
							<plx:leadingInfo>
								<plx:docComment>
									<summary>Required designer variable.</summary>
								</plx:docComment>
							</plx:leadingInfo>
							<plx:initialize>
								<plx:nullKeyword/>
							</plx:initialize>
						</plx:field>
						<plx:field name="MasterTabControl" visibility="private" dataTypeName="TabControl" dataTypeQualifier="System.Windows.Forms"/>

						<!-- Dispose Method -->
						<plx:function name="Dispose" overload="true" modifier="override" visibility="protected">
							<plx:leadingInfo>
								<plx:docComment>
									<summary>Clean up any resources being used.</summary>
									<param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
								</plx:docComment>
							</plx:leadingInfo>
							<plx:param name="disposing" dataTypeName=".boolean"/>
							<plx:branch>
								<plx:condition>
									<plx:binaryOperator type="booleanAnd">
										<plx:left>
											<plx:nameRef name="disposing"/>
										</plx:left>
										<plx:right>
											<plx:binaryOperator type="identityInequality">
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
							<plx:callThis name="Dispose" accessor="base">
								<plx:passParam>
									<plx:nameRef name="disposing"/>
								</plx:passParam>
							</plx:callThis>
						</plx:function>

						<!-- InitializeComponent Method -->
						<plx:function visibility="private" name="InitializeComponent">
							<plx:leadingInfo>
								<plx:pragma type="region" data="InitializeComponent method"/>
								<plx:docComment>
									<summary>Required method for Designer support - do not modify the contents of this method with the code editor.</summary>
								</plx:docComment>
							</plx:leadingInfo>
							<plx:trailingInfo>
								<plx:pragma type="closeRegion" data="InitializeComponent method"/>
							</plx:trailingInfo>

							<!-- set Form fields and properties -->
							<plx:assign>
								<plx:left>
									<plx:callThis name="MasterTabControl" type="field"/>
								</plx:left>
								<plx:right>
									<plx:callNew dataTypeName="TabControl" type="new" dataTypeQualifier="System.Windows.Forms"/>
								</plx:right>
							</plx:assign>

							<plx:assign>
								<plx:left>
									<plx:callInstance name="Dock" type="property">
										<plx:callObject>
											<plx:callThis name="MasterTabControl" accessor="this" type="field"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:left>
								<plx:right>
									<plx:callStatic name="Fill" dataTypeName="DockStyle" dataTypeQualifier="System.Windows.Forms" type="property"/>
								</plx:right>
							</plx:assign>

							<plx:assign>
								<plx:left>
									<plx:callInstance name="Multiline" type="property">
										<plx:callObject>
											<plx:callThis name="MasterTabControl" accessor="this" type="field"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:left>
								<plx:right>
									<plx:trueKeyword/>
								</plx:right>
							</plx:assign>

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
									<plx:string data="NORMA:  {$ModelName} Data Test Form"/>
								</plx:right>
							</plx:assign>

							<plx:assign>
								<plx:left>
									<plx:callInstance name="Size" type="property">
										<plx:callObject>
											<plx:callThis name="MasterTabControl" type="field" accessor="this"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:left>
								<plx:right>
									<plx:callNew dataTypeName="Size" dataTypeQualifier="System.Drawing">
										<plx:passParam>
											<plx:value data="540" type="i4"/>
										</plx:passParam>
										<plx:passParam>
											<plx:value data="520" type="i4"/>
										</plx:passParam>
									</plx:callNew>
								</plx:right>
							</plx:assign>

							<plx:callInstance name="Add" type="methodCall">
								<plx:callObject>
									<plx:callThis name="Controls" type="property" accessor="this"/>
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
											<plx:value data="550" type="i4"/>
										</plx:passParam>
										<plx:passParam>
											<plx:value data="550" type="i4"/>
										</plx:passParam>
									</plx:callNew>
								</plx:right>
							</plx:assign>

							<!-- Add tab page for each class.  The tab will contain a custom control for manipulating instances of the object. -->
							<xsl:for-each select="$AllClasses[@displayName]">
								<xsl:sort select="@name"/>
								<plx:comment blankLine="true"/>
								<plx:callInstance name="Add" type="methodCall">
									<plx:callObject>
										<plx:callInstance name="TabPages" type="property">
											<plx:callObject>
												<plx:callInstance name="MasterTabControl" type="field">
													<plx:callObject>
														<plx:thisKeyword/>
													</plx:callObject>
												</plx:callInstance>
											</plx:callObject>
										</plx:callInstance>
									</plx:callObject>
									<plx:passParam>
										<plx:string data="{@displayName}"/>
									</plx:passParam>
									<plx:passParam>
										<plx:string data="{@displayName}"/>
									</plx:passParam>
								</plx:callInstance>

								<plx:local dataTypeName="{concat(@name, '_InputControl')}" name="{concat('ic', @name)}">
									<plx:initialize>
										<plx:callNew dataTypeName="{concat(@name, '_InputControl')}"/>
									</plx:initialize>
								</plx:local>

								<plx:assign>
									<plx:left>
										<plx:callInstance name="Dock" type="property">
											<plx:callObject>
												<plx:nameRef name="{concat('ic', @name)}" type="local"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:callStatic name="Fill" dataTypeName="DockStyle" dataTypeQualifier="System.Windows.Forms" type="property"/>
									</plx:right>
								</plx:assign>

								<plx:callInstance name="Add" type="methodCall">
									<plx:callObject>
										<plx:callInstance name="Controls" type="property">
											<plx:callObject>
												<plx:callInstance name="TabPages" type="property">
													<plx:callObject>
														<plx:callInstance name="MasterTabControl" type="field">
															<plx:callObject>
																<plx:thisKeyword/>
															</plx:callObject>
														</plx:callInstance>
													</plx:callObject>
													<plx:passParam>
														<plx:string data="{@displayName}"/>
													</plx:passParam>
												</plx:callInstance>
											</plx:callObject>
										</plx:callInstance>
									</plx:callObject>
									<plx:passParam>
										<plx:nameRef name="{concat('ic', @name)}"/>
									</plx:passParam>
								</plx:callInstance>
							</xsl:for-each>

						</plx:function>

					</plx:class>
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

</xsl:stylesheet>
