<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet
	version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:se="http://NetTiers/2.2/SchemaExplorer"
	xmlns:exsl="http://exslt.org/common"
	xmlns:dcl="http://schemas.orm.net/DIL/DCIL"
	extension-element-prefixes="exsl"
	exclude-result-prefixes="xsl se">
	<xsl:output method="xml" indent="no"/>
	<xsl:template match="dcl:schema">
		<xsl:variable name="domains" select="dcl:domain"/>
		<se:databaseSchema name="{@name}">
			<xsl:variable name="tables" select="dcl:table"/>
			<xsl:if test="$tables">
				<se:tables>
					<xsl:apply-templates select="$tables">
						<xsl:with-param name="domains" select="$domains"/>
					</xsl:apply-templates>
				</se:tables>
			</xsl:if>
		</se:databaseSchema>
	</xsl:template>
	<xsl:template match="dcl:table">
		<xsl:param name="domains"/>
		<se:table name="{translate(@name,'&quot;','')}" owner="{../@name}">
			<xsl:variable name="columns" select="dcl:column"/>
			<xsl:variable name="uniquenessConstraints" select="dcl:uniquenessConstraint"/>
			<xsl:variable name="primaryColumnNames" select="$uniquenessConstraints[@isPrimary='true' or @isPrimary='1']/dcl:columnRef/@name"/>
			<xsl:if test="$columns">
				<se:columns>
					<xsl:apply-templates select="$columns[@name=$primaryColumnNames]">
						<xsl:sort select="@name"/>
						<xsl:with-param name="domains" select="$domains"/>
					</xsl:apply-templates>
					<xsl:apply-templates select="$columns[@name[not(.=$primaryColumnNames)]=$uniquenessConstraints[not(@isPrimary='true' or @isPrimary='1')]/dcl:columnRef/@name]">
						<xsl:sort select="@name"/>
						<xsl:with-param name="domains" select="$domains"/>
					</xsl:apply-templates>
					<xsl:apply-templates select="$columns[not(@name=$uniquenessConstraints/dcl:columnRef/@name)]">
						<xsl:sort select="@name"/>
						<xsl:with-param name="domains" select="$domains"/>
					</xsl:apply-templates>
				</se:columns>
				<xsl:if test="$uniquenessConstraints">
					<se:indexes>
						<xsl:for-each select="$uniquenessConstraints">
							<se:index name="{translate(@name,'&quot;','')}">
								<xsl:if test="@isPrimary='true' or @isPrimary=1">
									<xsl:attribute name="isPrimary">
										<xsl:text>true</xsl:text>
									</xsl:attribute>
								</xsl:if>
								<xsl:for-each select="dcl:columnRef">
									<se:column ref="{translate(@name,'&quot;','')}"/>
								</xsl:for-each>
							</se:index>
						</xsl:for-each>
					</se:indexes>
				</xsl:if>
				<xsl:variable name="foreignKeys" select="dcl:referenceConstraint"/>
				<xsl:if test="$foreignKeys">
					<se:keys>
						<xsl:for-each select="$foreignKeys">
							<se:key name="{translate(@name,'&quot;','')}" targetTable="{translate(@targetTable,'&quot;','')}" targetOwner="{../../@name}">
								<xsl:for-each select="dcl:columnRef">
									<se:columnReference column="{@sourceName}" targetColumn="{@targetName}"/>
								</xsl:for-each>
							</se:key>
						</xsl:for-each>
					</se:keys>
				</xsl:if>
			</xsl:if>
		</se:table>
	</xsl:template>
	<xsl:template match="dcl:column">
		<xsl:param name="domains"/>
		<se:column name="{translate(@name,'&quot;','')}">
			<xsl:if test="@isNullable='true' or @isNullable='1'">
				<xsl:attribute name="nullable">
					<xsl:text>true</xsl:text>
				</xsl:attribute>
			</xsl:if>
			<xsl:if test="@isIdentity='true' or @isIdentity='1'">
				<xsl:attribute name="isIdentity">
					<xsl:text>true</xsl:text>
				</xsl:attribute>
			</xsl:if>
			<xsl:variable name="predefinedType" select="dcl:predefinedDataType"/>
			<xsl:choose>
				<xsl:when test="$predefinedType">
					<xsl:call-template name="ProcessDataType">
						<xsl:with-param name="dataType" select="$predefinedType"/>
					</xsl:call-template>
				</xsl:when>
				<xsl:otherwise>
					<xsl:call-template name="ProcessDataType">
						<xsl:with-param name="dataType" select="$domains[@name=current()/dcl:domainRef/@name]/dcl:predefinedDataType"/>
					</xsl:call-template>
				</xsl:otherwise>
			</xsl:choose>
		</se:column>
	</xsl:template>
	<xsl:template name="ProcessDataType">
		<xsl:param name="dataType"/>
		<xsl:for-each select="$dataType">
			<xsl:variable name="dclName" select="string(@name)"/>
			<xsl:attribute name="nativeType">
				<xsl:choose>
					<xsl:when test="$dclName='CHARACTER LARGE OBJECT'">
						<xsl:text>nvarchar</xsl:text> <!-- UNDONE: character large object-->
					</xsl:when>
					<xsl:when test="$dclName='CHARACTER'">
						<xsl:text>nchar</xsl:text>
					</xsl:when>
					<xsl:when test="$dclName='CHARACTER VARYING'">
						<xsl:text>nvarchar</xsl:text>
					</xsl:when>
					<xsl:when test="$dclName='BINARY LARGE OBJECT'">
						<xsl:text>varbinary</xsl:text> <!-- UNDONE: binary large object -->
					</xsl:when>
					<xsl:when test="$dclName='NUMERIC'">
						<xsl:text>numeric</xsl:text>
					</xsl:when>
					<xsl:when test="$dclName='DECIMAL'">
						<xsl:text>decimal</xsl:text>
					</xsl:when>
					<xsl:when test="$dclName='SMALLINT'">
						<xsl:text>smallint</xsl:text>
					</xsl:when>
					<xsl:when test="$dclName='INTEGER'">
						<xsl:text>int</xsl:text>
					</xsl:when>
					<xsl:when test="$dclName='BIGINT'">
						<xsl:text>bigint</xsl:text>
					</xsl:when>
					<xsl:when test="$dclName='FLOAT'">
						<xsl:text>float</xsl:text>
					</xsl:when>
					<xsl:when test="$dclName='REAL'">
						<xsl:text>real</xsl:text>
					</xsl:when>
					<xsl:when test="$dclName='DOUBLE PRECISION'">
						<xsl:text>double precision</xsl:text>
					</xsl:when>
					<xsl:when test="$dclName='BOOLEAN'">
						<xsl:text>bit</xsl:text>
					</xsl:when>
					<xsl:when test="$dclName='DATE'">
						<xsl:text>datetime</xsl:text>
					</xsl:when>
					<xsl:when test="$dclName='TIME'">
						<xsl:text>datetime</xsl:text>
					</xsl:when>
					<xsl:when test="$dclName='TIMESTAMP'">
						<xsl:text>timestamp</xsl:text>
					</xsl:when>
					<xsl:when test="$dclName='INTERVAL'">
						<xsl:text>interval</xsl:text>
					</xsl:when>
				</xsl:choose>
			</xsl:attribute>
			<xsl:if test="@length">
				<xsl:attribute name="size">
					<xsl:value-of select="@length"/>
				</xsl:attribute>
			</xsl:if>
			<xsl:if test="@precision">
				<xsl:attribute name="precision">
					<xsl:value-of select="@precision"/>
				</xsl:attribute>
			</xsl:if>
			<xsl:if test="@scale">
				<xsl:attribute name="scale">
					<xsl:value-of select="@scale"/>
				</xsl:attribute>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>
</xsl:stylesheet>