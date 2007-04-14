#define XML_SCHEMA_CATALOG
#if XML_SCHEMA_CATALOG
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Security;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Schema;
using Microsoft.Win32;

namespace Neumont.Xml
{
	// UNDONE: Until this is moved to a separate Neumont.Xml assembly, it is internal.
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	internal static class XmlSchemaCatalog
	{
		internal static void WaitForLoadingToFinish(bool aboveNormalPriority)
		{
			Loader.WaitForLoadingToFinish(aboveNormalPriority);
		}

		public static readonly XmlNameTable XmlNameTable = new NameTable();
		public static readonly XmlResolver XmlResolver = new XmlUrlResolver();
		private static readonly XmlReaderSettings SchemaReaderSettings = InitializeSchemaReaderSettings();
		internal static readonly XmlSchemaSet XmlSchemaSet = InitializeXmlSchemaSet();
		private static XmlReaderSettings InitializeSchemaReaderSettings()
		{
			XmlReaderSettings schemaReaderSettings = new XmlReaderSettings();
			schemaReaderSettings.CloseInput = true;
			schemaReaderSettings.IgnoreComments = true;
			schemaReaderSettings.IgnoreWhitespace = true;
			schemaReaderSettings.NameTable = XmlSchemaCatalog.XmlNameTable;
			schemaReaderSettings.ProhibitDtd = false;
			schemaReaderSettings.XmlResolver = XmlSchemaCatalog.XmlResolver;
			return schemaReaderSettings;
		}
		private static XmlSchemaSet InitializeXmlSchemaSet()
		{
			XmlSchemaSet xmlSchemaSet = new XmlSchemaSet(XmlSchemaCatalog.XmlNameTable);
			xmlSchemaSet.XmlResolver = XmlSchemaCatalog.XmlResolver;
#if TRACE
			xmlSchemaSet.ValidationEventHandler += XmlSchemaSetValidationEventHandler;
#endif //TRACE
			XmlSchemaCatalog.SchemaReaderSettings.Schemas = xmlSchemaSet;

			Loader.Start();

			return xmlSchemaSet;
		}

		#region XmlSchemaSetValidationEventHandler method
#if TRACE
		private static void XmlSchemaSetValidationEventHandler(object sender, ValidationEventArgs e)
		{
			if (e.Severity == XmlSeverityType.Error)
			{
				Trace.TraceError(e.Message + Environment.NewLine + e.Exception);
			}
			else
			{
				Trace.TraceWarning(e.Message + Environment.NewLine + e.Exception);
			}
		}
#endif //TRACE
		#endregion // XmlSchemaSetValidationEventHandler method

		#region Loader class
		private static class Loader
		{
			private static bool PriorityBoosted;
			public static void WaitForLoadingToFinish(bool aboveNormalPriority)
			{
				ManualResetEvent loadingFinished = LoadingFinished;
				Thread loaderThread = LoaderThread;
				if (!loadingFinished.WaitOne(0, false))
				{
					try
					{
						PriorityBoosted = true;
						loaderThread.Priority = aboveNormalPriority ? ThreadPriority.AboveNormal : ThreadPriority.Normal;
						loadingFinished.WaitOne();
					}
					finally
					{
						PriorityBoosted = false;
						loaderThread.Priority = ThreadPriority.BelowNormal;
					}
				}
			}

			private static readonly object LockObject = new object();

			private static readonly ManualResetEvent LoadingFinished = new ManualResetEvent(false);
			private static readonly ManualResetEvent LoadingPending = new ManualResetEvent(true);

			private static string WritableCacheDirectory;

			private static readonly Queue<string> LoaderQueue = new Queue<string>(64);

			private static readonly Thread LoaderThread = new Thread(StartLoaderThread);

			public static void Start()
			{
				Thread loaderThread = LoaderThread;
				loaderThread.IsBackground = true;
				loaderThread.Name = "XmlSchemaCatalog Loader Thread";
				loaderThread.Start();
			}

			#region Environment variable setup
			private static void SetEnvironmentVariable(string name, string value)
			{
				if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
				{
					string currentValue = Environment.GetEnvironmentVariable(name);
					if (string.IsNullOrEmpty(currentValue))
					{
						Environment.SetEnvironmentVariable(name, value, EnvironmentVariableTarget.Process);
					}
				}
			}
			private static void SetEnvironmentVariable(string name, Environment.SpecialFolder specialFolder)
			{
				SetEnvironmentVariable(name, Environment.GetFolderPath(specialFolder));
			}
			private static void SetupEvironmentVariables()
			{
				// Several of these are for compatibility with the Visual Studio XML Editor

				SetEnvironmentVariable("System", Environment.SpecialFolder.System);
				SetEnvironmentVariable("ProgramFiles", Environment.SpecialFolder.ProgramFiles);
				SetEnvironmentVariable("Programs", Environment.SpecialFolder.Programs);
				SetEnvironmentVariable("CommonProgramFiles", Environment.SpecialFolder.CommonProgramFiles);
				SetEnvironmentVariable("ApplicationData", Environment.SpecialFolder.ApplicationData);
				SetEnvironmentVariable("CommonApplicationData", Environment.SpecialFolder.CommonApplicationData);
				SetEnvironmentVariable("MyDocs", Environment.SpecialFolder.Personal);

				SetEnvironmentVariable("LCID", CultureInfo.CurrentUICulture.LCID.ToString(NumberFormatInfo.InvariantInfo));

				using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\VisualStudio\8.0\Setup\VS", RegistryKeyPermissionCheck.ReadSubTree))
				{
					if (key != null)
					{
						string productDir = key.GetValue("ProductDir", null) as string;
						// VsInstallDir is used in the original release of Visual Studio 2005
						SetEnvironmentVariable("VsInstallDir", productDir);
						// InstallRoot is used in Visual Studio 2005 with Service Pack 1 and later
						SetEnvironmentVariable("InstallRoot", productDir);
						// However, some of the MSDN documentation incorrectly refers to the new variable used as InstallDir rather than InstallRoot.
						// We define this as well so that everything will still work for anyone who was misled by the documentation.
						SetEnvironmentVariable("InstallDir", productDir);
					}
				}
			}
			#endregion // Environment variable setup

			private static void StartLoaderThread()
			{
				const string CACHE_DIRECTORY_NAME = @"Xml\Schemas";

				SetupEvironmentVariables();

				DirectoryInfo commonSchemasDirectoryInfo = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles), CACHE_DIRECTORY_NAME));
				if (!commonSchemasDirectoryInfo.Exists)
				{
					try
					{
						commonSchemasDirectoryInfo.Create();
						WritableCacheDirectory = commonSchemasDirectoryInfo.FullName;
					}
					catch (SecurityException)
					{
						WritableCacheDirectory = null;
					}
				}
				else
				{
					try
					{
						File.Create(Path.Combine(commonSchemasDirectoryInfo.FullName, "__TestFile.delete"), 1, FileOptions.DeleteOnClose).Close();
						WritableCacheDirectory = commonSchemasDirectoryInfo.FullName;
					}
					catch (SecurityException)
					{
						WritableCacheDirectory = null;
					}
					EnqueueDirectory(commonSchemasDirectoryInfo);
				}

