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
	<xsl:key name="EntityTypeRoleInstanceRefs" match="*/oldOrm:ORMModel/oldOrm:Objects/oldOrm:*[self::oldOrm:EntityType or self::oldOrm:ObjectifiedType]/oldOrm:Instances/oldOrm:EntityTypeInstance/oldOrm:RoleInstances/oldOrm:EntityTypeRoleInstance" use="@ref"/>
	<xsl:key name="EntityTypeUnaryRoleInstanceRefs" match="*/oldOrm:ORMModel/oldOrm:Objects/oldOrm:*[self::oldOrm:EntityType or self::oldOrm:ObjectifiedType]/oldOrm:Instances/oldOrm:EntityTypeInstance/oldOrm:RoleInstances/oldOrm:EntityTypeUnaryRoleInstance" use="@ref"/>
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
	<xsl:template match="oldOrm:ImpliedFact">
		<orm:ImpliedFactType>
			<xsl:apply-templates select="@*|*|text()|comment()">
				<xsl:with-param name="mappedEntityTypeRoleInstances" select="oldOrm:DUMMY"/>
			</xsl:apply-templates>
		</orm:ImpliedFactType>
	</xsl:template>
	<xsl:template match="oldOrm:SubtypeFact">
		<orm:SubtypingFactType>
			<xsl:apply-templates select="@*|*|text()|comment()">
				<xsl:with-param name="mappedEntityTypeRoleInstances" select="oldOrm:DUMMY"/>
			</xsl:apply-templates>
		</orm:SubtypingFactType>
	</xsl:template>
	<xsl:template match="oldOrm:Objects">
		<orm:DomainObjectTypes>
			<xsl:apply-templates select="@*|*|text()|comment()"/>
		</orm:DomainObjectTypes>
	</xsl:template>
	<xsl:template match="oldOrm:FactRoles|oldOrm:QueryRoles">
		<xsl:param name="mappedEntityTypeRoleInstances"/>
		<orm:Roles>
			<xsl:apply-templates select="@*|*|text()|comment()">
				<xsl:with-param name="mappedEntityTypeRoleInstances" select="$mappedEntityTypeRoleInstances"/>
			</xsl:apply-templates>
		</orm:Roles>
	</xsl:template>
	<xsl:template match="oldOrm:SubtypeMetaRole">
		<xsl:param name="mappedEntityTypeRoleInstances"/>
		<orm:SubtypeRole>
			<xsl:apply-templates select="@*|*|text()|comment()">
				<xsl:with-param name="mappedEntityTypeRoleInstances" select="$mappedEntityTypeRoleInstances"/>
			</xsl:apply-templates>
		</orm:SubtypeRole>
	</xsl:template>
	<xsl:template match="oldOrm:SupertypeMetaRole">
		<xsl:param name="mappedEntityTypeRoleInstances"/>
		<orm:SupertypeRole>
			<xsl:apply-templates select="@*|*|text()|comment()">
				<xsl:with-param name="mappedEntityTypeRoleInstances" select="$mappedEntityTypeRoleInstances"/>
			</xsl:apply-templates>
		</orm:SupertypeRole>
	</xsl:template>
	<xsl:template match="oldOrm:FactRoles/oldOrm:Role">
		<xsl:param name="mappedEntityTypeRoleInstances"/>
		<xsl:variable name="roleId" select="string(@id)"/>
		<xsl:variable name="rolePlayerId" select="string(oldOrm:RolePlayer/@ref)"/>
		<xsl:variable name="oppositeInstances" select="$mappedEntityTypeRoleInstances[not(../../@id=$roleId) and key('EntityTypeRoleInstanceRefs',@id)/../../../../@id=$rolePlayerId]"/>
		<xsl:variable name="unaryInstances" select="key('EntityTypeUnaryRoleInstanceRefs',$roleId)"/>
		<orm:Role>
			<xsl:choose>
				<xsl:when test="($oppositeInstances or $unaryInstances) and not(oldOrm:RoleInstances)">
					<!-- We need to construct a RoleInstances block since there isn't one there yet. This comes before
					orm:Extensions in the schema, so we break this out.-->
					<xsl:apply-templates select="@*|*[not(self::oldOrm:Extensions)]|text()|comment()">
						<xsl:with-param name="mappedEntityTypeRoleInstances" select="$mappedEntityTypeRoleInstances"/>
					</xsl:apply-templates>
					<orm:RoleInstances>
						<xsl:for-each select="$oppositeInstances">
							<orm:RoleInstance id="{@id}_opposite" ref="{key('EntityTypeRoleInstanceRefs',@id)/../../@id}"/>
						</xsl:for-each>
						<xsl:for-each select="$unaryInstances">
							<orm:RoleInstance id="{../../@id}_of{@ref}" ref="{../../@id}"/>
						</xsl:for-each>
					</orm:RoleInstances>
					<xsl:variable name="extensions" select="oldOrm:Extensions"/>
					<xsl:if test="$extensions">
						<xsl:apply-templates select="$extensions">
							<xsl:with-param name="mappedEntityTypeRoleInstances" select="$mappedEntityTypeRoleInstances"/>
						</xsl:apply-templates>
					</xsl:if>
				</xsl:when>
				<xsl:otherwise>
					<xsl:apply-templates select="@*|*|text()|comment()">
						<xsl:with-param name="mappedEntityTypeRoleInstances" select="$mappedEntityTypeRoleInstances"/>
					</xsl:apply-templates>
				</xsl:otherwise>
			</xsl:choose>
		</orm:Role>
	</xsl:template>
	<xsl:template match="oldOrm:NestedPredicate">
		<orm:Objectification>
			<xsl:apply-templates select="@*|*|text()|comment()"/>
		</orm:Objectification>
	</xsl:template>

	<xsl:template match="oldOrm:Facts/oldOrm:Fact">
		<xsl:element name="orm:FactType">
			<xsl:variable name="entityTypeRoleInstances" select="oldOrm:FactRoles/oldOrm:Role/oldOrm:RoleInstances/oldOrm:EntityTypeRoleInstance"/>
			<xsl:variable name="entityTypeUnaryRoleInstances" select="key('EntityTypeUnaryRoleInstanceRefs', oldOrm:FactRoles/oldOrm:Role/@id)"/>
			<xsl:choose>
				<xsl:when test="$entityTypeRoleInstances or $entityTypeUnaryRoleInstances">
					<xsl:variable name="factTypeId" select="string(@id)"/>
					<!-- We upgrade all role instances that do not reference roles in the objectified fact type. These are now implied by the referenced objectified fact. -->
					<xsl:variable name="test" select="key('EntityTypeRoleInstanceRefs',$entityTypeRoleInstances/@id)"/>
					<xsl:variable name="nonObjectifiedInstanceRefs" select="key('EntityTypeRoleInstanceRefs',$entityTypeRoleInstances/@id)[not(../../../../oldOrm:NestedPredicate/@ref=$factTypeId)]"/>
					<xsl:variable name="mappedEntityTypeRoleInstances" select="$entityTypeRoleInstances[@id=$nonObjectifiedInstanceRefs/@ref]"/>
					<xsl:choose>
						<xsl:when test="oldOrm:Instances or not($mappedEntityTypeRoleInstances or $entityTypeUnaryRoleInstances)">
							<!-- We'll add the mapped instances in the orm:Instances block, everything is already in the right order-->
							<xsl:apply-templates select="@*|*|text()|comment()">
								<xsl:with-param name="mappedEntityTypeRoleInstances" select="$mappedEntityTypeRoleInstances"/>
							</xsl:apply-templates>
						</xsl:when>
						<xsl:otherwise>
							<!-- We need to add the container orm:Instances. The only thing after it is orm:Extensions-->
							<xsl:apply-templates select="@*|*[not(self::oldOrm:Extensions)]|text()|comment()">
								<!-- Extensions are the only thing after Instances, which we need to mock up. -->
								<xsl:with-param name="mappedEntityTypeRoleInstances" select="$mappedEntityTypeRoleInstances"/>
							</xsl:apply-templates>
							<orm:Instances>
								<xsl:for-each select="$mappedEntityTypeRoleInstances">
									<orm:Fact id="{@id}_fact">
										<orm:RoleInstances>
											<orm:RoleInstance ref="{@id}"/>
											<orm:RoleInstance ref="{@id}_opposite"/>
										</orm:RoleInstances>
									</orm:Fact>
								</xsl:for-each>
								<xsl:for-each select="$entityTypeUnaryRoleInstances">
									<orm:Fact id="{../../@id}_of{@ref}_fact">
										<orm:RoleInstances>
											<orm:RoleInstance ref="{../../@id}_of{@ref}"/>
										</orm:RoleInstances>
									</orm:Fact>
								</xsl:for-each>
							</orm:Instances>
							<xsl:variable name="extensions" select="oldOrm:Extensions"/>
							<xsl:if test="$extensions">
								<xsl:apply-templates select="$extensions">
									<xsl:with-param name="mappedEntityTypeRoleInstances" select="$mappedEntityTypeRoleInstances"/>
								</xsl:apply-templates>
							</xsl:if>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:otherwise>
					<xsl:apply-templates select="@*|*|text()|comment()">
						<xsl:with-param name="mappedEntityTypeRoleInstances" select="oldOrm:DUMMY"/>
					</xsl:apply-templates>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:element>
	</xsl:template>
	<xsl:template match="oldOrm:Facts/oldOrm:Fact/oldOrm:Instances">
		<xsl:param name="mappedEntityTypeRoleInstances"/>
		<orm:Instances>
			<xsl:apply-templates select="@*|*|text()|comment()">
				<xsl:with-param name="mappedEntityTypeRoleInstances" select="$mappedEntityTypeRoleInstances"/>
			</xsl:apply-templates>
			<xsl:for-each select="$mappedEntityTypeRoleInstances">
				<orm:Fact id="{@id}_fact">
					<orm:RoleInstances>
						<orm:RoleInstance ref="{@id}"/>
						<orm:RoleInstance ref="{@id}_opposite"/>
					</orm:RoleInstances>
				</orm:Fact>
			</xsl:for-each>
			<xsl:for-each select="key('EntityTypeUnaryRoleInstanceRefs', ../oldOrm:FactRoles/oldOrm:Role/@id)">
				<orm:Fact id="{../../@id}_of{@ref}_fact">
					<orm:RoleInstances>
						<orm:RoleInstance ref="{../../@id}_of{@ref}"/>
					</orm:RoleInstances>
				</orm:Fact>
			</xsl:for-each>
		</orm:Instances>
	</xsl:template>
	<xsl:template match="oldOrm:FactRoles/oldOrm:Role/oldOrm:RoleInstances">
		<xsl:param name="mappedEntityTypeRoleInstances"/>
		<xsl:variable name="mappedUnaryInstances" select="key('EntityTypeUnaryRoleInstanceRefs',../@id)"/>
		<xsl:if test="$mappedUnaryInstances or oldOrm:*[not(self::oldOrm:EntityTypeRoleInstance) or self::oldOrm:EntityTypeRoleInstance[@id=$mappedEntityTypeRoleInstances/@id]]">
			<orm:RoleInstances>
				<xsl:apply-templates select="@*|*|text()|comment()">
					<xsl:with-param name="mappedEntityTypeRoleInstances" select="$mappedEntityTypeRoleInstances"/>
				</xsl:apply-templates>
				<xsl:for-each select="$mappedEntityTypeRoleInstances">
					<xsl:variable name="entityIdRole" select="string(../../@id)"/>
					<xsl:if test="$entityIdRole!=../../@id">
						<xsl:variable name="entityTypeRoleInstanceRef" select="key('EntityTypeRoleInstanceRefs',@id)"/>
						<xsl:if test="$entityTypeRoleInstanceRef/../../../../@id=../../oldOrm:RolePlayer/@ref">
							<orm:RoleInstance id="{@id}_opposite" ref="{$entityTypeRoleInstanceRef/../../@id}"/>
						</xsl:if>
					</xsl:if>
				</xsl:for-each>
				<xsl:for-each select="$mappedUnaryInstances">
					<orm:RoleInstance id="{../../@id}_of{@ref}" ref="{../../@id}"/>
				</xsl:for-each>
			</orm:RoleInstances>
		</xsl:if>
	</xsl:template>
	<xsl:template match="oldOrm:EntityTypeRoleInstance[@id]">
		<xsl:param name="mappedEntityTypeRoleInstances"/>
		<xsl:if test="@id=$mappedEntityTypeRoleInstances/@id">
			<orm:RoleInstance id="{@id}" ref="{@ref}"/>
		</xsl:if>
		<!-- Otherwise blank. These are either removed or converted. -->
	</xsl:template>
	<xsl:template match="oldOrm:EntityTypeRoleInstance[not(@id)]">
		<!-- All references to an EntityTypeRoleInstance end up blank. -->
	</xsl:template>

	<!-- Sample population renames and removals for new format. -->
	<xsl:template match="oldOrm:EntityTypeInstance/oldOrm:RoleInstances">
		<!-- Intentionally empty, these are removed and replaced with role instances on a fact. -->
	</xsl:template>
	<xsl:template match="oldOrm:FactTypeInstance">
		<orm:Fact>
			<xsl:apply-templates select="@*|*|text()|comment()"/>
		</orm:Fact>
	</xsl:template>
	<xsl:template match="oldOrm:FactTypeRoleInstance">
		<orm:RoleInstance>
			<xsl:apply-templates select="@*|*|text()|comment()"/>
		</orm:RoleInstance>
	</xsl:template>
	<xsl:template match="oldOrm:EntityTypeInstance">
		<orm:Entity>
			<xsl:apply-templates select="@*|*|text()|comment()"/>
		</orm:Entity>
	</xsl:template>
	<xsl:template match="oldOrm:ValueTypeInstance">
		<orm:Value>
			<xsl:apply-templates select="@*|*|text()|comment()"/>
		</orm:Value>
	</xsl:template>
	<xsl:template match="oldOrm:Value">
		<orm:LexicalValue>
			<xsl:apply-templates select="@*|*|text()|comment()"/>
		</orm:LexicalValue>
	</xsl:template>
	<xsl:template match="oldOrm:InvariantValue">
		<orm:InvariantLexicalValue>
			<xsl:apply-templates select="@*|*|text()|comment()"/>
		</orm:InvariantLexicalValue>
	</xsl:template>
	<xsl:template match="oldOrm:ObjectifiedInstance">
		<orm:ObjectifiedFact>
			<xsl:apply-templates select="@*|*|text()|comment()"/>
		</orm:ObjectifiedFact>
	</xsl:template>
	<xsl:template match="oldOrm:EntityTypeSubtypeInstance">
		<orm:Entity>
			<xsl:apply-templates select="@*|*|text()|comment()"/>
		</orm:Entity>
	</xsl:template>
	<xsl:template match="oldOrm:SupertypeInstance">
		<orm:SupertypeEntity>
			<xsl:apply-templates select="@*|*|text()|comment()"/>
		</orm:SupertypeEntity>
	</xsl:template>
	<xsl:template match="oldOrm:ObjectTypeInstance">
		<orm:ObjectInstance>
			<xsl:apply-templates select="@*|*|text()|comment()"/>
		</orm:ObjectInstance>
	</xsl:template>
	<xsl:template match="oldOrm:ObjectifiedInstanceRequiredError">
		<orm:ObjectifiedFactRequiredError>
			<xsl:apply-templates select="@*|*|text()|comment()"/>
		</orm:ObjectifiedFactRequiredError>
	</xsl:template>
	<xsl:template match="oldOrm:ObjectifyingInstanceRequiredError">
		<orm:ObjectifyingEntityRequiredError>
			<xsl:apply-templates select="@*|*|text()|comment()"/>
		</orm:ObjectifyingEntityRequiredError>
	</xsl:template>
	<xsl:template match="oldOrm:CompatibleValueTypeInstanceValueError">
		<orm:CompatibleValueError>
			<xsl:apply-templates select="@*|*|text()|comment()"/>
		</orm:CompatibleValueError>
	</xsl:template>
	<xsl:template match="oldOrm:TooFewEntityTypeRoleInstancesError">
		<orm:IncompleteEntityIdentifierError>
			<xsl:apply-templates select="@*|*|text()|comment()"/>
		</orm:IncompleteEntityIdentifierError>
	</xsl:template>
	<xsl:template match="oldOrm:TooFewFactTypeRoleInstancesError">
		<orm:IncompleteFactError>
			<xsl:apply-templates select="@*|*|text()|comment()"/>
		</orm:IncompleteFactError>
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
