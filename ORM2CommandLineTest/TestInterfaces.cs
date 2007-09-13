using System;
using System.Collections.Generic;
using System.Xml;
using System.Reflection;
using Microsoft.VisualStudio.Modeling;
namespace Neumont.Tools.ORM.SDK.TestEngine
{
	#region IORMToolTestServices Interface
	/// <summary>
	/// Interface for services offered by the testing engine.
	/// An implementation of this interface can be retrieved
	/// from the IORMToolServices.ServiceProvider.GetService method
	/// for the IORMToolServices passed to the testclass constructor.
	/// </summary>
	public interface IORMToolTestServices
	{
		/// <summary>
		/// Load an .orm file from the test assembly. The file
		/// must be in the same directory as the test class
		/// and included as an embedded resource in the assembly.
		/// The file must be called class.method.Load[.referenceName].orm
		/// </summary>
		/// <param name="testMethod">The method used to execute
		/// the test. Use (MethodInfo)System.Reflection.MethodInfo.GetCurrentMethod()
		/// from an executing test procedure.</param>
		/// <param name="referenceName">An extra name used
		/// to load multiple files during the same test. If
		/// referenceName is not specified and the resource
		/// stream cannot be found, then a new Store is created and
		/// the default 'ORM Model File' template is loaded into the
		/// store.</param>
		/// <param name="extensions">An optional list of <see cref="SuiteExtension"/>s to support with the load.</param>
		/// <returns>Newly created store with the file loaded into it.</returns>
		Store Load(MethodInfo testMethod, string referenceName, IList<SuiteExtension> extensions);
		/// <summary>
		/// Compare an .orm file to the current contents of the
		/// specified store. This involves saving the current
		/// contents and comparing the current and expected results.
		/// The file must be in the same directory as the test class
		/// and included as an embedded resource in the assembly.
		/// The file must be called class.method.Compare[.referenceName].orm
		/// </summary>
		/// <param name="store">A loaded store to serialize. Should be
		/// the store returned by a call to Load.</param>
		/// <param name="testMethod">The method used to execute
		/// the test. Use (MethodInfo)System.Reflection.MethodInfo.GetCurrentMethod()
		/// from an executing test procedure.</param>
		/// <param name="referenceName">An extra name used
		/// to load multiple files during the same test. If the
		/// resource name is not found then no comparison is made.</param>
		void Compare(Store store, MethodInfo testMethod, string referenceName);
		/// <summary>
		/// Write the current set of validation errors. Called automatically
		/// when a test is completed after the automatic Compare call when
		/// a test completes.
		/// </summary>
		/// <param name="referenceName">An extra name written to the
		/// test report for tracking purposes. If a reference name is
		/// provided then a report is always written, even if no
		/// validation errors are currently present.</param>
		void LogValidationErrors(string referenceName);
		/// <summary>
		/// Write a custom message to the test report
		/// </summary>
		/// <param name="message">The message to write</param>
		void LogMessage(string message);
		/// <summary>
		/// Log a thrown exception
		/// </summary>
		/// <param name="exception">The exception object to log and report</param>
		void LogException(Exception exception);
		/// <summary>
		/// Begin a new test report. An existing report that
		/// has not been closed is lost. Used by the test engine
		/// only, should not be called by a test.
		/// </summary>
		void OpenReport();
		/// <summary>
		/// Close the current report (opened with OpenReport). Used by the
		/// test engine only, should not be called by a test
		/// </summary>
		/// <param name="testMethod">The method used to execute
		/// the test. Use (MethodInfo)System.Reflection.MethodInfo.GetCurrentMethod()
		/// from an executing test procedure.</param>
		/// <returns>An XmlReader instance containing the difference between the expected
		/// report and the actual report for this test method. A file of the name
		/// testclass.method.Report.xml must be included in the test assembly as an
		/// embedded resource in the same directory as the test class. If this file is not
		/// present, then an empty report (a TestReport node with no contents) is used
		/// as the base.</returns>
		XmlReader CloseReport(MethodInfo testMethod);
	}
	#endregion // IORMToolTestServices Interface
	#region ORMTestResult enum
	/// <summary>
	/// The possible results of a test run by the test engine
	/// </summary>
	public enum ORMTestResult
	{
		/// <summary>
		/// The test passed. The test method executed, a baseline report
		/// was provided, and the test report matched the baseline.
		/// </summary>
		Pass,
		/// <summary>
		/// The test method could not be executed
		/// </summary>
		FailBind,
		/// <summary>
		/// The test method was executed, but a baseline report was not provided.
		/// </summary>
		FailReportBaseline,
		/// <summary>
		/// The test method executed and a baseline report was provided, but the
		/// actual report is different than the baseline.
		/// </summary>
		FailReportDiffgram,
	}
	#endregion // ORMTestResult enum
	#region ORMSuiteReportResult enum
	/// <summary>
	/// An enum to report the different kinds of failures
	/// encountered during a suite run. Returned from
	/// IORMToolTestSuiteReport.CloseSuiteReport
	/// </summary>
	[Flags]
	public enum ORMSuiteReportResult
	{
		/// <summary>
		/// No failures were reported
		/// </summary>
		NoFailure = 0,
		/// <summary>
		/// One or more assemblies failed to load
		/// </summary>
		AssemblyLoadFailure = 1,
		/// <summary>
		/// One or more classes with a Tests attribute
		/// could not be created
		/// </summary>
		ClassLoadFailure = 2,
		/// <summary>
		/// One or more methods with a Test attribute did not pass
		/// </summary>
		TestFailure = 4,
	}
	#endregion // ORMSuiteReportResult enum
	#region IORMToolTestSuiteReport Interface
	/// <summary>
	/// An interface for writing a suite report
	/// </summary>
	public interface IORMToolTestSuiteReport
	{
		/// <summary>
		/// Begin a new suite. The element is always
		/// written, even if no children are present
		/// </summary>
		/// <param name="suiteName">The name of the suite (required)</param>
		void BeginSuite(string suiteName);
		/// <summary>
		/// Begin tests in the new assembly. The element is not written
		/// if no contained tests are run, unless a loadFailure is specified.
		/// </summary>
		/// <param name="path">The relative path from the
		/// suite file to the test assembly.</param>
		/// <param name="loadFailure">True if the test assembly failed to load.</param>
		void BeginTestAssembly(string path, bool loadFailure);
		/// <summary>
		/// Begin a new test class. This element is always written, but
		/// is generally called only when the first test method in the class
		/// is positively identified. A test class must have a public constructor
		/// with a single IORMToolServices parameter to be a valid test class.
		/// </summary>
		/// <param name="testClassNamespace">The class namespace</param>
		/// <param name="testClassName">The class name</param>
		/// <param name="loadFailure">true if the class failed to load</param>
		void BeginTestClass(string testClassNamespace, string testClassName, bool loadFailure);
		/// <summary>
		/// Report the completion of a test. A test with a diffReport
		/// is considered a failure. A test with no diffReport is a pass.
		/// </summary>
		/// <param name="testMethodName">The method name of the test</param>
		/// <param name="result">Result of the running the test. The contents of the report
		/// parameter change depending on this value</param>
		/// <param name="report">A reader containing the xml for the report.
		/// If provided, the reader will be closed automatically by this method.</param>
		void ReportTestResults(string testMethodName, ORMTestResult result, XmlReader report);
		/// <summary>
		/// Close all elements and return any encountered failures
		/// </summary>
		ORMSuiteReportResult CloseSuiteReport();
	}
	#endregion // IORMToolTestSuiteReport Interface
	#region IORMToolTestSuiteReportFactory Interface
	/// <summary>
	/// An interface for creation of a test suite report.
	/// An implementation of this interface can be retrieved
	/// from the IORMToolServices.ServiceProvide.GetService method
	/// for the IORMToolServices passed to the testclass constructor.
	/// </summary>
	public interface IORMToolTestSuiteReportFactory
	{
		/// <summary>
		/// Create a test suite report on the specified
		/// xml writer.
		/// </summary>
		/// <param name="writer">The xml writer for output</param>
		/// <returns>New implementation of IORMToolTestSuiteReport</returns>
		IORMToolTestSuiteReport Create(XmlWriter writer);
	}
	#endregion // IORMToolTestSuiteReportFactory Interface
}
