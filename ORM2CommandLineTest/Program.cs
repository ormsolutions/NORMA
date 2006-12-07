using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.SDK.TestEngine;

namespace Neumont.Tools.ORM.SDK.TestDriver
{
	internal static class Program
	{
		public const string SchemaNamespace = "http://schemas.neumont.edu/ORM/SDK/TestSuite";
		public const string ReportSchemaNamespace = "http://schemas.neumont.edu/ORM/SDK/TestSuiteReport";

		private static int Main(string[] args)
		{
			string suiteFile = args[0];
			FileInfo suiteFileInfo = new FileInfo(suiteFile);
			string fullName = suiteFileInfo.FullName;
			string extension = suiteFileInfo.Extension;

			XmlReaderSettings readerSettings = new XmlReaderSettings();
			readerSettings.CloseInput = false;

			using (FileStream fileStream = suiteFileInfo.OpenRead())
			{
				XmlTextReader suitesReader = new XmlTextReader(new StreamReader(fileStream));
				using (XmlReader reader = XmlReader.Create(suitesReader, readerSettings))
				{
					reader.MoveToContent();
					string LoadingSchemaNamespace = reader.NamespaceURI;
					ORMSuiteReportResult result = ORMSuiteReportResult.NoFailure;
					if (LoadingSchemaNamespace == SchemaNamespace)
					{
						//If the suite is not a report then we need to generate a report.
						IList<Suite> suites = Suite.LoadSuiteFile(args[0]);
						if (suites != null)
						{
							int suiteCount = suites.Count;
							IORMToolServices services = Suite.CreateServices();
							XmlWriterSettings reportSettings = new XmlWriterSettings();
							reportSettings.Indent = true;
							reportSettings.IndentChars = "\t";
							using (XmlWriter reportWriter = XmlTextWriter.Create(string.Concat(fullName.Substring(0, fullName.Length - extension.Length), ".Report", extension), reportSettings))
							{
								IORMToolTestSuiteReport report = ((IORMToolTestSuiteReportFactory)services.ServiceProvider.GetService(typeof(IORMToolTestSuiteReportFactory))).Create(reportWriter);
								try
								{
									for (int i = 0; i < suiteCount; ++i)
									{
										suites[i].Run(services, report);
									}
								}
								finally
								{
									result = report.CloseSuiteReport();
								}	
							}
						}
					}
					else if (LoadingSchemaNamespace == ReportSchemaNamespace)
					{
						//TODO:  where to go with a report
						
					}
					return (int)result;
				}	
			}
		}
	}
}