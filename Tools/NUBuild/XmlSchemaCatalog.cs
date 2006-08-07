//#define XML_SCHEMA_CATALOG
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

namespace Neumont.Tools.Xml
{
	// UNDONE: Until this is moved to a separate Neumont.Tools.Xml assembly, it is internal.
#if TRACE
	[Switch("XmlSchemaCacheSwitch", typeof(TraceSwitch))]
#endif //TRACE
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	internal static class XmlSchemaCatalog
	{
		#region Trace support
#if TRACE
		private static readonly TraceSwitch TraceSwitch = new TraceSwitch("XmlSchemaCacheSwitch", "Controls trace output for XmlSchemaCache", "Warning");
#endif //TRACE
		[Conditional("TRACE")]
		private static void TraceInformation(string message)
		{
#if TRACE
			if (TraceSwitch.TraceInfo)
			{
				Trace.TraceInformation(message);
			}
#endif //TRACE
		}
		[Conditional("TRACE")]
		private static void TraceInformation(string format, params object[] args)
		{
#if TRACE
			if (TraceSwitch.TraceInfo)
			{
				Trace.TraceInformation(format, args);
			}
#endif //TRACE
		}
		[Conditional("TRACE")]
		private static void TraceWarning(string message)
		{
#if TRACE
			if (TraceSwitch.TraceWarning)
			{
				Trace.TraceWarning(message);
			}
#endif //TRACE
		}
		[Conditional("TRACE")]
		private static void TraceWarning(string format, params object[] args)
		{
#if TRACE
			if (TraceSwitch.TraceWarning)
			{
				Trace.TraceWarning(format, args);
			}
#endif //TRACE
		}
		[Conditional("TRACE")]
		private static void TraceError(string message)
		{
#if TRACE
			if (TraceSwitch.TraceError)
			{
				Trace.TraceError(message);
			}
#endif //TRACE
		}
		[Conditional("TRACE")]
		private static void TraceError(string format, params object[] args)
		{
#if TRACE
			if (TraceSwitch.TraceError)
			{
				Trace.TraceError(format, args);
			}
#endif //TRACE
		}
		#endregion // Trace support

		public static readonly XmlNameTable XmlNameTable = new NameTable();
		public static readonly XmlResolver XmlResolver = new XmlUrlResolver();
		private static readonly XmlReaderSettings XmlReaderSettings = InitializeXmlReaderSettings();
		private static readonly XmlSchemaSet XmlSchemaSet = InitializeXmlSchemaSet();
		private static XmlReaderSettings InitializeXmlReaderSettings()
		{
			XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
			xmlReaderSettings.CloseInput = true;
			xmlReaderSettings.IgnoreComments = true;
			xmlReaderSettings.IgnoreWhitespace = true;
			xmlReaderSettings.NameTable = XmlSchemaCatalog.XmlNameTable;
			xmlReaderSettings.ProhibitDtd = false;
			xmlReaderSettings.ValidationFlags =
				XmlSchemaValidationFlags.AllowXmlAttributes |
				XmlSchemaValidationFlags.ProcessIdentityConstraints |
				XmlSchemaValidationFlags.ProcessInlineSchema |
				XmlSchemaValidationFlags.ProcessSchemaLocation;
			xmlReaderSettings.ValidationType = ValidationType.Schema;
			xmlReaderSettings.XmlResolver = XmlSchemaCatalog.XmlResolver;
			return xmlReaderSettings;
		}
		private static XmlSchemaSet InitializeXmlSchemaSet()
		{
			XmlSchemaSet xmlSchemaSet = new XmlSchemaSet(XmlSchemaCatalog.XmlNameTable);
			xmlSchemaSet.XmlResolver = XmlSchemaCatalog.XmlResolver;
#if TRACE
			xmlSchemaSet.ValidationEventHandler += XmlSchemaSetValidationEventHandler;
#endif //TRACE
			XmlSchemaCatalog.XmlReaderSettings.Schemas = xmlSchemaSet;
			return xmlSchemaSet;
		}

		#region XmlSchemaSetValidationEventHandler method
#if TRACE
		private static void XmlSchemaSetValidationEventHandler(object sender, ValidationEventArgs e)
		{
			if (e.Severity == XmlSeverityType.Error && TraceSwitch.TraceError)
			{
				Trace.TraceError(e.Message + Environment.NewLine + e.Exception);
			}
			else if (TraceSwitch.TraceWarning)
			{
				Trace.TraceWarning(e.Message + Environment.NewLine + e.Exception);
			}
		}
#endif //TRACE
		#endregion // XmlSchemaSetValidationEventHandler method


		private static class Loader
		{
			private static readonly object LockObject = new object();

			private static readonly ManualResetEvent LoadingFinished = new ManualResetEvent(true);
			private static readonly ManualResetEvent LoadingPending = new ManualResetEvent(false);

			private static string WritableCacheDirectory;

			private static readonly Queue<string> LoaderQueue = new Queue<string>(64);

			private static readonly Thread LoaderThread = new Thread(StartLoaderThread);

			public static void Start()
			{
				Thread loaderThread = LoaderThread;
				loaderThread.IsBackground = true;
				loaderThread.Name = "XmlSchemaCatalog Loader Thread";
				loaderThread.Priority = ThreadPriority.BelowNormal;
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
				SetEnvironmentVariable("System", Environment.SpecialFolder.System);
				SetEnvironmentVariable("ProgramFiles", Environment.SpecialFolder.ProgramFiles);
				SetEnvironmentVariable("Programs", Environment.SpecialFolder.Programs);
				SetEnvironmentVariable("CommonProgramFiles", Environment.SpecialFolder.CommonProgramFiles);
				SetEnvironmentVariable("ApplicationData", Environment.SpecialFolder.ApplicationData);
				SetEnvironmentVariable("CommonApplicationData", Environment.SpecialFolder.CommonApplicationData);
				SetEnvironmentVariable("MyDocs", Environment.SpecialFolder.Personal);

				SetEnvironmentVariable("LCID", CultureInfo.CurrentUICulture.LCID.ToString(NumberFormatInfo.InvariantInfo));

				if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("VsInstallDir")))
				{
					using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\VisualStudio\8.0\Setup\VS", RegistryKeyPermissionCheck.ReadSubTree))
					{
						if (key != null)
						{
							SetEnvironmentVariable("VsInstallDir", key.GetValue("ProductDir", null));
						}
					}
				}
			}
			#endregion // Environment variable setup

