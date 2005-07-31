using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Shell.Interop;
using MsOle = Microsoft.VisualStudio.OLE.Interop;
using System.Runtime.InteropServices;
using System.IO;
using Microsoft.VisualStudio;
using System.Text;
using System.Diagnostics;

namespace Neumont.Tools.Converters
{
	/// <summary>
	/// A custom tool to shell the Microsoft-provided ImsToDmd.exe tool.
	/// Generates an .dmd file from a .ims file when applied as a custom tool
	/// on an .ims file in the Solution Explorer.
	/// </summary>
	[Guid("a4febd86-790b-43a9-a6e7-2813886ab0d5")]
	public sealed class ImsToDmdCustomTool : IVsSingleFileGenerator, MsOle.IObjectWithSite
	{
		#region Member Variables
		private MsOle.IServiceProvider myServiceProvider;
		private static string mysInstallDirectory;
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
			string s = InstallDirectory;
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
		private static object mysLockObject;
		private static object LockObject
		{
			get
			{
				if (mysLockObject == null)
				{
					System.Threading.Interlocked.CompareExchange(ref mysLockObject, new object(), null);
				}
				return mysLockObject;
			}
		}
		private string InstallDirectory
		{
			get
			{
				if (mysInstallDirectory == null)
				{
					lock (LockObject)
					{
						if (mysInstallDirectory == null)
						{
							Guid hierGuid = typeof(IVsHierarchy).GUID;
							IntPtr pvHier = IntPtr.Zero;
							IntPtr pvShell = IntPtr.Zero;
							try
							{
								// The service provider we are handed is very limited, but getting
								// the hierarchy is one of the supported services. This gets another
								// service from the hierarchy which is then used to get the install
								// directory for the shell. This is pushing things a bit, but is
								// the only reliable way to get the VS shell install directory. Note
								// that we could also look at our assembly location, but that is more
								// fragile because the success of this tool would then depend of the relative
								// position of the install directory, and whether or not it was placed in the gac.
								myServiceProvider.QueryService(ref hierGuid, ref hierGuid, out pvHier);
								if (pvHier != IntPtr.Zero)
								{
									IVsHierarchy hier = Marshal.GetObjectForIUnknown(pvHier) as IVsHierarchy;
									if (hier != null)
									{
										MsOle.IServiceProvider spFull;
										hier.GetSite(out spFull);
										if (spFull != null)
										{
											Guid shellGuid = typeof(IVsShell).GUID;
											spFull.QueryService(ref shellGuid, ref shellGuid, out pvShell);
											if (pvShell != IntPtr.Zero)
											{
												IVsShell shellPointer = Marshal.GetObjectForIUnknown(pvShell) as IVsShell;
												if (shellPointer != null)
												{
													object installDir;
													shellPointer.GetProperty((int)__VSSPROPID.VSSPROPID_InstallDirectory, out installDir);
													mysInstallDirectory = (string)installDir;
												}
											}
										}
									}
								}
							}
							finally
							{
								if (pvShell != IntPtr.Zero)
								{
									Marshal.Release(pvShell);
								}
								if (pvHier != IntPtr.Zero)
								{
									Marshal.Release(pvHier);
								}
							}
						}
					}
				}
				return mysInstallDirectory;
			}
		}
		/// <summary>
		/// Generate a dmd file for the current ims file contents. Note that this
		/// takes a contents instead of a file name so that we don't have to save
		/// the file to regenerate the .dmd.
		/// </summary>
		/// <param name="imsContents">Contents of an ims file</param>
		/// <returns>Contents of the corresponding dmd file</returns>
		private string GenerateDmd(string imsContents)
		{
			FileInfo imsFileInfo = null;
			FileInfo dmdFileInfo = null;
			string retVal = "";
			try
			{
				// Generate temp files and names
				imsFileInfo = new FileInfo(Path.GetTempFileName());
				dmdFileInfo = new FileInfo(Path.GetTempFileName());
				// Set the extensions to something the ImsToDmd tool will like.
				// Changing the extension is a clear hack, but the chances of the other
				// files existing are small. GetTempFileName should
				// have overloads to take a prefix/extension, but it doesn't.
				imsFileInfo.MoveTo(imsFileInfo.FullName + ".ims");
				dmdFileInfo.MoveTo(dmdFileInfo.FullName + ModelFileExtension);

				// Move the ims data into the temporary .ims file
				FileStream fileStream = imsFileInfo.OpenWrite();
				using (StreamWriter writer = new StreamWriter(fileStream))
				{
					writer.Write(imsContents);
				}
				fileStream.Close();

				// Shell the process
				Process proc = new Process();
				ProcessStartInfo startInfo = proc.StartInfo;
				startInfo.FileName = InstallDirectory + @"\IMSTODMD.exe";
				startInfo.Arguments = string.Format(@"""{0}"" ""{1}""", imsFileInfo.FullName, dmdFileInfo.FullName);
				startInfo.WindowStyle = ProcessWindowStyle.Hidden;
				proc.Start();
				proc.WaitForExit();

				// Retrieve the dmd data and return it
				StreamReader reader = dmdFileInfo.OpenText();
				retVal = reader.ReadToEnd();
				reader.Close();
			}
			finally
			{
				if (imsFileInfo != null)
				{
					imsFileInfo.Delete();
				}
				if (imsFileInfo != null)
				{
					dmdFileInfo.Delete();
				}
			}
			return retVal;
		}
		#endregion // CustomTool Specific
	}
}
