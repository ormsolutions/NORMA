#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright � Neumont University. All rights reserved.                     *
* Copyright � ORM Solutions, LLC. All rights reserved.                     *
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
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using ORMSolutions.ORMArchitect.Core;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Core.ShapeModel;
using Microsoft.VisualStudio;
using System.ComponentModel;

namespace ORMSolutions.ORMArchitect.Core.Shell
{
	public partial class ORMDesignerDocView
	{
		/// <summary>
		/// Create a command set for this type of view. Should be called
		/// once when the package loads
		/// </summary>
		/// <param name="serviceProvider">IServiceProvider</param>
		/// <returns></returns>
		public static CommandSet CreateCommandSet(IServiceProvider serviceProvider)
		{
			return new ORMDesignerCommandSet(serviceProvider);
		}
		/// <summary>
		/// Command objects for the ORMDesignerDocView
		/// </summary>
		protected class ORMDesignerCommandSet : CommandSet, IEnumerable<MenuCommand>, IDisposable
		{
			/// <summary>
			/// Commands
			/// </summary>
			private List<MenuCommand> myCommands;

			/// <summary>
			/// ORMDesignerCommandSet constructor
			/// </summary>
			/// <param name="serviceProvider"></param>
			public ORMDesignerCommandSet(IServiceProvider serviceProvider)
				: base(serviceProvider)
			{
				List<MenuCommand> commands = this.myCommands = new List<MenuCommand>
				(
					#region Array of menu commands
					new MenuCommand[]
					{
						// Commands
						new DynamicStatusMenuCommand(
						new EventHandler(OnStatusDebugCommand),
						new EventHandler(OnMenuDebugViewStore),
						ORMDesignerCommandIds.DebugViewStore),
						new DynamicStatusMenuCommand(
						new EventHandler(OnStatusDebugCommand),
						new EventHandler(OnMenuDebugViewTransactionLogs),
						ORMDesignerCommandIds.DebugViewTransactionLogs),
						new DynamicStatusMenuCommand(
						new EventHandler(OnStatusStandardWindow),
						new EventHandler(OnMenuReferenceModesWindow),
						ORMDesignerCommandIds.ViewReferenceModeEditor)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusStandardWindow),
						new EventHandler(OnMenuInformalDescriptionWindow),
						ORMDesignerCommandIds.ViewInformalDescriptionWindow)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusStandardWindow),
						new EventHandler(OnMenuNotesWindow),
						ORMDesignerCommandIds.ViewNotesWindow)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusStandardWindow),
						new EventHandler(OnMenuContextWindow),
						ORMDesignerCommandIds.ViewContextWindow)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusStandardWindow),
						new EventHandler(OnMenuDiagramSpyWindow),
						ORMDesignerCommandIds.ViewDiagramSpyWindow)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusCopyImage),
						new EventHandler(OnMenuCopyImage),
						ORMDesignerCommandIds.CopyImage)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusDeleteAlternate),
						new EventHandler(OnMenuDeleteAlternate),
						ORMDesignerCommandIds.DeleteAlternate)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusSelectAll),
						new EventHandler(OnMenuSelectAll),
						StandardCommands.SelectAll)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusDelete),
						new EventHandler(OnMenuDelete),
						StandardCommands.Delete)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusReadingsWindow),
						new EventHandler(OnMenuReadingsWindow),
						ORMDesignerCommandIds.ViewReadingEditor)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusInsertRole),
						new EventHandler(OnMenuInsertRoleBefore),
						ORMDesignerCommandIds.InsertRoleBefore)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusInsertRole),
						new EventHandler(OnMenuInsertRoleAfter),
						ORMDesignerCommandIds.InsertRoleAfter)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusStandardWindow),
						new EventHandler(OnMenuFactEditorWindow),
						ORMDesignerCommandIds.ViewFactEditor)
						// Constraint editing commands
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusActivateRoleSequence),
						new EventHandler(OnMenuActivateRoleSequence),
						ORMDesignerCommandIds.ViewActivateRoleSequence)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusDeleteRowSequence),
						new EventHandler(OnMenuDeleteRowSequence),
						ORMDesignerCommandIds.ViewDeleteRoleSequence)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusMoveRoleSequenceUp),
						new EventHandler(OnMenuMoveRoleSequenceUp),
						ORMDesignerCommandIds.ViewMoveRoleSequenceUp)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusMoveRoleSequenceDown),
						new EventHandler(OnMenuMoveRoleSequenceDown),
						ORMDesignerCommandIds.ViewMoveRoleSequenceDown)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusEditRoleSequenceConstraint),
						new EventHandler(OnMenuEditRoleSequenceConstraint),
						ORMDesignerCommandIds.ViewEditRoleSequenceConstraint)

						// Verbalization Commands
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusShowPositiveVerbalization),
						new EventHandler(OnMenuShowPositiveVerbalization),
						ORMDesignerCommandIds.ShowPositiveVerbalization)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusVerbalizationHyperlinkToDiagramSpy),
						new EventHandler(OnMenuVerbalizationHyperlinkToDiagramSpy),
						ORMDesignerCommandIds.VerbalizationHyperlinkToDiagramSpy)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusShowNegativeVerbalization),
						new EventHandler(OnMenuShowNegativeVerbalization),
						ORMDesignerCommandIds.ShowNegativeVerbalization)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusStandardWindow),
						new EventHandler(OnMenuVerbalizationWindow),
						ORMDesignerCommandIds.ViewVerbalizationBrowser)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusStandardWindow),
						new EventHandler(OnMenuSamplePopulationEditor),
						ORMDesignerCommandIds.ViewSamplePopulationEditor)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusStandardWindow),
						new EventHandler(OnMenuViewORMModelBrowser),
						ORMDesignerCommandIds.ViewModelBrowser)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusAutoLayout),
						new EventHandler(OnMenuAutoLayout),
						ORMDesignerCommandIds.AutoLayout)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusToggleSimpleMandatory),
						new EventHandler(OnMenuToggleSimpleMandatory),
						ORMDesignerCommandIds.ToggleSimpleMandatory)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusAddInternalUniqueness),
						new EventHandler(OnMenuAddInternalUniqueness),
						ORMDesignerCommandIds.AddInternalUniqueness)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusExtensionManager),
						new EventHandler(OnMenuExtensionManager),
						ORMDesignerCommandIds.ExtensionManager)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusMoveRoleLeft), 
						new EventHandler(OnMenuMoveRoleLeft),
						ORMDesignerCommandIds.MoveRoleLeft)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusMoveRoleRight), 
						new EventHandler(OnMenuMoveRoleRight),
						ORMDesignerCommandIds.MoveRoleRight)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusObjectifyFactType), 
						new EventHandler(OnMenuObjectifyFactType),
						ORMDesignerCommandIds.ObjectifyFactType)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusDisplayOrientationHorizontal), 
						new EventHandler(OnMenuDisplayOrientationHorizontal),
						ORMDesignerCommandIds.DisplayOrientationHorizontal)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusDisplayOrientationRotatedLeft), 
						new EventHandler(OnMenuDisplayOrientationRotatedLeft),
						ORMDesignerCommandIds.DisplayOrientationRotatedLeft)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusDisplayOrientationRotatedRight), 
						new EventHandler(OnMenuDisplayOrientationRotatedRight),
						ORMDesignerCommandIds.DisplayOrientationRotatedRight)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusDisplayConstraintsOnTop), 
						new EventHandler(OnMenuDisplayConstraintsOnTop),
						ORMDesignerCommandIds.DisplayConstraintsOnTop)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusDisplayConstraintsOnBottom), 
						new EventHandler(OnMenuDisplayConstraintsOnBottom),
						ORMDesignerCommandIds.DisplayConstraintsOnBottom)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusDisplayReverseRoleOrder), 
						new EventHandler(OnMenuDisplayReverseRoleOrder),
						ORMDesignerCommandIds.DisplayReverseRoleOrder)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusExclusiveOrCoupler), 
						new EventHandler(OnMenuExclusiveOrCoupler),
						ORMDesignerCommandIds.ExclusiveOrCoupler)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusExclusiveOrDecoupler), 
						new EventHandler(OnMenuExclusiveOrDecoupler),
						ORMDesignerCommandIds.ExclusiveOrDecoupler)
						,new DynamicFreeFormCommand(
						new EventHandler(OnStatusFreeFormCommand),
						new EventHandler(OnMenuFreeFormCommand),
						ORMDesignerCommandIds.FreeFormCommandList)
						,new DynamicReportGeneratorCommand(
						new EventHandler(OnStatusReportGenerator),
						new EventHandler(OnMenuReportGenerator),
						ORMDesignerCommandIds.ReportGeneratorList)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusUnobjectifyFactType), 
						new EventHandler(OnMenuUnobjectifyFactType),
						ORMDesignerCommandIds.UnobjectifyFactType)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusDragInverseUnaryFactType),
						new EventHandler(OnMenuDragInverseUnaryFactType),
						ORMDesignerCommandIds.DragInverseUnaryFactType)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusDragLinkFactType),
						new EventHandler(OnMenuDragLinkFactType),
						ORMDesignerCommandIds.DragLinkFactType)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusDragNegatedUnaryConstraints),
						new EventHandler(OnMenuDragNegatedUnaryConstraints),
						ORMDesignerCommandIds.DragNegatedUnaryConstraints)
						// Alignment Commands
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusAlignShapes),
						new EventHandler(OnMenuAlignShapes),
						StandardCommands.AlignBottom)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusAlignShapes),
						new EventHandler(OnMenuAlignShapes),
						StandardCommands.AlignHorizontalCenters)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusAlignShapes),
						new EventHandler(OnMenuAlignShapes),
						StandardCommands.AlignLeft)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusAlignShapes),
						new EventHandler(OnMenuAlignShapes),
						StandardCommands.AlignRight)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusAlignShapes),
						new EventHandler(OnMenuAlignShapes),
						StandardCommands.AlignTop)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusAlignShapes),
						new EventHandler(OnMenuAlignShapes),
						StandardCommands.AlignVerticalCenters)
						,new DynamicErrorCommand(
						new EventHandler(OnStatusErrorList),
						new EventHandler(OnMenuErrorList),
						ORMDesignerCommandIds.ErrorList)
						,new DynamicErrorCommand(
						new EventHandler(OnStatusErrorList), // Share status with error list, items have the same content
						new EventHandler(OnMenuDisableErrorList),
						ORMDesignerCommandIds.DisableErrorList)
						,new DynamicErrorCommand(
						new EventHandler(OnStatusEnableErrorList),
						new EventHandler(OnMenuEnableErrorList),
						ORMDesignerCommandIds.EnableErrorList)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusSelectNextInCurrentDiagram),
						new EventHandler(OnMenuSelectNextInCurrentDiagram),
						ORMDesignerCommandIds.SelectNextInCurrentDiagram)
						,new DynamicDiagramCommand(
						new EventHandler(OnStatusDiagramList),
						new EventHandler(OnMenuDiagramList),
						ORMDesignerCommandIds.DiagramList)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusSelectInDiagramSpy),
						new EventHandler(OnMenuSelectInDiagramSpy),
						ORMDesignerCommandIds.SelectInDiagramSpy)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusSelectInModelBrowser),
						new EventHandler(OnMenuSelectInModelBrowser),
						ORMDesignerCommandIds.SelectInModelBrowser)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusSelectInDocumentWindow),
						new EventHandler(OnMenuSelectInDocumentWindow),
						ORMDesignerCommandIds.SelectInDocumentWindow)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusDiagramSpyAllDiagrams),
						new EventHandler(OnMenuDiagramSpyAllDiagrams),
						ORMDesignerCommandIds.DiagramSpyAllDiagrams)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusIncludeInNewGroup),
						new EventHandler(OnMenuIncludeInNewGroup),
						ORMDesignerCommandIds.IncludeInNewGroup)
						,new DynamicGroupCommand(
						new EventHandler(OnStatusIncludeInGroupList),
						new EventHandler(OnMenuIncludeInGroupList),
						ORMDesignerCommandIds.IncludeInGroupList)
						,new DynamicGroupCommand(
						new EventHandler(OnStatusDeleteFromGroupList),
						new EventHandler(OnMenuDeleteFromGroupList),
						ORMDesignerCommandIds.DeleteFromGroupList)
						,new DynamicGroupCommand(
						new EventHandler(OnStatusSelectInGroupList),
						new EventHandler(OnMenuSelectInGroupList),
						ORMDesignerCommandIds.SelectInGroupList)
						,new MenuCommand(
						new EventHandler(OnMenuNewWindow),
						new CommandID(VSConstants.GUID_VSStandardCommandSet97, (int)VSConstants.VSStd97CmdID.NewWindow))
						//Reading Editor Commands
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusAddReading),
						new EventHandler(OnMenuAddReading),
						ORMDesignerCommandIds.ReadingEditorAddReading)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusAddReadingOrder),
						new EventHandler(OnMenuAddReadingOrder),
						ORMDesignerCommandIds.ReadingEditorAddReadingOrder)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusDeleteReading),
						new EventHandler(OnMenuDeleteReading),
						ORMDesignerCommandIds.ReadingEditorDeleteReading)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusPromoteReading),
						new EventHandler(OnMenuPromoteReading),
						ORMDesignerCommandIds.ReadingEditorPromoteReading)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusDemoteReading),
						new EventHandler(OnMenuDemoteReading),
						ORMDesignerCommandIds.ReadingEditorDemoteReading)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusReadingEditorPromoteReadingOrder),
						new EventHandler(OnMenuReadingEditorPromoteReadingOrder),
						ORMDesignerCommandIds.ReadingEditorPromoteReadingOrder)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusReadingEditorDemoteReadingOrder),
						new EventHandler(OnMenuReadingEditorDemoteReadingOrder),
						ORMDesignerCommandIds.ReadingEditorDemoteReadingOrder)
						,new ZoomMenuCommand(
						new EventHandler(OnStatusZoom),
						new EventHandler(OnMenuZoom),
						ORMDesignerCommandIds.Zoom10,
						.1f)
						,new ZoomMenuCommand(
						new EventHandler(OnStatusZoom),
						new EventHandler(OnMenuZoom),
						ORMDesignerCommandIds.Zoom25,
						.25f)
						,new ZoomMenuCommand(
						new EventHandler(OnStatusZoom),
						new EventHandler(OnMenuZoom),
						ORMDesignerCommandIds.Zoom33,
						1f/3)
						,new ZoomMenuCommand(
						new EventHandler(OnStatusZoom),
						new EventHandler(OnMenuZoom),
						ORMDesignerCommandIds.Zoom50,
						.5f)
						,new ZoomMenuCommand(
						new EventHandler(OnStatusZoom),
						new EventHandler(OnMenuZoom),
						ORMDesignerCommandIds.Zoom66,
						2f/3)
						,new ZoomMenuCommand(
						new EventHandler(OnStatusZoom),
						new EventHandler(OnMenuZoom),
						ORMDesignerCommandIds.Zoom75,
						.75f)
						,new ZoomMenuCommand(
						new EventHandler(OnStatusZoom),
						new EventHandler(OnMenuZoom),
						ORMDesignerCommandIds.Zoom100,
						1f)
						,new ZoomMenuCommand(
						new EventHandler(OnStatusZoom),
						new EventHandler(OnMenuZoom),
						ORMDesignerCommandIds.Zoom125,
						1.25f)
						,new ZoomMenuCommand(
						new EventHandler(OnStatusZoom),
						new EventHandler(OnMenuZoom),
						ORMDesignerCommandIds.Zoom150,
						1.5f)
						,new ZoomMenuCommand(
						new EventHandler(OnStatusZoom),
						new EventHandler(OnMenuZoom),
						ORMDesignerCommandIds.Zoom200,
						2f)
						,new ZoomMenuCommand(
						new EventHandler(OnStatusZoom),
						new EventHandler(OnMenuZoom),
						ORMDesignerCommandIds.Zoom300,
						3f)
						,new ZoomMenuCommand(
						new EventHandler(OnStatusZoom),
						new EventHandler(OnMenuZoom),
						ORMDesignerCommandIds.Zoom400,
						4f)
						,new ZoomMenuCommand(
						new EventHandler(OnStatusZoom),
						new EventHandler(OnMenuZoom),
						ORMDesignerCommandIds.ZoomIn,
						float.MaxValue)
						,new ZoomMenuCommand(
						new EventHandler(OnStatusZoom),
						new EventHandler(OnMenuZoom),
						ORMDesignerCommandIds.ZoomOut,
						float.MinValue)
