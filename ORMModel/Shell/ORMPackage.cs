using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Resources;
using System.Runtime.InteropServices;
using VsShell = Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.EnterpriseTools.Shell;
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
	public sealed class ORMDesignerPackage : ModelingPackage
	{
		#region Member variables
		/// <summary>
		/// The commands supported by this package
		/// </summary>
		private object myCommandSet;
		private static ORMDesignerPackage mySingleton;
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
			if (!SetupMode)
			{
				// setup commands
				myCommandSet = ORMDesignerDocView.CreateCommandSet(this);

				// Create tool window
				AddToolWindow(new ORMBrowserToolWindow(this));
				AddToolWindow(new ORMReadingEditorToolWindow(this));
			}

		}
		/// <summary>
		/// This is called by the package base class when our package gets unloaded.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
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
		#endregion
	}
}
