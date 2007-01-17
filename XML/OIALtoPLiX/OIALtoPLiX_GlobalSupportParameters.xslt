<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:param name="GenerateCodeAnalysisAttributes" select="true()"/>
	<xsl:param name="GenerateAccessedThroughPropertyAttribute" select="false()"/>
	<xsl:param name="GenerateObjectDataSourceSupport" select="true()"/>
	<xsl:param name="RaiseEventsAsynchronously" select="true()"/>
	<xsl:param name="SynchronizeEventAddRemove" select="true()"/>
	<xsl:param name="DefaultNamespace" select="''"/>
	<xsl:param name="PrivateMemberPrefix" select="'_'"/>
	<xsl:param name="ImplementationClassSuffix" select="'Core'"/>
	<xsl:param name="ModelContextInterfaceImplementationVisibility" select="'public'"/>
	<xsl:param name="Int32MaxValue" select="number(2147483647)"/>
	
	<xsl:variable name="SprocSuffix" select="'SP'"/>
	<xsl:variable name="CollectionSuffix" select="'Collections'"/>
</xsl:stylesheet>