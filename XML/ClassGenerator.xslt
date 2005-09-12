<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:orm="http://Schemas.Neumont.edu/ORM/ORMCore"
	xmlns:ormRoot="http://Schemas.Neumont.edu/ORM/ORMRoot"
	xmlns:plx="http://Schemas.Neumont.edu/CodeGeneration/Plix"
	xmlns:ao="http://Schemas.Neumont.edu/ORM/SDK/ClassGenerator/AbsorbedObjects">
	<xsl:param name="CustomToolNamespace" select="'TestNamespace'"/>
	<xsl:param name="PrivateMemberPrefix" select="'my'"/>
	<xsl:param name="AssociationClassSuffix" select="'Association'"/>
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
												<xsl:value-of select="$oppositeMultiplicity='ZeroToOne' or $oppositeMultiplicity='ExactlyOne'"/>
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
							<xsl:variable name="roleName" select="@Name"/>
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

	<!-- Get the data type name and qualifier for the passed-in orm2 data type element.
	     Returns a DataType element with up to three attributes (dataTypeName, dataTypeQualifier, dataTypeIsSimpleArray)
		 to use in plix -->
	<xsl:template name="DataTypeToPlixValueType">
		<!-- Any element with standard Plix data type attributes -->
		<xsl:param name="DataType"/>
		<xsl:for-each select="$DataType">
			<!-- Here's how we map
			Char Char
			I1 SByte
			I2 Int16
			I4 Int32
			I8 Int64
			U1 Byte
			U2 UInt16
			U4 UInt32
			U8 UInt64
			R4 Single
			R8 Double
			-->
			<xsl:choose>
				<xsl:when test="@dataTypeQualifier='System'">
					<xsl:choose>
						<xsl:when test="@dataTypeName='Char'">
							<xsl:text>Char</xsl:text>
						</xsl:when>
						<xsl:when test="@dataTypeName='SByte'">
							<xsl:text>I1</xsl:text>
						</xsl:when>
						<xsl:when test="@dataTypeName='Int16'">
							<xsl:text>I2</xsl:text>
						</xsl:when>
						<xsl:when test="@dataTypeName='Int32'">
							<xsl:text>I4</xsl:text>
						</xsl:when>
						<xsl:when test="@dataTypeName='Int64'">
							<xsl:text>I8</xsl:text>
						</xsl:when>
						<xsl:when test="@dataTypeName='Byte'">
							<xsl:text>U1</xsl:text>
						</xsl:when>
						<xsl:when test="@dataTypeName='UInt16'">
							<xsl:text>U2</xsl:text>
						</xsl:when>
						<xsl:when test="@dataTypeName='UInt32'">
							<xsl:text>U4</xsl:text>
						</xsl:when>
						<xsl:when test="@dataTypeName='UInt64'">
							<xsl:text>U8</xsl:text>
						</xsl:when>
						<xsl:when test="@dataTypeName='Single'">
							<xsl:text>R4</xsl:text>
						</xsl:when>
						<xsl:when test="@dataTypeName='Double'">
							<xsl:text>R8</xsl:text>
						</xsl:when>
						<xsl:otherwise>
							<xsl:text>String</xsl:text>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text>String</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:for-each>
	</xsl:template>

	<!-- UNDONE: It would be nice to have all of this information in the AbsorbedObjects -->
	<!-- From an orm:Role in a Constraint, walks $AbsorbedObjects to get the parameter name and DataType -->
	<xsl:template name="GetParameterFromRole">
		<xsl:param name="Model"/>
		<!-- Pass a realRoleRef if you want the Parameter for any role that matches it to have @special set to true() -->
		<xsl:param name="realRoleRef"/>
		<xsl:variable name="ref" select="@ref"/>
		<!--object is an ao:AbsorbedObject or an ao:RelatedObject-->
		<xsl:variable name="object" select="$AbsorbedObjects/../ao:Object/child::*[@oppositeRoleRef=$ref]|$AbsorbedObjects/../ao:Association/ao:RelatedObject[@roleRef=$ref]|$AbsorbedObjects/../ao:Association/ao:AbsorbedObject[@thisRoleRef=$ref]"/>
		<xsl:variable name="propertyName">
			<xsl:choose>
				<xsl:when test="local-name($object/..)='Object'">
					<xsl:value-of select="$object/@oppositeRoleName"/>
				</xsl:when>
				<xsl:when test="local-name($object/..)='Association'">
					<xsl:value-of select="$object/@roleName"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="objectName">
			<xsl:choose>
				<xsl:when test="local-name($object/..)='Object'">
					<xsl:value-of select="$object/@oppositeObjectName"/>
				</xsl:when>
				<xsl:when test="local-name($object/..)='Association'">
					<xsl:choose>
						<xsl:when test="local-name($object)='RelatedObject'">
							<xsl:value-of select="$object/@className"/>
						</xsl:when>
						<xsl:when test="local-name($object)='AbsorbedObject'">
							<xsl:value-of select="$object/@name"/>
						</xsl:when>
					</xsl:choose>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>
		<Parameter special="{$ref=$realRoleRef}">
			<xsl:attribute name="name">
				<!-- Prefer propertyName, but fall back to objectName if we don't have it-->
				<xsl:choose>
					<xsl:when test="string-length($propertyName)">
						<xsl:value-of select="$propertyName"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$objectName"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:choose>
				<xsl:when test="local-name($object)='RelatedObject'">
					<!-- If we have an ao:RelatedObject, then objectName is the DataType -->
					<DataType dataTypeName="{$objectName}"/>
				</xsl:when>
				<xsl:otherwise>
					<!-- Otherwise, use the MapDataType template to get the DataType -->
					<xsl:variable name="rolePlayerId" select="$object/@ref"/>
					<xsl:variable name="rolePlayer" select="$Model/orm:Objects/child::*[@id=$rolePlayerId]"/>
					<xsl:choose>
						<xsl:when test="$rolePlayer/orm:ConceptualDataType">
							<xsl:variable name="dataTypeId"  select="$rolePlayer/orm:ConceptualDataType/@ref"/>
							<xsl:for-each select="$Model/orm:DataTypes/child::*[@id=$dataTypeId]">
								<xsl:call-template name="MapDataType"/>
							</xsl:for-each>
						</xsl:when>
						<xsl:otherwise>
							<xsl:variable name="transformedPropertyFragment">
								<xsl:apply-templates select="$object" mode="TransformAbsorbedObjects">
									<xsl:with-param name="Model" select="$Model"/>
								</xsl:apply-templates>
							</xsl:variable>
							<xsl:variable name="transformedProperty" select="msxsl:node-set($transformedPropertyFragment)/child::*"/>
							<xsl:attribute name="name">
								<!-- Make sure we have the right name by taking it from the transformedProperty -->
								<xsl:value-of select="$transformedProperty/@name"/>
							</xsl:attribute>
							<xsl:copy-of select="$transformedProperty/DataType"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:otherwise>
			</xsl:choose>
		</Parameter>
	</xsl:template>
	
	<xsl:template name="GenerateAbstractClass">
		<xsl:param name="Model"/>
		<xsl:param name="ModelContextName"/>
		<xsl:param name="className"/>
		<xsl:variable name="propertiesFragment">
			<xsl:apply-templates select="child::*" mode="TransformPropertyObjects">
				<xsl:with-param name="Model" select="$Model"/>
			</xsl:apply-templates>
		</xsl:variable>
		<xsl:variable name="properties" select="msxsl:node-set($propertiesFragment)/child::*"/>
		<plx:Class visibility="Public" abstract="true" partial="true" name="{$className}">
			<plx:ImplementsInterface dataTypeName="INotifyPropertyChanged"/>
			<plx:Function ctor="true" visibility="Protected"/>
			<plx:Event visibility="Public" abstract="true" name="PropertyChanged">
				<!-- PLIX_TODO: Plix currently doesn't seem to like InterfaceMember elements in Event elements -->
				<!--<plx:InterfaceMember member="PropertyChanged" dataTypeName="INotifyPropertyChanged"/>-->
				<plx:DelegateType dataTypeName="PropertyChangedEventHandler"/>
			</plx:Event>
			<xsl:variable name="contextPropertyFragment">
				<Property name="Context" readOnly="true">
					<DataType dataTypeName="{$ModelContextName}"/>
				</Property>
			</xsl:variable>
			<xsl:for-each select="msxsl:node-set($contextPropertyFragment)/child::*">
				<xsl:call-template name="GenerateAbstractProperty"/>
			</xsl:for-each>
			<xsl:for-each select="$properties">
				<xsl:call-template name="GenerateAbstractProperty"/>
			</xsl:for-each>
		</plx:Class>
	</xsl:template>
	<xsl:template name="GenerateAbstractProperty">
		<xsl:variable name="readOnly" select="@readOnly='true'"/>
		<plx:Property visibility="Public" abstract="true" name="{@name}" >
			<plx:Param type="RetVal" name="">
				<xsl:copy-of select="DataType/@*"/>
				<xsl:copy-of select="DataType/child::*"/>
			</plx:Param>
			<plx:Get/>
			<xsl:if test="not($readOnly)">
				<plx:Set/>
			</xsl:if>
		</plx:Property>
		<xsl:if test="not($readOnly)">
			<!-- PLIX_TODO: Plix currently seems to be ignoring the abstract attribute on Event elements... -->
			<plx:Event visibility="Public" abstract="true" name="{@name}Changing">
				<plx:DelegateType dataTypeName="EventHandler">
					<plx:PassTypeParam dataTypeName="PropertyChangingEventArgs">
						<plx:PassTypeParam>
							<xsl:copy-of select="DataType/@*"/>
							<xsl:copy-of select="DataType/child::*"/>
						</plx:PassTypeParam>
					</plx:PassTypeParam>
				</plx:DelegateType>
			</plx:Event>
			<!-- PLIX_TODO: Plix currently seems to be ignoring the abstract attribute on Event elements... -->
			<plx:Event visibility="Public" abstract="true" name="{@name}Changed">
				<plx:DelegateType dataTypeName="EventHandler">
					<plx:PassTypeParam dataTypeName="PropertyChangedEventArgs">
						<plx:PassTypeParam>
							<xsl:copy-of select="DataType/@*"/>
							<xsl:copy-of select="DataType/child::*"/>
						</plx:PassTypeParam>
					</plx:PassTypeParam>
				</plx:DelegateType>
			</plx:Event>
		</xsl:if>
	</xsl:template>
	
	<xsl:template name="GenerateMandatoryParameters">
		<xsl:param name="properties"/>
		<!--nonCustomOnly: set to true() to force creation of single values only parameters-->
		<xsl:param name="nonCustomOnly" select="false()"/>
		<!--nullPlaceHolders: set to true to generate a placeholder for custom types when
		nonCustomOnly = true() (used by GenerateDeserializationContextMethod)-->
		<xsl:param name="nullPlaceholders" select="false()"/>
		<xsl:variable name="localName" select="local-name()"/>
		<xsl:for-each select="$properties">
			<xsl:if test="(not($nonCustomOnly) or @customType='true') and (@mandatory='true' or @mandatory='relaxed' or $localName='Association')">
				<xsl:choose>
					<xsl:when test="$nullPlaceholders and @customType='true'">
						<plx:PassParam>
							<plx:NullObjectKeyword/>
						</plx:PassParam>
					</xsl:when>
					<xsl:otherwise>
						<plx:Param type="In" name="{@name}">
							<xsl:copy-of select="DataType/@*"/>
							<xsl:copy-of select="DataType/child::*"/>
						</plx:Param>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>
	
	<xsl:template name="GenerateGlobalSupportClasses">
			<xsl:variable name="PropertyChangeEventArgsClassBody">
				<plx:TypeParam name="TProperty"/>
				<plx:Field visibility="Private" readOnly="true" name="{$PrivateMemberPrefix}OldValue" dataTypeName="TProperty"/>
				<plx:Field visibility="Private" readOnly="true" name="{$PrivateMemberPrefix}NewValue" dataTypeName="TProperty"/>
				<plx:Function ctor="true" visibility="Public">
					<plx:Param type="In" name="oldValue" dataTypeName="TProperty"/>
					<plx:Param type="In" name="newValue" dataTypeName="TProperty"/>
					<plx:Operator type="Assign">
						<plx:Left>
							<plx:CallInstance name="{$PrivateMemberPrefix}OldValue" type="Field">
								<plx:CallObject>
									<plx:ThisKeyword/>
								</plx:CallObject>
							</plx:CallInstance>
						</plx:Left>
						<plx:Right>
							<plx:Value type="Parameter" data="oldValue"/>
						</plx:Right>
					</plx:Operator>
					<plx:Operator type="Assign">
						<plx:Left>
							<plx:CallInstance name="{$PrivateMemberPrefix}NewValue" type="Field">
								<plx:CallObject>
									<plx:ThisKeyword/>
								</plx:CallObject>
							</plx:CallInstance>
						</plx:Left>
						<plx:Right>
							<plx:Value type="Parameter" data="newValue"/>
						</plx:Right>
					</plx:Operator>
				</plx:Function>
				<plx:Property visibility="Public" name="OldValue">
					<plx:Param type="RetVal" name="" dataTypeName="TProperty"/>
					<plx:Get>
						<plx:Return>
							<plx:CallInstance name="{$PrivateMemberPrefix}OldValue" type="Field">
								<plx:CallObject>
									<plx:ThisKeyword/>
								</plx:CallObject>
							</plx:CallInstance>
						</plx:Return>
					</plx:Get>
				</plx:Property>
				<plx:Property visibility="Public" name="NewValue">
					<plx:Param type="RetVal" name="" dataTypeName="TProperty"/>
					<plx:Get>
						<plx:Return>
							<plx:CallInstance name="{$PrivateMemberPrefix}NewValue" type="Field">
								<plx:CallObject>
									<plx:ThisKeyword/>
								</plx:CallObject>
							</plx:CallInstance>
						</plx:Return>
					</plx:Get>
				</plx:Property>
			</xsl:variable>
			<plx:Class visibility="Public" sealed="true" name="PropertyChangingEventArgs">
				<plx:DerivesFromClass dataTypeName="CancelEventArgs"/>
				<xsl:copy-of select="$PropertyChangeEventArgsClassBody"/>
			</plx:Class>
			<plx:Class visibility="Public" sealed="true" name="PropertyChangedEventArgs">
				<plx:DerivesFromClass dataTypeName="EventArgs"/>
				<xsl:copy-of select="$PropertyChangeEventArgsClassBody"/>
			</plx:Class>
	</xsl:template>

	<xsl:template name="GenerateModelContextInterfaceMethods">
		<xsl:param name="Model"/>
		<xsl:call-template name="GenerateModelContextInterfaceLookupMethods">
			<xsl:with-param name="Model" select="$Model"/>
		</xsl:call-template>
	</xsl:template>
	<xsl:template name="GenerateModelContextInterfaceLookupMethods">
		<xsl:param name="Model"/>
		<xsl:param name="nonCustomOnly"/>
		<xsl:for-each select="orm:ExternalConstraints/orm:ExternalUniquenessConstraint">
			<xsl:variable name="firstRoleRef" select="orm:RoleSequence/orm:Role[1]/@ref" />
			<xsl:variable name="uniqueObjectName" select="$AbsorbedObjects/child::*[@oppositeRoleRef=$firstRoleRef]/../@name"/>
			<xsl:variable name="parametersFragment">
				<xsl:for-each select="orm:RoleSequence/orm:Role">
					<xsl:call-template name="GetParameterFromRole">
						<xsl:with-param name="Model" select="$Model"/>
					</xsl:call-template>
				</xsl:for-each>
			</xsl:variable>
			<xsl:variable name="parameters" select="msxsl:node-set($parametersFragment)/child::*"/>
			<plx:Function visibility="Public" name="Get{$uniqueObjectName}By{@Name}">
				<plx:Param type="RetVal" name="" dataTypeName="{$uniqueObjectName}"/>
				<xsl:for-each select="$parameters">
					<plx:Param type="In" name="{@name}">
						<xsl:copy-of select="DataType/@*"/>
						<xsl:copy-of select="DataType/child::*"/>
					</plx:Param>
				</xsl:for-each>
			</plx:Function>
		</xsl:for-each>
		<xsl:for-each select="$AbsorbedObjects/../ao:Association">
			<xsl:variable name="uniqueObjectName" select="concat(@name,$AssociationClassSuffix)"/>
			<xsl:for-each select="$Model/orm:Facts/orm:Fact[@id=current()/@id]/orm:InternalConstraints/orm:InternalUniquenessConstraint">
				<xsl:variable name="parametersFragment">
					<xsl:for-each select="orm:RoleSequence/orm:Role">
						<xsl:call-template name="GetParameterFromRole">
							<xsl:with-param name="Model" select="$Model"/>
						</xsl:call-template>
					</xsl:for-each>
				</xsl:variable>
				<xsl:variable name="parameters" select="msxsl:node-set($parametersFragment)/child::*"/>
				<plx:Function visibility="Public" name="Get{$uniqueObjectName}By{@Name}">
					<plx:Param type="RetVal" name="" dataTypeName="{$uniqueObjectName}"/>
					<xsl:for-each select="$parameters">
						<plx:Param type="In" name="{@name}">
							<xsl:copy-of select="DataType/@*"/>
							<xsl:copy-of select="DataType/child::*"/>
						</plx:Param>
					</xsl:for-each>
				</plx:Function>
			</xsl:for-each>
		</xsl:for-each>
	</xsl:template>
	<xsl:template name="GenerateModelContextInterfaceObjectMethods">
		<xsl:param name="Model"/>
		<xsl:param name="className"/>
		<xsl:param name="nonCustomOnly"/>
		<xsl:variable name="propertiesFragment">
			<xsl:apply-templates select="child::*" mode="TransformPropertyObjects">
				<xsl:with-param name="Model" select="$Model"/>
			</xsl:apply-templates>
		</xsl:variable>
		<xsl:variable name="properties" select="msxsl:node-set($propertiesFragment)/child::*"/>
		<xsl:call-template name="GenerateModelContextInterfaceCreateMethod">
			<xsl:with-param name="Model" select="$Model"/>
			<xsl:with-param name="className" select="$className"/>
			<xsl:with-param name="properties" select="$properties"/>
			<xsl:with-param name="nonCustomOnly" select="$nonCustomOnly"/>
		</xsl:call-template>
		<xsl:call-template name="GenerateModelContextInterfaceSimpleLookupMethods">
			<xsl:with-param name="Model" select="$Model"/>
			<xsl:with-param name="className" select="$className"/>
			<xsl:with-param name="properties" select="$properties"/>
		</xsl:call-template>
	</xsl:template>
	<xsl:template name="GenerateModelContextInterfaceCreateMethod">
		<xsl:param name="Model"/>
		<xsl:param name="className"/>
		<xsl:param name="properties"/>
		<xsl:param name="nonCustomOnly"/>
		<plx:Function visibility="Public" name="Create{$className}">
			<plx:Param type="RetVal" name="" dataTypeName="{$className}"/>
			<xsl:call-template name="GenerateMandatoryParameters">
				<xsl:with-param name="properties" select="$properties"/>
				<xsl:with-param name="nonCustomOnly" select="$nonCustomOnly"/>
			</xsl:call-template>
		</plx:Function>
	</xsl:template>
	<xsl:template name="GenerateModelContextInterfaceSimpleLookupMethods">
		<xsl:param name="Model"/>
		<xsl:param name="className"/>
		<xsl:param name="properties"/>
		<xsl:for-each select="$properties">
			<xsl:if test="@unique='true' and not(@customType='true')">
				<plx:Function name="Get{$className}By{@name}" visibility="Public">
					<plx:Param type="RetVal" name="" dataTypeName="{$className}"/>
					<plx:Param type="In" name="value">
						<xsl:copy-of select="DataType/@*"/>
						<xsl:copy-of select="DataType/child::*"/>
					</plx:Param>
				</plx:Function>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>

	<xsl:template match="ormRoot:ORM2">
		<xsl:text disable-output-escaping="yes"><![CDATA[<!--<ao>]]></xsl:text>
		<xsl:copy-of select="$AbsorbedObjects"/>
		<xsl:text disable-output-escaping="yes"><![CDATA[</ao>-->]]></xsl:text>
		<plx:Root>
			<plx:Using name="System"/>
			<plx:Using name="System.Collections.Generic"/>
			<plx:Using name="System.ComponentModel"/>
			<plx:Namespace name="{$CustomToolNamespace}">
			<xsl:call-template name="GenerateGlobalSupportClasses"/>
			<xsl:apply-templates mode="Main" select="orm:ORMModel"/>
			</plx:Namespace>
		</plx:Root>
	</xsl:template>
	<xsl:include href="ModelContext.xslt"/>
	<xsl:template match="orm:ORMModel" mode="Main">
		<xsl:variable name="ModelName" select="@Name"/>
		<xsl:variable name="ModelContextName" select="concat($ModelName,'Context')"/>
		<xsl:variable name="ModelDeserializationName" select="concat('Deserialization',$ModelContextName)"/>
		<plx:Namespace name="{$ModelName}">
			<xsl:apply-templates mode="ForGenerateAbstractClass" select="$AbsorbedObjects">
				<xsl:with-param name="Model" select="."/>
				<xsl:with-param name="ModelContextName" select="$ModelContextName"/>
			</xsl:apply-templates>
			<plx:Interface visibility="Public" name="I{$ModelContextName}">
				<plx:Function name="BeginDeserialization" visibility="Public">
					<plx:Param name="" type="RetVal" dataTypeName="I{$ModelDeserializationName}"/>
				</plx:Function>
				<xsl:call-template name="GenerateModelContextInterfaceMethods">
					<xsl:with-param name="Model" select="."/>
				</xsl:call-template>
				<xsl:apply-templates mode="ForGenerateModelContextInterfaceObjectMethods" select="$AbsorbedObjects">
					<xsl:with-param name="Model" select="."/>
					<xsl:with-param name="nonCustomOnly" select="false()"/>
				</xsl:apply-templates>
			</plx:Interface>
			<plx:Interface visibility="Public" name="I{$ModelDeserializationName}">
				<plx:ImplementsInterface dataTypeName="IDisposable"/>
				<xsl:apply-templates mode="ForGenerateModelContextInterfaceObjectMethods" select="$AbsorbedObjects">
					<xsl:with-param name="Model" select="."/>
					<xsl:with-param name="nonCustomOnly" select="true()"/>
				</xsl:apply-templates>
			</plx:Interface>
			<xsl:call-template name="GenerateImplementation">
				<xsl:with-param name="ModelContextName" select="$ModelContextName"/>
				<xsl:with-param name="ModelDeserializationName" select="$ModelDeserializationName"/>
			</xsl:call-template>
		</plx:Namespace>
	</xsl:template>

	<xsl:template match="ao:Object" mode="ForGenerateAbstractClass">
		<xsl:param name="Model"/>
		<xsl:param name="ModelContextName"/>
		<!-- TODO: Is this test necessary? Is it even possible to have an ao:Object that isn't an EntityType or an ObjectifiedType? -->
		<xsl:if test="@type='EntityType' or @type='ObjectifiedType'">
			<xsl:call-template name="GenerateAbstractClass">
				<xsl:with-param name="Model" select="$Model"/>
				<xsl:with-param name="ModelContextName" select="$ModelContextName"/>
				<xsl:with-param name="className" select="@name"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
	<xsl:template match="ao:Object" mode="ForGenerateModelContextInterfaceObjectMethods">
		<xsl:param name="Model"/>
		<xsl:param name="nonCustomOnly"/>
		<!-- TODO: Is this test necessary? Is it even possible to have an ao:Object that isn't an EntityType or an ObjectifiedType? -->
		<xsl:if test="@type='EntityType' or @type='ObjectifiedType'">
			<xsl:call-template name="GenerateModelContextInterfaceObjectMethods">
				<xsl:with-param name="Model" select="$Model"/>
				<xsl:with-param name="className" select="@name"/>
				<xsl:with-param name="nonCustomOnly" select="$nonCustomOnly"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
	
	<xsl:template match="ao:Association" mode="ForGenerateAbstractClass">
		<xsl:param name="Model"/>
		<xsl:param name="ModelContextName"/>
		<xsl:call-template name="GenerateAbstractClass">
			<xsl:with-param name="Model" select="$Model"/>
			<xsl:with-param name="ModelContextName" select="$ModelContextName"/>
			<xsl:with-param name="className" select="concat(@name,$AssociationClassSuffix)"/>
		</xsl:call-template>
	</xsl:template>
	<xsl:template match="ao:Association" mode="ForGenerateModelContextInterfaceObjectMethods">
		<xsl:param name="Model"/>
		<xsl:param name="nonCustomOnly"/>
		<xsl:call-template name="GenerateModelContextInterfaceObjectMethods">
			<xsl:with-param name="Model" select="$Model"/>
			<xsl:with-param name="className" select="concat(@name,$AssociationClassSuffix)"/>
			<xsl:with-param name="nonCustomOnly" select="$nonCustomOnly"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template match="ao:Object/ao:RelatedObject" mode="TransformPropertyObjects">
		<xsl:param name="Model"/>
		<!--The generated code is similar for both binary related objects and
				association objects. Build a Property element with a name attribute and
				DataType element for all both cases. The property and its backing field
				will be spit using the GenerateImplementationProperty template. -->
		<Property mandatory="{@mandatory}" unique="{@unique}" realRoleRef="{@oppositeRoleRef}" oppositeName="{@roleName}">
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
					<xsl:attribute name="customType">
						<xsl:value-of select="true()"/>
					</xsl:attribute>
					<xsl:choose>
						<xsl:when test="contains(@multiplicity,'Many')">
							<xsl:attribute name="readOnly">
								<xsl:value-of select="true()"/>
							</xsl:attribute>
							<DataType dataTypeName="ICollection">
								<plx:PassTypeParam dataTypeName="{@oppositeObjectName}"/>
							</DataType>
						</xsl:when>
						<xsl:otherwise>
							<xsl:attribute name="readOnly">
								<xsl:value-of select="false()"/>
							</xsl:attribute>
							<DataType dataTypeName="{@oppositeObjectName}"/>
						</xsl:otherwise>
					</xsl:choose>
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
						<xsl:choose>
							<!-- TODO: How could the arity ever be 1 on a relationship with an association object? -->
							<xsl:when test="@arity=1">
								<xsl:attribute name="readOnly">
									<xsl:value-of select="false()"/>
								</xsl:attribute>
								<xsl:attribute name="customType">
									<xsl:value-of select="false()"/>
								</xsl:attribute>
								<DataType dateTypeName="Nullable" dataTypeQualifier="System">
									<plx:PassTypeParam dataTypeName="Boolean" dataTypeQualifier="System"/>
								</DataType>
							</xsl:when>
							<xsl:otherwise>
								<xsl:attribute name="readOnly">
									<xsl:value-of select="true()"/>
								</xsl:attribute>
								<xsl:attribute name="customType">
									<xsl:value-of select="true()"/>
								</xsl:attribute>
								<DataType dataTypeName="ICollection">
									<plx:PassTypeParam dataTypeName="{$factName}{$AssociationClassSuffix}"/>
								</DataType>
							</xsl:otherwise>
						</xsl:choose>
				</xsl:otherwise>
			</xsl:choose>
		</Property>
	</xsl:template>
	<xsl:template match="ao:Association/ao:RelatedObject" mode="TransformPropertyObjects">
		<xsl:param name="Model"/>
		<Property mandatory="true" unique="{@unique}" realRoleRef="{@roleRef}">
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
			<xsl:attribute name="customType">
				<xsl:value-of select="true()"/>
			</xsl:attribute>
			<xsl:choose>
				<xsl:when test="contains(@multiplicity,'Many')">
					<xsl:attribute name="readOnly">
						<xsl:value-of select="true()"/>
					</xsl:attribute>
					<DataType dataTypeName="ICollection">
						<plx:PassTypeParam dataTypeName="{@className}"/>
					</DataType>
				</xsl:when>
				<xsl:otherwise>
					<xsl:attribute name="readOnly">
						<xsl:value-of select="false()"/>
					</xsl:attribute>
					<DataType dataTypeName="{@className}"/>
				</xsl:otherwise>
			</xsl:choose>
		</Property>
	</xsl:template>

	<xsl:template match="child::node()/ao:AbsorbedObject" mode="TransformPropertyObjects">
		<xsl:param name="Model"/>
		<xsl:apply-templates select="." mode="TransformAbsorbedObjects">
			<xsl:with-param name="Model" select="$Model"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="ao:AbsorbedObject" mode="TransformAbsorbedObjects">
		<xsl:param name="Model"/>
		<!--Absorbed objects can also absorb other objects. Use the deepest defined role name
				and the deepest fact to get names for the generated property. The data type
				for the property will always be the type of the deepest absorbed object. -->
		<xsl:choose>
			<xsl:when test="count(ao:AbsorbedObject)">
				<xsl:variable name="nestedFragment">
					<xsl:apply-templates select="ao:AbsorbedObject" mode="TransformAbsorbedObjects">
						<xsl:with-param name="Model" select="$Model"/>
					</xsl:apply-templates>
				</xsl:variable>
				<xsl:variable name="nested" select="msxsl:node-set($nestedFragment)/child::*"/>
				<Property multiplicity="{@multiplicity}" mandatory="{@mandatory}" unique="{@unique}" realRoleRef="{@thisRoleRef}" customType="{$nested/@customType}">
					<xsl:attribute name="name">
						<xsl:choose>
							<xsl:when test="string-length(@oppositeRoleName)">
								<xsl:value-of select="@oppositeRoleName"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="@name"/>
								<xsl:value-of select="$nested/@name"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:copy-of select="$nested/DataType"/>
				</Property>
			</xsl:when>
			<xsl:otherwise>
				<Property multiplicity="{@multiplicity}" mandatory="{@mandatory}" unique="{@unique}">
					<xsl:attribute name="realRoleRef">
						<xsl:choose>
							<xsl:when test="string-length(@oppositeRoleRef)">
								<xsl:value-of select="@oppositeRoleRef"/>
							</xsl:when>
							<xsl:when test="string-length(@thisRoleRef)">
								<xsl:value-of select="@thisRoleRef"/>
							</xsl:when>
						</xsl:choose>
					</xsl:attribute>
					<xsl:attribute name="name">
						<xsl:choose>
							<xsl:when test="string-length(@oppositeRoleName)">
								<xsl:value-of select="@oppositeRoleName"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="@name"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<!-- TODO: Is it even possible for it to NOT be a ValueType? -->
					<xsl:if test="@type='ValueType'">
						<xsl:attribute name="readOnly">
							<xsl:value-of select="false()"/>
						</xsl:attribute>
						<xsl:attribute name="customType">
							<xsl:value-of select="false()"/>
						</xsl:attribute>
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
	
</xsl:stylesheet>
