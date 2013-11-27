#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
* Copyright © ORM Solutions, LLC. All rights reserved.                     *
*                                                                          *
* The use and distribution terms for this software are covered by the      *
* Common Public License 1.0 (http://opensource.org/licenses/cpl) which     *
* can be found in the file CPL.txt at the root of this distribution.       *
* By using this software in any fashion, you are agreeing to be bound by   *
* the terms of this license.                                               *
*                                                                          *
* You must not remove this notice, or any other, from this software.       *
\**************************************************************************/
#endregion

using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Designer.Interfaces;
using Microsoft.VisualStudio.TextManager.Interop;
using VSLangProj;

using Debug = System.Diagnostics.Debug;
using IServiceProvider = System.IServiceProvider;
using DebuggerStepThroughAttribute = System.Diagnostics.DebuggerStepThroughAttribute;
using VSConstants = Microsoft.VisualStudio.VSConstants;
using ErrorHandler = Microsoft.VisualStudio.ErrorHandler;
using IOleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;
using IVsTextLines = Microsoft.VisualStudio.TextManager.Interop.IVsTextLines;
using IVsTextView = Microsoft.VisualStudio.TextManager.Interop.IVsTextView;
#if VISUALSTUDIO_10_0
using Microsoft.Build.Construction;
#else
using Microsoft.Build.BuildEngine;
#endif

namespace ORMSolutions.ORMArchitect.ORMCustomTool
{
	#region ORMCustomTool class
	/// <summary>
	/// <see cref="ORMCustomTool"/> coordinates generation activities between various <see cref="IORMGenerator"/>s, and
	/// interfaces with Visual Studio and other tools.
	/// </summary>
	[Guid("977BD01E-F2B4-4341-9C47-459420624A21")]
	public sealed partial class ORMCustomTool : IVsSingleFileGenerator, IObjectWithSite, IOleServiceProvider, IServiceProvider
	{
		#region Private Constants
		private const string DEFAULT_EXTENSION_DECORATOR = "._ORMCustomToolReport."; // Add the _ here to put it above other generators
		private const string EXTENSION_ORM = ".orm";
		private const string EXTENSION_XML = ".xml";
#if VISUALSTUDIO_12_0
		private const string GENERATORS_REGISTRYROOT = @"Software\ORM Solutions\Natural ORM Architect for Visual Studio 2013\Generators";
#elif VISUALSTUDIO_11_0
		private const string GENERATORS_REGISTRYROOT = @"Software\ORM Solutions\Natural ORM Architect for Visual Studio 2012\Generators";
#elif VISUALSTUDIO_10_0
		private const string GENERATORS_REGISTRYROOT = @"Software\ORM Solutions\Natural ORM Architect for Visual Studio 2010\Generators";
#elif VISUALSTUDIO_9_0
		private const string GENERATORS_REGISTRYROOT = @"Software\ORM Solutions\Natural ORM Architect for Visual Studio 2008\Generators";
#else // VISUALSTUDIO_8_0
		private const string GENERATORS_REGISTRYROOT = @"Software\ORM Solutions\Natural ORM Architect for Visual Studio 2005\Generators";
#endif
		private const string ITEMMETADATA_DEPENDENTUPON = "DependentUpon";
		private const string ITEMMETADATA_GENERATOR = "Generator";
		private const string ITEMMETADATA_ORMGENERATOR = "ORMGenerator";
		private const string ITEMMETADATA_AUTOGEN = "AutoGen";
		private const string ITEMMETADATA_DESIGNTIME = "DesignTime";
		private const string ITEMGROUP_CONDITIONSTART = "Exists('";
		private const string ITEMGROUP_CONDITIONEND = "')";
		private const string DEBUG_ERROR_CATEGORY = "ORMCustomTool";
		private const string ORM_OUTPUT_FORMAT = "ORM";
		#endregion // Private Constants
		#region Member Variables
		/// <summary>
		/// A wrapper object to provide unified managed and unmanaged IServiceProvider implementations
		/// </summary>
		private readonly ServiceProvider _serviceProvider;
		/// <summary>
		/// The service provider handed us by the shell during IObjectWithSite.SetSite. This
		/// service provider lets us retrieve the EnvDTE.ProjectItem and CodeDomProvider objects and very little else.
		/// </summary>
		private IOleServiceProvider _customToolServiceProvider;
		/// <summary>
		/// The full VS DTE service provider. We retrieve this on demand only
		/// </summary>
		private IOleServiceProvider _dteServiceProvider;
		private CodeDomProvider _codeDomProvider;
		#endregion // Member Variables
		#region Constructors
		/// <summary>
		/// Instantiates a new instance of <see cref="ORMCustomTool"/>.
		/// </summary>
		public ORMCustomTool()
		{
			// NOTE: Attempting to use any of the ServiceProviders will cause us to go into an infinite loop
			// unless SetSite has been called on us.
			this._serviceProvider = new ServiceProvider(this, true);
		}
		#endregion // Constructors
		#region Private Helper Methods
		private static void ReportError(string message, Exception ex)
		{
			ReportError(message, DEBUG_ERROR_CATEGORY, ex);
		}
		private static void ReportError(string message, string category, Exception ex)
		{
			Debug.WriteLine(message, category);
			Debug.Indent();
			Debug.WriteLine(ex);
			Debug.Unindent();
		}
#if VISUALSTUDIO_10_0
		/// <summary>
		/// Retrieve an item group for the specified project and item
		/// </summary>
		public static ProjectItemGroupElement GetItemGroup(ProjectRootElement rootElement, string projectItemName)
		{
			foreach (ProjectItemGroupElement itemGroup in rootElement.ItemGroups)
			{
				// We don't want to process BuildItemGroups that are from outside this project
				if (String.Equals(itemGroup.Condition.Trim(), string.Concat(ITEMGROUP_CONDITIONSTART, projectItemName, ITEMGROUP_CONDITIONEND), StringComparison.OrdinalIgnoreCase))
				{
					return itemGroup;
				}
			}
			return null;
		}
#else // VISUALSTUDIO_10_0
		/// <summary>
		/// Retrieve a build item group for the specified project and item
		/// </summary>
		public static BuildItemGroup GetItemGroup(Project project, string projectItemName)
		{
			foreach (BuildItemGroup buildItemGroup in project.ItemGroups)
			{
				// We don't want to process BuildItemGroups that are from outside this project
				if (!buildItemGroup.IsImported && String.Equals(buildItemGroup.Condition.Trim(), string.Concat(ITEMGROUP_CONDITIONSTART, projectItemName, ITEMGROUP_CONDITIONEND), StringComparison.OrdinalIgnoreCase))
				{
					return buildItemGroup;
				}
			}
			return null;
		}
#endif // VISUALSTUDIO_10_0

		/// <summary>
		/// Get the IVsTextLines from the DocData for the current document.
		/// </summary>
		/// <param name="fullPath">The full file path to the document</param>
		/// <param name="reloadRequired">If this returns true, then the text lines for the
		/// document are different than the text lines for the displayed document view. The
		/// textlines should be reloaded to force the view to update</param>
		/// <returns>IVsTextLines, or null</returns>
		private IVsTextLines GetTextLinesForDocument(string fullPath, out bool reloadRequired)
		{
			IVsTextLines textLines = null;
			IVsRunningDocumentTable rdt = GetService<IVsRunningDocumentTable>();
			reloadRequired = false;
			if (rdt != null)
			{
				IntPtr punkDocData = IntPtr.Zero;
				try
				{
					uint itemId;
					uint docCookie;
					IVsHierarchy hierarchy;
					if (ErrorHandler.Succeeded(rdt.FindAndLockDocument(0, fullPath, out hierarchy, out itemId, out punkDocData, out docCookie)) &&
						punkDocData != IntPtr.Zero)
					{
						object docData = Marshal.GetObjectForIUnknown(punkDocData);
						textLines = docData as IVsTextLines;
						if (textLines == null)
						{
							IVsTextBufferProvider bufferProvider = docData as IVsTextBufferProvider;
							if (null != bufferProvider)
							{
								bufferProvider.GetTextBuffer(out textLines);
							}
						}

						// See if the docview gives us the same text lines
						reloadRequired = true;
						IVsUIShellOpenDocument shellDoc;
						Guid logicalView = Guid.Empty;
						IVsUIHierarchy uiHierarchy;
						IVsWindowFrame windowFrame;
						object viewObject;
						uint[] itemIdOpen = new uint[] { itemId };
						int fOpen;
						if (null != textLines &&
							null != (shellDoc = GetService<IVsUIShellOpenDocument>()) &&
							ErrorHandler.Succeeded(shellDoc.IsDocumentOpen((IVsUIHierarchy)hierarchy, itemId, fullPath, ref logicalView, (uint)__VSIDOFLAGS.IDO_IgnoreLogicalView, out uiHierarchy, itemIdOpen, out windowFrame, out fOpen)) &&
							null != windowFrame &&
							ErrorHandler.Succeeded(windowFrame.GetProperty((int)__VSFPROPID.VSFPROPID_DocView, out viewObject)) &&
							viewObject != null)
						{
							IVsTextLines docViewTextLines = null;
							IVsTextView textView = viewObject as IVsTextView;
							if (textView == null || ErrorHandler.Failed(textView.GetBuffer(out docViewTextLines)) || docViewTextLines == null)
							{
								IVsCodeWindow codeWindow = viewObject as IVsCodeWindow;
								if (codeWindow != null)
								{
									if (ErrorHandler.Failed(codeWindow.GetBuffer(out docViewTextLines)) || docViewTextLines == null)
									{
										if (ErrorHandler.Failed(codeWindow.GetPrimaryView(out textView)) ||
											textView == null ||
											ErrorHandler.Failed(textView.GetBuffer(out docViewTextLines)) ||
											docViewTextLines == null)
										{
											if (ErrorHandler.Succeeded(codeWindow.GetSecondaryView(out textView)) &&
												textView != null)
											{
												textView.GetBuffer(out docViewTextLines);
											}
										}
									}
								}
							}
							reloadRequired = !object.ReferenceEquals(textLines, docViewTextLines);
						}
					}
				}
				finally
				{
					if (punkDocData != IntPtr.Zero)
					{
						Marshal.Release(punkDocData);
					}
				}
			}
			return textLines;
		}

		private CodeDomProvider CodeDomProvider
		{
			get
			{
				CodeDomProvider retVal = _codeDomProvider;
				if (retVal == null)
				{
					retVal = (CodeDomProvider)GetService<IVSMDCodeDomProvider>().CodeDomProvider;
					_codeDomProvider = retVal;
				}
				return retVal;
			}
		}
		#endregion // Private Helper Methods
		#region ServiceProvider Interface Implementations
		/// <summary>
		/// Returns a service instance of type <typeparamref name="T"/>, or <see langword="null"/> if no service instance of
		/// type <typeparamref name="T"/> is available.
		/// </summary>
		/// <typeparam name="T">The type of the service instance being requested.</typeparam>
		public T GetService<T>() where T : class
		{
			return this._serviceProvider.GetService(typeof(T)) as T;
		}
		#region IObjectWithSite Members
		void IObjectWithSite.GetSite(ref Guid riid, out IntPtr ppvSite)
		{
			(_serviceProvider as IObjectWithSite).GetSite(ref riid, out ppvSite);
		}
		void IObjectWithSite.SetSite(object pUnkSite)
		{
			_customToolServiceProvider = pUnkSite as IOleServiceProvider;
			// Don't call SetSite on _serviceProvider, we want the site to call back to use to us
			_dteServiceProvider = null;
			_codeDomProvider = null;
		}
		#endregion // IObjectWithSite Members
		#region IOleServiceProvider Members
		int IOleServiceProvider.QueryService(ref Guid guidService, ref Guid riid, out IntPtr ppvObject)
		{
			IOleServiceProvider customToolServiceProvider = this._customToolServiceProvider;

			if (customToolServiceProvider == null)
			{
				ppvObject = IntPtr.Zero;
				return VSConstants.E_NOINTERFACE;
			}

			// First try to service the request via the IOleServiceProvider we were given. If unsuccessful, try via DTE's
			// IOleServiceProvider implementation (if we have it).
			int errorCode = this._customToolServiceProvider.QueryService(ref guidService, ref riid, out ppvObject);

			if (ErrorHandler.Failed(errorCode) || ppvObject == IntPtr.Zero)
			{
				// Fallback on the full environment service provider if necessary
				IOleServiceProvider dteServiceProvider = this._dteServiceProvider;
				if (dteServiceProvider == null)
				{
					_dteServiceProvider = customToolServiceProvider;
					EnvDTE.ProjectItem projectItem = GetService<EnvDTE.ProjectItem>();
					if (null != (projectItem = GetService<EnvDTE.ProjectItem>()) &&
						null != (dteServiceProvider = projectItem.DTE as IOleServiceProvider))
					{
						_dteServiceProvider = dteServiceProvider;
					}
				}
				if (dteServiceProvider != null &&
					!object.ReferenceEquals(dteServiceProvider, customToolServiceProvider)) // Signal used to indicate failure to retrieve dte provider
				{
					errorCode = dteServiceProvider.QueryService(ref guidService, ref riid, out ppvObject);
				}
			}
			return errorCode;
		}
		#endregion // IOleServiceProvider Members
		#region IServiceProvider Members
		object IServiceProvider.GetService(Type serviceType)
		{
			// Pass this on to our ServiceProvider which will pass it back to us via our implementation of IOleServiceProvider
			return this._serviceProvider.GetService(serviceType);
		}
		#endregion // IServiceProvider Members
		#endregion // ServiceProvider Interface Implementations
		#region IVsSingleFileGenerator Members
		/// <summary>
		/// The types of reports to write during generation
		/// </summary>
		private enum ReportType
		{
			/// <summary>
			/// Write a comment line
			/// </summary>
			Comment,
			/// <summary>
			/// Write an error report
			/// </summary>
			Error,
			/// <summary>
			/// Write a warning report
			/// </summary>
			Warning,
		}
		/// <summary>
		/// A callback delegate for writing report contents during generation
		/// </summary>
		/// <param name="message">The message to write</param>
		/// <param name="type">The type of the report</param>
		/// <param name="ex">Exception information, used with an error report</param>
		private delegate void WriteReportItem(string message, ReportType type, Exception ex);
		int IVsSingleFileGenerator.DefaultExtension(out string pbstrDefaultExtension)
		{
			CodeDomProvider codeProvider = CodeDomProvider;
			pbstrDefaultExtension = DEFAULT_EXTENSION_DECORATOR + ((codeProvider != null) ? codeProvider.FileExtension : ".txt");
			return VSConstants.S_OK;
		}
		int IVsSingleFileGenerator.Generate(string wszInputFilePath, string bstrInputFileContents, string wszDefaultNamespace, IntPtr[] rgbOutputFileContents, out uint pcbOutput, IVsGeneratorProgress pGenerateProgress)
		{
			#region ParameterValidation
			if (String.IsNullOrEmpty(bstrInputFileContents))
			{
				if (!String.IsNullOrEmpty(wszInputFilePath))
				{
					bstrInputFileContents = File.ReadAllText(wszInputFilePath);
				}
				if (String.IsNullOrEmpty(bstrInputFileContents))
				{
					rgbOutputFileContents[0] = IntPtr.Zero;
					pcbOutput = 0;
					return VSConstants.E_INVALIDARG;
				}
			}
			#endregion
			StringWriter outputWriter = new StringWriter();
			CodeDomProvider codeProvider = CodeDomProvider;
			GenerateCode(
				bstrInputFileContents,
				wszDefaultNamespace,
				pGenerateProgress,
				#region Report callback
				delegate(string message, ReportType type, Exception ex)
				{
					switch (type)
					{
						case ReportType.Comment:
							if (codeProvider != null)
							{
								codeProvider.GenerateCodeFromStatement(new CodeCommentStatement(message), outputWriter, null);
							}
							else
							{
								outputWriter.WriteLine(message);
							}
							break;
						case ReportType.Error:
						case ReportType.Warning:
							if (codeProvider != null)
							{
								CodeStatement statement;
								if (type == ReportType.Warning)
								{
									statement = new CodeCommentStatement("WARNING: " + message);
								}
								else
								{
									statement = new CodeSnippetStatement("#error " + message);
								}
								codeProvider.GenerateCodeFromStatement(statement, outputWriter, null);
							}
							else
							{
								outputWriter.Write((type == ReportType.Warning) ? "WARNING: " : "ERROR: ");
								outputWriter.WriteLine(message);
							}
							pGenerateProgress.GeneratorError((type == ReportType.Warning) ? 1 : 0, 0, message, uint.MaxValue, uint.MaxValue);
							Exception currentException = ex;
							while (currentException != null)
							{
								message = ex.Message;
								string stackTrace = ex.StackTrace;
								string exType = ex.GetType().FullName;
								if (codeProvider != null)
								{
									codeProvider.GenerateCodeFromStatement(new CodeCommentStatement(exType), outputWriter, null);
									if (!string.IsNullOrEmpty(message))
									{
										codeProvider.GenerateCodeFromStatement(new CodeCommentStatement(message), outputWriter, null);
									}
									codeProvider.GenerateCodeFromStatement(new CodeCommentStatement(stackTrace), outputWriter, null);
								}
								else
								{
									outputWriter.WriteLine(exType);
									if (!string.IsNullOrEmpty(message))
									{
										outputWriter.WriteLine(message);
									}
									outputWriter.WriteLine(stackTrace);
								}
								currentException = currentException.InnerException;
								if (currentException != null)
								{
									message = "Information from InnerException";
									if (codeProvider != null)
									{
										codeProvider.GenerateCodeFromStatement(new CodeCommentStatement(message), outputWriter, null);
									}
									else
									{
										outputWriter.WriteLine(message);
									}
								}
							}
							break;
					}
				}
				#endregion // Report callback
			);
			outputWriter.Flush();
			byte[] bytes = Encoding.UTF8.GetBytes(outputWriter.ToString());
			byte[] preamble = Encoding.UTF8.GetPreamble();
			int bufferLength = bytes.Length + preamble.Length;
			IntPtr pBuffer = Marshal.AllocCoTaskMem(bufferLength);
			Marshal.Copy(preamble, 0, pBuffer, preamble.Length);
			Marshal.Copy(bytes, 0, (IntPtr)((uint)pBuffer + preamble.Length), bytes.Length);
			rgbOutputFileContents[0] = pBuffer;
			pcbOutput = (uint)bufferLength;
			return VSConstants.S_OK;
		}
		private sealed class ItemPropertiesImpl : IORMGeneratorItemProperties
		{
			#region Member Variables and Constructor
			private EnvDTE.Properties myProjectProperties;
			private EnvDTE.Properties myProjectItemProperties;
			private References myReferences;
			public ItemPropertiesImpl(EnvDTE.Project project, EnvDTE.ProjectItem projectItem)
			{
				VSProject vsProj = project.Object as VSProject;
				if (vsProj != null)
				{
					myReferences = vsProj.References;
				}
				myProjectProperties = project.Properties;
				myProjectItemProperties = projectItem.Properties;
			}
			#endregion // Member Variables and Constructor
			#region IORMGeneratorItemProperties Members
			/// <summary>
			/// Implements <see cref="IORMGeneratorItemProperties.GetItemProperty"/>
			/// </summary>
			public string GetItemProperty(string propertyName)
			{
				return GetProperty(myProjectItemProperties, propertyName);
			}
			/// <summary>
			/// Implements <see cref="IORMGeneratorItemProperties.GetProjectProperty"/>
			/// </summary>
			public string GetProjectProperty(string propertyName)
			{
				return GetProperty(myProjectProperties, propertyName);
			}
			/// <summary>
			/// Implements <see cref="IORMGeneratorItemProperties.EnsureProjectReference"/>
			/// </summary>
			public bool EnsureProjectReference(string referencedNamespace, string assemblyName)
			{
				References refs = myReferences;
				if (refs != null)
				{
					if (refs.Item(referencedNamespace) == null)
					{
						refs.Add(assemblyName);
					}
					return true;
				}
				return false;
			}
			private static string GetProperty(EnvDTE.Properties properties, string propertyName)
			{
				string retVal = "";
				try
				{
					EnvDTE.Property property = properties.Item(propertyName);
					if (property != null)
					{
						object value = property.Value;
						if (value != null)
						{
							retVal = value as string ?? value.ToString();
						}
					}
				}
				catch (ArgumentException)
				{
				}
				return retVal;
			}
			#endregion // IORMGeneratorItemProperties Members
		}
		/// <summary>
		/// Helper structure for GenerateCode to
		/// track build items.
		/// </summary>
		private struct BoundBuildItem
		{
			/// <summary>
			/// Create a new <see cref="BoundBuildItem"/>
			/// </summary>
			public BoundBuildItem(
#if VISUALSTUDIO_10_0
				ProjectItemElement item,
#else
				BuildItem item,
#endif
				IORMGenerator generator,
				int step)
			{
				BuildItem = item;
				ORMGenerator = generator;
				FormatStep = step;
			}
			/// <summary>
			/// Does the provided format step represent the
			/// next sequence in the format process?
			/// </summary>
			public bool IsNextFormatStep(int lastFormatStep)
			{
				if (lastFormatStep < 0)
				{
					return false;
				}
				else
				{
					int testFormatStep = FormatStep;
					if (testFormatStep < 0)
					{
						testFormatStep = ~testFormatStep;
					}
					return (testFormatStep - lastFormatStep) == 1;
				}
			}
#if VISUALSTUDIO_10_0
			/// <summary>
			/// The <see cref="ProjectItemElement"/> that corresponds to
			/// the generated file.
			/// </summary>
			public ProjectItemElement BuildItem;
#else // VISUALSTUDIO_10_0
			/// <summary>
			/// The <see cref="Microsoft.Build.BuildEngine.BuildItem"/> that corresponds to
			/// the generated file.
			/// </summary>
			public BuildItem BuildItem;
#endif // VISUALSTUDIO_10_0
			/// <summary>
			/// The <see cref="IORMGenerator"/> used to generate this step
			/// </summary>
			public IORMGenerator ORMGenerator;
			/// <summary>
			/// The format step. This is a lightly encoded number, with
			/// a bitwise-inverse negative number indicating the final step.
			/// So, {"Foo", -1} indicates that the format had a single
			/// step only, while {"Foo", 0} indicates that there will be
			/// a following {"Foo", ~n} to finalize the format. A stream
			/// with a none-final step should only be sent to the next
			/// step in the sequence.
			/// </summary>
			public int FormatStep;
		}
		private void GenerateCode(string bstrInputFileContents, string wszDefaultNamespace, IVsGeneratorProgress pGenerateProgress, WriteReportItem report)
		{
			report(
@"Report file generated by ORMCustomTool.
 Any generation errors will appear as #error lines in this file,
 followed by the exception message and stack trace.",
				ReportType.Comment,
				null);

			EnvDTE.ProjectItem projectItem = this.GetService<EnvDTE.ProjectItem>();
			Debug.Assert(projectItem != null);
			string projectItemName = projectItem.Name;

			// If we weren't passed a default namespace, pick one up from the project properties
			EnvDTE.Project envProject = projectItem.ContainingProject;
			if (String.IsNullOrEmpty(wszDefaultNamespace))
			{
				wszDefaultNamespace = envProject.Properties.Item("DefaultNamespace").Value as string;
			}

			string projectItemExtension = Path.GetExtension(projectItemName);
			if (!String.Equals(projectItemExtension, EXTENSION_ORM, StringComparison.OrdinalIgnoreCase) && !String.Equals(projectItemExtension, EXTENSION_XML, StringComparison.OrdinalIgnoreCase))
			{
				// UNDONE: Localize message.
				report(
					"ORMCustomTool is only supported on Object-Role Modeling files, which must have an '.orm' or '.xml' extension.",
					ReportType.Error,
					null);
				return;
			}

			// This is actually the full project path for the next couple of lines, and then it is changed to the project directory.
			string projectPath = envProject.FullName;

			// Get the relative path of the project item.
			string projectItemFullPath = (string)projectItem.Properties.Item("LocalPath").Value;
			string projectItemRelPath = (new Uri(projectPath)).MakeRelativeUri(new Uri(projectItemFullPath)).ToString();
#if VISUALSTUDIO_10_0
			ProjectRootElement project = ProjectRootElement.TryOpen(projectPath);
			ProjectItemGroupElement ormItemGroup = GetItemGroup(project, projectItemRelPath);
#else // VISUALSTUDIO_10_0
			Project project = Engine.GlobalEngine.GetLoadedProject(projectPath);
			BuildItemGroup ormItemGroup = GetItemGroup(project, projectItemRelPath);
#endif // VISUALSTUDIO_10_0
			Debug.Assert(project != null);
			pGenerateProgress.Progress(1, 20);

			pGenerateProgress.Progress(1, 10);

			if (ormItemGroup == null || ormItemGroup.Count <= 0)
			{
				// UNDONE: Localize message.
				report(
					"No BuildItemGroup was found for this ORM file. Use the ORMGeneratorSettings dialog to add items to the group, or clear the CustomTool property.",
					ReportType.Warning,
					null);
				return;
			}
			else
			{
				IORMGeneratorItemProperties itemProperties = new ItemPropertiesImpl(envProject, projectItem);
				List<BoundBuildItem> boundBuildItems = new List<BoundBuildItem>(ormItemGroup.Count);
				IDictionary<string, IORMGenerator> generators = ORMCustomTool.ORMGenerators;
#if VISUALSTUDIO_10_0
				foreach (ProjectItemElement buildItem in ormItemGroup.Items)
#else
				foreach (BuildItem buildItem in ormItemGroup)
#endif
				{
					string generatorNameData = buildItem.GetEvaluatedMetadata(ITEMMETADATA_ORMGENERATOR);
					string[] generatorNames;
					int generatorNameCount;
					if (!String.IsNullOrEmpty(generatorNameData) &&
						String.Equals(buildItem.GetEvaluatedMetadata(ITEMMETADATA_DEPENDENTUPON), projectItemName, StringComparison.OrdinalIgnoreCase) &&
						null != (generatorNames = generatorNameData.Split((char[])null, StringSplitOptions.RemoveEmptyEntries)) &&
						0 != (generatorNameCount = generatorNames.Length))
					{
						IORMGenerator resolvedGenerator = null;
						int resolvedGeneratorIndex = 0;
						for (int i = 0; i < generatorNameCount; ++i)
						{
							IORMGenerator generator;
							string generatorName = generatorNames[i];
							if (generators.TryGetValue(generatorName, out generator))
							{
								if (resolvedGenerator != null)
								{
									boundBuildItems.Add(new BoundBuildItem(buildItem, resolvedGenerator, resolvedGeneratorIndex));
									resolvedGenerator = null;
									++resolvedGeneratorIndex;
								}
								resolvedGenerator = generator;
							}
							else if (i == 0)
							{
								// UNDONE: Localize error message.
								report(
									string.Format(CultureInfo.InvariantCulture, "#error Skipping generation of '{0}' because generator '{1}' is not installed.", ORMCustomToolUtility.GetItemInclude(buildItem), generatorName),
									ReportType.Error,
									null);
								// We can't keep going with the primary generator for the item
								break;
							}
							else
							{
								// UNDONE: Localize error message.
								report(
									string.Format(CultureInfo.InvariantCulture, "#error Generation of '{0}' is incomplete because modifier generator '{1}' is not installed.", ORMCustomToolUtility.GetItemInclude(buildItem), generatorName),
									ReportType.Error,
									null);
							}
						}
						if (resolvedGenerator != null)
						{
							boundBuildItems.Add(new BoundBuildItem(buildItem, resolvedGenerator, ~resolvedGeneratorIndex));
						}
					}
				}
				pGenerateProgress.Progress(1, 5);

				Dictionary<string, Stream> outputFormatStreams = new Dictionary<string, Stream>(boundBuildItems.Count + 1, StringComparer.OrdinalIgnoreCase);
				ReadOnlyDictionary<string, Stream> readonlyOutputFormatStreams = new ReadOnlyDictionary<string, Stream>(outputFormatStreams);
				Dictionary<string, int> lastFormatSteps = new Dictionary<string, int>(boundBuildItems.Count + 1, StringComparer.OrdinalIgnoreCase);

				// Get a Stream for the input ORM file...
				Stream ormStream = null;
				EnvDTE.Document projectItemDocument = projectItem.Document;
				string itemPath;
				if (projectItemDocument != null)
				{
					if (null == (ormStream = ORMCustomToolUtility.GetDocumentExtension<Stream>(projectItemDocument, "ORMXmlStream", itemPath = projectItem.get_FileNames(0), _serviceProvider)))
					{
						EnvDTE.TextDocument projectItemTextDocument = ORMCustomToolUtility.GetDocumentExtension<EnvDTE.TextDocument>(projectItemDocument, "TextDocument", itemPath, _serviceProvider);
						if (projectItemTextDocument != null)
						{
							ormStream = new MemoryStream(Encoding.UTF8.GetBytes(projectItemTextDocument.StartPoint.CreateEditPoint().GetText(projectItemTextDocument.EndPoint)), false);
						}
					}
				}
				if (ormStream == null)
				{
					ormStream = new MemoryStream(Encoding.UTF8.GetBytes(bstrInputFileContents), false);
				}

				// Switch the ormStream to a readonly stream so we can reuse it multiple times
				ormStream = new ReadOnlyStream(ormStream);

				// Add the input ORM file Stream...
				outputFormatStreams.Add(ORM_OUTPUT_FORMAT, ormStream);
				lastFormatSteps.Add(ORM_OUTPUT_FORMAT, -1);

				ORMExtensionManager ormExtensionManager = new ORMExtensionManager(projectItem, projectItemDocument, ormStream);
				string[] ormExtensions = null; // Delay populated if a requires is made

				// Null out bstrInputFileContents to prevent its usage beyond this point.
				bstrInputFileContents = null;

				uint ormBuildItemsCount = (uint)boundBuildItems.Count;
				uint progressCurrent = (uint)(ormBuildItemsCount * 0.25);
				uint progressTotal = ormBuildItemsCount + progressCurrent + 3;
				pGenerateProgress.Progress(++progressCurrent, progressTotal);

				// Execute the rest of the generators.
				// We limit this to 100 iterations in order to avoid an infinite loop if no BuiltItem exists that provides
				// the format required by one of the BuildItems that do exist.
				string projectDirectory = null;
				bool boundBuildItemsChanged = true;
				while (boundBuildItemsChanged)
				{
					boundBuildItemsChanged = false;
					int i = 0;
					while (i < boundBuildItems.Count)
					{
						BoundBuildItem boundBuildItem = boundBuildItems[i];
#if VISUALSTUDIO_10_0
						ProjectItemElement buildItem = boundBuildItem.BuildItem;
#else
						BuildItem buildItem = boundBuildItem.BuildItem;
#endif
						IORMGenerator ormGenerator = boundBuildItem.ORMGenerator;
						try
						{
							IList<string> requiresInputFormats = ormGenerator.RequiresInputFormats;
							string outputFormat = ormGenerator.ProvidesOutputFormat;
							bool missingInputFormat = false;
							foreach (string inputFormat in requiresInputFormats)
							{
								int lastFormatStep;
								if (!(lastFormatSteps.TryGetValue(inputFormat, out lastFormatStep) &&
									(lastFormatStep < 0 || // Indicates a completed format, ignore if not next modifier
									(inputFormat == ormGenerator.ProvidesOutputFormat && boundBuildItem.IsNextFormatStep(lastFormatStep)))))
								{
									missingInputFormat = true;
									break;
								}
							}
							if (missingInputFormat)
							{
								// Go on to the next generator, we'll (probably) come back to this one
								++i;
								continue;
							}
							else
							{
								string fullItemPath = Path.Combine(projectDirectory ?? (projectDirectory = Path.GetDirectoryName(projectPath)), ORMCustomToolUtility.GetItemInclude(buildItem));
								FileInfo checkExisting;
								bool useExisting = ormGenerator.GeneratesOnce && (checkExisting = new FileInfo(fullItemPath)).Exists && checkExisting.Length != 0;
								Stream readonlyOutputStream = null;
								MemoryStream outputStream = null;
								int outputStreamLength = 0;
								if (!useExisting)
								{
									outputStream = new MemoryStream();
									try
									{
										// UNDONE: Extension checking should happen in the current generator
										// going back to the generator that produced the input file. We're only
										// extending ORM files right now, and the ORM file doesn't have a generator,
										// so we just do it here.
										bool extensionsSatisfied = true;
										foreach (string extension in ormGenerator.GetRequiredExtensionsForInputFormat(ORM_OUTPUT_FORMAT))
										{
											if (null == ormExtensions)
											{
												ormExtensions = ormExtensionManager.GetLoadedExtensions(_serviceProvider);
											}
											if (Array.BinarySearch<string>(ormExtensions, extension) < 0)
											{
												extensionsSatisfied = false;
												// UNDONE: Localize error messages.
												report(
													string.Format(CultureInfo.InvariantCulture, "The extension '{0}' in the '{1}' is required for generation of the '{2}' file. The existing contents of '{3}' will not be modified. Open the 'ORM Generator Selection' dialog and choose 'Save Changes' to automatically add required extensions.", extension, ORM_OUTPUT_FORMAT, ormGenerator.OfficialName, ORMCustomToolUtility.GetItemInclude(buildItem)),
													ReportType.Error,
													null);
											}
										}
										if (extensionsSatisfied)
										{
											ormGenerator.GenerateOutput(buildItem, outputStream, readonlyOutputFormatStreams, wszDefaultNamespace, itemProperties);
											readonlyOutputStream = new ReadOnlyStream(outputStream);
										}
									}
									catch (Exception ex)
									{
										// UNDONE: Localize error messages.
										report(
											string.Format(CultureInfo.InvariantCulture, "Exception occurred while executing transform '{0}'. The existing contents of '{1}' will not be modified.", ormGenerator.OfficialName, ORMCustomToolUtility.GetItemInclude(buildItem)),
											ReportType.Error,
											ex);
									}
									outputStreamLength = (int)outputStream.Length;
								}
								lastFormatSteps[outputFormat] = boundBuildItem.FormatStep;

								bool textLinesReloadRequired;
								IVsTextLines textLines = GetTextLinesForDocument(fullItemPath, out textLinesReloadRequired);
								// Write the result out to the appropriate file...
								if (textLines != null)
								{
									// Get edit points in the document to read the full file from
									// the in-memory editor
									object editPointStartObject;
									ErrorHandler.ThrowOnFailure(textLines.CreateEditPoint(0, 0, out editPointStartObject));
									EnvDTE.EditPoint editPointStart = editPointStartObject as EnvDTE.EditPoint;
									Debug.Assert(editPointStart != null);
									EnvDTE.EditPoint editPointEnd = editPointStart.CreateEditPoint();
									editPointEnd.EndOfDocument();

									if (readonlyOutputStream != null)
									{
										// Reset outputStream to the beginning of the stream...
										outputStream.Seek(0, SeekOrigin.Begin);

										// We're using the readonlyOutputStream here so that the StreamReader can't close the real stream
										using (StreamReader streamReader = new StreamReader(readonlyOutputStream, Encoding.UTF8, true, (int)outputStreamLength))
										{
											// We're not passing any flags to ReplaceText, because the output of the generators should
											// be the same whether or not the user has the generated document open
											editPointStart.ReplaceText(editPointEnd, streamReader.ReadToEnd(), 0);
											if (textLinesReloadRequired)
											{
												// If a textlines is available from the DocData but not the
												// DocView, then the view may not refresh if we simply update
												// the text lines, so we force a reload at this point.
												// This works with the xml schema editors (the default view
												// for an xsd file), which is the only case we've hit
												// so far that causes problems.
												ErrorHandler.ThrowOnFailure(textLines.Reload(1));
											}
										}
										IVsPersistDocData2 persist = textLines as IVsPersistDocData2;
										if (persist != null)
										{
											int dirtyFlag;
											if (ErrorHandler.Succeeded(persist.IsDocDataDirty(out dirtyFlag)) && dirtyFlag != 0)
											{
												string bstrMkDocumentNew;
												int fSaveCanceled;
												persist.SaveDocData(VSSAVEFLAGS.VSSAVE_Save, out bstrMkDocumentNew, out fSaveCanceled);
											}
										}
									}
									else
									{
										if (outputStream != null)
										{
											// Reset outputStream to the beginning of the stream...
											outputStream.Seek(0, SeekOrigin.Begin);
										}
										else
										{
											// Handle the 'useExisting' case
											outputStream = new MemoryStream();
										}

										// The file did not generate, use what we had before if it already exists
										using (StreamWriter writer = new StreamWriter(new UncloseableStream(outputStream), Encoding.UTF8))
										{
											writer.Write(editPointStart.GetText(editPointEnd));
											writer.Flush();
										}
										outputStream.SetLength(outputStream.Position);
										readonlyOutputStream = new ReadOnlyStream(outputStream);
									}
								}
								else if (readonlyOutputStream == null)
								{
									if (outputStream != null)
									{
										// The transform failed and the file is not loaded in the
										// environment. Attempt to load it from disk. The output
										// stream is no longer needed, shut it down now.
										outputStream.Close();
									}
									if (useExisting || File.Exists(fullItemPath))
									{
										readonlyOutputStream = new ReadOnlyStream(new FileStream(fullItemPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
									}
								}
								else
								{
									bool retriedOpenFileStream = false;
								LABEL_OPEN_FILESTREAM:
									FileStream fileStream = null;
									try
									{
										fileStream = File.Create(fullItemPath, outputStreamLength, FileOptions.SequentialScan);
										fileStream.Write(outputStream.GetBuffer(), 0, outputStreamLength);
									}
									catch (IOException)
									{
										// Something seems to keep the files locked occasionally (especially the larger ones), so
										// retry once after a brief wait if we get an IOException the first time we try to to open it.
										if (!retriedOpenFileStream)
										{
											retriedOpenFileStream = true;
											System.Threading.Thread.Sleep(150);
											goto LABEL_OPEN_FILESTREAM;
										}
										throw;
									}
									finally
									{
										if (fileStream != null)
										{
											fileStream.Close();
										}
									}
								}

								if (readonlyOutputStream != null)
								{
									// Save the stream for future use
									outputFormatStreams[ormGenerator.ProvidesOutputFormat] = readonlyOutputStream; // Use indexer in case this is an update
									// Reset outputStream to the beginning of the stream...
									readonlyOutputStream.Seek(0, SeekOrigin.Begin);
								}

								boundBuildItems.RemoveAt(i);
								pGenerateProgress.Progress(++progressCurrent, progressTotal);
								// Item was removed, do not increment the counter
								boundBuildItemsChanged = true;
								continue;
							}
						}
						catch (Exception ex)
						{
							// UNDONE: Localize error message.
							report(
								string.Format(CultureInfo.InvariantCulture, "Error occurred during generation of '{0}' via IORMGenerator '{1}'.", ORMCustomToolUtility.GetItemInclude(buildItem), ormGenerator.OfficialName),
								ReportType.Error,
								ex);
							boundBuildItems.RemoveAt(i);
							pGenerateProgress.Progress(++progressCurrent, progressTotal);
							// Item was removed, do not increment the counter
							boundBuildItemsChanged = true;
							continue;
						}
					}
				}

				pGenerateProgress.Progress(++progressCurrent, progressTotal);

				foreach (Stream stream in outputFormatStreams.Values)
				{
					ReadOnlyStream readOnlyStream;
					if ((readOnlyStream = stream as ReadOnlyStream) != null)
					{
						readOnlyStream.CloseBackingStream();
					}
				}

				foreach (EnvDTE.ProjectItem childProjectItem in projectItem.ProjectItems)
				{
					EnvDTE.Property customToolProperty = childProjectItem.Properties.Item("CustomTool");
					if (customToolProperty != null && !string.IsNullOrEmpty(customToolProperty.Value as string))
					{
						VSProjectItem vsChildProjectItem = childProjectItem.Object as VSProjectItem;
						if (vsChildProjectItem != null)
						{
							vsChildProjectItem.RunCustomTool();
						}
					}
				}
				
				pGenerateProgress.Progress(progressTotal, progressTotal);
			}
		}
		#endregion // IVsSingleFileGenerator Members
	}
	#endregion // ORMCustomTool class
	#region ORMCustomToolUtility class
	/// <summary>
	/// Miscellaneous extension methods matching older methods
	/// </summary>
	internal static class ORMCustomToolUtility
	{
		/// <summary>
		/// Get an extension property directly from <see cref="M:EnvDTE.Document.Object"/>
		/// with a direct query to <see cref="EnvDTE.IExtensibleObject"/> as a backup.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="document">The document.</param>
		/// <param name="extensionName">The name of the extension.</param>
		/// <param name="documentPath">The document path (fallback if document is partially constructed).</param>
		/// <param name="serviceProvider">The <see cref="IServiceProvider"/> (for document search)</param>
		/// <returns>Extension object of given type.</returns>
		public static T GetDocumentExtension<T>(EnvDTE.Document document, string extensionName, string documentPath, IServiceProvider serviceProvider) where T : class
		{
			object retVal = document.Object(extensionName);
#if VISUALSTUDIO_10_0
			// VS2010 is isolating the document significantly more than in the
			// earlier versions. If the extension information is not available
			// through the DTE document then use the running document table to
			// access it directly.
			if (null == retVal)
			{
				IVsRunningDocumentTable docTable;
				IVsHierarchy pHier;
				uint itemId;
				uint docCookie;
				IntPtr punkDocData = IntPtr.Zero;
				if (null != (docTable = serviceProvider.GetService(typeof(IVsRunningDocumentTable)) as IVsRunningDocumentTable) &&
					ErrorHandler.Succeeded(docTable.FindAndLockDocument(
						(uint)_VSRDTFLAGS.RDT_NoLock,
						documentPath,
						out pHier,
						out itemId,
						out punkDocData,
						out docCookie)))
				{
					try
					{
						EnvDTE.IExtensibleObject extensibleObject = Marshal.GetObjectForIUnknown(punkDocData) as EnvDTE.IExtensibleObject;
						if (extensibleObject != null)
						{
							extensibleObject.GetAutomationObject(extensionName, null, out retVal);
						}
					}
					finally
					{
						if (punkDocData != IntPtr.Zero)
						{
							Marshal.Release(punkDocData);
						}
					}
				}
			}
#endif // VISUALSTUDIO_10_0
			return retVal as T;
		}
#if VISUALSTUDIO_10_0
		/// <summary>
		/// Replacement for BuildItem method
		/// </summary>
		public static string GetEvaluatedMetadata(this ProjectItemElement item, string metadataName)
		{
			foreach (ProjectMetadataElement metadataElement in item.Metadata)
			{
				if (metadataElement.Name == metadataName)
				{
					// UNDONE: VS2010 This returns an unescaped value instead of an escaped
					// value so property expansions are not done. However, none of the metadata
					// we're requesting should be escaped, so in practice there is essentially
					// no impact here. We would need Evaluation.ProjectItem instead of Construction.ProjectItemElement
					// to get the escaped form.
					return metadataElement.Value;
				}
			}
			return null;
		}
		/// <summary>
		/// Replacement for BuildItem method
		/// </summary>
		public static string GetMetadata(this ProjectItemElement item, string metadataName)
		{
			foreach (ProjectMetadataElement metadataElement in item.Metadata)
			{
				if (metadataElement.Name == metadataName)
				{
					return metadataElement.Value;
				}
			}
			return null;
		}
		/// <summary>
		/// Replacement for BuildItem method
		/// </summary>
		public static void SetMetadata(this ProjectItemElement item, string metadataName, string metadataValue)
		{
			item.AddMetadata(metadataName, metadataValue);
		}
		/// <summary>
		/// Replacement for BuildItem.FinalItemSpec property
		/// </summary>
		public static string GetItemInclude(ProjectItemElement item)
		{
			// UNDONE: VS2010 This returns the equivalent of Construction.ProjectItemElement.Include
			// instead of Evaluation.ProjectItem.EvaluatedInclude, which corresponds to
			// BuildEngine.BuildItem.FinalItemSpec. However, in the vast majority of cases
			// these are the same (a hand project file edit is required otherwise), so this
			// is not generally an issue. All uses of this should also be examined to determine
			// if Include or EvaluatedInclude is the correct property to use. For example, the
			// ItemGroup key should probably be Include, not EvaluatedInclude.
			return item.Include;
		}
#else // VISUALSTUDIO_10_0
		/// <summary>
		/// Replacement for BuildItem.FinalItemSpec property
		/// </summary>
		public static string GetItemInclude(BuildItem item)
		{
			return item.FinalItemSpec;
		}
#endif // VISUALSTUDIO_10_0
	}
	#endregion // ORMCustomToolUtility class
}
