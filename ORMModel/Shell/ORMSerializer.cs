using System;
using System.Collections;
using System.Collections.Generic;
using SysDiag = System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Query;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Threading;
using Microsoft.VisualStudio.EnterpriseTools.Shell;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagnostics;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Northface.Tools.ORM.ObjectModel;
using Northface.Tools.ORM.ShapeModel;
namespace Northface.Tools.ORM.Shell
{
	/// <summary>
	/// Read/write .orm files leveraging the default IMS serializer
	/// </summary>
	public class ORMSerializer
	{
		#region Xsl transforms
		private const string TrimMdfOrmXsl =
@"<?xml version=""1.0"" encoding=""UTF-8"" ?>
<xsl:stylesheet
	version=""1.0""
	xmlns:xsl=""http://www.w3.org/1999/XSL/Transform""
	xmlns:om=""http://Microsoft.VisualStudio.Modeling""
	exclude-result-prefixes=""#default"">
	<xsl:output encoding=""UTF-8""/>
	<xsl:template match=""*"">
		<xsl:copy>
			<xsl:copy-of select=""@*""/>
			<xsl:value-of select=""text()""/>
			<xsl:apply-templates select=""child::*""/>
		</xsl:copy>
	</xsl:template>
	<xsl:template match=""om:Property[@Name='IsValueType']"" />
	<xsl:template match=""om:Property[@Name='IsMandatory']"" />
	<xsl:template match=""om:Property[@Name='NestedFactTypeDisplay']"" />
	<xsl:template match=""om:Property[@Name='RolePlayerDisplay']"" />
	<xsl:template match=""om:Property[@Name='NestingTypeDisplay']"" />
	<xsl:template match=""om:Property[@Name='IsPreferred']"" />
</xsl:stylesheet>";
		#endregion // Xsl transforms
		#region Synchronized code to load transform into static variable
		private static object myLockObject;
		private static XsltCommand myTrimMdfOrmTransform;
		private static object LockObject
		{
			get
			{
				if (myLockObject == null)
				{
					Interlocked.CompareExchange(ref myLockObject, new object(), null);
				}
				return myLockObject;
			}
		}
		private static XsltCommand EnsureTransform(ref XsltCommand transform, string xsl)
		{
			if (transform == null)
			{
				lock (LockObject)
				{
					if (transform == null)
					{
						XsltCommand newTrans = new XsltCommand();
						newTrans = new XsltCommand();
						newTrans.Compile(new XmlTextReader(new StringReader(xsl)));
						transform = newTrans;
					}
				}
			}
			return transform;
		}
		private XsltCommand TrimMdfOrmTransform
		{
			get
			{
				return EnsureTransform(ref myTrimMdfOrmTransform, TrimMdfOrmXsl);
			}
		}
		#endregion // Synchronized code to load transform into static variable
		#region Member Variables
		/// <summary>
		/// Major Version of File Format
		/// </summary>
		public const int MajorVersion = 1;

		/// <summary>
		/// Minor Version of File Format
		/// </summary>
		public const int MinorVersion = 0;

		/// <summary>
		/// Ref Counter for suspending/resuming Modeling Rule Engine
		/// </summary>
		private int myRuleSuspendCount = 0;

		/// <summary>
		/// Track when we've seen a diagram. We only want
		/// to record SubjectHasPresentation elements after
		/// a diagram to keep the object and shape portions
		/// of the model separate.
		/// </summary>
		private bool mySeenDiagram = false;

