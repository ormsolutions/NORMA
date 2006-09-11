using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using System.IO;
using System.Text;
using System.Diagnostics;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.SDK.TestEngine;
using Microsoft.VisualStudio.Modeling;
using Microsoft.XmlDiffPatch;


namespace Neumont.Tools.ORM.SDK.TestReportViewer
{

	#region ReportAssembly structure
	/// <summary>
	/// A structure holding the relative location
	/// from the report file to a test assembly,
	/// as well as the resolved assembly.
	/// </summary>
	public struct ReportAssembly
	{
		private string myLocation;
		private string myFullLocation;
		private Assembly myAssembly;
		private IList<TestClass> myTestclasses;
		/// <summary>
		/// Create a ReportAssembly structure
		/// </summary>
		/// <param name="location">The relative path from
		/// the report file to the assembly location</param>
		/// <param name="assembly">The resolved assembly, or
		/// null if the assembly could not be loaded</param>
		public ReportAssembly(string location, string baseDirectory, IList<TestClass> testclasses)
		{
			myFullLocation = Path.Combine(baseDirectory, location);
			myLocation = location;
			myAssembly = null;
			myTestclasses = testclasses;
		}
		/// <summary>
		/// The relative path from
		/// the report file to the assembly location
		/// </summary>
		public string Location
		{
			get
			{
				return myLocation;
			}
		}
		/// <summary>
		/// The resolved assembly, or
		/// null if the assembly could not be loaded
		/// </summary>
		public Assembly Assembly
		{
			get
			{

				if (myAssembly == null)
				{
					if (File.Exists(myFullLocation))
					{
						try
						{
							myAssembly = Assembly.LoadFile(myFullLocation);
						}
						catch (SystemException)
						{
							// Swallow other exceptions, report failure with a null
							myAssembly = null;
							//throw;
						}
					}
				}
				return myAssembly;
			}
		}
		public IList<TestClass> TestClasses
		{
			get
			{
				return myTestclasses;
			}
		}
		public TestClass GetTestClass(string TestClassName)
		{
			
			foreach (TestClass testclass in myTestclasses)
			{
				if (testclass.Name == TestClassName)
				{
					return testclass;
				}
			}
			//return blank test class if none found
			return new TestClass();
		}
	}
	#endregion // ReportAssembly structure
	#region TestClass structure
	/// <summary>
	/// A structure to hold information about a
	/// specific test class.
	/// </summary>
	public struct TestClass
	{
		private string myNamespace;
		private string myName;
		private IList<Test> myTests;
		public TestClass(string testName, string testNamespace, IList<Test> testList)
		{
			this.myName = testName;
			this.myNamespace = testNamespace;
			this.myTests = testList;
		}


		public string TestNamespace
		{
			get
			{
				return myNamespace;
			}
		}
		public string Name
		{
			get
			{
				return myName;
			}
		}
		public IList<Test> Tests
		{
			get 
			{ 
				return myTests; 
			}

		}
		public Test GetTest(string testName)
		{
			foreach (Test test in myTests)
			{
				if (test.Name == testName)
				{
					return test;
				}
			}
			return new Test();
		}
	}
	
	#endregion //TestClass structure
	#region Test Structure


	public struct Test
	{
		private string myName;
		private string myResult;
		private string myInnerXml;
		
		public Test(string name, string result, string innerXml)
		{
			myName = name;
			myResult = result;
			myInnerXml = innerXml;

		}

		public string Name
		{
			get
			{
				return myName;
			}
		}
		public string Result
		{
			get
			{
				return myResult;
			}
		}
		public string Content
		{
			get
			{
				return myInnerXml;
			}
		}

		public bool Passed
		{
			get
			{
				return myResult == "pass";
			}
		}
	}
	#endregion //Test Structure
	#region TestReport Structure
	public struct TestReport
	{
		private string xmlns;


		public TestReport(string XmlNamespace)
		{
			xmlns = XmlNamespace;
		}

