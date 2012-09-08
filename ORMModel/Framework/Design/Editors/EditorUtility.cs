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
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.VirtualTreeGrid;

namespace ORMSolutions.ORMArchitect.Framework.Design
{
	#region ContextElementParts struct
	/// <summary>
	/// Selected context instances are often represented by a wrapper
	/// element representing a shape or an element reference. Use
	/// the <see cref="ResolveContextInstance(object)"/> or <see cref="ResolveContextInstance(object,bool)"/>
	/// methods to drilldown into an instance to separate the primary,
	/// reference, and presentation parts.
	/// </summary>
	public struct ContextElementParts
	{
		private object myPrimaryElement;
		private IElementReference myReferenceElement;
		private PresentationElement myPresentationElement;
		/// <summary>
		/// Drilldown on instance parts for an element. Does not perform
		/// any array handling.
		/// </summary>
		/// <param name="instance">The context instance. For an editor context, this
		/// is the value returned by <see cref="ITypeDescriptorContext.Instance"/></param>
		/// <returns>A populated <see cref="ContextElementParts"/> structure</returns>
		public static ContextElementParts ResolveContextInstance(object instance)
		{
			return ResolveContextInstance(instance, false);
		}
		/// <summary>
		/// Drilldown on instance parts for an element. Does not perform
		/// any special array handling.
		/// </summary>
		/// <param name="instance">The context instance. For an editor context, this
		/// is the value returned by <see cref="ITypeDescriptorContext.Instance"/></param>
		/// <param name="pickAnyElement">If an array of elements is passed in, then use the first element as the context element.</param>
		/// <returns>A populated <see cref="ContextElementParts"/> structure</returns>
		public static ContextElementParts ResolveContextInstance(object instance, bool pickAnyElement)
		{
			// Note that this is duplicated in EditorUtility.ResolveContextInstance

			ContextElementParts retVal = new ContextElementParts();
			// Test early, prevent crashes if pickAnyElement is true
			if (instance == null)
			{
				return retVal;
			}
			PresentationElement pel;
			IElementReference reference;
			if (pickAnyElement && instance.GetType().IsArray)
			{
				instance = (instance as object[])[0];
			}
			if (null != (reference = instance as IElementReference))
			{
				instance = reference.ReferencedElement;
				retVal.myReferenceElement = reference;
			}
			if (null != (pel = instance as PresentationElement))
			{
				instance = pel.ModelElement;
				retVal.myPresentationElement = pel;
			}
			retVal.myPrimaryElement = instance;
			return retVal;
		}
		/// <summary>
		/// The primary element extracted from a wrapper instance.
		/// </summary>
		public object PrimaryElement
		{
			get
			{
				return myPrimaryElement;
			}
		}
		/// <summary>
		/// The <see cref="IElementReference"/> resolved to get the <see cref="PrimaryElement"/>
		/// </summary>
		public IElementReference ReferenceElement
		{
			get
			{
				return myReferenceElement;
			}
		}
		/// <summary>
		/// The <see cref="PresentationElement"/> resolved to get the <see cref="PrimaryElement"/>
		/// </summary>
		public PresentationElement PresentationElement
		{
			get
			{
				return myPresentationElement;
			}
		}
	}
	#endregion // ContextElementParts struct
	#region INotifyEscapeKeyPressed interface
	/// <summary>
	/// Callback interface to determine if an escape key was pressed
	/// while a <see cref="UITypeEditor"/> dropdown was open. A control
	/// implementing this interface should override the <see cref="Control.IsInputKey"/>
	/// method and notify an attached event if the escape key was pressed.
	/// To simplify implementation with composite controls, the <see cref="EditorUtility.AttachEscapeKeyPressedEventHandler"/>
	/// method can be used to recursively attach the event listener to any
	/// contained control implementing this interface.
	/// </summary>
	public interface INotifyEscapeKeyPressed
	{
		/// <summary>
		/// Attach an event handler for notification of the escape
		/// key being pressed.
		/// </summary>
		event EventHandler EscapePressed;
	}
	#endregion // INotifyEscapeKeyPressed interface
	#region EditorUtility class
	/// <summary>
	/// Static helper functions to use with <see cref="UITypeEditor"/>
	/// implementations.
	/// </summary>
	public static class EditorUtility
	{
		#region ResolveContextInstance
		/// <summary>
		/// Selection context is often based on a wrapper shape, such
		/// as a NodeShape or a tree node in a model browser. Use this
		/// helper function to resolve known element containers to get to the
		/// backing element.
		/// </summary>
		/// <param name="instance">The context instance. For an editor context, this
		/// is the value returned by <see cref="ITypeDescriptorContext.Instance"/></param>
		/// <param name="pickAnyElement">If an array of elements is passed in, then use the first element as the context element.</param>
		/// <returns>A resolved object, or the starting instance if the item is not wrapped.</returns>
		public static object ResolveContextInstance(object instance, bool pickAnyElement)
		{
			// Note that this is duplicated in ContextEditorParts.ResolveContextInstance

