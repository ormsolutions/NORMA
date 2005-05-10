<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:orm="http://schemas.northface.edu/orm/ormcore">
    <xsl:output method="xml" indent ="yes" encoding="utf-8"/>
    <xsl:template match="/ORM2/ORMModel">
        <xsl:element name="orm:ORMModel">
            <xsl:apply-templates select="ObjectTypeCollection"/>
            <xsl:apply-templates select="FactTypeCollection"/>
        </xsl:element>
    </xsl:template>
    <xsl:template name="Objects" match="ObjectTypeCollection">
        <xsl:element name="orm:Objects">
            <xsl:apply-templates select="ObjectType"/>
        </xsl:element>
    </xsl:template>
    <xsl:template name="EntityObjects" match="ObjectType[not(ValueTypeHasDataType) and not(NestingEntityTypeHasFactType.FactType)]">
        <xsl:element name="orm:EntityType" >
            <!--TODO: ReferenceModes-->
            <xsl:apply-templates select="@id"/>
            <xsl:apply-templates select="@Name"/>
            <xsl:apply-templates select="@IsIndependent"/>
            <xsl:element name="orm:PlayedRoles">
                <xsl:apply-templates select="ObjectTypePlaysRole.Role"/>
            </xsl:element>
        </xsl:element>
    </xsl:template>
    <xsl:template name="ValueTypeObjects" match="ObjectType[ValueTypeHasDataType]">
        <xsl:element name="orm:ValueType">
            <xsl:apply-templates select="@id"/>
            <xsl:apply-templates select="@Name"/>
            <xsl:apply-templates select="@IsIndependent"/>
            <xsl:element name="orm:PlayedRoles">
                <xsl:apply-templates select="ObjectTypePlaysRole.Role"/>
            </xsl:element>
            <xsl:apply-templates select="@TypeName"/>
        </xsl:element>
    </xsl:template>
    <xsl:template name="ObjectifiedObjects" match="ObjectType[not(ValueTypeHasDataType) and (NestingEntityTypeHasFactType.FactType)]">
        <xsl:element name="orm:ObjectifiedType" >
            <xsl:apply-templates select="@id"/>
            <xsl:apply-templates select="@Name"/>
            <xsl:apply-templates select="@IsIndependent"/>
            <xsl:element name="orm:PlayedRoles">
                <xsl:apply-templates select="ObjectTypePlaysRole.Role"/>
            </xsl:element>
            <xsl:apply-templates select="NestingEntityTypeHasFactType.FactType"/>
        </xsl:element>
    </xsl:template>
    <xsl:template match="NestingEntityTypeHasFactType.FactType">
        <xsl:element name="orm:NestedPredicate">
            <xsl:apply-templates select="@ref"/>
        </xsl:element>
    </xsl:template>
    <xsl:template match="ObjectTypePlaysRole.Role|ConstraintRoleSequenceHasRole.Role|ReadingOrderHasRole.Role">
        <xsl:element name="orm:RoleRef">
            <xsl:attribute name="ref">
            <xsl:apply-templates select="@ref"/>
            </xsl:attribute>
        </xsl:element>
    </xsl:template>
    <xsl:template match="@ref">
        <xsl:text>GUID_</xsl:text>
        <xsl:value-of select="."/>
    </xsl:template>
    <xsl:template match="@Name">
        <xsl:attribute name="Name">
            <xsl:value-of select="."/>
        </xsl:attribute>
    </xsl:template>
    <xsl:template match="@id">
        <xsl:attribute name="ID">
            <xsl:text>GUID_</xsl:text>
            <xsl:value-of select="."/>
        </xsl:attribute>
    </xsl:template>
    <xsl:template match="@IsIndependent">
        <xsl:if test =". = 'True'">
            <xsl:attribute name="IsIndependent">
                <xsl:value-of select="."/>
            </xsl:attribute>
        </xsl:if>
    </xsl:template>
    <xsl:template match="@TypeName">
        <xsl:if test =". != ''">
            <xsl:element name="orm:ConceptualDataType">
                <xsl:attribute name="Type">
                    <xsl:value-of select="."/>
                </xsl:attribute>
                <xsl:attribute name="Scale">
                    <xsl:value-of select="../ValueTypeHasDataType/@Scale"/>
                </xsl:attribute>
                <xsl:attribute name="Length">
                    <xsl:value-of select="../ValueTypeHasDataType/@Length"/>
                </xsl:attribute>
            </xsl:element>
        </xsl:if>
    </xsl:template>
    <xsl:template match="FactTypeCollection">
        <xsl:element name="orm:Facts">
            <xsl:apply-templates select="FactType"/>
        </xsl:element>
    </xsl:template>
    <xsl:template match="FactType">
        <xsl:element name="orm:Fact">
            <xsl:apply-templates select="@id"/>
            <xsl:apply-templates select="@Name"/>
            <xsl:apply-templates select="DerivationRule"/>
            <xsl:apply-templates select="ReadingOrderCollection"/>
            <xsl:apply-templates select="RoleCollection"/>
            <xsl:apply-templates select="InternalConstraintCollection"/>
            <!--Finish-->
            <xsl:element name="orm:ExternalConstraints"></xsl:element>
        </xsl:element>
    </xsl:template>
    <xsl:template match="RoleCollection">
        <xsl:element name="orm:FactRoles">
            <xsl:apply-templates select="Role"/>
        </xsl:element>
    </xsl:template>
    <xsl:template match="Role">
        <xsl:element name="orm:Role">
            <xsl:apply-templates select="@id"/>
            <xsl:apply-templates select="@Name"/>
            <xsl:apply-templates select="ObjectTypePlaysRole.ObjectType"/>
        </xsl:element>
    </xsl:template>
    <xsl:template match="ObjectTypePlaysRole.ObjectType">
        <xsl:element name="orm:RolePlayer">
            <xsl:apply-templates select="@ref"/>
        </xsl:element>
    </xsl:template>
    <xsl:template match="InternalConstraintCollection">
        <xsl:element name="orm:InternalConstraints">
            <xsl:apply-templates select="InternalUniquenessConstraint"/>
            <xsl:apply-templates select="SimpleMandatoryConstraint"/>
        </xsl:element>
    </xsl:template>
    <xsl:template match="InternalUniquenessConstraint|SimpleMandatoryConstraint">
        <xsl:element name="orm:{name()}">
            <xsl:apply-templates select="@id"/>
            <xsl:apply-templates select="@Name"/>
            <xsl:element name="orm:RoleSequence">
                <xsl:apply-templates select="ConstraintRoleSequenceHasRole.Role"/>
            </xsl:element>
        </xsl:element>
    </xsl:template>
    <xsl:template match="DerivationRule">
        <xsl:element name="orm:DerivationRule">
            <xsl:element name="orm:Data">
                <xsl:value-of  select ="."/>
            </xsl:element>
        </xsl:element>
    </xsl:template>
    <xsl:template match="ReadingOrderCollection">
        <xsl:element name="orm:ReadingOrders">
            <xsl:apply-templates select="ReadingOrder"/>
        </xsl:element>
    </xsl:template>
    <xsl:template match="ReadingOrder">
        <xsl:element name="orm:ReadingOrder">
            <xsl:apply-templates select="ReadingCollection"/>
            <xsl:element name="orm:RoleSequence">
                <xsl:apply-templates select="ReadingOrderHasRole.Role"/>
            </xsl:element>
        </xsl:element>
    </xsl:template>
    <xsl:template match="ReadingCollection">
        <xsl:element name="orm:Readings">
            <xsl:apply-templates select="Reading"/>
        </xsl:element>
    </xsl:template>
    <xsl:template match="Reading">
        <xsl:element name="orm:Reading">
            <xsl:if test ="@IsPrimary = 'True'">
                <xsl:attribute name="IsPrimary">
                    <xsl:text>true</xsl:text>
                </xsl:attribute>
            </xsl:if>
            <xsl:element name="orm:Data">
                <xsl:value-of select="@Text"/>
            </xsl:element>
        </xsl:element>
    </xsl:template>
    <xsl:template match="*">
		<xsl:copy>
			<xsl:copy-of select="@*"/>
			<xsl:value-of select="text()"/>
			<xsl:apply-templates select="child::*"/>
		</xsl:copy>
	</xsl:template>
</xsl:stylesheet>
