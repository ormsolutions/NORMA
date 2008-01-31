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

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.VirtualTreeGrid;
using MSOLE = Microsoft.VisualStudio.OLE.Interop;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.ObjectModel.Design;
using Neumont.Tools.ORM.ShapeModel;
using Neumont.Tools.ORM.Shell;
using Neumont.Tools.Modeling;
using Neumont.Tools.Modeling.Shell;
using System.Collections.ObjectModel;

namespace Neumont.Tools.ORM.Shell
{
	/// <summary>
	/// Defines the tool window that is used to modify the readings associated with a fact.
	/// </summary>
	[Guid("1E45D5B8-890A-4ED5-AE1A-80BE221398D2")]
	[CLSCompliant(false)]
	public class ORMReadingEditorToolWindow : ORMToolWindow, MSOLE.IOleCommandTarget
	{
		#region Member variables
		private ReadingsViewForm myForm;
		private IServiceProvider myCtorServiceProvider;
		#endregion // Member variables
		#region construction
		/// <summary>
		/// Creates a new instance of the reading editor tool window.
		/// </summary>
		public ORMReadingEditorToolWindow(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			this.myCtorServiceProvider = serviceProvider;
		}

		#endregion
		#region ToolWindow overrides
		/// <summary>
		/// See <see cref="ToolWindow.BitmapResource"/>.
		/// </summary>
		protected override int BitmapResource
		{
			get
			{
				return PackageResources.Id.ToolWindowIcons;
			}
		}
		/// <summary>
		/// See <see cref="ToolWindow.BitmapIndex"/>.
		/// </summary>
		protected override int BitmapIndex
		{
			get
			{
				return PackageResources.ToolWindowIconIndex.ReadingEditor;
			}
		}


		private object myCommandSet;
		private bool myCommandsPopulated;
		/// <summary>
		/// returns the menu service and instantiates a new command set if none exists
		/// </summary>
		public override IMenuCommandService MenuService
		{
			get
			{
				IMenuCommandService retVal = base.MenuService;
				if (retVal != null && !myCommandsPopulated)
				{
					myCommandsPopulated = true;
					myCommandSet = new ReadingEditorCommandSet(myCtorServiceProvider, retVal);
				}
				return retVal;
			}
		}

		/// <summary>
		/// Gets the title that will be displayed on the tool window.
		/// </summary>
		public override string WindowTitle
		{
			get
			{
				return ResourceStrings.ModelReadingEditorWindowTitle;
			}
		}

		/// <summary>
		/// Gets the window object that will be hosted by the tool window.
		/// </summary>
		public override IWin32Window Window
		{
			get
			{
				ReadingsViewForm form = myForm;
				if (form == null)
				{
					myForm = form = new ReadingsViewForm(this);
					ORMDesignerDocData currentDoc = CurrentDocument;
					if (currentDoc != null)
					{
						AttachEventHandlers(currentDoc);
						OnORMSelectionContainerChanged();
					}
				}
				return form;
			}
		}
		#endregion
		#region Reading activation helper
		/// <summary>
		/// Select the current reading in the window. The
		/// reading must be the child of the current fact.
		/// </summary>
		/// <param name="reading">Reading</param>
		public void ActivateReading(Reading reading)
		{
			ReadingsViewForm form = myForm;
			if (form != null)
			{
				form.ActivateReading(reading);
			}
		}

