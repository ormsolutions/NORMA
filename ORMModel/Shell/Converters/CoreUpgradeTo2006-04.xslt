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
	xmlns:oldCore="http://schemas.neumont.edu/ORM/2006-01/ORMCore"
	xmlns:orm="http://schemas.neumont.edu/ORM/2006-04/ORMCore"
	xmlns:exsl="http://exslt.org/common"
	exclude-result-prefixes="#default xsl oldCore"
	extension-element-prefixes="exsl">
	<xsl:output method="xml" encoding="utf-8" media-type="application/orm+xml" indent="no"/>
	<xsl:template match="@*|*|text()|comment()">
		<xsl:param name="ImpliedInternalConstraintIds"/>
		<xsl:param name="RoleProxyMap"/>
		<xsl:copy>
			<xsl:apply-templates select="@*|*|text()|comment()">
				<xsl:with-param name="ImpliedInternalConstraintIds" select="$ImpliedInternalConstraintIds"/>
				<xsl:with-param name="RoleProxyMap" select="$RoleProxyMap"/>
			</xsl:apply-templates>
		</xsl:copy>
	</xsl:template>
	<xsl:template match="oldCore:*">
		<xsl:param name="ImpliedInternalConstraintIds"/>
		<xsl:param name="RoleProxyMap"/>
		<xsl:element name="orm:{local-name()}">
			<xsl:apply-templates select="@*|*|text()|comment()">
				<xsl:with-param name="ImpliedInternalConstraintIds" select="$ImpliedInternalConstraintIds"/>
				<xsl:with-param name="RoleProxyMap" select="$RoleProxyMap"/>
			</xsl:apply-templates>
		</xsl:element>
	</xsl:template>
	<xsl:template match="oldCore:ORMModel">
		<xsl:variable name="roleProxyMapFragment">
			<xsl:for-each select="oldCore:ExternalConstraints/oldCore:ImpliedEqualityConstraint/oldCore:RoleSequences">
				<xsl:variable name="impliedRoles" select="oldCore:RoleSequence[2]/oldCore:Role/@ref"/>
				<xsl:for-each select="oldCore:RoleSequence[1]/oldCore:Role">
					<xsl:variable name="currentPosition" select="position()"/>
					<RoleProxyMap roleId="{@ref}" proxyId="{$impliedRoles[$currentPosition]}"/>
				</xsl:for-each>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="roleProxyMap" select="exsl:node-set($roleProxyMapFragment)/child::*"/>
		<xsl:variable name="impliedInternalConstraintIds" select="oldCore:Facts/oldCore:ImpliedFact/oldCore:InternalConstraints/child::oldCore:*[oldCore:RoleSequence/oldCore:Role[@ref=$roleProxyMap/@proxyId]]/@id"/>
		<orm:ORMModel>
			<xsl:apply-templates select="@*|*|text()|comment()">
				<xsl:with-param name="ImpliedInternalConstraintIds" select="$impliedInternalConstraintIds"/>
				<xsl:with-param name="RoleProxyMap" select="$roleProxyMap"/>
			</xsl:apply-templates>
			<xsl:if test="not(oldCore:ExternalConstraints)">
				<orm:Constraints>
					<xsl:apply-templates select="oldCore:Facts/child::*/oldCore:InternalConstraints/child::*" mode="MoveInternals">
						<xsl:with-param name="ImpliedInternalConstraintIds" select="$impliedInternalConstraintIds"/>
						<xsl:with-param name="RoleProxyMap" select="$roleProxyMap"/>
					</xsl:apply-templates>
				</orm:Constraints>
			</xsl:if>
		</orm:ORMModel>
	</xsl:template>
	<xsl:template match="oldCore:ExternalConstraints">
		<xsl:param name="ImpliedInternalConstraintIds"/>
		<xsl:param name="RoleProxyMap"/>
		<orm:Constraints>
			<xsl:apply-templates select="@*|*|text()|comment()"/>
			<xsl:apply-templates select="../oldCore:Facts/child::*/oldCore:InternalConstraints/child::*" mode="MoveInternals">
				<xsl:with-param name="ImpliedInternalConstraintIds" select="$ImpliedInternalConstraintIds"/>
				<xsl:with-param name="RoleProxyMap" select="$RoleProxyMap"/>
			</xsl:apply-templates>
		</orm:Constraints>
	</xsl:template>
	<xsl:template match="oldCore:InternalUniquenessConstraint" mode="MoveInternals">
		<xsl:param name="ImpliedInternalConstraintIds"/>
		<xsl:param name="RoleProxyMap"/>
		<xsl:if test="not(@id=$ImpliedInternalConstraintIds)">
			<orm:UniquenessConstraint>
				<xsl:apply-templates select="@*"/>
				<xsl:attribute name="IsInternal">
					<xsl:text>true</xsl:text>
				</xsl:attribute>
				<xsl:apply-templates select="*|text()|comment()">
					<xsl:with-param name="ImpliedInternalConstraintIds" select="$ImpliedInternalConstraintIds"/>
					<xsl:with-param name="RoleProxyMap" select="$RoleProxyMap"/>
				</xsl:apply-templates>
			</orm:UniquenessConstraint>
		</xsl:if>
	</xsl:template>
	<xsl:template match="oldCore:SimpleMandatoryConstraint" mode="MoveInternals">
		<xsl:param name="ImpliedInternalConstraintIds"/>
		<xsl:param name="RoleProxyMap"/>
		<xsl:if test="not(@id=$ImpliedInternalConstraintIds)">
			<orm:MandatoryConstraint>
				<xsl:apply-templates select="@*"/>
				<xsl:attribute name="IsSimple">
					<xsl:text>true</xsl:text>
				</xsl:attribute>
				<xsl:apply-templates select="*|text()|comment()">
					<xsl:with-param name="ImpliedInternalConstraintIds" select="$ImpliedInternalConstraintIds"/>
					<xsl:with-param name="RoleProxyMap" select="$RoleProxyMap"/>
				</xsl:apply-templates>
			</orm:MandatoryConstraint>
		</xsl:if>
	</xsl:template>
	<xsl:template match="oldCore:ImpliedFact/oldCore:FactRoles/oldCore:Role">
		<xsl:param name="ImpliedInternalConstraintIds"/>
		<xsl:param name="RoleProxyMap"/>
		<xsl:variable name="targetId" select="$RoleProxyMap[@proxyId=current()/@id]/@roleId"/>
		<xsl:choose>
			<xsl:when test="$targetId">
				<orm:RoleProxy id="{@id}">
					<orm:Role ref="{$targetId}"/>
				</orm:RoleProxy>
			</xsl:when>
			<xsl:otherwise>
				<orm:Role>
					<xsl:apply-templates select="@*|*|text()|comment()">
						<xsl:with-param name="ImpliedInternalConstraintIds" select="$ImpliedInternalConstraintIds"/>
						<xsl:with-param name="RoleProxyMap" select="$RoleProxyMap"/>
					</xsl:apply-templates>
				</orm:Role>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="oldCore:InternalConstraints/oldCore:SimpleMandatoryConstraint">
		<xsl:param name="ImpliedInternalConstraintIds"/>
		<xsl:if test="not(@id=$ImpliedInternalConstraintIds)">
			<orm:MandatoryConstraint ref="{@id}"/>
		</xsl:if>
	</xsl:template>
	<xsl:template match="oldCore:InternalConstraints/oldCore:InternalUniquenessConstraint">
		<xsl:param name="ImpliedInternalConstraintIds"/>
		<xsl:if test="not(@id=$ImpliedInternalConstraintIds)">
			<orm:UniquenessConstraint ref="{@id}"/>
		</xsl:if>
	</xsl:template>
	<xsl:template match="oldCore:PlayedRoles/oldCore:Role">
		<xsl:param name="ImpliedInternalConstraintIds"/>
		<xsl:param name="RoleProxyMap"/>
		<xsl:if test="not(@ref=$RoleProxyMap/@proxyId)">
			<orm:Role>
				<xsl:apply-templates select="@*|*|text()|comment()">
					<xsl:with-param name="ImpliedInternalConstraintIds" select="$ImpliedInternalConstraintIds"/>
					<xsl:with-param name="RoleProxyMap" select="$RoleProxyMap"/>
				</xsl:apply-templates>
			</orm:Role>
		</xsl:if>
	</xsl:template>
	<xsl:template match="oldCore:ExternalUniquenessConstraint | oldCore:InternalUniquenessConstraint">
		<xsl:param name="ImpliedInternalConstraintIds"/>
		<xsl:param name="RoleProxyMap"/>
		<orm:UniquenessConstraint>
			<xsl:apply-templates select="@*|*|text()|comment()">
				<xsl:with-param name="ImpliedInternalConstraintIds" select="$ImpliedInternalConstraintIds"/>
				<xsl:with-param name="RoleProxyMap" select="$RoleProxyMap"/>
			</xsl:apply-templates>
		</orm:UniquenessConstraint>
	</xsl:template>
	<xsl:template match="oldCore:DisjunctiveMandatoryConstraint">
		<xsl:param name="ImpliedInternalConstraintIds"/>
		<xsl:param name="RoleProxyMap"/>
		<orm:MandatoryConstraint>
			<xsl:apply-templates select="@*|*|text()|comment()">
				<xsl:with-param name="ImpliedInternalConstraintIds" select="$ImpliedInternalConstraintIds"/>
				<xsl:with-param name="RoleProxyMap" select="$RoleProxyMap"/>
			</xsl:apply-templates>
		</orm:MandatoryConstraint>
	</xsl:template>
	<xsl:template match="oldCore:EqualityIsImpliedByMandatoryError">
		<xsl:param name="ImpliedInternalConstraintIds"/>
		<xsl:param name="RoleProxyMap"/>
		<orm:EqualityImpliedByMandatoryError>
			<xsl:apply-templates select="@*|*|text()|comment()">
				<xsl:with-param name="ImpliedInternalConstraintIds" select="$ImpliedInternalConstraintIds"/>
				<xsl:with-param name="RoleProxyMap" select="$RoleProxyMap"/>
			</xsl:apply-templates>
		</orm:EqualityImpliedByMandatoryError>
	</xsl:template>
	<xsl:template match="oldCore:DisjunctiveMandatoryImpliedByMandatoryError">
		<xsl:param name="ImpliedInternalConstraintIds"/>
		<xsl:param name="RoleProxyMap"/>
		<orm:MandatoryImpliedByMandatoryError>
			<xsl:apply-templates select="@*|*|text()|comment()">
				<xsl:with-param name="ImpliedInternalConstraintIds" select="$ImpliedInternalConstraintIds"/>
				<xsl:with-param name="RoleProxyMap" select="$RoleProxyMap"/>
			</xsl:apply-templates>
		</orm:MandatoryImpliedByMandatoryError>
	</xsl:template>
	<xsl:template match="oldCore:MultiColumnConstraint">
		<xsl:param name="ImpliedInternalConstraintIds"/>
		<xsl:param name="RoleProxyMap"/>
		<orm:SetComparisonConstraint>
			<xsl:apply-templates select="@*|*|text()|comment()">
				<xsl:with-param name="ImpliedInternalConstraintIds" select="$ImpliedInternalConstraintIds"/>
				<xsl:with-param name="RoleProxyMap" select="$RoleProxyMap"/>
			</xsl:apply-templates>
		</orm:SetComparisonConstraint>
	</xsl:template>
	<xsl:template match="oldCore:SingleColumnConstraint">
		<xsl:param name="ImpliedInternalConstraintIds"/>
		<xsl:param name="RoleProxyMap"/>
		<orm:SetConstraint>
			<xsl:apply-templates select="@*|*|text()|comment()">
				<xsl:with-param name="ImpliedInternalConstraintIds" select="$ImpliedInternalConstraintIds"/>
				<xsl:with-param name="RoleProxyMap" select="$RoleProxyMap"/>
			</xsl:apply-templates>
		</orm:SetConstraint>
	</xsl:template>
	<!-- Remove implied constraints -->
	<xsl:template match="oldCore:ImpliedEqualityConstraint | oldCore:ImpliedExternalUniquenessConstraint"/>
</xsl:stylesheet>
