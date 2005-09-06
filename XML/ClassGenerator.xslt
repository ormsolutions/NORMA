<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:orm="http://Schemas.Neumont.edu/ORM/ORMCore"
	xmlns:ormRoot="http://Schemas.Neumont.edu/ORM/ORMRoot"
	xmlns:plx="http://Schemas.Neumont.edu/CodeGeneration/Plix"
	xmlns:ao="http://Schemas.Neumont.edu/ORM/SDK/ClassGenerator/AbsorbedObjects">
	<xsl:param name="CustomToolNamespace" select="'TestNamespace'"/>
	<xsl:param name="AssociationClassDecorator" select="'Association'"/>
	<xsl:param name="PrivateMemberPrefix" select="'my'"/>
	<xsl:param name="ImplementationClassSuffix" select="'Core'"/>
	<xsl:param name="RoleValueConstraintFor" select="'RoleValueConstraintFor'"/>
	<xsl:param name="ValueConstraintFor" select="'ValueConstraintFor'"/>

	<!-- 
	KNOWN ISSUES
	1. not generating a collection for value types that are absorbed in a non-functional direction
	2. not enforcing any constraints, nothing to prevent objects of identical values
	3. and certainly many other things
	-->

	<!-- All Binary and Unary Facts in the model -->
	<xsl:variable name="UnaryAndBinaryFacts" select="ormRoot:ORM2/orm:ORMModel/orm:Facts/orm:Fact[3&gt;count(orm:FactRoles/orm:Role)]"/>
	<!-- Unary Facts Only-->
	<xsl:variable name="UnaryFacts" select="$UnaryAndBinaryFacts[1=count(orm:FactRoles/orm:Role)]"/>
	<!-- Binary Facts Only-->
	<xsl:variable name="BinaryFacts" select="$UnaryAndBinaryFacts[2=count(orm:FactRoles/orm:Role)]"/>


	<!-- Functional Roles -->
	<!--<xsl:variable name ="FunctionalRoles" select=""/>-->
	<!-- Mandatory Functional Roles -->
	<!--<xsl:variable name ="MandatoryFunctionalRoles" select=""/>-->

	<!-- All functional Binary Facts (at least on single-role internal uniqueness constraint on the Fact). 
	     Add functionalRolesCount attribute to the fact and a functionalRole attribute to the role. -->
	<xsl:variable name="FunctionalBinaryFactsFragment">
		<xsl:for-each select="$BinaryFacts">
			<xsl:variable name="UniqueRolesFragment">
				<xsl:for-each select="orm:InternalConstraints/orm:InternalUniquenessConstraint/orm:RoleSequence[1=count(orm:Role)]/orm:Role">
					<!-- Add a UniqueRole ref attribute to each Binary Fact that has one role marked as unique -->
					<UniqueRole ref="{@ref}" />
				</xsl:for-each>
			</xsl:variable>
			<!-- Take the unique Roles out of Fragment form and make the node-set -->
			<xsl:variable name="UniqueRoles" select="msxsl:node-set($UniqueRolesFragment)/child::*"/>
			<xsl:if test="$UniqueRoles">
				<xsl:apply-templates select="." mode="ForFunctionalBinaryFacts">
					<!-- Count the number of unique Roles-->
					<xsl:with-param name="UniqueRolesCount" select="count($UniqueRoles)"/>
					<xsl:with-param name="UniqueRoles" select="$UniqueRoles"/>
				</xsl:apply-templates>
			</xsl:if>
		</xsl:for-each>
	</xsl:variable>
	<!-- Get all functional Binary Facts out of fragment form -->
	<xsl:variable name="FunctionalBinaryFacts" select="msxsl:node-set($FunctionalBinaryFactsFragment)/child::*"/>
	<!-- Copy template for the FunctionalyBinaryFacts variable generation. Matches a Fact -->
	<xsl:template match="orm:Fact|orm:SubtypeFact|orm:ImpliedFact" mode="ForFunctionalBinaryFacts">
		<xsl:param name="UniqueRolesCount"/>
		<xsl:param name="UniqueRoles"/>
		<xsl:copy>
			<xsl:copy-of select="@*"/>
			<xsl:attribute name="functionalRolesCount">
				<xsl:value-of select="$UniqueRolesCount"/>
			</xsl:attribute>
			<xsl:value-of select="text()"/>
			<xsl:apply-templates select="child::*" mode="ForFunctionalBinaryFacts">
				<xsl:with-param name="UniqueRoles" select="$UniqueRoles"/>
			</xsl:apply-templates>
		</xsl:copy>
	</xsl:template>
	<!-- Copy template for the FunctionalyBinaryFacts variable generation. Matches a Role -->
	<xsl:template match="orm:Role" mode="ForFunctionalBinaryFacts">
		<xsl:param name="UniqueRoles"/>
		<xsl:variable name="roleId" select="@id"/>
		<xsl:copy>
			<xsl:copy-of select="@*"/>
			<xsl:if test="count($UniqueRoles[@ref=$roleId])">
				<xsl:attribute name="functionalRole">
					<xsl:value-of select="true()"/>
				</xsl:attribute>
			</xsl:if>
			<xsl:value-of select="text()"/>
			<xsl:apply-templates select="child::*" mode="ForFunctionalBinaryFacts">
				<xsl:with-param name="UniqueRoles" select="$UniqueRoles"/>
			</xsl:apply-templates>
		</xsl:copy>
	</xsl:template>
	<!-- Default copy template for the FunctionalyBinaryFacts variable generation -->
	<xsl:template match="*" mode="ForFunctionalBinaryFacts">
		<xsl:param name="UniqueRoles"/>
		<xsl:copy>
			<xsl:copy-of select="@*"/>
			<xsl:value-of select="text()"/>
			<xsl:apply-templates select="child::*" mode="ForFunctionalBinaryFacts">
				<xsl:with-param name="UniqueRoles" select="$UniqueRoles"/>
			</xsl:apply-templates>
		</xsl:copy>
	</xsl:template>
	<!-- Objects with duplicates removed and a useCount attribute added. Objects are
		represented as a RolePlayer element with id and useCount attributes. -->
	<xsl:variable name="FunctionalRolePlayersFragment">
		<!-- All objects (including duplicates) that are role players for roles in
			a binary fact where the role is the only role player in an internal uniqueness
			constraint. -->
		<xsl:variable name="AllObjects">
			<xsl:for-each select="$FunctionalBinaryFacts">
				<xsl:variable name="Roles" select="orm:FactRoles/orm:Role"/>
				<xsl:for-each select="orm:InternalConstraints/orm:InternalUniquenessConstraint/orm:RoleSequence[1=count(orm:Role)]/orm:Role">
					<xsl:variable name="roleId" select="@ref"/>
					<xsl:for-each select="$Roles[@id=$roleId]/orm:RolePlayer">
						<RolePlayer id="{@ref}"/>
					</xsl:for-each>
				</xsl:for-each>
			</xsl:for-each>
		</xsl:variable>
		<!-- All candidate objects sort so that the duplicates are adjacent -->
		<xsl:variable name="SortedObjects">
			<xsl:for-each select="msxsl:node-set($AllObjects)/child::*">
				<xsl:sort select="@id" data-type="text"/>
				<xsl:copy-of select="."/>
			</xsl:for-each>
		</xsl:variable>
		<xsl:for-each select="msxsl:node-set($SortedObjects)/child::*">
			<xsl:choose>
				<xsl:when test="position()=last()">
					<xsl:choose>
						<xsl:when test="last()=1">
							<xsl:copy>
								<xsl:copy-of select="@*"/>
								<xsl:attribute name="useCount">1</xsl:attribute>
							</xsl:copy>
						</xsl:when>
						<xsl:when test="preceding-sibling::*[1]/@id!=@id">
							<xsl:copy>
								<xsl:copy-of select="@*"/>
								<xsl:attribute name="useCount">1</xsl:attribute>
							</xsl:copy>
						</xsl:when>
					</xsl:choose>
				</xsl:when>
				<xsl:when test="position()=1 or preceding-sibling::*[1]/@id!=@id">
					<!-- Not in the last position, there is more
						than one player, and we don't match our preceding player -->
					<xsl:variable name="contextId" select="@id"/>
					<xsl:variable name="sharedIdCount" select="1+count(following-sibling::*[@id=$contextId])"/>
					<xsl:copy>
						<xsl:copy-of select="@*"/>
						<xsl:attribute name="useCount">
							<xsl:value-of select="$sharedIdCount"/>
						</xsl:attribute>
					</xsl:copy>
				</xsl:when>
			</xsl:choose>
		</xsl:for-each>
	</xsl:variable>
	<xsl:variable name="FunctionalRolePlayers" select="msxsl:node-set($FunctionalRolePlayersFragment)/child::*"/>
	<!-- A set of FunctionalObject elements with a ref attribute (referencing the target object) and
		    nested FunctionalRole elements, also with a ref attribute (referencing the attached role) -->
	<xsl:variable name="FunctionalObjectsFragment">
		<xsl:variable name="AllFunctionalObjects">
			<!-- The algorithm here first: 
				(1)picks all roles with one candidate role only,
				(2)falling back on then the roles opposite a preferred uniqueness constraint,
				(3)falling back on the only mandatory role, 
				(4)falling back on the role that is a role player for the most roles, 
				(5)falling back on a random role. 
				Note that steps 4 and 5 are non-deterministic and we will need extension attributes attached to the roles to make a better choice. 
				The output is a set of FunctionalObject elements with attributes ref (pointing to the object type) and roleRef (indicating the attaching role).
				These are then transformed into FunctionalObject/FunctionalRole elements to handle the case where an object is functional in more than one fact type. -->
			<xsl:for-each select="$FunctionalBinaryFacts">
				<xsl:variable name="Roles" select="orm:FactRoles/orm:Role"/>
				<xsl:choose>
					<xsl:when test="@functionalRolesCount=1">
						<!-- (1) There is only one candidate role -->
						<xsl:for-each select="orm:InternalConstraints/orm:InternalUniquenessConstraint/orm:RoleSequence[1=count(orm:Role)]/orm:Role">
							<xsl:variable name="roleId" select="@ref"/>
							<xsl:for-each select="$Roles[@id=$roleId]/orm:RolePlayer">
								<FunctionalObject ref="{@ref}" roleRef="{$roleId}"/>
							</xsl:for-each>
						</xsl:for-each>
					</xsl:when>
					<xsl:otherwise>
						<!-- Roles that are not attached to preferred uniqueness constraints -->
						<xsl:variable name="OppositePreferredCandidateRolesFragment">
							<xsl:for-each select="orm:InternalConstraints">
								<xsl:if test="count(orm:InternalUniquenessConstraint/orm:PreferredIdentifierFor)">
									<xsl:for-each select="orm:InternalUniquenessConstraint[not(orm:PreferredIdentifierFor) and (1=count(orm:RoleSequence/orm:Role))]/orm:RoleSequence/orm:Role">
										<xsl:copy-of select="."/>
									</xsl:for-each>
								</xsl:if>
							</xsl:for-each>
						</xsl:variable>
						<xsl:variable name="OppositePreferredCandidateRoles" select="msxsl:node-set($OppositePreferredCandidateRolesFragment)/child::*"/>
						<xsl:choose>
							<xsl:when test="count($OppositePreferredCandidateRoles)=1">
								<!-- (2) The role is opposite a preferred uniqueness constraint -->
								<xsl:variable name="roleId" select="$OppositePreferredCandidateRoles[1]/@ref"/>
								<xsl:for-each select="$Roles[@id=$roleId]/orm:RolePlayer">
									<FunctionalObject ref="{@ref}" roleRef="{$roleId}"/>
								</xsl:for-each>
							</xsl:when>
							<xsl:otherwise>
								<!-- UNDONE: Also, IsMandatory should be in the derived namespace -->
								<xsl:variable name="MandatoryRoles" select="$Roles[@functionalRole and @IsMandatory='true']"/>
								<xsl:choose>
									<xsl:when test="1=count($MandatoryRoles)">
										<!-- (3) The role is the only mandatory role -->
										<xsl:for-each select="$MandatoryRoles">
											<FunctionalObject ref="{orm:RolePlayer/@ref}" roleRef="{@id}"/>
										</xsl:for-each>
									</xsl:when>
									<xsl:otherwise>
										<!-- (4) Use the object for the role with the most role players  -->
										<xsl:variable name="RolesWithPlayerCounts">
											<xsl:for-each select="$Roles">
												<xsl:if test="@functionalRole">
													<xsl:variable name="rolePlayerId" select="orm:RolePlayer/@ref"/>
													<xsl:copy>
														<xsl:copy-of select="@*"/>
														<xsl:attribute name="useCount">
															<xsl:value-of select="$FunctionalRolePlayers[@id=$rolePlayerId]/@useCount"/>
														</xsl:attribute>
														<xsl:value-of select="text()"/>
														<xsl:copy-of select="child::*"/>
													</xsl:copy>
												</xsl:if>
											</xsl:for-each>
										</xsl:variable>
										<xsl:for-each select="msxsl:node-set($RolesWithPlayerCounts)/child::*">
											<xsl:sort select="@useCount" data-type="number" order="descending"/>
											<xsl:if test="position()=1">
												<!-- Note this also picks up the fallback case (5) (a tie on the most) -->
												<xsl:variable name="roleId" select="@id"/>
												<xsl:for-each select="$Roles[@id=$roleId]/orm:RolePlayer">
													<FunctionalObject ref="{@ref}" roleRef="{$roleId}"/>
												</xsl:for-each>
											</xsl:if>
										</xsl:for-each>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="SortedFunctionalObjects">
			<xsl:for-each select="msxsl:node-set($AllFunctionalObjects)/child::*">
				<xsl:sort select="@ref" data-type="text"/>
				<xsl:copy-of select="."/>
			</xsl:for-each>
		</xsl:variable>
		<xsl:for-each select="msxsl:node-set($SortedFunctionalObjects)/child::*">
			<xsl:choose>
				<xsl:when test="position()=last()">
					<xsl:choose>
						<xsl:when test="last()=1">
							<xsl:copy>
								<xsl:copy-of select="@ref"/>
								<FunctionalRole ref="{@roleRef}"/>
							</xsl:copy>
						</xsl:when>
						<xsl:when test="preceding-sibling::*[1]/@ref!=@ref">
							<xsl:copy>
								<xsl:copy-of select="@ref"/>
								<FunctionalRole ref="{@roleRef}"/>
							</xsl:copy>
						</xsl:when>
					</xsl:choose>
				</xsl:when>
				<xsl:when test="position()=1 or preceding-sibling::*[1]/@ref!=@ref">
					<!-- Not in the last position, there is more
					     than one player, and we don't match our preceding player -->
					<xsl:variable name="contextRef" select="@ref"/>
					<xsl:copy>
						<xsl:copy-of select="@ref"/>
						<FunctionalRole ref="{@roleRef}"/>
						<xsl:for-each select="following-sibling::*[@ref=$contextRef]">
							<FunctionalRole ref="{@roleRef}"/>
						</xsl:for-each>
					</xsl:copy>
				</xsl:when>
			</xsl:choose>
		</xsl:for-each>
	</xsl:variable>
	<xsl:variable name="FunctionalObjects" select="msxsl:node-set($FunctionalObjectsFragment)/child::*"/>
	<xsl:variable name="DominantFunctionalRolesFragment">
		<xsl:for-each select="$FunctionalObjects">
			<xsl:copy-of select="FunctionalRole"/>
		</xsl:for-each>
	</xsl:variable>
	<xsl:variable name="DominantFunctionalRoles" select="msxsl:node-set($DominantFunctionalRolesFragment)/child::*"/>
	<xsl:template name="IsDominantFunctionalRole">
		<xsl:param name="RoleRef"/>
		<xsl:value-of select="0!=count($DominantFunctionalRoles[@ref=$RoleRef])"/>
	</xsl:template>
	<xsl:variable name="AbsorbedObjectsFragment">
		<xsl:for-each select="ormRoot:ORM2/orm:ORMModel">
			<xsl:variable name="RawFacts" select="orm:Facts/child::orm:*"/>
			<xsl:variable name="RawObjects" select="orm:Objects/child::orm:*"/>
			<xsl:variable name="BinaryAbsorbedObjectsFragment">
				<xsl:call-template name="BinaryAbsorbObjects">
					<xsl:with-param name="Objects">
						<xsl:for-each select="$RawObjects">
							<xsl:variable name="Object" select="."/>
							<ao:Object type="{local-name()}" id="{@id}" name="{@Name}">
								<xsl:variable name="PlayedRoles" select="orm:PlayedRoles/orm:Role"/>
								<xsl:for-each select="$PlayedRoles">
									<xsl:variable name="roleId" select="@ref"/>
									<xsl:for-each select="$RawFacts//orm:Role[@id=$roleId]">
										<xsl:variable name="parentFact" select="ancestor::*[2]"/>
										<ao:RelatedObject factRef="{$parentFact/@id}" roleRef="{$roleId}">
											<xsl:variable name="oppositeRoles" select="$parentFact/orm:FactRoles/child::*[@id!=$roleId]"/>
											<xsl:attribute name="arity">
												<xsl:value-of select="count($parentFact/orm:FactRoles/orm:Role)"/>
											</xsl:attribute>
											<!--If a role name has been specified, use it; otherwise, generate one if needed.-->
											<xsl:attribute name="roleName">
												<xsl:call-template name="GenerateRoleName">
													<xsl:with-param name="ObjectName" select="$Object/@Name"/>
													<xsl:with-param name="Role" select="."/>
													<xsl:with-param name="ParentFact" select="$parentFact"/>
												</xsl:call-template>
											</xsl:attribute>
											<!-- assign the manadatory roles as 'relaxed' if the fact is OneToOne or ManyToMany and both roles are mandatory -->
											<!-- opposite roles multiplicity -->
											<xsl:variable name="oppositeRoleMultiplicity" select="../orm:Role[@id!=$roleId]/@Multiplicity"/>
											<xsl:attribute name="mandatory">
												<xsl:choose>
													<!-- both are mandatory so set the dominant one to relaxed-->
													<!-- (1)one to one binary set the non dominant role to relaxed-->
													<xsl:when test="@Multiplicity='ExactlyOne' and count($DominantFunctionalRoles[@ref=$roleId])=0 and $oppositeRoleMultiplicity='ExactlyOne' ">
														<xsl:value-of select="'relaxed'"/>
													</xsl:when>
													<!-- (2)many to many binary set both to relaxed-->
													<xsl:when test="@Multiplicity='OneToMany' and $oppositeRoleMultiplicity='OneToMany'">
														<xsl:value-of select="'relaxed'"/>
													</xsl:when>
													<!-- (3)Opposite role is funtional w/ both mandatory set the non dominant role to relaxed-->
													<xsl:when test="@Multiplicity='ExactlyOne' and count($DominantFunctionalRoles[@ref=$roleId])=0 and $oppositeRoleMultiplicity='OneToMany'">
														<xsl:value-of select="'relaxed'"/>
													</xsl:when>
													<!-- (4)Set mandatory to False or True -->
													<xsl:otherwise>
														<xsl:value-of select="@IsMandatory"/>
													</xsl:otherwise>
												</xsl:choose>
											</xsl:attribute>
											<xsl:variable name="oppositeMultiplicity" select="@Multiplicity"/>
											<xsl:attribute name="unique">
												<xsl:value-of select="$oppositeMultiplicity = 'ZeroToOne' or $oppositeMultiplicity = 'ExactlyOne'"/>
											</xsl:attribute>
											<xsl:if test="1=count($oppositeRoles)">
												<xsl:for-each select="$oppositeRoles">
													<xsl:variable name="oppositeRoleId" select="@id"/>
													<xsl:variable name="oppositeObjectId" select="orm:RolePlayer/@ref"/>
													<xsl:variable name="oppositeObject" select="$RawObjects[@id=$oppositeObjectId]"/>
													<xsl:attribute name="multiplicity">
														<xsl:value-of select="@Multiplicity"/>
													</xsl:attribute>
													<xsl:attribute name="oppositeRoleRef">
														<xsl:value-of select="$oppositeRoleId"/>
													</xsl:attribute>
													<xsl:attribute name="oppositeObjectRef">
														<xsl:value-of select="$oppositeObjectId"/>
													</xsl:attribute>
													<xsl:attribute name="oppositeObjectName">
														<xsl:value-of select="$oppositeObject/@Name"/>
													</xsl:attribute>
													<xsl:attribute name="oppositeRoleName">
														<xsl:call-template name="GenerateRoleName">
															<xsl:with-param name="ObjectName" select="$oppositeObject/@Name"/>
															<xsl:with-param name="Role" select="$parentFact/orm:FactRoles/orm:Role[@id=$oppositeRoleId]"/>
															<xsl:with-param name="ParentFact" select="$parentFact"/>
														</xsl:call-template>
													</xsl:attribute>
												</xsl:for-each>
											</xsl:if>
										</ao:RelatedObject>
									</xsl:for-each>
								</xsl:for-each>
							</ao:Object>
						</xsl:for-each>
					</xsl:with-param>
				</xsl:call-template>
			</xsl:variable>
			<xsl:variable name="BinaryAbsorbedObjects" select="msxsl:node-set($BinaryAbsorbedObjectsFragment)/child::*"/>
			<xsl:variable name="AssociationFacts">
				<xsl:variable name="AllAssociations" select="$BinaryAbsorbedObjects//ao:RelatedObject[not(@oppositeRoleRef)]"/>
				<xsl:variable name="SortedAssociations">
					<xsl:for-each select="$AllAssociations">
						<xsl:sort data-type="text" select="@factRef"/>
						<xsl:copy-of select="."/>
					</xsl:for-each>
				</xsl:variable>
				<xsl:for-each select="msxsl:node-set($SortedAssociations)/child::*">
					<xsl:variable name="factId" select="@factRef"/>
					<xsl:choose>
						<xsl:when test="position()=last()">
							<xsl:if test="position()=1 or preceding-sibling::*[1]/@factRef!=$factId">
								<xsl:copy-of select="$RawFacts[@id=$factId]"/>
							</xsl:if>
						</xsl:when>
						<xsl:when test="position()=1 or preceding-sibling::*[1]/@factRef!=$factId">
							<xsl:copy-of select="$RawFacts[@id=$factId]"/>
						</xsl:when>
					</xsl:choose>
				</xsl:for-each>
			</xsl:variable>
			<xsl:call-template name="AbsorbAssociationFacts">
				<xsl:with-param name="Objects" select="$BinaryAbsorbedObjects"/>
				<xsl:with-param name="AssociationFacts" select="msxsl:node-set($AssociationFacts)/child::*"/>
			</xsl:call-template>
		</xsl:for-each>
	</xsl:variable>
	<xsl:template name="StringReplace">
		<!--Takes a string and replaces a given substring with a given replacement string.-->
		<xsl:param name="InputString"/>
		<xsl:param name="Match"/>
		<xsl:param name="ReplaceWith"/>
		<xsl:variable name="leftPart" select="substring-before($InputString, $Match)"/>
		<xsl:variable name="rightPart" select="substring-after($InputString, $Match)"/>
		<xsl:value-of select="concat($leftPart, $ReplaceWith, $rightPart)"/>
	</xsl:template>
	<xsl:template name="PieceAndParseReading">
		<!--Takes a reading, pops in the object name, and checks for hyphen binding.-->
		<xsl:param name="Reading"/>
		<xsl:param name="ObjectTypeName"/>
		<xsl:param name="ObjectTypeNr"/>
		<xsl:variable name="pieced">
			<xsl:call-template name="StringReplace">
				<xsl:with-param name="InputString" select="$Reading"/>
				<xsl:with-param name="Match" select="concat('{', $ObjectTypeNr, '}')"/>
				<xsl:with-param name="ReplaceWith" select="$ObjectTypeName"/>
			</xsl:call-template>
		</xsl:variable>
		<xsl:call-template name="StringAfterLastMatch">
			<xsl:with-param name="Value" select="$pieced"/>
			<xsl:with-param name="Match" select="string(' ')"/>
			<xsl:with-param name="Stopper" select="string('-')"/>
		</xsl:call-template>
	</xsl:template>
	<xsl:template name="StringAfterLastMatch">
		<!--Iterative template to find the last match of a string and return the string to the right
		of the last match with the spaces and hyphens translated. Used for hyphen binding.-->
		<xsl:param name="Value"/>
		<xsl:param name="Match"/>
		<xsl:param name="Stopper"/>
		<xsl:choose>
			<xsl:when test="contains($Value, $Match) and string-length(substring-before($Value, $Match)) &lt; string-length(substring-before($Value, $Stopper))">
				<xsl:call-template name="StringAfterLastMatch">
					<xsl:with-param name="Value" select="substring-after($Value, $Match)"/>
					<xsl:with-param name="Match" select="$Match"/>
					<xsl:with-param name="Stopper" select="$Stopper"/>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="translate(translate($Value, ' ', '_'), '-', '')"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="GenerateRoleName">
		<!--Takes a reading and one of it's attached objects and generates an object name.-->
		<xsl:param name="ObjectName"/>
		<xsl:param name="Role"/>
		<xsl:param name="ParentFact"/>
		<xsl:variable name="readingOrder" select="$ParentFact/orm:ReadingOrders/orm:ReadingOrder[1]"/>
		<xsl:variable name="roleId" select="$Role/@id"/>
		<xsl:variable name="givenRoleName" select="$Role/@Name"/>
		<xsl:choose>
			<xsl:when test="string-length($givenRoleName)">
				<xsl:value-of select="$givenRoleName"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:variable name="readingPosition">
					<xsl:for-each select="$readingOrder/orm:RoleSequence/orm:Role">
						<xsl:if test="@ref=$roleId">
							<xsl:value-of select="position()-1"/>
						</xsl:if>
					</xsl:for-each>
				</xsl:variable>
				<xsl:variable name="factPosition">
					<xsl:variable name="playerId" select="orm:RolePlayer/@ref"/>
					<xsl:variable name="markRoles">
						<xsl:for-each select="$ParentFact/orm:FactRoles/orm:Role">
							<xsl:choose>
								<xsl:when test="@id=$roleId">
									<xsl:text>y</xsl:text>
								</xsl:when>
								<xsl:when test="orm:RolePlayer/@ref=$playerId">
									<xsl:text>x</xsl:text>
								</xsl:when>
							</xsl:choose>
						</xsl:for-each>
					</xsl:variable>
					<xsl:if test="string-length($markRoles)> 1">
						<xsl:value-of select="string-length(substring-before($markRoles,'y')) + 1"/>
					</xsl:if>
				</xsl:variable>
				<xsl:variable name="roleReading" select="$readingOrder/orm:Readings/orm:Reading/orm:Data"/>
				<xsl:choose>
					<xsl:when test="contains(substring-before($roleReading, concat('{',$readingPosition,'}')), '- ') or contains($roleReading, concat('-{',$readingPosition,'}'))">
						<xsl:call-template name="PieceAndParseReading">
							<xsl:with-param name="Reading" select="$roleReading"/>
							<xsl:with-param name="ObjectTypeNr" select="$readingPosition"/>
							<xsl:with-param name="ObjectTypeName" select="$ObjectName"/>
						</xsl:call-template>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$ObjectName"/>
						<xsl:value-of select="$factPosition"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:variable name="AbsorbedObjects" select="msxsl:node-set($AbsorbedObjectsFragment)/child::*"/>
	<xsl:template name="AbsorbAssociationFacts">
		<xsl:param name="Objects"/>
		<xsl:param name="AssociationFacts"/>
		<xsl:for-each select="$Objects">
			<xsl:variable name="relatedObjectCount" select="count(ao:RelatedObject)"/>
			<xsl:variable name="relatedAssociationCount" select="count(ao:RelatedObject[not(@oppositeRoleRef)])"/>
			<xsl:choose>
				<xsl:when test="$relatedObjectCount=1">
					<xsl:choose>
						<!-- I'm being absorbed, don't copy -->
						<xsl:when test="($relatedAssociationCount=1 and count(ao:AbsorbedObject)&lt;=1) or @type='ValueType'"/>
						<!-- no absorbtion, so copy -->
						<xsl:otherwise>
							<xsl:copy-of select="."/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:otherwise>
					<xsl:copy-of select="."/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:for-each>
		<xsl:for-each select="$AssociationFacts">
			<xsl:variable name="currentFact" select="."/>
			<xsl:variable name="factId" select="@id"/>
			<xsl:choose>
				<!-- unary fact putting them in their own tag for custom handling -->
				<!-- just stop output of unary fact, object code will output unary facts -->
				<xsl:when test="count($UnaryFacts[@id=$factId])&gt;0">
					<!--					<Unary name="{@Name}" id="{@id}">
						<xsl:for-each select="orm:FactRoles/orm:Role">
							<xsl:variable name="roleId" select="@id"/>
							<xsl:variable name="roleName" select="@Name"/>
							<xsl:variable name="objectId" select="orm:RolePlayer/@ref"/>
							<xsl:variable name="rolePlayerObject" select="$Objects[@id=$objectId]"/>
							<xsl:for-each select="$rolePlayerObject">
								<ao:RelatedObject type="{@type}" objectRef="{@id}" roleRef="{$roleId}" roleName="{$roleName}" className="{@name}">
								</ao:RelatedObject>
							</xsl:for-each>
						</xsl:for-each>
					</Unary>-->
				</xsl:when>
				<xsl:otherwise>
					<ao:Association name="{@Name}" id="{@id}">
						<xsl:for-each select="orm:FactRoles/orm:Role">
							<xsl:variable name="roleId" select="@id"/>
							<xsl:variable name="hasMany" select="0=count($currentFact/orm:InternalConstraints/orm:InternalUniquenessConstraint/orm:RoleSequence/orm:Role[@ref=$roleId])"/>
							<xsl:variable name="roleName" select="@Name"></xsl:variable>
							<xsl:variable name="objectId" select="orm:RolePlayer/@ref"/>
							<xsl:variable name="rolePlayerObject" select="$Objects[@id=$objectId]"/>
							<xsl:for-each select="$rolePlayerObject">
								<xsl:choose>
									<xsl:when test="@type='ValueType' or (1=count(ao:RelatedObject) and count(ao:AbsorbedObject)&lt;=1)">
										<ao:AbsorbedObject type="{@type}" ref="{@id}" name="{@name}" roleName="{$roleName}" thisRoleRef="{$roleId}">
											<!--<xsl:if test="$hasMany">
												<xsl:attribute name="multiplicity">Many</xsl:attribute>
											</xsl:if>-->
											<xsl:copy-of select="ao:AbsorbedObject"/>
										</ao:AbsorbedObject>
									</xsl:when>
									<xsl:otherwise>
										<ao:RelatedObject type="{@type}" objectRef="{@id}" roleRef="{$roleId}" roleName="{$roleName}" className="{@name}">
											<xsl:variable name="myRelatedObjects" select="./child::ao:RelatedObject"/>
											<xsl:variable name="factRef" select="$myRelatedObjects[@roleRef=$roleId]/@factRef"/>
											<!--If there are more than one related objects with the same factRef, use the roleName from
											the relatedObject instead of the role.-->
											<xsl:if test="count($myRelatedObjects[@factRef=$factRef])>0">
												<xsl:attribute name="roleName">
													<xsl:value-of select="$myRelatedObjects[@roleRef=$roleId]/@roleName"/>
												</xsl:attribute>
											</xsl:if>
											<!--<xsl:if test="$hasMany">
												<xsl:attribute name="multiplicity">Many</xsl:attribute>
											</xsl:if>-->
										</ao:RelatedObject>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:for-each>
						</xsl:for-each>
					</ao:Association>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:for-each>
	</xsl:template>
	<xsl:template name="BinaryAbsorbObjects">
		<xsl:param name="Objects"/>
		<xsl:variable name="ObjectsSet" select="msxsl:node-set($Objects)"/>
		<xsl:variable name="ThisPassFragment">
			<xsl:for-each select="$ObjectsSet">
				<xsl:variable name="relatedObjectCount" select="count(ao:RelatedObject)"/>
				<xsl:choose>
					<xsl:when test="$relatedObjectCount=1">
						<!-- I might have been absorbed (I play one functional role or I'm a value type),
						     or I might absorb one other -->
						<xsl:variable name="isValueType" select="@type='ValueType'"/>
						<xsl:for-each select="ao:RelatedObject">
							<xsl:choose>
								<xsl:when test="@type='ValueType' and @oppositeRoleRef"/>
								<!-- Value type role players in binary relationships are always absorbed -->
								<xsl:otherwise>
									<xsl:variable name="oppositeObjectId" select="@oppositeObjectRef"/>
									<xsl:variable name="isDominantFunctional">
										<xsl:if test="string-length($oppositeObjectId)">
											<xsl:call-template name="IsDominantFunctionalRole">
												<xsl:with-param name="RoleRef" select="@roleRef"/>
											</xsl:call-template>
										</xsl:if>
									</xsl:variable>
									<xsl:variable name="oppositeObjectTemp">
										<xsl:if test="string-length($oppositeObjectId)">
											<xsl:copy-of select="$ObjectsSet[@id=$oppositeObjectId]"/>
										</xsl:if>
									</xsl:variable>
									<xsl:variable name="oppositeObject" select="msxsl:node-set($oppositeObjectTemp)/child::*"/>
									<xsl:variable name="oppositeRoleCount" select="count($oppositeObject/ao:RelatedObject)"/>
									<xsl:choose>
										<xsl:when test="$isDominantFunctional='true' and 1=$oppositeRoleCount">
											<!-- We're absorbing the opposite object, but only if it has not
											     absorbed more than one object itself -->
											<xsl:choose>
												<xsl:when test="count($oppositeObject/ao:AbsorbedObject)&lt;=1">
													<xsl:variable name="roleId" select="@roleRef"/>
													<xsl:variable name="roleName" select="@roleName"/>
													<xsl:variable name="isMandatory" select="@mandatory"/>
													<xsl:for-each select="..">
														<xsl:copy>
															<xsl:copy-of select="@*"/>
															<xsl:copy-of select="ao:AbsorbedObject"/>
															<xsl:for-each select="$oppositeObject">
																<ao:AbsorbedObject type="{@type}" ref="{@id}" name="{@name}" unique="{@unique}"  thisRoleName="{$roleName}" thisRoleRef="{$roleId}" oppositeRoleRef="{ao:RelatedObject/@roleRef}" oppositeRoleName="{ao:RelatedObject/@roleName}">
																	<xsl:if test="isMandatory">
																		<xsl:attribute name="mandatory">
																			<xsl:value-of select="true()"/>
																		</xsl:attribute>
																	</xsl:if>
																	<xsl:copy-of select="ao:AbsorbedObject"/>
																</ao:AbsorbedObject>
															</xsl:for-each>
														</xsl:copy>
													</xsl:for-each>
												</xsl:when>
												<xsl:otherwise>
													<xsl:copy-of select=".."/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:when>
										<xsl:otherwise>
											<!-- See if we're being absorbed by an opposite dominant functional role -->
											<xsl:variable name="isOppositeDominantFunctional">
												<xsl:if test="string-length(@oppositeRoleRef)">
													<xsl:call-template name="IsDominantFunctionalRole">
														<xsl:with-param name="RoleRef" select="@oppositeRoleRef"/>
													</xsl:call-template>
												</xsl:if>
											</xsl:variable>
											<xsl:choose>
												<!-- We're being absorbed if we haven't already absorbed more than 1 -->
												<xsl:when test="$isOppositeDominantFunctional='true' and count(../ao:AbsorbedObject)&lt;=1"/>
												<xsl:otherwise>
													<xsl:copy-of select=".."/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:for-each>
					</xsl:when>
					<xsl:when test="$relatedObjectCount&gt;1">
						<!-- I might absorb others -->
						<xsl:copy>
							<xsl:copy-of select="@*"/>
							<xsl:copy-of select="ao:AbsorbedObject"/>
							<xsl:for-each select="ao:RelatedObject">
								<xsl:variable name="roleId" select="@roleRef"/>
								<xsl:variable name="roleName" select="@roleName"/>
								<xsl:variable name="isMandatory" select="@mandatory"/>
								<xsl:variable name="oppositeObjectId" select="@oppositeObjectRef"/>
								<xsl:choose>
									<xsl:when test="string-length($oppositeObjectId)">
										<xsl:variable name="oppositeObject" select="$ObjectsSet[@id=$oppositeObjectId]"/>
										<xsl:variable name="shouldAbsorb">
											<xsl:choose>
												<xsl:when test="$oppositeObject/@type='ValueType'">x</xsl:when>
												<xsl:otherwise>
													<xsl:variable name="IsDominantFunctional">
														<xsl:call-template name="IsDominantFunctionalRole">
															<xsl:with-param name="RoleRef" select="@roleRef"/>
														</xsl:call-template>
													</xsl:variable>
													<!-- count how many related roles and absorbed roles -->
													<xsl:variable name="oppositeAbsorbedCount" select="count($oppositeObject/ao:AbsorbedObject)"/>
													<xsl:variable name="oppositeRelatedCount" select="count($oppositeObject/ao:RelatedObject)"/>
													<xsl:if test="$IsDominantFunctional='true' and 1=$oppositeRelatedCount and $oppositeAbsorbedCount&lt;2">x</xsl:if>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:variable>
										<xsl:choose>
											<xsl:when test="string-length($shouldAbsorb)">
												<xsl:for-each select="$oppositeObject">
													<xsl:variable name="oppositeRelatedObject" select="ao:RelatedObject[@oppositeRoleRef=$roleId]"/>
													<xsl:variable name="oppositeRelatedObjectMultiplicity" select="$oppositeRelatedObject/@multiplicity"/>
													<ao:AbsorbedObject type="{@type}" ref="{@id}" name="{@name}" unique="{$oppositeRelatedObjectMultiplicity = 'ZeroToOne' or oppositeRelatedObjectMultiplicity = 'ExactlyOne'}" thisRoleName="{$roleName}" thisRoleRef="{$roleId}" oppositeRoleRef="{$oppositeRelatedObject/@roleRef}" oppositeRoleName="{$oppositeRelatedObject/@roleName}">
														<xsl:if test="$isMandatory">
															<xsl:attribute name="mandatory">
																<xsl:value-of select="true()"/>
															</xsl:attribute>
														</xsl:if>
														<xsl:copy-of select="ao:AbsorbedObject"/>
													</ao:AbsorbedObject>
												</xsl:for-each>
											</xsl:when>
											<xsl:otherwise>
												<xsl:copy-of select="."/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:copy-of select="."/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:for-each>
						</xsl:copy>
					</xsl:when>
					<xsl:otherwise>
						<!-- I'm neutral -->
						<xsl:copy-of select="."/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="ThisPass" select="msxsl:node-set($ThisPassFragment)/child::*"/>
		<xsl:choose>
			<xsl:when test="count($ObjectsSet)=count($ThisPass)">
				<xsl:call-template name="AbsorbBinaryValueTypes">
					<xsl:with-param name="Objects" select="$ThisPass"/>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="BinaryAbsorbObjects">
					<xsl:with-param name="Objects" select="$ThisPass"/>
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="AbsorbBinaryValueTypes">
		<xsl:param name="Objects"/>
		<!-- Create a Objects absorption set where value types are absorbed, but any value type
		     that participates in a non-binary fact still remains. -->
		<xsl:for-each select="$Objects">
			<xsl:choose>
				<xsl:when test="@type='ValueType'">
					<xsl:variable name="newRelatedObjectsFragment">
						<xsl:for-each select="ao:RelatedObject">
							<xsl:if test="not(@oppositeRoleRef)">
								<xsl:copy-of select="."/>
							</xsl:if>
						</xsl:for-each>
					</xsl:variable>
					<xsl:variable name="newRelatedObjects" select="msxsl:node-set($newRelatedObjectsFragment)/child::*"/>
					<xsl:choose>
						<xsl:when test="count($newRelatedObjects)">
							<xsl:copy>
								<xsl:copy-of select="@*"/>
								<xsl:copy-of select="ao:AbsorbedObject"/>
								<xsl:copy-of select="$newRelatedObjects"/>
							</xsl:copy>
						</xsl:when>
					</xsl:choose>
				</xsl:when>
				<xsl:otherwise>
					<xsl:choose>
						<xsl:when test="count(RelatedObjects)">
							<xsl:variable name="newRelatedObjects">
								<xsl:for-each select="RelatedObjects">
									<xsl:choose>
										<xsl:when test="@oppositeRoleRef">
											<xsl:variable name="oppositeRoleRef" select="@oppositeRoleRef"/>
											<xsl:variable name="oppositeId" select="@oppositeObjectRef"/>
											<xsl:variable name="oppositeObject" select="$Objects[@id=$oppositeId]"/>
											<xsl:choose>
												<xsl:when test="$oppositeObject/@type='ValueType'">
													<xsl:variable name="roleId" select="@roleRef"/>
													<xsl:for-each select="$oppositeObject">
														<ao:AbsorbedObject type="{@type}" ref="{@id}" name="{@name}" thisRoleRef="{$roleId}" oppositeRoleRef="{$oppositeRoleRef}">
															<xsl:copy-of select="ao:AbsorbedObject"/>
														</ao:AbsorbedObject>
													</xsl:for-each>
												</xsl:when>
												<xsl:otherwise>
													<xsl:copy-of select="."/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:when>
										<xsl:otherwise>
											<xsl:copy-of select="."/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:for-each>
							</xsl:variable>
							<xsl:copy>
								<xsl:copy-of select="@*"/>
								<xsl:copy-of select="ao:AbsorbedObject"/>
								<xsl:copy-of select="msxsl:node-set($newRelatedObjects)/child::*"/>
							</xsl:copy>
						</xsl:when>
						<xsl:otherwise>
							<xsl:copy-of select="."/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:for-each>
	</xsl:template>
	<!-- Get the data type name and qualifier for the passed-in orm2 data type element.
	     Returns a DataType element with up to three attributes (dataTypeName, dataTypeQualifier, dataTypeIsSimpleArray)
		 to use in plix -->
	<xsl:template name="MapDataType">
		<xsl:variable name="tagName" select="local-name()"/>
		<xsl:choose>
			<xsl:when test="$tagName='FixedLengthTextDataType' or $tagName='VariableLengthTextDataType' or $tagName='LargeLengthTextDataType'">
				<DataType dataTypeName="String" dataTypeQualifier="System"/>
			</xsl:when>
			<xsl:when test="$tagName='SignedIntegerNumericDataType'">
				<DataType dataTypeName="Int32" dataTypeQualifier="System"/>
			</xsl:when>
			<xsl:when test="$tagName='UnsignedIntegerNumericDataType' or $tagName='AutoCounterNumericDataType'">
				<DataType dataTypeName="UInt32" dataTypeQualifier="System"/>
			</xsl:when>
			<xsl:when test="$tagName='FloatingPointNumericDataType'">
				<DataType dataTypeName="Single" dataTypeQualifier="System"/>
			</xsl:when>
			<xsl:when test="$tagName='DecimalNumericDataType' or $tagName='MoneyNumericDataType'">
				<DataType dataTypeName="Decimal" dataTypeQualifier="System"/>
			</xsl:when>
			<xsl:when test="$tagName='FixedLengthRawDataDataType' or $tagName='VariableLengthRawDataDataType' or $tagName='LargeLengthRawDataDataType' or $tagName='PictureRawDataDataType' or $tagName='OleObjectRawDataDataType'">
				<DataType dataTypeName="Byte" dataTypeQualifier="System" dataTypeIsSimpleArray="true"/>
			</xsl:when>
			<xsl:when test="$tagName='AutoTimestampTemporalDataType' or $tagName='TimeTemporalDataType' or $tagName='DateTemporalDataType' or $tagName='DateAndTimeTemporalDataType'">
				<DataType dataTypeName="DateTime" dataTypeQualifier="System"/>
			</xsl:when>
			<xsl:when test="$tagName='TrueOrFalseLogicalDataType' or $tagName='YesOrNoLogicalDataType'">
				<DataType dataTypeName="Boolean" dataTypeQualifier="System"/>
			</xsl:when>
			<xsl:when test="$tagName='RowIdOtherDataType' or $tagName='ObjectIdOtherDataType'">
				<DataType dataTypeName="UInt32" dataTypeQualifier="System"/>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
	<!-- Generate a public property. The current context should
	     be an element with a name attribute and a DataType child element with attributes/children
		 corresponding to the plix attributes and child nodes for a data type reference. -->
	<xsl:template name="GenerateAbstractProperty">
		<plx:Property abstract="true" name="{@name}" visibility="Public">
			<plx:Param type="RetVal" name="">
				<xsl:copy-of select="DataType/@*"/>
				<xsl:copy-of select="DataType/child::*"/>
			</plx:Param>
			<plx:Get/>
			<plx:Set/>
		</plx:Property>
	</xsl:template>
	<xsl:template name="GenerateBackedProperty">
		<xsl:param name="InitializeFields" select="true()"/>
		<xsl:param name="ClassName"/>
		<plx:Field name="{$PrivateMemberPrefix}{@name}" visibility="Private">
			<xsl:copy-of select="DataType/@*"/>
			<xsl:copy-of select="DataType/child::*"/>
			<xsl:if test="$InitializeFields">
				<plx:Initialize>
					<plx:DefaultValueOf>
						<xsl:copy-of select="DataType/@*"/>
						<xsl:copy-of select="DataType/child::*"/>
					</plx:DefaultValueOf>
				</plx:Initialize>
			</xsl:if>
		</plx:Field>
		<!-- Get and Set Properties for the given Object-->
		<plx:Property name="{@name}" visibility="Public" override="true">
			<plx:Param type="RetVal" name="">
				<xsl:copy-of select="DataType/@*"/>
				<xsl:copy-of select="DataType/child::*"/>
			</plx:Param>
			<!-- Get -->
			<plx:Get>
				<plx:Return>
					<plx:CallInstance name="{$PrivateMemberPrefix}{@name}" type="Field">
						<plx:CallObject>
							<plx:ThisKeyword/>
						</plx:CallObject>
					</plx:CallInstance>
				</plx:Return>
			</plx:Get>
			<!-- Set -->
			<plx:Set>
				<!-- Notify the ModelContext that we're changing the value of a property. -->
				<plx:Condition>
					<plx:Test>
						<plx:CallInstance name="On{$ClassName}{@name}Changing">
							<plx:CallObject>
								<plx:CallInstance name="{$PrivateMemberPrefix}Context" type="Field">
									<plx:CallObject>
										<plx:ThisKeyword />
									</plx:CallObject>
								</plx:CallInstance>
							</plx:CallObject>
							<plx:PassParam>
								<plx:ThisKeyword />
							</plx:PassParam>
							<plx:PassParam>
								<plx:ValueKeyword />
							</plx:PassParam>
						</plx:CallInstance>
					</plx:Test>
					<plx:Body>
						<plx:Variable name="oldValue">
							<xsl:copy-of select="DataType/@*"/>
							<xsl:copy-of select="DataType/child::*"/>
							<plx:Initialize>
								<plx:CallInstance name="{@name}" type="Property">
									<plx:CallObject>
										<plx:ThisKeyword />
									</plx:CallObject>
								</plx:CallInstance>
							</plx:Initialize>
						</plx:Variable>
						<plx:Operator type="Assign">
							<plx:Left>
								<plx:CallInstance name="{$PrivateMemberPrefix}{@name}" type="Field">
									<plx:CallObject>
										<plx:ThisKeyword/>
									</plx:CallObject>
								</plx:CallInstance>
							</plx:Left>
							<plx:Right>
								<plx:ValueKeyword/>
							</plx:Right>
						</plx:Operator>
						<plx:CallInstance name="On{$ClassName}{@name}Changed">
							<plx:CallObject>
								<plx:CallInstance name="{$PrivateMemberPrefix}Context" type="Field">
									<plx:CallObject>
										<plx:ThisKeyword />
									</plx:CallObject>
								</plx:CallInstance>
							</plx:CallObject>
							<plx:PassParam>
								<plx:ThisKeyword />
							</plx:PassParam>
							<plx:PassParam>
								<plx:Value type="Local" data="oldValue"/>
							</plx:PassParam>
						</plx:CallInstance>
					</plx:Body>
				</plx:Condition>
			</plx:Set>
		</plx:Property>
	</xsl:template>
	<xsl:template name="GenerateExternalUniquenessSimpleBinaryLookupMethods">
		<xsl:param name="Model"/>
		<xsl:variable name="firstRoleRef" select="orm:RoleSequence/orm:Role[position() = 1]/@ref" />
		<xsl:variable name="uniqueObjectName" select="$AbsorbedObjects/child::*[@oppositeRoleRef=$firstRoleRef]/../@name"/>
		<xsl:variable name="params">
			<xsl:for-each select="orm:RoleSequence/orm:Role">
				<xsl:variable name="ref" select="@ref"/>
				<plx:Param type="In">
					<!--object is an ao:AbsorbedObject or an ao:RelatedObject-->
					<xsl:variable name="object" select="$AbsorbedObjects/child::*[@oppositeRoleRef=$ref]"/>
					<xsl:variable name="oppositeRoleName" select="$object/@oppositeRoleName"/>
					<xsl:variable name="oppositeObjectName" select="$object/@oppositeObjectName"/>
					<xsl:attribute name="name">
						<xsl:choose>
							<xsl:when test="string-length($oppositeRoleName)">
								<xsl:value-of select="$oppositeRoleName"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$oppositeObjectName"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:choose>
						<xsl:when test="string-length($oppositeObjectName)">
							<xsl:attribute name="dataTypeName">
								<xsl:value-of select="$oppositeObjectName"/>
							</xsl:attribute>
						</xsl:when>
						<xsl:otherwise>
							<xsl:variable name="rolePlayerId" select="$Model/orm:Facts/descendant-or-self::node()[@id=$ref]/orm:RolePlayer/@ref"/>
							<xsl:variable name="rolePlayer" select="$Model/orm:Objects/child::*[@id=$rolePlayerId]"/>
							<xsl:variable name="dataTypeId"  select="$rolePlayer/orm:ConceptualDataType/@ref"/>
							<xsl:variable name="dataTypeFragment">
								<xsl:for-each select="$Model/orm:DataTypes/child::*[@id=$dataTypeId]">
									<xsl:call-template name="MapDataType"/>
								</xsl:for-each>
							</xsl:variable>
							<xsl:variable name="dataType" select="msxsl:node-set($dataTypeFragment)"/>
							<xsl:copy-of select="$dataType/DataType/@*"/>
							<xsl:copy-of select="$dataType/DataType/child::*"/>
						</xsl:otherwise>
					</xsl:choose>
				</plx:Param>
			</xsl:for-each>
		</xsl:variable>
		<plx:Function visibility="Public" name="Get{$uniqueObjectName}By{@Name}">
			<plx:Param type="RetVal" name="" dataTypeName="{$uniqueObjectName}"/>
			<xsl:copy-of select="$params"/>
			<plx:Return>
				<plx:CallInstance type="Indexer" name="Item">
					<plx:CallObject>
						<plx:CallInstance type="Field" name="{$PrivateMemberPrefix}{@Name}Dictionary">
							<plx:CallObject>
								<plx:ThisKeyword/>
							</plx:CallObject>
						</plx:CallInstance>
					</plx:CallObject>
					<plx:PassParam>
						<plx:CallStatic type="MethodCall" name="CreateTuple" dataTypeName="Tuple">
							<xsl:for-each select="msxsl:node-set($params)/child::*">
								<plx:PassParam>
									<plx:Value type="Parameter" data="{@name}"/>
								</plx:PassParam>
							</xsl:for-each>
						</plx:CallStatic>
					</plx:PassParam>
				</plx:CallInstance>
			</plx:Return>
		</plx:Function>
	</xsl:template>
	<xsl:template name="GenerateSimpleLookupMethods">
		<xsl:param name="ClassName"/>
		<xsl:if test="@unique='true'">
			<plx:Function name="Get{$ClassName}By{@name}" visibility="Public">
				<plx:Param type="RetVal" name="" dataTypeName="{$ClassName}"/>
				<plx:Param type="In" name="value">
					<xsl:copy-of select="DataType/@*"/>
					<xsl:copy-of select="DataType/child::*"/>
				</plx:Param>
				<plx:Return>
					<plx:CallInstance type="Indexer" name="Item">
						<plx:CallObject>
							<plx:CallInstance type="Field" name="{$PrivateMemberPrefix}{$ClassName}{@name}Dictionary">
								<plx:CallObject>
									<plx:ThisKeyword/>
								</plx:CallObject>
							</plx:CallInstance>
						</plx:CallObject>
						<plx:PassParam>
							<plx:Value type="Parameter" data="value"/>
						</plx:PassParam>
					</plx:CallInstance>
				</plx:Return>
			</plx:Function>
		</xsl:if>
	</xsl:template>
	<xsl:template name="GenerateChangeMethods">
		<xsl:param name="ClassName"/>
		<xsl:param name="Model"/>
		<xsl:variable name="oppositeRoleRef" select="@oppositeRoleRef"/>
		<xsl:variable name="externalUniquenessConstraints" select="$Model/orm:ExternalConstraints/orm:ExternalUniquenessConstraint[orm:RoleSequence/orm:Role/@ref=$oppositeRoleRef]"/>
		<xsl:if test="@unique='true'">
			<plx:Field name="{$PrivateMemberPrefix}{$ClassName}{@name}Dictionary" visibility="Private" dataTypeName="Dictionary">
				<plx:PassTypeParam>
					<xsl:copy-of select="DataType/@*"/>
					<xsl:copy-of select="DataType/child::*"/>
				</plx:PassTypeParam>
				<plx:PassTypeParam dataTypeName="{$ClassName}"/>
				<plx:Initialize>
					<plx:CallNew type="New" dataTypeName="Dictionary">
						<plx:PassTypeParam>
							<xsl:copy-of select="DataType/@*"/>
							<xsl:copy-of select="DataType/child::*"/>
						</plx:PassTypeParam>
						<plx:PassTypeParam dataTypeName="{$ClassName}"/>
					</plx:CallNew>
				</plx:Initialize>
			</plx:Field>
		</xsl:if>
		<plx:Function visibility="Private" name="On{$ClassName}{@name}Changing">
			<plx:Param type="RetVal" name="" dataTypeName="Boolean" dataTypeQualifier="System"/>
			<plx:Param type="In" name="instance" dataTypeName="{$ClassName}"/>
			<plx:Param type="In" name="newValue">
				<xsl:copy-of select="DataType/@*"/>
				<xsl:copy-of select="DataType/child::*"/>
			</plx:Param>
			<xsl:if test="@unique='true'">
				<plx:Condition>
					<plx:Test>
						<plx:Operator type="Inequality">
							<plx:Left>
								<plx:Value type="Parameter" data="newValue"/>
							</plx:Left>
							<plx:Right>
								<plx:NullObjectKeyword />
							</plx:Right>
						</plx:Operator>
					</plx:Test>
					<plx:Body>
						<plx:Variable name="currentInstance" dataTypeName="{$ClassName}">
							<plx:Initialize>
								<plx:Value type="Parameter" data="instance"/>
							</plx:Initialize>
						</plx:Variable>
						<plx:Condition>
							<plx:Test>
								<plx:CallInstance name="TryGetValue" type="MethodCall">
									<plx:CallObject>
										<plx:CallInstance name="{$PrivateMemberPrefix}{$ClassName}{@name}Dictionary" type="Field">
											<plx:CallObject>
												<plx:ThisKeyword />
											</plx:CallObject>
										</plx:CallInstance>
									</plx:CallObject>
									<plx:PassParam>
										<plx:Value type="Parameter" data="newValue"/>
									</plx:PassParam>
									<plx:PassParam passStyle="Out">
										<plx:Value type="Local" data="currentInstance"/>
									</plx:PassParam>
								</plx:CallInstance>
							</plx:Test>
							<plx:Body>
								<plx:Condition>
									<plx:Test>
										<plx:Operator type="Inequality">
											<plx:Left>
												<plx:Value type="Local" data="currentInstance"/>
											</plx:Left>
											<plx:Right>
												<plx:Value type="Parameter" data="instance"/>
											</plx:Right>
										</plx:Operator>
									</plx:Test>
									<plx:Body>
										<plx:Return>
											<plx:FalseKeyword/>
										</plx:Return>
									</plx:Body>
								</plx:Condition>
							</plx:Body>
						</plx:Condition>
					</plx:Body>
				</plx:Condition>
			</xsl:if>
			<xsl:if test="count($externalUniquenessConstraints)">
				<xsl:for-each select="$externalUniquenessConstraints">
					<!--<xsl:variable name="passTypeParams">
						<xsl:for-each select="orm:RoleSequence/orm:Role">
							<xsl:variable name="ref" select="@ref"/>
							<plx:PassTypeParam>
								-->
					<!--object is an ao:AbsorbedObject or an ao:RelatedObject-->
					<!--
								<xsl:variable name="object" select="$AbsorbedObjects/child::*[@oppositeRoleRef=$ref]"/>
								<xsl:variable name="oppositeObjectName" select="$object/@oppositeObjectName"/>
								<xsl:choose>
									<xsl:when test="string-length($oppositeObjectName)">
										<xsl:attribute name="dataTypeName">
											<xsl:value-of select="$oppositeObjectName"/>
										</xsl:attribute>
									</xsl:when>
									<xsl:otherwise>
										<xsl:variable name="rolePlayerId" select="$object/@ref"/>
										<xsl:variable name="rolePlayer" select="$Model/orm:Objects/child::*[@id=$rolePlayerId]"/>
										<xsl:variable name="dataTypeId"  select="$rolePlayer/orm:ConceptualDataType/@ref"/>
										<xsl:variable name="dataTypeFragment">
											<xsl:for-each select="$Model/orm:DataTypes/child::*[@id=$dataTypeId]">
												<xsl:call-template name="MapDataType"/>
											</xsl:for-each>
										</xsl:variable>
										<xsl:variable name="dataType" select="msxsl:node-set($dataTypeFragment)"/>
										<xsl:copy-of select="$dataType/DataType/@*"/>
										<xsl:copy-of select="$dataType/DataType/child::*"/>
									</xsl:otherwise>
								</xsl:choose>
							</plx:PassTypeParam>
						</xsl:for-each>												
					</xsl:variable>-->
					<xsl:variable name="passParams">
						<xsl:for-each select="orm:RoleSequence/orm:Role">
							<xsl:variable name="ref" select="@ref"/>
							<plx:PassParam>
								<!--object is an ao:AbsorbedObject or an ao:RelatedObject-->
								<xsl:variable name="object" select="$AbsorbedObjects/child::*[@oppositeRoleRef=$ref]"/>
								<xsl:choose>
									<xsl:when test="$ref=$oppositeRoleRef">
										<plx:Value type="Parameter" data="newValue" />
									</xsl:when>
									<xsl:otherwise>
										<xsl:variable name="oppositeRoleName" select="$object/@oppositeRoleName"/>
										<plx:CallInstance type="Property">
											<xsl:attribute name="name">
												<xsl:choose>
													<xsl:when test="string-length($oppositeRoleName)">
														<xsl:value-of select="$oppositeRoleName"/>
													</xsl:when>
													<xsl:otherwise>
														<xsl:value-of select="$object/@oppositeObjectName"/>
													</xsl:otherwise>
												</xsl:choose>
											</xsl:attribute>
											<plx:CallObject>
												<plx:Value type="Parameter" data="instance"/>
											</plx:CallObject>
										</plx:CallInstance>
									</xsl:otherwise>
								</xsl:choose>
							</plx:PassParam>
						</xsl:for-each>
					</xsl:variable>
					<plx:Condition>
						<plx:Test>
							<plx:Operator type="BooleanNot">
								<plx:CallInstance name="On{@Name}Changing" type="MethodCall">
									<plx:CallObject>
										<plx:ThisKeyword/>
									</plx:CallObject>
									<plx:PassParam>
										<plx:Value type="Parameter" data="instance"/>
									</plx:PassParam>
									<plx:PassParam>
										<plx:CallStatic type="MethodCall" dataTypeName="Tuple" name="CreateTuple">
											<!--Plix does not currently support calling Generic Methods-->
											<!--<xsl:copy-of select="$passTypeParams"/>-->
											<xsl:copy-of select="$passParams"/>
										</plx:CallStatic>
									</plx:PassParam>
								</plx:CallInstance>
							</plx:Operator>
						</plx:Test>
						<plx:Body>
							<plx:Return>
								<plx:FalseKeyword />
							</plx:Return>
						</plx:Body>
					</plx:Condition>
				</xsl:for-each>
			</xsl:if>
			<plx:Return>
				<plx:TrueKeyword />
			</plx:Return>
		</plx:Function>
		<plx:Function visibility="Private" name="On{$ClassName}{@name}Changed">
			<plx:Param type="In" name="instance" dataTypeName="{$ClassName}"/>
			<plx:Param type="In" name="oldValue">
				<xsl:copy-of select="DataType/@*"/>
				<xsl:copy-of select="DataType/child::*"/>
			</plx:Param>
			<xsl:if test="@unique='true'">
				<plx:Condition>
					<plx:Test>
						<plx:Operator type="Inequality">
							<plx:Left>
								<plx:Value type="Parameter" data="oldValue"/>
							</plx:Left>
							<plx:Right>
								<plx:NullObjectKeyword />
							</plx:Right>
						</plx:Operator>
					</plx:Test>
					<plx:Body>
						<plx:CallInstance name="Remove">
							<plx:CallObject>
								<plx:CallInstance name="{$PrivateMemberPrefix}{$ClassName}{@name}Dictionary" type="Field">
									<plx:CallObject>
										<plx:ThisKeyword />
									</plx:CallObject>
								</plx:CallInstance>
							</plx:CallObject>
							<plx:PassParam>
								<plx:Value type="Parameter" data="oldValue"/>
							</plx:PassParam>
						</plx:CallInstance>
					</plx:Body>
				</plx:Condition>
				<plx:Condition>
					<plx:Test>
						<plx:Operator type="Inequality">
							<plx:Left>
								<plx:CallInstance name="{@name}" type="Property">
									<plx:CallObject>
										<plx:Value type="Parameter" data="instance"/>
									</plx:CallObject>
								</plx:CallInstance>
							</plx:Left>
							<plx:Right>
								<plx:NullObjectKeyword />
							</plx:Right>
						</plx:Operator>
					</plx:Test>
					<plx:Body>
						<plx:CallInstance name="Add">
							<plx:CallObject>
								<plx:CallInstance name="{$PrivateMemberPrefix}{$ClassName}{@name}Dictionary" type="Field">
									<plx:CallObject>
										<plx:ThisKeyword />
									</plx:CallObject>
								</plx:CallInstance>
							</plx:CallObject>
							<plx:PassParam>
								<plx:CallInstance name="{@name}" type="Property">
									<plx:CallObject>
										<plx:Value type="Parameter" data="instance"/>
									</plx:CallObject>
								</plx:CallInstance>
							</plx:PassParam>
							<plx:PassParam>
								<plx:Value type="Parameter" data="instance"/>
							</plx:PassParam>
						</plx:CallInstance>
					</plx:Body>
				</plx:Condition>
			</xsl:if>
			<xsl:if test="count($externalUniquenessConstraints)">
				<xsl:for-each select="$externalUniquenessConstraints">
					<xsl:variable name="oldValuePassParams">
						<xsl:for-each select="orm:RoleSequence/orm:Role">
							<xsl:variable name="ref" select="@ref"/>
							<plx:PassParam>
								<!--object is an ao:AbsorbedObject or an ao:RelatedObject-->
								<xsl:variable name="object" select="$AbsorbedObjects/child::*[@oppositeRoleRef=$ref]"/>
								<xsl:choose>
									<xsl:when test="$ref=$oppositeRoleRef">
										<plx:Value type="Parameter" data="oldValue" />
									</xsl:when>
									<xsl:otherwise>
										<xsl:variable name="oppositeRoleName" select="$object/@oppositeRoleName"/>
										<plx:CallInstance type="Property">
											<xsl:attribute name="name">
												<xsl:choose>
													<xsl:when test="string-length($oppositeRoleName)">
														<xsl:value-of select="$oppositeRoleName"/>
													</xsl:when>
													<xsl:otherwise>
														<xsl:value-of select="$object/@oppositeObjectName"/>
													</xsl:otherwise>
												</xsl:choose>
											</xsl:attribute>
											<plx:CallObject>
												<plx:Value type="Parameter" data="instance"/>
											</plx:CallObject>
										</plx:CallInstance>
									</xsl:otherwise>
								</xsl:choose>
							</plx:PassParam>
						</xsl:for-each>
					</xsl:variable>
					<xsl:variable name="newValuePassParams">
						<xsl:for-each select="orm:RoleSequence/orm:Role">
							<xsl:variable name="ref" select="@ref"/>
							<plx:PassParam>
								<!--object is an ao:AbsorbedObject or an ao:RelatedObject-->
								<xsl:variable name="object" select="$AbsorbedObjects/child::*[@oppositeRoleRef=$ref]"/>
								<xsl:variable name="oppositeRoleName" select="$object/@oppositeRoleName"/>
								<plx:CallInstance type="Property">
									<xsl:attribute name="name">
										<xsl:choose>
											<xsl:when test="string-length($oppositeRoleName)">
												<xsl:value-of select="$oppositeRoleName"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="$object/@oppositeObjectName"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:attribute>
									<plx:CallObject>
										<plx:Value type="Parameter" data="instance"/>
									</plx:CallObject>
								</plx:CallInstance>
							</plx:PassParam>
						</xsl:for-each>
					</xsl:variable>
					<plx:CallInstance name="On{@Name}Changed" type="MethodCall">
						<plx:CallObject>
							<plx:ThisKeyword/>
						</plx:CallObject>
						<plx:PassParam>
							<plx:Value type="Parameter" data="instance"/>
						</plx:PassParam>
						<plx:PassParam>
							<plx:CallStatic type="MethodCall" dataTypeName="Tuple" name="CreateTuple">
								<!--Plix does not currently support calling Generic Methods-->
								<!--<xsl:copy-of select="$passTypeParams"/>-->
								<xsl:copy-of select="$oldValuePassParams"/>
							</plx:CallStatic>
						</plx:PassParam>
						<plx:PassParam>
							<plx:CallStatic type="MethodCall" dataTypeName="Tuple" name="CreateTuple">
								<!--Plix does not currently support calling Generic Methods-->
								<!--<xsl:copy-of select="$passTypeParams"/>-->
								<xsl:copy-of select="$newValuePassParams"/>
							</plx:CallStatic>
						</plx:PassParam>
					</plx:CallInstance>
				</xsl:for-each>
			</xsl:if>
		</plx:Function>
	</xsl:template>
	<xsl:template match="Property" mode="GenerateConstructorParams">
		<!--'pass' is a boolean to indicate whether or not to generate a PassParam (pass=true)
		or a Param (pass=false) plx tag-->
		<xsl:param name="Pass"/>
		<xsl:param name="ForceMandatory" select="false()"/>
		<xsl:if test="$ForceMandatory or @mandatory='true' or @mandatory='relaxed'">
			<xsl:choose>
				<xsl:when test="$Pass">
					<plx:PassParam>
						<plx:Value type="Parameter" data="{@name}"/>
					</plx:PassParam>
				</xsl:when>
				<xsl:otherwise>
					<plx:Param name="{@name}" type="In">
						<xsl:for-each select="DataType">
							<xsl:copy-of select="@*"/>
							<xsl:copy-of select="child::*"/>
						</xsl:for-each>
					</plx:Param>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>
	<xsl:template match="Property" mode="GenerateConstructorParamsSingleValuesOnly">
		<!--'pass' is a boolean to indicate whether or not to generate a PassParam (pass=true)
		or a Param (pass=false) plx tag-->
		<xsl:param name="Pass"/>
		<xsl:param name="ForceMandatory" select="false()"/>
		<xsl:if test="@basedOn='AbsorbedObject'">
			<xsl:apply-templates mode="GenerateConstructorParams" select=".">
				<xsl:with-param name="Pass" select="$Pass"/>
				<xsl:with-param name="ForceMandatory" select="$ForceMandatory"/>
			</xsl:apply-templates>
		</xsl:if>
	</xsl:template>
	<xsl:template name="GenerateConstructorAssignmentForConstructorParams">
		<xsl:param name="ForceMandatory" select="false()"/>
		<xsl:if test="$ForceMandatory or @mandatory='true' or @mandatory='relaxed'">
			<plx:Operator type="Assign">
				<plx:Left>
					<plx:CallInstance name="{@name}" type="Field">
						<plx:CallObject>
							<plx:ThisKeyword/>
						</plx:CallObject>
					</plx:CallInstance>
				</plx:Left>
				<plx:Right>
					<plx:Value type="Parameter" data="{@name}"/>
				</plx:Right>
			</plx:Operator>
		</xsl:if>
	</xsl:template>
	<xsl:template name="AssignConstructorContext">
		<plx:Operator type="Assign">
			<plx:Left>
				<plx:CallInstance name="{$PrivateMemberPrefix}Context" type="Field">
					<plx:CallObject>
						<plx:ThisKeyword/>
					</plx:CallObject>
				</plx:CallInstance>
			</plx:Left>
			<plx:Right>
				<plx:Value type="Parameter" data="context"/>
			</plx:Right>
		</plx:Operator>
	</xsl:template>
	<xsl:template match="ao:Object | ao:Association" mode="AddPropertiesToAbsorbedObjects">
		<xsl:param name="Model"/>
		<xsl:variable name="propertyFragment">
			<xsl:apply-templates mode="WalkAbsorbedObjects" select="child::*">
				<xsl:with-param name="Model" select="$Model"/>
			</xsl:apply-templates>
		</xsl:variable>
		<xsl:copy>
			<xsl:copy-of select="@*"/>
			<xsl:copy-of select="child::*"/>
			<Properties>
				<xsl:copy-of select="$propertyFragment"/>
			</Properties>
		</xsl:copy>
	</xsl:template>
	<xsl:template name="GenerateExternalUniquenessSimpleBinaryDictionaries">
		<xsl:param name="Model"/>
		<xsl:variable name="firstRoleRef" select="orm:RoleSequence/orm:Role[position() = 1]/@ref" />
		<xsl:variable name="uniqueObjectName" select="$AbsorbedObjects/child::*[@oppositeRoleRef=$firstRoleRef]/../@name"/>
		<xsl:variable name="passTypeParams">
			<xsl:for-each select="orm:RoleSequence/orm:Role">
				<xsl:variable name="ref" select="@ref"/>
				<plx:PassTypeParam>
					<!--object is an ao:AbsorbedObject or an ao:RelatedObject-->
					<xsl:variable name="object" select="$AbsorbedObjects/child::*[@oppositeRoleRef=$ref]"/>
					<xsl:variable name="oppositeObjectName" select="$object/@oppositeObjectName"/>
					<xsl:choose>
						<xsl:when test="string-length($oppositeObjectName)">
							<xsl:attribute name="dataTypeName">
								<xsl:value-of select="$oppositeObjectName"/>
							</xsl:attribute>
						</xsl:when>
						<xsl:otherwise>
							<xsl:variable name="rolePlayerId" select="$object/@ref"/>
							<xsl:variable name="rolePlayer" select="$Model/orm:Objects/child::*[@id=$rolePlayerId]"/>
							<xsl:variable name="dataTypeId"  select="$rolePlayer/orm:ConceptualDataType/@ref"/>
							<xsl:variable name="dataTypeFragment">
								<xsl:for-each select="$Model/orm:DataTypes/child::*[@id=$dataTypeId]">
									<xsl:call-template name="MapDataType"/>
								</xsl:for-each>
							</xsl:variable>
							<xsl:variable name="dataType" select="msxsl:node-set($dataTypeFragment)"/>
							<xsl:copy-of select="$dataType/DataType/@*"/>
							<xsl:copy-of select="$dataType/DataType/child::*"/>
						</xsl:otherwise>
					</xsl:choose>
				</plx:PassTypeParam>
			</xsl:for-each>
		</xsl:variable>
		<plx:Field name="{$PrivateMemberPrefix}{@Name}Dictionary" dataTypeName="Dictionary" visibility="Private">
			<plx:PassTypeParam dataTypeName="Tuple">
				<xsl:copy-of select="$passTypeParams"/>
			</plx:PassTypeParam>
			<plx:PassTypeParam dataTypeName="{$uniqueObjectName}"/>
			<plx:Initialize>
				<plx:CallNew type="New" dataTypeName="Dictionary">
					<plx:PassTypeParam dataTypeName="Tuple">
						<xsl:copy-of select="$passTypeParams"/>
					</plx:PassTypeParam>
					<plx:PassTypeParam dataTypeName="{$uniqueObjectName}"/>
				</plx:CallNew>
			</plx:Initialize>
		</plx:Field>
		<plx:Function visibility="Private" name ="On{@Name}Changing">
			<plx:Param name="" dataTypeName="Boolean" dataTypeQualifier="System" type="RetVal" />
			<plx:Param name="instance" dataTypeName="{$uniqueObjectName}" type="In" />
			<plx:Param name="newValue" dataTypeName="Tuple" type="In">
				<xsl:copy-of select="$passTypeParams"/>
			</plx:Param>
			<plx:Condition>
				<plx:Test>
					<plx:Operator type="Inequality">
						<plx:Left>
							<plx:Value type="Parameter" data="newValue"/>
						</plx:Left>
						<plx:Right>
							<plx:NullObjectKeyword/>
						</plx:Right>
					</plx:Operator>
				</plx:Test>
				<plx:Body>
					<plx:Variable name="currentInstance" dataTypeName="{$uniqueObjectName}">
						<plx:Initialize>
							<plx:Value type="Parameter" data="instance"/>
						</plx:Initialize>
					</plx:Variable>
					<plx:Condition>
						<plx:Test>
							<plx:CallInstance name="TryGetValue" type="MethodCall">
								<plx:CallObject>
									<plx:CallInstance name="{$PrivateMemberPrefix}{@Name}Dictionary" type="Field">
										<plx:CallObject>
											<plx:ThisKeyword />
										</plx:CallObject>
									</plx:CallInstance>
								</plx:CallObject>
								<plx:PassParam>
									<plx:Value type="Parameter" data="newValue"/>
								</plx:PassParam>
								<plx:PassParam passStyle="Out">
									<plx:Value type="Local" data="currentInstance"/>
								</plx:PassParam>
							</plx:CallInstance>
						</plx:Test>
						<plx:Body>
							<plx:Return>
								<plx:Operator type="IdentityEquality">
									<plx:Left>
										<plx:Value type="Local" data="currentInstance"/>
									</plx:Left>
									<plx:Right>
										<plx:Value type="Parameter" data="instance"/>
									</plx:Right>
								</plx:Operator>
							</plx:Return>
						</plx:Body>
					</plx:Condition>
				</plx:Body>
			</plx:Condition>
			<plx:Return>
				<plx:TrueKeyword/>
			</plx:Return>
		</plx:Function>
		<plx:Function visibility="Private" name ="On{@Name}Changed">
			<plx:Param name="instance" dataTypeName="{$uniqueObjectName}" type="In" />
			<plx:Param name="oldValue" dataTypeName="Tuple" type="In">
				<xsl:copy-of select="$passTypeParams"/>
			</plx:Param>
			<plx:Param name="newValue" dataTypeName="Tuple" type="In">
				<xsl:copy-of select="$passTypeParams"/>
			</plx:Param>
			<plx:Condition>
				<plx:Test>
					<plx:Operator type="Inequality">
						<plx:Left>
							<plx:Value type="Parameter" data="oldValue"/>
						</plx:Left>
						<plx:Right>
							<plx:NullObjectKeyword/>
						</plx:Right>
					</plx:Operator>
				</plx:Test>
				<plx:Body>
					<plx:CallInstance name="Remove" type="MethodCall">
						<plx:CallObject>
							<plx:CallInstance name="{$PrivateMemberPrefix}{@Name}Dictionary" type="Field">
								<plx:CallObject>
									<plx:ThisKeyword/>
								</plx:CallObject>
							</plx:CallInstance>
						</plx:CallObject>
						<plx:PassParam>
							<plx:Value type="Parameter" data="oldValue"/>
						</plx:PassParam>
					</plx:CallInstance>
				</plx:Body>
			</plx:Condition>
			<plx:Condition>
				<plx:Test>
					<plx:Operator type="Inequality">
						<plx:Left>
							<plx:Value type="Parameter" data="newValue"/>
						</plx:Left>
						<plx:Right>
							<plx:NullObjectKeyword/>
						</plx:Right>
					</plx:Operator>
				</plx:Test>
				<plx:Body>
					<plx:CallInstance name="Add" type="MethodCall">
						<plx:CallObject>
							<plx:CallInstance name="{$PrivateMemberPrefix}{@Name}Dictionary" type="Field">
								<plx:CallObject>
									<plx:ThisKeyword/>
								</plx:CallObject>
							</plx:CallInstance>
						</plx:CallObject>
						<plx:PassParam>
							<plx:Value type="Parameter" data="newValue"/>
						</plx:PassParam>
						<plx:PassParam>
							<plx:Value type="Parameter" data="instance"/>
						</plx:PassParam>
					</plx:CallInstance>
				</plx:Body>
			</plx:Condition>
		</plx:Function>
	</xsl:template>
	<xsl:template match="ormRoot:ORM2">
		<xsl:text disable-output-escaping="yes"><![CDATA[<!--<ao>]]></xsl:text>
		<xsl:copy-of select="$AbsorbedObjects"/>
		<xsl:text disable-output-escaping="yes"><![CDATA[</ao>-->]]></xsl:text>
		<xsl:apply-templates mode="Main" select="orm:ORMModel"/>
	</xsl:template>
	<xsl:include href="ModelContext.xslt"/>
	<xsl:template match="orm:ORMModel" mode="Main">
		<xsl:variable name="AbsorbedObjectsExFragment">
			<xsl:apply-templates select="$AbsorbedObjects" mode="AddPropertiesToAbsorbedObjects">
				<xsl:with-param name="Model" select="."/>
			</xsl:apply-templates>
		</xsl:variable>
		<xsl:variable name="AbsorbedObjectsEx" select="msxsl:node-set($AbsorbedObjectsExFragment)/child::*"/>
		<plx:Root>
			<plx:Using name="System" />
			<plx:Using name="System.Collections.Generic" />
			<plx:Namespace name="{$CustomToolNamespace}">
				<xsl:apply-templates mode="ModelContext" select="."/>
				<xsl:call-template name="GenerateFactoryInterface">
					<xsl:with-param name="AbsorbedObjectsEx" select="$AbsorbedObjectsEx"/>
				</xsl:call-template>
				<xsl:apply-templates mode="WalkAbsorbedObjects" select="$AbsorbedObjectsEx">
					<xsl:with-param name="Model" select="."/>
				</xsl:apply-templates>
			</plx:Namespace>
		</plx:Root>
	</xsl:template>
	<xsl:template match="ao:Object" mode="WalkAbsorbedObjects">
		<xsl:param name="Model"/>
		<xsl:if test="@type='EntityType' or @type='ObjectifiedType'">
			<xsl:variable name="property" select="./Properties/Property"/>
			<!--<xsl:variable name="AbsorbedMandatory" select="$Model/orm:ORMModel/orm:Facts/orm:Fact/orm:FactRoles/orm:Role[@IsMandatory='true']/@DataType"/>-->
			<xsl:variable name="className" select="@name"/>
			<plx:Class visibility="Public" abstract="true" partial="true" name="{$className}">
				<plx:Function ctor="true" visibility="Protected"/>
				<xsl:for-each select="$property">
					<xsl:call-template name="GenerateAbstractProperty"/>
				</xsl:for-each>
			</plx:Class>
			<plx:Class visibility="Public" partial="true" name="{$ModelContextName}">
				<xsl:for-each select="$property">
					<xsl:call-template name="GenerateSimpleLookupMethods">
						<xsl:with-param name="ClassName" select="$className"/>
					</xsl:call-template>
					<xsl:call-template name="GenerateChangeMethods">
						<xsl:with-param name="ClassName" select="$className"/>
						<xsl:with-param name="Model" select="$Model"/>
					</xsl:call-template>
				</xsl:for-each>
				<plx:Function name="Create{$className}" visibility="Private">
					<plx:InterfaceMember dataTypeName="{$IModelFactoryName}" member="Create{$className}"/>
					<xsl:apply-templates select="$property" mode="GenerateConstructorParams">
						<xsl:with-param name="Pass" select="false()"/>
					</xsl:apply-templates>
					<plx:Param type="RetVal" name="" dataTypeName="{$className}"/>
					<plx:Return>
						<plx:CallNew type="New" dataTypeName="{$className}{$ImplementationClassSuffix}">
							<plx:PassParam passStyle="In">
								<plx:ThisKeyword/>
							</plx:PassParam>
							<xsl:apply-templates select="$property" mode="GenerateConstructorParams">
								<xsl:with-param name="Pass" select="true()"/>
							</xsl:apply-templates>
						</plx:CallNew>
					</plx:Return>
				</plx:Function>
				<plx:Class visibility="Private" sealed="true" partial="true" name="{$className}{$ImplementationClassSuffix}">
					<plx:DerivesFromClass dataTypeName="{$className}"/>
					<plx:Field name="{$PrivateMemberPrefix}Context" visibility="Private" dataTypeName="{$ModelContextName}"/>
					<plx:Function ctor="true" visibility="Public">
						<plx:Param name="context" dataTypeName="{$ModelContextName}"/>
						<xsl:apply-templates select="$property" mode="GenerateConstructorParams">
							<xsl:with-param name="Pass" select="false()"/>
						</xsl:apply-templates>
						<xsl:call-template name="AssignConstructorContext"/>
						<xsl:for-each select="$property">
							<xsl:call-template name="GenerateConstructorAssignmentForConstructorParams"/>
						</xsl:for-each>
					</plx:Function>
					<!-- TODO: make version with parameters based on required roles -->
					<xsl:for-each select="$property">
						<xsl:call-template name="GenerateBackedProperty">
							<xsl:with-param name="ClassName" select="$className"/>
							<xsl:with-param name="InitializeFields" select="true()"/>
						</xsl:call-template>
					</xsl:for-each>
				</plx:Class>
			</plx:Class>
		</xsl:if>
	</xsl:template>
	<xsl:template match="ao:Association" mode="WalkAbsorbedObjects">
		<xsl:param name="Model"/>
		<xsl:variable name="className">
			<xsl:value-of select="@name"/>
			<xsl:value-of select="$AssociationClassDecorator"/>
		</xsl:variable>
		<xsl:variable name="property" select="./Properties/Property"/>
		<plx:Class visibility="Public" abstract="true" partial="true" name="{$className}">
			<plx:Function ctor="true" visibility="Protected"/>
			<xsl:for-each select="$property">
				<xsl:call-template name="GenerateAbstractProperty"/>
			</xsl:for-each>
		</plx:Class>
		<plx:Class visibility="Public" partial="true" name="{$ModelContextName}">
			<xsl:for-each select="$property">
				<xsl:call-template name="GenerateSimpleLookupMethods">
					<xsl:with-param name="ClassName" select="$className"/>
				</xsl:call-template>
				<xsl:call-template name="GenerateChangeMethods">
					<xsl:with-param name="ClassName" select="$className"/>
					<xsl:with-param name="Model" select="$Model"/>
				</xsl:call-template>
			</xsl:for-each>
			<plx:Function name="Create{$className}" visibility="Private">
				<plx:InterfaceMember dataTypeName="{$IModelFactoryName}" member="Create{$className}"/>
				<xsl:apply-templates select="$property" mode="GenerateConstructorParams">
					<xsl:with-param name="Pass" select="false()"/>
					<xsl:with-param name="ForceMandatory" select="true()"/>
				</xsl:apply-templates>
				<plx:Param type="RetVal" name="" dataTypeName="{$className}"/>
				<plx:Return>
					<plx:CallNew type="New" dataTypeName="{$className}{$ImplementationClassSuffix}">
						<plx:PassParam passStyle="In">
							<plx:ThisKeyword/>
						</plx:PassParam>
						<xsl:apply-templates select="$property" mode="GenerateConstructorParams">
							<xsl:with-param name="Pass" select="true()"/>
							<xsl:with-param name="ForceMandatory" select="true()"/>
						</xsl:apply-templates>
					</plx:CallNew>
				</plx:Return>
			</plx:Function>
			<plx:Class visibility="Private" sealed="true" partial="true" name="{$className}{$ImplementationClassSuffix}">
				<plx:DerivesFromClass dataTypeName="{$className}"/>
				<plx:Field name="{$PrivateMemberPrefix}Context" visibility="Private" dataTypeName="{$ModelContextName}"/>
				<plx:Function ctor="true" visibility="Protected">
					<plx:Param name="context" dataTypeName="{$ModelContextName}"/>
					<xsl:apply-templates select="$property" mode="GenerateConstructorParams">
						<xsl:with-param name="Pass" select="false()"/>
						<xsl:with-param name="ForceMandatory" select="true()"/>
					</xsl:apply-templates>
					<xsl:call-template name="AssignConstructorContext"/>
					<xsl:for-each select="$property">
						<xsl:call-template name="GenerateConstructorAssignmentForConstructorParams">
							<xsl:with-param name="ForceMandatory" select="true()"/>
						</xsl:call-template>
					</xsl:for-each>
				</plx:Function>
				<xsl:for-each select="$property">
					<xsl:call-template name="GenerateBackedProperty">
						<xsl:with-param name="InitializeFields" select="false()"/>
						<xsl:with-param name="ClassName" select="$className"/>
					</xsl:call-template>
				</xsl:for-each>
			</plx:Class>
		</plx:Class>
	</xsl:template>
	<xsl:template match="ao:Object/ao:RelatedObject" mode="WalkAbsorbedObjects">
		<xsl:param name="Model"/>
		<!-- The generated code is similar for both binary related objects and
		     association objects. Build a Property element with a name attribute and
			 DataType element for all both cases, then spit the property and its backing field
			 using the GenerateBackedProperty template. -->
		<!--		<xsl:variable name="property">-->
		<Property mandatory="{@mandatory}" unique="{@unique}" oppositeRoleRef="{@oppositeRoleRef}">
			<xsl:attribute name="basedOn">
				<xsl:value-of select="local-name()"/>
			</xsl:attribute>
			<xsl:choose>
				<xsl:when test="@oppositeObjectRef">
					<!-- Related to an object by a binary fact -->
					<xsl:attribute name="name">
						<xsl:choose>
							<xsl:when test="string-length(@oppositeRoleName)">
								<xsl:value-of select="@oppositeRoleName"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="@oppositeObjectName"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<DataType>
						<xsl:choose>
							<xsl:when test="contains(@multiplicity, 'Many')">
								<xsl:attribute name="dataTypeName">ICollection</xsl:attribute>
								<plx:PassTypeParam dataTypeName="{@oppositeObjectName}"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:attribute name="dataTypeName">
									<xsl:value-of select="@oppositeObjectName"/>
								</xsl:attribute>
							</xsl:otherwise>
						</xsl:choose>
					</DataType>
				</xsl:when>
				<xsl:otherwise>
					<!-- Related to an association object -->
					<xsl:variable name="factId" select="@factRef"/>
					<xsl:variable name="relatedFact" select="$Model/orm:Facts/orm:*[@id=$factId]"/>
					<xsl:variable name="factName" select="$relatedFact/@Name"/>
					<xsl:attribute name="name">
						<xsl:choose>
							<xsl:when test="string-length(@roleName)">
								<xsl:value-of select="@roleName"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$factName"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<DataType>
						<xsl:choose>
							<xsl:when test="@arity=1">
								<xsl:attribute name="dataTypeName">Nullable</xsl:attribute>
								<plx:PassTypeParam dataTypeName="Boolean"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:attribute name="dataTypeName">ICollection</xsl:attribute>
								<plx:PassTypeParam dataTypeName="{$factName}{$AssociationClassDecorator}"/>
							</xsl:otherwise>
						</xsl:choose>
					</DataType>
				</xsl:otherwise>
			</xsl:choose>
		</Property>
		<!--		</xsl:variable>-->
		<!--		<xsl:for-each select="$property/child::*">
			<xsl:call-template name="GenerateBackedProperty"/>
		</xsl:for-each>-->
	</xsl:template>
	<xsl:template match="ao:Object/ao:AbsorbedObject" mode="WalkAbsorbedObjects">
		<xsl:param name="Model"/>
		<!-- Absorbed objects can also absorb other objects. Use the deepest defined role name
		     and the deepest fact to get names for the generated property. The data type
			 for the property will always be type of the deepest absorbed object. -->
		<!--		<xsl:variable name="absorbedData">-->
		<xsl:apply-templates select="." mode="InterpretAbsorbed">
			<xsl:with-param name="Model" select="$Model"/>
		</xsl:apply-templates>
		<!--		</xsl:variable>-->
		<!--		<xsl:for-each select="$absorbedData/child::*">
			<xsl:variable name="property">
				<Property name="{@roleName}">
					<xsl:copy-of select="DataType"/>
				</Property>
			</xsl:variable>
			<xsl:for-each select="$property/child::*">
				<xsl:call-template name="GenerateBackedProperty"/>
			</xsl:for-each>
		</xsl:for-each>-->
	</xsl:template>
	<xsl:template match="ao:AbsorbedObject" mode="InterpretAbsorbed">
		<xsl:param name="Model"/>
		<xsl:choose>
			<xsl:when test="count(ao:AbsorbedObject)">
				<xsl:variable name="nestedTemp">
					<xsl:apply-templates select="ao:AbsorbedObject" mode="InterpretAbsorbed">
						<xsl:with-param name="Model" select="$Model"/>
					</xsl:apply-templates>
				</xsl:variable>
				<xsl:variable name="nested" select="msxsl:node-set($nestedTemp)/child::*"/>
				<Property multiplicity="{@multiplicity}" mandatory="{@mandatory}" unique="{@unique}" oppositeRoleRef="{@oppositeRoleRef}">
					<xsl:attribute name="name">
						<xsl:choose>
							<xsl:when test="string-length(@roleName)">
								<xsl:value-of select="@roleName"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="@name"/>
							</xsl:otherwise>
						</xsl:choose>
						<xsl:value-of select="$nested/@roleName"/>
					</xsl:attribute>
					<xsl:copy-of select="$nested/DataType"/>
				</Property>
			</xsl:when>
			<xsl:otherwise>
				<Property name="{@roleName}" multiplicity="{@multiplicity}" mandatory="{@mandatory}" unique="{@unique}" oppositeRoleRef="{@oppositeRoleRef}">
					<xsl:attribute name="basedOn">
						<xsl:value-of select="local-name()"/>
					</xsl:attribute>
					<!--<xsl:if test="0=string-length(@roleName)">
						<xsl:attribute name="name">
							<xsl:value-of select="@name"/>
						</xsl:attribute>
					</xsl:if>-->
					<xsl:attribute name="name">
						<xsl:choose>
							<xsl:when test="0=string-length(@oppositeRoleName)">
								<xsl:value-of select="@name"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="@oppositeRoleName"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:if test="@type='ValueType'">
						<xsl:variable name="objectId" select="@ref"/>
						<xsl:variable name="valueObject" select="$Model/orm:Objects/orm:ValueType[@id=$objectId]"/>
						<xsl:variable name="dataTypeId" select="$valueObject/orm:ConceptualDataType/@ref"/>
						<xsl:for-each select="$Model/orm:DataTypes/child::*[@id=$dataTypeId]">
							<xsl:call-template name="MapDataType"/>
						</xsl:for-each>
					</xsl:if>
				</Property>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="ao:Association/ao:RelatedObject" mode="WalkAbsorbedObjects">
		<xsl:param name="Model"/>
		<!--		<ao:RelatedObject type="{@type}" objectRef="{@id}" roleRef="{$roleId}" roleName="{$roleName}" className="{@name}">
			<xsl:if test="$hasMany">
				<xsl:attribute name="multiplicity">Many</xsl:attribute>
			</xsl:if>
		</ao:RelatedObject>	-->
		<!--		<xsl:variable name="property">-->
		<Property unique="{@unique}" oppositeRoleRef="{@oppositeRoleRef}">
			<xsl:attribute name="name">
				<xsl:choose>
					<xsl:when test="string-length(@roleName)">
						<xsl:value-of select="@roleName"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="@className"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<DataType>
				<xsl:choose>
					<xsl:when test="contains(@multiplicity, 'Many')">
						<xsl:attribute name="dataTypeName">ICollection</xsl:attribute>
						<plx:PassTypeParam dataTypeName="{@className}"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:attribute name="dataTypeName">
							<xsl:value-of select="@className"/>
						</xsl:attribute>
					</xsl:otherwise>
				</xsl:choose>
			</DataType>
		</Property>
		<!--		</xsl:variable>-->
		<!--		<xsl:for-each select="$property/child::*">
			<xsl:call-template name="GenerateBackedProperty"/>
		</xsl:for-each>-->
	</xsl:template>
	<xsl:template match="ao:Association/ao:AbsorbedObject" mode="WalkAbsorbedObjects">
		<xsl:param name="Model"/>
		<!--		<xsl:variable name="absorbedData">-->
		<xsl:apply-templates select="." mode="InterpretAbsorbed">
			<xsl:with-param name="Model" select="$Model"/>
		</xsl:apply-templates>
		<!--		</xsl:variable>-->
		<!--		<xsl:for-each select="$absorbedData/child::*">
			<xsl:variable name="dataTypeTemp">
				<DataType>
					<xsl:choose>
						<xsl:when test="contains(@multiplicity, 'Many')">
							<xsl:attribute name="dataTypeName">ICollection</xsl:attribute>
							<plx:PassTypeParam>
								<xsl:copy-of select="DataType/@*"/>
							</plx:PassTypeParam>
						</xsl:when>
						<xsl:otherwise>
							<xsl:copy-of select="DataType/@*"/>
						</xsl:otherwise>
					</xsl:choose>
				</DataType>
			</xsl:variable>
			<xsl:variable name="dataType" select="$dataTypeTemp/child::*"/>
			<plx:Field name="{$PrivateMemberPrefix}{@roleName}" visibility="Private">
				<xsl:copy-of select="$dataType/@*"/>
				<xsl:copy-of select="$dataType/child::*"/>
				<plx:Initialize>
					<plx:DefaultValueOf>
						<xsl:copy-of select="$dataType/@*"/>
						<xsl:copy-of select="$dataType/child::*"/>
					</plx:DefaultValueOf>
				</plx:Initialize>
			</plx:Field>
			<plx:Property name="{@roleName}" visibility="Public">
				<plx:Param type="RetVal" name="">
					<xsl:copy-of select="$dataType/@*"/>
					<xsl:copy-of select="$dataType/child::*"/>
				</plx:Param>
				<plx:Get>
					<plx:Return>
						<plx:CallInstance name="{$PrivateMemberPrefix}{@roleName}" type="Field">
							<plx:CallObject><plx:ThisKeyword/></plx:CallObject>
						</plx:CallInstance>
					</plx:Return>
				</plx:Get>
			</plx:Property>
		</xsl:for-each>-->
	</xsl:template>
</xsl:stylesheet>