		public string XmlNamespace
		{
			get
			{
				return xmlns;
			}
			set
			{
				xmlns = value;
			}
		}

		
	}
	#endregion //TestReport Structure
	#region SuiteAssembly structure
	/// <summary>
	/// A structure holding the relative location
	/// from the suite file to a test assembly,
	/// as well as the resolved assembly.
	/// </summary>
	public struct SuiteAssembly
	{
		private string myLocation;
		private Assembly myAssembly;
		/// <summary>
		/// Create a SuiteAssembly structure
		/// </summary>
		/// <param name="location">The relative path from
		/// the suite file to the assembly location</param>
		/// <param name="assembly">The resolved assembly, or
		/// null if the assembly could not be loaded</param>
		public SuiteAssembly(string location, Assembly assembly)
		{
			myLocation = location;
			myAssembly = assembly;
		}
		/// <summary>
		/// The relative path from
		/// the suite file to the assembly location
		/// </summary>
		public string Location
		{
			get
			{
				return myLocation;
			}
		}
		/// <summary>
		/// The resolved assembly, or
		/// null if the assembly could not be loaded
		/// </summary>
		public Assembly Assembly
		{
			get
			{
				return myAssembly;
			}
		}
	}
	#endregion // SuiteAssembly structure
	public partial struct ReportSuite
	{
		#region Instance data and accessors
		private string myName;
		private IList<ReportAssembly> myAssemblies;
		

		public ReportSuite(string name, IList<ReportAssembly> assemblies)
		{
			myName = name;
			myAssemblies = assemblies;
			
		}
		/// <summary>
		/// The suite name
		/// </summary>
		public string Name
		{
			get
			{
				return myName;
			}
		}
		/// <summary>
		/// A list of assemblies to find test classes in
		/// </summary>
		public IList<ReportAssembly> Assemblies
		{
			get
			{
				return myAssemblies;
			}
		}

