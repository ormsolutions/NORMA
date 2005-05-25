<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:plx="http://Schemas.Northface.edu/CodeGeneration/Plix"
    xmlns:se="http://Schemas.Northface.edu/Private/SerializationExtensions">
    <xsl:template match="se:CustomSerializedElements">
        <plx:Root xmlns:plx="http://Schemas.Northface.edu/CodeGeneration/Plix">
			<plx:Using name="System"/>
			<plx:Using name="System.Collections.Generic"/>
			<plx:Using name="System.Collections"/>
            <plx:Using name="Microsoft.VisualStudio.Modeling"/>
            <plx:Using name="Northface.Tools.ORM.Shell"/>
            <plx:Namespace name="Northface.Tools.ORM.ObjectModel">
                <xsl:apply-templates/>
            </plx:Namespace>
        </plx:Root>
    </xsl:template>
    <xsl:template match="se:Element">
        <xsl:variable name="ClassName" select="@Class"/>
        <plx:Class name="{$ClassName}" visibility="Public" partial="true">
            <plx:ImplementsInterface dataTypeName="IORMCustomSerializedElement"/>
            <plx:Function visibility="Protected" name="GetSupportedOperations">
                <plx:InterfaceMember dataTypeName="IORMCustomSerializedElement" member="GetSupportedOperations"/>
                <plx:Param name="" style="RetVal" dataTypeName="ORMCustomSerializedElementSupportedOperations" dataTypeQualifier="Northface.Tools.ORM.Shell"/>
                <plx:Return>
					<xsl:call-template name="ReturnORMCustomSerializedElementSupportedOperations">
						<xsl:with-param name="combinedElements" select="count(se:CombinedElement)"/>
						<xsl:with-param name="element" select="count(@Prefix)+count(@Name)+count(@Namespace)+count(@WriteStyle)+count(@DoubleTagName)+count(se:ConditionalName)"/>
						<xsl:with-param name="attributes" select="count(se:Attribute)"/>
						<xsl:with-param name="links" select="count(se:Link)"/>
						<xsl:with-param name="customSort" select="@SortChildElements='true'"/>
					</xsl:call-template>
                </plx:Return>
            </plx:Function>
            <plx:Function visibility="Protected" name="HasMixedTypedAttributes">
                <plx:InterfaceMember dataTypeName="IORMCustomSerializedElement" member="HasMixedTypedAttributes"/>
                <plx:Param name="" style="RetVal" dataTypeName="Boolean" dataTypeQualifier="System"/>
                <plx:Return>
                    <xsl:choose>
                        <xsl:when test="@HasMixedTypedAttributes='true'">
                            <plx:TrueKeyword/>
                        </xsl:when>
                        <xsl:otherwise>
                            <plx:FalseKeyword/>
                        </xsl:otherwise>
                    </xsl:choose>
                </plx:Return>
            </plx:Function>
            <plx:Function visibility="Protected" name="GetCustomSerializedCombinedElementInfo">
                <plx:InterfaceMember dataTypeName="IORMCustomSerializedElement" member="GetCustomSerializedCombinedElementInfo"/>
                <plx:Param name="" style="RetVal" dataTypeName="ORMCustomSerializedCombinedElementInfo[]" dataTypeQualifier="Northface.Tools.ORM.Shell"/>
                <xsl:choose>
                    <xsl:when test="count(se:CombinedElement)">
                        <plx:Variable name="ret" dataTypeQualifier="Northface.Tools.ORM.Shell" dataTypeName="ORMCustomSerializedCombinedElementInfo" dataTypeIsSimpleArray="true" const="true">
                            <plx:Initialize>
                                <plx:CallNew style="New" dataTypeName="ORMCustomSerializedCombinedElementInfo" dataTypeQualifier="Northface.Tools.ORM.Shell" dataTypeIsSimpleArray="true">
                                    <plx:PassParam>
                                        <plx:Value type="I4">
                                            <xsl:value-of select="count(se:CombinedElement)"/>
                                        </plx:Value>
                                    </plx:PassParam>
                                </plx:CallNew>
                            </plx:Initialize>
                        </plx:Variable>
                        <xsl:for-each select="se:CombinedElement">
                            <xsl:variable name="index" select="position()-1"/>
                            <plx:Variable name="guids{$index}" dataTypeQualifier="System" dataTypeName="Guid" dataTypeIsSimpleArray="true" const="true">
                                <plx:Initialize>
                                    <plx:CallNew style="New" dataTypeName="Guid" dataTypeQualifier="System" dataTypeIsSimpleArray="true">
                                        <plx:PassParam>
                                            <plx:Value type="I4">
                                                <xsl:value-of select="count(se:Link)"/>
                                            </plx:Value>
                                        </plx:PassParam>
                                    </plx:CallNew>
                                </plx:Initialize>
                            </plx:Variable>
                            <xsl:for-each select="se:Link">
                                <plx:Operator name="Assign">
                                    <plx:Left>
                                        <plx:CallInstance name="" style="ArrayIndexer">
                                            <plx:CallObject>
                                                <plx:Value type="Local">guids<xsl:value-of select="$index"/></plx:Value>
                                            </plx:CallObject>
                                            <plx:PassParam>
                                                <plx:Value type="Local">
                                                    <xsl:value-of select="position()-1"/>
                                                </plx:Value>
                                            </plx:PassParam>
                                        </plx:CallInstance>
                                    </plx:Left>
                                    <plx:Right>
                                        <plx:Value type="Local">
                                            <xsl:value-of select="@RelationshipName"/>
                                            <xsl:text>.</xsl:text>
                                            <xsl:value-of select="@RoleName"/>
                                            <xsl:text>MetaRoleGuid</xsl:text>
                                        </plx:Value>
                                    </plx:Right>
                                </plx:Operator>
                            </xsl:for-each>
                            <plx:Operator name="Assign">
                                <plx:Left>
                                    <plx:CallInstance name="" style="ArrayIndexer">
                                        <plx:CallObject>
                                            <plx:Value type="Local">ret</plx:Value>
                                        </plx:CallObject>
                                        <plx:PassParam>
                                            <plx:Value type="Local">
                                                <xsl:value-of select="$index"/>
                                            </plx:Value>
                                        </plx:PassParam>
                                    </plx:CallInstance>
                                </plx:Left>
                                <plx:Right>
                                    <plx:CallNew style="New" dataTypeQualifier="Northface.Tools.ORM.Shell" dataTypeName="ORMCustomSerializedCombinedElementInfo">
                                        <plx:PassParam>
                                            <plx:String>
                                                <xsl:value-of select="@Name"/>
                                            </plx:String>
                                        </plx:PassParam>
                                        <plx:PassParam>
                                            <plx:Value type="Local">guids<xsl:value-of select="$index"/></plx:Value>
                                        </plx:PassParam>
                                    </plx:CallNew>
                                </plx:Right>
                            </plx:Operator>
                        </xsl:for-each>
                        <plx:Return>
                            <plx:Value type="Local">ret</plx:Value>
                        </plx:Return>
                    </xsl:when>
                    <xsl:otherwise>
                        <plx:Throw>
                            <plx:CallNew style="New" dataTypeQualifier="System" dataTypeName="NotSupportedException"/>
                        </plx:Throw>
                    </xsl:otherwise>
                </xsl:choose>
            </plx:Function>
            <plx:Function visibility="Protected" name="GetCustomSerializedElementInfo">
                <plx:InterfaceMember dataTypeName="IORMCustomSerializedElement" member="GetCustomSerializedElementInfo"/>
                <plx:Param name="" style="RetVal" dataTypeName="ORMCustomSerializedElementInfo" dataTypeQualifier="Northface.Tools.ORM.Shell"/>
                <xsl:choose>
                    <xsl:when test="count(@Prefix)+count(@Name)+count(@Namespace)+count(@WriteStyle)+count(@DoubleTagName)+count(se:ConditionalName)">
                        <xsl:call-template name="ReturnORMCustomSerializedElementInfo"/>
                    </xsl:when>
                    <xsl:otherwise>
                        <plx:Throw>
                            <plx:CallNew style="New" dataTypeQualifier="System" dataTypeName="NotSupportedException"/>
                        </plx:Throw>
                    </xsl:otherwise>
                </xsl:choose>
            </plx:Function>
            <plx:Function visibility="Protected" name="GetCustomSerializedAttributeInfo">
                <plx:InterfaceMember dataTypeName="IORMCustomSerializedElement" member="GetCustomSerializedAttributeInfo"/>
                <plx:Param name="" style="RetVal" dataTypeName="ORMCustomSerializedAttributeInfo" dataTypeQualifier="Northface.Tools.ORM.Shell"/>
                <plx:Param name="attributeInfo" dataTypeName="MetaAttributeInfo" dataTypeQualifier="Microsoft.VisualStudio.Modeling"></plx:Param>
                <plx:Param name="rolePlayedInfo" dataTypeName="MetaRoleInfo" dataTypeQualifier="Microsoft.VisualStudio.Modeling"></plx:Param>
                <xsl:choose>
                    <xsl:when test="count(se:Attribute)">
                        <xsl:for-each select="se:Attribute">
                            <plx:Condition>
                                <plx:Test>
                                    <plx:Operator name="Equality">
                                        <plx:Left>
                                            <plx:CallInstance style="Property" name="Id">
                                                <plx:CallObject>
                                                    <plx:Value type="Parameter">attributeInfo</plx:Value>
                                                </plx:CallObject>
                                            </plx:CallInstance>
                                        </plx:Left>
                                        <plx:Right>
                                            <plx:CallType style="Field" name="{@ID}MetaAttributeGuid" dataTypeName="{$ClassName}" />
                                        </plx:Right>
                                    </plx:Operator>
                                </plx:Test>
                                <plx:Body>
                                    <xsl:for-each select="se:RolePlayed">
                                        <plx:Condition>
                                            <plx:Test>
                                                <plx:Operator name="Equality">
                                                    <plx:Left>
                                                        <plx:CallInstance style="Property" name="Id">
                                                            <plx:CallObject>
                                                                <plx:Value type="Parameter">rolePlayedInfo</plx:Value>
                                                            </plx:CallObject>
                                                        </plx:CallInstance>
                                                    </plx:Left>
                                                    <plx:Right>
                                                        <plx:CallType style="Field" name="{@ID}MetaRoleGuid" dataTypeName="{$ClassName}" />
                                                    </plx:Right>
                                                </plx:Operator>
                                            </plx:Test>
                                            <plx:Body>
                                                <xsl:call-template name="ReturnORMCustomSerializedAttributeInfo"/>
                                            </plx:Body>
                                        </plx:Condition>
                                    </xsl:for-each>
                                    <xsl:call-template name="ReturnORMCustomSerializedAttributeInfo"/>
                                </plx:Body>
                            </plx:Condition>
                        </xsl:for-each>
                        <plx:Return>
                            <plx:Value type="Local">ORMCustomSerializedAttributeInfo.Default</plx:Value>
                        </plx:Return>
                    </xsl:when>
                    <xsl:otherwise>
                        <plx:Throw>
                            <plx:CallNew style="New" dataTypeQualifier="System" dataTypeName="NotSupportedException"/>
                        </plx:Throw>
                    </xsl:otherwise>
                </xsl:choose>
            </plx:Function>
            <plx:Function visibility="Protected" name="GetCustomSerializedLinkInfo">
                <plx:InterfaceMember dataTypeName="IORMCustomSerializedElement" member="GetCustomSerializedLinkInfo"/>
                <plx:Param name="" style="RetVal" dataTypeName="ORMCustomSerializedElementInfo" dataTypeQualifier="Northface.Tools.ORM.Shell"/>
                <plx:Param name="rolePlayedInfo" dataTypeName="MetaRoleInfo" dataTypeQualifier="Microsoft.VisualStudio.Modeling"></plx:Param>
                <xsl:choose>
                    <xsl:when test="count(se:Link)">
                        <xsl:for-each select="se:Link">
                            <plx:Condition>
                                <plx:Test>
                                    <plx:Operator name="Equality">
                                        <plx:Left>
                                            <plx:CallInstance style="Property" name="Id">
                                                <plx:CallObject>
                                                    <plx:Value type="Parameter">rolePlayedInfo</plx:Value>
                                                </plx:CallObject>
                                            </plx:CallInstance>
                                        </plx:Left>
                                        <plx:Right>
                                            <plx:CallType style="Field" name="{@RoleName}MetaRoleGuid" dataTypeName="{@RelationshipName}" />
                                        </plx:Right>
                                    </plx:Operator>
                                </plx:Test>
                                <plx:Body>
                                    <xsl:call-template name="ReturnORMCustomSerializedElementInfo"/>
                                </plx:Body>
                            </plx:Condition>
                        </xsl:for-each>
                        <plx:Return>
                            <plx:Value type="Local">ORMCustomSerializedElementInfo.Default</plx:Value>
                        </plx:Return>
                    </xsl:when>
                    <xsl:otherwise>
                        <plx:Throw>
                            <plx:CallNew style="New" dataTypeQualifier="System" dataTypeName="NotSupportedException"/>
                        </plx:Throw>
                    </xsl:otherwise>
                </xsl:choose>
            </plx:Function>
			<xsl:choose>
				<xsl:when test="@SortChildElements='true'">
					<plx:Field name="myCustomSortChildComparer" shared="true" visibility="Private" dataTypeName="IComparer"/>
					<plx:Class name="CustomSortChildComparer" visibility="Private">
						<plx:ImplementsInterface dataTypeName="IComparer"/>
						<plx:Field name="myRoleOrderDictionary" visibility="Private" dataTypeName="Dictionary">
							<plx:PassTypeParam dataTypeName="MetaRoleInfo"/>
							<plx:PassTypeParam dataTypeName="Int32" dataTypeQualifier="System"/>
						</plx:Field>
						<plx:Function ctor="true" name="" visibility="Public">
							<plx:Param name="store" dataTypeName="Store" style="In"/>
							<xsl:variable name="SortedLevels">
								<!-- Define a variable with structure <SortLevel><Role/><SortLevel/> -->
								<xsl:for-each select="se:Link | se:CombinedElement">
									<xsl:if test="not(@NotSorted='true')">
										<xsl:choose>
											<xsl:when test="local-name()='Link'">
												<SortLevel><Role RelationshipName="{@RelationshipName}" RoleName="{@RoleName}"/></SortLevel>
											</xsl:when>
											<xsl:when test="local-name()='CombinedElement'">
												<xsl:choose>
													<xsl:when test="@SortChildElements='true'">
														<!-- Add one sort level for each child -->
														<xsl:for-each select="se:Link">
															<SortLevel><Role RelationshipName="{@RelationshipName}" RoleName="{@RoleName}"/></SortLevel>
														</xsl:for-each>
													</xsl:when>
													<xsl:otherwise>
														<!-- Add one sort level for all children -->
														<SortLevel>
															<xsl:for-each select="se:Link">
																<Role RelationshipName="{@RelationshipName}" RoleName="{@RoleName}"/>
															</xsl:for-each>
														</SortLevel>
													</xsl:otherwise>
												</xsl:choose>
											</xsl:when>
										</xsl:choose>
									</xsl:if>
								</xsl:for-each>
							</xsl:variable>
							<plx:Variable name="metaDataDir" dataTypeName="MetaDataDirectory">
								<plx:Initialize>
									<plx:CallInstance name="MetaDataDirectory" style="Property"><plx:CallObject><plx:Value type="Parameter">store</plx:Value></plx:CallObject></plx:CallInstance>
								</plx:Initialize>
							</plx:Variable>
							<plx:Variable name="roleOrderDictionary" dataTypeName="Dictionary">
								<plx:PassTypeParam dataTypeName="MetaRoleInfo"/>
								<plx:PassTypeParam dataTypeName="Int32" dataTypeQualifier="System"/>
								<plx:Initialize>
									<plx:CallNew dataTypeName="Dictionary">
										<plx:PassTypeParam dataTypeName="MetaRoleInfo"/>
										<plx:PassTypeParam dataTypeName="Int32" dataTypeQualifier="System"/>
									</plx:CallNew>
								</plx:Initialize>
							</plx:Variable>
							<plx:Variable name="metaRole" dataTypeName="MetaRoleInfo"/>
							<xsl:for-each select="$SortedLevels/SortLevel">
								<xsl:variable name="level" select="position()-1"/>
								<xsl:for-each select="Role">
									<plx:Operator name="Assign">
										<plx:Left><plx:Value type="Local">metaRole</plx:Value></plx:Left>
										<plx:Right>
											<plx:CallInstance name="FindMetaRole">
												<plx:CallObject><plx:Value type="Local">metaDataDir</plx:Value></plx:CallObject>
												<plx:PassParam>
													<plx:CallType dataTypeName="{@RelationshipName}" name="{@RoleName}MetaRoleGuid" style="Field"/>
												</plx:PassParam>
											</plx:CallInstance>
										</plx:Right>
									</plx:Operator>
									<plx:Operator name="Assign">
										<plx:Left>
											<plx:CallInstance name="" style="Indexer">
												<plx:CallObject><plx:Value type="Local">roleOrderDictionary</plx:Value></plx:CallObject>
												<plx:PassParam>
													<plx:CallInstance name="OppositeMetaRole" style="Property">
														<plx:CallObject><plx:Value type="Local">metaRole</plx:Value></plx:CallObject>
													</plx:CallInstance>
												</plx:PassParam>
											</plx:CallInstance>
										</plx:Left>
										<plx:Right>
											<plx:Value type="I4"><xsl:value-of select="$level"/></plx:Value>
										</plx:Right>
									</plx:Operator>
								</xsl:for-each>
							</xsl:for-each>
							<plx:Operator name="Assign">
								<plx:Left>
									<plx:CallInstance name="myRoleOrderDictionary" style="Field"><plx:CallObject><plx:ThisKeyword/></plx:CallObject></plx:CallInstance>
								</plx:Left>
								<plx:Right>
									<plx:Value type="Local">roleOrderDictionary</plx:Value>
								</plx:Right>
							</plx:Operator>
						</plx:Function>
						<plx:Function visibility="Private" name="Compare">
							<plx:InterfaceMember dataTypeName="IComparer" member="Compare"/>
							<plx:Param style="RetVal" name="" dataTypeName="Int32" dataTypeQualifier="System"/>
							<plx:Param style="In" name="x" dataTypeName="Object" dataTypeQualifier="System"/>
							<plx:Param style="In" name="y" dataTypeName="Object" dataTypeQualifier="System"/>
							<xsl:variable name="paramVals">
								<Value>x</Value>
								<Value>y</Value>
							</xsl:variable>
							<xsl:for-each select="$paramVals/child::*">
								<plx:Variable name="{.}Pos" dataTypeName="Int32" dataTypeQualifier="System"/>
								<plx:Condition>
									<plx:Test>
										<plx:Operator name="BooleanNot">
											<plx:CallInstance name="TryGetValue">
												<plx:CallObject>
													<plx:CallInstance name="myRoleOrderDictionary" style="Field">
														<plx:CallObject>
															<plx:ThisKeyword/>
														</plx:CallObject>
													</plx:CallInstance>
												</plx:CallObject>
												<plx:PassParam passStyle="In">
													<plx:Cast style="TypeCastException">
														<plx:TargetType dataTypeName="MetaRoleInfo"/>
														<plx:CastExpression>
															<plx:Value type="Parameter"><xsl:value-of select="."/></plx:Value>
														</plx:CastExpression>
													</plx:Cast>
												</plx:PassParam>
												<plx:PassParam passStyle="Out">
													<plx:Value type="Local">
														<xsl:value-of select="."/>
														<xsl:text>Pos</xsl:text>
													</plx:Value>
												</plx:PassParam>
											</plx:CallInstance>
										</plx:Operator>
									</plx:Test>
									<plx:Body>
										<plx:Operator name="Assign">
											<plx:Left>
												<plx:Value type="Local">
													<xsl:value-of select="."/>
													<xsl:text>Pos</xsl:text>
												</plx:Value>
											</plx:Left>
											<plx:Right>
												<plx:CallType dataTypeName="Int32" dataTypeQualifier="System" name="MaxValue" style="Field"/>
											</plx:Right>
										</plx:Operator>
									</plx:Body>
								</plx:Condition>
							</xsl:for-each>
							<plx:Condition>
								<plx:Test>
									<plx:Operator name="Equality">
										<plx:Left><plx:Value type="Local">xPos</plx:Value></plx:Left>
										<plx:Right><plx:Value type="Local">yPos</plx:Value></plx:Right>
									</plx:Operator>
								</plx:Test>
								<plx:Body>
									<plx:Return><plx:Value type="I4">0</plx:Value></plx:Return>
								</plx:Body>
								<plx:FallbackCondition>
									<plx:Test>
										<plx:Operator name="LessThan">
											<plx:Left><plx:Value type="Local">xPos</plx:Value></plx:Left>
											<plx:Right><plx:Value type="Local">yPos</plx:Value></plx:Right>
										</plx:Operator>
									</plx:Test>
									<plx:Body>
										<plx:Return><plx:Value type="I4">-1</plx:Value></plx:Return>
									</plx:Body>
								</plx:FallbackCondition>
							</plx:Condition>
							<plx:Return><plx:Value type="I4">1</plx:Value></plx:Return>
						</plx:Function>
					</plx:Class>
					<plx:Function visibility="Protected" name="SortCustomSerializedChildRoles">
						<plx:InterfaceMember dataTypeName="IORMCustomSerializedElement" member="SortCustomSerializedChildRoles"/>
						<plx:Param name="playedMetaRoles" dataTypeName="MetaRoleInfo" dataTypeIsSimpleArray="true"/>
						<plx:Condition>
							<plx:Test>
								<plx:Operator name="IdentityEquality">
									<plx:Left><plx:NullObjectKeyword/></plx:Left>
									<plx:Right><plx:CallType dataTypeName="{$ClassName}" name="myCustomSortChildComparer" style="Field"/></plx:Right>
								</plx:Operator>
							</plx:Test>
							<plx:Body>
								<plx:Operator name="Assign">
									<plx:Left><plx:CallType dataTypeName="{$ClassName}" name="myCustomSortChildComparer" style="Field"/></plx:Left>
									<plx:Right>
										<plx:CallNew dataTypeName="CustomSortChildComparer">
											<plx:PassParam>
												<plx:CallInstance name="Store" style="Property">
													<plx:CallObject><plx:ThisKeyword/></plx:CallObject>
												</plx:CallInstance>
											</plx:PassParam>
										</plx:CallNew>
									</plx:Right>
								</plx:Operator>
							</plx:Body>
						</plx:Condition>
						<plx:CallType dataTypeName="Array" name="Sort">
							<plx:PassParam>
								<plx:Value type="Parameter">playedMetaRoles</plx:Value>
							</plx:PassParam>
							<plx:PassParam>
								<plx:CallType dataTypeName="{$ClassName}" name="myCustomSortChildComparer" style="Field"></plx:CallType>
							</plx:PassParam>
						</plx:CallType>
					</plx:Function>
				</xsl:when>
				<xsl:otherwise>
					<plx:Function visibility="Protected" name="SortCustomSerializedChildRoles">
						<plx:InterfaceMember dataTypeName="IORMCustomSerializedElement" member="SortCustomSerializedChildRoles"/>
						<plx:Param name="playedMetaRoles" dataTypeName="MetaRoleInfo" dataTypeIsSimpleArray="true"/>
					</plx:Function>
				</xsl:otherwise>
			</xsl:choose>
        </plx:Class>
    </xsl:template>
    <xsl:template match="se:Namespaces">
        <plx:Class name="{@Class}" visibility="Public" partial="true">
            <plx:ImplementsInterface dataTypeName="IORMCustomElementNamespace"/>
            <plx:Function visibility="Protected" name="GetCustomElementNamespaces">
                <plx:InterfaceMember dataTypeName="IORMCustomElementNamespace" member="GetCustomElementNamespaces"/>
                <plx:Param name="" style="RetVal" dataTypeName="String" dataTypeQualifier="System">
                    <plx:ArrayDescriptor rank="2"/>
                </plx:Param>
                <plx:Variable name="ret" dataTypeQualifier="System" dataTypeName="String" const="true">
                    <plx:ArrayDescriptor rank="2"/>
                    <plx:Initialize>
                        <plx:CallNew style="New" dataTypeName="String" dataTypeQualifier="System">
                            <plx:ArrayDescriptor rank="2"/>
                            <plx:PassParam>
                                <plx:Value type="I4">
                                    <xsl:value-of select="count(se:Namespace)"/>
                                </plx:Value>
                            </plx:PassParam>
                            <plx:PassParam>
                                <plx:Value type="I4">2</plx:Value>
                            </plx:PassParam>
                        </plx:CallNew>
                    </plx:Initialize>
                </plx:Variable>
                <xsl:for-each select="se:Namespace">
                    <plx:Operator name="Assign">
                        <plx:Left>
                            <plx:CallInstance name="" style="ArrayIndexer">
                                <plx:CallObject>
                                    <plx:Value type="Local">ret</plx:Value>
                                </plx:CallObject>
                                <plx:PassParam>
                                    <plx:Value type="Local">
                                        <xsl:value-of select="position()-1"/>
                                    </plx:Value>
                                </plx:PassParam>
                                <plx:PassParam>
                                    <plx:Value type="Local">0</plx:Value>
                                </plx:PassParam>
                            </plx:CallInstance>
                        </plx:Left>
                        <plx:Right>
                            <plx:String>
                                <xsl:value-of select="@Prefix"/>
                            </plx:String>
                        </plx:Right>
                    </plx:Operator>
                    <plx:Operator name="Assign">
                        <plx:Left>
                            <plx:CallInstance name="" style="ArrayIndexer">
                                <plx:CallObject>
                                    <plx:Value type="Local">ret</plx:Value>
                                </plx:CallObject>
                                <plx:PassParam>
                                    <plx:Value type="Local">
                                        <xsl:value-of select="position()-1"/>
                                    </plx:Value>
                                </plx:PassParam>
                                <plx:PassParam>
                                    <plx:Value type="Local">1</plx:Value>
                                </plx:PassParam>
                            </plx:CallInstance>
                        </plx:Left>
                        <plx:Right>
                            <plx:String>
                                <xsl:value-of select="@URI"/>
                            </plx:String>
                        </plx:Right>
                    </plx:Operator>
                </xsl:for-each>
                <plx:Return>
                    <plx:Value type="Local">ret</plx:Value>
                </plx:Return>
            </plx:Function>
        </plx:Class>
    </xsl:template>
    <xsl:template name="ReturnORMCustomSerializedElementSupportedOperations">
        <xsl:param name="combinedElements" select="0"/>
        <xsl:param name="element" select="0"/>
        <xsl:param name="attributes" select="0"/>
        <xsl:param name="links" select="0"/>
		<xsl:param name="customSort" select="0"/>
		<xsl:variable name="supportedOperations">
			<xsl:if test="$combinedElements">
				<xsl:element name="SupportedOperation">
					<xsl:text>CustomSerializedCombinedElementInfo</xsl:text>
				</xsl:element>
			</xsl:if>
			<xsl:if test="$element">
				<xsl:element name="SupportedOperation">
					<xsl:text>CustomSerializedElementInfo</xsl:text>
				</xsl:element>
			</xsl:if>
			<xsl:if test="$attributes">
				<xsl:element name="SupportedOperation">
					<xsl:text>CustomSerializedAttributeInfo</xsl:text>
				</xsl:element>
			</xsl:if>
			<xsl:if test="$links">
				<xsl:element name="SupportedOperation">
					<xsl:text>CustomSerializedLinkInfo</xsl:text>
				</xsl:element>
			</xsl:if>
			<xsl:if test="$customSort">
				<xsl:element name="SupportedOperation">
					<xsl:text>CustomSortChildRoles</xsl:text>
				</xsl:element>
			</xsl:if>
		</xsl:variable>
		<xsl:variable name="operationCount" select="count($supportedOperations/child::*)"/>
		<xsl:choose>
			<xsl:when test="$operationCount=0">
				<plx:CallType dataTypeName="ORMCustomSerializedElementSupportedOperations" name="None" style="Field"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:for-each select="$supportedOperations/SupportedOperation">
					<xsl:if test="position()=1">
						<xsl:call-template name="OrTogetherEnumElements">
							<xsl:with-param name="EnumType" select="'ORMCustomSerializedElementSupportedOperations'"></xsl:with-param>
						</xsl:call-template>
					</xsl:if>
				</xsl:for-each>
			</xsl:otherwise>
		</xsl:choose>
    </xsl:template>
	<!-- Or together enum values from the given type. The current state on the initial
	     call should be the position()=1 element inside a for-each context where the elements
		 contain the (unqualified) names of the enum values to or together -->
	<xsl:template name="OrTogetherEnumElements">
		<xsl:param name="EnumType"/>
		<xsl:choose>
			<xsl:when test="position()=last()">
				<plx:CallType dataTypeName="{$EnumType}" name="{.}" style="Field"/>
			</xsl:when>
			<xsl:otherwise>
				<plx:Operator name="BitwiseOr">
					<plx:Left>
						<plx:CallType dataTypeName="{$EnumType}" name="{.}" style="Field"/>
					</plx:Left>
					<plx:Right>
						<xsl:for-each select="following-sibling::*">
							<xsl:if test="position()=1">
								<xsl:call-template name="OrTogetherEnumElements">
									<xsl:with-param name="EnumType" select="$EnumType"/>
								</xsl:call-template>
							</xsl:if>
						</xsl:for-each>							
					</plx:Right>
				</plx:Operator>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
    <xsl:template name="ReturnORMCustomSerializedElementInfo">
        <xsl:if test="count(se:ConditionalName)">
            <plx:Variable dataTypeName="String" dataTypeQualifier="System" name="name">
                <plx:Initialize>
                    <xsl:choose>
                        <xsl:when test="string-length(@Name)">
                            <plx:String>
                                <xsl:value-of select="@Name"/>
                            </plx:String>
                        </xsl:when>
                        <xsl:otherwise>
                            <plx:NullObjectKeyword/>
                        </xsl:otherwise>
                    </xsl:choose>
                </plx:Initialize>
            </plx:Variable>
            <xsl:for-each select="se:ConditionalName">
                <xsl:if test="position()=1">
                    <plx:Condition>
                        <plx:Test>
                            <xsl:copy-of select="child::*"/>
                        </plx:Test>
                        <plx:Body>
                            <plx:Operator name="Assign">
                                <plx:Left>
                                    <plx:Value type="Local">name</plx:Value>
                                </plx:Left>
                                <plx:Right>
                                    <plx:String>
                                        <xsl:value-of select="@Name"/>
                                    </plx:String>
                                </plx:Right>
                            </plx:Operator>
                        </plx:Body>
                        <xsl:for-each select="following-sibling::se:ConditionalName">
                            <plx:FallbackCondition>
                                <plx:Test>
                                    <xsl:copy-of select="child::*"/>
                                </plx:Test>
                                <plx:Body>
                                    <plx:Operator name="Assign">
                                        <plx:Left>
                                            <plx:Value type="Local">name</plx:Value>
                                        </plx:Left>
                                        <plx:Right>
                                            <plx:String>
                                                <xsl:value-of select="@Name"/>
                                            </plx:String>
                                        </plx:Right>
                                    </plx:Operator>
                                </plx:Body>
                            </plx:FallbackCondition>
                        </xsl:for-each>
                    </plx:Condition>
                </xsl:if>
            </xsl:for-each>
        </xsl:if>
        <plx:Return>
            <plx:CallNew style="New" dataTypeName="ORMCustomSerializedElementInfo">
                <plx:PassParam>
                    <xsl:choose>
                        <xsl:when test="string-length(@Prefix)">
                            <plx:String>
                                <xsl:value-of select="@Prefix"/>
                            </plx:String>
                        </xsl:when>
                        <xsl:otherwise>
                            <plx:NullObjectKeyword/>
                        </xsl:otherwise>
                    </xsl:choose>
                </plx:PassParam>
                <plx:PassParam>
                    <xsl:choose>
                        <xsl:when test="count(se:ConditionalName)">
                            <plx:Value type="Local">name</plx:Value>
                        </xsl:when>
                        <xsl:otherwise>
                            <xsl:choose>
                                <xsl:when test="string-length(@Name)">
                                    <plx:String>
                                        <xsl:value-of select="@Name"/>
                                    </plx:String>
                                </xsl:when>
                                <xsl:otherwise>
                                    <plx:NullObjectKeyword/>
                                </xsl:otherwise>
                            </xsl:choose>
                        </xsl:otherwise>
                    </xsl:choose>
                </plx:PassParam>
                <plx:PassParam>
                    <xsl:choose>
                        <xsl:when test="string-length(@Namespace)">
                            <plx:String>
                                <xsl:value-of select="@Namespace"/>
                            </plx:String>
                        </xsl:when>
                        <xsl:otherwise>
                            <plx:NullObjectKeyword/>
                        </xsl:otherwise>
                    </xsl:choose>
                </plx:PassParam>
                <plx:PassParam>
                    <plx:CallType style="Field" dataTypeName="ORMCustomSerializedElementWriteStyle">
                        <xsl:attribute name="name">
                            <xsl:choose>
                                <xsl:when test="string-length(@DoubleTagName)">
                                    <xsl:text>DoubleTaggedElement</xsl:text>
                                </xsl:when>
                                <xsl:otherwise>
                                    <xsl:choose>
                                        <xsl:when test="string-length(@WriteStyle)">
                                            <xsl:value-of select="@WriteStyle"/>
                                        </xsl:when>
                                        <xsl:otherwise>
                                            <xsl:text>Element</xsl:text>
                                        </xsl:otherwise>
                                    </xsl:choose>
                                </xsl:otherwise>
                            </xsl:choose>
                        </xsl:attribute>
                    </plx:CallType>
                </plx:PassParam>
                <plx:PassParam>
                    <xsl:choose>
                        <xsl:when test="string-length(@DoubleTagName)">
                            <plx:String>
                                <xsl:value-of select="@DoubleTagName"/>
                            </plx:String>
                        </xsl:when>
                        <xsl:otherwise>
                            <plx:NullObjectKeyword/>
                        </xsl:otherwise>
                    </xsl:choose>
                </plx:PassParam>
            </plx:CallNew>
        </plx:Return>
    </xsl:template>
    <xsl:template name="ReturnORMCustomSerializedAttributeInfo">
        <plx:Return>
            <plx:CallNew style="New" dataTypeName="ORMCustomSerializedAttributeInfo">
                <plx:PassParam>
                    <xsl:choose>
                        <xsl:when test="string-length(@Prefix)">
                            <plx:String>
                                <xsl:value-of select="@Prefix"/>
                            </plx:String>
                        </xsl:when>
                        <xsl:otherwise>
                            <plx:NullObjectKeyword/>
                        </xsl:otherwise>
                    </xsl:choose>
                </plx:PassParam>
                <plx:PassParam>
                    <xsl:choose>
                        <xsl:when test="string-length(@Name)">
                            <plx:String>
                                <xsl:value-of select="@Name"/>
                            </plx:String>
                        </xsl:when>
                        <xsl:otherwise>
                            <plx:NullObjectKeyword/>
                        </xsl:otherwise>
                    </xsl:choose>
                </plx:PassParam>
                <plx:PassParam>
                    <xsl:choose>
                        <xsl:when test="string-length(@Namespace)">
                            <plx:String>
                                <xsl:value-of select="@Namespace"/>
                            </plx:String>
                        </xsl:when>
                        <xsl:otherwise>
                            <plx:NullObjectKeyword/>
                        </xsl:otherwise>
                    </xsl:choose>
                </plx:PassParam>
                <plx:PassParam>
                    <xsl:choose>
                        <xsl:when test="@WriteCustomStorage='true'">
                            <plx:TrueKeyword/>
                        </xsl:when>
                        <xsl:otherwise>
                            <plx:FalseKeyword/>
                        </xsl:otherwise>
                    </xsl:choose>
                </plx:PassParam>
                <plx:PassParam>
                    <plx:CallType style="Field" dataTypeName="ORMCustomSerializedAttributeWriteStyle">
                        <xsl:attribute name="name">
                            <xsl:choose>
                                <xsl:when test="string-length(@DoubleTagName)">
                                    <xsl:text>DoubleTaggedElement</xsl:text>
                                </xsl:when>
                                <xsl:otherwise>
                                    <xsl:choose>
                                        <xsl:when test="string-length(@WriteStyle)">
                                            <xsl:value-of select="@WriteStyle"/>
                                        </xsl:when>
                                        <xsl:otherwise>
                                            <xsl:text>Attribute</xsl:text>
                                        </xsl:otherwise>
                                    </xsl:choose>
                                </xsl:otherwise>
                            </xsl:choose>
                        </xsl:attribute>
                    </plx:CallType>
                </plx:PassParam>
                <plx:PassParam>
                    <xsl:choose>
                        <xsl:when test="string-length(@DoubleTagName)">
                            <plx:String>
                                <xsl:value-of select="@DoubleTagName"/>
                            </plx:String>
                        </xsl:when>
                        <xsl:otherwise>
                            <plx:NullObjectKeyword/>
                        </xsl:otherwise>
                    </xsl:choose>
                </plx:PassParam>
            </plx:CallNew>
        </plx:Return>
    </xsl:template>
</xsl:stylesheet>