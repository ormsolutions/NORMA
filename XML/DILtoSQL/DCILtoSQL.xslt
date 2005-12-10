<?xml version="1.0" encoding="utf-8"?>
<!-- THIS WAS A PROOF OF CONCEPT AND IS NO LONGER IN USE -->
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:dcil="http://schemas.neumont.edu/DIL/DCIL.xsd"
	xmlns:dmil="http://schemas.neumont.edu/DIL/DMIL.xsd"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt">

	<xsl:variable name="DelimitIdentifiers" select="true()"/>

	<xsl:variable name="NewLine" select="'&#x0D;&#x0A;'"/>
	<xsl:variable name="Indent" select="'&#x09;'"/>
	<xsl:variable name="NewLineAndIndent" select="concat($NewLine,$Indent)"/>
	<xsl:variable name="DelimitedIdentifierStart" select="'&quot;'"/>
	<xsl:variable name="DelimitedIdentifierEnd" select="'&quot;'"/>
	<xsl:variable name="StringConstantStart">
		<xsl:text>&apos;</xsl:text>
	</xsl:variable>
	<xsl:variable name="StringConstantEnd">
		<xsl:text>&apos;</xsl:text>
	</xsl:variable>
	<xsl:variable name="StatementTerminator" select="';'"/>
	<xsl:variable name="StatementTerminatorAndNewLine" select="concat($StatementTerminator,$NewLine)"/>
	<xsl:variable name="TableDefinitionItemSeparater" select="','"/>
	<xsl:variable name="ColumnListSeparater" select="','"/>

	<xsl:template name="RenderIdentifier">
		<xsl:param name="name"/>
		<xsl:choose>
			<xsl:when test="$DelimitIdentifiers">
				<xsl:value-of select="$DelimitedIdentifierStart"/>
				<xsl:value-of select="$name"/>
				<xsl:value-of select="$DelimitedIdentifierEnd"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$name"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="RenderIdentifierAsString">
		<xsl:param name="name"/>
		<xsl:value-of select="$StringConstantStart"/>
		<xsl:choose>
			<xsl:when test="$DelimitIdentifiers">
				<xsl:value-of select="$name"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="translate($name,'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')"/>
			</xsl:otherwise>
		</xsl:choose>
		<xsl:value-of select="$StringConstantEnd"/>
	</xsl:template>


	<xsl:template match="dcil:root">
		<xsl:apply-templates/>
	</xsl:template>

	<xsl:template match="dcil:database">
		<xsl:call-template name="RenderCreateDatabase"/>
		<xsl:value-of select="$StatementTerminatorAndNewLine"/>
		<xsl:apply-templates/>
	</xsl:template>

	<xsl:template match="dcil:schema">
		<xsl:call-template name="RenderCreateSchema"/>
		<xsl:value-of select="$StatementTerminatorAndNewLine"/>
		<xsl:call-template name="RenderSetSchema"/>
		<xsl:value-of select="$StatementTerminatorAndNewLine"/>
		<xsl:apply-templates/>
		<xsl:call-template name="RenderCommit"/>
		<xsl:value-of select="$StatementTerminatorAndNewLine"/>
	</xsl:template>

	<xsl:template match="dcil:table">
		<xsl:call-template name="RenderCreateTableStart"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:call-template name="RenderTableChildren"/>
		<xsl:call-template name="RenderCreateTableEnd"/>
		<xsl:value-of select="$StatementTerminatorAndNewLine"/>
	</xsl:template>

	<xsl:template name="RenderTableChildren">
		<xsl:for-each select="child::*">
			<xsl:value-of select="$Indent"/>
			<xsl:apply-templates select="."/>
			<xsl:if test="not(position()=last())">
				<xsl:value-of select="$TableDefinitionItemSeparater"/>
			</xsl:if>
			<xsl:value-of select="$NewLine"/>
		</xsl:for-each>
	</xsl:template>

	<xsl:template match="dcil:column">
		<xsl:call-template name="RenderColumnDefinition"/>
	</xsl:template>
	<xsl:template match="dcil:uniquenessConstraint">
		<xsl:call-template name="RenderUniquenessConstraint"/>
	</xsl:template>
	<xsl:template match="dcil:referenceConstraint">
		<xsl:call-template name="RenderReferenceConstraint"/>
	</xsl:template>
	<xsl:template match="dcil:checkConstraint">
		<xsl:call-template name="RenderCheckConstraint"/>
	</xsl:template>

	<xsl:template name="RenderColumnList">
		<xsl:param name="type"/>
		<xsl:for-each select="child::*">
			<xsl:variable name="name">
				<xsl:choose>
					<xsl:when test="$type='source'">
						<xsl:value-of select="@sourceName"/>
					</xsl:when>
					<xsl:when test="$type='target'">
						<xsl:value-of select="@targetName"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="@name"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:call-template name="RenderIdentifier">
				<xsl:with-param name="name" select="$name"/>
			</xsl:call-template>
			<xsl:if test="not(position()=last())">
				<xsl:value-of select="$ColumnListSeparater"/>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>
	<xsl:template name="RenderSourceColumnList">
		<xsl:call-template name="RenderColumnList">
			<xsl:with-param name="type" select="'source'"/>
		</xsl:call-template>
	</xsl:template>
	<xsl:template name="RenderTargetColumnList">
		<xsl:call-template name="RenderColumnList">
			<xsl:with-param name="type" select="'target'"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template match="dcil:trigger">
		<xsl:call-template name="RenderCreateTrigger"/>
		<xsl:value-of select="$StatementTerminatorAndNewLine"/>
	</xsl:template>

</xsl:stylesheet>
