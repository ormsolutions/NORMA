using System;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Diagnostics;
using System.Resources;
using System.Reflection;
using System.Runtime.InteropServices;
using VsShell = Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.EnterpriseTools.Shell;
using Northface.Tools.ORM.FactEditor;
namespace Northface.Tools.ORM.Shell
{
	/// <summary>
	/// Entry point for the ORMPackage package. An instance of this class is created by the VS
	/// shell whenever one of our services is required.
	/// </summary>
	[Guid("EFDDC549-1646-4451-8A51-E5A5E94D647C")]
	[CLSCompliant(false)]

	// "ORM Designer" and "General" correspond and must be in sync with
	// the VRG file and are defined also in ORMDesignerUI.rc
	[ProvideOptionPage(typeof(OptionsPage), "ORM Designer", "General", 105, 106, false)]
	[InstalledProductRegistration(UseInterface=true)]
	public sealed class ORMDesignerPackage : ModelingPackage, IVsInstalledProduct
	{
		#region Member variables
		/// <summary>
		/// The commands supported by this package
		/// </summary>
		private object myCommandSet;
		private static ORMDesignerPackage mySingleton;
		private FactEditorFactory factEditorFactory;
#if FACTEDITORPROTOTYPE
		private uint myLanguageInfoEditorCookie = 0;
#endif // FACTEDITORPROTOTYPE
		#endregion
		#region Construction/destruction
		/// <summary>
		/// Class constructor.
		/// </summary>
		public ORMDesignerPackage()
		{
			Debug.Assert(mySingleton == null); // Should only be loaded once per IDE session
			mySingleton = this;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the singleton command set create for this package.
		/// </summary>
		public static object CommandSet
		{
			get
			{
				ORMDesignerPackage package = mySingleton;
				return (package != null) ? package.myCommandSet : null;
			}
		}
		#endregion // Properties
		#region Base overrides
		/// <summary>
		/// This is called by the package base class when our package is loaded. When devenv is run
		/// with the "/setup" command line switch it is not able to do a lot of the normal things,
		/// such as creating output windows and tool windows. Under normal circumstances our package
		/// isn't loaded when run with this switch. However, our package will be loaded when items 
		/// are added to the toolbox, even when run with "/setup". To be safe we'll check for "setup"
		/// and we don't do anything interesting in MgdSetSite if we find it. 
		/// </summary>
		protected override void Initialize()
		{
			base.Initialize();

			// register the class designer editor factory
			RegisterModelingEditorFactory(new ORMDesignerEditorFactory(this));

#if FACTEDITORPROTOTYPE
			factEditorFactory = new FactEditorFactory(this);
			base.RegisterEditorFactory(factEditorFactory);
#endif // FACTEDITORPROTOTYPE

			if (!SetupMode)
			{
#if FACTEDITORPROTOTYPE
				IProfferService proffer = (IProfferService)GetService(typeof(IProfferService));
				Guid iid = typeof(FactLanguageService).GUID;
				proffer.ProfferService(ref iid, new FactLanguageService(this), out myLanguageInfoEditorCookie);
#endif // FACTEDITORPROTOTYPE

				// setup commands
				myCommandSet = ORMDesignerDocView.CreateCommandSet(this);

				// Create tool window
				AddToolWindow(new ORMBrowserToolWindow(this));
				AddToolWindow(new ORMReadingEditorToolWindow(this));
				AddToolWindow(new ORMReferenceModeEditorToolWindow(this));
			}

		}
		/// <summary>
		/// This is called by the package base class when our package gets unloaded.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
#if FACTEDITORPROTOTYPE
				if (myLanguageInfoEditorCookie != 0)
				{
					IProfferService proffer = (IProfferService)GetService(typeof(IProfferService));
					if (proffer != null)
					{
						proffer.RevokeService(myLanguageInfoEditorCookie);
					}
				}
#endif // FACTEDITORPROTOTYPE
				// dispose of any private objects here
			}
			base.Dispose(disposing);
		}
		/// <summary>
		/// Specifies the name of the DTE object used to bootstrap unit testing.  Derived classes
		/// should specify a unique name.
		/// </summary>
		protected override string UnitTestObjectName
		{
			get
			{
				return "ORMDesignerTestDriver";
			}
		}
		/// <summary>
		/// For use by unit tests. Also used by ModelElementLocator.
		/// Private to discourage use outside of unit testing,
		/// may only be accessed through reflection.
		/// </summary>
		private static ORMDesignerPackage Singleton
		{
			get { return mySingleton; }
		}

		#endregion // Base overrides
		#region IVsInstalledProduct Members
		int IVsInstalledProduct.IdBmpSplash(out uint pIdBmp)
		{
			// UNDONE: implement splash screen here
			pIdBmp = 111;
			return NativeMethods.S_OK;
		}

		int IVsInstalledProduct.IdIcoLogoForAboutbox(out uint pIdIco)
		{
			// UNDONE: replace hard-coded ID for AboutBox icon
			pIdIco = 110;
			return NativeMethods.S_OK;
		}

		int IVsInstalledProduct.OfficialName(out string pbstrName)
		{
			pbstrName = ResourceStrings.PackageOfficialName;
			return NativeMethods.S_OK;
		}

		int IVsInstalledProduct.ProductDetails(out string pbstrProductDetails)
		{
			pbstrProductDetails = ResourceStrings.PackageProductDetails;
			return NativeMethods.S_OK;
		}

		int IVsInstalledProduct.ProductID(out string pbstrPID)
		{
			// UNDONE: we need to sync the productID with the assembly
			pbstrPID = "1.0";
			return NativeMethods.S_OK;
		}

#endregion
		#region Tool Window properties
		/// <summary>
		/// Browser tool window.
		/// </summary>
		public static ORMBrowserToolWindow BrowserWindow
		{
			get
			{
				return (ORMBrowserToolWindow)mySingleton.GetToolWindow(typeof(ORMBrowserToolWindow));
			}
		}

		/// <summary>
		/// Reading editor tool window.
		/// </summary>
		public static ORMReadingEditorToolWindow ReadingEditorWindow
		{
			get
			{
				return (ORMReadingEditorToolWindow)mySingleton.GetToolWindow(typeof(ORMReadingEditorToolWindow));
			}
		}
		/// <summary>
		/// The reference mode editor window.
		/// </summary>
		public static ORMReferenceModeEditorToolWindow ReferenceModeEditorWindow
		{
			get
			{
				return (ORMReferenceModeEditorToolWindow)mySingleton.GetToolWindow(typeof(ORMReferenceModeEditorToolWindow));
			}
		}
		#endregion
	}
}
