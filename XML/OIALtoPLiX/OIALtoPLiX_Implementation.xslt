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
	xmlns:odt="http://schemas.orm.net/ORMDataTypes"
	xmlns:plx="http://schemas.neumont.edu/CodeGeneration/PLiX"
	xmlns:prop="urn:schemas-orm-net:PLiX:CLI:Properties"
	exclude-result-prefixes="oil odt"
	extension-element-prefixes="exsl">

	<xsl:import href="OIALtoPLiX.xslt"/>

	<xsl:output method="xml" encoding="utf-8" media-type="text/xml" indent="yes"/>

	<xsl:template match="oil:model" mode="OIALtoPLiX_Implementation">
		<xsl:param name="Model"/>
		<xsl:param name="ModelContextName"/>
		<xsl:param name="ConceptTypes"/>
		<xsl:param name="InformationTypeFormatMappings"/>
		<xsl:param name="AllProperties"/>

		<plx:class visibility="public" modifier="sealed" name="{$ModelContextName}">
			<plx:leadingInfo>
				<plx:pragma type="region" data="{$ModelContextName}"/>
			</plx:leadingInfo>
			<plx:trailingInfo>
				<plx:pragma type="closeRegion" data="{$ModelContextName}"/>
			</plx:trailingInfo>
			<xsl:copy-of select="$GeneratedCodeAttribute"/>
			<plx:implementsInterface dataTypeName="I{$ModelContextName}"/>

			<plx:function name=".construct"  visibility="public">
				<xsl:for-each select="$ConceptTypes">
					<xsl:variable name="ClassName" select="@name"/>
					<plx:local name="{$ClassName}List" dataTypeName="List">
						<plx:passTypeParam dataTypeName="{$ClassName}"/>
						<plx:initialize>
							<plx:callNew dataTypeName="List">
								<plx:passTypeParam dataTypeName="{$ClassName}"/>
							</plx:callNew>
						</plx:initialize>
					</plx:local>
					<plx:assign>
						<plx:left>
							<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}{$ClassName}List"/>
						</plx:left>
						<plx:right>
							<plx:nameRef type="local" name="{$ClassName}List"/>
						</plx:right>
					</plx:assign>
					<plx:assign>
						<plx:left>
							<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}{$ClassName}ReadOnlyCollection"/>
						</plx:left>
						<plx:right>
							<plx:callNew dataTypeName="ReadOnlyCollection">
								<plx:passTypeParam dataTypeName="{$ClassName}"/>
								<plx:passParam>
									<plx:nameRef type="local" name="{$ClassName}List"/>
								</plx:passParam>
							</plx:callNew>
						</plx:right>
					</plx:assign>
				</xsl:for-each>
			</plx:function>

			<xsl:call-template name="GenerateConstraintEnforcementCollection"/>
			
			<xsl:call-template name="GenerateModelContextMethods">
				<xsl:with-param name="Model" select="."/>
				<xsl:with-param name="ModelContextName" select="$ModelContextName"/>
				<xsl:with-param name="AllProperties" select="$AllProperties"/>
			</xsl:call-template>

			<xsl:for-each select="$ConceptTypes">
				<xsl:apply-templates select="." mode="GenerateImplementationClass">
					<xsl:with-param name="Model" select="$Model"/>
					<xsl:with-param name="ModelContextName" select="$ModelContextName"/>
					<xsl:with-param name="InformationTypeFormatMappings" select="$InformationTypeFormatMappings"/>
					<xsl:with-param name="Properties" select="$AllProperties[@conceptTypeName=current()/@name]/prop:Property"/>
				</xsl:apply-templates>
			</xsl:for-each>

		</plx:class>

	</xsl:template>

	<xsl:template name="GenerateModelContextMethods">
		<xsl:param name="Model"/>
		<xsl:param name="ModelContextName"/>
		<xsl:param name="AllProperties"/>
		<plx:field name="{$PrivateMemberPrefix}IsDeserializing" visibility="private" dataTypeName=".boolean"/>
		<plx:property visibility="{$ModelContextInterfaceImplementationVisibility}" name="IsDeserializing">
			<plx:interfaceMember memberName="IsDeserializing" dataTypeName="I{$ModelContextName}"/>
			<plx:returns dataTypeName=".boolean"/>
			<plx:get>
				<plx:return>
					<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}IsDeserializing"/>
				</plx:return>
			</plx:get>
		</plx:property>
		<xsl:call-template name="GenerateModelContextLookupMethods">
			<xsl:with-param name="Model" select="$Model"/>
			<xsl:with-param name="ModelContextName" select="$ModelContextName"/>
			<xsl:with-param name="AllProperties" select="$AllProperties"/>
		</xsl:call-template>
	</xsl:template>
	<xsl:template name="GenerateModelContextLookupMethods">
		<xsl:param name="Model"/>
		<xsl:param name="ModelContextName"/>
		<xsl:param name="AllProperties"/>
		
		<!-- TODO: This will break for oil:roleSequenceUniquenessConstraint elements that contain oil:typeRef elements with more than one oil:conceptType reference by @targetConceptType. -->
		<xsl:for-each select="$Model//oil:roleSequenceUniquenessConstraint">
			<xsl:variable name="uniqueConceptTypeName" select="parent::oil:conceptType/@name"/>
			<xsl:variable name="parametersFragment">
				<xsl:for-each select="oil:roleSequence/oil:typeRef">
					<plx:param name="{@targetChild}">
						<xsl:variable name="targetProperty" select="$AllProperties[@conceptTypeName=$uniqueConceptTypeName]/prop:Property[@name=current()/@targetChild]"/>
						<xsl:copy-of select="$targetProperty/prop:DataType/@*"/>
						<xsl:copy-of select="$targetProperty/prop:DataType/child::*"/>
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
			
			<plx:field visibility="private" readOnly="true" name="{$PrivateMemberPrefix}{@name}Dictionary" dataTypeName="Dictionary">
				<plx:passTypeParam dataTypeName="Tuple">
					<xsl:copy-of select="$passTypeParams"/>
				</plx:passTypeParam>
				<plx:passTypeParam dataTypeName="{$uniqueConceptTypeName}"/>
				<plx:initialize>
					<plx:callNew dataTypeName="Dictionary">
						<plx:passTypeParam dataTypeName="Tuple">
							<xsl:copy-of select="$passTypeParams"/>
						</plx:passTypeParam>
						<plx:passTypeParam dataTypeName="{$uniqueConceptTypeName}"/>
					</plx:callNew>
				</plx:initialize>
			</plx:field>

			<plx:function visibility="{$ModelContextInterfaceImplementationVisibility}" name="Get{$uniqueConceptTypeName}By{@name}">
				<plx:interfaceMember memberName="Get{$uniqueConceptTypeName}By{@name}" dataTypeName="I{$ModelContextName}"/>
				<xsl:copy-of select="$parameters"/>
				<plx:returns dataTypeName="{$uniqueConceptTypeName}"/>
				<plx:return>
					<plx:callInstance type="indexerCall" name=".implied">
						<plx:callObject>
							<plx:callThis type="field" name="{$PrivateMemberPrefix}{@name}Dictionary"/>
						</plx:callObject>
						<plx:passParam>
							<plx:callStatic type="methodCall" name="CreateTuple" dataTypeName="Tuple">
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
						</plx:passParam>
					</plx:callInstance>
				</plx:return>
			</plx:function>

			<plx:function visibility="private" name="On{@name}Changing">
				<plx:param name="instance" dataTypeName="{$uniqueConceptTypeName}"/>
				<plx:param name="newValue" dataTypeName="Tuple">
					<xsl:copy-of select="$passTypeParams"/>
				</plx:param>
				<plx:returns dataTypeName=".boolean"/>
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="identityInequality">
							<plx:left>
								<plx:nameRef type="parameter" name="newValue"/>
							</plx:left>
							<plx:right>
								<plx:nullKeyword/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:local name="currentInstance" dataTypeName="{$uniqueConceptTypeName}">
						<plx:initialize>
							<plx:nameRef type="parameter" name="instance"/>
						</plx:initialize>
					</plx:local>
					<plx:branch>
						<plx:condition>
							<plx:callInstance name="TryGetValue">
								<plx:callObject>
									<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}{@name}Dictionary"/>
								</plx:callObject>
								<plx:passParam>
									<plx:nameRef type="parameter" name="newValue"/>
								</plx:passParam>
								<plx:passParam type="out">
									<plx:nameRef type="local" name="currentInstance"/>
								</plx:passParam>
							</plx:callInstance>
						</plx:condition>
						<plx:return>
							<plx:binaryOperator type="identityEquality">
								<plx:left>
									<plx:nameRef name="currentInstance"/>
								</plx:left>
								<plx:right>
									<plx:nameRef type="parameter" name="instance"/>
								</plx:right>
							</plx:binaryOperator>
						</plx:return>
					</plx:branch>
				</plx:branch>
				<plx:return>
					<plx:trueKeyword/>
				</plx:return>
			</plx:function>

			<plx:function visibility="private" name="On{@name}Changed">
				<plx:param name="instance" dataTypeName="{$uniqueConceptTypeName}" />
				<plx:param name="oldValue" dataTypeName="Tuple">
					<xsl:copy-of select="$passTypeParams"/>
				</plx:param>
				<plx:param name="newValue" dataTypeName="Tuple">
					<xsl:copy-of select="$passTypeParams"/>
				</plx:param>
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="identityInequality">
							<plx:left>
								<plx:nameRef type="parameter" name="oldValue"/>
							</plx:left>
							<plx:right>
								<plx:nullKeyword/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:callInstance name="Remove">
						<plx:callObject>
							<plx:callThis name="{$PrivateMemberPrefix}{@name}Dictionary" type="field"/>
						</plx:callObject>
						<plx:passParam>
							<plx:nameRef type="parameter" name="oldValue"/>
						</plx:passParam>
					</plx:callInstance>
				</plx:branch>
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="identityInequality">
							<plx:left>
								<plx:nameRef type="parameter" name="newValue"/>
							</plx:left>
							<plx:right>
								<plx:nullKeyword/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:callInstance name="Add">
						<plx:callObject>
							<plx:callThis name="{$PrivateMemberPrefix}{@name}Dictionary" type="field"/>
						</plx:callObject>
						<plx:passParam>
							<plx:nameRef type="parameter" name="newValue"/>
						</plx:passParam>
						<plx:passParam>
							<plx:nameRef type="parameter" name="instance"/>
						</plx:passParam>
					</plx:callInstance>
				</plx:branch>
			</plx:function>
				
		</xsl:for-each>

		<xsl:for-each select="$AllProperties/prop:Property[@isUnique='true' and not(@isCustomType='true')]">
			<!-- The 'On<Property>Changing' and 'On<Property>Changed' methods are generated later in another transform. -->
			<xsl:variable name="uniqueConceptTypeName" select="parent::prop:Properties/@conceptTypeName"/>

			<plx:field visibility="private" readOnly="true" name="{$PrivateMemberPrefix}{$uniqueConceptTypeName}{@name}Dictionary" dataTypeName="Dictionary">
				<plx:passTypeParam>
					<xsl:copy-of select="prop:DataType/@*"/>
					<xsl:copy-of select="prop:DataType/child::*"/>
				</plx:passTypeParam>
				<plx:passTypeParam dataTypeName="{$uniqueConceptTypeName}"/>
				<plx:initialize>
					<plx:callNew dataTypeName="Dictionary">
						<plx:passTypeParam>
							<xsl:copy-of select="prop:DataType/@*"/>
							<xsl:copy-of select="prop:DataType/child::*"/>
						</plx:passTypeParam>
						<plx:passTypeParam dataTypeName="{$uniqueConceptTypeName}"/>
					</plx:callNew>
				</plx:initialize>
			</plx:field>
			
			<!-- TODO: In Get{Thing}By{Name}, {Name} should be oil:singleRoleUniquenessConstraint/@name rather than prop:Property/@name. -->
			<plx:function visibility="{$ModelContextInterfaceImplementationVisibility}" name="Get{$uniqueConceptTypeName}By{@name}">
				<plx:interfaceMember memberName="Get{$uniqueConceptTypeName}By{@name}" dataTypeName="I{$ModelContextName}"/>
				<plx:param name="{@name}">
					<xsl:copy-of select="prop:DataType/@*"/>
					<xsl:copy-of select="prop:DataType/child::*"/>
				</plx:param>
				<plx:returns dataTypeName="{$uniqueConceptTypeName}"/>
				<plx:return>
					<plx:callInstance type="indexerCall" name=".implied">
						<plx:callObject>
							<plx:callThis type="field" name="{$PrivateMemberPrefix}{$uniqueConceptTypeName}{@name}Dictionary"/>
						</plx:callObject>
						<plx:passParam>
							<plx:nameRef type="parameter" name="{@name}"/>
						</plx:passParam>
					</plx:callInstance>
				</plx:return>
			</plx:function>

		</xsl:for-each>
		
	</xsl:template>


	<xsl:template match="oil:conceptType" mode="GenerateImplementationClass">
		<xsl:param name="Model"/>
		<xsl:param name="ModelContextName"/>
		<xsl:param name="InformationTypeFormatMappings"/>
		<xsl:param name="Properties"/>
		<xsl:variable name="ClassName" select="@name"/>
		<xsl:variable name="ImplementationClassName" select="concat($ClassName,$ImplementationClassSuffix)"/>
		<xsl:variable name="mandatoryParametersFragment">
			<xsl:call-template name="GenerateMandatoryParameters">
				<xsl:with-param name="Properties" select="$Properties"/>
			</xsl:call-template>
		</xsl:variable>
		<xsl:variable name="mandatoryParameters" select="exsl:node-set($mandatoryParametersFragment)/child::*"/>

		<xsl:variable name="allRoleSequenceUniquenessConstraints" select="$Model//oil:roleSequenceUniquenessConstraint"/>
		
		<xsl:apply-templates select="$Properties" mode="GenerateImplementationPropertyChangeMethods">
			<xsl:with-param name="Model" select="$Model"/>
			<xsl:with-param name="ClassName" select="$ClassName"/>
			<xsl:with-param name="Properties" select="$Properties"/>
			<xsl:with-param name="AllRoleSequenceUniquenessConstraints" select="$allRoleSequenceUniquenessConstraints"/>
		</xsl:apply-templates>

		<plx:function visibility="public" name="Create{$ClassName}">
			<xsl:copy-of select="$mandatoryParameters"/>
			<plx:returns dataTypeName="{$ClassName}"/>
			<plx:branch>
				<plx:condition>
					<plx:unaryOperator type="booleanNot">
						<plx:callThis accessor="this" type="property" name="IsDeserializing"/>
					</plx:unaryOperator>
				</plx:condition>
				<xsl:for-each select="$mandatoryParameters">
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
					<plx:branch>
						<plx:condition>
							<plx:unaryOperator type="booleanNot">
								<plx:callThis accessor="this" type="methodCall" name="On{$ClassName}{@name}Changing">
									<plx:passParam>
										<plx:nullKeyword/>
									</plx:passParam>
									<plx:passParam>
										<plx:nameRef type="parameter" name="{@name}"/>
									</plx:passParam>
								</plx:callThis>
							</plx:unaryOperator>
						</plx:condition>
						<plx:throw>
							<plx:callNew dataTypeName="ArgumentException">
								<plx:passParam>
									<plx:string>Argument failed constraint enforcement.</plx:string>
								</plx:passParam>
								<plx:passParam>
									<plx:string>
										<xsl:value-of select="@name"/>
									</plx:string>
								</plx:passParam>
							</plx:callNew>
						</plx:throw>
					</plx:branch>
				</xsl:for-each>
			</plx:branch>
			<plx:return>
				<plx:callNew dataTypeName="{$ImplementationClassName}">
					<plx:passParam>
						<plx:thisKeyword/>
					</plx:passParam>
					<xsl:for-each select="$mandatoryParameters">
						<plx:passParam>
							<plx:nameRef type="parameter" name="{@name}"/>
						</plx:passParam>
					</xsl:for-each>
				</plx:callNew>
			</plx:return>
		</plx:function>

		<plx:field visibility="private" readOnly="true" name="{$PrivateMemberPrefix}{$ClassName}List" dataTypeName="List">
			<plx:passTypeParam dataTypeName="{$ClassName}"/>
		</plx:field>
		<plx:field visibility="private" readOnly="true" name="{$PrivateMemberPrefix}{$ClassName}ReadOnlyCollection" dataTypeName="ReadOnlyCollection">
			<plx:passTypeParam dataTypeName="{$ClassName}"/>
		</plx:field>
		<plx:property visibility="{$ModelContextInterfaceImplementationVisibility}" name="{$ClassName}Collection">
			<plx:interfaceMember memberName="{$ClassName}Collection" dataTypeName="I{$ModelContextName}"/>
			<plx:returns dataTypeName="ReadOnlyCollection">
				<plx:passTypeParam dataTypeName="{$ClassName}"/>
			</plx:returns>
			<plx:get>
				<plx:return>
					<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}{$ClassName}ReadOnlyCollection"/>
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
			<plx:derivesFromClass dataTypeName="{$ClassName}"/>
			<plx:function visibility="public" name=".construct">
				<plx:param name="context" dataTypeName="{$ModelContextName}"/>
				<xsl:copy-of select="$mandatoryParameters"/>
				<plx:assign>
					<plx:left>
						<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}Context"/>
					</plx:left>
					<plx:right>
						<plx:nameRef type="parameter" name="context"/>
					</plx:right>
				</plx:assign>
				<xsl:apply-templates select="$Properties" mode="GenerateImplementationPropertyFieldInitializer">
					<xsl:with-param name="ClassName" select="$ClassName"/>
				</xsl:apply-templates>
				<xsl:variable name="mandatoryProperties" select="$Properties[@name=$mandatoryParameters/@name]"/>
				<xsl:for-each select="$mandatoryProperties">
					<plx:assign>
						<plx:left>
							<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}{@name}"/>
						</plx:left>
						<plx:right>
							<plx:nameRef type="parameter" name="{@name}"/>
						</plx:right>
					</plx:assign>
					<xsl:variable name="roleSequenceUniquenessConstraints" select="$allRoleSequenceUniquenessConstraints[oil:roleSequence/oil:typeRef[@targetConceptType=$ClassName and @targetChild=current()/@name]]"/>
					<xsl:if test="@isUnique='true' or @isCustomType='true' or $roleSequenceUniquenessConstraints">
						<plx:callInstance type="methodCall" name="On{$ClassName}{@name}Changed">
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
				<plx:callInstance type="methodCall" name="Add">
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
				<xsl:with-param name="AllRoleSequenceUniquenessConstraints" select="$allRoleSequenceUniquenessConstraints"/>
			</xsl:apply-templates>
		</plx:class>
	</xsl:template>

	<xsl:template match="prop:Property[@isCollection='true']" mode="GenerateImplementationPropertyFieldInitializer">
		<xsl:param name="ClassName"/>
		<plx:assign>
			<plx:left>
				<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}{@name}"/>
			</plx:left>
			<plx:right>
				<plx:callNew dataTypeName="ConstraintEnforcementCollection">
					<plx:passTypeParam dataTypeName="{$ClassName}"/>
					<xsl:copy-of select="prop:DataType/child::*"/>
					<plx:passParam>
						<plx:thisKeyword/>
					</plx:passParam>
					<plx:passParam>
						<plx:callNew type="newDelegate" dataTypeName="PotentialCollectionModificationCallback">
							<plx:passTypeParam dataTypeName="{$ClassName}"/>
							<xsl:copy-of select="prop:DataType/child::*"/>
							<plx:passParam>
								<plx:callInstance type="methodReference" name="On{$ClassName}{@name}Adding">
									<plx:callObject>
										<plx:nameRef type="parameter" name="context"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:passParam>
						</plx:callNew>
					</plx:passParam>
					<plx:passParam>
						<plx:callNew type="newDelegate" dataTypeName="CommittedCollectionModificationCallback">
							<plx:passTypeParam dataTypeName="{$ClassName}"/>
							<xsl:copy-of select="prop:DataType/child::*"/>
							<plx:passParam>
								<plx:callInstance type="methodReference" name="On{$ClassName}{@name}Added">
									<plx:callObject>
										<plx:nameRef type="parameter" name="context"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:passParam>
						</plx:callNew>
					</plx:passParam>
					<plx:passParam>
						<plx:callNew type="newDelegate" dataTypeName="PotentialCollectionModificationCallback">
							<plx:passTypeParam dataTypeName="{$ClassName}"/>
							<xsl:copy-of select="prop:DataType/child::*"/>
							<plx:passParam>
								<plx:callInstance type="methodReference" name="On{$ClassName}{@name}Removing">
									<plx:callObject>
										<plx:nameRef type="parameter" name="context"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:passParam>
						</plx:callNew>
					</plx:passParam>
					<plx:passParam>
						<plx:callNew type="newDelegate" dataTypeName="CommittedCollectionModificationCallback">
							<plx:passTypeParam dataTypeName="{$ClassName}"/>
							<xsl:copy-of select="prop:DataType/child::*"/>
							<plx:passParam>
								<plx:callInstance type="methodReference" name="On{$ClassName}{@name}Removed">
									<plx:callObject>
										<plx:nameRef type="parameter" name="context"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:passParam>
						</plx:callNew>
					</plx:passParam>
				</plx:callNew>
			</plx:right>
		</plx:assign>
	</xsl:template>
	
	<xsl:template match="prop:Property" mode="GenerateImplementationProperty">
		<xsl:param name="ClassName"/>
		<xsl:param name="AllRoleSequenceUniquenessConstraints"/>
		<plx:field visibility="private" readOnly="{@isCollection}" name="{$PrivateMemberPrefix}{@name}">
			<xsl:copy-of select="prop:DataType/@*"/>
			<xsl:copy-of select="prop:DataType/child::*"/>
		</plx:field>
		<plx:property visibility="public" modifier="override" name="{@name}">
			<plx:returns>
				<xsl:copy-of select="prop:DataType/@*"/>
				<xsl:copy-of select="prop:DataType/child::*"/>
			</plx:returns>
			<plx:get>
				<plx:return>
					<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}{@name}"/>
				</plx:return>
			</plx:get>
			<xsl:if test="not(@isCollection='true')">
				<plx:set>
					<xsl:if test="@mandatory='alethic' and @canBeNull='true'">
						<plx:branch>
							<plx:condition>
								<plx:binaryOperator type="identityEquality">
									<plx:left>
										<plx:valueKeyword/>
									</plx:left>
									<plx:right>
										<plx:nullKeyword/>
									</plx:right>
								</plx:binaryOperator>
							</plx:condition>
							<plx:return/>
						</plx:branch>
					</xsl:if>
					<plx:branch>
						<plx:condition>
							<plx:unaryOperator type="booleanNot">
								<plx:callStatic type="methodCall" name="Equals" dataTypeName=".object">
									<plx:passParam>
										<plx:callThis accessor="this" type="property" name="{@name}"/>
									</plx:passParam>
									<plx:passParam>
										<plx:valueKeyword/>
									</plx:passParam>
								</plx:callStatic>
							</plx:unaryOperator>
						</plx:condition>
						<plx:branch>
							<plx:condition>
								<plx:callInstance type="methodCall" name="On{$ClassName}{@name}Changing">
									<plx:callObject>
										<plx:callThis accessor="this" type="property" name="Context"/>
									</plx:callObject>
									<plx:passParam>
										<plx:thisKeyword />
									</plx:passParam>
									<plx:passParam>
										<plx:valueKeyword />
									</plx:passParam>
								</plx:callInstance>
							</plx:condition>
							<plx:branch>
								<plx:condition>
									<plx:callThis accessor="base" type="methodCall" name="Raise{@name}ChangingEvent">
										<plx:passParam>
											<plx:valueKeyword/>
										</plx:passParam>
									</plx:callThis>
								</plx:condition>
								<plx:local name="oldValue">
									<xsl:copy-of select="prop:DataType/@*"/>
									<xsl:copy-of select="prop:DataType/child::*"/>
									<plx:initialize>
										<plx:callThis accessor="this" type="property" name="{@name}"/>
									</plx:initialize>
								</plx:local>
								<plx:assign>
									<plx:left>
										<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}{@name}"/>
									</plx:left>
									<plx:right>
										<plx:valueKeyword/>
									</plx:right>
								</plx:assign>
								<xsl:if test="@isUnique='true' or @isCustomType='true' or $AllRoleSequenceUniquenessConstraints[oil:roleSequence/oil:typeRef[@targetConceptType=$ClassName and @targetChild=current()/@name]]">
									<plx:callInstance name="On{$ClassName}{@name}Changed">
										<plx:callObject>
											<plx:callThis accessor="this" type="property" name="Context"/>
										</plx:callObject>
										<plx:passParam>
											<plx:thisKeyword/>
										</plx:passParam>
										<plx:passParam>
											<plx:nameRef type="local" name="oldValue"/>
										</plx:passParam>
									</plx:callInstance>
								</xsl:if>
									<plx:callThis accessor="base" type="methodCall" name="Raise{@name}ChangedEvent">
										<plx:passParam>
											<plx:nameRef type="local" name="oldValue"/>
										</plx:passParam>
									</plx:callThis>
							</plx:branch>
						</plx:branch>
					</plx:branch>
				</plx:set>
			</xsl:if>
		</plx:property>
	</xsl:template>

	<xsl:template match="prop:Property" mode="GenerateImplementationPropertyChangeMethods">
		<xsl:param name="Model"/>
		<xsl:param name="ClassName"/>
		<xsl:param name="Properties"/>
		<xsl:param name="AllRoleSequenceUniquenessConstraints"/>
		<xsl:variable name="propertyName" select="@name"/>
		<xsl:choose>
			<xsl:when test="@isCollection='true'">
				<xsl:variable name="valueFragment">
					<plx:nameRef type="parameter" name="value"/>
				</xsl:variable>
				<xsl:variable name="value" select="exsl:node-set($valueFragment)/child::*"/>
				<plx:function visibility="private" name="On{$ClassName}{@name}Adding">
					<plx:param name="instance" dataTypeName="{$ClassName}"/>
					<plx:param name="value">
						<xsl:copy-of select="prop:DataType/plx:passTypeParam/@*"/>
						<xsl:copy-of select="prop:DataType/plx:passTypeParam/child::*"/>
					</plx:param>
					<plx:returns dataTypeName=".boolean"/>
					<plx:return>
						<plx:trueKeyword/>
					</plx:return>
				</plx:function>
				<plx:function visibility="private" name="On{$ClassName}{@name}Added">
					<plx:param name="instance" dataTypeName="{$ClassName}"/>
					<plx:param name="value">
						<xsl:copy-of select="prop:DataType/plx:passTypeParam/@*"/>
						<xsl:copy-of select="prop:DataType/plx:passTypeParam/child::*"/>
					</plx:param>
					<xsl:if test="@isUnique='true' or @isCustomType='true'">
						<xsl:call-template name="GetImplementationPropertyChangedMethodUpdateNewOppositeObjectCode">
							<xsl:with-param name="ClassName" select="$ClassName"/>
							<xsl:with-param name="NewValue" select="$value"/>
						</xsl:call-template>
					</xsl:if>
				</plx:function>
				<plx:function visibility="private" name="On{$ClassName}{@name}Removing">
					<plx:param name="instance" dataTypeName="{$ClassName}"/>
					<plx:param name="value">
						<xsl:copy-of select="prop:DataType/plx:passTypeParam/@*"/>
						<xsl:copy-of select="prop:DataType/plx:passTypeParam/child::*"/>
					</plx:param>
					<plx:returns dataTypeName=".boolean"/>
					<plx:return>
						<plx:trueKeyword/>
					</plx:return>
				</plx:function>
				<plx:function visibility="private" name="On{$ClassName}{@name}Removed">
					<plx:param name="instance" dataTypeName="{$ClassName}"/>
					<plx:param name="value">
						<xsl:copy-of select="prop:DataType/plx:passTypeParam/@*"/>
						<xsl:copy-of select="prop:DataType/plx:passTypeParam/child::*"/>
					</plx:param>
					<xsl:if test="@isUnique='true' or @isCustomType='true'">
						<xsl:call-template name="GetImplementationPropertyChangedMethodUpdateOldOppositeObjectCode">
							<xsl:with-param name="ClassName" select="$ClassName"/>
							<xsl:with-param name="OldValue" select="$value"/>
							<xsl:with-param name="ShouldCheckForNull" select="true()"/>
						</xsl:call-template>
					</xsl:if>
				</plx:function>
			</xsl:when>
			<xsl:otherwise>
				<xsl:variable name="roleSequenceUniquenessConstraints" select="$AllRoleSequenceUniquenessConstraints[oil:roleSequence/oil:typeRef[@targetConceptType=$ClassName and @targetChild=$propertyName]]"/>

				<plx:function visibility="private" name="On{$ClassName}{@name}Changing">
					<xsl:if test="@isCustomType='true'">
						<xsl:call-template name="GenerateSuppressMessageAttribute">
							<xsl:with-param name="category" select="'Microsoft.Usage'"/>
							<xsl:with-param name="checkId" select="'CA2208'"/>
						</xsl:call-template>
						<xsl:call-template name="GenerateSuppressMessageAttribute">
							<xsl:with-param name="category" select="'Microsoft.Globalization'"/>
							<xsl:with-param name="checkId" select="'CA1303'"/>
						</xsl:call-template>
					</xsl:if>
					<plx:param name="instance" dataTypeName="{$ClassName}"/>
					<plx:param name="newValue">
						<xsl:copy-of select="prop:DataType/@*"/>
						<xsl:copy-of select="prop:DataType/child::*"/>
					</plx:param>
					<plx:returns dataTypeName=".boolean"/>
					
					<xsl:if test="@isCustomType='true' or (@isUnique='true' and not(@isCustomType='true'))">
						<xsl:variable name="validationCodeFragment">
							<xsl:choose>
								<xsl:when test="@isCustomType='true'">
									<plx:branch>
										<plx:condition>
											<plx:binaryOperator type="identityInequality">
												<plx:left>
													<plx:thisKeyword/>
												</plx:left>
												<plx:right>
													<plx:callInstance type="property" name="Context">
														<plx:callObject>
															<plx:nameRef type="parameter" name="newValue"/>
														</plx:callObject>
													</plx:callInstance>
												</plx:right>
											</plx:binaryOperator>
										</plx:condition>
										<plx:throw>
											<plx:callNew dataTypeName="ArgumentException">
												<plx:passParam>
													<plx:string>All objects in a relationship must be part of the same Context.</plx:string>
												</plx:passParam>
												<plx:passParam>
													<plx:string>value</plx:string>
												</plx:passParam>
											</plx:callNew>
										</plx:throw>
									</plx:branch>
								</xsl:when>
								<xsl:otherwise>
									<plx:local name="currentInstance" dataTypeName="{$ClassName}">
										<plx:initialize>
											<plx:nameRef type="parameter" name="instance"/>
										</plx:initialize>
									</plx:local>
									<plx:branch>
										<plx:condition>
											<plx:callInstance name="TryGetValue">
												<plx:callObject>
													<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}{$ClassName}{@name}Dictionary"/>
												</plx:callObject>
												<plx:passParam>
													<plx:nameRef type="parameter" name="newValue"/>
												</plx:passParam>
												<plx:passParam type="out">
													<plx:nameRef name="currentInstance"/>
												</plx:passParam>
											</plx:callInstance>
										</plx:condition>
										<plx:branch>
											<plx:condition>
												<plx:unaryOperator type="booleanNot">
													<plx:callStatic name="Equals" dataTypeName=".object">
														<plx:passParam>
															<plx:nameRef name="currentInstance"/>
														</plx:passParam>
														<plx:passParam>
															<plx:nameRef type="parameter" name="instance"/>
														</plx:passParam>
													</plx:callStatic>
												</plx:unaryOperator>
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
										<plx:binaryOperator type="identityInequality">
											<plx:left>
												<plx:nameRef type="parameter" name="newValue"/>
											</plx:left>
											<plx:right>
												<plx:nullKeyword/>
											</plx:right>
										</plx:binaryOperator>
									</plx:condition>
									<xsl:copy-of select="$validationCode"/>
								</plx:branch>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:if>

					<xsl:if test="$roleSequenceUniquenessConstraints">
						<plx:branch>
							<plx:condition>
								<plx:binaryOperator type="identityInequality">
									<plx:left>
										<plx:nameRef type="parameter" name="instance"/>
									</plx:left>
									<plx:right>
										<plx:nullKeyword/>
									</plx:right>
								</plx:binaryOperator>
							</plx:condition>
							<xsl:for-each select="$roleSequenceUniquenessConstraints">
								<plx:branch>
									<plx:condition>
										<plx:unaryOperator type="booleanNot">
											<plx:callThis accessor="this" type="methodCall" name="On{@name}Changing">
												<plx:passParam>
													<plx:nameRef type="parameter" name="instance"/>
												</plx:passParam>
												<plx:passParam>
													<plx:callStatic type="methodCall" name="CreateTuple" dataTypeName="Tuple">
														<xsl:for-each select="oil:roleSequence/oil:typeRef">
															<plx:passMemberTypeParam>
																<xsl:variable name="dataType" select="$Properties[@name=current()/@targetChild]/prop:DataType"/>
																<xsl:copy-of select="$dataType/@*"/>
																<xsl:copy-of select="$dataType/child::*"/>
															</plx:passMemberTypeParam>
														</xsl:for-each>
														<xsl:for-each select="oil:roleSequence/oil:typeRef">
															<plx:passParam>
																<xsl:choose>
																	<xsl:when test="@targetChild=$propertyName">
																		<plx:nameRef type="parameter" name="newValue"/>
																	</xsl:when>
																	<xsl:otherwise>
																		<plx:callInstance type="property" name="{@targetChild}">
																			<plx:callObject>
																				<plx:nameRef type="parameter" name="instance"/>
																			</plx:callObject>
																		</plx:callInstance>
																	</xsl:otherwise>
																</xsl:choose>
															</plx:passParam>
														</xsl:for-each>
													</plx:callStatic>
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

				<xsl:if test="@isUnique='true' or @isCustomType='true' or $roleSequenceUniquenessConstraints">
					<plx:function visibility="private" overload="true" name="On{$ClassName}{$propertyName}Changed">
						<plx:param name="instance" dataTypeName="{$ClassName}"/>
						<plx:param name="oldValue">
							<xsl:choose>
								<xsl:when test="not(@canBeNull='true')">
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
								<xsl:with-param name="NewValue" select="exsl:node-set($newValueFragment)/child::*"/>
							</xsl:call-template>
						</xsl:if>

						<xsl:variable name="oldValueNotNullValueFragment">
							<xsl:choose>
								<xsl:when test="not(@canBeNull='true')">
									<plx:callInstance type="property" name="Value">
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

						<xsl:variable name="roleSequenceUniquenessConstraintsDataTypesFragment">
							<xsl:for-each select="$roleSequenceUniquenessConstraints">
								<RoleSequenceUniquenessConstraint name="{@name}">
									<xsl:for-each select="oil:roleSequence/oil:typeRef">
										<xsl:copy-of select="$Properties[@name=current()/@targetChild]/prop:DataType"/>
									</xsl:for-each>
								</RoleSequenceUniquenessConstraint>
							</xsl:for-each>
						</xsl:variable>
						<xsl:variable name="roleSequenceUniquenessConstraintsDataTypes" select="exsl:node-set($roleSequenceUniquenessConstraintsDataTypesFragment)/child::*"/>

						<xsl:for-each select="$roleSequenceUniquenessConstraints">
							<plx:local name="{@name}OldValueTuple" dataTypeName="Tuple">
								<xsl:for-each select="$roleSequenceUniquenessConstraintsDataTypes[@name=current()/@name]/prop:DataType">
									<plx:passTypeParam>
										<xsl:copy-of select="@*"/>
										<xsl:copy-of select="child::*"/>
									</plx:passTypeParam>
								</xsl:for-each>
							</plx:local>
						</xsl:for-each>

						<plx:branch>
							<plx:condition>
								<plx:binaryOperator type="identityInequality">
									<plx:left>
										<plx:nameRef type="parameter" name="oldValue"/>
									</plx:left>
									<plx:right>
										<plx:nullKeyword/>
									</plx:right>
								</plx:binaryOperator>
							</plx:condition>
							<xsl:if test="@isUnique='true' or @isCustomType='true'">
								<xsl:call-template name="GetImplementationPropertyChangedMethodUpdateOldOppositeObjectCode">
									<xsl:with-param name="ClassName" select="$ClassName"/>
									<xsl:with-param name="OldValue" select="$oldValueNotNullValue"/>
								</xsl:call-template>
							</xsl:if>
							<xsl:for-each select="$roleSequenceUniquenessConstraints">
								<plx:assign>
									<plx:left>
										<plx:nameRef type="local" name="{@name}OldValueTuple"/>
									</plx:left>
									<plx:right>
										<plx:callStatic dataTypeName="Tuple" name="CreateTuple">
											<xsl:for-each select="$roleSequenceUniquenessConstraintsDataTypes[@name=current()/@name]/prop:DataType">
												<plx:passMemberTypeParam>
													<xsl:copy-of select="@*"/>
													<xsl:copy-of select="child::*"/>
												</plx:passMemberTypeParam>
											</xsl:for-each>
											<xsl:for-each select="oil:roleSequence/oil:typeRef">
												<plx:passParam>
													<xsl:choose>
														<xsl:when test="@targetChild=$propertyName">
															<xsl:copy-of select="$oldValueNotNullValue"/>
														</xsl:when>
														<xsl:otherwise>
															<plx:callInstance type="property" name="{@targetChild}">
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
						<plx:fallbackBranch>
							<xsl:for-each select="$roleSequenceUniquenessConstraints">
								<plx:assign>
									<plx:left>
										<plx:nameRef type="local" name="{@name}OldValueTuple"/>
									</plx:left>
									<plx:right>
										<plx:nullKeyword/>
									</plx:right>
								</plx:assign>
							</xsl:for-each>
						</plx:fallbackBranch>

						<xsl:for-each select="$roleSequenceUniquenessConstraints">
							<plx:callThis accessor="this" type="methodCall" name="On{@name}Changed">
								<plx:passParam>
									<plx:nameRef type="parameter" name="instance"/>
								</plx:passParam>
								<plx:passParam>
									<plx:nameRef type="local" name="{@name}OldValueTuple"/>
								</plx:passParam>
								<plx:passParam>
									<plx:callStatic dataTypeName="Tuple" name="CreateTuple">
										<xsl:for-each select="$roleSequenceUniquenessConstraintsDataTypes[@name=current()/@name]/prop:DataType">
											<plx:passMemberTypeParam>
												<xsl:copy-of select="@*"/>
												<xsl:copy-of select="child::*"/>
											</plx:passMemberTypeParam>
										</xsl:for-each>
										<xsl:for-each select="oil:roleSequence/oil:typeRef">
											<plx:passParam>
												<plx:callInstance type="property" name="{@targetChild}">
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
						
					</plx:function>
				</xsl:if>

			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="GetImplementationPropertyChangedMethodUpdateOldOppositeObjectCode">
		<xsl:param name="ClassName"/>
		<xsl:param name="OldValue"/>
		<xsl:param name="ShouldCheckForNull" select="false()"/>
		<xsl:variable name="updateCodeFragment">
			<xsl:choose>
				<xsl:when test="@isUnique='true' and @isCustomType='true'">
					<plx:assign>
						<plx:left>
							<plx:callInstance type="property" name="{@oppositeName}">
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
					<plx:callInstance type="methodCall" name="Remove">
						<plx:callObject>
							<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}{$ClassName}{@name}Dictionary"/>
						</plx:callObject>
						<plx:passParam>
							<xsl:copy-of select="$OldValue"/>
						</plx:passParam>
					</plx:callInstance>
				</xsl:when>
				<xsl:when test="not(@isUnique='true') and @isCustomType='true'">
					<plx:callInstance type="methodCall" name="Remove">
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
					</plx:callInstance>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="updateCode" select="exsl:node-set($updateCodeFragment)/child::*"/>
		<xsl:choose>
			<xsl:when test="$ShouldCheckForNull and @canBeNull='true'">
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="identityInequality">
							<plx:left>
								<xsl:copy-of select="$OldValue"/>
							</plx:left>
							<plx:right>
								<plx:nullKeyword/>
							</plx:right>
						</plx:binaryOperator>
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
		<xsl:variable name="updateCodeFragment">
			<xsl:choose>
				<xsl:when test="@isUnique='true' and @isCustomType='true'">
					<plx:assign>
						<plx:left>
							<plx:callInstance type="property" name="{@oppositeName}">
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
					<plx:callInstance type="methodCall" name="Add">
						<plx:callObject>
							<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}{$ClassName}{@name}Dictionary"/>
						</plx:callObject>
						<plx:passParam>
							<xsl:copy-of select="$NewValue"/>
						</plx:passParam>
						<plx:passParam>
							<plx:nameRef type="parameter" name="instance"/>
						</plx:passParam>
					</plx:callInstance>
				</xsl:when>
				<xsl:when test="not(@isUnique='true') and @isCustomType='true'">
					<plx:callInstance type="methodCall" name="Add">
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
					</plx:callInstance>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="updateCode" select="exsl:node-set($updateCodeFragment)/child::*"/>
		<xsl:choose>
			<!-- If @mandatory='alethic', NewValue has already been checked by the caller to ensure that it is not null. -->
			<xsl:when test="@mandatory='alethic' or @canBeNull='false'">
				<xsl:copy-of select="$updateCode"/>
			</xsl:when>
			<xsl:otherwise>
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="identityInequality">
							<plx:left>
								<xsl:copy-of select="$NewValue"/>
							</plx:left>
							<plx:right>
								<plx:nullKeyword/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<xsl:copy-of select="$updateCode"/>
				</plx:branch>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="GenerateConstraintEnforcementCollection">
		<plx:delegate visibility="private" name="PotentialCollectionModificationCallback">
			<plx:leadingInfo>
				<plx:pragma type="region" data="ConstraintEnforcementCollection"/>
			</plx:leadingInfo>
			<plx:typeParam requireReferenceType="true" name="TClass"/>
			<plx:typeParam name="TProperty"/>
			<plx:param name="instance" dataTypeName="TClass"/>
			<plx:param name="value" dataTypeName="TProperty"/>
			<plx:returns dataTypeName=".boolean"/>
		</plx:delegate>
		<plx:delegate visibility="private" name="CommittedCollectionModificationCallback">
			<plx:typeParam requireReferenceType="true" name="TClass"/>
			<plx:typeParam name="TProperty"/>
			<plx:param name="instance" dataTypeName="TClass"/>
			<plx:param name="value" dataTypeName="TProperty"/>
		</plx:delegate>
		<plx:class visibility="private" modifier="sealed" name="ConstraintEnforcementCollection">
			<plx:trailingInfo>
				<plx:pragma type="closeRegion" data="ConstraintEnforcementCollection"/>
			</plx:trailingInfo>
			<plx:typeParam requireReferenceType="true" name="TClass"/>
			<plx:typeParam name="TProperty"/>
			<plx:implementsInterface dataTypeName="ICollection">
				<plx:passTypeParam dataTypeName="TProperty"/>
			</plx:implementsInterface>

			<plx:field visibility="private" readOnly="true" name="{$PrivateMemberPrefix}Instance" dataTypeName="TClass"/>
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
				<plx:param name="adding" dataTypeName="PotentialCollectionModificationCallback">
					<plx:passTypeParam dataTypeName="TClass"/>
					<plx:passTypeParam dataTypeName="TProperty"/>
				</plx:param>
				<plx:param name="added" dataTypeName="CommittedCollectionModificationCallback">
					<plx:passTypeParam dataTypeName="TClass"/>
					<plx:passTypeParam dataTypeName="TProperty"/>
				</plx:param>
				<plx:param name="removing" dataTypeName="PotentialCollectionModificationCallback">
					<plx:passTypeParam dataTypeName="TClass"/>
					<plx:passTypeParam dataTypeName="TProperty"/>
				</plx:param>
				<plx:param name="removed" dataTypeName="CommittedCollectionModificationCallback">
					<plx:passTypeParam dataTypeName="TClass"/>
					<plx:passTypeParam dataTypeName="TProperty"/>
				</plx:param>
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="identityEquality">
							<plx:left>
								<plx:nameRef type="parameter" name="instance"/>
							</plx:left>
							<plx:right>
								<plx:nullKeyword/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:throw>
						<plx:callNew dataTypeName="ArgumentNullException">
							<plx:passParam>
								<plx:string>instance</plx:string>
							</plx:passParam>
						</plx:callNew>
					</plx:throw>
				</plx:branch>
				<plx:assign>
					<plx:left>
						<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}Instance"/>
					</plx:left>
					<plx:right>
						<plx:nameRef type="parameter" name="instance"/>
					</plx:right>
				</plx:assign>
				<plx:assign>
					<plx:left>
						<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}Adding"/>
					</plx:left>
					<plx:right>
						<plx:nameRef type="parameter" name="adding"/>
					</plx:right>
				</plx:assign>
				<plx:assign>
					<plx:left>
						<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}Added"/>
					</plx:left>
					<plx:right>
						<plx:nameRef type="parameter" name="added"/>
					</plx:right>
				</plx:assign>
				<plx:assign>
					<plx:left>
						<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}Removing"/>
					</plx:left>
					<plx:right>
						<plx:nameRef type="parameter" name="removing"/>
					</plx:right>
				</plx:assign>
				<plx:assign>
					<plx:left>
						<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}Removed"/>
					</plx:left>
					<plx:right>
						<plx:nameRef type="parameter" name="removed"/>
					</plx:right>
				</plx:assign>
			</plx:function>

			<plx:field visibility="private" readOnly="true" name="{$PrivateMemberPrefix}Adding" dataTypeName="PotentialCollectionModificationCallback">
				<plx:passTypeParam dataTypeName="TClass"/>
				<plx:passTypeParam dataTypeName="TProperty"/>
			</plx:field>
			<plx:field visibility="private" readOnly="true" name="{$PrivateMemberPrefix}Added" dataTypeName="CommittedCollectionModificationCallback">
				<plx:passTypeParam dataTypeName="TClass"/>
				<plx:passTypeParam dataTypeName="TProperty"/>
			</plx:field>
			<plx:field visibility="private" readOnly="true" name="{$PrivateMemberPrefix}Removing" dataTypeName="PotentialCollectionModificationCallback">
				<plx:passTypeParam dataTypeName="TClass"/>
				<plx:passTypeParam dataTypeName="TProperty"/>
			</plx:field>
			<plx:field visibility="private" readOnly="true" name="{$PrivateMemberPrefix}Removed" dataTypeName="CommittedCollectionModificationCallback">
				<plx:passTypeParam dataTypeName="TClass"/>
				<plx:passTypeParam dataTypeName="TProperty"/>
			</plx:field>

			<plx:function visibility="private" name="OnAdding">
				<plx:param name="value" dataTypeName="TProperty"/>
				<plx:returns dataTypeName=".boolean"/>
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="identityInequality">
							<plx:left>
								<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}Adding"/>
							</plx:left>
							<plx:right>
								<plx:nullKeyword/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:return>
						<plx:callInstance type="delegateCall" name=".implied">
							<plx:callObject>
								<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}Adding"/>
							</plx:callObject>
							<plx:passParam>
								<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}Instance"/>
							</plx:passParam>
							<plx:passParam>
								<plx:nameRef type="parameter" name="value"/>
							</plx:passParam>
						</plx:callInstance>
					</plx:return>
				</plx:branch>
				<plx:return>
					<plx:trueKeyword/>
				</plx:return>
			</plx:function>
			<plx:function visibility="private" name="OnAdded">
				<plx:param name="value" dataTypeName="TProperty"/>
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="identityInequality">
							<plx:left>
								<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}Added"/>
							</plx:left>
							<plx:right>
								<plx:nullKeyword/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
						<plx:callInstance type="delegateCall" name=".implied">
							<plx:callObject>
								<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}Added"/>
							</plx:callObject>
							<plx:passParam>
								<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}Instance"/>
							</plx:passParam>
							<plx:passParam>
								<plx:nameRef type="parameter" name="value"/>
							</plx:passParam>
						</plx:callInstance>
				</plx:branch>
			</plx:function>
			<plx:function visibility="private" name="OnRemoving">
				<plx:param name="value" dataTypeName="TProperty"/>
				<plx:returns dataTypeName=".boolean"/>
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="identityInequality">
							<plx:left>
								<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}Removing"/>
							</plx:left>
							<plx:right>
								<plx:nullKeyword/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:return>
						<plx:callInstance type="delegateCall" name=".implied">
							<plx:callObject>
								<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}Removing"/>
							</plx:callObject>
							<plx:passParam>
								<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}Instance"/>
							</plx:passParam>
							<plx:passParam>
								<plx:nameRef type="parameter" name="value"/>
							</plx:passParam>
						</plx:callInstance>
					</plx:return>
				</plx:branch>
				<plx:return>
					<plx:trueKeyword/>
				</plx:return>
			</plx:function>
			<plx:function visibility="private" name="OnRemoved">
				<plx:param name="value" dataTypeName="TProperty"/>
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="identityInequality">
							<plx:left>
								<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}Removed"/>
							</plx:left>
							<plx:right>
								<plx:nullKeyword/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:callInstance type="delegateCall" name=".implied">
						<plx:callObject>
							<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}Removed"/>
						</plx:callObject>
						<plx:passParam>
							<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}Instance"/>
						</plx:passParam>
						<plx:passParam>
							<plx:nameRef type="parameter" name="value"/>
						</plx:passParam>
					</plx:callInstance>
				</plx:branch>
			</plx:function>
			
			<plx:function visibility="privateInterfaceMember" name="GetEnumerator">
				<plx:interfaceMember memberName="GetEnumerator" dataTypeName="IEnumerable" dataTypeQualifier="System.Collections"/>
				<plx:returns dataTypeName="IEnumerator" dataTypeQualifier="System.Collections"/>
				<plx:return>
					<plx:callThis accessor="this" type="methodCall" name="GetEnumerator"/>
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
					<plx:callInstance type="methodCall" name="GetEnumerator">
						<plx:callObject>
							<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}List"/>
						</plx:callObject>
					</plx:callInstance>
				</plx:return>
			</plx:function>

			<plx:function visibility="public" name="Add">
				<plx:interfaceMember memberName="Add" dataTypeName="ICollection">
					<plx:passTypeParam dataTypeName="TProperty"/>
				</plx:interfaceMember>
				<plx:param name="item" dataTypeName="TProperty"/>
				<plx:branch>
					<plx:condition>
						<plx:callThis accessor="this" type="methodCall" name="OnAdding">
							<plx:passParam>
								<plx:nameRef type="parameter" name="item"/>
							</plx:passParam>
						</plx:callThis>
					</plx:condition>
					<plx:callInstance type="methodCall" name="Add">
						<plx:callObject>
							<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}List"/>
						</plx:callObject>
						<plx:passParam>
							<plx:nameRef type="parameter" name="item"/>
						</plx:passParam>
					</plx:callInstance>
					<plx:callThis accessor="this" type="methodCall" name="OnAdded">
						<plx:passParam>
							<plx:nameRef type="parameter" name="item"/>
						</plx:passParam>
					</plx:callThis>
				</plx:branch>
			</plx:function>

			<plx:function visibility="public" name="Remove">
				<plx:interfaceMember memberName="Remove" dataTypeName="ICollection">
					<plx:passTypeParam dataTypeName="TProperty"/>
				</plx:interfaceMember>
				<plx:param name="item" dataTypeName="TProperty"/>
				<plx:returns dataTypeName=".boolean"/>
				<plx:branch>
					<plx:condition>
						<plx:callThis accessor="this" type="methodCall" name="OnRemoving">
							<plx:passParam>
								<plx:nameRef type="parameter" name="item"/>
							</plx:passParam>
						</plx:callThis>
					</plx:condition>
					<plx:branch>
						<plx:condition>
							<plx:callInstance type="methodCall" name="Remove">
								<plx:callObject>
									<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}List"/>
								</plx:callObject>
								<plx:passParam>
									<plx:nameRef type="parameter" name="item"/>
								</plx:passParam>
							</plx:callInstance>
						</plx:condition>
						<plx:callThis accessor="this" type="methodCall" name="OnRemoved">
							<plx:passParam>
								<plx:nameRef type="parameter" name="item"/>
							</plx:passParam>
						</plx:callThis>
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
				<plx:loop>
					<plx:initializeLoop>
						<plx:local name="i" dataTypeName=".i4">
							<plx:initialize>
								<plx:value type="i4" data="0"/>
							</plx:initialize>
						</plx:local>
					</plx:initializeLoop>
					<plx:condition>
						<plx:binaryOperator type="lessThan">
							<plx:left>
								<plx:nameRef type="local" name="i"/>
							</plx:left>
							<plx:right>
								<plx:callInstance type="property" name="Count">
									<plx:callObject>
										<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}List"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:beforeLoop>
						<plx:increment>
							<plx:nameRef name="i"/>
						</plx:increment>
					</plx:beforeLoop>
					<plx:callThis accessor="this" type="methodCall" name="Remove">
						<plx:passParam>
							<plx:callInstance type="indexerCall" name=".implied">
								<plx:callObject>
									<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}List"/>
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
					<plx:callInstance name="Contains">
						<plx:callObject>
							<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}List"/>
						</plx:callObject>
						<plx:passParam>
							<plx:nameRef type="parameter" name="item"/>
						</plx:passParam>
					</plx:callInstance>
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
						<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}List"/>
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
								<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}List"/>
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
	
</xsl:stylesheet>
