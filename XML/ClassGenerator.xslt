<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:orm="http://schemas.neumont.edu/ORM/ORMCore"
	xmlns:ormRoot="http://schemas.neumont.edu/ORM/ORMRoot"
	xmlns:plx="http://schemas.neumont.edu/CodeGeneration/PLiX"
	xmlns:ao="http://schemas.neumont.edu/ORM/SDK/ClassGenerator/AbsorbedObjects">

	<!-- Indenting is useful for debugging the transform, but a waste of memory at generation time. For this
		 to work correctly, you also need to eliminate the copy-of select="$AbsorbedObjects" code in the
		 match="ormRoot:ORM2" template-->
	<!--<xsl:output indent="yes"/>-->
	<xsl:param name="CustomToolNamespace" select="'TestNamespace'"/>
	<xsl:param name="PrivateMemberPrefix" select="'my'"/>
	<xsl:param name="ModelContextInterfaceImplementationVisibility" select="'private'"/>
	<xsl:param name="GenerateSerializationClasses" select="false()"/>
	<xsl:param name="GenerateCodeAnalysisAttributes" select="true()"/>
	<xsl:param name="RaiseEventsAsynchronously" select="true()"/>
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
								<!-- UNDONE: Also, _IsMandatory should be in the derived namespace -->
								<xsl:variable name="MandatoryRoles" select="$Roles[@functionalRole and @_IsMandatory='true']"/>
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
											<xsl:variable name="arity" select="count($parentFact/orm:FactRoles/orm:Role)"/>
											<xsl:attribute name="arity">
												<xsl:value-of select="$arity"/>
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
											<xsl:variable name="oppositeRoleMultiplicity" select="../orm:Role[@id!=$roleId]/@_Multiplicity"/>
											<xsl:attribute name="mandatory">
												<xsl:choose>
													<!-- both are mandatory so set the dominant one to relaxed-->
													<!-- (1)one to one binary set the non dominant role to relaxed-->
													<xsl:when test="@_Multiplicity='ExactlyOne' and count($DominantFunctionalRoles[@ref=$roleId])=0 and $oppositeRoleMultiplicity='ExactlyOne' ">
														<xsl:value-of select="'relaxed'"/>
													</xsl:when>
													<!-- (2)many to many binary set both to relaxed-->
													<xsl:when test="@_Multiplicity='OneToMany' and $oppositeRoleMultiplicity='OneToMany'">
														<xsl:value-of select="'relaxed'"/>
													</xsl:when>
													<!-- (3)Opposite role is funtional w/ both mandatory set the non dominant role to relaxed-->
													<xsl:when test="@_Multiplicity='ExactlyOne' and count($DominantFunctionalRoles[@ref=$roleId])=0 and $oppositeRoleMultiplicity='OneToMany'">
														<xsl:value-of select="'relaxed'"/>
													</xsl:when>
													<!-- (4)Set mandatory to False or True -->
													<xsl:otherwise>
														<xsl:value-of select="@_IsMandatory"/>
													</xsl:otherwise>
												</xsl:choose>
											</xsl:attribute>
											<xsl:variable name="oppositeMultiplicity" select="@_Multiplicity"/>
											<xsl:attribute name="unique">
												<xsl:value-of select="$oppositeMultiplicity='ZeroToOne' or $oppositeMultiplicity='ExactlyOne' or $arity&gt;2"/>
											</xsl:attribute>
											<xsl:if test="1=count($oppositeRoles)">
												<xsl:for-each select="$oppositeRoles">
													<xsl:variable name="oppositeRoleId" select="@id"/>
													<xsl:variable name="oppositeObjectId" select="orm:RolePlayer/@ref"/>
													<xsl:variable name="oppositeObject" select="$RawObjects[@id=$oppositeObjectId]"/>
													<xsl:attribute name="multiplicity">
														<xsl:value-of select="@_Multiplicity"/>
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
										<ao:RelatedObject type="{@type}" objectRef="{@id}" roleRef="{$roleId}" roleName="{$roleName}" className="{@name}" unique="false">
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
													<xsl:variable name="multiplicity" select="@multiplicity"/>
													<xsl:for-each select="..">
														<xsl:copy>
															<xsl:copy-of select="@*"/>
															<xsl:copy-of select="ao:AbsorbedObject"/>
															<xsl:for-each select="$oppositeObject">
																<ao:AbsorbedObject type="{@type}" ref="{@id}" name="{@name}" unique="{@unique}" multiplicity="{$multiplicity}" thisRoleName="{$roleName}" thisRoleRef="{$roleId}" oppositeRoleRef="{ao:RelatedObject/@roleRef}" oppositeRoleName="{ao:RelatedObject/@roleName}" mandatory="{$isMandatory}">
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
								<xsl:variable name="multiplicity" select="@multiplicity"/>
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
													<ao:AbsorbedObject type="{@type}" ref="{@id}" name="{@name}" unique="{$oppositeRelatedObjectMultiplicity = 'ZeroToOne' or oppositeRelatedObjectMultiplicity = 'ExactlyOne'}" multiplicity="{$multiplicity}" thisRoleName="{$roleName}" thisRoleRef="{$roleId}" oppositeRoleRef="{$oppositeRelatedObject/@roleRef}" oppositeRoleName="{$oppositeRelatedObject/@roleName}" mandatory="{$isMandatory}">
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
											<xsl:variable name="multiplicity" select="@multiplicity"/>
											<xsl:variable name="isMandatory" select="@mandatory"/>
											<xsl:choose>
												<xsl:when test="$oppositeObject/@type='ValueType'">
													<xsl:variable name="roleId" select="@roleRef"/>
													<xsl:for-each select="$oppositeObject">
														<ao:AbsorbedObject type="{@type}" ref="{@id}" name="{@name}" thisRoleRef="{$roleId}" oppositeRoleRef="{$oppositeRoleRef}" multiplicity="{$multiplicity}" mandatory="{$isMandatory}">
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
				<DataType dataTypeName=".string"/>
			</xsl:when>
			<xsl:when test="$tagName='SignedIntegerNumericDataType' or $tagName='AutoCounterNumericDataType'">
				<DataType dataTypeName=".i4"/>
			</xsl:when>
			<xsl:when test="$tagName='UnsignedIntegerNumericDataType'">
				<DataType dataTypeName=".u4"/>
			</xsl:when>
			<xsl:when test="$tagName='FloatingPointNumericDataType'">
				<DataType dataTypeName=".r8"/>
			</xsl:when>
			<xsl:when test="$tagName='DecimalNumericDataType' or $tagName='MoneyNumericDataType'">
				<DataType dataTypeName=".decimal"/>
			</xsl:when>
			<xsl:when test="$tagName='FixedLengthRawDataDataType' or $tagName='VariableLengthRawDataDataType' or $tagName='LargeLengthRawDataDataType' or $tagName='PictureRawDataDataType' or $tagName='OleObjectRawDataDataType'">
				<DataType dataTypeName=".u1" dataTypeIsSimpleArray="true"/>
			</xsl:when>
			<xsl:when test="$tagName='AutoTimestampTemporalDataType' or $tagName='TimeTemporalDataType' or $tagName='DateTemporalDataType' or $tagName='DateAndTimeTemporalDataType'">
				<DataType dataTypeName=".date"/>
			</xsl:when>
			<xsl:when test="$tagName='TrueOrFalseLogicalDataType' or $tagName='YesOrNoLogicalDataType'">
				<DataType dataTypeName=".boolean"/>
			</xsl:when>
			<xsl:when test="$tagName='RowIdOtherDataType' or $tagName='ObjectIdOtherDataType'">
				<DataType dataTypeName=".u8"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:message terminate="yes">Could not map DataType.</xsl:message>
			</xsl:otherwise>
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
				<xsl:when test="substring(@dataTypeName,1,1)='.'">
					<xsl:value-of select="substring(@dataTypeName,2)"/>
				</xsl:when>
				<xsl:when test="@dataTypeQualifier='System'">
					<xsl:choose>
						<xsl:when test="@dataTypeName='Char'">
							<xsl:text>char</xsl:text>
						</xsl:when>
						<xsl:when test="@dataTypeName='SByte'">
							<xsl:text>i1</xsl:text>
						</xsl:when>
						<xsl:when test="@dataTypeName='Int16'">
							<xsl:text>i2</xsl:text>
						</xsl:when>
						<xsl:when test="@dataTypeName='Int32'">
							<xsl:text>i4</xsl:text>
						</xsl:when>
						<xsl:when test="@dataTypeName='Int64'">
							<xsl:text>i8</xsl:text>
						</xsl:when>
						<xsl:when test="@dataTypeName='Byte'">
							<xsl:text>u1</xsl:text>
						</xsl:when>
						<xsl:when test="@dataTypeName='UInt16'">
							<xsl:text>u2</xsl:text>
						</xsl:when>
						<xsl:when test="@dataTypeName='UInt32'">
							<xsl:text>u4</xsl:text>
						</xsl:when>
						<xsl:when test="@dataTypeName='UInt64'">
							<xsl:text>u8</xsl:text>
						</xsl:when>
						<xsl:when test="@dataTypeName='Single'">
							<xsl:text>r4</xsl:text>
						</xsl:when>
						<xsl:when test="@dataTypeName='Double'">
							<xsl:text>r8</xsl:text>
						</xsl:when>
						<xsl:otherwise>
							<xsl:text>string</xsl:text>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text>string</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:for-each>
	</xsl:template>

	<!-- Helper template to get a PLiX Value element -->
	<xsl:template name="GetHexValue">
		<xsl:param name="Position" select="position()"/>
		<plx:value type="hex4">
			<xsl:attribute name="data">
				<xsl:value-of select="substring('1248',(($Position - 1) mod 4) + 1, 1)"/>
				<xsl:if test="$Position &gt; 4">
					<xsl:value-of select="substring('000000000000000000000000000000000000000000000000000000000000000',1, floor(($Position - 1) div 4))"/>
				</xsl:if>
			</xsl:attribute>
		</plx:value>
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

	<xsl:template name="GetUniquePropertyDataType">
		<xsl:if test="not(@unique='true')">
			<xsl:message terminate="yes">
				<xsl:text>This template should not be called for Property elements where the unique attribute is not 'true'.</xsl:text>
			</xsl:message>
		</xsl:if>
		<xsl:choose>
			<xsl:when test="@collection='true'">
				<xsl:copy-of select="DataType/plx:passTypeParam/@*"/>
				<xsl:copy-of select="DataType/plx:passTypeParam/child::*"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:copy-of select="DataType/@*"/>
				<xsl:copy-of select="DataType/child::*"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template name="GenerateErrorEnums">
		<xsl:param name="AbsorbedObjects"/>
		<xsl:for-each select="$AbsorbedObjects/../ao:Object">
			<xsl:if test="count(child::*[@mandatory='relaxed']) &gt; 0">
				<xsl:variable name="relaxedObjectsFragment">
					<xsl:for-each select="child::ao:RelatedObject[@mandatory='relaxed']">
						<elem oppositeRoleName="{@oppositeRoleName}"/>
					</xsl:for-each>
				</xsl:variable>
				<xsl:variable name="relaxedObjects" select="msxsl:node-set($relaxedObjectsFragment)/child::*"/>
				<plx:Enum visibility="public" name="{@name}Errors">
					<plx:Attribute dataTypeName="Flags"/>
					<plx:EnumItem name="None">
						<plx:initialize>
							<plx:value data="0" type="i4"/>
						</plx:initialize>
					</plx:EnumItem>
					<xsl:for-each select="$relaxedObjects">
						<plx:EnumItem name="{@oppositeRoleName}Required">
							<plx:initialize>
								<xsl:call-template name="GetHexValue"/>
							</plx:initialize>
						</plx:EnumItem>
					</xsl:for-each>
				</plx:Enum>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>
	<xsl:template name="GenerateErrorGetter">
		<xsl:param name="properties"/>

		<xsl:if test="count(child::*[@mandatory='relaxed']) &gt; 0">
			<xsl:variable name="returnDataType" select="concat(@name,'Errors')"/>
			<plx:property visibility="public" name="ErrorState">
				<plx:returns dataTypeName="{$returnDataType}"/>
				<plx:get>
					<plx:local name="retVal" dataTypeName="{$returnDataType}">
						<plx:initialize>
							<plx:callStatic type="field" dataTypeName="{$returnDataType}" name="None"/>
						</plx:initialize>
					</plx:local>
					<xsl:for-each select="$properties">
						<xsl:choose>
							<xsl:when test="@mandatory = 'relaxed'">
								<plx:branch>
									<plx:condition>
										<plx:binaryOperator type="equality">
											<plx:left>
												<plx:callThis name="{@name}" type="property"/>
											</plx:left>
											<plx:right>
												<plx:nullKeyword/>
											</plx:right>
										</plx:binaryOperator>
									</plx:condition>
									<plx:body>
										<plx:assign>
											<plx:left>
												<plx:nameRef name="retVal"/>
											</plx:left>
											<plx:right>
												<plx:binaryOperator type="bitwiseOr">
													<plx:left>
														<plx:nameRef name="retVal"/>
													</plx:left>
													<plx:right>
														<plx:callStatic dataTypeName="{$returnDataType}" name="{@name}Required" type="field"/>
													</plx:right>
												</plx:binaryOperator>
											</plx:right>
										</plx:assign>
									</plx:body>
								</plx:branch>
							</xsl:when>
						</xsl:choose>
					</xsl:for-each>
					<plx:return>
						<plx:nameRef name="retVal"/>
					</plx:return>
				</plx:get>
			</plx:property>
		</xsl:if>
	</xsl:template>

	<xsl:template name="GenerateCLSCompliantAttributeIfNecessary">
		<xsl:variable name="dataTypeFragment">
			<xsl:choose>
				<xsl:when test="string-length(@dataTypeName)">
					<xsl:copy-of select="."/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:copy-of select="DataType"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="dataType" select="msxsl:node-set($dataTypeFragment)/child::*"/>
		<xsl:choose>
			<xsl:when test="starts-with($dataType/@dataTypeName,'.u')">
				<plx:attribute dataTypeName="CLSCompliantAttribute" dataTypeQualifier="System">
					<plx:passParam>
						<plx:falseKeyword/>
					</plx:passParam>
				</plx:attribute>
			</xsl:when>
			<xsl:otherwise>
				<xsl:for-each select="$dataType/child::node()">
					<xsl:call-template name="GenerateCLSCompliantAttributeIfNecessary"/>
				</xsl:for-each>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="GenerateSuppressMessageAttribute">
		<xsl:param name="category"/>
		<xsl:param name="checkId"/>
		<xsl:param name="justification"/>
		<xsl:param name="messageId"/>
		<xsl:param name="scope"/>
		<xsl:param name="target"/>
		<xsl:if test="$GenerateCodeAnalysisAttributes">
			<plx:attribute dataTypeName="SuppressMessageAttribute" dataTypeQualifier="System.Diagnostics.CodeAnalysis">
				<plx:passParam>
					<plx:string>
						<xsl:value-of select="$category"/>
					</plx:string>
				</plx:passParam>
				<plx:passParam>
					<plx:string>
						<xsl:value-of select="$checkId"/>
					</plx:string>
				</plx:passParam>
				<xsl:if test="$justification">
					<plx:passParam>
						<plx:binaryOperator type="assignNamed">
							<plx:left>
								<plx:nameRef type="namedParameter" name="Justification"/>
							</plx:left>
							<plx:right>
								<plx:string>
									<xsl:value-of select="$justification"/>
								</plx:string>
							</plx:right>
						</plx:binaryOperator>
					</plx:passParam>
				</xsl:if>
				<xsl:if test="$messageId">
					<plx:passParam>
						<plx:binaryOperator type="assignNamed">
							<plx:left>
								<plx:nameRef type="namedParameter" name="MessageId"/>
							</plx:left>
							<plx:right>
								<plx:string>
									<xsl:value-of select="$messageId"/>
								</plx:string>
							</plx:right>
						</plx:binaryOperator>
					</plx:passParam>
				</xsl:if>
				<xsl:if test="$scope">
					<plx:passParam>
						<plx:binaryOperator type="assignNamed">
							<plx:left>
								<plx:nameRef type="namedParameter" name="Scope"/>
							</plx:left>
							<plx:right>
								<plx:string>
									<xsl:value-of select="$scope"/>
								</plx:string>
							</plx:right>
						</plx:binaryOperator>
					</plx:passParam>
				</xsl:if>
				<xsl:if test="$target">
					<plx:passParam>
						<plx:binaryOperator type="assignNamed">
							<plx:left>
								<plx:nameRef type="namedParameter" name="Target"/>
							</plx:left>
							<plx:right>
								<plx:string>
									<xsl:value-of select="$target"/>
								</plx:string>
							</plx:right>
						</plx:binaryOperator>
					</plx:passParam>
				</xsl:if>
			</plx:attribute>
		</xsl:if>
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
		<xsl:variable name="events" select="$properties[not(@readOnly='true')]"/>
		<plx:class visibility="public" modifier="abstract" partial="true" name="{$className}">
			<plx:leadingInfo>
				<plx:pragma type="region" data="{$className}"/>
			</plx:leadingInfo>
			<plx:trailingInfo>
				<plx:pragma type="closeRegion" data="{$className}"/>
			</plx:trailingInfo>
			<plx:implementsInterface dataTypeName="INotifyPropertyChanged"/>
			<plx:function name=".construct" visibility="protected"/>
			<plx:field visibility="private" readOnly="true" name="Events" dataTypeIsSimpleArray="true" dataTypeName="Delegate" dataTypeQualifier="System">
				<plx:initialize>
					<plx:callNew dataTypeIsSimpleArray="true" dataTypeName="Delegate" dataTypeQualifier="System">
						<plx:passParam>
							<plx:value type="i4" data="{count($events)+1}"/>
						</plx:passParam>
					</plx:callNew>
				</plx:initialize>
			</plx:field>
			<xsl:call-template name="GenerateINotifyPropertyChangedImplementation"/>
			<xsl:variable name="contextPropertyFragment">
				<Property name="Context" readOnly="true">
					<DataType dataTypeName="{$ModelContextName}"/>
				</Property>
			</xsl:variable>
			<xsl:for-each select="msxsl:node-set($contextPropertyFragment)/child::*">
				<xsl:call-template name="GenerateAbstractProperty"/>
			</xsl:for-each>
			<xsl:for-each select="$properties">
				<xsl:call-template name="GenerateAbstractProperty">
					<xsl:with-param name="className" select="$className"/>
				</xsl:call-template>
			</xsl:for-each>
			<xsl:for-each select="$events">
				<xsl:call-template name="GeneratePropertyChangeEvents">
					<xsl:with-param name="eventIndex" select="position()"/>
				</xsl:call-template>
			</xsl:for-each>
			<xsl:call-template name="GenerateToString">
				<xsl:with-param name="className" select="$className"/>
				<xsl:with-param name="properties" select="$properties"/>
			</xsl:call-template>
			<xsl:call-template name="GenerateErrorGetter">
				<xsl:with-param name="properties" select="$properties"/>
			</xsl:call-template>
		</plx:class>
	</xsl:template>
	<xsl:template name="GenerateAbstractProperty">
		<xsl:variable name="readOnly" select="@readOnly='true'"/>
		<plx:property visibility="public" modifier="abstract" name="{@name}" >
			<xsl:call-template name="GenerateCLSCompliantAttributeIfNecessary"/>
			<plx:returns>
				<xsl:copy-of select="DataType/@*"/>
				<xsl:copy-of select="DataType/child::*"/>
			</plx:returns>
			<plx:get/>
			<xsl:if test="not($readOnly)">
				<plx:set/>
			</xsl:if>
		</plx:property>
	</xsl:template>
	<xsl:template name="GeneratePropertyChangeEvents">
		<xsl:param name="eventIndex"/>
		<xsl:call-template name="GeneratePropertyChangeEvent">
			<xsl:with-param name="changeType" select="'Changing'"/>
			<xsl:with-param name="eventIndex" select="$eventIndex"/>
		</xsl:call-template>
		<xsl:call-template name="GeneratePropertyChangeEventRaiseMethod">
			<xsl:with-param name="changeType" select="'Changing'"/>
			<xsl:with-param name="eventIndex" select="$eventIndex"/>
		</xsl:call-template>
		<xsl:call-template name="GeneratePropertyChangeEvent">
			<xsl:with-param name="changeType" select="'Changed'"/>
			<xsl:with-param name="eventIndex" select="$eventIndex"/>
		</xsl:call-template>
		<xsl:call-template name="GeneratePropertyChangeEventRaiseMethod">
			<xsl:with-param name="changeType" select="'Changed'"/>
			<xsl:with-param name="eventIndex" select="$eventIndex"/>
		</xsl:call-template>
	</xsl:template>
	<xsl:template name="GeneratePropertyChangeEvent">
		<xsl:param name="changeType"/>
		<xsl:param name="eventIndex"/>
		<xsl:param name="propertyChangedEvent" select="false()"/>
		<plx:event visibility="public" name="{@name}{$changeType}">
			<xsl:call-template name="GenerateCLSCompliantAttributeIfNecessary"/>
			<xsl:choose>
				<xsl:when test="$propertyChangedEvent">
					<xsl:attribute name="visibility">private</xsl:attribute>
					<xsl:attribute name="name">PropertyChanged</xsl:attribute>
					<xsl:call-template name="GenerateSuppressMessageAttribute">
						<xsl:with-param name="category" select="'Microsoft.Design'"/>
						<xsl:with-param name="checkId" select="'CA1033'"/>
					</xsl:call-template>
					<plx:interfaceMember memberName="PropertyChanged" dataTypeName="INotifyPropertyChanged"/>
					<plx:explicitDelegateType dataTypeName="PropertyChangedEventHandler"/>
				</xsl:when>
				<xsl:otherwise>
					<plx:explicitDelegateType dataTypeName="EventHandler"/>
					<plx:passTypeParam  dataTypeName="Property{$changeType}EventArgs">
						<plx:passTypeParam>
							<xsl:copy-of select="DataType/@*"/>
							<xsl:copy-of select="DataType/child::*"/>
						</plx:passTypeParam>
					</plx:passTypeParam>
				</xsl:otherwise>
			</xsl:choose>
			<plx:onAdd>
				<xsl:call-template name="GetPropertyChangeEventOnAddRemoveCode">
					<xsl:with-param name="eventIndex" select="$eventIndex"/>
					<xsl:with-param name="methodName" select="'Combine'"/>
				</xsl:call-template>
			</plx:onAdd>
			<plx:onRemove>
				<xsl:call-template name="GetPropertyChangeEventOnAddRemoveCode">
					<xsl:with-param name="eventIndex" select="$eventIndex"/>
					<xsl:with-param name="methodName" select="'Remove'"/>
				</xsl:call-template>
			</plx:onRemove>
		</plx:event>
	</xsl:template>
	<xsl:template name="GetPropertyChangeEventOnAddRemoveCode">
		<xsl:param name="eventIndex"/>
		<xsl:param name="methodName"/>
		<plx:assign>
			<plx:left>
				<plx:callInstance type="arrayIndexer" name=".implied">
					<plx:callObject>
						<plx:callThis accessor="this" type="field" name="Events"/>
					</plx:callObject>
					<plx:passParam>
						<plx:value type="i4" data="{$eventIndex}"/>
					</plx:passParam>
				</plx:callInstance>
			</plx:left>
			<plx:right>
				<plx:callStatic type="methodCall" name="{$methodName}" dataTypeName="Delegate" dataTypeQualifier="System">
					<plx:passParam>
						<plx:callInstance type="arrayIndexer" name=".implied">
							<plx:callObject>
								<plx:callThis accessor="this" type="field" name="Events"/>
							</plx:callObject>
							<plx:passParam>
								<plx:value type="i4" data="{$eventIndex}"/>
							</plx:passParam>
						</plx:callInstance>
					</plx:passParam>
					<plx:passParam>
						<plx:valueKeyword/>
					</plx:passParam>
				</plx:callStatic>
			</plx:right>
		</plx:assign>
	</xsl:template>
	<xsl:template name="GeneratePropertyChangeEventRaiseMethod">
		<xsl:param name="changeType"/>
		<xsl:param name="eventIndex"/>
		<xsl:variable name="changing" select="$changeType='Changing'"/>
		<xsl:variable name="changed" select="$changeType='Changed'"/>
		<plx:function visibility="protected" name="Raise{@name}{$changeType}Event">
			<xsl:call-template name="GenerateCLSCompliantAttributeIfNecessary"/>
			<xsl:call-template name="GenerateSuppressMessageAttribute">
				<xsl:with-param name="category" select="'Microsoft.Design'"/>
				<xsl:with-param name="checkId" select="'CA1030'"/>
			</xsl:call-template>
			<xsl:choose>
				<xsl:when test="$changing">
					<plx:param name="newValue">
						<xsl:copy-of select="DataType/@*"/>
						<xsl:copy-of select="DataType/child::*"/>
					</plx:param>
					<plx:returns dataTypeName=".boolean"/>
				</xsl:when>
				<xsl:when test="$changed">
					<plx:param name="oldValue">
						<xsl:copy-of select="DataType/@*"/>
						<xsl:copy-of select="DataType/child::*"/>
					</plx:param>
				</xsl:when>
			</xsl:choose>
			<plx:local name="eventHandler" dataTypeName="EventHandler">
				<plx:passTypeParam dataTypeName="Property{$changeType}EventArgs">
					<plx:passTypeParam>
						<xsl:copy-of select="DataType/@*"/>
						<xsl:copy-of select="DataType/child::*"/>
					</plx:passTypeParam>
				</plx:passTypeParam>
				<plx:initialize>
					<plx:cast type="testCast" dataTypeName="EventHandler">
						<plx:passTypeParam dataTypeName="Property{$changeType}EventArgs">
							<plx:passTypeParam>
								<xsl:copy-of select="DataType/@*"/>
								<xsl:copy-of select="DataType/child::*"/>
							</plx:passTypeParam>
						</plx:passTypeParam>
						<plx:callInstance type="arrayIndexer" name=".implied">
							<plx:callObject>
								<plx:callThis name="Events" type="field"/>
							</plx:callObject>
							<plx:passParam>
								<plx:value type="i4" data="{$eventIndex}"/>
							</plx:passParam>
						</plx:callInstance>
					</plx:cast>
				</plx:initialize>
			</plx:local>
			<plx:branch>
				<plx:condition>
					<plx:binaryOperator type="identityInequality">
						<plx:left>
							<plx:nameRef name="eventHandler"/>
						</plx:left>
						<plx:right>
							<plx:nullKeyword/>
						</plx:right>
					</plx:binaryOperator>
				</plx:condition>
				<plx:body>
					<xsl:variable name="createNewEventArgs">
						<xsl:variable name="currentValue">
							<plx:callThis type="property"  name="{@name}"/>
						</xsl:variable>
						<plx:callNew dataTypeName="Property{$changeType}EventArgs">
							<plx:passTypeParam>
								<xsl:copy-of select="DataType/@*"/>
								<xsl:copy-of select="DataType/child::*"/>
							</plx:passTypeParam>
							<plx:passParam>
								<xsl:choose>
									<xsl:when test="$changing">
										<xsl:copy-of select="$currentValue"/>
									</xsl:when>
									<xsl:when test="$changed">
										<plx:nameRef name="oldValue"/>
									</xsl:when>
								</xsl:choose>
							</plx:passParam>
							<plx:passParam>
								<xsl:choose>
									<xsl:when test="$changing">
										<plx:nameRef name="newValue"/>
									</xsl:when>
									<xsl:when test="$changed">
										<xsl:copy-of select="$currentValue"/>
									</xsl:when>
								</xsl:choose>
							</plx:passParam>
						</plx:callNew>
					</xsl:variable>
					<xsl:if test="$changing">
						<plx:local name="eventArgs" dataTypeName="PropertyChangingEventArgs">
							<plx:passTypeParam>
								<xsl:copy-of select="DataType/@*"/>
								<xsl:copy-of select="DataType/child::*"/>
							</plx:passTypeParam>
							<plx:initialize>
								<xsl:copy-of select="$createNewEventArgs"/>
							</plx:initialize>
						</plx:local>
					</xsl:if>
					<xsl:variable name="commonCallCode">
						<plx:callObject>
							<plx:nameRef type="local" name="eventHandler"/>
						</plx:callObject>
						<plx:passParam>
							<plx:thisKeyword/>
						</plx:passParam>
						<plx:passParam>
							<xsl:choose>
								<xsl:when test="$changing">
									<plx:nameRef type="local" name="eventArgs"/>
								</xsl:when>
								<xsl:when test="$changed">
									<xsl:copy-of select="$createNewEventArgs"/>
								</xsl:when>
							</xsl:choose>
						</plx:passParam>
					</xsl:variable>
					<xsl:choose>
						<xsl:when test="$changing or not($RaiseEventsAsynchronously)">
							<plx:callInstance name=".implied" type="delegateCall">
								<xsl:copy-of select="$commonCallCode"/>
							</plx:callInstance>
						</xsl:when>
						<xsl:when test="$changed and $RaiseEventsAsynchronously">
							<plx:callInstance type="methodCall" name="BeginInvoke">
								<xsl:copy-of select="$commonCallCode"/>
								<plx:passParam>
									<plx:callNew type="newDelegate" dataTypeName="AsyncCallback" dataTypeQualifier="System">
										<plx:passParam>
											<plx:callInstance type="methodReference" name="EndInvoke">
												<plx:callObject>
													<plx:nameRef type="local" name="eventHandler"/>
												</plx:callObject>
											</plx:callInstance>
										</plx:passParam>
									</plx:callNew>
								</plx:passParam>
								<plx:passParam>
									<plx:nullKeyword/>
								</plx:passParam>
							</plx:callInstance>
						</xsl:when>
					</xsl:choose>
					<xsl:choose>
						<xsl:when test="$changing">
							<plx:return>
								<plx:unaryOperator type="booleanNot">
									<plx:callInstance name="Cancel" type="property">
										<plx:callObject>
											<plx:nameRef name="eventArgs"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:unaryOperator>
							</plx:return>
						</xsl:when>
						<xsl:when test="$changed">
							<plx:callThis name="RaisePropertyChangedEvent">
								<plx:passParam>
									<plx:string>
										<xsl:value-of select="@name"/>
									</plx:string>
								</plx:passParam>
							</plx:callThis>
						</xsl:when>
					</xsl:choose>
				</plx:body>
			</plx:branch>
			<xsl:if test="$changing">
				<plx:return>
					<plx:trueKeyword/>
				</plx:return>
			</xsl:if>
		</plx:function>
	</xsl:template>
	<xsl:template name="GenerateINotifyPropertyChangedImplementation">
		<xsl:call-template name="GeneratePropertyChangeEvent">
			<xsl:with-param name="changeType" select="'Changed'"/>
			<xsl:with-param name="eventIndex" select="0"/>
			<xsl:with-param name="propertyChangedEvent" select="true()"/>
		</xsl:call-template>
		<plx:function visibility="private" name="RaisePropertyChangedEvent">
			<plx:param name="propertyName" dataTypeName=".string"/>
			<plx:local name="eventHandler" dataTypeName="PropertyChangedEventHandler">
				<plx:initialize>
					<plx:cast type="testCast" dataTypeName="PropertyChangedEventHandler">
						<plx:callInstance type="arrayIndexer" name=".implied">
							<plx:callObject>
								<plx:callThis name="Events" type="field"/>
							</plx:callObject>
							<plx:passParam>
								<plx:value type="i4" data="0"/>
							</plx:passParam>
						</plx:callInstance>
					</plx:cast>
				</plx:initialize>
			</plx:local>
			<plx:branch>
				<plx:condition>
					<plx:binaryOperator type="identityInequality">
						<plx:left>
							<plx:nameRef name="eventHandler"/>
						</plx:left>
						<plx:right>
							<plx:nullKeyword/>
						</plx:right>
					</plx:binaryOperator>
				</plx:condition>
				<plx:body>
					<xsl:variable name="commonCallCode">
						<plx:callObject>
							<plx:nameRef type="local" name="eventHandler"/>
						</plx:callObject>
						<plx:passParam>
							<plx:thisKeyword/>
						</plx:passParam>
						<plx:passParam>
							<plx:callNew dataTypeName="PropertyChangedEventArgs">
								<plx:passParam>
									<plx:nameRef type="parameter" name="propertyName"/>
								</plx:passParam>
							</plx:callNew>
						</plx:passParam>
					</xsl:variable>
					<xsl:choose>
						<xsl:when test="$RaiseEventsAsynchronously">
							<plx:callInstance type="methodCall" name="BeginInvoke">
								<xsl:copy-of select="$commonCallCode"/>
								<plx:passParam>
									<plx:callNew type="newDelegate" dataTypeName="AsyncCallback" dataTypeQualifier="System">
										<plx:passParam>
											<plx:callInstance type="methodReference" name="EndInvoke">
												<plx:callObject>
													<plx:nameRef type="local" name="eventHandler"/>
												</plx:callObject>
											</plx:callInstance>
										</plx:passParam>
									</plx:callNew>
								</plx:passParam>
								<plx:passParam>
									<plx:nullKeyword/>
								</plx:passParam>
							</plx:callInstance>
						</xsl:when>
						<xsl:otherwise>
							<plx:callInstance name=".implied" type="delegateCall">
								<xsl:copy-of select="$commonCallCode"/>
							</plx:callInstance>
						</xsl:otherwise>
					</xsl:choose>
				</plx:body>
			</plx:branch>
		</plx:function>
	</xsl:template>
	<xsl:template name="GenerateToString">
		<xsl:param name="className"/>
		<xsl:param name="properties"/>
		<xsl:variable name="nonCollectionProperties" select="$properties[not(@collection='true')]"/>
		<plx:function visibility="public" modifier="sealedOverride" overload="true" name="ToString">
			<plx:returns dataTypeName=".string"/>
			<plx:return>
				<plx:callThis accessor="this" type="methodCall" name="ToString">
					<plx:passParam>
						<plx:nullKeyword/>
					</plx:passParam>
				</plx:callThis>
			</plx:return>
		</plx:function>
		<plx:function visibility="public" overload="true" name="ToString">
			<plx:param type="in" name="provider" dataTypeName="IFormatProvider"/>
			<plx:returns dataTypeName=".string"/>
			<plx:return>
				<plx:callStatic name="Format" dataTypeName=".string">
					<plx:passParam>
						<plx:nameRef type="parameter" name="provider"/>
					</plx:passParam>
					<plx:passParam>
						<plx:string>
							<xsl:value-of select="concat($className,'{0}{{{0}{1}')"/>
							<xsl:for-each select="$nonCollectionProperties">
								<xsl:value-of select="concat(@name,' = ')"/>
								<xsl:if test="not(@customType='true')">
									<xsl:value-of select="'&quot;'"/>
								</xsl:if>
								<xsl:value-of select="concat('{',position()+1,'}')"/>
								<xsl:if test="not(@customType='true')">
									<xsl:value-of select="'&quot;'"/>
								</xsl:if>
								<xsl:if test="not(position()=last())">
									<xsl:value-of select="',{0}{1}'"/>
								</xsl:if>
							</xsl:for-each>
							<xsl:value-of select="'{0}}}'"/>
						</plx:string>
					</plx:passParam>
					<plx:passParam>
						<plx:callStatic type="field" name="NewLine" dataTypeName="Environment"/>
					</plx:passParam>
					<plx:passParam>
						<plx:string>
							<xsl:text>&#x09;</xsl:text>
						</plx:string>
					</plx:passParam>
					<xsl:for-each select="$nonCollectionProperties">
						<plx:passParam>
							<xsl:choose>
								<xsl:when test="@customType='true'">
									<plx:string>TODO: Recursively call ToString for customTypes...</plx:string>
								</xsl:when>
								<xsl:otherwise>
									<plx:callThis accessor="this" type="property" name="{@name}"/>
								</xsl:otherwise>
							</xsl:choose>
						</plx:passParam>
					</xsl:for-each>
				</plx:callStatic>
			</plx:return>
		</plx:function>
	</xsl:template>

	<xsl:template name="GenerateMandatoryParameters">
		<xsl:param name="properties"/>
		<!--nonCustomOnly: set to true() to force creation of single values only parameters-->
		<xsl:param name="nonCustomOnly" select="false()"/>
		<!--nullPlaceHolders: set to true to generate a placeholder for custom types when
		nonCustomOnly = true() (used by GenerateDeserializationContextMethod)-->
		<xsl:param name="nullPlaceholders" select="false()"/>
		<xsl:for-each select="$properties">
			<xsl:if test="(not($nonCustomOnly) or not(@customType='true')) and (@mandatory='true' or @mandatory='relaxed')">
				<xsl:choose>
					<xsl:when test="$nullPlaceholders and @customType='true'">
						<plx:passParam>
							<plx:nullKeyword/>
						</plx:passParam>
					</xsl:when>
					<xsl:otherwise>
						<plx:param name="{@name}">
							<xsl:copy-of select="DataType/@*"/>
							<xsl:copy-of select="DataType/child::*"/>
						</plx:param>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>

	<xsl:template name="GenerateModelContextInterfaceMethods">
		<xsl:param name="Model"/>
		<xsl:param name="ModelDeserializationName"/>
		<plx:function visibility="public" modifier="abstract" name="BeginDeserialization">
			<plx:returns dataTypeName="I{$ModelDeserializationName}"/>
		</plx:function>
		<plx:property visibility="public" modifier="abstract" name="IsDeserializing">
			<plx:returns dataTypeName=".boolean"/>
			<plx:get/>
		</plx:property>
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
			<plx:function visibility="public" name="Get{$uniqueObjectName}By{@Name}">
				<xsl:for-each select="$parameters">
					<plx:param name="{@name}">
						<xsl:copy-of select="DataType/@*"/>
						<xsl:copy-of select="DataType/child::*"/>
					</plx:param>
				</xsl:for-each>
				<plx:returns dataTypeName="{$uniqueObjectName}"/>
			</plx:function>
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
				<plx:function visibility="public" name="Get{$uniqueObjectName}By{@Name}">
					<xsl:for-each select="$parameters">
						<plx:param name="{@name}">
							<xsl:copy-of select="DataType/@*"/>
							<xsl:copy-of select="DataType/child::*"/>
						</plx:param>
					</xsl:for-each>
					<plx:returns dataTypeName="{$uniqueObjectName}"/>
				</plx:function>
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
		<xsl:if test="not($nonCustomOnly)">
			<plx:property visibility="public" modifier="abstract" name="{$className}Collection">
				<plx:returns dataTypeName="ReadOnlyCollection">
					<plx:passTypeParam dataTypeName="{$className}"/>
				</plx:returns>
				<plx:get/>
			</plx:property>
			<xsl:call-template name="GenerateModelContextInterfaceSimpleLookupMethods">
				<xsl:with-param name="Model" select="$Model"/>
				<xsl:with-param name="className" select="$className"/>
				<xsl:with-param name="properties" select="$properties"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
	<xsl:template name="GenerateModelContextInterfaceCreateMethod">
		<xsl:param name="Model"/>
		<xsl:param name="className"/>
		<xsl:param name="properties"/>
		<xsl:param name="nonCustomOnly"/>
		<plx:function visibility="public" modifier="abstract" name="Create{$className}">
			<xsl:call-template name="GenerateMandatoryParameters">
				<xsl:with-param name="properties" select="$properties"/>
				<xsl:with-param name="nonCustomOnly" select="$nonCustomOnly"/>
			</xsl:call-template>
			<plx:returns dataTypeName="{$className}"/>
		</plx:function>
	</xsl:template>
	<xsl:template name="GenerateModelContextInterfaceSimpleLookupMethods">
		<xsl:param name="Model"/>
		<xsl:param name="className"/>
		<xsl:param name="properties"/>
		<xsl:for-each select="$properties">
			<xsl:if test="@unique='true' and not(@customType='true')">
				<plx:function visibility="public" modifier="abstract" name="Get{$className}By{@name}">
					<plx:param name="value">
						<xsl:call-template name="GetUniquePropertyDataType"/>
					</plx:param>
					<plx:returns dataTypeName="{$className}"/>
				</plx:function>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>

	<xsl:include href="GenerateGlobalSupportClasses.xslt"/>
	<xsl:include href="ModelContext.xslt"/>
	<xsl:include href="SerializationEngine.xslt"/>
	<xsl:template match="ormRoot:ORM2">
		<plx:root>
			<plx:namespaceImport name="System"/>
			<plx:namespaceImport name="System.Collections"/>
			<plx:namespaceImport name="System.Collections.Generic"/>
			<plx:namespaceImport name="System.Collections.ObjectModel"/>
			<plx:namespaceImport name="System.ComponentModel"/>
			<plx:namespaceImport name="System.Xml"/>
			<xsl:call-template name="GenerateGlobalSupportClasses"/>
			<plx:namespace name="{$CustomToolNamespace}">
				<plx:leadingInfo>
					<plx:comment blankLine="true"/>
					<plx:comment>
						<xsl:text disable-output-escaping="yes">&lt;![CDATA[</xsl:text>
						<xsl:copy-of select="$AbsorbedObjects"/>
						<xsl:text disable-output-escaping="yes">]]&gt;</xsl:text>
					</plx:comment>
					<plx:comment blankLine="true"/>
				</plx:leadingInfo>
				<xsl:apply-templates mode="Main" select="orm:ORMModel"/>
			</plx:namespace>
		</plx:root>
	</xsl:template>
	<xsl:template match="orm:ORMModel" mode="Main">
		<xsl:variable name="ModelName" select="@Name"/>
		<xsl:variable name="ModelContextName" select="concat($ModelName,'Context')"/>
		<xsl:variable name="ModelDeserializationName" select="concat('Deserialization',$ModelContextName)"/>
		<plx:namespace name="{$ModelName}">
			<xsl:apply-templates mode="ForGenerateAbstractClass" select="$AbsorbedObjects">
				<xsl:with-param name="Model" select="."/>
				<xsl:with-param name="ModelContextName" select="$ModelContextName"/>
			</xsl:apply-templates>
			<plx:interface visibility="public" name="I{$ModelContextName}">
				<plx:leadingInfo>
					<plx:pragma type="region" data="I{$ModelContextName}"/>
				</plx:leadingInfo>
				<plx:trailingInfo>
					<plx:pragma type="closeRegion" data="I{$ModelContextName}"/>
				</plx:trailingInfo>
				<xsl:call-template name="GenerateModelContextInterfaceMethods">
					<xsl:with-param name="Model" select="."/>
					<xsl:with-param name="ModelDeserializationName" select="$ModelDeserializationName"/>
				</xsl:call-template>
				<xsl:apply-templates mode="ForGenerateModelContextInterfaceObjectMethods" select="$AbsorbedObjects">
					<xsl:with-param name="Model" select="."/>
					<xsl:with-param name="nonCustomOnly" select="false()"/>
				</xsl:apply-templates>
			</plx:interface>
			<plx:interface visibility="public" name="I{$ModelDeserializationName}">
				<plx:leadingInfo>
					<plx:pragma type="region" data="I{$ModelDeserializationName}"/>
				</plx:leadingInfo>
				<plx:trailingInfo>
					<plx:pragma type="closeRegion" data="I{$ModelDeserializationName}"/>
				</plx:trailingInfo>
				<plx:implementsInterface dataTypeName="IDisposable"/>
				<xsl:apply-templates mode="ForGenerateModelContextInterfaceObjectMethods" select="$AbsorbedObjects">
					<xsl:with-param name="Model" select="."/>
					<xsl:with-param name="nonCustomOnly" select="true()"/>
				</xsl:apply-templates>
			</plx:interface>
			<xsl:call-template name="GenerateErrorEnums">
				<xsl:with-param name="AbsorbedObjects" select="$AbsorbedObjects" />
			</xsl:call-template>
			<xsl:call-template name="GenerateImplementation">
				<xsl:with-param name="ModelContextName" select="$ModelContextName"/>
				<xsl:with-param name="ModelDeserializationName" select="$ModelDeserializationName"/>
			</xsl:call-template>
			<xsl:if test="boolean($GenerateSerializationClasses)">
				<xsl:call-template name="CreateSerializationClass">
					<xsl:with-param name="AbsorbedObjects" select="$AbsorbedObjects"/>
					<xsl:with-param name="ModelContextName" select="$ModelContextName"/>
				</xsl:call-template>
			</xsl:if>
		</plx:namespace>
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
		<Property mandatory="{@mandatory}" unique="{@unique}" realRoleRef="{@oppositeRoleRef}" customType="true" oppositeName="{@roleName}">
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
					<xsl:choose>
						<xsl:when test="contains(@multiplicity,'Many')">
							<xsl:attribute name="readOnly">
								<xsl:value-of select="true()"/>
							</xsl:attribute>
							<xsl:attribute name="collection">
								<xsl:value-of select="true()"/>
							</xsl:attribute>
							<DataType dataTypeName="ICollection">
								<plx:passTypeParam dataTypeName="{@oppositeObjectName}"/>
							</DataType>
						</xsl:when>
						<xsl:otherwise>
							<xsl:attribute name="readOnly">
								<xsl:value-of select="false()"/>
							</xsl:attribute>
							<xsl:attribute name="collection">
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
						<xsl:value-of select="$factName"/>
						<xsl:text>As</xsl:text>
						<xsl:value-of select="@roleName"/>
					</xsl:attribute>
					<xsl:attribute name="readOnly">
						<xsl:value-of select="true()"/>
					</xsl:attribute>
					<xsl:attribute name="collection">
						<xsl:value-of select="true()"/>
					</xsl:attribute>
					<DataType dataTypeName="ICollection">
						<plx:passTypeParam dataTypeName="{$factName}{$AssociationClassSuffix}"/>
					</DataType>
				</xsl:otherwise>
			</xsl:choose>
		</Property>
	</xsl:template>
	<xsl:template match="ao:Association/ao:RelatedObject" mode="TransformPropertyObjects">
		<xsl:param name="Model"/>
		<Property mandatory="true" unique="{@unique}" realRoleRef="{@roleRef}" readOnly="false" customType="true" collection="false">
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
			<xsl:attribute name="oppositeName">
				<xsl:value-of select="../@name"/>
				<xsl:text>As</xsl:text>
				<xsl:value-of select="@roleName"/>
			</xsl:attribute>
			<DataType dataTypeName="{@className}"/>
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
				<Property multiplicity="{@multiplicity}" unique="{@unique}" realRoleRef="{@thisRoleRef}" customType="{$nested/@customType}" collection="{$nested/@collection}">
					<xsl:attribute name="name">
						<xsl:choose>
							<xsl:when test="string-length(@oppositeRoleName)">
								<xsl:value-of select="@oppositeRoleName"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:if test="not(contains($nested/@name,@name))">
									<xsl:value-of select="@name"/>
								</xsl:if>
								<xsl:value-of select="$nested/@name"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:attribute name="mandatory">
						<xsl:choose>
							<xsl:when test="local-name(..)='Association'">
								<xsl:value-of select="true()"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="@mandatory"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:copy-of select="$nested/DataType"/>
				</Property>
			</xsl:when>
			<xsl:otherwise>
				<Property multiplicity="{@multiplicity}" mandatory="{@mandatory}" unique="{@unique}" customType="false">
					<xsl:attribute name="realRoleRef">
						<xsl:choose>
							<xsl:when test="string-length(@oppositeRoleRef)">
								<xsl:value-of select="@oppositeRoleRef"/>
							</xsl:when>
							<xsl:when test="string-length(@thisRoleRef)">
								<xsl:value-of select="@thisRoleRef"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:message terminate="yes">If we've hit this point, something is very wrong...</xsl:message>
							</xsl:otherwise>
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
					<xsl:variable name="objectId" select="@ref"/>
					<xsl:variable name="valueObject" select="$Model/orm:Objects/orm:ValueType[@id=$objectId]"/>
					<xsl:variable name="dataTypeId" select="$valueObject/orm:ConceptualDataType/@ref"/>
					<xsl:variable name="dataTypeFragment">
						<xsl:for-each select="$Model/orm:DataTypes/child::*[@id=$dataTypeId]">
							<xsl:call-template name="MapDataType"/>
						</xsl:for-each>
					</xsl:variable>
					<xsl:variable name="dataType" select="msxsl:node-set($dataTypeFragment)/child::*"/>
					<xsl:choose>
						<xsl:when test="contains(@multiplicity,'Many')">
							<xsl:attribute name="readOnly">
								<xsl:value-of select="true()"/>
							</xsl:attribute>
							<xsl:attribute name="collection">
								<xsl:value-of select="true()"/>
							</xsl:attribute>
							<DataType dataTypeName="ICollection">
								<plx:passTypeParam>
									<xsl:copy-of select="$dataType/@*"/>
								</plx:passTypeParam>
							</DataType>
						</xsl:when>
						<xsl:otherwise>
							<xsl:attribute name="readOnly">
								<xsl:value-of select="false()"/>
							</xsl:attribute>
							<xsl:attribute name="collection">
								<xsl:value-of select="false()"/>
							</xsl:attribute>
							<xsl:copy-of select="$dataType"/>
						</xsl:otherwise>
					</xsl:choose>
				</Property>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

</xsl:stylesheet>
