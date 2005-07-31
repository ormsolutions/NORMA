#region Using directives

using System;
using System.Text;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager;
using Microsoft.VisualStudio.TextManager.Interop;
using Neumont.Tools.ORM.Shell;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using IOleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

#endregion

namespace Neumont.Tools.ORM.FactEditor
{
	/// <summary>
	/// The host of the text editor
	/// </summary>
	public class FactCodeWindowManager : IVsCodeWindowManager
	{
		private IVsCodeWindow myCodeWindow;
		private ORMDesignerPackage myPackage;
		private LinkedList<FactTextViewFilter> myListViews;

		/// <summary>
		/// Create a window manager
		/// </summary>
		/// <param name="package">That package on which to create text views</param>
		/// <param name="codeWindow">The source window for the facts being entered</param>
		public FactCodeWindowManager(ORMDesignerPackage package, IVsCodeWindow codeWindow)
		{
			myPackage = package;
			myCodeWindow = codeWindow;
			myListViews = new LinkedList<FactTextViewFilter>();
		}

		#region IVsCodeWindowManager Members

		int IVsCodeWindowManager.AddAdornments()
		{
			return AddAdornments();
		}
		/// <summary>
		/// Implements IVsCodeWindowManager.AddAdornments
		/// </summary>
		/// <returns></returns>
		protected int AddAdornments()
		{
			// call on new view ourselves for the current view
			IVsTextView tv;
			myCodeWindow.GetPrimaryView(out tv);
			(this as IVsCodeWindowManager).OnNewView(tv);

			// retrieve the IServiceProvider from the code window
			IOleServiceProvider sp = (IOleServiceProvider)myCodeWindow;
			Guid tempGuid = typeof(IVsWindowFrame).GUID;
			IntPtr pFrame = IntPtr.Zero;
			int hr = sp.QueryService(ref tempGuid, ref tempGuid, out pFrame);
			if (hr == VSConstants.S_OK)
			{
				try
				{
					IVsWindowFrame frame = (IVsWindowFrame)Marshal.GetObjectForIUnknown(pFrame);
					tempGuid = FactGuidList.CmdUIGuidTextEditor;
					hr = frame.SetGuidProperty((int)__VSFPROPID.VSFPROPID_InheritKeyBindings, ref tempGuid);
				}
				finally
				{
					if (pFrame != IntPtr.Zero)
					{
						Marshal.Release(pFrame);
					}
				}
			}
			return hr;
		}

		int IVsCodeWindowManager.OnNewView(IVsTextView pView)
		{
			return OnNewView(pView);
		}
		/// <summary>
		/// Implements IVsCodeWindowManager.OnNewView
		/// </summary>
		/// <param name="view"></param>
		/// <returns></returns>
		protected int OnNewView(IVsTextView view)
		{
			FactTextViewFilter textViewFilter;
			textViewFilter = new FactTextViewFilter(myPackage, view);
			textViewFilter.Init();
			myListViews.AddFirst(textViewFilter);
			return VSConstants.S_OK;
		}

		int IVsCodeWindowManager.RemoveAdornments()
		{
			return RemoveAdornments();
		}
		/// <summary>
		/// Implements IVsCodeWindowManager.RemoveAdornments
		/// </summary>
		/// <returns></returns>
		protected int RemoveAdornments()
		{
			// take our view out of the command target chain
			foreach (FactTextViewFilter ftvf in myListViews)
			{
				ftvf.TextView.RemoveCommandFilter(ftvf);
			}
			return VSConstants.S_OK;
		}

		#endregion
	}
}
