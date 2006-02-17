<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" 
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:plx="http://schemas.neumont.edu/CodeGeneration/PLiX"
    xmlns:arg="http://schemas.neumont.edu/ORM/SDK/AttachRulesGenerator"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt">
	<!-- Indenting is useful for debugging the transform, but a waste of memory at generation time -->
	<!--<xsl:output indent="yes"/>-->
	<!-- Pick up param value supplied automatically by plix loader -->
	<xsl:param name="CustomToolNamespace" select="'TestNamespace'"/>
	<xsl:template match="arg:Rules">
		<plx:root>
			<plx:namespaceImport name="System"/>
			<plx:namespaceImport name="System.Reflection"/>
			<xsl:apply-templates select="child::*"/>
		</plx:root>
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
		<plx:namespace name="{$namespaceName}">
			<plx:class name="{@class}" visibility="public" partial="true">
				<plx:leadingInfo>
					<plx:pragma type="region" data="Attach rules to {@class} model"/>
				</plx:leadingInfo>
				<plx:trailingInfo>
					<plx:pragma type="closeRegion" data="Attach rules to {@class} model"/>
				</plx:trailingInfo>
				<plx:function name="AllMetaModelTypes" visibility="protected" modifier="override">
					<plx:leadingInfo>
						<plx:docComment>
							<summary>Generated code to attach rules to the store.</summary>
						</plx:docComment>
					</plx:leadingInfo>
					<plx:returns dataTypeName="Type" dataTypeIsSimpleArray="true"/>
					<plx:branch>
						<plx:condition>
							<plx:unaryOperator type="booleanNot">
								<plx:callStatic dataTypeName="ORMMetaModel" dataTypeQualifier="Neumont.Tools.ORM.ObjectModel" name="InitializingToolboxItems" type="property"/>
							</plx:unaryOperator>
						</plx:condition>
						<plx:return>
							<plx:callStatic dataTypeName="Type" name="EmptyTypes" type="property"/>
						</plx:return>
					</plx:branch>
					<plx:local name="retVal" dataTypeName="Type" dataTypeIsSimpleArray="true">
						<plx:initialize>
							<plx:callNew dataTypeName="Type" dataTypeIsSimpleArray="true">
								<plx:arrayInitializer>
									<xsl:variable name="contextClass" select="@class"/>
									<xsl:for-each select="arg:Rule">
										<plx:passParam>
											<xsl:call-template name="GenerateTypeOf">
												<xsl:with-param name="className" select="@class"/>
												<xsl:with-param name="namespace" select="@namespace"/>
												<xsl:with-param name="contextClass" select="$contextClass"/>
												<xsl:with-param name="contextNamespace" select="$namespaceName"/>
											</xsl:call-template>
										</plx:passParam>
									</xsl:for-each>
								</plx:arrayInitializer>
							</plx:callNew>
						</plx:initialize>
					</plx:local>
					<plx:callStatic name="Assert" dataTypeName="Debug" dataTypeQualifier="System.Diagnostics">
						<plx:passParam>
							<plx:unaryOperator type="booleanNot">
								<plx:callInstance name="Contains">
									<plx:callObject>
										<plx:cast type="exceptionCast" dataTypeName="IList" dataTypeQualifier="System.Collections">
											<plx:nameRef name="retVal"/>
										</plx:cast>
									</plx:callObject>
									<plx:passParam>
										<plx:nullKeyword/>
									</plx:passParam>
								</plx:callInstance>
							</plx:unaryOperator>
						</plx:passParam>
						<plx:passParam>
							<plx:string>One or more rule types failed to resolve. The file and/or package will fail to load.</plx:string>
						</plx:passParam>
					</plx:callStatic>
					<plx:return>
						<plx:nameRef name="retVal"/>
					</plx:return>
				</plx:function>
			</plx:class>
		</plx:namespace>
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
					<plx:typeOf dataTypeName="{$className}">
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
					</plx:typeOf>
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
			<plx:callInstance name="GetNestedType">
				<plx:callObject>
					<xsl:copy-of select="$callObject"/>
				</plx:callObject>
				<plx:passParam>
					<plx:string>
						<xsl:choose>
							<xsl:when test="string-length($firstType)">
								<xsl:value-of select="$firstType"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$nestedTypes"/>
							</xsl:otherwise>
						</xsl:choose>
					</plx:string>
				</plx:passParam>
				<plx:passParam>
					<plx:binaryOperator type="bitwiseOr">
						<plx:left>
							<plx:callStatic name="Public" dataTypeName="BindingFlags" type="field"/>
						</plx:left>
						<plx:right>
							<plx:callStatic name="NonPublic" dataTypeName="BindingFlags" type="field"/>
						</plx:right>
					</plx:binaryOperator>
				</plx:passParam>
			</plx:callInstance>
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