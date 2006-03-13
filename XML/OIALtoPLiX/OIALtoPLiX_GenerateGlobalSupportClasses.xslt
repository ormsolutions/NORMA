<?xml version="1.0" encoding="utf-8"?>
<!--
	Copyright © Neumont University. All rights reserved.

	This software is provided 'as-is', without any express or implied warranty. In no event will the authors be held liable for any damages arising from the use of this software.
	Permission is granted to anyone to use this software for any purpose, including commercial applications, and to alter it and redistribute it freely, subject to the following restrictions:
	1. The origin of this software must not be misrepresented; you must not claim that you wrote the original software. If you use this software in a product, an acknowledgment in the product documentation would be appreciated but is not required.
	2. Altered source versions must be plainly marked as such, and must not be misrepresented as being the original software.
	3. This notice may not be removed or altered from any source distribution.
-->
<!-- Contributors: Kevin M. Owen -->
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:exsl="http://exslt.org/common"
	xmlns:plx="http://schemas.neumont.edu/CodeGeneration/PLiX"
	extension-element-prefixes="exsl">

	<xsl:output method="xml" encoding="utf-8" media-type="text/xml" indent="yes"/>
	
	<xsl:include href="OIALtoPLiX_GenerateTuple.xslt"/>

	<!-- TODO: Determine if these classes are already available in the solution (or anything referenced by it) prior to generating them. -->
	<xsl:template name="GenerateGlobalSupportClasses">
		<xsl:variable name="propertyChangeEventArgsClassBodyFragment">
			<plx:implementsInterface dataTypeName="IPropertyChangeEventArgs">
				<plx:passTypeParam dataTypeName="TProperty"/>
			</plx:implementsInterface>
			<plx:field visibility="private" readOnly="true" name="oldValue" dataTypeName="TProperty"/>
			<plx:field visibility="private" readOnly="true" name="newValue" dataTypeName="TProperty"/>
			<plx:function name=".construct"  visibility="public">
				<plx:param name="oldValue" dataTypeName="TProperty"/>
				<plx:param name="newValue" dataTypeName="TProperty"/>
				<plx:assign>
					<plx:left>
						<plx:callThis name="oldValue" type="field"/>
					</plx:left>
					<plx:right>
						<plx:nameRef type="parameter" name="oldValue"/>
					</plx:right>
				</plx:assign>
				<plx:assign>
					<plx:left>
						<plx:callThis name="newValue" type="field"/>
					</plx:left>
					<plx:right>
						<plx:nameRef type="parameter" name="newValue"/>
					</plx:right>
				</plx:assign>
			</plx:function>
			<plx:property visibility="public" name="OldValue">
				<plx:interfaceMember memberName="OldValue" dataTypeName="IPropertyChangeEventArgs"/>
				<plx:returns dataTypeName="TProperty"/>
				<plx:get>
					<plx:return>
						<plx:callThis name="oldValue" type="field"/>
					</plx:return>
				</plx:get>
			</plx:property>
			<plx:property visibility="public" name="NewValue">
				<plx:interfaceMember memberName="NewValue" dataTypeName="IPropertyChangeEventArgs"/>
				<plx:returns dataTypeName="TProperty"/>
				<plx:get>
					<plx:return>
						<plx:callThis name="newValue" type="field"/>
					</plx:return>
				</plx:get>
			</plx:property>
		</xsl:variable>
		<xsl:variable name="propertyChangeEventArgsClassBody" select="exsl:node-set($propertyChangeEventArgsClassBodyFragment)/child::*"/>
		<plx:namespace name="System">
			<plx:leadingInfo>
				<plx:pragma type="region" data="Global Support Classes"/>
			</plx:leadingInfo>
			<plx:trailingInfo>
				<plx:pragma type="closeRegion" data="Global Support Classes"/>
			</plx:trailingInfo>
			<xsl:call-template name="GenerateCommonTuples"/>
			<plx:interface visibility="public" name="IPropertyChangeEventArgs">
				<plx:leadingInfo>
					<plx:pragma type="region" data="Property Change Event Support"/>
				</plx:leadingInfo>
				<plx:typeParam name="TProperty"/>
				<plx:property visibility="public" modifier="abstract" name="OldValue">
					<plx:returns dataTypeName="TProperty"/>
					<plx:get/>
				</plx:property>
				<plx:property visibility="public" modifier="abstract" name="NewValue">
					<plx:returns dataTypeName="TProperty"/>
					<plx:get/>
				</plx:property>
			</plx:interface>
			<plx:class visibility="public" modifier="sealed" name="PropertyChangingEventArgs">
				<plx:typeParam name="TProperty"/>
				<plx:derivesFromClass dataTypeName="CancelEventArgs"/>
				<xsl:copy-of select="$propertyChangeEventArgsClassBody"/>
			</plx:class>
			<plx:class visibility="public" modifier="sealed" name="PropertyChangedEventArgs">
				<plx:trailingInfo>
					<plx:pragma type="closeRegion" data="Property Change Event Support"/>
				</plx:trailingInfo>
				<plx:typeParam name="TProperty"/>
				<plx:derivesFromClass dataTypeName="EventArgs"/>
				<xsl:copy-of select="$propertyChangeEventArgsClassBody"/>
			</plx:class>
		</plx:namespace>
	</xsl:template>

</xsl:stylesheet>
