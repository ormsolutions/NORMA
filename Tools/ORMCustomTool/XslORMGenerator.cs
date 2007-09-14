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
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using Microsoft.Build.BuildEngine;
using Microsoft.Win32;

using Debug = System.Diagnostics.Debug;

namespace Neumont.Tools.ORM.ORMCustomTool
{
	public sealed partial class ORMCustomTool
	{
		private sealed class XslORMGenerator : IORMGenerator
		{
			private static readonly string[] EmptyStringArray = new string[0];
			private static readonly XmlResolver XmlResolver = new XmlUrlResolver();
			private static readonly XmlReaderSettings XmlReaderSettings = new XmlReaderSettings();
			private static readonly XPathExpression DocumentElementXPathExpression = XPathExpression.Compile("/child::*");

			static XslORMGenerator()
			{
				XmlReaderSettings.CloseInput = false;
				XmlReaderSettings.IgnoreComments = true;
				XmlReaderSettings.IgnoreWhitespace = true;
				XmlReaderSettings.XmlResolver = XslORMGenerator.XmlResolver;
			}

			public XslORMGenerator(RegistryKey generatorKey)
			{
				// TODO: We need a better way to localize DisplayName and DisplayDescription for XSLT stylesheets...
				this._displayDescription = generatorKey.GetValue("DisplayDescription", null) as string;
				Debug.Assert(this._displayDescription != null);
				this._displayName = generatorKey.GetValue("DisplayName", null) as string;
				Debug.Assert(this._displayName != null);
				this._fileExtension = generatorKey.GetValue("FileExtension", null) as string;
				Debug.Assert(this._fileExtension != null);
				this._officialName = generatorKey.GetValue("OfficialName", null) as string;
				Debug.Assert(this._officialName != null);
				this._providesOutputFormat = generatorKey.GetValue("ProvidesOutputFormat", null) as string;
				Debug.Assert(this._providesOutputFormat != null);
				this._compilable = Convert.ToBoolean((int)generatorKey.GetValue("Compilable", 0));
				this._generatesSupportFile = Convert.ToBoolean((int)generatorKey.GetValue("GeneratesSupportFile", 0));
				this._customTool = generatorKey.GetValue("CustomTool", null) as string;

				Dictionary<string, IEnumerable<string>> extensions = null;
				string sourceInputFormat = generatorKey.GetValue("SourceInputFormat", null) as string;
				Debug.Assert(sourceInputFormat != null);
				string[] referenceInputFormats = generatorKey.GetValue("ReferenceInputFormats", EmptyStringArray) as string[];
				string[] prerequisiteInputFormats = generatorKey.GetValue("PrerequisiteInputFormats", EmptyStringArray) as string[];

				List<string> requiresInputFormats;

				requiresInputFormats = new List<string>(referenceInputFormats.Length + prerequisiteInputFormats.Length + 1);
				requiresInputFormats.Add(sourceInputFormat = StripFormatExtensions(sourceInputFormat, ref extensions));
				for (int i = 0; i < referenceInputFormats.Length; ++i)
				{
					requiresInputFormats.Add(referenceInputFormats[i] = StripFormatExtensions(referenceInputFormats[i], ref extensions));
				}
				for (int i = 0; i < prerequisiteInputFormats.Length; ++i)
				{
					requiresInputFormats.Add(StripFormatExtensions(prerequisiteInputFormats[i], ref extensions));
				}
				this._sourceInputFormat = sourceInputFormat;
				this._referenceInputFormats = referenceInputFormats;
				this._requiredExtensions = extensions;
				this._requiresInputFormats = new ReadOnlyCollection<string>(requiresInputFormats);

				this._transform = new XslCompiledTransform(System.Diagnostics.Debugger.IsAttached);

				Uri transformUri = XmlResolver.ResolveUri(null, generatorKey.GetValue("TransformUri", null) as string);
				this._transformCanonicalUri = transformUri.ToString();
				if (transformUri.IsFile || transformUri.IsUnc)
				{
					this._transformLocalPath = transformUri.LocalPath;
				}
			}

			private static readonly char[] _extensionSplitChars = new char[] { ' ' };
			private static string StripFormatExtensions(string format, ref Dictionary<string, IEnumerable<string>> extensions)
			{
				string[] substrings = format.Split(_extensionSplitChars, StringSplitOptions.RemoveEmptyEntries);
				int substringCount = substrings.Length;
				if (substringCount > 0)
				{
					format = substrings[0];
					if (substringCount > 1)
					{
						string[] oldExtensions = null;
						if (extensions == null)
						{
							extensions = new Dictionary<string, IEnumerable<string>>();
							oldExtensions = null;
						}
						else
						{
							IEnumerable<string> oldEntry;
							if (extensions.TryGetValue(format, out oldEntry))
							{
								oldExtensions = (string[])oldEntry;
							}
						}
						int oldLength = (oldExtensions != null) ? oldExtensions.Length : 0;
						string[] newExtensions = new string[oldLength + substringCount - 1];
						if (oldLength != 0)
						{
							oldExtensions.CopyTo(newExtensions, 0);
						}
						for (int i = 1; i < substringCount; ++i, ++oldLength)
						{
							newExtensions[oldLength] = substrings[i];
						}
						extensions[format] = newExtensions;
					}
				}
				return format;
			}

