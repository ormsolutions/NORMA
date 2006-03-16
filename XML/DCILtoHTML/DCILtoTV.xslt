<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:dcl="http://schemas.orm.net/DIL/DCIL"
	xmlns:dil="http://schemas.orm.net/DIL/DIL"
	xmlns:ddt="http://schemas.orm.net/DIL/DILDT"
	xmlns:dep="http://schemas.orm.net/DIL/DILEP">
	<xsl:output method="xml" encoding="utf-8" media-type="text/xml" indent="yes"/>
	<xsl:template match="dcl:schema">
		<!--Extracting the foreign keys so they can be easily identified-->
		<xsl:variable name="FKeys1">
			<xsl:for-each select="dcl:table/dcl:referenceConstraint">
				<tableFKs>
					<xsl:attribute name="sourceTable">
						<xsl:value-of select="../@name"/>
					</xsl:attribute>
					<FK>
						<xsl:for-each select="dcl:columnRef">
							<xsl:copy-of select="../@name"/>
							<xsl:attribute name="targetTable">
								<xsl:value-of select="../@targetTable"/>
							</xsl:attribute>
							<column>
								<xsl:attribute name="sourceColumn">
									<xsl:value-of select="@sourceName"/>
								</xsl:attribute>
								<xsl:attribute name="targetColumn">
									<xsl:value-of select="@targetName"/>
								</xsl:attribute>
							</column>
						</xsl:for-each>
					</FK>
				</tableFKs>
			</xsl:for-each>
		</xsl:variable>
		<!--Compress the keys to be more space efficient-->
		<xsl:variable name="FKeys2">
			<xsl:for-each select="msxsl:node-set($FKeys1)/tableFKs">
				<xsl:if test="(position()=1) or (preceding-sibling::tableFKs[1]/@sourceTable != current()/@sourceTable)">
					<tableFKs>
						<xsl:copy-of select="@sourceTable"/>
						<xsl:for-each select="../tableFKs[@sourceTable = current()/@sourceTable]">
							<xsl:copy-of select="FK"/>
						</xsl:for-each>
					</tableFKs>
				</xsl:if>
			</xsl:for-each>
		</xsl:variable>
		<!--Extract the information about the tables and columns and put them into a useful format-->
		<xsl:variable name="tables">
			<xsl:for-each select="dcl:table">
				<table>
					<xsl:copy-of select="@name"/>
					<xsl:for-each select="dcl:column">
						<column>
							<xsl:copy-of select="@name"/>
							<xsl:attribute name="isPrimaryKey">
								<xsl:choose>
									<xsl:when test="../dcl:uniquenessConstraint/dcl:columnRef/@name = current()/@name">
										<xsl:value-of select="../dcl:uniquenessConstraint/dcl:columnRef[@name = current()/@name]/../@isPrimary"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:text>false</xsl:text>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
							<xsl:attribute name="isOptional">
								<xsl:value-of select="@isNullable"/>
							</xsl:attribute>
						</column>
					</xsl:for-each>
					<xsl:for-each select="dcl:uniquenessConstraint">
						<!--<xsl:sort select="@isPrimary" order="descending"/>  sort removed to preserve origional generated column order-->
						<uniquenessConstraint>
							<xsl:copy-of select="@name"/>
							<xsl:copy-of select="@isPrimary"/>
							<xsl:for-each select="dcl:columnRef">
								<column>
									<xsl:attribute name="name">
										<xsl:value-of select="@name"/>
									</xsl:attribute>
								</column>
							</xsl:for-each>
						</uniquenessConstraint>
				</xsl:for-each>
				</table>
			</xsl:for-each>
		</xsl:variable>
		<!--Reordering the information from the previous variable to place the columns in a more readable order-->
		<!--Removed to preserve origional column order-->
		<!--<xsl:variable name="sortedTables">
			<xsl:for-each select="msxsl:node-set($tables)/table">
				<table>
					<xsl:copy-of select="@*"/>
					<xsl:for-each select="column">
						<xsl:sort select="@isPrimaryKey" order="descending"/>
						<xsl:sort select="@isOptional"/>
						<column>
							<xsl:copy-of select="@*"/>
						</column>
					</xsl:for-each>
					<xsl:copy-of select="uniquenessConstraint"/>
				</table>	
			</xsl:for-each>
		</xsl:variable>-->
		<TView>
			<tables>
				<xsl:copy-of select="$tables"/>
			</tables>
			<FKeys>
				<xsl:copy-of select="$FKeys2"/>
			</FKeys>
		</TView>
	</xsl:template>
</xsl:stylesheet>