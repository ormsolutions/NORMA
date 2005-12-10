<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:orm="http://schemas.neumont.edu/ORM/ORMCore"
	xmlns:ormRoot="http://schemas.neumont.edu/ORM/ORMRoot"
	xmlns:odt="http://schemas.orm.net/ORMDataTypes"
	xmlns:oil="http://schemas.orm.net/OIAL"
	extension-element-prefixes="msxsl"
	exclude-result-prefixes="orm ormRoot">

	<xsl:output method="xml" encoding="utf-8" media-type="text/xml" indent="yes"/>
	<xsl:strip-space elements="*"/>

	<xsl:variable name="EnableAssertions" select="true()"/>
	<xsl:variable name="OutputDebugInformation" select="false()"/>

	<xsl:template match="ormRoot:ORM2">
		<xsl:apply-templates select="orm:ORMModel"/>
	</xsl:template>
	<xsl:template match="orm:ORMModel">
		<xsl:variable name="Model" select="."/>
		<!-- Any reference to Model after this point should be via the $Model variable. -->
		<!-- It is not guarenteed that the context node of this template will remain Model. -->
		
		<xsl:variable name="objectsAndFacts" select="($Model/orm:Objects|$Model/orm:Facts)/child::*"/>

		<xsl:variable name="ObjectTypeInformationFragment">
			<xsl:for-each select="$Model/orm:Objects/child::*">
				<xsl:copy>
					<xsl:copy-of select="@*"/>
					<xsl:call-template name="GetObjectTypeInformation">
						<xsl:with-param name="Model" select="$Model"/>
					</xsl:call-template>
				</xsl:copy>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="ObjectTypeInformation" select="msxsl:node-set($ObjectTypeInformationFragment)/child::*"/>

		<!-- NOTE: The descriptions below EXCLUDE facts that are part of the preferred reference mode. -->
		<!-- Get independent object types. -->
		<xsl:variable name="IndependentObjectTypes" select="$ObjectTypeInformation[@IsIndependent='true']"/>
		<!-- Get subtypes that are not independent. -->
		<xsl:variable name="NonIndependentSubtypeObjectTypes" select="$ObjectTypeInformation[subtypeMetaFacts/child::* and not(@IsIndependent='true')]"/>
		<!-- Get the non-objectified fact types that have uniqueness constraints spanning more than one role. -->
		<xsl:variable name="NonObjectifiedMultiRoleUniquenessFactTypes" select="($Model/orm:Facts/orm:Fact|$Model/orm:Facts/orm:ImpliedFact)[orm:InternalConstraints/orm:InternalUniquenessConstraint/orm:RoleSequence[count(orm:Role)>1] and not(@id=$Model/orm:Objects/orm:ObjectifiedType/orm:NestedPredicate/@ref)]"/>
		
		<xsl:variable name="FactTypeAbsorptionsFragment">
			<!-- For each binary, one-to-one fact type... -->
			<xsl:for-each select="($Model/orm:Facts/orm:Fact|$Model/orm:Facts/orm:ImpliedFact)[count(orm:FactRoles/orm:Role)=2 and count(orm:InternalConstraints/orm:InternalUniquenessConstraint)=2]">
				<xsl:variable name="countMandatories" select="count(orm:InternalConstraints/orm:SimpleMandatoryConstraint)"/>
				<xsl:variable name="rolePlayerIds" select="orm:FactRoles/orm:Role/orm:RolePlayer/@ref"/>
				<xsl:variable name="rolePlayers" select="$ObjectTypeInformation[@id=$rolePlayerIds]"/>
				<AbsorbFactType ref="{@id}">
					<xsl:choose>
						<!-- If only one role is mandatory... -->
						<xsl:when test="$countMandatories = 1">
							<xsl:variable name="mandatoryRolePlayerId" select="orm:InternalConstraints/orm:SimpleMandatoryConstraint/orm:RoleSequence/orm:Role/@ref"/>
							<xsl:variable name="nonMandatoryRolePlayer" select="$rolePlayers[not(@id=$mandatoryRolePlayerId)]"/>
							<xsl:choose>
								<!-- We know that there is at least one functional role, see if there is another... -->
								<!-- TODO: How do we handle Value Types here? Do we even need to? -->
								<!--<xsl:when test="not(local-name($nonMandatoryRolePlayer)='ValueType') and count($nonMandatoryRolePlayer/functionalNonPreferredIdentifierDirectFacts/child::*) > 1">-->
								<xsl:when test="count($nonMandatoryRolePlayer/functionalNonPreferredIdentifierDirectFacts/child::*) > 1">
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
						<!-- If neither or both roles are mandatory... -->
						<xsl:otherwise>
							<xsl:variable name="firstRolePlayer" select="$rolePlayers[@id=$rolePlayerIds[1]]"/>
							<xsl:variable name="secondRolePlayer" select="$rolePlayers[@id=$rolePlayerIds[2]]"/>
							<xsl:variable name="firstRolePlayerCountNonDependentFunctionalNonPreferredIdentifierDirectFacts" select="count($firstRolePlayer/nonDependentFunctionalNonPreferredIdentifierDirectFacts/child::*)"/>
							<xsl:variable name="secondRolePlayerCountNonDependentFunctionalNonPreferredIdentifierDirectFacts" select="count($secondRolePlayer/nonDependentFunctionalNonPreferredIdentifierDirectFacts/child::*)"/>
							<xsl:attribute name="towards">
								<xsl:choose>
									<!-- TODO: How do we handle Value Types here? Do we even need to? -->
									<!--<xsl:when test="not(local-name($firstRolePlayer)='ValueType') and $firstRolePlayerCountNonDependentFunctionalNonPreferredIdentifierDirectFacts >= $secondRolePlayerCountNonDependentFunctionalNonPreferredIdentifierDirectFacts">-->
									<xsl:when test="$firstRolePlayerCountNonDependentFunctionalNonPreferredIdentifierDirectFacts >= $secondRolePlayerCountNonDependentFunctionalNonPreferredIdentifierDirectFacts">
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
		
		<oil:model name="{$Model/@Name}" sourceRef="{$Model/@id}">
			
			<xsl:if test="$OutputDebugInformation">
				<DEBUG_INFORMATION>
					<ObjectTypeInformation>
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
					</TopLevelTypes>
				</DEBUG_INFORMATION>
			</xsl:if>

			<oil:informationTypeFormats>
				<!-- TODO: UNDONE: This template should spit oil:informationTypeFormat elements for each orm:ValueType. Since the entire existence and meaning of orm:ValueType is currently in flux, this template currently just serves as a place holder. -->
				<!-- TODO: UNDONE: We now only have two weeks before we have to demo this, so (the above statement * 10). -->
				<xsl:comment>These may change in the future once they are integrated into the core ORM model file.</xsl:comment>
				<xsl:apply-templates select="$Model/orm:Objects/orm:ValueType" mode="GenerateInformationTypeFormats">
					<xsl:with-param name="Model" select="$Model"/>
				</xsl:apply-templates>
			</oil:informationTypeFormats>
			
			<xsl:apply-templates select="$TopLevelTypes" mode="GenerateConceptTypes">
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

		<xsl:if test="$EnableAssertions and not(self::orm:EntityType or self::orm:ObjectifiedType or self::orm:ValueType)">
			<xsl:message terminate="yes">
				<xsl:text>This template has only been designed to work with EntityType and ObjectifiedType elements. It kind of works for ValueType elements as well.</xsl:text>
			</xsl:message>
		</xsl:if>

		<!-- Any subtype meta facts where this object type is the subtype. -->
		<xsl:variable name="subtypeMetaFacts" select="$Model/orm:Facts/orm:SubtypeFact[orm:FactRoles/orm:Role[1]/orm:RolePlayer/@ref=current()/@id]"/>

		<!-- Any subtype meta facts where this object type is the supertype. -->
		<xsl:variable name="supertypeMetaFacts" select="$Model/orm:Facts/orm:SubtypeFact[orm:FactRoles/orm:Role[2]/orm:RolePlayer/@ref=current()/@id]"/>

		<!-- TODO: Filter out roles that are in objectified fact types, since the implied fact types will take care of them. -->
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

	<xsl:template match="orm:ValueType" mode="GenerateInformationTypeFormats">
		<xsl:param name="Model"/>
		<xsl:variable name="dataTypeName" select="@Name"/>
		<xsl:variable name="modelConceptualDataType" select="orm:ConceptualDataType"/>
		<xsl:variable name="modelDataType" select="$Model/orm:DataTypes/child::*[@id=$modelConceptualDataType/@ref]"/>
		<xsl:variable name="modelValueRanges" select="orm:ValueConstraint/orm:ValueRangeDefinition/orm:ValueRanges/orm:ValueRange"/>
		<xsl:variable name="length" select="$modelConceptualDataType/@Length"/>
		<xsl:variable name="scale" select="$modelConceptualDataType/@Scale"/>

		<xsl:choose>
			<xsl:when test="$modelDataType/self::orm:AutoCounterNumericDataType or $modelDataType/self::orm:RowIdOtherDataType">
				<odt:identity name="{$dataTypeName}"/>
			</xsl:when>
			<xsl:when test="$modelDataType/self::orm:TrueOrFalseLogicalDataType or $modelDataType/self::orm:YesOrNoLogicalDataType">
				<odt:boolean name="{$dataTypeName}">
					<xsl:if test="$modelValueRanges">
						<xsl:attribute name="fixed">
							<!-- This is a boolean, so there will only ever be at most one ValueRange for it, and @MinValue will always match @MaxValue -->
							<xsl:value-of select="$modelValueRanges/@MinValue"/>
						</xsl:attribute>
					</xsl:if>
				</odt:boolean>
			</xsl:when>
			<xsl:when test="$modelDataType/self::orm:SignedIntegerNumericDataType or $modelDataType/self::orm:UnsignedIntegerNumericDataType or $modelDataType/self::orm:DecimalNumericDataType or $modelDataType/self::orm:MoneyNumericDataType">
				<odt:decimalNumber name="{$dataTypeName}">
					<xsl:if test="$length > 0">
						<xsl:attribute name="totalDigits">
							<xsl:value-of select="$length"/>
						</xsl:attribute>
					</xsl:if>
					<xsl:choose>
						<xsl:when test="$modelDataType/self::orm:SignedIntegerNumericDataType or $modelDataType/self::orm:UnsignedIntegerNumericDataType">
							<xsl:attribute name="fractionDigits">
								<xsl:value-of select="0"/>
							</xsl:attribute>
						</xsl:when>
						<xsl:when test="$scale > 0">
							<xsl:attribute name="fractionDigits">
								<xsl:value-of select="$scale"/>
							</xsl:attribute>
						</xsl:when>
					</xsl:choose>
					<xsl:apply-templates select="$modelValueRanges" mode="ProcessOrmValueRange"/>
				</odt:decimalNumber>
			</xsl:when>
			<xsl:when test="$modelDataType/self::orm:FloatingPointNumericDataType">
				<!-- TODO: Is the precision the $scale or the $length? -->
				<odt:floatingPointNumber name="{$dataTypeName}" precision="{$scale}">
					<xsl:apply-templates select="$modelValueRanges" mode="ProcessOrmValueRange"/>
				</odt:floatingPointNumber>
			</xsl:when>
			<xsl:when test="$modelDataType/self::orm:FixedLengthTextDataType or $modelDataType/self::orm:VariableLengthTextDataType or $modelDataType/self::orm:LargeLengthTextDataType">
				<odt:string name="{$dataTypeName}">
					<xsl:if test="$length > 0">
						<xsl:attribute name="maxLength">
							<xsl:value-of select="$length"/>
						</xsl:attribute>
						<xsl:if test="$modelDataType/self::orm:FixedLengthTextDataType">
							<xsl:attribute name="minLength">
								<xsl:value-of select="$length"/>
							</xsl:attribute>
						</xsl:if>
					</xsl:if>
					<xsl:for-each select="$modelValueRanges">
						<xsl:choose>
							<xsl:when test="@MinValue=@MaxValue">
								<odt:enumeration value="{@MinValue}"/>
							</xsl:when>
							<xsl:otherwise>
								<odt:pattern>
									<xsl:attribute name="value">
										<xsl:text>[</xsl:text>
										<xsl:value-of select="@MinValue"/>
										<xsl:text>-</xsl:text>
										<xsl:value-of select="@MaxValue"/>
										<xsl:text>]</xsl:text>
									</xsl:attribute>
								</odt:pattern>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:for-each>
				</odt:string>
			</xsl:when>
			<xsl:when test="$modelDataType/self::orm:FixedLengthRawDataDataType or $modelDataType/self::orm:VariableLengthRawDataDataType or $modelDataType/self::orm:LargeLengthRawDataDataType or $modelDataType/self::orm:PictureRawDataDataType or $modelDataType/self::orm:OleObjectRawDataDataType">
				<odt:binary name="{$dataTypeName}" maxLength="{$length}">
					<xsl:if test="$length > 0">
						<xsl:attribute name="maxLength">
							<xsl:value-of select="$length"/>
						</xsl:attribute>
						<xsl:if test="$modelDataType/self::orm:FixedLengthRawDataDataType">
							<xsl:attribute name="minLength">
								<xsl:value-of select="$length"/>
							</xsl:attribute>
						</xsl:if>
					</xsl:if>
				</odt:binary>
			</xsl:when>
			<xsl:otherwise>
				<xsl:comment>
					<xsl:text>WARNING: We currently don't support the data type '</xsl:text>
					<xsl:value-of select="local-name($modelDataType)"/>
					<xsl:text>' that was chosen for value type "</xsl:text>
					<xsl:value-of select="$dataTypeName"/>
					<xsl:text>"</xsl:text>
				</xsl:comment>
			</xsl:otherwise>
		</xsl:choose>
		
	</xsl:template>
	
	
	<xsl:template match="orm:EntityType | orm:ValueType | orm:ObjectifiedType" mode="GenerateConceptTypes">
		<xsl:param name="Model"/>
		<xsl:param name="ObjectTypeInformation"/>
		<xsl:param name="FactTypeAbsorptions"/>
		<xsl:param name="ObjectTypeAbsorptions"/>
		<xsl:param name="TopLevelTypes"/>
		<xsl:param name="Mandatory"/>
		<xsl:param name="SourceRoleRef"/>
		<xsl:param name="OilConstraintsFromParent"/>
		<xsl:variable name="thisObjectTypeId" select="@id"/>
		<xsl:variable name="thisObjectTypeName" select="@Name"/>
		<xsl:variable name="thisObjectTypeInformation" select="."/>
		<oil:conceptType name="{$thisObjectTypeName}" sourceRef="{$thisObjectTypeId}">
			<xsl:if test="string-length($Mandatory)">
				<xsl:attribute name="mandatory">
					<xsl:value-of select="$Mandatory"/>
				</xsl:attribute>
			</xsl:if>
			<xsl:if test="string-length($SourceRoleRef)">
				<xsl:attribute name="sourceRoleRef">
					<xsl:value-of select="$SourceRoleRef"/>
				</xsl:attribute>
			</xsl:if>

			<xsl:if test="self::orm:ValueType">
				<oil:informationType name="{concat($thisObjectTypeName,'Value')}" mandatory="alethic" sourceRef="{$thisObjectTypeId}" formatRef="{$thisObjectTypeName}">
					<oil:singleRoleUniquenessConstraint name="{concat($thisObjectTypeName,'Value_Unique')}" modality="alethic" sourceRef="{$thisObjectTypeId}" isPrimary="true"/>
				</oil:informationType>
			</xsl:if>

			<!-- Process all functional direct facts that are not absorbed away from us. -->
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
					<xsl:variable name="simpleMandatoryConstraint" select="orm:InternalConstraints/orm:SimpleMandatoryConstraint[orm:RoleSequence/orm:Role/@ref=$thisRole/@id]"/>
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
								<xsl:value-of select="$thisObjectTypeInformation/preferredIdentifier/orm:InternalUniquenessConstraint/@id = $uniquenessConstraint/@id"/>
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
				<xsl:variable name="oilConstraintsFragment">
					<xsl:copy-of select="$oppositeOilUniquenessConstraint"/>
					<xsl:copy-of select="$oppositeOilFrequencyConstraint"/>
					<xsl:copy-of select="$thisOilValueConstraint"/>
					<xsl:copy-of select="$oppositeOilValueConstraint"/>
				</xsl:variable>
				<!-- HACK: This node-set() call doesn't strictly need to be here, but if it is not, the output formatting done by the processor gets screwed up. -->
				<xsl:variable name="oilConstraints" select="msxsl:node-set($oilConstraintsFragment)/child::*"/>
				<xsl:choose>
					<xsl:when test="not(string-length($oppositeRolePlayerTopLevelTypeId))">
						<xsl:call-template name="GetOilInformationTypes">
							<xsl:with-param name="Model" select="$Model"/>
							<xsl:with-param name="ObjectTypeInformation" select="$ObjectTypeInformation"/>
							<xsl:with-param name="ContainingConceptTypeName" select="$thisObjectTypeName"/>
							<xsl:with-param name="RolePlayer" select="$oppositeRolePlayer"/>
							<xsl:with-param name="Mandatory" select="$mandatory"/>
							<xsl:with-param name="SourceRoleRef" select="$thisRoleId"/>
							<xsl:with-param name="BaseName" select="$name"/>
							<xsl:with-param name="OilConstraints" select="$oilConstraints"/>
						</xsl:call-template>
					</xsl:when>
					<xsl:when test="$oppositeRolePlayerTopLevelTypeId = $thisObjectTypeId">
						<!-- TODO: Absorbed Entity Types still need some testing... -->
						<!-- TODO: The absorbed type shouldn't contain an entry for the fact through which it is absorbed... Fixing the need-to-exclude-objectified-facts bug should fix this also... -->
						<xsl:apply-templates select="$oppositeRolePlayer" mode="GenerateConceptTypes">
							<xsl:with-param name="Model" select="$Model"/>
							<xsl:with-param name="ObjectTypeInformation" select="$ObjectTypeInformation"/>
							<xsl:with-param name="FactTypeAbsorptions" select="$FactTypeAbsorptions"/>
							<xsl:with-param name="ObjectTypeAbsorptions" select="$ObjectTypeAbsorptions"/>
							<xsl:with-param name="TopLevelTypes" select="$TopLevelTypes"/>
							<xsl:with-param name="Mandatory" select="$mandatory"/>
							<xsl:with-param name="SourceRoleRef" select="$thisRoleId"/>
							<xsl:with-param name="OilConstraintsFromParent" select="$oilConstraints"/>
						</xsl:apply-templates>
					</xsl:when>
					<xsl:when test="not($EnableAssertions) or ($oppositeRolePlayerTopLevelTypeId=($TopLevelTypes/@id|$ObjectTypeAbsorptions/@ref))">
						<oil:conceptTypeRef name="{$name}" target="{$oppositeRolePlayerName}" mandatory="{$mandatory}" sourceRoleRef="{$thisRoleId}">
							<xsl:copy-of select="$oilConstraints"/>
						</oil:conceptTypeRef>
					</xsl:when>
					<xsl:otherwise>
						<xsl:message terminate="yes">
							<xsl:text>SANITY CHECK: Opposite role players must be not absorbed, absorbed by us, or absorbed by another top-level type.</xsl:text>
						</xsl:message>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:for-each>

			<!-- HACK: This node-set() call doesn't strictly need to be here, but if it is not, the output formatting done by the processor gets screwed up. -->
			<!-- This copy-of needs to be the last thing to put child elements into this conceptType. -->
			<xsl:copy-of select="msxsl:node-set($OilConstraintsFromParent)/child::*"/>

		</oil:conceptType>
	</xsl:template>
	<xsl:template match="orm:Fact" mode="GenerateConceptTypes">
		<xsl:param name="Model"/>
		<xsl:param name="TopLevelTypes"/>
		<xsl:param name="ObjectTypeInformation"/>
		<xsl:variable name="thisObjectTypeInformation" select="$ObjectTypeInformation[@id=current()/@id]"/>
		<oil:conceptType name="{@Name}" sourceRef="{@id}">

		</oil:conceptType>
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
				<xsl:apply-templates select="orm:ValueRanges/orm:ValueRange" mode="ProcessOrmValueRange"/>
			</oil:valueConstraint>
		</xsl:for-each>
	</xsl:template>
	<xsl:template match="orm:ValueRange" mode="ProcessOrmValueRange">
		<xsl:choose>
			<xsl:when test="@MinValue=@MaxValue">
				<odt:enumeration value="{@MinValue}"/>
			</xsl:when>
			<xsl:otherwise>
				<odt:range>
					<xsl:if test="string-length(@MinValue)">
						<odt:lowerBound value="{@MinValue}">
							<xsl:attribute name="clusivity">
								<xsl:call-template name="GetClusivity">
									<xsl:with-param name="Inclusion" select="@MinInclusion"/>
								</xsl:call-template>
							</xsl:attribute>
						</odt:lowerBound>
					</xsl:if>
					<xsl:if test="string-length(@MaxValue)">
						<odt:upperBound value="{@MaxValue}">
							<xsl:attribute name="clusivity">
								<xsl:call-template name="GetClusivity">
									<xsl:with-param name="Inclusion" select="@MaxInclusion"/>
								</xsl:call-template>
							</xsl:attribute>
						</odt:upperBound>
					</xsl:if>
				</odt:range>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template name="GetOilInformationTypes">
		<xsl:param name="Model"/>
		<xsl:param name="ObjectTypeInformation"/>
		<xsl:param name="ContainingConceptTypeName"/>
		<xsl:param name="RolePlayer"/>
		<xsl:param name="Mandatory"/>
		<xsl:param name="SourceRoleRef"/>
		<xsl:param name="BaseName"/>
		<xsl:param name="OilConstraints"/>
		<xsl:variable name="oilInformationTypesFragment">
			<xsl:call-template name="GetOilInformationTypesInternal">
				<xsl:with-param name="Model" select="$Model"/>
				<xsl:with-param name="ObjectTypeInformation" select="$ObjectTypeInformation"/>
				<xsl:with-param name="RolePlayer" select="$RolePlayer"/>
				<xsl:with-param name="Mandatory" select="$Mandatory"/>
				<xsl:with-param name="SourceRoleRef" select="$SourceRoleRef"/>
				<xsl:with-param name="BaseName" select="$BaseName"/>
				<xsl:with-param name="OilConstraints" select="$OilConstraints"/>
			</xsl:call-template>
		</xsl:variable>
		<xsl:variable name="oilInformationTypes" select="msxsl:node-set($oilInformationTypesFragment)/child::*"/>
		<xsl:copy-of select="$oilInformationTypes"/>
		<!-- Check if there is more than one informationType. If there is, and they aren't all alethicly mandatory, we need to spit an equalityConstraint for them. -->
		<xsl:if test="count($oilInformationTypes)>1 and $oilInformationTypes[not(@mandatory='alethic')]">
			<oil:equalityConstraint name="{concat($BaseName,'_Equality')}" modality="alethic" sourceRef="{$RolePlayer/@id}">
				<xsl:for-each select="$oilInformationTypes">
					<oil:roleSequence>
						<oil:typeRef conceptTypeTarget="{$ContainingConceptTypeName}" informationTypeTarget="{@name}"/>
					</oil:roleSequence>
				</xsl:for-each>
			</oil:equalityConstraint>
		</xsl:if>
	</xsl:template>
	<xsl:template name="GetOilInformationTypesInternal">
		<xsl:param name="Model"/>
		<xsl:param name="ObjectTypeInformation"/>
		<xsl:param name="RolePlayer"/>
		<xsl:param name="Mandatory"/>
		<xsl:param name="SourceRoleRef"/>
		<xsl:param name="BaseName"/>
		<xsl:param name="OilConstraints"/>
		<xsl:param name="IsFirst" select="true()"/>
		<xsl:choose>
			<xsl:when test="local-name($RolePlayer)='ValueType'">
				<xsl:variable name="name">
					<xsl:choose>
						<xsl:when test="$IsFirst">
							<xsl:value-of select="$BaseName"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="concat($BaseName,'_',$RolePlayer/@Name)"/>
						</xsl:otherwise>
					</xsl:choose>					
				</xsl:variable>
				<oil:informationType name="{$name}" formatRef="{$RolePlayer/@Name}" mandatory="{$Mandatory}" sourceRef="{@id}" sourceRoleRef="{$SourceRoleRef}">
					<xsl:copy-of select="$OilConstraints"/>
					<!-- TODO: Process other constraints here, also... -->
				</oil:informationType>
			</xsl:when>
			<xsl:otherwise>
				<xsl:for-each select="$RolePlayer/preferredIdentifierFacts/child::*">
					<xsl:variable name="newBaseName">
						<xsl:choose>
							<xsl:when test="$IsFirst">
								<xsl:value-of select="$BaseName"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="concat($BaseName,'_',$RolePlayer/@Name)"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<xsl:variable name="oppositeRolePlayerId" select="orm:FactRoles/orm:Role[not(orm:RolePlayer/@ref=$RolePlayer/@id)]/orm:RolePlayer/@ref"/>
					<xsl:variable name="oppositeRolePlayer" select="$ObjectTypeInformation[@id=$oppositeRolePlayerId]"/>
					<xsl:call-template name="GetOilInformationTypesInternal">
						<xsl:with-param name="Model" select="$Model"/>
						<xsl:with-param name="ObjectTypeInformation" select="$ObjectTypeInformation"/>
						<xsl:with-param name="RolePlayer" select="$oppositeRolePlayer"/>
						<xsl:with-param name="Mandatory" select="$Mandatory"/>
						<xsl:with-param name="SourceRoleRef" select="$SourceRoleRef"/>
						<xsl:with-param name="BaseName" select="$newBaseName"/>
						<xsl:with-param name="OilConstraints" select="$OilConstraints"/>
						<xsl:with-param name="IsFirst" select="false()"/>
					</xsl:call-template>
					<!-- TODO: Process constraints here, also... -->
				</xsl:for-each>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
</xsl:stylesheet>