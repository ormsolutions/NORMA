using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Xml;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.Win32;    
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Modeling;
using EnvDTE;
using EnvDTE80;

using ORMSolutions.ORMArchitect.Core.Shell;
using ORMSolutions.ORMArchitect.Framework;


namespace unibz.ORMInferenceEngine
{

    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    ///
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane, 
    /// usually implemented by the package implementer.
    ///
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its 
    /// implementation of the IVsWindowPane interface.
    /// </summary>
    [Guid("8480864C-71C6-47C4-83F1-D95DD520172F")]
    public class ORMInferenceEngineToolWindow : ORMToolWindow
    {
        // This is the user control hosted by the tool window; it is exposed to the base class 
        // using the Window property. Note that, even if this class implements IDispose, we are
        // not calling Dispose on this object. This is because ToolWindowPane calls Dispose on 
        // the object returned by the Window property.
        private ORMInferenceTreeWindow inferenceWindow;

        public override string WindowTitle
        {
            get { return Resources.ToolWindowTitle; }
        }

        protected override int BitmapResource
        {
            get { return 301; }
        }

        protected override int BitmapIndex
        {
            get { return 1; }
        }

        protected override void ManageEventHandlers(Store store, ModelingEventManager eventManager, EventHandlerAction action)
        {
            return;
        }

        protected override void Initialize()
        {
            base.Initialize();
            IVsToolWindowToolbarHost host = ToolBarHost;
            if (host != null)
            {
                Guid commandGuid = GuidList.guidORMInferenceEnginePackageCmdSet;
                host.AddToolbar(VSTWT_LOCATION.VSTWT_TOP, ref commandGuid, PkgCmdIDList.ORMInferenceToolbarID);
            }
        }

        protected override bool HasToolBar
        {
            get
            {
                return true;
            }
        }
        public ORMInferenceEngineToolWindow(IServiceProvider serviceProvider) :
            base(serviceProvider)
        {
            
            // Create the handlers for the toolbar commands.
			OleMenuCommandService mcs = GetService(typeof(IMenuCommandService))
                as OleMenuCommandService;
            if (null != mcs)
            {
				CommandID toolbarbtnCmdID = new CommandID(
                    GuidList.guidORMInferenceEnginePackageCmdSet,
                    (int)PkgCmdIDList.cmdidORMInferenceEngineStart);
				MenuCommand menuItem = new MenuCommand(new EventHandler(
                    RunInferenceEngineButtonHandler), toolbarbtnCmdID);
                mcs.AddCommand(menuItem);
            }

            inferenceWindow = new ORMInferenceTreeWindow();
        }

        /// <summary>
        /// This property returns the handle to the user control that should
        /// be hosted in the Tool Window.
        /// </summary>
        override public IWin32Window Window
        {
            get
            {
                return (IWin32Window)inferenceWindow;
            }
        }
        public int RunInferenceEngine()
        {
            MessageBox.Show("InferenceEngine");
  
            return 0;
        }

        private void RunInferenceEngineButtonHandler(object sender, EventArgs arguments)
        {
            RunInferenceEngine();
        }
    }
}
