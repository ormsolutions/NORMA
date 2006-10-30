#region zlib/libpng Copyright Notice
/**************************************************************************\
* Neumont Build System                                                     *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
*                                                                          *
* This software is provided 'as-is', without any express or implied        *
* warranty. In no event will the authors be held liable for any damages    *
* arising from the use of this software.                                   *
*                                                                          *
* Permission is granted to anyone to use this software for any purpose,    *
* including commercial applications, and to alter it and redistribute it   *
* freely, subject to the following restrictions:                           *
*                                                                          *
* 1. The origin of this software must not be misrepresented; you must not  *
*    claim that you wrote the original software. If you use this software  *
*    in a product, an acknowledgment in the product documentation would be *
*    appreciated but is not required.                                      *
*                                                                          *
* 2. Altered source versions must be plainly marked as such, and must not  *
*    be misrepresented as being the original software.                     *
*                                                                          *
* 3. This notice may not be removed or altered from any source             *
*    distribution.                                                         *
\**************************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.Build.Tasks;
using Neumont.Tools.Xml;

namespace Neumont.Tools.Build.Tasks
{
	/// <summary>
	/// Preprocesses certain file types after copying them.
	/// </summary>
	public class CopyWithPreprocessing : Copy
	{
		/// <summary>See <see cref="Copy.Execute"/>.</summary>
		public override bool Execute()
		{
			if (!base.SkipUnchangedFiles)
			{
				throw new InvalidOperationException();
			}
			if (this.DisableAllPreprocessing)
			{
				base.Log.LogMessage(MessageImportance.Normal, "Skipped all preprocessing because $(DisableAllPreprocessing) is 'true'", null);
				return base.Execute();
			}
			
			ITaskItem[] originalSourceFiles = base.SourceFiles;
			ITaskItem[] originalDestinationFiles = base.DestinationFiles;
			string[] xmlFileExtensionsToPreprocess = this._xmlFileExtensionsToPreprocess;

			if (xmlFileExtensionsToPreprocess == null || xmlFileExtensionsToPreprocess.Length <= 0 || originalDestinationFiles == null)
			{
				return base.Execute();
			}

			List<ITaskItem> newSourceFiles = new List<ITaskItem>(originalSourceFiles);
			List<ITaskItem> newDestinationFiles = new List<ITaskItem>(originalDestinationFiles);
			List<ITaskItem> filesToPreprocess = new List<ITaskItem>();

			Array.Sort<string>(xmlFileExtensionsToPreprocess, StringComparer.OrdinalIgnoreCase);

			for (int i = newSourceFiles.Count - 1; i >= 0; i--)
			{
				ITaskItem sourceItem = newSourceFiles[i];
				ITaskItem destinationItem = newDestinationFiles[i];
				bool disablePreprocessing;
				if (bool.TryParse(sourceItem.GetMetadata("DisablePreprocessing"), out disablePreprocessing) && disablePreprocessing)
				{
					base.Log.LogMessage(MessageImportance.Normal, "Skipped preprocessing for \"{0}\" because %(DisablePreprocessing) is 'true'", sourceItem.ItemSpec);
					continue;
				}
				if (Array.BinarySearch<string>(xmlFileExtensionsToPreprocess, sourceItem.GetMetadata("Extension")) < 0)
				{
					continue;
				}

				FileInfo destinationFile = new FileInfo(destinationItem.GetMetadata("FullPath"));
				if (!destinationFile.Exists)
				{
					filesToPreprocess.Add(destinationItem);
					continue;
				}
				FileInfo sourceFile = new FileInfo(sourceItem.GetMetadata("FullPath"));
				if (sourceFile.LastWriteTimeUtc <= destinationFile.LastWriteTimeUtc)
				{
					// Remove it from the list so the regular copy doesn't see it
					if (sourceFile.Length != destinationFile.Length)
					{
						base.Log.LogMessage("Skipping preprocessing for \"{0}\" because the last modified time of the source is less than or equal to that of the destination. " +
							"Hiding from regular Copy task so that it doesn't see the file size difference.", sourceItem.ItemSpec);
						newSourceFiles.RemoveAt(i);
						newDestinationFiles.RemoveAt(i);
					}
				}
				else
				{
					filesToPreprocess.Add(destinationItem);
				}
			}

			base.SourceFiles = newSourceFiles.ToArray();
			base.DestinationFiles = newDestinationFiles.ToArray();

			bool retVal = base.Execute();

			foreach (ITaskItem taskItem in filesToPreprocess)
			{
				PreprocessXmlFile(taskItem);
			}

			base.SourceFiles = originalSourceFiles;
			base.DestinationFiles = originalDestinationFiles;

			// We can't actually change CopiedFiles, but we can at least remove everything from it so that any problems are more apparent.
			ITaskItem[] copiedFiles = base.CopiedFiles;
			Array.Clear(copiedFiles, 0, copiedFiles.Length);

			return retVal;
		}

		#region Xml Settings
		private static readonly XmlReaderSettings XmlReaderSettingsForSchema = InitializeXmlReaderSettingsForSchema();
		private static readonly XmlReaderSettings XmlReaderSettingsForDTD = InitializeXmlReaderSettingsForDTD();
		private static XmlReaderSettings InitializeXmlReaderSettingsForSchema()
		{
			XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
			xmlReaderSettings.CheckCharacters = false;
			// CloseInput is true so that the Schema-validating reader closes the DTD-validating reader
			xmlReaderSettings.CloseInput = true;
			xmlReaderSettings.ConformanceLevel = ConformanceLevel.Auto;
			xmlReaderSettings.IgnoreComments = true;
			xmlReaderSettings.IgnoreWhitespace = true;
			xmlReaderSettings.NameTable = XmlSchemaCatalog.XmlNameTable;
			xmlReaderSettings.Schemas = XmlSchemaCatalog.XmlSchemaSet;
			xmlReaderSettings.XmlResolver = XmlSchemaCatalog.XmlResolver;
			xmlReaderSettings.ProhibitDtd = false;
			xmlReaderSettings.ValidationFlags =
				XmlSchemaValidationFlags.AllowXmlAttributes |
				XmlSchemaValidationFlags.ProcessIdentityConstraints |
				XmlSchemaValidationFlags.ProcessInlineSchema |
				XmlSchemaValidationFlags.ProcessSchemaLocation;
			xmlReaderSettings.ValidationType = ValidationType.Schema;
			return xmlReaderSettings;
		}
		private static XmlReaderSettings InitializeXmlReaderSettingsForDTD()
		{
			XmlReaderSettings xmlReaderSettings = XmlReaderSettingsForSchema.Clone();
			// CloseInput is false so that the DTD-validating reader does not close the underlying stream
			xmlReaderSettings.CloseInput = false;
			xmlReaderSettings.ValidationType = ValidationType.DTD;
			return xmlReaderSettings;
		}
		private static XmlReader CreateDualValidatingReader(Stream stream)
		{
			// Wrap the DTD-validating reader with an XSD-validating reader.
			return XmlReader.Create(XmlReader.Create(stream, XmlReaderSettingsForDTD), XmlReaderSettingsForSchema);
		}
		private static readonly UTF8Encoding UTF8EncodingNoBOM = new UTF8Encoding(false, true);
		private static readonly XmlWriterSettings XmlWriterSettings = InitializeXmlWriterSettings();
		private static XmlWriterSettings InitializeXmlWriterSettings()
		{
			XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
			xmlWriterSettings.CheckCharacters = false;
			xmlWriterSettings.CloseOutput = false;
			xmlWriterSettings.ConformanceLevel = ConformanceLevel.Auto;
			xmlWriterSettings.Encoding = UTF8EncodingNoBOM;
			xmlWriterSettings.Indent = false;
			xmlWriterSettings.IndentChars = string.Empty;
			xmlWriterSettings.NewLineChars = string.Empty;
			xmlWriterSettings.NewLineHandling = NewLineHandling.Replace;
			xmlWriterSettings.NewLineOnAttributes = false;
			xmlWriterSettings.OmitXmlDeclaration = true;
			return xmlWriterSettings;
		}
		#endregion // Xml Settings

		#region PreprocessXmlFile method
		private void PreprocessXmlFile(ITaskItem taskItem)
		{
			const System.Security.AccessControl.FileSystemRights ReadExecuteAndWrite =
				System.Security.AccessControl.FileSystemRights.ReadAndExecute |
				System.Security.AccessControl.FileSystemRights.Write;

			XmlSchemaCatalog.WaitForLoadingToFinish(true);

			try
			{
				FileInfo fileInfo = new FileInfo(taskItem.GetMetadata("FullPath"));
				long startLength = fileInfo.Length;
				long finalLength;

				using (FileStream fileStream = new FileStream(fileInfo.FullName, FileMode.Open, ReadExecuteAndWrite, FileShare.None, (int)startLength, FileOptions.SequentialScan))
				{
					using (MemoryStream bufferStream = new MemoryStream((int)startLength))
					{
						using (XmlWriter xmlWriter = XmlWriter.Create(bufferStream, XmlWriterSettings))
						{	
							using (XmlReader xmlReader = CreateDualValidatingReader(fileStream))
							{
								// UNDONE: For some reason, default attribute values still aren't being stripped out correctly
								if (string.Equals(taskItem.GetMetadata("Extension"), ".XSD", StringComparison.OrdinalIgnoreCase))
								{
									XmlDocument xmlDocument = new XmlDocument(xmlReader.NameTable);
									xmlDocument.Load(xmlReader);
									XPathNavigator xPathNavigator = xmlDocument.CreateNavigator();
									XPathNodeIterator xPathNodeIterator = xPathNavigator.SelectDescendants("annotation", XmlSchema.Namespace, false);
									while (xPathNodeIterator.MoveNext())
									{
										xPathNodeIterator.Current.DeleteSelf();
									}
									xmlWriter.WriteNode(xPathNavigator, false);
								}
								else
								{
									xmlWriter.WriteNode(xmlReader, false);
								}
							}
						}
						byte[] buffer = bufferStream.GetBuffer();
						int bufferLength = (int)bufferStream.Length;
						List<int> stripSpaceIndexes = new List<int>(512);
						for (int i = bufferLength - 1; i >= 2; i--)
						{
							if (buffer[i] == '>' && buffer[--i] == '/' && buffer[--i] == ' ')
							{
								stripSpaceIndexes.Add(i);
							}
						}

						fileStream.Seek(0, SeekOrigin.Begin);

						int stripSpaceCount = stripSpaceIndexes.Count;
						finalLength = bufferLength - stripSpaceCount;
						int bufferIndex = 0;
						for (int i = stripSpaceCount - 1; i >= 0; i--)
						{
							fileStream.Write(buffer, bufferIndex, -bufferIndex + (bufferIndex = stripSpaceIndexes[i] + 1) - 1);
						}
						fileStream.Write(buffer, bufferIndex, bufferLength - bufferIndex);
					}
					fileStream.SetLength(finalLength);
				}
				Log.LogMessage(MessageImportance.Normal, "Successfully preprocessed \"{0}\". Original size was {1:n0} bytes, new size is {2:n0} bytes.", taskItem.ItemSpec, startLength, finalLength);
				
				// Update the destination file time to match the source file time so that we know not to process it again next time
				fileInfo.LastWriteTimeUtc = File.GetLastWriteTimeUtc(base.SourceFiles[Array.IndexOf<ITaskItem>(base.DestinationFiles, taskItem)].GetMetadata("FullPath"));
			}
			catch (Exception ex)
			{
				Log.LogErrorFromException(ex, true);
			}
		}
		#endregion // PreprocessXmlFile method

		#region Task properties

		private bool _disableAllPreprocessing;
		/// <summary>
		/// If <see langword="true"/>, no preprocessing is performed.
		/// </summary>
		public bool DisableAllPreprocessing
		{
			get
			{
				return this._disableAllPreprocessing;
			}
			set
			{
				this._disableAllPreprocessing = value;
			}
		}

		private string[] _xmlFileExtensionsToPreprocess;
		/// <summary>
		/// The extensions of the file types which should be preprocessed as XML files.
		/// </summary>
		[Required]
		public string[] XmlFileExtensionsToPreprocess
		{
			get
			{
				return this._xmlFileExtensionsToPreprocess;
			}
			set
			{
				this._xmlFileExtensionsToPreprocess = value;
			}
		}
		#endregion // Task properties
	}
}
