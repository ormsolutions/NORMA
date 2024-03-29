﻿<?xml version="1.0" encoding="utf-8"?>
<!--
	Copyright © Neumont University. All rights reserved.

	This software is provided 'as-is', without any express or implied warranty. In no event will the authors be held liable for any damages arising from the use of this software.
	Permission is granted to anyone to use this software for any purpose, including commercial applications, and to alter it and redistribute it freely, subject to the following restrictions:
	1. The origin of this software must not be misrepresented; you must not claim that you wrote the original software. If you use this software in a product, an acknowledgment in the product documentation would be appreciated but is not required.
	2. Altered source versions must be plainly marked as such, and must not be misrepresented as being the original software.
	3. This notice may not be removed or altered from any source distribution.
-->
<!-- Contributors: Rexford Morgan, Joshua Arnold -->
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:orm="http://schemas.neumont.edu/ORM/2006-04/ORMCore"
	xmlns:exsl="http://exslt.org/common"
	xmlns:odt="http://schemas.neumont.edu/ORM/Abstraction/2007-06/DataTypes/Core"
	xmlns:oil="http://schemas.neumont.edu/ORM/Abstraction/2007-06/Core"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt" 
	xmlns:fn="urn:functions" 
	extension-element-prefixes="exsl msxsl fn"
	exclude-result-prefixes="odt oil">
	
	<xsl:param name="RequireReadingModification" select="false()"/>
	
	<xsl:output method="xml" encoding="utf-8" media-type="application/orm+xml" indent="yes"/>

	<xsl:template match="oil:model">
		<xsl:variable name="allConceptTypes" select="oil:conceptTypes/oil:conceptType"/>
		<xsl:variable name="allConceptTypeChildren" select="$allConceptTypes/oil:children/oil:*"/>
		<xsl:variable name="allAssociationChildren" select="$allConceptTypes/oil:association/oil:associationChild"/>
		<orm:ORMModel id="{fn:EncodeName(@name)}" Name="{@name}">
			<orm:DataTypes>
				<!-- Just throw in all possible data types for now. These will not be saved after the initial load if they are not used,
				so predicting data type use is not worth the effort. -->
				<orm:UnspecifiedDataType id="UnspecifiedDataType"/>
				<orm:FixedLengthTextDataType id="FixedLengthTextDataType"/>
				<orm:VariableLengthTextDataType id="VariableLengthTextDataType"/>
				<orm:LargeLengthTextDataType id="LargeLengthTextDataType"/>
				<orm:SignedIntegerNumericDataType id="SignedIntegerNumericDataType"/>
				<orm:SignedSmallIntegerNumericDataType id="SignedSmallIntegerNumericDataType"/>
				<orm:SignedLargeIntegerNumericDataType id="SignedLargeIntegerNumericDataType"/>
				<orm:UnsignedIntegerNumericDataType id="UnsignedIntegerNumericDataType"/>
				<orm:UnsignedTinyIntegerNumericDataType id="UnsignedTinyIntegerNumericDataType"/>
				<orm:UnsignedSmallIntegerNumericDataType id="UnsignedSmallIntegerNumericDataType"/>
				<orm:UnsignedLargeIntegerNumericDataType id="UnsignedLargeIntegerNumericDataType"/>
				<orm:AutoCounterNumericDataType id="AutoCounterNumericDataType"/>
				<orm:FloatingPointNumericDataType id="FloatingPointNumericDataType"/>
				<orm:SinglePrecisionFloatingPointNumericDataType id="SinglePrecisionFloatingPointNumericDataType"/>
				<orm:DoublePrecisionFloatingPointNumericDataType id="DoublePrecisionFloatingPointNumericDataType"/>
				<orm:DecimalNumericDataType id="DecimalNumericDataType"/>
				<orm:MoneyNumericDataType id="MoneyNumericDataType"/>
				<orm:FixedLengthRawDataDataType id="FixedLengthRawDataDataType"/>
				<orm:VariableLengthRawDataDataType id="VariableLengthRawDataDataType"/>
				<orm:LargeLengthRawDataDataType id="LargeLengthRawDataDataType"/>
				<orm:PictureRawDataDataType id="PictureRawDataDataType"/>
				<orm:OleObjectRawDataDataType id="OleObjectRawDataDataTypeType"/>
				<orm:AutoTimestampTemporalDataType id="AutoTimestampTemporalDataType"/>
				<orm:TimeTemporalDataType id="TimeTemporalDataType"/>
				<orm:DateTemporalDataType id="DateTemporalDataType"/>
				<orm:DateAndTimeTemporalDataType id="DateAndTimeTemporalDataType"/>
				<orm:TrueOrFalseLogicalDataType id="TrueOrFalseLogicalDataType"/>
				<orm:YesOrNoLogicalDataType id="YesOrNoLogicalDataType"/>
				<orm:RowIdOtherDataType id="RowIdOtherDataType"/>
				<orm:ObjectIdOtherDataType id="ObjectIdOtherDataType"/>
				<orm:UUIDNumericDataType id="UUIDNumericDataType"/>
			</orm:DataTypes>
			<orm:Objects>
				<xsl:apply-templates select="$allConceptTypes" mode="GenerateObjectTypes">
					<xsl:with-param name="allConceptTypes" select="$allConceptTypes"/>
					<xsl:with-param name="allInformationTypeFormats" select="oil:informationTypeFormats/child::*"/>
					<xsl:with-param name="allInformationTypes" select="$allConceptTypes/oil:children/oil:informationType"/>
				</xsl:apply-templates>
			</orm:Objects>
			<xsl:variable name="allFactTypesFragment">
				<xsl:apply-templates mode="GenerateAssociationFactTypes" select="$allConceptTypes[oil:association]">
					<xsl:with-param name="allConceptTypes" select="$allConceptTypes"/>
					<xsl:with-param name="allConceptTypeChildren" select="$allConceptTypeChildren"/>
				</xsl:apply-templates>
				<xsl:apply-templates mode="GenerateFactTypes" select="$allConceptTypes/oil:children/oil:*">
					<xsl:with-param name="allAssociationChildren" select="$allAssociationChildren"/>
				</xsl:apply-templates>
			</xsl:variable>
			<xsl:variable name="allFactTypes" select="exsl:node-set($allFactTypesFragment)/child::*"/>
			<orm:Facts>
				<xsl:copy-of select="$allFactTypes"/>
			</orm:Facts>
			<orm:Constraints>
				<xsl:apply-templates mode="GenerateConstraints" select="$allConceptTypes/oil:children/oil:* | $allConceptTypes/oil:uniquenessConstraints/oil:uniquenessConstraint"/>
			</orm:Constraints>
			<xsl:if test="$RequireReadingModification">
				<orm:ModelErrors>
					<xsl:for-each select="$allFactTypes/orm:ReadingOrders/orm:ReadingOrder/orm:Readings/orm:Reading">
						<orm:ReadingRequiresUserModificationError Name="" id="ReadingRequiresUserModificationError.{@id}">
							<orm:Reading ref="{@id}"/>
						</orm:ReadingRequiresUserModificationError>
					</xsl:for-each>
				</orm:ModelErrors>
			</xsl:if>
		</orm:ORMModel>
	</xsl:template>
	
	<!-- Match relatedConceptType or informationType and generate the appropriate FactType -->
	<xsl:template mode="GenerateFactTypes" match="oil:relatedConceptType | oil:informationType | oil:assimilatedConceptType">
		<xsl:param name="allAssociationChildren"/>
		<xsl:variable name="conceptType" select="parent::oil:children/parent::oil:conceptType"/>
		<xsl:variable name="isSubtypingRelationship" select="boolean(@refersToSubtype)"/>
		<xsl:variable name="isPartOfAssociationForParent" select="boolean($conceptType/oil:association/oil:associationChild[@ref = current()/@id])"/>
		<xsl:variable name="isPartOfAssociationForTarget" select="not($isPartOfAssociationForParent) and @id = $allAssociationChildren/@ref"/>
		<xsl:variable name="isPartOfAssociation" select="$isPartOfAssociationForParent or $isPartOfAssociationForTarget"/>
		<xsl:variable name="id" select="fn:EncodeName(@id)"/>
		<!-- $isPartOfAssociationForParent ==> proxy is for role played by target -->
		<!-- $isPartOfAssociationForTarget ==> proxy is for role played by parent -->
		<xsl:if test="$isPartOfAssociation and $isSubtypingRelationship">
			<xsl:message terminate="yes">
				<xsl:text>SANITY CHECK: Can't be part of an association and a subtyping relationship at the same time.</xsl:text>
			</xsl:message>
		</xsl:if>
		<xsl:variable name="elementName">
			<xsl:choose>
				<xsl:when test="$isSubtypingRelationship">
					<xsl:value-of select="'SubtypeFact'"/>
				</xsl:when>
				<xsl:when test="$isPartOfAssociation">
					<xsl:value-of select="'ImpliedFact'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="'Fact'"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:element name="orm:{$elementName}">
			<xsl:attribute name="id">
				<xsl:text>FactType.</xsl:text>
				<xsl:value-of select="$id"/>
			</xsl:attribute>
			<xsl:variable name="rolesFragment">
				<!-- Handle the parent -->
				<xsl:choose>
					<xsl:when test="$isSubtypingRelationship">
						<orm:SupertypeMetaRole id="Role.Parent.{$id}" Name="">
							<xsl:apply-templates select="$conceptType" mode="GenerateRolePlayer"/>
						</orm:SupertypeMetaRole>
					</xsl:when>
					<xsl:when test="$isPartOfAssociationForTarget">
						<!-- Make the proxy for the parent -->
						<orm:RoleProxy id="Role.Proxy.{$id}">
							<orm:Role ref="Role.Parent.{$id}"/>
						</orm:RoleProxy>
					</xsl:when>
					<xsl:otherwise>
						<!-- Make the role for the parent -->
						<orm:Role id="Role.Parent.{$id}" Name="">
							<xsl:variable name="oppositeName" select="string(@oppositeName)"/>
							<!-- UNDONE: Don't insert the name if it is the same as the role player name. -->
							<xsl:if test="$oppositeName">
								<xsl:attribute name="Name">
									<xsl:value-of select="$oppositeName"/>
								</xsl:attribute>
							</xsl:if>
							<xsl:apply-templates select="$conceptType" mode="GenerateRolePlayer"/>
						</orm:Role>
					</xsl:otherwise>
				</xsl:choose>

				<!-- Handle the target -->
				<xsl:choose>
					<xsl:when test="$isSubtypingRelationship">
						<orm:SubtypeMetaRole id="Role.Target.{$id}" Name="">
							<xsl:apply-templates select="." mode="GenerateRolePlayer"/>
						</orm:SubtypeMetaRole>
					</xsl:when>
					<xsl:when test="$isPartOfAssociationForParent">
						<!-- Make the proxy for the target -->
						<orm:RoleProxy id="Role.Proxy.{$id}">
							<orm:Role ref="Role.Target.{$id}"/>
						</orm:RoleProxy>
					</xsl:when>
					<xsl:otherwise>
						<!-- Make the role for the target -->
						<orm:Role id="Role.Target.{$id}" Name="">
							<xsl:variable name="name" select="string(@name)"/>
							<!-- UNDONE: Don't insert the name if it is the same as the role player name. -->
							<xsl:if test="$name">
								<xsl:attribute name="Name">
									<xsl:value-of select="$name"/>
								</xsl:attribute>
							</xsl:if>
							<xsl:apply-templates select="." mode="GenerateRolePlayer"/>
						</orm:Role>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="roles" select="exsl:node-set($rolesFragment)/child::*"/>
			<orm:FactRoles>
				<xsl:copy-of select="$roles"/>
			</orm:FactRoles>
			<xsl:if test="not($isSubtypingRelationship)">
				<orm:ReadingOrders>
					<orm:ReadingOrder id="ReadingOrder.ParentTarget.{$id}">
						<orm:Readings>
							<orm:Reading id="Reading.ParentTarget.{$id}">
								<orm:Data>
									<xsl:text>{0} has {1}</xsl:text>
								</orm:Data>
							</orm:Reading>
						</orm:Readings>
						<orm:RoleSequence>
							<orm:Role ref="{$roles[1]/@id}"/>
							<orm:Role ref="{$roles[2]/@id}"/>
						</orm:RoleSequence>
					</orm:ReadingOrder>
				</orm:ReadingOrders>
			</xsl:if>
			<xsl:if test="$isPartOfAssociationForParent">
				<orm:ImpliedByObjectification ref="NestedPredicate.{fn:EncodeName($conceptType/@id)}"/>
			</xsl:if>
			<xsl:if test="$isPartOfAssociationForTarget">
				<orm:ImpliedByObjectification ref="NestedPredicate.{fn:EncodeName(@ref)}"/>
			</xsl:if>
		</xsl:element>
	</xsl:template>

	
	<xsl:template match="oil:conceptType | oil:informationType" mode="GenerateRolePlayer">
		<orm:RolePlayer ref="ObjectType.{fn:EncodeName(@id)}"/>
	</xsl:template>
	<xsl:template match="oil:relatedConceptType | oil:assimilatedConceptType" mode="GenerateRolePlayer">
		<orm:RolePlayer ref="ObjectType.{fn:EncodeName(@ref)}"/>
	</xsl:template>


	<xsl:template match="oil:conceptType[oil:association]" mode="GenerateAssociationFactTypes">
		<xsl:param name="allConceptTypes"/>
		<xsl:param name="allConceptTypeChildren"/>
		<xsl:variable name="conceptType" select="."/>
		<xsl:variable name="conceptTypeId" select="fn:EncodeName(@id)"/>
		<orm:Fact id="FactType.Association.{$conceptTypeId}">
			<xsl:variable name="rolesFragment">
				<xsl:for-each select="oil:association/oil:associationChild">
					<xsl:variable name="conceptTypeChild" select="$allConceptTypeChildren[@id = current()/@ref]"/>
					<xsl:variable name="conceptTypeChildId" select="fn:EncodeName($conceptTypeChild/@id)"/>
					<xsl:choose>
						<xsl:when test="$conceptType/oil:children/oil:*[@id = current()/@ref]">
							<!-- The association child is one of our children -->
							<orm:Role id="Role.Target.{$conceptTypeChildId}" Name="{$conceptTypeChild/@name}">
								<xsl:apply-templates select="$conceptTypeChild" mode="GenerateRolePlayer"/>
							</orm:Role>
						</xsl:when>
						<xsl:otherwise>
							<!-- The association child is NOT one of our children, which means it targets us -->
							<orm:Role id="Role.Parent.{$conceptTypeChildId}" Name="{$conceptTypeChild/@oppositeName}">
								<xsl:apply-templates select="$allConceptTypes[oil:children/oil:*/@id = $conceptTypeChild/@id]" mode="GenerateRolePlayer"/>
							</orm:Role>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:for-each>
			</xsl:variable>
			<xsl:variable name="roles" select="exsl:node-set($rolesFragment)/child::*"/>
			<orm:FactRoles>
				<xsl:copy-of select="$roles"/>
			</orm:FactRoles>
			<!-- UNDONE: Readings with only spaces don't help, just leave them empty for associations
			until we can figure out how to do something real. -->
			<!-- <orm:ReadingOrders>
				<orm:ReadingOrder id="ReadingOrder.Association.{@id}">
					<orm:Readings>
						<orm:Reading id="Reading.Association.{@id}">
							<orm:Data>
								<xsl:for-each select="oil:association/oil:associationChild">
									<xsl:text>{</xsl:text>
									<xsl:value-of select="position() - 1"/>
									<xsl:text>}</xsl:text>
									<xsl:if test="position() != last()">
										<xsl:text> </xsl:text>
									</xsl:if>
								</xsl:for-each>
							</orm:Data>
						</orm:Reading>
					</orm:Readings>
					<orm:RoleSequence>
						<xsl:for-each select="$roles">
							<orm:Role ref="{@id}"/>
						</xsl:for-each>
					</orm:RoleSequence>
				</orm:ReadingOrder>
			</orm:ReadingOrders> -->
		</orm:Fact>
	</xsl:template>
	
	<!-- Match each conceptType to generate EntityTypes and cascade down to each child informationType -->
	<xsl:template mode="GenerateObjectTypes" match="oil:conceptType">
		<xsl:param name="allConceptTypes"/>
		<xsl:param name="allInformationTypeFormats"/>
		<xsl:param name="allInformationTypes"/>
		<xsl:variable name="id" select="fn:EncodeName(@id)"/>
		<xsl:choose>
			<xsl:when test="oil:association">
				<orm:ObjectifiedType id="ObjectType.{$id}" Name="{@name}" IsIndependent="true">
					<!-- We need to be explicitly objectified if any of our children are not part of the association, or if there are any references to us elsewhere. -->
					<xsl:variable name="needsExplicitObjectification" select="oil:children/oil:*[not(@id = current()/oil:association/oil:associationChild/@ref)] or $allConceptTypes/oil:children[oil:relatedConceptType[@ref = current()/@id] or oil:assimilatedConceptType[@ref = current()/@id]]"/>
					<orm:NestedPredicate id="NestedPredicate.{$id}" ref="FactType.Association.{$id}" IsImplied="{not($needsExplicitObjectification)}"/>
				</orm:ObjectifiedType>
			</xsl:when>
			<xsl:otherwise>
				<orm:EntityType id="ObjectType.{$id}" Name="{@name}" />
			</xsl:otherwise>
		</xsl:choose>
		<xsl:apply-templates select="oil:children/oil:informationType" mode="GenerateObjectTypes">
			<xsl:with-param name="allConceptTypes" select="$allConceptTypes"/>
			<xsl:with-param name="allInformationTypeFormats" select="$allInformationTypeFormats"/>
			<xsl:with-param name="allInformationTypes" select="$allInformationTypes"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template mode="GenerateObjectTypes" match="oil:informationType">
		<xsl:param name="allConceptTypes"/>
		<xsl:param name="allInformationTypeFormats"/>
		<xsl:param name="allInformationTypes"/>
		<xsl:variable name="conceptTypeName" select="string(parent::oil:children/parent::oil:conceptType/@name)"/>
		<xsl:variable name="id" select="fn:EncodeName(@id)"/>
		<orm:ValueType id="ObjectType.{$id}">
			<xsl:attribute name="Name">
				<xsl:if test="(count($allInformationTypes[@name = current()/@name]) + count($allConceptTypes[@name = current()/@name])) > 1">
					<xsl:value-of select="$conceptTypeName"/>
					<xsl:text>_</xsl:text>
				</xsl:if>
				<xsl:value-of select="@name"/>
			</xsl:attribute>
			<orm:ConceptualDataType id="ConceptualDataType.{$id}">
				<xsl:for-each select="$allInformationTypeFormats[@id = current()/@ref]">
					<xsl:apply-templates select="." mode="GenerateDataTypeRef"/>
					<xsl:if test="@generated[.='true' or .='1']">
						<xsl:attribute name="AutoGenerated">
							<xsl:text>true</xsl:text>
						</xsl:attribute>
					</xsl:if>
				</xsl:for-each>
			</orm:ConceptualDataType>
		</orm:ValueType>
	</xsl:template>

	<xsl:template match="odt:boolean" mode="GenerateDataTypeRef">
		<xsl:attribute name="ref">
			<xsl:text>TrueOrFalseLogicalDataType</xsl:text>
		</xsl:attribute>
	</xsl:template>
	<xsl:template match="odt:string" mode="GenerateDataTypeRef">
		<xsl:variable name="length" select="number(@maxLength)"/>
		<xsl:attribute name="ref">
			<xsl:choose>
				<xsl:when test="$length and number(@minLength)=$length">
					<xsl:text>FixedLengthTextDataType</xsl:text>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text>VariableLengthTextDataType</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:attribute>
		<xsl:if test="$length">
			<xsl:attribute name="Length">
				<xsl:value-of select="$length"/>
			</xsl:attribute>
		</xsl:if>
	</xsl:template>
	<xsl:template match="odt:decimalNumber" mode="GenerateDataTypeRef">
		<xsl:variable name="scale" select="number(@fractionDigits)"/>
		<xsl:variable name="length" select="number(@totalDigits)"/>
		<xsl:attribute name="ref">
			<xsl:choose>
				<xsl:when test="$scale=4 and ($length=10 or $length=19)">
					<!-- Hijack known combinations to represent money and small money -->
					<xsl:text>MoneyNumericDataType</xsl:text>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text>DecimalNumericDataType</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:attribute>
		<xsl:attribute name="Length">
			<xsl:choose>
				<xsl:when test="$length">
					<xsl:value-of select="$length"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:choose>
						<xsl:when test="$scale">
							<xsl:value-of select="18-$scale"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:text>18</xsl:text>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:attribute>
		<xsl:attribute name="Scale">
			<xsl:choose>
				<xsl:when test="$scale">
					<xsl:value-of select="$scale"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text>0</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:attribute>
	</xsl:template>
	<xsl:template match="odt:integerNumber" mode="GenerateDataTypeRef">
		<xsl:variable name="bytes" select="number(@bytes)"/>
		<xsl:variable name="sizeFragment">
			<xsl:choose>
				<xsl:when test="$bytes=1">
					<xsl:text>Tiny</xsl:text>
				</xsl:when>
				<xsl:when test="$bytes=2">
					<xsl:text>Small</xsl:text>
				</xsl:when>
				<xsl:when test="$bytes=4">
					<xsl:text></xsl:text>
				</xsl:when>
				<xsl:when test="$bytes=8">
					<xsl:text>Big</xsl:text>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="signedFragment">
			<xsl:choose>
				<xsl:when test="@unsigned[.='true' or .='1']">
					<xsl:text>Unsigned</xsl:text>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text>Signed</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:attribute name="ref">
			<xsl:value-of select="string($signedFragment)"/>
			<xsl:value-of select="string($sizeFragment)"/>
			<xsl:text>IntegerNumericDataType</xsl:text>
		</xsl:attribute>
	</xsl:template>
	<xsl:template match="odt:floatingPointNumber" mode="GenerateDataTypeRef">
		<xsl:variable name="isSingle" select="@precision=24"/>
		<xsl:variable name="isDouble" select="@precision=53"/>
		<xsl:choose>
			<xsl:when test="@precision=24">
				<xsl:attribute name="ref">
					<xsl:text>SinglePrecisionFloatingPointNumericDataType</xsl:text>
				</xsl:attribute>
			</xsl:when>
			<xsl:when test="@precision=53">
				<xsl:attribute name="ref">
					<xsl:text>DoublePrecisionFloatingPointNumericDataType</xsl:text>
				</xsl:attribute>
			</xsl:when>
			<xsl:otherwise>
				<xsl:attribute name="ref">
					<xsl:text>FloatingPointNumericDataType</xsl:text>
				</xsl:attribute>
				<xsl:if test="@precision">
					<xsl:attribute name="Length">
						<xsl:value-of select="@precision"/>
					</xsl:attribute>
				</xsl:if>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="odt:binary" mode="GenerateDataTypeRef">
		<xsl:variable name="length" select="number(@maxLength)"/>
		<xsl:attribute name="ref">
			<xsl:choose>
				<xsl:when test="$length and number(@minLength)=$length">
					<xsl:text>FixedLengthRawDataDataType</xsl:text>
				</xsl:when>
				<xsl:when test="$length">
					<xsl:text>VariableLengthRawDataDataType</xsl:text>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text>LargeLengthRawDataDataType</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:attribute>
		<xsl:if test="$length">
			<xsl:attribute name="Length">
				<xsl:value-of select="$length"/>
			</xsl:attribute>
		</xsl:if>
	</xsl:template>
	<xsl:template match="odt:uniqueIdentifier" mode="GenerateDataTypeRef">
		<xsl:attribute name="ref">
			<xsl:text>UUIDNumericDataType</xsl:text>
		</xsl:attribute>
	</xsl:template>
	<xsl:template match="*" mode="GenerateDataTypeRef">
		<xsl:message terminate="yes">
			<xsl:text>ERROR: Unsupported data type '</xsl:text>
			<xsl:value-of select="local-name()"/>
			<xsl:text>' with name '</xsl:text>
			<xsl:value-of select="@name"/>
			<xsl:text>'.</xsl:text>
		</xsl:message>
	</xsl:template>
	
	
	<!-- Templates to generate Constraints -->
	<xsl:template mode="GenerateConstraints" match="oil:informationType | oil:relatedConceptType | oil:assimilatedConceptType">
		<xsl:variable name="conceptType" select="parent::oil:children/parent::oil:conceptType"/>
		<xsl:variable name="conceptTypeName" select="string($conceptType/@name)"/>
		<xsl:variable name="id" select="fn:EncodeName(@id)"/>
		<orm:UniquenessConstraint id="UniquenessConstraint.Parent.{$id}" Name="UniquenessConstraint.Parent.{$conceptTypeName}.{@name}" IsInternal="true">
			<orm:RoleSequence>
				<orm:Role ref="Role.Parent.{$id}"/>
			</orm:RoleSequence>
			<xsl:if test="self::oil:assimilatedConceptType/@isPreferredForTarget">
				<orm:PreferredIdentifierFor ref="ObjectType.{fn:EncodeName(@ref)}"/>
			</xsl:if>
		</orm:UniquenessConstraint>
		<xsl:if test="@isMandatory">
			<orm:MandatoryConstraint id="MandatoryConstraint.Parent.{$id}" Name="MandatoryConstraint.Parent.{$conceptTypeName}.{@name}" IsSimple="true">
				<orm:RoleSequence>
					<orm:Role ref="Role.Parent.{$id}"/>
				</orm:RoleSequence>
			</orm:MandatoryConstraint>
		</xsl:if>
		<xsl:if test="self::oil:assimilatedConceptType">
			<orm:UniquenessConstraint id="UniquenessConstraint.Target.{$id}" Name="UniquenessConstraint.Target.{$conceptTypeName}.{@name}" IsInternal="true">
				<orm:RoleSequence>
					<orm:Role ref="Role.Target.{$id}"/>
				</orm:RoleSequence>
				<xsl:if test="@isPreferredForParent">
					<orm:PreferredIdentifierFor ref="ObjectType.{fn:EncodeName($conceptType/@id)}"/>
				</xsl:if>
			</orm:UniquenessConstraint>
			<orm:MandatoryConstraint id="MandatoryConstraint.Target.{$id}" Name="MandatoryConstraint.Target.{$conceptTypeName}.{@name}" IsSimple="true">
				<orm:RoleSequence>
					<orm:Role ref="Role.Target.{$id}"/>
				</orm:RoleSequence>
			</orm:MandatoryConstraint>
		</xsl:if>
	</xsl:template>
	<xsl:template mode="GenerateConstraints" match="oil:uniquenessConstraint">
		<xsl:variable name="uniquenessConstraintId" select="fn:EncodeName(@id)"/>
		<xsl:variable name="conceptType" select="parent::oil:uniquenessConstraints/parent::oil:conceptType"/>
		<xsl:variable name="association" select="$conceptType/oil:association"/>
		<!-- The uniqueness constraint is internal if it is over only a single child, or if this concept type is an association and ALL children of the uniqueness constraint are in the association. -->
		<orm:UniquenessConstraint id="UniquenessConstraint.{$uniquenessConstraintId}" Name="{@name}" IsInternal="{(count(oil:uniquenessChild) = 1) or ($association and not(oil:uniquenessChild[not(@ref = $association/oil:associationChild/@ref)]))}">
			<orm:RoleSequence>
				<xsl:for-each select="oil:uniquenessChild">
					<xsl:variable name="ref" select="fn:EncodeName(@ref)"/>
					<orm:Role id="UniquenessConstraintRoleReference.{$uniquenessConstraintId}.{$ref}" ref="Role.Target.{$ref}"/>
				</xsl:for-each>
			</orm:RoleSequence>
			<xsl:if test="@isPreferred">
				<orm:PreferredIdentifierFor ref="ObjectType.{fn:EncodeName($conceptType/@id)}"/>
			</xsl:if>
		</orm:UniquenessConstraint>
	</xsl:template>
	<msxsl:script implements-prefix="fn" language="CSharp">
		<![CDATA[
		public static string EncodeName(string value)
		{
			return System.Xml.XmlConvert.EncodeName(value);
		}
		]]>
	</msxsl:script>
</xsl:stylesheet>