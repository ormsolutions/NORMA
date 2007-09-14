// Generate threw an exception
// The element 'assign' in namespace 'http://schemas.neumont.edu/CodeGeneration/PLiX' has invalid child element 'left' in namespace 'http://schemas.neumont.edu/CodeGeneration/PLiX'.
//    at System.Xml.Schema.XmlSchemaValidator.SendValidationEvent(ValidationEventHandler eventHandler, Object sender, XmlSchemaValidationException e, XmlSeverityType severity)
//   at System.Xml.Schema.XmlSchemaValidator.ElementValidationError(XmlQualifiedName name, ValidationState context, ValidationEventHandler eventHandler, Object sender, String sourceUri, Int32 lineNo, Int32 linePos, Boolean getParticles)
//   at System.Xml.Schema.XmlSchemaValidator.ValidateElementContext(XmlQualifiedName elementName, Boolean& invalidElementInContext)
//   at System.Xml.Schema.XmlSchemaValidator.ValidateElement(String localName, String namespaceUri, XmlSchemaInfo schemaInfo, String xsiType, String xsiNil, String xsiSchemaLocation, String xsiNoNamespaceSchemaLocation)
//   at System.Xml.XsdValidatingReader.ProcessElementEvent()
//   at System.Xml.XsdValidatingReader.ProcessReaderEvent()
//   at System.Xml.XsdValidatingReader.Read()
//   at System.Xml.XmlWrappingReader.Read()
//   at System.Xml.Xsl.Runtime.WhitespaceRuleReader.Read()
//   at System.Xml.XPath.XPathDocument.LoadFromReader(XmlReader reader, XmlSpace space)
//   at System.Xml.XPath.XPathDocument..ctor(XmlReader reader, XmlSpace space)
//   at System.Xml.Xsl.Runtime.XmlQueryContext.ConstructDocument(Object dataSource, String uriRelative, Uri uriResolved)
//   at System.Xml.Xsl.Runtime.XmlQueryContext..ctor(XmlQueryRuntime runtime, Object defaultDataSource, XmlResolver dataSources, XsltArgumentList argList, WhitespaceRuleLookup wsRules)
//   at System.Xml.Xsl.Runtime.XmlQueryRuntime..ctor(XmlQueryStaticData data, Object defaultDataSource, XmlResolver dataSources, XsltArgumentList argList, XmlSequenceWriter seqWrt)
//   at System.Xml.Xsl.XmlILCommand.Execute(Object defaultDocument, XmlResolver dataSources, XsltArgumentList argumentList, XmlSequenceWriter results)
//   at System.Xml.Xsl.XmlILCommand.Execute(Object defaultDocument, XmlResolver dataSources, XsltArgumentList argumentList, XmlWriter writer, Boolean closeWriter)
//   at System.Xml.Xsl.XmlILCommand.Execute(XmlReader contextDocument, XmlResolver dataSources, XsltArgumentList argumentList, TextWriter results)
//   at System.Xml.Xsl.XslCompiledTransform.Transform(XmlReader input, XsltArgumentList arguments, TextWriter results)
//   at Neumont.Tools.CodeGeneration.PlixLoaderCustomTool.GenerateCode(String fileContents, String defaultNamespace) in c:\Projects\ORM\codegen\CodeGenCustomTool\PlixLoaderCustomTool.cs:line 695
#error NUPlixLoader Exception
