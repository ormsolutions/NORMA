<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" 
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:plx="http://Schemas.Neumont.edu/CodeGeneration/Plix"
    xmlns:arg="http://Schemas.Neumont.edu/Private/AttachRulesGenerator"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt">
	<!-- Pick up param value supplied automatically by plix loader -->
	<xsl:param name="CustomToolNamespace" select="'TestNamespace'"/>
	<xsl:template match="arg:Rules">
		<plx:Root>
			<plx:Using name="System"/>
			<plx:Using name="System.Reflection"/>
			<xsl:apply-templates select="child::*"/>
		</plx:Root>
	</xsl:template>
	<xsl:template match="arg:Model">
		<xsl:variable name="namespaceNameTemp">
			<xsl:choose>
				<xsl:when test="string-length(@namespace)">
					<xsl:value-of select="@namespace"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$CustomToolNamespace"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="namespaceName" select="string($namespaceNameTemp)"/>
		<plx:Namespace name="{$namespaceName}">
			<plx:Class name="{@class}" visibility="Public" partial="true">
				<plx:Function name="AllMetaModelTypes" visibility="Protected" override="true">
					<plx:Param type="RetVal" name="" dataTypeName="Type" dataTypeIsSimpleArray="true"/>
					<plx:Variable name="retVal" dataTypeName="Type" dataTypeIsSimpleArray="true">
						<plx:Initialize>
							<plx:CallNew type="New" dataTypeName="Type" dataTypeIsSimpleArray="true">
								<plx:ArrayInitializer>
									<xsl:variable name="contextClass" select="@class"/>
									<xsl:for-each select="arg:Rule">
										<plx:PassParam>
											<xsl:call-template name="GenerateTypeOf">
												<xsl:with-param name="className" select="@class"/>
												<xsl:with-param name="namespace" select="@namespace"/>
												<xsl:with-param name="contextClass" select="$contextClass"/>
												<xsl:with-param name="contextNamespace" select="$namespaceName"/>
											</xsl:call-template>
										</plx:PassParam>
									</xsl:for-each>
								</plx:ArrayInitializer>
							</plx:CallNew>
						</plx:Initialize>
					</plx:Variable>
					<plx:CallStatic name="Assert" dataTypeName="Debug" dataTypeQualifier="System.Diagnostics">
						<plx:PassParam>
							<plx:Operator type="BooleanNot">
								<plx:CallInstance name="Contains">
									<plx:CallObject>
										<plx:Cast type="TypeCastException">
											<plx:TargetType dataTypeName="IList" dataTypeQualifier="System.Collections"/>
											<plx:CastExpression>
												<plx:Value type="Local" data="retVal"/>
											</plx:CastExpression>
										</plx:Cast>
									</plx:CallObject>
									<plx:PassParam>
										<plx:NullObjectKeyword/>
									</plx:PassParam>
								</plx:CallInstance>
							</plx:Operator>
						</plx:PassParam>
						<plx:PassParam>
							<plx:String>One or more rule types failed to resolve. The file and/or package will fail to load.</plx:String>
						</plx:PassParam>
					</plx:CallStatic>
					<plx:Return>
						<plx:Value type="Local" data="retVal"/>
					</plx:Return>
				</plx:Function>
			</plx:Class>
		</plx:Namespace>
	</xsl:template>
	<xsl:template name="GenerateTypeOf">
		<xsl:param name="className"/>
		<xsl:param name="namespace" select="''"/>
		<xsl:param name="contextClass" select="''"/>
		<xsl:param name="contextNamespace"/>
		<xsl:variable name="namespaceNameTemp">
			<xsl:choose>
				<xsl:when test="string-length($namespace)">
					<xsl:value-of select="$namespace"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$CustomToolNamespace"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="namespaceName" select="string($namespaceNameTemp)"/>
		<xsl:variable name="normalizedName" select="normalize-space(translate($className, '+.','  '))"/>
		<xsl:variable name="publicPart" select="substring-before($normalizedName,' ')"/>
		<xsl:choose>
			<xsl:when test="string-length($publicPart) and $namespaceName=$contextNamespace and $publicPart=$contextClass">
				<xsl:call-template name="GenerateTypeOf">
					<xsl:with-param name="className" select="substring($normalizedName, string-length($publicPart)+2)"/>
					<xsl:with-param name="contextNamespace" select="$contextNamespace"/>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:variable name="primaryTypeOf">
					<plx:TypeOf dataTypeName="{$className}">
						<xsl:if test="string-length($publicPart)">
							<xsl:attribute name="dataTypeName">
								<xsl:value-of select="$publicPart"/>
							</xsl:attribute>
						</xsl:if>
						<xsl:if test="$contextNamespace!=$namespaceName">
							<xsl:attribute name="dataTypeQualifier">
								<xsl:value-of select="$namespaceName"/>
							</xsl:attribute>
						</xsl:if>
					</plx:TypeOf>
				</xsl:variable>
				<xsl:choose>
					<xsl:when test="string-length($publicPart)">
						<xsl:call-template name="GeneratedNestedTypeCall">
							<xsl:with-param name="nestedTypes" select="substring($normalizedName, string-length($publicPart)+2)"/>
							<xsl:with-param name="callObject" select="$primaryTypeOf"/>
						</xsl:call-template>
					</xsl:when>
					<xsl:otherwise>
						<xsl:copy-of select="$primaryTypeOf"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:otherwise>		  
		</xsl:choose>
	</xsl:template>
	<xsl:template name="GeneratedNestedTypeCall">
		<xsl:param name="nestedTypes"/>
		<xsl:param name="callObject"/>
		<xsl:variable name="firstType" select="substring-before($nestedTypes, ' ')"/>
		<xsl:variable name="nestedTypeCall">
			<plx:CallInstance name="GetNestedType">
				<plx:CallObject>
					<xsl:copy-of select="$callObject"/>
				</plx:CallObject>
				<plx:PassParam>
					<plx:String>
						<xsl:choose>
							<xsl:when test="string-length($firstType)">
								<xsl:value-of select="$firstType"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$nestedTypes"/>
							</xsl:otherwise>
						</xsl:choose>
					</plx:String>
				</plx:PassParam>
				<plx:PassParam>
					<plx:Operator type="BitwiseOr">
						<plx:Left>
							<plx:CallStatic name="Public" dataTypeName="BindingFlags" type="Field"/>
						</plx:Left>
						<plx:Right>
							<plx:CallStatic name="NonPublic" dataTypeName="BindingFlags" type="Field"/>
						</plx:Right>
					</plx:Operator>
				</plx:PassParam>
			</plx:CallInstance>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="string-length($firstType)">
				<xsl:call-template name="GeneratedNestedTypeCall">
					<xsl:with-param name="nestedTypes" select="substring($nestedTypes, string-length($firstType)+2)"/>
					<xsl:with-param name="callObject" select="$nestedTypeCall"/>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:copy-of select="$nestedTypeCall"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>