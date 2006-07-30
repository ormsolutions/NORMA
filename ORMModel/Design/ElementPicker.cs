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
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.VirtualTreeGrid;

namespace Neumont.Tools.ORM.Design
{
	/// <summary>
	/// A base class used to display a simple list of elements
	/// the property grid. Derived classes override the GetContentList
	/// method to return items, and alternately the LastControlSize and
	/// NullItemText getters to control the list contents.
	/// </summary>
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public abstract class ElementPicker : UITypeEditor
	{
		#region DropDownListBox class. Handles Escape key for ListBox
		private class DropDownListBox : ListBox
		{
			private bool myEscapePressed;
			private int myLastSelectedIndex = -1;
			protected override bool IsInputKey(Keys keyData)
			{
				if ((keyData & Keys.KeyCode) == Keys.Escape)
				{
					myEscapePressed = true;
				}
				return base.IsInputKey(keyData);
			}
			public bool EscapePressed
			{
				get
				{
					return myEscapePressed;
				}
			}
			// UNDONE: This sounds like it may have been an early beta bug. Someone should check whether this still happens.
			/// <summary>
			/// For some reason, the base SelectedItem property
			/// is null if the commit is made with an Enter instead
			/// of a double click. Track the selected item index separately.
			/// </summary>
			public int LastSelectedIndex
			{
				get
				{
					return myLastSelectedIndex;
				}
			}
			protected override void OnSelectedIndexChanged(EventArgs e)
			{
				myLastSelectedIndex = SelectedIndex;
				base.OnSelectedIndexChanged(e);
			}
		}
		private sealed class NullFirstItemDropDownListBox : DropDownListBox
		{
			public sealed override string Text
			{
				get
				{
					return (SelectedIndex == 0) ? null : base.Text;
				}
			}
		}
		#endregion // DropDownListBox class. Handles Escape key for ListBox
		#region IList wrapper to automatically display a null element
		private sealed class NullElementList : IList
		{
			#region NullPlaceholder class
			private sealed class NullPlaceholder
			{
				private string myText;
				public NullPlaceholder(string nullText)
				{
					myText = nullText;
				}
				public sealed override string ToString()
				{
					return myText;
				}
			}
			#endregion // NullPlaceholder class
			#region Constructors, Accessors, and MemberVariables
			private IList myInner;
			private NullPlaceholder myPlaceholder;
			public NullElementList(IList innerList, string nullText)
			{
				if (innerList == null)
				{
					innerList = new object[0];
				}
				myInner = innerList;
				myPlaceholder = new NullPlaceholder(nullText);
			}
			public bool IsNullItem(object test)
			{
				return test == null || test == myPlaceholder;
			}
			public object NullItem
			{
				get
				{
					return myPlaceholder;
				}
			}
			#endregion // Constructors, Accessors, and MemberVariables
			#region IList Members
			public int Add(object value)
			{
				return myInner.Add(value) + 1;
			}
			public void Clear()
			{
				myInner.Clear();
			}
			public bool Contains(object value)
			{
				return (value == null || value == myPlaceholder) || myInner.Contains(value);
			}
			public int IndexOf(object value)
			{
				if (value == null || value == myPlaceholder)
				{
					return 0;
				}
				else
				{
					int retVal = myInner.IndexOf(value);
					return (retVal >= 0) ? retVal + 1 : retVal;
				}
			}
			public void Insert(int index, object value)
			{
				if (index > 0)
				{
					myInner.Insert(--index, value);
				}
			}
			public bool IsFixedSize
			{
				get
				{
					return myInner.IsFixedSize;
				}
			}
			public bool IsReadOnly
			{
				get
				{
					return myInner.IsReadOnly;
				}
			}
			public void Remove(object value)
			{
				myInner.Remove(value);
			}
			public void RemoveAt(int index)
			{
				if (index > 0)
				{
					myInner.RemoveAt(--index);
				}
			}
			public object this[int index]
			{
				get
				{
					return (index == 0) ? null : myInner[--index];
				}
				set
				{
					if (index > 0)
					{
						myInner[--index] = value;
					}
				}
			}
			#endregion // IList Members
			#region ICollection Members
			public void CopyTo(Array array, int index)
			{
				array.SetValue(null, index);
				myInner.CopyTo(array, index + 1);
			}
			public int Count
			{
				get
				{
					return myInner.Count + 1;
				}
			}
			public bool IsSynchronized
			{
				get
				{
					Debug.Fail("Not tested");
					return myInner.IsSynchronized;
				}
			}
			public object SyncRoot
			{
				get
				{
					Debug.Fail("Not tested");
					return myInner.SyncRoot;
				}
			}
			#endregion // ICollection Members
			#region IEnumerable Members
			public IEnumerator GetEnumerator()
			{
				yield return myPlaceholder;
				foreach (object obj in myInner)
				{
					yield return obj;
				}
			}
			#endregion // IEnumerable Members
		}
		#endregion // IList wrapper to automatically display a null element
		#region UITypeEditor overrides
		private IWindowsFormsEditorService myEditor;
		private object myInitialSelectionValue;
		private static Size myLastControlSize = Size.Empty;
		/// <summary>
		/// Required UITypeEditor override. Opens dropdown modally
		/// and waits for user input.
		/// </summary>
		/// <param name="context">The descriptor context. Used to retrieve
		/// the live instance and other data.</param>
		/// <param name="provider">The service provider for the given context.</param>
		/// <param name="value">The current property value</param>
		/// <returns>The updated property value, or the orignal value to effect a cancel</returns>
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			myEditor = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
			if (myEditor != null)
			{
				object newObject = value;
				// Get the list contents and add a null handler if needed
				IList elements = GetContentList(context, value);
				IList sourceList = elements;
				NullElementList nullList = null;
				string nullText = NullItemText;
				if (!string.IsNullOrEmpty(nullText))
				{
					sourceList = nullList = new NullElementList(sourceList, nullText);
				}

				// Proceed if there is anything to show
				if (sourceList != null && sourceList.Count > 0)
				{
					DropDownListBox listBox = null;
					try
					{
						// Create a listbox with its events
						listBox = (nullList != null) ? new NullFirstItemDropDownListBox() :  new DropDownListBox();
						listBox.BindingContextChanged += new EventHandler(HandleBindingContextChanged);
						listBox.MouseDoubleClick += new MouseEventHandler(HandleDoubleClick);
						listBox.BorderStyle = BorderStyle.None;
						listBox.IntegralHeight = false;

						// Manage the size of the control
						Size lastSize = LastControlSize;
						if (!lastSize.IsEmpty)
						{
							listBox.Size = lastSize;
						}

						// Attach the data source and handle initial selection
						listBox.DataSource = sourceList;
						if (value != null)
						{
							myInitialSelectionValue = TranslateToDisplayObject(value, elements);
						}
						else if (nullList != null)
						{
							myInitialSelectionValue = nullList.NullItem;
						}

						// Show the dropdown. This is modal.
						myEditor.DropDownControl(listBox);

						// Record the final size, we'll use it next time for this type of control
						LastControlSize = listBox.Size;

						// Make sure the user didn't cancel, and translate the null placeholder
						// back to null if necessary
						if (!listBox.EscapePressed)
						{
							int lastIndex = listBox.LastSelectedIndex;
							if (lastIndex != -1)
							{
								newObject = sourceList[lastIndex];
								if (nullList != null && nullList.IsNullItem(newObject))
								{
									newObject = null;
								}
								// Give the caller the chance to change the type of the chosen object
								newObject = TranslateFromDisplayObject((nullList == null) ? lastIndex : lastIndex - 1, newObject);

							}
						}
					}
					finally
					{
						if (listBox != null && !listBox.IsDisposed)
						{
							listBox.Dispose();
						}
					}
				}
				return newObject;
			}
			return value;
		}
		/// <summary>
		/// Select a drop down style
		/// </summary>
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}
		/// <summary>
		/// Allow resizing
		/// </summary>
		public override bool IsDropDownResizable
		{
			get
			{
				return true;
			}
		}
		#endregion // UITypeEditor overrides
		#region ElementPicker Specifics
		private void HandleBindingContextChanged(object sender, EventArgs e)
		{
			ListBox l = (ListBox)sender;
			if (myInitialSelectionValue != null)
			{
				l.BindingContextChanged -= new EventHandler(HandleBindingContextChanged);
				object value = myInitialSelectionValue;
				myInitialSelectionValue = null;
				l.SelectedItem = value;
				if (l.SelectedItem == null)
				{
					// Sometimes this doesn't take
					myInitialSelectionValue = value;
					l.BindingContextChanged += new EventHandler(HandleBindingContextChanged);
				}
			}
		}

		private void HandleDoubleClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				myEditor.CloseDropDown();
			}
		}
		/// <summary>
		/// Override and set a text value to allow a null element to be returned
		/// </summary>
		protected virtual string NullItemText
		{
			get
			{
				return null;
			}
		}
		/// <summary>
		/// Generate and return a list to display in the item picker. The initial
		/// value (if it is not null) must be included in the returned set. If null values
		/// are allowed, the NullItemText getter should also be overridden.
		/// </summary>
		/// <param name="context">ITypeDescriptorContext passed in by the system</param>
		/// <param name="value">The current value</param>
		/// <returns>A list. An empty list will</returns>
		protected abstract IList GetContentList(ITypeDescriptorContext context, object value);
		/// <summary>
		/// Controls the size of the dropdown for a given type as it opens and closes. Override
		/// both the setter and getter to change the value for specific controls
		/// </summary>
		protected virtual Size LastControlSize
		{
			get
			{
				return myLastControlSize;
			}
			set
			{
				myLastControlSize = value;
			}
		}
		/// <summary>
		/// Give a derived class with an opportunity to change the
		/// chosen object from the dropdown list into a different type
		/// of object. This allows the passed in candidate list to contain
		/// objects that have a different type than the type of the property.
		/// TranslateFromDisplayObject is called when the user selects an item
		/// in the dropdown.
		/// </summary>
		/// <param name="newIndex">The index chosen from the list. If the null
		/// item was chosen, then the newIndex will be -1</param>
		/// <param name="newObject">The chosen object. Can be null.</param>
		/// <returns>Default implementation returns newObject</returns>
		protected virtual object TranslateFromDisplayObject(int newIndex, object newObject)
		{
			return newObject;
		}
		/// <summary>
		/// Give a derived class with an opportunity to change the
		/// chosen object from the dropdown list into a different type
		/// of object. This allows the passed in candidate list to contain
		/// objects that have a different type than the type of the property.
		/// TranslateToDisplayObject is called to determine the initial selection
		/// in the dropdown.
		/// </summary>
		/// <param name="initialObject">The starting value of the property</param>
		/// <param name="contentList">The list returned from GetContentList</param>
		/// <returns>Default implementation returns initialObject</returns>
		protected virtual object TranslateToDisplayObject(object initialObject, IList contentList)
		{
			return initialObject;
		}
		#endregion // ElementPicker Specifics
	}
	/// <summary>
	/// A base class used to display a list of elements in a
	/// VirtualTreeGrid control. Override the GetTree method to
	/// populate the constrol, and alternately the LastControlSize property.
	/// </summary>
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public abstract class TreePicker : UITypeEditor
	{
		#region DropDownTreeControl class. Handles Escape key for ListBox
		private sealed class DropDownTreeControl : VirtualTreeControl
		{
			private bool myEscapePressed;
			private int myLastSelectedRow = -1;
			private int myLastSelectedColumn = -1;
			public event DoubleClickEventHandler AfterDoubleClick;
			protected override bool IsInputKey(Keys keyData)
			{
				if ((keyData & Keys.KeyCode) == Keys.Escape)
				{
					myEscapePressed = true;
				}
				return base.IsInputKey(keyData);
			}
			protected override CreateParams CreateParams
			{
				[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					CreateParams @params = base.CreateParams;
					@params.ExStyle &= ~0x200; // Turn off Fixed3D border style
					return @params;
				}
			}
			protected override void OnDoubleClick(DoubleClickEventArgs e)
			{
				base.OnDoubleClick(e);
				if (AfterDoubleClick != null)
				{
					AfterDoubleClick(this, e);
				}
			}
			public bool EscapePressed
			{
				get
				{
					return myEscapePressed;
				}
			}
			public int LastSelectedRow
			{
				get
				{
					return myLastSelectedRow;
				}
			}
			public int LastSelectedColumn
			{
				get
				{
					return myLastSelectedColumn;
				}
			}
			protected sealed override void OnSelectionChanged(EventArgs e)
			{
				myLastSelectedColumn = CurrentColumn;
				myLastSelectedRow = CurrentIndex;
				base.OnSelectionChanged(e);
			}
		}
		#endregion // DropDownTreeControl class. Handles Escape key for VirtualTreeControl
		#region UITypeEditor overrides
		private IWindowsFormsEditorService myEditor;
		private object myInitialSelectionValue;
		private static Size myLastControlSize = Size.Empty;
		/// <summary>
		/// Required UITypeEditor override. Opens dropdown modally
		/// and waits for user input.
		/// </summary>
		/// <param name="context">The descriptor context. Used to retrieve
		/// the live instance and other data.</param>
		/// <param name="provider">The service provider for the given context.</param>
		/// <param name="value">The current property value</param>
		/// <returns>The updated property value, or the orignal value to effect a cancel</returns>
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			myEditor = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
			if (myEditor != null)
			{
				object newObject = value;
				// Get the tree contents
				ITree tree = GetTree(context, value);
				// Proceed if there is anything to show
				// Don't check tree.VisibleItemCount. Allows the derived class to display an empty dropdown
				// by returning a tree with no visible elements.
				if (tree != null)
				{
					DropDownTreeControl treeControl = null;
					try
					{
						// Create a listbox with its events
						treeControl = new DropDownTreeControl();
						if (UseStandardCheckBoxes)
						{
							ImageList images = new ImageList();
							images.ImageSize = new Size(16, 16);
							treeControl.ImageList = images;
							treeControl.StandardCheckBoxes = true;
						}
						treeControl.BindingContextChanged += new EventHandler(HandleBindingContextChanged);
						treeControl.AfterDoubleClick += new DoubleClickEventHandler(HandleDoubleClick);

						// Manage the size of the control
						Size lastSize = LastControlSize;
						if (!lastSize.IsEmpty)
						{
							treeControl.Size = lastSize;
						}
						myInitialSelectionValue = value;

						// Show the dropdown. This is modal.
						IMultiColumnTree multiTree = tree as IMultiColumnTree;
						if (multiTree != null)
						{
							treeControl.MultiColumnTree = multiTree;
						}
						else
						{
							treeControl.Tree = tree;
						}
						myEditor.DropDownControl(treeControl);

						// Record the final size, we'll use it next time for this type of control
						LastControlSize = treeControl.Size;

						// Make sure the user didn't cancel, and give derived classes a chance
						// to translate the value displayed in the tree to an appropriately
						// typed value for the associated property.
						if (!treeControl.EscapePressed)
						{
							int lastRow = treeControl.LastSelectedRow;
							int lastColumn = treeControl.LastSelectedColumn;
							if (lastRow != -1)
							{
								newObject = TranslateToValue(context, value, tree, lastRow, lastColumn);
							}
						}
					}
					finally
					{
						if (treeControl != null && !treeControl.IsDisposed)
						{
							treeControl.Dispose();
						}
					}
				}
				return newObject;
			}
			return value;
		}
		/// <summary>
		/// Select a drop down style
		/// </summary>
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}
		/// <summary>
		/// Allow resizing
		/// </summary>
		public override bool IsDropDownResizable
		{
			get
			{
				return true;
			}
		}
		#endregion // UITypeEditor overrides
		#region TreePicker Specifics
		private void HandleBindingContextChanged(object sender, EventArgs e)
		{
			Control l = (Control)sender;
			if (myInitialSelectionValue != null)
			{
				l.BindingContextChanged -= new EventHandler(HandleBindingContextChanged);
				object value = myInitialSelectionValue;
				myInitialSelectionValue = null;
			}
		}

		private void HandleDoubleClick(object sender, DoubleClickEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				myEditor.CloseDropDown();
			}
		}
		/// <summary>
		/// Generate the tree to display in the tree control. If the
		/// control also implements IMultiColumnTree then it will be
		/// shown as a multi-column tree.
		/// </summary>
		/// <param name="context">ITypeDescriptorContext passed in by the system</param>
		/// <param name="value">The current value</param>
		/// <returns>A list. An empty list will</returns>
		protected abstract ITree GetTree(ITypeDescriptorContext context, object value);
		/// <summary>
		/// Translate the current state of the tree to a new value for the property
		/// </summary>
		/// <param name="context">ITypeDescriptorContext passed in by the system</param>
		/// <param name="oldValue">The starting value</param>
		/// <param name="tree">The tree returned by GetTree</param>
		/// <param name="selectedRow">The last selected row in the tree</param>
		/// <param name="selectedColumn">The last selected column in the tree</param>
		/// <returns>Default implementation returns oldValue</returns>
		protected virtual object TranslateToValue(ITypeDescriptorContext context, object oldValue, ITree tree, int selectedRow, int selectedColumn)
		{
			return oldValue;
		}
		/// <summary>
		/// Should standard checkboxes be enabled on the parent tree control. Defaults to true.
		/// </summary>
		protected virtual bool UseStandardCheckBoxes
		{
			get
			{
				return true;
			}
		}
		/// <summary>
		/// Controls the size of the dropdown for a given type as it opens and closes. Override
		/// both the setter and getter to change the value for specific controls
		/// </summary>
		protected virtual Size LastControlSize
		{
			get
			{
				return myLastControlSize;
			}
			set
			{
				myLastControlSize = value;
			}
		}
		#endregion // TreePicker Specifics
	}
	/// <summary>
	/// A base class used to display text in a multi-line text box control.
	/// This editor and derived classes can be directly applied to any string
	/// property. Overriding the LastControlSize property allows dropdown
	/// size to be control for each editor implementation.
	/// </summary>
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class MultilineTextEditor : UITypeEditor
	{
		#region TextBoxControl class. Handles Escape key for TextBox
		private sealed class TextBoxControl : TextBox
		{
			private bool myEscapePressed;
			/// <summary>
			/// Set the appropriate TextBox styles
			/// </summary>
			public TextBoxControl() : base()
			{
				Multiline = true;
				ScrollBars = ScrollBars.Vertical;
				BorderStyle = BorderStyle.None;
			}
			protected sealed override bool IsInputKey(Keys keyData)
			{
				if ((keyData & Keys.KeyCode) == Keys.Escape)
				{
					myEscapePressed = true;
				}
				return base.IsInputKey(keyData);
			}
			public bool EscapePressed
			{
				get
				{
					return myEscapePressed;
				}
			}
		}
		#endregion // TextBoxControl class. Handles Escape key for TextBox
		#region UITypeEditor overrides
		/// <summary>
		/// The default <see cref="Size.Width"/> used for the <see cref="LastControlSize"/> property
		/// the first time the control is shown.
		/// </summary>
		protected const int DefaultInitialControlWidth = 272;
		/// <summary>
		/// The default <see cref="Size.Height"/> used for the <see cref="LastControlSize"/> property
		/// the first time the control is shown.
		/// </summary>
		protected const int DefaultInitialControlHeight = 128;
		/// <summary>
		/// The default <see cref="Size"/> used for the <see cref="LastControlSize"/> property
		/// the first time the control is shown.
		/// </summary>
		protected static readonly Size DefaultInitialControlSize = new Size(DefaultInitialControlWidth, DefaultInitialControlHeight);
		private static Size myLastControlSize = DefaultInitialControlSize;
		/// <summary>
		/// Required UITypeEditor override. Opens dropdown modally
		/// and waits for user input.
		/// </summary>
		/// <param name="context">The descriptor context. Used to retrieve
		/// the live instance and other data.</param>
		/// <param name="provider">The service provider for the given context.</param>
		/// <param name="value">The current property value</param>
		/// <returns>The updated property value, or the orignal value to effect a cancel</returns>
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			IWindowsFormsEditorService editor = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
			if (editor != null)
			{
				string newText = value as string ?? string.Empty;

				// Create a textbox with its events
				using (TextBoxControl textControl = new TextBoxControl())
				{
					// Manage the size of the control
					Size lastSize = LastControlSize;
					if (!lastSize.IsEmpty)
					{
						textControl.Size = lastSize;
					}

					textControl.Text = newText;
					textControl.Select(0, 0);
					editor.DropDownControl(textControl);

					// Record the final size, we'll use it next time for this type of control
					LastControlSize = textControl.Size;

					// Make sure the user didn't cancel
					if (!textControl.EscapePressed)
					{
						newText = textControl.Text;
					}
				}
				return newText;
			}
			return value;
		}
		/// <summary>
		/// Select a drop down style
		/// </summary>
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}
		/// <summary>
		/// Allow resizing
		/// </summary>
		public override bool IsDropDownResizable
		{
			get
			{
				return true;
			}
		}
		#endregion // UITypeEditor overrides
		#region MultilineTextEditor Specifics
		/// <summary>
		/// Controls the size of the dropdown for a given type as it opens and closes. Override
		/// both the setter and getter to change the value for specific controls
		/// </summary>
		protected virtual Size LastControlSize
		{
			get
			{
				return myLastControlSize;
			}
			set
			{
				myLastControlSize = value;
			}
		}
		#endregion // TreePicker Specifics
	}
	/// <summary>
	/// Static helper functions to use with <see cref="UITypeEditor"/>
	/// implementations.
	/// </summary>
	public static class EditorUtility
	{
		#region EditorUtility Specific
		/// <summary>
		/// Selection context is often based on a wrapper shape, such
		/// as a NodeShape or a tree node in a model browser. Use this
		/// helper function to resolve known element containers to get to the
		/// backing element.
		/// </summary>
		/// <param name="instance">The selected object returned by ITypeDescriptorContext.Instance</param>
		/// <param name="pickAnyElement">If an array of elements is passed in, then any element will work as the context element.</param>
		/// <returns>A resolved object, or the starting instance if the item is not wrapped.</returns>
		public static object ResolveContextInstance(object instance, bool pickAnyElement)
		{
			// Test early, prevent crashes if pickAnyElement is true
			if (instance == null)
			{
				return null;
			}
			PresentationElement pel;
			Microsoft.VisualStudio.Modeling.Shell.ModelElementTreeNode treeNode;
			if (pickAnyElement && instance.GetType().IsArray)
			{
				instance = (instance as object[])[0];
			}
			if (null != (pel = instance as PresentationElement))
			{
				instance = pel.ModelElement;
			}
			else if (null != (treeNode = instance as Microsoft.VisualStudio.Modeling.Shell.ModelElementTreeNode))
			{
				instance = treeNode.ModelElement;
			}
			return instance;
		}
		[DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
		private static extern IntPtr GetFocus();
		/// <summary>
		/// Open the Properties Window, select the target property
		/// descriptor, and activate the edit field.
		/// </summary>
		/// <param name="serviceProvider">The service provider to use</param>
		/// <param name="targetDescriptor">The property descriptor to activate</param>
		/// <param name="openDropDown">true to open the dropdown when the edit field is activated.</param>
		public static void ActivatePropertyEditor(IServiceProvider serviceProvider, PropertyDescriptor targetDescriptor, bool openDropDown)
		{
			IVsUIShell shell;
			if (null != serviceProvider &&
				null != (shell = (IVsUIShell)serviceProvider.GetService(typeof(IVsUIShell))))
			{
				Guid windowGuid = new Guid(ToolWindowGuids.PropertyBrowser);
				IVsWindowFrame frame;
				ErrorHandler.ThrowOnFailure(shell.FindToolWindow((uint)(__VSFINDTOOLWIN.FTW_fForceCreate), ref windowGuid, out frame));
				ErrorHandler.ThrowOnFailure(frame.Show());
				SendKeys.Flush();
				Control ctl = Control.FromHandle(GetFocus());
				PropertyGrid propertyGrid = null;
				while (ctl != null)
				{
					propertyGrid = ctl as PropertyGrid;
					if (propertyGrid != null)
					{
						break;
					}
					ctl = ctl.Parent;
				}
				if (propertyGrid != null)
				{
					// Make sure any selection change has posted
					shell.RefreshPropertyBrowser(-1); // DISPID_UNKNOWN
					string targetCategory = targetDescriptor.Category;
					string targetDisplayName = targetDescriptor.DisplayName;
					GridItem activateItem = null;
					GridItem selectedItem = propertyGrid.SelectedGridItem;
					if (selectedItem.GridItemType == GridItemType.Property && selectedItem.Label == targetDisplayName)
					{
						activateItem = selectedItem;
					}
					else
					{
						GridItem currentItem = selectedItem;
						bool moveDown = false;
						switch (currentItem.GridItemType)
						{
							case GridItemType.Property:
								if (currentItem.Label == targetDisplayName &&
									currentItem.PropertyDescriptor.Category == targetCategory)
								{
									activateItem = currentItem;
								}
								break;
							case GridItemType.Category:
								if (currentItem.Label == targetCategory)
								{
									moveDown = true;
								}
								break;
						}
						while (activateItem == null && currentItem != null)
						{
							GridItemCollection items = null;
							if (moveDown)
							{
								if (currentItem.Expandable && !currentItem.Expanded)
								{
									currentItem.Expanded = true;
								}
								items = currentItem.GridItems;
							}
							else
							{
								currentItem = currentItem.Parent;
								while (!moveDown && currentItem != null)
								{
									switch (currentItem.GridItemType)
									{
										case GridItemType.Category:
											if (currentItem.Label == targetCategory)
											{
												items = currentItem.GridItems;
												moveDown = true;
											}
											break;
										case GridItemType.Root:
											items = currentItem.GridItems;
											moveDown = true;
											break;
										default:
											currentItem = currentItem.Parent;
											break;

									}
								}
								if (moveDown)
								{
									if (currentItem.Expandable && !currentItem.Expanded)
									{
										currentItem.Expanded = true;
									}
								}
							}
							if (activateItem == null && items != null)
							{
								currentItem = null;
								foreach (GridItem item in items)
								{
									items = null;
									GridItemType itemType = item.GridItemType;
									if (itemType == GridItemType.Category)
									{
										if (item.Label == targetCategory)
										{
											if (item.Expandable && !item.Expanded)
											{
												item.Expanded = true;
											}
											items = item.GridItems;
											break;
										}
									}
									else if (itemType == GridItemType.Property)
									{
										if (item.Label == targetDisplayName)
										{
											activateItem = item;
											break;
										}
									}
								}
							}
						}
					}
					if (activateItem != null && activateItem.Select())
					{
						SendKeys.Flush();
						SendKeys.SendWait("{TAB}");
						if (openDropDown)
						{
							SendKeys.SendWait("%{DOWN}");
						}
					}
				}
			}
		}
		#endregion // EditorUtility Specific
	}
}
