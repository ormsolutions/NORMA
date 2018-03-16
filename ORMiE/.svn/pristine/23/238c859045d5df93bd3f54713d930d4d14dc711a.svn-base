// VsPkg.cs : Implementation of ORMInferenceEngine
//

using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using Microsoft.Win32;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using EnvDTE;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling.Shell;

using ORMSolutions.ORMArchitect.Core.Shell;
using ORMSolutions.ORMArchitect.Framework;

namespace unibz.ORMInferenceEngine
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// 
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the registration utility (regpkg.exe) that this class needs
    // to be registered as package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
#if !VISUALSTUDIO_10_0
	// This gives build warnings in VS2010, but there does not appear to be an alternative. The
	// settings for this attribute are maintained by hand in the pkgdef file.
	// This attribute is used to register the informations needed to show the this package
	// in the Help/About dialog of Visual Studio.
	[InstalledProductRegistration(false, "#110", "#112", "1.0", IconResourceID = 400)]
#endif
	// In order be loaded inside Visual Studio in a machine that has not the VS SDK installed, 
	// package needs to have a valid load key (it can be requested at 
	// http://msdn.microsoft.com/vstudio/extend/). This attributes tells the shell that this 
	// package has a load key embedded in its resources.
	[ProvideLoadKey("Standard", "1.0", "ORM2 Inference Engine", "Free University of Bozen-Bolzano", 1)]
    // This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideMenuResource(1000, 1)]
    // This attribute registers a tool window exposed by this package.
    [ProvideToolWindow(typeof(ORMInferenceEngineToolWindow), Orientation = ToolWindowOrientation.Right, Transient=true, Style = VsDockStyle.Tabbed, Window = ToolWindowGuids.SolutionExplorer)]
    [Guid(GuidList.guidORMInferenceEnginePkgString)]
    public sealed class ORMInferenceEnginePackage : ModelingPackage
    {
        /// <summary>
        /// Default constructor of the package.
        /// Inside this method you can place any initialization code that does not require 
        /// any Visual Studio service because at this point the package object is created but 
        /// not sited yet inside Visual Studio environment. The place to do all the other 
        /// initialization is the Initialize method.
        /// </summary>
        public ORMInferenceEnginePackage()
        {
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this.ToString()));
        }

        /// <summary>
        /// This function is called when the user clicks the menu item that shows the 
        /// tool window. See the Initialize method to see how the menu item is associated to 
        /// this function using the OleMenuCommandService service and the MenuCommand class.
        /// </summary>
        private void ShowToolWindow(object sender, EventArgs e)
        {
            // Get the instance number 0 of this tool window. This window is single instance so this instance
            // is actually the only one.
            // The last flag is set to true so that if the tool window does not exists it will be created.
            ToolWindow window = this.GetToolWindow(typeof(ORMInferenceEngineToolWindow), true);
            if ((null == window) || (null == window.Frame))
            {
                throw new NotSupportedException(Resources.CanNotCreateWindow);
            }
            IVsWindowFrame windowFrame = (IVsWindowFrame)window.Frame;
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());
        }

        private void CanShowToolWindow(object sender, EventArgs e)
        {
            MenuCommand command = sender as MenuCommand;
            command.Enabled = true;
            command.Visible = true;
        }

        /////////////////////////////////////////////////////////////////////////////
        // Overriden Package Implementation
        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initilaization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            if (!SetupMode)
            {
                this.AddToolWindow(typeof(ORMInferenceEngineToolWindow));

                // Add our command handlers for menu (commands must exist in the .vsct file)
                OleMenuCommandService mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
                if (null != mcs)
                {

                    // Create the command for the menu item.
                    CommandID menuCommandID = new CommandID(GuidList.guidORMInferenceEnginePackageCmdSet, (int)PkgCmdIDList.cmdidORMInferenceEngineStart);
                    MenuCommand menuItem = new MenuCommand(MenuItemCallback, menuCommandID);
                    mcs.AddCommand(menuItem);
                    // Create the command for the tool window
                    CommandID toolwndCommandID = new CommandID(GuidList.guidORMInferenceEnginePackageCmdSet, (int)PkgCmdIDList.cmdidORMInferenceExplorer);
                    //MenuCommand menuToolWin = new MenuCommand(ShowToolWindow, toolwndCommandID);
                    //mcs.AddCommand(menuToolWin);
                    MenuCommand dynamicMenuToolWin = new DynamicStatusMenuCommand(CanShowToolWindow, ShowToolWindow, toolwndCommandID);
                    mcs.AddCommand(dynamicMenuToolWin);
                }
            }
        }
        #endregion

        /// <summary>
        /// This function is the callback used to execute a command when the a menu item is clicked.
        /// See the Initialize method to see how the menu item is associated to this function using
        /// the OleMenuCommandService service and the MenuCommand class.
        /// </summary>
        private void MenuItemCallback(object sender, EventArgs e)
        {
            // Show a Message Box to prove we were here
            IVsUIShell uiShell = (IVsUIShell)GetService(typeof(SVsUIShell));
            Guid clsid = Guid.Empty;
            ORMInferenceEngineToolWindow window = (ORMInferenceEngineToolWindow)this.GetToolWindow(typeof(ORMInferenceEngineToolWindow), true);
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(
                    window.RunInferenceEngine()
                );
        }

    }
}