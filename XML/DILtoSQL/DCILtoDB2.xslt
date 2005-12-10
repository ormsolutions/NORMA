<?xml version="1.0" encoding="utf-8"?>
<!-- THIS WAS A PROOF OF CONCEPT AND IS NO LONGER IN USE -->
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:dcil="http://schemas.neumont.edu/DIL/DCIL.xsd"
	xmlns:dmil="http://schemas.neumont.edu/DIL/DMIL.xsd"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt">

	<xsl:import href="DCILtoSQL.xslt"/>
	<xsl:output method="text" encoding="utf-8" indent="no"/>
	<xsl:strip-space elements="*"/>

	<xsl:template match="dcil:predefinedDataType">
		<xsl:value-of select="@name"/>
		<xsl:if test="string-length(@precision)">
			<xsl:text>(</xsl:text>
			<xsl:value-of select="@precision"/>
			<xsl:if test="string-length(@scale)">
				<xsl:text>,</xsl:text>
				<xsl:value-of select="@scale"/>
			</xsl:if>
			<xsl:text>)</xsl:text>
		</xsl:if>
	</xsl:template>

	<xsl:template match="dcil:domainDataType">
		<!-- TODO: Handle domainDataType -->
	</xsl:template>

	<xsl:template name="RenderCreateDatabase">
		<xsl:text>CREATE DATABASE </xsl:text>
		<xsl:call-template name="RenderIdentifier">
			<xsl:with-param name="name" select="@name"/>
		</xsl:call-template>
		<xsl:text> USING CODESET UTF-8 TERRITORY US COLLATE USING UCA400_NO</xsl:text>
	</xsl:template>

	<xsl:template name="RenderCreateSchema">
		<xsl:text>CREATE SCHEMA </xsl:text>
		<xsl:call-template name="RenderIdentifier">
			<xsl:with-param name="name" select="@name"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template name="RenderCommit">
		<xsl:text>COMMIT WORK</xsl:text>
	</xsl:template>

	<xsl:template name="RenderSetSchema">
		<xsl:text>SET SCHEMA </xsl:text>
		<xsl:call-template name="RenderIdentifierAsString">
			<xsl:with-param name="name" select="@name"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template name="RenderCreateTableStart">
		<xsl:text>CREATE TABLE </xsl:text>
		<xsl:call-template name="RenderIdentifier">
			<xsl:with-param name="name" select="@name"/>
		</xsl:call-template>
		<xsl:value-of select="$NewLine"/>
		<xsl:text>(</xsl:text>
	</xsl:template>
	<xsl:template name="RenderCreateTableEnd">
		<xsl:text>)</xsl:text>
	</xsl:template>

	<xsl:template name="RenderColumnDefinition">
		<xsl:call-template name="RenderIdentifier">
			<xsl:with-param name="name" select="@name"/>
		</xsl:call-template>
		<xsl:value-of select="'&#x20;'"/>
		<xsl:apply-templates/>
		<xsl:if test="not(@isNullable='true')">
			<xsl:text> NOT NULL</xsl:text>
		</xsl:if>
		<xsl:choose>
			<xsl:when test="@isIdentity='true'">
				<xsl:text> GENERATED ALWAYS AS IDENTITY</xsl:text>
			</xsl:when>
			<xsl:when test="dcil:generationCode">
				<!-- TODO: Handle generation code -->
			</xsl:when>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="RenderUniquenessConstraint">
		<xsl:text>CONSTRAINT </xsl:text>
		<xsl:call-template name="RenderIdentifier">
			<xsl:with-param name="name" select="@name"/>
		</xsl:call-template>
		<xsl:choose>
			<xsl:when test="@isPrimary='true'">
				<xsl:text> PRIMARY KEY</xsl:text>
			</xsl:when>
			<xsl:otherwise>
				<xsl:text> UNIQUE</xsl:text>
			</xsl:otherwise>
		</xsl:choose>
		<xsl:text>(</xsl:text>
		<xsl:call-template name="RenderColumnList"/>
		<xsl:text>)</xsl:text>
	</xsl:template>

	<xsl:template name="RenderReferenceConstraint">
		<xsl:text>CONSTRAINT </xsl:text>
		<xsl:call-template name="RenderIdentifier">
			<xsl:with-param name="name" select="@name"/>
		</xsl:call-template>
		<xsl:text> FOREIGN KEY(</xsl:text>
		<xsl:call-template name="RenderSourceColumnList"/>
		<xsl:text>) REFERENCES </xsl:text>
		<xsl:call-template name="RenderIdentifier">
			<xsl:with-param name="name" select="@targetTable"/>
		</xsl:call-template>
		<xsl:text>(</xsl:text>
		<xsl:call-template name="RenderTargetColumnList"/>
		<xsl:text>)</xsl:text>
	</xsl:template>

	<xsl:template name="RenderCheckConstraint">
		<xsl:text>CONSTRAINT </xsl:text>
		<xsl:call-template name="RenderIdentifier">
			<xsl:with-param name="name" select="@name"/>
		</xsl:call-template>
		<xsl:text> CHECK(</xsl:text>
		<!-- TODO: Process DMIL content here -->
		<xsl:value-of select="dmil:literalCode/child::text()"/>
		<xsl:text>)</xsl:text>
	</xsl:template>

	<xsl:template name="RenderCreateTrigger">
		<xsl:variable name="level" select="@level"/>
		<xsl:text>CREATE TRIGGER </xsl:text>
		<xsl:call-template name="RenderIdentifier">
			<xsl:with-param name="name" select="@name"/>
		</xsl:call-template>
		<xsl:value-of select="$NewLineAndIndent"/>
		<xsl:value-of select="@actionTime"/>
		<xsl:value-of select="'&#x20;'"/>
		<xsl:value-of select="@event"/>
		<xsl:for-each select="dcil:columns">
			<xsl:text> OF </xsl:text>
			<xsl:call-template name="RenderColumnList"/>
		</xsl:for-each>
		<xsl:text> ON </xsl:text>
		<xsl:call-template name="RenderIdentifier">
			<xsl:with-param name="name" select="@targetTable"/>
		</xsl:call-template>
		<xsl:value-of select="$NewLineAndIndent"/>
		<xsl:for-each select="dcil:referencing">
			<xsl:text>REFERENCING </xsl:text>
			<xsl:copy-of select="@state"/>
			<xsl:choose>
				<xsl:when test="$level='ROW'">
					<xsl:text>ROW</xsl:text>
				</xsl:when>
				<xsl:when test="$level='STATEMENT'">
					<xsl:text>TABLE</xsl:text>
				</xsl:when>
			</xsl:choose>
			<xsl:text> AS </xsl:text>
			<xsl:call-template name="RenderIdentifier">
				<xsl:with-param name="name" select="@name"/>
			</xsl:call-template>
			<xsl:value-of select="$NewLineAndIndent"/>
		</xsl:for-each>
		<xsl:text>FOR EACH </xsl:text>
		<xsl:value-of select="$level"/>
		<xsl:if test="dcil:when">
			<xsl:value-of select="$NewLineAndIndent"/>
			<xsl:text>WHEN</xsl:text>
			<xsl:value-of select="$NewLineAndIndent"/>
			<xsl:text>(</xsl:text>
			<xsl:value-of select="$NewLineAndIndent"/>
			<xsl:value-of select="$Indent"/>
			<!-- Process DMIL code here. -->
			<xsl:value-of select="$NewLineAndIndent"/>
			<xsl:text>)</xsl:text>
		</xsl:if>
		<xsl:value-of select="$NewLine"/>
		<!-- Process DMIL code here. -->
	</xsl:template>

</xsl:stylesheet>
