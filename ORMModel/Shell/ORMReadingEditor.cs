#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.EnterpriseTools.Shell;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.ObjectModel.Editors;
using MSOLE = Microsoft.VisualStudio.OLE.Interop;
using System.Diagnostics;
using Microsoft.VisualStudio;
using System.ComponentModel.Design;

#endregion

namespace Neumont.Tools.ORM.Shell
{
	/// <summary>
	/// Defines the tool window that is used to modify the readings associated with a fact.
	/// </summary>
	[Guid("992C221B-4BE5-4A9B-900D-9882B4FA0F99")]
	[CLSCompliant(false)]
	public class ORMReadingEditorToolWindow : ToolWindow, MSOLE.IOleCommandTarget
	{
		#region Member variables
		private ReadingsViewForm myForm = new ReadingsViewForm();
		private ORMDesignerDocData myCurrentDocument;
		#endregion // Member variables

		#region construction
		/// <summary>
		/// Creates a new instance of the reading editor tool window.
		/// </summary>
		public ORMReadingEditorToolWindow(IServiceProvider serviceProvider) : base(serviceProvider)
		{
			IMonitorSelectionService monitor = (IMonitorSelectionService)serviceProvider.GetService(typeof(IMonitorSelectionService));
			monitor.DocumentWindowChanged += new EventHandler<MonitorSelectionEventArgs>(DocumentWindowChangedEvent);
			monitor.SelectionChanged += new EventHandler<MonitorSelectionEventArgs>(SelectionChangedEvent);
			CurrentDocument = monitor.CurrentDocument as ORMDesignerDocData;
		}
		#endregion

		#region selection monitor event handlers and helpers
		private void DocumentWindowChangedEvent(object sender, MonitorSelectionEventArgs e)
		{
			CurrentDocument = ((IMonitorSelectionService)sender).CurrentDocument as ORMDesignerDocData;
		}

		private void SelectionChangedEvent(object sender, MonitorSelectionEventArgs e)
		{
			ORMDesignerDocView theView = e.NewValue as ORMDesignerDocView;
			if (theView != null)
			{
				FactType theFact = EditorUtility.ResolveContextFactType(theView.PrimarySelection);
				FactType currentFact = EditingFactType;
				if (theFact == null && currentFact != null)
				{
					EditingFactType = null;
				}
				//selection could change between the shapes that are related to the fact
				else if (!object.ReferenceEquals(theFact, currentFact))
				{
					EditingFactType = theFact;
				}
			}
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
				return 125;
			}
		}
		/// <summary>
		/// See <see cref="ToolWindow.BitmapResource"/>.
		/// </summary>
		protected override int BitmapIndex
		{
			get
			{
				return 1;
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
				return myForm;
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
			myForm.ActivateReading(reading);
		}

		/// <summary>
		/// Select the primary reading for the reading order
		/// matching the role order of the fact, if there
		/// isn't one activate the new entry for that order.
		/// </summary>
		/// <param name="fact">FactType</param>
		public void ActivateReading(FactType fact)
		{
			myForm.ActivateReading(fact);
		}
		#endregion // Reading activation helper

		#region properties
		/// <summary>
		/// Controls which fact is being displayed by the tool window.
		/// </summary>
		public FactType EditingFactType
		{
			get
			{
				return myForm.EditingFactType;
			}
			set
			{
				myForm.EditingFactType = value;
			}
		}

		private ORMDesignerDocData CurrentDocument
		{
			set
			{
				if (myCurrentDocument != null)
				{
					if (value != null && object.ReferenceEquals(myCurrentDocument, value))
					{
						return;
					}
					myForm.ReadingEditor.DetachEventHandlers(myCurrentDocument.Store);
				}
				myCurrentDocument = value;
				if (value != null)
				{
					myForm.ReadingEditor.AttachEventHandlers(myCurrentDocument.Store);
				}
				else
				{
					EditingFactType = null;
				}
			}
		}
		#endregion

		#region nested class ReadingsViewForm
		private class ReadingsViewForm : Form
		{
			private ReadingEditor myReadingEditor;
			private Label myNoSelectionLabel;

			#region construction
			public ReadingsViewForm()
			{
				Initialize();
			}

			private void Initialize()
			{
				myReadingEditor = new ReadingEditor();
				this.Controls.Add(myReadingEditor);
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
			public FactType EditingFactType
			{
				get
				{
					return myReadingEditor.EditingFactType;
				}
				set
				{
					bool editVisible = value != null;
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
			/// <param name="fact"></param>
			public void ActivateReading(FactType fact)
			{
				myReadingEditor.ActivateReading(fact);
			}

			public void DeleteSelectedReading()
			{
				myReadingEditor.DeleteSelectedReading();
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
				hr = (int)MSOLE.Constants.OLECMDERR_E_NOTSUPPORTED;
				switch ((VSConstants.VSStd97CmdID)nCmdID)
				{
					case VSConstants.VSStd97CmdID.Delete:
						// If we aren't in label edit (in which case the commands should be passed down to the
						// VirtualTreeView control), handle the delete command and set the hresult to a handled status.
						if (this.myForm.ReadingEditor.EditingFactType != null)
						{
							if (!myReadingEditor.InLabelEdit)
							{
								if (myReadingEditor.IsReadingPaneActive && myReadingEditor.CurrentReading != null)
								{
									myForm.DeleteSelectedReading();
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
						if (this.myForm.ReadingEditor.EditingFactType != null)
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

			if (!handled && myCurrentDocument != null)
			{
				Debug.Assert(ErrorHandler.Failed(hr));
				MSOLE.IOleCommandTarget forwardTo = myCurrentDocument.UndoManager.VSUndoManager as MSOLE.IOleCommandTarget;
				// If the command wasn't handled already, give the undo manager a chance to handle the command.
				hr = forwardTo.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
			}
			return hr;
		}
		int MSOLE.IOleCommandTarget.Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
		{
			return Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
		}

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
						if (this.myForm.ReadingEditor.EditingFactType != null)
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
						if (this.myForm.ReadingEditor.EditingFactType != null)
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
			if (!handled && myCurrentDocument != null)
			{
				Debug.Assert(ErrorHandler.Failed(hr));
				MSOLE.IOleCommandTarget forwardTo = myCurrentDocument.UndoManager.VSUndoManager as MSOLE.IOleCommandTarget;
				// If the command wasn't handled already, forward it to the undo manager.
				hr = forwardTo.QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
			}
			return hr;
		}
		int MSOLE.IOleCommandTarget.QueryStatus(ref Guid pguidCmdGroup, uint cCmds, MSOLE.OLECMD[] prgCmds, IntPtr pCmdText)
		{
			return QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
		}

		#endregion
	}
}
