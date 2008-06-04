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
	<!-- Output file:  InputControl.Designer.PLiX.xml -->

	<xsl:import href="../OIALtoPLiX_GlobalSupportFunctions.xslt"/>
	
	<xsl:param name="OIAL"/>

	<xsl:output method="xml" encoding="utf-8" media-type="text/xml" indent="yes"/>

	<xsl:template match="/">
		<xsl:variable name="Model" select="./child::*"/>
		<xsl:variable name="ModelName" select="$Model/plx:namespace/@name"/>
		<xsl:variable name="ClassName" select="'InputControl'"/>
		<xsl:variable name="GridViewName" select="'dgDataView'"/>
		<xsl:variable name="OIL" select="$OIAL"/>

		<xsl:variable name="OilInfoFragment">
			<!-- The AllTypes element section may not be needed. -->
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
				<!-- Entity Types - Get each type, what fields it has, and what the preferred identifier(s) are. -->
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
								<xsl:for-each select="../../oil:informationTypeFormats/ormdt:identity[@name = current()/@formatRef]">
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
									<xsl:for-each select="../../../../oil:informationTypeFormats/ormdt:identity[@name = current()/@formatRef]">
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
									<xsl:for-each select="../../../../oil:informationTypeFormats/ormdt:identity[@name = current()/@formatRef]">
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
								</xsl:element>
							</xsl:for-each>
							<xsl:for-each select="oil:informationType[oil:singleRoleUniquenessConstraint/@isPreferred='true']">
								<xsl:element name="Field">
									<xsl:copy-of select="@*"/>
									<xsl:attribute name="id_kind">
										<xsl:text>value</xsl:text>
									</xsl:attribute>
									<xsl:for-each select="../../oil:informationTypeFormats/ormdt:identity[@name = current()/@formatRef]">
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
					<!-- These classes represent an entity type -->
					<xsl:when test="substring(@name, string-length(@name) - 3 ) = 'Core'">
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
										<xsl:value-of select="plx:returns/@dataTypeName"/>
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
						<!-- This class can be used to obtain an instance of an entity type. -->
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
										<xsl:value-of select="plx:returns/@dataTypeName"/>
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
			<xsl:variable name="bodyFragment">
				<plx:namespace name="{$ModelName}">
					<!-- There will be 4 custom controls created for each class representing an entity type. -->
					<xsl:for-each select="$AllClasses/../Class">
						<xsl:sort select="@name"/>
						<!--
					
					Input Control Class
					
					-->
						<!-- This custom control has a tab control that holds a custom control that performs a specific action on the entity type. -->
						<plx:class name="{@name}_{$ClassName}" partial="true" visibility="public">
							<plx:leadingInfo>
								<plx:pragma type="region" data="{@name}_{$ClassName}"/>
							</plx:leadingInfo>
							<plx:trailingInfo>
								<plx:pragma type="closeRegion" data="{@name}_{$ClassName}"/>
							</plx:trailingInfo>

							<plx:field name="components" visibility="private" dataTypeName="IContainer" dataTypeQualifier="System.ComponentModel">
								<plx:leadingInfo>
									<plx:docComment>
										<summary>Required designer variable.</summary>
									</plx:docComment>
								</plx:leadingInfo>
							</plx:field>

							<plx:field name="actionTabs" visibility="private" dataTypeName="TabControl" dataTypeQualifier="System.Windows.Forms"/>

							<xsl:call-template name="DeclareCommonFields">
								<xsl:with-param name="ModelName" select="$ModelName"/>
								<xsl:with-param name="TestClassName" select="../TestClass/@name"/>
								<xsl:with-param name="AbstractTypeName" select="substring-before(@name, 'Core')"/>
							</xsl:call-template>

							<plx:function name="Dispose" modifier="override" overload="true" visibility="protected">
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
							</plx:function>

							<plx:function name="InitializeComponent" visibility="private">
								<plx:leadingInfo>
									<plx:pragma type="region" data="InitializeComponent method"/>
									<plx:docComment>
										<summary>Required method for Designer support - do not modify the contents of this method with the code editor.</summary>
									</plx:docComment>
								</plx:leadingInfo>
								<plx:trailingInfo>
									<plx:pragma type="closeRegion" data="InitializeComponent method"/>
								</plx:trailingInfo>

								<plx:assign>
									<plx:left>
										<plx:callThis name="components" type="field"/>
									</plx:left>
									<plx:right>
										<plx:callNew dataTypeName="Container" dataTypeQualifier="System.ComponentModel" type="new"/>
									</plx:right>
								</plx:assign>

								<xsl:call-template name="InitializeCommonFields">
									<xsl:with-param name="ModelName" select="$ModelName"/>
									<xsl:with-param name="TestClassName" select="../TestClass/@name"/>
									<xsl:with-param name="AbstractTypeName" select="substring-before(@name, 'Core')"/>
									<xsl:with-param name="Properties" select="Property[not(@name = 'Context')][substring-before(@name, 'Collection') = '']"/>
									<xsl:with-param name="OilInfo" select="$OilInfo"/>
								</xsl:call-template>

								<!--
							//
							// actionTabs
							//
							-->
								<plx:comment></plx:comment>
								<plx:comment>actionTabs</plx:comment>
								<plx:comment></plx:comment>
								<plx:assign>
									<plx:left>
										<plx:callThis name="actionTabs" type="field"/>
									</plx:left>
									<plx:right>
										<plx:callNew dataTypeName="TabControl" type="new" dataTypeQualifier="System.Windows.Forms"/>
									</plx:right>
								</plx:assign>

								<plx:assign>
									<plx:left>
										<plx:callInstance name="Dock" type="property">
											<plx:callObject>
												<plx:callThis name="actionTabs" type="field"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:callStatic name="Fill" dataTypeName="DockStyle" dataTypeQualifier="System.Windows.Forms" type="property"/>
									</plx:right>
								</plx:assign>

								<plx:assign>
									<plx:left>
										<plx:callInstance name="Size" type="property">
											<plx:callObject>
												<plx:callThis name="actionTabs" type="field" accessor="this"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:callNew dataTypeName="Size" dataTypeQualifier="System.Drawing">
											<plx:passParam>
												<plx:value data="530" type="i4"/>
											</plx:passParam>
											<plx:passParam>
												<plx:value data="485" type="i4"/>
											</plx:passParam>
										</plx:callNew>
									</plx:right>
								</plx:assign>

								<plx:assign>
									<plx:left>
										<plx:callInstance name="Location" type="property">
											<plx:callObject>
												<plx:callThis name="actionTabs" type="field" accessor="this"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:callNew dataTypeName="Point" dataTypeQualifier="System.Drawing">
											<plx:passParam>
												<plx:value data="0" type="i4"/>
											</plx:passParam>
											<plx:passParam>
												<plx:value data="0" type="i4"/>
											</plx:passParam>
										</plx:callNew>
									</plx:right>
								</plx:assign>

								<plx:assign>
									<plx:left>
										<plx:callInstance name="Name" type="property">
											<plx:callObject>
												<plx:callThis name="actionTabs" type="field" accessor="this"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:string>actionTabs</plx:string>
									</plx:right>
								</plx:assign>

								<xsl:call-template name="AddActionTabsTab">
									<xsl:with-param name="ActionType" select="'Collection'"/>
									<xsl:with-param name="Name" select="@name"/>
								</xsl:call-template>

								<xsl:call-template name="AddActionTabsTab">
									<xsl:with-param name="ActionType" select="'Create'"/>
									<xsl:with-param name="Name" select="@name"/>
								</xsl:call-template>

								<xsl:call-template name="AddActionTabsTab">
									<xsl:with-param name="ActionType" select="'Select'"/>
									<xsl:with-param name="Name" select="@name"/>
								</xsl:call-template>

								<!--
							//
							// input control
							//
							-->
								<plx:comment></plx:comment>
								<plx:comment>this</plx:comment>
								<plx:comment></plx:comment>
								<plx:callInstance name="Add" type="methodCall">
									<plx:callObject>
										<plx:callThis name="Controls" type="property" accessor="this"/>
									</plx:callObject>
									<plx:passParam>
										<plx:nameRef name="actionTabs"/>
									</plx:passParam>
								</plx:callInstance>

								<plx:assign>
									<plx:left>
										<plx:callThis name="Size" type="property" accessor="this"/>
									</plx:left>
									<plx:right>
										<plx:callNew dataTypeName="Size" dataTypeQualifier="System.Drawing">
											<plx:passParam>
												<plx:value data="540" type="i4"/>
											</plx:passParam>
											<plx:passParam>
												<plx:value data="500" type="i4"/>
											</plx:passParam>
										</plx:callNew>
									</plx:right>
								</plx:assign>
							</plx:function>

						</plx:class>

						<!--
					
					Create Class
					
					-->
						<!-- This custom control allows you to create an instance of the entity type in the database. -->
						<plx:class name="Create_{@name}_{$ClassName}" partial="true" visibility="public">
							<plx:leadingInfo>
								<plx:pragma type="region" data="Create_{@name}_{$ClassName}"/>
							</plx:leadingInfo>
							<plx:trailingInfo>
								<plx:pragma type="closeRegion" data="Create_{@name}_{$ClassName}"/>
							</plx:trailingInfo>

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

							<xsl:call-template name="DeclareCommonFields">
								<xsl:with-param name="ActionType" select="'Create'"/>
								<xsl:with-param name="ModelName" select="$ModelName"/>
								<xsl:with-param name="TestClassName" select="../TestClass/@name"/>
								<xsl:with-param name="AbstractTypeName" select="substring-before(@name, 'Core')"/>
							</xsl:call-template>

							<plx:function name="Dispose" modifier="override" overload="true" visibility="protected">
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
							</plx:function>

							<plx:function name="InitializeComponent" visibility="private">
								<plx:leadingInfo>
									<plx:pragma type="region" data="InitializeComponent method"/>
									<plx:docComment>
										<summary>Required method for Designer support - do not modify the contents of this method with the code editor.</summary>
									</plx:docComment>
								</plx:leadingInfo>
								<plx:trailingInfo>
									<plx:pragma type="closeRegion" data="InitializeComponent method"/>
								</plx:trailingInfo>

								<plx:assign>
									<plx:left>
										<plx:callThis name="components" type="field"/>
									</plx:left>
									<plx:right>
										<plx:callNew dataTypeName="Container" dataTypeQualifier="System.ComponentModel" type="new"/>
									</plx:right>
								</plx:assign>

								<xsl:call-template name="InitializeCommonFields">
									<xsl:with-param name="ActionType" select="'Create'"/>
									<xsl:with-param name="ModelName" select="$ModelName"/>
									<xsl:with-param name="TestClassName" select="../TestClass/@name"/>
									<xsl:with-param name="AbstractTypeName" select="substring-before(@name, 'Core')"/>
									<xsl:with-param name="Properties" select="Property[not(@name = 'Context')][substring-before(@name, 'Collection') = '']"/>
									<xsl:with-param name="OilInfo" select="$OilInfo"/>
								</xsl:call-template>
							</plx:function>

						</plx:class>

						<!--
					
					Collection Class
					
					-->
						<!-- This custom control allows you to view all instances of the entity type in the database. -->
						<plx:class name="Collection_{@name}_{$ClassName}" partial="true" visibility="public">
							<plx:leadingInfo>
								<plx:pragma type="region" data="Collection_{@name}_{$ClassName}"/>
							</plx:leadingInfo>
							<plx:trailingInfo>
								<plx:pragma type="closeRegion" data="Collection_{@name}_{$ClassName}"/>
							</plx:trailingInfo>

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

							<xsl:call-template name="DeclareCommonFields">
								<xsl:with-param name="ActionType" select="'Collection'"/>
								<xsl:with-param name="ModelName" select="$ModelName"/>
								<xsl:with-param name="TestClassName" select="../TestClass/@name"/>
								<xsl:with-param name="AbstractTypeName" select="substring-before(@name, 'Core')"/>
							</xsl:call-template>

							<plx:function name="Dispose" modifier="override" overload="true" visibility="protected">
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
							</plx:function>

							<plx:function name="InitializeComponent" visibility="private">
								<plx:leadingInfo>
									<plx:pragma type="region" data="InitializeComponent method"/>
									<plx:docComment>
										<summary>Required method for Designer support - do not modify the contents of this method with the code editor.</summary>
									</plx:docComment>
								</plx:leadingInfo>
								<plx:trailingInfo>
									<plx:pragma type="closeRegion" data="InitializeComponent method"/>
								</plx:trailingInfo>

								<plx:assign>
									<plx:left>
										<plx:callThis name="components" type="field" accessor="this"/>
									</plx:left>
									<plx:right>
										<plx:callNew dataTypeName="Container" dataTypeQualifier="System.ComponentModel" type="new"/>
									</plx:right>
								</plx:assign>

								<xsl:call-template name="InitializeCommonFields">
									<xsl:with-param name="ActionType" select="'Collection'"/>
									<xsl:with-param name="ModelName" select="$ModelName"/>
									<xsl:with-param name="TestClassName" select="../TestClass/@name"/>
									<xsl:with-param name="AbstractTypeName" select="substring-before(@name, 'Core')"/>
									<xsl:with-param name="Properties" select="Property[not(@name = 'Context')][substring-before(@name, 'Collection') = '']"/>
									<xsl:with-param name="OilInfo" select="$OilInfo"/>
								</xsl:call-template>
							</plx:function>

						</plx:class>

						<!--
					
					Select Class
					
					-->
						<!-- This custom control allows you to select an instance of the entity type in the database and modify it. -->
						<plx:class name="Select_{@name}_{$ClassName}" partial="true" visibility="public">
							<plx:leadingInfo>
								<plx:pragma type="region" data="Select{@name}_{$ClassName}"/>
							</plx:leadingInfo>
							<plx:trailingInfo>
								<plx:pragma type="closeRegion" data="Select_{@name}_{$ClassName}"/>
							</plx:trailingInfo>


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

							<plx:field name="lblSelectionMode" visibility="private" dataTypeName="Label" dataTypeQualifier="System.Windows.Forms"/>
							<plx:field name="cbxSelectionMode" visibility="private" dataTypeName="ComboBox" dataTypeQualifier="System.Windows.Forms"/>
							<plx:field name="lblCurrentObject" visibility="private" dataTypeName="Label" dataTypeQualifier="System.Windows.Forms"/>
							<plx:field name="dgvCurrentObject" visibility="private" dataTypeName="DataGridView" dataTypeQualifier="System.Windows.Forms"/>
							<plx:field name="btnSave" visibility="private" dataTypeName="Button" dataTypeQualifier="System.Windows.Forms"/>
							<plx:field name="btnCancel" visibility="private" dataTypeName="Button" dataTypeQualifier="System.Windows.Forms"/>
							<plx:field name="pnlSave" visibility="private" dataTypeName="Panel" dataTypeQualifier="System.Windows.Forms"/>
							<plx:field name="editMode" visibility="private" dataTypeName=".boolean"/>
							<plx:field name="lblNeedToSave" visibility="private" dataTypeName="Label" dataTypeQualifier="System.Windows.Forms"/>

							<xsl:call-template name="DeclareCommonFields">
								<xsl:with-param name="ActionType" select="'Select'"/>
								<xsl:with-param name="ModelName" select="$ModelName"/>
								<xsl:with-param name="TestClassName" select="../TestClass/@name"/>
								<xsl:with-param name="AbstractTypeName" select="substring-before(@name, 'Core')"/>
							</xsl:call-template>

							<plx:function name="Dispose" modifier="override" overload="true" visibility="protected">
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
							</plx:function>

							<plx:function name="InitializeComponent" visibility="private">
								<plx:leadingInfo>
									<plx:pragma type="region" data="InitializeComponent method"/>
									<plx:docComment>
										<summary>Required method for Designer support - do not modify the contents of this method with the code editor.</summary>
									</plx:docComment>
								</plx:leadingInfo>
								<plx:trailingInfo>
									<plx:pragma type="closeRegion" data="InitializeComponent method"/>
								</plx:trailingInfo>

								<!--
							//
							// Data other than controls
							//
							-->
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
										<plx:callThis name="editMode" type="field"/>
									</plx:left>
									<plx:right>
										<plx:falseKeyword/>
									</plx:right>
								</plx:assign>

								<!--
							// 
							// lblNeedToSave
							// 
							-->
								<plx:comment></plx:comment>
								<plx:comment>lblNeedToSave</plx:comment>
								<plx:comment></plx:comment>
								<plx:assign>
									<plx:left>
										<plx:callThis name="lblNeedToSave" accessor="this" type="field"/>
									</plx:left>
									<plx:right>
										<plx:callNew dataTypeName="Label" dataTypeQualifier="System.Windows.Forms"/>
									</plx:right>
								</plx:assign>

								<plx:assign>
									<plx:left>
										<plx:callInstance name="Location" type="field">
											<plx:callObject>
												<plx:callThis name="lblNeedToSave" type="field" accessor="this"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:callNew dataTypeName="Point" dataTypeQualifier="System.Drawing">
											<plx:passParam>
												<plx:value data="0" type="i4"/>
											</plx:passParam>
											<plx:passParam>
												<plx:value data="10" type="i4"/>
											</plx:passParam>
										</plx:callNew>
									</plx:right>
								</plx:assign>

								<plx:assign>
									<plx:left>
										<plx:callInstance name="Name" type="field">
											<plx:callObject>
												<plx:callThis name="lblNeedToSave" type="field" accessor="this"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:string>lblNeedToSave</plx:string>
									</plx:right>
								</plx:assign>

								<plx:assign>
									<plx:left>
										<plx:callInstance name="Size" type="field">
											<plx:callObject>
												<plx:callThis name="lblNeedToSave" type="field" accessor="this"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:callNew dataTypeName="Size" dataTypeQualifier="System.Drawing">
											<plx:passParam>
												<plx:value data="200" type="i4"/>
											</plx:passParam>
											<plx:passParam>
												<plx:value data="15" type="i4"/>
											</plx:passParam>
										</plx:callNew>
									</plx:right>
								</plx:assign>

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

								<!--
							// 
							// btnCancel
							// 
							-->
								<plx:comment></plx:comment>
								<plx:comment>btnCancel</plx:comment>
								<plx:comment></plx:comment>
								<plx:assign>
									<plx:left>
										<plx:callThis name="btnCancel" accessor="this" type="field"/>
									</plx:left>
									<plx:right>
										<plx:callNew dataTypeName="Button" dataTypeQualifier="System.Windows.Forms"/>
									</plx:right>
								</plx:assign>

								<plx:assign>
									<plx:left>
										<plx:callInstance name="Location" type="field">
											<plx:callObject>
												<plx:callThis name="btnCancel" type="field" accessor="this"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:callNew dataTypeName="Point" dataTypeQualifier="System.Drawing">
											<plx:passParam>
												<plx:value data="300" type="i4"/>
											</plx:passParam>
											<plx:passParam>
												<plx:value data="10" type="i4"/>
											</plx:passParam>
										</plx:callNew>
									</plx:right>
								</plx:assign>

								<plx:assign>
									<plx:left>
										<plx:callInstance name="Name" type="field">
											<plx:callObject>
												<plx:callThis name="btnCancel" type="field" accessor="this"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:string data="btnCancel"/>
									</plx:right>
								</plx:assign>

								<plx:assign>
									<plx:left>
										<plx:callInstance name="TabIndex" type="field">
											<plx:callObject>
												<plx:callThis name="btnCancel" type="field" accessor="this"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:value data="6" type="i4"/>
									</plx:right>
								</plx:assign>

								<plx:assign>
									<plx:left>
										<plx:callInstance name="Text" type="field">
											<plx:callObject>
												<plx:callThis name="btnCancel" type="field" accessor="this"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:string data="Cancel"/>
									</plx:right>
								</plx:assign>

								<plx:assign>
									<plx:left>
										<plx:callInstance name="UseVisualStyleBackColor" type="field">
											<plx:callObject>
												<plx:callThis name="btnCancel" type="field" accessor="this"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:trueKeyword/>
									</plx:right>
								</plx:assign>

								<plx:attachEvent>
									<plx:left>
										<plx:callInstance name="Click" type="event">
											<plx:callObject>
												<plx:callThis name="btnCancel" type="field" accessor="this"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:callNew dataTypeName="EventHandler" dataTypeQualifier="System" type="new">
											<plx:passParam>
												<plx:callThis name="btnCancel_Click" type="methodReference"/>
											</plx:passParam>
										</plx:callNew>
									</plx:right>
								</plx:attachEvent>

								<!--
							// 
							// btnSave
							// 
							-->
								<plx:comment></plx:comment>
								<plx:comment>btnSave</plx:comment>
								<plx:comment></plx:comment>
								<plx:assign>
									<plx:left>
										<plx:callThis name="btnSave" accessor="this" type="field"/>
									</plx:left>
									<plx:right>
										<plx:callNew dataTypeName="Button" dataTypeQualifier="System.Windows.Forms"/>
									</plx:right>
								</plx:assign>

								<plx:assign>
									<plx:left>
										<plx:callInstance name="Location" type="field">
											<plx:callObject>
												<plx:callThis name="btnSave" type="field" accessor="this"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:callNew dataTypeName="Point" dataTypeQualifier="System.Drawing">
											<plx:passParam>
												<plx:value data="400" type="i4"/>
											</plx:passParam>
											<plx:passParam>
												<plx:value data="10" type="i4"/>
											</plx:passParam>
										</plx:callNew>
									</plx:right>
								</plx:assign>

								<plx:assign>
									<plx:left>
										<plx:callInstance name="Name" type="field">
											<plx:callObject>
												<plx:callThis name="btnSave" type="field" accessor="this"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:string data="btnSave"/>
									</plx:right>
								</plx:assign>

								<plx:assign>
									<plx:left>
										<plx:callInstance name="TabIndex" type="field">
											<plx:callObject>
												<plx:callThis name="btnSave" type="field" accessor="this"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:value data="7" type="i4"/>
									</plx:right>
								</plx:assign>

								<plx:assign>
									<plx:left>
										<plx:callInstance name="Text" type="field">
											<plx:callObject>
												<plx:callThis name="btnSave" type="field" accessor="this"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:string data="Save"/>
									</plx:right>
								</plx:assign>

								<plx:assign>
									<plx:left>
										<plx:callInstance name="UseVisualStyleBackColor" type="field">
											<plx:callObject>
												<plx:callThis name="btnSave" type="field" accessor="this"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:trueKeyword/>
									</plx:right>
								</plx:assign>

								<plx:attachEvent>
									<plx:left>
										<plx:callInstance name="Click" type="event">
											<plx:callObject>
												<plx:callThis name="btnSave" type="field" accessor="this"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:callNew dataTypeName="EventHandler" dataTypeQualifier="System" type="new">
											<plx:passParam>
												<plx:callThis name="btnSave_Click" type="methodReference"/>
											</plx:passParam>
										</plx:callNew>
									</plx:right>
								</plx:attachEvent>

								<!--
							//
							//pnlSave
							//
							-->
								<plx:comment></plx:comment>
								<plx:comment>pnlSave</plx:comment>
								<plx:comment></plx:comment>
								<plx:assign>
									<plx:left>
										<plx:callThis name="pnlSave" accessor="this" type="field"/>
									</plx:left>
									<plx:right>
										<plx:callNew dataTypeName="Panel" dataTypeQualifier="System.Windows.Forms"/>
									</plx:right>
								</plx:assign>

								<plx:assign>
									<plx:left>
										<plx:callInstance name="Dock" type="property">
											<plx:callObject>
												<plx:callThis name="pnlSave" accessor="this" type="field"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:callStatic name="Top" dataTypeName="DockStyle" dataTypeQualifier="System.Windows.Forms" type="property"/>
									</plx:right>
								</plx:assign>

								<plx:assign>
									<plx:left>
										<plx:callInstance name="AutoSize" type="field">
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
										<plx:callInstance name="Location" type="field">
											<plx:callObject>
												<plx:callThis name="pnlSave" type="field" accessor="this"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>

									<plx:right>
										<plx:callNew dataTypeName="Point" dataTypeQualifier="System.Drawing">
											<plx:passParam>
												<plx:value data="0" type="i4"/>
											</plx:passParam>
											<plx:passParam>
												<plx:value data="0" type="i4"/>
											</plx:passParam>
										</plx:callNew>
									</plx:right>
								</plx:assign>

								<plx:assign>
									<plx:left>
										<plx:callInstance name="Name" type="field">
											<plx:callObject>
												<plx:callThis name="pnlSave" type="field" accessor="this"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:string data="pnlSave"/>
									</plx:right>
								</plx:assign>

								<plx:assign>
									<plx:left>
										<plx:callInstance name="AutoScroll" type="field">
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
										<plx:callInstance name="TabIndex" type="field">
											<plx:callObject>
												<plx:callThis name="pnlSave" type="field" accessor="this"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:value data="5" type="i4"/>
									</plx:right>
								</plx:assign>

								<plx:callInstance name="Add" type="methodCall">
									<plx:callObject>
										<plx:callInstance name="Controls" type="property">
											<plx:callObject>
												<plx:callThis name="pnlSave" type="field" accessor="this"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:callObject>
									<plx:passParam>
										<plx:callThis name="btnSave" type="field" accessor="this"/>
									</plx:passParam>
								</plx:callInstance>

								<plx:callInstance name="Add" type="methodCall">
									<plx:callObject>
										<plx:callInstance name="Controls" type="property">
											<plx:callObject>
												<plx:callThis name="pnlSave" type="field" accessor="this"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:callObject>
									<plx:passParam>
										<plx:callThis name="btnCancel" type="field" accessor="this"/>
									</plx:passParam>
								</plx:callInstance>

								<plx:callInstance name="Add" type="methodCall">
									<plx:callObject>
										<plx:callInstance name="Controls" type="property">
											<plx:callObject>
												<plx:callThis name="pnlSave" type="field" accessor="this"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:callObject>
									<plx:passParam>
										<plx:callThis name="lblNeedToSave" type="field" accessor="this"/>
									</plx:passParam>
								</plx:callInstance>

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

								<!--
							// 
							// lblSelectionMode
							// 
							-->
								<plx:comment></plx:comment>
								<plx:comment>lblSelectionMode</plx:comment>
								<plx:comment></plx:comment>
								<plx:assign>
									<plx:left>
										<plx:callThis name="lblSelectionMode" accessor="this" type="field"/>
									</plx:left>
									<plx:right>
										<plx:callNew dataTypeName="Label" dataTypeQualifier="System.Windows.Forms"/>
									</plx:right>
								</plx:assign>

								<plx:assign>
									<plx:left>
										<plx:callInstance name="Location" type="field">
											<plx:callObject>
												<plx:callThis name="lblSelectionMode" type="field" accessor="this"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:callNew dataTypeName="Point" dataTypeQualifier="System.Drawing">
											<plx:passParam>
												<plx:value data="0" type="i4"/>
											</plx:passParam>
											<plx:passParam>
												<plx:value data="10" type="i4"/>
											</plx:passParam>
										</plx:callNew>
									</plx:right>
								</plx:assign>

								<plx:assign>
									<plx:left>
										<plx:callInstance name="Name" type="field">
											<plx:callObject>
												<plx:callThis name="lblSelectionMode" type="field" accessor="this"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:string data="lblSelectionMode"/>
									</plx:right>
								</plx:assign>

								<plx:assign>
									<plx:left>
										<plx:callInstance name="Text" type="field">
											<plx:callObject>
												<plx:callThis name="lblSelectionMode" type="field" accessor="this"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:string>SelectionMode:</plx:string>
									</plx:right>
								</plx:assign>

								<!--
							//
							//cbxSelectionMode
							//
							-->
								<plx:comment></plx:comment>
								<plx:comment>cbxSelectionMode</plx:comment>
								<plx:comment></plx:comment>
								<plx:assign>
									<plx:left>
										<plx:callThis name="cbxSelectionMode" accessor="this" type="field"/>
									</plx:left>
									<plx:right>
										<plx:callNew dataTypeName="ComboBox" dataTypeQualifier="System.Windows.Forms"/>
									</plx:right>
								</plx:assign>

								<plx:assign>
									<plx:left>
										<plx:callInstance name="Location" type="field">
											<plx:callObject>
												<plx:callThis name="cbxSelectionMode" type="field" accessor="this"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:callNew dataTypeName="Point" dataTypeQualifier="System.Drawing">
											<plx:passParam>
												<plx:value data="100" type="i4"/>
											</plx:passParam>
											<plx:passParam>
												<plx:value data="10" type="i4"/>
											</plx:passParam>
										</plx:callNew>
									</plx:right>
								</plx:assign>

								<plx:assign>
									<plx:left>
										<plx:callInstance name="Name" type="field">
											<plx:callObject>
												<plx:callThis name="cbxSelectionMode" type="field" accessor="this"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:string data="cbxSelectionMode"/>
									</plx:right>
								</plx:assign>

								<plx:assign>
									<plx:left>
										<plx:callInstance name="Size" type="field">
											<plx:callObject>
												<plx:callThis name="cbxSelectionMode" type="field" accessor="this"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:callNew dataTypeName="Size" dataTypeQualifier="System.Drawing">
											<plx:passParam>
												<plx:value data="200" type="i4"/>
											</plx:passParam>
											<plx:passParam>
												<plx:value data="15" type="i4"/>
											</plx:passParam>
										</plx:callNew>
									</plx:right>
								</plx:assign>

								<plx:assign>
									<plx:left>
										<plx:callInstance name="DropDownStyle" type="field">
											<plx:callObject>
												<plx:callThis name="cbxSelectionMode" type="field" accessor="this"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:callStatic name="DropDownList" dataTypeName="ComboBoxStyle" dataTypeQualifier="System.Windows.Forms" type="property"/>
									</plx:right>
								</plx:assign>

								<plx:assign>
									<plx:left>
										<plx:callInstance name="TabIndex" type="field">
											<plx:callObject>
												<plx:callThis name="cbxSelectionMode" type="field" accessor="this"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:value data="2" type="i4"/>
									</plx:right>
								</plx:assign>

								<plx:attachEvent>
									<plx:left>
										<plx:callInstance name="SelectedIndexChanged" type="event">
											<plx:callObject>
												<plx:callThis name="cbxSelectionMode" type="field" accessor="this"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:callNew dataTypeName="EventHandler" dataTypeQualifier="System" type="new">
											<plx:passParam>
												<plx:callThis name="cbxSelectionMode_SelectedIndexChanged" type="methodReference"/>
											</plx:passParam>
										</plx:callNew>
									</plx:right>
								</plx:attachEvent>

								<xsl:for-each select="../TestClass/Actions/Select/Function[current()/@displayName = @objectToSelect]">
									<xsl:sort select="@selectionMode"/>
									<plx:callInstance name="Add" type="methodCall">
										<plx:callObject>
											<plx:callInstance name="Items" type="property">
												<plx:callObject>
													<plx:callThis name="cbxSelectionMode" type="field" accessor="this"/>
												</plx:callObject>
											</plx:callInstance>
										</plx:callObject>
										<plx:passParam>
											<plx:string data="{@selectionMode}"/>
										</plx:passParam>
									</plx:callInstance>
								</xsl:for-each>

								<!--
							// 
							// lblCurrentObject
							// 
							-->
								<plx:comment></plx:comment>
								<plx:comment>lblCurrentObject</plx:comment>
								<plx:comment></plx:comment>
								<plx:assign>
									<plx:left>
										<plx:callThis name="lblCurrentObject" accessor="this" type="field"/>
									</plx:left>
									<plx:right>
										<plx:callNew dataTypeName="Label" dataTypeQualifier="System.Windows.Forms"/>
									</plx:right>
								</plx:assign>

								<plx:assign>
									<plx:left>
										<plx:callInstance name="Location" type="field">
											<plx:callObject>
												<plx:callThis name="lblCurrentObject" type="field" accessor="this"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:callNew dataTypeName="Point" dataTypeQualifier="System.Drawing">
											<plx:passParam>
												<plx:value data="0" type="i4"/>
											</plx:passParam>
											<plx:passParam>
												<plx:value data="45" type="i4"/>
											</plx:passParam>
										</plx:callNew>
									</plx:right>
								</plx:assign>

								<plx:assign>
									<plx:left>
										<plx:callInstance name="Width" type="property">
											<plx:callObject>
												<plx:callThis name="lblCurrentObject" type="field" accessor="this"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:value data="300" type="i4"/>
									</plx:right>
								</plx:assign>

								<plx:assign>
									<plx:left>
										<plx:callInstance name="Name" type="field">
											<plx:callObject>
												<plx:callThis name="lblCurrentObject" type="field" accessor="this"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:string data="lblCurrentObject"/>
									</plx:right>
								</plx:assign>

								<plx:assign>
									<plx:left>
										<plx:callInstance name="Text" type="field">
											<plx:callObject>
												<plx:callThis name="lblCurrentObject" type="field" accessor="this"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:string data="There is no selected {@displayName}."/>
									</plx:right>
								</plx:assign>

								<!--
							// 
							// dgvCurrentObject
							// 
							-->
								<plx:comment></plx:comment>
								<plx:comment>dgvCurrentObject</plx:comment>
								<plx:comment></plx:comment>
								<plx:assign>
									<plx:left>
										<plx:callThis name="dgvCurrentObject" accessor="this" type="field"/>
									</plx:left>
									<plx:right>
										<plx:callNew dataTypeName="DataGridView" dataTypeQualifier="System.Windows.Forms"/>
									</plx:right>
								</plx:assign>

								<plx:assign>
									<plx:left>
										<plx:callInstance name="Dock" type="property">
											<plx:callObject>
												<plx:callThis name="dgvCurrentObject" accessor="this" type="field"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:callStatic name="Top" dataTypeName="DockStyle" dataTypeQualifier="System.Windows.Forms" type="field"/>
									</plx:right>
								</plx:assign>

								<plx:assign>
									<plx:left>
										<plx:callInstance name="ColumnHeadersHeightSizeMode" type="field">
											<plx:callObject>
												<plx:callThis name="dgvCurrentObject" type="field" accessor="this"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:callStatic name="AutoSize" dataTypeName="DataGridViewColumnHeadersHeightSizeMode" dataTypeQualifier="System.Windows.Forms" type="field"/>
									</plx:right>
								</plx:assign>

								<plx:assign>
									<plx:left>
										<plx:callInstance name="Location" type="field">
											<plx:callObject>
												<plx:callThis name="dgvCurrentObject" type="field" accessor="this"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:callNew dataTypeName="Point" dataTypeQualifier="System.Drawing">
											<plx:passParam>
												<plx:value data="0" type="i4"/>
											</plx:passParam>
											<plx:passParam>
												<plx:value data="265" type="i4"/>
											</plx:passParam>
										</plx:callNew>
									</plx:right>
								</plx:assign>

								<plx:assign>
									<plx:left>
										<plx:callInstance name="Name" type="field">
											<plx:callObject>
												<plx:callThis name="dgvCurrentObject" type="field" accessor="this"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:string>dgvCurrentObject</plx:string>
									</plx:right>
								</plx:assign>

								<plx:assign>
									<plx:left>
										<plx:callInstance name="Size" type="field">
											<plx:callObject>
												<plx:callThis name="dgvCurrentObject" type="field" accessor="this"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:callNew dataTypeName="Size" dataTypeQualifier="System.Drawing">
											<plx:passParam>
												<plx:value data="500" type="i4"/>
											</plx:passParam>
											<plx:passParam>
												<plx:value data="150" type="i4"/>
											</plx:passParam>
										</plx:callNew>
									</plx:right>
								</plx:assign>

								<plx:assign>
									<plx:left>
										<plx:callInstance name="TabIndex" type="field">
											<plx:callObject>
												<plx:callThis name="dgvCurrentObject" type="field" accessor="this"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:value data="4" type="i4"/>
									</plx:right>
								</plx:assign>

								<xsl:for-each select="Property[not(@name = 'Context')][substring-before(@name, 'Collection') = '']">
									<plx:callInstance name="Add" type="methodCall">
										<plx:callObject>
											<plx:callInstance name="Columns" type="property">
												<plx:callObject>
													<plx:callInstance name="dgvCurrentObject" type="field">
														<plx:callObject>
															<plx:thisKeyword/>
														</plx:callObject>
													</plx:callInstance>
												</plx:callObject>
											</plx:callInstance>
										</plx:callObject>
										<xsl:choose>
											<xsl:when test="@displayName">
												<plx:passParam>
													<plx:string data="{@displayName}"/>
												</plx:passParam>
												<plx:passParam>
													<plx:string data="{@displayName}"/>
												</plx:passParam>
											</xsl:when>
											<xsl:otherwise>
												<plx:passParam>
													<plx:string data="{@name}"/>
												</plx:passParam>
												<plx:passParam>
													<plx:string data="{@name}"/>
												</plx:passParam>
											</xsl:otherwise>
										</xsl:choose>
									</plx:callInstance>

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
											<plx:falseKeyword/>
										</plx:right>
									</plx:assign>
								</xsl:for-each>

								<plx:assign>
									<plx:left>
										<plx:callInstance name="Visible" type="field">
											<plx:callObject>
												<plx:callThis name="dgvCurrentObject" type="field" accessor="this"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:falseKeyword/>
									</plx:right>
								</plx:assign>

								<xsl:for-each select="Property[@autoIncrement = 'true'][not(@name = 'Context')][substring-before(@name, 'Collection') = '']">
									<plx:assign>
										<plx:left>
											<plx:callInstance name="ReadOnly" type="property">
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

								<plx:attachEvent>
									<plx:left>
										<plx:callInstance name="CellBeginEdit" type="event">
											<plx:callObject>
												<plx:callThis name="dgvCurrentObject" type="field" accessor="this"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:callNew dataTypeName="DataGridViewCellCancelEventHandler" dataTypeQualifier="System.Windows.Forms" type="new">
											<plx:passParam>
												<plx:callThis name="dgvCurrentObject_CellBeginEdit" type="methodReference"/>
											</plx:passParam>
										</plx:callNew>
									</plx:right>
								</plx:attachEvent>

								<plx:callInstance name="Add" type="methodCall">
									<plx:callObject>
										<plx:callThis name="Controls" type="property" accessor="this"/>
									</plx:callObject>
									<plx:passParam>
										<plx:callThis name="pnlSave" type="field" accessor="this"/>
									</plx:passParam>
								</plx:callInstance>

								<plx:callInstance name="Add" type="methodCall">
									<plx:callObject>
										<plx:callThis name="Controls" type="property" accessor="this"/>
									</plx:callObject>
									<plx:passParam>
										<plx:callThis name="dgvCurrentObject" type="field" accessor="this"/>
									</plx:passParam>
								</plx:callInstance>

								<xsl:call-template name="InitializeCommonFields">
									<xsl:with-param name="ActionType" select="'Select'"/>
									<xsl:with-param name="ModelName" select="$ModelName"/>
									<xsl:with-param name="TestClassName" select="../TestClass/@name"/>
									<xsl:with-param name="AbstractTypeName" select="substring-before(@name, 'Core')"/>
									<xsl:with-param name="Properties" select="Property[not(@name = 'Context')][substring-before(@name, 'Collection') = '']"/>
									<xsl:with-param name="OilInfo" select="$OilInfo"/>
								</xsl:call-template>

								<plx:assign>
									<plx:left>
										<plx:callInstance name="Height" type="property">
											<plx:callObject>
												<plx:callThis name="dgvCurrentObject" type="field" accessor="this"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:callInstance name="Height" type="property">
											<plx:callObject>
												<plx:callThis name="dgvSelect" type="field" accessor="this"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:right>
								</plx:assign>

								<!--
							//
							// Add constrols specific to the select and edit manipulations to pnlDisplay
							//
							-->
								<plx:callInstance name="Add" type="methodCall">
									<plx:callObject>
										<plx:callInstance name="Controls" type="property">
											<plx:callObject>
												<plx:callThis name="pnlDisplay" type="field" accessor="this"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:callObject>
									<plx:passParam>
										<plx:callThis name="lblCurrentObject" type="field" accessor="this"/>
									</plx:passParam>
								</plx:callInstance>

								<plx:callInstance name="Add" type="methodCall">
									<plx:callObject>
										<plx:callInstance name="Controls" type="property">
											<plx:callObject>
												<plx:callThis name="pnlDisplay" type="field" accessor="this"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:callObject>
									<plx:passParam>
										<plx:callThis name="cbxSelectionMode" type="field" accessor="this"/>
									</plx:passParam>
								</plx:callInstance>

								<plx:callInstance name="Add" type="methodCall">
									<plx:callObject>
										<plx:callInstance name="Controls" type="property">
											<plx:callObject>
												<plx:callThis name="pnlDisplay" type="field" accessor="this"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:callObject>
									<plx:passParam>
										<plx:callThis name="lblSelectionMode" type="field" accessor="this"/>
									</plx:passParam>
								</plx:callInstance>

								<plx:comment blankLine="true"/>

								<plx:branch>
									<plx:condition>
										<plx:binaryOperator type="greaterThan">
											<plx:left>
												<plx:callInstance name="Count" type="property">
													<plx:callObject>
														<plx:callInstance name="Items" type="property">
															<plx:callObject>
																<plx:callThis name="cbxSelectionMode" type="field" accessor="this"/>
															</plx:callObject>
														</plx:callInstance>
													</plx:callObject>
												</plx:callInstance>
											</plx:left>
											<plx:right>
												<plx:value data="0" type="i4"/>
											</plx:right>
										</plx:binaryOperator>
									</plx:condition>
									<plx:assign>
										<plx:left>
											<plx:callInstance name="SelectedIndex" type="property">
												<plx:callObject>
													<plx:callThis name="cbxSelectionMode" type="field" accessor="this"/>
												</plx:callObject>
											</plx:callInstance>
										</plx:left>
										<plx:right>
											<plx:value data="0" type="i4"/>
										</plx:right>
									</plx:assign>
								</plx:branch>
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

	<!-- Declare fields needed in each custom control.  Can filter by ActionType. -->
	<xsl:template name="DeclareCommonFields">
		<xsl:param name="ActionType"/>
		<!-- The type of manipulation you are doing.  Do not use if the control is not doing any manipulation. - Create, Select, Collection -->
		<xsl:param name="ModelName"/>
		<!-- The name of the ORM model -->
		<xsl:param name="TestClassName"/>
		<!-- The name of the test class.  It will end with 'Context'. -->
		<xsl:param name="AbstractTypeName"/>
		<!-- The name of the abstract entity type to manipulate -->

		<xsl:choose>
			<xsl:when test="$ActionType">
				<plx:field name="lbl{$ActionType}" visibility="private" dataTypeName="Label" dataTypeQualifier="System.Windows.Forms"/>
				<plx:field name="btn{$ActionType}" visibility="private" dataTypeName="Button" dataTypeQualifier="System.Windows.Forms"/>
				<plx:field name="dgv{$ActionType}" visibility="private" dataTypeName="DataGridView" dataTypeQualifier="System.Windows.Forms"/>
				<plx:field name="pnlDisplay" visibility="private" dataTypeName="Panel" dataTypeQualifier="System.Windows.Forms"/>
				<plx:field name="{$ActionType}_{$AbstractTypeName}_connect" visibility="private" dataTypeName="ConnectionDelegate" dataTypeQualifier="{$TestClassName}"/>
				<plx:field name="testVar" visibility="private" dataTypeName="I{$TestClassName}"/>
				<plx:field name="abstractTypeVar" visibility="private" dataTypeName="{$AbstractTypeName}"/>
			</xsl:when>
		</xsl:choose>
	</xsl:template>

	<!-- Initialize fields needed in each custom control.  Can filter by ActionType. -->
	<xsl:template name="InitializeCommonFields">
		<xsl:param name="ActionType"/>
		<!-- The type of manipulation you are doing.  Do not use if the control is not doing any manipulation. - Create, Select, Collection -->
		<xsl:param name="ModelName"/>
		<!-- The name of the ORM model -->
		<xsl:param name="TestClassName"/>
		<!-- The name of the test class.  It will end with 'Context'. -->
		<xsl:param name="AbstractTypeName"/>
		<!-- The name of the abstract entity type to manipulate -->
		<xsl:param name="Properties"/>
		<!-- The properties of the abstract entity type -->
		<xsl:param name="OilInfo"/>
		<!-- OilInfo var -->

		<xsl:choose>
			<xsl:when test="$ActionType">
				<!--
				//
				//connect
				//
				-->
				<plx:comment></plx:comment>
				<plx:comment>connect</plx:comment>
				<plx:comment></plx:comment>
				<plx:assign>
					<plx:left>
						<plx:callThis name="{$ActionType}_{$AbstractTypeName}_connect" accessor="this" type="field"/>
					</plx:left>
					<plx:right>
						<plx:callNew dataTypeName="ConnectionDelegate" dataTypeQualifier="{$TestClassName}" >
							<plx:passParam>
								<plx:nameRef name="GetConnection"/>
							</plx:passParam>
						</plx:callNew>
					</plx:right>
				</plx:assign>

				<!--
				//
				//testVar
				//
				-->
				<plx:comment></plx:comment>
				<plx:comment>testVar</plx:comment>
				<plx:comment></plx:comment>
				<plx:assign>
					<plx:left>
						<plx:callThis name="testVar" accessor="this" type="field"/>
					</plx:left>
					<plx:right>
						<plx:callNew dataTypeName="{$TestClassName}">
							<plx:passParam>
								<plx:nameRef name="{$ActionType}_{$AbstractTypeName}_connect"/>
							</plx:passParam>
						</plx:callNew>
					</plx:right>
				</plx:assign>

				<!--
				//
				//abstractTypeVar
				//
				-->
				<plx:comment></plx:comment>
				<plx:comment>abstractTypeVar</plx:comment>
				<plx:comment></plx:comment>
				<plx:assign>
					<plx:left>
						<plx:callThis name="abstractTypeVar" accessor="this" type="field"/>
					</plx:left>
					<plx:right>
						<plx:nullKeyword/>
					</plx:right>
				</plx:assign>


				<!--
				// 
				// lbl
				// 
				-->
				<plx:comment></plx:comment>
				<plx:comment>lbl</plx:comment>
				<plx:comment></plx:comment>
				<plx:assign>
					<plx:left>
						<plx:callThis name="lbl{$ActionType}" accessor="this" type="field"/>
					</plx:left>
					<plx:right>
						<plx:callNew dataTypeName="Label" dataTypeQualifier="System.Windows.Forms"/>
					</plx:right>
				</plx:assign>

				<plx:assign>
					<plx:left>
						<plx:callInstance name="Dock" type="property">
							<plx:callObject>
								<plx:callThis name="lbl{$ActionType}" accessor="this" type="field"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:left>
					<plx:right>
						<plx:callStatic name="Top" dataTypeName="DockStyle" dataTypeQualifier="System.Windows.Forms" type="field"/>
					</plx:right>
				</plx:assign>

				<plx:assign>
					<plx:left>
						<plx:callInstance name="Name" type="field">
							<plx:callObject>
								<plx:callThis name="lbl{$ActionType}" type="field" accessor="this"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:left>
					<plx:right>
						<plx:string data="lbl{$ActionType}"/>
					</plx:right>
				</plx:assign>

				<plx:assign>
					<plx:left>
						<plx:callInstance name="Text" type="field">
							<plx:callObject>
								<plx:callThis name="lbl{$ActionType}" type="field" accessor="this"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:left>
					<plx:right>
						<xsl:choose>
							<xsl:when test="$ActionType = 'Collection'">
								<plx:string data="Click the {$ActionType} button to refresh the list of {$AbstractTypeName} records."/>
							</xsl:when>
							<xsl:otherwise>
								<plx:string data="Enter data to {$ActionType} {$AbstractTypeName} by:"/>
							</xsl:otherwise>
						</xsl:choose>
					</plx:right>
				</plx:assign>

				<!--
				// 
				// btn
				// 
				-->
				<plx:comment></plx:comment>
				<plx:comment>btn</plx:comment>
				<plx:comment></plx:comment>
				<plx:assign>
					<plx:left>
						<plx:callThis name="btn{$ActionType}" accessor="this" type="field"/>
					</plx:left>
					<plx:right>
						<plx:callNew dataTypeName="Button" dataTypeQualifier="System.Windows.Forms"/>
					</plx:right>
				</plx:assign>

				<plx:assign>
					<plx:left>
						<plx:callInstance name="Location" type="field">
							<plx:callObject>
								<plx:callThis name="btn{$ActionType}" type="field" accessor="this"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:left>
					<plx:right>
						<plx:callNew dataTypeName="Point" dataTypeQualifier="System.Drawing">
							<plx:passParam>
								<plx:value data="400" type="i4"/>
							</plx:passParam>
							<plx:passParam>
								<plx:value data="10" type="i4"/>
							</plx:passParam>
						</plx:callNew>
					</plx:right>
				</plx:assign>

				<plx:assign>
					<plx:left>
						<plx:callInstance name="Name" type="field">
							<plx:callObject>
								<plx:callThis name="btn{$ActionType}" type="field" accessor="this"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:left>
					<plx:right>
						<plx:string data="btn{$ActionType}"/>
					</plx:right>
				</plx:assign>

				<plx:assign>
					<plx:left>
						<plx:callInstance name="TabIndex" type="field">
							<plx:callObject>
								<plx:callThis name="btn{$ActionType}" type="field" accessor="this"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:left>
					<plx:right>
						<plx:value data="3" type="i4"/>
					</plx:right>
				</plx:assign>

				<plx:assign>
					<plx:left>
						<plx:callInstance name="Text" type="field">
							<plx:callObject>
								<plx:callThis name="btn{$ActionType}" type="field" accessor="this"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:left>
					<plx:right>
						<plx:string data="{$ActionType}"/>
					</plx:right>
				</plx:assign>

				<plx:assign>
					<plx:left>
						<plx:callInstance name="UseVisualStyleBackColor" type="field">
							<plx:callObject>
								<plx:callThis name="btn{$ActionType}" type="field" accessor="this"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:left>
					<plx:right>
						<plx:trueKeyword/>
					</plx:right>
				</plx:assign>

				<plx:attachEvent>
					<plx:left>
						<plx:callInstance name="Click" type="event">
							<plx:callObject>
								<plx:callThis name="btn{$ActionType}" type="field" accessor="this"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:left>
					<plx:right>
						<plx:callNew dataTypeName="EventHandler" dataTypeQualifier="System" type="new">
							<plx:passParam>
								<plx:callThis name="btn{$ActionType}_Click" type="methodReference"/>
							</plx:passParam>
						</plx:callNew>
					</plx:right>
				</plx:attachEvent>

				<!--
				// 
				// dgv
				// 
				-->
				<plx:comment></plx:comment>
				<plx:comment>dgv</plx:comment>
				<plx:comment></plx:comment>
				<plx:assign>
					<plx:left>
						<plx:callThis name="dgv{$ActionType}" accessor="this" type="field"/>
					</plx:left>
					<plx:right>
						<plx:callNew dataTypeName="DataGridView" dataTypeQualifier="System.Windows.Forms"/>
					</plx:right>
				</plx:assign>

				<plx:assign>
					<plx:left>
						<plx:callInstance name="Dock" type="property">
							<plx:callObject>
								<plx:callThis name="dgv{$ActionType}" accessor="this" type="field"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:left>
					<plx:right>
						<plx:callStatic name="Top" dataTypeName="DockStyle" dataTypeQualifier="System.Windows.Forms" type="field"/>
					</plx:right>
				</plx:assign>

				<plx:assign>
					<plx:left>
						<plx:callInstance name="ColumnHeadersHeightSizeMode" type="field">
							<plx:callObject>
								<plx:callThis name="dgv{$ActionType}" type="field" accessor="this"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:left>
					<plx:right>
						<plx:callStatic name="AutoSize" dataTypeName="DataGridViewColumnHeadersHeightSizeMode" dataTypeQualifier="System.Windows.Forms" type="field"/>
					</plx:right>
				</plx:assign>

				<plx:assign>
					<plx:left>
						<plx:callInstance name="Name" type="field">
							<plx:callObject>
								<plx:callThis name="dgv{$ActionType}" type="field" accessor="this"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:left>
					<plx:right>
						<plx:string data="dgv{$ActionType}"/>
					</plx:right>
				</plx:assign>

				<plx:assign>
					<plx:left>
						<plx:callInstance name="TabIndex" type="field">
							<plx:callObject>
								<plx:callThis name="dgv{$ActionType}" type="field" accessor="this"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:left>
					<plx:right>
						<plx:value data="0" type="i4"/>
					</plx:right>
				</plx:assign>

				<xsl:choose>
					<xsl:when test="$ActionType = 'Create'">
						<xsl:for-each select="$Properties[not(@autoIncrement = 'true')]">
							<plx:callInstance name="Add" type="methodCall">
								<plx:callObject>
									<plx:callInstance name="Columns" type="property">
										<plx:callObject>
											<plx:callInstance name="dgv{$ActionType}" type="field">
												<plx:callObject>
													<plx:thisKeyword/>
												</plx:callObject>
											</plx:callInstance>
										</plx:callObject>
									</plx:callInstance>
								</plx:callObject>
								<xsl:choose>
									<xsl:when test="@displayName">
										<plx:passParam>
											<plx:string data="{@displayName}"/>
										</plx:passParam>
										<plx:passParam>
											<plx:string data="{@displayName}"/>
										</plx:passParam>
									</xsl:when>
									<xsl:otherwise>
										<plx:passParam>
											<plx:string data="{@name}"/>
										</plx:passParam>
										<plx:passParam>
											<plx:string data="{@name}"/>
										</plx:passParam>
									</xsl:otherwise>
								</xsl:choose>
							</plx:callInstance>
						</xsl:for-each>
					</xsl:when>
					<xsl:otherwise>
						<xsl:for-each select="$Properties">
							<plx:callInstance name="Add" type="methodCall">
								<plx:callObject>
									<plx:callInstance name="Columns" type="property">
										<plx:callObject>
											<plx:callInstance name="dgv{$ActionType}" type="field">
												<plx:callObject>
													<plx:thisKeyword/>
												</plx:callObject>
											</plx:callInstance>
										</plx:callObject>
									</plx:callInstance>
								</plx:callObject>
								<xsl:choose>
									<xsl:when test="@displayName">
										<plx:passParam>
											<plx:string data="{@displayName}"/>
										</plx:passParam>
										<plx:passParam>
											<plx:string data="{@displayName}"/>
										</plx:passParam>
									</xsl:when>
									<xsl:otherwise>
										<plx:passParam>
											<plx:string data="{@name}"/>
										</plx:passParam>
										<plx:passParam>
											<plx:string data="{@name}"/>
										</plx:passParam>
									</xsl:otherwise>
								</xsl:choose>
							</plx:callInstance>
						</xsl:for-each>
					</xsl:otherwise>
				</xsl:choose>

				<plx:assign>
					<plx:left>
						<plx:callInstance name="ScrollBars" type="property">
							<plx:callObject>
								<plx:callThis name="dgv{$ActionType}" type="field" accessor="this"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:left>
					<plx:right>
						<plx:callStatic name="Both" dataTypeName="ScrollBars" dataTypeQualifier="System.Windows.Forms" type="field"/>
					</plx:right>
				</plx:assign>

				<xsl:choose>
					<xsl:when test="$ActionType = 'Collection'">
						<plx:assign>
							<plx:left>
								<plx:callInstance name="Height" type="property">
									<plx:callObject>
										<plx:callThis name="dgv{$ActionType}" type="field" accessor="this"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:left>
							<plx:right>
								<plx:value data="300" type="i4"/>
							</plx:right>
						</plx:assign>
					</xsl:when>
					<xsl:otherwise>
						<plx:assign>
							<plx:left>
								<plx:callInstance name="Height" type="property">
									<plx:callObject>
										<plx:callThis name="dgv{$ActionType}" type="field" accessor="this"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:left>
							<plx:right>
								<plx:value data="75" type="i4"/>
							</plx:right>
						</plx:assign>
					</xsl:otherwise>
				</xsl:choose>

				<!--
				//
				//pnlDisplay
				//
				-->
				<plx:comment></plx:comment>
				<plx:comment>pnlDisplay</plx:comment>
				<plx:comment></plx:comment>
				<plx:assign>
					<plx:left>
						<plx:callThis name="pnlDisplay" accessor="this" type="field"/>
					</plx:left>
					<plx:right>
						<plx:callNew dataTypeName="Panel" dataTypeQualifier="System.Windows.Forms"/>
					</plx:right>
				</plx:assign>

				<plx:assign>
					<plx:left>
						<plx:callInstance name="Dock" type="property">
							<plx:callObject>
								<plx:callThis name="pnlDisplay" accessor="this" type="field"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:left>
					<plx:right>
						<plx:callStatic name="Top" dataTypeName="DockStyle" dataTypeQualifier="System.Windows.Forms" type="field"/>
					</plx:right>
				</plx:assign>

				<plx:assign>
					<plx:left>
						<plx:callInstance name="AutoSize" type="field">
							<plx:callObject>
								<plx:callThis name="pnlDisplay" type="field" accessor="this"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:left>
					<plx:right>
						<plx:trueKeyword/>
					</plx:right>
				</plx:assign>

				<plx:assign>
					<plx:left>
						<plx:callInstance name="Location" type="field">
							<plx:callObject>
								<plx:callThis name="pnlDisplay" type="field" accessor="this"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:left>

					<plx:right>
						<plx:callNew dataTypeName="Point" dataTypeQualifier="System.Drawing">
							<plx:passParam>
								<plx:value data="0" type="i4"/>
							</plx:passParam>
							<plx:passParam>
								<plx:value data="0" type="i4"/>
							</plx:passParam>
						</plx:callNew>
					</plx:right>
				</plx:assign>

				<plx:assign>
					<plx:left>
						<plx:callInstance name="Name" type="field">
							<plx:callObject>
								<plx:callThis name="pnlDisplay" type="field" accessor="this"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:left>
					<plx:right>
						<plx:string data="pnlDisplay"/>
					</plx:right>
				</plx:assign>

				<plx:assign>
					<plx:left>
						<plx:callInstance name="AutoScroll" type="field">
							<plx:callObject>
								<plx:callThis name="pnlDisplay" type="field" accessor="this"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:left>
					<plx:right>
						<plx:trueKeyword/>
					</plx:right>
				</plx:assign>

				<plx:assign>
					<plx:left>
						<plx:callInstance name="TabIndex" type="field">
							<plx:callObject>
								<plx:callThis name="pnlDisplay" type="field" accessor="this"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:left>
					<plx:right>
						<plx:value data="1" type="i4"/>
					</plx:right>
				</plx:assign>

				<plx:callInstance name="Add" type="methodCall">
					<plx:callObject>
						<plx:callInstance name="Controls" type="property">
							<plx:callObject>
								<plx:callThis name="pnlDisplay" type="field" accessor="this"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:callObject>
					<plx:passParam>
						<plx:callThis name="btn{$ActionType}" type="field" accessor="this"/>
					</plx:passParam>
				</plx:callInstance>

				<!--
				// 
				// this
				// 
				-->
				<plx:comment></plx:comment>
				<plx:comment>this</plx:comment>
				<plx:comment></plx:comment>
				<plx:assign>
					<plx:left>
						<plx:callThis name="AutoScaleDimensions" type="field" accessor="this"/>
					</plx:left>
					<plx:right>
						<plx:callNew dataTypeName="SizeF" dataTypeQualifier="System.Drawing">
							<plx:passParam>
								<plx:value data="6" type="r4"/>
							</plx:passParam>
							<plx:passParam>
								<plx:value data="13" type="r4"/>
							</plx:passParam>
						</plx:callNew>
					</plx:right>
				</plx:assign>

				<plx:assign>
					<plx:left>
						<plx:callThis name="AutoScaleMode" type="field" accessor="this"/>
					</plx:left>
					<plx:right>
						<plx:callStatic type="field" name="Font" dataTypeName="AutoScaleMode" dataTypeQualifier="System.Windows.Forms"/>
					</plx:right>
				</plx:assign>

				<plx:assign>
					<plx:left>
						<plx:callThis name="BorderStyle" type="field" accessor="this"/>
					</plx:left>
					<plx:right>
						<plx:callStatic type="field" name="Fixed3D" dataTypeName="BorderStyle" dataTypeQualifier="System.Windows.Forms"/>
					</plx:right>
				</plx:assign>

				<plx:callInstance name="Add" type="methodCall">
					<plx:callObject>
						<plx:callThis name="Controls" type="field" accessor="this"/>
					</plx:callObject>
					<plx:passParam>
						<plx:callThis name="pnlDisplay" type="field" accessor="this"/>
					</plx:passParam>
				</plx:callInstance>

				<plx:callInstance name="Add" type="methodCall">
					<plx:callObject>
						<plx:callThis name="Controls" type="field" accessor="this"/>
					</plx:callObject>
					<plx:passParam>
						<plx:callThis name="dgv{$ActionType}" type="field" accessor="this"/>
					</plx:passParam>
				</plx:callInstance>

				<plx:callInstance name="Add" type="methodCall">
					<plx:callObject>
						<plx:callThis name="Controls" type="field" accessor="this"/>
					</plx:callObject>
					<plx:passParam>
						<plx:callThis name="lbl{$ActionType}" type="field" accessor="this"/>
					</plx:passParam>
				</plx:callInstance>

				<plx:assign>
					<plx:left>
						<plx:callThis name="Name" type="field" accessor="this"/>
					</plx:left>
					<plx:right>
						<plx:string data="ic{$ActionType}{$AbstractTypeName}InputControl"/>
					</plx:right>
				</plx:assign>

				<plx:assign>
					<plx:left>
						<plx:callThis name="Size" type="field" accessor="this"/>
					</plx:left>
					<plx:right>
						<plx:callNew dataTypeName="Size" dataTypeQualifier="System.Drawing">
							<plx:passParam>
								<plx:value data="530" type="i4"/>
							</plx:passParam>
							<plx:passParam>
								<plx:value data="490" type="i4"/>
							</plx:passParam>
						</plx:callNew>
					</plx:right>
				</plx:assign>

				<plx:callInstance name="EndInit" type="methodCall">
					<plx:callObject>
						<plx:cast dataTypeName="ISupportInitialize" dataTypeQualifier="System.ComponentModel">
							<plx:callThis name="dgv{$ActionType}" type="field" accessor="this"/>
						</plx:cast>
					</plx:callObject>
				</plx:callInstance>

				<plx:callThis name="ResumeLayout" type="methodCall" accessor="this">
					<plx:passParam>
						<plx:falseKeyword/>
					</plx:passParam>
				</plx:callThis>
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

	<!-- Use when adding a tab to the tab control on the first custom control. -->
	<xsl:template name="AddActionTabsTab">
		<xsl:param name="ActionType"/>
		<!-- The type of manipulation you are doing.  Do not use if the control is not doing any manipulation. - Create, Select, Collection -->
		<xsl:param name="Name"/>
		<!-- The name of the abstract entity type to manipulate. -->

		<plx:callInstance name="Add" type="methodCall">
			<plx:callObject>
				<plx:callInstance name="TabPages" type="property">
					<plx:callObject>
						<plx:callInstance name="actionTabs" type="field">
							<plx:callObject>
								<plx:thisKeyword/>
							</plx:callObject>
						</plx:callInstance>
					</plx:callObject>
				</plx:callInstance>
			</plx:callObject>
			<plx:passParam>
				<plx:string>
					<xsl:value-of select="$ActionType"/>
				</plx:string>
			</plx:passParam>
			<plx:passParam>
				<plx:string>
					<xsl:choose>
						<xsl:when test="$ActionType = 'Select'">
							<xsl:value-of select="concat($ActionType, ' and Edit')"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="$ActionType"/>
						</xsl:otherwise>
					</xsl:choose>
				</plx:string>
			</plx:passParam>
		</plx:callInstance>

		<plx:local dataTypeName="{concat($ActionType, '_', $Name, '_InputControl')}" name="ic{$ActionType}">
			<plx:initialize>
				<plx:callNew dataTypeName="{concat($ActionType, '_', $Name, '_InputControl')}"/>
			</plx:initialize>
		</plx:local>

		<plx:assign>
			<plx:left>
				<plx:callInstance name="Dock" type="property">
					<plx:callObject>
						<plx:nameRef name="ic{$ActionType}" type="local"/>
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
								<plx:callInstance name="actionTabs" type="field">
									<plx:callObject>
										<plx:thisKeyword/>
									</plx:callObject>
								</plx:callInstance>
							</plx:callObject>
							<plx:passParam>
								<plx:string>
									<xsl:value-of select="$ActionType"/>
								</plx:string>
							</plx:passParam>
						</plx:callInstance>
					</plx:callObject>
				</plx:callInstance>
			</plx:callObject>
			<plx:passParam>
				<plx:nameRef name="ic{$ActionType}"/>
			</plx:passParam>
		</plx:callInstance>
	</xsl:template>

	<!-- Used when creating the AllClasses var from the Oil var -->
	<xsl:template name="CreateValueTypeElement">
		<xsl:param name="Element"/>
		<!-- The Element from the OIAL file containing value type. -->
		<xsl:param name="Type"/>
		<!-- The type of the value type. -->
		<xsl:param name="AutoIncrement"/>
		<!-- Pass in true if this value type is incremented by the database. -->
		<xsl:element name="ValueType">
			<xsl:copy-of select="$Element/@*"/>
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

</xsl:stylesheet>
