#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
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
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Shell;

namespace ORMSolutions.ORMArchitect.Core.ShapeModel
{
	#region ModelElementDragAction class
	/// <summary>
	/// Put any model element into a drag state that can be dropped on a diagram
	/// </summary>
	public class ModelElementDragAction : SelectAction
	{
		#region UnsafeNativeMethods
		[System.Security.SuppressUnmanagedCodeSecurity]
		private static partial class UnsafeNativeMethods
		{
			[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
			private static extern IntPtr LoadLibrary(string dllToLoad);

			[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
			private static extern IntPtr LoadCursor([In] IntPtr hInstance, UInt16 lpCursorName);
			public enum DragCursorType
			{
				Move,
				Copy,
				Link
			}
			private static Cursor myMoveCursor = null;
			private static Cursor myCopyCursor = null;
			private static Cursor myLinkCursor = null;
			/// <summary>
			/// Get a cursor for a standard drag operation without entering drag mode
			/// </summary>
			public static Cursor GetDragCursor(DragCursorType cursorType)
			{
				switch (cursorType)
				{
					case DragCursorType.Move:
						return GetDragCursor(ref myMoveCursor, 2);
					case DragCursorType.Copy:
						return GetDragCursor(ref myCopyCursor, 3);
					case DragCursorType.Link:
						return GetDragCursor(ref myLinkCursor, 4);
				}
				return null;
			}
			private static Cursor GetDragCursor(ref Cursor cachedCursor, UInt16 imageNumber)
			{
				Cursor cursor = cachedCursor;
				if (cursor == null)
				{
					IntPtr libPtr = LoadLibrary("ole32.dll");
					IntPtr cursorHandle = LoadCursor(libPtr, imageNumber);
					cachedCursor = cursor = new Cursor(cursorHandle);
				}
				return cursor;
			}
		}
		#endregion // UnsafeNativeMethods
		#region Member variables
		private ModelElement[] myModelElements;
		private Cursor myCursor;
		private int myNextElement;
		#endregion // Member variables
		#region Constructors
		/// <summary>
		/// Create a click action to add one or more shapes to a diagram.
		/// </summary>
		/// <param name="diagram">The hosting diagram</param>
		/// <param name="element">The model element to drop</param>
		public ModelElementDragAction(Diagram diagram, ModelElement element) : base(diagram)
		{
			Reset();
			myModelElements = element == null ? Array.Empty<ModelElement>() : new ModelElement[] { element };
		}
		/// <summary>
		/// Create a click action to add one or more shapes to a diagram.
		/// </summary>
		/// <param name="diagram">The hosting diagram</param>
		/// <param name="elements">The model elements to click-add to the diagram</param>
		public ModelElementDragAction(Diagram diagram, IEnumerable<ModelElement> elements) : base(diagram)
		{
			Reset();
			myModelElements = elements == null ? Array.Empty<ModelElement>() :elements.ToArray();
		}
		#endregion // Constructors
		#region Base overrides
		/// <summary>
		/// Show that a drop is allowed on any open diagram space
		/// </summary>
		protected override void OnMouseMove(DiagramMouseEventArgs e)
		{
			if (e.HitDiagramItem.IsDiagram)
			{
				ActivateCopyCursor();
			}
			else
			{
				myCursor = Cursors.No;
			}
		}
		/// <summary>
		/// Display the cursor selected in a mouse move
		/// </summary>
		public override Cursor GetCursor(Cursor currentCursor, DiagramClientView diagramClientView, PointD mousePosition)
		{
			return myCursor;
		}
		/// <summary>
		/// Reset the member variables
		/// </summary>
		protected override void OnMouseActionDeactivated(DiagramEventArgs e)
		{
			base.OnMouseActionDeactivated(e);
			Reset();
		}
		/// <summary>
		/// Activate the copy cursor immediately on activate.
		/// </summary>
		/// <remarks>The no-drag cursor immediately after activation and before a mouse move seems very abrupt.
		/// By default, assume the drop point is over the diagram (copy cursor) instead of a shape. An actual
		/// mouse move is needed to get the exact setting, but 'active' is a better default for the majority of
		/// the surface area of a diagram.</remarks>
		protected override void OnMouseActionActivated(DiagramEventArgs e)
		{
			ActivateCopyCursor();
		}
		#endregion // Base overrides
		#region ModelElementDragAction specific
		private void ActivateCopyCursor()
		{
			myCursor = UnsafeNativeMethods.GetDragCursor(UnsafeNativeMethods.DragCursorType.Copy);
		}
		/// <summary>
		/// Check remaining elements. Recommendation is to to activate if there are no remaining elements
		/// </summary>
		public bool HasRemainingElements
		{
			get
			{
				return myNextElement < (myModelElements?.Length ?? 0);
			}
		}
		private void Reset()
		{
			myModelElements = null;
			myNextElement = 0;
			myCursor = Cursors.No;
		}
		/// <summary>
		/// Drop the data object.
		/// </summary>
		protected override void OnClicked(MouseActionEventArgs e)
		{
			DiagramClientView clientView;
			if (null != (clientView = e.DiagramClientView))
			{
				ModelElement[] elements;
				bool deactivate = true;
				if (null != (elements = myModelElements))
				{
					int elementCount = elements.Length;
					int nextIndex = myNextElement;
					while (nextIndex < elementCount)
					{
						ModelElement element = elements[nextIndex];
						deactivate = ++nextIndex >= elementCount;
						myNextElement = nextIndex;
						if (!element.IsDeleted)
						{
							DataObject dataObject = new DataObject();
							dataObject.SetData(element.GetType(), element);
							Point p = clientView.PointToScreen(clientView.WorldToDevice(e.CurrentMousePosition));
							clientView.OnDragDropCommon(new DragEventArgs(dataObject, 0, p.X, p.Y, DragDropEffects.Move | DragDropEffects.Copy | DragDropEffects.Scroll, DragDropEffects.None), true, base.MouseDownHitShape);
							break;
						}
					}
				}

				if (deactivate && IsActive)
				{
					Cancel(clientView);
				}
			}
		}
		#endregion // ModelElementDragAction specific
	}
	#endregion // ModelElementDragAction class
}
