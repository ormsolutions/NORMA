<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:orm="http://schemas.neumont.edu/ORM/ORMCore"
	xmlns:ormRoot="http://schemas.neumont.edu/ORM/ORMRoot"
	xmlns:plx="http://schemas.neumont.edu/CodeGeneration/PLiX"
	xmlns:ao="http://schemas.neumont.edu/ORM/SDK/ClassGenerator/AbsorbedObjects">

	<!-- UNDONE: Using "uid" is a "magic string" cheat.  It may not be a problem, but it's still cheap. -->
	<xsl:variable name="ObjectUniqueIdentifier" select="'uid'"/>
	<!-- A word on the naming of absorbed objects in XML:
	Because object types can interact with each other more than once, we cannot simply say Object1 contains Object2.
	We must specify the relationship that is going on between the two.  Ideally, we would simply use the role name
	of the opposite object.  For example, in "Person was born on Date", Date plays the role "birthday".  If that role
	name is filled in, use it.  Otherwise, we need to identify it as something like "PersonwasbornonDate".  An uglier
	name to be sure, but it is far better than simply using "Date".
	
	Currently, an ao:AbsorbedObject is serialized as an attribute with the ID of 
	concat(@oppositeRoleName,@name)
	An ao:RelatedObject is serialized as either an attribute or an element with the name of 
	concat(@oppositeRoleName,@oppositeObjectName).-->
	<xsl:template name="CreateSerializationClass">
		<xsl:param name="AbsorbedObjects"/>
		<xsl:param name="ModelContextName"/>
		<!--<plx:namespaceImport name="System.Xml"/>
		<plx:namespaceImport name="System.Xml.Serialization"/>
		<plx:namespaceImport name="System.IO"/>-->
		<plx:class name="SerializationEngine" visibility="public">
			<plx:field name="myFactory" visibility="private" dataTypeName="I{$ModelContextName}"/>
			<plx:field name="deserializationTable" visibility="private" dataTypeName="ElementLoaderNameTable"/>
			<plx:function name=".construct"  visibility="public">
				<plx:param name="factory" dataTypeName="I{$ModelContextName}"/>
				<plx:assign>
					<plx:left>
						<plx:callThis name="myFactory" type="field"/>
					</plx:left>
					<plx:right>
						<plx:nameRef name="factory" type="parameter"/>
					</plx:right>
				</plx:assign>
			</plx:function>

			<!-- Serialization function -->
			<plx:function name="Serialize" visibility="public">
				<plx:param name="writer" dataTypeName="XmlWriter"/>
				<xsl:for-each select="$AbsorbedObjects">
					<xsl:variable name="Decorator">
						<xsl:if test="local-name()='Association'">
							<xsl:value-of select="$AssociationClassSuffix"/>
						</xsl:if>
					</xsl:variable>
					<plx:local name="{@name}{$Decorator}Collection" dataTypeName="ReadOnlyCollection">
						<plx:passTypeParam dataTypeName="{@name}{$Decorator}"/>
						<plx:initialize>
							<plx:callInstance name="Created{@name}{$Decorator}Collection" type="property">
								<plx:callObject>
									<plx:callThis name="myFactory" type="field"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:initialize>
					</plx:local>		
				</xsl:for-each>
				<xsl:for-each select="$AbsorbedObjects">
					<xsl:variable name="Decorator">
						<xsl:if test="local-name()='Association'">
							<xsl:value-of select="$AssociationClassSuffix"/>
						</xsl:if>
					</xsl:variable>
					<xsl:variable name="ObjectName" select="concat(@name,$Decorator)"/>
					<xsl:variable name="LocalInstance" select="concat('a',$ObjectName)"/>
					<plx:iterator localName="{$LocalInstance}" dataTypeName="{$ObjectName}">
						<plx:passTypeParam dataTypeName="{$ObjectName}Collection"/>
						<plx:initialize>
							<plx:nameRef name="{$ObjectName}Collection"/>
						</plx:initialize>
						<plx:callInstance name="WriteStartElement">
							<plx:callObject>
								<plx:nameRef type="parameter" name="writer"/>
							</plx:callObject>
							<plx:passParam>
								<plx:string>
									<xsl:value-of select="@name"/>
								</plx:string>
							</plx:passParam>
						</plx:callInstance>
						<!-- Write the (artificial) unique identifier for this object. -->
						<plx:callInstance name="WriteAttributeString">
							<plx:callObject>
								<plx:nameRef type="parameter" name="writer"/>
							</plx:callObject>
							<plx:passParam>
								<plx:string>
									<xsl:value-of select="$ObjectUniqueIdentifier"/>
								</plx:string>
							</plx:passParam>
							<plx:passParam>
								<plx:callStatic name="Concat" dataTypeName=".string">
									<plx:passParam>
										<plx:string>
											<xsl:value-of select="@name"/>
										</plx:string>
									</plx:passParam>
									<plx:passParam>
										<plx:callInstance name="ToString">
											<plx:callObject>
												<plx:callInstance name="IndexOf">
													<plx:callObject>
														<plx:nameRef name="{@name}{$Decorator}Collection"/>
													</plx:callObject>
													<plx:passParam>
														<plx:nameRef name="a{@name}{$Decorator}"/>
													</plx:passParam>
												</plx:callInstance>
											</plx:callObject>
										</plx:callInstance>
									</plx:passParam>
								</plx:callStatic>
							</plx:passParam>
						</plx:callInstance>
						<!--Start the WriteAttributeString() for each name in the absorbedOject-->
						<xsl:for-each select="ao:AbsorbedObject">
							<plx:callInstance name="WriteAttributeString">
								<plx:callObject>
									<plx:nameRef type="parameter" name="writer"/>
								</plx:callObject>
								<plx:passParam>
									<plx:string>
										<xsl:value-of select="@name"/>
									</plx:string>
								</plx:passParam>
								<plx:passParam>
									<plx:callInstance name="{@name}" type="property">
										<plx:callObject>
											<plx:nameRef name="{$LocalInstance}"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:passParam>
							</plx:callInstance>
						</xsl:for-each>
						<!--End WriteAttributeString()-->
						<!--Check to see if mulitiplicity is 0 to Many or 1 to Many for ReferenceValues-->
						<xsl:for-each select="ao:RelatedObject">
							<xsl:variable name="RelatedObject" select="concat(@oppositeObjectName,@oppositeRolName)"/>
							<xsl:variable name="RoleNameInstance" select="concat('a',@roleName)"/>
							<xsl:variable name="OppositeObjectNameInstance" select="concat('a',@oppositeObjectName)"/>
							<xsl:if test="(@multiplicity = 'ZeroToMany') or (@multiplicity = 'OneToMany')">
								<plx:local name="{$ObjectName}{@oppositeObjectName}Collection" dataTypeName="ICollection">
									<plx:passTypeParam dataTypeName="{@oppositeObjectName}"/>
									<plx:initialize>
										<plx:callInstance name="{@oppositeObjectName}" type="property">
											<plx:callObject>
												<plx:nameRef name="{$RoleNameInstance}"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:initialize>
								</plx:local>
								<plx:iterator localName="{$OppositeObjectNameInstance}" dataTypeName="Task">
									<plx:passTypeParam dataTypeName="PersonTaskCollection"/>
									<plx:initialize>
										<plx:nameRef name="PersonTaskCollection"/>
									</plx:initialize>
									<plx:branch>
										<plx:condition>
											<plx:binaryOperator type="identityInequality">
												<plx:left>
													<plx:nullKeyword/>
												</plx:left>
												<plx:right>
													<plx:nameRef name="{$OppositeObjectNameInstance}"/>
												</plx:right>
											</plx:binaryOperator>
										</plx:condition>
										<plx:body>
											<plx:callInstance name="WriteStartElement">
												<plx:callObject>
													<plx:nameRef type="parameter" name="writer"/>
												</plx:callObject>
												<plx:passParam>
													<plx:string>
														<xsl:value-of select="$RelatedObject"/>
													</plx:string>
												</plx:passParam>
											</plx:callInstance>
											<xsl:call-template name="WriteRelatedObjectIdentifierToAttribute">
												<xsl:with-param name="RelatedObject" select="$RelatedObject"/>
												<!--<xsl:with-param name="RelatedObjectCollection" select=""/>-->
											</xsl:call-template>
											<plx:callInstance name="WriteAttributeString">
												<plx:callObject>
													<plx:nameRef type="parameter" name="writer"/>
												</plx:callObject>
												<plx:passParam>
													<plx:string>
														<xsl:value-of select="$RelatedObject"/>
													</plx:string>
												</plx:passParam>
												<plx:passParam>
													<plx:callInstance name="ToString">
														<plx:callObject>
															<plx:callInstance name="TaskID" type="property">
																<plx:callObject>
																	<plx:nameRef name="{$OppositeObjectNameInstance}"/>
																</plx:callObject>
															</plx:callInstance>
														</plx:callObject>
													</plx:callInstance>
												</plx:passParam>
											</plx:callInstance>
											<plx:callInstance name="WriteEndElement">
												<plx:callObject>
													<plx:nameRef type="parameter" name="writer"/>
												</plx:callObject>
											</plx:callInstance>
										</plx:body>
									</plx:branch>
								</plx:iterator>
								<plx:callInstance name="WriteEndElement">
									<plx:callObject>
										<plx:nameRef type="parameter" name="writer"/>
									</plx:callObject>
								</plx:callInstance>
							</xsl:if>
							<!--Checking to see if multiplicity is ExactlyOne or ZeroToOne for References-->
							<xsl:if test="(@multiplicity = 'ExactlyOne') or (@multiplicity = 'ZeroToOne')">
								<xsl:call-template name="WriteRelatedObjectIdentifierToAttribute">
									<xsl:with-param name="RelatedObject" select="$RelatedObject"/>
									<!--<xsl:with-param name="RelatedObjectCollection" select=""/>-->
								</xsl:call-template>
								<plx:callInstance name="WriteEndElement">
									<plx:callObject>
										<plx:nameRef type="parameter" name="writer"/>
									</plx:callObject>
								</plx:callInstance>
							</xsl:if>
						</xsl:for-each>
					</plx:iterator>
				</xsl:for-each>
			</plx:function>

			<!-- Deserialization function -->
			<plx:function name="Deserialize" visibility="public">
				<plx:param name="reader" dataTypeName="XmlReader"/>
				<!-- XmlReaderSettings readerSettings = new XmlReaderSettings(); -->
				<plx:local name="readerSettings" dataTypeName="XmlReaderSettings" dataTypeQualifier="System.Xml">
					<plx:initialize>
						<plx:callNew dataTypeName="XmlReaderSettings" dataTypeQualifier="System.Xml"/>
					</plx:initialize>
				</plx:local>
				<!-- readerSettings.CloseInput = false; -->
				<plx:assign>
					<plx:left>
						<plx:callInstance name="CloseInput" type="property">
							<plx:callObject>
								<plx:nameRef name="readerSettings"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:left>
					<plx:right>
						<plx:falseKeyword/>
					</plx:right>
				</plx:assign>
				<!-- XmlReader myReader = XmlReader.Create(reader, readerSettings); -->
				<plx:local name="myReader" dataTypeName="XmlReader" dataTypeQualifier="System.Xml">
					<plx:initialize>
						<plx:callStatic name="Create" dataTypeName="XmlReader" dataTypeQualifier="System.Xml">
							<plx:passParam>
								<plx:nameRef type="parameter" name="reader"/>
							</plx:passParam>
							<plx:passParam>
								<plx:nameRef name="readerSettings"/>
							</plx:passParam>
						</plx:callStatic>
					</plx:initialize>
				</plx:local>

				<!-- myReader.NameTable = ElementLoaderSchema.Names; -->
				<plx:assign>
					<plx:left>
						<plx:callInstance name="NameTable" type="property">
							<plx:callObject>
								<plx:nameRef type="parameter" name="myReader"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:left>
					<plx:right>
						<plx:callStatic name="Names" type="property" dataTypeName="ElementLoaderSchema"/>
					</plx:right>
				</plx:assign>
				<!--IModel1DeserializationFactory deserializationFactory = myIModel1Factory.BeginDeserialization;-->
				<plx:local name="deserializationFactory" dataTypeName="IDeserialization{$ModelContextName}">
					<plx:initialize>
						<plx:callInstance name="BeginDeserialization">
							<plx:callObject>
								<plx:callThis name="myFactory" type="field"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:initialize>
				</plx:local>
				<!-- Bool continueLoop = true -->
				<plx:local name="continueLoop" dataTypeName=".boolean">
					<plx:initialize>
						<plx:trueKeyword/>
					</plx:initialize>
				</plx:local>
				<plx:try>
					<plx:body>
						<xsl:for-each select="$AbsorbedObjects">
							<xsl:variable name="Decorator">
								<xsl:if test="local-name()='Association'">
									<xsl:value-of select="$AssociationClassSuffix"/>
								</xsl:if>
							</xsl:variable>
							<plx:local name="Created{@name}{$Decorator}Dictionary" dataTypeName="Dictionary">
								<plx:passTypeParam dataTypeName=".string"/>
								<plx:passTypeParam dataTypeName="{@name}{$Decorator}"/>
								<plx:initialize>
									<plx:callNew dataTypeName="Dictionary">
										<plx:passTypeParam dataTypeName=".string"/>
										<plx:passTypeParam dataTypeName="{@name}{$Decorator}"/>
									</plx:callNew>
								</plx:initialize>
							</plx:local>
						</xsl:for-each>
						<!-- First of two passes for deserialization -->
						<plx:loop>
							<!-- This is the best simulation we can get for:
							while(myReader.Read() && continueLoop-->
							<plx:condition>
								<plx:binaryOperator type="booleanAnd">
									<plx:left>
										<plx:callInstance name="Read">
											<plx:callObject>
												<plx:nameRef type="parameter" name="myReader"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:nameRef name="continueLoop"/>
									</plx:right>
								</plx:binaryOperator>
							</plx:condition>
							<plx:body>
								<plx:local name="nodeType" dataTypeName="XmlNodeType" dataTypeQualifier="System.Xml">
									<plx:initialize>
										<plx:callInstance name="NodeType" type="property">
											<plx:callObject>
												<plx:nameRef type="parameter" name="myReader"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:initialize>
								</plx:local>
								<plx:branch>
									<!-- Process XmlNodeType.Element nodes -->
									<plx:condition>
										<plx:binaryOperator type="equality">
											<plx:left>
												<plx:nameRef name="nodeType"/>
											</plx:left>
											<plx:right>
												<plx:callStatic name="Element" type="property" dataTypeName="XmlNodeType"/>
											</plx:right>
										</plx:binaryOperator>
									</plx:condition>
									<plx:body>
										<!-- string localName = myReader.LocalName-->
										<plx:local name="localName" dataTypeName=".string">
											<plx:initialize>
												<plx:callInstance name="LocalName" type="property">
													<plx:callObject>
														<plx:nameRef type="parameter" name="myReader"/>
													</plx:callObject>
												</plx:callInstance>
											</plx:initialize>
										</plx:local>
										<xsl:for-each select="$AbsorbedObjects">
											<xsl:if test="position()=1">
												<xsl:call-template name="DeserializationFirstPassLoop"/>
											</xsl:if>
										</xsl:for-each>
									</plx:body>
								</plx:branch>
							</plx:body>
						</plx:loop>
						<!-- Reset the reader for another pass -->
						<plx:assign>
							<plx:left>
								<plx:nameRef name="myReader"/>
							</plx:left>
							<plx:right>
								<plx:callStatic name="Create" dataTypeName="XmlReader" dataTypeQualifier="System.Xml">
									<plx:passParam>
										<plx:nameRef type="parameter" name="reader"/>
									</plx:passParam>
									<plx:passParam>
										<plx:nameRef name="readerSettings"/>
									</plx:passParam>
								</plx:callStatic>
							</plx:right>
						</plx:assign>
						<plx:assign>
							<plx:left>
								<plx:nameRef name="continueLoop"/>
							</plx:left>
							<plx:right>
								<plx:trueKeyword/>
							</plx:right>
						</plx:assign>
						<!-- Second of two passes for deserialization -->
						<plx:loop>
							<!-- This is the best simulation we can get for:
							while(myReader.Read() && continueLoop-->
							<plx:condition>
								<plx:binaryOperator type="booleanAnd">
									<plx:left>
										<plx:callInstance name="Read">
											<plx:callObject>
												<plx:nameRef type="parameter" name="myReader"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:nameRef name="continueLoop"/>
									</plx:right>
								</plx:binaryOperator>
							</plx:condition>
							<plx:body>
								<plx:local name="nodeType" dataTypeName="XmlNodeType" dataTypeQualifier="System.Xml">
									<plx:initialize>
										<plx:callInstance name="NodeType" type="property">
											<plx:callObject>
												<plx:nameRef type="parameter" name="myReader"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:initialize>
								</plx:local>
								<!-- Process references that were absorbed as attributes -->
									<plx:branch>
										<plx:condition>
										<plx:binaryOperator type="equality">
											<plx:left>
												<plx:nameRef name="nodeType"/>
											</plx:left>
												<plx:right>
													<plx:callStatic name="Element" type="property" dataTypeName="XmlNodeType"/>
											</plx:right>
											</plx:binaryOperator>
										</plx:condition>
										<plx:body>
											<!-- string localName = myReader.LocalName-->
											<plx:local name="localName" dataTypeName=".string">
												<plx:initialize>
													<plx:callInstance name="LocalName" type="property">
														<plx:callObject>
															<plx:nameRef type="parameter" name="myReader"/>
													</plx:callObject>
													</plx:callInstance>
												</plx:initialize>
											</plx:local>
											<xsl:for-each select="$AbsorbedObjects">
												<xsl:if test="position()=1">
													<!-- UNDONE: This is currently spitting bad plix for any absorbed object with more than
														 one related child. -->
													<!--<xsl:call-template name="DeserializationSecondPassLoop"/>-->
												</xsl:if>
											</xsl:for-each>
										</plx:body>
									</plx:branch>
									<!--<xsl:for-each select="ao:RelatedObject">
										<xsl:element name="tyTemp"/>
										<plx:branch>
											<plx:condition>
												<plx:binaryOperator type="equality">
													<plx:left>
														<plx:value data="nodeType"/>
													</plx:left>
													<plx:right>
														<plx:callStatic name="Element" type="property" dataTypeName="XmlNodeType"/>
													</plx:right>
												</plx:binaryOperator>
											</plx:condition>
										</plx:branch>
									</xsl:for-each>-->
								</plx:body>
							</plx:loop>
						</plx:body>
					<plx:finally>
						<plx:callInstance name="Close">
							<plx:callObject>
								<plx:nameRef name="myReader"/>
							</plx:callObject>
						</plx:callInstance>
						<plx:callInstance name="Close">
							<plx:callObject>
								<plx:nameRef type="parameter" name="reader"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:finally>
				</plx:try>
			</plx:function>
		</plx:class>
		<plx:class name="ElementLoaderSchema" visibility="public" modifier="static">
			<xsl:for-each select="$AbsorbedObjects">
				<xsl:variable name="ObjectName" select="@name"/>
				<plx:field name="{$ObjectName}_Object" visibility="public" const="true" dataTypeName=".string">
					<plx:initialize>
						<plx:string>
							<xsl:value-of select="$ObjectName"/>
						</plx:string>
					</plx:initialize>
				</plx:field>
				<xsl:for-each select="ao:AbsorbedObject">
					<plx:field name="{$ObjectName}{@name}_Attribute" visibility="public" const="true" dataTypeName=".string">
						<plx:initialize>
							<plx:string>
								<xsl:value-of select="@name"/>
							</plx:string>
						</plx:initialize>
					</plx:field>
				</xsl:for-each>
				<xsl:for-each select="ao:RelatedObject[@arity='2']">
					<!-- UNDONE: Association classes (arity > 2) -->
					<plx:field name="{$ObjectName}{@oppositeRoleName}{@oppositeObjectName}_Reference" visibility="public" const="true" dataTypeName=".string">
						<plx:initialize>
							<plx:string>
								<xsl:value-of select="@oppositeRoleName"/>
								<xsl:value-of select="@oppositeObjectName"/>
								<xsl:text>_Ref</xsl:text>
							</plx:string>
						</plx:initialize>
					</plx:field>
				</xsl:for-each>
			</xsl:for-each>
			<plx:field name="myNames" visibility="private" static="true" dataTypeName="ElementLoaderNameTable"/>
			<plx:property name="Names" visibility="public" modifier="static">
				<plx:returns dataTypeName="ElementLoaderNameTable"/>
				<plx:get>
					<plx:local name="retVal" dataTypeName="ElementLoaderNameTable">
						<plx:initialize>
							<plx:callStatic type="field" name="myNames" dataTypeName="ElementLoaderNameTable"/>
						</plx:initialize>
					</plx:local>
					<plx:branch>
						<plx:condition>
							<plx:binaryOperator type="equality">
								<plx:left>
									<plx:nameRef name="retVal"/>
								</plx:left>
								<plx:right>
									<plx:nullKeyword/>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:body>
							<plx:assign>
								<plx:left>
									<plx:nameRef name="myNames"/>
								</plx:left>
								<plx:right>
									<plx:callNew dataTypeName="ElementLoaderNameTable"/>
								</plx:right>
							</plx:assign>
							<plx:assign>
								<plx:left>
									<plx:nameRef name="retVal"/>
								</plx:left>
								<plx:right>
									<plx:nameRef name="myNames"/>
								</plx:right>
							</plx:assign>
						</plx:body>
					</plx:branch>
					<plx:return>
						<plx:nameRef name="retVal"/>
					</plx:return>
				</plx:get>
			</plx:property>
		</plx:class>
		<plx:class name="ElementLoaderNameTable" visibility="public">
			<plx:derivesFromClass dataTypeName="NameTable" dataTypeQualifier="System.Xml"/>
			<!-- To ensure uniqueness for each element in the name table, we need to be careful about naming
			our objects, absorbed objects, and related objects.  The convention used here is:
			ao:Object : concat(@name, '_Object')
			ao:AbsorbedObject : concat(parent::@name, @name, '_Attribute')
			ao:RelatedObject : concat(parent::@name, @oppositeRoleName, @oppositeObjectName, '_Reference')
			-->
			<xsl:for-each select="$AbsorbedObjects">
				<xsl:variable name="ObjectName" select="@name"/>
				<plx:field name="{$ObjectName}_Object" visibility="public" readOnly="true" dataTypeName=".string"/>
				<xsl:for-each select="ao:AbsorbedObject">
					<plx:field name="{$ObjectName}{@name}_Attribute" visibility="public" readOnly="true" dataTypeName=".string"/>
				</xsl:for-each>
				<xsl:for-each select="ao:RelatedObject[@arity='2']">
					<!-- UNDONE: Association classes (arity > 2) -->
					<plx:field name="{$ObjectName}{@oppositeRoleName}{@oppositeObjectName}_Reference" visibility="public" readOnly="true" dataTypeName=".string"/>
				</xsl:for-each>
			</xsl:for-each>
			<plx:function name=".construct" visibility="public">
				<plx:initialize>
					<plx:callThis name=".implied"/>
				</plx:initialize>
				<xsl:for-each select="$AbsorbedObjects">
					<xsl:variable name="ObjectName" select="@name"/>
					<plx:assign>
						<plx:left>
							<plx:callThis name="{@name}_Object" type="field"/>
						</plx:left>
						<plx:right>
							<plx:callThis name="Add">
								<plx:passParam>
									<plx:callStatic name="{$ObjectName}_Object" dataTypeName="ElementLoaderSchema" type="property"/>
								</plx:passParam>
							</plx:callThis>
						</plx:right>
					</plx:assign>
					<xsl:for-each select="ao:AbsorbedObject">
						<plx:assign>
							<plx:left>
								<plx:callThis name="{$ObjectName}{@name}_Attribute" type="field"/>
							</plx:left>
							<plx:right>
								<plx:callThis name="Add">
									<plx:passParam>
										<plx:callStatic name="{$ObjectName}{@name}_Attribute" dataTypeName="ElementLoaderSchema" type="property"/>
									</plx:passParam>
								</plx:callThis>
							</plx:right>
						</plx:assign>
					</xsl:for-each>
					<xsl:for-each select="ao:RelatedObject">
						<plx:assign>
							<plx:left>
								<plx:callThis name="{$ObjectName}{@oppositeRoleName}{@oppositeObjectName}_Reference" type="field"/>
							</plx:left>
							<plx:right>
								<plx:callThis name="Add">
									<plx:passParam>
										<plx:callStatic name="{$ObjectName}{@oppositeRoleName}{@oppositeObjectName}_Reference" dataTypeName="ElementLoaderSchema" type="property"/>
									</plx:passParam>
								</plx:callThis>
							</plx:right>
						</plx:assign>
					</xsl:for-each>
				</xsl:for-each>
			</plx:function>
		</plx:class>
	</xsl:template>
	<xsl:template name="WriteRelatedObjectIdentifierToAttribute">
		<xsl:param name="RelatedObject"/>
		<xsl:param name="RelatedObjectCollection"/>
		<plx:callInstance name="WriteAttributeString">
			<plx:callObject>
				<plx:nameRef type="parameter" name="writer"/>
			</plx:callObject>
			<plx:passParam>
				<plx:string>
					<xsl:value-of select="concat(@roleName,@oppositeRoleName,@oppositeObjectName,'_Ref')"/>
				</plx:string>
			</plx:passParam>
			<plx:passParam>
				<plx:callStatic name="Concat" dataTypeName=".string">
					<plx:passParam>
						<plx:string>
							<xsl:value-of select="concat(@roleName,@oppositeRoleName,@oppositeObjectName)"/>
						</plx:string>
					</plx:passParam>
					<plx:passParam>
				<plx:callInstance name="ToString">
					<plx:callObject>
						<plx:callInstance name="IndexOf">
							<plx:callObject>
								<plx:nameRef name="{@oppositeObjectName}Collection"/>
							</plx:callObject>
							<plx:passParam>
								<plx:nameRef name="a{@oppositeObjectName}"/>
							</plx:passParam>
						</plx:callInstance>
					</plx:callObject>
				</plx:callInstance>
					</plx:passParam>
				</plx:callStatic>
			</plx:passParam>
		</plx:callInstance>
	</xsl:template>
	
	<xsl:template name="DeserializationFirstPassLoop">
		<xsl:param name="Fallback" select="false()"/>
		<xsl:choose>
			<xsl:when test="$Fallback">
				<plx:alternateBranch>
					<xsl:call-template name="DeserializationFirstPassLoopContents">
						<xsl:with-param name="ObjectName" select="@name"/>
					</xsl:call-template>
				</plx:alternateBranch>
			</xsl:when>
			<xsl:otherwise>
				<plx:branch>
					<xsl:call-template name="DeserializationFirstPassLoopContents">
						<xsl:with-param name="ObjectName" select="@name"/>
					</xsl:call-template>
					<xsl:if test="position()!=last()">
						<xsl:for-each select="following-sibling::*">
							<xsl:call-template name="DeserializationFirstPassLoop">
								<xsl:with-param name="Fallback" select="true()"/>
							</xsl:call-template>
						</xsl:for-each>
					</xsl:if>
					<!--  These lines can be uncommented to let you have XSL Intellisense.
					We're writing just the fallback condition here, but the XSD says you need a
					Test and a Body before you can do FallbackCondition. 
					Our Test and Body are elsewhere-->
					<!--<plx:condition></plx:condition>
					<plx:body></plx:body>-->
					<plx:alternateBranch>
						<plx:condition>
							<plx:binaryOperator type="equality">
								<plx:left>
									<plx:nameRef name="nodeType"/>
								</plx:left>
								<plx:right>
									<plx:callStatic name="EndElement" dataTypeName="XmlNodeType" type="property"/>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:body>
							<plx:assign>
								<plx:left>
									<plx:nameRef name="continueLoop"/>
								</plx:left>
								<plx:right>
									<plx:falseKeyword/>
								</plx:right>
							</plx:assign>
						</plx:body>
					</plx:alternateBranch>
				</plx:branch>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="DeserializationFirstPassLoopContents">
		<xsl:variable name="ObjectName"/>
		<!--<plx:branch>-->
		<plx:condition>
			<plx:callStatic name="ReferenceEquals" dataTypeName="ElementLoaderSchema">
				<plx:passParam>
					<plx:nameRef name="localName"/>
				</plx:passParam>
				<plx:passParam>
					<plx:callInstance name="{@name}_Object" type="property">
						<plx:callObject>
							<plx:nameRef name="deserializationTable"/>
						</plx:callObject>
					</plx:callInstance>
				</plx:passParam>
			</plx:callStatic>
		</plx:condition>
		<plx:body>
			<xsl:variable name="aoAbsorbedObjects" select="ao:AbsorbedObject[@mandatory='true']"/>
			<xsl:for-each select="$aoAbsorbedObjects">
				<plx:local name="local{@name}" dataTypeName=".string">
					<plx:initialize>
						<plx:callInstance name=".implied" type="indexerCall">
							<plx:callObject>
								<plx:nameRef type="parameter" name="myReader"/>
							</plx:callObject>
							<plx:passParam>
								<plx:string>
									<xsl:value-of select="@name"/>
								</plx:string>
							</plx:passParam>
						</plx:callInstance>
					</plx:initialize>
				</plx:local>

			</xsl:for-each>
			<plx:callInstance name="Add">
				<plx:callObject>
					<plx:nameRef name="Created{@name}Dictionary"/>
				</plx:callObject>
				<plx:passParam>
					<plx:callInstance name=".implied" type="indexerCall">
						<plx:callObject>
							<plx:nameRef name="myReader"/>
						</plx:callObject>
						<plx:passParam>
							<plx:string>id</plx:string>
						</plx:passParam>
					</plx:callInstance>
				</plx:passParam>
				<plx:passParam>
					<plx:callInstance name="Create{@name}">
						<plx:callObject>
							<plx:nameRef name="deserializationFactory"/>
						</plx:callObject>
						<xsl:for-each select="$aoAbsorbedObjects">
							<plx:passParam>
								<plx:nameRef name="local{@name}"/>
							</plx:passParam>
						</xsl:for-each>
					</plx:callInstance>
				</plx:passParam>
			</plx:callInstance>
		</plx:body>
		<!--</plx:branch>-->
	</xsl:template>
	
	<xsl:template name="DeserializationSecondPassLoop">
		<xsl:param name="Fallback" select="false()"/>
		<xsl:choose>
			<xsl:when test="$Fallback">
				<!--<xsl:if test="(@multiplicity = 'ZeroToOne') or (@multiplicity = 'ExactlyOne')">-->
					<plx:alternateBranch>
						<xsl:call-template name="DeserializationSecondPassLoopContents">
							<xsl:with-param name="ObjectName" select="@name"/>
						</xsl:call-template>
					</plx:alternateBranch>
				<!--</xsl:if>-->
			</xsl:when>
			<xsl:otherwise>
				<plx:branch>
					<xsl:call-template name="DeserializationSecondPassLoopContents">
						<xsl:with-param name="ObjectName" select="@name"/>
					</xsl:call-template>
					<xsl:if test="position()!=last()">
						<xsl:for-each select="following-sibling::*">
							<xsl:call-template name="DeserializationSecondPassLoop">
								<xsl:with-param name="Fallback" select="true()"/>
							</xsl:call-template>
						</xsl:for-each>
					</xsl:if>
					<!--  These lines can be uncommented to let you have XSL Intellisense.
					We're writing just the fallback condition here, but the XSD says you need a
					Test and a Body before you can do FallbackCondition. 
					Our Test and Body are elsewhere-->
					<!--<plx:condition></plx:condition>
					<plx:body></plx:body>-->
					<plx:alternateBranch>
						<plx:condition>
							<plx:binaryOperator type="equality">
								<plx:left>
									<plx:nameRef name="nodeType"/>
								</plx:left>
								<plx:right>
									<plx:callStatic name="EndElement" dataTypeName="XmlNodeType" type="property"/>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:body>
							<plx:assign>
								<plx:left>
									<plx:nameRef name="continueLoop"/>
								</plx:left>
								<plx:right>
									<plx:falseKeyword/>
								</plx:right>
							</plx:assign>
						</plx:body>
					</plx:alternateBranch>
				</plx:branch>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="DeserializationSecondPassLoopContents">
		<xsl:param name="ObjectName" select="'Unknown_Object'"/>
		<xsl:for-each select="ao:RelatedObject">
			<xsl:choose>
				<xsl:when test="(@multiplicity = 'ZeroToOne') or (@multiplicity = 'ExactlyOne')">
					<!--<plx:branch>-->
						<plx:condition>
							<plx:callStatic name="ReferenceEquals" dataTypeName="ElementLoaderSchema">
								<plx:passParam>
									<plx:nameRef name="localName"/>
								</plx:passParam>
								<plx:passParam>
									<plx:callInstance name="{$ObjectName}_Object" type="property">
										<plx:callObject>
											<plx:nameRef name="deserializationTable"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:passParam>
							</plx:callStatic>
						</plx:condition>

						<plx:body>
							<plx:local name="local{$ObjectName}" dataTypeName="{$ObjectName}">
								<plx:initialize>
									<plx:callInstance name=".implied" type="indexerCall">
										<plx:callObject>
											<plx:nameRef name="Created{$ObjectName}Dictionary"/>
										</plx:callObject>
										<plx:passParam>
											<plx:callInstance name=".implied" type="indexerCall">
												<plx:callObject>
													<plx:nameRef name="myReader"/>
												</plx:callObject>
												<plx:passParam>
													<plx:string>
														<xsl:value-of select="$ObjectUniqueIdentifier"/>
													</plx:string>
												</plx:passParam>
											</plx:callInstance>
										</plx:passParam>
									</plx:callInstance>
								</plx:initialize>
							</plx:local>
							<plx:assign>
								<plx:left>
									<!-- TODO: First, the getters and setters for this property need to be created at
									the factory level.  Next, their names need to reflect the roles that the object types
									play.  It makes no sense to have Object1 Rs Object2 when there could be many
									interactions between those two Object Types.  Once the names for the relationships
									have been formalized, change the name here to reflect the update.  -->
									<plx:callInstance name="{@oppositeObjectName}" type="property">
										<plx:callObject>
											<plx:nameRef name="local{$ObjectName}"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:left>
								<plx:right>
									<plx:callInstance name=".implied" type="indexerCall">
										<plx:callObject>
											<plx:nameRef name="Created{@oppositeObjectName}Dictionary"/>
										</plx:callObject>
										<plx:passParam>
											<plx:callInstance name=".implied" type="indexerCall">
												<plx:callObject>
													<plx:nameRef name="myReader"/>
												</plx:callObject>
												<plx:passParam>
													<plx:string>
														<xsl:value-of select="concat($ObjectName, @oppositeRoleName, @oppositeObjectName, '_Reference')"/>
													</plx:string>
												</plx:passParam>
											</plx:callInstance>
										</plx:passParam>
									</plx:callInstance>
								</plx:right>
							</plx:assign>
						</plx:body>
					<!--</plx:branch>-->
				</xsl:when>
				<xsl:otherwise>
					<plx:condition>
						<plx:callStatic name="ReferenceEquals" dataTypeName="ElementLoaderSchema">
							<plx:passParam>
								<plx:nameRef name="localName"/>
							</plx:passParam>
							<plx:passParam>
								<plx:callInstance name="{$ObjectName}{@oppositeRoleName}{@oppositeObjectName}_Reference" type="property">
									<plx:callObject>
										<plx:nameRef name="deserializationTable"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:passParam>
						</plx:callStatic>
					</plx:condition>
					<!-- TODO: Create a body for this method.  It should go something like this:
					Look up the created object that (in the CreatedObjectDictionary) is the parent of the current node.
					Walk all child elements that match the ao:RelatedObject that we're dealing with right now.
					For each child element, look up the created object and add the link between it and the parent object.  
					
					This is currently left undone because there are special cases for @multiplicity='ManyToMany' and
					n-aries that we have not yet covered.  Also, the getters and setters for the 
					@multiplicityi='ZeroToOne' or = 'ExactlyOne' -should- be making the link between the
					object and its container class automatically.
					
					-->
				</xsl:otherwise>
			</xsl:choose>
		</xsl:for-each>
	</xsl:template>
</xsl:stylesheet>