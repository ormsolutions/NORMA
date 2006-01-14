using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Shell.Interop;
using MsOle = Microsoft.VisualStudio.OLE.Interop;
using System.Runtime.InteropServices;
using System.IO;
using Microsoft.VisualStudio;
using System.Text;
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.EnterpriseTools.Corona.Concepts;
using Microsoft.VisualStudio.EnterpriseTools.Corona.Serialization;
using Microsoft.VisualStudio.EnterpriseTools.Phoenix.ObjectModel;
using System.Reflection;

namespace Neumont.Tools.Converters
{
	/// <summary>
	/// A custom tool to replace the ImsToDmd.exe tool provided by Microsoft
	/// in earlier drops of the DSLTools SDK.
	/// Generates a .dsldm file from a .ims file when applied as a custom tool
	/// on an .ims file in the Solution Explorer.
	/// </summary>
	[Guid("a4febd86-790b-43a9-a6e7-2813886ab0d5")]
	public sealed class ImsToDmdCustomTool : IVsSingleFileGenerator, MsOle.IObjectWithSite
	{
		#region Member Variables
		private MsOle.IServiceProvider myServiceProvider;
		private const string ModelFileExtension = ".dsldm";
		#endregion // Member Variables
		#region Constructor
		/// <summary>
		/// Public constructor
		/// </summary>
		public ImsToDmdCustomTool()
		{
		}
		#endregion // Constructor
		#region IVsSingleFileGenerator Implementation
		int IVsSingleFileGenerator.DefaultExtension(out string pbstrDefaultExtension)
		{
			pbstrDefaultExtension = ModelFileExtension;
			return VSConstants.S_OK;
		}
		int IVsSingleFileGenerator.Generate(string wszInputFilePath, string bstrInputFileContents, string wszDefaultNamespace, IntPtr[] rgbOutputFileContents, out uint pcbOutput, IVsGeneratorProgress pGenerateProgress)
		{
			pGenerateProgress.Progress(1, 4);
			byte[] bytes = Encoding.UTF8.GetBytes(GenerateDmd(bstrInputFileContents));
			pGenerateProgress.Progress(3, 4);
			byte[] preamble = Encoding.UTF8.GetPreamble();
			int bufferLength = bytes.Length + preamble.Length;
			IntPtr pBuffer = Marshal.AllocCoTaskMem(bufferLength);
			Marshal.Copy(preamble, 0, pBuffer, preamble.Length);
			Marshal.Copy(bytes, 0, (IntPtr)((uint)pBuffer + preamble.Length), bytes.Length);
			rgbOutputFileContents[0] = pBuffer;
			pcbOutput = (uint)bufferLength;
			return VSConstants.S_OK;
		}
		#endregion // IVsSingleFileGenerator Implementation
		#region IObjectWithSite Implementation
		void MsOle.IObjectWithSite.GetSite(ref Guid riid, out IntPtr ppvSite)
		{
			ppvSite = IntPtr.Zero;
		}
		void MsOle.IObjectWithSite.SetSite(object punkSite)
		{
			myServiceProvider = punkSite as MsOle.IServiceProvider;
		}
		#endregion // IObjectWithSite Implementation
		#region CustomTool Specific
		/// <summary>
		/// Generate a dmd file for the current ims file contents. Note that this
		/// takes a contents instead of a file name so that we don't have to save
		/// the file to regenerate the .dmd.
		/// </summary>
		/// <param name="imsContents">Contents of an ims file</param>
		/// <returns>Contents of the corresponding dmd file</returns>
		private string GenerateDmd(string imsContents)
		{
			Store store = CreateStore();
			using (MemoryStream contentsStream = new MemoryStream())
			{
				using (StreamWriter writer = new StreamWriter(contentsStream))
				{
					// Get the contents into a stream for the ImsReader API
					writer.Write(imsContents);
					writer.Flush();
					// Note: don't close the writer yet, we'll recycle the stream
					
					// Set the stream back to the beginning and read it in
					contentsStream.Position = 0;
					ImsReader.Load(store, contentsStream, false);

					// Resuse the stream to save the store back into
					contentsStream.Position = 0;
					DmdWriter.Save(store, contentsStream);
					// If the saved stream is smaller than the starting stream
					// then the length will be wrong
					contentsStream.SetLength(contentsStream.Position);

					// Dump it back out as a string
					contentsStream.Position = 0;
					using (StreamReader reader = new StreamReader(contentsStream))
					{
						return reader.ReadToEnd();
					}
				}
			}
		}
		private static Store CreateStore()
		{
			Store store = new Store();
			Type[] substoreTypes = new Type[] { typeof(CoreDesignSurface), typeof(CoronaConcepts), typeof(PhoenixMetaModel) };
			int substoreTypeCount = substoreTypes.Length;
			store.LoadMetaModels(substoreTypes);
			object[] createParams = new object[]{store};
			Type[] ctorArgs = new Type[]{typeof(Store)};
			for (int i = 0; i < substoreTypeCount; ++i)
			{
				substoreTypes[i].GetConstructor(ctorArgs).Invoke(createParams);
			}
			return store;
		}
		#endregion // CustomTool Specific
	}
}
