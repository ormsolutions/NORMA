<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:orm="http://schemas.neumont.edu/ORM/ORMCore" xmlns:ormRoot="http://schemas.neumont.edu/ORM/ORMRoot" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:loc="urn:local-temps" xmlns:xs="http://www.w3.org/2001/XMLSchema" extension-element-prefixes="msxsl" exclude-result-prefixes="loc xs">
	<xsl:param name="CoRefOppositeRoleIdDecorator" select="'_opposite'"/>
	<xsl:param name="CoRefInternalUniquenessIdDecorator" select="'_unique'"/>
	<xsl:param name="CoRefInternalUniquenessNameDecorator" select="'_unique'"/>
	<xsl:param name="CoRefSimpleMandatoryIdDecorator" select="'_mandatory'"/>
	<xsl:param name="CoRefSimpleMandatoryNameDecorator" select="'_mandatory'"/>
	<xsl:param name="CoRefFactIdDecorator" select="'_coref_fact'"/>
	<xsl:param name="CoRefFactNameDecorator" select="'_coref_fact'"/>
	<xsl:param name="CoRefValueDataTypeIdDecorator" select="'_Data_Type'"/>
	<xsl:variable name="ExistingTrueOrFalseLogicalDataType" select="ormRoot:ORM2/orm:ORMModel/orm:DataTypes/orm:TrueOrFalseLogicalDataType"/>
	<xsl:variable name="CoRefLogicalDataTypeIdDecoratorFragment">
		<xsl:choose>
			<xsl:when test="$ExistingTrueOrFalseLogicalDataType">
				<xsl:value-of select="orm:TrueOrFalseLogicalDataType/@id"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="'_true_or_false_logical_Data_Type'"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="CoRefLogicalDataTypeIdDecorator" select="string($CoRefLogicalDataTypeIdDecoratorFragment)"/>

	<xsl:output method="xml" encoding="utf-8" media-type="text/xml" indent="yes"/>
	<xsl:strip-space elements="*"/>

	<xs:schema targetNamespace="urn:local-temps" xmlns="urn:local-temps" elementFormDefault="qualified">
		<xs:element name="mappedRole" type="mappedRoleType"/>
		<xs:complexType name="mappedRoleType">
			<xs:annotation>
				<xs:documentation>
					Indicates that a role has been deleted and any reference to it
					needs to map to another role.
				</xs:documentation>
			</xs:annotation>
			<xs:attribute name="fromRoleRef" type="xs:NMTOKEN"/>
			<xs:attribute name="toRoleRef" type="xs:NMTOKEN"/>
		</xs:complexType>
	</xs:schema>

	<xsl:template match="orm:PlayedRoles" mode="ForRoles">
		<xsl:copy-of select="."/>
	</xsl:template>

	<xsl:template match="orm:ImpliedEqualityConstraint" mode="BuildRoleMap">
		<!-- The primary fact roles are stored in the first role sequence, the
			 implied roles are in the second. Any reference to the secondary role
			 needs to map to a reference to the primary role. -->
		<xsl:for-each select="orm:RoleSequences">
			<xsl:variable name="primaryRoles" select="orm:RoleSequence[1]/orm:Role"/>
			<xsl:for-each select="orm:RoleSequence[2]/orm:Role">
				<xsl:variable name="currentPosition" select="position()"/>
				<loc:mappedRole fromRoleRef="{@ref}" toRoleRef="{$primaryRoles[$currentPosition]/@ref}"/>
			</xsl:for-each>
		</xsl:for-each>
	</xsl:template>

	<!-- Set the priority low here for testing purposes. This is meant to be directly included in other xsl files, and
		 we don't want to interfere with other root matches. -->
	<xsl:template match="ormRoot:ORM2" priority="-3">
		<xsl:call-template name="CoRefORMModel">
			<xsl:with-param name="Model" select="orm:ORMModel"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template name="CoRefORMModel">
		<xsl:param name="Model" select="/ormRoot:ORM2/orm:ORMModel"/>
		<xsl:variable name="RoleMapFragment">
			<xsl:apply-templates select="$Model/orm:ExternalConstraints/orm:ImpliedEqualityConstraint" mode="BuildRoleMap"/>
		</xsl:variable>
		<xsl:variable name="RoleMap" select="msxsl:node-set($RoleMapFragment)/child::*"/>
		<xsl:variable name="ObjectifiedTypes" select="$Model/orm:Objects/orm:ObjectifiedType"/>
		<xsl:variable name="ImpliedFacts" select="$Model/orm:Facts/orm:ImpliedFact[orm:ImpliedByObjectification]" />
		<xsl:variable name="ImpliedExternalUniquenessConstraints" select="$Model/orm:ExternalConstraints/orm:ImpliedExternalUniquenessConstraint" />
		<xsl:variable name="ObjectifiedFactsFragment">
			<xsl:for-each select="$Model/orm:Facts/orm:Fact[@id=$ObjectifiedTypes/orm:NestedPredicate/@ref]">
				<xsl:copy>
					<xsl:for-each select="$ObjectifiedTypes[orm:NestedPredicate/@ref=current()/@id]">
						<xsl:attribute name="ObjectificationId">
							<xsl:value-of select="orm:NestedPredicate/@id"/>
						</xsl:attribute>
						<xsl:attribute name="ObjectifiedTypeId">
							<xsl:value-of select="@id"/>
						</xsl:attribute>
						<xsl:attribute name="PreferredIdentifier">
							<xsl:variable name="firstImpliedEUC" select="$ImpliedExternalUniquenessConstraints[orm:ImpliedByObjectification/@ref=current()/orm:NestedPredicate/@id][1]"/>
							<xsl:choose>
								<xsl:when test="$ImpliedExternalUniquenessConstraints[orm:ImpliedByObjectification/@ref=current()/orm:NestedPredicate/@id]">
									<xsl:value-of select="$firstImpliedEUC/@id"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:variable name="CurrentImpliedFact" select="$ImpliedFacts[self::orm:ImpliedFact][orm:ImpliedByObjectification/@ref=current()/orm:NestedPredicate/@id]" />
									<xsl:value-of select="$CurrentImpliedFact/orm:InternalConstraints/orm:InternalUniquenessConstraint[2]/@id"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
					</xsl:for-each>
					<xsl:copy-of select="@*|*"/>
				</xsl:copy>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="ObjectifiedFacts" select="msxsl:node-set($ObjectifiedFactsFragment)/child::*"/>
		<!-- At one point BinarizableFacts were known as MultiRoleUniquenessFactTypes. Enough said. -->
		<!-- TODO: BinarizableFacts and MultiRoleUniquenessFactTypes are not 100% equivalent; the latter is actually a subset of the former. Unaries are also binarizable, and need to be processed by this transform. -->
		<xsl:variable name="BinarizableFacts" select="$Model/orm:Facts/orm:Fact[not(@id=$ObjectifiedFacts/@id) and (orm:InternalConstraints/orm:InternalUniquenessConstraint/orm:RoleSequence[count(orm:Role)>1] or count(orm:FactRoles/orm:Role)=1)]"/>

		<xsl:apply-templates select="$Model" mode="CoRefORM">
			<xsl:with-param name="Model" select="$Model"/>
			<xsl:with-param name="RoleMap" select="$RoleMap"/>
			<xsl:with-param name="ObjectifiedFacts" select="$ObjectifiedFacts"/>
			<xsl:with-param name="BinarizableFacts" select="$BinarizableFacts"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="orm:ORMModel" mode="CoRefORM">
		<xsl:param name="Model"/>
		<xsl:param name="RoleMap"/>
		<xsl:param name="ObjectifiedFacts"/>
		<xsl:param name="BinarizableFacts"/>
		<xsl:copy>
			<xsl:apply-templates select="node()|@*" mode="CoRefORM">
				<xsl:with-param name="Model" select="$Model"/>
				<xsl:with-param name="RoleMap" select="$RoleMap"/>
				<xsl:with-param name="ObjectifiedFacts" select="$ObjectifiedFacts"/>
				<xsl:with-param name="BinarizableFacts" select="$BinarizableFacts"/>
			</xsl:apply-templates>
		</xsl:copy>
	</xsl:template>
	<xsl:template match="orm:ImpliedFact" mode="CoRefORM">
		<xsl:param name="Model"/>
		<xsl:param name="RoleMap"/>
		<xsl:param name="ObjectifiedFacts"/>
		<xsl:param name="BinarizableFacts"/>
		<orm:Fact>
			<xsl:apply-templates select="node()|@*" mode="CoRefORM">
				<xsl:with-param name="Model" select="$Model"/>
				<xsl:with-param name="RoleMap" select="$RoleMap"/>
				<xsl:with-param name="ObjectifiedFacts" select="$ObjectifiedFacts"/>
				<xsl:with-param name="BinarizableFacts" select="$BinarizableFacts"/>
			</xsl:apply-templates>
		</orm:Fact>
	</xsl:template>
	<xsl:template match="orm:ImpliedExternalUniquenessConstraint" mode="CoRefORM">
		<xsl:param name="Model"/>
		<xsl:param name="RoleMap"/>
		<xsl:param name="ObjectifiedFacts"/>
		<xsl:param name="BinarizableFacts"/>
		<orm:ExternalUniquenessConstraint>
			<xsl:apply-templates select="node()|@*" mode="CoRefORM">
				<xsl:with-param name="Model" select="$Model"/>
				<xsl:with-param name="RoleMap" select="$RoleMap"/>
				<xsl:with-param name="ObjectifiedFacts" select="$ObjectifiedFacts"/>
				<xsl:with-param name="BinarizableFacts" select="$BinarizableFacts"/>
			</xsl:apply-templates>
		</orm:ExternalUniquenessConstraint>
	</xsl:template>
	<xsl:template match="orm:ImpliedByObjectification | orm:ImpliedEqualityConstraint | orm:NestedPredicate" mode="CoRefORM"/>
	<xsl:template match="orm:ObjectifiedType" mode="CoRefORM">
		<xsl:param name="Model" />
		<xsl:param name="RoleMap"/>
		<xsl:param name="ObjectifiedFacts" />
		<xsl:param name="BinarizableFacts"/>
		<xsl:variable name="object" select="$ObjectifiedFacts[@ObjectifiedTypeId=current()/@id]"/>
		<orm:EntityType _ReferenceMode="?">
			<xsl:apply-templates select="node()|@*" mode="CoRefORM">
				<xsl:with-param name="Model" select="$Model" />
				<xsl:with-param name="RoleMap" select="$RoleMap"/>
				<xsl:with-param name="ObjectifiedFacts" select="$ObjectifiedFacts"/>
				<xsl:with-param name="BinarizableFacts" select="$BinarizableFacts"/>
			</xsl:apply-templates>
			<orm:PreferredIdentifier>
				<xsl:attribute name="ref">
					<xsl:value-of select="$object/@PreferredIdentifier"/>
				</xsl:attribute>
			</orm:PreferredIdentifier>
		</orm:EntityType>
	</xsl:template>
	<xsl:template match="*|@*|text()" name="DefaultCoRefORMTemplate" mode="CoRefORM">
		<xsl:param name="Model"/>
		<xsl:param name="RoleMap"/>
		<xsl:param name="ObjectifiedFacts"/>
		<xsl:param name="BinarizableFacts"/>
		<xsl:copy>
			<xsl:apply-templates select="node()|@*" mode="CoRefORM">
				<xsl:with-param name="Model" select="$Model"/>
				<xsl:with-param name="RoleMap" select="$RoleMap"/>
				<xsl:with-param name="ObjectifiedFacts" select="$ObjectifiedFacts"/>
				<xsl:with-param name="BinarizableFacts" select="$BinarizableFacts"/>
			</xsl:apply-templates>
		</xsl:copy>
	</xsl:template>
	<xsl:template match="orm:PlayedRoles/orm:Role" mode="CoRefORM">
		<xsl:param name="Model"/>
		<xsl:param name="RoleMap"/>
		<xsl:param name="ObjectifiedFacts"/>
		<xsl:param name="BinarizableFacts"/>
		<xsl:if test="not($RoleMap[@fromRoleRef=current()/@ref])">
			<xsl:call-template name="DefaultCoRefORMTemplate">
				<xsl:with-param name="Model" select="$Model"/>
				<xsl:with-param name="RoleMap" select="$RoleMap"/>
				<xsl:with-param name="ObjectifiedFacts" select="$ObjectifiedFacts"/>
				<xsl:with-param name="BinarizableFacts" select="$BinarizableFacts"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
	<xsl:template match="orm:Role/@ref | orm:ImpliedFact/orm:FactRoles/orm:Role/@id" mode="CoRefORM">
		<xsl:param name="Model"/>
		<xsl:param name="RoleMap"/>
		<xsl:param name="ObjectifiedFacts"/>
		<xsl:param name="BinarizableFacts"/>
		<xsl:variable name="map" select="$RoleMap[@fromRoleRef=current()]"/>
		<xsl:choose>
			<xsl:when test="$map">
				<xsl:attribute name="{local-name()}">
					<xsl:value-of select="$map/@toRoleRef"/>
				</xsl:attribute>
			</xsl:when>
			<xsl:otherwise>
				<xsl:copy-of select="."/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="orm:Facts/orm:Fact" mode="CoRefORM">
		<xsl:param name="Model"/>
		<xsl:param name="RoleMap"/>
		<xsl:param name="ObjectifiedFacts"/>
		<xsl:param name="BinarizableFacts"/>
		<xsl:variable name="factId" select="@id"/>
		<xsl:choose>
			<xsl:when test="$ObjectifiedFacts[@id=$factId]"/>
			<xsl:when test="$BinarizableFacts[@id=$factId]">
				<xsl:variable name="fact" select="."/>
				<xsl:for-each select="$fact/orm:FactRoles/orm:Role">
					<orm:Fact id="{$factId}{$CoRefFactIdDecorator}{position()}" Name="{$fact/@Name}{$CoRefFactNameDecorator}{position()}">
						<orm:FactRoles>
							<!-- UNDONE: Consider getting multiplicity right for both these roles -->
							<orm:Role id="{@id}{$CoRefOppositeRoleIdDecorator}" Name="" _IsMandatory="true">
								<orm:RolePlayer ref="{$factId}"/>
							</orm:Role>
							<xsl:copy>
								<xsl:copy-of select="@*[local-name()!='_Multiplicity']"/>
								<xsl:copy-of select="orm:RolePlayer"/>
							</xsl:copy>
						</orm:FactRoles>
						<orm:InternalConstraints>
							<xsl:copy-of select="$fact/orm:InternalConstraints/orm:SimpleMandatoryConstraint[orm:RoleSequence/orm:Role[@ref=current()/@id]]"/>
							<orm:InternalUniquenessConstraint id="{@id}{$CoRefInternalUniquenessIdDecorator}" Name="{$fact/@Name}{$CoRefInternalUniquenessNameDecorator}">
								<orm:RoleSequence>
									<orm:Role ref="{@id}{$CoRefOppositeRoleIdDecorator}"/>
								</orm:RoleSequence>
							</orm:InternalUniquenessConstraint>
							<orm:SimpleMandatoryConstraint id="{@id}{$CoRefSimpleMandatoryIdDecorator}" Name="{$fact/@Name}{$CoRefSimpleMandatoryNameDecorator}">
								<orm:RoleSequence>
									<orm:Role ref="{@id}{$CoRefOppositeRoleIdDecorator}"/>
								</orm:RoleSequence>
							</orm:SimpleMandatoryConstraint>
						</orm:InternalConstraints>
					</orm:Fact>
				</xsl:for-each>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="DefaultCoRefORMTemplate">
					<xsl:with-param name="Model" select="$Model"/>
					<xsl:with-param name="RoleMap" select="$RoleMap"/>
					<xsl:with-param name="ObjectifiedFacts" select="$ObjectifiedFacts"/>
					<xsl:with-param name="BinarizableFacts" select="$BinarizableFacts"/>
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="orm:ExternalConstraints" mode="CoRefORM">
		<xsl:param name="Model"/>
		<xsl:param name="RoleMap"/>
		<xsl:param name="ObjectifiedFacts"/>
		<xsl:param name="BinarizableFacts"/>
		<xsl:copy>
			<xsl:apply-templates select="node()|@*" mode="CoRefORM">
				<xsl:with-param name="Model" select="$Model"/>
				<xsl:with-param name="RoleMap" select="$RoleMap"/>
				<xsl:with-param name="ObjectifiedFacts" select="$ObjectifiedFacts"/>
				<xsl:with-param name="BinarizableFacts" select="$BinarizableFacts"/>
			</xsl:apply-templates>
			<xsl:for-each select="$BinarizableFacts/orm:InternalConstraints/orm:InternalUniquenessConstraint">
				<orm:ExternalUniquenessConstraint>
					<xsl:copy-of select="@id"/>
					<xsl:copy-of select="@Name"/>
					<xsl:copy-of select="@Modality"/>
					<xsl:copy-of select="orm:RoleSequence"/>
				</orm:ExternalUniquenessConstraint>
			</xsl:for-each>
		</xsl:copy>
	</xsl:template>
	<xsl:template match="orm:Objects" mode="CoRefORM">
		<xsl:param name="Model"/>
		<xsl:param name="RoleMap"/>
		<xsl:param name="ObjectifiedFacts"/>
		<xsl:param name="BinarizableFacts"/>
		<xsl:copy>
			<xsl:apply-templates select="node()|@*" mode="CoRefORM">
				<xsl:with-param name="Model" select="$Model"/>
				<xsl:with-param name="RoleMap" select="$RoleMap"/>
				<xsl:with-param name="ObjectifiedFacts" select="$ObjectifiedFacts"/>
				<xsl:with-param name="BinarizableFacts" select="$BinarizableFacts"/>
			</xsl:apply-templates>
			<xsl:for-each select="$BinarizableFacts">
				<xsl:choose>
					<xsl:when test="orm:InternalConstraints/orm:InternalUniquenessConstraint/orm:RoleSequence[count(orm:Role)>1]">
						<orm:EntityType Name="{@Name}" id="{@id}" IsIndependent="true">
							<orm:PlayedRoles>
								<xsl:for-each select="orm:FactRoles/orm:Role">
									<orm:Role ref="{@id}{$CoRefOppositeRoleIdDecorator}"/>
								</xsl:for-each>
							</orm:PlayedRoles>
							<orm:PreferredIdentifier ref="{orm:InternalConstraints/orm:InternalUniquenessConstraint[not(@Modality) or @Modality='Alethic'][1]/@id}"/>
						</orm:EntityType>
					</xsl:when>
					<xsl:otherwise>
						<orm:ValueType Name="{@Name}" id="{@id}" IsExternal="false" IsIndependent="false">
							<orm:PlayedRoles>
								<xsl:for-each select="orm:FactRoles/orm:Role">
									<orm:Role ref="{@id}{$CoRefOppositeRoleIdDecorator}"/>
								</xsl:for-each>
							</orm:PlayedRoles>
							<xsl:choose>
								<xsl:when test="$ExistingTrueOrFalseLogicalDataType">
									<orm:ConceptualDataType id="{@id}{$CoRefValueDataTypeIdDecorator}" ref="{$CoRefLogicalDataTypeIdDecorator}" Scale="0" Length="0" />
								</xsl:when>
								<xsl:otherwise>
									<!--need to add logical data type to the model-->
									<xsl:apply-templates select="orm:DataTypes" />
									<orm:ConceptualDataType id="{@id}{$CoRefValueDataTypeIdDecorator}" ref="{$CoRefLogicalDataTypeIdDecorator}" Scale="0" Length="0" />
								</xsl:otherwise>
							</xsl:choose>
						</orm:ValueType>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:for-each>
		</xsl:copy>
	</xsl:template>
	<xsl:template match="orm:DataTypes" mode="CoRefORM">
		<xsl:copy>
			<xsl:if test="not($ExistingTrueOrFalseLogicalDataType)">
				<orm:TrueOrFalseLogicalDataType id="{$CoRefLogicalDataTypeIdDecorator}" />
			</xsl:if>
			<xsl:copy-of select="@*"/>
			<xsl:copy-of select="child::*"/>
		</xsl:copy>
	</xsl:template>
</xsl:stylesheet>