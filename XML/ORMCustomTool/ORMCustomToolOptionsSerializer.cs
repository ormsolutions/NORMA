namespace Neumont.Tools.ORM.ORMCustomTool.Serialization
{
	public class XmlSerializationReaderORMCustomToolOptions : System.Xml.Serialization.XmlSerializationReader
	{

		public ORMCustomToolOptions Read5_ormCustomToolOptions()
		{
			ORMCustomToolOptions o = null;
			Reader.MoveToContent();
			if (Reader.NodeType == System.Xml.XmlNodeType.Element)
			{
				if (((object)Reader.LocalName == (object)id1_ormCustomToolOptions && (object)Reader.NamespaceURI == (object)id2_Item))
				{
					o = Read4_ORMCustomToolOptions(false, true);
				}
				else
				{
					throw CreateUnknownNodeException();
				}
			}
			else
			{
				UnknownNode(null, @"http://schemas.neumont.edu/ORM/ORMCustomToolOptions:ormCustomToolOptions");
			}
			return o;
		}

		global::Neumont.Tools.ORM.ORMCustomTool.ORMCustomToolOptions Read4_ORMCustomToolOptions(bool isNullable, bool checkType)
		{
			System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
			bool isNull = false;
			if (isNullable) isNull = ReadNull();
			if (checkType)
			{
				if (xsiType == null || ((object)((System.Xml.XmlQualifiedName)xsiType).Name == (object)id3_Item && (object)((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item))
				{
				}
				else
					throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
			}
			if (isNull) return null;
			global::Neumont.Tools.ORM.ORMCustomTool.ORMCustomToolOptions o;
			o = new global::Neumont.Tools.ORM.ORMCustomTool.ORMCustomToolOptions();
			bool[] paramsRead = new bool[2];
			while (Reader.MoveToNextAttribute())
			{
				if (!paramsRead[1] && ((object)Reader.LocalName == (object)id4_outputFileExtension && (object)Reader.NamespaceURI == (object)id3_Item))
				{
					o.@OutputFileExtension = ToXmlNmToken(Reader.Value);
					paramsRead[1] = true;
				}
				else if (!IsXmlnsAttribute(Reader.Name))
				{
					UnknownNode((object)o, @":outputFileExtension");
				}
			}
			Reader.MoveToElement();
			if (Reader.IsEmptyElement)
			{
				Reader.Skip();
				return o;
			}
			Reader.ReadStartElement();
			Reader.MoveToContent();
			int whileIterations0 = 0;
			int readerCount0 = ReaderCount;
			while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None)
			{
				if (Reader.NodeType == System.Xml.XmlNodeType.Element)
				{
					if (!paramsRead[0] && ((object)Reader.LocalName == (object)id5_transformation && (object)Reader.NamespaceURI == (object)id2_Item))
					{
						o.@Transformation = Read3_Transformation(false, true);
						paramsRead[0] = true;
					}
					else
					{
						UnknownNode((object)o, @"http://schemas.neumont.edu/ORM/ORMCustomToolOptions:transformation");
					}
				}
				else
				{
					UnknownNode((object)o, @"http://schemas.neumont.edu/ORM/ORMCustomToolOptions:transformation");
				}
				Reader.MoveToContent();
				CheckReaderCount(ref whileIterations0, ref readerCount0);
			}
			ReadEndElement();
			return o;
		}

		global::Neumont.Tools.ORM.ORMCustomTool.Transformation Read3_Transformation(bool isNullable, bool checkType)
		{
			System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
			bool isNull = false;
			if (isNullable) isNull = ReadNull();
			if (checkType)
			{
				if (xsiType == null || ((object)((System.Xml.XmlQualifiedName)xsiType).Name == (object)id6_Transformation && (object)((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item))
				{
				}
				else
					throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
			}
			if (isNull) return null;
			global::Neumont.Tools.ORM.ORMCustomTool.Transformation o;
			o = new global::Neumont.Tools.ORM.ORMCustomTool.Transformation();
			bool[] paramsRead = new bool[2];
			while (Reader.MoveToNextAttribute())
			{
				if (!paramsRead[1] && ((object)Reader.LocalName == (object)id7_stylesheet && (object)Reader.NamespaceURI == (object)id3_Item))
				{
					o.@Stylesheet = CollapseWhitespace(Reader.Value);
					paramsRead[1] = true;
				}
				else if (!IsXmlnsAttribute(Reader.Name))
				{
					UnknownNode((object)o, @":stylesheet");
				}
			}
			Reader.MoveToElement();
			if (Reader.IsEmptyElement)
			{
				Reader.Skip();
				return o;
			}
			Reader.ReadStartElement();
			Reader.MoveToContent();
			int whileIterations1 = 0;
			int readerCount1 = ReaderCount;
			while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None)
			{
				if (Reader.NodeType == System.Xml.XmlNodeType.Element)
				{
					if (!paramsRead[0] && ((object)Reader.LocalName == (object)id5_transformation && (object)Reader.NamespaceURI == (object)id2_Item))
					{
						o.@InputForXmlSerializer = Read3_Transformation(false, true);
						paramsRead[0] = true;
					}
					else if (!paramsRead[0] && ((object)Reader.LocalName == (object)id8_file && (object)Reader.NamespaceURI == (object)id2_Item))
					{
						o.@InputForXmlSerializer = Read2_TransformationFile(false, true);
						paramsRead[0] = true;
					}
					else
					{
						UnknownNode((object)o, @"http://schemas.neumont.edu/ORM/ORMCustomToolOptions:transformation, http://schemas.neumont.edu/ORM/ORMCustomToolOptions:file");
					}
				}
				else
				{
					UnknownNode((object)o, @"http://schemas.neumont.edu/ORM/ORMCustomToolOptions:transformation, http://schemas.neumont.edu/ORM/ORMCustomToolOptions:file");
				}
				Reader.MoveToContent();
				CheckReaderCount(ref whileIterations1, ref readerCount1);
			}
			ReadEndElement();
			return o;
		}

		global::Neumont.Tools.ORM.ORMCustomTool.TransformationFile Read2_TransformationFile(bool isNullable, bool checkType)
		{
			System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
			bool isNull = false;
			if (isNullable) isNull = ReadNull();
			if (checkType)
			{
				if (xsiType == null || ((object)((System.Xml.XmlQualifiedName)xsiType).Name == (object)id3_Item && (object)((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item))
				{
				}
				else
					throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
			}
			if (isNull) return null;
			global::Neumont.Tools.ORM.ORMCustomTool.TransformationFile o;
			o = new global::Neumont.Tools.ORM.ORMCustomTool.TransformationFile();
			bool[] paramsRead = new bool[1];
			while (Reader.MoveToNextAttribute())
			{
				if (!paramsRead[0] && ((object)Reader.LocalName == (object)id9_href && (object)Reader.NamespaceURI == (object)id3_Item))
				{
					o.@Href = CollapseWhitespace(Reader.Value);
					paramsRead[0] = true;
				}
				else if (!IsXmlnsAttribute(Reader.Name))
				{
					UnknownNode((object)o, @":href");
				}
			}
			Reader.MoveToElement();
			if (Reader.IsEmptyElement)
			{
				Reader.Skip();
				return o;
			}
			Reader.ReadStartElement();
			Reader.MoveToContent();
			int whileIterations2 = 0;
			int readerCount2 = ReaderCount;
			while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None)
			{
				if (Reader.NodeType == System.Xml.XmlNodeType.Element)
				{
					UnknownNode((object)o, @"");
				}
				else
				{
					UnknownNode((object)o, @"");
				}
				Reader.MoveToContent();
				CheckReaderCount(ref whileIterations2, ref readerCount2);
			}
			ReadEndElement();
			return o;
		}

		protected override void InitCallbacks()
		{
		}

		string id2_Item;
		string id4_outputFileExtension;
		string id9_href;
		string id8_file;
		string id7_stylesheet;
		string id3_Item;
		string id6_Transformation;
		string id1_ormCustomToolOptions;
		string id5_transformation;

		protected override void InitIDs()
		{
			id2_Item = Reader.NameTable.Add(@"http://schemas.neumont.edu/ORM/ORMCustomToolOptions");
			id4_outputFileExtension = Reader.NameTable.Add(@"outputFileExtension");
			id9_href = Reader.NameTable.Add(@"href");
			id8_file = Reader.NameTable.Add(@"file");
			id7_stylesheet = Reader.NameTable.Add(@"stylesheet");
			id3_Item = Reader.NameTable.Add(@"");
			id6_Transformation = Reader.NameTable.Add(@"Transformation");
			id1_ormCustomToolOptions = Reader.NameTable.Add(@"ormCustomToolOptions");
			id5_transformation = Reader.NameTable.Add(@"transformation");
		}
	}

	public sealed class ORMCustomToolOptionsSerializer : System.Xml.Serialization.XmlSerializer
	{
		protected override System.Xml.Serialization.XmlSerializationReader CreateReader()
		{
			return new XmlSerializationReaderORMCustomToolOptions();
		}

		public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader)
		{
			return xmlReader.IsStartElement(@"ormCustomToolOptions", @"http://schemas.neumont.edu/ORM/ORMCustomToolOptions");
		}

		protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader)
		{
			return ((XmlSerializationReaderORMCustomToolOptions)reader).Read5_ormCustomToolOptions();
		}
	}
}
