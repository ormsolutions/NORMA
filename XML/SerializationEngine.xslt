<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:orm="http://Schemas.Neumont.edu/ORM/ORMCore"
	xmlns:ormRoot="http://Schemas.Neumont.edu/ORM/ORMRoot"
	xmlns:plx="http://Schemas.Neumont.edu/CodeGeneration/Plix"
	xmlns:ao="http://Schemas.Neumont.edu/ORM/SDK/ClassGenerator/AbsorbedObjects">

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
		<!--<plx:Using name="System.Xml"/>
		<plx:Using name="System.Xml.Serialization"/>
		<plx:Using name="System.IO"/>-->
		<plx:Class name="SerializationEngine" visibility="Public">
			<plx:Field name="myFactory" visibility="Private" dataTypeName="I{$ModelContextName}"/>
			<plx:Field name="deserializationTable" visibility="Private" dataTypeName="ElementLoaderNameTable"/>
			<plx:Function name="SerializationEngine" ctor="true" visibility="Public">
				<plx:Param name="IModel1Factory_in" dataTypeName="IModel1Factory"/>
				<plx:Operator type="Assign">
					<plx:Left>
						<plx:CallInstance name="myFactory" type="Field">
							<plx:CallObject>
								<plx:ThisKeyword/>
							</plx:CallObject>
						</plx:CallInstance>
					</plx:Left>
					<plx:Right>
						<plx:Value data="IModel1Factory_in" type="Local"/>
					</plx:Right>
				</plx:Operator>
			</plx:Function>

			<!-- Serialization function -->
			<plx:Function name="Serialize" visibility="Public">
				<plx:Param name="writer" dataTypeName="XmlWriter"/>
				<xsl:for-each select="$AbsorbedObjects">
					<plx:Variable name="{@name}Collection" dataTypeName="ReadOnlyCollection&lt;{@name}&gt;" dataTypeQualifier="System.Collections.ObjectModel">
						<plx:Initialize>
							<plx:CallInstance name="Created{@name}Collection" type="MethodCall">
								<plx:CallObject>
									<plx:CallInstance name="myFactory" type="Field">
										<plx:CallObject>
											<plx:ThisKeyword/>
										</plx:CallObject>
									</plx:CallInstance>
								</plx:CallObject>
							</plx:CallInstance>
						</plx:Initialize>
					</plx:Variable>		
				</xsl:for-each>
				<xsl:for-each select="$AbsorbedObjects">
					<xsl:variable name="ObjectName" select="@name"/>
					<xsl:variable name="LocalInstance" select="concat('a',$ObjectName)"/>
					<plx:Iterator variableName="{$LocalInstance}" dataTypeName="{@name}">
						<plx:PassTypeParam dataTypeName="{$ObjectName}Collection"/>
						<plx:Initialize>
							<plx:Value data="{$ObjectName}Collection" type="Local"/>
						</plx:Initialize>
						<plx:Body>
							<plx:CallInstance name="WriteStartElement" type="MethodCall">
								<plx:CallObject>
									<plx:Value type="Parameter" data="writer"/>
								</plx:CallObject>
								<plx:PassParam passStyle="In">
									<plx:String>
										<xsl:value-of select="@name"/>
									</plx:String>
								</plx:PassParam>
							</plx:CallInstance>
							<!-- Write the (artificial) unique identifier for this object. -->
							<plx:CallInstance name="WriteAttributeString" type="MethodCall">
								<plx:CallObject>
									<plx:Value data="writer" type="Parameter"/>
								</plx:CallObject>
								<plx:PassParam>
									<plx:String>
										<xsl:value-of select="$ObjectUniqueIdentifier"/>
									</plx:String>
								</plx:PassParam>
								<plx:PassParam>
									<plx:CallStatic name="Concat" dataTypeName="String" dataTypeQualifier="System" type="MethodCall">
										<plx:PassParam>
											<plx:String>
												<xsl:value-of select="@name"/>
											</plx:String>
										</plx:PassParam>
										<plx:PassParam>
											<plx:CallInstance name="ToString" type="MethodCall">
												<plx:CallObject>
													<plx:CallInstance name="IndexOf" type="MethodCall">
														<plx:CallObject>
															<plx:Value data="{@name}Collection" type="Local"/>
														</plx:CallObject>
														<plx:PassParam>
															<plx:Value data="a{@name}" type="Local"/>
														</plx:PassParam>
													</plx:CallInstance>
												</plx:CallObject>
											</plx:CallInstance>
										</plx:PassParam>
									</plx:CallStatic>
								</plx:PassParam>
							</plx:CallInstance>
							<!--Start the WriteAttributeString() for each name in the absorbedOject-->
							<xsl:for-each select="ao:AbsorbedObject">
								<plx:CallInstance name="WriteAttributeString" type="MethodCall">
									<plx:CallObject>
										<plx:Value data="writer" type="Parameter"/>
									</plx:CallObject>
									<plx:PassParam passStyle="In">
										<plx:String>
											<xsl:value-of select="@name"/>
										</plx:String>
									</plx:PassParam>
									<plx:PassParam>
										<plx:CallInstance name="{@name}" type="Property">
											<plx:CallObject>
												<plx:Value data="{$LocalInstance}" type="Local"/>
											</plx:CallObject>
										</plx:CallInstance>
									</plx:PassParam>
								</plx:CallInstance>
							</xsl:for-each>
							<!--End WriteAttributeString()-->
							<!--Check to see if mulitiplicity is 0 to Many or 1 to Many for ReferenceValues-->
							<xsl:for-each select="ao:RelatedObject">
								<xsl:variable name="RelatedObject" select="concat(@oppositeObjectName,@oppositeRolName)"/>
								<xsl:variable name="RoleNameInstance" select="concat('a',@roleName)"/>
								<xsl:variable name="OppositeObjectNameInstance" select="concat('a',@oppositeObjectName)"/>
								<xsl:if test="(@multiplicity = 'ZeroToMany') or (@multiplicity = 'OneToMany')">
									<plx:Variable name="{$ObjectName}{@oppositeObjectName}Collection" dataTypeName="ICollection&lt;{@oppositeObjectName}&gt;">
										<plx:Initialize>
											<plx:CallInstance name="{@oppositeObjectName}" type="Property">
												<plx:CallObject>
													<plx:Value data="$RoleNameInstance" type="Local"/>
												</plx:CallObject>
											</plx:CallInstance>
										</plx:Initialize>
									</plx:Variable>
											<plx:Iterator variableName="$OppositeObjectNameInstance" dataTypeName="Task">
												<plx:PassTypeParam dataTypeName="PersonTaskCollection"/>
												<plx:Initialize>
													<plx:Value data="PersonTaskCollection" type="Local"/>
												</plx:Initialize>
												<plx:Body>
									<plx:Condition>
										<plx:Test>
											<plx:Operator type="IdentityInequality">
												<plx:Left>
													<plx:NullObjectKeyword/>
												</plx:Left>
												<plx:Right>
													<plx:Value data="$OppositeObjectNameInstance" type="Local"/>
												</plx:Right>
											</plx:Operator>
										</plx:Test>
										<plx:Body>
											<plx:CallInstance name="WriteStartElement" type="MethodCall">
												<plx:CallObject>
													<plx:Value type="Parameter" data="writer"/>
												</plx:CallObject>
												<plx:PassParam passStyle="In">
													<plx:String>
														<xsl:value-of select="$RelatedObject"/>
													</plx:String>
												</plx:PassParam>
											</plx:CallInstance>
											<xsl:call-template name="WriteRelatedObjectIdentifierToAttribute">
												<xsl:with-param name="RelatedObject" select="$RelatedObject"/>
												<!--<xsl:with-param name="RelatedObjectCollection" select=""/>-->
											</xsl:call-template>
											<plx:CallInstance name="WriteAttributeString" type="MethodCall">
												<plx:CallObject>
													<plx:Value type="Parameter" data="writer"/>
												</plx:CallObject>
												<plx:PassParam passStyle="In">
													<plx:String>
														<xsl:value-of select="$RelatedObject"/>
													</plx:String>
												</plx:PassParam>
												<plx:PassParam>
													<plx:CallInstance name="ToString" type="MethodCall">
														<plx:CallObject>
															<plx:CallInstance name="TaskID" type="Property">
																<plx:CallObject>
																	<plx:Value data="$OppositeObjectNameInstance" type="Local"/>
																</plx:CallObject>
															</plx:CallInstance>
														</plx:CallObject>
													</plx:CallInstance>
												</plx:PassParam>
											</plx:CallInstance>
											<plx:CallInstance name="WriteEndElement" type="MethodCall">
												<plx:CallObject>
													<plx:Value type="Parameter" data="writer"/>
												</plx:CallObject>
											</plx:CallInstance>
										</plx:Body>
									</plx:Condition>
												</plx:Body>
											</plx:Iterator>
									<plx:CallInstance name="WriteEndElement" type="MethodCall">
										<plx:CallObject>
											<plx:Value type="Parameter" data="writer"/>
										</plx:CallObject>
									</plx:CallInstance>
								</xsl:if>
								<!--Checking to see if multiplicity is ExactlyOne or ZeroToOne for References-->
								<xsl:if test="(@multiplicity = 'ExactlyOne') or (@multiplicity = 'ZeroToOne')">
									<xsl:call-template name="WriteRelatedObjectIdentifierToAttribute">
										<xsl:with-param name="RelatedObject" select="$RelatedObject"/>
										<!--<xsl:with-param name="RelatedObjectCollection" select=""/>-->
									</xsl:call-template>
									<plx:CallInstance name="WriteEndElement" type="MethodCall">
										<plx:CallObject>
											<plx:Value type="Parameter" data="writer"/>
										</plx:CallObject>
									</plx:CallInstance>
								</xsl:if>
							</xsl:for-each>
						</plx:Body>
					</plx:Iterator>
				</xsl:for-each>
			</plx:Function>

			<!-- Deserialization function -->
			<plx:Function name="Deserialize" visibility="Public">
				<plx:Param name="reader" dataTypeName="XmlReader"/>
				<!-- XmlReaderSettings readerSettings = new XmlReaderSettings(); -->
				<plx:Variable name="readerSettings" dataTypeName="XmlReaderSettings" dataTypeQualifier="System.Xml">
					<plx:Initialize>
						<plx:CallNew dataTypeName="XmlReaderSettings" dataTypeQualifier="System.Xml" type="New"/>
					</plx:Initialize>
				</plx:Variable>
				<!-- readerSettings.CloseInput = false; -->
				<plx:Operator type="Assign">
					<plx:Left>
						<plx:CallInstance name="CloseInput" type="Property">
							<plx:CallObject>
								<plx:Value data="readerSettings" type="Local"/>
							</plx:CallObject>
						</plx:CallInstance>
					</plx:Left>
					<plx:Right>
						<plx:FalseKeyword/>
					</plx:Right>
				</plx:Operator>
				<!-- XmlReader myReader = XmlReader.Create(reader, readerSettings); -->
				<plx:Variable name="myReader" dataTypeName="XmlReader" dataTypeQualifier="System.Xml">
					<plx:Initialize>
						<plx:CallStatic name="Create" dataTypeName="XmlReader" type="MethodCall" dataTypeQualifier="System.Xml">
							<plx:PassParam>
								<plx:Value data="reader" type="Parameter"/>
							</plx:PassParam>
							<plx:PassParam>
								<plx:Value data="readerSettings" type="Local"/>
							</plx:PassParam>
						</plx:CallStatic>
					</plx:Initialize>
				</plx:Variable>

				<!-- myReader.NameTable = ElementLoaderSchema.Names; -->
				<plx:Operator type="Assign">
					<plx:Left>
						<plx:CallInstance name="NameTable" type="Property">
							<plx:CallObject>
								<plx:Value data="myReader" type="Parameter"/>
							</plx:CallObject>
						</plx:CallInstance>
					</plx:Left>
					<plx:Right>
						<plx:CallStatic name="Names" type="Property" dataTypeName="ElementLoaderSchema"/>
					</plx:Right>
				</plx:Operator>
				<!--IModel1DeserializationFactory deserializationFactory = myIModel1Factory.BeginDeserialization;-->
				<plx:Variable name="deserializationFactory" dataTypeName="IDeserialization{$ModelContextName}">
					<plx:Initialize>
						<plx:CallInstance name="BeginDeserialization" type="MethodCall">
							<plx:CallObject>
								<plx:CallInstance name="myFactory" type="Field">
									<plx:CallObject>
										<plx:ThisKeyword/>
									</plx:CallObject>
								</plx:CallInstance>
							</plx:CallObject>
						</plx:CallInstance>
					</plx:Initialize>
				</plx:Variable>
				<!-- Bool continueLoop = true -->
				<plx:Variable name="continueLoop" dataTypeName="Boolean" dataTypeQualifier="System">
					<plx:Initialize>
						<plx:TrueKeyword/>
					</plx:Initialize>
				</plx:Variable>
				<plx:Try>
					<plx:Body>
						<xsl:for-each select="$AbsorbedObjects">
							<plx:Variable name="Created{@name}Dictionary" dataTypeName="Dictionary&lt;string, {@name}&gt;">
								<plx:Initialize>
									<plx:CallNew dataTypeName="Dictionary&lt;string, {@name}&gt;"/>
								</plx:Initialize>
							</plx:Variable>
						</xsl:for-each>
						<!-- First of two passes for deserialization -->
						<plx:Loop>
							<!-- This is the best simulation we can get for:
							while(myReader.Read() && continueLoop-->
							<plx:LoopTest apply="Before">
								<plx:Operator type="BooleanAnd">
									<plx:Left>
										<plx:CallInstance name="Read" type="MethodCall">
											<plx:CallObject>
												<plx:Value data="myReader" type="Parameter"/>
											</plx:CallObject>
										</plx:CallInstance>
									</plx:Left>
									<plx:Right>
										<plx:Value data="continueLoop" type="Local"/>
									</plx:Right>
								</plx:Operator>
							</plx:LoopTest>
							<plx:Body>
								<plx:Variable name="nodeType" dataTypeName="XmlNodeType" dataTypeQualifier="System.Xml">
									<plx:Initialize>
										<plx:CallInstance name="NodeType" type="Property">
											<plx:CallObject>
												<plx:Value data="myReader" type="Parameter"/>
											</plx:CallObject>
										</plx:CallInstance>
									</plx:Initialize>
								</plx:Variable>
								<plx:Condition>
									<!-- Process XmlNodeType.Element nodes -->
									<plx:Test>
										<plx:Operator type="Equality">
											<plx:Left>
												<plx:Value data="nodeType" type="Local"/>
											</plx:Left>
											<plx:Right>
												<plx:CallStatic name="Element" type="Property" dataTypeName="XmlNodeType"/>
											</plx:Right>
										</plx:Operator>
									</plx:Test>
									<plx:Body>
										<!-- string localName = myReader.LocalName-->
										<plx:Variable name="localName" dataTypeName="String" dataTypeQualifier="System">
											<plx:Initialize>
												<plx:CallInstance name="LocalName" type="Property">
													<plx:CallObject>
														<plx:Value data="myReader" type="Parameter"/>
													</plx:CallObject>
												</plx:CallInstance>
											</plx:Initialize>
										</plx:Variable>
										<xsl:for-each select="$AbsorbedObjects">
											<xsl:if test="position()=1">
												<xsl:call-template name="DeserializationFirstPassLoop"/>
											</xsl:if>
										</xsl:for-each>
									</plx:Body>
								</plx:Condition>
							</plx:Body>
						</plx:Loop>
						<!-- Reset the reader for another pass -->
						<plx:Operator type="Assign">
							<plx:Left>
								<plx:Value data="myReader" type="Local"/>
							</plx:Left>
							<plx:Right>
								<plx:CallStatic name="Create" dataTypeName="XmlReader" type="MethodCall" dataTypeQualifier="System.Xml">
									<plx:PassParam>
										<plx:Value data="reader" type="Parameter"/>
									</plx:PassParam>
									<plx:PassParam>
										<plx:Value data="readerSettings" type="Local"/>
									</plx:PassParam>
								</plx:CallStatic>
							</plx:Right>
						</plx:Operator>
						<plx:Operator type="Assign">
							<plx:Left>
								<plx:Value data="continueLoop" type="Local"/>
							</plx:Left>
							<plx:Right>
								<plx:TrueKeyword/>
							</plx:Right>
						</plx:Operator>
						<!-- Second of two passes for deserialization -->
						<plx:Loop>
							<!-- This is the best simulation we can get for:
							while(myReader.Read() && continueLoop-->
							<plx:LoopTest apply="Before">
								<plx:Operator type="BooleanAnd">
									<plx:Left>
										<plx:CallInstance name="Read" type="MethodCall">
											<plx:CallObject>
												<plx:Value data="myReader" type="Parameter"/>
											</plx:CallObject>
										</plx:CallInstance>
									</plx:Left>
									<plx:Right>
										<plx:Value data="continueLoop" type="Local"/>
									</plx:Right>
								</plx:Operator>
							</plx:LoopTest>
							<plx:Body>
								<plx:Variable name="nodeType" dataTypeName="XmlNodeType" dataTypeQualifier="System.Xml">
									<plx:Initialize>
										<plx:CallInstance name="NodeType" type="Property">
											<plx:CallObject>
												<plx:Value data="myReader" type="Parameter"/>
											</plx:CallObject>
										</plx:CallInstance>
									</plx:Initialize>
								</plx:Variable>
								<!-- Process references that were absorbed as attributes -->
									<plx:Condition>
										<plx:Test>
										<plx:Operator type="Equality">
											<plx:Left>
												<plx:Value data="nodeType" type="Local"/>
											</plx:Left>
												<plx:Right>
													<plx:CallStatic name="Element" type="Property" dataTypeName="XmlNodeType"/>
											</plx:Right>
											</plx:Operator>
										</plx:Test>
										<plx:Body>
											<!-- string localName = myReader.LocalName-->
											<plx:Variable name="localName" dataTypeName="String" dataTypeQualifier="System">
												<plx:Initialize>
													<plx:CallInstance name="LocalName" type="Property">
														<plx:CallObject>
															<plx:Value data="myReader" type="Parameter"/>
													</plx:CallObject>
													</plx:CallInstance>
												</plx:Initialize>
											</plx:Variable>
											<xsl:for-each select="$AbsorbedObjects">
												<xsl:if test="position()=1">
													<xsl:call-template name="DeserializationSecondPassLoop"/>
												</xsl:if>
											</xsl:for-each>
										</plx:Body>
									</plx:Condition>
									<!--<xsl:for-each select="ao:RelatedObject">
										<xsl:element name="tyTemp"/>
										<plx:Condition>
											<plx:Test>
												<plx:Operator type="Equality">
													<plx:Left>
														<plx:Value data="nodeType" type="Local"/>
													</plx:Left>
													<plx:Right>
														<plx:CallStatic name="Element" type="Property" dataTypeName="XmlNodeType"/>
													</plx:Right>
												</plx:Operator>
											</plx:Test>
										</plx:Condition>
									</xsl:for-each>-->
								</plx:Body>
							</plx:Loop>
						</plx:Body>
					<plx:Finally>
						<plx:CallInstance name="Close" type="MethodCall">
							<plx:CallObject>
								<plx:Value data="myReader" type="Local"/>
							</plx:CallObject>
						</plx:CallInstance>
						<plx:CallInstance name="Close" type="MethodCall">
							<plx:CallObject>
								<plx:Value data="reader" type="Parameter"/>
							</plx:CallObject>
						</plx:CallInstance>
					</plx:Finally>
				</plx:Try>
			</plx:Function>
		</plx:Class>
		<plx:Class name="ElementLoaderSchema" visibility="Public" static="true">
			<xsl:for-each select="$AbsorbedObjects">
				<xsl:variable name="ObjectName" select="@name"/>
				<plx:Field name="{$ObjectName}_Object" visibility="Public" const="true" dataTypeName="String" dataTypeQualifier="System">
					<plx:Initialize>
						<plx:String>
							<xsl:value-of select="$ObjectName"/>
						</plx:String>
					</plx:Initialize>
				</plx:Field>
				<xsl:for-each select="ao:AbsorbedObject">
					<plx:Field name="{$ObjectName}{@name}_Attribute" visibility="Public" const="true" dataTypeName="String" dataTypeQualifier="System">
						<plx:Initialize>
							<plx:String>
								<xsl:value-of select="@name"/>
							</plx:String>
						</plx:Initialize>
					</plx:Field>
				</xsl:for-each>
				<xsl:for-each select="ao:RelatedObject">
					<plx:Field name="{$ObjectName}{@oppositeRoleName}{@oppositeObjectName}_Reference" visibility="Public" const="true" dataTypeName="String" dataTypeQualifier="System">
						<plx:Initialize>
							<plx:String>
								<xsl:value-of select="@oppositeRoleName"/>
								<xsl:value-of select="@oppositeObjectName"/>
								<xsl:text>_Ref</xsl:text>
							</plx:String>
						</plx:Initialize>
					</plx:Field>
				</xsl:for-each>
			</xsl:for-each>
			<plx:Field name="myNames" visibility="Private" static="true" dataTypeName="ElementLoaderNameTable"/>
			<plx:Property name="Names" visibility="Public" static="true">
				<plx:Param dataTypeName="ElementLoaderNameTable" name="" type="RetVal"/>
				<plx:Get>
					<plx:Variable name="retVal" dataTypeName="ElementLoaderNameTable">
						<plx:Initialize>
							<plx:CallStatic type="Field" name="myNames" dataTypeName="ElementLoaderNameTable"/>
						</plx:Initialize>
					</plx:Variable>
					<plx:Condition>
						<plx:Test>
							<plx:Operator type="Equality">
								<plx:Left>
									<plx:Value data="retVal" type="Local"/>
								</plx:Left>
								<plx:Right>
									<plx:NullObjectKeyword/>
								</plx:Right>
							</plx:Operator>
						</plx:Test>
						<plx:Body>
							<plx:Operator type="Assign">
								<plx:Left>
									<plx:Value data="myNames" type="Local"/>
								</plx:Left>
								<plx:Right>
									<plx:CallNew type="New" dataTypeName="ElementLoaderNameTable"/>
								</plx:Right>
							</plx:Operator>
							<plx:Operator type="Assign">
								<plx:Left>
									<plx:Value data="retVal" type="Local"/>
								</plx:Left>
								<plx:Right>
									<plx:Value data="myNames" type="Local"/>
								</plx:Right>
							</plx:Operator>
						</plx:Body>
					</plx:Condition>
					<plx:Return>
						<plx:Value data="retVal" type="Local"/>
					</plx:Return>
				</plx:Get>
			</plx:Property>
		</plx:Class>
		<plx:Class name="ElementLoaderNameTable" visibility="Public">
			<plx:DerivesFromClass dataTypeName="NameTable" dataTypeQualifier="System.Xml"/>
			<!-- To ensure uniqueness for each element in the name table, we need to be careful about naming
			our objects, absorbed objects, and related objects.  The convention used here is:
			ao:Object : concat(@name, '_Object')
			ao:AbsorbedObject : concat(parent::@name, @name, '_Attribute')
			ao:RelatedObject : concat(parent::@name, @oppositeRoleName, @oppositeObjectName, '_Reference')
			-->
			<xsl:for-each select="$AbsorbedObjects">
				<xsl:variable name="ObjectName" select="@name"/>
				<plx:Field name="{$ObjectName}_Object" visibility="Public" readOnly="true" dataTypeName="String" dataTypeQualifier="System"/>
				<xsl:for-each select="ao:AbsorbedObject">
					<plx:Field name="{$ObjectName}{@name}_Attribute" visibility="Public" readOnly="true" dataTypeName="String" dataTypeQualifier="System"/>
				</xsl:for-each>
				<xsl:for-each select="ao:RelatedObject">
					<plx:Field name="{$ObjectName}{@oppositeRoleName}{@oppositeObjectName}_Reference" visibility="Public" readOnly="true" dataTypeName="String" dataTypeQualifier="System"/>
				</xsl:for-each>
			</xsl:for-each>
			<plx:Function name="ElementLoaderNameTable" ctor="true" visibility="Public">
				<plx:Initialize>
					<plx:CallInstance name="Blah" type="MethodCall">
						<plx:CallObject>
							<plx:ThisKeyword/>
						</plx:CallObject>
					</plx:CallInstance>
				</plx:Initialize>
				<xsl:for-each select="$AbsorbedObjects">
					<xsl:variable name="ObjectName" select="@name"/>
					<plx:Operator type="Assign">
						<plx:Left>
							<plx:CallInstance name="{@name}_Object" type="Field">
								<plx:CallObject>
									<plx:ThisKeyword/>
								</plx:CallObject>
							</plx:CallInstance>
						</plx:Left>
						<plx:Right>
							<plx:CallInstance name="Add" type="MethodCall">
								<plx:CallObject>
									<plx:ThisKeyword/>
								</plx:CallObject>
								<plx:PassParam>
									<plx:CallStatic name="{$ObjectName}_Object" dataTypeName="ElementLoaderSchema" type="Property"/>
								</plx:PassParam>
							</plx:CallInstance>
						</plx:Right>
					</plx:Operator>
					<xsl:for-each select="ao:AbsorbedObject">
						<plx:Operator type="Assign">
							<plx:Left>
								<plx:CallInstance name="{$ObjectName}{@name}_Attribute" type="Field">
									<plx:CallObject>
										<plx:ThisKeyword/>
									</plx:CallObject>
								</plx:CallInstance>
							</plx:Left>
							<plx:Right>
								<plx:CallInstance name="Add" type="MethodCall">
									<plx:CallObject>
										<plx:ThisKeyword/>
									</plx:CallObject>
									<plx:PassParam>
										<plx:CallStatic name="{$ObjectName}{@name}_Attribute" dataTypeName="ElementLoaderSchema" type="Property"/>
									</plx:PassParam>
								</plx:CallInstance>
							</plx:Right>
						</plx:Operator>
					</xsl:for-each>
					<xsl:for-each select="ao:RelatedObject">
						<plx:Operator type="Assign">
							<plx:Left>
								<plx:CallInstance name="{$ObjectName}{@oppositeRoleName}{@oppositeObjectName}_Reference" type="Field">
									<plx:CallObject>
										<plx:ThisKeyword/>
									</plx:CallObject>
								</plx:CallInstance>
							</plx:Left>
							<plx:Right>
								<plx:CallInstance name="Add" type="MethodCall">
									<plx:CallObject>
										<plx:ThisKeyword/>
									</plx:CallObject>
									<plx:PassParam>
										<plx:CallStatic name="{$ObjectName}{@oppositeRoleName}{@oppositeObjectName}_Reference" dataTypeName="ElementLoaderSchema" type="Property"/>
									</plx:PassParam>
								</plx:CallInstance>
							</plx:Right>
						</plx:Operator>
					</xsl:for-each>
				</xsl:for-each>
			</plx:Function>
		</plx:Class>
	</xsl:template>
	<xsl:template name="WriteRelatedObjectIdentifierToAttribute">
		<xsl:param name="RelatedObject"/>
		<xsl:param name="RelatedObjectCollection"/>
		<plx:CallInstance name="WriteAttributeString" type="MethodCall">
			<plx:CallObject>
				<plx:Value type="Parameter" data="writer"/>
			</plx:CallObject>
			<plx:PassParam passStyle="In">
				<plx:String>
					<xsl:value-of select="concat(@roleName,@oppositeRoleName,@oppositeObjectName,'_Ref')"/>
				</plx:String>
			</plx:PassParam>
			<plx:PassParam>
				<plx:CallStatic name="Concat" dataTypeName="String" dataTypeQualifier="System" type="MethodCall">
					<plx:PassParam>
						<plx:String>
							<xsl:value-of select="concat(@roleName,@oppositeRoleName,@oppositeObjectName)"/>
						</plx:String>
					</plx:PassParam>
					<plx:PassParam>
				<plx:CallInstance name="ToString">
					<plx:CallObject>
						<plx:CallInstance name="IndexOf" type="MethodCall">
							<plx:CallObject>
								<plx:Value data="{@oppositeObjectName}Collection" type="Local"/>
							</plx:CallObject>
							<plx:PassParam>
								<plx:Value data="a{@oppositeObjectName}" type="Local"/>
							</plx:PassParam>
						</plx:CallInstance>
					</plx:CallObject>
				</plx:CallInstance>
					</plx:PassParam>
				</plx:CallStatic>
			</plx:PassParam>
		</plx:CallInstance>
	</xsl:template>
	
	<xsl:template name="DeserializationFirstPassLoop">
		<xsl:param name="Fallback" select="false()"/>
		<xsl:choose>
			<xsl:when test="$Fallback">
				<plx:FallbackCondition>
					<xsl:call-template name="DeserializationFirstPassLoopContents">
						<xsl:with-param name="ObjectName" select="@name"/>
					</xsl:call-template>
				</plx:FallbackCondition>
			</xsl:when>
			<xsl:otherwise>
				<plx:Condition>
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
					<!--<plx:Test></plx:Test>
					<plx:Body></plx:Body>-->
					<plx:FallbackCondition>
						<plx:Test>
							<plx:Operator type="Equality">
								<plx:Left>
									<plx:Value data="nodeType" type="Local"/>
								</plx:Left>
								<plx:Right>
									<plx:CallStatic name="EndElement" dataTypeName="XmlNodeType" type="Property"/>
								</plx:Right>
							</plx:Operator>
						</plx:Test>
						<plx:Body>
							<plx:Operator type="Assign">
								<plx:Left>
									<plx:Value data="continueLoop" type="Local"/>
								</plx:Left>
								<plx:Right>
									<plx:FalseKeyword/>
								</plx:Right>
							</plx:Operator>
						</plx:Body>
					</plx:FallbackCondition>
				</plx:Condition>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="DeserializationFirstPassLoopContents">
		<xsl:variable name="ObjectName"/>
		<!--<plx:Condition>-->
		<plx:Test>
			<plx:CallStatic name="ReferenceEquals" type="MethodCall" dataTypeName="ElementLoaderSchema">
				<plx:PassParam>
					<plx:Value data="localName" type="Local"/>
				</plx:PassParam>
				<plx:PassParam>
					<plx:CallInstance name="{@name}_Object" type="Property">
						<plx:CallObject>
							<plx:Value data="deserializationTable" type="Local"/>
						</plx:CallObject>
					</plx:CallInstance>
				</plx:PassParam>
			</plx:CallStatic>
		</plx:Test>
		<plx:Body>
			<xsl:variable name="aoAbsorbedObjects">
				<xsl:copy-of select="ao:AbsorbedObject[@mandatory='true']"/>
			</xsl:variable>
			<xsl:for-each select="msxsl:node-set($aoAbsorbedObjects)/child::*">
				<plx:Variable name="local{@name}" dataTypeName="String" dataTypeQualifier="System">
					<plx:Initialize>
						<plx:CallInstance name="" type="ArrayIndexer">
							<plx:CallObject>
								<plx:Value data="myReader" type="Parameter"/>
							</plx:CallObject>
							<plx:PassParam>
								<plx:String>
									<xsl:value-of select="@name"/>
								</plx:String>
							</plx:PassParam>
						</plx:CallInstance>
					</plx:Initialize>
				</plx:Variable>

			</xsl:for-each>
			<plx:CallInstance name="Add">
				<plx:CallObject>
					<plx:Value data="Created{@name}Dictionary" type="Local"/>
				</plx:CallObject>
				<plx:PassParam>
					<plx:CallInstance name="{$ObjectUniqueIdentifier}" type="ArrayIndexer">
						<plx:CallObject>
							<plx:Value data="myReader" type="Local"/>
						</plx:CallObject>
						<plx:PassParam>
							<plx:String>id</plx:String>
						</plx:PassParam>
					</plx:CallInstance>
				</plx:PassParam>
				<plx:PassParam>
					<plx:CallInstance name="Create{@name}">
						<plx:CallObject>
							<plx:Value data="deserializationFactory" type="Local"/>
						</plx:CallObject>
						<xsl:for-each select="msxsl:node-set($aoAbsorbedObjects)/child::*">
							<plx:PassParam>
								<plx:Value data="local{@name}" type="Local"/>
							</plx:PassParam>
						</xsl:for-each>
					</plx:CallInstance>
				</plx:PassParam>
			</plx:CallInstance>
		</plx:Body>
		<!--</plx:Condition>-->
	</xsl:template>
	
	<xsl:template name="DeserializationSecondPassLoop">
		<xsl:param name="Fallback" select="false()"/>
		<xsl:choose>
			<xsl:when test="$Fallback">
				<!--<xsl:if test="(@multiplicity = 'ZeroToOne') or (@multiplicity = 'ExactlyOne')">-->
					<plx:FallbackCondition>
						<xsl:call-template name="DeserializationSecondPassLoopContents">
							<xsl:with-param name="ObjectName" select="@name"/>
						</xsl:call-template>
					</plx:FallbackCondition>
				<!--</xsl:if>-->
			</xsl:when>
			<xsl:otherwise>
				<plx:Condition>
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
					<!--<plx:Test></plx:Test>
					<plx:Body></plx:Body>-->
					<plx:FallbackCondition>
						<plx:Test>
							<plx:Operator type="Equality">
								<plx:Left>
									<plx:Value data="nodeType" type="Local"/>
								</plx:Left>
								<plx:Right>
									<plx:CallStatic name="EndElement" dataTypeName="XmlNodeType" type="Property"/>
								</plx:Right>
							</plx:Operator>
						</plx:Test>
						<plx:Body>
							<plx:Operator type="Assign">
								<plx:Left>
									<plx:Value data="continueLoop" type="Local"/>
								</plx:Left>
								<plx:Right>
									<plx:FalseKeyword/>
								</plx:Right>
							</plx:Operator>
						</plx:Body>
					</plx:FallbackCondition>
				</plx:Condition>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="DeserializationSecondPassLoopContents">
		<xsl:param name="ObjectName" select="'Unknown_Object'"/>
		<xsl:variable name="aoRelatedObjects">
			<xsl:copy-of select="ao:RelatedObject"/>
		</xsl:variable>
		<xsl:for-each select="msxsl:node-set($aoRelatedObjects)/child::*">
			<xsl:choose>
				<xsl:when test="(@multiplicity = 'ZeroToOne') or (@multiplicity = 'ExactlyOne')">
					<!--<plx:Condition>-->
						<plx:Test>
							<plx:CallStatic name="ReferenceEquals" type="MethodCall" dataTypeName="ElementLoaderSchema">
								<plx:PassParam>
									<plx:Value data="localName" type="Local"/>
								</plx:PassParam>
								<plx:PassParam>
									<plx:CallInstance name="{$ObjectName}_Object" type="Property">
										<plx:CallObject>
											<plx:Value data="deserializationTable" type="Local"/>
										</plx:CallObject>
									</plx:CallInstance>
								</plx:PassParam>
							</plx:CallStatic>
						</plx:Test>

						<plx:Body>
							<plx:Variable name="local{$ObjectName}" dataTypeName="{$ObjectName}">
								<plx:Initialize>
									<plx:CallInstance name="" type="ArrayIndexer">
										<plx:CallObject>
											<plx:Value data="Created{$ObjectName}Dictionary" type="Local"/>
										</plx:CallObject>
										<plx:PassParam>
											<plx:CallInstance name="" type="ArrayIndexer">
												<plx:CallObject>
													<plx:Value data="myReader" type="Local"/>
												</plx:CallObject>
												<plx:PassParam>
													<plx:String>
														<xsl:value-of select="$ObjectUniqueIdentifier"/>
													</plx:String>
												</plx:PassParam>
											</plx:CallInstance>
										</plx:PassParam>
									</plx:CallInstance>
								</plx:Initialize>
							</plx:Variable>
							<plx:Operator type="Assign">
								<plx:Left>
									<!-- TODO: First, the getters and setters for this property need to be created at
									the factory level.  Next, their names need to reflect the roles that the object types
									play.  It makes no sense to have Object1 Rs Object2 when there could be many
									interactions between those two Object Types.  Once the names for the relationships
									have been formalized, change the name here to reflect the update.  -->
									<plx:CallInstance name="{@oppositeObjectName}" type="Property">
										<plx:CallObject>
											<plx:Value data="local{$ObjectName}" type="Local"/>
										</plx:CallObject>
									</plx:CallInstance>
								</plx:Left>
								<plx:Right>
									<plx:CallInstance name="" type="ArrayIndexer">
										<plx:CallObject>
											<plx:Value data="Created{@oppositeObjectName}Dictionary" type="Local"/>
										</plx:CallObject>
										<plx:PassParam>
											<plx:CallInstance name="" type="ArrayIndexer">
												<plx:CallObject>
													<plx:Value data="myReader" type="Local"/>
												</plx:CallObject>
												<plx:PassParam>
													<plx:String>
														<xsl:value-of select="concat($ObjectName, @oppositeRoleName, @oppositeObjectName, '_Reference')"/>
													</plx:String>
												</plx:PassParam>
											</plx:CallInstance>
										</plx:PassParam>
									</plx:CallInstance>
								</plx:Right>
							</plx:Operator>
						</plx:Body>
					<!--</plx:Condition>-->
				</xsl:when>
				<xsl:otherwise>
					<plx:Test>
						<plx:CallStatic name="ReferenceEquals" type="MethodCall" dataTypeName="ElementLoaderSchema">
							<plx:PassParam>
								<plx:Value data="localName" type="Local"/>
							</plx:PassParam>
							<plx:PassParam>
								<plx:CallInstance name="{$ObjectName}{@oppositeRoleName}{@oppositeObjectName}_Reference" type="Property">
									<plx:CallObject>
										<plx:Value data="deserializationTable" type="Local"/>
									</plx:CallObject>
								</plx:CallInstance>
							</plx:PassParam>
						</plx:CallStatic>
					</plx:Test>
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