#if VSIX_Per_User
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusLoadNORMASchemas),
						new EventHandler(OnMenuLoadNORMASchemas),
						ORMDesignerCommandIds.LoadNORMASchemas)
#endif // VSIX_Per_User
						,new OleMenuCommand(
						new EventHandler(OnMenuORMDesignerOptions),
						ORMDesignerCommandIds.ORMDesignerOptions)
					}
					#endregion
				);

				// Get Print and Page Setup command handlers from our base class
				foreach (MenuCommand menuCommand in base.GetMenuCommands())
				{
					CommandID commandID = menuCommand.CommandID;
					if (commandID.Equals(CommonModelingCommands.PageSetup) || commandID.Equals(CommonModelingCommands.Print))
					{
						commands.Add(menuCommand);
					}
				}
			}

			/// <summary>
			/// See <see cref="CommandSet.GetMenuCommands"/>.
			/// </summary>
			protected override IList<MenuCommand> GetMenuCommands()
			{
				return this.myCommands;
			}

			/// <summary>
			/// Called to remove a set of commands. This should be called
			/// by Dispose.
			/// </summary>
			/// <param name="commands">Commands to add</param>
			protected virtual void RemoveCommands(IList<MenuCommand> commands)
			{
				IMenuCommandService menuService = base.MenuService;
				if (menuService != null)
				{
					foreach (MenuCommand menuCommand in commands)
					{
						menuService.RemoveCommand(menuCommand);
					}
				}
			}
			/// <summary>
			/// Cleanup code 
			/// </summary>
			public void Dispose()
			{
				if (myCommands != null)
				{
					RemoveCommands(myCommands);
				}
				myCommands = null;
			}

			/// <summary>
			/// Show the New ORM Model Browser
			/// </summary>
			protected void OnMenuViewORMModelBrowser(object sender, EventArgs e)
			{
				ORMDesignerPackage.ORMModelBrowserWindow.Show();
			}

			/// <summary>
			/// Status callback
			/// </summary>
			protected void OnStatusCopyImage(object sender, EventArgs e)
			{
				ORMDesignerCommandManager.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.CopyImage);
			}
			/// <summary>
			/// Copies as image
			/// </summary>
			protected void OnMenuCopyImage(object sender, EventArgs e)
			{
				IORMDesignerView designerView = CurrentORMView;
				if (designerView != null)
				{
					// call CopyImage on the doc view
					designerView.CommandManager.OnMenuCopyImage();
				}
			}

			/// <summary>
			/// Status callback
			/// </summary>
			protected void OnStatusDebugCommand(object sender, EventArgs e)
			{
				MenuCommand command = sender as MenuCommand;
				bool turnOn = OptionsPage.CurrentShowDebugCommands;
				command.Enabled = true; // No reason to disable it, we just don't want to show it
				command.Visible = turnOn;
			}
			/// <summary>
			/// Show a debug window displaying the contents of the current store
			/// </summary>
			protected void OnMenuDebugViewStore(object sender, EventArgs e)
			{
				Microsoft.VisualStudio.Modeling.Diagnostics.StoreViewer.Show(((ModelingDocData)CurrentORMView.DocData).Store);
			}
			/// <summary>
			/// Show a debug window displaying the contents of the current store
			/// </summary>
			protected void OnMenuDebugViewTransactionLogs(object sender, EventArgs e)
			{
				Store store = ((ModelingDocData)CurrentORMView.DocData).Store;
				if (store != null && !store.Disposed)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TransactionLogViewer.Show(store, ((IORMToolServices)store).ServiceProvider);
				}
			}

			/// <summary>
			/// Status callback
			/// </summary>
			private void OnStatusDeleteAlternate(object sender, EventArgs e)
			{
				ORMDesignerCommandManager.OnStatusCommand(
					sender,
					CurrentORMView,
					(OptionsPage.CurrentPrimaryDeleteBehavior == PrimaryDeleteBehavior.DeleteShape) ?
						ORMDesignerCommands.Delete | ORMDesignerCommands.DeleteAny :
						ORMDesignerCommands.DeleteShape | ORMDesignerCommands.DeleteAnyShape);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			private void OnMenuDeleteAlternate(object sender, EventArgs e)
			{
				IORMDesignerView designerView = CurrentORMView;
				if (designerView != null)
				{
					// call the appropriate delete on the doc view
					switch (OptionsPage.CurrentPrimaryDeleteBehavior)
					{
						case PrimaryDeleteBehavior.DeleteShape:
							designerView.CommandManager.OnMenuDeleteElement((sender as OleMenuCommand).Text);
							break;
						case PrimaryDeleteBehavior.DeleteElement:
							designerView.CommandManager.OnMenuDeleteShape((sender as OleMenuCommand).Text);
							break;
					}
				}
			}
			/// <summary>
			/// Status callback
			/// </summary>
			private void OnStatusDelete(object sender, EventArgs e)
			{
				ORMDesignerCommandManager.OnStatusCommand(
					sender,
					CurrentORMView,
					(OptionsPage.CurrentPrimaryDeleteBehavior == PrimaryDeleteBehavior.DeleteElement) ?
						ORMDesignerCommands.Delete | ORMDesignerCommands.DeleteAny :
						ORMDesignerCommands.DeleteShape | ORMDesignerCommands.DeleteAnyShape);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			private void OnMenuDelete(object sender, EventArgs e)
			{
				IORMDesignerView designerView = CurrentORMView;
				if (designerView != null)
				{
					// call the appropriate delete on the doc view
					switch (OptionsPage.CurrentPrimaryDeleteBehavior)
					{
						case PrimaryDeleteBehavior.DeleteElement:
							designerView.CommandManager.OnMenuDeleteElement((sender as OleMenuCommand).Text);
							break;
						case PrimaryDeleteBehavior.DeleteShape:
							designerView.CommandManager.OnMenuDeleteShape((sender as OleMenuCommand).Text);
							break;
					}
				}
			}
			/// <summary>
			/// Status callback
			/// </summary>
			private void OnStatusReadingsWindow(object sender, EventArgs e)
			{
				ORMDesignerCommandManager.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.DisplayStandardWindows);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuReadingsWindow(object sender, EventArgs e)
			{
				ORMReadingEditorToolWindow editorWindow = ORMDesignerPackage.ReadingEditorWindow;
				editorWindow.Show();
			}

			private void OnStatusSelectAll(object sender, EventArgs e)
			{
				ORMDesignerCommandManager.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.SelectAll);
			}
			/// <summary>
			/// Menu handler for the SelectAll command
			/// </summary>
			protected void OnMenuSelectAll(object sender, EventArgs e)
			{
				IORMDesignerView designerView = CurrentORMView;
				if (designerView != null)
				{
					// call delete on the doc view
					designerView.CommandManager.OnMenuSelectAll();
				}
			}
			private void OnStatusAutoLayout(object sender, EventArgs e)
			{
				ORMDesignerCommandManager.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.AutoLayout);
			}
			/// <summary>
			/// Menu handler for the SelectAll command
			/// </summary>
			protected void OnMenuAutoLayout(object sender, EventArgs e)
			{
				IORMDesignerView designerView = CurrentORMView;
				if (designerView != null)
				{
					// call auto layout on the doc view
					designerView.CommandManager.OnMenuAutoLayout();
				}
			}
			/// <summary>
			/// Status callback
			/// </summary>
			private void OnStatusInsertRole(object sender, EventArgs e)
			{
				ORMDesignerCommandManager.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.InsertRole);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuInsertRoleAfter(object sender, EventArgs e)
			{
				IORMDesignerView designerView = CurrentORMView;
				if (designerView != null)
				{
					// Defer to the doc view
					designerView.CommandManager.OnMenuInsertRole(true);
				}
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuInsertRoleBefore(object sender, EventArgs e)
			{
				IORMDesignerView designerView = CurrentORMView;
				if (designerView != null)
				{
					// Defer to the doc view
					designerView.CommandManager.OnMenuInsertRole(false);
				}
			}
			/// <summary>
			/// Status callback
			/// </summary>
			private void OnStatusToggleSimpleMandatory(object sender, EventArgs e)
			{
				ORMDesignerCommandManager.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.ToggleSimpleMandatory);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuToggleSimpleMandatory(object sender, EventArgs e)
			{
				IORMDesignerView designerView = CurrentORMView;
				if (designerView != null)
				{
					// Defer to the doc view
					designerView.CommandManager.OnMenuToggleSimpleMandatory();
				}
			}
			/// <summary>
			/// Status callback
			/// </summary>
			private void OnStatusAddInternalUniqueness(object sender, EventArgs e)
			{
				ORMDesignerCommandManager.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.AddInternalUniqueness);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuAddInternalUniqueness(object sender, EventArgs e)
			{
				IORMDesignerView designerView = CurrentORMView;
				if (designerView != null)
				{
					// Defer to the doc view
					designerView.CommandManager.OnMenuAddInternalUniqueness();
				}
			}
			/// <summary>
			/// Status callback
			/// </summary>
			private void OnStatusExtensionManager(object sender, EventArgs e)
			{
				ORMDesignerCommandManager.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.ExtensionManager);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuExtensionManager(object sender, EventArgs e)
			{
				IORMDesignerView designerView = CurrentORMView;
				if (designerView != null)
				{
					// Defer to the doc view
					designerView.CommandManager.OnMenuExtensionManager();
				}
			}
			private void OnStatusMoveRoleLeft(object sender, EventArgs e)
			{
				ORMDesignerCommandManager.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.MoveRoleLeft);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuMoveRoleLeft(object sender, EventArgs e)
			{
				IORMDesignerView designerView = CurrentORMView;
				if (designerView != null)
				{
					// Defer to the doc view
					designerView.CommandManager.OnMenuMoveRoleLeft();
				}
			}
			private void OnStatusMoveRoleRight(object sender, EventArgs e)
			{
				ORMDesignerCommandManager.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.MoveRoleRight);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuMoveRoleRight(object sender, EventArgs e)
			{
				IORMDesignerView designerView = CurrentORMView;
				if (designerView != null)
				{
					// Defer to the doc view
					designerView.CommandManager.OnMenuMoveRoleRight();
				}
			}
			private void OnStatusObjectifyFactType(object sender, EventArgs e)
			{
				ORMDesignerCommandManager.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.ObjectifyFactType);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuObjectifyFactType(object sender, EventArgs e)
			{
				IORMDesignerView designerView = CurrentORMView;
				if (designerView != null)
				{
					// Defer to the doc view
					designerView.CommandManager.OnMenuObjectifyFactType();
				}
			}
			private void OnStatusDisplayOrientationHorizontal(object sender, EventArgs e)
			{
				ORMDesignerCommandManager.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.DisplayOrientationHorizontal);
			}
			private void OnStatusDisplayOrientationRotatedLeft(object sender, EventArgs e)
			{
				ORMDesignerCommandManager.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.DisplayOrientationRotatedLeft);
			}
			private void OnStatusDisplayOrientationRotatedRight(object sender, EventArgs e)
			{
				ORMDesignerCommandManager.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.DisplayOrientationRotatedRight);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuDisplayOrientationHorizontal(object sender, EventArgs e)
			{
				OnMenuDisplayOrientation(DisplayOrientation.Horizontal);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuDisplayOrientationRotatedLeft(object sender, EventArgs e)
			{
				OnMenuDisplayOrientation(DisplayOrientation.VerticalRotatedLeft);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuDisplayOrientationRotatedRight(object sender, EventArgs e)
			{
				OnMenuDisplayOrientation(DisplayOrientation.VerticalRotatedRight);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			private void OnMenuDisplayOrientation(DisplayOrientation orientation)
			{
				IORMDesignerView designerView = CurrentORMView;
				if (designerView != null)
				{
					// Defer to the doc view
					designerView.CommandManager.OnMenuDisplayOrientation(orientation);
				}
			}
			private void OnStatusDisplayConstraintsOnTop(object sender, EventArgs e)
			{
				ORMDesignerCommandManager.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.DisplayConstraintsOnTop);
			}
			private void OnStatusDisplayConstraintsOnBottom(object sender, EventArgs e)
			{
				ORMDesignerCommandManager.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.DisplayConstraintsOnBottom);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuDisplayConstraintsOnTop(object sender, EventArgs e)
			{
				OnMenuDisplayConstraintPosition(ConstraintDisplayPosition.Top);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuDisplayConstraintsOnBottom(object sender, EventArgs e)
			{
				OnMenuDisplayConstraintPosition(ConstraintDisplayPosition.Bottom);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			private void OnMenuDisplayConstraintPosition(ConstraintDisplayPosition position)
			{
				IORMDesignerView designerView = CurrentORMView;
				if (designerView != null)
				{
					// Defer to the doc view
					designerView.CommandManager.OnMenuDisplayConstraintPosition(position);
				}
			}
			private void OnStatusDisplayReverseRoleOrder(object sender, EventArgs e)
			{
				ORMDesignerCommandManager.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.DisplayReverseRoleOrder);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			private void OnMenuDisplayReverseRoleOrder(object sender, EventArgs e)
			{
				IORMDesignerView designerView = CurrentORMView;
				if (designerView != null)
				{
					// Defer to the doc view
					designerView.CommandManager.OnMenuDisplayReverseRoleOrder();
				}
			}
			private void OnStatusExclusiveOrCoupler(object sender, EventArgs e)
			{
				ORMDesignerCommandManager.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.ExclusiveOrCoupler);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			private void OnMenuExclusiveOrCoupler(object sender, EventArgs e)
			{
				IORMDesignerView designerView = CurrentORMView;
				if (designerView != null)
				{
					// Defer to the doc view
					designerView.CommandManager.OnMenuExclusiveOrCoupler();
				}
			}
			private void OnStatusExclusiveOrDecoupler(object sender, EventArgs e)
			{
				ORMDesignerCommandManager.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.ExclusiveOrDecoupler);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			private void OnMenuExclusiveOrDecoupler(object sender, EventArgs e)
			{
				IORMDesignerView designerView = CurrentORMView;
				if (designerView != null)
				{
					// Defer to the doc view
					designerView.CommandManager.OnMenuExclusiveOrDecoupler();
				}
			}
			private void OnStatusFreeFormCommand(object sender, EventArgs e)
			{
				ORMDesignerCommandManager.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.FreeFormCommandList);
				((OleMenuCommand)sender).MatchedCommandId = 0;
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			private void OnMenuFreeFormCommand(object sender, EventArgs e)
			{
				IORMDesignerView designerView = CurrentORMView;
				if (designerView != null)
				{
					// Defer to the doc view
					designerView.CommandManager.OnMenuFreeFormCommand(((OleMenuCommand)sender).MatchedCommandId);
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


					if (testId >= 0 && testId < ORMDesignerCommandIds.FreeFormCommandListLength)
					{
						MatchedCommandId = testId;
						return true;
					}
					return false;
				}
			}
			private void OnStatusReportGenerator(object sender, EventArgs e)
			{
				ORMDesignerCommandManager.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.ReportGeneratorList);
				((OleMenuCommand)sender).MatchedCommandId = 0;
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			private void OnMenuReportGenerator(object sender, EventArgs e)
			{
				IORMDesignerView designerView = CurrentORMView;
				if (designerView != null)
				{
					// Defer to the doc view
					designerView.CommandManager.OnMenuGenerateReport(((OleMenuCommand)sender).MatchedCommandId);
				}
			}
			private sealed class DynamicReportGeneratorCommand : DynamicStatusMenuCommand
			{
				public DynamicReportGeneratorCommand(EventHandler statusHandler, EventHandler invokeHandler, CommandID id)
					: base(statusHandler, invokeHandler, id)
				{
				}
				public sealed override bool DynamicItemMatch(int cmdId)
				{
					int baseCmdId = CommandID.ID;
					int testId = cmdId - baseCmdId;


					if (testId >= 0 && testId < ORMDesignerCommandIds.ReportGeneratorListLength)
					{
						MatchedCommandId = testId;
						return true;
					}
					return false;
				}
			}
			private void OnStatusUnobjectifyFactType(object sender, EventArgs e)
			{
				ORMDesignerCommandManager.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.UnobjectifyFactType);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuUnobjectifyFactType(object sender, EventArgs e)
			{
				IORMDesignerView designerView = CurrentORMView;
				if (designerView != null)
				{
					// Defer to the doc view
					designerView.CommandManager.OnMenuUnobjectifyFactType();
				}
			}
			private void OnStatusDragInverseUnaryFactType(object sender, EventArgs e)
			{
				ORMDesignerCommandManager.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.DragInverseUnaryFactType);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuDragInverseUnaryFactType(object sender, EventArgs e)
			{
				IORMDesignerView designerView = CurrentORMView;
				if (designerView != null)
				{
					// Defer to the doc view
					designerView.CommandManager.OnMenuDragInverseUnaryFactType();
				}
			}
			private void OnStatusDragLinkFactType(object sender, EventArgs e)
			{
				ORMDesignerCommandManager.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.DragLinkFactType);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuDragLinkFactType(object sender, EventArgs e)
			{
				IORMDesignerView designerView = CurrentORMView;
				if (designerView != null)
				{
					// Defer to the doc view
					designerView.CommandManager.OnMenuDragLinkFactType();
				}
			}
			private void OnStatusDragNegatedUnaryConstraints(object sender, EventArgs e)
			{
				ORMDesignerCommandManager.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.DragNegatedUnaryConstraints);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuDragNegatedUnaryConstraints(object sender, EventArgs e)
			{
				IORMDesignerView designerView = CurrentORMView;
				if (designerView != null)
				{
					// Defer to the doc view
					designerView.CommandManager.OnMenuDragNegatedUnaryConstraints();
				}
			}
			/// <summary>
			/// Status callback
			/// </summary>
			private void OnStatusAlignShapes(object sender, EventArgs e)
			{
				ORMDesignerCommandManager.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.AlignShapes);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuAlignShapes(object sender, EventArgs e)
			{
				IORMDesignerView designerView = CurrentORMView;
				if (designerView != null)
				{
					// Defer to the doc view
					designerView.CommandManager.OnMenuAlignShapes((sender as MenuCommand).CommandID.ID);
				}
			}
			private sealed class ZoomMenuCommand : DynamicStatusMenuCommand
			{
				/// <summary>
				/// The zoom factor for this command
				/// </summary>
				public readonly float ZoomFactor;
				/// <summary>
				/// Create a zoom command
				/// </summary>
				public ZoomMenuCommand(EventHandler statusHandler, EventHandler invokeHandler, CommandID id, float zoomFactor)
					: base(statusHandler, invokeHandler, id)
				{
					ZoomFactor = zoomFactor;
				}
			}

			/// <summary>
			/// Status callback
			/// </summary>
			private void OnStatusZoom(object sender, EventArgs e)
			{
				ORMDesignerCommandManager.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.Zoom);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuZoom(object sender, EventArgs e)
			{
				IORMDesignerView designerView = CurrentORMView;
				DiagramView diagramView;
				DiagramClientView clientView;
				ZoomMenuCommand command;
				if (null != (designerView = CurrentORMView) &&
					null != (diagramView = designerView.CurrentDesigner) &&
					null != (clientView = diagramView.DiagramClientView) &&
					null != (command = sender as ZoomMenuCommand))
				{
					float zoomFactor = command.ZoomFactor;
					if (zoomFactor == float.MinValue)
					{
						clientView.ZoomOut();
					}
					else if (zoomFactor == float.MaxValue)
					{
						clientView.ZoomIn();
					}
					else
					{
						clientView.ZoomAtViewCenter(zoomFactor);
					}
				}
			}
#if VSIX_Per_User
			/// <summary>
			/// Status callback
			/// </summary>
			private void OnStatusLoadNORMASchemas(object sender, EventArgs e)
			{
				IMonitorSelectionService monitorSelection;
				object selectionContainer;
				bool inXmlEditor =
					null != (monitorSelection = MonitorSelection) &&
					null != (selectionContainer = monitorSelection.CurrentSelectionContainer) &&
					selectionContainer.GetType().FullName == "Microsoft.XmlEditor.XmlDocumentProperties";

				MenuCommand command = (MenuCommand)sender;
				command.Supported = true;
				command.Enabled = inXmlEditor;
				command.Visible = inXmlEditor;
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuLoadNORMASchemas(object sender, EventArgs e)
			{
				EnvDTE.DTE dte;
#if VISUALSTUDIO_16_0
				if (null != (dte = ORMDesignerPackage.Singleton.GetService<EnvDTE.DTE, EnvDTE.DTE>()))
#else
				if (null != (dte = ORMDesignerPackage.Singleton.GetService(typeof(EnvDTE.DTE)) as EnvDTE.DTE))
#endif
				{
					EnvDTE.ItemOperations operations = dte.ItemOperations;
					string[] catalogs = ORMDesignerPackage.SchemaCatalogs;

					for (int i = 0; i < catalogs.Length; ++i)
					{
						string catalog = catalogs[i];
						if (System.IO.File.Exists(catalog) && !dte.ItemOperations.IsFileOpen(catalog))
						{
							operations.OpenFile(catalog);
						}
					}
				}
			}
#endif // VSIX_Per_User
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuORMDesignerOptions(object sender, EventArgs e)
			{
				ORMDesignerPackage.Singleton.ShowOptionPage(typeof(OptionsPage));
			}
			#region ReadingEditor context menu handlers
			/// <summary>
			/// Status callback
			/// </summary>
			private void OnStatusAddReading(object sender, EventArgs e)
			{
				ReadingEditor.OnStatusCommand(sender, ReadingEditorCommands.AddReading);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuAddReading(object sender, EventArgs e)
			{
				ReadingEditor.Instance.OnMenuAddReading();
			}
			/// <summary>
			/// Status callback
			/// </summary>
			private void OnStatusAddReadingOrder(object sender, EventArgs e)
			{
				ReadingEditor.OnStatusCommand(sender, ReadingEditorCommands.AddReadingOrder);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuAddReadingOrder(object sender, EventArgs e)
			{
				ReadingEditor.Instance.OnMenuAddReadingOrder();
			}
			/// <summary>
			/// Status callback
			/// </summary>
			private void OnStatusDeleteReading(object sender, EventArgs e)
			{
				ReadingEditor.OnStatusCommand(sender, ReadingEditorCommands.DeleteReading);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuDeleteReading(object sender, EventArgs e)
			{
				ReadingEditor.Instance.OnMenuDeleteSelectedReading();
			}
			/// <summary>
			/// Status callback
			/// </summary>
			private void OnStatusPromoteReading(object sender, EventArgs e)
			{
				ReadingEditor.OnStatusCommand(sender, ReadingEditorCommands.PromoteReading);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuPromoteReading(object sender, EventArgs e)
			{
				ReadingEditor.Instance.OnMenuPromoteReading();
			}
			/// <summary>
			/// Status callback
			/// </summary>
			private void OnStatusDemoteReading(object sender, EventArgs e)
			{
				ReadingEditor.OnStatusCommand(sender, ReadingEditorCommands.DemoteReading);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuDemoteReading(object sender, EventArgs e)
			{
				ReadingEditor.Instance.OnMenuDemoteReading();
			}
			/// <summary>
			/// Status callback
			/// </summary>
			private void OnStatusReadingEditorPromoteReadingOrder(object sender, EventArgs e)
			{
				ReadingEditor.OnStatusCommand(sender, ReadingEditorCommands.PromoteReadingOrder);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuReadingEditorPromoteReadingOrder(object sender, EventArgs e)
			{
				ReadingEditor.Instance.OnMennuPromoteReadingOrder();
			}
			/// <summary>
			/// Status callback
			/// </summary>
			private void OnStatusReadingEditorDemoteReadingOrder(object sender, EventArgs e)
			{
				ReadingEditor.OnStatusCommand(sender, ReadingEditorCommands.DemoteReadingOrder);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuReadingEditorDemoteReadingOrder(object sender, EventArgs e)
			{
				ReadingEditor.Instance.OnMenuDemoteReadingOrder();
			}
			#endregion // ReadingEditor context menu handlers

			private sealed class DynamicErrorCommand : DynamicStatusMenuCommand
			{
				public DynamicErrorCommand(EventHandler statusHandler, EventHandler invokeHandler, CommandID id)
					: base(statusHandler, invokeHandler, id)
				{
					//Declare class variable with object containing error list
				}
				public sealed override bool DynamicItemMatch(int cmdId)
				{
					int baseCmdId = CommandID.ID;
					int testId = cmdId - baseCmdId;


					if (testId >= 0 && testId < ORMDesignerCommandIds.ErrorListLength)
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
			protected void OnStatusErrorList(object sender, EventArgs e)
			{
				ORMDesignerCommandManager.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.ErrorList);
				((OleMenuCommand)sender).MatchedCommandId = 0;
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuErrorList(object sender, EventArgs e)
			{
				IORMDesignerView designerView = CurrentORMView;
				if (designerView != null)
				{
					// Defer to the doc view
					designerView.CommandManager.OnMenuErrorList(((OleMenuCommand)sender).MatchedCommandId);
				}
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuDisableErrorList(object sender, EventArgs e)
			{
				IORMDesignerView designerView = CurrentORMView;
				if (designerView != null)
				{
					// Defer to the doc view
					designerView.CommandManager.OnMenuDisableErrorList(((OleMenuCommand)sender).MatchedCommandId);
				}
			}
			/// <summary>
			/// Status callback
			/// </summary>
			protected void OnStatusEnableErrorList(object sender, EventArgs e)
			{
				ORMDesignerCommandManager.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.DisabledErrorList);
				((OleMenuCommand)sender).MatchedCommandId = 0;
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuEnableErrorList(object sender, EventArgs e)
			{
				IORMDesignerView designerView = CurrentORMView;
				if (designerView != null)
				{
					// Defer to the doc view
					designerView.CommandManager.OnMenuEnableErrorList(((OleMenuCommand)sender).MatchedCommandId);
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


					if (testId >= 0 && testId < ORMDesignerCommandIds.DiagramListLength)
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
			protected void OnStatusSelectInDiagramSpy(object sender, EventArgs e)
			{
				ORMDesignerCommandManager.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.SelectInDiagramSpy);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuSelectInDiagramSpy(object sender, EventArgs e)
			{
				IORMDesignerView designerView = CurrentORMView;
				if (designerView != null)
				{
					designerView.CommandManager.OnMenuSelectInDiagramSpy();
				}
			}
			/// <summary>
			/// Status callback
			/// </summary>
			protected void OnStatusSelectInModelBrowser(object sender, EventArgs e)
			{
				ORMDesignerCommandManager.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.SelectInModelBrowser);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuSelectInModelBrowser(object sender, EventArgs e)
			{
				IORMDesignerView designerView = CurrentORMView;
				if (designerView != null)
				{
					designerView.CommandManager.OnMenuSelectInModelBrowser();
				}
			}
			/// <summary>
			/// Status callback
			/// </summary>
			protected void OnStatusSelectInDocumentWindow(object sender, EventArgs e)
			{
				ORMDesignerCommandManager.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.SelectInDocumentWindow);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuSelectInDocumentWindow(object sender, EventArgs e)
			{
				IORMDesignerView designerView = CurrentORMView;
				if (designerView != null)
				{
					designerView.CommandManager.OnMenuSelectInDocumentWindow();
				}
			}
			/// <summary>
			/// Status callback
			/// </summary>
			protected void OnStatusDiagramSpyAllDiagrams(object sender, EventArgs e)
			{
				ORMDesignerCommandManager.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.DiagramSpyAllDiagrams);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuDiagramSpyAllDiagrams(object sender, EventArgs e)
			{
				IORMDesignerView designerView = CurrentORMView;
				if (designerView != null)
				{
					designerView.CommandManager.OnMenuDiagramSpyAllDiagrams();
				}
			}
			/// <summary>
			/// Status callback
			/// </summary>
			protected void OnStatusSelectNextInCurrentDiagram(object sender, EventArgs e)
			{
				ORMDesignerCommandManager.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.SelectNextInCurrentDiagram);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuSelectNextInCurrentDiagram(object sender, EventArgs e)
			{
				IORMDesignerView designerView = CurrentORMView;
				if (designerView != null)
				{
					designerView.CommandManager.OnMenuSelectNextInCurrentDiagram();
				}
			}
			/// <summary>
			/// Status callback
			/// </summary>
			protected void OnStatusDiagramList(object sender, EventArgs e)
			{
				ORMDesignerCommandManager.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.DiagramList);
				((OleMenuCommand)sender).MatchedCommandId = 0;
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuDiagramList(object sender, EventArgs e)
			{
				IORMDesignerView designerView = CurrentORMView;
				if (designerView != null)
				{
					// Defer to the doc view
					designerView.CommandManager.OnMenuDiagramList(((OleMenuCommand)sender).MatchedCommandId);
				}
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuNewWindow(object sender, EventArgs e)
			{
				IORMDesignerView designerView = CurrentORMView;
				if (designerView != null)
				{
					designerView.CommandManager.OnMenuNewWindow();
				}
			}
			#region External Constraint editing menu options
			#region Grouping Commands
			/// <summary>
			/// Status callback
			/// </summary>
			protected void OnStatusIncludeInNewGroup(object sender, EventArgs e)
			{
				ORMDesignerCommandManager.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.IncludeInNewGroup);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuIncludeInNewGroup(object sender, EventArgs e)
			{
				IORMDesignerView designerView = CurrentORMView;
				if (designerView != null)
				{
					designerView.CommandManager.OnMenuIncludeInNewGroup();
				}
			}
			private sealed class DynamicGroupCommand : DynamicStatusMenuCommand
			{
				public DynamicGroupCommand(EventHandler statusHandler, EventHandler invokeHandler, CommandID id)
					: base(statusHandler, invokeHandler, id)
				{
					//Declare class variable with object containing diagram list
				}
				public sealed override bool DynamicItemMatch(int cmdId)
				{
					int baseCmdId = CommandID.ID;
					int testId = cmdId - baseCmdId;


					if (testId >= 0 && testId < ORMDesignerCommandIds.GroupListLength)
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
			protected void OnStatusIncludeInGroupList(object sender, EventArgs e)
			{
				ORMDesignerCommandManager.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.IncludeInGroupList);
				((OleMenuCommand)sender).MatchedCommandId = 0;
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuIncludeInGroupList(object sender, EventArgs e)
			{
				IORMDesignerView designerView = CurrentORMView;
				if (designerView != null)
				{
					// Defer to the doc view
					designerView.CommandManager.OnMenuIncludeInGroupList(((OleMenuCommand)sender).MatchedCommandId);
				}
			}
			/// <summary>
			/// Status callback
			/// </summary>
			protected void OnStatusDeleteFromGroupList(object sender, EventArgs e)
			{
				ORMDesignerCommandManager.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.DeleteFromGroupList);
				((OleMenuCommand)sender).MatchedCommandId = 0;
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuDeleteFromGroupList(object sender, EventArgs e)
			{
				IORMDesignerView designerView = CurrentORMView;
				if (designerView != null)
				{
					// Defer to the doc view
					designerView.CommandManager.OnMenuDeleteFromGroupList(((OleMenuCommand)sender).MatchedCommandId);
				}
			}
			/// <summary>
			/// Status callback
			/// </summary>
			protected void OnStatusSelectInGroupList(object sender, EventArgs e)
			{
				ORMDesignerCommandManager.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.SelectInGroupList);
				((OleMenuCommand)sender).MatchedCommandId = 0;
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuSelectInGroupList(object sender, EventArgs e)
			{
				IORMDesignerView designerView = CurrentORMView;
				if (designerView != null)
				{
					// Defer to the doc view
					designerView.CommandManager.OnMenuSelectInGroupList(((OleMenuCommand)sender).MatchedCommandId);
				}
			}
			#endregion // Grouping Commands
			#region Status queries
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuReferenceModesWindow(object sender, EventArgs e)
			{
				ORMDesignerPackage.ReferenceModeEditorWindow.Show();
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuInformalDescriptionWindow(object sender, EventArgs e)
			{
				ORMDescriptionToolWindow definitionWindow = ORMDesignerPackage.InformalDescriptionWindow;
				definitionWindow.Show();
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuNotesWindow(object sender, EventArgs e)
			{
				ORMNotesToolWindow notesWindow = ORMDesignerPackage.NotesWindow;
				notesWindow.Show();
			}
			/// <summary>
			/// Context window menu handler
			/// </summary>
			protected void OnMenuContextWindow(object sender, EventArgs e)
			{
				ORMContextWindow contextWindow = ORMDesignerPackage.ContextWindow;
				contextWindow.Show();
			}
			/// <summary>
			/// Diagram Spy menu handler
			/// </summary>
			protected void OnMenuDiagramSpyWindow(object sender, EventArgs e)
			{
				ORMDiagramSpyWindow diagramSpyWindow = ORMDesignerPackage.DiagramSpyWindow;
				diagramSpyWindow.Show();
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuFactEditorWindow(object sender, EventArgs e)
			{
				IVsWindowFrame editorWindow = ORMDesignerPackage.FactEditorWindow;
				editorWindow.Show();
			}

			/// <summary>
			/// Status callback
			/// </summary>
			private void OnStatusStandardWindow(object sender, EventArgs e)
			{
				ORMDesignerCommandManager.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.DisplayStandardWindows);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuVerbalizationWindow(object sender, EventArgs e)
			{
				ORMVerbalizationToolWindow verbWin = ORMDesignerPackage.VerbalizationWindow;
				verbWin.Show();
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuSamplePopulationEditor(object sender, EventArgs e)
			{
				ORMSamplePopulationToolWindow sampleWindow = ORMDesignerPackage.SamplePopulationEditorWindow;
				sampleWindow.Show();
			}
			/// <summary>
			/// Status callback
			/// </summary>
			protected void OnStatusShowNegativeVerbalization(object sender, EventArgs e)
			{
				MenuCommand command = sender as MenuCommand;
				command.Enabled = true;
				command.Visible = true;
				command.Supported = true;
				command.Checked = ORMDesignerPackage.VerbalizationWindowSettings.ShowNegativeVerbalizations;
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuShowNegativeVerbalization(object sender, EventArgs e)
			{
				ORMDesignerPackage.VerbalizationWindowSettings.ShowNegativeVerbalizations = true;
			}
			/// <summary>
			/// Status callback
			/// </summary>
			protected void OnStatusShowPositiveVerbalization(object sender, EventArgs e)
			{
				MenuCommand command = sender as MenuCommand;
				command.Enabled = true;
				command.Visible = true;
				command.Supported = true;
				command.Checked = !ORMDesignerPackage.VerbalizationWindowSettings.ShowNegativeVerbalizations;
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuShowPositiveVerbalization(object sender, EventArgs e)
			{
				ORMDesignerPackage.VerbalizationWindowSettings.ShowNegativeVerbalizations = false;
			}
			/// <summary>
			/// Status callback
			/// </summary>
			protected void OnStatusVerbalizationHyperlinkToDiagramSpy(object sender, EventArgs e)
			{
				MenuCommand command = sender as MenuCommand;
				command.Enabled = true;
				command.Visible = true;
				command.Supported = true;
				command.Checked = ORMDesignerPackage.VerbalizationWindowSettings.HyperlinkToDiagramSpy;
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuVerbalizationHyperlinkToDiagramSpy(object sender, EventArgs e)
			{
				ORMVerbalizationToolWindowSettings settings = ORMDesignerPackage.VerbalizationWindowSettings;
				settings.HyperlinkToDiagramSpy = !settings.HyperlinkToDiagramSpy;
			}
			/// <summary>
			/// Status callback
			/// </summary>
			protected void OnStatusEditRoleSequenceConstraint(object sender, EventArgs e)
			{
				ORMDesignerCommandManager.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.EditRoleSequenceConstraint);
			}
			/// <summary>
			/// Status callback
			/// </summary>
			protected void OnStatusActivateRoleSequence(object sender, EventArgs e)
			{
				ORMDesignerCommandManager.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.ActivateRoleSequence);
			}
			/// <summary>
			/// Status callback
			/// </summary>
			protected void OnStatusDeleteRowSequence(object sender, EventArgs e)
			{
				ORMDesignerCommandManager.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.DeleteRoleSequence);
			}
			/// <summary>
			/// Status callback
			/// </summary>
			protected void OnStatusMoveRoleSequenceUp(object sender, EventArgs e)
			{
				ORMDesignerCommandManager.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.MoveRoleSequenceUp);
			}
			/// <summary>
			/// Status callback
			/// </summary>
			protected void OnStatusMoveRoleSequenceDown(object sender, EventArgs e)
			{
				ORMDesignerCommandManager.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.MoveRoleSequenceDown);
			}
			#endregion // Status queries
			#region Menu actions
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuEditRoleSequenceConstraint(object sender, EventArgs e)
			{
				IORMDesignerView designerView = CurrentORMView;
				if (designerView != null)
				{
					designerView.CommandManager.OnMenuEditRoleSequenceConstraint();
				}
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuActivateRoleSequence(object sender, EventArgs e)
			{
				IORMDesignerView designerView = CurrentORMView;
				if (designerView != null)
				{
					designerView.CommandManager.OnMenuActivateRoleSequence();
				}
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuDeleteRowSequence(object sender, EventArgs e)
			{
				IORMDesignerView designerView = CurrentORMView;
				if (designerView != null)
				{
					// call delete on the doc view
					designerView.CommandManager.OnMenuDeleteRoleSequence();
				}
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuMoveRoleSequenceUp(object sender, EventArgs e)
			{
				IORMDesignerView designerView = CurrentORMView;
				if (designerView != null)
				{
					designerView.CommandManager.OnMenuMoveRoleSequenceUp();
				}
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuMoveRoleSequenceDown(object sender, EventArgs e)
			{
				IORMDesignerView designerView = CurrentORMView;
				if (designerView != null)
				{
					designerView.CommandManager.OnMenuMoveRoleSequenceDown();
				}
			}
			#endregion // Menu actions
			#endregion // External Constraint editing menu options
			/// <summary>
			/// Currently focused ORM designer view
			/// </summary>
			protected IORMDesignerView CurrentORMView
			{
				get
				{
					IORMDesignerView retVal = MonitorSelection.CurrentWindow as IORMDesignerView;
					if (null != retVal &&
						null != retVal.CurrentDesigner)
					{
						return retVal;
					}
					return CurrentDocView as IORMDesignerView;
				}
			}
			#region IEnumerable<MenuCommand> Implementation
			/// <summary>
			/// Enumerate the current menu commands
			/// </summary>
			public IEnumerator<MenuCommand> GetEnumerator()
			{
				return GetMenuCommands().GetEnumerator();
			}
			IEnumerator<MenuCommand> IEnumerable<MenuCommand>.GetEnumerator()
			{
				return GetEnumerator();
			}
			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
			#endregion // IEnumerable<MenuCommand> Implementation
		}

		/// <summary>
		/// CommandIDs for the Application Designer package.
		/// </summary>
		[Guid("7C51C000-1EAD-4B39-89B5-42BC9F49EA24")] // keep in sync with PkgCmd.vsct
		public static class ORMDesignerCommandIds
		{
			/// <summary>
			/// The global identifier for the command set used by the ORM designer.
			/// </summary>
			private static readonly Guid guidORMDesignerCommandSet = typeof(ORMDesignerCommandIds).GUID;
			#region CommandID objects for commands
			/// <summary>
			/// A command to view transaction contents in debug mode
			/// </summary>
			public static readonly CommandID DebugViewTransactionLogs = new CommandID(guidORMDesignerCommandSet, cmdIdDebugViewTransactionLogs);
			/// <summary>
			/// A command to view the current store contents in debug mode
			/// </summary>
			public static readonly CommandID DebugViewStore = new CommandID(guidORMDesignerCommandSet, cmdIdDebugViewStore);
			/// <summary>
			/// The ORM Model Browser item on the view menu
			/// </summary>
			public static readonly CommandID ViewModelBrowser = new CommandID(guidORMDesignerCommandSet, cmdIdViewModelBrowser);
			/// <summary>
			/// Copy selected elements as an image.
			/// </summary>
			public static readonly CommandID CopyImage = new CommandID(guidORMDesignerCommandSet, cmdIdCopyImage);
			/// <summary>
			/// The ORM Readings Window item on the fact type context menu
			/// </summary>
			public static readonly CommandID ViewReadingEditor = new CommandID(guidORMDesignerCommandSet, cmdIdViewReadingEditor);
			/// <summary>
			/// The sample population editor item on the context menu
			/// </summary>
			public static readonly CommandID ViewSamplePopulationEditor = new CommandID(guidORMDesignerCommandSet, cmdIdViewSamplePopulationEditor);
			/// <summary>
			/// The ORM Informal Description Window item on the context menu
			/// </summary>
			public static readonly CommandID ViewInformalDescriptionWindow = new CommandID(guidORMDesignerCommandSet, cmdIdViewInformalDescriptionWindow);
			/// <summary>
			/// The ORM Note Window item on the context menu
			/// </summary>
			public static readonly CommandID ViewNotesWindow = new CommandID(guidORMDesignerCommandSet, cmdIdViewNotesWindow);
			/// <summary>
			/// The ORM Context Window item on the context menu
			/// </summary>
			public static readonly CommandID ViewContextWindow = new CommandID(guidORMDesignerCommandSet, cmdIdViewContextWindow);
			/// <summary>
			/// The ORM Diagram Spy Window item on the context menu
			/// </summary>
			public static readonly CommandID ViewDiagramSpyWindow = new CommandID(guidORMDesignerCommandSet, cmdIdViewDiagramSpyWindow);
			/// <summary>
			/// Insert a role after the selected role
			/// </summary>
			public static readonly CommandID InsertRoleAfter = new CommandID(guidORMDesignerCommandSet, cmdIdInsertRoleAfter);
			/// <summary>
			/// Insert a role before the selected role
			/// </summary>
			public static readonly CommandID InsertRoleBefore = new CommandID(guidORMDesignerCommandSet, cmdIdInsertRoleBefore);
			/// <summary>
			/// The Custom Reference Mode Editor Explorer item on the view menu
			/// </summary>
			public static readonly CommandID ViewReferenceModeEditor = new CommandID(guidORMDesignerCommandSet, cmdIdViewReferenceModeEditor);
			/// <summary>
			/// The ORM Fact Editor Window item on the fact type context menu
			/// </summary>
			public static readonly CommandID ViewFactEditor = new CommandID(guidORMDesignerCommandSet, cmdIdViewFactEditor);
			/// <summary>
			/// The ORM Verbalization Browser Window item on the context menu
			/// </summary>
			public static readonly CommandID ViewVerbalizationBrowser = new CommandID(guidORMDesignerCommandSet, cmdIdViewVerbalizationBrowser);
			/// <summary>
			/// The ORM Verbalization Browser toolbar button for positive verbalization
			/// </summary>
			public static readonly CommandID ShowPositiveVerbalization = new CommandID(guidORMDesignerCommandSet, cmdIdShowPositiveVerbalization);
			/// <summary>
			/// The ORM Verbalization Browser toolbar button for negative verbalization
			/// </summary>
			public static readonly CommandID ShowNegativeVerbalization = new CommandID(guidORMDesignerCommandSet, cmdIdShowNegativeVerbalization);
			/// <summary>
			/// Verbalization option toggle, enable indicates jump to diagram spy instead
			/// of document window.
			/// </summary>
			public static readonly CommandID VerbalizationHyperlinkToDiagramSpy = new CommandID(guidORMDesignerCommandSet, cmdIdVerbalizationHyperlinkToDiagramSpy);
			/// <summary>
			/// The AutoLayout command
			/// </summary>
			public static readonly CommandID AutoLayout = new CommandID(guidORMDesignerCommandSet, cmdIdAutoLayout);
			/// <summary>
			/// Toggle simple mandatory command. Available on a single role selection.
			/// </summary>
			public static readonly CommandID ToggleSimpleMandatory = new CommandID(guidORMDesignerCommandSet, cmdIdToggleSimpleMandatory);
			/// <summary>
			/// Add internal uniqueness command. Available on a single or multi role
			/// role selection if all roles are in the same fact and an internal
			/// uniqueness constraint is not yet defined for the combination.
			/// </summary>
			public static readonly CommandID AddInternalUniqueness = new CommandID(guidORMDesignerCommandSet, cmdIdAddInternalUniqueness);
			/// <summary>
			/// The Extension Manager dialog command.  Launches the Extension Manager dialog.
			/// </summary>
			public static readonly CommandID ExtensionManager = new CommandID(guidORMDesignerCommandSet, cmdIdExtensionManager);
			/// <summary>
			/// The standard delete command is bound to shape deletion by
			/// default in the designer. This can be changed to element deletion
			/// with the options page. The alternate delete command is bound to
			/// Control-Delete and does the command not handled directly by delete.
			/// </summary>
			public static readonly CommandID DeleteAlternate = new CommandID(guidORMDesignerCommandSet, cmdIdDeleteAlternate);
			/// <summary>
			/// Move a role to the left in its order within the fact type.
			/// </summary>
			public static readonly CommandID MoveRoleLeft = new CommandID(guidORMDesignerCommandSet, cmdIdMoveRoleLeft);
			/// <summary>
			/// Move a role to the right in its order within the fact type.
			/// </summary>
			public static readonly CommandID MoveRoleRight = new CommandID(guidORMDesignerCommandSet, cmdIdMoveRoleRight);
			/// <summary>
			/// The commands for the ReadingEditor context menu -- Add a Reading to the Reading Order Selected
			/// </summary>
			public static readonly CommandID ReadingEditorAddReading = new CommandID(guidORMDesignerCommandSet, cmdIdReadingEditorAddReading);
			/// <summary>
			/// The commands for the ReadingEditor context menu -- Add a ReadingOrder 
			/// </summary>
			public static readonly CommandID ReadingEditorAddReadingOrder = new CommandID(guidORMDesignerCommandSet, cmdIdReadingEditorAddReadingOrder);
			/// <summary>
			/// The commands for the ReadingEditor context menu -- Delete a Reading rom the Reading Order Selected
			/// </summary>
			public static readonly CommandID ReadingEditorDeleteReading = new CommandID(guidORMDesignerCommandSet, cmdIdReadingEditorDeleteReading);
			/// <summary>
			/// The commands for the ReadingEditor context menu -- Move the selected Reading Up
			/// </summary>
			public static readonly CommandID ReadingEditorPromoteReading = new CommandID(guidORMDesignerCommandSet, cmdIdReadingEditorPromoteReading);
			/// <summary>
			/// The commands for the ReadingEditor context menu -- Move the selected Reading Down
			/// </summary>
			public static readonly CommandID ReadingEditorDemoteReading = new CommandID(guidORMDesignerCommandSet, cmdIdReadingEditorDemoteReading);
			/// <summary>
			/// The commands for the ReadingEditor context menu -- Move the selected RoleOrder Up
			/// </summary>
			public static readonly CommandID ReadingEditorPromoteReadingOrder = new CommandID(guidORMDesignerCommandSet, cmdIdReadingEditorPromoteReadingOrder);
			/// <summary>
			/// The commands for the ReadingEditor context menu -- Move the selected RoleOrder Down
			/// </summary>
			public static readonly CommandID ReadingEditorDemoteReadingOrder = new CommandID(guidORMDesignerCommandSet, cmdIdReadingEditorDemoteReadingOrder);
			/// <summary>
			/// Objectifies the fact type.
			/// </summary>
			public static readonly CommandID ObjectifyFactType = new CommandID(guidORMDesignerCommandSet, cmdIdObjectifyFactType);
			/// <summary>
			/// Set the FactTypeShape.DisplayOrientation property to Horizontal
			/// </summary>
			public static readonly CommandID DisplayOrientationHorizontal = new CommandID(guidORMDesignerCommandSet, cmdIdDisplayOrientationHorizontal);
			/// <summary>
			/// Set the FactTypeShape.DisplayOrientation property to RotatedLeft
			/// </summary>
			public static readonly CommandID DisplayOrientationRotatedLeft = new CommandID(guidORMDesignerCommandSet, cmdIdDisplayOrientationRotatedLeft);
			/// <summary>
			/// Set the FactTypeShape.DisplayOrientation property to RotatedRight
			/// </summary>
			public static readonly CommandID DisplayOrientationRotatedRight = new CommandID(guidORMDesignerCommandSet, cmdIdDisplayOrientationRotatedRight);
			/// <summary>
			/// Set the FactTypeShape.ConstraintDisplayPosition property to Top
			/// </summary>
			public static readonly CommandID DisplayConstraintsOnTop = new CommandID(guidORMDesignerCommandSet, cmdIdDisplayConstraintsOnTop);
			/// <summary>
			/// Set the FactTypeShape.ConstraintDisplayPosition property to Bottom
			/// </summary>
			public static readonly CommandID DisplayConstraintsOnBottom = new CommandID(guidORMDesignerCommandSet, cmdIdDisplayConstraintsOnBottom);
			/// <summary>
			/// Reverse the current role order
			/// </summary>
			public static readonly CommandID DisplayReverseRoleOrder = new CommandID(guidORMDesignerCommandSet, cmdIdDisplayReverseRoleOrder);
			/// <summary>
			/// Couple disjunctive mandatory and exclusion constraints
			/// </summary>
			public static readonly CommandID ExclusiveOrCoupler = new CommandID(guidORMDesignerCommandSet, cmdIdExclusiveOrCoupler);
			/// <summary>
			/// Decouple disjunctive mandatory and exclusion constraints
			/// </summary>
			public static readonly CommandID ExclusiveOrDecoupler = new CommandID(guidORMDesignerCommandSet, cmdIdExclusiveOrDecoupler);
			/// <summary>
			/// Unobjectifies the fact type.
			/// </summary>
			public static readonly CommandID UnobjectifyFactType = new CommandID(guidORMDesignerCommandSet, cmdIdUnobjectifyFactType);
			/// <summary>
			/// Place the inverse unary fact type in drag mode
			/// </summary>
			public static readonly CommandID DragInverseUnaryFactType = new CommandID(guidORMDesignerCommandSet, cmdIdDragInverseUnaryFactType);
			/// <summary>
			/// Place the link fact type in drag mode
			/// </summary>
			public static readonly CommandID DragLinkFactType = new CommandID(guidORMDesignerCommandSet, cmdIdDragLinkFactType);
			/// <summary>
			/// Place the constraint(s) for a negated unary in drag mode
			/// </summary>
			public static readonly CommandID DragNegatedUnaryConstraints = new CommandID(guidORMDesignerCommandSet, cmdIdDragNegatedUnaryConstraints);
			#endregion // CommandID objects for commands
			#region CommandID objects for menus
			/// <summary>
			/// The context menu for the diagram
			/// </summary>
			public static readonly CommandID ViewContextMenu = new CommandID(guidORMDesignerCommandSet, menuIdContextMenu);
			/// <summary>
			/// The context menu for the ReadingEditor
			/// </summary>
			public static readonly CommandID ReadingEditorContextMenu = new CommandID(guidORMDesignerCommandSet, menuIdReadingEditorContextMenu);
			/// <summary>
			/// The toolbar for the verbalization window
			/// </summary>
			public static readonly CommandID VerbalizationToolbar = new CommandID(guidORMDesignerCommandSet, menuIdVerbalizationToolbar);
			/// <summary>
			/// The toolbar for the fact editor
			/// </summary>
			public static readonly CommandID FactEditorToolbar = new CommandID(guidORMDesignerCommandSet, menuIdFactEditorToolbar);
			/// <summary>
			/// Available on any role belonging to the active RoleSequence in the active MCEC or SCEC.
			/// </summary>
			public static readonly CommandID ViewActivateRoleSequence = new CommandID(guidORMDesignerCommandSet, cmdIdActivateRoleSequence);
			/// <summary>
			/// Available on any role belonging to any RoleSequence in the active MCEC.
			/// </summary>
			public static readonly CommandID ViewDeleteRoleSequence = new CommandID(guidORMDesignerCommandSet, cmdIdDeleteRoleSequence);
			/// <summary>
			/// Available on any non-active external constraint or an internal uniqueness constraint.
			/// </summary>
			public static readonly CommandID ViewEditRoleSequenceConstraint = new CommandID(guidORMDesignerCommandSet, cmdIdEditRoleSequenceConstraint);
			/// <summary>
			/// Available on any role belonging to any RoleSequence in the active MCEC.
			/// </summary>
			public static readonly CommandID ViewMoveRoleSequenceUp = new CommandID(guidORMDesignerCommandSet, cmdIdMoveRoleSequenceUp);
			/// <summary>
			/// Available on any role belonging to any RoleSequence in the active MCEC.
			/// </summary>
			public static readonly CommandID ViewMoveRoleSequenceDown = new CommandID(guidORMDesignerCommandSet, cmdIdMoveRoleSequenceDown);
			/// <summary>
			/// Available to any type that is in a state of error
			/// </summary>
			public static readonly CommandID ErrorList = new CommandID(guidORMDesignerCommandSet, cmdIdErrorList);
			/// <summary>
			/// Indicates the number of command ids reserved for reporting errors
			/// </summary>
			public const int ErrorListLength = cmdIdErrorListEnd - cmdIdErrorList + 1;
			/// <summary>
			/// Available to any type that has enabled errors
			/// </summary>
			public static readonly CommandID DisableErrorList = new CommandID(guidORMDesignerCommandSet, cmdIdDisableErrorList);
			/// <summary>
			/// Available to any type that has disabled errors
			/// </summary>
			public static readonly CommandID EnableErrorList = new CommandID(guidORMDesignerCommandSet, cmdIdEnableErrorList);
			/// <summary>
			/// Activate the next shape in the same diagram corresponding the current backing element.
			/// </summary>
			public static readonly CommandID SelectNextInCurrentDiagram = new CommandID(guidORMDesignerCommandSet, cmdIdSelectNextInCurrentDiagram);
			/// <summary>
			/// Available to any element that is displayed on multiple diagrams
			/// </summary>
			public static readonly CommandID DiagramList = new CommandID(guidORMDesignerCommandSet, cmdIdDiagramList);
			/// <summary>
			/// Indicates the number of command ids reserved for display diagrams
			/// </summary>
			public const int DiagramListLength = cmdIdDiagramListEnd - cmdIdDiagramList + 1;
			/// <summary>
			/// Available to display a complementary diagram list targeted at the diagram spy window
			/// </summary>
			public static readonly CommandID DiagramSpyDiagramList = new CommandID(guidORMDesignerCommandSet, cmdIdDiagramSpyDiagramList);
			/// <summary>
			/// Available if report generators are registered
			/// </summary>
			public static readonly CommandID ReportGeneratorList = new CommandID(guidORMDesignerCommandSet, cmdIdReportGeneratorList);
			/// <summary>
			/// Indicates the number of command ids reserved for display diagrams
			/// </summary>
			public const int ReportGeneratorListLength = cmdIdReportGeneratorListEnd - cmdIdReportGeneratorList + 1;
			/// <summary>
			/// Activate the selected display element in the diagram spy window.
			/// </summary>
			public static readonly CommandID SelectInDiagramSpy = new CommandID(guidORMDesignerCommandSet, cmdIdSelectInDiagramSpy);
			/// <summary>
			/// Activate the selected display element in the document window.
			/// </summary>
			public static readonly CommandID SelectInDocumentWindow = new CommandID(guidORMDesignerCommandSet, cmdIdSelectInDocumentWindow);
			/// <summary>
			/// Show the 'All Diagrams' page in the diagram spy window (enabled in diagram spy)
			/// </summary>
			public static readonly CommandID DiagramSpyAllDiagrams = new CommandID(guidORMDesignerCommandSet, cmdIdDiagramSpyAllDiagrams);
			/// <summary>
			/// Activate the selected display element in the model browser.
			/// </summary>
			public static readonly CommandID SelectInModelBrowser = new CommandID(guidORMDesignerCommandSet, cmdIdSelectInModelBrowser);
			/// <summary>
			/// Available if free form context commands are supported for the current selection
			/// </summary>
			public static readonly CommandID FreeFormCommandList = new CommandID(guidORMDesignerCommandSet, cmdIdFreeFormCommandList);
			/// <summary>
			/// Indicates the number of command ids reserved for free form context commands
			/// </summary>
			public const int FreeFormCommandListLength = cmdIdFreeFormCommandListEnd - cmdIdFreeFormCommandList + 1;
			/// <summary>
			/// Include an explicitly excluded element from a group. Explicit
			/// exclusion occurs on a delete request of an automatic element.
			/// </summary>
			public static readonly CommandID IncludeInGroup = new CommandID(guidORMDesignerCommandSet, cmdIdIncludeInGroup);
			/// <summary>
			/// Add selected elements to a new group and select the group.
			/// </summary>
			public static readonly CommandID IncludeInNewGroup = new CommandID(guidORMDesignerCommandSet, cmdIdIncludeInNewGroup);
			/// <summary>
			/// The list of available groups to add to
			/// </summary>
			public static readonly CommandID IncludeInGroupList = new CommandID(guidORMDesignerCommandSet, cmdIdIncludeInGroupList);
			/// <summary>
			/// Indicates the number of command ids reserved for adding an element to a group
			/// </summary>
			public const int GroupListLength = cmdIdIncludeInGroupListEnd - cmdIdIncludeInGroupList + 1;
			/// <summary>
			/// The list of available groups containing the selected elements
			/// </summary>
			public static readonly CommandID DeleteFromGroupList = new CommandID(guidORMDesignerCommandSet, cmdIdDeleteFromGroupList);
			/// <summary>
			/// The list of available groups containing the selected elements
			/// </summary>
			public static readonly CommandID SelectInGroupList = new CommandID(guidORMDesignerCommandSet, cmdIdSelectInGroupList);
			/// <summary>
			/// Zoom 10 percent.
			/// </summary>
			public static readonly CommandID Zoom10 = new CommandID(guidORMDesignerCommandSet, cmdIdZoom10);
			/// <summary>
			/// Zoom 25 percent.
			/// </summary>
			public static readonly CommandID Zoom25 = new CommandID(guidORMDesignerCommandSet, cmdIdZoom25);
			/// <summary>
			/// Zoom 33 percent.
			/// </summary>
			public static readonly CommandID Zoom33 = new CommandID(guidORMDesignerCommandSet, cmdIdZoom33);
			/// <summary>
			/// Zoom 50 percent.
			/// </summary>
			public static readonly CommandID Zoom50 = new CommandID(guidORMDesignerCommandSet, cmdIdZoom50);
			/// <summary>
			/// Zoom 66 percent.
			/// </summary>
			public static readonly CommandID Zoom66 = new CommandID(guidORMDesignerCommandSet, cmdIdZoom66);
			/// <summary>
			/// Zoom 75 percent.
			/// </summary>
			public static readonly CommandID Zoom75 = new CommandID(guidORMDesignerCommandSet, cmdIdZoom75);
			/// <summary>
			/// Zoom 100 percent.
			/// </summary>
			public static readonly CommandID Zoom100 = new CommandID(guidORMDesignerCommandSet, cmdIdZoom100);
			/// <summary>
			/// Zoom 125 percent.
			/// </summary>
			public static readonly CommandID Zoom125 = new CommandID(guidORMDesignerCommandSet, cmdIdZoom125);
			/// <summary>
			/// Zoom 150 percent.
			/// </summary>
			public static readonly CommandID Zoom150 = new CommandID(guidORMDesignerCommandSet, cmdIdZoom150);
			/// <summary>
			/// Zoom 200 percent.
			/// </summary>
			public static readonly CommandID Zoom200 = new CommandID(guidORMDesignerCommandSet, cmdIdZoom200);
			/// <summary>
			/// Zoom 300 percent.
			/// </summary>
			public static readonly CommandID Zoom300 = new CommandID(guidORMDesignerCommandSet, cmdIdZoom300);
			/// <summary>
			/// Zoom 400 percent.
			/// </summary>
			public static readonly CommandID Zoom400 = new CommandID(guidORMDesignerCommandSet, cmdIdZoom400);
			/// <summary>
			/// Zoom In.
			/// </summary>
			public static readonly CommandID ZoomIn = new CommandID(guidORMDesignerCommandSet, cmdIdZoomIn);
			/// <summary>
			/// Zoom Out.
			/// </summary>
			public static readonly CommandID ZoomOut = new CommandID(guidORMDesignerCommandSet, cmdIdZoomOut);
#if VSIX_Per_User
			/// <summary>
			/// Load NORMA Schemas
			/// </summary>
			public static readonly CommandID LoadNORMASchemas = new CommandID(guidORMDesignerCommandSet, cmdIdLoadNORMASchemas);
#endif // VSIX_Per_User
			/// <summary>
			/// NORMA Options Page
			/// </summary>
			public static readonly CommandID ORMDesignerOptions = new CommandID(guidORMDesignerCommandSet, cmdIdORMDesignerOptions);
			#endregion //CommandID objects for menus
			#region cmdIds
			// IMPORTANT: keep these constants in sync with PkgCmd.vsct

			/// <summary>
			/// A command to view transaction contents in debug mode
			/// </summary>
			private const int cmdIdDebugViewTransactionLogs = 0x28FE;
			/// <summary>
			/// A command to view the current store contents in debug mode
			/// </summary>
			private const int cmdIdDebugViewStore = 0x28FF;

			/// <summary>
			/// The ORM Model Browser item on the view menu
			/// </summary>
			private const int cmdIdViewModelBrowser = 0x2900;
			/// <summary>
			/// The ORM Readings Window item on the fact type context menu
			/// </summary>
			private const int cmdIdViewReadingEditor = 0x2901;
			/// <summary>
			/// The Sample Population item on the context menu
			/// </summary>
			private const int cmdIdViewSamplePopulationEditor = 0x2920;
			/// <summary>
			/// View the reference mode editor
			/// </summary>
			private const int cmdIdViewReferenceModeEditor = 0x2902;
			/// <summary>
			/// Insert a role after the selected role
			/// </summary>
			private const int cmdIdInsertRoleAfter = 0x2903;
			/// <summary>
			/// Insert a role before the selected role
			/// </summary>
			private const int cmdIdInsertRoleBefore = 0x2904;
			/// <summary>
			/// The ORM Fact Editor Window item on the fact type context menu
			/// </summary>
			private const int cmdIdViewFactEditor = 0x2905;
			/// <summary>
			/// Available on any role belonging to the active RoleSequence in the active MCEC or SCEC.
			/// </summary>
			private const int cmdIdActivateRoleSequence = 0x2906;
			/// <summary>
			/// Available on any role belonging to any RoleSequence in the active MCEC.
			/// </summary>
			private const int cmdIdDeleteRoleSequence = 0x2907;
			/// <summary>
			/// Available on any non-active external constraint or an internal uniqueness constraint.
			/// </summary>
			private const int cmdIdEditRoleSequenceConstraint = 0x2908;
			/// <summary>
			/// Available on any role belonging to any RoleSequence in the active MCEC.
			/// </summary>
			private const int cmdIdMoveRoleSequenceUp = 0x2909;
			/// <summary>
			/// Available on any role belonging to any RoleSequence in the active MCEC.
			/// </summary>
			private const int cmdIdMoveRoleSequenceDown = 0x290A;
			/// <summary>
			/// Copy selected elements as an image.
			/// </summary>
			private const int cmdIdCopyImage = 0x290B;
			/// <summary>
			/// The ORM Verbalization Browser Window item on the View and context menu
			/// </summary>
			private const int cmdIdViewVerbalizationBrowser = 0x290C;
			/// <summary>
			/// The ORM Verbalization Browser toolbar button for positive verbalization
			/// </summary>
			private const int cmdIdShowPositiveVerbalization = 0x290D;
			/// <summary>
			/// The ORM Verbalization Browser toolbar button for negative verbalization
			/// </summary>
			private const int cmdIdShowNegativeVerbalization = 0x290E;
			/// <summary>
			/// AutoLayout command
			/// </summary>
			private const int cmdIdAutoLayout = 0x290F;
			/// <summary>
			/// Toggle simple mandatory command. Available on a single role selection.
			/// </summary>
			private const int cmdIdToggleSimpleMandatory = 0x2910;
			/// <summary>
			/// Add internal uniqueness command. Available on a single or multi role
			/// role selection if all roles are in the same fact and an internal
			/// uniqueness constraint is not yet defined for the combination.
			/// </summary>
			private const int cmdIdAddInternalUniqueness = 0x2911;
			/// <summary>
			/// Opens up the Extension Manager dialog
			/// </summary>
			private const int cmdIdExtensionManager = 0x2912;
			/// <summary>
			/// The ORM Note Window item on the context menu
			/// </summary>
			private const int cmdIdViewNotesWindow = 0x2913;
			/// <summary>
			/// The standard delete command is bound to shape deletion by
			/// default in the designer. This can be changed to element deletion
			/// with the options page. The alternate delete command is bound to
			/// Control-Delete and does the command not handled directly by delete.
			/// </summary>
			private const int cmdIdDeleteAlternate = 0x2914;
			/// <summary>
			/// The context menu for the local errors
			/// </summary>
			private const int cmdIdErrorList = 0x2a00;
			/// <summary>
			/// The last allowed id for an error list
			/// </summary>
			private const int cmdIdErrorListEnd = 0x2afe;
			/// <summary>
			/// The context menu item for related diagrams
			/// </summary>
			private const int cmdIdDiagramList = 0x2b00;
			/// <summary>
			/// The last allowed id for a diagram list
			/// </summary>
			private const int cmdIdDiagramListEnd = 0x2bfe;
			/// <summary>
			/// The context menu item for available report generators
			/// </summary>
			private const int cmdIdReportGeneratorList = 0x2c00;
			/// <summary>
			/// The last allowed id for a report generator list
			/// </summary>
			private const int cmdIdReportGeneratorListEnd = 0x2cfe;
			/// <summary>
			/// The context menu for disabling local errors
			/// </summary>
			private const int cmdIdDisableErrorList = 0x3100;
			// private const int cmdIdDisableErrorListEnd = 0x31fe; // Uses the same offset as cmdIdErrorList to cmdIdErrorListEnd
			/// <summary>
			/// The context menu for enabling local errors
			/// </summary>
			private const int cmdIdEnableErrorList = 0x3200;
			// private const int cmdIdEnabledErrorListEnd = 0x32fe; // Uses the same offset as cmdIdErrorList to cmdIdErrorListEnd
			/// <summary>
			/// The context menu for the diagram
			/// </summary>
			private const int menuIdContextMenu = 0x0100;
			/// <summary>
			/// The toolbar for the verbalization window
			/// </summary>
			private const int menuIdVerbalizationToolbar = 0x0101;
			/// <summary>
			/// The context menu for the New Reading Editor
			/// </summary>
			private const int menuIdReadingEditorContextMenu = 0x0103;
			/// <summary>
			/// The toolbar for the fact editor
			/// </summary>
			private const int menuIdFactEditorToolbar = 0x0108;
			/// <summary>
			/// Moves the role to the left in its order with the fact type.
			/// </summary>
			private const int cmdIdMoveRoleLeft = 0x2915;
			/// <summary>
			/// Moves the role to the right in its order with the fact type.
			/// </summary>
			private const int cmdIdMoveRoleRight = 0x2916;
			/// <summary>
			/// The ORM Context Window
			/// </summary>
			private const int cmdIdViewContextWindow = 0x2917;
			/// <summary>
			/// Initiates the drop down to add a new reading in the Reading Editor
			/// </summary>
			private const int cmdIdReadingEditorAddReading = 0x2918;
			/// <summary>
			/// Initiates the Addition of a new Reading Order
			/// </summary>
			private const int cmdIdReadingEditorAddReadingOrder = 0x2919;
			/// <summary>
			/// Initiates the deletion of the selected Reading in the Reading Editor
			/// </summary>
			private const int cmdIdReadingEditorDeleteReading = 0x291A;
			/// <summary>
			/// Promotes the selected Reading in the Reading Editor
			/// </summary>
			private const int cmdIdReadingEditorPromoteReading = 0x291B;
			/// <summary>
			/// Demotes the selected Reading in the Reading Editor
			/// </summary>
			private const int cmdIdReadingEditorDemoteReading = 0x291C;
			/// <summary>
			/// Promotes the RoleOrder in the Reading Editor
			/// </summary>
			private const int cmdIdReadingEditorPromoteReadingOrder = 0x291D;
			/// <summary>
			/// Demotes the RoleOrder in the Reading Editor
			/// </summary>
			private const int cmdIdReadingEditorDemoteReadingOrder = 0x291E;
			/// <summary>
			/// Objectifies the fact type.
			/// </summary>
			private const int cmdIdObjectifyFactType = 0x291F;
			/// <summary>
			/// Set the FactTypeShape.DisplayOrientation property to Horizontal
			/// </summary>
			private const int cmdIdDisplayOrientationHorizontal = 0x2921;
			/// <summary>
			/// Set the FactTypeShape.DisplayOrientation property to RotatedLeft
			/// </summary>
			private const int cmdIdDisplayOrientationRotatedLeft = 0x2922;
			/// <summary>
			/// Set the FactTypeShape.DisplayOrientation property to RotatedRight
			/// </summary>
			private const int cmdIdDisplayOrientationRotatedRight = 0x2923;
			/// <summary>
			/// Set the FactTypeShape.ConstraintDisplayPosition property to Top
			/// </summary>
			private const int cmdIdDisplayConstraintsOnTop = 0x2924;
			/// <summary>
			/// Set the FactTypeShape.ConstraintDisplayPosition property to Bottom
			/// </summary>
			private const int cmdIdDisplayConstraintsOnBottom = 0x2925;
			/// <summary>
			/// Reverse the current role order
			/// </summary>
			private const int cmdIdDisplayReverseRoleOrder = 0x2926;
			/// <summary>
			/// Couple disjunctive mandatory and exclusion constraints
			/// </summary>
			private const int cmdIdExclusiveOrCoupler = 0x2927;
			/// <summary>
			/// Decouple disjunctive mandatory and exclusion constraints
			/// </summary>
			private const int cmdIdExclusiveOrDecoupler = 0x2928;
			/// <summary>
			/// Unobjectifies the fact type.
			/// </summary>
			private const int cmdIdUnobjectifyFactType = 0x2929;
			/// <summary>
			/// The ORM Informal Description Window item on the context menu
			/// </summary>
			private const int cmdIdViewInformalDescriptionWindow = 0x292a;
			// Commit a line in the fact editor, not used by the designer
			//private const int cmdIdFactEditorCommitLine = 0x292b;
			/// <summary>
			/// The ORM Diagram Spy Window item on the context menu
			/// </summary>
			private const int cmdIdViewDiagramSpyWindow = 0x292c;
			/// <summary>
			/// Verbalization option toggle, enable indicates jump to diagram spy instead
			/// of document window.
			/// </summary>
			private const int cmdIdVerbalizationHyperlinkToDiagramSpy = 0x292d;
			/// <summary>
			/// Activate the selected display element in the diagram spy window.
			/// </summary>
			private const int cmdIdSelectInDiagramSpy = 0x292e;
			/// <summary>
			/// Activate the selected display element in the document window.
			/// </summary>
			private const int cmdIdSelectInDocumentWindow = 0x292f;
			/// <summary>
			/// Include an explicitly excluded element from a group. Explicit
			/// exclusion occurs on a delete request of an automatic element.
			/// </summary>
			private const int cmdIdIncludeInGroup = 0x2930;
			/// <summary>
			/// Add selected elements to a new group and select the group.
			/// </summary>
			private const int cmdIdIncludeInNewGroup = 0x2931;
			/// <summary>
			/// Activate the selected display element in the model browser.
			/// </summary>
			private const int cmdIdSelectInModelBrowser = 0x2932;
			/// <summary>
			/// Activate the next shape in the same diagram corresponding the current backing element.
			/// </summary>
			private const int cmdIdSelectNextInCurrentDiagram = 0x2933;
			/// <summary>
			/// Zoom 10 percent.
			/// </summary>
			private const int cmdIdZoom10 = 0x2934;
			/// <summary>
			/// Zoom 25 percent.
			/// </summary>
			private const int cmdIdZoom25 = 0x2935;
			/// <summary>
			/// Zoom 33 percent.
			/// </summary>
			private const int cmdIdZoom33 = 0x2936;
			/// <summary>
			/// Zoom 50 percent.
			/// </summary>
			private const int cmdIdZoom50 = 0x2937;
			/// <summary>
			/// Zoom 66 percent.
			/// </summary>
			private const int cmdIdZoom66 = 0x2938;
			/// <summary>
			/// Zoom 75 percent.
			/// </summary>
			private const int cmdIdZoom75 = 0x2939;
			/// <summary>
			/// Zoom 100 percent.
			/// </summary>
			private const int cmdIdZoom100 = 0x293a;
			/// <summary>
			/// Zoom 125 percent.
			/// </summary>
			private const int cmdIdZoom125 = 0x293b;
			/// <summary>
			/// Zoom 150 percent.
			/// </summary>
			private const int cmdIdZoom150 = 0x293c;
			/// <summary>
			/// Zoom 200 percent.
			/// </summary>
			private const int cmdIdZoom200 = 0x293d;
			/// <summary>
			/// Zoom 300 percent.
			/// </summary>
			private const int cmdIdZoom300 = 0x293e;
			/// <summary>
			/// Zoom 400 percent.
			/// </summary>
			private const int cmdIdZoom400 = 0x293f;
			/// <summary>
			/// Zoom In.
			/// </summary>
			private const int cmdIdZoomIn = 0x2940;
			/// <summary>
			/// Zoom Out.
			/// </summary>
			private const int cmdIdZoomOut = 0x2941;
#if VSIX_Per_User
			/// <summary>
			/// Load NORMA Schemas
			/// </summary>
			private const int cmdIdLoadNORMASchemas = 0x2942;
#endif // VSIX_Per_User
			/// <summary>
			/// NORMA Options page
			/// </summary>
			private const int cmdIdORMDesignerOptions = 0x2943;
			/// <summary>
			/// Place the inverse unary fact type in drag mode
			/// </summary>
			private const int cmdIdDragInverseUnaryFactType = 0x2944;
			/// <summary>
			/// Place the link fact type in drag mode
			/// </summary>
			private const int cmdIdDragLinkFactType = 0x2945;
			/// <summary>
			/// Show the 'All Diagrams' page in the diagram spy window (enabled in diagram spy)
			/// </summary>
			private const int cmdIdDiagramSpyAllDiagrams = 0x2947;
			/// <summary>
			/// Place the constraint(s) for a negated unary in drag mode
			/// </summary>
			private const int cmdIdDragNegatedUnaryConstraints = 0x2948;
			/// <summary>
			/// The context menu item for related diagrams, targeted to the diagram spy
			/// </summary>
			private const int cmdIdDiagramSpyDiagramList = 0x2d00;
			/// <summary>
			/// The list of free form commands placed at the top of context menu
			/// </summary>
			private const int cmdIdFreeFormCommandList = 0x2e00;
			/// <summary>
			/// The last allowed id for a free form list.
			/// This is a relatively short range because it is heavily requested
			/// on a top-level menu and is meant to correspond to limit extension-provided
			/// menu items, not items based on user data.
			/// </summary>
			private const int cmdIdFreeFormCommandListEnd = 0x2e20;
			/// <summary>
			/// The list of available groups to add to
			/// </summary>
			private const int cmdIdIncludeInGroupList = 0x2f00;
			/// <summary>
			/// The last allowed id for an add to group list
			/// </summary>
			private const int cmdIdIncludeInGroupListEnd = 0x2ffe;
			/// <summary>
			/// The list of available groups to remove from
			/// </summary>
			private const int cmdIdDeleteFromGroupList = 0x3000;
			// private const int cmdIdDeleteFromGroupListEnd = 0x30fe; // Uses same offset as cmdIdIncludeInGroupList to cmdIdIncludeInGroupListEnd
			/// <summary>
			/// The list of available groups to remove from
			/// </summary>
			private const int cmdIdSelectInGroupList = 0x3300;
			// private const int cmdIdSelectInGroupListEnd = 0x33fe; // Uses same offset as cmdIdIncludeInGroupList to cmdIdIncludeInGroupListEnd
			#endregion
		}
	}
}
