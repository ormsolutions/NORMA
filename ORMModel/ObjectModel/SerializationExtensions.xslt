<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:plx="http://Schemas.Northface.edu/CodeGeneration/Plix"
    xmlns:se="http://Schemas.Northface.edu/Private/SerializationExtensions">
    <xsl:template match="se:CustomSerializedElements">
        <plx:Root xmlns:plx="http://Schemas.Northface.edu/CodeGeneration/Plix">
            <plx:Using name="Microsoft.VisualStudio.Modeling"/>
            <plx:Using name="Northface.Tools.ORM.Shell"/>
			<plx:Using name="System"/>
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
                <xsl:variable name="supportedOperations">
                    <xsl:call-template name="ReturnORMCustomSerializedElementSupportedOperations">
                        <xsl:with-param name="combinedElements" select="count(se:CombinedElement)"/>
                        <xsl:with-param name="element" select="count(@Prefix)+count(@Name)+count(@Namespace)+count(@WriteStyle)+count(@DoubleTagName)+count(se:ConditionalName)"/>
                        <xsl:with-param name="attributes" select="count(se:Attribute)"/>
                        <xsl:with-param name="links" select="count(se:Link)"/>
                    </xsl:call-template>
                </xsl:variable>
                <plx:Return>
                    <plx:Value type="Local">
                        <xsl:for-each select="$supportedOperations">
                            <xsl:for-each select="SupportedOperation">
                                <xsl:value-of select="."/>
                                <xsl:if test="not(position()=last())">
                                    <xsl:text>|</xsl:text>
                                </xsl:if>
                            </xsl:for-each>
                        </xsl:for-each>
                    </plx:Value>
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
			<plx:Function visibility="Protected" name="SortCustomSerializedChildRoles">
                <plx:InterfaceMember dataTypeName="IORMCustomSerializedElement" member="SortCustomSerializedChildRoles"/>
				<plx:Param name="playedMetaRoles" dataTypeName="MetaRoleInfo" dataTypeIsSimpleArray="true"/>
			</plx:Function>
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
        <xsl:if test="$combinedElements">
            <xsl:element name="SupportedOperation">
                <xsl:text>ORMCustomSerializedElementSupportedOperations.CustomSerializedCombinedElementInfo</xsl:text>
            </xsl:element>
        </xsl:if>
        <xsl:if test="$element">
            <xsl:element name="SupportedOperation">
                <xsl:text>ORMCustomSerializedElementSupportedOperations.CustomSerializedElementInfo</xsl:text>
            </xsl:element>
        </xsl:if>
        <xsl:if test="$attributes">
            <xsl:element name="SupportedOperation">
                <xsl:text>ORMCustomSerializedElementSupportedOperations.CustomSerializedAttributeInfo</xsl:text>
            </xsl:element>
        </xsl:if>
        <xsl:if test="$links">
            <xsl:element name="SupportedOperation">
                <xsl:text>ORMCustomSerializedElementSupportedOperations.CustomSerializedLinkInfo</xsl:text>
            </xsl:element>
        </xsl:if>
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