				DirectoryInfo userSchemasDirectoryInfo = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), CACHE_DIRECTORY_NAME));
				if (!userSchemasDirectoryInfo.Exists)
				{
					userSchemasDirectoryInfo.Create();
				}
				else
				{
					EnqueueDirectory(userSchemasDirectoryInfo);
				}

				if ((object)WritableCacheDirectory == null)
				{
					WritableCacheDirectory = userSchemasDirectoryInfo.FullName;
				}

				{
					string vsSchemaCacheLocation = null;
					using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\VisualStudio\8.0\XmlEditor", RegistryKeyPermissionCheck.ReadSubTree))
					{
						if (key != null)
						{
							vsSchemaCacheLocation = key.GetValue("SchemaCacheLocation", null) as string;
						}
					}
					if (!string.IsNullOrEmpty(vsSchemaCacheLocation))
					{
						DirectoryInfo schemaCacheDirectoryInfo = new DirectoryInfo(Environment.ExpandEnvironmentVariables(vsSchemaCacheLocation));
						if (schemaCacheDirectoryInfo.Exists)
						{
							EnqueueDirectory(schemaCacheDirectoryInfo);
						}
					}
				}

				Main();
			}

			private static void EnqueueDirectory(string path)
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(path);
				if (directoryInfo.Exists)
				{
					EnqueueDirectory(directoryInfo);
				}
			}
			private static void EnqueueDirectory(DirectoryInfo directoryInfo)
			{
				// UNDONE: Process other XML files, too (catalogs, schemas with other extensions, etc.)
				foreach (FileInfo fileInfo in directoryInfo.GetFiles("*.xsd"))
				{
					EnqueueLocalFile(fileInfo.FullName);
				}
			}
			private static void EnqueueFile(Uri uri)
			{
				if (!XmlSchemaCatalog.XmlSchemaSet.Contains(uri.ToString()))
				{
					string localPath;
					if ((uri.IsFile || uri.IsUnc) && File.Exists(localPath = uri.LocalPath))
					{
						EnqueueLocalFile(localPath);
					}
					else
					{
						throw new NotImplementedException();
						// Start the download, and have the completion callback enqueue it.
					}
				}
			}
			private static void EnqueueLocalFile(string localPath)
			{
				lock (LockObject)
				{
					Trace.TraceInformation("Enqueued local file \"{0}\"", localPath);
					LoaderQueue.Enqueue(localPath);
					LoadingFinished.Reset();
					LoadingPending.Set();
				}
			}

			private static void Main()
			{
				if (!PriorityBoosted)
				{
					LoaderThread.Priority = ThreadPriority.BelowNormal;
				}
				object lockObject = LockObject;
				Queue<string> loaderQueue = LoaderQueue;
				ManualResetEvent loadingFinished = LoadingFinished;
				ManualResetEvent loadingPending = LoadingPending;
				XmlSchemaSet xmlSchemaSet = XmlSchemaCatalog.XmlSchemaSet;

				while (true)
				{
					loadingPending.WaitOne();

					while (loaderQueue.Count > 0)
					{
						string currentPath;
						lock (lockObject)
						{
							currentPath = loaderQueue.Dequeue();
						}
						ProcessFile(currentPath);
					}
					lock (lockObject)
					{
						if (loaderQueue.Count > 0)
						{
							continue;
						}
						xmlSchemaSet.Compile();
						loadingPending.Reset();
						loadingFinished.Set();
					}
				}
			}
			private static void ProcessFile(string localPath)
			{
				using (FileStream fileStream = new FileStream(localPath, FileMode.Open, FileSystemRights.Read, FileShare.Read, 512 * 1024, FileOptions.SequentialScan))
				{
					using (XmlReader xmlReader = XmlReader.Create(fileStream, XmlSchemaCatalog.SchemaReaderSettings))
					{
						Trace.TraceInformation("Loading schema from \"{0}\"", localPath);
						XmlSchema xmlSchema = XmlSchemaCatalog.XmlSchemaSet.Add(null, xmlReader);
						if (xmlSchema != null)
						{
							Trace.TraceInformation("Loaded schema for namespace \"{0}\" from \"{1}\"", xmlSchema.TargetNamespace, localPath);
						}
					}
				}
			}
		}
		#endregion // Loader class
	}
}
#endif //XML_SCHEMA_CATALOG