		/// <summary>
		/// Select the primary reading for the reading order
		/// matching the role order of the fact, if there
		/// isn't one activate the new entry for that order.
		/// </summary>
		/// <param name="fact">FactType</param>
		public void ActivateReading(FactType fact)
		{
			ReadingsViewForm form = myForm;
			if (form != null)
			{
				form.ActivateReading(fact);
			}
		}
		#endregion // Reading activation helper
		#region Selection Monitoring
		/// <summary>
		/// Clear a covered window when the document changes and when the selection changes.
		/// </summary>
		protected override CoveredFrameContentActions CoveredFrameContentActions
		{
			get
			{
				return CoveredFrameContentActions.ClearContentsOnSelectionChanged | CoveredFrameContentActions.ClearContentsOnDocumentChanged;
			}
		}
		/// <summary>
		/// Update our reading to reflect the current selection
		/// </summary>
		protected override void OnORMSelectionContainerChanged()
		{
			if (CurrentORMSelectionContainer != null)
			{
				ICollection selectedObjects = base.GetSelectedComponents();
				FactType theFact = null;
				FactType secondaryFact = null;
				if (selectedObjects != null)
				{
					foreach (object o in selectedObjects)
					{
						FactType testFact = ORMEditorUtility.ResolveContextFactType(o);
						// Handle selection of multiple elements as long as
						// they all resolve to the same fact
						if (theFact == null)
						{
							theFact = testFact;
							Role testImpliedRole;
							RoleProxy proxy;
							if (null != (testImpliedRole = o as Role) &&
								null != (proxy = testImpliedRole.Proxy))
							{
								secondaryFact = proxy.FactType;
							}
						}
						else if (testFact != theFact)
						{
							theFact = null;
							break;
						}
						else
						{
							secondaryFact = null;
						}
					}
				}
				if (theFact != null && theFact.HasImplicitReadings)
				{
					theFact = null;
					secondaryFact = null;
				}

				ActiveFactType activeFact = EditingFactType;

				FactType currentFact = activeFact.FactType;
				FactType currentImpliedFact = activeFact.ImpliedFactType;

				if (theFact == null && currentFact != null)
				{
					EditingFactType = ActiveFactType.Empty;
				}
				//selection could change between the shapes that are related to the fact
				else if (theFact != currentFact || secondaryFact != currentImpliedFact)
				{
					ReadOnlyCollection<RoleBase> displayOrder = null;
					ORMDesignerDocView docView = CurrentORMSelectionContainer as ORMDesignerDocView;
					if (docView != null)
					{
						SelectedShapesCollection shapes = docView.CurrentDesigner.DiagramClientView.Selection;
						if (shapes.Count == 1)
						{
							DiagramItem item = null;
							foreach (DiagramItem iter in shapes)
							{
								item = iter;
								break;
							}
							ShapeElement shape = item.Shape;
							FactTypeShape factShape = shape as FactTypeShape;
							while (factShape == null)
							{
								shape = shape.ParentShape;
								if (shape == null)
								{
									break;
								}
								factShape = shape as FactTypeShape;
							}
							if (factShape != null)
							{
								displayOrder = new ReadOnlyCollection<RoleBase>(factShape.DisplayedRoleOrder);
							}
						}
					}
					EditingFactType = new ActiveFactType(theFact, secondaryFact, displayOrder);
				}
			}
			else
			{
				EditingFactType = ActiveFactType.Empty;
			}
		}
		#endregion // Selection Monitoring
		#region properties

		/// <summary>
		/// Controls which fact is being displayed by the tool window.
		/// </summary>
		public ActiveFactType EditingFactType
		{
			get
			{
				ReadingsViewForm form = myForm;
				return (form != null) ? form.EditingFactType : ActiveFactType.Empty;
			}
			set
			{
				ReadingsViewForm form = myForm;
				if (form != null)
				{
					form.EditingFactType = value;
				}
			}
		}
		#endregion
		#region nested class ReadingsViewForm
		private sealed class ReadingsViewForm : ContainerControl
		{
			private readonly ReadingEditor myReadingEditor;
			private readonly Label myNoSelectionLabel;

			#region construction
			public ReadingsViewForm(ORMReadingEditorToolWindow toolWindow)
			{
				myReadingEditor = new ReadingEditor(toolWindow);
				this.Controls.Add(myReadingEditor);
				System.Drawing.Point location = this.Controls[this.Controls.Count - 1].Location;
				myReadingEditor.Dock = DockStyle.Fill;
				myReadingEditor.Visible = false;
				myNoSelectionLabel = new Label();
				myNoSelectionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
				myNoSelectionLabel.Text = ResourceStrings.ModelReadingEditorUnsupportedSelectionText;
				this.Controls.Add(myNoSelectionLabel);
				myNoSelectionLabel.Dock = DockStyle.Fill;
				myNoSelectionLabel.Visible = true;
			}
			#endregion

			#region properties
			public ActiveFactType EditingFactType
			{
				get
				{
					return myReadingEditor.EditingFactType;
				}
				set
				{
					bool editVisible = !value.IsEmpty;
					myReadingEditor.Visible = editVisible;
					myNoSelectionLabel.Visible = !editVisible;
					myReadingEditor.EditingFactType = value;
				}
			}
			public ReadingEditor ReadingEditor
			{
				get
				{
					return myReadingEditor;
				}
			}
			#endregion

			#region Reading activation helper
			/// <summary>
			/// Select the current reading in the window. The
			/// reading must be the child of the current fact.
			/// </summary>
			/// <param name="reading">Reading</param>
			public void ActivateReading(Reading reading)
			{
				myReadingEditor.ActivateReading(reading);
			}

