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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.VirtualTreeGrid;
using OleInterop = Microsoft.VisualStudio.OLE.Interop;
using Neumont.Tools.Modeling;
using Neumont.Tools.Modeling.Design;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.ShapeModel;
using Neumont.Tools.ORM.Shell;
using Neumont.Tools.Modeling.Shell;

#if VISUALSTUDIO_9_0
using VirtualTreeInPlaceControlFlags = Microsoft.VisualStudio.VirtualTreeGrid.VirtualTreeInPlaceControls;
#endif //VISUALSTUDIO_9_0

namespace Neumont.Tools.ORM.Shell
{
	/// <summary>
	/// Valid Commands for context menu
	/// </summary>
	[Flags]
	public enum ReadingEditorCommands
	{
		/// <summary>
		/// Commands not set
		/// </summary>
		None = 0,
		/// <summary>
		///
		/// </summary>
		AddReading = 1,
		/// <summary>
		/// 
		/// </summary>
		AddReadingOrder = 2,
		/// <summary>
		/// 
		/// </summary>
		DeleteReading = 4,
		/// <summary>
		/// 
		/// </summary>
		PromoteReading = 8,
		/// <summary>
		///
		/// </summary>
		DemoteReading = 0x10,
		/// <summary>
		/// 
		/// </summary>
		PromoteReadingOrder = 0x20,
		/// <summary>
		/// 
		/// </summary>
		DemoteReadingOrder = 0x40
	}

	#region ActiveFactType structure
	/// <summary>
	/// A structure to represent the currently active fact. Allows for
	/// a custom display order, such as the custom order available from
	/// a FactTypeShape.
	/// </summary>
	public struct ActiveFactType : IEquatable<ActiveFactType>
	{
		/// <summary>
		/// Represents an empty ActiveFactType
		/// </summary>
		public static readonly ActiveFactType Empty = new ActiveFactType();
		/// <summary>
		/// Is the structure equivalent to ActiveFactType.Empty?
		/// </summary>
		public bool IsEmpty
		{
			get
			{
				return myFactType == null;
			}
		}
		private readonly FactType myFactType;
		private readonly FactType myImpliedFactType;
		private readonly IList<RoleBase> myDisplayOrder;
		/// <summary>
		/// Set the active fact type to just use the native role order for its display order
		/// </summary>
		/// <param name="factType">A fact type. Can be null.</param>
		public ActiveFactType(FactType factType) : this(factType, null, null) { }
		/// <summary>
		/// Set the active fact type to just use the native role order for its display order
		/// </summary>
		/// <param name="factType">A fact type. Can be null.</param>
		/// <param name="displayOrder">A custom order representing
		/// the display order for a graphical representation of the fact</param>
		public ActiveFactType(FactType factType, IList<RoleBase> displayOrder) : this(factType, null, displayOrder) { }
		/// <summary>
		/// Set the active fact type and an implied fact to just use the native role order for its display order
		/// </summary>
		/// <param name="factType">A fact type. Can be null.</param>
		/// <param name="impliedFactType">A fact type implied by the main fact type. Can be null;</param>
		public ActiveFactType(FactType factType, FactType impliedFactType) : this(factType, impliedFactType, null) { }
		/// <summary>
		/// Set the active fact type to just use the native role order for its display order
		/// </summary>
		/// <param name="factType">A fact type. Can be null.</param>
		/// <param name="impliedFactType">A fact type implied by the main fact type. Can be null;</param>
		/// <param name="displayOrder">A custom order representing
		/// the display order for a graphical representation of the fact</param>
		public ActiveFactType(FactType factType, FactType impliedFactType, IList<RoleBase> displayOrder)
		{
			myFactType = factType;
			if (factType != null)
			{
				myImpliedFactType = impliedFactType;
				myDisplayOrder = (displayOrder != null) ? displayOrder : GetReadingRoleCollection(factType, true);
			}
			else
			{
				myImpliedFactType = null;
				myDisplayOrder = null;
			}
		}
		/// <summary>
		/// Get the current FactType
		/// </summary>
		public FactType FactType
		{
			get
			{
				return myFactType;
			}
		}
		/// <summary>
		/// Get the current implied fact type
		/// </summary>
		public FactType ImpliedFactType
		{
			get
			{
				return myImpliedFactType;
			}
		}
		/// <summary>
		/// Get the current display order
		/// </summary>
		public IList<RoleBase> DisplayOrder
		{
			get
			{
				return myDisplayOrder;
			}
		}
		/// <summary>
		/// Equals operator override
		/// </summary>
		public static bool operator ==(ActiveFactType fact1, ActiveFactType fact2)
		{
			return fact1.Equals(fact2);
		}
		/// <summary>
		/// Not equals operator override
		/// </summary>
		public static bool operator !=(ActiveFactType fact1, ActiveFactType fact2)
		{
			return !(fact1.Equals(fact2));
		}
		/// <summary>
		/// Standard Equals override
		/// </summary>
		public override bool Equals(object obj)
		{
			return (obj is ActiveFactType) && Equals((ActiveFactType)obj);
		}
		/// <summary>
		/// Typed Equals method
		/// </summary>
		public bool Equals(ActiveFactType obj)
		{
			return IsEmpty ? obj.IsEmpty :
				(myFactType == obj.myFactType &&
				myImpliedFactType == obj.myImpliedFactType &&
				AreDisplayOrdersEqual(myDisplayOrder, obj.myDisplayOrder));
		}
		private static bool AreDisplayOrdersEqual(IList<RoleBase> order1, IList<RoleBase> order2)
		{
			if (order1 == order2)
			{
				return true;
			}
			bool retVal = false;
			int count = order1.Count;
			if (count != 0 && count == order2.Count)
			{
				retVal = true;
				for (int i = 0; i < count; ++i)
				{
					if (order1[i] != order2[i])
					{
						retVal = false;
						break;
					}
				}
			}
			return retVal;
		}
		/// <summary>
		/// Standard override
		/// </summary>
		public override int GetHashCode()
		{
			FactType fact = myFactType;
			int hashCode = 0;
			if (fact != null)
			{
				hashCode = fact.GetHashCode();
				IList<RoleBase> order = myDisplayOrder;
				int count = order.Count;
				for (int i = 0; i < count; ++i)
				{
					hashCode ^= Utility.RotateRight(order[i].GetHashCode(), i);
				}
				fact = myImpliedFactType;
				if (fact != null)
				{
					hashCode ^= Utility.RotateRight(fact.GetHashCode(), (count == 0) ? 1 : count);
				}
			}
			return hashCode;
		}
		#region Reading role order collection helper
		private static IList<RoleBase> GetReadingRoleCollection(FactType factType)
		{
			return GetReadingRoleCollection(factType, false);
		}
		private static IList<RoleBase> GetReadingRoleCollection(FactType factType, bool snapshot)
		{
			// Return a single-element collection for binarized unaries
			Role unaryRole = factType.UnaryRole;
			if (unaryRole != null)
			{
				return new RoleBase[] { unaryRole };
			}
			else if (snapshot)
			{
				return factType.RoleCollection.ToArray();
			}
			return factType.RoleCollection;
		}
		#endregion
	}
	#endregion // ActiveFactType structure

	public partial class ReadingEditor : UserControl
	{
		#region InplaceReadingEditor control
		private sealed class InplaceReadingEditor : ReadingRichTextBox, IVirtualTreeInPlaceControl
		{
			#region NativeMethods class
			[System.Security.SuppressUnmanagedCodeSecurity]
			private sealed class NativeMethods
			{
				[StructLayout(LayoutKind.Sequential)]
				public struct RECT
				{
					public int left;
					public int top;
					public int right;
					public int bottom;
					public int width
					{
						get
						{
							return right - left;
						}
					}