			// Test early, prevent crashes if pickAnyElement is true
			if (instance == null)
			{
				return null;
			}
			PresentationElement pel;
			IElementReference reference;
			if (pickAnyElement && instance.GetType().IsArray)
			{
				instance = (instance as object[])[0];
			}
			if (null != (reference = instance as IElementReference))
			{
				instance = reference.ReferencedElement;
			}
			if (null != (pel = instance as PresentationElement))
			{
				instance = pel.ModelElement;
			}
			return instance;
		}
		#endregion // ResolveContextInstance
		#region ActivatePropertyEditor
		/// <summary>
		/// Helper method to recursively find a control of a given type
		/// </summary>
		/// <typeparam name="ControlType">The type of the to find</typeparam>
		/// <param name="control">The starting control</param>
		/// <returns>A control instance of the given type, or null</returns>
		private static ControlType FindContainedControl<ControlType>(Control control) where ControlType : Control
		{
			ControlType retVal = control as ControlType;
			if (retVal != null)
			{
				return retVal;
			}
			foreach (Control childControl in control.Controls)
			{
				retVal = FindContainedControl<ControlType>(childControl);
				if (retVal != null)
				{
					return retVal;
				}
			}
			return null;
		}
		/// <summary>
		/// Open the Properties Window, select the target property
		/// descriptor, and activate the edit field.
		/// </summary>
		/// <param name="serviceProvider">The service provider to use</param>
		/// <param name="targetDescriptor">The property descriptor to activate</param>
		/// <param name="openDropDown">true to open the dropdown when the edit field is activated.</param>
		public static void ActivatePropertyEditor(IServiceProvider serviceProvider, PropertyDescriptor targetDescriptor, bool openDropDown)
		{
			if (targetDescriptor == null)
			{
				throw new ArgumentNullException("targetDescriptor");
			}
			IVsUIShell shell;
			if (null != serviceProvider &&
				null != (shell = (IVsUIShell)serviceProvider.GetService(typeof(IVsUIShell))))
			{
				Guid windowGuid = StandardToolWindows.PropertyBrowser;
				IVsWindowFrame frame;
				ErrorHandler.ThrowOnFailure(shell.FindToolWindow((uint)(__VSFINDTOOLWIN.FTW_fForceCreate), ref windowGuid, out frame));
				ErrorHandler.ThrowOnFailure(frame.Show());

				WindowPane propertiesPane;
				IWin32Window propertiesWindow;
				Control ctl;
				PropertyGrid propertyGrid;
				if (null != (propertiesPane = serviceProvider.GetService(typeof(SVSMDPropertyBrowser)) as WindowPane) &&
					null != (propertiesWindow = propertiesPane.Window) &&
					null != (ctl = Control.FromHandle(propertiesWindow.Handle)) &&
					null != (propertyGrid = FindContainedControl<PropertyGrid>(ctl)))
				{
					propertyGrid.Focus();
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
						SendKeys.Send(openDropDown ? "%{DOWN}" : "{TAB}");
					}
				}
			}
		}
		#endregion // ActivatePropertyEditor
		#region GetAttributeArray
		/// <summary>
		/// Convert an <see cref="AttributeCollection"/> to an <see cref="Attribute"/> array.
		/// </summary>
		public static Attribute[] GetAttributeArray(AttributeCollection attributes)
		{
			int attributeCount;
			if (attributes == null || 0 == (attributeCount = attributes.Count))
			{
				return null;
			}
			Attribute[] retVal = new Attribute[attributeCount];
			attributes.CopyTo(retVal, 0);
			return retVal;
		}
		#endregion // GetAttributeArray
		#region GetEditablePropertyDescriptors method
		/// <summary>
		/// Get an editable <see cref="PropertyDescriptorCollection"/>
		/// </summary>
		/// <param name="properties">The properties to extend. Can be <see langword="null"/></param>
		/// <returns>An editable <see cref="PropertyDescriptorCollection"/></returns>
		public static PropertyDescriptorCollection GetEditablePropertyDescriptors(PropertyDescriptorCollection properties)
		{
			if (properties == null)
			{
				properties = new PropertyDescriptorCollection(null);
			}
			else if (((IList)properties).IsReadOnly)
			{
				PropertyDescriptor[] descriptorArray = new PropertyDescriptor[properties.Count];
				properties.CopyTo(descriptorArray, 0);
				properties = new PropertyDescriptorCollection(descriptorArray);
			}
			return properties;
		}
		#endregion // GetEditablePropertyDescriptors method
		#region ModifyPropertyDescriptorDisplay
		/// <summary>
		/// Modify the display settings for a <see cref="PropertyDescriptor"/> by
		/// wrapping the base descriptor with another property descriptor instance.
		/// </summary>
		/// <param name="basedOnDescriptor">The original descriptor.</param>
		/// <param name="displayName">The modified display name. If this is <see langword="null"/>, then the original display name is used.</param>
		/// <param name="description">The modified description. If this is <see langword="null"/>, then the original description is used.</param>
		/// <param name="category">The modified category. If this is <see langword="null"/>, then the original category is used.</param>
		/// <returns>A wrapper <see cref="PropertyDescriptor"/></returns>
		public static PropertyDescriptor ModifyPropertyDescriptorDisplay(PropertyDescriptor basedOnDescriptor, string displayName, string description, string category)
		{
			return new DisplayModifiedPropertyDescriptor(basedOnDescriptor, displayName, description, category);
		}
		/// <summary>
		/// Modify the display settings for a <see cref="PropertyDescriptor"/> by
		/// wrapping the base descriptor with another property descriptor instance.
		/// </summary>
		/// <param name="descriptorCollection">A collection of descriptors.</param>
		/// <param name="propertyName">The non-localized name of the property to modify.</param>
		/// <param name="displayName">The modified display name. If this is <see langword="null"/>, then the original display name is used.</param>
		/// <param name="description">The modified description. If this is <see langword="null"/>, then the original description is used.</param>
		/// <param name="category">The modified category. If this is <see langword="null"/>, then the original category is used.</param>
		/// <returns>A wrapper <see cref="PropertyDescriptor"/></returns>
		public static void ModifyPropertyDescriptorDisplay(PropertyDescriptorCollection descriptorCollection, string propertyName, string displayName, string description, string category)
		{
			PropertyDescriptor descriptor;
			if (descriptorCollection != null &&
				null != (descriptor = descriptorCollection[propertyName]))
			{
				descriptorCollection.Remove(descriptor);
				descriptorCollection.Add(ModifyPropertyDescriptorDisplay(descriptor, displayName, description, category));
			}
		}
		/// <summary>
		/// Wrapper <see cref="PropertyDescriptor"/> class to support display modification
		/// </summary>
		private sealed class DisplayModifiedPropertyDescriptor : PropertyDescriptor
		{
			#region Member Variables
			private readonly PropertyDescriptor myInner;
			private readonly string myDisplayName;
			private readonly string myDescription;
			private readonly string myCategory;
			#endregion // Member Variables
			#region Constructor
			/// <summary>
			/// Create a wrapped descriptor
			/// </summary>
			/// <param name="modifyDescriptor">The descriptor to wrap.</param>
			/// <param name="displayName">The modified display name. If this is <see langword="null"/>, then the original display name is used.</param>
			/// <param name="description">The modified description. If this is <see langword="null"/>, then the original description is used.</param>
			/// <param name="category">The modified category. If this is <see langword="null"/>, then the original category is used.</param>
			public DisplayModifiedPropertyDescriptor(PropertyDescriptor modifyDescriptor, string displayName, string description, string category)
				: base(modifyDescriptor.Name, EditorUtility.GetAttributeArray(modifyDescriptor.Attributes))
			{
				myInner = modifyDescriptor;
				myDisplayName = displayName;
				myDescription = description;
				myCategory = category;
			}
			#endregion // Constructor
			#region Display overrides
			public override string Category
			{
				get
				{
					return myCategory ?? myInner.Category;
				}
			}
			public override string DisplayName
			{
				get
				{
					return myDisplayName ?? myInner.DisplayName;
				}
			}
			public override string Description
			{
				get
				{
					return myDescription ?? myInner.Description;
				}
			}
			#endregion // Display overrides
			#region Other overrides
			public override bool CanResetValue(object component)
			{
				return myInner.CanResetValue(component);
			}
			public override Type ComponentType
			{
				get { return myInner.ComponentType; }
			}
			public override object GetValue(object component)
			{
				return myInner.GetValue(component);
			}
			public override bool IsReadOnly
			{
				get { return myInner.IsReadOnly; }
			}
			public override Type PropertyType
			{
				get { return myInner.PropertyType; }
			}
			public override void ResetValue(object component)
			{
				myInner.ResetValue(component);
			}
			public override void SetValue(object component, object value)
			{
				myInner.SetValue(component, value);
			}
			public override bool ShouldSerializeValue(object component)
			{
				return myInner.ShouldSerializeValue(component);
			}
			public override void AddValueChanged(object component, EventHandler handler)
			{
				myInner.AddValueChanged(component, handler);
			}
			public override AttributeCollection Attributes
			{
				get { return myInner.Attributes; }
			}
			public override TypeConverter Converter
			{
				get { return myInner.Converter; }
			}
			public override bool DesignTimeOnly
			{
				get { return myInner.DesignTimeOnly; }
			}
			public override PropertyDescriptorCollection GetChildProperties(object instance, Attribute[] filter)
			{
				return myInner.GetChildProperties(instance, filter);
			}
			public override object GetEditor(Type editorBaseType)
			{
				return myInner.GetEditor(editorBaseType);
			}
			public override bool IsBrowsable
			{
				get { return myInner.IsBrowsable; }
			}
			public override bool IsLocalizable
			{
				get { return myInner.IsLocalizable; }
			}
			public override void RemoveValueChanged(object component, EventHandler handler)
			{
				myInner.RemoveValueChanged(component, handler);
			}
			public override bool SupportsChangeEvents
			{
				get { return myInner.SupportsChangeEvents; }
			}
			public override string ToString()
			{
				return myInner.ToString();
			}
			#endregion // Other overrides
		}
		#endregion // ModifyPropertyDescriptorDisplay
		#region ReflectStoreEnabledPropertyDescriptor
		/// <summary>
		/// Create a property descriptor based on reflection
		/// for a property on an element that is not defined
		/// in the domain model.
		/// </summary>
		/// <param name="componentType">The type of the component. The component
		/// is expected to be a subtype of <see cref="ModelElement"/>, or each instance
		/// must implement <see cref="IElementReference"/> and return a subtype of ModelElement.</param>
		/// <param name="propertyName">The name of the property in the class.</param>
		/// <param name="propertyType">The type of the property.</param>
		/// <param name="displayName">The display name for the property. If this is <see langword="null"/>,
		/// then the normal property descriptor display name mechanism is used to retrieve the property name.
		/// This provides a simpler approach than setting <see cref="DisplayNameAttribute"/> attributes
		/// for each property.</param>
		/// <param name="description">The description for the property. If this is <see langword="null"/>,
		/// then the normal property descriptor description mechanism is used to retrieve the description.
		/// This provides a simpler approach than setting <see cref="DescriptionAttribute"/> attributes
		/// for each property.</param>
		/// <param name="category">The category for the property. If this is <see langword="null"/>,
		/// then the normal property descriptor category mechanism is used to retrieve the category.
		/// This provides a simpler approach than setting <see cref="CategoryAttribute"/> attributes
		/// for each property.</param>
		/// <returns>A <see cref="PropertyDescriptor"/> that can be used similarly to an <see cref="ElementPropertyDescriptor"/>.
		/// Setting and resetting values with this descriptor will automatically create the appropriate <see cref="Transaction"/>.</returns>
		public static PropertyDescriptor ReflectStoreEnabledPropertyDescriptor(Type componentType, string propertyName, Type propertyType, string displayName, string description, string category)
		{
			return new StoreEnabledPropertyDescriptor(TypeDescriptor.CreateProperty(componentType, propertyName, propertyType), displayName, description, category);
		}
		#endregion // ReflectStoreEnabledPropertyDescriptor
		#region RedirectPropertyDescriptor
		/// <summary>
		/// Create a wrapped descriptor that presents a direct property of
		/// <paramref name="wrappedComponent"/> as a direct property of
		/// a component with <paramref name="componentType"/>.
		/// </summary>
		/// <param name="wrappedComponent">The target component.</param>
		/// <param name="wrappedDescriptor">The property descriptor for a specific property on the <paramref name="wrappedComponent"/>.</param>
		/// <param name="componentType">The type of the presented component.</param>
		/// <returns>A wrapped descriptor with the <see cref="PropertyDescriptor.ComponentType"/> modified.</returns>
		/// <remarks>If <paramref name="wrappedDescriptor"/> is itself redirected, then the redirection is collapsed.</remarks>
		public static PropertyDescriptor RedirectPropertyDescriptor(object wrappedComponent, PropertyDescriptor wrappedDescriptor, Type componentType)
		{
			return RedirectedPropertyDescriptor.Create(wrappedComponent, wrappedDescriptor, componentType);
		}
		#region RedirectedPropertyDescriptor class
		/// <summary>
		/// A <see cref="PropertyDescriptor"/> wrapper used to present
		/// properties of related components directly as properties
		/// of another element with a different component type.
		/// </summary>
		private sealed class RedirectedPropertyDescriptor : PropertyDescriptor
		{
			#region Member Variables
			private readonly PropertyDescriptor myInnerDescriptor;
			private readonly object myInnerComponent;
			private readonly Type myComponentType;
			#endregion // Member Variables
			#region Constructor
			/// <summary>
			/// Create a wrapped descriptor that presents a direct property of
			/// <paramref name="wrappedComponent"/> as a direct property of
			/// a component with <paramref name="componentType"/>.
			/// </summary>
			/// <param name="wrappedComponent">The target component.</param>
			/// <param name="wrappedDescriptor">The property descriptor for a specific property on the <paramref name="wrappedComponent"/>.</param>
			/// <param name="componentType">The type of the presented component.</param>
			public static RedirectedPropertyDescriptor Create(object wrappedComponent, PropertyDescriptor wrappedDescriptor, Type componentType)
			{
				RedirectedPropertyDescriptor nestedRedirect = wrappedDescriptor as RedirectedPropertyDescriptor;
				if (nestedRedirect != null)
				{
					return new RedirectedPropertyDescriptor(nestedRedirect.myInnerComponent, nestedRedirect.myInnerDescriptor, componentType);
				}
				return new RedirectedPropertyDescriptor(wrappedComponent, wrappedDescriptor, componentType);
			}
			/// <summary>
			/// Create a wrapped descriptor that presents a direct property of
			/// <paramref name="wrappedComponent"/> as a direct property of
			/// a component with <paramref name="componentType"/>.
			/// </summary>
			/// <param name="wrappedComponent">The target component.</param>
			/// <param name="wrappedDescriptor">The property descriptor for a specific property on the <paramref name="wrappedComponent"/>.</param>
			/// <param name="componentType">The type of the presented component.</param>
			private RedirectedPropertyDescriptor(object wrappedComponent, PropertyDescriptor wrappedDescriptor, Type componentType)
				: base(wrappedDescriptor.Name, EditorUtility.GetAttributeArray(wrappedDescriptor.Attributes))
			{
				myInnerDescriptor = wrappedDescriptor;
				myInnerComponent = wrappedComponent;
				myComponentType = componentType;
			}
			#endregion // Constructor
			#region Accessor Properties
			/// <summary>
			/// Get the inner descriptor
			/// </summary>
			public PropertyDescriptor WrappedDescriptor
			{
				get
				{
					return myInnerDescriptor;
				}
			}
			#endregion // Accessor Properties
			#region Other overrides
			/// <summary>
			/// Standard override. Defer to the wrapped descriptor.
			/// </summary>
			public override string Category
			{
				get
				{
					return myInnerDescriptor.Category;
				}
			}
			/// <summary>
			/// Standard override. Defer to the wrapped descriptor.
			/// </summary>
			public override string DisplayName
			{
				get
				{
					return myInnerDescriptor.DisplayName;
				}
			}
			/// <summary>
			/// Standard override. Defer to the wrapped descriptor.
			/// </summary>
			public override string Description
			{
				get
				{
					return myInnerDescriptor.Description;
				}
			}
			/// <summary>
			/// Standard override. Defer to the wrapped descriptor.
			/// </summary>
			public override bool CanResetValue(object component)
			{
				if (null != (component = myInnerComponent))
				{
					return myInnerDescriptor.CanResetValue(component);
				}
				return false;
			}
			/// <summary>
			/// Standard override. Defer to the wrapped descriptor.
			/// </summary>
			public override Type ComponentType
			{
				get { return myComponentType; }
			}
			/// <summary>
			/// Standard override. Defer to the wrapped descriptor.
			/// </summary>
			public override object GetValue(object component)
			{
				if (null != (component = myInnerComponent))
				{
					return myInnerDescriptor.GetValue(component);
				}
				return null;
			}
			/// <summary>
			/// Standard override. Defer to the wrapped descriptor.
			/// </summary>
			public override bool IsReadOnly
			{
				get { return myInnerDescriptor.IsReadOnly; }
			}
			/// <summary>
			/// Standard override. Defer to the wrapped descriptor.
			/// </summary>
			public override Type PropertyType
			{
				get { return myInnerDescriptor.PropertyType; }
			}
			/// <summary>
			/// Standard override. Defer to the wrapped descriptor.
			/// </summary>
			public override void ResetValue(object component)
			{
				if (null != (component = myInnerComponent))
				{
					myInnerDescriptor.ResetValue(component);
				}
			}
			/// <summary>
			/// Standard override. Defer to the wrapped descriptor.
			/// </summary>
			public override void SetValue(object component, object value)
			{
				if (null != (component = myInnerComponent))
				{
					myInnerDescriptor.SetValue(component, value);
				}
			}
			/// <summary>
			/// Standard override. Defer to the wrapped descriptor.
			/// </summary>
			public override bool ShouldSerializeValue(object component)
			{
				if (null != (component = myInnerComponent))
				{
					return myInnerDescriptor.ShouldSerializeValue(component);
				}
				return false;
			}
			/// <summary>
			/// Standard override. Defer to the wrapped descriptor.
			/// </summary>
			public override void AddValueChanged(object component, EventHandler handler)
			{
				if (null != (component = myInnerComponent))
				{
					myInnerDescriptor.AddValueChanged(component, handler);
				}
			}
			/// <summary>
			/// Standard override. Defer to the wrapped descriptor.
			/// </summary>
			public override AttributeCollection Attributes
			{
				get { return myInnerDescriptor.Attributes; }
			}
			/// <summary>
			/// Standard override. Defer to the wrapped descriptor.
			/// </summary>
			public override TypeConverter Converter
			{
				get { return myInnerDescriptor.Converter; }
			}
			/// <summary>
			/// Standard override. Defer to the wrapped descriptor.
			/// </summary>
			public override bool DesignTimeOnly
			{
				get { return myInnerDescriptor.DesignTimeOnly; }
			}
			/// <summary>
			/// Standard override. Defer to the wrapped descriptor.
			/// </summary>
			public override PropertyDescriptorCollection GetChildProperties(object instance, Attribute[] filter)
			{
				if (null != (instance = myInnerComponent))
				{
					return myInnerDescriptor.GetChildProperties(instance, filter);
				}
				return null;
			}
			/// <summary>
			/// Standard override. Defer to the wrapped descriptor.
			/// </summary>
			public override object GetEditor(Type editorBaseType)
			{
				return myInnerDescriptor.GetEditor(editorBaseType);
			}
			/// <summary>
			/// Standard override. Defer to the wrapped descriptor.
			/// </summary>
			public override bool IsBrowsable
			{
				get { return myInnerDescriptor.IsBrowsable; }
			}
			/// <summary>
			/// Standard override. Defer to the wrapped descriptor.
			/// </summary>
			public override bool IsLocalizable
			{
				get { return myInnerDescriptor.IsLocalizable; }
			}
			/// <summary>
			/// Standard override. Defer to the wrapped descriptor.
			/// </summary>
			public override void RemoveValueChanged(object component, EventHandler handler)
			{
				if (null != (component = myInnerComponent))
				{
					myInnerDescriptor.RemoveValueChanged(component, handler);
				}
			}
			/// <summary>
			/// Standard override. Defer to the wrapped descriptor.
			/// </summary>
			public override bool SupportsChangeEvents
			{
				get { return myInnerDescriptor.SupportsChangeEvents; }
			}
			/// <summary>
			/// Standard override. Defer to the wrapped descriptor.
			/// </summary>
			public override string ToString()
			{
				return myInnerDescriptor.ToString();
			}
			#endregion // Other overrides
		}
		#endregion // RedirectedPropertyDescriptor class
		#endregion // RedirectPropertyDescriptor
		#region PropertyDescriptorAs
		/// <summary>
		/// See if a property descriptor is of a given type, including resolution of
		/// any wrapped descriptors returned by <see cref="RedirectPropertyDescriptor"/>.
		/// </summary>
		/// <typeparam name="DescriptorType">A type assignable to <see cref="PropertyDescriptor"/></typeparam>
		/// <param name="descriptor">The (possibly redirection) property descriptor.</param>
		/// <returns>An instance of the given type, or <see langword="null"/></returns>
		public static DescriptorType PropertyDescriptorAs<DescriptorType>(PropertyDescriptor descriptor)
			where DescriptorType : PropertyDescriptor
		{
			DescriptorType retVal;
			RedirectedPropertyDescriptor redirectedDescriptor;
			if (null == (retVal = descriptor as DescriptorType) &&
				null != (redirectedDescriptor = descriptor as RedirectedPropertyDescriptor))
			{
				retVal = redirectedDescriptor.WrappedDescriptor as DescriptorType;
			}
			return retVal;
		}
		#endregion // PropertyDescriptorAs
		#region AttachEscapeKeyPressedEventHandler
		/// <summary>
		/// Recursively test if a <paramref name="control"/> or its contained
		/// controls implement <see cref="INotifyEscapeKeyPressed"/> and attach
		/// the provided <paramref name="escapeKeyPressedHandler"/>.
		/// </summary>
		public static void AttachEscapeKeyPressedEventHandler(Control control, EventHandler escapeKeyPressedHandler)
		{
			INotifyEscapeKeyPressed escapeKeyPressed = control as INotifyEscapeKeyPressed;
			if (escapeKeyPressed != null)
			{
				escapeKeyPressed.EscapePressed += escapeKeyPressedHandler;
			}
			foreach (Control nestedControl in control.Controls)
			{
				AttachEscapeKeyPressedEventHandler(nestedControl, escapeKeyPressedHandler);
			}
		}
		#endregion // AttachEscapeKeyPressedEventHandler
	}
	#endregion // EditorUtility class
	#region StoreEnabledPropertyDescriptor class
	/// <summary>
	/// A helper class to provide property descriptors that are easily
	/// merged with DSL-provided property descriptors.
	/// </summary>
	public class StoreEnabledPropertyDescriptor : PropertyDescriptor
	{
		#region Member Variables
		private readonly PropertyDescriptor myInner;
		private readonly string myDisplayName;
		private readonly string myDescription;
		private readonly string myCategory;
		#endregion // Member Variables
		#region Constructor
		/// <summary>
		/// Create a wrapped descriptor that automatically resolves and forwards
		/// a component to the provided descriptor. The component must be a <see cref="ModelElement"/>
		/// or implement <see cref="IElementReference"/> and return a ModelElement.
		/// </summary>
		/// <param name="modifyDescriptor">A standard property descriptor to wrap.</param>
		/// <param name="displayName">A customized display name. If this is <see langword="null"/>, then the original display name is used.</param>
		/// <param name="description">A customized description. If this is <see langword="null"/>, then the original description is used.</param>
		/// <param name="category">A customized category. If this is <see langword="null"/>, then the original category is used.</param>
		public StoreEnabledPropertyDescriptor(PropertyDescriptor modifyDescriptor, string displayName, string description, string category)
			: base(modifyDescriptor.Name, EditorUtility.GetAttributeArray(modifyDescriptor.Attributes))
		{
			myInner = modifyDescriptor;
			myDisplayName = displayName;
			myDescription = description;
			myCategory = category;
		}
		#endregion // Constructor
		#region Component Resolution
		private object ResolveComponent(object component)
		{
			Type componentType;
			IElementReference redirection;
			if (null != component &&
				null != (componentType = myInner.ComponentType) &&
				!componentType.IsAssignableFrom(component.GetType()) &&
				null != (redirection = component as IElementReference))
			{
				return redirection.ReferencedElement;
			}
			return component;
		}
		#endregion // Component Resolution
		#region Other overrides
		/// <summary>
		/// Return the custom category if provided in the constructor, or
		/// the default category from the wrapped descriptor.
		/// </summary>
		public override string Category
		{
			get
			{
				return myCategory ?? myInner.Category;
			}
		}
		/// <summary>
		/// Return the custom display name if provided in the constructor, or
		/// the default display name from the wrapped descriptor.
		/// </summary>
		public override string DisplayName
		{
			get
			{
				return myDisplayName ?? myInner.DisplayName;
			}
		}
		/// <summary>
		/// Return the custom description if provided in the constructor, or
		/// the default description from the wrapped descriptor.
		/// </summary>
		public override string Description
		{
			get
			{
				return myDescription ?? myInner.Description;
			}
		}
		/// <summary>
		/// Standard override. Defer to the wrapped descriptor.
		/// </summary>
		public override bool CanResetValue(object component)
		{
			return myInner.CanResetValue(ResolveComponent(component));
		}
		/// <summary>
		/// Standard override. Defer to the wrapped descriptor.
		/// </summary>
		public override Type ComponentType
		{
			get { return myInner.ComponentType; }
		}
		/// <summary>
		/// Standard override. Defer to the wrapped descriptor.
		/// </summary>
		public override object GetValue(object component)
		{
			return myInner.GetValue(ResolveComponent(component));
		}
		/// <summary>
		/// Standard override. Defer to the wrapped descriptor.
		/// </summary>
		public override bool IsReadOnly
		{
			get { return myInner.IsReadOnly; }
		}
		/// <summary>
		/// Standard override. Defer to the wrapped descriptor.
		/// </summary>
		public override Type PropertyType
		{
			get { return myInner.PropertyType; }
		}
		/// <summary>
		/// Standard override. Defer to the wrapped descriptor
		/// inside a transaction.
		/// </summary>
		public override void ResetValue(object component)
		{
			object element;
			IElementReference elementRef;
			ModelElement mel;
			Store store;
			if (null == (element = ResolveComponent(component)) ||
				(null == (mel = element as ModelElement) &&
				(null == (elementRef = element as IElementReference) ||
				null == (mel = elementRef.ReferencedElement as ModelElement))) ||
				null == (store = Utility.ValidateStore(mel.Store)))
			{
				return;
			}
			using (Transaction t = store.TransactionManager.BeginTransaction(ElementPropertyDescriptor.GetSetValueTransactionName(myInner.DisplayName)))
			{
				myInner.ResetValue(element);
				if (t.HasPendingChanges)
				{
					t.Commit();
				}
			}
		}
		/// <summary>
		/// Standard override. Defer to the wrapped descriptor
		/// inside a transaction.
		/// </summary>
		public override void SetValue(object component, object value)
		{
			object element;
			IElementReference elementRef;
			ModelElement mel;
			Store store;
			if (null == (element = ResolveComponent(component)) ||
				(null == (mel = element as ModelElement) &&
				(null == (elementRef = element as IElementReference) ||
				null == (mel = elementRef.ReferencedElement as ModelElement))) ||
				null == (store = Utility.ValidateStore(mel.Store)))
			{
				return;
			}
			using (Transaction t = store.TransactionManager.BeginTransaction(ElementPropertyDescriptor.GetSetValueTransactionName(myInner.DisplayName)))
			{
				myInner.SetValue(element, value);
				if (t.HasPendingChanges)
				{
					t.Commit();
				}
			}
		}
		/// <summary>
		/// Standard override. Defer to the wrapped descriptor.
		/// </summary>
		public override bool ShouldSerializeValue(object component)
		{
			return myInner.ShouldSerializeValue(ResolveComponent(component));
		}
		/// <summary>
		/// Standard override. Defer to the wrapped descriptor.
		/// </summary>
		public override void AddValueChanged(object component, EventHandler handler)
		{
			myInner.AddValueChanged(ResolveComponent(component), handler);
		}
		/// <summary>
		/// Standard override. Defer to the wrapped descriptor.
		/// </summary>
		public override AttributeCollection Attributes
		{
			get { return myInner.Attributes; }
		}
		/// <summary>
		/// Standard override. Defer to the wrapped descriptor.
		/// </summary>
		public override TypeConverter Converter
		{
			get { return myInner.Converter; }
		}
		/// <summary>
		/// Standard override. Defer to the wrapped descriptor.
		/// </summary>
		public override bool DesignTimeOnly
		{
			get { return myInner.DesignTimeOnly; }
		}
		/// <summary>
		/// Standard override. Defer to the wrapped descriptor.
		/// </summary>
		public override PropertyDescriptorCollection GetChildProperties(object instance, Attribute[] filter)
		{
			return myInner.GetChildProperties(instance, filter);
		}
		/// <summary>
		/// Standard override. Defer to the wrapped descriptor.
		/// </summary>
		public override object GetEditor(Type editorBaseType)
		{
			return myInner.GetEditor(editorBaseType);
		}
		/// <summary>
		/// Standard override. Defer to the wrapped descriptor.
		/// </summary>
		public override bool IsBrowsable
		{
			get { return myInner.IsBrowsable; }
		}
		/// <summary>
		/// Standard override. Defer to the wrapped descriptor.
		/// </summary>
		public override bool IsLocalizable
		{
			get { return myInner.IsLocalizable; }
		}
		/// <summary>
		/// Standard override. Defer to the wrapped descriptor.
		/// </summary>
		public override void RemoveValueChanged(object component, EventHandler handler)
		{
			myInner.RemoveValueChanged(ResolveComponent(component), handler);
		}
		/// <summary>
		/// Standard override. Defer to the wrapped descriptor.
		/// </summary>
		public override bool SupportsChangeEvents
		{
			get { return myInner.SupportsChangeEvents; }
		}
		/// <summary>
		/// Standard override. Defer to the wrapped descriptor.
		/// </summary>
		public override string ToString()
		{
			return myInner.ToString();
		}
		#endregion // Other overrides
	}
	#endregion // StoreEnabledPropertyDescriptor class
	#region StoreEnabledReadOnlyPropertyDescriptor class
	/// <summary>
	/// A read-only version of <see cref="StoreEnabledPropertyDescriptor"/>
	/// </summary>
	public class StoreEnabledReadOnlyPropertyDescriptor : StoreEnabledPropertyDescriptor
	{
		#region Constructor
		/// <summary>
		/// Create a wrapped read-only descriptor that automatically resolves and forwards
		/// <see cref="ModelElement"/> components to the provided descriptor.
		/// </summary>
		/// <param name="modifyDescriptor">A standard property descriptor to wrap.</param>
		/// <param name="displayName">A customized display name. If this is <see langword="null"/>, then the original display name is used.</param>
		/// <param name="description">A customized description. If this is <see langword="null"/>, then the original description is used.</param>
		/// <param name="category">A customized category. If this is <see langword="null"/>, then the original category is used.</param>
		public StoreEnabledReadOnlyPropertyDescriptor(PropertyDescriptor modifyDescriptor, string displayName, string description, string category)
			: base(modifyDescriptor, displayName, description, category)
		{
		}
		#endregion // Constructor
		#region Base overrides
		/// <summary>
		/// Force a read-only state regardless of settings on the modified descriptor
		/// </summary>
		public override bool IsReadOnly
		{
			get
			{
				return true;
			}
		}
		#endregion // Base overrides
	}
	#endregion // StoreEnabledReadOnlyPropertyDescriptor class
}
