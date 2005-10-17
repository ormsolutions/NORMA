<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:orm="http://schemas.neumont.edu/ORM/ORMCore"
	xmlns:ormRoot="http://schemas.neumont.edu/ORM/ORMRoot"
	xmlns:plx="http://schemas.neumont.edu/CodeGeneration/PLiX"
	xmlns:ao="http://schemas.neumont.edu/ORM/SDK/ClassGenerator/AbsorbedObjects">
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
	<!--Template applied to ao:Object nodes to kick off GenerateDeserializationContextMethod
	with the appropriate class name-->
	<xsl:template match="ao:Object" mode="ForGenerateDeserializationContextMethod">
		<xsl:param name="Model"/>
		<xsl:param name="ModelDeserializationName"/>
		<xsl:call-template name="GenerateDeserializationContextMethod">
			<xsl:with-param name="Model" select="$Model"/>
			<xsl:with-param name="ModelDeserializationName" select="$ModelDeserializationName"/>
			<xsl:with-param name="className" select="@name"/>
		</xsl:call-template>
	</xsl:template>
	<!--Template applied to ao:Association nodes to kick off GenerateDeserializationContextMethod
	with the appropriate class name-->
	<xsl:template match="ao:Association" mode="ForGenerateDeserializationContextMethod">
		<xsl:param name="Model"/>
		<xsl:param name="ModelDeserializationName"/>
		<xsl:call-template name="GenerateDeserializationContextMethod">
			<xsl:with-param name="Model" select="$Model"/>
			<xsl:with-param name="ModelDeserializationName" select="$ModelDeserializationName"/>
			<xsl:with-param name="className" select="concat(@name,$AssociationClassSuffix)"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template name="GenerateImplementation">
		<xsl:param name="ModelContextName"/>
		<xsl:param name="ModelDeserializationName"/>
		<plx:class visibility="public" modifier="sealed" name="{$ModelContextName}">
			<plx:leadingInfo>
				<plx:pragma type="region" data="{$ModelContextName}"/>
			</plx:leadingInfo>
			<plx:trailingInfo>
				<plx:pragma type="closeRegion" data="{$ModelContextName}"/>
			</plx:trailingInfo>
			<plx:implementsInterface dataTypeName="I{$ModelContextName}"/>
			<plx:function name=".construct"  visibility="public"/>
			<plx:function visibility="{$ModelContextInterfaceImplementationVisibility}" name="BeginDeserialization">
				<plx:interfaceMember memberName="BeginDeserialization" dataTypeName="I{$ModelContextName}"/>
				<plx:returns dataTypeName="I{$ModelDeserializationName}"/>
				<plx:return>
					<plx:callNew dataTypeName="{$ModelDeserializationName}">
						<plx:passParam>
							<plx:thisKeyword/>
						</plx:passParam>
					</plx:callNew>
				</plx:return>
			</plx:function>
			<plx:field name="{$PrivateMemberPrefix}IsDeserializing" visibility="private" dataTypeName=".boolean"/>
			<plx:property visibility="{$ModelContextInterfaceImplementationVisibility}" name="IsDeserializing">
				<plx:interfaceMember memberName="IsDeserializing" dataTypeName="I{$ModelContextName}"/>
				<plx:returns dataTypeName=".boolean"/>
				<plx:get>
					<plx:return>
						<plx:callThis name="{$PrivateMemberPrefix}IsDeserializing" type="field"/>
					</plx:return>
				</plx:get>
			</plx:property>
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
		</plx:class>
	</xsl:template>

	<!--Build the DeserializationFactory class-->
	<xsl:template name="GenerateDeserializationFactoryClass">
		<xsl:param name="Model"/>
		<xsl:param name="ModelContextName"/>
		<xsl:param name="ModelDeserializationName"/>
		<plx:class visibility="private" modifier="sealed" name="{$ModelDeserializationName}">
			<plx:leadingInfo>
				<plx:pragma type="region" data="{$ModelDeserializationName}"/>
			</plx:leadingInfo>
			<plx:trailingInfo>
				<plx:pragma type="closeRegion" data="{$ModelDeserializationName}"/>
			</plx:trailingInfo>
			<plx:implementsInterface dataTypeName="I{$ModelDeserializationName}"/>
			<plx:field name="{$PrivateMemberPrefix}Context" visibility="private" dataTypeName="{$ModelContextName}"/>
			<plx:function visibility="public" name=".construct">
				<plx:param name="context" dataTypeName="{$ModelContextName}"/>
				<plx:assign>
					<plx:left>
						<plx:callThis name="{$PrivateMemberPrefix}Context" type="field"/>
					</plx:left>
					<plx:right>
						<plx:nameRef type="parameter" name="context"/>
					</plx:right>
				</plx:assign>
				<plx:assign>
					<plx:left>
						<plx:callInstance name="{$PrivateMemberPrefix}IsDeserializing" type="field">
							<plx:callObject>
								<plx:nameRef type="parameter" name="context"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:left>
					<plx:right>
						<plx:trueKeyword/>
					</plx:right>
				</plx:assign>
			</plx:function>
			<plx:function visibility="public" name="Dispose">
				<plx:interfaceMember dataTypeName="IDisposable" memberName="Dispose"/>
				<plx:assign>
					<plx:left>
						<plx:callInstance type="field"  name="{$PrivateMemberPrefix}IsDeserializing">
							<plx:callObject>
								<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}Context"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:left>
					<plx:right>
						<plx:falseKeyword/>
					</plx:right>
				</plx:assign>
			</plx:function>
			<xsl:apply-templates select="$AbsorbedObjects" mode="ForGenerateDeserializationContextMethod">
				<xsl:with-param name="Model" select="$Model"/>
				<xsl:with-param name="ModelDeserializationName" select="$ModelDeserializationName"/>
			</xsl:apply-templates>
		</plx:class>
	</xsl:template>

	<!--Build the Deserialization methods of the DeserializationFactory class for constructing new
	core objects.-->
	<xsl:template name="GenerateDeserializationContextMethod">
		<xsl:param name="Model"/>
		<xsl:param name="ModelDeserializationName"/>
		<xsl:param name="className"/>
		<xsl:variable name="propertiesFragment">
			<xsl:apply-templates select="child::*" mode="TransformPropertyObjects">
				<xsl:with-param name="Model" select="$Model"/>
			</xsl:apply-templates>
		</xsl:variable>
		<xsl:variable name="properties" select="msxsl:node-set($propertiesFragment)/child::*"/>
		<plx:function visibility="public" name="Create{$className}">
			<plx:interfaceMember memberName="Create{$className}" dataTypeName="I{$ModelDeserializationName}"/>
			<xsl:variable name="mandatoryParametersFragment">
				<xsl:call-template name="GenerateMandatoryParameters">
					<xsl:with-param name="properties" select="$properties"/>
					<xsl:with-param name="nullPlaceholders" select="true()"/>
				</xsl:call-template>
			</xsl:variable>
			<xsl:variable name="mandatoryParameters" select="msxsl:node-set($mandatoryParametersFragment)"/>
			<xsl:copy-of select="$mandatoryParameters/plx:param"/>
			<plx:returns dataTypeName="{$className}"/>
			<plx:return>
				<plx:callNew dataTypeName="{$className}{$ImplementationClassSuffix}">
					<plx:passParam>
						<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}Context"/>
					</plx:passParam>
					<xsl:for-each select="$mandatoryParameters/child::*">
						<xsl:choose>
							<!--Change plx:param tags from the GenerateMandatoryParameters 
							template to plx:passParam tags-->
							<xsl:when test="local-name()='param'">
								<plx:passParam>
									<plx:nameRef type="parameter" name="{@name}"/>
								</plx:passParam>
							</xsl:when>
							<xsl:otherwise>
								<xsl:copy-of select="."/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:for-each>
				</plx:callNew>
			</plx:return>
		</plx:function>
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

	<xsl:template name="GetValueConstraintValidationFunctionNameForRole">
		<xsl:param name="Role"/>
		<xsl:variable name="allRoles" select="$Role/.."/>
		<xsl:variable name="parentFact" select="$allRoles/.."/>
		<xsl:variable name="roleId" select="$Role/@id"/>
		<xsl:variable name="rolePosition">
			<xsl:for-each select="$allRoles/child::*">
				<xsl:if test="@id=$roleId">
					<xsl:value-of select="position()"/>
				</xsl:if>
			</xsl:for-each>
		</xsl:variable>
		<xsl:value-of select="$RoleValueConstraintFor"/>
		<xsl:value-of select="$parentFact/@Name"/>
		<xsl:text>Role</xsl:text>
		<xsl:value-of select="$rolePosition"/>
	</xsl:template>

	<xsl:template name="FindValueConstraintForAbsorbedObjectRecursively">
		<xsl:param name="Model"/>
		<xsl:param name="entityType"/>
		<xsl:variable name="preferredIdentifierInternalUniquenessConstraint" select="$Model/orm:Facts/orm:Fact/orm:InternalConstraints/orm:InternalUniquenessConstraint[@id=$entityType/orm:PreferredIdentifier/@ref]"/>
		<xsl:if test="count($preferredIdentifierInternalUniquenessConstraint/orm:RoleSequence/orm:Role)=1">
			<xsl:variable name="preferredIdentifierInternalUniquenessConstraintFactRolePlayerRef" select="$preferredIdentifierInternalUniquenessConstraint/../../orm:FactRoles/orm:Role[@id=$preferredIdentifierInternalUniquenessConstraint/orm:RoleSequence/orm:Role/@ref]/orm:RolePlayer/@ref"/>
			<xsl:variable name="preferredIdentifierInternalUniquenessConstraintFactRolePlayerObject" select="$Model/orm:Objects/child::node()[@id=$preferredIdentifierInternalUniquenessConstraintFactRolePlayerRef]"/>
			<xsl:choose>
				<xsl:when test="local-name($preferredIdentifierInternalUniquenessConstraintFactRolePlayerObject)='EntityType'">
					<xsl:call-template name="FindValueConstraintForAbsorbedObjectRecursively">
						<xsl:with-param name="Model" select="$Model"/>
						<xsl:with-param name="entityType" select="$preferredIdentifierInternalUniquenessConstraintFactRolePlayerObject"/>
					</xsl:call-template>
				</xsl:when>
				<xsl:when test="local-name($preferredIdentifierInternalUniquenessConstraintFactRolePlayerObject)='ValueType'">
					<!-- Don't even think of removing orm:ValueConstraint/.. from the select in the next line -->
					<xsl:value-of select="$preferredIdentifierInternalUniquenessConstraintFactRolePlayerObject/orm:ValueConstraint/../@Name"/>
				</xsl:when>
			</xsl:choose>
		</xsl:if>
	</xsl:template>

	<xsl:template name="GetValueTypeValueConstraintCode">
		<xsl:param name="ModelContextName"/>
		<xsl:param name="name"/>
		<plx:branch>
			<plx:condition>
				<plx:unaryOperator type="booleanNot">
					<plx:callStatic dataTypeName="{$ModelContextName}" name="{$ValueConstraintFor}{$name}">
						<plx:passParam>
							<plx:nameRef type="parameter" name="newValue"/>
						</plx:passParam>
						<plx:passParam>
							<plx:nameRef type="parameter" name="throwOnFailure"/>
						</plx:passParam>
					</plx:callStatic>
				</plx:unaryOperator>
			</plx:condition>
			<plx:body>
				<plx:return>
					<plx:falseKeyword/>
				</plx:return>
			</plx:body>
		</plx:branch>
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
		<xsl:variable name="isStringValue" select="$valueType='string'"/>
		<plx:function modifier="static" name="{$FunctionName}" visibility="private">
			<xsl:call-template name="GenerateSuppressMessageAttribute">
				<xsl:with-param name="category" select="'Microsoft.Usage'"/>
				<xsl:with-param name="checkId" select="'CA2208'"/>
			</xsl:call-template>
			<plx:param name="value">
				<xsl:copy-of select="$DataType/@*"/>
				<xsl:copy-of select="$DataType/child::*"/>
			</plx:param>
			<plx:param name="throwOnFailure" dataTypeName=".boolean"/>
			<plx:returns dataTypeName=".boolean"/>
			<xsl:variable name="ValueRangeOperatorsFragment">
				<xsl:for-each select="orm:ValueRanges/orm:ValueRange">
					<xsl:choose>
						<xsl:when test="@MinValue=@MaxValue">
							<plx:binaryOperator type="equality">
								<plx:left>
									<plx:nameRef type="parameter" name="value"/>
								</plx:left>
								<plx:right>
									<xsl:choose>
										<xsl:when test="$isStringValue">
											<plx:string>
												<xsl:value-of select="@MinValue"/>
											</plx:string>
										</xsl:when>
										<xsl:otherwise>
											<plx:value type="{$valueType}" data="{@MinValue}"/>
										</xsl:otherwise>
									</xsl:choose>
								</plx:right>
							</plx:binaryOperator>
						</xsl:when>
						<xsl:otherwise>
							<plx:binaryOperator type="booleanAnd">
								<plx:left>
									<plx:binaryOperator type="lessThanOrEqual">
										<xsl:if test="@MinInclusion='Open'">
											<xsl:attribute name="type">
												<xsl:text>lessThan</xsl:text>
											</xsl:attribute>
										</xsl:if>
										<plx:left>
											<xsl:choose>
												<xsl:when test="$isStringValue">
													<plx:string>
														<xsl:value-of select="@MinValue"/>
													</plx:string>
												</xsl:when>
												<xsl:otherwise>
													<plx:value type="{$valueType}" data="{@MinValue}"/>
												</xsl:otherwise>
											</xsl:choose>
										</plx:left>
										<plx:right>
											<plx:nameRef type="parameter" name="value"/>
										</plx:right>
									</plx:binaryOperator>
								</plx:left>
								<plx:right>
									<plx:binaryOperator type="lessThanOrEqual">
										<xsl:if test="@MaxInclusion='Open'">
											<xsl:attribute name="type">
												<xsl:text>lessThan</xsl:text>
											</xsl:attribute>
										</xsl:if>
										<plx:left>
											<plx:nameRef type="parameter" name="value"/>
										</plx:left>
										<plx:right>
											<xsl:choose>
												<xsl:when test="$isStringValue">
													<plx:string>
														<xsl:value-of select="@MaxValue"/>
													</plx:string>
												</xsl:when>
												<xsl:otherwise>
													<plx:value type="{$valueType}" data="{@MaxValue}"/>
												</xsl:otherwise>
											</xsl:choose>
										</plx:right>
									</plx:binaryOperator>
								</plx:right>
							</plx:binaryOperator>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:for-each>
			</xsl:variable>
			<plx:branch>
				<plx:condition>
					<plx:unaryOperator type="booleanNot">
						<xsl:for-each select="msxsl:node-set($ValueRangeOperatorsFragment)/child::*">
							<xsl:if test="position()=1">
								<xsl:call-template name="CombineElements">
									<xsl:with-param name="OperatorType" select="'booleanOr'"/>
								</xsl:call-template>
							</xsl:if>
						</xsl:for-each>
					</plx:unaryOperator>
				</plx:condition>
				<plx:body>
					<plx:branch>
						<plx:condition>
							<plx:nameRef type="parameter" name="throwOnFailure"/>
						</plx:condition>
						<plx:body>
							<plx:throw>
								<plx:callNew dataTypeName="ArgumentOutOfRangeException" dataTypeQualifier="System"/>
							</plx:throw>
						</plx:body>
					</plx:branch>
					<plx:return>
						<plx:falseKeyword/>
					</plx:return>
				</plx:body>
			</plx:branch>
			<plx:return>
				<plx:trueKeyword/>
			</plx:return>
		</plx:function>
	</xsl:template>
	<xsl:template name="CombineElements">
		<xsl:param name="OperatorType"/>
		<xsl:choose>
			<xsl:when test="position()=last()">
				<xsl:copy-of select="."/>
			</xsl:when>
			<xsl:otherwise>
				<plx:binaryOperator type="{$OperatorType}">
					<plx:left>
						<xsl:copy-of select="."/>
					</plx:left>
					<plx:right>
						<xsl:for-each select="following-sibling::*">
							<xsl:if test="position()=1">
								<xsl:call-template name="CombineElements">
									<xsl:with-param name="OperatorType" select="$OperatorType"/>
								</xsl:call-template>
							</xsl:if>
						</xsl:for-each>
					</plx:right>
				</plx:binaryOperator>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

</xsl:stylesheet>
