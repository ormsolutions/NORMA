#region Common Public License Copyright Notice
/**************************************************************************\
* Neumont Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
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
using Microsoft.Build.BuildEngine;
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

namespace Neumont.Tools.ORM.ORMCustomTool
{
	/// <summary>
	/// <see cref="ORMCustomTool"/> coordinates generation activities between various <see cref="IORMGenerator"/>s, and
	/// interfaces with Visual Studio and other tools.
	/// </summary>
	[Guid("977BD01E-F2B4-4341-9C47-459420624A21")]
	public sealed partial class ORMCustomTool : IVsSingleFileGenerator, IObjectWithSite, IOleServiceProvider, IServiceProvider
	{
		#region Private Constants
		private const string DEFAULT_EXTENSION_DECORATOR = ".ORMCustomToolReport.";
		private const string EXTENSION_ORM = ".orm";
		private const string EXTENSION_XML = ".xml";
		private const string GENERATORS_REGISTRYROOT = @"Software\Neumont\ORM Architect for Visual Studio\Generators";
		private const string ITEMMETADATA_DEPENDENTUPON = "DependentUpon";
		private const string ITEMMETADATA_GENERATOR = "Generator";
		private const string ITEMMETADATA_ORMGENERATOR = "ORMGenerator";
		private const string ITEMMETADATA_AUTOGEN = "AutoGen";
		private const string ITEMMETADATA_DESIGNTIME = "DesignTime";
		private const string ITEMGROUP_CONDITIONSTART = "Exists('";
		private const string ITEMGROUP_CONDITIONEND = "')";
		private const string DEBUG_ERROR_CATEGORY = "ORMCustomTool";
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
		/// <summary>
		/// Retrieve a build item group for the specified project and item
		/// </summary>
		public static BuildItemGroup GetBuildItemGroup(Project project, string projectItemName)
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
		private void GenerateCode(string bstrInputFileContents, string wszDefaultNamespace, IVsGeneratorProgress pGenerateProgress, WriteReportItem report)
		{
			string message =
@"Report file generated by ORMCustomTool.
 Any generation errors will appear as #error lines in this file,
 followed by the exception message and stack trace.";
			report(message, ReportType.Comment, null);

			EnvDTE.ProjectItem projectItem = this.GetService<EnvDTE.ProjectItem>();
			Debug.Assert(projectItem != null);
			string projectItemName = projectItem.Name;

			// If we weren't passed a default namespace, pick one up from the project properties
			if (String.IsNullOrEmpty(wszDefaultNamespace))
			{
				wszDefaultNamespace = projectItem.ContainingProject.Properties.Item("DefaultNamespace").Value as string;
			}

			string projectItemExtension = Path.GetExtension(projectItemName);
			if (!String.Equals(projectItemExtension, EXTENSION_ORM, StringComparison.OrdinalIgnoreCase) && !String.Equals(projectItemExtension, EXTENSION_XML, StringComparison.OrdinalIgnoreCase))
			{
				// UNDONE: Localize message.
				message = "ORMCustomTool is only supported on Object-Role Modeling files, which must have an '.orm' or '.xml' extension.";
				report(message, ReportType.Error, null);
				return;
			}

			// This is actually the full project path for the next couple of lines, and then it is changed to the project directory.
			string projectPath = projectItem.ContainingProject.FullName;
			Project project = Engine.GlobalEngine.GetLoadedProject(projectPath);
			Debug.Assert(project != null);
			pGenerateProgress.Progress(1, 20);

			// Get the relative path of the project item.
			string projectItemFullPath = (string)projectItem.Properties.Item("LocalPath").Value;
			string projectItemRelPath = (new Uri(projectPath)).MakeRelativeUri(new Uri(projectItemFullPath)).ToString();

			BuildItemGroup ormBuildItemGroup = GetBuildItemGroup(project, projectItemRelPath);
			pGenerateProgress.Progress(1, 10);

			if (ormBuildItemGroup == null || ormBuildItemGroup.Count <= 0)
			{
				// UNDONE: Localize message.
				message = "No BuildItemGroup was found for this ORM file. Use the ORMGeneratorSettings dialog to add items to the group, or clear the CustomTool property.";
				report(message, ReportType.Warning, null);
				return;
			}
			else
			{
				List<BuildItem> ormBuildItems = new List<BuildItem>(ormBuildItemGroup.Count - 1);
				foreach (BuildItem buildItem in ormBuildItemGroup)
				{
					string ormGeneratorName = buildItem.GetEvaluatedMetadata(ITEMMETADATA_ORMGENERATOR);
					if (!String.IsNullOrEmpty(ormGeneratorName) && String.Equals(buildItem.GetEvaluatedMetadata(ITEMMETADATA_DEPENDENTUPON), projectItemName, StringComparison.OrdinalIgnoreCase))
					{
						ormBuildItems.Add(buildItem);
					}
				}
				pGenerateProgress.Progress(1, 5);

				Dictionary<string, Stream> outputFormatStreams = new Dictionary<string, Stream>(ormBuildItems.Count + 2, StringComparer.OrdinalIgnoreCase);
				ReadOnlyDictionary<string, Stream> readonlyOutputFormatStreams = new ReadOnlyDictionary<string, Stream>(outputFormatStreams);

				// Get a Stream for the input ORM file...
				Stream ormStream = null;
				EnvDTE.Document projectItemDocument = projectItem.Document;
				if (projectItemDocument != null)
				{
					if ((ormStream = projectItemDocument.Object("ORMXmlStream") as Stream) == null)
					{
						EnvDTE.TextDocument projectItemTextDocument = projectItemDocument.Object("TextDocument") as EnvDTE.TextDocument;
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

				// Add the input ORM file Stream...
				outputFormatStreams.Add(ORMOutputFormat.ORM, new ReadOnlyStream(ormStream));

				// Null out bstrInputFileContents to prevent its usage beyond this point.
				bstrInputFileContents = null;

				uint ormBuildItemsCount = (uint)ormBuildItems.Count;
				uint progressCurrent = (uint)(ormBuildItemsCount * 0.25);
				uint progressTotal = ormBuildItemsCount + progressCurrent + 3;
				pGenerateProgress.Progress(++progressCurrent, progressTotal);

				// Execute the rest of the generators.
				// We limit this to 100 iterations in order to avoid an infinite loop if no BuiltItem exists that provides
				// the format required by one of the BuildItems that do exist.
				for (int i = 0; ormBuildItems.Count > 0 && i < 100; ++i)
				{
				LABEL_START_INNER_LOOP:
					// TODO: This would perform *much* better if we did real dependency analysis and scheduling for RequiresInputFormats
					for (int j = 0; j < ormBuildItems.Count; ++j)
					{
						BuildItem buildItem = ormBuildItems[j];
						IORMGenerator ormGenerator;
						if (!ORMCustomTool.ORMGenerators.TryGetValue(buildItem.GetEvaluatedMetadata(ITEMMETADATA_ORMGENERATOR), out ormGenerator))
						{
							// UNDONE: Localize error message.
							message = string.Format(CultureInfo.InvariantCulture, "#error Skipping generation of '{0}' because IORMGenerator '{1}' could not be found.", buildItem.FinalItemSpec, buildItem.GetEvaluatedMetadata(ITEMMETADATA_ORMGENERATOR));
							report(message, ReportType.Error, null);
							ormBuildItems.Remove(buildItem);
							pGenerateProgress.Progress(++progressCurrent, progressTotal);
							// Because we removed buildItem, we need to restart the loop...
							goto LABEL_START_INNER_LOOP;
						}
						try
						{
							IList<string> requiresInputFormats = ormGenerator.RequiresInputFormats;
							bool missingInputFormat = false;
							foreach (string inputFormat in requiresInputFormats)
							{
								if (!outputFormatStreams.ContainsKey(inputFormat))
								{
									missingInputFormat = true;
									break;
								}
							}
							if (missingInputFormat)
							{
								// Go on to the next generator, we'll (probably) come back to this one
								continue;
							}
							else
							{
								MemoryStream outputStream = new MemoryStream();
								Stream readonlyOutputStream = null;
								try
								{
									ormGenerator.GenerateOutput(buildItem, outputStream, readonlyOutputFormatStreams, wszDefaultNamespace);
									readonlyOutputStream = new ReadOnlyStream(outputStream);
								}
								catch (Exception ex)
								{
									// UNDONE: Localize error messages.
									message = string.Format(CultureInfo.InvariantCulture, "Exception occurred while executing transform '{0}'. The existing contents of '{1}' will not be modified.", ormGenerator.OfficialName, buildItem.FinalItemSpec);
									report(message, ReportType.Error, ex);
								}

								string fullItemPath = Path.Combine(Path.GetDirectoryName(projectPath), buildItem.FinalItemSpec);
								bool textLinesReloadRequired;
								IVsTextLines textLines = GetTextLinesForDocument(fullItemPath, out textLinesReloadRequired);
								// Write the result out to the appropriate file...
								int outputStreamLength = (int)outputStream.Length;
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

									// Reset outputStream to the beginning of the stream...
									outputStream.Seek(0, SeekOrigin.Begin);

									if (readonlyOutputStream != null)
									{
										// We're using the readonlyOutputStream here so that the StreamReader can't close the real stream
										using (StreamReader streamReader = new StreamReader(readonlyOutputStream, Encoding.UTF8, true, (int)outputStream.Length))
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
									// The transform failed and the file is not loaded in the
									// environment. Attempt to load it from disk. The output
									// stream is no longer needed, shut it down now.
									outputStream.Close();
									if (File.Exists(fullItemPath))
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
										fileStream.Write(outputStream.GetBuffer(), 0, (int)outputStream.Length);
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
									outputFormatStreams.Add(ormGenerator.ProvidesOutputFormat, readonlyOutputStream);
									// Reset outputStream to the beginning of the stream...
									readonlyOutputStream.Seek(0, SeekOrigin.Begin);
								}

								ormBuildItems.Remove(buildItem);
								pGenerateProgress.Progress(++progressCurrent, progressTotal);
								// Because we removed buildItem, we need to restart the loop...
								goto LABEL_START_INNER_LOOP;
							}
						}
						catch (Exception ex)
						{
							// UNDONE: Localize error message.
							message = string.Format(CultureInfo.InvariantCulture, "Error occurred during generation of '{0}' via IORMGenerator '{1}'.", buildItem.FinalItemSpec, ormGenerator.OfficialName);
							report(message, ReportType.Error, ex);
							ormBuildItems.Remove(buildItem);
							pGenerateProgress.Progress(++progressCurrent, progressTotal);
							// Because we removed buildItem, we need to restart the loop...
							goto LABEL_START_INNER_LOOP;
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
}
