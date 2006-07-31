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

namespace Neumont.Tools.Modeling.Design
{
	/// <summary>
	/// A base class used to display a simple list of elements
	/// the property grid. Derived classes override the GetContentList
	/// method to return items, and alternately the LastControlSize and
	/// NullItemText getters to control the list contents.
	/// </summary>
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public abstract class ElementPicker<T> : SizePreservingEditor<T>
		where T : ElementPicker<T>
	{
		#region DropDownListBox class. Handles Escape key for ListBox
		private class DropDownListBox : ListBox
		{
			private bool myEscapePressed;
			private int myLastSelectedIndex = -1;
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
			protected sealed override void OnSelectedIndexChanged(EventArgs e)
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
		private object myInitialSelectionValue;
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
					// Create a listbox with its events
					using (DropDownListBox listBox = (nullList != null) ? new NullFirstItemDropDownListBox() : new DropDownListBox())
					{
						listBox.BindingContextChanged += this.HandleBindingContextChanged;
						listBox.MouseDoubleClick += delegate(object sender, MouseEventArgs e)
						{
							if (e.Button == MouseButtons.Left)
							{
								editor.CloseDropDown();
							}
						};
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
						editor.DropDownControl(listBox);

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
				}
				return newObject;
			}
			return value;
		}
		#endregion // UITypeEditor overrides
		#region ElementPicker Specifics
		private void HandleBindingContextChanged(object sender, EventArgs e)
		{
			ListBox listBox = (ListBox)sender;
			if (myInitialSelectionValue != null)
			{
				listBox.BindingContextChanged -= this.HandleBindingContextChanged;
				object value = myInitialSelectionValue;
				myInitialSelectionValue = null;
				listBox.SelectedItem = value;
				if (listBox.SelectedItem == null)
				{
					// Sometimes this doesn't take
					myInitialSelectionValue = value;
					listBox.BindingContextChanged += this.HandleBindingContextChanged;
				}
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
}
