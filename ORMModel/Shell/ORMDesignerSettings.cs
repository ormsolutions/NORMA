//#define DEBUG_CONVERTER_TRANSFORMS
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio;
using Neumont.Tools.ORM.ObjectModel;

namespace Neumont.Tools.ORM.Shell
{
	/// <summary>
	/// A class used to read XmlConverters section of the designer settings file
	/// and run transforms between registered converter types.
	/// </summary>
	public class ORMDesignerSettings
	{
		#region Schema definition classes
		#region ORMDesignerSchema class
		private static class ORMDesignerSchema
		{
			#region String Constants
			public const string SchemaNamespace = "http://schemas.neumont.edu/ORM/DesignerSettings";
			public const string SettingsElement = "settings";

			public const string XmlConvertersElement = "xmlConverters";
			public const string XmlConverterElement = "xmlConverter";
			public const string TransformFileAttribute = "transformFile";
			public const string SourceElementAttribute = "sourceElement";
			public const string TargetElementAttribute = "targetElement";
			public const string DescriptionAttribute = "description";

			public const string TransformParameterElement = "transformParameter";
			public const string NameAttribute = "name";
			public const string ValueAttribute = "value";

			public const string ExtensionClassElement = "extensionClass";
			public const string XslNamespaceAttribute = "xslNamespace";
			public const string ClassNameAttribute = "className";
			#endregion // String Constants
			#region Static properties
			private static ORMDesignerNameTable myNames;
			public static ORMDesignerNameTable Names
			{
				get
				{
					ORMDesignerNameTable retVal = myNames;
					if (retVal == null)
					{
						lock (LockObject)
						{
							retVal = myNames;
							if (retVal == null)
							{
								retVal = myNames = new ORMDesignerNameTable();
							}
						}
					}
					return retVal;
				}
			}
			private static XmlReaderSettings myReaderSettings;
			public static XmlReaderSettings ReaderSettings
			{
				get
				{
					XmlReaderSettings retVal = myReaderSettings;
					if (retVal == null)
					{
						lock (LockObject)
						{
							retVal = myReaderSettings;
							if (retVal == null)
							{
								retVal = myReaderSettings = new XmlReaderSettings();
								retVal.ValidationType = ValidationType.Schema;
								retVal.Schemas.Add(SchemaNamespace, new XmlTextReader(typeof(ORMDesignerSettings).Assembly.GetManifestResourceStream(typeof(ORMDesignerSettings), "ORMDesignerSettings.xsd")));
								retVal.NameTable = Names;
							}
						}
					}
					return retVal;
				}
			}
			#endregion // Static properties
		}
		#endregion // ORMDesignerSchema class
		#region ORMDesignerNameTable class
		private class ORMDesignerNameTable : NameTable
		{
			public readonly string SchemaNamespace;
			public readonly string SettingsElement;
			public readonly string XmlConvertersElement;
			public readonly string XmlConverterElement;
			public readonly string TransformFileAttribute;
			public readonly string SourceElementAttribute;
			public readonly string TargetElementAttribute;
			public readonly string DescriptionAttribute;
			public readonly string TransformParameterElement;
			public readonly string NameAttribute;
			public readonly string ValueAttribute;
			public readonly string ExtensionClassElement;
			public readonly string XslNamespaceAttribute;
			public readonly string ClassNameAttribute;