			private void LoadTransform()
			{
				this._transform.Load(this._transformCanonicalUri, XsltSettings.TrustedXslt, XmlResolver);
				this._transformLoadedTime = DateTime.UtcNow;
				XmlWriterSettings outputSettings = this._xmlWriterSettings = this._transform.OutputSettings.Clone();
				outputSettings.CloseOutput = false;
				outputSettings.IndentChars = "\t";
			}
			private void EnsureTransform()
			{
				// Check if OutputSettings is null in order to see if stylesheet has been loaded yet.
				if (this._transform.OutputSettings == null)
				{
					this.LoadTransform();
				}
				else if (this._transformLocalPath != null)
				{
					// If the stylesheet is reachable via the file system, check to see if it has been modified,
					// and reload it if it has.
					FileInfo transformFileInfo = new FileInfo(this._transformLocalPath);
					Debug.Assert(transformFileInfo.Exists);
					DateTime lastWriteTimeUtc = transformFileInfo.LastWriteTimeUtc;

					if (lastWriteTimeUtc > this._transformLoadedTime)
					{
						this.LoadTransform();
					}
				}
			}

			private readonly string _transformCanonicalUri;
			private readonly string _transformLocalPath;
			private readonly XslCompiledTransform _transform;
			private readonly string _customTool;
			private XmlWriterSettings _xmlWriterSettings;
			private DateTime _transformLoadedTime;

			private readonly string _officialName;
			public string OfficialName
			{
				get { return this._officialName; }
			}

			private readonly string _displayName;
			public string DisplayName
			{
				get { return this._displayName; }
			}

			private readonly string _displayDescription;
			public string DisplayDescription
			{
				get { return this._displayDescription; }
			}

			private readonly string _providesOutputFormat;
			public string ProvidesOutputFormat
			{
				get { return this._providesOutputFormat; }
			}

			private readonly IDictionary<string, IEnumerable<string>> _requiredExtensions;
			public IEnumerable<string> GetRequiredExtensionsForInputFormat(string outputFormat)
			{
				IDictionary<string, IEnumerable<string>> dictionary = _requiredExtensions;
				IEnumerable<string> retVal;
				return (dictionary != null && dictionary.TryGetValue(outputFormat, out retVal)) ?
					retVal :
					EmptyStringArray;
			}

			private readonly bool _generatesSupportFile;
			public bool GeneratesSupportFile
			{
				get { return this._generatesSupportFile; }
			}

			private readonly string _sourceInputFormat;
			private readonly string[] _referenceInputFormats;
			private readonly IList<string> _requiresInputFormats;
			public IList<string> RequiresInputFormats
			{
				get { return this._requiresInputFormats; }
			}

			private readonly string _fileExtension;
			public string GetOutputFileDefaultName(string sourceFileName)
			{
				return Path.ChangeExtension(sourceFileName, this._fileExtension);
			}

			private readonly bool _compilable;
			public BuildItem AddGeneratedFileBuildItem(BuildItemGroup buildItemGroup, string sourceFileName, string outputFileName)
			{
				if (outputFileName == null || outputFileName.Length == 0)
				{
					outputFileName = GetOutputFileDefaultName(sourceFileName);
				}
				bool compilable = this._compilable;
				BuildItem buildItem = buildItemGroup.AddNewItem(compilable ? "Compile" : "None", outputFileName);
				buildItem.SetMetadata(ITEMMETADATA_AUTOGEN, "True");
				if (compilable)
				{
					buildItem.SetMetadata(ITEMMETADATA_DESIGNTIME, "True");
				}
				string customTool = this._customTool;
				if (!string.IsNullOrEmpty(customTool))
				{
					buildItem.SetMetadata(ITEMMETADATA_GENERATOR, customTool);
				}
				buildItem.SetMetadata(ITEMMETADATA_DEPENDENTUPON, sourceFileName);
				buildItem.SetMetadata(ITEMMETADATA_ORMGENERATOR, this.OfficialName);
				return buildItem;
			}

			public void GenerateOutput(BuildItem buildItem, Stream outputStream, IDictionary<string, Stream> inputFormatStreams, string defaultNamespace)
			{
				this.EnsureTransform();
				Stream inputStream = inputFormatStreams[this._sourceInputFormat];
				
				XsltArgumentList argumentList = new XsltArgumentList();
				argumentList.AddParam("DefaultNamespace", string.Empty, defaultNamespace);

				string[] referenceFormats = this._referenceInputFormats;
				if (referenceFormats != null)
				{
					for (int i = 0; i < referenceFormats.Length; i++)
					{
						string referenceFormat = referenceFormats[i];
						Stream referenceStream = inputFormatStreams[referenceFormat];
						using (XmlReader referenceReader = XmlReader.Create(referenceStream))
						{
							argumentList.AddParam(referenceFormat, string.Empty, new XPathDocument(referenceReader).CreateNavigator().Select(DocumentElementXPathExpression));
						}
						referenceStream.Seek(0, SeekOrigin.Begin);
					}
				}

				XmlReader inputReader = null;
				XmlWriter outputWriter = null;
				try
				{
					inputReader = XmlReader.Create(inputStream, XmlReaderSettings);
					outputWriter = XmlWriter.Create(outputStream, this._xmlWriterSettings);
					this._transform.Transform(inputReader, argumentList, outputWriter);
				}
				finally
				{
					if (inputReader != null)
					{
						inputReader.Close();
					}
					if (outputWriter != null)
					{
						outputWriter.Close();
					}
					inputStream.Seek(0, SeekOrigin.Begin);
				}
			}
		}
	}
}
