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
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitectSDK.TestEngine;


using EnvDTE;
using EnvDTE80;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace ORMSolutions.ORMArchitectSDK.TestReportViewer
{

	public partial class ReportViewer : Form
	{
		#region private variables
		private string[] args;
		private const string MissingReportBaseline = "failReportMissingBaseline";
		private const string MissingBaseline = "failMissingBaseline";
		private const string ReportDiffgram = "failReportDiffgram";
		private const string TestPassed = "pass";
		private const string ReportFileName = "Report.xml";
		private string myBaseDirectory;
		IList<ReportSuite> myReportSuites;

		private RichTextBox tbLeft;
		private TreeView reportTreeView;
		private SplitContainer splitContainerExpectedActual;
		private Button btnUpdateBaseline;
		private Button btnCompare;
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
		private ToolStripSeparator toolStripMenuItem1;
		private ToolStripMenuItem exitToolStripMenuItem;
		private ToolTip toolTips;
		private IContainer components;
		private ToolStripMenuItem optionsToolStripMenuItem;
		private ToolStripMenuItem compareApplicationToolStripMenuItem;
		private ToolStripMenuItem associatedSolutionToolStripMenuItem;
		private OpenFileDialog compareApplicationFileDialog;
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
			public void Initialize(IList<ReportSuite> reportSuites, TestCaseNode node)
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
		/// <summary>
		/// Color representing that the testcase passed in the report file
		/// </summary>
		public static Color PassedOnLoadColor
		{
			get
			{
				return Color.Green;
			}
		}
		/// <summary>
		/// Color representing that the testcase failed in the report file
		/// </summary>
		public static Color FailedOnLoadColor
		{
			get
			{
				return Color.Red;
			}
		}
		/// <summary>
		/// Color representng that the testcase is in a repaired state
		/// </summary>
		public static Color RepairedLoadFailureColor
		{
			get
			{
				return SystemColors.WindowText;
			}
		}
		/// <summary>
		/// The failure state of a <see cref="FailStateTreeNode"/>
		/// </summary>
		private enum NodeFailState
		{
			/// <summary>
			/// The node represents a currently passing test
			/// </summary>
			Passed,
			/// <summary>
			/// The node represents an unrepaired test
			/// </summary>
			Failed,
			/// <summary>
			/// The node represents a repaired test
			/// </summary>
			Repaired,
		}
		private abstract class FailStateNode : TreeNode
		{
			public FailStateNode(string text)
				: base(text)
			{
			}
			public abstract NodeFailState FailState { get;}
		}
		private abstract class CompareNode : FailStateNode
		{
			public CompareNode(string text)
				: base(text)
			{
			}
		}
		private class PassingCompareNode : CompareNode
		{
			public PassingCompareNode(string text)
				: base(text)
			{
				ForeColor = PassedOnLoadColor;
			}
			public override NodeFailState FailState
			{
				get
				{
					return NodeFailState.Passed;
				}
			}
		}
		private class FailingCompareNode : CompareNode
		{
			private bool myIsRepaired;
			private bool myIsMissing;
			public FailingCompareNode(string text, bool baselineMissing)
				: base(text)
			{
				ForeColor = FailedOnLoadColor;
				myIsMissing = baselineMissing;
			}
			public override NodeFailState FailState
			{
				get
				{
					return myIsRepaired ? NodeFailState.Repaired : NodeFailState.Failed;
				}
			}
			/// <summary>
			/// Has the file for this node been updated?
			/// </summary>
			public bool IsRepaired
			{
				get
				{
					return myIsRepaired;
				}
				set
				{
					bool oldRepaired = myIsRepaired;
					if (oldRepaired != value)
					{
						ContainerNode parentNode = Parent as ContainerNode;
						if (oldRepaired)
						{
							// Switch to failed
							ForeColor = FailedOnLoadColor;
							if (parentNode != null)
							{
								parentNode.AddFailure();
							}
						}
						else
						{
							// Switch to repaired
							ForeColor = RepairedLoadFailureColor;
							if (parentNode != null)
							{
								parentNode.RemoveFailure();
							}
						}
						myIsRepaired = value;
					}
				}
			}
			/// <summary>
			/// Is this failure node for a missing baseline file as opposed
			/// to a diffgram?
			/// </summary>
			public bool MissingBaseline
			{
				get
				{
					return myIsMissing;
				}
			}
		}
		private class DummyTreeNode : CompareNode
		{
			public DummyTreeNode(string text)
				: base(text)
			{
			}
			public override NodeFailState FailState
			{
				get
				{
					return NodeFailState.Failed;
				}
			}
		}
		private class ContainerNode : FailStateNode
		{
			private int myFailCount;
			public ContainerNode(string text)
				: base(text)
			{
				myFailCount = -1;
				ForeColor = PassedOnLoadColor;
			}
			/// <summary>
			/// Add a new failure to this node
			/// </summary>
			public void AddFailure()
			{
				int failCount = myFailCount;
				if (failCount == -1)
				{
					failCount = 0;
				}
				if (failCount == 0)
				{
					ForeColor = FailedOnLoadColor;
					ContainerNode parentNode = this.Parent as ContainerNode;
					if (parentNode != null)
					{
						parentNode.AddFailure();
					}
				}
				++failCount;
				myFailCount = failCount;
			}
			/// <summary>
			/// Remove a failure from this node
			/// </summary>
			public void RemoveFailure()
			{
				int failCount = myFailCount;
				Debug.Assert(failCount > 0);
				if (failCount <= 0)
				{
					return;
				}
				if (failCount == 1)
				{
					ForeColor = RepairedLoadFailureColor;
					ContainerNode parentNode = this.Parent as ContainerNode;
					if (parentNode != null)
					{
						parentNode.RemoveFailure();
					}
				}
				--failCount;
				myFailCount = failCount;
			}
			/// <summary>
			/// Call when the node is initially attached to its parent node
			/// </summary>
			public void NotifyAttachedToParent()
			{
				ContainerNode parentNode = this.Parent as ContainerNode;
				if (parentNode != null)
				{
					switch (myFailCount)
					{
						case -1:
							// Nothing to do, leave the parent alone
							break;
						case 0:
							// Make sure the parent node does not have a 'passed on load' state
							parentNode.AddFailure();
							parentNode.RemoveFailure();
							break;
						default:
							parentNode.AddFailure();
							break;
					}
				}
			}
			public void NotifyChildAttached(FailStateNode childNode)
			{
				switch (childNode.FailState)
				{
					case NodeFailState.Failed:
						AddFailure();
						break;
					case NodeFailState.Repaired:
						AddFailure();
						RemoveFailure();
						break;
				}
			}
			public void NotifyChildDetached(FailStateNode childNode)
			{
				switch (childNode.FailState)
				{
					case NodeFailState.Failed:
						RemoveFailure();
						break;
				}
			}
			public override NodeFailState FailState
			{
				get
				{
					switch (myFailCount)
					{
						case -1:
							return NodeFailState.Passed;
						case 0:
							return NodeFailState.Repaired;
						default:
							return NodeFailState.Failed;
					}
				}
			}
		}
		private class TestCaseNode : ContainerNode
		{
			public TestCaseNode(string text)
				: base(text)
			{
			}
		}
		private void SetTreeNodes(IList<ReportSuite> reportSuites)
		{
			foreach (ReportSuite suite in reportSuites)
			{
				ContainerNode suitenode = new ContainerNode(suite.Name);
				suitenode.Name = suite.Name;
				foreach (ReportAssembly reportAssembly in suite.Assemblies)
				{
					ContainerNode assemblyNode = new ContainerNode(reportAssembly.Location);
					assemblyNode.Name = reportAssembly.Location;
					foreach (TestClass testClass in reportAssembly.TestClasses)
					{
						ContainerNode testClassNode = new ContainerNode(testClass.Name);
						testClassNode.Name = testClass.TestNamespace;
						foreach (Test test in testClass.Tests)
						{
							bool testPassed = test.Passed;
							string testName = test.Name;
							TestCaseNode testNode = new TestCaseNode(testName);
							if (!testPassed)
							{
								FailStateNode dummy = new DummyTreeNode("-");
								testNode.Nodes.Add(dummy);
								testNode.NotifyChildAttached(dummy);
							}
							testClassNode.Nodes.Add(testNode);
							testClassNode.NotifyChildAttached(testNode);
						}
						assemblyNode.Nodes.Add(testClassNode);
						testClassNode.NotifyAttachedToParent();
					}
					suitenode.Nodes.Add(assemblyNode);
					assemblyNode.NotifyAttachedToParent();
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
		}
		private void ReportViewer_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Modifiers == Keys.Control)
			{
				switch (e.KeyCode)
				{
					case Keys.M:
						if (btnCompare.Enabled)
						{
							btnCompare_Click(null, null);
							e.Handled = true;
						}
						break;
					case Keys.B:
						if (btnUpdateBaseline.Enabled)
						{
							btnUpdateBaseline_Click(null, null);
							e.Handled = true;
						}
						break;
				}
			}
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
			TestInfo testInfo = new TestInfo();
			FailStateNode node = (FailStateNode)e.Node;
			TreeNode parentNode = node.Parent;
			string leftText = null;
			string rightText = null;
			bool informationalText = false;
			if (parentNode != null)
			{
				TestCaseNode parentTestCase = parentNode as TestCaseNode;
				CompareNode compareNode = node as CompareNode;
				if (parentTestCase != null)
				{
					testInfo.Initialize(myReportSuites, parentTestCase);
					string result = testInfo.Result;

					ReportAssembly reportAssembly = GetReportAssembly(testInfo);
					string nodeText = node.Text;
					if (result == MissingReportBaseline)
					{
						if (nodeText == ReportFileName)
						{
							rightText = FindMissingReport(testInfo, reportAssembly, true);
						}
						else if (compareNode.FailState != NodeFailState.Passed)
						{
							FailingCompareNode failingCompare = (FailingCompareNode)compareNode;
							string missingReport = FindMissingReport(testInfo, reportAssembly, false);
							if (failingCompare.MissingBaseline)
							{
								// The full text is in the report file
								using (XmlReader reportReader = XmlReader.Create(new StringReader(missingReport)))
								{
									rightText = FindMissingCompare(reportReader, nodeText);
								}
							}
							else
							{
								// The full text is available by patching with data in the assembly
								leftText = GetExpectedText(testInfo, reportAssembly, nodeText);
								rightText = GetActualCompare(testInfo, reportAssembly, missingReport, nodeText);
							}
						}
						else
						{
							informationalText = true;
							leftText = GetExpectedText(testInfo, reportAssembly, nodeText);
						}
					}
					else if (result == ReportDiffgram)
					{
						if (node.FailState != NodeFailState.Passed)
						{
							FailingCompareNode failingCompare = (FailingCompareNode)node;
							if (failingCompare.MissingBaseline)
							{
								rightText = FindMissingCompare(testInfo, reportAssembly.Assembly, nodeText);
							}
							else
							{
								leftText = GetExpectedText(testInfo, reportAssembly, nodeText);
								string actualText = nodeText == ReportFileName
									? GetFakePassReport(testInfo, reportAssembly)
									: GetActualCompare(testInfo, reportAssembly, null, nodeText);
								if (!string.IsNullOrEmpty(actualText))
								{
									rightText = actualText;
								}
							}
						}
						else
						{
							informationalText = true;
							leftText = GetExpectedText(testInfo, reportAssembly, nodeText);
						}
					}
				}
				else if (null != (parentTestCase = node as TestCaseNode))
				{
					informationalText = true;
					testInfo.Initialize(myReportSuites, parentTestCase);

					ReportAssembly reportAssembly = GetReportAssembly(testInfo);
					//get the expected report
					if (testInfo.Result == TestPassed)
					{
						leftText = GetExpectedText(testInfo, reportAssembly, ReportFileName);
					}
					else
					{
						leftText = ResourceStrings.FailureDetailsAvailableText;
					}
				}
			}
			tbLeft.Text = leftText;
			tbRight.Text = rightText;
			if (leftText != null && !informationalText)
			{
				btnCompare.Enabled = rightText != null;
				btnUpdateBaseline.Enabled = rightText != null;
			}
			else if (rightText != null && !informationalText)
			{
				btnCompare.Enabled = false;
				btnUpdateBaseline.Enabled = true;
			}
			else
			{
				btnCompare.Enabled = false;
				btnUpdateBaseline.Enabled = false;
			}

			// Assembly information is located on level 1
			associatedSolutionToolStripMenuItem.Enabled = node.Level != 0 || node.Nodes.Count == 1;
		}
		private string FindMissingReport(TestInfo testInfo, ReportAssembly reportAssembly, bool fakeComparePass)
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
			if (!fakeComparePass)
			{
				return content;
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

						using (MemoryStream formatStream = new MemoryStream((int)memoryStream.Position))
						{
							memoryStream.Position = 0;
							using (XmlReader formatReader = XmlReader.Create(memoryStream))
							{
								using (XmlWriter formatWriter = XmlWriter.Create(formatStream, writerSettings))
								{
									FormatXml(formatReader, formatWriter);
								}
							}
							formatStream.Position = 0;
							return new StreamReader(formatStream).ReadToEnd();
						}
					}
				}
			}
		}
		private string FindMissingCompare(TestInfo testInfo, Assembly assembly, string compareExtension)
		{
			string baseResourceName = String.Concat(testInfo.TestNamespace, ".", testInfo.TestClassName, ".", testInfo.TestName);
			StringReader diffReader = new StringReader(testInfo.Content);

			using (MemoryStream patchedStream = new MemoryStream())
			{

				using (Stream reportStream = assembly.GetManifestResourceStream(string.Format("{0}.Report.xml", baseResourceName)))
				{
					if (reportStream == null)
					{
						return "Report File Is Missing.";
					}
					using (XmlReader reportReader = XmlReader.Create(reportStream))
					{
						using (XmlReader diffreader = XmlReader.Create(diffReader))
						{
							new XmlPatch().Patch(reportReader, patchedStream, diffreader);
						}
					}
				}
				patchedStream.Position = 0;
				using (XmlReader patchedReader = XmlReader.Create(patchedStream))
				{
					return FindMissingCompare(patchedReader, compareExtension);
				}
			}
		}
		private string FindMissingCompare(XmlReader reportReader, string compareExtension)
		{
			using (MemoryStream memStream = new MemoryStream())
			{
				using (XmlReader innerReader = XmlReader.Create(ExtractStringReaderFromCompareByName(reportReader, compareExtension)))
				{

					XmlWriterSettings writerSettings = new XmlWriterSettings();
					writerSettings.Indent = true;
					writerSettings.IndentChars = "\t";
					using (XmlWriter writer = XmlWriter.Create(memStream, writerSettings))
					{
						FormatXml(innerReader, writer);
					}
				}
				memStream.Position = 0;
				return new StreamReader(memStream).ReadToEnd();
			}
		}
		private ReportAssembly GetReportAssembly(TestInfo testInfo)
		{
			ReportAssembly reportAssembly = new ReportAssembly();
			foreach (ReportSuite reportSuite in myReportSuites)
			{
				if (reportSuite.Name == testInfo.SuiteName)
				{
					foreach (ReportAssembly assembly in reportSuite.Assemblies)
					{
						if (assembly.Location == testInfo.AssemblyLocation)
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
			TestCaseNode parentNode = e.Node as TestCaseNode;
			if (parentNode != null)
			{
				TestInfo testInfo = new TestInfo();
				testInfo.Initialize(myReportSuites, parentNode);

				//clear out the dummy subnode, if present...
				TreeNodeCollection nodes = parentNode.Nodes;
				if (nodes.Count > 0)
				{
					FailStateNode firstNode = (FailStateNode)parentNode.FirstNode;
					if (firstNode is DummyTreeNode)
					{
						nodes.Clear();
						foreach (FailStateNode childNode in GetTestCompareNodes(testInfo))
						{
							nodes.Add(childNode);
							parentNode.NotifyChildAttached(childNode);
						}
						parentNode.NotifyChildDetached(firstNode);
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
		private IEnumerable<FailStateNode> GetTestCompareNodes(TestInfo testInfo)
		{
			string result = testInfo.Result;
			bool missingBaseReport;

			if ((missingBaseReport = result == MissingReportBaseline) || result == ReportDiffgram)
			{
				ReportAssembly reportAssembly = GetReportAssembly(testInfo);

				XmlReaderSettings readerSettings = new XmlReaderSettings();
				readerSettings.CloseInput = true;

				//The nodes returned are based on the Compare nodes found in the result report file.
				//therefore, it is necessary to obtain this report file. 
				Assembly assembly = reportAssembly.Assembly;
				using (Stream reportStream = missingBaseReport ? null : new MemoryStream())
				{
					string rawMissingReport = null;
					if (missingBaseReport)
					{
						rawMissingReport = FindMissingReport(testInfo, reportAssembly, false);
						FailStateNode reportNode = new FailingCompareNode(ReportFileName, true);
						yield return reportNode;
					}
					else
					{
						using (XmlReader reportReader = XmlReader.Create(assembly.GetManifestResourceStream(testInfo.BaseFileName + ".Report.xml"), readerSettings))
						{
							using (XmlReader diffReader = XmlReader.Create(new StringReader(testInfo.Content), readerSettings))
							{
								new XmlPatch().Patch(reportReader, reportStream, diffReader);
								reportStream.Position = 0;
							}
						}

						// Eith reportStream now contains a TestReport XML file with at least 1 Compare node.
						// Check it against a 'faked pass', to determine if the report node is a pass or a failure.
						readerSettings.CloseInput = false;
						using (MemoryStream fakeReportStream = new MemoryStream())
						{
							using (XmlReader reportReader = XmlReader.Create(reportStream, readerSettings))
							{
								XmlWriterSettings writerSettings = new XmlWriterSettings();
								writerSettings.CloseOutput = false;
								using (XmlWriter fakeReportWriter = XmlWriter.Create(fakeReportStream, writerSettings))
								{
									FakePassTransform.Transform(reportReader, fakeReportWriter);
								}
							}
							fakeReportStream.Position = 0;
							reportStream.Position = 0;
							readerSettings.CloseInput = true;
							using (XmlReader reportReader = XmlReader.Create(assembly.GetManifestResourceStream(testInfo.BaseFileName + ".Report.xml"), readerSettings))
							{
								using (XmlReader fakeReportReader = XmlReader.Create(fakeReportStream, readerSettings))
								{
									yield return new XmlDiff(XmlDiffOptions.IgnoreXmlDecl | XmlDiffOptions.IgnorePrefixes | XmlDiffOptions.IgnoreWhitespace | XmlDiffOptions.IgnoreComments | XmlDiffOptions.IgnorePI).Compare(fakeReportReader, reportReader) ?
										new PassingCompareNode(ReportFileName) as FailStateNode :
										new FailingCompareNode(ReportFileName, false);
								}
							}
						}
					}

					//Next, create a treenode object for each compare node in the XML.
					readerSettings.CloseInput = true;
					using (XmlReader testResultReader = missingBaseReport ? XmlReader.Create(new StringReader(rawMissingReport), readerSettings) :  XmlReader.Create(reportStream, readerSettings))
					{
						while (testResultReader.Read())
						{
							if (testResultReader.IsStartElement() && testResultReader.LocalName == "Compare")
							{
								string name = testResultReader.GetAttribute("name");
								string testResult = testResultReader.GetAttribute("result");
								string testName = string.Format("Compare{0}.orm", name == null ? "" : "." + name);
								yield return testResult == TestPassed ?
									new PassingCompareNode(testName) as FailStateNode :
									new FailingCompareNode(testName, testResult == MissingBaseline);
							}
						}
					}
				}
			}
		}
		#endregion
		#region getTempFilePath functions
		//these functions use the current FileInfo object to create
		//temporary files. The testInfo object must be set before they are called.


		private string GetActualCompare(TestInfo testInfo, ReportAssembly reportAssembly, string missingReport, string extension)
		{
			Assembly assembly = reportAssembly.Assembly;

			foreach (TestClass testclass in reportAssembly.TestClasses)
			{
				if (testclass.Name == testInfo.TestClassName)
				{
					foreach (Test test in testclass.Tests)
					{
						if (test.Name == testInfo.TestName && testclass.TestNamespace == testInfo.TestNamespace)
						{
							string result = test.Result;
							string baseResourceName = testInfo.BaseFileName;
							XmlReaderSettings readerSettings = new XmlReaderSettings();

							if (result == ReportDiffgram || missingReport != null)
							{
								using (Stream reportStream = missingReport != null ? null : assembly.GetManifestResourceStream(baseResourceName + ".Report.xml"))
								{
									if (reportStream != null || missingReport != null)
									{
										XmlPatch patcher = new XmlPatch();
										using (XmlReader reportReader = missingReport != null ?
											XmlReader.Create(new StringReader(missingReport)) :
											XmlReader.Create(reportStream, readerSettings))
										{
											StringReader compareStringReader;
											if (missingReport != null)
											{
												compareStringReader = ExtractStringReaderFromCompareByName(reportReader, extension);
											}
											else
											{
												using (XmlReader diffReader = XmlReader.Create(new StringReader(test.Content), readerSettings))
												{
													using (MemoryStream compareStream = new MemoryStream())
													{
														patcher.Patch(reportReader, new UncloseableStream(compareStream), diffReader);
														compareStream.Position = 0;
														//the filestream is now the expected test report, containing a compare. 
														//Now to get that compare...

														compareStringReader = ExtractStringReaderFromCompareByName(XmlReader.Create(compareStream), extension);
													}
												}
											}

											//and apply the diffgram within in to the original Compare.orm
											using (Stream compareStream = assembly.GetManifestResourceStream(string.Format("{0}.{1}", baseResourceName, extension)))
											{
												if (compareStream != null)
												{
													using (XmlReader compareReader = XmlReader.Create(compareStream, readerSettings))
													{
														using (MemoryStream patchedCompareStream = new MemoryStream())
														{
															using (XmlReader reportCompareReader = XmlReader.Create(compareStringReader, readerSettings))
															{
																//an exception thrown here is generally caused by the report suite being outdated. 
																patcher.Patch(compareReader, new UncloseableStream(patchedCompareStream), reportCompareReader);
															}
															patchedCompareStream.Position = 0;
															using (MemoryStream patchedOutputStream = new MemoryStream())
															{
																using (XmlReader reader = XmlReader.Create(patchedCompareStream))
																{
																	XmlWriterSettings writerSettings = new XmlWriterSettings();
																	writerSettings.Indent = true;
																	writerSettings.IndentChars = "\t";
																	writerSettings.CloseOutput = false;
																	using (XmlWriter writer = XmlWriter.Create(patchedOutputStream, writerSettings))
																	{
																		FormatXml(reader, writer);
																	}
																}
																patchedOutputStream.Position = 0;
																return new StreamReader(patchedOutputStream).ReadToEnd();
															}
														}
													}
												}
											}
										}
									}
								}
							}
							return null;
						}
					}
				}
			}
			Debug.Fail("Internal Error. function: GetActualCompare.");
			return null;
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
					retval = FindMissingCompare(testInfo, assembly, extension);
				}
			}
			return retval;
		}



		private string GetFakePassReport(TestInfo testInfo, ReportAssembly reportAssembly)
		{
			Assembly assembly = reportAssembly.Assembly;
			XslCompiledTransform transform = FakePassTransform;
			XmlReaderSettings readerSettings = new XmlReaderSettings();

			string baseResourceName = String.Concat(testInfo.TestNamespace, ".", testInfo.TestClassName, ".", testInfo.TestName);
			foreach (TestClass testclass in reportAssembly.TestClasses)
			{
				if (testclass.Name == testInfo.TestClassName)
				{
					foreach (Test test in testclass.Tests)
					{
						if (test.Name == testInfo.TestName && testclass.TestNamespace == testInfo.TestNamespace)
						{
							MemoryStream patchedStream = null;
							try
							{
								using (Stream stream = assembly.GetManifestResourceStream(baseResourceName + ".Report.xml"))
								{
									if (stream != null)
									{
										using (XmlReader reportReader = XmlReader.Create(stream, readerSettings))
										{
											using (XmlReader diffReader = XmlReader.Create(new StringReader(test.Content), readerSettings))
											{
												patchedStream = new MemoryStream();
												new XmlPatch().Patch(reportReader, new UncloseableStream(patchedStream), diffReader);
											}
										}
									}
									else
									{
										return null;
									}
								}
								patchedStream.Position = 0;
								using (XmlReader failReader = XmlReader.Create(patchedStream, readerSettings))
								{
									using (MemoryStream fakePassStream = new MemoryStream())
									{
										XmlWriterSettings writerSettings = new XmlWriterSettings();
										writerSettings.Indent = true;
										writerSettings.IndentChars = "\t";
										writerSettings.CloseOutput = false;
										using (XmlWriter fakePassWriter = XmlWriter.Create(fakePassStream, writerSettings))
										{
											transform.Transform(failReader, fakePassWriter);
										}
										fakePassStream.Position = 0;
										return new StreamReader(fakePassStream).ReadToEnd();
									}
								}
							}
							finally
							{
								if (patchedStream != null)
								{
									patchedStream.Dispose();
								}
							}
						}
					}
				}
			}
			return null;
		}


		private StringReader ExtractStringReaderFromCompareByName(XmlReader reader, string compareExtension)
		{
			string[] split = compareExtension.Split('.');
			string name = split.Length == 3 ? split[1] : null;
			//change an empty name to a null, for the comparison with the getAttribute function later

			reader.MoveToContent();
			while (reader.Read())
			{
				if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "Compare")
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
			Control startActive = ActiveControl;
			string baselineFile = Path.GetTempFileName();
			string resultFile = Path.GetTempFileName();

			File.WriteAllText(baselineFile, tbLeft.Text);
			File.WriteAllText(resultFile, tbRight.Text);

			System.Diagnostics.Process proc = new System.Diagnostics.Process();
			ProcessStartInfo startInfo = proc.StartInfo;

			// Start the process
			//@"C:\Program Files\Microsoft Visual Studio 8\Common7\Tools\Bin\WinDiff.exe";
			startInfo.FileName = Settings.Default.DiffProgram;
			startInfo.Arguments = string.Format(@"""{0}"" ""{1}""", baselineFile, resultFile);
			FormWindowState startWindowState = WindowState;
			startInfo.WindowStyle = (startWindowState == FormWindowState.Maximized) ? ProcessWindowStyle.Maximized : ProcessWindowStyle.Normal;

			bool reshell = true;
			while (reshell)
			{
				try
				{
					reshell = false;
					proc.Start();
					proc.WaitForInputIdle();
					break;
				}
				catch (Win32Exception)
				{
					reshell = true;
				}
				if (reshell)
				{
					if (ResetCompareApplication())
					{
						startInfo.FileName = Settings.Default.DiffProgram;
						continue;
					}
				}
				// We'll only get here is something went wrong with the
				// selected app and the user cancels the dialog.
				File.Delete(baselineFile);
				File.Delete(resultFile);
				return;
			}

			Visible = false;
			WindowState = FormWindowState.Minimized;
			proc.WaitForExit();

			// Delete the temp files
			File.Delete(baselineFile);
			File.Delete(resultFile);

			// Reactivate this application
			Visible = true;
			WindowState = startWindowState;
			Activate();
			if (startActive != null)
			{
				startActive.Focus();
			}
		}

		private void btnUpdateBaseline_Click(object sender, EventArgs e)
		{
			FailingCompareNode currentNode = reportTreeView.SelectedNode as FailingCompareNode;
			if (currentNode == null)
			{
				MessageBox.Show("Please make sure the correct item in the tree view to the left is selected.");
				return;
			}
			
			string extension = currentNode.Text;
			TestInfo testInfo = new TestInfo();
			testInfo.Initialize(myReportSuites, (TestCaseNode)currentNode.Parent);
			string newText = tbRight.Text;
			if (newText.Length < 2)
			{
				MessageBox.Show("Textbox cannot be empty.");
				return;
			}
			
			string assemblyLocation = myBaseDirectory + testInfo.AssemblyLocation;
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
					solution = MapAssemblyToSolution(assemblyLocation, solution, true, false);
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
					MapAssemblyToSolution(assemblyLocation, "", false, false);
					solution = "";
				}
			}

			string fileDirectory = testInfo.TestNamespace;
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
			string testClassName = testInfo.TestClassName;
			string testName = testInfo.TestName;

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

			ProjectItem existingProjectItem = ProjectContains(project.ProjectItems, fileName);
			if (existingProjectItem != null)
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
				SetFileProperties(existingProjectItem);
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
			currentNode.IsRepaired = true;
			return;
		}


		#region replace baseline file helper methods
		private string MapAssemblyToSolution(string assemblyLocation, string solution, bool allowInteraction, bool forceInteraction)
		{
			if ((forceInteraction || (allowInteraction && solution.Length == 0)) || !File.Exists(solution))
			{
				//request solution for the given assembly from the user
				FileInfo assemblyInfo = new FileInfo(assemblyLocation);
				solutionFileDialog.Title = "Solution file for " + assemblyInfo.Directory.Name + "\\" + assemblyInfo.Name;

				if (!forceInteraction || (!string.IsNullOrEmpty(solution) && !File.Exists(solution)))
				{
					solution = "";
				}
				if (!string.IsNullOrEmpty(solution))
				{
					solutionFileDialog.FileName = solution;
				}
				else
				{
					solutionFileDialog.InitialDirectory = assemblyInfo.DirectoryName;
				}
				if (solutionFileDialog.ShowDialog() == DialogResult.OK)
				{
					solution = solutionFileDialog.FileName;
				}
				else if (forceInteraction)
				{
					return "";
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
				SetFileProperties(newItem);
			}
		}
		private static void SetFileProperties(ProjectItem item)
		{
			Properties properties = item.Properties;
			properties.Item("BuildAction").Value = 3;   // 3 == VSLangProj.prjBuildAction.prjBuildActionEmbeddedResource
			properties.Item("CustomTool").Value = "";
		}
		private static ProjectItem ProjectContains(ProjectItems items, string fileName)
		{
			foreach (ProjectItem testItem in items)
			{
				if (fileName == testItem.Name)
				{
					return testItem;
				}
				ProjectItem retVal = ProjectContains(testItem.ProjectItems, fileName);
				if (retVal != null)
				{
					return retVal;
				}
			}
			return null;
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportViewer));
			this.tbLeft = new System.Windows.Forms.RichTextBox();
			this.tbRight = new System.Windows.Forms.RichTextBox();
			this.reportTreeView = new System.Windows.Forms.TreeView();
			this.splitContainerExpectedActual = new System.Windows.Forms.SplitContainer();
			this.labelExpected = new System.Windows.Forms.Label();
			this.labelActual = new System.Windows.Forms.Label();
			this.btnUpdateBaseline = new System.Windows.Forms.Button();
			this.btnCompare = new System.Windows.Forms.Button();
			this.solutionFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.reportViewerMenu = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.reportSuiteFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.splitContainerOuter = new System.Windows.Forms.SplitContainer();
			this.labelTreeView = new System.Windows.Forms.Label();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolTips = new System.Windows.Forms.ToolTip(this.components);
			this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.associatedSolutionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.compareApplicationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.compareApplicationFileDialog = new System.Windows.Forms.OpenFileDialog();
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
			this.tbLeft.Size = new System.Drawing.Size(367, 631);
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
			this.tbRight.Location = new System.Drawing.Point(0, 16);
			this.tbRight.Name = "tbRight";
			this.tbRight.Size = new System.Drawing.Size(367, 631);
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
			this.reportTreeView.Size = new System.Drawing.Size(313, 671);
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
			// 
			// splitContainerExpectedActual.Panel2
			// 
			this.splitContainerExpectedActual.Panel2.Controls.Add(this.labelActual);
			this.splitContainerExpectedActual.Panel2.Controls.Add(this.tbRight);
			this.splitContainerExpectedActual.Size = new System.Drawing.Size(739, 650);
			this.splitContainerExpectedActual.SplitterDistance = 369;
			this.splitContainerExpectedActual.SplitterWidth = 3;
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
			// labelActual
			// 
			this.labelActual.AutoSize = true;
			this.labelActual.Location = new System.Drawing.Point(3, 3);
			this.labelActual.Name = "labelActual";
			this.labelActual.Size = new System.Drawing.Size(40, 13);
			this.labelActual.TabIndex = 4;
			this.labelActual.Text = "&Actual:";
			// 
			// btnUpdateBaseline
			// 
			this.btnUpdateBaseline.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnUpdateBaseline.Location = new System.Drawing.Point(625, 656);
			this.btnUpdateBaseline.Name = "btnUpdateBaseline";
			this.btnUpdateBaseline.Size = new System.Drawing.Size(104, 25);
			this.btnUpdateBaseline.TabIndex = 7;
			this.btnUpdateBaseline.Text = "Update &Baseline";
			this.toolTips.SetToolTip(this.btnUpdateBaseline, "Update project baseline files to the Actual contents (Ctrl-B)");
			this.btnUpdateBaseline.UseVisualStyleBackColor = true;
			this.btnUpdateBaseline.Click += new System.EventHandler(this.btnUpdateBaseline_Click);
			// 
			// btnCompare
			// 
			this.btnCompare.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCompare.Location = new System.Drawing.Point(510, 656);
			this.btnCompare.Name = "btnCompare";
			this.btnCompare.Size = new System.Drawing.Size(104, 23);
			this.btnCompare.TabIndex = 6;
			this.btnCompare.Text = "Co&mpare";
			this.toolTips.SetToolTip(this.btnCompare, "Launch an application to compare files (Ctrl-M)");
			this.btnCompare.UseVisualStyleBackColor = true;
			this.btnCompare.Click += new System.EventHandler(this.btnCompare_Click);
			// 
			// solutionFileDialog
			// 
			this.solutionFileDialog.Filter = "Solution File (*.sln)|*.sln|All Files|*.*";
			// 
			// reportViewerMenu
			// 
			this.reportViewerMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem});
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
            this.clearToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
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
			this.clearToolStripMenuItem.Text = "&Close";
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
			// 
			// splitContainerOuter.Panel2
			// 
			this.splitContainerOuter.Panel2.Controls.Add(this.splitContainerExpectedActual);
			this.splitContainerOuter.Panel2.Controls.Add(this.btnUpdateBaseline);
			this.splitContainerOuter.Panel2.Controls.Add(this.btnCompare);
			this.splitContainerOuter.Size = new System.Drawing.Size(1061, 687);
			this.splitContainerOuter.SplitterDistance = 313;
			this.splitContainerOuter.SplitterWidth = 3;
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
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(149, 6);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.exitToolStripMenuItem.Text = "E&xit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// optionsToolStripMenuItem
			// 
			this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.compareApplicationToolStripMenuItem,
            this.associatedSolutionToolStripMenuItem});
			this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
			this.optionsToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
			this.optionsToolStripMenuItem.Text = "&Options";
			// 
			// associatedSolutionToolStripMenuItem
			// 
			this.associatedSolutionToolStripMenuItem.Enabled = false;
			this.associatedSolutionToolStripMenuItem.Name = "associatedSolutionToolStripMenuItem";
			this.associatedSolutionToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
			this.associatedSolutionToolStripMenuItem.Text = "&Associated Solution...";
			this.associatedSolutionToolStripMenuItem.ToolTipText = "Specify implementing solution file for selected test assembly.";
			this.associatedSolutionToolStripMenuItem.Click += new System.EventHandler(this.associatedSolutionToolStripMenuItem_Click);
			// 
			// compareApplicationToolStripMenuItem
			// 
			this.compareApplicationToolStripMenuItem.Name = "compareApplicationToolStripMenuItem";
			this.compareApplicationToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
			this.compareApplicationToolStripMenuItem.Text = "&Compare Application...";
			this.compareApplicationToolStripMenuItem.ToolTipText = "Specify the application used to compare files.";
			this.compareApplicationToolStripMenuItem.Click += new System.EventHandler(this.compareApplicationToolStripMenuItem_Click);
			// 
			// compareApplicationFileDialog
			// 
			this.compareApplicationFileDialog.DefaultExt = "exe";
			this.compareApplicationFileDialog.Filter = "Compare Application (*.exe)|*.exe|All files|*.*";
			this.compareApplicationFileDialog.Title = "Select Compare Application";
			// 
			// ReportViewer
			// 
			this.ClientSize = new System.Drawing.Size(1061, 714);
			this.Controls.Add(this.splitContainerOuter);
			this.Controls.Add(this.reportViewerMenu);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.MainMenuStrip = this.reportViewerMenu;
			this.Name = "ReportViewer";
			this.Text = "ReportViewer";
			this.Load += new System.EventHandler(this.ReportViewer_Load);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ReportViewer_KeyDown);
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
				clearToolStripMenuItem_Click(null, null);
				SetReportFile(reportSuiteFileDialog.FileName);
				reportTreeView.Select();
			}
		}

		private void clearToolStripMenuItem_Click(object sender, EventArgs e)
		{
			reportTreeView.Nodes.Clear();
			tbLeft.Clear();
			tbRight.Clear();
			btnCompare.Enabled = false;
			btnUpdateBaseline.Enabled = false;
			associatedSolutionToolStripMenuItem.Enabled = false;
		}
		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}
		private void associatedSolutionToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// Find the assembly information from the tree node at level 1
			TreeNode node = reportTreeView.SelectedNode;
			if (node != null)
			{
				int nodeLevel = node.Level;
				if (nodeLevel == 0)
				{
					TreeNodeCollection childNodes = node.Nodes;
					if (childNodes.Count != 1)
					{
						return;
					}
					node = childNodes[0];
					nodeLevel = 1;
				}
				while (nodeLevel > 1)
				{
					node = node.Parent;
					--nodeLevel;
				}
				string assemblyLocation = myBaseDirectory + node.Text;
				string existingSolution = LookupSolution(assemblyLocation);
				MapAssemblyToSolution(assemblyLocation, existingSolution, true, true);
			}
		}
		private void compareApplicationToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ResetCompareApplication();
		}
		private bool ResetCompareApplication()
		{
			string programToShellTo = Settings.Default.DiffProgram;
			if (File.Exists(programToShellTo))
			{
				compareApplicationFileDialog.FileName = programToShellTo;
			}
			else
			{
				compareApplicationFileDialog.FileName = "";
				compareApplicationFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
			}
			if (compareApplicationFileDialog.ShowDialog() == DialogResult.OK)
			{
				Settings.Default.DiffProgram = compareApplicationFileDialog.FileName;
				Settings.Default.Save();
				return true;
			}
			return false;
		}
		#endregion Menu options
	}
}