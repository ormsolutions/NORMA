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

using IServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;
using VSConstants = Microsoft.VisualStudio.VSConstants;
using Debug = System.Diagnostics.Debug;
using DebuggerStepThroughAttribute = System.Diagnostics.DebuggerStepThroughAttribute;

namespace Neumont.Tools.ORM.ORMCustomTool
{
	[Guid("977BD01E-F2B4-4341-9C47-459420624A21")]
	public sealed partial class ORMCustomTool : IVsSingleFileGenerator, IObjectWithSite, IServiceProvider, System.IServiceProvider
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

		private static void ReportError(string message, Exception ex)
		{
			Debug.WriteLine(message);
			Debug.Indent();
			Debug.WriteLine(ex);
			Debug.Unindent();
		}

		public ORMCustomTool()
		{
			// NOTE: Attempting to use the ServiceProvider will cause it to go into an infinite loop
			// unless SetSite has been called on it.
			this._objectWithSite = this._serviceProvider = new ServiceProvider(this);
		}

		private readonly ServiceProvider _serviceProvider;
		private readonly IObjectWithSite _objectWithSite;

		public static System.ComponentModel.PropertyDescriptor GetPropertyDescriptor()
		{
			return new ORMCustomToolPropertyDescriptor();
		}

		/// <summary>
		/// Returns a service instance of type <typeparamref name="T"/>, or <see langword="null"/> if no service instance of
		/// type <typeparamref name="T"/> is available.
		/// </summary>
		/// <typeparam name="T">The type of the service instance being requested.</typeparam>
		public T GetService<T>() where T : class
		{
			return this._serviceProvider.GetService(typeof(T)) as T;
		}

		private static BuildItemGroup GetBuildItemGroup(Project project, string projectItemName)
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

		#region IObjectWithSite Members

		void IObjectWithSite.GetSite(ref Guid riid, out IntPtr ppvSite)
		{
			this._objectWithSite.GetSite(ref riid, out ppvSite);
		}

		void IObjectWithSite.SetSite(object pUnkSite)
		{
			this._objectWithSite.SetSite(pUnkSite);
		}		

		#endregion // IObjectWithSite Members

		#region IServiceProvider Members

		int Microsoft.VisualStudio.OLE.Interop.IServiceProvider.QueryService(ref Guid guidService, ref Guid riid, out IntPtr ppvObject)
		{
			return this.GetService<IServiceProvider>().QueryService(ref guidService, ref riid, out ppvObject);
		}

		object System.IServiceProvider.GetService(Type serviceType)
		{
			return this._serviceProvider.GetService(serviceType);
		}

		#endregion // IServiceProvider Members

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
				ormToOialGenerator.GenerateOutput(ormToOialItem, oialStream, readonlyOutputFormatStreams);
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
				START_INNER_LOOP:
					// TODO: This would perform *much* better if we did real dependency analysis and scheduling for RequiresInputFormats
					for (int j = 0; j < ormBuildItems.Count; j++)
					{
						BuildItem buildItem = ormBuildItems[j];
						IORMGenerator ormGenerator = ORMCustomTool.ORMGenerators[buildItem.GetEvaluatedMetadata(ITEMMETADATA_ORMGENERATOR)];
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
								ormGenerator.GenerateOutput(buildItem, outputStream, readonlyOutputFormatStreams);
							}
							catch (Exception ex)
							{
								// TODO: Localize error message.
								byte[] errorOutput = Encoding.UTF8.GetBytes(String.Concat("Error while executing transform: \"", ormGenerator.OfficialName, "\".", Environment.NewLine, Environment.NewLine, ex.ToString()));
								outputStream.Write(errorOutput, 0, errorOutput.Length);
							}
							outputFormatStreams.Add(ormGenerator.ProvidesOutputFormat, outputStream);
							int outputStreamLength = (int)outputStream.Length;
							// Write the result out to the appropriate file...
							using (FileStream fileStream = File.Create(Path.Combine(projectDirectory, buildItem.FinalItemSpec), outputStreamLength, FileOptions.SequentialScan))
							{
								fileStream.Write(outputStream.GetBuffer(), 0, outputStreamLength);
							
							}
							// Reset outputStream to the beginning of the stream...
							outputStream.Seek(0, SeekOrigin.Begin);
							ormBuildItems.Remove(buildItem);
							pGenerateProgress.Progress(++progressCurrent, progressTotal);
							// Because we removed buildItem, we need to restart the loop...
							goto START_INNER_LOOP;
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
