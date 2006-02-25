<?xml version="1.0" encoding="utf-8"?>
<!--
	Copyright © Neumont University. All rights reserved.

	This software is provided 'as-is', without any express or implied warranty. In no event will the authors be held liable for any damages arising from the use of this software.
	Permission is granted to anyone to use this software for any purpose, including commercial applications, and to alter it and redistribute it freely, subject to the following restrictions:
	1. The origin of this software must not be misrepresented; you must not claim that you wrote the original software. If you use this software in a product, an acknowledgment in the product documentation would be appreciated but is not required.
	2. Altered source versions must be plainly marked as such, and must not be misrepresented as being the original software.
	3. This notice may not be removed or altered from any source distribution.
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:ormRoot="http://schemas.neumont.edu/ORM/2006-01/ORMRoot"
	xmlns:orm="http://schemas.neumont.edu/ORM/2006-01/ORMCore"
	xmlns:ormDiagram="http://schemas.neumont.edu/ORM/2006-01/ORMDiagram"
	exclude-result-prefixes="#default xsl">
	<xsl:output method="xml" encoding="utf-8" indent="no"/>
	<xsl:template match="orm:ORMModel">
		<ormRoot:ORM2>
			<xsl:copy-of select="."/>
			<ormDiagram:ORMDiagram id="{@id}_diagram" AutoPopulateShapes="true" IsCompleteView="false" Name="{@Name}" BaseFontName="Tahoma" BaseFontSize="0.0972222238779068">
				<ormDiagram:Shapes/>
				<ormDiagram:Subject ref="{@id}"/>
			</ormDiagram:ORMDiagram>
		</ormRoot:ORM2>
	</xsl:template>
</xsl:stylesheet>