<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet 
    version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:plx="http://Schemas.Northface.edu/CodeGeneration/Plix">
    <xsl:template match="DataTypes">
        <plx:Root xmlns:plx="http://Schemas.Northface.edu/CodeGeneration/Plix">
            <plx:Using name="System;&#13;&#10;///&lt;summary&gt;{comment}&lt;/summary&gt;&#13;&#10;[CLSCompliant(true)]//"/>
            <plx:Namespace name="Northface.Tools.ORM.ObjectModel">
                <plx:Enum name="PortableDataType" visibility="Public">
                    <xsl:for-each select="DataType">
                        <xsl:for-each select="SubType">
                            <plx:EnumItem name="///&lt;summary&gt;{comment}&lt;/summary&gt;&#13;&#10;{../@name}{@name}"/>
                        </xsl:for-each>
                    </xsl:for-each>
                </plx:Enum>
                <plx:Class name="ORMModel" visibility="Public" partial="true">
                    <plx:Class name="AddIntrinsicDataTypesFixupListener" visibility="Private" partial="true" sealed="true">
                        <plx:Variable name="typeArray" dataTypeName="Type" dataTypeQualifier="System" dataTypeIsSimpleArray="true" shared="true" visibility="Private">
                            <plx:Initialize>
                                <plx:Call name="Type" qualifier="System" style="NewArray">
                                    <plx:ArrayInitializer>
                                        <xsl:for-each select="DataType">
                                            <xsl:if test="not(@enumOnly)">
                                                <xsl:for-each select="SubType">
                                                    <plx:PassParam passStyle="In">
                                                        <plx:TypeOf dataTypeName="{@name}{../@name}DataType" />
                                                    </plx:PassParam>
                                                </xsl:for-each>
                                            </xsl:if>
                                        </xsl:for-each>
                                    </plx:ArrayInitializer>
                                </plx:Call>
                            </plx:Initialize>
                        </plx:Variable>
                    </plx:Class>
                </plx:Class>
                <xsl:for-each select="DataType">
                    <xsl:if test="not(@enumOnly)">
                            <xsl:for-each select="SubType">
                                <xsl:choose>
                                    <xsl:when test="not(@name = '')">
                                        <plx:Class name="{@name}{../@name}DataType" partial="true" visibility="Public">
                                            <plx:Property name="PortableDataType" override="true" visibility="Public">
                                                <plx:Param name="" style="RetVal" dataTypeName="PortableDataType"/>
                                                <plx:Get>
                                                    <plx:Return>
                                                        <plx:Call name="{../@name}{@name}" qualifier="PortableDataType" style="Field"/>
                                                    </plx:Return>
                                                </plx:Get>
                                            </plx:Property>
                                            <plx:Function override="true" name="ToString" visibility="Public">
                                                <plx:Param name="" style="RetVal" dataTypeName="String" dataTypeQualifier="System"/>
                                                <plx:Return>
                                                    <plx:Call name="PortableDataType{../@name}{@name}" qualifier="ResourceStrings" style="Field"/>
                                                </plx:Return>
                                            </plx:Function>
                                        </plx:Class>
                                    </xsl:when>
                                    <xsl:otherwise>
                         <plx:Class name="{../@name}DataType" partial="true" visibility="Public">
                                       <plx:Property name="PortableDataType" override="true" visibility="Public">
                                            <plx:Param name="" style="RetVal" dataTypeName="PortableDataType"/>
                                            <plx:Get>
                                                <plx:Return>
                                                    <plx:Call name="{../@name}" qualifier="PortableDataType" style="Field"/>
                                                </plx:Return>
                                            </plx:Get>
                                        </plx:Property>
                                        <plx:Function override="true" name="ToString" visibility="Public">
                                            <plx:Param name="" style="RetVal" dataTypeName="String" dataTypeQualifier="System"/>
                                            <plx:Return>
                                                <plx:Call name="PortableDataType{../@name}" qualifier="ResourceStrings" style="Field"/>
                                            </plx:Return>
                                        </plx:Function>
                        </plx:Class>
                                    </xsl:otherwise>
                                </xsl:choose>
                            </xsl:for-each>
                    </xsl:if>
                </xsl:for-each>
            </plx:Namespace>
        </plx:Root>
    </xsl:template>
</xsl:stylesheet>
