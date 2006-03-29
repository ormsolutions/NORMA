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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Build.BuildEngine;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
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
		private const string EXTENSION_OIAL = ".OIAL" + EXTENSION_XML;
		private const string EXTENSION_ORM = ".orm";
		private const string EXTENSION_XML = ".xml";
		private const string GENERATORS_REGISTRYROOT = @"Software\Neumont\ORM Architect for Visual Studio\Generators";
		private const string GENERATOR_ORMTOOIAL = "ORMtoOIAL";
		private const string ITEMMETADATA_DEPENDENTUPON = "DependentUpon";
		private const string ITEMMETADATA_ORMGENERATOR = "ORMGenerator";
		private const string ITEMMETADATA_AUTOGEN = "AutoGen";
		private const string ITEMMETADATA_DESIGNTIME = "DesignTime";
		private const string ITEMGROUP_CONDITIONSTART = "Exists('";
		private const string ITEMGROUP_CONDITIONEND = "')";
		private const string DEBUG_ERROR_CATEGORY = "ORMCustomTool";

		/// <summary>
		/// Instantiates a new instance of <see cref="ORMCustomTool"/>.
		/// </summary>
		public ORMCustomTool()
		{
			// NOTE: Attempting to use any of the ServiceProviders will cause us to go into an infinite loop
			// unless SetSite has been called on us.
			this._objectWithSite = this._serviceProvider = new ServiceProvider(this, true);
		}

		private readonly ServiceProvider _serviceProvider;
		private readonly IObjectWithSite _objectWithSite;
		private IOleServiceProvider _customToolServiceProvider;
		private IOleServiceProvider _dteServiceProvider;

		/// <summary>
		/// Returns a service instance of type <typeparamref name="T"/>, or <see langword="null"/> if no service instance of
		/// type <typeparamref name="T"/> is available.
		/// </summary>
		/// <typeparam name="T">The type of the service instance being requested.</typeparam>
		public T GetService<T>() where T : class
		{
			return this._serviceProvider.GetService(typeof(T)) as T;
		}

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

		private IVsTextView GetTextViewForDocument(string fullPath)
		{
			IVsUIHierarchy uiHierarchy;
			uint itemId;
			IVsWindowFrame windowFrame;
			if (VsShellUtilities.IsDocumentOpen(this, fullPath, Guid.Empty, out uiHierarchy, out itemId, out windowFrame))
			{
				return VsShellUtilities.GetTextView(windowFrame);
			}
			return null;
		}
		private static IVsTextLines GetTextLinesForTextView(IVsTextView textView)
		{
			if (textView == null)
			{
				throw new ArgumentNullException("textView");
			}
			IVsTextLines textLines;
			ErrorHandler.ThrowOnFailure(textView.GetBuffer(out textLines));
			return textLines;
		}

		#endregion // Private Helper Methods

		#region ServiceProvider Interface Implementations

		#region IObjectWithSite Members

		void IObjectWithSite.GetSite(ref Guid riid, out IntPtr ppvSite)
		{
			this._objectWithSite.GetSite(ref riid, out ppvSite);
		}

		void IObjectWithSite.SetSite(object pUnkSite)
		{
			IOleServiceProvider customToolServiceProvider = pUnkSite as IOleServiceProvider;
			if (customToolServiceProvider != null)
			{
				this._customToolServiceProvider = customToolServiceProvider;
				EnvDTE.ProjectItem projectItem = this.GetService<EnvDTE.ProjectItem>();
				IOleServiceProvider dteServiceProvider = null;
				if (projectItem != null && (dteServiceProvider = projectItem.DTE as IOleServiceProvider) != null)
				{
					this._dteServiceProvider = dteServiceProvider;
				}
			}
		}		

		#endregion // IObjectWithSite Members

		#region IOleServiceProvider Members

		int IOleServiceProvider.QueryService(ref Guid guidService, ref Guid riid, out IntPtr ppvObject)
		{
			IOleServiceProvider customToolServiceProvider = this._customToolServiceProvider;
			IOleServiceProvider dteServiceProvider = this._dteServiceProvider;

			if (customToolServiceProvider == null)
			{
				throw new InvalidOperationException();
			}

			// First try to service the request via the IOleServiceProvider we were given. If unsuccessful, try via DTE's
			// IOleServiceProvider implementation (if we have it).
			int errorCode = this._customToolServiceProvider.QueryService(ref guidService, ref riid, out ppvObject);
			if (dteServiceProvider == null || (ErrorHandler.Succeeded(errorCode) && ppvObject != IntPtr.Zero))
			{
				return errorCode;
			}
			else
			{
				return this._dteServiceProvider.QueryService(ref guidService, ref riid, out ppvObject);
			}
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

		int IVsSingleFileGenerator.DefaultExtension(out string pbstrDefaultExtension)
		{
			pbstrDefaultExtension = EXTENSION_OIAL;
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
				// TODO: Localize message.
				pGenerateProgress.GeneratorError(0, 0, "ORMCustomTool is only supported on Object-Role Modeling files, which must have the extension \"" + EXTENSION_ORM + "\" or \"" + EXTENSION_XML + "\".", uint.MaxValue, uint.MaxValue);
				rgbOutputFileContents[0] = IntPtr.Zero;
				pcbOutput = 0;
				return VSConstants.E_FAIL;
			}

			// This is actually the full project path for the next couple of lines, and then it is changed to the project directory.
			string projectDirectory = projectItem.ContainingProject.FullName;
			Project project = Engine.GlobalEngine.GetLoadedProject(projectDirectory);
			Debug.Assert(project != null);
			projectDirectory = Path.GetDirectoryName(projectDirectory);
			pGenerateProgress.Progress(1, 20);

			BuildItemGroup ormBuildItemGroup = GetBuildItemGroup(project, projectItemName);
			pGenerateProgress.Progress(1, 10);

			if (ormBuildItemGroup == null || ormBuildItemGroup.Count <= 0)
			{
				// TODO: Localize message.
				pGenerateProgress.GeneratorError(1, 0, "No BuildItemGroup was found for this ORM file.", uint.MaxValue, uint.MaxValue);
				pcbOutput = 0;
				return VSConstants.S_OK;
			}
			else
			{
				BuildItem ormToOialItem = null;
				List<BuildItem> ormBuildItems = new List<BuildItem>(ormBuildItemGroup.Count - 1);
				foreach (BuildItem buildItem in ormBuildItemGroup)
				{
					string ormGeneratorName = buildItem.GetEvaluatedMetadata(ITEMMETADATA_ORMGENERATOR);
					if (!String.IsNullOrEmpty(ormGeneratorName) && String.Equals(buildItem.GetEvaluatedMetadata(ITEMMETADATA_DEPENDENTUPON), projectItemName, StringComparison.OrdinalIgnoreCase))
					{
						if (String.Equals(ormGeneratorName, GENERATOR_ORMTOOIAL, StringComparison.OrdinalIgnoreCase))
						{
							ormToOialItem = buildItem;
						}
						else
						{
							ormBuildItems.Add(buildItem);
						}
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

				// Execute the ORMtoOIAL generator first, since it should always be present,
				// and almost everything else will depend on it...
				MemoryStream oialStream = new MemoryStream();
				IORMGenerator ormToOialGenerator = ORMCustomTool.ORMGenerators[GENERATOR_ORMTOOIAL];
				ormToOialGenerator.GenerateOutput(ormToOialItem, oialStream, readonlyOutputFormatStreams, wszDefaultNamespace);
				// Reset oialStream to the beginning of the stream...
				oialStream.Seek(0, SeekOrigin.Begin);
				outputFormatStreams.Add(ormToOialGenerator.ProvidesOutputFormat, new ReadOnlyStream(oialStream));

				uint ormBuildItemsCount = (uint)ormBuildItems.Count;
				uint progressCurrent = (uint)(ormBuildItemsCount * 0.25);
				uint progressTotal = ormBuildItemsCount + progressCurrent + 3;
				pGenerateProgress.Progress(++progressCurrent, progressTotal);

				// Execute the rest of the generators.
				// We limit this to 100 iterations in order to avoid an infinite loop if no BuiltItem exists that provides
				// the format required by one of the BuildItems that do exist.
				for (int i = 0; ormBuildItems.Count > 0 && i < 100; i++)
				{
				LABEL_START_INNER_LOOP:
					// TODO: This would perform *much* better if we did real dependency analysis and scheduling for RequiresInputFormats
					for (int j = 0; j < ormBuildItems.Count; j++)
					{
						BuildItem buildItem = ormBuildItems[j];
						IORMGenerator ormGenerator;
						if (!ORMCustomTool.ORMGenerators.TryGetValue(buildItem.GetEvaluatedMetadata(ITEMMETADATA_ORMGENERATOR), out ormGenerator))
						{
							// TODO: Localize error message.
							pGenerateProgress.GeneratorError(1, 0, String.Concat("Skipping generation of \"", buildItem.FinalItemSpec, "\" because IORMGenerator \"", buildItem.GetEvaluatedMetadata(ITEMMETADATA_ORMGENERATOR), "\" could not be found."),uint.MaxValue, uint.MaxValue);
							ormBuildItems.Remove(buildItem);
							pGenerateProgress.Progress(++progressCurrent, progressTotal);
							// Because we removed buildItem, we need to restart the loop...
							goto LABEL_START_INNER_LOOP;
						}
						try
						{
							ReadOnlyCollection<string> requiresInputFormats = ormGenerator.RequiresInputFormats;
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
								try
								{
									ormGenerator.GenerateOutput(buildItem, outputStream, readonlyOutputFormatStreams, wszDefaultNamespace);
								}
								catch (Exception ex)
								{
									// TODO: Localize error messages.
									pGenerateProgress.GeneratorError(1, 0, String.Concat("Error occurred while executing transform \"", ormGenerator.OfficialName, "\". See \"", buildItem.FinalItemSpec, "\" for more information."), uint.MaxValue, uint.MaxValue);
									byte[] errorOutput = Encoding.UTF8.GetBytes(String.Concat("Error while executing transform: \"", ormGenerator.OfficialName, "\".", Environment.NewLine, Environment.NewLine, ex.ToString()));
									outputStream.Write(errorOutput, 0, errorOutput.Length);
								}
								Stream readonlyOutputStream = new ReadOnlyStream(outputStream);
								outputFormatStreams.Add(ormGenerator.ProvidesOutputFormat, readonlyOutputStream);

								// Write the result out to the appropriate file...
								int outputStreamLength = (int)outputStream.Length;
								string fullItemPath = Path.Combine(projectDirectory, buildItem.FinalItemSpec);
								IVsTextView textView = this.GetTextViewForDocument(fullItemPath);
								IVsTextLines textLines;
								if (textView != null && (textLines = GetTextLinesForTextView(textView)) != null)
								{
									object editPointStartObject;
									ErrorHandler.ThrowOnFailure(textLines.CreateEditPoint(0, 0, out editPointStartObject));
									EnvDTE.EditPoint editPointStart = editPointStartObject as EnvDTE.EditPoint;
									Debug.Assert(editPointStart != null);
									EnvDTE.EditPoint editPointEnd = editPointStart.CreateEditPoint();
									editPointEnd.EndOfDocument();
									// Reset outputStream to the beginning of the stream...
									outputStream.Seek(0, SeekOrigin.Begin);
									// We're using the readonlyOutputStream here so that the StreamReader can't close the real stream
									using (StreamReader streamReader = new StreamReader(readonlyOutputStream, Encoding.UTF8, true, outputStreamLength))
									{
										// We're not passing any flags to ReplaceText, because the output of the generators should
										// be the same whether or not the user has the generated document open
										editPointStart.ReplaceText(editPointEnd, streamReader.ReadToEnd(), 0);
									}
									VsShellUtilities.SaveFileIfDirty(textView);
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

								// Reset outputStream to the beginning of the stream...
								outputStream.Seek(0, SeekOrigin.Begin);
								ormBuildItems.Remove(buildItem);
								pGenerateProgress.Progress(++progressCurrent, progressTotal);
								// Because we removed buildItem, we need to restart the loop...
								goto LABEL_START_INNER_LOOP;
							}
						}
						catch (Exception ex)
						{
							// TODO: Localize error message.
							string errorMessage = String.Concat("Error occurred during generation of \"", buildItem.FinalItemSpec, "\" (via IORMGenerator \"", ormGenerator.OfficialName, "\"):", Environment.NewLine, Environment.NewLine, ex.ToString());
							Debug.WriteLine(errorMessage, DEBUG_ERROR_CATEGORY);
							pGenerateProgress.GeneratorError(1, 0, errorMessage, uint.MaxValue, uint.MaxValue);
							ormBuildItems.Remove(buildItem);
							pGenerateProgress.Progress(++progressCurrent, progressTotal);
							// Because we removed buildItem, we need to restart the loop...
							goto LABEL_START_INNER_LOOP;
						}
					}
				}

				int oialLength = (int)oialStream.Length;
				IntPtr outputFileContents = Marshal.AllocCoTaskMem(oialLength);
				Marshal.Copy(oialStream.GetBuffer(), 0, outputFileContents, oialLength);
				rgbOutputFileContents[0] = outputFileContents;
				pcbOutput = (uint)oialLength;

				pGenerateProgress.Progress(++progressCurrent, progressTotal);

				foreach (Stream stream in outputFormatStreams.Values)
				{
					ReadOnlyStream readOnlyStream;
					if ((readOnlyStream = stream as ReadOnlyStream) != null)
					{
						readOnlyStream.CloseBackingStream();
					}
				}
				
				pGenerateProgress.Progress(progressTotal, progressTotal);

				return VSConstants.S_OK;
			}
		}

		#endregion // IVsSingleFileGenerator Members
	}
}
