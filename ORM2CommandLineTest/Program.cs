using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.SDK.TestEngine;

namespace Neumont.Tools.ORM.SDK.TestDriver
{
	class Program
	{
		static int Main(string[] args)
		{
			string suiteFile = args[0];
			IList<Suite> suites = Suite.LoadSuiteFile(args[0]);
			ORMSuiteReportResult result = ORMSuiteReportResult.NoFailure;
			if (suites != null)
			{
				int suiteCount = suites.Count;
				IORMToolServices services = Suite.CreateServices();
				FileInfo fileInfo = new FileInfo(suiteFile);
				string fullName = fileInfo.FullName;
				string extension = fileInfo.Extension;
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
			return (int)result;
		}
	}
}
