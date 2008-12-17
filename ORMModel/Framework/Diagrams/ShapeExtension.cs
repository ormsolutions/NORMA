#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
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
using System.Diagnostics;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using System.ComponentModel;
using Microsoft.VisualStudio.OLE.Interop;

namespace Neumont.Tools.Modeling.Diagrams
{
	#region IShapeExtender interface
	/// <summary>
	/// Provides a generic interface to use from <see cref="ShapeElement"/> overrides for supporting
	/// add child shapes beyond those specified in the domain model for the main shape.
	/// </summary>
	public interface IShapeExtender<S> where S : ShapeElement
	{
		/// <summary>
		/// Extension callback for the <see cref="M:ShapeElement.ShouldAddShapeForElement"/> method
		/// </summary>
		/// <param name="extendedShape">The shape to extend</param>
		/// <param name="element">The element to add to the diagram</param>
		/// <returns><see langword="true"/> to continue with shape creation for this element</returns>
		bool ShouldAddShapeForElement(S extendedShape, ModelElement element);
		/// <summary>
		/// Extension callback for the <see cref="M:ShapeElement.CreateChildShape"/> method
		/// </summary>
		/// <param name="extendedShape">The shape to extend</param>
		/// <param name="element">The element to create a child shape for</param>
		/// <returns>A new child shape</returns>
		ShapeElement CreateChildShape(S extendedShape, ModelElement element);
		/// <summary>
		/// Get toolbox filter attributes used by this extender
		/// </summary>
		ICollection<ToolboxItemFilterAttribute> GetToolboxFilterAttributes();
		/// <summary>
		/// Get the mouse action for the selected toolbox item, which can
		/// be tested with the <see cref="M:DiagramView.SelectedToolboxItemSupportsFilterString"/>
		/// </summary>
		/// <param name="extendedShape">The shape to extend</param>
		/// <param name="activeView">The active <see cref="DiagramView"/> for the shape</param>
		/// <returns>A <see cref="MouseAction"/>, or null if the selected item does not match.</returns>
		MouseAction GetMouseAction(S extendedShape, DiagramView activeView);
		/// <summary>
		/// Provide drag feedback in a drag over. Called if the <paramref name="extendedShape"/> does not
		/// directly handle the drag over event. If the extender handles this event, then the <see cref="P:DiagramDragEventArgs.Handled"/>
		/// property should be set before returning.
		/// </summary>
		/// <param name="extendedShape">The shape to extend</param>
		/// <param name="e">Forwarded <see cref="DiagramDragEventArgs"/></param>
		void OnDragOver(S extendedShape, DiagramDragEventArgs e);
		/// <summary>
		/// Provide drag feedback in a drag drop event. Called if the <paramref name="extendedShape"/> does not
		/// directly handle the drag drop event. If the extender handles this event, then the <see cref="P:DiagramDragEventArgs.Handled"/>
		/// property should be set before returning.
		/// </summary>
		/// <param name="extendedShape">The shape to extend</param>
		/// <param name="e">Forwarded <see cref="DiagramDragEventArgs"/></param>
		void OnDragDrop(S extendedShape, DiagramDragEventArgs e);
		/// <summary>
		/// If the extended shape implements <see cref="IDisposable"/>, then allow any shape extenders
		/// to explicitly dispose state as well
		/// </summary>
		/// <param name="extendedShape">The shape to extend</param>
		void ExtendedShapeDisposed(S extendedShape);
	}
	#endregion // IShapeExtender interface
	#region IConfigureChildShape interface
	/// <summary>
	/// An interface to implement <see cref="M:ShapeElement.OnChildConfiguring"/>
	/// on the child shape instead of the parent. Implementing child configuration
	/// on the parent shape requires that the parent know the full set of child
	/// shape elements.
	/// </summary>
	public interface IConfigureAsChildShape
	{
		/// <summary>
		/// Called during <see cref="M:ShapeElement.OnChildConfiguring"/> from the parent shape to
		/// configure a shape immediately after it is added as a child of the parent shape or diagram.
		/// </summary>
		/// <param name="parentShape">The parent shape or diagram</param>
		/// <param name="createdDuringViewFixup">Whether this shape was created as part of a view fixup</param>
		void ConfiguringAsChildOf(NodeShape parentShape, bool createdDuringViewFixup);
	}
	#endregion // IConfigureChildShape interface
}
