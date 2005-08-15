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
	<xs:schema targetNamespace="http://Schemas.Neumont.edu/ORM/SDK/ClassGenerator/AbsorbedObjects" attributeFormDefault="unqualified" elementFormDefault="qualified" xmlns:xs="http://www.w3.org/2001/XMLSchema">
		<xs:element name="Object" type="ao:ObjectType"/>
		<xs:element name="Association" type="ao:AssociationType"/>
		<xs:simpleType name="ObjectTypeStyle">
			<xs:annotation>
				<xs:documentation>A list of the different names for ObjectType tags in the .orm file.</xs:documentation>
			</xs:annotation>
			<xs:restriction base="xs:string">
				<xs:enumeration value="EntityType">
					<xs:annotation>
						<xs:documentation>The ObjectType element is based on an EntityType element in the .orm file</xs:documentation>
					</xs:annotation>
				</xs:enumeration>
				<xs:enumeration value="ObjectifiedType">
					<xs:annotation>
						<xs:documentation>The ObjectType element is based on an ObjectifiedType element in the .orm file</xs:documentation>
					</xs:annotation>
				</xs:enumeration>
				<xs:enumeration value="ValueType">
					<xs:annotation>
						<xs:documentation>The ObjectType element is based on a ValueType element in the .orm file</xs:documentation>
					</xs:annotation>
				</xs:enumeration>
			</xs:restriction>
		</xs:simpleType>
		<xs:complexType name="ObjectType">
			<xs:annotation>
				<xs:documentation>An EntityType that is not absorbed</xs:documentation>
			</xs:annotation>
			<xs:sequence>
				<xs:choice maxOccurs="unbounded">
					<xs:element maxOccurs="unbounded" name="AbsorbedObject" type="ao:AbsorbedObjectType"/>
					<xs:element maxOccurs="unbounded" name="RelatedObject">
						<xs:annotation>
							<xs:documentation>An object not being absorbed, but is related</xs:documentation>
						</xs:annotation>
						<xs:complexType>
							<xs:attribute name="factRef" type="xs:string" use="required">
								<xs:annotation>
									<xs:documentation>id of this Fact</xs:documentation>
								</xs:annotation>
							</xs:attribute>
							<xs:attribute name="roleRef" type="xs:string" use="required">
								<xs:annotation>
									<xs:documentation>Role id of absorbing Object</xs:documentation>
								</xs:annotation>
							</xs:attribute>
							<xs:attribute name="roleName" type="xs:string" use="required">
								<xs:annotation>
									<xs:documentation>Role name of absorbing Object</xs:documentation>
								</xs:annotation>
							</xs:attribute>
							<xs:attribute name="arity" type="xs:unsignedByte" use="required">
								<xs:annotation>
									<xs:documentation>Number of roles in Fact</xs:documentation>
								</xs:annotation>
							</xs:attribute>
							<xs:attribute name="multiplicity" type="xs:string" use="optional">
								<xs:annotation>
									<xs:documentation>Multiplicity of RelatedObject</xs:documentation>
								</xs:annotation>
							</xs:attribute>
							<xs:attribute name="oppositeRoleRef" type="xs:string" use="optional">
								<xs:annotation>
									<xs:documentation>Role id of RelatedObject</xs:documentation>
								</xs:annotation>
							</xs:attribute>
							<xs:attribute name="oppositeObjectRef" type="xs:string" use="optional">
								<xs:annotation>
									<xs:documentation>id of RelatedObject</xs:documentation>
								</xs:annotation>
							</xs:attribute>
							<xs:attribute name="oppositeObjectName" type="xs:string" use="optional">
								<xs:annotation>
									<xs:documentation>Name of RelatedObject</xs:documentation>
								</xs:annotation>
							</xs:attribute>
							<xs:attribute name="oppositeRoleName" type="xs:string" use="optional">
								<xs:annotation>
									<xs:documentation>RoleName of RelatedObject</xs:documentation>
								</xs:annotation>
							</xs:attribute>
						</xs:complexType>
					</xs:element>
				</xs:choice>
			</xs:sequence>
			<xs:attribute name="type" type="ao:ObjectTypeStyle" use="required"/>
			<xs:attribute name="id" type="xs:string" use="required">
				<xs:annotation>
					<xs:documentation>id of ObjectType</xs:documentation>
				</xs:annotation>
			</xs:attribute>
			<xs:attribute name="name" type="xs:string" use="required">
				<xs:annotation>
					<xs:documentation>Name of the ObjectType</xs:documentation>
				</xs:annotation>
			</xs:attribute>
		</xs:complexType>
		<xs:complexType name="AbsorbedObjectType">
			<xs:annotation>
				<xs:documentation>An absorbed EntityType or ValueType</xs:documentation>
			</xs:annotation>
			<xs:sequence minOccurs="0">
				<xs:element name="AbsorbedObject" type="ao:AbsorbedObjectType" />
			</xs:sequence>
			<xs:attribute name="type" type="ao:ObjectTypeStyle" use="required"/>
			<xs:attribute name="ref" type="xs:string" use="required">
				<xs:annotation>
					<xs:documentation>id of this AbsorbedObject</xs:documentation>
				</xs:annotation>
			</xs:attribute>
			<xs:attribute name="name" type="xs:string" use="required">
				<xs:annotation>
					<xs:documentation>Name of this AbsorbedObject</xs:documentation>
				</xs:annotation>
			</xs:attribute>
			<xs:attribute name="thisRoleName" type="xs:string" use="required">
				<xs:annotation>
					<xs:documentation>Role Name of absorbing Object</xs:documentation>
				</xs:annotation>
			</xs:attribute>
			<xs:attribute name="thisRoleRef" type="xs:string" use="required">
				<xs:annotation>
					<xs:documentation>Role id of absorbing Object</xs:documentation>
				</xs:annotation>
			</xs:attribute>
			<xs:attribute name="oppositeRoleRef" type="xs:string" use="required">
				<xs:annotation>
					<xs:documentation>Role id of this AbsorbedObject</xs:documentation>
				</xs:annotation>
			</xs:attribute>
			<xs:attribute name="oppositeRoleName" type="xs:string" use="required">
				<xs:annotation>
					<xs:documentation>Role Name of this AbsorbedObject</xs:documentation>
				</xs:annotation>
			</xs:attribute>
		</xs:complexType>
		<xs:complexType name="AssociationType">
			<xs:annotation>
				<xs:documentation>For Facts with an arity of 3 or more</xs:documentation>
			</xs:annotation>
			<xs:choice minOccurs="0" maxOccurs="unbounded">
				<xs:element maxOccurs="unbounded" name="RelatedObject">
					<xs:annotation>
						<xs:documentation>An object not being absorbed</xs:documentation>
					</xs:annotation>
					<xs:complexType>
						<xs:attribute name="type" type="ao:ObjectTypeStyle" use="required"/>
						<xs:attribute name="objectRef" type="xs:string" use="required">
							<xs:annotation>
								<xs:documentation>id of this RelatedObject</xs:documentation>
							</xs:annotation>
						</xs:attribute>
						<xs:attribute name="roleRef" type="xs:string" use="required">
							<xs:annotation>
								<xs:documentation>Role id of this RelatedObject</xs:documentation>
							</xs:annotation>
						</xs:attribute>
						<xs:attribute name="roleName" type="xs:string" use="required">
							<xs:annotation>
								<xs:documentation>Role Name of this RelatedObject</xs:documentation>
							</xs:annotation>
						</xs:attribute>
						<xs:attribute name="objectName" type="xs:string" use="required">
							<xs:annotation>
								<xs:documentation>Name of this RelatedObject</xs:documentation>
							</xs:annotation>
						</xs:attribute>
					</xs:complexType>
				</xs:element>
				<xs:element maxOccurs="unbounded" name="AbsorbedObject">
					<xs:annotation>
						<xs:documentation>An absorbed EntityType or ValueType</xs:documentation>
					</xs:annotation>
					<xs:complexType>
						<xs:sequence minOccurs="0">
							<xs:element name="AbsorbedObject" type="ao:AbsorbedObjectType" />
						</xs:sequence>
						<xs:attribute name="type" type="ao:ObjectTypeStyle" use="required"/>
						<xs:attribute name="ref" type="xs:string" use="required">
							<xs:annotation>
								<xs:documentation>id of this AbsorbedObject</xs:documentation>
							</xs:annotation>
						</xs:attribute>
						<xs:attribute name="name" type="xs:string" use="required">
							<xs:annotation>
								<xs:documentation>Name of this AbsorbedObject</xs:documentation>
							</xs:annotation>
						</xs:attribute>
						<xs:attribute name="roleName" type="xs:string" use="required">
							<xs:annotation>
								<xs:documentation>Role Name of this AbsorbedObject</xs:documentation>
							</xs:annotation>
						</xs:attribute>
						<xs:attribute name="thisRoleRef" type="xs:string" use="required">
							<xs:annotation>
								<xs:documentation>Role id of this AbsorbedObject</xs:documentation>
							</xs:annotation>
						</xs:attribute>
					</xs:complexType>
				</xs:element>
			</xs:choice>
			<xs:attribute name="name" type="xs:string" use="required">
				<xs:annotation>
					<xs:documentation>Name of Fact</xs:documentation>
				</xs:annotation>
			</xs:attribute>
			<xs:attribute name="id" type="xs:string" use="required">
				<xs:annotation>
					<xs:documentation>id of Fact</xs:documentation>
				</xs:annotation>
			</xs:attribute>
		</xs:complexType>
	</xs:schema>

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
	<xsl:template match="orm:Fact" mode="ForFunctionalBinaryFacts">
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
			<!-- The algorithm here first picks all roles with one candidate role only (1), falling back on
				then the roles opposite a preferred uniqueness constraint (2), falling back on the only mandatory
				role (3), falling back on the role that is a role player for the most roles (4), falling back
				on a random role (5). Note that steps 4 and 5 are non-deterministic and we will need extension
				attributes attached to the roles to make a better choice. The output is a set of FunctionalObject
				elements with attributes ref (pointing to the object type) and roleRef (indicating the attaching role).
				These are then transformed into FunctionalObject/FunctionalRole elements to handle the case where an
				object is functional in more than one fact type. -->
			<xsl:for-each select="$FunctionalBinaryFacts">
				<xsl:variable name="Roles" select="orm:FactRoles/orm:Role"/>
				<xsl:choose>
					<xsl:when test="@functionalRolesCount=1">
						<!-- There is only one candidate role (1) -->
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
								<!-- The roles is opposite a preferred uniqueness constraint (2) -->
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
										<!-- The role is the only mandatory role (3) -->
										<xsl:for-each select="$MandatoryRoles">
											<FunctionalObject ref="{orm:RolePlayer/@ref}" roleRef="{@id}"/>
										</xsl:for-each>
									</xsl:when>
									<xsl:otherwise>
										<!-- Use the object for the role with the most role players (4) -->
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
							<ao:Object type="{local-name()}" id="{@id}" name="{@Name}">
								<xsl:for-each select="orm:PlayedRoles/orm:Role">
									<xsl:variable name="roleId" select="@ref"/>
									<xsl:for-each select="$RawFacts//orm:Role[@id=$roleId]">
										<xsl:variable name="parentFact" select="ancestor::*[2]"/>
										<ao:RelatedObject factRef="{$parentFact/@id}" roleRef="{$roleId}" roleName="{$parentFact/orm:FactRoles/orm:Role[@id=$roleId]/@Name}">
											<xsl:variable name="oppositeRoles" select="$parentFact/orm:FactRoles/child::*[@id!=$roleId]"/>
											<xsl:attribute name="arity">
												<xsl:value-of select="count($parentFact/orm:FactRoles/orm:Role)"/>
											</xsl:attribute>
											<xsl:if test="1=count($oppositeRoles)">
												<xsl:for-each select="$oppositeRoles">
													<xsl:attribute name="multiplicity">
														<xsl:value-of select="@Multiplicity"/>
													</xsl:attribute>
													<xsl:variable name="oppositeRoleId" select="@id"/>
													<xsl:attribute name="oppositeRoleRef">
														<xsl:value-of select="$oppositeRoleId"/>
													</xsl:attribute>
													<xsl:variable name="oppositeObjectId" select="orm:RolePlayer/@ref"/>
													<xsl:attribute name="oppositeObjectRef">
														<xsl:value-of select="$oppositeObjectId"/>
													</xsl:attribute>
													<xsl:variable name="oppositeObject" select="$RawObjects[@id=$oppositeObjectId]"/>
													<xsl:attribute name="oppositeObjectName">
														<xsl:value-of select="$oppositeObject/@Name"/>
													</xsl:attribute>
													<xsl:attribute name="oppositeRoleName">
														<xsl:value-of select="$RawFacts//orm:Role[@id=$oppositeRoleId]/@Name"/>
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
								<ao:RelatedObject type="{@type}" objectRef="{@id}" roleRef="{$roleId}" roleName="{$roleName}" objectName="{@name}">
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
										<ao:RelatedObject type="{@type}" objectRef="{@id}" roleRef="{$roleId}" roleName="{$roleName}" objectName="{@name}">
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
													<xsl:for-each select="..">
														<xsl:copy>
															<xsl:copy-of select="@*"/>
															<xsl:copy-of select="ao:AbsorbedObject"/>
															<xsl:for-each select="$oppositeObject">
																<ao:AbsorbedObject type="{@type}" ref="{@id}" name="{@name}" thisRoleName="{$roleName}" thisRoleRef="{$roleId}" oppositeRoleRef="{ao:RelatedObject/@roleRef}" oppositeRoleName="{ao:RelatedObject/@roleName}">
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
													<ao:AbsorbedObject type="{@type}" ref="{@id}" name="{@name}" thisRoleName="{$roleName}" thisRoleRef="{$roleId}" oppositeRoleRef="{$oppositeRelatedObject/@roleRef}" oppositeRoleName="{$oppositeRelatedObject/@roleName}">
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
			<xsl:when test="$tagName='FixedLengthTextDataType' or $tagName='VariableLengthTextDataType' or $tagName='VariableLengthTextDataType'">
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
				<DataType dataTypeName="DataTime" dataTypeQualifier="System"/>
			</xsl:when>
			<xsl:when test="$tagName='TrueOrFalseLogicalDataType' or $tagName='YesOrNoLogicalDataType'">
				<DataType dataTypeName="Boolean" dataTypeQualifier="System"/>
			</xsl:when>
			<xsl:when test="$tagName='RowIdOtherDataType' or $tagName='ObjectIdOtherDataType'">
				<DataType dataTypeName="UInt32" dataTypeQualifier="System"/>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
	<!-- Generate a public readonly property backed by a field. The current context should
	     be an element with a name attribute and a DataType child element with attributes/children
		 corresponding to the plix attributes and child nodes for a data type reference. -->
	<xsl:template name="GenerateBackedProperty">
		<xsl:param name="initializeFields" select="true()"/>
		<plx:Field name="my{@name}" visibility="Private">
			<xsl:for-each select="DataType">
				<xsl:copy-of select="@*"/>
				<xsl:copy-of select="child::*"/>
				<xsl:if test="$initializeFields">
					<plx:Initialize>
						<plx:DefaultValueOf>
							<xsl:copy-of select="@*"/>
							<xsl:copy-of select="child::*"/>
						</plx:DefaultValueOf>
					</plx:Initialize>
				</xsl:if>
			</xsl:for-each>
		</plx:Field>
		<plx:Property name="{@name}" visibility="Public">
			<plx:Param style="RetVal" name="">
				<xsl:for-each select="DataType">
					<xsl:copy-of select="@*"/>
					<xsl:copy-of select="child::*"/>
				</xsl:for-each>
			</plx:Param>
			<plx:Get>
				<plx:Return>
					<plx:CallInstance name="my{@name}" style="Field">
						<plx:CallObject>
							<plx:ThisKeyword/>
						</plx:CallObject>
					</plx:CallInstance>
				</plx:Return>
			</plx:Get>
		</plx:Property>
	</xsl:template>
	<xsl:template name="GenerateParameters">
		<plx:Param name="{@name}" style="In">
			<xsl:for-each select="DataType">
				<xsl:copy-of select="@*"/>
				<xsl:copy-of select="child::*"/>
			</xsl:for-each>
		</plx:Param>
	</xsl:template>
	<xsl:template name="GenerateConstructorAssignment">
		<plx:Operator name="Assign">
			<plx:Left>
				<plx:CallInstance name="my{@name}" style="Field">
					<plx:CallObject>
						<plx:ThisKeyword/>
					</plx:CallObject>
				</plx:CallInstance>
			</plx:Left>
			<plx:Right>
				<plx:Value type="Parameter">
					<xsl:value-of select="@name"/>
				</plx:Value>
			</plx:Right>
		</plx:Operator>
	</xsl:template>
	<xsl:template name="GenerateFactoryMethod">
		<xsl:param name="property"/>
		<xsl:param name="objectName"/>
		<plx:Function name="CreateInstance" visibility="Public" virtual="true" shared="true">
			<xsl:for-each select="$property">
				<xsl:call-template name="GenerateParameters"/>
			</xsl:for-each>
			<plx:Param name="" style="RetVal" dataTypeName="{$objectName}"/>
			<plx:Return>
				<plx:CallNew dataTypeName="{$objectName}">
					<xsl:for-each select="$property">
						<plx:PassParam>
							<plx:Value type="Parameter">
								<xsl:value-of select="@name"/>
							</plx:Value>
						</plx:PassParam>
					</xsl:for-each>
				</plx:CallNew>
			</plx:Return>
		</plx:Function>
	</xsl:template>
	<xsl:template match="ormRoot:ORM2">
		<!--<ao>
			<xsl:copy-of select="$AbsorbedObjects"/>
		</ao>-->
		<xsl:apply-templates mode="Main" select="orm:ORMModel"/>
	</xsl:template>
	<xsl:template match="orm:ORMModel" mode="Main">
		<plx:Root>
			<plx:Using name="System" />
			<plx:Using name="System.Collections.Generic" />
			<plx:Namespace name="{$CustomToolNamespace}">
				<xsl:apply-templates mode="WalkAbsorbedObjects" select="$AbsorbedObjects">
					<xsl:with-param name="Model" select="."/>
				</xsl:apply-templates>
			</plx:Namespace>
		</plx:Root>
	</xsl:template>
	<xsl:template match="ao:Object" mode="WalkAbsorbedObjects">
		<xsl:param name="Model"/>
		<xsl:if test="@type='EntityType'">
			<xsl:variable name="propertyFragment">
				<xsl:apply-templates mode="WalkAbsorbedObjects" select="child::*">
					<xsl:with-param name="Model" select="$Model"/>
				</xsl:apply-templates>
			</xsl:variable>
			<xsl:variable name="property" select="msxsl:node-set($propertyFragment)/child::*"/>
			<plx:Class visibility="Public" partial="true" name="{@name}">
				<plx:Function ctor="true" visibility="Protected">
					<xsl:for-each select="$property">
						<xsl:call-template name="GenerateParameters"/>
					</xsl:for-each>
					<xsl:for-each select="$property">
						<xsl:call-template name="GenerateConstructorAssignment"/>
					</xsl:for-each>
				</plx:Function>
				<!-- TODO: make version with parameters based on required roles -->
				<xsl:for-each select="$property">
					<xsl:call-template name="GenerateBackedProperty">
						<xsl:with-param name="initializeFields" select="true()"/>
					</xsl:call-template>
				</xsl:for-each>
				<xsl:call-template name="GenerateFactoryMethod">
					<xsl:with-param name="property" select="$property"/>
					<xsl:with-param name="objectName" select="@name"/>
				</xsl:call-template>
			</plx:Class>
		</xsl:if>
	</xsl:template>
	<xsl:template match="ao:Association" mode="WalkAbsorbedObjects">
		<xsl:param name="Model"/>
		<xsl:variable name="structName">
			<xsl:value-of select="@name"/>
			<xsl:value-of select="$AssociationClassDecorator"/>
		</xsl:variable>
		<xsl:variable name="propertyFragment">
			<xsl:apply-templates mode="WalkAbsorbedObjects" select="child::*">
				<xsl:with-param name="Model" select="$Model"/>
			</xsl:apply-templates>
		</xsl:variable>
		<xsl:variable name="property" select="msxsl:node-set($propertyFragment)/child::*"/>
		<plx:Structure visibility="Public" partial="true" name="{$structName}">
			<plx:Function ctor="true" visibility="Protected">
				<xsl:for-each select="$property">
					<xsl:call-template name="GenerateParameters"/>
				</xsl:for-each>
				<xsl:for-each select="$property">
					<xsl:call-template name="GenerateConstructorAssignment"/>
				</xsl:for-each>
			</plx:Function>
			<xsl:for-each select="$property">
				<xsl:call-template name="GenerateBackedProperty">
					<xsl:with-param name="initializeFields" select="false()"/>
				</xsl:call-template>
			</xsl:for-each>
			<xsl:call-template name="GenerateFactoryMethod">
				<xsl:with-param name="property" select="$property"/>
				<xsl:with-param name="objectName" select="$structName"/>
			</xsl:call-template>
		</plx:Structure>
	</xsl:template>
	<xsl:template match="ao:Object/ao:RelatedObject" mode="WalkAbsorbedObjects">
		<xsl:param name="Model"/>
		<!-- The generated code is similar for both binary related objects and
		     association objects. Build a Property element with a name attribute and
			 DataType element for all both cases, then spit the property and its backing field
			 using the GenerateBackedProperty template. -->
		<!--		<xsl:variable name="property">-->
		<Property>
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
					<xsl:variable name="relatedFact" select="$Model/orm:Facts/orm:Fact[@id=$factId]"/>
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
								<xsl:attribute name="dataTypeName">Nullable&lt;bool&gt;</xsl:attribute>
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
				<Property multiplicity="{@multiplicity}">
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
				<Property name="{@roleName}" multiplicity="{@multiplicity}">
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
		<!--		<ao:RelatedObject type="{@type}" objectRef="{@id}" roleRef="{$roleId}" roleName="{$roleName}" objectName="{@name}">
			<xsl:if test="$hasMany">
				<xsl:attribute name="multiplicity">Many</xsl:attribute>
			</xsl:if>
		</ao:RelatedObject>	-->
		<!--		<xsl:variable name="property">-->
		<Property>
			<xsl:attribute name="name">
				<xsl:choose>
					<xsl:when test="string-length(@roleName)">
						<xsl:value-of select="@roleName"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="@objectName"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<DataType>
				<xsl:choose>
					<xsl:when test="contains(@multiplicity, 'Many')">
						<xsl:attribute name="dataTypeName">ICollection</xsl:attribute>
						<plx:PassTypeParam dataTypeName="{@objectName}"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:attribute name="dataTypeName">
							<xsl:value-of select="@objectName"/>
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
			<plx:Field name="my{@roleName}" visibility="Private">
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
				<plx:Param style="RetVal" name="">
					<xsl:copy-of select="$dataType/@*"/>
					<xsl:copy-of select="$dataType/child::*"/>
				</plx:Param>
				<plx:Get>
					<plx:Return>
						<plx:CallInstance name="my{@roleName}" style="Field">
							<plx:CallObject><plx:ThisKeyword/></plx:CallObject>
						</plx:CallInstance>
					</plx:Return>
				</plx:Get>
			</plx:Property>
		</xsl:for-each>-->
	</xsl:template>
</xsl:stylesheet>