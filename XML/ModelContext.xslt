<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:orm="http://Schemas.Neumont.edu/ORM/ORMCore"
	xmlns:ormRoot="http://Schemas.Neumont.edu/ORM/ORMRoot"
	xmlns:plx="http://Schemas.Neumont.edu/CodeGeneration/Plix"
	xmlns:ao="http://Schemas.Neumont.edu/ORM/SDK/ClassGenerator/AbsorbedObjects">
	<xsl:variable name="ModelContextName" select="concat(ormRoot:ORM2/orm:ORMModel/@Name,'Context')"/>
	<xsl:template match="orm:ORMModel" mode="ModelContext">
		<plx:Class visibility="Public" partial="true" name="{$ModelContextName}">
			<xsl:call-template name="BuildValueConstraintValidationFunctions"/>
		</plx:Class>
	</xsl:template>
	<xsl:template name="BuildValueConstraintValidationFunctions">
		<xsl:variable name="cacheDataTypes" select="orm:DataTypes/child::*"/>
		<xsl:variable name="cacheValueTypes" select="orm:Objects/orm:ValueType"/>
		<!-- Role Value Constraints -->
		<xsl:for-each select="orm:Facts/orm:Fact">
			<xsl:variable name="factName" select="@Name"/>
			<xsl:for-each select="orm:FactRoles/orm:Role">
				<xsl:variable name="currentRole" select="."/>
				<xsl:variable name="roleId" select="concat('Role',position())"/>
				<xsl:for-each select="orm:ValueConstraint/orm:RoleValueRangeDefinition">
					<xsl:variable name="dataTypeFragment">
						<xsl:for-each select="$cacheDataTypes[@id=$cacheValueTypes[@id=$currentRole/orm:RolePlayer/@ref]/orm:ConceptualDataType/@ref]">
							<xsl:call-template name="MapDataType"/>
						</xsl:for-each>
					</xsl:variable>
					<xsl:call-template name="GenerateValueConstraintFunction">
						<xsl:with-param name="FunctionName" select="concat($RoleValueConstraintFor,$factName,$roleId)"/>
						<xsl:with-param name="DataType" select="msxsl:node-set($dataTypeFragment)/child::*"/>
					</xsl:call-template>
				</xsl:for-each>
			</xsl:for-each>
		</xsl:for-each>
		<!-- Value Type Value Constraints -->
		<xsl:for-each select="$cacheValueTypes">
			<xsl:variable name="valueTypeName" select="@Name"/>
			<xsl:variable name="dataTypeId" select="orm:ConceptualDataType/@ref"/>
			<xsl:variable name="dataTypeFragment">
				<xsl:for-each select="$cacheDataTypes[@id=$dataTypeId]">
					<xsl:call-template name="MapDataType"/>
				</xsl:for-each>
			</xsl:variable>
			<xsl:for-each select="orm:ValueConstraint/orm:ValueRangeDefinition">
				<xsl:call-template name="GenerateValueConstraintFunction">
					<xsl:with-param name="FunctionName" select="concat($ValueConstraintFor,$valueTypeName)"/>
					<xsl:with-param name="DataType" select="msxsl:node-set($dataTypeFragment)/child::*"/>
				</xsl:call-template>
			</xsl:for-each>
		</xsl:for-each>
	</xsl:template>
	<xsl:template name="GenerateValueConstraintFunction">
		<xsl:param name="FunctionName"/>
		<xsl:param name="DataType"/>
		<xsl:variable name="valueType">
			<xsl:call-template name="DataTypeToPlixValueType">
				<xsl:with-param name="DataType" select="$DataType"/>
			</xsl:call-template>
		</xsl:variable>
		<xsl:variable name="isStringValue" select="$valueType='String'"/>
		<plx:Function static="true" name="{$FunctionName}" visibility="Private">
			<plx:Param name="" type="RetVal" dataTypeName="Boolean" dataTypeQualifier="System"/>
			<plx:Param name="value" type="In">
				<xsl:copy-of select="$DataType/@*"/>
				<xsl:copy-of select="$DataType/child::*"/>
			</plx:Param>
			<plx:Param name="throwOnFailure" dataTypeName="Boolean" dataTypeQualifier="System"/>
			<xsl:variable name="ValueRangeOperatorsFragment">
				<xsl:for-each select="orm:ValueRanges/orm:ValueRange">
					<xsl:choose>
						<xsl:when test="@MinValue=@MaxValue">
							<plx:Operator type="Equality">
								<plx:Left>
									<plx:Value type="Parameter" data="value"/>
								</plx:Left>
								<plx:Right>
									<xsl:choose>
										<xsl:when test="$isStringValue">
											<plx:String>
												<xsl:value-of select="@MinValue"/>
											</plx:String>
										</xsl:when>
										<xsl:otherwise>
											<plx:Value type="{$valueType}" data="{@MinValue}"/>
										</xsl:otherwise>
									</xsl:choose>
								</plx:Right>
							</plx:Operator>
						</xsl:when>
						<xsl:otherwise>
							<plx:Operator type="BooleanAnd">
								<plx:Left>
									<plx:Operator type="LessThanOrEqual">
										<xsl:if test="@MinInclusion='Open'">
											<xsl:attribute name="type">
												<xsl:text>LessThan</xsl:text>
											</xsl:attribute>
										</xsl:if>
										<plx:Left>
											<xsl:choose>
												<xsl:when test="$isStringValue">
													<plx:String>
														<xsl:value-of select="@MinValue"/>
													</plx:String>
												</xsl:when>
												<xsl:otherwise>
													<plx:Value type="{$valueType}" data="{@MinValue}"/>
												</xsl:otherwise>
											</xsl:choose>
										</plx:Left>
										<plx:Right>
											<plx:Value type="Parameter" data="value"/>
										</plx:Right>
									</plx:Operator>
								</plx:Left>
								<plx:Right>
									<plx:Operator type="LessThanOrEqual">
										<xsl:if test="@MaxInclusion='Open'">
											<xsl:attribute name="type">
												<xsl:text>LessThan</xsl:text>
											</xsl:attribute>
										</xsl:if>
										<plx:Left>
											<plx:Value type="Parameter" data="value"/>
										</plx:Left>
										<plx:Right>
											<xsl:choose>
												<xsl:when test="$isStringValue">
													<plx:String>
														<xsl:value-of select="@MaxValue"/>
													</plx:String>
												</xsl:when>
												<xsl:otherwise>
													<plx:Value type="{$valueType}" data="{@MaxValue}"/>
												</xsl:otherwise>
											</xsl:choose>
										</plx:Right>
									</plx:Operator>
								</plx:Right>
							</plx:Operator>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:for-each>
			</xsl:variable>
			<plx:Condition>
				<plx:Test>
					<plx:Operator type="BooleanNot">
						<xsl:for-each select="msxsl:node-set($ValueRangeOperatorsFragment)/child::*">
							<xsl:if test="position()=1">
								<xsl:call-template name="CombineElements">
									<xsl:with-param name="OperatorType" select="'BooleanOr'"/>
								</xsl:call-template>
							</xsl:if>
						</xsl:for-each>
					</plx:Operator>
				</plx:Test>
				<plx:Body>
					<plx:Condition>
						<plx:Test>
							<plx:Value type="Parameter" data="throwOnFailure"/>
						</plx:Test>
						<plx:Body>
							<plx:Throw>
								<plx:CallNew dataTypeName="ArgumentOutOfRangeException" type="New"/>
							</plx:Throw>
						</plx:Body>
					</plx:Condition>
					<plx:Return>
						<plx:FalseKeyword/>
					</plx:Return>
				</plx:Body>
			</plx:Condition>
			<plx:Return>
				<plx:TrueKeyword/>
			</plx:Return>
		</plx:Function>
	</xsl:template>
	<xsl:template name="CombineElements">
		<xsl:param name="OperatorType"/>
		<xsl:choose>
			<xsl:when test="position()=last()">
				<xsl:copy-of select="."/>
			</xsl:when>
			<xsl:otherwise>
				<plx:Operator type="{$OperatorType}">
					<plx:Left>
						<xsl:copy-of select="."/>
					</plx:Left>
					<plx:Right>
						<xsl:for-each select="following-sibling::*">
							<xsl:if test="position()=1">
								<xsl:call-template name="CombineElements">
									<xsl:with-param name="OperatorType" select="$OperatorType"/>
								</xsl:call-template>
							</xsl:if>
						</xsl:for-each>
					</plx:Right>
				</plx:Operator>
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
</xsl:stylesheet>