﻿// Generate threw an exception
// The 'name' attribute is invalid - The value '' is invalid according to its datatype 'http://schemas.neumont.edu/CodeGeneration/PLiX:callNameType' - The value '' is not valid according to any of the memberTypes of the union.
//    at System.Xml.Schema.XmlSchemaValidator.SendValidationEvent(XmlSchemaValidationException e, XmlSeverityType severity)
//   at System.Xml.Schema.XmlSchemaValidator.SendValidationEvent(String code, String[] args, Exception innerException)
//   at System.Xml.Schema.XmlSchemaValidator.CheckAttributeValue(Object value, SchemaAttDef attdef)
//   at System.Xml.Schema.XmlSchemaValidator.ValidateAttribute(String lName, String ns, XmlValueGetter attributeValueGetter, String attributeStringValue, XmlSchemaInfo schemaInfo)
//   at System.Xml.Schema.XmlSchemaValidator.ValidateAttribute(String localName, String namespaceUri, XmlValueGetter attributeValue, XmlSchemaInfo schemaInfo)
//   at System.Xml.XsdValidatingReader.ValidateAttributes()
//   at System.Xml.XsdValidatingReader.ProcessElementEvent()
//   at System.Xml.XsdValidatingReader.ProcessReaderEvent()
//   at System.Xml.XsdValidatingReader.Read()
//   at System.Xml.XmlWrappingReader.Read()
//   at System.Xml.Xsl.Runtime.WhitespaceRuleReader.Read()
//   at System.Xml.XPath.XPathDocument.LoadFromReader(XmlReader reader, XmlSpace space)
//   at System.Xml.XPath.XPathDocument..ctor(XmlReader reader, XmlSpace space)
//   at System.Xml.Xsl.Runtime.XmlQueryContext.ConstructDocument(Object dataSource, String uriRelative, Uri uriResolved)
//   at System.Xml.Xsl.Runtime.XmlQueryContext..ctor(XmlQueryRuntime runtime, Object defaultDataSource, XmlResolver dataSources, XsltArgumentList argList, WhitespaceRuleLookup wsRules)
//   at System.Xml.Xsl.Runtime.XmlQueryRuntime..ctor(XmlILCommand cmd, Object defaultDataSource, XmlResolver dataSources, XsltArgumentList argList, XmlSequenceWriter seqWrt)
//   at System.Xml.Xsl.XmlILCommand.Execute(Object defaultDocument, XmlResolver dataSources, XsltArgumentList argumentList, XmlSequenceWriter results)
//   at System.Xml.Xsl.XmlILCommand.Execute(Object defaultDocument, XmlResolver dataSources, XsltArgumentList argumentList, XmlWriter writer, Boolean closeWriter)
//   at System.Xml.Xsl.XmlILCommand.Execute(XmlReader contextDocument, XmlResolver dataSources, XsltArgumentList argumentList, TextWriter results)
//   at System.Xml.Xsl.XslCompiledTransform.Transform(XmlReader input, XsltArgumentList arguments, TextWriter results)
//   at Neumont.Tools.CodeGeneration.PlixLoaderCustomTool.GenerateCode(String fileContents, String defaultNamespace) in c:\Documents and Settings\jarnold\My Documents\Classes\Fall 2006\Projects IV\ORMCodeGen\CodeGenCustomTool\PlixLoaderCustomTool.cs:line 695
// Info from InnerException
// The 'name' attribute is invalid - The value '' is invalid according to its datatype 'http://schemas.neumont.edu/CodeGeneration/PLiX:callNameType' - The value '' is not valid according to any of the memberTypes of the union.
//    at System.Xml.Schema.XmlSchemaValidator.SendValidationEvent(XmlSchemaValidationException e, XmlSeverityType severity)
//   at System.Xml.Schema.XmlSchemaValidator.SendValidationEvent(String code, String[] args, Exception innerException)
//   at System.Xml.Schema.XmlSchemaValidator.CheckAttributeValue(Object value, SchemaAttDef attdef)
//   at System.Xml.Schema.XmlSchemaValidator.ValidateAttribute(String lName, String ns, XmlValueGetter attributeValueGetter, String attributeStringValue, XmlSchemaInfo schemaInfo)
//   at System.Xml.Schema.XmlSchemaValidator.ValidateAttribute(String localName, String namespaceUri, XmlValueGetter attributeValue, XmlSchemaInfo schemaInfo)
//   at System.Xml.XsdValidatingReader.ValidateAttributes()
//   at System.Xml.XsdValidatingReader.ProcessElementEvent()
//   at System.Xml.XsdValidatingReader.ProcessReaderEvent()
//   at System.Xml.XsdValidatingReader.Read()
//   at System.Xml.XmlWrappingReader.Read()
//   at System.Xml.Xsl.Runtime.WhitespaceRuleReader.Read()
//   at System.Xml.XPath.XPathDocument.LoadFromReader(XmlReader reader, XmlSpace space)
//   at System.Xml.XPath.XPathDocument..ctor(XmlReader reader, XmlSpace space)
//   at System.Xml.Xsl.Runtime.XmlQueryContext.ConstructDocument(Object dataSource, String uriRelative, Uri uriResolved)
//   at System.Xml.Xsl.Runtime.XmlQueryContext..ctor(XmlQueryRuntime runtime, Object defaultDataSource, XmlResolver dataSources, XsltArgumentList argList, WhitespaceRuleLookup wsRules)
//   at System.Xml.Xsl.Runtime.XmlQueryRuntime..ctor(XmlILCommand cmd, Object defaultDataSource, XmlResolver dataSources, XsltArgumentList argList, XmlSequenceWriter seqWrt)
//   at System.Xml.Xsl.XmlILCommand.Execute(Object defaultDocument, XmlResolver dataSources, XsltArgumentList argumentList, XmlSequenceWriter results)
//   at System.Xml.Xsl.XmlILCommand.Execute(Object defaultDocument, XmlResolver dataSources, XsltArgumentList argumentList, XmlWriter writer, Boolean closeWriter)
//   at System.Xml.Xsl.XmlILCommand.Execute(XmlReader contextDocument, XmlResolver dataSources, XsltArgumentList argumentList, TextWriter results)
//   at System.Xml.Xsl.XslCompiledTransform.Transform(XmlReader input, XsltArgumentList arguments, TextWriter results)
//   at Neumont.Tools.CodeGeneration.PlixLoaderCustomTool.GenerateCode(String fileContents, String defaultNamespace) in c:\Documents and Settings\jarnold\My Documents\Classes\Fall 2006\Projects IV\ORMCodeGen\CodeGenCustomTool\PlixLoaderCustomTool.cs:line 695
#error NUPlixLoader Exception