		#endregion // Instance data and accessors
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
		#region Shared Xml reader/writer settings
		/// <summary>
		/// XmlReaderSettings used to create an xml reader that does not
		/// automatically close the input stream when it is closed
		/// </summary>
		private static readonly XmlReaderSettings DetachableReaderSettings;
		/// <summary>
		/// XmlWriterSettings used to create an xml writer that does not
		/// automatically close the output stream when it is closed
		/// </summary>
		private static readonly XmlWriterSettings DetachableWriterSettings;
		/// <summary>
		/// A shared diff engine used for comparing xml streams
		/// </summary>
		private static readonly XmlDiff DiffEngine = new XmlDiff(XmlDiffOptions.IgnoreComments |
														XmlDiffOptions.IgnoreWhitespace |
														XmlDiffOptions.IgnoreXmlDecl |
														XmlDiffOptions.IgnorePI |
														XmlDiffOptions.IgnorePrefixes);
		/// <summary>
		/// Initialize shared settings
		/// </summary>
		static ReportSuite()
		{
			XmlReaderSettings readerSettings = new XmlReaderSettings();
			readerSettings.CloseInput = false;
			XmlWriterSettings writerSettings = new XmlWriterSettings();
			writerSettings.CloseOutput = false;
			DetachableReaderSettings = readerSettings;
			DetachableWriterSettings = writerSettings;
		}
		#endregion // Shared Xml reader/writer settings
		#region Loading Code
		public static IList<ReportSuite> LoadReportSuitesFile(string reportFile)
		{
			List<ReportSuite> retVal = null;
			if (!File.Exists(reportFile))
			{
				return null;
			}
			FileInfo reportSuiteFileInfo = new FileInfo(reportFile);
			string baseDirectory = reportSuiteFileInfo.DirectoryName;
			using (FileStream fileStream = reportSuiteFileInfo.OpenRead())
			{
				ReportViewerNameTable names = ReportViewerSchema.Names;
				using (XmlTextReader reportSuitesReader = new XmlTextReader(new StreamReader(fileStream), names))
				{
					using (XmlReader reader = XmlReader.Create(reportSuitesReader, ReportViewerSchema.ReaderSettings))
					{
						while (reader.Read())
						{
							if (reader.NodeType == XmlNodeType.Element && !reader.IsEmptyElement)
							{
								Debug.Assert(TestElementName(reader.LocalName, names.SuitesElement)); // Only value allowed by the validating loader
								while (reader.Read())
								{
									XmlNodeType nodeType1 = reader.NodeType;
									if (nodeType1 == XmlNodeType.Element)
									{
										// Process suite element
										Debug.Assert(TestElementName(reader.LocalName, names.SuiteElement)); // Only value allowed by the validating loader
										string suiteName = reader.GetAttribute(names.NameAttribute);
										IList<ReportAssembly> assemblies = null;
										if (!reader.IsEmptyElement)
										{
											assemblies = ProcessReportAssemblies(reader, names, baseDirectory);
										}
										if (retVal == null)
										{
											retVal = new List<ReportSuite>();
										}
										retVal.Add(new ReportSuite(suiteName, assemblies));
										
									}
									else if (nodeType1 == XmlNodeType.EndElement)
									{
										break;
									}		
								}
							}
						}
					}
				}
			}
			return retVal;
		}
		private static IList<ReportAssembly> ProcessReportAssemblies(XmlReader reader, ReportViewerNameTable names, string baseDirectory)
		{
			if (reader.IsEmptyElement)
			{
				return null;
			}
			List<ReportAssembly> retVal = null;
			while (reader.Read())
			{
				XmlNodeType nodeType = reader.NodeType;
				if (nodeType == XmlNodeType.Element)
				{
					string localName = reader.LocalName;
					Debug.Assert(TestElementName(localName, names.TestAssemblyElement));
					if (retVal == null)
					{
						retVal = new List<ReportAssembly>();
					}
					retVal.Add(ProcessReportAssembly(reader, names, baseDirectory));
				}
					
				else if (nodeType == XmlNodeType.EndElement)
				{
					break;
				}
				
			}
			return retVal;
		}
		private static ReportAssembly ProcessReportAssembly(XmlReader reader, ReportViewerNameTable names, string baseDirectory)
		{

			string location = reader.GetAttribute(names.LocationAttribute);

			List<TestClass> testclasses;
			testclasses = ProcessTestClasses(reader, names);

			if (!reader.IsEmptyElement)
			{
				PassEndElement(reader);
			}

			return new ReportAssembly(location,baseDirectory , testclasses);
		}
		private static List<TestClass> ProcessTestClasses(XmlReader reader, ReportViewerNameTable names)
		{
			if (reader.IsEmptyElement)
			{
				return null;
			}
			List<TestClass> retVal = null;
			
			while (reader.Read())
			{
				XmlNodeType nodeType = reader.NodeType;
				if (nodeType == XmlNodeType.Element)
				{
					string localName = reader.LocalName;
					if (TestElementName(localName, names.TestClassElement))
					{
						if (retVal == null)
						{
							retVal = new List<TestClass>();
						}
						retVal.Add(ProcessTestClass(reader, names));

					}
					else
					{
						Debug.Fail("Element should have been blocked by validing reader.");
						PassEndElement(reader);
					}
				}

			}
			return retVal;
		}
		private static TestClass ProcessTestClass(XmlReader reader, ReportViewerNameTable names)
		{
			string testNamespace = reader.GetAttribute(names.NamespaceAttribute);
			string testName = reader.GetAttribute(names.NameAttribute);
			List<Test> testList = null;

			while(reader.Read())
			{
				XmlNodeType nodeType = reader.NodeType;
				if (nodeType == XmlNodeType.Element)
				{
					string localName = reader.LocalName;
					if (TestElementName(localName, names.TestElement))
					{
						if (testList == null)
						{
							testList = new List<Test>();
						}
						testList.Add(ProcessTest(reader, names));
					}
					else
					{
						Debug.Fail("Element should have been blocked by validing reader.");
						PassEndElement(reader);
					}

				}
				else if (nodeType == XmlNodeType.EndElement)
				{
					break;
				}

			}

			return new TestClass(testName, testNamespace, testList);

		}
		private static Test ProcessTest(XmlReader reader, ReportViewerNameTable names)
		{
			string name = reader.GetAttribute(names.NameAttribute);
			string result = reader.GetAttribute(names.ResultAttribute);
			string content = reader.ReadInnerXml();
			return new Test(name, result, content);
		}
		private static bool TestElementName(string localName, string elementName)
		{
			return object.ReferenceEquals(localName, elementName);
		}
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
		#endregion
		#region ReportViewerSchema class
		private static class ReportViewerSchema
		{
			#region String Constants
			public const string SchemaNamespace = "http://schemas.neumont.edu/ORM/SDK/TestSuiteReport";
			public const string SuitesElement = "Suites";
			public const string SuiteElement = "Suite";
			public const string TestAssemblyElement = "TestAssembly";
			public const string TestClassElement = "TestClass";
			public const string TestElement = "Test";
			public const string TestReportElement = "TestReport";
			public const string CompareElement = "Compare";
			public const string ResultAttribute = "result";
			public const string NameAttribute = "name";
			public const string PassValue = "pass";
			public const string MissingBaselineValue = "failReportMissingBaseline";
			public const string ReportDiffgramValue = "failReportDiffgram";
			public const string LocationAttribute = "location";
			public const string NamespaceAttribute = "namespace";
			public const string XmlNamespaceAttribute = "xmlns";
			#endregion // String Constants
			#region Static properties
			private static ReportViewerNameTable myNames;
			public static ReportViewerNameTable Names
			{
				get
				{
					ReportViewerNameTable retVal = myNames;
					if (retVal == null)
					{
						lock (LockObject)
						{
							retVal = myNames;
							if (retVal == null)
							{
								retVal = myNames = new ReportViewerNameTable();
							}
						}
					}
					return retVal;
				}
			}
			private static XmlReaderSettings myReaderSettings;
			public static XmlReaderSettings ReaderSettings
			{
				get
				{
					XmlReaderSettings retVal = myReaderSettings;
					if (retVal == null)
					{
						lock (LockObject)
						{
							retVal = myReaderSettings;
							if (retVal == null)
							{
								retVal = myReaderSettings = new XmlReaderSettings();
								retVal.ValidationType = ValidationType.Schema;
								retVal.Schemas.Add(SchemaNamespace, new XmlTextReader(typeof(ReportSuite).Assembly.GetManifestResourceStream(typeof(ReportSuite), "ORMTestSuiteReport.xsd")));
								retVal.NameTable = Names;
							}
						}
					}
					return retVal;
				}
			}
			#endregion // Static properties
		}
		#endregion // ReportViewerSchema class
		#region ReportViewer Name Table
		private class ReportViewerNameTable : NameTable
		{
			public readonly string SchemaNamespace;
			public readonly string SuitesElement;
			public readonly string SuiteElement;
			public readonly string TestAssemblyElement;
			public readonly string TestClassElement;
			public readonly string TestElement;
			public readonly string TestReportElement;
			public readonly string CompareElement;
			public readonly string ResultAttribute;
			public readonly string NameAttribute;
			public readonly string PassValue;
			public readonly string MissingBaselineValue;
			public readonly string ReportDiffgramValue;
			public readonly string LocationAttribute;
			public readonly string NamespaceAttribute;
			public readonly string XmlNamespaceAttribute;

