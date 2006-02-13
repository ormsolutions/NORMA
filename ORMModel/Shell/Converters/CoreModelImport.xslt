<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:ormRoot="http://schemas.neumont.edu/ORM/2006-01/ORMRoot"
	xmlns:orm="http://schemas.neumont.edu/ORM/2006-01/ORMCore"
	xmlns:ormDiagram="http://schemas.neumont.edu/ORM/2006-01/ORMDiagram"
	exclude-result-prefixes="#default xsl">
	<xsl:template match="orm:ORMModel">
		<ormRoot:ORM2>
			<xsl:copy-of select="."/>
			<ormDiagram:ORMDiagram id="{@id}_diagram" AutoPopulateShapes="true" IsCompleteView="false" Name="{@Name}" BaseFontName="Tahoma" BaseFontSize="0.0972222238779068">
				<ormDiagram:Shapes></ormDiagram:Shapes>
				<ormDiagram:Subject ref="{@id}"/>
			</ormDiagram:ORMDiagram>
		</ormRoot:ORM2>
	</xsl:template>
</xsl:stylesheet>