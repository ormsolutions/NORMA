<?xml version="1.0" encoding="UTF-8" ?>
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
	<!-- Output file:  app.config -->
	
	<xsl:param name="OIAL" select="document('Portfolio.OIAL.xml')/child::*"/>

	<xsl:variable name="ModelName" select="$OIAL/@name"/>

	<!-- For possible future customization -->
	<xsl:variable name="AllClassesFragment">
		<xsl:for-each select="plx:root/plx:namespace//plx:class">
			<xsl:choose>
				<xsl:when test="not(substring-before(@name, 'Core') = '')">
					<xsl:element name="Class">
						<xsl:copy-of select="@name"/>
					</xsl:element>
				</xsl:when>
			</xsl:choose>
		</xsl:for-each>
	</xsl:variable>
	<xsl:variable name="AllClasses" select="exsl:node-set($AllClassesFragment)/child::*"/>

	<xsl:output method="xml" encoding="utf-8" media-type="text/xml" indent="yes"/>
	<xsl:template match="/">
		<xsl:element name="configuration">
			<xsl:element name="connectionStrings">
				<!-- Add a connection string -->
				<xsl:call-template name="AddConnectionString" />
			</xsl:element>
		</xsl:element>
	</xsl:template>

	<xsl:template name="AddConnectionString">
		<xsl:element name="add">
			<!-- The name of the connection string will be 'connString'. -->
			<xsl:attribute name="name">
				<xsl:text>connString</xsl:text>
			</xsl:attribute>
			<!-- A generic connection string.  Do we want this to be generated based on user input? -->
			<xsl:attribute name="connectionString">
				<xsl:text>Data Source=localhost;Initial Catalog=</xsl:text>
				<xsl:value-of select="$ModelName"/>
				<xsl:text>;Integrated Security=True</xsl:text>
			</xsl:attribute>
			<!-- In the future this information should probably be passed in as a parameter. -->
			<xsl:attribute name="providerName">
				<xsl:text>System.Data.SqlClient</xsl:text>
			</xsl:attribute>
		</xsl:element>
	</xsl:template>
</xsl:stylesheet>