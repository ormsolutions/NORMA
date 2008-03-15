<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet
	version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:plx="http://schemas.neumont.edu/CodeGeneration/PLiX"
	xmlns:orm="http://schemas.neumont.edu/ORM/2006-04/ORMCore"
	xmlns:oial="http://schemas.neumont.edu/ORM/Abstraction/2007-06/Core"
	xmlns:odt="http://schemas.neumont.edu/ORM/Abstraction/2007-06/DataTypes/Core"
	xmlns:ormRoot="http://schemas.neumont.edu/ORM/2006-04/ORMRoot"
	xmlns:ormtooial="http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMToORMAbstraction"
	xmlns:exsl="http://exslt.org/common"
	extension-element-prefixes="exsl"
	>
	<xsl:output indent="yes"/>
	<xsl:param name="GenerateCodeAnalysisAttributes" select="true()"/>
	<xsl:param name="GenerateAccessedThroughPropertyAttribute" select="false()"/>
	<xsl:param name="GenerateObjectDataSourceSupport" select="true()"/>
	<xsl:param name="RaiseEventsAsynchronously" select="true()"/>
	<xsl:param name="SynchronizeEventAddRemove" select="true()"/>
	<xsl:param name="DefaultNamespace" select="'MyNamespace'"/>
	<xsl:param name="PrivateMemberPrefix" select="'_'"/>
	<xsl:variable name="GenerateServiceLayer" select="true()"/>
	<xsl:variable name="GenerateLinqAttributes" select="true()"/>
	<xsl:variable name="CollectionSuffix" select="'Collection'"/>
	<xsl:variable name="TableSuffix" select="'Table'"/>


	<xsl:template match="/">
		<plx:root>
			<!--Create the Namespaces Needed-->
			<plx:namespaceImport name="System"/>
			<plx:namespaceImport name="System.Collections.Generic"/>
			<plx:namespaceImport name="System.ComponentModel"/>
			<plx:namespaceImport name="System.Data"/>
			<plx:namespaceImport name="System.Data.Linq"/>
			<plx:namespaceImport name="System.Data.Linq.Mapping"/>
			<plx:namespaceImport name="System.Diagnostics.CodeAnalysis"/>
			<plx:namespaceImport name="System.Linq"/>
			<plx:namespaceImport name="System.Runtime.Serialization"/>
			<xsl:if test="$GenerateServiceLayer">
				<plx:namespaceImport name="System.ServiceModel"/>
			</xsl:if>
			<plx:namespaceImport name="System.Threading"/>
			<!--Create the DatabaseContext and the ServiceContract-->
			<xsl:apply-templates select="ormRoot:ORM2/oial:model" mode="GenerateNamespace"/>
		</plx:root>
	</xsl:template>

	<xsl:template match="oial:model" mode="GenerateNamespace">
		<xsl:variable name="modelName" select="@name"/>
		<xsl:variable name="fullyQualifiedNamespace" select="$DefaultNamespace"/>
		<plx:namespace name="{$fullyQualifiedNamespace}">
			<xsl:if test="$GenerateServiceLayer">
				<xsl:apply-templates select="." mode="GenerateServiceContract">
					<xsl:with-param name="modelName" select="$modelName"/>
				</xsl:apply-templates>
			</xsl:if>
			<!-- Generate Database Context-->
			<xsl:apply-templates select="." mode="GenerateDatabaseContext">
				<xsl:with-param name="modelName" select="$modelName"/>
				<xsl:with-param name="fullyQualifiedNamespace" select="$fullyQualifiedNamespace"/>
			</xsl:apply-templates>
			<!--Generate Enumerations for any ValueConstraints that qualify. -->
			<xsl:apply-templates select="/ormRoot:ORM2/orm:ORMModel/orm:Objects/orm:ValueType[orm:ValueRestriction/orm:ValueConstraint/orm:ValueRanges[count(orm:ValueRange) = count(orm:ValueRange[@MinValue=@MaxValue])]]" mode="GenerateEnumerations"/>
			<!-- Generate Business Entities for Database Context-->
			<xsl:apply-templates select="oial:conceptTypes/oial:conceptType" mode="GenerateBusinessEntities">

			</xsl:apply-templates>
			<!--Create the DataContracts and the TableAttributes-->
			<xsl:call-template name="GenerateGlobalSupportClasses"/>
		</plx:namespace>
	</xsl:template>

	<xsl:template match="oial:model" mode="GenerateServiceContract">
		<xsl:param name="modelName" select="@name"/>
		<xsl:if test="$GenerateServiceLayer">
			<plx:interface visibility="public" name="I{$modelName}Service">
				<plx:attribute dataTypeName="ServiceContractAttribute"/>
				<!--<xsl:apply-templates select="oial:conceptTypes/oial:conceptType" mode="GenerateOperationContract"/>-->
			</plx:interface>
		</xsl:if>
	</xsl:template>

	<xsl:template match="oial:conceptType" mode="GenerateBusinessEntities">
		<xsl:variable name="conceptTypeName" select="@name"/>
		<plx:class visibility="public" name="{$conceptTypeName}">
			<plx:implementsInterface dataTypeName="INotifyPropertyChanging"/>
			<plx:implementsInterface dataTypeName="INotifyPropertyChanged" />
			<xsl:if test="$GenerateServiceLayer">
				<plx:attribute dataTypeName="DataContract">
					<plx:passParam>
						<plx:binaryOperator type="assignNamed">
							<plx:left>
								<plx:nameRef name="Name"/>
							</plx:left>
							<plx:right>
								<!--TODO: Set this via a settings file.-->
								<plx:string data="{$conceptTypeName}"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:passParam>
				</plx:attribute>
			</xsl:if>
			<xsl:if test="$GenerateLinqAttributes">
				<plx:attribute dataTypeName="Table">
					<plx:passParam>
						<plx:binaryOperator type="assignNamed">
							<plx:left>
								<plx:nameRef name="Name"/>
							</plx:left>
							<plx:right>
								<!--TODO: Set this via a settings file.-->
								<plx:string data="{$conceptTypeName}"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:passParam>
				</plx:attribute>
			</xsl:if>
			<xsl:variable name="containingConceptType" select="."/>
			<plx:function name=".construct" visibility="public">
				<xsl:for-each select="../oial:conceptType[oial:children/oial:relatedConceptType/@ref = current()/@id]">
					<plx:assign>
						<plx:left>
							<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}{./@name}"/>
						</plx:left>
						<plx:right>
							<plx:callNew dataTypeName="EntitySet">
								<plx:passTypeParam dataTypeName="{@name}"/>
								<plx:passParam>
									<plx:callNew dataTypeName="Action">
										<plx:passTypeParam dataTypeName="{@name}"/>
										<plx:passParam>
											<plx:callThis accessor="this" name="On{@name}Added" type="fireCustomEvent"/>
										</plx:passParam>
									</plx:callNew>
								</plx:passParam>
								<plx:passParam>
									<plx:callNew dataTypeName="Action">
										<plx:passTypeParam dataTypeName="{@name}"/>
										<plx:passParam>
											<plx:callThis accessor="this" type="fireCustomEvent" name="On{@name}Removed"/>
										</plx:passParam>
									</plx:callNew>
								</plx:passParam>
							</plx:callNew>
						</plx:right>
					</plx:assign>
					<!--this._Persons = new EntitySet<Person> (new Action<Person>(this.attach_Persons), new Action<Person>(this.detach_Persons));-->
					
				</xsl:for-each>
			</plx:function>
			<plx:pragma type="region" data="Databinding Information for INotifyPropertyChanging and INotifyPropertyChanged"/>
			<xsl:call-template name="GenerateINotifyPropertyChangingImplementation"/>
			<xsl:call-template name="GenerateINotifyPropertyChangedImplementation"/>
			<plx:pragma type="closeRegion" data="Databinding Information for INotifyPropertyChanging and INotifyPropertyChanged"/>
			<xsl:apply-templates select="oial:children/child::*" mode="GenerateEntityMembers"/>
			<xsl:for-each select="../oial:conceptType[oial:children/oial:relatedConceptType/@ref = current()/@id]">
				<xsl:apply-templates select="." mode="GenerateEntitySetMembers">
					<xsl:with-param name="containingConceptType" select="$containingConceptType"/>
				</xsl:apply-templates>
			</xsl:for-each>
		</plx:class>
	</xsl:template>

	<xsl:template match="oial:conceptType" mode="GenerateEntitySetMembers">
		<xsl:param name="containingConceptType"/>
		<xsl:variable name="preferredIdentifierInformationTypesFragment">
			<xsl:apply-templates select="$containingConceptType" mode="GetInformationTypesForPreferredIdentifier"/>
		</xsl:variable>
		<!--<xsl:for-each select="exsl:node-set($preferredIdentifierInformationTypesFragment)/child::*">
			<xsl:apply-templates select="." mode="GenerateEntityMembers">
				<xsl:with-param name="informationTypeName" select="@name"/>
			</xsl:apply-templates>
		</xsl:for-each>-->
		<xsl:variable name="conceptTypeName" select="@name"/>
		<!--TODO: Use @oppositeName instead of @name -->
		<xsl:variable name="oppositeRoleName" select="@name"/>
		<!--TODO: Update this field to include the @name instead of the oppositeName when the serialization engine is fixed.-->
		<xsl:variable name="privateMemberName" select="concat($PrivateMemberPrefix,$conceptTypeName)"/>
		<!-- TODO: determine the correct datatype -->
		<plx:field visibility="private" static="true" readOnly="true" dataTypeName="PropertyChangingEventArgs" name="{$conceptTypeName}PropertyChangingEventArgs">
			<plx:initialize>
				<plx:callNew dataTypeName="PropertyChangingEventArgs">
					<plx:passParam>
						<plx:string data="{$oppositeRoleName}"/>
					</plx:passParam>
				</plx:callNew>
			</plx:initialize>
		</plx:field>
		<plx:field visibility="private" static="true" readOnly="true" dataTypeName="PropertyChangedEventArgs" name="{$conceptTypeName}PropertyChangedEventArgs">
			<plx:initialize>
				<plx:callNew dataTypeName="PropertyChangedEventArgs">
					<plx:passParam>
						<plx:string data="{$oppositeRoleName}"/>
					</plx:passParam>
				</plx:callNew>
			</plx:initialize>
		</plx:field>

		<plx:field visibility="private" dataTypeName="EntitySet" name="{$privateMemberName}">
			<plx:passTypeParam dataTypeName="{$conceptTypeName}"/>
		</plx:field>
		<plx:property visibility="public" name="{$oppositeRoleName}">
			<xsl:if test="$GenerateServiceLayer">
				<plx:attribute dataTypeName="DataMember">
					<plx:passParam>
						<plx:binaryOperator type="assignNamed">
							<plx:left>
								<plx:nameRef name="Name"/>
							</plx:left>
							<plx:right>
								<!--TODO: Set this via a settings file.-->
								<plx:string data="{$oppositeRoleName}"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:passParam>
				</plx:attribute>
			</xsl:if>
			<xsl:if test="$GenerateLinqAttributes">
				<plx:attribute dataTypeName="Association">
					<plx:passParam>
						<plx:binaryOperator type="assignNamed">
							<plx:left>
								<plx:nameRef name="Storage"/>
							</plx:left>
							<plx:right>
								<!--TODO: Set this via a settings file.-->
								<plx:string data="{$privateMemberName}"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:passParam>
					<plx:passParam>
						<plx:binaryOperator type="assignNamed">
							<plx:left>
								<plx:nameRef name="OtherKey"/>
							</plx:left>
							<plx:right>
								<plx:string>
									<xsl:for-each select="exsl:node-set($preferredIdentifierInformationTypesFragment)/child::*">
										<xsl:value-of select="@name"/>
										<xsl:if test="position() != last()">
											<xsl:text>,</xsl:text>
										</xsl:if>
									</xsl:for-each>
								</plx:string>
							</plx:right>
						</plx:binaryOperator>
					</plx:passParam>
					<!--TODO: When NORMA provides information as to how the identites are being genrated, set the IsDbGenerated Attribute Property-->
					<!--TODO: When we can inferr what kind of datatypes the database will be using for these columns, generate the DbType Attribute Property.-->
				</plx:attribute>
			</xsl:if>
			<plx:returns dataTypeName="EntitySet">
				<plx:passTypeParam dataTypeName="{$conceptTypeName}"/>
			</plx:returns>
			<plx:get>
				<plx:return>
					<plx:callThis accessor="this" type="field" name="{$privateMemberName}"/>
				</plx:return>
			</plx:get>
			<plx:set>
				<plx:callInstance type="methodCall" name="Assign">
					<plx:callObject>
						<plx:callThis name="{$privateMemberName}" accessor="this" type="field"/>
					</plx:callObject>
					<plx:passParam>
						<plx:valueKeyword/>
					</plx:passParam>
				</plx:callInstance>
			</plx:set>
		</plx:property>
		<plx:function visibility="private" name="On{$conceptTypeName}Added">
			<plx:param dataTypeName="{$conceptTypeName}" name="entity"/>
			<plx:callThis accessor="this" name="OnPropertyChanging">
				<plx:passParam>
					<plx:callThis accessor="static" type="field" name="{$conceptTypeName}PropertyChangingEventArgs"/>
				</plx:passParam>
			</plx:callThis>
			<plx:assign>
				<plx:left>
					<plx:callInstance type="property" name="{$containingConceptType/@name}">
						<plx:callObject>
							<plx:nameRef name="entity" type="parameter"/>
						</plx:callObject>
					</plx:callInstance>
				</plx:left>
				<plx:right>
					<plx:thisKeyword/>
				</plx:right>
			</plx:assign>
			<plx:callThis accessor="this" name="OnPropertyChanged">
				<plx:passParam>
					<plx:callThis accessor="static" type="field" name="{$conceptTypeName}PropertyChangedEventArgs"/>
				</plx:passParam>
			</plx:callThis>
		</plx:function>
		<plx:function visibility="private" name="On{$conceptTypeName}Removed">
			<plx:param dataTypeName="{$conceptTypeName}" name="entity"/>
			<plx:callThis accessor="this" name="OnPropertyChanging">
				<plx:passParam>
					<plx:callThis accessor="static" type="field" name="{$conceptTypeName}PropertyChangingEventArgs"/>
				</plx:passParam>
			</plx:callThis>
			<plx:assign>
				<plx:left>
					<plx:callInstance type="property" name="{$containingConceptType/@name}">
						<plx:callObject>
							<plx:nameRef name="entity" type="parameter"/>
						</plx:callObject>
					</plx:callInstance>
				</plx:left>
				<plx:right>
					<plx:nullKeyword/>
				</plx:right>
			</plx:assign>
			<plx:callThis accessor="this" name="OnPropertyChanged">
				<plx:passParam>
					<plx:callThis accessor="static" type="field" name="{$conceptTypeName}PropertyChangedEventArgs"/>
				</plx:passParam>
			</plx:callThis>
		</plx:function>
	</xsl:template>

	<xsl:template match="oial:informationType" mode="GenerateEntityMembers">
		<xsl:variable name="informationTypeName" select="@name"/>
		<!-- TODO: determine the correct datatype -->
		<plx:field visibility="private" static="true" readOnly="true" dataTypeName="PropertyChangingEventArgs" name="{$informationTypeName}PropertyChangingEventArgs">
			<plx:initialize>
				<plx:callNew dataTypeName="PropertyChangingEventArgs">
					<plx:passParam>
						<plx:string data="{$informationTypeName}"/>
					</plx:passParam>
				</plx:callNew>
			</plx:initialize>
		</plx:field>
		<plx:field visibility="private" static="true" readOnly="true" dataTypeName="PropertyChangedEventArgs" name="{$informationTypeName}PropertyChangedEventArgs">
			<plx:initialize>
				<plx:callNew dataTypeName="PropertyChangedEventArgs">
					<plx:passParam>
						<plx:string data="{$informationTypeName}"/>
					</plx:passParam>
				</plx:callNew>
			</plx:initialize>
		</plx:field>
		<plx:field visibility="private" name="{$PrivateMemberPrefix}{$informationTypeName}">
			<xsl:choose>
				<xsl:when test="/ormRoot:ORM2/orm:ORMModel/orm:Objects/orm:ValueType[orm:ValueRestriction/orm:ValueConstraint/orm:ValueRanges[count(orm:ValueRange) = count(orm:ValueRange[@MinValue=@MaxValue])]][@id = /ormRoot:ORM2/ormtooial:Bridge/ormtooial:InformationTypeFormatIsForValueType[@InformationTypeFormat = current()/@ref]/@ValueType]">
					<xsl:attribute name="dataTypeName">
						<xsl:value-of select="$informationTypeName"/>
					</xsl:attribute>
				</xsl:when>
				<xsl:otherwise>
					<xsl:attribute name="dataTypeName">
						<xsl:value-of select="'.object'"/>
					</xsl:attribute>
					<!--TODO: Set Nullible Type for plx:field-->
					<!--<xsl:choose>
						<xsl:when test="@isMandatory">
							<xsl:attribute name="dataTypeName">

							</xsl:attribute>
						</xsl:when>
						<xsl:otherwise>
							<xsl:attribute name="dataTypeName">
								<xsl:value-of select="'.object'"/>
							</xsl:attribute>
						</xsl:otherwise>
					</xsl:choose>-->
				</xsl:otherwise>
			</xsl:choose>
		</plx:field>
		<plx:property visibility="public" name="{$informationTypeName}">
			<xsl:if test="$GenerateServiceLayer">
				<plx:attribute dataTypeName="DataMember">
					<plx:passParam>
						<plx:binaryOperator type="assignNamed">
							<plx:left>
								<plx:nameRef name="Name"/>
							</plx:left>
							<plx:right>
								<!--TODO: Set this via a settings file.-->
								<plx:string data="{$informationTypeName}"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:passParam>
					<plx:passParam>
						<plx:binaryOperator type="assignNamed">
							<plx:left>
								<plx:nameRef name="IsRequired"/>
							</plx:left>
							<plx:right>
								<!--TODO: Set this via a settings file.-->
								<xsl:choose>
									<xsl:when test="@isMandatory">
										<plx:trueKeyword/>
									</xsl:when>
									<xsl:otherwise>
										<plx:falseKeyword/>
									</xsl:otherwise>
								</xsl:choose>
							</plx:right>
						</plx:binaryOperator>
					</plx:passParam>
				</plx:attribute>
			</xsl:if>
			<xsl:if test="$GenerateLinqAttributes">
				<plx:attribute dataTypeName="Column">
					<plx:passParam>
						<plx:binaryOperator type="assignNamed">
							<plx:left>
								<plx:nameRef name="Storage"/>
							</plx:left>
							<plx:right>
								<!--TODO: Set this via a settings file.-->
								<plx:string data="{$PrivateMemberPrefix}{$informationTypeName}"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:passParam>
					<plx:passParam>
						<plx:binaryOperator type="assignNamed">
							<plx:left>
								<plx:nameRef name="CanBeNull"/>
							</plx:left>
							<plx:right>
								<!--TODO: Set this via a settings file.-->
								<xsl:choose>
									<xsl:when test="@isMandatory">
										<plx:falseKeyword/>
									</xsl:when>
									<xsl:otherwise>
										<plx:trueKeyword/>
									</xsl:otherwise>
								</xsl:choose>
							</plx:right>
						</plx:binaryOperator>
					</plx:passParam>
					<xsl:if test="../../oial:uniquenessConstraints/oial:uniquenessConstraint[@isPreferred=true()]/child::*[@ref = current()/@id]">
						<plx:passParam>
							<plx:binaryOperator type="assignNamed">
								<plx:left>
									<plx:nameRef name="IsPrimaryKey"/>
								</plx:left>
								<plx:right>
									<!--TODO: Set this via a settings file.-->
									<plx:trueKeyword/>
								</plx:right>
							</plx:binaryOperator>
						</plx:passParam>
					</xsl:if>
					<!--TODO: When NORMA provides information as to how the identites are being genrated-->
					<!--<xsl:if test="">
								<plx:passParam>
									<plx:binaryOperator type="assignNamed">
										<plx:left>
											<plx:nameRef name="IsDbGenerated"/>
										</plx:left>
										<plx:right>
											-->
					<!--TODO: Set this via a settings file.-->
					<!--
											<plx:trueKeyword/>
										</plx:right>
									</plx:binaryOperator>
								</plx:passParam>
							</xsl:if>-->
					<!--TODO: When we can inferr what kind of datatypes the database will be using for these columns.-->
					<!--<xsl:if test="">
								<plx:passParam>
									<plx:binaryOperator type="assignNamed">
										<plx:left>
											<plx:nameRef name="DbType"/>
										</plx:left>
										<plx:right>
											-->
					<!--TODO: Set this via a settings file.-->
					<!--
								<plx:string data="{$PrivateMemberPrefix}{$informationTypeName}"/>
										</plx:right>
									</plx:binaryOperator>
								</plx:passParam>
							</xsl:if>-->
				</plx:attribute>
			</xsl:if>
			<!-- TODO: Pass in correct datatype -->
			<plx:returns>
				<xsl:choose>
					<xsl:when test="/ormRoot:ORM2/orm:ORMModel/orm:Objects/orm:ValueType[orm:ValueRestriction/orm:ValueConstraint/orm:ValueRanges[count(orm:ValueRange) = count(orm:ValueRange[@MinValue=@MaxValue])]][@id = /ormRoot:ORM2/ormtooial:Bridge/ormtooial:InformationTypeFormatIsForValueType[@InformationTypeFormat = current()/@ref]/@ValueType]">
						<xsl:attribute name="dataTypeName">
							<xsl:value-of select="$informationTypeName"/>
						</xsl:attribute>
					</xsl:when>
					<xsl:otherwise>
						<!--TODO: Set Nullible Type for plx:field-->
						<xsl:attribute name="dataTypeName">
							<xsl:value-of select="'.object'"/>
						</xsl:attribute>
					</xsl:otherwise>
				</xsl:choose>
			</plx:returns>
			<plx:get>
				<plx:return>
					<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}{$informationTypeName}" />
				</plx:return>
			</plx:get>
			<plx:set>
				<xsl:if test="@isMandatory">
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
						<plx:throw>
							<plx:callNew dataTypeName="ArgumentNullException"/>
						</plx:throw>
					</plx:branch>
				</xsl:if>
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="identityInequality">
							<plx:left>
								<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}{$informationTypeName}" />
							</plx:left>
							<plx:right>
								<plx:valueKeyword/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:callThis accessor="this" type="methodCall" name="OnPropertyChanging">
						<plx:passParam>
							<plx:nameRef name="{$informationTypeName}PropertyChangingEventArgs"/>
						</plx:passParam>
					</plx:callThis>
					<plx:assign>
						<plx:left>
							<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}{$informationTypeName}" />
						</plx:left>
						<plx:right>
							<plx:valueKeyword/>
						</plx:right>
					</plx:assign>
					<plx:callThis accessor="this" type="methodCall" name="OnPropertyChanged">
						<plx:passParam>
							<plx:nameRef name="{$informationTypeName}PropertyChangedEventArgs"/>
						</plx:passParam>
					</plx:callThis>
				</plx:branch>
			</plx:set>
		</plx:property>
	</xsl:template>

	<!--TODO: Figure out how to call this.-->
	<xsl:template name="ValidateMandatoryParameter">
		<xsl:param name="paramToValidate"/>
		<plx:branch>
			<plx:condition>
				<plx:binaryOperator type="identityEquality">
					<plx:left>
						<xsl:value-of select="$paramToValidate"/>
					</plx:left>
					<plx:right>
						<plx:nullKeyword/>
					</plx:right>
				</plx:binaryOperator>
			</plx:condition>
			<plx:throw>
				<plx:callNew dataTypeName="ArgumentNullException"/>
			</plx:throw>
		</plx:branch>
	</xsl:template>

	<xsl:template match="oial:conceptType" mode="GetInformationTypesForPreferredIdentifier">
		<!-- First see if we have a uniquenessConstraint that is preferred. -->
		<xsl:variable name="conceptType" select="."/>
		<xsl:variable name="preferredUniquenessConstraint" select="oial:uniquenessConstraints/oial:uniquenessConstraint[@isPreferred = 'true' or @isPreferred = 1]"/>
		<xsl:variable name="preferredAssimilatedConceptType" select="oial:children/oial:assimilatedConceptType[@isPreferredForParent = 'true' or @isPreferredForParent = 1]"/>
		<xsl:variable name="preferredAssimilationsOfThisConceptType" select="parent::oial:conceptTypes/oial:conceptType/oial:children/oial:assimilatedConceptType[@ref = current()/@id and (@isPreferredForTarget = 'true' or @isPreferredForTarget = 1)]"/>
		<xsl:choose>
			<xsl:when test="$preferredUniquenessConstraint">
				<xsl:for-each select="$preferredUniquenessConstraint/oial:uniquenessChild">
					<!-- Find the right child for each member of the uniqueness constraint here rather than in for-each so that the order of the uniqueness constraint is preserved. -->
					<xsl:apply-templates select="$conceptType/oial:children/oial:*[@id = current()/@ref]" mode="GetInformationTypesForPreferredIdentifier"/>
				</xsl:for-each>
			</xsl:when>
			<xsl:when test="$preferredAssimilatedConceptType">
				<xsl:apply-templates select="parent::oial:conceptTypes/oial:conceptType[@id = $preferredAssimilatedConceptType/@ref]" mode="GetInformationTypesForPreferredIdentifier"/>
			</xsl:when>
			<xsl:when test="$preferredAssimilationsOfThisConceptType">
				<xsl:apply-templates select="$preferredAssimilationsOfThisConceptType[1]/parent::oial:conceptType" mode="GetInformationTypesForPreferredIdentifier"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:message terminate="yes">
					<xsl:text>SANITY CHECK: Concept type '</xsl:text>
					<xsl:value-of select="@name"/>
					<xsl:text>' (id '</xsl:text>
					<xsl:value-of select="@id"/>
					<xsl:text>') does not have any preferred identifier.</xsl:text>
				</xsl:message>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="oial:informationType" mode="GetInformationTypesForPreferredIdentifier">
		<xsl:copy-of select="."/>
	</xsl:template>

	<xsl:template match="oial:relatedConceptType | oial:assimilatedConceptType" mode="GetInformationTypesForPreferredIdentifier">
		<xsl:apply-templates select="parent::oial:children/parent::oial:conceptType/parent::oial:conceptTypes/oial:conceptType[@id = current()/@ref]" mode="GetInformationTypesForPreferredIdentifier"/>
	</xsl:template>

	<xsl:template match="oial:relatedConceptType" mode="GenerateEntityMembers">
		<xsl:variable name="preferredIdentifierInformationTypesFragment">
			<xsl:apply-templates select="." mode="GetInformationTypesForPreferredIdentifier"/>
		</xsl:variable>
		<xsl:for-each select="exsl:node-set($preferredIdentifierInformationTypesFragment)/child::*">
			<xsl:apply-templates select="." mode="GenerateEntityMembers">
				<xsl:with-param name="informationTypeName" select="@name"/>
			</xsl:apply-templates>
		</xsl:for-each>

		<!--TODO: Update this field to include the @name instead of the oppositeName when the serialization engine is fixed.-->
		<xsl:variable name="conceptTypeName" select="@oppositeName"/>
		<xsl:variable name="privateMemberName" select="concat($PrivateMemberPrefix,$conceptTypeName)"/>
		<!-- TODO: determine the correct datatype -->
		<plx:field visibility="private" static="true" readOnly="true" dataTypeName="PropertyChangingEventArgs" name="{$conceptTypeName}PropertyChangingEventArgs">
			<plx:initialize>
				<plx:callNew dataTypeName="PropertyChangingEventArgs">
					<plx:passParam>
						<plx:string data="{$conceptTypeName}"/>
					</plx:passParam>
				</plx:callNew>
			</plx:initialize>
		</plx:field>
		<plx:field visibility="private" static="true" readOnly="true" dataTypeName="PropertyChangedEventArgs" name="{$conceptTypeName}PropertyChangedEventArgs">
			<plx:initialize>
				<plx:callNew dataTypeName="PropertyChangedEventArgs">
					<plx:passParam>
						<plx:string data="{$conceptTypeName}"/>
					</plx:passParam>
				</plx:callNew>
			</plx:initialize>
		</plx:field>

		<plx:field visibility="private" dataTypeName="EntityRef" name="{$privateMemberName}">
			<plx:passTypeParam dataTypeName="{$conceptTypeName}"/>
		</plx:field>
		<plx:property visibility="public" name="{$conceptTypeName}">
			<xsl:if test="$GenerateServiceLayer">
				<plx:attribute dataTypeName="DataMember">
					<plx:passParam>
						<plx:binaryOperator type="assignNamed">
							<plx:left>
								<plx:nameRef name="Name"/>
							</plx:left>
							<plx:right>
								<!--TODO: Set this via a settings file.-->
								<plx:string data="{$conceptTypeName}"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:passParam>
					<plx:passParam>
						<plx:binaryOperator type="assignNamed">
							<plx:left>
								<plx:nameRef name="IsRequired"/>
							</plx:left>
							<plx:right>
								<!--TODO: Set this via a settings file.-->
								<xsl:choose>
									<xsl:when test="@isMandatory">
										<plx:trueKeyword/>
									</xsl:when>
									<xsl:otherwise>
										<plx:falseKeyword/>
									</xsl:otherwise>
								</xsl:choose>
							</plx:right>
						</plx:binaryOperator>
					</plx:passParam>
				</plx:attribute>
			</xsl:if>
			<xsl:if test="$GenerateLinqAttributes">
				<plx:attribute dataTypeName="Association">
					<plx:passParam>
						<plx:binaryOperator type="assignNamed">
							<plx:left>
								<plx:nameRef name="Storage"/>
							</plx:left>
							<plx:right>
								<!--TODO: Set this via a settings file.-->
								<plx:string data="{$privateMemberName}"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:passParam>
					<plx:passParam>
						<plx:binaryOperator type="assignNamed">
							<plx:left>
								<plx:nameRef name="IsForeignKey"/>
							</plx:left>
							<plx:right>
								<!--TODO: Set this via a settings file.-->
								<plx:trueKeyword/>
							</plx:right>
						</plx:binaryOperator>
					</plx:passParam>
					<xsl:if test="../../oial:uniquenessConstraints/oial:uniquenessConstraint[@isPreferred=true()]/child::*[@ref = current()/@id]">
						<plx:passParam>
							<plx:binaryOperator type="assignNamed">
								<plx:left>
									<plx:nameRef name="IsPrimaryKey"/>
								</plx:left>
								<plx:right>
									<!--TODO: Set this via a settings file.-->
									<plx:trueKeyword/>
								</plx:right>
							</plx:binaryOperator>
						</plx:passParam>
					</xsl:if>
					<plx:passParam>
						<plx:binaryOperator type="assignNamed">
							<plx:left>
								<plx:nameRef name="ThisKey"/>
							</plx:left>
							<plx:right>
								<plx:string>
									<xsl:for-each select="exsl:node-set($preferredIdentifierInformationTypesFragment)/child::*">
										<xsl:value-of select="@name"/>
										<xsl:if test="position() != last()">
											<xsl:text>,</xsl:text>
										</xsl:if>
									</xsl:for-each>
								</plx:string>
							</plx:right>
						</plx:binaryOperator>
					</plx:passParam>
					<!--TODO: When NORMA provides information as to how the identites are being genrated, set the IsDbGenerated Attribute Property-->
					<!--TODO: When we can inferr what kind of datatypes the database will be using for these columns, generate the DbType Attribute Property.-->
				</plx:attribute>
			</xsl:if>
			<plx:returns dataTypeName="{$conceptTypeName}"/>
			<plx:get>
				<plx:return>
					<plx:callInstance type="property" name="Entity">
						<plx:callObject>
							<plx:callThis accessor="this" type="field" name="{$privateMemberName}"/>
						</plx:callObject>
					</plx:callInstance>
				</plx:return>
			</plx:get>
			<plx:set>
				<xsl:if test="@isMandatory">
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
						<plx:throw>
							<plx:callNew dataTypeName="ArgumentNullException"/>
						</plx:throw>
					</plx:branch>
				</xsl:if>
				<plx:local name="previousValue" dataTypeName="{$conceptTypeName}">
					<plx:initialize>
						<plx:callInstance type="property" name="Entity">
							<plx:callObject>
								<plx:callThis accessor="this" type="field" name="{$privateMemberName}"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:initialize>
				</plx:local>
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="booleanOr">
							<plx:left>
								<plx:unaryOperator type="booleanNot">
									<plx:callInstance type="property" name="HasLoadedOrAssignedValue">
										<plx:callObject>
											<plx:callThis type="field" accessor="this" name="{$privateMemberName}"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:unaryOperator>
							</plx:left>
							<plx:right>
								<plx:binaryOperator type="inequality">
									<plx:left>
										<plx:nameRef name="previousValue"/>
									</plx:left>
									<plx:right>
										<plx:valueKeyword/>
									</plx:right>
								</plx:binaryOperator>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:callThis accessor="this" name="OnPropertyChanging">
						<plx:passParam>
							<plx:callThis accessor="static" type="field" name="{$conceptTypeName}PropertyChangingEventArgs"/>
						</plx:passParam>
					</plx:callThis>
					<plx:branch>
						<plx:condition>
							<plx:binaryOperator type="inequality">
								<plx:left>
									<plx:nameRef name="previousValue"/>
								</plx:left>
								<plx:right>
									<plx:nullKeyword/>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:assign>
							<plx:left>
								<plx:callInstance type="property" name="Entity">
									<plx:callObject>
										<plx:callThis type="field" name="{$privateMemberName}" accessor="this"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:left>
							<plx:right>
								<plx:nullKeyword/>
							</plx:right>
						</plx:assign>
						<plx:callInstance type="methodCall" name="Remove">
							<plx:callObject>
								<!--TODO: Need to change @name to @oppositeName when serialization is fixed.-->
								<plx:callInstance type="property" name="{@name}">
									<plx:callObject>
										<plx:nameRef name="previousValue"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:callObject>
							<plx:passParam>
								<plx:thisKeyword/>
							</plx:passParam>
						</plx:callInstance>
					</plx:branch>
					<plx:assign>
						<plx:left>
							<plx:callInstance type="property" name="Entity">
								<plx:callObject>
									<plx:callThis accessor="this" type="field" name="{$privateMemberName}"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:left>
						<plx:right>
							<plx:valueKeyword/>
						</plx:right>
					</plx:assign>
					<plx:branch>
						<plx:condition>
							<plx:binaryOperator type="inequality">
								<plx:left>
									<plx:valueKeyword/>
								</plx:left>
								<plx:right>
									<plx:nullKeyword/>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:callInstance type="methodCall" name="Add">
							<plx:callObject>
								<!--TODO: Change this reference to opposite name when serialization is fixed.-->
								<plx:callInstance type="property" name="{@name}">
									<plx:callObject>
										<plx:valueKeyword/>
									</plx:callObject>
								</plx:callInstance>
							</plx:callObject>
							<plx:passParam>
								<plx:thisKeyword/>
							</plx:passParam>
						</plx:callInstance>
						<xsl:for-each select="exsl:node-set($preferredIdentifierInformationTypesFragment)/child::*">
							<plx:assign>
								<plx:left>
									<plx:callThis accessor="this" type="field" name="{@name}"/>
								</plx:left>
								<plx:right>
									<plx:callInstance type="field" name="{@name}">
										<plx:callObject>
											<plx:valueKeyword/>
										</plx:callObject>
									</plx:callInstance>
								</plx:right>
							</plx:assign>
						</xsl:for-each>
					</plx:branch>

				</plx:branch>

			</plx:set>
		</plx:property>
	</xsl:template>

	<xsl:template match="orm:ValueType" mode="GenerateEnumerations">
		<plx:enum visibility="public" name="{@Name}">
			<plx:attribute dataTypeName="Serializable"/>
			<xsl:if test="$GenerateServiceLayer">
				<plx:attribute dataTypeName="DataContract">
					<plx:passParam>
						<plx:binaryOperator type="assignNamed">
							<plx:left>
								<plx:nameRef name="Name"/>
							</plx:left>
							<plx:right>
								<!--TODO: Set this via a settings file.-->
								<plx:string data="{@Name}"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:passParam>
				</plx:attribute>
			</xsl:if>
			<xsl:for-each select="orm:ValueRestriction/orm:ValueConstraint/orm:ValueRanges/orm:ValueRange">
				<xsl:variable name="valueConstratinMember"/>
				<plx:enumItem name="{@MinValue}">
					<xsl:if test="$GenerateServiceLayer">
						<plx:attribute dataTypeName="EnumMember">
							<plx:passParam>
								<plx:binaryOperator type="assignNamed">
									<plx:left>
										<plx:nameRef name="Value"/>
									</plx:left>
									<plx:right>
										<!--TODO: Set this via a settings file.-->
										<plx:string data="{@MinValue}"/>
									</plx:right>
								</plx:binaryOperator>
							</plx:passParam>
						</plx:attribute>
					</xsl:if>
				</plx:enumItem>
			</xsl:for-each>
		</plx:enum>
	</xsl:template>

	<xsl:template match="oial:model" mode="GenerateDatabaseContext">
		<xsl:param name="modelName" select="@name"/>
		<xsl:param name="fullyQualifiedNamespace"/>
		<plx:class visibility="public" name="{$modelName}">
			<plx:derivesFromClass dataTypeName="DataContext"/>
			<xsl:if test="$GenerateServiceLayer">
				<plx:implementsInterface dataTypeName="I{$modelName}Service"/>
			</xsl:if>
			<!--private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();-->
			<plx:field visibility="private" static="true" name="mappingSource" dataTypeName="MappingSource">
				<plx:initialize>
					<plx:callNew dataTypeName="AttributeMappingSource"/>
				</plx:initialize>
			</plx:field>
			<plx:function name=".construct" visibility="public">
				<xsl:variable name="firstConstructorParameter" select="'connection'"/>
				<plx:param name="{$firstConstructorParameter}" dataTypeName="IDbConnection"/>
				<plx:initialize>
					<plx:callThis name=".implied" accessor="base">
						<plx:passParam>
							<plx:nameRef name="{$firstConstructorParameter}" type="parameter"/>
						</plx:passParam>
					</plx:callThis>
				</plx:initialize>
			</plx:function>
			<plx:function name=".construct" visibility="public">
				<xsl:variable name="secondConstructorParameter" select="'fileOrServerConnection'"/>
				<plx:param name="{$secondConstructorParameter}" dataTypeName="string"/>
				<plx:initialize>
					<plx:callThis name=".implied" accessor="base">
						<plx:passParam>
							<plx:nameRef name="{$secondConstructorParameter}" type="parameter"/>
						</plx:passParam>
					</plx:callThis>
				</plx:initialize>
			</plx:function>
			<plx:function name=".construct" visibility="public">
				<plx:param name="connection" dataTypeName="IDbConnection"/>
				<plx:param name="mapping" dataTypeName="MappingSource"/>
				<plx:initialize>
					<plx:callThis name=".implied" accessor="base">
						<plx:passParam>
							<plx:nameRef name="connection" type="parameter"/>
						</plx:passParam>
						<plx:passParam>
							<plx:nameRef name="mapping" type="parameter"/>
						</plx:passParam>
					</plx:callThis>
				</plx:initialize>
			</plx:function>
			<plx:function name=".construct" visibility="public">
				<plx:param name="fileOrServerOrConnection" dataTypeName=".string"/>
				<plx:param name="mapping" dataTypeName="MappingSource"/>
				<plx:initialize>
					<plx:callThis name=".implied" accessor="base">
						<plx:passParam>
							<plx:nameRef name="fileOrServerOrConnection" type="parameter"/>
						</plx:passParam>
						<plx:passParam>
							<plx:nameRef name="mapping" type="parameter"/>
						</plx:passParam>
					</plx:callThis>
				</plx:initialize>
			</plx:function>
			<!--Create property accessors for all tables.-->
			<xsl:apply-templates select="oial:conceptTypes/oial:conceptType" mode="CreateDatabaseContextTableAccessorProperties"/>
			<!--Create procedure calls for all procedures.-->
		</plx:class>
	</xsl:template>

	<xsl:template match="oial:conceptType" mode="CreateDatabaseContextTableAccessorProperties">
		<plx:property name="{@name}{$TableSuffix}" visibility="public">
			<plx:returns dataTypeName="Table">
				<plx:passTypeParam dataTypeName="{@name}"/>
			</plx:returns>
			<plx:get>
				<!--return this.GetTable<{@name}>();-->
				<plx:return>
					<plx:callThis accessor="this" name="GetTable">
						<plx:passMemberTypeParam dataTypeName="{@name}"/>
					</plx:callThis>
				</plx:return>
			</plx:get>
		</plx:property>
	</xsl:template>

	<xsl:template name="GenerateINotifyPropertyChangedImplementation">
		<plx:field visibility="private" name="{$PrivateMemberPrefix}propertyChangedEventHandler" dataTypeName="PropertyChangedEventHandler"/>
		<plx:event visibility="privateInterfaceMember" name="PropertyChanged">
			<!-- Suppress the 'InterfaceMethodsShouldBeCallableByChildTypes' FxCop warning, since it is not applicable here. -->
			<!-- Child types call the property-specific notification methods, which in turn raise the INotifyPropertyChanged.PropertyChanged event. -->
			<xsl:call-template name="GenerateSuppressMessageAttribute">
				<xsl:with-param name="category" select="'Microsoft.Design'"/>
				<xsl:with-param name="checkId" select="'CA1033:InterfaceMethodsShouldBeCallableByChildTypes'"/>
			</xsl:call-template>
			<plx:interfaceMember memberName="PropertyChanged" dataTypeName="INotifyPropertyChanged"/>
			<plx:param name="sender" dataTypeName=".object"/>
			<plx:param name="e" dataTypeName="PropertyChangedEventArgs"/>
			<plx:explicitDelegateType dataTypeName="PropertyChangedEventHandler"/>
			<plx:onAdd>
				<xsl:call-template name="GetINotifyPropertyChangedImplementationEventOnAddRemoveCode">
					<xsl:with-param name="MethodName" select="'Combine'"/>
				</xsl:call-template>
			</plx:onAdd>
			<plx:onRemove>
				<xsl:call-template name="GetINotifyPropertyChangedImplementationEventOnAddRemoveCode">
					<xsl:with-param name="MethodName" select="'Remove'"/>
				</xsl:call-template>
			</plx:onRemove>
		</plx:event>
		<plx:function visibility="private" overload="{not($RaiseEventsAsynchronously)}" name="OnPropertyChanged">
			<plx:param name="e" dataTypeName="PropertyChangedEventArgs"/>
			<plx:local name="eventHandler" dataTypeName="PropertyChangedEventHandler"/>
			<plx:branch>
				<plx:condition>
					<plx:binaryOperator type="identityInequality">
						<plx:left>
							<plx:cast type="exceptionCast" dataTypeName=".object">
								<plx:inlineStatement dataTypeName="PropertyChangedEventHandler">
									<plx:assign>
										<plx:left>
											<plx:nameRef type="local" name="eventHandler"/>
										</plx:left>
										<plx:right>
											<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}propertyChangedEventHandler"/>
										</plx:right>
									</plx:assign>
								</plx:inlineStatement>
							</plx:cast>
						</plx:left>
						<plx:right>
							<plx:nullKeyword/>
						</plx:right>
					</plx:binaryOperator>
				</plx:condition>
				<xsl:variable name="commonCallCodeFragment">
					<plx:passParam>
						<plx:thisKeyword/>
					</plx:passParam>
					<plx:passParam>
						<plx:nameRef type="parameter" name="e"/>
					</plx:passParam>
				</xsl:variable>
				<xsl:variable name="commonCallCode" select="exsl:node-set($commonCallCodeFragment)/child::*"/>
				<xsl:choose>
					<xsl:when test="$RaiseEventsAsynchronously">
						<plx:callStatic name="InvokeEventHandlerAsync" dataTypeName="EventHandlerUtility" type="methodCall">
							<plx:passParam>
								<plx:nameRef type="local" name="eventHandler"/>
							</plx:passParam>
							<xsl:copy-of select="$commonCallCode"/>
						</plx:callStatic>
					</xsl:when>
					<xsl:otherwise>
						<plx:callInstance type="delegateCall" name=".implied">
							<plx:callObject>
								<plx:nameRef type="local" name="eventHandler"/>
							</plx:callObject>
							<xsl:copy-of select="$commonCallCode"/>
						</plx:callInstance>
					</xsl:otherwise>
				</xsl:choose>
			</plx:branch>
		</plx:function>
		<xsl:if test="not($RaiseEventsAsynchronously)">
			<plx:function visibility="private" overload="true" name="OnPropertyChanged">
				<plx:param name="e" dataTypeName="PropertyChangedEventArgs"/>
				<plx:local name="eventHandler" dataTypeName="PropertyChangedEventHandler"/>
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="identityInequality">
							<plx:left>
								<plx:cast type="exceptionCast" dataTypeName=".object">
									<plx:inlineStatement dataTypeName="PropertyChangedEventHandler">
										<plx:assign>
											<plx:left>
												<plx:nameRef type="local" name="eventHandler"/>
											</plx:left>
											<plx:right>
												<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}propertyChangedEventHandler"/>
											</plx:right>
										</plx:assign>
									</plx:inlineStatement>
								</plx:cast>
							</plx:left>
							<plx:right>
								<plx:nullKeyword/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:callInstance type="delegateCall" name=".implied">
						<plx:callObject>
							<plx:nameRef type="local" name="eventHandler"/>
						</plx:callObject>
						<plx:passParam>
							<plx:thisKeyword/>
						</plx:passParam>
						<plx:passParam>
							<plx:nameRef type="parameter" name="e"/>
						</plx:passParam>
					</plx:callInstance>
				</plx:branch>
			</plx:function>
		</xsl:if>
	</xsl:template>

	<xsl:template name="GenerateINotifyPropertyChangingImplementation">
		<plx:field visibility="private" name="{$PrivateMemberPrefix}propertyChangingEventHandler" dataTypeName="PropertyChangingEventHandler"/>
		<plx:event visibility="privateInterfaceMember" name="PropertyChanging">
			<!-- Suppress the 'InterfaceMethodsShouldBeCallableByChildTypes' FxCop warning, since it is not applicable here. -->
			<!-- Child types call the property-specific notification methods, which in turn raise the INotifyPropertyChanging.PropertyChanging event. -->
			<xsl:call-template name="GenerateSuppressMessageAttribute">
				<xsl:with-param name="category" select="'Microsoft.Design'"/>
				<xsl:with-param name="checkId" select="'CA1033:InterfaceMethodsShouldBeCallableByChildTypes'"/>
			</xsl:call-template>
			<plx:interfaceMember memberName="PropertyChanging" dataTypeName="INotifyPropertyChanging"/>
			<plx:param name="sender" dataTypeName=".object"/>
			<plx:param name="e" dataTypeName="PropertyChangingEventArgs"/>
			<plx:explicitDelegateType dataTypeName="PropertyChangingEventHandler"/>
			<plx:onAdd>
				<xsl:call-template name="GetINotifyPropertyChangingImplementationEventOnAddRemoveCode">
					<xsl:with-param name="MethodName" select="'Combine'"/>
				</xsl:call-template>
			</plx:onAdd>
			<plx:onRemove>
				<xsl:call-template name="GetINotifyPropertyChangingImplementationEventOnAddRemoveCode">
					<xsl:with-param name="MethodName" select="'Remove'"/>
				</xsl:call-template>
			</plx:onRemove>
		</plx:event>
		<plx:function visibility="private" overload="{not($RaiseEventsAsynchronously)}" name="OnPropertyChanging">
			<plx:param name="e" dataTypeName="PropertyChangingEventArgs"/>
			<plx:local name="eventHandler" dataTypeName="PropertyChangingEventHandler"/>
			<plx:branch>
				<plx:condition>
					<plx:binaryOperator type="identityInequality">
						<plx:left>
							<plx:cast type="exceptionCast" dataTypeName=".object">
								<plx:inlineStatement dataTypeName="PropertyChangingEventHandler">
									<plx:assign>
										<plx:left>
											<plx:nameRef type="local" name="eventHandler"/>
										</plx:left>
										<plx:right>
											<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}propertyChangingEventHandler"/>
										</plx:right>
									</plx:assign>
								</plx:inlineStatement>
							</plx:cast>
						</plx:left>
						<plx:right>
							<plx:nullKeyword/>
						</plx:right>
					</plx:binaryOperator>
				</plx:condition>
				<xsl:variable name="commonCallCodeFragment">
					<plx:passParam>
						<plx:thisKeyword/>
					</plx:passParam>
					<plx:passParam>
						<plx:nameRef type="parameter" name="e"/>
					</plx:passParam>
				</xsl:variable>
				<xsl:variable name="commonCallCode" select="exsl:node-set($commonCallCodeFragment)/child::*"/>
				<xsl:choose>
					<xsl:when test="$RaiseEventsAsynchronously">
						<plx:callStatic name="InvokeEventHandlerAsync" dataTypeName="EventHandlerUtility" type="methodCall">
							<plx:passParam>
								<plx:nameRef type="local" name="eventHandler"/>
							</plx:passParam>
							<xsl:copy-of select="$commonCallCode"/>
						</plx:callStatic>
					</xsl:when>
					<xsl:otherwise>
						<plx:callInstance type="delegateCall" name=".implied">
							<plx:callObject>
								<plx:nameRef type="local" name="eventHandler"/>
							</plx:callObject>
							<xsl:copy-of select="$commonCallCode"/>
						</plx:callInstance>
					</xsl:otherwise>
				</xsl:choose>
			</plx:branch>
		</plx:function>
		<xsl:if test="not($RaiseEventsAsynchronously)">
			<plx:function visibility="private" overload="true" name="OnPropertyChanging">
				<plx:param name="e" dataTypeName="PropertyChangingEventArgs"/>
				<plx:local name="eventHandler" dataTypeName="PropertyChangingEventHandler"/>
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="identityInequality">
							<plx:left>
								<plx:cast type="exceptionCast" dataTypeName=".object">
									<plx:inlineStatement dataTypeName="PropertyChangingEventHandler">
										<plx:assign>
											<plx:left>
												<plx:nameRef type="local" name="eventHandler"/>
											</plx:left>
											<plx:right>
												<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}propertyChangingEventHandler"/>
											</plx:right>
										</plx:assign>
									</plx:inlineStatement>
								</plx:cast>
							</plx:left>
							<plx:right>
								<plx:nullKeyword/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:callInstance type="delegateCall" name=".implied">
						<plx:callObject>
							<plx:nameRef type="local" name="eventHandler"/>
						</plx:callObject>
						<plx:passParam>
							<plx:thisKeyword/>
						</plx:passParam>
						<plx:passParam>
							<plx:nameRef type="parameter" name="e"/>
						</plx:passParam>
					</plx:callInstance>
				</plx:branch>
			</plx:function>
		</xsl:if>
	</xsl:template>

	<xsl:template name="GenerateSuppressMessageAttribute">
		<xsl:param name="category"/>
		<xsl:param name="checkId"/>
		<xsl:param name="justification"/>
		<xsl:param name="messageId"/>
		<xsl:param name="scope"/>
		<xsl:param name="target"/>
		<xsl:if test="$GenerateCodeAnalysisAttributes">
			<plx:attribute dataTypeName="SuppressMessageAttribute">
				<plx:passParam>
					<plx:string>
						<xsl:value-of select="$category"/>
					</plx:string>
				</plx:passParam>
				<plx:passParam>
					<plx:string>
						<xsl:value-of select="$checkId"/>
					</plx:string>
				</plx:passParam>
				<xsl:if test="$justification">
					<plx:passParam>
						<plx:binaryOperator type="assignNamed">
							<plx:left>
								<plx:nameRef type="namedParameter" name="Justification"/>
							</plx:left>
							<plx:right>
								<plx:string>
									<xsl:value-of select="$justification"/>
								</plx:string>
							</plx:right>
						</plx:binaryOperator>
					</plx:passParam>
				</xsl:if>
				<xsl:if test="$messageId">
					<plx:passParam>
						<plx:binaryOperator type="assignNamed">
							<plx:left>
								<plx:nameRef type="namedParameter" name="MessageId"/>
							</plx:left>
							<plx:right>
								<plx:string>
									<xsl:value-of select="$messageId"/>
								</plx:string>
							</plx:right>
						</plx:binaryOperator>
					</plx:passParam>
				</xsl:if>
				<xsl:if test="$scope">
					<plx:passParam>
						<plx:binaryOperator type="assignNamed">
							<plx:left>
								<plx:nameRef type="namedParameter" name="Scope"/>
							</plx:left>
							<plx:right>
								<plx:string>
									<xsl:value-of select="$scope"/>
								</plx:string>
							</plx:right>
						</plx:binaryOperator>
					</plx:passParam>
				</xsl:if>
				<xsl:if test="$target">
					<plx:passParam>
						<plx:binaryOperator type="assignNamed">
							<plx:left>
								<plx:nameRef type="namedParameter" name="Target"/>
							</plx:left>
							<plx:right>
								<plx:string>
									<xsl:value-of select="$target"/>
								</plx:string>
							</plx:right>
						</plx:binaryOperator>
					</plx:passParam>
				</xsl:if>
			</plx:attribute>
		</xsl:if>
	</xsl:template>

	<xsl:template name="GetINotifyPropertyChangingImplementationEventOnAddRemoveCode">
		<xsl:param name="MethodName"/>
		<plx:branch>
			<plx:condition>
				<plx:binaryOperator type="identityInequality">
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
			<xsl:choose>
				<xsl:when test="$SynchronizeEventAddRemove">
					<plx:local name="currentHandler" dataTypeName="PropertyChangingEventHandler"/>
					<plx:loop checkCondition="before">
						<plx:condition>
							<plx:binaryOperator type="identityInequality">
								<plx:left>
									<plx:cast type="exceptionCast" dataTypeName=".object">
										<plx:callStatic type="methodCall" name="CompareExchange" dataTypeName="Interlocked">
											<plx:passMemberTypeParam dataTypeName="PropertyChangingEventHandler"/>
											<plx:passParam type="inOut">
												<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}propertyChangingEventHandler"/>
											</plx:passParam>
											<plx:passParam>
												<plx:cast type="exceptionCast" dataTypeName="PropertyChangingEventHandler">
													<!-- PLIX_TODO: Once the PLiX formatters support keyword filtering, remove the dataTypeQualifier attribute from the next line. -->
													<plx:callStatic type="methodCall" name="{$MethodName}" dataTypeName="Delegate" dataTypeQualifier="System">
														<plx:passParam>
															<plx:inlineStatement dataTypeName="PropertyChangingEventHandler">
																<plx:assign>
																	<plx:left>
																		<plx:nameRef type="local" name="currentHandler"/>
																	</plx:left>
																	<plx:right>
																		<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}propertyChangingEventHandler"/>
																	</plx:right>
																</plx:assign>
															</plx:inlineStatement>
														</plx:passParam>
														<plx:passParam>
															<plx:valueKeyword/>
														</plx:passParam>
													</plx:callStatic>
												</plx:cast>
											</plx:passParam>
											<plx:passParam>
												<plx:nameRef type="local" name="currentHandler"/>
											</plx:passParam>
										</plx:callStatic>
									</plx:cast>
								</plx:left>
								<plx:right>
									<plx:cast type="exceptionCast" dataTypeName=".object">
										<plx:nameRef type="local" name="currentHandler"/>
									</plx:cast>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
					</plx:loop>
				</xsl:when>
				<xsl:otherwise>
					<plx:assign>
						<plx:left>
							<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}propertyChangingEventHandler"/>
						</plx:left>
						<plx:right>
							<plx:cast type="exceptionCast" dataTypeName="PropertyChangingEventHandler">
								<!-- PLIX_TODO: Once the PLiX formatters support keyword filtering, remove the dataTypeQualifier attribute from the next line. -->
								<plx:callStatic type="methodCall" name="Combine" dataTypeName="Delegate" dataTypeQualifier="System">
									<plx:passParam>
										<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}propertyChangingEventHandler"/>
									</plx:passParam>
									<plx:passParam>
										<plx:valueKeyword/>
									</plx:passParam>
								</plx:callStatic>
							</plx:cast>
						</plx:right>
					</plx:assign>
				</xsl:otherwise>
			</xsl:choose>
		</plx:branch>
	</xsl:template>

	<xsl:template name="GetINotifyPropertyChangedImplementationEventOnAddRemoveCode">
		<xsl:param name="MethodName"/>
		<plx:branch>
			<plx:condition>
				<plx:binaryOperator type="identityInequality">
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
			<xsl:choose>
				<xsl:when test="$SynchronizeEventAddRemove">
					<plx:local name="currentHandler" dataTypeName="PropertyChangedEventHandler"/>
					<plx:loop checkCondition="before">
						<plx:condition>
							<plx:binaryOperator type="identityInequality">
								<plx:left>
									<plx:cast type="exceptionCast" dataTypeName=".object">
										<plx:callStatic type="methodCall" name="CompareExchange" dataTypeName="Interlocked">
											<plx:passMemberTypeParam dataTypeName="PropertyChangedEventHandler"/>
											<plx:passParam type="inOut">
												<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}propertyChangedEventHandler"/>
											</plx:passParam>
											<plx:passParam>
												<plx:cast type="exceptionCast" dataTypeName="PropertyChangedEventHandler">
													<!-- PLIX_TODO: Once the PLiX formatters support keyword filtering, remove the dataTypeQualifier attribute from the next line. -->
													<plx:callStatic type="methodCall" name="{$MethodName}" dataTypeName="Delegate" dataTypeQualifier="System">
														<plx:passParam>
															<plx:inlineStatement dataTypeName="PropertyChangedEventHandler">
																<plx:assign>
																	<plx:left>
																		<plx:nameRef type="local" name="currentHandler"/>
																	</plx:left>
																	<plx:right>
																		<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}propertyChangedEventHandler"/>
																	</plx:right>
																</plx:assign>
															</plx:inlineStatement>
														</plx:passParam>
														<plx:passParam>
															<plx:valueKeyword/>
														</plx:passParam>
													</plx:callStatic>
												</plx:cast>
											</plx:passParam>
											<plx:passParam>
												<plx:nameRef type="local" name="currentHandler"/>
											</plx:passParam>
										</plx:callStatic>
									</plx:cast>
								</plx:left>
								<plx:right>
									<plx:cast type="exceptionCast" dataTypeName=".object">
										<plx:nameRef type="local" name="currentHandler"/>
									</plx:cast>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
					</plx:loop>
				</xsl:when>
				<xsl:otherwise>
					<plx:assign>
						<plx:left>
							<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}propertyChangedEventHandler"/>
						</plx:left>
						<plx:right>
							<plx:cast type="exceptionCast" dataTypeName="PropertyChangedEventHandler">
								<!-- PLIX_TODO: Once the PLiX formatters support keyword filtering, remove the dataTypeQualifier attribute from the next line. -->
								<plx:callStatic type="methodCall" name="Combine" dataTypeName="Delegate" dataTypeQualifier="System">
									<plx:passParam>
										<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}propertyChangedEventHandler"/>
									</plx:passParam>
									<plx:passParam>
										<plx:valueKeyword/>
									</plx:passParam>
								</plx:callStatic>
							</plx:cast>
						</plx:right>
					</plx:assign>
				</xsl:otherwise>
			</xsl:choose>
		</plx:branch>
	</xsl:template>

	<xsl:template name="GenerateGlobalSupportClasses">
		<plx:class visibility="public" modifier="static" name="EventHandlerUtility">
			<plx:attribute dataTypeName="HostProtectionAttribute" dataTypeQualifier="System.Security.Permissions">
				<plx:passParam>
					<plx:callStatic type="field" name="LinkDemand" dataTypeName="SecurityAction" dataTypeQualifier="System.Security.Permissions"/>
				</plx:passParam>
				<plx:passParam>
					<plx:binaryOperator type="assignNamed">
						<plx:left>
							<plx:nameRef type="namedParameter" name="SharedState"/>
						</plx:left>
						<plx:right>
							<plx:trueKeyword/>
						</plx:right>
					</plx:binaryOperator>
				</plx:passParam>
			</plx:attribute>

			<plx:function visibility="public" modifier="static" name="InvokeEventHandlerAsync">
				<plx:param name="eventHandler" dataTypeName="PropertyChangedEventHandler"/>
				<plx:param name="sender" dataTypeName=".object"/>
				<plx:param name="e" dataTypeName="PropertyChangedEventArgs"/>
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="identityEquality">
							<plx:left>
								<plx:cast type="exceptionCast" dataTypeName=".object">
									<plx:nameRef type="parameter" name="eventHandler"/>
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
								<plx:string data="eventHandler"/>
							</plx:passParam>
						</plx:callNew>
					</plx:throw>
				</plx:branch>
				<!-- PLIX_TODO: Once the PLiX formatters support keyword filtering, remove the dataTypeQualifier attribute from the next line. -->
				<plx:local name="invocationList" dataTypeName="Delegate" dataTypeQualifier="System" dataTypeIsSimpleArray="true">
					<plx:initialize>
						<plx:callInstance type="methodCall" name="GetInvocationList">
							<plx:callObject>
								<plx:nameRef type="parameter" name="eventHandler"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:initialize>
				</plx:local>
				<plx:loop checkCondition="before">
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
								<plx:callInstance type="property" name="Length">
									<plx:callObject>
										<plx:nameRef type="local" name="invocationList"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:beforeLoop>
						<plx:increment type="post">
							<plx:nameRef type="local" name="i"/>
						</plx:increment>
					</plx:beforeLoop>
					<plx:local name="currentEventHandler" dataTypeName="PropertyChangedEventHandler">
						<plx:initialize>
							<plx:cast type="exceptionCast" dataTypeName="PropertyChangedEventHandler">
								<plx:callInstance type="arrayIndexer" name=".implied">
									<plx:callObject>
										<plx:nameRef type="local" name="invocationList"/>
									</plx:callObject>
									<plx:passParam>
										<plx:nameRef type="local" name="i"/>
									</plx:passParam>
								</plx:callInstance>
							</plx:cast>
						</plx:initialize>
					</plx:local>
					<plx:callInstance type="methodCall" name="BeginInvoke">
						<plx:callObject>
							<plx:nameRef type="local" name="currentEventHandler"/>
						</plx:callObject>
						<plx:passParam>
							<plx:nameRef type="parameter" name="sender"/>
						</plx:passParam>
						<plx:passParam>
							<plx:nameRef type="parameter" name="e"/>
						</plx:passParam>
						<plx:passParam>
							<plx:callNew dataTypeName="AsyncCallback">
								<plx:passParam>
									<plx:callInstance type="methodReference" name="EndInvoke">
										<plx:callObject>
											<plx:nameRef type="local" name="currentEventHandler"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:passParam>
							</plx:callNew>
						</plx:passParam>
						<plx:passParam>
							<plx:nullKeyword/>
						</plx:passParam>
					</plx:callInstance>
				</plx:loop>
			</plx:function>
			<plx:function visibility="public" modifier="static" name="InvokeEventHandlerAsync">
				<plx:param name="eventHandler" dataTypeName="PropertyChangingEventHandler"/>
				<plx:param name="sender" dataTypeName=".object"/>
				<plx:param name="e" dataTypeName="PropertyChangingEventArgs"/>
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="identityEquality">
							<plx:left>
								<plx:cast type="exceptionCast" dataTypeName=".object">
									<plx:nameRef type="parameter" name="eventHandler"/>
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
								<plx:string data="eventHandler"/>
							</plx:passParam>
						</plx:callNew>
					</plx:throw>
				</plx:branch>
				<!-- PLIX_TODO: Once the PLiX formatters support keyword filtering, remove the dataTypeQualifier attribute from the next line. -->
				<plx:local name="invocationList" dataTypeName="Delegate" dataTypeQualifier="System" dataTypeIsSimpleArray="true">
					<plx:initialize>
						<plx:callInstance type="methodCall" name="GetInvocationList">
							<plx:callObject>
								<plx:nameRef type="parameter" name="eventHandler"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:initialize>
				</plx:local>
				<plx:loop checkCondition="before">
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
								<plx:callInstance type="property" name="Length">
									<plx:callObject>
										<plx:nameRef type="local" name="invocationList"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:beforeLoop>
						<plx:increment type="post">
							<plx:nameRef type="local" name="i"/>
						</plx:increment>
					</plx:beforeLoop>
					<plx:local name="currentEventHandler" dataTypeName="PropertyChangingEventHandler">
						<plx:initialize>
							<plx:cast type="exceptionCast" dataTypeName="PropertyChangingEventHandler">
								<plx:callInstance type="arrayIndexer" name=".implied">
									<plx:callObject>
										<plx:nameRef type="local" name="invocationList"/>
									</plx:callObject>
									<plx:passParam>
										<plx:nameRef type="local" name="i"/>
									</plx:passParam>
								</plx:callInstance>
							</plx:cast>
						</plx:initialize>
					</plx:local>
					<plx:callInstance type="methodCall" name="BeginInvoke">
						<plx:callObject>
							<plx:nameRef type="local" name="currentEventHandler"/>
						</plx:callObject>
						<plx:passParam>
							<plx:nameRef type="parameter" name="sender"/>
						</plx:passParam>
						<plx:passParam>
							<plx:nameRef type="parameter" name="e"/>
						</plx:passParam>
						<plx:passParam>
							<plx:callNew dataTypeName="AsyncCallback">
								<plx:passParam>
									<plx:callInstance type="methodReference" name="EndInvoke">
										<plx:callObject>
											<plx:nameRef type="local" name="currentEventHandler"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:passParam>
							</plx:callNew>
						</plx:passParam>
						<plx:passParam>
							<plx:nullKeyword/>
						</plx:passParam>
					</plx:callInstance>
				</plx:loop>
			</plx:function>
		</plx:class>
	</xsl:template>

	<xsl:template name="GenerateToString">
		<xsl:param name="ClassName"/>
		<xsl:param name="Properties"/>
		<xsl:variable name="nonCollectionProperties" select="$Properties[not(@isCollection='true')]"/>
		<plx:function visibility="public" modifier="override" overload="true" name="ToString">
			<plx:returns dataTypeName=".string"/>
			<plx:return>
				<plx:callThis accessor="this" type="methodCall" name="ToString">
					<plx:passParam>
						<plx:nullKeyword/>
					</plx:passParam>
				</plx:callThis>
			</plx:return>
		</plx:function>
		<plx:function visibility="public" modifier="virtual" overload="true" name="ToString">
			<plx:param name="provider" dataTypeName="IFormatProvider"/>
			<plx:returns dataTypeName=".string"/>
			<plx:return>
				<plx:callStatic name="Format" dataTypeName=".string">
					<plx:passParam>
						<plx:nameRef type="parameter" name="provider"/>
					</plx:passParam>
					<plx:passParam>
						<plx:string>
							<xsl:value-of select="concat($ClassName,'{0}{{{0}{1}')"/>
							<xsl:for-each select="$nonCollectionProperties">
								<xsl:value-of select="concat(@name,' = ')"/>
								<xsl:if test="not(@isCustomType='true')">
									<xsl:value-of select="'&quot;'"/>
								</xsl:if>
								<xsl:value-of select="concat('{',position()+1,'}')"/>
								<xsl:if test="not(@isCustomType='true')">
									<xsl:value-of select="'&quot;'"/>
								</xsl:if>
								<xsl:if test="not(position()=last())">
									<xsl:value-of select="',{0}{1}'"/>
								</xsl:if>
							</xsl:for-each>
							<xsl:value-of select="'{0}}}'"/>
						</plx:string>
					</plx:passParam>
					<plx:passParam>
						<plx:callStatic type="field" name="NewLine" dataTypeName="Environment"/>
					</plx:passParam>
					<plx:passParam>
						<plx:string>
							<xsl:text disable-output-escaping="yes">&amp;#x09;</xsl:text>
						</plx:string>
					</plx:passParam>
					<xsl:for-each select="$nonCollectionProperties">
						<plx:passParam>
							<xsl:choose>
								<xsl:when test="@isCustomType='true'">
									<plx:string>TODO: Recursively call ToString for customTypes...</plx:string>
								</xsl:when>
								<xsl:otherwise>
									<plx:callThis accessor="this" type="property" name="{@name}"/>
								</xsl:otherwise>
							</xsl:choose>
						</plx:passParam>
					</xsl:for-each>
				</plx:callStatic>
			</plx:return>
		</plx:function>
	</xsl:template>
</xsl:stylesheet>