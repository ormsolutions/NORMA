<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet 
    version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:plx="http://Schemas.Northface.edu/CodeGeneration/Plix">
    <xsl:template match="ResourceStrings">
        <plx:Root xmlns:plx="http://Schemas.Northface.edu/CodeGeneration/Plix">
            <plx:Namespace name="Northface.Tools.ORM">
                <plx:Class name="ResourceStrings" partial="true" visibility="Internal">
                    <xsl:for-each select="ResourceString">
                        <plx:Variable const="true" visibility="Private" dataTypeName="String" dataTypeQualifier="System" name="{@name}_Id">
                            <plx:Initialize>
                                <plx:String>
                                    <xsl:value-of select="@resourceName"/>
                                </plx:String>
                            </plx:Initialize>
                        </plx:Variable>
                    </xsl:for-each>
                    <xsl:for-each select="ResourceString">
                        <plx:Property name="{@name}" shared="true" visibility="Public">
                            <plx:Param name="" style="RetVal" dataTypeName="String" dataTypeQualifier="System"></plx:Param>
                            <plx:Get>
                                <plx:Return>
                                    <plx:Call name="GetString" style="MethodCall" qualifier="ResourceStrings">
                                        <plx:PassParam passStyle="In">
                                            <plx:Call name="{@model}" qualifier="ResourceManagers" style="Field"/>
                                        </plx:PassParam>
                                        <plx:PassParam passStyle="In">
                                            <plx:Call name="{@name}_Id" qualifier="ResourceStrings" style="Field"/>
                                        </plx:PassParam>
                                    </plx:Call>
                                </plx:Return>
                            </plx:Get>
                        </plx:Property>
                    </xsl:for-each>
                </plx:Class>
            </plx:Namespace>
        </plx:Root>
    </xsl:template>
</xsl:stylesheet>
