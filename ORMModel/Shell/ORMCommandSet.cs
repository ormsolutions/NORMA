using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Diagnostics;
using Microsoft.VisualStudio.EnterpriseTools.Shell;
using Microsoft.VisualStudio.EnterpriseTools.Validation.UI;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace Northface.Tools.ORM.Shell
{	
	public partial class ORMDesignerDocView
	{
		/// <summary>
		/// Create a command set for this type of view. Should be called
		/// once when the package loads
		/// </summary>
		/// <param name="serviceProvider">IServiceProvider</param>
		/// <returns></returns>
		public static object CreateCommandSet(IServiceProvider serviceProvider)
		{
			return new ORMDesignerCommandSet(serviceProvider);
		}
		/// <summary>
		/// Command objects for the ORMDesignerDocView
		/// </summary>
		[CLSCompliant(false)]
		protected class ORMDesignerCommandSet : MarshalByRefObject, IDisposable
		{
			private IMenuCommandService myMenuService;
			private IMonitorSelectionService myMonitorSelection;
			private IServiceProvider myServiceProvider;
			private ModelElementLocator myElementLocator;

			/// <summary>
			/// Commands
			/// </summary>
			private MenuCommand[] myCommands;

			/// <summary>
			/// 
			/// </summary>
			/// <param name="serviceProvider"></param>
			public ORMDesignerCommandSet(IServiceProvider serviceProvider)
			{
				myServiceProvider = serviceProvider;
				// add view ORM Model Explorer commands in the top-level menu.
				// These do not need a status handler (always enabled when the application designer is 
				// active), so we don't need an EFTMenuCommand
				MenuCommand menuCommand = new MenuCommand(new EventHandler(OnMenuViewORMModelExplorer), ORMDesignerCommandIds.ViewModelExplorer);
				menuCommand.Supported = false;
				MenuService.AddCommand(menuCommand);

				#region Array of menu commands
				myCommands = new MenuCommand[]
			{
				// Commands
				new DynamicStatusMenuCommand(
				new EventHandler(OnStatusDelete),
				new EventHandler(OnMenuDelete),
				StandardCommands.Delete)
			};
				#endregion
				AddCommands(myCommands);


			}

			/// <summary>
			/// Called to add a set of commands. This should be called
			/// by Initialize.
			/// </summary>
			/// <param name="commands">Commands to add</param>
			protected virtual void AddCommands(MenuCommand[] commands)
			{
				IMenuCommandService menuService = MenuService; // Use the accessor to force creation
				if (menuService != null)
				{
					int count = commands.Length;
					for (int i = 0; i < count; ++i)
					{
						menuService.AddCommand(commands[i]);
					}
				}
			}
			/// <summary>
			/// Called to remove a set of commands. This should be called
			/// by Dispose.
			/// </summary>
			/// <param name="commands">Commands to add</param>
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
			/// <summary>
			/// 
			/// </summary>
			public virtual void Dispose()
			{
				if (myCommands != null)
				{
					RemoveCommands(myCommands);
				}
				myMenuService = null;
				myMonitorSelection = null;
				myServiceProvider = null;
				myCommands = null;
				myElementLocator = null;
			}

			/// <summary>
			/// Show the ORM Model Explorer
			/// </summary>
			/// <param name="sender"></param>
			/// <param name="e"></param>
			protected void OnMenuViewORMModelExplorer(object sender, EventArgs e)
			{
				BrowserWindow.Show();
			}

			/// <summary>
			/// Status callback
			/// </summary>
			/// <param name="sender">Sender</param>
			/// <param name="e">Event args</param>
			private void OnStatusDelete(object sender, EventArgs e)
			{
				ORMDesignerDocView.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.Delete);
			}

			/// <summary>
			/// Menu handler
			/// </summary>
			/// <param name="sender">Sender</param>
			/// <param name="e">Event args</param>
			private void OnMenuDelete(object sender, EventArgs e)
			{
				ORMDesignerDocView docView = CurrentORMView;
				if (docView != null)
				{
					// call delete on the doc view
					docView.OnMenuDelete();
				}
			}

			/// <summary>
			/// 
			/// </summary>
			protected IMenuCommandService MenuService
			{
				get
				{
					IMenuCommandService menuService = myMenuService;
					if (menuService == null)
					{
						try
						{
							myMenuService = menuService = (IMenuCommandService)myServiceProvider.GetService(typeof(IMenuCommandService));
						}
						catch (InvalidCastException)
						{
							Debug.Assert(false, "CommandSet relies on the menu command service, which is unavailable.");
							throw;
						}
					}

					return menuService;
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

			/// <summary>
			/// An element locator. Used to navigate to items.
			/// </summary>
			/// <value></value>
			public ModelElementLocator ElementLocator
			{
				get
				{
					ModelElementLocator locator = myElementLocator;
					if (locator == null)
					{
						myElementLocator = locator = new ModelElementLocator((ModelingPackage)myServiceProvider);
					}
					return locator;
				}
			}
			/// <summary>
			/// Currently focused document
			/// </summary>
			protected ORMDesignerDocData CurrentData
			{
				get
				{
					return MonitorSelection.CurrentDocument as ORMDesignerDocData;
				}
			}

			/// <summary>
			/// Currently focused ORM document view
			/// </summary>
			protected ORMDesignerDocView CurrentORMView
			{
				get
				{
					return MonitorSelection.CurrentDocumentView as ORMDesignerDocView;
				}
			}

			/// <summary>
			/// Browser tool window.
			/// </summary>
			protected ORMBrowserToolWindow BrowserWindow
			{
				get
				{
					ORMDesignerPackage package = myServiceProvider.GetService(typeof(Package)) as ORMDesignerPackage;

					if (package != null)
					{
						return (ORMBrowserToolWindow)package.GetToolWindow(typeof(ORMBrowserToolWindow));
					}

					return null;
				}
			}
		}
		/// <summary>
		/// CommandIDs for the Application Designer package.
		/// </summary>
		public class ORMDesignerCommandIds
		{
			/// <summary>
			/// 
			/// </summary>
			public static readonly Guid guidORMDesignerCommandSet = new Guid("7C51C000-1EAD-4b39-89B5-42BC9F49EA24");    // keep in sync with SatDll\PkgCmd.ctc

			#region CommandID objects
			/// <summary>
			/// The ORM Model Explorer item on the view menu
			/// </summary>
			public static readonly CommandID ViewModelExplorer = new CommandID(guidORMDesignerCommandSet, cmdIdViewModelExplorer);

			/// <summary>
			/// The context menu for the diagram
			/// </summary>
			public static readonly CommandID ViewContextMenu = new CommandID(guidORMDesignerCommandSet, menuIdContextMenu);

			#endregion

			#region cmdIds
			// IMPORTANT: keep these constants in sync with SatDll\PkgCmdID.h

			/// <summary>
			/// The ORM Model Explorer item on the view menu
			/// </summary>
			public const int cmdIdViewModelExplorer = 0x2900;

			/// <summary>
			/// The context menu for the diagram
			/// </summary>
			public const int menuIdContextMenu = 0x0100;

			#endregion

		}
	}
}