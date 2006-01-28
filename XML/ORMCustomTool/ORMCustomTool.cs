using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell.Interop;
using Neumont.Tools.ORM.ORMCustomTool.Serialization;

using VSConstants = Microsoft.VisualStudio.VSConstants;
using Marshal = System.Runtime.InteropServices.Marshal;

namespace Neumont.Tools.ORM.ORMCustomTool
{
	[System.Runtime.InteropServices.Guid("977BD01E-F2B4-4341-9C47-459420624A21")]
	public class ORMCustomTool : IVsSingleFileGenerator, IObjectWithSite
	{
		private static readonly XmlUrlResolver xmlResolver = new XmlUrlResolver();
		private static readonly ORMCustomToolOptionsSerializer optionsSerializer = new ORMCustomToolOptionsSerializer();

		#region CompiledStylesheet
		private struct CompiledStylesheet : IEquatable<CompiledStylesheet>
		{
			public CompiledStylesheet(DateTime lastWriteTimeUtc, XslCompiledTransform transform)
			{
				this.LastWriteTimeUtc = lastWriteTimeUtc;
				this.Transform = transform;
			}

			public readonly DateTime LastWriteTimeUtc;
			public readonly XslCompiledTransform Transform;

			public bool Equals(CompiledStylesheet other)
			{
				return (this.LastWriteTimeUtc == other.LastWriteTimeUtc) && (this.Transform == other.Transform);
			}
		}
		#endregion

		private static readonly Dictionary<string, CompiledStylesheet> compiledStylesheets = new Dictionary<string, CompiledStylesheet>(StringComparer.OrdinalIgnoreCase);
		private static readonly Dictionary<string, FileSystemWatcher> fileSystemWatchers = new Dictionary<string, FileSystemWatcher>(StringComparer.OrdinalIgnoreCase);

		private static Uri ResolveUri(string uri)
		{
			return xmlResolver.ResolveUri(null, uri);
		}

		internal static void Transform(string stylesheetUri, XmlReader inputDocument, Stream outputDocument)
		{
			Uri uri = ResolveUri(stylesheetUri);
			string canonicalUri = uri.ToString();
	
			DateTime lastWriteTimeUtc = default(DateTime);
			if (uri.IsFile || uri.IsUnc)
			{
				try
				{
					FileInfo fileInfo = new FileInfo(uri.OriginalString);
					lastWriteTimeUtc = fileInfo.LastWriteTimeUtc;
				}
				catch { /* Not being able to get the LastWriteTime of the file isn't a problem... */ }
			}

			CompiledStylesheet compiledStylesheet = default(CompiledStylesheet);
			if (compiledStylesheets.ContainsKey(canonicalUri))
			{
				compiledStylesheet = compiledStylesheets[canonicalUri];
				if (compiledStylesheet.LastWriteTimeUtc < lastWriteTimeUtc)
				{
					compiledStylesheet = default(CompiledStylesheet);
					compiledStylesheets.Remove(canonicalUri);
				}
			}
			
			if (compiledStylesheet.Transform == null)
			{
				XslCompiledTransform xslCompiledTransform = new XslCompiledTransform(false);
				// Note: We should probably mention in the documentation that only trusted stylesheets should be used with this custom tool.
				xslCompiledTransform.Load(canonicalUri, XsltSettings.TrustedXslt, xmlResolver);
				compiledStylesheets[canonicalUri] = compiledStylesheet = new CompiledStylesheet(lastWriteTimeUtc, xslCompiledTransform);
				try
				{
					xslCompiledTransform.TemporaryFiles.Delete();
				}
				catch { /* Not being able to delete the temporary files isn't a problem... */ }
			}

			compiledStylesheet.Transform.Transform(inputDocument, null, outputDocument);
		}

		#region IVsSingleFileGenerator Members

		private string _defaultExtension;

		public int DefaultExtension(out string pbstrDefaultExtension)
		{
			pbstrDefaultExtension = this._defaultExtension;
			return VSConstants.S_OK;
		}

		[CLSCompliant(false)]
		public int Generate(string wszInputFilePath, string bstrInputFileContents, string wszDefaultNamespace, IntPtr[] rgbOutputFileContents, out uint pcbOutput, IVsGeneratorProgress pGenerateProgress)
		{
			#region ParameterValidation
			if (String.IsNullOrEmpty(bstrInputFileContents))
			{
				if (!String.IsNullOrEmpty(wszInputFilePath))
				{
					bstrInputFileContents = File.ReadAllText(wszInputFilePath);
				}
			}
			if (String.IsNullOrEmpty(bstrInputFileContents))
			{
				pcbOutput = 0;
				return VSConstants.E_INVALIDARG;
			}
			#endregion

			ORMCustomToolOptions options;
			using (StringReader reader = new StringReader(bstrInputFileContents))
			{
				options = optionsSerializer.Deserialize(reader) as ORMCustomToolOptions;
			}

			this._defaultExtension = options.OutputFileExtension;

			byte[] buffer;
			using (Stream output = options.Transformation.GetOutput())
			{
				buffer = new byte[output.Length];
				output.Read(buffer, 0, (int)output.Length);
			}
			IntPtr outputFileContents = Marshal.AllocCoTaskMem(buffer.Length);
			Marshal.Copy(buffer, 0, outputFileContents, buffer.Length);
			rgbOutputFileContents[0] = outputFileContents;
			pcbOutput = (uint)buffer.Length;

			return VSConstants.S_OK;
		}

		#endregion

		#region IObjectWithSite Members

		public void GetSite(ref Guid riid, out IntPtr ppvSite)
		{
			ppvSite = IntPtr.Zero;
		}

		public void SetSite(object pUnkSite)
		{
			Microsoft.VisualStudio.OLE.Interop.IServiceProvider serviceProvider = pUnkSite as Microsoft.VisualStudio.OLE.Interop.IServiceProvider;
			
			EnvDTE.ProjectItem projItem = null;
			Guid projectItemGuid = typeof(EnvDTE.ProjectItem).GUID;
			IntPtr ptr;
			serviceProvider.QueryService(ref projectItemGuid, ref projectItemGuid, out ptr);
			if (ptr != IntPtr.Zero)
			{
				projItem = Marshal.GetObjectForIUnknown(ptr) as EnvDTE.ProjectItem;
				Marshal.Release(ptr);
			}

			if (projItem != null)
			{
				Directory.SetCurrentDirectory(Path.GetDirectoryName(projItem.get_FileNames(0)));
			}
		}

		#endregion
	}
}