		/// <summary>
		/// Current store object. Set in constructor
		/// </summary>
		private Store myStore;
		#endregion // Member Variables
		#region Constructor
		/// <summary>
		/// Create a serializer on the given store
		/// </summary>
		/// <param name="store">Store instance</param>
		public ORMSerializer(Store store)
		{
			myStore = store;
		}
		#endregion // Constructor
		#region Rule Suspension
		/// <summary>
		/// Block rules on store during serialization/deserialization
		/// </summary>
		public bool RulesSuspended
		{
			get
			{
				return myRuleSuspendCount > 0;
			}
			set
			{
				if (value)
				{
					// Turn on for first set
					if (1 == ++myRuleSuspendCount)
					{
						myStore.RuleManager.SuspendRuleNotification();
					}
				}
				else
				{
					// Turn off for balanced call
					SysDiag.Debug.Assert(myRuleSuspendCount > 0);
					if (0 == --myRuleSuspendCount)
					{
						myStore.RuleManager.ResumeRuleNotification();
					}
				}
			}
		}
		#endregion // Rule Suspension
		#region Serialize
		/// <summary>
		/// Save the contents of the current store to a stream
		/// </summary>
		/// <param name="stream">An initialized stream</param>
		public void Save(Stream stream)
		{
			const bool onDiskFormat = true;
			RulesSuspended = true;
			try
			{
				MemoryStream tempStream = new MemoryStream();
				// Write out the MDF format to a temporary string stream
				XmlSerialization.SerializeStore(myStore, RootElements, tempStream, false, false, MajorVersion, MinorVersion, new XmlSerialization.ShouldSerialize(ShouldSerialize));

				// Strip unwanted elements that won't reload
				tempStream.Position = 0;
				XsltCommand transform = TrimMdfOrmTransform;

				Stream rawStream = stream;
				if (onDiskFormat)
				{
					rawStream = new MemoryStream();
				}
				XmlTextWriter xmlTextWriter = new XmlTextWriter(rawStream, Encoding.UTF8);
				transform.Execute(new XPathDocument(new XmlTextReader(new StreamReader(tempStream)), XmlSpace.None), null, null, xmlTextWriter);
				if (onDiskFormat)
				{
					rawStream.Position = 0;
					XmlTextWriter xmlWriter = new XmlTextWriter(stream, Encoding.UTF8);
					xmlWriter.Formatting = Formatting.Indented;
					xmlWriter.Namespaces = true;
					xmlWriter.Indentation = 2;
					xmlWriter.IndentChar = ' ';
					xmlWriter.WriteProcessingInstruction("xml", @"version=""1.0"" encoding=""utf-8"" standalone=""yes""");
					FormatXml(new StreamReader(rawStream), xmlWriter);
					xmlWriter.Flush();
				}
			}
			finally
			{
				RulesSuspended = false;
			}
		}
		/// <summary>
		/// Determine if an element should be serialized
		/// </summary>
		/// <param name="modelElement">Element to test</param>
		/// <returns>true</returns>
		private bool ShouldSerialize(ModelElement modelElement)
		{
			if (modelElement is ExternalFactConstraint ||
				modelElement is ExternalRoleConstraint ||
				modelElement is InternalFactConstraint ||
				modelElement is ExternalConstraintLink ||
				modelElement is RolePlayerLink)
			{
				return false;
			}
			else if (modelElement is ORMDiagram)
			{
				mySeenDiagram = true;
			}
			else if (modelElement is SubjectHasPresentation)
			{
				return mySeenDiagram;
			}
			return true; // Serialize everything else.
		}
		/// <summary>
		/// Generate an array of all top level model elements
		/// </summary>
		private ModelElement[] RootElements
		{
			get
			{
				ElementDirectory elementDir = myStore.ElementDirectory;
				return GenerateElementArray(new IList[]
					{
						elementDir.GetElements(ORMModel.MetaClassGuid),
						elementDir.GetElements(DataType.MetaClassGuid),
						elementDir.GetElements(ORMDiagram.MetaClassGuid)
					});
			}
		}
		private ModelElement[] GenerateElementArray(IList[] elementLists)
		{
			int listCount = elementLists.Length;
			int elementCount = 0;
			for (int i = 0; i < listCount; ++i)
			{
				elementCount += elementLists[i].Count;
			}

			ModelElement[] retVal = new ModelElement[elementCount];

			if (elementCount > 0)
			{
				int nextCopyIndex = 0;
				for (int i = 0; i < listCount; ++i)
				{
					IList curList = elementLists[i];
					int curCount = curList.Count;
					if (curCount > 0)
					{
						curList.CopyTo(retVal, nextCopyIndex);
						nextCopyIndex += curCount;
					}
				}
			}
			return retVal;
		}
		/// <summary>
		/// Get the formatting the way we want it. Getting it consistent
		/// via Xsl is very difficult
		/// </summary>
		/// <param name="textReader">The xml to format</param>
		/// <param name="writer">The writer for the new Xml. Processing instructions should be written before this call.</param>
		private static void FormatXml(TextReader textReader, XmlTextWriter writer)
		{
			XmlReader reader = new XmlTextReader(textReader);
			bool emptyElement;
			while (reader.Read())
			{
				switch (reader.NodeType)
				{
					case XmlNodeType.Element:
						writer.WriteStartElement(reader.Prefix, reader.LocalName, reader.NamespaceURI);
						emptyElement = reader.IsEmptyElement; // Read this before moving to an attribute
						while (reader.MoveToNextAttribute())
						{
							writer.WriteAttributeString(reader.Prefix, reader.LocalName, reader.NamespaceURI, reader.Value);
						}
						if (emptyElement)
						{
							writer.WriteEndElement();
						}
						break;
					case XmlNodeType.Text:
						writer.WriteString(reader.Value);
						break;
					case XmlNodeType.CDATA:
						writer.WriteCData(reader.Value);
						break;
					case XmlNodeType.ProcessingInstruction:
						writer.WriteProcessingInstruction(reader.Name, reader.Value);
						break;
					case XmlNodeType.Comment:
						writer.WriteComment(reader.Value);
						break;
					case XmlNodeType.Document:
						System.Diagnostics.Debug.Assert(false, "Hit XmlNodeType.Document, not expected"); // Not expected
						break;
					case XmlNodeType.Whitespace:
						break;
					case XmlNodeType.SignificantWhitespace:
						writer.WriteWhitespace(reader.Value);
						break;
					case XmlNodeType.EndElement:
						writer.WriteEndElement();
						break;
				}
			}
			reader.Close();
		}
		#endregion // Serialize
		#region Deserialize
		/// <summary>
		/// Load the stream contents into the current store
		/// </summary>
		/// <param name="stream">An initialized stream</param>
		public void Load(Stream stream)
		{
			// Leave rules on so all of the links reconnect. Links are not saved.
//			RulesSuspended = true;
//			try
//			{
			XmlSerialization.DeserializeStore(myStore, stream, MajorVersion, MinorVersion, new XmlSerialization.UpgradeFileFormat(UpgradeFileFormat), null);
//			}
//			finally
//			{
//				RulesSuspended = false;
//			}
		}
		/// <summary>
		/// Called to upgrade an old file format.
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="majorVersion"></param>
		/// <param name="minorVersion"></param>
		/// <returns></returns>
		private bool UpgradeFileFormat(ref Stream stream, ref int majorVersion, ref int minorVersion)
		{
			SysDiag.Debug.Fail("Nothing to upgrade yet");
			return false;
		}
		#endregion // Deserialize
	}
}