<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:exsl="http://exslt.org/common"
	xmlns:esu="urn:schemas-neumont-edu:ORM:ExtensionStripperUtility"
	xmlns:dummy="urn:dummy" 
	extension-element-prefixes="exsl esu"
	exclude-result-prefixes="dummy">

	<xsl:output method="xml" encoding="utf-8" media-type="application/orm+xml" indent="no"/>

	<xsl:variable name="whateverFragment">
		<xsl:call-template name="GetNextSelectedNamespace"/>
	</xsl:variable>
	<xsl:variable name="selectedNamespaces" select="exsl:node-set($whateverFragment)/child::*/@namespaceUri"/>

	<xsl:template name="AddNamespacePrefix">
		<xsl:param name="Prefix"/>
		<xsl:param name="Namespace"/>
		<xsl:variable name="DummyFragment">
			<xsl:choose>
				<xsl:when test="string-length($Prefix)">
					<xsl:element name="{$Prefix}:PickAName" namespace="{$Namespace}"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:element name="PickAName" namespace="{$Namespace}"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:copy-of select="exsl:node-set($DummyFragment)/child::*/namespace::node()[local-name()!='xml']"/>
	</xsl:template>

	<xsl:template match="/">
		<!-- This is written to work with any root element name and namespace that follow the
		SerializationEngine pattern (a root element with all of the namespaces listed on the root and
		the root element namespace used only for the root element).
		The namespace and element name of the root element cannot be changed by this transform. -->
		<xsl:for-each select="*">
			<!-- Do an initial check to see if we're just adding namespaces, or removing them. If we
			are not removing, then we simply need to add the namespaces to the root element instead of
			stripping the document and verifying all references -->
			<xsl:variable name="rootNamespace" select="namespace-uri()"/>
			<xsl:variable name="currentNamespaces" select="namespace::node()[not(local-name()='xml') and not(.=$rootNamespace)]"/>
			<xsl:variable name="testNamespaceRemovalFragment">
				<xsl:for-each select="$currentNamespaces">
					<xsl:if test="not(esu:IsNamespaceActive(.))">
						<xsl:text>1</xsl:text>
					</xsl:if>
				</xsl:for-each>
			</xsl:variable>
			<xsl:choose>
				<xsl:when test="string($testNamespaceRemovalFragment)">
					<!-- Remove namespaces and associated elements and add new namespaces to the root element -->
					<xsl:variable name="namespaceAdjustedDocumentFragment">
						<xsl:element name="{name()}" namespace="{$rootNamespace}">
							<xsl:for-each select="$currentNamespaces">
								<xsl:if test="esu:IsNamespaceActive(.)">
									<xsl:copy-of select="."/>
									<xsl:if test="esu:AddNamespace(.)"/>
								</xsl:if>
							</xsl:for-each>
							<xsl:for-each select="$selectedNamespaces">
								<xsl:if test="not(esu:WasNamespaceAdded(.))">
									<xsl:call-template name="AddNamespacePrefix">
										<xsl:with-param name="Prefix" select="esu:GetRandomPrefix()"/>
										<xsl:with-param name="Namespace" select="."/>
									</xsl:call-template>
									<xsl:if test="esu:AddNamespace(.)"/>
								</xsl:if>
							</xsl:for-each>
							<xsl:apply-templates select="@*|*|text()|comment()" mode="StripRemovedNamespaceElements"/>
						</xsl:element>
					</xsl:variable>
					<!-- Removing elements can leave dangling references. Remove any remaining element with an attribute that
					references a remove identifier. -->
					<xsl:call-template name="StripDanglingReferences">
						<xsl:with-param name="VerifyFragment" select="$namespaceAdjustedDocumentFragment"/>
					</xsl:call-template>
				</xsl:when>
				<xsl:otherwise>
					<!-- Add new namespaces to the root element. The rest does not change -->
					<xsl:element name="{name()}" namespace="{$rootNamespace}">
						<xsl:for-each select="$currentNamespaces">
							<xsl:if test="esu:IsNamespaceActive(.)">
								<xsl:copy-of select="."/>
								<xsl:if test="esu:AddNamespace(.)"/>
							</xsl:if>
						</xsl:for-each>
						<xsl:for-each select="$selectedNamespaces">
							<xsl:if test="not(esu:WasNamespaceAdded(.))">
								<xsl:call-template name="AddNamespacePrefix">
									<xsl:with-param name="Prefix" select="esu:GetRandomPrefix()"/>
									<xsl:with-param name="Namespace" select="."/>
								</xsl:call-template>
								<xsl:if test="esu:AddNamespace(.)"/>
							</xsl:if>
						</xsl:for-each>
						<xsl:copy-of select="@*|*|text()|comment()"/>
					</xsl:element>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:for-each>
	</xsl:template>

	<xsl:template name="GetNextSelectedNamespace">
		<xsl:variable name="nextSelectedNamespace" select="esu:GetNextSelectedNamespace()"/>
		<xsl:if test="$nextSelectedNamespace">
			<dummy:whatever namespaceUri="{$nextSelectedNamespace}"/>
			<xsl:call-template name="GetNextSelectedNamespace"/>
		</xsl:if>
	</xsl:template>

	<xsl:template match="*" mode="StripRemovedNamespaceElements">
		<xsl:variable name="namespace" select="namespace-uri()"/>
		<xsl:choose>
			<xsl:when test="esu:IsNamespaceActive($namespace)">
				<xsl:copy>
					<xsl:apply-templates select="@*|*|text()|comment()" mode="StripRemovedNamespaceElements"/>
				</xsl:copy>
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="." mode="TrackRemovedIds"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="@*|text()|comment()" mode="StripRemovedNamespaceElements">
		<xsl:copy>
			<xsl:apply-templates select="@*|*|text()|comment()" mode="StripRemovedNamespaceElements"/>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*" mode="TrackRemovedIds">
		<xsl:for-each select="@id">
			<xsl:if test="esu:RemoveId(string(.))"/>
		</xsl:for-each>
		<xsl:apply-templates select="*" mode="TrackRemovedIds"/>
	</xsl:template>

	<xsl:template name="StripDanglingReferences">
		<xsl:param name="VerifyFragment"/>
		<xsl:choose>
			<xsl:when test="esu:BeginIdRemovalPhase()">
				<xsl:variable name="removalPhaseFragment">
					<xsl:apply-templates select="exsl:node-set($VerifyFragment)/child::*" mode="VerifyIdReferences"/>
				</xsl:variable>
				<xsl:call-template name="StripDanglingReferences">
					<xsl:with-param name="VerifyFragment" select="$removalPhaseFragment"/>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:copy-of select="$VerifyFragment"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="*" mode="VerifyIdReferences">
		<xsl:variable name="testAttributesFragment">
			<xsl:for-each select="@*[not(name()='id')]">
				<xsl:if test="esu:IsRemovedId(string(.))">
					<xsl:text>1</xsl:text>
				</xsl:if>
			</xsl:for-each>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="string($testAttributesFragment)">
				<xsl:apply-templates select="." mode="TrackRemovedIds"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:copy>
					<xsl:apply-templates select="@*|*|text()|comment()" mode="VerifyIdReferences"/>
				</xsl:copy>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="@*|text()|comment()" mode="VerifyIdReferences">
		<xsl:copy-of select="."/>
	</xsl:template>
</xsl:stylesheet>
