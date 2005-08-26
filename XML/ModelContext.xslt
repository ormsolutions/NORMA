<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:orm="http://Schemas.Neumont.edu/ORM/ORMCore"
	xmlns:ormRoot="http://Schemas.Neumont.edu/ORM/ORMRoot"
	xmlns:plx="http://Schemas.Neumont.edu/CodeGeneration/Plix"
	xmlns:ao="http://Schemas.Neumont.edu/ORM/SDK/ClassGenerator/AbsorbedObjects">
	<xsl:variable name="ModelContextName" select="concat(ormRoot:ORM2/orm:ORMModel/@Name,'Context')"/>
	<xsl:template match="orm:ORMModel" mode="ModelContext">
		<plx:Class visibility="Public" partial="true" name="{$ModelContextName}"/>
	</xsl:template>
</xsl:stylesheet>