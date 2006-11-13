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
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.Build.Tasks;
using Neumont.Xml;

namespace Neumont.Build.Tasks
{
	/// <summary>
	/// Compiles one or more regular expressions into a stand-alone <see cref="Assembly"/>.
	/// </summary>
	public class RegexCompiler : Task
	{
		#region LoadRegexCompilationInfo method and support
		private static readonly XmlReaderSettings XmlReaderSettings = InitializeXmlReaderSettings();
		private static XmlReaderSettings InitializeXmlReaderSettings()
		{
			XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
			xmlReaderSettings.CheckCharacters = true;
			xmlReaderSettings.CloseInput = true;
			xmlReaderSettings.ConformanceLevel = ConformanceLevel.Auto;
			xmlReaderSettings.IgnoreComments = true;
			xmlReaderSettings.IgnoreWhitespace = true;
			xmlReaderSettings.NameTable = XmlSchemaCatalog.XmlNameTable;
			xmlReaderSettings.Schemas = XmlSchemaCatalog.XmlSchemaSet;
			xmlReaderSettings.XmlResolver = XmlSchemaCatalog.XmlResolver;
			xmlReaderSettings.ProhibitDtd = false;
			xmlReaderSettings.ValidationType = ValidationType.Schema;
			return xmlReaderSettings;
		}

		private static RegexCompilationInfo LoadRegexCompilationInfo(TaskLoggingHelper log, string regexFilePath)
		{
			log.LogMessage("Loading RegexCompilationInfo from \"{0}\".", regexFilePath);
			bool isPublic = false;
			string name = null;
			string @namespace = null;
			string pattern = null;
			RegexOptions regexOptions = RegexOptions.Compiled;

			FileInfo fileInfo = new FileInfo(regexFilePath);
			using (FileStream fileStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete, (int)fileInfo.Length, FileOptions.SequentialScan))
			{
				using (XmlReader xmlReader = XmlReader.Create(fileStream, XmlReaderSettings))
				{
					while (xmlReader.MoveToNextAttribute() || xmlReader.Read())
					{
						switch (xmlReader.NodeType)
						{
							case XmlNodeType.Element:
							case XmlNodeType.Attribute:
								switch (xmlReader.LocalName)
								{
									case "IsPublic":
										isPublic = xmlReader.ReadContentAsBoolean();
										break;
									case "Name":
										name = xmlReader.ReadContentAsString();
										break;
									case "Namespace":
										@namespace = xmlReader.ReadContentAsString();
										break;
									case "CultureInvariant":
										regexOptions |= RegexOptions.CultureInvariant;
										break;
									case "ECMAScript":
										regexOptions |= RegexOptions.ECMAScript;
										break;
									case "ExplicitCapture":
										regexOptions |= RegexOptions.ExplicitCapture;
										break;
									case "IgnoreCase":
										regexOptions |= RegexOptions.IgnoreCase;
										break;
									case "IgnorePatternWhitespace":
										regexOptions |= RegexOptions.IgnorePatternWhitespace;
										break;
									case "Multiline":
										regexOptions |= RegexOptions.Multiline;
										break;
									case "RightToLeft":
										regexOptions |= RegexOptions.RightToLeft;
										break;
									case "Singleline":
										regexOptions |= RegexOptions.Singleline;
										break;
									case "Pattern":
										pattern = xmlReader.ReadElementContentAsString();
										break;
								}
								break;
						}
					}
				}
			}

			RegexCompilationInfo regexCompilationInfo = new RegexCompilationInfo(pattern, regexOptions, name, @namespace, isPublic);
			log.LogMessage("RegexCompilationInfo loaded with settings:", null);
			log.LogMessage("\tNamespace:\t\t{0}", regexCompilationInfo.Namespace);
			log.LogMessage("\tName:\t\t\t{0}", regexCompilationInfo.Name);
			log.LogMessage("\tIsPublic:\t\t{0}", regexCompilationInfo.IsPublic);
			log.LogMessage("\tRegexOptions:\t{0}", regexCompilationInfo.Options);
			return regexCompilationInfo;
		}
		#endregion // LoadRegexCompilationInfo method and support

		#region RegexCompilationProxyWrapper and RegexCompilationProxy classes
		private sealed class RegexCompilationProxyWrapper : IDisposable
		{
			private AppDomain _separateAppDomain;
			private RegexCompilationProxy _regexCompilationProxy;
			public RegexCompilationProxyWrapper(bool alwaysCompileRegexInSeparateAppDomain)
			{
				if (alwaysCompileRegexInSeparateAppDomain)
				{
					AppDomain separateAppDomain = this._separateAppDomain = AppDomain.CreateDomain("RegexCompiler Isolation AppDomain", null, AppDomain.CurrentDomain.SetupInformation);
					Assembly assembly = Assembly.GetExecutingAssembly();
					if (!assembly.GlobalAssemblyCache)
					{
						separateAppDomain.AssemblyResolve += delegate(object sender, ResolveEventArgs e)
						{
							Assembly executingAssembly = Assembly.GetExecutingAssembly();
							return (e.Name == executingAssembly.FullName) ? executingAssembly : null;
						};
					}
					this._regexCompilationProxy = (RegexCompilationProxy)separateAppDomain.CreateInstanceAndUnwrap(assembly.FullName, typeof(RegexCompilationProxy).FullName);
				}
				else
				{
					this._regexCompilationProxy = new RegexCompilationProxy();
				}
			}

			public void Dispose()
			{
				AppDomain separateAppDomain = this._separateAppDomain;
				if (separateAppDomain != null)
				{
					AppDomain.Unload(separateAppDomain);
					this._separateAppDomain = null;
				}
				this._regexCompilationProxy = null;
			}