					public int height
					{
						get
						{
							return bottom - top;
						}
					}
				}
				[DllImport("user32.dll", CharSet = CharSet.Auto)]
				public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, out RECT rect);
				public const int EM_GETRECT = 0xB2;
				public const int WM_MOUSEWHEEL = 0x20A;
				public const int ES_AUTOVSCROLL = 0x0040;
				public const int ES_AUTOHSCROLL = 0x0080;
			}
			#endregion // NativeMethods class
			#region Member variables
			private readonly VirtualTreeInPlaceControlHelper myInPlaceHelper;
			private bool myIgnoreNextSetText;
			private bool myEscapePressed;
			#endregion // Member variables
			#region Constructors
			/// <summary>
			/// Create a new InplaceReadingEditor
			/// </summary>
			public InplaceReadingEditor()
			{
				BorderStyle = BorderStyle.FixedSingle;
				myInPlaceHelper = VirtualTreeControl.CreateInPlaceControlHelper(this);
			}
			#endregion // Constructors
			#region InplaceReadingEditor Specific
			/// <summary>
			/// Provide additional functionality when the Initialize method is complete.
			/// Used to ignore the Text = labelText call coming from VirtualTreeControl.CreateEditInplaceWindow
			/// </summary>
			protected override void InitializeCompleted()
			{
				myIgnoreNextSetText = true;
			}
			/// <summary>
			/// Override text property to enable control to ignore Text = labelText call
			/// coming from VirtualTreeControl.CreateEditInplaceWindow
			/// </summary>
			public override string Text
			{
				get
				{
					return base.Text;
				}
				set
				{
					if (myIgnoreNextSetText)
					{
						myIgnoreNextSetText = false;
						return;
					}
					base.Text = value;
				}
			}
			/// <summary>
			/// Was the escape key pressed?
			/// </summary>
			public bool EscapePressed
			{
				get
				{
					return myEscapePressed;
				}
			}
			#endregion // InplaceReadingEditor Specific
			#region Event Forwarding, etc (Copied from VirtualTreeGrid.VirtualTreeInPlaceEditControl)
			protected override bool IsInputKey(Keys keyData)
			{
				if ((keyData & Keys.KeyCode) == Keys.Escape)
				{
					myEscapePressed = true;
				}
				return base.IsInputKey(keyData);
			}
			protected override void OnKeyDown(KeyEventArgs e)
			{
				base.OnKeyDown(e);
				if (!e.Handled)
				{
					e.Handled = this.myInPlaceHelper.OnKeyDown(e);
				}
			}
			protected override void OnKeyPress(KeyPressEventArgs e)
			{
				base.OnKeyPress(e);
				if (!e.Handled)
				{
					e.Handled = this.myInPlaceHelper.OnKeyPress(e);
				}
			}
			protected override void OnLostFocus(EventArgs e)
			{
				this.myInPlaceHelper.OnLostFocus();
				base.OnLostFocus(e);
			}
			protected override void OnTextChanged(EventArgs e)
			{
				this.myInPlaceHelper.OnTextChanged();
				base.OnTextChanged(e);
			}
			protected override CreateParams CreateParams
			{
				[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode), SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					CreateParams retVal = base.CreateParams;
					retVal.Style |= NativeMethods.ES_AUTOHSCROLL;
					retVal.Style &= ~NativeMethods.ES_AUTOVSCROLL;
					return retVal;
				}
			}
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode), SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			protected override void WndProc(ref Message m)
			{
				try
				{
					if (m.Msg == NativeMethods.WM_MOUSEWHEEL)
					{
						this.DefWndProc(ref m);
					}
					else
					{
						base.WndProc(ref m);
					}
				}
				catch (Exception ex)
				{
					if (Utility.IsCriticalException(ex))
					{
						throw;
					}
					myInPlaceHelper.DisplayException(ex);
				}
			}
			#endregion // Event Forwarding, etc (Copied from VirtualTreeGrid.VirtualTreeInPlaceEditControl)
			#region IVirtualTreeInPlaceControlDefer Implementation
			bool IVirtualTreeInPlaceControlDefer.Dirty
			{
				get
				{
					return myInPlaceHelper.Dirty;
				}
				set
				{
					myInPlaceHelper.Dirty = value;
				}
			}
			VirtualTreeInPlaceControlFlags IVirtualTreeInPlaceControlDefer.Flags
			{
				get
				{
					return myInPlaceHelper.Flags;
				}
				set
				{
					myInPlaceHelper.Flags = value;
				}
			}
			Control IVirtualTreeInPlaceControlDefer.InPlaceControl
			{
				get
				{
					return myInPlaceHelper.InPlaceControl;
				}
			}
			int IVirtualTreeInPlaceControlDefer.LaunchedByMessage
			{
				get
				{
					return myInPlaceHelper.LaunchedByMessage;
				}
				set
				{
					myInPlaceHelper.LaunchedByMessage = value;
				}
			}
			VirtualTreeControl IVirtualTreeInPlaceControlDefer.Parent
			{
				get
				{
					return myInPlaceHelper.Parent;
				}
				set
				{
					myInPlaceHelper.Parent = value;
				}
			}
			#endregion // IVirtualTreeInPlaceControlDefer Implementation
			#region IVirtualTreeInPlaceControl Implementation
			int IVirtualTreeInPlaceControl.ExtraEditWidth
			{
				get
				{
					return VirtualTreeInPlaceControlHelper.DefaultExtraEditWidth;
				}
			}
			Rectangle IVirtualTreeInPlaceControl.FormattingRectangle
			{
				get
				{
					NativeMethods.RECT rect;
					NativeMethods.SendMessage(new HandleRef(this, Handle), NativeMethods.EM_GETRECT, 0, out rect);
					return new Rectangle(rect.left, rect.top, rect.width, rect.height);
				}
			}
			int IVirtualTreeInPlaceControl.MaxTextLength
			{
				get
				{
					return MaxLength;
				}
				set
				{
					MaxLength = value;
				}
			}
			void IVirtualTreeInPlaceControl.SelectAllText()
			{
				// Don't acknowledge this request. This is used for initial selection, but
				// we already provide an initial selection during the Initialize method.
			}

			int IVirtualTreeInPlaceControl.SelectionStart
			{
				get
				{
					return SelectionStart;
				}
				set
				{
					// Don't acknowledge this request. This is used for initial selection, but
					// we already provide an initial selection during the Initialize method.
				}
			}
			#endregion // IVirtualTreeInPlaceControl Implementation
		}
		#endregion // Inplace reading editor control
		#region OrderBranch Interface
		private interface IReadingEditorBranch : IBranch, IMultiColumnBranch
		{
			void AddNewReading(int itemLocation);
			void AddNewReadingOrder();
			void DemoteSelectedReading(int readingItemLocation, int orderItemLocation);
			void DemoteSelectedReadingOrder(int itemLocation);
			void EditReadingOrder(IList<RoleBase> collection);
			bool IsAdding { get; }
			void PromoteSelectedReading(int readingItemLocation, int orderItemLocation);
			void PromoteSelectedReadingOrder(int itemLocation);
			void ReadingAdded(Reading reading);
			void ReadingLocationUpdate(Reading reading);
			void ReadingOrderLocationUpdate(ReadingOrder order);
			void ReadingOrderRemoved(ReadingOrder order, FactType fact);
			void ReadingRemoved(Reading reading, ReadingOrder order);
			void RemoveSelectedItem();
			ReadingEditorCommands SupportedSelectionCommands(int itemLocation);
			void UpdateReading(Reading reading);
		}
		#endregion //OrderBranch Interface

		#region Enums
		private enum ColumnIndex
		{
			ReadingOrder = 0,
			ReadingBranch = 1,
		}
		private enum RowType
		{
			Committed = 0,
			Uncommitted = 1,
			TypeEditorHost = 3,
			None = 4
		}
		#endregion // Enums

		#region Static Properties
		/// <summary>
		/// Returns the latest instance of the editor
		/// </summary>
		public static ReadingEditor Instance
		{
			get
			{
				return ReadingEditor.myInstance;
			}
		}
		#endregion // Static Methods

		#region Static Variables
		/// <summary>
		/// Provides a ref to the ReadingOrderBranch from nested objects
		/// </summary>
		private IReadingEditorBranch myMainBranch;
		/// <summary>
		/// Provides a ref to the Reading Editorl from nested objects
		/// </summary>
		private static ReadingEditor myInstance;
		#endregion // Static Variables
		#region Member Variables
		private ORMReadingEditorToolWindow myToolWindow;
		#endregion // Member Variables

		#region Member Variables
		private FactType myFact;
		private FactType mySecondaryFact;
		private IList<RoleBase> myDisplayRoleOrder;
		private ImageList myImageList;
		private ReadingEditorCommands myVisibleCommands;
		private bool myInEvents;
		#endregion // Member Variables

		#region Construction
		/// <summary>
		/// Default constructor.
		/// </summary>
		[CLSCompliant(false)]
		public ReadingEditor(ORMReadingEditorToolWindow toolWindow)
		{
			myToolWindow = toolWindow;
			Debug.Assert(myInstance == null, "ORMReadingEditorToolWindow should be a singleton");
			myInstance = this;
			InitializeComponent();
			vtrReadings.LabelEditControlChanged += Tree_LabelEditControlChanged;
		}
		private void SetHeaders(bool wideHeader)
		{
			CustomVirtualTreeControl treeControl = this.vtrReadings;
			int indent = treeControl.IndentWidth + 3;

			if (wideHeader)
			{
				indent += indent;
			}

			VirtualTreeColumnHeader[] headers = new VirtualTreeColumnHeader[ReadingOrderBranch.COLUMN_COUNT]
			  {
				  new VirtualTreeColumnHeader(" ", indent + treeControl.ImageList.ImageSize.Width, false, VirtualTreeColumnHeaderStyles.Default), 	
				  new VirtualTreeColumnHeader(" ", 1f, 100, VirtualTreeColumnHeaderStyles.Default)
			  };
			treeControl.SetColumnHeaders(headers, false);
		}
		#endregion //construction

		#region Properties
		/// <summary>
		/// The fact that is being edited in the control, or that needs to be edited.
		/// </summary>
		public ActiveFactType EditingFactType
		{
			get
			{
				return new ActiveFactType(myFact, mySecondaryFact, myDisplayRoleOrder);
			}
			set
			{
				myFact = value.FactType;
				mySecondaryFact = value.ImpliedFactType;
				myDisplayRoleOrder = value.DisplayOrder;
				if (myFact != null)
				{
					PopulateControl();
				}
			}
		}
		/// <summary>
		/// The role order we're using to display the current fact.
		/// </summary>
		public IList<RoleBase> DisplayRoleOrder
		{
			get
			{
				return myDisplayRoleOrder;
			}
		}
		/// <summary>
		/// The reading that is being edited in the control, or that needs to be edited.
		/// </summary>
		public object CurrentReading
		{
			get
			{
				object retVal = null;
				ITree tree = vtrReadings.Tree;
				int currentIndex = vtrReadings.CurrentIndex;
				if (currentIndex >= 0)
				{
					VirtualTreeItemInfo itemInfo = vtrReadings.Tree.GetItemInfo(currentIndex, vtrReadings.CurrentColumn, false);
					int input = 0;
					retVal = itemInfo.Branch.GetObject(itemInfo.Row, itemInfo.Column, ObjectStyle.TrackingObject, ref input);
					return (retVal == null) ? null : retVal;
				}
				return null;
			}
		}
		/// <summary>
		/// Returns true if a reading is currently in edit mode.
		/// </summary>
		public bool InLabelEdit
		{
			get
			{
				return vtrReadings.InLabelEdit;
			}
		}
		/// <summary>
		/// Returns the active label edit control, or null
		/// </summary>
		public Control LabelEditControl
		{
			get
			{
				return vtrReadings.LabelEditControl;
			}
		}
		/// <summary>
		/// Returns true if the reading pane is active
		/// </summary>
		public bool IsReadingPaneActive
		{
			get
			{
				if (vtrReadings != null)
				{
					ContainerControl sc = this.ActiveControl as ContainerControl;
					if (sc != null)
					{
						return vtrReadings == sc.ActiveControl;
					}
				}
				return false;
			}
		}

		#endregion //Properties
		#region Tree Events
		private void Tree_LabelEditControlChanged(object sender, EventArgs e)
		{
			myToolWindow.ActiveInPlaceEditWindow = vtrReadings.LabelEditControl;
		}
		#endregion // Tree Events

		#region PopulateControl and helpers
		private void PopulateControl()
		{
			Debug.Assert(myFact != null);
			if (myDisplayRoleOrder != null)
			{
				LinkedElementCollection<RoleBase> roles = myFact.RoleCollection;
				if ((FactType.GetUnaryRoleIndex(roles).HasValue ? 1 : roles.Count) != myDisplayRoleOrder.Count)
				{
					myDisplayRoleOrder = null;
				}
			}

			ReadingVirtualTree rvt = null;

			if (mySecondaryFact != null)
			{
				this.SetHeaders(true);
				rvt = new ReadingVirtualTree(myMainBranch = new FactTypeBranch(myFact, mySecondaryFact));
			}
			else
			{
				this.SetHeaders(false);
				rvt = new ReadingVirtualTree(myMainBranch = new ReadingOrderBranch(myFact, myDisplayRoleOrder));
			}

			VirtualTreeControl control = this.vtrReadings;

			ITree oldTree = vtrReadings.MultiColumnTree as ITree;
			// Turn off all event handlers for old branches whenever we repopulate
			bool turnedDelayRedrawOff = false;
			if (oldTree != null)
			{
				oldTree.Root = null;
				if (myInEvents)
				{
					turnedDelayRedrawOff = true;
					this.vtrReadings.Tree.DelayRedraw = false;
				}
			}
			this.vtrReadings.MultiColumnTree = rvt;
			if (turnedDelayRedrawOff)
			{
				this.vtrReadings.Tree.DelayRedraw = true;
			}

			if (this.vtrReadings.Tree.IsExpandable(0, 0))
			{
				this.vtrReadings.Tree.ToggleExpansion(0, 0); //expand ReadingOrderBranch by default
			}

		}
		/// <summary>
		/// Provides a ref to the tree control from nested objects
		/// </summary>
		private CustomVirtualTreeControl TreeControl
		{
			get
			{
				return vtrReadings;
			}
		}
		#endregion //PopulateControl and helpers
		#region Reading role order collection helper
		private static IList<RoleBase> GetReadingRoleCollection(FactType fact)
		{
			// Return a single-element collection for binarized unaries
			Role unaryRole = fact.UnaryRole;
			if (unaryRole != null)
			{
				return new RoleBase[] { unaryRole };
			}
			else
			{
				return fact.RoleCollection;
			}
		}
		#endregion // Reading role order collection helper
		#region Reading activation helper
		/// <summary>
		/// Select the current reading in the window. The
		/// reading must be the child of the current fact.
		/// </summary>
		/// <param name="reading">Reading</param>
		public void ActivateReading(Reading reading)
		{
			ReadingOrder order;
			FactType factType;
			if (null != (order = reading.ReadingOrder)
				&& null != (factType = order.FactType)
				&& (factType == myFact || factType == mySecondaryFact))
			{
				if (TreeControl.SelectObject(myMainBranch, reading, (int)ObjectStyle.TrackingObject, 0))
				{
					TreeControl.BeginLabelEdit();
				}
			}
		}

		/// <summary>
		/// Select the primary reading of the reading order
		/// matching the fact's role display order if one exists,
		/// if not selects the new entry for the role
		/// sequence matching the facts role display order.
		/// </summary>
		/// <param name="fact">FactType</param>
		public void ActivateReading(FactType fact)
		{
			if (fact == myFact || fact == mySecondaryFact)
			{
				myMainBranch.EditReadingOrder(GetReadingRoleCollection(fact));
			}
		}
		/// <summary>
		/// Puts the reading that is currently selected in the reading order into edit mode.
		/// </summary>
		public void EditSelectedReading()
		{
			using (Transaction t = myFact.Store.TransactionManager.BeginTransaction(ResourceStrings.CommandEditReadingText))
			{
				vtrReadings.BeginLabelEdit();
			}
		}

		#endregion // Reading activation helper

		#region Tree Context Menu Methods
		private void OnMenuInvoked(object sender, ContextMenuEventArgs e)
		{
			myToolWindow.MenuService.ShowContextMenu(ORMDesignerDocView.ORMDesignerCommandIds.ReadingEditorContextMenu, e.X, e.Y);
		}
		private void UpdateMenuItems()
		{
			if (TreeControl.CurrentIndex != -1) //make sure somthing is selected
			{
				VirtualTreeItemInfo itemInfo;
				if (this.GetItemInfo(out itemInfo, ColumnIndex.ReadingOrder))
				{
					myVisibleCommands = (itemInfo.Branch as IReadingEditorBranch).SupportedSelectionCommands(itemInfo.Row);
				}
			}
			else
			{
				myVisibleCommands = ReadingEditorCommands.None;
			}
		}
		/// <summary>
		/// Event for selection changed
		/// </summary>
		/// <param name="sender">The VirtualTreeControl</param>
		/// <param name="e">EventArgs</param>
		private void OnTreeControlSelectionChanged(object sender, EventArgs e)
		{
			this.UpdateMenuItems();
		}
		/// <summary>
		/// Check the current status of the requested command. 
		/// </summary>
		/// <param name="sender">MenuCommand</param>
		/// <param name="commandFlag">The command to check for enabled</param>
		public static void OnStatusCommand(object sender, ReadingEditorCommands commandFlag)
		{
			MenuCommand command = sender as MenuCommand;
			Debug.Assert(command != null);
			command.Enabled = command.Visible = 0 != (commandFlag & ReadingEditor.myInstance.myVisibleCommands);
		}
		/// <summary>
		/// Call the Drop Down list of Reading Orders to Select a new reading entry
		/// </summary>
		public void OnMenuAddReading()
		{
			VirtualTreeItemInfo itemInfo;
			if (this.GetItemInfo(out itemInfo, ColumnIndex.ReadingOrder))
			{
				(itemInfo.Branch as IReadingEditorBranch).AddNewReading(itemInfo.Row);
			}
		}
		/// <summary>
		/// Call the Drop Down list of Reading Orders to Select a new readingOrder entry
		/// </summary>
		public void OnMenuAddReadingOrder()
		{
			VirtualTreeItemInfo itemInfo;
			if (this.GetItemInfo(out itemInfo, ColumnIndex.ReadingOrder))
			{
				(itemInfo.Branch as IReadingEditorBranch).AddNewReadingOrder();
			}
		}
		/// <summary>
		/// Moves the Reading up within the ReadingOrder Collection
		/// </summary>
		public void OnMenuPromoteReading()
		{
			//Get the ReadingItemRow (must use directly as class is nested within the ReadingOrderBranch
			VirtualTreeItemInfo itemInfo = TreeControl.Tree.GetItemInfo(TreeControl.CurrentIndex, (int)ColumnIndex.ReadingBranch, true);
			int readingItemRow = itemInfo.Row;
			if (readingItemRow != -1 && this.GetItemInfo(out itemInfo, ColumnIndex.ReadingOrder)) //Get the ReadingOrder and pass it the Reading to Promote
			{
				(itemInfo.Branch as IReadingEditorBranch).PromoteSelectedReading(readingItemRow, itemInfo.Row);
			}
		}
		/// <summary>
		/// Moves the Reading down within the ReadingOrder Collection
		/// </summary>
		public void OnMenuDemoteReading()
		{
			//Get the ReadingItemRow (must use directly as class is nested within the ReadingOrderBranch
			VirtualTreeItemInfo itemInfo = TreeControl.Tree.GetItemInfo(TreeControl.CurrentIndex, (int)ColumnIndex.ReadingBranch, true);
			int readingItemRow = itemInfo.Row;
			if (readingItemRow != -1 && this.GetItemInfo(out itemInfo, ColumnIndex.ReadingOrder)) //Get the ReadingOrder and pass it the Reading to Promote
			{
				(itemInfo.Branch as IReadingEditorBranch).DemoteSelectedReading(readingItemRow, itemInfo.Row);
			}
		}
		/// <summary>
		/// Moves the ReadingOrder up within the ReadingOrder Collection
		/// </summary>
		public void OnMennuPromoteReadingOrder()
		{
			VirtualTreeItemInfo itemInfo;
			if (this.GetItemInfo(out itemInfo, ColumnIndex.ReadingOrder))
			{
				(itemInfo.Branch as IReadingEditorBranch).PromoteSelectedReadingOrder(itemInfo.Row);
			}
		}
		/// <summary>
		/// Moves the ReadingOrder down within the ReadingOrder Collection
		/// </summary>
		public void OnMenuDemoteReadingOrder()
		{
			VirtualTreeItemInfo itemInfo;
			if (this.GetItemInfo(out itemInfo, ColumnIndex.ReadingOrder))
			{
				(itemInfo.Branch as IReadingEditorBranch).DemoteSelectedReadingOrder(itemInfo.Row);
			}
		}
		/// <summary>
		/// Deletes the reading that is currently selected in the reading order
		/// </summary>
		public void OnMenuDeleteSelectedReading()
		{
			VirtualTreeItemInfo itemInfo;
			if (this.GetItemInfo(out itemInfo, ColumnIndex.ReadingOrder))
			{
				(itemInfo.Branch as IReadingEditorBranch).RemoveSelectedItem();
			}
		}

		/// <summary>
		/// used to get the VirtualTreeItemInfo object for the current selection 
		/// </summary>
		/// <param name="itemInfo">VitrualTreeItemInfo out</param>
		/// <param name="column">Column to generate the itemInfo for (always uses the CurrentIndex of the tree control)</param>
		/// <returns>true if the itemInfo.branch is a top level branch</returns>
		private bool GetItemInfo(out VirtualTreeItemInfo itemInfo, ColumnIndex column)
		{
			itemInfo = TreeControl.Tree.GetItemInfo(TreeControl.CurrentIndex, (int)column, true);
			if (itemInfo.Branch.GetType() == typeof(ReadingOrderBranch) || itemInfo.Branch.GetType() == typeof(FactTypeBranch))
			{
				return true;
			}
			return false;
		}
		#endregion  //Tree Context Menu Methods

		#region model events and handlers
		#region Nested event handler attach/detach methods
		/// <summary>
		/// Manages <see cref="EventHandler{TEventArgs}"/>s in the <see cref="Store"/> so that the <see cref="ORMReadingEditorToolWindow"/>
		/// contents can be updated to reflect any model changes.
		/// </summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/>s should be managed.</param>
		/// <param name="eventManager">The <see cref="ModelingEventManager"/> used to manage the <see cref="EventHandler{TEventArgs}"/>s.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>s.</param>
		public void ManageEventHandlers(Store store, ModelingEventManager eventManager, EventHandlerAction action)
		{
			if (store == null || store.Disposed)
			{
				return; // Bail out
			}
			DomainDataDirectory dataDirectory = store.DomainDataDirectory;
			DomainClassInfo classInfo = dataDirectory.FindDomainRelationship(ReadingOrderHasReading.DomainClassId);

			// Track Reading changes
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(ReadingLinkAddedEvent), action);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(ReadingLinkRemovedEvent), action);

			classInfo = dataDirectory.FindDomainClass(Reading.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementPropertyChangedEventArgs>(ReadingAttributeChangedEvent), action);

			// Track ReadingOrder changes
			classInfo = dataDirectory.FindDomainRelationship(FactTypeHasReadingOrder.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(ReadingOrderLinkRemovedEvent), action);

			//Track FactType RoleOrder changes
			classInfo = dataDirectory.FindDomainRelationship(FactTypeHasRole.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(FactTypeHasRoleAddedOrDeletedEvent), action);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(FactTypeHasRoleAddedOrDeletedEvent), action);

			// Track fact type removal
			classInfo = dataDirectory.FindDomainRelationship(ModelHasFactType.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(FactTypeRemovedEvent), action);

			//Track Order Change
			classInfo = dataDirectory.FindDomainRelationship(FactTypeHasReadingOrder.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<RolePlayerOrderChangedEventArgs>(ReadingOrderPositionChangedHandler), action);
			classInfo = dataDirectory.FindDomainRelationship(ReadingOrderHasReading.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<RolePlayerOrderChangedEventArgs>(ReadingPositionChangedHandler), action);

			//Track Currently Executing Events
			eventManager.AddOrRemoveHandler(new EventHandler<ElementEventsBegunEventArgs>(ElementEventsBegunEvent), action);
			eventManager.AddOrRemoveHandler(new EventHandler<ElementEventsEndedEventArgs>(ElementEventsEndedEvent), action);
		}

		#endregion //Nested event handler attach/detach methods

		#region Pre/Post Event routines
		private void ElementEventsBegunEvent(object sender, ElementEventsBegunEventArgs e)
		{
			myInEvents = false; // Sanity, should not be needed
			if (myFact != null)
			{
				myInEvents = true;
				ITree currentTree = this.vtrReadings.Tree;
				if (currentTree != null)
				{
					currentTree.DelayRedraw = true;
				}
			}
		}

		private void ElementEventsEndedEvent(object sender, ElementEventsEndedEventArgs e)
		{
			if (myInEvents)
			{
				myInEvents = false;
				ITree currentTree = this.vtrReadings.Tree;
				if (currentTree != null)
				{
					currentTree.DelayRedraw = false;
				}
			}
		}
		#endregion //Pre/Post Event routines

		#region Reading Event Handlers
		//handling model events Related to changes in Readings and their
		//connections so the reading editor can accurately reflect the model
		private void ReadingLinkAddedEvent(object sender, ElementAddedEventArgs e)
		{
			if (myFact == null)
			{
				return;
			}
			ReadingOrderHasReading link = e.ModelElement as ReadingOrderHasReading;
			if (!link.IsDeleted)
			{
				ReadingOrder readingOrder = link.ReadingOrder;
				FactType fact = readingOrder.FactType;
				if (fact == myFact || fact == mySecondaryFact)
				{
					myMainBranch.ReadingAdded(link.Reading);
				}
				this.UpdateMenuItems();
			}
		}
		private void ReadingLinkRemovedEvent(object sender, ElementDeletedEventArgs e)
		{
			if (myFact == null)
			{
				return;
			}
			ReadingOrderHasReading link = e.ModelElement as ReadingOrderHasReading;
			ReadingOrder order = link.ReadingOrder;
			// Handled all at once by ReadingOrderLinkRemovedEvent if all are gone.
			if (!order.IsDeleted)
			{
				if (order.FactType == myFact || order.FactType == mySecondaryFact)
				{
					myMainBranch.ReadingRemoved(link.Reading, order); //UNDONE: use interface and locate object
				}
			}
		}
		private void ReadingAttributeChangedEvent(object sender, ElementPropertyChangedEventArgs e)
		{
			if (myFact == null)
			{
				return;
			}
			Reading reading = e.ModelElement as Reading;
			ReadingOrder order = reading.ReadingOrder;
			Guid attributeId = e.DomainProperty.Id;
			if (attributeId == Reading.TextDomainPropertyId
				&& !reading.IsDeleted
				&& null != (order = reading.ReadingOrder)
				&& (order.FactType == myFact || order.FactType == mySecondaryFact))
			{
				myMainBranch.UpdateReading(reading);
			}
		}
		private void ReadingPositionChangedHandler(object sender, RolePlayerOrderChangedEventArgs e)
		{
			if (myFact == null)
			{
				return;
			}
			myMainBranch.ReadingLocationUpdate(e.CounterpartRolePlayer as Reading);
			this.UpdateMenuItems();
		}
		#endregion //Reading Event Handlers

		#region ReadingOrder Event Handlers
		//handle model events related to the ReadingOrder or its Roles being removed in order to
		//keep the editor window in sync with what is in the model.
		private void ReadingOrderLinkRemovedEvent(object sender, ElementDeletedEventArgs e)
		{
			FactTypeHasReadingOrder link = e.ModelElement as FactTypeHasReadingOrder;
			FactType factType = link.FactType;

			if ((factType == myFact || factType == mySecondaryFact) && !factType.IsDeleting && !factType.IsDeleted)
			{
				//test to implelement thru the interface
				//VirtualTreeCoordinate coords = ReadingEditor.TreeControl.Tree.LocateObject(myMainBranch, factType, (int)ObjectStyle.TrackingObject, 0);
				//VirtualTreeItemInfo info = ReadingEditor.TreeControl.Tree.GetItemInfo(coords.Row, coords.Column, false);
				//(info.Branch as IReadingEditorBranch).ReadingOrderRemoved(link.ReadingOrder, factType);
				myMainBranch.ReadingOrderRemoved(link.ReadingOrder, factType);
			}
		}

		private void ReadingOrderPositionChangedHandler(object sender, RolePlayerOrderChangedEventArgs e)
		{
			if (myFact == null)
			{
				return;
			}
			myMainBranch.ReadingOrderLocationUpdate(e.CounterpartRolePlayer as ReadingOrder);
			this.UpdateMenuItems();
		}
		#endregion

		#region FactType Event Handlers
		private void FactTypeHasRoleAddedOrDeletedEvent(object sender, ElementEventArgs e)
		{
			if (myFact != null && myFact == ((FactTypeHasRole)e.ModelElement).FactType && !myFact.IsDeleted && !myFact.IsDeleting)
			{
				this.PopulateControl();
			}
		}
		private void FactTypeRemovedEvent(object sender, ElementDeletedEventArgs e)
		{
			ModelHasFactType link = e.ModelElement as ModelHasFactType;
			if (link.FactType == myFact)
			{
				ORMDesignerPackage.ReadingEditorWindow.EditingFactType = ActiveFactType.Empty;
			}

			if (myFact != null && myFact == link.FactType && !myFact.IsDeleted && !myFact.IsDeleting)
			{
				this.PopulateControl();
			}
			else if (myFact != null && myFact == link.FactType)
			{
				ITree currentTree = this.vtrReadings.Tree;
				if (currentTree != null)
				{
					currentTree.Root = null;
				}
			}
		}

		#endregion // FactType Event Handlers
		#endregion //model events and handlers

		#region nested class FactTypeBranch
		private sealed class FactTypeBranch : IReadingEditorBranch
		{
			private const int OrderBranchRow = 0;
			private const int ImpliedBranchRow = 1;

			private readonly FactType myFact;
			private readonly FactType mySecondaryFact;
			private ReadingOrderBranch myReadingOrderBranch;
			private ReadingOrderBranch myImpliedFactTypeBranch;

			#region Construction
			public FactTypeBranch(FactType fact, FactType secondaryFact)
			{
				myFact = fact;
				mySecondaryFact = secondaryFact;
			}
			#endregion //Construction

			#region IBranch Interface Members
			public VirtualTreeLabelEditData BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
			{
				return VirtualTreeLabelEditData.Default;
			}
			public LabelEditResult CommitLabelEdit(int row, int column, string newText)
			{
				return LabelEditResult.CancelEdit;
			}
			public BranchFeatures Features
			{
				get
				{
					return BranchFeatures.Expansions;
				}
			}
			public VirtualTreeAccessibilityData GetAccessibilityData(int row, int column)
			{
				return VirtualTreeAccessibilityData.Empty;
			}
			public VirtualTreeDisplayData GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
			{
				VirtualTreeDisplayData retval = VirtualTreeDisplayData.Empty;
				retval.ForeColor = Color.Gray;
				return retval;
			}
			public object GetObject(int row, int column, ObjectStyle style, ref int options)
			{
				if (row == OrderBranchRow)
				{
					return OrderBranch;
				}
				else
				{
					return ImpliedBranch;
				}
			}
			public string GetText(int row, int column)
			{
				return (row == OrderBranchRow) ? ResourceStrings.ModelReadingEditorPrimaryFactTypeReadingsText : ResourceStrings.ModelReadingEditorImpliedFactTypeReadingsText;
			}
			public string GetTipText(int row, int column, ToolTipType tipType)
			{
				return null;
			}
			public bool IsExpandable(int row, int column)
			{
				return true; // mySecondaryFact != null;
			}
			public LocateObjectData LocateObject(object obj, ObjectStyle style, int locateOptions)
			{
				switch (style)
				{
					case ObjectStyle.TrackingObject:
						Reading reading;
						ReadingOrder order;
						FactType fact = null;
						if (null != (reading = (obj as Reading)))
						{
							order = reading.ReadingOrder;
							fact = order.FactType;
						}
						else
						{
							fact = (obj as FactType);
						}

						if (fact == myFact)
						{
							return new LocateObjectData(OrderBranchRow, 1, (int)TrackingObjectAction.NextLevel);
						}
						else if (fact == mySecondaryFact)
						{
							return new LocateObjectData(ImpliedBranchRow, 1, (int)TrackingObjectAction.NextLevel);
						}
						break;
				}
				return new LocateObjectData();
			}
			event BranchModificationEventHandler IBranch.OnBranchModification
			{
				// Do nothing. We never raise this event, so we don't need to keep track of who is subscribed to it.
				add
				{
				}
				remove
				{
				}
			}
			public void OnDragEvent(object sender, int row, int column, DragEventType eventType, DragEventArgs args)
			{
			}
			public void OnGiveFeedback(GiveFeedbackEventArgs args, int row, int column)
			{
			}
			public void OnQueryContinueDrag(QueryContinueDragEventArgs args, int row, int column)
			{
			}
			public VirtualTreeStartDragData OnStartDrag(object sender, int row, int column, DragReason reason)
			{
				return VirtualTreeStartDragData.Empty;
			}
			public StateRefreshChanges ToggleState(int row, int column)
			{
				return StateRefreshChanges.None;
			}
			public StateRefreshChanges SynchronizeState(int row, int column, IBranch matchBranch, int matchRow, int matchColumn)
			{
				return StateRefreshChanges.None;
			}
			public int UpdateCounter
			{
				get
				{
					return 0;
				}
			}
			public int VisibleItemCount
			{
				get
				{
					return 2;
				}
			}
			#endregion

			#region Properties
			private ReadingOrderBranch OrderBranch
			{
				get
				{
					ReadingOrderBranch retVal = myReadingOrderBranch;
					if (retVal == null)
					{
						return myReadingOrderBranch = new ReadingOrderBranch(myFact, ReadingEditor.myInstance.myDisplayRoleOrder);
					}
					return retVal;
				}
			}

			private ReadingOrderBranch ImpliedBranch
			{
				get
				{
					ReadingOrderBranch retVal = myImpliedFactTypeBranch;
					if (retVal == null)
					{
						return myImpliedFactTypeBranch = new ReadingOrderBranch(mySecondaryFact, null);
					}
					return retVal;
				}
			}
			#endregion //Properties

			#region IReadingEditorBranch Interface Members
			#region Reading Methods
			/// <summary>
			/// Initiates a New Reading within the selected Reading Order
			/// </summary>
			public void AddNewReading(int itemLocaiton)
			{
			}
			/// <summary>
			/// Instruct the readingbranch that a reading has been added to the collection
			/// </summary>
			/// <param name="reading">the reading to add</param>
			public void ReadingAdded(Reading reading)
			{
				ReadingOrder order;
				FactType orderFactType;
				if (!reading.IsDeleted &&
					null != (order = reading.ReadingOrder) &&
					null != (orderFactType = order.FactType))
				{
					if (orderFactType == myFact)
					{
						this.OrderBranch.ReadingAdded(reading);
					}
					else
					{
						this.ImpliedBranch.ReadingAdded(reading);
					}
				}
			}
			/// <summary>
			/// Triggers the events notifying the tree that a Reading in the Readingbranch has been updated. 
			/// </summary>
			/// <param name="reading">The Reading affected</param>
			public void UpdateReading(Reading reading)
			{
				ReadingOrder order = reading.ReadingOrder;
				if (order.FactType == myFact)
				{
					this.OrderBranch.UpdateReading(reading);
				}
				else
				{
					this.ImpliedBranch.UpdateReading(reading);
				}
			}
			/// <summary>
			/// Event callback from Changing the Order of  and item in the ReadingOrdersMovableCollectoin: 
			/// will update the branch to reflect the changed order
			/// </summary>
			/// <param name="reading">The Reading moved</param>
			public void ReadingLocationUpdate(Reading reading)
			{
				ReadingOrder order = reading.ReadingOrder;
				if (order.FactType == myFact)
				{
					this.OrderBranch.ReadingLocationUpdate(reading);
				}
				else
				{
					this.ImpliedBranch.ReadingLocationUpdate(reading);
				}
			}
			/// <summary>
			/// Triggers notification that a Reading has been removed from the branch.
			/// </summary>
			/// <param name="reading">The Reading which has been removed</param>
			/// <param name="order">The order of the link</param>
			public void ReadingRemoved(Reading reading, ReadingOrder order)
			{
				if (order.FactType == myFact)
				{
					this.OrderBranch.ReadingRemoved(reading, order);
				}
				else
				{
					this.ImpliedBranch.ReadingRemoved(reading, order);
				}
			}
			/// <summary>
			/// Moves the selected Reading Up
			/// </summary>
			public void PromoteSelectedReading(int readingItemLocation, int orderItemLocation)
			{
				//not implemented
			}
			/// <summary>
			/// Moves the selected Reading Down
			/// </summary>
			public void DemoteSelectedReading(int readingItemLocation, int orderItemLocation)
			{
				//not implemented
			}
			#endregion //Reading Methods

			#region ReadingOrder Methods
			/// <summary>
			/// Used to find out if the branch is in the process of adding a new entry from input into the branch.
			/// </summary>
			public bool IsAdding
			{
				get
				{
					return (OrderBranch.IsAdding || ImpliedBranch.IsAdding);
				}
			}
			/// <summary>
			/// Initiates the Drop Down to select a new reading order for the reading to add
			/// </summary>
			public void AddNewReadingOrder()
			{
				//not implemented
			}
			/// <summary>
			/// Initiate edit for first reading within the <see cref="LinkedElementCollection{RoleBase}"/> (will create a new item if needed)
			/// </summary>
			/// <param name="collection"><see cref="LinkedElementCollection{RoleBase}"/> collection</param>
			public void EditReadingOrder(IList<RoleBase> collection)
			{
				this.OrderBranch.EditReadingOrder(collection); //only need to call on order branch as you cannot create a new order (as they already exist) for  an implied fact 
			}
			/// <summary>
			/// Moves the selected ReadingOrder up
			/// </summary>
			/// <param name="itemLocation">location</param>
			public void PromoteSelectedReadingOrder(int itemLocation)
			{
				//not implemented			
			}
			/// <summary>
			/// Moves the selected ReadingOrder Down
			/// </summary>
			/// <param name="itemLocation">location</param>
			public void DemoteSelectedReadingOrder(int itemLocation)
			{
				//not implemented
			}
			/// <summary>
			/// Removes the item selected when called by the context menu
			/// </summary>
			public void RemoveSelectedItem()
			{
				//not implemented
			}
			/// <summary>
			/// Event callback from Changing the Order of  and item in the ReadingOrdersMovableCollectoin: 
			/// will update the branch to reflect the changed order
			/// </summary>
			/// <param name="order">The ReadingOrder affected</param>
			public void ReadingOrderLocationUpdate(ReadingOrder order)
			{
				VirtualTreeControl control = ReadingEditor.Instance.TreeControl;
				((IReadingEditorBranch)control.Tree.GetItemInfo(control.CurrentIndex, 0, true).Branch).ReadingOrderLocationUpdate(order);
			}
			/// <summary>
			/// CallBack notification that a ReadingOrder has been removed from the branch.
			/// </summary>
			/// <param name="order">The Reading Order which has been removed</param>
			/// <param name="fact">The Fact to which the Reading Order belongs</param>
			public void ReadingOrderRemoved(ReadingOrder order, FactType fact)
			{
				if (fact == myFact)
				{
					this.OrderBranch.ReadingOrderRemoved(order, fact);
				}
				else
				{
					this.ImpliedBranch.ReadingOrderRemoved(order, fact);
				}
			}
			/// <summary>
			///  Get context menu commands supported for current selection
			/// </summary>
			/// <returns>ReadingEditorCommands bit field</returns>
			public ReadingEditorCommands SupportedSelectionCommands(int itemLocation)
			{
				//not implemented
				return ReadingEditorCommands.None;
			}
			#endregion //ReadingOrder Methods
			#endregion //IReadingEditorBranch Interface Members

			#region IMultiColumnBranch Interface Members

			public int ColumnCount
			{
				get
				{
					return 0;
				}
			}

			public SubItemCellStyles ColumnStyles(int column)
			{
				return SubItemCellStyles.Complex;
			}

			public int GetJaggedColumnCount(int row)
			{
				return 0;
			}

			#endregion
		}
		#endregion //nested class FactTypeBranch

		#region nested class ReadingOrderBranch
		private sealed class ReadingOrderBranch : IReadingEditorBranch
		{
			public const int COLUMN_COUNT = 2;
			private readonly FactType myFact;
			private readonly ReadingOrderKeyedCollection myReadingOrderKeyedCollection;
			private ReadingOrderKeyedCollection myReadingOrderPermutations;
			private readonly IList<RoleBase> myRoleDisplayOrder;
			private string[] myRoleNames;
			private int myInsertedRow = -1;

			#region Construction
			public ReadingOrderBranch(FactType fact, IList<RoleBase> roleDisplayOrder)
			{
				myFact = fact; //impliedbyobjectification
				myReadingOrderKeyedCollection = new ReadingOrderKeyedCollection();
				myRoleDisplayOrder = roleDisplayOrder != null && roleDisplayOrder.Count > 0 ? roleDisplayOrder : GetReadingRoleCollection(fact);
				this.PopulateReadingOrderInfo(-1); //Populate for all readings
			}
			#endregion

			#region Branch Properties
			private IList<RoleBase> DisplayOrder
			{
				get
				{
					return myRoleDisplayOrder;
				}
			}
			private FactType Fact
			{
				get
				{
					return myFact;
				}
			}
			#endregion //Branch Properties

			#region IBranch Interface Members
			public VirtualTreeLabelEditData BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
			{
				RowType rowType = myReadingOrderKeyedCollection.GetRowType(row);
				if (column == (int)ColumnIndex.ReadingBranch && rowType == RowType.TypeEditorHost)
				{
					if (myReadingOrderPermutations == null)
					{
						ReadingOrderInformation roi;
						int arity = myRoleDisplayOrder.Count;
						// UNDONE: This should all change to using RoleBase[] when BuildPermutations is brought to O(sanity)
						//		public decimal Factorial(decimal number) //total num [] elements
						//    {   if (number >= 2)
						//        {   number *= Factorial(number - 1);  }
						//        return number; }
						List<RoleBase> tempRoleList = new List<RoleBase>(arity);
						myReadingOrderPermutations = new ReadingOrderKeyedCollection();
						for (int i = 0; i < arity; ++i)
						{
							tempRoleList.Add(myRoleDisplayOrder[i]);
						}
						List<List<RoleBase>> myPerms; myPerms = this.BuildPermutations(tempRoleList);
						int numPerms = myPerms.Count;
						for (int i = 0; i < numPerms; ++i)
						{
							roi = new ReadingOrderInformation(this, myPerms[i].ToArray());
							myReadingOrderPermutations.Add(roi);
						}
					}
					TypeEditorHost host = OnScreenTypeEditorHost.Create(new ReadingOrderDescriptor(myFact, this, ResourceStrings.ModelReadingEditorNewItemText), myReadingOrderPermutations, TypeEditorHostEditControlStyle.TransparentEditRegion);
					host.Flags = VirtualTreeInPlaceControlFlags.DrawItemText | VirtualTreeInPlaceControlFlags.ForwardKeyEvents | VirtualTreeInPlaceControlFlags.SizeToText;

					return new VirtualTreeLabelEditData(
						host,
						delegate(VirtualTreeItemInfo itemInfo, Control editControl)
						{
							return LabelEditResult.AcceptEdit;
						});
				}
				else if (column == (int)ColumnIndex.ReadingBranch && rowType == RowType.Uncommitted)
				{
					InplaceReadingEditor editor = new InplaceReadingEditor();
					editor.Initialize(myReadingOrderKeyedCollection[row].OrderedReplacementFields, SystemColors.WindowText, SystemColors.GrayText);
					return new VirtualTreeLabelEditData(
						editor,
						delegate(VirtualTreeItemInfo itemInfo, Control editControl)
						{
							Store store = myFact.Store;
							if (editor.EscapePressed || !(store as IORMToolServices).CanAddTransaction)
							{
								return LabelEditResult.CancelEdit;
							}
							string newReadingText = editor.BuildReadingText();
							if (newReadingText.Length != 0)
							{
								Reading theNewReading;
								try
								{
									myInsertedRow = row;
									using (Transaction t = store.TransactionManager.BeginTransaction(ResourceStrings.ModelReadingEditorNewReadingTransactionText))
									{
										ReadingOrder theOrder = myFact.EnsureReadingOrder(myReadingOrderKeyedCollection[row].RoleOrder);
										Debug.Assert(theOrder != null, "A ReadingOrder should have been found or created.");
										theNewReading = new Reading(store);
										LinkedElementCollection<Reading> readings = theOrder.ReadingCollection;
										readings.Add(theNewReading);
										theNewReading.Text = newReadingText;
										t.Commit();
									}
								}
								finally
								{
									myInsertedRow = -1;
								}
								return LabelEditResult.AcceptEdit;
							}
							return LabelEditResult.CancelEdit;
						});
				}
				else
				{
					return VirtualTreeLabelEditData.Invalid;
				}
			}
			public LabelEditResult CommitLabelEdit(int row, int column, string newText)
			{
				return LabelEditResult.CancelEdit;
			}
			public BranchFeatures Features
			{
				get
				{
					return BranchFeatures.ExplicitLabelEdits | BranchFeatures.ImmediateSelectionLabelEdits | BranchFeatures.PositionTracking | BranchFeatures.DelayedLabelEdits | BranchFeatures.InsertsAndDeletes | BranchFeatures.ComplexColumns;
				}
			}
			public VirtualTreeAccessibilityData GetAccessibilityData(int row, int column)
			{
				return VirtualTreeAccessibilityData.Empty;
			}
			public VirtualTreeDisplayData GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
			{
				RowType rowType = myReadingOrderKeyedCollection.GetRowType(row);
				VirtualTreeDisplayData retval = VirtualTreeDisplayData.Empty;
				if (column == (int)ColumnIndex.ReadingBranch)
				{
					retval.ForeColor = (rowType == RowType.Uncommitted) ? Color.Gray : retval.ForeColor;
				}
				else
				{
					if (rowType != RowType.TypeEditorHost)
					{
						retval.Image = 0;
						retval.SelectedImage = 0;   //you must set both .Image and .SelectedImage or an exception will be thrown
					}
				}
				return retval;
			}
			public object GetObject(int row, int column, ObjectStyle style, ref int options)
			{
				switch (style)
				{
					case ObjectStyle.SubItemRootBranch:
						if (myReadingOrderKeyedCollection.GetRowType(row) == RowType.Committed)
						{
							return myReadingOrderKeyedCollection[row].EnsureBranch();
						}
						break;
					case ObjectStyle.TrackingObject:
						if (row == this.VisibleItemCount - 1)
						{
							return new object(); //just need somthing which is not null
						}
						break;
					default:
						return null;
				}
				return null;
			}
			public string GetText(int row, int column)
			{
				if (column == (int)ColumnIndex.ReadingBranch)
				{
					switch (myReadingOrderKeyedCollection.GetRowType(row))
					{
						case RowType.TypeEditorHost:
							return ResourceStrings.ModelReadingEditorNewItemText;
						case RowType.Uncommitted:
							return myReadingOrderKeyedCollection[row].Text;
					}
				}
				return null;
			}
			public string GetTipText(int row, int column, ToolTipType tipType)
			{
				if (column == (int)ColumnIndex.ReadingOrder)
				{
					switch (myReadingOrderKeyedCollection.GetRowType(row))
					{
						case RowType.Committed:
							return myReadingOrderKeyedCollection[row].ToString();
						case RowType.Uncommitted:
							return myReadingOrderKeyedCollection[row].Text;
						//does not get displayed as no icon or text is displayed on this column/row
						//case RowType.TypeEditorHost: 
						//    return "Choose a New Reading Order from the Drop Down List"; 
					}
				}
				return null;
			}
			public bool IsExpandable(int row, int column)
			{
				return false;
			}
			public LocateObjectData LocateObject(object obj, ObjectStyle style, int locateOptions)
			{
				if (style == ObjectStyle.TrackingObject)
				{
					ReadingOrderInformation info;
					LinkedElementCollection<RoleBase> roles;
					Reading reading;
					FactType fact;
					int location = -1;

					if (null != (info = obj as ReadingOrderInformation))
					{
						location = myReadingOrderKeyedCollection.IndexOf(myReadingOrderKeyedCollection[info.RoleOrder]);
					}
					else if (null != (roles = obj as LinkedElementCollection<RoleBase>))
					{
						location = myReadingOrderKeyedCollection.IndexOf(myReadingOrderKeyedCollection[roles]);
					}
					else if (null != (reading = (obj as Reading)))
					{
						ReadingOrder order = reading.ReadingOrder;
						location = myReadingOrderKeyedCollection.IndexOf(myReadingOrderKeyedCollection[order.RoleCollection]);
					}
					else if (null != (fact = (obj as FactType)))
					{
						return new LocateObjectData(0, (int)ColumnIndex.ReadingOrder, (int)TrackingObjectAction.ThisLevel);
					}
					else if (RowType.TypeEditorHost.Equals(obj))
					{
						return new LocateObjectData(this.VisibleItemCount - 1, (int)ColumnIndex.ReadingBranch, (int)TrackingObjectAction.ThisLevel);
					}

					if (-1 != location && myReadingOrderKeyedCollection.GetRowType(location) == RowType.Uncommitted)
					{
						return new LocateObjectData(location, (int)ColumnIndex.ReadingBranch, (int)TrackingObjectAction.ThisLevel);
					}
					else if (location >= 0)
					{
						return new LocateObjectData(location, (int)ColumnIndex.ReadingBranch, (int)TrackingObjectAction.NextLevel);
					}
				}
				return new LocateObjectData();
			}
			public event BranchModificationEventHandler OnBranchModification;
			public void OnDragEvent(object sender, int row, int column, DragEventType eventType, DragEventArgs args)
			{
			}
			public void OnGiveFeedback(GiveFeedbackEventArgs args, int row, int column)
			{
			}
			public void OnQueryContinueDrag(QueryContinueDragEventArgs args, int row, int column)
			{
			}
			public VirtualTreeStartDragData OnStartDrag(object sender, int row, int column, DragReason reason)
			{
				return VirtualTreeStartDragData.Empty;
			}
			public StateRefreshChanges ToggleState(int row, int column)
			{
				return StateRefreshChanges.None;
			}
			public StateRefreshChanges SynchronizeState(int row, int column, IBranch matchBranch, int matchRow, int matchColumn)
			{
				return StateRefreshChanges.None;
			}
			public int UpdateCounter
			{
				get
				{
					return 0;
				}
			}
			public int VisibleItemCount
			{
				get
				{
					return myReadingOrderKeyedCollection.Count + 1;
				}
			}
			#endregion

			#region IMultiColumnBranch Interface Members
			public int ColumnCount
			{
				get
				{
					return COLUMN_COUNT;
				}
			}
			public SubItemCellStyles ColumnStyles(int column)
			{
				return SubItemCellStyles.Complex;
			}
			public int GetJaggedColumnCount(int row)
			{
				return COLUMN_COUNT;
			}
			#endregion

			#region IReadingEditorBranch Interface Members
			#region Reading Branch Methods
			/// <summary>
			/// Initiates a New Reading within the selected Reading Order
			/// </summary>
			/// <param name="itemLocation">Reading Order Row</param>
			public void AddNewReading(int itemLocation)
			{
				ReadingOrderInformation orderInfo = myReadingOrderKeyedCollection[itemLocation];
				orderInfo.EnsureBranch().ShowNewRow(true);
				VirtualTreeControl control = ReadingEditor.Instance.TreeControl;
				control.SelectObject(this, orderInfo, (int)ObjectStyle.TrackingObject, 0);
				control.BeginLabelEdit();
			}
			/// <summary>
			/// Instruct the readingbranch that a reading has been added to the collection
			/// </summary>
			/// <param name="reading">the reading to add</param>
			public void ReadingAdded(Reading reading)
			{
				ReadingOrder order = reading.ReadingOrder;
				if (order != null)
				{
					int location = this.LocateCollectionItem(order);

					if (location < 0)
					{
						this.PopulateReadingOrderInfo(order);
						if (OnBranchModification != null)
						{
							int newLoc = this.LocateCollectionItem(order);
							OnBranchModification(this, BranchModificationEventArgs.InsertItems(this, newLoc - 1, 1));
							OnBranchModification(this, BranchModificationEventArgs.UpdateCellStyle(this, newLoc, (int)ColumnIndex.ReadingBranch, true)); //may not be needed due to callback on update
							//redraw off and back on in the branch if it has no more than 1 reading
						}
					}
					else
					{
						myReadingOrderKeyedCollection[location].EnsureBranch().AddReading(reading);
						if (OnBranchModification != null)
						{
							OnBranchModification(this, BranchModificationEventArgs.UpdateCellStyle(this, location, (int)ColumnIndex.ReadingBranch, true));

							int actualIndex = myFact.ReadingOrderCollection.IndexOf(order);
							if (actualIndex != location)
							{
								this.ReadingOrderLocationUpdate(order);
							}
							else
							{
								OnBranchModification(this, BranchModificationEventArgs.Redraw(false));
								OnBranchModification(this, BranchModificationEventArgs.Redraw(true));
							}
						}
					}
				}
			}
			/// <summary>
			/// Triggers the events notifying the tree that a Reading in the Readingbranch has been updated. 
			/// </summary>
			/// <param name="reading">The Reading affected</param>
			public void UpdateReading(Reading reading)
			{
				ReadingOrderInformation info = new ReadingOrderInformation(this, reading.ReadingOrder);
				ReadingOrderKeyedCollection orderCollection = myReadingOrderKeyedCollection;
				if (!orderCollection.Contains(info.RoleOrder))
				{
					orderCollection.Add(info);
				}
				int row = orderCollection.IndexOf(orderCollection[info.RoleOrder]);
				Debug.Assert(row >= 0, "Reading Must exist before it can be updated");
				ReadingBranch branch = orderCollection[row].Branch;
				if (branch != null)
				{
					branch.UpdateReading(reading);
				}
			}
			/// <summary>
			/// Event callback from Changing the Order of  and item in the ReadingOrdersMovableCollectoin: 
			/// will update the branch to reflect the changed order
			/// </summary>
			/// <param name="reading">The Reading moved</param>
			public void ReadingLocationUpdate(Reading reading)
			{
				if (OnBranchModification != null)
				{
					ReadingOrder order = reading.ReadingOrder;
					int currentLocation = this.LocateCollectionItem(order);
					Debug.Assert(currentLocation >= 0, "Cannot Locate Reading");
					if (currentLocation >= 0)
					{
						ReadingBranch branch = myReadingOrderKeyedCollection[currentLocation].Branch;
						if (branch != null)
						{
							branch.ReadingItemOrderChanged(reading);
						}
					}
				}
			}
			/// <summary>
			/// Triggers notification that a Reading has been removed from the branch.
			/// </summary>
			/// <param name="reading">The Reading which has been removed</param>
			/// <param name="order">The order of the link</param>
			public void ReadingRemoved(Reading reading, ReadingOrder order)
			{
				if (OnBranchModification != null)
				{
					int location = this.LocateCollectionItem(order);
					if (location >= 0)
					{
						ReadingBranch branch = myReadingOrderKeyedCollection[location].Branch;
						if (branch != null)
						{
							branch.ItemRemoved(reading);
						}
					}
				}
			}
			/// <summary>
			/// Moves the selected Reading Up
			/// </summary>
			public void PromoteSelectedReading(int readingItemLocation, int orderItemLocation)
			{
				ReadingBranch branch = myReadingOrderKeyedCollection[orderItemLocation].Branch;
				if (branch != null)
				{
					branch.PromoteItem(readingItemLocation);
				}
			}
			/// <summary>
			/// Moves the selected Reading Down
			/// </summary>
			public void DemoteSelectedReading(int readingItemLocation, int orderItemLocation)
			{
				ReadingBranch branch = myReadingOrderKeyedCollection[orderItemLocation].Branch;
				if (branch != null)
				{
					branch.DemoteItem(readingItemLocation);
				}
			}
			#endregion //Reading Branch Methods

			#region ReadingOrder Branch Methods
			/// <summary>
			/// Used to find out if the branch is in the process of adding a new entry from input into the branch.
			/// </summary>
			public bool IsAdding
			{
				get
				{
					return myInsertedRow != -1;
				}
			}
			/// <summary>
			/// Initiates the Drop Down to select a new reading order for the reading to add
			/// </summary>
			public void AddNewReadingOrder()
			{
				VirtualTreeControl control = ReadingEditor.Instance.TreeControl;
				control.SelectObject(this, RowType.TypeEditorHost, (int)ObjectStyle.TrackingObject, 0);
				control.BeginLabelEdit();
			}
			/// <summary>
			/// Initiate edit for first reading within the <see cref="LinkedElementCollection{RoleBase}"/> (will create a new item if needed)
			/// </summary>
			/// <param name="collection">The <see cref="LinkedElementCollection{RoleBase}"/> to use</param>
			public void EditReadingOrder(IList<RoleBase> collection)
			{
				Debug.Assert(collection != null, "LinkedElementCollection<RoleBase> is null");

				RoleBase[] roles = new RoleBase[collection.Count];
				for (int i = 0; i < collection.Count; ++i)
				{
					roles[i] = collection[i];
				}

				ReadingOrderInformation orderInfo = new ReadingOrderInformation(this, roles);
				if (!myReadingOrderKeyedCollection.Contains(orderInfo))
				{
					int newOrder = this.ShowNewOrder(roles);
				}

				VirtualTreeControl control = ReadingEditor.Instance.TreeControl;
				if (control.SelectObject(this, orderInfo, (int)ObjectStyle.TrackingObject, 0))
				{
					control.BeginLabelEdit();
				}
			}
			/// <summary>
			/// Moves the selected ReadingOrder up
			/// </summary>
			/// <param name="itemLocation">location</param>
			public void PromoteSelectedReadingOrder(int itemLocation)
			{
				this.MoveItem(true, itemLocation);
			}
			/// <summary>
			/// Moves the selected ReadingOrder Down
			/// </summary>
			/// <param name="itemLocation">location</param>
			public void DemoteSelectedReadingOrder(int itemLocation)
			{
				this.MoveItem(false, itemLocation);
			}
			/// <summary>
			/// Removes the item selected when called by the context menu
			/// </summary>
			public void RemoveSelectedItem()
			{
				VirtualTreeItemInfo orderItem = this.CurrentSelectionInfoReadingOrderBranch();
				RowType rowType = myReadingOrderKeyedCollection.GetRowType(orderItem.Row);

				if (rowType == RowType.Committed)
				{
					VirtualTreeItemInfo readingItem = this.CurrentSelectionInfoReadingBranch();
					ReadingBranch branch = myReadingOrderKeyedCollection[orderItem.Row].Branch;
					if (branch != null)
					{
						branch.RemoveItem(readingItem.Row);  //Remove selected Row in the ReadingBranch
					}
				}
				else if (rowType == RowType.Uncommitted) //remove the row from the readingOrderBranch
				{
					OnBranchModification(this, BranchModificationEventArgs.DeleteItems(this, orderItem.Row, 1));
					myReadingOrderKeyedCollection.RemoveAt(orderItem.Row);
				}
			}
			/// <summary>
			/// Event callback from Changing the Order of  and item in the ReadingOrdersMovableCollectoin: 
			/// will update the branch to reflect the changed order
			/// </summary>
			/// <param name="order">The ReadingOrder affected</param>
			public void ReadingOrderLocationUpdate(ReadingOrder order)
			{
				if (OnBranchModification != null)
				{
					int currentLocation = this.LocateCollectionItem(order);
					if (currentLocation >= 0) //make sure it was found
					{
						ReadingOrderInformation readingOrderInfo = myReadingOrderKeyedCollection[currentLocation];
						int newLocation = myFact.ReadingOrderCollection.IndexOf(order);
						myReadingOrderKeyedCollection.RemoveAt(currentLocation);
						myReadingOrderKeyedCollection.Insert(newLocation, readingOrderInfo);
						OnBranchModification(this, BranchModificationEventArgs.MoveItem(this, currentLocation, newLocation));
					}
				}
			}
			/// <summary>
			/// Handles removing a ReadingOrder which has been deleted
			/// </summary>
			/// <param name="order">The ReadingOrder which has been removed</param>
			/// <param name="fact">The Fact for the ReadingOrder</param>
			public void ReadingOrderRemoved(ReadingOrder order, FactType fact)
			{
				if (OnBranchModification != null)
				{
					int location = this.LocateCollectionItem(order);
					if (location >= 0) //make sure it was found
					{
						ReadingBranch branch = myReadingOrderKeyedCollection[location].Branch;
						if (branch != null && branch.HasNewRow) //handle bug (m.s. introduced) in Virtual Tree which does not return the correct count when removing a branch
						{
							branch.ShowNewRow(false);
						}
						myReadingOrderKeyedCollection.RemoveAt(location);
						OnBranchModification(this, BranchModificationEventArgs.DeleteItems(this, location, 1));
					}
				}
			}
			/// <summary>
			///  Get context menu commands supported for current selection
			/// </summary>
			/// <returns>ReadingEditorCommands bit field</returns>
			/// <param name="itemLocation">Reading Order Row</param>
			public ReadingEditorCommands SupportedSelectionCommands(int itemLocation)
			{
				ReadingEditorCommands retval = ReadingEditorCommands.AddReadingOrder;
				if (itemLocation < this.VisibleItemCount - 1)
				{
					//ReadingOrder Context Menu Options
					if (myReadingOrderKeyedCollection.Count > 0)
					{
						if (myReadingOrderKeyedCollection[itemLocation].ReadingOrder != null)
						{
							retval |= ReadingEditorCommands.AddReading;
						}

						if (itemLocation > 0 && myReadingOrderKeyedCollection[itemLocation].ReadingOrder != null)
						{
							retval |= ReadingEditorCommands.PromoteReadingOrder;
						}

						if (itemLocation < this.VisibleItemCount - 2
							&& myReadingOrderKeyedCollection[itemLocation].ReadingOrder != null
							&& myReadingOrderKeyedCollection[itemLocation + 1].ReadingOrder != null)
						{
							retval |= ReadingEditorCommands.DemoteReadingOrder;
						}
					}

					//Reading Context Menu Options
					if (myReadingOrderKeyedCollection[itemLocation].ReadingOrder != null)
					{
						VirtualTreeItemInfo readingItemInfo = this.CurrentSelectionInfoReadingBranch();
						if (readingItemInfo.Row < myReadingOrderKeyedCollection[itemLocation].ReadingOrder.ReadingCollection.Count) //Catch if readingbranch is displaying ONLY an uncomitted reading
						{
							retval |= ReadingEditorCommands.DeleteReading;
							ReadingBranch readingBranch = myReadingOrderKeyedCollection[itemLocation].Branch;
							if (readingBranch != null)
							{
								if (readingItemInfo.Row > 0 && readingBranch.RowCount > 1)
								{
									retval |= ReadingEditorCommands.PromoteReading;
								}

								if (readingItemInfo.Row < readingBranch.RowCount - ((readingBranch.HasNewRow) ? 2 : 1)) //catch if displaying an uncommitted reading and committed readings
								{
									retval |= ReadingEditorCommands.DemoteReading;
								}
							}
						}
					}
				}
				return retval;
			}
			#endregion //ReadingOrder Branch Methods
			#endregion //IReadingEditorBranch Interface Members

			#region Branch Helper Functions
			private string[] GetRoleNames()
			{
				string[] retVal = myRoleNames;
				if (retVal == null)
				{
					IList<RoleBase> factRoles = GetReadingRoleCollection(myFact);
					ObjectType rolePlayer;
					int factArity = factRoles.Count;
					if (factArity == 1)
					{
						rolePlayer = factRoles[0].Role.RolePlayer;
						retVal = new string[] { (rolePlayer != null) ? rolePlayer.Name : ResourceStrings.ModelReadingEditorMissingRolePlayerText };
					}
					else
					{
						IList<RoleBase> roleDisplayOrder = myRoleDisplayOrder;
						ObjectType[] rolePlayers = new ObjectType[factArity];
						for (int i = 0; i < factArity; ++i)
						{
							rolePlayers[i] = roleDisplayOrder[i].Role.RolePlayer;
						}
						retVal = new string[factArity];
						for (int i = 0; i < factArity; ++i)
						{
							rolePlayer = roleDisplayOrder[i].Role.RolePlayer;

							int subscript = 0;
							bool useSubscript = false;
							int j = 0;
							for (; j < i; ++j)
							{
								if (rolePlayer == rolePlayers[j])
								{
									useSubscript = true;
									++subscript;
								}
							}
							for (j = i + 1; !(useSubscript) && (j < factArity); ++j)
							{
								if (rolePlayer == rolePlayers[j])
								{
									useSubscript = true;
								}
							}
							if (useSubscript)
							{
								retVal[i] = string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelReferenceModePickerFormatString, (rolePlayer != null) ? rolePlayer.Name : ResourceStrings.ModelReadingEditorMissingRolePlayerText, subscript + 1);
							}
							else
							{
								retVal[i] = (rolePlayer != null) ? rolePlayer.Name : ResourceStrings.ModelReadingEditorMissingRolePlayerText;
							}
						}
					}
					return myRoleNames = retVal;
				}
				return retVal;
			}
			private int ShowNewOrder(IList<RoleBase> order)
			{
				ReadingOrderInformation info = new ReadingOrderInformation(this, order as RoleBase[]);
				if (!myReadingOrderKeyedCollection.Contains(order))
				{
					myReadingOrderKeyedCollection.Add(info);
					if (OnBranchModification != null)
					{
						OnBranchModification(this, BranchModificationEventArgs.InsertItems(this, this.VisibleItemCount - 2, 1));
					}
				}
				return myReadingOrderKeyedCollection.IndexOf(myReadingOrderKeyedCollection[info.RoleOrder]);
			}
			/// <summary>
			/// Populates the tree
			/// </summary>
			/// <param name="readingOrderIndex">Use -1 for all, or the index of a specific ReadingOrder within the collection </param>
			private void PopulateReadingOrderInfo(int readingOrderIndex)
			{
				LinkedElementCollection<ReadingOrder> readingOrders = myFact.ReadingOrderCollection;
				if (readingOrderIndex == -1)
				{
					myReadingOrderKeyedCollection.Clear();
					int thisBranchCount = readingOrders.Count;
					for (int i = 0; i < thisBranchCount; ++i)
					{
						this.PopulateReadingOrderInfo(readingOrders[i]);
					}
				}
				else
				{
					this.PopulateReadingOrderInfo(readingOrders[readingOrderIndex]);
				}
			}
			private void PopulateReadingOrderInfo(ReadingOrder order)
			{
				myReadingOrderKeyedCollection.Add(new ReadingOrderInformation(this, order));
			}
			/// <summary>
			/// Initiates a transaction for Moving a ReadingOrder within the collection
			/// </summary>
			/// <param name="promote">True if Moving UP, False if Moving DOWN</param>
			/// <param name="orderRow">Row to be moved</param>
			private void MoveItem(bool promote, int orderRow)
			{
				int currentLocation = -1, newLocation = 0;
				Debug.Assert(orderRow >= 0, "Attempt to move item which does not exist in the collection");
				currentLocation = orderRow; // orderItemInfo.Row;
				newLocation = currentLocation + (promote ? -1 : 1);
				using (Transaction t = myFact.Store.TransactionManager.BeginTransaction(ResourceStrings.ModelReadingEditorMoveReadingOrder))
				{
					myFact.ReadingOrderCollection.Move(currentLocation, newLocation);
					t.Commit();
				}
			}
			/// <summary>
			/// Locates the OrderInformation item in the collection which reference matches the order passed in
			/// </summary>
			/// <param name="order">The ReadingOrder to match</param>
			/// <returns>index of item in the collection, -1 if not found</returns>
			private int LocateCollectionItem(ReadingOrder order)
			{
				if (order != null)
				{
					ReadingOrderKeyedCollection orders = myReadingOrderKeyedCollection;
					if (order.IsDeleted)
					{
						// The role collection on a removed order will always be empty, just search the list
						int orderCount = orders.Count;
						for (int i = 0; i < orderCount; ++i)
						{
							if (orders[i].ReadingOrder == order)
							{
								return i;
							}
						}
					}
					else
					{
						LinkedElementCollection<RoleBase> roleCollection = order.RoleCollection;
						if (orders.Contains(roleCollection)) //UNDONE: need to re-work "IndexOf" to return -1 if non-existent
						{
							return orders.IndexOf(orders[roleCollection]);
						}
					}
				}
				return -1;
			}
			/// <summary>
			/// Information about the current selected cell in a tree for the ReadingOrderBranch
			/// </summary>
			/// <returns>VirtualTreeItemInfo</returns>
			private VirtualTreeItemInfo CurrentSelectionInfoReadingOrderBranch()
			{
				VirtualTreeControl control = ReadingEditor.Instance.TreeControl;
				return control.Tree.GetItemInfo(control.CurrentIndex, (int)ColumnIndex.ReadingOrder, true);
			}
			/// <summary>
			/// Information about the current selected cell in a tree for the ReadingBranch
			/// </summary>
			/// <returns>VirtualTreeItemInfo</returns>
			private VirtualTreeItemInfo CurrentSelectionInfoReadingBranch()
			{
				VirtualTreeControl control = ReadingEditor.Instance.TreeControl;
				return control.Tree.GetItemInfo(control.CurrentIndex, control.CurrentColumn, true);
			}

			private List<List<RoleBase>> BuildPermutations(List<RoleBase> roleList)
			{
				// UNDONE: This is absolutely nuts. It should return RoleBase[][] and allocate the arrays once
				List<List<RoleBase>> retval = new List<List<RoleBase>>();
				List<RoleBase> tmpList = null;
				int count = roleList.Count;
				if (count == 1)
				{
					retval.Add(roleList);
				}
				else
				{
					for (int i = 0; i < count; ++i)
					{
						RoleBase currentRole = roleList[i];
						tmpList = new List<RoleBase>(count - 1);
						for (int j = 0; j < count; ++j)
						{
							if (roleList[j] != currentRole)
							{
								tmpList.Add(roleList[j]);
							}
						}
						List<List<RoleBase>> result = BuildPermutations(tmpList);
						int resCount = result.Count;
						for (int j = 0; j < resCount; ++j)
						{
							result[j].Insert(0, currentRole);
							retval.Add(result[j]);
						}
					}
				}
				return retval;
			}
			#endregion //Branch Helper Functions

			#region Nested KeyedCollection class
			// UNDONE: Using an IList (or any other mutable object) as the key into a Dictionary is potentially error-prone. This needs to be reviewed.
			private sealed class ReadingOrderKeyedCollection : KeyedCollection<IList<RoleBase>, ReadingOrderInformation>
			{
				public ReadingOrderKeyedCollection() : base(ListEqualityComparer.Comparer, 16) { }
				protected sealed override IList<RoleBase> GetKeyForItem(ReadingOrderInformation item)
				{
					return item.RoleOrder;
				}

				/// <summary>
				/// Returns a RowType for the row requested, defaults to RowType.Committed
				/// </summary>
				/// <param name="row">zero based Row index</param>
				/// <returns>RowType</returns>
				public RowType GetRowType(int row)
				{
					if (row >= Count)
					{
						return RowType.TypeEditorHost;
					}
					return (this[row].ReadingOrder != null) ? RowType.Committed : RowType.Uncommitted;
				}

				#region Nested Class to implement IEqualityComparer for an IList
				private sealed class ListEqualityComparer : IEqualityComparer<IList<RoleBase>>
				{
					private ListEqualityComparer() { }
					public static readonly ListEqualityComparer Comparer = new ListEqualityComparer();

					public bool Equals(IList<RoleBase> x, IList<RoleBase> y)
					{
						int xCount = x.Count;
						if (xCount != y.Count)
						{
							return false;
						}
						for (int i = 0; i < xCount; ++i)
						{
							if (x[i] != y[i])
							{
								return false;
							}
						}
						return true;
					}

					public int GetHashCode(IList<RoleBase> obj)
					{
						int objCount = obj.Count;
						if (objCount == 0)
						{
							return 0;
						}

						int hashCode = obj[0].GetHashCode();
						for (int i = 1; i < objCount; ++i)
						{
							hashCode ^= Utility.RotateRight(obj[i].GetHashCode(), i);
						}
						return hashCode;
					}
				}
				#endregion // Nested Class to implement IEqualityComparer for an IList
			}
			#endregion nested KeyedCollection class

			#region Nested ReadingInformation class used with the KeyedCollection class
			private sealed class ReadingOrderInformation
			{
				private readonly ReadingOrderBranch myParentBranch;
				private readonly IList<RoleBase> myRoleOrder;
				private ReadingOrder myReadingOrder;
				private string[] myOrderedReplacementFields;
				private string myText;
				private ReadingBranch myReadingBranch;

				/// <summary>
				/// Used for a new order that does not exist in the model (uncommitted)
				/// </summary>
				/// <param name="parentBranch">branch this exists in</param>
				/// <param name="roleOrder">RoleBase[]</param>
				public ReadingOrderInformation(ReadingOrderBranch parentBranch, RoleBase[] roleOrder)
				{
					myParentBranch = parentBranch;
					myRoleOrder = roleOrder;
				}

				/// <summary>
				/// Used for a an order that does exist in the model (committed)
				/// </summary>
				/// <param name="parentBranch">branch this exists in</param>
				/// <param name="readingOrder">ReadingOrder to add</param>
				public ReadingOrderInformation(ReadingOrderBranch parentBranch, ReadingOrder readingOrder)
				{
					myParentBranch = parentBranch;
					myReadingOrder = readingOrder;
					if (readingOrder != null)
					{
						myRoleOrder = readingOrder.RoleCollection;
					}
				}

				/// <summary>
				/// Returns the ReadingOrder of this Reading Order Information Item
				/// </summary>
				public ReadingOrder ReadingOrder
				{
					get
					{
						return myReadingOrder;
					}
				}
				/// <summary>
				/// Returns a ReadingBranch for this Order, the ReadingOrder must already exist in the model.
				/// This will always return a Branch. Use the <see cref="Branch"/> property to get the current
				/// branch without forcing it to be created.
				/// </summary>
				public ReadingBranch EnsureBranch()
				{
					ReadingBranch retval = myReadingBranch;
					if (retval == null)
					{
						if (myReadingOrder == null) //obtain new readingOrder to commit a new reading (in readingOrder is non-existent)
						{
							myReadingOrder = myParentBranch.Fact.EnsureReadingOrder(this.myRoleOrder);
						}
						myReadingBranch = new ReadingBranch(myParentBranch.Fact, myReadingOrder, this);
						return myReadingBranch;
					}
					return retval;
				}
				/// <summary>
				/// Return the current branch. If the branch has not been created this will
				/// return <see langword="null"/>. Use the <see cref="EnsureBranch"/> method
				/// to force the branch to be created.
				/// </summary>
				public ReadingBranch Branch
				{
					get
					{
						return myReadingBranch;
					}
				}
				/// <summary>
				/// Returns an <see cref="IList{RoleBase}"/> of the <see cref="RoleBase"/> Order
				/// </summary>
				public IList<RoleBase> RoleOrder
				{
					get
					{
						return myRoleOrder;
					}
				}
				/// <summary>
				/// Returns Display Text for the RoleBase Order
				/// </summary>
				public string Text
				{
					get
					{
						string retVal = myText;
						if ((object)retVal == null)
						{
							return myText = string.Join(CultureInfo.CurrentCulture.TextInfo.ListSeparator, OrderedReplacementFields);
						}
						return retVal;
					}
				}
				/// <summary>
				/// Returns a string[] for the Role Order Replacement Fields
				/// </summary>
				public string[] OrderedReplacementFields
				{
					get
					{
						string[] retVal = myOrderedReplacementFields;
						if (retVal == null)
						{
							string[] roleNames = myParentBranch.GetRoleNames();
							IList<RoleBase> displayOrder = myParentBranch.DisplayOrder;
							int arity = roleNames.Length;
							Debug.Assert(roleNames.Length == displayOrder.Count);
							IList<RoleBase> roles = myRoleOrder;
							switch (arity)
							{
								case 1:
									retVal = new string[] { roleNames[0] };
									break;
								case 2:
									if (roles[0].FactType.UnaryRole != null)
									{
										retVal = new string[] { roleNames[0] };
									}
									else
									{
										retVal = new string[]{
										roleNames[displayOrder.IndexOf(roles[0])],
										roleNames[displayOrder.IndexOf(roles[1])]};
									}
									break;
								case 3:
									retVal = new string[]{
										roleNames[displayOrder.IndexOf(roles[0])],
										roleNames[displayOrder.IndexOf(roles[1])],
										roleNames[displayOrder.IndexOf(roles[2])]};
									break;
								default:
									retVal = new string[arity];
									for (int i = 0; i < retVal.Length; ++i)
									{
										retVal[i] = roleNames[displayOrder.IndexOf(roles[i])];
									}
									break;
							}
							return myOrderedReplacementFields = retVal;
						}
						return retVal;
					}
				}
				/// <summary>
				/// Overridden, returns Text
				/// </summary>
				/// <returns></returns>
				public sealed override string ToString()
				{
					return Text;
				}
			}
			#endregion //Nested ReadingInformation class used with the KeyedCollection class

			#region  Nested Class Branch Property Descriptor for New Reading Drop Down
			private sealed class ReadingOrderDescriptor : PropertyDescriptor
			{
				private readonly FactType myFact;
				private readonly ReadingOrderBranch myBranch;

				private sealed class ReadingOrderEditor : ElementPicker<ReadingOrderEditor>
				{
					public ReadingOrderEditor() { }
					protected sealed override object TranslateFromDisplayObject(int newIndex, object newObject)
					{
						return newObject as ReadingOrderInformation;
					}
					protected sealed override object TranslateToDisplayObject(object initialObject, IList contentList)
					{
						Debug.Assert(contentList is ReadingOrderKeyedCollection);
						return contentList.ToString();
					}
					protected sealed override System.Collections.IList GetContentList(ITypeDescriptorContext context, object value)
					{
						if (context != null)
						{
							return context.Instance as IList;
						}
						return null;
					}
				}
				public ReadingOrderDescriptor(FactType fact, ReadingOrderBranch branch, string name)
					: base(name, null)
				{
					myFact = fact;
					myBranch = branch;
				}
				public sealed override object GetEditor(Type editorBaseType)
				{
					Debug.Assert(editorBaseType == typeof(UITypeEditor));
					return new ReadingOrderEditor();
				}
				public sealed override bool CanResetValue(object component)
				{
					return false;
				}
				public sealed override Type ComponentType
				{
					get { return typeof(ReadingOrderInformation); }
				}
				public sealed override object GetValue(object component)
				{
					return this.Name;
				}
				public sealed override bool IsReadOnly
				{
					get { return true; }
				}
				public sealed override Type PropertyType
				{
					get { return typeof(ReadingOrderInformation); }
				}
				public sealed override void ResetValue(object component)
				{
				}
				public sealed override bool ShouldSerializeValue(object component)
				{
					return true;
				}
				public sealed override void SetValue(object component, object value)
				{
					ReadingOrderKeyedCollection collection = myBranch.myReadingOrderKeyedCollection;
					ReadingOrderInformation info = value as ReadingOrderInformation;
					int branchLocation = -1;
					if (info != null && collection.Contains(info.RoleOrder))
					{
						branchLocation = collection.IndexOf(collection[info.RoleOrder]);
					}

					if (branchLocation >= 0 && collection[branchLocation].ReadingOrder != null)
					{
						collection[branchLocation].EnsureBranch().ShowNewRow(true);
					}
					else
					{
						branchLocation = myBranch.ShowNewOrder(info.RoleOrder);
					}
					VirtualTreeControl control = ReadingEditor.Instance.TreeControl;
					control.SelectObject(myBranch, info, (int)ObjectStyle.TrackingObject, 0);
					control.BeginLabelEdit();
				}
			}
			#endregion

			#region Nested class ReadingBranch
			private sealed class ReadingBranch : IBranch
			{
				public const int COLUMN_COUNT = 1;
				private readonly FactType myFact;
				private readonly LinkedElementCollection<Reading> myReadingMC;
				private readonly List<ReadingData> myReadings = new List<ReadingData>();
				private readonly ReadingOrder myReadingOrder;
				private readonly ReadingOrderInformation myReadingInformation;
				private bool myShowNewRow;
				private int myInsertedRow = -1;

				#region Construction
				public ReadingBranch(FactType fact, ReadingOrder order, ReadingOrderInformation orderInformation)
				{
					myReadingOrder = order;
					myReadingMC = myReadingOrder.ReadingCollection;
					myFact = fact;
					myReadingInformation = orderInformation;
					this.PopulateBranchData();
				}
				#endregion

				#region Branch Properties
				/// <summary>
				/// Returns number of reading orders currently in the branch including new uncommitted readings
				/// </summary>
				public int RowCount
				{
					get
					{
						return this.VisibleItemCount;
					}
				}
				/// <summary>
				/// Returns true of branch has a new uncommitted reading, false otherwise
				/// </summary>
				public bool HasNewRow
				{
					get
					{
						return myShowNewRow;
					}
				}
				#endregion //Branch Properties

				#region IBranch Interface Methods

				public VirtualTreeLabelEditData BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
				{
					if (row == myReadings.Count - 1 && myShowNewRow)
					{
						InplaceReadingEditor editor = new InplaceReadingEditor();
						editor.Initialize(myReadingInformation.OrderedReplacementFields, SystemColors.WindowText, SystemColors.GrayText);
						return new VirtualTreeLabelEditData(
							editor,
							delegate(VirtualTreeItemInfo itemInfo, Control editControl)
							{
								Store store = myFact.Store;
								if (editor.EscapePressed || !(store as IORMToolServices).CanAddTransaction)
								{
									return LabelEditResult.CancelEdit;
								}
								string newReadingText = editor.BuildReadingText();
								if (newReadingText.Length != 0)
								{
									try
									{
										myInsertedRow = row;
										using (Transaction t = store.TransactionManager.BeginTransaction(ResourceStrings.ModelReadingEditorNewReadingTransactionText))
										{
											Debug.Assert(myReadingOrder != null, "A ReadingOrder should have been found or created.");
											this.ShowNewRow(false);
											Reading theNewReading = new Reading(store);
											LinkedElementCollection<Reading> readings = myReadingOrder.ReadingCollection;
											readings.Add(theNewReading);
											theNewReading.Text = newReadingText;
											t.Commit();
										}
									}
									finally
									{
										myInsertedRow = -1;
									}
									return LabelEditResult.AcceptEdit;
								}
								return LabelEditResult.CancelEdit;
							});
					}
					else if (0 != (activationStyle & VirtualTreeLabelEditActivationStyles.ImmediateSelection))
					{
						return VirtualTreeLabelEditData.DeferActivation;
					}
					else
					{
						Reading currentReading = myReadings[row].Reading;
						string startReadingText = currentReading.Text;
						InplaceReadingEditor editor = new InplaceReadingEditor();
						editor.Initialize(startReadingText, myReadingInformation.OrderedReplacementFields, SystemColors.WindowText, SystemColors.GrayText);
						return new VirtualTreeLabelEditData(
							editor,
							delegate(VirtualTreeItemInfo itemInfo, Control editControl)
							{
								Store store = currentReading.Store;
								if (editor.EscapePressed || !(store as IORMToolServices).CanAddTransaction)
								{
									return LabelEditResult.CancelEdit;
								}
								string newReadingText = editor.BuildReadingText();
								if (newReadingText != startReadingText)
								{
									using (Transaction t = store.TransactionManager.BeginTransaction(ResourceStrings.ModelReadingEditorChangePrimaryReadingText))
									{
										currentReading.Text = newReadingText;
										t.Commit();
									}
									return LabelEditResult.AcceptEdit;
								}
								return LabelEditResult.CancelEdit;
							});
					}
				}

				public LabelEditResult CommitLabelEdit(int row, int column, string newText)
				{
					return LabelEditResult.CancelEdit;
				}

				public BranchFeatures Features
				{
					get
					{
						return BranchFeatures.DelayedLabelEdits | BranchFeatures.ExplicitLabelEdits | BranchFeatures.PositionTracking | BranchFeatures.InsertsAndDeletes;
					}
				}

				public VirtualTreeAccessibilityData GetAccessibilityData(int row, int column)
				{
					return VirtualTreeAccessibilityData.Empty;
				}

				public VirtualTreeDisplayData GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
				{
					VirtualTreeDisplayData retVal = VirtualTreeDisplayData.Empty;
					if (row == this.VisibleItemCount - 1 && myShowNewRow)
					{
						retVal.ForeColor = Color.Gray;
					}
					return retVal;
				}

				public object GetObject(int row, int column, ObjectStyle style, ref int options)
				{
					if (style == ObjectStyle.TrackingObject)
					{
						return myReadingMC[row] as Object;
					}
					return null;
				}

				public string GetText(int row, int column)
				{
					if (row < this.VisibleItemCount)
					{
						return myReadings[row].Text;
					}
					else
					{
						return null;
					}
				}

				public string GetTipText(int row, int column, ToolTipType tipType)
				{
					return null;
				}

				public bool IsExpandable(int row, int column)
				{
					return false;
				}

				public LocateObjectData LocateObject(object obj, ObjectStyle style, int locateOptions)
				{
					Reading reading;
					if (style == ObjectStyle.TrackingObject)
					{
						if (obj == myReadingInformation.RoleOrder || myShowNewRow)
						{
							return new LocateObjectData(this.VisibleItemCount - 1, 0, (int)TrackingObjectAction.ThisLevel);
						}
						else if (null != (reading = (obj as Reading)))
						{
							int row = myReadingMC.IndexOf(reading);
							if (row >= 0)
							{
								return new LocateObjectData(row, 0, (int)TrackingObjectAction.ThisLevel);
							}
						}
					}
					return new LocateObjectData();
				}

				public event BranchModificationEventHandler OnBranchModification;

				public void OnDragEvent(object sender, int row, int column, DragEventType eventType, DragEventArgs args)
				{
				}

				public void OnGiveFeedback(GiveFeedbackEventArgs args, int row, int column)
				{
				}

				public void OnQueryContinueDrag(QueryContinueDragEventArgs args, int row, int column)
				{
				}

				public VirtualTreeStartDragData OnStartDrag(object sender, int row, int column, DragReason reason)
				{
					return VirtualTreeStartDragData.Empty;
				}

				public StateRefreshChanges ToggleState(int row, int column)
				{
					return StateRefreshChanges.None;
				}

				public StateRefreshChanges SynchronizeState(int row, int column, IBranch matchBranch, int matchRow, int matchColumn)
				{
					return StateRefreshChanges.None;
				}

				public int UpdateCounter
				{
					get
					{
						return 0;
					}
				}

				public int VisibleItemCount
				{
					get
					{
						return myReadings.Count; //get actual count for number of readings as a reading may not exist which is in the myReadings List
					}
				}

				#endregion

				#region Branch update methods
				/// <summary>
				/// Addition of a New Reading into the ReadingBranch
				/// </summary>
				/// <param name="reading">The reading changed or added</param>
				public void AddReading(Reading reading)
				{
					if (!myReadings.Contains(new ReadingData(null, reading)))
					{
						int index = myReadingMC.IndexOf(reading);
						myReadings.Insert(index, new ReadingData(FactType.PopulatePredicateText(reading, null, null, myReadingOrder.RoleCollection, myReadingInformation.OrderedReplacementFields), reading));
						if (OnBranchModification != null)
						{
							OnBranchModification(this, BranchModificationEventArgs.InsertItems(this, -1, 1));
						}
					}
					else
					{
						this.UpdateReading(reading);
					}
				}

				/// <summary>
				/// Initiates a Begin Label Edit for the specified Reading
				/// </summary>
				/// <param name="reading">the Reading to edit</param>
				public void EditReading(Reading reading)
				{
					int count = myReadingMC.Count;
					int location = myReadingMC.IndexOf(reading);
					if (location >= 0)
					{
						this.BeginLabelEdit(location, 0, VirtualTreeLabelEditActivationStyles.ImmediateSelection);
					}
				}
				/// <summary>
				/// Used to find out if the branch is in the process of adding a new entry from
				/// input into the branch.
				/// </summary>
				public bool IsAdding
				{
					get
					{
						return myInsertedRow != -1;
					}
				}
				/// <summary>
				/// Displays the new row for adding a reading to the reading order
				/// </summary>
				public void ShowNewRow(bool show)
				{
					if (!myShowNewRow && show)
					{
						myShowNewRow = true;
						myReadings.Add(new ReadingData(myReadingInformation.Text, null));
						if (OnBranchModification != null)
						{
							OnBranchModification(this, BranchModificationEventArgs.InsertItems(this, -1, 1));
						}
					}
					else if (myShowNewRow && !show)
					{
						myShowNewRow = false;
						if (OnBranchModification != null)
						{
							OnBranchModification(this, BranchModificationEventArgs.DeleteItems(this, this.VisibleItemCount - 1, 1));
						}
						myReadings.Remove(new ReadingData(myReadingInformation.Text, null));
					}
				}
				/// <summary>
				/// Initiates an Update of an existing Reading
				/// </summary>
				/// <param name="reading">The reading changed</param>
				public void UpdateReading(Reading reading)
				{
					int location = myReadings.IndexOf(new ReadingData(null, reading));
					if (location >= 0)
					{
						LinkedElementCollection<RoleBase> roles = reading.ReadingOrder.RoleCollection;
						string[] replacements = myReadingInformation.OrderedReplacementFields;
						if (roles.Count == replacements.Length)
						{
							myReadings[location] = new ReadingData(FactType.PopulatePredicateText(reading, null, null, roles, replacements), reading);

							if (OnBranchModification != null)
							{
								OnBranchModification(this, BranchModificationEventArgs.DisplayDataChanged(new DisplayDataChangedData(VirtualTreeDisplayDataChanges.Text, this, location, 0, 1)));
								OnBranchModification(this, BranchModificationEventArgs.Redraw(false));
								OnBranchModification(this, BranchModificationEventArgs.Redraw(true));
							}
						}
					}
				}
				/// <summary>
				/// Removes a reading from the brach based upon the reference of the reading object sent
				/// </summary>
				/// <param name="reading">The reading object which has been removed</param>
				public void ItemRemoved(Reading reading)
				{
					for (int i = myReadings.Count - 1; i >= 0; --i) // run counter backwards so we can modify the set
					{
						if (myReadings[i].Reading == reading)
						{
							myReadings.RemoveAt(i);
							if (OnBranchModification != null)
							{
								OnBranchModification(this, BranchModificationEventArgs.DeleteItems(this, i, 1));
							}
						}
					}
				}
				/// <summary>
				/// Initiate the removal of the current item selected
				/// </summary>
				/// <param name="row">Row to remove</param>
				public void RemoveItem(int row)
				{
					if (myShowNewRow && row == this.VisibleItemCount - 1)
					{
						this.ShowNewRow(false);
					}
					else
					{
						this.CommitLabelEdit(row, 0, string.Empty);
					}
				}
				/// <summary>
				/// Move a Reading Up
				/// </summary>
				/// <param name="row">Row to move up</param>
				public void PromoteItem(int row)
				{
					this.MoveItem(row, row - 1);
				}
				/// <summary>
				/// Move a Reading Down
				/// </summary>
				/// <param name="row">Row to move down</param>
				public void DemoteItem(int row)
				{
					this.MoveItem(row, row + 1);
				}
				/// <summary>
				/// Updates the Display Text and notifies the tree that the display data has changed
				/// </summary>
				public void ReadingItemOrderChanged(Reading reading)
				{
					int oldRow = myReadings.IndexOf(new ReadingData(null, reading));
					int currentRow = myReadingMC.IndexOf(reading);

					if (oldRow != currentRow)
					{
						ReadingData readingData = myReadings[oldRow];
						myReadings.RemoveAt(oldRow);
						myReadings.Insert(currentRow, readingData);

						if (OnBranchModification != null)
						{
							OnBranchModification(this, BranchModificationEventArgs.MoveItem(this, oldRow, currentRow));
						}
					}
				}
				#endregion

				#region Branch Helper Methods, Structs
				/// <summary>
				/// Iinitiates a transaction for moving a Reading within this Reading Order
				/// </summary>
				/// <param name="currentRow">Index of Reading To Move</param>
				/// <param name="newLocation">New Location in the collection</param>
				private void MoveItem(int currentRow, int newLocation)
				{
					using (Transaction t = myFact.Store.TransactionManager.BeginTransaction(ResourceStrings.ModelReadingEditorMoveReading))
					{
						myReadingMC.Move(currentRow, newLocation);
						t.Commit();
					}
				}
				/// <summary>
				/// Structure to hold Cached Reading Display Text with Populated Role Names, and the Reading Object associated
				/// </summary>
				private struct ReadingData : IEquatable<ReadingData>
				{
					private readonly Reading myReading;
					private readonly string myText;

					/// <summary>
					/// Constructor
					/// </summary>
					/// <param name="text">Populated Reading Display Text</param>
					/// <param name="reading">Reading Object</param>
					public ReadingData(string text, Reading reading)
					{
						this.myText = text;
						this.myReading = reading;
					}

					/// <summary>
					/// Returns the Populated Reading Display Text
					/// </summary>
					public string Text
					{
						get { return this.myText; }
					}

					/// <summary>
					/// Returns the Reading Object
					/// </summary>
					public Reading Reading
					{
						get { return this.myReading; }
					}
					/// <param name="other">Reading to compare</param>
					/// <returns>True if reading passed matches current</returns>
					public bool Equals(ReadingData other)
					{
						return (object)other.myReading == myReading;
					}
					public override bool Equals(object obj)
					{
						return obj is ReadingData && this.Equals((ReadingData)obj);
					}
					public override int GetHashCode()
					{
						return this.myReading.GetHashCode();
					}
					public static bool operator ==(ReadingData left, ReadingData right)
					{
						return left.Equals(right);
					}
					public static bool operator !=(ReadingData left, ReadingData right)
					{
						return !left.Equals(right);
					}
				}
				/// <summary>
				/// Populates the Readings List with ReadingData Structure Types 
				/// </summary>
				private void PopulateBranchData()
				{
					myReadings.Clear();
					LinkedElementCollection<RoleBase> roleCollection = myReadingOrder.RoleCollection;
					int numReadings = myReadingMC.Count;
					for (int i = 0; i < numReadings; ++i)
					{
						myReadings.Add(new ReadingData(FactType.PopulatePredicateText(myReadingMC[i], null, null, roleCollection, myReadingInformation.OrderedReplacementFields), myReadingMC[i]));
					}
				}
				#endregion //Branch Helper Methods
			}
			#endregion
		}
		#endregion Nested ReadingOrderBranch class

		#region nested class ReadingVirtualTree
		private sealed class ReadingVirtualTree : StandardMultiColumnTree, IMultiColumnTree
		{
			public ReadingVirtualTree(IBranch root)
				: base(ReadingOrderBranch.COLUMN_COUNT)
			{
				Debug.Assert(root != null);
				this.Root = root;
			}
		}
		#endregion

		#region nested class CustomVirtualTreeControl
		/// <summary>
		/// This custom class is needed to override the height of column headers to provide 
		/// the Appearance of NO Headers when column permutation is used. 
		/// If column headers are not used when column permutation is used an "index out of range exception" 
		/// is fired when the mouse travels over the region occupied by the scrollbar if no scrollbar exists.
		/// </summary>
		public class CustomVirtualTreeControl : StandardVirtualTreeControl
		{
			/// <summary>
			/// Needed for creating a header to prevent the vertical scrollbar from thowing an exception
			/// </summary>
			/// <returns></returns>
			protected override VirtualTreeHeaderControl CreateHeaderControl()
			{
				return new ZeroHeightHeader(this);
			}
			private sealed class ZeroHeightHeader : VirtualTreeHeaderControl
			{
				public ZeroHeightHeader(VirtualTreeControl associatedControl) : base(associatedControl) { }
				public sealed override int HeaderHeight
				{
					get
					{
						return 0;
					}
				}
			}
		}
		#endregion
	}
}
