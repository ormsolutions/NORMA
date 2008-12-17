#region Common Public License Copyright Notice
/**************************************************************************\
* Neumont Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
* Copyright © Matthew Curland. All rights reserved.                        *
*                                                                          *
* The use and distribution terms for this software are covered by the      *
* Common Public License 1.0 (http://opensource.org/licenses/cpl) which     *
* can be found in the file CPL.txt at the root of this distribution.       *
* By using this software in any fashion, you are agreeing to be bound by   *
* the terms of this license.                                               *
*                                                                          *
* You must not remove this notice, or any other, from this software.       *
\**************************************************************************/
#endregion

using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Shell;
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
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio;
using Neumont.Tools.ORM.ShapeModel;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace Neumont.Tools.ORM.Shell
{
	public partial class ORMModelBrowserToolWindow
	{
		private sealed class ORMModelBrowserCommandSet : MarshalByRefObject, IDisposable
		{
			private IMenuCommandService myMenuService;
			private IMonitorSelectionService myMonitorSelection;
			private IServiceProvider myServiceProvider;
			private MenuCommand[] myCommands;
			private static CommandID EditLabelCommandID = new CommandID(VSConstants.GUID_VSStandardCommandSet97, (int)VSConstants.VSStd97CmdID.EditLabel);

			public ORMModelBrowserCommandSet(IServiceProvider provider, IMenuCommandService menuService)
			{
				myServiceProvider = provider;
				myMenuService = menuService;
				#region command array
				myCommands = new MenuCommand[]{
					new DynamicStatusMenuCommand(
					new EventHandler(OnStatusDelete),
					new EventHandler(OnMenuDelete),
					StandardCommands.Delete)
					,new DynamicStatusMenuCommand(
					new EventHandler(OnStatusEditLabel),
					new EventHandler(OnMenuEditLabel),
					EditLabelCommandID)
					,new DynamicDiagramCommand(
					new EventHandler(OnStatusDiagramList),
					new EventHandler(OnMenuDiagramList),
					ORMDesignerDocView.ORMDesignerCommandIds.DiagramList)
					,new DynamicDiagramCommand(
					new EventHandler(OnStatusDiagramList),
					new EventHandler(OnMenuDiagramSpyDiagramList),
					ORMDesignerDocView.ORMDesignerCommandIds.DiagramSpyDiagramList)
					,new DynamicStatusMenuCommand(
					new EventHandler(OnStatusSelectShapeInDocumentWindow),
					new EventHandler(OnMenuSelectShapeInDocumentWindow),
					ORMDesignerDocView.ORMDesignerCommandIds.SelectInDocumentWindow)
					,new DynamicStatusMenuCommand(
					new EventHandler(OnStatusSelectShapeInDiagramSpy),
					new EventHandler(OnMenuSelectShapeInDiagramSpy),
					ORMDesignerDocView.ORMDesignerCommandIds.SelectInDiagramSpy)
				};
				#endregion //command array
				AddCommands(myCommands);
			}
			private void AddCommands(MenuCommand[] commands)
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
			private void RemoveCommands(MenuCommand[] commands)
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
			private IMenuCommandService MenuService
			{
				get 
				{
					Debug.Assert(myMenuService != null); // Should be passed into the constructor
					return myMenuService;
				}
			}
			private ORMModelBrowserToolWindow CurrentToolWindow
			{
				get 
				{
					return MonitorSelection.CurrentWindow as ORMModelBrowserToolWindow;
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
			#endregion // IDisposable Members
			#region Command Handlers

			public void OnStatusDelete(Object sender, EventArgs e)
			{
				ORMModelBrowserToolWindow.OnStatusCommand(sender, ORMDesignerCommands.Delete | ORMDesignerCommands.DeleteAny, CurrentToolWindow); 
			}
			public void OnMenuDelete(Object sender, EventArgs e)
			{
				ORMModelBrowserToolWindow currentWindow = CurrentToolWindow;
				if (currentWindow != null)
				{
					currentWindow.OnMenuDelete((sender as OleMenuCommand).Text);
				}
			}
			public void OnStatusEditLabel(Object sender, EventArgs e)
			{
				MenuCommand command = sender as MenuCommand;
				if (command != null)
				{
					// Support this command regardless of whether or not it is supported by the current
					// element or the current state of the inline editor. If we do not do this, then an F2
					// keypress with an editor already open will report the command as disabled and we would
					// need to use IVsUIShell.UpdateCommandUI whenever an editor closed to reenable the command.
					command.Visible = true;
					command.Enabled = true;
				}
			}
			public void OnMenuEditLabel(Object sender, EventArgs e)
			{
				ORMModelBrowserToolWindow currentWindow = CurrentToolWindow;
				if (currentWindow != null)
				{
					currentWindow.OnMenuEditLabel();
				}
			}
			private sealed class DynamicDiagramCommand : DynamicStatusMenuCommand
			{
				public DynamicDiagramCommand(EventHandler statusHandler, EventHandler invokeHandler, CommandID id)
					: base(statusHandler, invokeHandler, id)
				{
					//Declare class variable with object containing diagram list
				}
				public sealed override bool DynamicItemMatch(int cmdId)
				{
					int baseCmdId = CommandID.ID;
					int testId = cmdId - baseCmdId;


					if (testId >= 0 && testId < ORMDesignerDocView.ORMDesignerCommandIds.DiagramListLength)
					{
						MatchedCommandId = testId;
						return true;
					}
					return false;
				}
			}
			/// <summary>
			/// Status callback
			/// </summary>
			public void OnStatusDiagramList(object sender, EventArgs e)
			{
				ORMModelBrowserToolWindow.OnStatusCommand(sender, ORMDesignerCommands.DiagramList, CurrentToolWindow);
				((OleMenuCommand)sender).MatchedCommandId = 0;
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			public void OnMenuDiagramList(object sender, EventArgs e)
			{
				ORMModelBrowserToolWindow currentWindow = CurrentToolWindow;
				if (currentWindow != null)
				{
					currentWindow.OnMenuDiagramList(((OleMenuCommand)sender).MatchedCommandId, NavigateToWindow.Document);
				}
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			public void OnMenuDiagramSpyDiagramList(object sender, EventArgs e)
			{
				ORMModelBrowserToolWindow currentWindow = CurrentToolWindow;
				if (currentWindow != null)
				{
					currentWindow.OnMenuDiagramList(((OleMenuCommand)sender).MatchedCommandId, NavigateToWindow.DiagramSpy);
				}
			}
			/// <summary>
			/// Status callback
			/// </summary>
			public void OnStatusSelectShapeInDocumentWindow(object sender, EventArgs e)
			{
				ORMModelBrowserToolWindow.OnStatusCommand(sender, ORMDesignerCommands.SelectInDocumentWindow, CurrentToolWindow);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			public void OnMenuSelectShapeInDocumentWindow(object sender, EventArgs e)
			{
				ORMModelBrowserToolWindow currentWindow = CurrentToolWindow;
				if (currentWindow != null)
				{
					currentWindow.OnMenuSelectShape(NavigateToWindow.Document);
				}
			}
			/// <summary>
			/// Status callback
			/// </summary>
			public void OnStatusSelectShapeInDiagramSpy(object sender, EventArgs e)
			{
				ORMModelBrowserToolWindow.OnStatusCommand(sender, ORMDesignerCommands.SelectInDiagramSpy, CurrentToolWindow);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			public void OnMenuSelectShapeInDiagramSpy(object sender, EventArgs e)
			{
				ORMModelBrowserToolWindow currentWindow = CurrentToolWindow;
				if (currentWindow != null)
				{
					currentWindow.OnMenuSelectShape(NavigateToWindow.DiagramSpy);
				}
			}
			#endregion // Command Handlers
		}
	}
}
