<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" 
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns="http://schemas.microsoft.com/VisualStudio/2005/DslTools/DslDefinitionModel"
	xmlns:exsl="http://exslt.org/common"
	xmlns:oil="http://schemas.orm.net/OIAL"
	xmlns:odt="http://schemas.orm.net/ORMDataTypes"
	extension-element-prefixes="exsl"
	exclude-result-prefixes="oil odt">

	<xsl:param name="DefaultNamespace" select="''"/>
	<!--<xsl:param name="dslVersion" select="1.0.0.0"/>-->
	<xsl:param name="ModelNamespace" select="concat($DefaultNamespace,/oil:model/@name)"/>
	<xsl:output method="xml" encoding="utf-8" media-type="text/xml" indent="yes"/>
	<xsl:strip-space elements="*"/>

	<xsl:template match="/">
		<xsl:apply-templates select="child::*"/>
	</xsl:template>
	<xsl:variable name="conceptTypes" select="//oil:conceptType"/>
	<xsl:variable name="manyToManyRelationships" select="$conceptTypes[oil:conceptTypeRef/@name = oil:roleSequenceUniquenessConstraint[@isPreferred='true' and count(oil:roleSequence/oil:typeRef)=2]/oil:roleSequence/oil:typeRef[1]/@targetChild and oil:conceptTypeRef/@name = oil:roleSequenceUniquenessConstraint[@isPreferred='true' and count(oil:roleSequence/oil:typeRef)=2]/oil:roleSequence/oil:typeRef[2]/@targetChild]"/>
	<xsl:variable name="domainEnumerationsFragment">
		<xsl:apply-templates select="oil:informationTypeFormats/child::*" mode="GenerateType"/>
	</xsl:variable>
	<xsl:variable name="domainEnumerations" select="exsl:node-set($domainEnumerationsFragment)/child::*"/>

	<xsl:template match="oil:model">
		<Dsl dslVersion="1.0.0.0" Name="{@name}" Namespace="{$ModelNamespace}">
			<xsl:apply-templates select="@sourceRef" mode="GenerateId"/>
			<Classes>
				<xsl:apply-templates select="$conceptTypes[not(@name=$manyToManyRelationships/@name)]" mode="GenerateDomainClass"/>
			</Classes>
			<Relationships>
				<xsl:apply-templates select="$manyToManyRelationships" mode="GenerateDomainRelationship"/>
			</Relationships>
			<Types>
				<ExternalType Namespace="System" Name="String"/>
				<ExternalType Namespace="System" Name="Byte[]"/>

				<ExternalType Namespace="System" Name="Boolean"/>
				<ExternalType Namespace="System" Name="Byte"/>
				<ExternalType Namespace="System" Name="Int16"/>
				<ExternalType Namespace="System" Name="Int32"/>
				<ExternalType Namespace="System" Name="Int64"/>
				<ExternalType Namespace="System" Name="SByte"/>
				<ExternalType Namespace="System" Name="UInt16"/>
				<ExternalType Namespace="System" Name="UInt32"/>
				<ExternalType Namespace="System" Name="UInt64"/>
				<ExternalType Namespace="System" Name="Decimal"/>
				<ExternalType Namespace="System" Name="Single"/>
				<ExternalType Namespace="System" Name="Double"/>

				<ExternalType Namespace="System" Name="Nullable&lt;global::System.Boolean&gt;"/>
				<ExternalType Namespace="System" Name="Nullable&lt;global::System.Byte&gt;"/>
				<ExternalType Namespace="System" Name="Nullable&lt;global::System.Int16&gt;"/>
				<ExternalType Namespace="System" Name="Nullable&lt;global::System.Int32&gt;"/>
				<ExternalType Namespace="System" Name="Nullable&lt;global::System.Int64&gt;"/>
				<ExternalType Namespace="System" Name="Nullable&lt;global::System.SByte&gt;"/>
				<ExternalType Namespace="System" Name="Nullable&lt;global::System.UInt16&gt;"/>
				<ExternalType Namespace="System" Name="Nullable&lt;global::System.UInt32&gt;"/>
				<ExternalType Namespace="System" Name="Nullable&lt;global::System.UInt64&gt;"/>
				<ExternalType Namespace="System" Name="Nullable&lt;global::System.Decimal&gt;"/>
				<ExternalType Namespace="System" Name="Nullable&lt;global::System.Single&gt;"/>
				<ExternalType Namespace="System" Name="Nullable&lt;global::System.Double&gt;"/>

				<xsl:copy-of select="$domainEnumerations"/>
			</Types>
			<Shapes>

			</Shapes>
			<Connectors>

			</Connectors>
			<XmlSerializationBehavior Name="{@name}SerializationBehavior" Namespace="{$ModelNamespace}"/>
		</Dsl>
	</xsl:template>

	<xsl:template match="odt:string" mode="GenerateType">
		<xsl:choose>
			<xsl:when test="odt:enumeration and not(odt:pattern)">
				<DomainEnumeration Name="{@name}" AccessModifier="Public" Namespace="{$ModelNamespace}">
					<Literals>
						<xsl:for-each select="odt:enumeration">
							<EnumerationLiteral Name="{@value}"/>
						</xsl:for-each>
					</Literals>
				</DomainEnumeration>
			</xsl:when>
			<xsl:when test="odt:pattern">
				<!-- TODO: Not supported. -->
			</xsl:when>
			<xsl:when test="@maxLength">
				<!-- TODO: Not supported. -->
			</xsl:when>
			<xsl:when test="@minLength">
				<!-- TODO: Not supported. -->
			</xsl:when>
			<xsl:otherwise>

			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="odt:boolean" mode="GenerateType">
		<!-- TODO: Not supported. -->
	</xsl:template>
	<xsl:template match="odt:decimalNumber" mode="GenerateType">
		<xsl:choose>
			<xsl:when test="odt:enumeration and @fractionDigits=0 and not(odt:range)">
				<DomainEnumeration Name="{@name}" AccessModifier="Public" Namespace="{$ModelNamespace}">
					<Literals>
						<xsl:for-each select="odt:enumeration">
							<EnumerationLiteral Name="Literal{@value}" Value="{@value}"/>
						</xsl:for-each>
					</Literals>
				</DomainEnumeration>
			</xsl:when>
			<xsl:when test="odt:range">
				<!-- TODO: Not supported. -->
			</xsl:when>
			<xsl:otherwise>

			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="odt:floatingPointNumber" mode="GenerateType">
		<xsl:choose>
			<xsl:when test="odt:enumeration">
				<!-- TODO: Not supported. -->
			</xsl:when>
			<xsl:when test="odt:pattern">
				<!-- TODO: Not supported. -->
			</xsl:when>
			<xsl:otherwise>

			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="odt:binary" mode="GenerateType">
		<!-- TODO: Not supported. -->
	</xsl:template>

	<xsl:template match="@sourceRef|@sourceRoleRef" mode="GenerateId">
		<xsl:attribute name="Id">
			<xsl:value-of select="substring-after(.,'_')"/>
		</xsl:attribute>
	</xsl:template>

	<xsl:template match="oil:conceptType" mode="GenerateDomainClass">
		<DomainClass Name="{@name}" Namespace="{$ModelNamespace}">
			<xsl:apply-templates select="@sourceRef" mode="GenerateId"/>
			<xsl:if test="oil:informationType">
				<Properties>
					<xsl:apply-templates select="oil:informationType" mode="GenerateDomainProperty"/>
				</Properties>
			</xsl:if>
			<xsl:apply-templates select="oil:conceptType" mode="GenerateElementMergeDirectives"/>
		</DomainClass>
	</xsl:template>

	<xsl:template match="oil:conceptType" mode="GenerateDomainRelationship">
		<DomainRelationship Name="{@name}" Namespace="{$ModelNamespace}">
			<xsl:apply-templates select="@sourceRef" mode="GenerateId"/>
			<xsl:if test="oil:informationType">
				<Properties>
					<xsl:apply-templates select="oil:informationType" mode="GenerateDomainProperty"/>
				</Properties>
			</xsl:if>
			<xsl:variable name="domainRoles" select="oil:conceptTypeRef[@name=current()/oil:roleSequenceUniquenessConstraint[@isPreferred='true']/oil:roleSequence/oil:typeRef/@targetChild]"/>
			<Source>
				<DomainRole Name="{$domainRoles[1]/@name}" PropertyName="{$domainRoles[2]/@name}Collection">
					<xsl:attribute name="Multiplicity">
						<xsl:choose>
							<xsl:when test="@mandatory='alethic'">
								<xsl:value-of select="'OneMany'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="'ZeroMany'"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:apply-templates select="@sourceRoleRef" mode="GenerateId"/>
					<RolePlayer>
						<DomainClassMoniker Name="{$domainRoles[1]/@target}"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="{$domainRoles[2]/@name}" PropertyName="{$domainRoles[1]/@name}Collection">
					<xsl:attribute name="Multiplicity">
						<xsl:choose>
							<xsl:when test="@mandatory='alethic'">
								<xsl:value-of select="'OneMany'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="'ZeroMany'"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:apply-templates select="@sourceRoleRef" mode="GenerateId"/>
					<RolePlayer>
						<DomainClassMoniker Name="{$domainRoles[2]/@target}"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
	</xsl:template>

	<xsl:template match="oil:informationType" mode="GenerateDomainProperty">
		<xsl:variable name="informationTypeFormat" select="/oil:model/oil:informationTypeFormats/child::*[@name=current()/@formatRef]"/>
		<xsl:if test="not($informationTypeFormat/self::odt:identity)">
			<DomainProperty Name="{@name}">
				<xsl:apply-templates select="@sourceRoleRef" mode="GenerateId"/>
				<Type>
					<xsl:call-template name="GetTypeMoniker">
						<xsl:with-param name="InformationTypeFormat" select="$informationTypeFormat"/>
						<xsl:with-param name="Modality" select="@mandatory"/>
					</xsl:call-template>
				</Type>
			</DomainProperty>
		</xsl:if>
	</xsl:template>
	<xsl:template name="GetTypeMoniker">
		<xsl:param name="InformationTypeFormat"/>
		<xsl:param name="Modality"/>
		<xsl:variable name="domainEnumeration" select="$domainEnumerations[@name=$InformationTypeFormat/@name]"/>
		<xsl:choose>
			<xsl:when test="$domainEnumeration">
				<DomainEnumerationMoniker Name="{$domainEnumeration/@name}"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="$InformationTypeFormat" mode="GenerateExternalTypeMoniker">
					<xsl:with-param name="Modality" select="$Modality"/>
				</xsl:apply-templates>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="GenerateExternalTypeMonikerForValueType">
		<!-- If support for types outside of the system namespace is needed, this will need to be extended. -->
		<xsl:param name="Modality"/>
		<xsl:param name="BaseTypeName"/>
		<xsl:variable name="typeName">
			<xsl:choose>
				<xsl:when test="$Modality='alethic'">
					<xsl:value-of select="$BaseTypeName"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="concat('Nullable&lt;global::System.',$BaseTypeName,'&gt;')"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<ExternalTypeMoniker Name="/System/{$typeName}"/>
	</xsl:template>
	<xsl:template match="odt:floatingPointNumber" mode="GenerateExternalTypeMoniker">
		<xsl:param name="Modality"/>
		<xsl:variable name="typeName">
			<xsl:variable name="baseTypeName">
			</xsl:variable>
		</xsl:variable>
		<xsl:if test="@precision &gt;= 54">
			<xsl:comment>WARNING: .NET doesn't have support for IEEE 754 floating point types with greater than 54 binary digits in the significand. Therefore, you get a Double instead.</xsl:comment>
		</xsl:if>
		<xsl:call-template name="GenerateExternalTypeMonikerForValueType">
			<xsl:with-param name="Modality" select="$Modality"/>
			<xsl:with-param name="BaseTypeName">
				<xsl:choose>
					<xsl:when test="@precision &lt; 25">
						<xsl:value-of select="'Single'"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="'Double'"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<xsl:template match="odt:binary" mode="GetExternalTypeMoniker">
		<ExternalTypeMoniker Name="/System/Byte[]"/>
	</xsl:template>
	<xsl:template match="odt:decimalNumber" mode="GenerateExternalTypeMoniker">
		<xsl:param name="Modality"/>
		<!-- TODO: Map to correct size. -->
		<xsl:call-template name="GenerateExternalTypeMonikerForValueType">
			<xsl:with-param name="Modality" select="$Modality"/>
			<xsl:with-param name="BaseTypeName">
				<xsl:choose>
					<xsl:when test="true()">
						<xsl:value-of select="'Decimal'"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="'Int32'"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<xsl:template match="odt:string" mode="GenerateExternalTypeMoniker">
		<ExternalTypeMoniker Name="/System/String"/>
	</xsl:template>
	<xsl:template match="odt:boolean" mode="GenerateExternalTypeMoniker">
		<xsl:param name="Modality"/>
		<xsl:call-template name="GenerateExternalTypeMonikerForValueType">
			<xsl:with-param name="Modality" select="$Modality"/>
			<xsl:with-param name="BaseTypeName" select="'Boolean'"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template match="oil:conceptType" mode="GenerateDomainProperty">
		<ElementMergeDirectives>
			<ElementMergeDirective>
				<Index>
					<DomainClassMoniker Name=""/>
				</Index>
				<LinkCreationPaths>
					<DomainPath></DomainPath>
				</LinkCreationPaths>
			</ElementMergeDirective>
		</ElementMergeDirectives>
	</xsl:template>
	<xsl:template match="oil:conceptTypeRef" mode="GenerateDomainProperty">
		<ElementMergeDirectives>
			<ElementMergeDirective>
				<Index>
					<DomainClassMoniker Name=""/>
				</Index>
				<LinkCreationPaths>
					<DomainPath></DomainPath>
				</LinkCreationPaths>
			</ElementMergeDirective>
		</ElementMergeDirectives>
	</xsl:template>

	<xsl:template match="oil:conceptTypeRef" mode="GenerateRelationships">
		<!--TODO:-->
		<DomainRelationship Id="" Name="" DisplayName="" Namespace="">
			<xsl:variable name="ContainerName" select="../@name"/>
			<Source>
				<DomainRole Id="" Name="{@name}" DisplayName="{@name}" PropertyName="">
					<RolePlayer>
						<DomainClassMoniker Name="{$ModelNamespace}.{@name}"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="" Name="{@target}" DisplayName="{@target}" PropertyName="">
					<RolePlayer>
						<DomainClassMoniker Name="{$ModelNamespace}{@target}"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
	</xsl:template>
	<xsl:template match="@id" mode="GenerateConnectors">
		<!--TODO:-->
		<Connector Id="" Name="" DisplayName="" Namespace="" FixedTooltipText="" DashStyle="Dash" TargetEndStyle="EmptyArrow">
			<ConnectorHasDecorators Position="TargetBottom" OffsetFromShape="0" OffsetFromLine="0">
				<TextDecorator Name="" DisplayName="" DefaultText=""/>
			</ConnectorHasDecorators>
		</Connector>
	</xsl:template>
	<xsl:template match="@id" mode="GenerateShapes">
		<!--TODO:-->
		<GeometryShape Id="" Name="" DisplayName="" Namespace="" FixedTooltipText="" FillColor="LightBlue" OutlineColor="" InitialWidth="2" InitialHeight="0.75" Geometry="Rectangle">
			<!--<Notes></Notes>-->
			<ShapeHasDecorators Position="Center" HorizontalOffset="0" VerticalOffset="0">
				<TextDecorator Name="" DisplayName="" DefaultText=""/>
			</ShapeHasDecorators>
		</GeometryShape>
	</xsl:template>
	<xsl:template name="CreateXmlSerializationBehavior">
		<XmlSerializationBehavior>
			<ClassData>
			</ClassData>
		</XmlSerializationBehavior>
	</xsl:template>
</xsl:stylesheet>