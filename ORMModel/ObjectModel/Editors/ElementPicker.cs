using System;
using System.Diagnostics;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
namespace Neumont.Tools.ORM.ObjectModel.Editors
{
	/// <summary>
	/// A base class used to display a simple list of elements
	/// the property grid. Derived classes override the GetContentList
	/// method to return items, and alternately the LastControlSize and
	/// NullItemText getters to control the list contents.
	/// </summary>
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
		#endregion // DropDownListBox class. Handles Escape key for ListBox
		#region IList wrapper to automatically display a null element
		private class NullElementList : IList
		{
			#region NullPlaceholder class
			private class NullPlaceholder
			{
				private string myText;
				public NullPlaceholder(string nullText)
				{
					myText = nullText;
				}
				public override string ToString()
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
				return object.ReferenceEquals(test, myPlaceholder);
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
			int IList.Add(object value)
			{
				return myInner.Add(value) + 1;
			}
			void IList.Clear()
			{
				myInner.Clear();
			}
			bool IList.Contains(object value)
			{
				return (value == null || object.ReferenceEquals(value, myPlaceholder)) ? true : myInner.Contains(value);
			}
			int IList.IndexOf(object value)
			{
				if (value == null || object.ReferenceEquals(value, myPlaceholder))
				{
					return 0;
				}
				else
				{
					int retVal = myInner.IndexOf(value);
					return (retVal >= 0) ? retVal + 1 : retVal;
				}
			}
			void IList.Insert(int index, object value)
			{
				if (index > 0)
				{
					myInner.Insert(--index, value);
				}
			}
			bool IList.IsFixedSize
			{
				get
				{
					return myInner.IsFixedSize;
				}
			}
			bool IList.IsReadOnly
			{
				get
				{
					return myInner.IsReadOnly;
				}
			}
			void IList.Remove(object value)
			{
				myInner.Remove(value);
			}
			void IList.RemoveAt(int index)
			{
				if (index > 0)
				{
					myInner.RemoveAt(--index);
				}
			}
			object IList.this[int index]
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
			void ICollection.CopyTo(Array array, int index)
			{
				((object[])array)[index] = null;
				myInner.CopyTo(array, ++index);
			}
			int ICollection.Count
			{
				get
				{
					return myInner.Count + 1;
				}
			}
			bool ICollection.IsSynchronized
			{
				get
				{
					Debug.Assert(false); // Not handled
					return false;
				}
			}
			object ICollection.SyncRoot
			{
				get
				{
					Debug.Assert(false); // Not handled
					return null;
				}
			}
			#endregion // ICollection Members
			#region IEnumerable Members
			IEnumerator IEnumerable.GetEnumerator()
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
		[SecurityPermission(SecurityAction.LinkDemand)]
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
				if (nullText != null && nullText.Length != 0)
				{
					sourceList = nullList = new NullElementList(sourceList, nullText);
				}

				// Proceed if there is anything to show
				if (sourceList != null && sourceList.Count != 0)
				{
					DropDownListBox listBox = null;
					try
					{
						// Create a listbox with its events
						listBox = new DropDownListBox();
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
	/// Static helper functions to use with UITypeEditor
	/// implementations
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
			Microsoft.VisualStudio.EnterpriseTools.Shell.ModelElementTreeNode treeNode;
			if (pickAnyElement && instance.GetType().IsArray)
			{
				instance = (instance as object[])[0];
			}
			if (null != (pel = instance as PresentationElement))
			{
				instance = pel.ModelElement;
			}
			else if (null != (treeNode = instance as Microsoft.VisualStudio.EnterpriseTools.Shell.ModelElementTreeNode))
			{
				instance = treeNode.ModelElement;
			}
			return instance;
		}
		#endregion // EditorUtility Specific
	}
}