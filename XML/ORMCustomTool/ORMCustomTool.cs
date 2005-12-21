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
		private delegate object get_XmlILCommand(XslCompiledTransform @this);
		private delegate void Execute(object @this, XmlReader contextDocument, XmlResolver dataSources, XsltArgumentList argumentList, Stream results);

		private static get_XmlILCommand GetXmlILCommand;
		private static Execute XmlILCommandExecute;

		static ORMCustomTool()
		{
			Type xmlILCommand = typeof(System.Xml.Xsl.Runtime.XmlQueryRuntime).Assembly.GetType("System.Xml.Xsl.XmlILCommand", true, false);
			
			DynamicMethod getXmlILCommand = new DynamicMethod("GetXmlILCommand", xmlILCommand, new Type[] { typeof(XslCompiledTransform) }, typeof(XslCompiledTransform), true);
			ILGenerator getXmlILCommandIL = getXmlILCommand.GetILGenerator(6);
			getXmlILCommandIL.Emit(OpCodes.Ldarg_0);
			getXmlILCommandIL.Emit(OpCodes.Ldfld, typeof(XslCompiledTransform).GetField("command", BindingFlags.Instance | BindingFlags.NonPublic));
			getXmlILCommandIL.Emit(OpCodes.Ret);
			GetXmlILCommand = (get_XmlILCommand)getXmlILCommand.CreateDelegate(typeof(get_XmlILCommand));

			DynamicMethod execute = new DynamicMethod("Execute", null, new Type[] { typeof(object), typeof(XmlReader), typeof(XmlResolver), typeof(XsltArgumentList), typeof(Stream) }, xmlILCommand, true);
			ILGenerator executeIL = execute.GetILGenerator(12);
			executeIL.Emit(OpCodes.Ldarg_0);
			executeIL.Emit(OpCodes.Ldarg_1);
			executeIL.Emit(OpCodes.Ldarg_2);
			executeIL.Emit(OpCodes.Ldarg_3);
			executeIL.Emit(OpCodes.Ldarg_S, 4);
			executeIL.Emit(OpCodes.Tailcall);
			executeIL.Emit(OpCodes.Call, xmlILCommand.GetMethod("Execute", BindingFlags.Instance | BindingFlags.Public, null, new Type[] { typeof(XmlReader), typeof(XmlResolver), typeof(XsltArgumentList), typeof(Stream) }, null));
			executeIL.Emit(OpCodes.Ret);
			XmlILCommandExecute = (Execute)execute.CreateDelegate(typeof(Execute));
		}

		private static XmlUrlResolver xmlResolver = new XmlUrlResolver();
		private static ORMCustomToolOptionsSerializer optionsSerializer = new ORMCustomToolOptionsSerializer();

		#region CompiledStylesheet
		private struct CompiledStylesheet : IEquatable<CompiledStylesheet>
		{
			public CompiledStylesheet(DateTime lastWriteTimeUtc, object xmlILCommand)
			{
				this.LastWriteTimeUtc = lastWriteTimeUtc;
				this.XmlILCommand = xmlILCommand;
			}

			public readonly DateTime LastWriteTimeUtc;
			public readonly object XmlILCommand;

			public bool Equals(CompiledStylesheet other)
			{
				return (this.LastWriteTimeUtc == other.LastWriteTimeUtc) && (this.XmlILCommand == other.XmlILCommand);
			}
		}
		#endregion

		private static Dictionary<string, CompiledStylesheet> xmlCommands = new Dictionary<string, CompiledStylesheet>(StringComparer.OrdinalIgnoreCase);
		private static Dictionary<string, FileSystemWatcher> fileSystemWatchers = new Dictionary<string, FileSystemWatcher>(StringComparer.OrdinalIgnoreCase);

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
				catch { }
			}

			CompiledStylesheet compiledStylesheet = default(CompiledStylesheet);
			if (xmlCommands.ContainsKey(canonicalUri))
			{
				compiledStylesheet = xmlCommands[canonicalUri];
				if (compiledStylesheet.LastWriteTimeUtc < lastWriteTimeUtc)
				{
					compiledStylesheet = default(CompiledStylesheet);
					xmlCommands.Remove(canonicalUri);
				}
			}
			
			if (compiledStylesheet.XmlILCommand == null)
			{
				XslCompiledTransform xslCompiledTransform = new XslCompiledTransform(false);
				// Note: We should probably mention in the documentation that only trusted stylesheets should be used with this custom tool.
				xslCompiledTransform.Load(canonicalUri, XsltSettings.TrustedXslt, xmlResolver);
				xmlCommands[canonicalUri] = compiledStylesheet = new CompiledStylesheet(lastWriteTimeUtc, GetXmlILCommand(xslCompiledTransform));
				try
				{
					xslCompiledTransform.TemporaryFiles.Delete();
				}
				catch { }
			}
			
			XmlILCommandExecute(compiledStylesheet.XmlILCommand, inputDocument, xmlResolver, null, outputDocument);
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
