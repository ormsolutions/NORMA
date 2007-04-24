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
using System.Drawing;
using System.Diagnostics;
using System.ComponentModel.Design;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;
using MSOLE = Microsoft.VisualStudio.OLE.Interop;
using Neumont.Tools.Modeling.Design;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.ObjectModel.Design;
using Neumont.Tools.Modeling;

namespace Neumont.Tools.ORM.Shell
{
	/// <summary>
	/// ToolWindow for hosting the Sample Population Editor
	/// </summary>
	[Guid("051209C1-250B-45a7-B7B1-8AFB50BEC9B7")]
	[CLSCompliant(false)]
	public class ORMSamplePopulationToolWindow : ORMToolWindow, MSOLE.IOleCommandTarget
	{
		private SamplePopulationEditor myEditor;

		#region Construction
		/// <summary>
		/// Construct a sample population tool window
		/// </summary>
		/// <param name="serviceProvider">Service provider</param>
		public ORMSamplePopulationToolWindow(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}

		/// <summary>
		/// Initialize here after we have the frame so we can grab the toolbar host
		/// </summary>
		protected override void Initialize()
		{
			base.Initialize();
			IVsToolWindowToolbarHost host = ToolBarHost;
			Debug.Assert(host != null); // Should be set with HasToolbar true
			if (host != null)
			{
				CommandID command = ORMDesignerDocView.ORMDesignerCommandIds.ViewSamplePopulationEditor;
				Guid commandGuid = command.Guid;
				host.AddToolbar(VSTWT_LOCATION.VSTWT_LEFT, ref commandGuid, (uint)command.ID);
			}
		}
		/// <summary>
		/// Make sure the toolbar flag gets set
		/// </summary>
		protected override bool HasToolBar
		{
			get
			{
				return true;
			}
		}
		#endregion // Construction
		#region ToolWindow Overrides
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
				return PackageResources.ToolWindowIconIndex.None;
			}
		}
		/// <summary>
		/// Gets the user control hosted in the tool window
		/// </summary>
		public override IWin32Window Window
		{
			get
			{
				SamplePopulationEditor editor = myEditor;
				if (editor == null)
				{
					myEditor = editor = new SamplePopulationEditor();
					ORMDesignerDocData currentDoc = CurrentDocument;
					if (currentDoc != null)
					{
						AttachEventHandlers(currentDoc);
						OnORMSelectionContainerChanged();
					}
				}
				return myEditor;
			}
		}
		/// <summary>
		/// Clean up any existing objects
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
			{
				if (myEditor != null)
				{
					myEditor.Dispose();
				}
			}
		}
		#endregion // ToolWindow Overrides
		#region Selection Monitoring
		/// <summary>
		/// Update our reading to reflect the current selection
		/// </summary>
		protected override void OnORMSelectionContainerChanged()
		{
			if (myEditor == null)
			{
				return;
			}
			ICollection selectedObjects;
			if (CurrentORMSelectionContainer != null && (null != (selectedObjects = base.GetSelectedComponents())) && selectedObjects.Count == 1)
			{
				foreach (object selectedObject in selectedObjects)
				{
					ObjectType testObjectType = EditorUtility.ResolveContextInstance(selectedObject, false) as ObjectType;
					if (testObjectType != null)
					{
						if (testObjectType.IsValueType)
						{
							SelectedValueType = testObjectType;
						}
						else if (testObjectType != null)
						{
							SelectedEntityType = testObjectType;
						}
						else
						{
							NullSelection();
						}
					}
					else
					{
						FactType testFactType = ORMEditorUtility.ResolveContextFactType(selectedObject) as FactType;
						if (testFactType != null)
						{
							SelectedFactType = testFactType;
						}
						else
						{
							NullSelection();
						}
					}
				}
			}
			else
			{
				NullSelection();
			}
		}
		#endregion // Selection Monitoring
		#region Properties
		/// <summary>
		/// Controls which ValueType is being displayed by the tool window.
		/// </summary>
		public ObjectType SelectedValueType
		{
			get
			{
				return myEditor.SelectedValueType;
			}
			set
			{
				myEditor.SelectedValueType = value;
			}
		}

		/// <summary>
		/// Controls which EntityType is being displayed by the tool window.
		/// </summary>
		public ObjectType SelectedEntityType
		{
			get
			{
				return myEditor.SelectedEntityType;
			}
			set
			{
				myEditor.SelectedEntityType = value;
			}
		}

		/// <summary>
		/// Controls which FactType is being displayed by the tool window.
		/// </summary>
		public FactType SelectedFactType
		{
			get
			{
				return myEditor.SelectedFactType;
			}
			set
			{
				myEditor.SelectedFactType = value;
			}
		}

		/// <summary>
		/// Nulls all properties for the embedded editor to force a blank window
		/// </summary>
		private void NullSelection()
		{
			Debug.Assert(myEditor != null, "Don't call before editor is initialized");
			myEditor.NullSelection();
		}
		#endregion // Properties
		#region PopulationMandatoryError Fixup
		/// <summary>
		/// Attempts to fix a PopulationMandatoryError
		/// </summary>
		/// <param name="error">Error to be corrected</param>
		public void AutoCorrectMandatoryError(PopulationMandatoryError error)
		{
			ObjectTypeInstance objectInstance = error.ObjectTypeInstance;
			LinkedElementCollection<Role> constraintRoles = error.MandatoryConstraint.RoleCollection;
			// Only supports simple mandatory constraints
			if (constraintRoles.Count == 1)
			{
				this.SelectedFactType = constraintRoles[0].FactType;
				myEditor.AutoCorrectMandatoryError(error);
				this.Show();
			}
		}
		#endregion
		#region ORMToolWindow Implementation
		/// <summary>
		/// Gets the title that will be displayed on the tool window.
		/// </summary>
		public override string WindowTitle
		{
			get
			{
				return ResourceStrings.ModelSamplePopulationEditorWindowTitle;
			}
		}
		/// <summary>
		/// Wires event handlers to the store.
		/// </summary>
		protected override void ManageEventHandlers(Store store, ModelingEventManager eventManager, EventHandlerAction action)
		{
			SamplePopulationEditor editor = myEditor;
			if (editor != null)
			{
				editor.ManageEventHandlers(store, eventManager, action);
			}
		}
		#endregion // ORMToolWindow Implementation
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
						if (this.myEditor.FullRowSelect)
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
						// Inform the shell that we should have a chance to handle the edit command
						if (!this.myEditor.FullRowSelect && !this.myEditor.InLabelEdit)
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
				SamplePopulationEditor samplePopulationEditor = this.myEditor;
				// Default to a not-supported status.
				switch ((VSConstants.VSStd97CmdID)nCmdID)
				{
					case VSConstants.VSStd97CmdID.Delete:
						if (samplePopulationEditor.SelectedEntityType != null
							|| samplePopulationEditor.SelectedFactType != null
							|| samplePopulationEditor.SelectedValueType != null)
						{
							if (samplePopulationEditor.FullRowSelect)
							{
								samplePopulationEditor.DeleteSelectedSamplePopulationInstance();
							}
							else
							{
								Control editControl = samplePopulationEditor.LabelEditControl;
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
						if (samplePopulationEditor.SelectedEntityType != null
							|| samplePopulationEditor.SelectedFactType != null
							|| samplePopulationEditor.SelectedValueType != null)
						{
							if (!samplePopulationEditor.FullRowSelect && !samplePopulationEditor.InLabelEdit)
							{
								samplePopulationEditor.BeginEditSamplePopulationInstance();
							}
							else
							{
								Control editControl = samplePopulationEditor.LabelEditControl;
								if (editControl != null)
								{
									IntPtr editHandle = editControl.Handle;
									// WM_KEYDOWN == 0x100
									SendMessage(editHandle, 0x100, (int)Keys.F2, 1);
									// WM_KEYUP == 0x101
									SendMessage(editHandle, 0x101, (int)Keys.F2, 0x40000001);
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
	}
}
