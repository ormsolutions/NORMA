#region Common Public License Copyright Notice
/**************************************************************************\
* Neumont Object-Role Modeling Architect for Visual Studio                 *
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
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using Microsoft.XmlDiffPatch;

namespace CleanDSLDM
{
	class Program
	{
		#region Shared Xml reader/writer settings
		/// <summary>
		/// A shared diff engine used for comparing xml streams
		/// </summary>
		private static readonly XmlDiff DiffEngine = new XmlDiff(XmlDiffOptions.IgnoreComments |
														XmlDiffOptions.IgnoreWhitespace |
														XmlDiffOptions.IgnoreXmlDecl |
														XmlDiffOptions.IgnorePI |
														XmlDiffOptions.IgnorePrefixes);
		#endregion // Shared Xml reader/writer settings
		#region Shared LockObject
		private static object myLockObject;
		/// <summary>
		/// LockObject to share across this and nested classes
		/// </summary>
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
		#endregion // Shared LockObject
		#region Load transform
		private static XslCompiledTransform myIdMapTransform;
		private static XslCompiledTransform IdMapTransform
		{
			get
			{
				XslCompiledTransform retVal = myIdMapTransform;
				if (retVal == null)
				{
					lock (LockObject)
					{
						retVal = myIdMapTransform;
						if (retVal == null)
						{
							retVal = new XslCompiledTransform();
							Type resourceType = typeof(Program);
							using (Stream transformStream = resourceType.Assembly.GetManifestResourceStream(resourceType, "MapIds.xslt"))
							{
								using (StreamReader reader = new StreamReader(transformStream))
								{
									using (XmlReader xmlReader = new XmlTextReader(reader))
									{
										retVal.Load(xmlReader, null, null);
									}
								}
							}
							myIdMapTransform = retVal;
						}
					}
				}
				return retVal;
			}
		}
		#endregion // Load transform
		#region Diffgram processing (pulled from suite code)
		/// <summary>
		/// Used in a stack to track position and value
		/// while walking a an Xml Diff map.
		/// </summary>
		private struct DiffMatchRecord
		{
			public DiffMatchRecord(int level, int match)
			{
				Level = level;
				Match = match;
			}
			/// <summary>
			/// The recursion level
			/// </summary>
			public int Level;
			/// <summary>
			/// The match value from the diff xml
			/// </summary>
			public int Match;
		}
		/// <summary>
		/// Provide a dictionary class suitable to be used as
		/// an extension object from an xslt transform
		/// </summary>
		private class StringMapDictionary : Dictionary<string, string>
		{
			/// <summary>
			/// Map the current value to an alternate
			/// value if one is specified. Otherwise, leave
			/// return the value.
			/// </summary>
			/// <param name="value">Original value</param>
			/// <returns>Mapped value, or original value</returns>
			public string Map(string value)
			{
				string retVal;
				return TryGetValue(value, out retVal) ? retVal : value;
			}
		}
		/// <summary>
		/// The XmlDiffPatch engine spits the new data, but not the old. We need to
		/// reexamine the original document to retrieve the old data for the id attributes.
		/// Note that this routine assumes all ids are globally unique values.
		/// </summary>
		/// <param name="docReader">An initialized reader for the original document</param>
		/// <param name="diffReader">An initialized reader for the xml diffgram</param>
		/// <param name="knownIds">A dictionary of recognized id values. Any changed id that
		/// is known in the original document is not considered mappable.</param>
		/// <returns>Dictionary of replacement id values keyed off the original values</returns>
		private static StringMapDictionary GetIdMap(XmlReader docReader, XmlReader diffReader, Dictionary<string, object> knownIds)
		{
			StringMapDictionary retVal = null;
			Stack<DiffMatchRecord> diffMatchStack = new Stack<DiffMatchRecord>();
			diffMatchStack.Push(new DiffMatchRecord(0, 0)); // Seed so we can Peek safely
			int currentLevel = 0;
			diffReader.MoveToContent(); // Jump to root element
			while (diffReader.Read())
			{
				XmlNodeType diffNodeType = diffReader.NodeType;
				switch (diffNodeType)
				{
					case XmlNodeType.Element:
						string localName = diffReader.LocalName;
						if (localName == "node")
						{
							// Pull the match value from the Xml document
							int newDiffMatch = XmlConvert.ToInt32(diffReader.GetAttribute("match"));

							// Increment out level
							++currentLevel;
							int lastDiffMatch = 0;

							// Manage the stack to reflect the current level and value.
							// We keep the stack as small as possible by never having
							// more than one record on the stack for a level with the same parent.
							DiffMatchRecord lastRecord = diffMatchStack.Pop();
							if (currentLevel < lastRecord.Level)
							{
								do
								{
									lastRecord = diffMatchStack.Pop();
								} while (currentLevel < lastRecord.Level);
							}
							if (lastRecord.Level == currentLevel)
							{
								// Update the value for this level and repush
								lastDiffMatch = lastRecord.Match;
								lastRecord.Match = newDiffMatch;
								diffMatchStack.Push(lastRecord);
							}
							else
							{
								// We popped one too many, push it back on the
								// stack and add a new entry for this level
								diffMatchStack.Push(lastRecord);
								diffMatchStack.Push(new DiffMatchRecord(currentLevel, newDiffMatch));
							}

							// Advance docReader the specified number of elements
							int stopDocAfter = newDiffMatch - lastDiffMatch;
							while (stopDocAfter != 0 && docReader.Read())
							{
								XmlNodeType docNodeType = docReader.NodeType;
								switch (docNodeType)
								{
									case XmlNodeType.XmlDeclaration:
										--stopDocAfter;
										break;
									case XmlNodeType.Element:
										if (0 != --stopDocAfter)
										{
											PassEndElement(docReader);
										}
										break;
								}
							}

							if (diffReader.IsEmptyElement)
							{
								// Treat this just like an end element
								--currentLevel;
								PassEndElement(docReader);
							}
						}
						else if (localName == "change")
						{
							// Note that this is the only part of this
							// routine that is id-specific. The bulk of the
							// routine could be moved to a helper function, and
							// this could be a delegate callback
							string attr = diffReader.GetAttribute("match");
							bool readString = false;
							if (attr != null && attr == "@identity")
							{
								string newId = diffReader.ReadString();
								readString = true;
								if (!knownIds.ContainsKey(newId) && docReader.LocalName == "generatedProperty")
								{
									string originalId = docReader.GetAttribute("identity");
									if (originalId != null && originalId.Length != 0)
									{
										if (retVal == null)
										{
											retVal = new StringMapDictionary();
										}
										retVal[newId] = originalId;
									}
								}
							}
							if (!readString)
							{
								// ReadString causes the reader to advance, don't advance it again.
								PassEndElement(diffReader);
							}
						}
						else
						{
							PassEndElement(diffReader);
						}
						break;
					case XmlNodeType.EndElement:
						if (diffReader.LocalName == "node")
						{
							--currentLevel;
							PassEndElement(docReader);
						}
						break;
				}
			}
			return retVal;
		}
		#endregion // Diffgram processing (pulled from suite code)
		#region Xml Helper Functions
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
		/// <summary>
		/// Get the formatting the way we want it. Getting it consistent
		/// via Xsl is very difficult
		/// </summary>
		/// <param name="textReader">The xml to format</param>
		/// <param name="writer">The writer for the new Xml. Processing instructions should be written before this call.</param>
		private static void FormatXml(TextReader textReader, XmlWriter writer)
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
		#endregion // Xml Helper Functions
		#region Main program
		/// <summary>
		/// Utility function to take a .dsldm file with modified generatedProperty identifier
		/// attributes and return the attributes to the original values in the .svn directory.
		/// The dsldm designer regenerates these guids on every save. This utility will remove
		/// these diffs to allow a diff viewer on the generated ones only.
		/// </summary>
		/// <param name="args">Pass in the name of the model file to fix</param>
		static void Main(string[] args)
		{
			try
			{
				FileInfo modelFile = new FileInfo(args[0]);
				if (!modelFile.Exists)
				{
					throw new FileNotFoundException(modelFile.FullName);
				}
				FileInfo baseFile = new FileInfo(string.Concat(modelFile.DirectoryName, @"\_svn\text-base\", modelFile.Name, ".svn-base"));
				if (!baseFile.Exists)
				{
					baseFile = new FileInfo(string.Concat(modelFile.DirectoryName, @"\.svn\text-base\", modelFile.Name, ".svn-base"));
				}
				if (!baseFile.Exists)
				{
					throw new FileNotFoundException(baseFile.FullName);
				}
				bool hasDiff = false;

				// See if the data is different. If it is different, we'll
				// first attempt to clean up any GUID differences, then compare
				// again to see if differences remain.
				using (FileStream baselineStream = baseFile.OpenRead())
				{
					using (FileStream currentStream = modelFile.OpenRead())
					{
						XmlDiff diff = DiffEngine;
						XmlReaderSettings readerSettings = new XmlReaderSettings();
						readerSettings.CloseInput = false;
						readerSettings.IgnoreWhitespace = true;
						XmlWriterSettings writerSettings = new XmlWriterSettings();
						writerSettings.CloseOutput = false;
						using (MemoryStream diffStream = new MemoryStream())
						{
							using (XmlReader baselineReader = XmlReader.Create(baselineStream, readerSettings))
							{
								using (XmlReader currentReader = XmlTextReader.Create(currentStream, readerSettings))
								{
									using (XmlWriter diffWriter = XmlWriter.Create(diffStream, writerSettings))
									{
										hasDiff = !diff.Compare(baselineReader, currentReader, diffWriter);
									}
								}
							}
							if (hasDiff)
							{
								// Clean up Guid differences
								diffStream.Seek(0, SeekOrigin.Begin);
								// Code for testing purposes, xmlDump will contain the diffgram xml
								//string xmlDump = "";
								//using (StreamReader xmlReader = new StreamReader(diffStream))
								//{
								//    xmlDump = xmlReader.ReadToEnd();
								//}
								baselineStream.Seek(0, SeekOrigin.Begin);
								StringMapDictionary idMap = null;

								// Get a list of recognized id values from the base document.
								// This allows the GetIdMap function to distinguish a mapped
								// id (one created for a new element that is otherwise equivalent
								// to the new element in the base) from a reordered element.
								Dictionary<string, object> knownIds = new Dictionary<string, object>();
								using (XmlReader docReader = XmlReader.Create(baselineStream, readerSettings))
								{
									while (docReader.Read())
									{
										if (docReader.NodeType == XmlNodeType.Element && docReader.LocalName == "generatedProperty")
										{
											string attr = docReader.GetAttribute("identity");
											if (attr != null && attr.Length != 0)
											{
												knownIds[attr] = null;
											}
										}
									}
								}
								baselineStream.Seek(0, SeekOrigin.Begin);

								// Get a map for all changed ids that are not
								// known in the base file to the values they
								// are replacing.
								using (XmlReader docReader = XmlReader.Create(baselineStream, readerSettings))
								{
									using (XmlReader diffReader = XmlTextReader.Create(diffStream, readerSettings))
									{
										idMap = GetIdMap(docReader, diffReader, knownIds);
									}
								}
								diffStream.Seek(0, SeekOrigin.Begin);

								if (idMap != null)
								{
									XsltArgumentList transformArgs = new XsltArgumentList();
									transformArgs.AddExtensionObject("id-map-extension", idMap);
									using (MemoryStream modifiedCurrentStream = new MemoryStream())
									{
										// Transform the current stream to resolve guid differences
										currentStream.Seek(0, SeekOrigin.Begin);
										using (XmlReader currentReader = XmlTextReader.Create(currentStream)) // Use default reader settings (close stream reader close)
										{
											using (XmlWriter modifiedCurrentWriter = XmlWriter.Create(modifiedCurrentStream, writerSettings))
											{
												IdMapTransform.Transform(currentReader, transformArgs, modifiedCurrentWriter);
											}
										}
										modifiedCurrentStream.Seek(0, SeekOrigin.Begin);
										currentStream.Close();
										using (FileStream outStream = modelFile.Open(FileMode.Truncate, FileAccess.Write, FileShare.None))
										{
											writerSettings.CloseOutput = true;
											writerSettings.OmitXmlDeclaration = false;
											writerSettings.Indent = true;
											writerSettings.IndentChars = "  ";
											writerSettings.Encoding = IdMapTransform.OutputSettings.Encoding;
											using (XmlWriter xmlWriter = XmlWriter.Create(outStream, writerSettings))
											{
												xmlWriter.WriteProcessingInstruction("xml", @"version=""1.0"""); // Write it explicitly to make output match designer-saved xml declaration
												FormatXml(new StreamReader(modifiedCurrentStream), xmlWriter);
												xmlWriter.Flush();
											}
										}
									}
								}
							}
						}
					}

				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("This program cleans all generated-property identity differences between two model files. Please enter the path of an existing DSLDM File under TortoiseSVN source control.");
				Console.WriteLine(ex.Message);
				Console.WriteLine("\nPress any key to continue...");
				Console.ReadKey(true);
			}
		}
		#endregion // Main program
	}
}
