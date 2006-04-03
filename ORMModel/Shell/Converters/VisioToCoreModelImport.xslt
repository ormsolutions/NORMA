<?xml version="1.0" encoding="utf-8"?>
<!--
	Neumont Object-Role Modeling Architect for Visual Studio

	Copyright © Neumont University. All rights reserved.

	The use and distribution terms for this software are covered by the
	Common Public License 1.0 (http://opensource.org/licenses/cpl) which
	can be found in the file CPL.txt at the root of this distribution.
	By using this software in any fashion, you are agreeing to be bound by
	the terms of this license.

	You must not remove this notice, or any other, from this software.
-->
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:exsl="http://exslt.org/common"
	xmlns:ormRoot="http://schemas.neumont.edu/ORM/2006-01/ORMRoot"
	xmlns:orm="http://schemas.neumont.edu/ORM/2006-01/ORMCore"
	xmlns:ormDiagram="http://schemas.neumont.edu/ORM/2006-01/ORMDiagram"
	exclude-result-prefixes="#default xsl"
	extension-element-prefixes="exsl">
	<xsl:output method="xml" encoding="utf-8" media-type="application/orm+xml" indent="no"/>
	<xsl:variable name="apos" select='"&apos;"' />
	<xsl:variable name="seperator" select='", "' />
	<!--We Shouldn't have to do this for some reason the importer will not run without it.-->
	<xsl:template match="/">
		<xsl:apply-templates select="VisioModels/ORMSourceModels/ORMSourceModel" />
	</xsl:template>
	<xsl:template match="VisioModels/ORMSourceModels/ORMSourceModel">
		<orm:ORMModel id="Model_id" Name="importedModel">
			<xsl:apply-templates select="Objects" />
			<xsl:apply-templates select="Facts" />
			<xsl:apply-templates select="Constraints"/>
			<xsl:call-template name="DataTypes" />
			<xsl:call-template name="ReferenceModeKinds" />
		</orm:ORMModel>
	</xsl:template>
	<!--Begin Object Templates-->
	<xsl:template match="Objects">
		<orm:Objects>
			<xsl:apply-templates select="Object[@ObjectKind='Entity Type' and not(@NestedPredicateFactID)]" />
			<xsl:apply-templates select="Object[@ObjectKind='Entity Type' and @NestedPredicateFactID]" />
			<xsl:apply-templates select="Object[@ObjectKind='Value Type']" />
		</orm:Objects>
	</xsl:template>
	<xsl:template match="Object[@ObjectKind='Entity Type' and not(@NestedPredicateFactID)]">
		<orm:EntityType>
			<xsl:apply-templates select="@ObjectID" />
			<xsl:apply-templates select="@ObjectName" />
			<xsl:apply-templates select="@IsIndependent" />
			<xsl:apply-templates select="@IsExternal" />
			<xsl:apply-templates select="@IsPersonal" />
			<xsl:apply-templates select="@ReferenceMode" />
			<xsl:apply-templates select="ObjectNotes" />
		</orm:EntityType>
	</xsl:template>
	<xsl:template match="Object[@ObjectKind='Entity Type' and @NestedPredicateFactID]">
		<orm:ObjectifiedType>
			<xsl:apply-templates select="@ObjectID" />
			<xsl:apply-templates select="@ObjectName" />
			<xsl:apply-templates select="@IsIndependent" />
			<xsl:apply-templates select="@IsExternal" />
			<xsl:apply-templates select="@IsPersonal" />
			<xsl:apply-templates select="ObjectNotes" />
			<xsl:apply-templates select="@NestedPredicateFactID" />
		</orm:ObjectifiedType>
	</xsl:template>
	<xsl:template match="Object[@ObjectKind='Value Type']">
		<orm:ValueType>
			<xsl:apply-templates select="@ObjectID" />
			<xsl:apply-templates select="@ObjectName" />
			<xsl:apply-templates select="@IsIndependent" />
			<xsl:apply-templates select="@IsExternal" />
			<xsl:apply-templates select="@IsPersonal" />
			<xsl:apply-templates select="ObjectNotes" />
			<xsl:apply-templates select="@ConceptualDatatype" />
			<xsl:apply-templates select="@AllowableValues" />
		</orm:ValueType>
	</xsl:template>
	<xsl:template match="@ObjectID">
		<xsl:attribute name="id">
			<xsl:text>GUID_ObjectID</xsl:text>
			<xsl:value-of select="."/>
		</xsl:attribute>
	</xsl:template>
	<xsl:template match="@ObjectName">
		<xsl:attribute name="Name">
			<xsl:value-of select="."/>
		</xsl:attribute>
	</xsl:template>
	<xsl:template match="@IsIndependent">
		<xsl:copy />
	</xsl:template>
	<xsl:template match="@IsExternal">
		<xsl:copy />
	</xsl:template>
	<xsl:template match="@IsPersonal">
		<xsl:copy />
	</xsl:template>
	<xsl:template match="@ReferenceMode">
		<xsl:attribute name="_ReferenceMode">
			<xsl:value-of select="."/>
		</xsl:attribute>
	</xsl:template>
	<xsl:template match="ObjectNotes">
		<orm:Notes>
			<orm:Note id="GUID_ObjectNoteID_{../@ObjectID}">
				<orm:Text>
					<xsl:value-of select ="."/>
				</orm:Text>
			</orm:Note>
		</orm:Notes>
	</xsl:template>
	<xsl:template match="@NestedPredicateFactID">
		<orm:NestedPredicate ref="GUID_FactID{.}" />
	</xsl:template>
	<xsl:template match="@ConceptualDatatype">
		<orm:ConceptualDataType>
			<xsl:attribute name="id">
				<xsl:text>GUID_DatatypeID</xsl:text>
				<xsl:value-of select="../@ObjectID"/>
			</xsl:attribute>
			<xsl:attribute name="ref">
				<xsl:choose>
					<xsl:when test="contains(., 'R-Fixed Length')">
						<xsl:text>GUID_FixedLengthRawDataDataType</xsl:text>
					</xsl:when>
					<xsl:when test="contains(., 'N-Floating Point')">
						<xsl:text>GUID_FloatingPointNumericDataType</xsl:text>
					</xsl:when>
					<xsl:when test="contains(., 'N-Decimal')">
						<xsl:text>GUID_DecimalNumericDataType</xsl:text>
					</xsl:when>
					<xsl:when test="contains(., 'C-Large Length')">
						<xsl:text>GUID_LargeLengthTextDataType</xsl:text>
					</xsl:when>
					<xsl:when test="contains(., 'R-OLE Object')">
						<xsl:text>GUID_OleObjectRawDataDataType</xsl:text>
					</xsl:when>
					<xsl:when test="contains(., 'N-Signed Integer')">
						<xsl:text>GUID_SignedIntegerNumericDataType</xsl:text>
					</xsl:when>
					<xsl:when test="contains(., 'N-Unsigned Integer')">
						<xsl:text>GUID_UnsignedIntegerNumericDataType</xsl:text>
					</xsl:when>
					<xsl:when test="contains(., 'T-Date &amp; Time')">
						<xsl:text>GUID_DateAndTimeTemporalDataType</xsl:text>
					</xsl:when>
					<xsl:when test="contains(., 'N-Money')">
						<xsl:text>GUID_MoneyNumericDataType</xsl:text>
					</xsl:when>
					<xsl:when test="contains(., 'N-Auto Counter')">
						<xsl:text>GUID_AutoCounterNumericDataType</xsl:text>
					</xsl:when>
					<xsl:when test="contains(., 'C-Fixed Length')">
						<xsl:text>GUID_FixedLengthTextDataType</xsl:text>
					</xsl:when>
					<xsl:when test="contains(., 'L-True or False')">
						<xsl:text>GUID_TrueOrFalseLogicalDataType</xsl:text>
					</xsl:when>
					<xsl:when test="contains(., 'C-Variable Length')">
						<xsl:text>GUID_VariableLengthTextDataType</xsl:text>
					</xsl:when>
					<xsl:when test="contains(., 'R-Variable Length')">
						<xsl:text>GUID_VariableLengthRawDataDataType</xsl:text>
					</xsl:when>
					<xsl:otherwise>
						<xsl:text>GUID_UnspecifiedDataType</xsl:text>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="Scale">
				<xsl:variable name="lengthScale" select="substring-before(substring-after(.,'('),')')"/>
				<xsl:choose>
					<xsl:when test="contains($lengthScale,',')">
						<xsl:value-of select="substring-after($lengthScale, ',')"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="'0'"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="Length">
				<xsl:variable name="lengthScale" select="substring-before(substring-after(.,'('),')')"/>
				<xsl:choose>
					<xsl:when test="string-length($lengthScale) = 0">
						<xsl:value-of select="'0'"/>
					</xsl:when>
					<xsl:when test="contains($lengthScale, ',')">
						<xsl:value-of select="substring-before($lengthScale, ',')"/>
					</xsl:when>
					<xsl:when test="not(contains($lengthScale, ','))">
						<xsl:value-of select="$lengthScale"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="'0'"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
		</orm:ConceptualDataType>
	</xsl:template>
	<xsl:template match="@AllowableValues">
		<orm:ValueRestriction>
			<xsl:variable name="playedRoleId" select="../PlayedRoles/PlayedRole[1]/@PlayedRoleID"/>
			<orm:ValueConstraint id="GUID_RoleValueRangeDefinitionID{$playedRoleId}" Name="RoleValueRangeDefinitionID{$playedRoleId}">
				<orm:ValueRanges>
					<xsl:call-template name="valueRanges">
						<xsl:with-param name="allowedValues" select="translate(normalize-space(.), $apos, '')" />
						<xsl:with-param name="objectID" select="$playedRoleId" />
					</xsl:call-template>
				</orm:ValueRanges>
			</orm:ValueConstraint>
		</orm:ValueRestriction>
	</xsl:template>
	<xsl:template name="valueRanges">
		<xsl:param name="allowedValues" />
		<xsl:param name="objectID" />
		<xsl:choose>
			<xsl:when test="contains($allowedValues, $seperator)">
				<xsl:variable name="valueRange" select="substring-before($allowedValues, $seperator)" />
				<orm:ValueRange id="GUID_ObjectID{$objectID}_ValueRange_{translate($valueRange, ' ', '')}" MinValue="{$valueRange}" MaxValue="{$valueRange}" MinInclusion="NotSet" MaxInclusion="NotSet" />
				<xsl:call-template name="valueRanges">
					<xsl:with-param name="allowedValues" select="substring-after($allowedValues, $seperator)" />
					<xsl:with-param name="objectID" select="$objectID" />
				</xsl:call-template>
			</xsl:when>
			<xsl:when test="not(contains($allowedValues, $seperator))" >
				<xsl:choose>
					<xsl:when test="not(contains($allowedValues, ' .. '))">
						<xsl:choose>
							<xsl:when test="contains($allowedValues, ' ..')">
								<xsl:variable name="adjustedValueRange" select="translate($allowedValues, ' ..', '-')"/>
								<orm:ValueRange id="GUID_ObjectID{$objectID}_ValueRange_{$adjustedValueRange}" MinValue="{substring-before($adjustedValueRange, '-')}" MaxValue="" MinInclusion="NotSet" MaxInclusion="NotSet" />
							</xsl:when>
							<xsl:otherwise>
								<orm:ValueRange id="GUID_ObjectID{$objectID}_ValueRange_{translate($allowedValues, ' ', '')}" MinValue="{$allowedValues}" MaxValue="{$allowedValues}" MinInclusion="NotSet" MaxInclusion="NotSet" />
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:when test="contains($allowedValues, ' .. ')">
						<xsl:variable name="adjustedValueRange" select="translate($allowedValues, ' .. ', '-')"/>
						<orm:ValueRange id="GUID_ObjectID{$objectID}_ValueRange_{$adjustedValueRange}" MinValue="{substring-before($adjustedValueRange, '--')}" MaxValue="{substring-after($adjustedValueRange, '--')}" MinInclusion="NotSet" MaxInclusion="NotSet" />
					</xsl:when>
				</xsl:choose>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
	<!--End Object Templates-->
	<!--Being Fact Templates-->
	<xsl:template match="Facts">
		<orm:Facts>
			<xsl:apply-templates select="Fact" />
			<xsl:call-template name="SubtypeFacts">
				<xsl:with-param name="superTypeObjects" select="../Objects/Object[Supertypes]" />
			</xsl:call-template>
		</orm:Facts>
	</xsl:template>
	<xsl:template match="Fact">
		<orm:Fact>
			<xsl:apply-templates select="@FactID" />
			<xsl:apply-templates select="@IsExternal" />
			<xsl:apply-templates select="FactNotes" />
			<xsl:apply-templates select="FactRoles" />
			<xsl:apply-templates select="FactReadings"/>
			<xsl:apply-templates select="FactConstraints"/>
		</orm:Fact>
	</xsl:template>
	<xsl:template match="@FactID">
		<xsl:attribute name="id">
			<xsl:text>GUID_FactID</xsl:text>
			<xsl:value-of select="."/>
		</xsl:attribute>
		<xsl:attribute name="Name">
			<xsl:text>FactID</xsl:text>
			<xsl:value-of select="."/>
		</xsl:attribute>
	</xsl:template>
	<xsl:template match="FactNotes">
		<orm:Notes>
			<orm:Note id="GUID_FactNoteID_{../@FactID}">
				<orm:Text>
					<xsl:value-of select="."/>
				</orm:Text>
			</orm:Note>
		</orm:Notes>
	</xsl:template>
	<xsl:template match="FactRoles">
		<orm:FactRoles>
			<xsl:apply-templates select="FactRole"/>
		</orm:FactRoles>
	</xsl:template>
	<xsl:template match="FactRole">
		<xsl:variable name="ValueRole" select="../../../../Roles/Role[@RoleID=current()/@FactRoleID]" />
		<orm:Role>
			<xsl:attribute name="id">
				<xsl:text>GUID_RoleID</xsl:text>
				<xsl:value-of select ="@FactRoleID"/>
			</xsl:attribute>
			<xsl:attribute name="Name">
				<xsl:text>RoleID</xsl:text>
				<xsl:value-of select ="@FactRoleID"/>
			</xsl:attribute>
			<xsl:attribute name="_IsMandatory">
				<!--This is a calculated field the loader ignores it anyway.-->
				<xsl:text>false</xsl:text>
			</xsl:attribute>
			<xsl:attribute name="_Multiplicity">
				<!--This is a calculated field the loader ignores it anyway.-->
				<xsl:text>Unspecified</xsl:text>
			</xsl:attribute>
			<orm:RolePlayer>
				<xsl:attribute name="ref">
					<xsl:text>GUID_ObjectID</xsl:text>
					<xsl:value-of select="$ValueRole/@RolePlayerObjectID"/>
				</xsl:attribute>
			</orm:RolePlayer>
			<xsl:variable name="innerObject" select="../../../../Objects/Object[@ObjectKind='Value Type' and @ObjectNamespace and @ObjectID=$ValueRole/@RolePlayerObjectID]" />
			<xsl:apply-templates select="../../../../Objects/Object[@ObjectName=$innerObject/@ObjectNamespace and @AllowableValues]" mode="ValueRanges">
				<xsl:with-param name="ValueRole" select="$ValueRole/@RolePlayerObjectID" />
			</xsl:apply-templates>
		</orm:Role>
	</xsl:template>
	<xsl:template match="Object" mode="ValueRanges">
		<xsl:param name="ValueRole" />
		<xsl:variable name="Values" select="normalize-space(@AllowableValues)" />
		<orm:ValueRestriction>
			<orm:RoleValueConstraint id="GUID_RoleValueRangeDefinitionID{$ValueRole}" Name="RoleValueRangeDefinitionID{$ValueRole}">
				<orm:ValueRanges>
					<xsl:call-template name="valueRanges">
						<xsl:with-param name="allowedValues" select="translate($Values, $apos, '')" />
						<xsl:with-param name="objectID" select="$ValueRole" />
					</xsl:call-template>
				</orm:ValueRanges>
			</orm:RoleValueConstraint>
		</orm:ValueRestriction>
	</xsl:template>
	<xsl:template match="FactReadings">
		<orm:ReadingOrders>
			<xsl:apply-templates select="FactReading"/>
		</orm:ReadingOrders>
	</xsl:template>
	<xsl:template match="FactReading">
		<xsl:variable name="location" select="position()"/>
		<xsl:variable name="factRoleID" select="../../FactRoles/FactRole[$location]/@FactRoleID"/>
		<xsl:variable name="role" select="../../../../Roles/Role[@RoleID=$factRoleID]"/>
		<xsl:variable name="factRole" select="../../FactRoles/FactRole" />
		<orm:ReadingOrder>
			<xsl:attribute name="id">
				<xsl:text>GUID_ReadingOrderID</xsl:text>
				<xsl:value-of select="$factRoleID"/>
			</xsl:attribute>
			<orm:Readings>
				<orm:Reading>
					<xsl:attribute name="id">
						<xsl:text>GUID_ReadingID</xsl:text>
						<xsl:value-of select="$factRoleID"/>
					</xsl:attribute>
					<xsl:attribute name="IsPrimary">
						<xsl:value-of select="'true'"/>
					</xsl:attribute>
					<orm:Data>
						<xsl:variable name="currentReading" select="$role/@RoleReading" />
						<xsl:choose>
							<xsl:when test="count($factRole) = 1">
								<xsl:text>{0} </xsl:text>
								<xsl:value-of select="$currentReading"/>
							</xsl:when>
							<xsl:when test="count($factRole) = 2">
								<xsl:choose>
									<xsl:when test="contains($currentReading, '...')">
										<xsl:call-template name="readingParse">
											<xsl:with-param name="readingString" select="$currentReading" />
										</xsl:call-template>
									</xsl:when>
									<xsl:when test="not(contains($currentReading, '...'))">
										<xsl:text>{0} </xsl:text>
										<xsl:value-of select="$currentReading"/>
										<xsl:text> {1}</xsl:text>
									</xsl:when>
								</xsl:choose>
							</xsl:when>
							<xsl:when test="count($factRole) &gt;= 3">
								<xsl:call-template name="readingParse">
									<xsl:with-param name="readingString" select="$currentReading" />
								</xsl:call-template>
							</xsl:when>
						</xsl:choose>
					</orm:Data>
				</orm:Reading>
			</orm:Readings>
			<orm:RoleSequence>
				<xsl:variable name="factRoleIDs" select="../../FactRoles/FactRole/@FactRoleID"/>
				<xsl:variable name="matchingRoles" select="../../../../Roles/Role[@RoleID=$factRoleIDs]" />
				<xsl:variable name="factRoleIDsAscendingFrag">
					<xsl:for-each select="$matchingRoles">
						<xsl:sort select="@PredicatePosition" data-type="number" order="ascending"/>
						<xsl:copy-of select="."/>
					</xsl:for-each>
				</xsl:variable>
				<xsl:variable name="factRoleIDsDescendingFrag">
					<xsl:for-each select="$matchingRoles">
						<xsl:sort select="@PredicatePosition" data-type="number" order="descending"/>
						<xsl:copy-of select="."/>
					</xsl:for-each>
				</xsl:variable>
				<xsl:variable name="factRoleIDsAscending" select="exsl:node-set($factRoleIDsAscendingFrag)/child::*"/>
				<xsl:variable name="factRoleIDsDescending" select="exsl:node-set($factRoleIDsDescendingFrag)/child::*"/>
				<xsl:choose>
					<xsl:when test="$location = 1">
						<xsl:for-each select="$factRoleIDsAscending">
							<orm:Role>
								<xsl:attribute name="ref">
									<xsl:text>GUID_RoleID</xsl:text>
									<xsl:value-of select="@RoleID"/>
								</xsl:attribute>
							</orm:Role>
						</xsl:for-each>
					</xsl:when>
					<xsl:when test="$location = 2">
						<xsl:for-each select="$factRoleIDsDescending">
							<orm:Role>
								<xsl:attribute name="ref">
									<xsl:text>GUID_RoleID</xsl:text>
									<xsl:value-of select="@RoleID"/>
								</xsl:attribute>
							</orm:Role>
						</xsl:for-each>
					</xsl:when>
				</xsl:choose>
			</orm:RoleSequence>
		</orm:ReadingOrder>
	</xsl:template>
	<xsl:template name="readingParse">
		<xsl:param name="readingString" />
		<xsl:param name="count" select="number(0)" />
		<xsl:choose>
			<xsl:when test="contains($readingString, '...')">
				<xsl:text> {</xsl:text>
				<xsl:value-of select="$count"/>
				<xsl:text>} </xsl:text>
				<xsl:value-of select="substring-before(substring-after($readingString, '...'), '...')" />
				<xsl:call-template name="readingParse">
					<xsl:with-param name="readingString">
						<xsl:value-of select="substring-after($readingString, '...')" />
					</xsl:with-param>
					<xsl:with-param name="count" select="$count + 1" />
				</xsl:call-template>
			</xsl:when>
			<xsl:when test="not(contains($readingString, '...'))">
				<xsl:value-of select="$readingString"/>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="FactConstraints">
		<orm:InternalConstraints>
			<xsl:apply-templates select="FactConstraint"/>
		</orm:InternalConstraints>
	</xsl:template>
	<xsl:template match="FactConstraint">
		<xsl:variable name="tempConstraint" select="../../../../Constraints/Constraint[@ConstraintID=current()/@FactConstraintID]"/>
		<xsl:apply-templates select="$tempConstraint[@IsInternal='true' and @ConstraintType='Uniqueness' and @IsPrimaryReference='false']" />
		<xsl:apply-templates select="$tempConstraint[@IsInternal='true' and @ConstraintType='Mandatory']" />
		<xsl:apply-templates select="$tempConstraint[@IsInternal='true' and @ConstraintType='Uniqueness' and @IsPrimaryReference='true']" />
	</xsl:template>
	<xsl:template match="Constraint[@IsInternal='true' and @ConstraintType='Uniqueness' and @IsPrimaryReference='false']">
		<xsl:variable name="tempID" select="@ConstraintID" />
		<orm:InternalUniquenessConstraint id="GUID_ConstraintID{$tempID}" Name="InternalUniquenessConstraint{$tempID}">
			<orm:RoleSequence>
				<xsl:for-each select="RoleSequences/RoleSequence/RoleSequenceItems/RoleSequenceItem/@RoleSequenceItemRoleID">
					<orm:Role ref="GUID_RoleID{.}" />
				</xsl:for-each>
			</orm:RoleSequence>
		</orm:InternalUniquenessConstraint>
	</xsl:template>
	<xsl:template match="Constraint[@IsInternal='true' and @ConstraintType='Mandatory']">
		<xsl:variable name="tempID" select="@ConstraintID" />
		<orm:SimpleMandatoryConstraint id="GUID_ConstraintID{$tempID}" Name="SimpleMandatoryConstraint{$tempID}">
			<orm:RoleSequence>
				<xsl:for-each select="RoleSequences/RoleSequence/RoleSequenceItems/RoleSequenceItem/@RoleSequenceItemRoleID">
					<orm:Role ref="GUID_RoleID{.}" />
				</xsl:for-each>
			</orm:RoleSequence>
		</orm:SimpleMandatoryConstraint>
	</xsl:template>
	<xsl:template match="Constraint[@IsInternal='true' and @ConstraintType='Uniqueness' and @IsPrimaryReference='true']">
		<xsl:variable name="tempID" select="@ConstraintID" />
		<orm:InternalUniquenessConstraint id="GUID_ConstraintID{$tempID}" Name="InternalUniquenessConstraint{$tempID}">
			<orm:RoleSequence>
				<xsl:for-each select="RoleSequences/RoleSequence/RoleSequenceItems/RoleSequenceItem/@RoleSequenceItemRoleID">
					<orm:Role ref="GUID_RoleID{.}" />
				</xsl:for-each>
			</orm:RoleSequence>
			<xsl:variable name="tempFact" select="../../Facts/Fact[FactConstraints/FactConstraint/@FactConstraintID=current()/@ConstraintID]/FactRoles/FactRole[1]/@FactRoleID" />
			<xsl:variable name="tempRole" select="../../Roles/Role[@RoleID = $tempFact]/@RolePlayerObjectID" />
			<orm:PreferredIdentifierFor ref="GUID_ObjectID{$tempRole}" />
		</orm:InternalUniquenessConstraint>
	</xsl:template>
	<!--End Fact Templates-->
	<!--Begin SubtypeFactTemplates-->
	<xsl:template name="SubtypeFacts">
		<xsl:param name="superTypeObjects" />
		<xsl:apply-templates select="$superTypeObjects" mode="subtyping" />
	</xsl:template>
	<xsl:template match="Object" mode="subtyping">
		<orm:SubtypeFact id="GUID_SubtypeFactID{@ObjectID}" Name="SubtypeFactID{@ObjectID}" IsExternal="false">
			<xsl:call-template name="superTypeFactRoles" />
			<xsl:call-template name="supertypeFactReadings" />
			<xsl:call-template name="supertypeFactConstraints" />
		</orm:SubtypeFact>
	</xsl:template>
	<xsl:template name="superTypeFactRoles">
		<orm:FactRoles>
			<xsl:call-template name="supertypeFactRolesRole" />
		</orm:FactRoles>
	</xsl:template>
	<xsl:template name="supertypeFactRolesRole">
		<xsl:apply-templates select="Supertypes" mode="subtypeing" />
	</xsl:template>
	<xsl:template match="Supertypes" mode="subtypeing">
		<xsl:apply-templates select="Supertype" mode="subtypeing" />
	</xsl:template>
	<xsl:template match="Supertype" mode="subtypeing">
		<orm:SubtypeMetaRole id="GUID_subtypeFactRoleID{../../@ObjectID}_1" Name="subtypeFactRoleID{../../@ObjectID}_1" _IsMandatory="false" _Multiplicity="Unspecified">
			<orm:RolePlayer ref="GUID_ObjectID{../../@ObjectID}" />
		</orm:SubtypeMetaRole>
		<orm:SupertypeMetaRole id="GUID_subtypeFactRoleID{../../@ObjectID}_2" Name="subtypeFactRoleID{../../@ObjectID}_2" _IsMandatory="false" _Multiplicity="Unspecified">
			<orm:RolePlayer ref="GUID_ObjectID{@SupertypeObjectID}" />
		</orm:SupertypeMetaRole>
	</xsl:template>
	<xsl:template name="supertypeFactReadings">
		<orm:ReadingOrders>
			<xsl:call-template name="supertypeFactReading" />
		</orm:ReadingOrders>
	</xsl:template>
	<xsl:template name="supertypeFactReading">
		<orm:ReadingOrder id="GUID_supertypeFactReadingOrder{@ObjectID}_1">
			<orm:Readings>
				<orm:Reading id="GUID_supertypeFactReading{@ObjectID}_1" IsPrimary="true">
					<orm:Data>{0} is a subtype of {1}</orm:Data>
				</orm:Reading>
			</orm:Readings>
			<orm:RoleSequence>
				<orm:Role ref="GUID_subtypeFactRoleID{@ObjectID}_1" />
				<orm:Role ref="GUID_subtypeFactRoleID{@ObjectID}_2" />
			</orm:RoleSequence>
		</orm:ReadingOrder>
		<orm:ReadingOrder id="GUID_supertypeFactReadingOrder{@ObjectID}_2">
			<orm:Readings>
				<orm:Reading id="GUID_supertypeFactReading{@ObjectID}_2" IsPrimary="true">
					<orm:Data>{0} is a supertype of {1}</orm:Data>
				</orm:Reading>
			</orm:Readings>
			<orm:RoleSequence>
				<orm:Role ref="GUID_subtypeFactRoleID{@ObjectID}_2" />
				<orm:Role ref="GUID_subtypeFactRoleID{@ObjectID}_1" />
			</orm:RoleSequence>
		</orm:ReadingOrder>
	</xsl:template>
	<xsl:template name="supertypeFactConstraints">
		<orm:InternalConstraints>
			<orm:InternalUniquenessConstraint id="GUID_InternalUniquenessConstraint_subtypeFactRoleID{@ObjectID}_1" Name="InternalUniquenessConstraint_subtypeFactRoleID{@ObjectID}_1">
				<orm:RoleSequence>
					<orm:Role ref="GUID_subtypeFactRoleID{@ObjectID}_1" />
				</orm:RoleSequence>
			</orm:InternalUniquenessConstraint>
			<orm:SimpleMandatoryConstraint id="GUID_SimpleMandatoryConstraintID_subtypeFactRoleID{@ObjectID}_1" Name="SimpleMandatoryConstraintID_subtypeFactRoleID{@ObjectID}_1">
				<orm:RoleSequence>
					<orm:Role ref="GUID_subtypeFactRoleID{@ObjectID}_2" />
				</orm:RoleSequence>
			</orm:SimpleMandatoryConstraint>
			<orm:InternalUniquenessConstraint id="GUID_InternalUniquenessConstraint_subtypeFactRoleID{@ObjectID}_2" Name="InternalUniquenessConstraint_subtypeFactRoleID{@ObjectID}_2">
				<orm:RoleSequence>
					<orm:Role ref="GUID_subtypeFactRoleID{@ObjectID}_2" />
				</orm:RoleSequence>
			</orm:InternalUniquenessConstraint>
		</orm:InternalConstraints>
	</xsl:template>
	<!--End SubtypeFactTemplates-->
	<!--Begin External Constraints Template-->
	<xsl:template match="Constraints">
		<orm:ExternalConstraints>
			<xsl:apply-templates select="Constraint[@IsInternal='false' and @ConstraintType='Uniqueness' and @IsPrimaryReference='false']" />
			<xsl:apply-templates select="Constraint[@IsInternal='false' and @ConstraintType='Uniqueness' and @IsPrimaryReference='true']" />
			<xsl:apply-templates select="Constraint[@IsInternal='false' and @ConstraintType='Mandatory']" />
			<xsl:apply-templates select="Constraint[@IsInternal='false' and @ConstraintType='Exclusion']" />
			<xsl:apply-templates select="Constraint[@ConstraintType='Ring']" />
			<xsl:apply-templates select="Constraint[@ConstraintType='Equality']" />
			<xsl:apply-templates select="Constraint[@ConstraintType='Subset']" />
			<xsl:apply-templates select="Constraint[@ConstraintType='Frequency']" />
		</orm:ExternalConstraints>
	</xsl:template>
	<xsl:template match="Constraint[@IsInternal='false' and @ConstraintType='Uniqueness' and @IsPrimaryReference='false']">
		<orm:ExternalUniquenessConstraint id="GUID_ExternalConstraintID{@ConstraintID}" Name="ExternalConstraintID{@ConstraintID}">
			<xsl:for-each select="RoleSequences/RoleSequence">
				<orm:RoleSequence>
					<xsl:for-each select="RoleSequenceItems/RoleSequenceItem">
						<xsl:sort select="@SequencePositionNumber" data-type="number"/>
						<orm:Role ref="GUID_RoleID{@RoleSequenceItemRoleID}"/>
					</xsl:for-each>
				</orm:RoleSequence>
			</xsl:for-each>
		</orm:ExternalUniquenessConstraint>
	</xsl:template>
	<xsl:template match="Constraint[@IsInternal='false' and @ConstraintType='Uniqueness' and @IsPrimaryReference='true']">
		<orm:ExternalUniquenessConstraint id="GUID_ExternalConstraintID{@ConstraintID}" Name="ExternalConstraintID{@ConstraintID}">
			<xsl:for-each select="RoleSequences/RoleSequence">
				<orm:RoleSequence>
					<xsl:for-each select="RoleSequenceItems/RoleSequenceItem">
						<xsl:sort select="@SequencePositionNumber" data-type="number"/>
						<orm:Role ref="GUID_RoleID{@RoleSequenceItemRoleID}"/>
					</xsl:for-each>
				</orm:RoleSequence>
			</xsl:for-each>
			<xsl:variable name="tempFact" select="../../Facts/Fact[FactConstraints/FactConstraint/@FactConstraintID=current()/@ConstraintID][1]/FactRoles/FactRole[1]/@FactRoleID" />
			<xsl:variable name="tempRole" select="../../Roles/Role[@RoleID = $tempFact]/@RolePlayerObjectID" />
			<orm:PreferredIdentifierFor ref="GUID_ObjectID{$tempRole}" />
		</orm:ExternalUniquenessConstraint>
	</xsl:template>
	<xsl:template match="Constraint[@IsInternal='false' and @ConstraintType='Mandatory']">
		<orm:DisjunctiveMandatoryConstraint id="GUID_ExternalConstraintID{@ConstraintID}" Name="ExternalConstraintID{@ConstraintID}">
			<xsl:for-each select="RoleSequences/RoleSequence">
				<orm:RoleSequence>
					<xsl:for-each select="RoleSequenceItems/RoleSequenceItem">
						<xsl:sort select="@SequencePositionNumber" data-type="number"/>
						<orm:Role ref="GUID_RoleID{@RoleSequenceItemRoleID}"/>
					</xsl:for-each>
				</orm:RoleSequence>
			</xsl:for-each>
		</orm:DisjunctiveMandatoryConstraint>
	</xsl:template>
	<xsl:template match="Constraint[@IsInternal='false' and @ConstraintType='Exclusion']">
		<orm:ExclusionConstraint id="GUID_ExternalConstraingID{@ConstraintID}" Name="ExternalConstraintID{@ConstraintID}">
			<orm:RoleSequences>
				<xsl:for-each select="RoleSequences/RoleSequence">
					<orm:RoleSequence id="GUID_RoleSequenceID{../../@ConstraintID}_{@SequenceNumber}">
						<xsl:for-each select="RoleSequenceItems/RoleSequenceItem">
							<xsl:sort select="@SequencePositionNumber" data-type="number"/>
							<orm:Role ref="GUID_RoleID{@RoleSequenceItemRoleID}"/>
						</xsl:for-each>
					</orm:RoleSequence>
				</xsl:for-each>
			</orm:RoleSequences>
		</orm:ExclusionConstraint>
	</xsl:template>
	<xsl:template match="Constraint[@ConstraintType='Ring']">
		<orm:RingConstraint id="GUID_RingConstraintID{@ConstraintID}" Name="RingConstraintID{@ConstraintID}" Type="{@RingConstraintType}">
			<xsl:for-each select="RoleSequences/RoleSequence">
				<orm:RoleSequence>
					<xsl:for-each select="RoleSequenceItems/RoleSequenceItem">
						<xsl:sort select="@SequencePositionNumber" data-type="number"/>
						<orm:Role ref="GUID_RoleID{@RoleSequenceItemRoleID}"/>
					</xsl:for-each>
				</orm:RoleSequence>
			</xsl:for-each>
		</orm:RingConstraint>
	</xsl:template>
	<xsl:template match="Constraint[@ConstraintType='Equality']">
		<orm:EqualityConstraint id="GUID_EqualityConstraintID{@ConstraintID}" Name="EqualityConstraintID{@ConstraintID}">
			<orm:RoleSequences>
				<xsl:for-each select="RoleSequences/RoleSequence">
					<orm:RoleSequence>
						<xsl:attribute name="id">
							<xsl:text>GUID_EqualityConstraintID</xsl:text>
							<xsl:value-of select="../../@ConstraintID"/>
							<xsl:text>_Role</xsl:text>
							<xsl:value-of select="position()"/>
						</xsl:attribute>
						<xsl:for-each select="RoleSequenceItems/RoleSequenceItem">
							<xsl:sort select="@SequencePositionNumber" data-type="number"/>
							<orm:Role ref="GUID_RoleID{@RoleSequenceItemRoleID}"/>
						</xsl:for-each>
					</orm:RoleSequence>
				</xsl:for-each>
			</orm:RoleSequences>
		</orm:EqualityConstraint>
	</xsl:template>
	<xsl:template match="Constraint[@ConstraintType='Subset']">
		<orm:SubsetConstraint id="GUID_SubsetConstraintID{@ConstraintID}" Name="SubsetConstraintID{@ConstraintID}">
			<orm:RoleSequences>
				<xsl:for-each select="RoleSequences/RoleSequence">
					<orm:RoleSequence id="GUID_RoleSequenceID{../../@ConstraintID}_{@SequenceNumber}">
						<xsl:for-each select="RoleSequenceItems/RoleSequenceItem">
							<orm:Role ref="GUID_RoleID{@RoleSequenceItemRoleID}" />
						</xsl:for-each>
					</orm:RoleSequence>
				</xsl:for-each>
			</orm:RoleSequences>
		</orm:SubsetConstraint>
	</xsl:template>
	<xsl:template match="Constraint[@ConstraintType='Frequency']">
		<orm:FrequencyConstraint id="GUID_FrequencyConstraintID{@ConstraintID}" Name="" MinFrequency="{@MinimumFrequency}" MaxFrequency="{@MaximumFrequency}">
			<xsl:for-each select="RoleSequences/RoleSequence">
				<orm:RoleSequence>
					<xsl:for-each select="RoleSequenceItems/RoleSequenceItem">
						<orm:Role ref="GUID_RoleID{@RoleSequenceItemRoleID}" />
					</xsl:for-each>
				</orm:RoleSequence>
			</xsl:for-each>
		</orm:FrequencyConstraint>
	</xsl:template>
	<!--End External Constraints Template-->
	<!--Begin DataType Templates-->
	<xsl:template name="DataTypes">
		<orm:DataTypes>
			<xsl:variable name="uniqueConceptualDatatypesFragment">
				<xsl:variable name="allValueTypes">
					<xsl:for-each select="Objects/Object[@ObjectKind='Value Type']">
						<xsl:sort select="@ConceptualDatatype"/>
						<xsl:copy-of select="."/>
					</xsl:for-each>
				</xsl:variable>
				<xsl:copy-of select="exsl:node-set($allValueTypes)/child::*[position()=last() or following-sibling::*[1]/@ConceptualDatatype!=@ConceptualDatatype]"/>
			</xsl:variable>
			<xsl:variable name="uniqueConceptualDatatypes" select="exsl:node-set($uniqueConceptualDatatypesFragment)/child::*"/>
			<xsl:variable name="uniqueConceptualDatatypes2Fragment">
				<xsl:for-each select="$uniqueConceptualDatatypes">
					<xsl:choose>
						<xsl:when test="position()=1 and contains(@ConceptualDatatype, '(')">
							<xsl:copy>
								<xsl:copy-of select="substring-before(@ConceptualDatatype, '(')"/>
							</xsl:copy>
						</xsl:when>
						<xsl:when test="position()!=1 and contains(@ConceptualDatatype, '(')">
							<xsl:if test="substring-before(@ConceptualDatatype, '(')!=substring-before(preceding-sibling::*[1]/@ConceptualDatatype, '(')">
								<xsl:copy>
									<xsl:copy-of select="substring-before(@ConceptualDatatype, '(')"/>
								</xsl:copy>
							</xsl:if>
						</xsl:when>
						<xsl:otherwise>
							<xsl:copy>
								<xsl:value-of select="@ConceptualDatatype"/>
							</xsl:copy>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:for-each>
			</xsl:variable>
			<xsl:variable name="uniqueConceptualDatatypes2" select="exsl:node-set($uniqueConceptualDatatypes2Fragment)/child::*"/>
			<xsl:for-each select="$uniqueConceptualDatatypes2">
				<xsl:choose>
					<xsl:when test="contains(., 'R-Fixed Length')">
						<orm:FixedLengthRawDataDataType id="GUID_FixedLengthRawDataDataType"/>
					</xsl:when>
					<xsl:when test="contains(., 'N-Floating Point')">
						<orm:FloatingPointNumericDataType id="GUID_FloatingPointNumericDataType"/>
					</xsl:when>
					<xsl:when test="contains(., 'N-Decimal')">
						<orm:DecimalNumericDataType id="GUID_DecimalNumericDataType"/>
					</xsl:when>
					<xsl:when test="contains(., 'C-Large Length')">
						<orm:LargeLengthTextDataType id="GUID_LargeLengthTextDataType"/>
					</xsl:when>
					<xsl:when test="contains(., 'R-OLE Object')">
						<orm:OleObjectRawDataDataType id="GUID_OleObjectRawDataDataType"/>
					</xsl:when>
					<xsl:when test="contains(., 'N-Signed Integer')">
						<orm:SignedIntegerNumericDataType id="GUID_SignedIntegerNumericDataType"/>
					</xsl:when>
					<xsl:when test="contains(., 'N-Unsigned Integer')">
						<orm:UnsignedIntegerNumericDataType id="GUID_UnsignedIntegerNumericDataType"/>
					</xsl:when>
					<xsl:when test="contains(., 'T-Date &amp; Time')">
						<orm:DateAndTimeTemporalDataType id="GUID_DateAndTimeTemporalDataType"/>
					</xsl:when>
					<xsl:when test="contains(., 'N-Money')">
						<orm:MoneyNumericDataType id="GUID_MoneyNumericDataType"/>
					</xsl:when>
					<xsl:when test="contains(., 'N-Auto Counter')">
						<orm:AutoCounterNumericDataType id="GUID_AutoCounterNumericDataType"/>
					</xsl:when>
					<xsl:when test="contains(., 'C-Fixed Length')">
						<orm:FixedLengthTextDataType id="GUID_FixedLengthTextDataType"/>
					</xsl:when>
					<xsl:when test="contains(., 'L-True or False')">
						<orm:TrueOrFalseLogicalDataType id="GUID_TrueOrFalseLogicalDataType"/>
					</xsl:when>
					<xsl:when test="contains(., 'C-Variable Length')">
						<orm:VariableLengthTextDataType id="GUID_VariableLengthTextDataType"/>
					</xsl:when>
					<xsl:when test="contains(., 'R-Variable Length')">
						<orm:VariableLengthRawDataDataType id="GUID_VariableLengthRawDataDataType"/>
					</xsl:when>
					<xsl:otherwise>
						<orm:UnspecifiedDataType id="GUID_UnspecifiedDataType"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:for-each>
		</orm:DataTypes>
	</xsl:template>
	<!--End DataType Templates-->
	<!--Begin ReferenceModeKinds Templates-->
	<xsl:template name="ReferenceModeKinds">
		<orm:ReferenceModeKinds>
			<orm:ReferenceModeKind id="RMKind_1" ReferenceModeType="Popular">
				<xsl:attribute name="FormatString">
					<xsl:text>{0}{1}</xsl:text>
				</xsl:attribute>
			</orm:ReferenceModeKind>
		</orm:ReferenceModeKinds>
	</xsl:template>
	<!--End ReferenceModeKinds Templates-->
</xsl:stylesheet>