using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.Build.Tasks;
//using Neumont.Tools.Xml;

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
						base.Log.LogMessage("Skipping preprocessing for \"{0}\" because source is older than destination. " +
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
			xmlReaderSettings.CloseInput = true;
			xmlReaderSettings.ConformanceLevel = ConformanceLevel.Auto;
			xmlReaderSettings.IgnoreComments = true;
			xmlReaderSettings.IgnoreWhitespace = true;
			//xmlReaderSettings.NameTable = XmlSchemaCatalog.XmlNameTable;
			xmlReaderSettings.NameTable = new NameTable();
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
			xmlReaderSettings.CloseInput = false;
			xmlReaderSettings.ValidationType = ValidationType.DTD;
			return xmlReaderSettings;
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

		private void PreprocessXmlFile(ITaskItem taskItem)
		{
			const System.Security.AccessControl.FileSystemRights ReadExecuteAndWrite =
				System.Security.AccessControl.FileSystemRights.ReadAndExecute |
				System.Security.AccessControl.FileSystemRights.Write;

			// UNDONE: For XSD, also strip xs:annotation
			bool isXsd = string.Equals(taskItem.GetMetadata("Extension"), ".XSD", StringComparison.OrdinalIgnoreCase);

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
							// Wrap the DTD-validating reader with an XSD-validating reader. 
							using (XmlReader xmlReader = XmlReader.Create(XmlReader.Create(fileStream, XmlReaderSettingsForDTD), XmlReaderSettingsForSchema))
							{
								if (xmlReader.NodeType == XmlNodeType.XmlDeclaration)
								{
									xmlReader.Read();
								}
								xmlWriter.WriteNode(xmlReader, false);
							}
						}
						fileStream.Seek(0, SeekOrigin.Begin);
						fileStream.Write(bufferStream.GetBuffer(), 0, (int)(finalLength = bufferStream.Length));
					}
					fileStream.SetLength(finalLength);
				}
				Log.LogMessage(MessageImportance.Normal, "Successfully preprocessed \"{0}\". Original size was {1:n} bytes, new size is {2:n} bytes.", taskItem.ItemSpec, startLength, finalLength);
				
				// Update the source file time to match the destination file time so that we know not to process it again next time
				fileInfo.Refresh();
				File.SetLastWriteTimeUtc(base.SourceFiles[Array.IndexOf<ITaskItem>(base.DestinationFiles, taskItem)].GetMetadata("FullPath"), fileInfo.LastWriteTimeUtc);
			}
			catch (Exception ex)
			{
				Log.LogErrorFromException(ex, true);
			}
		}

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
	}
}
