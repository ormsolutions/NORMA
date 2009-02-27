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
using Microsoft.VisualStudio.Shell;
using System.ComponentModel.Design;

namespace Neumont.Tools.Modeling.Design
{
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
	/// <summary>
	/// Static helper functions to use with <see cref="UITypeEditor"/>
	/// implementations.
	/// </summary>
	public static class EditorUtility
	{
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
	}
}