			/// <summary>
			/// Select the primary reading of the order matching
			/// the display order of the fact, if one doesn't
			/// exist select the new entry.
			/// </summary>
			/// <param name="fact">Fact</param>
			public void ActivateReading(FactType fact)
			{
				myReadingEditor.ActivateReading(fact);
			}

			public void DeleteSelectedReading()
			{
				myReadingEditor.OnMenuDeleteSelectedReading();
			}

			public void EditSelectedReading()
			{
				myReadingEditor.EditSelectedReading();
			}
			#endregion // Reading activation helper
		}
		#endregion
		#region IOleCommandTarget Members

		/// <summary>
		/// Provides a first chance to tell the shell that this window is capable of handling certain commands. Implements IOleCommandTarget.QueryStatus
		/// </summary>
		protected int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, MSOLE.OLECMD[] prgCmds, IntPtr pCmdText)
		{
			int hr = VSConstants.S_OK;
			bool handled = true;
			// Only handle commands from the Office 97 Command Set (aka VSStandardCommandSet97).
			if (pguidCmdGroup == VSConstants.GUID_VSStandardCommandSet97)
			{
				// There typically is only one command passed in to this array - in any case, we only care
				// about the first command.
				MSOLE.OLECMD cmd = prgCmds[0];
				switch ((VSConstants.VSStd97CmdID)cmd.cmdID)
				{
					case VSConstants.VSStd97CmdID.Delete:
						// Inform the shell that we should have a chance to handle the delete command.
						if (!this.myForm.ReadingEditor.EditingFactType.IsEmpty)
						{
							cmd.cmdf = (int)(MSOLE.OLECMDF.OLECMDF_SUPPORTED | MSOLE.OLECMDF.OLECMDF_ENABLED);
							prgCmds[0] = cmd;
						}
						else
						{
							goto default;
						}
						break;

					case VSConstants.VSStd97CmdID.EditLabel:
						// Inform the shell that we should have a chance to handle the edit label (rename) command.
						if (!this.myForm.ReadingEditor.EditingFactType.IsEmpty)
						{
							cmd.cmdf = (int)(MSOLE.OLECMDF.OLECMDF_SUPPORTED | MSOLE.OLECMDF.OLECMDF_ENABLED);
							prgCmds[0] = cmd;
						}
						else
						{
							goto default;
						}
						break;

					default:
						// Inform the shell that we don't support any other commands.
						handled = false;
						hr = (int)MSOLE.Constants.OLECMDERR_E_NOTSUPPORTED;
						break;
				}
			}
			else
			{
				// Inform the shell that we don't recognize this command group.
				handled = false;
				hr = (int)MSOLE.Constants.OLECMDERR_E_UNKNOWNGROUP;
			}
			if (!handled)
			{
				Debug.Assert(ErrorHandler.Failed(hr));
				ModelingDocData docData = CurrentDocument;
				Microsoft.VisualStudio.Modeling.Shell.UndoManager undoManager;
				MSOLE.IOleCommandTarget forwardTo;
				if ((docData != null &&
					null != (undoManager = docData.UndoManager) &&
					null != (forwardTo = undoManager.VSUndoManager as MSOLE.IOleCommandTarget)) ||
					null != (forwardTo = GetService(typeof(MSOLE.IOleCommandTarget)) as MSOLE.IOleCommandTarget))
				{
					// If the command wasn't handled already, forward it to the undo manager.
					hr = forwardTo.QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
				}
			}
			return hr;
		}
		int MSOLE.IOleCommandTarget.QueryStatus(ref Guid pguidCmdGroup, uint cCmds, MSOLE.OLECMD[] prgCmds, IntPtr pCmdText)
		{
			return QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
		}
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
		/// <summary>
		/// Provides a first chance to handle any command that MSOLE.IOleCommandTarget.QueryStatus
		/// informed the shell to pass to this window. Implements IOleCommandTarget.Exec
		/// </summary>
		protected int Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
		{
			int hr = 0;
			bool handled = true;
			// Only handle commands from the Office 97 Command Set (aka VSStandardCommandSet97).
			if (pguidCmdGroup == VSConstants.GUID_VSStandardCommandSet97)
			{
				ReadingEditor myReadingEditor = this.myForm.ReadingEditor;
				// Default to a not-supported status.
				switch ((VSConstants.VSStd97CmdID)nCmdID)
				{
					case VSConstants.VSStd97CmdID.Delete:
						// If we aren't in label edit (in which case the commands should be passed down to the
						// VirtualTreeView control), handle the delete command and set the hresult to a handled status.
						if (!this.myForm.ReadingEditor.EditingFactType.IsEmpty)
						{
							if (!myReadingEditor.InLabelEdit)
							{
								if (myReadingEditor.IsReadingPaneActive && myReadingEditor.CurrentReading != null)
								{
									myForm.DeleteSelectedReading();
								}
							}
							else
							{
								Control editControl = myReadingEditor.LabelEditControl;
								if (editControl != null)
								{
									IntPtr editHandle = editControl.Handle;
									// WM_KEYDOWN == 0x100
									SendMessage(editHandle, 0x100, (int)Keys.Delete, 1);
									// WM_KEYUP == 0x101
									SendMessage(editHandle, 0x101, (int)Keys.Delete, 0x40000001);
								}
							}
							// We enabled the command, so we say we handled it regardless of the further conditions
							hr = VSConstants.S_OK;
						}
						else
						{
							goto default;
						}
						break;

					case VSConstants.VSStd97CmdID.EditLabel:
						// If we aren't in label edit (in which case the commands should be passed down to the
						// VirtualTreeView control), handle the edit command and set the hresult to a handled status.
						if (!this.myForm.ReadingEditor.EditingFactType.IsEmpty)
						{
							if (myReadingEditor.CurrentReading != null)
							{
								if (myReadingEditor.IsReadingPaneActive)
								{
									myForm.EditSelectedReading();
								}
							}
							// We enabled the command, so we say we handled it regardless of the further conditions
							hr = VSConstants.S_OK;
						}
						else
						{
							goto default;
						}
						break;

					default:
						// If the command is from our command set, but not explicitly handled, inform the shell
						// that we didn't handle the command.
						handled = false;
						hr = (int)MSOLE.Constants.OLECMDERR_E_NOTSUPPORTED;
						break;
				}
			}
			// The command is from an unknown group.
			else
			{
				handled = false;
				hr = (int)MSOLE.Constants.OLECMDERR_E_UNKNOWNGROUP;
			}
			if (!handled)
			{
				Debug.Assert(ErrorHandler.Failed(hr));
				ModelingDocData docData = CurrentDocument;
				Microsoft.VisualStudio.Modeling.Shell.UndoManager undoManager;
				MSOLE.IOleCommandTarget forwardTo;
				if ((docData != null &&
					null != (undoManager = docData.UndoManager) &&
					null != (forwardTo = undoManager.VSUndoManager as MSOLE.IOleCommandTarget)) ||
					null != (forwardTo = GetService(typeof(MSOLE.IOleCommandTarget)) as MSOLE.IOleCommandTarget))
				{
					// If the command wasn't handled already, give the undo manager a chance to handle the command.
					hr = forwardTo.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
				}
			}
			return hr;
		}
		int MSOLE.IOleCommandTarget.Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
		{
			return Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
		}

