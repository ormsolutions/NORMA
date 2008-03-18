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
	xmlns:dil="http://schemas.orm.net/DIL/DIL"
	xmlns:ddt="http://schemas.orm.net/DIL/DILDT"
	xmlns:dep="http://schemas.orm.net/DIL/DILEP"
	xmlns:dms="http://schemas.orm.net/DIL/DILMS"
	xmlns:dml="http://schemas.orm.net/DIL/DMIL"
	xmlns:ddl="http://schemas.orm.net/DIL/DDIL"
	extension-element-prefixes="exsl">

	<xsl:output method="xml" encoding="utf-8" media-type="text/xml" indent="yes"/>
	<xsl:strip-space elements="*"/>

	<xsl:template match="*" mode="TruthValueTestRemover">
		<xsl:copy>
			<xsl:copy-of select="@*"/>
			<xsl:apply-templates mode="TruthValueTestRemover"/>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="dep:is | dep:isNot" mode="TruthValueTestRemover">
		<xsl:choose>
			<xsl:when test="@truthValue = 'UNKNOWN'">
				<dep:nullPredicate type="NULL">
					<xsl:if test="self::dep:isNot">
						<xsl:attribute name="type">
							<xsl:value-of select="'NOT NULL'"/>
						</xsl:attribute>
					</xsl:if>
					<xsl:apply-templates mode="TruthValueTestRemover"/>
				</dep:nullPredicate>
			</xsl:when>
			<xsl:otherwise>
				<dep:parenthesizedValueExpression>
					<dep:and>
						<dep:nullPredicate type="NOT NULL">
							<xsl:apply-templates mode="TruthValueTestRemover"/>
						</dep:nullPredicate>
						<dep:comparisonPredicate operator="equals">
							<xsl:if test="self::dep:isNot">
								<xsl:attribute name="operator">
									<xsl:value-of select="'notEquals'"/>
								</xsl:attribute>
							</xsl:if>
							<xsl:apply-templates mode="TruthValueTestRemover"/>
							<ddt:booleanLiteral value="{@truthValue}"/>
						</dep:comparisonPredicate>
					</dep:and>
				</dep:parenthesizedValueExpression>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>


</xsl:stylesheet>