			public ORMDesignerNameTable()
				: base()
			{
				SchemaNamespace = Add(ORMDesignerSchema.SchemaNamespace);
				SettingsElement = Add(ORMDesignerSchema.SettingsElement);
				XmlConvertersElement = Add(ORMDesignerSchema.XmlConvertersElement);
				XmlConverterElement = Add(ORMDesignerSchema.XmlConverterElement);
				TransformFileAttribute = Add(ORMDesignerSchema.TransformFileAttribute);
				SourceElementAttribute = Add(ORMDesignerSchema.SourceElementAttribute);
				TargetElementAttribute = Add(ORMDesignerSchema.TargetElementAttribute);
				DescriptionAttribute = Add(ORMDesignerSchema.DescriptionAttribute);
				TransformParameterElement = Add(ORMDesignerSchema.TransformParameterElement);
				NameAttribute = Add(ORMDesignerSchema.NameAttribute);
				ValueAttribute = Add(ORMDesignerSchema.ValueAttribute);
				ExtensionClassElement = Add(ORMDesignerSchema.ExtensionClassElement);
				XslNamespaceAttribute = Add(ORMDesignerSchema.XslNamespaceAttribute);
				ClassNameAttribute = Add(ORMDesignerSchema.ClassNameAttribute);
			}
		}
		#endregion // PlixLoaderNameTable class
		#endregion // Schema definition classes
		#region Member Variables
		private IServiceProvider myServiceProvider;
		private bool myIsLoaded;
		private const string ORMDesignerRelativeDirectory = @"\..\..\Neumont\ORMDesigner\";
		private const string XmlConvertersDirectory = @"XmlConverters\";
		private const string ORMDesignerGlobalSettingsFile = "ORMDesignerSettings.xml";
		private Dictionary<XmlElementIdentifier, LinkedList<TransformNode>> myXmlConverters;
		#endregion // Member Variables
		#region Static Variables
		private static string mySettingsDirectory;
		private static object myLockObject;
		private static object LockObject
		{
			get
			{
				if (myLockObject == null)
				{
					object lockObj = new object();
					System.Threading.Interlocked.CompareExchange(ref myLockObject, lockObj, null);
				}
				return myLockObject;
			}
		}
		#endregion // Static Variables
		#region Constructors
		/// <summary>
		/// Construct new designer settings
		/// </summary>
		/// <param name="serviceProvider">The service provider to use</param>
		public ORMDesignerSettings(IServiceProvider serviceProvider)
		{
			myServiceProvider = serviceProvider;
		}
		#endregion // Constructors
		#region SettingsDirectory property
		private string SettingsDirectory
		{
			get
			{
				string retVal = mySettingsDirectory;
				if (retVal == null)
				{
					lock (LockObject)
					{
						if (null == (retVal = mySettingsDirectory))
						{
							IVsShell shellService = (IVsShell)myServiceProvider.GetService(typeof(IVsShell));
							object installVar;
							ErrorHandler.ThrowOnFailure(shellService.GetProperty((int)__VSSPROPID.VSSPROPID_InstallDirectory, out installVar));
							string vsInstallDir = (string)installVar;
							mySettingsDirectory = retVal = (new FileInfo(vsInstallDir + ORMDesignerRelativeDirectory)).FullName;
						}
					}
				}
				return retVal;
			}
		}
		#endregion // SettingsDirectory property
		#region ConvertStream method
		/// <summary>
		/// Convert the given stream to a new stream with converted contents.
		/// Regardless of the action taken, the starting stream is returned
		/// at its original position.
		/// </summary>
		/// <param name="stream">The starting stream</param>
		/// <returns>A new stream with modified content. The caller is responsible for disposing the new stream.</returns>
		public Stream ConvertStream(Stream stream)
		{
			EnsureGlobalSettingsLoaded();
			if (myXmlConverters == null)
			{
				return null;
			}
			long startingPosition = stream.Position;
			try
			{
				XmlReaderSettings readerSettings = new XmlReaderSettings();
				readerSettings.CloseInput = false;
				TransformNode node = default(TransformNode);
				bool haveTransform = false;
				using (XmlReader reader = XmlReader.Create(stream, readerSettings))
				{
					reader.MoveToContent();
					if (reader.NodeType == XmlNodeType.Element)
					{
						string namespaceURI = reader.NamespaceURI;
						string localName = reader.LocalName;
						if (namespaceURI != ORMSerializer.RootXmlNamespace || localName != ORMSerializer.RootXmlElementName)
						{
							XmlElementIdentifier sourceId = new XmlElementIdentifier(namespaceURI, localName);
							if (myXmlConverters != null)
							{
								LinkedList<TransformNode> nodes;
								if (myXmlConverters.TryGetValue(sourceId, out nodes))
								{
									// UNDONE: If there is more than one transform, then ask the user
									node = nodes.First.Value;
									haveTransform = true;
								}
							}
						}
					}
				}
				if (haveTransform)
				{
					stream.Position = startingPosition;
					using (XmlReader reader = XmlReader.Create(stream, readerSettings))
					{
						MemoryStream outputStream = new MemoryStream();
						XmlWriterSettings writerSettings = new XmlWriterSettings();
						writerSettings.CloseOutput = false;
						using (XmlWriter writer = XmlWriter.Create(outputStream, writerSettings))
						{
							node.Transform.Transform(reader, node.Arguments, writer);
						}
						outputStream.Position = 0;
						Stream nextStream = ConvertStream(outputStream);
						if (nextStream != null)
						{
							outputStream.Dispose();
							return nextStream;
						}
						return outputStream;
					}
				}
			}
			finally
			{
				stream.Position = startingPosition;
			}
			return null;
		}
		#endregion // ConvertStream method
		#region Global Settings Loader
		private void EnsureGlobalSettingsLoaded()
		{
			if (myIsLoaded)
			{
				return;
			}
			myIsLoaded = false;
			string settingsFile = SettingsDirectory + ORMDesignerGlobalSettingsFile;
			if (File.Exists(settingsFile))
			{
				ORMDesignerNameTable names = ORMDesignerSchema.Names;
				using (FileStream designerSettingsStream = new FileStream(settingsFile, FileMode.Open, FileAccess.Read))
				{
					using (XmlTextReader settingsReader = new XmlTextReader(new StreamReader(designerSettingsStream), names))
					{
						using (XmlReader reader = XmlReader.Create(settingsReader, ORMDesignerSchema.ReaderSettings))
						{
							if (XmlNodeType.Element == reader.MoveToContent())
							{
								if (TestElementName(reader.NamespaceURI, names.SchemaNamespace) && TestElementName(reader.LocalName, names.SettingsElement))
								{
									while (reader.Read())
									{
										XmlNodeType nodeType = reader.NodeType;
										if (nodeType == XmlNodeType.Element)
										{
											if (TestElementName(reader.LocalName, names.XmlConvertersElement))
											{
												ProcessXmlConverters(reader, names);
											}
											else
											{
												Debug.Fail("Validating reader should have failed");
												PassEndElement(reader);
											}
										}
										else if (nodeType == XmlNodeType.EndElement)
										{
											break;
										}
									}
								}
							}
						}
					}
				}
			}
		}
		private void ProcessXmlConverters(XmlReader reader, ORMDesignerNameTable names)
		{
			if (reader.IsEmptyElement)
			{
				return;
			}
			while (reader.Read())
			{
				XmlNodeType nodeType = reader.NodeType;
				if (nodeType == XmlNodeType.Element)
				{
					if (TestElementName(reader.LocalName, names.XmlConverterElement))
					{
						ProcessXmlConverter(reader, names);
					}
					else
					{
						Debug.Fail("Validating reader should have failed");
						PassEndElement(reader);
					}
				}
				else if (nodeType == XmlNodeType.EndElement)
				{
					break;
				}
			}
		}
		#region Helper Classes
		#region XmlElementIdentifier Structure
		private struct XmlElementIdentifier
		{
			#region Member Variables
			/// <summary>
			/// The namespace for the element. Can be an empty string.
			/// </summary>
			private string myNamespaceURI;
			/// <summary>
			/// The local name of the element. Cannot be an empty string.
			/// </summary>
			private string myLocalName;
			#endregion // Member Variables
			#region Constructors
			/// <summary>
			/// Create an element identifier based on a known namespace and unqualified element name
			/// </summary>
			/// <param name="namespaceURI">The namespace for the element. Can be an empty string.</param>
			/// <param name="localName">The local name of the element. Cannot be an empty string.</param>
			public XmlElementIdentifier(string namespaceURI, string localName)
			{
				myNamespaceURI = namespaceURI;
				myLocalName = localName;
			}
			/// <summary>
			/// Parse the provided element name to find the namespaceURI and local name for the element
			/// </summary>
			/// <param name="elementName">A qualified xml name</param>
			/// <param name="reader">A reader used to resolve namespace prefixes</param>
			public XmlElementIdentifier(string elementName, XmlReader reader)
			{
				if (elementName == ".ORMRoot")
				{
					myNamespaceURI = ORMSerializer.RootXmlNamespace;
					myLocalName = ORMSerializer.RootXmlElementName;
				}
				else if (elementName == ".ORMModel")
				{
					myNamespaceURI = ORMModel.RootXmlNamespace;
					myLocalName = ORMModel.RootXmlElementName;
				}
				else
				{
					int colonIndex = elementName.IndexOf(':');
					if (colonIndex != -1)
					{
						myLocalName = elementName.Substring(colonIndex + 1);
						myNamespaceURI = reader.LookupNamespace(elementName.Substring(0, colonIndex));
					}
					else
					{
						myNamespaceURI = "";
						myLocalName = elementName;
					}
				}
			}
			#endregion // Constructors
			#region Accessor Functions
			/// <summary>
			/// The namespace for the element. Can be an empty string.
			/// </summary>
			public string NamespaceURI
			{
				get
				{
					return myNamespaceURI;
				}
			}
			/// <summary>
			/// The local name of the element. Cannot be an empty string.
			/// </summary>
			public string LocalName
			{
				get
				{
					return myLocalName;
				}
			}
			#endregion // Accessor Functions
		}
		#endregion // XmlElementIdentifier Structure
		#region TransformNode Structure
		private struct TransformNode
		{
			#region Member Variables
			private string myDescription;
			private XslCompiledTransform myTransform;
			private string myTransformFile;
			private XsltArgumentList myArguments;
			private XmlElementIdentifier myTargetElement;
			#endregion // Member Variables
			#region Constructors
			public TransformNode(XmlElementIdentifier targetElement, string description, string transformFile, XsltArgumentList arguments)
			{
				myTargetElement = targetElement;
				myDescription = description;
				myTransform = null;
				myTransformFile = transformFile;
				myArguments = arguments;
			}
			#endregion // Constructors
			#region Accessor Functions
			/// <summary>
			/// Description of the transform
			/// </summary>
			public string Description
			{
				get
				{
					return myDescription;
				}
			}
			/// <summary>
			/// The transform to apply
			/// </summary>
			public XslCompiledTransform Transform
			{
				get
				{
					XslCompiledTransform retVal = myTransform;
					if (retVal == null)
					{
#if DEBUG_CONVERTER_TRANSFORMS 
						retVal = new XslCompiledTransform(true);
#else
						retVal = new XslCompiledTransform();
#endif
						retVal.Load(myTransformFile);
						myTransform = retVal;
					}
					return retVal;
				}
			}
			/// <summary>
			/// The element identifier for the expected output
			/// </summary>
			public XmlElementIdentifier TargetElement
			{
				get
				{
					return myTargetElement;
				}
			}
			/// <summary>
			/// The argument list to pass to the transform
			/// </summary>
			public XsltArgumentList Arguments
			{
				get
				{
					return myArguments;
				}
			}
			#endregion // Accessor Functions
		}
		#endregion // TransformNode Structure
		#endregion // Helper Classes
		private void ProcessXmlConverter(XmlReader reader, ORMDesignerNameTable names)
		{
			if (myXmlConverters == null)
			{
				myXmlConverters = new Dictionary<XmlElementIdentifier, LinkedList<TransformNode>>();
			}
			string sourceElement = reader.GetAttribute(names.SourceElementAttribute);
			string targetElement = reader.GetAttribute(names.TargetElementAttribute);
			string transformFile = reader.GetAttribute(names.TransformFileAttribute);
			string description = reader.GetAttribute(names.DescriptionAttribute);

			XmlElementIdentifier sourceIdentifier = new XmlElementIdentifier(sourceElement, reader);
			XmlElementIdentifier targetIdentifier = new XmlElementIdentifier(targetElement, reader);
			XsltArgumentList arguments = null;

			if (!reader.IsEmptyElement)
			{
				while (reader.Read())
				{
					XmlNodeType nodeType = reader.NodeType;
					if (nodeType == XmlNodeType.Element)
					{
						string localName = reader.LocalName;
						if (TestElementName(localName, names.TransformParameterElement))
						{
							// Add an argument for the transform
							if (arguments == null)
							{
								arguments = new XsltArgumentList();
							}
							arguments.AddParam(reader.GetAttribute(names.NameAttribute), "", reader.GetAttribute(names.ValueAttribute));
							PassEndElement(reader);
						}
						else if (TestElementName(localName, names.ExtensionClassElement))
						{
							// Load an extension class and associate it with an extension namespace
							// used by the transform
							if (arguments == null)
							{
								arguments = new XsltArgumentList();
							}
							arguments.AddExtensionObject(reader.GetAttribute(names.XslNamespaceAttribute), Type.GetType(reader.GetAttribute(names.ClassNameAttribute), true, false).GetConstructor(Type.EmptyTypes).Invoke(new object[0]));
							PassEndElement(reader);
						}
						else
						{
							Debug.Fail("Validating reader should have failed");
							PassEndElement(reader);
						}
					}
					else if (nodeType == XmlNodeType.EndElement)
					{
						break;
					}
				}
			}
			TransformNode transformNode = new TransformNode(targetIdentifier, description, SettingsDirectory + XmlConvertersDirectory + transformFile, arguments);
			LinkedList<TransformNode> nodes;
			if (myXmlConverters.TryGetValue(sourceIdentifier, out nodes))
			{
				nodes.AddLast(new LinkedListNode<TransformNode>(transformNode));
			}
			else
			{
				nodes = new LinkedList<TransformNode>();
				nodes.AddFirst(new LinkedListNode<TransformNode>(transformNode));
				myXmlConverters.Add(sourceIdentifier, nodes);
			}
		}
		private static bool TestElementName(string localName, string elementName)
		{
			return object.ReferenceEquals(localName, elementName);
		}
		/// <summary>
		/// Move the reader to the node immediately after the end element corresponding to the current open element
		/// </summary>
		/// <param name="reader">The XmlReader to advance</param>
		private static void PassEndElement(XmlReader reader)
		{
			if (!reader.IsEmptyElement)
			{
				bool finished = false;
				while (!finished && reader.Read())
				{
					switch (reader.NodeType)
					{
						case XmlNodeType.Element:
							PassEndElement(reader);
							break;

						case XmlNodeType.EndElement:
							finished = true;
							break;
					}
				}
			}
		}
		#endregion // Global Settings Loader
	}
}
