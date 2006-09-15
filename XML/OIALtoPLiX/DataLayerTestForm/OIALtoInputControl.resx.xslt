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
	<!-- Output file:  InputControl.resx -->

	<xsl:output method="xml" encoding="utf-8" media-type="text/xml" indent="yes"/>

	<xsl:template match="/">
		<xsl:variable name="Model" select="./child::*"/>
		<xsl:variable name="ModelName" select="$Model/plx:namespace/@name"/>

		<!-- This just generates a generic resx file for .NET 2.0.  Do we want to possibly do some custom generation in the future? -->
		<xsl:element name="root">

			<xsl:element name="resheader">
				<xsl:attribute name="name">
					<xsl:text>resmimetype</xsl:text>
				</xsl:attribute>
				<xsl:element name="value">
					<xsl:text>text/microsoft-resx</xsl:text>
				</xsl:element>
			</xsl:element>

			<xsl:element name="resheader">
				<xsl:attribute name="name">
					<xsl:text>version</xsl:text>
				</xsl:attribute>
				<xsl:element name="value">
					<xsl:text>2.0</xsl:text>
				</xsl:element>
			</xsl:element>

			<xsl:element name="resheader">
				<xsl:attribute name="name">
					<xsl:text>reader</xsl:text>
				</xsl:attribute>
				<xsl:element name="value">
					<xsl:text>System.Resources.ResXResourceReader, System.Windows.Forms, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</xsl:text>
				</xsl:element>
			</xsl:element>

			<xsl:element name="resheader">
				<xsl:attribute name="name">
					<xsl:text>writer</xsl:text>
				</xsl:attribute>
				<xsl:element name="value">
					<xsl:text>System.Resources.ResXResourceWriter, System.Windows.Forms, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</xsl:text>
				</xsl:element>
			</xsl:element>

		</xsl:element>

	</xsl:template>



</xsl:stylesheet>
