<?xml version="1.0" encoding="utf-8"?>
<!--
	Neumont Object-Role Modeling Architect for Visual Studio

	Copyright © Neumont University. All rights reserved.
	Copyright © ORM Solutions, LLC. All rights reserved.

	The use and distribution terms for this software are covered by the
	Common Public License 1.0 (http://opensource.org/licenses/cpl) which
	can be found in the file CPL.txt at the root of this distribution.
	By using this software in any fashion, you are agreeing to be bound by
	the terms of this license.

	You must not remove this notice, or any other, from this software.
-->
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:orm="http://schemas.neumont.edu/ORM/2006-04/ORMCore"
	xmlns:exsl="http://exslt.org/common"
	xmlns:loc="urn:local-temps"
	xmlns:xs="http://www.w3.org/2001/XMLSchema"
	extension-element-prefixes="exsl"
	exclude-result-prefixes="loc xs">

	<xsl:output method="xml" encoding="utf-8" media-type="text/xml" indent="yes"/>
	<xsl:strip-space elements="*"/>

	<!-- Internal data structure -->
	<xs:schema targetNamespace="urn:local-temps" xmlns="urn:local-temps" elementFormDefault="qualified">
		<xs:element name="roleMaps">
			<xs:complexType>
				<xs:sequence>
					<xs:element name="map" type="mappedRoleType" minOccurs="0" maxOccurs="unbounded"/>
				</xs:sequence>
			</xs:complexType>
		</xs:element>
		<xs:element name="constraintMaps">
			<xs:complexType>
				<xs:sequence>
					<xs:element name="constraint" type="mappedConstraintType" minOccurs="0" maxOccurs="unbounded"/>
				</xs:sequence>
			</xs:complexType>
		</xs:element>
		<xs:complexType name="mappedRoleType">
			<xs:annotation>
				<xs:documentation>Indicates that a role has been deleted and any reference to it needs to map to another role.</xs:documentation>
			</xs:annotation>
			<xs:attribute name="fromRole" type="xs:NMTOKEN"/>
			<xs:attribute name="toRole" type="xs:NMTOKEN" use="optional"/>
			<xs:attribute name="paired" type="xs:boolean" use="optional">
				<xs:annotation>
					<xs:documentation>toRole is a positive unary role that fromRole has been paired with. Single-role constraints are removed and multi-role constraints eliminate the reference.</xs:documentation>
				</xs:annotation>
			</xs:attribute>
			<xs:attribute name="unaryValueRole" type="xs:boolean" use="optional">
				<xs:annotation>
					<xs:documentation>fromRole is a unary role and this is the id for the injected unary value role. If toRole is not set then this is not a replaced role. Use for uniqueness and frequency constraint redirection to the injected role.</xs:documentation>
				</xs:annotation>
			</xs:attribute>
		</xs:complexType>
		<xs:complexType name="mappedConstraintType">
			<xs:annotation>
				<xs:documentation>Indicates that a constraint as a whole should be removed or modified.</xs:documentation>
			</xs:annotation>
			<xs:attribute name="constraint" type="xs:NMTOKEN"/>
			<xs:attribute name="remove" type="xs:boolean" use="optional"/>
			<xs:attribute name="simple" type="xs:boolean" use="optional"/>
		</xs:complexType>
	</xs:schema>

	<xsl:param name="CoRefImpliedMandatoryDecorator" select="'_implied_mandatory'"/>
	<xsl:param name="CoRefUnaryValueRoleDecorator" select="'_unary_value_role'"/>
	<xsl:param name="CoRefUnaryValueTypeDecorator" select="'_unary_value'"/>
	<xsl:param name="CoRefUnaryValueConstraintDecorator" select="'_unary_value_constraint'"/>
	<xsl:param name="CoRefUnaryValueRangeDecorator" select="'_unary_value_range'"/>
	<xsl:param name="CoRefValueDataTypeIdDecorator" select="'_logical_data_type'"/>

	<xsl:key name="KeyedObjectTypes" match="*/orm:ORMModel/orm:Objects/orm:*" use="@id"/>
	<xsl:key name="KeyedFactTypes" match="*/orm:ORMModel/orm:Facts/orm:*" use="@id"/>
	<xsl:key name="KeyedObjectifiedTypesByPredicateId" match="*/orm:ORMModel/orm:Objects/orm:ObjectifiedType" use="orm:NestedPredicate/@ref"/>
	<xsl:key name="KeyedRoleMap" match="loc:roleMaps/loc:map" use="@fromRole"/>
	<xsl:key name="KeyedConstraintMap" match="loc:constraintMaps/loc:map" use="@constraint"/>

	<xsl:variable name="ORMModel" select="*/orm:ORMModel"/>
	<xsl:variable name="ExistingTrueOrFalseLogicalDataType" select="$ORMModel/orm:DataTypes/orm:TrueOrFalseLogicalDataType"/>
	<xsl:variable name="CoRefLogicalDataTypeIdDecoratorFragment">
		<xsl:choose>
			<xsl:when test="$ExistingTrueOrFalseLogicalDataType">
				<xsl:value-of select="$ExistingTrueOrFalseLogicalDataType/@id"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="'_true_or_false_logical_Data_Type'"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="CoRefLogicalDataTypeIdDecorator" select="string($CoRefLogicalDataTypeIdDecoratorFragment)"/>

	<!-- Cache information about proxy and unary roles up front. Objectified fact types are
	eliminated, with the objectified roles injected in the link fact types. Unary fact types
	are expanded to binaries with boolean values, with negations folded into the positive unary
	fact types (unless either unary is objectified) -->
	<xsl:variable name="UnobjectifiedUnaryFactTypes" select="$ORMModel/orm:Facts/orm:Fact[not(key('KeyedObjectifiedTypesByPredicateId',@id)) and count(orm:FactRoles/orm:Role)=1]"/>
	<xsl:variable name="UnaryNegations" select="$UnobjectifiedUnaryFactTypes[@UnaryPattern='Negation']"/>
	<xsl:variable name="PairedUnaryNegations" select="$UnaryNegations[@id=$UnobjectifiedUnaryFactTypes/orm:NegatedByUnaryFactType/@ref]"/>
	<!-- Note that negation ids is not expected to be a large set, so they are not currently indexed. -->
	<xsl:variable name="PairedUnaryNegationIds" select="$PairedUnaryNegations/@id"/>
	<xsl:variable name="ExpandedUnaries" select="$UnobjectifiedUnaryFactTypes[not(@UnaryPattern='Negation') or not(@id=$PairedUnaryNegationIds)]"/>
	<xsl:variable name="MapFragment">
		<xsl:variable name="PairedPositiveUnaries" select="$ExpandedUnaries[not(@UnaryPattern='Negation') and not(starts-with(@UnaryPattern,'OptionalWithoutNegation')) and orm:NegatedByUnaryFactType/@ref=$PairedUnaryNegationIds]"/>
		<loc:roleMaps>
			<xsl:for-each select="$ORMModel/orm:Facts/orm:ImpliedFact/orm:FactRoles/orm:RoleProxy">
				<loc:map fromRole="{@id}" toRole="{orm:Role/@ref}"/>
			</xsl:for-each>
			<xsl:for-each select="$ExpandedUnaries">
				<loc:map fromRole="{orm:FactRoles/orm:Role/@id}" unaryValueRole="{@id}{$CoRefUnaryValueRoleDecorator}"/>
			</xsl:for-each>
			<xsl:for-each select="$PairedPositiveUnaries">
				<loc:map fromRole="{key('KeyedFactTypes',orm:NegatedByUnaryFactType/@ref)/orm:FactRoles/orm:Role/@id}" toRole="{orm:FactRoles/orm:Role/@id}" paired="1" unaryValueRole="{@id}{$CoRefUnaryValueRoleDecorator}"/>
			</xsl:for-each>
		</loc:roleMaps>
		<loc:constraintMaps>
			<xsl:for-each select="$PairedPositiveUnaries/orm:UnaryNegationMandatoryConstraint">
				<loc:map constraint="{@ref}" simple="1"/>
			</xsl:for-each>
			<xsl:for-each select="$PairedUnaryNegations/orm:InternalConstraints/orm:UniquenessConstraint">
				<loc:map constraint="{@ref}" remove="1"/>
			</xsl:for-each>
			<xsl:for-each select="$PairedPositiveUnaries/orm:UnaryNegationExclusionConstraint">
				<loc:map constraint="{@ref}" remove="1"/>
			</xsl:for-each>
		</loc:constraintMaps>
	</xsl:variable>
	<xsl:variable name="MapDoc" select="exsl:node-set($MapFragment)"/>

	<xsl:template match="/">
		<xsl:apply-templates select="*/orm:ORMModel" mode="CoRefORM"/>
	</xsl:template>

	<xsl:template match="orm:ORMModel" mode="CoRefORM">
		<xsl:call-template name="DefaultCoRefORMTemplate"/>
	</xsl:template>
	<xsl:template match="orm:ImpliedFact" mode="CoRefORM">
		<orm:Fact>
			<xsl:apply-templates select="node()|@*" mode="CoRefORM"/>
		</orm:Fact>
	</xsl:template>
	<xsl:template match="orm:ImpliedByObjectification | orm:NestedPredicate" mode="CoRefORM"/>
	<xsl:template match="orm:ObjectifiedType" mode="CoRefORM">
		<orm:EntityType>
			<xsl:apply-templates select="node()|@*" mode="CoRefORM"/>
		</orm:EntityType>
	</xsl:template>
	<xsl:template match="*|@*|text()" name="DefaultCoRefORMTemplate" mode="CoRefORM">
		<xsl:copy>
			<xsl:apply-templates select="node()|@*" mode="CoRefORM"/>
		</xsl:copy>
	</xsl:template>
	<xsl:template match="orm:PlayedRoles/orm:Role" mode="CoRefORM">
		<xsl:variable name="self" select="."/>
		<xsl:for-each select="$MapDoc">
			<xsl:variable name="roleMap" select="key('KeyedRoleMap',$self/@ref)"/>
			<xsl:if test="not($roleMap/@toRole)">
				<xsl:for-each select="$self">
					<xsl:call-template name="DefaultCoRefORMTemplate"/>
				</xsl:for-each>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>
	<xsl:template match="orm:RoleProxy" mode="CoRefORM">
		<xsl:variable name="proxiedRole" select="../../../orm:Fact/child::orm:FactRoles/child::orm:Role[@id=current()/orm:Role/@ref]"/>
		<orm:Role _Multiplicity="ExactlyOne">
			<xsl:copy-of select="$proxiedRole/@*[not(local-name()='_Multiplicity')]|$proxiedRole/orm:*[not(self::orm:RoleInstances)]"/>
		</orm:Role>
	</xsl:template>
	<xsl:template match="orm:Role/@ref | orm:ImpliedFact/orm:FactRoles/orm:Role/@id" mode="CoRefORM">
		<xsl:variable name="self" select="."/>
		<xsl:for-each select="$MapDoc">
			<xsl:variable name="map" select="key('KeyedRoleMap',$self)[@toRole]"/>
			<xsl:choose>
				<xsl:when test="$map">
					<xsl:attribute name="{local-name($self)}">
						<xsl:value-of select="$map/@toRole"/>
					</xsl:attribute>
				</xsl:when>
				<xsl:otherwise>
					<xsl:copy-of select="$self"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:for-each>
	</xsl:template>
	<xsl:template name="GetUnaryFactTypeRoleName">
		<xsl:param name="UnaryFactType"/>
		<xsl:variable name="roleName" select="string($UnaryFactType/orm:FactRoles/orm:*/@Name)"/>
		<xsl:choose>
			<xsl:when test="$roleName">
				<xsl:value-of select="$roleName"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:variable name="reading" select="translate(normalize-space($UnaryFactType/orm:ReadingOrders/orm:ReadingOrder[1]/orm:Readings/orm:Reading[1]/orm:Data/text()),' ','')"/>
				<xsl:value-of select="concat(substring-before($reading,'{0}'),substring-after($reading,'{0}'))"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="orm:Facts/orm:Fact" mode="CoRefORM">
		<xsl:variable name="factTypeId" select="@id"/>
		<xsl:choose>
			<xsl:when test="key('KeyedObjectifiedTypesByPredicateId',$factTypeId) or $PairedUnaryNegationIds=$factTypeId"/>
			<xsl:when test="$ExpandedUnaries[@id=$factTypeId]">
				<xsl:variable name="unaryPattern" select="string(@UnaryPattern)"/>
				<xsl:variable name="paired" select="not($unaryPattern='Negation') and not(starts-with($unaryPattern,'OptionalWithoutNegation')) and orm:NegatedByUnaryFactType/@ref=$PairedUnaryNegationIds"/>
				<!-- Using contains instead of starts-with picks up deontic constraints as well -->
				<xsl:variable name="isMandatory" select="$paired and contains($unaryPattern,'RequiredWithNegation')"/>
				<xsl:variable name="factType" select="."/>
				<xsl:copy>
					<xsl:copy-of select="@*[not(local-name()='UnaryPattern')]"/>
					<!-- Note that notes are not included -->
					<xsl:copy-of select="orm:Definitions"/>
					<orm:FactRoles>
						<xsl:for-each select="orm:FactRoles/*">
							<xsl:copy>
								<xsl:copy-of select="@*[not(local-name()='_Multiplicity')]"/>
								<xsl:attribute name="_Multiplicity">
									<!-- This is ExactlyOne if we inject the implied mandatory on the
									value type as an internal constraint. We leave it implied to stay
									away from the balanced 1-1 double mandatory, which is expensive to
									process. -->
									<xsl:text>ZeroToOne</xsl:text>
								</xsl:attribute>
								<xsl:copy-of select="child::*[not(self::orm:RoleInstances)]"/>
							</xsl:copy>
						</xsl:for-each>
						<orm:Role id="{$factTypeId}{$CoRefUnaryValueRoleDecorator}">
							<xsl:attribute name="Name">
								<xsl:call-template name="GetUnaryFactTypeRoleName">
									<xsl:with-param name="UnaryFactType" select="$factType"/>
								</xsl:call-template>
							</xsl:attribute>
							<xsl:attribute name="_Multiplicity">
								<xsl:choose>
									<xsl:when test="$isMandatory and starts-with($unaryPattern,'RequiredWithNegation')">
										<xsl:text>ExactlyOne</xsl:text>
									</xsl:when>
									<xsl:otherwise>
										<xsl:text>ZeroToOne</xsl:text>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
							<orm:RolePlayer ref="{$factTypeId}{$CoRefUnaryValueTypeDecorator}"/>
						</orm:Role>
						<!-- We don't try to rebuild the readings. The default reading is incorporated into the far role name.  -->
					</orm:FactRoles>
					<orm:InternalConstraints>
						<xsl:copy-of select="orm:InternalConstraints/*"/>
						<xsl:if test="$isMandatory">
							<xsl:variable name="mandatoryId" select="orm:UnaryNegationMandatoryConstraint/@ref"/>
							<xsl:if test="$mandatoryId">
								<orm:MandatoryConstraint ref="{$mandatoryId}" />
							</xsl:if>
						</xsl:if>
					</orm:InternalConstraints>
					<!-- Do not copy orm:Instances -->
					<xsl:copy-of select="orm:DerivationRule|orm:Extensions"/>
				</xsl:copy>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="DefaultCoRefORMTemplate"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="orm:Constraints" mode="CoRefORM">
		<xsl:copy>
			<xsl:apply-templates select="node()|@*" mode="CoRefORM"/>
			<xsl:for-each select="$ExpandedUnaries">
				<!-- The implied unary value type plays one role, which makes it implicitly mandatory.
				Allow downstream algorithms to see this as an implied instead of normal mandatory -->
				<xsl:variable name="valueTypeNameFragment">
					<xsl:value-of select="key('KeyedObjectTypes',orm:FactRoles/orm:Role/orm:RolePlayer/@ref)/@Name"/>
					<xsl:text>_</xsl:text>
					<xsl:call-template name="GetUnaryFactTypeRoleName">
						<xsl:with-param name="UnaryFactType" select="."/>
					</xsl:call-template>
				</xsl:variable>
				<orm:MandatoryConstraint id="{@id}{$CoRefImpliedMandatoryDecorator}" Name="{string($valueTypeNameFragment)}{$CoRefImpliedMandatoryDecorator}" IsImplied="true">
					<orm:RoleSequence>
						<orm:Role ref="{@id}{$CoRefUnaryValueRoleDecorator}"/>
					</orm:RoleSequence>
					<orm:ImpliedByObjectType ref="{@id}{$CoRefUnaryValueTypeDecorator}"/>
				</orm:MandatoryConstraint>
			</xsl:for-each>
		</xsl:copy>
	</xsl:template>
	<xsl:template match="orm:UniquenessConstraint[count(orm:RoleSequence/child::*)>1 and @IsInternal='true']" mode="CoRefORM">
		<!-- After binarization, no internal uniqueness constraint will have more than one role, so remove the @IsInternal attribute. -->
		<xsl:copy>
			<xsl:apply-templates select="@*[not(local-name()='IsInternal')]|node()"/>
		</xsl:copy>
	</xsl:template>
	<xsl:template match="orm:Constraints/orm:*" mode="CoRefORM">
		<xsl:variable name="self" select="."/>
		<xsl:for-each select="$MapDoc">
			<xsl:variable name="map" select="key('KeyedConstraintMap',$self/@id)"/>
			<xsl:choose>
				<xsl:when test="$map">
					<xsl:choose>
						<xsl:when test="$map/@remove">
							<!-- Nothing to do -->
						</xsl:when>
						<xsl:when test="$map/@simple">
							<xsl:for-each select="$self">
								<xsl:copy>
									<xsl:apply-templates select="@*" mode="CoRefORM"/>
									<xsl:attribute name="IsSimple">
										<xsl:text>true</xsl:text>
									</xsl:attribute>
									<xsl:apply-templates select="node()" mode="CoRefORM"/>
								</xsl:copy>
							</xsl:for-each>
						</xsl:when>
					</xsl:choose>
				</xsl:when>
				<xsl:otherwise>
					<xsl:for-each select="$self">
						<xsl:call-template name="DefaultCoRefORMTemplate"/>
					</xsl:for-each>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:for-each>
	</xsl:template>
	<xsl:template match="orm:RoleSequence[ancestor::orm:Constraints]" mode="CoRefORM">
		<xsl:variable name="self" select="."/>
		<xsl:for-each select="$MapDoc">
			<xsl:variable name="roleMaps" select="key('KeyedRoleMap',$self/orm:Role/@ref)"/>
			<xsl:choose>
				<xsl:when test="$roleMaps">
					<!-- Use the alternate unaryValueRole instead of a mapped role -->
					<xsl:for-each select="$self">
						<xsl:variable name="checkUnaryValueRole" select="boolean(parent::orm:UniquenessConstraint[not(@IsInternal[.='true' or .='1'])] | parent::orm:FrequencyConstraint)"/>
						<xsl:copy>
							<xsl:for-each select="orm:Role">
								<xsl:variable name="map" select="$roleMaps[@fromRole=current()/@ref]"/>
								<xsl:choose>
									<xsl:when test="$map">
										<xsl:choose>
											<xsl:when test="$map[@paired and $roleMaps[@fromRole=$map/@toRole]]">
												<!-- Nothing to do. The paired role is also part of the constraint, and we don't want it listed twice. -->
											</xsl:when>
											<xsl:when test="$checkUnaryValueRole and $map/@unaryValueRole">
												<xsl:copy>
													<xsl:copy-of select="@id"/>
													<xsl:attribute name="ref">
														<xsl:value-of select="$map/@unaryValueRole"/>
													</xsl:attribute>
												</xsl:copy>
											</xsl:when>
											<xsl:when test="$map/@toRole">
												<xsl:copy>
													<xsl:copy-of select="@id"/>
													<xsl:attribute name="ref">
														<xsl:value-of select="$map/@toRole"/>
													</xsl:attribute>
												</xsl:copy>
											</xsl:when>
											<xsl:otherwise>
												<xsl:copy>
													<xsl:copy-of select="@*"/>
												</xsl:copy>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:copy>
											<xsl:copy-of select="@*"/>
										</xsl:copy>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:for-each>
							<!-- Do not copy the JoinRule back in -->
						</xsl:copy>
					</xsl:for-each>
				</xsl:when>
				<xsl:otherwise>
					<xsl:for-each select="$self">
						<xsl:call-template name="DefaultCoRefORMTemplate"/>
					</xsl:for-each>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:for-each>
	</xsl:template>
	<xsl:template match="orm:ExclusiveOrExclusionConstraint|orm:ExclusiveOrMandatoryConstraint" mode="CoRefORM">
		<xsl:variable name="self" select="."/>
		<xsl:for-each select="$MapDoc">
			<xsl:variable name="map" select="key('KeyedConstraintMap',$self/@ref)"/>
			<xsl:if test="not($map/@remove)">
				<xsl:for-each select="$self">
					<xsl:call-template name="DefaultCoRefORMTemplate"/>
				</xsl:for-each>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>
	<xsl:template match="orm:Objects" mode="CoRefORM">
		<xsl:copy>
			<xsl:apply-templates select="node()|@*" mode="CoRefORM"/>
			<xsl:for-each select="$ExpandedUnaries">
				<xsl:variable name="factTypeId" select="string(@id)"/>
				<xsl:variable name="valueTypeNameFragment">
					<xsl:value-of select="key('KeyedObjectTypes',orm:FactRoles/orm:Role/orm:RolePlayer/@ref)/@Name"/>
					<xsl:text>_</xsl:text>
					<xsl:call-template name="GetUnaryFactTypeRoleName">
						<xsl:with-param name="UnaryFactType" select="."/>
					</xsl:call-template>
				</xsl:variable>
				<xsl:variable name="valueTypeName" select="string($valueTypeNameFragment)"/>
				<orm:ValueType id="{$factTypeId}{$CoRefUnaryValueTypeDecorator}" Name="{$valueTypeName}">
					<orm:PlayedRoles>
						<orm:Role ref="{$factTypeId}{$CoRefUnaryValueRoleDecorator}"/>
					</orm:PlayedRoles>
					<orm:ConceptualDataType id="{$factTypeId}{$CoRefValueDataTypeIdDecorator}" ref="{$CoRefLogicalDataTypeIdDecorator}" Scale="0" Length="0" />
					<xsl:if test="not(starts-with(@UnaryPattern,'RequiredWithNegation') and orm:NegatedByUnaryFactType/@ref=$PairedUnaryNegationIds)">
						<orm:ValueRestriction>
							<orm:ValueConstraint id="{$factTypeId}{$CoRefUnaryValueConstraintDecorator}" Name="{$valueTypeName}">
								<orm:ValueRanges>
									<orm:ValueRange id="{$factTypeId}{$CoRefUnaryValueRangeDecorator}" MinValue="True" MaxValue="True" MinInclusion="NotSet" MaxInclusion="NotSet" />
								</orm:ValueRanges>
							</orm:ValueConstraint>
						</orm:ValueRestriction>
					</xsl:if>
				</orm:ValueType>
			</xsl:for-each>
		</xsl:copy>
	</xsl:template>
	<xsl:template match="orm:DataTypes" mode="CoRefORM">
		<xsl:copy>
			<xsl:if test="not($ExistingTrueOrFalseLogicalDataType) and $ExpandedUnaries">
				<orm:TrueOrFalseLogicalDataType id="{$CoRefLogicalDataTypeIdDecorator}" />
			</xsl:if>
			<xsl:copy-of select="orm:*"/>
		</xsl:copy>
	</xsl:template>
	<xsl:template match="orm:DerivationExpression | orm:SubtypeDerivationExpression | orm:FactTypeDerivationPath | orm:SubtypeDerivationPath">
		<!-- No one is interpreting the role-referencing contents of these elements. However, the
		attributes are used to determine derivation state, so leave them in place -->
		<xsl:copy>
			<xsl:copy-of select="@*"/>
		</xsl:copy>
	</xsl:template>
	<xsl:template match="orm:Instances | orm:RoleInstances | orm:ModelErrors | orm:ModelNotes | orm:GeneralRules | orm:JoinRule" mode="CoRefORM">
		<!-- We just need the coref form. Do not try to rebuild instances. -->
		<!-- There is enough reduction here that it is very likely an error can reference
		a deleted item, eliminate the errors -->
		<!-- Kill notes in the coref orm file. We're not using them anywhere, and we end
		up turning some facts into object types, so the ModelNote/ReferencedByFactType links
		may fail to load. -->
		<!-- The consumers of this output are not reading rules -->
	</xsl:template>
</xsl:stylesheet>
