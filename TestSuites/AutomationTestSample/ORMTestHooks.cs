using System;
using System.Drawing;
using System.ComponentModel;
using System.Diagnostics;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using ORMSolutions.ORMArchitect.Core.Shell;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Modeling.Diagrams;
using System.Runtime.InteropServices;
using ORMSolutions.ORMArchitect.Framework.Design;

namespace ORMRegressionTestAddin
{
	/// <summary>
	/// The location to click on an item. Used with ClickAccessibleObject,
	/// DragAccessibleObject, and DropOnAccessibleObject methods in
	/// the ORMTestWindow structure.
	/// </summary>
	public enum ClickLocation
	{
		/// <summary>
		/// Click in the center of the object
		/// </summary>
		Center = 0,
		/// <summary>
		/// Click on the left corner of the object. Generally
		/// used with the xOffset and yOffset parameters.
		/// </summary>
		UpperLeft = 1,
	}
	/// <summary>
	/// A structure representing an ORM window
	/// </summary>
	[CLSCompliant(false)]
	public struct ORMTestWindow
	{
		private DiagramClientView myClientView;
		private ORMTestHooks myHooks;
		/// <summary>
		/// Represents no window
		/// </summary>
		public static readonly ORMTestWindow Empty = new ORMTestWindow();
		private ORMTestWindow(DiagramClientView clientView, ORMTestHooks ormHooks)
		{
			myClientView = clientView;
			myHooks = ormHooks;
		}
		/// <summary>
		/// Get the window's starting accessible object. The
		/// diagram accessible object will be an immediate child
		/// of this accessible object.
		/// </summary>
		public AccessibleObject AccessibleObject
		{
			get
			{
				DiagramClientView view = myClientView;
				return (view != null) ? view.AccessibilityObject : null;
			}
		}
		/// <summary>
		/// True if the structure has not been initialized
		/// </summary>
		public bool IsEmpty
		{
			get
			{
				return myClientView == null;
			}
		}
		/// <summary>
		/// Get the ORMTestWindow structure for the given ORM file
		/// </summary>
		/// <param name="ormHooks">An ORMTestHooks instance</param>
		/// <param name="fullName">The full name of the file. Retrieve from a Document.FullName property.
		/// If fullName is null or empty then the path of the active document is used.</param>
		/// <returns>ORMTestWindow structure. May be empty.</returns>
		public static ORMTestWindow FindORMTestWindow(ORMTestHooks ormHooks, string fullName)
		{
			if (fullName == null || fullName.Length == 0)
			{
				fullName = ormHooks.DTE.ActiveDocument.FullName;
			}
			RunningDocumentTable docTable = new RunningDocumentTable(ormHooks.ServiceProvider);
			ORMDesignerDocData document = docTable.FindDocument(fullName) as ORMDesignerDocData;
			if (document != null)
			{
				ORMDesignerDocView docView = (ORMDesignerDocView)document.DocViews[0];
				return new ORMTestWindow(docView.CurrentDesigner.DiagramClientView, ormHooks);
			}
			return Empty;
		}
		/// <summary>
		/// Helper function for TranslateAccessibleObject and EnsureAccessibleObjectVisible
		/// </summary>
		private DiagramItem TranslateAccessibleObjectToDiagramItem(AccessibleObject accessibleObject, bool returnShape)
		{
			if (accessibleObject == null)
			{
				return null;
			}
			DiagramItem hitItem = null;
			DiagramClientView clientView = myClientView;
			DiagramHitTestInfo hitInfo = new DiagramHitTestInfo(clientView);
			RectangleD boundsD = clientView.DeviceToWorld(clientView.RectangleToClient(accessibleObject.Bounds));
			if (clientView.Diagram.DoHitTest(boundsD.Center, hitInfo, false))
			{
				hitItem = hitInfo.HitDiagramItem;
				if (!returnShape)
				{
					// Wind back out the parent stack if the hit test went too far
					if (hitItem.SubField != null)
					{
						if (!(accessibleObject is SubfieldAccessibleObject))
						{
							if (!(accessibleObject is FieldAccessibleObject))
							{
								hitItem = new DiagramItem(hitItem.Shape);
							}
							else
							{
								hitItem = new DiagramItem(hitItem.Shape, hitItem.Field);
							}
						}
					}
					else if (hitItem.Field != null && !(accessibleObject is FieldAccessibleObject))
					{
						hitItem = new DiagramItem(hitItem.Shape);
					}
				}
			}
			return hitItem;
		}
		/// <summary>
		/// Turn an accessibleObject into an element in the underlying model.
		/// </summary>
		/// <param name="accessibleObject">The accessible object</param>
		/// <param name="returnShape">True to return the shape object corresponding to the
		/// accessible object instead of the underlying model element. Note that you may get
		/// a shape for a parent object if the requested accessible object is drawn as part
		/// of another object and does not have its own shape (roles, internal uniqueness constraints, etc)</param>
		/// <returns>ModelElement, or null</returns>
		public ModelElement TranslateAccessibleObject(AccessibleObject accessibleObject, bool returnShape)
		{
			DiagramItem hitItem = TranslateAccessibleObjectToDiagramItem(accessibleObject, returnShape);
			if (hitItem == null)
			{
				return null;
			}
			else if (returnShape)
			{
				return hitItem.Shape;
			}
			else
			{
				ModelElement retVal = null;
				foreach (object element in hitItem.RepresentedElements)
				{
					retVal = element as ModelElement;
					break;
				}
				PresentationElement pel = retVal as PresentationElement;
				if (pel != null)
				{
					// Resolve to a ModelElement if we don't have one already
					retVal = pel.ModelElement;
				}
				return retVal;
			}
		}
		[Flags]
		private enum MouseEventFlags
		{
			Move = 1,
			LeftDown = 2,
			LeftUp = 4,
			RightDown = 8,
			RightUp = 0x10,
			MiddleDown = 0x20,
			MiddleUp = 0x40,
			XDown = 0x80,
			XUp = 0x100,
			Wheel = 0x800,
			VirtualDesk = 0x4000,
			Absolute = 0x8000,
		}
		[DllImport("user32.dll", CharSet = CharSet.Ansi)]
		private static extern void mouse_event(MouseEventFlags dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
		[Flags]
		private enum KeyboardEventFlags
		{
			None = 0,
			ExtendedKey = 1,
			KeyUp = 2,
			Unicode = 4,
			ScanCode = 8,
		}
		[DllImport("user32.dll", CharSet = CharSet.Ansi)]
		private static extern void keybd_event(byte bVk, byte bScan, KeyboardEventFlags dwFlags, IntPtr dwExtraInfo);
		/// <summary>
		/// Click on an accessible object. The caller should make sure that the
		/// accessible object is visible before making this call.
		/// </summary>
		/// <param name="accessibleObject">A child accessible object to click on</param>
		public void ClickAccessibleObject(AccessibleObject accessibleObject)
		{
			ClickAccessibleObject(accessibleObject, 1, ClickLocation.Center, 0, 0);
		}
		/// <summary>
		/// Click on an accessible object. The caller should make sure that the
		/// accessible object is visible before making this call.
		/// </summary>
		/// <param name="accessibleObject">A child accessible object to click on</param>
		/// <param name="clicks">The number of clicks (1 for single click, 2 for double)</param>
		public void ClickAccessibleObject(AccessibleObject accessibleObject, int clicks)
		{
			ClickAccessibleObject(accessibleObject, clicks, ClickLocation.Center, 0, 0);
		}
		/// <summary>
		/// Click on an accessible object. The caller should make sure that the
		/// accessible object is visible before making this call.
		/// </summary>
		/// <param name="accessibleObject">A child accessible object to click on</param>
		/// <param name="clicks">The number of clicks (1 for single click, 2 for double)</param>
		/// <param name="location">The location in the accessible object to click on</param>
		/// <param name="xOffset">A horizontal offset (in pixels) from the specified click location.</param>
		/// <param name="yOffset">A vertical offset (in pixels) from the specified click location.</param>
		public void ClickAccessibleObject(AccessibleObject accessibleObject, int clicks, ClickLocation location, int xOffset, int yOffset)
		{
			Rectangle rect = accessibleObject.Bounds;
			PointF pt;
			switch (location)
			{
				case ClickLocation.UpperLeft:
					pt = new PointF((float)rect.Left + xOffset, (float)rect.Top + yOffset);
					break;
				case ClickLocation.Center:
				default:
					pt = new PointF(((float)rect.Left + rect.Right) / 2 + xOffset, ((float)rect.Top + rect.Bottom) / 2 + yOffset);
					break;
			}
			Rectangle screenBounds = Screen.GetBounds(myClientView);
			MoveMouseClose(screenBounds, pt);
			int dx = (int)(pt.X / screenBounds.Width * 65535f);
			int dy = (int)(pt.Y / screenBounds.Height * 65535f);
			SendKeys.Flush();
			mouse_event(MouseEventFlags.Move | MouseEventFlags.Absolute, dx, dy, 0, 0);
			SendKeys.Flush();
			while (clicks > 0)
			{
				mouse_event(MouseEventFlags.LeftDown | MouseEventFlags.Absolute, dx, dy, 0, 0);
				SendKeys.Flush();
				mouse_event(MouseEventFlags.LeftUp | MouseEventFlags.Absolute, dx, dy, 0, 0);
				SendKeys.Flush();
				--clicks;
			}
			SendKeys.Flush();
		}
		/// <summary>
		/// Start a drag operation on an accessible object. The caller should make sure that the
		/// accessible object is visible before making this call.
		/// </summary>
		/// <param name="accessibleObject">A child accessible object to drag</param>
		public void DragAccessibleObject(AccessibleObject accessibleObject)
		{
			DragAccessibleObject(accessibleObject, ClickLocation.Center, 0, 0);
		}
		/// <summary>
		/// Start a drag operation on an accessible object. The caller should make sure that the
		/// accessible object is visible before making this call.
		/// </summary>
		/// <param name="accessibleObject">A child accessible object to drag</param>
		/// <param name="location">The location in the accessible object to drag from</param>
		/// <param name="xOffset">A horizontal offset (in pixels) from the specified drag location.</param>
		/// <param name="yOffset">A vertical offset (in pixels) from the specified drag location.</param>
		public void DragAccessibleObject(AccessibleObject accessibleObject, ClickLocation location, int xOffset, int yOffset)
		{
			Rectangle rect = accessibleObject.Bounds;
			PointF pt;
			switch (location)
			{
				case ClickLocation.UpperLeft:
					pt = new PointF((float)rect.Left + xOffset, (float)rect.Top + yOffset);
					break;
				case ClickLocation.Center:
				default:
					pt = new PointF(((float)rect.Left + rect.Right) / 2 + xOffset, ((float)rect.Top + rect.Bottom) / 2 + yOffset);
					break;
			}
			Rectangle screenBounds = Screen.GetBounds(myClientView);
			MoveMouseClose(screenBounds, pt);
			int dx = (int)(pt.X / screenBounds.Width * 65535f);
			int dy = (int)(pt.Y / screenBounds.Height * 65535f);
			Size dragSize = SystemInformation.DragSize;
			float dxDrag = (dragSize.Width / screenBounds.Width * 65535f);
			float dyDrag = (dragSize.Height / screenBounds.Height * 65535f);
			SendKeys.Flush();
			mouse_event(MouseEventFlags.Move | MouseEventFlags.Absolute, dx, dy, 0, 0);
			SendKeys.Flush();
			mouse_event(MouseEventFlags.LeftDown | MouseEventFlags.Absolute, dx, dy, 0, 0);
			// Move .5 the drag size, the full drag size, and 1.5 the drag size to make sure we got it
			mouse_event(MouseEventFlags.Move | MouseEventFlags.Absolute, dx + (int)(dxDrag * .5), dy + (int)(dyDrag * .5), 0, 0);
			SendKeys.Flush();
			mouse_event(MouseEventFlags.Move | MouseEventFlags.Absolute, dx + (int)dxDrag, dy + (int)dyDrag, 0, 0);
			SendKeys.Flush();
			mouse_event(MouseEventFlags.Move | MouseEventFlags.Absolute, dx + (int)(dxDrag * 1.5), dy + (int)(dyDrag * 1.5), 0, 0);
			SendKeys.Flush();
		}
		/// <summary>
		/// End a drag operation with a drop on an accessible object. The caller should make sure that the
		/// accessible object is visible before making this call.
		/// </summary>
		/// <param name="accessibleObject">A child accessible object to drop on</param>
		public void DropOnAccessibleObject(AccessibleObject accessibleObject)
		{
			DropOnAccessibleObject(accessibleObject, ClickLocation.Center, 0, 0);
		}
		/// <summary>
		/// End a drag operation with a drop on an accessible object. The caller should make sure that the
		/// accessible object is visible before making this call.
		/// </summary>
		/// <param name="accessibleObject">A child accessible object to drop on</param>
		/// <param name="location">The location in the accessible object to drop on</param>
		/// <param name="xOffset">A horizontal offset (in pixels) from the specified drop location.</param>
		/// <param name="yOffset">A vertical offset (in pixels) from the specified drop location.</param>
		public void DropOnAccessibleObject(AccessibleObject accessibleObject, ClickLocation location, int xOffset, int yOffset)
		{
			Rectangle rect = accessibleObject.Bounds;
			PointF pt;
			switch (location)
			{
				case ClickLocation.UpperLeft:
					pt = new PointF((float)rect.Left + xOffset, (float)rect.Top + yOffset);
					break;
				case ClickLocation.Center:
				default:
					pt = new PointF(((float)rect.Left + rect.Right) / 2 + xOffset, ((float)rect.Top + rect.Bottom) / 2 + yOffset);
					break;
			}
			Rectangle screenBounds = Screen.GetBounds(myClientView);
			MoveMouseClose(screenBounds, pt);
			int dx = (int)(pt.X / screenBounds.Width * 65535f);
			int dy = (int)(pt.Y / screenBounds.Height * 65535f);
			SendKeys.Flush();
			mouse_event(MouseEventFlags.Move | MouseEventFlags.Absolute, dx, dy, 0, 0);
			SendKeys.Flush();
			mouse_event(MouseEventFlags.LeftUp | MouseEventFlags.Absolute, dx, dy, 0, 0);
			SendKeys.Flush();
		}
		/// <summary>
		/// Move the mouse to get close to the target position. A final move at the target position is left to the caller.
		/// </summary>
		/// <param name="screenBounds">The bounds of the screen to work in</param>
		/// <param name="targetPoint">The destination point to target</param>
		private static void MoveMouseClose(Rectangle screenBounds, PointF targetPoint)
		{
			MoveMouseClose(screenBounds, targetPoint, 5);
		}
		/// <summary>
		/// Move the mouse to get close to the target position. A final move at the target position is left to the caller.
		/// </summary>
		/// <param name="screenBounds">The bounds of the screen to work in</param>
		/// <param name="targetPoint">The destination point to target</param>
		/// <param name="pixelGranularity">The distance in pixels for a single incremental move</param>
		private static void MoveMouseClose(Rectangle screenBounds, PointF targetPoint, int pixelGranularity)
		{
			PointF startPt = Control.MousePosition;
			PointF difPt = new PointF(targetPoint.X - startPt.X, targetPoint.Y - startPt.Y);
			double distance = Math.Sqrt(difPt.X * difPt.X + difPt.Y * difPt.Y);
			int steps = (int)Math.Abs(distance / pixelGranularity);
			SizeF step = new SizeF((float)difPt.X / steps, (float)difPt.Y / steps);
			while (steps > 0)
			{
				startPt += step;
				SendKeys.Flush();
				mouse_event(MouseEventFlags.Move | MouseEventFlags.Absolute, (int)(startPt.X / screenBounds.Width * 65535f), (int)(startPt.Y / screenBounds.Height * 65535f), 0, 0);
				--steps;
			}
		}
		/// <summary>
		/// Press the specified key. Inserts a KeyDown in the message stream
		/// without a KeyUp. Note that most keyboard emulation can be done with
		/// the static methods on the System.Windows.Forms.SendKeys class. However,
		/// if you need to press and hold a key during another operation, then
		/// you need to use this function explicitly.
		/// </summary>
		/// <param name="key">A Keys value. Generally ControlKey, ShiftKey, or Menu.</param>
		public void KeyDown(Keys key)
		{
			keybd_event((byte)key, 0, KeyboardEventFlags.None, IntPtr.Zero);
		}
		/// <summary>
		/// Press the specified key. Inserts a KeyUp in the message stream. This
		/// call generally follows a call to KeyDown.
		/// </summary>
		/// <param name="key">A Keys value. Generally ControlKey, ShiftKey, or Menu.</param>
		public void KeyUp(Keys key)
		{
			keybd_event((byte)key, 0, KeyboardEventFlags.KeyUp, IntPtr.Zero);
		}
		/// <summary>
		/// Make sure an accessible object is visible. Returns a new AccessibleObject with
		/// different bounds. Using this method with an "ORMDiagram" accessible object is not
		/// recommended (it will zoom out the diagram until the full diagram fits in the window).
		/// Use the EnsurePointVisible method instead to show a specific point on the diagram.
		/// </summary>
		/// <param name="accessibleObject">Accessible object to bring into view</param>
		/// <returns>Updated accessible object</returns>
		public AccessibleObject EnsureAccessibleObjectVisible(AccessibleObject accessibleObject)
		{
			AccessibleObject retVal = accessibleObject;
			DiagramItem hitItem = TranslateAccessibleObjectToDiagramItem(accessibleObject, false);
			if (hitItem != null)
			{
				myClientView.EnsureVisible(hitItem.AbsoluteBoundingBox);
				retVal = hitItem.GetAccessibleObject(myClientView);
				SendKeys.Flush();
			}
			return retVal;
		}
		/// <summary>
		/// Ensure a point on the diagram is visible.
		/// </summary>
		/// <param name="point">Relative point in pixels</param>
		public void EnsurePointVisible(Point point)
		{
			PointD zeroPoint = myClientView.DeviceToWorld(new Point(0, 0));
			PointD worldPoint = myClientView.DeviceToWorld(point);
			myClientView.EnsureVisible(new RectangleD(new PointD(worldPoint.X - zeroPoint.X, worldPoint.Y - zeroPoint.Y), new SizeD(1d, 1d)));
			SendKeys.Flush();
		}
		/// <summary>
		/// Activate an item on the ORM Designer toolbox
		/// </summary>
		/// <param name="itemName">The name of the item ('Binary Fact', 'Role Connector', etc)</param>
		public void ActivateToolboxItem(string itemName)
		{
			CommonTestHooks.ActivateToolboxItem(myHooks.DTE, "ORM Designer", itemName);
			myClientView.Diagram.OnViewMouseEnter(new DiagramPointEventArgs(0, 0, PointRelativeTo.Client, myClientView));
			SendKeys.Flush();
		}
		/// <summary>
		/// Get an array of selected accessible objects
		/// </summary>
		public AccessibleObject[] GetSelectedAccessibleObjects()
		{
			DiagramClientView view = myClientView;
			SelectedShapesCollection shapes = view.Selection;
			int selectionCount = shapes.Count;
			AccessibleObject[] retVal = new AccessibleObject[shapes.Count];
			int i = 0;
			foreach (DiagramItem item in shapes)
			{
				retVal[i] = item.GetAccessibleObject(view);
				++i;
			}
			return retVal;
		}
	}
	/// <summary>
	/// Class specific to testing ORM
	/// </summary>
	[CLSCompliant(false)]
	public class ORMTestHooks
	{
		private IServiceProvider myServiceProvider;
		private DTE2 myDTE;
		/// <summary>
		/// Create a new ORMTestHooks class
		/// </summary>
		/// <param name="applicationObject">The application object passed to the addion</param>
		public ORMTestHooks(_DTE applicationObject)
		{
			myDTE = (DTE2)applicationObject;
		}
		/// <summary>
		/// Retrieve the current DTE application object
		/// </summary>
		public DTE2 DTE
		{
			get
			{
				return myDTE;
			}
		}
		/// <summary>
		/// Get Visual Studio's managed service provider
		/// </summary>
		public IServiceProvider ServiceProvider
		{
			get
			{
				IServiceProvider retVal = myServiceProvider;
				if (retVal == null)
				{
					myServiceProvider = retVal = CommonTestHooks.CreateServiceProvider(myDTE);
				}
				return retVal;
			}
		}
		/// <summary>
		/// Get the top-level accessible object for the given ORM file
		/// </summary>
		/// <param name="fullName">The full name of the file. Retrieve from a Document.FullName property.
		/// If fullName is null or empty then the path of the active document is used.</param>
		/// <returns>ORMTestWindow structure. May be empty.</returns>
		public ORMTestWindow FindORMTestWindow(string fullName)
		{
			return ORMTestWindow.FindORMTestWindow(this, fullName);
		}
		/// <summary>
		/// Create a property descriptor for the the given meta attribute.
		/// Property descriptors emulate the behavior of modifying this
		/// property in the properties window.
		/// </summary>
		/// <param name="element"></param>
		/// <param name="attributeId">The meta attribute id for the property. This
		/// is available as a static field in the public object model of the element
		/// you're trying to set. For example, to change the name property of a FactType
		/// element you would use FactType.NameMetaAttributeGuid as your attributeId</param>
		/// <returns>PropertyDescriptor, or null if not available</returns>
		public PropertyDescriptor CreatePropertyDescriptor(ModelElement element, Guid attributeId)
		{
			DomainPropertyInfo attrInfo = element.Store.DomainDataDirectory.FindDomainProperty(attributeId);
			return (attrInfo != null) ? DomainTypeDescriptor.CreatePropertyDescriptor(element, attrInfo) : null;
		}
		/// <summary>
		/// Create a property descriptor for the given property on
		/// the element to emulate the behavior of modifying this
		/// property in the properties window. This will succeed if the property
		/// passed in has a corresponding meta attribute.
		/// </summary>
		/// <param name="element">The element to create a descriptor for</param>
		/// <param name="propertyName">The name of the property on the native object.
		/// Note that the name of this property may be displayed differently in the
		/// properties window. For example, Role.RolePlayerDisplay is shown as RolePlayer
		/// in the properties window.</param>
		/// <returns>PropertyDescriptor, or null if not available</returns>
		public PropertyDescriptor CreatePropertyDescriptor(ModelElement element, string propertyName)
		{
			Type elementType = element.GetType();
			PropertyInfo propInfo = elementType.GetProperty(propertyName);
			if (propInfo != null)
			{
				DomainPropertyInfo attrInfo = element.Store.DomainDataDirectory.FindDomainClass(propInfo.DeclaringType).FindDomainProperty(propertyName, true);
				if (attrInfo != null)
				{
					return DomainTypeDescriptor.CreatePropertyDescriptor(element, attrInfo);
				}
			}
			return null;
		}
	}
}