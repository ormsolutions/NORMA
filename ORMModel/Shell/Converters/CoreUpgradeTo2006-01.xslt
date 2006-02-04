<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:oldCore="http://schemas.neumont.edu/ORM/ORMCore"
	xmlns:orm="http://schemas.neumont.edu/ORM/2006-01/ORMCore"
	exclude-result-prefixes="#default xsl oldCore">
	<xsl:template match="@*|*|text()|comment()">
		<xsl:copy>
			<xsl:apply-templates select="@*|*|text()|comment()"/>
		</xsl:copy>
	</xsl:template>
	<xsl:template match="oldCore:*">
		<xsl:param name="SubtypeRoles"/>
		<xsl:param name="SupertypeRoles"/>
		<xsl:element name="orm:{local-name()}">
			<xsl:apply-templates select="@*|*|text()|comment()">
				<xsl:with-param name="SubtypeRoles" select="$SubtypeRoles"/>
				<xsl:with-param name="SupertypeRoles" select="$SupertypeRoles"/>
			</xsl:apply-templates>
		</xsl:element>
	</xsl:template>
	<xsl:template match="oldCore:ORMModel">
		<xsl:variable name="SubtypeFactRoles" select="oldCore:Facts/oldCore:SubtypeFact/oldCore:FactRoles"/>
		<xsl:element name="orm:{local-name()}">
			<xsl:apply-templates select="@*|*|text()|comment()">
				<xsl:with-param name="SubtypeRoles" select="$SubtypeFactRoles/oldCore:Role[1]"/>
				<xsl:with-param name="SupertypeRoles" select="$SubtypeFactRoles/oldCore:Role[2]"/>
			</xsl:apply-templates>
		</xsl:element>
	</xsl:template>
	<xsl:template name="DuplicateRoleElement">
		<xsl:param name="RoleId" select="@id"/>
		<xsl:param name="SubtypeRoles"/>
		<xsl:param name="SupertypeRoles"/>
		<xsl:variable name="elementName">
			<xsl:variable name="SubtypeRole" select="$SubtypeRoles[@id=$RoleId]"/>
			<xsl:choose>
				<xsl:when test="$SubtypeRole">
					<xsl:text>SubtypeMetaRole</xsl:text>
				</xsl:when>
				<xsl:otherwise>
					<xsl:variable name="SupertypeRole" select="$SupertypeRoles[@id=$RoleId]"/>
					<xsl:choose>
						<xsl:when test="$SupertypeRole">
							<xsl:text>SupertypeMetaRole</xsl:text>
						</xsl:when>
						<xsl:otherwise>
							<xsl:text>Role</xsl:text>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:element name="orm:{$elementName}">
			<xsl:apply-templates select="@*|*|text()|comment()">
				<xsl:with-param name="SubtypeRoles" select="$SubtypeRoles"/>
				<xsl:with-param name="SupertypeRoles" select="$SupertypeRoles"/>
			</xsl:apply-templates>
		</xsl:element>
	</xsl:template>
	<xsl:template match="oldCore:PlayedRoles/oldCore:Role">
		<xsl:param name="SubtypeRoles"/>
		<xsl:param name="SupertypeRoles"/>
		<xsl:call-template name="DuplicateRoleElement">
			<xsl:with-param name="RoleId" select="@ref"/>
			<xsl:with-param name="SubtypeRoles" select="$SubtypeRoles"/>
			<xsl:with-param name="SupertypeRoles" select="$SupertypeRoles"/>
		</xsl:call-template>
	</xsl:template>
	<xsl:template match="oldCore:SubtypeFact/oldCore:FactRoles/oldCore:Role">
		<xsl:param name="SubtypeRoles"/>
		<xsl:param name="SupertypeRoles"/>
		<xsl:call-template name="DuplicateRoleElement">
			<xsl:with-param name="SubtypeRoles" select="$SubtypeRoles"/>
			<xsl:with-param name="SupertypeRoles" select="$SupertypeRoles"/>
		</xsl:call-template>
	</xsl:template>
	<xsl:template match="oldCore:ValueType/oldCore:ValueConstraint/oldCore:ValueRangeDefinition">
		<xsl:param name="SubtypeRoles"/>
		<xsl:param name="SupertypeRoles"/>
		<orm:ValueConstraint>
			<xsl:if test="not(@Name)">
				<xsl:attribute name="Name"/>
			</xsl:if>
			<xsl:apply-templates select="@*|*|text()|comment()">
				<xsl:with-param name="SubtypeRoles" select="$SubtypeRoles"/>
				<xsl:with-param name="SupertypeRoles" select="$SupertypeRoles"/>
			</xsl:apply-templates>
		</orm:ValueConstraint>
	</xsl:template>
	<xsl:template match="oldCore:Role/oldCore:ValueConstraint/oldCore:RoleValueRangeDefinition">
		<xsl:param name="SubtypeRoles"/>
		<xsl:param name="SupertypeRoles"/>
		<orm:RoleValueConstraint>
			<xsl:if test="not(@Name)">
				<xsl:attribute name="Name"/>
			</xsl:if>
			<xsl:apply-templates select="@*|*|text()|comment()">
				<xsl:with-param name="SubtypeRoles" select="$SubtypeRoles"/>
				<xsl:with-param name="SupertypeRoles" select="$SupertypeRoles"/>
			</xsl:apply-templates>
		</orm:RoleValueConstraint>
	</xsl:template>
	<xsl:template match="oldCore:ValueConstraint">
		<xsl:param name="SubtypeRoles"/>
		<xsl:param name="SupertypeRoles"/>
		<orm:ValueRestriction>
			<xsl:apply-templates select="@*|*|text()|comment()">
				<xsl:with-param name="SubtypeRoles" select="$SubtypeRoles"/>
				<xsl:with-param name="SupertypeRoles" select="$SupertypeRoles"/>
			</xsl:apply-templates>
		</orm:ValueRestriction>
	</xsl:template>
	<xsl:template match="oldCore:SimpleMandatoryImpliesDisjunctiveMandatoryError">
		<xsl:param name="SubtypeRoles"/>
		<xsl:param name="SupertypeRoles"/>
		<orm:DisjunctiveMandatoryImpliedByMandatoryError>
			<xsl:apply-templates select="@*|*|text()|comment()">
				<xsl:with-param name="SubtypeRoles" select="$SubtypeRoles"/>
				<xsl:with-param name="SupertypeRoles" select="$SupertypeRoles"/>
			</xsl:apply-templates>
		</orm:DisjunctiveMandatoryImpliedByMandatoryError>
	</xsl:template>
	<!-- RoleSequence names not needed on multicolumn externals, eliminate them here -->
	<xsl:template match="oldCore:RoleSequences/oldCore:RoleSequence/@Name"/>
</xsl:stylesheet>