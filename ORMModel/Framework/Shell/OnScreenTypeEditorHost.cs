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
using System.Diagnostics;
using Microsoft.VisualStudio.VirtualTreeGrid;
using System.ComponentModel;
using System.Reflection;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Drawing;

namespace ORMSolutions.ORMArchitect.Framework.Shell
{
	/// <summary>
	/// A version of <see cref="TypeEditorHost"/> that keeps dropdowns near the
	/// left edge of the screen on the screen.
	/// </summary>
	public class OnScreenTypeEditorHost : TypeEditorHost
	{
		#region Public factory methods
		/// <summary>
		/// Factory method for creating the appropriate drop-down control based on the given property descriptor
		/// </summary>
		/// <param name="propertyDescriptor">A property descriptor describing the property being set</param>
		/// <param name="instance">The object instance being edited</param>
		/// <returns>A TypeEditorHost instance if the given property descriptor supports it, null otherwise.</returns>
		public static new TypeEditorHost Create(PropertyDescriptor propertyDescriptor, object instance)
		{
			TypeEditorHost host = null;
			if (propertyDescriptor != null)
			{
				UITypeEditor editor = propertyDescriptor.GetEditor(typeof(UITypeEditor)) as UITypeEditor;
				if (editor != null)
				{
					return new OnScreenTypeEditorHost(editor, propertyDescriptor, instance);
				}
				TypeConverter typeConverter = propertyDescriptor.Converter;
				if ((typeConverter != null) && typeConverter.GetStandardValuesSupported(null))
				{
					host = new OnScreenTypeEditorHostListBox(typeConverter, propertyDescriptor, instance);
				}
			}
			return host;
		}
		/// <summary>
		/// Factory method for creating the appropriate drop-down control based on the given property descriptor.
		/// If the property descriptor supports a UITypeEditor, a TypeEditorHost will be created with that editor.
		/// If not, and the TypeConverver attached to the PropertyDescriptor supports standard values, a
		/// TypeEditorHostListBox will be created with this TypeConverter.
		/// </summary>
		/// <param name="propertyDescriptor">A property descriptor describing the property being set</param>
		/// <param name="instance">The object instance being edited</param>
		/// <param name="editControlStyle">The type of control to show in the edit area.</param>
		/// <returns>A TypeEditorHost instance if the given property descriptor supports it, null otherwise.</returns>
		public static new TypeEditorHost Create(PropertyDescriptor propertyDescriptor, object instance, TypeEditorHostEditControlStyle editControlStyle)
		{
			TypeEditorHost host = null;
			if (propertyDescriptor != null)
			{
				UITypeEditor editor = propertyDescriptor.GetEditor(typeof(UITypeEditor)) as UITypeEditor;
				if (editor != null)
				{
					return new OnScreenTypeEditorHost(editor, propertyDescriptor, instance, editControlStyle);
				}
				TypeConverter typeConverter = propertyDescriptor.Converter;
				if ((typeConverter != null) && typeConverter.GetStandardValuesSupported(null))
				{
					host = new OnScreenTypeEditorHostListBox(typeConverter, propertyDescriptor, instance, editControlStyle);
				}
			}
			return host;
		}
		#endregion // Public factory methods
		#region Static on screen handling
		private static bool myRetrievedField;
		private static FieldInfo myDropdownHolderField;
		/// <summary>
		/// Attach a callback even to move the
		/// </summary>
		private static void AttachDropdownActivator(TypeEditorHost host)
		{
			if (!myRetrievedField)
			{
				System.Threading.Interlocked.CompareExchange<FieldInfo>(
					ref myDropdownHolderField,
					typeof(TypeEditorHost).GetField("dropDownHolder", BindingFlags.NonPublic | BindingFlags.Instance),
					null);
				myRetrievedField = true;
			}
			FieldInfo dropDownHolderField = myDropdownHolderField;
			if (dropDownHolderField != null)
			{
				Form form = dropDownHolderField.GetValue(host) as Form;
				if (form != null)
				{
					form.Activated += delegate(object sender, EventArgs e2)
					{
						Control ctl = (Control)sender;
						Rectangle rect = ctl.Bounds;
						Screen screen = Screen.FromControl(ctl);
						int testLeft;
						if ((screen != null) && ((rect.X - rect.Width / 3) < (testLeft = screen.WorkingArea.Left)))
						{
							// With the size bar always on the left we want to leave room
							// to resize the dropdown. Anchor it on the left edge of the
							// dropdown button instead of the default right edge.
							rect.X = rect.Right - SystemInformation.VerticalScrollBarArrowHeight;
							ctl.Bounds = rect;
						}
					};
				}
			}
		}
		#endregion // Static on screen handling
		#region Constructors
		/// <summary>
		/// Creates a new TypeEditorHost to display the given UITypeEditor
		/// </summary>
		/// <param name="editor">The UITypeEditor instance to host</param>
		/// <param name="propertyDescriptor">Property descriptor used to get/set values in the drop-down.</param>
		/// <param name="instance">Instance object used to get/set values in the drop-down.</param>
		protected OnScreenTypeEditorHost(UITypeEditor editor, PropertyDescriptor propertyDescriptor, object instance)
			: base(editor, propertyDescriptor, instance)
		{
		}
		/// <summary>
		/// Creates a new TypeEditorHost to display the given UITypeEditor
		/// </summary>
		/// <param name="editor">The UITypeEditor instance to host</param>
		/// <param name="editControlStyle">The type of control to show in the edit area.</param>
		/// <param name="propertyDescriptor">Property descriptor used to get/set values in the drop-down.</param>
		/// <param name="instance">Instance object used to get/set values in the drop-down.</param>
		protected OnScreenTypeEditorHost(UITypeEditor editor, PropertyDescriptor propertyDescriptor, object instance, TypeEditorHostEditControlStyle editControlStyle)
			: base(editor, propertyDescriptor, instance, editControlStyle)
		{
		}
		/// <summary>
		/// Creates a new TypeEditorHost with the given editStyle.
		/// </summary>
		/// <param name="editStyle">Style of editor to create.</param>
		/// /// <param name="propertyDescriptor">Property descriptor used to get/set values in the drop-down.</param>
		/// <param name="instance">Instance object used to get/set values in the drop-down.</param>
		/// <param name="editControlStyle">Style of text box to create.</param>
		protected OnScreenTypeEditorHost(UITypeEditorEditStyle editStyle, PropertyDescriptor propertyDescriptor, object instance, TypeEditorHostEditControlStyle editControlStyle)
			: base(editStyle, propertyDescriptor, instance, editControlStyle)
		{
		}
		#endregion // Constructors
		#region Base override
		/// <summary>
		/// Force the dropdown horizontally onto the screen working area
		/// </summary>
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			AttachDropdownActivator(this);
		}
		#endregion // Base override
		#region OnScreenTypeEditorHostListBox class
		/// <summary>
		/// An overridden class to handle moving the list on screen during creation
		/// </summary>
		protected class OnScreenTypeEditorHostListBox : TypeEditorHostListBox
		{
			#region Constructors
			/// <summary>
			/// Creates a new drop-down control to display the given TypeConverter
			/// </summary>
			/// <param name="typeConverter">The TypeConverter instance to retrieve drop-down values from</param>
			/// <param name="propertyDescriptor">Property descriptor used to get/set values in the drop-down.</param>
			/// <param name="instance">Instance object used to get/set values in the drop-down.</param>
			public OnScreenTypeEditorHostListBox(TypeConverter typeConverter, PropertyDescriptor propertyDescriptor, object instance)
				: base(typeConverter, propertyDescriptor, instance)
			{
			}
			/// <summary>
			/// Creates a new drop-down list to display the given type converter.
			/// The type converter must support a standard values collection.
			/// </summary>
			/// <param name="typeConverter">The TypeConverter instance to retrieve drop-down values from</param>
			/// <param name="propertyDescriptor">Property descriptor used to get/set values in the drop-down.</param>
			/// <param name="instance">Instance object used to get/set values in the drop-down.</param>
			/// <param name="editControlStyle">Edit control style.</param>
			public OnScreenTypeEditorHostListBox(TypeConverter typeConverter, PropertyDescriptor propertyDescriptor, object instance, TypeEditorHostEditControlStyle editControlStyle)
				: base(typeConverter, propertyDescriptor, instance, editControlStyle)
			{
			}
			#endregion // Constructors
			#region Base overrides
			/// <summary>
			/// Force the dropdown horizontally onto the screen working area
			/// </summary>
			protected override void OnHandleCreated(EventArgs e)
			{
				base.OnHandleCreated(e);
				AttachDropdownActivator(this);
			}
			#endregion // Base overrides
		}
		#endregion // OnScreenTypeEditorHostListBox class
	}
}