using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TextManager.Interop;
using System.Reflection;
using Northface.Tools.ORM.Shell;

using IOleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

namespace Northface.Tools.ORM.FactEditor
{
	/// <summary>
	/// The factory which creates instances of the ORM fact editor. A factory is created
	/// when the ORM package initializes
	/// </summary>
	[Guid("044DD982-9DD6-42AC-8C70-BA6A4565E0AE")]
	[CLSCompliant(false)]
	public class FactEditorFactory : IVsEditorFactory
	{
		private ORMDesignerPackage myPackage;
		private IOleServiceProvider vsServiceProvider;
		private System.IServiceProvider vsServiceProviderManaged;

		/// <summary>
		/// Create an instance of the package
		/// </summary>
		/// <param name="package">An ORMDesignerPackage which has a managed IServerProvider</param>
		public FactEditorFactory(ORMDesignerPackage package)
		{
			vsServiceProviderManaged = package;
			Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering {0} constructor", this.ToString()));
			myPackage = package;
		}

		#region IVsEditorFactory Members

		int IVsEditorFactory.Close()
		{
			return Close();
		}
		/// <summary>
		/// Implements IVsEditorFactory.Close
		/// </summary>
		/// <returns></returns>
		protected int Close()
		{
			return NativeMethods.S_OK;
		}

		int IVsEditorFactory.CreateEditorInstance(
						uint grfCreateDoc,
						string pszMkDocument,
						string pszPhysicalView,
						IVsHierarchy pvHier,
						uint itemid,
						System.IntPtr punkDocDataExisting,
						out System.IntPtr ppunkDocView,
						out System.IntPtr ppunkDocData,
						out string pbstrEditorCaption,
						out Guid pguidCmdUI,
						out int pgrfCDW)
		{
			return CreateEditorInstance(grfCreateDoc, pszMkDocument, pszPhysicalView, pvHier, itemid, punkDocDataExisting, out ppunkDocView, out ppunkDocData, out pbstrEditorCaption, out pguidCmdUI, out pgrfCDW);
		}
		/// <summary>
		/// Implements IVsEditorFactory.CreateEditorInstance
		/// </summary>
		/// <param name="grfCreateDoc"></param>
		/// <param name="pszMkDocument"></param>
		/// <param name="pszPhysicalView"></param>
		/// <param name="pvHier"></param>
		/// <param name="itemid"></param>
		/// <param name="punkDocDataExisting"></param>
		/// <param name="ppunkDocView"></param>
		/// <param name="ppunkDocData"></param>
		/// <param name="pbstrEditorCaption"></param>
		/// <param name="pguidCmdUI"></param>
		/// <param name="pgrfCDW"></param>
		/// <returns></returns>
		protected int CreateEditorInstance(
						uint grfCreateDoc,
						string pszMkDocument,
						string pszPhysicalView,
						IVsHierarchy pvHier,
						uint itemid,
						System.IntPtr punkDocDataExisting,
						out System.IntPtr ppunkDocView,
						out System.IntPtr ppunkDocData,
						out string pbstrEditorCaption,
						out Guid pguidCmdUI,
						out int pgrfCDW)
		{
			Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering {0} CreateEditorInstace()", this.ToString()));

			// Initialize to null
			ppunkDocView = new System.IntPtr();
			ppunkDocData = new System.IntPtr();
			pguidCmdUI = new Guid();
			pgrfCDW = 0;
			pbstrEditorCaption = null;

			// Validate inputs
			if ((grfCreateDoc & (NativeMethods.CEF_OPENFILE | NativeMethods.CEF_SILENT)) == 0)
			{
				throw new ArgumentException("Only Open or Silent is valid");
			}
			if (punkDocDataExisting != IntPtr.Zero)
			{
				return NativeMethods.VS_E_INCOMPATIBLEDOCDATA;
			}

			// Create a text buffer if one is not passed to us
			//
			// If you look in textmgr.idl you will see that the
			// object described by 'coclass VsTextBuffer' has two interfaces.
			//
			// By looking at IVsTextLines you see it inherits from IVsTextBuffer
			// so by getting a pointer to IVsTextLines you can call all the methods
			// described by both interfaces.
			//
			ILocalRegistry3 locReg = (ILocalRegistry3)vsServiceProviderManaged.GetService(typeof(ILocalRegistry));
			IntPtr pBuf = IntPtr.Zero;
			Guid iid = typeof(IVsTextLines).GUID;
			NativeMethods.ThrowOnFailure(locReg.CreateInstance(
				typeof(VsTextBufferClass).GUID,
				null,
				ref iid,
				(uint)CLSCTX.CLSCTX_INPROC_SERVER,
				out pBuf));

			IVsTextLines lines = null;
			IObjectWithSite objectWithSite = null;
			try
			{
				// Get an object to tie to the IDE
				lines = (IVsTextLines)Marshal.GetObjectForIUnknown(pBuf);
				objectWithSite = lines as IObjectWithSite;
				objectWithSite.SetSite(vsServiceProvider);
			}
			finally
			{
				if (pBuf != IntPtr.Zero)
				{
					Marshal.Release(pBuf);
				}
			}

			// Create a std code view (text)
			IntPtr srpCodeWin = IntPtr.Zero;
			iid = typeof(IVsCodeWindow).GUID;

			// create code view (does CoCreateInstance if not in shell's registry)
			NativeMethods.ThrowOnFailure(locReg.CreateInstance(
				typeof(VsCodeWindowClass).GUID,
				null,
				ref iid,
				(uint)CLSCTX.CLSCTX_INPROC_SERVER,
				out srpCodeWin));

			IVsCodeWindow codeWindow = null;
			try
			{
				// Get an object to tie to the IDE
				codeWindow = (IVsCodeWindow)Marshal.GetObjectForIUnknown(srpCodeWin);
			}
			finally
			{
				if (srpCodeWin != IntPtr.Zero)
				{
					Marshal.Release(srpCodeWin);
				}
			}

			codeWindow.SetBuffer(lines);

			// assign the doc data and doc view to their respective objects
			ppunkDocData = Marshal.GetIUnknownForObject(lines);
			ppunkDocView = Marshal.GetIUnknownForObject(codeWindow);

			return NativeMethods.S_OK;
		}

		int IVsEditorFactory.MapLogicalView(ref Guid rguidLogicalView, out string pbstrPhysicalView)
		{
			return MapLogicalView(ref rguidLogicalView, out pbstrPhysicalView);
		}
		/// <summary>
		/// Implements IVsEditorFactory.MapLogicalView
		/// </summary>
		/// <param name="rguidLogicalView"></param>
		/// <param name="pbstrPhysicalView"></param>
		/// <returns></returns>
		protected int MapLogicalView(ref Guid rguidLogicalView, out string pbstrPhysicalView)
		{
			// We only support 1 phisical view, so return null
			pbstrPhysicalView = null;
			return NativeMethods.S_OK;
		}

		int IVsEditorFactory.SetSite(Microsoft.VisualStudio.OLE.Interop.IServiceProvider psp)
		{
			return SetSite(psp);
		}
		/// <summary>
		/// Implements IVsEditorFactory.SetSite
		/// </summary>
		/// <param name="psp"></param>
		/// <returns></returns>
		protected int SetSite(Microsoft.VisualStudio.OLE.Interop.IServiceProvider psp)
		{
			vsServiceProvider = psp;
			return NativeMethods.S_OK;
		}

#endregion
	}
} 