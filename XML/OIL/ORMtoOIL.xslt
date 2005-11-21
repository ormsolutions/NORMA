<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:orm="http://schemas.neumont.edu/ORM/ORMCore"
	xmlns:ormRoot="http://schemas.neumont.edu/ORM/ORMRoot"
	xmlns:oil="http://schemas.neumont.edu/ORM/OIL.xsd"
	extension-element-prefixes="msxsl"
	exclude-result-prefixes="orm ormRoot">

	<xsl:output method="xml" encoding="utf-8" media-type="text/xml" indent="yes"/>
	<xsl:strip-space elements="*"/>

	<xsl:variable name="EnableAssertions" select="true()"/>

	<xsl:template match="ormRoot:ORM2">
		<xsl:apply-templates select="orm:ORMModel"/>
	</xsl:template>
	<xsl:template match="orm:ORMModel">
		<xsl:variable name="Model" select="."/>
		<xsl:variable name="objectsAndFacts" select="(orm:Objects|orm:Facts)/child::*"/>

		<xsl:variable name="ObjectTypeInformationFragment">
			<xsl:for-each select="orm:Objects/child::*">
				<xsl:copy>
					<xsl:copy-of select="@*"/>
					<xsl:call-template name="GetObjectTypeInformation">
						<xsl:with-param name="Model" select="$Model"/>
					</xsl:call-template>
				</xsl:copy>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="ObjectTypeInformation" select="msxsl:node-set($ObjectTypeInformationFragment)/child::*"/>

		<!-- NOTE: The descriptions below IGNORE facts that are part of the preferred reference mode. -->
		<!-- Get independent object types. -->
		<xsl:variable name="IndependentObjectTypes" select="$ObjectTypeInformation[@IsIndependent='true']"/>
		<!-- Get subtypes that are not independent. -->
		<xsl:variable name="NonIndependentSubtypeObjectTypes" select="$ObjectTypeInformation[subtypeMetaFacts/child::* and not(@id=$IndependentObjectTypes/@id)]"/>
		<!-- Get the non-objectified fact types that have uniqueness constraints spanning more than one role. -->
		<xsl:variable name="NonObjectifiedMultiRoleUniquenessFactTypes" select="($Model/orm:Facts/orm:Fact|$Model/orm:Facts/orm:ImpliedFact)[orm:InternalConstraints/orm:InternalUniquenessConstraint[count(orm:RoleSequence/orm:Role)>1] and not(@id=$Model/orm:Objects/orm:ObjectifiedType/orm:NestedPredicate/@ref)]"/>
		
		<xsl:variable name="FactTypeAbsorptionsFragment">
			<xsl:for-each select="(orm:Facts/orm:Fact|orm:Facts/orm:ImpliedFact)[count(orm:FactRoles/orm:Role)=2 and count(orm:InternalConstraints/orm:InternalUniquenessConstraint)=2]">
				<xsl:variable name="countMandatories" select="count(orm:InternalConstraints/orm:SimpleMandatoryConstraint)"/>
				<xsl:variable name="rolePlayerIds" select="orm:FactRoles/orm:Role/orm:RolePlayer/@ref"/>
				<xsl:variable name="rolePlayers" select="$ObjectTypeInformation[@id=$rolePlayerIds]"/>
				<AbsorbFactType ref="{@id}">
					<xsl:choose>
						<xsl:when test="$countMandatories = 1">
							<xsl:variable name="mandatoryRolePlayerId" select="orm:InternalConstraints/orm:SimpleMandatoryConstraint/orm:RoleSequence/orm:Role/@ref"/>
							<xsl:variable name="nonMandatoryRolePlayer" select="$rolePlayers[not(@id=$mandatoryRolePlayerId)]"/>
							<xsl:choose>
								<!-- We know that there is at least one functional role, see if there is another... -->
								<xsl:when test="not(local-name($nonMandatoryRolePlayer)='ValueType') and count($nonMandatoryRolePlayer/functionalNonPreferredIdentifierDirectFacts/child::*) > 1">
									<xsl:attribute name="towards">
										<xsl:value-of select="$nonMandatoryRolePlayer/@id"/>
									</xsl:attribute>
									<xsl:attribute name="type">
										<xsl:value-of select="'fully'"/>
									</xsl:attribute>
								</xsl:when>
								<xsl:otherwise>
									<xsl:attribute name="towards">
										<xsl:value-of select="$mandatoryRolePlayerId"/>
									</xsl:attribute>
									<xsl:attribute name="type">
										<xsl:value-of select="'factOnly'"/>
									</xsl:attribute>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:when>
						<xsl:otherwise>
							<xsl:variable name="firstRolePlayer" select="$rolePlayers[@id=$rolePlayerIds[1]]"/>
							<xsl:variable name="secondRolePlayer" select="$rolePlayers[@id=$rolePlayerIds[2]]"/>
							<xsl:variable name="firstRolePlayerCountNonDependentFunctionalNonPreferredIdentifierDirectFacts" select="count($firstRolePlayer/nonDependentFunctionalNonPreferredIdentifierDirectFacts/child::*)"/>
							<xsl:variable name="secondRolePlayerCountNonDependentFunctionalNonPreferredIdentifierDirectFacts" select="count($secondRolePlayer/nonDependentFunctionalNonPreferredIdentifierDirectFacts/child::*)"/>
							<xsl:attribute name="towards">
								<xsl:choose>
									<xsl:when test="not(local-name($firstRolePlayer)='ValueType') and $firstRolePlayerCountNonDependentFunctionalNonPreferredIdentifierDirectFacts >= $secondRolePlayerCountNonDependentFunctionalNonPreferredIdentifierDirectFacts">
										<xsl:value-of select="$firstRolePlayer/@id"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$secondRolePlayer/@id"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
							<xsl:attribute name="type">
								<xsl:choose>
									<xsl:when test="$countMandatories = 0">
										<xsl:value-of select="'factOnly'"/>
									</xsl:when>
									<xsl:when test="$countMandatories = 2">
										<xsl:value-of select="'fully'"/>
									</xsl:when>
									<xsl:when test="$EnableAssertions">
										<xsl:message terminate="yes">
											<xsl:text>SANITY CHECK: A binary really shouldn't have more than two simple mandatory constraints on it.</xsl:text>
										</xsl:message>
									</xsl:when>
								</xsl:choose>
							</xsl:attribute>
						</xsl:otherwise>
					</xsl:choose>
				</AbsorbFactType>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="FactTypeAbsorptions" select="msxsl:node-set($FactTypeAbsorptionsFragment)/child::*"/>
		
		<xsl:variable name="ObjectTypeAbsorptionsFragment">
			<xsl:for-each select="$NonIndependentSubtypeObjectTypes">
				<!-- TODO: The next line should be selecting the PRIMARY supertype, not the FIRST subtype. -->
				<xsl:variable name="absorbingSupertype" select="subtypeMetaFacts/child::*[1]/orm:FactRoles/orm:Role[2]/orm:RolePlayer/@ref"/>
				<AbsorbObjectType ref="{@id}" towards="{$absorbingSupertype}"/>
			</xsl:for-each>
			<!-- Get the non-independent, non-subtype object types that play at least one mandatory functional role in a fact type that that object type is also functionally dependent on. -->
			<xsl:for-each select="$ObjectTypeInformation[mandatoryDependentFunctionalNonPreferredIdentifierDirectFacts/child::* and not(@id=$IndependentObjectTypes/@id) and not(@id=$NonIndependentSubtypeObjectTypes/@id)]">
				<xsl:variable name="specialCaseObjectTypeId" select="@id"/>
				<xsl:for-each select="$FactTypeAbsorptions[@ref=current()/mandatoryDependentFunctionalNonPreferredIdentifierDirectFacts/child::*/@id]">
					<xsl:if test="@type='fully'">
						<AbsorbObjectType ref="{$specialCaseObjectTypeId}" towards="{@towards}"/>
					</xsl:if>
				</xsl:for-each>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="ObjectTypeAbsorptions" select="msxsl:node-set($ObjectTypeAbsorptionsFragment)/child::*"/>

		<!-- Get the non-independent, non-subtype object types that play at least one functional role that isn't absorbed away from that object type. -->
		<xsl:variable name="NonAbsorbedFunctionalRolePlayingObjectTypesFragment">
			<xsl:for-each select="$ObjectTypeInformation[functionalNonPreferredIdentifierDirectFacts/child::* and not(@id=$IndependentObjectTypes/@id) and not(@id=$NonIndependentSubtypeObjectTypes/@id) and not(@id=$ObjectTypeAbsorptions/@ref)]">
				<xsl:variable name="factTypeAbsorptionsAwayFromThisObjectType" select="$FactTypeAbsorptions[not(@towards=current()/@id)]"/>
				<xsl:if test="functionalNonPreferredIdentifierDirectFacts/child::*[not(@id=$factTypeAbsorptionsAwayFromThisObjectType/@ref)]">
					<xsl:copy-of select="."/>
				</xsl:if>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="NonAbsorbedFunctionalRolePlayingObjectTypes" select="msxsl:node-set($NonAbsorbedFunctionalRolePlayingObjectTypesFragment)/child::*"/>

		<!-- Get the independent object types,
			non-objectified fact types with uniqueness constraints that span more than one role,
			value types that play functional roles in fact types that they are not also functionally dependent on,
			and object types that play functional roles that are not fully absorbed by something else. -->
		<xsl:variable name="TopLevelTypes" select="$IndependentObjectTypes | $NonObjectifiedMultiRoleUniquenessFactTypes | $NonAbsorbedFunctionalRolePlayingObjectTypes"/>
		
		<oil:model name="{@Name}" modelRef="{@id}">

			<!-- DEBUG -->
			<!--<ObjectTypeInformation>
				<xsl:copy-of select="$ObjectTypeInformation"/>
			</ObjectTypeInformation>
			<FactTypeAbsorptions>
				<xsl:copy-of select="$FactTypeAbsorptions"/>
			</FactTypeAbsorptions>
			<ObjectTypeAbsorptions>
				<xsl:copy-of select="$ObjectTypeAbsorptions"/>
			</ObjectTypeAbsorptions>
			<TopLevelTypes>
				<xsl:copy-of select="$TopLevelTypes"/>
			</TopLevelTypes>-->
			
			<xsl:apply-templates select="$TopLevelTypes" mode="GenerateEntityTypes">
				<xsl:with-param name="Model" select="$Model"/>
				<xsl:with-param name="ObjectTypeInformation" select="$ObjectTypeInformation"/>
				<xsl:with-param name="FactTypeAbsorptions" select="$FactTypeAbsorptions"/>
				<xsl:with-param name="ObjectTypeAbsorptions" select="$ObjectTypeAbsorptions"/>
				<xsl:with-param name="TopLevelTypes" select="$TopLevelTypes"/>
			</xsl:apply-templates>
			
		</oil:model>

	</xsl:template>

	<!-- Returns a variety of useful information for the current context node. Don't call this directly, use the cached info from $ObjectTypeInformation. -->
	<xsl:template name="GetObjectTypeInformation">
		<xsl:param name="Model"/>

		<xsl:if test="$EnableAssertions and not(local-name()='EntityType' or local-name()='ObjectifiedType' or local-name()='ValueType')">
			<xsl:message terminate="yes">
				<xsl:text>This template has only been designed to work with EntityType and ObjectifiedType elements. It kind of works for ValueType elements as well.</xsl:text>
			</xsl:message>
		</xsl:if>

		<!-- Any subtype meta facts where this object type is the subtype. -->
		<xsl:variable name="subtypeMetaFacts" select="$Model/orm:Facts/orm:SubtypeFact[orm:FactRoles/orm:Role[1]/orm:RolePlayer/@ref=current()/@id]"/>

		<!-- Any subtype meta facts where this object type is the supertype. -->
		<xsl:variable name="supertypeMetaFacts" select="$Model/orm:Facts/orm:SubtypeFact[orm:FactRoles/orm:Role[2]/orm:RolePlayer/@ref=current()/@id]"/>

		<!-- All roles directly played by this object type. -->
		<xsl:variable name="directPlayedRoles" select="orm:PlayedRoles/orm:Role"/>
		
		<!-- All direct and inherited roles that are played by the supertype(s) of this object type. -->
		<xsl:variable name="inheritedPlayedRolesFragment">
			<xsl:for-each select="$Model/orm:Objects/child::*[@id=$subtypeMetaFacts/orm:FactRoles/orm:Role[2]/orm:RolePlayer/@ref]">
				<xsl:call-template name="GetSpecificObjectTypeInformation">
					<xsl:with-param name="Model" select="$Model"/>
					<xsl:with-param name="RequestedInformation" select="'directAndInheritedPlayedRoles'"/>
				</xsl:call-template>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="inheritedPlayedRoles" select="msxsl:node-set($inheritedPlayedRolesFragment)/child::*"/>

		<!-- All roles directly played by this object type or its supertype(s).  -->
		<xsl:variable name="directAndInheritedPlayedRoles" select="$directPlayedRoles | $inheritedPlayedRoles"/>

		<!-- All direct and inherited facts participated in by the supertype(s) of this object type. -->
		<xsl:variable name="inheritedFacts" select="($Model/orm:Facts/orm:Fact|$Model/orm:Facts/orm:ImpliedFact)[orm:FactRoles/orm:Role/@id=$inheritedPlayedRoles/@ref]"/>
		
		<!-- Facts that this object type directly participates in. Facts participated in via join paths and subtyping relationships are NOT included. -->
		<xsl:variable name="directFacts" select="($Model/orm:Facts/orm:Fact|$Model/orm:Facts/orm:ImpliedFact)[orm:FactRoles/orm:Role/@id=current()/orm:PlayedRoles/orm:Role/@ref]"/>

		<!-- All direct and inherited facts for this object type. -->
		<xsl:variable name="directAndInheritedFacts" select="$directFacts | $inheritedFacts"/>
		
		<!-- The internal or external uniqueness constraint that is the preferred identifier for this object type. -->
		<xsl:variable name="preferredIdentifier" select="(($Model/orm:Facts/orm:Fact|$Model/orm:Facts/orm:ImpliedFact)/orm:InternalConstraints/orm:InternalUniquenessConstraint|$Model/orm:ExternalConstraints/orm:ExternalUniquenessConstraint)[@id=current()/orm:PreferredIdentifier/@ref]"/>
		<!-- The facts that are directly part of the preferred identifier of this object type. Note that this may include facts that are NOT in $directAndInheritedFacts. -->
		<xsl:variable name="preferredIdentifierFacts" select="($Model/orm:Facts/orm:Fact|$Model/orm:Facts/orm:ImpliedFact)[orm:FactRoles/orm:Role/@id=$preferredIdentifier/orm:RoleSequence/orm:Role/@ref]"/>

		<!-- $directFacts that are not also $preferredIdentifierFacts -->
		<xsl:variable name="nonPreferredIdentifierDirectFacts" select="$directFacts[not(@id=$preferredIdentifierFacts/@id)]"/>
		
		<!-- $inheritedFacts that are not also $preferredIdentifierFacts -->
		<xsl:variable name="nonPreferredIdentifierInheritedFacts" select="$inheritedFacts[not(@id=$preferredIdentifierFacts/@id)]"/>
		
		<!-- $directAndInheritedFacts that are not also $preferredIdentifierFacts -->
		<xsl:variable name="nonPreferredIdentifierDirectAndInheritedFacts" select="$directAndInheritedFacts[not(@id=$preferredIdentifierFacts/@id)]"/>

		<!-- $nonPreferredIdentifierDirectFacts that are alethicly mandatory -->
		<xsl:variable name="mandatoryNonPreferredIdentifierDirectFacts" select="$nonPreferredIdentifierDirectFacts[orm:InternalConstraints/orm:SimpleMandatoryConstraint[(not(@Modality) or @Modality='Alethic') and orm:RoleSequence/orm:Role/@ref=$directPlayedRoles/@ref]]"/>

		<!-- $nonPreferredIdentifierDirectFacts on which this object type is functionally dependent. -->
		<xsl:variable name="dependentNonPreferredIdentifierDirectFacts" select="$nonPreferredIdentifierDirectFacts[orm:InternalConstraints/orm:InternalUniquenessConstraint[(not(@Modality) or @Modality='Alethic')]/orm:RoleSequence[not(orm:Role/@ref=$directPlayedRoles/@ref)]]"/>

		<!-- $directFacts in which this object type plays a functional role -->
		<xsl:variable name="functionalDirectFacts" select="$directFacts[orm:InternalConstraints/orm:InternalUniquenessConstraint[(not(@Modality) or @Modality='Alethic')]/orm:RoleSequence[count(orm:Role)=1 and orm:Role/@ref=$directPlayedRoles/@ref]]"/>
		
		<!-- $functionalDirectFacts that are not also $preferredIdentifierFacts-->
		<xsl:variable name="functionalNonPreferredIdentifierDirectFacts" select="$functionalDirectFacts[not(@id=$preferredIdentifierFacts/@id)]"/>

		<!-- $functionalNonPreferredIdentifierDirectFacts that are not also $dependentNonPreferredIdentifierDirectFacts -->
		<xsl:variable name="nonDependentFunctionalNonPreferredIdentifierDirectFacts" select="$functionalNonPreferredIdentifierDirectFacts[not(@id=$dependentNonPreferredIdentifierDirectFacts/@id)]"/>
		
		<!-- The intersection of $nonPreferredIdentifierDirectFacts, $nonPreferredIdentifierDirectFacts, and $nonPreferredIdentifierDirectFacts. -->
		<xsl:variable name="mandatoryDependentFunctionalNonPreferredIdentifierDirectFacts" select="$mandatoryNonPreferredIdentifierDirectFacts[@id=$dependentNonPreferredIdentifierDirectFacts/@id and @id=$functionalNonPreferredIdentifierDirectFacts/@id]"/>
		
		<subtypeMetaFacts>
			<xsl:copy-of select="$subtypeMetaFacts"/>
		</subtypeMetaFacts>
		<supertypeMetaFacts>
			<xsl:copy-of select="$supertypeMetaFacts"/>
		</supertypeMetaFacts>
		<directPlayedRoles>
			<xsl:copy-of select="$directPlayedRoles"/>
		</directPlayedRoles>
		<inheritedPlayedRoles>
			<xsl:copy-of select="$inheritedPlayedRoles"/>
		</inheritedPlayedRoles>
		<directAndInheritedPlayedRoles>
			<xsl:copy-of select="$directAndInheritedPlayedRoles"/>
		</directAndInheritedPlayedRoles>
		<inheritedFacts>
			<xsl:copy-of select="$inheritedFacts"/>
		</inheritedFacts>
		<directFacts>
			<xsl:copy-of select="$directFacts"/>
		</directFacts>
		<directAndInheritedFacts>
			<xsl:copy-of select="$directAndInheritedFacts"/>
		</directAndInheritedFacts>
		<preferredIdentifier>
			<xsl:copy-of select="$preferredIdentifier"/>
		</preferredIdentifier>
		<preferredIdentifierFacts>
			<xsl:copy-of select="$preferredIdentifierFacts"/>
		</preferredIdentifierFacts>
		<nonPreferredIdentifierDirectFacts>
			<xsl:copy-of select="$nonPreferredIdentifierDirectFacts"/>
		</nonPreferredIdentifierDirectFacts>
		<nonPreferredIdentifierInheritedFacts>
			<xsl:copy-of select="$nonPreferredIdentifierInheritedFacts"/>
		</nonPreferredIdentifierInheritedFacts>
		<nonPreferredIdentifierDirectAndInheritedFacts>
			<xsl:copy-of select="$nonPreferredIdentifierDirectAndInheritedFacts"/>
		</nonPreferredIdentifierDirectAndInheritedFacts>
		<mandatoryNonPreferredIdentifierDirectFacts>
			<xsl:copy-of select="$mandatoryNonPreferredIdentifierDirectFacts"/>
		</mandatoryNonPreferredIdentifierDirectFacts>
		<dependentNonPreferredIdentifierDirectFacts>
			<xsl:copy-of select="$dependentNonPreferredIdentifierDirectFacts"/>
		</dependentNonPreferredIdentifierDirectFacts>
		<functionalDirectFacts>
			<xsl:copy-of select="$functionalDirectFacts"/>
		</functionalDirectFacts>
		<functionalNonPreferredIdentifierDirectFacts>
			<xsl:copy-of select="$functionalNonPreferredIdentifierDirectFacts"/>
		</functionalNonPreferredIdentifierDirectFacts>
		<nonDependentFunctionalNonPreferredIdentifierDirectFacts>
			<xsl:copy-of select="$nonDependentFunctionalNonPreferredIdentifierDirectFacts"/>
		</nonDependentFunctionalNonPreferredIdentifierDirectFacts>
		<mandatoryDependentFunctionalNonPreferredIdentifierDirectFacts>
			<xsl:copy-of select="$mandatoryDependentFunctionalNonPreferredIdentifierDirectFacts"/>
		</mandatoryDependentFunctionalNonPreferredIdentifierDirectFacts>

	
