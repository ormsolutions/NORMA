<?xml version="1.0" encoding="utf-8"?>
<!--
	Neumont Object-Role Modeling Architect for Visual Studio

	Copyright © Neumont University. All rights reserved.
	Copyright © ORM Solutions, LLC. All rights reserved.

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
	xmlns:oial="http://schemas.neumont.edu/ORM/Abstraction/2007-06/Core"
	xmlns:plx="http://schemas.neumont.edu/CodeGeneration/PLiX"
	xmlns:prop="urn:schemas-orm-net:PLiX:CLI:Properties"
	exclude-result-prefixes="oial"
	extension-element-prefixes="exsl">

	<xsl:import href="OIALtoPLiX_GlobalSupportFunctions.xslt"/>
	<xsl:param name="ORM"/>
	<xsl:output method="xml" encoding="utf-8" media-type="text/xml" indent="yes"/>

	<xsl:variable name="OialModel" select="$ORM/oial:model"/>
	<xsl:variable name="PropertiesRoot" select="prop:AllProperties"/>

	<xsl:template match="/">
		<plx:root>
			<plx:namespaceImport name="System"/>
			<plx:namespaceImport name="System.Collections.Generic"/>
			<plx:namespaceImport name="System.Collections.ObjectModel"/>
			<plx:namespaceImport name="System.ComponentModel"/>
			<plx:namespaceImport name="System.Xml"/>
			<xsl:variable name="modelName" select="string($PropertiesRoot/@modelName)"/>
			<xsl:variable name="allProperties" select="$PropertiesRoot/prop:Properties"/>
			<xsl:choose>
				<xsl:when test="$DefaultNamespace">
					<plx:namespace name="{$DefaultNamespace}">
						<xsl:apply-templates select="$OialModel" mode="OIALtoPLiX_InMemory_Implementation">
							<xsl:with-param name="ModelName" select="$modelName"/>
							<xsl:with-param name="AllProperties" select="$allProperties"/>
						</xsl:apply-templates>
					</plx:namespace>
				</xsl:when>
				<xsl:otherwise>
					<xsl:apply-templates select="$OialModel" mode="OIALtoPLiX_InMemory_Implementation">
						<xsl:with-param name="ModelName" select="$modelName"/>
						<xsl:with-param name="AllProperties" select="$allProperties"/>
					</xsl:apply-templates>
				</xsl:otherwise>
			</xsl:choose>
		</plx:root>
	</xsl:template>

	<xsl:template match="oial:model" mode="OIALtoPLiX_InMemory_Implementation">
		<xsl:param name="ModelName"/>
		<xsl:param name="AllProperties"/>
		<xsl:variable name="modelContextName" select="concat($ModelName, 'Context')"/>
		<xsl:variable name="model" select="."/>
		<xsl:variable name="conceptTypes" select="oial:conceptTypes/oial:conceptType"/>
		<xsl:variable name="assimilations" select="$conceptTypes/oial:children/oial:assimilatedConceptType"/>
		<xsl:variable name="allReverseChildProperties" select="$AllProperties/prop:Property[@reverseChildId]"/>
		<xsl:variable name="allUniquenessConstraints" select="$conceptTypes/oial:uniquenessConstraints/oial:uniquenessConstraint"/>

		<xsl:variable name="constraintEnforcementCollectionCallbacksDictionariesAssignmentsFragment">
			<xsl:for-each select="$AllProperties">
				<xsl:variable name="className" select="@conceptTypeName"/>
				<xsl:for-each select="prop:Property[@isCollection='true']">
					<xsl:variable name="propertyName" select="@name"/>
					<xsl:variable name="propertyTypeName" select="prop:DataType/plx:passTypeParam/@dataTypeName"/>
					<xsl:variable name="newCallbacksCodeFragment">
						
						<plx:callNew dataTypeName="ConstraintEnforcementCollectionCallbacks">
							<plx:passTypeParam dataTypeName="{$className}"/>
							<plx:passTypeParam dataTypeName="{$propertyTypeName}"/>
							<!-- COLLECTION_CHANGE_METHODS_CONDITIONS: Search for this comment to find other locations where these conditions also exist. -->
							<plx:passParam>
								<xsl:choose>
									<xsl:when test="@isCustomType='true'">
										<plx:callNew type="newDelegate" dataTypeName="PotentialCollectionModificationCallback">
											<plx:passTypeParam dataTypeName="{$className}"/>
											<plx:passTypeParam dataTypeName="{$propertyTypeName}"/>
											<plx:passParam>
												<plx:callThis type="methodReference" name="On{$className}{$propertyName}Adding"/>
											</plx:passParam>
										</plx:callNew>
									</xsl:when>
									<xsl:otherwise>
										<plx:nullKeyword/>
									</xsl:otherwise>
								</xsl:choose>
							</plx:passParam>
							<plx:passParam>
								<xsl:choose>
									<xsl:when test="@isUnique='true' or @isCustomType='true'">
										<plx:callNew type="newDelegate" dataTypeName="CommittedCollectionModificationCallback">
											<plx:passTypeParam dataTypeName="{$className}"/>
											<plx:passTypeParam dataTypeName="{$propertyTypeName}"/>
											<plx:passParam>
												<plx:callThis type="methodReference" name="On{$className}{$propertyName}Added"/>
											</plx:passParam>
										</plx:callNew>
									</xsl:when>
									<xsl:otherwise>
										<plx:nullKeyword/>
									</xsl:otherwise>
								</xsl:choose>
							</plx:passParam>
							<plx:passParam>
								<xsl:choose>
									<xsl:when test="false()">
										<plx:callNew type="newDelegate" dataTypeName="PotentialCollectionModificationCallback">
											<plx:passTypeParam dataTypeName="{$className}"/>
											<plx:passTypeParam dataTypeName="{$propertyTypeName}"/>
											<plx:passParam>
												<plx:callThis type="methodReference" name="On{$className}{$propertyName}Removing"/>
											</plx:passParam>
										</plx:callNew>
									</xsl:when>
									<xsl:otherwise>
										<plx:nullKeyword/>
									</xsl:otherwise>
								</xsl:choose>
							</plx:passParam>
							<plx:passParam>
								<xsl:choose>
									<xsl:when test="@isUnique='true' or @isCustomType='true'">
										<plx:callNew type="newDelegate" dataTypeName="CommittedCollectionModificationCallback">
											<plx:passTypeParam dataTypeName="{$className}"/>
											<plx:passTypeParam dataTypeName="{$propertyTypeName}"/>
											<plx:passParam>
												<plx:callThis type="methodReference" name="On{$className}{$propertyName}Removed"/>
											</plx:passParam>
										</plx:callNew>
									</xsl:when>
									<xsl:otherwise>
										<plx:nullKeyword/>
									</xsl:otherwise>
								</xsl:choose>
							</plx:passParam>
						</plx:callNew>
					</xsl:variable>
					<xsl:variable name="newCallbacksCode" select="exsl:node-set($newCallbacksCodeFragment)/child::*"/>
					<xsl:choose>
						<xsl:when test="count(../prop:Property[@isCollection='true' and prop:DataType/plx:passTypeParam/@dataTypeName=$propertyTypeName])>1">
							<plx:callInstance name="Add">
								<plx:callObject>
									<plx:nameRef name="constraintEnforcementCollectionCallbacksByTypeAndNameDictionary"/>
								</plx:callObject>
								<plx:passParam>
									<plx:callNew dataTypeName="ConstraintEnforcementCollectionTypeAndPropertyNameKey">
										<plx:passParam>
											<plx:callInstance type="property" name="TypeHandle">
												<plx:callObject>
													<plx:typeOf dataTypeName="ConstraintEnforcementCollectionWithPropertyName">
														<plx:passTypeParam dataTypeName="{$className}"/>
														<plx:passTypeParam dataTypeName="{$propertyTypeName}"/>
													</plx:typeOf>
												</plx:callObject>
											</plx:callInstance>
										</plx:passParam>
										<plx:passParam>
											<plx:string>
												<xsl:value-of select="$propertyName"/>
											</plx:string>
										</plx:passParam>
									</plx:callNew>
								</plx:passParam>
								<plx:passParam>
									<xsl:copy-of select="$newCallbacksCode"/>
								</plx:passParam>
							</plx:callInstance>
						</xsl:when>
						<xsl:otherwise>
							<plx:callInstance name="Add">
								<plx:callObject>
									<plx:nameRef name="constraintEnforcementCollectionCallbacksByTypeDictionary"/>
								</plx:callObject>
								<plx:passParam>
									<plx:callInstance type="property" name="TypeHandle">
										<plx:callObject>
											<plx:typeOf dataTypeName="ConstraintEnforcementCollection">
												<plx:passTypeParam dataTypeName="{$className}"/>
												<plx:passTypeParam dataTypeName="{$propertyTypeName}"/>
											</plx:typeOf>
										</plx:callObject>
									</plx:callInstance>
								</plx:passParam>
								<plx:passParam>
									<xsl:copy-of select="$newCallbacksCode"/>
								</plx:passParam>
							</plx:callInstance>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:for-each>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="constraintEnforcementCollectionCallbacksDictionariesAssignments" select="exsl:node-set($constraintEnforcementCollectionCallbacksDictionariesAssignmentsFragment)/child::*"/>
		<plx:namespace name="{$ModelName}">
			<plx:class visibility="public" modifier="sealed" name="{$modelContextName}">
				<plx:leadingInfo>
					<plx:pragma type="region" data="{$modelContextName}"/>
				</plx:leadingInfo>
				<plx:trailingInfo>
					<plx:pragma type="closeRegion" data="{$modelContextName}"/>
				</plx:trailingInfo>
				<xsl:copy-of select="$GeneratedCodeAttribute"/>
				<xsl:copy-of select="$StructLayoutAttribute"/>
				<plx:implementsInterface dataTypeName="I{$modelContextName}"/>

				<xsl:variable name="countForType" select="count($constraintEnforcementCollectionCallbacksDictionariesAssignments[plx:callObject/plx:nameRef/@name='constraintEnforcementCollectionCallbacksByTypeDictionary'])"/>
				<xsl:variable name="countForTypeAndName" select="count($constraintEnforcementCollectionCallbacksDictionariesAssignments[plx:callObject/plx:nameRef/@name='constraintEnforcementCollectionCallbacksByTypeAndNameDictionary'])"/>
				<xsl:variable name="generateForType" select="$countForType > 0"/>
				<xsl:variable name="generateForTypeAndName" select="$countForTypeAndName > 0"/>

				<plx:function visibility="public" name=".construct">
					<xsl:if test="$generateForType">
						<plx:local name="constraintEnforcementCollectionCallbacksByTypeDictionary" dataTypeName="Dictionary">
							<plx:passTypeParam dataTypeName="RuntimeTypeHandle"/>
							<plx:passTypeParam dataTypeName=".object"/>
							<plx:initialize>
								<plx:inlineStatement dataTypeName="Dictionary">
									<plx:passTypeParam dataTypeName="RuntimeTypeHandle"/>
									<plx:passTypeParam dataTypeName=".object"/>
									<plx:assign>
										<plx:left>
											<plx:callThis type="field" name="{$PrivateMemberPrefix}ContraintEnforcementCollectionCallbacksByTypeDictionary"/>
										</plx:left>
										<plx:right>
											<plx:callNew dataTypeName="Dictionary">
												<plx:passTypeParam dataTypeName="RuntimeTypeHandle"/>
												<plx:passTypeParam dataTypeName=".object"/>
												<plx:passParam>
													<plx:value type="i4" data="{$countForType}"/>
												</plx:passParam>
												<plx:passParam>
													<plx:callStatic type="field" name="Instance" dataTypeName="RuntimeTypeHandleEqualityComparer"/>
												</plx:passParam>
											</plx:callNew>
										</plx:right>
									</plx:assign>
								</plx:inlineStatement>
							</plx:initialize>
						</plx:local>
					</xsl:if>
					<xsl:if test="$generateForTypeAndName">
						<plx:local name="constraintEnforcementCollectionCallbacksByTypeAndNameDictionary" dataTypeName="Dictionary">
							<plx:passTypeParam dataTypeName="ConstraintEnforcementCollectionTypeAndPropertyNameKey"/>
							<plx:passTypeParam dataTypeName=".object"/>
							<plx:initialize>
								<plx:inlineStatement dataTypeName="Dictionary">
									<plx:passTypeParam dataTypeName="ConstraintEnforcementCollectionTypeAndPropertyNameKey"/>
									<plx:passTypeParam dataTypeName=".object"/>
									<plx:assign>
										<plx:left>
											<plx:callThis type="field" name="{$PrivateMemberPrefix}ContraintEnforcementCollectionCallbacksByTypeAndNameDictionary"/>
										</plx:left>
										<plx:right>
											<plx:callNew dataTypeName="Dictionary">
												<plx:passTypeParam dataTypeName="ConstraintEnforcementCollectionTypeAndPropertyNameKey"/>
												<plx:passTypeParam dataTypeName=".object"/>
												<plx:passParam>
													<plx:value type="i4" data="{$countForTypeAndName}"/>
												</plx:passParam>
											</plx:callNew>
										</plx:right>
									</plx:assign>
								</plx:inlineStatement>
							</plx:initialize>
						</plx:local>
					</xsl:if>
					<xsl:copy-of select="$constraintEnforcementCollectionCallbacksDictionariesAssignments"/>
					<xsl:for-each select="$conceptTypes">
						<xsl:variable name="className" select="string($AllProperties[@conceptTypeId=current()/@id]/@conceptTypeName)"/>
						<plx:assign>
							<plx:left>
								<plx:callThis type="field" name="{$PrivateMemberPrefix}{$className}ReadOnlyCollection"/>
							</plx:left>
							<plx:right>
								<plx:callNew dataTypeName="ReadOnlyCollection">
									<plx:passTypeParam dataTypeName="{$className}"/>
									<plx:passParam>
										<plx:inlineStatement dataTypeName="List">
											<plx:passTypeParam dataTypeName="{$className}"/>
											<plx:assign>
												<plx:left>
													<plx:callThis type="field" name="{$PrivateMemberPrefix}{$className}List"/>
												</plx:left>
												<plx:right>
													<plx:callNew dataTypeName="List">
														<plx:passTypeParam dataTypeName="{$className}"/>
													</plx:callNew>
												</plx:right>
											</plx:assign>
										</plx:inlineStatement>
									</plx:passParam>
								</plx:callNew>
							</plx:right>
						</plx:assign>
					</xsl:for-each>
				</plx:function>

				<plx:pragma type="region" data="Exception Helpers"/>
				<plx:function visibility="private" modifier="static" overload="true" name="GetDifferentContextsException">
					<plx:returns dataTypeName="ArgumentException"/>
					<plx:return>
						<plx:callStatic name="GetDifferentContextsException" dataTypeName="{$modelContextName}">
							<plx:passParam>
								<plx:valueKeyword stringize="true"/>
							</plx:passParam>
						</plx:callStatic>
					</plx:return>
				</plx:function>
				<plx:function visibility="private" modifier="static" overload="true" name="GetDifferentContextsException">
					<xsl:call-template name="GenerateSuppressMessageAttribute">
						<xsl:with-param name="category" select="'Microsoft.Usage'"/>
						<xsl:with-param name="checkId" select="'CA2208:InstantiateArgumentExceptionsCorrectly'"/>
					</xsl:call-template>
					<plx:param name="paramName" dataTypeName=".string"/>
					<plx:returns dataTypeName="ArgumentException"/>
					<plx:return>
						<plx:callNew dataTypeName="ArgumentException">
							<plx:passParam>
								<plx:string data="All objects in a relationship must be part of the same Context."/>
							</plx:passParam>
							<plx:passParam>
								<plx:nameRef type="parameter" name="paramName"/>
							</plx:passParam>
						</plx:callNew>
					</plx:return>
				</plx:function>
				<plx:function visibility="private" modifier="static" name="GetConstraintEnforcementFailedException">
					<plx:param name="paramName" dataTypeName=".string"/>
					<plx:returns dataTypeName="ArgumentException"/>
					<plx:return>
						<plx:callNew dataTypeName="ArgumentException">
							<plx:passParam>
								<plx:string data="Argument failed constraint enforcement."/>
							</plx:passParam>
							<plx:passParam>
								<plx:nameRef type="parameter" name="paramName"/>
							</plx:passParam>
						</plx:callNew>
					</plx:return>
				</plx:function>
				<plx:pragma type="closeRegion" data="Exception Helpers"/>

				<plx:pragma type="region" data="Lookup and External Constraint Enforcement"/>
				<xsl:call-template name="GenerateModelContextLookupAndExternalConstraintEnforcementMembers">
					<xsl:with-param name="Model" select="."/>
					<xsl:with-param name="ModelContextName" select="$modelContextName"/>
					<xsl:with-param name="AllProperties" select="$AllProperties"/>
					<xsl:with-param name="AllUniquenessConstraints" select="$allUniquenessConstraints"/>
				</xsl:call-template>
				<plx:pragma type="closeRegion" data="Lookup and External Constraint Enforcement"/>

				<xsl:call-template name="GenerateConstraintEnforcementCollection">
					<xsl:with-param name="ModelContextName" select="$modelContextName"/>
					<xsl:with-param name="GenerateForType" select="$generateForType"/>
					<xsl:with-param name="GenerateForTypeAndName" select="$generateForTypeAndName"/>
				</xsl:call-template>

				<xsl:for-each select="$conceptTypes">
					<xsl:variable name="currentProperties" select="$AllProperties[@conceptTypeId=current()/@id]"/>
					<xsl:apply-templates select="." mode="GenerateImplementationClass">
						<xsl:with-param name="Model" select="$model"/>
						<xsl:with-param name="ModelContextName" select="$modelContextName"/>
						<xsl:with-param name="AllProperties" select="$AllProperties"/>
						<xsl:with-param name="ConceptTypes" select="$conceptTypes"/>
						<xsl:with-param name="Assimilations" select="$assimilations"/>
						<xsl:with-param name="AllReverseChildProperties" select="$allReverseChildProperties"/>
						<xsl:with-param name="AllUniquenessConstraints" select="$allUniquenessConstraints"/>
						<xsl:with-param name="ClassName" select="string($currentProperties/@conceptTypeName)"/>
						<xsl:with-param name="Properties" select="$currentProperties/prop:Property"/>
					</xsl:apply-templates>
				</xsl:for-each>

			</plx:class>
		</plx:namespace>
	</xsl:template>

	<xsl:template match="prop:Property" mode="CopyDataTypeWithoutNullable">
		<!-- Copies the data type of the property. If that data type is Nullable, copies the underlying type instead. -->
		<xsl:choose>
			<xsl:when test="@isCustomType='false' and @canBeNull='true' and prop:DataType/@dataTypeName='Nullable'">
				<xsl:copy-of select="prop:DataType/plx:passTypeParam/@*"/>
				<xsl:copy-of select="prop:DataType/plx:passTypeParam/child::*"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:copy-of select="prop:DataType/@*"/>
				<xsl:copy-of select="prop:DataType/child::*"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="prop:IdentityField" mode="CopyDataTypeWithoutNullable">
		<!-- Treat identity fields as integer properties -->
		<xsl:attribute name="dataTypeName">
			<xsl:text>.i4</xsl:text>
		</xsl:attribute>
	</xsl:template>

	<xsl:template name="GenerateModelContextLookupAndExternalConstraintEnforcementMembers">
		<xsl:param name="Model"/>
		<xsl:param name="ModelContextName"/>
		<xsl:param name="AllProperties"/>
		<xsl:param name="AllUniquenessConstraints"/>

		<!-- TODO: This will break for uniquenessConstraint elements that reference more than one conceptType. -->
		<xsl:for-each select="$AllUniquenessConstraints">
			<xsl:variable name="uniqueConceptTypePropertiesContainer" select="$AllProperties[@conceptTypeId=current()/../../@id]"/>
			<xsl:variable name="uniqueConceptTypeName" select="string($uniqueConceptTypePropertiesContainer/@conceptTypeName)"/>
			<xsl:variable name="uniqueConceptTypeParamName" select="string($uniqueConceptTypePropertiesContainer/@conceptTypeParamName)"/>
			<xsl:variable name="uniqueConceptTypeProperties" select="$uniqueConceptTypePropertiesContainer/prop:*"/>
			<xsl:variable name="uniquenessChildren" select="oial:uniquenessChild"/>
			<xsl:variable name="singleChildUniqueness" select="count($uniquenessChildren)=1"/>
			<!-- TODO: The special handling of Nullable here (that is, $nullablePassTypeParams, $hasNullables, the associated xsl:choose blocks in On{@name}Changing and On{@name}Changed, and the GetExternalConstraintEnforcementNullableHackCode template) is a temporary hack that will be significantly improved on or replaced in the future. -->
			<xsl:variable name="nullablePassTypeParamsFragment">
				<xsl:for-each select="$uniquenessChildren">
					<xsl:variable name="targetProperty" select="$uniqueConceptTypeProperties[@childId=current()/@ref]"/>
					<plx:passTypeParam>
						<xsl:choose>
							<xsl:when test="$targetProperty[self::prop:IdentityField]">
								<xsl:attribute name="dataTypeName">
									<xsl:text>.i4</xsl:text>
								</xsl:attribute>
							</xsl:when>
							<xsl:otherwise>
								<xsl:copy-of select="$targetProperty/prop:DataType/@*"/>
								<xsl:copy-of select="$targetProperty/prop:DataType/child::*"/>
							</xsl:otherwise>
						</xsl:choose>
					</plx:passTypeParam>
				</xsl:for-each>
			</xsl:variable>
			<xsl:variable name="nullablePassTypeParams" select="exsl:node-set($nullablePassTypeParamsFragment)/child::*"/>
			<xsl:variable name="nullableTypedParamFragment">
				<!-- Leave off any parameter name, this will be supplied later. -->
				<plx:param>
					<xsl:choose>
						<xsl:when test="$singleChildUniqueness">
							<xsl:copy-of select="$nullablePassTypeParams/@*"/>
							<xsl:copy-of select="$nullablePassTypeParams/plx:*"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:attribute name="dataTypeName">
								<xsl:text>Tuple</xsl:text>
							</xsl:attribute>
							<xsl:copy-of select="$nullablePassTypeParams"/>
						</xsl:otherwise>
					</xsl:choose>
				</plx:param>
			</xsl:variable>
			<xsl:variable name="nullableTypedParam" select="exsl:node-set($nullableTypedParamFragment)/child::*"/>
			<xsl:variable name="hasNullables" select="boolean($nullablePassTypeParams[@dataTypeName='Nullable'])"/>
			<xsl:variable name="parametersFragment">
				<xsl:for-each select="$uniquenessChildren">
					<xsl:variable name="targetProperty" select="$uniqueConceptTypeProperties[@childId=current()/@ref]"/>
					<plx:param name="{$targetProperty/@paramName}">
						<xsl:apply-templates select="$targetProperty" mode="CopyDataTypeWithoutNullable"/>
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
			<xsl:variable name="dictionaryKeyTypeFragment">
				<xsl:choose>
					<xsl:when test="$singleChildUniqueness">
						<xsl:copy-of select="$passTypeParams"/>
					</xsl:when>
					<xsl:otherwise>
						<plx:passTypeParam dataTypeName="Tuple">
							<xsl:copy-of select="$passTypeParams"/>
						</plx:passTypeParam>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="dictionaryKeyType" select="exsl:node-set($dictionaryKeyTypeFragment)/child::*"/>
			<xsl:variable name="dictionaryKeyFragment">
				<xsl:choose>
					<xsl:when test="$singleChildUniqueness">
						<plx:nameRef type="parameter" name="{$parameters/@name}"/>
					</xsl:when>
					<xsl:otherwise>
						<plx:callStatic name="CreateTuple" dataTypeName="Tuple">
							<xsl:for-each select="$parameters">
								<plx:passMemberTypeParam>
									<xsl:copy-of select="self::plx:param/@*[not(local-name()='name')]"/>
									<xsl:copy-of select="self::plx:param/child::*"/>
								</plx:passMemberTypeParam>
							</xsl:for-each>
							<xsl:for-each select="$parameters">
								<plx:passParam>
									<plx:nameRef type="parameter" name="{@name}"/>
								</plx:passParam>
							</xsl:for-each>
						</plx:callStatic>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="dictionaryKey" select="exsl:node-set($dictionaryKeyFragment)/child::*"/>

			<!-- We ignore the uniqueness name here, which is rarely informative, in favor of a list of the property
			names used by the uniqueness constraint. -->
			<xsl:variable name="uniquenessSignatureFragment">
				<xsl:for-each select="$uniquenessChildren">
					<xsl:if test="position()!=1">
						<xsl:text>And</xsl:text>
					</xsl:if>
					<xsl:value-of select="$uniqueConceptTypeProperties[@childId=current()/@ref]/@name"/>
				</xsl:for-each>
			</xsl:variable>
			<xsl:variable name="uniquenessSignature" select="string($uniquenessSignatureFragment)"/>
			<plx:field visibility="private" readOnly="true" name="{$PrivateMemberPrefix}{$uniqueConceptTypeName}{$uniquenessSignature}Dictionary" dataTypeName="Dictionary">
				<xsl:copy-of select="$dictionaryKeyType"/>
				<plx:passTypeParam dataTypeName="{$uniqueConceptTypeName}"/>
				<plx:initialize>
					<plx:callNew dataTypeName="Dictionary">
						<xsl:copy-of select="$dictionaryKeyType"/>
						<plx:passTypeParam dataTypeName="{$uniqueConceptTypeName}"/>
					</plx:callNew>
				</plx:initialize>
			</plx:field>

			<plx:function visibility="{$ModelContextInterfaceImplementationVisibility}" name="Get{$uniqueConceptTypeName}By{$uniquenessSignature}">
				<plx:interfaceMember memberName="Get{$uniqueConceptTypeName}By{$uniquenessSignature}" dataTypeName="I{$ModelContextName}"/>
				<xsl:copy-of select="$parameters"/>
				<plx:returns dataTypeName="{$uniqueConceptTypeName}"/>
				<plx:return>
					<plx:callInstance type="indexerCall" name=".implied">
						<plx:callObject>
							<plx:callThis type="field" name="{$PrivateMemberPrefix}{$uniqueConceptTypeName}{$uniquenessSignature}Dictionary"/>
						</plx:callObject>
						<plx:passParam>
							<xsl:copy-of select="$dictionaryKey"/>
						</plx:passParam>
					</plx:callInstance>
				</plx:return>
			</plx:function>
			<plx:function visibility="{$ModelContextInterfaceImplementationVisibility}" name="TryGet{$uniqueConceptTypeName}By{$uniquenessSignature}">
				<plx:interfaceMember memberName="TryGet{$uniqueConceptTypeName}By{$uniquenessSignature}" dataTypeName="I{$ModelContextName}"/>
				<xsl:copy-of select="$parameters"/>
				<plx:param type="out" name="{$uniqueConceptTypeParamName}" dataTypeName="{$uniqueConceptTypeName}"/>
				<plx:returns dataTypeName=".boolean"/>
				<plx:return>
					<plx:callInstance name="TryGetValue">
						<plx:callObject>
							<plx:callThis type="field" name="{$PrivateMemberPrefix}{$uniqueConceptTypeName}{$uniquenessSignature}Dictionary"/>
						</plx:callObject>
						<plx:passParam>
							<xsl:copy-of select="$dictionaryKey"/>
						</plx:passParam>
						<plx:passParam type="out">
							<plx:nameRef type="parameter" name="{$uniqueConceptTypeParamName}"/>
						</plx:passParam>
					</plx:callInstance>
				</plx:return>
			</plx:function>
			<xsl:if test="not($singleChildUniqueness)">
				<plx:function visibility="private" name="On{$uniquenessSignature}Changing">
					<plx:param name="instance" dataTypeName="{$uniqueConceptTypeName}"/>
					<plx:param name="newValue">
						<xsl:copy-of select="$nullableTypedParam/@*"/>
						<xsl:copy-of select="$nullableTypedParam/child::*"/>
					</plx:param>
					<plx:returns dataTypeName=".boolean"/>
					<plx:branch>
						<plx:condition>
							<plx:binaryOperator type="identityInequality">
								<plx:left>
									<plx:cast type="exceptionCast" dataTypeName=".object">
										<plx:nameRef type="parameter" name="newValue"/>
									</plx:cast>
								</plx:left>
								<plx:right>
									<plx:nullKeyword/>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:local name="currentInstance" dataTypeName="{$uniqueConceptTypeName}"/>
						<plx:branch>
							<plx:condition>
								<plx:callInstance name="TryGetValue">
									<plx:callObject>
										<plx:callThis type="field" name="{$PrivateMemberPrefix}{$uniqueConceptTypeName}{$uniquenessSignature}Dictionary"/>
									</plx:callObject>
									<plx:passParam>
										<xsl:choose>
											<xsl:when test="$hasNullables">
												<xsl:call-template name="GetExternalConstraintEnforcementNullableHackCode">
													<xsl:with-param name="PassTypeParams" select="$passTypeParams"/>
													<xsl:with-param name="NullablePassTypeParams" select="$nullablePassTypeParams"/>
													<xsl:with-param name="SourceParameterName" select="'newValue'"/>
												</xsl:call-template>
											</xsl:when>
											<xsl:otherwise>
												<plx:nameRef type="parameter" name="newValue"/>
											</xsl:otherwise>
										</xsl:choose>
									</plx:passParam>
									<plx:passParam type="out">
										<plx:nameRef name="currentInstance"/>
									</plx:passParam>
								</plx:callInstance>
							</plx:condition>
							<plx:return>
								<plx:binaryOperator type="identityEquality">
									<plx:left>
										<plx:cast type="exceptionCast" dataTypeName=".object">
											<plx:nameRef name="currentInstance"/>
										</plx:cast>
									</plx:left>
									<plx:right>
										<plx:cast type="exceptionCast" dataTypeName=".object">
											<plx:nameRef type="parameter" name="instance"/>
										</plx:cast>
									</plx:right>
								</plx:binaryOperator>
							</plx:return>
						</plx:branch>
					</plx:branch>
					<plx:return>
						<plx:trueKeyword/>
					</plx:return>
				</plx:function>

				<plx:function visibility="private" name="On{$uniquenessSignature}Changed">
					<plx:param name="instance" dataTypeName="{$uniqueConceptTypeName}"/>
					<plx:param name="oldValue">
						<xsl:copy-of select="$nullableTypedParam/@*"/>
						<xsl:copy-of select="$nullableTypedParam/child::*"/>
					</plx:param>
					<plx:param name="newValue">
						<xsl:copy-of select="$nullableTypedParam/@*"/>
						<xsl:copy-of select="$nullableTypedParam/child::*"/>
					</plx:param>
					<plx:branch>
						<plx:condition>
							<plx:binaryOperator type="identityInequality">
								<plx:left>
									<plx:cast type="exceptionCast" dataTypeName=".object">
										<plx:nameRef type="parameter" name="oldValue"/>
									</plx:cast>
								</plx:left>
								<plx:right>
									<plx:nullKeyword/>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:callInstance name="Remove">
							<plx:callObject>
								<plx:callThis name="{$PrivateMemberPrefix}{$uniqueConceptTypeName}{$uniquenessSignature}Dictionary" type="field"/>
							</plx:callObject>
							<plx:passParam>
								<xsl:choose>
									<xsl:when test="$hasNullables">
										<xsl:call-template name="GetExternalConstraintEnforcementNullableHackCode">
											<xsl:with-param name="PassTypeParams" select="$passTypeParams"/>
											<xsl:with-param name="NullablePassTypeParams" select="$nullablePassTypeParams"/>
											<xsl:with-param name="SourceParameterName" select="'oldValue'"/>
										</xsl:call-template>
									</xsl:when>
									<xsl:otherwise>
										<plx:nameRef type="parameter" name="oldValue"/>
									</xsl:otherwise>
								</xsl:choose>
							</plx:passParam>
						</plx:callInstance>
					</plx:branch>
					<plx:branch>
						<plx:condition>
							<plx:binaryOperator type="identityInequality">
								<plx:left>
									<plx:cast type="exceptionCast" dataTypeName=".object">
										<plx:nameRef type="parameter" name="newValue"/>
									</plx:cast>
								</plx:left>
								<plx:right>
									<plx:nullKeyword/>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:callInstance name="Add">
							<plx:callObject>
								<plx:callThis name="{$PrivateMemberPrefix}{$uniqueConceptTypeName}{$uniquenessSignature}Dictionary" type="field"/>
							</plx:callObject>
							<plx:passParam>
								<xsl:choose>
									<xsl:when test="$hasNullables">
										<xsl:call-template name="GetExternalConstraintEnforcementNullableHackCode">
											<xsl:with-param name="PassTypeParams" select="$passTypeParams"/>
											<xsl:with-param name="NullablePassTypeParams" select="$nullablePassTypeParams"/>
											<xsl:with-param name="SourceParameterName" select="'newValue'"/>
										</xsl:call-template>
									</xsl:when>
									<xsl:otherwise>
										<plx:nameRef type="parameter" name="newValue"/>
									</xsl:otherwise>
								</xsl:choose>
							</plx:passParam>
							<plx:passParam>
								<plx:nameRef type="parameter" name="instance"/>
							</plx:passParam>
						</plx:callInstance>
					</plx:branch>
				</plx:function>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>

	<xsl:template name="GetExternalConstraintEnforcementNullableHackCode">
		<xsl:param name="PassTypeParams"/>
		<xsl:param name="NullablePassTypeParams"/>
		<xsl:param name="SourceParameterName"/>
		<xsl:choose>
			<xsl:when test="count($PassTypeParams)=1">
				<xsl:for-each select="$NullablePassTypeParams">
					<xsl:choose>
						<xsl:when test="@dataTypeName='Nullable'">
							<plx:callInstance name="GetValueOrDefault">
								<plx:callObject>
									<plx:nameRef type="parameter" name="{$SourceParameterName}"/>
								</plx:callObject>
							</plx:callInstance>
						</xsl:when>
						<xsl:otherwise>
							<plx:nameRef type="parameter" name="{$SourceParameterName}"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:for-each>
			</xsl:when>
			<xsl:otherwise>
				<plx:callNew dataTypeName="Tuple">
					<xsl:copy-of select="$PassTypeParams"/>
					<xsl:for-each select="$NullablePassTypeParams">
						<plx:passParam>
							<xsl:choose>
								<xsl:when test="@dataTypeName='Nullable'">
									<!-- We can use GetValueOrDefault since we already know that this has a value, else the whole Tuple would have been null. -->
									<plx:callInstance name="GetValueOrDefault">
										<plx:callObject>
											<plx:callInstance type="property" name="Item{position()}">
												<plx:callObject>
													<plx:nameRef type="parameter" name="{$SourceParameterName}"/>
												</plx:callObject>
											</plx:callInstance>
										</plx:callObject>
									</plx:callInstance>
								</xsl:when>
								<xsl:otherwise>
									<plx:callInstance type="property" name="Item{position()}">
										<plx:callObject>
											<plx:nameRef type="parameter" name="{$SourceParameterName}"/>
										</plx:callObject>
									</plx:callInstance>
								</xsl:otherwise>
							</xsl:choose>
						</plx:passParam>
					</xsl:for-each>
				</plx:callNew>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="GenerateConstraintEnforcementCollection">
		<xsl:param name="ModelContextName"/>
		<xsl:param name="GenerateForType"/>
		<xsl:param name="GenerateForTypeAndName"/>
		<xsl:if test="$GenerateForType or $GenerateForTypeAndName">
			<plx:pragma type="region" data="ConstraintEnforcementCollection"/>
			<xsl:variable name="actionsFragment">
				<Action properName="Adding" paramName="adding" delegateType="PotentialCollectionModificationCallback" returnsBool="true"/>
				<Action properName="Added" paramName="added" delegateType="CommittedCollectionModificationCallback" returnsBool="false"/>
				<Action properName="Removing" paramName="removing" delegateType="PotentialCollectionModificationCallback" returnsBool="true"/>
				<Action properName="Removed" paramName="removed" delegateType="CommittedCollectionModificationCallback" returnsBool="false"/>
			</xsl:variable>
			<xsl:variable name="actions" select="exsl:node-set($actionsFragment)/child::*"/>
			<xsl:variable name="typeParamsFragment">
				<plx:typeParam requireReferenceType="true" name="TClass">
					<plx:typeConstraint dataTypeName="IHas{$ModelContextName}"/>
				</plx:typeParam>
				<plx:typeParam name="TProperty"/>
			</xsl:variable>
			<xsl:variable name="typeParams" select="exsl:node-set($typeParamsFragment)/child::*"/>
			<plx:delegate visibility="private" name="PotentialCollectionModificationCallback">
				<xsl:copy-of select="$typeParams"/>
				<plx:param name="instance" dataTypeName="TClass"/>
				<plx:param name="item" dataTypeName="TProperty"/>
				<plx:returns dataTypeName=".boolean"/>
			</plx:delegate>
			<plx:delegate visibility="private" name="CommittedCollectionModificationCallback">
				<xsl:copy-of select="$typeParams"/>
				<plx:param name="instance" dataTypeName="TClass"/>
				<plx:param name="item" dataTypeName="TProperty"/>
			</plx:delegate>
			<plx:class visibility="private" modifier="sealed" name="ConstraintEnforcementCollectionCallbacks">
				<xsl:copy-of select="$StructLayoutAttribute"/>
				<xsl:copy-of select="$typeParams"/>
				<plx:function visibility="public" name=".construct">
					<xsl:for-each select="$actions">
						<plx:param name="{@paramName}" dataTypeName="{@delegateType}">
							<plx:passTypeParam dataTypeName="TClass"/>
							<plx:passTypeParam dataTypeName="TProperty"/>
						</plx:param>
					</xsl:for-each>
					<xsl:for-each select="$actions">
						<plx:assign>
							<plx:left>
								<plx:callThis type="field" name="{@properName}"/>
							</plx:left>
							<plx:right>
								<plx:nameRef type="parameter" name="{@paramName}"/>
							</plx:right>
						</plx:assign>
					</xsl:for-each>
				</plx:function>
				<xsl:for-each select="$actions">
					<plx:field visibility="public" readOnly="true" name="{@properName}" dataTypeName="{@delegateType}">
						<plx:passTypeParam dataTypeName="TClass"/>
						<plx:passTypeParam dataTypeName="TProperty"/>
					</plx:field>
				</xsl:for-each>
			</plx:class>
			<xsl:if test="$GenerateForType">
				<plx:function visibility="private" overload="{$GenerateForTypeAndName}" name="GetConstraintEnforcementCollectionCallbacks">
					<xsl:copy-of select="$typeParams"/>
					<plx:returns dataTypeName="ConstraintEnforcementCollectionCallbacks">
						<plx:passTypeParam dataTypeName="TClass"/>
						<plx:passTypeParam dataTypeName="TProperty"/>
					</plx:returns>
					<plx:return>
						<plx:cast type="exceptionCast" dataTypeName="ConstraintEnforcementCollectionCallbacks">
							<plx:passTypeParam dataTypeName="TClass"/>
							<plx:passTypeParam dataTypeName="TProperty"/>
							<plx:callInstance type="indexerCall" name=".implied">
								<plx:callObject>
									<plx:callThis type="field" name="{$PrivateMemberPrefix}ContraintEnforcementCollectionCallbacksByTypeDictionary"/>
								</plx:callObject>
								<plx:passParam>
									<plx:callInstance type="property" name="TypeHandle">
										<plx:callObject>
											<plx:typeOf dataTypeName="ConstraintEnforcementCollection">
												<plx:passTypeParam dataTypeName="TClass"/>
												<plx:passTypeParam dataTypeName="TProperty"/>
											</plx:typeOf>
										</plx:callObject>
									</plx:callInstance>
								</plx:passParam>
							</plx:callInstance>
						</plx:cast>
					</plx:return>
				</plx:function>
				<plx:class visibility="private" modifier="sealed" name="RuntimeTypeHandleEqualityComparer">
					<xsl:copy-of select="$StructLayoutAttribute"/>
					<plx:implementsInterface dataTypeName="IEqualityComparer">
						<plx:passTypeParam dataTypeName="RuntimeTypeHandle"/>
					</plx:implementsInterface>
					<plx:field visibility="public" static="true" readOnly="true" name="Instance" dataTypeName="RuntimeTypeHandleEqualityComparer">
						<plx:initialize>
							<plx:callNew dataTypeName="RuntimeTypeHandleEqualityComparer"/>
						</plx:initialize>
					</plx:field>
					<plx:function visibility="private" name=".construct"/>
					<plx:function visibility="public" overload="true" name="Equals">
						<plx:interfaceMember memberName="Equals" dataTypeName="IEqualityComparer">
							<plx:passTypeParam dataTypeName="RuntimeTypeHandle"/>
						</plx:interfaceMember>
						<plx:param name="x" dataTypeName="RuntimeTypeHandle"/>
						<plx:param name="y" dataTypeName="RuntimeTypeHandle"/>
						<plx:returns dataTypeName=".boolean"/>
						<plx:return>
							<plx:callInstance name="Equals">
								<plx:callObject>
									<plx:nameRef type="parameter" name="x"/>
								</plx:callObject>
								<plx:passParam>
									<plx:nameRef type="parameter" name="y"/>
								</plx:passParam>
							</plx:callInstance>
						</plx:return>
					</plx:function>
					<plx:function visibility="public" overload="true" name="GetHashCode">
						<plx:interfaceMember memberName="GetHashCode" dataTypeName="IEqualityComparer">
							<plx:passTypeParam dataTypeName="RuntimeTypeHandle"/>
						</plx:interfaceMember>
						<plx:param name="obj" dataTypeName="RuntimeTypeHandle"/>
						<plx:returns dataTypeName=".i4"/>
						<plx:return>
							<plx:callInstance name="GetHashCode">
								<plx:callObject>
									<plx:nameRef type="parameter" name="obj"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:return>
					</plx:function>
				</plx:class>
				<plx:field visibility="private" readOnly="true" name="{$PrivateMemberPrefix}ContraintEnforcementCollectionCallbacksByTypeDictionary" dataTypeName="Dictionary">
					<plx:passTypeParam dataTypeName="RuntimeTypeHandle"/>
					<plx:passTypeParam dataTypeName=".object"/>
				</plx:field>
				<xsl:call-template name="GetConstraintEnforcementCollectionClassCode">
					<xsl:with-param name="TypeParams" select="$typeParams"/>
				</xsl:call-template>
			</xsl:if>
			<xsl:if test="$GenerateForTypeAndName">
				<plx:function visibility="private" overload="{$GenerateForType}" name="GetConstraintEnforcementCollectionCallbacks">
					<xsl:copy-of select="$typeParams"/>
					<plx:param name="propertyName" dataTypeName=".string"/>
					<plx:returns dataTypeName="ConstraintEnforcementCollectionCallbacks">
						<plx:passTypeParam dataTypeName="TClass"/>
						<plx:passTypeParam dataTypeName="TProperty"/>
					</plx:returns>
					<plx:return>
						<plx:cast type="exceptionCast" dataTypeName="ConstraintEnforcementCollectionCallbacks">
							<plx:passTypeParam dataTypeName="TClass"/>
							<plx:passTypeParam dataTypeName="TProperty"/>
							<plx:callInstance type="indexerCall" name=".implied">
								<plx:callObject>
									<plx:callThis type="field" name="{$PrivateMemberPrefix}ContraintEnforcementCollectionCallbacksByTypeAndNameDictionary"/>
								</plx:callObject>
								<plx:passParam>
									<plx:callNew dataTypeName="ConstraintEnforcementCollectionTypeAndPropertyNameKey">
										<plx:passParam>
											<plx:callInstance type="property" name="TypeHandle">
												<plx:callObject>
													<plx:typeOf dataTypeName="ConstraintEnforcementCollectionWithPropertyName">
														<plx:passTypeParam dataTypeName="TClass"/>
														<plx:passTypeParam dataTypeName="TProperty"/>
													</plx:typeOf>
												</plx:callObject>
											</plx:callInstance>
										</plx:passParam>
										<plx:passParam>
											<plx:nameRef type="parameter" name="propertyName"/>
										</plx:passParam>
									</plx:callNew>
								</plx:passParam>
							</plx:callInstance>
						</plx:cast>
					</plx:return>
				</plx:function>
				<plx:structure visibility="private" name="ConstraintEnforcementCollectionTypeAndPropertyNameKey">
					<xsl:copy-of select="$StructLayoutAttribute"/>
					<plx:implementsInterface dataTypeName="IEquatable">
						<plx:passTypeParam dataTypeName="ConstraintEnforcementCollectionTypeAndPropertyNameKey"/>
					</plx:implementsInterface>
					<plx:function visibility="public" name=".construct">
						<plx:param name="typeHandle" dataTypeName="RuntimeTypeHandle"/>
						<plx:param name="name" dataTypeName=".string"/>
						<plx:assign>
							<plx:left>
								<plx:callThis type="field" name="TypeHandle"/>
							</plx:left>
							<plx:right>
								<plx:nameRef type="parameter" name="typeHandle"/>
							</plx:right>
						</plx:assign>
						<plx:assign>
							<plx:left>
								<plx:callThis type="field" name="Name"/>
							</plx:left>
							<plx:right>
								<plx:nameRef type="parameter" name="name"/>
							</plx:right>
						</plx:assign>
					</plx:function>
					<plx:field visibility="public" readOnly="true" name="TypeHandle" dataTypeName="RuntimeTypeHandle"/>
					<plx:field visibility="public" readOnly="true" name="Name" dataTypeName=".string"/>
					<plx:function visibility="public" modifier="override" name="GetHashCode">
						<plx:returns dataTypeName=".i4"/>
						<plx:return>
							<plx:binaryOperator type="bitwiseExclusiveOr">
								<plx:left>
									<plx:callInstance name="GetHashCode">
										<plx:callObject>
											<plx:callThis type="field" name="TypeHandle"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:left>
								<plx:right>
									<plx:callInstance name="GetHashCode">
										<plx:callObject>
											<plx:callThis type="field" name="Name"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:right>
							</plx:binaryOperator>
						</plx:return>
					</plx:function>
					<plx:function visibility="public" modifier="override" overload="true" name="Equals">
						<!-- Suppress the 'OverloadOperatorEqualsOnOverridingValueTypeEquals' FxCop warning. -->
						<!-- This struct is only accessible from generated code, and we don't use the equality or inequality operators on it. -->
						<xsl:call-template name="GenerateSuppressMessageAttribute">
							<xsl:with-param name="category" select="'Microsoft.Usage'"/>
							<xsl:with-param name="checkId" select="'CA2231:OverloadOperatorEqualsOnOverridingValueTypeEquals'"/>
						</xsl:call-template>
						<plx:param name="obj" dataTypeName=".object"/>
						<plx:returns dataTypeName=".boolean"/>
						<plx:return>
							<plx:binaryOperator type="booleanAnd">
								<plx:left>
									<plx:binaryOperator type="typeEquality">
										<plx:left>
											<plx:nameRef type="parameter" name="obj"/>
										</plx:left>
										<plx:right>
											<plx:directTypeReference dataTypeName="ConstraintEnforcementCollectionTypeAndPropertyNameKey"/>
										</plx:right>
									</plx:binaryOperator>
								</plx:left>
								<plx:right>
									<plx:callThis name="Equals">
										<plx:passParam>
											<plx:cast type="exceptionCast" dataTypeName="ConstraintEnforcementCollectionTypeAndPropertyNameKey">
												<plx:nameRef type="parameter" name="obj"/>
											</plx:cast>
										</plx:passParam>
									</plx:callThis>
								</plx:right>
							</plx:binaryOperator>
						</plx:return>
					</plx:function>
					<plx:function visibility="public" overload="true" name="Equals">
						<!-- Suppress the 'OverloadOperatorEqualsOnOverridingValueTypeEquals' FxCop warning. -->
						<!-- This struct is only accessible from generated code, and we don't use the equality or inequality operators on it. -->
						<xsl:call-template name="GenerateSuppressMessageAttribute">
							<xsl:with-param name="category" select="'Microsoft.Usage'"/>
							<xsl:with-param name="checkId" select="'CA2231:OverloadOperatorEqualsOnOverridingValueTypeEquals'"/>
						</xsl:call-template>
						<plx:interfaceMember memberName="Equals" dataTypeName="IEquatable">
							<plx:passTypeParam dataTypeName="ConstraintEnforcementCollectionTypeAndPropertyNameKey"/>
						</plx:interfaceMember>
						<plx:param name="other" dataTypeName="ConstraintEnforcementCollectionTypeAndPropertyNameKey"/>
						<plx:returns dataTypeName=".boolean"/>
						<plx:return>
							<plx:binaryOperator type="booleanAnd">
								<plx:left>
									<plx:callInstance name="Equals">
										<plx:callObject>
											<plx:callThis type="field" name="TypeHandle"/>
										</plx:callObject>
										<plx:passParam>
											<plx:callInstance type="field" name="TypeHandle">
												<plx:callObject>
													<plx:nameRef type="parameter" name="other"/>
												</plx:callObject>
											</plx:callInstance>
										</plx:passParam>
									</plx:callInstance>
								</plx:left>
								<plx:right>
									<plx:callInstance name="Equals">
										<plx:callObject>
											<plx:callThis type="field" name="Name"/>
										</plx:callObject>
										<plx:passParam>
											<plx:callInstance type="field" name="Name">
												<plx:callObject>
													<plx:nameRef type="parameter" name="other"/>
												</plx:callObject>
											</plx:callInstance>
										</plx:passParam>
									</plx:callInstance>
								</plx:right>
							</plx:binaryOperator>
						</plx:return>
					</plx:function>
				</plx:structure>
				<plx:field visibility="private" readOnly="true" name="{$PrivateMemberPrefix}ContraintEnforcementCollectionCallbacksByTypeAndNameDictionary" dataTypeName="Dictionary">
					<plx:passTypeParam dataTypeName="ConstraintEnforcementCollectionTypeAndPropertyNameKey"/>
					<plx:passTypeParam dataTypeName=".object"/>
				</plx:field>
				<xsl:call-template name="GetConstraintEnforcementCollectionClassCode">
					<xsl:with-param name="TypeParams" select="$typeParams"/>
					<xsl:with-param name="WithPropertyName" select="'WithPropertyName'"/>
				</xsl:call-template>
			</xsl:if>
			<plx:pragma type="closeRegion" data="ConstraintEnforcementCollection"/>
		</xsl:if>
	</xsl:template>

	<xsl:template name="GetConstraintEnforcementCollectionClassCode">
		<xsl:param name="TypeParams"/>
		<xsl:param name="WithPropertyName"/>
		<plx:class visibility="private" modifier="sealed" name="{concat('ConstraintEnforcementCollection',$WithPropertyName)}">
			<xsl:copy-of select="$StructLayoutAttribute"/>
			<xsl:copy-of select="$TypeParams"/>
			<plx:implementsInterface dataTypeName="ICollection">
				<plx:passTypeParam dataTypeName="TProperty"/>
			</plx:implementsInterface>

			<plx:field visibility="private" readOnly="true" name="{$PrivateMemberPrefix}Instance" dataTypeName="TClass"/>
			<xsl:if test="$WithPropertyName">
				<plx:field visibility="private" readOnly="true" name="{$PrivateMemberPrefix}PropertyName" dataTypeName=".string"/>
			</xsl:if>
			<plx:field visibility="private" readOnly="true" name="{$PrivateMemberPrefix}List" dataTypeName="List">
				<plx:passTypeParam dataTypeName="TProperty"/>
				<plx:initialize>
					<plx:callNew dataTypeName="List">
						<plx:passTypeParam dataTypeName="TProperty"/>
					</plx:callNew>
				</plx:initialize>
			</plx:field>

			<plx:function visibility="public" name=".construct">
				<plx:param name="instance" dataTypeName="TClass"/>
				<xsl:if test="$WithPropertyName">
					<plx:param name="propertyName" dataTypeName=".string"/>
				</xsl:if>
				<plx:assign>
					<plx:left>
						<plx:callThis type="field" name="{$PrivateMemberPrefix}Instance"/>
					</plx:left>
					<plx:right>
						<plx:nameRef type="parameter" name="instance"/>
					</plx:right>
				</plx:assign>
				<xsl:if test="$WithPropertyName">
					<plx:assign>
						<plx:left>
							<plx:callThis type="field" name="{$PrivateMemberPrefix}PropertyName"/>
						</plx:left>
						<plx:right>
							<plx:nameRef type="parameter" name="propertyName"/>
						</plx:right>
					</plx:assign>
				</xsl:if>
			</plx:function>

			<plx:function visibility="privateInterfaceMember" name="GetNonGenericEnumerator">
				<plx:interfaceMember memberName="GetEnumerator" dataTypeName="IEnumerable" dataTypeQualifier="System.Collections"/>
				<plx:returns dataTypeName="IEnumerator" dataTypeQualifier="System.Collections"/>
				<plx:return>
					<plx:callThis name="GetEnumerator"/>
				</plx:return>
			</plx:function>
			<plx:function visibility="public" name="GetEnumerator">
				<plx:interfaceMember memberName="GetEnumerator" dataTypeName="IEnumerable">
					<plx:passTypeParam dataTypeName="TProperty"/>
				</plx:interfaceMember>
				<plx:returns dataTypeName="IEnumerator">
					<plx:passTypeParam dataTypeName="TProperty"/>
				</plx:returns>
				<plx:return>
					<plx:callInstance name="GetEnumerator">
						<plx:callObject>
							<plx:callThis type="field" name="{$PrivateMemberPrefix}List"/>
						</plx:callObject>
					</plx:callInstance>
				</plx:return>
			</plx:function>

			<xsl:variable name="commonAddRemoveCodeFragment">
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="identityEquality">
							<plx:left>
								<plx:nameRef type="parameter" name="item"/>
							</plx:left>
							<plx:right>
								<plx:nullKeyword/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:throw>
						<plx:callNew dataTypeName="ArgumentNullException">
							<plx:passParam>
								<plx:string data="item"/>
							</plx:passParam>
						</plx:callNew>
					</plx:throw>
				</plx:branch>
				<plx:local name="instance" dataTypeName="TClass">
					<plx:initialize>
						<plx:callThis type="field" name="{$PrivateMemberPrefix}Instance"/>
					</plx:initialize>
				</plx:local>
				<plx:local name="callbacks" dataTypeName="ConstraintEnforcementCollectionCallbacks">
					<plx:passTypeParam dataTypeName="TClass"/>
					<plx:passTypeParam dataTypeName="TProperty"/>
					<plx:initialize>
						<plx:callInstance name="GetConstraintEnforcementCollectionCallbacks">
							<plx:passMemberTypeParam dataTypeName="TClass"/>
							<plx:passMemberTypeParam dataTypeName="TProperty"/>
							<plx:callObject>
								<plx:callInstance type="property" name="Context">
									<plx:callObject>
										<plx:nameRef name="instance"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:callObject>
							<xsl:if test="$WithPropertyName">
								<plx:passParam>
									<plx:callThis type="field" name="{$PrivateMemberPrefix}PropertyName"/>
								</plx:passParam>
							</xsl:if>
						</plx:callInstance>
					</plx:initialize>
				</plx:local>
			</xsl:variable>
			<xsl:variable name="commonAddRemoveCode" select="exsl:node-set($commonAddRemoveCodeFragment)/child::*"/>		

			<plx:function visibility="public" name="Add">
				<plx:interfaceMember memberName="Add" dataTypeName="ICollection">
					<plx:passTypeParam dataTypeName="TProperty"/>
				</plx:interfaceMember>
				<plx:param name="item" dataTypeName="TProperty"/>
				<xsl:copy-of select="$commonAddRemoveCode"/>
				<plx:local name="adding" dataTypeName="PotentialCollectionModificationCallback">
					<plx:passTypeParam dataTypeName="TClass"/>
					<plx:passTypeParam dataTypeName="TProperty"/>
					<plx:initialize>
						<plx:callInstance type="field" name="Adding">
							<plx:callObject>
								<plx:nameRef name="callbacks"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:initialize>
				</plx:local>
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="booleanOr">
							<plx:left>
								<plx:binaryOperator type="identityEquality">
									<plx:left>
										<plx:cast type="exceptionCast" dataTypeName=".object">
											<plx:nameRef name="adding"/>
										</plx:cast>
									</plx:left>
									<plx:right>
										<plx:nullKeyword/>
									</plx:right>
								</plx:binaryOperator>
							</plx:left>
							<plx:right>
								<plx:callInstance type="delegateCall" name=".implied">
									<plx:callObject>
										<plx:nameRef name="adding"/>
									</plx:callObject>
									<plx:passParam>
										<plx:nameRef name="instance"/>
									</plx:passParam>
									<plx:passParam>
										<plx:nameRef type="parameter" name="item"/>
									</plx:passParam>
								</plx:callInstance>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:callInstance name="Add">
						<plx:callObject>
							<plx:callThis type="field" name="{$PrivateMemberPrefix}List"/>
						</plx:callObject>
						<plx:passParam>
							<plx:nameRef type="parameter" name="item"/>
						</plx:passParam>
					</plx:callInstance>
					<plx:local name="added" dataTypeName="CommittedCollectionModificationCallback">
						<plx:passTypeParam dataTypeName="TClass"/>
						<plx:passTypeParam dataTypeName="TProperty"/>
						<plx:initialize>
							<plx:callInstance type="field" name="Added">
								<plx:callObject>
									<plx:nameRef name="callbacks"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:initialize>
					</plx:local>
					<plx:branch>
						<plx:condition>
							<plx:binaryOperator type="identityInequality">
								<plx:left>
									<plx:cast type="exceptionCast" dataTypeName=".object">
										<plx:nameRef name="added"/>
									</plx:cast>
								</plx:left>
								<plx:right>
									<plx:nullKeyword/>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:callInstance type="delegateCall" name=".implied">
							<plx:callObject>
								<plx:nameRef name="added"/>
							</plx:callObject>
							<plx:passParam>
								<plx:nameRef name="instance"/>
							</plx:passParam>
							<plx:passParam>
								<plx:nameRef type="parameter" name="item"/>
							</plx:passParam>
						</plx:callInstance>
					</plx:branch>
				</plx:branch>
			</plx:function>

			<plx:function visibility="public" name="Remove">
				<plx:interfaceMember memberName="Remove" dataTypeName="ICollection">
					<plx:passTypeParam dataTypeName="TProperty"/>
				</plx:interfaceMember>
				<plx:param name="item" dataTypeName="TProperty"/>
				<plx:returns dataTypeName=".boolean"/>
				<xsl:copy-of select="$commonAddRemoveCode"/>
				<plx:local name="removing" dataTypeName="PotentialCollectionModificationCallback">
					<plx:passTypeParam dataTypeName="TClass"/>
					<plx:passTypeParam dataTypeName="TProperty"/>
					<plx:initialize>
						<plx:callInstance type="field" name="Removing">
							<plx:callObject>
								<plx:nameRef name="callbacks"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:initialize>
				</plx:local>
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="booleanOr">
							<plx:left>
								<plx:binaryOperator type="identityEquality">
									<plx:left>
										<plx:cast type="exceptionCast" dataTypeName=".object">
											<plx:nameRef name="removing"/>
										</plx:cast>
									</plx:left>
									<plx:right>
										<plx:nullKeyword/>
									</plx:right>
								</plx:binaryOperator>
							</plx:left>
							<plx:right>
								<plx:callInstance type="delegateCall" name=".implied">
									<plx:callObject>
										<plx:nameRef name="removing"/>
									</plx:callObject>
									<plx:passParam>
										<plx:nameRef name="instance"/>
									</plx:passParam>
									<plx:passParam>
										<plx:nameRef type="parameter" name="item"/>
									</plx:passParam>
								</plx:callInstance>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:branch>
						<plx:condition>
							<plx:callInstance name="Remove">
								<plx:callObject>
									<plx:callThis type="field" name="{$PrivateMemberPrefix}List"/>
								</plx:callObject>
								<plx:passParam>
									<plx:nameRef type="parameter" name="item"/>
								</plx:passParam>
							</plx:callInstance>
						</plx:condition>
						<plx:local name="removed" dataTypeName="CommittedCollectionModificationCallback">
							<plx:passTypeParam dataTypeName="TClass"/>
							<plx:passTypeParam dataTypeName="TProperty"/>
							<plx:initialize>
								<plx:callInstance type="field" name="Removed">
									<plx:callObject>
										<plx:nameRef name="callbacks"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:initialize>
						</plx:local>
						<plx:branch>
							<plx:condition>
								<plx:binaryOperator type="identityInequality">
									<plx:left>
										<plx:cast type="exceptionCast" dataTypeName=".object">
											<plx:nameRef name="removed"/>
										</plx:cast>
									</plx:left>
									<plx:right>
										<plx:nullKeyword/>
									</plx:right>
								</plx:binaryOperator>
							</plx:condition>
							<plx:callInstance type="delegateCall" name=".implied">
								<plx:callObject>
									<plx:nameRef name="removed"/>
								</plx:callObject>
								<plx:passParam>
									<plx:nameRef name="instance"/>
								</plx:passParam>
								<plx:passParam>
									<plx:nameRef type="parameter" name="item"/>
								</plx:passParam>
							</plx:callInstance>
						</plx:branch>
						<plx:return>
							<plx:trueKeyword/>
						</plx:return>
					</plx:branch>
				</plx:branch>
				<plx:return>
					<plx:falseKeyword/>
				</plx:return>
			</plx:function>
			
			<plx:function visibility="public" name="Clear">
				<plx:interfaceMember memberName="Clear" dataTypeName="ICollection">
					<plx:passTypeParam dataTypeName="TProperty"/>
				</plx:interfaceMember>
				<plx:local name="list" dataTypeName="List">
					<plx:passTypeParam dataTypeName="TProperty"/>
					<plx:initialize>
						<plx:callThis type="field" name="{$PrivateMemberPrefix}List"/>
					</plx:initialize>
				</plx:local>
				<plx:loop>
					<plx:initializeLoop>
						<plx:local name="i" dataTypeName=".i4">
							<plx:initialize>
								<plx:binaryOperator type="subtract">
									<plx:left>
										<plx:callInstance type="property" name="Count">
											<plx:callObject>
												<plx:nameRef name="list"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:value type="i4" data="1"/>
									</plx:right>
								</plx:binaryOperator>
							</plx:initialize>
						</plx:local>
					</plx:initializeLoop>
					<plx:condition>
						<plx:binaryOperator type="greaterThan">
							<plx:left>
								<plx:nameRef name="i"/>
							</plx:left>
							<plx:right>
								<plx:value type="i4" data="0"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:beforeLoop>
						<plx:decrement>
							<plx:nameRef name="i"/>
						</plx:decrement>
					</plx:beforeLoop>
					<plx:callThis name="Remove">
						<plx:passParam>
							<plx:callInstance type="indexerCall" name=".implied">
								<plx:callObject>
									<plx:nameRef name="list"/>
								</plx:callObject>
								<plx:passParam>
									<plx:nameRef name="i"/>
								</plx:passParam>
							</plx:callInstance>
						</plx:passParam>
					</plx:callThis>
				</plx:loop>
			</plx:function>

			<plx:function visibility="public" name="Contains">
				<plx:interfaceMember memberName="Contains" dataTypeName="ICollection">
					<plx:passTypeParam dataTypeName="TProperty"/>
				</plx:interfaceMember>
				<plx:param name="item" dataTypeName="TProperty"/>
				<plx:returns dataTypeName=".boolean"/>
				<plx:return>
					<plx:binaryOperator type="booleanAnd">
						<plx:left>
							<plx:binaryOperator type="identityInequality">
								<plx:left>
									<plx:nameRef type="parameter" name="item"/>
								</plx:left>
								<plx:right>
									<plx:nullKeyword/>
								</plx:right>
							</plx:binaryOperator>
						</plx:left>
						<plx:right>
							<plx:callInstance name="Contains">
								<plx:callObject>
									<plx:callThis type="field" name="{$PrivateMemberPrefix}List"/>
								</plx:callObject>
								<plx:passParam>
									<plx:nameRef type="parameter" name="item"/>
								</plx:passParam>
							</plx:callInstance>
						</plx:right>
					</plx:binaryOperator>
				</plx:return>
			</plx:function>
			<plx:function visibility="public" name="CopyTo">
				<plx:interfaceMember memberName="CopyTo" dataTypeName="ICollection">
					<plx:passTypeParam dataTypeName="TProperty"/>
				</plx:interfaceMember>
				<plx:param name="array" dataTypeName="TProperty" dataTypeIsSimpleArray="true"/>
				<plx:param name="arrayIndex" dataTypeName=".i4"/>
				<plx:callInstance name="CopyTo">
					<plx:callObject>
						<plx:callThis type="field" name="{$PrivateMemberPrefix}List"/>
					</plx:callObject>
					<plx:passParam>
						<plx:nameRef type="parameter" name="array"/>
					</plx:passParam>
					<plx:passParam>
						<plx:nameRef type="parameter" name="arrayIndex"/>
					</plx:passParam>
				</plx:callInstance>
			</plx:function>
			<plx:property visibility="public" name="Count">
				<plx:interfaceMember memberName="Count" dataTypeName="ICollection">
					<plx:passTypeParam dataTypeName="TProperty"/>
				</plx:interfaceMember>
				<plx:returns dataTypeName=".i4"/>
				<plx:get>
					<plx:return>
						<plx:callInstance type="property" name="Count">
							<plx:callObject>
								<plx:callThis type="field" name="{$PrivateMemberPrefix}List"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:return>
				</plx:get>
			</plx:property>
			<plx:property visibility="public" name="IsReadOnly">
				<plx:interfaceMember memberName="IsReadOnly" dataTypeName="ICollection">
					<plx:passTypeParam dataTypeName="TProperty"/>
				</plx:interfaceMember>
				<plx:returns dataTypeName=".boolean"/>
				<plx:get>
					<plx:return>
						<plx:falseKeyword/>
					</plx:return>
				</plx:get>
			</plx:property>

		</plx:class>
	</xsl:template>


	<xsl:template match="oial:conceptType" mode="GenerateImplementationClass">
		<xsl:param name="Model"/>
		<xsl:param name="ModelContextName"/>
		<xsl:param name="AllUniquenessConstraints"/>
		<xsl:param name="AllProperties"/>
		<xsl:param name="AllReverseChildProperties"/>
		<xsl:param name="Properties"/>
		<xsl:param name="ClassName"/>
		<xsl:param name="ConceptTypes"/>
		<xsl:param name="Assimilations"/>
		<xsl:variable name="ImplementationClassName" select="concat($ClassName,$ImplementationClassSuffix)"/>
		<xsl:variable name="mandatoryProperties" select="$Properties[@mandatory='alethic']"/>
		<xsl:variable name="uniquenessConstraints" select="oial:uniquenessConstraints/oial:uniquenessConstraint"/>
		<plx:pragma type="region" data="{$ClassName}"/>
		<plx:function visibility="{$ModelContextInterfaceImplementationVisibility}" name="Create{$ClassName}">
			<plx:interfaceMember memberName="Create{$ClassName}" dataTypeName="I{$ModelContextName}"/>
			<xsl:for-each select="$mandatoryProperties[not(@isIdentity='true')]">
				<plx:param name="{@paramName}">
					<xsl:copy-of select="prop:DataType/@*"/>
					<xsl:copy-of select="prop:DataType/child::*"/>
				</plx:param>
			</xsl:for-each>
			<plx:returns dataTypeName="{$ClassName}"/>
			<xsl:for-each select="$mandatoryProperties[@canBeNull='true']">
				<!-- We don't need to worry about Nullable here, since a property that is alethicly mandatory will never be made Nullable. -->
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="identityEquality">
							<plx:left>
								<plx:cast type="exceptionCast" dataTypeName=".object">
									<plx:nameRef type="parameter" name="{@paramName}"/>
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
									<xsl:value-of select="@paramName"/>
								</plx:string>
							</plx:passParam>
						</plx:callNew>
					</plx:throw>
				</plx:branch>
			</xsl:for-each>
			<xsl:for-each select="$mandatoryProperties[not(@isIdentity='true')]">
				<plx:branch>
					<plx:condition>
						<plx:unaryOperator type="booleanNot">
							<plx:callThis name="On{$ClassName}{@name}Changing">
								<plx:passParam>
									<plx:nullKeyword/>
								</plx:passParam>
								<plx:passParam>
									<plx:nameRef type="parameter" name="{@paramName}"/>
								</plx:passParam>
							</plx:callThis>
						</plx:unaryOperator>
					</plx:condition>
					<plx:throw>
						<plx:callStatic name="GetConstraintEnforcementFailedException" dataTypeName="{$ModelContextName}">
							<plx:passParam>
								<plx:string>
									<xsl:value-of select="@paramName"/>
								</plx:string>
							</plx:passParam>
						</plx:callStatic>
					</plx:throw>
				</plx:branch>
			</xsl:for-each>
			<plx:return>
				<plx:callNew dataTypeName="{$ImplementationClassName}">
					<plx:passParam>
						<plx:thisKeyword/>
					</plx:passParam>
					<xsl:for-each select="$mandatoryProperties[not(@isIdentity='true')]">
						<plx:passParam>
							<plx:nameRef type="parameter" name="{@paramName}"/>
						</plx:passParam>
					</xsl:for-each>
				</plx:callNew>
			</plx:return>
		</plx:function>

		<xsl:apply-templates select="$Properties" mode="GenerateImplementationPropertyChangeMethods">
			<xsl:with-param name="Model" select="$Model"/>
			<xsl:with-param name="ModelContextName" select="$ModelContextName"/>
			<xsl:with-param name="ClassName" select="$ClassName"/>
			<xsl:with-param name="Properties" select="$Properties"/>
			<xsl:with-param name="AllReverseChildProperties" select="$AllReverseChildProperties"/>
			<xsl:with-param name="AllUniquenessConstraints" select="$AllUniquenessConstraints"/>
		</xsl:apply-templates>
		
		<plx:field visibility="private" readOnly="true" name="{$PrivateMemberPrefix}{$ClassName}List" dataTypeName="List">
			<plx:passTypeParam dataTypeName="{$ClassName}"/>
		</plx:field>
		<plx:field visibility="private" readOnly="true" name="{$PrivateMemberPrefix}{$ClassName}ReadOnlyCollection" dataTypeName="ReadOnlyCollection">
			<plx:passTypeParam dataTypeName="{$ClassName}"/>
		</plx:field>
		<plx:property visibility="{$ModelContextInterfaceImplementationVisibility}" name="{$ClassName}Collection">
			<plx:interfaceMember memberName="{$ClassName}Collection" dataTypeName="I{$ModelContextName}"/>
			<plx:returns dataTypeName="IEnumerable">
				<plx:passTypeParam dataTypeName="{$ClassName}"/>
			</plx:returns>
			<plx:get>
				<plx:return>
					<plx:callThis type="field" name="{$PrivateMemberPrefix}{$ClassName}ReadOnlyCollection"/>
				</plx:return>
			</plx:get>
		</plx:property>

		<plx:class visibility="private" modifier="sealed" name="{$ImplementationClassName}">
			<plx:leadingInfo>
				<plx:pragma type="region" data="{$ImplementationClassName}"/>
			</plx:leadingInfo>
			<plx:trailingInfo>
				<plx:pragma type="closeRegion" data="{$ImplementationClassName}"/>
			</plx:trailingInfo>
			<xsl:copy-of select="$StructLayoutAttribute"/>
			<plx:derivesFromClass dataTypeName="{$ClassName}"/>

			<plx:function visibility="public" name=".construct">
				<plx:param name="context" dataTypeName="{$ModelContextName}"/>
				<xsl:for-each select="$mandatoryProperties[not(@isIdentity='true')]">
					<plx:param name="{@paramName}">
						<xsl:copy-of select="prop:DataType/@*"/>
						<xsl:copy-of select="prop:DataType/child::*"/>
					</plx:param>
				</xsl:for-each>
				<plx:assign>
					<plx:left>
						<plx:callThis type="field" name="{$PrivateMemberPrefix}Context"/>
					</plx:left>
					<plx:right>
						<plx:nameRef type="parameter" name="context"/>
					</plx:right>
				</plx:assign>
				<xsl:for-each select="$Properties[@isCollection='true']">
					<xsl:variable name="propertyTypeName" select="prop:DataType/plx:passTypeParam/@dataTypeName"/>
					<plx:assign>
						<plx:left>
							<plx:callThis type="field" name="{$PrivateMemberPrefix}{@name}"/>
						</plx:left>
						<plx:right>
							<xsl:choose>
								<xsl:when test="count($Properties[@isCollection='true' and prop:DataType/plx:passTypeParam/@dataTypeName=$propertyTypeName])>1">
									<plx:callNew dataTypeName="ConstraintEnforcementCollectionWithPropertyName">
										<plx:passTypeParam dataTypeName="{$ClassName}"/>
										<plx:passTypeParam dataTypeName="{$propertyTypeName}"/>
										<plx:passParam>
											<plx:thisKeyword/>
										</plx:passParam>
										<plx:passParam>
											<plx:string>
												<xsl:value-of select="@name"/>
											</plx:string>
										</plx:passParam>
									</plx:callNew>
								</xsl:when>
								<xsl:otherwise>
									<plx:callNew dataTypeName="ConstraintEnforcementCollection">
										<plx:passTypeParam dataTypeName="{$ClassName}"/>
										<plx:passTypeParam dataTypeName="{$propertyTypeName}"/>
										<plx:passParam>
											<plx:thisKeyword/>
										</plx:passParam>
									</plx:callNew>
								</xsl:otherwise>
							</xsl:choose>
						</plx:right>
					</plx:assign>
				</xsl:for-each>
				<xsl:for-each select="$mandatoryProperties[not(@isIdentity='true')]">
					<plx:assign>
						<plx:left>
							<plx:callThis type="field" name="{$PrivateMemberPrefix}{@name}"/>
						</plx:left>
						<plx:right>
							<plx:nameRef type="parameter" name="{@paramName}"/>
						</plx:right>
					</plx:assign>
					<xsl:if test="@isUnique='true' or @isCustomType='true' or $uniquenessConstraints[oial:uniquenessChild/@ref=current()/@childId]">
						<plx:callInstance name="On{$ClassName}{@name}Changed">
							<plx:callObject>
								<plx:nameRef type="parameter" name="context"/>
							</plx:callObject>
							<plx:passParam>
								<plx:thisKeyword/>
							</plx:passParam>
							<plx:passParam>
								<plx:nullKeyword/>
							</plx:passParam>
						</plx:callInstance>
					</xsl:if>
				</xsl:for-each>
				<plx:callInstance name="Add">
					<plx:callObject>
						<plx:callInstance type="field" name="{$PrivateMemberPrefix}{$ClassName}List">
							<plx:callObject>
								<plx:nameRef type="parameter" name="context"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:callObject>
					<plx:passParam>
						<plx:thisKeyword/>
					</plx:passParam>
				</plx:callInstance>
			</plx:function>

			<plx:field visibility="private" readOnly="true" name="{$PrivateMemberPrefix}Context" dataTypeName="{$ModelContextName}"/>
			<plx:property visibility="public" modifier="sealedOverride" name="Context">
				<plx:returns dataTypeName="{$ModelContextName}"/>
				<plx:get>
					<plx:return>
						<plx:callThis type="field" name="{$PrivateMemberPrefix}Context"/>
					</plx:return>
				</plx:get>
			</plx:property>

			<xsl:apply-templates select="$Properties" mode="GenerateImplementationProperty">
				<xsl:with-param name="ClassName" select="$ClassName"/>
				<xsl:with-param name="AllUniquenessConstraints" select="$AllUniquenessConstraints"/>
			</xsl:apply-templates>
		</plx:class>
		<plx:pragma type="closeRegion" data="{$ClassName}"/>
	</xsl:template>

	<xsl:template match="prop:Property" mode="GenerateImplementationProperty">
		<xsl:param name="ClassName"/>
		<xsl:param name="AllUniquenessConstraints"/>
		
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
		<plx:property visibility="public" modifier="sealedOverride" name="{@name}">
			<plx:returns>
				<xsl:copy-of select="prop:DataType/@*"/>
				<xsl:copy-of select="prop:DataType/child::*"/>
			</plx:returns>
			<plx:get>
				<plx:return>
					<xsl:choose>
						<xsl:when test="@isIdentity='true'">
							<plx:thisKeyword/>
						</xsl:when>
						<xsl:otherwise>
							<plx:callThis type="field" name="{$PrivateMemberPrefix}{@name}"/>
						</xsl:otherwise>
					</xsl:choose>
				</plx:return>
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
										<plx:valueKeyword stringize="true"/>
									</plx:passParam>
								</plx:callNew>
							</plx:throw>
						</plx:branch>
					</xsl:if>
					<plx:local name="oldValue">
						<xsl:copy-of select="prop:DataType/@*"/>
						<xsl:copy-of select="prop:DataType/child::*"/>
						<plx:initialize>
							<plx:callThis type="field" name="{$PrivateMemberPrefix}{@name}"/>
						</plx:initialize>
					</plx:local>
					<plx:branch>
						<plx:condition>
							<xsl:choose>
								<xsl:when test="@isCustomType='true'">
									<plx:binaryOperator type="identityInequality">
										<plx:left>
											<plx:cast type="exceptionCast" dataTypeName=".object">
												<plx:nameRef name="oldValue"/>
											</plx:cast>
										</plx:left>
										<plx:right>
											<plx:cast type="exceptionCast" dataTypeName=".object">
												<plx:valueKeyword/>
											</plx:cast>
										</plx:right>
									</plx:binaryOperator>
								</xsl:when>
								<xsl:when test="@canBeNull='false'">
									<plx:binaryOperator type="inequality">
										<plx:left>
											<plx:nameRef name="oldValue"/>
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
													<plx:callInstance name="GetValueOrDefault">
														<plx:callObject>
															<plx:nameRef name="oldValue"/>
														</plx:callObject>
													</plx:callInstance>
												</plx:left>
												<plx:right>
													<plx:callInstance name="GetValueOrDefault">
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
															<plx:nameRef name="oldValue"/>
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
								<xsl:when test="@mandatory='alethic'">
									<!-- We have already checked the new value to ensure it is not null. -->
									<plx:binaryOperator type="booleanAnd">
										<plx:left>
											<plx:binaryOperator type="identityInequality">
												<plx:left>
													<plx:cast type="exceptionCast" dataTypeName=".object">
														<plx:nameRef name="oldValue"/>
													</plx:cast>
												</plx:left>
												<plx:right>
													<plx:cast type="exceptionCast" dataTypeName=".object">
														<plx:valueKeyword/>
													</plx:cast>
												</plx:right>
											</plx:binaryOperator>
										</plx:left>
										<plx:right>
											<plx:unaryOperator type="booleanNot">
												<plx:callInstance name="Equals">
													<plx:callObject>
														<plx:valueKeyword/>
													</plx:callObject>
													<plx:passParam>
														<plx:nameRef name="oldValue"/>
													</plx:passParam>
												</plx:callInstance>
											</plx:unaryOperator>
										</plx:right>
									</plx:binaryOperator>
								</xsl:when>
								<xsl:otherwise>
									<plx:unaryOperator type="booleanNot">
										<plx:callStatic name="Equals" dataTypeName=".object">
											<plx:passParam>
												<plx:nameRef name="oldValue"/>
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
								<plx:binaryOperator type="booleanAnd">
									<plx:left>
										<plx:callInstance name="On{$ClassName}{@name}Changing">
											<plx:callObject>
												<plx:callThis type="field" name="{$PrivateMemberPrefix}Context"/>
											</plx:callObject>
											<plx:passParam>
												<plx:thisKeyword/>
											</plx:passParam>
											<plx:passParam>
												<plx:valueKeyword/>
											</plx:passParam>
										</plx:callInstance>
									</plx:left>
									<plx:right>
										<plx:callThis accessor="base" name="On{@name}Changing">
											<plx:passParam>
												<plx:valueKeyword/>
											</plx:passParam>
										</plx:callThis>
									</plx:right>
								</plx:binaryOperator>
							</plx:condition>
							<plx:assign>
								<plx:left>
									<plx:callThis type="field" name="{$PrivateMemberPrefix}{@name}"/>
								</plx:left>
								<plx:right>
									<plx:valueKeyword/>
								</plx:right>
							</plx:assign>
							<xsl:if test="@isUnique='true' or @isCustomType='true' or $AllUniquenessConstraints[../../@id=current()/../@conceptTypeId][oial:uniquenessChild/@ref=current()/@childId]">
								<plx:callInstance name="On{$ClassName}{@name}Changed">
									<plx:callObject>
										<plx:callThis type="field" name="{$PrivateMemberPrefix}Context"/>
									</plx:callObject>
									<plx:passParam>
										<plx:thisKeyword/>
									</plx:passParam>
									<plx:passParam>
										<plx:nameRef name="oldValue"/>
									</plx:passParam>
								</plx:callInstance>
							</xsl:if>
							<plx:callThis accessor="base" name="On{@name}Changed">
								<plx:passParam>
									<plx:nameRef name="oldValue"/>
								</plx:passParam>
							</plx:callThis>
						</plx:branch>
					</plx:branch>
				</plx:set>
			</xsl:if>
		</plx:property>
	</xsl:template>

	<xsl:template match="prop:Property" mode="GenerateImplementationPropertyChangeMethods">
		<xsl:param name="Model"/>
		<xsl:param name="ModelContextName"/>
		<xsl:param name="ClassName"/>
		<xsl:param name="Properties"/>
		<xsl:param name="AllReverseChildProperties"/>
		<xsl:param name="AllUniquenessConstraints"/>
		<xsl:variable name="propertyName" select="string(@name)"/>
		<xsl:variable name="propertyChildId" select="@childId"/>
		<xsl:choose>
			<xsl:when test="@isCollection='true'">
				<xsl:variable name="itemFragment">
					<plx:nameRef type="parameter" name="item"/>
				</xsl:variable>
				<xsl:variable name="item" select="exsl:node-set($itemFragment)/child::*"/>
				<!-- COLLECTION_CHANGE_METHODS_CONDITIONS: Search for this comment to find other locations where these conditions also exist. -->
				<xsl:if test="@isCustomType='true'">
					<plx:function visibility="private" name="On{$ClassName}{@name}Adding">
						<plx:param name="instance" dataTypeName="{$ClassName}"/>
						<plx:param name="item">
							<xsl:copy-of select="prop:DataType/plx:passTypeParam/@*"/>
							<xsl:copy-of select="prop:DataType/plx:passTypeParam/child::*"/>
						</plx:param>
						<plx:returns dataTypeName=".boolean"/>
						<xsl:if test="@isCustomType='true'">
							<xsl:call-template name="GetImplementationPropertyChangingMethodCheckSameContextCode">
								<xsl:with-param name="ModelContextName" select="$ModelContextName"/>
								<xsl:with-param name="NewValue" select="$item"/>
								<xsl:with-param name="ParamName" select="'item'"/>
							</xsl:call-template>
						</xsl:if>
						<plx:return>
							<plx:trueKeyword/>
						</plx:return>
					</plx:function>
				</xsl:if>
				<xsl:if test="@isUnique='true' or @isCustomType='true'">
					<plx:function visibility="private" name="On{$ClassName}{@name}Added">
						<plx:param name="instance" dataTypeName="{$ClassName}"/>
						<plx:param name="item">
							<xsl:copy-of select="prop:DataType/plx:passTypeParam/@*"/>
							<xsl:copy-of select="prop:DataType/plx:passTypeParam/child::*"/>
						</plx:param>
						<xsl:call-template name="GetImplementationPropertyChangedMethodUpdateNewOppositeObjectCode">
							<xsl:with-param name="ClassName" select="$ClassName"/>
							<xsl:with-param name="NewValue" select="$item"/>
							<xsl:with-param name="AllReverseChildProperties" select="$AllReverseChildProperties"/>
							<xsl:with-param name="ShouldCheckForNull" select="false()"/>
						</xsl:call-template>
					</plx:function>
				</xsl:if>
				<xsl:if test="false()">
					<plx:function visibility="private" name="On{$ClassName}{@name}Removing">
						<plx:param name="instance" dataTypeName="{$ClassName}"/>
						<plx:param name="item">
							<xsl:copy-of select="prop:DataType/plx:passTypeParam/@*"/>
							<xsl:copy-of select="prop:DataType/plx:passTypeParam/child::*"/>
						</plx:param>
						<plx:returns dataTypeName=".boolean"/>
						<plx:return>
							<plx:trueKeyword/>
						</plx:return>
					</plx:function>
				</xsl:if>
				<xsl:if test="@isUnique='true' or @isCustomType='true'">
					<plx:function visibility="private" name="On{$ClassName}{@name}Removed">
						<plx:param name="instance" dataTypeName="{$ClassName}"/>
						<plx:param name="item">
							<xsl:copy-of select="prop:DataType/plx:passTypeParam/@*"/>
							<xsl:copy-of select="prop:DataType/plx:passTypeParam/child::*"/>
						</plx:param>
						<xsl:variable name="implementationPropertyChangedMethodUpdateOldOppositeObjectCodeFragment">
							<xsl:call-template name="GetImplementationPropertyChangedMethodUpdateOldOppositeObjectCode">
								<xsl:with-param name="ClassName" select="$ClassName"/>
								<xsl:with-param name="OldValue" select="$item"/>
								<xsl:with-param name="AllReverseChildProperties" select="$AllReverseChildProperties"/>
								<xsl:with-param name="ShouldCheckForNull" select="false()"/>
							</xsl:call-template>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="@isUnique='true' and @isCustomType='true'">
								<plx:branch>
									<plx:condition>
										<plx:binaryOperator type="identityEquality">
											<plx:left>
												<plx:cast type="exceptionCast" dataTypeName=".object">
													<plx:callInstance type="property" name="{@oppositeName}">
														<plx:callObject>
															<xsl:copy-of select="$item"/>
														</plx:callObject>
													</plx:callInstance>
												</plx:cast>
											</plx:left>
											<plx:right>
												<plx:cast type="exceptionCast" dataTypeName=".object">
													<plx:nameRef type="parameter" name="instance"/>
												</plx:cast>
											</plx:right>
										</plx:binaryOperator>
									</plx:condition>
									<xsl:copy-of select="exsl:node-set($implementationPropertyChangedMethodUpdateOldOppositeObjectCodeFragment)/child::*"/>
								</plx:branch>
							</xsl:when>
							<xsl:otherwise>
								<xsl:copy-of select="exsl:node-set($implementationPropertyChangedMethodUpdateOldOppositeObjectCodeFragment)/child::*"/>
							</xsl:otherwise>
						</xsl:choose>
					</plx:function>
				</xsl:if>
			</xsl:when>
			<xsl:otherwise>
				<xsl:variable name="compoundUniquenessConstraints" select="$AllUniquenessConstraints[../../@id=current()/../@conceptTypeId][count(oial:uniquenessChild)>1][oial:uniquenessChild/@ref=current()/@childId]"/>
				<xsl:if test="not(@isIdentity='true')">
					<plx:function visibility="private" name="On{$ClassName}{@name}Changing">
						<plx:param name="instance" dataTypeName="{$ClassName}"/>
						<plx:param name="newValue">
							<xsl:copy-of select="prop:DataType/@*"/>
							<xsl:copy-of select="prop:DataType/child::*"/>
						</plx:param>
						<plx:returns dataTypeName=".boolean"/>
						
						<xsl:if test="@isCustomType='true' or @isUnique='true'">
							<xsl:variable name="validationCodeFragment">
								<xsl:choose>
									<xsl:when test="@isCustomType='true'">
										<xsl:variable name="newValueFragment">
											<plx:nameRef type="parameter" name="newValue"/>
										</xsl:variable>
										<xsl:call-template name="GetImplementationPropertyChangingMethodCheckSameContextCode">
											<xsl:with-param name="ModelContextName" select="$ModelContextName"/>
											<xsl:with-param name="NewValue" select="exsl:node-set($newValueFragment)/child::*"/>
										</xsl:call-template>
									</xsl:when>
									<xsl:otherwise>
										<plx:local name="currentInstance" dataTypeName="{$ClassName}"/>
										<plx:branch>
											<plx:condition>
												<plx:callInstance name="TryGetValue">
													<plx:callObject>
														<plx:callThis type="field" name="{$PrivateMemberPrefix}{$ClassName}{@name}Dictionary"/>
													</plx:callObject>
													<plx:passParam>
														<xsl:choose>
															<xsl:when test="@canBeNull='true' and prop:DataType/@dataTypeName='Nullable'">
																<plx:callInstance name="GetValueOrDefault">
																	<plx:callObject>
																		<plx:nameRef type="parameter" name="newValue"/>
																	</plx:callObject>
																</plx:callInstance>
															</xsl:when>
															<xsl:otherwise>
																<plx:nameRef type="parameter" name="newValue"/>
															</xsl:otherwise>
														</xsl:choose>
													</plx:passParam>
													<plx:passParam type="out">
														<plx:nameRef name="currentInstance"/>
													</plx:passParam>
												</plx:callInstance>
											</plx:condition>
											<plx:branch>
												<plx:condition>
													<plx:binaryOperator type="identityInequality">
														<plx:left>
															<plx:cast type="exceptionCast" dataTypeName=".object">
																<plx:nameRef name="currentInstance"/>
															</plx:cast>
														</plx:left>
														<plx:right>
															<plx:cast type="exceptionCast" dataTypeName=".object">
																<plx:nameRef type="parameter" name="instance"/>
															</plx:cast>
														</plx:right>
													</plx:binaryOperator>
												</plx:condition>
												<plx:return>
													<plx:falseKeyword/>
												</plx:return>
											</plx:branch>
										</plx:branch>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<xsl:variable name="validationCode" select="exsl:node-set($validationCodeFragment)/child::*"/>
							
							<xsl:choose>
								<!-- If @mandatory='alethic', newValue has already been checked by the caller to ensure that it is not null. -->
								<xsl:when test="@mandatory='alethic' or @canBeNull='false'">
									<xsl:copy-of select="$validationCode"/>
								</xsl:when>
								<xsl:otherwise>
									<plx:branch>
										<plx:condition>
											<xsl:choose>
												<xsl:when test="@canBeNull='true' and prop:DataType/@dataTypeName='Nullable'">
													<plx:callInstance type="property" name="HasValue">
														<plx:callObject>
															<plx:nameRef type="parameter" name="newValue"/>
														</plx:callObject>
													</plx:callInstance>
												</xsl:when>
												<xsl:otherwise>
													<plx:binaryOperator type="identityInequality">
														<plx:left>
															<plx:cast type="exceptionCast" dataTypeName=".object">
																<plx:nameRef type="parameter" name="newValue"/>
															</plx:cast>
														</plx:left>
														<plx:right>
															<plx:nullKeyword/>
														</plx:right>
													</plx:binaryOperator>
												</xsl:otherwise>
											</xsl:choose>
										</plx:condition>
										<xsl:copy-of select="$validationCode"/>
									</plx:branch>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:if>

						<xsl:if test="$compoundUniquenessConstraints">
							<plx:branch>
								<plx:condition>
									<plx:binaryOperator type="identityInequality">
										<plx:left>
											<plx:cast type="exceptionCast" dataTypeName=".object">
												<plx:nameRef type="parameter" name="instance"/>
											</plx:cast>
										</plx:left>
										<plx:right>
											<plx:nullKeyword/>
										</plx:right>
									</plx:binaryOperator>
								</plx:condition>
								<xsl:for-each select="$compoundUniquenessConstraints">
									<xsl:variable name="uniquenessChildren" select="oial:uniquenessChild"/>
									<xsl:variable name="uniquenessSignatureFragment">
										<xsl:for-each select="$uniquenessChildren">
											<xsl:if test="position()!=1">
												<xsl:text>And</xsl:text>
											</xsl:if>
											<xsl:value-of select="$Properties[@childId=current()/@ref]/@name"/>
										</xsl:for-each>
									</xsl:variable>
									<xsl:variable name="uniquenessSignature" select="string($uniquenessSignatureFragment)"/>
									<xsl:if test="$uniquenessSignature='CommunicationSatelliteAnd'">
										<xsl:message>Stop</xsl:message>
									</xsl:if>
									<xsl:variable name="hasNullables" select="$Properties[@childId=$uniquenessChildren/@ref and @isCustomType='false' and @canBeNull='true' and prop:DataType/@dataTypeName='Nullable']"/>
									<xsl:if test="$hasNullables">
										<!-- TODO: We could do null checking and handling here, rather than relying on CreateTuple to do it for us. This would also be better for Nullables. -->
									</xsl:if>
									<plx:branch>
										<plx:condition>
											<plx:unaryOperator type="booleanNot">
												<plx:callThis name="On{$uniquenessSignature}Changing">
													<plx:passParam>
														<plx:nameRef type="parameter" name="instance"/>
													</plx:passParam>
													<plx:passParam>
														<xsl:choose>
															<xsl:when test="count($uniquenessChildren)=1">
																<plx:nameRef type="parameter" name="newValue"/>
															</xsl:when>
															<xsl:otherwise>
																<plx:callStatic name="CreateTuple" dataTypeName="Tuple">
																	<xsl:for-each select="$uniquenessChildren">
																		<plx:passMemberTypeParam>
																			<xsl:variable name="dataType" select="$Properties[@childId=current()/@ref]/prop:DataType"/>
																			<xsl:copy-of select="$dataType/@*"/>
																			<xsl:copy-of select="$dataType/child::*"/>
																		</plx:passMemberTypeParam>
																	</xsl:for-each>
																	<xsl:for-each select="$uniquenessChildren">
																		<plx:passParam>
																			<xsl:choose>
																				<xsl:when test="@ref=$propertyChildId">
																					<plx:nameRef type="parameter" name="newValue"/>
																				</xsl:when>
																				<xsl:otherwise>
																					<plx:callInstance type="property" name="{$Properties[@childId=current()/@ref]/@name}">
																						<plx:callObject>
																							<plx:nameRef type="parameter" name="instance"/>
																						</plx:callObject>
																					</plx:callInstance>
																				</xsl:otherwise>
																			</xsl:choose>
																		</plx:passParam>
																	</xsl:for-each>
																</plx:callStatic>
															</xsl:otherwise>
														</xsl:choose>
													</plx:passParam>
												</plx:callThis>
											</plx:unaryOperator>
										</plx:condition>
										<plx:return>
											<plx:falseKeyword/>
										</plx:return>
									</plx:branch>
								</xsl:for-each>
							</plx:branch>
						</xsl:if>
						
						<plx:return>
							<plx:trueKeyword/>
						</plx:return>
					</plx:function>

					<xsl:if test="@isUnique='true' or @isCustomType='true' or $compoundUniquenessConstraints">
						<plx:function visibility="private" overload="true" name="On{$ClassName}{$propertyName}Changed">
							<plx:param name="instance" dataTypeName="{$ClassName}"/>
							<plx:param name="oldValue">
								<xsl:choose>
									<xsl:when test="@canBeNull='false'">
										<xsl:attribute name="dataTypeName">
											<xsl:value-of select="'Nullable'"/>
										</xsl:attribute>
										<plx:passTypeParam>
											<xsl:copy-of select="prop:DataType/@*"/>
											<xsl:copy-of select="prop:DataType/child::*"/>
										</plx:passTypeParam>
									</xsl:when>
									<xsl:otherwise>
										<xsl:copy-of select="prop:DataType/@*"/>
										<xsl:copy-of select="prop:DataType/child::*"/>
									</xsl:otherwise>
								</xsl:choose>
							</plx:param>
							
							<xsl:if test="@isUnique='true' or @isCustomType='true'">
								<xsl:variable name="newValueFragment">
									<plx:callInstance type="property" name="{$propertyName}">
										<plx:callObject>
											<plx:nameRef type="parameter" name="instance"/>
										</plx:callObject>
									</plx:callInstance>
								</xsl:variable>
								<xsl:call-template name="GetImplementationPropertyChangedMethodUpdateNewOppositeObjectCode">
									<xsl:with-param name="ClassName" select="$ClassName"/>
									<xsl:with-param name="NewValue" select="$newValueFragment"/>
									<xsl:with-param name="AllReverseChildProperties" select="$AllReverseChildProperties"/>
								</xsl:call-template>
							</xsl:if>

							<xsl:variable name="oldValueNotNullValueFragment">
								<xsl:choose>
									<xsl:when test="@canBeNull='false' or prop:DataType/@dataTypeName='Nullable'">
										<plx:callInstance name="GetValueOrDefault">
											<plx:callObject>
												<plx:nameRef type="parameter" name="oldValue"/>
											</plx:callObject>
										</plx:callInstance>
									</xsl:when>
									<xsl:otherwise>
										<plx:nameRef type="parameter" name="oldValue"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<xsl:variable name="oldValueNotNullValue" select="exsl:node-set($oldValueNotNullValueFragment)/child::*"/>

							<xsl:variable name="uniquenessConstraintsDataTypesFragment">
								<xsl:for-each select="$compoundUniquenessConstraints">
									<xsl:variable name="uniquenessChildren" select="oial:uniquenessChild"/>
									<xsl:variable name="uniquenessSignatureFragment">
										<xsl:for-each select="$uniquenessChildren">
											<xsl:if test="position()!=1">
												<xsl:text>And</xsl:text>
											</xsl:if>
											<xsl:value-of select="$Properties[@childId=current()/@ref]/@name"/>
										</xsl:for-each>
									</xsl:variable>
									<xsl:variable name="uniquenessSignature" select="string($uniquenessSignatureFragment)"/>
									<RoleSequenceUniquenessConstraint id="{@id}" signature="{$uniquenessSignature}">
										<xsl:for-each select="$uniquenessChildren">
											<xsl:copy-of select="$Properties[@childId=current()/@ref]/prop:DataType"/>
										</xsl:for-each>
									</RoleSequenceUniquenessConstraint>
								</xsl:for-each>
							</xsl:variable>
							<xsl:variable name="uniquenessConstraintsDataTypes" select="exsl:node-set($uniquenessConstraintsDataTypesFragment)/child::*"/>

							<xsl:for-each select="$compoundUniquenessConstraints">
								<xsl:variable name="cachedDataTypes" select="$uniquenessConstraintsDataTypes[@id=current()/@id]"/>
								<plx:local name="{$cachedDataTypes/@signature}OldValueTuple" dataTypeName="Tuple">
									<xsl:for-each select="$cachedDataTypes/prop:DataType">
										<plx:passTypeParam>
											<xsl:copy-of select="@*"/>
											<xsl:copy-of select="child::*"/>
										</plx:passTypeParam>
									</xsl:for-each>
								</plx:local>
							</xsl:for-each>

							<plx:branch>
								<plx:condition>
									<xsl:choose>
										<xsl:when test="@canBeNull='false' or prop:DataType/@dataTypeName='Nullable'">
											<plx:callInstance type="property" name="HasValue">
												<plx:callObject>
													<plx:nameRef type="parameter" name="oldValue"/>
												</plx:callObject>
											</plx:callInstance>
										</xsl:when>
										<xsl:otherwise>
											<plx:binaryOperator type="identityInequality">
												<plx:left>
													<plx:cast type="exceptionCast" dataTypeName=".object">
														<plx:nameRef type="parameter" name="oldValue"/>
													</plx:cast>
												</plx:left>
												<plx:right>
													<plx:nullKeyword/>
												</plx:right>
											</plx:binaryOperator>
										</xsl:otherwise>
									</xsl:choose>
								</plx:condition>
								<xsl:if test="@isUnique='true' or @isCustomType='true'">
									<xsl:call-template name="GetImplementationPropertyChangedMethodUpdateOldOppositeObjectCode">
										<xsl:with-param name="ClassName" select="$ClassName"/>
										<xsl:with-param name="OldValue" select="$oldValueNotNullValue"/>
										<xsl:with-param name="AllReverseChildProperties" select="$AllReverseChildProperties"/>
										<xsl:with-param name="ShouldCheckForNull" select="false()"/>
									</xsl:call-template>
								</xsl:if>
								<xsl:for-each select="$compoundUniquenessConstraints">
									<plx:assign>
										<plx:left>
											<plx:nameRef name="{$uniquenessConstraintsDataTypes[@id=current()/@id]/@signature}OldValueTuple"/>
										</plx:left>
										<plx:right>
											<plx:callStatic dataTypeName="Tuple" name="CreateTuple">
												<xsl:for-each select="$uniquenessConstraintsDataTypes[@id=current()/@id]/prop:DataType">
													<plx:passMemberTypeParam>
														<xsl:copy-of select="@*"/>
														<xsl:copy-of select="child::*"/>
													</plx:passMemberTypeParam>
												</xsl:for-each>
												<xsl:for-each select="oial:uniquenessChild">
													<plx:passParam>
														<xsl:choose>
															<xsl:when test="@ref=$propertyChildId">
																<xsl:copy-of select="$oldValueNotNullValue"/>
															</xsl:when>
															<xsl:otherwise>
																<plx:callInstance type="property" name="{$Properties[@childId=current()/@ref]/@name}">
																	<plx:callObject>
																		<plx:nameRef type="parameter" name="instance"/>
																	</plx:callObject>
																</plx:callInstance>
															</xsl:otherwise>
														</xsl:choose>
													</plx:passParam>
												</xsl:for-each>
											</plx:callStatic>
										</plx:right>
									</plx:assign>
								</xsl:for-each>
							</plx:branch>

							<xsl:if test="$compoundUniquenessConstraints">
								<plx:fallbackBranch>
									<xsl:for-each select="$compoundUniquenessConstraints">
										<plx:assign>
											<plx:left>
												<plx:nameRef name="{$uniquenessConstraintsDataTypes[@id=current()/@id]/@signature}OldValueTuple"/>
											</plx:left>
											<plx:right>
												<plx:nullKeyword/>
											</plx:right>
										</plx:assign>
									</xsl:for-each>
								</plx:fallbackBranch>

								<xsl:for-each select="$compoundUniquenessConstraints">
									<!-- TODO: Would calling this only if at least one of the Tuples is not null be better? -->
									<xsl:variable name="cachedDataTypes" select="$uniquenessConstraintsDataTypes[@id=current()/@id]"/>
									<xsl:variable name="signature" select="string($cachedDataTypes/@signature)"/>
									<plx:callThis name="On{$signature}Changed">
										<plx:passParam>
											<plx:nameRef type="parameter" name="instance"/>
										</plx:passParam>
										<plx:passParam>
											<plx:nameRef name="{$signature}OldValueTuple"/>
										</plx:passParam>
										<plx:passParam>
											<plx:callStatic dataTypeName="Tuple" name="CreateTuple">
												<xsl:for-each select="$cachedDataTypes/prop:DataType">
													<plx:passMemberTypeParam>
														<xsl:copy-of select="@*"/>
														<xsl:copy-of select="child::*"/>
													</plx:passMemberTypeParam>
												</xsl:for-each>
												<xsl:for-each select="oial:uniquenessChild">
													<plx:passParam>
														<plx:callInstance type="property" name="{$Properties[@childId=current()/@ref]/@name}">
															<plx:callObject>
																<plx:nameRef type="parameter" name="instance"/>
															</plx:callObject>
														</plx:callInstance>
													</plx:passParam>
												</xsl:for-each>
											</plx:callStatic>
										</plx:passParam>
									</plx:callThis>
								</xsl:for-each>

							</xsl:if>
								
						</plx:function>
					</xsl:if>
				</xsl:if>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="GetImplementationPropertyChangingMethodCheckSameContextCode">
		<xsl:param name="ModelContextName"/>
		<xsl:param name="NewValue"/>
		<xsl:param name="ParamName"/>
		<plx:branch>
			<plx:condition>
				<plx:binaryOperator type="identityInequality">
					<plx:left>
						<plx:cast type="exceptionCast" dataTypeName=".object">
							<plx:thisKeyword/>
						</plx:cast>
					</plx:left>
					<plx:right>
						<plx:cast type="exceptionCast" dataTypeName=".object">
							<plx:callInstance type="property" name="Context">
								<plx:callObject>
									<xsl:copy-of select="$NewValue"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:cast>
					</plx:right>
				</plx:binaryOperator>
			</plx:condition>
			<plx:throw>
				<plx:callStatic name="GetDifferentContextsException" dataTypeName="{$ModelContextName}">
					<xsl:if test="$ParamName">
						<plx:passParam>
							<plx:string data="{$ParamName}"/>
						</plx:passParam>
					</xsl:if>
				</plx:callStatic>
			</plx:throw>
		</plx:branch>
	</xsl:template>

	<!-- Given the childId of a property, find a reverse property with the same id and
	place the name of the reverse child property in the name attribute. -->
	<xsl:template name="GetReversePropertyNameAttribute">
		<xsl:param name="AllReverseChildProperties"/>
		<xsl:if test="@childId">
			<xsl:for-each select="$AllReverseChildProperties[@reverseChildId=current()/@childId]">
				<xsl:attribute name="name">
					<xsl:value-of select="@name"/>
				</xsl:attribute>
			</xsl:for-each>
		</xsl:if>
	</xsl:template>
	
	<xsl:template name="GetImplementationPropertyChangedMethodUpdateOldOppositeObjectCode">
		<xsl:param name="ClassName"/>
		<xsl:param name="OldValue"/>
		<xsl:param name="AllReverseChildProperties"/>
		<xsl:param name="ShouldCheckForNull" select="false()"/>
		<xsl:variable name="updateCodeFragment">
			<xsl:choose>
				<xsl:when test="@isUnique='true' and @isCustomType='true'">
					<plx:assign>
						<plx:left>
							<plx:callInstance type="property" name="{@oppositeName}">
								<xsl:call-template name="GetReversePropertyNameAttribute">
									<xsl:with-param name="AllReverseChildProperties" select="$AllReverseChildProperties"/>
								</xsl:call-template>
								<plx:callObject>
									<xsl:copy-of select="$OldValue"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:left>
						<plx:right>
							<plx:nullKeyword/>
						</plx:right>
					</plx:assign>
				</xsl:when>
				<xsl:when test="@isUnique='true' and not(@isCustomType='true')">
					<plx:callInstance name="Remove">
						<plx:callObject>
							<plx:callThis type="field" name="{$PrivateMemberPrefix}{$ClassName}{@name}Dictionary"/>
						</plx:callObject>
						<plx:passParam>
							<xsl:copy-of select="$OldValue"/>
						</plx:passParam>
					</plx:callInstance>
				</xsl:when>
				<xsl:when test="not(@isUnique='true') and @isCustomType='true'">
					<plx:callInstance name="Remove">
						<plx:callObject>
							<plx:cast type="exceptionCast" dataTypeName="ICollection">
								<plx:passTypeParam dataTypeName="{$ClassName}"/>
								<plx:callInstance type="property" name="{@oppositeName}Via{@name}Collection">
									<xsl:call-template name="GetReversePropertyNameAttribute">
										<xsl:with-param name="AllReverseChildProperties" select="$AllReverseChildProperties"/>
									</xsl:call-template>
									<plx:callObject>
										<xsl:copy-of select="$OldValue"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:cast>
						</plx:callObject>
						<plx:passParam>
							<plx:nameRef type="parameter" name="instance"/>
						</plx:passParam>
					</plx:callInstance>
					<!--<plx:callInstance name="Remove">
						<plx:callObject>
							<plx:callInstance type="property" name="{@oppositeName}">
								<plx:callObject>
									<xsl:copy-of select="$OldValue"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:callObject>
						<plx:passParam>
							<plx:nameRef type="parameter" name="instance"/>
						</plx:passParam>
					</plx:callInstance>-->
				</xsl:when>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="updateCode" select="exsl:node-set($updateCodeFragment)/child::*"/>
		<xsl:choose>
			<xsl:when test="$ShouldCheckForNull">
				<plx:branch>
					<plx:condition>
						<xsl:choose>
							<xsl:when test="prop:DataType/@dataTypeName='Nullable'">
								<plx:callInstance type="property" name="HasValue">
									<plx:callObject>
										<xsl:copy-of select="$OldValue"/>
									</plx:callObject>
								</plx:callInstance>
							</xsl:when>
							<xsl:otherwise>
								<plx:binaryOperator type="identityInequality">
									<plx:left>
										<plx:cast type="exceptionCast" dataTypeName=".object">
											<xsl:copy-of select="$OldValue"/>
										</plx:cast>
									</plx:left>
									<plx:right>
										<plx:nullKeyword/>
									</plx:right>
								</plx:binaryOperator>
							</xsl:otherwise>
						</xsl:choose>
					</plx:condition>
					<xsl:copy-of select="$updateCode"/>
				</plx:branch>
			</xsl:when>
			<xsl:otherwise>
				<xsl:copy-of select="$updateCode"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template name="GetImplementationPropertyChangedMethodUpdateNewOppositeObjectCode">
		<xsl:param name="ClassName"/>
		<xsl:param name="NewValue"/>
		<xsl:param name="AllReverseChildProperties"/>
		<!-- If @mandatory='alethic', NewValue has already been checked by the caller to ensure that it is not null. -->
		<xsl:param name="ShouldCheckForNull" select="not(@mandatory='alethic') and @canBeNull='true'"/>
		<xsl:variable name="updateCodeFragment">
			<xsl:choose>
				<xsl:when test="@isUnique='true' and @isCustomType='true'">
					<plx:assign>
						<plx:left>
							<plx:callInstance type="property" name="{@oppositeName}">
								<xsl:call-template name="GetReversePropertyNameAttribute">
									<xsl:with-param name="AllReverseChildProperties" select="$AllReverseChildProperties"/>
								</xsl:call-template>
								<plx:callObject>
									<xsl:copy-of select="$NewValue"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:left>
						<plx:right>
							<plx:nameRef type="parameter" name="instance"/>
						</plx:right>
					</plx:assign>
				</xsl:when>
				<xsl:when test="@isUnique='true' and not(@isCustomType='true')">
					<plx:callInstance name="Add">
						<plx:callObject>
							<plx:callThis type="field" name="{$PrivateMemberPrefix}{$ClassName}{@name}Dictionary"/>
						</plx:callObject>
						<plx:passParam>
							<xsl:choose>
								<xsl:when test="@canBeNull='true' and prop:DataType/@dataTypeName='Nullable'">
									<plx:callInstance name="GetValueOrDefault">
										<plx:callObject>
											<xsl:copy-of select="$NewValue"/>
										</plx:callObject>
									</plx:callInstance>
								</xsl:when>
								<xsl:otherwise>
									<xsl:copy-of select="$NewValue"/>
								</xsl:otherwise>
							</xsl:choose>
						</plx:passParam>
						<plx:passParam>
							<plx:nameRef type="parameter" name="instance"/>
						</plx:passParam>
					</plx:callInstance>
				</xsl:when>
				<xsl:when test="not(@isUnique='true') and @isCustomType='true'">
					<plx:callInstance name="Add">
						<plx:callObject>
							<plx:cast type="exceptionCast" dataTypeName="ICollection">
								<plx:passTypeParam dataTypeName="{$ClassName}"/>
								<plx:callInstance type="property" name="{@oppositeName}Via{@name}Collection">
									<xsl:call-template name="GetReversePropertyNameAttribute">
										<xsl:with-param name="AllReverseChildProperties" select="$AllReverseChildProperties"/>
									</xsl:call-template>
									<plx:callObject>
										<xsl:copy-of select="$NewValue"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:cast>
						</plx:callObject>
						<plx:passParam>
							<plx:nameRef type="parameter" name="instance"/>
						</plx:passParam>
					</plx:callInstance>
					<!--<plx:callInstance name="Add">
						<plx:callObject>
							<plx:callInstance type="property" name="{@oppositeName}">
								<plx:callObject>
									<xsl:copy-of select="$NewValue"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:callObject>
						<plx:passParam>
							<plx:nameRef type="parameter" name="instance"/>
						</plx:passParam>
					</plx:callInstance>-->
				</xsl:when>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="updateCode" select="exsl:node-set($updateCodeFragment)/child::*"/>
		<xsl:choose>			
			<xsl:when test="$ShouldCheckForNull">
				<plx:branch>
					<plx:condition>
						<xsl:choose>
							<xsl:when test="prop:DataType/@dataTypeName='Nullable'">
								<plx:callInstance type="property" name="HasValue">
									<plx:callObject>
										<xsl:copy-of select="$NewValue"/>
									</plx:callObject>
								</plx:callInstance>
							</xsl:when>
							<xsl:otherwise>
								<plx:binaryOperator type="identityInequality">
									<plx:left>
										<plx:cast type="exceptionCast" dataTypeName=".object">
											<xsl:copy-of select="$NewValue"/>
										</plx:cast>
									</plx:left>
									<plx:right>
										<plx:nullKeyword/>
									</plx:right>
								</plx:binaryOperator>
							</xsl:otherwise>
						</xsl:choose>
					</plx:condition>
					<xsl:copy-of select="$updateCode"/>
				</plx:branch>
			</xsl:when>
			<xsl:otherwise>
				<xsl:copy-of select="$updateCode"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
</xsl:stylesheet>
