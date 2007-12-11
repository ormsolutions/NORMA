<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet
	version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	exclude-result-prefixes="xsl">
	<xsl:output method="xml" indent="yes"/>
	<xsl:template match="/">
		<settings>
			<setting name="DataAccessLayerNamespace">Data</setting>
			<setting name="BusinessLogicLayerNamespace">EntityLayer</setting>
			<setting name="IncludeCustoms"/>
			<setting name="IncludeDelete"/>
			<setting name="IncludeDrop"/>
			<setting name="IncludeFind"/>
			<setting name="IncludeGet"/>
			<setting name="IncludeGetList"/>
			<setting name="IncludeGetListByFK"/>
			<setting name="IncludeGetListByIX"/>
			<setting name="IncludeInsert"/>
			<setting name="IncludeManyToMany"/>
			<setting name="IncludeRelations"/>
			<setting name="IncludeSave"/>
			<setting name="IncludeUpdate"/>
			<setting name="ChangeUnderscoreToPascalCase"/>
			<setting name="IncludeWCFDataAttributes"/>
			<setting name="SerializeEntityState"/>
			<setting name="UsePascalCasing">Style2</setting>
			<setting name="MethodNames">
				<methodName name="BulkInsert">BulkInsert</methodName>
				<methodName name="DeepLoad">DeepLoad</methodName>
				<methodName name="DeepSave">DeepSave</methodName>
				<methodName name="Delete">Delete</methodName>
				<methodName name="Find">Find</methodName>
				<methodName name="Get">Get</methodName>
				<methodName name="GetAll">GetAll</methodName>
				<methodName name="GetPaged">GetPaged</methodName>
				<methodName name="GetTotalItems">GetTotalItems</methodName>
				<methodName name="Insert">Insert</methodName>
				<methodName name="Save">Save</methodName>
				<methodName name="Update">Update</methodName>
			</setting>
		</settings>
	</xsl:template>
</xsl:stylesheet>