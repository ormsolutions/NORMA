<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:orm="http://schemas.neumont.edu/ORM/ORMCore"
	xmlns:ormRoot="http://schemas.neumont.edu/ORM/ORMRoot"
	xmlns:plx="http://schemas.neumont.edu/CodeGeneration/PLiX"
	xmlns:ao="http://schemas.neumont.edu/ORM/SDK/ClassGenerator/AbsorbedObjects">


	<xsl:template name="GenerateImplementationConstructor">
		<xsl:param name="properties"/>
		<xsl:param name="className"/>
		<xsl:param name="ModelContextName"/>
		<plx:function name=".construct"  visibility="public">
			<plx:param name="context" dataTypeName="{$ModelContextName}"/>
			<xsl:variable name="mandatoryParameters">
				<xsl:call-template name="GenerateMandatoryParameters">
					<xsl:with-param name="properties" select="$properties"/>
				</xsl:call-template>
			</xsl:variable>
			<xsl:copy-of select="$mandatoryParameters"/>
			<plx:assign>
				<plx:left>
					<plx:callThis name="{$PrivateMemberPrefix}Context" type="field"/>
				</plx:left>
				<plx:right>
					<plx:nameRef type="parameter" name="context"/>
				</plx:right>
			</plx:assign>
			<xsl:for-each select="msxsl:node-set($mandatoryParameters)/child::*">
				<plx:assign>
					<plx:left>
						<plx:callThis type="field" name="{$PrivateMemberPrefix}{@name}"/>
					</plx:left>
					<plx:right>
						<plx:nameRef type="parameter" name="{@name}"/>
					</plx:right>
				</plx:assign>
				<plx:callInstance type="methodCall" name="On{$className}{@name}Changed">
					<plx:callObject>
						<plx:callThis type="property" name="Context"/>
					</plx:callObject>
					<plx:passParam>
						<plx:thisKeyword/>
					</plx:passParam>
				</plx:callInstance>
			</xsl:for-each>
			<plx:callInstance type="methodCall" name="Add">
				<plx:callObject>
					<plx:callInstance type="field" name="{$PrivateMemberPrefix}{$className}Collection">
						<plx:callObject>
							<plx:nameRef type="parameter" name="context"/>
						</plx:callObject>
					</plx:callInstance>
				</plx:callObject>
				<plx:passParam>
					<plx:thisKeyword/>
				</plx:passParam>
			</plx:callInstance>
		</plx:function>
	</xsl:template>
	<xsl:template name="GenerateFactoryMethod">
		<xsl:param name="ModelContextName"/>
		<xsl:param name="properties"/>
		<xsl:param name="className"/>
		<plx:function name="Create{$className}" visibility="{$ModelContextInterfaceImplementationVisibility}">
			<xsl:call-template name="GenerateSuppressMessageAttribute">
				<xsl:with-param name="category" select="'Microsoft.Globalization'"/>
				<xsl:with-param name="checkId" select="'CA1303'"/>
			</xsl:call-template>
			<plx:interfaceMember memberName="Create{$className}" dataTypeName="I{$ModelContextName}"/>
			<xsl:variable name="mandatoryParametersFragment">
				<xsl:call-template name="GenerateMandatoryParameters">
					<xsl:with-param name="properties" select="$properties"/>
				</xsl:call-template>
			</xsl:variable>
			<xsl:variable name="mandatoryParameters" select="msxsl:node-set($mandatoryParametersFragment)/child::*"/>
			<xsl:copy-of select="$mandatoryParameters"/>
			<plx:returns dataTypeName="{$className}"/>
			<plx:branch>
				<plx:condition>
					<xsl:choose>
						<xsl:when test="starts-with($ModelContextInterfaceImplementationVisibility,'private')">
							<plx:callInstance type="property" name="IsDeserializing">
								<plx:callObject>
									<plx:cast type="testCast" dataTypeName="I{$ModelContextName}">
										<plx:thisKeyword/>
									</plx:cast>
								</plx:callObject>
							</plx:callInstance>
						</xsl:when>
						<xsl:otherwise>
							<plx:callThis accessor="this" type="property" name="IsDeserializing"/>
						</xsl:otherwise>
					</xsl:choose>
				</plx:condition>
				<plx:throw>
					<plx:callNew dataTypeName="InvalidOperationException">
						<plx:passParam>
							<plx:string>This factory method cannot be called while IsDeserializing returns true.</plx:string>
						</plx:passParam>
					</plx:callNew>
				</plx:throw>
			</plx:branch>
			<!-- UNDONE: We currently aren't validating multi-role constraints prior to object creation. -->
			<xsl:for-each select="$mandatoryParameters">
				<plx:branch>
					<plx:condition>
						<plx:unaryOperator type="booleanNot">
							<plx:callThis name="On{$className}{@name}Changing">
								<plx:passParam>
									<plx:nullKeyword/>
								</plx:passParam>
								<plx:passParam>
									<plx:nameRef type="parameter" name="{@name}"/>
								</plx:passParam>
								<plx:passParam>
									<plx:trueKeyword/>
								</plx:passParam>
							</plx:callThis>
						</plx:unaryOperator>
					</plx:condition>
					<plx:throw>
						<!-- Not all constraints are capable of throwing on their own,
						so we provide a default exception for them here if they return false. -->
						<plx:callNew dataTypeName="ArgumentException" dataTypeQualifier="System">
							<plx:passParam>
								<plx:string>An argument failed constraint enforcement.</plx:string>
							</plx:passParam>
							<plx:passParam>
								<plx:string>
									<xsl:value-of select="@name"/>
								</plx:string>
							</plx:passParam>
						</plx:callNew>
					</plx:throw>
				</plx:branch>
			</xsl:for-each>
			<plx:return>
				<plx:callNew dataTypeName="{$className}{$ImplementationClassSuffix}">
					<plx:passParam>
						<plx:thisKeyword/>
					</plx:passParam>
					<xsl:for-each select="$mandatoryParameters">
						<plx:passParam>
							<plx:nameRef type="parameter" name="{@name}"/>
						</plx:passParam>
					</xsl:for-each>
				</plx:callNew>
			</plx:return>
		</plx:function>
	</xsl:template>

	<xsl:template name="GenerateImplementationClass">
		<xsl:param name="Model"/>
		<xsl:param name="ModelContextName"/>
		<xsl:param name="className"/>
		<xsl:variable name="implementationClassName" select="concat($className,$ImplementationClassSuffix)"/>
		<xsl:variable name="propertiesFragment">
			<xsl:apply-templates select="child::*" mode="TransformPropertyObjects">
				<xsl:with-param name="Model" select="$Model"/>
			</xsl:apply-templates>
		</xsl:variable>
		<xsl:variable name="properties" select="msxsl:node-set($propertiesFragment)/child::*"/>
		<!--<xsl:variable name="AbsorbedMandatory" select="$Model/orm:ORMModel/orm:Facts/orm:Fact/orm:FactRoles/orm:Role[@_IsMandatory='true']/@DataType"/>-->
		<xsl:for-each select="$properties">
			<xsl:call-template name="GenerateImplementationSimpleLookupMethod">
				<xsl:with-param name="className" select="$className"/>
				<xsl:with-param name="ModelContextName" select="$ModelContextName"/>
			</xsl:call-template>
			<xsl:call-template name="GenerateImplementationPropertyChangeMethods">
				<xsl:with-param name="className" select="$className"/>
				<xsl:with-param name="Model" select="$Model"/>
				<xsl:with-param name="ModelContextName" select="$ModelContextName"/>
			</xsl:call-template>
		</xsl:for-each>
		<xsl:call-template name="GenerateFactoryMethod">
			<xsl:with-param name="ModelContextName" select="$ModelContextName"/>
			<xsl:with-param name="properties" select="$properties"/>
			<xsl:with-param name="className" select="$className"/>
		</xsl:call-template>
		<plx:field visibility="private" readOnly="true" name="{$PrivateMemberPrefix}{$className}Collection" dataTypeName="List">
			<plx:passTypeParam dataTypeName="{$className}"/>
			<plx:leadingInfo>
				<plx:pragma type="region" data="{$className}"/>
			</plx:leadingInfo>
			<plx:initialize>
				<plx:callNew dataTypeName="List">
					<plx:passTypeParam dataTypeName="{$className}"/>
				</plx:callNew>
			</plx:initialize>
		</plx:field>
		<plx:property visibility="{$ModelContextInterfaceImplementationVisibility}" name="{$className}Collection">
			<plx:interfaceMember memberName="{$className}Collection" dataTypeName="I{$ModelContextName}"/>
			<plx:returns dataTypeName="ReadOnlyCollection">
				<plx:passTypeParam dataTypeName="{$className}"/>
			</plx:returns>
			<plx:get>
				<plx:return>
					<plx:callInstance name="AsReadOnly">
						<plx:callObject>
							<plx:callThis type="field" name="{$PrivateMemberPrefix}{$className}Collection"/>
						</plx:callObject>
					</plx:callInstance>
				</plx:return>
			</plx:get>
		</plx:property>
		<plx:class visibility="private" modifier="sealed" name="{$implementationClassName}">
			<plx:trailingInfo>
				<plx:pragma type="closeRegion" data="{$className}"/>
			</plx:trailingInfo>
			<plx:derivesFromClass dataTypeName="{$className}"/>
			<xsl:call-template name="GenerateImplementationConstructor">
				<xsl:with-param name="properties" select="$properties"/>
				<xsl:with-param name="className" select="$className"/>
				<xsl:with-param name="ModelContextName" select="$ModelContextName"/>
			</xsl:call-template>
			<xsl:variable name="contextPropertyFragment">
				<Property name="Context" readOnly="true">
					<DataType dataTypeName="{$ModelContextName}"/>
				</Property>
			</xsl:variable>
			<xsl:for-each select="msxsl:node-set($contextPropertyFragment)/child::*">
				<xsl:call-template name="GenerateImplementationProperty"/>
			</xsl:for-each>
			<xsl:for-each select="$properties">
				<xsl:call-template name="GenerateImplementationProperty">
					<xsl:with-param name="className" select="$className"/>
				</xsl:call-template>
			</xsl:for-each>
		</plx:class>
	</xsl:template>
	<xsl:template name="GenerateImplementationProperty">
		<xsl:param name="className"/>
		<xsl:param name="initializeField" select="false()"/>
		<xsl:if test="@collection='true'">
			<xsl:call-template name="GenerateCollectionClass">
				<xsl:with-param name="className" select="$className"/>
			</xsl:call-template>
		</xsl:if>
		<plx:field name="{$PrivateMemberPrefix}{@name}" visibility="private">
			<xsl:if test="@readOnly='true' and @customType='true' and not(@collection='true')">
				<xsl:attribute name="readOnly">
					<xsl:text>true</xsl:text>
				</xsl:attribute>
			</xsl:if>
			<xsl:copy-of select="DataType/@*"/>
			<xsl:copy-of select="DataType/child::*"/>
			<xsl:if test="$initializeField">
				<plx:initialize>
					<plx:defaultValueOf>
						<xsl:copy-of select="DataType/@*"/>
						<xsl:copy-of select="DataType/child::*"/>
					</plx:defaultValueOf>
				</plx:initialize>
			</xsl:if>
		</plx:field>
		<!-- Get and Set Properties for the given Object-->
		<plx:property name="{@name}" visibility="public" modifier="override">
			<plx:returns>
				<xsl:copy-of select="DataType/@*"/>
				<xsl:copy-of select="DataType/child::*"/>
			</plx:returns>
			<!-- Get -->
			<plx:get>
				<xsl:if test="@collection='true'">
					<plx:branch>
						<plx:condition>
							<plx:binaryOperator type="identityEquality">
								<plx:left>
									<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}{@name}"/>
								</plx:left>
								<plx:right>
									<plx:nullKeyword/>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:assign>
							<plx:left>
								<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}{@name}"/>
							</plx:left>
							<plx:right>
								<plx:callNew dataTypeName="{@name}Collection">
									<plx:passParam>
										<plx:thisKeyword/>
									</plx:passParam>
								</plx:callNew>
							</plx:right>
						</plx:assign>
					</plx:branch>
				</xsl:if>
				<plx:return>
					<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}{@name}"/>
				</plx:return>
			</plx:get>
			<!-- Set -->
			<xsl:if test="not(@readOnly='true')">
				<plx:set>
					<plx:branch>
						<plx:condition>
							<plx:unaryOperator type="booleanNot">
								<plx:callStatic type="methodCall" name="Equals" dataTypeName=".object">
									<plx:passParam>
										<plx:callThis accessor="this" type="property" name="{@name}"/>
									</plx:passParam>
									<plx:passParam>
										<plx:valueKeyword/>
									</plx:passParam>
								</plx:callStatic>
							</plx:unaryOperator>
						</plx:condition>
						<!-- Notify the ModelContext that we're changing the value of a property. -->
						<plx:branch>
							<plx:condition>
								<plx:callInstance type="methodCall" name="On{$className}{@name}Changing">
									<plx:callObject>
										<plx:callThis accessor="this" type="property" name="Context"/>
									</plx:callObject>
									<plx:passParam>
										<plx:thisKeyword />
									</plx:passParam>
									<plx:passParam>
										<plx:valueKeyword />
									</plx:passParam>
									<plx:passParam>
										<plx:trueKeyword/>
									</plx:passParam>
								</plx:callInstance>
							</plx:condition>
							<plx:branch>
								<plx:condition>
									<plx:callThis accessor="base" type="methodCall" name="Raise{@name}ChangingEvent">
										<plx:passParam>
											<plx:valueKeyword/>
										</plx:passParam>
									</plx:callThis>
								</plx:condition>
								<plx:local name="oldValue">
									<xsl:copy-of select="DataType/@*"/>
									<xsl:copy-of select="DataType/child::*"/>
									<plx:initialize>
										<plx:callThis accessor="this" type="property" name="{@name}"/>
									</plx:initialize>
								</plx:local>
								<plx:assign>
									<plx:left>
										<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}{@name}"/>
									</plx:left>
									<plx:right>
										<plx:valueKeyword/>
									</plx:right>
								</plx:assign>
								<plx:callInstance name="On{$className}{@name}Changed">
									<plx:callObject>
										<plx:callThis accessor="this" type="property" name="Context"/>
									</plx:callObject>
									<plx:passParam>
										<plx:thisKeyword />
									</plx:passParam>
									<plx:passParam>
										<plx:nameRef type="local" name="oldValue"/>
									</plx:passParam>
								</plx:callInstance>
								<plx:callThis accessor="base" type="methodCall" name="Raise{@name}ChangedEvent">
									<plx:passParam>
										<plx:nameRef type="local" name="oldValue"/>
									</plx:passParam>
								</plx:callThis>
							</plx:branch>
						</plx:branch>
					</plx:branch>
				</plx:set>
			</xsl:if>
		</plx:property>
	</xsl:template>
	<xsl:template name="GenerateImplementationPropertyChangeMethods">
		<xsl:param name="className"/>
		<xsl:param name="Model"/>
		<xsl:param name="ModelContextName"/>
		<xsl:if test="@unique='true' and not(@customType='true')">
			<plx:field name="{$PrivateMemberPrefix}{$className}{@name}Dictionary" visibility="private" dataTypeName="Dictionary">
				<plx:passTypeParam>
					<xsl:call-template name="GetUniquePropertyDataType"/>
				</plx:passTypeParam>
				<plx:passTypeParam dataTypeName="{$className}"/>
				<plx:initialize>
					<plx:callNew dataTypeName="Dictionary">
						<plx:passTypeParam>
							<xsl:call-template name="GetUniquePropertyDataType"/>
						</plx:passTypeParam>
						<plx:passTypeParam dataTypeName="{$className}"/>
					</plx:callNew>
				</plx:initialize>
			</plx:field>
		</xsl:if>
		<xsl:choose>
			<xsl:when test="@collection='true'">
				<plx:function visibility="private" name="On{$className}{@name}Adding">
					<plx:param name="instance" dataTypeName="{$className}"/>
					<plx:param name="value">
						<xsl:copy-of select="DataType/plx:passTypeParam/@*"/>
						<xsl:copy-of select="DataType/plx:passTypeParam/child::*"/>
					</plx:param>
					<plx:returns dataTypeName=".boolean"/>
					<plx:return>
						<plx:trueKeyword/>
					</plx:return>
				</plx:function>
				<plx:function visibility="private" name="On{$className}{@name}Added">
					<plx:param name="instance" dataTypeName="{$className}"/>
					<plx:param name="value">
						<xsl:copy-of select="DataType/plx:passTypeParam/@*"/>
						<xsl:copy-of select="DataType/plx:passTypeParam/child::*"/>
					</plx:param>
					<xsl:if test="@unique='true' or @customType='true'">
						<xsl:if test="not(@customType='true')">
							<plx:pragma type="warningDisable" data="0472"/>
						</xsl:if>
						<xsl:call-template name="GetImplementationPropertyChangedMethodUpdateNewOppositeObjectCode">
							<xsl:with-param name="className" select="$className"/>
							<xsl:with-param name="newValue">
								<plx:nameRef type="parameter" name="value"/>
							</xsl:with-param>
						</xsl:call-template>
					</xsl:if>
				</plx:function>
				<plx:function visibility="private" name="On{$className}{@name}Removing">
					<plx:param name="instance" dataTypeName="{$className}"/>
					<plx:param name="value">
						<xsl:copy-of select="DataType/plx:passTypeParam/@*"/>
						<xsl:copy-of select="DataType/plx:passTypeParam/child::*"/>
					</plx:param>
					<plx:returns dataTypeName=".boolean"/>
					<plx:return>
						<plx:trueKeyword/>
					</plx:return>
				</plx:function>
				<plx:function visibility="private" name="On{$className}{@name}Removed">
					<plx:param name="instance" dataTypeName="{$className}"/>
					<plx:param name="value">
						<xsl:copy-of select="DataType/plx:passTypeParam/@*"/>
						<xsl:copy-of select="DataType/plx:passTypeParam/child::*"/>
					</plx:param>
					<xsl:if test="@unique='true' or @customType='true'">
						<xsl:call-template name="GetImplementationPropertyChangedMethodUpdateOldOppositeObjectCode">
							<xsl:with-param name="className" select="$className"/>
							<xsl:with-param name="oldValue">
								<plx:nameRef type="parameter" name="value"/>
							</xsl:with-param>
						</xsl:call-template>
						<xsl:if test="not(@customType='true')">
							<plx:pragma type="warningRestore" data="0472"/>
						</xsl:if>
					</xsl:if>
				</plx:function>
			</xsl:when>
			<xsl:when test="not(@readOnly='true')">
				<xsl:variable name="realRoleRef" select="@realRoleRef"/>
				<!-- this is the constrained role for a role value constraint-->
				<xsl:variable name="roleValueConstraint" select="$Model/orm:Facts/orm:Fact/orm:FactRoles/orm:Role[@id=$realRoleRef]/orm:ValueConstraint"/>
				<xsl:variable name="externalUniquenessConstraints" select="$Model/orm:ExternalConstraints/orm:ExternalUniquenessConstraint[orm:RoleSequence/orm:Role/@ref=$realRoleRef]"/>
				<xsl:variable name="associationUniquenessConstraintsFragment">
					<xsl:for-each select="$AbsorbedObjects/../ao:Association">
						<xsl:copy-of select="$Model/orm:Facts/orm:Fact[@id=current()/@id]/orm:InternalConstraints/orm:InternalUniquenessConstraint[orm:RoleSequence/orm:Role/@ref=$realRoleRef]"/>
					</xsl:for-each>
				</xsl:variable>
				<xsl:variable name="associationUniquenessConstraints" select="msxsl:node-set($associationUniquenessConstraintsFragment)/child::*"/>
				<plx:function visibility="private" name="On{$className}{@name}Changing">
					<xsl:if test="@customType='true'">
						<xsl:call-template name="GenerateSuppressMessageAttribute">
							<xsl:with-param name="category" select="'Microsoft.Usage'"/>
							<xsl:with-param name="checkId" select="'CA2208'"/>
						</xsl:call-template>
						<xsl:call-template name="GenerateSuppressMessageAttribute">
							<xsl:with-param name="category" select="'Microsoft.Globalization'"/>
							<xsl:with-param name="checkId" select="'CA1303'"/>
						</xsl:call-template>
					</xsl:if>
					<plx:param name="instance" dataTypeName="{$className}"/>
					<plx:param name="newValue">
						<xsl:copy-of select="DataType/@*"/>
						<xsl:copy-of select="DataType/child::*"/>
					</plx:param>
					<plx:param name="throwOnFailure" dataTypeName=".boolean"/>
					<plx:returns dataTypeName=".boolean"/>
					<xsl:choose>
						<xsl:when test="$roleValueConstraint">
							<plx:branch>
								<plx:condition>
									<plx:unaryOperator type="booleanNot">
										<plx:callStatic dataTypeName="{$ModelContextName}">
											<xsl:attribute name="name">
												<xsl:call-template name="GetValueConstraintValidationFunctionNameForRole">
													<xsl:with-param name="Role" select="$roleValueConstraint/.."/>
												</xsl:call-template>
											</xsl:attribute>
											<plx:passParam>
												<plx:nameRef type="parameter" name="newValue"/>
											</plx:passParam>
											<plx:passParam>
												<plx:nameRef type="parameter" name="throwOnFailure"/>
											</plx:passParam>
										</plx:callStatic>
									</plx:unaryOperator>
								</plx:condition>
								<plx:return>
									<plx:falseKeyword/>
								</plx:return>
							</plx:branch>
						</xsl:when>
						<xsl:otherwise>
							<xsl:variable name="valueTypeValueConstraint" select="$Model/orm:Objects/orm:ValueType[orm:PlayedRoles/orm:Role/@ref=$realRoleRef]/orm:ValueConstraint"/>
							<xsl:choose>
								<xsl:when test="$valueTypeValueConstraint">
									<xsl:call-template name="GetValueTypeValueConstraintCode">
										<xsl:with-param name="ModelContextName" select="$ModelContextName"/>
										<xsl:with-param name="name" select="@name"/>
									</xsl:call-template>
								</xsl:when>
								<xsl:otherwise>
									<xsl:variable name="absorbedValueTypeValueConstraintName">
										<xsl:call-template name="FindValueConstraintForAbsorbedObjectRecursively">
											<xsl:with-param name="Model" select="$Model"/>
											<xsl:with-param name="entityType" select="$Model/orm:Objects/orm:EntityType[orm:PlayedRoles/orm:Role/@ref=$realRoleRef]"/>
										</xsl:call-template>
									</xsl:variable>
									<xsl:if test="string-length($absorbedValueTypeValueConstraintName)">
										<xsl:call-template name="GetValueTypeValueConstraintCode">
											<xsl:with-param name="ModelContextName" select="$ModelContextName"/>
											<xsl:with-param name="name" select="$absorbedValueTypeValueConstraintName"/>
										</xsl:call-template>
									</xsl:if>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:otherwise>
					</xsl:choose>
					<xsl:if test="@customType='true' or (@unique='true' and not(@customType='true'))">
						<xsl:if test="not(@customType='true')">
							<plx:pragma type="warningDisable" data="0472"/>
						</xsl:if>
						<plx:branch>
							<plx:condition>
								<plx:binaryOperator type="identityInequality">
									<plx:left>
										<plx:nameRef type="parameter" name="newValue"/>
									</plx:left>
									<plx:right>
										<plx:nullKeyword />
									</plx:right>
								</plx:binaryOperator>
							</plx:condition>
							<xsl:choose>
								<xsl:when test="@customType='true'">
									<plx:branch>
										<plx:condition>
											<plx:binaryOperator type="identityInequality">
												<plx:left>
													<plx:thisKeyword/>
												</plx:left>
												<plx:right>
													<plx:callInstance type="property" name="Context">
														<plx:callObject>
															<plx:nameRef type="parameter" name="newValue"/>
														</plx:callObject>
													</plx:callInstance>
												</plx:right>
											</plx:binaryOperator>
										</plx:condition>
										<plx:throw>
											<plx:callNew dataTypeName="ArgumentException">
												<plx:passParam>
													<plx:string>All objects in a relationship must be part of the same Context.</plx:string>
												</plx:passParam>
												<plx:passParam>
													<plx:string>value</plx:string>
												</plx:passParam>
											</plx:callNew>
										</plx:throw>
									</plx:branch>
								</xsl:when>
								<xsl:when test="@unique='true' and not(@customType='true')">
									<plx:local name="currentInstance" dataTypeName="{$className}">
										<plx:initialize>
											<plx:nameRef type="parameter" name="instance"/>
										</plx:initialize>
									</plx:local>
									<plx:branch>
										<plx:condition>
											<plx:callInstance name="TryGetValue">
												<plx:callObject>
													<plx:callThis name="{$PrivateMemberPrefix}{$className}{@name}Dictionary" type="field"/>
												</plx:callObject>
												<plx:passParam>
													<plx:nameRef type="parameter" name="newValue"/>
												</plx:passParam>
												<plx:passParam type="out">
													<plx:nameRef name="currentInstance"/>
												</plx:passParam>
											</plx:callInstance>
										</plx:condition>
										<plx:branch>
											<plx:condition>
												<plx:unaryOperator type="booleanNot">
													<plx:callStatic name="Equals" dataTypeName=".object">
														<plx:passParam>
															<plx:nameRef name="currentInstance"/>
														</plx:passParam>
														<plx:passParam>
															<plx:nameRef type="parameter" name="instance"/>
														</plx:passParam>
													</plx:callStatic>
												</plx:unaryOperator>
											</plx:condition>
											<plx:return>
												<plx:falseKeyword/>
											</plx:return>
										</plx:branch>
									</plx:branch>
								</xsl:when>
								<xsl:otherwise>
									<xsl:message terminate="yes">
										<xsl:text>Sanity Check (this should never be executed)</xsl:text>
									</xsl:message>
								</xsl:otherwise>
							</xsl:choose>
						</plx:branch>
						<xsl:if test="not(@customType='true')">
							<plx:pragma type="warningRestore" data="0472"/>
						</xsl:if>
					</xsl:if>
					<xsl:if test="count($externalUniquenessConstraints)">
						<xsl:for-each select="$externalUniquenessConstraints">
							<xsl:call-template name="GetSimpleBinaryUniquenessPropertyChangingCode">
								<xsl:with-param name="Model" select="$Model"/>
								<xsl:with-param name="realRoleRef" select="$realRoleRef"/>
							</xsl:call-template>
						</xsl:for-each>
					</xsl:if>
					<xsl:if test="count($associationUniquenessConstraints)">
						<xsl:for-each select="$associationUniquenessConstraints">
							<xsl:call-template name="GetSimpleBinaryUniquenessPropertyChangingCode">
								<xsl:with-param name="Model" select="$Model"/>
								<xsl:with-param name="realRoleRef" select="$realRoleRef"/>
							</xsl:call-template>
						</xsl:for-each>
					</xsl:if>
					<plx:return>
						<plx:trueKeyword/>
					</plx:return>
				</plx:function>
				<plx:function visibility="private" overload="true" name="On{$className}{@name}Changed">
					<plx:param name="instance" dataTypeName="{$className}"/>
					<plx:param name="oldValue">
						<xsl:copy-of select="DataType/@*"/>
						<xsl:copy-of select="DataType/child::*"/>
					</plx:param>
					<xsl:if test="@unique='true' or @customType='true'">
						<xsl:call-template name="GetImplementationPropertyChangedMethodUpdateOldOppositeObjectCode">
							<xsl:with-param name="className" select="$className"/>
							<xsl:with-param name="oldValue">
								<plx:nameRef type="parameter" name="oldValue"/>
							</xsl:with-param>
						</xsl:call-template>
						<xsl:call-template name="GetImplementationPropertyChangedMethodUpdateNewOppositeObjectCode">
							<xsl:with-param name="className" select="$className"/>
							<xsl:with-param name="newValue">
								<plx:callInstance name="{@name}" type="property">
									<plx:callObject>
										<plx:nameRef type="parameter" name="instance"/>
									</plx:callObject>
								</plx:callInstance>
							</xsl:with-param>
						</xsl:call-template>
					</xsl:if>
					<xsl:if test="count($externalUniquenessConstraints)">
						<xsl:for-each select="$externalUniquenessConstraints">
							<xsl:call-template name="GetSimpleBinaryUniquenessPropertyChangedCode">
								<xsl:with-param name="Model" select="$Model"/>
								<xsl:with-param name="realRoleRef" select="$realRoleRef"/>
								<xsl:with-param name="haveOldValue" select="true()"/>
							</xsl:call-template>
						</xsl:for-each>
					</xsl:if>
					<xsl:if test="count($associationUniquenessConstraints)">
						<xsl:for-each select="$associationUniquenessConstraints">
							<xsl:call-template name="GetSimpleBinaryUniquenessPropertyChangedCode">
								<xsl:with-param name="Model" select="$Model"/>
								<xsl:with-param name="realRoleRef" select="$realRoleRef"/>
								<xsl:with-param name="haveOldValue" select="true()"/>
							</xsl:call-template>
						</xsl:for-each>
					</xsl:if>
				</plx:function>
				<xsl:if test="@mandatory='true' or @mandatory='relaxed'">
					<plx:function visibility="private" overload="true" name="On{$className}{@name}Changed">
						<plx:param name="instance" dataTypeName="{$className}"/>
						<xsl:if test="@unique='true' or @customType='true'">
							<xsl:if test="not(@customType='true')">
								<plx:pragma type="warningDisable" data="0472"/>
							</xsl:if>
							<xsl:call-template name="GetImplementationPropertyChangedMethodUpdateNewOppositeObjectCode">
								<xsl:with-param name="className" select="$className"/>
								<xsl:with-param name="newValue">
									<plx:callInstance name="{@name}" type="property">
										<plx:callObject>
											<plx:nameRef type="parameter" name="instance"/>
										</plx:callObject>
									</plx:callInstance>
								</xsl:with-param>
							</xsl:call-template>
						</xsl:if>
						<xsl:if test="count($externalUniquenessConstraints)">
							<xsl:for-each select="$externalUniquenessConstraints">
								<xsl:call-template name="GetSimpleBinaryUniquenessPropertyChangedCode">
									<xsl:with-param name="Model" select="$Model"/>
									<xsl:with-param name="realRoleRef" select="$realRoleRef"/>
									<xsl:with-param name="haveOldValue" select="false()"/>
								</xsl:call-template>
							</xsl:for-each>
						</xsl:if>
						<xsl:if test="count($associationUniquenessConstraints)">
							<xsl:for-each select="$associationUniquenessConstraints">
								<xsl:call-template name="GetSimpleBinaryUniquenessPropertyChangedCode">
									<xsl:with-param name="Model" select="$Model"/>
									<xsl:with-param name="realRoleRef" select="$realRoleRef"/>
									<xsl:with-param name="haveOldValue" select="false()"/>
								</xsl:call-template>
							</xsl:for-each>
						</xsl:if>
					</plx:function>
				</xsl:if>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="GenerateImplementationSimpleLookupMethod">
		<xsl:param name="ModelContextName"/>
		<xsl:param name="className"/>
		<xsl:if test="@unique='true' and not(@customType='true')">
			<plx:function name="Get{$className}By{@name}" visibility="{$ModelContextInterfaceImplementationVisibility}">
				<plx:interfaceMember memberName="Get{$className}By{@name}" dataTypeName="I{$ModelContextName}"/>
				<plx:param name="value">
					<xsl:call-template name="GetUniquePropertyDataType"/>
				</plx:param>
				<plx:returns dataTypeName="{$className}"/>
				<plx:return>
					<plx:callInstance type="indexerCall" name=".implied">
						<plx:callObject>
							<plx:callThis type="field" name="{$PrivateMemberPrefix}{$className}{@name}Dictionary"/>
						</plx:callObject>
						<plx:passParam>
							<plx:nameRef type="parameter" name="value"/>
						</plx:passParam>
					</plx:callInstance>
				</plx:return>
			</plx:function>
		</xsl:if>
	</xsl:template>
	<xsl:template name="GetImplementationPropertyChangedMethodUpdateOldOppositeObjectCode">
		<xsl:param name="className"/>
		<xsl:param name="oldValue"/>
		<xsl:if test="not(@customType='true')">
			<plx:pragma type="warningDisable" data="0472"/>
		</xsl:if>
		<plx:branch>
			<plx:condition>
				<plx:binaryOperator type="identityInequality">
					<plx:left>
						<xsl:copy-of select="$oldValue"/>
					</plx:left>
					<plx:right>
						<plx:nullKeyword />
					</plx:right>
				</plx:binaryOperator>
			</plx:condition>
			<xsl:choose>
				<xsl:when test="@unique='true' and @customType='true'">
					<plx:assign>
						<plx:left>
							<plx:callInstance type="property" name="{@oppositeName}">
								<plx:callObject>
									<xsl:copy-of select="$oldValue"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:left>
						<plx:right>
							<plx:nullKeyword/>
						</plx:right>
					</plx:assign>
				</xsl:when>
				<xsl:when test="@unique='true' and not(@customType='true')">
					<plx:callInstance name="Remove">
						<plx:callObject>
							<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}{$className}{@name}Dictionary"/>
						</plx:callObject>
						<plx:passParam>
							<xsl:copy-of select="$oldValue"/>
						</plx:passParam>
					</plx:callInstance>
				</xsl:when>
				<xsl:when test="not(@unique='true') and @customType='true'">
					<plx:callInstance name="Remove">
						<plx:callObject>
							<plx:callInstance type="property" name="{@oppositeName}">
								<plx:callObject>
									<xsl:copy-of select="$oldValue"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:callObject>
						<plx:passParam>
							<plx:nameRef type="parameter" name="instance"/>
						</plx:passParam>
					</plx:callInstance>
				</xsl:when>
			</xsl:choose>
		</plx:branch>
	</xsl:template>
	<xsl:template name="GetImplementationPropertyChangedMethodUpdateNewOppositeObjectCode">
		<xsl:param name="className"/>
		<xsl:param name="newValue"/>
		<plx:branch>
			<plx:condition>
				<plx:binaryOperator type="identityInequality">
					<plx:left>
						<xsl:copy-of select="$newValue"/>
					</plx:left>
					<plx:right>
						<plx:nullKeyword/>
					</plx:right>
				</plx:binaryOperator>
			</plx:condition>
			<xsl:choose>
				<xsl:when test="@unique='true' and @customType='true'">
					<plx:assign>
						<plx:left>
							<plx:callInstance type="property" name="{@oppositeName}">
								<plx:callObject>
									<xsl:copy-of select="$newValue"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:left>
						<plx:right>
							<plx:nameRef type="parameter" name="instance"/>
						</plx:right>
					</plx:assign>
				</xsl:when>
				<xsl:when test="@unique='true' and not(@customType='true')">
					<plx:callInstance name="Add">
						<plx:callObject>
							<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}{$className}{@name}Dictionary"/>
						</plx:callObject>
						<plx:passParam>
							<xsl:copy-of select="$newValue"/>
						</plx:passParam>
						<plx:passParam>
							<plx:nameRef type="parameter" name="instance"/>
						</plx:passParam>
					</plx:callInstance>
				</xsl:when>
				<xsl:when test="not(@unique='true') and @customType='true'">
					<plx:callInstance name="Add">
						<plx:callObject>
							<plx:callInstance type="property" name="{@oppositeName}">
								<plx:callObject>
									<xsl:copy-of select="$newValue"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:callObject>
						<plx:passParam>
							<plx:nameRef type="parameter" name="instance"/>
						</plx:passParam>
					</plx:callInstance>
				</xsl:when>
			</xsl:choose>
		</plx:branch>
		<xsl:if test="not(@customType='true')">
			<plx:pragma type="warningRestore" data="0472"/>
		</xsl:if>
	</xsl:template>

	<xsl:template name="GenerateSimpleBinaryUniquenessChangeMethods">
		<xsl:param name="Model"/>
		<xsl:param name="uniqueObjectName"/>
		<xsl:param name="parameters"/>
		<xsl:variable name="passTypeParams">
			<xsl:for-each select="$parameters">
				<plx:passTypeParam>
					<xsl:copy-of select="DataType/@*"/>
					<xsl:copy-of select="DataType/child::*"/>
				</plx:passTypeParam>
			</xsl:for-each>
		</xsl:variable>
		<plx:field name="{$PrivateMemberPrefix}{@Name}Dictionary" dataTypeName="Dictionary" visibility="private">
			<plx:passTypeParam dataTypeName="Tuple">
				<xsl:copy-of select="$passTypeParams"/>
			</plx:passTypeParam>
			<plx:passTypeParam dataTypeName="{$uniqueObjectName}"/>
			<plx:initialize>
				<plx:callNew dataTypeName="Dictionary">
					<plx:passTypeParam dataTypeName="Tuple">
						<xsl:copy-of select="$passTypeParams"/>
					</plx:passTypeParam>
					<plx:passTypeParam dataTypeName="{$uniqueObjectName}"/>
				</plx:callNew>
			</plx:initialize>
		</plx:field>
		<plx:function visibility="private" name="On{@Name}Changing">
			<plx:param name="instance" dataTypeName="{$uniqueObjectName}"/>
			<plx:param name="newValue" dataTypeName="Tuple">
				<xsl:copy-of select="$passTypeParams"/>
			</plx:param>
			<plx:returns dataTypeName=".boolean"/>
			<plx:branch>
				<plx:condition>
					<plx:binaryOperator type="identityInequality">
						<plx:left>
							<plx:nameRef type="parameter" name="newValue"/>
						</plx:left>
						<plx:right>
							<plx:nullKeyword/>
						</plx:right>
					</plx:binaryOperator>
				</plx:condition>
				<plx:local name="currentInstance" dataTypeName="{$uniqueObjectName}">
					<plx:initialize>
						<plx:nameRef type="parameter" name="instance"/>
					</plx:initialize>
				</plx:local>
				<plx:branch>
					<plx:condition>
						<plx:callInstance name="TryGetValue">
							<plx:callObject>
								<plx:callThis name="{$PrivateMemberPrefix}{@Name}Dictionary" type="field"/>
							</plx:callObject>
							<plx:passParam>
								<plx:nameRef type="parameter" name="newValue"/>
							</plx:passParam>
							<plx:passParam type="out">
								<plx:nameRef name="currentInstance"/>
							</plx:passParam>
						</plx:callInstance>
					</plx:condition>
					<plx:return>
						<plx:binaryOperator type="identityEquality">
							<plx:left>
								<plx:nameRef name="currentInstance"/>
							</plx:left>
							<plx:right>
								<plx:nameRef type="parameter" name="instance"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:return>
				</plx:branch>
			</plx:branch>
			<plx:return>
				<plx:trueKeyword/>
			</plx:return>
		</plx:function>
		<plx:function visibility="private" name="On{@Name}Changed">
			<plx:param name="instance" dataTypeName="{$uniqueObjectName}" />
			<plx:param name="oldValue" dataTypeName="Tuple">
				<xsl:copy-of select="$passTypeParams"/>
			</plx:param>
			<plx:param name="newValue" dataTypeName="Tuple">
				<xsl:copy-of select="$passTypeParams"/>
			</plx:param>
			<plx:branch>
				<plx:condition>
					<plx:binaryOperator type="identityInequality">
						<plx:left>
							<plx:nameRef type="parameter" name="oldValue"/>
						</plx:left>
						<plx:right>
							<plx:nullKeyword/>
						</plx:right>
					</plx:binaryOperator>
				</plx:condition>
				<plx:callInstance name="Remove">
					<plx:callObject>
						<plx:callThis name="{$PrivateMemberPrefix}{@Name}Dictionary" type="field"/>
					</plx:callObject>
					<plx:passParam>
						<plx:nameRef type="parameter" name="oldValue"/>
					</plx:passParam>
				</plx:callInstance>
			</plx:branch>
			<plx:branch>
				<plx:condition>
					<plx:binaryOperator type="identityInequality">
						<plx:left>
							<plx:nameRef type="parameter" name="newValue"/>
						</plx:left>
						<plx:right>
							<plx:nullKeyword/>
						</plx:right>
					</plx:binaryOperator>
				</plx:condition>
				<plx:callInstance name="Add">
					<plx:callObject>
						<plx:callThis name="{$PrivateMemberPrefix}{@Name}Dictionary" type="field"/>
					</plx:callObject>
					<plx:passParam>
						<plx:nameRef type="parameter" name="newValue"/>
					</plx:passParam>
					<plx:passParam>
						<plx:nameRef type="parameter" name="instance"/>
					</plx:passParam>
				</plx:callInstance>
			</plx:branch>
		</plx:function>
	</xsl:template>
	<xsl:template name="GenerateSimpleBinaryUniquenessLookupMethod">
		<xsl:param name="ModelContextName"/>
		<xsl:param name="uniqueObjectName"/>
		<xsl:param name="parameters"/>
		<plx:function visibility="{$ModelContextInterfaceImplementationVisibility}" name="Get{$uniqueObjectName}By{@Name}">
			<plx:interfaceMember memberName="Get{$uniqueObjectName}By{@Name}" dataTypeName="I{$ModelContextName}"/>
			<xsl:for-each select="$parameters">
				<plx:param name="{@name}">
					<xsl:copy-of select="DataType/@*"/>
					<xsl:copy-of select="DataType/child::*"/>
				</plx:param>
			</xsl:for-each>
			<plx:returns dataTypeName="{$uniqueObjectName}"/>
			<plx:return>
				<plx:callInstance type="indexerCall" name=".implied">
					<plx:callObject>
						<plx:callThis type="field" name="{$PrivateMemberPrefix}{@Name}Dictionary"/>
					</plx:callObject>
					<plx:passParam>
						<plx:callStatic name="CreateTuple" dataTypeName="Tuple">
							<xsl:for-each select="$parameters">
								<plx:passParam>
									<plx:nameRef type="parameter" name="{@name}"/>
								</plx:passParam>
							</xsl:for-each>
						</plx:callStatic>
					</plx:passParam>
				</plx:callInstance>
			</plx:return>
		</plx:function>
	</xsl:template>
	<xsl:template name="GetSimpleBinaryUniquenessPropertyChangingCode">
		<xsl:param name="Model"/>
		<xsl:param name="realRoleRef"/>
		<xsl:variable name="parametersFragment">
			<xsl:for-each select="orm:RoleSequence/orm:Role">
				<xsl:call-template name="GetParameterFromRole">
					<xsl:with-param name="Model" select="$Model"/>
					<xsl:with-param name="realRoleRef" select="$realRoleRef"/>
				</xsl:call-template>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="parameters" select="msxsl:node-set($parametersFragment)/child::*"/>
		<xsl:variable name="passMemberTypeParams">
			<xsl:for-each select="$parameters">
				<plx:passMemberTypeParam>
					<xsl:copy-of select="DataType/@*"/>
					<xsl:copy-of select="DataType/child::*"/>
				</plx:passMemberTypeParam>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="passParams">
			<xsl:for-each select="$parameters">
				<plx:passParam>
					<xsl:choose>
						<xsl:when test="@special='true'">
							<plx:nameRef type="parameter" name="newValue" />
						</xsl:when>
						<xsl:otherwise>
							<plx:callInstance name="{@name}" type="property">
								<plx:callObject>
									<plx:nameRef type="parameter" name="instance"/>
								</plx:callObject>
							</plx:callInstance>
						</xsl:otherwise>
					</xsl:choose>
				</plx:passParam>
			</xsl:for-each>
		</xsl:variable>
		<plx:branch>
			<plx:condition>
				<plx:binaryOperator type="identityEquality">
					<plx:left>
						<plx:nameRef type="parameter" name="instance"/>
					</plx:left>
					<plx:right>
						<plx:nullKeyword/>
					</plx:right>
				</plx:binaryOperator>
			</plx:condition>
			<plx:return>
				<!-- UNDONE: We currently aren't validating multi-role constraints prior to object creation. -->
				<plx:trueKeyword/>
			</plx:return>
		</plx:branch>
		<plx:branch>
			<plx:condition>
				<plx:unaryOperator type="booleanNot">
					<plx:callThis name="On{@Name}Changing">
						<plx:passParam>
							<plx:nameRef type="parameter" name="instance"/>
						</plx:passParam>
						<plx:passParam>
							<plx:callStatic dataTypeName="Tuple" name="CreateTuple">
								<xsl:copy-of select="$passMemberTypeParams"/>
								<xsl:copy-of select="$passParams"/>
							</plx:callStatic>
						</plx:passParam>
					</plx:callThis>
				</plx:unaryOperator>
			</plx:condition>
			<plx:return>
				<plx:falseKeyword />
			</plx:return>
		</plx:branch>
	</xsl:template>
	<xsl:template name="GetSimpleBinaryUniquenessPropertyChangedCode">
		<xsl:param name="Model"/>
		<xsl:param name="realRoleRef"/>
		<xsl:param name="haveOldValue"/>
		<xsl:variable name="parametersFragment">
			<xsl:for-each select="orm:RoleSequence/orm:Role">
				<xsl:call-template name="GetParameterFromRole">
					<xsl:with-param name="Model" select="$Model"/>
					<xsl:with-param name="realRoleRef" select="$realRoleRef"/>
				</xsl:call-template>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="parameters" select="msxsl:node-set($parametersFragment)/child::*"/>
		<xsl:variable name="passMemberTypeParams">
			<xsl:for-each select="$parameters">
				<plx:passMemberTypeParam>
					<xsl:copy-of select="DataType/@*"/>
					<xsl:copy-of select="DataType/child::*"/>
				</plx:passMemberTypeParam>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="oldValuePassParams">
			<xsl:for-each select="$parameters">
				<plx:passParam>
					<xsl:choose>
						<xsl:when test="@special='true'">
							<plx:nameRef type="parameter" name="oldValue" />
						</xsl:when>
						<xsl:otherwise>
							<plx:callInstance name="{@name}"  type="property">
								<plx:callObject>
									<plx:nameRef type="parameter" name="instance"/>
								</plx:callObject>
							</plx:callInstance>
						</xsl:otherwise>
					</xsl:choose>
				</plx:passParam>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="newValuePassParams">
			<xsl:for-each select="$parameters">
				<plx:passParam>
					<plx:callInstance name="{@name}"  type="property">
						<plx:callObject>
							<plx:nameRef type="parameter" name="instance"/>
						</plx:callObject>
					</plx:callInstance>
				</plx:passParam>
			</xsl:for-each>
		</xsl:variable>
		<plx:callThis name="On{@Name}Changed">
			<plx:passParam>
				<plx:nameRef type="parameter" name="instance"/>
			</plx:passParam>
			<plx:passParam>
				<xsl:choose>
					<xsl:when test="$haveOldValue">
						<plx:callStatic dataTypeName="Tuple" name="CreateTuple">
							<xsl:copy-of select="$passMemberTypeParams"/>
							<xsl:copy-of select="$oldValuePassParams"/>
						</plx:callStatic>
					</xsl:when>
					<xsl:when test="not($haveOldValue)">
						<plx:nullKeyword/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:message terminate="yes">Sanity check...</xsl:message>
					</xsl:otherwise>
				</xsl:choose>
			</plx:passParam>
			<plx:passParam>
				<plx:callStatic dataTypeName="Tuple" name="CreateTuple">
					<xsl:copy-of select="$passMemberTypeParams"/>
					<xsl:copy-of select="$newValuePassParams"/>
				</plx:callStatic>
			</plx:passParam>
		</plx:callThis>
	</xsl:template>

	<xsl:template name="GenerateCollectionClass">
		<xsl:param name="className"/>
		<xsl:param name="ModelContextName"/>
		<plx:class visibility="private" modifier="sealed" name="{@name}Collection">
			<plx:implementsInterface>
				<xsl:copy-of select="DataType/@*"/>
				<xsl:copy-of select="DataType/child::*"/>
			</plx:implementsInterface>
			
			<plx:field visibility="private" name="{$PrivateMemberPrefix}{$className}" dataTypeName="{$className}"/>
			<plx:field visibility="private" name="{$PrivateMemberPrefix}List" dataTypeName="List">
				<plx:passTypeParam>
					<xsl:copy-of select="DataType/plx:passTypeParam/@*"/>
					<xsl:copy-of select="DataType/plx:passTypeParam/child::*"/>
				</plx:passTypeParam>
				<plx:initialize>
					<plx:callNew dataTypeName="List">
						<plx:passTypeParam>
							<xsl:copy-of select="DataType/plx:passTypeParam/@*"/>
							<xsl:copy-of select="DataType/plx:passTypeParam/child::*"/>
						</plx:passTypeParam>
					</plx:callNew>
				</plx:initialize>
			</plx:field>

			<plx:function visibility="public" name=".construct">
				<plx:param name="instance" dataTypeName="{$className}"/>
				<plx:assign>
					<plx:left>
						<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}{$className}"/>
					</plx:left>
					<plx:right>
						<plx:nameRef type="parameter" name="instance"/>
					</plx:right>
				</plx:assign>
			</plx:function>

			<plx:function visibility="private" name="GetEnumerator">
				<plx:interfaceMember memberName="GetEnumerator" dataTypeName="IEnumerable"/>
				<plx:returns dataTypeName="IEnumerator"/>
				<plx:return>
					<plx:callThis accessor="this" type="methodCall" name="GetEnumerator"/>
				</plx:return>
			</plx:function>
			<plx:function visibility="public" name="GetEnumerator">
				<plx:interfaceMember memberName="GetEnumerator" dataTypeName="IEnumerable">
					<plx:passTypeParam>
						<xsl:copy-of select="DataType/plx:passTypeParam/@*"/>
						<xsl:copy-of select="DataType/plx:passTypeParam/child::*"/>
					</plx:passTypeParam>
				</plx:interfaceMember>
				<plx:returns dataTypeName="IEnumerator">
					<plx:passTypeParam>
						<xsl:copy-of select="DataType/plx:passTypeParam/@*"/>
						<xsl:copy-of select="DataType/plx:passTypeParam/child::*"/>
					</plx:passTypeParam>
				</plx:returns>
				<plx:return>
					<plx:callInstance type="methodCall" name="GetEnumerator">
						<plx:callObject>
							<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}List"/>
						</plx:callObject>
					</plx:callInstance>
				</plx:return>
			</plx:function>

			<plx:function visibility="public" name="Add">
				<plx:interfaceMember memberName="Add">
					<xsl:copy-of select="DataType/@*"/>
					<xsl:copy-of select="DataType/child::*"/>
				</plx:interfaceMember>
				<plx:param name="item">
					<xsl:copy-of select="DataType/plx:passTypeParam/@*"/>
					<xsl:copy-of select="DataType/plx:passTypeParam/child::*"/>
				</plx:param>
				<plx:branch>
					<plx:condition>
						<xsl:call-template name="GetCollectionClassNotfiyContextCode">
							<xsl:with-param name="className" select="$className"/>
							<xsl:with-param name="action" select="'Adding'"/>
						</xsl:call-template>
					</plx:condition>
					<plx:callInstance type="methodCall" name="Add">
						<plx:callObject>
							<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}List"/>
						</plx:callObject>
						<plx:passParam>
							<plx:nameRef type="parameter" name="item"/>
						</plx:passParam>
					</plx:callInstance>
					<xsl:call-template name="GetCollectionClassNotfiyContextCode">
						<xsl:with-param name="className" select="$className"/>
						<xsl:with-param name="action" select="'Added'"/>
					</xsl:call-template>
				</plx:branch>
			</plx:function>

			<plx:function visibility="public" name="Remove">
				<plx:interfaceMember memberName="Remove">
					<xsl:copy-of select="DataType/@*"/>
					<xsl:copy-of select="DataType/child::*"/>
				</plx:interfaceMember>
				<plx:param name="item">
					<xsl:copy-of select="DataType/plx:passTypeParam/@*"/>
					<xsl:copy-of select="DataType/plx:passTypeParam/child::*"/>
				</plx:param>
				<plx:returns dataTypeName=".boolean"/>
				<plx:branch>
					<plx:condition>
						<xsl:call-template name="GetCollectionClassNotfiyContextCode">
							<xsl:with-param name="className" select="$className"/>
							<xsl:with-param name="action" select="'Removing'"/>
						</xsl:call-template>
					</plx:condition>
					<plx:branch>
						<plx:condition>
							<plx:callInstance type="methodCall" name="Remove">
								<plx:callObject>
									<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}List"/>
								</plx:callObject>
								<plx:passParam>
									<plx:nameRef type="parameter" name="item"/>
								</plx:passParam>
							</plx:callInstance>
						</plx:condition>
						<xsl:call-template name="GetCollectionClassNotfiyContextCode">
							<xsl:with-param name="className" select="$className"/>
							<xsl:with-param name="action" select="'Removed'"/>
						</xsl:call-template>
						<plx:return>
							<plx:trueKeyword/>
						</plx:return>
					</plx:branch>
				</plx:branch>
				<plx:return>
					<plx:falseKeyword/>
				</plx:return>
			</plx:function>

			<plx:function visibility="public" name="Clear">
				<plx:interfaceMember memberName="Clear">
					<xsl:copy-of select="DataType/@*"/>
					<xsl:copy-of select="DataType/child::*"/>
				</plx:interfaceMember>
				<plx:loop>
					<plx:initializeLoop>
						<plx:local name="i" dataTypeName=".i4">
							<plx:initialize>
								<plx:value type="i4" data="0"/>
							</plx:initialize>
						</plx:local>
					</plx:initializeLoop>
					<plx:condition>
						<plx:binaryOperator type="lessThan">
							<plx:left>
								<plx:nameRef type="local" name="i"/>
							</plx:left>
							<plx:right>
								<plx:callInstance type="property" name="Count">
									<plx:callObject>
										<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}List"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:beforeLoop>
						<plx:increment>
							<plx:nameRef name="i"/>
						</plx:increment>
					</plx:beforeLoop>
					<plx:callThis accessor="this" type="methodCall" name="Remove">
						<plx:passParam>
							<plx:callInstance type="indexerCall" name=".implied">
								<plx:callObject>
									<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}List"/>
								</plx:callObject>
								<plx:passParam>
									<plx:nameRef name="i"/>
								</plx:passParam>
							</plx:callInstance>
						</plx:passParam>
					</plx:callThis>
				</plx:loop>
			</plx:function>
			
			<plx:function visibility="public" name="Contains">
				<plx:interfaceMember memberName="Contains">
					<xsl:copy-of select="DataType/@*"/>
					<xsl:copy-of select="DataType/child::*"/>
				</plx:interfaceMember>
				<plx:param name="item">
					<xsl:copy-of select="DataType/plx:passTypeParam/@*"/>
					<xsl:copy-of select="DataType/plx:passTypeParam/child::*"/>
				</plx:param>
				<plx:returns dataTypeName=".boolean"/>
				<plx:return>
					<plx:callInstance name="Contains">
						<plx:callObject>
							<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}List"/>
						</plx:callObject>
						<plx:passParam>
							<plx:nameRef type="parameter" name="item"/>
						</plx:passParam>
					</plx:callInstance>
				</plx:return>
			</plx:function>
			<plx:function visibility="public" name="CopyTo">
				<plx:interfaceMember memberName="CopyTo">
					<xsl:copy-of select="DataType/@*"/>
					<xsl:copy-of select="DataType/child::*"/>
				</plx:interfaceMember>
				<plx:param name="array" dataTypeIsSimpleArray="true">
					<xsl:copy-of select="DataType/plx:passTypeParam/@*"/>
					<xsl:copy-of select="DataType/plx:passTypeParam/child::*"/>
				</plx:param>
				<plx:param name="arrayIndex" dataTypeName=".i4"/>
				<plx:callInstance name="CopyTo">
					<plx:callObject>
						<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}List"/>
					</plx:callObject>
					<plx:passParam>
						<plx:nameRef type="parameter" name="array"/>
					</plx:passParam>
					<plx:passParam>
						<plx:nameRef type="parameter" name="arrayIndex"/>
					</plx:passParam>
				</plx:callInstance>
			</plx:function>
			<plx:property visibility="public" name="Count">
				<plx:interfaceMember memberName="Count">
					<xsl:copy-of select="DataType/@*"/>
					<xsl:copy-of select="DataType/child::*"/>
				</plx:interfaceMember>
				<plx:returns dataTypeName=".i4"/>
				<plx:get>
					<plx:return>
						<plx:callInstance type="property" name="Count">
							<plx:callObject>
								<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}List"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:return>
				</plx:get>
			</plx:property>
			<plx:property visibility="public" name="IsReadOnly">
				<plx:interfaceMember memberName="IsReadOnly">
					<xsl:copy-of select="DataType/@*"/>
					<xsl:copy-of select="DataType/child::*"/>
				</plx:interfaceMember>
				<plx:returns dataTypeName=".boolean"/>
				<plx:get>
					<plx:return>
						<plx:falseKeyword/>
					</plx:return>
				</plx:get>
			</plx:property>
			
		</plx:class>
	</xsl:template>
	<xsl:template name="GetCollectionClassNotfiyContextCode">
		<xsl:param name="className"/>
		<xsl:param name="action"/>
		<plx:callInstance type="methodCall" name="On{$className}{@name}{$action}">
			<plx:callObject>
				<plx:callInstance type="property"  name="Context">
					<plx:callObject>
						<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}{$className}"/>
					</plx:callObject>
				</plx:callInstance>
			</plx:callObject>
			<plx:passParam>
				<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}{$className}"/>
			</plx:passParam>
			<plx:passParam>
				<plx:nameRef type="parameter" name="item"/>
			</plx:passParam>
		</plx:callInstance>
	</xsl:template>
	
</xsl:stylesheet>
