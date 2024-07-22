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
	xmlns:rcd="http://schemas.neumont.edu/ORM/Relational/2007-06/ConceptualDatabase"
	xmlns:oialtocdb="http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMAbstractionToConceptualDatabase"
	xmlns:dcl="http://schemas.orm.net/DIL/DCIL"
	xmlns:dil="http://schemas.orm.net/DIL/DIL"
	xmlns:ddt="http://schemas.orm.net/DIL/DILDT"
	xmlns:dep="http://schemas.orm.net/DIL/DILEP"
	xmlns:dml="http://schemas.orm.net/DIL/DMIL"
	xmlns:ddl="http://schemas.orm.net/DIL/DDIL"
	xmlns:loc="urn:local-cache"
	extension-element-prefixes="exsl dsf"
	exclude-result-prefixes="orm ormRoot ormtooial oial odt rcd oialtocdb loc">

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

	<xsl:variable name="Document" select="."/>
	<xsl:key name="KeyedConceptTypes" match="ormRoot:ORM2/oial:model/oial:conceptTypes/oial:conceptType" use="@id"/>
	<xsl:key name="KeyedConceptTypeChildren" match="ormRoot:ORM2/oial:model/oial:conceptTypes/oial:conceptType/oial:children/oial:*" use="@id"/>
	<xsl:key name="KeyedFactTypes" match="ormRoot:ORM2/orm:ORMModel/orm:Facts/orm:*" use="@id"/>
	<xsl:key name="KeyedFactTypeNegationLink" match="ormRoot:ORM2/orm:ORMModel/orm:Facts/orm:NegatedByUnaryFactType" use="@ref"/>
	<xsl:key name="KeyedRoles" match="ormRoot:ORM2/orm:ORMModel/orm:Facts/orm:*/orm:FactRoles/orm:*" use="@id"/>
	<xsl:key name="KeyedObjectTypes" match="ormRoot:ORM2/orm:ORMModel/orm:Objects/orm:*" use="@id"/>
	<xsl:key name="KeyedObjectTypeByConceptType" match="ormRoot:ORM2/ormtooial:Bridge/ormtooial:ConceptTypeIsForObjectType" use="@ConceptType"/>
	<xsl:key name="KeyedPathFactTypes" match="ormRoot:ORM2/ormtooial:Bridge/ormtooial:ConceptTypeChildHasPathFactType" use="@ConceptTypeChild"/>
	<xsl:key name="KeyedTowardsRole" match="ormRoot:ORM2/ormtooial:Bridge/ormtooial:FactTypeMapsTowardsRole" use="@FactType"/>
	<xsl:template match="ormRoot:ORM2">
		<xsl:apply-templates select="rcd:Catalog/rcd:Schemas/rcd:Schema"/>
	</xsl:template>
	<xsl:template match="rcd:Schema">
		<xsl:variable name="root" select="/ormRoot:ORM2"/>
		<xsl:variable name="oialDcilBridge" select="$root/oialtocdb:Bridge"/>
		<xsl:variable name="ormOialBridge" select="$root/ormtooial:Bridge"/>
		<xsl:variable name="oialModel" select="$root/oial:model[@id = $oialDcilBridge/oialtocdb:SchemaIsForAbstractionModel[@Schema = current()/@id]/@AbstractionModel]"/>
		<xsl:variable name="ormModel" select="$root/orm:ORMModel[@id = $ormOialBridge/ormtooial:AbstractionModelIsForORMModel[@AbstractionModel = $oialModel/@id]/@ORMModel]"/>
		<xsl:variable name="mappedValueTypes" select="$ormModel/orm:Objects/orm:ValueType[@id = $ormOialBridge/ormtooial:InformationTypeFormatIsForValueType[@InformationTypeFormat = $oialModel/oial:informationTypeFormats/child::*/@id]/@ValueType]"/>
		<xsl:variable name="booleanFormats" select="$oialModel/oial:informationTypeFormats/odt:*[self::odt:booleanTrue | self::odt:booleanFalse]"/>
		<xsl:variable name="unaryInformationTypeIds" select="$oialModel/oial:conceptTypes/oial:conceptType/oial:children/oial:informationType[@ref=$booleanFormats/@id]/@id"/>
		<xsl:variable name="initialDataTypeMappingsFragment">
			<xsl:for-each select="$mappedValueTypes">
				<DataTypeMapping id="{@id}">
					<xsl:apply-templates select="." mode="GenerateDataTypeMapping">
						<xsl:with-param name="ormModel" select="$ormModel"/>
					</xsl:apply-templates>
				</DataTypeMapping>
			</xsl:for-each>
			<!-- Always add a mapping for boolean. This is ignored if it isn't used because it doesn't have a domain. -->
			<DataTypeMapping id="__BOOLEAN">
				<dcl:predefinedDataType name="BOOLEAN" />
			</DataTypeMapping>
			<xsl:if test="$oialDcilBridge/oialtocdb:ColumnHasConceptTypeChild[@AbsorptionIndicator[.='true' or .='1'] or @ConceptTypeChild=$unaryInformationTypeIds][not(oialtocdb:InverseConceptTypeChild)]">
				<DataTypeMapping id="__BOOLEAN_TRUE">
					<dcl:domain>
						<xsl:call-template name="AddNameAttributes">
							<xsl:with-param name="requestedName" select="'BOOLEAN_TRUE'"/>
						</xsl:call-template>
						<dcl:predefinedDataType name="BOOLEAN" />
						<dcl:checkConstraint>
							<xsl:call-template name="AddNameAttributes">
								<xsl:with-param name="requestedName" select="'BOOLEAN_TRUE_CHK'"/>
							</xsl:call-template>
							<dep:comparisonPredicate operator="equals">
								<dep:valueKeyword />
								<ddt:booleanLiteral value="TRUE"/>
							</dep:comparisonPredicate>
						</dcl:checkConstraint>
					</dcl:domain>
				</DataTypeMapping>
			</xsl:if>
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

		<dcl:schema>
			<xsl:call-template name="AddNameAttributes"/>
			<xsl:copy-of select="$initialDataTypeMappings/dcl:domain"/>
			<xsl:apply-templates mode="GenerateSchemaContent" select="rcd:Tables/rcd:Table">
				<xsl:with-param name="oialDcilBridge" select="$oialDcilBridge"/>
				<xsl:with-param name="oialModel" select="$oialModel"/>
				<xsl:with-param name="ormModel" select="$ormModel"/>
				<xsl:with-param name="dataTypeMappings" select="$dataTypeMappings"/>
				<xsl:with-param name="initialDataTypeMappings" select="$initialDataTypeMappings"/>
				<xsl:with-param name="booleanFormats" select="$booleanFormats"/>
				<xsl:with-param name="unaryInformationTypeIds" select="$unaryInformationTypeIds"/>
			</xsl:apply-templates>
		</dcl:schema>
	</xsl:template>
	<xsl:template match="rcd:Table" mode="GenerateSchemaContent">
		<xsl:param name="oialDcilBridge"/>
		<xsl:param name="oialModel"/>
		<xsl:param name="ormModel"/>
		<xsl:param name="dataTypeMappings"/>
		<xsl:param name="initialDataTypeMappings"/>
		<xsl:param name="booleanFormats"/>
		<xsl:param name="unaryInformationTypeIds"/>
		<xsl:variable name="rawTableName" select="string(@Name)"/>
		<dcl:table>
			<xsl:call-template name="AddNameAttributes">
				<xsl:with-param name="requestedName" select="$rawTableName"/>
			</xsl:call-template>
			<xsl:variable name="uniquenessConstraints" select="rcd:Constraints/rcd:UniquenessConstraint"/>
			<xsl:apply-templates mode="GenerateTableContent" select="rcd:Columns/rcd:Column">
				<xsl:with-param name="oialDcilBridge" select="$oialDcilBridge"/>
				<xsl:with-param name="oialModel" select="$oialModel"/>
				<xsl:with-param name="ormModel" select="$ormModel"/>
				<xsl:with-param name="dataTypeMappings" select="$dataTypeMappings"/>
				<xsl:with-param name="initialDataTypeMappings" select="$initialDataTypeMappings"/>
				<xsl:with-param name="unaryInformationTypeIds" select="$unaryInformationTypeIds"/>
				<xsl:with-param name="booleanFormats" select="$booleanFormats"/>
				<xsl:with-param name="rawTableName" select="$rawTableName"/>
			</xsl:apply-templates>
			<xsl:apply-templates mode="GenerateTableContent" select="rcd:Constraints/rcd:*"/>
			<xsl:apply-templates mode="GenerateAbsorptionConstraints" select=".">
				<xsl:with-param name="oialDcilBridge" select="$oialDcilBridge"/>
				<xsl:with-param name="booleanFormats" select="$booleanFormats"/>
			</xsl:apply-templates>
		</dcl:table>
	</xsl:template>
	<xsl:template match="rcd:Column" mode="GenerateTableContent">
		<xsl:param name="oialDcilBridge"/>
		<xsl:param name="oialModel"/>
		<xsl:param name="ormModel"/>
		<xsl:param name="dataTypeMappings"/>
		<xsl:param name="initialDataTypeMappings"/>
		<xsl:param name="booleanFormats"/>
		<xsl:param name="unaryInformationTypeIds"/>
		<xsl:param name="rawTableName"/>

		<xsl:variable name="column" select="."/>

		<!-- Get the ordered child path -->
		<xsl:variable name="conceptTypeChildPath" select="$oialDcilBridge/oialtocdb:ColumnHasConceptTypeChild[@Column = current()/@id]"/>

		<!-- Get all fact types for easier lookup. This will be ordered per child, but the selected fact type order order may not be in the child path. -->
		<xsl:variable name="pathFactTypes" select="key('KeyedPathFactTypes',$conceptTypeChildPath/@ConceptTypeChild)"/>

		<!-- Extract value constraint and default value information from the path by looking at opposite roles and unary patterns. -->
		<xsl:variable name="valueDataFragment">
			<xsl:for-each select="$conceptTypeChildPath">
				<xsl:variable name="childPathNode" select="."/>
				<xsl:variable name="childId" select="string(@ConceptTypeChild)"/>
				<xsl:variable name="conceptTypeChild" select="key('KeyedConceptTypeChildren',$childId)"/>

				<!-- Do a preliminary test on how to handle defaults. Role value constraints can always be propagated
				up through the stack, but a default value can only be used if a reference constraint is complete
				and an assimilation is mandatory. We never want to implicitly type an element as a subtype
				by defaulting an assimilated column at the database level, leaving defaults in the model to be
				applied in different parts of the application.
				r=default and value constraint from opposite role
				b=block default
				t=true default
				f=false default
				-->
				<xsl:variable name="valueStyleFragment">
					<xsl:choose>
						<xsl:when test="$conceptTypeChild[self::oial:relatedConceptType]">
							<!-- A default value is available if this reference maps to an item with a single-valued
							identifier. There are numerous recursive ways to determine this back through the ORM model,
							but we already have the answer in the table based on the number of column references in
							the reference constraint. Even without a default possible, a role value constraint
							may still be possible from the downstream path. -->
							<xsl:choose>
								<xsl:when test="count($column/../../rcd:Constraints/rcd:ReferenceConstraint/rcd:ColumnReferences[rcd:ColumnReference[@SourceColumn=$column/@id]]/rcd:ColumnReference)=1">
									<xsl:text>r</xsl:text>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>b</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:when>
						<xsl:when test="$conceptTypeChild[self::oial:assimilatedConceptType]">
							<xsl:choose>
								<xsl:when test="$childPathNode[@AbsorptionIndicator[.='true' or .='1']]">
									<!-- This is either an extra column added for a subtype-style assimilation or
									an indicator column used when paired unaries are both objectified. Only the unary
									case can have default values and value constraints. All other case will have BOOLEAN_TRUE
									types without a default or constraint. -->
									<xsl:choose>
										<xsl:when test="not($conceptTypeChild/@refersToSubtype)">
											<!-- Look for objectified unary -->
											<xsl:variable name="objectifiedFactType" select="key('KeyedFactTypes', key('KeyedObjectTypes',key('KeyedObjectTypeByConceptType',$conceptTypeChild/@ref)/@ObjectType)[self::orm:ObjectifiedType]/orm:NestedPredicate/@ref)"/>
											<xsl:choose>
												<xsl:when test="count($objectifiedFactType/orm:FactRoles/orm:*)=1">
													<xsl:call-template name="ColumnDefaultFromUnaryFactType">
														<xsl:with-param name="columnPathNode" select="$childPathNode"/>
														<xsl:with-param name="factType" select="$objectifiedFactType"/>
													</xsl:call-template>
												</xsl:when>
											</xsl:choose>
										</xsl:when>
									</xsl:choose>
								</xsl:when>
								<xsl:when test="not($conceptTypeChild[@isMandatory[.='true' or .='1']])">
									<!-- We can't use relational defaults for optional secondary types in the table. -->
									<xsl:text>b</xsl:text>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>r</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:when>
						<xsl:when test="$conceptTypeChild[self::oial:informationType]">
							<xsl:choose>
								<xsl:when test="$conceptTypeChild/@ref=$booleanFormats/@id">
									<xsl:call-template name="ColumnDefaultFromUnaryFactType">
										<xsl:with-param name="columnPathNode" select="$childPathNode"/>
										<xsl:with-param name="factType" select="key('KeyedFactTypes',$pathFactTypes[@ConceptTypeChild=$childId])"/>
									</xsl:call-template>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>r</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:when>
					</xsl:choose>
				</xsl:variable>
				<xsl:variable name="valueStyle" select="string($valueStyleFragment)"/>
				<xsl:if test="$valueStyle">
					<xsl:choose>
						<xsl:when test="$valueStyle='t'">
							<loc:defaultValue boolean="t"/>
						</xsl:when>
						<xsl:when test="$valueStyle='f'">
							<loc:defaultValue boolean="f"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:variable name="ignoreDefault" select="$valueStyle='b'"/>
							<xsl:if test="$ignoreDefault">
								<loc:defaultValue block="1"/>
							</xsl:if>
							<xsl:for-each select="$pathFactTypes[@ConceptTypeChild=$childId]">
								<!-- Retrieve fact types individually so we get the path order, not the document order -->
								<xsl:variable name="trailingFactType" select="position()=last()"/>
								<xsl:for-each select="key('KeyedFactTypes',@PathFactType)">
									<xsl:variable name="factTypeMapping" select="key('KeyedTowardsRole',@id)"/>
									<xsl:variable name="fromRoleOrProxy" select="orm:FactRoles/orm:*[not(@id=$factTypeMapping/@TowardsRole)]"/>
									<xsl:choose>
										<xsl:when test="$fromRoleOrProxy[self::orm:RoleProxy]">
											<xsl:call-template name="ValueDataFromRole">
												<xsl:with-param name="role" select="key('KeyedRoles',$fromRoleOrProxy/orm:Role/@ref)"/>
												<xsl:with-param name="ignoreDefault" select="$ignoreDefault"/>
												<xsl:with-param name="checkTypeDefault" select="$trailingFactType"/>
											</xsl:call-template>
										</xsl:when>
										<xsl:otherwise>
											<xsl:call-template name="ValueDataFromRole">
												<xsl:with-param name="role" select="$fromRoleOrProxy"/>
												<xsl:with-param name="ignoreDefault" select="$ignoreDefault"/>
												<xsl:with-param name="checkTypeDefault" select="$trailingFactType"/>
											</xsl:call-template>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:for-each>
							</xsl:for-each>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:if>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="valueData" select="exsl:node-set($valueDataFragment)/*"/>
		<xsl:variable name="valueRestrictedRoleId" select="$valueData[self::loc:valueConstraint][1]/@role"/>
		<xsl:variable name="defaultValue" select="$valueData[self::loc:defaultValue][1][not(@block)]"/>

		<xsl:variable name="valueTypeId">
			<xsl:variable name="childPathTypeNode" select="$conceptTypeChildPath[last()]"/>
			<xsl:variable name="informationTypeId" select="$childPathTypeNode/@ConceptTypeChild"/>
			<xsl:choose>
				<xsl:when test="$childPathTypeNode[@AbsorptionIndicator[.='true' or .='1']] or $informationTypeId=$unaryInformationTypeIds">
					<!-- The presense of an inverse child reference tells us if this is a straight boolean or a true-only boolean. Use special id values -->
					<xsl:choose>
						<xsl:when test="$childPathTypeNode/oialtocdb:InverseConceptTypeChild">
							<xsl:text>__BOOLEAN</xsl:text>
						</xsl:when>
						<xsl:otherwise>
							<xsl:text>__BOOLEAN_TRUE</xsl:text>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:otherwise>
					<xsl:variable name="factTypeId" select="$pathFactTypes[@ConceptTypeChild = $informationTypeId][last()]/@PathFactType"/>
					<xsl:choose>
						<xsl:when test="$factTypeId">
							<xsl:variable name="factTypeMapping" select="key('KeyedTowardsRole',$factTypeId)"/>
							<xsl:variable name="factType" select="key('KeyedFactTypes',$factTypeId)"/>
							<xsl:variable name="roleOrProxy" select="$factType/orm:FactRoles/orm:*[not(@id = $factTypeMapping/@TowardsRole)]"/>
							<xsl:if test="count($roleOrProxy) != 1">
								<xsl:message terminate="yes">
									<xsl:text>SANITY CHECK: Found no or multiple roles for column "</xsl:text>
									<xsl:value-of select="@Name"/>
									<xsl:text>" in table "</xsl:text>
									<xsl:value-of select="parent::rcd:Columns/parent::rcd:Table/@Name"/>
									<xsl:text>".</xsl:text>
								</xsl:message>
							</xsl:if>
							<xsl:choose>
								<xsl:when test="$roleOrProxy[self::orm:Role]">
									<xsl:value-of select="$roleOrProxy/orm:RolePlayer/@ref"/>
								</xsl:when>
								<xsl:when test="$roleOrProxy[self::orm:RoleProxy]">
									<xsl:value-of select="key('KeyedRoles', $roleOrProxy/orm:Role/@ref)/orm:RolePlayer/@ref"/>
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
							<xsl:variable name="conceptTypeId" select="key('KeyedConceptTypeChildren',$informationTypeId)/parent::oial:children/parent::oial:concepType/@id"/>
							<xsl:variable name="objectTypeId" select="key('KeyedObjectTypeByConceptType',$conceptTypeId)/@ObjectType"/>
							<xsl:if test="not(key('KeyedObjectTypes',$objectTypeId)[self::orm:ValueType])">
								<xsl:message terminate="yes">
									<xsl:text>SANITY CHECK: Found no roles and no value type for column "</xsl:text>
									<xsl:value-of select="@Name"/>
									<xsl:text>" in table "</xsl:text>
									<xsl:value-of select="parent::rcd:Columns/parent::rcd:Table/@Name"/>
									<xsl:text>".</xsl:text>
								</xsl:message>
							</xsl:if>
							<xsl:value-of select="$objectTypeId"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="dataTypeMapping" select="$dataTypeMappings[@id = $valueTypeId]"/>
		<xsl:variable name="rawColumnName" select="string(@Name)"/>
		<xsl:variable name="columnName" select="dsf:makeValidIdentifier($rawColumnName)"/>

		<xsl:variable name="valueLiteralNameFragment">
			<xsl:if test="$valueRestrictedRoleId or $defaultValue">
				<xsl:variable name="predefinedDataType" select="$initialDataTypeMappings[@id = $valueTypeId]//dcl:predefinedDataType"/>
				<xsl:variable name="predefinedDataTypeName" select="string($predefinedDataType/@name)"/>
				<xsl:choose>
					<xsl:when test="
						$predefinedDataTypeName = 'CHARACTER LARGE OBJECT' or
						$predefinedDataTypeName = 'CHARACTER' or
						$predefinedDataTypeName = 'CHARACTER VARYING'">
						<xsl:text>characterStringLiteral</xsl:text>
					</xsl:when>
					<xsl:when test="
						$predefinedDataTypeName = 'BINARY LARGE OBJECT' or
						$predefinedDataTypeName = 'BINARY' or
						$predefinedDataTypeName = 'BINARY VARYING'">
						<xsl:text>binaryStringLiteral</xsl:text>
					</xsl:when>
					<xsl:when test="
						$predefinedDataTypeName = 'NUMERIC' or
						$predefinedDataTypeName = 'DECIMAL' or
						$predefinedDataTypeName = 'TINYINT' or
						$predefinedDataTypeName = 'SMALLINT' or
						$predefinedDataTypeName = 'INTEGER' or
						$predefinedDataTypeName = 'BIGINT'">
						<xsl:text>exactNumericLiteral</xsl:text>
					</xsl:when>
					<xsl:when test="
						$predefinedDataTypeName = 'FLOAT' or
						$predefinedDataTypeName = 'REAL' or
						$predefinedDataTypeName = 'DOUBLE PRECISION'">
						<xsl:text>approximateNumericLiteral</xsl:text>
					</xsl:when>
					<xsl:when test="$predefinedDataTypeName = 'BOOLEAN'">
						<xsl:text>booleanLiteral</xsl:text>
					</xsl:when>
					<xsl:when test="
						$predefinedDataTypeName = 'DATE'">
						<xsl:text>dateLiteral</xsl:text>
					</xsl:when>
					<xsl:when test="
						$predefinedDataTypeName = 'TIME'">
						<xsl:text>timeLiteral</xsl:text>
					</xsl:when>
					<xsl:when test="
						$predefinedDataTypeName = 'DATETIME'">
						<xsl:text>datetimeLiteral</xsl:text>
					</xsl:when>
					<xsl:when test="
						$predefinedDataTypeName = 'TIMESTAMP'">
						<xsl:text>timestampLiteral</xsl:text>
					</xsl:when>
					<xsl:when test="
						$predefinedDataTypeName = 'INTERVAL'">
						<xsl:choose>
							<xsl:when test="$predefinedDataType/@fields = 'YEAR' or $predefinedDataType/@fields = 'YEAR TO MONTH' or $predefinedDataType/@fields = 'MONTH'">
								<xsl:text>yearMonthIntervalLiteral</xsl:text>
							</xsl:when>
							<xsl:otherwise>
								<xsl:text>dayTimeIntervalLiteral</xsl:text>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:message terminate="yes">
							<xsl:text>ERROR: Unrecognized data type '</xsl:text>
							<xsl:value-of select="$predefinedDataTypeName"/>
							<xsl:text>'.</xsl:text>
						</xsl:message>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:if>
		</xsl:variable>
		<xsl:variable name="valueLiteralName" select="string($valueLiteralNameFragment)"/>

		<dcl:column name="{$columnName}">
			<xsl:if test="$columnName!=$rawColumnName">
				<xsl:attribute name="requestedName">
					<xsl:value-of select="$rawColumnName"/>
				</xsl:attribute>
			</xsl:if>
			<xsl:attribute name="isNullable">
				<xsl:value-of select="boolean(@IsNullable[.='true' or .='1'])"/>
			</xsl:attribute>
			<xsl:variable name="useIdentity" select="
				$dataTypeMapping[@isIdentity='true'] and
				(count($conceptTypeChildPath) = 1 or
				(not(key('KeyedConceptTypeChildren',$conceptTypeChildPath/@ConceptTypeChild)[self::oial:relatedConceptType]) and
				not(parent::rcd:Columns/parent::rcd:Table/rcd:Constraints/rcd:ReferenceConstraint/rcd:ColumnReferences/rcd:ColumnReference[@SourceColumn = current()/@id])))"/>
			<xsl:if test="$useIdentity">
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
			<xsl:copy-of select="$dataTypeMapping/dcl:*"/>
			<xsl:if test="not($useIdentity)">
				<xsl:for-each select="$defaultValue">
					<dil:defaultClause>
						<xsl:choose>
							<xsl:when test="@boolean">
								<ddt:booleanLiteral value="TRUE">
									<xsl:if test="@boolean='f'">
										<xsl:attribute name="value">
											<xsl:text>FALSE</xsl:text>
										</xsl:attribute>
									</xsl:if>
								</ddt:booleanLiteral>
							</xsl:when>
							<xsl:otherwise>
								<xsl:element name="ddt:{$valueLiteralName}">
									<xsl:attribute name="value">
										<xsl:choose>
											<xsl:when test="@empty">
												<!-- Leave the value empty -->
											</xsl:when>
											<xsl:when test="@invariantValue">
												<xsl:value-of select="@invariantValue"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="@value"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:attribute>
								</xsl:element>
							</xsl:otherwise>
						</xsl:choose>
					</dil:defaultClause>
				</xsl:for-each>
			</xsl:if>
		</dcl:column>

		<xsl:if test="$valueRestrictedRoleId">
			<xsl:variable name="valueReferenceFragment">
				<dep:columnReference name="{$columnName}"/>
			</xsl:variable>
			<xsl:variable name="valueReference" select="exsl:node-set($valueReferenceFragment)/child::*"/>

			<xsl:for-each select="key('KeyedRoles',$valueRestrictedRoleId)/orm:ValueRestriction/orm:RoleValueConstraint[not(@Modality='Deontic')]">
				<dcl:checkConstraint>
					<xsl:call-template name="AddNameAttributes">
						<xsl:with-param name="requestedName" select="concat($rawTableName, '_', $rawColumnName, '_', @Name)"/>
					</xsl:call-template>
					<xsl:call-template name="ProcessValueConstraintRanges">
						<xsl:with-param name="literalName" select="$valueLiteralName"/>
						<xsl:with-param name="valueRanges" select="orm:ValueRanges/orm:ValueRange"/>
						<xsl:with-param name="valueReference" select="$valueReference"/>
					</xsl:call-template>
				</dcl:checkConstraint>
			</xsl:for-each>
		</xsl:if>
	</xsl:template>

	<xsl:template match="rcd:UniquenessConstraint" mode="GenerateTableContent">
		<xsl:variable name="sourceTable" select="parent::rcd:Constraints/parent::rcd:Table"/>
		<dcl:uniquenessConstraint>
			<xsl:call-template name="AddNameAttributes"/>
			<xsl:attribute name="isPrimary">
				<xsl:value-of select="@IsPrimary='true' or @IsPrimary=1"/>
			</xsl:attribute>
			<xsl:for-each select="rcd:Columns/rcd:Column">
				<dcl:columnRef name="{dsf:makeValidIdentifier($sourceTable/rcd:Columns/rcd:Column[@id = current()/@ref]/@Name)}"/>
			</xsl:for-each>
		</dcl:uniquenessConstraint>
	</xsl:template>

	<xsl:template match="rcd:ReferenceConstraint" mode="GenerateTableContent">
		<xsl:variable name="sourceTable" select="parent::rcd:Constraints/parent::rcd:Table"/>
		<xsl:variable name="targetTable" select="$sourceTable/parent::rcd:Tables/rcd:Table[@id = current()/rcd:TargetTable/@ref]"/>
		<dcl:referenceConstraint>
			<xsl:call-template name="AddNameAttributes"/>
			<xsl:attribute name="targetTable">
				<xsl:value-of select="dsf:makeValidIdentifier($targetTable/@Name)"/>
			</xsl:attribute>
			<xsl:for-each select="rcd:ColumnReferences/rcd:ColumnReference">
				<dcl:columnRef sourceName="{dsf:makeValidIdentifier($sourceTable/rcd:Columns/rcd:Column[@id = current()/@SourceColumn]/@Name)}" targetName="{dsf:makeValidIdentifier($targetTable/rcd:Columns/rcd:Column[@id = current()/@TargetColumn]/@Name)}"/>
			</xsl:for-each>
		</dcl:referenceConstraint>
	</xsl:template>

	<xsl:template name="BuildChildAndLeafHierarchy">
		<xsl:param name="startLinks"/>
		<xsl:param name="columns"/>
		<xsl:param name="contextConceptType"/>
		<xsl:param name="booleanFormats"/>
		<xsl:choose>
			<xsl:when test="$startLinks[oialtocdb:InverseConceptTypeChild or @AbsorptionIndicator[.='true' or .='1']]">
				<!-- Expand the starting set to get clean and annotated data in the main algorithm instead of
				attempting to extrapolate this information inline.
				1) Determine paired mandatory constraints for inverse children.
				2) Expand the absorption indicator with a trailing node so it can have a natural leaf.
				3) Mark whether a column requires comparison to true or false instead of null to determine if set.
				4) Create a full column path for a pseudo column of an absorption indicator with an inverse (both must be assimilations).
				5) Create a full path for an inverse assimilation that inverts an information type.
				   In this case, we would have an absorption indicator if the assimilation were the parent
				   and the child pointed to information type, but in this mixed case we always place the
				   information type as the parent. We will simply replicate the path. -->
				<xsl:variable name="expandedLinksFragment">
					<xsl:for-each select="$startLinks">
						<!-- We can assume root document context here, no need to push $Document -->
						<xsl:variable name="inverseChild" select="key('KeyedConceptTypeChildren',oialtocdb:InverseConceptTypeChild/@ref)"/>
						<xsl:variable name="pairFlagsFragment">
							<xsl:if test="$inverseChild">
								<!-- We don't known which end of the negation we're on from an oial perspective at this point, guess one
								to determine if the pair is mandatory.(m means pair is mandatory) -->
								<xsl:variable name="negation" select="$inverseChild/oial:negatesChild"/>
								<xsl:choose>
									<xsl:when test="$negation">
										<!-- The inverse is the negation. -->
										<xsl:if test="$negation[@pairIsMandatory[.='true' or .='1']]">
											<xsl:text>m</xsl:text>
										</xsl:if>
									</xsl:when>
									<xsl:when test="key('KeyedConceptTypeChildren',@ConceptTypeChild)/oial:negatesChild/@pairIsMandatory[.='true' or .='1']">
										<xsl:text>m</xsl:text>
									</xsl:when>
								</xsl:choose>

								<!-- Check if the inverse maps to a true or false value. (f means positive is false, reserve of normal) -->
								<xsl:choose>
									<xsl:when test="$inverseChild[self::oial:informationType]">
										<xsl:if test="$booleanFormats[@id=$inverseChild/@ref][self::odt:booleanTrue]">
											<xsl:text>f</xsl:text>
										</xsl:if>
									</xsl:when>
									<xsl:otherwise>
										<!-- Check for an information type on the containing child before resorting to multiple hops for a unary pattern test. -->
										<xsl:variable name="otherChild" select="key('KeyedConceptTypeChildren',@ConceptTypeChild)[self::oial:informationType]"/>
										<xsl:choose>
											<xsl:when test="$otherChild">
												<xsl:if test="$booleanFormats[@id=$otherChild/@ref][self::odt:booleanFalse]">
													<xsl:text>f</xsl:text>
												</xsl:if>
											</xsl:when>
											<xsl:otherwise>
												<!-- Fall back on pulling the path fact type via objectification -->
												<xsl:if test="key('KeyedFactTypes', key('KeyedObjectTypes',key('KeyedObjectTypeByConceptType',$inverseChild/@ref)/@ObjectType)[self::orm:ObjectifiedType]/orm:NestedPredicate/@ref)/@UnaryPattern[.!='Negation']">
													<xsl:text>f</xsl:text>
												</xsl:if>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:if>
						</xsl:variable>
						<xsl:variable name="pairFlags" select="string($pairFlagsFragment)"/>
						<xsl:variable name="pairIsMandatory" select="contains($pairFlags,'m')"/>
						<xsl:variable name="reverseNegation" select="contains($pairFlags,'f')"/>
						<xsl:choose>
							<xsl:when test="@AbsorptionIndicator[.='true' or .='1']">
								<!-- This is always a leaf. Add a pseudo column step so the absorption
								leaf can be an assimilating parent (leave off ConceptTypeChild).-->
								<xsl:copy>
									<xsl:copy-of select="@*[local-name()!='AbsorptionIndicator']"/>
								</xsl:copy>

								<xsl:variable name="columnId" select="string(@Column)"/>
								<xsl:choose>
									<xsl:when test="$inverseChild">
										<!-- Add a final pseudo step (ConceptTypeChild is not set) -->
										<oialtocdb:ColumnHasConceptTypeChild Column="{$columnId}" booleanTest="TRUE">
											<xsl:if test="$reverseNegation">
												<xsl:attribute name="booleanTest">
													<xsl:text>FALSE</xsl:text>
												</xsl:attribute>
											</xsl:if>
										</oialtocdb:ColumnHasConceptTypeChild>

										<!-- Create a new pseudo column with a modified column id and realColumn attribute. -->
										<xsl:for-each select="preceding-sibling::oialtocdb:ColumnHasConceptTypeChild[@Column=$columnId]">
											<oialtocdb:ColumnHasConceptTypeChild Column="{$columnId}_Inverse" realColumn="{$columnId}" ConceptTypeChild="{@ConceptTypeChild}"/>
										</xsl:for-each>
										<oialtocdb:ColumnHasConceptTypeChild Column="{$columnId}_Inverse" realColumn="{$columnId}" ConceptTypeChild="{$inverseChild/@id}"/>
										<oialtocdb:ColumnHasConceptTypeChild Column="{$columnId}_Inverse" realColumn="{$columnId}" booleanTest="FALSE">
											<xsl:if test="$reverseNegation">
												<xsl:attribute name="booleanTest">
													<xsl:text>TRUE</xsl:text>
												</xsl:attribute>
											</xsl:if>
										</oialtocdb:ColumnHasConceptTypeChild>

										<xsl:if test="$pairIsMandatory">
											<!-- Add another column that is mandatory in the containing path regardless of the true/false value. -->
											<xsl:for-each select="preceding-sibling::oialtocdb:ColumnHasConceptTypeChild[@Column=$columnId]">
												<oialtocdb:ColumnHasConceptTypeChild Column="{$columnId}_Mandatory" realColumn="{$columnId}" ConceptTypeChild="{@ConceptTypeChild}"/>
											</xsl:for-each>
											<oialtocdb:ColumnHasConceptTypeChild Column="{$columnId}_Mandatory" realColumn="{$columnId}" forceMandatory="1"/>
										</xsl:if>
									</xsl:when>
									<xsl:otherwise>
										<!-- This has true/null data, so a null check will work. -->
										<oialtocdb:ColumnHasConceptTypeChild Column="{$columnId}" booleanTest="NULL"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:when test="$inverseChild">
								<!-- The starting step will always reference an information type if there is no absorption indicator,
								but the inverse can be an assimilation. Create a new column path for this case, leaving the same
								structure as an assimilation. -->
								<xsl:choose>
									<xsl:when test="$inverseChild[self::oial:assimilatedConceptType]">
										<xsl:copy>
											<!-- In the context of the parent, the true/false column is simply tested as null/not null.
											We only care about an explicit true/false test when we're determining if the column is set
											for the true/false branch of one of the assimilated states. So, booleanTest is added for the
											assimilation path, but the current node is left with a null test. -->
											<xsl:copy-of select="@*"/>

											<!-- Give the real column information on mandatory state (used for parent assimilation, not this one). -->
											<xsl:if test="$pairIsMandatory">
												<xsl:attribute name="forceMandatory">
													<xsl:text>1</xsl:text>
												</xsl:attribute>
											</xsl:if>
										</xsl:copy>

										<!-- Create the pseudo column for the assimilation as in the absorption indicator case.-->
										<xsl:variable name="columnId" select="string(@Column)"/>
										<xsl:for-each select="preceding-sibling::oialtocdb:ColumnHasConceptTypeChild[@Column=$columnId]">
											<oialtocdb:ColumnHasConceptTypeChild Column="{$columnId}_Inverse" realColumn="{$columnId}" ConceptTypeChild="{@ConceptTypeChild}"/>
										</xsl:for-each>
										<oialtocdb:ColumnHasConceptTypeChild Column="{$columnId}_Inverse" realColumn="{$columnId}" ConceptTypeChild="{$inverseChild/@id}"/>
										<oialtocdb:ColumnHasConceptTypeChild Column="{$columnId}_Inverse" realColumn="{$columnId}" booleanTest="TRUE">
											<xsl:if test="$reverseNegation">
												<xsl:attribute name="booleanTest">
													<xsl:text>FALSE</xsl:text>
												</xsl:attribute>
											</xsl:if>
										</oialtocdb:ColumnHasConceptTypeChild>
									</xsl:when>
									<xsl:otherwise>
										<xsl:copy>
											<!-- No need to copy the inverse child. This is two information types, so an explicit
											true/false test is not needed, just use the stock null test. -->
											<xsl:copy-of select="@*"/>
											<xsl:if test="$pairIsMandatory">
												<xsl:attribute name="forceMandatory">
													<xsl:text>1</xsl:text>
												</xsl:attribute>
											</xsl:if>
										</xsl:copy>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:copy-of select="."/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:for-each>
				</xsl:variable>
				<xsl:call-template name="BuildChildAndLeafHierarchyRecurse">
					<xsl:with-param name="startLinks" select="exsl:node-set($expandedLinksFragment)/child::*"/>
					<xsl:with-param name="columns" select="$columns"/>
					<xsl:with-param name="contextConceptType" select="$contextConceptType"/>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="BuildChildAndLeafHierarchyRecurse">
					<xsl:with-param name="startLinks" select="$startLinks"/>
					<xsl:with-param name="columns" select="$columns"/>
					<xsl:with-param name="contextConceptType" select="$contextConceptType"/>
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="BuildChildAndLeafHierarchyRecurse">
		<xsl:param name="startLinks"/>
		<xsl:param name="columns"/>
		<xsl:param name="contextConceptType"/>
		<xsl:param name="startingMandatoryDepth" select="0"/>
		<xsl:param name="generateLeafNodes" select="false()"/>
		<xsl:param name="depth" select="1"/>
		<xsl:variable name="uniqueLeadConceptTypeChildFragment">
			<xsl:variable name="sortedLeadConceptTypeChildFragment">
				<xsl:for-each select="$startLinks[not(preceding-sibling::*[1]/@Column=@Column)]">
					<xsl:sort select="dsf:coalesce(@ConceptTypeChild,@Column)"/>
					<loc:sorting>
						<xsl:copy-of select="@forceMandatory"/>
						<xsl:choose>
							<xsl:when test="@ConceptTypeChild">
								<xsl:value-of select="@ConceptTypeChild"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:attribute name="columnTip">
									<xsl:text>1</xsl:text>
								</xsl:attribute>
								<!-- This will always be the final node in the column path. We need to provide some id. -->
								<xsl:value-of select="@Column"/>
							</xsl:otherwise>
						</xsl:choose>
					</loc:sorting>
				</xsl:for-each>
			</xsl:variable>
			<xsl:copy-of select="exsl:node-set($sortedLeadConceptTypeChildFragment)/child::*[not(following-sibling::*[1]=.)]"/>
		</xsl:variable>

		<xsl:for-each select="exsl:node-set($uniqueLeadConceptTypeChildFragment)/child::*">
			<xsl:variable name="sortNode" select="."/>
			<xsl:variable name="currentChildId" select="string(.)"/>
			<xsl:for-each select="$Document">
				<!-- Push context so that keyed lookups work correctly -->
				<xsl:variable name="currentConceptTypeChild" select="key('KeyedConceptTypeChildren',$currentChildId)"/>
				<xsl:variable name="currentAssimilation" select="$currentConceptTypeChild/self::oial:assimilatedConceptType"/>
				<xsl:variable name="parentConceptTypeId" select="$currentConceptTypeChild/parent::oial:children/parent::oial:conceptType/@id"/>
				<xsl:variable name="currentMandatoryAndNextFragment">
					<loc:dummy isMandatory="{$currentConceptTypeChild/@isMandatory}" nextComingFrom="{$currentConceptTypeChild/@ref}">
						<!-- For most cases, the mandatory state corresponds directly to the isMandatory property on the
						current concept type child. However, if the place we're coming from does not match the parent
						of a current assimilation, then we're walking in reverse and we always treat it as mandatory. Also,
						in this case, the next node maps to the assimilation parent. -->
						<xsl:choose>
							<xsl:when test="$currentAssimilation and not($contextConceptType=$parentConceptTypeId)">
								<xsl:attribute name="isMandatory">
									<xsl:text>true</xsl:text>
								</xsl:attribute>
								<xsl:attribute name="nextComingFrom">
									<xsl:value-of select="$parentConceptTypeId"/>
								</xsl:attribute>
							</xsl:when>
							<xsl:when test="$sortNode/@forceMandatory">
								<xsl:attribute name="isMandatory">
									<xsl:text>true</xsl:text>
								</xsl:attribute>
							</xsl:when>
						</xsl:choose>
					</loc:dummy>
				</xsl:variable>
				<xsl:variable name="currentMandatoryAndNext" select="exsl:node-set($currentMandatoryAndNextFragment)/child::*"/>
				<xsl:variable name="mandatoryDepthFragment">
					<xsl:choose>
						<xsl:when test="$currentMandatoryAndNext/@isMandatory='true'">
							<xsl:choose>
								<xsl:when test="$startingMandatoryDepth!=0">
									<xsl:value-of select="$startingMandatoryDepth" />
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$depth"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="0"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:variable name="mandatoryDepth" select="number($mandatoryDepthFragment)"/>
				<xsl:variable name="childLinksFragment">
					<xsl:for-each select="$startLinks[not(preceding-sibling::*[1]/@Column=@Column)][dsf:coalesce(@ConceptTypeChild,@Column)=$currentChildId]">
						<xsl:variable name="linksForColumn" select="$startLinks[@Column=current()/@Column]"/>
						<xsl:choose>
							<xsl:when test="count($linksForColumn)=1">
								<xsl:if test="$generateLeafNodes">
									<loc:leaf>
										<xsl:copy-of select="@*"/>
										<xsl:choose>
											<xsl:when test="@realColumn">
												<xsl:copy-of select="$columns[@id=current()/@realColumn]/@Name"/>
												<!-- This is an absorption indicator column or unary condition test. It is never mandatory. -->
											</xsl:when>
											<xsl:otherwise>
												<xsl:variable name="resolvedColumn" select="$columns[@id=current()/@Column]"/>
												<xsl:copy-of select="$resolvedColumn/@Name"/>
												<xsl:if test="not($resolvedColumn/@IsNullable[.='true' or .='1'])">
													<xsl:attribute name="alwaysMandatory">
														<xsl:value-of select="true()"/>
													</xsl:attribute>
												</xsl:if>
											</xsl:otherwise>
										</xsl:choose>
										<xsl:if test="$mandatoryDepth!=0">
											<xsl:attribute name="isMandatoryStartingAt">
												<xsl:value-of select="$mandatoryDepth"/>
											</xsl:attribute>
										</xsl:if>
									</loc:leaf>
								</xsl:if>
							</xsl:when>
							<xsl:otherwise>
								<xsl:copy-of select="$linksForColumn[position()!=1]"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:for-each>
				</xsl:variable>
				<xsl:variable name="childLinks" select="exsl:node-set($childLinksFragment)/child::*"/>
				<xsl:copy-of select="$childLinks/self::loc:leaf"/>

				<xsl:variable name="nextLinks" select="$childLinks/self::oialtocdb:ColumnHasConceptTypeChild"/>
				<xsl:if test="$nextLinks">
					<loc:child ConceptTypeChild="{$currentChildId}" ConceptTypeChildName="{key('KeyedConceptTypes',$currentConceptTypeChild/@ref)/@name}" IsMandatory="{$mandatoryDepth!=0}">
						<xsl:call-template name="BuildChildAndLeafHierarchyRecurse">
							<xsl:with-param name="startLinks" select="$nextLinks"/>
							<xsl:with-param name="columns" select="$columns"/>
							<xsl:with-param name="contextConceptType" select="string($currentMandatoryAndNext/@nextComingFrom)"/>
							<xsl:with-param name="startingMandatoryDepth" select="$mandatoryDepth"/>
							<xsl:with-param name="generateLeafNodes" select="true()"/>
							<xsl:with-param name="depth" select="$depth+1"/>
						</xsl:call-template>
					</loc:child>
				</xsl:if>
			</xsl:for-each>
		</xsl:for-each>
	</xsl:template>

	<xsl:template match="rcd:Table" mode="GenerateAbsorptionConstraints">
		<xsl:param name="oialDcilBridge"/>
		<xsl:param name="booleanFormats"/>
		<xsl:variable name="columns" select="rcd:Columns/rcd:Column"/>

		<xsl:variable name="nestedChildFragment">
			<xsl:call-template name="BuildChildAndLeafHierarchy">
				<xsl:with-param name="startLinks" select="$oialDcilBridge/oialtocdb:ColumnHasConceptTypeChild[@Column = $columns/@id]"/>
				<xsl:with-param name="columns" select="$columns"/>
				<xsl:with-param name="contextConceptType" select="string($oialDcilBridge/oialtocdb:TableIsPrimarilyForConceptType[@Table=current()/@id]/@ConceptType)"/>
				<xsl:with-param name="booleanFormats" select="$booleanFormats"/>
			</xsl:call-template>
		</xsl:variable>
		<xsl:variable name="nestedChildren" select="exsl:node-set($nestedChildFragment)/child::*"/>

		<xsl:if test="$nestedChildren">
			<xsl:variable name="absorptionCheckConstraintsFragment">
				<xsl:apply-templates select="$nestedChildren" mode="GenerateAssimilationCheckConstraints">
					<xsl:with-param name="columns" select="$columns"/>
					<xsl:with-param name="currentDepth" select="1"/>
				</xsl:apply-templates>
			</xsl:variable>
			<xsl:variable name="absorptionCheckConstraints" select="exsl:node-set($absorptionCheckConstraintsFragment)/child::*"/>
			<xsl:if test="$absorptionCheckConstraints">
				<xsl:variable name="tableName" select="string(@Name)"/>
				<xsl:for-each select="$absorptionCheckConstraints">
					<xsl:variable name="nameInstanceCount">
						<xsl:variable name="precedingCount" select="count(preceding-sibling::*[@name=current()/@name])"/>
						<xsl:choose>
							<xsl:when test="$precedingCount">
								<xsl:value-of select="$precedingCount+1"/>
							</xsl:when>
							<xsl:when test="following-sibling::*[@name=current()/@name][1]">
								<xsl:value-of select="1"/>
							</xsl:when>
						</xsl:choose>
					</xsl:variable>
					<dcl:checkConstraint>
						<xsl:call-template name="AddNameAttributes">
							<xsl:with-param name="requestedName" select="concat($tableName,'_',@name,$nameInstanceCount,'_MandatoryGroup')"/>
						</xsl:call-template>
						<xsl:copy-of select="@*[local-name()!='name']"/>
						<xsl:copy-of select="*"/>
					</dcl:checkConstraint>
				</xsl:for-each>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="loc:child" mode="GenerateAssimilationCheckConstraints">
		<xsl:param name="columns"/>
		<xsl:param name="currentDepth"/>

		<xsl:variable name="mandatoryAtDepthFragment">
			<xsl:variable name="rawFragment">
				<xsl:apply-templates mode="GetMandatoryAtDepthColumnOperation">
					<xsl:with-param name="columns" select="$columns"/>
					<xsl:with-param name="currentDepth" select="$currentDepth + 1"/>
				</xsl:apply-templates>
			</xsl:variable>
			<xsl:call-template name="ConsolidateColumnOperations">
				<xsl:with-param name="operations" select="exsl:node-set($rawFragment)/*"/>
			</xsl:call-template>
		</xsl:variable>
		<xsl:variable name="mandatoryAtDepth" select="exsl:node-set($mandatoryAtDepthFragment)/*"/>
		<xsl:if test="$mandatoryAtDepth">
			<xsl:variable name="optionalAtDepthFragment">
				<xsl:variable name="rawFragment">
					<xsl:apply-templates mode="GetOptionalAtDepthColumnOperations">
						<xsl:with-param name="columns" select="$columns"/>
						<xsl:with-param name="currentDepth" select="$currentDepth + 1"/>
					</xsl:apply-templates>
				</xsl:variable>
				<xsl:call-template name="ConsolidateColumnOperations">
					<xsl:with-param name="operations" select="exsl:node-set($rawFragment)/*"/>
				</xsl:call-template>
			</xsl:variable>
			<xsl:variable name="optionalAtDepth" select="exsl:node-set($optionalAtDepthFragment)/*"/>
			<xsl:if test="count($optionalAtDepth) >= 2">
				<dcl:checkConstraint name="{translate(normalize-space(@ConceptTypeChildName),' ','_')}">
					<dep:or>
						<xsl:call-template name="RenderColumnOperations">
							<xsl:with-param name="operations" select="$mandatoryAtDepth"/>
						</xsl:call-template>
						<xsl:call-template name="RenderColumnOperations">
							<xsl:with-param name="operations" select="$optionalAtDepth"/>
						</xsl:call-template>
					</dep:or>
				</dcl:checkConstraint>
			</xsl:if>
		</xsl:if>

		<!--
			UNDONE: There is potentially a reduction that can be made here.
			If there is only a single column (from the current level?) in the not null clause,
			that column does not need to be included in the null clause as well.
			Example:
				a IS NOT NULL OR b IS NULL AND a IS NULL
				can become
				a IS NOT NULL OR b IS NULL
		-->

		<xsl:apply-templates select="loc:child" mode="GenerateAssimilationCheckConstraints">
			<xsl:with-param name="columns" select="$columns"/>
			<xsl:with-param name="currentDepth" select="$currentDepth + 1"/>
		</xsl:apply-templates>

	</xsl:template>

	<xsl:template name="ConsolidateColumnOperations">
		<xsl:param name="operations"/>
		<!-- columnName/compareTo NULL TRUE FALSE/invert 1 -->
		<!-- This reduces multiple operations on the same column to a single
		operation. Note that the invert level is assumed to be consistent for
		all items in the set. -->
		<xsl:if test="$operations">
			<xsl:variable name="sortedOperationsFragment">
				<xsl:for-each select="$operations">
					<xsl:sort select="@columnName"/>
					<xsl:copy-of select="."/>
				</xsl:for-each>
			</xsl:variable>
			<xsl:variable name="sortedOperations" select="exsl:node-set($sortedOperationsFragment)/*"/>
			<xsl:for-each select="$sortedOperations[not(preceding-sibling::*[1]/@columnName=@columnName)]">
				<xsl:variable name="matchedOperations" select="following-sibling::*[@columnName=current()/@columnName]"/>
				<xsl:choose>
					<xsl:when test="$matchedOperations">
						<xsl:choose>
							<xsl:when test="@compareTo='NULL'">
								<xsl:copy-of select="."/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:variable name="otherNull" select="$matchedOperations[@compareTo='NULL']"/>
								<xsl:choose>
									<xsl:when test="$otherNull">
										<xsl:copy-of select="$otherNull[1]"/>
									</xsl:when>
									<xsl:when test="(@compareTo='FALSE' and $matchedOperations[@compareTo='TRUE']) or (@compareTo='TRUE' and $matchedOperations[@compareTo='FALSE'])">
										<loc:operation columnName="{@columnName}" compareTo="NULL">
											<xsl:if test="not(@invert)">
												<xsl:attribute name="invert">
													<xsl:text>1</xsl:text>
												</xsl:attribute>
											</xsl:if>
										</loc:operation>
									</xsl:when>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:copy-of select="."/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:for-each>
		</xsl:if>
	</xsl:template>

	<xsl:template name="RenderColumnOperations">
		<xsl:param name="operations"/>
		<xsl:param name="checkContainer" select="true()"/>
		<xsl:if test="$operations">
			<xsl:choose>
				<xsl:when test="$checkContainer and count($operations)!=1">
					<dep:and>
						<xsl:call-template name="RenderColumnOperations">
							<xsl:with-param name="operations" select="$operations"/>
							<xsl:with-param name="checkContainer" select="false()"/>
						</xsl:call-template>
					</dep:and>
				</xsl:when>
				<xsl:otherwise>
					<xsl:for-each select="$operations">
						<xsl:variable name="comparand" select="string(@compareTo)"/>
						<xsl:choose>
							<xsl:when test="$comparand='NULL'">
								<dep:nullPredicate type="NULL">
									<xsl:if test="@invert">
										<xsl:attribute name="type">
											<xsl:text>NOT NULL</xsl:text>
										</xsl:attribute>
									</xsl:if>
									<dep:columnReference name="{dsf:makeValidIdentifier(@columnName)}"/>
								</dep:nullPredicate>
							</xsl:when>
							<xsl:otherwise>
								<dep:comparisonPredicate operator="equals">
									<xsl:if test="@invert">
										<xsl:attribute name="operator">
											<xsl:text>notEquals</xsl:text>
										</xsl:attribute>
									</xsl:if>
									<dep:columnReference name="{dsf:makeValidIdentifier(@columnName)}"/>
									<ddt:booleanLiteral value="{$comparand}"/>
								</dep:comparisonPredicate>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:for-each>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>

	<xsl:template match="loc:child" mode="GetMandatoryAtDepthColumnOperation">
		<xsl:param name="columns"/>
		<xsl:param name="currentDepth"/>
		<xsl:param name="invert" select="false"/>
		<xsl:variable name="mandatoryNextStepColumnOperationsFragment">
			<xsl:apply-templates mode="GetMandatoryAtDepthColumnOperation">
				<xsl:with-param name="columns" select="$columns"/>
				<xsl:with-param name="currentDepth" select="$currentDepth"/>
				<xsl:with-param name="invert" select="$invert"/>
			</xsl:apply-templates>
		</xsl:variable>
		<xsl:copy-of select="exsl:node-set($mandatoryNextStepColumnOperationsFragment)/*[1]"/>
	</xsl:template>
	<xsl:template match="loc:leaf" mode="GetMandatoryAtDepthColumnOperation">
		<xsl:param name="columns"/>
		<xsl:param name="currentDepth"/>
		<xsl:param name="invert" select="false()"/>
		<xsl:if test="not(@alwaysMandatory) and @isMandatoryStartingAt &lt;= $currentDepth">
			<xsl:variable name="self" select="."/>
			<loc:operation columnName="{$columns[@id = dsf:coalesce($self/@realColumn,$self/@Column)]/@Name}" compareTo="NULL">
				<xsl:choose>
					<xsl:when test="@booleanTest[.!='NULL']">
						<xsl:attribute name="compareTo">
							<xsl:value-of select="@booleanTest"/>
						</xsl:attribute>
						<xsl:if test="@invert">
							<xsl:attribute name="invert">
								<xsl:text>1</xsl:text>
							</xsl:attribute>
						</xsl:if>
					</xsl:when>
					<xsl:when test="not(@invert)">
						<xsl:attribute name="invert">
							<!-- Compare to NOT NULL for an 'is set' test. -->
							<xsl:text>1</xsl:text>
						</xsl:attribute>
					</xsl:when>
				</xsl:choose>
			</loc:operation>
		</xsl:if>
	</xsl:template>
	<xsl:template match="loc:child" mode="GetOptionalAtDepthColumnOperations">
		<xsl:param name="columns"/>
		<xsl:param name="currentDepth"/>
		<xsl:variable name="mandatoryNextStepColumnOperationsFragment">
			<xsl:apply-templates select="." mode="GetMandatoryAtDepthColumnOperation">
				<xsl:with-param name="columns" select="$columns"/>
				<xsl:with-param name="currentDepth" select="$currentDepth"/>
				<xsl:with-param name="invert" select="true()"/>
			</xsl:apply-templates>
		</xsl:variable>
		<xsl:variable name="mandatoryNextStepColumnOperations" select="exsl:node-set(mandatoryNextStepColumnOperationsFragment)/*"/>
		<xsl:choose>
			<xsl:when test="$mandatoryNextStepColumnOperations">
				<xsl:copy-of select="$mandatoryNextStepColumnOperations[1]"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates mode="GetOptionalAtDepthColumnOperations">
					<xsl:with-param name="columns" select="$columns"/>
					<xsl:with-param name="currentDepth" select="$currentDepth + 1"/>
				</xsl:apply-templates>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="loc:leaf" mode="GetOptionalAtDepthColumnOperations">
		<xsl:param name="columns"/>
		<xsl:if test="not(@alwaysMandatory)">
			<xsl:variable name="self" select="."/>
			<loc:operation columnName="{$columns[@id = dsf:coalesce($self/@realColumn,$self/@Column)]/@Name}" compareTo="NULL">
				<xsl:if test="@booleanTest[.!='NULL']">
					<xsl:attribute name="compareTo">
						<xsl:value-of select="@booleanTest"/>
					</xsl:attribute>
					<xsl:attribute name="invert">
						<xsl:text>1</xsl:text>
					</xsl:attribute>
				</xsl:if>
			</loc:operation>
		</xsl:if>
	</xsl:template>

	<!-- Determine the true/false default for a unary column. This returns 't', 'f' or empty text. -->
	<xsl:template name="ColumnDefaultFromUnaryFactType">
		<xsl:param name="columnPathNode"/>
		<xsl:param name="factType"/>
		<xsl:variable name="unaryPattern" select="string($factType/@UnaryPattern)"/>
		<xsl:choose>
			<xsl:when test="$unaryPattern='Negation'">
				<xsl:call-template name="ColumnDefaultFromUnaryFactType">
					<xsl:with-param name="columnPathNode" select="$columnPathNode"/>
					<xsl:with-param name="factType" select="key('KeyedFactTypeNegationLink',$factType/@id)/.."/>
				</xsl:call-template>
			</xsl:when>
			<xsl:when test="contains($unaryPattern,'DefaultTrue')">
				<!-- It doesn't matter if we're paired or not, just use the true value. -->
				<xsl:text>t</xsl:text>
			</xsl:when>
			<xsl:when test="contains($unaryPattern,'DefaultFalse')">
				<xsl:choose>
					<xsl:when test="$columnPathNode[oialtocdb:InverseConceptTypeChild]">
						<!-- False is a valid default for a column that represents both true and false values -->
						<xsl:text>f</xsl:text>
					</xsl:when>
					<xsl:otherwise>
						<!-- An unpaired negation is always true. -->
						<xsl:text>t</xsl:text>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
		</xsl:choose>
	</xsl:template>

	<!-- Get intermediate loc:defaultValue and loc:valueConstraint nodes for a role. -->
	<xsl:template name="ValueDataFromRole">
		<xsl:param name="role"/>
		<xsl:param name="ignoreDefault"/>
		<xsl:param name="checkTypeDefault"/>
		<xsl:if test="$role/orm:ValueRestriction/orm:RoleValueConstraint[not(@Modality='Deontic')]">
			<loc:valueConstraint role="{$role/@id}"/>
			<!-- Note that value type value constraints are folded into domains -->
		</xsl:if>
		<xsl:if test="not($ignoreDefault)">
			<xsl:variable name="defaultState" select="string($role/@DefaultState)"/>
			<xsl:choose>
				<xsl:when test="$defaultState='EmptyValue'">
					<loc:defaultValue empty="1"/>
				</xsl:when>
				<xsl:when test="$defaultState='IgnoreContext'">
					<loc:defaultValue block="1"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:variable name="defaultValue" select="string($role/orm:DefaultValue)"/>
					<xsl:choose>
						<xsl:when test="$defaultValue">
							<loc:defaultValue value="{$defaultValue}" invariantValue="{$role/orm:InvariantDefaultValue}"/>
						</xsl:when>
						<xsl:when test="$checkTypeDefault">
							<xsl:for-each select="key('KeyedObjectTypes',$role/orm:RolePlayer/@ref)[self::orm:ValueType]">
								<xsl:choose>
									<xsl:when test="@DefaultState='EmptyValue'">
										<loc:defaultValue empty="1"/>
									</xsl:when>
									<!-- Value types do not support IgnoreContext (there is nothing to ignore) -->
									<xsl:otherwise>
										<xsl:variable name="typeDefaultValue" select="string($role/orm:DefaultValue)"/>
										<xsl:if test="$typeDefaultValue">
											<loc:defaultValue value="{$defaultValue}" invariantValue="{$role/orm:InvariantDefaultValue}"/>
										</xsl:if>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:for-each>
						</xsl:when>
					</xsl:choose>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>

	<xsl:template match="orm:ValueType" mode="GenerateDataTypeMapping">
		<xsl:param name="ormModel"/>
		<xsl:variable name="dataTypeName" select="@Name"/>
		<xsl:variable name="modelConceptualDataType" select="orm:ConceptualDataType"/>
		<xsl:variable name="modelDataType" select="$ormModel/orm:DataTypes/child::*[@id=$modelConceptualDataType/@ref]"/>
		<xsl:variable name="modelValueConstraint" select="orm:ValueRestriction/orm:ValueConstraint[not(@Modality='Deontic')]"/>
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
					<xsl:when test="$modelValueRanges and not($hasTrueConstraint and $hasFalseConstraint)">
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
				$modelDataType/self::orm:UnsignedTinyIntegerNumericDataType or
				$modelDataType/self::orm:SignedSmallIntegerNumericDataType or
				$modelDataType/self::orm:UnsignedSmallIntegerNumericDataType or
				$modelDataType/self::orm:SignedIntegerNumericDataType or
				$modelDataType/self::orm:UnsignedIntegerNumericDataType or
				$modelDataType/self::orm:SignedLargeIntegerNumericDataType or
				$modelDataType/self::orm:UnsignedLargeIntegerNumericDataType or
				$modelDataType/self::orm:DecimalNumericDataType or
				$modelDataType/self::orm:MoneyNumericDataType or
				$modelDataType/self::orm:UUIDNumericDataType">

				<!-- AutoGenerated is explicitly added for a handful of integer types where it is optional. It is not specified with auto counter use, which implies it. -->
				<xsl:if test="$modelDataType/self::orm:AutoCounterNumericDataType or $modelDataType/self::orm:RowIdOtherDataType or $modelConceptualDataType/@AutoGenerated[.='true' or .=1]">
					<xsl:attribute name="isIdentity">
						<xsl:value-of select="true()"/>
					</xsl:attribute>
				</xsl:if>

				<xsl:variable name="isIntegral" select="
					$modelDataType/self::orm:AutoCounterNumericDataType or
					$modelDataType/self::orm:RowIdOtherDataType or
					$modelDataType/self::orm:UnsignedTinyIntegerNumericDataType or
					$modelDataType/self::orm:SignedSmallIntegerNumericDataType or
					$modelDataType/self::orm:UnsignedSmallIntegerNumericDataType or
					$modelDataType/self::orm:SignedIntegerNumericDataType or
					$modelDataType/self::orm:UnsignedIntegerNumericDataType or
					$modelDataType/self::orm:SignedLargeIntegerNumericDataType or
					$modelDataType/self::orm:UnsignedLargeIntegerNumericDataType"/>

				<!-- TINYINT is unsigned already, so we don't need additional enforcement on it. -->
				<xsl:variable name="needsUnsignedEnforcement" select="
					$modelDataType/self::orm:UnsignedSmallIntegerNumericDataType or
					$modelDataType/self::orm:UnsignedIntegerNumericDataType or
					$modelDataType/self::orm:UnsignedLargeIntegerNumericDataType"/>

				<xsl:variable name="predefinedDataType">
					<dcl:predefinedDataType>
						<xsl:choose>
							<xsl:when test="$isIntegral">
								<xsl:attribute name="name">
									<xsl:choose>
										<xsl:when test="$modelDataType/self::orm:UnsignedTinyIntegerNumericDataType">
											<xsl:text>TINYINT</xsl:text>
										</xsl:when>
										<xsl:when test="$modelDataType/self::orm:SignedSmallIntegerNumericDataType or $modelDataType/self::orm:UnsignedSmallIntegerNumericDataType">
											<xsl:text>SMALLINT</xsl:text>
										</xsl:when>
										<xsl:when test="$modelDataType/self::orm:SignedLargeIntegerNumericDataType or $modelDataType/self::orm:UnsignedLargeIntegerNumericDataType">
											<xsl:text>BIGINT</xsl:text>
										</xsl:when>
										<xsl:otherwise>
											<xsl:text>INTEGER</xsl:text>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:attribute>
							</xsl:when>
							<xsl:when test="$modelDataType/self::orm:UUIDNumericDataType">
								<xsl:attribute name="name">
									<xsl:text>UNIQUEIDENTIFIER</xsl:text>
								</xsl:attribute>
							</xsl:when>
							<xsl:when test="$modelDataType/self::orm:DecimalNumericDataType or $modelDataType/self::orm:MoneyNumericDataType">
								<xsl:attribute name="name">
									<xsl:text>DECIMAL</xsl:text>
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
													<!-- Make sure that we don't make the scale greater than the length, if we have one. -->
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
					<xsl:when test="$modelValueRanges or $needsUnsignedEnforcement">
						<dcl:domain>
							<xsl:call-template name="AddNameAttributes">
								<xsl:with-param name="requestedName" select="$dataTypeName"/>
							</xsl:call-template>
							<xsl:copy-of select="exsl:node-set($predefinedDataType)"/>
							<xsl:if test="$needsUnsignedEnforcement and not($modelValueRanges)">
								<dcl:checkConstraint>
									<xsl:call-template name="AddNameAttributes">
										<xsl:with-param name="requestedName" select="concat($dataTypeName, '_Unsigned_Chk')"/>
									</xsl:call-template>
									<dep:comparisonPredicate operator="greaterThanOrEquals">
										<dep:valueKeyword/>
										<ddt:exactNumericLiteral value="0"/>
									</dep:comparisonPredicate>
								</dcl:checkConstraint>
							</xsl:if>
							<xsl:if test="$modelValueRanges">
								<dcl:checkConstraint>
									<xsl:call-template name="AddNameAttributes">
										<xsl:with-param name="requestedName" select="$modelValueConstraint/@Name"/>
									</xsl:call-template>
									<xsl:choose>
										<xsl:when test="$needsUnsignedEnforcement">
											<dep:and>
												<dep:comparisonPredicate operator="greaterThanOrEquals">
													<dep:valueKeyword/>
													<ddt:exactNumericLiteral value="0"/>
												</dep:comparisonPredicate>
												<xsl:call-template name="ProcessValueConstraintRanges">
													<xsl:with-param name="literalName" select="'exactNumericLiteral'"/>
													<xsl:with-param name="valueRanges" select="$modelValueRanges"/>
												</xsl:call-template>
											</dep:and>
										</xsl:when>
										<xsl:otherwise>
											<xsl:call-template name="ProcessValueConstraintRanges">
												<xsl:with-param name="literalName" select="'exactNumericLiteral'"/>
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
						<dcl:domain>
							<xsl:call-template name="AddNameAttributes">
								<xsl:with-param name="requestedName" select="$dataTypeName"/>
							</xsl:call-template>
							<xsl:copy-of select="exsl:node-set($predefinedDataType)"/>
							<dcl:checkConstraint>
								<xsl:call-template name="AddNameAttributes">
									<xsl:with-param name="requestedName" select="$modelValueConstraint/@Name"/>
								</xsl:call-template>
								<xsl:call-template name="ProcessValueConstraintRanges">
									<xsl:with-param name="literalName" select="'approximateNumericLiteral'"/>
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
									<xsl:text>CHARACTER</xsl:text>
								</xsl:when>
								<xsl:when test="$modelDataType/self::orm:VariableLengthTextDataType">
									<xsl:text>CHARACTER VARYING</xsl:text>
								</xsl:when>
								<xsl:when test="$modelDataType/self::orm:LargeLengthTextDataType">
									<xsl:text>CHARACTER LARGE OBJECT</xsl:text>
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
						<dcl:domain>
							<xsl:call-template name="AddNameAttributes">
								<xsl:with-param name="requestedName" select="$dataTypeName"/>
							</xsl:call-template>
							<xsl:copy-of select="exsl:node-set($predefinedDataType)"/>
							<dcl:checkConstraint>
								<xsl:call-template name="AddNameAttributes">
									<xsl:with-param name="requestedName" select="$modelValueConstraint/@Name"/>
								</xsl:call-template>
								<!-- This may or may not work for actual ranges (where @MinValue != @MaxValue). -->
								<xsl:call-template name="ProcessValueConstraintRanges">
									<xsl:with-param name="literalName" select="'characterStringLiteral'"/>
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

				<xsl:variable name="predefinedDataType">
					<dcl:predefinedDataType>
						<xsl:attribute name="name">
							<xsl:choose>
								<xsl:when test="$modelDataType/self::orm:FixedLengthRawDataDataType">
									<xsl:text>BINARY</xsl:text>
								</xsl:when>
								<xsl:when test="$modelDataType/self::orm:LargeLengthRawDataDataType">
									<xsl:text>BINARY LARGE OBJECT</xsl:text>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>BINARY VARYING</xsl:text>
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

				<xsl:copy-of select="exsl:node-set($predefinedDataType)"/>
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
						<xsl:when test="$modelDataType/self::orm:AutoTimestampTemporalDataType">
							<xsl:text>TIMESTAMP</xsl:text>
						</xsl:when>
						<xsl:when test="$modelDataType/self::orm:DateAndTimeTemporalDataType">
							<xsl:text>DATETIME</xsl:text>
						</xsl:when>
						<xsl:when test="$modelDataType/self::orm:TimeTemporalDataType">
							<xsl:text>TIME</xsl:text>
						</xsl:when>
						<xsl:when test="$modelDataType/self::orm:DateTemporalDataType">
							<xsl:text>DATE</xsl:text>
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
						<dcl:domain>
							<xsl:call-template name="AddNameAttributes">
								<xsl:with-param name="requestedName" select="$dataTypeName"/>
							</xsl:call-template>
							<dcl:predefinedDataType name="{$predefinedDataTypeName}"/>
							<dcl:checkConstraint>
								<xsl:call-template name="AddNameAttributes">
									<xsl:with-param name="requestedName" select="$modelValueConstraint/@Name"/>
								</xsl:call-template>
								<xsl:call-template name="ProcessValueConstraintRanges">
									<xsl:with-param name="literalName" select="concat(translate($predefinedDataTypeName, 'DATEIMSAP', 'dateimsap'), 'Literal')"/>
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
		<xsl:param name="valueReference"/>

		<xsl:variable name="realValueReferenceFragment">
			<xsl:choose>
				<xsl:when test="$valueReference">
					<xsl:copy-of select="$valueReference"/>
				</xsl:when>
				<xsl:otherwise>
					<dep:valueKeyword/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="realValueReference" select="exsl:node-set($realValueReferenceFragment)/child::*"/>
		<xsl:variable name="valueRangesForIn" select="$valueRanges[string-length(@MinValue) and string-length(@MaxValue) and (@MinValue = @MaxValue)]"/>
		<xsl:variable name="valueRangesForComparisons" select="$valueRanges[not(string-length(@MinValue)) or not(string-length(@MaxValue)) or not(@MinValue = @MaxValue)]"/>

		<xsl:choose>
			<xsl:when test="$valueRangesForIn and $valueRangesForComparisons">
				<dep:or>
					<xsl:call-template name="ProcessValueConstraintRangesForIn">
						<xsl:with-param name="literalName" select="$literalName"/>
						<xsl:with-param name="valueRanges" select="$valueRangesForIn"/>
						<xsl:with-param name="valueReference" select="$realValueReference"/>
					</xsl:call-template>
					<xsl:call-template name="ProcessValueConstraintRangesForComparisons">
						<xsl:with-param name="literalName" select="$literalName"/>
						<xsl:with-param name="valueRanges" select="$valueRangesForComparisons"/>
						<xsl:with-param name="valueReference" select="$realValueReference"/>
					</xsl:call-template>
				</dep:or>
			</xsl:when>
			<xsl:when test="$valueRangesForIn and not($valueRangesForComparisons)">
				<xsl:call-template name="ProcessValueConstraintRangesForIn">
					<xsl:with-param name="literalName" select="$literalName"/>
					<xsl:with-param name="valueRanges" select="$valueRangesForIn"/>
					<xsl:with-param name="valueReference" select="$realValueReference"/>
				</xsl:call-template>
			</xsl:when>
			<xsl:when test="not($valueRangesForIn) and $valueRangesForComparisons">
				<xsl:call-template name="ProcessValueConstraintRangesForComparisons">
					<xsl:with-param name="literalName" select="$literalName"/>
					<xsl:with-param name="valueRanges" select="$valueRangesForComparisons"/>
					<xsl:with-param name="valueReference" select="$realValueReference"/>
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
		<xsl:param name="valueReference"/>
		<dep:inPredicate type="IN">
			<xsl:copy-of select="$valueReference"/>
			<xsl:for-each select="$valueRanges">
				<xsl:element name="ddt:{$literalName}">
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
		<xsl:param name="valueReference"/>
		<xsl:param name="currentNr" select="1"/>
		<xsl:param name="totalNr" select="count($valueRanges)"/>
		<xsl:variable name="rangeCode">
			<xsl:variable name="currentRange" select="$valueRanges[$currentNr]"/>
			<xsl:variable name="lowerBoundLiteral">
				<xsl:if test="string-length($currentRange/@MinValue)">
					<xsl:element name="ddt:{$literalName}">
						<xsl:attribute name="value">
							<xsl:value-of select="$currentRange/@MinValue"/>
						</xsl:attribute>
					</xsl:element>
				</xsl:if>
			</xsl:variable>
			<xsl:variable name="upperBoundLiteral">
				<xsl:if test="string-length($currentRange/@MaxValue)">
					<xsl:element name="ddt:{$literalName}">
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
							<xsl:text>greaterThan</xsl:text>
						</xsl:when>
						<xsl:otherwise>
							<xsl:text>greaterThanOrEquals</xsl:text>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:if>
			</xsl:variable>
			<xsl:variable name="upperBoundOperator">
				<xsl:if test="string-length($currentRange/@MaxValue)">
					<xsl:choose>
						<xsl:when test="$currentRange/@MaxInclusion='Open'">
							<xsl:text>lessThan</xsl:text>
						</xsl:when>
						<xsl:otherwise>
							<xsl:text>lessThanOrEquals</xsl:text>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:if>
			</xsl:variable>
			<xsl:choose>
				<xsl:when test="$lowerBoundOperator='greaterThanOrEquals' and $upperBoundOperator='lessThanOrEquals'">
					<dep:betweenPredicate type="BETWEEN">
						<xsl:copy-of select="$valueReference"/>
						<xsl:copy-of select="exsl:node-set($lowerBoundLiteral)"/>
						<xsl:copy-of select="exsl:node-set($upperBoundLiteral)"/>
					</dep:betweenPredicate>
				</xsl:when>
				<xsl:when test="string-length($lowerBoundOperator) and string-length($upperBoundOperator)">
					<dep:and>
						<dep:comparisonPredicate operator="{$lowerBoundOperator}">
							<xsl:copy-of select="$valueReference"/>
							<xsl:copy-of select="exsl:node-set($lowerBoundLiteral)"/>
						</dep:comparisonPredicate>
						<dep:comparisonPredicate operator="{$upperBoundOperator}">
							<xsl:copy-of select="$valueReference"/>
							<xsl:copy-of select="exsl:node-set($upperBoundLiteral)"/>
						</dep:comparisonPredicate>
					</dep:and>
				</xsl:when>
				<xsl:when test="string-length($lowerBoundOperator)">
					<dep:comparisonPredicate operator="{$lowerBoundOperator}">
						<xsl:copy-of select="$valueReference"/>
						<xsl:copy-of select="exsl:node-set($lowerBoundLiteral)"/>
					</dep:comparisonPredicate>
				</xsl:when>
				<xsl:when test="string-length($upperBoundOperator)">
					<dep:comparisonPredicate operator="{$upperBoundOperator}">
						<xsl:copy-of select="$valueReference"/>
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
						<xsl:with-param name="valueReference" select="$valueReference"/>
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
	<xsl:template name="AddNameAttributes">
		<xsl:param name="requestedName" select="string(@Name)"/>
		<xsl:variable name="decoratedName" select="dsf:makeValidIdentifier($requestedName)"/>
		<xsl:attribute name="name">
			<xsl:value-of select="$decoratedName"/>
		</xsl:attribute>
		<xsl:if test="$decoratedName!=$requestedName">
			<xsl:attribute name="requestedName">
				<xsl:value-of select="$requestedName"/>
			</xsl:attribute>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>
