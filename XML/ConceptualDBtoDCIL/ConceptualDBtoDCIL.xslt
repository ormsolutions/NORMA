<?xml version="1.0" encoding="utf-8"?>
<!--
	Copyright © Neumont University. All rights reserved.

	This software is provided 'as-is', without any express or implied warranty. In no event will the authors be held liable for any damages arising from the use of this software.
	Permission is granted to anyone to use this software for any purpose, including commercial applications, and to alter it and redistribute it freely, subject to the following restrictions:
	1. The origin of this software must not be misrepresented; you must not claim that you wrote the original software. If you use this software in a product, an acknowledgment in the product documentation would be appreciated but is not required.
	2. Altered source versions must be plainly marked as such, and must not be misrepresented as being the original software.
	3. This notice may not be removed or altered from any source distribution.
-->
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:exsl="http://exslt.org/common"
	xmlns:dsf="urn:schemas-orm-net:DIL:DILSupportFunctions"
	xmlns:orm="http://schemas.neumont.edu/ORM/2006-04/ORMCore"
	xmlns:ormRoot="http://schemas.neumont.edu/ORM/2006-04/ORMRoot"
	xmlns:ormtooial="http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMToORMAbstraction"
	xmlns:oial="http://schemas.neumont.edu/ORM/Abstraction/2007-06/Core"
	xmlns:odt="http://schemas.neumont.edu/ORM/Abstraction/2007-06/DataTypes/Core"
	xmlns:cdb="http://schemas.neumont.edu/ORM/Relational/2007-06/ConceptualDatabase"
	xmlns:oialtocdb="http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMAbstractionToConceptualDatabase"
	xmlns:dcl="http://schemas.orm.net/DIL/DCIL"
	xmlns:dil="http://schemas.orm.net/DIL/DIL"
	xmlns:ddt="http://schemas.orm.net/DIL/DILDT"
	xmlns:dep="http://schemas.orm.net/DIL/DILEP"
	xmlns:dml="http://schemas.orm.net/DIL/DMIL"
	xmlns:ddl="http://schemas.orm.net/DIL/DDIL"
	extension-element-prefixes="exsl dsf"
	exclude-result-prefixes="orm ormRoot ormtooial oial odt cdb oialtocdb">

	<xsl:import href="../../DIL/Transforms/DILSupportFunctions.xslt"/>

	<xsl:output method="xml" encoding="utf-8" media-type="text/xml" indent="yes"/>
	<xsl:strip-space elements="*"/>

	<!-- These aren't used yet, but it doesn't hurt to leave them here for now. --> 
	<xsl:param name="SMALLINT_MinValue" select="number(-32768)"/>
	<xsl:param name="SMALLINT_MaxValue" select="number(32767)"/>
	<xsl:param name="INTEGER_MinValue" select="number(-2147483648)"/>
	<xsl:param name="INTEGER_MaxValue" select="number(2147483647)"/>
	<xsl:param name="BIGINT_MinValue" select="number(-9223372036854775808)"/>
	<xsl:param name="BIGINT_MaxValue" select="number(9223372036854775807)"/>

	<xsl:template match="ormRoot:ORM2">
		<xsl:apply-templates select=".//cdb:Schema"/>
	</xsl:template>
	
	<xsl:template match="cdb:Schema">

		<xsl:variable name="oialDcilBridge" select="//oialtocdb:Bridge"/>
		<xsl:variable name="ormOialBridge" select="//ormtooial:Bridge"/>
		<xsl:variable name="oialModel" select="//oial:model[@id = $oialDcilBridge/oialtocdb:SchemaIsForAbstractionModel[@Schema = current()/@id]/@AbstractionModel]"/>
		<xsl:variable name="ormModel" select="//orm:ORMModel[@id = $ormOialBridge/ormtooial:AbstractionModelIsForORMModel[@AbstractionModel = $oialModel/@id]/@ORMModel]"/>
		
		<xsl:variable name="mappedValueTypes" select="$ormModel/orm:Objects/orm:ValueType[@id = $ormOialBridge/ormtooial:InformationTypeFormatIsForValueType[@InformationTypeFormat = $oialModel/oial:informationTypeFormats/child::*/@id]/@ValueType]"/>
		<xsl:variable name="initialDataTypeMappingsFragment">
			<xsl:for-each select="$mappedValueTypes">
				<DataTypeMapping id="{@id}">
					<xsl:apply-templates select="." mode="GenerateDataTypeMapping">
						<xsl:with-param name="ormModel" select="$ormModel"/>
					</xsl:apply-templates>
				</DataTypeMapping>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="initialDataTypeMappings" select="exsl:node-set($initialDataTypeMappingsFragment)/child::*"/>
		<xsl:variable name="dataTypeMappingsFragment">
			<xsl:for-each select="$initialDataTypeMappings">
				<xsl:copy>
					<xsl:copy-of select="@*"/>
					<xsl:choose>
						<xsl:when test="dcl:domain">
							<dcl:domainRef name="{dcl:domain/@name}"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:copy-of select="*"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:copy>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="dataTypeMappings" select="exsl:node-set($dataTypeMappingsFragment)/child::*"/>

		<dcl:schema name="{dsf:makeValidIdentifier(@Name)}">
			<xsl:copy-of select="$initialDataTypeMappings/dcl:domain"/>
			<xsl:apply-templates mode="GenerateSchemaContent" select="cdb:Tables/cdb:Table">
				<xsl:with-param name="oialDcilBridge" select="$oialDcilBridge"/>
				<xsl:with-param name="ormOialBridge" select="$ormOialBridge"/>
				<xsl:with-param name="oialModel" select="$oialModel"/>
				<xsl:with-param name="ormModel" select="$ormModel"/>
				<xsl:with-param name="dataTypeMappings" select="$dataTypeMappings"/>
			</xsl:apply-templates>
		</dcl:schema>
	</xsl:template>

	<xsl:template match="cdb:Table" mode="GenerateSchemaContent">
		<xsl:param name="oialDcilBridge"/>
		<xsl:param name="ormOialBridge"/>
		<xsl:param name="oialModel"/>
		<xsl:param name="ormModel"/>
		<xsl:param name="dataTypeMappings"/>
		<dcl:table name="{dsf:makeValidIdentifier(@Name)}">
			<xsl:variable name="uniquenessConstraints" select="cdb:Constraints/cdb:UniquenessConstraint"/>
			<xsl:apply-templates mode="GenerateTableContent" select="cdb:Columns/cdb:Column">
				<xsl:sort data-type="number" select="number(not(boolean(@id = $uniquenessConstraints[@IsPrimary='true' or @IsPrimary=1]/cdb:Columns/cdb:Column/@ref)))"/>
				<xsl:sort data-type="number" select="number(boolean(@IsNullable='true' or @IsNullable=1))"/>
				<xsl:sort data-type="number" select="number(not(boolean(@id = $uniquenessConstraints[not(@IsPrimary='true' or @IsPrimary=1)]/cdb:Columns/cdb:Column/@ref)))"/>
				<xsl:with-param name="oialDcilBridge" select="$oialDcilBridge"/>
				<xsl:with-param name="ormOialBridge" select="$ormOialBridge"/>
				<xsl:with-param name="oialModel" select="$oialModel"/>
				<xsl:with-param name="ormModel" select="$ormModel"/>
				<xsl:with-param name="dataTypeMappings" select="$dataTypeMappings"/>
			</xsl:apply-templates>
			<xsl:apply-templates mode="GenerateTableContent" select="cdb:Constraints/cdb:*"/>
		</dcl:table>
	</xsl:template>

	<xsl:template match="cdb:Column" mode="GenerateTableContent">
		<xsl:param name="oialDcilBridge"/>
		<xsl:param name="ormOialBridge"/>
		<xsl:param name="oialModel"/>
		<xsl:param name="ormModel"/>
		<xsl:param name="dataTypeMappings"/>
		<dcl:column name="{dsf:makeValidIdentifier(@Name)}" isNullable="{@IsNullable='true' or @IsNullable=1}" isIdentity="false">
			<xsl:variable name="conceptTypeChildPath" select="$oialDcilBridge/oialtocdb:ColumnHasConceptTypeChild[@Column = current()/@id]"/>
			<xsl:variable name="valueTypeId">
				<xsl:variable name="informationTypeId" select="$conceptTypeChildPath[last()]/@ConceptTypeChild"/>
				<xsl:variable name="factTypeId" select="$ormOialBridge/ormtooial:ConceptTypeChildHasPathFactType[@ConceptTypeChild = $informationTypeId][last()]/@PathFactType"/>
				<xsl:choose>
					<xsl:when test="string-length($factTypeId)">
						<xsl:variable name="factTypeMapping" select="$ormOialBridge/ormtooial:FactTypeMapsTowardsRole[@FactType = $factTypeId]"/>
						<xsl:variable name="factType" select="$ormModel/orm:Facts/orm:*[@id = $factTypeId]"/>
						<xsl:variable name="roleOrProxy" select="$factType/orm:FactRoles/orm:*[not(@id = $factTypeMapping/@TowardsRole)]"/>
						<xsl:if test="count($roleOrProxy) != 1">
							<xsl:message terminate="yes">
								<xsl:text>SANITY CHECK: Found no or multiple roles for column "</xsl:text>
								<xsl:value-of select="@Name"/>
								<xsl:text>" in table "</xsl:text>
								<xsl:value-of select="parent::cdb:Columns/parent::cdb:Table/@Name"/>
								<xsl:text>".</xsl:text>
							</xsl:message>
						</xsl:if>
						<xsl:choose>
							<xsl:when test="$roleOrProxy/self::orm:Role">
								<xsl:value-of select="$roleOrProxy/orm:RolePlayer/@ref"/>
							</xsl:when>
							<xsl:when test="$roleOrProxy/self::orm:RoleProxy">
								<xsl:value-of select="$ormModel/orm:Facts/orm:*/orm:FactRoles/orm:Role[@id = $roleOrProxy/orm:Role/@ref]/orm:RolePlayer/@ref"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:message terminate="yes">
									<xsl:text>SANITY CHECK: Unexpected type of role (</xsl:text>
									<xsl:value-of select="local-name($roleOrProxy)"/>
									<xsl:text>).</xsl:text>
								</xsl:message>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:variable name="conceptTypeId" select="$oialModel/oial:conceptTypes/oial:conceptType[oial:children/oial:informationType/@id = $informationTypeId]/@id"/>
						<xsl:variable name="objectTypeId" select="$ormOialBridge/ormtooial:ConceptTypeIsForObjectType[@ConceptType = $conceptTypeId]/@ObjectType"/>
						<xsl:if test="not($ormModel/orm:Objects/orm:ValueType[@id = $objectTypeId])">
							<xsl:message terminate="yes">
								<xsl:text>SANITY CHECK: Found no roles and no value type for column "</xsl:text>
								<xsl:value-of select="@Name"/>
								<xsl:text>" in table "</xsl:text>
								<xsl:value-of select="parent::cdb:Columns/parent::cdb:Table/@Name"/>
								<xsl:text>".</xsl:text>
							</xsl:message>
						</xsl:if>
						<xsl:value-of select="$objectTypeId"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>

			<xsl:variable name="dataTypeMapping" select="$dataTypeMappings[@id = $valueTypeId]"/>
			<xsl:if test="
				count($conceptTypeChildPath) = 1 or
				(not($oialModel/oial:conceptTypes/oial:conceptType/oial:children/oial:*[@id = $conceptTypeChildPath/@ConceptTypeChild and self::oial:relatedConceptType]) and
				not(parent::cdb:Columns/parent::cdb:Table/cdb:Constraints/cdb:ReferenceConstraint/cdb:ColumnReferences/cdb:ColumnReference[@SourceColumn = current()/@id]))">
				<!--
					Only set the column as being an identity column if we have a single entry in the concept type child path,
					or (we have no concept type relations in the path, and no reference constraints coming from this column).
				-->
				<!--
					UNDONE: We may also need to filter out columns with concept type assimilations in their path that are separated
					or mapped in the opposite direction, but checking the reference constraints might already take care of that.
				-->
				<xsl:copy-of select="$dataTypeMapping/@isIdentity"/>
			</xsl:if>
			<xsl:if test="not($dataTypeMapping) or not($dataTypeMapping/dcl:*)">
				<xsl:message>
					<xsl:text>Something is wrong.</xsl:text>
				</xsl:message>
			</xsl:if>
			<xsl:copy-of select="$dataTypeMapping/dcl:*"/>
		</dcl:column>
	</xsl:template>

	<xsl:template match="cdb:UniquenessConstraint" mode="GenerateTableContent">
		<xsl:variable name="sourceTable" select="parent::cdb:Constraints/parent::cdb:Table"/>
		<dcl:uniquenessConstraint name="{dsf:makeValidIdentifier(@Name)}" isPrimary="{@IsPrimary='true' or @IsPrimary=1}">
			<xsl:for-each select="cdb:Columns/cdb:Column">
				<dcl:columnRef name="{dsf:makeValidIdentifier($sourceTable/cdb:Columns/cdb:Column[@id = current()/@ref]/@Name)}"/>
			</xsl:for-each>
		</dcl:uniquenessConstraint>
	</xsl:template>

	<xsl:template match="cdb:ReferenceConstraint" mode="GenerateTableContent">
		<xsl:variable name="sourceTable" select="parent::cdb:Constraints/parent::cdb:Table"/>
		<xsl:variable name="targetTable" select="$sourceTable/parent::cdb:Tables/cdb:Table[@id = current()/cdb:TargetTable/@ref]"/>
		<dcl:referenceConstraint name="{dsf:makeValidIdentifier(@Name)}" targetTable="{dsf:makeValidIdentifier($targetTable/@Name)}">
			<xsl:for-each select="cdb:ColumnReferences/cdb:ColumnReference">
				<dcl:columnRef sourceName="{dsf:makeValidIdentifier($sourceTable/cdb:Columns/cdb:Column[@id = current()/@SourceColumn]/@Name)}" targetName="{dsf:makeValidIdentifier($targetTable/cdb:Columns/cdb:Column[@id = current()/@TargetColumn]/@Name)}"/>
			</xsl:for-each>
		</dcl:referenceConstraint>
	</xsl:template>


	<xsl:template match="orm:ValueType" mode="GenerateDataTypeMapping">
		<xsl:param name="ormModel"/>
		<xsl:variable name="dataTypeName" select="@Name"/>
		<xsl:variable name="modelConceptualDataType" select="orm:ConceptualDataType"/>
		<xsl:variable name="modelDataType" select="$ormModel/orm:DataTypes/child::*[@id=$modelConceptualDataType/@ref]"/>
		<xsl:variable name="modelValueConstraint" select="orm:ValueRestriction/orm:ValueConstraint"/>
		<xsl:variable name="modelValueRanges" select="$modelValueConstraint/orm:ValueRanges/orm:ValueRange"/>
		<xsl:variable name="length" select="number($modelConceptualDataType/@Length)"/>
		<xsl:variable name="scale" select="number($modelConceptualDataType/@Scale)"/>

		<xsl:choose>

			<xsl:when test="
				$modelDataType/self::orm:TrueOrFalseLogicalDataType or
				$modelDataType/self::orm:YesOrNoLogicalDataType">

				<xsl:variable name="hasTrueConstraint" select="$modelValueRanges[translate(@MinValue, 'true', 'TRUE') = 'TRUE' or translate(@MinValue, 'yes', 'YES') = 'YES' or @MinValue = 1]"/>
				<xsl:variable name="hasFalseConstraint" select="$modelValueRanges[translate(@MinValue, 'false', 'FALSE') = 'FALSE' or translate(@MinValue, 'no', 'NO') = 'NO' or @MinValue = 0]"/>

				<xsl:choose>
					<!-- BOOLEAN_HACK: Remove the false() on the next line to stop forcing open-world-with-negation. -->
					<xsl:when test="false() and $modelValueRanges and not($hasTrueConstraint and $hasFalseConstraint)">
						<dcl:domain name="{dsf:makeValidIdentifier($dataTypeName)}">
							<dcl:predefinedDataType name="BOOLEAN"/>
							<dcl:checkConstraint name="{dsf:makeValidIdentifier($modelValueConstraint/@Name)}">
								<xsl:choose>
									<xsl:when test="$hasTrueConstraint">
										<dep:comparisonPredicate operator="equals">
											<dep:valueKeyword/>
											<ddt:booleanLiteral value="TRUE"/>
										</dep:comparisonPredicate>
									</xsl:when>
									<xsl:otherwise>
										<dep:comparisonPredicate operator="equals">
											<dep:valueKeyword/>
											<ddt:booleanLiteral value="FALSE"/>
										</dep:comparisonPredicate>
									</xsl:otherwise>
								</xsl:choose>
							</dcl:checkConstraint>
						</dcl:domain>
					</xsl:when>
					<xsl:otherwise>
						<dcl:predefinedDataType name="BOOLEAN"/>
					</xsl:otherwise>
				</xsl:choose>
				<odt:boolean name="{$dataTypeName}">
					<xsl:if test="$modelValueRanges">
						<xsl:attribute name="fixed">
							<!-- This is a boolean, so there will only ever be at most one ValueRange for it, and @MinValue will always match @MaxValue -->
							<xsl:value-of select="$modelValueRanges/@MinValue"/>
						</xsl:attribute>
					</xsl:if>
				</odt:boolean>
			</xsl:when>

			<xsl:when test="
				$modelDataType/self::orm:AutoCounterNumericDataType or
				$modelDataType/self::orm:RowIdOtherDataType or
				$modelDataType/self::orm:SignedSmallIntegerNumericDataType or
				$modelDataType/self::orm:UnsignedSmallIntegerNumericDataType or
				$modelDataType/self::orm:SignedIntegerNumericDataType or
				$modelDataType/self::orm:UnsignedIntegerNumericDataType or
				$modelDataType/self::orm:SignedLargeIntegerNumericDataType or
				$modelDataType/self::orm:UnsignedLargeIntegerNumericDataType or
				$modelDataType/self::orm:DecimalNumericDataType or
				$modelDataType/self::orm:MoneyNumericDataType">

				<xsl:if test="$modelDataType/self::orm:AutoCounterNumericDataType or $modelDataType/self::orm:RowIdOtherDataType">
					<xsl:attribute name="isIdentity">
						<xsl:value-of select="true()"/>
					</xsl:attribute>
				</xsl:if>

				<xsl:variable name="isIntegral" select="
					$modelDataType/self::orm:AutoCounterNumericDataType or
					$modelDataType/self::orm:RowIdOtherDataType or
					$modelDataType/self::orm:SignedSmallIntegerNumericDataType or
					$modelDataType/self::orm:UnsignedSmallIntegerNumericDataType or
					$modelDataType/self::orm:SignedIntegerNumericDataType or
					$modelDataType/self::orm:UnsignedIntegerNumericDataType or
					$modelDataType/self::orm:SignedLargeIntegerNumericDataType or
					$modelDataType/self::orm:UnsignedLargeIntegerNumericDataType"/>

				<xsl:variable name="isUnsigned" select="
					$modelDataType/self::orm:UnsignedSmallIntegerNumericDataType or
					$modelDataType/self::orm:UnsignedIntegerNumericDataType or
					$modelDataType/self::orm:UnsignedLargeIntegerNumericDataType"/>

				<xsl:variable name="predefinedDataType">
					<dcl:predefinedDataType>
						<xsl:choose>
							<xsl:when test="$isIntegral">
								<xsl:attribute name="name">
									<xsl:choose>
										<xsl:when test="$modelDataType/self::orm:SignedSmallIntegerNumericDataType or $modelDataType/self::orm:UnsignedSmallIntegerNumericDataType">
											<xsl:value-of select="'SMALLINT'"/>
										</xsl:when>
										<xsl:when test="$modelDataType/self::orm:SignedLargeIntegerNumericDataType or $modelDataType/self::orm:UnsignedLargeIntegerNumericDataType">
											<xsl:value-of select="'BIGINT'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="'INTEGER'"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:attribute>
							</xsl:when>
							<xsl:when test="$modelDataType/self::orm:DecimalNumericDataType or $modelDataType/self::orm:MoneyNumericDataType">
								<xsl:attribute name="name">
									<xsl:value-of select="'DECIMAL'"/>
								</xsl:attribute>
								<xsl:if test="$length > 0 or $modelDataType/self::orm:MoneyNumericDataType">
									<xsl:attribute name="precision">
										<xsl:choose>
											<xsl:when test="$length > 0">
												<xsl:value-of select="$length"/>
											</xsl:when>
											<xsl:when test="$modelDataType/self::orm:MoneyNumericDataType">
												<xsl:value-of select="19"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:message terminate="yes">
													<xsl:text>SANITY CHECK: Boolean logic is broken...</xsl:text>
												</xsl:message>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:attribute>
									<xsl:choose>
										<xsl:when test="$scale > 0">
											<xsl:attribute name="scale">
												<xsl:value-of select="$scale"/>
											</xsl:attribute>
										</xsl:when>
										<xsl:when test="$modelDataType/self::orm:MoneyNumericDataType">
											<xsl:attribute name="scale">
												<xsl:choose>
													<xsl:when test="$length > 0 and $length &lt; 4">
														<xsl:value-of select="$length"/>
													</xsl:when>
													<xsl:otherwise>
														<xsl:value-of select="4"/>
													</xsl:otherwise>
												</xsl:choose>
											</xsl:attribute>
										</xsl:when>
									</xsl:choose>
								</xsl:if>
							</xsl:when>
						</xsl:choose>
					</dcl:predefinedDataType>
				</xsl:variable>

				<xsl:choose>
					<xsl:when test="$modelValueRanges or $isUnsigned">
						<dcl:domain name="{dsf:makeValidIdentifier($dataTypeName)}">
							<xsl:copy-of select="exsl:node-set($predefinedDataType)"/>
							<xsl:if test="$isUnsigned and not($modelValueRanges)">
								<dcl:checkConstraint name="{dsf:makeValidIdentifier(concat($dataTypeName, '_Unsigned_Chk'))}">
									<dep:comparisonPredicate operator="greaterThanOrEquals">
										<dep:valueKeyword/>
										<ddt:exactNumericLiteral value="0"/>
									</dep:comparisonPredicate>
								</dcl:checkConstraint>
							</xsl:if>
							<xsl:if test="$modelValueRanges">
								<dcl:checkConstraint name="{dsf:makeValidIdentifier($modelValueConstraint/@Name)}">
									<xsl:choose>
										<xsl:when test="$isUnsigned">
											<dep:and>
												<dep:comparisonPredicate operator="greaterThanOrEquals">
													<dep:valueKeyword/>
													<ddt:exactNumericLiteral value="0"/>
												</dep:comparisonPredicate>
												<xsl:call-template name="ProcessValueConstraintRanges">
													<xsl:with-param name="literalName" select="'ddt:exactNumericLiteral'"/>
													<xsl:with-param name="valueRanges" select="$modelValueRanges"/>
												</xsl:call-template>
											</dep:and>
										</xsl:when>
										<xsl:otherwise>
											<xsl:call-template name="ProcessValueConstraintRanges">
												<xsl:with-param name="literalName" select="'ddt:exactNumericLiteral'"/>
												<xsl:with-param name="valueRanges" select="$modelValueRanges"/>
											</xsl:call-template>
										</xsl:otherwise>
									</xsl:choose>
								</dcl:checkConstraint>
							</xsl:if>
						</dcl:domain>
					</xsl:when>
					<xsl:otherwise>
						<xsl:copy-of select="exsl:node-set($predefinedDataType)"/>
					</xsl:otherwise>
				</xsl:choose>

			</xsl:when>

			<xsl:when test="
				$modelDataType/self::orm:FloatingPointNumericDataType or
				$modelDataType/self::orm:SinglePrecisionFloatingPointNumericDataType or
				$modelDataType/self::orm:DoublePrecisionFloatingPointNumericDataType">

				<xsl:variable name="predefinedDataType">
					<xsl:choose>
						<xsl:when test="$modelDataType/self::orm:FloatingPointNumericDataType">
							<dcl:predefinedDataType name="FLOAT">
								<!-- TODO: Is the precision the $scale or the $length? For now, we'll just try both, preferring $length if both are specified. -->
								<xsl:if test="$scale > 0">
									<xsl:attribute name="precision">
										<xsl:value-of select="$scale"/>
									</xsl:attribute>
								</xsl:if>
								<xsl:if test="$length > 0">
									<xsl:attribute name="precision">
										<xsl:value-of select="$length"/>
									</xsl:attribute>
								</xsl:if>
							</dcl:predefinedDataType>
						</xsl:when>
						<xsl:when test="$modelDataType/self::orm:SinglePrecisionFloatingPointNumericDataType">
							<dcl:predefinedDataType name="REAL"/>
						</xsl:when>
						<xsl:when test="$modelDataType/self::orm:DoublePrecisionFloatingPointNumericDataType">
							<dcl:predefinedDataType name="DOUBLE PRECISION"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:message terminate="yes">
								<xsl:text>SANITY CHECK: Boolean logic is broken...</xsl:text>
							</xsl:message>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>

				<xsl:choose>
					<xsl:when test="$modelValueRanges">
						<dcl:domain name="{dsf:makeValidIdentifier($dataTypeName)}">
							<xsl:copy-of select="exsl:node-set($predefinedDataType)"/>
							<dcl:predefinedDataType/>
							<dcl:checkConstraint name="{dsf:makeValidIdentifier($modelValueConstraint/@Name)}">
								<xsl:call-template name="ProcessValueConstraintRanges">
									<xsl:with-param name="literalName" select="'ddt:approximateNumericLiteral'"/>
									<xsl:with-param name="valueRanges" select="$modelValueRanges"/>
								</xsl:call-template>
							</dcl:checkConstraint>
						</dcl:domain>
					</xsl:when>
					<xsl:otherwise>
						<xsl:copy-of select="exsl:node-set($predefinedDataType)"/>
					</xsl:otherwise>
				</xsl:choose>
				
			</xsl:when>

			<xsl:when test="
				$modelDataType/self::orm:FixedLengthTextDataType or
				$modelDataType/self::orm:VariableLengthTextDataType or
				$modelDataType/self::orm:LargeLengthTextDataType">

				<xsl:variable name="predefinedDataType">
					<dcl:predefinedDataType>
						<xsl:attribute name="name">
							<xsl:choose>
								<xsl:when test="$modelDataType/self::orm:FixedLengthTextDataType">
									<xsl:value-of select="'CHARACTER'"/>
								</xsl:when>
								<xsl:when test="$modelDataType/self::orm:VariableLengthTextDataType">
									<xsl:value-of select="'CHARACTER VARYING'"/>
								</xsl:when>
								<xsl:when test="$modelDataType/self::orm:LargeLengthTextDataType">
									<xsl:value-of select="'CHARACTER LARGE OBJECT'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:message terminate="yes">
										<xsl:text>SANITY CHECK: Boolean logic is broken...</xsl:text>
									</xsl:message>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:if test="$length > 0">
							<xsl:attribute name="length">
								<xsl:value-of select="$length"/>
							</xsl:attribute>
						</xsl:if>
					</dcl:predefinedDataType>
				</xsl:variable>

				<xsl:choose>
					<xsl:when test="$modelValueRanges and not($modelDataType/self::orm:LargeLengthTextDataType)">
						<dcl:domain name="{dsf:makeValidIdentifier($dataTypeName)}">
							<xsl:copy-of select="exsl:node-set($predefinedDataType)"/>
							<dcl:checkConstraint name="{dsf:makeValidIdentifier($modelValueConstraint/@Name)}">
								<!-- This may or may not work for actual ranges (where @MinValue != @MaxValue). -->
								<xsl:call-template name="ProcessValueConstraintRanges">
									<xsl:with-param name="literalName" select="'ddt:characterStringLiteral'"/>
									<xsl:with-param name="valueRanges" select="$modelValueRanges"/>
								</xsl:call-template>
							</dcl:checkConstraint>
						</dcl:domain>
					</xsl:when>
					<xsl:otherwise>
						<xsl:copy-of select="exsl:node-set($predefinedDataType)"/>
					</xsl:otherwise>
				</xsl:choose>

			</xsl:when>

			<xsl:when test="
				$modelDataType/self::orm:FixedLengthRawDataDataType or
				$modelDataType/self::orm:VariableLengthRawDataDataType or
				$modelDataType/self::orm:LargeLengthRawDataDataType or
				$modelDataType/self::orm:PictureRawDataDataType or
				$modelDataType/self::orm:OleObjectRawDataDataType">

				<dcl:predefinedDataType name="BINARY LARGE OBJECT" length="{$length}"/>
				<xsl:if test="$modelValueRanges">
					<xsl:comment>
						<xsl:text>WARNING: You had a value constraint on this binary data type, which has been ignored.</xsl:text>
					</xsl:comment>
				</xsl:if>

			</xsl:when>

			<xsl:when test="
				$modelDataType/self::orm:AutoTimestampTemporalDataType or
				$modelDataType/self::orm:DateAndTimeTemporalDataType or
				$modelDataType/self::orm:TimeTemporalDataType or
				$modelDataType/self::orm:DateTemporalDataType">

				<xsl:variable name="predefinedDataTypeName">
					<xsl:choose>
						<xsl:when test="$modelDataType/self::orm:AutoTimestampTemporalDataType or $modelDataType/self::orm:DateAndTimeTemporalDataType">
							<xsl:value-of select="'TIMESTAMP'"/>
						</xsl:when>
						<xsl:when test="$modelDataType/self::orm:TimeTemporalDataType">
							<xsl:value-of select="'TIME'"/>
						</xsl:when>
						<xsl:when test="$modelDataType/self::orm:DateTemporalDataType">
							<xsl:value-of select="'DATE'"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:message terminate="yes">
								<xsl:text>SANITY CHECK: Boolean logic is broken...</xsl:text>
							</xsl:message>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>

				<xsl:choose>
					<xsl:when test="$modelValueRanges">
						<dcl:domain name="{dsf:makeValidIdentifier($dataTypeName)}">
							<dcl:predefinedDataType name="{$predefinedDataTypeName}"/>
							<dcl:checkConstraint name="{dsf:makeValidIdentifier($modelValueConstraint/@Name)}">
								<xsl:call-template name="ProcessValueConstraintRanges">
									<xsl:with-param name="literalName" select="concat('ddt:', translate($predefinedDataTypeName, 'DATEIMSAP', 'dateimsap'), 'Literal')"/>
									<xsl:with-param name="valueRanges" select="$modelValueRanges"/>
								</xsl:call-template>
							</dcl:checkConstraint>
						</dcl:domain>
					</xsl:when>
					<xsl:otherwise>
						<dcl:predefinedDataType name="{$predefinedDataTypeName}"/>
					</xsl:otherwise>
				</xsl:choose>
				
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


	<xsl:template name="ProcessValueConstraintRanges">
		<xsl:param name="literalName"/>
		<xsl:param name="valueRanges"/>
		<xsl:variable name="valueRangesForIn" select="$valueRanges[string-length(@MinValue) and string-length(@MaxValue) and (@MinValue = @MaxValue)]"/>
		<xsl:variable name="valueRangesForComparisons" select="$valueRanges[not(string-length(@MinValue)) or not(string-length(@MaxValue)) or not(@MinValue = @MaxValue)]"/>

		<xsl:choose>
			<xsl:when test="$valueRangesForIn and $valueRangesForComparisons">
				<dep:or>
					<xsl:call-template name="ProcessValueConstraintRangesForIn">
						<xsl:with-param name="literalName" select="$literalName"/>
						<xsl:with-param name="valueRanges" select="$valueRangesForIn"/>
					</xsl:call-template>
					<xsl:call-template name="ProcessValueConstraintRangesForComparisons">
						<xsl:with-param name="literalName" select="$literalName"/>
						<xsl:with-param name="valueRanges" select="$valueRangesForComparisons"/>
					</xsl:call-template>
				</dep:or>
			</xsl:when>
			<xsl:when test="$valueRangesForIn and not($valueRangesForComparisons)">
				<xsl:call-template name="ProcessValueConstraintRangesForIn">
					<xsl:with-param name="literalName" select="$literalName"/>
					<xsl:with-param name="valueRanges" select="$valueRangesForIn"/>
				</xsl:call-template>
			</xsl:when>
			<xsl:when test="not($valueRangesForIn) and $valueRangesForComparisons">
				<xsl:call-template name="ProcessValueConstraintRangesForComparisons">
					<xsl:with-param name="literalName" select="$literalName"/>
					<xsl:with-param name="valueRanges" select="$valueRangesForComparisons"/>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:message terminate="yes">
					<xsl:text>SANITY CHECK: This template should only be used on value types that have value constraints requiring processing.</xsl:text>
				</xsl:message>
			</xsl:otherwise>
		</xsl:choose>
		
	</xsl:template>
	<xsl:template name="ProcessValueConstraintRangesForIn">
		<xsl:param name="literalName"/>
		<xsl:param name="valueRanges"/>
		<dep:inPredicate type="IN">
			<dep:valueKeyword/>
			<xsl:for-each select="$valueRanges">
				<xsl:element name="{$literalName}">
					<xsl:attribute name="value">
						<xsl:value-of select="@MinValue"/>
					</xsl:attribute>
				</xsl:element>
			</xsl:for-each>
		</dep:inPredicate>
	</xsl:template>
	<xsl:template name="ProcessValueConstraintRangesForComparisons">
		<xsl:param name="literalName"/>
		<xsl:param name="valueRanges"/>
		<xsl:param name="currentNr" select="1"/>
		<xsl:param name="totalNr" select="count($valueRanges)"/>
		<xsl:variable name="rangeCode">
			<xsl:variable name="currentRange" select="$valueRanges[$currentNr]"/>
			<xsl:variable name="lowerBoundLiteral">
				<xsl:if test="string-length($currentRange/@MinValue)">
					<xsl:element name="{$literalName}">
						<xsl:attribute name="value">
							<xsl:value-of select="$currentRange/@MinValue"/>
						</xsl:attribute>
					</xsl:element>
				</xsl:if>
			</xsl:variable>
			<xsl:variable name="upperBoundLiteral">
				<xsl:if test="string-length($currentRange/@MaxValue)">
					<xsl:element name="{$literalName}">
						<xsl:attribute name="value">
							<xsl:value-of select="$currentRange/@MaxValue"/>
						</xsl:attribute>
					</xsl:element>
				</xsl:if>
			</xsl:variable>
			<xsl:variable name="lowerBoundOperator">
				<xsl:if test="string-length($currentRange/@MinValue)">
					<xsl:choose>
						<xsl:when test="$currentRange/@MinInclusion='Open'">
							<xsl:value-of select="'greaterThan'"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="'greaterThanOrEquals'"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:if>
			</xsl:variable>
			<xsl:variable name="upperBoundOperator">
				<xsl:if test="string-length($currentRange/@MaxValue)">
					<xsl:choose>
						<xsl:when test="$currentRange/@MaxInclusion='Open'">
							<xsl:value-of select="'lessThan'"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="'lessThanOrEquals'"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:if>
			</xsl:variable>
			<xsl:choose>
				<xsl:when test="$lowerBoundOperator='greaterThanOrEquals' and $upperBoundOperator='lessThanOrEquals'">
					<dep:betweenPredicate type="BETWEEN">
						<dep:valueKeyword/>
						<xsl:copy-of select="exsl:node-set($lowerBoundLiteral)"/>
						<xsl:copy-of select="exsl:node-set($upperBoundLiteral)"/>
					</dep:betweenPredicate>
				</xsl:when>
				<xsl:when test="string-length($lowerBoundOperator) and string-length($upperBoundOperator)">
					<dep:and>
						<dep:comparisonPredicate operator="{$lowerBoundOperator}">
							<dep:valueKeyword/>
							<xsl:copy-of select="exsl:node-set($lowerBoundLiteral)"/>
						</dep:comparisonPredicate>
						<dep:comparisonPredicate operator="{$upperBoundOperator}">
							<dep:valueKeyword/>
							<xsl:copy-of select="exsl:node-set($upperBoundLiteral)"/>
						</dep:comparisonPredicate>
					</dep:and>
				</xsl:when>
				<xsl:when test="string-length($lowerBoundOperator)">
					<dep:comparisonPredicate operator="{$lowerBoundOperator}">
						<dep:valueKeyword/>
						<xsl:copy-of select="exsl:node-set($lowerBoundLiteral)"/>
					</dep:comparisonPredicate>
				</xsl:when>
				<xsl:when test="string-length($upperBoundOperator)">
					<dep:comparisonPredicate operator="{$upperBoundOperator}">
						<dep:valueKeyword/>
						<xsl:copy-of select="exsl:node-set($upperBoundLiteral)"/>
					</dep:comparisonPredicate>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="$currentNr &lt; $totalNr">
				<dep:or>
					<xsl:copy-of select="exsl:node-set($rangeCode)"/>
					<xsl:call-template name="ProcessValueConstraintRangesForComparisons">
						<xsl:with-param name="literalName" select="$literalName"/>
						<xsl:with-param name="valueRanges" select="$valueRanges"/>
						<xsl:with-param name="currentNr" select="$currentNr + 1"/>
						<xsl:with-param name="totalNr" select="$totalNr"/>
					</xsl:call-template>
				</dep:or>
			</xsl:when>
			<xsl:otherwise>
				<xsl:copy-of select="exsl:node-set($rangeCode)"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	

</xsl:stylesheet>
