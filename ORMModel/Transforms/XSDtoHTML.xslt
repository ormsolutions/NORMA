<?xml version="1.0" encoding="UTF-8" ?>
<!DOCTYPE stylesheet [
	<!ENTITY nbsp "<xsl:text disable-output-escaping='yes'>&amp;nbsp;</xsl:text>">
]>
<xsl:stylesheet
	version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:xs="http://www.w3.org/2001/XMLSchema"
	xmlns:exsl="http://exslt.org/common"
	exclude-result-prefixes="xs"
	extension-element-prefixes="exsl">
	<xsl:output method="html" indent="no" doctype-public="-//W3C//DTD XHTML 1.0 Strict//EN" doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" encoding="utf-8" omit-xml-declaration="no"/>
	<xsl:param name="TypeLinkDecorator" select="'_Type'"/>
	<xsl:param name="WriteGeneratedBy" select="false()"/>
	<xsl:variable name="TargetNamespace" select="string(/xs:schema/@targetNamespace)"/>
	<xsl:template name="WriteName" xmlns="http://www.w3.org/1999/xhtml">
		<xsl:param name="IsType" select="false()"/>
		<xsl:param name="NamePrefix" select="''"/>
		<xsl:param name="Reference" select="false()"/>
		<xsl:param name="WriteMultiplicity" select="false()"/>
		<xsl:param name="GlobalComplexTypes"/>
		<xsl:param name="GlobalSimpleTypes"/>
		<xsl:param name="MinOccurs"/>
		<xsl:param name="MaxOccurs"/>
		<xsl:if test="string-length(@name)+string-length(@ref)">
			<xsl:variable name="elementNameFragment">
				<xsl:choose>
					<xsl:when test="string-length(@name)">
						<xsl:variable name="nameDecorator">
							<xsl:if test="$IsType">
								<xsl:value-of select="$TypeLinkDecorator"/>
							</xsl:if>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="not($Reference) and (parent::xs:schema or not(parent::*))">
								<a id="{@name}{$nameDecorator}">
									<xsl:value-of select="$NamePrefix"/>
									<xsl:value-of select="@name"/>
								</a>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$NamePrefix"/>
								<xsl:value-of select="@name"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<a href="#{@ref}">
							<xsl:value-of select="@ref"/>
						</a>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:choose>
				<xsl:when test="@_originalAbstract=true() or @abstract='true' or @abstract=1">
					<b>
						<i>
							<xsl:copy-of select="$elementNameFragment"/>
						</i>
					</b>					
					<xsl:text> (external extension point)</xsl:text>
				</xsl:when>
				<xsl:otherwise>
					<b>
						<xsl:copy-of select="$elementNameFragment"/>
					</b>
				</xsl:otherwise>
			</xsl:choose>
			<xsl:variable name="hasUse" select="@use and not(@use='optional')"/>
			<xsl:variable name="hasDefault" select="string(@default)"/>
			<xsl:choose>
				<xsl:when test="string-length(@type)">
					<xsl:text> (</xsl:text>
					<xsl:call-template name="RenderTypeReference"/>
					<xsl:if test="$hasUse">
						<xsl:text>, </xsl:text>
						<xsl:value-of select="@use"/>
					</xsl:if>
					<xsl:if test="$hasDefault">
						<xsl:text>, default=</xsl:text>
						<xsl:value-of select="@default"/>
					</xsl:if>
					<xsl:text>)</xsl:text>
				</xsl:when>
				<xsl:when test="$hasUse or $hasDefault">
					<xsl:text> (</xsl:text>
					<xsl:choose>
						<xsl:when test="$hasUse">
							<xsl:value-of select="@use"/>
							<xsl:if test="$hasDefault">
								<xsl:text>, default=</xsl:text>
								<xsl:value-of select="@default"/>
							</xsl:if>
						</xsl:when>
						<xsl:when test="$hasDefault">
							<xsl:text>default=</xsl:text>
							<xsl:value-of select="@default"/>
						</xsl:when>
					</xsl:choose>
					<xsl:text>)</xsl:text>
				</xsl:when>
			</xsl:choose>
			<xsl:if test="$WriteMultiplicity">
				<xsl:call-template name="RenderMultiplicity">
					<xsl:with-param name="MinOccurs" select="$MinOccurs"/>
					<xsl:with-param name="MaxOccurs" select="$MaxOccurs"/>
				</xsl:call-template>
			</xsl:if>
			<xsl:variable name="localDocumentationFragment">
				<xsl:apply-templates select="xs:annotation/xs:documentation"/>
			</xsl:variable>
			<xsl:variable name="localDocumentation" select="string($localDocumentationFragment)"/>
			<xsl:choose>
				<xsl:when test="$localDocumentation">
					<xsl:text>: </xsl:text>
					<xsl:value-of select="$localDocumentation"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:variable name="fallbackDocumentationFragment">
						<xsl:if test="@type">
							<xsl:if test="$GlobalComplexTypes">
								<xsl:apply-templates select="$GlobalComplexTypes[@name=current()/@type]/xs:annotation/xs:documentation"/>
							</xsl:if>
							<xsl:if test="$GlobalSimpleTypes">
								<xsl:apply-templates select="$GlobalSimpleTypes[@name=current()/@type]/xs:annotation/xs:documentation"/>
							</xsl:if>
						</xsl:if>
					</xsl:variable>
					<xsl:variable name="fallbackDocumentation" select="string($fallbackDocumentationFragment)"/>
					<xsl:if test="$fallbackDocumentation">
						<xsl:text>: </xsl:text>
						<xsl:value-of select="$fallbackDocumentation"/>
					</xsl:if>
					<xsl:if test="not($fallbackDocumentation)">
						<span style="font-size:larger">
							<xsl:text>: NO DOCUMENTATION</xsl:text>
						</span>
					</xsl:if>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>
	<xsl:template name="RenderTypeReference" xmlns="http://www.w3.org/1999/xhtml">
		<xsl:param name="QName" select="string(@type)"/>
		<xsl:param name="ContextNamespaces" select="namespace::*"/>
		<xsl:variable name="prefix" select="substring-before($QName,':')"/>
		<xsl:choose>
			<xsl:when test="$TargetNamespace=string($ContextNamespaces[local-name()=$prefix])">
				<xsl:variable name="localNameFragment">
					<xsl:choose>
						<xsl:when test="$prefix">
							<xsl:value-of select="substring($QName,string-length($prefix)+2)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="$QName"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:variable name="localName" select="string($localNameFragment)"/>
				<a href="#{$localName}{$TypeLinkDecorator}">
					<xsl:value-of select="$localName" />
				</a>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$QName"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="/" xmlns="http://www.w3.org/1999/xhtml">
		<html>
			<head>
				<style type="text/css">
					body {font-family:verdana; font-size:smaller;}
					.targetNamespaceHeader {font-size:smaller; font-weight:400;}
					.mainHeader {font-size:large;text-align:center;}
					.tableSimpleType {border-collapse:collapse; width:94%; border-color:#000000;border-style:solid;border-width:1px;}
					.trSimpleType {font-weight:700; text-align:center; background-color:#000000; color:#FFFFFF;}
					.unit {border-style: solid; border-width: 1px; border-color: lightgrey;}
					.comment {font-family:courier new; color:#008000;}
				</style>
				<xsl:for-each select="xs:schema">
					<title>
						<xsl:if test="@id">
							<xsl:value-of select="@id"/>
							<xsl:text> </xsl:text>
						</xsl:if>
						<xsl:text>Schema: </xsl:text>
						<xsl:value-of select="@targetNamespace"/>
					</title>					
				</xsl:for-each>
			</head>
			<body>
				<xsl:apply-templates/>
				<xsl:if test="$WriteGeneratedBy">
					<p>
						<span class="comment">
							<xsl:text><![CDATA[//------------------------------------------------------------------------------]]></xsl:text>
							<br />
							<xsl:text><![CDATA[// <autogenerated>]]></xsl:text>
							<br />
							<xsl:text><![CDATA[//     This document was generated by XSDtoHTML.xslt]]></xsl:text>
							<br />
							<xsl:text><![CDATA[//     and must be regenerated if the XSD changes.]]></xsl:text>
							<br />
							<xsl:text><![CDATA[// </autogenerated>]]></xsl:text>
							<br />
							<xsl:text><![CDATA[//------------------------------------------------------------------------------]]></xsl:text>
							<br />
						</span>
					</p>
				</xsl:if>
			</body>
		</html>
	</xsl:template>
	<xsl:template name="RenderHeader" xmlns="http://www.w3.org/1999/xhtml">
		<xsl:param name="HeaderName"/>
		<xsl:param name="HeaderLink1"/>
		<xsl:param name="HeaderLink2"/>
		<h4 id="{translate($HeaderName,' ','')}_Header">
			<xsl:value-of select="$HeaderName"/>
			<xsl:if test="$HeaderLink1 or $HeaderLink2">
				<span style="font-size:smaller">
					&nbsp;&nbsp;
					<xsl:text>(</xsl:text>
					<xsl:choose>
						<xsl:when test="$HeaderLink1">
							<xsl:copy-of select="$HeaderLink1"/>
							<xsl:if test="$HeaderLink2">
								<xsl:text>,</xsl:text>
								&nbsp;
								<xsl:copy-of select="$HeaderLink2"/>
							</xsl:if>
						</xsl:when>
						<xsl:otherwise>
							<xsl:copy-of select="$HeaderLink2"/>
						</xsl:otherwise>
					</xsl:choose>
					<xsl:text>)</xsl:text>
				</span>
			</xsl:if>
		</h4>
	</xsl:template>
	<xsl:template match="xs:schema" xmlns="http://www.w3.org/1999/xhtml">
		<h3 class="mainHeader">
			<xsl:value-of select="@id"/> Schema<br/><span class="targetNamespaceHeader">
				<xsl:value-of select="@targetNamespace"/>
			</span>
		</h3>
		<xsl:variable name="complexTypes" select="xs:complexType"/>
		<xsl:variable name="simpleTypes" select="xs:simpleType"/>
		<xsl:variable name="collatedComplexTypesFragment">
			<xsl:if test="$complexTypes">
				<xsl:apply-templates select="$complexTypes" mode="collate">
					<xsl:with-param name="GlobalComplexTypes" select="$complexTypes"/>
					<xsl:with-param name="GlobalSimpleTypes" select="$simpleTypes"/>
				</xsl:apply-templates>
			</xsl:if>
		</xsl:variable>
		<xsl:variable name="collatedComplexTypes" select="exsl:node-set($collatedComplexTypesFragment)/child::*"/>
		<xsl:variable name="elements" select="xs:element"/>
		<xsl:variable name="elementsHeaderFragment">
			<xsl:if test="$elements">
				<a href="#Elements_Header">Elements</a>
			</xsl:if>
		</xsl:variable>
		<xsl:variable name="elementsHeader" select="exsl:node-set($elementsHeaderFragment)/child::*"/>
		<xsl:variable name="complexTypesHeaderFragment">
			<xsl:if test="$complexTypes">
				<a href="#ComplexTypes_Header">Complex Types</a>
			</xsl:if>
		</xsl:variable>
		<xsl:variable name="complexTypesHeader" select="exsl:node-set($complexTypesHeaderFragment)/child::*"/>
		<xsl:variable name="simpleTypesHeaderFragment">
			<xsl:if test="$simpleTypes">
				<a href="#SimpleTypes_Header">Simple Types</a>
			</xsl:if>
		</xsl:variable>
		<xsl:variable name="simpleTypesHeader" select="exsl:node-set($simpleTypesHeaderFragment)/child::*"/>
		<xsl:if test="$elements">
			<xsl:call-template name="RenderHeader">
				<xsl:with-param name="HeaderName" select="'Elements'"/>
				<xsl:with-param name="HeaderLink1" select="$complexTypesHeader"/>
				<xsl:with-param name="HeaderLink2" select="$simpleTypesHeader"/>
			</xsl:call-template>
			<ul>
				<xsl:apply-templates select="$elements">
					<xsl:sort select="@name"/>
					<xsl:with-param name="GlobalComplexTypes" select="$collatedComplexTypes"/>
					<xsl:with-param name="GlobalSimpleTypes" select="$simpleTypes"/>
				</xsl:apply-templates>
			</ul>
		</xsl:if>
		<xsl:if test="$complexTypes">
			<xsl:variable name="collapsedComplexTypesFragment">
				<xsl:apply-templates select="$collatedComplexTypes" mode="collapse"/>
			</xsl:variable>
			<xsl:variable name="collapsedComplexTypes" select="exsl:node-set($collapsedComplexTypesFragment)/child::*"/>
			<xsl:call-template name="RenderHeader">
				<xsl:with-param name="HeaderName" select="'Complex Types'"/>
				<xsl:with-param name="HeaderLink1" select="$elementsHeader"/>
				<xsl:with-param name="HeaderLink2" select="$simpleTypesHeader"/>
			</xsl:call-template>
			<ul>
				<!--<xsl:apply-templates select="$complexTypes">
					<xsl:sort select="@name"/>
					<xsl:with-param name="GlobalComplexTypes" select="$complexTypes"/>
					<xsl:with-param name="GlobalSimpleTypes" select="$simpleTypes"/>
				</xsl:apply-templates>-->
				<!--<xsl:apply-templates select="$collatedComplexTypes">
					<xsl:sort select="@name"/>
					<xsl:with-param name="GlobalComplexTypes" select="$collatedComplexTypes"/>
					<xsl:with-param name="GlobalSimpleTypes" select="$simpleTypes"/>
				</xsl:apply-templates>-->
				<xsl:apply-templates select="$collapsedComplexTypes">
					<xsl:sort select="@name"/>
					<xsl:with-param name="GlobalComplexTypes" select="$collapsedComplexTypes"/>
					<xsl:with-param name="GlobalSimpleTypes" select="$simpleTypes"/>
					<xsl:with-param name="PreCollapsed" select="true()"/>
				</xsl:apply-templates>
			</ul>
		</xsl:if>
		<xsl:if test="$simpleTypes">
			<xsl:call-template name="RenderHeader">
				<xsl:with-param name="HeaderName" select="'Simple Types'"/>
				<xsl:with-param name="HeaderLink1" select="$elementsHeader"/>
				<xsl:with-param name="HeaderLink2" select="$complexTypesHeader"/>
			</xsl:call-template>
			<ul style="list-style-type:none;">
				<xsl:apply-templates select="$simpleTypes">
					<xsl:sort select="@name"/>
					<xsl:with-param name="GlobalSimpleTypes" select="$simpleTypes"/>
				</xsl:apply-templates>
			</ul>
		</xsl:if>
	</xsl:template>
	<xsl:template name="RenderSimpleType" xmlns="http://www.w3.org/1999/xhtml">
		<xsl:param name="InlineType" select="false()"/>
		<xsl:param name="GlobalSimpleTypes"/>
		<table border="1" class="tableSimpleType">
			<tr class="trSimpleType">
				<td colspan="2">
					<xsl:choose>
						<xsl:when test="$InlineType">
							<xsl:text>(inlineType)</xsl:text>
							<xsl:if test="xs:annotation/xs:documentation">
								<xsl:text>: </xsl:text>
								<xsl:apply-templates select="xs:annotation/xs:documentation"/>
							</xsl:if>
						</xsl:when>
						<xsl:otherwise>
							<xsl:call-template name="WriteName">
								<xsl:with-param name="IsType" select="true()"/>
								<xsl:with-param name="GlobalSimpleTypes" select="$GlobalSimpleTypes"/>
							</xsl:call-template>
						</xsl:otherwise>
					</xsl:choose>
				</td>
			</tr>
			<xsl:for-each select="xs:restriction/@base">
				<tr>
					<td>
						<xsl:text>(restricts)</xsl:text>
					</td>
					<td style="width:100%;">
						<xsl:call-template name="RenderTypeReference">
							<xsl:with-param name="QName" select="."/>
							<xsl:with-param name="ContextNamespaces" select="../namespace::*"/>
						</xsl:call-template>
					</td>
				</tr>
			</xsl:for-each>
			<xsl:apply-templates select="xs:restriction/child::xs:* | xs:union | xs:list">
				<xsl:with-param name="GlobalSimpleTypes" select="$GlobalSimpleTypes"/>
			</xsl:apply-templates>
		</table>
	</xsl:template>
	<xsl:template match="xs:simpleType[@name]" xmlns="http://www.w3.org/1999/xhtml">
		<xsl:param name="GlobalSimpleTypes"/>
		<li>
			<xsl:call-template name="RenderSimpleType">
				<xsl:with-param name="GlobalSimpleTypes" select="$GlobalSimpleTypes"/>
			</xsl:call-template>
		</li>
	</xsl:template>
	<xsl:template match="xs:simpleType" xmlns="http://www.w3.org/1999/xhtml">
		<xsl:param name="GlobalSimpleTypes"/>
		<ul>
			<li>
				<xsl:call-template name="RenderSimpleType">
					<xsl:with-param name="GlobalSimpleTypes" select="$GlobalSimpleTypes"/>
				</xsl:call-template>
			</li>
		</ul>
	</xsl:template>
	<xsl:template match="xs:union" xmlns="http://www.w3.org/1999/xhtml">
		<xsl:param name="GlobalSimpleTypes"/>
		<tr>
			<td colspan="2">
				<xsl:text>Match a valid value from any of the following types:</xsl:text>
			</td>
		</tr>
		<xsl:variable name="memberTypesFragment">
			<xsl:call-template name="ParseList">
				<xsl:with-param name="ListValues" select="@memberTypes"/>
			</xsl:call-template>
		</xsl:variable>
		<tr>
			<td colspan="2">
				<ul>
					<xsl:variable name="contextNamespaces" select="namespace::*"/>
					<xsl:for-each select="exsl:node-set($memberTypesFragment)/child::*">
						<li>
							<xsl:call-template name="RenderTypeReference">
								<xsl:with-param name="QName" select="string(.)"/>
								<xsl:with-param name="ContextNamespaces" select="$contextNamespaces"/>
							</xsl:call-template>
						</li>
					</xsl:for-each>
					<xsl:for-each select="xs:simpleType">
						<li>
							<xsl:call-template name="RenderSimpleType">
								<xsl:with-param name="InlineType" select="true()"/>
								<xsl:with-param name="GlobalSimpleTypes" select="$GlobalSimpleTypes"/>
							</xsl:call-template>
						</li>
					</xsl:for-each>
				</ul>
			</td>
		</tr>
	</xsl:template>
	<xsl:template match="xs:list" xmlns="http://www.w3.org/1999/xhtml">
		<xsl:param name="GlobalSimpleTypes"/>
		<tr>
			<td colspan="2">
				<xsl:text>A space-delimited list of values from:</xsl:text>
			</td>
		</tr>
		<tr>
			<td colspan="2">
				<ul>
					<xsl:for-each select="@itemType">
						<li>
							<xsl:call-template name="RenderTypeReference">
								<xsl:with-param name="QName" select="string(.)"/>
							</xsl:call-template>
						</li>
					</xsl:for-each>
					<xsl:for-each select="xs:simpleType">
						<li>
							<xsl:call-template name="RenderSimpleType">
								<xsl:with-param name="GlobalSimpleTypes" select="$GlobalSimpleTypes"/>
								<xsl:with-param name="InlineType" select="true()"/>
							</xsl:call-template>
						</li>
					</xsl:for-each>
				</ul>
			</td>
		</tr>
	</xsl:template>
	<xsl:template name="ParseList">
		<xsl:param name="ListValues"/>
		<xsl:call-template name="ParseListHelper">
			<xsl:with-param name="ListValues" select="normalize-space($ListValues)"/>
		</xsl:call-template>
	</xsl:template>
	<xsl:template name="ParseListHelper">
		<xsl:param name="ListValues"/>
		<!-- Run after the string has been normalized -->
		<xsl:variable name="beforeSpace" select="substring-before($ListValues, ' ')"/>
		<xsl:choose>
			<xsl:when test="$beforeSpace">
				<listItem>
					<xsl:value-of select="$beforeSpace"/>
				</listItem>
				<xsl:call-template name="ParseListHelper">
					<xsl:with-param name="ListValues" select="substring($ListValues,string-length($beforeSpace)+2)"/>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<listItem>
					<xsl:value-of select="$ListValues"/>
				</listItem>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="xs:complexType[@name]" xmlns="http://www.w3.org/1999/xhtml">
		<xsl:param name="GlobalComplexTypes"/>
		<xsl:param name="GlobalSimpleTypes"/>
		<xsl:param name="PreCollapsed" select="false()"/>
		<xsl:if test="not(@abstract='true' or @abstract='1')">
			<xsl:variable name="contentsFragment">
				<xsl:call-template name="PopulateComplexType">
					<xsl:with-param name="GlobalComplexTypes" select="$GlobalComplexTypes"/>
					<xsl:with-param name="GlobalSimpleTypes" select="$GlobalSimpleTypes"/>
					<xsl:with-param name="PreCollapsed" select="$PreCollapsed"/>
				</xsl:call-template>
			</xsl:variable>
			<xsl:variable name="contents" select="exsl:node-set($contentsFragment)/child::*"/>
			<li>
				<xsl:call-template name="WriteName">
					<xsl:with-param name="IsType" select="true()"/>
					<xsl:with-param name="GlobalComplexTypes" select="$GlobalComplexTypes"/>
					<xsl:with-param name="GlobalSimpleTypes" select="$GlobalSimpleTypes"/>
				</xsl:call-template>
				<xsl:if test="$contents">
					<ul>
					<xsl:copy-of select="$contents"/>
				</ul>
				</xsl:if>
			</li>
		</xsl:if>
	</xsl:template>
	<xsl:template match="xs:complexType[@name]" mode="collate">
		<xsl:param name="GlobalComplexTypes"/>
		<xsl:param name="GlobalSimpleTypes"/>
		<xsl:copy>
			<xsl:copy-of select="@*"/>
			<xsl:call-template name="PopulateComplexType_Collate">
				<xsl:with-param name="GlobalComplexTypes" select="$GlobalComplexTypes"/>
				<xsl:with-param name="GlobalSimpleTypes" select="$GlobalSimpleTypes"/>
			</xsl:call-template>
		</xsl:copy>
	</xsl:template>
	<xsl:template match="xs:complexType">
		<xsl:param name="GlobalComplexTypes"/>
		<xsl:param name="GlobalSimpleTypes"/>
		<xsl:param name="PreCollapsed" select="false()"/>
		<xsl:variable name="contentsFragment">
			<xsl:call-template name="PopulateComplexType">
				<xsl:with-param name="GlobalComplexTypes" select="$GlobalComplexTypes"/>
				<xsl:with-param name="GlobalSimpleTypes" select="$GlobalSimpleTypes"/>
				<xsl:with-param name="PreCollapsed" select="$PreCollapsed"/>
			</xsl:call-template>
		</xsl:variable>
		<xsl:variable name="contents" select="exsl:node-set($contentsFragment)/child::*"/>
		<xsl:if test="$contents">
			<ul>
				<xsl:copy-of select="$contents"/>
			</ul>
		</xsl:if>
	</xsl:template>
	<xsl:template match="xs:complexType" mode="collate">
		<xsl:param name="GlobalComplexTypes"/>
		<xsl:param name="GlobalSimpleTypes"/>
		<xsl:call-template name="PopulateComplexType_Collate">
			<xsl:with-param name="GlobalComplexTypes" select="$GlobalComplexTypes"/>
			<xsl:with-param name="GlobalSimpleTypes" select="$GlobalSimpleTypes"/>
		</xsl:call-template>
	</xsl:template>
	<xsl:template name="PopulateComplexType">
		<xsl:param name="GlobalComplexTypes"/>
		<xsl:param name="GlobalSimpleTypes"/>
		<xsl:param name="PreCollapsed" select="false()"/>
		<xsl:variable name="extensionContentsFragment">
			<xsl:apply-templates select="xs:complexContent/xs:extension | xs:simpleContent/xs:extension">
				<xsl:with-param name="GlobalComplexTypes" select="$GlobalComplexTypes"/>
				<xsl:with-param name="GlobalSimpleTypes" select="$GlobalSimpleTypes"/>
			</xsl:apply-templates>
		</xsl:variable>
		<xsl:variable name="extensionContents" select="exsl:node-set($extensionContentsFragment)/child::*"/>
		<!-- Note the xs:element here is not standard xsd, but supports the collapsed form we generate earlier -->
		<xsl:variable name="childElements" select="xs:element | xs:any | xs:sequence | xs:choice | xs:all | xs:group"/>
		<xsl:variable name="childAttributes" select="xs:attribute | xs:attributeGroup"/>
		<xsl:variable name="simpleContentType" select="xs:simpleContentType[1]"/>
		<xsl:if test="$childElements or $childAttributes or $extensionContents or $simpleContentType">
			<xsl:apply-templates select="$childAttributes">
				<xsl:with-param name="GlobalSimpleTypes" select="$GlobalSimpleTypes"/>
			</xsl:apply-templates>
			<xsl:apply-templates select="$simpleContentType"/>
			<xsl:copy-of select="$extensionContents"/>
			<xsl:apply-templates select="$childElements">
				<xsl:with-param name="GlobalComplexTypes" select="$GlobalComplexTypes"/>
				<xsl:with-param name="GlobalSimpleTypes" select="$GlobalSimpleTypes"/>
				<xsl:with-param name="PreCollapsed" select="$PreCollapsed"/>
			</xsl:apply-templates>
		</xsl:if>
	</xsl:template>
	<xsl:template name="PopulateComplexType_Collate">
		<xsl:param name="GlobalComplexTypes"/>
		<xsl:param name="GlobalSimpleTypes"/>
		<xsl:variable name="extensionContentsFragment">
			<xsl:apply-templates select="xs:complexContent/xs:extension | xs:simpleContent/xs:extension" mode="collate">
				<xsl:with-param name="GlobalComplexTypes" select="$GlobalComplexTypes"/>
				<xsl:with-param name="GlobalSimpleTypes" select="$GlobalSimpleTypes"/>
			</xsl:apply-templates>
		</xsl:variable>
		<xsl:variable name="extensionContents" select="exsl:node-set($extensionContentsFragment)/child::*"/>
		<xsl:variable name="localAnnotations" select="xs:annotation"/>
		<xsl:variable name="childElements" select="xs:sequence | xs:choice | xs:all | xs:group"/>
		<xsl:variable name="childAttributes" select="xs:attribute | xs:attributeGroup"/>
		<xsl:variable name="hasLocalContents" select="boolean($childElements) or boolean($childAttributes)"/>
		<xsl:choose>
			<xsl:when test="$extensionContents">
				<xsl:choose>
					<xsl:when test="$hasLocalContents">
						<xsl:choose>
							<xsl:when test="$childElements">
								<xsl:variable name="nonAttributeExtensions" select="$extensionContents[not(self::xs:attribute | self::xs:annotation | self::xs:simpleContentType)]"/>
								<xsl:choose>
									<xsl:when test="$nonAttributeExtensions">
										<xsl:call-template name="CollateAnnotations">
											<xsl:with-param name="PrimaryAnnotations" select="$localAnnotations"/>
											<xsl:with-param name="FallbackAnnotations" select="$extensionContents[self::xs:annotation]"/>
										</xsl:call-template>
										<xs:sequence minOccurs="1" maxOccurs="1">
											<xsl:copy-of select="nonAttributeExtensions"/>
											<xsl:apply-templates select="$childElements" mode="collate">
												<xsl:with-param name="GlobalComplexTypes" select="$GlobalComplexTypes"/>
												<xsl:with-param name="GlobalSimpleTypes" select="$GlobalSimpleTypes"/>
											</xsl:apply-templates>
										</xs:sequence>
									</xsl:when>
									<xsl:otherwise>
										<xsl:call-template name="CollateAnnotations">
											<xsl:with-param name="PrimaryAnnotations" select="$localAnnotations"/>
											<xsl:with-param name="FallbackAnnotations" select="$extensionContents[self::xs:annotation]"/>
										</xsl:call-template>
										<xsl:apply-templates select="$childElements" mode="collate">
											<xsl:with-param name="GlobalComplexTypes" select="$GlobalComplexTypes"/>
											<xsl:with-param name="GlobalSimpleTypes" select="$GlobalSimpleTypes"/>
										</xsl:apply-templates>
									</xsl:otherwise>
								</xsl:choose>
								<xsl:copy-of select="$extensionContents[self::xs:attribute]"/>
								<xsl:apply-templates select="$childAttributes" mode="collate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:call-template name="CollateAnnotations">
									<xsl:with-param name="PrimaryAnnotations" select="$localAnnotations"/>
									<xsl:with-param name="FallbackAnnotations" select="$extensionContents[self::xs:annotation]"/>
								</xsl:call-template>
								<xsl:copy-of select="$extensionContents[not(self::xs:annotation)]"/>
								<xsl:apply-templates select="$childAttributes" mode="collate"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="CollateAnnotations">
							<xsl:with-param name="PrimaryAnnotations" select="$localAnnotations"/>
							<xsl:with-param name="FallbackAnnotations" select="$extensionContents[self::xs:annotation]"/>
						</xsl:call-template>
						<xsl:copy-of select="$extensionContents[not(self::xs:annotation)]"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<xsl:otherwise>
				<xsl:copy-of select="$localAnnotations"/>
				<xsl:choose>
					<xsl:when test="parent::xs:simpleContent">
						<!-- Manufacture a node to track the deepest simple type-->
						<xs:simpleContentType name="{@base}" _sortPriority="-1"/>
					</xsl:when>
				</xsl:choose>
				<xsl:apply-templates select="$childElements" mode="collate">
					<xsl:with-param name="GlobalComplexTypes" select="$GlobalComplexTypes"/>
					<xsl:with-param name="GlobalSimpleTypes" select="$GlobalSimpleTypes"/>
				</xsl:apply-templates>
				<xsl:apply-templates select="xs:simpleContentType[1]" mode="collate"/>
				<xsl:apply-templates select="$childAttributes" mode="collate"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="CollateAnnotations">
		<xsl:param name="PrimaryAnnotations"/>
		<xsl:param name="FallbackAnnotations"/>
		<xsl:choose>
			<xsl:when test="$PrimaryAnnotations">
				<xsl:choose>
					<xsl:when test="$FallbackAnnotations">
						<xsl:variable name="primaryDocumentation" select="string($PrimaryAnnotations/xs:documentation)"/>
						<xsl:variable name="fallbackDocumentation" select="string($FallbackAnnotations/xs:documentation)"/>
						<!-- Make sure we only get one documentation tag, makes the rest of this file easier -->
						<xs:annotation>
							<xsl:if test="$primaryDocumentation or $fallbackDocumentation">
								<xs:documentation>
									<xsl:choose>
										<xsl:when test="$primaryDocumentation">
											<xsl:value-of select="$primaryDocumentation"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="$fallbackDocumentation"/>
										</xsl:otherwise>
									</xsl:choose>
								</xs:documentation>
							</xsl:if>
							<xsl:copy-of select="$PrimaryAnnotations/xs:appinfo"/>
							<xsl:copy-of select="$FallbackAnnotations/xs:appinfo"/>
						</xs:annotation>
					</xsl:when>
					<xsl:otherwise>
						<xsl:copy-of select="$PrimaryAnnotations"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<xsl:when test="$FallbackAnnotations">
				<xsl:copy-of select="$FallbackAnnotations"/>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="xs:complexContent/xs:extension | xs:simpleContent/xs:extension">
		<xsl:param name="GlobalComplexTypes"/>
		<xsl:param name="GlobalSimpleTypes"/>
		<xsl:for-each select="$GlobalComplexTypes[@name=current()/@base]">
			<xsl:call-template name="PopulateComplexType">
				<xsl:with-param name="GlobalComplexTypes" select="$GlobalComplexTypes"/>
				<xsl:with-param name="GlobalSimpleTypes" select="$GlobalSimpleTypes"/>
			</xsl:call-template>
		</xsl:for-each>
		<xsl:call-template name="PopulateComplexType">
			<xsl:with-param name="GlobalComplexTypes" select="$GlobalComplexTypes"/>
			<xsl:with-param name="GlobalSimpleTypes" select="$GlobalSimpleTypes"/>
		</xsl:call-template>
	</xsl:template>
	<xsl:template match="xs:complexContent/xs:extension | xs:simpleContent/xs:extension" mode="collate">
		<xsl:param name="GlobalComplexTypes"/>
		<xsl:param name="GlobalSimpleTypes"/>
		<xsl:variable name="baseContentsFragment">
			<xsl:for-each select="$GlobalComplexTypes[@name=current()/@base]">
				<xsl:call-template name="PopulateComplexType_Collate">
					<xsl:with-param name="GlobalComplexTypes" select="$GlobalComplexTypes"/>
					<xsl:with-param name="GlobalSimpleTypes" select="$GlobalSimpleTypes"/>
				</xsl:call-template>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="baseContents" select="exsl:node-set($baseContentsFragment)/child::*"/>
		<xsl:choose>
			<xsl:when test="$baseContents">
				<xsl:variable name="localContentsFragment">
					<xsl:call-template name="PopulateComplexType_Collate">
						<xsl:with-param name="GlobalComplexTypes" select="$GlobalComplexTypes"/>
						<xsl:with-param name="GlobalSimpleTypes" select="$GlobalSimpleTypes"/>
					</xsl:call-template>
				</xsl:variable>
				<xsl:variable name="localContents" select="exsl:node-set($localContentsFragment)/child::*"/>
				<xsl:choose>
					<xsl:when test="$localContents">
						<xsl:variable name="localElements" select="$localContents[not(self::xs:attribute | self::xs:annotation | self::xs:simpleContentType)]"/>
						<xsl:choose>
							<xsl:when test="$localElements">
								<xsl:variable name="baseElements" select="$baseContents[not(self::xs:attribute | self::xs:annotation | self::xs:simpleContentType)]"/>
								<xsl:copy-of select="$baseContents[self::xs:annotation]"/>
								<xsl:copy-of select="$localContents[self::xs:annotation]"/>
								<xsl:choose>
									<xsl:when test="$baseElements">
										<xs:sequence minOccurs="1" maxOccurs="1">
											<xsl:copy-of select="$baseElements"/>
											<xsl:copy-of select="$localElements"/>
										</xs:sequence>
									</xsl:when>
									<xsl:otherwise>
										<xsl:copy-of select="$localElements"/>
									</xsl:otherwise>
								</xsl:choose>
								<xsl:copy-of select="$baseContents[self::xs:attribute]"/>
								<xsl:copy-of select="$localContents[self::xs:attribute]"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:copy-of select="$baseContents"/>
								<xsl:copy-of select="$localContents[self::xs:attribute | self::xs:simpleContentType]"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:copy-of select="$baseContents"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="PopulateComplexType_Collate">
					<xsl:with-param name="GlobalComplexTypes" select="$GlobalComplexTypes"/>
					<xsl:with-param name="GlobalSimpleTypes" select="$GlobalSimpleTypes"/>
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="xs:group">
		<xsl:param name="Referred" select="false()"/>
		<xsl:choose>
			<xsl:when test="@ref">
				<xsl:apply-templates select="/xs:schema/xs:group[@name=current()/@ref]">
					<xsl:with-param name="Referred" select="true()"/>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:when test="$Referred">
				<xsl:apply-templates select="xs:choice | xs:sequence | xs:all"/>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="xs:group" mode="collate">
		<xsl:param name="GlobalComplexTypes"/>
		<xsl:param name="GlobalSimpleTypes"/>
		<xsl:param name="Referred" select="false()"/>
		<xsl:param name="MinOccurs"/>
		<xsl:param name="MaxOccurs"/>
		<xsl:choose>
			<xsl:when test="@ref">
				<xsl:apply-templates select="/xs:schema/xs:group[@name=current()/@ref]" mode="collate">
					<xsl:with-param name="GlobalComplexTypes" select="$GlobalComplexTypes"/>
					<xsl:with-param name="GlobalSimpleTypes" select="$GlobalSimpleTypes"/>
					<xsl:with-param name="Referred" select="true()"/>
					<xsl:with-param name="MinOccurs" select="@minOccurs"/>
					<xsl:with-param name="MaxOccurs" select="@maxOccurs"/>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:when test="$Referred">
				<xsl:apply-templates select="xs:choice | xs:sequence | xs:all" mode="collate">
					<xsl:with-param name="GlobalComplexTypes" select="$GlobalComplexTypes"/>
					<xsl:with-param name="GlobalSimpleTypes" select="$GlobalSimpleTypes"/>
					<xsl:with-param name="Referred" select="true()"/>
					<xsl:with-param name="MinOccurs" select="$MinOccurs"/>
					<xsl:with-param name="MaxOccurs" select="$MaxOccurs"/>
				</xsl:apply-templates>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="xs:element" xmlns="http://www.w3.org/1999/xhtml">
		<xsl:param name="GlobalComplexTypes"/>
		<xsl:param name="GlobalSimpleTypes"/>
		<xsl:param name="MinOccurs" select="@minOccurs"/>
		<xsl:param name="MaxOccurs" select="@maxOccurs"/>
		<xsl:param name="Referred" select="false()"/>
		<xsl:param name="PreCollapsed" select="false()"/>
		<xsl:choose>
			<xsl:when test="@ref">
				<xsl:apply-templates select="/xs:schema/xs:element[@name=current()/@ref]">
					<xsl:with-param name="GlobalComplexTypes" select="$GlobalComplexTypes"/>
					<xsl:with-param name="GlobalSimpleTypes" select="$GlobalSimpleTypes"/>
					<xsl:with-param name="Referred" select="true()"/>
					<xsl:with-param name="MinOccurs" select="$MinOccurs"/>
					<xsl:with-param name="MaxOccurs" select="$MaxOccurs"/>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:variable name="abstractElement" select="@abstract='true' or @abstract='1'"/>
				<xsl:if test="not($abstractElement) or (not(parent::xs:schema/xs:element[@substitutionGroup=current()/@name]) and not($GlobalComplexTypes//xs:element[@_originalAbstract][@name=current()/@name]))">
					<li>
						<xsl:call-template name="WriteName">
							<xsl:with-param name="Reference" select="$Referred"/>
							<xsl:with-param name="WriteMultiplicity" select="true()"/>
							<xsl:with-param name="MinOccurs" select="$MinOccurs"/>
							<xsl:with-param name="MaxOccurs" select="$MaxOccurs"/>
							<xsl:with-param name="GlobalComplexTypes" select="$GlobalComplexTypes"/>
							<xsl:with-param name="GlobalSimpleTypes" select="$GlobalSimpleTypes"/>
						</xsl:call-template>
						<xsl:variable name="nestedComplexType" select="xs:complexType"/>
						<xsl:choose>
							<xsl:when test="$nestedComplexType">
								<xsl:variable name="collatedNestedFragment">
									<xsl:apply-templates select="$nestedComplexType" mode="collate">
										<xsl:with-param name="GlobalComplexTypes" select="$GlobalComplexTypes"/>
										<xsl:with-param name="GlobalSimpleTypes" select="$GlobalSimpleTypes"/>
									</xsl:apply-templates>
								</xsl:variable>
								<xsl:variable name="collapsedNestedFragment">
									<xsl:apply-templates select="exsl:node-set($collatedNestedFragment)/child::*" mode="collapse">
										<xsl:with-param name="GlobalComplexTypes" select="$GlobalComplexTypes"/>
										<xsl:with-param name="GlobalSimpleTypes" select="$GlobalSimpleTypes"/>
									</xsl:apply-templates>
								</xsl:variable>
								<xsl:variable name="collapsedNested" select="exsl:node-set($collapsedNestedFragment)/child::*"/>
								<xsl:if test="$collapsedNested">
									<ul>
										<xsl:apply-templates select="$collapsedNested[not(self::xs:simpleContentType)]">
											<xsl:sort select="@_sortPriority" data-type="number"/>
											<xsl:sort select="@name"/>
											<xsl:with-param name="Referred" select="true()"/>
											<xsl:with-param name="GlobalComplexTypes" select="$GlobalComplexTypes"/>
											<xsl:with-param name="GlobalSimpleTypes" select="$GlobalSimpleTypes"/>
											<xsl:with-param name="PreCollapsed" select="boolean($collapsedNested/child::xs:*[not(self::xs:annotation)])"/>
										</xsl:apply-templates>
										<xsl:apply-templates select="$collapsedNested[self::xs:simpleContentType][1]"/>
									</ul>
								</xsl:if>
							</xsl:when>
							<xsl:otherwise>
								<xsl:variable name="nestedSimpleType" select="xs:simpleType"/>
								<xsl:choose>
									<xsl:when test="$nestedSimpleType">
										<xsl:apply-templates select="$nestedSimpleType">
											<xsl:with-param name="GlobalComplexTypes" select="$GlobalComplexTypes"/>
											<xsl:with-param name="GlobalSimpleTypes" select="$GlobalSimpleTypes"/>
										</xsl:apply-templates>
									</xsl:when>
									<xsl:when test="$PreCollapsed">
										<xsl:variable name="significantChildElements" select="child::*[not(self::xs:annotation)]"/>
										<xsl:if test="$significantChildElements">
											<ul>
												<xsl:apply-templates select="$significantChildElements">
													<xsl:with-param name="Referred" select="true()"/>
													<xsl:with-param name="GlobalComplexTypes" select="$GlobalComplexTypes"/>
													<xsl:with-param name="GlobalSimpleTypes" select="$GlobalSimpleTypes"/>
													<xsl:with-param name="PreCollapsed" select="boolean(child::xs:*[not(self::xs:annotation)])"/>
												</xsl:apply-templates>
											</ul>
										</xsl:if>
									</xsl:when>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>
					</li>
				</xsl:if>
				<xsl:if test="$Referred">
					<xsl:apply-templates select="/xs:schema/xs:element[@substitutionGroup=current()/@name]">
						<xsl:with-param name="GlobalComplexTypes" select="$GlobalComplexTypes"/>
						<xsl:with-param name="GlobalSimpleTypes" select="$GlobalSimpleTypes"/>
						<xsl:with-param name="Referred" select="true()"/>
						<xsl:with-param name="MinOccurs" select="$MinOccurs"/>
						<xsl:with-param name="MaxOccurs" select="$MaxOccurs"/>
					</xsl:apply-templates>
				</xsl:if>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="xs:element" mode="collate">
		<xsl:param name="GlobalComplexTypes"/>
		<xsl:param name="GlobalSimpleTypes"/>
		<xsl:param name="MinOccurs" select="@minOccurs"/>
		<xsl:param name="MaxOccurs" select="@maxOccurs"/>
		<xsl:param name="Referred" select="false()"/>
		<xsl:choose>
			<xsl:when test="@ref">
				<xs:choice>
					<xsl:choose>
						<xsl:when test="$Referred">
							<xsl:call-template name="EnsureMinMaxOccursAttributes">
								<xsl:with-param name="MinOccurs" select="$MinOccurs"/>
								<xsl:with-param name="MaxOccurs" select="$MaxOccurs"/>
							</xsl:call-template>
						</xsl:when>
						<xsl:otherwise>
							<xsl:call-template name="EnsureMinMaxOccursAttributes"/>
						</xsl:otherwise>
					</xsl:choose>
					<xsl:variable name="dummyFragment">
						<!-- MinOccurs and MaxOccurs need to be actual attributes, not numbers. Construct a fragment to accomodate -->
						<dummy minOccurs="1" maxOccurs="1"/>
					</xsl:variable>
					<xsl:variable name="dummy" select="exsl:node-set($dummyFragment)/child::*"/>
					<xsl:apply-templates select="/xs:schema/xs:element[@name=current()/@ref]" mode="collate">
						<xsl:with-param name="GlobalComplexTypes" select="$GlobalComplexTypes"/>
						<xsl:with-param name="GlobalSimpleTypes" select="$GlobalSimpleTypes"/>
						<xsl:with-param name="Referred" select="true()"/>
						<xsl:with-param name="MinOccurs" select="$dummy/@minOccurs"/>
						<xsl:with-param name="MaxOccurs" select="$dummy/@maxOccurs"/>
					</xsl:apply-templates>
				</xs:choice>
			</xsl:when>
			<xsl:otherwise>
				<xsl:variable name="abstractElement" select="@abstract='true' or @abstract='1'"/>
				<xsl:if test="not($abstractElement)">
					<xsl:copy>
						<xsl:copy-of select="@*"/>
						<xsl:choose>
							<xsl:when test="$Referred">
								<xsl:call-template name="EnsureMinMaxOccursAttributes">
									<xsl:with-param name="MinOccurs" select="$MinOccurs"/>
									<xsl:with-param name="MaxOccurs" select="$MaxOccurs"/>
								</xsl:call-template>
							</xsl:when>
							<xsl:otherwise>
								<xsl:call-template name="EnsureMinMaxOccursAttributes"/>
							</xsl:otherwise>
						</xsl:choose>
						<xsl:apply-templates select="xs:annotation | xs:complexType | xs:simpleType" mode="collate">
							<xsl:with-param name="GlobalComplexTypes" select="$GlobalComplexTypes"/>
							<xsl:with-param name="GlobalSimpleTypes" select="$GlobalSimpleTypes"/>
						</xsl:apply-templates>
					</xsl:copy>
				</xsl:if>
				<xsl:if test="$Referred">
					<xsl:variable name="substitutionsFragment">
						<xsl:apply-templates select="/xs:schema/xs:element[@substitutionGroup=current()/@name]" mode="collate">
							<xsl:with-param name="GlobalComplexTypes" select="$GlobalComplexTypes"/>
							<xsl:with-param name="GlobalSimpleTypes" select="$GlobalSimpleTypes"/>
							<xsl:with-param name="Referred" select="true()"/>
							<xsl:with-param name="MinOccurs" select="$MinOccurs"/>
							<xsl:with-param name="MaxOccurs" select="$MaxOccurs"/>
						</xsl:apply-templates>
					</xsl:variable>
					<xsl:choose>
						<xsl:when test="$abstractElement">
							<xsl:variable name="substitutions" select="exsl:node-set($substitutionsFragment)/child::*"/>
							<xsl:choose>
								<xsl:when test="$substitutions">
									<xsl:copy-of select="$substitutions"/>
								</xsl:when>
								<xsl:otherwise>
									<!-- If an element is abstract and referred and has no substitution group elements,
									then we need to list it anyway to indicate an extension point for external schemas. -->
									<xsl:copy>
										<xsl:copy-of select="@*"/>
										<xsl:attribute name="abstract">
											<xsl:value-of select="false()"/>
										</xsl:attribute>
										<xsl:attribute name="_originalAbstract">
											<xsl:value-of select="true()"/>
										</xsl:attribute>
										<xsl:choose>
											<xsl:when test="$Referred">
												<xsl:call-template name="EnsureMinMaxOccursAttributes">
													<xsl:with-param name="MinOccurs" select="$MinOccurs"/>
													<xsl:with-param name="MaxOccurs" select="$MaxOccurs"/>
												</xsl:call-template>
											</xsl:when>
											<xsl:otherwise>
												<xsl:call-template name="EnsureMinMaxOccursAttributes"/>
											</xsl:otherwise>
										</xsl:choose>
										<xsl:apply-templates select="xs:annotation | xs:complexType | xs:simpleType" mode="collate">
											<xsl:with-param name="GlobalComplexTypes" select="$GlobalComplexTypes"/>
											<xsl:with-param name="GlobalSimpleTypes" select="$GlobalSimpleTypes"/>
										</xsl:apply-templates>
									</xsl:copy>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:when>
						<xsl:otherwise>
							<xsl:copy-of select="$substitutionsFragment"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:if>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="xs:annotation/xs:documentation">
		<xsl:value-of select="." />
	</xsl:template>
	<xsl:template match="xs:annotation" mode="collate">
		<xsl:copy-of select="."/>
	</xsl:template>
	<xsl:template match="xs:restriction/xs:enumeration" priority="1" xmlns="http://www.w3.org/1999/xhtml">
		<tr>
			<td>
				<xsl:value-of select="@value"/>
			</td>
			<td style="width:100%;">
				<xsl:apply-templates select="xs:annotation/xs:documentation"/>
			</td>
		</tr>
	</xsl:template>
	<xsl:template match="xs:restriction/child::xs:*" xmlns="http://www.w3.org/1999/xhtml">
		<tr>
			<td>
				<xsl:text>(</xsl:text>
				<xsl:value-of select="local-name()"/>
				<xsl:text>)</xsl:text>
			</td>
			<td style="width:100%;">
				<xsl:value-of select="@value"/>
			</td>
		</tr>
	</xsl:template>
	<xsl:template match="xs:any" xmlns="http://www.w3.org/1999/xhtml">
		<xsl:param name="MinOccurs" select="@minOccurs"/>
		<xsl:param name="MaxOccurs" select="@maxOccurs"/>
		<xsl:variable name="namespaceListFragment">
			<xsl:call-template name="ParseList">
				<xsl:with-param name="ListValues" select="@namespace"/>
			</xsl:call-template>
		</xsl:variable>
		<xsl:variable name="namespaceList" select="exsl:node-set($namespaceListFragment)/child::*"/>
		<li>
			<xsl:call-template name="RenderMultiplicity">
				<xsl:with-param name="MinOccurs" select="$MinOccurs"/>
				<xsl:with-param name="MaxOccurs" select="$MaxOccurs"/>
				<xsl:with-param name="LeadPadding" select="''"/>
				<xsl:with-param name="TrailPadding" select="' '"/>
			</xsl:call-template>
			<xsl:choose>
				<xsl:when test="count($namespaceList)=1">
					<xsl:call-template name="DescribeAnyNamespace">
						<xsl:with-param name="NamespaceName" select="string($namespaceList[1])"/>
					</xsl:call-template>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text>Any element matching one of the following namespace restrictions:</xsl:text>
					<ul>
						<xsl:for-each select="$namespaceList">
							<li>
								<xsl:call-template name="DescribeAnyNamespace">
									<xsl:with-param name="NamespaceName" select="string(.)"/>
								</xsl:call-template>
							</li>
						</xsl:for-each>
					</ul>
				</xsl:otherwise>
			</xsl:choose>
		</li>
	</xsl:template>
	<xsl:template name="DescribeAnyNamespace" xmlns="http://www.w3.org/1999/xhtml">
		<xsl:param name="NamespaceName"/>
		<xsl:choose>
			<xsl:when test="$NamespaceName='##any'">
				<xsl:text>Any element with a non-empty namespace</xsl:text>
			</xsl:when>
			<xsl:when test="$NamespaceName='##other'">
				<xsl:text>Any element </xsl:text><b>not</b><xsl:text> in namespace </xsl:text>
				<b>
					<xsl:value-of select="$TargetNamespace"/>
				</b>
			</xsl:when>
			<xsl:when test="$NamespaceName='##targetNamespace'">
				<xsl:text>Any element in namespace </xsl:text>
				<b>
					<xsl:value-of select="$TargetNamespace"/>
				</b>
			</xsl:when>
			<xsl:when test="$NamespaceName='##local'">
				<xsl:text>Any element in the empty namespace</xsl:text>
			</xsl:when>
			<xsl:otherwise>
				<xsl:text>Any element in namespace </xsl:text>
				<b>
					<xsl:value-of select="$NamespaceName"/>
				</b>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="xs:any" mode="collate">
		<xsl:copy>
			<xsl:copy-of select="@*"/>
			<xsl:attribute name="_sortPriority">
				<xsl:text>2</xsl:text>
			</xsl:attribute>
			<xsl:call-template name="EnsureMinMaxOccursAttributes"/>
			<xsl:copy-of select="*|text()"/>
		</xsl:copy>
	</xsl:template>
	<xsl:template name="EnsureMinMaxOccursAttributes">
		<xsl:param name="MinOccurs" select="@minOccurs"/>
		<xsl:param name="MaxOccurs" select="@maxOccurs"/>
		<xsl:choose>
			<xsl:when test="$MinOccurs">
				<xsl:copy-of select="$MinOccurs"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:attribute name="minOccurs">
					<xsl:text>1</xsl:text>
				</xsl:attribute>
			</xsl:otherwise>
		</xsl:choose>
		<xsl:choose>
			<xsl:when test="$MaxOccurs">
				<xsl:copy-of select="$MaxOccurs"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:attribute name="maxOccurs">
					<xsl:text>1</xsl:text>
				</xsl:attribute>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="RenderMultiplicity">
		<xsl:param name="MinOccurs" select="@minOccurs"/>
		<xsl:param name="MaxOccurs" select="@maxOccurs"/>
		<xsl:param name="LeadPadding" select="' '"/>
		<xsl:param name="TrailPadding" select="''"/>
		<xsl:choose>
			<xsl:when test="$MinOccurs=0">
				<xsl:choose>
					<xsl:when test="not($MaxOccurs) or $MaxOccurs=1">
						<xsl:value-of select="$LeadPadding"/>
						<xsl:text>(optional)</xsl:text>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$LeadPadding"/>
						<xsl:text>(optional, repeat</xsl:text>
						<xsl:if test="not($MaxOccurs='unbounded')">
							<xsl:text> at most </xsl:text>
							<xsl:value-of select="$MaxOccurs"/>
							<xsl:text> times</xsl:text>
						</xsl:if>
						<xsl:text>)</xsl:text>
						<xsl:value-of select="$TrailPadding"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<xsl:when test="not($MinOccurs) or $MinOccurs='1'">
				<xsl:choose>
					<xsl:when test="not($MaxOccurs) or $MaxOccurs=1">
						<!-- Nothing for required -->
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$LeadPadding"/>
						<xsl:text>(repeat</xsl:text>
						<xsl:if test="not($MaxOccurs='unbounded')">
							<xsl:text> at most </xsl:text>
							<xsl:value-of select="$MaxOccurs"/>
							<xsl:text> times</xsl:text>
						</xsl:if>
						<xsl:text>)</xsl:text>
						<xsl:value-of select="$TrailPadding"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<xsl:otherwise>
				<xsl:choose>
					<xsl:when test="$MaxOccurs=$MinOccurs">
						<xsl:value-of select="$LeadPadding"/>
						<xsl:text>(repeat </xsl:text>
						<xsl:value-of select="$MinOccurs"/>
						<xsl:text> times)</xsl:text>
						<xsl:value-of select="$TrailPadding"/>
					</xsl:when>
					<xsl:when test="$MaxOccurs='unbounded'">
						<xsl:value-of select="$LeadPadding"/>
						<xsl:text>(repeat at least </xsl:text>
						<xsl:value-of select="$MinOccurs"/>
						<xsl:text> times)</xsl:text>
						<xsl:value-of select="$TrailPadding"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$LeadPadding"/>
						<xsl:text>(repeat </xsl:text>
						<xsl:value-of select="$MinOccurs"/>
						<xsl:text> to </xsl:text>
						<xsl:value-of select="$MaxOccurs"/>
						<xsl:text> times)</xsl:text>
						<xsl:value-of select="$TrailPadding"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="xs:sequence" xmlns="http://www.w3.org/1999/xhtml">
		<xsl:param name="GlobalComplexTypes"/>
		<xsl:param name="GlobalSimpleTypes"/>
		<xsl:param name="PreCollapsed" select="false()"/>
		<li class="unit">
			<xsl:text>Sequence</xsl:text>
			<xsl:call-template name="RenderMultiplicity"/>
			<ul>
				<xsl:apply-templates select="xs:element | xs:any | xs:choice | xs:sequence | xs:group">
					<xsl:with-param name="GlobalComplexTypes" select="$GlobalComplexTypes"/>
					<xsl:with-param name="GlobalSimpleTypes" select="$GlobalSimpleTypes"/>
					<xsl:with-param name="PreCollapsed" select="$PreCollapsed"/>
				</xsl:apply-templates>
			</ul>
		</li>
	</xsl:template>
	<xsl:template match="xs:choice" xmlns="http://www.w3.org/1999/xhtml">
		<xsl:param name="GlobalComplexTypes"/>
		<xsl:param name="GlobalSimpleTypes"/>
		<xsl:param name="PreCollapsed" select="false()"/>
		<li class="unit">
			<xsl:text>Choose One</xsl:text>
			<xsl:call-template name="RenderMultiplicity"/>
			<ul>
				<xsl:apply-templates select="xs:element | xs:any | xs:choice | xs:sequence | xs:group">
					<xsl:sort select="@_sortPriority" data-type="number"/>
					<xsl:sort select="@name"/>
					<xsl:with-param name="GlobalComplexTypes" select="$GlobalComplexTypes"/>
					<xsl:with-param name="GlobalSimpleTypes" select="$GlobalSimpleTypes"/>
					<xsl:with-param name="PreCollapsed" select="$PreCollapsed"/>
				</xsl:apply-templates>
			</ul>
		</li>
	</xsl:template>
	<xsl:template match="xs:all" xmlns="http://www.w3.org/1999/xhtml">
		<xsl:param name="GlobalComplexTypes"/>
		<xsl:param name="GlobalSimpleTypes"/>
		<xsl:param name="PreCollapsed" select="false()"/>
		<li class="unit">
			<xsl:text>Choose one of each in any order</xsl:text>
			<xsl:call-template name="RenderMultiplicity"/>
			<ul>
				<xsl:apply-templates select="xs:element | xs:any | xs:choice | xs:sequence | xs:group">
					<xsl:sort select="@_sortPriority" data-type="number"/>
					<xsl:sort select="@name"/>
					<xsl:with-param name="GlobalComplexTypes" select="$GlobalComplexTypes"/>
					<xsl:with-param name="GlobalSimpleTypes" select="$GlobalSimpleTypes"/>
					<xsl:with-param name="PreCollapsed" select="$PreCollapsed"/>
				</xsl:apply-templates>
			</ul>
		</li>
	</xsl:template>
	<xsl:template match="*" mode="collate">
		<xsl:copy>
			<xsl:copy-of select="@*"/>
			<xsl:apply-templates select="node()" mode="collate"/>
		</xsl:copy>
	</xsl:template>
	<xsl:template match="xs:sequence | xs:choice | xs:all" mode="collate">
		<xsl:param name="Referred" select="false()"/>
		<xsl:param name="MinOccurs"/>
		<xsl:param name="MaxOccurs"/>
		<xsl:param name="GlobalComplexTypes"/>
		<xsl:param name="GlobalSimpleTypes"/>
		<xsl:copy>
			<xsl:copy-of select="@*"/>
			<xsl:choose>
				<xsl:when test="$Referred">
					<xsl:call-template name="EnsureMinMaxOccursAttributes">
						<xsl:with-param name="MinOccurs" select="$MinOccurs"/>
						<xsl:with-param name="MaxOccurs" select="$MaxOccurs"/>
					</xsl:call-template>
				</xsl:when>
				<xsl:otherwise>
					<xsl:call-template name="EnsureMinMaxOccursAttributes"/>
				</xsl:otherwise>
			</xsl:choose>
			<xsl:apply-templates select="child::*" mode="collate">
				<xsl:with-param name="GlobalComplexTypes" select="$GlobalComplexTypes"/>
				<xsl:with-param name="GlobalSimpleTypes" select="$GlobalSimpleTypes"/>
			</xsl:apply-templates>
		</xsl:copy>
	</xsl:template>
	<xsl:template match="xs:attribute" xmlns="http://www.w3.org/1999/xhtml">
		<xsl:param name="GlobalSimpleTypes"/>
		<li>
			<xsl:call-template name="WriteName">
				<xsl:with-param name="NamePrefix" select="'@'"/>
				<xsl:with-param name="GlobalSimpleTypes" select="$GlobalSimpleTypes"/>
			</xsl:call-template>
			<xsl:for-each select="xs:simpleType">
				<xsl:call-template name="RenderSimpleType">
					<xsl:with-param name="InlineType" select="true()"/>
					<xsl:with-param name="GlobalSimpleTypes" select="$GlobalSimpleTypes"/>
				</xsl:call-template>
			</xsl:for-each>
		</li>
	</xsl:template>
	<xsl:template match="xs:simpleContentType" xmlns="http://www.w3.org/1999/xhtml">
		<li>
			<i>
				<xsl:text>Content: </xsl:text>
			</i>
			<xsl:call-template name="RenderTypeReference">
				<xsl:with-param name="QName" select="@name"/>
			</xsl:call-template>
		</li>
	</xsl:template>
	<xsl:template match="xs:attributeGroup">
		<xsl:param name="Referred" select="false()"/>
		<xsl:choose>
			<xsl:when test="@ref">
				<xsl:apply-templates select="/xs:schema/xs:attributeGroup[@name=current()/@ref]">
					<xsl:with-param name="Referred" select="true()"/>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:when test="$Referred">
				<xsl:apply-templates select="xs:attribute | xs:attributeGroup"/>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="xs:attributeGroup" mode="collate">
		<xsl:param name="Referred" select="false()"/>
		<xsl:choose>
			<xsl:when test="@ref">
				<xsl:apply-templates select="/xs:schema/xs:attributeGroup[@name=current()/@ref]" mode="collate">
					<xsl:with-param name="Referred" select="true()"/>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:when test="$Referred">
				<xsl:apply-templates select="xs:attribute | xs:attributeGroup" mode="collate"/>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="*" mode="collapse">
		<xsl:copy>
			<xsl:copy-of select="@*"/>
			<xsl:apply-templates select="node()" mode="collapse"/>
		</xsl:copy>
	</xsl:template>
	<xsl:template match="xs:sequence" mode="collapse">
		<xsl:param name="MinOccurs" select="@minOccurs"/>
		<xsl:param name="MaxOccurs" select="@maxOccurs"/>
		<xsl:param name="InChoice" select="false()"/>
		<xsl:param name="InOptionalChoice" select="false()"/>
		<xsl:param name="InOneChoice" select="false()"/>
		<xsl:variable name="children" select="child::*[not(self::xs:annotation)]"/>
		<xsl:choose>
			<xsl:when test="count($children)=1">
				<xsl:call-template name="CollapseSingleChildCompositor">
					<xsl:with-param name="MinOccurs" select="$MinOccurs"/>
					<xsl:with-param name="MaxOccurs" select="$MaxOccurs"/>
					<xsl:with-param name="InChoice" select="$InChoice"/>
					<xsl:with-param name="InOptionalChoice" select="$InOptionalChoice"/>
					<xsl:with-param name="InOneChoice" select="$InOneChoice"/>
					<xsl:with-param name="Child" select="$children[1]"/>
				</xsl:call-template>
			</xsl:when>
			<xsl:when test="not($InChoice) and $MinOccurs=1 and $MaxOccurs=1">
				<xsl:apply-templates select="child::*"  mode="collapse"/>
			</xsl:when>
			<xsl:when test="$MinOccurs=0 and $MaxOccurs=1 and count($children[@minOccurs=0])=count($children)">
				<!-- optional sequence with all optional children, collapse -->
				<xsl:apply-templates select="child::*"  mode="collapse"/>
			</xsl:when>
			<xsl:otherwise>
				<!-- Include this compositor with any overriden min/max values passed in from above -->
				<xsl:copy>
					<xsl:copy-of select="@*"/>
					<xsl:attribute name="_sortPriority">
						<xsl:text>2</xsl:text>
					</xsl:attribute>
					<xsl:copy-of select="$MinOccurs"/>
					<xsl:copy-of select="$MaxOccurs"/>
					<xsl:apply-templates select="child::*" mode="collapse"/>
				</xsl:copy>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="xs:all" mode="collapse">
		<xsl:param name="MinOccurs" select="@minOccurs"/>
		<xsl:param name="MaxOccurs" select="@maxOccurs"/>
		<xsl:param name="InChoice" select="false()"/>
		<xsl:param name="InOptionalChoice" select="false()"/>
		<xsl:param name="InOneChoice" select="false()"/>
		<xsl:variable name="children" select="child::*[not(self::xs:annotation)]"/>
		<xsl:choose>
			<xsl:when test="count($children)=1">
				<xsl:call-template name="CollapseSingleChildCompositor">
					<xsl:with-param name="MinOccurs" select="$MinOccurs"/>
					<xsl:with-param name="MaxOccurs" select="$MaxOccurs"/>
					<xsl:with-param name="InChoice" select="$InChoice"/>
					<xsl:with-param name="InOptionalChoice" select="$InOptionalChoice"/>
					<xsl:with-param name="InOneChoice" select="$InOneChoice"/>
					<xsl:with-param name="Child" select="$children[1]"/>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<!-- Include this compositor with any overriden min/max values passed in from above -->
				<xsl:copy>
					<xsl:copy-of select="@*"/>
					<xsl:attribute name="_sortPriority">
						<xsl:text>4</xsl:text>
					</xsl:attribute>
					<xsl:copy-of select="$MinOccurs"/>
					<xsl:copy-of select="$MaxOccurs"/>
					<xsl:apply-templates select="child::*" mode="collapse"/>
				</xsl:copy>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="xs:choice" mode="collapse">
		<xsl:param name="MinOccurs" select="@minOccurs"/>
		<xsl:param name="MaxOccurs" select="@maxOccurs"/>
		<xsl:param name="InOptionalChoice" select="false()"/>
		<xsl:param name="InOneChoice" select="false()"/>
		<xsl:param name="InChoice" select="false()"/>
		<xsl:variable name="children" select="child::*[not(self::xs:annotation)]"/>
		<xsl:choose>
			<xsl:when test="count($children)=1">
				<xsl:call-template name="CollapseSingleChildCompositor">
					<xsl:with-param name="MinOccurs" select="$MinOccurs"/>
					<xsl:with-param name="MaxOccurs" select="$MaxOccurs"/>
					<xsl:with-param name="Child" select="$children[1]"/>
				</xsl:call-template>
			</xsl:when>
			<xsl:when test="($InChoice or $InOptionalChoice or $InOneChoice) and $MinOccurs=1 and $MaxOccurs=1">
				<xsl:apply-templates select="child::*" mode="collapse">
					<xsl:with-param name="InChoice" select="true()"/>
					<xsl:with-param name="InOptionalChoice" select="$InOptionalChoice"/>
					<xsl:with-param name="InOneChoice" select="$InOneChoice"/>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<!-- Include this compositor with any overriden min/max values passed in from above -->
				<xsl:copy>
					<xsl:copy-of select="@*"/>
					<xsl:attribute name="_sortPriority">
						<xsl:text>3</xsl:text>
					</xsl:attribute>
					<xsl:copy-of select="$MinOccurs"/>
					<xsl:copy-of select="$MaxOccurs"/>
					<xsl:apply-templates select="child::*" mode="collapse">
						<xsl:with-param name="InChoice" select="true()"/>
						<xsl:with-param name="InOptionalChoice" select="$MinOccurs=0"/>
						<xsl:with-param name="InOneChoice" select="$MinOccurs=1 and $MaxOccurs=1"/>
					</xsl:apply-templates>
				</xsl:copy>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="xs:element | xs:any" mode="collapse">
		<xsl:param name="MinOccurs" select="@minOccurs"/>
		<xsl:param name="MaxOccurs" select="@maxOccurs"/>
		<xsl:copy>
			<xsl:copy-of select="@*"/>
			<xsl:attribute name="_sortPriority">
				<xsl:text>0</xsl:text>
			</xsl:attribute>
			<xsl:copy-of select="$MinOccurs"/>
			<xsl:copy-of select="$MaxOccurs"/>
			<xsl:apply-templates select="child::*" mode="collapse"/>
		</xsl:copy>
	</xsl:template>
	<xsl:template name="CollapseSingleChildCompositor">
		<xsl:param name="MinOccurs"/>
		<xsl:param name="MaxOccurs"/>
		<xsl:param name="InChoice"/>
		<xsl:param name="InOptionalChoice"/>
		<xsl:param name="InOneChoice"/>
		<xsl:param name="Child"/>
		<xsl:variable name="currentMin" select="number($MinOccurs)"/>
		<xsl:variable name="currentMax" select="string($MaxOccurs)"/>
		<xsl:variable name="childMin" select="number($Child/@minOccurs)"/>
		<xsl:variable name="childMax" select="string($Child/@maxOccurs)"/>
		<xsl:variable name="dummyFragment">
			<!-- Collect minOccurs/maxOccurs attributes on a dummy node. Both must be present
				 to continue with collapse. -->
			<dummy>
				<xsl:choose>
					<xsl:when test="$currentMin=0">
						<xsl:choose>
							<xsl:when test="$childMin=0 or $childMin=1">
								<!-- pick up 0 min -->
								<xsl:copy-of select="$MinOccurs"/>
							</xsl:when>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:attribute name="minOccurs">
							<xsl:value-of select="$currentMin * $childMin"/>
						</xsl:attribute>
					</xsl:otherwise>
				</xsl:choose>
				<xsl:choose>
					<xsl:when test="$currentMax=1">
						<xsl:copy-of select="$Child/@maxOccurs"/>
					</xsl:when>
					<xsl:when test="$currentMax='unbounded'">
						<xsl:choose>
							<xsl:when test="$childMax=1 or $childMax='unbounded'">
								<!-- pick up unbounded max -->
								<xsl:copy-of select="$MaxOccurs"/>
							</xsl:when>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:choose>
							<xsl:when test="$childMax=1">
								<!-- pick up parent max -->
								<xsl:copy-of select="$MaxOccurs"/>
							</xsl:when>
							<xsl:when test="$childMax='unbounded'">
								<xsl:copy-of select="$Child/@maxOccurs"/>
							</xsl:when>
						</xsl:choose>
					</xsl:otherwise>
				</xsl:choose>
			</dummy>
		</xsl:variable>
		<xsl:variable name="dummy" select="exsl:node-set($dummyFragment)/child::*"/>
		<xsl:choose>
			<xsl:when test="count($dummy/@*)=2">
				<!-- Skip the compositor but pick up the children with new
					 min/max values -->
				<xsl:apply-templates select="child::*" mode="collapse">
					<xsl:with-param name="MinOccurs" select="$dummy/@minOccurs"/>
					<xsl:with-param name="MaxOccurs" select="$dummy/@maxOccurs"/>
					<xsl:with-param name="InChoice" select="$InChoice"/>
					<xsl:with-param name="InOptionalChoice" select="$InOptionalChoice"/>
					<xsl:with-param name="InOneChoice" select="$InOneChoice"/>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<!-- Include this compositor with any overriden min/max values passed in from above -->
				<xsl:copy>
					<xsl:copy-of select="@*"/>
					<xsl:copy-of select="$MinOccurs"/>
					<xsl:copy-of select="$MaxOccurs"/>
					<xsl:apply-templates select="child::*" mode="collapse">
						<xsl:with-param name="InChoice" select="self::xs:choice"/>
						<xsl:with-param name="InOptionalChoice" select="self::xs:choice and $MinOccurs=0"/>
						<xsl:with-param name="InOneChoice" select="self::xs:choice and $MinOccurs=1 and $MaxOccurs=1"/>
					</xsl:apply-templates>
				</xsl:copy>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>