			public void CompileToAssembly(RegexCompilationInfo[] regexCompilationInfos, AssemblyName assemblyName)
			{
				this._regexCompilationProxy.CompileToAssembly(regexCompilationInfos, assemblyName);
			}

			private sealed class RegexCompilationProxy : MarshalByRefObject
			{
				// Although this method does not access any instance data, DO NOT MARK IT STATIC!
				// It needs to remain an instance method so that the call to it is remoted into the separate AppDomain.
				public void CompileToAssembly(RegexCompilationInfo[] regexCompilationInfos, AssemblyName assemblyName)
				{
					string currentDirectory = Directory.GetCurrentDirectory();
					try
					{
						Directory.SetCurrentDirectory(Path.GetDirectoryName(assemblyName.CodeBase));
						Regex.CompileToAssembly(regexCompilationInfos, assemblyName);
					}
					finally
					{
						Directory.SetCurrentDirectory(currentDirectory);
					}
				}
			}
		}
		#endregion RegexCompilationProxyWrapper and RegexCompilationProxy classes

		#region Execute method
		/// <summary>See <see cref="Task.Execute"/>.</summary>
		public override bool Execute()
		{
			TaskLoggingHelper log = base.Log;
			ITaskItem[] regexCompilationInfoFiles = this.RegexCompilationInfoFiles;
			RegexCompilationInfo[] regexCompilationInfos = new RegexCompilationInfo[regexCompilationInfoFiles.Length];
			for (int i = 0; i < regexCompilationInfoFiles.Length; i++)
			{
				regexCompilationInfos[i] = RegexCompiler.LoadRegexCompilationInfo(log, regexCompilationInfoFiles[i].GetMetadata("FullPath"));
			}

			AssemblyName assemblyName = new AssemblyName(this.AssemblyName);
			string relativeAssemblyCodeBase = Path.Combine(this.IntermediateOutputPath, assemblyName.Name + ".dll");
			assemblyName.CodeBase = Path.GetFullPath(relativeAssemblyCodeBase);
			string strongNameKeySource;
			if (!string.IsNullOrEmpty(strongNameKeySource = this.KeyContainer))
			{
				assemblyName.KeyPair = new StrongNameKeyPair(strongNameKeySource);
			}
			else if (!string.IsNullOrEmpty(strongNameKeySource = this.KeyFile))
			{
				using (FileStream fileStream = new FileStream(strongNameKeySource, FileMode.Open, FileAccess.Read, FileShare.Read, 8192, FileOptions.SequentialScan))
				{
					assemblyName.KeyPair = new StrongNameKeyPair(fileStream);
				}
			}

			log.LogMessage("{0} regex assembly will be created with AssemblyName \"{1}\" and CodeBase \"{2}\".", assemblyName.KeyPair != null ? "Signed" : "Unsigned", assemblyName.FullName, relativeAssemblyCodeBase);

			using (RegexCompilationProxyWrapper regexCompilationProxyWrapper = new RegexCompilationProxyWrapper(this.AlwaysCompileRegexInSeparateDomain))
			{
				regexCompilationProxyWrapper.CompileToAssembly(regexCompilationInfos, assemblyName);
			}

			this._outputAssembly = new TaskItem(relativeAssemblyCodeBase);

			return !base.Log.HasLoggedErrors;
		}
		#endregion // Execute method

		#region Properties
		private bool _alwaysCompileRegexInSeparateDomain;
		/// <summary>
		/// Indicates whether regular expressions should always be compiled in a separate <see cref="AppDomain"/> that can be unloaded
		/// after compilation has completed.
		/// </summary>
		public bool AlwaysCompileRegexInSeparateDomain
		{
			get
			{
				return this._alwaysCompileRegexInSeparateDomain;
			}
			set
			{
				this._alwaysCompileRegexInSeparateDomain = value;
			}
		}


		private string _assemblyName;
		/// <summary>
		/// The name (in <see cref="AssemblyName"/> format) of the <see cref="Assembly"/> to which the regular expressions should be compiled.
		/// </summary>
		[Required]
		public string AssemblyName
		{
			get
			{
				return this._assemblyName;
			}
			set
			{
				this._assemblyName = value;
			}
		}

		private string _intermediateOutputPath;
		/// <summary>
		/// The path to which intermediate output files should be saved.
		/// </summary>
		[Required]
		public string IntermediateOutputPath
		{
			get
			{
				return this._intermediateOutputPath;
			}
			set
			{
				this._intermediateOutputPath = value;
			}
		}

		private ITaskItem[] _regexCompilationInfoFiles;
		/// <summary>
		/// The regular expression compilation information files containing the regular expressions to compile.
		/// </summary>
		[Required]
		public ITaskItem[] RegexCompilationInfoFiles
		{
			get
			{
				return this._regexCompilationInfoFiles;
			}
			set
			{
				this._regexCompilationInfoFiles = value;
			}
		}

		private ITaskItem _outputAssembly;
		/// <summary>
		/// The full path to the compiled <see cref="Assembly"/>.
		/// </summary>
		[Output]
		public ITaskItem OutputAssembly
		{
			get
			{
				return this._outputAssembly;
			}
		}

		private string _keyContainer;
		/// <summary>
		/// Gets or sets the name of the cryptographic key container.
		/// </summary>
		public string KeyContainer
		{
			get
			{
				return this._keyContainer;
			}
			set
			{
				this._keyContainer = value;
			}
		}

		private string _keyFile;
		/// <summary>
		/// Gets or sets the file name containing the cryptographic key.
		/// </summary>
		public string KeyFile
		{
			get
			{
				return this._keyFile;
			}
			set
			{
				this._keyFile = value;
			}
		}
		#endregion // Properties
	}
}
