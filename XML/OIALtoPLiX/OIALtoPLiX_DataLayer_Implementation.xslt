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
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:exsl="http://exslt.org/common"
	xmlns:oil="http://schemas.orm.net/OIAL"
	xmlns:ormdt="http://schemas.orm.net/ORMDataTypes"
	xmlns:plx="http://schemas.neumont.edu/CodeGeneration/PLiX"
	xmlns:prop="urn:schemas-orm-net:PLiX:CLI:Properties"
	xmlns:xs="http://www.w3.org/2001/XMLSchema"
	exclude-result-prefixes="oil ormdt"
	extension-element-prefixes="exsl">

	<xsl:import href="OIALtoPLiX_GlobalSupportFunctions.xslt"/>
	<!--<xsl:import href="../../DIL/Transforms/DILSupportFunctions.xslt"/>-->
	<xsl:param name="OIAL" />
	<xsl:output method="xml" encoding="utf-8" media-type="text/xml" indent="yes"/>

	<xsl:variable name="ModelName" select="$OIAL/@name"/>
	<xsl:variable name="ConceptTypes" select="$OIAL//oil:conceptType"/>
	<xsl:variable name="SprocFreeFragment">
		<xsl:apply-templates select="." mode="GetSprocFree"/>
	</xsl:variable>
	<xsl:variable name="SprocFree" select="boolean(exsl:node-set($SprocFreeFragment)/node())"/>
	<xsl:variable name="AllProperties" select="prop:AllProperties/prop:Properties" />
	<xsl:variable name="AllRoleSequenceUniquenessConstraints" select="$OIAL//oil:roleSequenceUniquenessConstraint"/>

	<xsl:template match="/">
		<plx:root>
			<plx:namespaceImport name="System"/>
			<plx:namespaceImport name="System.Collections.Generic"/>
			<plx:namespaceImport name="System.Collections.ObjectModel"/>
			<plx:namespaceImport name="System.ComponentModel"/>
			<plx:namespaceImport name="System.Data"/>
			<plx:namespaceImport name="System.Xml"/>
			<xsl:choose>
				<xsl:when test="$DefaultNamespace">
					<plx:namespace name="{$DefaultNamespace}">
						<xsl:apply-templates select="$OIAL" mode="OIALtoPLiX_DataLayer_Implementation">
							<xsl:with-param name="ModelContextName" select="concat($ModelName, 'Context')"/>
						</xsl:apply-templates>
					</plx:namespace>
				</xsl:when>
				<xsl:otherwise>
					<xsl:apply-templates select="$OIAL" mode="OIALtoPLiX_DataLayer_Implementation">
						<xsl:with-param name="ModelContextName" select="concat($ModelName, 'Context')"/>
					</xsl:apply-templates>
				</xsl:otherwise>
			</xsl:choose>
		</plx:root>
	</xsl:template>
	<xsl:template match="*" mode="GetSprocFree">
		<!-- Leave this empty. Return something non-empty to get SprocFree from another template that imports this one -->
	</xsl:template>
	<xsl:template match="oil:model" mode="OIALtoPLiX_DataLayer_Implementation">
		<xsl:param name="Model" select="."/>
		<xsl:param name="ModelContextName"/>
		<plx:namespace name="{$ModelName}">
			<plx:class visibility="public" modifier="sealed" name="{$ModelContextName}">
				<plx:leadingInfo>
					<plx:pragma type="region" data="{$ModelContextName}"/>
					
				</plx:leadingInfo>
				<plx:trailingInfo>
					<plx:pragma type="closeRegion" data="{$ModelContextName}"/>
				</plx:trailingInfo>
				<xsl:copy-of select="$GeneratedCodeAttribute"/>
				<xsl:copy-of select="$StructLayoutAttribute"/>
				<plx:implementsInterface dataTypeName="{concat('I', $ModelName,'Context')}"/>

				<!-- CreateCallback Delegate -->
				<plx:delegate visibility="private" name="CreateCallback">
					<plx:typeParam name="T" requireReferenceType="true"/>
					<plx:param name="reader" dataTypeName="IDataReader"/>
					<plx:returns dataTypeName="T" />
				</plx:delegate>

				<plx:delegate visibility="public" name="ConnectionDelegate">
					<plx:returns dataTypeName="IDbConnection"/>
				</plx:delegate>

				<plx:field visibility="private" readOnly="true" name="GetConnection" dataTypeName="ConnectionDelegate"/>
				
				<plx:function name=".construct"  visibility="public">
					<plx:param dataTypeName="ConnectionDelegate" name="connection"/>
					<plx:branch>
						<plx:condition>
							<plx:binaryOperator type="identityInequality">
								<plx:left>
									<plx:nameRef name="connection" type="parameter"/>
								</plx:left>
								<plx:right>
									<plx:nullKeyword/>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:assign>
							<plx:left>
								<plx:nameRef name="GetConnection"/>
							</plx:left>
							<plx:right>
								<plx:nameRef name="connection"/>
							</plx:right>
						</plx:assign>
					</plx:branch>
					<plx:fallbackBranch>
						<plx:throw>
							<plx:callNew dataTypeName="ArgumentNullException" type="new">
								<plx:passParam>
									<plx:string>connection</plx:string>
								</plx:passParam>
								<plx:passParam>
									<plx:string>ConnectionDelegate cannot be null</plx:string>
								</plx:passParam>
							</plx:callNew>
						</plx:throw>
					</plx:fallbackBranch>
				</plx:function>

				<plx:function visibility="private" modifier="static" name="GetDifferentContextsException">
					<plx:leadingInfo>
						<plx:pragma type="region" data="Exception Helpers"/>
					</plx:leadingInfo>
					<xsl:call-template name="GenerateSuppressMessageAttribute">
						<xsl:with-param name="category" select="'Microsoft.Usage'"/>
						<xsl:with-param name="checkId" select="'CA2208:InstantiateArgumentExceptionsCorrectly'"/>
					</xsl:call-template>
					<plx:returns dataTypeName="ArgumentException"/>
					<plx:return>
						<plx:callNew dataTypeName="ArgumentException">
							<plx:passParam>
								<plx:string>All objects in a relationship must be part of the same Context.</plx:string>
							</plx:passParam>
							<plx:passParam>
								<plx:string>value</plx:string>
							</plx:passParam>
						</plx:callNew>
					</plx:return>
				</plx:function>
				<plx:function visibility="private" modifier="static" name="GetConstraintEnforcementFailedException">
					<plx:trailingInfo>
						<plx:pragma type="closeRegion" data="Exception Helpers"/>
						<!--<plx:pragma type="region" data="Lookup and External Constraint Enforcement"/>-->
					</plx:trailingInfo>
					<plx:param name="paramName" dataTypeName=".string"/>
					<plx:returns dataTypeName="ArgumentException"/>
					<plx:return>
						<plx:callNew dataTypeName="ArgumentException">
							<plx:passParam>
								<plx:string>Argument failed constraint enforcement.</plx:string>
							</plx:passParam>
							<plx:passParam>
								<plx:nameRef type="parameter" name="paramName"/>
							</plx:passParam>
						</plx:callNew>
					</plx:return>
				</plx:function>
				
				
				<xsl:call-template name="GenerateModelContextLookupAndExternalConstraintEnforcementMembers">
					<xsl:with-param name="Model" select="$Model"/>
					<xsl:with-param name="ModelContextName" select="$ModelContextName"/>
					<xsl:with-param name="AllProperties" select="$AllProperties"/>
					<xsl:with-param name="AllRoleSequenceUniquenessConstraints" select="$AllRoleSequenceUniquenessConstraints"/>
				</xsl:call-template>


				<xsl:for-each select="$ConceptTypes">
					<xsl:apply-templates select="." mode="GenerateImplementationClass">
						<xsl:with-param name="Model" select="$Model"/>
						<xsl:with-param name="ModelContextName" select="$ModelContextName"/>
						<xsl:with-param name="AllRoleSequenceUniquenessConstraints" select="$AllRoleSequenceUniquenessConstraints"/>
						<xsl:with-param name="Properties" select="$AllProperties[@conceptTypeName=current()/@name]/prop:Property"/>
						<xsl:with-param name="AllProperties" select="$AllProperties"/>
					</xsl:apply-templates>
				</xsl:for-each>

				<!-- DataReaderEnumerator struct -->
				<plx:structure name="DataReaderEnumerator" visibility="private">
					<plx:typeParam name="T">
						<plx:typeConstraint dataTypeName="class"/>
					</plx:typeParam>
					<plx:implementsInterface dataTypeName="IEnumerable">
						<plx:passTypeParam dataTypeName="T"/>
					</plx:implementsInterface>
					<plx:implementsInterface dataTypeName="IEnumerator">
						<plx:passTypeParam dataTypeName="T" />
					</plx:implementsInterface>
					<plx:field readOnly="true" name="{$PrivateMemberPrefix}reader" dataTypeName="IDataReader" visibility="private"/>
					<plx:field readOnly="true" name="{$PrivateMemberPrefix}creator" dataTypeName="CreateCallback" visibility="private">
						<plx:passTypeParam dataTypeName="T"/>
					</plx:field>
					<plx:field name="{$PrivateMemberPrefix}current" dataTypeName="T" visibility="private"/>
					<plx:function name=".construct" visibility="public">
						<plx:param name="reader" dataTypeName="IDataReader"/>
						<plx:param name="creator" dataTypeName="CreateCallback">
							<plx:passTypeParam dataTypeName="T"/>
						</plx:param>
						<plx:assign>
							<plx:left>
								<plx:nameRef name="{$PrivateMemberPrefix}reader" type="local"/>
							</plx:left>
							<plx:right>
								<plx:nameRef name="reader" type="parameter"/>
							</plx:right>
						</plx:assign>
						<plx:assign>
							<plx:left>
								<plx:nameRef name="{$PrivateMemberPrefix}creator" type="local"/>
							</plx:left>
							<plx:right>
								<plx:nameRef name="creator" type="parameter"/>
							</plx:right>
						</plx:assign>
						<plx:assign>
							<plx:left>
								<plx:nameRef name="{$PrivateMemberPrefix}current" type="local"/>
							</plx:left>
							<plx:right>
								<plx:nullKeyword/>
							</plx:right>
						</plx:assign>
					</plx:function>
					<plx:property name="Current" visibility="public">
						<plx:returns dataTypeName="T"/>
						<plx:get>
							<plx:branch>
								<plx:condition>
									<plx:binaryOperator type="equality">
										<plx:left>
											<plx:callThis name="{$PrivateMemberPrefix}current" accessor="this" type="field"/>
										</plx:left>
										<plx:right>
											<plx:nullKeyword/>
										</plx:right>
									</plx:binaryOperator>
								</plx:condition>
								<plx:assign>
									<plx:left>
										<plx:callThis name="{$PrivateMemberPrefix}current" accessor="this" type="field"/>
									</plx:left>
									<plx:right>
										<plx:callThis name="{$PrivateMemberPrefix}creator" accessor="this" type="methodCall">
											<plx:passParam>
												<plx:nameRef name="{$PrivateMemberPrefix}reader" type="local"/>
											</plx:passParam>
										</plx:callThis>
									</plx:right>
								</plx:assign>
							</plx:branch>
							<plx:return>
								<plx:callThis name="{$PrivateMemberPrefix}current" accessor="this" type="field"/>
							</plx:return>
						</plx:get>
					</plx:property>
					<plx:function name="Dispose" visibility="public">
						<plx:callInstance name="Close" type="methodCall">
							<plx:callObject>
								<plx:callThis name="{$PrivateMemberPrefix}reader" accessor="this" type="field"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:function>
					
					<plx:property name="Current" visibility="privateInterfaceMember">
						<plx:interfaceMember memberName="Current" dataTypeName="IEnumerator" dataTypeQualifier="System.Collections"/>
						<plx:returns dataTypeName=".object"/>
						<plx:get>
							<plx:return>
								<plx:callThis name="Current" accessor="this" type="property"/>
							</plx:return>
						</plx:get>
					</plx:property>

					<plx:function name="MoveNext" visibility="public">
						<plx:returns dataTypeName=".boolean"/>
						<plx:assign>
							<plx:left>
								<plx:nameRef name="{$PrivateMemberPrefix}current" type="local"/>
							</plx:left>
							<plx:right>
								<plx:nullKeyword/>
							</plx:right>
						</plx:assign>
						<plx:branch>
							<plx:condition>
								<plx:unaryOperator type="booleanNot">
									<plx:callInstance name="IsClosed" type="property">
										<plx:callObject>
											<plx:nameRef name="{$PrivateMemberPrefix}reader" type="local"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:unaryOperator>
							</plx:condition>
							<plx:return>
								<plx:callInstance name="Read" type="methodCall">
									<plx:callObject>
										<plx:callThis name="{$PrivateMemberPrefix}reader" accessor="this" type="field"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:return>

						</plx:branch>
						<plx:fallbackBranch>
							<plx:return>
								<plx:falseKeyword />
							</plx:return>
						</plx:fallbackBranch>
					</plx:function>
					<plx:function name="GetEnumerator" visibility="public" >
						<plx:returns dataTypeName="DataReaderEnumerator">
							<plx:passTypeParam dataTypeName="T"/>
						</plx:returns>
						<plx:return>
							<plx:thisKeyword/>
						</plx:return>
					</plx:function>
					<plx:function name="GetEnumerator" visibility="privateInterfaceMember">
						<plx:interfaceMember memberName="GetEnumerator" dataTypeName="IEnumerable">
							<plx:passTypeParam dataTypeName="T"/>
						</plx:interfaceMember>
						<plx:returns dataTypeName="IEnumerator">
							<plx:passTypeParam dataTypeName="T"/>
						</plx:returns>
						<plx:return>
							<plx:thisKeyword/>
						</plx:return>
					</plx:function>
					<plx:function name="GetEnumerator" visibility="privateInterfaceMember">
						<plx:interfaceMember memberName="GetEnumerator" dataTypeName="IEnumerable" dataTypeQualifier="System.Collections"/>
						<plx:returns dataTypeName="IEnumerator" dataTypeQualifier="System.Collections"/>
						<plx:return>
							<plx:thisKeyword/>
						</plx:return>
					</plx:function>
					<plx:function name="Reset" visibility="privateInterfaceMember">
						<plx:interfaceMember memberName="Reset" dataTypeName="IEnumerator" dataTypeQualifier="System.Collections"/>
						<plx:throw>
							<plx:callNew type="new" dataTypeName="NotSupportedException" />
						</plx:throw>
					</plx:function>
				</plx:structure>
			</plx:class>
		</plx:namespace>
	</xsl:template>

	<xsl:template name="GenerateModelContextLookupAndExternalConstraintEnforcementMembers">
		<xsl:param name="Model"/>
		<xsl:param name="ModelContextName"/>
		<xsl:param name="AllProperties"/>
		<xsl:param name="AllRoleSequenceUniquenessConstraints"/>
		
		<!-- TODO: This will break for oil:roleSequenceUniquenessConstraint elements that contain oil:typeRef elements with more than one oil:conceptType reference by @targetConceptType. -->
		<xsl:for-each select="$AllRoleSequenceUniquenessConstraints">
			<xsl:variable name="uniqueConceptTypeName" select="parent::oil:conceptType/@name"/>
			<xsl:variable name="parametersFragment">
				<xsl:for-each select="oil:roleSequence/oil:typeRef">
					<plx:param name="{@targetChild}">
						<xsl:variable name="targetProperty" select="$AllProperties[@conceptTypeName=$uniqueConceptTypeName]/prop:Property[@name=current()/@targetChild]"/>
						<xsl:choose>
							<xsl:when test="$targetProperty/@isCustomType='false' and $targetProperty/@canBeNull='true' and $targetProperty/prop:DataType/@dataTypeName='Nullable'">
								<xsl:copy-of select="$targetProperty/prop:DataType/plx:passTypeParam/@*"/>
								<xsl:copy-of select="$targetProperty/prop:DataType/plx:passTypeParam/child::*"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:copy-of select="$targetProperty/prop:DataType/@*"/>
								<xsl:copy-of select="$targetProperty/prop:DataType/child::*"/>
							</xsl:otherwise>
						</xsl:choose>
					</plx:param>
				</xsl:for-each>
			</xsl:variable>
			<xsl:variable name="parameters" select="exsl:node-set($parametersFragment)/child::*"/>
			<xsl:variable name="passTypeParamsFragment">
				<xsl:for-each select="$parameters">
					<plx:passTypeParam>
						<xsl:copy-of select="self::plx:param/@*[not(local-name()='name')]"/>
						<xsl:copy-of select="self::plx:param/child::*"/>
					</plx:passTypeParam>
				</xsl:for-each>
			</xsl:variable>
			<xsl:variable name="passTypeParams" select="exsl:node-set($passTypeParamsFragment)/child::*"/>

			<xsl:variable name="properties" select="$AllProperties[@conceptTypeName=$uniqueConceptTypeName]/prop:Property"/>
			<xsl:variable name="ParametersWithPreferredIdentifiersFragment">
				<xsl:for-each select="$parameters">
					<xsl:choose>
						<xsl:when test="$properties[@name=current()/@name]/@isCustomType='true'">
							<xsl:call-template name="GetPreferredIdentifierProperties">
								<xsl:with-param name="Model" select="$Model"/>
								<xsl:with-param name="AllProperties" select="$AllProperties"/>
								<xsl:with-param name="TargetConceptTypeName" select="@dataTypeName"/>
								<xsl:with-param name="ParentName" select="@name"/>
							</xsl:call-template>
						</xsl:when>
						<xsl:otherwise>
							<xsl:copy-of select="."/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:for-each>
			</xsl:variable>
			<xsl:variable name="ParametersWithPreferredIdentifiers" select="exsl:node-set($ParametersWithPreferredIdentifiersFragment)/child::*"/>

			<plx:function visibility="{$ModelContextInterfaceImplementationVisibility}" name="Get{$uniqueConceptTypeName}By{@name}">
				<plx:interfaceMember memberName="Get{$uniqueConceptTypeName}By{@name}" dataTypeName="I{$ModelContextName}"/>
				<xsl:copy-of select="$parameters"/>
				<plx:returns dataTypeName="{$uniqueConceptTypeName}"/>
				<xsl:for-each select="$parameters">
					<plx:branch>
						<plx:condition>
							<plx:binaryOperator type="identityEquality">
								<plx:left>
									<plx:nameRef type="parameter" name="{@name}"/>
								</plx:left>
								<plx:right>
									<plx:nullKeyword/>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:throw>
							<plx:callNew dataTypeName="ArgumentNullException">
								<plx:passParam>
									<plx:string>
										<xsl:value-of select="@name"/>
									</plx:string>
								</plx:passParam>
							</plx:callNew>
						</plx:throw>
					</plx:branch>
				</xsl:for-each>

				<plx:local name="command" dataTypeName="IDbCommand">
					<plx:initialize>
						<plx:callInstance type="methodCall" name="CreateCommand">
							<plx:callObject>
								<plx:callThis type="methodCall" accessor="this" name="GetConnection" />
							</plx:callObject>
						</plx:callInstance>
					</plx:initialize>
				</plx:local>

				<plx:try>
					<plx:branch>
						<plx:condition>
							<plx:binaryOperator type="inequality">
								<plx:left>
									<plx:callInstance name="State" type="property">
										<plx:callObject>
											<plx:callInstance name="Connection" type="property">
												<plx:callObject>
													<plx:nameRef name="command" type="local"/>
												</plx:callObject>
											</plx:callInstance>
										</plx:callObject>
									</plx:callInstance>
								</plx:left>
								<plx:right>
									<plx:callStatic name="Open" dataTypeName="ConnectionState" type="field"/>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:callInstance name="Open" type="methodCall">
							<plx:callObject>
								<plx:callInstance name="Connection" type="property">
									<plx:callObject>
										<plx:nameRef name="command" type="local"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:callObject>
						</plx:callInstance>
					</plx:branch>

					<!-- Determine CommandType (either StoredProcedure or Text) -->
					<xsl:call-template name="GenerateIDBCommandType">
						<xsl:with-param name="commandName" select="'command'"/>
					</xsl:call-template>
					<plx:assign>
						<plx:left>
							<plx:callInstance type="property" name="CommandText">
								<plx:callObject>
									<plx:nameRef type="local"  name="command"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:left>
						<plx:right>
							<xsl:choose>
								<xsl:when test="$SprocFree">
									<!-- Nodeset representing the WHERE Clause of the SQL statement -->
									<xsl:variable name="selectStringFragment">
										<xsl:for-each select="$ParametersWithPreferredIdentifiers">
											<xsl:choose>
												<xsl:when test="@parentName">
													<plx:string>
														<xsl:value-of select="@parentName"/>_<xsl:value-of select="@name"/> = @<xsl:value-of select="@parentName"/><xsl:value-of select="@name"/><xsl:if test="position() != last()"> and </xsl:if>
													</plx:string>
												</xsl:when>
												<xsl:otherwise>
													<plx:string>
														<xsl:value-of select="@name"/> = @<xsl:value-of select="@name"/><xsl:if test="position() != last()"> and </xsl:if>
													</plx:string>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:for-each>
									</xsl:variable>

									<xsl:variable name="selectString" select="exsl:node-set($selectStringFragment)/child::*"/>
									<plx:string>
										<xsl:value-of select="concat('SELECT * FROM ', $Model/@name, '.', $uniqueConceptTypeName, ' WHERE ')"/>
										<xsl:for-each select="$selectString">
											<xsl:value-of select="."/>
										</xsl:for-each>
									</plx:string>
								</xsl:when>
								<xsl:otherwise>
									<plx:string>
										<xsl:value-of select="concat('Get',$uniqueConceptTypeName,'By',@name)"/>
									</plx:string>
								</xsl:otherwise>
							</xsl:choose>

						</plx:right>
					</plx:assign>
					<xsl:for-each select="$ParametersWithPreferredIdentifiers">
						<xsl:variable name="commandParameterFragment">
							<xsl:choose>
								<xsl:when test="@parentName">
									<commandParameter name="{@name}" parentName="{@parentName}">
										<plx:callInstance name="{@name}" type="property">
											<plx:callObject>
												<plx:nameRef type="parameter" name="{@parentName}"/>
											</plx:callObject>
										</plx:callInstance>
									</commandParameter>
								</xsl:when>
								<xsl:otherwise>
									<commandParameter name="{@name}">
										<plx:nameRef type="parameter" name="{@name}"/>
									</commandParameter>
								</xsl:otherwise>
							</xsl:choose>

						</xsl:variable>
						<xsl:variable name="commandParameter" select="exsl:node-set($commandParameterFragment)/child::*"/>
						<xsl:call-template name="GenerateIDataParameter">
							<xsl:with-param name="commandName" select="'command'"/>
							<xsl:with-param name="parameterName" select="concat(@parentName, @name)"/>
							<xsl:with-param name="parameterValue" select="$commandParameter/child::*"/>
						</xsl:call-template>

					</xsl:for-each>
					<xsl:call-template name="GenerateCreateObjectReaderBlock">
						<xsl:with-param name="ObjectToCreateName" select="$uniqueConceptTypeName"/>
						<xsl:with-param name="CommandName" select="'command'"/>
					</xsl:call-template>
					<plx:finally>
						<xsl:call-template name="GenerateCommandObjectCleanup"/>
					</plx:finally>
				</plx:try>
			</plx:function>

			<plx:function visibility="{$ModelContextInterfaceImplementationVisibility}" name="TryGet{$uniqueConceptTypeName}By{@name}">
				<plx:interfaceMember memberName="TryGet{$uniqueConceptTypeName}By{@name}" dataTypeName="I{$ModelContextName}"/>
				<xsl:copy-of select="$parameters"/>
				<plx:param type="out" name="{$uniqueConceptTypeName}" dataTypeName="{$uniqueConceptTypeName}"/>
				<plx:returns dataTypeName=".boolean"/>

				<plx:assign>
					<plx:left>
						<plx:nameRef type="local" name="{$uniqueConceptTypeName}"/>
					</plx:left>
					<plx:right>
						<plx:callThis type="methodCall" name="Get{$uniqueConceptTypeName}By{@name}">
							<xsl:for-each select="$parameters">
								<plx:passParam>
									<plx:nameRef name="{@name}"/>
								</plx:passParam>
							</xsl:for-each>
						</plx:callThis>
					</plx:right>
				</plx:assign>

				<plx:return>
					<plx:binaryOperator type="identityInequality">
						<plx:left>
							<plx:cast dataTypeName="object">
								<plx:nameRef type="local" name="{$uniqueConceptTypeName}"/>
							</plx:cast>
						</plx:left>
						<plx:right>
							<plx:nullKeyword/>
						</plx:right>
					</plx:binaryOperator>
				</plx:return>
			</plx:function>

		</xsl:for-each>

		<xsl:for-each select="$AllProperties/prop:Property[@isUnique='true' and not(@isCustomType='true')]">
			<!-- The 'On<Property>Changing' and 'On<Property>Changed' methods are generated later in another transform. -->
			<xsl:variable name="uniqueConceptTypeName" select="parent::prop:Properties/@conceptTypeName"/>

			<xsl:variable name="passTypeParamsFragment">
				<xsl:choose>
					<xsl:when test="@isCustomType='false' and @canBeNull='true' and prop:DataType/@dataTypeName='Nullable'">
						<xsl:copy-of select="prop:DataType/plx:passTypeParam"/>
					</xsl:when>
					<xsl:otherwise>
						<plx:passTypeParam>
							<xsl:copy-of select="prop:DataType/@*"/>
							<xsl:copy-of select="prop:DataType/child::*"/>
						</plx:passTypeParam>
					</xsl:otherwise>
				</xsl:choose>
				<plx:passTypeParam dataTypeName="{$uniqueConceptTypeName}"/>
			</xsl:variable>
			<xsl:variable name="passTypeParams" select="exsl:node-set($passTypeParamsFragment)/child::*"/>

			<xsl:variable name="properties" select="$AllProperties[@conceptTypeName=$uniqueConceptTypeName]/prop:Property"/>

			<!-- TODO: In Get{Thing}By{Name}, {Name} should be oil:singleRoleUniquenessConstraint/@name rather than prop:Property/@name. -->
			<plx:function visibility="{$ModelContextInterfaceImplementationVisibility}" name="Get{$uniqueConceptTypeName}By{@name}">
				<plx:interfaceMember memberName="Get{$uniqueConceptTypeName}By{@name}" dataTypeName="I{$ModelContextName}"/>
				<plx:param name="{@name}">
					<xsl:copy-of select="$passTypeParams[1]/@*"/>
					<xsl:copy-of select="$passTypeParams[1]/child::*"/>
				</plx:param>
				<plx:returns dataTypeName="{$uniqueConceptTypeName}"/>
				<xsl:if test="@canBeNull='true'">
					<plx:branch>
						<plx:condition>
							<plx:binaryOperator type="identityEquality">
								<plx:left>
									<plx:nameRef type="parameter" name="{@name}"/>
								</plx:left>
								<plx:right>
									<plx:nullKeyword/>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:throw>
							<plx:callNew dataTypeName="ArgumentNullException">
								<plx:passParam>
									<plx:string>
										<xsl:value-of select="@name"/>
									</plx:string>
								</plx:passParam>
							</plx:callNew>
						</plx:throw>
					</plx:branch>
				</xsl:if>
				<plx:local name="command" dataTypeName="IDbCommand">
					<plx:initialize>
						<plx:callInstance type="methodCall" name="CreateCommand">
							<plx:callObject>
								<plx:callThis type="methodCall" accessor="this" name="GetConnection" />
							</plx:callObject>
						</plx:callInstance>
					</plx:initialize>
				</plx:local>
				<plx:try>
					<plx:branch>
						<plx:condition>
							<plx:binaryOperator type="inequality">
								<plx:left>
									<plx:callInstance name="State" type="property">
										<plx:callObject>
											<plx:callInstance name="Connection" type="property">
												<plx:callObject>
													<plx:nameRef name="command" type="local"/>
												</plx:callObject>
											</plx:callInstance>
										</plx:callObject>
									</plx:callInstance>
								</plx:left>
								<plx:right>
									<plx:callStatic name="Open" dataTypeName="ConnectionState" type="field"/>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:callInstance name="Open" type="methodCall">
							<plx:callObject>
								<plx:callInstance name="Connection" type="property">
									<plx:callObject>
										<plx:nameRef name="command" type="local"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:callObject>
						</plx:callInstance>
					</plx:branch>

					<xsl:call-template name="GenerateIDBCommandType" />
					
					<plx:assign>
						<plx:left>
							<plx:callInstance type="property" name="CommandText">
								<plx:callObject>
									<plx:nameRef type="local" name="command"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:left>
						<plx:right>
							<xsl:choose>
								<xsl:when test="$SprocFree">
									<plx:string>
										<xsl:value-of select="concat('SELECT * FROM ', $Model/@name, '.', $uniqueConceptTypeName, ' WHERE ', @name, ' = @', @name)"/>
									</plx:string>
								</xsl:when>
								<xsl:otherwise>
									<plx:string>
										<xsl:value-of select="concat('Get',$uniqueConceptTypeName,'By',@name)"/>
									</plx:string>
								</xsl:otherwise>
							</xsl:choose>
						</plx:right>
					</plx:assign>
					<xsl:variable name="commandParameterFragment">
						<xsl:choose>
							<xsl:when test="@parentName">
								<commandParameter name="{@name}" parentName="{@parentName}">
									<plx:callInstance name="{@name}" type="property">
										<plx:callObject>
											<plx:nameRef type="parameter" name="{@parentName}"/>
										</plx:callObject>
									</plx:callInstance>
								</commandParameter>
							</xsl:when>
							<xsl:otherwise>
								<commandParameter name="{@name}">
									<plx:nameRef type="parameter" name="{@name}"/>
								</commandParameter>
							</xsl:otherwise>
						</xsl:choose>

					</xsl:variable>
					<xsl:variable name="commandParameter" select="exsl:node-set($commandParameterFragment)/child::*"/>
					<xsl:call-template name="GenerateIDataParameter">
						<xsl:with-param name="commandName" select="'command'"/>
						<xsl:with-param name="parameterName" select="concat(@parentName, @name)"/>
						<xsl:with-param name="parameterValue" select="$commandParameter/child::*"/>
					</xsl:call-template>
					<xsl:call-template name="GenerateCreateObjectReaderBlock">
						<xsl:with-param name="ObjectToCreateName" select="$uniqueConceptTypeName"/>
						<xsl:with-param name="CommandName" select="'command'"/>
					</xsl:call-template>
					<plx:finally>
						<xsl:call-template name="GenerateCommandObjectCleanup"/>
					</plx:finally>
				</plx:try>
			</plx:function>
			<!-- TODO: In TryGet{Thing}By{Name}, {Name} should be oil:singleRoleUniquenessConstraint/@name rather than prop:Property/@name. -->
			<plx:function visibility="{$ModelContextInterfaceImplementationVisibility}" name="TryGet{$uniqueConceptTypeName}By{@name}">
				<plx:interfaceMember memberName="TryGet{$uniqueConceptTypeName}By{@name}" dataTypeName="I{$ModelContextName}"/>
				<plx:param name="{@name}">
					<xsl:copy-of select="$passTypeParams[1]/@*"/>
					<xsl:copy-of select="$passTypeParams[1]/child::*"/>
				</plx:param>
				<plx:param type="out" name="{$uniqueConceptTypeName}" dataTypeName="{$uniqueConceptTypeName}"/>
				<plx:returns dataTypeName=".boolean"/>
				<plx:assign>
					<plx:left>
						<plx:nameRef type="local" name="{$uniqueConceptTypeName}"/>
					</plx:left>
					<plx:right>
						<plx:callThis type="methodCall" name="Get{$uniqueConceptTypeName}By{@name}">
							<plx:passParam>
								<plx:nameRef name="{@name}"/>
							</plx:passParam>
						</plx:callThis>
					</plx:right>
				</plx:assign>

				<plx:return>
					<plx:binaryOperator type="identityInequality">
						<plx:left>
							<plx:cast dataTypeName=".object">
								<plx:nameRef type="local" name="{$uniqueConceptTypeName}"/>
							</plx:cast>
						</plx:left>
						<plx:right>
							<plx:nullKeyword/>
						</plx:right>
					</plx:binaryOperator>
				</plx:return>
			</plx:function>
		</xsl:for-each>

	</xsl:template>

	<xsl:template match="oil:conceptType" mode="GenerateImplementationClass">
		<xsl:param name="Model"/>
		<xsl:param name="ModelContextName"/>
		<xsl:param name="AllRoleSequenceUniquenessConstraints"/>
		<xsl:param name="Properties"/>
		<xsl:param name="AllProperties"/>
		<xsl:variable name="ClassName" select="@name"/>
		<xsl:variable name="Ancestor" select="self::node()/ancestor::oil:conceptType[position() = last() and @name!=current()/@name]"/>
		<xsl:variable name="ImplementationClassName" select="concat($ClassName,$ImplementationClassSuffix)"/>
		<xsl:variable name="mandatoryProperties" select="$Properties[@mandatory='alethic']"/>
		<xsl:variable name="mandatoryPropertiesWithPreferredIdentifiersFragment">
			<xsl:for-each select="$mandatoryProperties">
				<xsl:choose>
					<xsl:when test="@isCustomType='true'">
						<xsl:call-template name="GetPreferredIdentifierProperties">
							<xsl:with-param name="Model" select="$Model"/>
							<xsl:with-param name="AllProperties" select="$AllProperties"/>
							<xsl:with-param name="TargetConceptTypeName" select="prop:DataType/@dataTypeName"/>
							<xsl:with-param name="ParentName" select="@name"/>
						</xsl:call-template>
					</xsl:when>
					<xsl:otherwise>
						<xsl:copy-of select="."/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="mandatoryPropertiesWithPreferredIdentifiers" select="exsl:node-set($mandatoryPropertiesWithPreferredIdentifiersFragment)/child::*"/>

		<plx:function visibility="{$ModelContextInterfaceImplementationVisibility}" name="Create{$ClassName}">
			<plx:leadingInfo>
				<plx:pragma type="region" data="{$ClassName}"/>
			</plx:leadingInfo>
			<plx:interfaceMember memberName="Create{$ClassName}" dataTypeName="I{$ModelContextName}"/>
			<xsl:for-each select="$mandatoryProperties[not(@isIdentity='true')]">
				<plx:param name="{@name}">
					<xsl:copy-of select="prop:DataType/@*"/>
					<xsl:copy-of select="prop:DataType/child::*"/>
				</plx:param>
			</xsl:for-each>
			<plx:returns dataTypeName="{$ClassName}"/>
			<xsl:for-each select="$mandatoryProperties[@canBeNull='true']">

				<!--We don't need to worry about Nullable here, since a property that is alethicly mandatory will never be made Nullable.-->

				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="identityEquality">
							<plx:left>
								<plx:cast type="exceptionCast" dataTypeName=".object">
									<plx:nameRef type="parameter" name="{@name}"/>
								</plx:cast>
							</plx:left>
							<plx:right>
								<plx:nullKeyword/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:throw>
						<plx:callNew dataTypeName="ArgumentNullException">
							<plx:passParam>
								<plx:string>
									<xsl:value-of select="@name"/>
								</plx:string>
							</plx:passParam>
						</plx:callNew>
					</plx:throw>
				</plx:branch>
			</xsl:for-each>

			<plx:local name="command" dataTypeName="IDbCommand">
				<plx:initialize>
					<plx:callInstance type="methodCall" name="CreateCommand">
						<plx:callObject>
							<plx:callThis type="methodCall" accessor="this" name="GetConnection" />
						</plx:callObject>
					</plx:callInstance>
				</plx:initialize>
			</plx:local>

			<xsl:call-template name="GenerateIDBCommandType" />

			<plx:assign>
				<plx:left>
					<plx:callInstance name="CommandText" type="property">
						<plx:callObject>
							<plx:nameRef name="command" type="local"/>
						</plx:callObject>
					</plx:callInstance>
				</plx:left>
				<plx:right>
					<xsl:choose>
						<xsl:when test="$SprocFree">
							<plx:string>
								<plx:string>
									<xsl:choose>
										<xsl:when test="$Ancestor">
											<xsl:value-of select="concat('INSERT INTO ', $ModelName, '.', $Ancestor/@name, ' (')"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="concat('INSERT INTO ', $ModelName, '.', $ClassName, ' (')"/>
										</xsl:otherwise>
									</xsl:choose>
									<xsl:choose>
										<xsl:when test="$mandatoryPropertiesWithPreferredIdentifiers/@parentName">
											<xsl:for-each select="$mandatoryPropertiesWithPreferredIdentifiers">
												<xsl:if test="position() != 1">
													<xsl:text>, </xsl:text>
												</xsl:if>
												<xsl:choose>
													<xsl:when test="@parentName">
														<xsl:value-of select="concat(@parentName, '_', @name)"/>
													</xsl:when>
													<xsl:otherwise>
														<xsl:value-of select="@name"/>
													</xsl:otherwise>
												</xsl:choose>
											</xsl:for-each>
										</xsl:when>
										<xsl:otherwise>
											<xsl:for-each select="$mandatoryPropertiesWithPreferredIdentifiers[not(@isIdentity = 'true')]">
												<xsl:if test="position() != 1">
													<xsl:text>, </xsl:text>
												</xsl:if>
												<xsl:value-of select="@name"/>
											</xsl:for-each>
										</xsl:otherwise>
									</xsl:choose>

									<!--<xsl:for-each select="$mandatoryPropertiesWithPreferredIdentifiers">
										<xsl:choose>
											<xsl:when test="@parentName">
												<xsl:if test="position() != 1">
													<xsl:text>, </xsl:text>
												</xsl:if>
												<xsl:value-of select="concat(@parentName, '_', @name)"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:if test="not(@isIdentity = 'true')">
													<xsl:if test="position() != 1">
														<xsl:text>, </xsl:text>
													</xsl:if>
													<xsl:value-of select="@name"/>
												</xsl:if>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:for-each>-->
									<xsl:text>) VALUES (</xsl:text>
									<xsl:choose>
										<xsl:when test="$mandatoryPropertiesWithPreferredIdentifiers/@parentName">
											<xsl:for-each select="$mandatoryPropertiesWithPreferredIdentifiers">
												<xsl:if test="position() != 1">
													<xsl:text>, </xsl:text>
												</xsl:if>
												<xsl:value-of select="concat('@',@parentName, @name)"/>
											</xsl:for-each>
										</xsl:when>
										<xsl:otherwise>
											<xsl:for-each select="$mandatoryPropertiesWithPreferredIdentifiers[not(@isIdentity = 'true')]">
												<xsl:if test="position() != 1">
													<xsl:text>, </xsl:text>
												</xsl:if>
												<xsl:value-of select="concat('@',@name)"/>
											</xsl:for-each>
										</xsl:otherwise>
									</xsl:choose>
									<!--<xsl:for-each select="$mandatoryPropertiesWithPreferredIdentifiers">
										<xsl:choose>
											<xsl:when test="@parentName">
												<xsl:if test="position() != 1">
													<xsl:text>, </xsl:text>
												</xsl:if>
												<xsl:value-of select="concat('@',@parentName, @name)"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:if test="not(@isIdentity = 'true')">
													<xsl:if test="position() != 1">
														<xsl:text>, </xsl:text>
													</xsl:if>
													<xsl:value-of select="concat('@',@name)"/>
												</xsl:if>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:for-each>-->
									<xsl:text>)</xsl:text>
									<xsl:if test="$mandatoryPropertiesWithPreferredIdentifiers[@isIdentity = 'true' and not(@parentName)]">
										<xsl:text>; SELECT @@IDENTITY;</xsl:text>
										<!-- Instead of using a parameter to get the identity, use ExecuteScalar method of the command object and just return the identity -->
										<!--<xsl:choose>
											<xsl:when test="$mandatoryPropertiesWithPreferredIdentifiers[@isIdentity = 'true']/@parentName">
												<xsl:value-of select="concat('SELECT @', $mandatoryPropertiesWithPreferredIdentifiers[@isIdentity = 'true']/@parentName, $mandatoryPropertiesWithPreferredIdentifiers[@isIdentity = 'true']/@name, ' = @@Identity;')" />
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="concat('SELECT @', $mandatoryPropertiesWithPreferredIdentifiers[@isIdentity = 'true']/@name, ' = @@Identity;')"/>
											</xsl:otherwise>
										</xsl:choose>-->
									</xsl:if>
								</plx:string>
							</plx:string>
						</xsl:when>
						<xsl:otherwise>
							<plx:string>
								<xsl:value-of select="concat('Create',$ClassName,$SprocSuffix)"/>
							</plx:string>
						</xsl:otherwise>
					</xsl:choose>
				</plx:right>				
			</plx:assign>
			<xsl:variable name="commandParameterFragment">
				<xsl:for-each select="$mandatoryPropertiesWithPreferredIdentifiers">
					<commandParameter name="{@name}">
						<xsl:attribute name="isIdentity">
							<xsl:choose>
								<xsl:when test="@isIdentity='true'">
									<xsl:value-of select="'true'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'false'"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:choose>
							<xsl:when test="@parentName">
								<xsl:attribute name="parentName">
									<xsl:value-of select="@parentName"/>
								</xsl:attribute>
								<plx:callInstance name="{@name}" type="property">
									<plx:callObject>
										<plx:nameRef type="parameter" name="{@parentName}"/>
									</plx:callObject>
								</plx:callInstance>
							</xsl:when>
							<xsl:otherwise>
								<plx:nameRef type="parameter" name="{@name}"/>
							</xsl:otherwise>
						</xsl:choose>
					</commandParameter>
				</xsl:for-each>
			</xsl:variable>
			<xsl:variable name="commandParameter" select="exsl:node-set($commandParameterFragment)/child::*"/>
			
			<xsl:for-each select="$commandParameter">
				<xsl:call-template name="GenerateIDataParameter">
					<xsl:with-param name="commandName" select="'command'"/>
					<xsl:with-param name="parameterName" select="concat(@parentName, @name)"/>
					<xsl:with-param name="parameterValue" select="*"/>
					<xsl:with-param name="isIdentity" select="@isIdentity"/>
					<xsl:with-param name="parentName" select="@parentName" />
				</xsl:call-template>
			</xsl:for-each>

			<plx:try>
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="inequality">
							<plx:left>
								<plx:callInstance name="State" type="property">
									<plx:callObject>
										<plx:callInstance name="Connection" type="property">
											<plx:callObject>
												<plx:nameRef name="command" type="local"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:callObject>
								</plx:callInstance>
							</plx:left>
							<plx:right>
								<plx:callStatic type="field" dataTypeName="ConnectionState" name="Open"/>
							</plx:right>
						</plx:binaryOperator>

					</plx:condition>
					<plx:callInstance name="Open" type="methodCall">
						<plx:callObject>
							<plx:callInstance name="Connection" type="property">
								<plx:callObject>
									<plx:nameRef name="command" type="local"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:callObject>
					</plx:callInstance>
				</plx:branch>

				<xsl:choose>
					<xsl:when test="$SprocFree">
						<plx:local name="identity" dataTypeName=".object">
							<plx:initialize>
								<plx:callInstance name="ExecuteScalar" type="methodCall">
									<plx:callObject>
										<plx:nameRef name="command" type="local"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:initialize>
						</plx:local>
					</xsl:when>
					<xsl:otherwise>
						<plx:callInstance name="ExecuteNonQuery" type="methodCall">
							<plx:callObject>
								<plx:nameRef name="command" type="local"/>
							</plx:callObject>
						</plx:callInstance>
					</xsl:otherwise>
				</xsl:choose>

				<plx:return>
					<plx:callNew dataTypeName="{concat($ClassName,$ImplementationClassSuffix)}" type="new">
						<plx:passParam>
							<plx:thisKeyword/>
						</plx:passParam>
						<xsl:for-each select="$commandParameter">
							<plx:passParam>
								<xsl:choose>
									<xsl:when test="@parentName">
										<plx:callInstance name="{@name}" type="property">
											<plx:callObject>
												<plx:nameRef name="{@parentName}" type="parameter"/>
											</plx:callObject>
										</plx:callInstance>
									</xsl:when>
									<xsl:when test="@isIdentity='true'">
										<xsl:choose>
											<xsl:when test="$SprocFree">
												<plx:nameRef name="identity" type="local"/>
											</xsl:when>
											<xsl:otherwise>
												<plx:callInstance name="Value" type="property">
													<plx:callObject>
														<plx:nameRef name="parameter{@name}" />
													</plx:callObject>
												</plx:callInstance>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<plx:nameRef name="{@name}" type="parameter"/>
									</xsl:otherwise>
								</xsl:choose>
							</plx:passParam>
						</xsl:for-each>
					</plx:callNew>
				</plx:return>
				<plx:finally>
					<xsl:call-template name="GenerateCommandObjectCleanup"/>
				</plx:finally>
			</plx:try>
		</plx:function>

		<xsl:apply-templates select="$Properties" mode="GenerateImplementationPropertyChangeMethods">
			<xsl:with-param name="Model" select="$Model"/>
			<xsl:with-param name="ModelContextName" select="$ModelContextName"/>
			<xsl:with-param name="ClassName" select="$ClassName"/>
			<xsl:with-param name="Properties" select="$Properties"/>
			<xsl:with-param name="AllRoleSequenceUniquenessConstraints" select="$AllRoleSequenceUniquenessConstraints"/>
		</xsl:apply-templates>

		<!-- Retrieve Collection Property -->
		<plx:property visibility="{$ModelContextInterfaceImplementationVisibility}" name="{$ClassName}Collection">
			<plx:interfaceMember memberName="{$ClassName}Collection" dataTypeName="I{$ModelContextName}"/>
			<plx:returns dataTypeName="IEnumerable">
				<plx:passTypeParam dataTypeName="{$ClassName}"/>
			</plx:returns>
			<plx:get>
				<plx:local name="command" dataTypeName="IDbCommand">
					<plx:initialize>
						<plx:callInstance name="CreateCommand" type="methodCall">
							<plx:callObject>
								<plx:callThis type="methodCall" accessor="this" name="GetConnection"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:initialize>
				</plx:local>
				<xsl:call-template name="GenerateIDBCommandType" />
				<plx:assign>
					<plx:left>
						<plx:callInstance name="CommandText" type="property">
							<plx:callObject>
								<plx:nameRef name="command" type="local"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:left>
					<plx:right>
						<plx:string>
							<xsl:choose>
								<xsl:when test="$SprocFree">
									<xsl:value-of select="concat('SELECT * FROM ', $Model/@name, '.', @name)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="@name"/>
									<xsl:value-of select="$CollectionSuffix"/>
									<xsl:value-of select="$SprocSuffix"/>
								</xsl:otherwise>
							</xsl:choose>
						</plx:string>
					</plx:right>
				</plx:assign>
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="inequality">
							<plx:left>
								<plx:callInstance name="State" type ="property">
									<plx:callObject>
										<plx:callInstance name="Connection" type="property">
											<plx:callObject>
												<plx:nameRef name="command" type="local"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:callObject>
								</plx:callInstance>
							</plx:left>
							<plx:right>
								<plx:callStatic name="Open" dataTypeName="ConnectionState" type="field"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:callInstance name="Open" type="methodCall">
						<plx:callObject>
							<plx:callInstance name="Connection" type="property">
								<plx:callObject>
									<plx:nameRef name="command" type="local"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:callObject>
					</plx:callInstance>
				</plx:branch>

				<plx:return>
					<plx:callNew dataTypeName="DataReaderEnumerator" type="new">
						<plx:passTypeParam dataTypeName="{@name}" />
						<plx:passParam>
							<plx:callInstance name="ExecuteReader" type="methodCall">
								<plx:callObject>
									<plx:nameRef name="command" type="local"/>
								</plx:callObject>
								<plx:passParam>
									<plx:binaryOperator type ="bitwiseOr">
										<plx:left>
											<plx:callStatic name="SingleResult" dataTypeName="CommandBehavior" type="field" />
										</plx:left>
										<plx:right>
											<plx:callStatic name="CloseConnection" dataTypeName="CommandBehavior" type="field"/>
										</plx:right>
									</plx:binaryOperator>
								</plx:passParam>
							</plx:callInstance>
						</plx:passParam>
						<plx:passParam>
							<plx:callNew dataTypeName="CreateCallback" type="new">
								<plx:passTypeParam dataTypeName="{@name}" />
								<plx:passParam>
									<plx:callThis name="Create{@name}" accessor="this" type="methodReference"/>
								</plx:passParam>
							</plx:callNew>
						</plx:passParam>
					</plx:callNew>
				</plx:return>
			</plx:get>
		</plx:property>

		<!-- Create{@name} -->
		<xsl:variable name="uniqueConceptTypeName" select="@name"/>
		<xsl:variable name="properties" select="$AllProperties[@conceptTypeName=$uniqueConceptTypeName]/prop:Property"/>
		<plx:function name="Create{$uniqueConceptTypeName}" visibility="private">
			<plx:param name="reader" dataTypeName="IDataReader"/>
			<plx:returns dataTypeName="{$uniqueConceptTypeName}"/>
			<xsl:for-each select="$mandatoryPropertiesWithPreferredIdentifiers">
				<!-- Generate integer place holder of the ordinal values of the columns based on name -->
				<plx:local name="ordinal{@parentName}{@name}" dataTypeName=".i4">
					<plx:initialize>
						<plx:callInstance name="GetOrdinal" type="methodCall">
							<plx:callObject>
								<plx:nameRef name="reader" type="parameter"/>
							</plx:callObject>
							<plx:passParam>
								<plx:string>
									<xsl:choose>
										<xsl:when test="@parentName">
											<xsl:value-of select="concat(@parentName, '_', @name)"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="@name"/>
										</xsl:otherwise>
									</xsl:choose>
								</plx:string>
							</plx:passParam>
						</plx:callInstance>
					</plx:initialize>
				</plx:local>
			</xsl:for-each>
			<plx:return>
				<plx:callNew dataTypeName="{$uniqueConceptTypeName}Core">
					<plx:passParam>
						<plx:thisKeyword/>
					</plx:passParam>
					<xsl:for-each select="$mandatoryPropertiesWithPreferredIdentifiers">
						<xsl:call-template name="GenerateReaderGetItem">
							<xsl:with-param name="reader" select="'reader'"/>
							<xsl:with-param name="dataType" select="*/@dataTypeName"/>
							<xsl:with-param name="ordinalName" select="concat('ordinal', @parentName, @name)"/>
						</xsl:call-template>
					</xsl:for-each>
				</plx:callNew>
			</plx:return>
		</plx:function>
		<plx:class visibility="private" modifier="sealed" name="{$ImplementationClassName}">
			<plx:leadingInfo>
				<plx:pragma type="region" data="{$ImplementationClassName}"/>
			</plx:leadingInfo>
			<plx:trailingInfo>
				<plx:pragma type="closeRegion" data="{$ImplementationClassName}"/>
				<plx:pragma type="closeRegion" data="{$ClassName}"/>
			</plx:trailingInfo>
			<xsl:copy-of select="$StructLayoutAttribute"/>
			<plx:derivesFromClass dataTypeName="{$ClassName}"/>
			<plx:function visibility="public" name=".construct">
				<plx:param name="context" dataTypeName="{$ModelContextName}"/>
				<xsl:for-each select="$mandatoryPropertiesWithPreferredIdentifiers">
					<plx:param name="{@parentName}{@name}">
						<xsl:copy-of select="prop:DataType/@*"/>
						<xsl:copy-of select="prop:DataType/child::*"/>
					</plx:param>
				</xsl:for-each>
				<plx:assign>
					<plx:left>
						<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}Context"/>
					</plx:left>
					<plx:right>
						<plx:nameRef type="parameter" name="context"/>
					</plx:right>
				</plx:assign>

				<xsl:for-each select="$mandatoryPropertiesWithPreferredIdentifiers">
					<plx:assign>
						<plx:left>
							<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}{@parentName}{@name}"/>
						</plx:left>
						<plx:right>
							<plx:nameRef type="parameter" name="{@parentName}{@name}"/>
						</plx:right>
					</plx:assign>
				</xsl:for-each>
			</plx:function>

			<plx:field visibility="private" readOnly="true" name="{$PrivateMemberPrefix}Context" dataTypeName="{$ModelContextName}"/>
			<plx:property visibility="public" modifier="override" name="Context">
				<plx:returns dataTypeName="{$ModelContextName}"/>
				<plx:get>
					<plx:return>
						<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}Context"/>
					</plx:return>
				</plx:get>
			</plx:property>

			<xsl:apply-templates select="$Properties" mode="GenerateImplementationProperty">
				<xsl:with-param name="ClassName" select="$ClassName"/>
				<xsl:with-param name="AllRoleSequenceUniquenessConstraints" select="$AllRoleSequenceUniquenessConstraints"/>
				<xsl:with-param name="Model" select="$Model"/>
				<xsl:with-param name="AllProperties" select="$AllProperties"/>
			</xsl:apply-templates>
		</plx:class>
	</xsl:template>

	<!-- This template generates all the properties of the core class -->
	<xsl:template match="prop:Property" mode="GenerateImplementationProperty">
		<xsl:param name="ClassName"/>
		<xsl:param name="AllRoleSequenceUniquenessConstraints"/>
		<xsl:param name="Model"/>
		<xsl:param name="AllProperties"/>

		<xsl:variable name="NonCollectionCustomTypePreferredIdentifiersFragment">
			<xsl:if test="@isCustomType='true' and not(@isCollection='true')">
				<xsl:call-template name="GetPreferredIdentifierProperties">
					<xsl:with-param name="Model" select="$Model"/>
					<xsl:with-param name="AllProperties" select="$AllProperties"/>
					<xsl:with-param name="TargetConceptTypeName" select="prop:DataType/@dataTypeName"/>
					<xsl:with-param name="ParentName" select="@name"/>
				</xsl:call-template>
			</xsl:if>
		</xsl:variable>
		<xsl:variable name="NonCollectionCustomTypePreferredIdentifiers" select="exsl:node-set($NonCollectionCustomTypePreferredIdentifiersFragment)/child::*"/>

		<!-- Generate variable that has the preferred identifier(s) of the class -->
		<xsl:variable name="ConceptTypePreferredIdentifiersFragment" >
			<xsl:for-each select="$Model/oil:conceptType[@name=$ClassName]">
				<xsl:choose>
					<xsl:when test="oil:roleSequenceUniquenessConstraint/@isPreferred='true'">
						<xsl:for-each select="oil:conceptTypeRef[@name=current()/oil:roleSequenceUniquenessConstraint[@isPreferred='true']/oil:roleSequence/oil:typeRef/@targetChild]">
							<xsl:call-template name="GetPreferredIdentifierProperties">
								<xsl:with-param name="Model" select="$Model"/>
								<xsl:with-param name="AllProperties" select="$AllProperties"/>
								<xsl:with-param name="TargetConceptTypeName" select="@target"/>
								<xsl:with-param name="ParentName" select="@name"/>
							</xsl:call-template>
						</xsl:for-each>
						<xsl:for-each select="oil:informationType[@name=current()/oil:roleSequenceUniquenessConstraint[@isPreferred='true']/oil:roleSequence/oil:typeRef/@targetChild]">
							<xsl:copy-of select="$AllProperties[@conceptTypeName=$ClassName]/prop:Property[@name=current()/@name]"/>
						</xsl:for-each>
					</xsl:when>
					<xsl:otherwise>
						<xsl:for-each select="oil:informationType[oil:singleRoleUniquenessConstraint/@isPreferred='true']">
							<xsl:copy-of select="$AllProperties[@conceptTypeName=$ClassName]/prop:Property[@name=current()/@name]"/>
						</xsl:for-each>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="ConceptTypePreferredIdentifiers" select="exsl:node-set($ConceptTypePreferredIdentifiersFragment)/child::*"/>
		
		<xsl:choose>
			<xsl:when test="@isCollection='true'"/>
			<xsl:when test="@isCustomType='true'">
				<xsl:for-each select="$NonCollectionCustomTypePreferredIdentifiers">
					<plx:field visibility="private" readOnly="{@isCollection}" name="{$PrivateMemberPrefix}{@parentName}{@name}">
						<xsl:copy-of select="prop:DataType/@*"/>
						<xsl:copy-of select="prop:DataType/child::*"/>
					</plx:field>
				</xsl:for-each>
			</xsl:when>
			<xsl:otherwise>
				<plx:field visibility="private" readOnly="{@isCollection}" name="{$PrivateMemberPrefix}{@name}">
					<xsl:copy-of select="prop:DataType/@*"/>
					<xsl:copy-of select="prop:DataType/child::*"/>
					<xsl:if test="$GenerateAccessedThroughPropertyAttribute">
						<plx:attribute dataTypeName="AccessedThroughPropertyAttribute" dataTypeQualifier="System.Runtime.CompilerServices">
							<plx:passParam>
								<plx:string>
									<xsl:value-of select="@name"/>
								</plx:string>
							</plx:passParam>
						</plx:attribute>
					</xsl:if>
				</plx:field>
			</xsl:otherwise>
		</xsl:choose>
		<plx:property visibility="public" modifier="override" name="{@name}">
			<plx:returns>
				<xsl:copy-of select="prop:DataType/@*"/>
				<xsl:copy-of select="prop:DataType/child::*"/>
			</plx:returns>
			<plx:get>
				<xsl:choose>
					<xsl:when test="@isCollection='true'">
						<!-- Query for other endpoints using "@oppositeName = this" -->
						<xsl:variable name="procedureName" select="concat('Get',prop:DataType/plx:passTypeParam/@dataTypeName,'Via',@oppositeName)"/>
						<xsl:variable name="ThisConceptTypePreferredIdentifiersFragment">
							<xsl:call-template name="GetPreferredIdentifierProperties">
								<xsl:with-param name="Model" select="$Model"/>
								<xsl:with-param name="AllProperties" select="$AllProperties"/>
								<xsl:with-param name="TargetConceptTypeName" select="$ClassName"/>
								<xsl:with-param name="ParentName" select="@name"/>
							</xsl:call-template>
						</xsl:variable>
						<xsl:variable name="ThisConceptTypePreferredIdentifiers" select="exsl:node-set($ThisConceptTypePreferredIdentifiersFragment)/child::*"/>
						<plx:local name="command" dataTypeName="IDbCommand">
							<plx:initialize>
								<plx:callInstance name="CreateCommand" type="methodCall">
									<plx:callObject>
										<plx:callInstance name="GetConnection" type="delegateCall">
											<plx:callObject>
												<plx:callThis accessor="this" name="{$PrivateMemberPrefix}Context" type="field"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:callObject>
								</plx:callInstance>
							</plx:initialize>
						</plx:local>
						
						<xsl:call-template name="GenerateIDBCommandType" />
						
						<plx:assign>
							<plx:left>
								<plx:callInstance name="CommandText" type="property">
									<plx:callObject>
										<plx:nameRef name="command" type="local"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:left>
							<plx:right>
								<plx:string>
									<xsl:choose>
										<xsl:when test="$SprocFree">
											<xsl:value-of select="concat('SELECT * FROM ', $Model/@name, '.', prop:DataType/plx:passTypeParam/@dataTypeName, ' WHERE ', @oppositeName, '_', $ThisConceptTypePreferredIdentifiers/@name, ' = @', $ThisConceptTypePreferredIdentifiers/@name)"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="$procedureName"/>
											<xsl:value-of select="$SprocSuffix"/>
										</xsl:otherwise>
									</xsl:choose>
								</plx:string>
							</plx:right>
						</plx:assign>



						<xsl:for-each select="$ThisConceptTypePreferredIdentifiers">
							<xsl:variable name="commandParameterFragment">
								<xsl:choose>
									<xsl:when test="$NonCollectionCustomTypePreferredIdentifiers">
										<xsl:for-each select="$NonCollectionCustomTypePreferredIdentifiers">
											<commandParameter name="{@name}" parentName="{@parentName}">
												<plx:callInstance type="property">
													<xsl:copy-of select="@name"/>
													<plx:callObject>
														<plx:callThis name="{@name}" accessor="this" type="property"/>
													</plx:callObject>
												</plx:callInstance>
											</commandParameter>
										</xsl:for-each>
									</xsl:when>
									<xsl:otherwise>
										<commandParameter name="{@name}">
											<plx:callThis name="{@name}" accessor="this" type="property"/>
										</commandParameter>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<xsl:variable name="commandParameter" select="exsl:node-set($commandParameterFragment)/child::*"/>
							<xsl:for-each select="$commandParameter">
								<xsl:call-template name="GenerateIDataParameter">
									<xsl:with-param name="commandName" select="'command'"/>
									<xsl:with-param name="parameterName" select="concat(@parentName,@name)"/>
									<xsl:with-param name="parameterValue" select="*"/>
								</xsl:call-template>
							</xsl:for-each>
						</xsl:for-each>
						<plx:branch>
							<plx:condition>
								<plx:binaryOperator type="inequality">
									<plx:left>
										<plx:callInstance name="State" type ="property">
											<plx:callObject>
												<plx:callInstance name="Connection" type="property">
													<plx:callObject>
														<plx:nameRef name="command" type="local"/>
													</plx:callObject>
												</plx:callInstance>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:callStatic name="Open" dataTypeName="ConnectionState" type="field"/>
									</plx:right>
								</plx:binaryOperator>
							</plx:condition>
							<plx:callInstance name="Open" type="methodCall">
								<plx:callObject>
									<plx:callInstance name="Connection" type="property">
										<plx:callObject>
											<plx:nameRef name="command" type="local"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:callObject>
							</plx:callInstance>
						</plx:branch>
						<plx:return>
							<plx:callNew dataTypeName="DataReaderEnumerator" type="new">
								<plx:passTypeParam dataTypeName="{prop:DataType/plx:passTypeParam/@dataTypeName}" />
								<plx:passParam>
									<plx:callInstance name="ExecuteReader" type="methodCall">
										<plx:callObject>
											<plx:nameRef name="command" type="local"/>
										</plx:callObject>
										<plx:passParam>
											<plx:binaryOperator type ="bitwiseOr">
												<plx:left>
													<plx:callStatic name="SingleResult" dataTypeName="CommandBehavior" type="field" />
												</plx:left>
												<plx:right>
													<plx:callStatic name="CloseConnection" dataTypeName="CommandBehavior" type="field"/>
												</plx:right>
											</plx:binaryOperator>
										</plx:passParam>
									</plx:callInstance>
								</plx:passParam>
								<plx:passParam>
									<plx:callNew dataTypeName="CreateCallback" type="new">
										<plx:passTypeParam dataTypeName="{prop:DataType/plx:passTypeParam/@dataTypeName}" />
										<plx:passParam>
											<plx:callInstance name="Create{prop:DataType/plx:passTypeParam/@dataTypeName}" type="methodReference">
												<plx:callObject>
													<plx:callThis accessor="this" name="{$PrivateMemberPrefix}Context" type="field"/>
												</plx:callObject>
											</plx:callInstance>
										</plx:passParam>
									</plx:callNew>
								</plx:passParam>
							</plx:callNew>
						</plx:return>
					</xsl:when>
					<xsl:when test="@isCustomType='true'">
						<!-- Query for other endpoint using $PreferredIdentifiers -->
						<xsl:variable name="isSingleRoleUniqueness" select="count($NonCollectionCustomTypePreferredIdentifiers)=1"/>
						<xsl:variable name="methodName">
							<xsl:value-of select="concat('Get',prop:DataType/@dataTypeName,'By')"/>
							<xsl:choose>
								<xsl:when test="$isSingleRoleUniqueness">
									<!-- TODO: In Get{Thing}By{Name}, {Name} should be oil:singleRoleUniquenessConstraint/@name rather than prop:Property/@name. -->
									<xsl:value-of select="$NonCollectionCustomTypePreferredIdentifiers/@name"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$Model//oil:conceptType[@name=current()/prop:DataType/@dataTypeName]/oil:roleSequenceUniquenessConstraint[@isPreferred='true']/@name"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<plx:return>
							<plx:callInstance type="methodCall" name="{$methodName}">
								<plx:callObject>
									<plx:callThis accessor="this" type="property" name="Context"/>
								</plx:callObject>
								<xsl:for-each select="$NonCollectionCustomTypePreferredIdentifiers">
									<plx:passParam>
										<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}{@parentName}{@name}"/>
									</plx:passParam>
								</xsl:for-each>
							</plx:callInstance>
						</plx:return>
					</xsl:when>
					<xsl:otherwise>
						<plx:return>
							<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}{@name}"/>
						</plx:return>
					</xsl:otherwise>
				</xsl:choose>
			</plx:get>
			<xsl:if test="not(@isCollection='true') and not(@isIdentity='true')">
				<plx:set>
					<xsl:if test="@mandatory='alethic' and @canBeNull='true'">
						<!-- We don't need to worry about Nullable here, since a property that is alethicly mandatory will never be made Nullable. -->
						<plx:branch>
							<plx:condition>
								<plx:binaryOperator type="identityEquality">
									<plx:left>
										<plx:cast type="exceptionCast" dataTypeName=".object">
											<plx:valueKeyword/>
										</plx:cast>
									</plx:left>
									<plx:right>
										<plx:nullKeyword/>
									</plx:right>
								</plx:binaryOperator>
							</plx:condition>
							<plx:throw>
								<plx:callNew dataTypeName="ArgumentNullException">
									<plx:passParam>
										<plx:string>value</plx:string>
									</plx:passParam>
								</plx:callNew>
							</plx:throw>
						</plx:branch>
					</xsl:if>
					<xsl:choose>
						<xsl:when test="$NonCollectionCustomTypePreferredIdentifiers">
							<xsl:for-each select="$NonCollectionCustomTypePreferredIdentifiers">
								<plx:local name="oldValue{@parentName}{@name}" dataTypeName="{*/@dataTypeName}">
									<plx:initialize>
										<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}{@parentName}{@name}"/>
									</plx:initialize>
								</plx:local>
							</xsl:for-each>
						</xsl:when>
						<xsl:otherwise>
							<plx:local name="oldValue">
								<xsl:copy-of select="prop:DataType/@*"/>
								<xsl:copy-of select="prop:DataType/child::*"/>
								<plx:initialize>
									<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}{@name}"/>
								</plx:initialize>
							</plx:local>
						</xsl:otherwise>
					</xsl:choose>

					<plx:branch>
						<plx:condition>
							<xsl:choose>
								<xsl:when test="@isCustomType='true'">
									<xsl:call-template name="GenerateCompoundComparison">
										<xsl:with-param name="Identifiers" select="$NonCollectionCustomTypePreferredIdentifiers"/>
										<xsl:with-param name="mainOperator" select="'booleanOr'"/>
										<xsl:with-param name="innerOperator" select="'inequality'"/>
										<xsl:with-param name="operationCount" select="count($NonCollectionCustomTypePreferredIdentifiers)"/>
										<xsl:with-param name="currentPosition" select="'1'"/>
									</xsl:call-template>
								</xsl:when>
								<xsl:when test="@canBeNull='false'">
									<plx:binaryOperator type="inequality">
										<plx:left>
											<plx:nameRef type="local" name="oldValue"/>
										</plx:left>
										<plx:right>
											<plx:valueKeyword/>
										</plx:right>
									</plx:binaryOperator>
								</xsl:when>
								<xsl:when test="prop:DataType/@dataTypeName='Nullable'">
									<plx:binaryOperator type="booleanOr">
										<plx:left>
											<plx:binaryOperator type="inequality">
												<plx:left>
													<plx:callInstance type="methodCall" name="GetValueOrDefault">
														<plx:callObject>
															<plx:nameRef type="local" name="oldValue"/>
														</plx:callObject>
													</plx:callInstance>
												</plx:left>
												<plx:right>
													<plx:callInstance type="methodCall" name="GetValueOrDefault">
														<plx:callObject>
															<plx:valueKeyword/>
														</plx:callObject>
													</plx:callInstance>
												</plx:right>
											</plx:binaryOperator>
										</plx:left>
										<plx:right>
											<plx:binaryOperator type="inequality">
												<plx:left>
													<plx:callInstance type="property" name="HasValue">
														<plx:callObject>
															<plx:nameRef type="local" name="oldValue"/>
														</plx:callObject>
													</plx:callInstance>
												</plx:left>
												<plx:right>
													<plx:callInstance type="property" name="HasValue">
														<plx:callObject>
															<plx:valueKeyword/>
														</plx:callObject>
													</plx:callInstance>
												</plx:right>
											</plx:binaryOperator>
										</plx:right>
									</plx:binaryOperator>
								</xsl:when>
								<xsl:otherwise>
									<plx:unaryOperator type="booleanNot">
										<plx:callStatic type="methodCall" name="Equals" dataTypeName=".object">
											<plx:passParam>
												<plx:nameRef type="local" name="oldValue"/>
											</plx:passParam>
											<plx:passParam>
												<plx:valueKeyword/>
											</plx:passParam>
										</plx:callStatic>
									</plx:unaryOperator>
								</xsl:otherwise>
							</xsl:choose>
						</plx:condition>
						<plx:branch>
							<plx:condition>
								<plx:callThis accessor="base" type="methodCall" name="On{@name}Changing">
									<plx:passParam>
										<plx:valueKeyword/>
									</plx:passParam>
								</plx:callThis>
							</plx:condition>
							<plx:local name="command" dataTypeName="IDbCommand">
								<plx:initialize>
									<plx:callInstance type="methodCall" name="CreateCommand">
										<plx:callObject>
											<plx:callInstance type="methodCall" name="GetConnection">
												<plx:callObject>
													<plx:callThis type="property" accessor="this" name="Context" />
												</plx:callObject>
											</plx:callInstance>
										</plx:callObject>
									</plx:callInstance>
								</plx:initialize>
							</plx:local>
							<xsl:call-template name="GenerateIDBCommandType" />
							<plx:assign>
								<plx:left>
									<plx:callInstance type="property" name="CommandText">
										<plx:callObject>
											<plx:nameRef type="local" name="command"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:left>
								<plx:right>
									<plx:string>
										<xsl:choose>
											<xsl:when test="$SprocFree">
												<xsl:value-of select="concat('UPDATE ', $Model/@name, '.', $ClassName, ' SET ')"/>
												<!-- Set the new value of the column -->
												<xsl:choose>
													<xsl:when test="$NonCollectionCustomTypePreferredIdentifiers">
														<xsl:value-of select="concat($NonCollectionCustomTypePreferredIdentifiers/@parentName, '_', $NonCollectionCustomTypePreferredIdentifiers/@name, ' = @New', $NonCollectionCustomTypePreferredIdentifiers/@parentName, $NonCollectionCustomTypePreferredIdentifiers/@name)"/>
													</xsl:when>
													<xsl:otherwise>
														<xsl:value-of select="concat(@name, ' = @New', @name)"/>
													</xsl:otherwise>
												</xsl:choose>
												<xsl:text> WHERE </xsl:text>
												<!-- Build WHERE condition(s) -->
												<xsl:for-each select="$ConceptTypePreferredIdentifiers">
													<xsl:choose>
														<xsl:when test="@parentName">
															<xsl:value-of select="concat(@parentName, '_', @name, ' = @', @parentName, @name)"/>
														</xsl:when>
														<xsl:otherwise>
															<xsl:value-of select="concat(@name, ' = @', @name)"/>
														</xsl:otherwise>
													</xsl:choose>
													<xsl:if test="position() != last()">
														<xsl:text> AND </xsl:text>
													</xsl:if>
												</xsl:for-each>
											</xsl:when>
											<xsl:otherwise>
												<!-- Uses stored procedures to update -->
												<xsl:choose>
													<xsl:when test="$NonCollectionCustomTypePreferredIdentifiers">
														<xsl:value-of select="concat('Update',$ClassName,'Via',$NonCollectionCustomTypePreferredIdentifiers/@parentName,$NonCollectionCustomTypePreferredIdentifiers/@name,$SprocSuffix)"/>
													</xsl:when>
													<xsl:otherwise>
														<xsl:value-of select="concat('Update',$ClassName,@name,$SprocSuffix)"/>
													</xsl:otherwise>
												</xsl:choose>
											</xsl:otherwise>
										</xsl:choose>
									</plx:string>
								</plx:right>
							</plx:assign>
							<!-- Generate parameter(s) for new values -->
							<xsl:variable name="commandParameterFragment">
								<xsl:choose>
									<xsl:when test="$NonCollectionCustomTypePreferredIdentifiers">
										<xsl:for-each select="$NonCollectionCustomTypePreferredIdentifiers">
											<commandParameter name="{@name}" parentName="{@parentName}">
												<plx:callInstance type="property">
													<xsl:copy-of select="@name"/>
													<plx:callObject>
														<plx:valueKeyword/>
													</plx:callObject>
												</plx:callInstance>
											</commandParameter>
										</xsl:for-each>
									</xsl:when>
									<xsl:otherwise>
										<commandParameter name="{@name}">
											<plx:valueKeyword/>
										</commandParameter>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<xsl:variable name="commandParameter" select="exsl:node-set($commandParameterFragment)/child::*"/>
							<xsl:for-each select="$commandParameter">
								<xsl:call-template name="GenerateIDataParameter">
									<xsl:with-param name="commandName" select="'command'"/>
									<xsl:with-param name="parameterName" select="concat('New',@parentName,@name)"/>
									<xsl:with-param name="parameterValue" select="*"/>
								</xsl:call-template>
							</xsl:for-each>

							
							<!-- Generate parameters variable for each preferred identifier -->
							<xsl:variable name="commandParametersFragment">
								<xsl:for-each select="$ConceptTypePreferredIdentifiers">
									<commandParameter name="{@name}" parentName="{@parentName}">
										<plx:callThis accessor="this" name="{$PrivateMemberPrefix}{@parentName}{@name}" type="property"/>
									</commandParameter>
								</xsl:for-each>
							</xsl:variable>
							<xsl:variable name="commandParameters" select="exsl:node-set($commandParametersFragment)/child::*"/>
							<!-- Generate IDataParameters code -->
							<xsl:for-each select="$commandParameters">
								<xsl:call-template name="GenerateIDataParameter">
									<xsl:with-param name="commandName" select="'command'"/>
									<xsl:with-param name="parameterName" select="concat(@parentName,@name)"/>
									<xsl:with-param name="parameterValue" select="*"/>
								</xsl:call-template>
							</xsl:for-each>
							<plx:branch>
								<plx:condition>
									<plx:binaryOperator type="inequality">
										<plx:left>
											<plx:callInstance name="State" type="property">
												<plx:callObject>
													<plx:callInstance name="Connection" type="property">
														<plx:callObject>
															<plx:nameRef name="command" type="local"/>
														</plx:callObject>
													</plx:callInstance>
												</plx:callObject>
											</plx:callInstance>
										</plx:left>
										<plx:right>
											<plx:callStatic name="Open" dataTypeName="ConnectionState" type="property"/>
										</plx:right>
									</plx:binaryOperator>
								</plx:condition>
								<plx:callInstance name="Open" type="methodCall">
									<plx:callObject>
										<plx:callInstance name="Connection" type="property">
											<plx:callObject>
												<plx:nameRef name="command" type="local"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:callObject>
								</plx:callInstance>
							</plx:branch>
							<plx:try>
								<plx:callInstance type="methodCall" name="ExecuteNonQuery">
									<plx:callObject>
										<plx:nameRef type="local" name="command"/>
									</plx:callObject>
								</plx:callInstance>
								<plx:assign>
									<xsl:choose>
										<xsl:when test="$NonCollectionCustomTypePreferredIdentifiers">
											<xsl:for-each select="$NonCollectionCustomTypePreferredIdentifiers">
												<plx:left>
													<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}{@parentName}{@name}"/>
												</plx:left>
												<plx:right>
													<plx:callInstance name="{@name}" type="property">
														<plx:callObject>
															<plx:valueKeyword/>
														</plx:callObject>
													</plx:callInstance>
												</plx:right>
											</xsl:for-each>
										</xsl:when>
										<xsl:otherwise>
											<plx:left>
												<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}{@name}"/>
											</plx:left>
											<plx:right>
												<plx:valueKeyword/>
											</plx:right>
										</xsl:otherwise>
									</xsl:choose>
								</plx:assign>
								<!-- TODO: Needs decision made as to how the ChangedEvent should be raised.
										in-memory model passes in the old object.  The data access version only
										keeps track of the references for custom types.  Either modify the event
										to accept just the reference or create an instance of the previous object
										to pass to the current way the event is defined.  Issue with passing in the old
										object is that it would require a trip to the DB to build it, in essence each
										set method will require at least 2 transactions against the DB (get the old info
										and update with the new info).-->
								<!--<plx:callThis accessor="base" type="methodCall" name="Raise{@name}ChangedEvent">
									<plx:passParam>
										<plx:nameRef type="local" name="oldValue"/>
									</plx:passParam>
								</plx:callThis>-->
								<plx:finally>
									<xsl:call-template name="GenerateCommandObjectCleanup"/>
								</plx:finally>
							</plx:try>
						</plx:branch>
					</plx:branch>
				</plx:set>
			</xsl:if>
		</plx:property>
	</xsl:template>

	<xsl:template name="GetPreferredIdentifierProperties">
		<xsl:param name="Model"/>
		<xsl:param name="AllProperties"/>
		<xsl:param name="TargetConceptTypeName"/>
		<xsl:param name="ParentName"/>
		<xsl:variable name="targetConceptType" select="$Model//oil:conceptType[@name=$TargetConceptTypeName]"/>
		<xsl:variable name="targetConceptTypeProperties" select="$AllProperties[@conceptTypeName=$TargetConceptTypeName]/child::*"/>
		<xsl:variable name="singleRolePreferred" select="$targetConceptType/oil:*[oil:singleRoleUniquenessConstraint/@isPreferred='true']"/>
		<xsl:variable name="roleSequencePreferredUniquenessConstraint" select="$targetConceptType/oil:roleSequenceUniquenessConstraint[@isPreferred='true']"/>
		<xsl:variable name="roleSequencePreferredRolesFragment">
			<!-- This for-each is to preserve the role order from the uniqueness constraint. -->
			<xsl:for-each select="$roleSequencePreferredUniquenessConstraint/oil:roleSequence/oil:typeRef">
				<xsl:copy-of select="$targetConceptType/oil:*[@name=current()/@targetChild]"/>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="preferredRoles" select="$singleRolePreferred | exsl:node-set($roleSequencePreferredRolesFragment)/child::*"/>
		<xsl:choose>
			<xsl:when test="$preferredRoles">
				<xsl:for-each select="$preferredRoles">
					<xsl:choose>
						<xsl:when test="self::oil:informationType">
							<xsl:for-each select="$targetConceptTypeProperties[@name=current()/@name]">
								<xsl:copy>
									<xsl:attribute name="parentName">
										<xsl:value-of select="$ParentName"/>
									</xsl:attribute>
									<xsl:copy-of select="node()|@*"/>
								</xsl:copy>
							</xsl:for-each>
						</xsl:when>
						<xsl:when test="self::oil:conceptTypeRef">
							<xsl:call-template name="GetPreferredIdentifierProperties">
								<xsl:with-param name="Model" select="$Model"/>
								<xsl:with-param name="AllProperties" select="$AllProperties"/>
								<!-- CHANGE: @targetConceptType to @target -->
								<xsl:with-param name="TargetConceptTypeName" select="@target"/>
								<xsl:with-param name="ParentName" select="$ParentName"/>
							</xsl:call-template>
						</xsl:when>
					</xsl:choose>
				</xsl:for-each>
			</xsl:when>
			<xsl:when test="$targetConceptType/parent::oil:conceptType">
				<xsl:call-template name="GetPreferredIdentifierProperties">
					<xsl:with-param name="Model" select="$Model"/>
					<xsl:with-param name="AllProperties" select="$AllProperties"/>
					<xsl:with-param name="TargetConceptTypeName" select="$targetConceptType/parent::oil:conceptType/@name"/>
					<xsl:with-param name="ParentName" select="$ParentName"/>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:message terminate="yes">An oil:conceptType must have a preferred identifier.</xsl:message>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="GenerateReaderGetItem">
		<xsl:param name="reader" select="'reader'"/>
		<xsl:param name="dataType"/>
		<xsl:param name="ordinalName"/>

		<xsl:variable name="methodFragment">
			<method>
				<xsl:attribute name="name">
					<xsl:choose>
						<xsl:when test="$dataType='.boolean'">
							<xsl:value-of select="'GetBoolean'"/>
						</xsl:when>
						<xsl:when test="$dataType='.char'">
							<xsl:value-of select="'GetChar'"/>
						</xsl:when>
						<xsl:when test="$dataType='.date'">
							<xsl:value-of select="'GetDateTime'"/>
						</xsl:when>
						<xsl:when test="$dataType='.u1' or $dataType='.i1'">
							<xsl:value-of select="'GetByte'"/>
						</xsl:when>
						<xsl:when test="$dataType='.object'">
							<xsl:value-of select="'GetValue'"/>
						</xsl:when>
						<xsl:when test="$dataType='.r4'">
							<xsl:value-of select="'GetFloat'"/>
						</xsl:when>
						<xsl:when test="$dataType='.r8'">
							<xsl:value-of select="'GetDouble'"/>
						</xsl:when>
						<xsl:when test="$dataType='.string'">
							<xsl:value-of select="'GetString'"/>
						</xsl:when>
						<xsl:when test="$dataType='.i2' or $dataType='.u2'">
							<xsl:value-of select="'GetInt16'"/>
						</xsl:when>
						<xsl:when test="$dataType='.i4' or $dataType='.u4'">
							<xsl:value-of select="'GetInt32'"/>
						</xsl:when>
						<xsl:when test="$dataType='.i8' or $dataType='.u8'">
							<xsl:value-of select="GetInt64"/>
						</xsl:when>
						<xsl:when test="$dataType='.decimal'">
							<xsl:value-of select="'GetDecimal'"/>
						</xsl:when>
					</xsl:choose>
				</xsl:attribute>
				<xsl:if test="$dataType='.i1' or $dataType='.u2' or $dataType='.u4' or $dataType='.u8'">
					<xsl:attribute name="castMethod">
						<xsl:choose>
							<xsl:when test="$dataType='.i1'">
								<xsl:value-of select="'ToSByte'"/>
							</xsl:when>
							<xsl:when test="$dataType='.u2'">
								<xsl:value-of select="'ToUInt16'"/>
							</xsl:when>
							<xsl:when test="$dataType='.u4'">
								<xsl:value-of select="'ToUInt32'"/>
							</xsl:when>
							<xsl:when test="$dataType='.u8'">
								<xsl:value-of select="'ToUInt64'"/>
							</xsl:when>
						</xsl:choose>
					</xsl:attribute>
				</xsl:if>
			</method>
		</xsl:variable>
		<xsl:variable name="method" select="exsl:node-set($methodFragment)/child::*"/>
		<plx:passParam>
			<xsl:choose>
				<xsl:when test="$method/@castMethod">
					<plx:callStatic name="{$method/@castMethod}" type="methodCall" dataTypeName="Convert">
						<plx:passParam>
							<plx:callInstance type="methodCall" name="{$method/@name}">
								<plx:callObject>
									<plx:nameRef name="{$reader}"/>
								</plx:callObject>
								<plx:passParam>
									<plx:nameRef name="{$ordinalName}"/>
								</plx:passParam>
							</plx:callInstance>
						</plx:passParam>
					</plx:callStatic>
				</xsl:when>
				<xsl:otherwise>
					<plx:callInstance type="methodCall" name="{$method/@name}">
						<plx:callObject>
							<plx:nameRef name="{$reader}"/>
						</plx:callObject>
						<plx:passParam>
							<plx:nameRef name="{$ordinalName}"/>
						</plx:passParam>
					</plx:callInstance>
				</xsl:otherwise>
			</xsl:choose>
		</plx:passParam>
	</xsl:template>

	<xsl:template name="GenerateCreateObjectReaderBlock">
		<xsl:param name="ObjectToCreateName"/>
		<xsl:param name="CommandName" />
		<plx:branch>
			<plx:condition>
				<plx:binaryOperator type="inequality">
					<plx:left>
						<plx:callInstance name="State" type="property">
							<plx:callObject>
								<plx:callInstance name="Connection" type="property">
									<plx:callObject>
										<plx:nameRef name="{$CommandName}" type="local"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:callObject>
						</plx:callInstance>
					</plx:left>
					<plx:right>
						<plx:callStatic name="Open" dataTypeName="ConnectionState" type="property" />
					</plx:right>
				</plx:binaryOperator>
			</plx:condition>
			<plx:callInstance name="Open" type="methodCall">
				<plx:callObject>
					<plx:callInstance name="Connection" type="property">
						<plx:callObject>
							<plx:nameRef name="{$CommandName}" type="local"/>
						</plx:callObject>
					</plx:callInstance>
				</plx:callObject>
			</plx:callInstance>
		</plx:branch>
		<plx:autoDispose localName="reader" dataTypeName="IDataReader">
			<plx:initialize>
				<plx:callInstance name="ExecuteReader" type="methodCall">
					<plx:callObject>
						<plx:nameRef name="{$CommandName}" type="local"/>
					</plx:callObject>
					<plx:passParam>
						<plx:binaryOperator type="bitwiseOr">
							<plx:left>
								<plx:callStatic type="field" dataTypeName="CommandBehavior" name="CloseConnection"/>
							</plx:left>
							<plx:right>
								<plx:callStatic type="field" dataTypeName="CommandBehavior" name="SingleRow"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:passParam>
				</plx:callInstance>
			</plx:initialize>
			<plx:branch>
				<plx:condition>
					<plx:binaryOperator type="booleanAnd">
						<plx:left>
							<plx:unaryOperator type="booleanNot">
								<plx:callInstance name="IsClosed" type="property">
									<plx:callObject>
										<plx:nameRef type="local" name="reader"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:unaryOperator>
						</plx:left>
						<plx:right>
							<plx:callInstance name="Read" type="methodCall">
								<plx:callObject>
									<plx:nameRef type="local" name="reader"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:right>
					</plx:binaryOperator>
				</plx:condition>
				<plx:return>
					<plx:callThis type="methodCall" name="Create{$ObjectToCreateName}">
						<plx:passParam>
							<plx:nameRef name="reader"/>
						</plx:passParam>
					</plx:callThis>
				</plx:return>
			</plx:branch>
			<plx:fallbackBranch>
				<plx:return>
					<plx:nullKeyword/>
				</plx:return>
			</plx:fallbackBranch>
		</plx:autoDispose>
	</xsl:template>

	<xsl:template name="GenerateIDataParameter">
		<xsl:param name="commandName"/>
		<xsl:param name="parameterName"/>
		<xsl:param name="parameterValue"/>
		<xsl:param name="isIdentity" select="'false'"/>
		<xsl:param name="parentName" />
		<xsl:if test="(($parentName or ($isIdentity != 'true')) and $SprocFree) or not($SprocFree)">
			<!-- Declare and initialize parameter -->
			<plx:local name="parameter{$parameterName}" dataTypeName="IDataParameter">
				<plx:initialize>
					<plx:callInstance name="CreateParameter" type="methodCall">
						<plx:callObject>
							<plx:nameRef name="{$commandName}"/>
						</plx:callObject>
					</plx:callInstance>
				</plx:initialize>
			</plx:local>
			<!-- Assign parameter name -->
			<plx:assign>
				<plx:left>
					<plx:callInstance name="ParameterName" type="property">
						<plx:callObject>
							<plx:nameRef name="parameter{$parameterName}"/>
						</plx:callObject>
					</plx:callInstance>
				</plx:left>
				<plx:right>
					<plx:string>
						<xsl:value-of select="$parameterName"/>
					</plx:string>
				</plx:right>
			</plx:assign>
			<xsl:choose>
				<xsl:when test="$isIdentity='true' and not(@parentName)">
					<!-- Set parameter direction to output -->
					<plx:assign>
						<plx:left>
							<plx:callInstance name="Direction" type="property">
								<plx:callObject>
									<plx:nameRef name="parameter{$parameterName}"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:left>
						<plx:right>
							<plx:callStatic name="Output" type="property" dataTypeName="ParameterDirection" />
						</plx:right>
					</plx:assign>
				</xsl:when>
				<xsl:otherwise>
					<!-- Assign parameter value-->
					<plx:assign>
						<plx:left>
							<plx:callInstance name="Value" type="property">
								<plx:callObject>
									<plx:nameRef name="parameter{$parameterName}"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:left>
						<plx:right>
							<xsl:copy-of select="$parameterValue"/>
						</plx:right>
					</plx:assign>
				</xsl:otherwise>
			</xsl:choose>
			<!-- Add parameter to the command object-->
			<plx:callInstance name="Add" type="methodCall">
				<plx:callObject>
					<plx:callInstance name="Parameters" type="property">
						<plx:callObject>
							<plx:nameRef name="{$commandName}"/>
						</plx:callObject>
					</plx:callInstance>
				</plx:callObject>
				<plx:passParam>
					<plx:nameRef name="{concat('parameter', $parameterName)}" type="local"/>
				</plx:passParam>
			</plx:callInstance>
		</xsl:if>
		
	</xsl:template>

	<xsl:template name="GenerateCompoundComparison">
		<xsl:param name="Identifiers"/>
		<xsl:param name="mainOperator"/>
		<xsl:param name="innerOperator"/>
		<xsl:param name="operationCount"/>
		<xsl:param name="currentPosition"/>
		<xsl:choose>
			<xsl:when test="$Identifiers or not($Identifiers[$currentPosition]/@name='')">
				<xsl:variable name="chosenCodeFragment">
					<xsl:variable name="currentIdentifier" select="$Identifiers[$currentPosition]"/>
					<plx:binaryOperator type="{$innerOperator}">
						<plx:left>
							<plx:nameRef type="local" name="oldValue{$currentIdentifier/@parentName}{$currentIdentifier/@name}"/>
						</plx:left>
						<plx:right>
							<plx:callInstance name="{$currentIdentifier/@name}" type="property">
								<plx:callObject>
									<plx:valueKeyword/>
								</plx:callObject>
							</plx:callInstance>
						</plx:right>
					</plx:binaryOperator>
				</xsl:variable>
				<xsl:variable name="chosenCode" select="exsl:node-set($chosenCodeFragment)/child::*"/>
				<xsl:choose>
					<xsl:when test="not($currentPosition=$operationCount)">
						<plx:binaryOperator type="{$mainOperator}">
							<plx:left>
								<xsl:copy-of select="$chosenCode"/>
							</plx:left>
							<plx:right>
								<xsl:call-template name="GenerateCompoundComparison">
									<xsl:with-param name="Identifiers" select="$Identifiers"/>
									<xsl:with-param name="mainOperator" select="$mainOperator"/>
									<xsl:with-param name="innerOperator" select="$innerOperator"/>
									<xsl:with-param name="operationCount" select="$operationCount"/>
									<xsl:with-param name="currentPosition" select="$currentPosition + 1"/>
								</xsl:call-template>
							</plx:right>
						</plx:binaryOperator>
					</xsl:when>
					<xsl:otherwise>
						<xsl:copy-of select="$chosenCode"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<xsl:otherwise>
				<xsl:message terminate="yes">GenerateCompoundComparison failed.  Missing/unknown Identifier.</xsl:message>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="GenerateCommandObjectCleanup">
		<xsl:param name="cmdName" select="'command'"/>
		<!-- Close connection if open -->
		<plx:branch>
			<plx:condition>
				<plx:binaryOperator type="equality">
					<plx:left>
						<plx:callInstance name="State" type="property">
							<plx:callObject>
								<plx:callInstance name="Connection" type="property">
									<plx:callObject>
										<plx:nameRef name="{$cmdName}" type="local"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:callObject>
						</plx:callInstance>
					</plx:left>
					<plx:right>
						<plx:callStatic name="Open" dataTypeName="ConnectionState" type="field"/>
					</plx:right>
				</plx:binaryOperator>
			</plx:condition>
			<plx:callInstance name="Close" type="methodCall">
				<plx:callObject>
					<plx:callInstance name="Connection" type="property">
						<plx:callObject>
							<plx:nameRef name="{$cmdName}" type="local"/>
						</plx:callObject>
					</plx:callInstance>
				</plx:callObject>
			</plx:callInstance>
		</plx:branch>
		<!-- Dispose command -->
		<plx:callInstance name="Dispose" type="methodCall">
			<plx:callObject>
				<plx:nameRef name="{$cmdName}" type="local"/>
			</plx:callObject>
		</plx:callInstance>
	</xsl:template>
	<xsl:template name="GenerateIDBCommandType">
		<xsl:param name="commandName" select="'command'"/>
		<plx:assign>
			<plx:left>
				<plx:callInstance type="property" name="CommandType">
					<plx:callObject>
						<plx:nameRef type="local" name="{$commandName}"/>
					</plx:callObject>
				</plx:callInstance>
			</plx:left>
			<plx:right>
				<xsl:choose>
					<xsl:when test="$SprocFree">
						<plx:callStatic type="field" name="Text" dataTypeName="CommandType" />
					</xsl:when>
					<xsl:otherwise>
						<plx:callStatic type="field" name="StoredProcedure" dataTypeName="CommandType" />
					</xsl:otherwise>
				</xsl:choose>
			</plx:right>
		</plx:assign>
	</xsl:template>
</xsl:stylesheet>
