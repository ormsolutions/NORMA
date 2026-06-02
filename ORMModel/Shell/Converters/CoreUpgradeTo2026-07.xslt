<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:oldOrm="http://schemas.neumont.edu/ORM/2006-04/ORMCore"
	xmlns:orm="http://schemas.neumont.edu/ORM/2026-07/ORMCore"
	xmlns:exsl="http://exslt.org/common"
	exclude-result-prefixes="#default xsl oldOrm"
	extension-element-prefixes="exsl">
	
	<!-- Implementation note: The beginning of this file will basically be the same
	for any extension upgrade that is not registered as a 'runWith' transform. The root
	element needs to be modified to get the correct prefix, and the individual elements
	need to be upgraded to the new namespace. -->
	<xsl:variable name="targetPrefix" select="'orm'"/>
	<xsl:variable name="targetNamespace" select="'http://schemas.neumont.edu/ORM/2026-07/ORMCore'"/>
	<xsl:variable name="replaceNamespace" select="'http://schemas.neumont.edu/ORM/2006-04/ORMCore'"/>
	<xsl:key name="SubtypeRefIds" match="*/oldOrm:ORMModel/oldOrm:Facts/oldOrm:SubtypeFact/oldOrm:FactRoles/oldOrm:SubtypeMetaRole/oldOrm:RolePlayer" use="@ref"/>
	<xsl:template match="*">
		<xsl:element name="{name()}" namespace="{namespace-uri()}">
			<xsl:apply-templates select="@*"/>
			<xsl:apply-templates select="node()"/>
		</xsl:element>
	</xsl:template>
	<xsl:template match="@*|text()|comment()">
		<xsl:copy/>
	</xsl:template>
	<xsl:template match="oldOrm:*">
		<xsl:element name="orm:{local-name()}">
			<xsl:apply-templates select="@*|*|text()|comment()"/>
		</xsl:element>
	</xsl:template>
	<xsl:template match="oldOrm:Facts">
		<orm:DomainFactTypes>
			<xsl:apply-templates select="@*|*|text()|comment()"/>
		</orm:DomainFactTypes>
	</xsl:template>
	<xsl:template match="oldOrm:Fact">
		<orm:FactType>
			<xsl:apply-templates select="@*|*|text()|comment()"/>
		</orm:FactType>
	</xsl:template>
	<xsl:template
		match="oldOrm:ObjectTypeRequiresPrimarySupertypeError
		|oldOrm:FrequencyConstraintContradictsInternalUniquenessConstraintError
		|oldOrm:EqualityImpliedByMandatoryError
		|oldOrm:SubtypeFact/oldOrm:DerivationRule|oldOrm:ImpliedFact/oldOrm:DerivationRule
		|oldOrm:CorrelatedPathRoleRequiresCompatibleRolePlayerError
		|oldOrm:PathStartRoleFollowsRootObjectTypeError">
		<!-- Deprecated element in either the model or extraneous in prior xsd, remove. -->
	</xsl:template>
	<xsl:template match="oldOrm:SubtypeFact/@IsPrimary">
		<xsl:if test=".='true' or .='1'">
			<xsl:attribute name="PreferredIdentificationPath">
				<xsl:text>true</xsl:text>
			</xsl:attribute>
		</xsl:if>
	</xsl:template>
	<xsl:template match="oldOrm:SubtypeFact">
		<orm:SubtypingFactType>
			<xsl:apply-templates select="@*|*|text()|comment()"/>
		</orm:SubtypingFactType>
	</xsl:template>
	<xsl:template match="oldOrm:ImpliedFact">
		<orm:ImpliedFactType>
			<xsl:apply-templates select="@*|*|text()|comment()"/>
		</orm:ImpliedFactType>
	</xsl:template>
	<xsl:template match="oldOrm:Objects">
		<orm:DomainObjectTypes>
			<xsl:apply-templates select="@*|*|text()|comment()"/>
		</orm:DomainObjectTypes>
	</xsl:template>
	<xsl:template match="oldOrm:FactRoles|oldOrm:QueryRoles">
		<orm:Roles>
			<xsl:apply-templates select="@*|*|text()|comment()"/>
		</orm:Roles>
	</xsl:template>
	<xsl:template match="oldOrm:SubtypeMetaRole">
		<orm:SubtypeRole>
			<xsl:apply-templates select="@*|*|text()|comment()"/>
		</orm:SubtypeRole>
	</xsl:template>
	<xsl:template match="oldOrm:SupertypeMetaRole">
		<orm:SupertypeRole>
			<xsl:apply-templates select="@*|*|text()|comment()"/>
		</orm:SupertypeRole>
	</xsl:template>
	<xsl:template match="oldOrm:NestedPredicate">
		<orm:Objectification>
			<xsl:apply-templates select="@*|*|text()|comment()"/>
		</orm:Objectification>
	</xsl:template>

	<!-- Move SubtypeDerivationRules/SubtypeDerivationExpression data to SubtypeDervationPath in the next -->
	<xsl:template match="oldOrm:Objects/oldOrm:*/oldOrm:SubtypeDerivationRule/oldOrm:SubtypeDerivationPath">
		<xsl:variable name="derivationExpressions" select="../oldOrm:SubtypeDerivationExpression | key('SubtypeRefIds',../../@id)/../../../oldOrm:DerivationRule/oldOrm:DerivationExpression"/>
		<orm:SubtypeDerivationPath>
			<!-- Use completeness and storage attributes from existing path, ignore the DerivationStorage in the expression. -->
			<xsl:apply-templates select="@*"/>
			<xsl:choose>
				<xsl:when test="$derivationExpressions">
					<xsl:variable name="currentRuleBody" select="oldOrm:InformalRule/oldOrm:DerivationNote/oldOrm:Body"/>
					<xsl:choose>
						<xsl:when test="$currentRuleBody">
							<orm:InformalRule>
								<orm:DerivationNote id="{$currentRuleBody/../@id}">
									<orm:Body>
										<xsl:value-of select="$currentRuleBody"/>
										<xsl:for-each select="$derivationExpressions">
											<xsl:text>&#x0D;&#x0A;</xsl:text>
											<xsl:value-of select="oldOrm:Body"/>
										</xsl:for-each>
									</orm:Body>
								</orm:DerivationNote>
							</orm:InformalRule>
						</xsl:when>
						<xsl:otherwise>
							<orm:InformalRule>
								<orm:DerivationNote id="{@id}_informal">
									<orm:Body>
										<xsl:for-each select="$derivationExpressions">
											<xsl:if test="position()&gt;1">
												<xsl:text>&#x0D;&#x0A;</xsl:text>
											</xsl:if>
											<xsl:value-of select="oldOrm:Body"/>
										</xsl:for-each>
									</orm:Body>
								</orm:DerivationNote>
							</orm:InformalRule>
						</xsl:otherwise>
					</xsl:choose>
					<xsl:apply-templates select="child::*[not(self::oldOrm:InformalRule)]|comment()"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:apply-templates select="*|comment()"/>
				</xsl:otherwise>
			</xsl:choose>
		</orm:SubtypeDerivationPath>
	</xsl:template>
	<xsl:template name="ExpandDerivationStorageAttributes">
		<xsl:param name="expressions"/>
		<!-- If we have a derivation storage then map to corresponding attributes. This was only shown as being available on the SubtypeFact expressions.-->
		<xsl:variable name="derivationStorage" select="string($expressions/@DerivationStorage)"/>
		<xsl:if test="$derivationStorage">
			<!-- Defaults are FullyDerived and NotStored, corresponding to 'Derived'. Pick up other values. -->
			<xsl:choose>
				<xsl:when test="$derivationStorage='DerivedAndStored'">
					<xsl:attribute name="DerivationStorage">
						<xsl:text>Stored</xsl:text>
					</xsl:attribute>
				</xsl:when>
				<xsl:when test="$derivationStorage='PartiallyDerived'">
					<xsl:attribute name="DerivationCompleteness">
						<xsl:text>PartiallyDerived</xsl:text>
					</xsl:attribute>
				</xsl:when>
				<xsl:when test="$derivationStorage='PartiallyDerivedAndStored'">
					<xsl:attribute name="DerivationStorage">
						<xsl:text>Stored</xsl:text>
					</xsl:attribute>
					<xsl:attribute name="DerivationCompleteness">
						<xsl:text>PartiallyDerived</xsl:text>
					</xsl:attribute>
				</xsl:when>
			</xsl:choose>
		</xsl:if>
	</xsl:template>
	<xsl:template match="oldOrm:Objects/oldOrm:*/oldOrm:SubtypeDerivationRule/oldOrm:SubtypeDerivationExpression">
		<!-- The derivation expression is picked up previously if there is a derivation path. We create a path if there isn't one already, but we're abandoning the expression. -->
		<xsl:if test="not(../oldOrm:SubtypeDerivationPath)">
			<orm:SubtypeDerivationPath id="{../../@id}_path" ExternalDerivation="true">
				<xsl:variable name="subtypingExpressions" select="key('SubtypeRefIds', ../../@id)/../../../oldOrm:DerivationRule/oldOrm:DerivationExpression"/>
				<xsl:call-template name="ExpandDerivationStorageAttributes">
					<xsl:with-param name="expressions" select="$subtypingExpressions"/>
				</xsl:call-template>
				<orm:InformalRule>
					<orm:DerivationNote id="{@id}">
						<orm:Body>
							<xsl:value-of select="oldOrm:Body"/>
							<xsl:for-each select="$subtypingExpressions">
								<xsl:text>&#x0D;&#x0A;</xsl:text>
								<xsl:value-of select="oldOrm:Body"/>
							</xsl:for-each>
						</orm:Body>
					</orm:DerivationNote>
				</orm:InformalRule>
			</orm:SubtypeDerivationPath>
		</xsl:if>
	</xsl:template>
	<xsl:template match="oldOrm:Objects/oldOrm:*">
		<xsl:element name="orm:{local-name()}">
			<xsl:apply-templates select="@*|*|text()|comment()"/>
			<xsl:if test="not(oldOrm:SubtypeDerivationRule)">
				<xsl:variable name="subtypingExpressions" select="key('SubtypeRefIds', @id)/../../../oldOrm:DerivationRule/oldOrm:DerivationExpression"/>
				<xsl:if test="$subtypingExpressions">
					<!-- Mockup a subtype derivation rule to hold an informal note. -->
					<orm:SubtypeDerivationRule>
						<orm:SubtypeDerivationPath id="{@id}_path" ExternalDerivation="true">
							<xsl:call-template name="ExpandDerivationStorageAttributes">
								<xsl:with-param name="expressions" select="$subtypingExpressions"/>
							</xsl:call-template>
							<orm:InformalRule>
								<orm:DerivationNote id="{string($subtypingExpressions[1]/@id)}">
									<orm:Body>
										<xsl:for-each select="$subtypingExpressions">
											<xsl:if test="position()&gt;1">
												<xsl:text>&#x0D;&#x0A;</xsl:text>
											</xsl:if>
											<xsl:value-of select="oldOrm:Body"/>
										</xsl:for-each>
									</orm:Body>
								</orm:DerivationNote>
							</orm:InformalRule>
						</orm:SubtypeDerivationPath>
					</orm:SubtypeDerivationRule>
				</xsl:if>
			</xsl:if>
		</xsl:element>
	</xsl:template>
	<xsl:template match="oldOrm:Facts/oldOrm:Fact/oldOrm:DerivationRule/oldOrm:DerivationExpression">
		<!-- The derivation expression is picked up previously if there is a derivation path. We create a path if there isn't one already, but we're abandoning the expression. -->
		<xsl:if test="not(../oldOrm:FactTypeDerivationPath)">
			<orm:FactTypeDerivationPath id="{../../@id}_path" ExternalDerivation="true">
				<xsl:call-template name="ExpandDerivationStorageAttributes">
					<xsl:with-param name="expressions" select="."/>
				</xsl:call-template>
				<orm:InformalRule>
					<orm:DerivationNote id="{@id}">
						<orm:Body>
							<xsl:value-of select="oldOrm:Body"/>
						</orm:Body>
					</orm:DerivationNote>
				</orm:InformalRule>
			</orm:FactTypeDerivationPath>
		</xsl:if>
	</xsl:template>
	<xsl:template match="oldOrm:Facts/oldOrm:Fact/oldOrm:DerivationRule/oldOrm:FactTypeDerivationPath">
		<xsl:variable name="derivationExpression" select="../oldOrm:DerivationExpression"/>
		<orm:FactTypeDerivationPath>
			<!-- Use completeness and storage attributes from existing path, ignore the DerivationStorage value in the expression. -->
			<xsl:apply-templates select="@*"/>
			<xsl:choose>
				<xsl:when test="$derivationExpression">
					<xsl:variable name="currentRuleBody" select="oldOrm:InformalRule/oldOrm:DerivationNote/oldOrm:Body"/>
					<xsl:choose>
						<xsl:when test="$currentRuleBody">
							<orm:InformalRule>
								<orm:DerivationNote id="{$currentRuleBody/../@id}">
									<orm:Body>
										<xsl:value-of select="$currentRuleBody"/>
										<xsl:text>&#x0D;&#x0A;</xsl:text>
										<xsl:value-of select="$derivationExpression/oldOrm:Body"/>
									</orm:Body>
								</orm:DerivationNote>
							</orm:InformalRule>
						</xsl:when>
						<xsl:otherwise>
							<orm:InformalRule>
								<orm:DerivationNote id="{$derivationExpression/@id}">
									<orm:Body>
										<xsl:value-of select="oldOrm:Body"/>
									</orm:Body>
								</orm:DerivationNote>
							</orm:InformalRule>
						</xsl:otherwise>
					</xsl:choose>
					<xsl:apply-templates select="child::*[not(self::oldOrm:InformalRule)]|comment()"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:apply-templates select="*|comment()"/>
				</xsl:otherwise>
			</xsl:choose>
		</orm:FactTypeDerivationPath>
	</xsl:template>
</xsl:stylesheet>