</xsl:template>
	<xsl:template name="GetSpecificObjectTypeInformation">
		<xsl:param name="Model"/>
		<xsl:param name="RequestedInformation"/>
		<xsl:variable name="allInformationFragment">
			<xsl:call-template name="GetObjectTypeInformation">
				<xsl:with-param name="Model" select="$Model"/>
			</xsl:call-template>
		</xsl:variable>
		<xsl:copy-of select="msxsl:node-set($allInformationFragment)/child::*[local-name()=$RequestedInformation]/child::*"/>
	</xsl:template>
	
	<xsl:template match="orm:EntityType | orm:ObjectifiedType" mode="GenerateEntityTypes">
		<xsl:param name="Model"/>
		<xsl:param name="ObjectTypeInformation"/>
		<xsl:param name="FactTypeAbsorptions"/>
		<xsl:param name="ObjectTypeAbsorptions"/>
		<xsl:param name="TopLevelTypes"/>
		<xsl:variable name="thisObjectTypeId" select="@id"/>
		<xsl:variable name="thisObjectTypeInformation" select="."/>
		<oil:model>
			<oil:entityType name="{@Name}" sourceRef="{@id}">
				
				<xsl:for-each select="$thisObjectTypeInformation/functionalDirectFacts/child::*[not(@id=$FactTypeAbsorptions[not(@towards=$thisObjectTypeId)])]">
					<xsl:variable name="thisRole" select="orm:FactRoles/orm:Role[orm:RolePlayer/@ref=$thisObjectTypeId]"/>
					<xsl:variable name="thisRoleId" select="$thisRole/@id"/>
					<xsl:variable name="oppositeRole" select="orm:FactRoles/orm:Role[not(orm:RolePlayer/@ref=$thisObjectTypeId)]"/>
					<xsl:variable name="oppositeRoleId" select="$oppositeRole/@id"/>
					<xsl:variable name="oppositeRolePlayerId" select="$oppositeRole/orm:RolePlayer/@ref"/>
					<xsl:variable name="oppositeRolePlayer" select="$ObjectTypeInformation[@id=$oppositeRolePlayerId]"/>
					<xsl:variable name="oppositeRolePlayerName" select="$oppositeRolePlayer/@Name"/>
					<xsl:variable name="oppositeRolePlayerTopLevelTypeId">
						<xsl:call-template name="GetTopLevelTypeId">
							<xsl:with-param name="ObjectTypeAbsorptions" select="$ObjectTypeAbsorptions"/>
							<xsl:with-param name="TopLevelTypes" select="$TopLevelTypes"/>
							<xsl:with-param name="TargetId" select="$oppositeRolePlayerId"/>
						</xsl:call-template>
					</xsl:variable>
					<xsl:variable name="mandatory">
						<xsl:variable name="simpleMandatoryConstraint" select="orm:InternalConstriants/orm:SimpleMandatoryConstraint[orm:RoleSequence/orm:Role=$thisRole/@id]"/>
						<xsl:choose>
							<xsl:when test="$simpleMandatoryConstraint">
								<xsl:call-template name="GetModality">
									<xsl:with-param name="Target" select="$simpleMandatoryConstraint"/>
								</xsl:call-template>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="'false'"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<xsl:variable name="name">
						<xsl:choose>
							<xsl:when test="string-length($oppositeRole/@Name)&gt;0">
								<xsl:value-of select="$oppositeRole/@Name"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$oppositeRolePlayerName"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<xsl:variable name="oppositeOilUniquenessConstraint">
						<xsl:variable name="uniquenessConstraint" select="orm:InternalConstraints/orm:InternalUniquenessConstraint[orm:RoleSequence/orm:Role/@ref=$oppositeRoleId]"/>
						<xsl:if test="$uniquenessConstraint">
							<oil:singleRoleUniquenessConstraint name="{$uniquenessConstraint/@Name}" sourceRef="{$uniquenessConstraint/@id}">
								<xsl:attribute name="modality">
									<xsl:call-template name="GetModality">
										<xsl:with-param name="Target" select="$uniquenessConstraint"/>
									</xsl:call-template>
								</xsl:attribute>
								<xsl:attribute name="isPrimary">
									<xsl:value-of select="$thisObjectTypeInformation/primaryIdentifier/orm:InternalUniquenessConstraint/@id = $uniquenessConstraint/@id"/>
								</xsl:attribute>
							</oil:singleRoleUniquenessConstraint>
					</xsl:if>
					</xsl:variable>
					<xsl:variable name="oppositeOilFrequencyConstraint">
						<!-- TODO: Handle frequency constraints in here. -->
					</xsl:variable>
					<xsl:variable name="thisOilValueConstraint">
						<xsl:call-template name="GetOilValueConstraint">
							<xsl:with-param name="Role" select="$thisRole"/>
							<xsl:with-param name="AppliesTo" select="'parent'"/>
						</xsl:call-template>
					</xsl:variable>
					<xsl:variable name="oppositeOilValueConstraint">
						<xsl:call-template name="GetOilValueConstraint">
							<xsl:with-param name="Role" select="$oppositeRole"/>
							<xsl:with-param name="AppliesTo" select="'self'"/>
						</xsl:call-template>
					</xsl:variable>
					<xsl:choose>
						<xsl:when test="not(string-length($oppositeRolePlayerTopLevelTypeId))">
							<xsl:call-template name="GetOilValueTypes">
								<xsl:with-param name="Model" select="$Model"/>
								<xsl:with-param name="ObjectTypeInformation" select="$ObjectTypeInformation"/>
								<xsl:with-param name="RolePlayer" select="$oppositeRolePlayer"/>
								<xsl:with-param name="Mandatory" select="$mandatory"/>
								<xsl:with-param name="SourceRoleRef" select="$thisRoleId"/>
								<xsl:with-param name="BaseName" select="$name"/>
							</xsl:call-template>
						</xsl:when>
						<xsl:when test="$oppositeRolePlayerTopLevelTypeId = $thisObjectTypeId">
							<!-- TODO: Absorbed Entity Types -->
							
						</xsl:when>
						<xsl:when test="not($EnableAssertions) or ($oppositeRolePlayerTopLevelTypeId=($TopLevelTypes/@id|$ObjectTypeAbsorptions/@ref))">
							<oil:entityTypeRef name="{$name}" target="{$oppositeRolePlayerName}" mandatory="{$mandatory}" sourceRoleRef="{$thisRoleId}">
								<xsl:copy-of select="$oppositeOilUniquenessConstraint"/>
								<xsl:copy-of select="$oppositeOilFrequencyConstraint"/>
								<xsl:copy-of select="$thisOilValueConstraint"/>
								<xsl:copy-of select="$oppositeOilValueConstraint"/>
							</oil:entityTypeRef>
						</xsl:when>
						<xsl:otherwise>
							<xsl:message terminate="yes">
								<xsl:text>Opposite role players must be not absorbed, absorbed by us, or absorbed by another top-level type.</xsl:text>
							</xsl:message>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:for-each>
				
			</oil:entityType>
		</oil:model>
	</xsl:template>
	<xsl:template match="orm:ValueType" mode="GenerateEntityTypes">
		<xsl:param name="Model"/>
		<xsl:param name="TopLevelTypes"/>
		<xsl:param name="ObjectTypeInformation"/>
		<xsl:variable name="thisObjectTypeInformation" select="$ObjectTypeInformation[@id=current()/@id]"/>
		<oil:entityType name="{@Name}" sourceRef="{@id}">

		</oil:entityType>
	</xsl:template>
	<xsl:template match="orm:Fact" mode="GenerateEntityTypes">
		<xsl:param name="Model"/>
		<xsl:param name="TopLevelTypes"/>
		<xsl:param name="ObjectTypeInformation"/>
		<xsl:variable name="thisObjectTypeInformation" select="$ObjectTypeInformation[@id=current()/@id]"/>
		<oil:entityType name="{@Name}" sourceRef="{@id}">

		</oil:entityType>
	</xsl:template>

	<xsl:template name="GetTopLevelTypeId">
		<xsl:param name="ObjectTypeAbsorptions"/>
		<xsl:param name="TopLevelTypes"/>
		<xsl:param name="TargetId"/>
		<xsl:choose>
			<xsl:when test="not(string-length($TargetId))"/>
			<xsl:when test="$TopLevelTypes[@id=$TargetId]">
				<xsl:value-of select="$TargetId"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="GetTopLevelTypeId">
					<xsl:with-param name="ObjectTypeAbsorptions" select="$ObjectTypeAbsorptions"/>
					<xsl:with-param name="TopLevelTypes" select="$TopLevelTypes"/>
					<xsl:with-param name="TargetId" select="$ObjectTypeAbsorptions[@ref=$TargetId]/@towards"/>
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>

	</xsl:template>
	<xsl:template name="GetModality">
		<xsl:param name="Target"/>
		<xsl:choose>
			<xsl:when test="$Target/@Modality='Deontic'">
				<xsl:value-of select="'deontic'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="'alethic'"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="GetClusivity">
		<xsl:param name="Inclusion"/>
		<xsl:choose>
			<xsl:when test="$Inclusion='Open'">
				<xsl:value-of select="'exclusive'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="'inclusive'"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="GetOilValueConstraint">
		<xsl:param name="Role"/>
		<xsl:param name="AppliesTo"/>
		<xsl:for-each select="$Role/orm:ValueConstraint/orm:RoleValueRangeDefinition">
			<oil:valueConstraint name="{@Name}" sourceRef="{@id}" appliesTo="{$AppliesTo}">
				<xsl:attribute name="modality">
					<xsl:call-template name="GetModality">
						<xsl:with-param name="Target" select="."/>
					</xsl:call-template>
				</xsl:attribute>
				<xsl:for-each select="orm:ValueRanges/orm:ValueRange">
					<xsl:choose>
						<xsl:when test="@MinValue=@MaxValue">
							<oil:value value="{@MinValue}"/>
						</xsl:when>
						<xsl:otherwise>
							<oil:range>
								<xsl:if test="string-length(@MinValue)">
									<oil:lowerBound value="{@MinValue}">
										<xsl:attribute name="clusivity">
											<xsl:call-template name="GetClusivity">
												<xsl:with-param name="Inclusion" select="@MinInclusion"/>
											</xsl:call-template>
										</xsl:attribute>
									</oil:lowerBound>
								</xsl:if>
								<xsl:if test="string-length(@MaxValue)">
									<oil:upperBound value="{@MaxValue}">
										<xsl:attribute name="clusivity">
											<xsl:call-template name="GetClusivity">
												<xsl:with-param name="Inclusion" select="@MaxInclusion"/>
											</xsl:call-template>
										</xsl:attribute>
									</oil:upperBound>
								</xsl:if>
							</oil:range>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:for-each>
			</oil:valueConstraint>
		</xsl:for-each>
	</xsl:template>
	<xsl:template name="GetOilValueTypes">
		<xsl:param name="Model"/>
		<xsl:param name="RolePlayer"/>
		<xsl:param name="Mandatory"/>
		<xsl:param name="SourceRoleRef"/>
		<xsl:param name="BaseName"/>
		<xsl:choose>
			<xsl:when test="local-name($RolePlayer)='ValueType'">
				<xsl:variable name="conceptualDataType" select="$RolePlayer/orm:ConceptualDataType"/>
				<oil:valueType mandatory="{$Mandatory}" sourceRef="{@id}" sourceRoleRef="{$SourceRoleRef}" dataType="{local-name($Model/orm:DataTypes[@id=$conceptualDataType/@ref])}" scale="{$conceptualDataType/@Scale}" length="{$conceptualDataType/@Length}">
					<xsl:attribute name="name">
						<xsl:choose>
							<xsl:when test="$RolePlayer/@Name=$BaseName">
								<xsl:value-of select="$BaseName"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="concat($BaseName,'_',$RolePlayer/@Name)"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<!-- TODO: Process constraints here, also... -->
				</oil:valueType>
			</xsl:when>
			<xsl:otherwise>
				<!-- For each opposite role player in the primaryIdentifierFactTypes of this object, recursively call ourselves. -->
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
</xsl:stylesheet>