			private static void StartLoaderThread()
			{
				const string CACHE_DIRECTORY_NAME = @"Xml\Schemas";

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
						File.Create(Path.Combine(commonSchemasDirectoryInfo.FullName, "__TestFile.delete"), 0, FileOptions.DeleteOnClose).Close();
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

				if ((object)WritableCacheDirectory = null)
				{
					WritableCacheDirectory = userSchemasDirectoryInfo.FullName;
				}

				{
					string vsSchemaCacheLocation = null;
					using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\VisualStudio\8.0\XmlEditor", RegistryKeyPermissionCheck.ReadSubTree))
					{
						if (key != null)
						{
							vsSchemaCacheLocation = key.GetValue("SchemaCacheLocation", null);
						}
					}
					if (!string.IsNullOrEmpty(vsSchemaCacheLocation))
					{
						DirectoryInfo schemaCacheDirectoryInfo = new DirectoryInfo(vsSchemaCacheLocation);
						if (schemaCacheDirectoryInfo.Exists)
						{
							EnqueueDirectory(schemaCacheDirectoryInfo);
						}
					}
				}
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

			}
			private static void EnqueueFile(Uri uri)
			{
				if (!XmlSchemaSet.Contains(uri.ToString()))
				{
					string localPath;
					if (uri.IsFile || uri.IsUnc || File.Exists(localPath = uri.LocalPath))
					{
						EnqueueLocalFile(localPath);
					}
					else
					{
						// Start the download, and have the completion callback enqueue it.
					}
				}
			}
			private static void EnqueueLocalFile(string localPath)
			{
				lock (LockObject)
				{
					
				}
			}

			private static void Main()
			{
				ManualResetEvent loadingFinished = LoadingFinished;
				ManualResetEvent loadingPending = LoadingPending;
				
				while (true)
				{
					loadingPending.WaitOne();

					// If Queue empty, reset loadingPending and singnal LoadingFinished (do both inside lock!)
				}
			}
		}
	}
}
#endif //XML_SCHEMA_CATALOG