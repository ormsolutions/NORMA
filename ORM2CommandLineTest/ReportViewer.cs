using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Resources;

using Microsoft.XmlDiffPatch;
using System.Diagnostics;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.IO;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.SDK.TestEngine;


using EnvDTE;
using EnvDTE80;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Neumont.Tools.ORM.SDK.TestReportViewer
{

	public partial class ReportViewer : Form
	{
		#region private variables
		private string[] args;
		private const string MissingReportBaseline = "failReportMissingBaseline";
		private const string ReportDiffgram = "failReportDiffgram";
		private const string TestPassed = "pass";
		private string myBaseDirectory;
		IList<ReportSuite> myReportSuites;
		private TestInfo myTestInfo;

		private RichTextBox tbLeft;
		private TreeView reportTreeView;
		private SplitContainer splitContainerExpectedActual;
		private Button btnLeft;
		private Button btnRight;
		private Button btnCompare;
		private Label labelResult;
		private Label lblResult;
		private OpenFileDialog solutionFileDialog;
		private MenuStrip reportViewerMenu;
		private ToolStripMenuItem fileToolStripMenuItem;
		private ToolStripMenuItem openToolStripMenuItem;
		private ToolStripMenuItem clearToolStripMenuItem;
		private OpenFileDialog reportSuiteFileDialog;
		private SplitContainer splitContainerOuter;
		private Label labelTreeView;
		private Label labelExpected;
		private Label labelActual;
		private RichTextBox tbRight;
		
		


		private struct TestInfo
		{
			private string myTestNamespace;
			private string myTestClassName;
			private string myTestName;
			private string myResult;
			private string mySuiteName;
			private string myAssemblyLocation;
			private string myContent;
			private string myBaseFileName;
			private bool initialized;


			public string TestClassName
			{
				get { return myTestClassName; }
			}
			public string SuiteName
			{
				get { return mySuiteName; }
			}
			public string TestName
			{
				get { return myTestName; }
			}
			public string TestNamespace
			{
				get { return myTestNamespace; }
			}
			public string AssemblyLocation
			{
				get { return myAssemblyLocation; }
			}
			public string Content
			{
				get { return myContent; }
			}
			public string BaseFileName
			{
				get { return myBaseFileName; }
				set { myBaseFileName = value; }
			}
			public string Result
			{
				get
				{
					return myResult;
				}

			}
			public bool IsInitialized
			{
				get
				{
					// if value is null, return false.
					//return initialized == null ? false : initialized;
					return initialized;
				}
			}
			public void Initialize(IList<ReportSuite> reportSuites, TreeNode node)
			{
				//set private variables based on the contents of the tree node
				mySuiteName = node.Parent.Parent.Parent.Text;
				myAssemblyLocation = node.Parent.Parent.Text;
				myTestClassName = node.Parent.Text;
				myTestNamespace = node.Parent.Name;
				myTestName = node.Text;

				//with the information taken from the node, get the Result and Content information
				//by walking through the reportsuites structure.
				foreach (ReportSuite suite in reportSuites)
				{
					if (suite.Name == mySuiteName)
					{
						foreach (ReportAssembly reportAssembly in suite.Assemblies)
						{
							if (reportAssembly.Location == myAssemblyLocation)
							{
								foreach (TestClass testClass in reportAssembly.TestClasses)
								{
									if (testClass.Name == myTestClassName && testClass.TestNamespace == myTestNamespace)
									{
										foreach (Test test in testClass.Tests)
										{
											if (test.Name == myTestName)
											{
												myResult = test.Result;
												myContent = test.Content;
												myBaseFileName = String.Concat(myTestNamespace, ".", myTestClassName, ".", myTestName);
												initialized = true;
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}

		#endregion //private variables
		public ReportViewer(string[] args)
		{
			this.args = args;
			InitializeComponent();
		}


		#region set tree control
		private abstract class TestResultTreeNode : TreeNode
		{
			public TestResultTreeNode(string text)
				: base(text)
			{
			}
			public abstract bool IsPassing { get;}
		}
		private class PassingTreeNode : TestResultTreeNode
		{
			public PassingTreeNode(string text)
				: base(text)
			{
				ForeColor = Color.Green;
			}
			public override bool IsPassing
			{
				get
				{
					return true;
				}
			}
		}
		private class FailingTreeNode : TestResultTreeNode
		{
			public FailingTreeNode(string text)
				: base(text)
			{
				ForeColor = Color.Red;
			}
			public override bool IsPassing
			{
				get
				{
					return false;
				}
			}
		}
		private class UnknownTreeNode : TestResultTreeNode
		{
			public UnknownTreeNode(string text)
				: base(text)
			{
				ForeColor = Color.Black;
			}
			public override bool IsPassing
			{
				get
				{
					return false;
				}
			}
		}
		private class DummyTreeNode : TestResultTreeNode
		{
			public DummyTreeNode(string text)
				: base(text)
			{
			}
			public override bool IsPassing
			{
				get
				{
					return false;
				}
			}
		}
		private void SetTreeNodes(IList<ReportSuite> reportSuites)
		{
			foreach (ReportSuite suite in reportSuites)
			{
				TreeNode suitenode = new TreeNode(suite.Name);
				suitenode.Name = suite.Name;
				foreach (ReportAssembly reportAssembly in suite.Assemblies)
				{
					TreeNode assemblyNode = new TreeNode(reportAssembly.Location);
					assemblyNode.Name = reportAssembly.Location;
					foreach (TestClass testClass in reportAssembly.TestClasses)
					{
						TreeNode testClassNode = new TreeNode(testClass.Name);
						testClassNode.Name = testClass.TestNamespace;
						foreach (Test test in testClass.Tests)
						{
							bool testPassed = test.Passed;
							string testName = test.Name;
							TreeNode testNode = testPassed ? new PassingTreeNode(testName) as TreeNode : new FailingTreeNode(testName);
							if (!test.Passed)
							{

								testNode.Nodes.Add(new DummyTreeNode("-"));
							}
							testClassNode.Nodes.Add(testNode);
						}
						assemblyNode.Nodes.Add(testClassNode);
					}
					suitenode.Nodes.Add(assemblyNode);
				}
				reportTreeView.Nodes.Add(suitenode);
			}

		}
		#endregion
		#region XSL Transforms
		private static XslCompiledTransform myFakePassTransform;
		private XslCompiledTransform FakePassTransform
		{
			get
			{
				XslCompiledTransform retVal = myFakePassTransform;
				if (retVal == null)
				{
					lock (LockObject)
					{
						retVal = myFakePassTransform;
						if (retVal == null)
						{
							retVal = new XslCompiledTransform();
							Type resourceType = typeof(ReportSuite);
							using (Stream transformStream = resourceType.Assembly.GetManifestResourceStream(resourceType, "FakePass.xslt"))
							{
								using (StreamReader reader = new StreamReader(transformStream))
								{
									using (XmlReader xmlReader = new XmlTextReader(reader))
									{
										retVal.Load(xmlReader, null, null);
									}
								}
							}
							myFakePassTransform = retVal;
						}
					}
				}
				return retVal;
			}
		}
		#endregion
		#region EventHandlers

		private void ReportViewer_Load(object sender, EventArgs e)
		{
			if (args.Length > 0)
			{
				SetReportFile(args[0]);
			}
			reportTreeView.BeforeExpand += new TreeViewCancelEventHandler(reportTreeView_BeforeExpand);
			reportTreeView.BeforeSelect += new TreeViewCancelEventHandler(reportTreeView_BeforeSelect);
		}
		private void SetReportFile(string reportFile)
		{
			FileInfo reportFileInfo = new FileInfo(reportFile);
			if (reportFileInfo.Exists)
			{
				myBaseDirectory = reportFileInfo.DirectoryName + "\\";
				this.myReportSuites = ReportSuite.LoadReportSuitesFile(reportFile);
				SetTreeNodes(myReportSuites);
			}
		}

		void reportTreeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
		{
			tbLeft.Clear();
			tbRight.Clear();
			lblResult.Text = "";
			myTestInfo = new TestInfo();
			if (e.Node.Parent != null)
			{
				EnableButtons(e.Node is TestResultTreeNode || e.Node.Parent is TestResultTreeNode, e.Node.Parent is TestResultTreeNode);
				if (e.Node.Parent is TestResultTreeNode)
				{
					myTestInfo.Initialize(myReportSuites, e.Node.Parent);
					string result = myTestInfo.Result;
					lblResult.Text = result;
					tbRight.Clear();
					tbLeft.Clear();

					ReportAssembly reportAssembly = GetReportAssembly(myTestInfo);
					string nodeText = e.Node.Text;
					if (result == TestPassed)
					{
					}
					else if (result == MissingReportBaseline)
					{
						tbLeft.Text = FindMissingReport(reportAssembly, myTestInfo);
					}
					else if (result == ReportDiffgram)
					{
						tbLeft.Text = GetExpectedText(myTestInfo, reportAssembly, nodeText);

						if (!(e.Node is PassingTreeNode))
						{
							string actual = nodeText == "Report.xml"
								? GetFakeReportPassPath(myTestInfo, reportAssembly)
								: GetORMActualPath(myTestInfo, reportAssembly, nodeText);
							if (File.Exists(actual))
							{
								XmlReaderSettings readerSettings = new XmlReaderSettings();
								using (XmlReader reader = XmlReader.Create(File.OpenRead(actual), readerSettings))
								{
									reader.MoveToContent();
									tbRight.Text = reader.ReadOuterXml();
								}
							}
						}
					}
				}
				else if (e.Node is TestResultTreeNode)
				{
					myTestInfo.Initialize(myReportSuites, e.Node);
					lblResult.Text = myTestInfo.Result;

					//if the test has had its baseline updated, add a ?
					if (!(e.Node is PassingTreeNode) && e.Node.Nodes.Count == 0)
					{
						lblResult.Text += '?';
					}
					ReportAssembly reportAssembly = GetReportAssembly(myTestInfo);
					//get the expected report
					if (myTestInfo.Result == MissingReportBaseline)
					{
						tbLeft.Text = string.Format(ResourceStrings.FailReportMessageText, myTestInfo.TestClassName, myTestInfo.TestName);
					}
					else if (myTestInfo.Result == "pass")
					{
						tbLeft.Text = GetExpectedText(myTestInfo, reportAssembly, "Report.xml");
					}
					else
					{
						tbLeft.Text = "Test Failure";
					}
				}
			}
		}
		private string FindMissingReport(ReportAssembly reportAssembly, TestInfo testInfo)
		{
			string content = null;
			foreach (TestClass testclass in reportAssembly.TestClasses)
			{
				if (testclass.Name != testInfo.TestClassName || testclass.TestNamespace != testInfo.TestNamespace)
				{
					continue;
				}
				foreach (Test test in testclass.Tests)
				{
					if (test.Name != testInfo.TestName)
					{
						continue;
					}
					content = testInfo.Content;
					break;
				}
				break;
			}

			if (content == null)
			{
				return "XML failure: Report File Not Found.";
			}
			XmlWriterSettings writerSettings = new XmlWriterSettings();
			writerSettings.Indent = true;
			writerSettings.IndentChars = "\t";
			writerSettings.Encoding = Encoding.UTF8;
			using (MemoryStream memoryStream = new MemoryStream(content.Length * 2))
			{
				using (XmlWriter writer = XmlWriter.Create(memoryStream, writerSettings))
				{
					using (XmlReader reader = XmlReader.Create(new StringReader(content)))
					{

						// As this is an expected report, it is expected to pass!
						// Apply the fake pass template.
						XslCompiledTransform transform = FakePassTransform;
						transform.Transform(reader, writer);


						// Format the XML.
						FormatXml(reader, writer);

					}
				}
				memoryStream.Position = 0;



				return new StreamReader(memoryStream).ReadToEnd();
			}
		}
		private string FindMissingCompare(Assembly assembly, TestInfo testInfo, string stageName)
		{
			string baseResourceName = String.Concat(testInfo.TestNamespace, ".", testInfo.TestClassName, ".", testInfo.TestName);
			XmlPatch patcher = new XmlPatch();
			string tempfile = Path.GetTempFileName();
			StringReader diffReader = new StringReader(testInfo.Content);

			using (FileStream patchedstream = new FileStream(tempfile, FileMode.OpenOrCreate))
			{

				using (Stream reportstream = assembly.GetManifestResourceStream(string.Format("{0}.Report.xml", baseResourceName)))
				{
					if (reportstream == null)
					{
						return "Report File Is Missing.";
					}
					using (XmlReader reportReader = XmlReader.Create(reportstream))
					{
						using (XmlReader diffreader = XmlReader.Create(diffReader))
						{
							patcher.Patch(reportReader, patchedstream, diffreader);
						}
					}
				}
				patchedstream.Position = 0;
				using (MemoryStream memStream = new MemoryStream())
				{
					using (XmlReader patchedReader = XmlReader.Create(patchedstream))
					{

						using (XmlReader innerreader = XmlReader.Create(ExtractStringReaderFromCompareByName(patchedReader, stageName)))
						{

							XmlWriterSettings writerSettings = new XmlWriterSettings();
							writerSettings.Indent = true;
							writerSettings.IndentChars = "\t";
							using (XmlWriter writer = XmlWriter.Create(memStream, writerSettings))
							{
								FormatXml(innerreader, writer);
							}
						}
					}
					memStream.Position = 0;
					return new StreamReader(memStream).ReadToEnd();
				}
			}
		}
		private ReportAssembly GetReportAssembly(TestInfo testinfo)
		{
			ReportAssembly reportAssembly = new ReportAssembly();
			foreach (ReportSuite reportSuite in myReportSuites)
			{
				if (reportSuite.Name == myTestInfo.SuiteName)
				{
					foreach (ReportAssembly assembly in reportSuite.Assemblies)
					{
						if (assembly.Location == myTestInfo.AssemblyLocation)
						{
							reportAssembly = assembly;
						}
					}
				}
			}
			Debug.Assert(reportAssembly.Location != null, "Assembly failed to load. ");
			return reportAssembly;
		}
		void reportTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
		{
			if (e.Node is TestResultTreeNode)
			{
				myTestInfo.Initialize(myReportSuites, e.Node);
				lblResult.Text = myTestInfo.Result;

				//clear out the dummy subnode, if present...
				if (e.Node.Nodes.Count > 0)
				{
					if (e.Node.FirstNode is DummyTreeNode)
					{
						e.Node.Nodes.Clear();
						foreach (TreeNode node in GetTestCompareNodes(myTestInfo))
						{
							e.Node.Nodes.Add(node);
						}
					}
				}
			}
		}
		/// <summary>
		/// helper function to the BeforeExpand method. Takes a test info object and returns a
		/// collection of TreeNodes representing the Compare nodes in the report for that test.
		/// </summary>
		/// <param name="testInfo">struct containing information on a specific test.</param>
		/// <returns>a collection of TreeNodes representing the Compare nodes in the report for that test</returns
		private IList<TreeNode> GetTestCompareNodes(TestInfo testInfo)
		{
			IList<TreeNode> retval = new List<TreeNode>();
			string result = testInfo.Result;

			if (result == MissingReportBaseline)
			{
				TreeNode reportNode = new TreeNode("Report.xml");
				reportNode.Name = "missingBaseline";
				retval.Add(reportNode);
			}
			else if (result == ReportDiffgram)
			{
				ReportAssembly reportAssembly = GetReportAssembly(testInfo);

				XmlReaderSettings readerSettings = new XmlReaderSettings();

				//The nodes returned are based on the Compare nodes found in the result report file.
				//therefore, it is necessary to obtain this report file. 
				XmlPatch patcher = new XmlPatch();
				string tempFile = Path.GetTempFileName();
				Assembly assembly = reportAssembly.Assembly;
				StringReader innerreader = new StringReader(testInfo.Content);
				using (XmlReader reportReader = XmlReader.Create(assembly.GetManifestResourceStream(testInfo.BaseFileName + ".Report.xml"), readerSettings))
				{
					using (XmlReader diffReader = XmlReader.Create(innerreader, readerSettings))
					{
						using (FileStream fs = new FileStream(tempFile, FileMode.Open))
						{
							patcher.Patch(reportReader, fs, diffReader);
						}
					}
				}

				//the tempFile now contains a TestReport XML file. This has at lease 1 Compare node.
				//check it against a 'faked pass', to determine if a report node is needed.
				using (XmlReader reportReader = XmlReader.Create(assembly.GetManifestResourceStream(testInfo.BaseFileName + ".Report.xml"), readerSettings))
				{
					string fakepassPath = GetFakeReportPassPath(testInfo, reportAssembly);
					using (XmlReader fakepassReader = XmlReader.Create(fakepassPath, readerSettings))
					{
						using (XmlWriter disposableWriter = XmlWriter.Create(@"C:\deleteme.xml"))
						{
							XmlDiff diff = new XmlDiff(XmlDiffOptions.IgnoreXmlDecl);
							if (!diff.Compare(fakepassReader, reportReader, disposableWriter))
							{
								retval.Add(new TreeNode("Report.xml"));
							}
						}
					}
				}
				//Next, create a treenode object for each compare node in the XML.

				using (XmlReader testResultReader = XmlReader.Create(tempFile, readerSettings))
				{
					while (testResultReader.Read())
					{
						if (testResultReader.IsStartElement() && testResultReader.LocalName == "Compare")
						{
							string name = testResultReader.GetAttribute("name");
							string testresult = testResultReader.GetAttribute("result");
							string testName = string.Format("Compare{0}.orm", name == null ? "" : "." + name);
							TreeNode node = testresult == "pass" ? 
								new PassingTreeNode(testName) as TreeNode: 
								new FailingTreeNode(testName);
							retval.Add(node);
						}
					}
				}
			}

			return retval;
		}
		private void EnableButtons(bool updateEnableStatus, bool compareEnableStatus)
		{
			btnCompare.Enabled = compareEnableStatus;
			btnLeft.Enabled = updateEnableStatus;
			btnRight.Enabled = updateEnableStatus;
		}

		#endregion
		#region getTempFilePath functions
		//these functions use the current FileInfo object to create
		//temporary files. The testInfo object must be set before they are called.


		private string GetORMActualPath(TestInfo testInfo, ReportAssembly reportAssembly, string extension)
		{
			Assembly assembly = reportAssembly.Assembly;

			string tempFile = Path.GetTempFileName();
			foreach (TestClass testclass in reportAssembly.TestClasses)
			{
				if (testclass.Name == testInfo.TestClassName)
				{
					foreach (Test test in testclass.Tests)
					{
						if (test.Name == testInfo.TestName && testclass.TestNamespace == testInfo.TestNamespace)
						{
							string tempReportFile = Path.GetTempFileName();
							string result = test.Result;
							string baseResourceName = testInfo.BaseFileName;
							XmlReaderSettings readerSettings = new XmlReaderSettings();
							XmlWriterSettings writerSettings = new XmlWriterSettings();
							writerSettings.IndentChars = "\t";
							StringReader innerreader = new StringReader(test.Content);

							if (result == ReportDiffgram)
							{
								XmlPatch patcher = new XmlPatch();
								using (Stream stream = assembly.GetManifestResourceStream(baseResourceName + ".Report.xml"))
								{
									if (stream != null)
									{
										using (XmlReader reportReader = XmlReader.Create(stream, readerSettings))
										{
											StringReader compareStringReader;
											using (XmlReader diffReader = XmlReader.Create(innerreader, readerSettings))
											{

												using (FileStream fs = new FileStream(tempReportFile, FileMode.Create))
												{
													patcher.Patch(reportReader, fs, diffReader);
													fs.Position = 0;
													//the filestream is now the expected test report, containing a compare. 
													//Now to get that compare...

													string[] split = extension.Split('.');
													string stageName = split.Length == 3 ? split[1] : "";
													compareStringReader = ExtractStringReaderFromCompareByName(XmlReader.Create(fs), stageName);
												}
											}

											//and apply the diffgram within in to the original Compare.orm
											using (Stream compareStream = assembly.GetManifestResourceStream(string.Format("{0}.{1}", baseResourceName, extension)))
											{
												if (compareStream != null)
												{
													using (XmlReader compareReader = XmlReader.Create(compareStream, readerSettings))
													{
														using (MemoryStream patchedPatchStream = new MemoryStream())
														{
															using (XmlReader reportCompareReader = XmlReader.Create(compareStringReader, readerSettings))
															{
																//an exception thrown here is generally caused by the report suite being outdated. 
																patcher.Patch(compareReader, new UncloseableStream(patchedPatchStream), reportCompareReader);
															}
															patchedPatchStream.Position = 0;
															using (FileStream patchedFileStream = new FileStream(tempFile, FileMode.OpenOrCreate))
															{
																using (XmlReader reader = XmlReader.Create(patchedPatchStream))
																{
																	XmlWriterSettings formattedSettings = new XmlWriterSettings();
																	formattedSettings.Indent = true;
																	formattedSettings.IndentChars = "\t";
																	using (XmlWriter writer = XmlWriter.Create(patchedFileStream, formattedSettings))
																	{
																		FormatXml(reader, writer);
																	}
																}
															}
														}
													}
												}
												else
												{
													return null;
												}
											}
										}
									}
									else
									{
										return null;
									}
								}
								return tempFile;
							}
							else
							{
								return null;
							}
						}
					}
				}
			}
			return "Internal Error. function: GetORMActualPath.";
		}

		private string GetExpectedText(TestInfo testInfo, ReportAssembly reportAssembly, string extension)
		{
			Assembly assembly = reportAssembly.Assembly;

			string baseResourceName = String.Concat(testInfo.TestNamespace, ".", testInfo.TestClassName, ".", testInfo.TestName);

			string retval = null;
			using (Stream stream = assembly.GetManifestResourceStream(string.Format("{0}.{1}", baseResourceName, extension)))
			{
				if (stream != null)
				{

					XmlReaderSettings readerSettings = new XmlReaderSettings();
					using (XmlReader reader = XmlReader.Create(stream, readerSettings))
					{
						reader.MoveToContent();
						retval = reader.ReadOuterXml();
					}
				}
				else
				{

					string[] split = extension.Split('.');
					string stageName = split.Length == 3 ? split[1] : null;
					retval = FindMissingCompare(assembly, testInfo, stageName);
				}
			}
			return retval;
		}



		private string GetFakeReportPassPath(TestInfo testInfo, ReportAssembly reportAssembly)
		{
			Assembly assembly = reportAssembly.Assembly;
			XmlPatch patcher = new XmlPatch();
			XslCompiledTransform transform = FakePassTransform;
			XmlWriterSettings writerSettings = new XmlWriterSettings();
			writerSettings.Indent = true;
			writerSettings.IndentChars = "\t";
			XmlReaderSettings readerSettings = new XmlReaderSettings();

			string baseResourceName = String.Concat(testInfo.TestNamespace, ".", testInfo.TestClassName, ".", testInfo.TestName);
			string fakedPass = Path.GetTempFileName();
			string tempFile = Path.GetTempFileName();
			foreach (TestClass testclass in reportAssembly.TestClasses)
			{
				if (testclass.Name == testInfo.TestClassName)
				{
					foreach (Test test in testclass.Tests)
					{
						if (test.Name == testInfo.TestName && testclass.TestNamespace == testInfo.TestNamespace)
						{
							StringReader innerreader = new StringReader(test.Content);

							using (Stream stream = assembly.GetManifestResourceStream(baseResourceName + ".Report.xml"))
							{
								if (stream != null)
								{
									using (XmlReader reportReader = XmlReader.Create(stream, readerSettings))
									{
										using (XmlReader diffReader = XmlReader.Create(innerreader, readerSettings))
										{
											using (FileStream fs = new FileStream(tempFile, FileMode.Open))
											{
												patcher.Patch(reportReader, fs, diffReader);
											}
										}
									}
								}
								else
								{
									return null;
								}
							}
							//the tempfile is now the test report, containing a compare with an ugly diffgram. 
							//Now to fake its pass..
							using (FileStream fs = new FileStream(tempFile, FileMode.Open))
							{
								using (XmlReader failReader = XmlReader.Create(fs, readerSettings))
								{
									using (XmlWriter fakePassWriter = XmlWriter.Create(fakedPass, writerSettings))
									{
										transform.Transform(failReader, fakePassWriter);
									}

								}
							}
						}
					}
				}
			}


			return fakedPass;
		}


		private StringReader ExtractStringReaderFromCompareByName(XmlReader reader, string name)
		{
			//change an empty name to a null, for the comparison with the getAttribute function later
			name = name == "" ? null : name;

			reader.MoveToContent();
			while (reader.Read())
			{
				if (reader.LocalName == "Compare")
				{
					if (name == (reader.GetAttribute("name")))
					{
						return new StringReader(reader.ReadInnerXml());
					}
				}
			}
			//the proper Compare node was not found, return null
			return null;
		}

		#endregion
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
		#region Update Baseline & Compare buttons
		private void btnCompare_Click(object sender, EventArgs e)
		{

			string baselineFile = Path.GetTempFileName();
			string resultFile = Path.GetTempFileName();

			File.WriteAllText(baselineFile, tbLeft.Text);
			File.WriteAllText(resultFile, tbRight.Text);

			System.Diagnostics.Process proc = new System.Diagnostics.Process();
			ProcessStartInfo startInfo = proc.StartInfo;


			string programToShellTo = Settings.Default.DiffProgram;
			//@"C:\Program Files\Microsoft Visual Studio 8\Common7\Tools\Bin\WinDiff.exe";
			startInfo.FileName = programToShellTo;
			startInfo.Arguments = string.Format(@"""{0}"" ""{1}""", baselineFile, resultFile);
			startInfo.WindowStyle = (WindowState == FormWindowState.Maximized) ? ProcessWindowStyle.Maximized : ProcessWindowStyle.Normal;

			proc.Start();
			proc.WaitForInputIdle();
			Visible = false;
			proc.WaitForExit();
			Visible = true;
			WindowState = FormWindowState.Normal;
			Activate();
		}

		private void btnUpdateBaseline_Click(object sender, EventArgs e)
		{
			TreeNode currentNode = reportTreeView.SelectedNode;
			if (currentNode == null)
			{
				MessageBox.Show("Please make sure the correct item in the tree view to the left is selected.");
				return;
			}
			
			string extension = currentNode.Text;
			myTestInfo.Initialize(myReportSuites, currentNode.Parent);
			string newText = ((object)sender == btnLeft) ? tbLeft.Text : tbRight.Text;
			if (newText.Length < 2)
			{
				MessageBox.Show("Textbox cannot be empty.");
				return;
			}
			
			string assemblyLocation = myBaseDirectory + myTestInfo.AssemblyLocation;
			string solution = LookupSolution(assemblyLocation);

			Project project = null;
			string targetOuputFile = (new FileInfo(assemblyLocation)).Name;
			string rootNamespace = "";
			string projectDirectory = "";
			DTE2 dte = null;
			while (project == null)
			{
				if (solution.Length == 0 || !File.Exists(solution))
				{
					solution = MapAssemblyToSolution(assemblyLocation, solution, true);
					if (solution.Length == 0)
					{
						return;
					}
				}

				dte = FindDTEInstance(solution);
				if (dte == null)
				{
					dte = Activator.CreateInstance(Type.GetTypeFromProgID("VisualStudio.DTE." + Settings.Default.VisualStudioVersion)) as DTE2;
					dte.MainWindow.Visible = true;
					dte.Solution.Open(solution);
				}
				// Guid.Empty.ToString("B") creates 32 digits separated by hyphens, enclosed in brackets: 
				//{00000000-0000-0000-0000-000000000000} 

				if (!dte.Solution.IsOpen)
				{
					return;
				}

				// Find the appropriate project in the solution from the assembly path
				foreach (Project testProject in dte.Solution.Projects)
				{
					Properties properties = testProject.Properties;
					if (properties != null)
					{
						try
						{
							if (0 == string.Compare((string)properties.Item("OutputFileName").Value, targetOuputFile, StringComparison.OrdinalIgnoreCase))
							{
								rootNamespace = (string)properties.Item("RootNamespace").Value;
								projectDirectory = (string)properties.Item("FullPath").Value;
								project = testProject;
								break;
							}
						}
						catch (ArgumentException)
						{
							// Swallow it
						}
					}
				}
				if (project == null)
				{
					MapAssemblyToSolution(assemblyLocation, "", false);
					solution = "";
				}
			}

			string fileDirectory = myTestInfo.TestNamespace;
			int rootNamespaceLength = rootNamespace.Length;
			if (rootNamespaceLength != 0)
			{
				int directoryLength = fileDirectory.Length;
				if (directoryLength == rootNamespaceLength)
				{
					fileDirectory = "";
				}
				else if (directoryLength > (rootNamespaceLength + 1) &&
					fileDirectory[rootNamespaceLength] == '.' &&
					fileDirectory.StartsWith(rootNamespace))
				{
					fileDirectory = fileDirectory.Substring(rootNamespaceLength + 1);
				}
			}
			fileDirectory = fileDirectory.Replace('.', '\\');
			string testClassName = myTestInfo.TestClassName;
			string testName = myTestInfo.TestName;

			string fileName = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.{2}", testClassName, testName, extension);
			string filePath = (fileDirectory.Length == 0) ?
				string.Format(CultureInfo.InvariantCulture, @"{0}{1}", projectDirectory, fileName) :
				string.Format(CultureInfo.InvariantCulture, @"{0}{1}\{2}", projectDirectory, fileDirectory, fileName);

			bool documentUpdated = false;
			Document doc = null;
			if (dte.ItemOperations.IsFileOpen(filePath, Guid.Empty.ToString("B")))
			{
				doc = dte.Documents.Item(filePath);
				TextDocument textDoc = doc.Object("TextDocument") as TextDocument;
				if (textDoc != null)
				{
					textDoc.StartPoint.CreateEditPoint().ReplaceText(textDoc.EndPoint, newText, 0);
					documentUpdated = true;
				}
			}

			if (ProjectContains(project.ProjectItems, fileName))
			{
				// the project does contain the item.
				if (documentUpdated)
				{
					doc.Save("");
				}
				else if (doc != null)
				{
					doc.Close(vsSaveChanges.vsSaveChangesNo);
				}
				else
				{
					File.WriteAllText(filePath, newText, Encoding.UTF8);
				}
			}
			else
			{
				if (doc != null)
				{
					doc.Close(documentUpdated ? vsSaveChanges.vsSaveChangesYes : vsSaveChanges.vsSaveChangesNo);
				}
				//the file is not listed as part of the project or is not in the expacted namespace and must be added.
				AddFileToProject(project.ProjectItems, filePath, fileDirectory, newText);
			}

			//clear out the correct tree node, to prevent this method running again against this same node.
			TreeNodeFixup(currentNode);
			return;
		}


		#region replace baseline file helper methods
		private string MapAssemblyToSolution(string assemblyLocation, string solution, bool interactive)
		{
			if (interactive && solution.Length == 0 || !File.Exists(solution))
			{
				//request solution for the given assembly from the user
				solutionFileDialog.Title = "Solution file for " + assemblyLocation;
				if (solutionFileDialog.ShowDialog() == DialogResult.OK)
				{
					solution = solutionFileDialog.FileName;
				}
				else
				{
					solution = "";
				}
			}
			string oldMap = Settings.Default.DllMapping;
			string newMap = null;
			string lookupKey = ';' + assemblyLocation + '|';
			int assemblyIndex = oldMap.IndexOf(lookupKey);
			if (assemblyIndex == -1)
			{
				if (!string.IsNullOrEmpty(solution))
				{
					if (string.IsNullOrEmpty(oldMap))
					{
						newMap = lookupKey + solution + ';';
					}
					else
					{
						newMap = oldMap + assemblyLocation + "|" + solution + ';';
					}
				}
			}
			else
			{
				int trailingSemiColon = oldMap.IndexOf(';', assemblyIndex + lookupKey.Length);
				if (trailingSemiColon == -1)
				{
					// Shouldn't happen, abandon anything after this point
					newMap = oldMap.Substring(0, assemblyIndex + lookupKey.Length) + solution + ';';
				}
				else
				{
					newMap = oldMap.Substring(0, assemblyIndex + lookupKey.Length) + solution + oldMap.Substring(trailingSemiColon);
				}
			}
			if (newMap != null)
			{
				Settings.Default.DllMapping = newMap;
				Settings.Default.Save();
			}
			return solution;
		}
		#region solution lookup functions
		private static string LookupSolution(string assemblyLocation)
		{
			string map = Settings.Default.DllMapping;
			string solution = map.Substring(map.IndexOf(';' + assemblyLocation + '|')+1);
			if (solution.Length == 0)
			{
				return "";
			}
			solution = solution.Remove(solution.IndexOf(';'));
			return solution.Substring(solution.IndexOf('|') +1 );
		}
		#endregion //solution lookup function
		private static void AddFileToProject(ProjectItems items, string filePath, string subDirectories, string fileText)
		{
			//ProjectItem destination = items.Item(0);
			string[] directories = subDirectories.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
			int depth = directories.Length;
			for (int i = 0; i < depth && items != null; ++i)
			{
				// This assumes the directory is there. If it is not, then
				// the guidelines for creating test libraries were not followed
				string directoryName = directories[i];
				ProjectItems nextItems = null;
				foreach (ProjectItem testItem in items)
				{
					if (testItem.Name == directoryName)
					{
						nextItems = testItem.ProjectItems;
						break;
					}
				}
				items = nextItems;
			}
			if (items != null)
			{
				File.WriteAllText(filePath, fileText, Encoding.UTF8);
				ProjectItem newItem = items.AddFromFile(filePath);
				Properties properties = newItem.Properties;
				properties.Item("BuildAction").Value = 3;   // 3 == VSLangProj.prjBuildAction.prjBuildActionEmbeddedResource
				properties.Item("CustomTool").Value = "";
			}
		}
		private static bool ProjectContains(ProjectItems items, string fileName)
		{
			foreach (ProjectItem testItem in items)
			{
				if (fileName == testItem.Name)
				{
					return true;
				}
				if (ProjectContains(testItem.ProjectItems, fileName))
				{
					return true;
				}
			}
			return false;
		}

		[DllImport("ole32.dll")]
		private static extern int GetRunningObjectTable(
			int reserved,
			out IRunningObjectTable prot);
		[DllImport("ole32.dll")]
		private static extern int CreateBindCtx(
			int reserved,
			out IBindCtx pbc);
		private DTE2 FindDTEInstance(string fullName)
		{
			foreach (DTE2 dte in EnumerateRunningDTEInstances())
			{
				if (dte.Mode != vsIDEMode.vsIDEModeDebug)
				{
					Solution soln = dte.Solution;
					if (soln.IsOpen)
					{
						Debug.WriteLine(soln.FullName);
						if (soln.FullName == fullName)
						{
							return dte;
						}
					}
				}
			}
			//not found, return null
			return null;
		}
		private static IEnumerable<DTE2> EnumerateRunningDTEInstances()
		{
			IRunningObjectTable rot;
			ThrowOnFailure(GetRunningObjectTable(0, out rot));
			IEnumMoniker enumMoniker;
			IBindCtx bindContext;
			ThrowOnFailure(CreateBindCtx(0, out bindContext));
			rot.EnumRunning(out enumMoniker);
			IMoniker[] moniker = new IMoniker[1];
			int hrEnum = 0;
			string targetMoniker = "!VisualStudio.DTE." + Settings.Default.VisualStudioVersion + ":";
			for (; ; )
			{
				hrEnum = enumMoniker.Next(1, moniker, IntPtr.Zero);
				if (hrEnum == 1)
				{
					break;
				}
				ThrowOnFailure(hrEnum);
				string displayName = null;
				moniker[0].GetDisplayName(bindContext, null, out displayName);
				if (displayName != null)
				{

					if (displayName.StartsWith(targetMoniker))
					{
						object objDTE = null;
						if (0 == rot.GetObject(moniker[0], out objDTE) &&
							objDTE != null)
						{
							DTE2 dte = objDTE as DTE2;
							if (dte != null)
							{
								yield return dte;
							}
						}
					}
				}
			}
		}
		private static void ThrowOnFailure(int hr)
		{
			if (hr < 0)
			{
				Marshal.ThrowExceptionForHR(hr);
			}
		}
		#endregion //replace baseline file helper methods
		private void TreeNodeFixup(TreeNode currentNode)
		{
			TreeNode testNode = currentNode.Parent;
			testNode.Nodes.Clear();
			testNode.Nodes.Add(new UnknownTreeNode("Updated"));

		}
		#endregion // Update baseline
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
		
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportViewer));
			this.tbLeft = new System.Windows.Forms.RichTextBox();
			this.tbRight = new System.Windows.Forms.RichTextBox();
			this.reportTreeView = new System.Windows.Forms.TreeView();
			this.splitContainerExpectedActual = new System.Windows.Forms.SplitContainer();
			this.labelExpected = new System.Windows.Forms.Label();
			this.btnLeft = new System.Windows.Forms.Button();
			this.labelActual = new System.Windows.Forms.Label();
			this.btnRight = new System.Windows.Forms.Button();
			this.btnCompare = new System.Windows.Forms.Button();
			this.labelResult = new System.Windows.Forms.Label();
			this.lblResult = new System.Windows.Forms.Label();
			this.solutionFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.reportViewerMenu = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.reportSuiteFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.splitContainerOuter = new System.Windows.Forms.SplitContainer();
			this.labelTreeView = new System.Windows.Forms.Label();
			this.splitContainerExpectedActual.Panel1.SuspendLayout();
			this.splitContainerExpectedActual.Panel2.SuspendLayout();
			this.splitContainerExpectedActual.SuspendLayout();
			this.reportViewerMenu.SuspendLayout();
			this.splitContainerOuter.Panel1.SuspendLayout();
			this.splitContainerOuter.Panel2.SuspendLayout();
			this.splitContainerOuter.SuspendLayout();
			this.SuspendLayout();
			// 
			// tbLeft
			// 
			this.tbLeft.AcceptsTab = true;
			this.tbLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tbLeft.Location = new System.Drawing.Point(0, 16);
			this.tbLeft.Name = "tbLeft";
			this.tbLeft.Size = new System.Drawing.Size(377, 596);
			this.tbLeft.TabIndex = 3;
			this.tbLeft.Text = "";
			this.tbLeft.WordWrap = false;
			// 
			// tbRight
			// 
			this.tbRight.AcceptsTab = true;
			this.tbRight.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tbRight.Location = new System.Drawing.Point(3, 16);
			this.tbRight.Name = "tbRight";
			this.tbRight.Size = new System.Drawing.Size(357, 596);
			this.tbRight.TabIndex = 5;
			this.tbRight.Text = "";
			this.tbRight.WordWrap = false;
			// 
			// reportTreeView
			// 
			this.reportTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.reportTreeView.HideSelection = false;
			this.reportTreeView.Location = new System.Drawing.Point(0, 16);
			this.reportTreeView.Name = "reportTreeView";
			this.reportTreeView.Size = new System.Drawing.Size(312, 634);
			this.reportTreeView.TabIndex = 1;
			this.reportTreeView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.reportTreeView_BeforeExpand);
			this.reportTreeView.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.reportTreeView_BeforeSelect);
			// 
			// splitContainerExpectedActual
			// 
			this.splitContainerExpectedActual.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainerExpectedActual.Location = new System.Drawing.Point(3, 0);
			this.splitContainerExpectedActual.Name = "splitContainerExpectedActual";
			// 
			// splitContainerExpectedActual.Panel1
			// 
			this.splitContainerExpectedActual.Panel1.Controls.Add(this.tbLeft);
			this.splitContainerExpectedActual.Panel1.Controls.Add(this.labelExpected);
			this.splitContainerExpectedActual.Panel1.Controls.Add(this.btnLeft);
			// 
			// splitContainerExpectedActual.Panel2
			// 
			this.splitContainerExpectedActual.Panel2.Controls.Add(this.labelActual);
			this.splitContainerExpectedActual.Panel2.Controls.Add(this.btnRight);
			this.splitContainerExpectedActual.Panel2.Controls.Add(this.tbRight);
			this.splitContainerExpectedActual.Size = new System.Drawing.Size(740, 650);
			this.splitContainerExpectedActual.SplitterDistance = 376;
			this.splitContainerExpectedActual.SplitterWidth = 1;
			this.splitContainerExpectedActual.TabIndex = 3;
			// 
			// labelExpected
			// 
			this.labelExpected.AutoSize = true;
			this.labelExpected.Location = new System.Drawing.Point(0, 2);
			this.labelExpected.Name = "labelExpected";
			this.labelExpected.Size = new System.Drawing.Size(55, 13);
			this.labelExpected.TabIndex = 2;
			this.labelExpected.Text = "&Expected:";
			// 
			// btnLeft
			// 
			this.btnLeft.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnLeft.Location = new System.Drawing.Point(144, 615);
			this.btnLeft.Name = "btnLeft";
			this.btnLeft.Size = new System.Drawing.Size(88, 25);
			this.btnLeft.TabIndex = 6;
			this.btnLeft.Text = "Update to this";
			this.btnLeft.UseVisualStyleBackColor = true;
			this.btnLeft.Click += new System.EventHandler(this.btnUpdateBaseline_Click);
			// 
			// labelActual
			// 
			this.labelActual.AutoSize = true;
			this.labelActual.Location = new System.Drawing.Point(3, 3);
			this.labelActual.Name = "labelActual";
			this.labelActual.Size = new System.Drawing.Size(37, 13);
			this.labelActual.TabIndex = 4;
			this.labelActual.Text = "&Actual";
			// 
			// btnRight
			// 
			this.btnRight.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnRight.Location = new System.Drawing.Point(139, 615);
			this.btnRight.Name = "btnRight";
			this.btnRight.Size = new System.Drawing.Size(86, 25);
			this.btnRight.TabIndex = 7;
			this.btnRight.Text = "Update to this";
			this.btnRight.UseVisualStyleBackColor = true;
			this.btnRight.Click += new System.EventHandler(this.btnUpdateBaseline_Click);
			// 
			// btnCompare
			// 
			this.btnCompare.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnCompare.Location = new System.Drawing.Point(320, 656);
			this.btnCompare.Name = "btnCompare";
			this.btnCompare.Size = new System.Drawing.Size(118, 23);
			this.btnCompare.TabIndex = 8;
			this.btnCompare.Text = "&Compare";
			this.btnCompare.UseVisualStyleBackColor = true;
			this.btnCompare.Click += new System.EventHandler(this.btnCompare_Click);
			// 
			// labelResult
			// 
			this.labelResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.labelResult.AutoSize = true;
			this.labelResult.Location = new System.Drawing.Point(3, 656);
			this.labelResult.Name = "labelResult";
			this.labelResult.Size = new System.Drawing.Size(43, 13);
			this.labelResult.TabIndex = 5;
			this.labelResult.Text = "Result: ";
			// 
			// lblResult
			// 
			this.lblResult.AutoSize = true;
			this.lblResult.Location = new System.Drawing.Point(82, 569);
			this.lblResult.Name = "lblResult";
			this.lblResult.Size = new System.Drawing.Size(40, 13);
			this.lblResult.TabIndex = 6;
			this.lblResult.Text = "           ";
			// 
			// solutionFileDialog
			// 
			this.solutionFileDialog.Filter = "Solution File (*.sln)|*.sln|All Files|*.*";
			this.solutionFileDialog.Title = "Missing Solution File";
			// 
			// reportViewerMenu
			// 
			this.reportViewerMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
			this.reportViewerMenu.Location = new System.Drawing.Point(0, 0);
			this.reportViewerMenu.Name = "reportViewerMenu";
			this.reportViewerMenu.Size = new System.Drawing.Size(1061, 24);
			this.reportViewerMenu.TabIndex = 7;
			this.reportViewerMenu.Text = "reportiewerMenuStrip";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.clearToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.openToolStripMenuItem.Text = "&Open";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			// 
			// clearToolStripMenuItem
			// 
			this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
			this.clearToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.clearToolStripMenuItem.Text = "&Clear";
			this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
			// 
			// reportSuiteFileDialog
			// 
			this.reportSuiteFileDialog.DefaultExt = "Report.xml";
			this.reportSuiteFileDialog.Filter = "Report File|*.Report.xml|Xml Files|*.xml|All files|*.*";
			// 
			// splitContainerOuter
			// 
			this.splitContainerOuter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainerOuter.Location = new System.Drawing.Point(0, 27);
			this.splitContainerOuter.Name = "splitContainerOuter";
			// 
			// splitContainerOuter.Panel1
			// 
			this.splitContainerOuter.Panel1.Controls.Add(this.labelTreeView);
			this.splitContainerOuter.Panel1.Controls.Add(this.reportTreeView);
			this.splitContainerOuter.Panel1.Controls.Add(this.labelResult);
			// 
			// splitContainerOuter.Panel2
			// 
			this.splitContainerOuter.Panel2.Controls.Add(this.splitContainerExpectedActual);
			this.splitContainerOuter.Panel2.Controls.Add(this.btnCompare);
			this.splitContainerOuter.Size = new System.Drawing.Size(1061, 687);
			this.splitContainerOuter.SplitterDistance = 313;
			this.splitContainerOuter.SplitterWidth = 2;
			this.splitContainerOuter.TabIndex = 8;
			// 
			// labelTreeView
			// 
			this.labelTreeView.AutoSize = true;
			this.labelTreeView.Location = new System.Drawing.Point(4, 1);
			this.labelTreeView.Name = "labelTreeView";
			this.labelTreeView.Size = new System.Drawing.Size(67, 13);
			this.labelTreeView.TabIndex = 0;
			this.labelTreeView.Text = "&Report Tree:";
			// 
			// ReportViewer
			// 
			this.ClientSize = new System.Drawing.Size(1061, 714);
			this.Controls.Add(this.splitContainerOuter);
			this.Controls.Add(this.lblResult);
			this.Controls.Add(this.reportViewerMenu);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.reportViewerMenu;
			this.Name = "ReportViewer";
			this.Text = "ReportViewer";
			this.Load += new System.EventHandler(this.ReportViewer_Load);
			this.splitContainerExpectedActual.Panel1.ResumeLayout(false);
			this.splitContainerExpectedActual.Panel1.PerformLayout();
			this.splitContainerExpectedActual.Panel2.ResumeLayout(false);
			this.splitContainerExpectedActual.Panel2.PerformLayout();
			this.splitContainerExpectedActual.ResumeLayout(false);
			this.reportViewerMenu.ResumeLayout(false);
			this.reportViewerMenu.PerformLayout();
			this.splitContainerOuter.Panel1.ResumeLayout(false);
			this.splitContainerOuter.Panel1.PerformLayout();
			this.splitContainerOuter.Panel2.ResumeLayout(false);
			this.splitContainerOuter.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#region Menu options
		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (reportSuiteFileDialog.ShowDialog() == DialogResult.OK)
			{
				reportTreeView.Nodes.Clear();
				tbLeft.Clear();
				tbRight.Clear(); 
				SetReportFile(reportSuiteFileDialog.FileName);
				reportTreeView.Select();
			}
		}

		private void clearToolStripMenuItem_Click(object sender, EventArgs e)
		{
			reportTreeView.Nodes.Clear();
			tbLeft.Clear();
			tbRight.Clear();
		}
		#endregion Menu options
	}
}