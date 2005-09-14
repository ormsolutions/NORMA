<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:plx="http://Schemas.Neumont.edu/CodeGeneration/Plix"
    xmlns:ve="http://Schemas.Neumont.edu/ORM/SDK/Verbalization"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	>
	<xsl:preserve-space elements="ve:Form"/>
	<!-- Pick up param value supplied automatically by plix loader -->
	<xsl:param name="CustomToolNamespace" select="'TestNamespace'"/>

	<!-- Names of the different classes we generate -->
	<xsl:param name="VerbalizationTextSnippetType" select="'VerbalizationTextSnippetType'"/>
	<xsl:param name="VerbalizationSet" select="'VerbalizationSet'"/>
	<xsl:param name="VerbalizationSets" select="'VerbalizationSets'"/>

	<!-- Include templates to generate the shared verbalization classes -->
	<xsl:include href="VerbalizationGenerator.Sets.xslt"/>
	<xsl:template match="ve:Root">
		<plx:Root>
			<plx:Using name="System"/>
			<plx:Using name="System.IO"/>
			<plx:Using name="System.Text"/>
			<plx:Namespace name="{$CustomToolNamespace}">
				<!-- Generate verbalization set classes and default populations -->
				<xsl:call-template name="GenerateVerbalizationSets"/>
				<!-- Generate verbalization implementations for constraints -->
				<xsl:call-template name="GenerateConstraintVerbalization"/>
			</plx:Namespace>
		</plx:Root>
	</xsl:template>
	<xsl:template name="GenerateConstraintVerbalization">
		<xsl:apply-templates select="ve:Constructs/ve:Constraints/ve:Constraint" mode="ConstraintVerbalization"/>
	</xsl:template>
	<xsl:template match="ve:Constraint" mode="ConstraintVerbalization">
		<xsl:variable name="patternGroup" select="@patternGroup"/>
		<xsl:variable name="isInternal" select="$patternGroup='InternalConstraint'"/>
		<xsl:variable name="isSingleColumn" select="$patternGroup='SingleColumnExternalConstraint'"/>
		<plx:Class name="{@type}" visibility="Public" partial="true">
			<plx:ImplementsInterface dataTypeName="IVerbalize"/>
			<plx:Function name="GetVerbalization" visibility="Protected">
				<plx:Param name="" type="RetVal" dataTypeName="Boolean" dataTypeQualifier="System"/>
				<plx:Param name="writer" dataTypeName="TextWriter"/>
				<plx:Param name="beginVerbalization" dataTypeName="NotifyBeginVerbalization"/>
				<plx:Param name="isNegative" dataTypeName="Boolean" dataTypeQualifier="System"/>
				<plx:InterfaceMember member="GetVerbalization" dataTypeName="IVerbalize"/>
				<plx:Variable name="sbTemp" dataTypeName="StringBuilder">
					<plx:Initialize>
						<plx:NullObjectKeyword/>
					</plx:Initialize>
				</plx:Variable>

				<!-- Don't proceed with verbalization if errors are present -->
				<plx:Variable name="errorOwner" dataTypeName="IModelErrorOwner">
					<plx:Initialize>
						<plx:Cast type="TypeCastTest">
							<plx:TargetType dataTypeName="IModelErrorOwner"/>
							<plx:CastExpression>
								<plx:ThisKeyword/>
							</plx:CastExpression>
						</plx:Cast>
					</plx:Initialize>
				</plx:Variable>
				<plx:Condition>
					<plx:Test>
						<plx:Operator type="IdentityInequality">
							<plx:Left>
								<plx:Value type="Local" data="errorOwner"/>
							</plx:Left>
							<plx:Right>
								<plx:NullObjectKeyword/>
							</plx:Right>
						</plx:Operator>
					</plx:Test>
					<plx:Body>
						<plx:Variable name="firstElement" dataTypeName="Boolean" dataTypeQualifier="System">
							<plx:Initialize>
								<plx:TrueKeyword/>
							</plx:Initialize>
						</plx:Variable>
						<plx:Iterator variableName="error" dataTypeName="ModelError">
							<plx:Initialize>
								<plx:CallInstance name="ErrorCollection" type="Property">
									<plx:CallObject>
										<plx:Value type="Local" data="errorOwner"/>
									</plx:CallObject>
								</plx:CallInstance>
							</plx:Initialize>
							<plx:Body>
								<plx:Condition>
									<plx:Test>
										<plx:Value type="Local" data="firstElement"/>
									</plx:Test>
									<plx:Body>
										<plx:Operator type="Assign">
											<plx:Left>
												<plx:Value type="Local" data="firstElement"/>
											</plx:Left>
											<plx:Right>
												<plx:FalseKeyword/>
											</plx:Right>
										</plx:Operator>
										<plx:CallInstance name="" type="DelegateCall">
											<plx:CallObject>
												<plx:Value type="Parameter" data="beginVerbalization"/>
											</plx:CallObject>
											<plx:PassParam>
												<plx:CallStatic name="ErrorReport" dataTypeName="VerbalizationContent" type="Field"/>
											</plx:PassParam>
										</plx:CallInstance>
									</plx:Body>
									<plx:Alternate>
										<plx:CallInstance name="WriteLine">
											<plx:CallObject>
												<plx:Value type="Parameter" data="writer"/>
											</plx:CallObject>
										</plx:CallInstance>
									</plx:Alternate>
								</plx:Condition>
								<plx:CallInstance name="Write">
									<plx:CallObject>
										<plx:Value type="Parameter" data="writer"/>
									</plx:CallObject>
									<plx:PassParam>
										<plx:CallInstance name="Name" type="Property">
											<plx:CallObject>
												<plx:Value type="Local" data="error"/>
											</plx:CallObject>
										</plx:CallInstance>
									</plx:PassParam>
								</plx:CallInstance>
							</plx:Body>
						</plx:Iterator>
						<plx:Condition>
							<plx:Test>
								<plx:Operator type="BooleanNot">
									<plx:Value type="Local" data="firstElement"/>
								</plx:Operator>
							</plx:Test>
							<plx:Body>
								<plx:Return>
									<plx:FalseKeyword/>
								</plx:Return>
							</plx:Body>
						</plx:Condition>
					</plx:Body>
				</plx:Condition>

				<!-- Pick up standard code we'll need for any constraint -->
				<plx:Variable name="snippets" dataTypeName="{$VerbalizationSets}">
					<plx:Initialize>
						<plx:CallStatic dataTypeName="{$VerbalizationSets}" name="Default" type="Property"/>
					</plx:Initialize>
				</plx:Variable>
				<plx:Variable name="isDeontic" dataTypeName="Boolean" dataTypeQualifier="System">
					<plx:Initialize>
						<plx:FalseKeyword/>
					</plx:Initialize>
				</plx:Variable>

				<!-- Pick up standard code we'll need for any constraint -->
				<plx:Variable name="parentFact" dataTypeName="FactType">
					<xsl:if test="$isInternal">
						<plx:Initialize>
							<plx:CallInstance name="FactType" type="Property">
								<plx:CallObject>
									<plx:ThisKeyword/>
								</plx:CallObject>
							</plx:CallInstance>
						</plx:Initialize>
					</xsl:if>
				</plx:Variable>
				<xsl:if test="$isInternal">
					<plx:Variable name="includedRoles" dataTypeName="RoleMoveableCollection">
						<plx:Initialize>
							<plx:CallInstance name="RoleCollection" type="Property">
								<plx:CallObject>
									<plx:ThisKeyword/>
								</plx:CallObject>
							</plx:CallInstance>
						</plx:Initialize>
					</plx:Variable>
				</xsl:if>
				<plx:Variable name="factRoles" dataTypeName="RoleMoveableCollection">
					<xsl:if test="$isInternal">
						<plx:Initialize>
							<plx:CallInstance name="RoleCollection" type="Property">
								<plx:CallObject>
									<plx:Value type="Local" data="parentFact"/>
								</plx:CallObject>
							</plx:CallInstance>
						</plx:Initialize>
					</xsl:if>
				</plx:Variable>
				<plx:Variable name="factArity" dataTypeName="Int32" dataTypeQualifier="System">
					<xsl:if test="$isInternal">
						<plx:Initialize>
							<plx:CallInstance name="Count" type="Property">
								<plx:CallObject>
									<plx:Value type="Local" data="factRoles"/>
								</plx:CallObject>
							</plx:CallInstance>
						</plx:Initialize>
					</xsl:if>
				</plx:Variable>
				<plx:Variable name="allReadingOrders" dataTypeName="ReadingOrderMoveableCollection">
					<xsl:if test="$isInternal">
						<plx:Initialize>
							<plx:CallInstance name="ReadingOrderCollection" type="Property">
								<plx:CallObject>
									<plx:Value type="Local" data="parentFact"/>
								</plx:CallObject>
							</plx:CallInstance>
						</plx:Initialize>
					</xsl:if>
				</plx:Variable>
				<xsl:if test="$isSingleColumn">
					<plx:Variable name="allConstraintRoles" dataTypeName="RoleMoveableCollection">
						<plx:Initialize>
							<plx:CallInstance name="RoleCollection" type="Property">
								<plx:CallObject>
									<plx:ThisKeyword/>
								</plx:CallObject>
							</plx:CallInstance>
						</plx:Initialize>
					</plx:Variable>
					<plx:Variable name="allFacts" dataTypeName="FactTypeMoveableCollection">
						<plx:Initialize>
							<plx:CallInstance name="FactTypeCollection" type="Property">
								<plx:CallObject>
									<plx:ThisKeyword/>
								</plx:CallObject>
							</plx:CallInstance>
						</plx:Initialize>
					</plx:Variable>
					<plx:Variable name="allFactsCount" dataTypeName="Int32" dataTypeQualifier="System">
						<plx:Initialize>
							<plx:CallInstance name="Count" type="Property">
								<plx:CallObject>
									<plx:Value type="Local" data="allFacts"/>
								</plx:CallObject>
							</plx:CallInstance>
						</plx:Initialize>
					</plx:Variable>
				</xsl:if>
				<xsl:choose>
					<xsl:when test="$isInternal">
						<!-- No readings is an error on the parent, so we can get past the error check without them -->
						<plx:Condition>
							<plx:Test>
								<plx:Operator type="Equality">
									<plx:Left>
										<plx:CallInstance name="Count" type="Property">
											<plx:CallObject>
												<plx:Value type="Local" data="allReadingOrders"/>
											</plx:CallObject>
										</plx:CallInstance>
									</plx:Left>
									<plx:Right>
										<plx:Value type="I4" data="0"/>
									</plx:Right>
								</plx:Operator>
							</plx:Test>
							<plx:Body>
								<plx:Return>
									<plx:FalseKeyword/>
								</plx:Return>
							</plx:Body>
						</plx:Condition>
						<plx:Variable name="includedArity" dataTypeName="Int32" dataTypeQualifier="System">
							<plx:Initialize>
								<plx:CallInstance name="Count" type="Property">
									<plx:CallObject>
										<plx:Value type="Local" data="includedRoles"/>
									</plx:CallObject>
								</plx:CallInstance>
							</plx:Initialize>
						</plx:Variable>
					</xsl:when>
					<xsl:when test="$isSingleColumn">
						<plx:Variable name="allBasicRoleReplacements" dataTypeName="String" dataTypeQualifier="System">
							<plx:ArrayDescriptor rank="1">
								<plx:ArrayDescriptor rank="1"/>
							</plx:ArrayDescriptor>
							<plx:Initialize>
								<plx:CallNew dataTypeName="String" dataTypeQualifier="System">
									<plx:ArrayDescriptor rank="1">
										<plx:ArrayDescriptor rank="1"/>
									</plx:ArrayDescriptor>
									<plx:PassParam>
										<plx:Value type="Local" data="allFactsCount"/>
									</plx:PassParam>
								</plx:CallNew>
							</plx:Initialize>
						</plx:Variable>
						<plx:Variable name="minFactArity" dataTypeName="Int32" dataTypeQualifier="System">
							<plx:Initialize>
								<plx:CallStatic name="MaxValue" dataTypeName="Int32" dataTypeQualifier="System" type="Field"/>
							</plx:Initialize>
						</plx:Variable>
						<plx:Variable name="maxFactArity" dataTypeName="Int32" dataTypeQualifier="System">
							<plx:Initialize>
								<plx:CallStatic name="MinValue" dataTypeName="Int32" dataTypeQualifier="System" type="Field"/>
							</plx:Initialize>
						</plx:Variable>
						<plx:Variable name="iFact" dataTypeName="Int32" dataTypeQualifier="System">
							<plx:Initialize>
								<plx:Value type="I4" data="0"/>
							</plx:Initialize>
						</plx:Variable>
						<plx:Loop>
							<plx:LoopTest>
								<plx:Operator type="LessThan">
									<plx:Left>
										<plx:Value type="Local" data="iFact"/>
									</plx:Left>
									<plx:Right>
										<plx:Value type="Local" data="allFactsCount"/>
									</plx:Right>
								</plx:Operator>
							</plx:LoopTest>
							<plx:LoopIncrement>
								<plx:Operator type="Assign">
									<plx:Left>
										<plx:Value type="Local" data="iFact"/>
									</plx:Left>
									<plx:Right>
										<plx:Operator type="Add">
											<plx:Left>
												<plx:Value type="Local" data="iFact"/>
											</plx:Left>
											<plx:Right>
												<plx:Value type="I4" data="1"/>
											</plx:Right>
										</plx:Operator>
									</plx:Right>
								</plx:Operator>
							</plx:LoopIncrement>
							<plx:Body>
								<!-- Return if there are no readings. We need readings for all facts
									 to verbalize the constraint -->
								<plx:Variable name="currentFact" dataTypeName="FactType">
									<plx:Initialize>
										<plx:CallInstance name="" type="ArrayIndexer">
											<plx:CallObject>
												<plx:Value type="Local" data="allFacts"/>
											</plx:CallObject>
											<plx:PassParam>
												<plx:Value type="Local" data="iFact"/>
											</plx:PassParam>
										</plx:CallInstance>
									</plx:Initialize>
								</plx:Variable>
								<plx:Condition>
									<plx:Test>
										<plx:Operator type="Equality">
											<plx:Left>
												<plx:CallInstance name="Count" type="Property">
													<plx:CallObject>
														<plx:CallInstance name="ReadingOrderCollection" type="Property">
															<plx:CallObject>
																<plx:Value type="Local" data="currentFact"/>
															</plx:CallObject>
														</plx:CallInstance>
													</plx:CallObject>
												</plx:CallInstance>
											</plx:Left>
											<plx:Right>
												<plx:Value type="I4" data="0"/>
											</plx:Right>
										</plx:Operator>
									</plx:Test>
									<plx:Body>
										<plx:Return>
											<plx:FalseKeyword/>
										</plx:Return>
									</plx:Body>
								</plx:Condition>
								<!-- Get the roles and role count for the current fact -->
								<plx:Operator type="Assign">
									<plx:Left>
										<plx:Value type="Local" data="factRoles"/>
									</plx:Left>
									<plx:Right>
										<plx:CallInstance name="RoleCollection" type="Property">
											<plx:CallObject>
												<plx:Value type="Local" data="currentFact"/>
											</plx:CallObject>
										</plx:CallInstance>
									</plx:Right>
								</plx:Operator>
								<plx:Operator type="Assign">
									<plx:Left>
										<plx:Value type="Local" data="factArity"/>
									</plx:Left>
									<plx:Right>
										<plx:CallInstance name="Count" type="Property">
											<plx:CallObject>
												<plx:Value type="Local" data="factRoles"/>
											</plx:CallObject>
										</plx:CallInstance>
									</plx:Right>
								</plx:Operator>
								<!-- Track the min and max values for our current fact arity -->
								<plx:Condition>
									<plx:Test>
										<plx:Operator type="LessThan">
											<plx:Left>
												<plx:Value type="Local" data="factArity"/>
											</plx:Left>
											<plx:Right>
												<plx:Value type="Local" data="minFactArity"/>
											</plx:Right>
										</plx:Operator>
									</plx:Test>
									<plx:Body>
										<plx:Operator type="Assign">
											<plx:Left>
												<plx:Value type="Local" data="minFactArity"/>
											</plx:Left>
											<plx:Right>
												<plx:Value type="Local" data="factArity"/>
											</plx:Right>
										</plx:Operator>
									</plx:Body>
								</plx:Condition>
								<plx:Condition>
									<plx:Test>
										<plx:Operator type="GreaterThan">
											<plx:Left>
												<plx:Value type="Local" data="factArity"/>
											</plx:Left>
											<plx:Right>
												<plx:Value type="Local" data="maxFactArity"/>
											</plx:Right>
										</plx:Operator>
									</plx:Test>
									<plx:Body>
										<plx:Operator type="Assign">
											<plx:Left>
												<plx:Value type="Local" data="maxFactArity"/>
											</plx:Left>
											<plx:Right>
												<plx:Value type="Local" data="factArity"/>
											</plx:Right>
										</plx:Operator>
									</plx:Body>
								</plx:Condition>
								<!-- Populate the basic replacements for this fact -->
								<xsl:call-template name="PopulateBasicRoleReplacements"/>
								<plx:Operator type="Assign">
									<plx:Left>
										<plx:CallInstance name="" type="ArrayIndexer">
											<plx:CallObject>
												<plx:Value type="Local" data="allBasicRoleReplacements"/>
											</plx:CallObject>
											<plx:PassParam>
												<plx:Value type="Local" data="iFact"/>
											</plx:PassParam>
										</plx:CallInstance>
									</plx:Left>
									<plx:Right>
										<plx:Value type="Local" data="basicRoleReplacements"/>
									</plx:Right>
								</plx:Operator>
							</plx:Body>
						</plx:Loop>
						<plx:Variable name="constraintRoleArity" dataTypeName="Int32" dataTypeQualifier="System">
							<plx:Initialize>
								<plx:CallInstance name="Count" type="Property">
									<plx:CallObject>
										<plx:Value type="Local" data="allConstraintRoles"/>
									</plx:CallObject>
								</plx:CallInstance>
							</plx:Initialize>
						</plx:Variable>
						<plx:Variable name="allConstraintRoleReadingOrders" dataTypeName="ReadingOrder" dataTypeIsSimpleArray="true">
							<plx:Initialize>
								<plx:CallNew dataTypeName="ReadingOrder" dataTypeIsSimpleArray="true">
									<plx:PassParam>
										<plx:Value type="Local" data="constraintRoleArity"/>
									</plx:PassParam>
								</plx:CallNew>
							</plx:Initialize>
						</plx:Variable>
					</xsl:when>
				</xsl:choose>
				<xsl:if test="$isInternal">
					<xsl:call-template name="PopulateBasicRoleReplacements"/>
				</xsl:if>
				<plx:Variable name="roleReplacements" dataTypeName="String" dataTypeQualifier="System" dataTypeIsSimpleArray="true">
					<plx:Initialize>
						<plx:CallNew dataTypeName="String" dataTypeQualifier="System" dataTypeIsSimpleArray="true">
							<plx:PassParam>
								<plx:Value type="Local" data="factArity">
									<xsl:if test="not($isInternal)">
										<xsl:attribute name="data">
											<xsl:text>maxFactArity</xsl:text>
										</xsl:attribute>
									</xsl:if>
								</plx:Value>
							</plx:PassParam>
						</plx:CallNew>
					</plx:Initialize>
				</plx:Variable>
				<plx:Variable name="readingOrder" dataTypeName="ReadingOrder"/>
				<xsl:for-each select="ve:ConstrainedRoles">
					<xsl:if test="position()=1">
						<xsl:call-template name="ConstraintConditions">
							<xsl:with-param name="PatternGroup" select="$patternGroup"/>
						</xsl:call-template>
					</xsl:if>
				</xsl:for-each>
				<plx:Return>
					<plx:TrueKeyword/>
				</plx:Return>
			</plx:Function>
		</plx:Class>
	</xsl:template>
	<!-- Handle the span constraint condition attribute -->
	<xsl:template match="@span" mode="ConstraintConditionOperator">
		<xsl:choose>
			<xsl:when test=".='all'">
				<plx:Operator type="Equality">
					<plx:Left>
						<plx:Value type="Local" data="factArity"/>
					</plx:Left>
					<plx:Right>
						<plx:Value type="Local" data="includedArity"/>
					</plx:Right>
				</plx:Operator>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="TerminateForInvalidAttribute">
					<xsl:with-param name="MessageText">Unrecognized value for span condition attribute</xsl:with-param>
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- Handle the factArity constraint condition attribute -->
	<xsl:template match="@factArity" mode="ConstraintConditionOperator">
		<plx:Operator type="Equality">
			<plx:Left>
				<plx:Value type="Local" data="factArity"/>
			</plx:Left>
			<plx:Right>
				<plx:Value type="I4" data="{.}"/>
			</plx:Right>
		</plx:Operator>
	</xsl:template>
	<!-- Handle the minFactArity constraint condition attribute -->
	<xsl:template match="@minFactArity" mode="ConstraintConditionOperator">
		<plx:Operator type="GreaterThanOrEqual">
			<plx:Left>
				<plx:Value type="Local" data="minFactArity"/>
			</plx:Left>
			<plx:Right>
				<plx:Value type="I4" data="{.}"/>
			</plx:Right>
		</plx:Operator>
	</xsl:template>
	<!-- Handle the maxFactArity constraint condition attribute -->
	<xsl:template match="@maxFactArity" mode="ConstraintConditionOperator">
		<plx:Operator type="LessThanOrEqual">
			<plx:Left>
				<plx:Value type="Local" data="maxFactArity"/>
			</plx:Left>
			<plx:Right>
				<plx:Value type="I4" data="{.}"/>
			</plx:Right>
		</plx:Operator>
	</xsl:template>
	<!-- Handle the sign constraint condition attributes -->
	<xsl:template match="@sign" mode="ConstraintConditionOperator">
		<xsl:choose>
			<xsl:when test=".='-'">
				<plx:Value type="Local" data="isNegative"/>
			</xsl:when>
			<xsl:when test=".='+'">
				<plx:Operator type="BooleanNot">
					<plx:Value type="Local" data="isNegative"/>
				</plx:Operator>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="TerminateForInvalidAttribute">
					<xsl:with-param name="MessageText">Unrecognized value for sign condition attribute</xsl:with-param>
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- Terminate processing if we see an unrecognized operator -->
	<xsl:template match="@*" mode="ConstraintConditionOperator">
		<xsl:call-template name="TerminateForInvalidAttribute">
			<xsl:with-param name="MessageText">Unrecognized constraint condition attribute</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<!-- Terminate processing for unrecognized attribute or attribute value-->
	<xsl:template name="TerminateForInvalidAttribute">
		<xsl:param name="MessageText"/>
		<xsl:message terminate="yes">
			<xsl:value-of select="$MessageText"/>
			<xsl:text>: </xsl:text>
			<xsl:value-of select="local-name()"/>
			<xsl:text>="</xsl:text>
			<xsl:value-of select="."/>
			<xsl:text>"</xsl:text>
		</xsl:message>
	</xsl:template>
	<!-- Helper template for spitting conditions based on specified conditions. All conditions
		 are combined with an and operator, and are given priority based on the order they
		 appear in the data file. The assumption is made that the unconstrained condition
		 is sorted last. -->
	<xsl:template name="ConstraintConditions">
		<xsl:param name="PatternGroup"/>
		<xsl:param name="fallback" select="false()"/>
		<xsl:variable name="conditionTestFragment">
			<xsl:variable name="conditionOperatorsFragment">
				<xsl:apply-templates select="@*" mode="ConstraintConditionOperator"/>
			</xsl:variable>
			<xsl:for-each select="msxsl:node-set($conditionOperatorsFragment)/child::*">
				<xsl:if test="position()=1">
					<xsl:call-template name="CombineElements">
						<xsl:with-param name="OperatorType" select="'BooleanAnd'"/>
					</xsl:call-template>
				</xsl:if>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="conditionTest" select="msxsl:node-set($conditionTestFragment)/child::*"/>
		<xsl:choose>
			<xsl:when test="$fallback">
				<xsl:choose>
					<xsl:when test="$conditionTest">
						<plx:FallbackCondition>
							<plx:Test>
								<xsl:copy-of select="$conditionTest"/>
							</plx:Test>
							<plx:Body>
								<xsl:call-template name="ConstraintBodyContent">
									<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
								</xsl:call-template>
							</plx:Body>
						</plx:FallbackCondition>
					</xsl:when>
					<xsl:otherwise>
						<plx:Alternate>
							<xsl:call-template name="ConstraintBodyContent">
								<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
							</xsl:call-template>
						</plx:Alternate>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<xsl:otherwise>
				<xsl:choose>
					<xsl:when test="$conditionTest">
						<plx:Condition>
							<plx:Test>
								<xsl:copy-of select="$conditionTest"/>
							</plx:Test>
							<plx:Body>
								<xsl:call-template name="ConstraintBodyContent">
									<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
								</xsl:call-template>
							</plx:Body>
							<xsl:for-each select="following-sibling::*">
								<xsl:call-template name="ConstraintConditions">
									<xsl:with-param name="fallback" select="true()"/>
									<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
								</xsl:call-template>
							</xsl:for-each>
						</plx:Condition>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="ConstraintBodyContent">
							<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
						</xsl:call-template>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- Helper template to combine expressions using the specified OperatorType. An external
		 call should fire this from inside a for each for the first element, it will then
		 recurse to pick up remaining elements -->
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
	<!-- Declare the basicRoleReplacements variable for a single fact and populate the basic
		 replacement fields. The fact's roles will be in the factRoles variable
		 and the fact arity in the factArity variable -->
	<xsl:template name="PopulateBasicRoleReplacements">
		<plx:Variable name="basicRoleReplacements" dataTypeName="String" dataTypeQualifier="System" dataTypeIsSimpleArray="true">
			<plx:Initialize>
				<plx:CallNew dataTypeName="String" dataTypeQualifier="System" dataTypeIsSimpleArray="true">
					<plx:PassParam>
						<plx:Value type="Local" data="factArity"/>
					</plx:PassParam>
				</plx:CallNew>
			</plx:Initialize>
		</plx:Variable>
		<plx:Variable name="i" dataTypeName="Int32" dataTypeQualifier="System">
			<plx:Initialize>
				<plx:Value type="I4" data="0"/>
			</plx:Initialize>
		</plx:Variable>
		<plx:Loop>
			<plx:LoopTest>
				<plx:Operator type="LessThan">
					<plx:Left>
						<plx:Value type="Local" data="i"/>
					</plx:Left>
					<plx:Right>
						<plx:Value type="Local" data="factArity"/>
					</plx:Right>
				</plx:Operator>
			</plx:LoopTest>
			<plx:LoopIncrement>
				<plx:Operator type="Assign">
					<plx:Left>
						<plx:Value type="Local" data="i"/>
					</plx:Left>
					<plx:Right>
						<plx:Operator type="Add">
							<plx:Left>
								<plx:Value type="Local" data="i"/>
							</plx:Left>
							<plx:Right>
								<plx:Value type="I4" data="1"/>
							</plx:Right>
						</plx:Operator>
					</plx:Right>
				</plx:Operator>
			</plx:LoopIncrement>
			<plx:Body>
				<plx:Variable name="rolePlayer" dataTypeName="ObjectType">
					<plx:Initialize>
						<plx:CallInstance name="RolePlayer" type="Property">
							<plx:CallObject>
								<plx:CallInstance name="" type="Indexer">
									<plx:CallObject>
										<plx:Value type="Local" data="factRoles"/>
									</plx:CallObject>
									<plx:PassParam>
										<plx:Value type="Local" data="i"/>
									</plx:PassParam>
								</plx:CallInstance>
							</plx:CallObject>
						</plx:CallInstance>
					</plx:Initialize>
				</plx:Variable>
				<plx:Variable name="basicReplacement" dataTypeName="String" dataTypeQualifier="System"/>
				<!--UNDONE: Ring situations-->
				<!--UNDONE: Localize or pull the role name from the snippet set-->
				<plx:Condition>
					<plx:Test>
						<plx:Operator type="IdentityInequality">
							<plx:Left>
								<plx:Value type="Local" data="rolePlayer"/>
							</plx:Left>
							<plx:Right>
								<plx:NullObjectKeyword/>
							</plx:Right>
						</plx:Operator>
					</plx:Test>
					<plx:Body>
						<plx:Operator type="Assign">
							<plx:Left>
								<plx:Value type="Local" data="basicReplacement"/>
							</plx:Left>
							<plx:Right>
								<plx:CallInstance name="Name" type="Property">
									<plx:CallObject>
										<plx:Value type="Local" data="rolePlayer"/>
									</plx:CallObject>
								</plx:CallInstance>
							</plx:Right>
						</plx:Operator>
					</plx:Body>
					<plx:Alternate>
						<plx:Operator type="Assign">
							<plx:Left>
								<plx:Value type="Local" data="basicReplacement"/>
							</plx:Left>
							<plx:Right>
								<plx:Operator type="Add">
									<plx:Left>
										<plx:String>Role</plx:String>
									</plx:Left>
									<plx:Right>
										<plx:CallInstance name="ToString">
											<plx:CallObject>
												<plx:Expression parens="true">
													<plx:Operator type="Add">
														<plx:Left>
															<plx:Value type="Local" data="i"/>
														</plx:Left>
														<plx:Right>
															<plx:Value type="I4" data="1"/>
														</plx:Right>
													</plx:Operator>
												</plx:Expression>
											</plx:CallObject>
											<plx:PassParam>
												<plx:CallInstance name="FormatProvider" type="Property">
													<plx:CallObject>
														<plx:Value type="Parameter" data="writer"/>
													</plx:CallObject>
												</plx:CallInstance>
											</plx:PassParam>
										</plx:CallInstance>
									</plx:Right>
								</plx:Operator>
							</plx:Right>
						</plx:Operator>
					</plx:Alternate>
				</plx:Condition>
				<plx:Operator type="Assign">
					<plx:Left>
						<plx:CallInstance name="" type="ArrayIndexer">
							<plx:CallObject>
								<plx:Value type="Local" data="basicRoleReplacements"/>
							</plx:CallObject>
							<plx:PassParam>
								<plx:Value type="Local" data="i"/>
							</plx:PassParam>
						</plx:CallInstance>
					</plx:Left>
					<plx:Right>
						<plx:Value type="Local" data="basicReplacement"/>
					</plx:Right>
				</plx:Operator>
			</plx:Body>
		</plx:Loop>
	</xsl:template>
	<xsl:template name="ConstraintBodyContent">
		<xsl:param name="PatternGroup"/>
		<!-- At this point we'll either have ConditionalReading or Quantifier children -->
		<xsl:apply-templates select="child::*" mode="ConstraintVerbalization">
			<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
			<xsl:with-param name="TopLevel" select="true()"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="ve:ConditionalReading" mode="ConstraintVerbalization">
		<xsl:param name="TopLevel" select="false()"/>
		<xsl:param name="IteratorContext" select="'all'"/>
		<xsl:param name="PatternGroup"/>
		<xsl:for-each select="ve:ReadingChoice">
			<xsl:if test="position()=1">
				<xsl:call-template name="ProcessConditionalReadingChoice">
					<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
					<xsl:with-param name="TopLevel" select="$TopLevel"/>
					<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
				</xsl:call-template>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>
	<xsl:template name="ProcessConditionalReadingChoice">
		<xsl:param name="IteratorContext" select="'all'"/>
		<xsl:param name="TopLevel" select="false()"/>
		<xsl:param name="Match" select="@match"/>
		<xsl:param name="PatternGroup"/>
		<xsl:choose>
			<xsl:when test="contains($Match,'All')">
				<xsl:variable name="singleMatch" select="concat(substring-before($Match,'All'), substring-after($Match,'All'))"/>
				<plx:Variable name="missingReading" dataTypeName="Boolean" dataTypeQualifier="System">
					<plx:Initialize>
						<plx:FalseKeyword/>
					</plx:Initialize>
				</plx:Variable>
				<plx:Variable name="readingMatchIndex" dataTypeName="Int32" dataTypeQualifier="System">
					<plx:Initialize>
						<plx:Value type="I4" data="0"/>
					</plx:Initialize>
				</plx:Variable>
				<plx:Loop>
					<plx:LoopTest apply="Before">
						<plx:Operator type="BooleanAnd">
							<plx:Left>
								<plx:Operator type="BooleanNot">
									<plx:Value type="Local" data="missingReading"/>
								</plx:Operator>
							</plx:Left>
							<plx:Right>
								<plx:Operator type="LessThan">
									<plx:Left>
										<plx:Value type="Local" data="readingMatchIndex"/>
									</plx:Left>
									<plx:Right>
										<plx:Value type="Local" data="constraintRoleArity"/>
									</plx:Right>
								</plx:Operator>
							</plx:Right>
						</plx:Operator>
					</plx:LoopTest>
					<plx:LoopIncrement>
						<plx:Operator type="Assign">
							<plx:Left>
								<plx:Value type="Local" data="readingMatchIndex"/>
							</plx:Left>
							<plx:Right>
								<plx:Operator type="Add">
									<plx:Left>
										<plx:Value type="Local" data="readingMatchIndex"/>
									</plx:Left>
									<plx:Right>
										<plx:Value type="I4" data="1"/>
									</plx:Right>
								</plx:Operator>
							</plx:Right>
						</plx:Operator>
					</plx:LoopIncrement>
					<plx:Body>
						<plx:Variable name="primaryRole" dataTypeName="Role">
							<plx:Initialize>
								<plx:CallInstance name="" type="Indexer">
									<plx:CallObject>
										<plx:Value type="Local" data="allConstraintRoles"/>
									</plx:CallObject>
									<plx:PassParam>
										<plx:Value type="Local" data="readingMatchIndex"/>
									</plx:PassParam>
								</plx:CallInstance>
							</plx:Initialize>
						</plx:Variable>
						<plx:Operator type="Assign">
							<plx:Left>
								<plx:Value type="Local" data="parentFact"/>
							</plx:Left>
							<plx:Right>
								<plx:CallInstance name="FactType" type="Property">
									<plx:CallObject>
										<plx:Value type="Local" data="primaryRole"/>
									</plx:CallObject>
								</plx:CallInstance>
							</plx:Right>
						</plx:Operator>
						<plx:Operator type="Assign">
							<plx:Left>
								<plx:Value type="Local" data="factRoles"/>
							</plx:Left>
							<plx:Right>
								<plx:CallInstance name="RoleCollection" type="Property">
									<plx:CallObject>
										<plx:Value type="Local" data="parentFact"/>
									</plx:CallObject>
								</plx:CallInstance>
							</plx:Right>
						</plx:Operator>
						<plx:Operator type="Assign">
							<plx:Left>
								<plx:Value type="Local" data="allReadingOrders"/>
							</plx:Left>
							<plx:Right>
								<plx:CallInstance name="ReadingOrderCollection" type="Property">
									<plx:CallObject>
										<plx:Value type="Local" data="parentFact"/>
									</plx:CallObject>
								</plx:CallInstance>
							</plx:Right>
						</plx:Operator>
						<xsl:call-template name="PopulateReadingOrder">
							<xsl:with-param name="ReadingChoice" select="$singleMatch"/>
							<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
						</xsl:call-template>
						<plx:Condition>
							<plx:Test>
								<plx:Operator type="IdentityEquality">
									<plx:Left>
										<plx:Value type="Local" data="readingOrder"/>
									</plx:Left>
									<plx:Right>
										<plx:NullObjectKeyword/>
									</plx:Right>
								</plx:Operator>
							</plx:Test>
							<plx:Body>
								<plx:Operator type="Assign">
									<plx:Left>
										<plx:Value type="Local" data="missingReading"/>
									</plx:Left>
									<plx:Right>
										<plx:TrueKeyword/>
									</plx:Right>
								</plx:Operator>
							</plx:Body>
							<plx:Alternate>
								<plx:Operator type="Assign">
									<plx:Left>
										<plx:CallInstance name="" type="ArrayIndexer">
											<plx:CallObject>
												<plx:Value type="Local" data="allConstraintRoleReadingOrders"/>
											</plx:CallObject>
											<plx:PassParam>
												<plx:Value type="Local" data="readingMatchIndex"/>
											</plx:PassParam>
										</plx:CallInstance>
									</plx:Left>
									<plx:Right>
										<plx:Value type="Local" data="readingOrder"/>
									</plx:Right>
								</plx:Operator> 
							</plx:Alternate>
						</plx:Condition>
					</plx:Body>
				</plx:Loop>
				<plx:Condition>
					<plx:Test>
						<plx:Operator type="BooleanNot">
							<plx:Value type="Local" data="missingReading"/>
						</plx:Operator>
					</plx:Test>
					<!-- The rest of this block is duplicated in otherwise condition, keep in sync -->
					<plx:Body>
						<xsl:apply-templates select="child::*" mode="ConstraintVerbalization">
							<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
							<xsl:with-param name="TopLevel" select="$TopLevel"/>
							<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
						</xsl:apply-templates>
					</plx:Body>
					<xsl:if test="position()!=last()">
						<plx:Alternate>
							<xsl:for-each select="following-sibling::*">
								<xsl:if test="position()=1">
									<xsl:call-template name="ProcessConditionalReadingChoice">
										<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
										<xsl:with-param name="TopLevel" select="$TopLevel"/>
										<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
									</xsl:call-template>
								</xsl:if>
							</xsl:for-each>
						</plx:Alternate>
					</xsl:if>
				</plx:Condition>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="PopulateReadingOrder">
					<xsl:with-param name="ReadingChoice" select="$Match"/>
					<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
				</xsl:call-template>
				<plx:Condition>
					<plx:Test>
						<plx:Operator type="IdentityInequality">
							<plx:Left>
								<plx:Value type="Local" data="readingOrder"/>
							</plx:Left>
							<plx:Right>
								<plx:NullObjectKeyword/>
							</plx:Right>
						</plx:Operator>
					</plx:Test>
					<!-- The rest of this block is duplicated in when condition, keep in sync -->
					<plx:Body>
						<xsl:apply-templates select="child::*" mode="ConstraintVerbalization">
							<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
							<xsl:with-param name="TopLevel" select="$TopLevel"/>
							<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
						</xsl:apply-templates>
					</plx:Body>
					<xsl:if test="position()!=last()">
						<plx:Alternate>
							<xsl:for-each select="following-sibling::*">
								<xsl:if test="position()=1">
									<xsl:call-template name="ProcessConditionalReadingChoice">
										<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
										<xsl:with-param name="TopLevel" select="$TopLevel"/>
										<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
									</xsl:call-template>
								</xsl:if>
							</xsl:for-each>
						</plx:Alternate>
					</xsl:if>
				</plx:Condition>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="ve:Quantifier" mode="ConstraintVerbalization">
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="'snippet'"/>
		<xsl:param name="TopLevel" select="false()"/>
		<xsl:param name="IteratorContext" select="'all'"/>
		<xsl:param name="PatternGroup"/>
		<xsl:if test="$TopLevel">
			<xsl:choose>
				<xsl:when test="position()&gt;1">
					<plx:CallInstance name="WriteLine">
						<plx:CallObject>
							<plx:Value type="Parameter" data="writer"/>
						</plx:CallObject>
					</plx:CallInstance>
				</xsl:when>
				<xsl:otherwise>
					<plx:CallInstance name="" type="DelegateCall">
						<plx:CallObject>
							<plx:Value type="Parameter" data="beginVerbalization"/>
						</plx:CallObject>
						<plx:PassParam>
							<plx:CallStatic name="Normal" dataTypeName="VerbalizationContent" type="Field"/>
						</plx:PassParam>
					</plx:CallInstance>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<plx:Variable name="{$VariablePrefix}{$VariableDecorator}" dataTypeName="String" dataTypeQualifier="System">
			<plx:Initialize>
				<xsl:call-template name="SnippetFor">
					<xsl:with-param name="SnippetType" select="@type"/>
				</xsl:call-template>
			</plx:Initialize>
		</plx:Variable>
		<xsl:for-each select="child::*">
			<plx:Variable name="{$VariablePrefix}{$VariableDecorator}replace{position()}" dataTypeName="String" dataTypeQualifier="System">
				<plx:Initialize>
					<plx:NullObjectKeyword/>
				</plx:Initialize>
			</plx:Variable>
			<xsl:apply-templates select="."  mode="ConstraintVerbalization">
				<xsl:with-param name="VariablePrefix" select="concat($VariablePrefix,$VariableDecorator,'replace')"/>
				<!-- The position will jump back to 1 with this call, so pick up the real position before jumping -->
				<xsl:with-param name="VariableDecorator" select="position()"/>
				<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
				<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
			</xsl:apply-templates>
		</xsl:for-each>
		<plx:CallInstance name="Write">
			<plx:CallObject>
				<plx:Value type="Parameter" data="writer"/>
			</plx:CallObject>
			<plx:PassParam>
				<plx:Value type="Local" data="{$VariablePrefix}{$VariableDecorator}"/>
			</plx:PassParam>
			<xsl:for-each select="child::*">
				<plx:PassParam>
					<plx:Value type="Local" data="{$VariablePrefix}{$VariableDecorator}replace{position()}"/>
				</plx:PassParam>
			</xsl:for-each>
		</plx:CallInstance>
	</xsl:template>
	<xsl:template match="ve:Fact" mode="ConstraintVerbalization">
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="'factText'"/>
		<xsl:param name="PatternGroup"/>
		<xsl:param name="FirstPassVariable"/>
		<!-- all, included or excluded are the supported IteratorContext -->
		<xsl:param name="IteratorContext" select="'all'"/>
		<xsl:param name="TopLevel" select="false()"/>
		<xsl:if test="$TopLevel">
			<xsl:choose>
				<xsl:when test="position()&gt;1">
					<plx:CallInstance name="WriteLine">
						<plx:CallObject>
							<plx:Value type="Parameter" data="writer"/>
						</plx:CallObject>
					</plx:CallInstance>
				</xsl:when>
				<xsl:otherwise>
					<plx:CallInstance name="" type="DelegateCall">
						<plx:CallObject>
							<plx:Value type="Parameter" data="beginVerbalization"/>
						</plx:CallObject>
						<plx:PassParam>
							<plx:CallStatic name="Normal" dataTypeName="VerbalizationContent" type="Field"/>
						</plx:PassParam>
					</plx:CallInstance>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:variable name="complexReplacement" select="0!=count(ve:PredicateReplacement)"/>
		<xsl:call-template name="PopulateReadingOrder">
			<xsl:with-param name="ReadingChoice" select="@readingChoice"/>
			<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
		</xsl:call-template>
		<xsl:choose>
			<xsl:when test="$complexReplacement">
				<xsl:variable name="iterVarName" select="concat($VariablePrefix,'factRoleIter',$VariableDecorator)"/>
				<plx:Variable name="{$iterVarName}" dataTypeName="Int32" dataTypeQualifier="System">
					<plx:Initialize>
						<plx:Value type="I4" data="0"/>
					</plx:Initialize>
				</plx:Variable>
				<plx:Loop>
					<plx:LoopTest apply="Before">
						<plx:Operator type="LessThan">
							<plx:Left>
								<plx:Value type="Local" data="{$iterVarName}"/>
							</plx:Left>
							<plx:Right>
								<plx:Value type="Local" data="factArity"/>
							</plx:Right>
						</plx:Operator>
					</plx:LoopTest>
					<plx:LoopIncrement>
						<plx:Operator type="Assign">
							<plx:Left>
								<plx:Value type="Local" data="{$iterVarName}"/>
							</plx:Left>
							<plx:Right>
								<plx:Operator type="Add">
									<plx:Left>
										<plx:Value type="Local" data="{$iterVarName}"/>
									</plx:Left>
									<plx:Right>
										<plx:Value type="I4" data="1"/>
									</plx:Right>
								</plx:Operator>
							</plx:Right>
						</plx:Operator>
					</plx:LoopIncrement>
					<plx:Body>
						<!-- Initialize variables used for all styles of predicate replacement -->
						<plx:Variable name="currentRole" dataTypeName="Role">
							<plx:Initialize>
								<plx:CallInstance name="" type="ArrayIndexer">
									<plx:CallObject>
										<plx:Value type="Local" data="factRoles"/>
									</plx:CallObject>
									<plx:PassParam>
										<plx:Value type="Local" data="{$iterVarName}"/>
									</plx:PassParam>
								</plx:CallInstance>
							</plx:Initialize>
						</plx:Variable>
						<plx:Variable name="roleReplacement" dataTypeName="String" dataTypeQualifier="System">
							<plx:Initialize>
								<plx:NullObjectKeyword/>
							</plx:Initialize>
						</plx:Variable>
						<plx:Variable name="basicReplacement" dataTypeName="String" dataTypeQualifier="System">
							<plx:Initialize>
								<plx:CallInstance name="" type="ArrayIndexer">
									<plx:CallObject>
										<plx:Value type="Local" data="basicRoleReplacements"/>
									</plx:CallObject>
									<plx:PassParam>
										<plx:Value type="Local" data="{$iterVarName}"/>
									</plx:PassParam>
								</plx:CallInstance>
							</plx:Initialize>
						</plx:Variable>

						<!-- Do specialized replacement for different role matches -->
						<xsl:for-each select="ve:PredicateReplacement">
							<!-- The assumption is made here that predicate replacement quantifiers
								 are single-valued. -->
							<xsl:if test="position()=1">
								<xsl:choose>
									<xsl:when test="(position()!=last()) or (string-length(@match) and not(@match='all'))">
										<plx:Condition>
											<plx:Test>
												<xsl:call-template name="PredicateReplacementConditionTest">
													<xsl:with-param name="Match" select="@match"/>
													<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
													<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
													<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
													<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
													<xsl:with-param name="FirstPassVariable" select="$FirstPassVariable"/>
												</xsl:call-template>
											</plx:Test>
											<plx:Body>
												<xsl:call-template name="PredicateReplacementBody"/>
											</plx:Body>
											<xsl:for-each select="following-sibling::*">
												<xsl:choose>
													<xsl:when test="(position()!=last()) or (string-length(@match) and not(@match='all'))">
														<plx:FallbackCondition>
															<plx:Test>
																<xsl:call-template name="PredicateReplacementConditionTest">
																	<xsl:with-param name="Match" select="@match"/>
																	<xsl:with-param name="IteratorContext" select="$IteratorContext"/>
																	<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
																	<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
																	<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
																	<xsl:with-param name="FirstPassVariable" select="$FirstPassVariable"/>
																</xsl:call-template>
															</plx:Test>
															<plx:Body>
																<xsl:call-template name="PredicateReplacementBody"/>
															</plx:Body>
														</plx:FallbackCondition>
													</xsl:when>
													<xsl:otherwise>
														<plx:Alternate>
															<xsl:call-template name="PredicateReplacementBody"/>
														</plx:Alternate>
													</xsl:otherwise>
												</xsl:choose>
											</xsl:for-each>
										</plx:Condition>
									</xsl:when>
									<xsl:otherwise>
										<xsl:call-template name="PredicateReplacementBody"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:if>
						</xsl:for-each>

						<!-- Use the default replacement for the predicate text if nothing was specified -->
						<plx:Condition>
							<plx:Test>
								<plx:Operator type="IdentityEquality">
									<plx:Left>
										<plx:Value type="Local" data="roleReplacement"/>
									</plx:Left>
									<plx:Right>
										<plx:NullObjectKeyword/>
									</plx:Right>
								</plx:Operator>
							</plx:Test>
							<plx:Body>
								<plx:Operator type="Assign">
									<plx:Left>
										<plx:Value type="Local" data="roleReplacement"/>
									</plx:Left>
									<plx:Right>
										<plx:Value type="Local" data="basicReplacement"/>
									</plx:Right>
								</plx:Operator>
							</plx:Body>
						</plx:Condition>
						<plx:Operator type="Assign">
							<plx:Left>
								<plx:CallInstance name="" type="ArrayIndexer">
									<plx:CallObject>
										<plx:Value type="Local" data="roleReplacements"/>
									</plx:CallObject>
									<plx:PassParam>
										<plx:Value type="Local" data="{$iterVarName}"/>
									</plx:PassParam>
								</plx:CallInstance>
							</plx:Left>
							<plx:Right>
								<plx:Value type="Local" data="roleReplacement"/>
							</plx:Right>
						</plx:Operator>
					</plx:Body>
				</plx:Loop>
			</xsl:when>
		</xsl:choose>
		<xsl:variable name="predicateText">
			<plx:CallStatic name="PopulatePredicateText" dataTypeName="FactType">
				<plx:PassParam>
					<plx:Value type="Local" data="readingOrder"/>
				</plx:PassParam>
				<plx:PassParam>
					<plx:Value type="Local" data="factRoles"/>
				</plx:PassParam>
				<plx:PassParam>
					<xsl:variable name="replacementSet">
						<xsl:choose>
							<xsl:when test="$complexReplacement">
								<xsl:text>roleReplacements</xsl:text>
							</xsl:when>
							<xsl:otherwise>
								<xsl:text>basicRoleReplacements</xsl:text>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<plx:Value type="Local" data="{$replacementSet}"/>
				</plx:PassParam>
			</plx:CallStatic>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="$TopLevel">
				<plx:CallInstance name="Write">
					<plx:CallObject>
						<plx:Value type="Parameter" data="writer"/>
					</plx:CallObject>
					<plx:PassParam>
						<xsl:copy-of select="$predicateText"/>
					</plx:PassParam>
				</plx:CallInstance>
			</xsl:when>
			<xsl:otherwise>
				<plx:Operator type="Assign">
					<plx:Left>
						<plx:Value type="Local" data="{$VariablePrefix}{$VariableDecorator}"/>
					</plx:Left>
					<plx:Right>
						<xsl:copy-of select="$predicateText"/>
					</plx:Right>
				</plx:Operator>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="PredicateReplacementConditionTest">
		<xsl:param name="Match"/>
		<xsl:param name="IteratorContext"/>
		<xsl:param name="PatternGroup"/>
		<xsl:param name="VariablePrefix"/>
		<xsl:param name="VariableDecorator"/>
		<xsl:param name="FirstPassVariable"/>
		<xsl:variable name="operatorsFragment">
			<xsl:choose>
				<xsl:when test="$Match='primary'">
					<plx:Operator type="IdentityEquality">
						<plx:Left>
							<plx:Value type="Local" data="primaryRole"/>
						</plx:Left>
						<plx:Right>
							<plx:Value type="Local" data="currentRole"/>
						</plx:Right>
					</plx:Operator>
				</xsl:when>
				<xsl:when test="$Match='secondary'">
					<xsl:choose>
						<xsl:when test="$IteratorContext='included'">
							<plx:Operator type="IdentityInequality">
								<plx:Left>
									<plx:Value type="Local" data="primaryRole"/>
								</plx:Left>
								<plx:Right>
									<plx:Value type="Local" data="currentRole"/>
								</plx:Right>
							</plx:Operator>
							<plx:Operator type="BooleanNot">
								<plx:CallInstance name="Contains">
									<plx:CallObject>
										<plx:Value type="Local" data="includedRoles"/>
									</plx:CallObject>
									<plx:PassParam>
										<plx:Value type="Local" data="currentRole"/>
									</plx:PassParam>
								</plx:CallInstance>
							</plx:Operator>
						</xsl:when>
						<xsl:when test="$IteratorContext='excluded'">
							<plx:Operator type="IdentityInequality">
								<plx:Left>
									<plx:Value type="Local" data="primaryRole"/>
								</plx:Left>
								<plx:Right>
									<plx:Value type="Local" data="currentRole"/>
								</plx:Right>
							</plx:Operator>
							<plx:CallInstance name="Contains">
								<plx:CallObject>
									<plx:Value type="Local" data="includedRoles"/>
								</plx:CallObject>
								<plx:PassParam>
									<plx:Value type="Local" data="currentRole"/>
								</plx:PassParam>
							</plx:CallInstance>
						</xsl:when>
						<xsl:otherwise>
							<plx:Operator type="IdentityInequality">
								<plx:Left>
									<plx:Value type="Local" data="primaryRole"/>
								</plx:Left>
								<plx:Right>
									<plx:Value type="Local" data="currentRole"/>
								</plx:Right>
							</plx:Operator>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:when test="$Match='included'">
					<xsl:choose>
						<xsl:when test="$IteratorContext='singleColumnConstraintRoles'">
							<!-- For the single column case, the included role is always a set consisting of the primary role only -->
							<plx:Operator type="IdentityEquality">
								<plx:Left>
									<plx:Value type="Local" data="currentRole"/>
								</plx:Left>
								<plx:Right>
									<plx:Value type="Local" data="primaryRole"/>
								</plx:Right>
							</plx:Operator>
						</xsl:when>
						<xsl:otherwise>
							<plx:CallInstance name="Contains">
								<plx:CallObject>
									<plx:Value type="Local" data="includedRoles"/>
								</plx:CallObject>
								<plx:PassParam>
									<plx:Value type="Local" data="currentRole"/>
								</plx:PassParam>
							</plx:CallInstance>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:when test="$Match='excluded'">
					<xsl:choose>
						<xsl:when test="$IteratorContext='singleColumnConstraintRoles'">
							<!-- For the single column case, the included role is always a set consisting of the primary role only -->
							<plx:Operator type="IdentityInequality">
								<plx:Left>
									<plx:Value type="Local" data="currentRole"/>
								</plx:Left>
								<plx:Right>
									<plx:Value type="Local" data="primaryRole"/>
								</plx:Right>
							</plx:Operator>
						</xsl:when>
						<xsl:otherwise>
							<plx:Operator type="BooleanNot">
								<plx:CallInstance name="Contains">
									<plx:CallObject>
										<plx:Value type="Local" data="includedRoles"/>
									</plx:CallObject>
									<plx:PassParam>
										<plx:Value type="Local" data="currentRole"/>
									</plx:PassParam>
								</plx:CallInstance>
							</plx:Operator>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
			</xsl:choose>
			<xsl:if test="@pass='first'">
				<plx:Value type="Local" data="{$FirstPassVariable}"/>
			</xsl:if>
		</xsl:variable>
		<xsl:for-each select="msxsl:node-set($operatorsFragment)/child::*">
			<xsl:if test="position()=1">
				<xsl:call-template name="CombineElements">
					<xsl:with-param name="OperatorType" select="'BooleanAnd'"/>
				</xsl:call-template>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>
	<xsl:template name="PredicateReplacementBody">
		<xsl:for-each select="ve:Quantifier">
			<plx:Operator type="Assign">
				<plx:Left>
					<plx:Value type="Local" data="roleReplacement"/>
				</plx:Left>
				<plx:Right>
					<xsl:choose>
						<xsl:when test="@type='null'">
							<!-- Special case so that we can eliminate replacement text fields -->
							<plx:String/>
						</xsl:when>
						<xsl:otherwise>
							<plx:CallStatic name="Format" dataTypeName="String" dataTypeQualifier="System">
								<plx:PassParam>
									<plx:CallInstance name="FormatProvider" type="Property">
										<plx:CallObject>
											<plx:Value type="Parameter" data="writer"/>
										</plx:CallObject>
									</plx:CallInstance>
								</plx:PassParam>
								<plx:PassParam>
									<xsl:call-template name="SnippetFor">
										<xsl:with-param name="SnippetType" select="@type"/>
									</xsl:call-template>
								</plx:PassParam>
								<plx:PassParam>
									<plx:Value type="Local" data="basicReplacement"/>
								</plx:PassParam>
							</plx:CallStatic>
						</xsl:otherwise>
					</xsl:choose>
				</plx:Right>
			</plx:Operator>
		</xsl:for-each>
	</xsl:template>
	<!-- An IterateRoles tag is used to walk a set of roles and combine verbalizations for
		 the roles into a list. The type of verbalization depends on the match and filter attributes
		 specified on the list. The list separators are determined by the contents of the listStyle attribute. -->
	<xsl:template match="ve:IterateRoles" mode="ConstraintVerbalization">
		<xsl:param name="TopLevel" select="false()"/>
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="'factText'"/>
		<xsl:param name="CompositeCount"/>
		<xsl:param name="CompositeIterator"/>
		<xsl:param name="ListStyle" select="@listStyle"/>
		<xsl:param name="PatternGroup"/>
		<!-- Other parameters should be forwarded to IterateRolesConstraintVerbalizationBody -->
		
		<!-- Normalize the match data -->
		<xsl:variable name="contextMatchFragment">
			<xsl:choose>
				<xsl:when test="string-length(@match)">
					<xsl:value-of select="@match"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text>all</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="contextMatch" select="string($contextMatchFragment)"/>
		<xsl:variable name="iterVarName" select="concat($VariablePrefix,'roleIter',$VariableDecorator)"/>
		<xsl:if test="not($TopLevel)">
			<xsl:call-template name="EnsureTempStringBuilder"/>
		</xsl:if>
		<plx:Variable name="{$iterVarName}" dataTypeName="Int32" dataTypeQualifier="System">
			<plx:Initialize>
				<plx:Value type="I4" data="0"/>
			</plx:Initialize>
		</plx:Variable>

		<!-- See if any filters are in place. If there are, then pre-walk the elements to
			 get a total count so we can build an accurate list during the main iterator -->
		<xsl:variable name="filterTestFragment">
			<xsl:variable name="filterOperatorsFragment">
				<xsl:apply-templates select="@*" mode="IterateRolesFilterOperator"/>
			</xsl:variable>
			<xsl:for-each select="msxsl:node-set($filterOperatorsFragment)/child::*">
				<xsl:if test="position()=1">
					<xsl:call-template name="CombineElements">
						<xsl:with-param name="OperatorType" select="'BooleanAnd'"/>
					</xsl:call-template>
				</xsl:if>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="filterTest" select="msxsl:node-set($filterTestFragment)/child::*"/>
		<xsl:variable name="filteredCountVarName" select="concat($VariablePrefix,'FilteredCount',$VariableDecorator)"/>
		<xsl:variable name="filteredIterVarName" select="concat($VariablePrefix,'FilteredIter',$VariableDecorator)"/>
		<xsl:variable name="trackFirstPass" select="0!=count(descendant::ve:PredicateReplacement[@pass='first'])"/>
		<xsl:variable name="trackFirstPassVarName" select="concat($VariablePrefix,'IsFirstPass',$VariableDecorator)"/>
		<xsl:if test="$trackFirstPass">
			<plx:Variable name="{$trackFirstPassVarName}" dataTypeName="Boolean" dataTypeQualifier="System">
				<plx:Initialize>
					<plx:TrueKeyword/>
				</plx:Initialize>
			</plx:Variable>
		</xsl:if>
		<xsl:if test="$filterTest">
			<xsl:if test="0=string-length($CompositeCount)">
				<plx:Variable name="{$filteredCountVarName}" dataTypeName="Int32" dataTypeQualifier="System">
					<plx:Initialize>
						<plx:Value type="I4" data="0"/>
					</plx:Initialize>
				</plx:Variable>
				<plx:Variable name="{$filteredIterVarName}" dataTypeName="Int32" dataTypeQualifier="System"/>
				<xsl:apply-templates select="." mode="CompositeOrFilteredListCount">
					<xsl:with-param name="TotalCountCollectorVariable" select="$filteredCountVarName"/>
					<xsl:with-param name="IteratorVariableName" select="$filteredIterVarName"/>
					<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
				</xsl:apply-templates>
				<plx:Operator type="Assign">
					<plx:Left>
						<plx:Value type="Local" data="{$filteredIterVarName}"/>
					</plx:Left>
					<plx:Right>
						<plx:Value type="I4" data="0"/>
					</plx:Right>
				</plx:Operator>
			</xsl:if>
		</xsl:if>
		<plx:Loop>
			<plx:LoopTest apply="Before">
				<plx:Operator type="LessThan">
					<plx:Left>
						<plx:Value type="Local" data="{$iterVarName}"/>
					</plx:Left>
					<plx:Right>
						<xsl:choose>
							<xsl:when test="@pass='first'">
								<plx:Value type="I4" data="1"/>
							</xsl:when>
							<xsl:otherwise>
								<plx:Value type="Local">
									<xsl:attribute name="data">
										<xsl:choose>
											<xsl:when test="$contextMatch='all'">
												<xsl:text>factArity</xsl:text>
											</xsl:when>
											<xsl:when test="$contextMatch='included'">
												<xsl:text>includedArity</xsl:text>
											</xsl:when>
											<xsl:when test="$contextMatch='singleColumnConstraintRoles'">
												<xsl:text>constraintRoleArity</xsl:text>
											</xsl:when>
											<!-- UNDONE: Support excluded match -->
										</xsl:choose>
									</xsl:attribute>
								</plx:Value>
							</xsl:otherwise>
						</xsl:choose>
					</plx:Right>
				</plx:Operator>
			</plx:LoopTest>
			<plx:LoopIncrement>
				<plx:Operator type="Assign">
					<plx:Left>
						<plx:Value type="Local" data="{$iterVarName}"/>
					</plx:Left>
					<plx:Right>
						<plx:Operator type="Add">
							<plx:Left>
								<plx:Value type="Local" data="{$iterVarName}"/>
							</plx:Left>
							<plx:Right>
								<plx:Value type="I4" data="1"/>
							</plx:Right>
						</plx:Operator>
					</plx:Right>
				</plx:Operator>
			</plx:LoopIncrement>
			<plx:Body>
				<plx:Variable name="primaryRole" dataTypeName="Role">
					<plx:Initialize>
						<plx:CallInstance name="" type="ArrayIndexer">
							<plx:CallObject>
								<plx:Value type="Local">
									<xsl:attribute name="data">
										<xsl:choose>
											<xsl:when test="$contextMatch='all'">
												<xsl:text>factRoles</xsl:text>
											</xsl:when>
											<xsl:when test="$contextMatch='included'">
												<xsl:text>includedRoles</xsl:text>
											</xsl:when>
											<xsl:when test="$contextMatch='singleColumnConstraintRoles'">
												<xsl:text>allConstraintRoles</xsl:text>
											</xsl:when>
											<!-- UNDONE: Support excluded match -->
										</xsl:choose>
									</xsl:attribute>
								</plx:Value>
							</plx:CallObject>
							<plx:PassParam>
								<plx:Value type="Local" data="{$iterVarName}"/>
							</plx:PassParam>
						</plx:CallInstance>
					</plx:Initialize>
				</plx:Variable>
				<xsl:if test="$contextMatch='singleColumnConstraintRoles'">
					<plx:Operator type="Assign">
						<plx:Left>
							<plx:Value type="Local" data="parentFact"/>
						</plx:Left>
						<plx:Right>
							<plx:CallInstance name="FactType" type="Property">
								<plx:CallObject>
									<plx:Value type="Local" data="primaryRole"/>
								</plx:CallObject>
							</plx:CallInstance>
						</plx:Right>
					</plx:Operator>
					<plx:Operator type="Assign">
						<plx:Left>
							<plx:Value type="Local" data="factRoles"/>
						</plx:Left>
						<plx:Right>
							<plx:CallInstance name="RoleCollection" type="Property">
								<plx:CallObject>
									<plx:Value type="Local" data="parentFact"/>
								</plx:CallObject>
							</plx:CallInstance>
						</plx:Right>
					</plx:Operator>
					<plx:Operator type="Assign">
						<plx:Left>
							<plx:Value type="Local" data="factArity"/>
						</plx:Left>
						<plx:Right>
							<plx:CallInstance name="Count" type="Property">
								<plx:CallObject>
									<plx:Value type="Local" data="factRoles"/>
								</plx:CallObject>
							</plx:CallInstance>
						</plx:Right>
					</plx:Operator>
					<plx:Operator type="Assign">
						<plx:Left>
							<plx:Value type="Local" data="allReadingOrders"/>
						</plx:Left>
						<plx:Right>
							<plx:CallInstance name="ReadingOrderCollection" type="Property">
								<plx:CallObject>
									<plx:Value type="Local" data="parentFact"/>
								</plx:CallObject>
							</plx:CallInstance>
						</plx:Right>
					</plx:Operator>
					<plx:Variable name="currentFactIndex" dataTypeName="Int32" dataTypeQualifier="System">
						<plx:Initialize>
							<plx:CallInstance name="IndexOf">
								<plx:CallObject>
									<plx:Value type="Local" data="allFacts"/>
								</plx:CallObject>
								<plx:PassParam>
									<plx:Value type="Local" data="parentFact"/>
								</plx:PassParam>
							</plx:CallInstance>
						</plx:Initialize>
					</plx:Variable>
					<plx:Variable name="basicRoleReplacements" dataTypeName="String" dataTypeQualifier="System" dataTypeIsSimpleArray="true">
						<plx:Initialize>
							<plx:CallInstance name="" type="ArrayIndexer">
								<plx:CallObject>
									<plx:Value type="Local" data="allBasicRoleReplacements"/>
								</plx:CallObject>
								<plx:PassParam>
									<plx:Value type="Local" data="currentFactIndex"/>
								</plx:PassParam>
							</plx:CallInstance>
						</plx:Initialize>
					</plx:Variable>
				</xsl:if>
				<xsl:choose>
					<xsl:when test="$filterTest">
						<plx:Condition>
							<plx:Test>
								<xsl:copy-of select="$filterTest"/>
							</plx:Test>
							<plx:Body>
								<xsl:variable name="passCompositeCount">
									<xsl:choose>
										<xsl:when test="string-length($CompositeCount)">
											<xsl:value-of select="$CompositeCount"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="$filteredCountVarName"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
								<xsl:variable name="passCompositeIterator">
									<xsl:choose>
										<xsl:when test="string-length($CompositeIterator)">
											<xsl:value-of select="$CompositeIterator"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="$filteredIterVarName"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
								<xsl:call-template name="IterateRolesConstraintVerbalizationBody">
									<xsl:with-param name="TopLevel" select="$TopLevel"/>
									<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
									<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
									<xsl:with-param name="CompositeCount" select="string($passCompositeCount)"/>
									<xsl:with-param name="CompositeIterator" select="string($passCompositeIterator)"/>
									<xsl:with-param name="ListStyle" select="$ListStyle"/>
									<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
									<xsl:with-param name="FirstPassVariable" select="$trackFirstPassVarName"/>
									<!-- Forwarded local parameters -->
									<xsl:with-param name="contextMatch" select="$contextMatch"/>
									<xsl:with-param name="iterVarName" select="$iterVarName"/>
								</xsl:call-template>
								<plx:Operator type="Assign">
									<plx:Left>
										<plx:Value type="Local" data="{$passCompositeIterator}"/>
									</plx:Left>
									<plx:Right>
										<plx:Operator type="Add">
											<plx:Left>
												<plx:Value type="Local" data="{$passCompositeIterator}"/>
											</plx:Left>
											<plx:Right>
												<plx:Value type="I4" data="1"/>
											</plx:Right>
										</plx:Operator>
									</plx:Right>
								</plx:Operator>
								<xsl:if test="$trackFirstPass">
									<plx:Operator type="Assign">
										<plx:Left>
											<plx:Value type="Local" data="{$trackFirstPassVarName}"/>
										</plx:Left>
										<plx:Right>
											<plx:FalseKeyword/>
										</plx:Right>
									</plx:Operator>
								</xsl:if>
							</plx:Body>
						</plx:Condition>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="IterateRolesConstraintVerbalizationBody">
							<xsl:with-param name="TopLevel" select="$TopLevel"/>
							<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
							<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
							<xsl:with-param name="CompositeCount" select="$CompositeCount"/>
							<xsl:with-param name="CompositeIterator" select="$CompositeIterator"/>
							<xsl:with-param name="ListStyle" select="$ListStyle"/>
							<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
							<xsl:with-param name="FirstPassVariable" select="$trackFirstPassVarName"/>
							<!-- Forwarded local parameters -->
							<xsl:with-param name="contextMatch" select="$contextMatch"/>
							<xsl:with-param name="iterVarName" select="$iterVarName"/>
						</xsl:call-template>
						<xsl:if test="$trackFirstPass">
							<plx:Operator type="Assign">
								<plx:Left>
									<plx:Value type="Local" data="{$trackFirstPassVarName}"/>
								</plx:Left>
								<plx:Right>
									<plx:FalseKeyword/>
								</plx:Right>
							</plx:Operator>
						</xsl:if>
					</xsl:otherwise>
				</xsl:choose>
			</plx:Body>
		</plx:Loop>
		<xsl:if test="not($TopLevel)">
			<plx:Operator type="Assign">
				<plx:Left>
					<plx:Value type="Local" data="{$VariablePrefix}{$VariableDecorator}"/>
				</plx:Left>
				<plx:Right>
					<plx:CallInstance name="ToString">
						<plx:CallObject>
							<plx:Value type="Local" data="sbTemp"/>
						</plx:CallObject>
					</plx:CallInstance>
				</plx:Right>
			</plx:Operator>
		</xsl:if>
	</xsl:template>
	<!-- A helper template to spit the body of an IterateRoles iteration
		 either inside a filter conditional or directly -->
	<xsl:template name="IterateRolesConstraintVerbalizationBody">
		<!-- Primary forward parameters -->
		<xsl:param name="TopLevel"/>
		<xsl:param name="VariableDecorator"/>
		<xsl:param name="VariablePrefix"/>
		<xsl:param name="CompositeCount"/>
		<xsl:param name="CompositeIterator"/>
		<xsl:param name="ListStyle"/>
		<xsl:param name="PatternGroup"/>
		<xsl:param name="FirstPassVariable"/>
		<!-- Forwarded local parameters -->
		<xsl:param name="contextMatch"/>
		<xsl:param name="iterVarName"/>
		<!-- Use the current snippets data to open the list -->
		<plx:Variable name="listSnippet" dataTypeName="{$VerbalizationTextSnippetType}"/>
		<xsl:choose>
			<xsl:when test="@pass='first' and 0=string-length($CompositeCount)">
				<xsl:call-template name="SetSnippetVariable">
					<xsl:with-param name="SnippetType" select="concat($ListStyle,'Open')"/>
					<xsl:with-param name="VariableName" select="'listSnippet'"/>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<plx:Condition>
					<plx:Test>
						<plx:Operator type="Equality">
							<plx:Left>
								<plx:Value type="Local" data="{$iterVarName}">
									<xsl:if test="string-length($CompositeIterator)">
										<xsl:attribute name="data">
											<xsl:value-of select="$CompositeIterator"/>
										</xsl:attribute>
									</xsl:if>
								</plx:Value>
							</plx:Left>
							<plx:Right>
								<plx:Value type="I4" data="0"/>
							</plx:Right>
						</plx:Operator>
					</plx:Test>
					<plx:Body>
						<xsl:call-template name="SetSnippetVariable">
							<xsl:with-param name="SnippetType" select="concat($ListStyle,'Open')"/>
							<xsl:with-param name="VariableName" select="'listSnippet'"/>
						</xsl:call-template>
					</plx:Body>
					<!-- UNDONE: We could spit less code here if we pass the arity
						 in from the ConstrainedRoles tag. -->
					<plx:FallbackCondition>
						<plx:Test>
							<plx:Operator type="Equality">
								<plx:Left>
									<plx:Value type="Local" data="{$iterVarName}">
										<xsl:if test="string-length($CompositeIterator)">
											<xsl:attribute name="data">
												<xsl:value-of select="$CompositeIterator"/>
											</xsl:attribute>
										</xsl:if>
									</plx:Value>
								</plx:Left>
								<plx:Right>
									<plx:Operator type="Subtract">
										<plx:Left>
											<plx:Value type="Local">
												<xsl:attribute name="data">
													<xsl:choose>
														<xsl:when test="string-length($CompositeCount)">
															<xsl:value-of select="$CompositeCount"/>
														</xsl:when>
														<xsl:when test="$contextMatch='all'">
															<xsl:text>factArity</xsl:text>
														</xsl:when>
														<xsl:when test="$contextMatch='included'">
															<xsl:text>includedArity</xsl:text>
														</xsl:when>
														<xsl:when test="$contextMatch='singleColumnConstraintRoles'">
															<xsl:text>constraintRoleArity</xsl:text>
														</xsl:when>
														<!-- UNDONE: Support excluded match -->
													</xsl:choose>
												</xsl:attribute>
											</plx:Value>
										</plx:Left>
										<plx:Right>
											<plx:Value type="I4" data="1"/>
										</plx:Right>
									</plx:Operator>
								</plx:Right>
							</plx:Operator>
						</plx:Test>
						<plx:Body>
							<plx:Condition>
								<plx:Test>
									<plx:Operator type="Equality">
										<plx:Left>
											<plx:Value type="Local" data="{$iterVarName}">
												<xsl:if test="string-length($CompositeIterator)">
													<xsl:attribute name="data">
														<xsl:value-of select="$CompositeIterator"/>
													</xsl:attribute>
												</xsl:if>
											</plx:Value>
										</plx:Left>
										<plx:Right>
											<plx:Value type="I4" data="1"/>
										</plx:Right>
									</plx:Operator>
								</plx:Test>
								<plx:Body>
									<xsl:call-template name="SetSnippetVariable">
										<xsl:with-param name="SnippetType" select="concat($ListStyle,'PairSeparator')"/>
										<xsl:with-param name="VariableName" select="'listSnippet'"/>
									</xsl:call-template>
								</plx:Body>
								<plx:Alternate>
									<xsl:call-template name="SetSnippetVariable">
										<xsl:with-param name="SnippetType" select="concat($ListStyle,'FinalSeparator')"/>
										<xsl:with-param name="VariableName" select="'listSnippet'"/>
									</xsl:call-template>
								</plx:Alternate>
							</plx:Condition>
						</plx:Body>
					</plx:FallbackCondition>
					<plx:Alternate>
						<xsl:call-template name="SetSnippetVariable">
							<xsl:with-param name="SnippetType" select="concat($ListStyle,'Separator')"/>
							<xsl:with-param name="VariableName" select="'listSnippet'"/>
						</xsl:call-template>
					</plx:Alternate>
				</plx:Condition>
			</xsl:otherwise>
		</xsl:choose>
		<plx:CallInstance name="Append">
			<xsl:if test="$TopLevel">
				<xsl:attribute name="name">
					<xsl:text>Write</xsl:text>
				</xsl:attribute>
			</xsl:if>
			<plx:CallObject>
				<xsl:choose>
					<xsl:when test="$TopLevel">
						<plx:Value type="Parameter" data="writer"/>
					</xsl:when>
					<xsl:otherwise>
						<plx:Value type="Local" data="sbTemp"/>
					</xsl:otherwise>
				</xsl:choose>
			</plx:CallObject>
			<plx:PassParam>
				<xsl:call-template name="SnippetFor">
					<xsl:with-param name="VariableName" select="'listSnippet'"/>
				</xsl:call-template>
			</plx:PassParam>
		</plx:CallInstance>

		<!-- Process the child contents for this role -->
		<xsl:choose>
			<xsl:when test="count(child::*)">
				<xsl:for-each select="child::*">
					<!-- Let children assign directly to the normal replacement variable so
						 that we don't have to communicate down the stack that they should assign
						 directly to the temp string builder. -->
					<plx:Operator type="Assign">
						<plx:Left>
							<plx:Value type="Local" data="{$VariablePrefix}{$VariableDecorator}"/>
						</plx:Left>
						<plx:Right>
							<plx:NullObjectKeyword/>
						</plx:Right>
					</plx:Operator>
					<xsl:apply-templates select="." mode="ConstraintVerbalization">
						<xsl:with-param name="VariablePrefix" select="$VariablePrefix"/>
						<!-- Pass the position in here or it will always be 1 -->
						<xsl:with-param name="VariableDecorator" select="$VariableDecorator"/>
						<xsl:with-param name="IteratorContext" select="$contextMatch"/>
						<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
						<xsl:with-param name="FirstPassVariable" select="$FirstPassVariable"/>
					</xsl:apply-templates>
					<plx:CallInstance name="Append">
						<xsl:if test="$TopLevel">
							<xsl:attribute name="name">
								<xsl:text>Write</xsl:text>
							</xsl:attribute>
						</xsl:if>
						<plx:CallObject>
							<xsl:choose>
								<xsl:when test="$TopLevel">
									<plx:Value type="Parameter" data="writer"/>
								</xsl:when>
								<xsl:otherwise>
									<plx:Value type="Local" data="sbTemp"/>
								</xsl:otherwise>
							</xsl:choose>
						</plx:CallObject>
						<plx:PassParam>
							<plx:Value type="Local" data="{$VariablePrefix}{$VariableDecorator}"/>
						</plx:PassParam>
					</plx:CallInstance>
				</xsl:for-each>
			</xsl:when>
			<xsl:otherwise>
				<plx:CallInstance name="Append">
					<xsl:if test="$TopLevel">
						<xsl:attribute name="name">
							<xsl:text>Write</xsl:text>
						</xsl:attribute>
					</xsl:if>
					<plx:CallObject>
						<xsl:choose>
							<xsl:when test="$TopLevel">
								<plx:Value type="Parameter" data="writer"/>
							</xsl:when>
							<xsl:otherwise>
								<plx:Value type="Local" data="sbTemp"/>
							</xsl:otherwise>
						</xsl:choose>
					</plx:CallObject>
					<plx:PassParam>
						<plx:CallInstance name="" type="ArrayIndexer">
							<plx:CallObject>
								<plx:Value type="Local" data="basicRoleReplacements"/>
							</plx:CallObject>
							<plx:PassParam>
								<xsl:choose>
									<xsl:when test="@match='included' or @match='singleColumnConstraintRoles'">
										<!-- The role index needs to be retrieved from the all roles list -->
										<plx:CallInstance name="IndexOf">
											<plx:CallObject>
												<plx:Value type="Local" data="factRoles"/>
											</plx:CallObject>
											<plx:PassParam>
												<plx:CallInstance name="" type="ArrayIndexer">
													<plx:CallObject>
														<plx:Value type="Local" data="includedRoles">
															<xsl:if test="@match='singleColumnConstraintRoles'">
																<xsl:attribute name="data">
																	<xsl:text>allConstraintRoles</xsl:text>
																</xsl:attribute>
															</xsl:if>
														</plx:Value>
													</plx:CallObject>
													<plx:PassParam>
														<plx:Value type="Local" data="{$iterVarName}"/>
													</plx:PassParam>
												</plx:CallInstance>
											</plx:PassParam>
										</plx:CallInstance>
									</xsl:when>
									<!-- UNDONE: Support excluded match -->
									<xsl:otherwise>
										<plx:Value type="Local" data="{$iterVarName}"/>
									</xsl:otherwise>
								</xsl:choose>
							</plx:PassParam>
						</plx:CallInstance>
					</plx:PassParam>
				</plx:CallInstance>
			</xsl:otherwise>
		</xsl:choose>

		<!-- Use the current snippets data to close the list -->
		<xsl:choose>
			<xsl:when test="@pass='first' and 0=string-length($CompositeCount)">
				<plx:CallInstance name="Append">
					<xsl:if test="$TopLevel">
						<xsl:attribute name="name">
							<xsl:text>Write</xsl:text>
						</xsl:attribute>
					</xsl:if>
					<plx:CallObject>
						<xsl:choose>
							<xsl:when test="$TopLevel">
								<plx:Value type="Parameter" data="writer"/>
							</xsl:when>
							<xsl:otherwise>
								<plx:Value type="Local" data="sbTemp"/>
							</xsl:otherwise>
						</xsl:choose>
					</plx:CallObject>
					<plx:PassParam>
						<xsl:call-template name="SnippetFor">
							<xsl:with-param name="SnippetType" select="concat($ListStyle,'Close')"/>
						</xsl:call-template>
					</plx:PassParam>
				</plx:CallInstance>
			</xsl:when>
			<xsl:otherwise>
				<plx:Condition>
					<plx:Test>
						<plx:Operator type="Equality">
							<plx:Left>
								<plx:Value type="Local" data="{$iterVarName}"/>
							</plx:Left>
							<plx:Right>
								<plx:Operator type="Subtract">
									<plx:Left>
										<plx:Value type="Local">
											<xsl:attribute name="data">
												<xsl:choose>
													<xsl:when test="string-length($CompositeCount)">
														<xsl:value-of select="$CompositeCount"/>
													</xsl:when>
													<xsl:when test="$contextMatch='all'">
														<xsl:text>factArity</xsl:text>
													</xsl:when>
													<xsl:when test="$contextMatch='included'">
														<xsl:text>includedArity</xsl:text>
													</xsl:when>
													<xsl:when test="$contextMatch='singleColumnConstraintRoles'">
														<xsl:text>constraintRoleArity</xsl:text>
													</xsl:when>
													<!-- UNDONE: Support excluded match -->
												</xsl:choose>
											</xsl:attribute>
										</plx:Value>
									</plx:Left>
									<plx:Right>
										<plx:Value type="I4" data="1"/>
									</plx:Right>
								</plx:Operator>
							</plx:Right>
						</plx:Operator>
					</plx:Test>
					<plx:Body>
						<plx:CallInstance name="Append">
							<xsl:if test="$TopLevel">
								<xsl:attribute name="name">
									<xsl:text>Write</xsl:text>
								</xsl:attribute>
							</xsl:if>
							<plx:CallObject>
								<xsl:choose>
									<xsl:when test="$TopLevel">
										<plx:Value type="Parameter" data="writer"/>
									</xsl:when>
									<xsl:otherwise>
										<plx:Value type="Local" data="sbTemp"/>
									</xsl:otherwise>
								</xsl:choose>
							</plx:CallObject>
							<plx:PassParam>
								<xsl:call-template name="SnippetFor">
									<xsl:with-param name="SnippetType" select="concat($ListStyle,'Close')"/>
								</xsl:call-template>
							</plx:PassParam>
						</plx:CallInstance>
					</plx:Body>
				</plx:Condition>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- A CompositeList tag is used to combine one or more IterateRoles lists into
		 a single list. The listStyle parameter is ignored on IterateRoles if this is set. -->
	<xsl:template match="ve:CompositeList" mode="ConstraintVerbalization">
		<xsl:param name="TopLevel" select="false()"/>
		<xsl:param name="VariableDecorator" select="position()"/>
		<xsl:param name="VariablePrefix" select="'list'"/>
		<xsl:param name="PatternGroup"/>
		<xsl:if test="$TopLevel">
			<xsl:choose>
				<xsl:when test="position()&gt;1">
					<plx:CallInstance name="WriteLine">
						<plx:CallObject>
							<plx:Value type="Parameter" data="writer"/>
						</plx:CallObject>
					</plx:CallInstance>
				</xsl:when>
				<xsl:otherwise>
					<plx:CallInstance name="" type="DelegateCall">
						<plx:CallObject>
							<plx:Value type="Parameter" data="beginVerbalization"/>
						</plx:CallObject>
						<plx:PassParam>
							<plx:CallStatic name="Normal" dataTypeName="VerbalizationContent" type="Field"/>
						</plx:PassParam>
					</plx:CallInstance>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:variable name="compositeCountVarName" select="concat($VariablePrefix,'CompositeCount',$VariableDecorator)"/>
		<plx:Variable name="{$compositeCountVarName}" dataTypeName="Int32" dataTypeQualifier="System">
			<plx:Initialize>
				<plx:Value type="I4" data="0"/>
			</plx:Initialize>
		</plx:Variable>
		<xsl:variable name="iteratorVarName" select="concat($VariablePrefix,'CompositeIterator',$VariableDecorator)"/>
		<plx:Variable name="{$iteratorVarName}" dataTypeName="Int32" dataTypeQualifier="System"/>
		<xsl:apply-templates select="child::*" mode="CompositeOrFilteredListCount">
			<xsl:with-param name="TotalCountCollectorVariable" select="$compositeCountVarName"/>
			<xsl:with-param name="IteratorVariableName" select="$iteratorVarName"/>
			<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
		</xsl:apply-templates>
		<plx:Operator type="Assign">
			<plx:Left>
				<plx:Value type="Local" data="{$iteratorVarName}"/>
			</plx:Left>
			<plx:Right>
				<plx:Value type="I4" data="0"/>
			</plx:Right>
		</plx:Operator>
		<xsl:variable name="ListStyle" select="@listStyle"/>
		<xsl:for-each select="child::*">
			<plx:Variable name="{$VariablePrefix}{$VariableDecorator}Item{position()}" dataTypeName="String" dataTypeQualifier="System">
				<plx:Initialize>
					<plx:NullObjectKeyword/>
				</plx:Initialize>
			</plx:Variable>
			<xsl:apply-templates select="." mode="ConstraintVerbalization">
				<xsl:with-param name="VariablePrefix" select="concat($VariablePrefix,$VariableDecorator,'Item')"/>
				<xsl:with-param name="VariableDecorator" select="position()"/>
				<xsl:with-param name="CompositeCount" select="$compositeCountVarName"/>
				<xsl:with-param name="CompositeIterator" select="$iteratorVarName"/>
				<xsl:with-param name="TopLevel" select="$TopLevel"/>
				<xsl:with-param name="ListStyle" select="$ListStyle"/>
				<xsl:with-param name="PatternGroup" select="$PatternGroup"/>
			</xsl:apply-templates>
		</xsl:for-each>
	</xsl:template>
	<!-- Handle the minFactArity IterateRoles filter attribute -->
	<xsl:template match="@minFactArity" mode="IterateRolesFilterOperator">
		<plx:Operator type="GreaterThanOrEqual">
			<plx:Left>
				<plx:Value type="Local" data="factArity"/>
			</plx:Left>
			<plx:Right>
				<plx:Value type="I4" data="{.}"/>
			</plx:Right>
		</plx:Operator>
	</xsl:template>
	<!-- Handle the maxFactArity IterateRoles filter attribute -->
	<xsl:template match="@maxFactArity" mode="IterateRolesFilterOperator">
		<plx:Operator type="LessThanOrEqual">
			<plx:Left>
				<plx:Value type="Local" data="factArity"/>
			</plx:Left>
			<plx:Right>
				<plx:Value type="I4" data="{.}"/>
			</plx:Right>
		</plx:Operator>
	</xsl:template>
	<!-- Handle the factArity IterateRoles filter attribute -->
	<xsl:template match="@factArity" mode="IterateRolesFilterOperator">
		<plx:Operator type="Equality">
			<plx:Left>
				<plx:Value type="Local" data="factArity"/>
			</plx:Left>
			<plx:Right>
				<plx:Value type="I4" data="{.}"/>
			</plx:Right>
		</plx:Operator>
	</xsl:template>
	<!-- Ignore the match attribute, it is not used as a filter -->
	<xsl:template match="@match|@listStyle|@pass" mode="IterateRolesFilterOperator"/>
	<!-- Terminate processing if we see an unrecognized operator -->
	<xsl:template match="@*" mode="IterateRolesFilterOperator">
		<xsl:call-template name="TerminateForInvalidAttribute">
			<xsl:with-param name="MessageText">Unrecognized role iterator filter attribute</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<xsl:template match="ve:IterateRoles" mode="CompositeOrFilteredListCount">
		<xsl:param name="TotalCountCollectorVariable"/>
		<xsl:param name="IteratorVariableName"/>
		<xsl:param name="PatternGroup"/>
		<xsl:variable name="contextMatchFragment">
			<xsl:choose>
				<xsl:when test="string-length(@match)">
					<xsl:value-of select="@match"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text>all</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="contextMatch" select="string($contextMatchFragment)"/>
		<xsl:variable name="filterTestFragment">
			<xsl:variable name="filterOperatorsFragment">
				<xsl:apply-templates select="@*" mode="IterateRolesFilterOperator"/>
			</xsl:variable>
			<xsl:for-each select="msxsl:node-set($filterOperatorsFragment)/child::*">
				<xsl:if test="position()=1">
					<xsl:call-template name="CombineElements">
						<xsl:with-param name="OperatorType" select="'BooleanAnd'"/>
					</xsl:call-template>
				</xsl:if>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="filterTest" select="msxsl:node-set($filterTestFragment)/child::*"/>
		
		<!-- Get the count for the set in a value, which will be used either to
			 increment the full count or as a filter upper bound -->
		<xsl:variable name="setCountValueFragment">
			<xsl:choose>
				<xsl:when test="@pass='first'">
					<plx:Value type="I4" data="1"/>
				</xsl:when>
				<xsl:otherwise>
					<plx:Value type="Local">
						<xsl:attribute name="data">
							<xsl:choose>
								<xsl:when test="$contextMatch='all'">
									<xsl:text>factArity</xsl:text>
								</xsl:when>
								<xsl:when test="$contextMatch='included'">
									<xsl:text>includedArity</xsl:text>
								</xsl:when>
								<xsl:when test="$contextMatch='singleColumnConstraintRoles'">
									<xsl:text>constraintRoleArity</xsl:text>
								</xsl:when>
								<!-- UNDONE: Support excluded match -->
							</xsl:choose>
						</xsl:attribute>
					</plx:Value>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="setCountValue" select="msxsl:node-set($setCountValueFragment)/child::*"/>
		<xsl:choose>
			<xsl:when test="$filterTest">
				<plx:Loop>
					<plx:Initialize>
						<plx:Operator type="Assign">
							<plx:Left>
								<plx:Value type="Local" data="{$IteratorVariableName}"/>
							</plx:Left>
							<plx:Right>
								<plx:Value type="I4" data="0"/>
							</plx:Right>
						</plx:Operator>
					</plx:Initialize>
					<plx:LoopTest>
						<plx:Operator type="LessThan">
							<plx:Left>
								<plx:Value type="Local" data="{$IteratorVariableName}"/>
							</plx:Left>
							<plx:Right>
								<xsl:copy-of select="$setCountValue"/>
							</plx:Right>
						</plx:Operator>
					</plx:LoopTest>
					<plx:LoopIncrement>
						<plx:Operator type="Assign">
							<plx:Left>
								<plx:Value type="Local" data="{$IteratorVariableName}"/>
							</plx:Left>
							<plx:Right>
								<plx:Operator type="Add">
									<plx:Left>
										<plx:Value type="Local" data="{$IteratorVariableName}"/>
									</plx:Left>
									<plx:Right>
										<plx:Value type="I4" data="1"/>
									</plx:Right>
								</plx:Operator>
							</plx:Right>
						</plx:Operator>
					</plx:LoopIncrement>
					<plx:Body>
						<!-- UNDONE: We may not need this for all cases, it depends on the
							 filters in place. -->
						<plx:Variable name="primaryRole" dataTypeName="Role">
							<plx:Initialize>
								<plx:CallInstance name="" type="ArrayIndexer">
									<plx:CallObject>
										<plx:Value type="Local">
											<xsl:attribute name="data">
												<xsl:choose>
													<xsl:when test="$contextMatch='all'">
														<xsl:text>factRoles</xsl:text>
													</xsl:when>
													<xsl:when test="$contextMatch='included'">
														<xsl:text>includedRoles</xsl:text>
													</xsl:when>
													<xsl:when test="$contextMatch='singleColumnConstraintRoles'">
														<xsl:text>allConstraintRoles</xsl:text>
													</xsl:when>
													<!-- UNDONE: Support excluded match -->
												</xsl:choose>
											</xsl:attribute>
										</plx:Value>
									</plx:CallObject>
									<plx:PassParam>
										<plx:Value type="Local" data="{$IteratorVariableName}"/>
									</plx:PassParam>
								</plx:CallInstance>
							</plx:Initialize>
						</plx:Variable>
						<xsl:if test="$contextMatch='singleColumnConstraintRoles'">
							<plx:Operator type="Assign">
								<plx:Left>
									<plx:Value type="Local" data="parentFact"/>
								</plx:Left>
								<plx:Right>
									<plx:CallInstance name="FactType" type="Property">
										<plx:CallObject>
											<plx:Value type="Local" data="primaryRole"/>
										</plx:CallObject>
									</plx:CallInstance>
								</plx:Right>
							</plx:Operator>
							<plx:Operator type="Assign">
								<plx:Left>
									<plx:Value type="Local" data="factRoles"/>
								</plx:Left>
								<plx:Right>
									<plx:CallInstance name="RoleCollection" type="Property">
										<plx:CallObject>
											<plx:Value type="Local" data="parentFact"/>
										</plx:CallObject>
									</plx:CallInstance>
								</plx:Right>
							</plx:Operator>
							<plx:Operator type="Assign">
								<plx:Left>
									<plx:Value type="Local" data="factArity"/>
								</plx:Left>
								<plx:Right>
									<plx:CallInstance name="Count" type="Property">
										<plx:CallObject>
											<plx:Value type="Local" data="factRoles"/>
										</plx:CallObject>
									</plx:CallInstance>
								</plx:Right>
							</plx:Operator>
							<plx:Operator type="Assign">
								<plx:Left>
									<plx:Value type="Local" data="allReadingOrders"/>
								</plx:Left>
								<plx:Right>
									<plx:CallInstance name="ReadingOrderCollection" type="Property">
										<plx:CallObject>
											<plx:Value type="Local" data="parentFact"/>
										</plx:CallObject>
									</plx:CallInstance>
								</plx:Right>
							</plx:Operator>
						</xsl:if>
						<plx:Condition>
							<plx:Test>
								<xsl:copy-of select="$filterTest"/>
							</plx:Test>
							<plx:Body>
								<plx:Operator type="Assign">
									<plx:Left>
										<plx:Value type="Local" data="{$TotalCountCollectorVariable}"/>
									</plx:Left>
									<plx:Right>
										<plx:Operator type="Add">
											<plx:Left>
												<plx:Value type="Local" data="{$TotalCountCollectorVariable}"/>
											</plx:Left>
											<plx:Right>
												<plx:Value type="I4" data="1"/>
											</plx:Right>
										</plx:Operator>
									</plx:Right>
								</plx:Operator>
							</plx:Body>
						</plx:Condition>
					</plx:Body>
				</plx:Loop>
			</xsl:when>
			<xsl:otherwise>
				<!-- No filter is in place, just use the total count for the matching set -->
				<plx:Operator type="Assign">
					<plx:Left>
						<plx:Value type="Local" data="{$TotalCountCollectorVariable}"/>
					</plx:Left>
					<plx:Right>
						<plx:Operator type="Add">
							<plx:Left>
								<plx:Value type="Local" data="{$TotalCountCollectorVariable}"/>
							</plx:Left>
							<plx:Right>
								<xsl:copy-of select="$setCountValue"/>
							</plx:Right>
						</plx:Operator>
					</plx:Right>
				</plx:Operator>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- Get the snippet value from the current snippets set.
		 This assumes snippets, isDeontic and isNegative local
		 variables are defined. Alternately, a VariableName
		 containing the name of a local variable containing the
		 text can be passed in instead of SnippetType. -->
	<xsl:template name="SnippetFor">
		<xsl:param name="SnippetType"/>
		<xsl:param name="VariableName"/>
		<plx:CallInstance name="GetSnippet">
			<plx:CallObject>
				<plx:Value type="Local" data="snippets"/>
			</plx:CallObject>
			<plx:PassParam>
				<xsl:choose>
					<xsl:when test="string-length($VariableName)">
						<plx:Value type="Local" data="{$VariableName}"/>
					</xsl:when>
					<xsl:otherwise>
						<plx:CallStatic name="{$SnippetType}" dataTypeName="{$VerbalizationTextSnippetType}" type="Field"/>
					</xsl:otherwise>
				</xsl:choose>
			</plx:PassParam>
			<plx:PassParam>
				<plx:Value type="Local" data="isDeontic"/>
			</plx:PassParam>
			<plx:PassParam>
				<plx:Value type="Local" data="isNegative"/>
			</plx:PassParam>
		</plx:CallInstance>
	</xsl:template>
	<!-- Assign the specified snippet type to a local variable. -->
	<xsl:template name="SetSnippetVariable">
		<xsl:param name="SnippetType"/>
		<xsl:param name="VariableName"/>
		<plx:Operator type="Assign">
			<plx:Left>
				<plx:Value type="Local" data="{$VariableName}"/>
			</plx:Left>
			<plx:Right>
				<plx:CallStatic name="{$SnippetType}" dataTypeName="{$VerbalizationTextSnippetType}" type="Field"/>
			</plx:Right>
		</plx:Operator>
	</xsl:template>
	<!-- Helper function to create an initialized string builder in the sbTemp local variable -->
	<xsl:template name="EnsureTempStringBuilder">
		<plx:Condition>
			<plx:Test>
				<plx:Operator type="IdentityEquality">
					<plx:Left>
						<plx:Value type="Local" data="sbTemp"/>
					</plx:Left>
					<plx:Right>
						<plx:NullObjectKeyword/>
					</plx:Right>
				</plx:Operator>
			</plx:Test>
			<plx:Body>
				<plx:Operator type="Assign">
					<plx:Left>
						<plx:Value type="Local" data="sbTemp"/>
					</plx:Left>
					<plx:Right>
						<plx:CallNew dataTypeName="StringBuilder"/>
					</plx:Right>
				</plx:Operator>
			</plx:Body>
			<plx:Alternate>
				<plx:Operator type="Assign">
					<plx:Left>
						<plx:CallInstance name="Length" type="Property">
							<plx:CallObject>
								<plx:Value type="Local" data="sbTemp"/>
							</plx:CallObject>
						</plx:CallInstance>
					</plx:Left>
					<plx:Right>
						<plx:Value type="I4" data="0"/>
					</plx:Right>
				</plx:Operator>
			</plx:Alternate>
		</plx:Condition>
	</xsl:template>
	<xsl:template name="PopulateReadingOrder">
		<!-- Support readings for {Conditional, {Prefer|Require}[Non][Primary]LeadReading[NoForwardText], null} ReadingChoice values -->
		<xsl:param name="ReadingChoice"/>
		<xsl:param name="PatternGroup"/>
		<xsl:choose>
			<xsl:when test="$ReadingChoice='Conditional' and $PatternGroup='SingleColumnExternalConstraint'">
				<plx:Operator type="Assign">
					<plx:Left>
						<plx:Value type="Local" data="readingOrder"/>
					</plx:Left>
					<plx:Right>
						<plx:CallInstance name="" type="ArrayIndexer">
							<plx:CallObject>
								<plx:Value type="Local" data="allConstraintRoleReadingOrders"/>
							</plx:CallObject>
							<plx:PassParam>
								<plx:Value type="Local" data="currentFactIndex"/>
							</plx:PassParam>
						</plx:CallInstance>
					</plx:Right>
				</plx:Operator>
			</xsl:when>
			<xsl:when test="not($ReadingChoice='Conditional')">
				<plx:Operator type="Assign">
					<plx:Left>
						<plx:Value type="Local" data="readingOrder"/>
					</plx:Left>
					<plx:Right>
						<plx:CallStatic name="GetMatchingReadingOrder" dataTypeName="FactType">
							<plx:PassParam>
								<!-- The readingOrders param-->
								<plx:Value type="Local" data="allReadingOrders"/>
							</plx:PassParam>
							<plx:PassParam>
								<!-- The matchLeadRole param -->
								<xsl:choose>
									<xsl:when test="contains($ReadingChoice,'PrimaryLeadReading')">
										<plx:Value type="Local" data="primaryRole"/>
									</xsl:when>
									<xsl:when test="contains($ReadingChoice, 'LeadReading')">
										<!-- LeadReading and PrimaryLeadReading should be treated the same for single column external constraints -->
										<xsl:choose>
											<xsl:when test="$PatternGroup='SingleColumnExternalConstraint'">
												<plx:Value type="Local" data="primaryRole"/>
											</xsl:when>
											<xsl:otherwise>
												<plx:NullObjectKeyword/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<plx:CallInstance name="" type="ArrayIndexer">
											<plx:CallObject>
												<plx:Value type="Local" data="factRoles"/>
											</plx:CallObject>
											<plx:PassParam>
												<plx:Value type="I4" data="0"/>
											</plx:PassParam>
										</plx:CallInstance>
									</xsl:otherwise>
								</xsl:choose>
							</plx:PassParam>
							<plx:PassParam>
								<!-- The matchAnyLeadRole param -->
								<xsl:choose>
									<xsl:when test="not($PatternGroup='SingleColumneExternalConstraint') and contains($ReadingChoice,'LeadReading') and not(contains($ReadingChoice,'PrimaryLeadReading'))">
										<plx:Value type="Local" data="includedRoles"/>
									</xsl:when>
									<xsl:otherwise>
										<plx:NullObjectKeyword/>
									</xsl:otherwise>
								</xsl:choose>
							</plx:PassParam>
							<plx:PassParam>
								<!-- The invertLeadRoles param -->
								<xsl:choose>
									<xsl:when test="contains($ReadingChoice,'Non')">
										<plx:TrueKeyword/>
									</xsl:when>
									<xsl:otherwise>
										<plx:FalseKeyword/>
									</xsl:otherwise>
								</xsl:choose>
							</plx:PassParam>
							<plx:PassParam>
								<!-- The noForwardText param -->
								<xsl:choose>
									<xsl:when test="contains($ReadingChoice,'NoForwardText')">
										<plx:TrueKeyword/>
									</xsl:when>
									<xsl:otherwise>
										<plx:FalseKeyword/>
									</xsl:otherwise>
								</xsl:choose>
							</plx:PassParam>
							<plx:PassParam>
								<!-- The defaultRoleOrder param -->
								<plx:Value type="Local" data="factRoles"/>
							</plx:PassParam>
							<plx:PassParam>
								<!-- The allowAnyOrder param -->
								<xsl:choose>
									<xsl:when test="not(starts-with($ReadingChoice,'Require'))">
										<plx:TrueKeyword/>
									</xsl:when>
									<xsl:otherwise>
										<plx:FalseKeyword/>
									</xsl:otherwise>
								</xsl:choose>
							</plx:PassParam>
						</plx:CallStatic>
					</plx:Right>
				</plx:Operator>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>