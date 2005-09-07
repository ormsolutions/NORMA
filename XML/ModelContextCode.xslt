<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:orm="http://Schemas.Neumont.edu/ORM/ORMCore"
	xmlns:ormRoot="http://Schemas.Neumont.edu/ORM/ORMRoot"
	xmlns:plx="http://Schemas.Neumont.edu/CodeGeneration/Plix"
	xmlns:ao="http://Schemas.Neumont.edu/ORM/SDK/ClassGenerator/AbsorbedObjects">


	<xsl:template name="GenerateImplementationConstructor">
		<xsl:param name="properties"/>
		<xsl:param name="ModelContextName"/>
		<plx:Function ctor="true" visibility="Public">
			<plx:Param type="In" name="context" dataTypeName="{$ModelContextName}"/>
			<xsl:variable name="mandatoryParameters">
				<xsl:call-template name="GenerateMandatoryParameters">
					<xsl:with-param name="properties" select="$properties"/>
				</xsl:call-template>
			</xsl:variable>
			<xsl:copy-of select="$mandatoryParameters"/>
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
					<plx:CallInstance name="Events" type="Field">
						<plx:CallObject>
							<plx:ThisKeyword/>
						</plx:CallObject>
					</plx:CallInstance>
				</plx:Left>
				<plx:Right>
					<plx:CallNew dataTypeName="EventHandlerList"/>
				</plx:Right>
			</plx:Operator>
			<xsl:for-each select="msxsl:node-set($mandatoryParameters)/child::*">
					<plx:Operator type="Assign">
						<plx:Left>
							<plx:CallInstance type="Property" name="{@name}">
								<plx:CallObject>
									<plx:ThisKeyword/>
								</plx:CallObject>
							</plx:CallInstance>
						</plx:Left>
						<plx:Right>
							<plx:Value type="Parameter" data="{@name}"/>
						</plx:Right>
					</plx:Operator>
			</xsl:for-each>
		</plx:Function>
	</xsl:template>
	<xsl:template name="GenerateFactoryMethod">
		<xsl:param name="properties"/>
		<xsl:param name="className"/>
		<plx:Function name="Create{$className}" visibility="Public">
			<plx:Param type="RetVal" name="" dataTypeName="{$className}"/>
			<xsl:variable name="mandatoryParameters">
				<xsl:call-template name="GenerateMandatoryParameters">
					<xsl:with-param name="properties" select="$properties"/>
				</xsl:call-template>
			</xsl:variable>
			<xsl:copy-of select="$mandatoryParameters"/>
			<plx:Return>
				<plx:CallNew dataTypeName="{$className}{$ImplementationClassSuffix}">
					<plx:PassParam>
						<plx:ThisKeyword/>
					</plx:PassParam>
					<xsl:for-each select="msxsl:node-set($mandatoryParameters)/child::*">
							<plx:PassParam>
								<plx:Value type="Parameter" data="{@name}"/>
							</plx:PassParam>
					</xsl:for-each>
				</plx:CallNew>
			</plx:Return>
		</plx:Function>
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
		<!--<xsl:variable name="AbsorbedMandatory" select="$Model/orm:ORMModel/orm:Facts/orm:Fact/orm:FactRoles/orm:Role[@IsMandatory='true']/@DataType"/>-->
		<xsl:for-each select="$properties">
			<xsl:call-template name="GenerateImplementationSimpleLookupMethod">
				<xsl:with-param name="className" select="$className"/>
			</xsl:call-template>
			<xsl:call-template name="GenerateImplementationPropertyChangeMethods">
				<xsl:with-param name="className" select="$className"/>
				<xsl:with-param name="Model" select="$Model"/>
			</xsl:call-template>
		</xsl:for-each>
		<xsl:call-template name="GenerateFactoryMethod">
			<xsl:with-param name="properties" select="$properties"/>
			<xsl:with-param name="className" select="$className"/>
		</xsl:call-template>
		<plx:Class visibility="Private" sealed="true" name="{$implementationClassName}">
			<plx:DerivesFromClass dataTypeName="{$className}"/>
			<!-- PLIX_TODO: Plix currently seems to be ignoring the readOnly attribute on Field elements... -->
			<plx:Field visibility="Private" readOnly="true" name="Events" dataTypeName="EventHandlerList"/>
			<xsl:call-template name="GenerateImplementationConstructor">
				<xsl:with-param name="properties" select="$properties"/>
				<xsl:with-param name="ModelContextName" select="$ModelContextName"/>
			</xsl:call-template>
			<xsl:call-template name="GenerateINotifyPropertyChangedImplementation">
				<xsl:with-param name="implementationClassName" select="$implementationClassName"/>
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
				<xsl:call-template name="GenerateImplementationPropertyChangeEvents">
					<xsl:with-param name="implementationClassName" select="$implementationClassName"/>
				</xsl:call-template>
				<xsl:call-template name="GenerateImplementationProperty">
					<xsl:with-param name="className" select="$className"/>
				</xsl:call-template>
			</xsl:for-each>
		</plx:Class>
	</xsl:template>
	<xsl:template name="GenerateImplementationProperty">
		<xsl:param name="className"/>
		<xsl:param name="initializeField" select="false()"/>
		<plx:Field name="{$PrivateMemberPrefix}{@name}" visibility="Private" readOnly="{@readOnly='true'}">
			<xsl:copy-of select="DataType/@*"/>
			<xsl:copy-of select="DataType/child::*"/>
			<xsl:if test="$initializeField">
				<plx:Initialize>
					<plx:DefaultValueOf>
						<xsl:copy-of select="DataType/@*"/>
						<xsl:copy-of select="DataType/child::*"/>
					</plx:DefaultValueOf>
				</plx:Initialize>
			</xsl:if>
		</plx:Field>
		<!-- Get and Set Properties for the given Object-->
		<plx:Property name="{@name}" visibility="Public" override="true">
			<plx:Param type="RetVal" name="">
				<xsl:copy-of select="DataType/@*"/>
				<xsl:copy-of select="DataType/child::*"/>
			</plx:Param>
			<!-- Get -->
			<plx:Get>
				<plx:Return>
					<plx:CallInstance name="{$PrivateMemberPrefix}{@name}" type="Field">
						<plx:CallObject>
							<plx:ThisKeyword/>
						</plx:CallObject>
					</plx:CallInstance>
				</plx:Return>
			</plx:Get>
			<!-- Set -->
			<xsl:if test="not(@readOnly='true')">
				<plx:Set>
					<xsl:if test="@customType='true'">
						<plx:Condition>
							<plx:Test>
								<plx:Operator type="IdentityInequality">
									<plx:Left>
										<plx:CallInstance name="Context" type="Property">
											<plx:CallObject>
												<plx:ThisKeyword/>
											</plx:CallObject>
										</plx:CallInstance>
									</plx:Left>
									<plx:Right>
										<plx:CallInstance name="Context" type="Property">
											<plx:CallObject>
												<plx:ValueKeyword/>
											</plx:CallObject>
										</plx:CallInstance>
									</plx:Right>
								</plx:Operator>
							</plx:Test>
							<plx:Body>
								<plx:Throw>
									<plx:CallNew dataTypeName="ArgumentException">
										<plx:PassParam>
											<plx:String>All objects in a relationship must be part of the same Context.</plx:String>
										</plx:PassParam>
										<plx:PassParam>
											<plx:String>value</plx:String>
										</plx:PassParam>
									</plx:CallNew>
								</plx:Throw>
							</plx:Body>
						</plx:Condition>
					</xsl:if>
					<plx:Condition>
						<plx:Test>
							<plx:Operator type="BooleanNot">
								<plx:CallStatic name="Equals" type="MethodCall" dataTypeName="Object" dataTypeQualifier="System">
									<plx:PassParam>
										<plx:CallInstance name="{@name}" type="Property">
											<plx:CallObject>
												<plx:ThisKeyword/>
											</plx:CallObject>
										</plx:CallInstance>
									</plx:PassParam>
									<plx:PassParam>
										<plx:ValueKeyword/>
									</plx:PassParam>
								</plx:CallStatic>
							</plx:Operator>
						</plx:Test>
						<plx:Body>
							<!-- Notify the ModelContext that we're changing the value of a property. -->
							<plx:Condition>
								<plx:Test>
									<plx:CallInstance name="On{$className}{@name}Changing" type="MethodCall">
										<plx:CallObject>
											<plx:CallInstance name="Context" type="Property">
												<plx:CallObject>
													<plx:ThisKeyword />
												</plx:CallObject>
											</plx:CallInstance>
										</plx:CallObject>
										<plx:PassParam>
											<plx:ThisKeyword />
										</plx:PassParam>
										<plx:PassParam>
											<plx:ValueKeyword />
										</plx:PassParam>
									</plx:CallInstance>
								</plx:Test>
								<plx:Body>
									<plx:Condition>
										<plx:Test>
											<plx:CallInstance name="Raise{@name}ChangingEvent" type="MethodCall">
												<plx:CallObject>
													<plx:ThisKeyword/>
												</plx:CallObject>
												<plx:PassParam>
													<plx:ValueKeyword/>
												</plx:PassParam>
											</plx:CallInstance>
										</plx:Test>
										<plx:Body>
											<plx:Variable name="oldValue">
												<xsl:copy-of select="DataType/@*"/>
												<xsl:copy-of select="DataType/child::*"/>
												<plx:Initialize>
													<plx:CallInstance name="{@name}" type="Property">
														<plx:CallObject>
															<plx:ThisKeyword />
														</plx:CallObject>
													</plx:CallInstance>
												</plx:Initialize>
											</plx:Variable>
											<plx:Operator type="Assign">
												<plx:Left>
													<plx:CallInstance name="{$PrivateMemberPrefix}{@name}" type="Field">
														<plx:CallObject>
															<plx:ThisKeyword/>
														</plx:CallObject>
													</plx:CallInstance>
												</plx:Left>
												<plx:Right>
													<plx:ValueKeyword/>
												</plx:Right>
											</plx:Operator>
											<plx:CallInstance name="On{$className}{@name}Changed">
												<plx:CallObject>
													<plx:CallInstance name="Context" type="Property">
														<plx:CallObject>
															<plx:ThisKeyword />
														</plx:CallObject>
													</plx:CallInstance>
												</plx:CallObject>
												<plx:PassParam>
													<plx:ThisKeyword />
												</plx:PassParam>
												<plx:PassParam>
													<plx:Value type="Local" data="oldValue"/>
												</plx:PassParam>
											</plx:CallInstance>
											<plx:CallInstance name="Raise{@name}ChangedEvent" type="MethodCall">
												<plx:CallObject>
													<plx:ThisKeyword/>
												</plx:CallObject>
												<plx:PassParam>
													<plx:Value type="Local" data="oldValue"/>
												</plx:PassParam>
											</plx:CallInstance>
										</plx:Body>
									</plx:Condition>
								</plx:Body>
							</plx:Condition>
						</plx:Body>
					</plx:Condition>
				</plx:Set>
			</xsl:if>
		</plx:Property>
	</xsl:template>
	<xsl:template name="GenerateImplementationPropertyChangeMethods">
		<xsl:param name="className"/>
		<xsl:param name="Model"/>
		<xsl:if test="not(@readOnly='true')">
			<xsl:variable name="realRoleRef" select="@realRoleRef"/>
			<xsl:variable name="externalUniquenessConstraints" select="$Model/orm:ExternalConstraints/orm:ExternalUniquenessConstraint[orm:RoleSequence/orm:Role/@ref=$realRoleRef]"/>
			<xsl:variable name="associationUniquenessConstraintsFragment">
				<xsl:for-each select="$AbsorbedObjects/../ao:Association">
					<xsl:copy-of select="$Model/orm:Facts/orm:Fact[@id=current()/@id]/orm:InternalConstraints/orm:InternalUniquenessConstraint[orm:RoleSequence/orm:Role/@ref=$realRoleRef]"/>
				</xsl:for-each>
			</xsl:variable>
			<xsl:variable name="associationUniquenessConstraints" select="msxsl:node-set($associationUniquenessConstraintsFragment)/child::*"/>
			<xsl:if test="@unique='true' and not(@customType='true')">
				<plx:Field name="{$PrivateMemberPrefix}{$className}{@name}Dictionary" visibility="Private" dataTypeName="Dictionary">
					<plx:PassTypeParam>
						<xsl:copy-of select="DataType/@*"/>
						<xsl:copy-of select="DataType/child::*"/>
					</plx:PassTypeParam>
					<plx:PassTypeParam dataTypeName="{$className}"/>
					<plx:Initialize>
						<plx:CallNew type="New" dataTypeName="Dictionary">
							<plx:PassTypeParam>
								<xsl:copy-of select="DataType/@*"/>
								<xsl:copy-of select="DataType/child::*"/>
							</plx:PassTypeParam>
							<plx:PassTypeParam dataTypeName="{$className}"/>
						</plx:CallNew>
					</plx:Initialize>
				</plx:Field>
			</xsl:if>
			<plx:Function visibility="Private" name="On{$className}{@name}Changing">
				<plx:Param type="RetVal" name="" dataTypeName="Boolean" dataTypeQualifier="System"/>
				<plx:Param type="In" name="instance" dataTypeName="{$className}"/>
				<plx:Param type="In" name="newValue">
					<xsl:copy-of select="DataType/@*"/>
					<xsl:copy-of select="DataType/child::*"/>
				</plx:Param>
				<xsl:if test="@unique='true' and not(@customType='true')">
					<plx:Condition>
						<plx:Test>
							<plx:Operator type="Inequality">
								<plx:Left>
									<plx:Value type="Parameter" data="newValue"/>
								</plx:Left>
								<plx:Right>
									<plx:NullObjectKeyword />
								</plx:Right>
							</plx:Operator>
						</plx:Test>
						<plx:Body>
							<plx:Variable name="currentInstance" dataTypeName="{$className}">
								<plx:Initialize>
									<plx:Value type="Parameter" data="instance"/>
								</plx:Initialize>
							</plx:Variable>
							<plx:Condition>
								<plx:Test>
									<plx:CallInstance name="TryGetValue" type="MethodCall">
										<plx:CallObject>
											<plx:CallInstance name="{$PrivateMemberPrefix}{$className}{@name}Dictionary" type="Field">
												<plx:CallObject>
													<plx:ThisKeyword />
												</plx:CallObject>
											</plx:CallInstance>
										</plx:CallObject>
										<plx:PassParam>
											<plx:Value type="Parameter" data="newValue"/>
										</plx:PassParam>
										<plx:PassParam passStyle="Out">
											<plx:Value type="Local" data="currentInstance"/>
										</plx:PassParam>
									</plx:CallInstance>
								</plx:Test>
								<plx:Body>
									<plx:Condition>
										<plx:Test>
											<plx:Operator type="BooleanNot">
												<plx:CallStatic name="Equals" type="MethodCall" dataTypeName="Object" dataTypeQualifier="System">
													<plx:PassParam>
														<plx:Value type="Local" data="currentInstance"/>
													</plx:PassParam>
													<plx:PassParam>
														<plx:Value type="Parameter" data="instance"/>
													</plx:PassParam>
												</plx:CallStatic>
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
						</plx:Body>
					</plx:Condition>
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
				<plx:Return>
					<plx:TrueKeyword />
				</plx:Return>
			</plx:Function>
			<plx:Function visibility="Private" name="On{$className}{@name}Changed">
				<plx:Param type="In" name="instance" dataTypeName="{$className}"/>
				<plx:Param type="In" name="oldValue">
					<xsl:copy-of select="DataType/@*"/>
					<xsl:copy-of select="DataType/child::*"/>
				</plx:Param>
				<xsl:if test="@unique='true'">
					<plx:Condition>
						<plx:Test>
							<plx:Operator type="Inequality">
								<plx:Left>
									<plx:Value type="Parameter" data="oldValue"/>
								</plx:Left>
								<plx:Right>
									<plx:NullObjectKeyword />
								</plx:Right>
							</plx:Operator>
						</plx:Test>
						<plx:Body>
							<xsl:choose>
								<xsl:when test="@customType='true'">
									<plx:Operator type="Assign">
										<plx:Left>
											<plx:CallInstance name="{@oppositeName}" type="Property">
												<plx:CallObject>
													<plx:Value type="Parameter" data="oldValue"/>
												</plx:CallObject>
											</plx:CallInstance>
										</plx:Left>
										<plx:Right>
											<plx:NullObjectKeyword/>
										</plx:Right>
									</plx:Operator>
								</xsl:when>
								<xsl:otherwise>
									<plx:CallInstance name="Remove">
										<plx:CallObject>
											<plx:CallInstance name="{$PrivateMemberPrefix}{$className}{@name}Dictionary" type="Field">
												<plx:CallObject>
													<plx:ThisKeyword />
												</plx:CallObject>
											</plx:CallInstance>
										</plx:CallObject>
										<plx:PassParam>
											<plx:Value type="Parameter" data="oldValue"/>
										</plx:PassParam>
									</plx:CallInstance>
								</xsl:otherwise>
							</xsl:choose>
						</plx:Body>
					</plx:Condition>
					<plx:Condition>
						<plx:Test>
							<plx:Operator type="Inequality">
								<plx:Left>
									<plx:CallInstance name="{@name}" type="Property">
										<plx:CallObject>
											<plx:Value type="Parameter" data="instance"/>
										</plx:CallObject>
									</plx:CallInstance>
								</plx:Left>
								<plx:Right>
									<plx:NullObjectKeyword />
								</plx:Right>
							</plx:Operator>
						</plx:Test>
						<plx:Body>
							<xsl:choose>
								<xsl:when test="@customType='true'">
									<plx:Operator type="Assign">
										<plx:Left>
											<plx:CallInstance name="{@oppositeName}" type="Property">
												<plx:CallObject>
													<plx:CallInstance name="{@name}" type="Property">
														<plx:CallObject>
															<plx:Value type="Parameter" data="instance"/>
														</plx:CallObject>
													</plx:CallInstance>
												</plx:CallObject>
											</plx:CallInstance>
										</plx:Left>
										<plx:Right>
											<plx:Value type="Parameter" data="instance"/>
										</plx:Right>
									</plx:Operator>
								</xsl:when>
								<xsl:otherwise>
									<plx:CallInstance name="Add">
										<plx:CallObject>
											<plx:CallInstance name="{$PrivateMemberPrefix}{$className}{@name}Dictionary" type="Field">
												<plx:CallObject>
													<plx:ThisKeyword />
												</plx:CallObject>
											</plx:CallInstance>
										</plx:CallObject>
										<plx:PassParam>
											<plx:CallInstance name="{@name}" type="Property">
												<plx:CallObject>
													<plx:Value type="Parameter" data="instance"/>
												</plx:CallObject>
											</plx:CallInstance>
										</plx:PassParam>
										<plx:PassParam>
											<plx:Value type="Parameter" data="instance"/>
										</plx:PassParam>
									</plx:CallInstance>
								</xsl:otherwise>
							</xsl:choose>
						</plx:Body>
					</plx:Condition>
				</xsl:if>
				<xsl:if test="count($externalUniquenessConstraints)">
					<xsl:for-each select="$externalUniquenessConstraints">
						<xsl:call-template name="GetSimpleBinaryUniquenessPropertyChangedCode">
							<xsl:with-param name="Model" select="$Model"/>
							<xsl:with-param name="realRoleRef" select="$realRoleRef"/>
						</xsl:call-template>
					</xsl:for-each>
				</xsl:if>
				<xsl:if test="count($associationUniquenessConstraints)">
					<xsl:for-each select="$associationUniquenessConstraints">
						<xsl:call-template name="GetSimpleBinaryUniquenessPropertyChangedCode">
							<xsl:with-param name="Model" select="$Model"/>
							<xsl:with-param name="realRoleRef" select="$realRoleRef"/>
						</xsl:call-template>
					</xsl:for-each>
				</xsl:if>
			</plx:Function>
		</xsl:if>
	</xsl:template>
	<xsl:template name="GenerateImplementationPropertyChangeEvents">
		<xsl:param name="implementationClassName"/>
		<xsl:if test="not(@readOnly='true')">
			<xsl:call-template name="GenerateImplementationPropertyChangeEvent">
				<xsl:with-param name="implementationClassName" select="$implementationClassName"/>
				<xsl:with-param name="changeType" select="'Changing'"/>
			</xsl:call-template>
			<xsl:call-template name="GenerateImplementationPropertyChangeEventRaiseMethod">
				<xsl:with-param name="implementationClassName" select="$implementationClassName"/>
				<xsl:with-param name="changeType" select="'Changing'"/>
			</xsl:call-template>
			<xsl:call-template name="GenerateImplementationPropertyChangeEvent">
				<xsl:with-param name="implementationClassName" select="$implementationClassName"/>
				<xsl:with-param name="changeType" select="'Changed'"/>
			</xsl:call-template>
			<xsl:call-template name="GenerateImplementationPropertyChangeEventRaiseMethod">
				<xsl:with-param name="implementationClassName" select="$implementationClassName"/>
				<xsl:with-param name="changeType" select="'Changed'"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
	<xsl:template name="GenerateImplementationPropertyChangeEvent">
		<xsl:param name="implementationClassName"/>
		<xsl:param name="changeType"/>
		<plx:Field visibility="Private" static="true" readOnly="true" name="Event{@name}{$changeType}" dataTypeName="Object" dataTypeQualifier="System">
			<plx:Initialize>
				<plx:CallNew dataTypeName="Object" dataTypeQualifier="System"/>
			</plx:Initialize>
		</plx:Field>
		<!-- PLIX_TODO: Plix currently seems to be ignoring the override attribute on Event elements... -->
		<plx:Event visibility="Public" override="true" name="{@name}{$changeType}">
			<xsl:choose>
				<xsl:when test="@name='Property'">
					<plx:DelegateType dataTypeName="PropertyChangedEventHandler"/>
				</xsl:when>
				<xsl:otherwise>
					<!-- PLIX_TODO: Plix currently seems to be ignoring PassTypeParam elements inside of DelegateType elements... -->
					<plx:DelegateType dataTypeName="EventHandler">
						<plx:PassTypeParam dataTypeName="Property{$changeType}EventArgs">
							<plx:PassTypeParam>
								<xsl:copy-of select="DataType/@*"/>
								<xsl:copy-of select="DataType/child::*"/>
							</plx:PassTypeParam>
						</plx:PassTypeParam>
					</plx:DelegateType>
				</xsl:otherwise>
			</xsl:choose>
			<!-- PLIX_TODO: Plix (actually, CodeDom) currently doesn't like Event elements that contain OnAdd and OnRemove elements -->
			<!--<plx:OnAdd>
				<plx:CallInstance name="AddHandler" type="MethodCall">
					<plx:CallObject>
						<plx:CallInstance name="Events" type="Field">
							<plx:CallObject>
								<plx:ThisKeyword/>
							</plx:CallObject>
						</plx:CallInstance>
					</plx:CallObject>
					<plx:PassParam>
						<plx:CallStatic name="Event{@name}{$changeType}" type="Field" dataTypeName="{$implementationClassName}"/>
					</plx:PassParam>
					<plx:PassParam>
						<plx:ValueKeyword/>
					</plx:PassParam>
				</plx:CallInstance>
			</plx:OnAdd>
			<plx:OnRemove>
				<plx:CallInstance name="RemoveHandler" type="MethodCall">
					<plx:CallObject>
						<plx:CallInstance name="Events" type="Field">
							<plx:CallObject>
								<plx:ThisKeyword/>
							</plx:CallObject>
						</plx:CallInstance>
					</plx:CallObject>
					<plx:PassParam>
						<plx:CallStatic name="Event{@name}{$changeType}" type="Field" dataTypeName="{$implementationClassName}"/>
					</plx:PassParam>
					<plx:PassParam>
						<plx:ValueKeyword/>
					</plx:PassParam>
				</plx:CallInstance>
			</plx:OnRemove>-->
		</plx:Event>
	</xsl:template>
	<xsl:template name="GenerateImplementationPropertyChangeEventRaiseMethod">
		<xsl:param name="implementationClassName"/>
		<xsl:param name="changeType"/>
		<xsl:variable name="changing" select="$changeType='Changing'"/>
		<xsl:variable name="changed" select="$changeType='Changed'"/>
		<plx:Function visibility="Private" name="Raise{@name}{$changeType}Event">
			<xsl:choose>
				<xsl:when test="$changing">
					<plx:Param type="RetVal" name="" dataTypeName="Boolean" dataTypeQualifier="System"/>
					<plx:Param type="In" name="newValue">
						<xsl:copy-of select="DataType/@*"/>
						<xsl:copy-of select="DataType/child::*"/>
					</plx:Param>
				</xsl:when>
				<xsl:when test="$changed">
					<plx:Param type="In" name="oldValue">
						<xsl:copy-of select="DataType/@*"/>
						<xsl:copy-of select="DataType/child::*"/>
					</plx:Param>
				</xsl:when>
			</xsl:choose>
			<plx:Variable name="eventHandler" dataTypeName="EventHandler">
				<plx:PassTypeParam dataTypeName="Property{$changeType}EventArgs">
					<plx:PassTypeParam>
						<xsl:copy-of select="DataType/@*"/>
						<xsl:copy-of select="DataType/child::*"/>
					</plx:PassTypeParam>
				</plx:PassTypeParam>
				<plx:Initialize>
					<plx:Cast type="TypeCastTest">
						<plx:TargetType dataTypeName="EventHandler">
							<plx:PassTypeParam dataTypeName="Property{$changeType}EventArgs">
								<plx:PassTypeParam>
									<xsl:copy-of select="DataType/@*"/>
									<xsl:copy-of select="DataType/child::*"/>
								</plx:PassTypeParam>
							</plx:PassTypeParam>
						</plx:TargetType>
						<plx:CastExpression>
							<plx:CallInstance name="Item" type="Indexer">
								<plx:CallObject>
									<plx:CallInstance name="Events" type="Field">
										<plx:CallObject>
											<plx:ThisKeyword/>
										</plx:CallObject>
									</plx:CallInstance>
								</plx:CallObject>
								<plx:PassParam>
									<plx:CallStatic name="Event{@name}{$changeType}" type="Field" dataTypeName="{$implementationClassName}"/>
								</plx:PassParam>
							</plx:CallInstance>
						</plx:CastExpression>
					</plx:Cast>
				</plx:Initialize>
			</plx:Variable>
			<plx:Condition>
				<plx:Test>
					<plx:Operator type="Inequality">
						<plx:Left>
							<plx:Value type="Local" data="eventHandler"/>
						</plx:Left>
						<plx:Right>
							<plx:NullObjectKeyword/>
						</plx:Right>
					</plx:Operator>
				</plx:Test>
				<plx:Body>
					<xsl:variable name="createNewEventArgs">
						<xsl:variable name="currentValue">
							<plx:CallInstance name="{@name}" type="Property">
								<plx:CallObject>
									<plx:ThisKeyword/>
								</plx:CallObject>
							</plx:CallInstance>
						</xsl:variable>
						<plx:CallNew dataTypeName="Property{$changeType}EventArgs">
							<plx:PassTypeParam>
								<xsl:copy-of select="DataType/@*"/>
								<xsl:copy-of select="DataType/child::*"/>
							</plx:PassTypeParam>
							<plx:PassParam>
								<xsl:choose>
									<xsl:when test="$changing">
										<xsl:copy-of select="$currentValue"/>
									</xsl:when>
									<xsl:when test="$changed">
										<plx:Value type="Local" data="oldValue"/>
									</xsl:when>
								</xsl:choose>
							</plx:PassParam>
							<plx:PassParam>
								<xsl:choose>
									<xsl:when test="$changing">
										<plx:Value type="Local" data="newValue"/>
									</xsl:when>
									<xsl:when test="$changed">
										<xsl:copy-of select="$currentValue"/>
									</xsl:when>
								</xsl:choose>
							</plx:PassParam>
						</plx:CallNew>
					</xsl:variable>
					<xsl:if test="$changing">
						<plx:Variable name="eventArgs" dataTypeName="PropertyChangingEventArgs">
							<plx:PassTypeParam>
								<xsl:copy-of select="DataType/@*"/>
								<xsl:copy-of select="DataType/child::*"/>
							</plx:PassTypeParam>
							<plx:Initialize>
								<xsl:copy-of select="$createNewEventArgs"/>
							</plx:Initialize>
						</plx:Variable>
					</xsl:if>
					<xsl:variable name="eventArgs">
						<xsl:choose>
							<xsl:when test="$changing">
								<plx:Value type="Local" data="eventArgs"/>
							</xsl:when>
							<xsl:when test="$changed">
								<xsl:copy-of select="$createNewEventArgs"/>
							</xsl:when>
						</xsl:choose>
					</xsl:variable>
					<plx:CallInstance name="" type="DelegateCall">
						<plx:CallObject>
							<plx:Value type="Local" data="eventHandler"/>
						</plx:CallObject>
						<plx:PassParam>
							<plx:ThisKeyword/>
						</plx:PassParam>
						<plx:PassParam>
							<xsl:copy-of select="$eventArgs"/>
						</plx:PassParam>
					</plx:CallInstance>
					<xsl:choose>
						<xsl:when test="$changing">
							<plx:Return>
								<plx:Operator type="BooleanNot">
									<plx:CallInstance name="Cancel" type="Property">
										<plx:CallObject>
											<plx:Value type="Local" data="eventArgs"/>
										</plx:CallObject>
									</plx:CallInstance>
								</plx:Operator>
							</plx:Return>
						</xsl:when>
						<xsl:when test="$changed">
							<plx:CallInstance name="RaisePropertyChangedEvent" type="MethodCall">
								<plx:CallObject>
									<plx:ThisKeyword/>
								</plx:CallObject>
								<plx:PassParam>
									<plx:String>
										<xsl:value-of select="@name"/>
									</plx:String>
								</plx:PassParam>
							</plx:CallInstance>
						</xsl:when>
					</xsl:choose>
				</plx:Body>
			</plx:Condition>
			<xsl:if test="$changing">
				<plx:Return>
					<plx:TrueKeyword/>
				</plx:Return>
			</xsl:if>
		</plx:Function>
	</xsl:template>
	<xsl:template name="GenerateImplementationSimpleLookupMethod">
		<xsl:param name="className"/>
		<xsl:if test="@unique='true' and not(@customType='true')">
			<plx:Function name="Get{$className}By{@name}" visibility="Public">
				<plx:Param type="RetVal" name="" dataTypeName="{$className}"/>
				<plx:Param type="In" name="value">
					<xsl:copy-of select="DataType/@*"/>
					<xsl:copy-of select="DataType/child::*"/>
				</plx:Param>
				<plx:Return>
					<plx:CallInstance type="Indexer" name="Item">
						<plx:CallObject>
							<plx:CallInstance type="Field" name="{$PrivateMemberPrefix}{$className}{@name}Dictionary">
								<plx:CallObject>
									<plx:ThisKeyword/>
								</plx:CallObject>
							</plx:CallInstance>
						</plx:CallObject>
						<plx:PassParam>
							<plx:Value type="Parameter" data="value"/>
						</plx:PassParam>
					</plx:CallInstance>
				</plx:Return>
			</plx:Function>
		</xsl:if>
	</xsl:template>

	<xsl:template name="GenerateINotifyPropertyChangedImplementation">
		<xsl:param name="implementationClassName"/>
		<xsl:variable name="propertyChangedProperty">
			<Property name="Property">
				<DataType dateTypeName="String" dataTypeQualifier="System"/>
			</Property>
		</xsl:variable>
		<xsl:for-each select="msxsl:node-set($propertyChangedProperty)/child::*">
			<xsl:call-template name="GenerateImplementationPropertyChangeEvent">
				<xsl:with-param name="implementationClassName" select="$implementationClassName"/>
				<xsl:with-param name="changeType" select="'Changed'"/>
			</xsl:call-template>
		</xsl:for-each>
		<plx:Function visibility="Private" name="RaisePropertyChangedEvent">
			<plx:Param type="In" name="propertyName" dataTypeName="String" dataTypeQualifier="System"/>
			<plx:Variable name="eventHandler" dataTypeName="PropertyChangedEventHandler">
				<plx:Initialize>
					<plx:Cast type="TypeCastTest">
						<plx:TargetType dataTypeName="PropertyChangedEventHandler"/>
						<plx:CastExpression>
							<plx:CallInstance name="Item" type="Indexer">
								<plx:CallObject>
									<plx:CallInstance name="Events" type="Field">
										<plx:CallObject>
											<plx:ThisKeyword/>
										</plx:CallObject>
									</plx:CallInstance>
								</plx:CallObject>
								<plx:PassParam>
									<plx:CallStatic name="EventPropertyChanged" type="Field" dataTypeName="{$implementationClassName}"/>
								</plx:PassParam>
							</plx:CallInstance>
						</plx:CastExpression>
					</plx:Cast>
				</plx:Initialize>
			</plx:Variable>
			<plx:Condition>
				<plx:Test>
					<plx:Operator type="Inequality">
						<plx:Left>
							<plx:Value type="Local" data="eventHandler"/>
						</plx:Left>
						<plx:Right>
							<plx:NullObjectKeyword/>
						</plx:Right>
					</plx:Operator>
				</plx:Test>
				<plx:Body>
					<plx:CallInstance name="" type="DelegateCall">
						<plx:CallObject>
							<plx:Value type="Local" data="eventHandler"/>
						</plx:CallObject>
						<plx:PassParam>
							<plx:ThisKeyword/>
						</plx:PassParam>
						<plx:PassParam>
							<plx:CallNew dataTypeName="PropertyChangedEventArgs">
								<plx:PassParam>
									<plx:Value type="Parameter" data="propertyName"/>
								</plx:PassParam>
							</plx:CallNew>
						</plx:PassParam>
					</plx:CallInstance>
				</plx:Body>
			</plx:Condition>
		</plx:Function>
	</xsl:template>

	<xsl:template name="GenerateSimpleBinaryUniquenessChangeMethods">
		<xsl:param name="Model"/>
		<xsl:param name="uniqueObjectName"/>
		<xsl:param name="parameters"/>
		<xsl:variable name="passTypeParams">
			<xsl:for-each select="$parameters">
				<plx:PassTypeParam>
					<xsl:copy-of select="DataType/@*"/>
					<xsl:copy-of select="DataType/child::*"/>
				</plx:PassTypeParam>
			</xsl:for-each>
		</xsl:variable>
		<plx:Field name="{$PrivateMemberPrefix}{@Name}Dictionary" dataTypeName="Dictionary" visibility="Private">
			<plx:PassTypeParam dataTypeName="Tuple">
				<xsl:copy-of select="$passTypeParams"/>
			</plx:PassTypeParam>
			<plx:PassTypeParam dataTypeName="{$uniqueObjectName}"/>
			<plx:Initialize>
				<plx:CallNew type="New" dataTypeName="Dictionary">
					<plx:PassTypeParam dataTypeName="Tuple">
						<xsl:copy-of select="$passTypeParams"/>
					</plx:PassTypeParam>
					<plx:PassTypeParam dataTypeName="{$uniqueObjectName}"/>
				</plx:CallNew>
			</plx:Initialize>
		</plx:Field>
		<plx:Function visibility="Private" name="On{@Name}Changing">
			<plx:Param name="" dataTypeName="Boolean" dataTypeQualifier="System" type="RetVal" />
			<plx:Param name="instance" dataTypeName="{$uniqueObjectName}" type="In" />
			<plx:Param name="newValue" dataTypeName="Tuple" type="In">
				<xsl:copy-of select="$passTypeParams"/>
			</plx:Param>
			<plx:Condition>
				<plx:Test>
					<plx:Operator type="Inequality">
						<plx:Left>
							<plx:Value type="Parameter" data="newValue"/>
						</plx:Left>
						<plx:Right>
							<plx:NullObjectKeyword/>
						</plx:Right>
					</plx:Operator>
				</plx:Test>
				<plx:Body>
					<plx:Variable name="currentInstance" dataTypeName="{$uniqueObjectName}">
						<plx:Initialize>
							<plx:Value type="Parameter" data="instance"/>
						</plx:Initialize>
					</plx:Variable>
					<plx:Condition>
						<plx:Test>
							<plx:CallInstance name="TryGetValue" type="MethodCall">
								<plx:CallObject>
									<plx:CallInstance name="{$PrivateMemberPrefix}{@Name}Dictionary" type="Field">
										<plx:CallObject>
											<plx:ThisKeyword />
										</plx:CallObject>
									</plx:CallInstance>
								</plx:CallObject>
								<plx:PassParam>
									<plx:Value type="Parameter" data="newValue"/>
								</plx:PassParam>
								<plx:PassParam passStyle="Out">
									<plx:Value type="Local" data="currentInstance"/>
								</plx:PassParam>
							</plx:CallInstance>
						</plx:Test>
						<plx:Body>
							<plx:Return>
								<plx:Operator type="Equality">
									<plx:Left>
										<plx:Value type="Local" data="currentInstance"/>
									</plx:Left>
									<plx:Right>
										<plx:Value type="Parameter" data="instance"/>
									</plx:Right>
								</plx:Operator>
							</plx:Return>
						</plx:Body>
					</plx:Condition>
				</plx:Body>
			</plx:Condition>
			<plx:Return>
				<plx:TrueKeyword/>
			</plx:Return>
		</plx:Function>
		<plx:Function visibility="Private" name="On{@Name}Changed">
			<plx:Param name="instance" dataTypeName="{$uniqueObjectName}" type="In" />
			<plx:Param name="oldValue" dataTypeName="Tuple" type="In">
				<xsl:copy-of select="$passTypeParams"/>
			</plx:Param>
			<plx:Param name="newValue" dataTypeName="Tuple" type="In">
				<xsl:copy-of select="$passTypeParams"/>
			</plx:Param>
			<plx:Condition>
				<plx:Test>
					<plx:Operator type="Inequality">
						<plx:Left>
							<plx:Value type="Parameter" data="oldValue"/>
						</plx:Left>
						<plx:Right>
							<plx:NullObjectKeyword/>
						</plx:Right>
					</plx:Operator>
				</plx:Test>
				<plx:Body>
					<plx:CallInstance name="Remove" type="MethodCall">
						<plx:CallObject>
							<plx:CallInstance name="{$PrivateMemberPrefix}{@Name}Dictionary" type="Field">
								<plx:CallObject>
									<plx:ThisKeyword/>
								</plx:CallObject>
							</plx:CallInstance>
						</plx:CallObject>
						<plx:PassParam>
							<plx:Value type="Parameter" data="oldValue"/>
						</plx:PassParam>
					</plx:CallInstance>
				</plx:Body>
			</plx:Condition>
			<plx:Condition>
				<plx:Test>
					<plx:Operator type="Inequality">
						<plx:Left>
							<plx:Value type="Parameter" data="newValue"/>
						</plx:Left>
						<plx:Right>
							<plx:NullObjectKeyword/>
						</plx:Right>
					</plx:Operator>
				</plx:Test>
				<plx:Body>
					<plx:CallInstance name="Add" type="MethodCall">
						<plx:CallObject>
							<plx:CallInstance name="{$PrivateMemberPrefix}{@Name}Dictionary" type="Field">
								<plx:CallObject>
									<plx:ThisKeyword/>
								</plx:CallObject>
							</plx:CallInstance>
						</plx:CallObject>
						<plx:PassParam>
							<plx:Value type="Parameter" data="newValue"/>
						</plx:PassParam>
						<plx:PassParam>
							<plx:Value type="Parameter" data="instance"/>
						</plx:PassParam>
					</plx:CallInstance>
				</plx:Body>
			</plx:Condition>
		</plx:Function>
	</xsl:template>
	<xsl:template name="GenerateSimpleBinaryUniquenessLookupMethod">
		<xsl:param name="uniqueObjectName"/>
		<xsl:param name="parameters"/>
		<plx:Function visibility="Public" name="Get{$uniqueObjectName}By{@Name}">
			<plx:Param type="RetVal" name="" dataTypeName="{$uniqueObjectName}"/>
			<xsl:for-each select="$parameters">
				<plx:Param type="In" name="{@name}">
					<xsl:copy-of select="DataType/@*"/>
					<xsl:copy-of select="DataType/child::*"/>
				</plx:Param>
			</xsl:for-each>
			<plx:Return>
				<plx:CallInstance type="Indexer" name="Item">
					<plx:CallObject>
						<plx:CallInstance type="Field" name="{$PrivateMemberPrefix}{@Name}Dictionary">
							<plx:CallObject>
								<plx:ThisKeyword/>
							</plx:CallObject>
						</plx:CallInstance>
					</plx:CallObject>
					<plx:PassParam>
						<plx:CallStatic type="MethodCall" name="CreateTuple" dataTypeName="Tuple">
							<xsl:for-each select="$parameters">
								<plx:PassParam>
									<plx:Value type="Parameter" data="{@name}"/>
								</plx:PassParam>
							</xsl:for-each>
						</plx:CallStatic>
					</plx:PassParam>
				</plx:CallInstance>
			</plx:Return>
		</plx:Function>
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
		<xsl:variable name="passTypeParams">
			<xsl:for-each select="$parameters">
				<plx:PassTypeParam>
					<xsl:copy-of select="DataType/@*"/>
					<xsl:copy-of select="DataType/child::*"/>
				</plx:PassTypeParam>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="passParams">
			<xsl:for-each select="$parameters">
				<plx:PassParam>
					<xsl:choose>
						<xsl:when test="@special='true'">
							<plx:Value type="Parameter" data="newValue" />
						</xsl:when>
						<xsl:otherwise>
							<plx:CallInstance name="{@name}" type="Property">
								<plx:CallObject>
									<plx:Value type="Parameter" data="instance"/>
								</plx:CallObject>
							</plx:CallInstance>
						</xsl:otherwise>
					</xsl:choose>
				</plx:PassParam>
			</xsl:for-each>
		</xsl:variable>
		<plx:Condition>
			<plx:Test>
				<plx:Operator type="BooleanNot">
					<plx:CallInstance name="On{@Name}Changing" type="MethodCall">
						<plx:CallObject>
							<plx:ThisKeyword/>
						</plx:CallObject>
						<plx:PassParam>
							<plx:Value type="Parameter" data="instance"/>
						</plx:PassParam>
						<plx:PassParam>
							<plx:CallStatic type="MethodCall" dataTypeName="Tuple" name="CreateTuple">
								<!-- PLIX_TODO: Plix does not currently support calling Generic Methods -->
								<!--<xsl:copy-of select="$passTypeParams"/>-->
								<xsl:copy-of select="$passParams"/>
							</plx:CallStatic>
						</plx:PassParam>
					</plx:CallInstance>
				</plx:Operator>
			</plx:Test>
			<plx:Body>
				<plx:Return>
					<plx:FalseKeyword />
				</plx:Return>
			</plx:Body>
		</plx:Condition>
	</xsl:template>
	<xsl:template name="GetSimpleBinaryUniquenessPropertyChangedCode">
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
		<xsl:variable name="passTypeParams">
			<xsl:for-each select="$parameters">
				<plx:PassTypeParam>
					<xsl:copy-of select="DataType/@*"/>
					<xsl:copy-of select="DataType/child::*"/>
				</plx:PassTypeParam>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="oldValuePassParams">
			<xsl:for-each select="$parameters">
				<plx:PassParam>
					<xsl:choose>
						<xsl:when test="@special='true'">
							<plx:Value type="Parameter" data="oldValue" />
						</xsl:when>
						<xsl:otherwise>
							<plx:CallInstance name="{@name}"  type="Property">
								<plx:CallObject>
									<plx:Value type="Parameter" data="instance"/>
								</plx:CallObject>
							</plx:CallInstance>
						</xsl:otherwise>
					</xsl:choose>
				</plx:PassParam>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="newValuePassParams">
			<xsl:for-each select="$parameters">
				<plx:PassParam>
					<plx:CallInstance name="{@name}"  type="Property">
						<plx:CallObject>
							<plx:Value type="Parameter" data="instance"/>
						</plx:CallObject>
					</plx:CallInstance>
				</plx:PassParam>
			</xsl:for-each>
		</xsl:variable>
		<plx:CallInstance name="On{@Name}Changed" type="MethodCall">
			<plx:CallObject>
				<plx:ThisKeyword/>
			</plx:CallObject>
			<plx:PassParam>
				<plx:Value type="Parameter" data="instance"/>
			</plx:PassParam>
			<plx:PassParam>
				<plx:CallStatic type="MethodCall" dataTypeName="Tuple" name="CreateTuple">
					<!-- PLIX_TODO: Plix does not currently support calling Generic Methods -->
					<!--<xsl:copy-of select="$passTypeParams"/>-->
					<xsl:copy-of select="$oldValuePassParams"/>
				</plx:CallStatic>
			</plx:PassParam>
			<plx:PassParam>
				<plx:CallStatic type="MethodCall" dataTypeName="Tuple" name="CreateTuple">
					<!-- PLIX_TODO: Plix does not currently support calling Generic Methods -->
					<!--<xsl:copy-of select="$passTypeParams"/>-->
					<xsl:copy-of select="$newValuePassParams"/>
				</plx:CallStatic>
			</plx:PassParam>
		</plx:CallInstance>
	</xsl:template>
	
</xsl:stylesheet>