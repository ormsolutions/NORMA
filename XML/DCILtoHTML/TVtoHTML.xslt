<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns="http://www.w3.org/1999/xhtml"
	exclude-result-prefixes="#default xsl msxsl">
	<xsl:output method="xml" doctype-public="-//W3C//DTD XHTML 1.0 Strict//EN" doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd" media-type="application/html+xml" encoding="utf-8" omit-xml-declaration="yes"/>
	<xsl:template match="TView">
		<xsl:variable name="tableGen">
			<!-- copying the XML into the local memory for faster access -->
			<xsl:copy-of select="."/>
		</xsl:variable>
		
	   <html>
			<head>
				<title>Horizontal Table Layout</title>
				<style type="text/css">
					.table { white-space:nowrap; }
					.indent { left: 20px; position: relative; }
				</style>
			</head>
			<body>
				<xsl:for-each select="msxsl:node-set($tableGen)/child::TView/tables/table">
					<p class="table">
						<b>
							<xsl:value-of select="@name"/>
						</b>
						<xsl:text> (</xsl:text>
						<xsl:for-each select="column">
							<xsl:call-template name="seperator"/>
							<xsl:choose>
								<xsl:when test="@isOptional = 'true'">
									<xsl:call-template name="optional"/>
								</xsl:when>
								<xsl:when test="../uniquenessConstraint/column/@name = current()/@name">
									<xsl:call-template name="unique"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="@name"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:for-each>
						<xsl:text>)</xsl:text>
						<br/>
						<span class="indent">
							<xsl:text>Primary Key: </xsl:text>
							<xsl:for-each select="column[@isPrimaryKey = 'true']">
								<xsl:call-template name="seperator"/>
								<xsl:value-of select="@name"/>
							</xsl:for-each>
							<br/>
							<span class="indent">
								<xsl:text>used by FKeys: </xsl:text>
								<xsl:for-each select="//TView/FKeys/tableFKs/FK[@targetTable = current()/@name]">
									<xsl:call-template name="seperator"/>
									<xsl:value-of select="../@sourceTable"/>
									<xsl:text>.</xsl:text>
									<xsl:value-of select="@name"/>
								</xsl:for-each>
								<xsl:if test ="count(//TView/FKeys/tableFKs/FK[@targetTable = current()/@name]) = '0'">
									None
								</xsl:if>
							</span>
						<br/>
						<xsl:text>Foriegn Keys: </xsl:text>
						<xsl:if test ="count(//TView/FKeys/tableFKs[@sourceTable = current()/@name]/FK) = '0'">
							None
						</xsl:if>
						<br/>
							<xsl:for-each select="//TView/FKeys/tableFKs[@sourceTable = current()/@name]/FK">
								<span class="indent">
									<xsl:value-of select="@name"/>
									<xsl:text> (</xsl:text>
									<xsl:for-each select="column">
										<xsl:call-template name="seperator"/>
										<xsl:value-of select="@sourceColumn"/>
										<xsl:text>--></xsl:text>
										<xsl:value-of select="../@targetTable"/>
										<xsl:text>.</xsl:text>
										<xsl:value-of select="@targetColumn"/>
									</xsl:for-each>
									<xsl:text>)</xsl:text>
									<br/>
								</span>
							</xsl:for-each>
						</span>
					</p>
				</xsl:for-each>
			</body>
		</html>
	</xsl:template>
	<xsl:template name="seperator">
		<xsl:if test="position() != '1'">
			<xsl:text>, </xsl:text>
		</xsl:if>
	</xsl:template>
	<xsl:template name="unique">
		<u>
			<xsl:choose>
				<xsl:when test="@isPrimaryKey = 'true'">
					<xsl:call-template name="primaryKey"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="@name"/>
				</xsl:otherwise>
			</xsl:choose>
		</u>
	</xsl:template>
	<xsl:template name="primaryKey">
		<b>
			<xsl:value-of select="@name"/>
		</b>
	</xsl:template>
	<xsl:template name="optional">
		<xsl:text>[</xsl:text>
		<xsl:choose>
			<xsl:when test="../uniquenessConstraint/column/@name = current()/@name">
				<xsl:call-template name="unique"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="@name"/>
			</xsl:otherwise>
		</xsl:choose>
		<xsl:text>]</xsl:text>
	</xsl:template>
</xsl:stylesheet> 