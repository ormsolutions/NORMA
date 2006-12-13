#region Common Public License Copyright Notice
/**************************************************************************\
* Neumont Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
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
//NOTICE: if you toggle HIDENEWMODELBROWSER on/off make sure to change it on in ORMPackage.cs as well
//#define HIDENEWMODELBROWSER 
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.ORM;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.ShapeModel;
using Microsoft.VisualStudio;
using System.ComponentModel;

namespace Neumont.Tools.ORM.Shell
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
		protected class ORMDesignerCommandSet : CommandSet, IDisposable
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
					#if DEBUG
						new MenuCommand(
						new EventHandler(OnMenuDebugViewStore),
						ORMDesignerCommandIds.DebugViewStore),
					#endif // DEBUG
						new DynamicStatusMenuCommand(
						new EventHandler(OnStatusReferenceModesWindow),
						new EventHandler(OnMenuReferenceModesWindow),
						ORMDesignerCommandIds.ViewReferenceModeEditor)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusStandardWindow),
						new EventHandler(OnMenuNotesWindow),
						ORMDesignerCommandIds.ViewNotesWindow)
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusStandardWindow),
						new EventHandler(OnMenuContextWindow),
						ORMDesignerCommandIds.NewContextWindow)
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
						new EventHandler(OnStatusFactEditorWindow),
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
						new EventHandler(OnStatusEditExternalConstraint),
						new EventHandler(OnMenuEditExternalConstraint),
						ORMDesignerCommandIds.ViewEditExternalConstraint)

						// Verbalization Commands
						,new DynamicStatusMenuCommand(
						new EventHandler(OnStatusShowPositiveVerbalization),
						new EventHandler(OnMenuShowPositiveVerbalization),
						ORMDesignerCommandIds.ShowPositiveVerbalization)
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
					#if !HIDENEWMODELBROWSER
						new EventHandler(OnStatusStandardWindow),
					#else
						delegate(object sender, EventArgs e){(sender as MenuCommand).Visible = false;},
					#endif
						new EventHandler(OnMenuViewNewORMModelBrowser),
						ORMDesignerCommandIds.ViewNewModelBrowser)
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
			/// Show the ORM Model Browser
			/// </summary>
			protected void OnMenuViewORMModelBrowser(object sender, EventArgs e)
			{
				ORMDesignerPackage.BrowserWindow.Show();
			}

			/// <summary>
			/// Show the New ORM Model Browser
			/// </summary>
			protected void OnMenuViewNewORMModelBrowser(object sender, EventArgs e)
			{
				ORMDesignerPackage.NewORMModelBrowserWindow.Show();
			}

			/// <summary>
			/// Status callback
			/// </summary>
			protected void OnStatusCopyImage(object sender, EventArgs e)
			{
				ORMDesignerDocView.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.CopyImage);
			}
			/// <summary>
			/// Copies as image
			/// </summary>
			protected void OnMenuCopyImage(object sender, EventArgs e)
			{
				ORMDesignerDocView docView = CurrentORMView;
				if (docView != null)
				{
					// call CopyImage on the doc view
					docView.OnMenuCopyImage();
				}
			}

#if DEBUG
			/// <summary>
			/// Show a debug window displaying the contents of the current store
			/// </summary>
			protected void OnMenuDebugViewStore(object sender, EventArgs e)
			{
				Microsoft.VisualStudio.Modeling.Diagnostics.StoreViewer.Show(((ModelingDocData)CurrentORMView.DocData).Store);
			}
#endif // DEBUG

			/// <summary>
			/// Status callback
			/// </summary>
			private void OnStatusDeleteAlternate(object sender, EventArgs e)
			{
				ORMDesignerDocView.OnStatusCommand(
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
				ORMDesignerDocView docView = CurrentORMView;
				if (docView != null)
				{
					// call the appropriate delete on the doc view
					switch (OptionsPage.CurrentPrimaryDeleteBehavior)
					{
						case PrimaryDeleteBehavior.DeleteShape:
							docView.OnMenuDeleteElement((sender as OleMenuCommand).Text);
							break;
						case PrimaryDeleteBehavior.DeleteElement:
							docView.OnMenuDeleteShape((sender as OleMenuCommand).Text);
							break;
					}
				}
			}
			/// <summary>
			/// Status callback
			/// </summary>
			private void OnStatusDelete(object sender, EventArgs e)
			{
				ORMDesignerDocView.OnStatusCommand(
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
				ORMDesignerDocView docView = CurrentORMView;
				if (docView != null)
				{
					// call the appropriate delete on the doc view
					switch (OptionsPage.CurrentPrimaryDeleteBehavior)
					{
						case PrimaryDeleteBehavior.DeleteElement:
							docView.OnMenuDeleteElement((sender as OleMenuCommand).Text);
							break;
						case PrimaryDeleteBehavior.DeleteShape:
							docView.OnMenuDeleteShape((sender as OleMenuCommand).Text);
							break;
					}
				}
			}
			private void OnStatusReferenceModesWindow(object sender, EventArgs e)
			{
				ORMDesignerDocView.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.DisplayCustomReferenceModeWindow);
			}
			/// <summary>
			/// Status callback
			/// </summary>
			private void OnStatusReadingsWindow(object sender, EventArgs e)
			{
				ORMDesignerDocView.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.DisplayReadingsWindow);
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
				ORMDesignerDocView.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.SelectAll);
			}
			/// <summary>
			/// Menu handler for the SelectAll command
			/// </summary>
			protected void OnMenuSelectAll(object sender, EventArgs e)
			{
				ORMDesignerDocView docView = CurrentORMView;
				if (docView != null)
				{
					// call delete on the doc view
					docView.OnMenuSelectAll();
				}
			}
			private void OnStatusAutoLayout(object sender, EventArgs e)
			{
				ORMDesignerDocView.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.AutoLayout);
			}
			/// <summary>
			/// Menu handler for the SelectAll command
			/// </summary>
			protected void OnMenuAutoLayout(object sender, EventArgs e)
			{
				ORMDesignerDocView docView = CurrentORMView;
				if (docView != null)
				{
					// call auto layout on the doc view
					docView.OnMenuAutoLayout();
				}
			}
			/// <summary>
			/// Status callback
			/// </summary>
			private void OnStatusInsertRole(object sender, EventArgs e)
			{
				ORMDesignerDocView.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.InsertRole);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuInsertRoleAfter(object sender, EventArgs e)
			{
				ORMDesignerDocView docView = CurrentORMView;
				if (docView != null)
				{
					// Defer to the doc view
					docView.OnMenuInsertRole(true);
				}
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuInsertRoleBefore(object sender, EventArgs e)
			{
				ORMDesignerDocView docView = CurrentORMView;
				if (docView != null)
				{
					// Defer to the doc view
					docView.OnMenuInsertRole(false);
				}
			}
			/// <summary>
			/// Status callback
			/// </summary>
			private void OnStatusToggleSimpleMandatory(object sender, EventArgs e)
			{
				ORMDesignerDocView.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.ToggleSimpleMandatory);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuToggleSimpleMandatory(object sender, EventArgs e)
			{
				ORMDesignerDocView docView = CurrentORMView;
				if (docView != null)
				{
					// Defer to the doc view
					docView.OnMenuToggleSimpleMandatory();
				}
			}
			/// <summary>
			/// Status callback
			/// </summary>
			private void OnStatusAddInternalUniqueness(object sender, EventArgs e)
			{
				ORMDesignerDocView.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.AddInternalUniqueness);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuAddInternalUniqueness(object sender, EventArgs e)
			{
				ORMDesignerDocView docView = CurrentORMView;
				if (docView != null)
				{
					// Defer to the doc view
					docView.OnMenuAddInternalUniqueness();
				}
			}
			/// <summary>
			/// Status callback
			/// </summary>
			private void OnStatusExtensionManager(object sender, EventArgs e)
			{
				ORMDesignerDocView.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.ExtensionManager);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuExtensionManager(object sender, EventArgs e)
			{
				ORMDesignerDocView docView = CurrentORMView;
				if (docView != null)
				{
					// Defer to the doc view
					docView.OnMenuExtensionManager();
				}
			}
			private void OnStatusMoveRoleLeft(object sender, EventArgs e)
			{
				ORMDesignerDocView.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.MoveRoleLeft);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuMoveRoleLeft(object sender, EventArgs e)
			{
				ORMDesignerDocView docView = CurrentORMView;
				if (docView != null)
				{
					// Defer to the doc view
					docView.OnMenuMoveRoleLeft();
				}
			}
			private void OnStatusMoveRoleRight(object sender, EventArgs e)
			{
				ORMDesignerDocView.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.MoveRoleRight);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuMoveRoleRight(object sender, EventArgs e)
			{
				ORMDesignerDocView docView = CurrentORMView;
				if (docView != null)
				{
					// Defer to the doc view
					docView.OnMenuMoveRoleRight();
				}
			}
			private void OnStatusObjectifyFactType(object sender, EventArgs e)
			{
				ORMDesignerDocView.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.ObjectifyFactType);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuObjectifyFactType(object sender, EventArgs e)
			{
				ORMDesignerDocView docView = CurrentORMView;
				if (docView != null)
				{
					// Defer to the doc view
					docView.OnMenuObjectifyFactType();
				}
			}
			private void OnStatusDisplayOrientationHorizontal(object sender, EventArgs e)
			{
				ORMDesignerDocView.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.DisplayOrientationHorizontal);
			}
			private void OnStatusDisplayOrientationRotatedLeft(object sender, EventArgs e)
			{
				ORMDesignerDocView.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.DisplayOrientationRotatedLeft);
			}
			private void OnStatusDisplayOrientationRotatedRight(object sender, EventArgs e)
			{
				ORMDesignerDocView.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.DisplayOrientationRotatedRight);
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
				ORMDesignerDocView docView = CurrentORMView;
				if (docView != null)
				{
					// Defer to the doc view
					docView.OnMenuDisplayOrientation(orientation);
				}
			}
			private void OnStatusDisplayConstraintsOnTop(object sender, EventArgs e)
			{
				ORMDesignerDocView.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.DisplayConstraintsOnTop);
			}
			private void OnStatusDisplayConstraintsOnBottom(object sender, EventArgs e)
			{
				ORMDesignerDocView.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.DisplayConstraintsOnBottom);
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
				ORMDesignerDocView docView = CurrentORMView;
				if (docView != null)
				{
					// Defer to the doc view
					docView.OnMenuDisplayConstraintPosition(position);
				}
			}
			private void OnStatusDisplayReverseRoleOrder(object sender, EventArgs e)
			{
				ORMDesignerDocView.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.DisplayReverseRoleOrder);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			private void OnMenuDisplayReverseRoleOrder(object sender, EventArgs e)
			{
				ORMDesignerDocView docView = CurrentORMView;
				if (docView != null)
				{
					// Defer to the doc view
					docView.OnMenuDisplayReverseRoleOrder();
				}
			}
			private void OnStatusExclusiveOrCoupler(object sender, EventArgs e)
			{
				ORMDesignerDocView.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.ExclusiveOrCoupler);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			private void OnMenuExclusiveOrCoupler(object sender, EventArgs e)
			{
				ORMDesignerDocView docView = CurrentORMView;
				if (docView != null)
				{
					// Defer to the doc view
					docView.OnMenuExclusiveOrCoupler();
				}
			}
			private void OnStatusExclusiveOrDecoupler(object sender, EventArgs e)
			{
				ORMDesignerDocView.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.ExclusiveOrDecoupler);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			private void OnMenuExclusiveOrDecoupler(object sender, EventArgs e)
			{
				ORMDesignerDocView docView = CurrentORMView;
				if (docView != null)
				{
					// Defer to the doc view
					docView.OnMenuExclusiveOrDecoupler();
				}
			}
			/// <summary>
			/// Status callback
			/// </summary>
			private void OnStatusAlignShapes(object sender, EventArgs e)
			{
				ORMDesignerDocView.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.AlignShapes);
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuAlignShapes(object sender, EventArgs e)
			{
				ORMDesignerDocView docView = CurrentORMView;
				if (docView != null)
				{
					// Defer to the doc view
					docView.OnMenuAlignShapes((sender as MenuCommand).CommandID.ID);
				}
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
				public DynamicErrorCommand(EventHandler statusHandler, EventHandler invokeHandler, CommandID id) : base(statusHandler, invokeHandler, id)
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
			//private System.Collections.Generic.IEnumerator<string> strings;
			/// <summary>
			/// Status callback
			/// </summary>
			protected void OnStatusErrorList(object sender, EventArgs e)
			{
				ORMDesignerDocView.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.ErrorList);
				((OleMenuCommand)sender).MatchedCommandId = 0;
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuErrorList(object sender, EventArgs e)
			{
				ORMDesignerDocView docView = CurrentORMView;
				if (docView != null)
				{
					// Defer to the doc view
					docView.OnMenuErrorList(((OleMenuCommand)sender).MatchedCommandId);
				}
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuNewWindow(object sender, EventArgs e)
			{
				ORMDesignerDocView docView = CurrentORMView;
				if (docView != null)
				{
					docView.OnMenuNewWindow();
				}
			}
			#region External Constraint editing menu options
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
			protected void OnMenuNotesWindow(object sender, EventArgs e)
			{
				ORMNotesToolWindow notesWindow = ORMDesignerPackage.NotesWindow;
				notesWindow.Show();
			}

			/// <summary>
			/// Context window menu handler
			/// </summary>
			/// <param name="sender">The sender.</param>
			/// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
			protected void OnMenuContextWindow(object sender, EventArgs e)
			{
				ORMContextWindow contextWindow = ORMDesignerPackage.ContextWindow;
				contextWindow.Show();
			}
			/// <summary>
			/// Status callback
			/// </summary>
			private void OnStatusFactEditorWindow(object sender, EventArgs e)
			{
				ORMDesignerDocView.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.DisplayFactEditorWindow);
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
				ORMDesignerDocView.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.DisplayStandardWindows);
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
			protected void OnStatusEditExternalConstraint(object sender, EventArgs e)
			{
				ORMDesignerDocView.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.EditExternalConstraint);
			}
			/// <summary>
			/// Status callback
			/// </summary>
			protected void OnStatusActivateRoleSequence(object sender, EventArgs e)
			{
				ORMDesignerDocView.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.ActivateRoleSequence);
			}
			/// <summary>
			/// Status callback
			/// </summary>
			protected void OnStatusDeleteRowSequence(object sender, EventArgs e)
			{
				ORMDesignerDocView.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.DeleteRoleSequence);
			}
			/// <summary>
			/// Status callback
			/// </summary>
			protected void OnStatusMoveRoleSequenceUp(object sender, EventArgs e)
			{
				ORMDesignerDocView.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.MoveRoleSequenceUp);
			}
			/// <summary>
			/// Status callback
			/// </summary>
			protected void OnStatusMoveRoleSequenceDown(object sender, EventArgs e)
			{
				ORMDesignerDocView.OnStatusCommand(sender, CurrentORMView, ORMDesignerCommands.MoveRoleSequenceDown);
			}
			#endregion // Status queries
			#region Menu actions
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuEditExternalConstraint(object sender, EventArgs e)
			{
				ORMDesignerDocView docView = CurrentORMView;
				if (docView != null)
				{
					docView.OnMenuEditExternalConstraint();
				}
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuActivateRoleSequence(object sender, EventArgs e)
			{
				ORMDesignerDocView docView = CurrentORMView;
				if (docView != null)
				{
					docView.OnMenuActivateRoleSequence();
				}
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuDeleteRowSequence(object sender, EventArgs e)
			{
				ORMDesignerDocView docView = CurrentORMView;
				if (docView != null)
				{
					// call delete on the doc view
					docView.OnMenuDeleteRoleSequence();
				}
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuMoveRoleSequenceUp(object sender, EventArgs e)
			{
				ORMDesignerDocView docView = CurrentORMView;
				if (docView != null)
				{
					docView.OnMenuMoveRoleSequenceUp();
				}
			}
			/// <summary>
			/// Menu handler
			/// </summary>
			protected void OnMenuMoveRoleSequenceDown(object sender, EventArgs e)
			{
				ORMDesignerDocView docView = CurrentORMView;
				if (docView != null)
				{
					docView.OnMenuMoveRoleSequenceDown();
				}
			}
			#endregion // Menu actions
			#endregion // External Constraint editing menu options
			
			/// <summary>
			/// Currently focused document
			/// </summary>
			protected ORMDesignerDocData CurrentORMData
			{
				get
				{
					return base.CurrentDocData as ORMDesignerDocData;
				}
			}
			/// <summary>
			/// Currently focused ORM document view
			/// </summary>
			protected ORMDesignerDocView CurrentORMView
			{
				get
				{
					return base.CurrentDocView as ORMDesignerDocView;
				}
			}
		}

		/// <summary>
		/// CommandIDs for the Application Designer package.
		/// </summary>
		public class ORMDesignerCommandIds
		{
			/// <summary>
			/// The global identifier for the command set used by the ORM designer.
			/// </summary>
			public static readonly Guid guidORMDesignerCommandSet = new Guid("7C51C000-1EAD-4b39-89B5-42BC9F49EA24");    // keep in sync with SatDll\PkgCmd.ctc
			#region CommandID objects for commands
#if DEBUG
			/// <summary>
			/// A command to view the current store contents in debug mode
			/// </summary>
			public static readonly CommandID DebugViewStore = new CommandID(guidORMDesignerCommandSet, cmdIdDebugViewStore);
#endif // DEBUG
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
			/// The ORM Note Window item on the context menu
			/// </summary>
			public static readonly CommandID ViewNotesWindow = new CommandID(guidORMDesignerCommandSet, cmdIdViewNotesWindow);
			/// <summary>
			/// The ORM Note Window item on the context menu
			/// </summary>
			public static readonly CommandID NewContextWindow = new CommandID(guidORMDesignerCommandSet, cmdIdNewORMContextWindow);
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
			/// The ORM Model Browser Window item on the context menu
			/// </summary>
			public static readonly CommandID ViewNewModelBrowser = new CommandID(guidORMDesignerCommandSet, cmdIdViewNewModelBrowser);
			/// <summary>
			/// The ORM Verbalization Browser toolbar button for positive verbalization
			/// </summary>
			public static readonly CommandID ShowPositiveVerbalization = new CommandID(guidORMDesignerCommandSet, cmdIdShowPositiveVerbalization);
			/// <summary>
			/// The ORM Verbalization Browser toolbar button for negative verbalization
			/// </summary>
			public static readonly CommandID ShowNegativeVerbalization = new CommandID(guidORMDesignerCommandSet, cmdIdShowNegativeVerbalization);
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
			public static readonly CommandID VerbalizationToolBar = new CommandID(guidORMDesignerCommandSet, menuIdVerbalizationToolBar);
			/// <summary>
			/// Available on any role belonging to the active RoleSequence in the active MCEC or SCEC.
			/// </summary>
			public static readonly CommandID ViewActivateRoleSequence = new CommandID(guidORMDesignerCommandSet, cmdIdActivateRoleSequence);
			/// <summary>
			/// Available on any role belonging to any RoleSequence in the active MCEC.
			/// </summary>
			public static readonly CommandID ViewDeleteRoleSequence = new CommandID(guidORMDesignerCommandSet, cmdIdDeleteRoleSequence);
			/// <summary>
			/// Available on any non-active external constraint.
			/// </summary>
			public static readonly CommandID ViewEditExternalConstraint = new CommandID(guidORMDesignerCommandSet, cmdIdEditExternalConstraint);
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
			#endregion //CommandID objects for menus
			#region cmdIds
			// IMPORTANT: keep these constants in sync with SatDll\PkgCmdID.h

#if DEBUG
			/// <summary>
			/// A command to view the current store contents in debug mode
			/// </summary>
			private const int cmdIdDebugViewStore = 0x28FF;
#endif // DEBUG
			/// <summary>
			/// The NewORM Model Browser item on the view menu
			/// </summary>
			private const int cmdIdViewNewModelBrowser = 0x2899;
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
			/// Available on any non-active external constraint.
			/// </summary>
			private const int cmdIdEditExternalConstraint = 0x2908;
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
			private const int cmdIdErrorListEnd = 0x2aff;
			/// <summary>
			/// The context menu for the diagram
			/// </summary>
			private const int menuIdContextMenu = 0x0100;
			/// <summary>
			/// The toolbar for the verbalization window
			/// </summary>
			private const int menuIdVerbalizationToolBar = 0x0101;
			/// <summary>
			/// The context menu for the New Reading Editor
			/// </summary>
			private const int menuIdReadingEditorContextMenu = 0x0103;
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
			private const int cmdIdNewORMContextWindow = 0x2917;
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
			#endregion
		}
	}
}