		#endregion
		#region event handler attach/detach methods
		/// <summary>
		/// Manages event handlers in the store so that the tool window
		/// contents can be updated to reflect any model changes.
		/// </summary>
		protected override void ManageEventHandlers(Store store, ModelingEventManager eventManager, EventHandlerAction action)
		{
			ReadingsViewForm form = myForm;
			ReadingEditor readingEditor = (form != null) ? form.ReadingEditor : null;

			if (readingEditor != null)
			{
				readingEditor.ManageEventHandlers(store, eventManager, action);
			}
		}
		#endregion
		#region Nested Tool Window Class
		private sealed class ReadingEditorCommandSet : MarshalByRefObject, IDisposable
		{
			private IMenuCommandService myMenuService;
			private IMonitorSelectionService myMonitorSelection;
			private IServiceProvider myServiceProvider;
			private MenuCommand[] myCommands;

			public ReadingEditorCommandSet(IServiceProvider provider, IMenuCommandService menuService)
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
			private ORMReadingEditorToolWindow CurrentToolWindow
			{
				get
				{
					return MonitorSelection.CurrentWindow as ORMReadingEditorToolWindow;
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
				//IMonitorSelectionService service = MonitorSelection;
				//ORMReadingEditorToolWindow.OnStatusCommand(sender, ORMDesignerCommands.Delete, service.CurrentWindow as ORMReadingEditorToolWindow);
			}
			public void OnMenuDelete(Object sender, EventArgs e)
			{
				//ORMReadingEditorToolWindow currentWindow = CurrentToolWindow;
				//if (currentWindow != null)
				//{
				//    currentWindow.OnMenuDelete((sender as OleMenuCommand).Text);
				//}
			}
			#endregion
		}

		#region Nested Click Location Class

		#endregion //Nested Click Location Class


		#endregion //Nested Tool Window Class
	}
}
