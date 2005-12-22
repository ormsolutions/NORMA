<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:orm="http://schemas.neumont.edu/ORM/ORMCore" exclude-result-prefixes="#default xsl msxsl">
	<!--<xsl:output method="xml" indent="yes" />-->
	<!--We shouldn't have to do this.  For some reason the transform won't run properly without it.-->
	<xsl:template match="/">
		<xsl:apply-templates select="VisioModels/ORMSourceModels/ORMSourceModel"/>
	</xsl:template>
	<xsl:template match="/VisioModels/ORMSourceModels/ORMSourceModel">
		<orm:ORMModel>
			<xsl:attribute name="id">
				<xsl:text>GUID_ModelID</xsl:text>
			</xsl:attribute>
			<xsl:attribute name="Name">
				<xsl:text>importedModel</xsl:text>
			</xsl:attribute>
			<xsl:apply-templates select="Objects"/>
			<xsl:apply-templates select="Facts"/>
			<!--TODO: Need to add aditional external constraints.-->
			<orm:ExternalConstraints>
				<xsl:variable name="externalConstraintsFragment">
					<xsl:for-each select="Constraints/Constraint[@IsInternal='false' or @ConstraintType='Ring']">
						<xsl:copy-of select="."/>
					</xsl:for-each>
				</xsl:variable>
				<xsl:variable name="externalConstraints" select="msxsl:node-set($externalConstraintsFragment)/child::*"/>
				<xsl:for-each select="$externalConstraints">
					<xsl:variable name="constriantID" select="@ConstraintID"/>
					<xsl:choose>
						<xsl:when test="@ConstraintType='Uniqueness'">
							<orm:ExternalUniquenessConstraint id="GUID_ExternalConstraintID{$constriantID[1]}" Name="ExternalConstraintID{$constriantID[1]}">
								<xsl:for-each select="RoleSequences/RoleSequence">
									<orm:RoleSequence>
										<xsl:for-each select="RoleSequenceItems/RoleSequenceItem">
											<xsl:sort select="@SequencePositionNumber" data-type="number"/>
											<orm:Role ref="GUID_RoleID{@RoleSequenceItemRoleID}"/>
										</xsl:for-each>
									</orm:RoleSequence>
								</xsl:for-each>
							</orm:ExternalUniquenessConstraint>
						</xsl:when>
						<xsl:when test="@ConstraintType='Exclusion'">
							<orm:ExclusionConstraint id="GUID_ExternalConstraingID{$constriantID}" Name="ExternalConstraintID{$constriantID}">
								<orm:RoleSequences>
									<xsl:for-each select="RoleSequences/RoleSequence">
										<orm:RoleSequence id="GUID_RoleSequenceID{$constriantID}_{@SequenceNumber}">
											<xsl:for-each select="RoleSequenceItems/RoleSequenceItem">
												<xsl:sort select="@SequencePositionNumber" data-type="number"/>
												<orm:Role ref="GUID_RoleID{@RoleSequenceItemRoleID}"/>
											</xsl:for-each>
										</orm:RoleSequence>
									</xsl:for-each>
								</orm:RoleSequences>
							</orm:ExclusionConstraint>
						</xsl:when>
						<xsl:when test="@ConstraintType='Mandatory'">
							<orm:DisjunctiveMandatoryConstraint id="GUID_ExternalConstraintID{$constriantID}" Name="ExternalConstraintID{$constriantID}">
								<xsl:for-each select="RoleSequences/RoleSequence">
									<orm:RoleSequence>
										<xsl:for-each select="RoleSequenceItems/RoleSequenceItem">
											<xsl:sort select="@SequencePositionNumber" data-type="number"/>
											<orm:Role ref="GUID_RoleID{@RoleSequenceItemRoleID}"/>
										</xsl:for-each>
									</orm:RoleSequence>
								</xsl:for-each>
							</orm:DisjunctiveMandatoryConstraint>
						</xsl:when>
						<!--We Currently don't handle ring constraints.-->
						<!--<xsl:when test="@ConstraintType='Ring'">
								<orm:RingConstraint id="GUID_RingConstraintID{$constriantID}" Name="RingConstraintID{$constriantID}">
									<xsl:for-each select="RoleSequences/RoleSequence">
										<orm:RoleSequence>
											<xsl:for-each select="RoleSequenceItems/RoleSequenceItem">
												<xsl:sort select="@SequencePositionNumber" data-type="number"/>
												<orm:Role ref="GUID_RoleID{@RoleSequenceItemRoleID}"/>
											</xsl:for-each>
										</orm:RoleSequence>
									</xsl:for-each>
								</orm:RingConstraint>
							</xsl:when>-->
					</xsl:choose>
				</xsl:for-each>
				<!--<xs:element name="EqualityConstraint" type="EqualityConstraint"/>
						<xs:element name="ImpliedEqualityConstraint" type="ImpliedEqualityConstraint"/>
						<xs:element name="SubsetConstraint" type="SubsetConstraint"/>
						<xs:element name="FrequencyConstraint" type="FrequencyConstraint"/>
						<xs:element name="ImpliedExternalUniquenessConstraint" type="ImpliedExternalUniquenessConstraint"/>-->
			</orm:ExternalConstraints>
			<orm:DataTypes>
				<xsl:variable name="uniqueConceptualDatatypes2Fragment">
					<xsl:variable name="allValueTypes">
						<xsl:for-each select="Objects/Object[@ObjectKind='Value Type']">
							<xsl:sort select="@ConceptualDatatype"/>
							<xsl:copy-of select="."/>
						</xsl:for-each>
					</xsl:variable>
					<xsl:copy-of select="msxsl:node-set($allValueTypes)/child::*[position()=last() or following-sibling::*[1]/@ConceptualDatatype!=@ConceptualDatatype]"/>
				</xsl:variable>
				<xsl:variable name="uniqueConceptualDatatypes2" select="msxsl:node-set($uniqueConceptualDatatypes2Fragment)/child::*"/>
				<xsl:variable name="uniqueConceptualDatatypesFragment3">
					<xsl:for-each select="$uniqueConceptualDatatypes2">
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
				<xsl:variable name="uniqueConceptualDatatypes3" select="msxsl:node-set($uniqueConceptualDatatypesFragment3)/child::*"/>
				<xsl:for-each select="$uniqueConceptualDatatypes3">
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
			<orm:ReferenceModeKinds>
				<orm:ReferenceModeKind id="RMKind_1" ReferenceModeType="Popular">
					<xsl:attribute name="FormatString">
						<xsl:text>{0}{1}</xsl:text>
					</xsl:attribute>
				</orm:ReferenceModeKind>
			</orm:ReferenceModeKinds>
		</orm:ORMModel>
	</xsl:template>
	<xsl:template match="Objects">
		<xsl:element name="orm:Objects">
			<xsl:apply-templates select="Object[@ObjectKind='Entity Type' and not(@NestedPredicateFactID)]"/>
			<xsl:apply-templates select="Object[@ObjectKind='Entity Type' and @NestedPredicateFactID]"/>
			<xsl:apply-templates select="Object[@ObjectKind='Value Type']"/>
		</xsl:element>
	</xsl:template>
	<xsl:template match="Object[@ObjectKind='Entity Type' and not(@NestedPredicateFactID)]">
		<xsl:element name="orm:EntityType" >
			<xsl:apply-templates select="@ObjectID"/>
			<xsl:apply-templates select="@ObjectName"/>
			<xsl:apply-templates select="@IsIndependent"/>
			<xsl:apply-templates select="@IsExternal"/>
			<xsl:apply-templates select="@IsPersonal"/>
			<xsl:apply-templates select="@ReferenceMode"/>
			<xsl:apply-templates select="ObjectNotes"/>
			<xsl:apply-templates select="Supertypes"/>
		</xsl:element>
	</xsl:template>
	<xsl:template name="ObjectifiedObjects" match="Object[@ObjectKind='Entity Type' and @NestedPredicateFactID]" >
		<xsl:element name="orm:ObjectifiedType" >
			<xsl:apply-templates select="@ObjectID"/>
			<xsl:apply-templates select="@ObjectName"/>
			<xsl:apply-templates select="@IsIndependent"/>
			<xsl:apply-templates select="@IsExternal"/>
			<xsl:apply-templates select="@IsPersonal"/>
			<xsl:apply-templates select="ObjectNotes"/>
			<xsl:apply-templates select="Supertypes"/>
			<xsl:apply-templates select="@NestedPredicateFactID"/>
		</xsl:element>
	</xsl:template>
	<xsl:template match="Object[@ObjectKind='Value Type']">
		<xsl:element name="orm:ValueType">
			<xsl:apply-templates select="@ObjectID"/>
			<xsl:apply-templates select="@ObjectName"/>
			<xsl:apply-templates select="@IsIndependent"/>
			<xsl:apply-templates select="@IsExternal"/>
			<xsl:apply-templates select="@IsPersonal"/>
			<xsl:apply-templates select="ObjectNotes"/>
			<xsl:apply-templates select="Supertypes"/>
			<xsl:apply-templates select="@ConceptualDatatype"/>
		</xsl:element>
	</xsl:template>
	<xsl:template match="ObjectNotes">
		<xsl:element name="orm:ObjectNotes">
			<xsl:element xml:space="default" name="orm:Data">
				<xsl:value-of select ="."/>
			</xsl:element>
		</xsl:element>
	</xsl:template>
	<xsl:template match="Supertypes">
		<xsl:element name="orm:Supertypes">
			<xsl:apply-templates select="Supertype"/>
		</xsl:element>
	</xsl:template>
	<xsl:template match="Supertype">
		<xsl:element name="orm:Object">
			<xsl:attribute name="ref">
				<xsl:apply-templates select="@SupertypeObjectID"/>
			</xsl:attribute>
		</xsl:element>
	</xsl:template>
	<xsl:template match="@SupertypeObjectID">
		<xsl:text>GUID_ObjectID</xsl:text>
		<xsl:value-of select="."/>
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
		<xsl:copy/>
	</xsl:template>
	<xsl:template match="@IsExternal">
		<xsl:copy/>
	</xsl:template>
	<xsl:template match="@IsPersonal">
		<xsl:copy/>
	</xsl:template>
	<xsl:template match="@ReferenceMode">
		<xsl:attribute name="_ReferenceMode">
			<xsl:text>id</xsl:text>
		</xsl:attribute>
	</xsl:template>
	<xsl:template match="@NestedPredicateFactID">
		<xsl:element name="orm:NestedPredicate">
			<xsl:attribute name="ref">
				<xsl:text>GUID_FactID</xsl:text>
				<xsl:value-of select="."/>
			</xsl:attribute>
		</xsl:element>
	</xsl:template>
	<xsl:template match="@ConceptualDatatype">
		<xsl:element name="orm:ConceptualDataType">
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
					<xsl:when test="contains($lengthScale, ',')">
						<xsl:value-of select="substring-before($lengthScale, ',')"/>
					</xsl:when>
					<xsl:when test="not(contains($lengthScale, ','))">
						<xsl:value-of select="'0'"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$lengthScale"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
		</xsl:element>
	</xsl:template>
	<xsl:template match="Facts">
		<xsl:element name="orm:Facts">
			<xsl:apply-templates select="Fact"/>
		</xsl:element>
	</xsl:template>
	<xsl:template match="Fact">
		<xsl:element name="orm:Fact">
			<xsl:attribute name="id">
				<xsl:text>GUID_FactID</xsl:text>
				<xsl:value-of select="@FactID"/>
			</xsl:attribute>
			<xsl:attribute name="Name">
				<xsl:text>FactID</xsl:text>
				<xsl:value-of select="@FactID"/>
			</xsl:attribute>
			<xsl:apply-templates select="@IsExternal"/>
			<xsl:apply-templates select="FactRoles"/>
			<orm:ReadingOrders>
				<xsl:for-each select="FactReadings/FactReading">
					<xsl:variable name="location" select="position()"/>
					<xsl:variable name="factRoleID" select="../../FactRoles/FactRole[$location]/@FactRoleID"/>
					<xsl:variable name="role" select="../../../../Roles/Role[@RoleID=$factRoleID]"/>
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
									<xsl:text>{0} </xsl:text>
									<xsl:value-of select="$role/@RoleReading"/>
									<xsl:if test="count(../../FactRoles/FactRole) = 2">
										<xsl:text> {1}</xsl:text>
									</xsl:if>
								</orm:Data>
							</orm:Reading>
						</orm:Readings>
						<orm:RoleSequence>
							<xsl:variable name="factRoleIDs" select="../../FactRoles/FactRole/@FactRoleID"/>
							<xsl:variable name="factRoleIDsAscendingFrag">
								<xsl:for-each select="../../../../Roles/Role[@RoleID=$factRoleIDs]">
									<xsl:sort select="@PredicatePosition" data-type="number" order="ascending"/>
									<xsl:copy-of select="."/>
								</xsl:for-each>
							</xsl:variable>
							<xsl:variable name="factRoleIDsDescendingFrag">
								<xsl:for-each select="../../../../Roles/Role[@RoleID=$factRoleIDs]">
									<xsl:sort select="@PredicatePosition" data-type="number" order="descending"/>
									<xsl:copy-of select="."/>
								</xsl:for-each>
							</xsl:variable>
							<xsl:variable name="factRoleIDsAscending" select="msxsl:node-set($factRoleIDsAscendingFrag)/child::*"/>
							<xsl:variable name="factRoleIDsDescending" select="msxsl:node-set($factRoleIDsDescendingFrag)/child::*"/>
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
				</xsl:for-each>
			</orm:ReadingOrders>
			<xsl:apply-templates select="FactConstraints"/>
		</xsl:element>
	</xsl:template>
	<xsl:template match="FactRoles">
		<xsl:element name="orm:FactRoles">
			<xsl:apply-templates select="FactRole"/>
		</xsl:element>
	</xsl:template>
	<xsl:template match="FactRole">
		<xsl:element name="orm:Role">
			<xsl:attribute name="id">
				<xsl:text>GUID_RoleID</xsl:text>
				<xsl:value-of select ="@FactRoleID"/>
			</xsl:attribute>
			<xsl:attribute name="Name">
				<xsl:text>RoleID</xsl:text>
				<xsl:value-of select ="@FactRoleID"/>
			</xsl:attribute>
			<xsl:attribute name="_IsMandatory">
				<xsl:value-of select="../../../../Roles/Role[@RoleID=current()/@FactRoleID]/@IsMandatory"/>
			</xsl:attribute>
			<xsl:attribute name="_Multiplicity">
				<!--This is a calculated field the loader ignores it anyway.-->
				<xsl:text>Unspecified</xsl:text>
			</xsl:attribute>
			<xsl:element name="orm:RolePlayer">
				<xsl:attribute name="ref">
					<xsl:text>GUID_ObjectID</xsl:text>
					<xsl:value-of select="../../../../Roles/Role[@RoleID=current()/@FactRoleID]/@RolePlayerObjectID"/>
				</xsl:attribute>
			</xsl:element>
		</xsl:element>
	</xsl:template>
	<xsl:template match="FactConstraints">
		<orm:InternalConstraints>
			<xsl:apply-templates select="FactConstraint"/>
		</orm:InternalConstraints>
	</xsl:template>
	<xsl:template match="FactConstraint">
		<xsl:variable name="constraintFragment" select="../../../../Constraints/Constraint[@ConstraintID=current()/@FactConstraintID]"/>
		<xsl:choose>
			<xsl:when test="$constraintFragment[@ConstraintID=current()/@FactConstraintID and @IsInternal='true' and @ConstraintType='Uniqueness']">
				<orm:InternalUniquenessConstraint>
					<xsl:attribute name="id">
						<xsl:text>GUID_ConstraintID</xsl:text>
						<xsl:value-of select="$constraintFragment/@ConstraintID"/>
					</xsl:attribute>
					<xsl:attribute name="Name">
						<xsl:text>InternalUniquenessConstraint</xsl:text>
						<xsl:value-of select="$constraintFragment/@ConstraintID"/>
					</xsl:attribute>
					<orm:RoleSequence>
						<orm:Role>
							<xsl:attribute name="ref">
								<xsl:text>GUID_RoleID</xsl:text>
								<xsl:value-of select="$constraintFragment/RoleSequences/RoleSequence/RoleSequenceItems/RoleSequenceItem/@RoleSequenceItemRoleID"/>
							</xsl:attribute>
						</orm:Role>
					</orm:RoleSequence>
					<xsl:if test="$constraintFragment/@IsPrimaryReference='true'">
						<!--Pretty dirty here.  I had to do it because Orthogonal
							references the prefered identifier by an attribute called @ObjectNamespace
							not the Object ID. So all of this is to get the ObjectID of the prefered Identifier.-->
						<xsl:variable name="objectIDFragment" select="../../../../Roles/Role[@RoleID=$constraintFragment/RoleSequences/RoleSequence/RoleSequenceItems/RoleSequenceItem/@RoleSequenceItemRoleID]/@RolePlayerObjectID"/>
						<xsl:variable name="objectNamespace" select="../../../../Objects/Object[@ObjectID=$objectIDFragment]/@ObjectNamespace"/>
						<orm:PreferredIdentifierFor>
							<xsl:attribute name="ref">
								<xsl:text>GUID_ObjectID</xsl:text>
								<xsl:value-of select="../../../../Objects/Object[@ObjectName=$objectNamespace]/@ObjectID"/>
							</xsl:attribute>
						</orm:PreferredIdentifierFor>
					</xsl:if>
				</orm:InternalUniquenessConstraint>
			</xsl:when>
			<xsl:when test="$constraintFragment[@ConstraintID=current()/@FactConstraintID and @IsInternal='true' and @ConstraintType='Mandatory']">
				<orm:SimpleMandatoryConstraint>
					<xsl:attribute name="id">
						<xsl:text>GUID_ConstraintID</xsl:text>
						<xsl:value-of select="$constraintFragment/@ConstraintID"/>
					</xsl:attribute>
					<xsl:attribute name="Name">
						<xsl:text>SimpleMandatoryConstraint</xsl:text>
						<xsl:value-of select="$constraintFragment/@ConstraintID"/>
					</xsl:attribute>
					<orm:RoleSequence>
						<orm:Role>
							<xsl:attribute name="ref">
								<xsl:text>GUID_RoleID</xsl:text>
								<xsl:value-of select="$constraintFragment/RoleSequences/RoleSequence/RoleSequenceItems/RoleSequenceItem/@RoleSequenceItemRoleID"/>
							</xsl:attribute>
						</orm:Role>
					</orm:RoleSequence>
				</orm:SimpleMandatoryConstraint>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>