<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				xmlns:test="http://schemas.neumont.edu/ORM/2006-01/TestOIALModel"
				xmlns:ormRoot="http://schemas.neumont.edu/ORM/2006-04/ORMRoot"
				xmlns:orm="http://schemas.neumont.edu/ORM/2006-04/ORMCore">

	<xsl:output method="xml" indent="yes" omit-xml-declaration="yes"/>

	<xsl:template match="/">
		<xsl:apply-templates select="ormRoot:ORM2/test:Model"/>
	</xsl:template>

	<xsl:template match="*">
		<xsl:copy>
			<xsl:copy-of select="@*[not(local-name()='id' or local-name()='ref')]"/>
			<xsl:if test="self::orm:Role or self::test:ValueType or self::test:ObjectType">
				<xsl:copy-of select="@ref"/>
			</xsl:if>
			<xsl:for-each select="*">
				<xsl:sort select="@Name"/>
				<xsl:copy>
					<xsl:copy-of select="@*[not(local-name()='id' or local-name()='ref')]"/>
					<xsl:if test="self::orm:Role or self::test:ValueType or self::test:ObjectType">
						<xsl:copy-of select="@ref"/>
					</xsl:if>
					<xsl:apply-templates select="*">
						<xsl:sort select="@Name"/>
					</xsl:apply-templates>
				</xsl:copy>
			</xsl:for-each>
		</xsl:copy>
	</xsl:template>
</xsl:stylesheet>
