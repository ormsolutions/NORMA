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

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using MSOLE = Microsoft.VisualStudio.OLE.Interop;
using ORMSolutions.ORMArchitect.Framework.Design;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Framework.Shell;

namespace ORMSolutions.ORMArchitect.Core.Shell
{
	#region CommandTargetTextBox control
	/// <summary>
	/// A <see cref="TextBox"/> that supports standard routed editing commands
	/// </summary>
	[CLSCompliant(false)]
	public class CommandTargetTextBox : TextBox, MSOLE.IOleCommandTarget
	{
		#region PInvoke functions
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern IntPtr SendMessage(HandleRef handle, int msg, int wParam, int lParam);
		#endregion // PInvoke functions
		#region Member variables
		private IServiceProvider myServiceProvider;
		private bool myHasSelection = false;
		private bool myHasText = false;
		#endregion // Member variables
		#region Constructors
		/// <summary>
		/// Enable notification
		/// </summary>
		public CommandTargetTextBox(IServiceProvider serviceProvider)
		{
			myHasSelection = false;
			myServiceProvider = serviceProvider;
			this.SetStyle(ControlStyles.EnableNotifyMessage, true);
		}
		#endregion // Constructors
		#region IOleCommandTarget Implementation
		/// <summary>
		/// Implements <see cref="MSOLE.IOleCommandTarget.QueryStatus"/>
		/// </summary>
		protected int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, MSOLE.OLECMD[] prgCmds, IntPtr pCmdText)
		{
			int hr = (int)MSOLE.Constants.OLECMDERR_E_NOTSUPPORTED;
			if (pguidCmdGroup == VSConstants.GUID_VSStandardCommandSet97)
			{
				int flags = 0; // 1 = SUPPORTED, 3 = SUPPORTED and ENABLED
				switch ((VSConstants.VSStd97CmdID)prgCmds[0].cmdID)
				{
					case VSConstants.VSStd97CmdID.Copy:
						flags = SelectionLength != 0 ? 3 : 1;
						break;
					case VSConstants.VSStd97CmdID.Cut:
						flags = (!ReadOnly && SelectionLength != 0) ? 3 : 1;
						break;
					case VSConstants.VSStd97CmdID.Paste:
						flags = (!ReadOnly && (Clipboard.ContainsText(TextDataFormat.Text) || Clipboard.ContainsText(TextDataFormat.UnicodeText))) ? 3 : 1;
						break;
					case VSConstants.VSStd97CmdID.SelectAll:
						flags = TextLength != 0 ? 3 : 1;
						break;
					case VSConstants.VSStd97CmdID.Undo:
						flags = !ReadOnly && CanUndo ? 3 : 1;
						break;
					case VSConstants.VSStd97CmdID.Delete:
						flags = 3;
						break;
					case VSConstants.VSStd97CmdID.Redo:
					case VSConstants.VSStd97CmdID.MultiLevelRedo:
					case VSConstants.VSStd97CmdID.MultiLevelUndo:
						flags = 1;
						break;
				}
				if (flags != 0)
				{
					prgCmds[0].cmdf = (uint)flags;
					hr = VSConstants.S_OK;
				}
			}
			return hr;
		}
		int MSOLE.IOleCommandTarget.QueryStatus(ref Guid pguidCmdGroup, uint cCmds, MSOLE.OLECMD[] prgCmds, IntPtr pCmdText)
		{
			return QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
		}
		/// <summary>
		/// Implements <see cref="MSOLE.IOleCommandTarget.Exec"/>
		/// </summary>
		int Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
		{
			int hr = (int)MSOLE.Constants.OLECMDERR_E_NOTSUPPORTED;
			if (pguidCmdGroup == VSConstants.GUID_VSStandardCommandSet97)
			{
				switch ((VSConstants.VSStd97CmdID)nCmdID)
				{
					case VSConstants.VSStd97CmdID.Copy:
						Copy();
						hr = VSConstants.S_OK;
						break;
					case VSConstants.VSStd97CmdID.Cut:
						Cut();
						hr = VSConstants.S_OK;
						break;
					case VSConstants.VSStd97CmdID.Paste:
						Paste();
						hr = VSConstants.S_OK;
						break;
					case VSConstants.VSStd97CmdID.SelectAll:
						SelectAll();
						hr = VSConstants.S_OK;
						break;
					case VSConstants.VSStd97CmdID.Undo:
						Undo();
						hr = VSConstants.S_OK;
						break;
					case VSConstants.VSStd97CmdID.Delete:
						HandleRef handleRef = new HandleRef(this, Handle);
						// WM_KEYDOWN == 0x100
						SendMessage(handleRef, 0x100, (int)Keys.Delete, 1);
						// WM_KEYUP == 0x101
						SendMessage(handleRef, 0x101, (int)Keys.Delete, 0x40000001);
						hr = VSConstants.S_OK;
						break;
				}
			}
			return hr;
		}
		int MSOLE.IOleCommandTarget.Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
		{
			return Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
		}
		#endregion // IOleCommandTarget Implementation
		#region Base overrides
		/// <summary>
		/// Catch selection change notifications to force UI update
		/// </summary>
		protected override void OnNotifyMessage(Message m)
		{
			if (m.Msg == 0x282 && m.WParam == (IntPtr)0xB) // WM_IME_NOTIFY = 0x282, IMN_SETCOMPOSITIONWINDOW = 0xB
			{
				bool hasCurrentSelection = SelectionLength != 0;
				bool hasText = hasCurrentSelection || TextLength != 0;
				if ((hasCurrentSelection ^ myHasSelection) || (hasText ^ myHasText))
				{
					myHasSelection = hasCurrentSelection;
					myHasText = hasText;
					if (this.Focused)
					{
						IVsUIShell service = myServiceProvider.GetService(typeof(SVsUIShell)) as IVsUIShell;
						if (service != null)
						{
							service.UpdateCommandUI(1);
						}
					}
				}
			}
		}
		#endregion // Base overrides
	}
	#endregion // CommandTargetTextBox control
	#region ORMNotesToolWindow class
	/// <summary>
	/// The ToolWindow which is responsible for displaying and allowing
	/// the update of notes on elements implementing <see cref="INoteOwner{Note}"/>.
	/// </summary>
	[Guid("A7C9E14E-9EEE-4D79-A7F4-9E9D1A567498")]
	[CLSCompliant(false)]
	public class ORMNotesToolWindow : ORMBaseNoteToolWindow<Note>
	{
		#region Constructor
		/// <summary>
		/// Returns a new <see cref="ORMNotesToolWindow"/>
		/// </summary>
		public ORMNotesToolWindow(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
		#endregion // Constructor
		#region Base overrides
		private static readonly Guid[] myNoteRoleIdentifiers = { ObjectTypeHasNote.NoteDomainRoleId, FactTypeHasNote.NoteDomainRoleId, ModelHasModelNote.NoteDomainRoleId, ModelHasPrimaryNote.NoteDomainRoleId };
		/// <summary>
		/// Return role identifiers for <see cref="ObjectType"/>, <see cref="FactType"/> and <see cref="ModelNote"/> <see cref="Note"/>s
		/// </summary>
		protected override Guid[] GetNoteRoleIdentifiers()
		{
			return myNoteRoleIdentifiers;
		}
		/// <summary>
		/// Returns the title of the window.
		/// </summary>
		public override string WindowTitle
		{
			get
			{
				return ResourceStrings.ModelNotesWindowTitle;
			}
		}
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
				return PackageResources.ToolWindowIconIndex.NotesEditor;
			}
		}
		/// <summary>
		/// Base override
		/// </summary>
		protected override string EmptyDisplayText
		{
			get
			{
				return ResourceStrings.ModelNotesWindowEmptyDisplayText;
			}
		}
		#endregion // Base overrides
	}
	#endregion // ORMNotesToolWindow class
	#region ORMDescriptionToolWindow class
	/// <summary>
	/// The ToolWindow which is responsible for displaying and allowing
	/// the update of definitions on elements implementing <see cref="INoteOwner{Definition}"/>.
	/// </summary>
	[Guid("FC6D8343-48D1-4294-915F-01B6350E0E12")]
	[CLSCompliant(false)]
	public class ORMDescriptionToolWindow : ORMBaseNoteToolWindow<Definition>
	{
		#region Constructor
		/// <summary>
		/// Returns a new <see cref="ORMDescriptionToolWindow"/>
		/// </summary>
		public ORMDescriptionToolWindow(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
		#endregion // Constructor
		#region Base overrides
		private static readonly Guid[] myNoteRoleIdentifiers = { ObjectTypeHasDefinition.DefinitionDomainRoleId, FactTypeHasDefinition.DefinitionDomainRoleId, ModelHasDefinition.DefinitionDomainRoleId };
		/// <summary>
		/// Return role identifiers for <see cref="ObjectType"/> and <see cref="FactType"/> <see cref="Definition"/>s
		/// </summary>
		protected override Guid[] GetNoteRoleIdentifiers()
		{
			return myNoteRoleIdentifiers;
		}
		/// <summary>
		/// Returns the title of the window.
		/// </summary>
		public override string WindowTitle
		{
			get
			{
				return ResourceStrings.ModelDescriptionWindowTitle;
			}
		}
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
				return PackageResources.ToolWindowIconIndex.DescriptionEditor;
			}
		}
		/// <summary>
		/// Base override
		/// </summary>
		protected override string EmptyDisplayText
		{
			get
			{
				return ResourceStrings.ModelDescriptionWindowEmptyDisplayText;
			}
		}
		#endregion // Base overrides
	}
	#endregion // ORMDescriptionToolWindow class
	#region ORMBaseNoteToolWindow class
	/// <summary>
	/// A base class for any ToolWindow that is used to display a note type field. Notes
	/// are assumed to be multiline string fields.
	/// </summary>
	/// <typeparam name="NoteType">The type of a DomainClass with a domain property called Text.</typeparam>
	[CLSCompliant(false)]
	public abstract class ORMBaseNoteToolWindow<NoteType> : ORMToolWindow, MSOLE.IOleCommandTarget where NoteType : ModelElement
	{
		#region Private data members
		private TextBox myTextBox;
		private List<INoteOwner<NoteType>> mySelectedNoteOwners;
		#endregion // Private data members
		#region Construction
		/// <summary>
		/// Returns a new <see cref="ORMBaseNoteToolWindow{NoteType}"/>
		/// </summary>
		protected ORMBaseNoteToolWindow(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
		/// <summary>
		/// Returns the parent of myTextBox.
		/// </summary>
		public override System.Windows.Forms.IWin32Window Window
		{
			get
			{
				TextBox textBox = myTextBox;	// Cache the textbox.
				if (textBox == null)	// If it's null,
				{
					myTextBox = textBox = new CommandTargetTextBox(this);	// instantiate it and set the properties, wire events,
					textBox.Dock = DockStyle.Fill;
					textBox.WordWrap = true;
					textBox.Multiline = true;
					textBox.ReadOnly = true;
					textBox.Enabled = false;
					textBox.Text = EmptyDisplayText;
					textBox.ScrollBars = ScrollBars.Vertical;
					textBox.LostFocus += new EventHandler(myTextBox_LostFocus);
					ContainerControl container = new ContainerControl();	// and set up a parent container.
					container.Controls.Add(textBox);
					container.ActiveControl = textBox;
					Guid commandSetId = typeof(ORMDesignerEditorFactory).GUID;
					Frame.SetGuidProperty((int)__VSFPROPID.VSFPROPID_InheritKeyBindings, ref commandSetId);
				}
				return textBox.Parent;	// Finally, pass the parent container back as the window.
			}
		}
		#endregion // Construction
		#region Event handlers
		/// <summary>
		/// Handles note change.
		/// </summary>
		private void NoteAlteredEventHandler(object sender, ElementPropertyChangedEventArgs e)
		{
			ModelElement element = e.ModelElement;
			Store store = element.Store;
			if (e.DomainProperty.Id == GetNoteTextPropertyId(store))
			{
				// First, see if the note element implements INoteOwner directly
				INoteOwner<NoteType> noteOwner = element as INoteOwner<NoteType>;
				if (noteOwner != null)
				{
					NoteAlteredEventHandler(noteOwner);
				}
				else
				{
					// If note, find the owning element
					Guid[] ownerRoles = GetNoteRoleIdentifiers();
					for (int i = 0; i < ownerRoles.Length; ++i)
					{
						noteOwner = DomainRoleInfo.GetLinkedElement(element, ownerRoles[i]) as INoteOwner<NoteType>;
						if (noteOwner != null)
						{
							NoteAlteredEventHandler(noteOwner);
							break;
						}
					}
				}
			}
		}
		/// <summary>
		/// Helper function to handler a note change event
		/// </summary>
		private void NoteAlteredEventHandler(INoteOwner<NoteType> noteOwner)
		{
			List<INoteOwner<NoteType>> currentOwners;
			if (null != noteOwner &&
				null != (currentOwners = mySelectedNoteOwners) &&
				currentOwners.Count != 0 &&
				currentOwners.Contains(noteOwner))
			{
				DisplayNotes();
			}
		}
		/// <summary>
		/// Event handler for the note textbox.  Forces the note to be saved
		/// if the textbox is enabled.
		/// </summary>
		void myTextBox_LostFocus(object sender, EventArgs e)
		{
			List<INoteOwner<NoteType>> owners = mySelectedNoteOwners;
			if (owners != null && owners.Count == 1 && myTextBox.Enabled)	// If we only have one selected note and the textbox is enabled,
			{
				SetNote(owners[0], myTextBox.Text);	// try to set the note.
			}
		}
		#endregion // Event handlers
		#region Helper functions
		/// <summary>
		/// Sets the NoteText property on an INoteOwner instance.
		/// </summary>
		/// <param name="owner">The INoteOwner on which the note should be set.</param>
		/// <param name="text">The text of the note.</param>
		private void SetNote(INoteOwner<NoteType> owner, string text)
		{
			PropertyDescriptor descriptor;
			ORMDesignerDocData currentDoc;
			IORMToolServices currentStore;
			if (null != owner &&
				null != (currentDoc = CurrentDocument) &&
				null != (currentStore = currentDoc.Store as IORMToolServices) &&
				currentStore.CanAddTransaction &&
				null != (descriptor = owner.NoteTextPropertyDescriptor))	// Be really defensive.
			{
				if (0 != string.CompareOrdinal((string)descriptor.GetValue(owner), text))
				{
					descriptor.SetValue(owner, text);
				}
			}
		}
		/// <summary>
		/// Populates mySelectedNoteOwners with all currently selected note
		/// owners in the current selection container.
		/// </summary>
		private void PopulateSelectedNoteOwners()
		{
			List<INoteOwner<NoteType>> selectedTypes = mySelectedNoteOwners;
			if (selectedTypes == null)
			{
				selectedTypes = new List<INoteOwner<NoteType>>();
				mySelectedNoteOwners = selectedTypes;
			}
			selectedTypes.Clear();	// Clear the list of selected root types.
			IORMSelectionContainer selectionContainer = CurrentORMSelectionContainer;
			if (selectionContainer != null)
			{

				ICollection objects = base.GetSelectedComponents();
				if (objects != null)
				{
					foreach (object o in objects)
					{
						INoteOwner<NoteType> owner = EditorUtility.ResolveContextInstance(o, false) as INoteOwner<NoteType>;	// and if they are an INoteOwner,
						if (owner != null)
						{
							selectedTypes.Add(owner);	// add them to the list of selected owners.
						}
					}
				}
			}
			DisplayNotes();	// Display the notes for all selected owners.
		}
		/// <summary>
		/// Return the text to show for an empty window display
		/// </summary>
		protected abstract string EmptyDisplayText { get;}
		/// <summary>
		/// Displays the notes for all selected note owners in the Notes Window.
		/// </summary>
		private void DisplayNotes()
		{
			List<INoteOwner<NoteType>> selectedNoteOwners = mySelectedNoteOwners; // Cache the list of selected note owners.
			int selectedNoteOwnersCount = selectedNoteOwners.Count; // Cache the count of selected note owners.
			TextBox textBox = myTextBox; // Cache the text box.
			if (textBox != null) // If the text box is not null,
			{
				textBox.Clear(); // clear it's text
				if (selectedNoteOwnersCount == 0) // If there aren't any selected note owners,
				{
					textBox.ReadOnly = true;
					textBox.Enabled = false;
					textBox.AppendText(EmptyDisplayText);
				}
				else if (selectedNoteOwnersCount == 1) // If there is only one selected note owner,
				{
					textBox.ReadOnly = false;
					textBox.Enabled = true;
					textBox.AppendText(selectedNoteOwners[0].NoteText); // add its note text to the text box.
				}
				else
				{
					textBox.ReadOnly = true; // Otherwise, the textbox should be read-only because multiple elements can't be edited together
					textBox.Enabled = true;
					StringBuilder sb = new StringBuilder();
					string formatString = ResourceStrings.ModelNotesWindowRootTypeNameNotesSeparatorFormatString;
					// root types are selected.
					bool first = true;
					for (int i = 0; i < selectedNoteOwnersCount; ++i) // Loop through the selected root types,
					{
						INoteOwner<NoteType> noteOwner = selectedNoteOwners[i]; // cache them locally,
						string noteText = noteOwner.NoteText; // and cache their note text.
						if (!string.IsNullOrEmpty(noteText)) // If there is note text on the note owner,
						{
							if (!first)
							{
								sb.AppendLine();
							}
							else
							{
								first = false;
							}
							sb.AppendFormat(CultureInfo.InvariantCulture, formatString, noteOwner.Name, noteText);
						}
					}
					textBox.Text = sb.ToString();
				}
			}
		}
		#endregion // Helper functions
		#region IOleCommandTarget Members
		/// <summary>
		/// Provides a first chance to tell the shell that this window is capable
		/// of handling certain commands. Implements IOleCommandTarget.QueryStatus
		/// </summary>
		protected int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, MSOLE.OLECMD[] prgCmds, IntPtr pCmdText)
		{
			int hr = VSConstants.S_OK;
			bool handled = true;
			if (pguidCmdGroup == VSConstants.GUID_VSStandardCommandSet97)	// Only handle commands from the Office 97
			// Command Set (aka VSStandardCommandSet97).
			{
				MSOLE.IOleCommandTarget forwardTo = myTextBox as MSOLE.IOleCommandTarget;
				if (forwardTo != null)
				{
					hr = forwardTo.QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
					handled = hr != (int)MSOLE.Constants.OLECMDERR_E_NOTSUPPORTED && hr != (int)MSOLE.Constants.OLECMDERR_E_UNKNOWNGROUP;
				}
				else
				{
					// Inform the shell that we don't support any other commands.
					hr = (int)MSOLE.Constants.OLECMDERR_E_NOTSUPPORTED;
					handled = false;
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
				else
				{
					hr = (int)MSOLE.Constants.MSOCMDERR_E_NOTSUPPORTED;
				}
			}
			return hr;
		}
		int MSOLE.IOleCommandTarget.QueryStatus(ref Guid pguidCmdGroup, uint cCmds, MSOLE.OLECMD[] prgCmds, IntPtr pCmdText)
		{
			return QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
		}

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
				// Default to a not-supported status.
				hr = (int)MSOLE.Constants.OLECMDERR_E_NOTSUPPORTED;
				MSOLE.IOleCommandTarget forwardTo = myTextBox as MSOLE.IOleCommandTarget;
				if (forwardTo != null)
				{
					hr = forwardTo.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
					// We enabled the command, so we say we handled it regardless of the further conditions
					if (hr != (int)MSOLE.Constants.OLECMDERR_E_NOTSUPPORTED && hr != (int)MSOLE.Constants.OLECMDERR_E_UNKNOWNGROUP)
					{
						hr = VSConstants.S_OK;
					}
					else
					{
						handled = false;
					}
				}
				else
				{
					// If the command is from our command set, but not explicitly handled, inform the shell
					// that we didn't handle the command.
					handled = false;
					hr = (int)MSOLE.Constants.OLECMDERR_E_NOTSUPPORTED;
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
		#region ORMToolWindow Implementation
		/// <summary>
		/// Populates the tool window when the selection changes
		/// </summary>
		protected override void OnORMSelectionContainerChanged()
		{
			PopulateSelectedNoteOwners();
		}
		/// <summary>
		/// Populate the tool window when the document changes
		/// </summary>
		protected override void OnCurrentDocumentChanged()
		{
			PopulateSelectedNoteOwners();
		}
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
		/// Return the role ids of the note end of all relationship that
		/// own this kind of note. This must return the same set of identifiers
		/// regardless of store instance.
		/// </summary>
		protected abstract Guid[] GetNoteRoleIdentifiers();
		private static EventHandler<ElementEventArgs>[] myOwningRelationshipHandlers = null;
		/// <summary>
		/// We cannot use anonymous delegates in ManageEventHandlers to respond to add/remove
		/// of relationships because the instance would change each time, making it impossible
		/// to remove the event handler from the watch list on a store. So, we build these
		/// once here. Note that this assumes that GetNoteRoleIdentifiers() returns a fixed
		/// set of identifiers and cannot change from store to store.
		/// </summary>
		private EventHandler<ElementEventArgs>[] GetOwningRelationshipHandlers(Store store)
		{
			EventHandler<ElementEventArgs>[] handlers = myOwningRelationshipHandlers;
			if (handlers == null)
			{
				Guid[] noteRoleIdentifiers = GetNoteRoleIdentifiers();
				handlers = new EventHandler<ElementEventArgs>[noteRoleIdentifiers.Length];
				for (int i = 0; i < noteRoleIdentifiers.Length; ++i)
				{
					DomainDataDirectory dataDirectory = store.DomainDataDirectory;
					DomainRoleInfo role = dataDirectory.FindDomainRole(noteRoleIdentifiers[i]);
					Guid oppositeRoleId = role.OppositeDomainRole.Id;
					handlers[i] = delegate(object sender, ElementEventArgs e)
					{
						NoteAlteredEventHandler(DomainRoleInfo.GetRolePlayer((ElementLink)e.ModelElement, oppositeRoleId) as INoteOwner<NoteType>);
					};
				}
				System.Threading.Interlocked.CompareExchange<EventHandler<ElementEventArgs>[]>(ref myOwningRelationshipHandlers, handlers, null);
				handlers = myOwningRelationshipHandlers;
			}
			return handlers;
		}
		private static Guid[] myNoteTextPropertyId; // Defined as an array so we can statically initialize without locking
		private Guid GetNoteTextPropertyId(Store store)
		{
			Guid[] propertyId = myNoteTextPropertyId;
			if (propertyId == null)
			{
				DomainDataDirectory dataDirectory = store.DomainDataDirectory;
				propertyId = new Guid[] { store.DomainDataDirectory.GetDomainClass(typeof(NoteType)).FindDomainProperty("Text", false).Id };
				System.Threading.Interlocked.CompareExchange<Guid[]>(ref myNoteTextPropertyId, propertyId, null);
				propertyId = myNoteTextPropertyId;
			}
			return propertyId[0];
		}
		/// <summary>
		/// Manages <see cref="EventHandler{TEventArgs}"/>s in the <see cref="Store"/> so that the <see cref="ORMNotesToolWindow"/>
		/// contents can be updated to reflect any model changes.
		/// </summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/>s should be managed.</param>
		/// <param name="eventManager">The <see cref="ModelingEventManager"/> used to manage the <see cref="EventHandler{TEventArgs}"/>s.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>s.</param>
		protected override void ManageEventHandlers(Store store, ModelingEventManager eventManager, EventHandlerAction action)
		{
			if (store == null || store.Disposed)
			{
				return; // Bail out
			}
			DomainDataDirectory dataDirectory = store.DomainDataDirectory;

			// Track Note additions and deletions changes
			Guid[] noteRoleIdentifiers = GetNoteRoleIdentifiers();
			EventHandler<ElementEventArgs>[] handlers = GetOwningRelationshipHandlers(store);
			for (int i = 0; i < noteRoleIdentifiers.Length; ++i)
			{
				EventHandler<ElementEventArgs> handler = handlers[i];
				DomainClassInfo classInfo = dataDirectory.FindDomainRole(noteRoleIdentifiers[i]).DomainRelationship;
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(handler), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(handler), action);
			}

			// Track Note.Text changes
			eventManager.AddOrRemoveHandler(dataDirectory.FindDomainProperty(GetNoteTextPropertyId(store)), new EventHandler<ElementPropertyChangedEventArgs>(NoteAlteredEventHandler), action);
		}
		#endregion // ORMToolWindow Implementation
	}
	#endregion // ORMBaseNoteToolWindow class
}
