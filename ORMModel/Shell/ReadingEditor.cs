#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
* Copyright © ORM Solutions, LLC. All rights reserved.                     *
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
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Framework.Design;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Core.ShapeModel;
using ORMSolutions.ORMArchitect.Core.Shell;
using ORMSolutions.ORMArchitect.Framework.Shell;

#if VISUALSTUDIO_9_0
using VirtualTreeInPlaceControlFlags = Microsoft.VisualStudio.VirtualTreeGrid.VirtualTreeInPlaceControls;
#endif //VISUALSTUDIO_9_0

namespace ORMSolutions.ORMArchitect.Core.Shell
{
	#region ReadingEditorCommands enum
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
	#endregion // ReadingEditorCommands enum
	#region ActiveFactType structure
	/// <summary>
	/// A structure to represent the currently active fact. Allows for
	/// a custom display order, such as the custom order available from
	/// a FactTypeShape.
	/// </summary>
	public struct ActiveFactType : IEquatable<ActiveFactType>
	{
		#region Static Fields
		/// <summary>
		/// Represents an empty ActiveFactType
		/// </summary>
		public static readonly ActiveFactType Empty = new ActiveFactType();
		#endregion // Static Fields
		#region Instance Fields
		private readonly FactType myFactType;
		private readonly FactType myImpliedFactType;
		private readonly IList<RoleBase> myDisplayOrder;
		#endregion // Instance Fields
		#region Constructors
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
		#endregion // Constructors
		#region Accessor Properties
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
		#endregion // Accessor Properties
		#region Equality Methods
		/// <summary>
		/// Equals operator override
		/// </summary>
		public static bool operator ==(ActiveFactType factType1, ActiveFactType factType2)
		{
			return factType1.Equals(factType2);
		}
		/// <summary>
		/// Not equals operator override
		/// </summary>
		public static bool operator !=(ActiveFactType factType1, ActiveFactType factType2)
		{
			return !(factType1.Equals(factType2));
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
			FactType factType = myFactType;
			int hashCode = 0;
			if (factType != null)
			{
				hashCode = factType.GetHashCode();
				IList<RoleBase> order = myDisplayOrder;
				int count = order.Count;
				for (int i = 0; i < count; ++i)
				{
					hashCode ^= Utility.RotateRight(order[i].GetHashCode(), i);
				}
				factType = myImpliedFactType;
				if (factType != null)
				{
					hashCode ^= Utility.RotateRight(factType.GetHashCode(), (count == 0) ? 1 : count);
				}
			}
			return hashCode;
		}
		#endregion // Equality Methods
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
		#endregion // Reading role order collection helper
	}
	#endregion // ActiveFactType structure
	public partial class ReadingEditor : UserControl
	{
		#region InplaceReadingEditor control
		private sealed class InplaceReadingEditor : ReadingRichTextBox, IVirtualTreeInPlaceControl, INotifyEscapeKeyPressed
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
			private EventHandler myEscapePressed;
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
			#endregion // InplaceReadingEditor Specific
			#region Event Forwarding, etc (Copied from VirtualTreeGrid.VirtualTreeInPlaceEditControl)
			protected override bool IsInputKey(Keys keyData)
			{
				if ((keyData & Keys.KeyCode) == Keys.Escape)
				{
					EventHandler escapePressed;
					if (null != (escapePressed = myEscapePressed))
					{
						escapePressed(this, EventArgs.Empty);
					}
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
			#region INotifyEscapeKeyPressed Implementation
			event EventHandler INotifyEscapeKeyPressed.EscapePressed
			{
				add { myEscapePressed += value; }
				remove { myEscapePressed -= value; }
			}
			#endregion // INotifyEscapeKeyPressed Implementation
		}
		#endregion // Inplace reading editor control
		#region BaseBranch class
		/// <summary>
		/// An empty IBranch implementation
		/// </summary>
		private abstract class BaseBranch : IBranch
		{
			#region IBranch Implementation
			VirtualTreeLabelEditData IBranch.BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
			{
				return VirtualTreeLabelEditData.Invalid;
			}
			LabelEditResult IBranch.CommitLabelEdit(int row, int column, string newText)
			{
				return LabelEditResult.CancelEdit;
			}
			BranchFeatures IBranch.Features
			{
				get
				{
					return BranchFeatures.None;
				}
			}
			VirtualTreeAccessibilityData IBranch.GetAccessibilityData(int row, int column)
			{
				return VirtualTreeAccessibilityData.Empty;
			}
			VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
			{
				return VirtualTreeDisplayData.Empty;
			}
			object IBranch.GetObject(int row, int column, ObjectStyle style, ref int options)
			{
				return null;
			}
			string IBranch.GetText(int row, int column)
			{
				Debug.Fail("Should override");
				return null;
			}
			string IBranch.GetTipText(int row, int column, ToolTipType tipType)
			{
				return null;
			}
			bool IBranch.IsExpandable(int row, int column)
			{
				return false;
			}
			LocateObjectData IBranch.LocateObject(object obj, ObjectStyle style, int locateOptions)
			{
				return default(LocateObjectData);
			}
			event BranchModificationEventHandler IBranch.OnBranchModification
			{
				add
				{
				}
				remove
				{
				}
			}
			void IBranch.OnDragEvent(object sender, int row, int column, DragEventType eventType, DragEventArgs args)
			{
			}
			void IBranch.OnGiveFeedback(GiveFeedbackEventArgs args, int row, int column)
			{
			}
			void IBranch.OnQueryContinueDrag(QueryContinueDragEventArgs args, int row, int column)
			{
			}
			VirtualTreeStartDragData IBranch.OnStartDrag(object sender, int row, int column, DragReason reason)
			{
				return VirtualTreeStartDragData.Empty;
			}
			StateRefreshChanges IBranch.ToggleState(int row, int column)
			{
				return StateRefreshChanges.None;
			}
			StateRefreshChanges IBranch.SynchronizeState(int row, int column, IBranch matchBranch, int matchRow, int matchColumn)
			{
				return StateRefreshChanges.None;
			}
			int IBranch.UpdateCounter
			{
				get
				{
					return 0;
				}
			}
			int IBranch.VisibleItemCount
			{
				get
				{
					Debug.Fail("Should override");
					return 0;
				}
			}
			#endregion // IBranch Implementation
		}
		#endregion // BaseBranch class
		#region RootBranch class
		/// <summary>
		/// A branch that can be a root branch in a the tree
		/// </summary>
		private abstract class RootBranch : BaseBranch
		{
			#region Virtual and Abstract Methods
			public virtual ReadingEditorCommands SupportedSelectionCommands(int itemLocation)
			{
				return ReadingEditorCommands.None;
			}
			/// <summary>
			/// Initiates a New Reading within the selected Reading Order
			/// </summary>
			/// <param name="itemLocation">Reading Order Row</param>
			public virtual void AddNewReading(int itemLocation)
			{
			}
			/// <summary>
			/// Initiates the Drop Down to select a new reading order for the reading to add
			/// </summary>
			public virtual void AddNewReadingOrder()
			{
			}
			/// <summary>
			/// Moves the selected Reading Down
			/// </summary>
			public virtual void DemoteSelectedReading(int readingItemLocation, int orderItemLocation)
			{
			}
			/// <summary>
			/// Moves the selected ReadingOrder Down
			/// </summary>
			/// <param name="itemLocation">location</param>
			public virtual void DemoteSelectedReadingOrder(int itemLocation)
			{
			}
			/// <summary>
			/// Initiate edit for first reading within the <see cref="LinkedElementCollection{RoleBase}"/> (will create a new item if needed)
			/// </summary>
			/// <param name="collection">The <see cref="LinkedElementCollection{RoleBase}"/> to use</param>
			public virtual void EditReadingOrder(IList<RoleBase> collection)
			{
			}
			/// <summary>
			/// Moves the selected Reading Up
			/// </summary>
			public virtual void PromoteSelectedReading(int readingItemLocation, int orderItemLocation)
			{
			}
			/// <summary>
			/// Moves the selected ReadingOrder up
			/// </summary>
			/// <param name="itemLocation">location</param>
			public virtual void PromoteSelectedReadingOrder(int itemLocation)
			{
			}
			/// <summary>
			/// Removes the item selected when called by the context menu
			/// </summary>
			public virtual void RemoveSelectedItem()
			{
			}
			/// <summary>
			/// Instruct the readingbranch that a reading has been added to the collection
			/// </summary>
			/// <param name="reading">the reading to add</param>
			public virtual void OnReadingAdded(Reading reading)
			{
			}
			/// <summary>
			/// Event callback from Changing the Order of  and item in the ReadingOrder LinkedElementCollection: 
			/// will update the branch to reflect the changed order
			/// </summary>
			/// <param name="reading">The Reading moved</param>
			public virtual void OnReadingLocationUpdated(Reading reading)
			{
			}
			/// <summary>
			/// Event callback from Changing the Order of  and item in the ReadingOrdersMovableCollectoin: 
			/// will update the branch to reflect the changed order
			/// </summary>
			/// <param name="order">The ReadingOrder affected</param>
			public virtual void OnReadingOrderLocationUpdated(ReadingOrder order)
			{
			}
			/// <summary>
			/// Handles removing a ReadingOrder which has been deleted
			/// </summary>
			/// <param name="order">The ReadingOrder which has been removed</param>
			/// <param name="factType">The Fact for the ReadingOrder</param>
			public virtual void OnReadingOrderRemoved(ReadingOrder order, FactType factType)
			{
			}
			/// <summary>
			/// Triggers notification that a Reading has been removed from the branch.
			/// </summary>
			/// <param name="reading">The Reading which has been removed</param>
			/// <param name="order">The order of the link</param>
			public virtual void OnReadingRemoved(Reading reading, ReadingOrder order)
			{
			}
			/// <summary>
			/// Triggers the events notifying the tree that a Reading in the Readingbranch has been updated. 
			/// </summary>
			/// <param name="reading">The Reading affected</param>
			public virtual void OnReadingUpdated(Reading reading)
			{
			}
			/// <summary>
			/// An extension property on a reading has changed. Notify the branches.
			/// </summary>
			/// <param name="reading">The Reading affected</param>
			/// <param name="descriptor">The static <see cref="PropertyDescriptor"/> that describes the change.</param>
			public virtual void OnExtensionPropertyChanged(Reading reading, PropertyDescriptor descriptor)
			{
			}
			#endregion // Virtual Methods
		}
		#endregion // RootBranch class
		#region Enums
		private enum ColumnIndex
		{
			ReadingOrder = 0,
			ReadingBranch = 1,
			FirstPropertyBranch = 2,
		}
		private enum RowType
		{
			/// <summary>
			/// The reading order is in the model
			/// </summary>
			Committed = 0,
			/// <summary>
			/// The reading order is not in the model
			/// </summary>
			Uncommitted = 1,
			/// <summary>
			/// The New reading row
			/// </summary>
			New = 3,
			/// <summary>
			/// Unknown row type
			/// </summary>
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
		#endregion // Static Properties
		#region Static Variables
		/// <summary>
		/// Provides a ref to the ReadingOrderBranch from nested objects
		/// </summary>
		private RootBranch myMainBranch;
		/// <summary>
		/// Provides a ref to the Reading Editorl from nested objects
		/// </summary>
		private static ReadingEditor myInstance;
		/// <summary>
		/// Cache the extension property descriptors in the Store.PropertyBag
		/// </summary>
		private const string StorePropertyDescriptorsKey = "ORMReadingEditor.PropertyDescriptors";
		/// <summary>
		/// Cache column header settings in the Store.PropertyBag
		/// </summary>
		private const string StoreColumnHeadersKey = "ORMReadingEditor.ColumnHeaders";
		/// <summary>
		/// Cache column permutations in the Store.PropertyBag
		/// </summary>
		private const string StoreColumnPermutationKey = "ORMReadingEditor.ColumnPermutation";
		/// <summary>
		/// The number of non-extended columns
		/// </summary>
		private const int BASE_COLUMN_COUNT = 2;
		#endregion // Static Variables
		#region Member Variables
		private ORMReadingEditorToolWindow myToolWindow;
		private FactType myFactType;
		private FactType mySecondaryFactType;
		private IList<RoleBase> myDisplayRoleOrder;
		private ImageList myImageList;
		private ReadingEditorCommands myVisibleCommands;
		private bool myInEvents;
		private bool myWideHeader; // The 'wideHeader' parameter was set in the last call to SetHeaders
		private bool myExpandImpliedBranch; // True if the implied fact types branch should be expanded automatically
		private PropertyDescriptorCollection myExtensionProperties;
		#endregion // Member Variables
		#region Constructor
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
		private void SetHeaders(bool storeChanged, bool wideHeader)
		{
			if (storeChanged || (wideHeader != myWideHeader))
			{
				CustomVirtualTreeControl treeControl = this.vtrReadings;

				VirtualTreeColumnHeader[] headers;
				ColumnPermutation permutation = null;
				object headersObject;
				bool computePercentages = false;
				if (storeChanged)
				{
					Dictionary<object, object> bag = myFactType.Store.PropertyBag;
					if (!bag.TryGetValue(StoreColumnHeadersKey, out headersObject) ||
						null == (headers = headersObject as VirtualTreeColumnHeader[]))
					{
						PropertyDescriptorCollection extensionProperties = myExtensionProperties;
						int propertyCount = extensionProperties != null ? extensionProperties.Count : 0;
						headers = new VirtualTreeColumnHeader[BASE_COLUMN_COUNT + propertyCount];
						headers[1] = new VirtualTreeColumnHeader(ResourceStrings.ModelReadingEditorColumnHeaderReadings, 1f, 100, VirtualTreeColumnHeaderStyles.ColumnPositionLocked);
						for (int i = 0; i < propertyCount; ++i)
						{
							headers[i + 2] = new VirtualTreeColumnHeader(extensionProperties[i].DisplayName, 1f, VirtualTreeColumnHeaderStyles.Default);
						}
						computePercentages = true;
					}
					object permutationObject;
					if (bag.TryGetValue(StoreColumnPermutationKey, out permutationObject))
					{
						permutation = permutationObject as ColumnPermutation;
					}
					myWideHeader = wideHeader;
				}
				else
				{
					headers = treeControl.GetColumnHeaders();
					permutation = treeControl.ColumnPermutation;
					myWideHeader = wideHeader;
				}
				int indent = treeControl.IndentWidth + 3;
				if (wideHeader)
				{
					indent += indent;
				}

				headers[0] = new VirtualTreeColumnHeader(" ", indent + treeControl.ImageList.ImageSize.Width, true, VirtualTreeColumnHeaderStyles.ColumnPositionLocked);
				ColumnPermutation currentPermutation = treeControl.ColumnPermutation;
				if (currentPermutation != null)
				{
					if (currentPermutation == permutation)
					{
						permutation = null;
					}
					else
					{
						// Clear before setting headers so that we don't have to worry about a count mismatch
						treeControl.ColumnPermutation = null;
					}
				}
				treeControl.SetColumnHeaders(headers, computePercentages);
				if (permutation != null)
				{
					treeControl.ColumnPermutation = permutation;
				}
				bool extraColumns = headers.Length > 2;
				treeControl.HasVerticalGridLines = extraColumns;
				treeControl.HeaderDragDrop = extraColumns;
				if (treeControl.HeaderControl.HeaderHeight == 0 ? extraColumns : !extraColumns)
				{
					// Force a recreate of the header control
					treeControl.DisplayColumnHeaders = false;
					treeControl.DisplayColumnHeaders = true;
				}
			}
		}
		#endregion // Constructor
		#region Properties
		/// <summary>
		/// The fact that is being edited in the control, or that needs to be edited.
		/// </summary>
		public ActiveFactType EditingFactType
		{
			get
			{
				return new ActiveFactType(myFactType, mySecondaryFactType, myDisplayRoleOrder);
			}
			set
			{
				// Get the current store settings
				FactType factType = myFactType;
				Store previousStore = factType != null ? Utility.ValidateStore(factType.Store) : null;

				// Check if the implied fact type branch is expanded so we can restore
				// the current expansion settings for a new branch.
				factType = mySecondaryFactType;
				ITree tree;
				VirtualTreeCoordinate coordinate;
				if (null != factType &&
					null != (tree = vtrReadings.Tree) &&
					(coordinate = tree.LocateObject(null, factType, (int)ObjectStyle.TrackingObject, 1 /* Special flag to change behavior */)).IsValid)
				{
					myExpandImpliedBranch = tree.IsExpanded(coordinate.Row, coordinate.Column);
				}

				// Get the new information
				myFactType = factType = value.FactType;
				mySecondaryFactType = value.ImpliedFactType;
				myDisplayRoleOrder = value.DisplayOrder;
				if (factType != null)
				{
					Store currentStore = factType.Store;
					if (currentStore != previousStore)
					{
						RememberDisplaySettings(previousStore);
						object extensionPropertiesObject;
						PropertyDescriptorCollection extensionProperties;
						if (!currentStore.PropertyBag.TryGetValue(StorePropertyDescriptorsKey, out extensionPropertiesObject) ||
							null == (extensionProperties = extensionPropertiesObject as PropertyDescriptorCollection))
						{
							extensionProperties = new PropertyDescriptorCollection(new PropertyDescriptor[0]);
							((IFrameworkServices)currentStore).PropertyProviderService.GetProvidedProperties(typeof(Reading), extensionProperties);
							currentStore.PropertyBag[StorePropertyDescriptorsKey] = extensionProperties;
						}
						myExtensionProperties = extensionProperties;
						PopulateControl(true);
					}
					else
					{
						PopulateControl(false);
					}
				}
				else
				{
					if (previousStore != null)
					{
						RememberDisplaySettings(previousStore);
					}
					myExtensionProperties = null;
				}
			}
		}
		/// <summary>
		/// Write all control display settings to the <see cref="Store.PropertyBag"/>
		/// </summary>
		private void RememberDisplaySettings(Store store)
		{
			if (null != store)
			{
				VirtualTreeControl treeControl = vtrReadings;
				Dictionary<object, object> bag = store.PropertyBag;
				bag[StoreColumnHeadersKey] = treeControl.GetColumnHeaders();
				bag[StoreColumnPermutationKey] = treeControl.ColumnPermutation;
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
				VirtualTreeControl treeControl = vtrReadings;
				ITree tree = treeControl.Tree;
				int currentIndex = treeControl.CurrentIndex;
				if (currentIndex >= 0)
				{
					VirtualTreeItemInfo itemInfo = tree.GetItemInfo(currentIndex, treeControl.CurrentColumn, false);
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
		#endregion // Properties
		#region Tree Events
		private void Tree_LabelEditControlChanged(object sender, EventArgs e)
		{
			myToolWindow.ActiveInPlaceEditWindow = vtrReadings.LabelEditControl;
		}
		#endregion // Tree Events
		#region PopulateControl and helpers
		/// <summary>
		/// Populate the control for the current FactType
		/// </summary>
		/// <param name="storeChanged">Set if the <see cref="Store"/> has changed since the
		/// previous calculation.</param>
		private void PopulateControl(bool storeChanged)
		{
			Debug.Assert(myFactType != null);
			if (myDisplayRoleOrder != null)
			{
				LinkedElementCollection<RoleBase> roles = myFactType.RoleCollection;
				if ((FactType.GetUnaryRoleIndex(roles).HasValue ? 1 : roles.Count) != myDisplayRoleOrder.Count)
				{
					myDisplayRoleOrder = null;
				}
			}

			VirtualTreeControl treeControl = vtrReadings;

			PropertyDescriptorCollection extensionProperties = myExtensionProperties;
			int extensionPropertyCount = extensionProperties != null ? extensionProperties.Count : 0;
			bool recreateTree = true;
			ITree oldTree = treeControl.MultiColumnTree as ITree;
			// Turn off all event handlers for old branches whenever we repopulate
			bool turnedDelayRedrawOff = false;
			if (oldTree != null)
			{
				oldTree.Root = null;
				if (((IMultiColumnTree)oldTree).ColumnCount == (BASE_COLUMN_COUNT + extensionPropertyCount))
				{
					if (myInEvents)
					{
						turnedDelayRedrawOff = true;
						oldTree.DelayRedraw = false;
					}
				}
				else
				{
					// The tree and header count have to match, or the tree must be null
					treeControl.Tree = null;
					recreateTree = true;
				}
			}

			object expandToObject = null;
			if (mySecondaryFactType != null)
			{
				this.SetHeaders(storeChanged, true);
				myMainBranch = new FactTypeBranch(this);
				if (myExpandImpliedBranch)
				{
					expandToObject = mySecondaryFactType;
				}
			}
			else
			{
				this.SetHeaders(storeChanged, false);
				myMainBranch = new ReadingOrderBranch(this, myFactType, myDisplayRoleOrder);
			}
			ITree tree;
			if (recreateTree)
			{
				treeControl.MultiColumnTree = new ReadingVirtualTree(myMainBranch, extensionPropertyCount);
				tree = treeControl.Tree;
			}
			else
			{
				tree = treeControl.Tree;
				tree.Root = myMainBranch;
			}
			if (turnedDelayRedrawOff)
			{
				tree.DelayRedraw = true;
			}

			VirtualTreeCoordinate coordinate;
			if (expandToObject != null &&
				(coordinate = tree.LocateObject(null, expandToObject, (int)ObjectStyle.TrackingObject, 1 /* Special flag to change behavior */)).IsValid)
			{
				tree.ToggleExpansion(coordinate.Row, coordinate.Column);
			}
			if (tree.IsExpandable(0, 0))
			{
				tree.ToggleExpansion(0, 0); //expand ReadingOrderBranch by default
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
		#endregion // PopulateControl and helpers
		#region Reading role order collection helper
		private static IList<RoleBase> GetReadingRoleCollection(FactType factType)
		{
			// Return a single-element collection for binarized unaries
			Role unaryRole = factType.UnaryRole;
			if (unaryRole != null)
			{
				return new RoleBase[] { unaryRole };
			}
			else
			{
				return factType.RoleCollection;
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
				&& (factType == myFactType || factType == mySecondaryFactType))
			{
				if (TreeControl.SelectObject(null, reading, (int)ObjectStyle.TrackingObject, 0))
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
		/// <param name="factType">FactType</param>
		public void ActivateReading(FactType factType)
		{
			if (factType == myFactType || factType == mySecondaryFactType)
			{
				myMainBranch.EditReadingOrder(GetReadingRoleCollection(factType));
			}
		}
		/// <summary>
		/// Puts the currently selected item into edit mode.
		/// </summary>
		public void EditSelection()
		{
			vtrReadings.BeginLabelEdit();
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
					myVisibleCommands = ((RootBranch)itemInfo.Branch).SupportedSelectionCommands(itemInfo.Row);
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
			MenuCommand command = (MenuCommand)sender;
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
				((RootBranch)itemInfo.Branch).AddNewReading(itemInfo.Row);
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
				((RootBranch)itemInfo.Branch).AddNewReadingOrder();
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
				((RootBranch)itemInfo.Branch).PromoteSelectedReading(readingItemRow, itemInfo.Row);
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
				((RootBranch)itemInfo.Branch).DemoteSelectedReading(readingItemRow, itemInfo.Row);
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
				((RootBranch)itemInfo.Branch).PromoteSelectedReadingOrder(itemInfo.Row);
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
				((RootBranch)itemInfo.Branch).DemoteSelectedReadingOrder(itemInfo.Row);
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
				((RootBranch)itemInfo.Branch).RemoveSelectedItem();
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
		#endregion  // Tree Context Menu Methods
		#region Model Event Handlers
		#region Attach/detach events
		/// <summary>
		/// Manages <see cref="EventHandler{TEventArgs}"/>s in the <see cref="Store"/> so that the <see cref="ORMReadingEditorToolWindow"/>
		/// contents can be updated to reflect any model changes.
		/// </summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/>s should be managed.</param>
		/// <param name="eventManager">The <see cref="ModelingEventManager"/> used to manage the <see cref="EventHandler{TEventArgs}"/>s.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>s.</param>
		public void ManageEventHandlers(Store store, ModelingEventManager eventManager, EventHandlerAction action)
		{
			// Remove extension listeners regardless of store state. These listeners are statically implemented and
			// will continue to notify the reading editor on changes in other stores if they are not removed.
			((IFrameworkServices)store).PropertyProviderService.AddOrRemoveChangeListener(typeof(Reading), ExtensionPropertyChangedEvent, action);

			if (Utility.ValidateStore(store) == null)
			{
				return; // Bail out
			}

			DomainDataDirectory dataDirectory = store.DomainDataDirectory;
			DomainClassInfo classInfo = dataDirectory.FindDomainRelationship(ReadingOrderHasReading.DomainClassId);

			// Track Reading changes
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(ReadingLinkAddedEvent), action);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(ReadingLinkRemovedEvent), action);

			classInfo = dataDirectory.FindDomainClass(Reading.DomainClassId);
			DomainPropertyInfo propertyInfo = dataDirectory.FindDomainProperty(Reading.TextDomainPropertyId);
			eventManager.AddOrRemoveHandler(classInfo, propertyInfo, new EventHandler<ElementPropertyChangedEventArgs>(ReadingAttributeChangedEvent), action);

			// Track ReadingOrder changes
			classInfo = dataDirectory.FindDomainRelationship(FactTypeHasReadingOrder.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(ReadingOrderLinkRemovedEvent), action);

			// Track FactType Role changes
			classInfo = dataDirectory.FindDomainRelationship(FactTypeHasRole.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(FactTypeHasRoleAddedOrDeletedEvent), action);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(FactTypeHasRoleAddedOrDeletedEvent), action);

			// Track role player changes
			classInfo = dataDirectory.FindDomainRelationship(ObjectTypePlaysRole.DomainClassId);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(RolePlayerAddedOrDeletedEvent), action);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(RolePlayerAddedOrDeletedEvent), action);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<RolePlayerChangedEventArgs>(RolePlayerChangedEvent), action);
			classInfo = dataDirectory.FindDomainClass(ObjectType.DomainClassId);
			propertyInfo = dataDirectory.FindDomainProperty(ObjectType.NameDomainPropertyId);
			eventManager.AddOrRemoveHandler(classInfo, propertyInfo, new  EventHandler<ElementPropertyChangedEventArgs>(RolePlayerNameChangedEvent), action);

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
		#endregion // Attach/detach events
		#region Pre/Post Event Handlers
		private void ElementEventsBegunEvent(object sender, ElementEventsBegunEventArgs e)
		{
			myInEvents = false; // Sanity, should not be needed
			if (myFactType != null)
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
		#endregion // Pre/Post Event Handlers
		#region Reading Event Handlers
		//handling model events Related to changes in Readings and their
		//connections so the reading editor can accurately reflect the model
		private void ReadingLinkAddedEvent(object sender, ElementAddedEventArgs e)
		{
			FactType selectedFactType;
			if (null == (selectedFactType = myFactType))
			{
				return;
			}
			ReadingOrderHasReading link = e.ModelElement as ReadingOrderHasReading;
			if (!link.IsDeleted)
			{
				ReadingOrder readingOrder = link.ReadingOrder;
				FactType factType = readingOrder.FactType;
				if (factType == selectedFactType || factType == mySecondaryFactType)
				{
					myMainBranch.OnReadingAdded(link.Reading);
				}
				this.UpdateMenuItems();
			}
		}
		private void ReadingLinkRemovedEvent(object sender, ElementDeletedEventArgs e)
		{
			FactType selectedFactType;
			if (null == (selectedFactType = myFactType))
			{
				return;
			}
			ReadingOrderHasReading link = (ReadingOrderHasReading)e.ModelElement;
			ReadingOrder readingOrder = link.ReadingOrder;
			// Handled all at once by ReadingOrderLinkRemovedEvent if all are gone.
			if (!readingOrder.IsDeleted)
			{
				FactType factType = readingOrder.FactType;
				if (factType == selectedFactType || factType == mySecondaryFactType)
				{
					myMainBranch.OnReadingRemoved(link.Reading, readingOrder); //UNDONE: use interface and locate object
				}
			}
		}
		private void ReadingAttributeChangedEvent(object sender, ElementPropertyChangedEventArgs e)
		{
			FactType selectedFactType;
			if (null == (selectedFactType = myFactType))
			{
				return;
			}
			Reading reading = (Reading)e.ModelElement;
			ReadingOrder readingOrder;
			FactType factType;
			if (!(reading = (Reading)e.ModelElement).IsDeleted &&
				!reading.IsDeleting &&
				null != (readingOrder = reading.ReadingOrder) &&
				((factType = readingOrder.FactType) == selectedFactType || factType == mySecondaryFactType))
			{
				myMainBranch.OnReadingUpdated(reading);
			}
		}
		private void ReadingPositionChangedHandler(object sender, RolePlayerOrderChangedEventArgs e)
		{
			if (myFactType == null)
			{
				return;
			}
			myMainBranch.OnReadingLocationUpdated((Reading)e.CounterpartRolePlayer);
			this.UpdateMenuItems();
		}
		#endregion // Reading Event Handlers
		#region ReadingOrder Event Handlers
		//handle model events related to the ReadingOrder or its Roles being removed in order to
		//keep the editor window in sync with what is in the model.
		private void ReadingOrderLinkRemovedEvent(object sender, ElementDeletedEventArgs e)
		{
			FactTypeHasReadingOrder link = (FactTypeHasReadingOrder)e.ModelElement;
			FactType selectedFactType;
			FactType factType;
			if (null != (selectedFactType = myFactType) &&
				((factType = link.FactType) == selectedFactType || factType == mySecondaryFactType) &&
				!factType.IsDeleting &&
				!factType.IsDeleted)
			{
				myMainBranch.OnReadingOrderRemoved(link.ReadingOrder, factType);
			}
		}
		private void ReadingOrderPositionChangedHandler(object sender, RolePlayerOrderChangedEventArgs e)
		{
			if (myFactType == null)
			{
				return;
			}
			myMainBranch.OnReadingOrderLocationUpdated((ReadingOrder)e.CounterpartRolePlayer);
			this.UpdateMenuItems();
		}
		#endregion // ReadingOrder Event Handlers
		#region FactType Event Handlers
		private void FactTypeHasRoleAddedOrDeletedEvent(object sender, ElementEventArgs e)
		{
			FactType selectedFactType;
			if (null != (selectedFactType = myFactType) &&
				selectedFactType == ((FactTypeHasRole)e.ModelElement).FactType &&
				!selectedFactType.IsDeleted &&
				!selectedFactType.IsDeleting)
			{
				this.PopulateControl(false);
			}
		}
		private void FactTypeRemovedEvent(object sender, ElementDeletedEventArgs e)
		{
			ModelHasFactType link = (ModelHasFactType)e.ModelElement;
			FactType selectedFactType;
			if (null != (selectedFactType = myFactType))
			{
				FactType deletedFactType = link.FactType;
				if (deletedFactType == selectedFactType)
				{
					ORMDesignerPackage.ReadingEditorWindow.EditingFactType = ActiveFactType.Empty;
					if (!selectedFactType.IsDeleted && !selectedFactType.IsDeleting)
					{
						this.PopulateControl(false);
					}
					else
					{
						ITree currentTree = this.vtrReadings.Tree;
						if (currentTree != null)
						{
							currentTree.Root = null;
						}
					}
				}
			}
		}
		private void RolePlayerAddedOrDeletedEvent(object sender, ElementEventArgs e)
		{
			Role role;
			FactType selectedFactType;
			if (null != (selectedFactType = myFactType) &&
				!selectedFactType.IsDeleted &&
				!selectedFactType.IsDeleting &&
				!(role = ((ObjectTypePlaysRole)e.ModelElement).PlayedRole).IsDeleted &&
				!role.IsDeleting &&
				role.FactType == selectedFactType)
			{
				this.PopulateControl(false);
			}
		}
		private void RolePlayerChangedEvent(object sender, RolePlayerChangedEventArgs e)
		{
			Role role;
			FactType selectedFactType;
			if (e.DomainRole.Id == ObjectTypePlaysRole.RolePlayerDomainRoleId &&
				null != (selectedFactType = myFactType) &&
				!selectedFactType.IsDeleted &&
				!selectedFactType.IsDeleting &&
				!(role = ((ObjectTypePlaysRole)e.ElementLink).PlayedRole).IsDeleted &&
				!role.IsDeleting &&
				role.FactType == selectedFactType)
			{
				this.PopulateControl(false);
			}
		}
		private void RolePlayerNameChangedEvent(object sender, ElementPropertyChangedEventArgs e)
		{
			// Update the control if a displayed role player changed or if the name of the current
			// objectification changed.
			ObjectType rolePlayer;
			FactType selectedFactType;
			if (null != (selectedFactType = myFactType) &&
				!(rolePlayer = (ObjectType)e.ModelElement).IsDeleted &&
				!rolePlayer.IsDeleting)
			{
				bool updateRequired = false;
				if (mySecondaryFactType != null &&
					rolePlayer == selectedFactType.NestingType)
				{
					updateRequired = true;
				}
				else
				{
					foreach (RoleBase roleBase in selectedFactType.RoleCollection)
					{
						if (roleBase.Role.RolePlayer == rolePlayer)
						{
							updateRequired = true;
							break;
						}
					}
				}
				if (updateRequired)
				{
					this.PopulateControl(false);
				}
			}
		}
		#endregion // FactType Event Handlers
		#region Extension Property Handlers
		/// <summary>
		/// A reading extension property has changed.
		/// </summary>
		private void ExtensionPropertyChangedEvent(object sender, ExtensionPropertyChangedEventArgs e)
		{
			if (myFactType == null)
			{
				return;
			}
			Reading reading = e.Instance as Reading;
			if (reading != null)
			{
				myMainBranch.OnExtensionPropertyChanged(reading, e.Descriptor);
			}
		}
		#endregion // Extension Property Handlers
		#endregion // Model Event Handlers
		#region FactTypeBranch class
		private sealed class FactTypeBranch : RootBranch, IBranch
		{
			#region Constants
			private const int OrderBranchRow = 0;
			private const int ImpliedBranchRow = 1;
			#endregion // Constants
			#region Member Variables
			private ReadingOrderBranch myReadingOrderBranch;
			private ReadingOrderBranch myImpliedFactTypeBranch;
			private ReadingEditor myEditor;
			#endregion // Member Variables
			#region Constructor
			public FactTypeBranch(ReadingEditor editor)
			{
				myEditor = editor;
			}
			#endregion // Constructor
			#region Accessor Properties
			private FactType FactType
			{
				get
				{
					return myEditor.myFactType;
				}
			}
			private FactType SecondaryFactType
			{
				get
				{
					return myEditor.mySecondaryFactType;
				}
			}
			#endregion // Accessor Properties
			#region IBranch Implementation
			BranchFeatures IBranch.Features
			{
				get
				{
					return BranchFeatures.Expansions;
				}
			}
			VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
			{
				VirtualTreeDisplayData retval = VirtualTreeDisplayData.Empty;
				retval.GrayText = true;
				return retval;
			}
			object IBranch.GetObject(int row, int column, ObjectStyle style, ref int options)
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
			string IBranch.GetText(int row, int column)
			{
				return (row == OrderBranchRow) ? ResourceStrings.ModelReadingEditorPrimaryFactTypeReadingsText : ResourceStrings.ModelReadingEditorImpliedFactTypeReadingsText;
			}
			bool IBranch.IsExpandable(int row, int column)
			{
				return true;
			}
			LocateObjectData IBranch.LocateObject(object obj, ObjectStyle style, int locateOptions)
			{
				switch (style)
				{
					case ObjectStyle.TrackingObject:
						Reading reading;
						ReadingOrder order;
						FactType factType = null;
						if (null != (reading = (obj as Reading)))
						{
							order = reading.ReadingOrder;
							factType = order.FactType;
						}
						else
						{
							factType = (obj as FactType);
							if (locateOptions != 0)
							{
								// Special request to get the fact type branch itself, not the first reading
								if (factType == FactType)
								{
									return new LocateObjectData(OrderBranchRow, 0, (int)TrackingObjectAction.ThisLevel);
								}
								else if (factType == SecondaryFactType)
								{
									return new LocateObjectData(ImpliedBranchRow, 0, (int)TrackingObjectAction.ThisLevel);
								}
								break;
							}
						}

						if (factType == FactType)
						{
							return new LocateObjectData(OrderBranchRow, 0, (int)TrackingObjectAction.NextLevel);
						}
						else if (factType == SecondaryFactType)
						{
							return new LocateObjectData(ImpliedBranchRow, 0, (int)TrackingObjectAction.NextLevel);
						}
						break;
				}
				return new LocateObjectData();
			}
			int IBranch.VisibleItemCount
			{
				get
				{
					return 2;
				}
			}
			#endregion // IBranch Implementation
			#region Properties
			private ReadingOrderBranch OrderBranch
			{
				get
				{
					ReadingOrderBranch retVal = myReadingOrderBranch;
					if (retVal == null)
					{
						myReadingOrderBranch = retVal = new ReadingOrderBranch(myEditor, FactType, myEditor.myDisplayRoleOrder);
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
						myImpliedFactTypeBranch = retVal = new ReadingOrderBranch(myEditor, SecondaryFactType, null);
					}
					return retVal;
				}
			}
			#endregion // Properties
			#region Base overrides
			#region Reading Methods
			/// <summary>
			/// Get the existing child branch for the specified Reading, or null
			/// </summary>
			private ReadingOrderBranch GetChildBranch(Reading reading)
			{
				ReadingOrder order;
				if (null != (order = reading.ReadingOrder))
				{
					return GetChildBranch(order);
				}
				return null;
			}
			/// <summary>
			/// Get the existing child branch for the specified ReadingOrder, or null
			/// </summary>
			private ReadingOrderBranch GetChildBranch(ReadingOrder order)
			{
				FactType factType;
				if (null != (factType = order.FactType))
				{
					return GetChildBranch(factType);
				}
				return null;
			}
			/// <summary>
			/// Get the existing child branch for the specified FactType, or null
			/// </summary>
			private ReadingOrderBranch GetChildBranch(FactType factType)
			{
				if (factType == FactType)
				{
					return myReadingOrderBranch;
				}
				else if (factType == SecondaryFactType)
				{
					return myImpliedFactTypeBranch;
				}
				return null;
			}
			public override void OnReadingAdded(Reading reading)
			{
				ReadingOrderBranch notifyBranch;
				if (!reading.IsDeleted &&
					null != (notifyBranch = GetChildBranch(reading)))
				{
					notifyBranch.OnReadingAdded(reading);
				}
			}
			public override void OnReadingUpdated(Reading reading)
			{
				ReadingOrderBranch notifyBranch;
				if (!reading.IsDeleted &&
					null != (notifyBranch = GetChildBranch(reading)))
				{
					notifyBranch.OnReadingUpdated(reading);
				}
			}
			public override void OnReadingLocationUpdated(Reading reading)
			{
				ReadingOrderBranch notifyBranch;
				if (!reading.IsDeleted &&
					null != (notifyBranch = GetChildBranch(reading)))
				{
					notifyBranch.OnReadingLocationUpdated(reading);
				}
			}
			public override void OnReadingRemoved(Reading reading, ReadingOrder order)
			{
				ReadingOrderBranch notifyBranch;
				if (!order.IsDeleted &&
					null != (notifyBranch = GetChildBranch(order)))
				{
					notifyBranch.OnReadingRemoved(reading, order);
				}
			}
			public override void OnExtensionPropertyChanged(Reading reading, PropertyDescriptor descriptor)
			{
				ReadingOrderBranch notifyBranch;
				if (!reading.IsDeleted &&
					null != (notifyBranch = GetChildBranch(reading)))
				{
					notifyBranch.OnExtensionPropertyChanged(reading, descriptor);
				}
			}
			#endregion // Reading Methods
			#region ReadingOrder Methods
			public override void EditReadingOrder(IList<RoleBase> collection)
			{
				FactType matchFactType = collection[0].FactType;
				if (matchFactType == FactType)
				{
					OrderBranch.EditReadingOrder(collection);
				}
				else if (matchFactType == SecondaryFactType)
				{
					ImpliedBranch.EditReadingOrder(collection);
				}
			}
			public override void OnReadingOrderLocationUpdated(ReadingOrder order)
			{
				VirtualTreeControl control = ReadingEditor.Instance.TreeControl;
				((RootBranch)control.Tree.GetItemInfo(control.CurrentIndex, 0, true).Branch).OnReadingOrderLocationUpdated(order);
			}
			public override void OnReadingOrderRemoved(ReadingOrder order, FactType factType)
			{
				ReadingOrderBranch notifyBranch;
				if (!factType.IsDeleted &&
					null != (notifyBranch = GetChildBranch(factType)))
				{
					notifyBranch.OnReadingOrderRemoved(order, factType);
				}
			}
			#endregion // ReadingOrder Methods
			#endregion // Base overrides
		}
		#endregion // FactTypeBranch class
		#region ReadingOrderBranch class
		private sealed class ReadingOrderBranch : RootBranch, IBranch, IMultiColumnBranch
		{
			#region Member Variables
			private readonly FactType myFactType;
			private readonly ReadingOrderInformationCollection myReadingOrderKeyedCollection;
			private ReadingOrderInformationCollection myReadingOrderPermutations;
			private readonly IList<RoleBase> myRoleDisplayOrder;
			private string[] myRoleNames;
			private BranchModificationEventHandler myModify;
			private ReadingEditor myEditor;
			#endregion // Member Variables
			#region Constructor
			public ReadingOrderBranch(ReadingEditor editor, FactType factType, IList<RoleBase> roleDisplayOrder)
			{
				myEditor = editor;
				myFactType = factType;
				myReadingOrderKeyedCollection = new ReadingOrderInformationCollection();
				myRoleDisplayOrder = roleDisplayOrder != null && roleDisplayOrder.Count > 0 ? roleDisplayOrder : GetReadingRoleCollection(factType);
				this.PopulateReadingOrderInfo(-1); //Populate for all readings
			}
			#endregion // Constructor
			#region Branch Properties
			private IList<RoleBase> DisplayOrder
			{
				get
				{
					return myRoleDisplayOrder;
				}
			}
			private FactType FactType
			{
				get
				{
					return myFactType;
				}
			}
			private int ExtensionPropertyCount
			{
				get
				{
					PropertyDescriptorCollection properties = myEditor.myExtensionProperties;
					return properties != null ? properties.Count : 0;
				}
			}
			#endregion // Branch Properties
			#region IBranch Implementation
			VirtualTreeLabelEditData IBranch.BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
			{
				RowType rowType = myReadingOrderKeyedCollection.GetRowType(row);
				if (column == (int)ColumnIndex.ReadingBranch && rowType == RowType.New)
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
						myReadingOrderPermutations = new ReadingOrderInformationCollection();
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
					TypeEditorHost host = OnScreenTypeEditorHost.Create(new ReadingOrderDescriptor(myFactType, this, ResourceStrings.ModelReadingEditorNewItemText), myReadingOrderPermutations, TypeEditorHostEditControlStyle.TransparentEditRegion);
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
					bool escapePressed = false;
					EditorUtility.AttachEscapeKeyPressedEventHandler(
						editor,
						delegate(object sender, EventArgs e)
						{
							escapePressed = true;
						});
					return new VirtualTreeLabelEditData(
						editor,
						delegate(VirtualTreeItemInfo itemInfo, Control editControl)
						{
							Store store = myFactType.Store;
							if (escapePressed || !(store as IORMToolServices).CanAddTransaction)
							{
								return LabelEditResult.CancelEdit;
							}
							string newReadingText = editor.BuildReadingText();
							if (newReadingText.Length != 0)
							{
								using (Transaction t = store.TransactionManager.BeginTransaction(ResourceStrings.ModelReadingEditorNewReadingTransactionText))
								{
									t.TopLevelTransaction.Context.ContextInfo[ORMModel.BlockDuplicateReadingSignaturesKey] = null;
									Reading newReading = new Reading(store);
									newReading.ReadingOrder = myFactType.EnsureReadingOrder(myReadingOrderKeyedCollection[row].RoleOrder);
									newReading.Text = newReadingText;
									t.Commit();
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
			BranchFeatures IBranch.Features
			{
				get
				{
					return BranchFeatures.ExplicitLabelEdits | BranchFeatures.ImmediateSelectionLabelEdits | BranchFeatures.PositionTracking | BranchFeatures.DelayedLabelEdits | BranchFeatures.InsertsAndDeletes | BranchFeatures.ComplexColumns;
				}
			}
			VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
			{
				RowType rowType = myReadingOrderKeyedCollection.GetRowType(row);
				VirtualTreeDisplayData retval = VirtualTreeDisplayData.Empty;
				switch ((ColumnIndex)column)
				{
					case ColumnIndex.ReadingBranch:
						if (rowType == RowType.Uncommitted)
						{
							retval.GrayText = true;
						}
						break;
					case ColumnIndex.ReadingOrder:
						if (rowType != RowType.New)
						{
							retval.Image = 0;
							retval.SelectedImage = 0;   //you must set both .Image and .SelectedImage or an exception will be thrown
						}
						break;
				}
				return retval;
			}
			object IBranch.GetObject(int row, int column, ObjectStyle style, ref int options)
			{
				switch (style)
				{
					case ObjectStyle.SubItemRootBranch:
						if (myReadingOrderKeyedCollection.GetRowType(row) == RowType.Committed)
						{
							ReadingBranch readingBranch = myReadingOrderKeyedCollection[row].EnsureBranch();
							return (column == 1) ? readingBranch : readingBranch.EnsureExtensionPropertyBranch(column - BASE_COLUMN_COUNT);
						}
						break;
					case ObjectStyle.TrackingObject:
						if (row == myReadingOrderKeyedCollection.Count)
						{
							return new object(); //just need something which is not null
						}
						break;
					default:
						return null;
				}
				return null;
			}
			string IBranch.GetText(int row, int column)
			{
				if (column == (int)ColumnIndex.ReadingBranch)
				{
					switch (myReadingOrderKeyedCollection.GetRowType(row))
					{
						case RowType.New:
							return ResourceStrings.ModelReadingEditorNewItemText;
						case RowType.Uncommitted:
							return myReadingOrderKeyedCollection[row].Text;
					}
				}
				return null;
			}
			string IBranch.GetTipText(int row, int column, ToolTipType tipType)
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
			LocateObjectData IBranch.LocateObject(object obj, ObjectStyle style, int locateOptions)
			{
				if (style == ObjectStyle.TrackingObject)
				{
					ReadingOrderInformation info;
					LinkedElementCollection<RoleBase> roles;
					Reading reading;
					FactType factType;
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
					else if (null != (factType = (obj as FactType)))
					{
						return new LocateObjectData(0, (int)ColumnIndex.ReadingOrder, (int)TrackingObjectAction.ThisLevel);
					}
					else if (RowType.New.Equals(obj))
					{
						return new LocateObjectData(myReadingOrderKeyedCollection.Count, (int)ColumnIndex.ReadingBranch, (int)TrackingObjectAction.ThisLevel);
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
			event BranchModificationEventHandler IBranch.OnBranchModification
			{
				add { myModify += value; }
				remove { myModify -= value; }
			}
			int IBranch.VisibleItemCount
			{
				get
				{
					return myReadingOrderKeyedCollection.Count + 1;
				}
			}
			#endregion // IBranch Implementation
			#region IMultiColumnBranch Implementation
			int IMultiColumnBranch.ColumnCount
			{
				get
				{
					return BASE_COLUMN_COUNT + ExtensionPropertyCount;
				}
			}
			SubItemCellStyles IMultiColumnBranch.ColumnStyles(int column)
			{
				return SubItemCellStyles.Complex;
			}
			int IMultiColumnBranch.GetJaggedColumnCount(int row)
			{
				return 0; // Not called unless features call for jagged columns
			}
			#endregion // IMultiColumnBranch Implementation
			#region Reading Branch Methods
			public override void AddNewReading(int itemLocation)
			{
				ReadingOrderInformation orderInfo = myReadingOrderKeyedCollection[itemLocation];
				orderInfo.EnsureBranch().ShowNewRow(true);
				VirtualTreeControl control = ReadingEditor.Instance.TreeControl;
				control.SelectObject(this, orderInfo, (int)ObjectStyle.TrackingObject, 0);
				control.BeginLabelEdit();
			}
			public override void OnReadingAdded(Reading reading)
			{
				ReadingOrder order = reading.ReadingOrder;
				if (order != null)
				{
					int location = this.LocateCollectionItem(order);
					BranchModificationEventHandler modify = myModify;

					if (location < 0)
					{
						this.PopulateReadingOrderInfo(order);
						if (modify != null)
						{
							int newLoc = this.LocateCollectionItem(order);
							modify(this, BranchModificationEventArgs.InsertItems(this, newLoc - 1, 1));
							modify(this, BranchModificationEventArgs.UpdateCellStyle(this, newLoc, (int)ColumnIndex.ReadingBranch, true)); //may not be needed due to callback on update
							int propertyCount = ExtensionPropertyCount;
							for (int i = 0; i < propertyCount; ++i)
							{
								modify(this, BranchModificationEventArgs.UpdateCellStyle(this, newLoc, (int)ColumnIndex.FirstPropertyBranch + i, true));
							}
							//redraw off and back on in the branch if it has no more than 1 reading
						}
					}
					else
					{
						myReadingOrderKeyedCollection[location].EnsureBranch().OnReadingAdded(reading);
						int actualIndex = myFactType.ReadingOrderCollection.IndexOf(order);
						if (modify != null)
						{
							modify(this, BranchModificationEventArgs.UpdateCellStyle(this, location, (int)ColumnIndex.ReadingBranch, true));
							int propertyCount = ExtensionPropertyCount;
							for (int i = 0; i < propertyCount; ++i)
							{
								modify(this, BranchModificationEventArgs.UpdateCellStyle(this, location, (int)ColumnIndex.FirstPropertyBranch + i, true));
							}

							if (actualIndex != location)
							{
								OnReadingOrderLocationUpdated(order);
							}
							else
							{
								modify(this, BranchModificationEventArgs.Redraw(false));
								modify(this, BranchModificationEventArgs.Redraw(true));
							}
						}
						else if (actualIndex != location)
						{
							OnReadingOrderLocationUpdated(order);
						}
					}
				}
			}
			public override void OnReadingUpdated(Reading reading)
			{
				ReadingOrderInformation info = new ReadingOrderInformation(this, reading.ReadingOrder);
				ReadingOrderInformationCollection orderCollection = myReadingOrderKeyedCollection;
				if (!orderCollection.Contains(info.RoleOrder))
				{
					orderCollection.Add(info);
				}
				int row;
				ReadingBranch branch;
				if (0 <= (row = orderCollection.IndexOf(orderCollection[info.RoleOrder])) &&
					null != (branch = orderCollection[row].Branch))
				{
					branch.OnReadingUpdated(reading);
				}
			}
			public override void OnReadingLocationUpdated(Reading reading)
			{
				ReadingOrder order = reading.ReadingOrder;
				int currentLocation = this.LocateCollectionItem(order);
				if (currentLocation >= 0)
				{
					ReadingBranch branch = myReadingOrderKeyedCollection[currentLocation].Branch;
					if (branch != null)
					{
						branch.OnReadingItemOrderChanged(reading);
					}
				}
			}
			public override void OnReadingRemoved(Reading reading, ReadingOrder order)
			{
				int location = this.LocateCollectionItem(order);
				if (location >= 0)
				{
					ReadingBranch branch = myReadingOrderKeyedCollection[location].Branch;
					if (branch != null)
					{
						branch.OnItemRemoved(reading);
					}
				}
			}
			public override void PromoteSelectedReading(int readingItemLocation, int orderItemLocation)
			{
				ReadingBranch branch = myReadingOrderKeyedCollection[orderItemLocation].Branch;
				if (branch != null)
				{
					branch.PromoteItem(readingItemLocation);
				}
			}
			public override void DemoteSelectedReading(int readingItemLocation, int orderItemLocation)
			{
				ReadingBranch branch = myReadingOrderKeyedCollection[orderItemLocation].Branch;
				if (branch != null)
				{
					branch.DemoteItem(readingItemLocation);
				}
			}
			public override void OnExtensionPropertyChanged(Reading reading, PropertyDescriptor descriptor)
			{
				ReadingOrder order;
				int orderIndex;
				ReadingBranch readingBranch;
				if (null != (order = reading.ReadingOrder) &&
					0 <= (orderIndex = LocateCollectionItem(order)) &&
					null != (readingBranch = myReadingOrderKeyedCollection[orderIndex].Branch))
				{
					readingBranch.OnExtensionPropertyChanged(reading, descriptor);
				}
			}
			#endregion // Reading Branch Methods
			#region ReadingOrder Branch Methods
			public override void AddNewReadingOrder()
			{
				VirtualTreeControl control = ReadingEditor.Instance.TreeControl;
				control.SelectObject(this, RowType.New, (int)ObjectStyle.TrackingObject, 0);
				control.BeginLabelEdit();
			}
			public override void EditReadingOrder(IList<RoleBase> collection)
			{
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
			public override void PromoteSelectedReadingOrder(int itemLocation)
			{
				this.MoveItem(true, itemLocation);
			}
			public override void DemoteSelectedReadingOrder(int itemLocation)
			{
				this.MoveItem(false, itemLocation);
			}
			public override void RemoveSelectedItem()
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
					int row = orderItem.Row;
					myReadingOrderKeyedCollection.RemoveAt(row);
					BranchModificationEventHandler modify = myModify;
					if (modify != null)
					{
						modify(this, BranchModificationEventArgs.DeleteItems(this, row, 1));
					}
				}
			}
			public override void OnReadingOrderLocationUpdated(ReadingOrder order)
			{
				int currentLocation = this.LocateCollectionItem(order);
				if (currentLocation >= 0) //make sure it was found
				{
					ReadingOrderInformation readingOrderInfo = myReadingOrderKeyedCollection[currentLocation];
					int newLocation = myFactType.ReadingOrderCollection.IndexOf(order);
					myReadingOrderKeyedCollection.RemoveAt(currentLocation);
					myReadingOrderKeyedCollection.Insert(newLocation, readingOrderInfo);
					BranchModificationEventHandler modify = myModify;
					if (modify != null)
					{
						modify(this, BranchModificationEventArgs.MoveItem(this, currentLocation, newLocation));
					}
				}
			}
			public override void OnReadingOrderRemoved(ReadingOrder order, FactType factType)
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
					BranchModificationEventHandler modify = myModify;
					if (modify != null)
					{
						modify(this, BranchModificationEventArgs.DeleteItems(this, location, 1));
					}
				}
			}
			/// <summary>
			///  Get context menu commands supported for current selection
			/// </summary>
			/// <returns>ReadingEditorCommands bit field</returns>
			/// <param name="itemLocation">Reading Order Row</param>
			public override ReadingEditorCommands SupportedSelectionCommands(int itemLocation)
			{
				ReadingEditorCommands retval = ReadingEditorCommands.AddReadingOrder;
				int itemCount = myReadingOrderKeyedCollection.Count;
				if (itemLocation < itemCount)
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

						if (itemLocation < itemCount - 1
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
			#region Branch Helper Functions
			private string[] GetRoleNames()
			{
				string[] retVal = myRoleNames;
				if (retVal == null)
				{
					IList<RoleBase> factRoles = GetReadingRoleCollection(myFactType);
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
					BranchModificationEventHandler modify = myModify;
					if (modify != null)
					{
						modify(this, BranchModificationEventArgs.InsertItems(this, myReadingOrderKeyedCollection.Count - 1, 1));
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
				LinkedElementCollection<ReadingOrder> readingOrders = myFactType.ReadingOrderCollection;
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
				using (Transaction t = myFactType.Store.TransactionManager.BeginTransaction(ResourceStrings.ModelReadingEditorMoveReadingOrder))
				{
					myFactType.ReadingOrderCollection.Move(orderRow, orderRow + (promote ? -1 : 1));
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
					ReadingOrderInformationCollection orders = myReadingOrderKeyedCollection;
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
			#region ReadingOrderInformation class
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
				/// Used for an order that does exist in the model (committed)
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
						FactType factType = myParentBranch.FactType;
						if (myReadingOrder == null) //obtain new readingOrder to commit a new reading (if readingOrder is non-existent)
						{
							myReadingOrder = factType.EnsureReadingOrder(this.myRoleOrder);
						}
						myReadingBranch = new ReadingBranch(myParentBranch.myEditor, factType, myReadingOrder, this);
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
			#endregion // ReadingOrderInformation class
			#region ReadingOrderInformationCollection class
			private sealed class ReadingOrderInformationCollection : KeyedCollection<IList<RoleBase>, ReadingOrderInformation>
			{
				#region Constructor
				public ReadingOrderInformationCollection()
					: base(ListEqualityComparer.Comparer, 16)
				{
				}
				#endregion // Constructor
				#region Base overrides
				protected sealed override IList<RoleBase> GetKeyForItem(ReadingOrderInformation item)
				{
					return item.RoleOrder;
				}
				#endregion // Base overrides
				#region Public Accessors
				/// <summary>
				/// Returns a RowType for the row requested, defaults to RowType.Committed
				/// </summary>
				/// <param name="row">zero based Row index</param>
				/// <returns>RowType</returns>
				public RowType GetRowType(int row)
				{
					if (row >= Count)
					{
						return RowType.New;
					}
					return (this[row].ReadingOrder != null) ? RowType.Committed : RowType.Uncommitted;
				}
				#endregion // Public Accessors
				protected override void InsertItem(int index, ReadingOrderInformation item)
				{
					// This is expensive, but there are generally only
					// a handful of reading orders. Verify that the insertion
					// point is accurate for existing reading orders.
					ReadingOrder order = item.ReadingOrder;
					if (order != null)
					{
						LinkedElementCollection<ReadingOrder> orders = order.FactType.ReadingOrderCollection;
						int orderIndex = orders.IndexOf(order);
						for (int i = index - 1; i >= 0; --i)
						{
							ReadingOrder testOrder = this[i].ReadingOrder;
							if (testOrder != null && orders.IndexOf(testOrder) > orderIndex)
							{
								index = i;
							}
						}
					}
					
					base.InsertItem(index, item);
				}
				#region ListEqualityComparer class
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
				#endregion // ListEqualityComparer class
			}
			#endregion // ReadingOrderInformationCollection class
			#region ReadingOrderDescriptor class (New Reading DropDown)
			private sealed class ReadingOrderDescriptor : PropertyDescriptor
			{
				private readonly FactType myFactType;
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
				public ReadingOrderDescriptor(FactType factType, ReadingOrderBranch branch, string name)
					: base(name, null)
				{
					myFactType = factType;
					myBranch = branch;
				}
				public sealed override object GetEditor(Type editorBaseType)
				{
					return editorBaseType == typeof(UITypeEditor) ? new ReadingOrderEditor() : base.GetEditor(editorBaseType);
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
					ReadingOrderInformationCollection collection = myBranch.myReadingOrderKeyedCollection;
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
			#endregion // ReadingOrderDescriptor class (New Reading DropDown)
			#region ReadingBranch class
			private sealed class ReadingBranch : BaseBranch, IBranch
			{
				#region Constants
				private const int COLUMN_COUNT = 1;
				#endregion // Constants
				#region Member Variables
				private readonly FactType myFactType;
				private readonly LinkedElementCollection<Reading> myLiveReadings;
				private readonly List<ReadingData> myReadings = new List<ReadingData>();
				private readonly ReadingOrder myReadingOrder;
				private readonly ReadingOrderInformation myReadingInformation;
				private readonly ReadingEditor myEditor;
				private ExtensionPropertyBranch[] myPropertyBranches;
				private bool myShowNewRow;
				private BranchModificationEventHandler myModify;
				#endregion // Member Variables
				#region Constructor
				public ReadingBranch(ReadingEditor editor, FactType factType, ReadingOrder order, ReadingOrderInformation orderInformation)
				{
					myEditor = editor;
					myReadingOrder = order;
					myLiveReadings = order.ReadingCollection;
					myFactType = factType;
					myReadingInformation = orderInformation;
					this.PopulateBranchData();
				}
				#endregion // Constructor
				#region Branch Properties
				/// <summary>
				/// Returns number of reading orders currently in the branch including new uncommitted readings
				/// </summary>
				public int RowCount
				{
					get
					{
						return myReadings.Count; //get actual count for number of readings as a reading may not exist which is in the myLiveReadings List
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
				#region IBranch Implementation
				VirtualTreeLabelEditData IBranch.BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
				{
					if (row == myReadings.Count - 1 && myShowNewRow)
					{
						InplaceReadingEditor editor = new InplaceReadingEditor();
						editor.Initialize(myReadingInformation.OrderedReplacementFields, SystemColors.WindowText, SystemColors.GrayText);
						bool escapePressed = false;
						EditorUtility.AttachEscapeKeyPressedEventHandler(
							editor,
							delegate(object sender, EventArgs e)
							{
								escapePressed = true;
							});
						return new VirtualTreeLabelEditData(
							editor,
							delegate(VirtualTreeItemInfo itemInfo, Control editControl)
							{
								Store store = myFactType.Store;
								if (escapePressed || !(store as IORMToolServices).CanAddTransaction)
								{
									return LabelEditResult.CancelEdit;
								}
								string newReadingText = editor.BuildReadingText();
								if (newReadingText.Length != 0)
								{
									Reading newReading = null;
									using (Transaction t = store.TransactionManager.BeginTransaction(ResourceStrings.ModelReadingEditorNewReadingTransactionText))
									{
										this.ShowNewRow(false);
										t.TopLevelTransaction.Context.ContextInfo[ORMModel.BlockDuplicateReadingSignaturesKey] = null;
										newReading = new Reading(store);
										newReading.ReadingOrder = myReadingOrder;
										newReading.Text = newReadingText;
										t.Commit();
									}
									if (newReading != null)
									{
										// Reselect the new reading. The option to this approach is to
										// replace the new row with the reading before the transaction
										// commits so that the row is not inserted/readded. For the same
										// user experience, this isn't worth the additional complication.
										myEditor.TreeControl.SelectObject(null, newReading, (int)ObjectStyle.TrackingObject, 0);
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
						bool escapePressed = false;
						EditorUtility.AttachEscapeKeyPressedEventHandler(
							editor,
							delegate(object sender, EventArgs e)
							{
								escapePressed = true;
							});
						return new VirtualTreeLabelEditData(
							editor,
							delegate(VirtualTreeItemInfo itemInfo, Control editControl)
							{
								Store store = currentReading.Store;
								if (escapePressed || !(store as IORMToolServices).CanAddTransaction)
								{
									return LabelEditResult.CancelEdit;
								}
								string newReadingText = editor.BuildReadingText();
								if (newReadingText != startReadingText)
								{
									using (Transaction t = store.TransactionManager.BeginTransaction(ResourceStrings.ModelReadingEditorChangePrimaryReadingText))
									{
										t.TopLevelTransaction.Context.ContextInfo[ORMModel.BlockDuplicateReadingSignaturesKey] = null;
										currentReading.Text = newReadingText;
										t.Commit();
									}
									return LabelEditResult.AcceptEdit;
								}
								return LabelEditResult.CancelEdit;
							});
					}
				}
				BranchFeatures IBranch.Features
				{
					get
					{
						return BranchFeatures.DelayedLabelEdits | BranchFeatures.ExplicitLabelEdits | BranchFeatures.PositionTracking | BranchFeatures.InsertsAndDeletes;
					}
				}
				VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
				{
					VirtualTreeDisplayData retVal = VirtualTreeDisplayData.Empty;
					if (row == RowCount - 1 && myShowNewRow)
					{
						retVal.GrayText = true;
					}
					return retVal;
				}
				object IBranch.GetObject(int row, int column, ObjectStyle style, ref int options)
				{
					if (style == ObjectStyle.TrackingObject)
					{
						return myLiveReadings[row] as Object;
					}
					return null;
				}
				string IBranch.GetText(int row, int column)
				{
					return myReadings[row].Text;
				}
				LocateObjectData IBranch.LocateObject(object obj, ObjectStyle style, int locateOptions)
				{
					Reading reading;
					if (style == ObjectStyle.TrackingObject)
					{
						if (obj == myReadingInformation.RoleOrder || myShowNewRow)
						{
							return new LocateObjectData(RowCount - 1, 0, (int)TrackingObjectAction.ThisLevel);
						}
						else if (null != (reading = (obj as Reading)))
						{
							int row = myLiveReadings.IndexOf(reading);
							if (row >= 0)
							{
								return new LocateObjectData(row, 0, (int)TrackingObjectAction.ThisLevel);
							}
						}
					}
					return new LocateObjectData();
				}
				event BranchModificationEventHandler IBranch.OnBranchModification
				{
					add { myModify += value; }
					remove { myModify -= value; }
				}
				int IBranch.VisibleItemCount
				{
					get
					{
						return RowCount;
					}
				}
				#endregion // IBranch Implementation
				#region Command Response Methods
				/// <summary>
				/// Initiates a Begin Label Edit for the specified Reading
				/// </summary>
				/// <param name="reading">the Reading to edit</param>
				public void EditReading(Reading reading)
				{
					int location = myLiveReadings.IndexOf(reading);
					if (location >= 0)
					{
						((IBranch)this).BeginLabelEdit(location, 0, VirtualTreeLabelEditActivationStyles.ImmediateSelection);
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
						BranchModificationEventHandler modify = myModify;
						if (modify != null)
						{
							// Notify addition at the end
							int insertPosition = myReadings.Count - 2;
							modify(this, BranchModificationEventArgs.InsertItems(this, insertPosition, 1));
						}
					}
					else if (myShowNewRow && !show)
					{
						myShowNewRow = false;
						int deleteIndex = myReadings.IndexOf(new ReadingData(myReadingInformation.Text, null));
						if (deleteIndex >= 0)
						{
							myReadings.RemoveAt(deleteIndex);
							BranchModificationEventHandler modify = myModify;
							if (modify != null)
							{
								modify(this, BranchModificationEventArgs.DeleteItems(this, deleteIndex, 1));
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
					if (myShowNewRow && row == RowCount - 1)
					{
						this.ShowNewRow(false);
					}
					else
					{
						using (Transaction t = myFactType.Store.TransactionManager.BeginTransaction(ResourceStrings.ModelReadingEditorDeleteReadingTransactionText))
						{
							myReadings[row].Reading.Delete();
							t.Commit();
						}
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
				#endregion // Command Response Methods
				#region Event Methods
				/// <summary>
				/// Addition of a New Reading into the ReadingBranch
				/// </summary>
				/// <param name="reading">The reading changed or added</param>
				public void OnReadingAdded(Reading reading)
				{
					if (!myReadings.Contains(new ReadingData(null, reading)))
					{
						int index = myLiveReadings.IndexOf(reading);
						myReadings.Insert(index, new ReadingData(FactType.PopulatePredicateText(reading, null, null, myReadingOrder.RoleCollection, myReadingInformation.OrderedReplacementFields), reading));
						BranchModificationEventHandler modify = myModify;
						if (modify != null)
						{
							modify(this, BranchModificationEventArgs.InsertItems(this, index - 1, 1));
							foreach (ExtensionPropertyBranch propertyBranch in ExtensionPropertyBranches)
							{
								modify(propertyBranch, BranchModificationEventArgs.InsertItems(propertyBranch, index - 1, 1));
							}
						}
					}
					else
					{
						this.OnReadingUpdated(reading);
					}
				}
				/// <summary>
				/// Initiates an Update of an existing Reading
				/// </summary>
				/// <param name="reading">The reading changed</param>
				public void OnReadingUpdated(Reading reading)
				{
					int location = myReadings.IndexOf(new ReadingData(null, reading));
					if (location >= 0)
					{
						LinkedElementCollection<RoleBase> roles = reading.ReadingOrder.RoleCollection;
						string[] replacements = myReadingInformation.OrderedReplacementFields;
						if (roles.Count == replacements.Length)
						{
							myReadings[location] = new ReadingData(FactType.PopulatePredicateText(reading, null, null, roles, replacements), reading);

							BranchModificationEventHandler modify = myModify;
							if (modify != null)
							{
								modify(this, BranchModificationEventArgs.DisplayDataChanged(new DisplayDataChangedData(VirtualTreeDisplayDataChanges.Text, this, location, 0, 1)));
								modify(this, BranchModificationEventArgs.Redraw(false));
								modify(this, BranchModificationEventArgs.Redraw(true));
							}
						}
					}
				}
				/// <summary>
				/// Removes a reading from the brach based upon the reference of the reading object sent
				/// </summary>
				/// <param name="reading">The reading object which has been removed</param>
				public void OnItemRemoved(Reading reading)
				{
					for (int i = myReadings.Count - 1; i >= 0; --i) // run counter backwards so we can modify the set
					{
						if (myReadings[i].Reading == reading)
						{
							myReadings.RemoveAt(i);
							BranchModificationEventHandler modify = myModify;
							if (modify != null)
							{
								modify(this, BranchModificationEventArgs.DeleteItems(this, i, 1));
								foreach (ExtensionPropertyBranch propertyBranch in ExtensionPropertyBranches)
								{
									modify(propertyBranch, BranchModificationEventArgs.DeleteItems(propertyBranch, i, 1));
								}
							}
						}
					}
				}
				/// <summary>
				/// Updates the Display Text and notifies the tree that the display data has changed
				/// </summary>
				public void OnReadingItemOrderChanged(Reading reading)
				{
					int oldRow = myReadings.IndexOf(new ReadingData(null, reading));
					int currentRow = myLiveReadings.IndexOf(reading);

					if (oldRow != currentRow)
					{
						ReadingData readingData = myReadings[oldRow];
						myReadings.RemoveAt(oldRow);
						myReadings.Insert(currentRow, readingData);

						BranchModificationEventHandler modify = myModify;
						if (modify != null)
						{
							modify(this, BranchModificationEventArgs.MoveItem(this, oldRow, currentRow));
							foreach (ExtensionPropertyBranch propertyBranch in ExtensionPropertyBranches)
							{
								modify(propertyBranch, BranchModificationEventArgs.MoveItem(propertyBranch, oldRow, currentRow));
							}
						}
					}
				}
				/// <summary>
				/// Updates the display for an extension property
				/// </summary>
				public void OnExtensionPropertyChanged(Reading reading, PropertyDescriptor descriptor)
				{
					BranchModificationEventHandler modify;
					int location;
					if (null != (modify = myModify) &&
						0 <= (location = myReadings.IndexOf(new ReadingData(null, reading))))
					{
						foreach (ExtensionPropertyBranch propertyBranch in ExtensionPropertyBranches)
						{
							if (propertyBranch.Descriptor == descriptor)
							{
								modify(propertyBranch, BranchModificationEventArgs.DisplayDataChanged(new DisplayDataChangedData(VirtualTreeDisplayDataChanges.VisibleElements, propertyBranch, location, 0, 1)));
								modify(propertyBranch, BranchModificationEventArgs.Redraw(false));
								modify(propertyBranch, BranchModificationEventArgs.Redraw(true));
								break;
							}
						}
					}
				}
				#endregion // Event Methods
				#region Branch Helper Methods and Structs
				/// <summary>
				/// Iinitiates a transaction for moving a Reading within this Reading Order
				/// </summary>
				/// <param name="currentRow">Index of Reading To Move</param>
				/// <param name="newLocation">New Location in the collection</param>
				private void MoveItem(int currentRow, int newLocation)
				{
					using (Transaction t = myFactType.Store.TransactionManager.BeginTransaction(ResourceStrings.ModelReadingEditorMoveReading))
					{
						myLiveReadings.Move(currentRow, newLocation);
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
					int numReadings = myLiveReadings.Count;
					for (int i = 0; i < numReadings; ++i)
					{
						myReadings.Add(new ReadingData(FactType.PopulatePredicateText(myLiveReadings[i], null, null, roleCollection, myReadingInformation.OrderedReplacementFields), myLiveReadings[i]));
					}
				}
				#endregion //Branch Helper Methods and Structs
				#region Extension Property Methods
				/// <summary>
				/// Create a subitem branch for the given property
				/// </summary>
				public IBranch EnsureExtensionPropertyBranch(int propertyIndex)
				{
					PropertyDescriptorCollection descriptors = myEditor.myExtensionProperties;
					ExtensionPropertyBranch[] propertyBranches = myPropertyBranches;
					ExtensionPropertyBranch retVal;
					if (propertyBranches == null)
					{
						myPropertyBranches = propertyBranches = new ExtensionPropertyBranch[descriptors.Count];
						retVal = null;
					}
					else
					{
						retVal = propertyBranches[propertyIndex];
					}
					if (retVal == null)
					{
						propertyBranches[propertyIndex] = retVal = new ExtensionPropertyBranch(this, descriptors[propertyIndex]);
					}
					return retVal;
				}
				private IEnumerable<ExtensionPropertyBranch> ExtensionPropertyBranches
				{
					get
					{
						ExtensionPropertyBranch[] propertyBranches = myPropertyBranches;
						if (propertyBranches != null)
						{
							for (int i = 0; i < propertyBranches.Length; ++i)
							{
								ExtensionPropertyBranch branch = propertyBranches[i];
								if (branch != null)
								{
									yield return branch;
								}
							}
						}
					}
				}
				#endregion // Extension Property Methods
				#region ExtensionPropertyBranch class
				/// <summary>
				/// A subitem expansion branch to show extension properties.
				/// These are displayed as subitem expansion of the parent
				/// branch, but have most in common with the reading branch.
				/// </summary>
				private sealed class ExtensionPropertyBranch : BaseBranch, IBranch
				{
					#region Member Variables
					private ReadingBranch myReadingBranch;
					private PropertyDescriptor myDescriptor;
					#endregion // Member Variables
					#region // Constructor
					#endregion // Constructor
					public ExtensionPropertyBranch(ReadingBranch readingBranch, PropertyDescriptor descriptor)
					{
						myReadingBranch = readingBranch;
						myDescriptor = descriptor;
					}
					#region Accessor Properties
					public PropertyDescriptor Descriptor
					{
						get
						{
							return myDescriptor;
						}
					}
					#endregion // Accessor Properties
					#region IBranch Implementation
					string IBranch.GetText(int row, int column)
					{
						string retVal = null;
						Reading reading;
						object value;
						PropertyDescriptor descriptor;
						if (null != (reading = myReadingBranch.myReadings[row].Reading) &&
							null != (value = (descriptor = myDescriptor).GetValue(reading)))
						{
							TypeConverter converter;
							if (!((null != (converter = descriptor.Converter) &&
								null != (retVal = converter.ConvertToString(value))) ||
								null != (retVal = value as string)))
							{
								retVal = value.ToString();
							}
						}
						return retVal;
					}
					VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
					{
						VirtualTreeDisplayData retVal = VirtualTreeDisplayData.Empty;
						Reading reading;
						if (null != (reading = myReadingBranch.myReadings[row].Reading))
						{
							PropertyDescriptor descriptor = myDescriptor;
							if (descriptor.ShouldSerializeValue(reading))
							{
								retVal.Bold = true;
							}
							if (descriptor.IsReadOnly)
							{
								retVal.GrayText = true;
							}
						}
						return retVal;
					}
					VirtualTreeLabelEditData IBranch.BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
					{
						VirtualTreeDisplayData retVal = VirtualTreeDisplayData.Empty;
						Reading reading;
						if (null != (reading = myReadingBranch.myReadings[row].Reading))
						{
							PropertyDescriptor descriptor = myDescriptor;
							TypeEditorHost host;
							TypeConverter converter;
							if (null != (host = OnScreenTypeEditorHost.Create(descriptor, reading)))
							{
								host.Flags = VirtualTreeInPlaceControlFlags.SizeToText;

								return new VirtualTreeLabelEditData(
									host,
									delegate(VirtualTreeItemInfo itemInfo, Control editControl)
									{
										return LabelEditResult.AcceptEdit;
									});
							}
							else if ((null != (converter = descriptor.Converter) ||
								null != (converter = TypeDescriptor.GetConverter(descriptor.PropertyType))) &&
								converter.CanConvertFrom(typeof(string)))
							{
								return new VirtualTreeLabelEditData(
									null,
									delegate(VirtualTreeItemInfo itemInfo, Control editControl)
									{
										descriptor.SetValue(reading, converter.ConvertFrom(editControl.Text));
										return LabelEditResult.AcceptEdit;
									});
							}
							else if (descriptor.PropertyType == typeof(string))
							{
								return new VirtualTreeLabelEditData(
									null,
									delegate(VirtualTreeItemInfo itemInfo, Control editControl)
									{
										descriptor.SetValue(reading, editControl.Text);
										return LabelEditResult.AcceptEdit;
									});
							}
						}
						return VirtualTreeLabelEditData.Invalid;
					}
					BranchFeatures IBranch.Features
					{
						get
						{
							return BranchFeatures.InsertsAndDeletes | BranchFeatures.ExplicitLabelEdits | BranchFeatures.DelayedLabelEdits;
						}
					}
					int IBranch.VisibleItemCount
					{
						get
						{
							ReadingBranch readingBranch = myReadingBranch;
							return readingBranch.HasNewRow ? readingBranch.RowCount - 1 : readingBranch.RowCount;
						}
					}
					#endregion // IBranch Implementation
				}
				#endregion // ExtensionPropertyBranch class
			}
			#endregion // ReadingBranch class
		}
		#endregion // ReadingOrderBranch class
		#region ReadingVirtualTree class
		private sealed class ReadingVirtualTree : StandardMultiColumnTree
		{
			public ReadingVirtualTree(IBranch root, int extensionPropertyCount)
				: base(BASE_COLUMN_COUNT + extensionPropertyCount)
			{
				this.Root = root;
			}
		}
		#endregion // ReadingVirtualTree class
		#region CustomVirtualTreeControl class
		/// <summary>
		/// This custom class is needed to override the height of column headers to provide 
		/// the Appearance of NO Headers when column permutation is used. 
		/// If column headers are not used when column permutation is used an "index out of range exception" 
		/// is fired when the mouse travels over the region occupied by the scrollbar if no scrollbar exists.
		/// </summary>
		private class CustomVirtualTreeControl : StandardVirtualTreeControl
		{
			/// <summary>
			/// Needed for creating a header to prevent the vertical scrollbar from thowing an exception
			/// </summary>
			/// <returns></returns>
			protected override VirtualTreeHeaderControl CreateHeaderControl()
			{
				VirtualTreeColumnHeader[] headers = GetColumnHeaders();
				if (headers != null && headers.Length > 2)
				{
					return base.CreateHeaderControl();
				}
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
		#endregion // CustomVirtualTreeControl class
	}
}