			public ReportViewerNameTable()
				: base()
			{
				SchemaNamespace = Add(ReportViewerSchema.SchemaNamespace);
				SuitesElement = Add(ReportViewerSchema.SuitesElement);
				SuiteElement = Add(ReportViewerSchema.SuiteElement);
				TestAssemblyElement = Add(ReportViewerSchema.TestAssemblyElement);
				TestClassElement = Add(ReportViewerSchema.TestClassElement);
				TestElement = Add(ReportViewerSchema.TestElement);
				TestReportElement = Add(ReportViewerSchema.TestReportElement);
				CompareElement = Add(ReportViewerSchema.CompareElement);
				ResultAttribute = Add(ReportViewerSchema.ResultAttribute);
				NameAttribute = Add(ReportViewerSchema.NameAttribute);
				PassValue = Add(ReportViewerSchema.PassValue);
				MissingBaselineValue = Add(ReportViewerSchema.MissingBaselineValue);
				ReportDiffgramValue = Add(ReportViewerSchema.ReportDiffgramValue);
				LocationAttribute = Add(ReportViewerSchema.LocationAttribute);
				NamespaceAttribute = Add(ReportViewerSchema.NamespaceAttribute);
				XmlNamespaceAttribute = Add(ReportViewerSchema.XmlNamespaceAttribute);
			}
		}
		#endregion

