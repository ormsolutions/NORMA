using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using System.IO;
using System.Text;
using System.Diagnostics;
using Neumont.Tools.ORM.ObjectModel;
using Microsoft.VisualStudio.Modeling;
using Microsoft.XmlDiffPatch;

namespace Neumont.Tools.ORM.SDK.TestEngine
{
	#region SuiteCategory structure
	/// <summary>
	/// A suite category. One or more supported categories can
	/// be specified with the TestAttribute, and categories can
	/// be either included or excluded with the test file. A SuiteCategory
	/// has a name and an include setting.
	/// </summary>
	public struct SuiteCategory
	{
		private string myName;
		private bool myInclude;
		/// <summary>
		/// SuiteCategory constructor
		/// </summary>
		/// <param name="name">The name of the category</param>
		/// <param name="include">true if the category should be included</param>
		public SuiteCategory(string name, bool include)
		{
			myName = name;
			myInclude = include;
		}
		/// <summary>
		/// The category name
		/// </summary>
		public string Name
		{
			get
			{
				return myName;
			}
		}
		/// <summary>
		/// True to include the category, false to exclude it
		/// </summary>
		public bool Include
		{
			get
			{
				return myInclude;
			}
		}
		/// <summary>
		/// True to exclude the category, false to include it
		/// </summary>
		public bool Exclude
		{
			get
			{
				return !myInclude;
			}
		}
	}
	#endregion // SuiteCategory structure
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
	#region Suite structure
	/// <summary>
	/// A single suite to run a suite has a name, assemblies
	/// containing test classes, and (optionally) an ordered list of categories.
	/// </summary>
	public partial struct Suite
	{
		#region Instance data and accessors
		private string myName;
		private IList<SuiteAssembly> myAssemblies;
		private IList<SuiteCategory> myCategories;
		/// <summary>
		/// Suite constructor
		/// </summary>
		/// <param name="name">The suite name</param>
		/// <param name="assemblies">A list of assemblies to search for test classes</param>
		/// <param name="categories">An (optional) list of categories to include/exclude</param>
		public Suite(string name, IList<SuiteAssembly> assemblies, IList<SuiteCategory> categories)
		{
			myName = name;
			myAssemblies = assemblies;
			myCategories = categories;
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
		public IList<SuiteAssembly> Assemblies
		{
			get
			{
				return myAssemblies;
			}
		}
		/// <summary>
		/// A list of categories, or null to run all tests
		/// </summary>
		public IList<SuiteCategory> Categories
		{
			get
			{
				return myCategories;
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
		static Suite()
		{
			XmlReaderSettings readerSettings = new XmlReaderSettings();
			readerSettings.CloseInput = false;
			XmlWriterSettings writerSettings = new XmlWriterSettings();
			writerSettings.CloseOutput = false;
			DetachableReaderSettings = readerSettings;
			DetachableWriterSettings = writerSettings;
		}
		#endregion // Shared Xml reader/writer settings
		#region Schema definition classes
		#region SuiteLoaderSchema class
		private static class SuiteLoaderSchema
		{
			#region String Constants
			public const string SchemaNamespace = "http://schemas.neumont.edu/ORM/SDK/TestSuite";
			public const string SuitesElement = "Suites";
			public const string SuiteElement = "Suite";
			public const string TestAssembliesElement = "TestAssemblies";
			public const string TestAssemblyElement = "TestAssembly";
			public const string CategoriesElement = "Categories";
			public const string ExcludeCategoryElement = "ExcludeCategory";
			public const string IncludeCategoryElement = "IncludeCategory";
			public const string LocationAttribute = "location";
			public const string NameAttribute = "name";
			#endregion // String Constants
			#region Static properties
			private static SuiteLoaderNameTable myNames;
			public static SuiteLoaderNameTable Names
			{
				get
				{
					SuiteLoaderNameTable retVal = myNames;
					if (retVal == null)
					{
						lock (LockObject)
						{
							retVal = myNames;
							if (retVal == null)
							{
								retVal = myNames = new SuiteLoaderNameTable();
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
								retVal.Schemas.Add(SchemaNamespace, new XmlTextReader(typeof(Suite).Assembly.GetManifestResourceStream(typeof(Suite), "ORMTestSuite.xsd")));
								retVal.NameTable = Names;
							}
						}
					}
					return retVal;
				}
			}
			#endregion // Static properties
		}
		#endregion // SuiteLoaderSchema class
		#region SuiteLoaderNameTable class
		private class SuiteLoaderNameTable : NameTable
		{
			public readonly string SchemaNamespace;
			public readonly string SuitesElement;
			public readonly string SuiteElement;
			public readonly string TestAssembliesElement;
			public readonly string TestAssemblyElement;
			public readonly string CategoriesElement;
			public readonly string ExcludeCategoryElement;
			public readonly string IncludeCategoryElement;
			public readonly string LocationAttribute;
			public readonly string NameAttribute;
			public SuiteLoaderNameTable()
				: base()
			{
				SchemaNamespace = Add(SuiteLoaderSchema.SchemaNamespace);
				SuitesElement = Add(SuiteLoaderSchema.SuitesElement);
				SuiteElement = Add(SuiteLoaderSchema.SuiteElement);
				TestAssembliesElement = Add(SuiteLoaderSchema.TestAssembliesElement);
				TestAssemblyElement = Add(SuiteLoaderSchema.TestAssemblyElement);
				CategoriesElement = Add(SuiteLoaderSchema.CategoriesElement);
				ExcludeCategoryElement = Add(SuiteLoaderSchema.ExcludeCategoryElement);
				IncludeCategoryElement = Add(SuiteLoaderSchema.IncludeCategoryElement);
				LocationAttribute = Add(SuiteLoaderSchema.LocationAttribute);
				NameAttribute = Add(SuiteLoaderSchema.NameAttribute);
			}
		}
		#endregion // SuiteLoaderNameTable class
		#endregion // Schema definition classes
		#region Loading code
		/// <summary>
		/// Load the suite information from the specified file
		/// </summary>
		/// <param name="suiteFile">The path of the file to load</param>
		/// <returns>A list of suites loaded from the file</returns>
		public static IList<Suite> LoadSuiteFile(string suiteFile)
		{
			List<Suite> retVal = null;
			if (File.Exists(suiteFile))
			{
				FileInfo suiteFileInfo = new FileInfo(suiteFile);
				string baseDirectory = suiteFileInfo.DirectoryName;
				using (FileStream fileStream = suiteFileInfo.OpenRead())
				{
					SuiteLoaderNameTable names = SuiteLoaderSchema.Names;
					using (XmlTextReader suitesReader = new XmlTextReader(new StreamReader(fileStream), names))
					{
						using (XmlReader reader = XmlReader.Create(suitesReader, SuiteLoaderSchema.ReaderSettings))
						{
							bool finished = false;
							while (!finished && reader.Read())
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
											IList<SuiteAssembly> assemblies = null;
											IList<SuiteCategory> categories = null;
											if (!reader.IsEmptyElement)
											{
												while (reader.Read())
												{
													XmlNodeType nodeType2 = reader.NodeType;
													if (nodeType2 == XmlNodeType.Element)
													{
														string localName = reader.LocalName;
														if (TestElementName(localName, names.TestAssembliesElement))
														{
															assemblies = ProcessTestAssemblies(reader, names, baseDirectory);
														}
														else if (TestElementName(localName, names.CategoriesElement))
														{
															categories = ProcessCategories(reader, names);
														}
														else
														{
															Debug.Fail("Element should have been blocked by validing reader.");
															PassEndElement(reader);
														}
													}
													else if (nodeType2 == XmlNodeType.EndElement)
													{
														break;
													}
												}
											}
											if (retVal == null)
											{
												retVal = new List<Suite>();
											}
											retVal.Add(new Suite(suiteName, assemblies, categories));
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
			}
			return retVal;
		}
		private static IList<SuiteAssembly> ProcessTestAssemblies(XmlReader reader, SuiteLoaderNameTable names, string baseDirectory)
		{
			if (reader.IsEmptyElement)
			{
				return null;
			}
			List<SuiteAssembly> retVal = null;
			while (reader.Read())
			{
				XmlNodeType nodeType = reader.NodeType;
				if (nodeType == XmlNodeType.Element)
				{
					string localName = reader.LocalName;
					if (TestElementName(localName, names.TestAssemblyElement))
					{
						if (retVal == null)
						{
							retVal = new List<SuiteAssembly>();
						}
						retVal.Add(ProcessTestAssembly(reader, names, baseDirectory));
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
			return retVal;
		}
		private static SuiteAssembly ProcessTestAssembly(XmlReader reader, SuiteLoaderNameTable names, string baseDirectory)
		{
			Assembly retVal = null;
			string location = reader.GetAttribute(names.LocationAttribute);
			string fullAssemblyPath = string.Concat(baseDirectory, @"\", location);
			if (File.Exists(fullAssemblyPath))
			{
				try
				{
					retVal = Assembly.LoadFile(fullAssemblyPath);
				}
				catch (SystemException)
				{
					// Swallow other exceptions, report failure with a null
					throw;
				}
			}
			if (!reader.IsEmptyElement)
			{
				PassEndElement(reader);
			}
			return new SuiteAssembly(location, retVal);
		}
		private static IList<SuiteCategory> ProcessCategories(XmlReader reader, SuiteLoaderNameTable names)
		{
			if (reader.IsEmptyElement)
			{
				return null;
			}
			List<SuiteCategory> retVal = null;
			while (reader.Read())
			{
				XmlNodeType nodeType = reader.NodeType;
				if (nodeType == XmlNodeType.Element)
				{
					string localName = reader.LocalName;
					if (TestElementName(localName, names.ExcludeCategoryElement))
					{
						if (retVal == null)
						{
							retVal = new List<SuiteCategory>();
						}
						retVal.Add(ProcessCategory(reader, names, false));
					}
					else if (TestElementName(localName, names.IncludeCategoryElement))
					{
						if (retVal == null)
						{
							retVal = new List<SuiteCategory>();
						}
						retVal.Add(ProcessCategory(reader, names, true));
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
			return retVal;
		}
		private static SuiteCategory ProcessCategory(XmlReader reader, SuiteLoaderNameTable names, bool include)
		{
			string categoryName = reader.GetAttribute(names.NameAttribute);
			if (!reader.IsEmptyElement)
			{
				PassEndElement(reader);
			}
			return new SuiteCategory(categoryName, include);
		}
		private static bool TestElementName(string localName, string elementName)
		{
			return object.ReferenceEquals(localName, elementName);
		}
		/// <summary>
		/// Move the reader to the end element corresponding to the current open element
		/// levels above the current element
		/// </summary>
		/// <param name="reader">The XmlReader to advance</param>
		/// <param name="levels">The number of document levels to close</param>
		private static void PassEndElement(XmlReader reader, int levels)
		{
			bool skipNextRead = false;
			if (reader.IsEmptyElement && levels > 1)
			{
				reader.Skip();
				skipNextRead = true;
				--levels;
			}
			while (levels != 0)
			{
				if (!reader.IsEmptyElement)
				{
					bool finished = false;
					while (!finished && (skipNextRead ? !reader.EOF : reader.Read()))
					{
						skipNextRead = false;
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
				--levels;
			}
		}
		/// <summary>
		/// Move the reader to the end element corresponding to the current open element
		/// </summary>
		/// <param name="reader">The XmlReader to advance</param>
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
		#endregion // Loading code
		#region Run method
		/// <summary>
		/// Run all methods in each of the listed assemblies
		/// </summary>
		public void Run(IORMToolServices services, IORMToolTestSuiteReport suiteReport)
		{
			suiteReport.BeginSuite(myName);
			IList<SuiteAssembly> allAssemblies = myAssemblies;
			if (allAssemblies == null)
			{
				return;
			}
			int assemblyCount = allAssemblies.Count;
			for (int i = 0; i < assemblyCount; ++i)
			{
				SuiteAssembly testAssembly = allAssemblies[i];
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
		}
		#endregion // RunTests method and helper functions
		#region NUnit Integration
		/// <summary>
		/// Run a test of the same name as the calling method
		/// for the NUnit testing framework.
		/// </summary>
		/// <param name="testInstance">The instance being called</param>
		/// <param name="testServices">A services instance, created by CreateServices followed
		/// by a GetService call to retrieve IORMToolTestServices</param>
		public static void RunNUnitTest(object testInstance, IORMToolTestServices testServices)
		{
			MethodInfo nunitMethod = (MethodInfo)(new StackTrace(1, false)).GetFrame(0).GetMethod();
			MethodInfo testMethod = nunitMethod.DeclaringType.GetMethod(nunitMethod.Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(Store) }, null);
			Store store = null;
			try
			{
				testServices.OpenReport();
				store = testServices.Load(testMethod, null);
				testMethod.Invoke(testInstance, new object[] { store });
			}
			finally
			{
				// Compare the current contents of the store with the
				// expected state
				testServices.Compare(store, testMethod, null);
				testServices.LogValidationErrors(null);
				Type testType = testMethod.DeclaringType;
				using (XmlReader reportReader = testServices.CloseReport(testMethod))
				{
					string methodName = testMethod.Name;
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
							XmlDiff diff = Suite.DiffEngine;
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
										StringBuilder builder = new StringBuilder();
										using (XmlWriter xmlWriter = XmlWriter.Create(builder, DetachableWriterSettings))
										{
											xmlWriter.WriteStartElement("TestFailure");
											xmlWriter.WriteAttributeString("result", "failReportDiffgram");
											FormatXml(diffReader, xmlWriter);
											xmlWriter.WriteEndElement();
										}
										NUnit.Framework.Assert.Fail(builder.ToString());
										//suiteReport.ReportTestResults(methodName, ORMTestResult.FailReportDiffgram, diffReader);
									}
								}
								else
								{
									// Record a passing result by being quiet
									//suiteReport.ReportTestResults(methodName, ORMTestResult.Pass, null);
								}
							}
						}
						else
						{
							// Record the full report, we have no baseline to compare against
							StringBuilder builder = new StringBuilder();
							using (XmlWriter xmlWriter = XmlWriter.Create(builder, DetachableWriterSettings))
							{
								xmlWriter.WriteStartElement("TestFailure");
								xmlWriter.WriteAttributeString("result", "failReportMissingBaseline");
								FormatXml(reportReader, xmlWriter);
								xmlWriter.WriteEndElement();
							}
							NUnit.Framework.Assert.Fail(builder.ToString());
							//suiteReport.ReportTestResults(methodName, ORMTestResult.FailReportBaseline, reportReader);
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
		#endregion // NUnit Integration
		#region FormatXml Helper Function
		/// <summary>
		/// Get the formatting the way we want it. Duplicates
		/// reader contents into the current writer.
		/// </summary>
		/// <param name="reader">The xml to format</param>
		/// <param name="writer">The writer for the new Xml</param>
		private static void FormatXml(XmlReader reader, XmlWriter writer)
		{
			bool emptyElement;
			while (reader.Read())
			{
				switch (reader.NodeType)
				{
					case XmlNodeType.Element:
						writer.WriteStartElement(reader.Prefix, reader.LocalName, reader.NamespaceURI);
						emptyElement = reader.IsEmptyElement; // Read this before moving to an attribute
						while (reader.MoveToNextAttribute())
						{
							writer.WriteAttributeString(reader.Prefix, reader.LocalName, reader.NamespaceURI, reader.Value);
						}
						if (emptyElement)
						{
							writer.WriteEndElement();
						}
						break;
					case XmlNodeType.Text:
						writer.WriteString(reader.Value);
						break;
					case XmlNodeType.CDATA:
						writer.WriteCData(reader.Value);
						break;
					case XmlNodeType.ProcessingInstruction:
						writer.WriteProcessingInstruction(reader.Name, reader.Value);
						break;
					case XmlNodeType.Comment:
						writer.WriteComment(reader.Value);
						break;
					case XmlNodeType.Document:
						Debug.Assert(false, "Hit XmlNodeType.Document, not expected"); // Not expected
						break;
					case XmlNodeType.Whitespace:
						break;
					case XmlNodeType.SignificantWhitespace:
						writer.WriteWhitespace(reader.Value);
						break;
					case XmlNodeType.EndElement:
						writer.WriteEndElement();
						break;
				}
			}
			reader.Close();
		}
		#endregion // FormatXml Helper Function
	}
	#endregion // Suite structure
}