<?xml version="1.0" encoding="utf-8"?>
<!--
	Neumont Object-Role Modeling Architect for Visual Studio

	Copyright © Neumont University. All rights reserved.

	The use and distribution terms for this software are covered by the
	Common Public License 1.0 (http://opensource.org/licenses/cpl) which
	can be found in the file CPL.txt at the root of this distribution.
	By using this software in any fashion, you are agreeing to be bound by
	the terms of this license.

	You must not remove this notice, or any other, from this software.
-->
<!-- Contributors: Joshua Arnold, Rexford Morgan, Jeremy Shovan -->
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:exsl="http://exslt.org/common"
	xmlns:oil="http://schemas.orm.net/OIAL"
	xmlns:ormdt="http://schemas.orm.net/ORMDataTypes"
	xmlns:plx="http://schemas.neumont.edu/CodeGeneration/PLiX"
	xmlns:xs="http://www.w3.org/2001/XMLSchema"
	exclude-result-prefixes="oil ormdt"
	extension-element-prefixes="exsl">
	<xsl:output method="xml" encoding="utf-8" media-type="text/xml" indent="yes"/>
	<xsl:param name="OIAL" select="/*"/>
	<xsl:variable name="ModelName" select="$OIAL/@name"/>
	<xsl:variable name="ConceptTypes" select="$OIAL//oil:conceptType"/>
	<xsl:variable name="AllRoleSequenceUniquenessConstraints" select="$OIAL//oil:roleSequenceUniquenessConstraint"/>
	<xsl:template match="/">
		<plx:root>
			<plx:namespace name="PHP">
				<plx:callStatic dataTypeName=".global" name="require_once" type="methodCall">
					<plx:passParam>
						<plx:string>
							<xsl:text>Entities.php</xsl:text>
						</plx:string>
					</plx:passParam>
				</plx:callStatic>
				<plx:callStatic dataTypeName=".global" name="require_once" type="methodCall">
					<plx:passParam>
						<plx:string>
							<xsl:text>DataLayer.php</xsl:text>
						</plx:string>
					</plx:passParam>
				</plx:callStatic>
				<xsl:apply-templates select="$OIAL" mode="OIALtoPLiX_PHPDataLayer_Implementation">
					<xsl:with-param name="ModelContextName" select="concat($ModelName, 'Context')"/>
				</xsl:apply-templates>
			</plx:namespace>
		</plx:root>
	</xsl:template>
	<xsl:template match="oil:model" mode="OIALtoPLiX_PHPDataLayer_Implementation">
		<xsl:param name="Model" select="."/>
		<xsl:param name="ModelContextName"/>
		<!-- Iterate through the Concept Types and create their respective Data Access Classes -->
		<xsl:for-each select="$ConceptTypes">
			<xsl:variable name="CurrentConceptType" select="."/>
			<xsl:variable name="EntityName" select="@name"/>
			<plx:class name="{$EntityName}Service" partial="true" visibility="public">
				<!-- Field to provide Singleton access -->
				<plx:field dataTypeName="{$EntityName}Service" static="true" name="instance" visibility="private"/>
				<plx:function name=".construct" visibility="public">
					<plx:leadingInfo>
						<plx:docComment>
							<xsl:text>Instantiates a new instance of </xsl:text>
							<xsl:value-of select="$EntityName"/>
							<xsl:text>Service</xsl:text>
						</plx:docComment>
					</plx:leadingInfo>
				</plx:function>
				<!-- Property to provide Singleton access -->
				<plx:property name="Instance" modifier="static" visibility="public">
					<plx:returns dataTypeName="{$EntityName}Service"/>
					<plx:get>
						<plx:branch>
							<plx:condition>
								<plx:unaryOperator type="booleanNot">
									<plx:callStatic dataTypeName=".global" name="isset"/>
								</plx:unaryOperator>
							</plx:condition>
							<plx:assign>
								<plx:left>
									<plx:callThis accessor="static" name="instance" type="field"/>
								</plx:left>
								<plx:right>
									<plx:callNew dataTypeName="{$EntityName}Service" type="new"/>
								</plx:right>
							</plx:assign>
						</plx:branch>
						<plx:return>
							<plx:callThis accessor="static" name="instance" type="field"/>
						</plx:return>
					</plx:get>
				</plx:property>
				<!-- Method to retrieve the entire collection of Concept Types from the database -->
				<plx:function name="getAll" visibility="public">
					<plx:leadingInfo>
						<plx:docComment>
							<xsl:text>Retrieves the entire collection of </xsl:text>
							<xsl:value-of select="$EntityName"/>
							<xsl:text> objects</xsl:text>
						</plx:docComment>
					</plx:leadingInfo>
					<!-- Call DAO method-->
					<plx:return>
						<plx:callInstance name="getAll" type="methodCall">
							<plx:callObject>
								<plx:callStatic name="Instance" type="property" dataTypeName="{$EntityName}DAO"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:return>
				</plx:function>
				<!-- Method to retrieves a single Concept Type from the database -->
				<plx:function name="getSingle" visibility="public">
					<plx:leadingInfo>
						<plx:docComment>
							<xsl:text>Retrieves the specified </xsl:text>
							<xsl:value-of select="$EntityName"/>
							<xsl:text> object from the database</xsl:text>
						</plx:docComment>
					</plx:leadingInfo>
					<xsl:variable name="uniqueInformationTypesFragment">
						<xsl:apply-templates select="." mode="RecursiveGetPreferredUniqueness"/>
					</xsl:variable>
					<xsl:variable name="uniqueInformationTypes" select="exsl:node-set($uniqueInformationTypesFragment)"/>
					<xsl:for-each select="$uniqueInformationTypes/oil:informationType">
						<plx:param name="{@name}">
						</plx:param>
					</xsl:for-each>
					<plx:returns dataTypeName="{$EntityName}"/>
					<plx:return>
						<plx:callInstance name="getSingle" type="methodCall">
							<plx:callObject>
								<plx:callStatic name="Instance" type="property" dataTypeName="{$EntityName}DAO"/>
							</plx:callObject>
							<xsl:for-each select="$uniqueInformationTypes/oil:informationType">
								<plx:passParam>
									<plx:nameRef name="{@name}" type="parameter"/>
								</plx:passParam>
							</xsl:for-each>
						</plx:callInstance>
					</plx:return>
				</plx:function>
				<!-- Method to Insert the object into the database -->
				<plx:function name="insert" visibility="public">
					<plx:leadingInfo>
						<plx:docComment>
							<xsl:text>Inserts the given </xsl:text>
							<xsl:value-of select="$EntityName"/>
							<xsl:text> object into the database</xsl:text>
						</plx:docComment>
					</plx:leadingInfo>
					<plx:param dataTypeName="{$EntityName}" name="{$EntityName}"/>
					<plx:returns dataTypeName=".boolean"/>
					<plx:return>
						<plx:callInstance name="insert" type="methodCall">
							<plx:callObject>
								<plx:callStatic name="Instance" type="property" dataTypeName="{$EntityName}DAO"/>
							</plx:callObject>
							<plx:passParam>
								<plx:nameRef name="{$EntityName}" type="parameter"/>
							</plx:passParam>
						</plx:callInstance>
					</plx:return>
				</plx:function>
				<!-- Method to Update the object into the database -->
				<plx:function name="update" visibility="public">
					<plx:leadingInfo>
						<plx:docComment>
							<xsl:text>Updates the given </xsl:text>
							<xsl:value-of select="$EntityName"/>
							<xsl:text> object in the database</xsl:text>
						</plx:docComment>
					</plx:leadingInfo>
					<plx:param dataTypeName="{$EntityName}" name="{$EntityName}"/>
					<plx:returns dataTypeName=".boolean"/>
					<plx:return>
						<plx:callInstance name="update" type="methodCall">
							<plx:callObject>
								<plx:callStatic name="Instance" type="property" dataTypeName="{$EntityName}DAO" />
							</plx:callObject>
							<plx:passParam>
								<plx:nameRef name="{$EntityName}" type="parameter"/>
							</plx:passParam>
						</plx:callInstance>
					</plx:return>
				</plx:function>
				<!-- Method to Delete the object from the database -->
				<plx:function name="delete" visibility="public">
					<plx:leadingInfo>
						<plx:docComment>
							<xsl:text>Deletes the given </xsl:text>
							<xsl:value-of select="$EntityName"/>
							<xsl:text> object from the database</xsl:text>
						</plx:docComment>
					</plx:leadingInfo>
					<plx:param dataTypeName="{$EntityName}" name="{$EntityName}"/>
					<plx:returns dataTypeName=".boolean"/>
					<plx:return>
						<plx:callInstance name="delete" type="methodCall">
							<plx:callObject>
								<plx:callStatic name="Instance" type="property" dataTypeName="{$EntityName}DAO"/>
							</plx:callObject>
							<plx:passParam>
								<plx:nameRef name="{$EntityName}" type="parameter"/>
							</plx:passParam>
						</plx:callInstance>
					</plx:return>
				</plx:function>
				<!-- Create Proxied One-to-Many Methods -->
				<xsl:for-each select="$OIAL//oil:conceptType/oil:conceptTypeRef[@target=$EntityName]">
					<xsl:variable name="oppositeName" select="@oppositeName"/>
					<xsl:variable name="proxyName" select="@target"/>
					<!-- Method to retrieve a collection of type parent::conceptType by the $EntityName's preferred identification scheme -->
					<plx:function name="get_{$oppositeName}_Collection_By_{@name}" visibility="public">
						<plx:leadingInfo>
							<plx:docComment>
								<summary>
									<xsl:text>Retrieves a collection of </xsl:text>
									<xsl:value-of select="$oppositeName"/>
									<xsl:text> objects by the given </xsl:text>
									<xsl:value-of select="$EntityName"/>
									<xsl:text> object</xsl:text>
								</summary>
							</plx:docComment>
						</plx:leadingInfo>
						<xsl:variable name="ProxyConceptType" select="$OIAL//oil:conceptType[@name=$proxyName]"/>
						<xsl:variable name="proxyUniqueInformationTypesFragment">
							<xsl:apply-templates select="$ProxyConceptType" mode="RecursiveGetPreferredUniqueness"/>
						</xsl:variable>
						<xsl:variable name="proxyUniqueInformationTypes" select="exsl:node-set($proxyUniqueInformationTypesFragment)"/>
						<xsl:variable name="ProxiedConceptType" select="$OIAL//oil:conceptType[@name=$oppositeName]"/>
						<xsl:variable name="proxiedUniqueInformationTypesFragment">
							<xsl:apply-templates select="$ProxiedConceptType" mode="RecursiveGetPreferredUniqueness"/>
						</xsl:variable>
						<xsl:for-each select="$proxyUniqueInformationTypes/oil:informationType">
							<plx:param name="{@name}">
								<xsl:attribute name="dataTypeName">
									<xsl:apply-templates select="$OIAL//oil:informationTypeFormats/ormdt:*[@name=current()/@formatRef]" mode="GetInformationTypeFormat"/>
								</xsl:attribute>
							</plx:param>
						</xsl:for-each>
						<xsl:variable name="proxiedUniqueInformationTypes" select="exsl:node-set($proxiedUniqueInformationTypesFragment)"/>
						<plx:returns dataTypeIsSimpleArray="true"/>
						<plx:return>
							<plx:callInstance name="get_{$oppositeName}_Collection_By_{@name}" type="methodCall">
								<plx:callObject>
									<plx:callStatic name="Instance" type="property" dataTypeName="{$EntityName}DAO"/>
								</plx:callObject>
								<xsl:for-each select="$proxyUniqueInformationTypes/oil:informationType">
									<plx:passParam>
										<plx:nameRef name="{@name}" type="parameter"/>
									</plx:passParam>
								</xsl:for-each>
							</plx:callInstance>
						</plx:return>
					</plx:function>
				</xsl:for-each>
			</plx:class>
		</xsl:for-each>
	</xsl:template>
	<!-- Uses the auto-generated DataAcess class to retrieve a configured Zend_Db_Adapter object -->
	<xsl:template name="getDbAdapter">
		<plx:local name="db" dataTypeName="Zend_Db_Adapter">
			<plx:initialize>
				<plx:callStatic dataTypeName="DataAccess" name="getDataAdapter"/>
			</plx:initialize>
		</plx:local>
	</xsl:template>
	<!-- Begin "ConceptType-value" generation templates-->
	<xsl:template match="oil:conceptType" mode="RecursiveGenerateConceptTypeValues">
		<xsl:param name="ArrayFragment"/>
		<xsl:param name="ParentInstanceFragment">
			<plx:nameRef name="{@name}"/>
		</xsl:param>
		<xsl:if test="parent::oil:conceptType">
			<xsl:apply-templates select="parent::oil:conceptType" mode="RecursiveGenerateConceptTypeValues">
				<xsl:with-param name="ArrayFragment" select="$ArrayFragment"/>
				<xsl:with-param name="ParentInstanceFragment">
					<plx:callInstance name="{parent::oil:conceptType/@name}" type="property">
						<plx:callObject>
							<xsl:copy-of select="$ParentInstanceFragment"/>
						</plx:callObject>
					</plx:callInstance>
				</xsl:with-param>
			</xsl:apply-templates>
		</xsl:if>
		<xsl:apply-templates select="child::*" mode="GenerateConceptTypeValues">
			<xsl:with-param name="ArrayFragment" select="$ArrayFragment"/>
			<xsl:with-param name="ParentInstanceFragment" select="$ParentInstanceFragment"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="*" mode="GenerateConceptTypeValues"/>
	<xsl:template match="oil:informationType" mode="GenerateConceptTypeValues">
		<xsl:param name="ArrayFragment"/>
		<xsl:param name="ParentInstanceFragment"/>
		<xsl:variable name="typeName" select="@name"/>
		<plx:assign>
			<plx:left>
				<plx:callInstance name=".implied" type="arrayIndexer">
					<plx:callObject>
						<xsl:copy-of select="$ArrayFragment"/>
					</plx:callObject>
					<plx:passParam>
						<plx:string data="{$typeName}"/>
					</plx:passParam>
				</plx:callInstance>
			</plx:left>
			<plx:right>
				<plx:callInstance name="{@name}" type="property">
					<plx:callObject>
						<xsl:copy-of select="$ParentInstanceFragment"/>
					</plx:callObject>
				</plx:callInstance>
			</plx:right>
		</plx:assign>
	</xsl:template>
	<xsl:template match="oil:conceptTypeRef" mode="GenerateConceptTypeValues">
		<xsl:param name="ArrayFragment"/>
		<xsl:param name="ParentInstanceFragment"/>
		<xsl:variable name="typeName" select="@name"/>
		<xsl:variable name="targetName" select="@target"/>
		<xsl:choose>
			<xsl:when test="$OIAL//oil:conceptType[@name=$targetName]/oil:informationType/oil:singleRoleUniquenessConstraint[@isPreferred='true']">
				<xsl:variable name="typeIdentifier" select="$OIAL//oil:conceptType[@name=$targetName]/oil:informationType[oil:singleRoleUniquenessConstraint[@isPreferred='true']]"/>
				<plx:assign>
					<plx:left>
						<plx:callInstance name=".implied" type="arrayIndexer">
							<plx:callObject>
								<xsl:copy-of select="$ArrayFragment"/>
							</plx:callObject>
							<plx:passParam>
								<plx:string data="{$typeName}"/>
							</plx:passParam>
						</plx:callInstance>
					</plx:left>
					<plx:right>
						<plx:callInstance name="{$typeIdentifier/@name}" type="property">
							<plx:callObject>
								<xsl:copy-of select="$ParentInstanceFragment"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:right>
				</plx:assign>
			</xsl:when>
			<xsl:otherwise>
				<xsl:for-each select="$OIAL//oil:conceptType[@name=$typeName]/oil:roleSequenceUniqueness[@isPreferred='true']/oil:roleSequence/oil:typeRef">
					<xsl:variable name="typeRef" select="$OIAL//oil:conceptType[@name=$typeName]/oil:*[current()/@name = $OIAL//oil:conceptType[@name=$typeName]/oil:roleSequenceUniqueness[@isPreferred='true']/oil:roleSequence/oil:typeRef/@targetChild]"/>
					<plx:assign>
						<plx:left>
							<xsl:copy-of select="$ArrayFragment"/>
						</plx:left>
						<plx:right>
							<xsl:choose>
								<xsl:when test="$typeRef[self::oil:informationType]">
									<xsl:apply-templates select="$typeRef" mode="GenerateConceptTypeValues">
										<xsl:with-param name="ArrayFragment" select="$ArrayFragment"/>
										<xsl:with-param name="ParentInstanceFragment">
											<!-- Find the currently referenced ConceptType from the $OIAL variable and check for a parent -->
											<xsl:choose>
												<xsl:when test="$OIAL//oil:conceptType[@name=$typeName]/parent::oil:conceptType">
													<plx:callInstance name="{$typeName}" type="property">
														<plx:callObject>
															<xsl:copy-of select="$ParentInstanceFragment"/>
														</plx:callObject>
													</plx:callInstance>
												</xsl:when>
												<xsl:otherwise>
													<xsl:copy-of select="$ParentInstanceFragment"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:with-param>
									</xsl:apply-templates>
								</xsl:when>
								<xsl:otherwise>
									<plx:comment>I'm right here</plx:comment>
								</xsl:otherwise>
							</xsl:choose>
						</plx:right>
					</plx:assign>
				</xsl:for-each>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- End "ConceptType-value" generation templates-->
	<!-- Recursively generates the WHERE clause for the primary identification scheme of the specified ConceptType -->
	<xsl:template match="oil:conceptType" mode="RecursiveGenerateWhereClause">
		<xsl:param name="ObjectFragment"/>
		<xsl:param name="MethodName" select="'quoteInto'"/>
		<xsl:param name="ParentInstanceFragment">
			<plx:nameRef name="{@name}"/>
		</xsl:param>
		<xsl:param name="UniqueInformationTypes"/>
		<xsl:choose>
			<xsl:when test="parent::oil:conceptType and (not(oil:informationType/oil:singleRoleUniquenessConstraint[@isPreferred='true']) and not(oil:roleSequenceUniquenessConstraint[@isPreferred='true']))">
				<xsl:apply-templates mode="RecursiveGenerateWhereClause" select="parent::oil:conceptType">
					<xsl:with-param name="ObjectFragment" select="$ObjectFragment"/>
					<xsl:with-param name="MethodName" select="$MethodName"/>
					<xsl:with-param name="ParentInstanceFragment">
						<plx:callInstance name="{parent::oil:conceptType/@name}" type="property">
							<plx:callObject>
								<xsl:copy-of select="$ParentInstanceFragment"/>
							</plx:callObject>
						</plx:callInstance>
					</xsl:with-param>
					<xsl:with-param name="UniqueInformationTypes" select="$UniqueInformationTypes"/>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:for-each select="child::*[oil:singleRoleUniquenessConstraint/@isPreferred='true' or @isPreferred='true']">
					<xsl:apply-templates select="." mode="GenerateWhereClause">
						<xsl:with-param name="ObjectFragment" select="$ObjectFragment"/>
						<xsl:with-param name="MethodName" select="$MethodName"/>
						<xsl:with-param name="ParentInstanceFragment" select="$ParentInstanceFragment"/>
						<xsl:with-param name="UniqueInformationTypes" select="$UniqueInformationTypes"/>
					</xsl:apply-templates>
				</xsl:for-each>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="oil:informationType" mode="GenerateWhereClause">
		<xsl:param name="ObjectFragment"/>
		<xsl:param name="MethodName"/>
		<xsl:param name="ParentInstanceFragment">
			<plx:nameRef name="@name"/>
		</xsl:param>
		<plx:callInstance name="{$MethodName}" type="methodCall">
			<plx:callObject>
				<xsl:copy-of select="$ObjectFragment"/>
			</plx:callObject>
			<plx:passParam>
				<plx:string data="{@name} = ?"/>
			</plx:passParam>
			<plx:passParam>
				<plx:callInstance name="{@name}" type="property">
					<plx:callObject>
						<xsl:copy-of select="$ParentInstanceFragment"/>
					</plx:callObject>
				</plx:callInstance>
			</plx:passParam>
		</plx:callInstance>
	</xsl:template>
	<xsl:template match="oil:roleSequenceUniquenessConstraint" mode="GenerateWhereClause">
		<xsl:param name="ObjectFragment"/>
		<xsl:param name="MethodName"/>
		<xsl:param name="ParentInstanceFragment">
			<plx:nameRef name="@name"/>
		</xsl:param>
		<xsl:for-each select="oil:roleSequence/oil:typeRef">
			<xsl:variable name="targetConceptType" select="@targetConceptType"/>
			<xsl:variable name="targetChild" select="@targetChild"/>
			<xsl:apply-templates select="$OIAL//oil:conceptType[@name=$targetConceptType]/child::*[@name=$targetChild]" mode="GenerateWhereClause">
				<xsl:with-param name="ObjectFragment" select="$ObjectFragment"/>
				<xsl:with-param name="MethodName" select="$MethodName"/>
				<xsl:with-param name="ParentInstanceFragment" select="$ParentInstanceFragment"/>
			</xsl:apply-templates>
		</xsl:for-each>
	</xsl:template>
	<xsl:template match="oil:conceptTypeRef" mode="GenerateWhereClause">
		<xsl:param name="ObjectFragment"/>
		<xsl:param name="MethodName"/>
		<xsl:param name="ParentInstanceFragment">
			<plx:nameRef name="@name"/>
		</xsl:param>
		<xsl:apply-templates select="$OIAL//oil:conceptType[@name=current()/@target]"  mode="RecursiveGenerateWhereClause">
			<xsl:with-param name="ObjectFragment" select="$ObjectFragment"/>
			<xsl:with-param name="MethodName" select="$MethodName"/>
			<xsl:with-param name="ParentInstanceFragment" select="$ParentInstanceFragment"/>
		</xsl:apply-templates>
	</xsl:template>
	<!-- Recursively finds the Parent Name of the ConceptType -->
	<xsl:template match="oil:conceptType" mode="FindParentName">
		<xsl:choose>
			<xsl:when test="parent::oil:conceptType">
				<xsl:apply-templates select="parent::oil:conceptType" mode="FindParentName"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="@name"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- Recursively retrieves oil:informationTypes for the preferred identification scheme of a given ConceptType -->
	<xsl:template match="oil:conceptType" mode="RecursiveGetPreferredUniqueness">
		<xsl:choose>
			<xsl:when test="parent::oil:conceptType and (not(oil:informationType/oil:singleRoleUniquenessConstraint[@isPreferred='true']) and not(oil:roleSequenceUniquenessConstraint[@isPreferred='true']))">
				<xsl:apply-templates select="parent::oil:conceptType" mode="RecursiveGetPreferredUniqueness"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:for-each select="child::*[oil:singleRoleUniquenessConstraint/@isPreferred='true' or @isPreferred='true']">
					<xsl:apply-templates select="." mode="GetPreferredUniquenessInformationTypes"/>
				</xsl:for-each>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="oil:informationType" mode="GetPreferredUniquenessInformationTypes">
		<xsl:copy-of select="."/>
	</xsl:template>
	<xsl:template match="oil:roleSequenceUniquenessConstraint" mode="GetPreferredUniquenessInformationTypes">
		<xsl:for-each select="oil:roleSequence/oil:typeRef">
			<xsl:variable name="targetConceptType" select="@targetConceptType"/>
			<xsl:variable name="targetChild" select="@targetChild"/>
			<xsl:apply-templates select="$OIAL//oil:conceptType[@name=$targetConceptType]/child::*[@name=$targetChild]" mode="GetPreferredUniquenessInformationTypes"/>
		</xsl:for-each>
	</xsl:template>
	<xsl:template match="oil:conceptTypeRef" mode="GetPreferredUniquenessInformationTypes">
		<xsl:apply-templates select="$OIAL//oil:conceptType[@name=current()/@target]"  mode="RecursiveGetPreferredUniqueness"/>
	</xsl:template>
	<!-- End preferred identification scheme of a given ConceptType -->
	<!-- Recursively calls the preferred identification scheme of a given ConceptType -->
	<xsl:template match="oil:conceptType" mode="RecursiveAccessPreferredUniqueness">
		<xsl:param name="ParentInstanceFragment">
			<plx:nameRef name="{@name}"/>
		</xsl:param>
		<xsl:param name="ArrayFragment"/>
		<xsl:choose>
			<xsl:when test="parent::oil:conceptType and (not(oil:informationType/oil:singleRoleUniquenessConstraint[@isPreferred='true']) and not(oil:roleSequenceUniquenessConstraint[@isPreferred='true']))">
				<xsl:apply-templates select="parent::oil:conceptType" mode="RecursiveAccessPreferredUniqueness">
					<xsl:with-param name="ParentInstanceFragment">
						<plx:callInstance name="{parent::oil:conceptType/@name}" type="property">
							<plx:callObject>
								<xsl:copy-of select="$ParentInstanceFragment"/>
							</plx:callObject>
						</plx:callInstance>
					</xsl:with-param>
					<xsl:with-param name="ArrayFragment" select="$ArrayFragment"/>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:for-each select="child::*[oil:singleRoleUniquenessConstraint/@isPreferred='true' or @isPreferred='true']">
					<xsl:apply-templates select="." mode="AccessPreferredUniquenessInformationTypes">
						<xsl:with-param name="ParentInstanceFragment" select="$ParentInstanceFragment"/>
						<xsl:with-param name="ArrayFragment" select="$ArrayFragment"/>
					</xsl:apply-templates>
				</xsl:for-each>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="*" mode="AccessPreferredUniquenessInformationTypes"/>
	<xsl:template match="oil:informationType" mode="AccessPreferredUniquenessInformationTypes">
		<xsl:param name="ParentInstanceFragment"/>
		<xsl:param name="ArrayFragment"/>
		<plx:assign>
			<plx:left>
				<plx:callInstance name="{@name}" type="property">
					<plx:callObject>
						<xsl:copy-of select="$ParentInstanceFragment"/>
					</plx:callObject>
				</plx:callInstance>
			</plx:left>
			<plx:right>
				<plx:callInstance name=".implied" type="arrayIndexer">
					<plx:callObject>
						<xsl:copy-of select="$ArrayFragment"/>
					</plx:callObject>
					<plx:passParam>
						<plx:string data="{@name}"/>
					</plx:passParam>
				</plx:callInstance>
			</plx:right>
		</plx:assign>
	</xsl:template>
	<xsl:template match="oil:roleSequenceUniquenessConstraint" mode="AccessPreferredUniquenessInformationTypes">
		<xsl:param name="ParentInstanceFragment"/>
		<xsl:param name="ArrayFragment"/>
		<xsl:variable name="CurrentConceptType" select="../@name"/>
		<xsl:for-each select="oil:roleSequence/oil:typeRef">
			<xsl:variable name="targetConceptType" select="@targetConceptType"/>
			<xsl:variable name="targetChild" select="@targetChild"/>
			<xsl:apply-templates select="$OIAL//oil:conceptType[@name=$targetConceptType]/child::*[@name=$targetChild]" mode="AccessPreferredUniquenessInformationTypes">
				<xsl:with-param name="ParentInstanceFragment">
					<xsl:choose>
						<xsl:when test="$targetConceptType=$CurrentConceptType">
							<xsl:copy-of select="$ParentInstanceFragment"/>
						</xsl:when>
						<xsl:otherwise>
							<plx:callInstance name="{$targetConceptType}" type="property">
								<plx:callObject>
									<xsl:copy-of select="$ParentInstanceFragment"/>
								</plx:callObject>
							</plx:callInstance>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:with-param>
				<xsl:with-param name="ArrayFragment" select="$ArrayFragment"/>
			</xsl:apply-templates>
		</xsl:for-each>
	</xsl:template>
	<xsl:template match="oil:conceptTypeRef" mode="AccessPreferredUniquenessInformationTypes">
		<xsl:param name="ParentInstanceFragment"/>
		<xsl:param name="ArrayFragment"/>
		<xsl:variable name="currentRef" select="@target"/>
		<xsl:apply-templates select="$OIAL//oil:conceptType[@name=$currentRef]"  mode="RecursiveAccessPreferredUniqueness">
			<xsl:with-param name="ParentInstanceFragment" select="$ParentInstanceFragment"/>
			<xsl:with-param name="ArrayFragment" select="$ArrayFragment"/>
		</xsl:apply-templates>
	</xsl:template>
	<!-- End preferred identification scheme of a given ConceptType -->
	<!-- Recursively calls all non-preferred identification scheme of a given ConceptType -->
	<xsl:template match="oil:conceptType" mode="RecursiveAccessAllInformationTypes">
		<xsl:param name="ParentInstanceFragment">
			<plx:nameRef name="{@name}"/>
		</xsl:param>
		<xsl:param name="ArrayFragment"/>
		<xsl:choose>
			<xsl:when test="parent::oil:conceptType">
				<xsl:apply-templates select="parent::oil:conceptType" mode="RecursiveAccessAllInformationTypes">
					<xsl:with-param name="ParentInstanceFragment">
						<plx:callInstance name="{parent::oil:conceptType/@name}" type="property">
							<plx:callObject>
								<xsl:copy-of select="$ParentInstanceFragment"/>
							</plx:callObject>
						</plx:callInstance>
					</xsl:with-param>
					<xsl:with-param name="ArrayFragment" select="$ArrayFragment"/>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:for-each select="child::*[(not(oil:singleRoleUniquenessConstraint) or oil:singleRoleUniquenessConstraint/@isPreferred='false') and (not(@isPreferred) or @isPreferred='false')]">
					<xsl:apply-templates select="." mode="AccessAllInformationTypes">
						<xsl:with-param name="ParentInstanceFragment" select="$ParentInstanceFragment"/>
						<xsl:with-param name="ArrayFragment" select="$ArrayFragment"/>
					</xsl:apply-templates>
				</xsl:for-each>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="*" mode="AccessAllInformationTypes"/>
	<xsl:template match="oil:informationType" mode="AccessAllInformationTypes">
		<xsl:param name="ParentInstanceFragment"/>
		<xsl:param name="ArrayFragment"/>
		<plx:assign>
			<plx:left>
				<plx:callInstance name="{@name}" type="property">
					<plx:callObject>
						<xsl:copy-of select="$ParentInstanceFragment"/>
					</plx:callObject>
				</plx:callInstance>
			</plx:left>
			<plx:right>
				<plx:callInstance name=".implied" type="arrayIndexer">
					<plx:callObject>
						<xsl:copy-of select="$ArrayFragment"/>
					</plx:callObject>
					<plx:passParam>
						<plx:string data="{@name}"/>
					</plx:passParam>
				</plx:callInstance>
			</plx:right>
		</plx:assign>
	</xsl:template>
	<xsl:template match="oil:conceptTypeRef" mode="AccessAllInformationTypes">
		<xsl:param name="ParentInstanceFragment"/>
		<xsl:param name="ArrayFragment"/>
		<xsl:variable name="currentRef" select="@target"/>
		<xsl:apply-templates select="$OIAL//oil:conceptType[@name=$currentRef]"  mode="RecursiveAccessPreferredUniqueness">
			<xsl:with-param name="ParentInstanceFragment" select="$ParentInstanceFragment"/>
			<xsl:with-param name="ArrayFragment" select="$ArrayFragment"/>
		</xsl:apply-templates>
	</xsl:template>
	<!-- End all non-preferred identification scheme of a given ConceptType -->
	<!-- Begin templates to convert data types -->
	<xsl:template match="*" mode="GetInformationTypeFormat"/>
	<xsl:template match="ormdt:identity" mode="GetInformationTypeFormat">
		<xsl:text>.i4</xsl:text>
	</xsl:template>
	<xsl:template match="ormdt:string" mode="GetInformationTypeFormat">
		<xsl:text>.string</xsl:text>
	</xsl:template>
	<xsl:template match="ormdt:boolean" mode="GetInformationTypeFormat">
		<xsl:text>.boolean</xsl:text>
	</xsl:template>
	<xsl:template match="ormdt:binary" mode="GetInformationTypeFormat">
		<xsl:text>.object</xsl:text>
	</xsl:template>
	<xsl:template match="ormdt:decimalNumber" mode="GetInformationTypeFormat">
		<xsl:text>.decimal</xsl:text>
	</xsl:template>
	<xsl:template match="ormdt:floatingPointNumber" mode="GetInformationTypeFormat">
		<xsl:text>.r4</xsl:text>
	</xsl:template>
	<!-- End templates to convert data types -->
</xsl:stylesheet>