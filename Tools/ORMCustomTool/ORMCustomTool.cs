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
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Designer.Interfaces;
using Microsoft.VisualStudio.TextManager.Interop;
using VSLangProj;

using Debug = System.Diagnostics.Debug;
using IServiceProvider = System.IServiceProvider;
using VSConstants = Microsoft.VisualStudio.VSConstants;
using ErrorHandler = Microsoft.VisualStudio.ErrorHandler;
using IOleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;
using IVsTextLines = Microsoft.VisualStudio.TextManager.Interop.IVsTextLines;
using IVsTextView = Microsoft.VisualStudio.TextManager.Interop.IVsTextView;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Core.Load;
using ORMSolutions.ORMArchitect.Core.Shell;
using Microsoft.VisualStudio.Modeling;
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
#if VISUALSTUDIO_15_0
		// The registry location is no longer fixed with Side by Side VSIX-only installation. This value is appended to a base key.
		private const string GENERATORS_REGISTRYROOT = @"ORM Solutions\Natural ORM Architect\Generators";
#elif VISUALSTUDIO_14_0
		private const string GENERATORS_REGISTRYROOT = @"Software\ORM Solutions\Natural ORM Architect for Visual Studio 2015\Generators";
#elif VISUALSTUDIO_12_0
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
			string matchCondition = string.Concat(ITEMGROUP_CONDITIONSTART, projectItemName, ITEMGROUP_CONDITIONEND);
			foreach (ProjectItemGroupElement itemGroup in rootElement.ItemGroups)
			{
				// We don't want to process BuildItemGroups that are from outside this project
				if (string.Equals(itemGroup.Condition.Trim(), matchCondition, StringComparison.OrdinalIgnoreCase))
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
			string matchCondition = string.Concat(ITEMGROUP_CONDITIONSTART, projectItemName, ITEMGROUP_CONDITIONEND);
			foreach (BuildItemGroup buildItemGroup in project.ItemGroups)
			{
				// We don't want to process BuildItemGroups that are from outside this project
				if (!buildItemGroup.IsImported && string.Equals(buildItemGroup.Condition.Trim(), matchCondition, StringComparison.OrdinalIgnoreCase))
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
			if (string.IsNullOrEmpty(bstrInputFileContents))
			{
				if (!string.IsNullOrEmpty(wszInputFilePath))
				{
					bstrInputFileContents = File.ReadAllText(wszInputFilePath);
				}
				if (string.IsNullOrEmpty(bstrInputFileContents))
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
				GeneratorTarget[] targetInstance,
				int step)
			{
				BuildItem = item;
				ORMGenerator = generator;
				TargetInstance = targetInstance;
				FormatStep = step;
				Signature = BuildSignature(generator.ProvidesOutputFormat, targetInstance);
			}
			/// <summary>
			/// Generator a string signature for the given format and target instances
			/// </summary>
			public static string BuildSignature(string generatedFormat, GeneratorTarget[] targetInstance, IList<int> filterIndices = null)
			{
				int instanceLength;
				if (targetInstance == null ||
					(instanceLength = targetInstance.Length) == 0 ||
					(filterIndices != null && (filterIndices.Count > instanceLength || (filterIndices[filterIndices.Count - 1] > (instanceLength - 1)))))
				{
					return generatedFormat;
				}

				// Signature is the generator name followed by alternating target type/target value
				// pairs. Delimiter is character 1. Note that GeneratorTarget ids are not guaranteed
				// to be unique and are used only for the generator transform.
				string[] names;
				if (filterIndices == null)
				{
					names = new string[instanceLength + instanceLength + 1];
					names[0] = generatedFormat;
					int iNext = 0;
					for (int i = 0; i < instanceLength; ++i)
					{
						GeneratorTarget target = targetInstance[i];
						names[++iNext] = target.TargetType;
						names[++iNext] = target.TargetName ?? string.Empty;
					}
				}
				else
				{
					instanceLength = filterIndices.Count;
					names = new string[instanceLength + instanceLength + 1];
					names[0] = generatedFormat;
					int iNext = 0;
					for (int i = 0; i < instanceLength; ++i)
					{
						GeneratorTarget target = targetInstance[filterIndices[i]];
						names[++iNext] = target.TargetType;
						names[++iNext] = target.TargetName ?? string.Empty;
					}
				}
				return string.Join(new string((char)1, 1), names);
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
			public readonly ProjectItemElement BuildItem;
#else // VISUALSTUDIO_10_0
			/// <summary>
			/// The <see cref="Microsoft.Build.BuildEngine.BuildItem"/> that corresponds to
			/// the generated file.
			/// </summary>
			public readonly BuildItem BuildItem;
#endif // VISUALSTUDIO_10_0
			/// <summary>
			/// The <see cref="IORMGenerator"/> used to generate this step
			/// </summary>
			public readonly IORMGenerator ORMGenerator;
			/// <summary>
			/// The format step. This is a lightly encoded number, with
			/// a bitwise-inverse negative number indicating the final step.
			/// So, {"Foo", -1} indicates that the format had a single
			/// step only, while {"Foo", 0} indicates that there will be
			/// a following {"Foo", ~n} to finalize the format. A stream
			/// with a none-final step should only be sent to the next
			/// step in the sequence.
			/// </summary>
			public readonly int FormatStep;
			/// <summary>
			/// Ordered generator targets specified with this build item.
			/// </summary>
			public readonly GeneratorTarget[] TargetInstance;
			/// <summary>
			/// A combination of the generator name and generator target names in string form
			/// </summary>
			public readonly string Signature;
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
			if (string.IsNullOrEmpty(wszDefaultNamespace))
			{
				wszDefaultNamespace = envProject.Properties.Item("DefaultNamespace").Value as string;
			}

			string projectItemExtension = Path.GetExtension(projectItemName);
			if (!string.Equals(projectItemExtension, EXTENSION_ORM, StringComparison.OrdinalIgnoreCase) && !string.Equals(projectItemExtension, EXTENSION_XML, StringComparison.OrdinalIgnoreCase))
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
				bool usingGeneratorTargets = false;
				bool retrievedGeneratorTargets = false;
				IDictionary<string, IORMGenerator> generators =
#if VISUALSTUDIO_15_0
					ORMCustomTool.GetORMGenerators(_serviceProvider);
#else
					ORMCustomTool.ORMGenerators;
#endif
				Action<
#if VISUALSTUDIO_10_0
					ProjectItemElement,
#else
					BuildItem
#endif
					Action<BoundBuildItem>,
					Action<IORMGenerator, bool>> bindBuildItem = delegate (
#if VISUALSTUDIO_10_0
					ProjectItemElement buildItem,
#else
					BuildItem buildItem,
#endif
					Action<BoundBuildItem> onItemBound,
					Action< IORMGenerator, bool> onGeneratorResolved)
				{
					string generatorNameData;
					string[] generatorNames;
					int generatorNameCount;
					if (!string.IsNullOrEmpty(generatorNameData = buildItem.GetEvaluatedMetadata(ITEMMETADATA_ORMGENERATOR)) &&
						!string.IsNullOrEmpty(generatorNameData = generatorNameData.Trim()) &&
						string.Equals(buildItem.GetEvaluatedMetadata(ITEMMETADATA_DEPENDENTUPON), projectItemName, StringComparison.OrdinalIgnoreCase) &&
						null != (generatorNames = generatorNameData.Split((char[])null, StringSplitOptions.RemoveEmptyEntries)) &&
						0 != (generatorNameCount = generatorNames.Length))
					{
						// Always calculate the build item generator targets. If these do no end up matching a resolved GeneratorTarget instance
						// when we report an error message and leave any existing file intact. However, we do not want to reprocess these
						// items as if there was no generator target specified, so we want this information regardless of whether or not
						// it binds to a resolved generator target instance.
						GeneratorTarget[] buildItemGeneratorTargets = ORMCustomToolUtility.GeneratorTargetsFromBuildItem(buildItem);
						IORMGenerator resolvedGenerator = null;
						int resolvedGeneratorIndex = 0;
						for (int i = 0; i < generatorNameCount; ++i)
						{
							IORMGenerator generator;
							string generatorName = generatorNames[i];
							if (generators.TryGetValue(generatorName, out generator))
							{
								if (onGeneratorResolved != null)
								{
									onGeneratorResolved(generator, i == 0);
								}
								if (resolvedGenerator != null)
								{
									onItemBound(new BoundBuildItem(buildItem, resolvedGenerator, buildItemGeneratorTargets, resolvedGeneratorIndex));
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
							onItemBound(new BoundBuildItem(buildItem, resolvedGenerator, buildItemGeneratorTargets, ~resolvedGeneratorIndex));
						}
					}
				};

				Dictionary<string, string> generatorNamesByOutputFormat = new Dictionary<string, string>();
#if VISUALSTUDIO_10_0
				foreach (ProjectItemElement buildItem in ormItemGroup.Items)
#else
				foreach (BuildItem buildItem in ormItemGroup)
#endif
				{
					bindBuildItem(
						buildItem,
						delegate (BoundBuildItem boundItem)
						{
							boundBuildItems.Add(boundItem);
						},
						delegate(IORMGenerator resolvedGenerator, bool isFirst)
						{
							if (!usingGeneratorTargets)
							{
								usingGeneratorTargets = resolvedGenerator.GeneratorTargetTypes != null && resolvedGenerator.GeneratorTargetTypes.Count != 0;
							}

							if (isFirst && !generatorNamesByOutputFormat.ContainsKey(resolvedGenerator.ProvidesOutputFormat))
							{
								generatorNamesByOutputFormat[resolvedGenerator.ProvidesOutputFormat] = resolvedGenerator.OfficialName;
							}
						});
				}
				pGenerateProgress.Progress(1, 5);

				Dictionary<string, Stream> outputFormatStreams = new Dictionary<string, Stream>(boundBuildItems.Count + 1, StringComparer.OrdinalIgnoreCase);
				ReadOnlyDictionary<string, Stream> readonlyOutputFormatStreams = new ReadOnlyDictionary<string, Stream>(outputFormatStreams);
				Dictionary<string, int> lastFormatSteps = new Dictionary<string, int>(boundBuildItems.Count + 1, StringComparer.OrdinalIgnoreCase);

				// Get a Stream for the input ORM file...
				Stream ormStream = null;
				EnvDTE.Document projectItemDocument = projectItem.Document;
				string itemPath;
				IDictionary<string, GeneratorTarget[]> docTargets = null;
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
					else if (usingGeneratorTargets && !retrievedGeneratorTargets)
					{
						// If ORMXmlStream worked then so will ORMGeneratorTargets. If it returns null then there are no targets
						// available, not because the request failed. Do not try again.
						retrievedGeneratorTargets = false;
						using (Stream targetsStream = ORMCustomToolUtility.GetDocumentExtension<Stream>(projectItemDocument, "ORMGeneratorTargets", itemPath, _serviceProvider))
						{
							if (targetsStream != null)
							{
								targetsStream.Seek(0, SeekOrigin.Begin);
								ORMCustomToolUtility.NormalizeGeneratorTargets(docTargets = new BinaryFormatter().Deserialize(targetsStream) as IDictionary<string, GeneratorTarget[]>);
							}
						}
					}
				}

				if (ormStream == null)
				{
					ormStream = new MemoryStream(Encoding.UTF8.GetBytes(bstrInputFileContents), false);
				}

				// Switch the ormStream to a readonly stream so we can reuse it multiple times
				ormStream = new ReadOnlyStream(ormStream);

				if (usingGeneratorTargets && !retrievedGeneratorTargets)
				{
					IVsShell shell;
					if (null != (shell = _serviceProvider.GetService(typeof(SVsShell)) as IVsShell))
					{
						Guid pkgId = typeof(ORMDesignerPackage).GUID;
						IVsPackage package;
						if (0 != shell.IsPackageLoaded(ref pkgId, out package) || package == null)
						{
							shell.LoadPackage(ref pkgId, out package);
						}

						// Temporarily load the document so that the generator targets can be resolved.
						// to extension manager load verification.
						using (Store store = new ModelLoader(ORMDesignerPackage.ExtensionLoader, true).Load(ormStream))
						{
							ORMCustomToolUtility.NormalizeGeneratorTargets(docTargets = GeneratorTarget.ConsolidateGeneratorTargets(store as IFrameworkServices));
						}
						ormStream.Seek(0, SeekOrigin.Begin);
					}
				}

				IDictionary<string, ORMCustomToolUtility.GeneratorTargetSet> targetSetsByFormatName = null;
				Dictionary<string, int> generatorTargetsBySignature = null; // The value is an index into the instances in targetSetsByFormatName.
				Dictionary<string, BitTracker> processedGeneratorTargets = null; // Each BitTracker has a count corresponding to the instance array in targetSetsByFormatName with the same key
				if (usingGeneratorTargets)
				{
					targetSetsByFormatName = ORMCustomToolUtility.ExpandGeneratorTargets(generatorNamesByOutputFormat, docTargets
#if VISUALSTUDIO_15_0
						, _serviceProvider
#endif // VISUALSTUDIO_15_0
						);

					if (targetSetsByFormatName != null)
					{
						generatorTargetsBySignature = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
						processedGeneratorTargets = new Dictionary<string, BitTracker>();
						foreach (KeyValuePair<string, ORMCustomToolUtility.GeneratorTargetSet> kvp in targetSetsByFormatName)
						{
							string generatedFormat = kvp.Key;
							GeneratorTarget[][] instances = kvp.Value.Instances;
							processedGeneratorTargets[generatedFormat] = new BitTracker(instances.Length);
							for (int i = 0; i < instances.Length; ++i)
							{
								generatorTargetsBySignature[BoundBuildItem.BuildSignature(generatedFormat, instances[i])] = i;
							}
						}
					}
				}

				// Add the input ORM file Stream. To support multiple outputs of the same format
				// these dictionaries are actually keyed by the BoundBuildItem.Signature, not just the
				// format. However, genertor targets always apply to downstream formats, not the
				// starting ORM file, so the format name is sufficient here.
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
				string projectDirectory = Path.GetDirectoryName(projectPath);
				string newItemDirectory = Path.GetDirectoryName(projectItemRelPath);
				EnvDTE.ProjectItems projectItems = projectItem.ProjectItems;
				string sourceFileName = projectItem.Name;
				Dictionary<string, string> sideEffectItemNames = null; // See comments in ORMGeneratorSelectionControl.OnClosed
				string tmpFile = null;
				bool boundBuildItemsChanged = true;
				Dictionary<string, string> inputFormatToSignature = null;
				Dictionary<string, int> currentTargetedFormats = null; // Value is the index of the original build item that triggered this, allowing the structure to be copied.
				int buildItemCount = boundBuildItems.Count;
				int originalBuildItemCount = buildItemCount;
				BitTracker buildItemProcessed = new BitTracker(buildItemCount);
				int remainingBuildItemCount = buildItemCount;
				int iMinBuildItem = 0;
				int iBuildItem = 0;
				bool processingExtraBuildItems = false;
#if VISUALSTUDIO_10_0
				List<ProjectItemElement> removeBuildItems = null;
#else
				List<BuildItem> removeBuildItems = null;
#endif

				Action stepLoop = delegate ()
				{
					buildItemProcessed[iBuildItem] = true;
					if (iBuildItem == iMinBuildItem)
					{
						++iMinBuildItem;
					}
					++iBuildItem;
					--remainingBuildItemCount;
					if (!processingExtraBuildItems)
					{
						pGenerateProgress.Progress(++progressCurrent, progressTotal);
					}
					boundBuildItemsChanged = true;
				};

				Action registerRemoveBuildItem = delegate ()
				{
#if VISUALSTUDIO_10_0
					ProjectItemElement item;
#else
					BuildItem item;
#endif
					item = boundBuildItems[iBuildItem].BuildItem;
					(removeBuildItems ?? (removeBuildItems =
#if VISUALSTUDIO_10_0
					new List<ProjectItemElement>()
#else
					new List<BuildItem>()
#endif
					)).Add(item);
					try
					{
						EnvDTE.ProjectItem subItem = projectItems.Item(ORMCustomToolUtility.GetItemInclude(item));
						if (subItem != null)
						{
							// This will delete the existing file, which might be regenerated with
							// under a new build item later. Do not defer this or we'll delete a file
							// we just created.
							subItem.Delete();
						}
					}
					catch (ArgumentException)
					{
						// Swallow
					}
				};

				Action<string> targetOutputFormat = delegate (string outputFormat)
				{
					// This is handling the possibility that the set of generator target names changed since the
					// dialog was run. Given that target instances will all have the same dependency structure
					// we make sure that if we encounter one instance that all new instances generate, and that
					// all obsolete instances are deleted.
					if (currentTargetedFormats == null)
					{
						currentTargetedFormats = new Dictionary<string, int>();
						currentTargetedFormats[outputFormat] = iBuildItem;
					}
					else if (!currentTargetedFormats.ContainsKey(outputFormat))
					{
						currentTargetedFormats[outputFormat] = iBuildItem;
					}
				};

				try
				{
					while (boundBuildItemsChanged)
					{
						iBuildItem = iMinBuildItem;
						processingExtraBuildItems = false;

						if (currentTargetedFormats != null && currentTargetedFormats.Count != 0)
						{
							int incrementalBuildItemCount = 0;
							foreach (KeyValuePair<string, int> kvp in currentTargetedFormats)
							{
								ORMCustomToolUtility.GeneratorTargetSet targetSet = null;
								BitTracker formatStatus = processedGeneratorTargets[kvp.Key];
								for (int iInstance = 0, instanceCount = formatStatus.Count; iInstance < instanceCount; ++iInstance)
								{
									if (formatStatus[iInstance])
									{
										continue;
									}

									GeneratorTarget[] missedInstance = (targetSet ?? (targetSet = targetSetsByFormatName[kvp.Key])).Instances[iInstance];
									int primaryBuildItemIndex = kvp.Value;
									int lastBuildItemIndex = primaryBuildItemIndex;

									// Find primary generator based on FormatStep. Note that the BoundBuildItem contents
									// are always added in order and we don't change the set, so format steps for the same
									// item are always grouped.
									BoundBuildItem primaryBuildItem = boundBuildItems[primaryBuildItemIndex];
									int formatStep = primaryBuildItem.FormatStep;
									if (formatStep != ~0)
									{
										if (formatStep < 0)
										{
											primaryBuildItemIndex -= ~formatStep;
										}
										else
										{
											primaryBuildItemIndex -= formatStep;

											while (formatStep >= 0)
											{
												formatStep = boundBuildItems[++lastBuildItemIndex].FormatStep;
												if (formatStep < 0)
												{
													break;
												}
											}
										}
										primaryBuildItem = boundBuildItems[primaryBuildItemIndex];
									}

									GeneratorTarget[] targetInstance = targetSetsByFormatName[kvp.Key].Instances[iInstance];
									string defaultFileName = primaryBuildItem.ORMGenerator.GetOutputFileDefaultName(sourceFileName);
									string fileName = targetInstance == null ? defaultFileName : ORMCustomToolUtility.GeneratorTargetSet.DecorateFileName(defaultFileName, targetInstance);
									string fileRelativePath = Path.Combine(newItemDirectory, fileName);
									string fileAbsolutePath = string.Concat(new FileInfo(projectPath).DirectoryName, Path.DirectorySeparatorChar, fileRelativePath);

#if VISUALSTUDIO_10_0
									ProjectItemElement newBuildItem;
									if (ormItemGroup.Parent == null)
									{
										// If the set of items dropped to empty then the item group can delete itself. Recreate it.
										ormItemGroup = project.AddItemGroup();
#else
									BuildItem newBuildItem;
									if (ormItemGroup.Count == 0)
									{
										ormItemGroup = project.AddNewItemGroup();
#endif
										ormItemGroup.Condition = string.Concat(ITEMGROUP_CONDITIONSTART, projectItemRelPath, ITEMGROUP_CONDITIONEND);
									}

									newBuildItem = primaryBuildItem.ORMGenerator.AddGeneratedFileItem(ormItemGroup, sourceFileName, fileRelativePath);
									ORMCustomToolUtility.SetGeneratorTargetMetadata(newBuildItem, targetInstance);
									if (primaryBuildItemIndex != lastBuildItemIndex)
									{
										// Multiple generators, adjust the generator name metadata.
										string[] generatorNames = new string[lastBuildItemIndex - primaryBuildItemIndex + 1];
										for (int i = 0, count = generatorNames.Length; i < count; ++i)
										{
											generatorNames[i] = boundBuildItems[i + primaryBuildItemIndex].ORMGenerator.OfficialName;
										}
										ORMCustomToolUtility.SetItemMetaData(newBuildItem, ITEMMETADATA_ORMGENERATOR, string.Join(" ", generatorNames));
									}

									(sideEffectItemNames ?? (sideEffectItemNames = new Dictionary<string, string>()))[fileRelativePath] = null;
									if (File.Exists(fileAbsolutePath))
									{
										try
										{
											projectItems.AddFromFile(fileAbsolutePath);
										}
										catch (ArgumentException)
										{
											// Swallow
										}
									}
									else
									{
										if (tmpFile == null)
										{
											tmpFile = Path.GetTempFileName();
										}
										EnvDTE.ProjectItem newProjectItem = projectItems.AddFromTemplate(tmpFile, fileName);
										string customTool;
										if (!string.IsNullOrEmpty(customTool = newBuildItem.GetMetadata(ITEMMETADATA_GENERATOR)))
										{
											newProjectItem.Properties.Item("CustomTool").Value = customTool;
										}
									}

									// Extend the set we're processing in the main loop
									bindBuildItem(
										newBuildItem,
										delegate (BoundBuildItem boundItem)
										{
											boundBuildItems.Add(boundItem);
											++incrementalBuildItemCount;
										},
										null);
								}
							}

							if (incrementalBuildItemCount != 0)
							{
								// Hijack the loop by dynamically adding bound build items
								processingExtraBuildItems = true;
								iBuildItem = buildItemCount;
								buildItemCount += incrementalBuildItemCount;
								remainingBuildItemCount += incrementalBuildItemCount;
								buildItemProcessed.Resize(buildItemCount);
							}
							currentTargetedFormats.Clear();
						}

						boundBuildItemsChanged = false;
						while (remainingBuildItemCount != 0 && iBuildItem < buildItemCount)
						{
							bool canMoveMin = iBuildItem == iMinBuildItem; // If items are skipped in the original list then the min item may be higher than it currently is.
							while (buildItemProcessed[iBuildItem])
							{
								++iBuildItem;
								if (canMoveMin)
								{
									++iMinBuildItem;
								}
							}

							BoundBuildItem boundBuildItem = boundBuildItems[iBuildItem];
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
								string outputSignature = boundBuildItem.Signature;
								int outputInstanceIndex = -1;
								GeneratorTarget[] outputInstance = null;
								ORMCustomToolUtility.GeneratorTargetSet outputTargetSet = null;

								if (boundBuildItem.TargetInstance != null)
								{
									if (generatorTargetsBySignature != null)
									{
										// If the outputFormat does not map to an outputTargetSet then
										// we've essentially replaced a generator that uses generator targets
										// to another one that does not. We could potentially handle this
										// by just removing the generator target metadata elements, but this
										// will likely leave us with the wrong file name.
										// In practice this is not something that will actually happen because
										// a generator change should only happen in the dialog, not during a custom tool
										// execution. The only thing we're handling here is a change in the generator
										// target data, not changes to the generator itself, so we punt on this
										// scenario and simply remove the item.
										if (targetSetsByFormatName.TryGetValue(outputFormat, out outputTargetSet) &&
											generatorTargetsBySignature.TryGetValue(outputSignature, out outputInstanceIndex))
										{
											outputInstance = outputTargetSet.Instances[outputInstanceIndex];
										}
										else
										{
											// The instance didn't match anything we currently have, so this is not processed, just deleted.
											targetOutputFormat(outputFormat);
											registerRemoveBuildItem();
											stepLoop();
											continue;
										}
									}
									else
									{
										// See comments above. This should only occur if the generator itself changes.
										// This should be extremely rare given that we're auto-filling in placeholders.
										// There is no way to handle the TargetInstance here, so we just delete the item.
										registerRemoveBuildItem();
										stepLoop();
										continue;
									}
								}

								bool missingInputFormat = false;
								bool hasModifiedInputSignatures = false;
								foreach (string inputFormat in requiresInputFormats)
								{
									string inputSignature = inputFormat;
									ORMCustomToolUtility.GeneratorTargetSet inputTargetSet = null;
									IList<int> inputIndicesInOutput = null;
									if (targetSetsByFormatName != null &&
										outputTargetSet != null &&
										targetSetsByFormatName.TryGetValue(inputFormat, out inputTargetSet) &&
										null != (inputIndicesInOutput = outputTargetSet.MatchInputTargetTypes(inputTargetSet.TargetTypes))) // Defensive null check, we should always have an output set if an input has a target set
									{
										if (!hasModifiedInputSignatures)
										{
											hasModifiedInputSignatures = true;
											if (inputFormatToSignature != null)
											{
												inputFormatToSignature.Clear();
											}
											else
											{
												inputFormatToSignature = new Dictionary<string, string>();
											}
										}
										inputSignature = BoundBuildItem.BuildSignature(inputFormat, outputInstance, inputIndicesInOutput);
										inputFormatToSignature[inputFormat] = inputSignature;
									}
									int lastFormatStep;
									if (!(lastFormatSteps.TryGetValue(inputSignature, out lastFormatStep) &&
										(lastFormatStep < 0 || // Indicates a completed format, ignore if not next modifier
										(inputFormat == outputFormat && boundBuildItem.IsNextFormatStep(lastFormatStep)))))
									{
										missingInputFormat = true;
										break;
									}
								}
								if (missingInputFormat)
								{
									// Go on to the next generator, we'll (probably) come back to this one
									++iBuildItem;
									continue;
								}
								else
								{
									if (generatorTargetsBySignature != null && boundBuildItem.TargetInstance != null)
									{
										targetOutputFormat(outputFormat);

										// Signature matching ignores placeholder status, so we don't actually
										// know if a build item has the placeholder state set correctly unless
										// we created the build item during this pass through the generator.
										if (iBuildItem < originalBuildItemCount)
										{
											ORMCustomToolUtility.UpdateGeneratorTargetPlaceholders(buildItem, outputInstance);
										}

										// Mark as processed regardless of the result below here, including failure and deletion.
										BitTracker tracker = processedGeneratorTargets[outputFormat];
										tracker[outputInstanceIndex] = true;
										processedGeneratorTargets[outputFormat] = tracker;
									}

									string fullItemPath = Path.Combine(projectDirectory, ORMCustomToolUtility.GetItemInclude(buildItem));
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
												GetFormatStream streamLookup = hasModifiedInputSignatures ?
													(GetFormatStream)delegate (string formatName)
													{
														string decoratedFormatName;
														Stream stream;
														return readonlyOutputFormatStreams.TryGetValue(inputFormatToSignature.TryGetValue(formatName, out decoratedFormatName) ? decoratedFormatName : formatName, out stream) ? stream : null;
													}
												:
													delegate (string formatName)
													{
														Stream stream;
														return readonlyOutputFormatStreams.TryGetValue(formatName, out stream) ? stream : null;
													};
												ormGenerator.GenerateOutput(buildItem, outputStream, streamLookup, wszDefaultNamespace, itemProperties, outputInstance);
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
									lastFormatSteps[outputSignature] = boundBuildItem.FormatStep;

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
										outputFormatStreams[outputSignature] = readonlyOutputStream; // Use indexer in case this is an update
				
										// Reset outputStream to the beginning of the stream
										readonlyOutputStream.Seek(0, SeekOrigin.Begin);
									}

									stepLoop();
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
								stepLoop();
								continue;
							}
						}
					}
					if (removeBuildItems != null)
					{
#if VISUALSTUDIO_10_0
						foreach (ProjectItemElement removeBuildItem in removeBuildItems)
						{
							ProjectElementContainer removeFrom;
							if (null != (removeFrom = removeBuildItem.Parent))
							{
								removeFrom.RemoveChild(removeBuildItem);
							}
#else // VISUALSTUDIO_10_0
						foreach (BuildItem removeBuildItem in removeBuildItems)
						{
							project.RemoveItem(removeBuildItem);
#endif // VISUALSTUDIO_10_0
						}
					}
				}
				finally
				{
					if (tmpFile != null)
					{
						File.Delete(tmpFile);
					}
				}

				if (sideEffectItemNames != null)
				{
					ORMCustomToolUtility.RemoveSideEffectItems(sideEffectItemNames, project, ormItemGroup);
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

				foreach (EnvDTE.ProjectItem childProjectItem in projectItems)
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
		// Suffix is index then _ then the target type, value is the name (not the id)
		private const string ITEMMETADATA_ORMGENERATORTARGET_PREFIX = "ORMGeneratorTarget_";
		private const string ITEMMETADATA_ORMGENERATORTARGET_PLACEHOLDERS = "ORMGeneratorTargetPlaceholders";

		/// <summary>
		/// A wrapper to enable use of a list of generator target type names
		/// as a key.
		/// </summary>
		public class GeneratorTargetSet
		{
#region Member variables and constructor
			private readonly IList<string> myTargetTypes;
			private readonly IList<string> myReadOnlyTargetTypes;
			private GeneratorTarget[][] myGeneratorInstances;
			/// <summary>
			/// Create a new <see cref="GeneratorTargetSet"/> from a list of target names.
			/// </summary>
			public GeneratorTargetSet(List<string> targetNames)
			{
				// Keep the original data so we can do instance checking on the original collection
				myTargetTypes = (IList<string>)targetNames ?? Array.Empty<string>();
				myReadOnlyTargetTypes = targetNames != null ? targetNames.AsReadOnly() : myTargetTypes;
			}
#endregion // Member variables and constructor
#region Accessor properties
			/// <summary>
			/// Get the ordered target names associated with this set.
			/// </summary>
			public IList<string> TargetTypes
			{
				get
				{
					return myReadOnlyTargetTypes;
				}
			}
			/// <summary>
			/// Get or set the instances for this target set.
			/// </summary>
			/// <remarks>For any given use of this target set the individual instances
			/// for each target type will be known and the combinations will not change.
			/// This provides a convenient location to store this data with the associated
			/// key. This data is not included in any equality check. This can be assigned
			/// to the return value from <see cref="PopulateInstances"/></remarks>
			public GeneratorTarget[][] Instances
			{
				get
				{
					return myGeneratorInstances;
				}
				set
				{
					myGeneratorInstances = value;
				}
			}
			/// <summary>
			/// Find the index of the given targets in the current instances
			/// </summary>
			/// <param name="targets">Array of targets.</param>
			/// <param name="nonPreferredInstance">Set and return true to deprioritize an instance in the target set.
			/// The instance will only be returned if no other item is available. This allows for duplicate discovery.</param>
			/// <returns>Instance index, or -1 if not found.</returns>
			public int IndexOfInstance(GeneratorTarget[] targets, Predicate<int> nonPreferredInstance)
			{
				GeneratorTarget[][] instances;
				int firstNonPreferred = -1;
				if (targets != null &&
					(instances = myGeneratorInstances) != null)
				{
					int targetCount = targets.Length;
					for (int instance = 0; instance < instances.Length; ++instance)
					{
						bool notPreferred = nonPreferredInstance != null && nonPreferredInstance(instance);

						GeneratorTarget[] instanceTargets = instances[instance];
						if (instanceTargets.Length == targets.Length)
						{
							int i = 0;
							for (; i < targetCount; ++i)
							{
								if (instanceTargets[i] != targets[i])
								{
									break;
								}
							}
							if (i == targetCount)
							{
								if (notPreferred)
								{
									if (firstNonPreferred == -1)
									{
										firstNonPreferred = instance;
									}
								}
								else
								{
									return instance;
								}
							}
						}
					}
				}
				return firstNonPreferred;
			}
			/// <summary>
			/// Assuming these is an expanded target set, the overlapping target types
			/// for any input format must be a subset of the target types for this instance.
			/// Extracting the matching input types allows us to find the correct input,
			/// which will have the same generator target names for the overlapping target types.
			/// </summary>
			/// <param name="inputTargetTypes">The target types from the input generator set.</param>
			/// <returns>A list of matching indices, or null.</returns>
			public IList<int> MatchInputTargetTypes(IList<string> inputTargetTypes)
			{
				if (inputTargetTypes == null)
				{
					return null;
				}
				// Furthermore, the target types must occur in the same order in the downstream type
				IList<string> currentTargetTypes = myReadOnlyTargetTypes;
				int currentTypeCount = currentTargetTypes.Count;
				int iCurrent = -1;
				int inputTypeCount = inputTargetTypes.Count;
				int iInput = 0;
				List<int> retVal = null;
				for (; iInput < inputTypeCount; ++iInput)
				{
					++iCurrent;
					string inputType = inputTargetTypes[iInput];
					for (; iCurrent < currentTypeCount; ++iCurrent)
					{
						if (0 == string.Compare(inputType, currentTargetTypes[iCurrent], StringComparison.OrdinalIgnoreCase))
						{
							(retVal ?? (retVal = new List<int>())).Add(iCurrent);
							break;
						}
					}
					if (iCurrent == currentTypeCount)
					{
						retVal = null;
						break;
					}
				}
				return iInput < inputTypeCount ? null : retVal;
			}
			/// <summary>
			/// Inject generator target names into a generator-default file name.
			/// The target names are placed immediately before the final file extension.
			/// </summary>
			public static string DecorateFileName(string fileName, GeneratorTarget[] targets)
			{
				int nextName = 0;
				int count = targets.Length;
				string[] names = null;
				for (int i = 0; i < count; ++i)
				{
					string name = targets[i].TargetName;
					if (!string.IsNullOrEmpty(name))
					{
						if (names == null)
						{
							names = new string[count + 1];
							names[0] = string.Empty; // We want a leading . separator, leave extra slot
							nextName = 1;
						}
						names[nextName] = name;
						++nextName;
					}
				}
				if (names != null)
				{
					string withoutSuffix = Path.GetFileNameWithoutExtension(fileName);
					return withoutSuffix + string.Join(".", names, 0, nextName) + fileName.Substring(withoutSuffix.Length);
				}
				return fileName;
			}
#endregion // Accessor properties
#region Instance expansion
			/// <summary>
			/// Generate all instance combinations matching the targets
			/// </summary>
			/// <param name="activeTargets">Dictionary keyed by target type with active targets.</param>
			/// <returns>Array of GeneratorTarget arrays representing all combinations of active
			/// targets. Instances must be fully populated, so if any of the types are not represented
			/// by a generator target then this returns null.</returns>
			public GeneratorTarget[][] PopulateInstances(IDictionary<string, GeneratorTarget[]> activeTargets)
			{
				IList<string> types = myTargetTypes;
				int typeCount = types.Count;
				int targetCount;
				GeneratorTarget[] targetList;
				GeneratorTarget[][] retVal = null;
				if (typeCount == 1)
				{
					if (activeTargets == null || !activeTargets.TryGetValue(types[0], out targetList) || 0 == (targetCount = targetList.Length))
					{
						retVal = new GeneratorTarget[1][];
						retVal[0] = new GeneratorTarget[] { new GeneratorTarget(types[0], null, null) };
					}
					else
					{
						retVal = new GeneratorTarget[targetCount][];
						for (int i = 0; i < targetCount; ++i)
						{
							retVal[i] = new GeneratorTarget[1] { targetList[i] };
						}
					}
				}
				else
				{
					// Make sure we have something for each list
					GeneratorTarget[][] simpleTargets = new GeneratorTarget[typeCount][];
					int instanceCount = 1;
					for (int i = 0; i < typeCount; ++i)
					{
						if (activeTargets == null || !activeTargets.TryGetValue(types[i], out targetList) || 0 == (targetCount = targetList.Length))
						{
							simpleTargets[i] = new GeneratorTarget[] { new GeneratorTarget(types[i], null, null) };
						}
						else
						{
							simpleTargets[i] = targetList;
							instanceCount *= targetCount;
						}
					}

					// Do combinatorics to get all instances
					int[] indices = new int[typeCount];
					int currentType = 0;
					int lastType = typeCount - 1;
					int nextInstance = 0;
					retVal = new GeneratorTarget[instanceCount][];
					for (; true;)
					{
						int currentIndex = indices[currentType];
						if (currentIndex < simpleTargets[currentType].Length)
						{
							if (currentType == lastType)
							{
								// At the end, create and populate a new instance off the indices
								GeneratorTarget[] instance = new GeneratorTarget[typeCount];
								retVal[nextInstance] = instance;
								++nextInstance;
								for (int i = 0; i < typeCount; ++i)
								{
									instance[i] = simpleTargets[i][indices[i]];
								}
								++indices[currentType];
							}
							else
							{
								++currentType;
							}
						}
						else if (currentType == 0)
						{
							// Nothing left to do, can't back out any more
							break;
						}
						else
						{
							indices[currentType] = 0; // Start over on this row
							++indices[--currentType]; // Return to previous row and bump up one
						}
					}
				}
				return retVal;
			}
#endregion // Instance expansion
#region Equality routines
			/// <summary>
			/// Strongly typed Equals override
			/// </summary>
			public bool Equals(GeneratorTargetSet obj)
			{
				if ((object)obj == null)
				{
					return false;
				}

				IList<string> typeNames;
				IList<string> otherTypeNames;
				if (object.ReferenceEquals(this, obj) || object.ReferenceEquals(typeNames = myTargetTypes, otherTypeNames = obj.myTargetTypes))
				{
					return true;
				}

				int count = typeNames.Count;
				if (otherTypeNames.Count == count)
				{
					for (int i = 0; i < count; ++i)
					{
						if (typeNames[i] != otherTypeNames[i])
						{
							return false;
						}
					}
					return true;
				}
				return false;
			}
			/// <summary>
			/// Standard Equals override
			/// </summary>
			public override bool Equals(object obj)
			{
				GeneratorTargetSet other = obj as GeneratorTargetSet;
				return (object)other != null && Equals(other);
			}
			/// <summary>
			/// Standard GetHashCode override
			/// </summary>
			public override int GetHashCode()
			{
				return Utility.GetCombinedHashCode<string>(myTargetTypes);
			}
			/// <summary>
			/// Equality operator
			/// </summary>
			public static bool operator ==(GeneratorTargetSet left, GeneratorTargetSet right)
			{
				if ((object)left == null)
				{
					return (object)right == null;
				}
				else if ((object)right == null)
				{
					return false;
				}
				return left.Equals(right);
			}
			/// <summary>
			/// Inequality operator
			/// </summary>
			public static bool operator !=(GeneratorTargetSet left, GeneratorTargetSet right)
			{
				if ((object)left == null)
				{
					return (object)right != null;
				}
				else if ((object)right == null)
				{
					return true;
				}
				return !left.Equals(right);
			}
#endregion // Equality routines
		}
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

		public delegate void NameValuePairCallback(string name, string value);
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
		/// <summary>
		/// Get matching metavalue names and values
		/// </summary>
		/// <param name="item">The item to iterate</param>
		/// <param name="nameFilter">A filter for the metadata names.</param>
		/// <param name="callback">Callback for the matching name/value pairs.</param>
		public static void GetMatchingMetadata(ProjectItemElement item, Predicate<string> nameFilter, NameValuePairCallback callback)
		{
			foreach (ProjectMetadataElement metadataElement in item.Metadata)
			{
				string name = metadataElement.Name;
				if (nameFilter(name))
				{
					callback(name, metadataElement.Value);
				}
			}
		}
		/// <summary>
		/// Set metadata on a build item for a given metadata key and value
		/// </summary>
		public static void SetItemMetaData(ProjectItemElement buildItem, string metadataName, string metadataValue)
		{
			// ProjectItemElement.SetMetadata adds a new metadata element with the same name
			// as the previous one. There is no 'RemoveMetadata' that takes a string, so we go through
			// the entire metadata collection and clean it out.
			foreach (ProjectMetadataElement element in buildItem.Metadata)
			{
				if (element.Name == metadataName)
				{
					// The Metadata collection is a read-only snapshot, so deleting from it is safe
					// inside the iterator.
					buildItem.RemoveChild(element);
					// Do not break. This handles removing multiple metadata items with the
					// same name, which will clean up project files affected by this problem
					// in previous drops.
				}
			}
			ProjectMetadataElement newElement = buildItem.AddMetadata(metadataName, metadataValue);
		}
		/// <summary>
		/// Remove a metadata item
		/// </summary>
		public static void RemoveItemMetaData(ProjectItemElement buildItem, string metadataName)
		{
			foreach (ProjectMetadataElement element in buildItem.Metadata)
			{
				if (element.Name == metadataName)
				{
					// The Metadata collection is a read-only snapshot, so deleting from it is safe
					// inside the iterator.
					buildItem.RemoveChild(element);
					break;
				}
			}
		}
		/// <summary>
		/// New items have been added to the item group tracking this orm file. Adding
		/// items to the project creates the side effiect of also adding each of these items
		/// to another item group in the project. Remove these items from the tracked list of
		/// side effect names, presented
		/// </summary>
		public static void RemoveSideEffectItems(IDictionary<string, string> sideEffectItemNames, ProjectRootElement project, ProjectItemGroupElement ignoreItemGroup)
		{
			if (sideEffectItemNames == null || sideEffectItemNames.Count == 0)
			{
				return;
			}

			string skipCondition = ignoreItemGroup.Condition.Trim();
			List<ProjectItemElement> removeItems = new List<ProjectItemElement>();
			foreach (ProjectItemGroupElement group in project.ItemGroups)
			{
				if (group.Condition.Trim() == skipCondition)
				{
					continue;
				}

				foreach (ProjectItemElement item in group.Items)
				{
					if (sideEffectItemNames.ContainsKey(item.Include))
					{
						removeItems.Add(item);
					}
				}
			}

			foreach (ProjectItemElement item in removeItems)
			{
				ProjectElementContainer removeFrom;
				if (null != (removeFrom = item.Parent))
				{
					removeFrom.RemoveChild(item);

					// Remove the container if it is empty. Note that
					// this happens automatically in the old build system.
					ProjectItemGroupElement groupElement;
					if (null != (groupElement = removeFrom as ProjectItemGroupElement) &&
						groupElement.Items.Count == 0 &&
						null != (removeFrom = groupElement.Parent))
					{
						removeFrom.RemoveChild(groupElement);
					}
				}
			}
		}
#else // VISUALSTUDIO_10_0
		/// <summary>
		/// New items have been added to the item group tracking this orm file. Adding
		/// items to the project creates the side effiect of also adding each of these items
		/// to another item group in the project. Remove these items from the tracked list of
		/// side effect names, presented
		/// </summary>
		public static void RemoveSideEffectItems(IDictionary<string, string> sideEffectItemNames, Project project, BuildItemGroup ignoreItemGroup)
		{
			if (sideEffectItemNames == null || sideEffectItemNames.Count == 0)
			{
				return;
			}

			string skipCondition = ignoreItemGroup.Condition.Trim();

			List<BuildItem> removeItems = new List<BuildItem>();
			foreach (BuildItemGroup group in project.ItemGroups)
			{
				if (group.Condition.Trim() == skipCondition)
				{
					continue;
				}

				foreach (BuildItem item in group)
				{
					if (sideEffectItemNames.ContainsKey(item.Include))
					{
						removeItems.Add(item);
					}
				}
			}

			foreach (BuildItem item in removeItems)
			{
				project.RemoveItem(item);
			}
		}
		/// <summary>
		/// Replacement for BuildItem.FinalItemSpec property
		/// </summary>
		public static string GetItemInclude(BuildItem item)
		{
			return item.FinalItemSpec;
		}
		/// <summary>
		/// Get matching metavalue names and values
		/// </summary>
		/// <param name="item">The item to iterate</param>
		/// <param name="nameFilter">A filter for the metadata names.</param>
		/// <param name="callback">Callback for the matching name/value pairs.</param>
		public static void GetMatchingMetadata(BuildItem item, Predicate<string> nameFilter, NameValuePairCallback callback)
		{
			foreach (object name in item.MetadataNames)
			{
				string nameString = name as string;
				if (nameString != null && nameFilter(nameString))
				{
					callback(nameString, item.GetEvaluatedMetadata(nameString);
				}
			}
		}
		/// <summary>
		/// Remove a metadata item
		/// </summary>
		public static void RemoveItemMetaData(BuildItem buildItem, string metadataName)
		{
			if (buildItem.HasMetadata(metadataName))
			{
				buildItem.RemoveMetadata(metadataName);
			}
		}
		/// <summary>
		/// Set metadata on a build item for a given metadata key and value
		/// </summary>
		public static void SetItemMetaData(BuildItem buildItem, string metadataName, string metadataValue)
		{
			buildItem.SetMetadata(metadataName, metadataValue);
		}
#endif // VISUALSTUDIO_10_0
		/// <summary>
		/// Get the primary generator name from a non-normalized name string that
		/// may include both primary and secondary names.
		/// </summary>
		/// <param name="rawNameData">The raw name data. This may be a space delimited
		/// list, but no space normalization is expected.</param>
		/// <returns>The first name from the list, or <see langword="null"/> if no name is specified.</returns>
		public static string GetPrimaryGeneratorName(string rawNameData)
		{
			if (string.IsNullOrEmpty(rawNameData))
			{
				return null;
			}
			if (rawNameData.IndexOf(' ') == -1)
			{
				return rawNameData;
			}
			else if (!string.IsNullOrEmpty(rawNameData = rawNameData.Trim()))
			{
				int spaceIndex;
				return (-1 == (spaceIndex = rawNameData.IndexOf(' '))) ? rawNameData : rawNameData.Substring(0, spaceIndex);
			}
			return null;
		}
		/// <summary>
		/// GeneratorTarget providers may not distinguish between null and empty values, but this tool
		/// uses a null TargetName values to indicate a placeholder target. This also gets rid of empty lists.
		/// </summary>
		/// <remarks>A placeholder is defined for each target type when a format used by an active generator
		/// does not have any generator targets. This not only allows a file to be created--possibly with minimal
		/// content indicating how the extension corresponding to that generator actually creates GeneratorTarget
		/// elements--it also allows the generator to be selected in the custom tool before the targets
		/// are created in the project.</remarks>
		public static void NormalizeGeneratorTargets(IDictionary<string, GeneratorTarget[]> targets)
		{
			if (targets != null)
			{
				List<string> removeEmptyKeys = null;
				foreach (KeyValuePair<string, GeneratorTarget[]> kvp in targets)
				{
					GeneratorTarget[] targetList = kvp.Value;
					if (targetList == null || targetList.Length == 0)
					{
						(removeEmptyKeys ?? (removeEmptyKeys = new List<string>())).Add(kvp.Key);
					}
					else
					{
						for (int i = 0; i < targetList.Length; ++i)
						{
							GeneratorTarget target = targetList[i];
							if (target.TargetName == null)
							{
								targetList[i] = new GeneratorTarget(target.TargetType, string.Empty, target.TargetId);
							}
						}
					}
				}

				if (removeEmptyKeys != null)
				{
					for (int i = 0, count = removeEmptyKeys.Count; i < count; ++i)
					{
						targets.Remove(removeEmptyKeys[i]);
					}
				}
			}
		}
		/// <summary>
		/// Create a generator target set (without identifiers) using the storaged metadata on a build item.
		/// </summary>
		/// <param name="item">The item to iterate.</param>
		/// <returns>An instance if GeneratorTarget data is available.</returns>
#if VISUALSTUDIO_10_0
		public static GeneratorTarget[] GeneratorTargetsFromBuildItem(ProjectItemElement item)
#else // VISUALSTUDIO_10_0
		public static GeneratorTarget[] GeneratorTargetsFromBuildItem(BuildItem item)
#endif // VISUALSTUDIO_10_0
		{
			// Preliminary, determine which items are placeholders. Placeholders are still named as attributes
			// to preserve numbering, but they are given a null value instead of an empty value for the GeneratorTarget
			string[] placeholders = null;
			GetMatchingMetadata(
				item,
				delegate(string test)
				{
					return test == ITEMMETADATA_ORMGENERATORTARGET_PLACEHOLDERS;
				},
				delegate (string name, string value)
				{
					if (!string.IsNullOrEmpty(value))
					{
						placeholders = value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
						if (placeholders != null && placeholders.Length == 0)
						{
							placeholders = null;
						}
					}
				}
				);

			// First pass, get an item count. These are stored as 1-based indexes, so the highest number is the count.
			// There is no guarantee that this matches currently registered generator targets, we're simply extracting text.
			int max = 0;
			GetMatchingMetadata(
				item,
				delegate (string test)
				{
					return test.StartsWith(ITEMMETADATA_ORMGENERATORTARGET_PREFIX);
				},
				delegate (string name, string value)
				{
					int searchStart = ITEMMETADATA_ORMGENERATORTARGET_PREFIX.Length;
					int secondDelimiter = name.IndexOf('_', searchStart);
					int testIndex;
					if (secondDelimiter != -1 && int.TryParse(name.Substring(searchStart, secondDelimiter - searchStart), out testIndex) && testIndex > max)
					{
						max = testIndex;
					}
				});

			// Second pass, populate the generator set
			if (max != 0)
			{
				GeneratorTarget[] retVal = new GeneratorTarget[max];
				GetMatchingMetadata(
					item,
					delegate (string test)
					{
						return test.StartsWith(ITEMMETADATA_ORMGENERATORTARGET_PREFIX);
					},
					delegate (string name, string value)
					{
						int searchStart = ITEMMETADATA_ORMGENERATORTARGET_PREFIX.Length;
						int secondDelimiter = name.IndexOf('_', searchStart);
						int testIndex;
						if (secondDelimiter != -1 && int.TryParse(name.Substring(searchStart, secondDelimiter - searchStart), out testIndex) && testIndex > 0)
						{
							string targetType = name.Substring(secondDelimiter + 1);
							retVal[--testIndex] = new GeneratorTarget(targetType, (placeholders == null || Array.IndexOf<string>(placeholders, targetType) == -1) ? value ?? string.Empty : null, null);
						}
					});

				for (int i = 0; i < max; ++i)
				{
					if (retVal[i] == null)
					{
						// Sanity check, not well formed
						return null;
					}
				}
				return retVal;
			}
			return null;
		}
		/// <summary>
		/// Create a generator target set (without identifiers) using the stored metadata on a build item.
		/// </summary>
		/// <param name="item">The item to iterate.</param>
		/// <param name="targetInstance">The generator target values.</param>
#if VISUALSTUDIO_10_0
		public static void SetGeneratorTargetMetadata(ProjectItemElement item, GeneratorTarget[] targetInstance)
#else // VISUALSTUDIO_10_0
		public static void SetGeneratorTargetMetadata(BuildItem item, GeneratorTarget[] targetInstance)
#endif // VISUALSTUDIO_10_0
		{
			List<string> placeholders = null;
			for (int i = 0, count = targetInstance.Length; i < count; ++i)
			{
				GeneratorTarget target = targetInstance[i];
				string type = target.TargetType;
				string name = target.TargetName;
				if (name == null)
				{
					(placeholders ?? (placeholders = new List<string>())).Add(type);
					name = string.Empty;
				}
				item.SetMetadata(ITEMMETADATA_ORMGENERATORTARGET_PREFIX + (i + 1).ToString() + "_" + type, name);
			}
			if (placeholders != null)
			{
				item.SetMetadata(ITEMMETADATA_ORMGENERATORTARGET_PLACEHOLDERS, placeholders.Count == 1 ? placeholders[0] : string.Join<string>(" ", placeholders));
			}
		}

		/// <summary>
		/// Items with generator target placeholders have the same signature as items
		/// without placeholders and empty name fields. Both of these are valid combinations and
		/// produce the same file name and signature. This means that an item can switch back and
		/// forth from placeholder to non-placeholder status.
		/// </summary>
		/// <param name="item">The item to update.</param>
		/// <param name="targetInstance">Instance to update. The target item will already match the signature for this instance.</param>
#if VISUALSTUDIO_10_0
		public static void UpdateGeneratorTargetPlaceholders(ProjectItemElement item, GeneratorTarget[] targetInstance)
#else // VISUALSTUDIO_10_0
		public static void UpdateGeneratorTargetPlaceholders(BuildItem item, GeneratorTarget[] targetInstance)
#endif // VISUALSTUDIO_10_0
		{
			List<string> placeholders = null;
			for (int i = 0, count = targetInstance.Length; i < count; ++i)
			{
				GeneratorTarget target = targetInstance[i];
				string name = target.TargetName;
				if (name == null)
				{
					(placeholders ?? (placeholders = new List<string>())).Add(target.TargetType);
				}
			}

			if (placeholders == null)
			{
				// Make sure the placeholders item is removed
				RemoveItemMetaData(item, ITEMMETADATA_ORMGENERATORTARGET_PLACEHOLDERS);
			}
			else
			{
				// Update or add item
				SetItemMetaData(item, ITEMMETADATA_ORMGENERATORTARGET_PLACEHOLDERS, placeholders.Count == 1 ? placeholders[0] : string.Join(" ", placeholders));
			}
		}

		private static List<string> EmptyStringList = new List<string>();
#pragma warning disable CS1587
		/// <summary>
		/// Get generator targets by output format.
		/// </summary>
		/// <param name="generatorNamesByOutputFormat">Dictionary mapping an output format to a specific generator.</param>
		/// <param name="activeTargets">Generator targets retrieved from the active model.</param>
#if VISUALSTUDIO_15_0
		/// <param name="serviceProvider"></param>
#endif
		/// <returns>Target sets keyed by format name. Each target set represents a list of target types used by the format, with
		/// instances pulled from different target combinations.</returns>
		/// <remarks>Generators are not directly tied to an output format. They are
		/// tied to a specific generator that creates that output format. This mapping
		/// is determined before this call.
		/// If the target types used by a generator do not have an entry in <paramref name="activeTargets"/>
		/// then placeholder elements (with a null TargetName) are created for the target types used by the
		/// selected generators.</remarks>
		public static IDictionary<string, GeneratorTargetSet> ExpandGeneratorTargets(IDictionary<string, string> generatorNamesByOutputFormat, IDictionary<string, GeneratorTarget[]> activeTargets
#if VISUALSTUDIO_15_0
			, IServiceProvider serviceProvider
#endif
			)
#pragma warning restore CS1587
		{
			// Filter the generator targets by those actually specified in the project.
			// Target type order is determined by the specified target order and the
			// specified dependency formats, with the source input format given
			// priority over the dependent formats.
			IDictionary<string, IORMGenerator> generatorsByName =
#if VISUALSTUDIO_15_0
				ORMCustomTool.GetORMGenerators(serviceProvider);
#else
				ORMCustomTool.ORMGenerators;
#endif
			Dictionary<string, List<string>> orderedTargetsByOutputFormat = new Dictionary<string, List<string>>();
			foreach (string generatorName in generatorNamesByOutputFormat.Values)
			{
				GetExpandedGeneratorTargets(generatorsByName[generatorName], generatorsByName, generatorNamesByOutputFormat, orderedTargetsByOutputFormat);
			}

			Dictionary<string, GeneratorTargetSet> targetSetsByOutputFormat = null;
			Dictionary<GeneratorTargetSet, GeneratorTargetSet> targetSets = null;
			foreach (KeyValuePair<string, List<string>> pair in orderedTargetsByOutputFormat)
			{
				List<string> targets = pair.Value;
				if (targets == EmptyStringList)
				{
					continue;
				}
				string outputFormat = pair.Key;
				GeneratorTargetSet targetSet = new GeneratorTargetSet(targets);

				if (targetSetsByOutputFormat == null)
				{
					targetSetsByOutputFormat = new Dictionary<string, GeneratorTargetSet>();
					targetSets = new Dictionary<GeneratorTargetSet, GeneratorTargetSet>();
				}

				GeneratorTargetSet existingTargetSet;
				if (targetSets.TryGetValue(targetSet, out existingTargetSet))
				{
					targetSet = existingTargetSet;
				}
				else
				{
					targetSet.Instances = targetSet.PopulateInstances(activeTargets);
				}
				targetSetsByOutputFormat[outputFormat] = targetSet;
			}
			return targetSetsByOutputFormat;
		}
		private static List<string> GetExpandedGeneratorTargets(
			IORMGenerator generator,
			IDictionary<string, IORMGenerator> generatorsByName,
			IDictionary<string, string> generatorNamesByOutputFormat,
			Dictionary<string, List<string>> orderedTargetsByOutputFormat)
		{
			string outputFormat = generator.ProvidesOutputFormat;
			List<string> existingTargets;
			if (orderedTargetsByOutputFormat.TryGetValue(outputFormat, out existingTargets))
			{
				// Do a basic recursion check
				if (existingTargets == null)
				{
					throw new InvalidOperationException("Generators have cyclic dependencies."); // UNDONE: Localize error (should never happen to an end user, but might happen to an extension developer)
				}
				return existingTargets == EmptyStringList ? null : existingTargets;
			}
			// Guard against recursion, will replace later by real data
			orderedTargetsByOutputFormat[outputFormat] = null;
			List<string> targets = null;
			int totalInputTargetCount = 0;
			foreach (string requiredInputFormat in generator.RequiresInputFormats)
			{
				IList<string> inputTargets;
				string generatorName;
				IORMGenerator inputGenerator;
				if (generatorNamesByOutputFormat.TryGetValue(requiredInputFormat, out generatorName) &&
					generatorsByName.TryGetValue(generatorName, out inputGenerator) &&
					null != (inputTargets = GetExpandedGeneratorTargets(
						inputGenerator,
						generatorsByName,
						generatorNamesByOutputFormat,
						orderedTargetsByOutputFormat)) &&
					inputTargets != EmptyStringList)
				{
					// Copy unique items into the list. Make sure we don't have duplication--
					// if two inputs have the same target type then we treat it as a single branch
					// with the name decoration happening on the first item.
					if (totalInputTargetCount == 0)
					{
						targets = new List<string>(inputTargets);
						totalInputTargetCount = targets.Count;
					}
					else
					{
						int inputTargetCount = inputTargets.Count;
						for (int i = 0; i < inputTargetCount; ++i)
						{
							string inputTargetType = inputTargets[i];
							int j = 0;
							for (; j < totalInputTargetCount; ++j)
							{
								if (inputTargetType == targets[j])
								{
									break;
								}
							}
							if (j == totalInputTargetCount)
							{
								targets.Add(inputTargetType);
								// Don't increment the target count until we're through this list,
								// which we already know is unique.
							}
						}
						totalInputTargetCount = targets.Count;
					}
				}
			}
			IList<string> targetTypes = generator.GeneratorTargetTypes;
			if (targetTypes != null &&
				targetTypes.Count != 0)
			{
				foreach (string targetType in targetTypes)
				{
					if (targets == null)
					{
						targets = new List<string>();
						targets.Add(targetType);
					}
					else
					{
						int i = 0;
						for (; i < totalInputTargetCount; ++i)
						{
							if (targetType == targets[i])
							{
								break;
							}
						}
						if (i == totalInputTargetCount)
						{
							targets.Add(targetType);
						}
					}
				}
			}
			if (targets == null)
			{
				targets = EmptyStringList; // Use a non-null value to facilitate recursion tracking
			}
			orderedTargetsByOutputFormat[outputFormat] = targets;
			return targets;
		}
	}
#endregion // ORMCustomToolUtility class
}
