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

	<!-- Input file:  [ORM Model Name].Implementation.PLiX.xml -->
	<!-- Output file:  [ORM Model Name]TestForm.PLiX.xml -->

	<xsl:output method="xml" encoding="utf-8" media-type="text/xml" indent="yes"/>

	<!-- The only thing generated for the test form is a constructor.  All other form code is in the designer file. -->

	<xsl:variable name="SprocSuffix" select="'SP'"/>
	<xsl:variable name="CollectionSuffix" select="'Collections'"/>
	<xsl:variable name="debugMode" select="'false'"/>

	<xsl:template match="/" mode="AddNamespaceImports">
		<plx:namespaceImport name="System"/>
		<plx:namespaceImport name="System.Collections.Generic"/>
		<plx:namespaceImport name="System.Collections.ObjectModel"/>
		<plx:namespaceImport name="System.ComponentModel"/>
		<plx:namespaceImport name="System.Xml"/>
		<plx:namespaceImport name="System.Data"/>
		<plx:namespaceImport name="System.Data.SqlClient"/>
		<plx:namespaceImport name="System.Drawing"/>
		<plx:namespaceImport name="System.Text" />
		<plx:namespaceImport name="System.Windows.Forms"/>
	</xsl:template>

	<xsl:template match="/">
		<xsl:variable name="Model" select="./child::*"/>
		<xsl:variable name="ModelName" select="$Model/plx:namespace/@name"/>

		<plx:root>
			<xsl:apply-templates select="." mode="AddNamespaceImports"/>
			<plx:namespace name="{$ModelName}">
				<plx:class name="{$ModelName}Tester" partial="true" visibility="public">
					<plx:derivesFromClass dataTypeName="Form"/>
					<!-- constructor -->
					<plx:function name=".construct" visibility="public">
						<plx:callThis name="InitializeComponent" type="methodCall"/>
					</plx:function>
				</plx:class>
			</plx:namespace>
		</plx:root>
	</xsl:template>

</xsl:stylesheet>
