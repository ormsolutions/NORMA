<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet 
    version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:plx="http://Schemas.Neumont.edu/CodeGeneration/Plix">
	<xsl:template match="ResourceStrings">
		<plx:Root xmlns:plx="http://Schemas.Neumont.edu/CodeGeneration/Plix">
			<plx:Namespace name="Neumont.Tools.ORM">
				<plx:Class name="ResourceStrings" partial="true" visibility="Internal">
					<!--<xsl:for-each select="ResourceString">
						<plx:Field const="true" visibility="Private" dataTypeName="String" dataTypeQualifier="System" name="{@name}_Id">
							<plx:Initialize>
								<plx:String>
									<xsl:value-of select="@resourceName"/>
								</plx:String>
							</plx:Initialize>
						</plx:Field>
					</xsl:for-each>-->
					<xsl:for-each select="ResourceString">
						<plx:Property name="{@name}" static="true" visibility="Public">
							<plx:Param name="" type="RetVal" dataTypeName="String" dataTypeQualifier="System"></plx:Param>
							<plx:Get>
								<plx:Return>
									<plx:CallStatic name="GetString" type="MethodCall" dataTypeName="ResourceStrings">
										<plx:PassParam passStyle="In">
											<plx:CallStatic name="{@model}" dataTypeName="ResourceManagers" type="Field"/>
										</plx:PassParam>
										<plx:PassParam passStyle="In">
											<plx:String>
												<xsl:value-of select="@resourceName"/>
											</plx:String>
											<!--<plx:CallStatic name="{@name}_Id" dataTypeName="ResourceStrings" type="Field"/>-->
										</plx:PassParam>
									</plx:CallStatic>
								</plx:Return>
							</plx:Get>
						</plx:Property>
					</xsl:for-each>
				</plx:Class>
			</plx:Namespace>
		</plx:Root>
	</xsl:template>
</xsl:stylesheet>
