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
	<!-- Output file:  Program.PLiX.xml -->

	<!-- This generates a generic Program.cs file.  All that it does is launch the generated Win Form. -->

	<xsl:output method="xml" encoding="utf-8" media-type="text/xml" indent="yes"/>
	
	<xsl:template match="/">
		<xsl:variable name="Model" select="./child::*"/>
		<xsl:variable name="ModelName" select="$Model/plx:namespace/@name"/>
		<plx:root>
			<plx:namespaceImport name="System" />
			<plx:namespaceImport name="System.Collections.Generic" />
			<plx:namespaceImport name="System.Windows.Forms" />

			<plx:namespace name="{$ModelName}">
				<plx:class name="Program" modifier="static" visibility="public">
					<plx:function name="Main" modifier="static" visibility="public">
						<plx:leadingInfo>
							<plx:docComment>
								<summary>The main entry point for the application.</summary>
							</plx:docComment>
						</plx:leadingInfo>
						<plx:callInstance name="EnableVisualStyles">
							<plx:callObject>
								<plx:nameRef name="Application"/>
							</plx:callObject>
						</plx:callInstance>
						<plx:callInstance name="SetCompatibleTextRenderingDefault">
							<plx:callObject>
								<plx:nameRef name="Application"/>
							</plx:callObject>
							<plx:passParam type="in">
								<plx:falseKeyword/>
							</plx:passParam>
						</plx:callInstance>
						<plx:callInstance name="Run">
							<plx:callObject>
								<plx:nameRef name="Application"/>
							</plx:callObject>
							<plx:passParam>
								<plx:callNew type="new" dataTypeName="{$ModelName}Tester"></plx:callNew>
							</plx:passParam>
						</plx:callInstance>
					</plx:function>
				</plx:class>
			</plx:namespace>
		</plx:root>
	</xsl:template>
</xsl:stylesheet>