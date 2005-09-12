<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:orm="http://Schemas.Neumont.edu/ORM/ORMCore"
	xmlns:ormRoot="http://Schemas.Neumont.edu/ORM/ORMRoot"
	xmlns:plx="http://Schemas.Neumont.edu/CodeGeneration/Plix"
	xmlns:ao="http://Schemas.Neumont.edu/ORM/SDK/ClassGenerator/AbsorbedObjects">
	<xsl:include href="ModelContextCode.xslt"/>

	<xsl:template match="ao:Object" mode="ForGenerateImplementationClass">
		<xsl:param name="Model"/>
		<xsl:param name="ModelContextName"/>
		<!-- TODO: Is this test necessary? Is it even possible to have an ao:Object that isn't an EntityType or an ObjectifiedType? -->
		<xsl:if test="@type='EntityType' or @type='ObjectifiedType'">
			<xsl:call-template name="GenerateImplementationClass">
				<xsl:with-param name="Model" select="$Model"/>
				<xsl:with-param name="ModelContextName" select="$ModelContextName"/>
				<xsl:with-param name="className" select="@name"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
	<xsl:template match="ao:Association" mode="ForGenerateImplementationClass">
		<xsl:param name="Model"/>
		<xsl:param name="ModelContextName"/>
		<xsl:call-template name="GenerateImplementationClass">
			<xsl:with-param name="Model" select="$Model"/>
			<xsl:with-param name="ModelContextName" select="$ModelContextName"/>
			<xsl:with-param name="className" select="concat(@name,$AssociationClassSuffix)"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template name="GenerateImplementation">
		<xsl:param name="ModelContextName"/>
		<xsl:param name="ModelDeserializationName"/>
		<plx:Class visibility="Public" sealed="true" name="{$ModelContextName}">
			<plx:ImplementsInterface dataTypeName="I{$ModelContextName}"/>
			<plx:Function ctor="true" visibility="Public"/>
			<plx:Field name="{$PrivateMemberPrefix}IsDeserializing" visibility="Private" dataTypeName="Boolean" dataTypeQualifier="System"/>
			<xsl:call-template name="GenerateModelContextMethods">
				<xsl:with-param name="Model" select="."/>
				<xsl:with-param name="ModelContextName" select="$ModelContextName"/>
			</xsl:call-template>
			<xsl:apply-templates mode="ForGenerateImplementationClass" select="$AbsorbedObjects">
				<xsl:with-param name="Model" select="."/>
				<xsl:with-param name="ModelContextName" select="$ModelContextName"/>
			</xsl:apply-templates>
			<xsl:call-template name="GenerateDeserializationFactoryClass">
				<xsl:with-param name="Model" select="."/>
				<xsl:with-param name="ModelContextName" select="$ModelContextName"/>
				<xsl:with-param name="ModelDeserializationName" select="$ModelDeserializationName"/>
			</xsl:call-template>
		</plx:Class>
	</xsl:template>

	<!--Build the DeserializationFactory class-->
	<xsl:template name="GenerateDeserializationFactoryClass">
		<xsl:param name="Model"/>
		<xsl:param name="ModelContextName"/>
		<xsl:param name="ModelDeserializationName"/>
		<plx:Function name="BeginDeserialization" visibility="Private">
			<plx:Param name="" type="RetVal" dataTypeName="I{$ModelDeserializationName}"/>
			<plx:InterfaceMember member="BeginDeserialization" dataTypeName="I{$ModelContextName}"/>
			<plx:Return>
				<plx:CallNew dataTypeName="DeserializationFactory">
					<plx:PassParam>
						<plx:ThisKeyword/>
					</plx:PassParam>
				</plx:CallNew>
			</plx:Return>
		</plx:Function>
		<plx:Class visibility="Private" name="DeserializationFactory">
			<plx:ImplementsInterface dataTypeName="I{$ModelDeserializationName}"/>
			<plx:Field name="{$PrivateMemberPrefix}Context" visibility="Private" dataTypeName="{$ModelContextName}"/>
			<plx:Function ctor="true" visibility="Public">
				<plx:Param type="In" name="context" dataTypeName="{$ModelContextName}"/>
				<plx:Operator type="Assign">
					<plx:Left>
						<plx:CallInstance name="{$PrivateMemberPrefix}Context" type="Field">
							<plx:CallObject>
								<plx:ThisKeyword/>
							</plx:CallObject>
						</plx:CallInstance>
					</plx:Left>
					<plx:Right>
						<plx:Value type="Parameter" data="context"/>
					</plx:Right>
				</plx:Operator>
				<plx:Operator type="Assign">
					<plx:Left>
						<plx:CallInstance name="{$PrivateMemberPrefix}IsDeserializing" type="Field">
							<plx:CallObject>
								<plx:Value type="Parameter" data="context"/>
							</plx:CallObject>
						</plx:CallInstance>
					</plx:Left>
					<plx:Right>
						<plx:TrueKeyword/>
					</plx:Right>
				</plx:Operator>
			</plx:Function>
			<plx:Function name="Dispose" visibility="Private">
				<plx:InterfaceMember dataTypeName="IDisposable" member="Dispose"/>
				<plx:Operator type="Assign">
					<plx:Left>
						<plx:CallInstance name="{$PrivateMemberPrefix}IsDeserializing" type="Field">
							<plx:CallObject>
								<plx:CallInstance name="{$PrivateMemberPrefix}Context" type="Field">
									<plx:CallObject>
										<plx:ThisKeyword/>
									</plx:CallObject>
								</plx:CallInstance>
							</plx:CallObject>
						</plx:CallInstance>
					</plx:Left>
					<plx:Right>
						<plx:FalseKeyword/>
					</plx:Right>
				</plx:Operator>
			</plx:Function>
			<xsl:apply-templates mode="GenerateDeserializationContextMethods" select="$AbsorbedObjects">
				<xsl:with-param name="Model" select="$Model"/>
				<xsl:with-param name="ModelDeserializationName" select="$ModelDeserializationName"/>
			</xsl:apply-templates>
		</plx:Class>
		<plx:Property visibility="Public" name="IsDeserializing">
			<plx:Param type="RetVal" name="" dataTypeName="Boolean" dataTypeQualifier="System"/>
			<plx:Get>
				<plx:Return>
					<plx:CallInstance name="{$PrivateMemberPrefix}IsDeserializing" type="Field">
						<plx:CallObject>
							<plx:ThisKeyword/>
						</plx:CallObject>
					</plx:CallInstance>
				</plx:Return>
			</plx:Get>
		</plx:Property>
	</xsl:template>

	<!--Template applied to ao:Object nodes to kick off GenerateDeserializationContextMethod
	with the appropriate class name-->
	<xsl:template match="ao:Object" mode="GenerateDeserializationContextMethods">
		<xsl:param name="Model"/>
		<xsl:param name="ModelDeserializationName"/>
		<xsl:call-template name="GenerateDeserializationContextMethod">
			<xsl:with-param name="ClassName" select="@name"/>
			<xsl:with-param name="Model" select="$Model"/>
			<xsl:with-param name="ModelDeserializationName" select="$ModelDeserializationName"/>
		</xsl:call-template>
	</xsl:template>
	<!--Template applied to ao:Association nodes to kick off GenerateDeserializationContextMethod
	with the appropriate class name-->
	<xsl:template match="ao:Association" mode="GenerateDeserializationContextMethods">
		<xsl:param name="Model"/>
		<xsl:param name="ModelDeserializationName"/>
		<xsl:call-template name="GenerateDeserializationContextMethod">
			<xsl:with-param name="ClassName" select="concat(@name,$AssociationClassSuffix)"/>
			<xsl:with-param name="Model" select="$Model"/>
			<xsl:with-param name="ModelDeserializationName" select="$ModelDeserializationName"/>
		</xsl:call-template>
	</xsl:template>

	<!--Build the Deserialization methods of the DeserializationFactory class for constructing new
	core objects.-->
	<xsl:template name="GenerateDeserializationContextMethod">
		<xsl:param name="Model"/>
		<xsl:param name="ClassName"/>
		<xsl:param name="ModelDeserializationName"/>
		<xsl:variable name="propertiesFragment">
			<xsl:apply-templates select="child::*" mode="TransformPropertyObjects">
				<xsl:with-param name="Model" select="$Model"/>
			</xsl:apply-templates>
		</xsl:variable>
		<xsl:variable name="properties" select="msxsl:node-set($propertiesFragment)/child::*"/>
		<plx:Function name="Create{$ClassName}" visibility="Private">
			<plx:InterfaceMember member="Create{$ClassName}" dataTypeName="I{$ModelDeserializationName}"/>
			<plx:Param type="RetVal" name="" dataTypeName="{$ClassName}"/>
			<xsl:variable name="mandatoryParametersFragment">
				<xsl:call-template name="GenerateMandatoryParameters">
					<xsl:with-param name="properties" select="$properties"/>
					<xsl:with-param name="nullPlaceholders" select="true()"/>
				</xsl:call-template>
			</xsl:variable>
			<xsl:variable name="mandatoryParameters" select="msxsl:node-set($mandatoryParametersFragment)"/>
			<xsl:copy-of select="$mandatoryParameters/plx:Param"/>
			<plx:Return>
				<plx:CallNew dataTypeName="{$ClassName}{$ImplementationClassSuffix}">
					<plx:PassParam>
						<plx:Value type="Local" data="{$PrivateMemberPrefix}Context"/>
					</plx:PassParam>
					<xsl:for-each select="$mandatoryParameters/child::*">
						<xsl:choose>
							<!--Change plx:Param tags from the GenerateMandatoryParameters 
							template to plx:PassParam tags-->
							<xsl:when test="local-name()='Param'">
								<plx:PassParam>
									<plx:Value type="Parameter" data="{@name}"/>
								</plx:PassParam>
							</xsl:when>
							<xsl:otherwise>
								<xsl:copy-of select="."/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:for-each>
				</plx:CallNew>
			</plx:Return>
		</plx:Function>
	</xsl:template>

	<xsl:template name="GenerateModelContextMethods">
		<xsl:param name="Model"/>
		<xsl:param name="ModelContextName"/>
		<xsl:call-template name="BuildExternalUniquenessConstraintValidationFunctions">
			<xsl:with-param name="Model" select="$Model"/>
			<xsl:with-param name="ModelContextName" select="$ModelContextName"/>
		</xsl:call-template>
		<xsl:call-template name="BuildAssociationUniquenessConstraintValidationFunctions">
			<xsl:with-param name="Model" select="$Model"/>
			<xsl:with-param name="ModelContextName" select="$ModelContextName"/>
		</xsl:call-template>
		<xsl:call-template name="BuildValueConstraintValidationFunctions"/>
	</xsl:template>
	
	<xsl:template name="BuildExternalUniquenessConstraintValidationFunctions">
		<xsl:param name="Model"/>
		<xsl:param name="ModelContextName"/>
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
			<!-- TODO: Only pass external uniqueness constraints that are composed of simple binaries to the following template-->
			<xsl:call-template name="GenerateSimpleBinaryUniquenessChangeMethods">
				<xsl:with-param name="Model" select="$Model"/>
				<xsl:with-param name="uniqueObjectName" select="$uniqueObjectName"/>
				<xsl:with-param name="parameters" select="$parameters"/>
			</xsl:call-template>
			<!-- TODO: Only pass external uniqueness constraints that are composed of simple binaries to the following template-->
			<xsl:call-template name="GenerateSimpleBinaryUniquenessLookupMethod">
				<xsl:with-param name="ModelContextName" select="$ModelContextName"/>
				<xsl:with-param name="uniqueObjectName" select="$uniqueObjectName"/>
				<xsl:with-param name="parameters" select="$parameters"/>
			</xsl:call-template>
		</xsl:for-each>
	</xsl:template>

	<xsl:template name="BuildAssociationUniquenessConstraintValidationFunctions">
		<xsl:param name="Model"/>
		<xsl:param name="ModelContextName"/>
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
				<!-- TODO: Only pass external uniqueness constraints to the following template if they are composed of simple binaries -->
				<xsl:call-template name="GenerateSimpleBinaryUniquenessChangeMethods">
					<xsl:with-param name="Model" select="$Model"/>
					<xsl:with-param name="uniqueObjectName" select="$uniqueObjectName"/>
					<xsl:with-param name="parameters" select="$parameters"/>
				</xsl:call-template>
				<!-- TODO: Only pass external uniqueness constraints to the following template if they are composed of simple binaries -->
				<xsl:call-template name="GenerateSimpleBinaryUniquenessLookupMethod">
					<xsl:with-param name="ModelContextName" select="$ModelContextName"/>
					<xsl:with-param name="uniqueObjectName" select="$uniqueObjectName"/>
					<xsl:with-param name="parameters" select="$parameters"/>
				</xsl:call-template>
			</xsl:for-each>
		</xsl:for-each>
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
					<xsl:call-template name="GenerateValueConstraintMethod">
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
				<xsl:call-template name="GenerateValueConstraintMethod">
					<xsl:with-param name="FunctionName" select="concat($ValueConstraintFor,$valueTypeName)"/>
					<xsl:with-param name="DataType" select="msxsl:node-set($dataTypeFragment)/child::*"/>
				</xsl:call-template>
			</xsl:for-each>
		</xsl:for-each>
	</xsl:template>
	<xsl:template name="GenerateValueConstraintMethod">
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
								<plx:CallNew dataTypeName="ArgumentOutOfRangeException" dataTypeQualifier="System" type="New"/>
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

</xsl:stylesheet>
