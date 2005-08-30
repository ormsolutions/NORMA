<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet 
    version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:plx="http://Schemas.Neumont.edu/CodeGeneration/Plix">
	<xsl:template match="DataTypes">
		<plx:Root xmlns:plx="http://Schemas.Neumont.edu/CodeGeneration/Plix">
			<plx:Using name="System"/>
			<plx:Namespace name="Neumont.Tools.ORM.ObjectModel">
				<plx:Enum name="PortableDataType" visibility="Public">
					<plx:Attribute dataTypeName="CLSCompliant" dataTypeQualifier="System">
						<plx:PassParam>
							<plx:TrueKeyword/>
						</plx:PassParam>
					</plx:Attribute>
					<xsl:for-each select="DataType">
						<xsl:for-each select="SubType">
							<!-- UNDONE: Add comment text when comments are supported -->
							<plx:EnumItem name="{../@name}{@name}"/>
						</xsl:for-each>
					</xsl:for-each>
				</plx:Enum>
				<plx:Class name="ORMModel" visibility="Public" partial="true">
					<plx:Class name="AddIntrinsicDataTypesFixupListener" visibility="Private" partial="true" sealed="true">
						<plx:Field name="typeArray" dataTypeName="Type" dataTypeQualifier="System" dataTypeIsSimpleArray="true" static="true" visibility="Private">
							<plx:Initialize>
								<plx:CallNew dataTypeName="Type" dataTypeQualifier="System" dataTypeIsSimpleArray="true" type="New">
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
								</plx:CallNew>
							</plx:Initialize>
						</plx:Field>
					</plx:Class>
				</plx:Class>
				<xsl:for-each select="DataType">
					<xsl:if test="not(@enumOnly)">
						<xsl:for-each select="SubType">
							<xsl:choose>
								<xsl:when test="not(@name = '')">
									<plx:Class name="{@name}{../@name}DataType" partial="true" visibility="Public">
										<plx:Property name="PortableDataType" override="true" visibility="Public">
											<plx:Param name="" type="RetVal" dataTypeName="PortableDataType"/>
											<plx:Get>
												<plx:Return>
													<plx:CallStatic name="{../@name}{@name}" dataTypeName="PortableDataType" type="Field"/>
												</plx:Return>
											</plx:Get>
										</plx:Property>
										<plx:Function override="true" name="ToString" visibility="Public">
											<plx:Param name="" type="RetVal" dataTypeName="String" dataTypeQualifier="System"/>
											<plx:Return>
												<plx:CallStatic name="PortableDataType{../@name}{@name}" dataTypeName="ResourceStrings" type="Property"/>
											</plx:Return>
										</plx:Function>
									</plx:Class>
								</xsl:when>
								<xsl:otherwise>
									<plx:Class name="{../@name}DataType" partial="true" visibility="Public">
										<plx:Property name="PortableDataType" override="true" visibility="Public">
											<plx:Param name="" type="RetVal" dataTypeName="PortableDataType"/>
											<plx:Get>
												<plx:Return>
													<plx:CallStatic name="{../@name}" dataTypeName="PortableDataType" type="Property"/>
												</plx:Return>
											</plx:Get>
										</plx:Property>
										<plx:Function override="true" name="ToString" visibility="Public">
											<plx:Param name="" type="RetVal" dataTypeName="String" dataTypeQualifier="System"/>
											<plx:Return>
												<plx:CallStatic name="PortableDataType{../@name}" dataTypeName="ResourceStrings" type="Property"/>
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
