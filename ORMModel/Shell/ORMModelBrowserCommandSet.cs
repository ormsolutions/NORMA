using Microsoft.VisualStudio.EnterpriseTools.Shell;
using Microsoft.VisualStudio.Modeling;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.ObjectModel.Editors;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Shell;

namespace Neumont.Tools.ORM.Shell
{
	public partial class ORMBrowserToolWindow : ModelExplorerToolWindow
	{
		private class ORMModelBrowserCommandSet : MarshalByRefObject, IDisposable
		{
			private IMenuCommandService myMenuService;
			private IMonitorSelectionService myMonitorSelection;
			private IServiceProvider myServiceProvider;
			

			private MenuCommand[] myCommands;

			public ORMModelBrowserCommandSet(IServiceProvider provider, IMenuCommandService menuService)
			{
				myServiceProvider = provider;
				myMenuService = menuService;
#region command array
				myCommands = new MenuCommand[]{
					new DynamicStatusMenuCommand(
					new EventHandler(OnStatusDelete),
					new EventHandler(OnMenuDelete),
					StandardCommands.Delete)};
#endregion //command array
				AddCommands(myCommands);
			}
			protected virtual void AddCommands(MenuCommand[] commands)
			{
				IMenuCommandService menuService = MenuService; //force creation of myMenuService
				if (menuService != null)
				{
					int count = commands.Length;
					for (int i = 0; i < count; ++i)
					{
						menuService.AddCommand(commands[i]);
					}
				}
			}
			protected virtual void RemoveCommands(MenuCommand[] commands)
			{
				IMenuCommandService menuService = myMenuService;
				if (menuService != null)
				{
					int count = commands.Length;
					for (int i = 0; i < count; ++i)
					{
						menuService.RemoveCommand(commands[i]);
					}
				}
			}
			protected IMenuCommandService MenuService
			{
				get 
				{
					Debug.Assert(myMenuService != null); // Should be passed into the constructor
					return myMenuService;
				}
			}
			protected ORMBrowserToolWindow CurrentToolWindow
			{
				get 
				{
					return MonitorSelection.CurrentWindow as ORMBrowserToolWindow;
				}
			}
			/// <summary>
			/// Load the monitor selection service
			/// </summary>
			private IMonitorSelectionService MonitorSelection
			{
				get
				{
					IMonitorSelectionService monitorSelect = myMonitorSelection;
					if (monitorSelect == null)
					{
						myMonitorSelection = monitorSelect = (IMonitorSelectionService)myServiceProvider.GetService(typeof(IMonitorSelectionService));
					}
					return monitorSelect;
				}
			}

			#region IDisposable Members

			public void Dispose()
			{
				if (myCommands != null)
				{
					RemoveCommands(myCommands);
				}
				myMenuService = null;
				myMonitorSelection = null;
				myServiceProvider = null;
				myCommands = null;
			}

			public void OnStatusDelete(Object sender, EventArgs e)
			{
				IMonitorSelectionService service = MonitorSelection;
				ORMBrowserToolWindow.OnStatusCommand(sender, ORMDesignerCommands.Delete, service.CurrentWindow as ORMBrowserToolWindow); 
			}
			public void OnMenuDelete(Object sender, EventArgs e)
			{
				ORMBrowserToolWindow currentWindow = CurrentToolWindow;
				if (currentWindow != null)
				{
					currentWindow.OnMenuDelete((sender as OleMenuCommand).Text);
				}
			}
			#endregion
		}
	}
}
