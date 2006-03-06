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
			private static readonly XmlResolver XmlResolver = new XmlUrlResolver();
			private static readonly XmlReaderSettings XmlReaderSettings = new XmlReaderSettings();

			private delegate void SetXmlWriterSettingsBoolean(XmlWriterSettings @this, bool value);
			private static readonly SetXmlWriterSettingsBoolean SetXmlWriterSettingsReadOnly;

			static XslORMGenerator()
			{
				XmlReaderSettings.CloseInput = false;
				XmlReaderSettings.IgnoreComments = true;
				XmlReaderSettings.IgnoreWhitespace = true;
				XmlReaderSettings.XmlResolver = XmlResolver;

				try
				{
					Type xmlWriterSettingsType = typeof(XmlWriterSettings);
					DynamicMethod dynamicMethod = new DynamicMethod("SetXmlWriterSettingsReadOnly", null, new Type[] { xmlWriterSettingsType, typeof(bool) }, xmlWriterSettingsType, true);
					ILGenerator ilGenerator = dynamicMethod.GetILGenerator(8);
					ilGenerator.Emit(OpCodes.Ldarg_0);
					ilGenerator.Emit(OpCodes.Ldarg_1);
					ilGenerator.Emit(OpCodes.Stfld, xmlWriterSettingsType.GetField("isReadOnly", BindingFlags.NonPublic | BindingFlags.Instance));
					ilGenerator.Emit(OpCodes.Ret);
					SetXmlWriterSettingsReadOnly = (SetXmlWriterSettingsBoolean)dynamicMethod.CreateDelegate(typeof(SetXmlWriterSettingsBoolean));
				}
				catch (Exception ex)
				{
					// TODO: Localize message.
					ORMCustomTool.ReportError("WARNING: Could not create XslORMGenerator.SetXmlWriterSettingsReadOnly method:", ex);
				}
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
				string sourceInputFormat = this._sourceInputFormat = generatorKey.GetValue("SourceInputFormat", null) as string;
				Debug.Assert(sourceInputFormat != null);
				this._requiresInputFormats = new ReadOnlyCollection<string>(new string[] { sourceInputFormat });
				this._transform = new XslCompiledTransform(false);

				Uri transformUri = XmlResolver.ResolveUri(null, generatorKey.GetValue("TransformUri", null) as string);
				this._transformCanonicalUri = transformUri.ToString();
				if (transformUri.IsFile || transformUri.IsUnc)
				{
					this._transformLocalPath = transformUri.LocalPath;
				}
			}

			private void LoadTransform()
			{
				this._transform.Load(this._transformCanonicalUri, XsltSettings.TrustedXslt, XmlResolver);
				this._transformLoadedTime = DateTime.UtcNow;
				if (SetXmlWriterSettingsReadOnly != null)
				{
					try
					{
						XmlWriterSettings outputSettings = this._transform.OutputSettings;
						SetXmlWriterSettingsReadOnly(outputSettings, false);
						outputSettings.CloseOutput = false;
						outputSettings.IndentChars = "\t";
						SetXmlWriterSettingsReadOnly(outputSettings, true);
					}
					catch (Exception ex)
					{
						// TODO: Localize message.
						ORMCustomTool.ReportError("WARNING: Exception ocurred while trying to change XslCompiledTransform.OutputSettings in XslORMGenerator:", ex);
					}
				}
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

			private readonly string _sourceInputFormat;
			private readonly ReadOnlyCollection<string> _requiresInputFormats;
			public System.Collections.ObjectModel.ReadOnlyCollection<string> RequiresInputFormats
			{
				get { return this._requiresInputFormats; }
			}

			private readonly string _fileExtension;
			public string GetOutputFileDefaultName(string sourceFileName)
			{
				return Path.ChangeExtension(sourceFileName, this._fileExtension);
			}

			public BuildItem AddGeneratedFileBuildItem(BuildItemGroup buildItemGroup, string sourceFileName, string outputFileName)
			{
				BuildItem buildItem = buildItemGroup.AddNewItem("None", outputFileName);
				buildItem.SetMetadata(ITEMMETADATA_AUTOGEN, "True");
				buildItem.SetMetadata(ITEMMETADATA_DEPENDENTUPON, sourceFileName);
				buildItem.SetMetadata(ITEMMETADATA_ORMGENERATOR, this.OfficialName);
				return buildItem;
			}

			public void GenerateOutput(BuildItem buildItem, Stream outputStream, IDictionary<string, Stream> inputFormatStreams)
			{
				this.EnsureTransform();
				Stream inputStream = inputFormatStreams[this._sourceInputFormat];
				XmlReader inputReader = null;
				try
				{
					inputReader = XmlReader.Create(inputStream, XmlReaderSettings);
					this._transform.Transform(inputReader, null, outputStream);
				}
				finally
				{
					if (inputReader != null)
					{
						inputReader.Close();
					}
					inputStream.Seek(0, SeekOrigin.Begin);
				}
			}
		}
	}
}
