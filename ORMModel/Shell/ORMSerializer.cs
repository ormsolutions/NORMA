#region Common Public License Copyright Notice
/**************************************************************************\
* Neumont Object Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
*                                                                          *
* The use and distribution terms for this software are covered by the      *
* Common Public License 1.0 (http://opensource.org/licenses/cpl) which     *
* can be found in the file CPL.txt at the root of this distribution.       *
* By using this software in any fashion, you are agreeing to be bound by   *
* the terms of this license.                                               *
*                                                                          *
* You must not remove this notice, or any other, from this software.       *
\**************************************************************************/
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using SysDiag = System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Threading;
using Microsoft.VisualStudio.EnterpriseTools.Shell;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagnostics;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.ShapeModel;

namespace Neumont.Tools.ORM.Shell
{
	/// <summary>
	/// Read/write .orm files leveraging the default IMS serializer
	/// </summary>
	public partial class ORMSerializer
	{
#if OLDSERIALIZE
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
	<xsl:template match=""om:Property[@Name='Multiplicity']"" />
	<xsl:template match=""om:Property[@Name='DataTypeDisplay']"" />
	<xsl:template match=""om:Property[@Name='ReferenceModeDisplay']"" />
	<xsl:template match=""om:Property[@Name='ReferenceModeString']"" />
	<xsl:template match=""om:Property[@Name='ReferenceMode']"" />
	<xsl:template match=""om:Property[@Name='ValueRangeText']"" />
	<xsl:template match=""om:Property[@Name='KindDisplay']"" />
	<xsl:template match=""om:Property[@Name='Text']"">
		<xsl:choose>
			<xsl:when test=""parent::om:ModelElement[@Type='Neumont.Tools.ORM.ObjectModel.ValueRange']""/>
			<xsl:when test=""parent::om:ModelElement[@Type='Neumont.Tools.ORM.ObjectModel.RoleValueConstraint']""/>
			<xsl:when test=""parent::om:ModelElement[@Type='Neumont.Tools.ORM.ObjectModel.ValueTypeValueConstraint']""/>
			<xsl:otherwise>
				<xsl:copy-of select="".""/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- DataTypeNotSpecifiedError spits as a link to a link, which won't deserialize, so don't save it -->
	<xsl:template match=""om:ModelElement[@Type='Neumont.Tools.ORM.ObjectModel.DataTypeNotSpecifiedError']""/>
</xsl:stylesheet>";
		#endregion // Xsl transforms
		#region Synchronized code to load transform into static variable
		private static object myLockObject;
		private static XslCompiledTransform myTrimMdfOrmTransform;
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
		private static XslCompiledTransform EnsureTransform(ref XslCompiledTransform transform, string xsl)
		{
			if (transform == null)
			{
				lock (LockObject)
				{
					if (transform == null)
					{
						XslCompiledTransform newTrans = new XslCompiledTransform();
						newTrans = new XslCompiledTransform();
						newTrans.Load(new XmlTextReader(new StringReader(xsl)));
						transform = newTrans;
					}
				}
			}
			return transform;
		}
		private static XslCompiledTransform TrimMdfOrmTransform
		{
			get
			{
				return EnsureTransform(ref myTrimMdfOrmTransform, TrimMdfOrmXsl);
			}
		}
		#endregion // Synchronized code to load transform into static variable
		#region Old Member Variables
		/// <summary>
		/// Major Version of File Format
		/// </summary>
		public const int MajorVersion = 1;

		/// <summary>
		/// Minor Version of File Format
		/// </summary>
		public const int MinorVersion = 0;

		/// <summary>
		/// Track when we've seen a diagram. We only want
		/// to record SubjectHasPresentation elements after
		/// a diagram to keep the object and shape portions
		/// of the model separate.
		/// </summary>
		private bool mySeenDiagram;
		#endregion // Old Member Variables
#endif // OLDSERIALIZE
		#region Member Variables
		/// <summary>
		/// Ref Counter for suspending/resuming Modeling Rule Engine
		/// </summary>
		private int myRuleSuspendCount;

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
#if OLDSERIALIZE
		#region Serialize
		/// <summary>
		/// Save the contents of the current store to a stream
		/// </summary>
		/// <param name="stream">An initialized stream</param>
		public void Save1(Stream stream)
		{
			const bool onDiskFormat = true;
			RulesSuspended = true;
			try
			{
				MemoryStream tempStream = new MemoryStream();
				// Write out the MDF format to a temporary string stream
				XmlSerialization.SerializeStore(myStore, tempStream, false, false, MajorVersion, MinorVersion, new XmlSerialization.ShouldSerialize(ShouldSerialize));

				// Strip unwanted elements that won't reload
				tempStream.Position = 0;
				XslCompiledTransform transform = TrimMdfOrmTransform;

				Stream rawStream = stream;
				if (onDiskFormat)
				{
					rawStream = new MemoryStream();
				}
				XmlTextWriter xmlTextWriter = new XmlTextWriter(rawStream, Encoding.UTF8);
				transform.Transform(new XPathDocument(new XmlTextReader(new StreamReader(tempStream)), XmlSpace.None), null, xmlTextWriter);
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
			return;
		}
		/// <summary>
		/// Determine if an element should be serialized
		/// </summary>
		/// <param name="modelElement">Element to test</param>
		/// <returns>true</returns>
		private bool ShouldSerialize(ModelElement modelElement)
		{
			DataType dataType;
			if (modelElement is ExternalFactConstraint ||
				modelElement is ExternalRoleConstraint ||
				modelElement is ExternalConstraintLink ||
				modelElement is ValueRangeLink ||
				modelElement is IntrinsicReferenceMode ||
				modelElement is RolePlayerLink ||
				modelElement is SubtypeLink ||
				modelElement is LinkConnectsToNode)
			{
				return false;
			}
			else if (null != (dataType = modelElement as DataType))
			{
				// If the only reference to this type is the aggregating
				// object type, then don't bother to write it out. These
				// are intrinsic types and will reappear when the model file
				// is reloaded.
				return dataType.GetElementLinks().Count > 1;
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
		/// <param name="fixupManager">Class used to perfom fixup operations
		/// after the load is complete.</param>
		public void Load1(Stream stream, DeserializationFixupManager fixupManager)
		{
			// Leave rules on so all of the links reconnect. Links are not saved.
			RulesSuspended = true;
			try
			{
				XmlSerialization.DeserializeStore(
					myStore,
					stream,
					MajorVersion,
					MinorVersion,
					new XmlSerialization.UpgradeFileFormat(UpgradeFileFormat),
					(fixupManager == null) ? null : new XmlSerialization.Deserialized((fixupManager as INotifyElementAdded).ElementAdded));
				if (fixupManager != null)
				{
					fixupManager.DeserializationComplete();
				}
			}
			finally
			{
				RulesSuspended = false;
			}
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
#endif // OLDSERIALIZE
	}
}