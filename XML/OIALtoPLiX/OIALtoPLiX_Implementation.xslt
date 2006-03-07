<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:exsl="http://exslt.org/common"
	xmlns:oil="http://schemas.orm.net/OIAL"
	xmlns:odt="http://schemas.orm.net/ORMDataTypes"
	xmlns:plx="http://schemas.neumont.edu/CodeGeneration/PLiX"
	xmlns:xs="http://www.w3.org/2001/XMLSchema"
	xmlns:prop="urn:schemas-orm-net:PLiX:CLI:Properties">

	<xsl:import href="OIALtoPLiX.xslt"/>

	<xsl:output method="xml" encoding="utf-8" indent="yes"/>

	<xsl:template match="oil:model" mode="OIALtoPLiX_Implementation">
		<xsl:param name="Model"/>
		<xsl:param name="ModelContextName"/>
		<xsl:param name="InformationTypeFormatMappings"/>
		<xsl:param name="AllProperties"/>

		<plx:class visibility="public" modifier="sealed" name="{$ModelContextName}">
			<plx:leadingInfo>
				<plx:pragma type="region" data="{$ModelContextName}"/>
			</plx:leadingInfo>
			<plx:trailingInfo>
				<plx:pragma type="closeRegion" data="{$ModelContextName}"/>
			</plx:trailingInfo>
			<plx:implementsInterface dataTypeName="I{$ModelContextName}"/>
			<plx:function name=".construct"  visibility="public"/>

			<xsl:call-template name="GenerateModelContextMethods">
				<xsl:with-param name="Model" select="."/>
				<xsl:with-param name="ModelContextName" select="$ModelContextName"/>
				<xsl:with-param name="AllProperties" select="$AllProperties"/>
			</xsl:call-template>
			<!--<xsl:apply-templates mode="ForGenerateImplementationClass" select="$AbsorbedObjects">
				<xsl:with-param name="Model" select="."/>
				<xsl:with-param name="ModelContextName" select="$ModelContextName"/>
			</xsl:apply-templates>-->
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
					<plx:callThis name="{$PrivateMemberPrefix}IsDeserializing" type="field"/>
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
						<xsl:copy-of select="self::plx:param/@*"/>
						<xsl:copy-of select="self::plx:param/child::*"/>
					</plx:passTypeParam>
				</xsl:for-each>
			</xsl:variable>
			<xsl:variable name="passTypeParams" select="exsl:node-set($passTypeParamsFragment)/child::*"/>
			
			<plx:field visibility="private" name="{$PrivateMemberPrefix}{@name}Dictionary" dataTypeName="Dictionary">
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
							<plx:callStatic name="CreateTuple" dataTypeName="Tuple">
								<xsl:for-each select="$parameters">
									<plx:passMemberTypeParam>
										<xsl:copy-of select="self::plx:param/@*"/>
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
			<xsl:variable name="uniqueConceptTypeName" select="parent::prop:Properties/@conceptTypeName"/>
			<!-- TODO: In Get{Thing}By{Name}, {Name} should be oil:singleRoleUniquenessConstraint/@name rather than prop:Property/@name. -->
			<plx:function visibility="public" name="Get{$uniqueConceptTypeName}By{@name}">
				<plx:param name="{@name}">
					<xsl:copy-of select="prop:DataType/@*"/>
					<xsl:copy-of select="prop:DataType/child::*"/>
				</plx:param>
				<plx:returns dataTypeName="{$uniqueConceptTypeName}"/>
			</plx:function>
		</xsl:for-each>
	</xsl:template>

</xsl:stylesheet>