		#region Run method
		/// <summary>
		/// Run all methods in each of the listed assemblies
		/// </summary>
		public void Run(IORMToolServices services, IORMToolTestSuiteReport suiteReport)
		{

			suiteReport.BeginSuite(myName);
			IList<ReportAssembly> allAssemblies = myAssemblies;
			if (allAssemblies == null)
			{
				return;
			}
			int assemblyCount = allAssemblies.Count;
			for (int i = 0; i < assemblyCount; ++i)
			{
				ReportAssembly testAssembly = allAssemblies[i];
				Assembly resolvedAssembly = testAssembly.Assembly;
				suiteReport.BeginTestAssembly(testAssembly.Location, resolvedAssembly == null);
				if (resolvedAssembly != null)
				{
					Type[] types = resolvedAssembly.GetTypes();
					int typeCount = types.Length;
					for (int j = 0; j < typeCount; ++j)
					{
						Type type = types[j];
						if (0 != type.GetCustomAttributes(typeof(ORMTestFixtureAttribute), true).Length)
						{
							RunTests(type, services, suiteReport);
						}
					}
				}
			}
			 
		}
		#endregion // Run method
		#region RunTests method and helper functions
		/// <summary>
		/// Run all tests for the given type that match the
		/// category filters specified for this suite
		/// </summary>
		/// <param name="testType">A type with a Test attribute</param>
		/// <param name="services">Services used to run the test. IORMToolTestServices can
		/// be retrieved from services.ServiceProvider.</param>
		/// <param name="suiteReport">The suite report callback</param>
		private void RunTests(Type testType, IORMToolServices services, IORMToolTestSuiteReport suiteReport)
		{
			IORMToolTestServices testServices = (IORMToolTestServices)services.ServiceProvider.GetService(typeof(IORMToolTestServices));
			object testTypeInstance = null;
			object[] methodParams = null;
			MethodInfo[] methods = testType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
			int methodCount = methods.Length;
			for (int i = 0; i < methodCount; ++i)
			{
				MethodInfo method = methods[i];
				object[] testAttributes = method.GetCustomAttributes(typeof(ORMTestAttribute), false);
				Debug.Assert(testAttributes.Length < 2, "Single use attribute with inherit=false, should only pick up zero or one attributes");
				// Make sure that the method is flagged as a Test method that can be run per the current category settings
				if (testAttributes.Length == 0 || !CheckCategoryFilters((ORMTestAttribute)testAttributes[0]))
				{
					continue;
				}

				// Make sure we've instantiated the test class
				if (testTypeInstance == null)
				{
					ConstructorInfo constructor;
					if (null != (constructor = testType.GetConstructor(new Type[] { typeof(IORMToolServices) })))
					{
						testTypeInstance = constructor.Invoke(new object[] { services });
						methodParams = new object[1];
					}
					bool loadFailure = null == testTypeInstance;
					suiteReport.BeginTestClass(testType.Namespace, testType.Name, loadFailure);
					if (loadFailure)
					{
						return;
					}
				}

				ParameterInfo[] methodParamInfos = method.GetParameters();
				if (!(methodParamInfos.Length == 1 && typeof(Store).IsAssignableFrom(methodParamInfos[0].ParameterType)))
				{
					// The test method does not match the signature we need, it should
					// not have been marked with the Test attribute
					suiteReport.ReportTestResults(method.Name, ORMTestResult.FailBind, null);
				}
				else
				{
					Store store = null;
					try
					{
						// Prepare the test services for a new test
						testServices.OpenReport();

						// Populate a store. Automatically loads the starting
						// file from the test assembly if one is provided
						store = testServices.Load(method, null);

						// Run the method
						methodParams[0] = store;
						method.Invoke(testTypeInstance, methodParams);

						// Compare the current contents of the store with the
						// expected state
						testServices.Compare(store, method, null);
						testServices.LogValidationErrors(null);
					}
					finally
					{
						if (store != null)
						{
							((IDisposable)store).Dispose();
						}

						// Close the report and see if the report matches the expected results
						using (XmlReader reportReader = testServices.CloseReport(method))
						{
							string methodName = method.Name;
							string resourceName = string.Concat(testType.FullName, ".", methodName, ".Report.xml");
							Stream baselineStream = null;
							try
							{
								Assembly testAssembly = testType.Assembly;
								// Get the baseline that we're comparing to
								if (null != testAssembly.GetManifestResourceInfo(resourceName))
								{
									baselineStream = testAssembly.GetManifestResourceStream(resourceName);
								}
								if (baselineStream != null)
								{
									bool hasDiff = false;

									// See if the data is different.
									XmlDiff diff = DiffEngine;
									XmlReaderSettings readerSettings = DetachableReaderSettings;
									XmlWriterSettings writerSettings = DetachableWriterSettings;
									using (MemoryStream diffStream = new MemoryStream())
									{
										using (XmlReader baselineReader = XmlReader.Create(baselineStream, readerSettings))
										{
											using (XmlWriter diffWriter = XmlWriter.Create(diffStream, writerSettings))
											{
												hasDiff = !diff.Compare(baselineReader, reportReader, diffWriter);
											}
										}
										if (hasDiff)
										{
											// Record the diffgram in the suite report
											diffStream.Seek(0, SeekOrigin.Begin);
											using (XmlReader diffReader = XmlTextReader.Create(diffStream, readerSettings))
											{
												suiteReport.ReportTestResults(methodName, ORMTestResult.FailReportDiffgram, diffReader);
											}
										}
										else
										{
											// Record a passing result
											suiteReport.ReportTestResults(methodName, ORMTestResult.Pass, null);
										}
									}
								}
								else
								{
									// Record the full report, we have no baseline to compare against
									suiteReport.ReportTestResults(methodName, ORMTestResult.FailReportBaseline, reportReader);
								}
							}
							finally
							{
								if (baselineStream != null)
								{
									((IDisposable)baselineStream).Dispose();
								}
							}
						}
					}
				}
			}
		}
		/// <summary>
		/// Verify that the current category settings for the suite
		/// allow the current test method to be executed
		/// </summary>
		/// <param name="testAttr">A TestAttribute from a test method</param>
		/// <returns>true to run the test</returns>
		private bool CheckCategoryFilters(ORMTestAttribute testAttr)
		{
			return false;
			/*
			bool runTest = true; // Default to true
			IList<SuiteCategory> categories = myCategories;
			if (categories != null)
			{
				// Now walk categories in order. The last category wins if there
				// is a difference of opinion between inclusion/exclusion.
				int catCount = categories.Count;
				int forceInclude = -1;
				int forceExclude = -1;
				bool sawInclude = false;
				for (int catIndex = 0; catIndex < catCount; ++catIndex)
				{
					SuiteCategory curCat = categories[catIndex];
					bool recognized = testAttr.SupportsCategory(curCat.Name);
					bool include = curCat.Include;
					if (recognized)
					{
						if (include)
						{
							forceInclude = catIndex;
						}
						else
						{
							forceExclude = catIndex;
						}
					}
					else if (include)
					{
						sawInclude = true;
					}
					if (forceInclude != -1)
					{
						runTest = forceInclude > forceExclude;
					}
					else if (forceExclude != -1 || sawInclude)
					{
						runTest = false;
					}
				}
			}
			return runTest;
			*/
		}
		#endregion // RunTests method and helper functions
	}
}
