#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
* Copyright © ORM Solutions, LLC. All rights reserved.                        *
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
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio;
using ORMSolutions.ORMArchitect.Core.ShapeModel;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace ORMSolutions.ORMArchitect.Core.Shell
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
					,new DynamicStatusMenuCommand(
					new EventHandler(OnStatusExclusiveOrDecoupler),
					new EventHandler(OnMenuExclusiveOrDecoupler),
					ORMDesignerDocView.ORMDesignerCommandIds.ExclusiveOrDecoupler)
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
					,new DynamicFreeFormCommand(
					new EventHandler(OnStatusFreeFormCommand),
					new EventHandler(OnMenuFreeFormCommand),
					ORMDesignerDocView.ORMDesignerCommandIds.FreeFormCommandList)
					,new DynamicStatusMenuCommand(
					new EventHandler(OnStatusIncludeInGroup),
					new EventHandler(OnMenuIncludeInGroup),
					ORMDesignerDocView.ORMDesignerCommandIds.IncludeInGroup)
					,new DynamicStatusMenuCommand(
					new EventHandler(OnStatusIncludeInNewGroup),
					new EventHandler(OnMenuIncludeInNewGroup),
					ORMDesignerDocView.ORMDesignerCommandIds.IncludeInNewGroup)
					,new DynamicIncludeInGroupCommand(
					new EventHandler(OnStatusIncludeInGroupList),
					new EventHandler(OnMenuIncludeInGroupList),
					ORMDesignerDocView.ORMDesignerCommandIds.IncludeInGroupList)
					,new DynamicDeleteFromGroupCommand(
					new EventHandler(OnStatusDeleteFromGroupList),
					new EventHandler(OnMenuDeleteFromGroupList),
					ORMDesignerDocView.ORMDesignerCommandIds.DeleteFromGroupList)
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
			public void OnStatusExclusiveOrDecoupler(object sender, EventArgs e)
			{
				ORMModelBrowserToolWindow.OnStatusCommand(sender, ORMDesignerCommands.ExclusiveOrDecoupler, CurrentToolWindow);
			}
			public void OnMenuExclusiveOrDecoupler(object sender, EventArgs e)
			{
				ORMModelBrowserToolWindow currentWindow = CurrentToolWindow;
				if (currentWindow != null)
				{
					currentWindow.OnMenuExclusiveOrDecoupler();
				}
			}
			public void OnStatusIncludeInGroup(object sender, EventArgs e)
			{
				ORMModelBrowserToolWindow.OnStatusCommand(sender, ORMDesignerCommands.IncludeInGroup, CurrentToolWindow);
			}
			public void OnMenuIncludeInGroup(object sender, EventArgs e)
			{
				ORMModelBrowserToolWindow currentWindow = CurrentToolWindow;
				if (currentWindow != null)
				{
					currentWindow.OnMenuIncludeInGroup();
				}
			}
			public void OnStatusIncludeInNewGroup(object sender, EventArgs e)
			{
				ORMModelBrowserToolWindow.OnStatusCommand(sender, ORMDesignerCommands.IncludeInNewGroup, CurrentToolWindow);
			}
			public void OnMenuIncludeInNewGroup(object sender, EventArgs e)
			{
				ORMModelBrowserToolWindow currentWindow = CurrentToolWindow;
				if (currentWindow != null)
				{
					currentWindow.OnMenuIncludeInNewGroup();
				}
			}
			private sealed class DynamicIncludeInGroupCommand : DynamicStatusMenuCommand
			{
				public DynamicIncludeInGroupCommand(EventHandler statusHandler, EventHandler invokeHandler, CommandID id)
					: base(statusHandler, invokeHandler, id)
				{
					//Declare class variable with object containing diagram list
				}
				public sealed override bool DynamicItemMatch(int cmdId)
				{
					int baseCmdId = CommandID.ID;
					int testId = cmdId - baseCmdId;


					if (testId >= 0 && testId < ORMDesignerDocView.ORMDesignerCommandIds.IncludeInGroupListLength)
					{
						MatchedCommandId = testId;
						return true;
					}
					return false;
				}
			}
			public void OnStatusIncludeInGroupList(object sender, EventArgs e)
			{
				ORMModelBrowserToolWindow.OnStatusCommand(sender, ORMDesignerCommands.IncludeInGroupList, CurrentToolWindow);
				((OleMenuCommand)sender).MatchedCommandId = 0;
			}
			public void OnMenuIncludeInGroupList(object sender, EventArgs e)
			{
				ORMModelBrowserToolWindow currentWindow = CurrentToolWindow;
				if (currentWindow != null)
				{
					currentWindow.OnMenuIncludeInGroupList(((OleMenuCommand)sender).MatchedCommandId);
				}
			}
			private sealed class DynamicDeleteFromGroupCommand : DynamicStatusMenuCommand
			{
				public DynamicDeleteFromGroupCommand(EventHandler statusHandler, EventHandler invokeHandler, CommandID id)
					: base(statusHandler, invokeHandler, id)
				{
					//Declare class variable with object containing diagram list
				}
				public sealed override bool DynamicItemMatch(int cmdId)
				{
					int baseCmdId = CommandID.ID;
					int testId = cmdId - baseCmdId;


					if (testId >= 0 && testId < ORMDesignerDocView.ORMDesignerCommandIds.DeleteFromGroupListLength)
					{
						MatchedCommandId = testId;
						return true;
					}
					return false;
				}
			}
			public void OnStatusDeleteFromGroupList(object sender, EventArgs e)
			{
				ORMModelBrowserToolWindow.OnStatusCommand(sender, ORMDesignerCommands.DeleteFromGroupList, CurrentToolWindow);
				((OleMenuCommand)sender).MatchedCommandId = 0;
			}
			public void OnMenuDeleteFromGroupList(object sender, EventArgs e)
			{
				ORMModelBrowserToolWindow currentWindow = CurrentToolWindow;
				if (currentWindow != null)
				{
					currentWindow.OnMenuDeleteFromGroupList(((OleMenuCommand)sender).MatchedCommandId);
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
			private void OnStatusFreeFormCommand(object sender, EventArgs e)
			{
				ORMModelBrowserToolWindow.OnStatusCommand(sender, ORMDesignerCommands.FreeFormCommandList, CurrentToolWindow);
				((OleMenuCommand)sender).MatchedCommandId = 0;
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			private void OnMenuFreeFormCommand(object sender, EventArgs e)
			{
				ORMModelBrowserToolWindow currentWindow = CurrentToolWindow;
				if (currentWindow != null)
				{
					// Defer to the doc view
					currentWindow.OnMenuFreeFormCommand(((OleMenuCommand)sender).MatchedCommandId);
				}
			}
			private sealed class DynamicFreeFormCommand : DynamicStatusMenuCommand
			{
				public DynamicFreeFormCommand(EventHandler statusHandler, EventHandler invokeHandler, CommandID id)
					: base(statusHandler, invokeHandler, id)
				{
				}
				public sealed override bool DynamicItemMatch(int cmdId)
				{
					int baseCmdId = CommandID.ID;
					int testId = cmdId - baseCmdId;


					if (testId >= 0 && testId < ORMDesignerDocView.ORMDesignerCommandIds.FreeFormCommandListLength)
					{
						MatchedCommandId = testId;
						return true;
					}
					return false;
				}
			}
			#endregion // Command Handlers
		}
	}
}
